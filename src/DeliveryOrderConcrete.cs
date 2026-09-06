using System;
using System.Collections;
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
using Architecture.BLL.pcc;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.Model.pcc;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
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
namespace Architecture.WinApp.pcc.Sale
{

public class DeliveryOrderConcrete : Form
{
	private Attachment AT = new Attachment();

	public DataTable dtViewAttachments = new DataTable();

	public DataTable dtAttachmentsUpload = new DataTable();

	public DataTable dthistoryfill = new DataTable();

	private DropDownBind DDL = new DropDownBind();

	public static int Id;

	public static int DeliveryOrderFormId;

	private bool IsApproved;

	private DataTable table = new DataTable();

	private DataTable dtWh = new DataTable();

	private DataTable dtCity = new DataTable();

	private DataTable dtJobLoat = new DataTable();

	private DataTable dtsuppcus = new DataTable();

	private DataTable dtRefParty = new DataTable();

	private DataTable dtvar = new DataTable();

	private DataTable dtOrderItem = new DataTable();

	private DataTable dtVehicleNos = new DataTable();

	private Rightsobjects formright = new Rightsobjects();

	public string dtSaleOrderIds = "";

	public string dtSaleOrderMainIds = "";

	private int updateDetailIndex;

	private bool SubsidiaryAccountAllownOnVouchers;

	private LoadSaleOrderForDeliveryOrderConcrete LoadTransactions;

	private List<InvDeliveryOrderdetail> lstRemoveRecord = new List<InvDeliveryOrderdetail>();

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

	private ToolStripButton btnPrintCustomerWise;

	private ToolStripButton btnattachment;

	private Label label23;

	private Panel panel4;

	private Label label33;

	private Label label13;

	private Label label18;

	private DateTimePicker DocDate;

	private TextBox txtdocno;

	private Label label5;

	private GroupBox groupBox3;

	private Button btnplus;

	private TextBox txtremarksdetail;

	private Label label47;

	private TextBox txtItemWeight;

	private TextBox txtqty;

	private Label label39;

	private Label label38;

	private Label label36;

	private Label label37;

	private Panel panel8;

	internal UltraCombo CmbItemName;

	internal UltraCombo CmbAttributeVarient;

	private CtrlGrdBar ctrlGrdBar1;

	private CtrlGrdBar ctrlGrdBar2;

	private Button btnUpdateDetail;

	private Button btnCancelUpdateDetial;

	private Panel panel12;

	private ToolStripButton btnRefresh;

	private ToolStrip toolStrip2;

	private ToolStripButton btnNewHistory;

	internal UltraCombo cmbBranchTo;

	internal UltraCombo cmbBranchFrom;

	internal UltraCombo CmbSupplierCustomer;

	private Label label2;

	private TextBox txtRemarksHeader;

	internal UltraCombo CmbWareHouse;

	private Label label3;

	private Label label4;

	internal UltraCombo CmbDeliveryOrderType;

	internal UltraCombo CmbJobLot;

	private Label label8;

	private ToolStripButton btnLoadSaleOrder;

	private CheckBox ChkPrint;

	private TextBox txtNetWeight;

	private Label label6;

	private ToolStripButton btnDefineReferenceParties;

	internal UltraCombo CmbOrderNo;

	private Label label9;

	private TextBox txtBalWeight;

	private Label label11;

	private TextBox txtBalQty;

	private Label label10;

	private TextBox txtSubNoId;

	private TextBox txtDocNoId;

	private TextBox txtRefDocTypeId;

	private Label lblBalance;

	private Label label12;

	internal UltraCombo CmbVehicleType;

	private Label label14;

	private ToolStripButton btnDelete;

	private GroupBox groupBox1;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripSeparator toolStripSeparator3;

	internal GridEX grd;

	private Panel panel5;

	private Label label15;

	private Label label16;

	internal GridEX GrdHistoryDetail;

	private Panel panel6;

	private Label label27;

	private TabControl DetailGridPanel;

	private TabPage tabPage3;

	private TextBox txtVarientEquivalent;

	private Label label17;

	private Button BtnDefineCity;

	internal UltraCombo combcityarea;

	private Label label19;

	private Label label21;

	private Label label20;

	private Label label22;

	private TextBox txtRefPartyAddress;

	private CheckBox chkisFreeOfCost;

	private Label label24;

	private ToolStripSeparator toolStripSeparator4;

	private ToolStripSeparator toolStripSeparator5;

	private ToolStripSeparator toolStripSeparator6;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripButton btnshortcutkeys;

	private RadioButton rdbtnItemCode;

	private RadioButton rdbtnItemName;

	private TextBox txtReferenceNoHeader;

	private Panel panel10;

	internal GridEX grdhistory;

	private Label label1;

	private GroupBox groupBox4;

	private Button btnShowHistory;

	internal UltraCombo CmbItemHistory;

	private Label label72;

	private Label label73;

	internal UltraCombo CmbCustomerHistory;

	private Label label75;

	internal UltraCombo cmbDateTypeHistory;

	private DateTimePicker txtToDateHistory;

	private Label label76;

	private TextBox txtToDocNoHistory;

	private DateTimePicker txtFromdateHistory;

	private Label label77;

	private TextBox txtFromNoHistory;

	private Label label25;

	internal UltraCombo CmbVehicleNoHistory;

	private ToolStripButton BtnRefreshHistory;

	private TextBox txtRefPartyCellNo;

	private ToolStripButton btnPrintSlip;

	private CheckBox chkPrintCustomerWise;

	internal UltraCombo CmbVehicleNo;

	private TextBox txtDistance;

	private Label label60;

	internal UltraCombo Cmbheight;

	internal UltraCombo CmbReferenceParty;

	private TextBox txtArea;

	internal UltraCombo Cmbstoreys;

	private Label label7;

	private Label label70;

	private Label label69;

