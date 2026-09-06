using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Export;
using Architecture.BLL.Import;
using Architecture.BLL.Inventory;
using Architecture.BLL.Reports.Export;
using Architecture.BLL.Reports.Import;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Export;
using Architecture.Model.Import;
using Architecture.Model.Inventory;
using Architecture.Reports;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Import;

public class ImForwardingGdn : Form
{
	private bool DoHaveSaveRight;

	private bool DoHavePrintRights;

	private bool DoHaveUpdateRights;

	private bool DoHaveCanViewAllRecordRights;

	private int RecId;

	private bool UpdateMode;

	private int updateDetailIndex;

	private DateTime lotcompletedate;

	private DropDownBind DDL = new DropDownBind();

	private UserAccount UserAccount = new UserAccount();

	private DataTable dtDetail = new DataTable();

	private DataTable dtGetInvoiceDetail = new DataTable();

	private DataTable dtgatepass = new DataTable();

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel5;

	private Panel panel4;

	private Panel panel3;

	private Panel panel2;

	private TabPage tabPage2;

	private ToolStrip toolStrip1;

	private ToolStripButton BtnNew;

	private ToolStripButton btnSave;

	private ToolStripButton btnUpdate;

	internal UltraCombo cmbCustomer;

	internal UltraCombo cmbShippingAgent;

	private TextBox txtShippedContainer;

	private Label label25;

	private TextBox txtNoOfContainer;

	private Label label16;

	private Label label24;

	private DateTimePicker txtCompletedDate;

	private Label label26;

	private TextBox txtOtherCharges;

	internal UltraCombo cmbShippingLIne;

	private TextBox txtFreight;

	private Label label15;

	internal UltraCombo cmbDestinationPort;

	private Label label23;

	private TextBox txtCNICNO;

	internal UltraCombo cmbLoadingPort;

	private Label label22;

	private Label label21;

	private Label label12;

	private Label label19;

	private Label label14;

	private TextBox txtBilityNo;

	private Label label11;

	private Label label10;

	private TextBox txtDriverCellNo;

	private Label label9;

	internal UltraCombo cmbtransporter;

	private TextBox txtDriverName;

	internal UltraCombo cmbLcOrderNo;

	private TextBox txtVehicleNo;

	private Label label8;

	private Label label7;

	private Label label6;

	private Label label4;

	private DateTimePicker txtGPDate;

	internal UltraCombo combranches;

	internal UltraCombo comproject;

	private DateTimePicker DocDate;

	private TextBox txtdocno;

	internal UltraCombo cmbpo;

	private GroupBox groupBox3;

	private TextBox txtItemDesc;

	private Label label20;

	private Button btnAddinGrid;

	internal UltraCombo cmbPackUOM;

	private TextBox txtNetWeight;

	private Label label30;

	private TextBox txtSeal;

	private TextBox txtContainerNo;

	private Label label37;

	private Label label36;

	private Label label31;

	internal UltraCombo cmbPackType;

	private Label label29;

	internal UltraCombo cmbItem;

	private Label label28;

	internal GridEX grdDetail;

	private Panel panel6;

	private Panel panel10;

	private Panel panel8;

	private ToolStrip toolStrip2;

	private ToolStripSeparator toolStripSeparator1;

	internal GridEX DataGridHistory;

	private CheckBox chkdateisactive;

	internal UltraCombo cmbGpNo;

	private Button btnDetailCancel;

	private Button btnDetailUpdate;

	internal UltraCombo txtLcOrder;

	private Label label27;

	internal UltraCombo CmbJobLot;

	private Label label55;

	internal UltraCombo CmbWareHouse;

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

	private ToolStripLabel toolStripLabel2;

	private TextBox txtDoNetWeight;

	private Label label2;

	private TextBox txtDoGrossWeight;

	private Label label1;

	private Label label38;

	private Label label13;

	private Label label5;

	private Label label17;

	private Label label3;

	private Label label18;

	private Label label39;

	private TextBox txtGrossWeight;

	private Panel panel7;

	private Label label40;

	private Label label60;

	private Label label59;

	private Label label58;

	private Label label57;

	private Label label56;

	private Label label53;

	private Label label52;

	private Label label48;

	private Label label47;

	private Label label46;

	private Label label45;

	private Label label42;

	private Label label41;

	private Label label61;

	private ToolStripButton btnPrint;

