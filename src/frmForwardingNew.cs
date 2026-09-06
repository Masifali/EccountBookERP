// using Architecture.Reports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Export;
using Architecture.BLL.Inventory;
using Architecture.BLL.Reports.Export;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Export;
using Architecture.Model.Inventory;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Contractor_Wages;
using Architecture.WinApp.Helper;
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
namespace Architecture.WinApp.ExportSalt
{

public class frmForwardingNew : Form
{
	private Attachment AT = new Attachment();

	private int RecId;

	private double PreStockWt;

	private bool UpdateMode;

	private int DefaultDaysToLessFromHistoryFromDate;

	private int updateDetailIndex;

	private int updateDetailIndexOther;

	private DateTime lotcompletedate;

	private DropDownBind DDL = new DropDownBind();

	private UserAccount UserAccount = new UserAccount();

	private DataTable dtDetail = new DataTable();

	private DataTable dtGetInvoiceDetail = new DataTable();

	private DataTable dtOtherItem = new DataTable();

	private DataTable dtpcktype = new DataTable();

	private Rightsobjects formright = new Rightsobjects();

	private int[] AryColWidth;

	private bool WagesStatus;

	private List<ExImForwardingPackingDetail> LstRemoveRecordDetail = new List<ExImForwardingPackingDetail>();

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel3;

	private Panel panel2;

	private TabPage tabPage2;

	private ToolStrip toolStrip1;

	private ToolStripButton BtnNew;

	private ToolStripButton btnSave;

	private ToolStripButton btnUpdate;

	private ToolStripButton BtnPrint;

	internal UltraCombo cmbCustomer;

	private TextBox txtNoOfContainer;

	private Label label24;

	private TextBox txtOtherCharges;

	private TextBox txtFreight;

	private Label label15;

	internal UltraCombo cmbDestinationPort;

	private Label label23;

	private TextBox txtCNICNO;

	internal UltraCombo cmbLoadingPort;

	private Label label22;

	private Label label21;

	private Label label12;

	private Label label14;

	private TextBox txtBilityNo;

	private Label label11;

	private Label label10;

	private TextBox txtDriverCellNo;

	private Label label9;

	internal UltraCombo cmbtransporter;

	private TextBox txtDriverName;

	private TextBox txtVehicleNo;

	private Label label8;

	private Label label7;

	private Label label4;

	private DateTimePicker txtGPDate;

	private DateTimePicker DocDate;

	private TextBox txtdocno;

	internal UltraCombo cmbInvoiceNo;

	private Button btnAddinGrid;

	internal UltraCombo cmbPackUOM;

	private TextBox txtNetWeight;

	private Label label30;

	private TextBox txtSeal;

	private Label label37;

	private Label label31;

	internal UltraCombo cmbPackType;

	private Label label29;

	internal UltraCombo cmbItem;

	private Label label28;

	internal GridEX DataGridHistory;

	internal UltraCombo cmbGpNo;

	private Button btnDetailCancel;

	private Button btnDetailUpdate;

	internal UltraCombo CmbJobLot;

	private Label label55;

	internal UltraCombo CmbWareHouseFrom;

	private Label label54;

	private ToolStripButton btnRefresh;

	private TextBox txtNoOfBags;

	private Label label32;

	private TextBox txtContainer2;

	private Label label34;

	private TextBox txtconainer1;

	private Label label33;

	private TextBox txtFactoryWeight;

	private Label label35;

	private CtrlGrdBar ctrlGrdBar1;

	private CtrlGrdBar ctrlGrdBar2;

	private TextBox txtDoNetWeight;

	private Label label2;

	private Label label17;

	private Label label18;

	private Label label39;

	private TextBox txtGrossWeight;

	private Panel panel4;

	private TabControl tabControl2;

	private TabPage tabPage3;

	private Panel panel5;

	private TabPage tabPage4;

	internal GridEX grdotheritems;

	private Panel panel9;

	private Button btnoCancel;

	private Button btnoUpdate;

	private Button btnoadd;

	private Label label27;

	private TextBox txtoRemarks;

	private TextBox txtoQty;

	private Label label41;

	private Label label70;

	private TextBox txtoAmount;

	internal UltraCombo CmbOtherItem;

	private Label label71;

	private TextBox txtOContainer;

	private Label label42;

	private TextBox txtoRate;

	private Label label63;

	private Label label45;

	private Label label44;

	private TextBox txtStockWeight;

	private TextBox txtAddLess;

	private TextBox txtDoGrossWeight;

	private Label label1;

	private Label label47;

	private Label label46;

	private TextBox txtPackingWeight;

	private TextBox txtPackingweightTotal;

	private TextBox txtInvoiceNetWeight;

	private Label InvoiceNetWeight;

	private TextBox txtNoofInvoices;

	private Label label48;

	private TextBox txtBalFcWeight;

	private TextBox txtDispatchWeight;

	private Label label49;

	private Label label43;

	internal UltraCombo cmbcontainerNo;

	private Label label50;

	private CheckBox ChkPrintPreview;

	private ToolStripButton btnAttachment;

	private Label label36;

	private GroupBox groupBox2;

	private GroupBox groupBox1;

	private GroupBox groupBox3;

	private Panel panel13;

	private Label label51;

	private Panel panel12;

	private Label label6;

	private Panel panel7;

	private Label label53;

	private Panel panel11;

	private Label label52;

	internal GridEX grdDetail;

	internal UltraCombo cmbWareHouseTo;

	private Label label20;

	private RichTextBox txtRemarksHeader;

	private Label label5;

	internal UltraCombo cmbWareHouseToOtherItem;

	private Label label16;

	internal UltraCombo cmbWareHouseFromOtherItem;

	private Label label13;

	internal GridEX grdHistoryDetail;

	private Label label26;

	private TextBox txtProductionNo;

	private Label label25;

	private TextBox txtPackingExpiryDate;

	internal GridEX grdgpInfo;

	private Panel panel14;

	private Label label38;

	private CtrlGrdBar ctrlGrdBar3;

	private ToolStripButton btnDelete;

	private Panel panel15;

	private Label label92;

	private Panel panel16;

	private GroupBox groupBox4;

	private RadioButton rdapproveddate;

	private Button btnShowHistory;

	private RadioButton rdmodifydate;

	private Label label40;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	private Label label73;

	private Label label89;

	private Label label90;

	private Label label91;

	internal UltraCombo CmbCustomerHistory;

	private TextBox ToDocNoHistory;

	private TextBox FromDocNoHistory;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	private Panel panel17;

	private Label label69;

	private ToolStrip toolStrip3;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripButton btnResetHistory;

	private ToolStripButton btnRefreshHistory;

	private Panel panel18;

	private Label label93;