	public DeliveryOrderConcrete(UserAccount user)
	{
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void PurchsaeOrder_Load(object sender, EventArgs e)
	{
		try
		{
			tabPage1.AutoScroll = true;
			tabPage1.AutoScrollMargin = new Size(5, 5);
			tabPage1.AutoScrollMinSize = new Size(1200, 500);
			tabPage2.AutoScroll = true;
			tabPage2.AutoScrollMargin = new Size(5, 5);
			tabPage2.AutoScrollMinSize = new Size(1200, 700);
			formright = CommonServices.SetRightsValueInRightsObject(Conversion.ToString((object)base.Name));
			btnsave.Enabled = formright.DoHaveSaveRight;
			btnupdate.Enabled = formright.DoHaveUpdateRights;
			btnDelete.Enabled = formright.DoHaveCanDelete;
			btnPrintSlip.Enabled = formright.DoHavePrintRights;
			btnPrintCustomerWise.Enabled = formright.DoHavePrintRights;
			ChkPrint.Enabled = formright.DoHavePrintRights;
			chkPrintCustomerWise.Enabled = formright.DoHavePrintRights;
			chkPrintCustomerWise.Checked = formright.DoHavePrintRights;
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
			if (dtvar.Columns.Count == 0)
			{
				dtvar.Columns.Add("ItemAttributeVarientId", typeof(int));
				dtvar.Columns.Add("ItemAttribute");
				dtvar.Columns.Add("VarientEquivalent", typeof(decimal));
			}
			dtVehicleNos.Columns.Add("Id", typeof(int));
			dtVehicleNos.Columns.Add("VehicleNo");
			if (dtWh.Columns.Count == 0)
			{
				dtWh.Columns.Add("Id", typeof(int));
				dtWh.Columns.Add("WareHouseName");
			}
			if (dtJobLoat.Columns.Count == 0)
			{
				dtJobLoat.Columns.Add("Id", typeof(int));
				dtJobLoat.Columns.Add("JobLotDescription");
			}
			if (dtCity.Columns.Count == 0)
			{
				dtCity.Columns.Add("Id", typeof(int));
				dtCity.Columns.Add("CityName");
			}
			if (dtOrderItem.Columns.Count == 0)
			{
				dtOrderItem.Columns.Add("ItemId", typeof(int));
				dtOrderItem.Columns.Add("ItemName");
				dtOrderItem.Columns.Add("ItemCode");
				dtOrderItem.Columns.Add("ItemWeight", typeof(decimal));
				dtOrderItem.Columns.Add("OrderDetailId", typeof(int));
				dtOrderItem.Columns.Add("DocumentTypeId", typeof(int));
			}
			DocumentNoFill();
			ReferencePartyFill();
			BuildingStoreysFill();
			BuildingHeightFill();
			vehicleTypefill();
			SupplierNameFill();
			WareHouseFill();
			JobLotFill();
			BinCity();
			BindVehicles();
			btnupdate.Visible = false;
			btnsave.Visible = true;
			btnplus.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			DocDate.Focus();
			dtViewAttachments.Columns.Add("AttchmentPath");
			table.Columns.Add("Id", typeof(int));
			table.Columns.Add("SupplierCustomerId", typeof(int));
			table.Columns.Add("SupplierCustomer", typeof(string));
			table.Columns.Add("OrderId", typeof(int));
			table.Columns.Add("OrderDetailId", typeof(int));
			table.Columns.Add("OrderNo", typeof(int));
			table.Columns.Add("ReferencePartyId", typeof(int));
			table.Columns.Add("ReferencePartyName", typeof(string));
			table.Columns.Add("ReferencePartyCellNo", typeof(string));
			table.Columns.Add("ReferencePartyAddress", typeof(string));
			table.Columns.Add("BuildingStoreyId", typeof(int));
			table.Columns.Add("BuildingStorey", typeof(string));
			table.Columns.Add("BuildingHeightId", typeof(int));
			table.Columns.Add("BuildingHeight", typeof(string));
			table.Columns.Add("BuildingArea", typeof(string));
			table.Columns.Add("WareHouseId", typeof(int));
			table.Columns.Add("WareHouse", typeof(string));
			table.Columns.Add("ItemId", typeof(int));
			table.Columns.Add("ItemCode", typeof(string));
			table.Columns.Add("ItemName", typeof(string));
			table.Columns.Add("AttributeVarientId", typeof(int));
			table.Columns.Add("AttributeVarient", typeof(string));
			table.Columns.Add("VarientUnit", typeof(decimal));
			table.Columns.Add("JobLotId", typeof(int));
			table.Columns.Add("JobLot", typeof(string));
			table.Columns.Add("QTY", typeof(decimal));
			table.Columns.Add("AvailableQty", typeof(decimal));
			table.Columns.Add("ItemWeight", typeof(decimal));
			table.Columns.Add("NetWeight", typeof(decimal));
			table.Columns.Add("CityId", typeof(int));
			table.Columns.Add("CityName", typeof(string));
			table.Columns.Add("RemarksDetail", typeof(string));
			table.Columns.Add("IsFOC", typeof(bool));
			grd.DataSource = table;
			grd.RetrieveStructure();
			grdSettings();
			DocDate.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void dtFillForGridCombosAsWell()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReferenceParties obj = new ReferenceParties();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			dtRefParty = ReferenceParties.GetAll(obj);
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
			int code = InvDeliveryOrder.GenerateCode(new InvDeliveryOrder
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeId = 1853
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

	private void vehicleTypefill()
	{
		try
		{
			int VehicleId = Conversion.ToInt(CmbVehicleType.Value);
			DataTable dt = VehicleType.GetAll();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbVehicleType, "Id", "VehicleDescription", "Vehicle Type", true);
				if (VehicleId > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == VehicleId))
					{
						CmbVehicleType.Value = VehicleId;
					}
					else
					{
						((Control)(object)CmbVehicleType).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbVehicleType).Text = string.Empty;
				((UltraGridBase)CmbVehicleType).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BinCity()
	{
		try
		{
			int Id = Conversion.ToInt(combcityarea.Value);
			dtCity.Clear();
			dtCity = CommonServices.CityGetAllService();
			if (dtCity.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDLNew(dtCity, combcityarea, "Id", "CityName", "City Name ", true);
			if (Id > 0)
			{
				if (dtCity.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
				{
					combcityarea.Value = Id;
				}
				else
				{
					((Control)(object)combcityarea).Text = string.Empty;
				}
			}
		}
		catch (Exception)
		{
			MessageBox.Show("City Not Found");
		}
	}

	private void BindVehicles()
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		try
		{
			int Id = Conversion.ToInt(CmbVehicleNo.Value);
			dtVehicleNos.Clear();
			DataTable dtGrid = DefineVehicleWeight.ReadAll(new DefineVehicleWeight
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtGrid.Rows.Count > 0)
			{
				for (int i = 0; i < dtGrid.Rows.Count; i++)
				{
					dtVehicleNos.Rows.Add(Conversion.ToInt(dtGrid.Rows[i]["Id"]), dtGrid.Rows[i]["VehicleNo"]);
				}
				if (dtVehicleNos.Rows.Count > 0)
				{
					DDL.BindDDL(dtVehicleNos, CmbVehicleNo, "Id", "VehicleNo", "Vehicle No ", ZeroIndex: true);
					if (Id > 0)
					{
						if (dtVehicleNos.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
						{
							CmbVehicleNo.Value = Id;
						}
						else
						{
							((Control)(object)CmbVehicleNo).Text = string.Empty;
						}
					}
				}
				else
				{
					((Control)(object)CmbVehicleNo).Text = string.Empty;
					((UltraGridBase)CmbVehicleNo).DataSource = null;
				}
			}
			else
			{
				((Control)(object)CmbVehicleNo).Text = string.Empty;
				((UltraGridBase)CmbVehicleNo).DataSource = null;
			}
		}
		catch (Exception)
		{
			MessageBox.Show("VehicleNo Not Found");
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
				DDL.BindDDLNew(dt, CmbReferenceParty, "Id", "ReferencePartyName", "Reference Party Name", true);
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

	private void SupplierNameFill()
	{
		try
		{
			int CustomerId = Conversion.ToInt(CmbSupplierCustomer.Value);
			if (SubsidiaryAccountAllownOnVouchers)
			{
				dtsuppcus = CommonServices.GetVendorsAndCustomers(2);
				if (dtsuppcus.Rows.Count > 0)
				{
					DDL.BindDDL(dtsuppcus, CmbSupplierCustomer, "Id", "CompanyName", "Customer Name ", ZeroIndex: true);
					((UltraGridBase)CmbSupplierCustomer).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				}
				else
				{
					((Control)(object)CmbSupplierCustomer).Text = string.Empty;
				}
			}
			else
			{
				dtsuppcus = CommonServices.SupplierCustomerGetforComboServiceBind();
				if (dtsuppcus.Rows.Count > 0)
				{
					DDL.BindDDL(dtsuppcus, CmbSupplierCustomer, "Id", "CompanyName", "Customer Name", ZeroIndex: true);
				}
				else
				{
					((Control)(object)CmbSupplierCustomer).Text = string.Empty;
				}
			}
			if (CustomerId > 0)
			{
				if (dtsuppcus.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == CustomerId))
				{
					CmbSupplierCustomer.Value = CustomerId;
				}
				else
				{
					((Control)(object)CmbSupplierCustomer).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SaleOrderBind(int SupplierCustomerId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		try
		{
			DataTable dt = SaleOrder.SaleOrderIdandNoGetForSupplierCustomerId(new SaleOrder
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				SupplierCustomerId = SupplierCustomerId,
				DocumentTypeId = 1852
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbOrderNo, "OrderId", "OrderNo", "Sale Order No", ZeroIndex: false);
				return;
			}
			((Control)(object)CmbOrderNo).Text = string.Empty;
			((UltraGridBase)CmbOrderNo).DataSource = null;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemBindbyOrderId(int OrderId)
	{
		try
		{
			int ItId = Conversion.ToInt(CmbItemName.Value);
			dtOrderItem.Clear();
			DataTable dtitem = SaleOrder.GetSaleOrderDetailbyOrderId(OrderId);
			if (dtitem.Rows.Count > 0)
			{
				for (int i = 0; i < dtitem.Rows.Count; i++)
				{
					dtOrderItem.Rows.Add(dtitem.Rows[i]["OrderItemId"], dtitem.Rows[i]["ItemName"], dtitem.Rows[i]["ItemCode"], dtitem.Rows[i]["ItemWeight"], dtitem.Rows[i]["OrderDetailId"], dtitem.Rows[i]["DocumentTypeId"]);
				}
				if (dtOrderItem.Rows.Count <= 0)
				{
					return;
				}
				DDL.BindDDL(dtOrderItem, CmbItemName, "ItemId", "ItemName", "ItemName", ZeroIndex: true);
				((UltraGridBase)CmbItemName).DisplayLayout.Bands[0].Columns[1].Width = 300;
				((UltraGridBase)CmbItemName).DisplayLayout.Bands[0].Columns[5].Hidden = true;
				if (ItId > 0)
				{
					if (dtOrderItem.AsEnumerable().Any((DataRow row) => row.Field<int>("ItemId") == ItId))
					{
						CmbItemName.Value = ItId;
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
			MessageBox.Show(ex.Message);
		}
	}

	private void WareHouseFill()
	{
		try
		{
			int WareHouseId = Conversion.ToInt(CmbWareHouse.Value);
			dtWh = CommonServices.getActiveWareHouse();
			if (dtWh.Rows.Count > 0)
			{
				DDL.BindDDL(dtWh, CmbWareHouse, "Id", "WareHouseName", "WareHouseName", ZeroIndex: true);
				if (WareHouseId > 0)
				{
					if (dtWh.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == WareHouseId))
					{
						CmbWareHouse.Value = WareHouseId;
					}
					else
					{
						((Control)(object)CmbWareHouse).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbWareHouse).Text = string.Empty;
				((UltraGridBase)CmbWareHouse).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void JobLotFill()
	{
		try
		{
			int JobLotId = Conversion.ToInt(CmbJobLot.Value);
			dtJobLoat = CommonServices.JobLotGetAllService();
			if (dtJobLoat.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtJobLoat, CmbJobLot, "Id", "JobLotDescription", "JobLotDescription", true);
				if (JobLotId > 0)
				{
					if (dtJobLoat.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == JobLotId))
					{
						CmbJobLot.Value = JobLotId;
					}
					else
					{
						((Control)(object)CmbJobLot).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbJobLot).Text = string.Empty;
				((UltraGridBase)CmbJobLot).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void bindvarientunit(int ID, int OrderId)
	{
		try
		{
			int ItId = Conversion.ToInt(CmbAttributeVarient.Value);
			DataTable dtunit = ItemAttributeVarient.GetAllForCombo(UserAccount.OrganizationId, UserAccount.CompanyId, ID);
			if (dtunit.Rows.Count > 0)
			{
				dtvar.Clear();
				for (int i = 0; i < dtunit.Rows.Count; i++)
				{
					dtvar.Rows.Add(dtunit.Rows[i]["ItemAttributeVarientId"], dtunit.Rows[i]["ItemAttribute"], dtunit.Rows[i]["VarientEquivalent"]);
				}
				DDL.BindDDL(dtvar, CmbAttributeVarient, "ItemAttributeVarientId", "ItemAttribute", "Varient", ZeroIndex: true);
				if (ItId > 0)
				{
					if (dtvar.AsEnumerable().Any((DataRow row) => row.Field<int>("ItemAttributeVarientId") == ItId))
					{
						CmbAttributeVarient.Value = ItId;
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

	private void rdbtnItemName_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (dtOrderItem.Rows.Count > 0)
			{
				int ItemId = 0;
				if (Conversion.ToInt(CmbItemName.Value) > 0)
				{
					ItemId = Conversion.ToInt(CmbItemName.Value);
				}
				if (rdbtnItemName.Checked)
				{
					DDL.BindDDL(dtOrderItem, CmbItemName, "ItemId", "ItemName", "Item Name", ZeroIndex: true);
				}
				else
				{
					DDL.BindDDL(dtOrderItem, CmbItemName, "ItemId", "ItemCode", "Item Name", ZeroIndex: true);
				}
				CmbItemName.Value = ItemId;
				CmbItemName.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbItemName_Leave(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(CmbItemName.Value) > 0)
			{
				txtItemWeight.Text = Conversion.ToString(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["ItemWeight"].Value);
				bindvarientunit(Conversion.ToInt(CmbItemName.Value), Conversion.ToInt(CmbOrderNo.Value));
				AvailableStockGetForLabel();
				CalculateOrderBalanceQtyandWeight();
			}
			else
			{
				txtItemWeight.Text = 0.ToString();
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

	private void CalculateOrderBalanceQtyandWeight()
	{
	}

	private void CalculateWeight()
	{
		try
		{
			if (Conversion.ToInt(CmbAttributeVarient.Value) > 0 && txtqty.Text != string.Empty)
			{
				decimal Varient = Conversion.ToDecimal((object)txtVarientEquivalent.Text);
				decimal Qty = Conversion.ToDecimal((object)txtqty.Text);
				decimal ItemWeight = Conversion.ToDecimal((object)txtItemWeight.Text);
				txtNetWeight.Text = (Varient * Qty * ItemWeight).ToString();
			}
			else
			{
				txtNetWeight.Text = 0.ToString();
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
			CalculateWeight();
			CalculateOrderBalanceQtyandWeight();
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
			CalculateWeight();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtPackSize_TextChanged(object sender, EventArgs e)
	{
		CalculateWeight();
	}

	private void AvailableStockGetForLabel()
	{
		try
		{
			int OrganizationId = UserAccount.OrganizationId;
			int CompanyId = UserAccount.CompanyId;
			int ItemId = Conversion.ToInt(CmbItemName.Value);
			int wareId = Conversion.ToInt(CmbWareHouse.Value);
			int JobLotId = Conversion.ToInt(CmbJobLot.Value);
			int AttributeVarientId = Conversion.ToInt(CmbAttributeVarient.Value);
			if (ItemId > 0)
			{
				DataTable DtStock = GetAvgRatesAndStockInHand.GetStockInHandAndAvgRateFromEvaluationConcrete(OrganizationId, CompanyId, ItemId, Conversion.ToDateTime((object)DocDate.Value), wareId, JobLotId, AttributeVarientId);
				if (DtStock.Rows.Count > 0)
				{
					lblBalance.Text = Conversion.ToDecimal(DtStock.Rows[0]["QtyInHand"]).ToString();
				}
				else
				{
					lblBalance.Text = "0";
				}
			}
			else
			{
				lblBalance.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AvailableStockUpdateInGrid()
	{
		try
		{
			if (grd.RowCount <= 0)
			{
				return;
			}
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow item in rows)
			{
				item.BeginEdit();
				int OrganizationId = UserAccount.OrganizationId;
				int CompanyId = UserAccount.CompanyId;
				int ItemId = Conversion.ToInt((object)item.Cells["ItemId"].Value.ToString());
				int wareId = Conversion.ToInt((object)item.Cells["WarehouseId"].Value.ToString());
				int JobLotId = Conversion.ToInt((object)item.Cells["JobLotId"].Value.ToString());
				int AttributeVarientId = Conversion.ToInt((object)item.Cells["AttributeVarientId"].Value.ToString());
				if (ItemId > 0 || wareId > 0)
				{
					DataTable DtStock = GetAvgRatesAndStockInHand.GetStockInHandAndAvgRateFromEvaluationConcrete(OrganizationId, CompanyId, ItemId, Conversion.ToDateTime((object)DocDate.Value), wareId, JobLotId, AttributeVarientId);
					if (DtStock.Rows.Count > 0)
					{
						item.Cells["AvailableQty"].Value = Conversion.ToDecimal(DtStock.Rows[0]["QtyInHand"]).ToString();
					}
					else
					{
						item.Cells["AvailableQty"].Value = 0.ToString();
					}
				}
				else
				{
					item.Cells["AvailableQty"].Value = 0.ToString();
				}
				item.BeginEdit();
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
			AvailableStockUpdateInGrid();
			AvailableStockGetForLabel();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbSupplierCustomer_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(CmbSupplierCustomer.Value) > 0)
			{
				SaleOrderBind(Conversion.ToInt(CmbSupplierCustomer.Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbOrderNo_TextChanged(object sender, EventArgs e)
	{
		try
		{
			ItemBindbyOrderId(Conversion.ToInt(CmbOrderNo.Value));
			CalculateOrderBalanceQtyandWeight();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbJobLot_Leave(object sender, EventArgs e)
	{
		try
		{
			AvailableStockGetForLabel();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbSupplierCustomer_Leave(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(CmbSupplierCustomer.Value) > 0)
			{
				SaleOrderBind(Conversion.ToInt(CmbSupplierCustomer.Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbPackUom_TextChanged(object sender, EventArgs e)
	{
		try
		{
			double Equivalent = 0.0;
			if (((UltraGridBase)CmbAttributeVarient).ActiveRow != null && Conversion.ToInt(CmbAttributeVarient.Value) > 0)
			{
				Equivalent = Conversion.ToDouble(((UltraDropDownBase)CmbAttributeVarient).SelectedRow.Cells["VarientEquivalent"].Value);
				txtVarientEquivalent.Text = Equivalent.ToString();
			}
			else
			{
				txtVarientEquivalent.Text = Equivalent.ToString();
			}
			CalculateOrderBalanceQtyandWeight();
			CalculateWeight();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbJobLot_TextChanged(object sender, EventArgs e)
	{
		try
		{
			AvailableStockGetForLabel();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbOrderNo_Leave(object sender, EventArgs e)
	{
		try
		{
			AvailableStockGetForLabel();
			ItemBindbyOrderId(Conversion.ToInt(CmbOrderNo.Value));
			CalculateOrderBalanceQtyandWeight();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbWareHouse_TextChanged(object sender, EventArgs e)
	{
		try
		{
			AvailableStockGetForLabel();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtVehicleNo_TextChanged(object sender, EventArgs e)
	{
	}

	private bool FormValidation()
	{
		if (txtdocno.Text.Trim() == string.Empty || txtdocno.Text.Trim() == "0")
		{
			MessageBox.Show("DocNo Field is Required");
			txtdocno.Focus();
			return false;
		}
		if (((UltraGridBase)CmbVehicleType).ActiveRow == null)
		{
			MessageBox.Show("VehicleType Field is Required");
			CmbVehicleType.Focus();
			return false;
		}
		if (((Control)(object)CmbVehicleNo).Text.Trim() == string.Empty || ((Control)(object)CmbVehicleNo).Text.Trim() == "0")
		{
			MessageBox.Show("VehicleNo Field is Required");
			CmbVehicleNo.Focus();
			return false;
		}
		return true;
	}

	private bool FormValidationDetail()
	{
		if (((UltraGridBase)CmbSupplierCustomer).ActiveRow == null || Conversion.ToInt(CmbSupplierCustomer.Value) == 0)
		{
			MessageBox.Show("Customer field is required");
			CmbSupplierCustomer.Focus();
			return false;
		}
		if (((UltraGridBase)CmbOrderNo).ActiveRow == null || Conversion.ToInt(CmbOrderNo.Value) == 0)
		{
			MessageBox.Show("OrderNo field is required");
			CmbOrderNo.Focus();
			return false;
		}
		if (((Control)(object)CmbReferenceParty).Text.Trim() == "" || ((Control)(object)CmbReferenceParty).Text.Trim() == null)
		{
			MessageBox.Show("Ref Party Name Field is Required");
			CmbReferenceParty.Focus();
			return false;
		}
		if (txtRefPartyCellNo.Text.Trim() == "" || txtRefPartyCellNo.Text.Trim() == null)
		{
			MessageBox.Show("Ref Party CellNo Field is Required");
			txtRefPartyCellNo.Focus();
			return false;
		}
		if (txtRefPartyAddress.Text.Trim() == "" || txtRefPartyAddress.Text.Trim() == null)
		{
			MessageBox.Show("Ref Party Address Field is Required");
			txtRefPartyAddress.Focus();
			return false;
		}
		if (((UltraGridBase)CmbWareHouse).ActiveRow == null || Conversion.ToInt(CmbWareHouse.Value) == 0)
		{
			MessageBox.Show("WareHouseName field is required");
			CmbWareHouse.Focus();
			return false;
		}
		if (((UltraGridBase)CmbItemName).ActiveRow == null || Conversion.ToInt(CmbItemName.Value) == 0)
		{
			MessageBox.Show("ItemName field is required");
			CmbItemName.Focus();
			return false;
		}
		if (((UltraGridBase)CmbAttributeVarient).ActiveRow == null || Conversion.ToInt(CmbAttributeVarient.Value) == 0)
		{
			MessageBox.Show("AttributeVarient field is required");
			CmbAttributeVarient.Focus();
			return false;
		}
		if (((UltraGridBase)CmbJobLot).ActiveRow == null || Conversion.ToInt(CmbJobLot.Value) == 0)
		{
			MessageBox.Show("JobLot field is required");
			CmbJobLot.Focus();
			return false;
		}
		if (Conversion.ToDecimal((object)txtqty.Text) == 0m)
		{
			MessageBox.Show("ItemQty field is required");
			txtqty.Focus();
			return false;
		}
		if (Conversion.ToDecimal((object)txtItemWeight.Text) == 0m)
		{
			MessageBox.Show("Item Weight field is required");
			txtItemWeight.Focus();
			return false;
		}
		if (Conversion.ToDecimal((object)txtNetWeight.Text) == 0m)
		{
			MessageBox.Show("NetWeight field is required");
			txtNetWeight.Focus();
			return false;
		}
		if (((UltraGridBase)combcityarea).ActiveRow == null || Conversion.ToInt(combcityarea.Value) == 0)
		{
			MessageBox.Show("City field is required");
			combcityarea.Focus();
			return false;
		}
		return true;
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		Reset();
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			ReferencePartyFill();
			BuildingStoreysFill();
			BuildingHeightFill();
			WareHouseFill();
			JobLotFill();
			vehicleTypefill();
			SupplierNameFill();
			BinCity();
			BindVehicles();
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
			dtSaleOrderIds = "";
			dtSaleOrderMainIds = "";
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			lstRemoveRecord.Clear();
			Id = 0;
			IsApproved = false;
			chkisFreeOfCost.Checked = false;
			txtReferenceNoHeader.Text = string.Empty;
			((Control)(object)CmbVehicleNo).Text = string.Empty;
			((Control)(object)CmbVehicleType).Text = string.Empty;
			btnsave.Visible = true;
			btnupdate.Visible = false;
			btnDelete.Visible = false;
			DocDate.Focus();
			DocumentNoFill();
			((Control)(object)CmbItemName).Text = string.Empty;
			((Control)(object)CmbAttributeVarient).Text = string.Empty;
			txtVarientEquivalent.Text = string.Empty;
			txtqty.Text = string.Empty;
			txtDistance.Text = string.Empty;
			txtItemWeight.Text = string.Empty;
			txtNetWeight.Text = "0";
			txtBalQty.Text = "0";
			txtBalWeight.Text = "0";
			((Control)(object)combcityarea).Text = string.Empty;
			txtRemarksHeader.Text = string.Empty;
			btnplus.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			((Control)(object)Cmbstoreys).Text = string.Empty;
			((Control)(object)Cmbheight).Text = string.Empty;
			txtArea.Text = string.Empty;
			SupplierNameFill();
			table.Rows.Clear();
			grd.DataSource = table;
			grd.RetrieveStructure();
			grdSettings();
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
			chkisFreeOfCost.Checked = false;
			((Control)(object)CmbOrderNo).Text = string.Empty;
			((Control)(object)CmbWareHouse).Text = string.Empty;
			((Control)(object)CmbItemName).Text = string.Empty;
			((Control)(object)CmbAttributeVarient).Text = string.Empty;
			txtVarientEquivalent.Text = string.Empty;
			((Control)(object)CmbJobLot).Text = string.Empty;
			txtqty.Text = string.Empty;
			txtItemWeight.Text = string.Empty;
			txtNetWeight.Text = "0";
			txtBalQty.Text = "0";
			txtBalWeight.Text = "0";
			txtremarksdetail.Text = string.Empty;
			btnplus.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			((UltraGridBase)CmbOrderNo).DataSource = null;
			((UltraGridBase)CmbItemName).DataSource = null;
			SupplierNameFill();
			CmbSupplierCustomer.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeliveryOrder_FormClosing(object sender, FormClosingEventArgs e)
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
				table.Rows.Add(0, Conversion.ToInt(CmbSupplierCustomer.Value), Conversion.ToString((object)((Control)(object)CmbSupplierCustomer).Text.Trim()), Conversion.ToInt(CmbOrderNo.Value), Conversion.ToInt(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["OrderDetailId"].Value), Conversion.ToInt((object)((Control)(object)CmbOrderNo).Text.Trim()), Conversion.ToInt(CmbReferenceParty.Value), Conversion.ToString((object)((Control)(object)CmbReferenceParty).Text.Trim()), Conversion.ToString((object)txtRefPartyCellNo.Text.Trim()), Conversion.ToString((object)txtRefPartyAddress.Text.Trim()), Conversion.ToInt(Cmbstoreys.Value), Conversion.ToString((object)((Control)(object)Cmbstoreys).Text), Conversion.ToInt(Cmbheight.Value), Conversion.ToString((object)((Control)(object)Cmbheight).Text), Conversion.ToString((object)txtArea.Text), Conversion.ToInt(CmbWareHouse.Value), Conversion.ToString((object)((Control)(object)CmbWareHouse).Text), Conversion.ToInt(CmbItemName.Value), Conversion.ToString(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["ItemCode"].Value), Conversion.ToString(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["ItemName"].Value), Conversion.ToInt(CmbAttributeVarient.Value), Conversion.ToString((object)((Control)(object)CmbAttributeVarient).Text.Trim()), Conversion.ToDecimal((object)txtVarientEquivalent.Text), Conversion.ToInt(CmbJobLot.Value), Conversion.ToString((object)((Control)(object)CmbJobLot).Text), Conversion.ToDecimal((object)txtqty.Text.Trim()), Conversion.ToDecimal((object)txtqty.Text.Trim()), Conversion.ToDecimal((object)txtItemWeight.Text.Trim()), Conversion.ToDecimal((object)txtNetWeight.Text.Trim()), Conversion.ToInt(combcityarea.Value), Conversion.ToString((object)((Control)(object)combcityarea).Text), Conversion.ToString((object)txtremarksdetail.Text.Trim()), Conversion.ToBool((object)chkisFreeOfCost.Checked));
				grd.DataSource = table;
				grd.RetrieveStructure();
				grdSettings();
				ResetDetail();
				CmbSupplierCustomer.Focus();
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
				DataTable dtsup = new DataTable();
				dtsup.Columns.Add("Id", typeof(int));
				dtsup.Columns.Add("CompanyName");
				dtsup.Rows.Add(item.Cells["SupplierCustomerId"].Value, item.Cells["SupplierCustomerId"].Text);
				DDL.BindDDL(dtsup, CmbSupplierCustomer, "Id", "CompanyName", "CustomerName", ZeroIndex: true);
				((UltraGridBase)CmbSupplierCustomer).Rows[1].Activate();
				DataTable dtOrder = new DataTable();
				dtOrder.Columns.Add("OrderId", typeof(int));
				dtOrder.Columns.Add("OrderNo", typeof(string));
				dtOrder.Rows.Add(item.Cells["OrderId"].Value, item.Cells["OrderNo"].Text);
				DDL.BindDDL(dtOrder, CmbOrderNo, "OrderId", "OrderNo", "OrderNo", ZeroIndex: true);
				((UltraGridBase)CmbOrderNo).Rows[1].Activate();
				Cmbstoreys.Value = Conversion.ToInt(item.Cells["BuildingStoreyId"].Value).ToString();
				Cmbheight.Value = Conversion.ToInt(item.Cells["BuildingHeightId"].Value).ToString();
				txtArea.Text = Conversion.ToString(item.Cells["BuildingArea"].Value);
				((Control)(object)CmbReferenceParty).Text = Conversion.ToString(item.Cells["ReferencePartyName"].Value);
				txtRefPartyCellNo.Text = Conversion.ToString(item.Cells["ReferencePartyCellNo"].Value);
				txtRefPartyAddress.Text = Conversion.ToString(item.Cells["ReferencePartyAddress"].Value);
				CmbWareHouse.Value = Conversion.ToInt(item.Cells["WareHouseId"].Value).ToString();
				dtOrderItem.Clear();
				dtOrderItem.Rows.Add(item.Cells["ItemId"].Value, item.Cells["ItemName"].Text, item.Cells["ItemCode"].Value, item.Cells["ItemWeight"].Value, item.Cells["OrderDetailId"].Value, 1853);
				DDL.BindDDL(dtOrderItem, CmbItemName, "ItemId", "ItemName", "Item Name", ZeroIndex: true);
				((UltraGridBase)CmbItemName).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				((UltraGridBase)CmbItemName).DisplayLayout.Bands[0].Columns[4].Hidden = true;
				((UltraGridBase)CmbItemName).DisplayLayout.Bands[0].Columns[5].Hidden = true;
				((UltraGridBase)CmbItemName).Rows[1].Activate();
				bindvarientunit(Conversion.ToInt(CmbItemName.Value), Conversion.ToInt(CmbOrderNo.Value));
				CmbAttributeVarient.Value = Conversion.ToInt(item.Cells["AttributeVarientId"].Value).ToString();
				((Control)(object)CmbAttributeVarient).Text = Conversion.ToString((object)item.Cells["AttributeVarient"].Text).ToString();
				txtVarientEquivalent.Text = Conversion.ToDecimal((object)item.Cells["VarientUnit"].Text).ToString();
				CmbJobLot.Value = Conversion.ToInt(item.Cells["JobLotId"].Value).ToString();
				txtqty.Text = Conversion.ToDecimal(item.Cells["QTY"].Value).ToString();
				txtItemWeight.Text = Conversion.ToDecimal(item.Cells["ItemWeight"].Value).ToString();
				txtNetWeight.Text = Conversion.ToDecimal(item.Cells["NetWeight"].Value).ToString();
				combcityarea.Value = Conversion.ToInt(item.Cells["CityId"].Value).ToString();
				txtremarksdetail.Text = Conversion.ToString(item.Cells["RemarksDetail"].Value).ToString();
				chkisFreeOfCost.Checked = Conversion.ToBool(item.Cells["IsFOC"].Value);
				btnplus.Visible = false;
				btnUpdateDetail.Visible = true;
				btnCancelUpdateDetial.Visible = true;
				CmbReferenceParty.Focus();
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
				table.Rows[updateDetailIndex]["SupplierCustomerId"] = Conversion.ToInt(CmbSupplierCustomer.Value);
				table.Rows[updateDetailIndex]["SupplierCustomer"] = Conversion.ToString((object)((Control)(object)CmbSupplierCustomer).Text);
				table.Rows[updateDetailIndex]["OrderId"] = Conversion.ToInt(((UltraDropDownBase)CmbOrderNo).SelectedRow.Cells["OrderId"].Value);
				table.Rows[updateDetailIndex]["OrderNo"] = Conversion.ToInt(((UltraDropDownBase)CmbOrderNo).SelectedRow.Cells["OrderNo"].Value);
				table.Rows[updateDetailIndex]["ReferencePartyName"] = Conversion.ToString((object)((Control)(object)CmbReferenceParty).Text);
				table.Rows[updateDetailIndex]["ReferencePartyCellNo"] = Conversion.ToString((object)txtRefPartyCellNo.Text);
				table.Rows[updateDetailIndex]["ReferencePartyAddress"] = Conversion.ToString((object)txtRefPartyAddress.Text);
				table.Rows[updateDetailIndex]["BuildingStoreyId"] = Conversion.ToInt(Cmbstoreys.Value);
				table.Rows[updateDetailIndex]["BuildingStorey"] = Conversion.ToString((object)((Control)(object)Cmbstoreys).Text);
				table.Rows[updateDetailIndex]["BuildingHeightId"] = Conversion.ToInt(Cmbheight.Value);
				table.Rows[updateDetailIndex]["BuildingHeight"] = Conversion.ToString((object)((Control)(object)Cmbheight).Text);
				table.Rows[updateDetailIndex]["BuildingArea"] = Conversion.ToString((object)txtArea.Text);
				table.Rows[updateDetailIndex]["WareHouseId"] = Conversion.ToInt(CmbWareHouse.Value);
				table.Rows[updateDetailIndex]["WareHouse"] = Conversion.ToString((object)((Control)(object)CmbWareHouse).Text);
				table.Rows[updateDetailIndex]["OrderDetailId"] = Conversion.ToInt(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["OrderDetailId"].Value);
				table.Rows[updateDetailIndex]["ItemId"] = Conversion.ToInt(CmbItemName.Value);
				table.Rows[updateDetailIndex]["ItemCode"] = Conversion.ToString(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["ItemCode"].Value);
				table.Rows[updateDetailIndex]["ItemName"] = Conversion.ToString(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["ItemName"].Value);
				table.Rows[updateDetailIndex]["AttributeVarientId"] = Conversion.ToInt(CmbAttributeVarient.Value);
				table.Rows[updateDetailIndex]["AttributeVarient"] = Conversion.ToString((object)((Control)(object)CmbAttributeVarient).Text);
				table.Rows[updateDetailIndex]["VarientUnit"] = Conversion.ToDecimal((object)txtVarientEquivalent.Text);
				table.Rows[updateDetailIndex]["JobLotId"] = Conversion.ToInt(CmbJobLot.Value);
				table.Rows[updateDetailIndex]["JobLot"] = Conversion.ToString((object)((Control)(object)CmbJobLot).Text);
				table.Rows[updateDetailIndex]["QTY"] = Conversion.ToDecimal((object)txtqty.Text);
				table.Rows[updateDetailIndex]["ItemWeight"] = Conversion.ToDecimal((object)txtItemWeight.Text);
				table.Rows[updateDetailIndex]["NetWeight"] = Conversion.ToDecimal((object)txtNetWeight.Text);
				table.Rows[updateDetailIndex]["CityId"] = Conversion.ToInt(combcityarea.Value);
				table.Rows[updateDetailIndex]["CityName"] = Conversion.ToString((object)((Control)(object)combcityarea).Text);
				table.Rows[updateDetailIndex]["RemarksDetail"] = Conversion.ToString((object)txtremarksdetail.Text);
				table.Rows[updateDetailIndex]["IsFOC"] = Conversion.ToBool((object)chkisFreeOfCost.Checked);
				grd.DataSource = table;
				btnplus.Visible = true;
				btnUpdateDetail.Visible = false;
				btnCancelUpdateDetial.Visible = false;
				ResetDetail();
				SupplierNameFill();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridComboFill()
	{
		try
		{
			grd.RootTable.Columns["SupplierCustomerId"].ColumnType = (ColumnType)0;
			grd.RootTable.Columns["SupplierCustomerId"].HasValueList = true;
			grd.RootTable.Columns["SupplierCustomerId"].LimitToList = true;
			grd.RootTable.Columns["SupplierCustomerId"].EditType = (EditType)4;
			grd.RootTable.Columns["SupplierCustomerId"].Caption = "Customer Name";
			grd.RootTable.Columns["SupplierCustomerId"].ValueList.PopulateValueList((IEnumerable)dtsuppcus.DefaultView, "Id", "CompanyName");
			grd.RootTable.Columns["SupplierCustomerId"].Width = 150;
			grd.RootTable.Columns["WareHouseId"].ColumnType = (ColumnType)0;
			grd.RootTable.Columns["WareHouseId"].HasValueList = true;
			grd.RootTable.Columns["WareHouseId"].LimitToList = true;
			grd.RootTable.Columns["WareHouseId"].EditType = (EditType)4;
			grd.RootTable.Columns["WareHouseId"].Caption = "WareHouseName";
			grd.RootTable.Columns["WareHouseId"].ValueList.PopulateValueList((IEnumerable)dtWh.DefaultView, "Id", "WareHouseName");
			grd.RootTable.Columns["WareHouseId"].Width = 150;
			grd.RootTable.Columns["JobLotId"].ColumnType = (ColumnType)0;
			grd.RootTable.Columns["JobLotId"].HasValueList = true;
			grd.RootTable.Columns["JobLotId"].LimitToList = true;
			grd.RootTable.Columns["JobLotId"].EditType = (EditType)4;
			grd.RootTable.Columns["JobLotId"].Caption = "JobLotDescription";
			grd.RootTable.Columns["JobLotId"].ValueList.PopulateValueList((IEnumerable)dtJobLoat.DefaultView, "Id", "JobLotDescription");
			grd.RootTable.Columns["JobLotId"].Width = 150;
			grd.RootTable.Columns["CityId"].ColumnType = (ColumnType)0;
			grd.RootTable.Columns["CityId"].HasValueList = true;
			grd.RootTable.Columns["CityId"].LimitToList = true;
			grd.RootTable.Columns["CityId"].EditType = (EditType)4;
			grd.RootTable.Columns["CityId"].Caption = "City Name";
			grd.RootTable.Columns["CityId"].ValueList.PopulateValueList((IEnumerable)dtCity.DefaultView, "Id", "CityName");
			grd.RootTable.Columns["CityId"].Width = 150;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettings()
	{
		//IL_05be: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05eb: Expected O, but got Unknown
		try
		{
			GridComboFill();
			grd.AllowDelete = (InheritableBoolean)2;
			grd.ColumnAutoResize = false;
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["SupplierCustomer"].Visible = false;
			grd.RootTable.Columns["OrderId"].Visible = false;
			grd.RootTable.Columns["ItemId"].Visible = false;
			grd.RootTable.Columns["OrderDetailId"].Visible = false;
			grd.RootTable.Columns["WareHouse"].Visible = false;
			grd.RootTable.Columns["AttributeVarientId"].Visible = false;
			grd.RootTable.Columns["JobLot"].Visible = false;
			grd.RootTable.Columns["CityName"].Visible = false;
			grd.RootTable.Columns["ReferencePartyId"].Visible = false;
			grd.RootTable.Columns["BuildingStoreyId"].Visible = false;
			grd.RootTable.Columns["BuildingHeightId"].Visible = false;
			grd.RootTable.Columns["Id"].EditType = (EditType)0;
			grd.RootTable.Columns["SupplierCustomerId"].EditType = (EditType)0;
			grd.RootTable.Columns["SupplierCustomer"].EditType = (EditType)0;
			grd.RootTable.Columns["OrderId"].EditType = (EditType)0;
			grd.RootTable.Columns["OrderDetailId"].EditType = (EditType)0;
			grd.RootTable.Columns["OrderNo"].EditType = (EditType)0;
			grd.RootTable.Columns["ReferencePartyId"].EditType = (EditType)0;
			grd.RootTable.Columns["ReferencePartyName"].EditType = (EditType)1;
			grd.RootTable.Columns["ReferencePartyCellNo"].EditType = (EditType)1;
			grd.RootTable.Columns["ReferencePartyAddress"].EditType = (EditType)1;
			grd.RootTable.Columns["BuildingStoreyId"].EditType = (EditType)0;
			grd.RootTable.Columns["BuildingStorey"].EditType = (EditType)0;
			grd.RootTable.Columns["BuildingHeightId"].EditType = (EditType)0;
			grd.RootTable.Columns["BuildingHeight"].EditType = (EditType)0;
			grd.RootTable.Columns["BuildingArea"].EditType = (EditType)0;
			grd.RootTable.Columns["WareHouseId"].EditType = (EditType)4;
			grd.RootTable.Columns["WareHouse"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemId"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemCode"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemName"].EditType = (EditType)0;
			grd.RootTable.Columns["AttributeVarientId"].EditType = (EditType)0;
			grd.RootTable.Columns["AttributeVarient"].EditType = (EditType)0;
			grd.RootTable.Columns["VarientUnit"].EditType = (EditType)0;
			grd.RootTable.Columns["JobLotId"].EditType = (EditType)4;
			grd.RootTable.Columns["JobLot"].EditType = (EditType)0;
			grd.RootTable.Columns["QTY"].EditType = (EditType)1;
			grd.RootTable.Columns["AvailableQty"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemWeight"].EditType = (EditType)0;
			grd.RootTable.Columns["NetWeight"].EditType = (EditType)0;
			grd.RootTable.Columns["RemarksDetail"].EditType = (EditType)1;
			grd.RootTable.Columns["CityId"].EditType = (EditType)4;
			grd.RootTable.Columns["CityName"].EditType = (EditType)0;
			grd.RootTable.Columns["IsFOC"].EditType = (EditType)8;
			new GridEXColumn();
			GridEXFormatCondition fc = new GridEXFormatCondition(grd.RootTable.Columns["IsFOC"], (ConditionOperator)0, (object)true);
			fc.FormatStyle.ForeColor = Color.Red;
			grd.RootTable.FormatConditions.Add(fc);
			grd.RootTable.Columns["VarientUnit"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["VarientUnit"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["VarientUnit"].FormatString = "##,#.##";
			grd.RootTable.Columns["VarientUnit"].TotalFormatString = "##,#.##";
			grd.RootTable.Columns["QTY"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["QTY"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["QTY"].FormatString = "##,#.##";
			grd.RootTable.Columns["QTY"].TotalFormatString = "##,#.##";
			grd.RootTable.Columns["AvailableQty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["AvailableQty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["AvailableQty"].FormatString = "##,#.##";
			grd.RootTable.Columns["AvailableQty"].TotalFormatString = "##,#.##";
			grd.RootTable.Columns["ItemWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ItemWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemWeight"].FormatString = "##,#.##";
			grd.RootTable.Columns["ItemWeight"].TotalFormatString = "##,#.##";
			grd.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["NetWeight"].FormatString = "##,#.##";
			grd.RootTable.Columns["NetWeight"].TotalFormatString = "##,#.##";
			CommonServices.GridAutoAdjustmentNew(grd);
			grd.RootTable.Columns.Add("Delete");
			grd.RootTable.Columns["Delete"].Key = "Delete";
			grd.RootTable.Columns["Delete"].Caption = "X";
			grd.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Delete"].ButtonText = "X";
			grd.RootTable.Columns["Delete"].Width = 20;
			grd.RootTable.Columns["Delete"].Position = 0;
			grd.RootTable.Columns.Add("Add");
			grd.RootTable.Columns["Add"].Key = "Add";
			grd.RootTable.Columns["Add"].Caption = "+";
			grd.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Add"].ButtonText = "+";
			grd.RootTable.Columns["Add"].Width = 20;
			grd.RootTable.Columns["Add"].Position = 1;
			grd.RootTable.Columns.Add("Edit");
			grd.RootTable.Columns["Edit"].Key = "Edit";
			grd.RootTable.Columns["Edit"].Caption = "Edit";
			grd.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Edit"].ButtonText = "Edit";
			grd.RootTable.Columns["Edit"].Width = 50;
			grd.RootTable.Columns["Edit"].Position = 2;
			grd.FrozenColumns = 3;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0385: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Expected O, but got Unknown
		//IL_0366: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			if (e.Column.Key == "Edit")
			{
				grd_DoubleClick(null, null);
			}
			if (e.Column.Key == "Delete")
			{
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					InvDeliveryOrderdetail vd = new InvDeliveryOrderdetail();
					vd.Id = Conversion.ToInt(r.Cells["Id"].Value);
					vd.SaleOrderId = Conversion.ToInt(r.Cells["OrderId"].Value);
					vd.SaleOrderDetailId = Conversion.ToInt(r.Cells["OrderDetailId"].Value);
					vd.SupplierCustomerId = Conversion.ToInt(r.Cells["SupplierCustomerId"].Value);
					vd.BuildingHeightId = Conversion.ToInt(r.Cells["BuildingHeightId"].Value);
					vd.BuildingStoreyId = Conversion.ToInt(r.Cells["BuildingStoreyId"].Value);
					vd.BuildingArea = Conversion.ToString(r.Cells["BuildingArea"].Value);
					vd.ReferencePartyId = Conversion.ToInt(r.Cells["ReferencePartyId"].Value);
					vd.ReferencePartyName = Conversion.ToString(r.Cells["ReferencePartyName"].Value);
					vd.ReferencePartyCellNo = Conversion.ToString(r.Cells["ReferencePartyCellNo"].Value);
					vd.ReferencePartyAddress = Conversion.ToString(r.Cells["ReferencePartyAddress"].Value);
					vd.WarehouseId = Conversion.ToInt(r.Cells["WareHouseId"].Value);
					vd.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					vd.ItemAttributeVarientId = Conversion.ToInt(r.Cells["AttributeVarientId"].Value);
					vd.VarientEquivalent = Conversion.ToDecimal(r.Cells["VarientUnit"].Value);
					vd.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
					vd.DoQty = Conversion.ToDecimal(r.Cells["QTY"].Value);
					vd.ItemWeight = Conversion.ToDecimal(r.Cells["ItemWeight"].Value);
					vd.DoNetWeight = Conversion.ToDecimal(r.Cells["DoNetWeight"].Value);
					vd.CityId = Conversion.ToInt(r.Cells["CityId"].Value);
					vd.RemarksDetail = Conversion.ToString(r.Cells["RemarksDetail"].Value);
					vd.ActionTypeId = 3;
					lstRemoveRecord.Add(vd);
					table.Rows[r.RowIndex].Delete();
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
				GetSaleOrderIds();
			}
			if (e.Column.Key == "Add")
			{
				DataRow dr = table.Rows[r.RowIndex];
				table.Rows.Add(dr.ItemArray);
				table.Rows[table.Rows.Count - 1]["Id"] = 0;
				grd.DataSource = table;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grd.UpdateData();
			GridEXRow item = grd.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				if (e.Column.Key == "QTY")
				{
					if (item.Cells["QTY"].Value.ToString() != string.Empty && item.Cells["VarientUnit"].Value.ToString() != string.Empty && item.Cells["ItemWeight"].Value.ToString() != string.Empty)
					{
						decimal num = Conversion.ToDecimal((object)item.Cells["QTY"].Value.ToString());
						decimal VarientUnit = Conversion.ToDecimal((object)item.Cells["VarientUnit"].Value.ToString());
						decimal ItemWeight = Conversion.ToDecimal((object)item.Cells["ItemWeight"].Value.ToString());
						decimal NetWeight = num * VarientUnit * ItemWeight;
						item.Cells["NetWeight"].Value = NetWeight;
					}
					else
					{
						item.Cells["NetWeight"].Value = 0;
					}
				}
				if (e.Column.Key == "WareHouseId")
				{
					item.Cells["WareHouse"].Value = Conversion.ToString((object)item.Cells["WareHouseId"].Text);
					int OrganizationId2 = UserAccount.OrganizationId;
					int CompanyId2 = UserAccount.CompanyId;
					int ItemId2 = Conversion.ToInt((object)item.Cells["ItemId"].Value.ToString());
					int wareId2 = Conversion.ToInt((object)item.Cells["WareHouseId"].Value.ToString());
					int JobLotId2 = Conversion.ToInt((object)item.Cells["JobLotId"].Value.ToString());
					int AttributeVarientId2 = Conversion.ToInt((object)item.Cells["AttributeVarientId"].Value.ToString());
					if (ItemId2 > 0 || wareId2 > 0)
					{
						DataTable DtStock2 = GetAvgRatesAndStockInHand.GetStockInHandAndAvgRateFromEvaluationConcrete(OrganizationId2, CompanyId2, ItemId2, Conversion.ToDateTime((object)DocDate.Value), wareId2, JobLotId2, AttributeVarientId2);
						if (DtStock2.Rows.Count > 0)
						{
							item.Cells["AvailableQty"].Value = Conversion.ToDecimal(DtStock2.Rows[0]["QtyInHand"]).ToString();
						}
						else
						{
							item.Cells["AvailableQty"].Value = 0.ToString();
						}
					}
					else
					{
						item.Cells["AvailableQty"].Value = 0.ToString();
					}
				}
				if (e.Column.Key == "JobLotId")
				{
					item.Cells["JobLot"].Value = Conversion.ToString((object)item.Cells["JobLotId"].Text);
					int OrganizationId = UserAccount.OrganizationId;
					int CompanyId = UserAccount.CompanyId;
					int ItemId = Conversion.ToInt((object)item.Cells["ItemId"].Value.ToString());
					int wareId = Conversion.ToInt((object)item.Cells["WareHouseId"].Value.ToString());
					int JobLotId = Conversion.ToInt((object)item.Cells["JobLotId"].Value.ToString());
					int AttributeVarientId = Conversion.ToInt((object)item.Cells["AttributeVarientId"].Value.ToString());
					if (ItemId > 0 || wareId > 0)
					{
						DataTable DtStock = GetAvgRatesAndStockInHand.GetStockInHandAndAvgRateFromEvaluationConcrete(OrganizationId, CompanyId, ItemId, Conversion.ToDateTime((object)DocDate.Value), wareId, JobLotId, AttributeVarientId);
						if (DtStock.Rows.Count > 0)
						{
							item.Cells["AvailableQty"].Value = Conversion.ToDecimal(DtStock.Rows[0]["QtyInHand"]).ToString();
						}
						else
						{
							item.Cells["AvailableQty"].Value = 0.ToString();
						}
					}
					else
					{
						item.Cells["AvailableQty"].Value = 0.ToString();
					}
				}
				if (e.Column.Key == "CityId")
				{
					item.Cells["CityName"].Value = Conversion.ToString((object)item.Cells["CityId"].Text);
				}
			}
			grd.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelUpdateDetial_Click(object sender, EventArgs e)
	{
		txtRefDocTypeId.Text = string.Empty;
		txtDocNoId.Text = string.Empty;
		txtSubNoId.Text = string.Empty;
		btnplus.Visible = true;
		btnUpdateDetail.Visible = false;
		btnCancelUpdateDetial.Visible = false;
		((Control)(object)Cmbstoreys).Text = string.Empty;
		((Control)(object)Cmbheight).Text = string.Empty;
		txtArea.Text = string.Empty;
		SupplierNameFill();
	}

	private void grd_Click(object sender, EventArgs e)
	{
		try
		{
			if (grd.CurrentColumn != null && grd.CurrentColumn.Key == "IsFOC")
			{
				grd.UpdateData();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		//IL_0316: Unknown result type (might be due to invalid IL or missing references)
		//IL_031d: Expected O, but got Unknown
		//IL_0ae5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aec: Expected O, but got Unknown
		try
		{
			if (grd.GetRows().Count() == 0)
			{
				throw new Exception("Grid Record Not Found");
			}
			if (!FormValidation())
			{
				return;
			}
			InvDeliveryOrder po = new InvDeliveryOrder();
			if (Id > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				po.Id = Id;
				po.ModifyUserId = UserAccount.ID;
				po.IsApproved = IsApproved;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			decimal DoQty = default(decimal);
			decimal NetWeight = default(decimal);
			bool OtherThanFOCExist = false;
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow r in rows)
			{
				DoQty += Conversion.ToDecimal(r.Cells["QTY"].Value);
				NetWeight += Conversion.ToDecimal(r.Cells["NetWeight"].Value);
				if (Conversion.ToInt(r.Cells["OrderId"].Value) == 0)
				{
					throw new Exception("SaleOrder No Not Found in Detail Grd Row No: " + (r.RowIndex + 1));
				}
				if (!Conversion.ToBool(r.Cells["IsFOC"].Value))
				{
					OtherThanFOCExist = true;
				}
			}
			if (!OtherThanFOCExist)
			{
				throw new Exception("Atleast one Record other than FreeOFCost in grd ");
			}
			po.OrganizationId = UserAccount.OrganizationId;
			po.CompanyId = UserAccount.CompanyId;
			po.BranchesId = Conversion.ToInt((object)UserAccount.BranchesId);
			po.ProjectsId = Conversion.ToInt((object)UserAccount.BranchesId);
			po.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			po.DocumentTypeId = 1853;
			po.DocDate = DocDate.Value;
			po.DocNo = Conversion.ToInt((object)txtdocno.Text);
			po.DeliveryOrderTypeId = 1;
			po.VehicleTypeId = Conversion.ToInt(CmbVehicleType.Value);
			po.VehicleNo = Conversion.ToString((object)((Control)(object)CmbVehicleNo).Text.Trim());
			po.RefrenenceNo = Conversion.ToString((object)txtReferenceNoHeader.Text.Trim());
			po.Distance = Conversion.ToString((object)txtDistance.Text.Trim());
			po.DoTotalQty = Conversion.ToDecimal((object)DoQty);
			po.IsApproved = false;
			po.EntryUserId = UserAccount.ID;
			po.ModifyUserId = UserAccount.ID;
			po.ModifyDate = DateTime.Now;
			po.EntryDate = DateTime.Now;
			po.ApprovedDate = DateTime.Now;
			po.InvDeliveryOrderDetailList = new List<InvDeliveryOrderdetail>();
			po.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			rows = grd.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				InvDeliveryOrderdetail vd = new InvDeliveryOrderdetail();
				if (Id == 0)
				{
					vd.ActionTypeId = 1;
				}
				else
				{
					vd.Id = Conversion.ToInt(r2.Cells["Id"].Value);
					if (vd.Id > 0)
					{
						vd.ActionTypeId = 2;
					}
					else
					{
						vd.ActionTypeId = 1;
					}
				}
				if (Conversion.ToInt(r2.Cells["OrderId"].Value) > 0)
				{
					vd.SaleOrderId = Conversion.ToInt(r2.Cells["OrderId"].Value);
					if (Conversion.ToInt(r2.Cells["OrderDetailId"].Value) > 0)
					{
						vd.SaleOrderDetailId = Conversion.ToInt(r2.Cells["OrderDetailId"].Value);
						if (Conversion.ToInt(r2.Cells["SupplierCustomerId"].Value) > 0)
						{
							vd.SupplierCustomerId = Conversion.ToInt(r2.Cells["SupplierCustomerId"].Value);
							if (Conversion.ToString(r2.Cells["ReferencePartyName"].Value) != string.Empty)
							{
								vd.ReferencePartyName = Conversion.ToString(r2.Cells["ReferencePartyName"].Value);
								if (Conversion.ToString(r2.Cells["ReferencePartyCellNo"].Value) != string.Empty)
								{
									vd.ReferencePartyCellNo = Conversion.ToString(r2.Cells["ReferencePartyCellNo"].Value);
									if (Conversion.ToString(r2.Cells["ReferencePartyAddress"].Value) != string.Empty)
									{
										vd.ReferencePartyAddress = Conversion.ToString(r2.Cells["ReferencePartyAddress"].Value);
										if (Conversion.ToInt(r2.Cells["WareHouseId"].Value) > 0)
										{
											vd.WarehouseId = Conversion.ToInt(r2.Cells["WareHouseId"].Value);
											if (Conversion.ToInt(r2.Cells["ItemId"].Value) > 0)
											{
												vd.ItemId = Conversion.ToInt(r2.Cells["ItemId"].Value);
												if (Conversion.ToInt(r2.Cells["AttributeVarientId"].Value) == 0)
												{
													throw new Exception("AttributeVarient Required in Detail Grid And row No: " + (r2.RowIndex + 1));
												}
												vd.ItemAttributeVarientId = Conversion.ToInt(r2.Cells["AttributeVarientId"].Value);
												if (Conversion.ToDecimal(r2.Cells["VarientUnit"].Value) == 0m)
												{
													throw new Exception("VarientUnit Required in Detail Grid And row No: " + (r2.RowIndex + 1));
												}
												vd.VarientEquivalent = Conversion.ToDecimal(r2.Cells["VarientUnit"].Value);
												if (Conversion.ToInt(r2.Cells["JobLotId"].Value) > 0)
												{
													vd.JobLotId = Conversion.ToInt(r2.Cells["JobLotId"].Value);
													if (Conversion.ToDecimal(r2.Cells["QTY"].Value) > 0m)
													{
														vd.DoQty = Conversion.ToDecimal(r2.Cells["QTY"].Value);
														if (Conversion.ToDecimal(r2.Cells["ItemWeight"].Value) > 0m)
														{
															vd.ItemWeight = Conversion.ToDecimal(r2.Cells["ItemWeight"].Value);
															if (Conversion.ToDecimal(r2.Cells["NetWeight"].Value) > 0m)
															{
																vd.DoNetWeight = Conversion.ToDecimal(r2.Cells["NetWeight"].Value);
																if (Conversion.ToInt(r2.Cells["CityId"].Value) > 0)
																{
																	vd.CityId = Conversion.ToInt(r2.Cells["CityId"].Value);
																	vd.BuildingHeightId = Conversion.ToInt(r2.Cells["BuildingHeightId"].Value);
																	vd.BuildingStoreyId = Conversion.ToInt(r2.Cells["BuildingStoreyId"].Value);
																	vd.BuildingArea = Conversion.ToString(r2.Cells["BuildingArea"].Value);
																	vd.ReferencePartyId = Conversion.ToInt(r2.Cells["ReferencePartyId"].Value);
																	vd.RemarksDetail = Conversion.ToString(r2.Cells["RemarksDetail"].Value);
																	vd.IsFOC = Conversion.ToBool(r2.Cells["IsFOC"].Value);
																	po.InvDeliveryOrderDetailList.Add(vd);
																	continue;
																}
																throw new Exception("City Field Required in Detail Grid Row No: " + (r2.RowIndex + 1));
															}
															throw new Exception("DoNetWeight Field Required in Detail Grid Row No: " + (r2.RowIndex + 1));
														}
														throw new Exception("ItemWeight Field Required in Detail Grid Row No: " + (r2.RowIndex + 1));
													}
													throw new Exception("ItemQty Field Required in Detail Grid Row No: " + (r2.RowIndex + 1));
												}
												throw new Exception("JobLot Field Required in Detail Grid Row No: " + (r2.RowIndex + 1));
											}
											throw new Exception("Item Name Field Required in Detail Grid Row No: " + (r2.RowIndex + 1));
										}
										throw new Exception("WareHouseName Field Required in Detail Grid Row No: " + (r2.RowIndex + 1));
									}
									throw new Exception("ReferencePartyAddress Field Required in Detail Grid Row No: " + (r2.RowIndex + 1));
								}
								throw new Exception("ReferencePartyCellNo Field Required in Detail Grid Row No: " + (r2.RowIndex + 1));
							}
							throw new Exception("ReferencePartyName Field Required in Detail Grid Row No: " + (r2.RowIndex + 1));
						}
						throw new Exception("Customer Field Required in Detail Grid Row No: " + (r2.RowIndex + 1));
					}
					throw new Exception("OrderDetailId Field Required in Detail Grid Row No: " + (r2.RowIndex + 1));
				}
				throw new Exception("OrderNo Field Required in Detail Grid Row No: " + (r2.RowIndex + 1));
			}
			if (lstRemoveRecord.Count > 0)
			{
				for (int j = 0; j < lstRemoveRecord.Count; j++)
				{
					po.InvDeliveryOrderDetailList.Add(lstRemoveRecord[j]);
				}
			}
			string WarningMessage = InvDeliveryOrder.DeliveryOrderStackWarningMessage(po);
			if (WarningMessage != null && WarningMessage != "" && MessageBox.Show(WarningMessage, "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
			{
				return;
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
			int success = InvDeliveryOrder.Save(po);
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
						if (!fileDeleted)
						{
							throw new Exception("Unable to delete file. It is in use by another process.");
						}
					}
				}
			}
			else
			{
				MessageBox.Show("Data Save Successfully....  " + po.DocNo);
			}
			Reset();
			if (ChkPrint.Checked)
			{
				CommonServices.DeliveryOrderConcreteSlip(success);
			}
			if (chkPrintCustomerWise.Checked)
			{
				CommonServices.DeliveryOrderConcreteSlipCustomerWise(success);
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

	public void getUpdate(int ID)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		try
		{
			Id = ID;
			InvDeliveryOrder po = new InvDeliveryOrder();
			po = InvDeliveryOrder.ReadById(ID);
			if (po == null)
			{
				return;
			}
			tabControl1.SelectedIndex = 0;
			DocDate.Value = po.DocDate;
			txtdocno.Text = Conversion.ToString((object)po.DocNo);
			((Control)(object)CmbVehicleType).Text = Conversion.ToString((object)po.VehicleType);
			((Control)(object)CmbVehicleNo).Text = po.VehicleNo;
			txtReferenceNoHeader.Text = po.RefrenenceNo;
			txtDistance.Text = po.Distance;
			txtRemarksHeader.Text = po.RemarksHeader;
			IsApproved = po.IsApproved;
			table.Clear();
			for (int i = 0; i < po.InvDeliveryOrderDetailList.Count; i++)
			{
				table.Rows.Add(po.InvDeliveryOrderDetailList[i].Id, po.InvDeliveryOrderDetailList[i].SupplierCustomerId, po.InvDeliveryOrderDetailList[i].CustomerName, po.InvDeliveryOrderDetailList[i].SaleOrderId, po.InvDeliveryOrderDetailList[i].SaleOrderDetailId, po.InvDeliveryOrderDetailList[i].OrderNo, po.InvDeliveryOrderDetailList[i].ReferencePartyId, po.InvDeliveryOrderDetailList[i].ReferencePartyName, po.InvDeliveryOrderDetailList[i].ReferencePartyCellNo, po.InvDeliveryOrderDetailList[i].ReferencePartyAddress, po.InvDeliveryOrderDetailList[i].BuildingStoreyId, po.InvDeliveryOrderDetailList[i].BuildingStorey, po.InvDeliveryOrderDetailList[i].BuildingHeightId, po.InvDeliveryOrderDetailList[i].BuildingHeight, po.InvDeliveryOrderDetailList[i].BuildingArea, po.InvDeliveryOrderDetailList[i].WarehouseId, po.InvDeliveryOrderDetailList[i].WareHouseName, po.InvDeliveryOrderDetailList[i].ItemId, po.InvDeliveryOrderDetailList[i].ItemCode, po.InvDeliveryOrderDetailList[i].ItemName, po.InvDeliveryOrderDetailList[i].ItemAttributeVarientId, po.InvDeliveryOrderDetailList[i].ItemAttributeVarient, po.InvDeliveryOrderDetailList[i].VarientEquivalent, po.InvDeliveryOrderDetailList[i].JobLotId, po.InvDeliveryOrderDetailList[i].JobLotDescription, po.InvDeliveryOrderDetailList[i].DoQty, po.InvDeliveryOrderDetailList[i].DoQty, po.InvDeliveryOrderDetailList[i].ItemWeight, po.InvDeliveryOrderDetailList[i].DoNetWeight, po.InvDeliveryOrderDetailList[i].CityId, po.InvDeliveryOrderDetailList[i].CityName, po.InvDeliveryOrderDetailList[i].RemarksDetail, po.InvDeliveryOrderDetailList[i].IsFOC);
			}
			grd.DataSource = table;
			grd.RetrieveStructure();
			grdSettings();
			AvailableStockUpdateInGrid();
			List<DMSAttachments> byID = global::Architecture.BLL.DMSAttachments.GetByID(Id, base.Name);
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
			btnDelete.Visible = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnDelete_Click(object sender, EventArgs e)
	{
		try
		{
			if (Id == 0)
			{
				MessageBox.Show("Record Id Not Found", "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
			else if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)
			{
				if (IsApproved)
				{
					MessageBox.Show("Record has been approved", "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					return;
				}
				InvDeliveryOrder.DeleteByID(UserAccount.ID, Id);
				MessageBox.Show("Delete Record Seccessfully");
				Reset();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void LoadInGridDetail()
	{
		try
		{
			LoadTransactions = new LoadSaleOrderForDeliveryOrderConcrete(UserAccount);
			LoadTransactions.ShowDialog();
			LoadTransactions.BringToFront();
			LoadDataDetailfromOrder();
			AvailableStockUpdateInGrid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadDataDetailfromOrder()
	{
		try
		{
			DataTable dtpurchaseinvoice = LoadTransactions.dtLoader;
			if (dtpurchaseinvoice.Rows.Count <= 0)
			{
				return;
			}
			txtReferenceNoHeader.Text = Conversion.ToString(dtpurchaseinvoice.Rows[0]["RefrenenceNo"]);
			txtDistance.Text = Conversion.ToString(dtpurchaseinvoice.Rows[0]["Distance"]);
			bool flag = false;
			for (int i = 0; i < dtpurchaseinvoice.Rows.Count; i++)
			{
				if (grd.GetRows().Length != 0)
				{
					GridEXRow[] rows = grd.GetRows();
					for (int j = 0; j < rows.Length; j++)
					{
						if (Conversion.ToInt(rows[j].Cells["OrderDetailId"].Value) == Conversion.ToInt(dtpurchaseinvoice.Rows[i]["DetailId"]))
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					table.Rows.Add(0, dtpurchaseinvoice.Rows[i]["SupplierCustomerId"], dtpurchaseinvoice.Rows[i]["SupplierCustomerId"], dtpurchaseinvoice.Rows[i]["Id"], dtpurchaseinvoice.Rows[i]["DetailId"], dtpurchaseinvoice.Rows[i]["DocNo"], dtpurchaseinvoice.Rows[0]["ReferencePartyId"], dtpurchaseinvoice.Rows[0]["ReferencPartyName"], dtpurchaseinvoice.Rows[0]["ReferencPartyCellNo"], dtpurchaseinvoice.Rows[0]["ReferencPartyAddress"], Conversion.ToInt(dtpurchaseinvoice.Rows[0]["BuildingStoreyId"]), Conversion.ToString(dtpurchaseinvoice.Rows[0]["BuildingStorey"]), Conversion.ToInt(dtpurchaseinvoice.Rows[0]["BuildingHeightId"]), Conversion.ToString(dtpurchaseinvoice.Rows[0]["BuildingHeight"]), Conversion.ToString(dtpurchaseinvoice.Rows[0]["BuildingArea"]), 0, "", dtpurchaseinvoice.Rows[i]["OrderItemId"], dtpurchaseinvoice.Rows[i]["ItemCode"], dtpurchaseinvoice.Rows[i]["ItemName"], dtpurchaseinvoice.Rows[i]["ItemAttributeVarientId"], dtpurchaseinvoice.Rows[i]["ItemArrtibuteVarient"], dtpurchaseinvoice.Rows[i]["VarientEquivalent"], dtpurchaseinvoice.Rows[i]["JobLotId"], dtpurchaseinvoice.Rows[i]["JobLotDescription"], dtpurchaseinvoice.Rows[i]["BalanceQty"], dtpurchaseinvoice.Rows[i]["BalanceQty"], dtpurchaseinvoice.Rows[i]["ItemWeight"], dtpurchaseinvoice.Rows[i]["BalanceWeight"], dtpurchaseinvoice.Rows[i]["CityId"], dtpurchaseinvoice.Rows[i]["CityName"], dtpurchaseinvoice.Rows[i]["RemarksDetail"], false);
					flag = false;
				}
			}
			grd.DataSource = table;
			grd.RetrieveStructure();
			grdSettings();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetSaleOrderIds()
	{
		try
		{
			dtSaleOrderIds = "";
			dtSaleOrderMainIds = "";
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow item in rows)
			{
				dtSaleOrderIds = dtSaleOrderIds + "," + Conversion.ToInt(item.Cells["OrderDetailId"].Value);
				dtSaleOrderMainIds = dtSaleOrderMainIds + "," + Conversion.ToInt(item.Cells["OrderId"].Value);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnLoadSaleOrder_Click(object sender, EventArgs e)
	{
		try
		{
			LoadInGridDetail();
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
			ParameterFill();
			HistoryComboBind();
			cmbDateTypeHistory.Focus();
			if (Conversion.ToInt(cmbDateTypeHistory.Value) == 0)
			{
				((UltraGridBase)cmbDateTypeHistory).Rows[0].Activate();
			}
		}
		else
		{
			DocDate.Focus();
		}
	}

	private void ParameterFill()
	{
		try
		{
			DataTable dt = CommonServices.DateType();
			DDL.BindDDL(dt, cmbDateTypeHistory, "Id", "Parameters", "Parameters", ZeroIndex: false);
			((UltraGridBase)cmbDateTypeHistory).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbDateTypeHistory_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (Convert.ToInt32(cmbDateTypeHistory.Value) == 1)
			{
				txtFromdateHistory.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbDateTypeHistory.Value) == 2)
			{
				txtFromdateHistory.Value = DateTime.Today.AddDays(-7.0);
			}
			else if (Convert.ToInt32(cmbDateTypeHistory.Value) == 3)
			{
				DateTime now = DateTime.UtcNow;
				DateTime first = now.Date.AddDays(-(now.Date.Day - 1));
				txtFromdateHistory.Value = first;
				txtToDateHistory.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbDateTypeHistory.Value) == 4)
			{
				int year = DateTime.Now.Year;
				DateTime first2 = new DateTime(year, 1, 1);
				txtFromdateHistory.Value = first2;
				txtToDateHistory.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbDateTypeHistory.Value) == 5)
			{
				txtFromdateHistory.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryComboBind()
	{
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Expected O, but got Unknown
		try
		{
			int Id = Conversion.ToInt(CmbCustomerHistory.Value);
			int IdC = Conversion.ToInt(CmbVehicleNoHistory.Value);
			int IdI = Conversion.ToInt(CmbItemHistory.Value);
			DataTable dt = InvDeliveryOrder.GetDataForDropDownFromDeliveryOrder(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				DataTable dtCustomer = new DataTable();
				dtCustomer.Columns.Add("Id", typeof(int));
				dtCustomer.Columns.Add("CustomerName");
				DataTable dtItemName = new DataTable();
				dtItemName.Columns.Add("Id", typeof(int));
				dtItemName.Columns.Add("ItemName");
				DataTable dtVehicleNo = new DataTable();
				dtVehicleNo.Columns.Add("Id", typeof(int));
				dtVehicleNo.Columns.Add("VehicleNo");
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToString(dt.Rows[i]["Activity"]) == "Customer")
					{
						dtCustomer.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
					else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "Item")
					{
						dtItemName.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
					else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "VehicleNo")
					{
						dtVehicleNo.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
				}
				if (dtCustomer.Rows.Count > 0)
				{
					DDL.BindDDL(dtCustomer, CmbCustomerHistory, "Id", "CustomerName", "Customer Name", ZeroIndex: true);
					if (Id > 0)
					{
						if (dtCustomer.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
						{
							CmbCustomerHistory.Value = Id;
						}
						else
						{
							((Control)(object)CmbCustomerHistory).Text = string.Empty;
						}
					}
				}
				else
				{
					((UltraGridBase)CmbCustomerHistory).DataSource = null;
				}
				if (dtItemName.Rows.Count > 0)
				{
					DDL.BindDDL(dtItemName, CmbItemHistory, "Id", "ItemName", "ItemName", ZeroIndex: true);
					if (IdI > 0)
					{
						if (dtItemName.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == IdI))
						{
							CmbItemHistory.Value = IdI;
						}
						else
						{
							((Control)(object)CmbItemHistory).Text = string.Empty;
						}
					}
				}
				else
				{
					((UltraGridBase)CmbItemHistory).DataSource = null;
				}
				if (dtVehicleNo.Rows.Count > 0)
				{
					DDL.BindDDL(dtVehicleNo, CmbVehicleNoHistory, "Id", "VehicleNo", "VehicleNo", ZeroIndex: true);
					if (IdC > 0)
					{
						if (dtVehicleNo.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == IdC))
						{
							CmbVehicleNoHistory.Value = IdC;
						}
						else
						{
							((Control)(object)CmbVehicleNoHistory).Text = string.Empty;
						}
					}
				}
				else
				{
					((UltraGridBase)CmbVehicleNoHistory).DataSource = null;
				}
			}
			else
			{
				((UltraGridBase)CmbCustomerHistory).DataSource = null;
				((UltraGridBase)CmbItemHistory).DataSource = null;
				((UltraGridBase)CmbVehicleNoHistory).DataSource = null;
			}
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
			HistoryComboBind();
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
			cmbDateTypeHistory.Value = 3;
			txtFromNoHistory.Text = string.Empty;
			txtToDocNoHistory.Text = string.Empty;
			((Control)(object)CmbCustomerHistory).Text = string.Empty;
			((Control)(object)CmbItemHistory).Text = string.Empty;
			((Control)(object)CmbVehicleNoHistory).Text = string.Empty;
			cmbDateTypeHistory.Focus();
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
			Resethistory();
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
			FillHistory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillHistory()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		try
		{
			dthistoryfill.Rows.Clear();
			ReportsParameters reports = new ReportsParameters();
			reports.OrganizationId = UserAccount.OrganizationId;
			reports.CompanyId = UserAccount.CompanyId;
			reports.BranchesId = UserAccount.BranchesId;
			reports.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			reports.DocumentTypeId = 1853;
			reports.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			if (!reports.CanViewAllRecord)
			{
				reports.EntryUser = UserAccount.ID;
			}
			reports.FromDate = Conversion.ToDateTime((object)txtFromdateHistory.Value);
			reports.ToDate = Conversion.ToDateTime((object)txtToDateHistory.Value);
			reports.FromDocNo = Conversion.ToInt((object)txtFromNoHistory.Text);
			reports.ToDocNo = Conversion.ToInt((object)txtToDocNoHistory.Text);
			if (Conversion.ToInt(CmbVehicleNoHistory.Value) > 0)
			{
				reports.VehicleNo = Conversion.ToString((object)((Control)(object)CmbVehicleNoHistory).Text);
			}
			reports.ItemId = Conversion.ToInt((object)((Control)(object)CmbItemHistory).Text);
			reports.SupplierCustomerId = Conversion.ToInt(CmbCustomerHistory.Value);
			dthistoryfill = InvDeliveryOrder.FormHistory(reports);
			if (dthistoryfill.Rows.Count > 0)
			{
				DataTable dtDeliveryOrder = new DataTable();
				dtDeliveryOrder.Columns.Add("Id", typeof(int));
				dtDeliveryOrder.Columns.Add("DocNo", typeof(string));
				dtDeliveryOrder.Columns.Add("DocumentTypeId", typeof(int));
				dtDeliveryOrder.Columns.Add("DocDate", typeof(DateTime));
				dtDeliveryOrder.Columns.Add("DeliveryOrderTypeId", typeof(int));
				dtDeliveryOrder.Columns.Add("RefrenenceNo", typeof(string));
				dtDeliveryOrder.Columns.Add("Distance", typeof(string));
				dtDeliveryOrder.Columns.Add("VehicleNo", typeof(string));
				dtDeliveryOrder.Columns.Add("VehicleType", typeof(string));
				dtDeliveryOrder.Columns.Add("DoTotalQty", typeof(int));
				dtDeliveryOrder.Columns.Add("RemarksHeader", typeof(string));
				dtDeliveryOrder.Columns.Add("IsApproved", typeof(bool));
				dtDeliveryOrder.Columns.Add("EntryDate", typeof(DateTime));
				dtDeliveryOrder.Columns.Add("EntryUserName", typeof(string));
				dtDeliveryOrder.Columns.Add("ModifyDate", typeof(DateTime));
				dtDeliveryOrder.Columns.Add("ModifyUserName", typeof(string));
				dtDeliveryOrder.Columns.Add("ApprovedDate", typeof(DateTime));
				dtDeliveryOrder.Columns.Add("ApprovedUserName", typeof(string));
				dtDeliveryOrder.Columns.Add("NoOfAttachments", typeof(int));
				dtDeliveryOrder.Rows.Clear();
				foreach (DataRow row in dthistoryfill.Rows)
				{
					if (!dtDeliveryOrder.AsEnumerable().Any((DataRow rr) => rr.Field<int>("Id") == Conversion.ToInt(row["Id"])))
					{
						dtDeliveryOrder.Rows.Add(row["Id"], row["DocNo"], row["DocumentTypeId"], row["DocDate"], row["DeliveryOrderTypeId"], row["RefrenenceNo"], row["Distance"], row["VehicleNo"], row["VehicleType"], row["DoTotalQty"], row["RemarksHeader"], row["IsApproved"], row["EntryDate"], row["EntryUserName"], row["ModifyDate"], row["ModifyUserName"], row["ApprovedDate"], row["ApprovedUserName"], row["NoOfAttachments"]);
					}
				}
				grdhistory.DataSource = dtDeliveryOrder;
				grdhistory.RetrieveStructure();
				HistoryGridSettings();
			}
			else
			{
				grdhistory.ClearStructure();
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
			grdhistory.AllowEdit = (InheritableBoolean)2;
			grdhistory.AllowDelete = (InheritableBoolean)2;
			grdhistory.RootTable.Columns["Id"].Visible = false;
			grdhistory.RootTable.Columns["DocumentTypeId"].Visible = false;
			grdhistory.RootTable.Columns["DeliveryOrderTypeId"].Visible = false;
			grdhistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdhistory.RootTable.Columns["NoOfAttachments"].Caption = "Attached";
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
			grdhistory.RootTable.Columns.Add("PrintII");
			grdhistory.RootTable.Columns["PrintII"].Caption = "PrintII";
			grdhistory.RootTable.Columns["PrintII"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdhistory.RootTable.Columns["PrintII"].ButtonStyle = (ButtonStyle)5;
			grdhistory.RootTable.Columns["PrintII"].ButtonText = "PrintII";
			grdhistory.RootTable.Columns["PrintII"].Width = 50;
			grdhistory.RootTable.Columns["NoOfAttachments"].Position = 3;
			grdhistory.RootTable.Columns["Edit"].Position = 0;
			grdhistory.RootTable.Columns["Print"].Position = 1;
			grdhistory.RootTable.Columns["PrintII"].Position = 2;
			grdhistory.FrozenColumns = 4;
			grdhistory.RootTable.Columns["DoTotalQty"].AggregateFunction = (AggregateFunction)2;
			grdhistory.RootTable.Columns["DoTotalQty"].TextAlignment = (TextAlignment)3;
			grdhistory.RootTable.Columns["DoTotalQty"].FormatString = "##,#.##";
			grdhistory.RootTable.Columns["DoTotalQty"].TotalFormatString = "##,#.##";
			CommonServices.GridAutoAdjustmentNew(grdhistory);
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdhistory_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			Id = Conversion.ToInt((object)grdhistory.CurrentRow.Cells["Id"].Value.ToString());
			getUpdate(Id);
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
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(item.Cells["Id"].Value), 1853);
			}
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
				getUpdate(Id);
			}
			if (e.Column.Key == "Print")
			{
				CommonServices.DeliveryOrderConcreteSlip(Conversion.ToInt(grdhistory.CurrentRow.Cells["Id"].Value));
			}
			if (e.Column.Key == "PrintII")
			{
				CommonServices.DeliveryOrderConcreteSlipCustomerWise(Conversion.ToInt(grdhistory.CurrentRow.Cells["Id"].Value));
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
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected O, but got Unknown
		try
		{
			GridEXRow item = grdhistory.CurrentRow;
			if (item == null || (int)item.RowType != 0 || item.Cells.Count <= 0)
			{
				return;
			}
			InvDeliveryOrder po = new InvDeliveryOrder();
			po = InvDeliveryOrder.ReadById(Conversion.ToInt(item.Cells["Id"].Value));
			if (po != null)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("SupplierCustomerId", typeof(int));
				dt.Columns.Add("SupplierCustomer", typeof(string));
				dt.Columns.Add("OrderId", typeof(int));
				dt.Columns.Add("OrderNo", typeof(int));
				dt.Columns.Add("ReferencePartyName", typeof(string));
				dt.Columns.Add("ReferencePartyCellNo", typeof(string));
				dt.Columns.Add("ReferencePartyAddress", typeof(string));
				dt.Columns.Add("WareHouse", typeof(string));
				dt.Columns.Add("ItemId", typeof(int));
				dt.Columns.Add("ItemCode", typeof(string));
				dt.Columns.Add("ItemName", typeof(string));
				dt.Columns.Add("AttributeVarient", typeof(string));
				dt.Columns.Add("VarientUnit", typeof(decimal));
				dt.Columns.Add("JobLot", typeof(string));
				dt.Columns.Add("QTY", typeof(decimal));
				dt.Columns.Add("ItemWeight", typeof(decimal));
				dt.Columns.Add("NetWeight", typeof(decimal));
				dt.Columns.Add("CityName", typeof(string));
				dt.Columns.Add("RemarksDetail", typeof(string));
				dt.Columns.Add("IsFOC", typeof(bool));
				dt.Clear();
				for (int i = 0; i < po.InvDeliveryOrderDetailList.Count; i++)
				{
					dt.Rows.Add(po.InvDeliveryOrderDetailList[i].Id, po.InvDeliveryOrderDetailList[i].SupplierCustomerId, po.InvDeliveryOrderDetailList[i].CustomerName, po.InvDeliveryOrderDetailList[i].SaleOrderId, po.InvDeliveryOrderDetailList[i].OrderNo, po.InvDeliveryOrderDetailList[i].ReferencePartyName, po.InvDeliveryOrderDetailList[i].ReferencePartyCellNo, po.InvDeliveryOrderDetailList[i].ReferencePartyAddress, po.InvDeliveryOrderDetailList[i].WareHouseName, po.InvDeliveryOrderDetailList[i].ItemId, po.InvDeliveryOrderDetailList[i].ItemCode, po.InvDeliveryOrderDetailList[i].ItemName, po.InvDeliveryOrderDetailList[i].ItemAttributeVarient, po.InvDeliveryOrderDetailList[i].VarientEquivalent, po.InvDeliveryOrderDetailList[i].JobLotDescription, po.InvDeliveryOrderDetailList[i].DoQty, po.InvDeliveryOrderDetailList[i].ItemWeight, po.InvDeliveryOrderDetailList[i].DoNetWeight, po.InvDeliveryOrderDetailList[i].CityName, po.InvDeliveryOrderDetailList[i].RemarksDetail, po.InvDeliveryOrderDetailList[i].IsFOC);
				}
				GrdHistoryDetail.DataSource = dt;
				GrdHistoryDetail.RetrieveStructure();
				GridDetailSetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridDetailSetting()
	{
		//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d8: Expected O, but got Unknown
		try
		{
			GrdHistoryDetail.RootTable.Columns["Id"].Visible = false;
			GrdHistoryDetail.RootTable.Columns["SupplierCustomerId"].Visible = false;
			GrdHistoryDetail.RootTable.Columns["OrderId"].Visible = false;
			GrdHistoryDetail.RootTable.Columns["ItemId"].Visible = false;
			GrdHistoryDetail.RootTable.Columns["VarientUnit"].AggregateFunction = (AggregateFunction)2;
			GrdHistoryDetail.RootTable.Columns["VarientUnit"].TextAlignment = (TextAlignment)3;
			GrdHistoryDetail.RootTable.Columns["VarientUnit"].FormatString = "#,##0.##";
			GrdHistoryDetail.RootTable.Columns["VarientUnit"].TotalFormatString = "#,##0.##";
			GrdHistoryDetail.RootTable.Columns["QTY"].AggregateFunction = (AggregateFunction)2;
			GrdHistoryDetail.RootTable.Columns["QTY"].TextAlignment = (TextAlignment)3;
			GrdHistoryDetail.RootTable.Columns["QTY"].FormatString = "#,##0.##";
			GrdHistoryDetail.RootTable.Columns["QTY"].TotalFormatString = "#,##0.##";
			GrdHistoryDetail.RootTable.Columns["ItemWeight"].AggregateFunction = (AggregateFunction)2;
			GrdHistoryDetail.RootTable.Columns["ItemWeight"].TextAlignment = (TextAlignment)3;
			GrdHistoryDetail.RootTable.Columns["ItemWeight"].FormatString = "#,##0.##";
			GrdHistoryDetail.RootTable.Columns["ItemWeight"].TotalFormatString = "#,##0.##";
			GrdHistoryDetail.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			GrdHistoryDetail.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			GrdHistoryDetail.RootTable.Columns["NetWeight"].FormatString = "#,##0.##";
			GrdHistoryDetail.RootTable.Columns["NetWeight"].TotalFormatString = "#,##0.##";
			CommonServices.GridAutoAdjustmentNew(GrdHistoryDetail);
			new GridEXColumn();
			GridEXFormatCondition fc = new GridEXFormatCondition(GrdHistoryDetail.RootTable.Columns["IsFOC"], (ConditionOperator)0, (object)true);
			fc.FormatStyle.ForeColor = Color.Red;
			GrdHistoryDetail.RootTable.FormatConditions.Add(fc);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GrdHistoryDetail_LinkClicked(object sender, ColumnActionEventArgs e)
	{
	}

	private void btnprint_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.DeliveryOrderConcreteSlip(Id);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrintCustomerWise_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.DeliveryOrderConcreteSlipCustomerWise(Id);
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

	private void btnDefineReferenceParties_Click(object sender, EventArgs e)
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
					((Control)(object)grdhistory).Focus();
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
				if (e.Control && e.KeyCode == Keys.U && btnupdate.Visible && btnupdate.Enabled)
				{
					btnupdate_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnnew_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefresh_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					DocDate.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grd).Focus();
				}
				if (e.Control && e.KeyCode == Keys.Right)
				{
					if (((Control)(object)grd).Focused)
					{
						CmbSupplierCustomer.Focus();
					}
					else
					{
						((Control)(object)grd).Focus();
					}
				}
				if (e.Control && e.KeyCode == Keys.Up)
				{
					CmbSupplierCustomer.Focus();
				}
				if (e.Control && e.KeyCode == Keys.F10)
				{
					btnattachment_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.P)
				{
					btnprint_Click(null, null);
				}
				if (e.Alt && (e.KeyCode == Keys.NumPad1 || e.KeyCode == Keys.D1))
				{
					btnprint_Click(null, null);
				}
				return;
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				((Control)(object)grdhistory).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)GrdHistoryDetail).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Right)
			{
				if (((Control)(object)GrdHistoryDetail).Focused)
				{
					((Control)(object)grdhistory).Focus();
				}
				else
				{
					((Control)(object)GrdHistoryDetail).Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				((Control)(object)grdhistory).Focus();
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
			dt.Rows.Add("Ctrl+Shift+Delete", "For Delete");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+P", "For Print Slip 1853");
			dt.Rows.Add("Alt+1", "For Print Slip 1853");
			dt.Rows.Add("Ctrl+F5", "For Focus on Customer Name");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+D", "For Adding an row in Focused Grid");
			dt.Rows.Add("Ctrl+Delete", "For Deleting an row of Focused Grid");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Customer Name in Detail Box");
			dt.Rows.Add("Ctrl+ArrowRight", "For Change Focus from one Grid To another Grid");
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

	private void grd_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_048a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0491: Expected O, but got Unknown
		//IL_0313: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Expected O, but got Unknown
		//IL_06d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f4: Unknown result type (might be due to invalid IL or missing references)
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
					if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
					{
						if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
						{
							return;
						}
						InvDeliveryOrderdetail vd = new InvDeliveryOrderdetail();
						vd.Id = Conversion.ToInt(r.Cells["Id"].Value);
						vd.SaleOrderId = Conversion.ToInt(r.Cells["OrderId"].Value);
						vd.SaleOrderDetailId = Conversion.ToInt(r.Cells["OrderDetailId"].Value);
						vd.SupplierCustomerId = Conversion.ToInt(r.Cells["SupplierCustomerId"].Value);
						vd.ReferencePartyName = Conversion.ToString(r.Cells["ReferencePartyName"].Value);
						vd.ReferencePartyCellNo = Conversion.ToString(r.Cells["ReferencePartyCellNo"].Value);
						vd.ReferencePartyAddress = Conversion.ToString(r.Cells["ReferencePartyAddress"].Value);
						vd.WarehouseId = Conversion.ToInt(r.Cells["WareHouseId"].Value);
						vd.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
						vd.ItemAttributeVarientId = Conversion.ToInt(r.Cells["AttributeVarientId"].Value);
						vd.VarientEquivalent = Conversion.ToDecimal(r.Cells["VarientUnit"].Value);
						vd.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
						vd.DoQty = Conversion.ToDecimal(r.Cells["QTY"].Value);
						vd.ItemWeight = Conversion.ToDecimal(r.Cells["ItemWeight"].Value);
						vd.DoNetWeight = Conversion.ToDecimal(r.Cells["DoNetWeight"].Value);
						vd.CityId = Conversion.ToInt(r.Cells["CityId"].Value);
						vd.RemarksDetail = Conversion.ToString(r.Cells["RemarksDetail"].Value);
						vd.ActionTypeId = 3;
						lstRemoveRecord.Add(vd);
						table.Rows[r.RowIndex].Delete();
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
					GetSaleOrderIds();
				}
				if (grd.CurrentColumn.Key == "Edit")
				{
					grd_DoubleClick(null, null);
				}
				if (grd.CurrentColumn.Key == "Add")
				{
					DataRow dr = table.Rows[r.RowIndex];
					table.Rows.Add(dr.ItemArray);
					table.Rows[table.Rows.Count - 1]["Id"] = 0;
					grd.DataSource = table;
				}
			}
			if (e.Control && e.KeyCode == Keys.D)
			{
				DataRow dr2 = table.Rows[r.RowIndex];
				table.Rows.Add(dr2.ItemArray);
				table.Rows[table.Rows.Count - 1]["Id"] = 0;
				grd.DataSource = table;
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				InvDeliveryOrderdetail vd2 = new InvDeliveryOrderdetail();
				vd2.Id = Conversion.ToInt(r.Cells["Id"].Value);
				vd2.SaleOrderId = Conversion.ToInt(r.Cells["OrderId"].Value);
				vd2.SaleOrderDetailId = Conversion.ToInt(r.Cells["OrderDetailId"].Value);
				vd2.SupplierCustomerId = Conversion.ToInt(r.Cells["SupplierCustomerId"].Value);
				vd2.ReferencePartyName = Conversion.ToString(r.Cells["ReferencePartyName"].Value);
				vd2.ReferencePartyCellNo = Conversion.ToString(r.Cells["ReferencePartyCellNo"].Value);
				vd2.ReferencePartyAddress = Conversion.ToString(r.Cells["ReferencePartyAddress"].Value);
				vd2.WarehouseId = Conversion.ToInt(r.Cells["WareHouseId"].Value);
				vd2.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
				vd2.ItemAttributeVarientId = Conversion.ToInt(r.Cells["AttributeVarientId"].Value);
				vd2.VarientEquivalent = Conversion.ToDecimal(r.Cells["VarientUnit"].Value);
				vd2.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
				vd2.DoQty = Conversion.ToDecimal(r.Cells["QTY"].Value);
				vd2.ItemWeight = Conversion.ToDecimal(r.Cells["ItemWeight"].Value);
				vd2.CityId = Conversion.ToInt(r.Cells["CityId"].Value);
				vd2.RemarksDetail = Conversion.ToString(r.Cells["RemarksDetail"].Value);
				vd2.ActionTypeId = 3;
				lstRemoveRecord.Add(vd2);
				table.Rows[r.RowIndex].Delete();
				if (r != null && (int)r.RowType == 0)
				{
					r.Delete();
				}
				grd.UpdateData();
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				grd_DoubleClick(null, null);
			}
			if (e.KeyCode == Keys.F1)
			{
				int Id = 0;
				string Name = "";
				if (grd.CurrentColumn != null && (grd.CurrentColumn.Key == "WareHouseId" || grd.CurrentColumn.Key == "WareHouse"))
				{
					GrdPopUp grdPopUp = new GrdPopUp(dtWh, "Id", "WareHouseName");
					grdPopUp.StartPosition = FormStartPosition.CenterScreen;
					grdPopUp.ShowDialog();
					grdPopUp.BringToFront();
					Id = Conversion.ToInt((object)grdPopUp.ReturnId);
					Name = Conversion.ToString((object)grdPopUp.ReturnName);
					r.Cells["WareHouseId"].Value = Id;
					r.Cells["WareHouse"].Value = Name;
					int OrganizationId2 = UserAccount.OrganizationId;
					int CompanyId2 = UserAccount.CompanyId;
					int ItemId2 = Conversion.ToInt((object)r.Cells["ItemId"].Value.ToString());
					int wareId2 = Conversion.ToInt((object)r.Cells["WareHouseId"].Value.ToString());
					int JobLotId2 = Conversion.ToInt((object)r.Cells["JobLotId"].Value.ToString());
					int AttributeVarientId2 = Conversion.ToInt((object)r.Cells["AttributeVarientId"].Value.ToString());
					if (ItemId2 > 0 || wareId2 > 0)
					{
						DataTable DtStock2 = GetAvgRatesAndStockInHand.GetStockInHandAndAvgRateFromEvaluationConcrete(OrganizationId2, CompanyId2, ItemId2, Conversion.ToDateTime((object)DocDate.Value), wareId2, JobLotId2, AttributeVarientId2);
						if (DtStock2.Rows.Count > 0)
						{
							r.Cells["AvailableQty"].Value = Conversion.ToDecimal(DtStock2.Rows[0]["QtyInHand"]).ToString();
						}
						else
						{
							r.Cells["AvailableQty"].Value = 0.ToString();
						}
					}
					else
					{
						r.Cells["AvailableQty"].Value = 0.ToString();
					}
				}
				if (grd.CurrentColumn != null && (grd.CurrentColumn.Key == "JobLotId" || grd.CurrentColumn.Key == "JobLot"))
				{
					GrdPopUp grdPopUp2 = new GrdPopUp(dtJobLoat, "Id", "JobLotDescription");
					grdPopUp2.StartPosition = FormStartPosition.CenterScreen;
					grdPopUp2.ShowDialog();
					grdPopUp2.BringToFront();
					Id = Conversion.ToInt((object)grdPopUp2.ReturnId);
					Name = Conversion.ToString((object)grdPopUp2.ReturnName);
					r.Cells["JobLotId"].Value = Id;
					r.Cells["JobLot"].Value = Name;
					int OrganizationId = UserAccount.OrganizationId;
					int CompanyId = UserAccount.CompanyId;
					int ItemId = Conversion.ToInt((object)r.Cells["ItemId"].Value.ToString());
					int wareId = Conversion.ToInt((object)r.Cells["WareHouseId"].Value.ToString());
					int JobLotId = Conversion.ToInt((object)r.Cells["JobLotId"].Value.ToString());
					int AttributeVarientId = Conversion.ToInt((object)r.Cells["AttributeVarientId"].Value.ToString());
					if (ItemId > 0 || wareId > 0)
					{
						DataTable DtStock = GetAvgRatesAndStockInHand.GetStockInHandAndAvgRateFromEvaluationConcrete(OrganizationId, CompanyId, ItemId, Conversion.ToDateTime((object)DocDate.Value), wareId, JobLotId, AttributeVarientId);
						if (DtStock.Rows.Count > 0)
						{
							r.Cells["AvailableQty"].Value = Conversion.ToDecimal(DtStock.Rows[0]["QtyInHand"]).ToString();
						}
						else
						{
							r.Cells["AvailableQty"].Value = 0.ToString();
						}
					}
					else
					{
						r.Cells["AvailableQty"].Value = 0.ToString();
					}
				}
				if (grd.CurrentColumn != null && (grd.CurrentColumn.Key == "CityName" || grd.CurrentColumn.Key == "CityId"))
				{
					GrdPopUp grdPopUp3 = new GrdPopUp(dtCity, "Id", "CityName");
					grdPopUp3.StartPosition = FormStartPosition.CenterScreen;
					grdPopUp3.ShowDialog();
					grdPopUp3.BringToFront();
					Id = Conversion.ToInt((object)grdPopUp3.ReturnId);
					Name = Conversion.ToString((object)grdPopUp3.ReturnName);
					r.Cells["CityId"].Value = Id;
					r.Cells["CityName"].Value = Name;
				}
				grd.UpdateData();
				CommonServices.GridAutoAdjustmentNew(grd);
			}
			((Control)(object)grd).Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdhistory.CurrentRow;
			if (item == null || (int)item.RowType != 0 || grdhistory.CurrentColumn == null)
			{
				return;
			}
			int CurrId = Conversion.ToInt((object)grdhistory.CurrentRow.Cells["Id"].Value.ToString());
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdhistory.CurrentColumn.Key == "Edit" && formright.DoHaveUpdateRights)
				{
					Id = CurrId;
					getUpdate(Id);
					DocDate.Focus();
				}
				if (grdhistory.CurrentColumn.Key == "Print")
				{
					CommonServices.DeliveryOrderConcreteSlip(CurrId);
				}
				if (grdhistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(CurrId, 1853);
				}
			}
			if (e.Control && e.KeyCode == Keys.Return && formright.DoHaveUpdateRights)
			{
				Id = CurrId;
				getUpdate(Id);
				DocDate.Focus();
			}
			if (e.Control && e.KeyCode == Keys.P && formright.DoHavePrintRights)
			{
				CommonServices.DeliveryOrderConcreteSlip(CurrId);
			}
			((Control)(object)grdhistory).Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdHistoryDetail_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = GrdHistoryDetail.CurrentRow;
			if (item != null && (int)item.RowType == 0 && GrdHistoryDetail.CurrentColumn != null)
			{
				int CurrOrderId = Conversion.ToInt((object)GrdHistoryDetail.CurrentRow.Cells["Id"].Value.ToString());
				if (e.Control && e.KeyCode == Keys.Space && GrdHistoryDetail.CurrentColumn.Key == "OrderNo")
				{
					CommonServices.SaleOrderSlipConcrete(CurrOrderId);
				}
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
		//IL_0696: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a0: Expected O, but got Unknown
		//IL_06d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e2: Expected O, but got Unknown
		//IL_0730: Unknown result type (might be due to invalid IL or missing references)
		//IL_073a: Expected O, but got Unknown
		//IL_0788: Unknown result type (might be due to invalid IL or missing references)
		//IL_0792: Expected O, but got Unknown
		//IL_079e: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a8: Expected O, but got Unknown
		//IL_07ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d4: Expected O, but got Unknown
		//IL_07f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0800: Expected O, but got Unknown
		//IL_080c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0816: Expected O, but got Unknown
		//IL_0822: Unknown result type (might be due to invalid IL or missing references)
		//IL_082c: Expected O, but got Unknown
		//IL_0885: Unknown result type (might be due to invalid IL or missing references)
		//IL_088f: Expected O, but got Unknown
		//IL_0890: Unknown result type (might be due to invalid IL or missing references)
		//IL_089a: Expected O, but got Unknown
		//IL_08f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_08fd: Expected O, but got Unknown
		//IL_08fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0908: Expected O, but got Unknown
		//IL_0998: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a2: Expected O, but got Unknown
		//IL_09c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ce: Expected O, but got Unknown
		//IL_09da: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e4: Expected O, but got Unknown
		//IL_0aed: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af7: Expected O, but got Unknown
		//IL_0b0e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b18: Expected O, but got Unknown
		//IL_0b5b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b65: Expected O, but got Unknown
		//IL_0b87: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b91: Expected O, but got Unknown
		//IL_0b92: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b9c: Expected O, but got Unknown
		//IL_0ba8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bb2: Expected O, but got Unknown
		//IL_125a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1264: Expected O, but got Unknown
		//IL_1271: Unknown result type (might be due to invalid IL or missing references)
		//IL_127b: Expected O, but got Unknown
		//IL_857e: Unknown result type (might be due to invalid IL or missing references)
		//IL_8588: Expected O, but got Unknown
		//IL_87cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_87d7: Expected O, but got Unknown
		//IL_87fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_8805: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance173 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance180 = new Appearance();
		Appearance appearance181 = new Appearance();
		Appearance appearance182 = new Appearance();
		Appearance appearance183 = new Appearance();
		Appearance appearance184 = new Appearance();
		Appearance appearance186 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance136 = new Appearance();
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
		Appearance appearance111 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance210 = new Appearance();
		Appearance appearance211 = new Appearance();
		Appearance appearance212 = new Appearance();
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
		Appearance appearance46 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
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
		Appearance appearance223 = new Appearance();
		Appearance appearance224 = new Appearance();
		Appearance appearance225 = new Appearance();
		Appearance appearance200 = new Appearance();
		Appearance appearance201 = new Appearance();
		Appearance appearance202 = new Appearance();
		Appearance appearance203 = new Appearance();
		Appearance appearance204 = new Appearance();
		Appearance appearance205 = new Appearance();
		Appearance appearance206 = new Appearance();
		Appearance appearance208 = new Appearance();
		Appearance appearance209 = new Appearance();
		Appearance appearance157 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance185 = new Appearance();
		Appearance appearance196 = new Appearance();
		Appearance appearance207 = new Appearance();
		Appearance appearance218 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance187 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance190 = new Appearance();
		Appearance appearance191 = new Appearance();
		Appearance appearance192 = new Appearance();
		Appearance appearance193 = new Appearance();
		Appearance appearance194 = new Appearance();
		Appearance appearance195 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance168 = new Appearance();
		Appearance appearance169 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.pcc.Sale.DeliveryOrderConcrete));
		GridEXLayout GrdHistoryDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdhistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance197 = new Appearance();
		Appearance appearance198 = new Appearance();
		Appearance appearance199 = new Appearance();
		Appearance appearance213 = new Appearance();
		Appearance appearance214 = new Appearance();
		Appearance appearance215 = new Appearance();
		Appearance appearance216 = new Appearance();
		Appearance appearance217 = new Appearance();
		Appearance appearance219 = new Appearance();
		Appearance appearance220 = new Appearance();
		Appearance appearance221 = new Appearance();
		Appearance appearance222 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance171 = new Appearance();
		Appearance appearance172 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance188 = new Appearance();
		Appearance appearance189 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance226 = new Appearance();
		Appearance appearance227 = new Appearance();
		Appearance appearance228 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance153 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.DetailGridPanel = new System.Windows.Forms.TabControl();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.grd = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label15 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.Cmbheight = new UltraCombo();
		this.rdbtnItemCode = new System.Windows.Forms.RadioButton();
		this.txtArea = new System.Windows.Forms.TextBox();
		this.rdbtnItemName = new System.Windows.Forms.RadioButton();
		this.label21 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.chkisFreeOfCost = new System.Windows.Forms.CheckBox();
		this.label20 = new System.Windows.Forms.Label();
		this.Cmbstoreys = new UltraCombo();
		this.label70 = new System.Windows.Forms.Label();
		this.btnplus = new System.Windows.Forms.Button();
		this.BtnDefineCity = new System.Windows.Forms.Button();
		this.label22 = new System.Windows.Forms.Label();
		this.label69 = new System.Windows.Forms.Label();
		this.txtRefPartyCellNo = new System.Windows.Forms.TextBox();
		this.txtRefPartyAddress = new System.Windows.Forms.TextBox();
		this.combcityarea = new UltraCombo();
		this.label19 = new System.Windows.Forms.Label();
		this.CmbReferenceParty = new UltraCombo();
		this.lblBalance = new System.Windows.Forms.Label();
		this.label47 = new System.Windows.Forms.Label();
		this.txtremarksdetail = new System.Windows.Forms.TextBox();
		this.CmbOrderNo = new UltraCombo();
		this.label9 = new System.Windows.Forms.Label();
		this.txtNetWeight = new System.Windows.Forms.TextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.CmbJobLot = new UltraCombo();
		this.label8 = new System.Windows.Forms.Label();
		this.CmbWareHouse = new UltraCombo();
		this.label3 = new System.Windows.Forms.Label();
		this.CmbSupplierCustomer = new UltraCombo();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.txtItemWeight = new System.Windows.Forms.TextBox();
		this.btnCancelUpdateDetial = new System.Windows.Forms.Button();
		this.label39 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.txtVarientEquivalent = new System.Windows.Forms.TextBox();
		this.txtqty = new System.Windows.Forms.TextBox();
		this.label38 = new System.Windows.Forms.Label();
		this.CmbAttributeVarient = new UltraCombo();
		this.CmbItemName = new UltraCombo();
		this.label17 = new System.Windows.Forms.Label();
		this.label36 = new System.Windows.Forms.Label();
		this.label37 = new System.Windows.Forms.Label();
		this.txtBalWeight = new System.Windows.Forms.TextBox();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.txtDistance = new System.Windows.Forms.TextBox();
		this.label60 = new System.Windows.Forms.Label();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.CmbVehicleNo = new UltraCombo();
		this.CmbVehicleType = new UltraCombo();
		this.label24 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.DocDate = new System.Windows.Forms.DateTimePicker();
		this.txtReferenceNoHeader = new System.Windows.Forms.TextBox();
		this.txtRemarksHeader = new System.Windows.Forms.TextBox();
		this.label11 = new System.Windows.Forms.Label();
		this.txtSubNoId = new System.Windows.Forms.TextBox();
		this.txtBalQty = new System.Windows.Forms.TextBox();
		this.label13 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.cmbBranchFrom = new UltraCombo();
		this.label10 = new System.Windows.Forms.Label();
		this.txtDocNoId = new System.Windows.Forms.TextBox();
		this.txtRefDocTypeId = new System.Windows.Forms.TextBox();
		this.cmbBranchTo = new UltraCombo();
		this.label4 = new System.Windows.Forms.Label();
		this.CmbDeliveryOrderType = new UltraCombo();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.chkPrintCustomerWise = new System.Windows.Forms.CheckBox();
		this.ChkPrint = new System.Windows.Forms.CheckBox();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
		this.btnattachment = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
		this.btnPrintSlip = new System.Windows.Forms.ToolStripButton();
		this.btnPrintCustomerWise = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.btnLoadSaleOrder = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.btnDefineReferenceParties = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.btnshortcutkeys = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.GrdHistoryDetail = new GridEX();
		this.panel10 = new System.Windows.Forms.Panel();
		this.label27 = new System.Windows.Forms.Label();
		this.grdhistory = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label1 = new System.Windows.Forms.Label();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel12 = new System.Windows.Forms.Panel();
		this.groupBox4 = new System.Windows.Forms.GroupBox();
		this.btnShowHistory = new System.Windows.Forms.Button();
		this.CmbItemHistory = new UltraCombo();
		this.label72 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.label73 = new System.Windows.Forms.Label();
		this.CmbVehicleNoHistory = new UltraCombo();
		this.CmbCustomerHistory = new UltraCombo();
		this.label75 = new System.Windows.Forms.Label();
		this.cmbDateTypeHistory = new UltraCombo();
		this.txtToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.label76 = new System.Windows.Forms.Label();
		this.txtToDocNoHistory = new System.Windows.Forms.TextBox();
		this.txtFromdateHistory = new System.Windows.Forms.DateTimePicker();
		this.label77 = new System.Windows.Forms.Label();
		this.txtFromNoHistory = new System.Windows.Forms.TextBox();
		this.panel8 = new System.Windows.Forms.Panel();
		this.label16 = new System.Windows.Forms.Label();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.BtnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.DetailGridPanel.SuspendLayout();
		this.tabPage3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel5.SuspendLayout();
		this.panel4.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.Cmbheight).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.Cmbstoreys).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combcityarea).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbReferenceParty).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbOrderNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWareHouse).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplierCustomer).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAttributeVarient).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).BeginInit();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbVehicleNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbVehicleType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchFrom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchTo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveryOrderType).BeginInit();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdHistoryDetail).BeginInit();
		this.panel10.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdhistory).BeginInit();
		this.panel6.SuspendLayout();
		this.panel12.SuspendLayout();
		this.groupBox4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbItemHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbVehicleNoHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDateTypeHistory).BeginInit();
		this.panel8.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1185, 725);
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
		this.tabControl1.Size = new System.Drawing.Size(1185, 725);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.DetailGridPanel);
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage1.Size = new System.Drawing.Size(1177, 696);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.DetailGridPanel.Controls.Add(this.tabPage3);
		this.DetailGridPanel.Dock = System.Windows.Forms.DockStyle.Fill;
		this.DetailGridPanel.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DetailGridPanel.Location = new System.Drawing.Point(2, 288);
		this.DetailGridPanel.Name = "DetailGridPanel";
		this.DetailGridPanel.SelectedIndex = 0;
		this.DetailGridPanel.Size = new System.Drawing.Size(1173, 406);
		this.DetailGridPanel.TabIndex = 6;
		this.tabPage3.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.tabPage3.Controls.Add(this.panel5);
		this.tabPage3.Location = new System.Drawing.Point(4, 25);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage3.Size = new System.Drawing.Size(1165, 377);
		this.tabPage3.TabIndex = 0;
		this.tabPage3.Text = "Do Detail";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.grd.AutomaticSort = false;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		this.grd.ColumnAutoResize = true;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(3, 27);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1159, 347);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.CellUpdated += new ColumnActionEventHandler(grd_CellUpdated);
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grd).Click += new System.EventHandler(grd_Click);
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grd_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		this.panel5.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel5.Controls.Add(this.ctrlGrdBar1);
		this.panel5.Controls.Add(this.label15);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(3, 3);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1159, 24);
		this.panel5.TabIndex = 2;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1119, 1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 20;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Tahoma", 11f, System.Drawing.FontStyle.Bold);
		this.label15.ForeColor = System.Drawing.Color.White;
		this.label15.Location = new System.Drawing.Point(8, 3);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(53, 18);
		this.label15.TabIndex = 0;
		this.label15.Text = "Detail";
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.groupBox3);
		this.panel4.Controls.Add(this.txtBalWeight);
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Controls.Add(this.label11);
		this.panel4.Controls.Add(this.txtSubNoId);
		this.panel4.Controls.Add(this.txtBalQty);
		this.panel4.Controls.Add(this.label13);
		this.panel4.Controls.Add(this.label18);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbBranchFrom);
		this.panel4.Controls.Add(this.label10);
		this.panel4.Controls.Add(this.txtDocNoId);
		this.panel4.Controls.Add(this.txtRefDocTypeId);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbBranchTo);
		this.panel4.Controls.Add(this.label4);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDeliveryOrderType);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(2, 51);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1173, 237);
		this.panel4.TabIndex = 2;
		this.groupBox3.BackColor = System.Drawing.Color.White;
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.Cmbheight);
		this.groupBox3.Controls.Add(this.rdbtnItemCode);
		this.groupBox3.Controls.Add(this.txtArea);
		this.groupBox3.Controls.Add(this.rdbtnItemName);
		this.groupBox3.Controls.Add(this.label21);
		this.groupBox3.Controls.Add(this.label7);
		this.groupBox3.Controls.Add(this.chkisFreeOfCost);
		this.groupBox3.Controls.Add(this.label20);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.Cmbstoreys);
		this.groupBox3.Controls.Add(this.label70);
		this.groupBox3.Controls.Add(this.btnplus);
		this.groupBox3.Controls.Add(this.BtnDefineCity);
		this.groupBox3.Controls.Add(this.label22);
		this.groupBox3.Controls.Add(this.label69);
		this.groupBox3.Controls.Add(this.txtRefPartyCellNo);
		this.groupBox3.Controls.Add(this.txtRefPartyAddress);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.combcityarea);
		this.groupBox3.Controls.Add(this.label19);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbReferenceParty);
		this.groupBox3.Controls.Add(this.lblBalance);
		this.groupBox3.Controls.Add(this.label47);
		this.groupBox3.Controls.Add(this.txtremarksdetail);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbOrderNo);
		this.groupBox3.Controls.Add(this.label9);
		this.groupBox3.Controls.Add(this.txtNetWeight);
		this.groupBox3.Controls.Add(this.label6);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobLot);
		this.groupBox3.Controls.Add(this.label8);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWareHouse);
		this.groupBox3.Controls.Add(this.label3);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSupplierCustomer);
		this.groupBox3.Controls.Add(this.btnUpdateDetail);
		this.groupBox3.Controls.Add(this.txtItemWeight);
		this.groupBox3.Controls.Add(this.btnCancelUpdateDetial);
		this.groupBox3.Controls.Add(this.label39);
		this.groupBox3.Controls.Add(this.label2);
		this.groupBox3.Controls.Add(this.txtVarientEquivalent);
		this.groupBox3.Controls.Add(this.txtqty);
		this.groupBox3.Controls.Add(this.label38);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbAttributeVarient);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemName);
		this.groupBox3.Controls.Add(this.label17);
		this.groupBox3.Controls.Add(this.label36);
		this.groupBox3.Controls.Add(this.label37);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(3, 74);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(1162, 159);
		this.groupBox3.TabIndex = 1;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Detail";
		this.Cmbheight.AllowNull = (DefaultableBoolean)1;
		this.Cmbheight.AutoCompleteMode = (AutoCompleteMode)4;
		this.Cmbheight.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.Cmbheight.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance173).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance173).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.Cmbheight).DisplayLayout.Appearance = (AppearanceBase)(object)appearance173;
		((UltraGridBase)this.Cmbheight).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.Cmbheight).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance175).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance175).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance175).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.Cmbheight).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance175;
		((AppearanceBase)appearance176).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.Cmbheight).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance176;
		((SpecialBoxBase)((UltraGridBase)this.Cmbheight).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance177).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance177).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance177).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.Cmbheight).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance177;
		((UltraGridBase)this.Cmbheight).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.Cmbheight).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance178).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.Cmbheight).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance178;
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance179).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.Cmbheight).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance179;
		((UltraGridBase)this.Cmbheight).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.Cmbheight).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance180).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.Cmbheight).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance180;
		((AppearanceBase)appearance181).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance181).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.Cmbheight).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance181;
		((UltraGridBase)this.Cmbheight).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.Cmbheight).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance182).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance182).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance182).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance182).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance182).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.Cmbheight).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance182;
		((AppearanceBase)appearance183).TextHAlignAsString = "Left";
		((UltraGridBase)this.Cmbheight).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance183;
		((UltraGridBase)this.Cmbheight).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.Cmbheight).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance184).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance184).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.Cmbheight).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance184;
		((UltraGridBase)this.Cmbheight).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance186).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.Cmbheight).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance186;
		((UltraGridBase)this.Cmbheight).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.Cmbheight).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.Cmbheight).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.Cmbheight).Enabled = false;
		((System.Windows.Forms.Control)(object)this.Cmbheight).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.Cmbheight.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.Cmbheight).Location = new System.Drawing.Point(920, 35);
		((System.Windows.Forms.Control)(object)this.Cmbheight).Name = "Cmbheight";
		((System.Windows.Forms.Control)(object)this.Cmbheight).Size = new System.Drawing.Size(149, 26);
		((System.Windows.Forms.Control)(object)this.Cmbheight).TabIndex = 6;
		this.rdbtnItemCode.AutoSize = true;
		this.rdbtnItemCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdbtnItemCode.Location = new System.Drawing.Point(275, 64);
		this.rdbtnItemCode.Margin = new System.Windows.Forms.Padding(2);
		this.rdbtnItemCode.Name = "rdbtnItemCode";
		this.rdbtnItemCode.Size = new System.Drawing.Size(52, 19);
		this.rdbtnItemCode.TabIndex = 129;
		this.rdbtnItemCode.Text = "Code";
		this.rdbtnItemCode.UseVisualStyleBackColor = true;
		this.txtArea.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtArea.Enabled = false;
		this.txtArea.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtArea.Location = new System.Drawing.Point(1071, 37);
		this.txtArea.Name = "txtArea";
		this.txtArea.Size = new System.Drawing.Size(90, 23);
		this.txtArea.TabIndex = 7;
		this.txtArea.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.rdbtnItemName.AutoSize = true;
		this.rdbtnItemName.Checked = true;
		this.rdbtnItemName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdbtnItemName.Location = new System.Drawing.Point(221, 64);
		this.rdbtnItemName.Margin = new System.Windows.Forms.Padding(2);
		this.rdbtnItemName.Name = "rdbtnItemName";
		this.rdbtnItemName.Size = new System.Drawing.Size(57, 19);
		this.rdbtnItemName.TabIndex = 128;
		this.rdbtnItemName.TabStop = true;
		this.rdbtnItemName.Text = "Name";
		this.rdbtnItemName.UseVisualStyleBackColor = true;
		this.rdbtnItemName.CheckedChanged += new System.EventHandler(rdbtnItemName_CheckedChanged);
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(569, 17);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(49, 15);
		this.label21.TabIndex = 50;
		this.label21.Text = "Address";
		this.label7.AutoSize = true;
		this.label7.BackColor = System.Drawing.Color.White;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(1071, 17);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(78, 15);
		this.label7.TabIndex = 52;
		this.label7.Text = "Building Area";
		this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.chkisFreeOfCost.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.chkisFreeOfCost.AutoSize = true;
		this.chkisFreeOfCost.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkisFreeOfCost.ForeColor = System.Drawing.Color.Black;
		this.chkisFreeOfCost.Location = new System.Drawing.Point(975, 110);
		this.chkisFreeOfCost.Name = "chkisFreeOfCost";
		this.chkisFreeOfCost.Size = new System.Drawing.Size(102, 19);
		this.chkisFreeOfCost.TabIndex = 19;
		this.chkisFreeOfCost.Text = "Is Free Of Cost";
		this.chkisFreeOfCost.UseVisualStyleBackColor = true;
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(450, 17);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(45, 15);
		this.label20.TabIndex = 49;
		this.label20.Text = "Cell No";
		this.Cmbstoreys.AllowNull = (DefaultableBoolean)1;
		this.Cmbstoreys.AutoCompleteMode = (AutoCompleteMode)4;
		this.Cmbstoreys.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.Cmbstoreys.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance124).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.Cmbstoreys).DisplayLayout.Appearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.Cmbstoreys).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.Cmbstoreys).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance125).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance125).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance125).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.Cmbstoreys).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance125;
		((AppearanceBase)appearance126).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.Cmbstoreys).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance126;
		((SpecialBoxBase)((UltraGridBase)this.Cmbstoreys).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance127).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance127).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance127).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.Cmbstoreys).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.Cmbstoreys).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.Cmbstoreys).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance129).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.Cmbstoreys).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance129;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance130).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.Cmbstoreys).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.Cmbstoreys).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.Cmbstoreys).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.Cmbstoreys).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance131;
		((AppearanceBase)appearance132).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance132).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.Cmbstoreys).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.Cmbstoreys).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.Cmbstoreys).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance133).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance133).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance133).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.Cmbstoreys).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance133;
		((AppearanceBase)appearance134).TextHAlignAsString = "Left";
		((UltraGridBase)this.Cmbstoreys).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance134;
		((UltraGridBase)this.Cmbstoreys).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.Cmbstoreys).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance135).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.Cmbstoreys).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.Cmbstoreys).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.Cmbstoreys).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.Cmbstoreys).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.Cmbstoreys).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.Cmbstoreys).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.Cmbstoreys).Enabled = false;
		((System.Windows.Forms.Control)(object)this.Cmbstoreys).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.Cmbstoreys.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.Cmbstoreys).Location = new System.Drawing.Point(769, 35);
		((System.Windows.Forms.Control)(object)this.Cmbstoreys).Name = "Cmbstoreys";
		((System.Windows.Forms.Control)(object)this.Cmbstoreys).Size = new System.Drawing.Size(149, 26);
		((System.Windows.Forms.Control)(object)this.Cmbstoreys).TabIndex = 5;
		this.label70.AutoSize = true;
		this.label70.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label70.Location = new System.Drawing.Point(920, 17);
		this.label70.Name = "label70";
		this.label70.Size = new System.Drawing.Size(90, 15);
		this.label70.TabIndex = 50;
		this.label70.Text = "Building Height";
		this.btnplus.BackColor = System.Drawing.Color.Teal;
		this.btnplus.FlatAppearance.BorderSize = 0;
		this.btnplus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnplus.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnplus.ForeColor = System.Drawing.Color.White;
		this.btnplus.Location = new System.Drawing.Point(973, 130);
		this.btnplus.Name = "btnplus";
		this.btnplus.Size = new System.Drawing.Size(39, 23);
		this.btnplus.TabIndex = 20;
		this.btnplus.Text = "+";
		this.btnplus.UseVisualStyleBackColor = false;
		this.btnplus.Click += new System.EventHandler(btnplus_Click);
		this.BtnDefineCity.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.BtnDefineCity.Location = new System.Drawing.Point(1128, 83);
		this.BtnDefineCity.Name = "BtnDefineCity";
		this.BtnDefineCity.Size = new System.Drawing.Size(29, 26);
		this.BtnDefineCity.TabIndex = 17;
		this.BtnDefineCity.TabStop = false;
		this.BtnDefineCity.UseVisualStyleBackColor = true;
		this.BtnDefineCity.Click += new System.EventHandler(BtnDefineCity_Click);
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(279, 17);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(54, 15);
		this.label22.TabIndex = 51;
		this.label22.Text = "Ref Party";
		this.label69.AutoSize = true;
		this.label69.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label69.Location = new System.Drawing.Point(769, 17);
		this.label69.Name = "label69";
		this.label69.Size = new System.Drawing.Size(93, 15);
		this.label69.TabIndex = 51;
		this.label69.Text = "Building Storeys";
		this.txtRefPartyCellNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRefPartyCellNo.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtRefPartyCellNo.Location = new System.Drawing.Point(448, 37);
		this.txtRefPartyCellNo.Name = "txtRefPartyCellNo";
		this.txtRefPartyCellNo.Size = new System.Drawing.Size(120, 23);
		this.txtRefPartyCellNo.TabIndex = 3;
		this.txtRefPartyAddress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRefPartyAddress.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtRefPartyAddress.Location = new System.Drawing.Point(569, 37);
		this.txtRefPartyAddress.Name = "txtRefPartyAddress";
		this.txtRefPartyAddress.Size = new System.Drawing.Size(198, 23);
		this.txtRefPartyAddress.TabIndex = 4;
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
		((System.Windows.Forms.Control)(object)this.combcityarea).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.combcityarea.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combcityarea).Location = new System.Drawing.Point(973, 83);
		((System.Windows.Forms.Control)(object)this.combcityarea).Name = "combcityarea";
		((System.Windows.Forms.Control)(object)this.combcityarea).Size = new System.Drawing.Size(153, 26);
		((System.Windows.Forms.Control)(object)this.combcityarea).TabIndex = 16;
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label19.Location = new System.Drawing.Point(973, 66);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(56, 15);
		this.label19.TabIndex = 49;
		this.label19.Text = "City/Area";
		this.CmbReferenceParty.AllowNull = (DefaultableBoolean)1;
		this.CmbReferenceParty.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbReferenceParty.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbReferenceParty.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance111).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbReferenceParty).DisplayLayout.Appearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.CmbReferenceParty).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbReferenceParty).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbReferenceParty).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance113).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbReferenceParty).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance113;
		((SpecialBoxBase)((UltraGridBase)this.CmbReferenceParty).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance114).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance114).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance114).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbReferenceParty).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.CmbReferenceParty).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbReferenceParty).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance115).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbReferenceParty).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance116).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbReferenceParty).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.CmbReferenceParty).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbReferenceParty).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbReferenceParty).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance118;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance119).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbReferenceParty).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.CmbReferenceParty).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbReferenceParty).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance120).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance120).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance120).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance120).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbReferenceParty).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance121).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbReferenceParty).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.CmbReferenceParty).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbReferenceParty).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance122).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbReferenceParty).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.CmbReferenceParty).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbReferenceParty).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.CmbReferenceParty).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbReferenceParty).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbReferenceParty).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbReferenceParty).Font = new System.Drawing.Font("Verdana", 9.75f);
		((System.Windows.Forms.Control)(object)this.CmbReferenceParty).Location = new System.Drawing.Point(279, 35);
		((System.Windows.Forms.Control)(object)this.CmbReferenceParty).Name = "CmbReferenceParty";
		((System.Windows.Forms.Control)(object)this.CmbReferenceParty).Size = new System.Drawing.Size(167, 26);
		((System.Windows.Forms.Control)(object)this.CmbReferenceParty).TabIndex = 2;
		this.lblBalance.AutoSize = true;
		this.lblBalance.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblBalance.Location = new System.Drawing.Point(332, 66);
		this.lblBalance.Name = "lblBalance";
		this.lblBalance.Size = new System.Drawing.Size(14, 15);
		this.lblBalance.TabIndex = 46;
		this.lblBalance.Text = "0";
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label47.Location = new System.Drawing.Point(2, 112);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(65, 15);
		this.label47.TabIndex = 25;
		this.label47.Text = "Comments";
		this.txtremarksdetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtremarksdetail.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtremarksdetail.Location = new System.Drawing.Point(2, 130);
		this.txtremarksdetail.Name = "txtremarksdetail";
		this.txtremarksdetail.Size = new System.Drawing.Size(968, 23);
		this.txtremarksdetail.TabIndex = 18;
		this.CmbOrderNo.AllowNull = (DefaultableBoolean)1;
		this.CmbOrderNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbOrderNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbOrderNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance79).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance79).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance79).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbOrderNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance90;
		((SpecialBoxBase)((UltraGridBase)this.CmbOrderNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance101).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance101).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance101).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance109).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance109;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance110).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance117;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance128).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance137).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance137).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance137).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance137).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance137;
		((AppearanceBase)appearance210).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance210;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance211).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance211).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance211;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance212).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance212;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbOrderNo).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbOrderNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbOrderNo).Location = new System.Drawing.Point(188, 35);
		((System.Windows.Forms.Control)(object)this.CmbOrderNo).Name = "CmbOrderNo";
		((System.Windows.Forms.Control)(object)this.CmbOrderNo).Size = new System.Drawing.Size(89, 26);
		((System.Windows.Forms.Control)(object)this.CmbOrderNo).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.CmbOrderNo).TextChanged += new System.EventHandler(CmbOrderNo_TextChanged);
		((System.Windows.Forms.Control)(object)this.CmbOrderNo).Leave += new System.EventHandler(CmbOrderNo_Leave);
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(188, 17);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(53, 15);
		this.label9.TabIndex = 43;
		this.label9.Text = "OrderNo";
		this.txtNetWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNetWeight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtNetWeight.Location = new System.Drawing.Point(872, 85);
		this.txtNetWeight.Name = "txtNetWeight";
		this.txtNetWeight.ReadOnly = true;
		this.txtNetWeight.Size = new System.Drawing.Size(98, 23);
		this.txtNetWeight.TabIndex = 15;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(874, 66);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(96, 15);
		this.label6.TabIndex = 39;
		this.label6.Text = "Item Net Weight";
		this.CmbJobLot.AllowNull = (DefaultableBoolean)1;
		this.CmbJobLot.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobLot.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobLot.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Appearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance54;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance60).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance61).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbJobLot.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Location = new System.Drawing.Point(549, 83);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Name = "CmbJobLot";
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Size = new System.Drawing.Size(154, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).TabIndex = 12;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).TextChanged += new System.EventHandler(CmbJobLot_TextChanged);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Leave += new System.EventHandler(CmbJobLot_Leave);
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(549, 66);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(43, 15);
		this.label8.TabIndex = 35;
		this.label8.Text = "JobLot";
		this.CmbWareHouse.AllowNull = (DefaultableBoolean)1;
		this.CmbWareHouse.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWareHouse.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWareHouse.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Appearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance57).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance57).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance57).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWareHouse).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance57;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance68;
		((SpecialBoxBase)((UltraGridBase)this.CmbWareHouse).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance138).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance138).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance138).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance139).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance139;
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance140).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance141;
		((AppearanceBase)appearance142).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance142).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance149).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance149).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance149).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance149).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance149;
		((AppearanceBase)appearance154).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance154;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance155).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance156;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbWareHouse.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).Location = new System.Drawing.Point(2, 84);
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).Name = "CmbWareHouse";
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).Size = new System.Drawing.Size(183, 26);
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).TabIndex = 8;
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).TextChanged += new System.EventHandler(CmbWareHouse_TextChanged);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(2, 66);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(69, 15);
		this.label3.TabIndex = 33;
		this.label3.Text = "WareHouse";
		this.CmbSupplierCustomer.AllowNull = (DefaultableBoolean)1;
		this.CmbSupplierCustomer.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSupplierCustomer.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSupplierCustomer.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Appearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance67;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance73).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance74).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSupplierCustomer).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbSupplierCustomer.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSupplierCustomer).Location = new System.Drawing.Point(2, 35);
		((System.Windows.Forms.Control)(object)this.CmbSupplierCustomer).Name = "CmbSupplierCustomer";
		((System.Windows.Forms.Control)(object)this.CmbSupplierCustomer).Size = new System.Drawing.Size(183, 26);
		((System.Windows.Forms.Control)(object)this.CmbSupplierCustomer).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbSupplierCustomer).TextChanged += new System.EventHandler(CmbSupplierCustomer_TextChanged);
		((System.Windows.Forms.Control)(object)this.CmbSupplierCustomer).Leave += new System.EventHandler(CmbSupplierCustomer_Leave);
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatAppearance.BorderSize = 0;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(976, 130);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(68, 23);
		this.btnUpdateDetail.TabIndex = 20;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Visible = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.txtItemWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemWeight.Enabled = false;
		this.txtItemWeight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtItemWeight.Location = new System.Drawing.Point(780, 85);
		this.txtItemWeight.Name = "txtItemWeight";
		this.txtItemWeight.Size = new System.Drawing.Size(91, 23);
		this.txtItemWeight.TabIndex = 14;
		this.btnCancelUpdateDetial.BackColor = System.Drawing.Color.Teal;
		this.btnCancelUpdateDetial.FlatAppearance.BorderSize = 0;
		this.btnCancelUpdateDetial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelUpdateDetial.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelUpdateDetial.ForeColor = System.Drawing.Color.White;
		this.btnCancelUpdateDetial.Location = new System.Drawing.Point(1046, 130);
		this.btnCancelUpdateDetial.Name = "btnCancelUpdateDetial";
		this.btnCancelUpdateDetial.Size = new System.Drawing.Size(65, 23);
		this.btnCancelUpdateDetial.TabIndex = 21;
		this.btnCancelUpdateDetial.Text = "Cancel";
		this.btnCancelUpdateDetial.UseVisualStyleBackColor = false;
		this.btnCancelUpdateDetial.Visible = false;
		this.btnCancelUpdateDetial.Click += new System.EventHandler(btnCancelUpdateDetial_Click);
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label39.Location = new System.Drawing.Point(784, 66);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(74, 15);
		this.label39.TabIndex = 15;
		this.label39.Text = "Item Weight";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(2, 17);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(93, 15);
		this.label2.TabIndex = 31;
		this.label2.Text = "Customer Name";
		this.txtVarientEquivalent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtVarientEquivalent.Enabled = false;
		this.txtVarientEquivalent.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtVarientEquivalent.Location = new System.Drawing.Point(474, 85);
		this.txtVarientEquivalent.Name = "txtVarientEquivalent";
		this.txtVarientEquivalent.Size = new System.Drawing.Size(73, 23);
		this.txtVarientEquivalent.TabIndex = 11;
		this.txtqty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtqty.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtqty.Location = new System.Drawing.Point(706, 85);
		this.txtqty.Name = "txtqty";
		this.txtqty.Size = new System.Drawing.Size(73, 23);
		this.txtqty.TabIndex = 13;
		this.txtqty.TextChanged += new System.EventHandler(txtqty_TextChanged);
		this.txtqty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtqty_KeyPress);
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.Location = new System.Drawing.Point(710, 66);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(54, 15);
		this.label38.TabIndex = 13;
		this.label38.Text = "Item Qty";
		this.CmbAttributeVarient.AllowNull = (DefaultableBoolean)1;
		this.CmbAttributeVarient.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbAttributeVarient.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbAttributeVarient.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance223).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance223).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Appearance = (AppearanceBase)(object)appearance223;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance224).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance224).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance224).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance224).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance224;
		((AppearanceBase)appearance225).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance225;
		((SpecialBoxBase)((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance200).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance200).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance200).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance200).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance200;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance201).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance201).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance201;
		((AppearanceBase)appearance202).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance202).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance202;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance203).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance203;
		((AppearanceBase)appearance204).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance204).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance204;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance205).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance205).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance205).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance205).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance205).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance205;
		((AppearanceBase)appearance206).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance206;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance208).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance208).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance208;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance209).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance209;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbAttributeVarient).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbAttributeVarient.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbAttributeVarient).Location = new System.Drawing.Point(383, 83);
		((System.Windows.Forms.Control)(object)this.CmbAttributeVarient).Name = "CmbAttributeVarient";
		((System.Windows.Forms.Control)(object)this.CmbAttributeVarient).Size = new System.Drawing.Size(89, 26);
		((System.Windows.Forms.Control)(object)this.CmbAttributeVarient).TabIndex = 10;
		((System.Windows.Forms.Control)(object)this.CmbAttributeVarient).TextChanged += new System.EventHandler(CmbPackUom_TextChanged);
		((System.Windows.Forms.Control)(object)this.CmbAttributeVarient).Leave += new System.EventHandler(combitempck_Leave);
		this.CmbItemName.AllowNull = (DefaultableBoolean)1;
		this.CmbItemName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance157).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.CmbItemName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance158).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance158).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance158).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance158;
		((AppearanceBase)appearance159).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance159;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance12).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance14).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance16;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance17).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance18).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance18).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance18).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance18;
		((AppearanceBase)appearance95).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance96).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbItemName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Location = new System.Drawing.Point(187, 84);
		((System.Windows.Forms.Control)(object)this.CmbItemName).Name = "CmbItemName";
		((System.Windows.Forms.Control)(object)this.CmbItemName).Size = new System.Drawing.Size(192, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemName).TabIndex = 9;
		this.CmbItemName.ValueChanged += new System.EventHandler(combitem_ValueChanged);
		((System.Windows.Forms.Control)(object)this.CmbItemName).Leave += new System.EventHandler(CmbItemName_Leave);
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(474, 66);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(62, 15);
		this.label17.TabIndex = 8;
		this.label17.Text = "Equivalent";
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(383, 66);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(54, 15);
		this.label36.TabIndex = 8;
		this.label36.Text = "Attribute";
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(187, 66);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(35, 15);
		this.label37.TabIndex = 9;
		this.label37.Text = "Item ";
		this.txtBalWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBalWeight.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBalWeight.Location = new System.Drawing.Point(1237, 115);
		this.txtBalWeight.Name = "txtBalWeight";
		this.txtBalWeight.ReadOnly = true;
		this.txtBalWeight.Size = new System.Drawing.Size(53, 25);
		this.txtBalWeight.TabIndex = 11;
		this.txtBalWeight.TabStop = false;
		this.txtBalWeight.Visible = false;
		this.groupBox1.Controls.Add(this.txtDistance);
		this.groupBox1.Controls.Add(this.label60);
		this.groupBox1.Controls.Add(this.txtdocno);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbVehicleNo);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbVehicleType);
		this.groupBox1.Controls.Add(this.label24);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add(this.label14);
		this.groupBox1.Controls.Add(this.label33);
		this.groupBox1.Controls.Add(this.label12);
		this.groupBox1.Controls.Add(this.DocDate);
		this.groupBox1.Controls.Add(this.txtReferenceNoHeader);
		this.groupBox1.Controls.Add(this.txtRemarksHeader);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(3, 1);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1162, 72);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Main";
		this.txtDistance.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDistance.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDistance.Location = new System.Drawing.Point(534, 44);
		this.txtDistance.Name = "txtDistance";
		this.txtDistance.Size = new System.Drawing.Size(120, 23);
		this.txtDistance.TabIndex = 5;
		this.txtDistance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label60.BackColor = System.Drawing.Color.LightGray;
		this.label60.Font = new System.Drawing.Font("Segoe UI Semibold", 11f, System.Drawing.FontStyle.Bold);
		this.label60.Location = new System.Drawing.Point(534, 16);
		this.label60.Name = "label60";
		this.label60.Size = new System.Drawing.Size(120, 23);
		this.label60.TabIndex = 52;
		this.label60.Text = "Distance";
		this.label60.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtdocno.BackColor = System.Drawing.Color.White;
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Enabled = false;
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocno.Location = new System.Drawing.Point(95, 16);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.Size = new System.Drawing.Size(69, 23);
		this.txtdocno.TabIndex = 0;
		this.CmbVehicleNo.AllowNull = (DefaultableBoolean)1;
		this.CmbVehicleNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbVehicleNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbVehicleNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance108).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance108).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance108).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbVehicleNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance108;
		((AppearanceBase)appearance148).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance148;
		((SpecialBoxBase)((UltraGridBase)this.CmbVehicleNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance163).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance163).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance163).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance163;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance174).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance174).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance174;
		((AppearanceBase)appearance185).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance185).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance185;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance196).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance196;
		((AppearanceBase)appearance207).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance207).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance207;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance218).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance218).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance218).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance218).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance218).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance218;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbVehicleNo).Font = new System.Drawing.Font("Verdana", 9.75f);
		((System.Windows.Forms.Control)(object)this.CmbVehicleNo).Location = new System.Drawing.Point(368, 42);
		((System.Windows.Forms.Control)(object)this.CmbVehicleNo).Name = "CmbVehicleNo";
		((System.Windows.Forms.Control)(object)this.CmbVehicleNo).Size = new System.Drawing.Size(161, 26);
		((System.Windows.Forms.Control)(object)this.CmbVehicleNo).TabIndex = 4;
		this.CmbVehicleType.AllowNull = (DefaultableBoolean)1;
		this.CmbVehicleType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbVehicleType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbVehicleType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance82).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance83).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance83).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance83).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbVehicleType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance84;
		((SpecialBoxBase)((UltraGridBase)this.CmbVehicleType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance85).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance85).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance85).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance86).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance86;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance87).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance88;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance89).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance91).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance91).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance91).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance91).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance91;
		((AppearanceBase)appearance92).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance93;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance94;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbVehicleType).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbVehicleType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbVehicleType).Location = new System.Drawing.Point(368, 14);
		((System.Windows.Forms.Control)(object)this.CmbVehicleType).Name = "CmbVehicleType";
		((System.Windows.Forms.Control)(object)this.CmbVehicleType).Size = new System.Drawing.Size(161, 26);
		((System.Windows.Forms.Control)(object)this.CmbVehicleType).TabIndex = 3;
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(4, 47);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(78, 15);
		this.label24.TabIndex = 3;
		this.label24.Text = "Reference No";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(4, 20);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(88, 15);
		this.label5.TabIndex = 3;
		this.label5.Text = "Doc No && Date";
		this.label14.AutoSize = true;
		this.label14.BackColor = System.Drawing.Color.White;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(295, 20);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(73, 15);
		this.label14.TabIndex = 50;
		this.label14.Text = "Vehicle Type";
		this.label33.BackColor = System.Drawing.Color.LightGray;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.label33.Location = new System.Drawing.Point(670, 16);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(65, 23);
		this.label33.TabIndex = 8;
		this.label33.Text = "Remarks";
		this.label33.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(295, 48);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(64, 15);
		this.label12.TabIndex = 50;
		this.label12.Text = "Vehicle No";
		this.DocDate.CustomFormat = "dd-MMM-yy";
		this.DocDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DocDate.Location = new System.Drawing.Point(166, 16);
		this.DocDate.Name = "DocDate";
		this.DocDate.Size = new System.Drawing.Size(126, 23);
		this.DocDate.TabIndex = 1;
		this.DocDate.TabStop = false;
		this.DocDate.ValueChanged += new System.EventHandler(DocDate_ValueChanged);
		this.txtReferenceNoHeader.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtReferenceNoHeader.Location = new System.Drawing.Point(95, 44);
		this.txtReferenceNoHeader.Name = "txtReferenceNoHeader";
		this.txtReferenceNoHeader.Size = new System.Drawing.Size(197, 23);
		this.txtReferenceNoHeader.TabIndex = 2;
		this.txtReferenceNoHeader.TextChanged += new System.EventHandler(txtVehicleNo_TextChanged);
		this.txtRemarksHeader.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtRemarksHeader.Location = new System.Drawing.Point(740, 14);
		this.txtRemarksHeader.Multiline = true;
		this.txtRemarksHeader.Name = "txtRemarksHeader";
		this.txtRemarksHeader.Size = new System.Drawing.Size(414, 52);
		this.txtRemarksHeader.TabIndex = 6;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(1176, 120);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(62, 15);
		this.label11.TabIndex = 45;
		this.label11.Text = "BalWeight";
		this.label11.Visible = false;
		this.txtSubNoId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSubNoId.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSubNoId.Location = new System.Drawing.Point(1293, 33);
		this.txtSubNoId.Name = "txtSubNoId";
		this.txtSubNoId.ReadOnly = true;
		this.txtSubNoId.Size = new System.Drawing.Size(21, 25);
		this.txtSubNoId.TabIndex = 48;
		this.txtSubNoId.TabStop = false;
		this.txtSubNoId.Visible = false;
		this.txtBalQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBalQty.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBalQty.Location = new System.Drawing.Point(1237, 88);
		this.txtBalQty.Name = "txtBalQty";
		this.txtBalQty.ReadOnly = true;
		this.txtBalQty.Size = new System.Drawing.Size(53, 25);
		this.txtBalQty.TabIndex = 10;
		this.txtBalQty.TabStop = false;
		this.txtBalQty.Visible = false;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(1176, 11);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(44, 15);
		this.label13.TabIndex = 0;
		this.label13.Text = "Branch";
		this.label13.Visible = false;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(1176, 38);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(60, 15);
		this.label18.TabIndex = 0;
		this.label18.Text = "Branch To";
		this.label18.Visible = false;
		this.cmbBranchFrom.AllowNull = (DefaultableBoolean)1;
		this.cmbBranchFrom.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbBranchFrom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbBranchFrom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbBranchFrom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.cmbBranchFrom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbBranchFrom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance98).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance98).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance98).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranchFrom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance98;
		((AppearanceBase)appearance99).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchFrom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance99;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranchFrom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance187).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance187).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance187).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance187).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchFrom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance187;
		((UltraGridBase)this.cmbBranchFrom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbBranchFrom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbBranchFrom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbBranchFrom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.cmbBranchFrom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbBranchFrom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance190).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchFrom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance190;
		((AppearanceBase)appearance191).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance191).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbBranchFrom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance191;
		((UltraGridBase)this.cmbBranchFrom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbBranchFrom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance192).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance192).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance192).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance192).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance192).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchFrom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance192;
		((AppearanceBase)appearance193).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbBranchFrom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance193;
		((UltraGridBase)this.cmbBranchFrom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbBranchFrom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance194).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance194).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbBranchFrom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance194;
		((UltraGridBase)this.cmbBranchFrom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance195).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbBranchFrom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance195;
		((UltraGridBase)this.cmbBranchFrom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbBranchFrom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbBranchFrom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbBranchFrom).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbBranchFrom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbBranchFrom).Location = new System.Drawing.Point(1237, 5);
		((System.Windows.Forms.Control)(object)this.cmbBranchFrom).Name = "cmbBranchFrom";
		((System.Windows.Forms.Control)(object)this.cmbBranchFrom).Size = new System.Drawing.Size(53, 26);
		((System.Windows.Forms.Control)(object)this.cmbBranchFrom).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.cmbBranchFrom).TabStop = false;
		((System.Windows.Forms.Control)(object)this.cmbBranchFrom).Visible = false;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(1176, 93);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(42, 15);
		this.label10.TabIndex = 45;
		this.label10.Text = "BalQty";
		this.label10.Visible = false;
		this.txtDocNoId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDocNoId.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNoId.Location = new System.Drawing.Point(1293, 6);
		this.txtDocNoId.Name = "txtDocNoId";
		this.txtDocNoId.ReadOnly = true;
		this.txtDocNoId.Size = new System.Drawing.Size(21, 25);
		this.txtDocNoId.TabIndex = 47;
		this.txtDocNoId.TabStop = false;
		this.txtDocNoId.Visible = false;
		this.txtRefDocTypeId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRefDocTypeId.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRefDocTypeId.Location = new System.Drawing.Point(1293, 60);
		this.txtRefDocTypeId.Name = "txtRefDocTypeId";
		this.txtRefDocTypeId.ReadOnly = true;
		this.txtRefDocTypeId.Size = new System.Drawing.Size(21, 25);
		this.txtRefDocTypeId.TabIndex = 46;
		this.txtRefDocTypeId.TabStop = false;
		this.txtRefDocTypeId.Visible = false;
		this.cmbBranchTo.AllowNull = (DefaultableBoolean)1;
		this.cmbBranchTo.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbBranchTo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbBranchTo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance160).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbBranchTo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance160;
		((UltraGridBase)this.cmbBranchTo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbBranchTo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance161).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance161).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance161).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance161).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranchTo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance161;
		((AppearanceBase)appearance162).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchTo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance162;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranchTo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance164).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance164).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance164).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchTo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.cmbBranchTo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbBranchTo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance165).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbBranchTo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance165;
		((AppearanceBase)appearance166).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance166).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbBranchTo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance166;
		((UltraGridBase)this.cmbBranchTo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbBranchTo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchTo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance167;
		((AppearanceBase)appearance168).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance168).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbBranchTo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.cmbBranchTo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbBranchTo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance169).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance169).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance169).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance169).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance169).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchTo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance169;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbBranchTo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbBranchTo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbBranchTo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbBranchTo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbBranchTo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbBranchTo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbBranchTo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbBranchTo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbBranchTo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbBranchTo).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbBranchTo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbBranchTo).Location = new System.Drawing.Point(1237, 32);
		((System.Windows.Forms.Control)(object)this.cmbBranchTo).Name = "cmbBranchTo";
		((System.Windows.Forms.Control)(object)this.cmbBranchTo).Size = new System.Drawing.Size(53, 26);
		((System.Windows.Forms.Control)(object)this.cmbBranchTo).TabIndex = 6;
		((System.Windows.Forms.Control)(object)this.cmbBranchTo).TabStop = false;
		((System.Windows.Forms.Control)(object)this.cmbBranchTo).Visible = false;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(1176, 65);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(53, 15);
		this.label4.TabIndex = 27;
		this.label4.Text = "DO Type";
		this.label4.Visible = false;
		this.CmbDeliveryOrderType.AllowNull = (DefaultableBoolean)1;
		this.CmbDeliveryOrderType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDeliveryOrderType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDeliveryOrderType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDeliveryOrderType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbDeliveryOrderType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDeliveryOrderType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliveryOrderType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliveryOrderType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance41;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliveryOrderType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliveryOrderType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbDeliveryOrderType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDeliveryOrderType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDeliveryOrderType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDeliveryOrderType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbDeliveryOrderType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDeliveryOrderType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliveryOrderType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDeliveryOrderType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbDeliveryOrderType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDeliveryOrderType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliveryOrderType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDeliveryOrderType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbDeliveryOrderType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDeliveryOrderType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDeliveryOrderType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbDeliveryOrderType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDeliveryOrderType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbDeliveryOrderType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDeliveryOrderType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDeliveryOrderType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryOrderType).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDeliveryOrderType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryOrderType).Location = new System.Drawing.Point(1237, 59);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryOrderType).Name = "CmbDeliveryOrderType";
		((System.Windows.Forms.Control)(object)this.CmbDeliveryOrderType).Size = new System.Drawing.Size(53, 26);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryOrderType).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryOrderType).TabStop = false;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryOrderType).Visible = false;
		this.panel3.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel3.Controls.Add(this.label23);
		this.panel3.Controls.Add(this.chkPrintCustomerWise);
		this.panel3.Controls.Add(this.ChkPrint);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(2, 27);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1173, 24);
		this.panel3.TabIndex = 1;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(8, 2);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(127, 19);
		this.label23.TabIndex = 0;
		this.label23.Text = "Delivery Order";
		this.chkPrintCustomerWise.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.chkPrintCustomerWise.AutoSize = true;
		this.chkPrintCustomerWise.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkPrintCustomerWise.ForeColor = System.Drawing.Color.White;
		this.chkPrintCustomerWise.Location = new System.Drawing.Point(894, 2);
		this.chkPrintCustomerWise.Name = "chkPrintCustomerWise";
		this.chkPrintCustomerWise.Size = new System.Drawing.Size(134, 20);
		this.chkPrintCustomerWise.TabIndex = 8;
		this.chkPrintCustomerWise.Text = "Preview 1853A";
		this.chkPrintCustomerWise.UseVisualStyleBackColor = true;
		this.ChkPrint.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkPrint.AutoSize = true;
		this.ChkPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkPrint.ForeColor = System.Drawing.Color.White;
		this.ChkPrint.Location = new System.Drawing.Point(1047, 3);
		this.ChkPrint.Name = "ChkPrint";
		this.ChkPrint.Size = new System.Drawing.Size(124, 20);
		this.ChkPrint.TabIndex = 8;
		this.ChkPrint.Text = "Preview 1853";
		this.ChkPrint.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(2, 2);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1173, 25);
		this.panel2.TabIndex = 0;
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[17]
		{
			this.btnnew, this.btnRefresh, this.toolStripSeparator4, this.btnsave, this.btnupdate, this.btnDelete, this.toolStripSeparator5, this.btnattachment, this.toolStripSeparator6, this.btnPrintSlip,
			this.btnPrintCustomerWise, this.toolStripSeparator2, this.btnLoadSaleOrder, this.toolStripSeparator3, this.btnDefineReferenceParties, this.toolStripSeparator1, this.btnshortcutkeys
		});
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1173, 27);
		this.toolStrip1.TabIndex = 0;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = (System.Drawing.Image)resources.GetObject("btnRefresh.Image");
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.toolStripSeparator4.Name = "toolStripSeparator4";
		this.toolStripSeparator4.Size = new System.Drawing.Size(6, 27);
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
		this.btnDelete.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(73, 24);
		this.btnDelete.Text = "Delete";
		this.btnDelete.Visible = false;
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.toolStripSeparator5.Name = "toolStripSeparator5";
		this.toolStripSeparator5.Size = new System.Drawing.Size(6, 27);
		this.btnattachment.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnattachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnattachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnattachment.Name = "btnattachment";
		this.btnattachment.Size = new System.Drawing.Size(109, 24);
		this.btnattachment.Text = "&Attachment";
		this.btnattachment.Click += new System.EventHandler(btnattachment_Click);
		this.toolStripSeparator6.Name = "toolStripSeparator6";
		this.toolStripSeparator6.Size = new System.Drawing.Size(6, 27);
		this.btnPrintSlip.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrintSlip.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrintSlip.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrintSlip.Name = "btnPrintSlip";
		this.btnPrintSlip.Size = new System.Drawing.Size(100, 24);
		this.btnPrintSlip.Text = "1853-Print";
		this.btnPrintSlip.Click += new System.EventHandler(btnprint_Click);
		this.btnPrintCustomerWise.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrintCustomerWise.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrintCustomerWise.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrintCustomerWise.Name = "btnPrintCustomerWise";
		this.btnPrintCustomerWise.Size = new System.Drawing.Size(202, 24);
		this.btnPrintCustomerWise.Text = "1853A-CustomerWisePrint";
		this.btnPrintCustomerWise.Click += new System.EventHandler(btnPrintCustomerWise_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
		this.btnLoadSaleOrder.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLoadSaleOrder.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadSaleOrder.Name = "btnLoadSaleOrder";
		this.btnLoadSaleOrder.Size = new System.Drawing.Size(160, 24);
		this.btnLoadSaleOrder.Text = "Load Order Main Detail";
		this.btnLoadSaleOrder.Click += new System.EventHandler(btnLoadSaleOrder_Click);
		this.toolStripSeparator3.Name = "toolStripSeparator3";
		this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
		this.btnDefineReferenceParties.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnDefineReferenceParties.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDefineReferenceParties.Name = "btnDefineReferenceParties";
		this.btnDefineReferenceParties.Size = new System.Drawing.Size(144, 24);
		this.btnDefineReferenceParties.Text = "Define Other Parties";
		this.btnDefineReferenceParties.Click += new System.EventHandler(btnDefineReferenceParties_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.btnshortcutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshortcutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnshortcutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnshortcutkeys.Name = "btnshortcutkeys";
		this.btnshortcutkeys.Size = new System.Drawing.Size(126, 24);
		this.btnshortcutkeys.Text = "ShortCut Keys";
		this.btnshortcutkeys.Click += new System.EventHandler(btnshortcutkeys_Click);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.GrdHistoryDetail);
		this.tabPage2.Controls.Add(this.panel10);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.grdhistory);
		this.tabPage2.Controls.Add(this.panel6);
		this.tabPage2.Controls.Add(this.panel12);
		this.tabPage2.Controls.Add(this.panel8);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage2.Size = new System.Drawing.Size(1177, 696);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.GrdHistoryDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.GrdHistoryDetail).BackColor = System.Drawing.Color.White;
		GrdHistoryDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GrdHistoryDetail.DesignTimeLayout = GrdHistoryDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GrdHistoryDetail.DynamicFiltering = true;
		this.GrdHistoryDetail.FilterMode = (FilterMode)2;
		this.GrdHistoryDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdHistoryDetail.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).Location = new System.Drawing.Point(2, 448);
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).Name = "GrdHistoryDetail";
		((ScrollableBase)this.GrdHistoryDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).Size = new System.Drawing.Size(1173, 246);
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).TabIndex = 48;
		this.GrdHistoryDetail.TotalRow = (InheritableBoolean)1;
		this.GrdHistoryDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdHistoryDetail).VisualStyle = (VisualStyle)4;
		this.GrdHistoryDetail.LinkClicked += new ColumnActionEventHandler(GrdHistoryDetail_LinkClicked);
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).KeyDown += new System.Windows.Forms.KeyEventHandler(GrdHistoryDetail_KeyDown);
		this.panel10.BackColor = System.Drawing.Color.Teal;
		this.panel10.Controls.Add(this.label27);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(2, 426);
		this.panel10.Margin = new System.Windows.Forms.Padding(2);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1173, 22);
		this.panel10.TabIndex = 49;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label27.ForeColor = System.Drawing.Color.White;
		this.label27.Location = new System.Drawing.Point(2, 3);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(211, 17);
		this.label27.TabIndex = 2;
		this.label27.Text = "Detail Of Above Selected Row";
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
		((System.Windows.Forms.Control)(object)this.grdhistory).Location = new System.Drawing.Point(2, 150);
		((System.Windows.Forms.Control)(object)this.grdhistory).Name = "grdhistory";
		((ScrollableBase)this.grdhistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdhistory).Size = new System.Drawing.Size(1173, 276);
		((System.Windows.Forms.Control)(object)this.grdhistory).TabIndex = 48;
		this.grdhistory.TotalRow = (InheritableBoolean)1;
		this.grdhistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdhistory).VisualStyle = (VisualStyle)4;
		this.grdhistory.ColumnButtonClick += new ColumnActionEventHandler(grdhistory_ColumnButtonClick);
		this.grdhistory.SelectionChanged += new System.EventHandler(grdhistory_SelectionChanged);
		this.grdhistory.LinkClicked += new ColumnActionEventHandler(grdhistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdhistory).DoubleClick += new System.EventHandler(grdhistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdhistory).KeyDown += new System.Windows.Forms.KeyEventHandler(grdHistory_KeyDown);
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.label1);
		this.panel6.Controls.Add(this.ctrlGrdBar2);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(2, 128);
		this.panel6.Margin = new System.Windows.Forms.Padding(2);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1173, 22);
		this.panel6.TabIndex = 47;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(2, 1);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(48, 19);
		this.label1.TabIndex = 22;
		this.label1.Text = "Main";
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1126, 0);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdhistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(43, 21);
		this.ctrlGrdBar2.TabIndex = 21;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.panel12.Controls.Add(this.groupBox4);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(2, 53);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1173, 75);
		this.panel12.TabIndex = 46;
		this.groupBox4.Controls.Add(this.btnShowHistory);
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemHistory);
		this.groupBox4.Controls.Add(this.label72);
		this.groupBox4.Controls.Add(this.label25);
		this.groupBox4.Controls.Add(this.label73);
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbVehicleNoHistory);
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomerHistory);
		this.groupBox4.Controls.Add(this.label75);
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory);
		this.groupBox4.Controls.Add(this.txtToDateHistory);
		this.groupBox4.Controls.Add(this.label76);
		this.groupBox4.Controls.Add(this.txtToDocNoHistory);
		this.groupBox4.Controls.Add(this.txtFromdateHistory);
		this.groupBox4.Controls.Add(this.label77);
		this.groupBox4.Controls.Add(this.txtFromNoHistory);
		this.groupBox4.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox4.Location = new System.Drawing.Point(3, 2);
		this.groupBox4.Name = "groupBox4";
		this.groupBox4.Size = new System.Drawing.Size(909, 72);
		this.groupBox4.TabIndex = 1;
		this.groupBox4.TabStop = false;
		this.groupBox4.Text = "Filters";
		this.btnShowHistory.BackColor = System.Drawing.Color.Teal;
		this.btnShowHistory.FlatAppearance.BorderSize = 0;
		this.btnShowHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowHistory.Font = new System.Drawing.Font("Verdana", 10.2f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShowHistory.ForeColor = System.Drawing.Color.White;
		this.btnShowHistory.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnShowHistory.Location = new System.Drawing.Point(840, 43);
		this.btnShowHistory.Margin = new System.Windows.Forms.Padding(2);
		this.btnShowHistory.Name = "btnShowHistory";
		this.btnShowHistory.Size = new System.Drawing.Size(62, 25);
		this.btnShowHistory.TabIndex = 7;
		this.btnShowHistory.Text = "Show";
		this.btnShowHistory.UseVisualStyleBackColor = false;
		this.btnShowHistory.Click += new System.EventHandler(btnShowHistory_Click);
		this.CmbItemHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbItemHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance197).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance197).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance197;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance198).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance198).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance198).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance198).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance198;
		((AppearanceBase)appearance199).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance199;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance213).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance213).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance213).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance213).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance213;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance214).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance214).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance214;
		((AppearanceBase)appearance215).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance215).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance215;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance216).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance216;
		((AppearanceBase)appearance217).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance217).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance217;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance219).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance219).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance219).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance219).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance219).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance219;
		((AppearanceBase)appearance220).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance220;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance221).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance221).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance221;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance222).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance222;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemHistory).Location = new System.Drawing.Point(634, 42);
		((System.Windows.Forms.Control)(object)this.CmbItemHistory).Name = "CmbItemHistory";
		((System.Windows.Forms.Control)(object)this.CmbItemHistory).Size = new System.Drawing.Size(201, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemHistory).TabIndex = 6;
		this.label72.AutoSize = true;
		this.label72.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label72.Location = new System.Drawing.Point(575, 48);
		this.label72.Name = "label72";
		this.label72.Size = new System.Drawing.Size(32, 15);
		this.label72.TabIndex = 180;
		this.label72.Text = "Item";
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(327, 48);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(64, 15);
		this.label25.TabIndex = 178;
		this.label25.Text = "Vehicle No";
		this.label73.AutoSize = true;
		this.label73.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label73.Location = new System.Drawing.Point(572, 19);
		this.label73.Name = "label73";
		this.label73.Size = new System.Drawing.Size(58, 15);
		this.label73.TabIndex = 178;
		this.label73.Text = "Customer";
		this.CmbVehicleNoHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbVehicleNoHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbVehicleNoHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbVehicleNoHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance170).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbVehicleNoHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance170;
		((UltraGridBase)this.CmbVehicleNoHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbVehicleNoHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance171).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance171).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance171).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance171).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbVehicleNoHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance171;
		((AppearanceBase)appearance172).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbVehicleNoHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance172;
		((SpecialBoxBase)((UltraGridBase)this.CmbVehicleNoHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance100).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbVehicleNoHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance100;
		((UltraGridBase)this.CmbVehicleNoHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbVehicleNoHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance102).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbVehicleNoHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance102;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance103).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbVehicleNoHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.CmbVehicleNoHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbVehicleNoHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbVehicleNoHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance104;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance105).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbVehicleNoHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.CmbVehicleNoHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbVehicleNoHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance106).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance106).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance106).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbVehicleNoHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance106;
		((AppearanceBase)appearance107).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbVehicleNoHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.CmbVehicleNoHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbVehicleNoHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance188).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance188).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbVehicleNoHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance188;
		((UltraGridBase)this.CmbVehicleNoHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance189).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbVehicleNoHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance189;
		((UltraGridBase)this.CmbVehicleNoHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbVehicleNoHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbVehicleNoHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbVehicleNoHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbVehicleNoHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbVehicleNoHistory).Location = new System.Drawing.Point(411, 42);
		((System.Windows.Forms.Control)(object)this.CmbVehicleNoHistory).Name = "CmbVehicleNoHistory";
		((System.Windows.Forms.Control)(object)this.CmbVehicleNoHistory).Size = new System.Drawing.Size(158, 26);
		((System.Windows.Forms.Control)(object)this.CmbVehicleNoHistory).TabIndex = 5;
		this.CmbCustomerHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomerHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomerHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomerHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance43).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance43).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance43).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance44;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance45).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance45).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance47;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance50).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance51).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance51).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance51).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance51).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance51;
		((AppearanceBase)appearance78).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomerHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Location = new System.Drawing.Point(634, 13);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Name = "CmbCustomerHistory";
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Size = new System.Drawing.Size(201, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).TabIndex = 5;
		this.label75.AutoSize = true;
		this.label75.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label75.Location = new System.Drawing.Point(5, 19);
		this.label75.Name = "label75";
		this.label75.Size = new System.Drawing.Size(60, 15);
		this.label75.TabIndex = 174;
		this.label75.Text = "Date Type";
		this.cmbDateTypeHistory.AllowNull = (DefaultableBoolean)1;
		this.cmbDateTypeHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDateTypeHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDateTypeHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance226).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance226).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance226;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance227).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance227).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance227).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance227).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance227;
		((AppearanceBase)appearance228).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance228;
		((SpecialBoxBase)((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance143).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance143).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance143).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance144).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance144;
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance145).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance146;
		((AppearanceBase)appearance147).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance147).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance147;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance150).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance150).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance150).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance150).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance150;
		((AppearanceBase)appearance151).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance152).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance152;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance153;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDateTypeHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Location = new System.Drawing.Point(97, 13);
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Name = "cmbDateTypeHistory";
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Size = new System.Drawing.Size(224, 26);
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).TabIndex = 0;
		this.cmbDateTypeHistory.ValueChanged += new System.EventHandler(cmbDateTypeHistory_ValueChanged);
		this.txtToDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDateHistory.CustomFormat = "dd-MMM-yy";
		this.txtToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtToDateHistory.Location = new System.Drawing.Point(210, 44);
		this.txtToDateHistory.Name = "txtToDateHistory";
		this.txtToDateHistory.Size = new System.Drawing.Size(111, 23);
		this.txtToDateHistory.TabIndex = 2;
		this.label76.AutoSize = true;
		this.label76.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label76.Location = new System.Drawing.Point(5, 48);
		this.label76.Name = "label76";
		this.label76.Size = new System.Drawing.Size(91, 15);
		this.label76.TabIndex = 171;
		this.label76.Text = "Date From && To";
		this.txtToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDocNoHistory.Location = new System.Drawing.Point(491, 15);
		this.txtToDocNoHistory.Name = "txtToDocNoHistory";
		this.txtToDocNoHistory.Size = new System.Drawing.Size(78, 23);
		this.txtToDocNoHistory.TabIndex = 4;
		this.txtFromdateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromdateHistory.CustomFormat = "dd-MMM-yy";
		this.txtFromdateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromdateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtFromdateHistory.Location = new System.Drawing.Point(97, 44);
		this.txtFromdateHistory.Name = "txtFromdateHistory";
		this.txtFromdateHistory.Size = new System.Drawing.Size(111, 23);
		this.txtFromdateHistory.TabIndex = 1;
		this.label77.AutoSize = true;
		this.label77.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label77.Location = new System.Drawing.Point(322, 19);
		this.label77.Name = "label77";
		this.label77.Size = new System.Drawing.Size(88, 15);
		this.label77.TabIndex = 173;
		this.label77.Text = "Doc From && To";
		this.txtFromNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFromNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromNoHistory.Location = new System.Drawing.Point(411, 15);
		this.txtFromNoHistory.Name = "txtFromNoHistory";
		this.txtFromNoHistory.Size = new System.Drawing.Size(78, 23);
		this.txtFromNoHistory.TabIndex = 3;
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.label16);
		this.panel8.Controls.Add(this.toolStrip2);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(2, 2);
		this.panel8.Margin = new System.Windows.Forms.Padding(2);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1173, 51);
		this.panel8.TabIndex = 0;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.ForeColor = System.Drawing.Color.White;
		this.label16.Location = new System.Drawing.Point(6, 30);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(190, 19);
		this.label16.TabIndex = 22;
		this.label16.Text = "Delivery Order History";
		this.toolStrip2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnNewHistory, this.BtnRefreshHistory });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1173, 27);
		this.toolStrip2.TabIndex = 1;
		this.toolStrip2.Text = "toolStrip2";
		this.btnNewHistory.Font = new System.Drawing.Font("Verdana", 10f);
		this.btnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewHistory.Name = "btnNewHistory";
		this.btnNewHistory.Size = new System.Drawing.Size(61, 24);
		this.btnNewHistory.Text = "&New";
		this.btnNewHistory.Click += new System.EventHandler(btnLoadAll_Click);
		this.BtnRefreshHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.BtnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefreshHistory.Name = "BtnRefreshHistory";
		this.BtnRefreshHistory.Size = new System.Drawing.Size(80, 24);
		this.BtnRefreshHistory.Text = "Refresh";
		this.BtnRefreshHistory.Click += new System.EventHandler(BtnRefreshHistory_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(1185, 725);
		base.Controls.Add(this.panel1);
		base.HelpButton = true;
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "DeliveryOrderConcrete";
		this.Text = "Delivery Order";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(DeliveryOrder_FormClosing);
		base.Load += new System.EventHandler(PurchsaeOrder_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(PurchsaeOrder_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.DetailGridPanel.ResumeLayout(false);
		this.tabPage3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.Cmbheight).EndInit();
		((System.ComponentModel.ISupportInitialize)this.Cmbstoreys).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combcityarea).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbReferenceParty).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbOrderNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWareHouse).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplierCustomer).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAttributeVarient).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).EndInit();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbVehicleNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbVehicleType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchFrom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchTo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveryOrderType).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GrdHistoryDetail).EndInit();
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdhistory).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel12.ResumeLayout(false);
		this.groupBox4.ResumeLayout(false);
		this.groupBox4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbItemHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbVehicleNoHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDateTypeHistory).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
