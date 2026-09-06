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
using Architecture.BLL.pcc;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Authentication;
using Architecture.Model.Common;
using Architecture.Model.Inventory;
using Architecture.Model.pcc;
using Architecture.Reports;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.pcc.Sale;

public class GdnDirectConcrete : Form
{
	public DataTable dtViewAttachments = new DataTable();

	public DataTable dtAttachmentsUpload = new DataTable();

	private Rightsobjects formright = new Rightsobjects();

	private DropDownBind DDL = new DropDownBind();

	private Attachment AT = new Attachment();

	private DataTable dtGrid = new DataTable();

	private DataTable dtWagesDetail = new DataTable();

	private DataTable dtSupplier = new DataTable();

	private DataTable dtjoblot = new DataTable();

	private DataTable dtAccountlst = new DataTable();

	private DataTable dtCity = new DataTable();

	private DataTable dtitem = new DataTable();

	private DataTable dtItemForWages = new DataTable();

	private DataTable dtVarientForWages = new DataTable();

	private DataTable dtwareh = new DataTable();

	private DataTable dtContr = new DataTable();

	private DataTable dtVehicleNos = new DataTable();

	private bool WagesStatus;

	private int updateDetailIndex;

	private int Id;

	private int ItemId;

	private bool Approved;

	private bool SubsidiaryAccountAllownOnVouchers;

	private bool GetItemagainstwarehouseId;

	private int OutwardFreightAccountId;

	private bool ItemSearchWithCode;

	private bool PartySearchWithCode;

	private DataTable dtCustomerHistory = new DataTable();

	private List<InvGdnDetail> lstRemoveDetailRecord = new List<InvGdnDetail>();

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel1;

	private Panel panel3;

	private ToolStrip toolStrip1;

	private ToolStripButton btnNew;

	private ToolStripButton btnUpdate;

	private ToolStripButton btnSave;

	private ToolStripButton btnAttachment;

	private TabPage tabPage2;

	private GroupBox groupBox1;

	private Label label40;

	private TextBox txtQty;

	private Label label38;

	private Label label32;

	private Label label33;

	private Label label31;

	private DateTimePicker txtgpdate;

	internal UltraCombo comjobLot;

	internal UltraCombo comItem;

	internal UltraCombo comWarehouse;

	private Label label47;

	private Label label48;

	private Button btnAdd;

	private TextBox txtgatepassno;

	internal GridEX grd;

	private TextBox txtSupplierGLId;

	private CtrlGrdBar ctrlGrdBar1;

	internal GridEX grdHistory;

	private Button btnCancelUpdateDetial;

	private Button btnUpdateDetail;

	private ToolStripButton btnSlip;

	private TextBox txtremarks;

	private Label label24;

	private Label label29;

	internal UltraCombo comsupplier;

	private Label label7;

	private TextBox txtSupplierReference;

	private Label label12;

	internal UltraCombo comproject;

	internal UltraCombo combranches;

	private Label label13;

	private Label label5;

	private Label label18;

	private DateTimePicker DocDate;

	private TextBox txtdocno;

	private ToolStripButton btnDelete;

	internal UltraCombo combdeliverytrm;

	private Panel panel9;

	private Panel panel2;

	private ToolStrip toolStrip2;

	private ToolStripButton BtnNewHistory;

	private ToolStripButton btnRefresh;

	private CtrlGrdBar ctrlGrdBar2;

	private Panel panel10;

	internal GridEX grdDetailHistory;

	private CheckBox ChkPrintslip;

	private CheckBox ChkResetOnSave;

	internal UltraCombo CmbTransporterAc;

	private TextBox txtFreightAmountHeader;

	private Label lblBalance;

	private ToolStripButton btnLoadGdn;

	internal UltraCombo txtCity;

	private Label label8;

	private Panel panel11;

	private GroupBox groupBox2;

	private Label label17;

	private Panel panel16;

	private Label label23;

	private Panel panel17;

	private Label label54;

	private Panel panel18;

	private CtrlGrdBar ctrlGrdBar3;

	private Label label55;

	private Panel panel19;

	private RadioButton rdbtnItemCode;

	private RadioButton rdbtnItemName;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripSeparator toolStripSeparator3;

	private ToolStripSeparator toolStripSeparator4;

	private ToolStripSeparator toolStripSeparator5;

	private ToolStripSeparator toolStripSeparator6;

	private ToolStripButton btnshortcutkeys;

	private ToolStripSeparator toolStripSeparator1;

	private TabControl tabControl2;

	private TabPage tabPage3;

	internal GridEX grdContractorWages;

	internal UltraCombo CmbAttributeVarient;

	private Label label1;

	private Label label2;

	private Label label9;

	private TextBox txtItemWeight;

	private TextBox txtVarientEquivalent;

	private Panel panel5;

	private Panel panel7;

	private Label label78;

	private GroupBox groupBox4;

	private Button btnshow;

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

	private Panel panel8;

	private Label label30;

	private Label label34;

	private Label label36;

	private TextBox txtRefPartyCellNo;

	private TextBox txtRefPartyAddress;

	private TextBox txtRefPartyName;

	private Label label43;

	private TextBox txtNetWeight;

	private Label label89;

	private Label label3;

	private ToolStripButton BtnRefreshHistory;

	private CheckBox chkisFreeOfCost;

	internal UltraCombo CmbVehicleType;

	private Label label14;

	private TextBox txtCNICNO;

	private Label label4;

	private Label label11;

	private Label label10;

	private TextBox txtDriverCellNo;

	private TextBox txtDriverName;

	private TextBox txtRemarksDetail;

	private Label label15;

	private Label label6;

	private TabPage tabPage4;

	private Panel panel4;

	private Label label16;

	private Label label19;

	internal UltraCombo CmbVehicleNo;

	private Button BtnGenerateWagesRows;

	private Button BtnUpdateConctratorInWages;

	private Label label22;

	private TextBox TxtDistance;

	private TextBox txtManualNo;

	internal UltraCombo CmbPackUom;

	private Label label58;

	internal UltraCombo CmbContractorForUpdateInWagesGrid;

	private Label label66;

	private Label label65;

	private Label label49;

	internal UltraCombo CmbActivityforContractorUpdateInWagesGrid;

	private RadioButton RdPartyByCode;

	private RadioButton RdPartyByName;

	private Label label20;

	private RadioButton RdCustomerHistorySearchByCode;

	private RadioButton RdCustomerHistorySearchByName;