	public frmForwardingNew(UserAccount user)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void ExpfrmForwarding_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1180, 800);
			formright = CommonServices.SetRightsValueInRightsObject("EximForwarding");
			btnSave.Enabled = formright.DoHaveSaveRight;
			btnUpdate.Enabled = formright.DoHaveUpdateRights;
			btnDelete.Enabled = formright.DoHaveCanDelete;
			dtpcktype.Columns.Add("id");
			dtpcktype.Columns.Add("Description");
			dtpcktype.Columns.Add("PackingWeight");
			gatepassGetAll();
			GridGatepassFill();
			generateCode();
			cmbGpNo.Focus();
			packingType();
			JobLotFill();
			WarehouseFill();
			CropYearFill();
			TransporterBind();
			TransporterFill();
			bindSeaPort();
			ItemFillForExport();
			dtDetail.Columns.Add("Id", typeof(int));
			dtDetail.Columns.Add("RefDocumentTypeId", typeof(int));
			dtDetail.Columns.Add("RefDocIdNo", typeof(int));
			dtDetail.Columns.Add("RefDocSubIdNo", typeof(int));
			dtDetail.Columns.Add("ProformaDetailId", typeof(int));
			dtDetail.Columns.Add("ProformaId", typeof(int));
			dtDetail.Columns.Add("ProformaNo", typeof(string));
			dtDetail.Columns.Add("PreInvoiceId", typeof(int));
			dtDetail.Columns.Add("PreInvoiceDetailId", typeof(int));
			dtDetail.Columns.Add("ItemId", typeof(int));
			dtDetail.Columns.Add("ItemName", typeof(string));
			dtDetail.Columns.Add("PackTypeId", typeof(int));
			dtDetail.Columns.Add("Packtype", typeof(string));
			dtDetail.Columns.Add("NoOfBags", typeof(double));
			dtDetail.Columns.Add("OuterUOMId", typeof(int));
			dtDetail.Columns.Add("OuterUOM", typeof(string));
			dtDetail.Columns.Add("NetWeight", typeof(double));
			dtDetail.Columns.Add("EbUnit", typeof(double));
			dtDetail.Columns.Add("EbTotal", typeof(double));
			dtDetail.Columns.Add("AddLess", typeof(double));
			dtDetail.Columns.Add("GrossWeight", typeof(double));
			dtDetail.Columns.Add("StockWeight", typeof(double));
			dtDetail.Columns.Add("JobLotId", typeof(int));
			dtDetail.Columns.Add("JobLot", typeof(string));
			dtDetail.Columns.Add("WarehouseId", typeof(int));
			dtDetail.Columns.Add("Warehouse", typeof(string));
			dtDetail.Columns.Add("WarehouseToId", typeof(int));
			dtDetail.Columns.Add("WarehouseTo", typeof(string));
			dtDetail.Columns.Add("Container#", typeof(string));
			dtDetail.Columns.Add("Seal#", typeof(string));
			dtDetail.Columns.Add("PackingExpiryDate", typeof(string));
			dtDetail.Columns.Add("ProductionNo", typeof(string));
			grdDetail.DataSource = dtDetail;
			grdDetail.RetrieveStructure();
			GridDetailSetting();
			dtOtherItem.Columns.Add("ItemId", typeof(int));
			dtOtherItem.Columns.Add("ItemName", typeof(string));
			dtOtherItem.Columns.Add("Qty", typeof(double));
			dtOtherItem.Columns.Add("Rate", typeof(double));
			dtOtherItem.Columns.Add("Amount", typeof(double));
			dtOtherItem.Columns.Add("ContainerNo", typeof(string));
			dtOtherItem.Columns.Add("WarehouseFromId", typeof(int));
			dtOtherItem.Columns.Add("WarehouseFrom", typeof(string));
			dtOtherItem.Columns.Add("WarehouseToId", typeof(int));
			dtOtherItem.Columns.Add("WarehouseTo", typeof(string));
			dtOtherItem.Columns.Add("Remarks", typeof(string));
			grdotheritems.DataSource = dtOtherItem;
			grdotheritems.RetrieveStructure();
			grdSettingsOtherItems();
			GetLastWareHouse();
			WagesStatus = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("WagesCompulsoryOnStockConversion"));
			btnDetailUpdate.Visible = false;
			btnDetailCancel.Visible = false;
			DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("DefaultDaysToLessFromHistoryFromDate"));
			HistoryCombosFill();
			FromDateHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetLastWareHouse()
	{
		try
		{
			DataTable dt = ExImForwarding.GetLastWareHouseFromForwarding(UserAccount.OrganizationId, UserAccount.CompanyId, 1817);
			if (dt.Rows.Count > 0)
			{
				cmbWareHouseTo.Value = Conversion.ToInt(dt.Rows[0][2]);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void generateCode()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		try
		{
			int Code = ExImForwarding.GenerateCode(new ExImForwarding
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeId = 1817
			});
			if (Code > 0)
			{
				txtdocno.Text = Code.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void bindInvoiceNo()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		try
		{
			DataTable dtInvoice = ExImInvoice.GetInvoiceNoForExportForwarding(new ExImInvoice
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Id = Conversion.ToInt(cmbGpNo.Value),
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			if (dtInvoice.Rows.Count > 0)
			{
				int NoOfInvoices = dtInvoice.Rows.Count;
				txtNoofInvoices.Text = NoOfInvoices.ToString();
				DDL.BindDDLNew(dtInvoice, cmbInvoiceNo, "Id", "InvoiceNo", "Pre Invoice No", false);
			}
			else
			{
				((Control)(object)cmbInvoiceNo).Text = string.Empty;
				((Control)(object)cmbInvoiceNo).Text = "0";
				((UltraGridBase)cmbInvoiceNo).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbInvoiceNo_Leave(object sender, EventArgs e)
	{
		try
		{
			GetDeliveryOrderbyGatepassId();
			ContractORDERNo();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetGatePassDataForForwarding()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Expected O, but got Unknown
		try
		{
			ReportsParameters val = new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Id = Conversion.ToInt(cmbGpNo.Value)
			};
			DataTable dtcontainer = new DataTable
			{
				Columns = { "Id", "Container" }
			};
			DataTable dtgatepass = GatePassOutward.GetGatepassOutwardDataForForwarding(val);
			if (dtgatepass.Rows.Count > 0)
			{
				txtVehicleNo.Text = Conversion.ToString(dtgatepass.Rows[0]["VehicleNo"]);
				txtBilityNo.Text = Conversion.ToString(dtgatepass.Rows[0]["BiltyNo"]);
				txtGPDate.Value = Conversion.ToDateTime(dtgatepass.Rows[0]["GpDate"]);
				txtFreight.Text = Conversion.ToDouble(dtgatepass.Rows[0]["Freight"]).ToString(clsGlobalVariables.stringFormatsingleForFcy);
				double FcWt = Conversion.ToDouble(dtgatepass.Rows[0]["WbGrossWeight"]) - Conversion.ToDouble(dtgatepass.Rows[0]["StockWeight"]) + PreStockWt;
				txtFactoryWeight.Text = Conversion.ToDouble(dtgatepass.Rows[0]["WbGrossWeight"]).ToString("#,##0.###");
				txtDispatchWeight.Text = Conversion.ToDouble((object)(Conversion.ToDouble(dtgatepass.Rows[0]["StockWeight"]) - PreStockWt)).ToString("#,##0.###");
				txtBalFcWeight.Text = FcWt.ToString("#,##0.###");
				txtDoGrossWeight.Text = Conversion.ToDouble(dtgatepass.Rows[0]["DoGrossWeight"]).ToString("#,##0.###");
				txtDoNetWeight.Text = Conversion.ToDouble(dtgatepass.Rows[0]["DoWeight"]).ToString("#,##0.###");
				txtconainer1.Text = Conversion.ToString(dtgatepass.Rows[0]["Container"]);
				txtContainer2.Text = Conversion.ToString(dtgatepass.Rows[0]["Container1"]);
				dtcontainer.Rows.Add(dtgatepass.Rows[0]["Container"], dtgatepass.Rows[0]["Container"]);
				dtcontainer.Rows.Add(dtgatepass.Rows[0]["Container1"], dtgatepass.Rows[0]["Container1"]);
				DDL.BindDDL(dtcontainer, cmbcontainerNo, "Id", "Container", "ContainerNo", ZeroIndex: false);
			}
			else
			{
				txtDoNetWeight.Text = string.Empty;
				txtVehicleNo.Text = string.Empty;
				txtBilityNo.Text = string.Empty;
				txtconainer1.Text = string.Empty;
				txtContainer2.Text = string.Empty;
				txtFactoryWeight.Text = "0";
				txtFreight.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ContractORDERNo()
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		try
		{
			if (Conversion.ToInt(cmbInvoiceNo.Value) <= 0)
			{
				return;
			}
			DataTable dteform = ExImInvoice.GetData(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				Ids = "1816",
				Id = Conversion.ToInt(cmbInvoiceNo.Value)
			});
			if (dteform.Rows.Count <= 0)
			{
				return;
			}
			DataTable dtcustomer = new DataTable();
			dtcustomer.Columns.Add("SupplierCustomerId");
			dtcustomer.Columns.Add("Customer");
			DataTable dtDestinationPort = new DataTable();
			dtDestinationPort.Columns.Add("DestinationPortId");
			dtDestinationPort.Columns.Add("DestinationPort");
			DataTable dtLoadingPort = new DataTable();
			dtLoadingPort.Columns.Add("LoadingPortId");
			dtLoadingPort.Columns.Add("LoadingPort");
			for (int i = 0; i < dteform.Rows.Count; i++)
			{
				if (Conversion.ToInt(dteform.Rows[i]["Id"]) == Conversion.ToInt(cmbInvoiceNo.Value))
				{
					dtcustomer.Rows.Add(dteform.Rows[i]["SupplierCustomerId"], dteform.Rows[i]["Customer"]);
					dtDestinationPort.Rows.Add(dteform.Rows[i]["DestinationPortId"], dteform.Rows[i]["DestinationPort"]);
					dtLoadingPort.Rows.Add(dteform.Rows[i]["LoadingPortId"], dteform.Rows[i]["LoadingPort"]);
				}
				if (dteform.Rows.Count > 0)
				{
					DDL.BindDDL(dtcustomer, cmbCustomer, "SupplierCustomerId", "Customer", "Customer", ZeroIndex: false);
					if (dtcustomer.Rows.Count > 0)
					{
						((UltraGridBase)cmbCustomer).Rows[0].Activate();
					}
					else
					{
						((Control)(object)cmbCustomer).Text = string.Empty;
						((UltraGridBase)cmbCustomer).DataSource = null;
					}
					DDL.BindDDL(dtDestinationPort, cmbDestinationPort, "DestinationPortId", "DestinationPort", "DestinationPort", ZeroIndex: false);
					if (dtDestinationPort.Rows.Count > 0)
					{
						((Control)(object)cmbDestinationPort).Enabled = false;
						((UltraGridBase)cmbDestinationPort).Rows[0].Activate();
					}
					else
					{
						((Control)(object)cmbDestinationPort).Text = string.Empty;
						((UltraGridBase)cmbDestinationPort).DataSource = null;
					}
					DDL.BindDDL(dtLoadingPort, cmbLoadingPort, "LoadingPortId", "LoadingPort", "LoadingPort", ZeroIndex: false);
					if (dtLoadingPort.Rows.Count > 0)
					{
						((Control)(object)cmbLoadingPort).Enabled = false;
						((UltraGridBase)cmbLoadingPort).Rows[0].Activate();
					}
					else
					{
						((Control)(object)cmbLoadingPort).Text = string.Empty;
						((UltraGridBase)cmbLoadingPort).DataSource = null;
					}
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TransporterFill()
	{
		try
		{
			DataTable dt = CommonServices.GetSupplierustomerByCustomerGroupId("10");
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbtransporter, "Id", "CompanyName", "Transporter", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TransporterBind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		try
		{
			DataTable dtShipping = ExImExportShipingLineBooking.ReadByInvoiceId(new ExImExportShipingLineBooking
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				ExImInvoiceId = Conversion.ToInt(cmbInvoiceNo.Value)
			});
			if (dtShipping.Rows.Count > 0)
			{
				cmbtransporter.Value = Conversion.ToInt(dtShipping.Rows[0]["TransporterId"]);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gatepassGetAll()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dt = GatePassOutward.GetPendingGPForForwardingWeighBridgeOptional(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbGpNo, "Id", "GpSrNo", "GatePass No", ZeroIndex: false);
				return;
			}
			((UltraGridBase)cmbGpNo).DataSource = null;
			((Control)(object)cmbGpNo).Text = string.Empty;
			txtGPDate.Value = DateTime.Now;
			txtBilityNo.Text = string.Empty;
			txtVehicleNo.Text = string.Empty;
			txtFreight.Text = string.Empty;
			txtconainer1.Text = string.Empty;
			txtContainer2.Text = string.Empty;
			txtFactoryWeight.Text = string.Empty;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemFillForExport()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtItemCombo = Item.GetExportItemsByOrganizationCompanyId(new Item
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtItemCombo.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtItemCombo, cmbItem, "Id", "ItemName", "ItemName", false);
				((UltraGridBase)cmbItem).DisplayLayout.Bands[0].Columns[1].Width = 450;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void bindSeaPort()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtSeaport = SeaPorts.Getall(new SeaPorts
			{
				CompanyId = UserAccount.CompanyId,
				OrganizationId = UserAccount.OrganizationId
			});
			if (dtSeaport.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtSeaport, cmbLoadingPort, "Id", "PortName", "Port Name", false);
				DDL.BindDDLNew(dtSeaport, cmbDestinationPort, "Id", "PortName", "Port Name", false);
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
			DataTable lst = CommonServices.JobLotGetAllService();
			if (lst.Rows.Count > 0)
			{
				DDL.BindDDLNew(lst, CmbJobLot, "Id", "JobLotDescription", "JobLot Description", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CropYearFill()
	{
	}

	private void WarehouseFill()
	{
		try
		{
			DataTable dtwh = CommonServices.getActiveWareHouse();
			if (dtwh.Rows.Count > 0)
			{
				DDL.BindDDL(dtwh, CmbWareHouseFrom, "Id", "WareHouseName", "WareHouse From", ZeroIndex: false);
				DDL.BindDDL(dtwh, cmbWareHouseTo, "Id", "WareHouseName", "WareHouse To", ZeroIndex: false);
				DDL.BindDDL(dtwh, cmbWareHouseFromOtherItem, "Id", "WareHouseName", "WareHouse From", ZeroIndex: false);
				DDL.BindDDL(dtwh, cmbWareHouseToOtherItem, "Id", "WareHouseName", "WareHouse To", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void packingType()
	{
		try
		{
			try
			{
				DataTable lst = InvPackingType.Getall();
				if (lst.Rows.Count > 0)
				{
					DDL.BindDDLNew(lst, cmbPackType, "Id", "PackTypeDesc", "Packing Type", false);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}
		catch (Exception ex2)
		{
			MessageBox.Show(ex2.Message);
		}
	}

	private void BindOtherItems()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		try
		{
			DataTable lst = ExImInvoice.GetOtherItemByExImInvoiceId(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Id = Conversion.ToInt(cmbInvoiceNo.Value)
			});
			if (lst.Rows.Count > 0)
			{
				DDL.BindDDL(lst, CmbOtherItem, "ItemId", "ItemName", "Item Name", ZeroIndex: false);
				((UltraGridBase)CmbOtherItem).DisplayLayout.Bands[0].Columns[1].Width = 400;
				((UltraGridBase)CmbOtherItem).DisplayLayout.Bands[0].Columns[2].Hidden = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbItem_Leave(object sender, EventArgs e)
	{
		try
		{
			DataTable dtUom = CommonServices.GetUomScheduleByItemId(Conversion.ToInt(cmbItem.Value));
			if (dtUom.Rows.Count > 0)
			{
				DDL.BindDDL(dtUom, cmbPackUOM, "Id", "UOMCode", "PackUom", ZeroIndex: false);
			}
			else
			{
				((UltraGridBase)cmbPackUOM).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		//IL_0407: Unknown result type (might be due to invalid IL or missing references)
		//IL_040e: Expected O, but got Unknown
		//IL_0a9a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa1: Expected O, but got Unknown
		//IL_0d8f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d94: Unknown result type (might be due to invalid IL or missing references)
		//IL_0da5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0db6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dcd: Expected O, but got Unknown
		//IL_0dee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0df3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e0e: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (grdDetail.RowCount == 0)
			{
				throw new Exception("Please Check Detail Grid");
			}
			if (!formValidation())
			{
				return;
			}
			ExImForwarding forwarding = new ExImForwarding();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				forwarding.Id = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			double StockWeight = 0.0;
			double DoInvoiceWt = Conversion.ToDouble((object)txtInvoiceNetWeight.Text.Trim());
			double FactoryWeight = Conversion.ToDouble((object)txtBalFcWeight.Text.Trim());
			double num = Conversion.ToDouble((object)grdDetail.GetTotal(grdDetail.RootTable.Columns["NetWeight"], (AggregateFunction)2).ToString());
			StockWeight = Conversion.ToDouble((object)grdDetail.GetTotal(grdDetail.RootTable.Columns["StockWeight"], (AggregateFunction)2).ToString());
			if (num != DoInvoiceWt)
			{
				throw new Exception("SumOfGrid NetWeight and DoNetWeight not equal please check");
			}
			if (btnSave.Visible && btnSave.Enabled)
			{
				if (Conversion.ToInt((object)txtNoofInvoices.Text) == 1)
				{
					if (StockWeight == FactoryWeight)
					{
					}
				}
				else if (StockWeight > FactoryWeight)
				{
					throw new Exception("StockWeight cannot be greater than FactoryWeight please check");
				}
			}
			forwarding.OrganizationId = UserAccount.OrganizationId;
			forwarding.CompanyId = UserAccount.CompanyId;
			forwarding.BranchesId = Conversion.ToInt((object)UserAccount.BranchesId);
			forwarding.ProjectsId = Conversion.ToInt((object)UserAccount.BranchesId);
			forwarding.DocNo = Conversion.ToInt((object)txtdocno.Text.Trim());
			forwarding.DocDate = DocDate.Value;
			forwarding.DocumentTypeId = 1817;
			forwarding.ExImInvoiceId = Conversion.ToInt(cmbInvoiceNo.Value);
			forwarding.GatePassOutwardId = Conversion.ToInt(cmbGpNo.Value);
			forwarding.GpNo = Conversion.ToString((object)((Control)(object)cmbGpNo).Text.Trim());
			forwarding.OtherRemarks = Conversion.ToString((object)txtRemarksHeader.Text.Trim());
			forwarding.GPDate = txtGPDate.Value;
			forwarding.TransporterId = Conversion.ToInt(cmbtransporter.Value);
			forwarding.VehicleNo = txtVehicleNo.Text;
			forwarding.BiltyNo = txtBilityNo.Text;
			forwarding.DriverName = txtDriverName.Text;
			forwarding.DriverCellNo = txtDriverCellNo.Text;
			forwarding.DriverCnicNo = txtCNICNO.Text;
			forwarding.Container = txtconainer1.Text;
			forwarding.Container1 = txtContainer2.Text;
			forwarding.FreightAmt = Conversion.ToDouble((object)txtFreight.Text);
			forwarding.OtherCharges = Conversion.ToDouble((object)txtFactoryWeight.Text);
			forwarding.SupplierCustomerId = Conversion.ToInt(cmbCustomer.Value);
			forwarding.LoadingPortId = Conversion.ToInt(cmbLoadingPort.Value);
			forwarding.DestinationPortId = Conversion.ToInt(cmbDestinationPort.Value);
			forwarding.FactoryWeight = Conversion.ToDouble((object)txtFactoryWeight.Text);
			forwarding.NoOfContainer = Conversion.ToInt((object)txtNoOfContainer.Text);
			forwarding.EntryUser = UserAccount.ID;
			forwarding.ModifyUser = UserAccount.ID;
			forwarding.EntryDate = DateTime.Now;
			forwarding.ModifyDate = DateTime.Now;
			forwarding.IsApproved = false;
			forwarding.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			forwarding.ExImForwardingDetail = new List<ExImForwardingPackingDetail>();
			forwarding.ExImForwardingOtherItemslist = new List<ExImForwardingOtherItems>();
			GridEXRow[] rows = grdDetail.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				ExImForwardingPackingDetail exImForwardingPackingDetail = new ExImForwardingPackingDetail();
				exImForwardingPackingDetail.Id = Conversion.ToInt(r2.Cells["Id"].Value);
				exImForwardingPackingDetail.RefDocumentTypeId = Conversion.ToInt(r2.Cells["RefDocumentTypeId"].Value);
				exImForwardingPackingDetail.RefDocIdNo = Conversion.ToInt(r2.Cells["RefDocIdNo"].Value);
				exImForwardingPackingDetail.RefDocSubIdNo = Conversion.ToInt(r2.Cells["RefDocSubIdNo"].Value);
				exImForwardingPackingDetail.ContractDetailId = Conversion.ToInt(r2.Cells["ProformaDetailId"].Value);
				exImForwardingPackingDetail.ExImLcOrderId = Conversion.ToInt(r2.Cells["ProformaId"].Value);
				exImForwardingPackingDetail.InvoiceDetailId = Conversion.ToInt(r2.Cells["PreInvoiceDetailId"].Value);
				exImForwardingPackingDetail.InvoiceId = Conversion.ToInt(r2.Cells["PreInvoiceId"].Value);
				if (Conversion.ToInt(r2.Cells["ItemId"].Value) != 0)
				{
					exImForwardingPackingDetail.ItemId = Conversion.ToInt(r2.Cells["ItemId"].Value);
					if (Conversion.ToInt(r2.Cells["PackTypeId"].Value) != 0)
					{
						exImForwardingPackingDetail.PackingMaterialId = Conversion.ToInt(r2.Cells["PackTypeId"].Value);
						if (Conversion.ToDouble(r2.Cells["NoOfBags"].Value) != 0.0)
						{
							exImForwardingPackingDetail.OuterQty = Conversion.ToDouble((object)r2.Cells["NoOfBags"].Text.Trim());
							exImForwardingPackingDetail.InnerQty = Conversion.ToDouble((object)r2.Cells["NoOfBags"].Text.Trim());
							if (Conversion.ToInt(r2.Cells["OuterUOMId"].Value) != 0)
							{
								exImForwardingPackingDetail.UOMScheduleIdOuter = Conversion.ToInt(r2.Cells["OuterUOMId"].Value);
								exImForwardingPackingDetail.UOMScheduleIdInner = Conversion.ToInt(r2.Cells["OuterUOMId"].Value);
								if (Conversion.ToDouble(r2.Cells["NetWeight"].Value) != 0.0)
								{
									exImForwardingPackingDetail.NetWeight = Conversion.ToDouble((object)r2.Cells["NetWeight"].Text.Trim());
									exImForwardingPackingDetail.EbUnit = Conversion.ToDouble((object)r2.Cells["EbUnit"].Text.Trim());
									exImForwardingPackingDetail.EbTotal = Conversion.ToDouble((object)r2.Cells["EbTotal"].Text.Trim());
									exImForwardingPackingDetail.AdLsWeight = Conversion.ToDouble((object)r2.Cells["AddLess"].Text.Trim());
									if (Conversion.ToDouble(r2.Cells["GrossWeight"].Value) != 0.0)
									{
										exImForwardingPackingDetail.GrossWeight = Conversion.ToDouble((object)r2.Cells["GrossWeight"].Text.Trim());
										exImForwardingPackingDetail.StockWeight = Conversion.ToDouble((object)r2.Cells["StockWeight"].Text.Trim());
										if (Conversion.ToInt(r2.Cells["JobLotId"].Value) != 0)
										{
											exImForwardingPackingDetail.LotJobId = Conversion.ToInt(r2.Cells["JobLotId"].Value);
											if (Conversion.ToInt(r2.Cells["WarehouseId"].Value) != 0)
											{
												exImForwardingPackingDetail.WarehouseId = Conversion.ToInt(r2.Cells["WarehouseId"].Value);
												if (Conversion.ToInt(r2.Cells["WarehouseToId"].Value) != 0)
												{
													exImForwardingPackingDetail.WarehouseToId = Conversion.ToInt(r2.Cells["WarehouseToId"].Value);
													if (exImForwardingPackingDetail.WarehouseId == exImForwardingPackingDetail.WarehouseToId)
													{
														MessageBox.Show("Warehouse From and Warehouse To Cannot be Same In Grid");
														return;
													}
													exImForwardingPackingDetail.ContainerNo = Conversion.ToString((object)r2.Cells["Container#"].Text.Trim());
													exImForwardingPackingDetail.SealNo = Conversion.ToString((object)r2.Cells["Seal#"].Text.Trim());
													exImForwardingPackingDetail.PackingExpiryDate = Conversion.ToString((object)r2.Cells["PackingExpiryDate"].Text.Trim());
													exImForwardingPackingDetail.ProductionNo = Conversion.ToString((object)r2.Cells["ProductionNo"].Text.Trim());
													if (exImForwardingPackingDetail.PackingExpiryDate == null || exImForwardingPackingDetail.PackingExpiryDate == string.Empty)
													{
														throw new Exception("Packing Expiry Date Required in Detail Grid At Row#" + r2.RowIndex);
													}
													if (exImForwardingPackingDetail.ProductionNo == null || exImForwardingPackingDetail.ProductionNo == string.Empty)
													{
														throw new Exception("Production No Required in Detail Grid At Row#" + r2.RowIndex);
													}
													exImForwardingPackingDetail.ActionTypeId = ((exImForwardingPackingDetail.Id <= 0) ? 1 : 2);
													forwarding.ExImForwardingDetail.Add(exImForwardingPackingDetail);
													continue;
												}
												MessageBox.Show("Warehouse To Required In Grid");
												return;
											}
											MessageBox.Show("Warehouse From Required In Grid");
											return;
										}
										MessageBox.Show("Job Lot Required In Grid");
										return;
									}
									MessageBox.Show("Gross Weight Required In Grid");
									return;
								}
								MessageBox.Show("NetWeight Required In Grid");
								return;
							}
							MessageBox.Show("Pack UOM Required In Grid");
							return;
						}
						MessageBox.Show("No Of Bags Required In Grid");
						return;
					}
					MessageBox.Show("Packing Type Required In Grid");
					return;
				}
				MessageBox.Show("Item Required In Grid");
				return;
			}
			if (grdotheritems.RowCount > 0)
			{
				rows = grdotheritems.GetRows();
				foreach (GridEXRow r in rows)
				{
					ExImForwardingOtherItems vd = new ExImForwardingOtherItems();
					if (Conversion.ToInt(r.Cells["ItemId"].Value) != 0)
					{
						vd.otherItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
						if (Conversion.ToInt(r.Cells["WareHouseFromId"].Value) != 0)
						{
							vd.WareHouseFromId = Conversion.ToInt(r.Cells["WareHouseFromId"].Value);
							if (Conversion.ToInt(r.Cells["WarehouseToId"].Value) != 0)
							{
								vd.WarehouseToId = Conversion.ToInt(r.Cells["WarehouseToId"].Value);
								if (vd.WareHouseFromId == vd.WarehouseToId)
								{
									MessageBox.Show("Warehouse From and Warehouse To Cannot be Same In Other Items grid");
									return;
								}
								vd.oItemQty = Conversion.ToDecimal(r.Cells["Qty"].Value);
								vd.oItemRate = Conversion.ToDouble(r.Cells["Rate"].Value);
								if (Conversion.ToInt(r.Cells["Amount"].Value) != 0)
								{
									vd.oItemAmount = Conversion.ToInt(r.Cells["Amount"].Value);
									vd.OtherItemRemarks = Conversion.ToString(r.Cells["Remarks"].Value);
									vd.ContainerNo = Conversion.ToString(r.Cells["ContainerNo"].Value);
									forwarding.ExImForwardingOtherItemslist.Add(vd);
									continue;
								}
								MessageBox.Show("Amount Required In Other Items Grid");
								return;
							}
							MessageBox.Show("WarehouseTo To Required In Other Items Grid");
							return;
						}
						MessageBox.Show("WarehouseFrom To Required In Other Items Grid");
						return;
					}
					MessageBox.Show("Item Required In Other Items Grid");
					return;
				}
			}
			forwarding.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			forwarding.DeleteAttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<ExImForwarding>(forwarding, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
			int success = ExImForwarding.Save(forwarding);
			if (RecId > 0)
			{
				MessageBox.Show("Record Update SuccessFully");
				if (AT.RemovedAttachmentListInUpdateCase.Count > 0 && AT.lst.Count == 0)
				{
					global::Architecture.BLL.DMSAttachments.RemoveByIdAndNames(RecId, base.Name, 1817, 0);
				}
			}
			else
			{
				MessageBox.Show("Record Save SuccessFully");
			}
			if (WagesStatus)
			{
				frmwagesBillHeader frmwagesBillHeader = new frmwagesBillHeader(UserAccount);
				frmwagesBillHeader.RefDocTypeId = 1817;
				frmwagesBillHeader.RefDocId = success;
				frmwagesBillHeader.ShowDialog();
				frmwagesBillHeader.BringToFront();
			}
			FormReset();
			if (ChkPrintPreview.Checked)
			{
				DataTable dt = ExportPdfReport.ExImForwarding_Slip_507(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					Id = Conversion.ToInt((object)success)
				});
				if (dt == null || dt.Rows.Count == 0)
				{
					MessageBox.Show("Not Record Found For Display");
					return;
				}
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dt, "507-ExImForwarding_Slip.rpt");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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

	private void FormReset()
	{
		try
		{
			RecId = 0;
			PreStockWt = 0.0;
			generateCode();
			gatepassGetAll();
			GridGatepassFill();
			((Control)(object)cmbGpNo).Enabled = true;
			((Control)(object)cmbtransporter).Text = string.Empty;
			txtBalFcWeight.Text = string.Empty;
			txtNoofInvoices.Text = string.Empty;
			((Control)(object)cmbLoadingPort).Text = string.Empty;
			((Control)(object)cmbDestinationPort).Text = string.Empty;
			((Control)(object)cmbInvoiceNo).Text = string.Empty;
			((Control)(object)cmbGpNo).Text = string.Empty;
			txtFactoryWeight.Text = "0";
			txtVehicleNo.Text = string.Empty;
			txtBilityNo.Text = string.Empty;
			txtDriverName.Text = string.Empty;
			txtDriverCellNo.Text = string.Empty;
			txtCNICNO.Text = string.Empty;
			txtOtherCharges.Text = string.Empty;
			txtFreight.Text = string.Empty;
			txtNoOfContainer.Text = string.Empty;
			((Control)(object)cmbCustomer).Text = string.Empty;
			txtconainer1.Text = string.Empty;
			txtContainer2.Text = string.Empty;
			txtDoGrossWeight.Text = string.Empty;
			txtDoNetWeight.Text = string.Empty;
			txtInvoiceNetWeight.Text = string.Empty;
			((Control)(object)cmbItem).Text = string.Empty;
			((UltraGridBase)cmbItem).DataSource = null;
			cmbGpNo.Focus();
			dtDetail.Rows.Clear();
			dtOtherItem.Rows.Clear();
			grdDetail.ClearStructure();
			grdotheritems.ClearStructure();
			btnSave.Visible = true;
			UpdateMode = false;
			btnUpdate.Visible = false;
			((Control)(object)cmbInvoiceNo).Enabled = true;
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			LstRemoveRecordDetail.Clear();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FormDetailReset()
	{
		try
		{
			((Control)(object)cmbItem).Text = string.Empty;
			((Control)(object)cmbPackType).Text = string.Empty;
			txtNetWeight.Text = string.Empty;
			((Control)(object)cmbPackUOM).Text = string.Empty;
			txtGrossWeight.Text = string.Empty;
			txtPackingWeight.Text = string.Empty;
			txtPackingweightTotal.Text = string.Empty;
			txtAddLess.Text = string.Empty;
			txtStockWeight.Text = string.Empty;
			txtSeal.Text = string.Empty;
			txtPackingExpiryDate.Text = string.Empty;
			txtProductionNo.Text = string.Empty;
			((Control)(object)CmbWareHouseFrom).Text = string.Empty;
			((Control)(object)CmbJobLot).Text = string.Empty;
			((Control)(object)cmbcontainerNo).Text = string.Empty;
			btnDetailUpdate.Visible = false;
			btnDetailCancel.Visible = false;
			cmbItem.ReadOnly = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ResetOtherItems()
	{
		try
		{
			((Control)(object)CmbOtherItem).Text = string.Empty;
			txtoQty.Text = string.Empty;
			txtoRate.Text = string.Empty;
			txtoAmount.Text = string.Empty;
			txtOContainer.Text = string.Empty;
			txtoRemarks.Text = string.Empty;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			FormReset();
			RecId = Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells["Id"].Value.ToString());
			ReadById(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void ReadById(int Id)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		//IL_0378: Unknown result type (might be due to invalid IL or missing references)
		//IL_037d: Unknown result type (might be due to invalid IL or missing references)
		//IL_038e: Unknown result type (might be due to invalid IL or missing references)
		//IL_039f: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ca: Expected O, but got Unknown
		try
		{
			btnSave.Visible = false;
			btnUpdate.Visible = true;
			ExImForwarding exImLc = new ExImForwarding();
			exImLc = ExImForwarding.GetByID(Id);
			if (exImLc != null)
			{
				tabControl1.SelectedIndex = 0;
				tabControl2.SelectedIndex = 0;
				txtdocno.Text = Conversion.ToString((object)exImLc.DocNo);
				DocDate.Value = exImLc.DocDate;
				DataTable dtInv = new DataTable();
				dtInv.Columns.Add("Id");
				dtInv.Columns.Add("Name");
				dtInv.Rows.Add(exImLc.ExImInvoiceId, exImLc.InvoiceNo);
				if (dtInv.Rows.Count > 0)
				{
					DDL.BindDDL(dtInv, cmbInvoiceNo, "Id", "Name", "InvoiceNo", ZeroIndex: false);
					((UltraGridBase)cmbInvoiceNo).Rows[0].Activate();
				}
				DataTable dtGatePass = new DataTable();
				dtGatePass.Columns.Add("Id");
				dtGatePass.Columns.Add("Name");
				dtGatePass.Rows.Add(exImLc.GatePassOutwardId, exImLc.GpSrNo);
				if (dtGatePass.Rows.Count > 0)
				{
					DDL.BindDDL(dtGatePass, cmbGpNo, "Id", "Name", "GpNo", ZeroIndex: false);
					((UltraGridBase)cmbGpNo).Rows[0].Activate();
				}
				DataTable dtCustomer = new DataTable();
				dtCustomer.Columns.Add("Id");
				dtCustomer.Columns.Add("Name");
				dtCustomer.Rows.Add(exImLc.SupplierCustomerId, exImLc.Customer);
				if (dtCustomer.Rows.Count > 0)
				{
					DDL.BindDDL(dtCustomer, cmbCustomer, "Id", "Name", "CustomerName", ZeroIndex: false);
					((UltraGridBase)cmbCustomer).Rows[0].Activate();
				}
				DataTable dtDestination = new DataTable();
				dtDestination.Columns.Add("Id");
				dtDestination.Columns.Add("Name");
				dtDestination.Rows.Add(exImLc.DestinationPortId, exImLc.DestinationPort);
				if (dtDestination.Rows.Count > 0)
				{
					DDL.BindDDL(dtDestination, cmbDestinationPort, "Id", "Name", "Destination Port", ZeroIndex: false);
					((UltraGridBase)cmbDestinationPort).Rows[0].Activate();
				}
				DataTable dtLoading = new DataTable();
				dtLoading.Columns.Add("Id");
				dtLoading.Columns.Add("Name");
				dtLoading.Rows.Add(exImLc.LoadingPortId, exImLc.LoadingPort);
				if (dtLoading.Rows.Count > 0)
				{
					DDL.BindDDL(dtLoading, cmbLoadingPort, "Id", "Name", "Loading Port", ZeroIndex: false);
					((UltraGridBase)cmbLoadingPort).Rows[0].Activate();
				}
				int NoOfInvoices = ExImInvoice.GetInvoiceNoForExportForwarding(new ExImInvoice
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					Id = Conversion.ToInt(cmbGpNo.Value),
					FinancialYearId = clsGlobalVariables.ActiveYr.Id
				}).Rows.Count + 1;
				txtNoofInvoices.Text = NoOfInvoices.ToString();
				cmbtransporter.Value = Conversion.ToInt((object)exImLc.TransporterId);
				txtDriverCellNo.Text = Conversion.ToString((object)exImLc.DriverCellNo);
				txtDriverName.Text = Conversion.ToString((object)exImLc.DriverName);
				txtCNICNO.Text = Conversion.ToString((object)exImLc.DriverCnicNo);
				txtGPDate.Value = exImLc.GPDate;
				txtVehicleNo.Text = Conversion.ToString((object)exImLc.VehicleNo);
				txtBilityNo.Text = Conversion.ToString((object)exImLc.BiltyNo);
				txtconainer1.Text = Conversion.ToString((object)exImLc.Container);
				txtContainer2.Text = Conversion.ToString((object)exImLc.Container1);
				txtFactoryWeight.Text = exImLc.OtherCharges.ToString("#,##0.###");
				txtFreight.Text = exImLc.FreightAmt.ToString(clsGlobalVariables.stringFormatsingleForFcy);
				txtRemarksHeader.Text = Conversion.ToString((object)exImLc.OtherRemarks);
				txtNoOfContainer.Text = Conversion.ToString((object)exImLc.NoOfContainer);
				BindOtherItems();
				dtDetail.Rows.Clear();
				double InvoiceWt = 0.0;
				for (int i = 0; i < exImLc.ExImForwardingDetail.Count; i++)
				{
					dtDetail.Rows.Add(exImLc.ExImForwardingDetail[i].Id, exImLc.ExImForwardingDetail[i].RefDocumentTypeId, exImLc.ExImForwardingDetail[i].RefDocIdNo, exImLc.ExImForwardingDetail[i].RefDocSubIdNo, exImLc.ExImForwardingDetail[i].ContractDetailId, exImLc.ExImForwardingDetail[i].ExImLcOrderId, exImLc.ExImForwardingDetail[i].LcOrderNo, exImLc.ExImForwardingDetail[i].InvoiceId, exImLc.ExImForwardingDetail[i].InvoiceDetailId, exImLc.ExImForwardingDetail[i].ItemId, exImLc.ExImForwardingDetail[i].ItemName, exImLc.ExImForwardingDetail[i].PackingMaterialId, exImLc.ExImForwardingDetail[i].Packtype, exImLc.ExImForwardingDetail[i].OuterQty, exImLc.ExImForwardingDetail[i].UOMScheduleIdOuter, exImLc.ExImForwardingDetail[i].OuterUOM, exImLc.ExImForwardingDetail[i].NetWeight, exImLc.ExImForwardingDetail[i].EbUnit, exImLc.ExImForwardingDetail[i].EbTotal, exImLc.ExImForwardingDetail[i].AdLsWeight, exImLc.ExImForwardingDetail[i].GrossWeight, exImLc.ExImForwardingDetail[i].StockWeight, exImLc.ExImForwardingDetail[i].LotJobId, exImLc.ExImForwardingDetail[i].JobLotName, exImLc.ExImForwardingDetail[i].WarehouseId, exImLc.ExImForwardingDetail[i].WareHouseName, exImLc.ExImForwardingDetail[i].WarehouseToId, exImLc.ExImForwardingDetail[i].WareHouseToName, exImLc.ExImForwardingDetail[i].ContainerNo, exImLc.ExImForwardingDetail[i].SealNo, exImLc.ExImForwardingDetail[i].PackingExpiryDate, exImLc.ExImForwardingDetail[i].ProductionNo);
					InvoiceWt += exImLc.ExImForwardingDetail[i].NetWeight;
					PreStockWt += exImLc.ExImForwardingDetail[i].StockWeight;
				}
				txtInvoiceNetWeight.Text = InvoiceWt.ToString("#,##0.###");
				GetGatePassDataForForwarding();
				grdDetail.DataSource = dtDetail;
				grdDetail.RetrieveStructure();
				GridDetailSetting();
				dtOtherItem.Rows.Clear();
				for (int x = 0; x < exImLc.ExImForwardingOtherItemslist.Count; x++)
				{
					dtOtherItem.Rows.Add(exImLc.ExImForwardingOtherItemslist[x].otherItemId, exImLc.ExImForwardingOtherItemslist[x].ItemName, exImLc.ExImForwardingOtherItemslist[x].oItemQty, exImLc.ExImForwardingOtherItemslist[x].oItemRate, exImLc.ExImForwardingOtherItemslist[x].oItemAmount, exImLc.ExImForwardingOtherItemslist[x].ContainerNo, exImLc.ExImForwardingOtherItemslist[x].WareHouseFromId, exImLc.ExImForwardingOtherItemslist[x].WareHouseFrom, exImLc.ExImForwardingOtherItemslist[x].WarehouseToId, exImLc.ExImForwardingOtherItemslist[x].WareHouseTo, exImLc.ExImForwardingOtherItemslist[x].OtherItemRemarks);
				}
				grdotheritems.DataSource = dtOtherItem;
				grdotheritems.RetrieveStructure();
				grdSettingsOtherItems();
				UpdateMode = true;
				((Control)(object)cmbGpNo).Enabled = false;
				((Control)(object)cmbInvoiceNo).Enabled = false;
				FormHelper.LoadAttachmentsForObject(AT, Id, base.Name);
			}
			else
			{
				FormReset();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool formdetailvalidation()
	{
		if (((UltraGridBase)cmbItem).ActiveRow == null)
		{
			MessageBox.Show("Please select Item");
			cmbItem.Focus();
			return false;
		}
		if (((UltraGridBase)cmbPackType).ActiveRow == null)
		{
			MessageBox.Show("Please select Packing Type");
			cmbPackType.Focus();
			return false;
		}
		if (txtNoOfBags.Text.Trim() == "" || !(Conversion.ToDouble((object)txtNoOfBags.Text) > 0.0))
		{
			MessageBox.Show("Please Insert No Of Bags");
			txtNoOfBags.Focus();
			return false;
		}
		if (((UltraGridBase)cmbPackUOM).ActiveRow == null)
		{
			MessageBox.Show("Please Select Pack UOM");
			cmbPackUOM.Focus();
			return false;
		}
		if (txtNetWeight.Text.Trim() == "" || Conversion.ToDouble((object)txtNetWeight.Text) == 0.0)
		{
			MessageBox.Show("Please Insert NetWeight");
			txtNetWeight.Focus();
			return false;
		}
		if (txtGrossWeight.Text.Trim() == "" || !(Conversion.ToDouble((object)txtGrossWeight.Text) > 0.0))
		{
			MessageBox.Show("Please Insert Gross weight");
			txtGrossWeight.Focus();
			return false;
		}
		if (((UltraGridBase)CmbJobLot).ActiveRow == null)
		{
			MessageBox.Show("Please Select Job Lot");
			CmbJobLot.Focus();
			return false;
		}
		if (((UltraGridBase)CmbWareHouseFrom).ActiveRow == null)
		{
			MessageBox.Show("Please Select Warehouse From");
			CmbWareHouseFrom.Focus();
			return false;
		}
		if (((UltraGridBase)cmbWareHouseTo).ActiveRow == null)
		{
			MessageBox.Show("Please Select Warehouse To");
			cmbWareHouseTo.Focus();
			return false;
		}
		if (Conversion.ToInt(CmbWareHouseFrom.Value) == Conversion.ToInt(cmbWareHouseTo.Value))
		{
			MessageBox.Show("WareHouse From And Warehouse To Can not be Same...");
			cmbWareHouseTo.Focus();
			return false;
		}
		if (txtPackingExpiryDate.Text == string.Empty)
		{
			MessageBox.Show("PackingExpiryDate Field Required");
			txtPackingExpiryDate.Focus();
			return false;
		}
		if (txtProductionNo.Text == string.Empty)
		{
			MessageBox.Show("Production No Field Required");
			txtProductionNo.Focus();
			return false;
		}
		return true;
	}

	private bool formValidation()
	{
		if (txtdocno.Text == string.Empty || txtdocno.Text == null || txtdocno.Text == "0")
		{
			MessageBox.Show("Please Check Doc No");
			txtdocno.Focus();
			return false;
		}
		if (((Control)(object)cmbGpNo).Text.Trim() == "" || Conversion.ToInt(cmbGpNo.Value) == 0)
		{
			MessageBox.Show("Please Select GatePass No");
			cmbGpNo.Focus();
			return false;
		}
		if (((Control)(object)cmbInvoiceNo).Text.Trim() == string.Empty || ((Control)(object)cmbInvoiceNo).Text == null || Conversion.ToInt(cmbInvoiceNo.Value) == 0)
		{
			MessageBox.Show("Please Select Pre Invoice No");
			cmbInvoiceNo.Focus();
			return false;
		}
		if (((Control)(object)cmbCustomer).Text.Trim() == string.Empty || ((Control)(object)cmbCustomer).Text == null || Conversion.ToInt(cmbCustomer.Value) == 0)
		{
			MessageBox.Show("Please Select Customer/Importer");
			cmbCustomer.Focus();
			return false;
		}
		if (txtDriverName.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Please Enter DriverName");
			txtDriverName.Focus();
			return false;
		}
		if (txtDriverCellNo.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Please Enter Driver Cell No");
			txtDriverCellNo.Focus();
			return false;
		}
		if (txtCNICNO.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Please Enter Driver CNIC No");
			txtCNICNO.Focus();
			return false;
		}
		if (txtVehicleNo.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Please Check Vehicle No");
			txtVehicleNo.Focus();
			return false;
		}
		if (txtFactoryWeight.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtFactoryWeight.Text) == 0.0)
		{
			MessageBox.Show("Please Check Factory Weight");
			txtFactoryWeight.Focus();
			return false;
		}
		if (txtDoGrossWeight.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtDoGrossWeight.Text) == 0.0)
		{
			MessageBox.Show("Please Check Do GrossWeight");
			txtDoGrossWeight.Focus();
			return false;
		}
		if (txtDoNetWeight.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtDoNetWeight.Text) == 0.0)
		{
			MessageBox.Show("Please Check Do NetWeight");
			txtDoNetWeight.Focus();
			return false;
		}
		return true;
	}

	private bool OtherItemFormValidation()
	{
		if (((UltraGridBase)CmbOtherItem).ActiveRow == null)
		{
			MessageBox.Show("Item field required");
			CmbOtherItem.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtoQty.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Qty field required");
			txtoQty.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtoRate.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Rate field required");
			txtoRate.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtoAmount.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Amount field required");
			txtoAmount.Focus();
			return false;
		}
		if (Conversion.ToString((object)txtOContainer.Text.Trim()) == string.Empty)
		{
			MessageBox.Show("ContainerNo field required");
			txtOContainer.Focus();
			return false;
		}
		if (((UltraGridBase)cmbWareHouseFromOtherItem).ActiveRow == null)
		{
			MessageBox.Show("WareHouseFrom field required");
			cmbWareHouseFromOtherItem.Focus();
			return false;
		}
		if (((UltraGridBase)cmbWareHouseTo).ActiveRow == null)
		{
			MessageBox.Show("WareHouse To field required");
			cmbWareHouseTo.Focus();
			return false;
		}
		if (Conversion.ToInt(cmbWareHouseFromOtherItem.Value) == Conversion.ToInt((object)cmbWareHouseToOtherItem))
		{
			MessageBox.Show("WareHouse From And Warehouse To Can not be Same...");
			cmbWareHouseToOtherItem.Focus();
			return false;
		}
		return true;
	}

	private void GridGatepassFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		try
		{
			DataTable dtGridRegister = GatePassOutward.GatePassOutwardDataForExportForwarding(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			if (dtGridRegister.Rows.Count > 0)
			{
				DataTable dtcol = new DataTable();
				dtcol.Columns.Add("Id", typeof(int));
				dtcol.Columns.Add("GpDate", typeof(DateTime));
				dtcol.Columns.Add("GpSrNo", typeof(int));
				dtcol.Columns.Add("Status", typeof(string));
				dtcol.Columns.Add("VehicleType", typeof(string));
				dtcol.Columns.Add("VehicleNo", typeof(string));
				dtcol.Columns.Add("BiltyNo", typeof(string));
				dtcol.Columns.Add("Container", typeof(string));
				dtcol.Columns.Add("Container1", typeof(string));
				dtcol.Columns.Add("SupplierWeight", typeof(decimal));
				dtcol.Columns.Add("FactoryWeight", typeof(decimal));
				dtcol.Columns.Add("DifferenceWeight", typeof(decimal));
				dtcol.Rows.Clear();
				for (int i = 0; i < dtGridRegister.Rows.Count; i++)
				{
					dtcol.Rows.Add(dtGridRegister.Rows[i]["Id"], dtGridRegister.Rows[i]["GpDate"], dtGridRegister.Rows[i]["GpSrNo"], dtGridRegister.Rows[i]["Status"], dtGridRegister.Rows[i]["VehicleType"], dtGridRegister.Rows[i]["VehicleNo"], dtGridRegister.Rows[i]["BiltyNo"], dtGridRegister.Rows[i]["Container"], dtGridRegister.Rows[i]["Container1"], dtGridRegister.Rows[i]["SupplierWeight"], dtGridRegister.Rows[i]["FactoryWeight"], dtGridRegister.Rows[i]["DifferenceWeight"]);
				}
				grdgpInfo.DataSource = dtcol;
				grdgpInfo.RetrieveStructure();
				grdgpInfo.RootTable.Columns["Id"].Visible = false;
				grdgpInfo.RootTable.Columns["SupplierWeight"].AggregateFunction = (AggregateFunction)2;
				grdgpInfo.RootTable.Columns["SupplierWeight"].TextAlignment = (TextAlignment)3;
				grdgpInfo.RootTable.Columns["SupplierWeight"].FormatString = "##,#.##";
				grdgpInfo.RootTable.Columns["SupplierWeight"].TotalFormatString = "##,#.##";
				grdgpInfo.RootTable.Columns["FactoryWeight"].AggregateFunction = (AggregateFunction)2;
				grdgpInfo.RootTable.Columns["FactoryWeight"].TextAlignment = (TextAlignment)3;
				grdgpInfo.RootTable.Columns["FactoryWeight"].FormatString = "##,#.##";
				grdgpInfo.RootTable.Columns["FactoryWeight"].TotalFormatString = "##,#.##";
				grdgpInfo.RootTable.Columns["DifferenceWeight"].AggregateFunction = (AggregateFunction)2;
				grdgpInfo.RootTable.Columns["DifferenceWeight"].TextAlignment = (TextAlignment)3;
				grdgpInfo.RootTable.Columns["DifferenceWeight"].FormatString = "##,#.##";
				grdgpInfo.RootTable.Columns["DifferenceWeight"].TotalFormatString = "##,#.##";
				CommonServices.GridAutoAdjustmentNew(grdgpInfo);
				grdgpInfo.RootTable.Columns.Add("Load");
				grdgpInfo.RootTable.Columns["Load"].Key = "Load";
				grdgpInfo.RootTable.Columns["Load"].Caption = "Load";
				grdgpInfo.RootTable.Columns["Load"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grdgpInfo.RootTable.Columns["Load"].ButtonStyle = (ButtonStyle)5;
				grdgpInfo.RootTable.Columns["Load"].ButtonText = "Load";
				grdgpInfo.RootTable.Columns["Load"].Width = 60;
				grdgpInfo.RootTable.Columns["Load"].Position = 0;
				grdgpInfo.FrozenColumns = 1;
			}
			else
			{
				grdgpInfo.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdgpInfo_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Load")
			{
				FormReset();
				GridEXRow item = grdgpInfo.CurrentRow;
				if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
				{
					int GPId = Conversion.ToInt(item.Cells["Id"].Value);
					cmbGpNo.Value = GPId;
					cmbGpNo_Leave(null, null);
				}
			}
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

	private void btnAddinGrid_Click(object sender, EventArgs e)
	{
		try
		{
			if (formdetailvalidation())
			{
				dtDetail.Rows.Add(Conversion.ToInt((object)0), Conversion.ToInt((object)0), Conversion.ToInt((object)0), Conversion.ToInt((object)0), Conversion.ToInt((object)0), Conversion.ToInt((object)0), Conversion.ToInt((object)0), Conversion.ToInt((object)0), Conversion.ToInt((object)0), Conversion.ToInt(cmbItem.Value), Conversion.ToString((object)((Control)(object)cmbItem).Text), Conversion.ToInt(cmbPackType.Value), Conversion.ToString((object)((Control)(object)cmbPackType).Text), Conversion.ToDouble((object)txtNoOfBags.Text.Trim()), Conversion.ToInt(cmbPackUOM.Value), Conversion.ToString((object)((Control)(object)cmbPackUOM).Text), Conversion.ToDouble((object)txtNetWeight.Text.Trim()), Conversion.ToDouble((object)txtPackingWeight.Text.Trim()), Conversion.ToDouble((object)txtPackingweightTotal.Text.Trim()), Conversion.ToDouble((object)txtAddLess.Text.Trim()), Conversion.ToDouble((object)txtGrossWeight.Text.Trim()), Conversion.ToDouble((object)txtStockWeight.Text.Trim()), Conversion.ToInt(CmbJobLot.Value), Conversion.ToString((object)((Control)(object)CmbJobLot).Text.Trim()), Conversion.ToInt(CmbWareHouseFrom.Value), Conversion.ToString((object)((Control)(object)CmbWareHouseFrom).Text.Trim()), Conversion.ToInt(cmbWareHouseTo.Value), Conversion.ToString((object)((Control)(object)cmbWareHouseTo).Text.Trim()), Conversion.ToString((object)((Control)(object)cmbcontainerNo).Text.Trim()), Conversion.ToString((object)txtSeal.Text), Conversion.ToString((object)txtPackingExpiryDate.Text), Conversion.ToString((object)txtProductionNo.Text));
				grdDetail.DataSource = dtDetail;
				grdDetail.RetrieveStructure();
				FormDetailReset();
				GridDetailSetting();
			}
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
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("ItemName");
				cmbItem.Value = item.Cells["ItemId"].Value;
				((Control)(object)cmbItem).Text = item.Cells["ItemName"].Text.ToString();
				dt.Rows.Add(item.Cells["ItemId"].Value, item.Cells["ItemName"].Text.ToString());
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, cmbItem, "Id", "ItemName", "ItemName", ZeroIndex: false);
					((UltraGridBase)cmbItem).Rows[0].Activate();
					((UltraGridBase)cmbItem).DisplayLayout.Bands[0].Columns[1].Width = 450;
				}
				cmbItem_Leave(null, null);
				cmbPackType.Value = item.Cells["PackTypeId"].Value;
				txtNoOfBags.Text = item.Cells["NoOfBags"].Text.ToString();
				cmbPackUOM.Value = item.Cells["OuterUOMId"].Text.ToString();
				txtNetWeight.Text = item.Cells["NetWeight"].Value.ToString();
				txtPackingWeight.Text = item.Cells["EbUnit"].Text.ToString();
				txtPackingweightTotal.Text = item.Cells["EbTotal"].Text.ToString();
				txtAddLess.Text = item.Cells["AddLess"].Value.ToString();
				txtGrossWeight.Text = item.Cells["GrossWeight"].Value.ToString();
				txtStockWeight.Text = item.Cells["StockWeight"].Value.ToString();
				CmbJobLot.Value = item.Cells["JobLotId"].Value.ToString();
				CmbWareHouseFrom.Value = item.Cells["WarehouseId"].Value.ToString();
				((Control)(object)CmbWareHouseFrom).Text = item.Cells["Warehouse"].Text.ToString();
				cmbWareHouseTo.Value = item.Cells["WarehouseToId"].Value.ToString();
				((Control)(object)cmbWareHouseTo).Text = item.Cells["WarehouseTo"].Text.ToString();
				((Control)(object)cmbcontainerNo).Text = item.Cells["Container#"].Value.ToString();
				txtSeal.Text = item.Cells["Seal#"].Value.ToString();
				txtPackingExpiryDate.Text = item.Cells["PackingExpiryDate"].Value.ToString();
				txtProductionNo.Text = item.Cells["ProductionNo"].Value.ToString();
				btnDetailUpdate.Visible = true;
				btnDetailCancel.Visible = true;
				btnAddinGrid.Visible = false;
				cmbItem.Focus();
				WeightCalculation();
				NetWeightCalculation();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnDetailUpdate_Click(object sender, EventArgs e)
	{
		try
		{
			if (formdetailvalidation())
			{
				dtDetail.Rows[updateDetailIndex]["ItemId"] = Conversion.ToInt(cmbItem.Value);
				dtDetail.Rows[updateDetailIndex]["ItemName"] = Conversion.ToString((object)((Control)(object)cmbItem).Text);
				dtDetail.Rows[updateDetailIndex]["PackTypeId"] = Conversion.ToInt(cmbPackType.Value);
				dtDetail.Rows[updateDetailIndex]["Packtype"] = Conversion.ToString((object)((Control)(object)cmbPackType).Text);
				dtDetail.Rows[updateDetailIndex]["NoOfBags"] = Conversion.ToDouble((object)txtNoOfBags.Text);
				dtDetail.Rows[updateDetailIndex]["OuterUOMId"] = Conversion.ToInt(cmbPackUOM.Value);
				dtDetail.Rows[updateDetailIndex]["OuterUOM"] = Conversion.ToString((object)((Control)(object)cmbPackUOM).Text);
				dtDetail.Rows[updateDetailIndex]["NetWeight"] = Conversion.ToDouble((object)txtNetWeight.Text);
				dtDetail.Rows[updateDetailIndex]["EbUnit"] = Conversion.ToDouble((object)txtPackingWeight.Text);
				dtDetail.Rows[updateDetailIndex]["EbTotal"] = Conversion.ToDouble((object)txtPackingweightTotal.Text);
				dtDetail.Rows[updateDetailIndex]["AddLess"] = Conversion.ToDouble((object)txtAddLess.Text);
				dtDetail.Rows[updateDetailIndex]["GrossWeight"] = Conversion.ToDouble((object)txtGrossWeight.Text);
				dtDetail.Rows[updateDetailIndex]["StockWeight"] = Math.Round(Conversion.ToDouble((object)txtStockWeight.Text), 3);
				dtDetail.Rows[updateDetailIndex]["WarehouseId"] = Conversion.ToInt(CmbWareHouseFrom.Value);
				dtDetail.Rows[updateDetailIndex]["Warehouse"] = Conversion.ToString((object)((Control)(object)CmbWareHouseFrom).Text);
				dtDetail.Rows[updateDetailIndex]["WarehouseToId"] = Conversion.ToInt(cmbWareHouseTo.Value);
				dtDetail.Rows[updateDetailIndex]["WarehouseTo"] = Conversion.ToString((object)((Control)(object)cmbWareHouseTo).Text);
				dtDetail.Rows[updateDetailIndex]["JobLotId"] = Conversion.ToInt(CmbJobLot.Value);
				dtDetail.Rows[updateDetailIndex]["JobLot"] = Conversion.ToString((object)((Control)(object)CmbJobLot).Text);
				dtDetail.Rows[updateDetailIndex]["Container#"] = Conversion.ToString((object)((Control)(object)cmbcontainerNo).Text);
				dtDetail.Rows[updateDetailIndex]["Seal#"] = Conversion.ToString((object)txtSeal.Text);
				dtDetail.Rows[updateDetailIndex]["PackingExpiryDate"] = Conversion.ToString((object)txtPackingExpiryDate.Text);
				dtDetail.Rows[updateDetailIndex]["ProductionNo"] = Conversion.ToString((object)txtProductionNo.Text);
				grdDetail.DataSource = dtDetail;
				grdDetail.RetrieveStructure();
				GridDetailSetting();
				btnDetailUpdate.Visible = false;
				btnDetailCancel.Visible = false;
				FormDetailReset();
				cmbItem.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetail_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0456: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Expected O, but got Unknown
		//IL_0443: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdDetail.CurrentRow;
			if (e.Column.Key == "Delete" && r != null && (int)r.RowType == 0)
			{
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					ExImForwardingPackingDetail exImForwardingPackingDetail = new ExImForwardingPackingDetail();
					exImForwardingPackingDetail.Id = Conversion.ToInt(r.Cells["Id"].Value);
					exImForwardingPackingDetail.RefDocumentTypeId = Conversion.ToInt(r.Cells["RefDocumentTypeId"].Value);
					exImForwardingPackingDetail.RefDocIdNo = Conversion.ToInt(r.Cells["RefDocIdNo"].Value);
					exImForwardingPackingDetail.RefDocSubIdNo = Conversion.ToInt(r.Cells["RefDocSubIdNo"].Value);
					exImForwardingPackingDetail.ContractDetailId = Conversion.ToInt(r.Cells["ProformaDetailId"].Value);
					exImForwardingPackingDetail.ExImLcOrderId = Conversion.ToInt(r.Cells["ProformaId"].Value);
					exImForwardingPackingDetail.InvoiceDetailId = Conversion.ToInt(r.Cells["PreInvoiceDetailId"].Value);
					exImForwardingPackingDetail.InvoiceId = Conversion.ToInt(r.Cells["PreInvoiceId"].Value);
					exImForwardingPackingDetail.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					exImForwardingPackingDetail.PackingMaterialId = Conversion.ToInt(r.Cells["PackTypeId"].Value);
					exImForwardingPackingDetail.OuterQty = Conversion.ToDouble((object)r.Cells["NoOfBags"].Text.Trim());
					exImForwardingPackingDetail.InnerQty = Conversion.ToDouble((object)r.Cells["NoOfBags"].Text.Trim());
					exImForwardingPackingDetail.UOMScheduleIdOuter = Conversion.ToInt(r.Cells["OuterUOMId"].Value);
					exImForwardingPackingDetail.UOMScheduleIdInner = Conversion.ToInt(r.Cells["OuterUOMId"].Value);
					exImForwardingPackingDetail.NetWeight = Conversion.ToDouble((object)r.Cells["NetWeight"].Text.Trim());
					exImForwardingPackingDetail.EbUnit = Conversion.ToDouble((object)r.Cells["EbUnit"].Text.Trim());
					exImForwardingPackingDetail.EbTotal = Conversion.ToDouble((object)r.Cells["EbTotal"].Text.Trim());
					exImForwardingPackingDetail.AdLsWeight = Conversion.ToDouble((object)r.Cells["AddLess"].Text.Trim());
					exImForwardingPackingDetail.GrossWeight = Conversion.ToDouble((object)r.Cells["GrossWeight"].Text.Trim());
					exImForwardingPackingDetail.StockWeight = Conversion.ToDouble((object)r.Cells["StockWeight"].Text.Trim());
					exImForwardingPackingDetail.LotJobId = Conversion.ToInt(r.Cells["JobLotId"].Value);
					exImForwardingPackingDetail.WarehouseId = Conversion.ToInt(r.Cells["WarehouseId"].Value);
					exImForwardingPackingDetail.WarehouseToId = Conversion.ToInt(r.Cells["WarehouseToId"].Value);
					exImForwardingPackingDetail.ContainerNo = Conversion.ToString((object)r.Cells["Container#"].Text.Trim());
					exImForwardingPackingDetail.SealNo = Conversion.ToString((object)r.Cells["Seal#"].Text.Trim());
					exImForwardingPackingDetail.PackingExpiryDate = Conversion.ToString((object)r.Cells["PackingExpiryDate"].Text.Trim());
					exImForwardingPackingDetail.ProductionNo = Conversion.ToString((object)r.Cells["ProductionNo"].Text.Trim());
					exImForwardingPackingDetail.ActionTypeId = 3;
					LstRemoveRecordDetail.Add(exImForwardingPackingDetail);
					dtDetail.Rows[r.RowIndex].Delete();
					if (r != null && (int)r.RowType == 0)
					{
						r.Delete();
					}
				}
				else if (r != null && (int)r.RowType == 0)
				{
					r.Delete();
				}
			}
			if (e.Column.Key == "Add")
			{
				DataRow dr = dtDetail.Rows[r.RowIndex];
				dtDetail.Rows.Add(dr.ItemArray);
				dtDetail.Rows[dtDetail.Rows.Count - 1]["Id"] = 0;
				grdDetail.DataSource = dtDetail;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridDetailSetting()
	{
		try
		{
			grdDetail.RootTable.Columns["Id"].Visible = false;
			grdDetail.RootTable.Columns["RefDocumentTypeId"].Visible = false;
			grdDetail.RootTable.Columns["RefDocIdNo"].Visible = false;
			grdDetail.RootTable.Columns["RefDocSubIdNo"].Visible = false;
			grdDetail.RootTable.Columns["ProformaDetailId"].Visible = false;
			grdDetail.RootTable.Columns["ProformaId"].Visible = false;
			grdDetail.RootTable.Columns["ProformaNo"].Visible = false;
			grdDetail.RootTable.Columns["PreInvoiceId"].Visible = false;
			grdDetail.RootTable.Columns["PreInvoiceDetailId"].Visible = false;
			grdDetail.RootTable.Columns["ItemId"].Visible = false;
			grdDetail.RootTable.Columns["PackTypeId"].Visible = false;
			grdDetail.RootTable.Columns["OuterUOMId"].Visible = false;
			grdDetail.RootTable.Columns["JobLotId"].Visible = false;
			grdDetail.RootTable.Columns["WarehouseId"].Visible = false;
			grdDetail.RootTable.Columns["WarehouseToId"].Visible = false;
			grdDetail.RootTable.Columns["ItemName"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["Packtype"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["NetWeight"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["OuterUOM"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["NoOfBags"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["EbUnit"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["EbTotal"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["AddLess"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["Container#"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["Seal#"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["JobLot"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["Warehouse"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["JobLot"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["StockWeight"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["PackingExpiryDate"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["ProductionNo"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["NoOfBags"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["NoOfBags"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["NoOfBags"].FormatString = "#,##0.###";
			grdDetail.RootTable.Columns["NoOfBags"].TotalFormatString = "#,##0.###";
			grdDetail.RootTable.Columns["EbUnit"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["EbUnit"].FormatString = "#,##0.###";
			grdDetail.RootTable.Columns["EbTotal"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["EbTotal"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["EbTotal"].FormatString = "#,##0.###";
			grdDetail.RootTable.Columns["EbTotal"].TotalFormatString = "#,##0.###";
			grdDetail.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["StockWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["NetWeight"].FormatString = "#,##0.###";
			grdDetail.RootTable.Columns["NetWeight"].TotalFormatString = "#,##0.###";
			grdDetail.RootTable.Columns["GrossWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["StockWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["GrossWeight"].FormatString = "#,##0.###";
			grdDetail.RootTable.Columns["GrossWeight"].TotalFormatString = "#,##0.###";
			grdDetail.RootTable.Columns["StockWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["StockWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["StockWeight"].FormatString = "#,##0.###";
			grdDetail.RootTable.Columns["StockWeight"].TotalFormatString = "#,##0.###";
			grdDetail.RootTable.Columns["AddLess"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["AddLess"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["AddLess"].FormatString = "#,##0.###";
			grdDetail.RootTable.Columns["AddLess"].TotalFormatString = "#,##0.###";
			grdDetail.RootTable.Columns["Packtype"].Caption = "Packing Type";
			grdDetail.RootTable.Columns["OuterUOM"].Caption = "Pack Uom";
			grdDetail.RootTable.Columns["EbUnit"].Caption = "Packing Weight";
			grdDetail.RootTable.Columns["EbTotal"].Caption = "Total Packing Weight";
			grdDetail.RootTable.Columns.Add("Add");
			grdDetail.RootTable.Columns["Add"].Key = "Add";
			grdDetail.RootTable.Columns["Add"].Caption = "+";
			grdDetail.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdDetail.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdDetail.RootTable.Columns["Add"].ButtonText = "+";
			grdDetail.RootTable.Columns["Add"].Width = 20;
			grdDetail.RootTable.Columns["Add"].Position = 0;
			grdDetail.FrozenColumns = 1;
			CommonServices.GridAutoAdjustmentNew(grdDetail);
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettingsOtherItems()
	{
		try
		{
			grdotheritems.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdotheritems.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdotheritems.RootTable.Columns["Qty"].FormatString = "0,0";
			grdotheritems.RootTable.Columns["Qty"].TotalFormatString = "#,##0.##";
			grdotheritems.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdotheritems.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdotheritems.RootTable.Columns["Amount"].FormatString = "0,0";
			grdotheritems.RootTable.Columns["Amount"].TotalFormatString = "#,##0.##";
			grdotheritems.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdotheritems.RootTable.Columns["Rate"].FormatString = "0,0";
			grdotheritems.RootTable.Columns["ItemId"].Visible = false;
			grdotheritems.RootTable.Columns["WarehouseFromId"].Visible = false;
			grdotheritems.RootTable.Columns["WarehouseToId"].Visible = false;
			CommonServices.GridAutoAdjustment(AryColWidth, grdotheritems);
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
			double GrossWt = 0.0;
			double AddLss = 0.0;
			double DoNetWt = 0.0;
			double TotalPckWt = 0.0;
			double PckWt = 0.0;
			double StockWeight = 0.0;
			double FcWeight = 0.0;
			double DoTotalWt = 0.0;
			double PckUom = 0.0;
			double NoofBags = 0.0;
			txtGrossWeight.Text = 0.ToString();
			txtStockWeight.Text = 0.ToString();
			FcWeight = Conversion.ToDouble((object)txtFactoryWeight.Text);
			DoTotalWt = Conversion.ToDouble((object)txtDoNetWeight.Text);
			DoNetWt = Conversion.ToDouble((object)txtNetWeight.Text);
			PckWt = Conversion.ToDouble((object)txtPackingWeight.Text);
			AddLss = Conversion.ToDouble((object)txtAddLess.Text);
			NoofBags = Conversion.ToDouble((object)txtNoOfBags.Text);
			if (((UltraGridBase)cmbPackUOM).ActiveRow != null)
			{
				PckUom = Conversion.ToDouble(((UltraDropDownBase)cmbPackUOM).SelectedRow.Cells[2].Value);
				if (PckWt > 0.0 && PckUom > 0.0 && NoofBags > 0.0)
				{
					TotalPckWt = NoofBags * PckWt;
					txtPackingweightTotal.Text = TotalPckWt.ToString();
				}
				else
				{
					txtPackingweightTotal.Text = "0";
				}
				if (DoNetWt > 0.0 && FcWeight > 0.0 && DoTotalWt > 0.0)
				{
					GrossWt = DoNetWt + TotalPckWt + AddLss;
					txtGrossWeight.Text = GrossWt.ToString();
					StockWeight = FcWeight / DoTotalWt * DoNetWt - TotalPckWt + AddLss;
					txtStockWeight.Text = Math.Round(StockWeight, 3).ToString();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void NetWeightCalculation()
	{
		try
		{
			double NetWeight = 0.0;
			double OuterUom = 0.0;
			double NoOfBags = 0.0;
			if (((UltraGridBase)cmbPackUOM).ActiveRow != null)
			{
				NoOfBags = Conversion.ToDouble((object)txtNoOfBags.Text);
				OuterUom = Conversion.ToDouble(((UltraDropDownBase)cmbPackUOM).SelectedRow.Cells[2].Value);
				if (NoOfBags > 0.0 && OuterUom > 0.0)
				{
					NetWeight = NoOfBags * OuterUom;
					txtNetWeight.Text = Conversion.ToString((object)NetWeight);
				}
				else
				{
					txtNetWeight.Text = "0";
				}
			}
			else
			{
				txtNetWeight.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbOuterUOM_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			NetWeightCalculation();
			WeightCalculation();
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
		else
		{
			DocDate.Focus();
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
			grdHistoryDetail.DataSource = null;
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
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		try
		{
			DataTable dtCustomer = new DataTable();
			dtCustomer.Columns.Add("Id", typeof(int));
			dtCustomer.Columns.Add("Customer", typeof(string));
			DataTable dt = ExImForwarding.GetDataForDropDownFromGoodsForwarding(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId
			});
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToString(dt.Rows[i]["Activity"]) == "Customer")
					{
						dtCustomer.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
				}
			}
			DDL.BindDDL(dtCustomer, CmbCustomerHistory, "Id", "Customer", "Party", ZeroIndex: false);
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
			GridBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridBind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters order = new ReportsParameters();
			order.OrganizationId = UserAccount.OrganizationId;
			order.CompanyId = UserAccount.CompanyId;
			order.BranchesId = UserAccount.BranchesId;
			order.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			order.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			if (drdocdate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					order.FromDate = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					order.ToDate = ToDateHistory.Value;
				}
			}
			else if (rdentrydate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					order.EntryDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					order.EntryDateTo = ToDateHistory.Value;
				}
			}
			else if (rdmodifydate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					order.ModifyDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					order.ModifyDateTo = ToDateHistory.Value;
				}
			}
			else if (rdapproveddate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					order.ApprovedDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					order.ApprovedDateTo = ToDateHistory.Value;
				}
			}
			order.FromDocNo = Conversion.ToInt((object)FromDocNoHistory.Text);
			order.ToDocNo = Conversion.ToInt((object)ToDocNoHistory.Text);
			order.SupplierCustomerId = Conversion.ToInt(CmbCustomerHistory.Value);
			DataTable dt = ExImForwarding.FormHistory(order);
			if (dt.Rows.Count > 0)
			{
				DataTable dtGrid = new DataTable();
				dtGrid.Columns.Add("Id", typeof(int));
				dtGrid.Columns.Add("DocNo", typeof(int));
				dtGrid.Columns.Add("DocDate", typeof(DateTime));
				dtGrid.Columns.Add("GPNo", typeof(int));
				dtGrid.Columns.Add("GPDate", typeof(DateTime));
				dtGrid.Columns.Add("PreInvoice#", typeof(string));
				dtGrid.Columns.Add("Customer/Importer", typeof(string));
				dtGrid.Columns.Add("TransporterName", typeof(string));
				dtGrid.Columns.Add("DriverName", typeof(string));
				dtGrid.Columns.Add("DriverCellNo", typeof(string));
				dtGrid.Columns.Add("DriverCNIC", typeof(string));
				dtGrid.Columns.Add("FreightAmount", typeof(double));
				dtGrid.Columns.Add("VehicleNo", typeof(string));
				dtGrid.Columns.Add("BiltyNo", typeof(string));
				dtGrid.Columns.Add("Remarks", typeof(string));
				dtGrid.Columns.Add("EntryUser", typeof(string));
				dtGrid.Columns.Add("EntryDate", typeof(DateTime));
				dtGrid.Columns.Add("ModifyUser", typeof(string));
				dtGrid.Columns.Add("ModifyDate", typeof(DateTime));
				dtGrid.Columns.Add("ApprovedStatus", typeof(string));
				dtGrid.Columns.Add("ApprovedUser", typeof(string));
				dtGrid.Columns.Add("ApprovedDate", typeof(DateTime));
				dtGrid.Columns.Add("NoOfAttachments", typeof(int));
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtGrid.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["DocNo"], Conversion.ToDateTime(dt.Rows[i]["DocDate"]).ToString("dd-MMM-yyyy"), dt.Rows[i]["GpNo"], Conversion.ToDateTime(dt.Rows[i]["GPDate"]).ToString("dd-MMM-yyyy"), dt.Rows[i]["InvoiceNo"], dt.Rows[i]["CustomerName"], dt.Rows[i]["TransporterName"], dt.Rows[i]["DriverName"], dt.Rows[i]["DriverCellNo"], dt.Rows[i]["DriverCnicNo"], dt.Rows[i]["FreightAmt"], dt.Rows[i]["VehicleNo"], dt.Rows[i]["BiltyNo"], dt.Rows[i]["OtherRemarks"], dt.Rows[i]["EntryUserName"], Conversion.ToDateTime(dt.Rows[i]["EntryDate"]).ToString("dd-MMM-yyyy"), dt.Rows[i]["ModifyUserName"], Conversion.ToDateTime(dt.Rows[i]["ModifyDate"]).ToString("dd-MMM-yyyy"), dt.Rows[i]["ApprovedStatus"], dt.Rows[i]["ApprovedUserName"], Conversion.ToDateTime(dt.Rows[i]["ApprovedDate"]).ToString("dd-MMM-yyyy"), dt.Rows[i]["NoOfAttachments"]);
				}
				DataGridHistory.DataSource = dtGrid;
				DataGridHistory.RetrieveStructure();
				HistoryGridSettings();
			}
			else
			{
				DataGridHistory.ClearStructure();
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
			DataGridHistory.RootTable.Columns["FreightAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["FreightAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["FreightAmount"].FormatString = "##,##.##";
			DataGridHistory.RootTable.Columns["FreightAmount"].TotalFormatString = "##,##.##";
			DataGridHistory.RootTable.Columns.Add("Edit");
			DataGridHistory.RootTable.Columns["Edit"].Caption = "Edit";
			DataGridHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			DataGridHistory.RootTable.Columns["Edit"].Width = 50;
			DataGridHistory.RootTable.Columns.Add("Print");
			DataGridHistory.RootTable.Columns["Print"].Caption = "Print";
			DataGridHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Print"].ButtonText = "Print";
			DataGridHistory.RootTable.Columns["Print"].Width = 50;
			DataGridHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			CommonServices.GridAutoAdjustment(AryColWidth, DataGridHistory);
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void BtnPrint_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = ExportPdfReport.ExImForwarding_Slip_507(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Id = Conversion.ToInt((object)RecId)
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "507-ExImForwarding_Slip.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnNew_Click(object sender, EventArgs e)
	{
		try
		{
			FormReset();
			FormDetailReset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnDetailCancel_Click(object sender, EventArgs e)
	{
		try
		{
			FormDetailReset();
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
			gatepassGetAll();
			TransporterFill();
			packingType();
			bindSeaPort();
			JobLotFill();
			WarehouseFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ExpfrmForwarding_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.S && tabControl1.SelectedIndex == 0 && !UpdateMode)
			{
				btnSave_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					BtnNew_Click(null, null);
				}
				else
				{
					BtnNew_Click(null, null);
				}
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
			if (e.Control && e.KeyCode == Keys.U && tabControl1.SelectedIndex == 0 && UpdateMode)
			{
				btnUpdate_Click(null, null);
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
			MessageBox.Show(ex.ToString());
		}
	}

	private void DataGridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Expected O, but got Unknown
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Edit")
			{
				FormReset();
				RecId = Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells["Id"].Value.ToString());
				ReadById(RecId);
			}
			if (e.Column.Key == "Print")
			{
				DataTable dt = new DataTable();
				dt = ExportPdfReport.ExImForwarding_Slip_507(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					Id = Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells["Id"].Value.ToString())
				});
				if (dt == null || dt.Rows.Count == 0)
				{
					MessageBox.Show("Not Record Found For Display");
					return;
				}
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dt, "507-ExImForwarding_Slip.rpt");
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

	private void cmbGpNo_TextChanged(object sender, EventArgs e)
	{
		try
		{
			GetGatePassDataForForwarding();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void GetDeliveryOrderbyGatepassId()
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Expected O, but got Unknown
		try
		{
			grdDetail.ClearStructure();
			double TotalNetWeight = 0.0;
			double TotalGrossWeight = 0.0;
			DataTable dtItem = new DataTable();
			dtItem.Columns.Add("Id");
			dtItem.Columns.Add("ItemName");
			DataTable dtDeliveryOrder = InvDeliveryOrder.DeliveryOrderDetailForExportGdnForwarding(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				ExImInvoiceId = Conversion.ToInt(cmbInvoiceNo.Value),
				Id = Conversion.ToInt(cmbGpNo.Value)
			});
			if (dtDeliveryOrder.Rows.Count <= 0)
			{
				return;
			}
			double ItemNetWeight = Conversion.ToDouble(dtDeliveryOrder.Compute("Sum(NetWeight)", ""));
			dtDetail.Rows.Clear();
			for (int i = 0; i < dtDeliveryOrder.Rows.Count; i++)
			{
				TotalNetWeight += Conversion.ToDouble(dtDeliveryOrder.Rows[i]["NetWeight"]);
				TotalGrossWeight += Conversion.ToDouble(dtDeliveryOrder.Rows[i]["GrossWeight"]);
				dtItem.Rows.Add(dtDeliveryOrder.Rows[i]["ItemId"], dtDeliveryOrder.Rows[i]["ItemName"]);
				if (RecId == 0 && !btnUpdate.Visible)
				{
					double StockWeight = 0.0;
					StockWeight = ((!(Conversion.ToDouble((object)txtDispatchWeight.Text) > 0.0)) ? Math.Round(Conversion.ToDouble((object)txtBalFcWeight.Text) / Conversion.ToDouble((object)txtDoNetWeight.Text) * Conversion.ToDouble(dtDeliveryOrder.Rows[i]["NetWeight"]) - Conversion.ToDouble(dtDeliveryOrder.Rows[i]["PackingWeight"]), 3) : Math.Round(Conversion.ToDouble((object)txtBalFcWeight.Text) / ItemNetWeight * Conversion.ToDouble(dtDeliveryOrder.Rows[i]["NetWeight"]) - Conversion.ToDouble(dtDeliveryOrder.Rows[i]["PackingWeight"]), 3));
					dtDetail.Rows.Add(0, Conversion.ToInt(dtDeliveryOrder.Rows[i]["RefDocumentTypeId"]), Conversion.ToInt(dtDeliveryOrder.Rows[i]["RefDocIdNo"]), Conversion.ToInt(dtDeliveryOrder.Rows[i]["RefDocSubIdNo"]), Conversion.ToInt(dtDeliveryOrder.Rows[i]["ContractDetailId"]), Conversion.ToInt(dtDeliveryOrder.Rows[i]["ContractId"]), Conversion.ToString(dtDeliveryOrder.Rows[i]["ProformaNo"]), Conversion.ToInt(cmbInvoiceNo.Value), Conversion.ToInt(dtDeliveryOrder.Rows[i]["InvoiceDetailId"]), Conversion.ToInt(dtDeliveryOrder.Rows[i]["ItemId"]), Conversion.ToString(dtDeliveryOrder.Rows[i]["ItemName"]), Conversion.ToInt(dtDeliveryOrder.Rows[i]["PackingMaterialTypeId"]), Conversion.ToString(dtDeliveryOrder.Rows[i]["PackMaterilaType"]), Conversion.ToDouble(dtDeliveryOrder.Rows[i]["OuterQty"]), Conversion.ToInt(dtDeliveryOrder.Rows[i]["OuterQtyUomId"]), Conversion.ToString(dtDeliveryOrder.Rows[i]["OuterUOM"]), Conversion.ToDouble(dtDeliveryOrder.Rows[i]["NetWeight"]), Conversion.ToDouble(dtDeliveryOrder.Rows[i]["EbUnit"]), Conversion.ToDouble(dtDeliveryOrder.Rows[i]["EbUnit"]) * Conversion.ToDouble(dtDeliveryOrder.Rows[i]["OuterQty"]), 0, Conversion.ToDouble(dtDeliveryOrder.Rows[i]["GrossWeight"]), StockWeight, Conversion.ToInt(dtDeliveryOrder.Rows[i]["JobLotId"]), Conversion.ToString(dtDeliveryOrder.Rows[i]["JobLotName"]), Conversion.ToInt(dtDeliveryOrder.Rows[i]["WarehouseId"]), Conversion.ToString(dtDeliveryOrder.Rows[i]["WareHouseName"]), Conversion.ToInt(dtDeliveryOrder.Rows[i]["WarehouseId"]), Conversion.ToString(dtDeliveryOrder.Rows[i]["WareHouseName"]), "", "", Conversion.ToString(dtDeliveryOrder.Rows[i]["PackingExpiryDate"]), "");
				}
			}
			txtInvoiceNetWeight.Text = TotalNetWeight.ToString("#,##0.###");
			txtDoGrossWeight.Text = TotalGrossWeight.ToString("#,##0.###");
			grdDetail.DataSource = dtDetail;
			grdDetail.RetrieveStructure();
			GridDetailSetting();
			if (dtItem.Rows.Count > 0)
			{
				DDL.BindDDL(dtItem, cmbItem, "Id", "ItemName", "ItemName", ZeroIndex: false);
				if (dtItem.Rows.Count == 1)
				{
					((UltraGridBase)cmbItem).Rows[0].Activate();
				}
				((UltraGridBase)cmbItem).DisplayLayout.Bands[0].Columns[1].Width = 450;
			}
			ExImInvoice ex = ExImInvoice.GetByID(Conversion.ToInt(cmbInvoiceNo.Value));
			dtOtherItem.Rows.Clear();
			BindOtherItems();
			for (int x = 0; x < ex.ExImInvoiceOtherItemslist.Count; x++)
			{
				dtOtherItem.Rows.Add(Conversion.ToInt((object)ex.ExImInvoiceOtherItemslist[x].otherItemId), Conversion.ToString((object)ex.ExImInvoiceOtherItemslist[x].ItemName), Conversion.ToDouble((object)ex.ExImInvoiceOtherItemslist[x].oItemQty), Conversion.ToDouble((object)ex.ExImInvoiceOtherItemslist[x].oItemRate), Conversion.ToDouble((object)ex.ExImInvoiceOtherItemslist[x].oItemAmount), Conversion.ToString((object)txtconainer1.Text), Conversion.ToInt((object)0), Conversion.ToString((object)""), Conversion.ToInt((object)0), Conversion.ToString((object)""), Conversion.ToString((object)ex.ExImInvoiceOtherItemslist[x].OtherItemRemarks));
			}
			grdotheritems.DataSource = dtOtherItem;
			grdotheritems.RetrieveStructure();
			grdSettingsOtherItems();
		}
		catch (Exception ex2)
		{
			MessageBox.Show(ex2.Message);
		}
	}

	private void toolStripButton1_Click(object sender, EventArgs e)
	{
		try
		{
			GridBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnoUpdate_Click(object sender, EventArgs e)
	{
		try
		{
			if (OtherItemFormValidation())
			{
				dtOtherItem.Rows[updateDetailIndexOther]["ItemId"] = Conversion.ToInt(CmbOtherItem.Value);
				dtOtherItem.Rows[updateDetailIndexOther]["ItemName"] = Conversion.ToString((object)((Control)(object)CmbOtherItem).Text);
				dtOtherItem.Rows[updateDetailIndexOther]["Qty"] = Conversion.ToDouble((object)txtoQty.Text);
				dtOtherItem.Rows[updateDetailIndexOther]["Rate"] = Conversion.ToDouble((object)txtoRate.Text);
				dtOtherItem.Rows[updateDetailIndexOther]["Amount"] = Conversion.ToDouble((object)txtoAmount.Text);
				dtOtherItem.Rows[updateDetailIndexOther]["ContainerNo"] = Conversion.ToString((object)txtOContainer.Text);
				dtOtherItem.Rows[updateDetailIndexOther]["WarehouseToId"] = Conversion.ToInt(cmbWareHouseToOtherItem.Value);
				dtOtherItem.Rows[updateDetailIndexOther]["WarehouseTo"] = Conversion.ToString((object)((Control)(object)cmbWareHouseToOtherItem).Text);
				dtOtherItem.Rows[updateDetailIndexOther]["WareHouseFromId"] = Conversion.ToInt(cmbWareHouseFromOtherItem.Value);
				dtOtherItem.Rows[updateDetailIndexOther]["WareHouseFrom"] = Conversion.ToString((object)((Control)(object)cmbWareHouseFromOtherItem).Text);
				dtOtherItem.Rows[updateDetailIndexOther]["Remarks"] = Conversion.ToString((object)txtoRemarks.Text);
				grdotheritems.DataSource = dtOtherItem;
				btnoadd.Visible = true;
				btnoUpdate.Visible = false;
				btnoCancel.Visible = false;
				ResetOtherItems();
				txtoQty.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OtherItemsCalculations()
	{
		try
		{
			double Qty = Conversion.ToDouble((object)txtoQty.Text.Trim());
			double Rate = Conversion.ToDouble((object)txtoRate.Text.Trim());
			if (Qty > 0.0 && Rate > 0.0)
			{
				txtoAmount.Text = (Qty * Rate).ToString(clsGlobalVariables.stringFormatsingleForFcy);
			}
			else
			{
				txtoAmount.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtoQty_TextChanged(object sender, EventArgs e)
	{
		OtherItemsCalculations();
	}

	private void grdotheritems_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grdotheritems.CurrentRow;
			if (item != null)
			{
				updateDetailIndexOther = item.RowIndex;
				CmbOtherItem.Value = item.Cells["ItemId"].Value.ToString();
				cmbWareHouseFromOtherItem.Value = item.Cells["WareHouseFromId"].Value.ToString();
				cmbWareHouseToOtherItem.Value = item.Cells["WarehouseToId"].Value.ToString();
				txtoQty.Text = Conversion.ToDouble(item.Cells["Qty"].Value).ToString("#,##0.###");
				txtoRate.Text = Conversion.ToDouble(item.Cells["Rate"].Value).ToString(clsGlobalVariables.DecimalFCYRateFormate);
				txtoAmount.Text = Conversion.ToDouble(item.Cells["Amount"].Value).ToString(clsGlobalVariables.stringFormatsingleForFcy);
				txtOContainer.Text = Conversion.ToString(item.Cells["ContainerNo"].Value);
				txtoRemarks.Text = item.Cells["Remarks"].Value.ToString();
				btnoadd.Visible = false;
				btnoUpdate.Visible = true;
				btnoCancel.Visible = true;
				txtoQty.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnoCancel_Click(object sender, EventArgs e)
	{
		try
		{
			btnoadd.Visible = true;
			btnoUpdate.Visible = false;
			btnoCancel.Visible = false;
			ResetOtherItems();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbGpNo_Leave(object sender, EventArgs e)
	{
		try
		{
			GetGatePassDataForForwarding();
			bindInvoiceNo();
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
			NetWeightCalculation();
			WeightCalculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtEbUnit_TextChanged(object sender, EventArgs e)
	{
		WeightCalculation();
	}

	private void txtAddLess_TextChanged(object sender, EventArgs e)
	{
		WeightCalculation();
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

	private void DataGridHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value), 1817);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tabPage3_Click(object sender, EventArgs e)
	{
	}

	private void txtFreight_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtNoOfBags_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtEbUnit_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtAddLess_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunctionWithMinus(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtoQty_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtoRate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			OtherItemsCalculations();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Invalid comparison between Unknown and I4
		try
		{
			GridEXRow item = DataGridHistory.CurrentRow;
			if (item != null && (int)item.RowType == 0 && (int)item.RowType != 5 && item.Cells.Count > 0)
			{
				GetDetailGrdByHeadId();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetDetailGrdByHeadId()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("RefDocumentTypeId", typeof(int));
			dt.Columns.Add("RefDocIdNo", typeof(int));
			dt.Columns.Add("RefDocSubIdNo", typeof(int));
			dt.Columns.Add("ProformaDetailId", typeof(int));
			dt.Columns.Add("ProformaId", typeof(int));
			dt.Columns.Add("ProformaNo", typeof(string));
			dt.Columns.Add("PreInvoiceId", typeof(int));
			dt.Columns.Add("PreInvoiceDetailId", typeof(int));
			dt.Columns.Add("ItemId", typeof(int));
			dt.Columns.Add("ItemName", typeof(string));
			dt.Columns.Add("PackingType", typeof(string));
			dt.Columns.Add("NoOfBags", typeof(double));
			dt.Columns.Add("PackUOM", typeof(string));
			dt.Columns.Add("NetWeight", typeof(double));
			dt.Columns.Add("PackingWeight", typeof(double));
			dt.Columns.Add("TotalPackingWeight", typeof(double));
			dt.Columns.Add("AddLess", typeof(double));
			dt.Columns.Add("GrossWeight", typeof(double));
			dt.Columns.Add("StockWeight", typeof(double));
			dt.Columns.Add("JobLot", typeof(string));
			dt.Columns.Add("Warehouse", typeof(string));
			dt.Columns.Add("WarehouseTo", typeof(string));
			dt.Columns.Add("Container#", typeof(string));
			dt.Columns.Add("Seal#", typeof(string));
			dt.Columns.Add("PackingExpiryDate", typeof(string));
			dt.Columns.Add("ProductionNo", typeof(string));
			dt.Clear();
			ExImForwarding exImLc = ExImForwarding.GetByID(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value));
			if (exImLc.ExImForwardingDetail.Count > 0)
			{
				dt.Rows.Clear();
				for (int i = 0; i < exImLc.ExImForwardingDetail.Count; i++)
				{
					dt.Rows.Add(exImLc.ExImForwardingDetail[i].Id, exImLc.ExImForwardingDetail[i].RefDocumentTypeId, exImLc.ExImForwardingDetail[i].RefDocIdNo, exImLc.ExImForwardingDetail[i].RefDocSubIdNo, exImLc.ExImForwardingDetail[i].ContractDetailId, exImLc.ExImForwardingDetail[i].ExImLcOrderId, exImLc.ExImForwardingDetail[i].LcOrderNo, exImLc.ExImForwardingDetail[i].InvoiceId, exImLc.ExImForwardingDetail[i].InvoiceDetailId, exImLc.ExImForwardingDetail[i].ItemId, exImLc.ExImForwardingDetail[i].ItemName, exImLc.ExImForwardingDetail[i].Packtype, exImLc.ExImForwardingDetail[i].OuterQty, exImLc.ExImForwardingDetail[i].OuterUOM, exImLc.ExImForwardingDetail[i].NetWeight, exImLc.ExImForwardingDetail[i].EbUnit, exImLc.ExImForwardingDetail[i].EbTotal, exImLc.ExImForwardingDetail[i].AdLsWeight, exImLc.ExImForwardingDetail[i].GrossWeight, exImLc.ExImForwardingDetail[i].StockWeight, exImLc.ExImForwardingDetail[i].JobLotName, exImLc.ExImForwardingDetail[i].WareHouseName, exImLc.ExImForwardingDetail[i].WareHouseToName, exImLc.ExImForwardingDetail[i].ContainerNo, exImLc.ExImForwardingDetail[i].SealNo, exImLc.ExImForwardingDetail[i].PackingExpiryDate, exImLc.ExImForwardingDetail[i].ProductionNo);
				}
				grdHistoryDetail.DataSource = dt;
				grdHistoryDetail.RetrieveStructure();
				GridDetailSettingHistory();
			}
			else
			{
				grdHistoryDetail.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridDetailSettingHistory()
	{
		try
		{
			grdHistoryDetail.RootTable.Columns["Id"].Visible = false;
			grdHistoryDetail.RootTable.Columns["RefDocumentTypeId"].Visible = false;
			grdHistoryDetail.RootTable.Columns["RefDocIdNo"].Visible = false;
			grdHistoryDetail.RootTable.Columns["RefDocSubIdNo"].Visible = false;
			grdHistoryDetail.RootTable.Columns["ProformaDetailId"].Visible = false;
			grdHistoryDetail.RootTable.Columns["ProformaId"].Visible = false;
			grdHistoryDetail.RootTable.Columns["ProformaNo"].Visible = false;
			grdHistoryDetail.RootTable.Columns["PreInvoiceId"].Visible = false;
			grdHistoryDetail.RootTable.Columns["PreInvoiceDetailId"].Visible = false;
			grdHistoryDetail.RootTable.Columns["ItemId"].Visible = false;
			grdHistoryDetail.RootTable.Columns["StockWeight"].AggregateFunction = (AggregateFunction)2;
			grdHistoryDetail.RootTable.Columns["GrossWeight"].AggregateFunction = (AggregateFunction)2;
			grdHistoryDetail.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			grdHistoryDetail.RootTable.Columns["NoOfBags"].AggregateFunction = (AggregateFunction)2;
			grdHistoryDetail.RootTable.Columns["PackingWeight"].AggregateFunction = (AggregateFunction)2;
			grdHistoryDetail.RootTable.Columns["TotalPackingWeight"].AggregateFunction = (AggregateFunction)2;
			grdHistoryDetail.RootTable.Columns["AddLess"].AggregateFunction = (AggregateFunction)2;
			grdHistoryDetail.RootTable.Columns["StockWeight"].FormatString = "#,##0.##";
			grdHistoryDetail.RootTable.Columns["GrossWeight"].FormatString = "#,##0.##";
			grdHistoryDetail.RootTable.Columns["NetWeight"].FormatString = "#,##0.###";
			grdHistoryDetail.RootTable.Columns["NoOfBags"].FormatString = "#,##0.##";
			grdHistoryDetail.RootTable.Columns["PackingWeight"].FormatString = "#,##0.##";
			grdHistoryDetail.RootTable.Columns["TotalPackingWeight"].FormatString = "#,##0.##";
			grdHistoryDetail.RootTable.Columns["AddLess"].FormatString = "#,##0.##";
			grdHistoryDetail.RootTable.Columns["NoOfBags"].TotalFormatString = "#,##0.##";
			grdHistoryDetail.RootTable.Columns["GrossWeight"].TotalFormatString = "#,##0.##";
			grdHistoryDetail.RootTable.Columns["StockWeight"].TotalFormatString = "#,##0.##";
			grdHistoryDetail.RootTable.Columns["NetWeight"].TotalFormatString = "#,##0.##";
			grdHistoryDetail.RootTable.Columns["PackingWeight"].TotalFormatString = "#,##0.##";
			grdHistoryDetail.RootTable.Columns["TotalPackingWeight"].TotalFormatString = "#,##0.##";
			grdHistoryDetail.RootTable.Columns["AddLess"].TotalFormatString = "#,##0.##";
			CommonServices.GridAutoAdjustment(AryColWidth, grdHistoryDetail);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmForwardingNew_FormClosing(object sender, FormClosingEventArgs e)
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

	private void btnDelete_Click(object sender, EventArgs e)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Expected O, but got Unknown
		try
		{
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					InvPurchaseInvoice.RemoveByID(new InvPurchaseInvoice
					{
						OrganizationId = UserAccount.OrganizationId,
						CompanyId = UserAccount.CompanyId,
						DocumentTypeId = 1817,
						Id = RecId,
						EntryUser = UserAccount.ID
					});
					MessageBox.Show("Delete Record Seccessfully");
					FormReset();
				}
				return;
			}
			throw new Exception("Record Not Found");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtFactoryWeight_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtFactoryWeight_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToDouble((object)txtFactoryWeight.Text) > 0.0)
			{
				txtBalFcWeight.Text = (Conversion.ToDouble((object)txtFactoryWeight.Text) - Conversion.ToDouble((object)txtDispatchWeight.Text)).ToString("#,##0.###");
			}
			else
			{
				txtBalFcWeight.Text = "0";
			}
			if (grdDetail.RowCount <= 0)
			{
				return;
			}
			GridEXRow[] rows = grdDetail.GetRows();
			foreach (GridEXRow item in rows)
			{
				item.BeginEdit();
				if (Conversion.ToDouble((object)txtBalFcWeight.Text) > 0.0)
				{
					double StockWeight = 0.0;
					StockWeight = ((!(Conversion.ToDouble((object)txtDispatchWeight.Text) > 0.0)) ? Math.Round(Conversion.ToDouble((object)txtBalFcWeight.Text) / Conversion.ToDouble((object)txtDoNetWeight.Text) * Conversion.ToDouble(item.Cells["NetWeight"].Value) - Conversion.ToDouble(item.Cells["EbTotal"].Value), 3) : Math.Round(Conversion.ToDouble((object)txtBalFcWeight.Text) / Conversion.ToDouble((object)txtInvoiceNetWeight.Text) * Conversion.ToDouble(item.Cells["NetWeight"].Value) - Conversion.ToDouble(item.Cells["EbTotal"].Value), 3));
					item.Cells["StockWeight"].Value = StockWeight;
				}
				else
				{
					item.Cells["StockWeight"].Value = 0;
				}
				item.EndEdit();
			}
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
		//IL_05f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0600: Expected O, but got Unknown
		//IL_0622: Unknown result type (might be due to invalid IL or missing references)
		//IL_062c: Expected O, but got Unknown
		//IL_0685: Unknown result type (might be due to invalid IL or missing references)
		//IL_068f: Expected O, but got Unknown
		//IL_06bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c6: Expected O, but got Unknown
		//IL_06c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d1: Expected O, but got Unknown
		//IL_0756: Unknown result type (might be due to invalid IL or missing references)
		//IL_0760: Expected O, but got Unknown
		//IL_0761: Unknown result type (might be due to invalid IL or missing references)
		//IL_076b: Expected O, but got Unknown
		//IL_0782: Unknown result type (might be due to invalid IL or missing references)
		//IL_078c: Expected O, but got Unknown
		//IL_07f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07fa: Expected O, but got Unknown
		//IL_0811: Unknown result type (might be due to invalid IL or missing references)
		//IL_081b: Expected O, but got Unknown
		//IL_083d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0847: Expected O, but got Unknown
		//IL_0853: Unknown result type (might be due to invalid IL or missing references)
		//IL_085d: Expected O, but got Unknown
		//IL_090e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0918: Expected O, but got Unknown
		//IL_0a21: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a2b: Expected O, but got Unknown
		//IL_0a58: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a62: Expected O, but got Unknown
		//IL_0adc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae6: Expected O, but got Unknown
		//IL_0b13: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b1d: Expected O, but got Unknown
		//IL_0b60: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b6a: Expected O, but got Unknown
		//IL_0b6b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b75: Expected O, but got Unknown
		//IL_0c10: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c1a: Expected O, but got Unknown
		//IL_0c1b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c25: Expected O, but got Unknown
		//IL_0ccb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cd5: Expected O, but got Unknown
		//IL_142e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1438: Expected O, but got Unknown
		//IL_16ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_16d4: Expected O, but got Unknown
		//IL_a5ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_a5b6: Expected O, but got Unknown
		//IL_a5da: Unknown result type (might be due to invalid IL or missing references)
		//IL_a5e4: Expected O, but got Unknown
		GridEXLayout grdgpInfo_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance73 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance99 = new Appearance();
		GridEXLayout grdotheritems_DesignTimeLayout = new GridEXLayout();
		Appearance appearance136 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance172 = new Appearance();
		Appearance appearance183 = new Appearance();
		Appearance appearance194 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance180 = new Appearance();
		Appearance appearance181 = new Appearance();
		Appearance appearance182 = new Appearance();
		Appearance appearance184 = new Appearance();
		Appearance appearance185 = new Appearance();
		Appearance appearance189 = new Appearance();
		Appearance appearance190 = new Appearance();
		Appearance appearance191 = new Appearance();
		Appearance appearance202 = new Appearance();
		Appearance appearance203 = new Appearance();
		Appearance appearance204 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance186 = new Appearance();
		Appearance appearance187 = new Appearance();
		Appearance appearance188 = new Appearance();
		Appearance appearance192 = new Appearance();
		Appearance appearance193 = new Appearance();
		Appearance appearance195 = new Appearance();
		Appearance appearance196 = new Appearance();
		Appearance appearance197 = new Appearance();
		Appearance appearance198 = new Appearance();
		Appearance appearance199 = new Appearance();
		Appearance appearance200 = new Appearance();
		Appearance appearance201 = new Appearance();
		GridEXLayout grdHistoryDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance156 = new Appearance();
		Appearance appearance157 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance168 = new Appearance();
		Appearance appearance169 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance171 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel4 = new System.Windows.Forms.Panel();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.grdgpInfo = new GridEX();
		this.panel14 = new System.Windows.Forms.Panel();
		this.label38 = new System.Windows.Forms.Label();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.grdDetail = new GridEX();
		this.panel7 = new System.Windows.Forms.Panel();
		this.label53 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel5 = new System.Windows.Forms.Panel();
		this.label26 = new System.Windows.Forms.Label();
		this.txtProductionNo = new System.Windows.Forms.TextBox();
		this.label25 = new System.Windows.Forms.Label();
		this.txtPackingExpiryDate = new System.Windows.Forms.TextBox();
		this.cmbWareHouseTo = new UltraCombo();
		this.label20 = new System.Windows.Forms.Label();
		this.panel11 = new System.Windows.Forms.Panel();
		this.label52 = new System.Windows.Forms.Label();
		this.label50 = new System.Windows.Forms.Label();
		this.cmbcontainerNo = new UltraCombo();
		this.cmbItem = new UltraCombo();
		this.label45 = new System.Windows.Forms.Label();
		this.label47 = new System.Windows.Forms.Label();
		this.label46 = new System.Windows.Forms.Label();
		this.label44 = new System.Windows.Forms.Label();
		this.label39 = new System.Windows.Forms.Label();
		this.label28 = new System.Windows.Forms.Label();
		this.txtStockWeight = new System.Windows.Forms.TextBox();
		this.txtPackingWeight = new System.Windows.Forms.TextBox();
		this.txtPackingweightTotal = new System.Windows.Forms.TextBox();
		this.txtAddLess = new System.Windows.Forms.TextBox();
		this.txtGrossWeight = new System.Windows.Forms.TextBox();
		this.label29 = new System.Windows.Forms.Label();
		this.CmbJobLot = new UltraCombo();
		this.cmbPackType = new UltraCombo();
		this.label55 = new System.Windows.Forms.Label();
		this.label31 = new System.Windows.Forms.Label();
		this.CmbWareHouseFrom = new UltraCombo();
		this.label54 = new System.Windows.Forms.Label();
		this.label37 = new System.Windows.Forms.Label();
		this.btnDetailCancel = new System.Windows.Forms.Button();
		this.label32 = new System.Windows.Forms.Label();
		this.btnDetailUpdate = new System.Windows.Forms.Button();
		this.txtNoOfBags = new System.Windows.Forms.TextBox();
		this.txtSeal = new System.Windows.Forms.TextBox();
		this.btnAddinGrid = new System.Windows.Forms.Button();
		this.label30 = new System.Windows.Forms.Label();
		this.cmbPackUOM = new UltraCombo();
		this.txtNetWeight = new System.Windows.Forms.TextBox();
		this.tabPage4 = new System.Windows.Forms.TabPage();
		this.grdotheritems = new GridEX();
		this.panel13 = new System.Windows.Forms.Panel();
		this.label51 = new System.Windows.Forms.Label();
		this.panel9 = new System.Windows.Forms.Panel();
		this.cmbWareHouseToOtherItem = new UltraCombo();
		this.label16 = new System.Windows.Forms.Label();
		this.cmbWareHouseFromOtherItem = new UltraCombo();
		this.label13 = new System.Windows.Forms.Label();
		this.panel12 = new System.Windows.Forms.Panel();
		this.label6 = new System.Windows.Forms.Label();
		this.txtOContainer = new System.Windows.Forms.TextBox();
		this.label42 = new System.Windows.Forms.Label();
		this.btnoCancel = new System.Windows.Forms.Button();
		this.btnoUpdate = new System.Windows.Forms.Button();
		this.btnoadd = new System.Windows.Forms.Button();
		this.label27 = new System.Windows.Forms.Label();
		this.txtoRemarks = new System.Windows.Forms.TextBox();
		this.txtoQty = new System.Windows.Forms.TextBox();
		this.label41 = new System.Windows.Forms.Label();
		this.txtoRate = new System.Windows.Forms.TextBox();
		this.label63 = new System.Windows.Forms.Label();
		this.label70 = new System.Windows.Forms.Label();
		this.txtoAmount = new System.Windows.Forms.TextBox();
		this.CmbOtherItem = new UltraCombo();
		this.label71 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.txtDoGrossWeight = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.txtInvoiceNetWeight = new System.Windows.Forms.TextBox();
		this.txtDoNetWeight = new System.Windows.Forms.TextBox();
		this.InvoiceNetWeight = new System.Windows.Forms.Label();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.txtFactoryWeight = new System.Windows.Forms.TextBox();
		this.label35 = new System.Windows.Forms.Label();
		this.label43 = new System.Windows.Forms.Label();
		this.txtBalFcWeight = new System.Windows.Forms.TextBox();
		this.label49 = new System.Windows.Forms.Label();
		this.txtDispatchWeight = new System.Windows.Forms.TextBox();
		this.txtNoofInvoices = new System.Windows.Forms.TextBox();
		this.txtFreight = new System.Windows.Forms.TextBox();
		this.label8 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.txtVehicleNo = new System.Windows.Forms.TextBox();
		this.txtBilityNo = new System.Windows.Forms.TextBox();
		this.label48 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.cmbLoadingPort = new UltraCombo();
		this.label22 = new System.Windows.Forms.Label();
		this.txtContainer2 = new System.Windows.Forms.TextBox();
		this.label23 = new System.Windows.Forms.Label();
		this.txtNoOfContainer = new System.Windows.Forms.TextBox();
		this.cmbDestinationPort = new UltraCombo();
		this.label34 = new System.Windows.Forms.Label();
		this.txtconainer1 = new System.Windows.Forms.TextBox();
		this.label24 = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.ChkPrintPreview = new System.Windows.Forms.CheckBox();
		this.txtOtherCharges = new System.Windows.Forms.TextBox();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.txtRemarksHeader = new System.Windows.Forms.RichTextBox();
		this.label5 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.txtDriverCellNo = new System.Windows.Forms.TextBox();
		this.cmbtransporter = new UltraCombo();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.txtDriverName = new System.Windows.Forms.TextBox();
		this.label17 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.cmbInvoiceNo = new UltraCombo();
		this.DocDate = new System.Windows.Forms.DateTimePicker();
		this.label11 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.txtCNICNO = new System.Windows.Forms.TextBox();
		this.cmbCustomer = new UltraCombo();
		this.cmbGpNo = new UltraCombo();
		this.label4 = new System.Windows.Forms.Label();
		this.txtGPDate = new System.Windows.Forms.DateTimePicker();
		this.label15 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.label36 = new System.Windows.Forms.Label();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.BtnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.BtnPrint = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.grdHistoryDetail = new GridEX();
		this.DataGridHistory = new GridEX();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel15 = new System.Windows.Forms.Panel();
		this.label92 = new System.Windows.Forms.Label();
		this.panel16 = new System.Windows.Forms.Panel();
		this.groupBox4 = new System.Windows.Forms.GroupBox();
		this.rdapproveddate = new System.Windows.Forms.RadioButton();
		this.btnShowHistory = new System.Windows.Forms.Button();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.label40 = new System.Windows.Forms.Label();
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
		this.panel17 = new System.Windows.Forms.Panel();
		this.label69 = new System.Windows.Forms.Label();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.btnResetHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.panel18 = new System.Windows.Forms.Panel();
		this.label93 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel4.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.tabPage3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdgpInfo).BeginInit();
		this.panel14.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel7.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbWareHouseTo).BeginInit();
		this.panel11.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbcontainerNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPackType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWareHouseFrom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPackUOM).BeginInit();
		this.tabPage4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdotheritems).BeginInit();
		this.panel13.SuspendLayout();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbWareHouseToOtherItem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbWareHouseFromOtherItem).BeginInit();
		this.panel12.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbOtherItem).BeginInit();
		this.panel3.SuspendLayout();
		this.groupBox3.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbLoadingPort).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDestinationPort).BeginInit();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbtransporter).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbInvoiceNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCustomer).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbGpNo).BeginInit();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel15.SuspendLayout();
		this.panel16.SuspendLayout();
		this.groupBox4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).BeginInit();
		this.panel17.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		this.panel18.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1154, 761);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1154, 761);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1146, 735);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel4.Controls.Add(this.tabControl2);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(3, 244);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1140, 488);
		this.panel4.TabIndex = 4;
		this.tabControl2.Controls.Add(this.tabPage3);
		this.tabControl2.Controls.Add(this.tabPage4);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl2.Location = new System.Drawing.Point(0, 0);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(1140, 488);
		this.tabControl2.TabIndex = 0;
		this.tabPage3.Controls.Add((System.Windows.Forms.Control)(object)this.grdgpInfo);
		this.tabPage3.Controls.Add(this.panel14);
		this.tabPage3.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.tabPage3.Controls.Add(this.panel7);
		this.tabPage3.Controls.Add(this.panel5);
		this.tabPage3.Location = new System.Drawing.Point(4, 22);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage3.Size = new System.Drawing.Size(1132, 462);
		this.tabPage3.TabIndex = 0;
		this.tabPage3.Text = "Detail";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.tabPage3.Click += new System.EventHandler(tabPage3_Click);
		this.grdgpInfo.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdgpInfo).BackColor = System.Drawing.Color.White;
		grdgpInfo_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdgpInfo.DesignTimeLayout = grdgpInfo_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdgpInfo).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdgpInfo).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdgpInfo.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdgpInfo).Location = new System.Drawing.Point(3, 340);
		((System.Windows.Forms.Control)(object)this.grdgpInfo).Name = "grdgpInfo";
		((ScrollableBase)this.grdgpInfo).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdgpInfo).Size = new System.Drawing.Size(1126, 119);
		((System.Windows.Forms.Control)(object)this.grdgpInfo).TabIndex = 525;
		this.grdgpInfo.TotalRow = (InheritableBoolean)1;
		this.grdgpInfo.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdgpInfo).VisualStyle = (VisualStyle)4;
		this.grdgpInfo.ColumnButtonClick += new ColumnActionEventHandler(grdgpInfo_ColumnButtonClick);
		this.panel14.BackColor = System.Drawing.Color.Teal;
		this.panel14.Controls.Add(this.label38);
		this.panel14.Controls.Add(this.ctrlGrdBar3);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel14.Location = new System.Drawing.Point(3, 318);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(1126, 22);
		this.panel14.TabIndex = 526;
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label38.ForeColor = System.Drawing.Color.White;
		this.label38.Location = new System.Drawing.Point(3, 2);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(159, 17);
		this.label38.TabIndex = 101;
		this.label38.Text = "Gate Pass information";
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(1084, 0);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.grdgpInfo;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(39, 21);
		this.ctrlGrdBar3.TabIndex = 36;
		this.grdDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdDetail).BackColor = System.Drawing.Color.White;
		grdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Top;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(3, 150);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1126, 168);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 1;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.grdDetail.ColumnButtonClick += new ColumnActionEventHandler(grdDetail_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdDetail).DoubleClick += new System.EventHandler(grdDetail_DoubleClick);
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.label53);
		this.panel7.Controls.Add(this.ctrlGrdBar1);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(3, 128);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1126, 22);
		this.panel7.TabIndex = 524;
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label53.ForeColor = System.Drawing.Color.White;
		this.label53.Location = new System.Drawing.Point(3, 2);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(228, 17);
		this.label53.TabIndex = 101;
		this.label53.Text = "Export Goods Forwarding Detail";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1084, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdDetail;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(39, 21);
		this.ctrlGrdBar1.TabIndex = 36;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.panel5.Controls.Add(this.label26);
		this.panel5.Controls.Add(this.txtProductionNo);
		this.panel5.Controls.Add(this.label25);
		this.panel5.Controls.Add(this.txtPackingExpiryDate);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbWareHouseTo);
		this.panel5.Controls.Add(this.label20);
		this.panel5.Controls.Add(this.panel11);
		this.panel5.Controls.Add(this.label50);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbcontainerNo);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItem);
		this.panel5.Controls.Add(this.label45);
		this.panel5.Controls.Add(this.label47);
		this.panel5.Controls.Add(this.label46);
		this.panel5.Controls.Add(this.label44);
		this.panel5.Controls.Add(this.label39);
		this.panel5.Controls.Add(this.label28);
		this.panel5.Controls.Add(this.txtStockWeight);
		this.panel5.Controls.Add(this.txtPackingWeight);
		this.panel5.Controls.Add(this.txtPackingweightTotal);
		this.panel5.Controls.Add(this.txtAddLess);
		this.panel5.Controls.Add(this.txtGrossWeight);
		this.panel5.Controls.Add(this.label29);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobLot);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPackType);
		this.panel5.Controls.Add(this.label55);
		this.panel5.Controls.Add(this.label31);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWareHouseFrom);
		this.panel5.Controls.Add(this.label54);
		this.panel5.Controls.Add(this.label37);
		this.panel5.Controls.Add(this.btnDetailCancel);
		this.panel5.Controls.Add(this.label32);
		this.panel5.Controls.Add(this.btnDetailUpdate);
		this.panel5.Controls.Add(this.txtNoOfBags);
		this.panel5.Controls.Add(this.txtSeal);
		this.panel5.Controls.Add(this.btnAddinGrid);
		this.panel5.Controls.Add(this.label30);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPackUOM);
		this.panel5.Controls.Add(this.txtNetWeight);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(3, 3);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1126, 125);
		this.panel5.TabIndex = 0;
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label26.Location = new System.Drawing.Point(871, 75);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(85, 15);
		this.label26.TabIndex = 530;
		this.label26.Text = "Production No";
		this.label26.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtProductionNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtProductionNo.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtProductionNo.Location = new System.Drawing.Point(868, 94);
		this.txtProductionNo.Name = "txtProductionNo";
		this.txtProductionNo.Size = new System.Drawing.Size(92, 23);
		this.txtProductionNo.TabIndex = 18;
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label25.Location = new System.Drawing.Point(748, 75);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(112, 15);
		this.label25.TabIndex = 528;
		this.label25.Text = "Packing Expiry Date";
		this.label25.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtPackingExpiryDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPackingExpiryDate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtPackingExpiryDate.Location = new System.Drawing.Point(750, 94);
		this.txtPackingExpiryDate.Name = "txtPackingExpiryDate";
		this.txtPackingExpiryDate.Size = new System.Drawing.Size(117, 23);
		this.txtPackingExpiryDate.TabIndex = 17;
		this.cmbWareHouseTo.AllowNull = (DefaultableBoolean)1;
		this.cmbWareHouseTo.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbWareHouseTo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbWareHouseTo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance75).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance75).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance75).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance75;
		((AppearanceBase)appearance76).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance76;
		((SpecialBoxBase)((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance77).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance77).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance77).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance78).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance78;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance81).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance82).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance82).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance82).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance82).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance82;
		((AppearanceBase)appearance83).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance84).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbWareHouseTo).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbWareHouseTo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbWareHouseTo).Location = new System.Drawing.Point(338, 92);
		((System.Windows.Forms.Control)(object)this.cmbWareHouseTo).Name = "cmbWareHouseTo";
		((System.Windows.Forms.Control)(object)this.cmbWareHouseTo).Size = new System.Drawing.Size(176, 26);
		((System.Windows.Forms.Control)(object)this.cmbWareHouseTo).TabIndex = 14;
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label20.Location = new System.Drawing.Point(337, 75);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(85, 15);
		this.label20.TabIndex = 526;
		this.label20.Text = "WareHouse To";
		this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.panel11.BackColor = System.Drawing.Color.Teal;
		this.panel11.Controls.Add(this.label52);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(0, 0);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1126, 22);
		this.panel11.TabIndex = 524;
		this.label52.AutoSize = true;
		this.label52.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label52.ForeColor = System.Drawing.Color.White;
		this.label52.Location = new System.Drawing.Point(3, 2);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(186, 17);
		this.label52.TabIndex = 101;
		this.label52.Text = "Export Goods Forwarding";
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label50.Location = new System.Drawing.Point(516, 75);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(68, 15);
		this.label50.TabIndex = 101;
		this.label50.Text = "Container #";
		this.label50.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.cmbcontainerNo.AllowNull = (DefaultableBoolean)1;
		this.cmbcontainerNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbcontainerNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbcontainerNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance96).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcontainerNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.cmbcontainerNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcontainerNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance107).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance107).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance107).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcontainerNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance107;
		((AppearanceBase)appearance118).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcontainerNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance118;
		((SpecialBoxBase)((UltraGridBase)this.cmbcontainerNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance126).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance126).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance126).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcontainerNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.cmbcontainerNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcontainerNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance127).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcontainerNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance127;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance129).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcontainerNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.cmbcontainerNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcontainerNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcontainerNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance130;
		((AppearanceBase)appearance131).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance131).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcontainerNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.cmbcontainerNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcontainerNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance132).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance132).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance132).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance132).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcontainerNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance132;
		((AppearanceBase)appearance133).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcontainerNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.cmbcontainerNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcontainerNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcontainerNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance134;
		((UltraGridBase)this.cmbcontainerNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcontainerNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.cmbcontainerNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcontainerNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcontainerNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcontainerNo).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbcontainerNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbcontainerNo).Location = new System.Drawing.Point(516, 92);
		((System.Windows.Forms.Control)(object)this.cmbcontainerNo).Name = "cmbcontainerNo";
		((System.Windows.Forms.Control)(object)this.cmbcontainerNo).Size = new System.Drawing.Size(119, 26);
		((System.Windows.Forms.Control)(object)this.cmbcontainerNo).TabIndex = 15;
		this.cmbItem.AllowNull = (DefaultableBoolean)1;
		this.cmbItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.cmbItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance114).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance114).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance114).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance114;
		((AppearanceBase)appearance115).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance115;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance116).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance116).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance116).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance117).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance117;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance119).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance121).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance121).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance122).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance122).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance122).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance122).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance122;
		((AppearanceBase)appearance123).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance124).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItem).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItem).Location = new System.Drawing.Point(6, 45);
		((System.Windows.Forms.Control)(object)this.cmbItem).Name = "cmbItem";
		this.cmbItem.ReadOnly = true;
		((System.Windows.Forms.Control)(object)this.cmbItem).Size = new System.Drawing.Size(244, 26);
		((System.Windows.Forms.Control)(object)this.cmbItem).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.cmbItem).Leave += new System.EventHandler(cmbItem_Leave);
		this.label45.AutoSize = true;
		this.label45.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label45.Location = new System.Drawing.Point(936, 28);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(79, 15);
		this.label45.TabIndex = 42;
		this.label45.Text = "Stock Weight";
		this.label45.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label47.Location = new System.Drawing.Point(790, 27);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(51, 15);
		this.label47.TabIndex = 42;
		this.label47.Text = "AddLess";
		this.label47.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label46.AutoSize = true;
		this.label46.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label46.Location = new System.Drawing.Point(694, 27);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(97, 15);
		this.label46.TabIndex = 42;
		this.label46.Text = "Packing Wt Total";
		this.label46.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label44.Location = new System.Drawing.Point(626, 27);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(68, 15);
		this.label44.TabIndex = 42;
		this.label44.Text = "Packing Wt";
		this.label44.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label39.Location = new System.Drawing.Point(849, 27);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(78, 15);
		this.label39.TabIndex = 42;
		this.label39.Text = "Gross Weight";
		this.label39.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label28.Location = new System.Drawing.Point(6, 27);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(67, 15);
		this.label28.TabIndex = 2;
		this.label28.Text = "Item Name";
		this.label28.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtStockWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtStockWeight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtStockWeight.Location = new System.Drawing.Point(936, 47);
		this.txtStockWeight.Name = "txtStockWeight";
		this.txtStockWeight.ReadOnly = true;
		this.txtStockWeight.Size = new System.Drawing.Size(90, 23);
		this.txtStockWeight.TabIndex = 11;
		this.txtStockWeight.TabStop = false;
		this.txtStockWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtPackingWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPackingWeight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtPackingWeight.Location = new System.Drawing.Point(629, 47);
		this.txtPackingWeight.Name = "txtPackingWeight";
		this.txtPackingWeight.Size = new System.Drawing.Size(64, 23);
		this.txtPackingWeight.TabIndex = 7;
		this.txtPackingWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtPackingWeight.TextChanged += new System.EventHandler(txtEbUnit_TextChanged);
		this.txtPackingWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtEbUnit_KeyPress);
		this.txtPackingweightTotal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPackingweightTotal.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtPackingweightTotal.Location = new System.Drawing.Point(694, 47);
		this.txtPackingweightTotal.Name = "txtPackingweightTotal";
		this.txtPackingweightTotal.ReadOnly = true;
		this.txtPackingweightTotal.Size = new System.Drawing.Size(95, 23);
		this.txtPackingweightTotal.TabIndex = 8;
		this.txtPackingweightTotal.TabStop = false;
		this.txtPackingweightTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtAddLess.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAddLess.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtAddLess.Location = new System.Drawing.Point(790, 47);
		this.txtAddLess.Name = "txtAddLess";
		this.txtAddLess.Size = new System.Drawing.Size(61, 23);
		this.txtAddLess.TabIndex = 9;
		this.txtAddLess.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtAddLess.TextChanged += new System.EventHandler(txtAddLess_TextChanged);
		this.txtAddLess.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtAddLess_KeyPress);
		this.txtGrossWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtGrossWeight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtGrossWeight.Location = new System.Drawing.Point(852, 47);
		this.txtGrossWeight.Name = "txtGrossWeight";
		this.txtGrossWeight.ReadOnly = true;
		this.txtGrossWeight.Size = new System.Drawing.Size(83, 23);
		this.txtGrossWeight.TabIndex = 10;
		this.txtGrossWeight.TabStop = false;
		this.txtGrossWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label29.Location = new System.Drawing.Point(252, 27);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(57, 15);
		this.label29.TabIndex = 4;
		this.label29.Text = "PackType";
		this.label29.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.CmbJobLot.AllowNull = (DefaultableBoolean)1;
		this.CmbJobLot.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobLot.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobLot.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Appearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance29;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance31;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance35;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance36).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance37).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance37).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance37).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance37;
		((AppearanceBase)appearance38).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbJobLot.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Location = new System.Drawing.Point(9, 92);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Name = "CmbJobLot";
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Size = new System.Drawing.Size(152, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).TabIndex = 12;
		this.cmbPackType.AllowNull = (DefaultableBoolean)1;
		this.cmbPackType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPackType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPackType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance100;
		((UltraGridBase)this.cmbPackType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPackType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance101).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance101).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance101).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPackType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance101;
		((AppearanceBase)appearance102).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPackType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance102;
		((SpecialBoxBase)((UltraGridBase)this.cmbPackType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance103).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance103).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance103).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPackType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.cmbPackType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPackType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance104).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance104;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance105).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance106;
		((AppearanceBase)appearance108).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance108).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance109).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance109).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance109).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance109).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance109;
		((AppearanceBase)appearance110).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance111).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.cmbPackType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPackType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPackType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPackType).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbPackType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPackType).Location = new System.Drawing.Point(251, 45);
		((System.Windows.Forms.Control)(object)this.cmbPackType).Name = "cmbPackType";
		((System.Windows.Forms.Control)(object)this.cmbPackType).Size = new System.Drawing.Size(120, 26);
		((System.Windows.Forms.Control)(object)this.cmbPackType).TabIndex = 3;
		this.label55.AutoSize = true;
		this.label55.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label55.Location = new System.Drawing.Point(9, 75);
		this.label55.Name = "label55";
		this.label55.Size = new System.Drawing.Size(43, 15);
		this.label55.TabIndex = 40;
		this.label55.Text = "JobLot";
		this.label55.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label31.Location = new System.Drawing.Point(451, 27);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(63, 15);
		this.label31.TabIndex = 8;
		this.label31.Text = "Pack UOM";
		this.label31.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.CmbWareHouseFrom.AllowNull = (DefaultableBoolean)1;
		this.CmbWareHouseFrom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWareHouseFrom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWareHouseFrom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance44).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance44).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance45;
		((SpecialBoxBase)((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance56).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance56).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance57;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance60).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance61).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWareHouseFrom).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbWareHouseFrom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWareHouseFrom).Location = new System.Drawing.Point(162, 92);
		((System.Windows.Forms.Control)(object)this.CmbWareHouseFrom).Name = "CmbWareHouseFrom";
		((System.Windows.Forms.Control)(object)this.CmbWareHouseFrom).Size = new System.Drawing.Size(175, 26);
		((System.Windows.Forms.Control)(object)this.CmbWareHouseFrom).TabIndex = 13;
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label54.Location = new System.Drawing.Point(161, 75);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(100, 15);
		this.label54.TabIndex = 38;
		this.label54.Text = "WareHouse From";
		this.label54.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label37.Location = new System.Drawing.Point(636, 75);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(39, 15);
		this.label37.TabIndex = 20;
		this.label37.Text = "Seal #";
		this.label37.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.btnDetailCancel.AutoSize = true;
		this.btnDetailCancel.BackColor = System.Drawing.Color.Teal;
		this.btnDetailCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnDetailCancel.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnDetailCancel.ForeColor = System.Drawing.Color.White;
		this.btnDetailCancel.Location = new System.Drawing.Point(1057, 92);
		this.btnDetailCancel.Name = "btnDetailCancel";
		this.btnDetailCancel.Size = new System.Drawing.Size(69, 28);
		this.btnDetailCancel.TabIndex = 21;
		this.btnDetailCancel.Text = "Cancel";
		this.btnDetailCancel.UseVisualStyleBackColor = false;
		this.btnDetailCancel.Click += new System.EventHandler(btnDetailCancel_Click);
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label32.Location = new System.Drawing.Point(370, 27);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(65, 15);
		this.label32.TabIndex = 18;
		this.label32.Text = "No of Bags";
		this.label32.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.btnDetailUpdate.AutoSize = true;
		this.btnDetailUpdate.BackColor = System.Drawing.Color.Teal;
		this.btnDetailUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnDetailUpdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnDetailUpdate.ForeColor = System.Drawing.Color.White;
		this.btnDetailUpdate.Location = new System.Drawing.Point(991, 92);
		this.btnDetailUpdate.Name = "btnDetailUpdate";
		this.btnDetailUpdate.Size = new System.Drawing.Size(72, 28);
		this.btnDetailUpdate.TabIndex = 20;
		this.btnDetailUpdate.Text = "Update";
		this.btnDetailUpdate.UseVisualStyleBackColor = false;
		this.btnDetailUpdate.Click += new System.EventHandler(btnDetailUpdate_Click);
		this.txtNoOfBags.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNoOfBags.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtNoOfBags.Location = new System.Drawing.Point(372, 47);
		this.txtNoOfBags.Name = "txtNoOfBags";
		this.txtNoOfBags.Size = new System.Drawing.Size(78, 23);
		this.txtNoOfBags.TabIndex = 4;
		this.txtNoOfBags.TextChanged += new System.EventHandler(txtNoOfBags_TextChanged);
		this.txtNoOfBags.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtNoOfBags_KeyPress);
		this.txtSeal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSeal.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtSeal.Location = new System.Drawing.Point(636, 94);
		this.txtSeal.Name = "txtSeal";
		this.txtSeal.Size = new System.Drawing.Size(113, 23);
		this.txtSeal.TabIndex = 16;
		this.btnAddinGrid.BackColor = System.Drawing.Color.Teal;
		this.btnAddinGrid.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAddinGrid.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAddinGrid.ForeColor = System.Drawing.Color.White;
		this.btnAddinGrid.Location = new System.Drawing.Point(960, 92);
		this.btnAddinGrid.Name = "btnAddinGrid";
		this.btnAddinGrid.Size = new System.Drawing.Size(32, 27);
		this.btnAddinGrid.TabIndex = 19;
		this.btnAddinGrid.Text = "+";
		this.btnAddinGrid.UseVisualStyleBackColor = false;
		this.btnAddinGrid.Visible = false;
		this.btnAddinGrid.Click += new System.EventHandler(btnAddinGrid_Click);
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label30.Location = new System.Drawing.Point(532, 27);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(65, 15);
		this.label30.TabIndex = 6;
		this.label30.Text = "NetWeight";
		this.label30.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.cmbPackUOM.AllowNull = (DefaultableBoolean)1;
		this.cmbPackUOM.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPackUOM.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPackUOM.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.Appearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance88).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance88).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance88).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPackUOM).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance88;
		((AppearanceBase)appearance89).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance89;
		((SpecialBoxBase)((UltraGridBase)this.cmbPackUOM).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance90).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance90).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance91).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance91;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance92).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance94).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance94).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance94;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance95).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance95).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance95).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance95).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance95;
		((AppearanceBase)appearance97).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance98).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance98;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPackUOM).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbPackUOM.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPackUOM).Location = new System.Drawing.Point(451, 45);
		((System.Windows.Forms.Control)(object)this.cmbPackUOM).Name = "cmbPackUOM";
		((System.Windows.Forms.Control)(object)this.cmbPackUOM).Size = new System.Drawing.Size(76, 26);
		((System.Windows.Forms.Control)(object)this.cmbPackUOM).TabIndex = 5;
		((System.Windows.Forms.Control)(object)this.cmbPackUOM).TextChanged += new System.EventHandler(cmbOuterUOM_ValueChanged);
		this.txtNetWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNetWeight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtNetWeight.Location = new System.Drawing.Point(528, 47);
		this.txtNetWeight.Name = "txtNetWeight";
		this.txtNetWeight.ReadOnly = true;
		this.txtNetWeight.Size = new System.Drawing.Size(100, 23);
		this.txtNetWeight.TabIndex = 6;
		this.txtNetWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtNetWeight.TextChanged += new System.EventHandler(cmbOuterUOM_ValueChanged);
		this.tabPage4.Controls.Add((System.Windows.Forms.Control)(object)this.grdotheritems);
		this.tabPage4.Controls.Add(this.panel13);
		this.tabPage4.Controls.Add(this.panel9);
		this.tabPage4.Location = new System.Drawing.Point(4, 22);
		this.tabPage4.Name = "tabPage4";
		this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage4.Size = new System.Drawing.Size(1132, 462);
		this.tabPage4.TabIndex = 1;
		this.tabPage4.Text = "Other Items";
		this.tabPage4.UseVisualStyleBackColor = true;
		this.grdotheritems.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdotheritems).BackColor = System.Drawing.Color.White;
		grdotheritems_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdotheritems.DesignTimeLayout = grdotheritems_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdotheritems).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdotheritems).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdotheritems.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdotheritems).Location = new System.Drawing.Point(3, 149);
		((System.Windows.Forms.Control)(object)this.grdotheritems).Name = "grdotheritems";
		((ScrollableBase)this.grdotheritems).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdotheritems).Size = new System.Drawing.Size(1126, 310);
		((System.Windows.Forms.Control)(object)this.grdotheritems).TabIndex = 0;
		this.grdotheritems.TotalRow = (InheritableBoolean)1;
		this.grdotheritems.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdotheritems).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdotheritems).DoubleClick += new System.EventHandler(grdotheritems_DoubleClick);
		this.panel13.BackColor = System.Drawing.Color.Teal;
		this.panel13.Controls.Add(this.label51);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel13.Location = new System.Drawing.Point(3, 127);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1126, 22);
		this.panel13.TabIndex = 524;
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label51.ForeColor = System.Drawing.Color.White;
		this.label51.Location = new System.Drawing.Point(3, 2);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(183, 17);
		this.label51.TabIndex = 101;
		this.label51.Text = "Other Items Detail            ";
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.cmbWareHouseToOtherItem);
		this.panel9.Controls.Add(this.label16);
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.cmbWareHouseFromOtherItem);
		this.panel9.Controls.Add(this.label13);
		this.panel9.Controls.Add(this.panel12);
		this.panel9.Controls.Add(this.txtOContainer);
		this.panel9.Controls.Add(this.label42);
		this.panel9.Controls.Add(this.btnoCancel);
		this.panel9.Controls.Add(this.btnoUpdate);
		this.panel9.Controls.Add(this.btnoadd);
		this.panel9.Controls.Add(this.label27);
		this.panel9.Controls.Add(this.txtoRemarks);
		this.panel9.Controls.Add(this.txtoQty);
		this.panel9.Controls.Add(this.label41);
		this.panel9.Controls.Add(this.txtoRate);
		this.panel9.Controls.Add(this.label63);
		this.panel9.Controls.Add(this.label70);
		this.panel9.Controls.Add(this.txtoAmount);
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.CmbOtherItem);
		this.panel9.Controls.Add(this.label71);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(3, 3);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1126, 124);
		this.panel9.TabIndex = 2;
		this.cmbWareHouseToOtherItem.AllowNull = (DefaultableBoolean)1;
		this.cmbWareHouseToOtherItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbWareHouseToOtherItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbWareHouseToOtherItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance136).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbWareHouseToOtherItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.cmbWareHouseToOtherItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbWareHouseToOtherItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance137).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance137).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance137).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbWareHouseToOtherItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance137;
		((AppearanceBase)appearance138).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbWareHouseToOtherItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance138;
		((SpecialBoxBase)((UltraGridBase)this.cmbWareHouseToOtherItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance140).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance140).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance140).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbWareHouseToOtherItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.cmbWareHouseToOtherItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbWareHouseToOtherItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance141).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbWareHouseToOtherItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance141;
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance142).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbWareHouseToOtherItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.cmbWareHouseToOtherItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbWareHouseToOtherItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbWareHouseToOtherItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance143;
		((AppearanceBase)appearance144).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance144).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbWareHouseToOtherItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.cmbWareHouseToOtherItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbWareHouseToOtherItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance145).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance145).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance145).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance145).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbWareHouseToOtherItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance145;
		((AppearanceBase)appearance146).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbWareHouseToOtherItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.cmbWareHouseToOtherItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbWareHouseToOtherItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance147).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbWareHouseToOtherItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance147;
		((UltraGridBase)this.cmbWareHouseToOtherItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbWareHouseToOtherItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance148;
		((UltraGridBase)this.cmbWareHouseToOtherItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbWareHouseToOtherItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbWareHouseToOtherItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbWareHouseToOtherItem).Font = new System.Drawing.Font("Verdana", 9.75f);
		((System.Windows.Forms.Control)(object)this.cmbWareHouseToOtherItem).Location = new System.Drawing.Point(843, 48);
		((System.Windows.Forms.Control)(object)this.cmbWareHouseToOtherItem).Name = "cmbWareHouseToOtherItem";
		((System.Windows.Forms.Control)(object)this.cmbWareHouseToOtherItem).Size = new System.Drawing.Size(217, 26);
		((System.Windows.Forms.Control)(object)this.cmbWareHouseToOtherItem).TabIndex = 6;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label16.Location = new System.Drawing.Point(843, 31);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(85, 15);
		this.label16.TabIndex = 527;
		this.label16.Text = "WareHouse To";
		this.cmbWareHouseFromOtherItem.AllowNull = (DefaultableBoolean)1;
		this.cmbWareHouseFromOtherItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbWareHouseFromOtherItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbWareHouseFromOtherItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbWareHouseFromOtherItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbWareHouseFromOtherItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbWareHouseFromOtherItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbWareHouseFromOtherItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance128).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbWareHouseFromOtherItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance128;
		((SpecialBoxBase)((UltraGridBase)this.cmbWareHouseFromOtherItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance139).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance139).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance139).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbWareHouseFromOtherItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance139;
		((UltraGridBase)this.cmbWareHouseFromOtherItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbWareHouseFromOtherItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance150).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbWareHouseFromOtherItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance150;
		((AppearanceBase)appearance161).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance161).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbWareHouseFromOtherItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance161;
		((UltraGridBase)this.cmbWareHouseFromOtherItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbWareHouseFromOtherItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbWareHouseFromOtherItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance172;
		((AppearanceBase)appearance183).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance183).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbWareHouseFromOtherItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance183;
		((UltraGridBase)this.cmbWareHouseFromOtherItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbWareHouseFromOtherItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance194).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance194).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance194).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance194).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance194).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbWareHouseFromOtherItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance194;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbWareHouseFromOtherItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbWareHouseFromOtherItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbWareHouseFromOtherItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbWareHouseFromOtherItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.cmbWareHouseFromOtherItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbWareHouseFromOtherItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.cmbWareHouseFromOtherItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbWareHouseFromOtherItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbWareHouseFromOtherItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbWareHouseFromOtherItem).Font = new System.Drawing.Font("Verdana", 9.75f);
		((System.Windows.Forms.Control)(object)this.cmbWareHouseFromOtherItem).Location = new System.Drawing.Point(624, 48);
		((System.Windows.Forms.Control)(object)this.cmbWareHouseFromOtherItem).Name = "cmbWareHouseFromOtherItem";
		((System.Windows.Forms.Control)(object)this.cmbWareHouseFromOtherItem).Size = new System.Drawing.Size(217, 26);
		((System.Windows.Forms.Control)(object)this.cmbWareHouseFromOtherItem).TabIndex = 5;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label13.Location = new System.Drawing.Point(624, 31);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(100, 15);
		this.label13.TabIndex = 525;
		this.label13.Text = "WareHouse From";
		this.panel12.BackColor = System.Drawing.Color.Teal;
		this.panel12.Controls.Add(this.label6);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(0, 0);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1126, 22);
		this.panel12.TabIndex = 523;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label6.ForeColor = System.Drawing.Color.White;
		this.label6.Location = new System.Drawing.Point(3, 2);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(93, 17);
		this.label6.TabIndex = 101;
		this.label6.Text = "Other Items";
		this.txtOContainer.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtOContainer.Location = new System.Drawing.Point(490, 50);
		this.txtOContainer.Name = "txtOContainer";
		this.txtOContainer.Size = new System.Drawing.Size(132, 23);
		this.txtOContainer.TabIndex = 4;
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label42.Location = new System.Drawing.Point(495, 31);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(77, 15);
		this.label42.TabIndex = 522;
		this.label42.Text = "Container No";
		this.btnoCancel.BackColor = System.Drawing.Color.Teal;
		this.btnoCancel.FlatAppearance.BorderSize = 0;
		this.btnoCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnoCancel.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnoCancel.ForeColor = System.Drawing.Color.White;
		this.btnoCancel.Location = new System.Drawing.Point(954, 93);
		this.btnoCancel.Name = "btnoCancel";
		this.btnoCancel.Size = new System.Drawing.Size(67, 26);
		this.btnoCancel.TabIndex = 9;
		this.btnoCancel.Text = "Cancel";
		this.btnoCancel.UseVisualStyleBackColor = false;
		this.btnoCancel.Click += new System.EventHandler(btnoCancel_Click);
		this.btnoUpdate.BackColor = System.Drawing.Color.Teal;
		this.btnoUpdate.FlatAppearance.BorderSize = 0;
		this.btnoUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnoUpdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnoUpdate.ForeColor = System.Drawing.Color.White;
		this.btnoUpdate.Location = new System.Drawing.Point(881, 93);
		this.btnoUpdate.Name = "btnoUpdate";
		this.btnoUpdate.Size = new System.Drawing.Size(70, 26);
		this.btnoUpdate.TabIndex = 8;
		this.btnoUpdate.Text = "Update";
		this.btnoUpdate.UseVisualStyleBackColor = false;
		this.btnoUpdate.Click += new System.EventHandler(btnoUpdate_Click);
		this.btnoadd.BackColor = System.Drawing.Color.Teal;
		this.btnoadd.FlatAppearance.BorderSize = 0;
		this.btnoadd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnoadd.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnoadd.ForeColor = System.Drawing.Color.White;
		this.btnoadd.Location = new System.Drawing.Point(849, 93);
		this.btnoadd.Name = "btnoadd";
		this.btnoadd.Size = new System.Drawing.Size(30, 26);
		this.btnoadd.TabIndex = 6;
		this.btnoadd.Text = "+";
		this.btnoadd.UseVisualStyleBackColor = false;
		this.btnoadd.Visible = false;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label27.Location = new System.Drawing.Point(7, 78);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(52, 15);
		this.label27.TabIndex = 521;
		this.label27.Text = "Remarks";
		this.txtoRemarks.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtoRemarks.Location = new System.Drawing.Point(7, 95);
		this.txtoRemarks.Name = "txtoRemarks";
		this.txtoRemarks.Size = new System.Drawing.Size(839, 23);
		this.txtoRemarks.TabIndex = 7;
		this.txtoQty.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtoQty.Location = new System.Drawing.Point(226, 50);
		this.txtoQty.Name = "txtoQty";
		this.txtoQty.Size = new System.Drawing.Size(77, 23);
		this.txtoQty.TabIndex = 1;
		this.txtoQty.Text = "0";
		this.txtoQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtoQty.TextChanged += new System.EventHandler(txtoQty_TextChanged);
		this.txtoQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtoQty_KeyPress);
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label41.Location = new System.Drawing.Point(230, 31);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(26, 15);
		this.label41.TabIndex = 518;
		this.label41.Text = "Qty";
		this.txtoRate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtoRate.Location = new System.Drawing.Point(305, 50);
		this.txtoRate.Name = "txtoRate";
		this.txtoRate.ReadOnly = true;
		this.txtoRate.Size = new System.Drawing.Size(80, 23);
		this.txtoRate.TabIndex = 2;
		this.txtoRate.Text = "0";
		this.txtoRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtoRate.TextChanged += new System.EventHandler(txtoRate_TextChanged);
		this.label63.AutoSize = true;
		this.label63.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label63.Location = new System.Drawing.Point(302, 31);
		this.label63.Name = "label63";
		this.label63.Size = new System.Drawing.Size(30, 15);
		this.label63.TabIndex = 520;
		this.label63.Text = "Rate";
		this.label70.AutoSize = true;
		this.label70.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label70.Location = new System.Drawing.Point(384, 31);
		this.label70.Name = "label70";
		this.label70.Size = new System.Drawing.Size(51, 15);
		this.label70.TabIndex = 519;
		this.label70.Text = "Amount";
		this.txtoAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtoAmount.Location = new System.Drawing.Point(387, 50);
		this.txtoAmount.Name = "txtoAmount";
		this.txtoAmount.ReadOnly = true;
		this.txtoAmount.Size = new System.Drawing.Size(101, 23);
		this.txtoAmount.TabIndex = 3;
		this.txtoAmount.Text = "0";
		this.txtoAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.CmbOtherItem.AllowNull = (DefaultableBoolean)1;
		this.CmbOtherItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbOtherItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbOtherItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance176).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance176).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance176;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance180).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance180).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance180).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance180).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbOtherItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance180;
		((AppearanceBase)appearance181).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance181;
		((SpecialBoxBase)((UltraGridBase)this.CmbOtherItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance182).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance182).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance182).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance182).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance182;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance184).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance184).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance184;
		((AppearanceBase)appearance185).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance185).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance185;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance189).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance189;
		((AppearanceBase)appearance190).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance190).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance190;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance191).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance191).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance191).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance191).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance191).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance191;
		((AppearanceBase)appearance202).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance202;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance203).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance203).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance203;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance204).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance204;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbOtherItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbOtherItem).Font = new System.Drawing.Font("Verdana", 9.75f);
		((System.Windows.Forms.Control)(object)this.CmbOtherItem).Location = new System.Drawing.Point(7, 48);
		((System.Windows.Forms.Control)(object)this.CmbOtherItem).Name = "CmbOtherItem";
		((System.Windows.Forms.Control)(object)this.CmbOtherItem).Size = new System.Drawing.Size(217, 26);
		((System.Windows.Forms.Control)(object)this.CmbOtherItem).TabIndex = 0;
		this.label71.AutoSize = true;
		this.label71.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label71.Location = new System.Drawing.Point(7, 31);
		this.label71.Name = "label71";
		this.label71.Size = new System.Drawing.Size(67, 15);
		this.label71.TabIndex = 517;
		this.label71.Text = "Item Name";
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add(this.groupBox3);
		this.panel3.Controls.Add(this.groupBox2);
		this.panel3.Controls.Add(this.ChkPrintPreview);
		this.panel3.Controls.Add(this.txtOtherCharges);
		this.panel3.Controls.Add(this.groupBox1);
		this.panel3.Controls.Add(this.label15);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.panel3.Location = new System.Drawing.Point(3, 56);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1140, 188);
		this.panel3.TabIndex = 0;
		this.groupBox3.Controls.Add(this.txtDoGrossWeight);
		this.groupBox3.Controls.Add(this.label1);
		this.groupBox3.Controls.Add(this.label2);
		this.groupBox3.Controls.Add(this.txtInvoiceNetWeight);
		this.groupBox3.Controls.Add(this.txtDoNetWeight);
		this.groupBox3.Controls.Add(this.InvoiceNetWeight);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(1010, 4);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(125, 146);
		this.groupBox3.TabIndex = 2;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "DO Data";
		this.txtDoGrossWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDoGrossWeight.Enabled = false;
		this.txtDoGrossWeight.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDoGrossWeight.Location = new System.Drawing.Point(5, 39);
		this.txtDoGrossWeight.Name = "txtDoGrossWeight";
		this.txtDoGrossWeight.Size = new System.Drawing.Size(117, 22);
		this.txtDoGrossWeight.TabIndex = 0;
		this.txtDoGrossWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label1.AutoSize = true;
		this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label1.Location = new System.Drawing.Point(5, 21);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(55, 15);
		this.label1.TabIndex = 91;
		this.label1.Text = "Gross Wt";
		this.label2.AutoSize = true;
		this.label2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label2.Location = new System.Drawing.Point(5, 64);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(75, 15);
		this.label2.TabIndex = 93;
		this.label2.Text = "Total Weight";
		this.txtInvoiceNetWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtInvoiceNetWeight.Enabled = false;
		this.txtInvoiceNetWeight.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtInvoiceNetWeight.Location = new System.Drawing.Point(5, 120);
		this.txtInvoiceNetWeight.Name = "txtInvoiceNetWeight";
		this.txtInvoiceNetWeight.Size = new System.Drawing.Size(117, 22);
		this.txtInvoiceNetWeight.TabIndex = 2;
		this.txtInvoiceNetWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtDoNetWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDoNetWeight.Enabled = false;
		this.txtDoNetWeight.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDoNetWeight.Location = new System.Drawing.Point(5, 80);
		this.txtDoNetWeight.Name = "txtDoNetWeight";
		this.txtDoNetWeight.Size = new System.Drawing.Size(117, 22);
		this.txtDoNetWeight.TabIndex = 1;
		this.txtDoNetWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.InvoiceNetWeight.AutoSize = true;
		this.InvoiceNetWeight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.InvoiceNetWeight.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.InvoiceNetWeight.Location = new System.Drawing.Point(5, 104);
		this.InvoiceNetWeight.Name = "InvoiceNetWeight";
		this.InvoiceNetWeight.Size = new System.Drawing.Size(68, 15);
		this.InvoiceNetWeight.TabIndex = 93;
		this.InvoiceNetWeight.Text = "Net Weight";
		this.groupBox2.Controls.Add(this.txtFactoryWeight);
		this.groupBox2.Controls.Add(this.label35);
		this.groupBox2.Controls.Add(this.label43);
		this.groupBox2.Controls.Add(this.txtBalFcWeight);
		this.groupBox2.Controls.Add(this.label49);
		this.groupBox2.Controls.Add(this.txtDispatchWeight);
		this.groupBox2.Controls.Add(this.txtNoofInvoices);
		this.groupBox2.Controls.Add(this.txtFreight);
		this.groupBox2.Controls.Add(this.label8);
		this.groupBox2.Controls.Add(this.label14);
		this.groupBox2.Controls.Add(this.txtVehicleNo);
		this.groupBox2.Controls.Add(this.txtBilityNo);
		this.groupBox2.Controls.Add(this.label48);
		this.groupBox2.Controls.Add(this.label9);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbLoadingPort);
		this.groupBox2.Controls.Add(this.label22);
		this.groupBox2.Controls.Add(this.txtContainer2);
		this.groupBox2.Controls.Add(this.label23);
		this.groupBox2.Controls.Add(this.txtNoOfContainer);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDestinationPort);
		this.groupBox2.Controls.Add(this.label34);
		this.groupBox2.Controls.Add(this.txtconainer1);
		this.groupBox2.Controls.Add(this.label24);
		this.groupBox2.Controls.Add(this.label33);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(606, 4);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(403, 177);
		this.groupBox2.TabIndex = 1;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Other Info";
		this.txtFactoryWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFactoryWeight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtFactoryWeight.Location = new System.Drawing.Point(74, 99);
		this.txtFactoryWeight.Name = "txtFactoryWeight";
		this.txtFactoryWeight.Size = new System.Drawing.Size(97, 23);
		this.txtFactoryWeight.TabIndex = 3;
		this.txtFactoryWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFactoryWeight.TextChanged += new System.EventHandler(txtFactoryWeight_TextChanged);
		this.txtFactoryWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFactoryWeight_KeyPress);
		this.label35.AutoSize = true;
		this.label35.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label35.Location = new System.Drawing.Point(5, 103);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(65, 15);
		this.label35.TabIndex = 89;
		this.label35.Text = "Fct Weight";
		this.label43.AutoSize = true;
		this.label43.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.label43.Font = new System.Drawing.Font("Segoe UI Semibold", 8f, System.Drawing.FontStyle.Bold);
		this.label43.Location = new System.Drawing.Point(5, 130);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(69, 13);
		this.label43.TabIndex = 89;
		this.label43.Text = "Dispatch Wt";
		this.txtBalFcWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBalFcWeight.Enabled = false;
		this.txtBalFcWeight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtBalFcWeight.Location = new System.Drawing.Point(74, 151);
		this.txtBalFcWeight.Name = "txtBalFcWeight";
		this.txtBalFcWeight.Size = new System.Drawing.Size(97, 23);
		this.txtBalFcWeight.TabIndex = 5;
		this.txtBalFcWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label49.AutoSize = true;
		this.label49.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.label49.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label49.Location = new System.Drawing.Point(5, 155);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(42, 15);
		this.label49.TabIndex = 89;
		this.label49.Text = "Bal Wt";
		this.txtDispatchWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDispatchWeight.Enabled = false;
		this.txtDispatchWeight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtDispatchWeight.Location = new System.Drawing.Point(74, 125);
		this.txtDispatchWeight.Name = "txtDispatchWeight";
		this.txtDispatchWeight.Size = new System.Drawing.Size(97, 23);
		this.txtDispatchWeight.TabIndex = 4;
		this.txtDispatchWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtNoofInvoices.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNoofInvoices.Enabled = false;
		this.txtNoofInvoices.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtNoofInvoices.Location = new System.Drawing.Point(269, 151);
		this.txtNoofInvoices.Name = "txtNoofInvoices";
		this.txtNoofInvoices.Size = new System.Drawing.Size(130, 23);
		this.txtNoofInvoices.TabIndex = 11;
		this.txtFreight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFreight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtFreight.Location = new System.Drawing.Point(74, 17);
		this.txtFreight.Name = "txtFreight";
		this.txtFreight.Size = new System.Drawing.Size(97, 23);
		this.txtFreight.TabIndex = 0;
		this.txtFreight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFreight_KeyPress);
		this.label8.AutoSize = true;
		this.label8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label8.Location = new System.Drawing.Point(5, 49);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(64, 15);
		this.label8.TabIndex = 65;
		this.label8.Text = "Vehicle No";
		this.label14.AutoSize = true;
		this.label14.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label14.Location = new System.Drawing.Point(5, 21);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(44, 15);
		this.label14.TabIndex = 40;
		this.label14.Text = "Freight";
		this.txtVehicleNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtVehicleNo.Enabled = false;
		this.txtVehicleNo.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtVehicleNo.Location = new System.Drawing.Point(74, 45);
		this.txtVehicleNo.Name = "txtVehicleNo";
		this.txtVehicleNo.Size = new System.Drawing.Size(97, 23);
		this.txtVehicleNo.TabIndex = 1;
		this.txtBilityNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBilityNo.Enabled = false;
		this.txtBilityNo.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtBilityNo.Location = new System.Drawing.Point(74, 73);
		this.txtBilityNo.Name = "txtBilityNo";
		this.txtBilityNo.Size = new System.Drawing.Size(97, 23);
		this.txtBilityNo.TabIndex = 2;
		this.label48.AutoSize = true;
		this.label48.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.label48.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label48.Location = new System.Drawing.Point(174, 155);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(86, 15);
		this.label48.TabIndex = 93;
		this.label48.Text = "No Of Invoices";
		this.label9.AutoSize = true;
		this.label9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label9.Location = new System.Drawing.Point(5, 77);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(52, 15);
		this.label9.TabIndex = 69;
		this.label9.Text = "Bility No";
		this.cmbLoadingPort.AllowNull = (DefaultableBoolean)1;
		this.cmbLoadingPort.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbLoadingPort.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbLoadingPort.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Appearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbLoadingPort).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance30;
		((SpecialBoxBase)((UltraGridBase)this.cmbLoadingPort).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance41).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance41).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance173).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance173;
		((AppearanceBase)appearance174).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance174).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance174;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance175).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance175).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance175).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance175).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance175;
		((AppearanceBase)appearance177).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance177;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance178).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance178;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance179;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbLoadingPort).Enabled = false;
		((System.Windows.Forms.Control)(object)this.cmbLoadingPort).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbLoadingPort.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbLoadingPort).Location = new System.Drawing.Point(269, 15);
		((System.Windows.Forms.Control)(object)this.cmbLoadingPort).Name = "cmbLoadingPort";
		((System.Windows.Forms.Control)(object)this.cmbLoadingPort).Size = new System.Drawing.Size(130, 26);
		((System.Windows.Forms.Control)(object)this.cmbLoadingPort).TabIndex = 6;
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label22.Location = new System.Drawing.Point(174, 21);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(75, 15);
		this.label22.TabIndex = 41;
		this.label22.Text = "Loading Port";
		this.txtContainer2.BackColor = System.Drawing.Color.White;
		this.txtContainer2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtContainer2.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtContainer2.Location = new System.Drawing.Point(269, 125);
		this.txtContainer2.Name = "txtContainer2";
		this.txtContainer2.ReadOnly = true;
		this.txtContainer2.Size = new System.Drawing.Size(130, 23);
		this.txtContainer2.TabIndex = 10;
		this.txtContainer2.TabStop = false;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label23.Location = new System.Drawing.Point(174, 49);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(93, 15);
		this.label23.TabIndex = 47;
		this.label23.Text = "Destination Port";
		this.txtNoOfContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNoOfContainer.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtNoOfContainer.Location = new System.Drawing.Point(269, 73);
		this.txtNoOfContainer.Name = "txtNoOfContainer";
		this.txtNoOfContainer.Size = new System.Drawing.Size(130, 23);
		this.txtNoOfContainer.TabIndex = 8;
		this.cmbDestinationPort.AllowNull = (DefaultableBoolean)1;
		this.cmbDestinationPort.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDestinationPort.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDestinationPort.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Appearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance18).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance18).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDestinationPort).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance18;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance19;
		((SpecialBoxBase)((UltraGridBase)this.cmbDestinationPort).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance20).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance20).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance25).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance26).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance26).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance26).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance68).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance69).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDestinationPort).Enabled = false;
		((System.Windows.Forms.Control)(object)this.cmbDestinationPort).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbDestinationPort.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDestinationPort).Location = new System.Drawing.Point(269, 43);
		((System.Windows.Forms.Control)(object)this.cmbDestinationPort).Name = "cmbDestinationPort";
		((System.Windows.Forms.Control)(object)this.cmbDestinationPort).Size = new System.Drawing.Size(130, 26);
		((System.Windows.Forms.Control)(object)this.cmbDestinationPort).TabIndex = 7;
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label34.Location = new System.Drawing.Point(174, 129);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(86, 15);
		this.label34.TabIndex = 87;
		this.label34.Text = "ContainerNo-2";
		this.txtconainer1.BackColor = System.Drawing.Color.White;
		this.txtconainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtconainer1.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtconainer1.Location = new System.Drawing.Point(269, 99);
		this.txtconainer1.Name = "txtconainer1";
		this.txtconainer1.ReadOnly = true;
		this.txtconainer1.Size = new System.Drawing.Size(130, 23);
		this.txtconainer1.TabIndex = 9;
		this.txtconainer1.TabStop = false;
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label24.Location = new System.Drawing.Point(174, 77);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(96, 15);
		this.label24.TabIndex = 55;
		this.label24.Text = "No. Of Container";
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label33.Location = new System.Drawing.Point(174, 103);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(84, 15);
		this.label33.TabIndex = 85;
		this.label33.Text = "ContainerNo-1";
		this.ChkPrintPreview.AutoSize = true;
		this.ChkPrintPreview.Checked = true;
		this.ChkPrintPreview.CheckState = System.Windows.Forms.CheckState.Checked;
		this.ChkPrintPreview.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.ChkPrintPreview.Location = new System.Drawing.Point(1020, 156);
		this.ChkPrintPreview.Name = "ChkPrintPreview";
		this.ChkPrintPreview.Size = new System.Drawing.Size(67, 19);
		this.ChkPrintPreview.TabIndex = 3;
		this.ChkPrintPreview.Text = "Preview";
		this.ChkPrintPreview.UseVisualStyleBackColor = true;
		this.txtOtherCharges.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOtherCharges.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOtherCharges.Location = new System.Drawing.Point(1155, 26);
		this.txtOtherCharges.Name = "txtOtherCharges";
		this.txtOtherCharges.Size = new System.Drawing.Size(80, 22);
		this.txtOtherCharges.TabIndex = 20;
		this.txtOtherCharges.TabStop = false;
		this.txtOtherCharges.Visible = false;
		this.groupBox1.Controls.Add(this.txtRemarksHeader);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add(this.txtDriverCellNo);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbtransporter);
		this.groupBox1.Controls.Add(this.txtdocno);
		this.groupBox1.Controls.Add(this.txtDriverName);
		this.groupBox1.Controls.Add(this.label17);
		this.groupBox1.Controls.Add(this.label10);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbInvoiceNo);
		this.groupBox1.Controls.Add(this.DocDate);
		this.groupBox1.Controls.Add(this.label11);
		this.groupBox1.Controls.Add(this.label21);
		this.groupBox1.Controls.Add(this.label18);
		this.groupBox1.Controls.Add(this.label12);
		this.groupBox1.Controls.Add(this.txtCNICNO);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCustomer);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbGpNo);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.txtGPDate);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(6, 4);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(599, 177);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Main";
		this.txtRemarksHeader.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksHeader.Location = new System.Drawing.Point(114, 129);
		this.txtRemarksHeader.Name = "txtRemarksHeader";
		this.txtRemarksHeader.Size = new System.Drawing.Size(483, 44);
		this.txtRemarksHeader.TabIndex = 6;
		this.txtRemarksHeader.Text = "";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label5.Location = new System.Drawing.Point(4, 136);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(52, 15);
		this.label5.TabIndex = 99;
		this.label5.Text = "Remarks";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label7.Location = new System.Drawing.Point(345, 21);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(67, 15);
		this.label7.TabIndex = 62;
		this.label7.Text = "Transporter";
		this.txtDriverCellNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDriverCellNo.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtDriverCellNo.Location = new System.Drawing.Point(422, 73);
		this.txtDriverCellNo.Name = "txtDriverCellNo";
		this.txtDriverCellNo.Size = new System.Drawing.Size(175, 23);
		this.txtDriverCellNo.TabIndex = 9;
		this.cmbtransporter.AllowNull = (DefaultableBoolean)1;
		this.cmbtransporter.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbtransporter.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbtransporter.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance164).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.Appearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance165).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance165).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance165).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbtransporter).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance165;
		((AppearanceBase)appearance65).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance65;
		((SpecialBoxBase)((UltraGridBase)this.cmbtransporter).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance46).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance47;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance50).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance51).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance51).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance51).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance51).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance51;
		((AppearanceBase)appearance52).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbtransporter).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbtransporter).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbtransporter.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbtransporter).Location = new System.Drawing.Point(422, 15);
		((System.Windows.Forms.Control)(object)this.cmbtransporter).Name = "cmbtransporter";
		((System.Windows.Forms.Control)(object)this.cmbtransporter).Size = new System.Drawing.Size(175, 26);
		((System.Windows.Forms.Control)(object)this.cmbtransporter).TabIndex = 7;
		this.txtdocno.BackColor = System.Drawing.Color.White;
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocno.Location = new System.Drawing.Point(114, 17);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.Size = new System.Drawing.Size(80, 23);
		this.txtdocno.TabIndex = 0;
		this.txtdocno.TabStop = false;
		this.txtDriverName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDriverName.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtDriverName.Location = new System.Drawing.Point(422, 45);
		this.txtDriverName.Name = "txtDriverName";
		this.txtDriverName.Size = new System.Drawing.Size(175, 23);
		this.txtDriverName.TabIndex = 8;
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(4, 21);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(88, 15);
		this.label17.TabIndex = 96;
		this.label17.Text = "Doc No && Date";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label10.Location = new System.Drawing.Point(345, 49);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(74, 15);
		this.label10.TabIndex = 25;
		this.label10.Text = "Driver Name";
		this.cmbInvoiceNo.AllowNull = (DefaultableBoolean)1;
		this.cmbInvoiceNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbInvoiceNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbInvoiceNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance5).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance5).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance5;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance6;
		((SpecialBoxBase)((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance7).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance7).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance10;
		((AppearanceBase)appearance11).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance11).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance13).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbInvoiceNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbInvoiceNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbInvoiceNo).Location = new System.Drawing.Point(114, 71);
		((System.Windows.Forms.Control)(object)this.cmbInvoiceNo).Name = "cmbInvoiceNo";
		((System.Windows.Forms.Control)(object)this.cmbInvoiceNo).Size = new System.Drawing.Size(229, 26);
		((System.Windows.Forms.Control)(object)this.cmbInvoiceNo).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.cmbInvoiceNo).Leave += new System.EventHandler(cmbInvoiceNo_Leave);
		this.DocDate.CustomFormat = "dd-MMM-yy";
		this.DocDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DocDate.Location = new System.Drawing.Point(196, 17);
		this.DocDate.Name = "DocDate";
		this.DocDate.Size = new System.Drawing.Size(147, 23);
		this.DocDate.TabIndex = 1;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label11.Location = new System.Drawing.Point(345, 77);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(80, 15);
		this.label11.TabIndex = 32;
		this.label11.Text = "Driver Cell No";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label21.Location = new System.Drawing.Point(4, 106);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(110, 15);
		this.label21.TabIndex = 36;
		this.label21.Text = "Customer/Importer";
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(4, 77);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(76, 15);
		this.label18.TabIndex = 98;
		this.label18.Text = "Pre Invoice #";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label12.Location = new System.Drawing.Point(345, 106);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(53, 15);
		this.label12.TabIndex = 37;
		this.label12.Text = "CNIC No";
		this.txtCNICNO.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCNICNO.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtCNICNO.Location = new System.Drawing.Point(422, 102);
		this.txtCNICNO.Name = "txtCNICNO";
		this.txtCNICNO.Size = new System.Drawing.Size(175, 23);
		this.txtCNICNO.TabIndex = 10;
		this.cmbCustomer.AllowNull = (DefaultableBoolean)1;
		this.cmbCustomer.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCustomer.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCustomer.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance149).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.Appearance = (AppearanceBase)(object)appearance149;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance151).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance151).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance151).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCustomer).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance151;
		((AppearanceBase)appearance152).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance152;
		((SpecialBoxBase)((UltraGridBase)this.cmbCustomer).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance153).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance153).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance153).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance153;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance154).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance154;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance155).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance67;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance71).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance72).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance72).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance72).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance159).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCustomer).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCustomer).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbCustomer.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCustomer).Location = new System.Drawing.Point(114, 100);
		((System.Windows.Forms.Control)(object)this.cmbCustomer).Name = "cmbCustomer";
		((System.Windows.Forms.Control)(object)this.cmbCustomer).Size = new System.Drawing.Size(229, 26);
		((System.Windows.Forms.Control)(object)this.cmbCustomer).TabIndex = 5;
		this.cmbGpNo.AllowNull = (DefaultableBoolean)1;
		this.cmbGpNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbGpNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbGpNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance186).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance186).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbGpNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance186;
		((UltraGridBase)this.cmbGpNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbGpNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance187).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance187).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance187).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance187).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbGpNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance187;
		((AppearanceBase)appearance188).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbGpNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance188;
		((SpecialBoxBase)((UltraGridBase)this.cmbGpNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance192).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance192).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance192).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance192).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbGpNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance192;
		((UltraGridBase)this.cmbGpNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbGpNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance193).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance193).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbGpNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance193;
		((AppearanceBase)appearance195).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance195).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbGpNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance195;
		((UltraGridBase)this.cmbGpNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbGpNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance196).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbGpNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance196;
		((AppearanceBase)appearance197).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance197).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbGpNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance197;
		((UltraGridBase)this.cmbGpNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbGpNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance198).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance198).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance198).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance198).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance198).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbGpNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance198;
		((AppearanceBase)appearance199).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbGpNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance199;
		((UltraGridBase)this.cmbGpNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbGpNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance200).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance200).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbGpNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance200;
		((UltraGridBase)this.cmbGpNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance201).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbGpNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance201;
		((UltraGridBase)this.cmbGpNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbGpNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbGpNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbGpNo).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbGpNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbGpNo).Location = new System.Drawing.Point(114, 43);
		((System.Windows.Forms.Control)(object)this.cmbGpNo).Name = "cmbGpNo";
		((System.Windows.Forms.Control)(object)this.cmbGpNo).Size = new System.Drawing.Size(80, 26);
		((System.Windows.Forms.Control)(object)this.cmbGpNo).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.cmbGpNo).TextChanged += new System.EventHandler(cmbGpNo_TextChanged);
		((System.Windows.Forms.Control)(object)this.cmbGpNo).Leave += new System.EventHandler(cmbGpNo_Leave);
		this.label4.AutoSize = true;
		this.label4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label4.Location = new System.Drawing.Point(4, 49);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(81, 15);
		this.label4.TabIndex = 56;
		this.label4.Text = "GP No && Date";
		this.txtGPDate.CustomFormat = "dd-MMM-yy";
		this.txtGPDate.Enabled = false;
		this.txtGPDate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtGPDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtGPDate.Location = new System.Drawing.Point(196, 45);
		this.txtGPDate.Name = "txtGPDate";
		this.txtGPDate.Size = new System.Drawing.Size(147, 23);
		this.txtGPDate.TabIndex = 3;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(1152, 7);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(83, 14);
		this.label15.TabIndex = 46;
		this.label15.Text = "Other Charges";
		this.label15.Visible = false;
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.label36);
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1140, 53);
		this.panel2.TabIndex = 0;
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.ForeColor = System.Drawing.Color.White;
		this.label36.Location = new System.Drawing.Point(4, 30);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(280, 19);
		this.label36.TabIndex = 101;
		this.label36.Text = "Export Goods Forwarding             ";
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[7] { this.BtnNew, this.btnRefresh, this.btnSave, this.btnUpdate, this.btnAttachment, this.BtnPrint, this.btnDelete });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1140, 27);
		this.toolStrip1.TabIndex = 1;
		this.toolStrip1.Text = "toolStrip1";
		this.BtnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.BtnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnNew.Name = "BtnNew";
		this.BtnNew.Size = new System.Drawing.Size(59, 24);
		this.BtnNew.Text = "&New";
		this.BtnNew.Click += new System.EventHandler(BtnNew_Click);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnSave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(64, 24);
		this.btnSave.Text = "&Save";
		this.btnSave.Click += new System.EventHandler(btnSave_Click);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(109, 24);
		this.btnAttachment.Text = "&Attachment";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.BtnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.BtnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnPrint.Name = "BtnPrint";
		this.BtnPrint.Size = new System.Drawing.Size(92, 24);
		this.BtnPrint.Text = "507-Print";
		this.BtnPrint.Click += new System.EventHandler(BtnPrint_Click);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.icons8_cut_16;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(73, 24);
		this.btnDelete.Text = "Delete";
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.tabPage2.Controls.Add(this.panel18);
		this.tabPage2.Controls.Add(this.panel15);
		this.tabPage2.Controls.Add(this.panel16);
		this.tabPage2.Controls.Add(this.panel17);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistoryDetail);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1146, 735);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.grdHistoryDetail.AllowEdit = (InheritableBoolean)2;
		this.grdHistoryDetail.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdHistoryDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistoryDetail.DesignTimeLayout = grdHistoryDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Dock = System.Windows.Forms.DockStyle.Bottom;
		this.grdHistoryDetail.DynamicFiltering = true;
		this.grdHistoryDetail.FilterMode = (FilterMode)2;
		this.grdHistoryDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistoryDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Location = new System.Drawing.Point(3, 466);
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Name = "grdHistoryDetail";
		((ScrollableBase)this.grdHistoryDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Size = new System.Drawing.Size(1140, 266);
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).TabIndex = 47;
		this.grdHistoryDetail.TotalRow = (InheritableBoolean)1;
		this.grdHistoryDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistoryDetail).VisualStyle = (VisualStyle)4;
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
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(3, 153);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1140, 294);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 45;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		this.DataGridHistory.SelectionChanged += new System.EventHandler(DataGridHistory_SelectionChanged);
		this.DataGridHistory.LinkClicked += new ColumnActionEventHandler(DataGridHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).DoubleClick += new System.EventHandler(DataGridHistory_DoubleClick);
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1091, -1);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(45, 23);
		this.ctrlGrdBar2.TabIndex = 54;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.panel15.BackColor = System.Drawing.Color.Teal;
		this.panel15.Controls.Add(this.ctrlGrdBar2);
		this.panel15.Controls.Add(this.label92);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.Location = new System.Drawing.Point(3, 130);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(1140, 23);
		this.panel15.TabIndex = 48;
		this.label92.AutoSize = true;
		this.label92.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label92.ForeColor = System.Drawing.Color.White;
		this.label92.Location = new System.Drawing.Point(3, 2);
		this.label92.Name = "label92";
		this.label92.Size = new System.Drawing.Size(142, 19);
		this.label92.TabIndex = 55;
		this.label92.Text = "Filtered Records";
		this.panel16.Controls.Add(this.groupBox4);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel16.Location = new System.Drawing.Point(3, 55);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(1140, 75);
		this.panel16.TabIndex = 49;
		this.groupBox4.Controls.Add(this.rdapproveddate);
		this.groupBox4.Controls.Add(this.btnShowHistory);
		this.groupBox4.Controls.Add(this.rdmodifydate);
		this.groupBox4.Controls.Add(this.label40);
		this.groupBox4.Controls.Add(this.drdocdate);
		this.groupBox4.Controls.Add(this.rdentrydate);
		this.groupBox4.Controls.Add(this.label73);
		this.groupBox4.Controls.Add(this.label89);
		this.groupBox4.Controls.Add(this.label90);
		this.groupBox4.Controls.Add(this.label91);
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomerHistory);
		this.groupBox4.Controls.Add(this.ToDocNoHistory);
		this.groupBox4.Controls.Add(this.FromDocNoHistory);
		this.groupBox4.Controls.Add(this.ToDateHistory);
		this.groupBox4.Controls.Add(this.FromDateHistory);
		this.groupBox4.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox4.Location = new System.Drawing.Point(4, 2);
		this.groupBox4.Name = "groupBox4";
		this.groupBox4.Size = new System.Drawing.Size(936, 71);
		this.groupBox4.TabIndex = 0;
		this.groupBox4.TabStop = false;
		this.groupBox4.Text = "Filters";
		this.rdapproveddate.AutoSize = true;
		this.rdapproveddate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdapproveddate.Location = new System.Drawing.Point(816, 44);
		this.rdapproveddate.Name = "rdapproveddate";
		this.rdapproveddate.Size = new System.Drawing.Size(105, 19);
		this.rdapproveddate.TabIndex = 9;
		this.rdapproveddate.Text = "Approved Date";
		this.rdapproveddate.UseVisualStyleBackColor = true;
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
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(816, 18);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 8;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.Location = new System.Drawing.Point(373, 20);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(58, 15);
		this.label40.TabIndex = 87;
		this.label40.Text = "Customer";
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
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance156).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance156;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance157).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance157).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance157).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance157;
		((AppearanceBase)appearance158).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance158;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance160).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance160).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance160).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance160;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance162).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance162;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance163).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance163;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance166).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance166;
		((AppearanceBase)appearance167).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance167).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance167;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance168).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance168).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance168).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance168).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance168;
		((AppearanceBase)appearance169).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance169;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance170).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance170;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance171).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance171;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomerHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Location = new System.Drawing.Point(434, 14);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Name = "CmbCustomerHistory";
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Size = new System.Drawing.Size(296, 26);
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
		this.panel17.BackColor = System.Drawing.Color.Teal;
		this.panel17.Controls.Add(this.label69);
		this.panel17.Controls.Add(this.toolStrip3);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel17.Location = new System.Drawing.Point(3, 3);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(1140, 52);
		this.panel17.TabIndex = 50;
		this.label69.AutoSize = true;
		this.label69.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label69.ForeColor = System.Drawing.Color.White;
		this.label69.Location = new System.Drawing.Point(3, 30);
		this.label69.Name = "label69";
		this.label69.Size = new System.Drawing.Size(223, 19);
		this.label69.TabIndex = 54;
		this.label69.Text = "Export Forwarding History";
		this.toolStrip3.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.toolStripSeparator2, this.btnResetHistory, this.btnRefreshHistory });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1140, 27);
		this.toolStrip3.TabIndex = 53;
		this.toolStrip3.Text = "toolStrip3";
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
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
		this.panel18.BackColor = System.Drawing.Color.Teal;
		this.panel18.Controls.Add(this.label93);
		this.panel18.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel18.Location = new System.Drawing.Point(3, 447);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(1140, 19);
		this.panel18.TabIndex = 51;
		this.label93.AutoSize = true;
		this.label93.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label93.ForeColor = System.Drawing.Color.White;
		this.label93.Location = new System.Drawing.Point(0, 1);
		this.label93.Name = "label93";
		this.label93.Size = new System.Drawing.Size(234, 17);
		this.label93.TabIndex = 2;
		this.label93.Text = "Detail of the Selected above Row";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1154, 761);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmForwardingNew";
		this.Text = "Forwarding";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmForwardingNew_FormClosing);
		base.Load += new System.EventHandler(ExpfrmForwarding_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(ExpfrmForwarding_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		this.tabControl2.ResumeLayout(false);
		this.tabPage3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdgpInfo).EndInit();
		this.panel14.ResumeLayout(false);
		this.panel14.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbWareHouseTo).EndInit();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbcontainerNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPackType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWareHouseFrom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPackUOM).EndInit();
		this.tabPage4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdotheritems).EndInit();
		this.panel13.ResumeLayout(false);
		this.panel13.PerformLayout();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbWareHouseToOtherItem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbWareHouseFromOtherItem).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbOtherItem).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbLoadingPort).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDestinationPort).EndInit();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbtransporter).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbInvoiceNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCustomer).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbGpNo).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistoryDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel15.ResumeLayout(false);
		this.panel15.PerformLayout();
		this.panel16.ResumeLayout(false);
		this.groupBox4.ResumeLayout(false);
		this.groupBox4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).EndInit();
		this.panel17.ResumeLayout(false);
		this.panel17.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		this.panel18.ResumeLayout(false);
		this.panel18.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
