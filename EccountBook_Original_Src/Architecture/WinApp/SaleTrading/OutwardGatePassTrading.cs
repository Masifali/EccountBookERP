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
using Architecture.BLL.Inventory;
using Architecture.BLL.Reports.Inventory;
using Architecture.BLL.WeightBridge;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Authentication;
using Architecture.Model.Inventory;
using Architecture.Model.WeightBridge;
using Architecture.Reports;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.SaleTrading;

public class OutwardGatePassTrading : Form
{
	private DropDownBind DDL = new DropDownBind();

	private Attachment AT = new Attachment();

	public DataTable dtViewAttachments = new DataTable();

	public DataTable dtAttachmentsUpload = new DataTable();

	private bool UpdateMode;

	private bool GpUpdateWithoutApproval;

	public UserAccount UserAccount = new UserAccount();

	private int Id;

	private int Qty;

	public int SOId;

	private int GpId;

	private bool SubsidiaryAccountAllownOnVouchers;

	private bool EnableSO;

	private bool EnableDO;

	private bool Approve;

	private DataTable table = new DataTable();

	private DataTable tablegrd = new DataTable();

	private DataTable lstSupplier = new DataTable();

	private DataTable dtItem = new DataTable();

	private DataTable dtOrderType = new DataTable();

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

	private Label label26;

	private TextBox txtFactoryWeight;

	private TextBox txtWeighBridgeSlipNo;

	private TextBox CmbOrderno;

	internal UltraCombo cmbsupp;

	private ToolStripButton btnRefresh;

	private CheckBox ChkBox;

	internal UltraCombo cmbvehicletype;

	internal UltraCombo cmbgptype;

	internal UltraCombo cmbcity;

	private ToolStrip toolStrip2;

	private ToolStripButton BtnNewHistory;

	internal UltraCombo CmbStatus;

	private ToolStripButton btnDelete;

	internal UltraCombo CmbGPBasedOn;

	private Label label18;

	private Label label19;

	private Label label27;

	private ToolStripButton btnPrint295;

	private Label lblApprovedBy;

	private TextBox txtApprovedBy;

	internal UltraCombo CmbWeighBridgeStatus;

	private Label label35;

	private Label label32;

	private Label label31;

	private ToolStripButton btndefvehicle;

	private Label label6;

	private TabControl tabControl2;

	private TabPage tabPage3;

	private TabPage tabDoInformation;

	private TabPage tabSoInformation;

	private TextBox txtgpno;

	private TextBox txtgptypeno;

	private Label label1;

	private Label label3;

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

	private Label label7;

	private Label label5;

	private Label label17;

	private TextBox txtremarks;

	internal GridEX grdhistory;

	private Panel panel8;

	private Panel panel5;

	private Label label21;

	private GroupBox groupBox4;

	private Button btnshow;

	private Label label73;

	internal UltraCombo CmbCustomer;

	private DateTimePicker txtToDateHistory;

	private Label label24;

	private Label label76;

	private TextBox txtToDocNoHistory;

	private DateTimePicker txtFromdateHistory;

	private Label label25;

	private Label label77;

	private TextBox txtFromNoHistory;

	private ToolStripSeparator toolStripSeparator3;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripSeparator toolStripSeparator4;

	private ToolStripSeparator toolStripSeparator5;

	private ToolStripSeparator toolStripSeparator6;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripButton btnShortCut;

	private ToolStripButton btnRefreshHistory;