	public GdnDirectConcrete(UserAccount user)
	{
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void InvfrmPurchasedirectInvoice_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(5, 5);
			base.AutoScrollMinSize = new Size(1200, 800);
			ItemSearchWithCode = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("ItemSearchByCode"));
			if (ItemSearchWithCode)
			{
				rdbtnItemCode.Checked = true;
			}
			else
			{
				rdbtnItemName.Checked = true;
			}
			PartySearchWithCode = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("SupplierCustomerDefaultFilterByPartyCode"));
			if (PartySearchWithCode)
			{
				RdPartyByCode.Checked = true;
			}
			else
			{
				RdPartyByName.Checked = true;
			}
			if (PartySearchWithCode)
			{
				RdCustomerHistorySearchByCode.Checked = true;
			}
			else
			{
				RdCustomerHistorySearchByName.Checked = true;
			}
			string ItemStatus = CommonServices.GetConfigurationByOrgCompandConfigDescription("GetItemsagainstWarehouse");
			if (ItemStatus != string.Empty)
			{
				GetItemagainstwarehouseId = Conversion.ToBool((object)ItemStatus);
			}
			List<ConfigrationsAllocation> list = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "FreightOutwardAc").ToList();
			if (list.Count > 0)
			{
				int value = Conversion.ToInt((object)list[0].ConfigKey.ToString());
				if (value > 0)
				{
					((Control)(object)CmbTransporterAc).Visible = true;
					txtFreightAmountHeader.Visible = true;
					OutwardFreightAccountId = value;
				}
			}
			dtViewAttachments.Columns.Add("AttchmentPath");
			string ScreenName = base.Name;
			SubsidiaryAccountAllownOnVouchers = CommonServices.GetERPFeatureById(4);
			formright = CommonServices.SetRightsValueInRightsObject(ScreenName);
			btnSave.Enabled = formright.DoHaveSaveRight;
			btnUpdate.Enabled = formright.DoHaveUpdateRights;
			btnDelete.Enabled = formright.DoHaveCanDelete;
			btnSlip.Enabled = formright.DoHavePrintRights;
			ChkPrintslip.Enabled = formright.DoHavePrintRights;
			ChkPrintslip.Checked = formright.DoHavePrintRights;
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
			ctrlGrdBar3.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar3.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar3.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar3.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar3.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar3.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			dtitem.Columns.Add("Id", typeof(int));
			dtitem.Columns.Add("ItemName", typeof(string));
			dtitem.Columns.Add("ItemCode", typeof(string));
			dtitem.Columns.Add("ItemWeight", typeof(double));
			dtitem.Columns.Add("ScheduleId", typeof(int));
			dtitem.Columns.Add("WithVariant");
			dtVehicleNos.Columns.Add("Id", typeof(int));
			dtVehicleNos.Columns.Add("VehicleNo");
			dtItemForWages.Columns.Add("Id", typeof(int));
			dtItemForWages.Columns.Add("ItemName", typeof(string));
			dtItemForWages.Columns.Add("ItemCode", typeof(string));
			dtVarientForWages.Columns.Add("ItemAttributeId", typeof(int));
			dtVarientForWages.Columns.Add("ItemAttribute", typeof(string));
			dtVarientForWages.Columns.Add("ItemId", typeof(int));
			dtVarientForWages.Columns.Add("ItemName", typeof(string));
			dtVarientForWages.Columns.Add("VarientEquivalent", typeof(decimal));
			dtVarientForWages.Columns.Add("Qty", typeof(decimal));
			dtVarientForWages.Columns.Add("NetWeight", typeof(decimal));
			dtForGridComboFill();
			dtGrid.Columns.Add("Id", typeof(int));
			dtGrid.Columns.Add("WarehouseId", typeof(int));
			dtGrid.Columns.Add("Warehouse", typeof(string));
			dtGrid.Columns.Add("ItemId", typeof(int));
			dtGrid.Columns.Add("ItemName", typeof(string));
			dtGrid.Columns.Add("ItemCode", typeof(string));
			dtGrid.Columns.Add("ScheduleId", typeof(string));
			dtGrid.Columns.Add("PackUomId", typeof(int));
			dtGrid.Columns.Add("PackUom");
			dtGrid.Columns.Add("PackUomEquivalent", typeof(decimal));
			dtGrid.Columns.Add("AttributeVarientId", typeof(int));
			dtGrid.Columns.Add("AttributeVarient", typeof(string));
			dtGrid.Columns.Add("VarientUnit", typeof(decimal));
			dtGrid.Columns.Add("JobLotId", typeof(int));
			dtGrid.Columns.Add("JobLot", typeof(string));
			dtGrid.Columns.Add("ItemQty", typeof(decimal));
			dtGrid.Columns.Add("AvailableQty", typeof(decimal));
			dtGrid.Columns.Add("ItemWeight", typeof(decimal));
			dtGrid.Columns.Add("NetWeight", typeof(decimal));
			dtGrid.Columns.Add("BalWeight", typeof(decimal));
			dtGrid.Columns.Add("CityId", typeof(int));
			dtGrid.Columns.Add("CityName", typeof(string));
			dtGrid.Columns.Add("IsFOC", typeof(bool));
			dtGrid.Columns.Add("RemarksDetail", typeof(string));
			grd.DataSource = dtGrid;
			grd.RetrieveStructure();
			grdSettings();
			dtWagesDetail.Columns.Add("Id", typeof(int));
			dtWagesDetail.Columns.Add("OrderId", typeof(int));
			dtWagesDetail.Columns.Add("OrderWagesId", typeof(int));
			dtWagesDetail.Columns.Add("ItemId", typeof(int));
			dtWagesDetail.Columns.Add("ItemName");
			dtWagesDetail.Columns.Add("AttributeVarientId", typeof(int));
			dtWagesDetail.Columns.Add("AttributeVarient");
			dtWagesDetail.Columns.Add("VarientEquivalent", typeof(decimal));
			dtWagesDetail.Columns.Add("ContractorId", typeof(int));
			dtWagesDetail.Columns.Add("ContractorName");
			dtWagesDetail.Columns.Add("ContractorWagesRateScheduleId", typeof(int));
			dtWagesDetail.Columns.Add("ServiceActivity");
			dtWagesDetail.Columns.Add("ParentUomId", typeof(int));
			dtWagesDetail.Columns.Add("Qty", typeof(decimal));
			dtWagesDetail.Columns.Add("ItemNetWeight", typeof(decimal));
			dtWagesDetail.Columns.Add("Rate", typeof(decimal));
			dtWagesDetail.Columns.Add("Amount", typeof(decimal));
			dtWagesDetail.Columns.Add("AddLess", typeof(decimal));
			dtWagesDetail.Columns.Add("NetAmount", typeof(decimal));
			dtWagesDetail.Columns.Add("Remarks");
			AddRowInContractorWagesGrid();
			grdContractorWages.RetrieveStructure();
			grdContractorWagesGridSetting();
			DocumentNo();
			Project();
			braches();
			suppliercustomer();
			DeliveryTerm();
			TransporterAcFill();
			bindWareHouse();
			item();
			JobLotBind();
			BinCity();
			BindVehicles();
			ConfigurationDefault();
			vehicleTypefill();
			DocDate.Focus();
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			btnDelete.Visible = false;
			btnAdd.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			dtCustomerHistory.Columns.Add("Id", typeof(int));
			dtCustomerHistory.Columns.Add("PartyName");
			dtCustomerHistory.Columns.Add("PartyCode");
			ParameterFill();
			HistoryComboBind();
			if (Conversion.ToInt(cmbDateTypeHistory.Value) == 0)
			{
				((UltraGridBase)cmbDateTypeHistory).Rows[0].Activate();
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

	private void DocumentNo()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		try
		{
			int Code = InvGdn.GenerateCode(new InvGdn
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				DocumentTypeId = 1866,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
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
			DataTable BranchList = CommonServices.BrancheServiceBind();
			if (BranchList.Rows.Count > 0)
			{
				DDL.BindDDLNew(BranchList, combranches, "Id", "BranchName", "Branch Name", false);
				((UltraGridBase)combranches).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void suppliercustomer()
	{
		try
		{
			int Id = Conversion.ToInt(comsupplier.Value);
			if (SubsidiaryAccountAllownOnVouchers)
			{
				dtSupplier = CommonServices.GetVendorsAndCustomers(2);
			}
			else
			{
				dtSupplier = CommonServices.SupplierCustomerGetAllServiceBind();
			}
			if (dtSupplier.Rows.Count > 0)
			{
				if (RdPartyByName.Checked)
				{
					DDL.BindDDL(dtSupplier, comsupplier, "Id", "CompanyName", "Party Name", ZeroIndex: true);
				}
				else
				{
					DDL.BindDDL(dtSupplier, comsupplier, "Id", "PartyCode", "Party Code", ZeroIndex: true);
				}
			}
			if (Id > 0)
			{
				if (dtSupplier.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
				{
					comsupplier.Value = Id;
				}
				else
				{
					((Control)(object)comsupplier).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeliveryTerm()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("DeliveryTerm");
			dt.Rows.Add(1, "Factory Loading");
			dt.Rows.Add(2, "Delivery");
			DDL.BindDDL(dt, combdeliverytrm, "Id", "DeliveryTerm", "Delivery Term", ZeroIndex: false);
			((UltraGridBase)combdeliverytrm).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TransporterAcFill()
	{
		try
		{
			int ItId1 = Conversion.ToInt(CmbTransporterAc.Value);
			DataTable dt = CommonServices.CoaAllocationAccountTitleByAccountTypeIds("10,3");
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDLNew(dt, CmbTransporterAc, "Id", "AccountTitle", "Account Title", true);
			if (ItId1 > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ItId1))
				{
					CmbTransporterAc.Value = ItId1;
				}
				else
				{
					((Control)(object)CmbTransporterAc).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void bindWareHouse()
	{
		try
		{
			int Id = Conversion.ToInt(comWarehouse.Value);
			dtwareh.Clear();
			dtwareh = CommonServices.getActiveWareHouse();
			if (dtwareh.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDL(dtwareh, comWarehouse, "Id", "WareHouseName", "Ware House ", ZeroIndex: true);
			if (Id > 0)
			{
				if (dtwareh.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
				{
					comWarehouse.Value = Id;
				}
				else
				{
					((Control)(object)comWarehouse).Text = string.Empty;
				}
			}
		}
		catch (Exception)
		{
			MessageBox.Show("WareHouse Not Found");
		}
	}

	private void item()
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		try
		{
			int Id = Conversion.ToInt(comItem.Value);
			((Control)(object)comItem).Text = string.Empty;
			dtitem.Rows.Clear();
			DataTable dt = ItemPricingSchedule.GetItemWithRatesForItemPricingSchedule(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				EffectedDate = DocDate.Value
			});
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtitem.Rows.Add(dt.Rows[i]["ItemId"], dt.Rows[i]["ItemName"], dt.Rows[i]["ItemCodeNew"], dt.Rows[i]["WeightKgs"], dt.Rows[i]["ScheduleId"], dt.Rows[i]["ItemWithVarient"]);
				}
			}
			if (dtitem.Rows.Count > 0)
			{
				if (rdbtnItemName.Checked)
				{
					DDL.BindDDL(dtitem, comItem, "Id", "ItemName", "Item Name", ZeroIndex: true);
				}
				else
				{
					DDL.BindDDL(dtitem, comItem, "Id", "ItemCode", "Item Name", ZeroIndex: true);
				}
				((UltraGridBase)comItem).DisplayLayout.Bands[0].Columns[1].Width = 300;
				if (Id > 0)
				{
					if (dtitem.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
					{
						comItem.Value = Id;
					}
					else
					{
						((Control)(object)comItem).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)comItem).Text = string.Empty;
				((UltraGridBase)comItem).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void bindvarientunit(int ID)
	{
		try
		{
			string ItId = Conversion.ToString((object)((Control)(object)CmbAttributeVarient).Text);
			DataTable dtunit = ItemAttributeVarient.GetAllForCombo(UserAccount.OrganizationId, UserAccount.CompanyId, ID);
			if (dtunit.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("ItemAttributeVarientId", typeof(int));
				dt.Columns.Add("ItemAttribute");
				dt.Columns.Add("VarientEquivalent", typeof(double));
				dt.Clear();
				for (int i = 0; i < dtunit.Rows.Count; i++)
				{
					dt.Rows.Add(dtunit.Rows[i]["ItemAttributeVarientId"], dtunit.Rows[i]["ItemAttribute"], dtunit.Rows[i]["VarientEquivalent"]);
				}
				DDL.BindDDL(dt, CmbAttributeVarient, "ItemAttributeVarientId", "ItemAttribute", "Varient", ZeroIndex: true);
				if (ItId != null && ItId != string.Empty)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<string>("ItemAttribute") == ItId))
					{
						((Control)(object)CmbAttributeVarient).Text = ItId;
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

	private void JobLotBind()
	{
		try
		{
			int Id = Conversion.ToInt(comjobLot.Value);
			dtjoblot.Clear();
			dtjoblot = CommonServices.JobLotGetAllService();
			if (dtjoblot.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDLNew(dtjoblot, comjobLot, "Id", "JobLotDescription", "Job Lot ", true);
			if (Id > 0)
			{
				if (dtjoblot.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
				{
					comjobLot.Value = Id;
				}
				else
				{
					((Control)(object)comjobLot).Text = string.Empty;
				}
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
			int Id = Conversion.ToInt(txtCity.Value);
			dtCity.Clear();
			dtCity = CommonServices.CityGetAllService();
			if (dtCity.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDLNew(dtCity, txtCity, "Id", "CityName", "City Name ", true);
			if (Id > 0)
			{
				if (dtCity.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
				{
					txtCity.Value = Id;
				}
				else
				{
					((Control)(object)txtCity).Text = string.Empty;
				}
			}
		}
		catch (Exception)
		{
			MessageBox.Show("City Not Found");
		}
	}

	private void dtForGridComboFill()
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		try
		{
			dtContr.Clear();
			dtAccountlst.Clear();
			SupplierCustomer supplier = new SupplierCustomer();
			supplier.OrganizationId = UserAccount.OrganizationId;
			supplier.CompanyId = UserAccount.CompanyId;
			dtContr = SupplierCustomer.ReadByOrganizationCompanyIdForContractorWages(supplier);
			dtAccountlst = CommonServices.CoaAllocationGetAllServiceBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ConfigurationDefault()
	{
		try
		{
			List<ConfigrationsAllocation> lstWareHouse = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Warehouse").ToList();
			if (lstWareHouse.Count > 0)
			{
				int WareHouseId = Conversion.ToInt((object)lstWareHouse[0].ConfigKey.ToString());
				comWarehouse.Value = WareHouseId;
			}
			List<ConfigrationsAllocation> list = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "City Area").ToList();
			if (list.Count > 0)
			{
				int CityId = Conversion.ToInt((object)list[0].ConfigKey.ToString());
				if (Conversion.ToInt(txtCity.Value) == 0)
				{
					txtCity.Value = CityId;
				}
			}
			List<ConfigrationsAllocation> lstJob = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Job/Lot").ToList();
			if (lstJob.Count > 0)
			{
				int JobId = Conversion.ToInt((object)lstJob[0].ConfigKey.ToString());
				if (Conversion.ToInt(comjobLot.Value) == 0)
				{
					comjobLot.Value = JobId;
				}
			}
		}
		catch (Exception)
		{
			MessageBox.Show("Confiquration list Error");
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

	public void bindPackUom(int ItemId)
	{
		try
		{
			string Packuom = ((Control)(object)CmbPackUom).Text;
			((Control)(object)CmbPackUom).Text = string.Empty;
			DataTable dtUom = CommonServices.GetUomScheduleByItemId(ItemId);
			if (dtUom.Rows.Count > 0)
			{
				DDL.BindDDL(dtUom, CmbPackUom, "Id", "UOMCode", "PackUOM", ZeroIndex: false);
				((UltraGridBase)CmbPackUom).DisplayLayout.Bands[0].Columns[1].Width = 75;
				((UltraGridBase)CmbPackUom).DisplayLayout.Bands[0].Columns[2].Width = 85;
				if (dtUom.AsEnumerable().Any((DataRow row) => row.Field<string>("UOMCode") == Packuom))
				{
					((Control)(object)CmbPackUom).Text = Packuom;
				}
				else
				{
					((Control)(object)CmbPackUom).Text = string.Empty;
				}
			}
			else
			{
				((Control)(object)CmbPackUom).Text = string.Empty;
				((UltraGridBase)CmbPackUom).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void comItem_Leave(object sender, EventArgs e)
	{
		try
		{
			bindPackUom(Conversion.ToInt(comItem.Value));
			if (Conversion.ToInt(comItem.Value) > 0)
			{
				txtItemWeight.Text = Conversion.ToString(((UltraDropDownBase)comItem).SelectedRow.Cells["ItemWeight"].Value);
				bindvarientunit(Conversion.ToInt(comItem.Value));
			}
			else
			{
				txtItemWeight.Text = 0.ToString();
			}
			AvailableStockGetByItem();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AvailableStockGetByItem()
	{
		try
		{
			int OrganizationId = UserAccount.OrganizationId;
			int CompanyId = UserAccount.CompanyId;
			int ItemId = Conversion.ToInt(comItem.Value);
			int wareId = Conversion.ToInt(comWarehouse.Value);
			int AttributeVarientId = Conversion.ToInt(CmbAttributeVarient.Value);
			int jobLotId = Conversion.ToInt(comjobLot.Value);
			if (ItemId > 0)
			{
				DataTable DtStock = GetAvgRatesAndStockInHand.GetStockInHandAndAvgRateFromEvaluationConcrete(OrganizationId, CompanyId, ItemId, Conversion.ToDateTime((object)DocDate.Value), wareId, jobLotId, AttributeVarientId);
				if (DtStock.Rows.Count > 0)
				{
					lblBalance.Text = Conversion.ToDecimal(DtStock.Rows[0]["QtyInHand"]).ToString();
				}
				else
				{
					lblBalance.Text = 0.ToString();
				}
			}
			else
			{
				lblBalance.Text = 0.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void comWarehouse_TextChanged(object sender, EventArgs e)
	{
		AvailableStockGetByItem();
	}

	private void comjobLot_Leave(object sender, EventArgs e)
	{
		AvailableStockGetByItem();
	}

	private void btnAdd_Click(object sender, EventArgs e)
	{
		try
		{
			if (!FormValidationDetail())
			{
				return;
			}
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow obj in rows)
			{
				int WareHouseId = Conversion.ToInt(obj.Cells["WareHouseId"].Value);
				int ItemId = Conversion.ToInt(obj.Cells["ItemId"].Value);
				int AttributeVarientId = Conversion.ToInt(obj.Cells["AttributeVarientId"].Value);
				int JobLotId = Conversion.ToInt(obj.Cells["JobLotId"].Value);
				int UomId = Conversion.ToInt(obj.Cells["PackUomId"].Value);
				int CityId = Conversion.ToInt(obj.Cells["CityId"].Value);
				bool IsFOC = Conversion.ToBool(obj.Cells["IsFOC"].Value);
				if (Conversion.ToInt(comWarehouse.Value) == WareHouseId && Conversion.ToInt(comItem.Value) == ItemId && Conversion.ToInt(CmbAttributeVarient.Value) == AttributeVarientId && Conversion.ToInt(comjobLot.Value) == JobLotId && Conversion.ToInt(txtCity.Value) == CityId && Conversion.ToInt(CmbPackUom.Value) == UomId && chkisFreeOfCost.Checked == IsFOC)
				{
					MessageBox.Show("Duplicate items are not added to the grid. An item with the same warehouse,Uom, AttributeVarient,job lot and City already exists");
					return;
				}
			}
			Conversion.ToInt((object)grd.RowCount);
			dtGrid.Rows.Add(0, Conversion.ToInt(comWarehouse.Value), Conversion.ToString((object)((Control)(object)comWarehouse).Text), Conversion.ToInt(comItem.Value), Conversion.ToString(((UltraDropDownBase)comItem).SelectedRow.Cells["ItemName"].Value), Conversion.ToString(((UltraDropDownBase)comItem).SelectedRow.Cells["ItemCode"].Value), Conversion.ToString(((UltraDropDownBase)comItem).SelectedRow.Cells["ScheduleId"].Value), Conversion.ToInt(CmbPackUom.Value), Conversion.ToString(((UltraDropDownBase)CmbPackUom).SelectedRow.Cells["UOMCode"].Value), Conversion.ToDecimal(((UltraDropDownBase)CmbPackUom).SelectedRow.Cells["Equivalent"].Value), Conversion.ToBool(((UltraDropDownBase)comItem).SelectedRow.Cells["WithVariant"].Value) ? Conversion.ToInt(CmbAttributeVarient.Value) : 0, (!Conversion.ToBool(((UltraDropDownBase)comItem).SelectedRow.Cells["WithVariant"].Value)) ? "" : Conversion.ToString((object)((Control)(object)CmbAttributeVarient).Text.Trim()), (!Conversion.ToBool(((UltraDropDownBase)comItem).SelectedRow.Cells["WithVariant"].Value)) ? 0m : Conversion.ToDecimal((object)txtVarientEquivalent.Text.Trim()), Conversion.ToInt(comjobLot.Value), Conversion.ToString((object)((Control)(object)comjobLot).Text), Conversion.ToDecimal((object)txtQty.Text), Conversion.ToDecimal((object)txtQty.Text), Conversion.ToDecimal((object)txtItemWeight.Text), Conversion.ToDecimal((object)txtNetWeight.Text), Conversion.ToDecimal((object)txtNetWeight.Text), Conversion.ToInt(txtCity.Value), Conversion.ToString((object)((Control)(object)txtCity).Text), Conversion.ToBool((object)chkisFreeOfCost.Checked), Conversion.ToString((object)txtRemarksDetail.Text));
			grd.DataSource = dtGrid;
			grd.RetrieveStructure();
			grdSettings();
			ResetDetail();
			WagesItemFillFromDetail();
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
			GridEXRow r = grd.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				updateDetailIndex = r.RowIndex;
				comWarehouse.Value = r.Cells["WareHouseId"].Value.ToString();
				ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
				comItem.Value = r.Cells["ItemId"].Value.ToString();
				bindvarientunit(Conversion.ToInt(comItem.Value));
				bindPackUom(Conversion.ToInt(r.Cells["ItemId"].Value));
				if (Conversion.ToInt(r.Cells["PackUomId"].Value) > 0)
				{
					CmbPackUom.Value = Conversion.ToInt(r.Cells["PackUomId"].Value);
				}
				if (Conversion.ToInt(r.Cells["AttributeVarientId"].Value) > 0)
				{
					CmbAttributeVarient.Value = Conversion.ToInt(r.Cells["AttributeVarientId"].Value);
				}
				txtVarientEquivalent.Text = r.Cells["VarientUnit"].Value.ToString();
				comjobLot.Value = r.Cells["JobLotId"].Value.ToString();
				txtQty.Text = Conversion.ToDecimal(r.Cells["ItemQty"].Value).ToString("#,##0.##");
				txtItemWeight.Text = Conversion.ToDecimal(r.Cells["ItemWeight"].Value).ToString("#,##0.##");
				txtNetWeight.Text = Conversion.ToDecimal(r.Cells["NetWeight"].Value).ToString("#,##0.##");
				txtCity.Value = Conversion.ToInt(r.Cells["CityId"].Value);
				chkisFreeOfCost.Checked = Conversion.ToBool(r.Cells["IsFOC"].Value);
				btnAdd.Visible = false;
				btnUpdateDetail.Visible = true;
				btnCancelUpdateDetial.Visible = true;
				comItem.Focus();
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
			if (!FormValidationDetail())
			{
				return;
			}
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow item in rows)
			{
				int WareHouseId = Conversion.ToInt(item.Cells["WareHouseId"].Value);
				int ItemId = Conversion.ToInt(item.Cells["ItemId"].Value);
				int AttributeVarientId = Conversion.ToInt(item.Cells["AttributeVarientId"].Value);
				int JobLotId = Conversion.ToInt(item.Cells["JobLotId"].Value);
				int UomId = Conversion.ToInt(item.Cells["PackUomId"].Value);
				int CityId = Conversion.ToInt(item.Cells["CityId"].Value);
				bool IsFOC = Conversion.ToBool(item.Cells["IsFOC"].Value);
				if (Conversion.ToInt(comWarehouse.Value) == WareHouseId && Conversion.ToInt(comItem.Value) == ItemId && Conversion.ToInt(CmbAttributeVarient.Value) == AttributeVarientId && Conversion.ToInt(comjobLot.Value) == JobLotId && Conversion.ToInt(txtCity.Value) == CityId && Conversion.ToInt(CmbPackUom.Value) == UomId && chkisFreeOfCost.Checked == IsFOC && item.RowIndex != updateDetailIndex)
				{
					MessageBox.Show("Duplicate items are not added to the grid. An item with the same warehouse,Uom, AttributeVarient,job lot and City already exists");
					return;
				}
			}
			this.ItemId = 0;
			dtGrid.Rows[updateDetailIndex]["WarehouseId"] = Conversion.ToInt(comWarehouse.Value);
			dtGrid.Rows[updateDetailIndex]["Warehouse"] = Conversion.ToString((object)((Control)(object)comWarehouse).Text);
			dtGrid.Rows[updateDetailIndex]["ItemId"] = Conversion.ToInt(comItem.Value);
			dtGrid.Rows[updateDetailIndex]["ItemName"] = Conversion.ToString(((UltraDropDownBase)comItem).SelectedRow.Cells["ItemName"].Value);
			dtGrid.Rows[updateDetailIndex]["ItemCode"] = Conversion.ToString(((UltraDropDownBase)comItem).SelectedRow.Cells["ItemCode"].Value);
			dtGrid.Rows[updateDetailIndex]["ScheduleId"] = Conversion.ToString(((UltraDropDownBase)comItem).SelectedRow.Cells["ScheduleId"].Value);
			dtGrid.Rows[updateDetailIndex]["PackUomId"] = Conversion.ToInt(CmbPackUom.Value);
			dtGrid.Rows[updateDetailIndex]["PackUom"] = Conversion.ToDecimal(((UltraDropDownBase)CmbPackUom).SelectedRow.Cells["UOMCode"].Value);
			dtGrid.Rows[updateDetailIndex]["PackUomEquivalent"] = Conversion.ToString(((UltraDropDownBase)CmbPackUom).SelectedRow.Cells["Equivalent"].Value);
			dtGrid.Rows[updateDetailIndex]["AttributeVarientId"] = (Conversion.ToBool(((UltraDropDownBase)comItem).SelectedRow.Cells["WithVariant"].Value) ? Conversion.ToInt(CmbAttributeVarient.Value) : 0);
			dtGrid.Rows[updateDetailIndex]["AttributeVarient"] = ((!Conversion.ToBool(((UltraDropDownBase)comItem).SelectedRow.Cells["WithVariant"].Value)) ? "" : Conversion.ToString((object)((Control)(object)CmbAttributeVarient).Text));
			dtGrid.Rows[updateDetailIndex]["VarientUnit"] = ((!Conversion.ToBool(((UltraDropDownBase)comItem).SelectedRow.Cells["WithVariant"].Value)) ? 0m : Conversion.ToDecimal((object)txtVarientEquivalent.Text));
			dtGrid.Rows[updateDetailIndex]["JobLotId"] = Conversion.ToInt(comjobLot.Value);
			dtGrid.Rows[updateDetailIndex]["JobLot"] = Conversion.ToString((object)((Control)(object)comjobLot).Text);
			dtGrid.Rows[updateDetailIndex]["ItemQty"] = Conversion.ToDecimal((object)txtQty.Text);
			dtGrid.Rows[updateDetailIndex]["ItemWeight"] = Conversion.ToDecimal((object)txtItemWeight.Text);
			dtGrid.Rows[updateDetailIndex]["NetWeight"] = Conversion.ToDecimal((object)txtNetWeight.Text);
			dtGrid.Rows[updateDetailIndex]["CityId"] = Conversion.ToInt(txtCity.Value);
			dtGrid.Rows[updateDetailIndex]["CityName"] = Conversion.ToString((object)((Control)(object)txtCity).Text);
			dtGrid.Rows[updateDetailIndex]["IsFOC"] = Conversion.ToBool((object)chkisFreeOfCost.Checked);
			dtGrid.Rows[updateDetailIndex]["RemarksDetail"] = Conversion.ToString((object)txtRemarksDetail.Text);
			btnAdd.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			ResetDetail();
			WagesItemFillFromDetail();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelUpdateDetial_Click(object sender, EventArgs e)
	{
		try
		{
			btnAdd.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			updateDetailIndex = -1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettings()
	{
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Expected O, but got Unknown
		try
		{
			WagesItemFillFromDetail();
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["AttributeVarientId"].Visible = false;
			grd.RootTable.Columns["ItemId"].Visible = false;
			grd.RootTable.Columns["ScheduleId"].Visible = false;
			grd.RootTable.Columns["PackUomId"].Visible = false;
			grd.RootTable.Columns["WarehouseId"].Visible = false;
			grd.RootTable.Columns["JobLotId"].Visible = false;
			grd.RootTable.Columns["CityId"].Visible = false;
			grd.RootTable.Columns["PackUomEquivalent"].Visible = false;
			grd.RootTable.Columns["PackUom"].Caption = "Uom";
			new GridEXColumn();
			GridEXFormatCondition fc = new GridEXFormatCondition(grd.RootTable.Columns["IsFOC"], (ConditionOperator)0, (object)true);
			fc.FormatStyle.ForeColor = Color.Red;
			grd.RootTable.FormatConditions.Add(fc);
			grd.RootTable.Columns["Id"].EditType = (EditType)0;
			grd.RootTable.Columns["WarehouseId"].EditType = (EditType)0;
			grd.RootTable.Columns["Warehouse"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemId"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemName"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemCode"].EditType = (EditType)0;
			grd.RootTable.Columns["PackUomId"].EditType = (EditType)0;
			grd.RootTable.Columns["PackUom"].EditType = (EditType)0;
			grd.RootTable.Columns["PackUomEquivalent"].EditType = (EditType)0;
			grd.RootTable.Columns["AttributeVarientId"].EditType = (EditType)0;
			grd.RootTable.Columns["AttributeVarient"].EditType = (EditType)0;
			grd.RootTable.Columns["VarientUnit"].EditType = (EditType)0;
			grd.RootTable.Columns["JobLotId"].EditType = (EditType)0;
			grd.RootTable.Columns["JobLot"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemQty"].EditType = (EditType)0;
			grd.RootTable.Columns["AvailableQty"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemWeight"].EditType = (EditType)0;
			grd.RootTable.Columns["NetWeight"].EditType = (EditType)0;
			grd.RootTable.Columns["BalWeight"].EditType = (EditType)0;
			grd.RootTable.Columns["CityId"].EditType = (EditType)0;
			grd.RootTable.Columns["CityName"].EditType = (EditType)0;
			grd.RootTable.Columns["IsFOC"].EditType = (EditType)0;
			grd.RootTable.Columns["VarientUnit"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["VarientUnit"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["VarientUnit"].FormatString = "#,##0.##";
			grd.RootTable.Columns["VarientUnit"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["BalWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["BalWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["BalWeight"].FormatString = "#,##0.##";
			grd.RootTable.Columns["BalWeight"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["AvailableQty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["AvailableQty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["AvailableQty"].FormatString = "#,##0.##";
			grd.RootTable.Columns["AvailableQty"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemQty"].FormatString = "#,##0.##";
			grd.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["ItemWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ItemWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemWeight"].FormatString = "#,##0.##";
			grd.RootTable.Columns["ItemWeight"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["NetWeight"].FormatString = "#,##0.##";
			grd.RootTable.Columns["NetWeight"].TotalFormatString = "#,##0.##";
			CommonServices.GridAutoAdjustmentNew(grd);
			grd.RootTable.Columns["ItemQty"].EditType = (EditType)1;
			grd.RootTable.Columns["IsFOC"].EditType = (EditType)8;
			grd.RootTable.Columns.Add("Delete");
			grd.RootTable.Columns["Delete"].Key = "Delete";
			grd.RootTable.Columns["Delete"].Caption = "X";
			grd.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Delete"].ButtonText = "X";
			grd.RootTable.Columns["Delete"].Width = 20;
			grd.RootTable.Columns["Delete"].Position = 0;
			grd.RootTable.Columns.Add("Edit");
			grd.RootTable.Columns["Edit"].Key = "Edit";
			grd.RootTable.Columns["Edit"].Caption = "Edit";
			grd.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Edit"].ButtonText = "Edit";
			grd.RootTable.Columns["Edit"].Width = 50;
			grd.RootTable.Columns["Edit"].Position = 1;
			grd.FrozenColumns = 2;
			ctrlGrdBar1_Load(null, null);
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
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
			{
				if (e.Column.Key == "ItemQty")
				{
					double unit = Conversion.ToDouble(item.Cells["VarientUnit"].Value);
					double PackUom = Conversion.ToDouble(item.Cells["PackUomEquivalent"].Value);
					if (PackUom > 0.0 && unit > 0.0)
					{
						double Qty = Conversion.ToDouble(item.Cells["ItemQty"].Value);
						double ItemWt2 = Conversion.ToDouble(item.Cells["ItemWeight"].Value);
						double NetWt = 0.0;
						NetWt = unit * Qty * PackUom * ItemWt2;
						item.Cells["NetWeight"].Value = NetWt;
					}
					else if (PackUom > 0.0 && unit == 0.0)
					{
						double num = Conversion.ToDouble(item.Cells["ItemQty"].Value);
						double ItemWt3 = Conversion.ToDouble(item.Cells["ItemWeight"].Value);
						double NetWt2 = 0.0;
						NetWt2 = num * PackUom * ItemWt3;
						item.Cells["NetWeight"].Value = NetWt2;
					}
					else if (PackUom == 0.0 && unit > 0.0)
					{
						double num2 = Conversion.ToDouble(item.Cells["ItemQty"].Value);
						double ItemWt = Conversion.ToDouble(item.Cells["ItemWeight"].Value);
						double NetWt3 = 0.0;
						NetWt3 = num2 * unit * ItemWt;
						item.Cells["NetWeight"].Value = NetWt3;
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
			CommonServices.GridAutoAdjustmentNew(grd);
			WagesItemFillFromDetail();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Expected O, but got Unknown
		//IL_0238: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0)
			{
				return;
			}
			if (e.Column.Key == "Delete")
			{
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					InvGdnDetail detail = new InvGdnDetail();
					detail.Id = Conversion.ToInt(r.Cells["Id"].Value);
					detail.WarehouseId = Conversion.ToInt(r.Cells["WarehouseId"].Value);
					detail.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					detail.ItemAttributeVarientId = Conversion.ToInt(r.Cells["AttributeVarientId"].Value);
					detail.VarientEquivalent = Conversion.ToDecimal(r.Cells["VarientUnit"].Value);
					detail.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
					detail.ItemQty = Conversion.ToDecimal(r.Cells["ItemQty"].Value);
					detail.ItemWeight = Conversion.ToDecimal(r.Cells["ItemWeight"].Value);
					detail.ItemNetWeight = Conversion.ToDecimal(r.Cells["NetWeight"].Value);
					detail.CityId = Conversion.ToInt(r.Cells["CityId"].Value);
					detail.IsFOC = Conversion.ToBool(r.Cells["IsFOC"].Value);
					detail.RemarksDetail = Conversion.ToString(r.Cells["RemarksDetail"].Value);
					detail.ActionTypeId = 3;
					lstRemoveDetailRecord.Add(detail);
					dtGrid.Rows[r.RowIndex].Delete();
					if (r != null && (int)r.RowType == 0)
					{
						r.Delete();
					}
				}
				else if (r != null && (int)r.RowType == 0)
				{
					r.Delete();
				}
				grd.UpdateData();
			}
			if (e.Column.Key == "Edit")
			{
				grd_DoubleClick(null, null);
			}
			WagesItemFillFromDetail();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_Click(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grd.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0 && grd.CurrentColumn != null && grd.CurrentColumn.Key == "IsFOC")
			{
				grd.UpdateData();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInContractorWagesGrid()
	{
		dtWagesDetail.Rows.Add(0, 0, 0, 0, "", 0, "", 0, 0, "", 0, "", 0, 0, 0, 0, 0, 0, 0, "");
		grdContractorWages.DataSource = dtWagesDetail;
	}

	private void WagesItemFillFromDetail()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grd.CurrentRow;
			if (item == null || (int)item.RowType != 0 || item.Cells.Count <= 0)
			{
				return;
			}
			dtItemForWages.Clear();
			dtVarientForWages.Clear();
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow r in rows)
			{
				if (!dtItemForWages.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Conversion.ToInt(r.Cells["ItemId"].Value)))
				{
					dtItemForWages.Rows.Add(Conversion.ToInt(r.Cells["ItemId"].Value), Conversion.ToString(r.Cells["ItemName"].Value), Conversion.ToString(r.Cells["ItemCode"].Value));
				}
				if (!dtVarientForWages.AsEnumerable().Any((DataRow row) => row.Field<int>("ItemAttributeId") == Conversion.ToInt(r.Cells["AttributeVarientId"].Value) && row.Field<int>("ItemId") == Conversion.ToInt(r.Cells["ItemId"].Value)))
				{
					decimal ItemQty = (from GridEXRow row in grd.GetRows()
						where Conversion.ToInt(row.Cells["AttributeVarientId"].Value) == Conversion.ToInt(r.Cells["AttributeVarientId"].Value) && Conversion.ToInt(row.Cells["ItemId"].Value) == Conversion.ToInt(r.Cells["ItemId"].Value)
						select (decimal)row.Cells["ItemQty"].Value).Sum();
					decimal NetWeight = (from GridEXRow row in grd.GetRows()
						where Conversion.ToInt(row.Cells["AttributeVarientId"].Value) == Conversion.ToInt(r.Cells["AttributeVarientId"].Value) && Conversion.ToInt(row.Cells["ItemId"].Value) == Conversion.ToInt(r.Cells["ItemId"].Value)
						select (decimal)row.Cells["NetWeight"].Value).Sum();
					dtVarientForWages.Rows.Add(Conversion.ToInt(r.Cells["AttributeVarientId"].Value), Conversion.ToString(r.Cells["AttributeVarient"].Value), Conversion.ToInt(r.Cells["ItemId"].Value), Conversion.ToString(r.Cells["ItemName"].Value), Conversion.ToString(r.Cells["VarientUnit"].Value), ItemQty, NetWeight);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateRowsInWagesGrid()
	{
		try
		{
			if (dtGrid.Rows.Count <= 0 || dtVarientForWages.Rows.Count <= 0)
			{
				return;
			}
			foreach (DataRow d in dtVarientForWages.Rows)
			{
				int ItemId = Conversion.ToInt(d["ItemId"]);
				string ItemName = Conversion.ToString(d["ItemName"]);
				int ItemAttributeId = Conversion.ToInt(d["ItemAttributeId"]);
				string ItemAttribute = Conversion.ToString(d["ItemAttribute"]);
				decimal VarientEquivalent = Conversion.ToDecimal(d["VarientEquivalent"]);
				decimal Qty = Conversion.ToDecimal(d["Qty"]);
				decimal NetWeight = Conversion.ToDecimal(d["NetWeight"]);
				DataTable serviceActivityByItemId = ContractorWagesRateSchedule.GetServiceActivityByItemId(2, Conversion.ToInt(d["ItemId"]), Conversion.ToDateTime((object)DocDate.Value), 0, 0);
				bool flag = false;
				foreach (DataRow r in serviceActivityByItemId.Rows)
				{
					if (grdContractorWages.GetDataRows().Length != 0)
					{
						GridEXRow[] dataRows = grdContractorWages.GetDataRows();
						foreach (GridEXRow item in dataRows)
						{
							if (Conversion.ToInt(item.Cells["ItemId"].Value) == ItemId && Conversion.ToInt(item.Cells["AttributeVarientId"].Value) == ItemAttributeId && Conversion.ToInt(item.Cells["ContractorWagesRateScheduleId"].Value) == Conversion.ToInt(r["ContractorWagesRateScheduleId"]))
							{
								item.BeginEdit();
								item.Cells["Qty"].Value = Qty;
								item.Cells["ItemNetWeight"].Value = NetWeight;
								decimal Amount2 = default(decimal);
								int ParentUomId2 = Conversion.ToInt(r["ParentUomId"]);
								decimal Rate2 = Conversion.ToDecimal(r["Rate"]);
								Amount2 = ParentUomId2 switch
								{
									2 => Rate2 * Qty, 
									3 => NetWeight / 1000m * Rate2, 
									_ => default(decimal), 
								};
								item.Cells["Amount"].Value = Amount2;
								decimal AddLess = Conversion.ToDecimal(item.Cells["AddLess"].Value);
								item.Cells["NetAmount"].Value = Amount2 + AddLess;
								item.EndEdit();
								flag = true;
								break;
							}
						}
					}
					if (!flag)
					{
						decimal Amount = default(decimal);
						int ParentUomId = Conversion.ToInt(r["ParentUomId"]);
						decimal Rate = Conversion.ToDecimal(r["Rate"]);
						Amount = ParentUomId switch
						{
							2 => Rate * Qty, 
							3 => NetWeight / 1000m * Rate, 
							_ => default(decimal), 
						};
						dtWagesDetail.Rows.Add(0, 0, 0, ItemId, ItemName, ItemAttributeId, ItemAttribute, VarientEquivalent, 0, "", Conversion.ToInt(r["ContractorWagesRateScheduleId"]), r["WagesActivity"], ParentUomId, Qty, NetWeight, Rate, Amount, 0, Amount, "");
					}
					flag = false;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdContractorWagesGridSetting()
	{
		try
		{
			grdContractorWages.RootTable.Columns["Id"].Visible = false;
			grdContractorWages.RootTable.Columns["OrderId"].Visible = false;
			grdContractorWages.RootTable.Columns["OrderWagesId"].Visible = false;
			grdContractorWages.RootTable.Columns["ItemId"].Visible = false;
			grdContractorWages.RootTable.Columns["AttributeVarientId"].Visible = false;
			grdContractorWages.RootTable.Columns["ContractorId"].Visible = false;
			grdContractorWages.RootTable.Columns["ContractorWagesRateScheduleId"].Visible = false;
			grdContractorWages.RootTable.Columns["ParentUomId"].Visible = false;
			grdContractorWages.RootTable.Columns["AttributeVarient"].Caption = "Varient";
			grdContractorWages.RootTable.Columns["VarientEquivalent"].Caption = "Unit";
			grdContractorWages.RootTable.Columns["ItemNetWeight"].Caption = "Weight";
			grdContractorWages.RootTable.Columns["Id"].EditType = (EditType)0;
			grdContractorWages.RootTable.Columns["OrderId"].EditType = (EditType)0;
			grdContractorWages.RootTable.Columns["OrderWagesId"].EditType = (EditType)0;
			grdContractorWages.RootTable.Columns["ItemId"].EditType = (EditType)0;
			grdContractorWages.RootTable.Columns["ItemName"].EditType = (EditType)0;
			grdContractorWages.RootTable.Columns["AttributeVarientId"].EditType = (EditType)0;
			grdContractorWages.RootTable.Columns["AttributeVarient"].EditType = (EditType)0;
			grdContractorWages.RootTable.Columns["VarientEquivalent"].EditType = (EditType)0;
			grdContractorWages.RootTable.Columns["ContractorId"].EditType = (EditType)0;
			grdContractorWages.RootTable.Columns["ContractorName"].EditType = (EditType)0;
			grdContractorWages.RootTable.Columns["ContractorWagesRateScheduleId"].EditType = (EditType)0;
			grdContractorWages.RootTable.Columns["ServiceActivity"].EditType = (EditType)0;
			grdContractorWages.RootTable.Columns["ParentUomId"].EditType = (EditType)0;
			grdContractorWages.RootTable.Columns["ItemNetWeight"].EditType = (EditType)0;
			grdContractorWages.RootTable.Columns["Qty"].EditType = (EditType)1;
			grdContractorWages.RootTable.Columns["Rate"].EditType = (EditType)0;
			grdContractorWages.RootTable.Columns["Amount"].EditType = (EditType)0;
			grdContractorWages.RootTable.Columns["AddLess"].EditType = (EditType)1;
			grdContractorWages.RootTable.Columns["NetAmount"].EditType = (EditType)0;
			grdContractorWages.RootTable.Columns["VarientEquivalent"].AggregateFunction = (AggregateFunction)2;
			grdContractorWages.RootTable.Columns["VarientEquivalent"].TextAlignment = (TextAlignment)3;
			grdContractorWages.RootTable.Columns["VarientEquivalent"].FormatString = "##,#.##";
			grdContractorWages.RootTable.Columns["VarientEquivalent"].TotalFormatString = "##,#.##";
			grdContractorWages.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdContractorWages.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdContractorWages.RootTable.Columns["Qty"].FormatString = "##,#.##";
			grdContractorWages.RootTable.Columns["Qty"].TotalFormatString = "##,#.##";
			grdContractorWages.RootTable.Columns["ItemNetWeight"].AggregateFunction = (AggregateFunction)2;
			grdContractorWages.RootTable.Columns["ItemNetWeight"].TextAlignment = (TextAlignment)3;
			grdContractorWages.RootTable.Columns["ItemNetWeight"].FormatString = "##,#.##";
			grdContractorWages.RootTable.Columns["ItemNetWeight"].TotalFormatString = "##,#.##";
			grdContractorWages.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdContractorWages.RootTable.Columns["Rate"].FormatString = "##,#.###";
			grdContractorWages.RootTable.Columns["Rate"].TotalFormatString = "##,#.###";
			grdContractorWages.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdContractorWages.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdContractorWages.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdContractorWages.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdContractorWages.RootTable.Columns["AddLess"].TextAlignment = (TextAlignment)3;
			grdContractorWages.RootTable.Columns["AddLess"].AggregateFunction = (AggregateFunction)2;
			grdContractorWages.RootTable.Columns["AddLess"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdContractorWages.RootTable.Columns["AddLess"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdContractorWages.RootTable.Columns["NetAmount"].TextAlignment = (TextAlignment)3;
			grdContractorWages.RootTable.Columns["NetAmount"].AggregateFunction = (AggregateFunction)2;
			grdContractorWages.RootTable.Columns["NetAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdContractorWages.RootTable.Columns["NetAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdContractorWages.RootTable.Groups.Add("ServiceActivity");
			grdContractorWages.RootTable.GroupTotals = (GroupTotals)2;
			CommonServices.GridAutoAdjustmentNew(grdContractorWages);
			grdContractorWages.RootTable.Columns.Add("DeleteAll");
			grdContractorWages.RootTable.Columns["DeleteAll"].Key = "DeleteAll";
			grdContractorWages.RootTable.Columns["DeleteAll"].Caption = "DeleteAll";
			grdContractorWages.RootTable.Columns["DeleteAll"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdContractorWages.RootTable.Columns["DeleteAll"].ButtonStyle = (ButtonStyle)5;
			grdContractorWages.RootTable.Columns["DeleteAll"].ButtonText = "DeleteAll";
			grdContractorWages.RootTable.Columns["DeleteAll"].Width = 80;
			grdContractorWages.RootTable.Columns["DeleteAll"].Position = 0;
			grdContractorWages.RootTable.Columns.Add("Delete");
			grdContractorWages.RootTable.Columns["Delete"].Key = "Delete";
			grdContractorWages.RootTable.Columns["Delete"].Caption = "X";
			grdContractorWages.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdContractorWages.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdContractorWages.RootTable.Columns["Delete"].ButtonText = "X";
			grdContractorWages.RootTable.Columns["Delete"].Width = 40;
			grdContractorWages.RootTable.Columns["Delete"].Position = 1;
			grdContractorWages.RootTable.Columns.Add("Add");
			grdContractorWages.RootTable.Columns["Add"].Key = "Add";
			grdContractorWages.RootTable.Columns["Add"].Caption = "+";
			grdContractorWages.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdContractorWages.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdContractorWages.RootTable.Columns["Add"].ButtonText = "+";
			grdContractorWages.RootTable.Columns["Add"].Width = 40;
			grdContractorWages.RootTable.Columns["Add"].Position = 2;
			grdContractorWages.FrozenColumns = 3;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdContractorWages_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0362: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdContractorWages.UpdateData();
			GridEXRow item = grdContractorWages.CurrentRow;
			if (e.Column.Key == "Qty" && item != null && (int)item.RowType == 0)
			{
				decimal Amount = default(decimal);
				decimal NetAmount2 = default(decimal);
				decimal Qty = default(decimal);
				decimal Weight = default(decimal);
				decimal Rate = default(decimal);
				decimal AddLess2 = default(decimal);
				int ParentUomId = 0;
				if (!(item.Cells["Qty"].Value.ToString() != string.Empty) || !(item.Cells["Rate"].Value.ToString() != string.Empty))
				{
					throw new Exception("Qty And Rate Should be greater than zero...");
				}
				Qty = Conversion.ToDecimal((object)item.Cells["Qty"].Value.ToString());
				DataRow foundRow = dtitem.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<int>("Id") == Conversion.ToInt(item.Cells["ItemId"].Value));
				decimal obj = ((foundRow != null) ? Conversion.ToDecimal(foundRow["ItemWeight"]) : 0m);
				decimal VarientEquivalent = Conversion.ToDecimal((object)item.Cells["VarientEquivalent"].Text.ToString());
				Weight = obj * Qty * VarientEquivalent;
				item.Cells["ItemNetWeight"].Value = Weight;
				Rate = Conversion.ToDecimal((object)item.Cells["Rate"].Text.ToString());
				AddLess2 = Conversion.ToDecimal((object)item.Cells["AddLess"].Text.ToString());
				switch (Conversion.ToInt((object)item.Cells["ParentUomId"].Text.ToString()))
				{
				case 2:
					Amount = Rate * Qty;
					item.Cells["Amount"].Value = Amount;
					NetAmount2 = Amount + AddLess2;
					item.Cells["NetAmount"].Value = NetAmount2;
					break;
				case 3:
					Amount = Weight / 1000m * Rate;
					item.Cells["Amount"].Value = Amount;
					NetAmount2 = Amount + AddLess2;
					item.Cells["NetAmount"].Value = NetAmount2;
					break;
				default:
					item.Cells["Amount"].Value = 0;
					NetAmount2 = Amount + AddLess2;
					item.Cells["NetAmount"].Value = NetAmount2;
					MessageBox.Show("ParentUom of RangeUom of Selected Wages Activity is not Defined...Please Check");
					return;
				}
			}
			if (e.Column.Key == "AddLess" && item != null && (int)item.RowType == 0)
			{
				decimal AddLess = default(decimal);
				decimal NetAmount = default(decimal);
				decimal num = Conversion.ToDecimal((object)item.Cells["Amount"].Text.ToString());
				AddLess = Conversion.ToDecimal((object)item.Cells["AddLess"].Text.ToString());
				NetAmount = num + AddLess;
				item.Cells["NetAmount"].Value = NetAmount;
			}
			grdContractorWages.UpdateData();
			CommonServices.GridAutoAdjustmentNew(grdContractorWages);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdContractorWages_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdContractorWages.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			if (e.Column.Key == "Delete")
			{
				dtWagesDetail.Rows[item.RowIndex].Delete();
				item.Delete();
				if (dtWagesDetail.Rows.Count == 0)
				{
					AddRowInContractorWagesGrid();
				}
			}
			if (e.Column.Key == "DeleteAll")
			{
				string Activity = Conversion.ToString(item.Cells["ServiceActivity"].Value);
				if (MessageBox.Show("Are you sure to Delete All Rows With Activity " + Activity + "?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				if (grdContractorWages.GetDataRows().Length != 0)
				{
					GridEXRow[] dataRows = grdContractorWages.GetDataRows();
					foreach (GridEXRow r in dataRows)
					{
						if (r != null && (int)r.RowType == 0 && Conversion.ToString(r.Cells["ServiceActivity"].Value) == Activity)
						{
							dtWagesDetail.Rows[r.RowIndex].Delete();
						}
					}
				}
				if (dtWagesDetail.Rows.Count == 0)
				{
					AddRowInContractorWagesGrid();
				}
			}
			if (e.Column.Key == "Add")
			{
				AddRowInContractorWagesGrid();
			}
			grdContractorWages.UpdateData();
			CommonServices.GridAutoAdjustmentNew(grdContractorWages);
			FillWagesGridFilterCombo();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void WagesValidationWithDetail()
	{
		try
		{
			if (grd.RowCount <= 0 || grdContractorWages.RowCount <= 0 || dtVarientForWages.Rows.Count <= 0)
			{
				return;
			}
			GridEXRow[] dataRows = grdContractorWages.GetDataRows();
			foreach (GridEXRow r in dataRows)
			{
				int itemId = Conversion.ToInt(r.Cells["ItemId"].Value);
				string item = Conversion.ToString(r.Cells["ItemName"].Value);
				string AttributeVarient = Conversion.ToString(r.Cells["AttributeVarient"].Value);
				int attributeVarientId = Conversion.ToInt(r.Cells["AttributeVarientId"].Value);
				int contractorWagesRateScheduleId = Conversion.ToInt(r.Cells["ContractorWagesRateScheduleId"].Value);
				if (itemId != 0)
				{
					if (!dtGrid.AsEnumerable().Any((DataRow row) => row.Field<int>("ItemId") == itemId))
					{
						throw new Exception("Item " + item + " in Contractor Wages Grid  does not exist in Detail Grid ");
					}
					decimal num = (from row in dtGrid.AsEnumerable()
						where row.Field<int>("ItemId") == itemId && row.Field<int>("AttributeVarientId") == attributeVarientId
						select row).Sum((DataRow row) => row.Field<decimal>("ItemQty"));
					decimal totalDetailGridQty = (from row in dtWagesDetail.AsEnumerable()
						where row.Field<int>("ItemId") == itemId && row.Field<int>("AttributeVarientId") == attributeVarientId && row.Field<int>("ContractorWagesRateScheduleId") == contractorWagesRateScheduleId
						select row).Sum((DataRow row) => row.Field<decimal>("Qty"));
					if (num != totalDetailGridQty)
					{
						throw new Exception("Qty is not equal to Detail Qty of Item " + item + " and Varient " + AttributeVarient + " in Contractor Wages Grid ");
					}
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BtnGenerateWagesRows_Click(object sender, EventArgs e)
	{
		try
		{
			WagesItemFillFromDetail();
			GenerateRowsInWagesGrid();
			FillWagesGridFilterCombo();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillWagesGridFilterCombo()
	{
		try
		{
			int Id1 = Conversion.ToInt(CmbActivityforContractorUpdateInWagesGrid.Value);
			string Id2 = Conversion.ToString((object)((Control)(object)CmbContractorForUpdateInWagesGrid).Text);
			if (grdContractorWages.GetDataRows().Count() > 0 && dtWagesDetail.Rows.Count > 0)
			{
				DataTable distinctRowsActivity = (from row in dtWagesDetail.AsEnumerable()
					group row by row.Field<string>("ServiceActivity") into @group
					select @group.First()).CopyToDataTable();
				if (dtContr.Rows.Count > 0)
				{
					DDL.BindDDLNew(dtContr, CmbContractorForUpdateInWagesGrid, "Id", "CompanyName", "ContractorName", true);
					if (Id1 > 0)
					{
						if (dtContr.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id1))
						{
							CmbContractorForUpdateInWagesGrid.Value = Id1;
						}
						else
						{
							((Control)(object)CmbContractorForUpdateInWagesGrid).Text = string.Empty;
						}
					}
				}
				else
				{
					((Control)(object)CmbContractorForUpdateInWagesGrid).Text = string.Empty;
					((UltraGridBase)CmbContractorForUpdateInWagesGrid).DataSource = null;
				}
				if (distinctRowsActivity.Rows.Count > 0)
				{
					DDL.BindDDLNew(distinctRowsActivity, CmbActivityforContractorUpdateInWagesGrid, "ContractorWagesRateScheduleId", "ServiceActivity", "ServiceActivity", true);
					if (Id2 != null && Id2 != string.Empty)
					{
						if (distinctRowsActivity.AsEnumerable().Any((DataRow row) => row.Field<string>("ServiceActivity") == Id2))
						{
							((Control)(object)CmbActivityforContractorUpdateInWagesGrid).Text = Id2;
						}
						else
						{
							((Control)(object)CmbActivityforContractorUpdateInWagesGrid).Text = string.Empty;
						}
					}
				}
				else
				{
					((Control)(object)CmbActivityforContractorUpdateInWagesGrid).Text = string.Empty;
					((UltraGridBase)CmbActivityforContractorUpdateInWagesGrid).DataSource = null;
				}
			}
			else
			{
				((Control)(object)CmbContractorForUpdateInWagesGrid).Text = string.Empty;
				((UltraGridBase)CmbContractorForUpdateInWagesGrid).DataSource = null;
				((Control)(object)CmbActivityforContractorUpdateInWagesGrid).Text = string.Empty;
				((UltraGridBase)CmbActivityforContractorUpdateInWagesGrid).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnUpdateConctratorInWages_Click(object sender, EventArgs e)
	{
		try
		{
			if (grdContractorWages.GetDataRows().Count() > 0)
			{
				GridEXRow[] dataRows;
				if ((((UltraGridBase)CmbContractorForUpdateInWagesGrid).ActiveRow == null || Conversion.ToInt(CmbContractorForUpdateInWagesGrid.Value) == 0) && (((UltraGridBase)CmbActivityforContractorUpdateInWagesGrid).ActiveRow == null || Conversion.ToInt(CmbActivityforContractorUpdateInWagesGrid.Value) == 0))
				{
					GridEXRow[] GridRows = grdContractorWages.GetDataRows();
					if (Conversion.ToInt(GridRows[0].Cells["ContractorId"].Value) != 0)
					{
						int ContrId = 0;
						string ContrName = "";
						ContrId = Conversion.ToInt(GridRows[0].Cells["ContractorId"].Value);
						ContrName = Conversion.ToString(GridRows[0].Cells["ContractorName"].Value);
						dataRows = grdContractorWages.GetDataRows();
						foreach (GridEXRow r in dataRows)
						{
							r.BeginEdit();
							r.Cells["ContractorId"].Value = ContrId;
							r.Cells["ContractorName"].Value = ContrName;
							decimal AddLess = Conversion.ToInt(r.Cells["AddLess"].Value);
							string ServiceActivityId2 = Conversion.ToString(r.Cells["ServiceActivity"].Value);
							DataTable dt = ContractorWagesRateSchedule.GetServiceActivityByItemId(2, Conversion.ToInt(r.Cells["ItemId"].Value), Conversion.ToDateTime((object)DocDate.Value), 0, ContrId);
							if (dt != null && dt.Rows.Count > 0)
							{
								DataRow foundRow = dt.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<string>("WagesActivity") == ServiceActivityId2);
								decimal Rate = ((foundRow != null) ? Conversion.ToDecimal(foundRow["Rate"]) : 0m);
								int ParentUomId = ((foundRow != null) ? Conversion.ToInt(foundRow["ParentUOMId"]) : 0);
								int ContractorWagesRateScheduleId = ((foundRow != null) ? Conversion.ToInt(foundRow["ContractorWagesRateScheduleId"]) : 0);
								r.Cells["ContractorWagesRateScheduleId"].Value = ContractorWagesRateScheduleId;
								r.Cells["Rate"].Value = Rate;
								r.Cells["ParentUomId"].Value = ParentUomId;
								decimal RowQty = Conversion.ToDecimal(r.Cells["Qty"].Value);
								switch (ParentUomId)
								{
								case 2:
								{
									decimal Amount = Rate * RowQty;
									r.Cells["Amount"].Value = Amount;
									decimal NetAmount = Amount + AddLess;
									r.Cells["NetAmount"].Value = NetAmount;
									break;
								}
								case 3:
								{
									decimal Amount2 = (decimal)Conversion.ToInt(r.Cells["ItemNetWeight"].Value) / 1000m * Rate;
									r.Cells["Amount"].Value = Amount2;
									decimal NetAmount2 = Amount2 + AddLess;
									r.Cells["NetAmount"].Value = NetAmount2;
									break;
								}
								default:
								{
									r.Cells["ParentUomId"].Value = 0;
									r.Cells["Rate"].Value = 0;
									decimal Amount3 = default(decimal);
									r.Cells["Amount"].Value = Amount3;
									decimal NetAmount3 = Amount3 + AddLess;
									r.Cells["NetAmount"].Value = NetAmount3;
									break;
								}
								}
							}
							r.EndEdit();
						}
						return;
					}
					throw new Exception("ContractorName Field is Empty in Wages Grid Row No: 01");
				}
				if (((UltraGridBase)CmbContractorForUpdateInWagesGrid).ActiveRow != null && Conversion.ToInt(CmbContractorForUpdateInWagesGrid.Value) != 0 && (((UltraGridBase)CmbActivityforContractorUpdateInWagesGrid).ActiveRow == null || Conversion.ToInt(CmbActivityforContractorUpdateInWagesGrid.Value) == 0))
				{
					int ContrId2 = Conversion.ToInt(CmbContractorForUpdateInWagesGrid.Value);
					string ContrName2 = Conversion.ToString((object)((Control)(object)CmbContractorForUpdateInWagesGrid).Text);
					dataRows = grdContractorWages.GetDataRows();
					foreach (GridEXRow r2 in dataRows)
					{
						r2.BeginEdit();
						r2.Cells["ContractorId"].Value = ContrId2;
						r2.Cells["ContractorName"].Value = ContrName2;
						decimal AddLess2 = Conversion.ToInt(r2.Cells["AddLess"].Value);
						string ServiceActivityId = Conversion.ToString(r2.Cells["ServiceActivity"].Value);
						DataTable dt2 = ContractorWagesRateSchedule.GetServiceActivityByItemId(2, Conversion.ToInt(r2.Cells["ItemId"].Value), Conversion.ToDateTime((object)DocDate.Value), 0, ContrId2);
						if (dt2 != null && dt2.Rows.Count > 0)
						{
							DataRow foundRow2 = dt2.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<string>("WagesActivity") == ServiceActivityId);
							decimal Rate2 = ((foundRow2 != null) ? Conversion.ToDecimal(foundRow2["Rate"]) : 0m);
							int ParentUomId2 = ((foundRow2 != null) ? Conversion.ToInt(foundRow2["ParentUOMId"]) : 0);
							int ContractorWagesRateScheduleId2 = ((foundRow2 != null) ? Conversion.ToInt(foundRow2["ContractorWagesRateScheduleId"]) : 0);
							r2.Cells["ContractorWagesRateScheduleId"].Value = ContractorWagesRateScheduleId2;
							r2.Cells["Rate"].Value = Rate2;
							r2.Cells["ParentUomId"].Value = ParentUomId2;
							decimal RowQty2 = Conversion.ToDecimal(r2.Cells["Qty"].Value);
							switch (ParentUomId2)
							{
							case 2:
							{
								decimal Amount4 = Rate2 * RowQty2;
								r2.Cells["Amount"].Value = Amount4;
								decimal NetAmount4 = Amount4 + AddLess2;
								r2.Cells["NetAmount"].Value = NetAmount4;
								break;
							}
							case 3:
							{
								decimal Amount5 = (decimal)Conversion.ToInt(r2.Cells["ItemNetWeight"].Value) / 1000m * Rate2;
								r2.Cells["Amount"].Value = Amount5;
								decimal NetAmount5 = Amount5 + AddLess2;
								r2.Cells["NetAmount"].Value = NetAmount5;
								break;
							}
							default:
							{
								r2.Cells["ParentUomId"].Value = 0;
								r2.Cells["Rate"].Value = 0;
								decimal Amount6 = default(decimal);
								r2.Cells["Amount"].Value = Amount6;
								decimal NetAmount6 = Amount6 + AddLess2;
								r2.Cells["NetAmount"].Value = NetAmount6;
								break;
							}
							}
						}
						r2.EndEdit();
					}
					return;
				}
				if ((((UltraGridBase)CmbContractorForUpdateInWagesGrid).ActiveRow == null || Conversion.ToInt(CmbContractorForUpdateInWagesGrid.Value) == 0) && ((UltraGridBase)CmbActivityforContractorUpdateInWagesGrid).ActiveRow != null && Conversion.ToInt(CmbActivityforContractorUpdateInWagesGrid.Value) != 0)
				{
					CmbContractorForUpdateInWagesGrid.Focus();
					throw new Exception("ContractorName Field is Empty");
				}
				int ContrId3 = Conversion.ToInt(CmbContractorForUpdateInWagesGrid.Value);
				string ContrName3 = Conversion.ToString((object)((Control)(object)CmbContractorForUpdateInWagesGrid).Text);
				string ActivtyName = Conversion.ToString((object)((Control)(object)CmbActivityforContractorUpdateInWagesGrid).Text);
				dataRows = grdContractorWages.GetDataRows();
				foreach (GridEXRow r3 in dataRows)
				{
					if (!(Conversion.ToString(r3.Cells["ServiceActivity"].Value) == ActivtyName))
					{
						continue;
					}
					r3.BeginEdit();
					r3.Cells["ContractorId"].Value = ContrId3;
					r3.Cells["ContractorName"].Value = ContrName3;
					decimal AddLess3 = Conversion.ToInt(r3.Cells["AddLess"].Value);
					DataTable dt3 = ContractorWagesRateSchedule.GetServiceActivityByItemId(2, Conversion.ToInt(r3.Cells["ItemId"].Value), Conversion.ToDateTime((object)DocDate.Value), 0, ContrId3);
					if (dt3 != null && dt3.Rows.Count > 0)
					{
						DataRow foundRow3 = dt3.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<string>("WagesActivity") == Conversion.ToString(r3.Cells["ServiceActivity"].Value));
						decimal Rate3 = ((foundRow3 != null) ? Conversion.ToDecimal(foundRow3["Rate"]) : 0m);
						int ParentUomId3 = ((foundRow3 != null) ? Conversion.ToInt(foundRow3["ParentUOMId"]) : 0);
						int ContractorWagesRateScheduleId3 = ((foundRow3 != null) ? Conversion.ToInt(foundRow3["ContractorWagesRateScheduleId"]) : 0);
						r3.Cells["ContractorWagesRateScheduleId"].Value = ContractorWagesRateScheduleId3;
						r3.Cells["Rate"].Value = Rate3;
						r3.Cells["ParentUomId"].Value = ParentUomId3;
						decimal RowQty3 = Conversion.ToDecimal(r3.Cells["Qty"].Value);
						switch (ParentUomId3)
						{
						case 2:
						{
							decimal Amount7 = Rate3 * RowQty3;
							r3.Cells["Amount"].Value = Amount7;
							decimal NetAmount7 = Amount7 + AddLess3;
							r3.Cells["NetAmount"].Value = NetAmount7;
							break;
						}
						case 3:
						{
							decimal Amount8 = (decimal)Conversion.ToInt(r3.Cells["ItemNetWeight"].Value) / 1000m * Rate3;
							r3.Cells["Amount"].Value = Amount8;
							decimal NetAmount8 = Amount8 + AddLess3;
							r3.Cells["NetAmount"].Value = NetAmount8;
							break;
						}
						default:
						{
							r3.Cells["ParentUomId"].Value = 0;
							r3.Cells["Rate"].Value = 0;
							decimal Amount9 = default(decimal);
							r3.Cells["Amount"].Value = Amount9;
							decimal NetAmount9 = Amount9 + AddLess3;
							r3.Cells["NetAmount"].Value = NetAmount9;
							break;
						}
						}
					}
					r3.EndEdit();
				}
				return;
			}
			throw new Exception("Wages Grid Record not Found");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UpdateWagesRateForAllRows()
	{
		try
		{
			if (grdContractorWages.GetDataRows().Length == 0)
			{
				return;
			}
			GridEXRow[] dataRows = grdContractorWages.GetDataRows();
			foreach (GridEXRow r in dataRows)
			{
				int ContrId = Conversion.ToInt(r.Cells["ContractorId"].Value);
				Conversion.ToString(r.Cells["ContractorName"].Value);
				int ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
				string ServiceActivityId = Conversion.ToString(r.Cells["ServiceActivity"].Value);
				if (ItemId <= 0)
				{
					continue;
				}
				r.BeginEdit();
				decimal AddLess = Conversion.ToInt(r.Cells["AddLess"].Value);
				DataTable dt = ContractorWagesRateSchedule.GetServiceActivityByItemId(2, ItemId, Conversion.ToDateTime((object)DocDate.Value), 0, ContrId);
				if (dt != null && dt.Rows.Count > 0)
				{
					DataRow foundRow = dt.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<string>("WagesActivity") == ServiceActivityId);
					decimal Rate = ((foundRow != null) ? Conversion.ToDecimal(foundRow["Rate"]) : 0m);
					int ParentUomId = ((foundRow != null) ? Conversion.ToInt(foundRow["ParentUOMId"]) : 0);
					int ContractorWagesRateScheduleId = ((foundRow != null) ? Conversion.ToInt(foundRow["ContractorWagesRateScheduleId"]) : 0);
					r.Cells["ContractorWagesRateScheduleId"].Value = ContractorWagesRateScheduleId;
					r.Cells["Rate"].Value = Rate;
					r.Cells["ParentUomId"].Value = ParentUomId;
					decimal RowQty = Conversion.ToDecimal(r.Cells["Qty"].Value);
					switch (ParentUomId)
					{
					case 2:
					{
						decimal Amount = Rate * RowQty;
						r.Cells["Amount"].Value = Amount;
						decimal NetAmount = Amount + AddLess;
						r.Cells["NetAmount"].Value = NetAmount;
						break;
					}
					case 3:
					{
						decimal Amount2 = (decimal)Conversion.ToInt(r.Cells["ItemNetWeight"].Value) / 1000m * Rate;
						r.Cells["Amount"].Value = Amount2;
						decimal NetAmount2 = Amount2 + AddLess;
						r.Cells["NetAmount"].Value = NetAmount2;
						break;
					}
					default:
					{
						r.Cells["ParentUomId"].Value = 0;
						r.Cells["Rate"].Value = 0;
						decimal Amount3 = default(decimal);
						r.Cells["Amount"].Value = Amount3;
						decimal NetAmount3 = Amount3 + AddLess;
						r.Cells["NetAmount"].Value = NetAmount3;
						break;
					}
					}
				}
				else
				{
					r.Cells["ParentUomId"].Value = 0;
					r.Cells["Rate"].Value = 0;
					decimal Amount4 = default(decimal);
					r.Cells["Amount"].Value = Amount4;
					decimal NetAmount4 = Amount4 + AddLess;
					r.Cells["NetAmount"].Value = NetAmount4;
				}
				r.EndEdit();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (txtdocno.Text.Trim() == "" || txtdocno.Text.Trim() == "0" || txtdocno.Text.Trim() == null)
		{
			MessageBox.Show("DocNo Field is Required");
			txtdocno.Focus();
			return false;
		}
		if (((UltraGridBase)comsupplier).ActiveRow == null || Conversion.ToInt(comsupplier.Value) == 0)
		{
			MessageBox.Show("CustomerName Field is Required");
			comsupplier.Focus();
			return false;
		}
		if (txtRefPartyName.Text.Trim() == "" || txtRefPartyName.Text.Trim() == null)
		{
			MessageBox.Show("Ref Party Name Field is Required");
			txtRefPartyName.Focus();
			return false;
		}
		if (txtRefPartyAddress.Text.Trim() == "" || txtRefPartyAddress.Text.Trim() == null)
		{
			MessageBox.Show("Ref Party Address Field is Required");
			txtRefPartyAddress.Focus();
			return false;
		}
		if (txtRefPartyCellNo.Text.Trim() == "" || txtRefPartyCellNo.Text.Trim() == null)
		{
			MessageBox.Show("Ref Party CellNo Field is Required");
			txtRefPartyCellNo.Focus();
			return false;
		}
		if (Conversion.ToDecimal((object)txtFreightAmountHeader.Text) != 0m && Conversion.ToInt(CmbTransporterAc.Value) == 0)
		{
			MessageBox.Show("Carriage Ac is Required when Carriage amount is greater than Zero");
			CmbTransporterAc.Focus();
			return false;
		}
		if (Conversion.ToDecimal((object)txtFreightAmountHeader.Text) == 0m && Conversion.ToInt(CmbTransporterAc.Value) != 0)
		{
			MessageBox.Show("Carriage Amount is Required when Carriage Ac is Selected");
			txtFreightAmountHeader.Focus();
			return false;
		}
		if (((UltraGridBase)combdeliverytrm).ActiveRow == null || Conversion.ToInt(combdeliverytrm.Value) == 0)
		{
			MessageBox.Show("Delivery Term Field is Required");
			combdeliverytrm.Focus();
			return false;
		}
		if (((UltraGridBase)CmbVehicleType).ActiveRow == null || Conversion.ToInt(CmbVehicleType.Value) == 0)
		{
			MessageBox.Show("Vehicle Type Term Field is Required");
			CmbVehicleType.Focus();
			return false;
		}
		if (((Control)(object)CmbVehicleNo).Text.Trim() == "" || ((Control)(object)CmbVehicleNo).Text.Trim() == null || Conversion.ToString((object)((Control)(object)CmbVehicleNo).Text) == string.Empty)
		{
			MessageBox.Show("Vehicle No Field is Required");
			CmbVehicleNo.Focus();
			return false;
		}
		return true;
	}

	private bool FormValidationDetail()
	{
		if (((UltraGridBase)comsupplier).ActiveRow == null || Conversion.ToInt(comsupplier.Value) == 0)
		{
			MessageBox.Show("Supplier Field is Required");
			comsupplier.Focus();
			return false;
		}
		if (((UltraGridBase)comWarehouse).ActiveRow == null || Conversion.ToInt(comWarehouse.Value) == 0)
		{
			MessageBox.Show("Warehouse Field is Required");
			comWarehouse.Focus();
			return false;
		}
		if (((UltraGridBase)comItem).ActiveRow == null || Conversion.ToInt(comItem.Value) == 0)
		{
			MessageBox.Show("Item Name Field is Required");
			comItem.Focus();
			return false;
		}
		if (((UltraGridBase)CmbAttributeVarient).ActiveRow == null || Conversion.ToInt(CmbAttributeVarient.Value) == 0)
		{
			MessageBox.Show("Attribute Varient Field is Required");
			CmbAttributeVarient.Focus();
			return false;
		}
		if (txtQty.Text.Trim() == "" || txtQty.Text.Trim() == "0")
		{
			MessageBox.Show("Qty Field is Required");
			txtQty.Focus();
			return false;
		}
		if (txtItemWeight.Text.Trim() == "" || txtItemWeight.Text.Trim() == "0")
		{
			MessageBox.Show("Item Weight Field is Required");
			txtItemWeight.Focus();
			return false;
		}
		if (((UltraGridBase)comjobLot).ActiveRow == null || Conversion.ToInt(comjobLot.Value) == 0)
		{
			MessageBox.Show("Job/Lot Field is Required");
			comjobLot.Focus();
			return false;
		}
		if (((UltraGridBase)txtCity).ActiveRow == null || Conversion.ToInt(txtCity.Value) == 0)
		{
			MessageBox.Show("City Field is Required");
			txtCity.Focus();
			return false;
		}
		return true;
	}

	private void DisableFields()
	{
		try
		{
			((Control)(object)comWarehouse).Enabled = false;
			((Control)(object)comItem).Enabled = false;
			((Control)(object)comjobLot).Enabled = false;
			txtQty.Enabled = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void EnablesFields()
	{
		try
		{
			((Control)(object)comWarehouse).Enabled = true;
			((Control)(object)comItem).Enabled = true;
			((Control)(object)comjobLot).Enabled = true;
			txtQty.Enabled = true;
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
			lstRemoveDetailRecord.Clear();
			EnablesFields();
			Id = 0;
			ItemId = 0;
			chkisFreeOfCost.Checked = false;
			Approved = false;
			AT = new Attachment();
			((Control)(object)CmbTransporterAc).Text = string.Empty;
			txtManualNo.Text = string.Empty;
			txtSupplierReference.Text = string.Empty;
			TxtDistance.Text = string.Empty;
			txtremarks.Text = string.Empty;
			txtFreightAmountHeader.Text = string.Empty;
			txtRefPartyName.Text = string.Empty;
			txtRefPartyAddress.Text = string.Empty;
			txtRefPartyCellNo.Text = string.Empty;
			txtgatepassno.Text = string.Empty;
			((Control)(object)CmbVehicleNo).Text = string.Empty;
			((Control)(object)combdeliverytrm).Text = string.Empty;
			txtDriverName.Text = string.Empty;
			txtDriverCellNo.Text = string.Empty;
			txtCNICNO.Text = string.Empty;
			txtgpdate.Value = DateTime.Now;
			((Control)(object)comsupplier).Text = string.Empty;
			((Control)(object)comsupplier).Enabled = true;
			dtWagesDetail.Rows.Clear();
			grdContractorWages.ClearStructure();
			AddRowInContractorWagesGrid();
			grdContractorWages.RetrieveStructure();
			grdContractorWagesGridSetting();
			((Control)(object)comItem).Text = string.Empty;
			txtQty.Text = string.Empty;
			DocDate.Focus();
			dtGrid.Rows.Clear();
			if (ChkResetOnSave.Checked)
			{
				OptionResetFields();
			}
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			btnDelete.Visible = false;
			btnAdd.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			ConfigurationDefault();
			DocumentNo();
			FillWagesGridFilterCombo();
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
			txtQty.Text = string.Empty;
			txtItemWeight.Text = string.Empty;
			txtNetWeight.Text = string.Empty;
			if (ChkResetOnSave.Checked)
			{
				OptionResetFields();
			}
			comItem.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OptionResetFields()
	{
		((Control)(object)comItem).Text = string.Empty;
		((Control)(object)comjobLot).Text = string.Empty;
		((Control)(object)comWarehouse).Text = string.Empty;
		txtgatepassno.Text = string.Empty;
		((Control)(object)CmbVehicleNo).Text = string.Empty;
		((Control)(object)txtCity).Text = string.Empty;
		comItem.Focus();
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			vehicleTypefill();
			Project();
			braches();
			suppliercustomer();
			TransporterAcFill();
			bindWareHouse();
			item();
			JobLotBind();
			BinCity();
			BindVehicles();
			dtForGridComboFill();
			ConfigurationDefault();
			FillWagesGridFilterCombo();
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

	private void InvfrmInvSaleInvoiceDirect_FormClosing(object sender, FormClosingEventArgs e)
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

	private void Insert()
	{
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Expected O, but got Unknown
		//IL_03ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b3: Expected O, but got Unknown
		//IL_0981: Unknown result type (might be due to invalid IL or missing references)
		//IL_0988: Expected O, but got Unknown
		//IL_0ed4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0edb: Expected O, but got Unknown
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
			if (Conversion.ToDecimal((object)txtFreightAmountHeader.Text.Trim()) > 0m)
			{
				int num = Conversion.ToInt(CmbTransporterAc.Value);
				if (num == 0)
				{
					CmbTransporterAc.Focus();
					throw new Exception("Transporter Account field Required");
				}
				int SupGlId = Conversion.ToInt((object)txtSupplierGLId.Text);
				if (num == SupGlId)
				{
					throw new Exception("Transporter Account can not be same as Customer Please check");
				}
			}
			InvGdn obj = new InvGdn();
			if (Id > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				obj.Id = Id;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			WagesValidationWithDetail();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = Conversion.ToInt((object)UserAccount.BranchesId);
			obj.ProjectsId = Conversion.ToInt((object)UserAccount.BranchesId);
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.DocumentTypeId = 1866;
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			obj.ApprovedDate = DateTime.Now;
			obj.EntryUserId = UserAccount.ID;
			obj.ModifyUserId = UserAccount.ID;
			obj.IsApproved = false;
			obj.OrderId = 0;
			obj.DocDate = DocDate.Value;
			obj.DocNo = Conversion.ToInt((object)txtdocno.Text);
			obj.ReferenceNo = txtSupplierReference.Text.Trim();
			obj.ManualNo = txtManualNo.Text.Trim();
			obj.SupplierCustomerId = Conversion.ToInt(comsupplier.Value);
			obj.RemarksHeader = txtremarks.Text.Trim();
			obj.ReferencPartyName = Conversion.ToString((object)txtRefPartyName.Text);
			obj.ReferencPartyAddress = Conversion.ToString((object)txtRefPartyAddress.Text);
			obj.ReferencPartyCellNo = Conversion.ToString((object)txtRefPartyCellNo.Text);
			obj.Distance = Conversion.ToString((object)TxtDistance.Text);
			obj.TransporterId = Conversion.ToInt(CmbTransporterAc.Value);
			obj.CarriageAmount = Conversion.ToDecimal((object)txtFreightAmountHeader.Text);
			obj.DeliveryTerm = Conversion.ToString((object)((Control)(object)combdeliverytrm).Text);
			obj.GpNo = Conversion.ToInt((object)txtgatepassno.Text);
			obj.GpDate = Conversion.ToDateTime((object)txtgpdate.Value);
			obj.VehicleTypeId = Conversion.ToInt(CmbVehicleType.Value);
			obj.VehicleNo = Conversion.ToString((object)((Control)(object)CmbVehicleNo).Text);
			obj.DriverName = Conversion.ToString((object)txtDriverName.Text);
			obj.DriverCellNo = Conversion.ToString((object)txtDriverCellNo.Text);
			obj.DriverCNICNo = Conversion.ToString((object)txtCNICNO.Text);
			obj.invGdnDetailList = new List<InvGdnDetail>();
			obj.InvGDNWagesDetailList = new List<InvGDNWagesDetail>();
			obj.AttachmentsList = new List<DMSAttachments>();
			decimal GrossWt = default(decimal);
			bool FOCExist = false;
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				InvGdnDetail detail = new InvGdnDetail();
				detail.Id = Conversion.ToInt(r2.Cells["Id"].Value);
				if (detail.Id > 0)
				{
					detail.ActionTypeId = 2;
				}
				else
				{
					detail.ActionTypeId = 1;
				}
				detail.LineId = r2.RowIndex + 1;
				detail.OrderTypeId = 0;
				detail.OrderId = 0;
				detail.OrderDetailId = 0;
				detail.DoOrderDetailId = 0;
				if (!FOCExist)
				{
					FOCExist = !Conversion.ToBool(r2.Cells["IsFoc"].Value);
				}
				if (Conversion.ToInt(r2.Cells["WarehouseId"].Value) == 0)
				{
					throw new Exception("Warehouse Required in Detail Grid And row No: " + (r2.RowIndex + 1));
				}
				detail.WarehouseId = Conversion.ToInt(r2.Cells["WarehouseId"].Value);
				if (Conversion.ToInt(r2.Cells["ItemId"].Value) == 0)
				{
					throw new Exception("Item Required in Detail Grid And row No: " + (r2.RowIndex + 1));
				}
				detail.ItemId = Conversion.ToInt(r2.Cells["ItemId"].Value);
				detail.ScheduleId = Conversion.ToInt(r2.Cells["ScheduleId"].Value);
				if (Conversion.ToInt(r2.Cells["AttributeVarientId"].Value) == 0 && Conversion.ToInt(r2.Cells["PackUomId"].Value) == 0)
				{
					throw new Exception("AttributeVarient or PackUomId Required in Detail Grid And row No: " + (r2.RowIndex + 1));
				}
				detail.ItemUomId = Conversion.ToInt(r2.Cells["PackUomId"].Value);
				detail.ItemAttributeVarientId = Conversion.ToInt(r2.Cells["AttributeVarientId"].Value);
				if (Conversion.ToInt(r2.Cells["AttributeVarientId"].Value) > 0 && Conversion.ToDecimal(r2.Cells["VarientUnit"].Value) == 0m)
				{
					throw new Exception("VarientUnit Required in Detail Grid And row No: " + (r2.RowIndex + 1));
				}
				detail.VarientEquivalent = Conversion.ToDecimal(r2.Cells["VarientUnit"].Value);
				if (Conversion.ToInt(r2.Cells["JobLotId"].Value) == 0)
				{
					throw new Exception("JobLot Required in Detail Grid And row No: " + (r2.RowIndex + 1));
				}
				detail.JobLotId = Conversion.ToInt(r2.Cells["JobLotId"].Value);
				if (Conversion.ToDecimal(r2.Cells["ItemQty"].Value) == 0m)
				{
					throw new Exception("ItemQty Required in Detail Grid And row No: " + (r2.RowIndex + 1));
				}
				detail.ItemQty = Conversion.ToDecimal(r2.Cells["ItemQty"].Value);
				if (Conversion.ToDecimal(r2.Cells["ItemWeight"].Value) == 0m)
				{
					throw new Exception("ItemWeight Required in Detail Grid And row No: " + (r2.RowIndex + 1));
				}
				detail.ItemWeight = Conversion.ToDecimal(r2.Cells["ItemWeight"].Value);
				if (Conversion.ToDecimal(r2.Cells["NetWeight"].Value) == 0m)
				{
					throw new Exception("NetWeight Required in Detail Grid And row No: " + (r2.RowIndex + 1));
				}
				detail.ItemNetWeight = Conversion.ToDecimal(r2.Cells["NetWeight"].Value);
				GrossWt += detail.ItemNetWeight;
				detail.CityId = Conversion.ToInt(r2.Cells["CityId"].Value);
				detail.IsFOC = Conversion.ToBool(r2.Cells["IsFOC"].Value);
				detail.RemarksDetail = Conversion.ToString(r2.Cells["RemarksDetail"].Value);
				obj.invGdnDetailList.Add(detail);
			}
			if (obj.invGdnDetailList.Count == 0)
			{
				throw new Exception("At least enter value/quantity in one of the rows of detail ");
			}
			if (lstRemoveDetailRecord.Count > 0)
			{
				for (int j = 0; j < lstRemoveDetailRecord.Count; j++)
				{
					obj.invGdnDetailList.Add(lstRemoveDetailRecord[j]);
				}
			}
			if (!FOCExist)
			{
				throw new Exception("Not all records can be FreeOfCost in Detail Grid: ");
			}
			rows = grdContractorWages.GetDataRows();
			foreach (GridEXRow r in rows)
			{
				if (Conversion.ToInt(r.Cells["ContractorWagesRateScheduleId"].Value) > 0 || Conversion.ToInt(r.Cells["ContractorId"].Value) > 0 || Conversion.ToInt(r.Cells["ItemId"].Value) > 0 || Conversion.ToInt(r.Cells["AttributeVarientId"].Value) > 0)
				{
					InvGDNWagesDetail d = new InvGDNWagesDetail();
					d.Id = Conversion.ToInt(r.Cells["Id"].Value);
					d.SaleOrderId = Conversion.ToInt(r.Cells["OrderId"].Value);
					d.SaleOrderWagesId = Conversion.ToInt(r.Cells["OrderWagesId"].Value);
					if (r.Cells["ItemId"].Text == string.Empty || Conversion.ToInt(r.Cells["ItemId"].Value) == 0)
					{
						MessageBox.Show("Item  Field Required in Grid Contractor Wages Row No: " + (r.RowIndex + 1));
						return;
					}
					d.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					if (r.Cells["AttributeVarientId"].Text == string.Empty || Conversion.ToInt(r.Cells["AttributeVarientId"].Value) == 0)
					{
						MessageBox.Show("AttributeVarient  Field Required in Grid Contractor Wages Row No: " + (r.RowIndex + 1));
						return;
					}
					d.ItemAttributeVarientId = Conversion.ToInt(r.Cells["AttributeVarientId"].Value);
					d.VarientEquivalent = Conversion.ToDecimal(r.Cells["VarientEquivalent"].Value);
					if (r.Cells["ContractorId"].Text == string.Empty || Conversion.ToInt(r.Cells["ContractorId"].Value) == 0)
					{
						MessageBox.Show("Contractor  Field Required in Grid Contractor Wages Row No: " + (r.RowIndex + 1));
						return;
					}
					d.ContractorId = Conversion.ToInt(r.Cells["ContractorId"].Value);
					if (r.Cells["ContractorWagesRateScheduleId"].Text == string.Empty || Conversion.ToInt(r.Cells["ContractorWagesRateScheduleId"].Value) == 0)
					{
						MessageBox.Show("Service Activity Field Required in Grid Contractor Wages Row No: " + (r.RowIndex + 1));
						return;
					}
					d.ContractorWagesRateScheduleId = Conversion.ToInt(r.Cells["ContractorWagesRateScheduleId"].Value);
					if (r.Cells["Amount"].Text == string.Empty || Conversion.ToDecimal(r.Cells["Amount"].Value) == 0m)
					{
						MessageBox.Show("Amount Field Required in Grid Contractor Wages Row No: " + (r.RowIndex + 1));
						return;
					}
					d.ParentUomId = Conversion.ToInt((object)r.Cells["ParentUomId"].Text);
					d.Qty = Conversion.ToDecimal((object)r.Cells["Qty"].Text);
					d.Rate = Conversion.ToDecimal((object)r.Cells["Rate"].Text);
					d.ItemNetWeight = Conversion.ToDecimal((object)r.Cells["ItemNetWeight"].Text);
					d.AddLess = Conversion.ToDecimal((object)r.Cells["AddLess"].Text);
					d.NetAmount = Conversion.ToDecimal((object)r.Cells["NetAmount"].Text);
					d.Amount = Conversion.ToDecimal((object)r.Cells["Amount"].Text);
					if (Conversion.ToString((object)r.Cells["Remarks"].Text) != string.Empty)
					{
						d.Remarks = Conversion.ToString((object)r.Cells["Remarks"].Text);
					}
					else
					{
						d.Remarks = "Activity: " + Conversion.ToString((object)r.Cells["ServiceActivity"].Text) + " Rate: " + Conversion.ToString((object)r.Cells["Rate"].Text) + " Amount: " + Conversion.ToString((object)r.Cells["Amount"].Text) + " AddLess: " + Conversion.ToString((object)r.Cells["AddLess"].Text) + " NetAmount: " + Conversion.ToString((object)r.Cells["NetAmount"].Text);
					}
					obj.InvGDNWagesDetailList.Add(d);
				}
			}
			for (int i = 0; i < Attachment.dtAttachments.Rows.Count; i++)
			{
				DMSAttachments attach = new DMSAttachments();
				attach.Attachment = Path.GetFileName(Attachment.dtAttachments.Rows[i][1].ToString());
				attach.ScreenName = base.Name;
				obj.AttachmentsList.Add(attach);
				string fileSavePath = Path.Combine(clsGlobalVariables.AttachmentFolderPath, attach.Attachment);
				if (!File.Exists(fileSavePath))
				{
					File.Copy(Attachment.dtAttachments.Rows[i][1].ToString(), fileSavePath, overwrite: true);
				}
			}
			int sucess = InvGdn.Save(obj);
			if (Id == 0)
			{
				MessageBox.Show("Record Saved Successfully [" + obj.DocNo + "] ");
			}
			else
			{
				MessageBox.Show("Record Update Successfully [" + obj.DocNo + "] ");
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
					}
				}
			}
			_ = WagesStatus;
			Reset();
			if (ChkPrintslip.Checked)
			{
				GenerateCustomerSlip(sucess);
			}
		}
		catch (Exception ex2)
		{
			MessageBox.Show(ex2.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BtnSave_Click(object sender, EventArgs e)
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

	public void ReadById_Update(int ID)
	{
		try
		{
			Id = Conversion.ToInt((object)ID);
			dtWagesDetail.Rows.Clear();
			grdContractorWages.ClearStructure();
			InvGdn invPI = InvGdn.ReadById(Id);
			if (invPI == null)
			{
				return;
			}
			tabControl1.SelectedIndex = 0;
			DocDate.Value = invPI.DocDate;
			txtdocno.Text = invPI.DocNo.ToString();
			txtSupplierReference.Text = invPI.ReferenceNo;
			TxtDistance.Text = invPI.Distance;
			txtManualNo.Text = invPI.ManualNo;
			comsupplier.Value = invPI.SupplierCustomerId;
			txtremarks.Text = invPI.RemarksHeader;
			((Control)(object)combdeliverytrm).Text = invPI.DeliveryTerm;
			txtRefPartyName.Text = invPI.ReferencPartyName;
			txtRefPartyAddress.Text = invPI.ReferencPartyAddress;
			txtRefPartyCellNo.Text = invPI.ReferencPartyCellNo;
			CmbTransporterAc.Value = invPI.TransporterId;
			txtFreightAmountHeader.Text = invPI.CarriageAmount.ToString(clsGlobalVariables.stringFormatsingle);
			txtgatepassno.Text = Conversion.ToString((object)invPI.GpNo);
			txtgpdate.Value = Conversion.ToDateTime((object)invPI.GpDate);
			CmbVehicleType.Value = Conversion.ToInt((object)invPI.VehicleTypeId);
			((Control)(object)CmbVehicleNo).Text = Conversion.ToString((object)invPI.VehicleNo);
			txtDriverName.Text = Conversion.ToString((object)invPI.DriverName);
			txtDriverCellNo.Text = Conversion.ToString((object)invPI.DriverCellNo);
			txtCNICNO.Text = Conversion.ToString((object)invPI.DriverCNICNo);
			combranches.Value = invPI.BranchesId;
			comproject.Value = invPI.ProjectsId;
			Approved = invPI.IsApproved;
			dtGrid.Rows.Clear();
			for (int j = 0; j < invPI.invGdnDetailList.Count; j++)
			{
				dtGrid.Rows.Add(invPI.invGdnDetailList[j].Id, invPI.invGdnDetailList[j].WarehouseId, invPI.invGdnDetailList[j].WareHouseName, invPI.invGdnDetailList[j].ItemId, invPI.invGdnDetailList[j].ItemName, invPI.invGdnDetailList[j].ItemCode, invPI.invGdnDetailList[j].ScheduleId, invPI.invGdnDetailList[j].ItemUomId, invPI.invGdnDetailList[j].PackUom, Conversion.ToDecimal((object)invPI.invGdnDetailList[j].PackEquivalent), invPI.invGdnDetailList[j].ItemAttributeVarientId, invPI.invGdnDetailList[j].ItemAttributeVarient, invPI.invGdnDetailList[j].VarientEquivalent, invPI.invGdnDetailList[j].JobLotId, invPI.invGdnDetailList[j].JobLotDescription, invPI.invGdnDetailList[j].ItemQty, invPI.invGdnDetailList[j].ItemQty, invPI.invGdnDetailList[j].ItemWeight, invPI.invGdnDetailList[j].ItemNetWeight, invPI.invGdnDetailList[j].ItemNetWeight, invPI.invGdnDetailList[j].CityId, invPI.invGdnDetailList[j].CityName, invPI.invGdnDetailList[j].IsFOC, invPI.invGdnDetailList[j].RemarksDetail);
			}
			if (invPI.InvGDNWagesDetailList.Count > 0)
			{
				dtWagesDetail.Clear();
				for (int i = 0; i < invPI.InvGDNWagesDetailList.Count; i++)
				{
					dtWagesDetail.Rows.Add(invPI.InvGDNWagesDetailList[i].Id, invPI.InvGDNWagesDetailList[i].SaleOrderId, invPI.InvGDNWagesDetailList[i].SaleOrderWagesId, invPI.InvGDNWagesDetailList[i].ItemId, invPI.InvGDNWagesDetailList[i].ItemName, invPI.InvGDNWagesDetailList[i].ItemAttributeVarientId, invPI.InvGDNWagesDetailList[i].ItemAttributeVarient, invPI.InvGDNWagesDetailList[i].VarientEquivalent, invPI.InvGDNWagesDetailList[i].ContractorId, invPI.InvGDNWagesDetailList[i].ContractorName, invPI.InvGDNWagesDetailList[i].ContractorWagesRateScheduleId, invPI.InvGDNWagesDetailList[i].ServiceActivity, invPI.InvGDNWagesDetailList[i].ParentUomId, invPI.InvGDNWagesDetailList[i].Qty, invPI.InvGDNWagesDetailList[i].ItemNetWeight, invPI.InvGDNWagesDetailList[i].Rate, invPI.InvGDNWagesDetailList[i].Amount, invPI.InvGDNWagesDetailList[i].AddLess, invPI.InvGDNWagesDetailList[i].NetAmount, invPI.InvGDNWagesDetailList[i].Remarks);
				}
			}
			else
			{
				AddRowInContractorWagesGrid();
			}
			grd.DataSource = dtGrid;
			grd.RetrieveStructure();
			grdSettings();
			grdContractorWages.DataSource = dtWagesDetail;
			grdContractorWages.RetrieveStructure();
			grdContractorWagesGridSetting();
			List<DMSAttachments> byID = DMSAttachments.GetByID(Id, base.Name);
			AT = new Attachment();
			foreach (DMSAttachments Attachment in byID)
			{
				TempAttachments TA = new TempAttachments();
				TA.Id = 0;
				TA.Path = Attachment.Attachment;
				AT.lst.Add(TA);
			}
			btnSave.Visible = false;
			btnUpdate.Visible = true;
			btnDelete.Visible = true;
			FillWagesGridFilterCombo();
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
			if (Approved)
			{
				throw new Exception("Record Not Delete beacause Record has approved");
			}
			if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				InvGdn.DeleteByID(UserAccount.OrganizationId, UserAccount.CompanyId, 1866, UserAccount.ID, Id);
				MessageBox.Show("Deleted Successfully");
				Reset();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtdocno_TextChanged(object sender, EventArgs e)
	{
	}

	private void btnUpdate_Click(object sender, EventArgs e)
	{
		try
		{
			if (Approved)
			{
				throw new Exception("Record Not Update beacause Record has approved");
			}
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryComboBind()
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		try
		{
			dtCustomerHistory.Rows.Clear();
			int Id = Conversion.ToInt(CmbCustomerHistory.Value);
			DataTable dt = InvGdn.GetDataForDropDownFromGdn(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Activity = "Supplier_Customer"
			});
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtCustomerHistory.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["name"], dt.Rows[i]["OtherDescription"]);
				}
				if (dtCustomerHistory.Rows.Count > 0)
				{
					if (RdCustomerHistorySearchByName.Checked)
					{
						DDL.BindDDL(dtCustomerHistory, CmbCustomerHistory, "Id", "PartyName", "Party Name", ZeroIndex: true);
					}
					else
					{
						DDL.BindDDL(dtCustomerHistory, CmbCustomerHistory, "Id", "PartyCode", "Party Code", ZeroIndex: true);
					}
					if (Id > 0)
					{
						if (dtCustomerHistory.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
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
			}
			else
			{
				((UltraGridBase)CmbCustomerHistory).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ParameterFill()
	{
		try
		{
			DataTable dt = CommonServices.DateType();
			DDL.BindDDL(dt, cmbDateTypeHistory, "Id", "Parameters", "Parameters", ZeroIndex: false);
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

	private void btnshow_Click(object sender, EventArgs e)
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
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters PI = new ReportsParameters();
			PI.OrganizationId = UserAccount.OrganizationId;
			PI.CompanyId = UserAccount.CompanyId;
			PI.BranchesId = UserAccount.BranchesId;
			PI.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			PI.DocumentTypeId = 1866;
			PI.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			PI.FromDate = Conversion.ToDateTime((object)txtFromdateHistory.Value);
			PI.ToDate = Conversion.ToDateTime((object)txtToDateHistory.Value);
			PI.FromDocNo = Conversion.ToInt((object)txtFromNoHistory.Text);
			PI.ToDocNo = Conversion.ToInt((object)txtToDocNoHistory.Text);
			PI.SupplierCustomerId = Conversion.ToInt(CmbCustomerHistory.Value);
			if (!PI.CanViewAllRecord)
			{
				PI.EntryUser = UserAccount.ID;
			}
			DataTable dthistory = InvGdn.FormHistory(PI);
			if (dthistory.Rows.Count > 0)
			{
				DataTable dtResult = new DataTable();
				dtResult.Columns.Add("Id", typeof(int));
				dtResult.Columns.Add("DocumentTypeId", typeof(int));
				dtResult.Columns.Add("DocDate", typeof(DateTime));
				dtResult.Columns.Add("DocNo", typeof(string));
				dtResult.Columns.Add("SupplierCustomerId", typeof(int));
				dtResult.Columns.Add("PartyName", typeof(string));
				dtResult.Columns.Add("PartyCode", typeof(string));
				dtResult.Columns.Add("ReferenceNo", typeof(string));
				dtResult.Columns.Add("Distance", typeof(string));
				dtResult.Columns.Add("ReferencPartyName", typeof(string));
				dtResult.Columns.Add("ReferencPartyAddress", typeof(string));
				dtResult.Columns.Add("ReferencPartyCellNo", typeof(string));
				dtResult.Columns.Add("DeliveryTerm", typeof(string));
				dtResult.Columns.Add("TransporterName", typeof(string));
				dtResult.Columns.Add("CarriageAmount", typeof(decimal));
				dtResult.Columns.Add("GpNo", typeof(string));
				dtResult.Columns.Add("GpDate", typeof(DateTime));
				dtResult.Columns.Add("VehicleType", typeof(string));
				dtResult.Columns.Add("VehicleNo", typeof(string));
				dtResult.Columns.Add("DriverName", typeof(string));
				dtResult.Columns.Add("DriverCNICNo", typeof(string));
				dtResult.Columns.Add("DriverCellNo", typeof(string));
				dtResult.Columns.Add("IsApproved", typeof(bool));
				dtResult.Columns.Add("EntryDate", typeof(DateTime));
				dtResult.Columns.Add("EntryUserName", typeof(string));
				dtResult.Columns.Add("ModifyDate", typeof(DateTime));
				dtResult.Columns.Add("ModifyUserName", typeof(string));
				dtResult.Columns.Add("RemarksHeader", typeof(string));
				dtResult.Columns.Add("NoOfAttachments", typeof(int));
				foreach (DataRow row in dthistory.Rows)
				{
					dtResult.Rows.Add(row["Id"], row["DocumentTypeId"], row["DocDate"], row["DocNo"], row["SupplierCustomerId"], row["CustomerName"], row["PartyCode"], row["ReferenceNo"], row["Distance"], row["ReferencPartyName"], row["ReferencPartyAddress"], row["ReferencPartyCellNo"], row["DeliveryTerm"], row["TransporterName"], row["CarriageAmount"], row["GpNo"], row["GpDate"], row["VehicleType"], row["VehicleNo"], row["DriverName"], row["DriverCNICNo"], row["DriverCellNo"], row["IsApproved"], row["EntryDate"], row["EntryUserName"], row["ModifyDate"], row["ModifyUserName"], row["RemarksHeader"], row["NoOfAttachments"]);
				}
				grdHistory.DataSource = dtResult;
				grdHistory.RetrieveStructure();
				HistoryGridSettings();
			}
			else
			{
				grdHistory.ClearStructure();
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
			grdHistory.AllowEdit = (InheritableBoolean)2;
			grdHistory.AllowDelete = (InheritableBoolean)2;
			grdHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdHistory.RootTable.Columns["Id"].Visible = false;
			grdHistory.RootTable.Columns["DocumentTypeId"].Visible = false;
			grdHistory.RootTable.Columns["SupplierCustomerId"].Visible = false;
			grdHistory.RootTable.Columns["NoOfAttachments"].Caption = "Attached";
			grdHistory.RootTable.Columns["CarriageAmount"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["CarriageAmount"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["CarriageAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdHistory.RootTable.Columns["CarriageAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustmentNew(grdHistory);
			grdHistory.RootTable.Columns.Add("Print");
			grdHistory.RootTable.Columns["Print"].Caption = "Print";
			grdHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["Print"].ButtonText = "Print";
			grdHistory.RootTable.Columns["Print"].Width = 50;
			grdHistory.RootTable.Columns.Add("Edit");
			grdHistory.RootTable.Columns["Edit"].Caption = "Edit";
			grdHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdHistory.RootTable.Columns["Edit"].Width = 50;
			grdHistory.RootTable.Columns["Edit"].Position = 0;
			grdHistory.RootTable.Columns["Print"].Position = 1;
			grdHistory.RootTable.Columns["NoOfAttachments"].Position = 2;
			grdHistory.FrozenColumns = 3;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (tabControl1.SelectedIndex == 1)
		{
			cmbDateTypeHistory.Focus();
		}
		else
		{
			DocDate.Focus();
		}
	}

	private void grdHistory_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			Id = Conversion.ToInt(grdHistory.CurrentRow.Cells["Id"].Value);
			ReadById_Update(Id);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdHistory.CurrentRow;
			if (item != null && (int)item.RowType == 0 && e.Column.Key == "NoOfAttachments")
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(item.Cells["Id"].Value), 1866);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			int RId = Conversion.ToInt(grdHistory.CurrentRow.Cells["Id"].Value);
			if (e.Column.Key == "Edit")
			{
				Reset();
				Id = RId;
				ReadById_Update(Id);
			}
			if (e.Column.Key == "Print")
			{
				GenerateCustomerSlip(Conversion.ToInt(grdHistory.CurrentRow.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdHistory_SelectionChanged(object sender, EventArgs e)
	{
		try
		{
			GetDetailGrdByHeadId();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetDetailGrdByHeadId()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		try
		{
			if (grdHistory.CurrentRow == null || (int)grdHistory.CurrentRow.RowType != 0 || grdHistory.CurrentRow.Cells.Count <= 0)
			{
				return;
			}
			InvGdn invPI = new InvGdn();
			invPI = InvGdn.ReadById(Conversion.ToInt(grdHistory.CurrentRow.Cells["Id"].Value));
			if (invPI.invGdnDetailList.Count > 0)
			{
				DataTable dtdetail = new DataTable();
				dtdetail.Columns.Add("Id", typeof(int));
				dtdetail.Columns.Add("Warehouse", typeof(string));
				dtdetail.Columns.Add("ItemId", typeof(int));
				dtdetail.Columns.Add("ItemName", typeof(string));
				dtdetail.Columns.Add("ItemCode", typeof(string));
				dtdetail.Columns.Add("Uom", typeof(string));
				dtdetail.Columns.Add("AttributeVarient", typeof(string));
				dtdetail.Columns.Add("VarientUnit", typeof(decimal));
				dtdetail.Columns.Add("JobLot", typeof(string));
				dtdetail.Columns.Add("ItemQty", typeof(decimal));
				dtdetail.Columns.Add("ItemWeight", typeof(decimal));
				dtdetail.Columns.Add("NetWeight", typeof(decimal));
				dtdetail.Columns.Add("CityName");
				dtdetail.Columns.Add("IsFOC", typeof(bool));
				dtdetail.Columns.Add("RemarksDetail");
				dtdetail.Rows.Clear();
				for (int i = 0; i < invPI.invGdnDetailList.Count; i++)
				{
					dtdetail.Rows.Add(invPI.invGdnDetailList[i].Id, invPI.invGdnDetailList[i].WareHouseName, invPI.invGdnDetailList[i].ItemId, invPI.invGdnDetailList[i].ItemName, invPI.invGdnDetailList[i].ItemCode, invPI.invGdnDetailList[i].PackUom, invPI.invGdnDetailList[i].ItemAttributeVarient, invPI.invGdnDetailList[i].VarientEquivalent, invPI.invGdnDetailList[i].JobLotDescription, invPI.invGdnDetailList[i].ItemQty, invPI.invGdnDetailList[i].ItemWeight, invPI.invGdnDetailList[i].ItemNetWeight, invPI.invGdnDetailList[i].CityName, invPI.invGdnDetailList[i].IsFOC, invPI.invGdnDetailList[i].RemarksDetail);
				}
				grdDetailHistory.DataSource = dtdetail;
				grdDetailHistory.RetrieveStructure();
				DetailGridSetting();
			}
			else
			{
				grdDetailHistory.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DetailGridSetting()
	{
		//IL_0260: Unknown result type (might be due to invalid IL or missing references)
		//IL_0287: Unknown result type (might be due to invalid IL or missing references)
		//IL_028d: Expected O, but got Unknown
		try
		{
			grdDetailHistory.RootTable.Columns["Id"].Visible = false;
			grdDetailHistory.RootTable.Columns["ItemId"].Visible = false;
			grdDetailHistory.RootTable.Columns["VarientUnit"].AggregateFunction = (AggregateFunction)2;
			grdDetailHistory.RootTable.Columns["VarientUnit"].TextAlignment = (TextAlignment)3;
			grdDetailHistory.RootTable.Columns["VarientUnit"].FormatString = "#,##0.##";
			grdDetailHistory.RootTable.Columns["VarientUnit"].TotalFormatString = "#,##0.##";
			grdDetailHistory.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdDetailHistory.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdDetailHistory.RootTable.Columns["ItemQty"].FormatString = "#,##0.##";
			grdDetailHistory.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.##";
			grdDetailHistory.RootTable.Columns["ItemWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetailHistory.RootTable.Columns["ItemWeight"].TextAlignment = (TextAlignment)3;
			grdDetailHistory.RootTable.Columns["ItemWeight"].FormatString = "#,##0.##";
			grdDetailHistory.RootTable.Columns["ItemWeight"].TotalFormatString = "#,##0.##";
			grdDetailHistory.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetailHistory.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			grdDetailHistory.RootTable.Columns["NetWeight"].FormatString = "#,##0.##";
			grdDetailHistory.RootTable.Columns["NetWeight"].TotalFormatString = "#,##0.##";
			new GridEXColumn();
			GridEXFormatCondition fc = new GridEXFormatCondition(grdDetailHistory.RootTable.Columns["IsFOC"], (ConditionOperator)0, (object)true);
			fc.FormatStyle.ForeColor = Color.Red;
			grdDetailHistory.RootTable.FormatConditions.Add(fc);
			CommonServices.GridAutoAdjustmentNew(grdDetailHistory);
			ctrlGrdBar3_Load(null, null);
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

	private void btnSlip_Click(object sender, EventArgs e)
	{
		try
		{
			GenerateCustomerSlip(Id);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateCustomerSlip(int PrintId)
	{
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Expected O, but got Unknown
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = InvGdn.GdnConcrete_Slip(UserAccount.OrganizationId, UserAccount.CompanyId, PrintId);
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(InvGdn.GdnWagesDetail_SubReport(PrintId), "InvGdn_SubReport.rpt");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "1866-InvGdnDirect_Slip.rpt", lstSubReports);
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

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdHistory).Name);
			if (fs != null)
			{
				grdHistory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
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

	private void ctrlGrdBar3_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdDetailHistory).Name);
			if (fs != null)
			{
				grdDetailHistory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void comsupplier_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (dtSupplier.Rows.Count > 0)
			{
				Conversion.ToInt(comsupplier.Value);
				_ = 0;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateWeight()
	{
		try
		{
			if (Conversion.ToInt(CmbAttributeVarient.Value) > 0 && txtQty.Text != string.Empty)
			{
				Conversion.ToDecimal((object)txtVarientEquivalent.Text);
				decimal Qty = Conversion.ToDecimal((object)txtQty.Text);
				decimal ItemWeight = Conversion.ToDecimal((object)txtItemWeight.Text);
				decimal unit = Conversion.ToDecimal((object)txtVarientEquivalent.Text);
				decimal NetWeight = default(decimal);
				if (Qty > 0m && Conversion.ToInt(comItem.Value) > 0)
				{
					if (!Conversion.ToBool(((UltraDropDownBase)comItem).SelectedRow.Cells["WithVariant"].Value))
					{
						unit = ((Conversion.ToInt(CmbPackUom.Value) <= 0) ? default(decimal) : Conversion.ToDecimal(((UltraDropDownBase)CmbPackUom).SelectedRow.Cells[2].Value));
						NetWeight = unit * Qty;
					}
					else
					{
						NetWeight = unit * Qty * ItemWeight;
					}
					txtNetWeight.Text = NetWeight.ToString("##,#.##");
				}
				else
				{
					txtNetWeight.Text = 0.ToString();
				}
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

	private void CmbPackUom_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateWeight();
			AvailableStockGetForLabel();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbAttributeVarient_Leave(object sender, EventArgs e)
	{
		try
		{
			double Equivalent = 0.0;
			if (Conversion.ToInt(CmbAttributeVarient.Value) > 0)
			{
				Equivalent = Conversion.ToDouble(((UltraDropDownBase)CmbAttributeVarient).SelectedRow.Cells["VarientEquivalent"].Value);
				txtVarientEquivalent.Text = Equivalent.ToString();
			}
			else
			{
				txtVarientEquivalent.Text = Equivalent.ToString();
			}
			CalculateWeight();
			AvailableStockGetForLabel();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtQty_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CommonServices.CommasApplyWhileTyping(sender, e);
			CalculateWeight();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AvailableStockGetForLabel()
	{
		try
		{
			int OrganizationId = UserAccount.OrganizationId;
			int CompanyId = UserAccount.CompanyId;
			int ItemId = Conversion.ToInt(comItem.Value);
			int wareId = Conversion.ToInt(comWarehouse.Value);
			int JobLotId = Conversion.ToInt(comjobLot.Value);
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

	private void combdeliverytrm_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (((Control)(object)combdeliverytrm).Text == "Ponch")
			{
				CmbTransporterAc.Value = OutwardFreightAccountId;
				return;
			}
			((Control)(object)CmbTransporterAc).Text = string.Empty;
			txtFreightAmountHeader.Text = "0";
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
			UpdateWagesRateForAllRows();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtQty_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtGrossWeight_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtEmptybagsUnit_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtwtcut_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtStockWeight_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtduedays_KeyPress(object sender, KeyPressEventArgs e)
	{
		e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
	}

	private void txtFreightAmount_KeyPress(object sender, KeyPressEventArgs e)
	{
		e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
	}

	private void txtExchangeRate_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtgatepassno_KeyPress(object sender, KeyPressEventArgs e)
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

	private void InvfrmPurchasedirectInvoice_KeyDown_1(object sender, KeyEventArgs e)
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
					cmbDateTypeHistory.Focus();
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
				if (e.Control && e.KeyCode == Keys.S && btnSave.Visible && btnSave.Enabled)
				{
					BtnSave_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.U && btnUpdate.Visible && btnUpdate.Enabled)
				{
					btnUpdate_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnNew_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefresh_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					comsupplier.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grd).Focus();
				}
				if (e.Control && e.KeyCode == Keys.Right)
				{
					if (((Control)(object)grd).Focused)
					{
						((Control)(object)grdContractorWages).Focus();
					}
					if (((Control)(object)grdContractorWages).Focused)
					{
						tabControl2.SelectedIndex = 0;
						((Control)(object)grd).Focus();
					}
					else
					{
						tabControl2.SelectedIndex = 0;
						((Control)(object)grd).Focus();
					}
				}
				if (e.Control && e.KeyCode == Keys.Up)
				{
					comWarehouse.Focus();
				}
				if (e.Control && e.KeyCode == Keys.F10)
				{
					btnAttachment_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.P)
				{
					btnSlip_Click(null, null);
				}
				if (e.Alt && (e.KeyCode == Keys.NumPad1 || e.KeyCode == Keys.D1))
				{
					btnSlip_Click(null, null);
				}
				return;
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				((Control)(object)grdHistory).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grdDetailHistory).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Right)
			{
				if (((Control)(object)grdDetailHistory).Focused)
				{
					((Control)(object)grdHistory).Focus();
				}
				else
				{
					((Control)(object)grdDetailHistory).Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				((Control)(object)grdHistory).Focus();
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
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Alt+1", "For Print Slip 1866");
			dt.Rows.Add("Ctrl+F5", "For Focus on DocDate");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+D", "For Adding an row in Focused Grid");
			dt.Rows.Add("Ctrl+Delete", "For Deleting an row of Focused Grid");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
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

	private void btnShortcutKeys_Click(object sender, EventArgs e)
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
		//IL_056e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0352: Unknown result type (might be due to invalid IL or missing references)
		//IL_0359: Expected O, but got Unknown
		//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Expected O, but got Unknown
		//IL_055b: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grd.CurrentRow;
			if (item == null || (int)item.RowType != 0 || grd.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grd.CurrentColumn.Key == "Delete")
				{
					GridEXRow r = grd.CurrentRow;
					if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
					{
						if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
						{
							return;
						}
						InvGdnDetail detail = new InvGdnDetail();
						detail.Id = Conversion.ToInt(r.Cells["Id"].Value);
						detail.OrderId = Conversion.ToInt(r.Cells["OrderId"].Value);
						detail.OrderDetailId = Conversion.ToInt(r.Cells["OrderDetailId"].Value);
						detail.WarehouseId = Conversion.ToInt(r.Cells["WarehouseId"].Value);
						detail.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
						detail.ItemAttributeVarientId = Conversion.ToInt(r.Cells["AttributeVarientId"].Value);
						detail.VarientEquivalent = Conversion.ToDecimal(r.Cells["VarientUnit"].Value);
						detail.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
						detail.ItemQty = Conversion.ToDecimal(r.Cells["ItemQty"].Value);
						detail.ItemWeight = Conversion.ToDecimal(r.Cells["ItemWeight"].Value);
						detail.ItemNetWeight = Conversion.ToDecimal(r.Cells["NetWeight"].Value);
						detail.CityId = Conversion.ToInt(r.Cells["CityId"].Value);
						detail.IsFOC = Conversion.ToBool(r.Cells["IsFOC"].Value);
						detail.RemarksDetail = Conversion.ToString(r.Cells["RemarksDetail"].Value);
						detail.ActionTypeId = 3;
						lstRemoveDetailRecord.Add(detail);
						dtGrid.Rows[r.RowIndex].Delete();
						if (r != null && (int)r.RowType == 0)
						{
							r.Delete();
						}
					}
					else if (r != null && (int)r.RowType == 0)
					{
						r.Delete();
					}
					grd.UpdateData();
					WagesItemFillFromDetail();
				}
				if (grd.CurrentColumn.Key == "Edit")
				{
					grd_DoubleClick(null, null);
				}
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				GridEXRow r2 = grd.CurrentRow;
				if (Conversion.ToInt(r2.Cells["Id"].Value) > 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					InvGdnDetail detail2 = new InvGdnDetail();
					detail2.Id = Conversion.ToInt(r2.Cells["Id"].Value);
					detail2.OrderId = Conversion.ToInt(r2.Cells["OrderId"].Value);
					detail2.OrderDetailId = Conversion.ToInt(r2.Cells["OrderDetailId"].Value);
					detail2.WarehouseId = Conversion.ToInt(r2.Cells["WarehouseId"].Value);
					detail2.ItemId = Conversion.ToInt(r2.Cells["ItemId"].Value);
					detail2.ItemAttributeVarientId = Conversion.ToInt(r2.Cells["AttributeVarientId"].Value);
					detail2.VarientEquivalent = Conversion.ToDecimal(r2.Cells["VarientUnit"].Value);
					detail2.JobLotId = Conversion.ToInt(r2.Cells["JobLotId"].Value);
					detail2.ItemQty = Conversion.ToDecimal(r2.Cells["ItemQty"].Value);
					detail2.ItemWeight = Conversion.ToDecimal(r2.Cells["ItemWeight"].Value);
					detail2.ItemNetWeight = Conversion.ToDecimal(r2.Cells["NetWeight"].Value);
					detail2.CityId = Conversion.ToInt(r2.Cells["CityId"].Value);
					detail2.IsFOC = Conversion.ToBool(r2.Cells["IsFOC"].Value);
					detail2.RemarksDetail = Conversion.ToString(r2.Cells["RemarksDetail"].Value);
					detail2.ActionTypeId = 3;
					lstRemoveDetailRecord.Add(detail2);
					dtGrid.Rows[r2.RowIndex].Delete();
					if (r2 != null && (int)r2.RowType == 0)
					{
						r2.Delete();
					}
				}
				else if (r2 != null && (int)r2.RowType == 0)
				{
					r2.Delete();
				}
				grd.UpdateData();
				WagesItemFillFromDetail();
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				grd_DoubleClick(null, null);
			}
			if (e.KeyCode == Keys.F1)
			{
				int Id = 0;
				string Name = "";
				if (grd.CurrentColumn != null && (grd.CurrentColumn.Key == "WarehouseId" || grd.CurrentColumn.Key == "Warehouse"))
				{
					GrdPopUp obj5 = new GrdPopUp(dtwareh, "Id", "WareHouseName");
					obj5.StartPosition = FormStartPosition.CenterScreen;
					obj5.ShowDialog();
					obj5.BringToFront();
					Id = Conversion.ToInt((object)obj5.ReturnId);
					Name = Conversion.ToString((object)obj5.ReturnName);
					item.Cells["WarehouseId"].Value = Id;
					item.Cells["Warehouse"].Value = Name;
					int OrganizationId3 = UserAccount.OrganizationId;
					int CompanyId3 = UserAccount.CompanyId;
					int ItemId3 = Conversion.ToInt((object)item.Cells["ItemId"].Value.ToString());
					int wareId3 = Conversion.ToInt((object)item.Cells["WareHouseId"].Value.ToString());
					int JobLotId3 = Conversion.ToInt((object)item.Cells["JobLotId"].Value.ToString());
					int AttributeVarientId3 = Conversion.ToInt((object)item.Cells["AttributeVarientId"].Value.ToString());
					if (ItemId3 > 0 || wareId3 > 0)
					{
						DataTable DtStock3 = GetAvgRatesAndStockInHand.GetStockInHandAndAvgRateFromEvaluationConcrete(OrganizationId3, CompanyId3, ItemId3, Conversion.ToDateTime((object)DocDate.Value), wareId3, JobLotId3, AttributeVarientId3);
						if (DtStock3.Rows.Count > 0)
						{
							item.Cells["AvailableQty"].Value = Conversion.ToDecimal(DtStock3.Rows[0]["QtyInHand"]).ToString();
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
				if (grd.CurrentColumn != null && (grd.CurrentColumn.Key == "AttributeVarientId" || grd.CurrentColumn.Key == "AttributeVarient"))
				{
					if (Conversion.ToInt(item.Cells["ItemId"].Value) <= 0)
					{
						MessageBox.Show("Please Select an Item First");
						return;
					}
					DataTable allForCombo = ItemAttributeVarient.GetAllForCombo(UserAccount.OrganizationId, UserAccount.CompanyId, Conversion.ToInt(item.Cells["ItemId"].Value));
					GrdPopUp obj4 = new GrdPopUp(allForCombo, "ItemAttributeVarientId", "ItemAttribute");
					obj4.StartPosition = FormStartPosition.CenterScreen;
					obj4.ShowDialog();
					obj4.BringToFront();
					Id = Conversion.ToInt((object)obj4.ReturnId);
					Name = Conversion.ToString((object)obj4.ReturnName);
					item.Cells["AttributeVarientId"].Value = Id;
					item.Cells["AttributeVarient"].Value = Name;
					DataRow foundRow = allForCombo.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<int>("ItemAttributeVarientId") == Id);
					decimal VarientEquivalent = ((foundRow != null) ? Conversion.ToDecimal(foundRow["VarientEquivalent"]) : 0m);
					item.Cells["VarientUnit"].Value = VarientEquivalent;
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
				if (grd.CurrentColumn != null && (grd.CurrentColumn.Key == "JobLotId" || grd.CurrentColumn.Key == "JobLot"))
				{
					GrdPopUp obj3 = new GrdPopUp(dtjoblot, "Id", "JobLotDescription");
					obj3.StartPosition = FormStartPosition.CenterScreen;
					obj3.ShowDialog();
					obj3.BringToFront();
					Id = Conversion.ToInt((object)obj3.ReturnId);
					Name = Conversion.ToString((object)obj3.ReturnName);
					item.Cells["JobLotId"].Value = Id;
					item.Cells["JobLot"].Value = Name;
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
				if (grd.CurrentColumn != null && (grd.CurrentColumn.Key == "CityName" || grd.CurrentColumn.Key == "CityId"))
				{
					GrdPopUp obj2 = new GrdPopUp(dtCity, "Id", "CityName");
					obj2.StartPosition = FormStartPosition.CenterScreen;
					obj2.ShowDialog();
					obj2.BringToFront();
					Id = Conversion.ToInt((object)obj2.ReturnId);
					Name = Conversion.ToString((object)obj2.ReturnName);
					item.Cells["CityId"].Value = Id;
					item.Cells["CityName"].Value = Name;
				}
				if (grd.CurrentColumn != null && grd.CurrentColumn.Key == "VehicleNo")
				{
					GrdPopUp obj = new GrdPopUp(dtVehicleNos, "Id", "VehicleNo");
					obj.StartPosition = FormStartPosition.CenterScreen;
					obj.ShowDialog();
					obj.BringToFront();
					Id = Conversion.ToInt((object)obj.ReturnId);
					Name = Conversion.ToString((object)obj.ReturnName);
					item.Cells["VehicleNo"].Value = Name;
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
			GridEXRow item = grdHistory.CurrentRow;
			if (item == null || (int)item.RowType != 0 || grdHistory.CurrentColumn == null)
			{
				return;
			}
			int CurrId = Conversion.ToInt((object)grdHistory.CurrentRow.Cells["Id"].Value.ToString());
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdHistory.CurrentColumn.Key == "Edit" && formright.DoHaveUpdateRights)
				{
					Id = CurrId;
					ReadById_Update(Id);
					comsupplier.Focus();
				}
				if (grdHistory.CurrentColumn.Key == "Print")
				{
					GenerateCustomerSlip(CurrId);
				}
				if (grdHistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(CurrId, 1866);
				}
			}
			if (e.Control && e.KeyCode == Keys.Return && formright.DoHaveUpdateRights)
			{
				Id = CurrId;
				ReadById_Update(Id);
				comsupplier.Focus();
			}
			if (e.Control && e.KeyCode == Keys.P && formright.DoHavePrintRights)
			{
				GenerateCustomerSlip(CurrId);
			}
			((Control)(object)grdHistory).Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdContractorWages_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0278: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdContractorWages.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdContractorWages.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
				if (grdContractorWages.CurrentColumn.Key == "Add")
				{
					AddRowInContractorWagesGrid();
				}
				if (grdContractorWages.CurrentColumn.Key == "DeleteAll")
				{
					string Activity = Conversion.ToString(r.Cells["ServiceActivity"].Value);
					if (MessageBox.Show("Are you sure to Delete All Rows With Activity " + Activity + "?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					if (grdContractorWages.GetDataRows().Length != 0)
					{
						GridEXRow[] dataRows = grdContractorWages.GetDataRows();
						foreach (GridEXRow row2 in dataRows)
						{
							if (row2 != null && (int)row2.RowType == 0 && Conversion.ToString(row2.Cells["ServiceActivity"].Value) == Activity)
							{
								dtWagesDetail.Rows[row2.RowIndex].Delete();
							}
						}
					}
					if (dtWagesDetail.Rows.Count == 0)
					{
						AddRowInContractorWagesGrid();
					}
				}
				if (grdContractorWages.CurrentColumn.Key == "Delete")
				{
					dtWagesDetail.Rows[r.RowIndex].Delete();
					if (r != null && (int)r.RowType == 0)
					{
						r.Delete();
					}
					if (dtWagesDetail.Rows.Count == 0)
					{
						AddRowInContractorWagesGrid();
					}
				}
				grdContractorWages.UpdateData();
			}
			if (e.Control && e.KeyCode == Keys.D)
			{
				AddRowInContractorWagesGrid();
				grdContractorWages.UpdateData();
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				dtWagesDetail.Rows[r.RowIndex].Delete();
				if (r != null && (int)r.RowType == 0)
				{
					r.Delete();
				}
				if (dtWagesDetail.Rows.Count == 0)
				{
					AddRowInContractorWagesGrid();
				}
				grdContractorWages.UpdateData();
			}
			if (e.KeyCode != Keys.F1)
			{
				return;
			}
			int Id = 0;
			string Name = "";
			if (grdContractorWages.CurrentColumn != null && (grdContractorWages.CurrentColumn.Key == "ContractorId" || grdContractorWages.CurrentColumn.Key == "ContractorName"))
			{
				GrdPopUp obj4 = new GrdPopUp(dtContr, "Id", "CompanyName");
				obj4.StartPosition = FormStartPosition.CenterScreen;
				obj4.ShowDialog();
				obj4.BringToFront();
				Id = Conversion.ToInt((object)obj4.ReturnId);
				Name = Conversion.ToString((object)obj4.ReturnName);
				r.Cells["ContractorId"].Value = Id;
				r.Cells["ContractorName"].Value = Name;
				string ServiceActivityId = Conversion.ToString(r.Cells["ServiceActivity"].Value);
				DataTable dt2 = ContractorWagesRateSchedule.GetServiceActivityByItemId(2, Conversion.ToInt(r.Cells["ItemId"].Value), Conversion.ToDateTime((object)DocDate.Value), 0, Id);
				if (dt2.Rows.Count > 0)
				{
					DataRow foundRow3 = dt2.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<string>("WagesActivity") == ServiceActivityId);
					decimal Rate3 = ((foundRow3 != null) ? Conversion.ToDecimal(foundRow3["Rate"]) : 0m);
					int ParentUomId2 = ((foundRow3 != null) ? Conversion.ToInt(foundRow3["ParentUOMId"]) : 0);
					int ContractorWagesRateScheduleId = ((foundRow3 != null) ? Conversion.ToInt(foundRow3["ContractorWagesRateScheduleId"]) : 0);
					r.Cells["ContractorWagesRateScheduleId"].Value = ContractorWagesRateScheduleId;
					r.Cells["Rate"].Value = Rate3;
					r.Cells["ParentUomId"].Value = ParentUomId2;
					decimal RowQty2 = Conversion.ToDecimal(r.Cells["Qty"].Value);
					decimal AddLess3 = Conversion.ToInt(r.Cells["AddLess"].Value);
					switch (ParentUomId2)
					{
					case 2:
					{
						decimal Amount9 = Rate3 * RowQty2;
						r.Cells["Amount"].Value = Amount9;
						decimal NetAmount6 = Amount9 + AddLess3;
						r.Cells["NetAmount"].Value = NetAmount6;
						break;
					}
					case 3:
					{
						decimal Amount10 = (decimal)Conversion.ToInt(r.Cells["ItemNetWeight"].Value) / 1000m * Rate3;
						r.Cells["Amount"].Value = Amount10;
						decimal NetAmount7 = Amount10 + AddLess3;
						r.Cells["NetAmount"].Value = NetAmount7;
						break;
					}
					default:
					{
						r.Cells["ParentUomId"].Value = 0;
						r.Cells["Rate"].Value = 0;
						decimal Amount11 = default(decimal);
						r.Cells["Amount"].Value = Amount11;
						decimal NetAmount8 = Amount11 + AddLess3;
						r.Cells["NetAmount"].Value = NetAmount8;
						MessageBox.Show("ParentUom of RangeUom of Selected Wages Activity is not Defined...Please Check");
						return;
					}
					}
				}
			}
			if (grdContractorWages.CurrentColumn != null && (grdContractorWages.CurrentColumn.Key == "ItemId" || grdContractorWages.CurrentColumn.Key == "ItemName"))
			{
				int PreviousItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
				GrdPopUp obj3 = new GrdPopUp(dtItemForWages, "Id", "ItemName");
				obj3.StartPosition = FormStartPosition.CenterScreen;
				obj3.ShowDialog();
				obj3.BringToFront();
				Id = Conversion.ToInt((object)obj3.ReturnId);
				Name = Conversion.ToString((object)obj3.ReturnName);
				r.Cells["ItemId"].Value = Id;
				r.Cells["ItemName"].Value = Name;
				if (Id != PreviousItemId)
				{
					r.Cells["AttributeVarientId"].Value = 0;
					r.Cells["AttributeVarient"].Value = "";
					r.Cells["VarientEquivalent"].Value = 0;
					r.Cells["Qty"].Value = 0;
					r.Cells["ItemNetWeight"].Value = 0;
					r.Cells["ContractorWagesRateScheduleId"].Value = 0;
					r.Cells["ServiceActivity"].Value = "";
					r.Cells["ParentUomId"].Value = 0;
					r.Cells["Rate"].Value = 0;
					r.Cells["Amount"].Value = 0;
				}
			}
			if (grdContractorWages.CurrentColumn != null && (grdContractorWages.CurrentColumn.Key == "AttributeVarientId" || grdContractorWages.CurrentColumn.Key == "AttributeVarient"))
			{
				if (Conversion.ToInt(r.Cells["ItemId"].Value) <= 0)
				{
					MessageBox.Show("Please Select an Item First");
					return;
				}
				DataRow[] matchingRows = dtVarientForWages.Select("ItemId=" + Conversion.ToInt(r.Cells["ItemId"].Value));
				if (matchingRows.Length == 0)
				{
					MessageBox.Show("Please Select an Item First");
					return;
				}
				GrdPopUp obj2 = new GrdPopUp(matchingRows.CopyToDataTable(), "ItemAttributeId", "ItemAttribute");
				obj2.StartPosition = FormStartPosition.CenterScreen;
				obj2.ShowDialog();
				obj2.BringToFront();
				Id = Conversion.ToInt((object)obj2.ReturnId);
				Name = Conversion.ToString((object)obj2.ReturnName);
				r.Cells["AttributeVarientId"].Value = Id;
				r.Cells["AttributeVarient"].Value = Name;
				if (Id > 0)
				{
					DataRow foundRow2 = dtVarientForWages.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<int>("ItemAttributeId") == Id && row.Field<int>("ItemId") == Conversion.ToInt(r.Cells["ItemId"].Value));
					decimal VarientEquivalent = ((foundRow2 != null) ? Conversion.ToDecimal(foundRow2["VarientEquivalent"]) : 0m);
					decimal CurrRowQty = ((foundRow2 != null) ? Conversion.ToDecimal(foundRow2["Qty"]) : 0m);
					decimal ItemNetWeight = ((foundRow2 != null) ? Conversion.ToDecimal(foundRow2["NetWeight"]) : 0m);
					r.Cells["Qty"].Value = CurrRowQty;
					r.Cells["ItemNetWeight"].Value = ItemNetWeight;
					r.Cells["VarientEquivalent"].Value = VarientEquivalent;
					decimal Rate2 = Conversion.ToInt(r.Cells["Rate"].Value);
					decimal AddLess2 = Conversion.ToInt(r.Cells["AddLess"].Value);
					switch (Conversion.ToInt(r.Cells["ParentUomId"].Value))
					{
					case 2:
					{
						decimal Amount5 = Rate2 * CurrRowQty;
						r.Cells["Amount"].Value = Amount5;
						break;
					}
					case 3:
					{
						decimal Amount6 = ItemNetWeight / 1000m * Rate2;
						r.Cells["Amount"].Value = Amount6;
						break;
					}
					default:
					{
						decimal Amount7 = default(decimal);
						r.Cells["Amount"].Value = Amount7;
						decimal NetAmount4 = Amount7 + AddLess2;
						r.Cells["NetAmount"].Value = NetAmount4;
						break;
					}
					}
				}
				else
				{
					r.Cells["Qty"].Value = 0;
					r.Cells["ItemNetWeight"].Value = 0;
					decimal Amount8 = default(decimal);
					r.Cells["Amount"].Value = Amount8;
					decimal NetAmount5 = Amount8 - 0m;
					r.Cells["NetAmount"].Value = NetAmount5;
				}
			}
			if (grdContractorWages.CurrentColumn != null && (grdContractorWages.CurrentColumn.Key == "ServiceActivity" || grdContractorWages.CurrentColumn.Key == "ContractorWagesRateScheduleId"))
			{
				if (Conversion.ToInt(r.Cells["ItemId"].Value) <= 0)
				{
					MessageBox.Show("Please Select an Item First");
					return;
				}
				int ContractorId = Conversion.ToInt(r.Cells["ContractorId"].Value);
				DataTable dt = ContractorWagesRateSchedule.GetServiceActivityByItemId(2, Conversion.ToInt(r.Cells["ItemId"].Value), Conversion.ToDateTime((object)DocDate.Value), 0, ContractorId);
				GrdPopUp obj = new GrdPopUp(dt, "ContractorWagesRateScheduleId", "WagesActivity");
				obj.StartPosition = FormStartPosition.CenterScreen;
				obj.ShowDialog();
				obj.BringToFront();
				Id = Conversion.ToInt((object)obj.ReturnId);
				Name = Conversion.ToString((object)obj.ReturnName);
				r.Cells["ContractorWagesRateScheduleId"].Value = Id;
				r.Cells["ServiceActivity"].Value = Name;
				if (Id > 0)
				{
					DataRow foundRow = dt.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<int>("ContractorWagesRateScheduleId") == Id);
					decimal Rate = ((foundRow != null) ? Conversion.ToDecimal(foundRow["Rate"]) : 0m);
					int ParentUomId = ((foundRow != null) ? Conversion.ToInt(foundRow["ParentUOMId"]) : 0);
					r.Cells["Rate"].Value = Rate;
					r.Cells["ParentUomId"].Value = ParentUomId;
					decimal RowQty = Conversion.ToDecimal(r.Cells["Qty"].Value);
					decimal AddLess = Conversion.ToInt(r.Cells["AddLess"].Value);
					switch (ParentUomId)
					{
					case 2:
					{
						decimal Amount = Rate * RowQty;
						r.Cells["Amount"].Value = Amount;
						decimal NetAmount = Amount + AddLess;
						r.Cells["NetAmount"].Value = NetAmount;
						break;
					}
					case 3:
					{
						decimal Amount2 = (decimal)Conversion.ToInt(r.Cells["ItemNetWeight"].Value) / 1000m * Rate;
						r.Cells["Amount"].Value = Amount2;
						decimal NetAmount2 = Amount2 + AddLess;
						r.Cells["NetAmount"].Value = NetAmount2;
						break;
					}
					default:
					{
						r.Cells["ContractorWagesRateScheduleId"].Value = 0;
						r.Cells["ServiceActivity"].Value = "";
						r.Cells["ParentUomId"].Value = 0;
						r.Cells["Rate"].Value = 0;
						decimal Amount3 = default(decimal);
						r.Cells["Amount"].Value = Amount3;
						decimal NetAmount3 = Amount3 + AddLess;
						r.Cells["NetAmount"].Value = NetAmount3;
						MessageBox.Show("ParentUom of RangeUom of Selected Wages Activity is not Defined...Please Check");
						return;
					}
					}
				}
				else
				{
					r.Cells["ContractorWagesRateScheduleId"].Value = 0;
					r.Cells["ServiceActivity"].Value = "";
					r.Cells["ParentUomId"].Value = 0;
					r.Cells["Rate"].Value = 0;
					decimal Amount4 = default(decimal);
					r.Cells["Amount"].Value = Amount4;
					r.Cells["NetAmount"].Value = Amount4;
				}
			}
			grdContractorWages.UpdateData();
			CommonServices.GridAutoAdjustmentNew(grdContractorWages);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RdPartyByName_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (dtSupplier.Rows.Count > 0)
			{
				int Id = 0;
				if (Conversion.ToInt(comsupplier.Value) > 0)
				{
					Id = Conversion.ToInt(comsupplier.Value);
				}
				if (RdPartyByName.Checked)
				{
					DDL.BindDDL(dtSupplier, comsupplier, "Id", "CompanyName", "Party Name", ZeroIndex: true);
				}
				else
				{
					DDL.BindDDL(dtSupplier, comsupplier, "Id", "PartyCode", "Party Code", ZeroIndex: true);
				}
				if (Id > 0)
				{
					comsupplier.Value = Id;
				}
				comsupplier.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RdCustomerHistorySearchByName_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (dtCustomerHistory.Rows.Count > 0)
			{
				int Id = 0;
				if (Conversion.ToInt(CmbCustomerHistory.Value) > 0)
				{
					Id = Conversion.ToInt(CmbCustomerHistory.Value);
				}
				if (RdCustomerHistorySearchByName.Checked)
				{
					DDL.BindDDL(dtCustomerHistory, CmbCustomerHistory, "Id", "PartyName", "Party Name", ZeroIndex: true);
				}
				else
				{
					DDL.BindDDL(dtCustomerHistory, CmbCustomerHistory, "Id", "PartyCode", "Party Code", ZeroIndex: true);
				}
				if (Id > 0)
				{
					CmbCustomerHistory.Value = Id;
				}
				CmbCustomerHistory.Focus();
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
		//IL_05e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ee: Expected O, but got Unknown
		//IL_061b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0625: Expected O, but got Unknown
		//IL_0631: Unknown result type (might be due to invalid IL or missing references)
		//IL_063b: Expected O, but got Unknown
		//IL_065d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0667: Expected O, but got Unknown
		//IL_069f: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a9: Expected O, but got Unknown
		//IL_06e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06eb: Expected O, but got Unknown
		//IL_0702: Unknown result type (might be due to invalid IL or missing references)
		//IL_070c: Expected O, but got Unknown
		//IL_0770: Unknown result type (might be due to invalid IL or missing references)
		//IL_077a: Expected O, but got Unknown
		//IL_07a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b1: Expected O, but got Unknown
		//IL_07b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_07bc: Expected O, but got Unknown
		//IL_0836: Unknown result type (might be due to invalid IL or missing references)
		//IL_0840: Expected O, but got Unknown
		//IL_0841: Unknown result type (might be due to invalid IL or missing references)
		//IL_084b: Expected O, but got Unknown
		//IL_084c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0856: Expected O, but got Unknown
		//IL_0954: Unknown result type (might be due to invalid IL or missing references)
		//IL_095e: Expected O, but got Unknown
		//IL_09a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ab: Expected O, but got Unknown
		//IL_09e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ed: Expected O, but got Unknown
		//IL_09ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f8: Expected O, but got Unknown
		//IL_0ae0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aea: Expected O, but got Unknown
		//IL_0b22: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b2c: Expected O, but got Unknown
		//IL_0b9b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ba5: Expected O, but got Unknown
		//IL_0bb1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bbb: Expected O, but got Unknown
		//IL_1258: Unknown result type (might be due to invalid IL or missing references)
		//IL_1262: Expected O, but got Unknown
		//IL_126f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1279: Expected O, but got Unknown
		//IL_15df: Unknown result type (might be due to invalid IL or missing references)
		//IL_15e9: Expected O, but got Unknown
		//IL_15f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1600: Expected O, but got Unknown
		//IL_9196: Unknown result type (might be due to invalid IL or missing references)
		//IL_91a0: Expected O, but got Unknown
		//IL_91c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_91ce: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdContractorWages_DesignTimeLayout = new GridEXLayout();
		Appearance appearance101 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance169 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance171 = new Appearance();
		Appearance appearance172 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance195 = new Appearance();
		Appearance appearance196 = new Appearance();
		Appearance appearance197 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance189 = new Appearance();
		Appearance appearance198 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance168 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance180 = new Appearance();
		Appearance appearance181 = new Appearance();
		Appearance appearance192 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance182 = new Appearance();
		Appearance appearance183 = new Appearance();
		Appearance appearance184 = new Appearance();
		Appearance appearance185 = new Appearance();
		Appearance appearance186 = new Appearance();
		Appearance appearance187 = new Appearance();
		Appearance appearance188 = new Appearance();
		Appearance appearance190 = new Appearance();
		Appearance appearance191 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance199 = new Appearance();
		Appearance appearance200 = new Appearance();
		Appearance appearance201 = new Appearance();
		GridEXLayout grdDetailHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance152 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance157 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance193 = new Appearance();
		Appearance appearance194 = new Appearance();
		Appearance appearance202 = new Appearance();
		Appearance appearance203 = new Appearance();
		Appearance appearance204 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.grd = new GridEX();
		this.panel16 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.tabPage4 = new System.Windows.Forms.TabPage();
		this.grdContractorWages = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.CmbContractorForUpdateInWagesGrid = new UltraCombo();
		this.label66 = new System.Windows.Forms.Label();
		this.label65 = new System.Windows.Forms.Label();
		this.label49 = new System.Windows.Forms.Label();
		this.CmbActivityforContractorUpdateInWagesGrid = new UltraCombo();
		this.BtnUpdateConctratorInWages = new System.Windows.Forms.Button();
		this.BtnGenerateWagesRows = new System.Windows.Forms.Button();
		this.label16 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.CmbPackUom = new UltraCombo();
		this.label58 = new System.Windows.Forms.Label();
		this.chkisFreeOfCost = new System.Windows.Forms.CheckBox();
		this.btnAdd = new System.Windows.Forms.Button();
		this.label43 = new System.Windows.Forms.Label();
		this.txtNetWeight = new System.Windows.Forms.TextBox();
		this.CmbAttributeVarient = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.comjobLot = new UltraCombo();
		this.label33 = new System.Windows.Forms.Label();
		this.txtRemarksDetail = new System.Windows.Forms.TextBox();
		this.txtQty = new System.Windows.Forms.TextBox();
		this.label9 = new System.Windows.Forms.Label();
		this.label38 = new System.Windows.Forms.Label();
		this.txtItemWeight = new System.Windows.Forms.TextBox();
		this.txtVarientEquivalent = new System.Windows.Forms.TextBox();
		this.rdbtnItemCode = new System.Windows.Forms.RadioButton();
		this.rdbtnItemName = new System.Windows.Forms.RadioButton();
		this.txtCity = new UltraCombo();
		this.label8 = new System.Windows.Forms.Label();
		this.lblBalance = new System.Windows.Forms.Label();
		this.btnCancelUpdateDetial = new System.Windows.Forms.Button();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.comWarehouse = new UltraCombo();
		this.comItem = new UltraCombo();
		this.label40 = new System.Windows.Forms.Label();
		this.label32 = new System.Windows.Forms.Label();
		this.label31 = new System.Windows.Forms.Label();
		this.label15 = new System.Windows.Forms.Label();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.RdPartyByCode = new System.Windows.Forms.RadioButton();
		this.RdPartyByName = new System.Windows.Forms.RadioButton();
		this.txtCNICNO = new System.Windows.Forms.TextBox();
		this.txtFreightAmountHeader = new System.Windows.Forms.TextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.label89 = new System.Windows.Forms.Label();
		this.CmbVehicleType = new UltraCombo();
		this.CmbVehicleNo = new UltraCombo();
		this.CmbTransporterAc = new UltraCombo();
		this.label4 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.txtSupplierGLId = new System.Windows.Forms.TextBox();
		this.label11 = new System.Windows.Forms.Label();
		this.label22 = new System.Windows.Forms.Label();
		this.label30 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.label34 = new System.Windows.Forms.Label();
		this.txtDriverCellNo = new System.Windows.Forms.TextBox();
		this.txtDriverName = new System.Windows.Forms.TextBox();
		this.label36 = new System.Windows.Forms.Label();
		this.TxtDistance = new System.Windows.Forms.TextBox();
		this.txtRefPartyCellNo = new System.Windows.Forms.TextBox();
		this.txtRefPartyAddress = new System.Windows.Forms.TextBox();
		this.txtRefPartyName = new System.Windows.Forms.TextBox();
		this.txtgatepassno = new System.Windows.Forms.TextBox();
		this.txtgpdate = new System.Windows.Forms.DateTimePicker();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.label47 = new System.Windows.Forms.Label();
		this.DocDate = new System.Windows.Forms.DateTimePicker();
		this.label5 = new System.Windows.Forms.Label();
		this.label48 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.combdeliverytrm = new UltraCombo();
		this.txtManualNo = new System.Windows.Forms.TextBox();
		this.txtSupplierReference = new System.Windows.Forms.TextBox();
		this.txtremarks = new System.Windows.Forms.TextBox();
		this.label24 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.comsupplier = new UltraCombo();
		this.label29 = new System.Windows.Forms.Label();
		this.panel11 = new System.Windows.Forms.Panel();
		this.label17 = new System.Windows.Forms.Label();
		this.ChkPrintslip = new System.Windows.Forms.CheckBox();
		this.ChkResetOnSave = new System.Windows.Forms.CheckBox();
		this.comproject = new UltraCombo();
		this.combranches = new UltraCombo();
		this.label13 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
		this.btnSlip = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
		this.btnLoadGdn = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
		this.btnshortcutkeys = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel17 = new System.Windows.Forms.Panel();
		this.panel19 = new System.Windows.Forms.Panel();
		this.grdDetailHistory = new GridEX();
		this.panel18 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.label55 = new System.Windows.Forms.Label();
		this.panel10 = new System.Windows.Forms.Panel();
		this.panel8 = new System.Windows.Forms.Panel();
		this.grdHistory = new GridEX();
		this.panel7 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label78 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.groupBox4 = new System.Windows.Forms.GroupBox();
		this.RdCustomerHistorySearchByCode = new System.Windows.Forms.RadioButton();
		this.RdCustomerHistorySearchByName = new System.Windows.Forms.RadioButton();
		this.label20 = new System.Windows.Forms.Label();
		this.btnshow = new System.Windows.Forms.Button();
		this.label73 = new System.Windows.Forms.Label();
		this.CmbCustomerHistory = new UltraCombo();
		this.label75 = new System.Windows.Forms.Label();
		this.cmbDateTypeHistory = new UltraCombo();
		this.txtToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.label76 = new System.Windows.Forms.Label();
		this.txtToDocNoHistory = new System.Windows.Forms.TextBox();
		this.txtFromdateHistory = new System.Windows.Forms.DateTimePicker();
		this.label3 = new System.Windows.Forms.Label();
		this.label77 = new System.Windows.Forms.Label();
		this.txtFromNoHistory = new System.Windows.Forms.TextBox();
		this.panel9 = new System.Windows.Forms.Panel();
		this.label54 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.BtnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.BtnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel1.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.tabPage3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel16.SuspendLayout();
		this.tabPage4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdContractorWages).BeginInit();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbContractorForUpdateInWagesGrid).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbActivityforContractorUpdateInWagesGrid).BeginInit();
		this.panel3.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAttributeVarient).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comjobLot).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.txtCity).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comWarehouse).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comItem).BeginInit();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbVehicleType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbVehicleNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTransporterAc).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combdeliverytrm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comsupplier).BeginInit();
		this.panel11.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.comproject).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combranches).BeginInit();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel17.SuspendLayout();
		this.panel19.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetailHistory).BeginInit();
		this.panel18.SuspendLayout();
		this.panel10.SuspendLayout();
		this.panel8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).BeginInit();
		this.panel7.SuspendLayout();
		this.panel5.SuspendLayout();
		this.groupBox4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDateTypeHistory).BeginInit();
		this.panel9.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1174, 670);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel1);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1166, 641);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.Controls.Add(this.tabControl2);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.toolStrip1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(3, 3);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1160, 635);
		this.panel1.TabIndex = 7;
		this.tabControl2.Controls.Add(this.tabPage3);
		this.tabControl2.Controls.Add(this.tabPage4);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl2.Location = new System.Drawing.Point(0, 326);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(1160, 309);
		this.tabControl2.TabIndex = 0;
		this.tabPage3.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.tabPage3.Controls.Add(this.panel16);
		this.tabPage3.Location = new System.Drawing.Point(4, 25);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage3.Size = new System.Drawing.Size(1152, 280);
		this.tabPage3.TabIndex = 0;
		this.tabPage3.Text = "Detail";
		this.tabPage3.UseVisualStyleBackColor = true;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(3, 24);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1146, 253);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.CellUpdated += new ColumnActionEventHandler(grd_CellUpdated);
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grd).Click += new System.EventHandler(grd_Click);
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grd_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		this.panel16.BackColor = System.Drawing.Color.Teal;
		this.panel16.Controls.Add(this.label23);
		this.panel16.Controls.Add(this.ctrlGrdBar1);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel16.Location = new System.Drawing.Point(3, 3);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(1146, 21);
		this.panel16.TabIndex = 33;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(5, 2);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(46, 17);
		this.label23.TabIndex = 35;
		this.label23.Text = "Detail";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1106, -1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(37, 21);
		this.ctrlGrdBar1.TabIndex = 33;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.tabPage4.Controls.Add((System.Windows.Forms.Control)(object)this.grdContractorWages);
		this.tabPage4.Controls.Add(this.panel4);
		this.tabPage4.Location = new System.Drawing.Point(4, 25);
		this.tabPage4.Name = "tabPage4";
		this.tabPage4.Size = new System.Drawing.Size(1152, 280);
		this.tabPage4.TabIndex = 1;
		this.tabPage4.Text = "Contractor Wages";
		this.tabPage4.UseVisualStyleBackColor = true;
		((ScrollableBase)this.grdContractorWages).BackColor = System.Drawing.Color.White;
		grdContractorWages_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdContractorWages.DesignTimeLayout = grdContractorWages_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdContractorWages).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdContractorWages).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdContractorWages.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdContractorWages).Location = new System.Drawing.Point(0, 27);
		((System.Windows.Forms.Control)(object)this.grdContractorWages).Name = "grdContractorWages";
		((ScrollableBase)this.grdContractorWages).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdContractorWages).Size = new System.Drawing.Size(1152, 253);
		((System.Windows.Forms.Control)(object)this.grdContractorWages).TabIndex = 0;
		this.grdContractorWages.TotalRow = (InheritableBoolean)1;
		this.grdContractorWages.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdContractorWages).VisualStyle = (VisualStyle)4;
		this.grdContractorWages.CellUpdated += new ColumnActionEventHandler(grdContractorWages_CellUpdated);
		this.grdContractorWages.ColumnButtonClick += new ColumnActionEventHandler(grdContractorWages_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdContractorWages).KeyDown += new System.Windows.Forms.KeyEventHandler(grdContractorWages_KeyDown);
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbContractorForUpdateInWagesGrid);
		this.panel4.Controls.Add(this.label66);
		this.panel4.Controls.Add(this.label65);
		this.panel4.Controls.Add(this.label49);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbActivityforContractorUpdateInWagesGrid);
		this.panel4.Controls.Add(this.BtnUpdateConctratorInWages);
		this.panel4.Controls.Add(this.BtnGenerateWagesRows);
		this.panel4.Controls.Add(this.label16);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 0);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1152, 27);
		this.panel4.TabIndex = 17;
		this.CmbContractorForUpdateInWagesGrid.AllowNull = (DefaultableBoolean)1;
		this.CmbContractorForUpdateInWagesGrid.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbContractorForUpdateInWagesGrid.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbContractorForUpdateInWagesGrid.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance101).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.Appearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance102).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance102).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance102).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance102;
		((AppearanceBase)appearance103).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance103;
		((SpecialBoxBase)((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance115).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance115).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance115).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance116).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance116;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance117).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance118;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance119).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance120).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance120).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance120).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance120).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance122).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance123).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbContractorForUpdateInWagesGrid).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbContractorForUpdateInWagesGrid).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbContractorForUpdateInWagesGrid.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbContractorForUpdateInWagesGrid).Location = new System.Drawing.Point(908, 0);
		((System.Windows.Forms.Control)(object)this.CmbContractorForUpdateInWagesGrid).Name = "CmbContractorForUpdateInWagesGrid";
		((System.Windows.Forms.Control)(object)this.CmbContractorForUpdateInWagesGrid).Size = new System.Drawing.Size(168, 26);
		((System.Windows.Forms.Control)(object)this.CmbContractorForUpdateInWagesGrid).TabIndex = 157;
		this.label66.AutoSize = true;
		this.label66.Font = new System.Drawing.Font("Segoe UI", 10f, System.Drawing.FontStyle.Bold);
		this.label66.Location = new System.Drawing.Point(829, 4);
		this.label66.Name = "label66";
		this.label66.Size = new System.Drawing.Size(81, 19);
		this.label66.TabIndex = 159;
		this.label66.Text = "Contractor";
		this.label65.AutoSize = true;
		this.label65.Font = new System.Drawing.Font("Segoe UI", 10f, System.Drawing.FontStyle.Bold);
		this.label65.Location = new System.Drawing.Point(634, 4);
		this.label65.Name = "label65";
		this.label65.Size = new System.Drawing.Size(60, 19);
		this.label65.TabIndex = 160;
		this.label65.Text = "Activity";
		this.label49.Font = new System.Drawing.Font("Tahoma", 8f, System.Drawing.FontStyle.Bold);
		this.label49.ForeColor = System.Drawing.Color.White;
		this.label49.Location = new System.Drawing.Point(190, -1);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(446, 28);
		this.label49.TabIndex = 158;
		this.label49.Text = "Click to update the selected contractor for the selected activity or the same contractor (as in the 1st row) when both combos are empty, in all rows of grid";
		this.CmbActivityforContractorUpdateInWagesGrid.AllowNull = (DefaultableBoolean)1;
		this.CmbActivityforContractorUpdateInWagesGrid.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbActivityforContractorUpdateInWagesGrid.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbActivityforContractorUpdateInWagesGrid.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance125).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.Appearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance126).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance126).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance126).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance126;
		((AppearanceBase)appearance127).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance127;
		((SpecialBoxBase)((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance128).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance128).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance128).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance129).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance129;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance130).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance131;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance133).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance134).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance134).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance134).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance134;
		((AppearanceBase)appearance135).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance136).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance137;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbActivityforContractorUpdateInWagesGrid).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbActivityforContractorUpdateInWagesGrid).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbActivityforContractorUpdateInWagesGrid.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbActivityforContractorUpdateInWagesGrid).Location = new System.Drawing.Point(695, 0);
		((System.Windows.Forms.Control)(object)this.CmbActivityforContractorUpdateInWagesGrid).Name = "CmbActivityforContractorUpdateInWagesGrid";
		((System.Windows.Forms.Control)(object)this.CmbActivityforContractorUpdateInWagesGrid).Size = new System.Drawing.Size(132, 26);
		((System.Windows.Forms.Control)(object)this.CmbActivityforContractorUpdateInWagesGrid).TabIndex = 156;
		this.BtnUpdateConctratorInWages.BackColor = System.Drawing.Color.FromArgb(0, 64, 64);
		this.BtnUpdateConctratorInWages.FlatAppearance.BorderColor = System.Drawing.Color.White;
		this.BtnUpdateConctratorInWages.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnUpdateConctratorInWages.Font = new System.Drawing.Font("Verdana", 9.5f, System.Drawing.FontStyle.Bold);
		this.BtnUpdateConctratorInWages.ForeColor = System.Drawing.Color.White;
		this.BtnUpdateConctratorInWages.Location = new System.Drawing.Point(1078, 1);
		this.BtnUpdateConctratorInWages.Name = "BtnUpdateConctratorInWages";
		this.BtnUpdateConctratorInWages.Size = new System.Drawing.Size(71, 26);
		this.BtnUpdateConctratorInWages.TabIndex = 127;
		this.BtnUpdateConctratorInWages.Text = "Update";
		this.BtnUpdateConctratorInWages.UseVisualStyleBackColor = false;
		this.BtnUpdateConctratorInWages.Click += new System.EventHandler(BtnUpdateConctratorInWages_Click);
		this.BtnGenerateWagesRows.BackColor = System.Drawing.Color.FromArgb(0, 64, 64);
		this.BtnGenerateWagesRows.FlatAppearance.BorderColor = System.Drawing.Color.White;
		this.BtnGenerateWagesRows.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnGenerateWagesRows.Font = new System.Drawing.Font("Verdana", 9.5f, System.Drawing.FontStyle.Bold);
		this.BtnGenerateWagesRows.ForeColor = System.Drawing.Color.White;
		this.BtnGenerateWagesRows.Location = new System.Drawing.Point(98, 1);
		this.BtnGenerateWagesRows.Name = "BtnGenerateWagesRows";
		this.BtnGenerateWagesRows.Size = new System.Drawing.Size(86, 26);
		this.BtnGenerateWagesRows.TabIndex = 127;
		this.BtnGenerateWagesRows.Text = "Generate";
		this.BtnGenerateWagesRows.UseVisualStyleBackColor = false;
		this.BtnGenerateWagesRows.Click += new System.EventHandler(BtnGenerateWagesRows_Click);
		this.label16.Font = new System.Drawing.Font("Tahoma", 8f, System.Drawing.FontStyle.Bold);
		this.label16.ForeColor = System.Drawing.Color.White;
		this.label16.Location = new System.Drawing.Point(4, 0);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(103, 26);
		this.label16.TabIndex = 126;
		this.label16.Text = "Click To Auto Generate Rows";
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add(this.groupBox1);
		this.panel3.Controls.Add(this.groupBox2);
		this.panel3.Controls.Add(this.panel11);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.comproject);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.combranches);
		this.panel3.Controls.Add(this.label13);
		this.panel3.Controls.Add(this.label18);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.panel3.Location = new System.Drawing.Point(0, 25);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1160, 301);
		this.panel3.TabIndex = 0;
		this.groupBox1.BackColor = System.Drawing.Color.White;
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPackUom);
		this.groupBox1.Controls.Add(this.label58);
		this.groupBox1.Controls.Add(this.chkisFreeOfCost);
		this.groupBox1.Controls.Add(this.btnAdd);
		this.groupBox1.Controls.Add(this.label43);
		this.groupBox1.Controls.Add(this.txtNetWeight);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbAttributeVarient);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.comjobLot);
		this.groupBox1.Controls.Add(this.label33);
		this.groupBox1.Controls.Add(this.txtRemarksDetail);
		this.groupBox1.Controls.Add(this.txtQty);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Controls.Add(this.label38);
		this.groupBox1.Controls.Add(this.txtItemWeight);
		this.groupBox1.Controls.Add(this.txtVarientEquivalent);
		this.groupBox1.Controls.Add(this.rdbtnItemCode);
		this.groupBox1.Controls.Add(this.rdbtnItemName);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.txtCity);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add(this.lblBalance);
		this.groupBox1.Controls.Add(this.btnCancelUpdateDetial);
		this.groupBox1.Controls.Add(this.btnUpdateDetail);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.comWarehouse);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.comItem);
		this.groupBox1.Controls.Add(this.label40);
		this.groupBox1.Controls.Add(this.label32);
		this.groupBox1.Controls.Add(this.label31);
		this.groupBox1.Controls.Add(this.label15);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(3, 183);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1148, 112);
		this.groupBox1.TabIndex = 1;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Detail";
		this.CmbPackUom.AllowNull = (DefaultableBoolean)1;
		this.CmbPackUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance169).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance169).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance169;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance170).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance170).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance170).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance170;
		((AppearanceBase)appearance171).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance171;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance172).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance172).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance172).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance172;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance173).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance173).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance173;
		((AppearanceBase)appearance174).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance174).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance174;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance175;
		((AppearanceBase)appearance176).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance176).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance176;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance177).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance177).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance177).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance177).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance177;
		((AppearanceBase)appearance195).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance195;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance196).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance196).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance196;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance197).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance197;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPackUom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Location = new System.Drawing.Point(444, 37);
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Name = "CmbPackUom";
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Size = new System.Drawing.Size(89, 26);
		((System.Windows.Forms.Control)(object)this.CmbPackUom).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).TextChanged += new System.EventHandler(CmbPackUom_TextChanged);
		this.label58.AutoSize = true;
		this.label58.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label58.Location = new System.Drawing.Point(444, 18);
		this.label58.Name = "label58";
		this.label58.Size = new System.Drawing.Size(61, 15);
		this.label58.TabIndex = 157;
		this.label58.Text = "Pack Uom";
		this.chkisFreeOfCost.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.chkisFreeOfCost.AutoSize = true;
		this.chkisFreeOfCost.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkisFreeOfCost.ForeColor = System.Drawing.Color.Black;
		this.chkisFreeOfCost.Location = new System.Drawing.Point(1011, 64);
		this.chkisFreeOfCost.Name = "chkisFreeOfCost";
		this.chkisFreeOfCost.Size = new System.Drawing.Size(102, 19);
		this.chkisFreeOfCost.TabIndex = 20;
		this.chkisFreeOfCost.Text = "Is Free Of Cost";
		this.chkisFreeOfCost.UseVisualStyleBackColor = true;
		this.btnAdd.BackColor = System.Drawing.Color.Teal;
		this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAdd.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAdd.ForeColor = System.Drawing.Color.White;
		this.btnAdd.Location = new System.Drawing.Point(1008, 82);
		this.btnAdd.Margin = new System.Windows.Forms.Padding(2);
		this.btnAdd.Name = "btnAdd";
		this.btnAdd.Size = new System.Drawing.Size(40, 26);
		this.btnAdd.TabIndex = 11;
		this.btnAdd.Text = "+";
		this.btnAdd.UseVisualStyleBackColor = false;
		this.btnAdd.Click += new System.EventHandler(btnAdd_Click);
		this.label43.AutoSize = true;
		this.label43.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label43.Location = new System.Drawing.Point(96, 65);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(68, 15);
		this.label43.TabIndex = 149;
		this.label43.Text = "Net Weight";
		this.txtNetWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNetWeight.Enabled = false;
		this.txtNetWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNetWeight.Location = new System.Drawing.Point(96, 84);
		this.txtNetWeight.Name = "txtNetWeight";
		this.txtNetWeight.Size = new System.Drawing.Size(107, 23);
		this.txtNetWeight.TabIndex = 8;
		this.txtNetWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.CmbAttributeVarient.AllowNull = (DefaultableBoolean)1;
		this.CmbAttributeVarient.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbAttributeVarient.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbAttributeVarient.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Appearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance36).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance36).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance37;
		((SpecialBoxBase)((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance38).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance38).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance38).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance46).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance54;
		((AppearanceBase)appearance64).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance64).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance67).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance67).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance67).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance67).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance67;
		((AppearanceBase)appearance65).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbAttributeVarient).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbAttributeVarient.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbAttributeVarient).Location = new System.Drawing.Point(536, 37);
		((System.Windows.Forms.Control)(object)this.CmbAttributeVarient).Name = "CmbAttributeVarient";
		((System.Windows.Forms.Control)(object)this.CmbAttributeVarient).Size = new System.Drawing.Size(101, 26);
		((System.Windows.Forms.Control)(object)this.CmbAttributeVarient).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.CmbAttributeVarient).Leave += new System.EventHandler(CmbAttributeVarient_Leave);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label1.Location = new System.Drawing.Point(3, 65);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(74, 15);
		this.label1.TabIndex = 131;
		this.label1.Text = "Item Weight";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label2.Location = new System.Drawing.Point(645, 18);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(69, 15);
		this.label2.TabIndex = 132;
		this.label2.Text = "Varient Unit";
		this.comjobLot.AllowNull = (DefaultableBoolean)1;
		this.comjobLot.AutoCompleteMode = (AutoCompleteMode)4;
		this.comjobLot.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comjobLot.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comjobLot).DisplayLayout.Appearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.comjobLot).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comjobLot).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance10).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance10).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comjobLot).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance10;
		((AppearanceBase)appearance11).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comjobLot).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance11;
		((SpecialBoxBase)((UltraGridBase)this.comjobLot).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance12).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comjobLot).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.comjobLot).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comjobLot).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance13;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance14).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance16;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance17).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance18).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance18).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance18).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance18;
		((AppearanceBase)appearance4).TextHAlignAsString = "Left";
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.comjobLot).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comjobLot).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comjobLot).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comjobLot).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.comjobLot.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comjobLot).Location = new System.Drawing.Point(762, 37);
		((System.Windows.Forms.Control)(object)this.comjobLot).Name = "comjobLot";
		((System.Windows.Forms.Control)(object)this.comjobLot).Size = new System.Drawing.Size(245, 26);
		((System.Windows.Forms.Control)(object)this.comjobLot).TabIndex = 5;
		((System.Windows.Forms.Control)(object)this.comjobLot).Leave += new System.EventHandler(comjobLot_Leave);
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label33.Location = new System.Drawing.Point(762, 18);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(46, 15);
		this.label33.TabIndex = 37;
		this.label33.Text = "Job Lot";
		this.txtRemarksDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarksDetail.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksDetail.Location = new System.Drawing.Point(444, 84);
		this.txtRemarksDetail.Name = "txtRemarksDetail";
		this.txtRemarksDetail.Size = new System.Drawing.Size(563, 23);
		this.txtRemarksDetail.TabIndex = 10;
		this.txtQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtQty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtQty.Location = new System.Drawing.Point(1010, 39);
		this.txtQty.Name = "txtQty";
		this.txtQty.Size = new System.Drawing.Size(101, 23);
		this.txtQty.TabIndex = 6;
		this.txtQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtQty.TextChanged += new System.EventHandler(txtQty_TextChanged);
		this.txtQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtQty_KeyPress);
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label9.Location = new System.Drawing.Point(536, 18);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(94, 15);
		this.label9.TabIndex = 133;
		this.label9.Text = "Attribute Varient";
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label38.Location = new System.Drawing.Point(1010, 18);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(26, 15);
		this.label38.TabIndex = 47;
		this.label38.Text = "Qty";
		this.txtItemWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemWeight.Enabled = false;
		this.txtItemWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemWeight.Location = new System.Drawing.Point(3, 84);
		this.txtItemWeight.Name = "txtItemWeight";
		this.txtItemWeight.Size = new System.Drawing.Size(92, 23);
		this.txtItemWeight.TabIndex = 7;
		this.txtVarientEquivalent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtVarientEquivalent.Enabled = false;
		this.txtVarientEquivalent.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtVarientEquivalent.Location = new System.Drawing.Point(639, 39);
		this.txtVarientEquivalent.Name = "txtVarientEquivalent";
		this.txtVarientEquivalent.Size = new System.Drawing.Size(121, 23);
		this.txtVarientEquivalent.TabIndex = 4;
		this.rdbtnItemCode.AutoSize = true;
		this.rdbtnItemCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdbtnItemCode.Location = new System.Drawing.Point(290, 16);
		this.rdbtnItemCode.Margin = new System.Windows.Forms.Padding(2);
		this.rdbtnItemCode.Name = "rdbtnItemCode";
		this.rdbtnItemCode.Size = new System.Drawing.Size(52, 19);
		this.rdbtnItemCode.TabIndex = 127;
		this.rdbtnItemCode.Text = "Code";
		this.rdbtnItemCode.UseVisualStyleBackColor = true;
		this.rdbtnItemName.AutoSize = true;
		this.rdbtnItemName.Checked = true;
		this.rdbtnItemName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdbtnItemName.Location = new System.Drawing.Point(237, 16);
		this.rdbtnItemName.Margin = new System.Windows.Forms.Padding(2);
		this.rdbtnItemName.Name = "rdbtnItemName";
		this.rdbtnItemName.Size = new System.Drawing.Size(57, 19);
		this.rdbtnItemName.TabIndex = 126;
		this.rdbtnItemName.TabStop = true;
		this.rdbtnItemName.Text = "Name";
		this.rdbtnItemName.UseVisualStyleBackColor = true;
		this.txtCity.AllowNull = (DefaultableBoolean)1;
		this.txtCity.AutoCompleteMode = (AutoCompleteMode)4;
		this.txtCity.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.txtCity.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance142).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.txtCity).DisplayLayout.Appearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.txtCity).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.txtCity).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance143).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance143).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.txtCity).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance143;
		((AppearanceBase)appearance144).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.txtCity).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance144;
		((SpecialBoxBase)((UltraGridBase)this.txtCity).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance146).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance146).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance146).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.txtCity).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.txtCity).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.txtCity).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance147).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance147;
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance148).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance148;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance149;
		((AppearanceBase)appearance150).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance150).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance150;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance151).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance151).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance151).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance151).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance151;
		((AppearanceBase)appearance83).TextHAlignAsString = "Left";
		((UltraGridBase)this.txtCity).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance84).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.txtCity).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.txtCity).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.txtCity).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.txtCity).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtCity.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.txtCity).Location = new System.Drawing.Point(205, 82);
		((System.Windows.Forms.Control)(object)this.txtCity).Name = "txtCity";
		((System.Windows.Forms.Control)(object)this.txtCity).Size = new System.Drawing.Size(236, 26);
		((System.Windows.Forms.Control)(object)this.txtCity).TabIndex = 9;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(205, 65);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(62, 15);
		this.label8.TabIndex = 84;
		this.label8.Text = "City Name";
		this.lblBalance.AutoSize = true;
		this.lblBalance.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblBalance.Location = new System.Drawing.Point(376, 18);
		this.lblBalance.Name = "lblBalance";
		this.lblBalance.Size = new System.Drawing.Size(14, 15);
		this.lblBalance.TabIndex = 80;
		this.lblBalance.Text = "0";
		this.btnCancelUpdateDetial.BackColor = System.Drawing.Color.Teal;
		this.btnCancelUpdateDetial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelUpdateDetial.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelUpdateDetial.ForeColor = System.Drawing.Color.White;
		this.btnCancelUpdateDetial.Location = new System.Drawing.Point(1076, 82);
		this.btnCancelUpdateDetial.Margin = new System.Windows.Forms.Padding(2);
		this.btnCancelUpdateDetial.Name = "btnCancelUpdateDetial";
		this.btnCancelUpdateDetial.Size = new System.Drawing.Size(68, 26);
		this.btnCancelUpdateDetial.TabIndex = 13;
		this.btnCancelUpdateDetial.Text = "Cancel";
		this.btnCancelUpdateDetial.UseVisualStyleBackColor = false;
		this.btnCancelUpdateDetial.Click += new System.EventHandler(btnCancelUpdateDetial_Click);
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(1008, 82);
		this.btnUpdateDetail.Margin = new System.Windows.Forms.Padding(2);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(70, 26);
		this.btnUpdateDetail.TabIndex = 12;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.comWarehouse.AllowNull = (DefaultableBoolean)1;
		this.comWarehouse.AutoCompleteMode = (AutoCompleteMode)4;
		this.comWarehouse.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comWarehouse.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Appearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.comWarehouse).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comWarehouse).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance71).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance71).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comWarehouse).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance71;
		((AppearanceBase)appearance72).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comWarehouse).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance72;
		((SpecialBoxBase)((UltraGridBase)this.comWarehouse).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comWarehouse).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.comWarehouse).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comWarehouse).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance58;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance59).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance60).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance60).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance60).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance60;
		((AppearanceBase)appearance61).TextHAlignAsString = "Left";
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance62).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.comWarehouse).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comWarehouse).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comWarehouse).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comWarehouse).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.comWarehouse.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comWarehouse).Location = new System.Drawing.Point(3, 37);
		((System.Windows.Forms.Control)(object)this.comWarehouse).Name = "comWarehouse";
		((System.Windows.Forms.Control)(object)this.comWarehouse).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.comWarehouse).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.comWarehouse).Leave += new System.EventHandler(comWarehouse_TextChanged);
		this.comItem.AllowNull = (DefaultableBoolean)1;
		this.comItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.comItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.comItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance23).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance23).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance24;
		((SpecialBoxBase)((UltraGridBase)this.comItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance25).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance25).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance25).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.comItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.comItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance29;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance30).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.comItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance31).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance31).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance31).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance32).TextHAlignAsString = "Left";
		((UltraGridBase)this.comItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.comItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.comItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.comItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comItem).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.comItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comItem).Location = new System.Drawing.Point(205, 37);
		((System.Windows.Forms.Control)(object)this.comItem).Name = "comItem";
		((System.Windows.Forms.Control)(object)this.comItem).Size = new System.Drawing.Size(236, 26);
		((System.Windows.Forms.Control)(object)this.comItem).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.comItem).Leave += new System.EventHandler(comItem_Leave);
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.Location = new System.Drawing.Point(682, 359);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(39, 18);
		this.label40.TabIndex = 15;
		this.label40.Text = "Rate";
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label32.Location = new System.Drawing.Point(202, 18);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(32, 15);
		this.label32.TabIndex = 39;
		this.label32.Text = "Item";
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label31.Location = new System.Drawing.Point(3, 18);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(69, 15);
		this.label31.TabIndex = 33;
		this.label31.Text = "WareHouse";
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(444, 65);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(52, 15);
		this.label15.TabIndex = 102;
		this.label15.Text = "Remarks";
		this.groupBox2.Controls.Add(this.RdPartyByCode);
		this.groupBox2.Controls.Add(this.RdPartyByName);
		this.groupBox2.Controls.Add(this.txtCNICNO);
		this.groupBox2.Controls.Add(this.txtFreightAmountHeader);
		this.groupBox2.Controls.Add(this.label6);
		this.groupBox2.Controls.Add(this.label89);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbVehicleType);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbVehicleNo);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbTransporterAc);
		this.groupBox2.Controls.Add(this.label4);
		this.groupBox2.Controls.Add(this.label14);
		this.groupBox2.Controls.Add(this.txtSupplierGLId);
		this.groupBox2.Controls.Add(this.label11);
		this.groupBox2.Controls.Add(this.label22);
		this.groupBox2.Controls.Add(this.label30);
		this.groupBox2.Controls.Add(this.label10);
		this.groupBox2.Controls.Add(this.label34);
		this.groupBox2.Controls.Add(this.txtDriverCellNo);
		this.groupBox2.Controls.Add(this.txtDriverName);
		this.groupBox2.Controls.Add(this.label36);
		this.groupBox2.Controls.Add(this.TxtDistance);
		this.groupBox2.Controls.Add(this.txtRefPartyCellNo);
		this.groupBox2.Controls.Add(this.txtRefPartyAddress);
		this.groupBox2.Controls.Add(this.txtRefPartyName);
		this.groupBox2.Controls.Add(this.txtgatepassno);
		this.groupBox2.Controls.Add(this.txtgpdate);
		this.groupBox2.Controls.Add(this.txtdocno);
		this.groupBox2.Controls.Add(this.label47);
		this.groupBox2.Controls.Add(this.DocDate);
		this.groupBox2.Controls.Add(this.label5);
		this.groupBox2.Controls.Add(this.label48);
		this.groupBox2.Controls.Add(this.label12);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.combdeliverytrm);
		this.groupBox2.Controls.Add(this.txtManualNo);
		this.groupBox2.Controls.Add(this.txtSupplierReference);
		this.groupBox2.Controls.Add(this.txtremarks);
		this.groupBox2.Controls.Add(this.label24);
		this.groupBox2.Controls.Add(this.label19);
		this.groupBox2.Controls.Add(this.label7);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.comsupplier);
		this.groupBox2.Controls.Add(this.label29);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(2, 27);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(1148, 156);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Main";
		this.RdPartyByCode.AutoSize = true;
		this.RdPartyByCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RdPartyByCode.Location = new System.Drawing.Point(60, 55);
		this.RdPartyByCode.Margin = new System.Windows.Forms.Padding(2);
		this.RdPartyByCode.Name = "RdPartyByCode";
		this.RdPartyByCode.Size = new System.Drawing.Size(52, 19);
		this.RdPartyByCode.TabIndex = 143;
		this.RdPartyByCode.Text = "Code";
		this.RdPartyByCode.UseVisualStyleBackColor = true;
		this.RdPartyByName.AutoSize = true;
		this.RdPartyByName.Checked = true;
		this.RdPartyByName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RdPartyByName.Location = new System.Drawing.Point(60, 39);
		this.RdPartyByName.Margin = new System.Windows.Forms.Padding(2);
		this.RdPartyByName.Name = "RdPartyByName";
		this.RdPartyByName.Size = new System.Drawing.Size(57, 19);
		this.RdPartyByName.TabIndex = 142;
		this.RdPartyByName.Text = "Name";
		this.RdPartyByName.UseVisualStyleBackColor = true;
		this.RdPartyByName.CheckedChanged += new System.EventHandler(RdPartyByName_CheckedChanged);
		this.txtCNICNO.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCNICNO.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCNICNO.Location = new System.Drawing.Point(930, 98);
		this.txtCNICNO.Name = "txtCNICNO";
		this.txtCNICNO.Size = new System.Drawing.Size(212, 23);
		this.txtCNICNO.TabIndex = 19;
		this.txtFreightAmountHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFreightAmountHeader.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFreightAmountHeader.Location = new System.Drawing.Point(670, 16);
		this.txtFreightAmountHeader.Name = "txtFreightAmountHeader";
		this.txtFreightAmountHeader.Size = new System.Drawing.Size(173, 23);
		this.txtFreightAmountHeader.TabIndex = 10;
		this.txtFreightAmountHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFreightAmountHeader.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFreightAmount_KeyPress);
		this.label6.AutoSize = true;
		this.label6.BackColor = System.Drawing.Color.White;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label6.Location = new System.Drawing.Point(592, 20);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(76, 15);
		this.label6.TabIndex = 128;
		this.label6.Text = "Carriage Amt";
		this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label89.AutoSize = true;
		this.label89.BackColor = System.Drawing.Color.White;
		this.label89.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label89.Location = new System.Drawing.Point(326, 129);
		this.label89.Name = "label89";
		this.label89.Size = new System.Drawing.Size(72, 15);
		this.label89.TabIndex = 128;
		this.label89.Text = "Carriage A/c";
		this.label89.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.CmbVehicleType.AllowNull = (DefaultableBoolean)1;
		this.CmbVehicleType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbVehicleType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbVehicleType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbVehicleType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance42;
		((AppearanceBase)appearance109).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance109;
		((SpecialBoxBase)((UltraGridBase)this.CmbVehicleType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance145).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance145).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance145).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance156).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance156;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance167).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance167;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance178;
		((AppearanceBase)appearance189).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance189).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance189;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance198).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance198).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance198).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance198).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance198).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance198;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbVehicleType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbVehicleType).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbVehicleType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbVehicleType).Location = new System.Drawing.Point(670, 96);
		((System.Windows.Forms.Control)(object)this.CmbVehicleType).Name = "CmbVehicleType";
		((System.Windows.Forms.Control)(object)this.CmbVehicleType).Size = new System.Drawing.Size(173, 26);
		((System.Windows.Forms.Control)(object)this.CmbVehicleType).TabIndex = 14;
		this.CmbVehicleNo.AllowNull = (DefaultableBoolean)1;
		this.CmbVehicleNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbVehicleNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbVehicleNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance45).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance45).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbVehicleNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance47;
		((SpecialBoxBase)((UltraGridBase)this.CmbVehicleNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance49).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance50).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance51;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance52).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance97).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance100;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbVehicleNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbVehicleNo).Font = new System.Drawing.Font("Verdana", 9.75f);
		((System.Windows.Forms.Control)(object)this.CmbVehicleNo).Location = new System.Drawing.Point(930, 14);
		((System.Windows.Forms.Control)(object)this.CmbVehicleNo).Name = "CmbVehicleNo";
		((System.Windows.Forms.Control)(object)this.CmbVehicleNo).Size = new System.Drawing.Size(212, 26);
		((System.Windows.Forms.Control)(object)this.CmbVehicleNo).TabIndex = 16;
		this.CmbTransporterAc.AllowNull = (DefaultableBoolean)1;
		this.CmbTransporterAc.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbTransporterAc.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbTransporterAc.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance139).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.Appearance = (AppearanceBase)(object)appearance139;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance140).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance140).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbTransporterAc).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance140;
		((AppearanceBase)appearance141).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance141;
		((SpecialBoxBase)((UltraGridBase)this.CmbTransporterAc).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance162).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance162).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance162).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance162;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance163).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance163;
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance164).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance166).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance166;
		((AppearanceBase)appearance168).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance168).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance179).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance179).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance179).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance179).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance179;
		((AppearanceBase)appearance180).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance180;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance181).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance181).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance181;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance192).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance192;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbTransporterAc).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbTransporterAc).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbTransporterAc.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbTransporterAc).Location = new System.Drawing.Point(407, 123);
		((System.Windows.Forms.Control)(object)this.CmbTransporterAc).Name = "CmbTransporterAc";
		((System.Windows.Forms.Control)(object)this.CmbTransporterAc).Size = new System.Drawing.Size(183, 26);
		((System.Windows.Forms.Control)(object)this.CmbTransporterAc).TabIndex = 9;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(848, 102);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(53, 15);
		this.label4.TabIndex = 116;
		this.label4.Text = "CNIC No";
		this.label14.AutoSize = true;
		this.label14.BackColor = System.Drawing.Color.White;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(592, 102);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(73, 15);
		this.label14.TabIndex = 141;
		this.label14.Text = "Vehicle Type";
		this.txtSupplierGLId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSupplierGLId.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSupplierGLId.Location = new System.Drawing.Point(912, 99);
		this.txtSupplierGLId.Name = "txtSupplierGLId";
		this.txtSupplierGLId.Size = new System.Drawing.Size(16, 22);
		this.txtSupplierGLId.TabIndex = 61;
		this.txtSupplierGLId.Visible = false;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(848, 48);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(80, 15);
		this.label11.TabIndex = 115;
		this.label11.Text = "Driver Cell No";
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(326, 20);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(53, 15);
		this.label22.TabIndex = 137;
		this.label22.Text = "Distance";
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(326, 48);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(76, 15);
		this.label30.TabIndex = 137;
		this.label30.Text = "Ref P. Cell No";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(848, 74);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(74, 15);
		this.label10.TabIndex = 114;
		this.label10.Text = "Driver Name";
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.Location = new System.Drawing.Point(326, 76);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(80, 15);
		this.label34.TabIndex = 138;
		this.label34.Text = "Ref P. Address";
		this.txtDriverCellNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDriverCellNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDriverCellNo.Location = new System.Drawing.Point(930, 44);
		this.txtDriverCellNo.Name = "txtDriverCellNo";
		this.txtDriverCellNo.Size = new System.Drawing.Size(212, 23);
		this.txtDriverCellNo.TabIndex = 17;
		this.txtDriverName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDriverName.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDriverName.Location = new System.Drawing.Point(930, 70);
		this.txtDriverName.Name = "txtDriverName";
		this.txtDriverName.Size = new System.Drawing.Size(212, 23);
		this.txtDriverName.TabIndex = 18;
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(3, 129);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(89, 15);
		this.label36.TabIndex = 139;
		this.label36.Text = "Reference Party";
		this.TxtDistance.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.TxtDistance.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.TxtDistance.Location = new System.Drawing.Point(407, 16);
		this.TxtDistance.Name = "TxtDistance";
		this.TxtDistance.Size = new System.Drawing.Size(183, 23);
		this.TxtDistance.TabIndex = 6;
		this.txtRefPartyCellNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRefPartyCellNo.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtRefPartyCellNo.Location = new System.Drawing.Point(407, 44);
		this.txtRefPartyCellNo.Name = "txtRefPartyCellNo";
		this.txtRefPartyCellNo.Size = new System.Drawing.Size(183, 23);
		this.txtRefPartyCellNo.TabIndex = 7;
		this.txtRefPartyAddress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRefPartyAddress.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtRefPartyAddress.Location = new System.Drawing.Point(407, 70);
		this.txtRefPartyAddress.Multiline = true;
		this.txtRefPartyAddress.Name = "txtRefPartyAddress";
		this.txtRefPartyAddress.Size = new System.Drawing.Size(183, 51);
		this.txtRefPartyAddress.TabIndex = 8;
		this.txtRefPartyName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRefPartyName.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtRefPartyName.Location = new System.Drawing.Point(118, 125);
		this.txtRefPartyName.Name = "txtRefPartyName";
		this.txtRefPartyName.Size = new System.Drawing.Size(207, 23);
		this.txtRefPartyName.TabIndex = 5;
		this.txtgatepassno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtgatepassno.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtgatepassno.Location = new System.Drawing.Point(670, 70);
		this.txtgatepassno.Name = "txtgatepassno";
		this.txtgatepassno.Size = new System.Drawing.Size(62, 23);
		this.txtgatepassno.TabIndex = 12;
		this.txtgatepassno.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtgatepassno_KeyPress);
		this.txtgpdate.CustomFormat = "dd-MM-yy";
		this.txtgpdate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtgpdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtgpdate.Location = new System.Drawing.Point(734, 70);
		this.txtgpdate.Name = "txtgpdate";
		this.txtgpdate.Size = new System.Drawing.Size(109, 23);
		this.txtgpdate.TabIndex = 13;
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocno.Location = new System.Drawing.Point(118, 16);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.Size = new System.Drawing.Size(90, 23);
		this.txtdocno.TabIndex = 0;
		this.txtdocno.Leave += new System.EventHandler(txtdocno_TextChanged);
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label47.Location = new System.Drawing.Point(592, 74);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(72, 15);
		this.label47.TabIndex = 60;
		this.label47.Text = "GP # && Date";
		this.DocDate.CustomFormat = "dd-MMM-yy";
		this.DocDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DocDate.Location = new System.Drawing.Point(210, 16);
		this.DocDate.Name = "DocDate";
		this.DocDate.Size = new System.Drawing.Size(115, 23);
		this.DocDate.TabIndex = 1;
		this.DocDate.ValueChanged += new System.EventHandler(DocDate_ValueChanged);
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label5.Location = new System.Drawing.Point(3, 20);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(88, 15);
		this.label5.TabIndex = 88;
		this.label5.Text = "Doc No && Date";
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.Location = new System.Drawing.Point(848, 20);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(64, 15);
		this.label48.TabIndex = 70;
		this.label48.Text = "Vehicle No";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label12.Location = new System.Drawing.Point(3, 76);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(78, 15);
		this.label12.TabIndex = 63;
		this.label12.Text = "Reference No";
		this.combdeliverytrm.AllowNull = (DefaultableBoolean)1;
		this.combdeliverytrm.AutoCompleteMode = (AutoCompleteMode)4;
		this.combdeliverytrm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combdeliverytrm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance87).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance87).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combdeliverytrm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance87;
		((AppearanceBase)appearance89).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance89;
		((SpecialBoxBase)((UltraGridBase)this.combdeliverytrm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance73).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance73).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance73).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance74).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance75).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance76;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance78).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance79).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance79).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance79).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance79).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance80).TextHAlignAsString = "Left";
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.combdeliverytrm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).Location = new System.Drawing.Point(670, 42);
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).Name = "combdeliverytrm";
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).Size = new System.Drawing.Size(173, 26);
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).TabIndex = 11;
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).TextChanged += new System.EventHandler(combdeliverytrm_TextChanged);
		this.txtManualNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtManualNo.Location = new System.Drawing.Point(118, 98);
		this.txtManualNo.Name = "txtManualNo";
		this.txtManualNo.Size = new System.Drawing.Size(207, 23);
		this.txtManualNo.TabIndex = 4;
		this.txtSupplierReference.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSupplierReference.Location = new System.Drawing.Point(118, 72);
		this.txtSupplierReference.Name = "txtSupplierReference";
		this.txtSupplierReference.Size = new System.Drawing.Size(207, 23);
		this.txtSupplierReference.TabIndex = 3;
		this.txtremarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtremarks.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtremarks.Location = new System.Drawing.Point(670, 125);
		this.txtremarks.Name = "txtremarks";
		this.txtremarks.Size = new System.Drawing.Size(472, 23);
		this.txtremarks.TabIndex = 15;
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label24.Location = new System.Drawing.Point(592, 129);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(52, 15);
		this.label24.TabIndex = 102;
		this.label24.Text = "Remarks";
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label19.Location = new System.Drawing.Point(3, 102);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(66, 15);
		this.label19.TabIndex = 65;
		this.label19.Text = "Manual No";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label7.Location = new System.Drawing.Point(3, 48);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(58, 15);
		this.label7.TabIndex = 65;
		this.label7.Text = "Party (Dr)";
		this.comsupplier.AllowNull = (DefaultableBoolean)1;
		this.comsupplier.AutoCompleteMode = (AutoCompleteMode)4;
		this.comsupplier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comsupplier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comsupplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.comsupplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comsupplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comsupplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comsupplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance35;
		((SpecialBoxBase)((UltraGridBase)this.comsupplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comsupplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.comsupplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comsupplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance77).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance77;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance88).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance94;
		((AppearanceBase)appearance98).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance98).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance98;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance110).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance110).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance110).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance110).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance110;
		((AppearanceBase)appearance121).TextHAlignAsString = "Left";
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance132).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.comsupplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comsupplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comsupplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comsupplier).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.comsupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comsupplier).Location = new System.Drawing.Point(118, 42);
		((System.Windows.Forms.Control)(object)this.comsupplier).Name = "comsupplier";
		((System.Windows.Forms.Control)(object)this.comsupplier).Size = new System.Drawing.Size(207, 26);
		((System.Windows.Forms.Control)(object)this.comsupplier).TabIndex = 2;
		this.comsupplier.ValueChanged += new System.EventHandler(comsupplier_ValueChanged);
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label29.Location = new System.Drawing.Point(592, 48);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(80, 15);
		this.label29.TabIndex = 101;
		this.label29.Text = "Delivery Term";
		this.panel11.BackColor = System.Drawing.Color.Teal;
		this.panel11.Controls.Add(this.label17);
		this.panel11.Controls.Add(this.ChkPrintslip);
		this.panel11.Controls.Add(this.ChkResetOnSave);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(0, 0);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1160, 26);
		this.panel11.TabIndex = 110;
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.ForeColor = System.Drawing.Color.White;
		this.label17.Location = new System.Drawing.Point(4, 4);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(186, 19);
		this.label17.TabIndex = 34;
		this.label17.Text = "Goods Dispatch Notes";
		this.ChkPrintslip.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkPrintslip.AutoSize = true;
		this.ChkPrintslip.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkPrintslip.ForeColor = System.Drawing.Color.White;
		this.ChkPrintslip.Location = new System.Drawing.Point(1070, 4);
		this.ChkPrintslip.Margin = new System.Windows.Forms.Padding(2);
		this.ChkPrintslip.Name = "ChkPrintslip";
		this.ChkPrintslip.Size = new System.Drawing.Size(88, 20);
		this.ChkPrintslip.TabIndex = 1;
		this.ChkPrintslip.Text = "Print Slip";
		this.ChkPrintslip.UseVisualStyleBackColor = true;
		this.ChkResetOnSave.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkResetOnSave.AutoSize = true;
		this.ChkResetOnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkResetOnSave.ForeColor = System.Drawing.Color.White;
		this.ChkResetOnSave.Location = new System.Drawing.Point(870, 3);
		this.ChkResetOnSave.Margin = new System.Windows.Forms.Padding(2);
		this.ChkResetOnSave.Name = "ChkResetOnSave";
		this.ChkResetOnSave.Size = new System.Drawing.Size(196, 20);
		this.ChkResetOnSave.TabIndex = 0;
		this.ChkResetOnSave.Text = "Reset On Add and  Save";
		this.ChkResetOnSave.UseVisualStyleBackColor = true;
		this.comproject.AllowNull = (DefaultableBoolean)1;
		this.comproject.AutoCompleteMode = (AutoCompleteMode)4;
		this.comproject.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comproject.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance182).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance182).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comproject).DisplayLayout.Appearance = (AppearanceBase)(object)appearance182;
		((UltraGridBase)this.comproject).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comproject).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance183).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance183).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance183).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance183).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comproject).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance183;
		((AppearanceBase)appearance184).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comproject).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance184;
		((SpecialBoxBase)((UltraGridBase)this.comproject).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance185).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance185).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance185).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance185).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comproject).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance185;
		((UltraGridBase)this.comproject).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comproject).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance186).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance186).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comproject).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance186;
		((AppearanceBase)appearance187).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance187).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comproject).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance187;
		((UltraGridBase)this.comproject).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comproject).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance188).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance188;
		((AppearanceBase)appearance190).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance190).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance190;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance191).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance191).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance191).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance191).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance191).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comproject).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance191;
		((AppearanceBase)appearance39).TextHAlignAsString = "Left";
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comproject).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.comproject).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comproject).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.comproject).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comproject).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comproject).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comproject).Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.comproject.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comproject).Location = new System.Drawing.Point(1392, 41);
		((System.Windows.Forms.Control)(object)this.comproject).Name = "comproject";
		((System.Windows.Forms.Control)(object)this.comproject).Size = new System.Drawing.Size(78, 26);
		((System.Windows.Forms.Control)(object)this.comproject).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.comproject).TabStop = false;
		((System.Windows.Forms.Control)(object)this.comproject).Visible = false;
		this.combranches.AllowNull = (DefaultableBoolean)1;
		this.combranches.AutoCompleteMode = (AutoCompleteMode)4;
		this.combranches.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combranches.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combranches).DisplayLayout.Appearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.combranches).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combranches).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance7).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance7).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combranches).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance7;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combranches).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance8;
		((SpecialBoxBase)((UltraGridBase)this.combranches).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance90).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance90).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combranches).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.combranches).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combranches).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance91).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combranches).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance91;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance92).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combranches).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.combranches).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combranches).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance95).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance95).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance96).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance96).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance96).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance96).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combranches).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance199).TextHAlignAsString = "Left";
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance199;
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance200).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance200).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combranches).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance200;
		((UltraGridBase)this.combranches).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance201).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combranches).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance201;
		((UltraGridBase)this.combranches).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combranches).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combranches).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combranches).Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combranches.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combranches).Location = new System.Drawing.Point(1392, 73);
		((System.Windows.Forms.Control)(object)this.combranches).Name = "combranches";
		((System.Windows.Forms.Control)(object)this.combranches).Size = new System.Drawing.Size(78, 26);
		((System.Windows.Forms.Control)(object)this.combranches).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.combranches).TabStop = false;
		((System.Windows.Forms.Control)(object)this.combranches).Visible = false;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(1302, 79);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(79, 15);
		this.label13.TabIndex = 86;
		this.label13.Text = "Branch Name";
		this.label13.Visible = false;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(1302, 47);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(79, 15);
		this.label18.TabIndex = 87;
		this.label18.Text = "Project Name";
		this.label18.Visible = false;
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[15]
		{
			this.btnNew, this.btnRefresh, this.toolStripSeparator2, this.btnUpdate, this.btnSave, this.btnDelete, this.toolStripSeparator3, this.btnAttachment, this.toolStripSeparator4, this.btnSlip,
			this.toolStripSeparator5, this.btnLoadGdn, this.toolStripSeparator6, this.btnshortcutkeys, this.toolStripSeparator1
		});
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1160, 25);
		this.toolStrip1.TabIndex = 26;
		this.toolStrip1.Text = "toolStrip1";
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(55, 22);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(76, 22);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
		this.btnUpdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(74, 22);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnSave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(60, 22);
		this.btnSave.Text = "&Save";
		this.btnSave.Click += new System.EventHandler(BtnSave_Click);
		this.btnDelete.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(69, 22);
		this.btnDelete.Text = "Delete";
		this.btnDelete.Visible = false;
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.toolStripSeparator3.Name = "toolStripSeparator3";
		this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
		this.btnAttachment.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(105, 22);
		this.btnAttachment.Text = "&Attachment";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.toolStripSeparator4.Name = "toolStripSeparator4";
		this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
		this.btnSlip.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSlip.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnSlip.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSlip.Name = "btnSlip";
		this.btnSlip.Size = new System.Drawing.Size(96, 22);
		this.btnSlip.Text = "1866-Print";
		this.btnSlip.Click += new System.EventHandler(btnSlip_Click);
		this.toolStripSeparator5.Name = "toolStripSeparator5";
		this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
		this.btnLoadGdn.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLoadGdn.Image = Architecture.WinApp.Properties.Resources.download_line;
		this.btnLoadGdn.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadGdn.Name = "btnLoadGdn";
		this.btnLoadGdn.Size = new System.Drawing.Size(99, 22);
		this.btnLoadGdn.Text = "Load Order";
		this.btnLoadGdn.Visible = false;
		this.toolStripSeparator6.Name = "toolStripSeparator6";
		this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
		this.btnshortcutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshortcutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnshortcutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnshortcutkeys.Name = "btnshortcutkeys";
		this.btnshortcutkeys.Size = new System.Drawing.Size(122, 22);
		this.btnshortcutkeys.Text = "ShortCut Keys";
		this.btnshortcutkeys.Click += new System.EventHandler(btnShortcutKeys_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
		this.tabPage2.Controls.Add(this.panel17);
		this.tabPage2.Controls.Add(this.panel10);
		this.tabPage2.Controls.Add(this.panel9);
		this.tabPage2.Controls.Add(this.panel2);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1166, 641);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel17.Controls.Add(this.panel19);
		this.panel17.Controls.Add(this.panel18);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel17.Location = new System.Drawing.Point(3, 443);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(1160, 195);
		this.panel17.TabIndex = 49;
		this.panel19.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetailHistory);
		this.panel19.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel19.Location = new System.Drawing.Point(0, 27);
		this.panel19.Name = "panel19";
		this.panel19.Size = new System.Drawing.Size(1160, 168);
		this.panel19.TabIndex = 49;
		this.grdDetailHistory.AllowEdit = (InheritableBoolean)2;
		grdDetailHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetailHistory.DesignTimeLayout = grdDetailHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetailHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdDetailHistory.DynamicFiltering = true;
		this.grdDetailHistory.FilterMode = (FilterMode)2;
		this.grdDetailHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdDetailHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetailHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdDetailHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdDetailHistory).Name = "grdDetailHistory";
		((ScrollableBase)this.grdDetailHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetailHistory).Size = new System.Drawing.Size(1160, 168);
		((System.Windows.Forms.Control)(object)this.grdDetailHistory).TabIndex = 0;
		this.grdDetailHistory.TotalRow = (InheritableBoolean)1;
		this.grdDetailHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetailHistory).VisualStyle = (VisualStyle)4;
		this.panel18.BackColor = System.Drawing.Color.Teal;
		this.panel18.Controls.Add(this.ctrlGrdBar3);
		this.panel18.Controls.Add(this.label55);
		this.panel18.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel18.Location = new System.Drawing.Point(0, 0);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(1160, 27);
		this.panel18.TabIndex = 48;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(1119, 3);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.grdDetailHistory;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar3.TabIndex = 34;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.label55.AutoSize = true;
		this.label55.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label55.ForeColor = System.Drawing.Color.White;
		this.label55.Location = new System.Drawing.Point(3, 4);
		this.label55.Name = "label55";
		this.label55.Size = new System.Drawing.Size(297, 17);
		this.label55.TabIndex = 35;
		this.label55.Text = "Detail Information of Above Selected Row";
		this.panel10.Controls.Add(this.panel8);
		this.panel10.Controls.Add(this.panel7);
		this.panel10.Controls.Add(this.panel5);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(3, 57);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1160, 386);
		this.panel10.TabIndex = 48;
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistory);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel8.Location = new System.Drawing.Point(0, 96);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1160, 290);
		this.panel8.TabIndex = 49;
		this.grdHistory.AllowEdit = (InheritableBoolean)2;
		grdHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistory.DesignTimeLayout = grdHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistory.DynamicFiltering = true;
		this.grdHistory.FilterMode = (FilterMode)2;
		this.grdHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdHistory).Name = "grdHistory";
		((ScrollableBase)this.grdHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistory).Size = new System.Drawing.Size(1160, 290);
		((System.Windows.Forms.Control)(object)this.grdHistory).TabIndex = 0;
		this.grdHistory.TotalRow = (InheritableBoolean)1;
		this.grdHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistory).VisualStyle = (VisualStyle)4;
		this.grdHistory.ColumnButtonClick += new ColumnActionEventHandler(grdHistory_ColumnButtonClick);
		this.grdHistory.SelectionChanged += new System.EventHandler(grdHistory_SelectionChanged);
		this.grdHistory.LinkClicked += new ColumnActionEventHandler(grdHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdHistory).DoubleClick += new System.EventHandler(grdHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(grdHistory_KeyDown);
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.ctrlGrdBar2);
		this.panel7.Controls.Add(this.label78);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 69);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1160, 27);
		this.panel7.TabIndex = 48;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1525, 0);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar2.MyGrid = this.grdHistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(51, 30);
		this.ctrlGrdBar2.TabIndex = 34;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.label78.AutoSize = true;
		this.label78.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label78.ForeColor = System.Drawing.Color.White;
		this.label78.Location = new System.Drawing.Point(5, 3);
		this.label78.Name = "label78";
		this.label78.Size = new System.Drawing.Size(72, 19);
		this.label78.TabIndex = 35;
		this.label78.Text = "History ";
		this.panel5.Controls.Add(this.groupBox4);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1160, 69);
		this.panel5.TabIndex = 0;
		this.groupBox4.Controls.Add(this.RdCustomerHistorySearchByCode);
		this.groupBox4.Controls.Add(this.RdCustomerHistorySearchByName);
		this.groupBox4.Controls.Add(this.label20);
		this.groupBox4.Controls.Add(this.btnshow);
		this.groupBox4.Controls.Add(this.label73);
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomerHistory);
		this.groupBox4.Controls.Add(this.label75);
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory);
		this.groupBox4.Controls.Add(this.txtToDateHistory);
		this.groupBox4.Controls.Add(this.label76);
		this.groupBox4.Controls.Add(this.txtToDocNoHistory);
		this.groupBox4.Controls.Add(this.txtFromdateHistory);
		this.groupBox4.Controls.Add(this.label3);
		this.groupBox4.Controls.Add(this.label77);
		this.groupBox4.Controls.Add(this.txtFromNoHistory);
		this.groupBox4.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox4.Location = new System.Drawing.Point(5, 2);
		this.groupBox4.Name = "groupBox4";
		this.groupBox4.Size = new System.Drawing.Size(892, 65);
		this.groupBox4.TabIndex = 0;
		this.groupBox4.TabStop = false;
		this.groupBox4.Text = "Filters";
		this.RdCustomerHistorySearchByCode.AutoSize = true;
		this.RdCustomerHistorySearchByCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RdCustomerHistorySearchByCode.Location = new System.Drawing.Point(654, 14);
		this.RdCustomerHistorySearchByCode.Margin = new System.Windows.Forms.Padding(2);
		this.RdCustomerHistorySearchByCode.Name = "RdCustomerHistorySearchByCode";
		this.RdCustomerHistorySearchByCode.Size = new System.Drawing.Size(52, 19);
		this.RdCustomerHistorySearchByCode.TabIndex = 181;
		this.RdCustomerHistorySearchByCode.Text = "Code";
		this.RdCustomerHistorySearchByCode.UseVisualStyleBackColor = true;
		this.RdCustomerHistorySearchByName.AutoSize = true;
		this.RdCustomerHistorySearchByName.Checked = true;
		this.RdCustomerHistorySearchByName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RdCustomerHistorySearchByName.Location = new System.Drawing.Point(596, 14);
		this.RdCustomerHistorySearchByName.Margin = new System.Windows.Forms.Padding(2);
		this.RdCustomerHistorySearchByName.Name = "RdCustomerHistorySearchByName";
		this.RdCustomerHistorySearchByName.Size = new System.Drawing.Size(57, 19);
		this.RdCustomerHistorySearchByName.TabIndex = 180;
		this.RdCustomerHistorySearchByName.Text = "Name";
		this.RdCustomerHistorySearchByName.UseVisualStyleBackColor = true;
		this.RdCustomerHistorySearchByName.CheckedChanged += new System.EventHandler(RdCustomerHistorySearchByName_CheckedChanged);
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(248, 17);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(48, 15);
		this.label20.TabIndex = 179;
		this.label20.Text = "Date To";
		this.btnshow.BackColor = System.Drawing.Color.Teal;
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(830, 36);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(54, 25);
		this.btnshow.TabIndex = 7;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.label73.AutoSize = true;
		this.label73.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label73.Location = new System.Drawing.Point(557, 16);
		this.label73.Name = "label73";
		this.label73.Size = new System.Drawing.Size(34, 15);
		this.label73.TabIndex = 178;
		this.label73.Text = "Party";
		this.CmbCustomerHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomerHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomerHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomerHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance152).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance152;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance153).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance153).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance153).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance153;
		((AppearanceBase)appearance154).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance154;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance155).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance155).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance155).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance157).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance157;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance158).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance158;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance159;
		((AppearanceBase)appearance160).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance160).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance160;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance161).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance161).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance161).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance161).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance161).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance161;
		((AppearanceBase)appearance165).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance165;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance193).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance193).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance193;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance194).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance194;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomerHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Location = new System.Drawing.Point(559, 35);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Name = "CmbCustomerHistory";
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Size = new System.Drawing.Size(268, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).TabIndex = 5;
		this.label75.AutoSize = true;
		this.label75.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label75.Location = new System.Drawing.Point(5, 17);
		this.label75.Name = "label75";
		this.label75.Size = new System.Drawing.Size(60, 15);
		this.label75.TabIndex = 174;
		this.label75.Text = "Date Type";
		this.cmbDateTypeHistory.AllowNull = (DefaultableBoolean)1;
		this.cmbDateTypeHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDateTypeHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDateTypeHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance202).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance202).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance202;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance203).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance203).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance203).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance203).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance203;
		((AppearanceBase)appearance204).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance204;
		((SpecialBoxBase)((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance104).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance104).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance104).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance105).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance105;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance106).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance107;
		((AppearanceBase)appearance108).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance108).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance111).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance111).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance111).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance111).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance111;
		((AppearanceBase)appearance112).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDateTypeHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Location = new System.Drawing.Point(5, 35);
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Name = "cmbDateTypeHistory";
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Size = new System.Drawing.Size(127, 26);
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).TabIndex = 0;
		this.cmbDateTypeHistory.ValueChanged += new System.EventHandler(cmbDateTypeHistory_ValueChanged);
		this.txtToDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDateHistory.CustomFormat = "dd-MMM-yy";
		this.txtToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtToDateHistory.Location = new System.Drawing.Point(248, 37);
		this.txtToDateHistory.Name = "txtToDateHistory";
		this.txtToDateHistory.Size = new System.Drawing.Size(111, 23);
		this.txtToDateHistory.TabIndex = 2;
		this.label76.AutoSize = true;
		this.label76.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label76.Location = new System.Drawing.Point(134, 17);
		this.label76.Name = "label76";
		this.label76.Size = new System.Drawing.Size(63, 15);
		this.label76.TabIndex = 171;
		this.label76.Text = "Date From";
		this.txtToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDocNoHistory.Location = new System.Drawing.Point(460, 37);
		this.txtToDocNoHistory.Name = "txtToDocNoHistory";
		this.txtToDocNoHistory.Size = new System.Drawing.Size(97, 23);
		this.txtToDocNoHistory.TabIndex = 4;
		this.txtFromdateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromdateHistory.CustomFormat = "dd-MMM-yy";
		this.txtFromdateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromdateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtFromdateHistory.Location = new System.Drawing.Point(134, 37);
		this.txtFromdateHistory.Name = "txtFromdateHistory";
		this.txtFromdateHistory.Size = new System.Drawing.Size(111, 23);
		this.txtFromdateHistory.TabIndex = 1;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(460, 17);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(45, 15);
		this.label3.TabIndex = 173;
		this.label3.Text = "Doc To";
		this.label77.AutoSize = true;
		this.label77.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label77.Location = new System.Drawing.Point(361, 17);
		this.label77.Name = "label77";
		this.label77.Size = new System.Drawing.Size(60, 15);
		this.label77.TabIndex = 173;
		this.label77.Text = "Doc From";
		this.txtFromNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFromNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromNoHistory.Location = new System.Drawing.Point(361, 37);
		this.txtFromNoHistory.Name = "txtFromNoHistory";
		this.txtFromNoHistory.Size = new System.Drawing.Size(97, 23);
		this.txtFromNoHistory.TabIndex = 3;
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.label54);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(3, 30);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1160, 27);
		this.panel9.TabIndex = 47;
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label54.ForeColor = System.Drawing.Color.White;
		this.label54.Location = new System.Drawing.Point(5, 3);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(113, 19);
		this.label54.TabIndex = 35;
		this.label54.Text = "GDN History ";
		this.panel2.Controls.Add(this.toolStrip2);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1160, 27);
		this.panel2.TabIndex = 46;
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.BtnNewHistory, this.BtnRefreshHistory });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1160, 25);
		this.toolStrip2.TabIndex = 27;
		this.toolStrip2.Text = "toolStrip2";
		this.BtnNewHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.BtnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnNewHistory.Name = "BtnNewHistory";
		this.BtnNewHistory.Size = new System.Drawing.Size(55, 22);
		this.BtnNewHistory.Text = "New";
		this.BtnNewHistory.Click += new System.EventHandler(btnLoadAll_Click);
		this.BtnRefreshHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.BtnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefreshHistory.Name = "BtnRefreshHistory";
		this.BtnRefreshHistory.Size = new System.Drawing.Size(76, 22);
		this.BtnRefreshHistory.Text = "Refresh";
		this.BtnRefreshHistory.Click += new System.EventHandler(BtnRefreshHistory_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1174, 670);
		base.Controls.Add(this.tabControl1);
		base.KeyPreview = true;
		base.Name = "GdnDirectConcrete";
		this.Text = "Goods Dispatch Notes";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(InvfrmInvSaleInvoiceDirect_FormClosing);
		base.Load += new System.EventHandler(InvfrmPurchasedirectInvoice_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(InvfrmPurchasedirectInvoice_KeyDown_1);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.tabControl2.ResumeLayout(false);
		this.tabPage3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel16.ResumeLayout(false);
		this.panel16.PerformLayout();
		this.tabPage4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdContractorWages).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbContractorForUpdateInWagesGrid).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbActivityforContractorUpdateInWagesGrid).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAttributeVarient).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comjobLot).EndInit();
		((System.ComponentModel.ISupportInitialize)this.txtCity).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comWarehouse).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comItem).EndInit();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbVehicleType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbVehicleNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTransporterAc).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combdeliverytrm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comsupplier).EndInit();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.comproject).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combranches).EndInit();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel17.ResumeLayout(false);
		this.panel19.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetailHistory).EndInit();
		this.panel18.ResumeLayout(false);
		this.panel18.PerformLayout();
		this.panel10.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistory).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.groupBox4.ResumeLayout(false);
		this.groupBox4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDateTypeHistory).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}