	public ImForwardingGdn(UserAccount user)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void ExpfrmForwarding_Load(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		try
		{
			List<tblUserRights> lstRights = tblUserRights.GetByUserId(new tblUserRights
			{
				UserId = UserAccount.ID,
				ScreenName = "frmImpGrnForwarding",
				RightName = UserAccount.RoleName
			});
			if (lstRights.Count > 0)
			{
				List<tblUserRights> newList = lstRights.Where((tblUserRights x) => x.RightName == "Save").ToList();
				DoHaveSaveRight = Conversion.ToBool((object)newList[0].Value.ToString());
				newList = lstRights.Where((tblUserRights x) => x.RightName == "Print").ToList();
				DoHavePrintRights = Conversion.ToBool((object)newList[0].Value.ToString());
				newList = lstRights.Where((tblUserRights x) => x.RightName == "Update").ToList();
				DoHaveUpdateRights = Conversion.ToBool((object)newList[0].Value.ToString());
				newList = lstRights.Where((tblUserRights x) => x.RightName == "CanView AllRecord").ToList();
				DoHaveCanViewAllRecordRights = Conversion.ToBool((object)newList[0].Value.ToString());
			}
			btnSave.Enabled = DoHaveSaveRight;
			btnPrint.Enabled = DoHavePrintRights;
			btnUpdate.Enabled = DoHaveUpdateRights;
			generateCode();
			gatepassGetAll();
			cmbpo.Focus();
			packingType();
			JobLotFill();
			Project();
			braches();
			WarehouseFill();
			chkdateisactive.Checked = false;
			txtCompletedDate.Enabled = false;
			bindSeaPort();
			ShippingAgentFill();
			dtDetail.Columns.Add("PoDetailId");
			dtDetail.Columns.Add("ItemId");
			dtDetail.Columns.Add("ItemName");
			dtDetail.Columns.Add("PackTypeId");
			dtDetail.Columns.Add("Packtype");
			dtDetail.Columns.Add("ItemDescription");
			dtDetail.Columns.Add("GrossWeight", typeof(double));
			dtDetail.Columns.Add("M.Ton", typeof(double));
			dtDetail.Columns.Add("OuterUOMId");
			dtDetail.Columns.Add("OuterUOM");
			dtDetail.Columns.Add("NoOfBags", typeof(double));
			dtDetail.Columns.Add("StockWeight", typeof(double));
			dtDetail.Columns.Add("WarehouseId");
			dtDetail.Columns.Add("Warehouse");
			dtDetail.Columns.Add("Container#");
			dtDetail.Columns.Add("Seal#");
			dtDetail.Columns.Add("JobLotId");
			dtDetail.Columns.Add("JobLot");
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
		//IL_0037: Expected O, but got Unknown
		try
		{
			int Code = ImForwarding.GenerateCode(new ImForwarding
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = 234
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

	private void ShippingAgentFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dt = SupplierCustomer.ReadByOrganizationCompanyIdForExportShippinglineNClearingAgents(new SupplierCustomer
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbShippingAgent, "Id", "CompanyName", "Shipping Agent", false);
				DDL.BindDDLNew(dt, cmbShippingLIne, "Id", "CompanyName", "Shipping Line", false);
				DDL.BindDDLNew(dt, cmbtransporter, "Id", "CompanyName", "Transporter", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void chkdateisactive_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (chkdateisactive.Checked)
			{
				txtCompletedDate.Enabled = true;
				lotcompletedate = txtCompletedDate.Value;
			}
			else
			{
				txtCompletedDate.Enabled = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbInvoiceNo_Leave(object sender, EventArgs e)
	{
		ContractORDERNo();
	}

	private void ContractORDERNo()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		try
		{
			DataTable dt = ExImLcOrderPurchaseOrder.GetLcContractDetailByPurchaseOrderId(new ExImLcOrderPurchaseOrder
			{
				CompanyId = UserAccount.CompanyId,
				OrganizationId = UserAccount.OrganizationId,
				Id = Conversion.ToInt(cmbpo.Value)
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbLcOrderNo, "ExImLcOrderId", "LcOrderNo", "LcOrder No", false);
				DDL.BindDDLNew(dt, cmbCustomer, "SupplierCustomerId", "Supplier", "Supplier", false);
				DDL.BindDDLNew(dt, cmbLoadingPort, "LoadingPortId", "LoadingPort", "Loading Port", false);
				DDL.BindDDLNew(dt, cmbDestinationPort, "DestinationPortId", "DestinationPort", "Destination Port", false);
				((UltraGridBase)cmbLcOrderNo).Rows[0].Activate();
				((UltraGridBase)cmbCustomer).Rows[0].Activate();
				((UltraGridBase)cmbLoadingPort).Rows[0].Activate();
				((UltraGridBase)cmbDestinationPort).Rows[0].Activate();
			}
			else
			{
				((Control)(object)cmbLcOrderNo).Text = string.Empty;
				((Control)(object)cmbCustomer).Text = string.Empty;
				((Control)(object)cmbLoadingPort).Text = string.Empty;
				((Control)(object)cmbDestinationPort).Text = string.Empty;
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
			DataTable dt = GatePassInward.GetGatePassForImportGrn(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbGpNo, "Id", "GpSrNo", "GatePass No", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetShipmentDataByGpNoForImport()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		try
		{
			DataTable dt = GatePassInward.GetShipmentDataByGpNoForImport(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Id = Conversion.ToInt(cmbGpNo.Value)
			});
			if (dt.Rows.Count > 0)
			{
				txtVehicleNo.Text = Conversion.ToString(dt.Rows[0]["VehicleNo"]);
				txtBilityNo.Text = Conversion.ToString(dt.Rows[0]["BiltyNo"]);
				txtGPDate.Value = Conversion.ToDateTime(dt.Rows[0]["GpDate"]);
				txtFreight.Text = Conversion.ToString(dt.Rows[0]["Freight"]);
				txtFactoryWeight.Text = Conversion.ToString(dt.Rows[0]["WbGrossWeight"]);
				txtconainer1.Text = Conversion.ToString(dt.Rows[0]["Container"]);
				txtContainer2.Text = Conversion.ToString(dt.Rows[0]["Container1"]);
				DataTable dtInvoice = new DataTable();
				dtInvoice.Columns.Add("Id");
				dtInvoice.Columns.Add("OrderNo");
				dtInvoice.Rows.Add(dt.Rows[0]["ShipmentId"], dt.Rows[0]["ShipmentNo"]);
				DDL.BindDDL(dtInvoice, cmbpo, "Id", "OrderNo", "OrderNo", ZeroIndex: false);
				((UltraGridBase)cmbpo).Rows[0].Activate();
				GetPurchaseOrderDetailById();
			}
			else
			{
				txtVehicleNo.Text = string.Empty;
				txtBilityNo.Text = string.Empty;
				txtconainer1.Text = string.Empty;
				txtContainer2.Text = string.Empty;
				((Control)(object)cmbpo).Text = string.Empty;
				((UltraGridBase)cmbpo).DataSource = null;
				txtFactoryWeight.Text = "0";
				txtFreight.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void item()
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
				DDL.BindDDLNew(dtItemCombo, cmbItem, "Id", "ItemName", "Item", false);
				((UltraGridBase)cmbItem).DisplayLayout.Bands[0].Columns[1].Width = 450;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Project()
	{
		try
		{
			DataTable dtproject = CommonServices.ProjectServiceBind();
			if (dtproject.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtproject, comproject, "Id", "ProjectName", "Project Name", false);
				((UltraGridBase)comproject).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void braches()
	{
		try
		{
			DataTable dt = CommonServices.BrancheServiceBind();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, combranches, "Id", "BranchName", "Branch Name", false);
				((UltraGridBase)combranches).Rows[0].Activate();
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

	private void WarehouseFill()
	{
		try
		{
			DataTable dtwh = CommonServices.getActiveWareHouse();
			if (dtwh.Rows.Count > 0)
			{
				DDL.BindDDL(dtwh, CmbWareHouse, "Id", "WareHouseName", "Godown Name", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void packingType()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dt = ExImPackMaterilaType.Getall(new ExImPackMaterilaType
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbPackType, "Id", "Description", "Pack Type ", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbItem_Leave(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		try
		{
			List<UOMSchedule> lst = UOMSchedule.SearchByObjectList(new UOMSchedule
			{
				CompanyId = UserAccount.CompanyId,
				OrganizationId = UserAccount.OrganizationId,
				ItemId = Conversion.ToInt(cmbItem.Value)
			});
			DataTable dtCOMMUOM = new DataTable();
			dtCOMMUOM.Columns.Add("Id");
			dtCOMMUOM.Columns.Add("Equivalent");
			for (int i = 0; i < lst.Count; i++)
			{
				dtCOMMUOM.Rows.Add(lst[i].Id, lst[i].Equivalent);
			}
			if (dtCOMMUOM.Rows.Count > 0)
			{
				DDL.BindDDL(dtCOMMUOM, cmbPackUOM, "Id", "Equivalent", "Inner UOM ", ZeroIndex: false);
				DDL.BindDDL(dtCOMMUOM, cmbPackUOM, "Id", "Equivalent", "Outer UOM ", ZeroIndex: false);
			}
			if (dtGetInvoiceDetail.Rows.Count > 0 && Conversion.ToInt(cmbItem.Value) == Conversion.ToInt(dtGetInvoiceDetail.Rows[0]["ItemId"]))
			{
				txtItemDesc.Text = Conversion.ToString(dtGetInvoiceDetail.Rows[0]["ItemDescriptionManual"]);
				cmbPackUOM.Value = Conversion.ToInt(dtGetInvoiceDetail.Rows[0]["ItemUOMId"]);
				cmbPackType.Value = Conversion.ToInt(dtGetInvoiceDetail.Rows[0]["PackingMaterialTypeId"]);
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
			GetShipmentDataByGpNoForImport();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetPurchaseOrderDetailById()
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Expected O, but got Unknown
		try
		{
			double TotalNetWeight = 0.0;
			double TotalGrossWeight = 0.0;
			DataTable dtItem = new DataTable();
			dtItem.Columns.Add("Id");
			dtItem.Columns.Add("Name");
			DataTable dtDeliveryOrder = ExImLcOrderPurchaseOrder.ReadPurchaseOrderByContainerNo(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Id = Conversion.ToInt(cmbpo.Value),
				Container1 = Conversion.ToString((object)txtconainer1.Text.Trim()) + " ," + Conversion.ToString((object)txtContainer2.Text.Trim())
			});
			if (dtDeliveryOrder.Rows.Count <= 0)
			{
				return;
			}
			txtNoOfContainer.Text = Conversion.ToString(dtDeliveryOrder.Rows[0]["Containers"]);
			for (int i = 0; i < dtDeliveryOrder.Rows.Count; i++)
			{
				TotalNetWeight += Conversion.ToDouble(dtDeliveryOrder.Rows[i]["NetWeight"]);
				TotalGrossWeight += Conversion.ToDouble(dtDeliveryOrder.Rows[i]["GrossWeight"]);
			}
			txtDoNetWeight.Text = TotalNetWeight.ToString();
			txtDoGrossWeight.Text = TotalGrossWeight.ToString();
			double FactoryWeight = Conversion.ToDouble((object)txtFactoryWeight.Text.Trim());
			dtDetail.Rows.Clear();
			for (int j = 0; j < dtDeliveryOrder.Rows.Count; j++)
			{
				double Mton = Conversion.ToDouble(dtDeliveryOrder.Rows[j]["NetWeight"]);
				double StockWeight = 0.0;
				if (TotalNetWeight > 0.0 && FactoryWeight > 0.0 && Mton > 0.0)
				{
					StockWeight = Conversion.ToDouble((object)(FactoryWeight * (Mton / TotalNetWeight * 100.0) / 100.0));
				}
				dtItem.Rows.Add(dtDeliveryOrder.Rows[j]["ItemId"], dtDeliveryOrder.Rows[j]["ItemName"]);
				if (RecId == 0 && !btnUpdate.Visible)
				{
					dtDetail.Rows.Add(dtDeliveryOrder.Rows[j]["Id"], dtDeliveryOrder.Rows[j]["ItemId"], dtDeliveryOrder.Rows[j]["ItemName"], dtDeliveryOrder.Rows[j]["PackingTypeId"], dtDeliveryOrder.Rows[j]["PackingType"], dtDeliveryOrder.Rows[j]["RemarksDetail"], Conversion.ToDouble(dtDeliveryOrder.Rows[j]["GrossWeight"]) / 1000.0, Conversion.ToDouble(dtDeliveryOrder.Rows[j]["NetWeight"]) / 1000.0, dtDeliveryOrder.Rows[j]["ItemUomId"], dtDeliveryOrder.Rows[j]["ItemUOM"], dtDeliveryOrder.Rows[j]["NoOfPacks"], StockWeight, dtDeliveryOrder.Rows[j]["ToWarehouseId"], dtDeliveryOrder.Rows[j]["WareHouseName"], dtDeliveryOrder.Rows[j]["ContainerNo"], dtDeliveryOrder.Rows[j]["SealNo"], dtDeliveryOrder.Rows[j]["JobLotId"], dtDeliveryOrder.Rows[j]["JobLotDescription"]);
				}
			}
			grdDetail.DataSource = dtDetail;
			grdDetail.RetrieveStructure();
			GridDetailSetting();
			if (dtItem.Rows.Count > 0)
			{
				DDL.BindDDL(dtItem, cmbItem, "Id", "Name", "Item", ZeroIndex: false);
				((UltraGridBase)cmbItem).Rows[0].Activate();
				((UltraGridBase)cmbItem).DisplayLayout.Bands[0].Columns[1].Width = 450;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnSave_Click(object sender, EventArgs e)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		//IL_041a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0421: Expected O, but got Unknown
		try
		{
			if (grdDetail.RowCount == 0)
			{
				throw new Exception("Insert Record In Detail Grid");
			}
			if (!formValidation())
			{
				return;
			}
			ImForwarding forwarding = new ImForwarding();
			if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			double Weight = 0.0;
			double TotalGWeight = 0.0;
			double DoNetWeight = Conversion.ToDouble((object)txtDoNetWeight.Text.Trim());
			double DoGrossWeight = Conversion.ToDouble((object)txtDoGrossWeight.Text.Trim());
			GridEXRow[] rows = grdDetail.GetRows();
			foreach (GridEXRow item in rows)
			{
				Weight += Conversion.ToDouble(item.Cells["M.Ton"].Value) * 1000.0;
				TotalGWeight += Conversion.ToDouble(item.Cells["GrossWeight"].Value) * 1000.0;
			}
			if (TotalGWeight != DoGrossWeight)
			{
				throw new Exception("Do Gross Weight and Detail Gross Weight Total not equal please check");
			}
			if (Weight != DoNetWeight)
			{
				throw new Exception("Do Net Weight and M.Ton not equal please check");
			}
			forwarding.OrganizationId = UserAccount.OrganizationId;
			forwarding.CompanyId = UserAccount.CompanyId;
			forwarding.BranchesId = Conversion.ToInt(combranches.Value);
			forwarding.ProjectsId = Conversion.ToInt(comproject.Value);
			forwarding.DocNo = Conversion.ToInt((object)txtdocno.Text.Trim());
			forwarding.DocDate = DocDate.Value;
			forwarding.DocumentTypeId = 234;
			forwarding.ExImInvoiceId = Conversion.ToInt(cmbpo.Value);
			forwarding.ExportSoNoId = Conversion.ToInt(cmbpo.Value);
			forwarding.LCOrderId = Conversion.ToInt(cmbLcOrderNo.Value);
			forwarding.GatePassOutwardId = Conversion.ToInt(cmbGpNo.Value);
			forwarding.GpNo = Conversion.ToString((object)((Control)(object)cmbGpNo).Text.Trim());
			forwarding.GPDate = txtGPDate.Value;
			forwarding.TransporterId = Conversion.ToInt(cmbtransporter.Value);
			forwarding.VehicleNo = txtVehicleNo.Text;
			forwarding.BiltyNo = txtBilityNo.Text;
			forwarding.DriverName = txtDriverName.Text;
			forwarding.DriverCellNo = txtDriverCellNo.Text;
			forwarding.DriverCnicNo = txtCNICNO.Text;
			forwarding.LotCompletedDate = txtCompletedDate.Value;
			forwarding.Container = txtconainer1.Text;
			forwarding.Container1 = txtContainer2.Text;
			forwarding.FreightAmt = Conversion.ToDouble((object)txtFreight.Text);
			forwarding.OtherCharges = Conversion.ToDouble((object)txtFactoryWeight.Text);
			forwarding.ShippingLineId = Conversion.ToInt(cmbShippingLIne.Value);
			forwarding.ShippingAgentId = Conversion.ToInt(cmbShippingAgent.Value);
			forwarding.SupplierCustomerId = Conversion.ToInt(cmbCustomer.Value);
			forwarding.LoadingPortId = Conversion.ToInt(cmbLoadingPort.Value);
			forwarding.DestinationPortId = Conversion.ToInt(cmbDestinationPort.Value);
			forwarding.ShippedContainer = Conversion.ToInt((object)txtShippedContainer.Text);
			if (chkdateisactive.Checked)
			{
				forwarding.LotCompletedDate = Conversion.ToDateTime((object)lotcompletedate);
			}
			forwarding.NoOfContainer = Conversion.ToInt((object)txtNoOfContainer.Text);
			forwarding.EntryUser = UserAccount.ID;
			forwarding.ModifyUser = UserAccount.ID;
			forwarding.EntryDate = DateTime.Now;
			forwarding.ModifyDate = DateTime.Now;
			forwarding.IsApproved = false;
			forwarding.ExImForwardingDetail = new List<ImForwardingPackingDetail>();
			rows = grdDetail.GetRows();
			foreach (GridEXRow r in rows)
			{
				ImForwardingPackingDetail exImForwardingPackingDetail = new ImForwardingPackingDetail();
				if (Conversion.ToInt(r.Cells["ItemId"].Value) != 0)
				{
					exImForwardingPackingDetail.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					exImForwardingPackingDetail.ImPurchaseOrderDetailId = Conversion.ToInt(r.Cells["PoDetailId"].Value);
					if (Conversion.ToInt(r.Cells["PackTypeId"].Value) != 0)
					{
						exImForwardingPackingDetail.PackingMaterialId = Conversion.ToInt(r.Cells["PackTypeId"].Value);
						if (Conversion.ToDouble(r.Cells["M.Ton"].Value) != 0.0)
						{
							exImForwardingPackingDetail.NetWeight = Conversion.ToDouble((object)r.Cells["M.Ton"].Text.Trim()) * 1000.0;
							if (Conversion.ToDouble(r.Cells["GrossWeight"].Value) != 0.0)
							{
								exImForwardingPackingDetail.GrossWeight = Conversion.ToDouble((object)r.Cells["GrossWeight"].Text.Trim()) * 1000.0;
								if (Conversion.ToDouble(r.Cells["StockWeight"].Value) != 0.0)
								{
									exImForwardingPackingDetail.StockWeight = Conversion.ToDouble((object)r.Cells["StockWeight"].Text.Trim());
									if (Conversion.ToInt(r.Cells["OuterUOMId"].Value) != 0)
									{
										exImForwardingPackingDetail.UOMScheduleIdOuter = Conversion.ToInt(r.Cells["OuterUOMId"].Value);
										exImForwardingPackingDetail.UOMScheduleIdInner = Conversion.ToInt(r.Cells["OuterUOMId"].Value);
										if (Conversion.ToDouble(r.Cells["NoOfBags"].Value) != 0.0)
										{
											exImForwardingPackingDetail.OuterQty = Conversion.ToDouble((object)r.Cells["NoOfBags"].Text.Trim());
											exImForwardingPackingDetail.InnerQty = Conversion.ToDouble((object)r.Cells["NoOfBags"].Text.Trim());
											if (Conversion.ToInt(r.Cells["WarehouseId"].Value) != 0)
											{
												exImForwardingPackingDetail.WarehouseId = Conversion.ToInt(r.Cells["WarehouseId"].Value);
												if (Conversion.ToString(r.Cells["Container#"].Value) != string.Empty)
												{
													exImForwardingPackingDetail.ContainerNo = Conversion.ToString((object)r.Cells["Container#"].Text.Trim());
													if (Conversion.ToString(r.Cells["Seal#"].Value) != string.Empty)
													{
														exImForwardingPackingDetail.SealNo = Conversion.ToString((object)r.Cells["Seal#"].Text.Trim());
														if (Conversion.ToInt(r.Cells["JobLotId"].Value) != 0)
														{
															exImForwardingPackingDetail.LotJobId = Conversion.ToInt(r.Cells["JobLotId"].Value);
															exImForwardingPackingDetail.ExImLcOrderId = forwarding.LCOrderId;
															exImForwardingPackingDetail.ImPurchaseOrderId = forwarding.ExImInvoiceId;
															exImForwardingPackingDetail.ItemDescription = Conversion.ToString((object)r.Cells["ItemDescription"].Text.Trim());
															forwarding.ExImForwardingDetail.Add(exImForwardingPackingDetail);
															continue;
														}
														MessageBox.Show("Job Lot Required In Grid");
														return;
													}
													MessageBox.Show("Seal No Required In Grid");
													return;
												}
												MessageBox.Show("Container Required In Grid");
												return;
											}
											MessageBox.Show("Warehouse Required In Grid");
											return;
										}
										MessageBox.Show("No Of Bags Required In Grid");
										return;
									}
									MessageBox.Show("Pack UOM Required In Grid");
									return;
								}
								MessageBox.Show("StockWeight Required In Grid");
								return;
							}
							MessageBox.Show("Gross Weight Required In Grid");
							return;
						}
						MessageBox.Show("M.Ton Required In Grid");
						return;
					}
					MessageBox.Show("Packing Type Required In Grid");
					return;
				}
				MessageBox.Show("Item Required In Grid");
				return;
			}
			if (ImForwarding.Save(forwarding) > 0)
			{
				MessageBox.Show("Record Save SuccessFully");
			}
			FormReset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdate_Click(object sender, EventArgs e)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		//IL_043c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0443: Expected O, but got Unknown
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
			ImForwarding forwarding = new ImForwarding();
			if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			if (RecId > 0)
			{
				forwarding.Id = RecId;
				double Weight = 0.0;
				double TotalGWeight = 0.0;
				double DoNetWeight = Conversion.ToDouble((object)txtDoNetWeight.Text.Trim());
				double DoGrossWeight = Conversion.ToDouble((object)txtDoGrossWeight.Text.Trim());
				GridEXRow[] rows = grdDetail.GetRows();
				foreach (GridEXRow item in rows)
				{
					Weight += Conversion.ToDouble(item.Cells["M.Ton"].Value) * 1000.0;
					TotalGWeight += Conversion.ToDouble(item.Cells["GrossWeight"].Value) * 1000.0;
				}
				if (TotalGWeight != DoGrossWeight)
				{
					throw new Exception("Do Gross Weight and Detail Gross Weight Total not equal please check");
				}
				if (Weight != DoNetWeight)
				{
					throw new Exception("Do Net Weight and M.Ton not equal please check");
				}
				forwarding.OrganizationId = UserAccount.OrganizationId;
				forwarding.CompanyId = UserAccount.CompanyId;
				forwarding.BranchesId = Conversion.ToInt(combranches.Value);
				forwarding.ProjectsId = Conversion.ToInt(comproject.Value);
				forwarding.DocNo = Conversion.ToInt((object)txtdocno.Text.Trim());
				forwarding.DocDate = DocDate.Value;
				forwarding.DocumentTypeId = 234;
				forwarding.ExImInvoiceId = Conversion.ToInt(cmbpo.Value);
				forwarding.ExportSoNoId = Conversion.ToInt(cmbpo.Value);
				forwarding.LCOrderId = Conversion.ToInt(cmbLcOrderNo.Value);
				forwarding.GatePassOutwardId = Conversion.ToInt(cmbGpNo.Value);
				forwarding.GpNo = Conversion.ToString((object)((Control)(object)cmbGpNo).Text.Trim());
				forwarding.GPDate = txtGPDate.Value;
				forwarding.TransporterId = Conversion.ToInt(cmbtransporter.Value);
				forwarding.VehicleNo = txtVehicleNo.Text;
				forwarding.BiltyNo = txtBilityNo.Text;
				forwarding.DriverName = txtDriverName.Text;
				forwarding.DriverCellNo = txtDriverCellNo.Text;
				forwarding.DriverCnicNo = txtCNICNO.Text;
				forwarding.LotCompletedDate = txtCompletedDate.Value;
				forwarding.Container = txtconainer1.Text;
				forwarding.Container1 = txtContainer2.Text;
				forwarding.FreightAmt = Conversion.ToDouble((object)txtFreight.Text);
				forwarding.OtherCharges = Conversion.ToDouble((object)txtFactoryWeight.Text);
				forwarding.ShippingLineId = Conversion.ToInt(cmbShippingLIne.Value);
				forwarding.ShippingAgentId = Conversion.ToInt(cmbShippingAgent.Value);
				forwarding.SupplierCustomerId = Conversion.ToInt(cmbCustomer.Value);
				forwarding.LoadingPortId = Conversion.ToInt(cmbLoadingPort.Value);
				forwarding.DestinationPortId = Conversion.ToInt(cmbDestinationPort.Value);
				forwarding.ShippedContainer = Conversion.ToInt((object)txtShippedContainer.Text);
				if (chkdateisactive.Checked)
				{
					forwarding.LotCompletedDate = Conversion.ToDateTime((object)lotcompletedate);
				}
				forwarding.NoOfContainer = Conversion.ToInt((object)txtNoOfContainer.Text);
				forwarding.EntryUser = UserAccount.ID;
				forwarding.ModifyUser = UserAccount.ID;
				forwarding.EntryDate = DateTime.Now;
				forwarding.ModifyDate = DateTime.Now;
				forwarding.IsApproved = false;
				forwarding.ExImForwardingDetail = new List<ImForwardingPackingDetail>();
				rows = grdDetail.GetRows();
				foreach (GridEXRow r in rows)
				{
					ImForwardingPackingDetail exImForwardingPackingDetail = new ImForwardingPackingDetail();
					if (Conversion.ToInt(r.Cells["ItemId"].Value) != 0)
					{
						exImForwardingPackingDetail.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
						exImForwardingPackingDetail.ImPurchaseOrderDetailId = Conversion.ToInt(r.Cells["PoDetailId"].Value);
						if (Conversion.ToInt(r.Cells["PackTypeId"].Value) != 0)
						{
							exImForwardingPackingDetail.PackingMaterialId = Conversion.ToInt(r.Cells["PackTypeId"].Value);
							if (Conversion.ToDouble(r.Cells["M.Ton"].Value) != 0.0)
							{
								exImForwardingPackingDetail.NetWeight = Conversion.ToDouble((object)r.Cells["M.Ton"].Text.Trim()) * 1000.0;
								if (Conversion.ToDouble(r.Cells["GrossWeight"].Value) != 0.0)
								{
									exImForwardingPackingDetail.GrossWeight = Conversion.ToDouble((object)r.Cells["GrossWeight"].Text.Trim()) * 1000.0;
									if (Conversion.ToDouble(r.Cells["StockWeight"].Value) != 0.0)
									{
										exImForwardingPackingDetail.StockWeight = Conversion.ToDouble((object)r.Cells["StockWeight"].Text.Trim());
										if (Conversion.ToInt(r.Cells["OuterUOMId"].Value) != 0)
										{
											exImForwardingPackingDetail.UOMScheduleIdOuter = Conversion.ToInt(r.Cells["OuterUOMId"].Value);
											exImForwardingPackingDetail.UOMScheduleIdInner = Conversion.ToInt(r.Cells["OuterUOMId"].Value);
											if (Conversion.ToDouble(r.Cells["NoOfBags"].Value) != 0.0)
											{
												exImForwardingPackingDetail.OuterQty = Conversion.ToDouble((object)r.Cells["NoOfBags"].Text.Trim());
												exImForwardingPackingDetail.InnerQty = Conversion.ToDouble((object)r.Cells["NoOfBags"].Text.Trim());
												if (Conversion.ToInt(r.Cells["WarehouseId"].Value) != 0)
												{
													exImForwardingPackingDetail.WarehouseId = Conversion.ToInt(r.Cells["WarehouseId"].Value);
													if (Conversion.ToString(r.Cells["Container#"].Value) != string.Empty)
													{
														exImForwardingPackingDetail.ContainerNo = Conversion.ToString((object)r.Cells["Container#"].Text.Trim());
														if (Conversion.ToString(r.Cells["Seal#"].Value) != string.Empty)
														{
															exImForwardingPackingDetail.SealNo = Conversion.ToString((object)r.Cells["Seal#"].Text.Trim());
															if (Conversion.ToInt(r.Cells["JobLotId"].Value) != 0)
															{
																exImForwardingPackingDetail.LotJobId = Conversion.ToInt(r.Cells["JobLotId"].Value);
																exImForwardingPackingDetail.ExImLcOrderId = forwarding.LCOrderId;
																exImForwardingPackingDetail.ImPurchaseOrderId = forwarding.ExImInvoiceId;
																exImForwardingPackingDetail.ItemDescription = Conversion.ToString((object)r.Cells["ItemDescription"].Text.Trim());
																forwarding.ExImForwardingDetail.Add(exImForwardingPackingDetail);
																continue;
															}
															MessageBox.Show("Job Lot Required In Grid");
															return;
														}
														MessageBox.Show("Seal No Required In Grid");
														return;
													}
													MessageBox.Show("Container Required In Grid");
													return;
												}
												MessageBox.Show("Warehouse Required In Grid");
												return;
											}
											MessageBox.Show("No Of Bags Required In Grid");
											return;
										}
										MessageBox.Show("Pack UOM Required In Grid");
										return;
									}
									MessageBox.Show("StockWeight Required In Grid");
									return;
								}
								MessageBox.Show("Gross Weight Required In Grid");
								return;
							}
							MessageBox.Show("M.Ton Required In Grid");
							return;
						}
						MessageBox.Show("Packing Type Required In Grid");
						return;
					}
					MessageBox.Show("Item Required In Grid");
					return;
				}
				if (ImForwarding.Save(forwarding) > 0)
				{
					MessageBox.Show("Record Update SuccessFully");
				}
				FormReset();
				return;
			}
			throw new Exception("Record Not Found For Update");
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
			generateCode();
			gatepassGetAll();
			((Control)(object)cmbLcOrderNo).Enabled = true;
			((Control)(object)cmbpo).Enabled = true;
			((Control)(object)cmbGpNo).Enabled = true;
			((UltraGridBase)combranches).Rows[0].Activate();
			((UltraGridBase)comproject).Rows[0].Activate();
			((Control)(object)cmbLcOrderNo).Text = string.Empty;
			((Control)(object)cmbtransporter).Text = string.Empty;
			((Control)(object)cmbLoadingPort).Text = string.Empty;
			((Control)(object)cmbDestinationPort).Text = string.Empty;
			((Control)(object)cmbShippingLIne).Text = string.Empty;
			((Control)(object)cmbpo).Text = string.Empty;
			((Control)(object)cmbShippingAgent).Text = string.Empty;
			txtCompletedDate.Value = DateTime.Now;
			((Control)(object)cmbGpNo).Text = string.Empty;
			((UltraGridBase)cmbGpNo).DataSource = null;
			txtFactoryWeight.Text = "0";
			txtVehicleNo.Text = string.Empty;
			txtBilityNo.Text = string.Empty;
			txtDriverName.Text = string.Empty;
			txtDriverCellNo.Text = string.Empty;
			txtCNICNO.Text = string.Empty;
			txtOtherCharges.Text = string.Empty;
			txtFreight.Text = string.Empty;
			txtShippedContainer.Text = string.Empty;
			txtNoOfContainer.Text = string.Empty;
			txtCompletedDate.Value = DateTime.Now;
			((Control)(object)cmbCustomer).Text = string.Empty;
			txtconainer1.Text = string.Empty;
			txtContainer2.Text = string.Empty;
			txtDoGrossWeight.Text = string.Empty;
			txtDoNetWeight.Text = string.Empty;
			((Control)(object)cmbItem).Text = string.Empty;
			((UltraGridBase)cmbItem).DataSource = null;
			cmbpo.Focus();
			dtDetail.Rows.Clear();
			grdDetail.ClearStructure();
			btnSave.Visible = true;
			UpdateMode = false;
			btnUpdate.Visible = false;
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
			((Control)(object)txtLcOrder).Text = string.Empty;
			((Control)(object)cmbItem).Text = string.Empty;
			((Control)(object)cmbPackType).Text = string.Empty;
			txtItemDesc.Text = string.Empty;
			txtNetWeight.Text = string.Empty;
			((Control)(object)cmbPackUOM).Text = string.Empty;
			((Control)(object)cmbPackUOM).Text = string.Empty;
			txtContainerNo.Text = string.Empty;
			txtSeal.Text = string.Empty;
			((Control)(object)CmbWareHouse).Text = string.Empty;
			((Control)(object)CmbJobLot).Text = string.Empty;
			btnDetailUpdate.Visible = false;
			btnDetailCancel.Visible = false;
			btnAddinGrid.Visible = true;
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
			RecId = Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells[0].Value.ToString());
			ReadById(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ReadById(int Id)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		try
		{
			btnSave.Visible = false;
			btnUpdate.Visible = true;
			ImForwarding exImLc = new ImForwarding();
			exImLc = ImForwarding.GetByID(Id);
			if (exImLc != null)
			{
				tabControl1.SelectedIndex = 0;
				txtdocno.Text = Conversion.ToString((object)exImLc.DocNo);
				txtGPDate.Value = exImLc.DocDate;
				DataTable dtPo = new DataTable();
				dtPo.Columns.Add("Id");
				dtPo.Columns.Add("Name");
				dtPo.Rows.Add(exImLc.ExImInvoiceId, exImLc.PurchaseOrderNo);
				if (dtPo.Rows.Count > 0)
				{
					DDL.BindDDL(dtPo, cmbpo, "Id", "Name", "PurchaseOrder No", ZeroIndex: false);
					((UltraGridBase)cmbpo).Rows[0].Activate();
				}
				DataTable dtContract = new DataTable();
				dtContract.Columns.Add("Id");
				dtContract.Columns.Add("Name");
				dtContract.Rows.Add(exImLc.LCOrderId, exImLc.LcOrderNo);
				if (dtContract.Rows.Count > 0)
				{
					DDL.BindDDL(dtContract, cmbLcOrderNo, "Id", "Name", "Contract No", ZeroIndex: false);
					((UltraGridBase)cmbLcOrderNo).Rows[0].Activate();
				}
				DataTable dtGatePass = new DataTable();
				dtGatePass.Columns.Add("Id");
				dtGatePass.Columns.Add("Name");
				dtGatePass.Rows.Add(exImLc.GatePassOutwardId, exImLc.GpSrNo);
				if (dtGatePass.Rows.Count > 0)
				{
					DDL.BindDDL(dtGatePass, cmbGpNo, "Id", "Name", "Gp SrNo", ZeroIndex: false);
					((UltraGridBase)cmbGpNo).Rows[0].Activate();
				}
				DataTable dtCustomer = new DataTable();
				dtCustomer.Columns.Add("Id");
				dtCustomer.Columns.Add("Name");
				dtCustomer.Rows.Add(exImLc.SupplierCustomerId, exImLc.Customer);
				if (dtCustomer.Rows.Count > 0)
				{
					DDL.BindDDL(dtCustomer, cmbCustomer, "Id", "Name", "Customer", ZeroIndex: false);
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
				comproject.Value = Conversion.ToInt((object)exImLc.ProjectsId);
				combranches.Value = Conversion.ToInt((object)exImLc.BranchesId);
				cmbShippingLIne.Value = Conversion.ToInt((object)exImLc.ShippingLineId);
				cmbShippingAgent.Value = Conversion.ToInt((object)exImLc.ShippingAgentId);
				cmbtransporter.Value = Conversion.ToInt((object)exImLc.TransporterId);
				txtDriverCellNo.Text = Conversion.ToString((object)exImLc.DriverCellNo);
				txtDriverName.Text = Conversion.ToString((object)exImLc.DriverName);
				txtCNICNO.Text = Conversion.ToString((object)exImLc.DriverCnicNo);
				txtGPDate.Value = exImLc.GPDate;
				txtVehicleNo.Text = Conversion.ToString((object)exImLc.VehicleNo);
				txtBilityNo.Text = Conversion.ToString((object)exImLc.BiltyNo);
				txtconainer1.Text = Conversion.ToString((object)exImLc.Container);
				txtContainer2.Text = Conversion.ToString((object)exImLc.Container1);
				txtFreight.Text = Conversion.ToString((object)exImLc.FreightAmt);
				txtFactoryWeight.Text = Conversion.ToString((object)exImLc.OtherCharges);
				txtShippedContainer.Text = Conversion.ToString((object)exImLc.ShippedContainer);
				txtNoOfContainer.Text = Conversion.ToString((object)exImLc.NoOfContainer);
				txtCompletedDate.Value = Conversion.ToDateTime((object)exImLc.LotCompletedDate);
				GetWbAndPurchaseByGatePassId(Conversion.ToInt(cmbGpNo.Value));
				dtDetail.Clear();
				for (int i = 0; i < exImLc.ExImForwardingDetail.Count; i++)
				{
					double PoNetWeight = Conversion.ToDouble((object)txtDoNetWeight.Text.Trim());
					double FactoryWeight = Conversion.ToDouble((object)txtFactoryWeight.Text.Trim());
					double Mton = exImLc.ExImForwardingDetail[i].NetWeight;
					double StockWeight = 0.0;
					if (PoNetWeight > 0.0 && FactoryWeight > 0.0 && Mton > 0.0)
					{
						StockWeight = Conversion.ToDouble((object)(FactoryWeight * (Mton / PoNetWeight * 100.0) / 100.0));
					}
					dtDetail.Rows.Add(exImLc.ExImForwardingDetail[i].ImPurchaseOrderDetailId, exImLc.ExImForwardingDetail[i].ItemId, exImLc.ExImForwardingDetail[i].ItemName, exImLc.ExImForwardingDetail[i].PackingMaterialId, exImLc.ExImForwardingDetail[i].Packtype, exImLc.ExImForwardingDetail[i].ItemDescription, exImLc.ExImForwardingDetail[i].GrossWeight / 1000.0, exImLc.ExImForwardingDetail[i].NetWeight / 1000.0, exImLc.ExImForwardingDetail[i].UOMScheduleIdOuter, exImLc.ExImForwardingDetail[i].OuterUOM, exImLc.ExImForwardingDetail[i].OuterQty, StockWeight, exImLc.ExImForwardingDetail[i].WarehouseId, exImLc.ExImForwardingDetail[i].WareHouseName, exImLc.ExImForwardingDetail[i].ContainerNo, exImLc.ExImForwardingDetail[i].SealNo, exImLc.ExImForwardingDetail[i].LotJobId, exImLc.ExImForwardingDetail[i].JobLotName);
				}
				grdDetail.DataSource = dtDetail;
				grdDetail.RetrieveStructure();
				GridDetailSetting();
				UpdateMode = true;
				((Control)(object)cmbGpNo).Enabled = false;
				((Control)(object)cmbLcOrderNo).Enabled = false;
				((Control)(object)cmbpo).Enabled = false;
				item();
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

	private void GetWbAndPurchaseByGatePassId(int gpiId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		try
		{
			DataTable dt = ImForwarding.GetWbAndPurchaseByGatePassId(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Id = gpiId,
				Container1 = txtconainer1.Text,
				Container2 = txtContainer2.Text
			});
			if (dt.Rows.Count > 0)
			{
				txtFactoryWeight.Text = Conversion.ToString(dt.Rows[0]["ScaleWeight"]);
				txtDoGrossWeight.Text = Conversion.ToString(dt.Rows[0]["GrossWeight"]);
				txtDoNetWeight.Text = Conversion.ToString(dt.Rows[0]["NetWeight"]);
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
			MessageBox.Show("Please select Pack Type");
			cmbPackType.Focus();
			return false;
		}
		if (txtGrossWeight.Text.Trim() == string.Empty || !(Conversion.ToDouble((object)txtGrossWeight.Text) > 0.0))
		{
			MessageBox.Show("Please Insert Gross weight");
			txtGrossWeight.Focus();
			return false;
		}
		if (txtNetWeight.Text.Trim() == string.Empty || !(Conversion.ToDouble((object)txtNetWeight.Text) > 0.0))
		{
			MessageBox.Show("Please Insert M.Ton");
			txtNetWeight.Focus();
			return false;
		}
		if (((UltraGridBase)cmbPackUOM).ActiveRow == null)
		{
			MessageBox.Show("Please Select Pack UOM");
			cmbPackUOM.Focus();
			return false;
		}
		if (!(Conversion.ToDouble((object)txtNoOfBags.Text) > 0.0))
		{
			MessageBox.Show("Please Insert No Of Bags");
			txtNoOfBags.Focus();
			return false;
		}
		if (((UltraGridBase)CmbJobLot).ActiveRow == null)
		{
			MessageBox.Show("Please Select Job Lot");
			CmbJobLot.Focus();
			return false;
		}
		if (((UltraGridBase)CmbWareHouse).ActiveRow == null)
		{
			MessageBox.Show("Please Select Warehouse");
			CmbWareHouse.Focus();
			return false;
		}
		if (txtContainerNo.Text.Trim() == "")
		{
			MessageBox.Show("Please Insert Container No");
			txtContainerNo.Focus();
			return false;
		}
		if (txtSeal.Text.Trim() == "")
		{
			MessageBox.Show("Please Insert Seal No");
			txtSeal.Focus();
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
		if (((UltraGridBase)cmbpo).ActiveRow == null)
		{
			MessageBox.Show("Please Select Invoice No");
			cmbpo.Focus();
			return false;
		}
		if (((UltraGridBase)cmbLcOrderNo).ActiveRow == null)
		{
			MessageBox.Show("Please Select Contract No");
			cmbLcOrderNo.Focus();
			return false;
		}
		if (((UltraGridBase)cmbCustomer).ActiveRow == null)
		{
			MessageBox.Show("Please Select Customer");
			cmbCustomer.Focus();
			return false;
		}
		if (txtDriverCellNo.Text.Trim() == string.Empty || txtDriverCellNo.Text == null)
		{
			MessageBox.Show("Driver Cell No Is Required");
			txtDriverCellNo.Focus();
			return false;
		}
		if (txtDriverName.Text.Trim() == string.Empty || txtDriverName.Text == null)
		{
			MessageBox.Show("Driver Name Is Required");
			txtDriverName.Focus();
			return false;
		}
		if (((UltraGridBase)cmbGpNo).ActiveRow == null)
		{
			MessageBox.Show("Please Select GatePass No");
			cmbGpNo.Focus();
			return false;
		}
		if (txtVehicleNo.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Please Check Vehicle No");
			txtVehicleNo.Focus();
			return false;
		}
		if (txtBilityNo.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Please Check Bilty No");
			txtBilityNo.Focus();
			return false;
		}
		if (((UltraGridBase)cmbLoadingPort).ActiveRow == null)
		{
			MessageBox.Show("Please Select Loading Port");
			cmbLoadingPort.Focus();
			return false;
		}
		if (((UltraGridBase)cmbDestinationPort).ActiveRow == null)
		{
			MessageBox.Show("Please Select Destination Port");
			cmbDestinationPort.Focus();
			return false;
		}
		if (txtShippedContainer.Text.Trim() == string.Empty || Conversion.ToInt((object)txtShippedContainer.Text) == 0)
		{
			MessageBox.Show("Please Check Shipped Container");
			txtShippedContainer.Focus();
			return false;
		}
		if (txtNoOfContainer.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtNoOfContainer.Text) == 0.0)
		{
			MessageBox.Show("Please Check No Of Container");
			txtNoOfContainer.Focus();
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

	private void btnclose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void btnAddinGrid_Click(object sender, EventArgs e)
	{
		try
		{
			_ = Conversion.ToInt((object)txtNetWeight.Text) / 1000;
			if (formdetailvalidation())
			{
				dtDetail.Rows.Add(cmbItem.Value, ((Control)(object)cmbItem).Text, cmbPackType.Value, ((Control)(object)cmbPackType).Text, txtItemDesc.Text, Conversion.ToDouble((object)txtGrossWeight.Text.Trim()), Conversion.ToDouble((object)txtNetWeight.Text.Trim()), cmbPackUOM.Value, ((Control)(object)cmbPackUOM).Text, Conversion.ToDouble((object)txtNoOfBags.Text.Trim()), 0, CmbWareHouse.Value, ((Control)(object)CmbWareHouse).Text.Trim(), txtContainerNo.Text, txtSeal.Text, CmbJobLot.Value, ((Control)(object)CmbJobLot).Text.Trim());
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
				cmbItem.Value = item.Cells["ItemId"].Value;
				cmbPackType.Value = item.Cells["PackTypeId"].Value;
				txtItemDesc.Text = item.Cells["ItemDescription"].Value.ToString();
				txtGrossWeight.Text = item.Cells["GrossWeight"].Value.ToString();
				txtNetWeight.Text = item.Cells["M.Ton"].Value.ToString();
				cmbPackUOM.Value = item.Cells["OuterUOMId"].Text.ToString();
				txtNoOfBags.Text = item.Cells["NoOfBags"].Text.ToString();
				CmbWareHouse.Value = item.Cells["WarehouseId"].Value.ToString();
				((Control)(object)CmbWareHouse).Text = item.Cells["Warehouse"].Text.ToString();
				txtContainerNo.Text = item.Cells["Container#"].Value.ToString();
				txtSeal.Text = item.Cells["Seal#"].Value.ToString();
				CmbJobLot.Value = item.Cells["JobLotId"].Value.ToString();
				btnDetailUpdate.Visible = true;
				btnDetailCancel.Visible = true;
				btnAddinGrid.Visible = false;
				cmbItem.Focus();
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
				double PoNetWeight = Conversion.ToDouble((object)txtDoNetWeight.Text.Trim());
				double FactoryWeight = Conversion.ToDouble((object)txtFactoryWeight.Text.Trim());
				double Mton = Conversion.ToDouble((object)txtNetWeight.Text) * 1000.0;
				double StockWeight = 0.0;
				if (PoNetWeight > 0.0 && FactoryWeight > 0.0 && Mton > 0.0)
				{
					StockWeight = Conversion.ToDouble((object)(FactoryWeight * (Mton / PoNetWeight * 100.0) / 100.0));
				}
				dtDetail.Rows[updateDetailIndex]["ItemId"] = cmbItem.Value;
				dtDetail.Rows[updateDetailIndex]["ItemName"] = ((Control)(object)cmbItem).Text;
				dtDetail.Rows[updateDetailIndex]["PackTypeId"] = cmbPackType.Value;
				dtDetail.Rows[updateDetailIndex]["Packtype"] = ((Control)(object)cmbPackType).Text;
				dtDetail.Rows[updateDetailIndex]["ItemDescription"] = txtItemDesc.Text;
				dtDetail.Rows[updateDetailIndex]["GrossWeight"] = txtGrossWeight.Text;
				dtDetail.Rows[updateDetailIndex]["M.Ton"] = txtNetWeight.Text;
				dtDetail.Rows[updateDetailIndex]["OuterUOMId"] = cmbPackUOM.Value;
				dtDetail.Rows[updateDetailIndex]["OuterUOM"] = ((Control)(object)cmbPackUOM).Text;
				dtDetail.Rows[updateDetailIndex]["NoOfBags"] = txtNoOfBags.Text;
				dtDetail.Rows[updateDetailIndex]["StockWeight"] = StockWeight;
				dtDetail.Rows[updateDetailIndex]["WarehouseId"] = CmbWareHouse.Value;
				dtDetail.Rows[updateDetailIndex]["Warehouse"] = ((Control)(object)CmbWareHouse).Text;
				dtDetail.Rows[updateDetailIndex]["Container#"] = txtContainerNo.Text;
				dtDetail.Rows[updateDetailIndex]["Seal#"] = txtSeal.Text;
				dtDetail.Rows[updateDetailIndex]["JobLotId"] = CmbJobLot.Value;
				dtDetail.Rows[updateDetailIndex]["JobLot"] = ((Control)(object)CmbJobLot).Text;
				btnDetailUpdate.Visible = false;
				btnDetailCancel.Visible = false;
				btnAddinGrid.Visible = true;
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
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow item = grdDetail.CurrentRow;
				dtDetail.Rows[item.RowIndex].Delete();
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
			}
			if (e.Column.Key == "Add")
			{
				dtDetail.Rows.Add(grdDetail.CurrentRow.Cells["ItemId"].Value, grdDetail.CurrentRow.Cells["ItemName"].Value, grdDetail.CurrentRow.Cells["PackTypeId"].Value, grdDetail.CurrentRow.Cells["Packtype"].Value, grdDetail.CurrentRow.Cells["ItemDescription"].Value, grdDetail.CurrentRow.Cells["GrossWeight"].Value, grdDetail.CurrentRow.Cells["M.Ton"].Value, grdDetail.CurrentRow.Cells["OuterUOMId"].Value, grdDetail.CurrentRow.Cells["OuterUOM"].Value, grdDetail.CurrentRow.Cells["NoOfBags"].Value, grdDetail.CurrentRow.Cells["WarehouseId"].Value, grdDetail.CurrentRow.Cells["Warehouse"].Value, grdDetail.CurrentRow.Cells["Container#"].Value, grdDetail.CurrentRow.Cells["Seal#"].Value, grdDetail.CurrentRow.Cells["JobLotId"].Value, grdDetail.CurrentRow.Cells["JobLot"].Value);
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

	private void GridDetailSetting()
	{
		try
		{
			grdDetail.RootTable.Columns["PoDetailId"].Visible = false;
			grdDetail.RootTable.Columns["ItemId"].Visible = false;
			grdDetail.RootTable.Columns["PackTypeId"].Visible = false;
			grdDetail.RootTable.Columns["OuterUOMId"].Visible = false;
			grdDetail.RootTable.Columns["JobLotId"].Visible = false;
			grdDetail.RootTable.Columns["WarehouseId"].Visible = false;
			grdDetail.RootTable.Columns["PoDetailId"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["ItemId"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["PackTypeId"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["OuterUOMId"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["JobLotId"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["WarehouseId"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["StockWeight"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["ItemName"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["Packtype"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["ItemDescription"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["M.Ton"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["OuterUOM"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["NoOfBags"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["Container#"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["Seal#"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["JobLot"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["Warehouse"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["JobLot"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["StockWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["GrossWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["M.Ton"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["NoOfBags"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["StockWeight"].FormatString = "#.##";
			grdDetail.RootTable.Columns["GrossWeight"].FormatString = "#,##0.###";
			grdDetail.RootTable.Columns["M.Ton"].FormatString = "#,##0.###";
			grdDetail.RootTable.Columns["NoOfBags"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["NoOfBags"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["M.Ton"].TotalFormatString = "#,##0.###";
			grdDetail.RootTable.Columns.Add("Add");
			grdDetail.RootTable.Columns["Add"].Key = "Add";
			grdDetail.RootTable.Columns["Add"].Caption = "+";
			grdDetail.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdDetail.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdDetail.RootTable.Columns["Add"].ButtonText = "+";
			grdDetail.RootTable.Columns["Add"].Width = 20;
			grdDetail.RootTable.Columns.Add("Delete");
			grdDetail.RootTable.Columns["Delete"].Key = "Delete";
			grdDetail.RootTable.Columns["Delete"].Caption = "X";
			grdDetail.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdDetail.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdDetail.RootTable.Columns["Delete"].ButtonText = "X";
			grdDetail.RootTable.Columns["Delete"].Width = 20;
			ctrlGrdBar1_Load(null, null);
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
			double MTon = 0.0;
			double OuterUom = 0.0;
			double NoOfBags = 0.0;
			MTon = Conversion.ToDouble((object)txtNetWeight.Text);
			OuterUom = Conversion.ToDouble((object)((Control)(object)cmbPackUOM).Text);
			NoOfBags = Conversion.ToDouble((object)txtNoOfBags);
			if (MTon > 0.0 && OuterUom > 0.0)
			{
				NoOfBags = MTon * 1000.0 / OuterUom;
				txtNoOfBags.Text = Conversion.ToString((object)NoOfBags);
			}
			else
			{
				txtNoOfBags.Text = "";
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
			GridBind();
		}
	}

	private void GridBind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		try
		{
			DataTable dt = ImForwarding.GetData(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				CanViewAllRecord = true,
				DocumentTypeId = 234
			});
			if (dt.Rows.Count > 0)
			{
				DataGridHistory.DataSource = dt;
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
			DataGridHistory.RootTable.Columns["RecordNo"].Visible = false;
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
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void BtnPrint_Click(object sender, EventArgs e)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = new DataTable();
			dt = ExportPdfReport.ExImForwarding_Slip_507(new ReportsParameters
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
			ShippingAgentFill();
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
			if (btnSave.Visible && btnSave.Enabled && e.Control && e.KeyCode == Keys.S && tabControl1.SelectedIndex == 0)
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
			if (btnUpdate.Visible && btnUpdate.Enabled && e.Control && e.KeyCode == Keys.U)
			{
				btnUpdate_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.P)
			{
				btnPrint_Click_1(null, null);
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
			if (File.Exists(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)grdDetail).Name))
			{
				FileStream fs = new FileStream(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)grdDetail).Name, FileMode.OpenOrCreate, FileAccess.ReadWrite);
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
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Expected O, but got Unknown
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Edit")
			{
				FormReset();
				RecId = Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells[0].Value.ToString());
				ReadById(RecId);
			}
			if (e.Column.Key == "Print")
			{
				DataTable dt = new DataTable();
				dt = ImportReports.ImportGrnSlipAndRegister(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					DocumentTypeId = 234,
					Id = Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value)
				});
				if (dt == null || dt.Rows.Count == 0)
				{
					MessageBox.Show("Not Record Found For Display");
					return;
				}
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dt, "803-ImGRnSlip.rpt");
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
			if (File.Exists(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)DataGridHistory).Name))
			{
				FileStream fs = new FileStream(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)DataGridHistory).Name, FileMode.OpenOrCreate, FileAccess.ReadWrite);
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

	private void btnPrint_Click_1(object sender, EventArgs e)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		DataTable dt = new DataTable();
		dt = ImportReports.ImportGrnSlipAndRegister(new ReportsParameters
		{
			OrganizationId = UserAccount.OrganizationId,
			CompanyId = UserAccount.CompanyId,
			DocumentTypeId = 234,
			Id = RecId
		});
		if (dt == null || dt.Rows.Count == 0)
		{
			MessageBox.Show("Not Record Found For Display");
			return;
		}
		Reporting val = new Reporting();
		val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
		val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
		val.ShowReportWithDataTable(dt, "803-ImGRnSlip.rpt");
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
		//IL_05cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d5: Expected O, but got Unknown
		//IL_0602: Unknown result type (might be due to invalid IL or missing references)
		//IL_060c: Expected O, but got Unknown
		//IL_0618: Unknown result type (might be due to invalid IL or missing references)
		//IL_0622: Expected O, but got Unknown
		//IL_0665: Unknown result type (might be due to invalid IL or missing references)
		//IL_066f: Expected O, but got Unknown
		//IL_06d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06dd: Expected O, but got Unknown
		//IL_06e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f3: Expected O, but got Unknown
		//IL_06ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0709: Expected O, but got Unknown
		//IL_0849: Unknown result type (might be due to invalid IL or missing references)
		//IL_0853: Expected O, but got Unknown
		//IL_086a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0874: Expected O, but got Unknown
		//IL_08a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ab: Expected O, but got Unknown
		//IL_08ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b6: Expected O, but got Unknown
		//IL_090f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0919: Expected O, but got Unknown
		//IL_091a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0924: Expected O, but got Unknown
		//IL_093b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0945: Expected O, but got Unknown
		//IL_095c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0966: Expected O, but got Unknown
		//IL_09d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_09df: Expected O, but got Unknown
		//IL_09eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f5: Expected O, but got Unknown
		//IL_0a4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a58: Expected O, but got Unknown
		//IL_0ad2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0adc: Expected O, but got Unknown
		//IL_0ff0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ffa: Expected O, but got Unknown
		//IL_93cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_93d6: Expected O, but got Unknown
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance166 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance171 = new Appearance();
		Appearance appearance172 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance180 = new Appearance();
		Appearance appearance181 = new Appearance();
		Appearance appearance182 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance186 = new Appearance();
		Appearance appearance197 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance192 = new Appearance();
		Appearance appearance193 = new Appearance();
		Appearance appearance194 = new Appearance();
		Appearance appearance195 = new Appearance();
		Appearance appearance196 = new Appearance();
		Appearance appearance198 = new Appearance();
		Appearance appearance199 = new Appearance();
		Appearance appearance200 = new Appearance();
		Appearance appearance201 = new Appearance();
		Appearance appearance202 = new Appearance();
		Appearance appearance203 = new Appearance();
		Appearance appearance204 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance157 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance183 = new Appearance();
		Appearance appearance184 = new Appearance();
		Appearance appearance185 = new Appearance();
		Appearance appearance187 = new Appearance();
		Appearance appearance188 = new Appearance();
		Appearance appearance189 = new Appearance();
		Appearance appearance190 = new Appearance();
		Appearance appearance191 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance168 = new Appearance();
		Appearance appearance169 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
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
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel5 = new System.Windows.Forms.Panel();
		this.grdDetail = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.label39 = new System.Windows.Forms.Label();
		this.txtGrossWeight = new System.Windows.Forms.TextBox();
		this.CmbJobLot = new UltraCombo();
		this.label55 = new System.Windows.Forms.Label();
		this.CmbWareHouse = new UltraCombo();
		this.label54 = new System.Windows.Forms.Label();
		this.btnDetailCancel = new System.Windows.Forms.Button();
		this.btnDetailUpdate = new System.Windows.Forms.Button();
		this.txtItemDesc = new System.Windows.Forms.TextBox();
		this.label20 = new System.Windows.Forms.Label();
		this.btnAddinGrid = new System.Windows.Forms.Button();
		this.cmbPackUOM = new UltraCombo();
		this.txtNetWeight = new System.Windows.Forms.TextBox();
		this.label30 = new System.Windows.Forms.Label();
		this.txtSeal = new System.Windows.Forms.TextBox();
		this.txtNoOfBags = new System.Windows.Forms.TextBox();
		this.txtContainerNo = new System.Windows.Forms.TextBox();
		this.label32 = new System.Windows.Forms.Label();
		this.label37 = new System.Windows.Forms.Label();
		this.label36 = new System.Windows.Forms.Label();
		this.label31 = new System.Windows.Forms.Label();
		this.cmbPackType = new UltraCombo();
		this.label29 = new System.Windows.Forms.Label();
		this.cmbItem = new UltraCombo();
		this.label28 = new System.Windows.Forms.Label();
		this.txtLcOrder = new UltraCombo();
		this.label27 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label61 = new System.Windows.Forms.Label();
		this.label60 = new System.Windows.Forms.Label();
		this.label59 = new System.Windows.Forms.Label();
		this.label58 = new System.Windows.Forms.Label();
		this.label57 = new System.Windows.Forms.Label();
		this.label56 = new System.Windows.Forms.Label();
		this.label53 = new System.Windows.Forms.Label();
		this.label52 = new System.Windows.Forms.Label();
		this.label48 = new System.Windows.Forms.Label();
		this.label47 = new System.Windows.Forms.Label();
		this.label46 = new System.Windows.Forms.Label();
		this.label45 = new System.Windows.Forms.Label();
		this.label42 = new System.Windows.Forms.Label();
		this.label41 = new System.Windows.Forms.Label();
		this.panel7 = new System.Windows.Forms.Panel();
		this.label40 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.label38 = new System.Windows.Forms.Label();
		this.txtDoNetWeight = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.txtDoGrossWeight = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.txtFactoryWeight = new System.Windows.Forms.TextBox();
		this.comproject = new UltraCombo();
		this.label35 = new System.Windows.Forms.Label();
		this.txtContainer2 = new System.Windows.Forms.TextBox();
		this.combranches = new UltraCombo();
		this.label34 = new System.Windows.Forms.Label();
		this.txtconainer1 = new System.Windows.Forms.TextBox();
		this.label33 = new System.Windows.Forms.Label();
		this.chkdateisactive = new System.Windows.Forms.CheckBox();
		this.cmbCustomer = new UltraCombo();
		this.cmbShippingAgent = new UltraCombo();
		this.txtShippedContainer = new System.Windows.Forms.TextBox();
		this.label25 = new System.Windows.Forms.Label();
		this.txtNoOfContainer = new System.Windows.Forms.TextBox();
		this.label16 = new System.Windows.Forms.Label();
		this.label24 = new System.Windows.Forms.Label();
		this.txtCompletedDate = new System.Windows.Forms.DateTimePicker();
		this.label26 = new System.Windows.Forms.Label();
		this.txtOtherCharges = new System.Windows.Forms.TextBox();
		this.cmbGpNo = new UltraCombo();
		this.cmbShippingLIne = new UltraCombo();
		this.txtFreight = new System.Windows.Forms.TextBox();
		this.label15 = new System.Windows.Forms.Label();
		this.cmbDestinationPort = new UltraCombo();
		this.label23 = new System.Windows.Forms.Label();
		this.txtCNICNO = new System.Windows.Forms.TextBox();
		this.cmbLoadingPort = new UltraCombo();
		this.label22 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.txtBilityNo = new System.Windows.Forms.TextBox();
		this.label11 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.txtDriverCellNo = new System.Windows.Forms.TextBox();
		this.label9 = new System.Windows.Forms.Label();
		this.cmbtransporter = new UltraCombo();
		this.txtDriverName = new System.Windows.Forms.TextBox();
		this.cmbLcOrderNo = new UltraCombo();
		this.txtVehicleNo = new System.Windows.Forms.TextBox();
		this.label8 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.txtGPDate = new System.Windows.Forms.DateTimePicker();
		this.DocDate = new System.Windows.Forms.DateTimePicker();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.cmbpo = new UltraCombo();
		this.panel2 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.BtnNew = new System.Windows.Forms.ToolStripButton();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel6 = new System.Windows.Forms.Panel();
		this.panel10 = new System.Windows.Forms.Panel();
		this.DataGridHistory = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel4.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWareHouse).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPackUOM).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPackType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.txtLcOrder).BeginInit();
		this.panel3.SuspendLayout();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.comproject).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combranches).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCustomer).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbShippingAgent).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbGpNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbShippingLIne).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDestinationPort).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbLoadingPort).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbtransporter).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbLcOrderNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbpo).BeginInit();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel10.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel8.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1251, 662);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1251, 662);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel5);
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1243, 636);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(3, 401);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1237, 232);
		this.panel5.TabIndex = 4;
		this.grdDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdDetail).BackColor = System.Drawing.Color.White;
		grdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).Office2007ColorScheme = (Office2007ColorScheme)4;
		((ScrollableBase)this.grdDetail).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 0);
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1237, 232);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 0;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.grdDetail.ColumnButtonClick += new ColumnActionEventHandler(grdDetail_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdDetail).DoubleClick += new System.EventHandler(grdDetail_DoubleClick);
		this.panel4.Controls.Add(this.groupBox3);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.txtLcOrder);
		this.panel4.Controls.Add(this.label27);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(3, 276);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1237, 125);
		this.panel4.TabIndex = 3;
		this.groupBox3.BackColor = System.Drawing.Color.White;
		this.groupBox3.Controls.Add(this.label39);
		this.groupBox3.Controls.Add(this.txtGrossWeight);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobLot);
		this.groupBox3.Controls.Add(this.label55);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWareHouse);
		this.groupBox3.Controls.Add(this.label54);
		this.groupBox3.Controls.Add(this.btnDetailCancel);
		this.groupBox3.Controls.Add(this.btnDetailUpdate);
		this.groupBox3.Controls.Add(this.txtItemDesc);
		this.groupBox3.Controls.Add(this.label20);
		this.groupBox3.Controls.Add(this.btnAddinGrid);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPackUOM);
		this.groupBox3.Controls.Add(this.txtNetWeight);
		this.groupBox3.Controls.Add(this.label30);
		this.groupBox3.Controls.Add(this.txtSeal);
		this.groupBox3.Controls.Add(this.txtNoOfBags);
		this.groupBox3.Controls.Add(this.txtContainerNo);
		this.groupBox3.Controls.Add(this.label32);
		this.groupBox3.Controls.Add(this.label37);
		this.groupBox3.Controls.Add(this.label36);
		this.groupBox3.Controls.Add(this.label31);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPackType);
		this.groupBox3.Controls.Add(this.label29);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItem);
		this.groupBox3.Controls.Add(this.label28);
		this.groupBox3.Location = new System.Drawing.Point(11, 5);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(1221, 118);
		this.groupBox3.TabIndex = 3;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Detail";
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label39.Location = new System.Drawing.Point(714, 17);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(80, 14);
		this.label39.TabIndex = 42;
		this.label39.Text = "Gross Weight";
		this.label39.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtGrossWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtGrossWeight.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGrossWeight.Location = new System.Drawing.Point(717, 36);
		this.txtGrossWeight.Name = "txtGrossWeight";
		this.txtGrossWeight.Size = new System.Drawing.Size(99, 22);
		this.txtGrossWeight.TabIndex = 3;
		this.CmbJobLot.AllowNull = (DefaultableBoolean)1;
		this.CmbJobLot.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobLot.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobLot.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance166).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance166).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Appearance = (AppearanceBase)(object)appearance166;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance170).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance170).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance170).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance170;
		((AppearanceBase)appearance171).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance171;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance172).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance172).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance172).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance172;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance173).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance173).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance173;
		((AppearanceBase)appearance174).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance174).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance174;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance176).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance176;
		((AppearanceBase)appearance177).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance177).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance177;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance178).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance178).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance178).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance178).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance178;
		((AppearanceBase)appearance31).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbJobLot.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Location = new System.Drawing.Point(489, 84);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Name = "CmbJobLot";
		this.CmbJobLot.ReadOnly = true;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Size = new System.Drawing.Size(172, 25);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).TabIndex = 10;
		this.label55.AutoSize = true;
		this.label55.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label55.Location = new System.Drawing.Point(487, 69);
		this.label55.Name = "label55";
		this.label55.Size = new System.Drawing.Size(41, 14);
		this.label55.TabIndex = 40;
		this.label55.Text = "JobLot";
		this.label55.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.CmbWareHouse.AllowNull = (DefaultableBoolean)1;
		this.CmbWareHouse.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWareHouse.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWareHouse.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance180).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance180).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Appearance = (AppearanceBase)(object)appearance180;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance181).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance181).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance181).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance181).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWareHouse).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance181;
		((AppearanceBase)appearance182).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance182;
		((SpecialBoxBase)((UltraGridBase)this.CmbWareHouse).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance101).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance101).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance101).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance102).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance102;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance104).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance105;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance106).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance107).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance107).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance107).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance107).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance107;
		((AppearanceBase)appearance108).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance109).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWareHouse.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).Location = new System.Drawing.Point(16, 83);
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).Name = "CmbWareHouse";
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).Size = new System.Drawing.Size(180, 25);
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).TabIndex = 7;
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label54.Location = new System.Drawing.Point(15, 70);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(71, 14);
		this.label54.TabIndex = 38;
		this.label54.Text = "WareHouse";
		this.label54.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnDetailCancel.AutoSize = true;
		this.btnDetailCancel.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnDetailCancel.Location = new System.Drawing.Point(662, 88);
		this.btnDetailCancel.Name = "btnDetailCancel";
		this.btnDetailCancel.Size = new System.Drawing.Size(66, 24);
		this.btnDetailCancel.TabIndex = 11;
		this.btnDetailCancel.Text = "Cancel";
		this.btnDetailCancel.UseVisualStyleBackColor = true;
		this.btnDetailCancel.Click += new System.EventHandler(btnDetailCancel_Click);
		this.btnDetailUpdate.AutoSize = true;
		this.btnDetailUpdate.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnDetailUpdate.Location = new System.Drawing.Point(662, 67);
		this.btnDetailUpdate.Name = "btnDetailUpdate";
		this.btnDetailUpdate.Size = new System.Drawing.Size(66, 24);
		this.btnDetailUpdate.TabIndex = 12;
		this.btnDetailUpdate.Text = "Update";
		this.btnDetailUpdate.UseVisualStyleBackColor = true;
		this.btnDetailUpdate.Click += new System.EventHandler(btnDetailUpdate_Click);
		this.txtItemDesc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemDesc.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemDesc.Location = new System.Drawing.Point(408, 36);
		this.txtItemDesc.Name = "txtItemDesc";
		this.txtItemDesc.Size = new System.Drawing.Size(308, 22);
		this.txtItemDesc.TabIndex = 2;
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(408, 16);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(97, 14);
		this.label20.TabIndex = 34;
		this.label20.Text = "Item Description";
		this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.btnAddinGrid.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAddinGrid.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnAddinGrid.Location = new System.Drawing.Point(662, 84);
		this.btnAddinGrid.Name = "btnAddinGrid";
		this.btnAddinGrid.Size = new System.Drawing.Size(23, 25);
		this.btnAddinGrid.TabIndex = 11;
		this.btnAddinGrid.UseVisualStyleBackColor = true;
		this.btnAddinGrid.Click += new System.EventHandler(btnAddinGrid_Click);
		this.cmbPackUOM.AllowNull = (DefaultableBoolean)1;
		this.cmbPackUOM.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPackUOM.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPackUOM.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance111).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.Appearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPackUOM).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance113).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance113;
		((SpecialBoxBase)((UltraGridBase)this.cmbPackUOM).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance115).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance115).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance115).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance116).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance116;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance117).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance118;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance119).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance120).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance120).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance120).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance120).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance121).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance122).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPackUOM).Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbPackUOM.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPackUOM).Location = new System.Drawing.Point(919, 35);
		((System.Windows.Forms.Control)(object)this.cmbPackUOM).Name = "cmbPackUOM";
		this.cmbPackUOM.ReadOnly = true;
		((System.Windows.Forms.Control)(object)this.cmbPackUOM).Size = new System.Drawing.Size(90, 25);
		((System.Windows.Forms.Control)(object)this.cmbPackUOM).TabIndex = 5;
		((System.Windows.Forms.Control)(object)this.cmbPackUOM).TextChanged += new System.EventHandler(cmbOuterUOM_ValueChanged);
		this.txtNetWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNetWeight.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNetWeight.Location = new System.Drawing.Point(818, 36);
		this.txtNetWeight.Name = "txtNetWeight";
		this.txtNetWeight.Size = new System.Drawing.Size(99, 22);
		this.txtNetWeight.TabIndex = 4;
		this.txtNetWeight.TextChanged += new System.EventHandler(cmbOuterUOM_ValueChanged);
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(818, 16);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(38, 14);
		this.label30.TabIndex = 6;
		this.label30.Text = "M.Ton";
		this.label30.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtSeal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSeal.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSeal.Location = new System.Drawing.Point(335, 85);
		this.txtSeal.Name = "txtSeal";
		this.txtSeal.Size = new System.Drawing.Size(153, 22);
		this.txtSeal.TabIndex = 9;
		this.txtNoOfBags.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNoOfBags.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNoOfBags.Location = new System.Drawing.Point(1011, 36);
		this.txtNoOfBags.Name = "txtNoOfBags";
		this.txtNoOfBags.Size = new System.Drawing.Size(103, 22);
		this.txtNoOfBags.TabIndex = 6;
		this.txtContainerNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtContainerNo.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtContainerNo.Location = new System.Drawing.Point(197, 85);
		this.txtContainerNo.Name = "txtContainerNo";
		this.txtContainerNo.ReadOnly = true;
		this.txtContainerNo.Size = new System.Drawing.Size(134, 22);
		this.txtContainerNo.TabIndex = 8;
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.Location = new System.Drawing.Point(1011, 16);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(65, 14);
		this.label32.TabIndex = 18;
		this.label32.Text = "No of Bags";
		this.label32.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(335, 70);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(40, 14);
		this.label37.TabIndex = 20;
		this.label37.Text = "Seal #";
		this.label37.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(197, 70);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(69, 14);
		this.label36.TabIndex = 18;
		this.label36.Text = "Container #";
		this.label36.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.Location = new System.Drawing.Point(919, 14);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(60, 14);
		this.label31.TabIndex = 8;
		this.label31.Text = "Pack UOM";
		this.label31.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.cmbPackType.AllowNull = (DefaultableBoolean)1;
		this.cmbPackType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPackType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPackType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance139).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance139;
		((UltraGridBase)this.cmbPackType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPackType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance140).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance140).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPackType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance140;
		((AppearanceBase)appearance141).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPackType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance141;
		((SpecialBoxBase)((UltraGridBase)this.cmbPackType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance143).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance143).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance143).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPackType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.cmbPackType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPackType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance144).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance144;
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance145).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance146;
		((AppearanceBase)appearance147).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance147).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance147;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance148).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance148).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance148).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance148).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance148;
		((AppearanceBase)appearance149).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance149;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance150).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance150;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.cmbPackType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPackType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPackType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPackType).Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbPackType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPackType).Location = new System.Drawing.Point(272, 35);
		((System.Windows.Forms.Control)(object)this.cmbPackType).Name = "cmbPackType";
		this.cmbPackType.ReadOnly = true;
		((System.Windows.Forms.Control)(object)this.cmbPackType).Size = new System.Drawing.Size(134, 25);
		((System.Windows.Forms.Control)(object)this.cmbPackType).TabIndex = 2;
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.Location = new System.Drawing.Point(272, 16);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(55, 14);
		this.label29.TabIndex = 4;
		this.label29.Text = "PackType";
		this.label29.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.cmbItem.AllowNull = (DefaultableBoolean)1;
		this.cmbItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.cmbItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance49).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance49).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance49).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance50).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance50;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance51).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance51).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance51).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance52;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance53).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance54;
		((AppearanceBase)appearance55).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance55).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance56).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance56).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance56).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance56).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance57).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItem).Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItem).Location = new System.Drawing.Point(13, 35);
		((System.Windows.Forms.Control)(object)this.cmbItem).Name = "cmbItem";
		this.cmbItem.ReadOnly = true;
		((System.Windows.Forms.Control)(object)this.cmbItem).Size = new System.Drawing.Size(257, 25);
		((System.Windows.Forms.Control)(object)this.cmbItem).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbItem).Leave += new System.EventHandler(cmbItem_Leave);
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.Location = new System.Drawing.Point(13, 16);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(32, 14);
		this.label28.TabIndex = 2;
		this.label28.Text = "Item";
		this.label28.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtLcOrder.AllowNull = (DefaultableBoolean)1;
		this.txtLcOrder.AutoCompleteMode = (AutoCompleteMode)4;
		this.txtLcOrder.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.txtLcOrder.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.txtLcOrder).DisplayLayout.Appearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.txtLcOrder).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.txtLcOrder).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance35).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance35).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.txtLcOrder).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance35;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.txtLcOrder).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance37;
		((SpecialBoxBase)((UltraGridBase)this.txtLcOrder).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance38).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance38).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance38).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.txtLcOrder).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.txtLcOrder).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.txtLcOrder).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.txtLcOrder).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance39;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance40).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.txtLcOrder).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.txtLcOrder).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.txtLcOrder).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.txtLcOrder).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance42).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.txtLcOrder).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.txtLcOrder).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.txtLcOrder).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance43).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance43).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance43).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance43).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.txtLcOrder).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).TextHAlignAsString = "Left";
		((UltraGridBase)this.txtLcOrder).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.txtLcOrder).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.txtLcOrder).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.txtLcOrder).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.txtLcOrder).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.txtLcOrder).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.txtLcOrder).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.txtLcOrder).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.txtLcOrder).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.txtLcOrder).Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtLcOrder.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.txtLcOrder).Location = new System.Drawing.Point(1066, 180);
		((System.Windows.Forms.Control)(object)this.txtLcOrder).Name = "txtLcOrder";
		((System.Windows.Forms.Control)(object)this.txtLcOrder).Size = new System.Drawing.Size(62, 25);
		((System.Windows.Forms.Control)(object)this.txtLcOrder).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.txtLcOrder).Visible = false;
		this.label27.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(1066, 155);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(62, 17);
		this.label27.TabIndex = 0;
		this.label27.Text = "LC Order #";
		this.label27.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label27.Visible = false;
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add(this.label61);
		this.panel3.Controls.Add(this.label60);
		this.panel3.Controls.Add(this.label59);
		this.panel3.Controls.Add(this.label58);
		this.panel3.Controls.Add(this.label57);
		this.panel3.Controls.Add(this.label56);
		this.panel3.Controls.Add(this.label53);
		this.panel3.Controls.Add(this.label52);
		this.panel3.Controls.Add(this.label48);
		this.panel3.Controls.Add(this.label47);
		this.panel3.Controls.Add(this.label46);
		this.panel3.Controls.Add(this.label45);
		this.panel3.Controls.Add(this.label42);
		this.panel3.Controls.Add(this.label41);
		this.panel3.Controls.Add(this.panel7);
		this.panel3.Controls.Add(this.label3);
		this.panel3.Controls.Add(this.label18);
		this.panel3.Controls.Add(this.label5);
		this.panel3.Controls.Add(this.label17);
		this.panel3.Controls.Add(this.label13);
		this.panel3.Controls.Add(this.label38);
		this.panel3.Controls.Add(this.txtDoNetWeight);
		this.panel3.Controls.Add(this.label2);
		this.panel3.Controls.Add(this.txtDoGrossWeight);
		this.panel3.Controls.Add(this.label1);
		this.panel3.Controls.Add(this.txtFactoryWeight);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.comproject);
		this.panel3.Controls.Add(this.label35);
		this.panel3.Controls.Add(this.txtContainer2);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.combranches);
		this.panel3.Controls.Add(this.label34);
		this.panel3.Controls.Add(this.txtconainer1);
		this.panel3.Controls.Add(this.label33);
		this.panel3.Controls.Add(this.chkdateisactive);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCustomer);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbShippingAgent);
		this.panel3.Controls.Add(this.txtShippedContainer);
		this.panel3.Controls.Add(this.label25);
		this.panel3.Controls.Add(this.txtNoOfContainer);
		this.panel3.Controls.Add(this.label16);
		this.panel3.Controls.Add(this.label24);
		this.panel3.Controls.Add(this.txtCompletedDate);
		this.panel3.Controls.Add(this.label26);
		this.panel3.Controls.Add(this.txtOtherCharges);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbGpNo);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbShippingLIne);
		this.panel3.Controls.Add(this.txtFreight);
		this.panel3.Controls.Add(this.label15);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDestinationPort);
		this.panel3.Controls.Add(this.label23);
		this.panel3.Controls.Add(this.txtCNICNO);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbLoadingPort);
		this.panel3.Controls.Add(this.label22);
		this.panel3.Controls.Add(this.label21);
		this.panel3.Controls.Add(this.label12);
		this.panel3.Controls.Add(this.label19);
		this.panel3.Controls.Add(this.label14);
		this.panel3.Controls.Add(this.txtBilityNo);
		this.panel3.Controls.Add(this.label11);
		this.panel3.Controls.Add(this.label10);
		this.panel3.Controls.Add(this.txtDriverCellNo);
		this.panel3.Controls.Add(this.label9);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbtransporter);
		this.panel3.Controls.Add(this.txtDriverName);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbLcOrderNo);
		this.panel3.Controls.Add(this.txtVehicleNo);
		this.panel3.Controls.Add(this.label8);
		this.panel3.Controls.Add(this.label7);
		this.panel3.Controls.Add(this.label6);
		this.panel3.Controls.Add(this.label4);
		this.panel3.Controls.Add(this.txtGPDate);
		this.panel3.Controls.Add(this.DocDate);
		this.panel3.Controls.Add(this.txtdocno);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbpo);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.panel3.Location = new System.Drawing.Point(3, 33);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1237, 243);
		this.panel3.TabIndex = 2;
		this.label61.AutoSize = true;
		this.label61.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label61.ForeColor = System.Drawing.Color.Red;
		this.label61.Location = new System.Drawing.Point(371, 74);
		this.label61.Name = "label61";
		this.label61.Size = new System.Drawing.Size(14, 17);
		this.label61.TabIndex = 8;
		this.label61.Text = "*";
		this.label60.AutoSize = true;
		this.label60.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label60.ForeColor = System.Drawing.Color.Red;
		this.label60.Location = new System.Drawing.Point(370, 100);
		this.label60.Name = "label60";
		this.label60.Size = new System.Drawing.Size(14, 17);
		this.label60.TabIndex = 9;
		this.label60.Text = "*";
		this.label59.AutoSize = true;
		this.label59.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label59.ForeColor = System.Drawing.Color.Red;
		this.label59.Location = new System.Drawing.Point(370, 48);
		this.label59.Name = "label59";
		this.label59.Size = new System.Drawing.Size(14, 17);
		this.label59.TabIndex = 7;
		this.label59.Text = "*";
		this.label58.AutoSize = true;
		this.label58.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label58.ForeColor = System.Drawing.Color.Red;
		this.label58.Location = new System.Drawing.Point(86, 211);
		this.label58.Name = "label58";
		this.label58.Size = new System.Drawing.Size(14, 17);
		this.label58.TabIndex = 6;
		this.label58.Text = "*";
		this.label57.AutoSize = true;
		this.label57.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label57.ForeColor = System.Drawing.Color.Red;
		this.label57.Location = new System.Drawing.Point(86, 184);
		this.label57.Name = "label57";
		this.label57.Size = new System.Drawing.Size(14, 17);
		this.label57.TabIndex = 5;
		this.label57.Text = "*";
		this.label56.AutoSize = true;
		this.label56.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label56.ForeColor = System.Drawing.Color.Red;
		this.label56.Location = new System.Drawing.Point(86, 129);
		this.label56.Name = "label56";
		this.label56.Size = new System.Drawing.Size(14, 17);
		this.label56.TabIndex = 3;
		this.label56.Text = "*";
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label53.ForeColor = System.Drawing.Color.Red;
		this.label53.Location = new System.Drawing.Point(86, 103);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(14, 17);
		this.label53.TabIndex = 2;
		this.label53.Text = "*";
		this.label52.AutoSize = true;
		this.label52.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label52.ForeColor = System.Drawing.Color.Red;
		this.label52.Location = new System.Drawing.Point(86, 75);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(14, 17);
		this.label52.TabIndex = 1;
		this.label52.Text = "*";
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.ForeColor = System.Drawing.Color.Red;
		this.label48.Location = new System.Drawing.Point(963, 185);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(14, 17);
		this.label48.TabIndex = 24;
		this.label48.Text = "*";
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label47.ForeColor = System.Drawing.Color.Red;
		this.label47.Location = new System.Drawing.Point(966, 129);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(14, 17);
		this.label47.TabIndex = 22;
		this.label47.Text = "*";
		this.label46.AutoSize = true;
		this.label46.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label46.ForeColor = System.Drawing.Color.Red;
		this.label46.Location = new System.Drawing.Point(952, 75);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(14, 17);
		this.label46.TabIndex = 518;
		this.label46.Text = "*";
		this.label45.AutoSize = true;
		this.label45.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label45.ForeColor = System.Drawing.Color.Red;
		this.label45.Location = new System.Drawing.Point(964, 48);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(14, 17);
		this.label45.TabIndex = 19;
		this.label45.Text = "*";
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label42.ForeColor = System.Drawing.Color.Red;
		this.label42.Location = new System.Drawing.Point(56, 155);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(14, 17);
		this.label42.TabIndex = 514;
		this.label42.Text = "*";
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label41.ForeColor = System.Drawing.Color.Red;
		this.label41.Location = new System.Drawing.Point(86, 48);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(14, 17);
		this.label41.TabIndex = 0;
		this.label41.Text = "*";
		this.panel7.BackColor = System.Drawing.Color.FromArgb(0, 192, 0);
		this.panel7.Controls.Add(this.label40);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1237, 37);
		this.panel7.TabIndex = 100;
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.ForeColor = System.Drawing.Color.White;
		this.label40.Location = new System.Drawing.Point(17, 6);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(333, 24);
		this.label40.TabIndex = 25;
		this.label40.Text = "Import :Goods Receipts Notes (GRN)   ";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(6, 184);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(76, 15);
		this.label3.TabIndex = 99;
		this.label3.Text = "Shipment No";
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(6, 213);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(82, 15);
		this.label18.TabIndex = 98;
		this.label18.Text = "I. Contract No";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(6, 130);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(46, 15);
		this.label5.TabIndex = 97;
		this.label5.Text = "Doc No";
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(6, 104);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(56, 15);
		this.label17.TabIndex = 96;
		this.label17.Text = "Doc Date";
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(6, 76);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(46, 15);
		this.label13.TabIndex = 95;
		this.label13.Text = "Project";
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.Location = new System.Drawing.Point(6, 49);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(46, 15);
		this.label38.TabIndex = 94;
		this.label38.Text = "Branch";
		this.txtDoNetWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDoNetWeight.Enabled = false;
		this.txtDoNetWeight.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDoNetWeight.Location = new System.Drawing.Point(389, 178);
		this.txtDoNetWeight.Name = "txtDoNetWeight";
		this.txtDoNetWeight.Size = new System.Drawing.Size(173, 22);
		this.txtDoNetWeight.TabIndex = 12;
		this.label2.AutoSize = true;
		this.label2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.label2.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(285, 182);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(84, 14);
		this.label2.TabIndex = 93;
		this.label2.Text = "Po Net Weight";
		this.txtDoGrossWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDoGrossWeight.Enabled = false;
		this.txtDoGrossWeight.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDoGrossWeight.Location = new System.Drawing.Point(389, 151);
		this.txtDoGrossWeight.Name = "txtDoGrossWeight";
		this.txtDoGrossWeight.Size = new System.Drawing.Size(173, 22);
		this.txtDoGrossWeight.TabIndex = 11;
		this.label1.AutoSize = true;
		this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.label1.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(285, 155);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(96, 14);
		this.label1.TabIndex = 91;
		this.label1.Text = "Po Gross Weight";
		this.txtFactoryWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFactoryWeight.Enabled = false;
		this.txtFactoryWeight.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFactoryWeight.Location = new System.Drawing.Point(389, 207);
		this.txtFactoryWeight.Name = "txtFactoryWeight";
		this.txtFactoryWeight.Size = new System.Drawing.Size(173, 22);
		this.txtFactoryWeight.TabIndex = 13;
		this.comproject.AllowNull = (DefaultableBoolean)1;
		this.comproject.AutoCompleteMode = (AutoCompleteMode)4;
		this.comproject.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comproject.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comproject).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.comproject).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comproject).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance91).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance91).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance91).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comproject).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance91;
		((AppearanceBase)appearance131).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comproject).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance131;
		((SpecialBoxBase)((UltraGridBase)this.comproject).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance142).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance142).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance142).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comproject).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.comproject).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comproject).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance153).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comproject).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance153;
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance164).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comproject).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.comproject).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comproject).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance175;
		((AppearanceBase)appearance186).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance186).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance186;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance197).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance197).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance197).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance197).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance197).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comproject).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance197;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comproject).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.comproject).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comproject).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.comproject).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comproject).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comproject).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		this.comproject.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comproject).Location = new System.Drawing.Point(106, 71);
		((System.Windows.Forms.Control)(object)this.comproject).Name = "comproject";
		((System.Windows.Forms.Control)(object)this.comproject).Size = new System.Drawing.Size(171, 25);
		((System.Windows.Forms.Control)(object)this.comproject).TabIndex = 1;
		this.label35.AutoSize = true;
		this.label35.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.label35.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(285, 211);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(87, 14);
		this.label35.TabIndex = 89;
		this.label35.Text = "Factory Weight";
		this.txtContainer2.BackColor = System.Drawing.Color.White;
		this.txtContainer2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtContainer2.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtContainer2.Location = new System.Drawing.Point(984, 211);
		this.txtContainer2.Name = "txtContainer2";
		this.txtContainer2.ReadOnly = true;
		this.txtContainer2.Size = new System.Drawing.Size(146, 22);
		this.txtContainer2.TabIndex = 25;
		this.txtContainer2.TabStop = false;
		this.combranches.AllowNull = (DefaultableBoolean)1;
		this.combranches.AutoCompleteMode = (AutoCompleteMode)4;
		this.combranches.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combranches.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance192).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance192).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combranches).DisplayLayout.Appearance = (AppearanceBase)(object)appearance192;
		((UltraGridBase)this.combranches).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combranches).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance193).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance193).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance193).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance193).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combranches).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance193;
		((AppearanceBase)appearance194).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combranches).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance194;
		((SpecialBoxBase)((UltraGridBase)this.combranches).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance195).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance195).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance195).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance195).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combranches).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance195;
		((UltraGridBase)this.combranches).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combranches).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance196).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance196).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combranches).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance196;
		((AppearanceBase)appearance198).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance198).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combranches).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance198;
		((UltraGridBase)this.combranches).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combranches).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance199).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance199;
		((AppearanceBase)appearance200).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance200).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance200;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance201).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance201).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance201).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance201).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance201).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combranches).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance201;
		((AppearanceBase)appearance202).TextHAlignAsString = "Left";
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance202;
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance203).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance203).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combranches).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance203;
		((UltraGridBase)this.combranches).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance204).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combranches).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance204;
		((UltraGridBase)this.combranches).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combranches).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combranches).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		this.combranches.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combranches).Location = new System.Drawing.Point(106, 44);
		((System.Windows.Forms.Control)(object)this.combranches).Name = "combranches";
		((System.Windows.Forms.Control)(object)this.combranches).Size = new System.Drawing.Size(171, 25);
		((System.Windows.Forms.Control)(object)this.combranches).TabIndex = 0;
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.Location = new System.Drawing.Point(872, 215);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(85, 14);
		this.label34.TabIndex = 87;
		this.label34.Text = "ContainerNo-2";
		this.txtconainer1.BackColor = System.Drawing.Color.White;
		this.txtconainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtconainer1.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtconainer1.Location = new System.Drawing.Point(984, 182);
		this.txtconainer1.Name = "txtconainer1";
		this.txtconainer1.ReadOnly = true;
		this.txtconainer1.Size = new System.Drawing.Size(146, 22);
		this.txtconainer1.TabIndex = 25;
		this.txtconainer1.TabStop = false;
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(872, 186);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(85, 14);
		this.label33.TabIndex = 85;
		this.label33.Text = "ContainerNo-1";
		this.chkdateisactive.AutoSize = true;
		this.chkdateisactive.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.chkdateisactive.Location = new System.Drawing.Point(1136, 155);
		this.chkdateisactive.Name = "chkdateisactive";
		this.chkdateisactive.Size = new System.Drawing.Size(86, 18);
		this.chkdateisactive.TabIndex = 26;
		this.chkdateisactive.Text = "Date Active";
		this.chkdateisactive.UseVisualStyleBackColor = true;
		this.chkdateisactive.CheckedChanged += new System.EventHandler(chkdateisactive_CheckedChanged);
		this.cmbCustomer.AllowNull = (DefaultableBoolean)1;
		this.cmbCustomer.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCustomer.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCustomer.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance152).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.Appearance = (AppearanceBase)(object)appearance152;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance154).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance154).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance154).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCustomer).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance154;
		((AppearanceBase)appearance155).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance155;
		((SpecialBoxBase)((UltraGridBase)this.cmbCustomer).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance156).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance156).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance156).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance156;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance157).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance157;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance158).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance158;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance159;
		((AppearanceBase)appearance160).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance160).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance160;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance161).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance161).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance161).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance161).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance161).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance161;
		((AppearanceBase)appearance162).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCustomer).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance162;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance163).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance163;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance165;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCustomer).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCustomer).Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbCustomer.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCustomer).Location = new System.Drawing.Point(389, 44);
		((System.Windows.Forms.Control)(object)this.cmbCustomer).Name = "cmbCustomer";
		((System.Windows.Forms.Control)(object)this.cmbCustomer).Size = new System.Drawing.Size(478, 25);
		((System.Windows.Forms.Control)(object)this.cmbCustomer).TabIndex = 6;
		this.cmbShippingAgent.AllowNull = (DefaultableBoolean)1;
		this.cmbShippingAgent.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbShippingAgent.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbShippingAgent.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbShippingAgent).DisplayLayout.Appearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.cmbShippingAgent).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbShippingAgent).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance72).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance72).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbShippingAgent).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbShippingAgent).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance73;
		((SpecialBoxBase)((UltraGridBase)this.cmbShippingAgent).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbShippingAgent).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.cmbShippingAgent).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbShippingAgent).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance75).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbShippingAgent).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance75;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance76).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbShippingAgent).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.cmbShippingAgent).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbShippingAgent).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbShippingAgent).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance77;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance78).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbShippingAgent).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.cmbShippingAgent).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbShippingAgent).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance79).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance79).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance79).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance79).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbShippingAgent).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance81).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbShippingAgent).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.cmbShippingAgent).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbShippingAgent).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance82).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbShippingAgent).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.cmbShippingAgent).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbShippingAgent).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.cmbShippingAgent).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbShippingAgent).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbShippingAgent).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbShippingAgent).Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbShippingAgent.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbShippingAgent).Location = new System.Drawing.Point(1284, 104);
		((System.Windows.Forms.Control)(object)this.cmbShippingAgent).Name = "cmbShippingAgent";
		((System.Windows.Forms.Control)(object)this.cmbShippingAgent).Size = new System.Drawing.Size(207, 25);
		((System.Windows.Forms.Control)(object)this.cmbShippingAgent).TabIndex = 8;
		((System.Windows.Forms.Control)(object)this.cmbShippingAgent).TabStop = false;
		((System.Windows.Forms.Control)(object)this.cmbShippingAgent).Visible = false;
		this.txtShippedContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtShippedContainer.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtShippedContainer.Location = new System.Drawing.Point(984, 100);
		this.txtShippedContainer.Name = "txtShippedContainer";
		this.txtShippedContainer.Size = new System.Drawing.Size(146, 22);
		this.txtShippedContainer.TabIndex = 21;
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(872, 104);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(108, 14);
		this.label25.TabIndex = 58;
		this.label25.Text = "Shipped Container";
		this.txtNoOfContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNoOfContainer.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNoOfContainer.Location = new System.Drawing.Point(984, 126);
		this.txtNoOfContainer.Name = "txtNoOfContainer";
		this.txtNoOfContainer.Size = new System.Drawing.Size(146, 22);
		this.txtNoOfContainer.TabIndex = 22;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(1287, 44);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(81, 14);
		this.label16.TabIndex = 50;
		this.label16.Text = "Shipping Line";
		this.label16.Visible = false;
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(872, 130);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(96, 14);
		this.label24.TabIndex = 55;
		this.label24.Text = "No. Of Container";
		this.txtCompletedDate.CustomFormat = "dd-MMM-yy";
		this.txtCompletedDate.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCompletedDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtCompletedDate.Location = new System.Drawing.Point(984, 153);
		this.txtCompletedDate.Name = "txtCompletedDate";
		this.txtCompletedDate.Size = new System.Drawing.Size(146, 22);
		this.txtCompletedDate.TabIndex = 23;
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(872, 157);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(95, 14);
		this.label26.TabIndex = 52;
		this.label26.Text = "Completed Date";
		this.txtOtherCharges.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOtherCharges.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOtherCharges.Location = new System.Drawing.Point(694, 182);
		this.txtOtherCharges.Name = "txtOtherCharges";
		this.txtOtherCharges.Size = new System.Drawing.Size(173, 22);
		this.txtOtherCharges.TabIndex = 18;
		this.txtOtherCharges.TabStop = false;
		this.txtOtherCharges.Visible = false;
		this.cmbGpNo.AllowNull = (DefaultableBoolean)1;
		this.cmbGpNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbGpNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbGpNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance84).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbGpNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.cmbGpNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbGpNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance85).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance85).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance85).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbGpNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance86).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbGpNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance86;
		((SpecialBoxBase)((UltraGridBase)this.cmbGpNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance87).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance87).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance87).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbGpNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.cmbGpNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbGpNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance88).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbGpNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance88;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance89).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbGpNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.cmbGpNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbGpNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbGpNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance90;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance93).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbGpNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance93;
		((UltraGridBase)this.cmbGpNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbGpNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance94).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance94).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance94).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance94).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbGpNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance94;
		((AppearanceBase)appearance95).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbGpNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.cmbGpNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbGpNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance96).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbGpNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.cmbGpNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbGpNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.cmbGpNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbGpNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbGpNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbGpNo).Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbGpNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbGpNo).Location = new System.Drawing.Point(104, 151);
		((System.Windows.Forms.Control)(object)this.cmbGpNo).Name = "cmbGpNo";
		((System.Windows.Forms.Control)(object)this.cmbGpNo).Size = new System.Drawing.Size(173, 25);
		((System.Windows.Forms.Control)(object)this.cmbGpNo).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.cmbGpNo).Leave += new System.EventHandler(cmbGpNo_TextChanged);
		this.cmbShippingLIne.AllowNull = (DefaultableBoolean)1;
		this.cmbShippingLIne.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbShippingLIne.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbShippingLIne.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance179).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbShippingLIne).DisplayLayout.Appearance = (AppearanceBase)(object)appearance179;
		((UltraGridBase)this.cmbShippingLIne).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbShippingLIne).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance183).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance183).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance183).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance183).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbShippingLIne).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance183;
		((AppearanceBase)appearance184).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbShippingLIne).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance184;
		((SpecialBoxBase)((UltraGridBase)this.cmbShippingLIne).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance185).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance185).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance185).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance185).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbShippingLIne).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance185;
		((UltraGridBase)this.cmbShippingLIne).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbShippingLIne).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance187).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance187).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbShippingLIne).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance187;
		((AppearanceBase)appearance188).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance188).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbShippingLIne).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance188;
		((UltraGridBase)this.cmbShippingLIne).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbShippingLIne).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance189).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbShippingLIne).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance189;
		((AppearanceBase)appearance190).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance190).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbShippingLIne).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance190;
		((UltraGridBase)this.cmbShippingLIne).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbShippingLIne).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance191).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance191).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance191).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance191).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance191).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbShippingLIne).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance191;
		((AppearanceBase)appearance17).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbShippingLIne).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmbShippingLIne).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbShippingLIne).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbShippingLIne).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbShippingLIne).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbShippingLIne).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.cmbShippingLIne).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbShippingLIne).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbShippingLIne).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbShippingLIne).Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbShippingLIne.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbShippingLIne).Location = new System.Drawing.Point(1290, 57);
		((System.Windows.Forms.Control)(object)this.cmbShippingLIne).Name = "cmbShippingLIne";
		((System.Windows.Forms.Control)(object)this.cmbShippingLIne).Size = new System.Drawing.Size(207, 25);
		((System.Windows.Forms.Control)(object)this.cmbShippingLIne).TabIndex = 7;
		((System.Windows.Forms.Control)(object)this.cmbShippingLIne).TabStop = false;
		((System.Windows.Forms.Control)(object)this.cmbShippingLIne).Visible = false;
		this.txtFreight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFreight.Enabled = false;
		this.txtFreight.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFreight.Location = new System.Drawing.Point(694, 153);
		this.txtFreight.Name = "txtFreight";
		this.txtFreight.Size = new System.Drawing.Size(173, 22);
		this.txtFreight.TabIndex = 17;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(573, 186);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(83, 14);
		this.label15.TabIndex = 46;
		this.label15.Text = "Other Charges";
		this.label15.Visible = false;
		this.cmbDestinationPort.AllowNull = (DefaultableBoolean)1;
		this.cmbDestinationPort.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDestinationPort.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDestinationPort.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Appearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDestinationPort).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance22;
		((SpecialBoxBase)((UltraGridBase)this.cmbDestinationPort).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance23).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance23).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance25).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance28).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance29).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance29;
		((AppearanceBase)appearance98).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance98;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance100;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDestinationPort).Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDestinationPort.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDestinationPort).Location = new System.Drawing.Point(984, 44);
		((System.Windows.Forms.Control)(object)this.cmbDestinationPort).Name = "cmbDestinationPort";
		((System.Windows.Forms.Control)(object)this.cmbDestinationPort).Size = new System.Drawing.Size(146, 25);
		((System.Windows.Forms.Control)(object)this.cmbDestinationPort).TabIndex = 19;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.Location = new System.Drawing.Point(872, 49);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(94, 14);
		this.label23.TabIndex = 47;
		this.label23.Text = "Destination Port";
		this.txtCNICNO.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCNICNO.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCNICNO.Location = new System.Drawing.Point(389, 125);
		this.txtCNICNO.Name = "txtCNICNO";
		this.txtCNICNO.Size = new System.Drawing.Size(173, 22);
		this.txtCNICNO.TabIndex = 10;
		this.cmbLoadingPort.AllowNull = (DefaultableBoolean)1;
		this.cmbLoadingPort.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbLoadingPort.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbLoadingPort.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance126).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Appearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance127).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance127).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbLoadingPort).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance127;
		((AppearanceBase)appearance128).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance128;
		((SpecialBoxBase)((UltraGridBase)this.cmbLoadingPort).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance129).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance129).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance129).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance130).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance130;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance132).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance133;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance134).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance134;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance135).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance135).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance135).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance135).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance135;
		((AppearanceBase)appearance136).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance137).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance137;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbLoadingPort).Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbLoadingPort.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbLoadingPort).Location = new System.Drawing.Point(984, 71);
		((System.Windows.Forms.Control)(object)this.cmbLoadingPort).Name = "cmbLoadingPort";
		((System.Windows.Forms.Control)(object)this.cmbLoadingPort).Size = new System.Drawing.Size(146, 25);
		((System.Windows.Forms.Control)(object)this.cmbLoadingPort).TabIndex = 20;
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(872, 76);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(74, 14);
		this.label22.TabIndex = 41;
		this.label22.Text = "Loading Port";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(285, 49);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(58, 14);
		this.label21.TabIndex = 36;
		this.label21.Text = "Customer";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(285, 129);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(49, 14);
		this.label12.TabIndex = 37;
		this.label12.Text = "CNIC No";
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(1287, 85);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(89, 14);
		this.label19.TabIndex = 26;
		this.label19.Text = "Shipping Agent";
		this.label19.Visible = false;
		this.label14.AutoSize = true;
		this.label14.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.label14.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(573, 157);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(45, 14);
		this.label14.TabIndex = 40;
		this.label14.Text = "Freight";
		this.txtBilityNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBilityNo.Enabled = false;
		this.txtBilityNo.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBilityNo.Location = new System.Drawing.Point(694, 126);
		this.txtBilityNo.Name = "txtBilityNo";
		this.txtBilityNo.Size = new System.Drawing.Size(173, 22);
		this.txtBilityNo.TabIndex = 16;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(285, 75);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(81, 14);
		this.label11.TabIndex = 32;
		this.label11.Text = "Driver Cell No";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(285, 101);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(74, 14);
		this.label10.TabIndex = 25;
		this.label10.Text = "Driver Name";
		this.txtDriverCellNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDriverCellNo.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDriverCellNo.Location = new System.Drawing.Point(389, 72);
		this.txtDriverCellNo.Name = "txtDriverCellNo";
		this.txtDriverCellNo.Size = new System.Drawing.Size(173, 22);
		this.txtDriverCellNo.TabIndex = 10;
		this.label9.AutoSize = true;
		this.label9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.label9.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(573, 130);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(53, 14);
		this.label9.TabIndex = 69;
		this.label9.Text = "Bility No";
		this.cmbtransporter.AllowNull = (DefaultableBoolean)1;
		this.cmbtransporter.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbtransporter.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbtransporter.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance167).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.Appearance = (AppearanceBase)(object)appearance167;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance168).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance168).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance168).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbtransporter).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance168;
		((AppearanceBase)appearance169).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance169;
		((SpecialBoxBase)((UltraGridBase)this.cmbtransporter).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance62).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance62;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance63).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance64;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance65).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance67).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbtransporter).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance68).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbtransporter).Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbtransporter.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbtransporter).Location = new System.Drawing.Point(1290, 150);
		((System.Windows.Forms.Control)(object)this.cmbtransporter).Name = "cmbtransporter";
		((System.Windows.Forms.Control)(object)this.cmbtransporter).Size = new System.Drawing.Size(207, 25);
		((System.Windows.Forms.Control)(object)this.cmbtransporter).TabIndex = 9;
		((System.Windows.Forms.Control)(object)this.cmbtransporter).TabStop = false;
		((System.Windows.Forms.Control)(object)this.cmbtransporter).Visible = false;
		this.txtDriverName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDriverName.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDriverName.Location = new System.Drawing.Point(389, 99);
		this.txtDriverName.Name = "txtDriverName";
		this.txtDriverName.Size = new System.Drawing.Size(173, 22);
		this.txtDriverName.TabIndex = 11;
		this.cmbLcOrderNo.AllowNull = (DefaultableBoolean)1;
		this.cmbLcOrderNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbLcOrderNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbLcOrderNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbLcOrderNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbLcOrderNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbLcOrderNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance30).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance30).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbLcOrderNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLcOrderNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance36;
		((SpecialBoxBase)((UltraGridBase)this.cmbLcOrderNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance47).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance47).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLcOrderNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.cmbLcOrderNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbLcOrderNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbLcOrderNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance58;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbLcOrderNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.cmbLcOrderNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbLcOrderNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLcOrderNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance92).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbLcOrderNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.cmbLcOrderNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbLcOrderNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance103).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance103).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance103).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLcOrderNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance103;
		((AppearanceBase)appearance114).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbLcOrderNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.cmbLcOrderNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbLcOrderNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance124).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbLcOrderNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.cmbLcOrderNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbLcOrderNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.cmbLcOrderNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbLcOrderNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbLcOrderNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbLcOrderNo).Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbLcOrderNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbLcOrderNo).Location = new System.Drawing.Point(105, 208);
		((System.Windows.Forms.Control)(object)this.cmbLcOrderNo).Name = "cmbLcOrderNo";
		((System.Windows.Forms.Control)(object)this.cmbLcOrderNo).Size = new System.Drawing.Size(171, 25);
		((System.Windows.Forms.Control)(object)this.cmbLcOrderNo).TabIndex = 5;
		this.txtVehicleNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtVehicleNo.Enabled = false;
		this.txtVehicleNo.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtVehicleNo.Location = new System.Drawing.Point(694, 100);
		this.txtVehicleNo.Name = "txtVehicleNo";
		this.txtVehicleNo.Size = new System.Drawing.Size(173, 22);
		this.txtVehicleNo.TabIndex = 15;
		this.label8.AutoSize = true;
		this.label8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.label8.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(573, 104);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(68, 14);
		this.label8.TabIndex = 65;
		this.label8.Text = "Vehicle No.";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(1287, 135);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(69, 14);
		this.label7.TabIndex = 62;
		this.label7.Text = "Transporter";
		this.label7.Visible = false;
		this.label6.AutoSize = true;
		this.label6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.label6.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(573, 76);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(50, 14);
		this.label6.TabIndex = 60;
		this.label6.Text = "GP Date";
		this.label4.AutoSize = true;
		this.label4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.label4.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(7, 156);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(43, 14);
		this.label4.TabIndex = 56;
		this.label4.Text = "GP NO.";
		this.txtGPDate.CustomFormat = "dd-MMM-yy";
		this.txtGPDate.Enabled = false;
		this.txtGPDate.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGPDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtGPDate.Location = new System.Drawing.Point(694, 72);
		this.txtGPDate.Name = "txtGPDate";
		this.txtGPDate.Size = new System.Drawing.Size(173, 22);
		this.txtGPDate.TabIndex = 14;
		this.DocDate.CustomFormat = "dd-MMM-yy";
		this.DocDate.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DocDate.Location = new System.Drawing.Point(106, 100);
		this.DocDate.Name = "DocDate";
		this.DocDate.Size = new System.Drawing.Size(171, 22);
		this.DocDate.TabIndex = 2;
		this.txtdocno.BackColor = System.Drawing.Color.White;
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocno.Location = new System.Drawing.Point(106, 126);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.Size = new System.Drawing.Size(171, 22);
		this.txtdocno.TabIndex = 3;
		this.txtdocno.TabStop = false;
		this.cmbpo.AllowNull = (DefaultableBoolean)1;
		this.cmbpo.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbpo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbpo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbpo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbpo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbpo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance5).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance5).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbpo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance5;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbpo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance6;
		((SpecialBoxBase)((UltraGridBase)this.cmbpo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance7).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance7).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbpo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.cmbpo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbpo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbpo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbpo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.cmbpo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbpo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbpo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance10;
		((AppearanceBase)appearance11).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance11).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbpo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.cmbpo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbpo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbpo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance13).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbpo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbpo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbpo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbpo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.cmbpo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbpo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbpo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbpo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbpo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbpo).Font = new System.Drawing.Font("Calibri", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbpo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbpo).Location = new System.Drawing.Point(105, 179);
		((System.Windows.Forms.Control)(object)this.cmbpo).Name = "cmbpo";
		((System.Windows.Forms.Control)(object)this.cmbpo).Size = new System.Drawing.Size(171, 25);
		((System.Windows.Forms.Control)(object)this.cmbpo).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.cmbpo).Leave += new System.EventHandler(cmbInvoiceNo_Leave);
		this.panel2.Controls.Add(this.ctrlGrdBar1);
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1237, 30);
		this.panel2.TabIndex = 0;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1188, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdDetail;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 21);
		this.ctrlGrdBar1.TabIndex = 36;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.BtnNew, this.btnSave, this.btnUpdate, this.btnRefresh, this.btnPrint });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1237, 27);
		this.toolStrip1.TabIndex = 1;
		this.toolStrip1.Text = "toolStrip1";
		this.BtnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.BtnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnNew.Name = "BtnNew";
		this.BtnNew.Size = new System.Drawing.Size(55, 24);
		this.BtnNew.Text = "&New";
		this.BtnNew.Click += new System.EventHandler(BtnNew_Click);
		this.btnSave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(55, 24);
		this.btnSave.Text = "&Save";
		this.btnSave.Click += new System.EventHandler(btnSave_Click);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(69, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(50, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(79, 24);
		this.btnPrint.Text = "803-Print";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click_1);
		this.tabPage2.Controls.Add(this.panel6);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1243, 636);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel6.Controls.Add(this.panel10);
		this.panel6.Controls.Add(this.panel8);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(3, 3);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1237, 630);
		this.panel6.TabIndex = 0;
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel10.Location = new System.Drawing.Point(0, 25);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1237, 605);
		this.panel10.TabIndex = 2;
		this.DataGridHistory.AllowDelete = (InheritableBoolean)1;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistory.DynamicFiltering = true;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).Office2007ColorScheme = (Office2007ColorScheme)4;
		((ScrollableBase)this.DataGridHistory).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 0);
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1237, 605);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 45;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).DoubleClick += new System.EventHandler(DataGridHistory_DoubleClick);
		this.panel8.Controls.Add(this.ctrlGrdBar2);
		this.panel8.Controls.Add(this.toolStrip2);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 0);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1237, 25);
		this.panel8.TabIndex = 0;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1186, 2);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdDetail;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(45, 21);
		this.ctrlGrdBar2.TabIndex = 54;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.toolStripSeparator1, this.toolStripLabel2 });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1237, 25);
		this.toolStrip2.TabIndex = 53;
		this.toolStrip2.Text = "toolStrip2";
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
		this.toolStripLabel2.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripLabel2.Name = "toolStripLabel2";
		this.toolStripLabel2.Size = new System.Drawing.Size(84, 22);
		this.toolStripLabel2.Text = "History      ";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1251, 662);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "ImForwardingGdn";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(ExpfrmForwarding_Load);
		base.Click += new System.EventHandler(ExpfrmForwarding_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(ExpfrmForwarding_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWareHouse).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPackUOM).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPackType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.txtLcOrder).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.comproject).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combranches).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCustomer).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbShippingAgent).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbGpNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbShippingLIne).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDestinationPort).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbLoadingPort).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbtransporter).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbLcOrderNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbpo).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.panel10.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}