	private RadioButton rdmodifydate;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	public OutwardGatePassTrading(UserAccount pUserAccount)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUserAccount;
	}

	private void OutwardGatePass_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1000, 625);
			formright = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnsave.Enabled = formright.DoHaveSaveRight;
			btnPrint.Enabled = formright.DoHavePrintRights;
			btnupdate.Enabled = formright.DoHaveUpdateRights;
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
			dtViewAttachments.Columns.Clear();
			dtViewAttachments.Columns.Add("AttchmentPath");
			SupplierFill();
			Status();
			WeighBridgeStatus();
			GpBasedOn();
			gpnofill();
			gatepasstype();
			vehicleTypefill();
			CityFill();
			grdfrmfill();
			CustomerHistoryFill();
			btnupdate.Visible = false;
			txtintime.Value = DateTime.Now;
			txtouttime.Value = DateTime.Now;
			btnsave.Visible = true;
			btnupdate.Visible = false;
			((UltraGridBase)CmbStatus).Rows[0].Activate();
			CmbGPBasedOn.Focus();
			ChkBox.Checked = true;
			cmbgptype_Leave_1(null, null);
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
			tabControl2.TabPages.Remove(tabSoInformation);
			tabControl2.TabPages.Remove(tabDoInformation);
			List<ConfigrationsAllocation> ConfiSo = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "EnableSaleOrderFlow").ToList();
			if (ConfiSo.Count > 0)
			{
				EnableSO = Conversion.ToBool((object)ConfiSo[0].ConfigKey.ToString());
			}
			List<ConfigrationsAllocation> ConfiDo = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "EnableDeliveryOrderFlow").ToList();
			if (ConfiDo.Count > 0)
			{
				EnableDO = Conversion.ToBool((object)ConfiDo[0].ConfigKey.ToString());
			}
			if (EnableSO)
			{
				tabControl2.TabPages.Add(tabSoInformation);
				SaleOrderGridFill();
			}
			if (EnableDO)
			{
				tabControl2.TabPages.Add(tabDoInformation);
				DeliveryOrderGridFill();
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
		try
		{
			dtOrderType = GatePassOutward.GetOrderTypeForOutwardGP_Engr(UserAccount.OrganizationId, UserAccount.CompanyId);
			if (dtOrderType.Rows.Count > 0)
			{
				DDL.BindDDL(dtOrderType, CmbGPBasedOn, "Id", "OrderType", "OrderType", ZeroIndex: false);
				((UltraGridBase)CmbGPBasedOn).Rows[0].Activate();
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
			DataTable dt = CommonServices.GatePassTypeFill("Outward");
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbgptype, "Id", "GpTypeDescription", "Gate Pass Type", ZeroIndex: false);
				((UltraGridBase)cmbgptype).Rows[1].Activate();
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
				return;
			}
			MessageBox.Show("Please GatePass Type Select");
			cmbgptype.Focus();
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
			DataTable dt = VehicleType.GetAll();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbvehicletype, "Id", "VehicleDescription", "Vehicle Description", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CityFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dt = City.GetAll(new City
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbcity, "Id", "CityName", "City Name", false);
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
			if (SubsidiaryAccountAllownOnVouchers)
			{
				lstSupplier = CommonServices.GetVendorsAndCustomers(1);
				DDL.BindDDL(lstSupplier, cmbsupp, "Id", "CompanyName", "Supplier Name", ZeroIndex: false);
				return;
			}
			lstSupplier = CommonServices.SupplierCustomerGetforComboServiceBind();
			if (lstSupplier.Rows.Count > 0)
			{
				DDL.BindDDL(lstSupplier, cmbsupp, "Id", "CompanyName", "Supplier Name", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbOrderno_Leave(object sender, EventArgs e)
	{
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Expected O, but got Unknown
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Expected O, but got Unknown
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		//IL_025f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0270: Unknown result type (might be due to invalid IL or missing references)
		//IL_0281: Unknown result type (might be due to invalid IL or missing references)
		//IL_0288: Unknown result type (might be due to invalid IL or missing references)
		//IL_0299: Expected O, but got Unknown
		try
		{
			if (((Control)(object)CmbGPBasedOn).Text == string.Empty || ((Control)(object)CmbGPBasedOn).Text == null)
			{
				CmbGPBasedOn.Focus();
				MessageBox.Show("Order Type Field Required");
				return;
			}
			if (((Control)(object)cmbgptype).Text == string.Empty || ((Control)(object)cmbgptype).Text == null)
			{
				cmbgptype.Focus();
				MessageBox.Show("GatePass Type Field Required");
				return;
			}
			int DocumentTypeId = Conversion.ToInt(CmbGPBasedOn.Value);
			switch (DocumentTypeId)
			{
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
					if (lst.Rows.Count > 0)
					{
						DDL.BindDDLNew(lst, cmbsupp, "Id", "CompanyName", "Supplier Name", false);
						((UltraGridBase)cmbsupp).Rows[0].Activate();
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
						txtqty.Text = Weight.Rows[0]["DoTotalQty"].ToString();
						string obj = Weight.Rows[0]["DeliveryOrderType"].ToString();
						txtvehicleno.Text = Weight.Rows[0]["VehicleNo"].ToString();
						((Control)(object)cmbvehicletype).Text = Weight.Rows[0]["VehicleType"].ToString();
						if (obj == "Export" && ((Control)(object)cmbgptype).Text != "Export")
						{
							MessageBox.Show("Please Select GatePass Type Export");
							CmbOrderno.Text = string.Empty;
						}
						else
						{
							DDL.BindDDLNew(Weight, cmbsupp, "Id", "CompanyName", "CustomerName", false);
							((UltraGridBase)cmbsupp).Rows[0].Activate();
						}
					}
					else
					{
						SOId = 0;
						Weight.Rows.Clear();
						txtFactoryWeight.Text = string.Empty;
						((Control)(object)cmbsupp).Text = string.Empty;
						((UltraGridBase)cmbsupp).DataSource = null;
					}
				}
				else
				{
					SOId = 0;
					CmbOrderno.Text = string.Empty;
					((Control)(object)cmbsupp).Text = string.Empty;
					((UltraGridBase)cmbsupp).DataSource = null;
				}
				break;
			}
			case 59:
			{
				DataTable dtCustomer = InvPurchaseInvoice.GetPartiesFromPurchaseInvoiceStoreWithGlAccount(UserAccount.OrganizationId, UserAccount.CompanyId);
				DDL.BindDDLNew(dtCustomer, cmbsupp, "Id", "CompanyName", "Customer Name", false);
				break;
			}
			}
		}
		catch (Exception ex)
		{
			((Control)(object)cmbsupp).Text = string.Empty;
			((UltraGridBase)cmbsupp).DataSource = null;
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CmbVariety_Leave(object sender, EventArgs e)
	{
	}

	private void CmbGPBasedOn_Leave(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(CmbGPBasedOn.Value) == 3)
			{
				CmbOrderno.TabStop = false;
				CmbOrderno.Enabled = false;
				SupplierFill();
			}
			else if (((Control)(object)CmbGPBasedOn).Text == "Purchase Return")
			{
				CmbOrderno.Enabled = false;
				CmbOrderno.Text = txtgpno.Text;
				CmbOrderno_Leave(null, null);
			}
			else
			{
				CmbOrderno.TabStop = true;
				CmbOrderno.Enabled = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool formvalidation()
	{
		if (((Control)(object)cmbgptype).Text.Trim() == "" || ((Control)(object)cmbgptype).Text.Trim() == null || ((Control)(object)cmbgptype).Text.Trim() == "0")
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
		if (((UltraGridBase)CmbWeighBridgeStatus).ActiveRow == null || Conversion.ToInt(CmbWeighBridgeStatus.Value) == 0)
		{
			MessageBox.Show("Please Select WeighBridgeStatus");
			CmbWeighBridgeStatus.Focus();
			return false;
		}
		if (((Control)(object)CmbGPBasedOn).Text == "SaleOrder")
		{
			if (CmbOrderno.Text.Trim() == string.Empty || CmbOrderno.Text.Trim() == "0" || CmbOrderno.Text.Trim() == null)
			{
				MessageBox.Show("Please Enter Order#");
				CmbOrderno.Focus();
				return false;
			}
			if (((UltraGridBase)cmbsupp).ActiveRow == null)
			{
				MessageBox.Show("Please Select Customer Name");
				cmbsupp.Focus();
				return false;
			}
		}
		else if (Conversion.ToInt(CmbGPBasedOn.Value) != 3 && (CmbOrderno.Text.Trim() == string.Empty || CmbOrderno.Text.Trim() == "0" || CmbOrderno.Text.Trim() == null))
		{
			MessageBox.Show("Please Enter Delivery Order #");
			CmbOrderno.Focus();
			return false;
		}
		return true;
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		try
		{
			Id = 0;
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void Insert()
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		//IL_0378: Unknown result type (might be due to invalid IL or missing references)
		//IL_037e: Expected O, but got Unknown
		try
		{
			if (!formvalidation())
			{
				return;
			}
			if (((Control)(object)CmbStatus).Text != "Open" && Id == 0)
			{
				CmbStatus.Focus();
				throw new Exception("Status should be Open in Case of Save");
			}
			GatePassOutward gatePass = new GatePassOutward();
			if (Id > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				gatePass.Id = Id;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			gatePass.DocumentTypeId = 91;
			gatePass.RefDocumentTypeId = 1606;
			gatePass.EntryUser = UserAccount.ID;
			gatePass.OrganizationId = UserAccount.OrganizationId;
			gatePass.CompanyId = UserAccount.CompanyId;
			gatePass.BranchesId = UserAccount.BranchesId;
			gatePass.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			gatePass.ModifyUser = UserAccount.ID;
			gatePass.IsApproved = Approve;
			gatePass.GpDate = txtgpdate.Value;
			gatePass.GpSrNo = Conversion.ToInt((object)txtgpno.Text);
			gatePass.GatepassType = ((Control)(object)cmbgptype).Text.ToString();
			gatePass.GpTypeSrNo = Conversion.ToInt((object)txtgptypeno.Text.Trim());
			gatePass.OtherSupCust = ((Control)(object)CmbGPBasedOn).Text.Trim().ToString();
			if (gatePass.OtherSupCust != "General" && gatePass.OtherSupCust != "Purchase Return" && SOId == 0 && Conversion.ToInt(CmbGPBasedOn.Value) != 3)
			{
				CmbGPBasedOn.Focus();
				throw new Exception("Order No Not Found");
			}
			gatePass.SupplierContractCode = CmbOrderno.Text.Trim();
			gatePass.SupplierCustomerId = Conversion.ToInt(cmbsupp.Value);
			gatePass.OtherRemarks = txtremarks.Text.Trim();
			gatePass.SaleOrderId = SOId;
			gatePass.NoOfPackages = Conversion.ToInt((object)txtqty.Text.Trim());
			gatePass.VehicleType = ((Control)(object)cmbvehicletype).Text.Trim();
			gatePass.VehicleNo = txtvehicleno.Text.Trim();
			gatePass.BiltyNo = txtbiltyno.Text.Trim();
			gatePass.Freight = Conversion.ToDouble((object)txtfreight.Text);
			gatePass.NetPaid = Conversion.ToDouble((object)txtNetPaid.Text.Trim());
			gatePass.InDateTimeStamp = txtintime.Value;
			gatePass.OutDateTimeStamp = txtouttime.Value;
			gatePass.CityId = Conversion.ToInt(cmbcity.Value);
			gatePass.Status = ((Control)(object)CmbStatus).Text.Trim().ToString();
			gatePass.DocAttachment = ((Control)(object)CmbWeighBridgeStatus).Text.Trim();
			gatePass.WeighBridgeId = Conversion.ToInt((object)txtWeighBridgeSlipNo.Text.Trim());
			gatePass.FactoryWeight = Conversion.ToDouble((object)txtFactoryWeight.Text.Trim());
			gatePass.AttachmentsList = new List<DMSAttachments>();
			for (int i = 0; i < Attachment.dtAttachments.Rows.Count; i++)
			{
				DMSAttachments attach = new DMSAttachments();
				attach.Attachment = Path.GetFileName(Attachment.dtAttachments.Rows[i][1].ToString());
				attach.ScreenName = base.Name;
				gatePass.AttachmentsList.Add(attach);
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
			int success = GatePassOutward.Save(gatePass);
			if (Id > 0)
			{
				MessageBox.Show("Data Update Successfully...." + gatePass.GpSrNo);
				if (AT.RemovedAttachmentListInUpdateCase.Count > 0)
				{
					for (int j = 0; j < AT.RemovedAttachmentListInUpdateCase.Count; j++)
					{
						using (FileStream fs = new FileStream(AT.RemovedAttachmentListInUpdateCase[j].Path, FileMode.Open, FileAccess.ReadWrite))
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
								File.Delete(AT.RemovedAttachmentListInUpdateCase[j].Path);
								fileDeleted = true;
							}
							catch (IOException)
							{
								attempts++;
								Thread.Sleep(500);
							}
						}
						if (!fileDeleted)
						{
							throw new Exception("Unable to delete file. It is in use by another process.");
						}
					}
				}
			}
			else
			{
				MessageBox.Show("Record Save Successfully...." + gatePass.GpSrNo);
			}
			Reset();
			if (ChkBox.Checked)
			{
				CommonServices.GatePassOutwardSlipandRegister(success);
			}
		}
		catch (Exception ex2)
		{
			MessageBox.Show(ex2.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnupdate_Click(object sender, EventArgs e)
	{
		try
		{
			if (Id == 0)
			{
				throw new Exception("Record not update because Id not found");
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
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_0218: Unknown result type (might be due to invalid IL or missing references)
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		//IL_0238: Expected O, but got Unknown
		//IL_0373: Unknown result type (might be due to invalid IL or missing references)
		//IL_0378: Unknown result type (might be due to invalid IL or missing references)
		//IL_0389: Unknown result type (might be due to invalid IL or missing references)
		//IL_039a: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b6: Expected O, but got Unknown
		try
		{
			GatePassOutward gatePass = new GatePassOutward();
			Id = Conversion.ToInt((object)ID);
			gatePass = GatePassOutward.GetByID(Id);
			if (gatePass == null)
			{
				return;
			}
			tabControl1.SelectedIndex = 0;
			txtbiltyno.Text = gatePass.BiltyNo;
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
			if (Conversion.ToInt(CmbGPBasedOn.Value) == 1605)
			{
				DataTable lst = SupplierCustomer.GetSupplierBySaleOrderNo(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					OrderId = Conversion.ToInt((object)CmbOrderno.Text),
					DocumentTypeId = 1605,
					FinancialYearId = clsGlobalVariables.ActiveYr.Id
				});
				if (lst.Rows.Count > 0)
				{
					SOId = Conversion.ToInt((object)lst.Rows[0]["SaleOrderId"].ToString());
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
			else if (Conversion.ToInt(CmbGPBasedOn.Value) == 1606)
			{
				SOId = gatePass.SaleOrderId;
				DataTable Weight = InvDeliveryOrder.GetTotalWeightFromDeliveryOrder(new InvDeliveryOrder
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					DocumentTypeId = 1606,
					Id = SOId
				});
				if (Weight.Rows.Count > 0)
				{
					DDL.BindDDLNew(Weight, cmbsupp, "Id", "CompanyName", "CustomerName", false);
					((UltraGridBase)cmbsupp).Rows[0].Activate();
				}
			}
			else if (Conversion.ToInt(CmbGPBasedOn.Value) == 3)
			{
				SOId = gatePass.SaleOrderId;
				SupplierFill();
				CmbOrderno.Text = "";
				CmbOrderno.Enabled = false;
			}
			cmbsupp.Value = Conversion.ToInt((object)gatePass.SupplierCustomerId);
			((Control)(object)CmbStatus).Text = Conversion.ToString((object)gatePass.Status);
			txtNetPaid.Text = gatePass.NetPaid.ToString();
			txtWeighBridgeSlipNo.Text = gatePass.WeighBridgeId.ToString();
			txtFactoryWeight.Text = gatePass.FactoryWeight.ToString();
			txtApprovedBy.Text = gatePass.ApprovedUserName;
			((Control)(object)CmbWeighBridgeStatus).Text = Conversion.ToString((object)gatePass.DocAttachment);
			Approve = gatePass.IsApproved;
			((Control)(object)cmbgptype).Enabled = false;
			if (Conversion.ToDouble((object)txtFactoryWeight.Text) > 0.0)
			{
				((Control)(object)CmbWeighBridgeStatus).Enabled = false;
				CmbStatus.Value = 2;
			}
			else
			{
				((Control)(object)CmbWeighBridgeStatus).Enabled = true;
			}
			List<DMSAttachments> byID = DMSAttachments.GetByID(Conversion.ToInt((object)Id.ToString()), base.Name);
			AT = new Attachment();
			foreach (DMSAttachments Attachment in byID)
			{
				TempAttachments TA = new TempAttachments();
				TA.Id = 0;
				TA.Path = Attachment.Attachment;
				AT.lst.Add(TA);
			}
			btnupdate.Visible = true;
			btnsave.Visible = false;
			UpdateMode = true;
			CmbGPBasedOn_Leave(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtgpno_TextChanged(object sender, EventArgs e)
	{
	}

	private void btnDelete_Click(object sender, EventArgs e)
	{
	}

	private void CustomerHistoryFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dt = SupplierCustomer.SupplierCustomerAgainstOutwarGP(new SupplierCustomer
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbCustomer, "Id", "CompanyName", "Customer Name", ZeroIndex: false);
				((UltraGridBase)CmbCustomer).DisplayLayout.Bands[0].Columns[1].Width = 400;
			}
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
				txtFromdateHistory.Value = DateTime.Now.AddDays(-3.0);
				txtFromdateHistory.Focus();
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

	private void Resethistory()
	{
		try
		{
			txtFromNoHistory.Text = string.Empty;
			txtToDocNoHistory.Text = string.Empty;
			((Control)(object)CmbCustomer).Text = string.Empty;
			txtFromdateHistory.Value = DateTime.Now.AddDays(-3.0);
			txtFromdateHistory.Focus();
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
			Resethistory();
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
			CustomerHistoryFill();
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
				if (txtFromdateHistory.Checked)
				{
					obj.GpDateF = txtFromdateHistory.Value;
				}
				if (txtToDateHistory.Checked)
				{
					obj.GpDateT = txtToDateHistory.Value;
				}
			}
			else if (rdentrydate.Checked)
			{
				if (txtFromdateHistory.Checked)
				{
					obj.EntryDateFrom = txtFromdateHistory.Value;
				}
				if (txtToDateHistory.Checked)
				{
					obj.EntryDateTo = txtToDateHistory.Value;
				}
			}
			else if (rdmodifydate.Checked)
			{
				if (txtFromdateHistory.Checked)
				{
					obj.ModifyDateFrom = txtFromdateHistory.Value;
				}
				if (txtToDateHistory.Checked)
				{
					obj.ModifyDateTo = txtToDateHistory.Value;
				}
			}
			obj.GpSrNoF = Conversion.ToInt((object)txtFromNoHistory.Text);
			obj.GpSrNoT = Conversion.ToInt((object)txtToDocNoHistory.Text);
			obj.SupplierCustomerId = Conversion.ToInt(CmbCustomer.Value);
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
				dt.Columns.Add("ItemQty", typeof(double));
				dt.Columns.Add("VehicleType");
				dt.Columns.Add("VehicleNo");
				dt.Columns.Add("BiltyNo");
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
				dt.Columns.Add("EntryUser");
				dt.Columns.Add("EntryDate", typeof(DateTime));
				dt.Columns.Add("ModifyUser");
				dt.Columns.Add("ModifyDate", typeof(DateTime));
				dt.Columns.Add("IsApproved");
				dt.Columns.Add("OtherRemarks");
				dt.Columns.Add("NoOfAttachments", typeof(int));
				for (int i = 0; i < getList.Rows.Count; i++)
				{
					dt.Rows.Add(getList.Rows[i]["Id"], getList.Rows[i]["GatepassType"], getList.Rows[i]["OrderType"], getList.Rows[i]["GpSrNo"], Conversion.ToDateTime(getList.Rows[i]["GpDate"]).ToString("dd-MMM-yyyy"), getList.Rows[i]["DeliveryOrderNo"], getList.Rows[i]["OrderNo"], getList.Rows[i]["CompanyName"], getList.Rows[i]["ItemQty"], getList.Rows[i]["VehicleType"], getList.Rows[i]["VehicleNo"], getList.Rows[i]["BiltyNo"], Conversion.ToDateTime(getList.Rows[i]["InTime"]).ToString("dd-MMM-yyyy HH:mm:ss"), Conversion.ToDateTime(getList.Rows[i]["OutTime"]).ToString("dd-MMM-yyyy HH:mm:ss"), getList.Rows[i]["Description"], getList.Rows[i]["Freight"], getList.Rows[i]["NetPaid"], getList.Rows[i]["Status"], getList.Rows[i]["SupplierWeight"], getList.Rows[i]["FactoryWeight"], getList.Rows[i]["DifferenceWeight"], getList.Rows[i]["WeighBridgeStatus"], getList.Rows[i]["UserName"], getList.Rows[i]["EntryDate"], getList.Rows[i]["ModifyUserName"], getList.Rows[i]["ModifyDate"], getList.Rows[i]["IsApproved"], getList.Rows[i]["OtherRemarks"], getList.Rows[i]["NoOfAttachments"]);
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
			grdhistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdhistory.RootTable.Columns["EntryDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			grdhistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MM-yyyy hh:mm tt";
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
			CommonServices.GridAutoAdjustmentNew(grdhistory);
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

	private void btnsearch_Click(object sender, EventArgs e)
	{
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
			GetTicketNoandWeighBridgeWeight();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_013d: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Expected O, but got Unknown
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdhistory.CurrentRow;
			if (item == null || (int)item.RowType != 0 || item.Cells.Count <= 0)
			{
				return;
			}
			if (e.Column.Key == "Edit")
			{
				Reset();
				Id = Conversion.ToInt((object)grdhistory.CurrentRow.Cells["Id"].Value.ToString());
				((UltraGridBase)CmbStatus).Rows[1].Activate();
				GpId = Id;
				ReadById(Id);
				GetTicketNoandWeighBridgeWeight();
			}
			if (e.Column.Key == "Print")
			{
				CommonServices.GatePassOutwardSlipandRegister(Conversion.ToInt((object)grdhistory.CurrentRow.Cells["Id"].Value.ToString()));
			}
			if (e.Column.Key == "Print290")
			{
				if (Conversion.ToInt(grdhistory.CurrentRow.Cells["Id"].Value) <= 0)
				{
					throw new Exception("Record Not Found For Display");
				}
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

	private void grdhistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdhistory.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0 && e.Column.Key == "NoOfAttachments")
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(item.Cells["Id"].Value), 91);
			}
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
				dtOpenGatePass.Columns.Add("Qty", typeof(double));
				dtOpenGatePass.Columns.Add("VehicleType");
				dtOpenGatePass.Columns.Add("VehicleNo");
				dtOpenGatePass.Columns.Add("BiltyNo");
				dtOpenGatePass.Columns.Add("Status");
				dtOpenGatePass.Columns.Add("CityName");
				dtOpenGatePass.Columns.Add("FactoryWeight", typeof(double));
				dtOpenGatePass.Columns.Add("ApprovedBy");
				dtOpenGatePass.Columns.Add("Remaks");
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtOpenGatePass.Rows.Add(dt.Rows[i][Id], Conversion.ToDateTime(dt.Rows[i]["GpDate"]).ToShortDateString(), dt.Rows[i]["GpSrNo"], dt.Rows[i]["GatepassType"], dt.Rows[i]["OrderType"], dt.Rows[i]["OrderNo"], dt.Rows[i]["CustomerName"], dt.Rows[i]["NoOfBags"], dt.Rows[i]["VehicleType"], dt.Rows[i]["VehicleNo"], dt.Rows[i]["BiltyNo"], dt.Rows[i]["Status"], dt.Rows[i]["CityName"], dt.Rows[i]["FactoryWeight"], dt.Rows[i]["ApprovedBy"], dt.Rows[i]["OtherRemarks"]);
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
			grd.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Qty"].FormatString = "#,##0.###";
			grd.RootTable.Columns["Qty"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["FactoryWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["FactoryWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["FactoryWeight"].FormatString = "#,##0.###";
			grd.RootTable.Columns["FactoryWeight"].TotalFormatString = "#,##0.###";
			CommonServices.GridAutoAdjustmentNew(grd);
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

	private void grd_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			Id = Conversion.ToInt((object)grd.CurrentRow.Cells["Id"].Value.ToString());
			((UltraGridBase)CmbStatus).Rows[1].Activate();
			GpId = Id;
			ReadById(Id);
			GetTicketNoandWeighBridgeWeight();
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
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
			{
				if (e.Column.Key == "Edit")
				{
					Id = Conversion.ToInt((object)grd.CurrentRow.Cells["Id"].Value.ToString());
					((UltraGridBase)CmbStatus).Rows[1].Activate();
					GpId = Id;
					ReadById(Id);
					GetTicketNoandWeighBridgeWeight();
				}
				if (e.Column.Key == "Print")
				{
					CommonServices.GatePassOutwardSlipandRegister(Conversion.ToInt(grd.CurrentRow.Cells["Id"].Value));
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeliveryOrderGridFill()
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
			DataTable dtGrid = InvDeliveryOrder.DeliveryOrderDetailForGatePassOutward_Engr(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeIds = "1606",
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				BranchesId = UserAccount.BranchesId
			});
			if (dtGrid.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("DocumentTypeId", typeof(int));
				dt.Columns.Add("DocDate", typeof(DateTime));
				dt.Columns.Add("DoNo", typeof(int));
				dt.Columns.Add("SONo", typeof(int));
				dt.Columns.Add("DeliveryOrderType", typeof(string));
				dt.Columns.Add("CustomerName", typeof(string));
				dt.Columns.Add("DoQty", typeof(decimal));
				dt.Columns.Add("RequestedBy", typeof(string));
				dt.Columns.Add("ApprovedBy", typeof(string));
				dt.Columns.Add("Remarks", typeof(string));
				dt.Columns.Add("AccountRemarks", typeof(string));
				dt.Columns.Add("EntryDate", typeof(DateTime));
				dt.Columns.Add("EntryUserName", typeof(string));
				foreach (DataRow row in dtGrid.Rows)
				{
					dt.Rows.Add(row["Id"], row["DocumentTypeId"], row["DocDate"], row["DoNo"], row["SaleOrderNo"], row["DeliveryOrderType"], row["CustomerName"], row["DoQty"], row["RequestedBy"], row["ApprovedBy"], row["LoadingInstructions"], row["AccountRemarks"], row["EntryDate"], row["EntryUserName"]);
				}
				grdDoInformation.DataSource = dt;
				grdDoInformation.RetrieveStructure();
				GridDeliveryOrderSetting();
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

	private void GridDeliveryOrderSetting()
	{
		try
		{
			grdDoInformation.ColumnAutoResize = false;
			grdDoInformation.RootTable.Columns["Id"].Visible = false;
			grdDoInformation.RootTable.Columns["DocumentTypeId"].Visible = false;
			grdDoInformation.RootTable.Columns["DoNo"].ColumnType = (ColumnType)5;
			grdDoInformation.RootTable.Columns["DeliveryOrderType"].Width = 150;
			grdDoInformation.RootTable.Columns["EntryUserName"].Width = 150;
			grdDoInformation.RootTable.Columns["Remarks"].Width = 300;
			grdDoInformation.RootTable.Columns["CustomerName"].Width = 300;
			grdDoInformation.RootTable.Columns["AccountRemarks"].Width = 300;
			grdDoInformation.RootTable.Columns["DoQty"].AggregateFunction = (AggregateFunction)2;
			grdDoInformation.RootTable.Columns["DoQty"].TextAlignment = (TextAlignment)3;
			grdDoInformation.RootTable.Columns["DoQty"].FormatString = "#,##0.###";
			grdDoInformation.RootTable.Columns["DoQty"].TotalFormatString = "#,##0.###";
			CommonServices.GridAutoAdjustmentNew(grdDoInformation);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDoInformation_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdDoInformation.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0 && e.Column.Key == "DoNo")
			{
				CommonServices.InvDeliveryOrderSlip_Engr1606(Conversion.ToInt(r.Cells["Id"].Value), Conversion.ToInt(r.Cells["DocumentTypeId"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SaleOrderGridFill()
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
			DataTable dtGrid = InvDeliveryOrder.SaleOrderDetailForGatePassOutward_Engr(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeIds = "1605"
			});
			if (dtGrid.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("DocumentTypeId", typeof(int));
				dt.Columns.Add("DocDate", typeof(DateTime));
				dt.Columns.Add("OrderNo", typeof(int));
				dt.Columns.Add("CustomerName", typeof(string));
				dt.Columns.Add("ItemQty", typeof(decimal));
				dt.Columns.Add("DispatchQty", typeof(decimal));
				dt.Columns.Add("BalQty", typeof(decimal));
				dt.Columns.Add("OrderStatus", typeof(string));
				dt.Columns.Add("OrderRemarks", typeof(string));
				dt.Columns.Add("EntryDate", typeof(DateTime));
				dt.Columns.Add("EntryUserName", typeof(string));
				foreach (DataRow row in dtGrid.Rows)
				{
					dt.Rows.Add(row["Id"], row["DocumentTypeId"], row["DocDate"], row["OrderNo"], row["CustomerName"], row["ItemQty"], row["DispatchQty"], row["BalQty"], row["OrderStatus"], row["OrderRemarks"], row["EntryDate"], row["EntryUserName"]);
				}
				GrdSaleOrderInformation.DataSource = dt;
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
			GrdSaleOrderInformation.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			GrdSaleOrderInformation.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			GrdSaleOrderInformation.RootTable.Columns["ItemQty"].FormatString = "#,##0.###";
			GrdSaleOrderInformation.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.###";
			GrdSaleOrderInformation.RootTable.Columns["CustomerName"].Width = 300;
			GrdSaleOrderInformation.RootTable.Columns["EntryUserName"].Width = 150;
			GrdSaleOrderInformation.RootTable.Columns["DispatchQty"].AggregateFunction = (AggregateFunction)2;
			GrdSaleOrderInformation.RootTable.Columns["DispatchQty"].TextAlignment = (TextAlignment)3;
			GrdSaleOrderInformation.RootTable.Columns["DispatchQty"].FormatString = "#,##0.###";
			GrdSaleOrderInformation.RootTable.Columns["DispatchQty"].TotalFormatString = "#,##0.###";
			GrdSaleOrderInformation.RootTable.Columns["BalQty"].AggregateFunction = (AggregateFunction)2;
			GrdSaleOrderInformation.RootTable.Columns["BalQty"].TextAlignment = (TextAlignment)3;
			GrdSaleOrderInformation.RootTable.Columns["BalQty"].FormatString = "#,##0.###";
			GrdSaleOrderInformation.RootTable.Columns["BalQty"].TotalFormatString = "#,##0.###";
			CommonServices.GridAutoAdjustmentNew(GrdSaleOrderInformation);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdSaleOrderInformation_LinkClicked(object sender, ColumnActionEventArgs e)
	{
	}

	private void Reset()
	{
		try
		{
			SOId = 0;
			Id = 0;
			GpId = 0;
			Approve = false;
			UpdateMode = false;
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
			txtfreight.Text = string.Empty;
			txtFactoryWeight.Text = "0";
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
			txtgpdate.Focus();
			((Control)(object)CmbGPBasedOn).Enabled = true;
			((Control)(object)cmbgptype).Enabled = true;
			CmbOrderno.Enabled = true;
			((Control)(object)CmbWeighBridgeStatus).Enabled = true;
			((UltraGridBase)CmbWeighBridgeStatus).Rows[0].Activate();
			tabControl2.TabPages.Remove(tabSoInformation);
			tabControl2.TabPages.Remove(tabDoInformation);
			if (EnableSO)
			{
				tabControl2.TabPages.Add(tabSoInformation);
				SaleOrderGridFill();
			}
			if (EnableDO)
			{
				tabControl2.TabPages.Add(tabDoInformation);
				DeliveryOrderGridFill();
			}
			CmbGPBasedOn_Leave(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
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

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			gatepasstype();
			vehicleTypefill();
			CityFill();
			grdfrmfill();
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

	private void btnPrint_Click(object sender, EventArgs e)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected O, but got Unknown
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (Id > 0)
			{
				DataTable dt = GatePassOutwardReports.GatePassOutwardSlipandRegister(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					Id = Id
				});
				if (dt.Rows.Count == 0 || dt == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val.ShowReportWithDataTable(dt, "290-InvRptOutwardGatePassSlip.rpt");
				return;
			}
			throw new Exception("Record Not Found For Display");
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
		e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
	}

	private void CmbOrderno_KeyPress(object sender, KeyPressEventArgs e)
	{
		e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
	}

	private void txtgpno_KeyPress(object sender, KeyPressEventArgs e)
	{
		e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
	}

	private void txtvehicleno_TextChanged(object sender, EventArgs e)
	{
		txtvehicleno.SelectionStart = txtvehicleno.Text.Length;
		txtvehicleno.Text = txtvehicleno.Text.ToUpper();
	}

	private void txtFromNoHistory_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtToDocNoHistory_KeyPress(object sender, KeyPressEventArgs e)
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

	private void textBox1_TextChanged(object sender, EventArgs e)
	{
	}

	private void txtqty_TextChanged(object sender, EventArgs e)
	{
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
					txtFromdateHistory.Focus();
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
						((Control)(object)grdDoInformation).Focus();
						tabControl2.SelectedIndex = 1;
					}
					else if (tabControl2.SelectedIndex == 1)
					{
						((Control)(object)GrdSaleOrderInformation).Focus();
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
					btnshow_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					BtnNewHistory_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.Up)
				{
					txtFromdateHistory.Focus();
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
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Expected O, but got Unknown
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
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
					GetTicketNoandWeighBridgeWeight();
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
				GetTicketNoandWeighBridgeWeight();
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
					GetTicketNoandWeighBridgeWeight();
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
				GetTicketNoandWeighBridgeWeight();
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
				CommonServices.InvDeliveryOrderSlip_Engr1606(Conversion.ToInt(r.Cells["Id"].Value), Conversion.ToInt(r.Cells["DocumentTypeId"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdSaleOrderInformation_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdDoInformation.CurrentRow;
			if (r != null && (int)r.RowType == 0 && grdDoInformation.CurrentColumn != null && e.Control && e.KeyData == Keys.Space && grdDoInformation.CurrentColumn.Key == "OrderNo")
			{
				CommonServices.SaleOrderSlipEngr_1605(Conversion.ToInt(r.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnshow_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl.Name != null && base.ActiveControl.Name == "ctrlGrdBar2")
			{
				txtFromdateHistory.Focus();
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
		//IL_030c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0316: Expected O, but got Unknown
		//IL_0343: Unknown result type (might be due to invalid IL or missing references)
		//IL_034d: Expected O, but got Unknown
		//IL_036f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0379: Expected O, but got Unknown
		//IL_0482: Unknown result type (might be due to invalid IL or missing references)
		//IL_048c: Expected O, but got Unknown
		//IL_0532: Unknown result type (might be due to invalid IL or missing references)
		//IL_053c: Expected O, but got Unknown
		//IL_0548: Unknown result type (might be due to invalid IL or missing references)
		//IL_0552: Expected O, but got Unknown
		//IL_055e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0568: Expected O, but got Unknown
		//IL_0574: Unknown result type (might be due to invalid IL or missing references)
		//IL_057e: Expected O, but got Unknown
		//IL_057f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0589: Expected O, but got Unknown
		//IL_058a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0594: Expected O, but got Unknown
		//IL_0687: Unknown result type (might be due to invalid IL or missing references)
		//IL_0691: Expected O, but got Unknown
		//IL_0700: Unknown result type (might be due to invalid IL or missing references)
		//IL_070a: Expected O, but got Unknown
		//IL_0e2b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e35: Expected O, but got Unknown
		//IL_11cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_11d6: Expected O, but got Unknown
		//IL_14b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_14c3: Expected O, but got Unknown
		//IL_58ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_58f7: Expected O, but got Unknown
		//IL_5904: Unknown result type (might be due to invalid IL or missing references)
		//IL_590e: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdDoInformation_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GrdSaleOrderInformation_DesignTimeLayout = new GridEXLayout();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.SaleTrading.OutwardGatePassTrading));
		GridEXLayout grdhistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
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
		this.tabDoInformation = new System.Windows.Forms.TabPage();
		this.grdDoInformation = new GridEX();
		this.panel10 = new System.Windows.Forms.Panel();
		this.label7 = new System.Windows.Forms.Label();
		this.tabSoInformation = new System.Windows.Forms.TabPage();
		this.GrdSaleOrderInformation = new GridEX();
		this.panel11 = new System.Windows.Forms.Panel();
		this.label5 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.label17 = new System.Windows.Forms.Label();
		this.txtgpdate = new System.Windows.Forms.DateTimePicker();
		this.label2 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.txtgpno = new System.Windows.Forms.TextBox();
		this.label23 = new System.Windows.Forms.Label();
		this.txtgptypeno = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
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
		this.txtfreight = new System.Windows.Forms.TextBox();
		this.txtWeighBridgeSlipNo = new System.Windows.Forms.TextBox();
		this.txtApprovedBy = new System.Windows.Forms.TextBox();
		this.txtremarks = new System.Windows.Forms.TextBox();
		this.txtbiltyno = new System.Windows.Forms.TextBox();
		this.lblApprovedBy = new System.Windows.Forms.Label();
		this.txtqty = new System.Windows.Forms.TextBox();
		this.txtNetPaid = new System.Windows.Forms.TextBox();
		this.label27 = new System.Windows.Forms.Label();
		this.txtFactoryWeight = new System.Windows.Forms.TextBox();
		this.label19 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.CmbGPBasedOn = new UltraCombo();
		this.txtintime = new System.Windows.Forms.DateTimePicker();
		this.CmbStatus = new UltraCombo();
		this.txtouttime = new System.Windows.Forms.DateTimePicker();
		this.cmbcity = new UltraCombo();
		this.label26 = new System.Windows.Forms.Label();
		this.cmbgptype = new UltraCombo();
		this.cmbvehicletype = new UltraCombo();
		this.cmbsupp = new UltraCombo();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label15 = new System.Windows.Forms.Label();
		this.ChkBox = new System.Windows.Forms.CheckBox();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.btnPrint295 = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
		this.btndefvehicle = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.btnShortCut = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.grdhistory = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.label21 = new System.Windows.Forms.Label();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel5 = new System.Windows.Forms.Panel();
		this.groupBox4 = new System.Windows.Forms.GroupBox();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.btnshow = new System.Windows.Forms.Button();
		this.label73 = new System.Windows.Forms.Label();
		this.CmbCustomer = new UltraCombo();
		this.txtToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.label24 = new System.Windows.Forms.Label();
		this.label76 = new System.Windows.Forms.Label();
		this.txtToDocNoHistory = new System.Windows.Forms.TextBox();
		this.txtFromdateHistory = new System.Windows.Forms.DateTimePicker();
		this.label25 = new System.Windows.Forms.Label();
		this.label77 = new System.Windows.Forms.Label();
		this.txtFromNoHistory = new System.Windows.Forms.TextBox();
		this.panel7 = new System.Windows.Forms.Panel();
		this.label16 = new System.Windows.Forms.Label();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.BtnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel9.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.tabPage3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel6.SuspendLayout();
		this.tabDoInformation.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDoInformation).BeginInit();
		this.panel10.SuspendLayout();
		this.tabSoInformation.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdSaleOrderInformation).BeginInit();
		this.panel11.SuspendLayout();
		this.panel4.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbWeighBridgeStatus).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbGPBasedOn).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbStatus).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcity).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbgptype).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbvehicletype).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbsupp).BeginInit();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdhistory).BeginInit();
		this.panel8.SuspendLayout();
		this.panel5.SuspendLayout();
		this.groupBox4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomer).BeginInit();
		this.panel7.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(983, 661);
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
		this.tabControl1.Size = new System.Drawing.Size(983, 661);
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
		this.tabPage1.Size = new System.Drawing.Size(975, 632);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel9.Controls.Add(this.tabControl2);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel9.Location = new System.Drawing.Point(2, 225);
		this.panel9.Margin = new System.Windows.Forms.Padding(2);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(971, 405);
		this.panel9.TabIndex = 3;
		this.tabControl2.Controls.Add(this.tabPage3);
		this.tabControl2.Controls.Add(this.tabDoInformation);
		this.tabControl2.Controls.Add(this.tabSoInformation);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl2.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.tabControl2.Location = new System.Drawing.Point(0, 0);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(971, 405);
		this.tabControl2.TabIndex = 0;
		this.tabPage3.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.tabPage3.Controls.Add(this.panel6);
		this.tabPage3.Location = new System.Drawing.Point(4, 25);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage3.Size = new System.Drawing.Size(963, 376);
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
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(957, 344);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
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
		this.panel6.Size = new System.Drawing.Size(957, 26);
		this.panel6.TabIndex = 2;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold);
		this.label9.ForeColor = System.Drawing.Color.White;
		this.label9.Location = new System.Drawing.Point(4, 5);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(584, 16);
		this.label9.TabIndex = 60;
		this.label9.Text = "This Grid Show the  Vehicles Present In The Factory,Pending For Acceptance Or Rejection";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(919, 1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 23);
		this.ctrlGrdBar1.TabIndex = 59;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.tabDoInformation.Controls.Add((System.Windows.Forms.Control)(object)this.grdDoInformation);
		this.tabDoInformation.Controls.Add(this.panel10);
		this.tabDoInformation.Location = new System.Drawing.Point(4, 25);
		this.tabDoInformation.Name = "tabDoInformation";
		this.tabDoInformation.Padding = new System.Windows.Forms.Padding(3);
		this.tabDoInformation.Size = new System.Drawing.Size(963, 376);
		this.tabDoInformation.TabIndex = 1;
		this.tabDoInformation.Text = "Delivery Order Detail";
		this.tabDoInformation.UseVisualStyleBackColor = true;
		this.grdDoInformation.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdDoInformation).BackColor = System.Drawing.Color.White;
		grdDoInformation_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDoInformation.DesignTimeLayout = grdDoInformation_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDoInformation).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdDoInformation.DynamicFiltering = true;
		this.grdDoInformation.FilterMode = (FilterMode)2;
		this.grdDoInformation.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdDoInformation).Font = new System.Drawing.Font("Verdana", 8.25f);
		((System.Windows.Forms.Control)(object)this.grdDoInformation).Location = new System.Drawing.Point(3, 29);
		((System.Windows.Forms.Control)(object)this.grdDoInformation).Name = "grdDoInformation";
		((ScrollableBase)this.grdDoInformation).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDoInformation).Size = new System.Drawing.Size(957, 344);
		((System.Windows.Forms.Control)(object)this.grdDoInformation).TabIndex = 0;
		this.grdDoInformation.TotalRow = (InheritableBoolean)1;
		this.grdDoInformation.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDoInformation).VisualStyle = (VisualStyle)4;
		this.grdDoInformation.LinkClicked += new ColumnActionEventHandler(grdDoInformation_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdDoInformation).KeyDown += new System.Windows.Forms.KeyEventHandler(grdDoInformation_KeyDown);
		this.panel10.BackColor = System.Drawing.Color.Teal;
		this.panel10.Controls.Add(this.label7);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(3, 3);
		this.panel10.Margin = new System.Windows.Forms.Padding(2);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(957, 26);
		this.panel10.TabIndex = 2;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.ForeColor = System.Drawing.Color.White;
		this.label7.Location = new System.Drawing.Point(4, 6);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(441, 16);
		this.label7.TabIndex = 61;
		this.label7.Text = "This Grid Show The Un-Completed And Approved Delivery Orders....";
		this.tabSoInformation.AccessibleName = "";
		this.tabSoInformation.Controls.Add((System.Windows.Forms.Control)(object)this.GrdSaleOrderInformation);
		this.tabSoInformation.Controls.Add(this.panel11);
		this.tabSoInformation.Location = new System.Drawing.Point(4, 25);
		this.tabSoInformation.Name = "tabSoInformation";
		this.tabSoInformation.Padding = new System.Windows.Forms.Padding(3);
		this.tabSoInformation.Size = new System.Drawing.Size(963, 376);
		this.tabSoInformation.TabIndex = 2;
		this.tabSoInformation.Text = "Sale Order Detail";
		this.tabSoInformation.UseVisualStyleBackColor = true;
		this.GrdSaleOrderInformation.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.GrdSaleOrderInformation).BackColor = System.Drawing.Color.White;
		GrdSaleOrderInformation_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GrdSaleOrderInformation.DesignTimeLayout = GrdSaleOrderInformation_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdSaleOrderInformation).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GrdSaleOrderInformation.DynamicFiltering = true;
		this.GrdSaleOrderInformation.FilterMode = (FilterMode)2;
		this.GrdSaleOrderInformation.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GrdSaleOrderInformation).Font = new System.Drawing.Font("Verdana", 8.25f);
		((System.Windows.Forms.Control)(object)this.GrdSaleOrderInformation).Location = new System.Drawing.Point(3, 29);
		((System.Windows.Forms.Control)(object)this.GrdSaleOrderInformation).Name = "GrdSaleOrderInformation";
		((ScrollableBase)this.GrdSaleOrderInformation).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdSaleOrderInformation).Size = new System.Drawing.Size(957, 344);
		((System.Windows.Forms.Control)(object)this.GrdSaleOrderInformation).TabIndex = 4;
		this.GrdSaleOrderInformation.TotalRow = (InheritableBoolean)1;
		this.GrdSaleOrderInformation.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdSaleOrderInformation).VisualStyle = (VisualStyle)4;
		this.GrdSaleOrderInformation.LinkClicked += new ColumnActionEventHandler(GrdSaleOrderInformation_LinkClicked);
		((System.Windows.Forms.Control)(object)this.GrdSaleOrderInformation).KeyDown += new System.Windows.Forms.KeyEventHandler(GrdSaleOrderInformation_KeyDown);
		this.panel11.BackColor = System.Drawing.Color.Teal;
		this.panel11.Controls.Add(this.label5);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(3, 3);
		this.panel11.Margin = new System.Windows.Forms.Padding(2);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(957, 26);
		this.panel11.TabIndex = 3;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.ForeColor = System.Drawing.Color.White;
		this.label5.Location = new System.Drawing.Point(3, 6);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(839, 16);
		this.label5.TabIndex = 61;
		this.label5.Text = "This Grid Show The Un-Completed And Approved Sale Orders.... Complete.Cancel And Un-Approved Orders Will Not Appear here. ";
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(2, 57);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(971, 168);
		this.panel4.TabIndex = 0;
		this.groupBox1.Controls.Add(this.label17);
		this.groupBox1.Controls.Add(this.txtgpdate);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.label20);
		this.groupBox1.Controls.Add(this.label14);
		this.groupBox1.Controls.Add(this.label11);
		this.groupBox1.Controls.Add(this.txtgpno);
		this.groupBox1.Controls.Add(this.label23);
		this.groupBox1.Controls.Add(this.txtgptypeno);
		this.groupBox1.Controls.Add(this.label1);
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
		this.groupBox1.Controls.Add(this.txtfreight);
		this.groupBox1.Controls.Add(this.txtWeighBridgeSlipNo);
		this.groupBox1.Controls.Add(this.txtApprovedBy);
		this.groupBox1.Controls.Add(this.txtremarks);
		this.groupBox1.Controls.Add(this.txtbiltyno);
		this.groupBox1.Controls.Add(this.lblApprovedBy);
		this.groupBox1.Controls.Add(this.txtqty);
		this.groupBox1.Controls.Add(this.txtNetPaid);
		this.groupBox1.Controls.Add(this.label27);
		this.groupBox1.Controls.Add(this.txtFactoryWeight);
		this.groupBox1.Controls.Add(this.label19);
		this.groupBox1.Controls.Add(this.label18);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbGPBasedOn);
		this.groupBox1.Controls.Add(this.txtintime);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbStatus);
		this.groupBox1.Controls.Add(this.txtouttime);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbcity);
		this.groupBox1.Controls.Add(this.label26);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbgptype);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbvehicletype);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbsupp);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(4, 3);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(966, 161);
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
		this.label8.Location = new System.Drawing.Point(356, 50);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(73, 15);
		this.label8.TabIndex = 48;
		this.label8.Text = "Vehicle Type";
		this.label3.AutoSize = true;
		this.label3.BackColor = System.Drawing.Color.White;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(356, 79);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(64, 15);
		this.label3.TabIndex = 105;
		this.label3.Text = "Vehicle No";
		this.label20.AutoSize = true;
		this.label20.BackColor = System.Drawing.Color.White;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(552, 136);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(62, 15);
		this.label20.TabIndex = 50;
		this.label20.Text = "City Name";
		this.label14.AutoSize = true;
		this.label14.BackColor = System.Drawing.Color.White;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(4, 136);
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
		this.label23.Location = new System.Drawing.Point(748, 23);
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
		this.label1.AutoSize = true;
		this.label1.BackColor = System.Drawing.Color.White;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(4, 23);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(81, 15);
		this.label1.TabIndex = 60;
		this.label1.Text = "Gp Date && No";
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
		this.label10.Location = new System.Drawing.Point(356, 108);
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
		this.label22.Location = new System.Drawing.Point(356, 23);
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
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWeighBridgeStatus).DisplayLayout.Appearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.CmbWeighBridgeStatus).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWeighBridgeStatus).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance15).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance15).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWeighBridgeStatus).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance15;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWeighBridgeStatus).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance16;
		((SpecialBoxBase)((UltraGridBase)this.CmbWeighBridgeStatus).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance17).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance17).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWeighBridgeStatus).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbWeighBridgeStatus).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWeighBridgeStatus).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWeighBridgeStatus).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance18;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWeighBridgeStatus).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.CmbWeighBridgeStatus).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWeighBridgeStatus).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWeighBridgeStatus).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance22).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance22).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWeighBridgeStatus).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbWeighBridgeStatus).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWeighBridgeStatus).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance23).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance23).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance23).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWeighBridgeStatus).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance24).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWeighBridgeStatus).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbWeighBridgeStatus).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWeighBridgeStatus).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWeighBridgeStatus).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbWeighBridgeStatus).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWeighBridgeStatus).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbWeighBridgeStatus).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWeighBridgeStatus).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWeighBridgeStatus).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWeighBridgeStatus).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbWeighBridgeStatus.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWeighBridgeStatus).Location = new System.Drawing.Point(837, 102);
		((System.Windows.Forms.Control)(object)this.CmbWeighBridgeStatus).Name = "CmbWeighBridgeStatus";
		((System.Windows.Forms.Control)(object)this.CmbWeighBridgeStatus).Size = new System.Drawing.Size(125, 26);
		((System.Windows.Forms.Control)(object)this.CmbWeighBridgeStatus).TabIndex = 20;
		this.txtvehicleno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtvehicleno.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtvehicleno.Location = new System.Drawing.Point(429, 75);
		this.txtvehicleno.Name = "txtvehicleno";
		this.txtvehicleno.Size = new System.Drawing.Size(117, 23);
		this.txtvehicleno.TabIndex = 10;
		this.txtvehicleno.TextChanged += new System.EventHandler(txtvehicleno_TextChanged);
		this.label35.AutoSize = true;
		this.label35.BackColor = System.Drawing.Color.White;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(748, 108);
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
		this.txtfreight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtfreight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtfreight.Location = new System.Drawing.Point(614, 19);
		this.txtfreight.Name = "txtfreight";
		this.txtfreight.Size = new System.Drawing.Size(129, 23);
		this.txtfreight.TabIndex = 12;
		this.txtfreight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtfreight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtfreight_KeyPress);
		this.txtWeighBridgeSlipNo.BackColor = System.Drawing.SystemColors.Menu;
		this.txtWeighBridgeSlipNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtWeighBridgeSlipNo.Enabled = false;
		this.txtWeighBridgeSlipNo.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtWeighBridgeSlipNo.Location = new System.Drawing.Point(837, 75);
		this.txtWeighBridgeSlipNo.Name = "txtWeighBridgeSlipNo";
		this.txtWeighBridgeSlipNo.Size = new System.Drawing.Size(125, 23);
		this.txtWeighBridgeSlipNo.TabIndex = 19;
		this.txtApprovedBy.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtApprovedBy.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtApprovedBy.Location = new System.Drawing.Point(837, 132);
		this.txtApprovedBy.Name = "txtApprovedBy";
		this.txtApprovedBy.ReadOnly = true;
		this.txtApprovedBy.Size = new System.Drawing.Size(125, 23);
		this.txtApprovedBy.TabIndex = 21;
		this.txtApprovedBy.TabStop = false;
		this.txtremarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtremarks.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtremarks.Location = new System.Drawing.Point(111, 132);
		this.txtremarks.Name = "txtremarks";
		this.txtremarks.Size = new System.Drawing.Size(435, 23);
		this.txtremarks.TabIndex = 7;
		this.txtbiltyno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtbiltyno.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtbiltyno.Location = new System.Drawing.Point(429, 104);
		this.txtbiltyno.Name = "txtbiltyno";
		this.txtbiltyno.Size = new System.Drawing.Size(117, 23);
		this.txtbiltyno.TabIndex = 11;
		this.lblApprovedBy.AutoSize = true;
		this.lblApprovedBy.BackColor = System.Drawing.Color.White;
		this.lblApprovedBy.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblApprovedBy.Location = new System.Drawing.Point(748, 136);
		this.lblApprovedBy.Name = "lblApprovedBy";
		this.lblApprovedBy.Size = new System.Drawing.Size(75, 15);
		this.lblApprovedBy.TabIndex = 80;
		this.lblApprovedBy.Text = "Approved By";
		this.txtqty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtqty.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtqty.Location = new System.Drawing.Point(429, 19);
		this.txtqty.Name = "txtqty";
		this.txtqty.Size = new System.Drawing.Size(117, 23);
		this.txtqty.TabIndex = 8;
		this.txtqty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtqty.TextChanged += new System.EventHandler(txtqty_TextChanged);
		this.txtqty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtqty_KeyPress);
		this.txtNetPaid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNetPaid.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtNetPaid.Location = new System.Drawing.Point(614, 46);
		this.txtNetPaid.Name = "txtNetPaid";
		this.txtNetPaid.Size = new System.Drawing.Size(129, 23);
		this.txtNetPaid.TabIndex = 13;
		this.txtNetPaid.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtNetPaid.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtNetPaid_KeyPress);
		this.label27.AutoSize = true;
		this.label27.BackColor = System.Drawing.Color.White;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(748, 50);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(88, 15);
		this.label27.TabIndex = 78;
		this.label27.Text = "Factory Weight";
		this.txtFactoryWeight.BackColor = System.Drawing.SystemColors.Menu;
		this.txtFactoryWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFactoryWeight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtFactoryWeight.Location = new System.Drawing.Point(837, 46);
		this.txtFactoryWeight.Name = "txtFactoryWeight";
		this.txtFactoryWeight.ReadOnly = true;
		this.txtFactoryWeight.Size = new System.Drawing.Size(125, 23);
		this.txtFactoryWeight.TabIndex = 18;
		this.txtFactoryWeight.TabStop = false;
		this.txtFactoryWeight.Text = "0";
		this.txtFactoryWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label19.AutoSize = true;
		this.label19.BackColor = System.Drawing.Color.White;
		this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.ForeColor = System.Drawing.Color.Red;
		this.label19.Location = new System.Drawing.Point(96, 76);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(15, 20);
		this.label19.TabIndex = 77;
		this.label19.Text = "*";
		this.label18.AutoSize = true;
		this.label18.BackColor = System.Drawing.Color.White;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(4, 79);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(96, 15);
		this.label18.TabIndex = 76;
		this.label18.Text = "Order Type && No";
		this.CmbGPBasedOn.AllowNull = (DefaultableBoolean)1;
		this.CmbGPBasedOn.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbGPBasedOn.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbGPBasedOn.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbGPBasedOn).DisplayLayout.Appearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.CmbGPBasedOn).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbGPBasedOn).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance41).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance41).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbGPBasedOn).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbGPBasedOn).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance42;
		((SpecialBoxBase)((UltraGridBase)this.CmbGPBasedOn).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance44).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance44).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbGPBasedOn).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbGPBasedOn).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbGPBasedOn).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbGPBasedOn).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance46).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbGPBasedOn).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.CmbGPBasedOn).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbGPBasedOn).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbGPBasedOn).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance47;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance48).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbGPBasedOn).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.CmbGPBasedOn).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbGPBasedOn).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance49).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance49).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance49).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance49).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbGPBasedOn).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance27).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbGPBasedOn).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.CmbGPBasedOn).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbGPBasedOn).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbGPBasedOn).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbGPBasedOn).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbGPBasedOn).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance29;
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
		this.txtintime.TabIndex = 14;
		this.txtintime.TabStop = false;
		this.txtintime.Value = new System.DateTime(2020, 8, 27, 9, 36, 0, 0);
		this.CmbStatus.AllowNull = (DefaultableBoolean)1;
		this.CmbStatus.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbStatus.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbStatus.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance94).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Appearance = (AppearanceBase)(object)appearance94;
		((UltraGridBase)this.CmbStatus).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbStatus).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance95).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance95).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance95).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbStatus).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance95;
		((AppearanceBase)appearance96).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbStatus).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance96;
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
		((System.Windows.Forms.Control)(object)this.CmbStatus).Location = new System.Drawing.Point(837, 17);
		((System.Windows.Forms.Control)(object)this.CmbStatus).Name = "CmbStatus";
		((System.Windows.Forms.Control)(object)this.CmbStatus).Size = new System.Drawing.Size(125, 26);
		((System.Windows.Forms.Control)(object)this.CmbStatus).TabIndex = 17;
		this.txtouttime.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
		this.txtouttime.CustomFormat = "HH:mm";
		this.txtouttime.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtouttime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtouttime.Location = new System.Drawing.Point(614, 104);
		this.txtouttime.Name = "txtouttime";
		this.txtouttime.Size = new System.Drawing.Size(129, 23);
		this.txtouttime.TabIndex = 15;
		this.txtouttime.TabStop = false;
		this.txtouttime.Value = new System.DateTime(2020, 8, 27, 9, 36, 0, 0);
		this.cmbcity.AllowNull = (DefaultableBoolean)1;
		this.cmbcity.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbcity.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbcity.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcity).DisplayLayout.Appearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.cmbcity).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcity).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance31).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance31).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcity).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcity).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance33;
		((SpecialBoxBase)((UltraGridBase)this.cmbcity).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcity).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.cmbcity).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcity).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance35;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance68;
		((AppearanceBase)appearance69).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance69).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance70).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance70).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance70).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance70).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance37).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcity).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.cmbcity).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcity).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcity).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcity).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbcity.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbcity).Location = new System.Drawing.Point(614, 130);
		((System.Windows.Forms.Control)(object)this.cmbcity).Name = "cmbcity";
		((System.Windows.Forms.Control)(object)this.cmbcity).Size = new System.Drawing.Size(129, 26);
		((System.Windows.Forms.Control)(object)this.cmbcity).TabIndex = 16;
		this.label26.AutoSize = true;
		this.label26.BackColor = System.Drawing.Color.White;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(748, 79);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(70, 15);
		this.label26.TabIndex = 64;
		this.label26.Text = "W.B Slip No";
		this.cmbgptype.AllowNull = (DefaultableBoolean)1;
		this.cmbgptype.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbgptype.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbgptype.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbgptype).DisplayLayout.Appearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.cmbgptype).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbgptype).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance82).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance82).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance82).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbgptype).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance82;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbgptype).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance83;
		((SpecialBoxBase)((UltraGridBase)this.cmbgptype).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance84).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance84).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbgptype).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.cmbgptype).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbgptype).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance85).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbgptype).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance87).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbgptype).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.cmbgptype).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbgptype).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbgptype).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance88;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance89).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbgptype).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.cmbgptype).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbgptype).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance90).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance90).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance90).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance90).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbgptype).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance90;
		((AppearanceBase)appearance91).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbgptype).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.cmbgptype).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbgptype).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbgptype).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.cmbgptype).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbgptype).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance93;
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
		this.cmbvehicletype.AllowNull = (DefaultableBoolean)1;
		this.cmbvehicletype.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbvehicletype.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbvehicletype.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbvehicletype).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance32;
		((SpecialBoxBase)((UltraGridBase)this.cmbvehicletype).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance43).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance43).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance54;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance61).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance64;
		((AppearanceBase)appearance75).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance75).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance86).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance86).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance86).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance86;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbvehicletype).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbvehicletype.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbvehicletype).Location = new System.Drawing.Point(429, 44);
		((System.Windows.Forms.Control)(object)this.cmbvehicletype).Name = "cmbvehicletype";
		((System.Windows.Forms.Control)(object)this.cmbvehicletype).Size = new System.Drawing.Size(117, 26);
		((System.Windows.Forms.Control)(object)this.cmbvehicletype).TabIndex = 9;
		this.cmbsupp.AllowNull = (DefaultableBoolean)1;
		this.cmbsupp.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbsupp.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbsupp.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbsupp).DisplayLayout.Appearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.cmbsupp).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbsupp).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance38).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance38).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbsupp).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbsupp).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance39;
		((SpecialBoxBase)((UltraGridBase)this.cmbsupp).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbsupp).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.cmbsupp).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbsupp).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbsupp).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance76;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance77).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbsupp).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.cmbsupp).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbsupp).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbsupp).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance78;
		((AppearanceBase)appearance79).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance79).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbsupp).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.cmbsupp).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbsupp).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance80).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance80).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance80).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbsupp).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance65).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbsupp).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.cmbsupp).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbsupp).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbsupp).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.cmbsupp).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbsupp).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance67;
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
		this.panel3.Controls.Add(this.label15);
		this.panel3.Controls.Add(this.ChkBox);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(2, 31);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(971, 26);
		this.panel3.TabIndex = 1;
		this.label15.AutoSize = true;
		this.label15.BackColor = System.Drawing.Color.Teal;
		this.label15.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label15.Location = new System.Drawing.Point(3, 5);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(165, 19);
		this.label15.TabIndex = 0;
		this.label15.Text = "Outward Gate Pass";
		this.ChkBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkBox.AutoSize = true;
		this.ChkBox.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkBox.ForeColor = System.Drawing.Color.White;
		this.ChkBox.Location = new System.Drawing.Point(889, 3);
		this.ChkBox.Name = "ChkBox";
		this.ChkBox.Size = new System.Drawing.Size(79, 20);
		this.ChkBox.TabIndex = 0;
		this.ChkBox.Text = "Preview";
		this.ChkBox.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(2, 2);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(971, 29);
		this.panel2.TabIndex = 0;
		this.toolStrip1.BackColor = System.Drawing.Color.White;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[16]
		{
			this.toolStripSeparator3, this.btnnew, this.btnRefresh, this.toolStripSeparator2, this.btnsave, this.btnupdate, this.btnDelete, this.toolStripSeparator4, this.btnAttachment, this.toolStripSeparator5,
			this.btnPrint, this.btnPrint295, this.toolStripSeparator6, this.btndefvehicle, this.toolStripSeparator1, this.btnShortCut
		});
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(971, 27);
		this.toolStrip1.TabIndex = 58;
		this.toolStrip1.Text = "toolStrip1";
		this.toolStripSeparator3.Name = "toolStripSeparator3";
		this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(60, 24);
		this.btnnew.Text = "&New";
		this.btnnew.ToolTipText = "Ctrl_N";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnRefresh.Image = (System.Drawing.Image)resources.GetObject("btnRefresh.Image");
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(81, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.ToolTipText = "Alt_R";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(65, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.ToolTipText = "Ctrl_S";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnupdate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.btnupdate.Image = (System.Drawing.Image)resources.GetObject("btnupdate.Image");
		this.btnupdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnupdate.Name = "btnupdate";
		this.btnupdate.Size = new System.Drawing.Size(79, 24);
		this.btnupdate.Text = "&Update";
		this.btnupdate.ToolTipText = "Ctrl_U";
		this.btnupdate.Click += new System.EventHandler(btnupdate_Click);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(74, 24);
		this.btnDelete.Text = "Delete";
		this.btnDelete.Visible = false;
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.toolStripSeparator4.Name = "toolStripSeparator4";
		this.toolStripSeparator4.Size = new System.Drawing.Size(6, 27);
		this.btnAttachment.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.btnAttachment.Image = (System.Drawing.Image)resources.GetObject("btnAttachment.Image");
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(110, 24);
		this.btnAttachment.Text = "&Attachment";
		this.btnAttachment.ToolTipText = "Alt_A";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.toolStripSeparator5.Name = "toolStripSeparator5";
		this.toolStripSeparator5.Size = new System.Drawing.Size(6, 27);
		this.btnPrint.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.btnPrint.Image = (System.Drawing.Image)resources.GetObject("btnPrint.Image");
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(93, 24);
		this.btnPrint.Text = "290-Print";
		this.btnPrint.ToolTipText = "Ctrl_P";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.btnPrint295.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint295.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint295.Name = "btnPrint295";
		this.btnPrint295.Size = new System.Drawing.Size(93, 24);
		this.btnPrint295.Text = "295-Print";
		this.btnPrint295.Click += new System.EventHandler(btnPrint291_Click);
		this.toolStripSeparator6.Name = "toolStripSeparator6";
		this.toolStripSeparator6.Size = new System.Drawing.Size(6, 27);
		this.btndefvehicle.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btndefvehicle.Name = "btndefvehicle";
		this.btndefvehicle.Size = new System.Drawing.Size(104, 24);
		this.btndefvehicle.Text = "Define Vehicle";
		this.btndefvehicle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.btndefvehicle.Click += new System.EventHandler(btndefvehicle_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.btnShortCut.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortCut.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortCut.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCut.Name = "btnShortCut";
		this.btnShortCut.Size = new System.Drawing.Size(127, 24);
		this.btnShortCut.Text = "ShortCut Keys";
		this.btnShortCut.Click += new System.EventHandler(btnShortCut_Click);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.grdhistory);
		this.tabPage2.Controls.Add(this.panel8);
		this.tabPage2.Controls.Add(this.panel5);
		this.tabPage2.Controls.Add(this.panel7);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage2.Size = new System.Drawing.Size(975, 632);
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
		((System.Windows.Forms.Control)(object)this.grdhistory).Location = new System.Drawing.Point(2, 158);
		((System.Windows.Forms.Control)(object)this.grdhistory).Name = "grdhistory";
		((ScrollableBase)this.grdhistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdhistory).Size = new System.Drawing.Size(971, 472);
		((System.Windows.Forms.Control)(object)this.grdhistory).TabIndex = 0;
		this.grdhistory.TotalRow = (InheritableBoolean)1;
		this.grdhistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdhistory).VisualStyle = (VisualStyle)4;
		this.grdhistory.ColumnButtonClick += new ColumnActionEventHandler(grdhistory_ColumnButtonClick);
		this.grdhistory.LinkClicked += new ColumnActionEventHandler(grdhistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdhistory).DoubleClick += new System.EventHandler(grdhistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdhistory).KeyDown += new System.Windows.Forms.KeyEventHandler(grdhistory_KeyDown);
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.label21);
		this.panel8.Controls.Add(this.ctrlGrdBar2);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(2, 132);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(971, 26);
		this.panel8.TabIndex = 78;
		this.label21.AutoSize = true;
		this.label21.BackColor = System.Drawing.Color.Teal;
		this.label21.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label21.Location = new System.Drawing.Point(5, 3);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(67, 19);
		this.label21.TabIndex = 74;
		this.label21.Text = "History";
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1235, 1);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar2.MyGrid = this.grdhistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(57, 28);
		this.ctrlGrdBar2.TabIndex = 73;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.panel5.Controls.Add(this.groupBox4);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(2, 52);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(971, 80);
		this.panel5.TabIndex = 76;
		this.groupBox4.Controls.Add(this.rdmodifydate);
		this.groupBox4.Controls.Add(this.drdocdate);
		this.groupBox4.Controls.Add(this.rdentrydate);
		this.groupBox4.Controls.Add(this.btnshow);
		this.groupBox4.Controls.Add(this.label73);
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomer);
		this.groupBox4.Controls.Add(this.txtToDateHistory);
		this.groupBox4.Controls.Add(this.label24);
		this.groupBox4.Controls.Add(this.label76);
		this.groupBox4.Controls.Add(this.txtToDocNoHistory);
		this.groupBox4.Controls.Add(this.txtFromdateHistory);
		this.groupBox4.Controls.Add(this.label25);
		this.groupBox4.Controls.Add(this.label77);
		this.groupBox4.Controls.Add(this.txtFromNoHistory);
		this.groupBox4.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox4.Location = new System.Drawing.Point(3, 3);
		this.groupBox4.Name = "groupBox4";
		this.groupBox4.Size = new System.Drawing.Size(966, 73);
		this.groupBox4.TabIndex = 0;
		this.groupBox4.TabStop = false;
		this.groupBox4.Text = "Filters";
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(869, 46);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 7;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(703, 46);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 5;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(783, 46);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 6;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.btnshow.BackColor = System.Drawing.Color.Teal;
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 10f, System.Drawing.FontStyle.Bold);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(630, 43);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(61, 25);
		this.btnshow.TabIndex = 8;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.btnshow.Leave += new System.EventHandler(btnshow_Leave);
		this.label73.AutoSize = true;
		this.label73.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label73.Location = new System.Drawing.Point(433, 20);
		this.label73.Name = "label73";
		this.label73.Size = new System.Drawing.Size(58, 15);
		this.label73.TabIndex = 178;
		this.label73.Text = "Customer";
		this.CmbCustomer.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomer.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomer.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomer.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Appearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance51).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance51).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance51).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomer).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance51;
		((AppearanceBase)appearance52).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance52;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomer).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance53).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance55;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance57;
		((AppearanceBase)appearance58).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance58).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance59).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance59).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance59).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance62).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomer).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomer.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomer).Location = new System.Drawing.Point(493, 14);
		((System.Windows.Forms.Control)(object)this.CmbCustomer).Name = "CmbCustomer";
		((System.Windows.Forms.Control)(object)this.CmbCustomer).Size = new System.Drawing.Size(198, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomer).TabIndex = 4;
		this.txtToDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDateHistory.CustomFormat = "dd-MMM-yy";
		this.txtToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtToDateHistory.Location = new System.Drawing.Point(66, 44);
		this.txtToDateHistory.Name = "txtToDateHistory";
		this.txtToDateHistory.ShowCheckBox = true;
		this.txtToDateHistory.Size = new System.Drawing.Size(138, 23);
		this.txtToDateHistory.TabIndex = 1;
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(1, 48);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(48, 15);
		this.label24.TabIndex = 171;
		this.label24.Text = "Date To";
		this.label76.AutoSize = true;
		this.label76.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label76.Location = new System.Drawing.Point(2, 20);
		this.label76.Name = "label76";
		this.label76.Size = new System.Drawing.Size(63, 15);
		this.label76.TabIndex = 171;
		this.label76.Text = "Date From";
		this.txtToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDocNoHistory.Location = new System.Drawing.Point(269, 44);
		this.txtToDocNoHistory.Name = "txtToDocNoHistory";
		this.txtToDocNoHistory.Size = new System.Drawing.Size(155, 23);
		this.txtToDocNoHistory.TabIndex = 3;
		this.txtToDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtToDocNoHistory_KeyPress);
		this.txtFromdateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromdateHistory.CustomFormat = "dd-MMM-yy";
		this.txtFromdateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromdateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtFromdateHistory.Location = new System.Drawing.Point(66, 16);
		this.txtFromdateHistory.Name = "txtFromdateHistory";
		this.txtFromdateHistory.ShowCheckBox = true;
		this.txtFromdateHistory.Size = new System.Drawing.Size(138, 23);
		this.txtFromdateHistory.TabIndex = 0;
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(210, 48);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(38, 15);
		this.label25.TabIndex = 173;
		this.label25.Text = "Gp To";
		this.label77.AutoSize = true;
		this.label77.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label77.Location = new System.Drawing.Point(210, 20);
		this.label77.Name = "label77";
		this.label77.Size = new System.Drawing.Size(53, 15);
		this.label77.TabIndex = 173;
		this.label77.Text = "Gp From";
		this.txtFromNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFromNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromNoHistory.Location = new System.Drawing.Point(269, 16);
		this.txtFromNoHistory.Name = "txtFromNoHistory";
		this.txtFromNoHistory.Size = new System.Drawing.Size(155, 23);
		this.txtFromNoHistory.TabIndex = 2;
		this.txtFromNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFromNoHistory_KeyPress);
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.label16);
		this.panel7.Controls.Add(this.toolStrip2);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(2, 2);
		this.panel7.Margin = new System.Windows.Forms.Padding(2);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(971, 50);
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
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.BtnNewHistory, this.btnRefreshHistory });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(971, 27);
		this.toolStrip2.TabIndex = 59;
		this.toolStrip2.Text = "toolStrip2";
		this.BtnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.BtnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnNewHistory.Name = "BtnNewHistory";
		this.BtnNewHistory.Size = new System.Drawing.Size(60, 24);
		this.BtnNewHistory.Text = "&New";
		this.BtnNewHistory.ToolTipText = "Ctrl_L";
		this.BtnNewHistory.Click += new System.EventHandler(BtnNewHistory_Click);
		this.btnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshHistory.Name = "btnRefreshHistory";
		this.btnRefreshHistory.Size = new System.Drawing.Size(81, 24);
		this.btnRefreshHistory.Text = "Refresh";
		this.btnRefreshHistory.Click += new System.EventHandler(btnRefreshHistory_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(983, 661);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "OutwardGatePassTrading";
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
		this.tabDoInformation.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDoInformation).EndInit();
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		this.tabSoInformation.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GrdSaleOrderInformation).EndInit();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbWeighBridgeStatus).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbGPBasedOn).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbStatus).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcity).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbgptype).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbvehicletype).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbsupp).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdhistory).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.groupBox4.ResumeLayout(false);
		this.groupBox4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomer).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}
