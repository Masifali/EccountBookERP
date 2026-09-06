// using Architecture.Reports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Export;
using Architecture.BLL.Inventory;
using Architecture.BLL.Production;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.Model.Production;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.Production
{

public class frmProductionJobOrder : Form
{
	private bool DoHaveSaveRight;

	private bool DoHavePrintRights;

	private bool DoHaveUpdateRights;

	private bool DoHaveCanViewAllRecordRights;

	private Attachment AT = new Attachment();

	public DataTable dtViewAttachments = new DataTable();

	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtPlannedOutput = new DataTable();

	private DataTable dtInputDetail = new DataTable();

	private DataTable dtPackType = new DataTable();

	private DataTable dtPackSize = new DataTable();

	private DataTable dtPackingType = new DataTable();

	private DataTable dtJobLot = new DataTable();

	private DataTable dtCropYear = new DataTable();

	private int updateDetailIndexoutput;

	private int updateDetailIndexInput;

	private DataTable table = new DataTable();

	private bool UpdateMode;

	private int RECID;

	private IContainer components;

	private ToolStrip miniToolStrip;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel2;

	private Panel panel3;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnsave;

	private ToolStripButton btnUpdate;

	private ToolStripButton Print;

	private TabPage tabPage2;

	private Panel panel4;

	private Label label11;

	private TextBox txtOtherInstruction;

	private Label label10;

	internal UltraCombo cmbSaleOrder;

	private Label label9;

	private Label label6;

	private TextBox txtPlantInstruction;

	private Label label3;

	private TextBox txtOtherReference;

	private Label label1;

	private TextBox txtInvoiceReference;

	private Label label2;

	private Label label8;

	private Label label7;

	private TextBox txtPlanCode;

	private DateTimePicker txtPlanDate;

	private Label label5;

	private Panel panel6;

	private Label label4;

	private TextBox txtProductionInstruction;

	private Label label13;

	private Label label17;

	private TextBox txtDeliveryInstruction;

	private DateTimePicker txtstartDate;

	private Label label16;

	private DateTimePicker txtEndDate;

	private Label label18;

	internal UltraCombo cmbPlantFeader;

	private Label label19;

	private TextBox txtPlanTypeSrNo;

	private Panel panel12;

	internal GridEX DataGridHistory;

	private Panel panel8;

	private ToolStrip toolStrip2;

	private ToolStripButton btnNewHistory;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripButton toolStripButton2;

	private ToolStripButton toolStripButton3;

	private Panel panel5;

	private Panel panel13;

	internal GridEX grdOutPut;

	private Panel panel11;

	internal GridEX grdInput;

	internal GridEX grdPlannedOutput;

	internal GridEX grdInputDetail;

	private ToolStripButton btnattachment;

	internal UltraCombo cmbPlanType;

	internal UltraCombo cmbProductionType;

	internal UltraCombo cmbStatus;

	private CheckBox chkPrint;

	internal UltraCombo CmbLotAc;

	private Label label22;

	internal UltraCombo CmbByProductionAc;

	private Label label21;

	internal UltraCombo CmbFinishGoodsAc;

	private Label label20;

	internal UltraCombo CmbWorkInProcess;

	private Label label15;

	internal UltraCombo CmbWipItem;

	private Label label23;

	internal UltraCombo cmbItem;

	private Label label37;

	internal UltraCombo cmbPackingType;

	private Label label35;

	private TextBox txtOuterqty;

	private Label label26;

	private TextBox txtinnerqty;

	private Label label38;

	internal UltraCombo cmbouteruom;

	private Label label25;

	internal UltraCombo cmbinneruom;

	private Label label24;

	private TextBox txtremarks;

	private Label label27;

	private TextBox txtweight;

	private Label label39;

	private Button btnUpdateDetail;

	private Button btnplus;

	private Button btnCancelUpdateDetial;

	internal UltraCombo cmbFiltersType;

	private Label cmbfiltertype;

	internal UltraCombo cmbDocNo;

	private Label label28;

	internal UltraCombo cmbcropyearinput;

	private Label label30;

	internal UltraCombo cmbItemInput;

	private Label label29;

	private TextBox txtweightinput;

	private Label label33;

	private TextBox txtqtyinput;

	private Label label31;

	internal UltraCombo cmbpacksizeinput;

	private Label label32;

	internal UltraCombo cmbjoblotinput;

	private Label label45;

	private TextBox txtremarksinput;

	private Label label36;

	internal UltraCombo cmbwarehouseinput;

	private Label label34;

	private Button btncancelgrdinput;

	private Button btnupdategrdinput;

	private Button btnadddetailinputgrd;

	private Label label40;

	internal UltraCombo cmbentrytype;

	private Panel panelinputgrd;

	private Panel paneloutputgrd;

	private Button btnProductionPlant;

	private TabControl tabControl2;

	private TabPage tabPage3;

	private Panel panel14;

	private TabPage tabPage4;

	private Panel panel15;

	private ToolStripButton btnRefresh;

	private Panel panel7;

	private Panel panel9;

	internal UltraCombo cmbWareHouseOutPut;

	private Label label12;

	private Label label14;

	internal UltraCombo cmbCropYearOutput;

	private Label label41;

	internal UltraCombo cmbJobLotOutput;

	private Label label42;

	internal UltraCombo cmbPackTypeInput;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripButton btnloadall;

	public frmProductionJobOrder(UserAccount user)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void frmProductionJobOrder_Load(object sender, EventArgs e)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		try
		{
			dtViewAttachments.Columns.Add("AttchmentPath");
			List<tblUserRights> lstRights = tblUserRights.GetByUserId(new tblUserRights
			{
				UserId = UserAccount.ID,
				ScreenName = base.Name,
				RightName = UserAccount.RoleName
			});
			if (lstRights.Count > 0)
			{
				List<tblUserRights> newList = lstRights.Where((tblUserRights x) => x.RightName == "Save").ToList();
				DoHaveSaveRight = Conversion.ToBool((object)newList[0].Value.ToString());
				newList = lstRights.Where((tblUserRights x) => x.RightName == "Update").ToList();
				DoHaveUpdateRights = Conversion.ToBool((object)newList[0].Value.ToString());
				newList = lstRights.Where((tblUserRights x) => x.RightName == "CanView AllRecord").ToList();
				DoHaveCanViewAllRecordRights = Conversion.ToBool((object)newList[0].Value.ToString());
			}
			btnsave.Enabled = DoHaveSaveRight;
			btnUpdate.Enabled = DoHaveUpdateRights;
			ItemFill();
			JobLotBind();
			AccountFills();
			PlanTypeBind();
			GeneratePlanCode();
			PlantFeader();
			SaleOrder();
			ProductionTypeBind();
			StatusBind();
			EntryTypeFill();
			CropYear();
			WareHouseFill();
			ItemDetailFill();
			CmbPackingTypeFill();
			FilterTypeFill();
			dtPlannedOutput.Columns.Add("ItemId");
			dtPlannedOutput.Columns.Add("ItemName");
			dtPlannedOutput.Columns.Add("PackingTypeId");
			dtPlannedOutput.Columns.Add("PackingTypeName");
			dtPlannedOutput.Columns.Add("QtyInner", typeof(double));
			dtPlannedOutput.Columns.Add("InnerPackSizeId");
			dtPlannedOutput.Columns.Add("InnerPackSizeName");
			dtPlannedOutput.Columns.Add("QtyOuter", typeof(double));
			dtPlannedOutput.Columns.Add("OuterPackSizeId");
			dtPlannedOutput.Columns.Add("OuterPackSizeName");
			dtPlannedOutput.Columns.Add("Weight", typeof(double));
			dtPlannedOutput.Columns.Add("PackRemarks");
			dtPlannedOutput.Columns.Add("CropYearId");
			dtPlannedOutput.Columns.Add("CropYear");
			dtPlannedOutput.Columns.Add("WareHouseId");
			dtPlannedOutput.Columns.Add("WareHouseName");
			dtPlannedOutput.Columns.Add("JobLotId");
			dtPlannedOutput.Columns.Add("JobLotDescription");
			dtPackType.Columns.Add("Id");
			dtPackType.Columns.Add("PackType");
			dtPackSize.Columns.Add("Id");
			dtPackSize.Columns.Add("PackSize");
			grdPlannedOutput.DataSource = dtPlannedOutput;
			grdPlannedOutput.RetrieveStructure();
			dtInputDetail.Columns.Add("ItemId");
			dtInputDetail.Columns.Add("ItemName");
			dtInputDetail.Columns.Add("CropYearId");
			dtInputDetail.Columns.Add("CropYear");
			dtInputDetail.Columns.Add("JobLotId");
			dtInputDetail.Columns.Add("JobLot");
			dtInputDetail.Columns.Add("Qty", typeof(double));
			dtInputDetail.Columns.Add("PackSizeId");
			dtInputDetail.Columns.Add("PackSize");
			dtInputDetail.Columns.Add("Weight", typeof(double));
			dtInputDetail.Columns.Add("WareHouseId");
			dtInputDetail.Columns.Add("WareHouse");
			dtInputDetail.Columns.Add("Remarks");
			dtInputDetail.Columns.Add("PackTypeId");
			dtInputDetail.Columns.Add("PackType");
			grdInputDetail.DataSource = dtInputDetail;
			grdInputDetail.RetrieveStructure();
			txtPlanDate.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CropYear()
	{
		try
		{
			dtCropYear = CommonServices.CropYearGetAllService();
			if (dtCropYear.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtCropYear, cmbcropyearinput, "Id", "CropYear", "Crop Year", false);
				DDL.BindDDLNew(dtCropYear, cmbCropYearOutput, "Id", "CropYear", "Crop Year", false);
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
			DataTable dtWh = CommonServices.getActiveWareHouse();
			if (dtWh.Rows.Count > 0)
			{
				DDL.BindDDL(dtWh, cmbwarehouseinput, "Id", "WareHouseName", "WareHouseName", ZeroIndex: false);
				DDL.BindDDL(dtWh, cmbWareHouseOutPut, "Id", "WareHouseName", "WareHouseName", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PlantFeader()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		try
		{
			DataTable dtProductionplant = new DataTable();
			dtProductionplant = InvProductionPlant.GetAll(new InvProductionPlant
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtProductionplant.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtProductionplant, cmbPlantFeader, "Id", "Description", "Plant/Feader ", false);
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
			dtJobLot = CommonServices.JobLotGetAllService();
			if (dtJobLot.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtJobLot, cmbjoblotinput, "Id", "JobLotDescription", "JobLotDescription", false);
				DDL.BindDDLNew(dtJobLot, cmbJobLotOutput, "Id", "JobLotDescription", "JobLotDescription", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void EntryTypeFill()
	{
		try
		{
			DataTable dtentryType = new DataTable();
			dtentryType.Columns.Add("Id");
			dtentryType.Columns.Add("name");
			dtentryType.Rows.Add(1, "Input");
			dtentryType.Rows.Add(2, "Output");
			DDL.BindDDL(dtentryType, cmbentrytype, "Id", "name", "Entry Type", ZeroIndex: false);
			((UltraGridBase)cmbentrytype).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbentrytype_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (((Control)(object)cmbentrytype).Text == "Input")
			{
				tabControl2.SelectedIndex = 1;
			}
			if (((Control)(object)cmbentrytype).Text == "Output")
			{
				tabControl2.SelectedIndex = 0;
			}
			cmbDocNo_Leave(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FilterTypeFill()
	{
		try
		{
			DataTable dtfiltertype = new DataTable();
			dtfiltertype.Columns.Add("Id");
			dtfiltertype.Columns.Add("name");
			dtfiltertype.Rows.Add(1, "SaleOrder");
			dtfiltertype.Rows.Add(2, "Export Contract");
			dtfiltertype.Rows.Add(3, "Item Type");
			dtfiltertype.Rows.Add(4, "Item Category");
			DDL.BindDDL(dtfiltertype, cmbFiltersType, "Id", "name", "Filter Type", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbFiltersType_Leave(object sender, EventArgs e)
	{
		try
		{
			cmbDocNo.Value = null;
			((Control)(object)cmbDocNo).Text = null;
			if (((Control)(object)cmbFiltersType).Text == "SaleOrder")
			{
				GetLocalSaleOrderNo();
			}
			if (((Control)(object)cmbFiltersType).Text == "Export Contract")
			{
				GetExportContractNo();
			}
			if (((Control)(object)cmbFiltersType).Text == "Item Type")
			{
				GetItemTypes();
			}
			if (((Control)(object)cmbFiltersType).Text == "Item Category")
			{
				GetItemCategory();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbDocNo_Leave(object sender, EventArgs e)
	{
		//IL_0442: Unknown result type (might be due to invalid IL or missing references)
		//IL_0447: Unknown result type (might be due to invalid IL or missing references)
		//IL_0458: Unknown result type (might be due to invalid IL or missing references)
		//IL_046e: Expected O, but got Unknown
		//IL_05e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0613: Expected O, but got Unknown
		try
		{
			if (((Control)(object)cmbFiltersType).Text != null && ((Control)(object)cmbentrytype).Text == "Output")
			{
				if (((Control)(object)cmbFiltersType).Text == "Item Category" && Conversion.ToInt(cmbDocNo.Value) > Conversion.ToInt((object)0))
				{
					DataTable dt4 = CommonServices.ItemGetAllServiceBind();
					DataTable dtItems4 = new DataTable();
					dtItems4.Columns.Add("Id");
					dtItems4.Columns.Add("ItemName");
					if (dt4.Rows.Count > 0)
					{
						((UltraGridBase)cmbItem).DataSource = null;
						((Control)(object)cmbItem).Text = string.Empty;
						for (int l = 0; l < dt4.Rows.Count; l++)
						{
							if (Conversion.ToInt(cmbDocNo.Value) == Conversion.ToInt(dt4.Rows[l]["ItemCategoryId"]))
							{
								dtItems4.Rows.Add(dt4.Rows[l]["Id"], dt4.Rows[l]["ItemName"]);
							}
						}
						DDL.BindDDLNew(dtItems4, cmbItem, "Id", "ItemName", "Item Name", false);
						((UltraGridBase)cmbItem).DisplayLayout.Bands[0].Columns[1].Width = 500;
					}
				}
				if (((Control)(object)cmbFiltersType).Text == "Item Type" && Conversion.ToInt(cmbDocNo.Value) > Conversion.ToInt((object)0))
				{
					DataTable dt3 = CommonServices.ItemGetAllServiceBind();
					DataTable dtItems3 = new DataTable();
					dtItems3.Columns.Add("Id");
					dtItems3.Columns.Add("ItemName");
					if (dt3.Rows.Count > 0)
					{
						((UltraGridBase)cmbItem).DataSource = null;
						((Control)(object)cmbItem).Text = string.Empty;
						for (int k = 0; k < dt3.Rows.Count; k++)
						{
							if (Conversion.ToInt(cmbDocNo.Value) == Conversion.ToInt(dt3.Rows[k]["ItemTypeId"]))
							{
								dtItems3.Rows.Add(dt3.Rows[k]["Id"], dt3.Rows[k]["ItemName"]);
							}
						}
						DDL.BindDDLNew(dtItems3, cmbItem, "Id", "ItemName", "Item Name", false);
						((UltraGridBase)cmbItem).DisplayLayout.Bands[0].Columns[1].Width = 500;
					}
				}
				if (((Control)(object)cmbFiltersType).Text == "SaleOrder" && Conversion.ToInt(cmbDocNo.Value) > Conversion.ToInt((object)0))
				{
					ItemDetailFillBySaleOrder();
				}
				if (((Control)(object)cmbFiltersType).Text == "Export Contract" && Conversion.ToInt(cmbDocNo.Value) > Conversion.ToInt((object)0))
				{
					ItemDetailFillByExportSaleContractId();
				}
			}
			else if (((Control)(object)cmbFiltersType).Text == null || ((Control)(object)cmbDocNo).Text == null || ((Control)(object)cmbFiltersType).Text == string.Empty || ((Control)(object)cmbDocNo).Text == string.Empty)
			{
				ItemDetailFill();
			}
			if (((Control)(object)cmbFiltersType).Text != null && ((Control)(object)cmbentrytype).Text == "Input")
			{
				if (((Control)(object)cmbFiltersType).Text == "Item Category" && Conversion.ToInt(cmbDocNo.Value) > Conversion.ToInt((object)0))
				{
					DataTable dt2 = Item.ReadAllItems(new Item
					{
						OrganizationId = UserAccount.OrganizationId,
						CompanyId = UserAccount.CompanyId
					});
					DataTable dtItems2 = new DataTable();
					dtItems2.Columns.Add("Id");
					dtItems2.Columns.Add("ItemName");
					if (dt2.Rows.Count > 0)
					{
						((UltraGridBase)cmbItemInput).DataSource = null;
						((Control)(object)cmbItemInput).Text = string.Empty;
						for (int j = 0; j < dt2.Rows.Count; j++)
						{
							if (Conversion.ToInt(cmbDocNo.Value) == Conversion.ToInt(dt2.Rows[j]["ItemCategoryId"]))
							{
								dtItems2.Rows.Add(dt2.Rows[j]["Id"], dt2.Rows[j]["ItemName"]);
							}
						}
						DDL.BindDDL(dtItems2, cmbItemInput, "Id", "ItemName", "Item Name", ZeroIndex: false);
						((UltraGridBase)cmbItem).DisplayLayout.Bands[0].Columns[1].Width = 500;
					}
				}
				if (((Control)(object)cmbFiltersType).Text == "Item Type" && Conversion.ToInt(cmbDocNo.Value) > Conversion.ToInt((object)0))
				{
					DataTable dt = Item.ReadAllItems(new Item
					{
						OrganizationId = UserAccount.OrganizationId,
						CompanyId = UserAccount.CompanyId
					});
					DataTable dtItems = new DataTable();
					dtItems.Columns.Add("Id");
					dtItems.Columns.Add("ItemName");
					if (dt.Rows.Count > 0)
					{
						((UltraGridBase)cmbItemInput).DataSource = null;
						((Control)(object)cmbItemInput).Text = string.Empty;
						for (int i = 0; i < dt.Rows.Count; i++)
						{
							if (Conversion.ToInt(cmbDocNo.Value) == Conversion.ToInt(dt.Rows[i]["ItemTypeId"]))
							{
								dtItems.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ItemName"]);
							}
						}
						DDL.BindDDL(dtItems, cmbItemInput, "Id", "ItemName", "Item Name", ZeroIndex: false);
						((UltraGridBase)cmbItem).DisplayLayout.Bands[0].Columns[1].Width = 500;
					}
				}
				if (((Control)(object)cmbFiltersType).Text == "SaleOrder" && Conversion.ToInt(cmbDocNo.Value) > Conversion.ToInt((object)0))
				{
					ItemDetailFillBySaleOrderInput();
				}
				if (((Control)(object)cmbFiltersType).Text == "Export Contract" && Conversion.ToInt(cmbDocNo.Value) > Conversion.ToInt((object)0))
				{
					ItemDetailFillByExportSaleContractIdInput();
				}
			}
			else if (((Control)(object)cmbFiltersType).Text == null || ((Control)(object)cmbDocNo).Text == null || ((Control)(object)cmbFiltersType).Text == string.Empty || ((Control)(object)cmbDocNo).Text == string.Empty)
			{
				ItemDetailFill();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemDetailFillBySaleOrder()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		try
		{
			DataTable dtItem = SaleOrder.ReadByPurchaseOrderIDNOrderItemId(new SaleOrder
			{
				Id = Conversion.ToInt(cmbDocNo.Value)
			});
			if (dtItem.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtItem, cmbItem, "OrderItemId", "ItemName", "Item Name", false);
				((UltraGridBase)cmbItem).DisplayLayout.Bands[0].Columns[1].Width = 500;
			}
			else
			{
				((UltraGridBase)cmbItem).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemDetailFillByExportSaleContractId()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		try
		{
			((UltraGridBase)cmbItem).DataSource = null;
			((Control)(object)cmbItem).Text = string.Empty;
			DataTable dtItem = ExImLcOrder.ReadByExportSaleOrderIDNOrderItemId(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Id = Conversion.ToInt(cmbDocNo.Value)
			});
			if (dtItem.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtItem, cmbItem, "ExImItemId", "ItemName", "Item Name", false);
				((UltraGridBase)cmbItem).DisplayLayout.Bands[0].Columns[1].Width = 500;
			}
			else
			{
				((UltraGridBase)cmbItem).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemDetailFillBySaleOrderInput()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		try
		{
			DataTable dtItem = SaleOrder.ReadByPurchaseOrderIDNOrderItemId(new SaleOrder
			{
				Id = Conversion.ToInt(cmbDocNo.Value)
			});
			if (dtItem.Rows.Count > 0)
			{
				((UltraGridBase)cmbItemInput).DataSource = null;
				((Control)(object)cmbItemInput).Text = string.Empty;
				DDL.BindDDLNew(dtItem, cmbItemInput, "OrderItemId", "ItemName", "Item Name", false);
				((UltraGridBase)cmbItem).DisplayLayout.Bands[0].Columns[1].Width = 500;
			}
			else
			{
				((UltraGridBase)cmbItem).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemDetailFillByExportSaleContractIdInput()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		try
		{
			((Control)(object)cmbItemInput).Text = string.Empty;
			((UltraGridBase)cmbItemInput).DataSource = null;
			DataTable dtItem = ExImLcOrder.ReadByExportSaleOrderIDNOrderItemId(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Id = Conversion.ToInt(cmbDocNo.Value)
			});
			if (dtItem.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtItem, cmbItemInput, "ExImItemId", "ItemName", "Item Name", false);
				((UltraGridBase)cmbItemInput).DisplayLayout.Bands[0].Columns[1].Width = 500;
			}
			else
			{
				((UltraGridBase)cmbItemInput).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetExportContractNo()
	{
		try
		{
			DataTable dtexportcontract = CommonServices.GetLcOrderNo();
			if (dtexportcontract.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtexportcontract, cmbDocNo, "Id", "LcOrderNo", "Export ContractNo", false);
			}
			else
			{
				((UltraGridBase)cmbDocNo).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetLocalSaleOrderNo()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtsaleorder = SaleOrder.GetSaleOrderNo(new SaleOrder
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtsaleorder.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtsaleorder, cmbDocNo, "Id", "DocNo", "SaleOrder", false);
			}
			else
			{
				((UltraGridBase)cmbDocNo).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetItemTypes()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtItemTypes = ItemType.Getall(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtItemTypes.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtItemTypes, cmbDocNo, "Id", "TypeDescription", "ItemType", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetItemCategory()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtItemCategory = ItemCategory.Getall(new ItemCategory
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtItemCategory.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtItemCategory, cmbDocNo, "Id", "CategoryDescription", "ItemCategory", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemDetailFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dt = Item.ReadAllItems(new Item
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbItem, "Id", "ItemName", "Item Name", false);
				((UltraGridBase)cmbItem).DisplayLayout.Bands[0].Columns[1].Width = 500;
				DDL.BindDDLNew(dt, cmbItemInput, "Id", "ItemName", "Item Name", false);
				((UltraGridBase)cmbItemInput).DisplayLayout.Bands[0].Columns[1].Width = 500;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbItem_Leave(object sender, EventArgs e)
	{
		bindRateUomAndItemPackUom(Conversion.ToInt(cmbItem.Value));
	}

	public void bindRateUomAndItemPackUom(int ID)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		try
		{
			DataTable dtuomsch = UOMSchedule.SearchByObject(new UOMSchedule
			{
				ItemId = Conversion.ToInt((object)ID),
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtuomsch.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtuomsch, cmbinneruom, "Id", "Equivalent", "Inner UOM", false);
				DDL.BindDDLNew(dtuomsch, cmbouteruom, "Id", "Equivalent", "Outer UOM", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbItemInput_Leave(object sender, EventArgs e)
	{
		bindPackSizeInput(Conversion.ToInt(cmbItemInput.Value));
	}

	public void bindPackSizeInput(int ID)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		try
		{
			DataTable dtuomsch = UOMSchedule.SearchByObject(new UOMSchedule
			{
				ItemId = Conversion.ToInt((object)ID),
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtuomsch.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtuomsch, cmbpacksizeinput, "Id", "Equivalent", "Inner UOM", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbPackingTypeFill()
	{
		try
		{
			dtPackingType = InvPackingType.Getall();
			if (dtPackingType.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtPackingType, cmbPackingType, "Id", "PackTypeDesc", "PackType", false);
				DDL.BindDDLNew(dtPackingType, cmbPackTypeInput, "Id", "PackTypeDesc", "PackType", false);
				((UltraGridBase)cmbPackingType).Rows[0].Activate();
				((UltraGridBase)cmbPackTypeInput).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemFill()
	{
		try
		{
			DataTable dtItem = CommonServices.ItemGetForComboServiceBind();
			if (dtItem.Rows.Count > 0)
			{
				DDL.BindDDL(dtItem, CmbWipItem, "Id", "ItemName", "ItemName", ZeroIndex: false);
				((UltraGridBase)CmbWipItem).DisplayLayout.Bands[0].Columns[1].Width = 500;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AccountFills()
	{
		try
		{
			DataTable dtCoa = CommonServices.CoaAllocationGetForComboServiceBind();
			if (dtCoa.Rows.Count > 0)
			{
				DDL.BindDDL(dtCoa, CmbWorkInProcess, "Id", "AccountTitle", "WorkInProcess", ZeroIndex: false);
				DDL.BindDDL(dtCoa, CmbFinishGoodsAc, "Id", "AccountTitle", "FinishGoodsAc", ZeroIndex: false);
				DDL.BindDDL(dtCoa, CmbByProductionAc, "Id", "AccountTitle", "ByProductionAc", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SaleOrder()
	{
	}

	private void GeneratePlanCode()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		try
		{
			int code = InvProductionJobOrder.GetGenerateCode(new InvProductionJobOrder
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = 401,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			if (code > 0)
			{
				txtPlanCode.Text = code.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PlanTypeBind()
	{
		try
		{
			DataTable dtpt = new DataTable();
			dtpt.Columns.Add("Id");
			dtpt.Columns.Add("Type");
			dtpt.Rows.Add(1, "Local");
			dtpt.Rows.Add(2, "Export");
			dtpt.Rows.Add(3, "Party");
			if (dtpt.Rows.Count > 0)
			{
				DDL.BindDDL(dtpt, cmbPlanType, "Id", "Type", "Plan Type ", ZeroIndex: true);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ProductionTypeBind()
	{
		try
		{
			DataTable dtprot = new DataTable();
			dtprot.Columns.Add("Id");
			dtprot.Columns.Add("ProductionType");
			dtprot.Rows.Add(1, "Husking");
			dtprot.Rows.Add(2, "Processing");
			dtprot.Rows.Add(3, "Other");
			if (dtprot.Rows.Count > 0)
			{
				DDL.BindDDL(dtprot, cmbProductionType, "Id", "ProductionType", "Production Type ", ZeroIndex: true);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void StatusBind()
	{
		try
		{
			DataTable dtstat = new DataTable();
			dtstat.Columns.Add("Id");
			dtstat.Columns.Add("Status");
			dtstat.Rows.Add(1, "In Process");
			dtstat.Rows.Add(2, "Complete");
			dtstat.Rows.Add(3, "Cancel");
			if (dtstat.Rows.Count > 0)
			{
				DDL.BindDDL(dtstat, cmbStatus, "Id", "Status", "Status", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void bindPackType()
	{
		try
		{
			DataTable dtpackingtype = new DataTable();
			dtpackingtype = InvPackingType.Getall();
			dtPackType.Clear();
			for (int i = 0; i < dtpackingtype.Rows.Count; i++)
			{
				dtPackType.Rows.Add(dtpackingtype.Rows[i]["Id"], dtpackingtype.Rows[i]["PackTypeDesc"]);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void bindPackingSize(int ItemId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Expected O, but got Unknown
		try
		{
			UOMSchedule val = new UOMSchedule
			{
				ItemId = ItemId,
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			};
			DataTable dtsearchByItemId = new DataTable();
			dtsearchByItemId = UOMSchedule.SearchByObject(val);
			dtPackSize.Clear();
			for (int i = 0; i < dtsearchByItemId.Rows.Count; i++)
			{
				dtPackSize.Rows.Add(dtsearchByItemId.Rows[i]["Id"], dtsearchByItemId.Rows[i]["Equivalent"]);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void PlannedOutputGridSetting()
	{
		try
		{
			grdPlannedOutput.RootTable.Columns["ItemId"].Visible = false;
			grdPlannedOutput.RootTable.Columns["PackingTypeId"].Visible = false;
			grdPlannedOutput.RootTable.Columns["InnerPackSizeId"].Visible = false;
			grdPlannedOutput.RootTable.Columns["OuterPackSizeId"].Visible = false;
			grdPlannedOutput.RootTable.Columns["CropYearId"].Visible = false;
			grdPlannedOutput.RootTable.Columns["WareHouseId"].Visible = false;
			grdPlannedOutput.RootTable.Columns["JobLotId"].Visible = false;
			grdPlannedOutput.RootTable.Columns["QtyInner"].Width = 60;
			grdPlannedOutput.RootTable.Columns["QtyOuter"].Width = 60;
			grdPlannedOutput.RootTable.Columns["Weight"].Width = 60;
			grdPlannedOutput.RootTable.Columns["PackRemarks"].Width = 200;
			grdPlannedOutput.RootTable.Columns["PackingTypeName"].EditType = (EditType)0;
			grdPlannedOutput.RootTable.Columns["InnerPackSizeName"].EditType = (EditType)0;
			grdPlannedOutput.RootTable.Columns["OuterPackSizeName"].EditType = (EditType)0;
			grdPlannedOutput.RootTable.Columns["Weight"].EditType = (EditType)0;
			grdPlannedOutput.RootTable.Columns["PackRemarks"].EditType = (EditType)0;
			grdPlannedOutput.RootTable.Columns["PackingTypeName"].FilterEditType = (FilterEditType)1;
			grdPlannedOutput.RootTable.Columns["QtyInner"].FilterEditType = (FilterEditType)1;
			grdPlannedOutput.RootTable.Columns["InnerPackSizeName"].FilterEditType = (FilterEditType)1;
			grdPlannedOutput.RootTable.Columns["QtyOuter"].FilterEditType = (FilterEditType)1;
			grdPlannedOutput.RootTable.Columns["OuterPackSizeName"].FilterEditType = (FilterEditType)1;
			grdPlannedOutput.RootTable.Columns["Weight"].FilterEditType = (FilterEditType)1;
			grdPlannedOutput.RootTable.Columns["PackRemarks"].FilterEditType = (FilterEditType)1;
			grdPlannedOutput.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			grdPlannedOutput.RootTable.Columns["QtyOuter"].AggregateFunction = (AggregateFunction)2;
			grdPlannedOutput.RootTable.Columns["QtyInner"].AggregateFunction = (AggregateFunction)2;
			grdPlannedOutput.RootTable.Columns["Weight"].FormatString = "0,0";
			grdPlannedOutput.RootTable.Columns["QtyOuter"].FormatString = "0,0";
			grdPlannedOutput.RootTable.Columns["QtyInner"].FormatString = "0,0";
			grdPlannedOutput.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			grdPlannedOutput.RootTable.Columns["QtyOuter"].TextAlignment = (TextAlignment)3;
			grdPlannedOutput.RootTable.Columns["QtyInner"].TextAlignment = (TextAlignment)3;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void bindPacksize(int ItemId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Expected O, but got Unknown
		try
		{
			UOMSchedule val = new UOMSchedule
			{
				ItemId = ItemId,
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			};
			DataTable dtsearchbyObj = new DataTable();
			dtsearchbyObj = UOMSchedule.SearchByObject(val);
			dtPackType.Clear();
			for (int i = 0; i < dtsearchbyObj.Rows.Count; i++)
			{
				dtPackType.Rows.Add(dtsearchbyObj.Rows[i]["Id"], dtsearchbyObj.Rows[i]["UOMCode"]);
			}
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public void InputDetailGridSetting()
	{
		try
		{
			grdInputDetail.RootTable.Columns["ItemId"].Visible = false;
			grdInputDetail.RootTable.Columns["CropYearId"].Visible = false;
			grdInputDetail.RootTable.Columns["JobLotId"].Visible = false;
			grdInputDetail.RootTable.Columns["PackSizeId"].Visible = false;
			grdInputDetail.RootTable.Columns["WareHouseId"].Visible = false;
			grdInputDetail.RootTable.Columns["PackTypeId"].Visible = false;
			grdInputDetail.RootTable.Columns["Qty"].Width = 70;
			grdInputDetail.RootTable.Columns["Weight"].Width = 70;
			grdInputDetail.RootTable.Columns["Remarks"].Width = 200;
			grdInputDetail.RootTable.Columns["ItemName"].EditType = (EditType)0;
			grdInputDetail.RootTable.Columns["CropYear"].EditType = (EditType)0;
			grdInputDetail.RootTable.Columns["JobLot"].EditType = (EditType)0;
			grdInputDetail.RootTable.Columns["Qty"].EditType = (EditType)0;
			grdInputDetail.RootTable.Columns["PackSize"].EditType = (EditType)0;
			grdInputDetail.RootTable.Columns["Weight"].EditType = (EditType)0;
			grdInputDetail.RootTable.Columns["WareHouse"].EditType = (EditType)0;
			grdInputDetail.RootTable.Columns["Remarks"].EditType = (EditType)0;
			grdInputDetail.RootTable.Columns["ItemName"].FilterEditType = (FilterEditType)1;
			grdInputDetail.RootTable.Columns["CropYear"].FilterEditType = (FilterEditType)1;
			grdInputDetail.RootTable.Columns["JobLot"].FilterEditType = (FilterEditType)1;
			grdInputDetail.RootTable.Columns["Qty"].FilterEditType = (FilterEditType)1;
			grdInputDetail.RootTable.Columns["PackSize"].FilterEditType = (FilterEditType)1;
			grdInputDetail.RootTable.Columns["Weight"].FilterEditType = (FilterEditType)1;
			grdInputDetail.RootTable.Columns["WareHouse"].FilterEditType = (FilterEditType)1;
			grdInputDetail.RootTable.Columns["Remarks"].FilterEditType = (FilterEditType)1;
			grdInputDetail.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdInputDetail.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			grdInputDetail.RootTable.Columns["Weight"].FormatString = "0,0";
			grdInputDetail.RootTable.Columns["Weight"].TotalFormatString = "#,##0.##";
			grdInputDetail.RootTable.Columns["Qty"].FormatString = "0,0";
			grdInputDetail.RootTable.Columns["Qty"].TotalFormatString = "#,##0.##";
			grdInputDetail.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdInputDetail.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
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

	public void Reset()
	{
		try
		{
			dtPlannedOutput.Clear();
			dtInputDetail.Clear();
			txtPlanDate.Value = DateTime.Now;
			txtInvoiceReference.Text = string.Empty;
			txtOtherInstruction.Text = string.Empty;
			txtOtherReference.Text = string.Empty;
			txtPlantInstruction.Text = string.Empty;
			txtProductionInstruction.Text = string.Empty;
			txtDeliveryInstruction.Text = string.Empty;
			((Control)(object)cmbSaleOrder).Text = string.Empty;
			((Control)(object)CmbWorkInProcess).Text = string.Empty;
			((Control)(object)CmbWipItem).Text = string.Empty;
			((Control)(object)CmbFinishGoodsAc).Text = string.Empty;
			((Control)(object)CmbByProductionAc).Text = string.Empty;
			((Control)(object)CmbLotAc).Text = string.Empty;
			cmbProductionType.Value = 0;
			cmbStatus.Value = 0;
			cmbPlanType.Value = 0;
			grdInputDetail.ClearStructure();
			grdPlannedOutput.ClearStructure();
			grdInputDetail.RetrieveStructure();
			grdPlannedOutput.RetrieveStructure();
			InputDetailGridSetting();
			PlannedOutputGridSetting();
			btnUpdate.Visible = false;
			btnsave.Visible = true;
			UpdateMode = false;
			GeneratePlanCode();
			SaleOrder();
			txtPlanDate.Focus();
			((Control)(object)cmbJobLotOutput).Enabled = true;
			((Control)(object)cmbjoblotinput).Enabled = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		Reset();
	}

	private bool FormValidation()
	{
		if (txtPlanCode.Text.Trim() == string.Empty || txtPlanCode.Text.Trim() == "0")
		{
			MessageBox.Show("Plan Code Field Required");
			txtPlanCode.Focus();
			return false;
		}
		if (((UltraGridBase)cmbPlantFeader).ActiveRow == null)
		{
			MessageBox.Show("Plan/Feader Field Required");
			cmbPlantFeader.Focus();
			return false;
		}
		if (txtInvoiceReference.Text == string.Empty || txtInvoiceReference.Text == "0")
		{
			MessageBox.Show("Mannual ReportNo Field Required");
			txtInvoiceReference.Focus();
			return false;
		}
		if (((UltraGridBase)cmbPlanType).ActiveRow == null)
		{
			MessageBox.Show("Plan Type Field Required");
			cmbPlanType.Focus();
			return false;
		}
		if (((UltraGridBase)cmbProductionType).ActiveRow == null)
		{
			MessageBox.Show("Production Type Field Required");
			cmbProductionType.Focus();
			return false;
		}
		if (((UltraGridBase)cmbStatus).ActiveRow == null)
		{
			MessageBox.Show("Status Field Required");
			cmbStatus.Focus();
			return false;
		}
		if (((UltraGridBase)CmbWipItem).ActiveRow == null)
		{
			MessageBox.Show("WIPItem Field Required");
			CmbWipItem.Focus();
			return false;
		}
		return true;
	}

	private bool FormValidationDetailOutPut()
	{
		if (((UltraGridBase)cmbItem).ActiveRow == null)
		{
			MessageBox.Show("Item Name Field is Required");
			cmbItem.Focus();
			return false;
		}
		if (((UltraGridBase)cmbPackingType).ActiveRow == null)
		{
			MessageBox.Show("Item Pack Unit Field is Required");
			cmbPackingType.Focus();
			return false;
		}
		if (((UltraGridBase)cmbinneruom).ActiveRow == null)
		{
			MessageBox.Show("Inner UOM Field is Required");
			cmbinneruom.Focus();
			return false;
		}
		if (txtOuterqty.Text.Trim() == string.Empty || txtOuterqty.Text.Trim() == "0")
		{
			MessageBox.Show("Outer Qty Field is Required");
			txtOuterqty.Focus();
			return false;
		}
		if (((UltraGridBase)cmbouteruom).ActiveRow == null)
		{
			MessageBox.Show("Outer UOM Field is Required");
			cmbouteruom.Focus();
			return false;
		}
		if (txtweight.Text.Trim() == string.Empty || txtweight.Text.Trim() == "0")
		{
			MessageBox.Show("Weight is Required");
			txtweight.Focus();
			return false;
		}
		if (((UltraGridBase)cmbCropYearOutput).ActiveRow == null)
		{
			MessageBox.Show("CropYear is Required");
			cmbCropYearOutput.Focus();
			return false;
		}
		if (((UltraGridBase)cmbWareHouseOutPut).ActiveRow == null)
		{
			MessageBox.Show("WareHouse is Required");
			cmbWareHouseOutPut.Focus();
			return false;
		}
		if (((UltraGridBase)cmbJobLotOutput).ActiveRow == null)
		{
			MessageBox.Show("JobLot is Required");
			cmbJobLotOutput.Focus();
			return false;
		}
		return true;
	}

	private bool FormValidationDetailInPut()
	{
		if (((UltraGridBase)cmbItemInput).ActiveRow == null)
		{
			MessageBox.Show("Item Name Field is Required");
			cmbItemInput.Focus();
			return false;
		}
		if (((UltraGridBase)cmbcropyearinput).ActiveRow == null)
		{
			MessageBox.Show("Cropt Year Field is Required");
			cmbcropyearinput.Focus();
			return false;
		}
		if (((UltraGridBase)cmbjoblotinput).ActiveRow == null)
		{
			MessageBox.Show("Job Lot Field is Required");
			cmbjoblotinput.Focus();
			return false;
		}
		if (txtqtyinput.Text.Trim() == string.Empty || txtqtyinput.Text.Trim() == "0")
		{
			MessageBox.Show("Qty Field is Required");
			txtqtyinput.Focus();
			return false;
		}
		if (((UltraGridBase)cmbpacksizeinput).ActiveRow == null)
		{
			MessageBox.Show("Pack Size Field is Required");
			cmbpacksizeinput.Focus();
			return false;
		}
		if (txtweightinput.Text.Trim() == string.Empty || txtweightinput.Text.Trim() == "0")
		{
			MessageBox.Show("Weight Field is Required");
			txtweightinput.Focus();
			return false;
		}
		if (((UltraGridBase)cmbwarehouseinput).ActiveRow == null)
		{
			MessageBox.Show("Warehouse is Required");
			cmbwarehouseinput.Focus();
			return false;
		}
		if (((UltraGridBase)cmbPackTypeInput).ActiveRow == null)
		{
			MessageBox.Show("PackType Input is Required");
			cmbPackTypeInput.Focus();
			return false;
		}
		return true;
	}

	private void Insert()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ec: Expected O, but got Unknown
		//IL_067c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0683: Expected O, but got Unknown
		//IL_0a5b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a62: Expected O, but got Unknown
		//IL_0b22: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b29: Expected O, but got Unknown
		try
		{
			if (!FormValidation())
			{
				return;
			}
			InvProductionJobOrder invProductionJobOrder = new InvProductionJobOrder();
			if (RECID > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				invProductionJobOrder.Id = RECID;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			invProductionJobOrder.DocumentTypeId = 401;
			invProductionJobOrder.PlanDate = txtPlanDate.Value;
			invProductionJobOrder.PlanCode = Conversion.ToInt((object)txtPlanCode.Text);
			invProductionJobOrder.InvProductionPlantId = Conversion.ToInt(cmbPlantFeader.Value);
			invProductionJobOrder.PlanType = Conversion.ToString((object)((Control)(object)cmbPlanType).Text);
			invProductionJobOrder.PlanTypeSrNo = Conversion.ToInt((object)txtPlanTypeSrNo.Text);
			invProductionJobOrder.RefInvoiceNo = Conversion.ToString((object)txtInvoiceReference.Text);
			invProductionJobOrder.LotReference = Conversion.ToString((object)txtOtherReference.Text);
			invProductionJobOrder.ProductionType = Conversion.ToString((object)((Control)(object)cmbProductionType).Text);
			invProductionJobOrder.RefSalesOrderId = Conversion.ToInt(cmbSaleOrder.Value);
			invProductionJobOrder.WorkInProccessAcId = Conversion.ToInt(CmbWorkInProcess.Value);
			invProductionJobOrder.FinishGoodsAcId = Conversion.ToInt(CmbFinishGoodsAc.Value);
			invProductionJobOrder.ByProductacId = Conversion.ToInt(CmbByProductionAc.Value);
			invProductionJobOrder.JobLotId = Conversion.ToInt(CmbLotAc.Value);
			invProductionJobOrder.WipItemId = Conversion.ToInt(CmbWipItem.Value);
			invProductionJobOrder.StartDate = txtstartDate.Value;
			invProductionJobOrder.EndDate = txtEndDate.Value;
			invProductionJobOrder.PlanStatus = Conversion.ToString((object)((Control)(object)cmbStatus).Text);
			invProductionJobOrder.PackingInstructions = txtPlantInstruction.Text;
			invProductionJobOrder.ProductionInsturction = txtProductionInstruction.Text;
			invProductionJobOrder.DeliveryInstructions = txtDeliveryInstruction.Text;
			invProductionJobOrder.OtherInstructions = txtOtherInstruction.Text;
			invProductionJobOrder.IsApproved = false;
			invProductionJobOrder.SupplierCustomerId = 0;
			invProductionJobOrder.OrganizationId = UserAccount.OrganizationId;
			invProductionJobOrder.CompanyId = UserAccount.CompanyId;
			invProductionJobOrder.EntryUser = UserAccount.ID;
			invProductionJobOrder.ModifyUser = UserAccount.ID;
			invProductionJobOrder.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			invProductionJobOrder.invProductionJobOrderOutput = new List<InvProductionJobOrderOutput>();
			invProductionJobOrder.invProductionJobOrderInput = new List<InvProductionJobOrderInput>();
			invProductionJobOrder.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			if (grdPlannedOutput.GetRows().Count() == 0)
			{
				throw new Exception("OutPut Grid Record Not Found");
			}
			if (grdInputDetail.GetRows().Count() == 0)
			{
				throw new Exception("Input Grid Record Not Found");
			}
			GridEXRow[] rows = grdPlannedOutput.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				InvProductionJobOrderOutput jobOrderOutputdetail = new InvProductionJobOrderOutput();
				if (r2.Cells["ItemId"].Text == string.Empty)
				{
					MessageBox.Show("Item Field Require in Planned Output Detail");
					return;
				}
				jobOrderOutputdetail.ItemId = Conversion.ToInt(r2.Cells["ItemId"].Value);
				if (r2.Cells["PackingTypeName"].Text == string.Empty)
				{
					MessageBox.Show("Packing Type in Planned Output Detail");
					return;
				}
				jobOrderOutputdetail.InvPackingTypeId = Conversion.ToInt((object)r2.Cells["PackingTypeId"].Text);
				if (!(r2.Cells["QtyInner"].Text == string.Empty) && !(r2.Cells["QtyInner"].Text == "0"))
				{
					jobOrderOutputdetail.QtyInner = Conversion.ToDouble((object)r2.Cells["QtyInner"].Text);
				}
				if (!(r2.Cells["InnerPackSizeId"].Text == string.Empty))
				{
					jobOrderOutputdetail.UomSchIdInner = Conversion.ToInt((object)r2.Cells["InnerPackSizeId"].Text);
				}
				if (!(r2.Cells["QtyOuter"].Text == string.Empty) && !(r2.Cells["QtyOuter"].Text == "0"))
				{
					jobOrderOutputdetail.QtyOuter = Conversion.ToDouble((object)r2.Cells["QtyOuter"].Text);
				}
				if (!(r2.Cells["OuterPackSizeId"].Text == string.Empty))
				{
					jobOrderOutputdetail.UomSchIdOuter = Conversion.ToInt((object)r2.Cells["OuterPackSizeId"].Text);
				}
				if (!(r2.Cells["Weight"].Text == string.Empty))
				{
					jobOrderOutputdetail.NetWeight = Conversion.ToDouble((object)r2.Cells["Weight"].Text);
				}
				jobOrderOutputdetail.PackRemarks = Conversion.ToString((object)r2.Cells["PackRemarks"].Text);
				if (r2.Cells["CropYearId"].Text == string.Empty)
				{
					MessageBox.Show("CropYear Field Require in Planned Output Detail");
					return;
				}
				jobOrderOutputdetail.CropYearId = Conversion.ToInt((object)r2.Cells["CropYearId"].Text);
				if (r2.Cells["WareHouseId"].Text == string.Empty)
				{
					MessageBox.Show("WareHouse Field Require in Planned Output Detail");
					return;
				}
				jobOrderOutputdetail.WarehouseId = Conversion.ToInt((object)r2.Cells["WareHouseId"].Text);
				if (r2.Cells["JobLotId"].Text == string.Empty)
				{
					MessageBox.Show("JobLot Field Require in Planned Output Detail");
					return;
				}
				jobOrderOutputdetail.JobLotId = Conversion.ToInt((object)r2.Cells["JobLotId"].Text);
				invProductionJobOrder.invProductionJobOrderOutput.Add(jobOrderOutputdetail);
			}
			rows = grdInputDetail.GetRows();
			foreach (GridEXRow r in rows)
			{
				InvProductionJobOrderInput jobOrderInputdetail = new InvProductionJobOrderInput();
				if (Conversion.ToInt(r.Cells["ItemId"].Value) == 0 || r.Cells["ItemId"].Text == string.Empty)
				{
					MessageBox.Show("Item Field Require in Planned Input Detail");
					return;
				}
				jobOrderInputdetail.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
				if (r.Cells["CropYear"].Text == string.Empty)
				{
					MessageBox.Show("CropYear Field Require in Planned Input Detail");
					return;
				}
				jobOrderInputdetail.CropYearId = Conversion.ToInt(r.Cells["CropYearId"].Value);
				jobOrderInputdetail.BatchCrop = Conversion.ToString((object)r.Cells["CropYear"].Text);
				if (Conversion.ToInt(r.Cells["JobLotId"].Value) == 0 || r.Cells["JobLotId"].Text == string.Empty)
				{
					MessageBox.Show("JobLot Field Require in Planned Input Detail");
					return;
				}
				jobOrderInputdetail.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
				if (r.Cells["Qty"].Text == string.Empty || r.Cells["Qty"].Text == "0")
				{
					MessageBox.Show("Quantity Field Require in Planned Input Detail");
					return;
				}
				jobOrderInputdetail.Qty = Conversion.ToDouble((object)r.Cells["Qty"].Text);
				if (r.Cells["PackSizeId"].Text == string.Empty || Conversion.ToInt(r.Cells["PackSizeId"].Value) == 0)
				{
					MessageBox.Show("PackSize Field Require in Planned Input Detail");
					return;
				}
				jobOrderInputdetail.UomSchIdQty = Conversion.ToInt(r.Cells["PackSizeId"].Value);
				if (r.Cells["Weight"].Text == string.Empty || r.Cells["Weight"].Text == "0")
				{
					MessageBox.Show("Weight Field Require in Planned Input Detail");
					return;
				}
				jobOrderInputdetail.NetWeight = Conversion.ToDouble((object)r.Cells["Weight"].Text);
				if (Conversion.ToInt(r.Cells["WareHouseId"].Value) == 0 || r.Cells["WareHouseId"].Text == string.Empty)
				{
					MessageBox.Show("WareHouse Field Require in Planned Input Detail");
					return;
				}
				jobOrderInputdetail.WarehouseId = Conversion.ToInt(r.Cells["WareHouseId"].Value);
				if (Conversion.ToInt(r.Cells["PackTypeId"].Value) == 0 || r.Cells["PackTypeId"].Text == string.Empty)
				{
					MessageBox.Show("Packing Type Field Require in Planned Input Detail");
					return;
				}
				jobOrderInputdetail.PackingTypeId = Conversion.ToInt(r.Cells["PackTypeId"].Value);
				jobOrderInputdetail.RemarksInputDt = Conversion.ToString((object)r.Cells["Remarks"].Text);
				invProductionJobOrder.invProductionJobOrderInput.Add(jobOrderInputdetail);
			}
			for (int i = 0; i < Attachment.dtAttachments.Rows.Count; i++)
			{
				DMSAttachments attach = new DMSAttachments();
				attach.Attachment = Attachment.dtAttachments.Rows[i][1].ToString();
				attach.ScreenName = base.Name;
				invProductionJobOrder.AttachmentsList.Add(attach);
			}
			int success = InvProductionJobOrder.Save(invProductionJobOrder);
			if (RECID == 0)
			{
				MessageBox.Show("Save Successfully" + invProductionJobOrder.PlanCode);
			}
			else
			{
				MessageBox.Show("Update Successfully" + invProductionJobOrder.PlanCode);
				UpdateMode = false;
			}
			Reset();
			if (chkPrint.Checked)
			{
				Reporting rpt = new Reporting();
				if (UserAccount.CompAddress != null)
				{
					rpt.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				}
				if (UserAccount.CompName != null)
				{
					rpt.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				}
				rpt.RptPerameter("@OrganizationId", UserAccount.OrganizationId.ToString());
				if (UserAccount.CompanyId != 0)
				{
					rpt.RptPerameter("@CompanyId", UserAccount.CompanyId.ToString());
				}
				rpt.RptPerameter("@hId", success.ToString());
				rpt.ShowReport("RptInvProductionJobOrderSlipA.rpt");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		try
		{
			RECID = 0;
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
			if (RECID == 0)
			{
				throw new Exception("Record Not Update Because Record Not Found");
			}
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_DoubleClick(object sender, EventArgs e)
	{
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Expected O, but got Unknown
		try
		{
			btnsave.Visible = false;
			btnUpdate.Visible = true;
			dtPlannedOutput.Clear();
			dtInputDetail.Clear();
			grdPlannedOutput.ClearStructure();
			grdInputDetail.ClearStructure();
			InvProductionJobOrder head = new InvProductionJobOrder();
			RECID = Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells[0].Value.ToString());
			head = InvProductionJobOrder.GetByID(RECID);
			tabControl1.SelectedIndex = 0;
			head.Id = RECID;
			txtPlanDate.Value = head.PlanDate;
			txtPlanCode.Text = Conversion.ToString((object)head.PlanCode);
			cmbPlantFeader.Value = Conversion.ToInt((object)head.InvProductionPlantId);
			CmbWorkInProcess.Value = Conversion.ToInt((object)head.WorkInProccessAcId);
			CmbFinishGoodsAc.Value = Conversion.ToInt((object)head.FinishGoodsAcId);
			CmbByProductionAc.Value = Conversion.ToInt((object)head.ByProductacId);
			CmbLotAc.Value = Conversion.ToInt((object)head.JobLotId);
			CmbWipItem.Value = Conversion.ToInt((object)head.WipItemId);
			((Control)(object)cmbPlanType).Text = Conversion.ToString((object)head.PlanType);
			txtPlanTypeSrNo.Text = Conversion.ToString((object)head.PlanTypeSrNo);
			txtInvoiceReference.Text = Conversion.ToString((object)head.RefInvoiceNo);
			txtOtherReference.Text = Conversion.ToString((object)head.LotReference);
			((Control)(object)cmbProductionType).Text = Conversion.ToString((object)head.ProductionType);
			cmbSaleOrder.Value = Conversion.ToInt((object)head.RefSalesOrderId);
			txtstartDate.Value = Conversion.ToDateTime((object)head.StartDate);
			txtEndDate.Value = Conversion.ToDateTime((object)head.EndDate);
			((Control)(object)cmbStatus).Text = Conversion.ToString((object)head.PlanStatus);
			txtProductionInstruction.Text = head.ProductionInsturction;
			txtDeliveryInstruction.Text = head.DeliveryInstructions;
			txtPlantInstruction.Text = head.PackingInstructions;
			txtOtherInstruction.Text = head.OtherInstructions;
			for (int j = 0; j < head.invProductionJobOrderInput.Count; j++)
			{
				dtInputDetail.Rows.Add(head.invProductionJobOrderInput[j].ItemId, head.invProductionJobOrderInput[j].ItemName, head.invProductionJobOrderInput[j].CropYearId, head.invProductionJobOrderInput[j].BatchCrop, head.invProductionJobOrderInput[j].JobLotId, head.invProductionJobOrderInput[j].JobLotCode, head.invProductionJobOrderInput[j].Qty, head.invProductionJobOrderInput[j].UomSchIdQty, head.invProductionJobOrderInput[j].UOMCode, head.invProductionJobOrderInput[j].NetWeight, head.invProductionJobOrderInput[j].WarehouseId, head.invProductionJobOrderInput[j].WareHouseName, head.invProductionJobOrderInput[j].RemarksInputDt, head.invProductionJobOrderInput[j].PackingTypeId, head.invProductionJobOrderInput[j].PackingType);
			}
			for (int i = 0; i < head.invProductionJobOrderOutput.Count; i++)
			{
				dtPlannedOutput.Rows.Add(head.invProductionJobOrderOutput[i].ItemId, head.invProductionJobOrderOutput[i].ItemName, head.invProductionJobOrderOutput[i].InvPackingTypeId, head.invProductionJobOrderOutput[i].PackTypeDesc, head.invProductionJobOrderOutput[i].QtyInner, head.invProductionJobOrderOutput[i].UomSchIdInner, head.invProductionJobOrderOutput[i].InnerUomCode, head.invProductionJobOrderOutput[i].QtyOuter, head.invProductionJobOrderOutput[i].UomSchIdOuter, head.invProductionJobOrderOutput[i].outerUOMCode, head.invProductionJobOrderOutput[i].NetWeight, head.invProductionJobOrderOutput[i].PackRemarks, head.invProductionJobOrderOutput[i].CropYearId, head.invProductionJobOrderOutput[i].CropYear, head.invProductionJobOrderOutput[i].WarehouseId, head.invProductionJobOrderOutput[i].WareHouseName, head.invProductionJobOrderOutput[i].JobLotId, head.invProductionJobOrderOutput[i].JobLotDescription);
			}
			List<DMSAttachments> byID = global::Architecture.BLL.DMSAttachments.GetByID(int.Parse(RECID.ToString()), base.Name);
			AT = new Attachment();
			foreach (global::Architecture.Model.DMSAttachments Attachment in byID)
			{
				TempAttachments TA = new TempAttachments();
				TA.Id = 0;
				TA.Path = Attachment.Attachment;
				AT.lst.Add(TA);
			}
			UpdateMode = true;
			grdInputDetail.DataSource = dtInputDetail;
			grdInputDetail.RetrieveStructure();
			grdPlannedOutput.DataSource = dtPlannedOutput;
			grdPlannedOutput.RetrieveStructure();
			InputDetailGridSetting();
			PlannedOutputGridSetting();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindHistoryGrid(int NoOfRecords = 0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected O, but got Unknown
		try
		{
			ReportsParameters val = new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = 401,
				CanViewAllRecord = DoHaveCanViewAllRecordRights,
				NoOfRecords = NoOfRecords
			};
			DataTable dtGetAll = new DataTable();
			dtGetAll = InvProductionJobOrder.FormHistory(val);
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Date");
			dt.Columns.Add("Code");
			dt.Columns.Add("Type");
			dt.Columns.Add("InvRef");
			dt.Columns.Add("PType");
			dt.Columns.Add("SO");
			dt.Columns.Add("StartDate");
			dt.Columns.Add("EndDate");
			dt.Columns.Add("PlanStatus");
			dt.Columns.Add("PlantInst");
			dt.Columns.Add("DelInst");
			dt.Columns.Add("ProdInst");
			dt.Columns.Add("OtherInst");
			dt.Columns.Add("Attachments");
			for (int i = 0; i < dtGetAll.Rows.Count; i++)
			{
				dt.Rows.Add(dtGetAll.Rows[i]["Id"], Conversion.ToDateTime(dtGetAll.Rows[i]["PlanDate"]).ToString("dd-MMM-yyy"), dtGetAll.Rows[i]["PlanCode"], dtGetAll.Rows[i]["PlanType"], dtGetAll.Rows[i]["RefInvoiceNo"], dtGetAll.Rows[i]["ProductionType"], dtGetAll.Rows[i]["RefSalesOrderId"], Conversion.ToDateTime(dtGetAll.Rows[i]["StartDate"]).ToString("dd-MMM-yyy"), Conversion.ToDateTime(dtGetAll.Rows[i]["EndDate"]).ToString("dd-MMM-yyy"), dtGetAll.Rows[i]["PlanStatus"], dtGetAll.Rows[i]["PackingInstructions"], dtGetAll.Rows[i]["DeliveryInstructions"], dtGetAll.Rows[i]["ProductionInsturction"], dtGetAll.Rows[i]["OtherInstructions"], dtGetAll.Rows[i]["NoOfAttachments"]);
			}
			DataGridHistory.DataSource = dt;
			DataGridHistory.RetrieveStructure();
			HgridSetting();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void HgridSetting()
	{
		try
		{
			DataGridHistory.RootTable.Columns["Id"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["Date"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["Code"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["Type"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["InvRef"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["PType"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["SO"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["StartDate"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["EndDate"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["PlanStatus"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["PlantInst"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["DelInst"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["ProdInst"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["OtherInst"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["Id"].FilterEditType = (FilterEditType)0;
			DataGridHistory.RootTable.Columns["Date"].FilterEditType = (FilterEditType)0;
			DataGridHistory.RootTable.Columns["Code"].FilterEditType = (FilterEditType)0;
			DataGridHistory.RootTable.Columns["Type"].FilterEditType = (FilterEditType)0;
			DataGridHistory.RootTable.Columns["InvRef"].FilterEditType = (FilterEditType)0;
			DataGridHistory.RootTable.Columns["PType"].FilterEditType = (FilterEditType)0;
			DataGridHistory.RootTable.Columns["SO"].FilterEditType = (FilterEditType)0;
			DataGridHistory.RootTable.Columns["StartDate"].FilterEditType = (FilterEditType)0;
			DataGridHistory.RootTable.Columns["EndDate"].FilterEditType = (FilterEditType)0;
			DataGridHistory.RootTable.Columns["PlanStatus"].FilterEditType = (FilterEditType)0;
			DataGridHistory.RootTable.Columns["PlantInst"].FilterEditType = (FilterEditType)0;
			DataGridHistory.RootTable.Columns["DelInst"].FilterEditType = (FilterEditType)0;
			DataGridHistory.RootTable.Columns["ProdInst"].FilterEditType = (FilterEditType)0;
			DataGridHistory.RootTable.Columns["OtherInst"].FilterEditType = (FilterEditType)0;
			DataGridHistory.RootTable.Columns.Add("Detail");
			DataGridHistory.RootTable.Columns["Detail"].Key = "Detail";
			DataGridHistory.RootTable.Columns["Detail"].Caption = "Detail";
			DataGridHistory.RootTable.Columns["Detail"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Detail"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Detail"].ButtonText = "Detail";
			DataGridHistory.RootTable.Columns["Detail"].Width = 50;
			DataGridHistory.RootTable.Columns["Id"].Visible = false;
			DataGridHistory.RootTable.Columns["Date"].Width = 100;
			DataGridHistory.RootTable.Columns["Code"].Width = 50;
			DataGridHistory.RootTable.Columns["Type"].Width = 70;
			DataGridHistory.RootTable.Columns["InvRef"].Width = 60;
			DataGridHistory.RootTable.Columns["PType"].Width = 80;
			DataGridHistory.RootTable.Columns["SO"].Width = 50;
			DataGridHistory.RootTable.Columns["StartDate"].Width = 100;
			DataGridHistory.RootTable.Columns["EndDate"].Width = 100;
			DataGridHistory.RootTable.Columns["PlanStatus"].Width = 80;
			DataGridHistory.RootTable.Columns["PlantInst"].Width = 150;
			DataGridHistory.RootTable.Columns["DelInst"].Width = 150;
			DataGridHistory.RootTable.Columns["ProdInst"].Width = 150;
			DataGridHistory.RootTable.Columns["OtherInst"].Width = 150;
			DataGridHistory.RootTable.Columns["Attachments"].ColumnType = (ColumnType)5;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmProductionJobOrder_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.KeyData == Keys.Return)
		{
			SendKeys.Send("{TAB}");
		}
		if (e.Control && e.KeyCode == Keys.S && tabControl1.SelectedIndex == 0 && !UpdateMode)
		{
			btnsave_Click(null, null);
		}
		if (e.Control && e.KeyCode == Keys.N)
		{
			if (tabControl1.SelectedIndex == 0)
			{
				btnnew_Click(null, null);
			}
			else
			{
				btnnew_Click(null, null);
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
		if (e.Control && e.KeyCode == Keys.U && UpdateMode)
		{
			btnUpdate_Click(null, null);
		}
	}

	private void cmbPlanType_Leave(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		try
		{
			int code = InvProductionJobOrder.GetGenerateCodePlanType(new InvProductionJobOrder
			{
				PlanType = ((Control)(object)cmbPlanType).Text,
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			if (code > 0)
			{
				txtPlanTypeSrNo.Text = code.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		try
		{
			if (!(e.Column.Key == "Detail"))
			{
				return;
			}
			InvProductionJobOrder head = new InvProductionJobOrder();
			if (Conversion.ToInt(DataGridHistory.CurrentRow.Cells[0].Value) > 0)
			{
				head = InvProductionJobOrder.GetByID(Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells[0].Value.ToString()));
				DataTable dtinput = new DataTable();
				DataTable dtOutput = new DataTable();
				dtOutput.Columns.Add("Item");
				dtOutput.Columns.Add("PackType");
				dtOutput.Columns.Add("Quantity", typeof(double));
				dtOutput.Columns.Add("IUOM");
				dtOutput.Columns.Add("PackRemarks");
				dtOutput.Columns.Add("OQty", typeof(double));
				dtOutput.Columns.Add("OUOM");
				dtOutput.Columns.Add("Weight", typeof(double));
				dtOutput.Columns.Add("WareHouse");
				dtOutput.Columns.Add("CropYear");
				dtOutput.Columns.Add("JobLot");
				dtinput.Columns.Add("Item");
				dtinput.Columns.Add("CropYear");
				dtinput.Columns.Add("JobLot");
				dtinput.Columns.Add("Qty", typeof(double));
				dtinput.Columns.Add("PackSize");
				dtinput.Columns.Add("Weight", typeof(double));
				dtinput.Columns.Add("WareHouse");
				dtinput.Columns.Add("Remarks");
				dtinput.Columns.Add("PackingType");
				for (int j = 0; j < head.invProductionJobOrderInput.Count; j++)
				{
					dtinput.Rows.Add(head.invProductionJobOrderInput[j].ItemName, head.invProductionJobOrderInput[j].BatchCrop, head.invProductionJobOrderInput[j].JobLotCode, head.invProductionJobOrderInput[j].Qty, head.invProductionJobOrderInput[j].UOMCode, head.invProductionJobOrderInput[j].NetWeight, head.invProductionJobOrderInput[j].WareHouseName, head.invProductionJobOrderInput[j].RemarksInputDt, head.invProductionJobOrderInput[j].PackingType);
				}
				for (int i = 0; i < head.invProductionJobOrderOutput.Count; i++)
				{
					dtOutput.Rows.Add(head.invProductionJobOrderOutput[i].ItemName, head.invProductionJobOrderOutput[i].PackTypeDesc, head.invProductionJobOrderOutput[i].QtyInner, head.invProductionJobOrderOutput[i].InnerUomCode, head.invProductionJobOrderOutput[i].PackRemarks, head.invProductionJobOrderOutput[i].QtyOuter, head.invProductionJobOrderOutput[i].outerUOMCode, head.invProductionJobOrderOutput[i].NetWeight, head.invProductionJobOrderOutput[i].WareHouseName, head.invProductionJobOrderOutput[i].CropYear, head.invProductionJobOrderOutput[i].JobLotDescription);
				}
				grdInput.DataSource = dtinput;
				grdInput.RetrieveStructure();
				grdOutPut.DataSource = dtOutput;
				grdOutPut.RetrieveStructure();
				GridSetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void GridSetting()
	{
		try
		{
			grdInput.RootTable.Columns["Item"].EditType = (EditType)0;
			grdInput.RootTable.Columns["CropYear"].EditType = (EditType)0;
			grdInput.RootTable.Columns["JobLot"].EditType = (EditType)0;
			grdInput.RootTable.Columns["Qty"].EditType = (EditType)0;
			grdInput.RootTable.Columns["PackSize"].EditType = (EditType)0;
			grdInput.RootTable.Columns["Weight"].EditType = (EditType)0;
			grdInput.RootTable.Columns["WareHouse"].EditType = (EditType)0;
			grdInput.RootTable.Columns["Remarks"].EditType = (EditType)0;
			grdInput.RootTable.Columns["Item"].FilterEditType = (FilterEditType)1;
			grdInput.RootTable.Columns["CropYear"].FilterEditType = (FilterEditType)1;
			grdInput.RootTable.Columns["JobLot"].FilterEditType = (FilterEditType)1;
			grdInput.RootTable.Columns["Qty"].FilterEditType = (FilterEditType)1;
			grdInput.RootTable.Columns["PackSize"].FilterEditType = (FilterEditType)1;
			grdInput.RootTable.Columns["Weight"].FilterEditType = (FilterEditType)1;
			grdInput.RootTable.Columns["WareHouse"].FilterEditType = (FilterEditType)1;
			grdInput.RootTable.Columns["Remarks"].FilterEditType = (FilterEditType)1;
			grdOutPut.RootTable.Columns["Item"].EditType = (EditType)0;
			grdOutPut.RootTable.Columns["PackType"].EditType = (EditType)0;
			grdOutPut.RootTable.Columns["Quantity"].EditType = (EditType)0;
			grdOutPut.RootTable.Columns["IUOM"].EditType = (EditType)0;
			grdOutPut.RootTable.Columns["PackRemarks"].EditType = (EditType)0;
			grdOutPut.RootTable.Columns["OQty"].EditType = (EditType)0;
			grdOutPut.RootTable.Columns["OUOM"].EditType = (EditType)0;
			grdOutPut.RootTable.Columns["Weight"].EditType = (EditType)0;
			grdOutPut.RootTable.Columns["Item"].FilterEditType = (FilterEditType)1;
			grdOutPut.RootTable.Columns["PackType"].FilterEditType = (FilterEditType)1;
			grdOutPut.RootTable.Columns["Quantity"].FilterEditType = (FilterEditType)1;
			grdOutPut.RootTable.Columns["IUOM"].FilterEditType = (FilterEditType)1;
			grdOutPut.RootTable.Columns["PackRemarks"].FilterEditType = (FilterEditType)1;
			grdOutPut.RootTable.Columns["OQty"].FilterEditType = (FilterEditType)1;
			grdOutPut.RootTable.Columns["OUOM"].FilterEditType = (FilterEditType)1;
			grdOutPut.RootTable.Columns["Weight"].FilterEditType = (FilterEditType)1;
			grdInput.RootTable.Columns["Item"].Width = 250;
			grdInput.RootTable.Columns["CropYear"].Width = 70;
			grdInput.RootTable.Columns["JobLot"].Width = 70;
			grdInput.RootTable.Columns["Qty"].Width = 50;
			grdInput.RootTable.Columns["PackSize"].Width = 70;
			grdInput.RootTable.Columns["Weight"].Width = 60;
			grdInput.RootTable.Columns["WareHouse"].Width = 100;
			grdInput.RootTable.Columns["Remarks"].Width = 200;
			grdOutPut.RootTable.Columns["Item"].Width = 250;
			grdOutPut.RootTable.Columns["PackType"].Width = 120;
			grdOutPut.RootTable.Columns["Quantity"].Width = 70;
			grdOutPut.RootTable.Columns["IUOM"].Width = 50;
			grdOutPut.RootTable.Columns["PackRemarks"].Width = 150;
			grdOutPut.RootTable.Columns["OQty"].Width = 50;
			grdOutPut.RootTable.Columns["OUOM"].Width = 50;
			grdOutPut.RootTable.Columns["Weight"].Width = 60;
			grdInput.RootTable.Columns["Qty"].FormatString = "0,0";
			grdInput.RootTable.Columns["Weight"].FormatString = "0,0";
			grdOutPut.RootTable.Columns["Weight"].FormatString = "0,0";
			grdOutPut.RootTable.Columns["Quantity"].FormatString = "0,0";
			grdOutPut.RootTable.Columns["OQty"].FormatString = "0,0";
			grdInput.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdInput.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			grdOutPut.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			grdOutPut.RootTable.Columns["Quantity"].AggregateFunction = (AggregateFunction)2;
			grdOutPut.RootTable.Columns["OQty"].AggregateFunction = (AggregateFunction)2;
			grdInput.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdInput.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			grdOutPut.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			grdOutPut.RootTable.Columns["Quantity"].TextAlignment = (TextAlignment)3;
			grdOutPut.RootTable.Columns["OQty"].TextAlignment = (TextAlignment)3;
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
			grdInput.ClearStructure();
			grdOutPut.ClearStructure();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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

	private void DataGridHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = DataGridHistory.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			List<global::Architecture.Model.DMSAttachments> dMSList = global::Architecture.BLL.DMSAttachments.GetByID(int.Parse(item.Cells["Id"].Value.ToString().ToString()), base.Name);
			dtViewAttachments.Clear();
			if (dMSList.Count > 0)
			{
				foreach (global::Architecture.Model.DMSAttachments itemList in dMSList)
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

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (tabControl1.SelectedIndex == 1)
		{
			BindHistoryGrid(50);
		}
	}

	private void Print_Click(object sender, EventArgs e)
	{
	}

	private void Getweight()
	{
		try
		{
			double outeruom = Conversion.ToDouble((object)((Control)(object)cmbouteruom).Text.Trim());
			double outerqty = Conversion.ToDouble((object)txtOuterqty.Text.Trim());
			if (outeruom > 0.0 && outerqty > 0.0)
			{
				txtweight.Text = Conversion.ToString((object)(outeruom * outerqty));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetweightInput()
	{
		try
		{
			double uom = Conversion.ToDouble((object)((Control)(object)cmbpacksizeinput).Text.Trim());
			double qty = Conversion.ToDouble((object)txtqtyinput.Text.Trim());
			if (uom > 0.0 && qty > 0.0)
			{
				txtweightinput.Text = Conversion.ToString((object)(uom * qty));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtOuterqty_TextChanged(object sender, EventArgs e)
	{
		try
		{
			Getweight();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbouteruom_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			Getweight();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtqtyinput_TextChanged(object sender, EventArgs e)
	{
		try
		{
			GetweightInput();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbpacksizeinput_VisibleChanged(object sender, EventArgs e)
	{
		try
		{
			GetweightInput();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbpacksizeinput_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			GetweightInput();
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
			if (FormValidationDetailOutPut())
			{
				dtPlannedOutput.Rows.Add(cmbItem.Value, ((Control)(object)cmbItem).Text.Trim(), cmbPackingType.Value, ((Control)(object)cmbPackingType).Text.Trim(), txtOuterqty.Text.Trim(), cmbinneruom.Value, ((Control)(object)cmbinneruom).Text.Trim(), txtOuterqty.Text.Trim(), cmbouteruom.Value, ((Control)(object)cmbouteruom).Text.Trim(), txtweight.Text.Trim(), txtremarks.Text.Trim(), cmbCropYearOutput.Value, ((Control)(object)cmbCropYearOutput).Text.Trim(), cmbWareHouseOutPut.Value, ((Control)(object)cmbWareHouseOutPut).Text.Trim(), cmbJobLotOutput.Value, ((Control)(object)cmbJobLotOutput).Text.Trim());
				grdPlannedOutput.DataSource = dtPlannedOutput;
				grdPlannedOutput.RetrieveStructure();
				PlannedOutputGridSetting();
				((Control)(object)cmbItem).Text = string.Empty;
				((Control)(object)cmbPackingType).Text = string.Empty;
				txtinnerqty.Text = string.Empty;
				((Control)(object)cmbinneruom).Text = string.Empty;
				txtOuterqty.Text = string.Empty;
				((Control)(object)cmbouteruom).Text = string.Empty;
				txtweight.Text = string.Empty;
				txtremarks.Text = string.Empty;
				cmbItem.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPlannedOutput_DoubleClick(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Expected O, but got Unknown
		try
		{
			if (InvProductionJobOrder.CheckJobOrderIdRefToInvFoodProduction(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = 112,
				JobOrderId = RECID
			}) > 0)
			{
				throw new Exception("Record Not Update Because Record has exist in Production OutPut");
			}
			GridEXRow item = grdPlannedOutput.CurrentRow;
			if (item != null)
			{
				updateDetailIndexoutput = item.RowIndex;
				cmbItem.Value = item.Cells["ItemId"].Value.ToString();
				bindRateUomAndItemPackUom(Conversion.ToInt(cmbItem.Value));
				cmbPackingType.Value = item.Cells["PackingTypeId"].Value.ToString();
				txtinnerqty.Text = item.Cells["QtyInner"].Value.ToString();
				cmbinneruom.Value = item.Cells["InnerPackSizeId"].Value.ToString();
				txtOuterqty.Text = item.Cells["QtyOuter"].Value.ToString();
				cmbouteruom.Value = item.Cells["OuterPackSizeId"].Value.ToString();
				txtweight.Text = item.Cells["Weight"].Value.ToString();
				txtremarks.Text = item.Cells["PackRemarks"].Value.ToString();
				cmbCropYearOutput.Value = item.Cells["CropYearId"].Value.ToString();
				cmbWareHouseOutPut.Value = item.Cells["WareHouseId"].Value.ToString();
				cmbJobLotOutput.Value = item.Cells["JobLotId"].Value.ToString();
				btnplus.Visible = false;
				btnUpdateDetail.Visible = true;
				btnCancelUpdateDetial.Visible = true;
				((UltraGridBase)cmbentrytype).Rows[1].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnCancelUpdateDetial_Click(object sender, EventArgs e)
	{
		try
		{
			btnplus.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			((Control)(object)cmbItem).Text = string.Empty;
			((Control)(object)cmbPackingType).Text = string.Empty;
			txtinnerqty.Text = string.Empty;
			((Control)(object)cmbinneruom).Text = string.Empty;
			txtOuterqty.Text = string.Empty;
			((Control)(object)cmbouteruom).Text = string.Empty;
			txtweight.Text = string.Empty;
			txtremarks.Text = string.Empty;
			((Control)(object)cmbWareHouseOutPut).Text = string.Empty;
			((Control)(object)cmbCropYearOutput).Text = string.Empty;
			((Control)(object)cmbJobLotOutput).Text = string.Empty;
			cmbItem.Focus();
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
			if (FormValidationDetailOutPut())
			{
				dtPlannedOutput.Rows[updateDetailIndexoutput]["ItemId"] = cmbItem.Value;
				dtPlannedOutput.Rows[updateDetailIndexoutput]["ItemName"] = ((Control)(object)cmbItem).Text;
				dtPlannedOutput.Rows[updateDetailIndexoutput]["PackingTypeId"] = cmbPackingType.Value;
				dtPlannedOutput.Rows[updateDetailIndexoutput]["PackingTypeName"] = ((Control)(object)cmbPackingType).Text;
				dtPlannedOutput.Rows[updateDetailIndexoutput]["QtyInner"] = txtinnerqty.Text;
				dtPlannedOutput.Rows[updateDetailIndexoutput]["InnerPackSizeId"] = cmbinneruom.Value;
				dtPlannedOutput.Rows[updateDetailIndexoutput]["InnerPackSizeName"] = ((Control)(object)cmbinneruom).Text;
				dtPlannedOutput.Rows[updateDetailIndexoutput]["QtyOuter"] = txtOuterqty.Text;
				dtPlannedOutput.Rows[updateDetailIndexoutput]["OuterPackSizeId"] = cmbouteruom.Value;
				dtPlannedOutput.Rows[updateDetailIndexoutput]["OuterPackSizeName"] = ((Control)(object)cmbouteruom).Text;
				dtPlannedOutput.Rows[updateDetailIndexoutput]["Weight"] = txtweight.Text;
				dtPlannedOutput.Rows[updateDetailIndexoutput]["PackRemarks"] = txtremarks.Text;
				dtPlannedOutput.Rows[updateDetailIndexoutput]["CropYearId"] = cmbCropYearOutput.Value;
				dtPlannedOutput.Rows[updateDetailIndexoutput]["CropYear"] = ((Control)(object)cmbCropYearOutput).Text;
				dtPlannedOutput.Rows[updateDetailIndexoutput]["WareHouseId"] = cmbWareHouseOutPut.Value;
				dtPlannedOutput.Rows[updateDetailIndexoutput]["WareHouseName"] = ((Control)(object)cmbWareHouseOutPut).Text;
				dtPlannedOutput.Rows[updateDetailIndexoutput]["JobLotId"] = cmbJobLotOutput.Value;
				dtPlannedOutput.Rows[updateDetailIndexoutput]["JobLotDescription"] = ((Control)(object)cmbJobLotOutput).Text;
				grdPlannedOutput.DataSource = dtPlannedOutput;
				btnplus.Visible = true;
				btnUpdateDetail.Visible = false;
				btnCancelUpdateDetial.Visible = false;
				((Control)(object)cmbItem).Text = string.Empty;
				((Control)(object)cmbPackingType).Text = string.Empty;
				txtinnerqty.Text = string.Empty;
				((Control)(object)cmbinneruom).Text = string.Empty;
				txtOuterqty.Text = string.Empty;
				((Control)(object)cmbouteruom).Text = string.Empty;
				txtweight.Text = string.Empty;
				txtremarks.Text = string.Empty;
				cmbItem.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnadddetailinputgrd_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValidationDetailInPut())
			{
				dtInputDetail.Rows.Add(cmbItemInput.Value, ((Control)(object)cmbItemInput).Text.Trim(), cmbcropyearinput.Value, ((Control)(object)cmbcropyearinput).Text.Trim(), cmbjoblotinput.Value, ((Control)(object)cmbjoblotinput).Text.Trim(), txtqtyinput.Text.Trim(), cmbpacksizeinput.Value, ((Control)(object)cmbpacksizeinput).Text.Trim(), txtweightinput.Text.Trim(), cmbwarehouseinput.Value, ((Control)(object)cmbwarehouseinput).Text.Trim(), txtremarksinput.Text.Trim(), cmbPackTypeInput.Value, ((Control)(object)cmbPackTypeInput).Text.Trim());
				grdInputDetail.DataSource = dtInputDetail;
				grdInputDetail.RetrieveStructure();
				InputDetailGridSetting();
				((Control)(object)cmbItemInput).Text = string.Empty;
				((Control)(object)cmbcropyearinput).Text = string.Empty;
				((Control)(object)cmbjoblotinput).Text = string.Empty;
				((Control)(object)cmbpacksizeinput).Text = string.Empty;
				txtqtyinput.Text = string.Empty;
				txtweightinput.Text = string.Empty;
				((Control)(object)cmbwarehouseinput).Text = string.Empty;
				txtremarksinput.Text = string.Empty;
				cmbItemInput.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInputDetail_DoubleClick(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Expected O, but got Unknown
		try
		{
			if (InvProductionJobOrder.CheckJobOrderIdRefToInvFoodProduction(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = 80,
				JobOrderId = RECID
			}) > 0)
			{
				throw new Exception("Record Not Update Because Record has exist in Production InPut");
			}
			GridEXRow item = grdInputDetail.CurrentRow;
			if (item != null)
			{
				ItemDetailFill();
				updateDetailIndexInput = item.RowIndex;
				cmbItemInput.Value = item.Cells["ItemId"].Value.ToString();
				bindPackSizeInput(Conversion.ToInt(cmbItemInput.Value));
				cmbcropyearinput.Value = item.Cells["CropYearId"].Value.ToString();
				cmbjoblotinput.Value = item.Cells["JobLotId"].Value.ToString();
				txtqtyinput.Text = item.Cells["Qty"].Value.ToString();
				cmbpacksizeinput.Value = item.Cells["PackSizeId"].Value.ToString();
				txtweightinput.Text = item.Cells["Weight"].Value.ToString();
				cmbwarehouseinput.Value = item.Cells["WareHouseId"].Value.ToString();
				txtremarksinput.Text = item.Cells["Remarks"].Value.ToString();
				cmbPackTypeInput.Value = item.Cells["PackTypeId"].Value.ToString();
				btnadddetailinputgrd.Visible = false;
				btnupdategrdinput.Visible = true;
				btncancelgrdinput.Visible = true;
				((UltraGridBase)cmbentrytype).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnupdategrdinput_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValidationDetailInPut())
			{
				dtInputDetail.Rows[updateDetailIndexInput]["ItemId"] = cmbItemInput.Value;
				dtInputDetail.Rows[updateDetailIndexInput]["ItemName"] = ((Control)(object)cmbItemInput).Text;
				dtInputDetail.Rows[updateDetailIndexInput]["CropYearId"] = cmbcropyearinput.Value;
				dtInputDetail.Rows[updateDetailIndexInput]["CropYear"] = ((Control)(object)cmbcropyearinput).Text;
				dtInputDetail.Rows[updateDetailIndexInput]["JobLotId"] = cmbjoblotinput.Value;
				dtInputDetail.Rows[updateDetailIndexInput]["JobLot"] = ((Control)(object)cmbjoblotinput).Text;
				dtInputDetail.Rows[updateDetailIndexInput]["Qty"] = txtqtyinput.Text;
				dtInputDetail.Rows[updateDetailIndexInput]["PackSizeId"] = cmbpacksizeinput.Value;
				dtInputDetail.Rows[updateDetailIndexInput]["PackSize"] = ((Control)(object)cmbpacksizeinput).Text;
				dtInputDetail.Rows[updateDetailIndexInput]["Weight"] = txtweightinput.Text;
				dtInputDetail.Rows[updateDetailIndexInput]["WareHouseId"] = cmbwarehouseinput.Value;
				dtInputDetail.Rows[updateDetailIndexInput]["WareHouse"] = ((Control)(object)cmbwarehouseinput).Text;
				dtInputDetail.Rows[updateDetailIndexInput]["Remarks"] = txtremarksinput.Text;
				dtInputDetail.Rows[updateDetailIndexInput]["PackTypeId"] = cmbPackTypeInput.Value;
				dtInputDetail.Rows[updateDetailIndexInput]["PackType"] = ((Control)(object)cmbPackTypeInput).Text;
				grdInputDetail.DataSource = dtInputDetail;
				btnadddetailinputgrd.Visible = true;
				btnupdategrdinput.Visible = false;
				btncancelgrdinput.Visible = false;
				((Control)(object)cmbItemInput).Text = string.Empty;
				((Control)(object)cmbcropyearinput).Text = string.Empty;
				((Control)(object)cmbjoblotinput).Text = string.Empty;
				((Control)(object)cmbpacksizeinput).Text = string.Empty;
				txtqtyinput.Text = string.Empty;
				txtweightinput.Text = string.Empty;
				((Control)(object)cmbwarehouseinput).Text = string.Empty;
				txtremarksinput.Text = string.Empty;
				cmbItemInput.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btncancelgrdinput_Click(object sender, EventArgs e)
	{
		try
		{
			btnadddetailinputgrd.Visible = true;
			btnupdategrdinput.Visible = false;
			btncancelgrdinput.Visible = false;
			((Control)(object)cmbItemInput).Text = string.Empty;
			((Control)(object)cmbcropyearinput).Text = string.Empty;
			((Control)(object)cmbjoblotinput).Text = string.Empty;
			((Control)(object)cmbpacksizeinput).Text = string.Empty;
			txtqtyinput.Text = string.Empty;
			txtweightinput.Text = string.Empty;
			((Control)(object)cmbwarehouseinput).Text = string.Empty;
			txtremarksinput.Text = string.Empty;
			((Control)(object)cmbPackTypeInput).Text = string.Empty;
			cmbItemInput.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnProductionPlant_Click(object sender, EventArgs e)
	{
		try
		{
			new DefineProductionPlant(UserAccount).Show();
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
			ItemFill();
			JobLotBind();
			AccountFills();
			PlanTypeBind();
			PlantFeader();
			CropYear();
			WareHouseFill();
			ItemDetailFill();
			CmbPackingTypeFill();
			FilterTypeFill();
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
				((UltraGridBase)cmbentrytype).Rows[1].Activate();
			}
			else
			{
				((UltraGridBase)cmbentrytype).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnloadall_Click(object sender, EventArgs e)
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
		//IL_05b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ba: Expected O, but got Unknown
		//IL_05ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c1: Expected O, but got Unknown
		//IL_05c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c8: Expected O, but got Unknown
		//IL_05c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cf: Expected O, but got Unknown
		//IL_05cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d6: Expected O, but got Unknown
		//IL_05d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05dd: Expected O, but got Unknown
		//IL_05dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e4: Expected O, but got Unknown
		//IL_05e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05eb: Expected O, but got Unknown
		//IL_05eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f2: Expected O, but got Unknown
		//IL_05f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f9: Expected O, but got Unknown
		//IL_05f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0600: Expected O, but got Unknown
		//IL_0600: Unknown result type (might be due to invalid IL or missing references)
		//IL_0607: Expected O, but got Unknown
		//IL_0607: Unknown result type (might be due to invalid IL or missing references)
		//IL_060e: Expected O, but got Unknown
		//IL_060e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0615: Expected O, but got Unknown
		//IL_0615: Unknown result type (might be due to invalid IL or missing references)
		//IL_061c: Expected O, but got Unknown
		//IL_061c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0623: Expected O, but got Unknown
		//IL_0623: Unknown result type (might be due to invalid IL or missing references)
		//IL_062a: Expected O, but got Unknown
		//IL_062a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0631: Expected O, but got Unknown
		//IL_0631: Unknown result type (might be due to invalid IL or missing references)
		//IL_0638: Expected O, but got Unknown
		//IL_0638: Unknown result type (might be due to invalid IL or missing references)
		//IL_063f: Expected O, but got Unknown
		//IL_063f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0646: Expected O, but got Unknown
		//IL_0646: Unknown result type (might be due to invalid IL or missing references)
		//IL_064d: Expected O, but got Unknown
		//IL_064d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0654: Expected O, but got Unknown
		//IL_0654: Unknown result type (might be due to invalid IL or missing references)
		//IL_065b: Expected O, but got Unknown
		//IL_065b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0662: Expected O, but got Unknown
		//IL_0662: Unknown result type (might be due to invalid IL or missing references)
		//IL_0669: Expected O, but got Unknown
		//IL_0669: Unknown result type (might be due to invalid IL or missing references)
		//IL_0670: Expected O, but got Unknown
		//IL_0670: Unknown result type (might be due to invalid IL or missing references)
		//IL_0677: Expected O, but got Unknown
		//IL_0677: Unknown result type (might be due to invalid IL or missing references)
		//IL_067e: Expected O, but got Unknown
		//IL_067e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0685: Expected O, but got Unknown
		//IL_0685: Unknown result type (might be due to invalid IL or missing references)
		//IL_068c: Expected O, but got Unknown
		//IL_068c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0693: Expected O, but got Unknown
		//IL_0693: Unknown result type (might be due to invalid IL or missing references)
		//IL_069a: Expected O, but got Unknown
		//IL_069a: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a1: Expected O, but got Unknown
		//IL_06a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a8: Expected O, but got Unknown
		//IL_06a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06af: Expected O, but got Unknown
		//IL_06af: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b6: Expected O, but got Unknown
		//IL_06b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_06bd: Expected O, but got Unknown
		//IL_06bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c4: Expected O, but got Unknown
		//IL_06c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06cb: Expected O, but got Unknown
		//IL_06cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d2: Expected O, but got Unknown
		//IL_06d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d9: Expected O, but got Unknown
		//IL_06d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e0: Expected O, but got Unknown
		//IL_06e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e7: Expected O, but got Unknown
		//IL_06e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ee: Expected O, but got Unknown
		//IL_06ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f5: Expected O, but got Unknown
		//IL_06f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fe: Expected O, but got Unknown
		//IL_0700: Unknown result type (might be due to invalid IL or missing references)
		//IL_0709: Expected O, but got Unknown
		//IL_070b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0714: Expected O, but got Unknown
		//IL_0716: Unknown result type (might be due to invalid IL or missing references)
		//IL_071f: Expected O, but got Unknown
		//IL_0721: Unknown result type (might be due to invalid IL or missing references)
		//IL_072a: Expected O, but got Unknown
		//IL_072c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0735: Expected O, but got Unknown
		//IL_0737: Unknown result type (might be due to invalid IL or missing references)
		//IL_0740: Expected O, but got Unknown
		//IL_0742: Unknown result type (might be due to invalid IL or missing references)
		//IL_074b: Expected O, but got Unknown
		//IL_074d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0756: Expected O, but got Unknown
		//IL_0758: Unknown result type (might be due to invalid IL or missing references)
		//IL_0761: Expected O, but got Unknown
		//IL_0763: Unknown result type (might be due to invalid IL or missing references)
		//IL_076c: Expected O, but got Unknown
		//IL_076e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0777: Expected O, but got Unknown
		//IL_0779: Unknown result type (might be due to invalid IL or missing references)
		//IL_0782: Expected O, but got Unknown
		//IL_0784: Unknown result type (might be due to invalid IL or missing references)
		//IL_078d: Expected O, but got Unknown
		//IL_078f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0798: Expected O, but got Unknown
		//IL_079a: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a3: Expected O, but got Unknown
		//IL_07a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ae: Expected O, but got Unknown
		//IL_07b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b9: Expected O, but got Unknown
		//IL_07bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c4: Expected O, but got Unknown
		//IL_07c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_07cf: Expected O, but got Unknown
		//IL_07d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07da: Expected O, but got Unknown
		//IL_07dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e5: Expected O, but got Unknown
		//IL_07e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f0: Expected O, but got Unknown
		//IL_07f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_07fb: Expected O, but got Unknown
		//IL_07fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0806: Expected O, but got Unknown
		//IL_0808: Unknown result type (might be due to invalid IL or missing references)
		//IL_0811: Expected O, but got Unknown
		//IL_0813: Unknown result type (might be due to invalid IL or missing references)
		//IL_081c: Expected O, but got Unknown
		//IL_081e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0827: Expected O, but got Unknown
		//IL_0829: Unknown result type (might be due to invalid IL or missing references)
		//IL_0832: Expected O, but got Unknown
		//IL_0834: Unknown result type (might be due to invalid IL or missing references)
		//IL_083d: Expected O, but got Unknown
		//IL_083f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0848: Expected O, but got Unknown
		//IL_084a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0853: Expected O, but got Unknown
		//IL_0855: Unknown result type (might be due to invalid IL or missing references)
		//IL_085e: Expected O, but got Unknown
		//IL_0860: Unknown result type (might be due to invalid IL or missing references)
		//IL_0869: Expected O, but got Unknown
		//IL_086b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0874: Expected O, but got Unknown
		//IL_0876: Unknown result type (might be due to invalid IL or missing references)
		//IL_087f: Expected O, but got Unknown
		//IL_0881: Unknown result type (might be due to invalid IL or missing references)
		//IL_088a: Expected O, but got Unknown
		//IL_088c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0895: Expected O, but got Unknown
		//IL_0897: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a0: Expected O, but got Unknown
		//IL_08a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ab: Expected O, but got Unknown
		//IL_08ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b6: Expected O, but got Unknown
		//IL_08b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c1: Expected O, but got Unknown
		//IL_08c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_08cc: Expected O, but got Unknown
		//IL_08ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d7: Expected O, but got Unknown
		//IL_08d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e2: Expected O, but got Unknown
		//IL_08e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ed: Expected O, but got Unknown
		//IL_08ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f8: Expected O, but got Unknown
		//IL_08fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0903: Expected O, but got Unknown
		//IL_0905: Unknown result type (might be due to invalid IL or missing references)
		//IL_090e: Expected O, but got Unknown
		//IL_0910: Unknown result type (might be due to invalid IL or missing references)
		//IL_0919: Expected O, but got Unknown
		//IL_091b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0924: Expected O, but got Unknown
		//IL_0926: Unknown result type (might be due to invalid IL or missing references)
		//IL_092f: Expected O, but got Unknown
		//IL_0931: Unknown result type (might be due to invalid IL or missing references)
		//IL_093a: Expected O, but got Unknown
		//IL_093c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0945: Expected O, but got Unknown
		//IL_0947: Unknown result type (might be due to invalid IL or missing references)
		//IL_0950: Expected O, but got Unknown
		//IL_0952: Unknown result type (might be due to invalid IL or missing references)
		//IL_095b: Expected O, but got Unknown
		//IL_095d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0966: Expected O, but got Unknown
		//IL_0968: Unknown result type (might be due to invalid IL or missing references)
		//IL_0971: Expected O, but got Unknown
		//IL_0973: Unknown result type (might be due to invalid IL or missing references)
		//IL_097c: Expected O, but got Unknown
		//IL_0993: Unknown result type (might be due to invalid IL or missing references)
		//IL_099c: Expected O, but got Unknown
		//IL_099e: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a7: Expected O, but got Unknown
		//IL_09a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b2: Expected O, but got Unknown
		//IL_0a23: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a2d: Expected O, but got Unknown
		//IL_0a39: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a43: Expected O, but got Unknown
		//IL_0a5a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a64: Expected O, but got Unknown
		//IL_0a70: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a7a: Expected O, but got Unknown
		//IL_0a7b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a85: Expected O, but got Unknown
		//IL_0a9c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa6: Expected O, but got Unknown
		//IL_0ab2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0abc: Expected O, but got Unknown
		//IL_0ac8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad2: Expected O, but got Unknown
		//IL_0b6d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b77: Expected O, but got Unknown
		//IL_0b8e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b98: Expected O, but got Unknown
		//IL_0baf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bb9: Expected O, but got Unknown
		//IL_0bf1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bfb: Expected O, but got Unknown
		//IL_0c12: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c1c: Expected O, but got Unknown
		//IL_0c33: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c3d: Expected O, but got Unknown
		//IL_0c5f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c69: Expected O, but got Unknown
		//IL_0c8b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c95: Expected O, but got Unknown
		//IL_0c96: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ca0: Expected O, but got Unknown
		//IL_0cac: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cb6: Expected O, but got Unknown
		//IL_0cc2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ccc: Expected O, but got Unknown
		//IL_0cd8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ce2: Expected O, but got Unknown
		//IL_0cee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cf8: Expected O, but got Unknown
		//IL_0d04: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d0e: Expected O, but got Unknown
		//IL_0d1a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d24: Expected O, but got Unknown
		//IL_0d51: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d5b: Expected O, but got Unknown
		//IL_0d5c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d66: Expected O, but got Unknown
		//IL_0d67: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d71: Expected O, but got Unknown
		//IL_0df6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e00: Expected O, but got Unknown
		//IL_0e01: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e0b: Expected O, but got Unknown
		//IL_0f1f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f29: Expected O, but got Unknown
		//IL_0f35: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f3f: Expected O, but got Unknown
		//IL_0f4b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f55: Expected O, but got Unknown
		//IL_bf65: Unknown result type (might be due to invalid IL or missing references)
		//IL_bf6f: Expected O, but got Unknown
		//IL_bf7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_bf86: Expected O, but got Unknown
		GridEXLayout grdPlannedOutput_DesignTimeLayout = new GridEXLayout();
		Appearance appearance46 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance168 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance190 = new Appearance();
		Appearance appearance201 = new Appearance();
		Appearance appearance212 = new Appearance();
		Appearance appearance224 = new Appearance();
		Appearance appearance235 = new Appearance();
		Appearance appearance239 = new Appearance();
		Appearance appearance240 = new Appearance();
		Appearance appearance281 = new Appearance();
		Appearance appearance282 = new Appearance();
		Appearance appearance283 = new Appearance();
		Appearance appearance297 = new Appearance();
		Appearance appearance298 = new Appearance();
		Appearance appearance299 = new Appearance();
		Appearance appearance300 = new Appearance();
		Appearance appearance301 = new Appearance();
		Appearance appearance303 = new Appearance();
		Appearance appearance304 = new Appearance();
		Appearance appearance305 = new Appearance();
		Appearance appearance306 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance245 = new Appearance();
		Appearance appearance246 = new Appearance();
		Appearance appearance248 = new Appearance();
		Appearance appearance272 = new Appearance();
		Appearance appearance273 = new Appearance();
		Appearance appearance274 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
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
		Appearance appearance136 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance226 = new Appearance();
		Appearance appearance227 = new Appearance();
		Appearance appearance228 = new Appearance();
		Appearance appearance229 = new Appearance();
		Appearance appearance230 = new Appearance();
		Appearance appearance231 = new Appearance();
		Appearance appearance232 = new Appearance();
		Appearance appearance233 = new Appearance();
		Appearance appearance234 = new Appearance();
		Appearance appearance236 = new Appearance();
		Appearance appearance237 = new Appearance();
		Appearance appearance238 = new Appearance();
		GridEXLayout grdInputDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance78 = new Appearance();
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
		Appearance appearance185 = new Appearance();
		Appearance appearance186 = new Appearance();
		Appearance appearance187 = new Appearance();
		Appearance appearance188 = new Appearance();
		Appearance appearance189 = new Appearance();
		Appearance appearance191 = new Appearance();
		Appearance appearance192 = new Appearance();
		Appearance appearance193 = new Appearance();
		Appearance appearance194 = new Appearance();
		Appearance appearance195 = new Appearance();
		Appearance appearance196 = new Appearance();
		Appearance appearance197 = new Appearance();
		Appearance appearance198 = new Appearance();
		Appearance appearance199 = new Appearance();
		Appearance appearance200 = new Appearance();
		Appearance appearance202 = new Appearance();
		Appearance appearance203 = new Appearance();
		Appearance appearance204 = new Appearance();
		Appearance appearance205 = new Appearance();
		Appearance appearance206 = new Appearance();
		Appearance appearance207 = new Appearance();
		Appearance appearance208 = new Appearance();
		Appearance appearance209 = new Appearance();
		Appearance appearance210 = new Appearance();
		Appearance appearance211 = new Appearance();
		Appearance appearance213 = new Appearance();
		Appearance appearance214 = new Appearance();
		Appearance appearance215 = new Appearance();
		Appearance appearance216 = new Appearance();
		Appearance appearance217 = new Appearance();
		Appearance appearance218 = new Appearance();
		Appearance appearance219 = new Appearance();
		Appearance appearance220 = new Appearance();
		Appearance appearance221 = new Appearance();
		Appearance appearance222 = new Appearance();
		Appearance appearance225 = new Appearance();
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
		Appearance appearance307 = new Appearance();
		Appearance appearance308 = new Appearance();
		Appearance appearance309 = new Appearance();
		Appearance appearance310 = new Appearance();
		Appearance appearance311 = new Appearance();
		Appearance appearance312 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
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
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance241 = new Appearance();
		Appearance appearance242 = new Appearance();
		Appearance appearance243 = new Appearance();
		Appearance appearance284 = new Appearance();
		Appearance appearance285 = new Appearance();
		Appearance appearance286 = new Appearance();
		Appearance appearance287 = new Appearance();
		Appearance appearance288 = new Appearance();
		Appearance appearance289 = new Appearance();
		Appearance appearance290 = new Appearance();
		Appearance appearance292 = new Appearance();
		Appearance appearance293 = new Appearance();
		Appearance appearance294 = new Appearance();
		Appearance appearance295 = new Appearance();
		Appearance appearance296 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance271 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance275 = new Appearance();
		Appearance appearance276 = new Appearance();
		Appearance appearance277 = new Appearance();
		Appearance appearance278 = new Appearance();
		Appearance appearance279 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance223 = new Appearance();
		Appearance appearance247 = new Appearance();
		Appearance appearance258 = new Appearance();
		Appearance appearance269 = new Appearance();
		Appearance appearance280 = new Appearance();
		Appearance appearance291 = new Appearance();
		Appearance appearance302 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance257 = new Appearance();
		Appearance appearance259 = new Appearance();
		Appearance appearance260 = new Appearance();
		Appearance appearance261 = new Appearance();
		Appearance appearance262 = new Appearance();
		Appearance appearance263 = new Appearance();
		Appearance appearance264 = new Appearance();
		Appearance appearance265 = new Appearance();
		Appearance appearance266 = new Appearance();
		Appearance appearance267 = new Appearance();
		Appearance appearance268 = new Appearance();
		Appearance appearance270 = new Appearance();
		Appearance appearance244 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance249 = new Appearance();
		Appearance appearance250 = new Appearance();
		Appearance appearance251 = new Appearance();
		Appearance appearance252 = new Appearance();
		Appearance appearance253 = new Appearance();
		Appearance appearance254 = new Appearance();
		Appearance appearance255 = new Appearance();
		Appearance appearance256 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance169 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance171 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance157 = new Appearance();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Production.frmProductionJobOrder));
		GridEXLayout grdOutPut_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdInput_DesignTimeLayout = new GridEXLayout();
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		this.miniToolStrip = new System.Windows.Forms.ToolStrip();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.panel14 = new System.Windows.Forms.Panel();
		this.panel9 = new System.Windows.Forms.Panel();
		this.grdPlannedOutput = new GridEX();
		this.paneloutputgrd = new System.Windows.Forms.Panel();
		this.cmbWareHouseOutPut = new UltraCombo();
		this.label12 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.cmbCropYearOutput = new UltraCombo();
		this.label41 = new System.Windows.Forms.Label();
		this.cmbJobLotOutput = new UltraCombo();
		this.cmbItem = new UltraCombo();
		this.label37 = new System.Windows.Forms.Label();
		this.label24 = new System.Windows.Forms.Label();
		this.cmbinneruom = new UltraCombo();
		this.label25 = new System.Windows.Forms.Label();
		this.cmbouteruom = new UltraCombo();
		this.label35 = new System.Windows.Forms.Label();
		this.cmbPackingType = new UltraCombo();
		this.label38 = new System.Windows.Forms.Label();
		this.txtinnerqty = new System.Windows.Forms.TextBox();
		this.label26 = new System.Windows.Forms.Label();
		this.txtOuterqty = new System.Windows.Forms.TextBox();
		this.label39 = new System.Windows.Forms.Label();
		this.txtweight = new System.Windows.Forms.TextBox();
		this.label27 = new System.Windows.Forms.Label();
		this.txtremarks = new System.Windows.Forms.TextBox();
		this.btnplus = new System.Windows.Forms.Button();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.btnCancelUpdateDetial = new System.Windows.Forms.Button();
		this.tabPage4 = new System.Windows.Forms.TabPage();
		this.panel15 = new System.Windows.Forms.Panel();
		this.panel7 = new System.Windows.Forms.Panel();
		this.grdInputDetail = new GridEX();
		this.panelinputgrd = new System.Windows.Forms.Panel();
		this.label42 = new System.Windows.Forms.Label();
		this.cmbPackTypeInput = new UltraCombo();
		this.txtweightinput = new System.Windows.Forms.TextBox();
		this.label29 = new System.Windows.Forms.Label();
		this.cmbItemInput = new UltraCombo();
		this.btnadddetailinputgrd = new System.Windows.Forms.Button();
		this.txtremarksinput = new System.Windows.Forms.TextBox();
		this.btnupdategrdinput = new System.Windows.Forms.Button();
		this.label36 = new System.Windows.Forms.Label();
		this.btncancelgrdinput = new System.Windows.Forms.Button();
		this.cmbwarehouseinput = new UltraCombo();
		this.label30 = new System.Windows.Forms.Label();
		this.label34 = new System.Windows.Forms.Label();
		this.cmbcropyearinput = new UltraCombo();
		this.label45 = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.cmbjoblotinput = new UltraCombo();
		this.txtqtyinput = new System.Windows.Forms.TextBox();
		this.label32 = new System.Windows.Forms.Label();
		this.label31 = new System.Windows.Forms.Label();
		this.cmbpacksizeinput = new UltraCombo();
		this.panel4 = new System.Windows.Forms.Panel();
		this.btnProductionPlant = new System.Windows.Forms.Button();
		this.label40 = new System.Windows.Forms.Label();
		this.cmbentrytype = new UltraCombo();
		this.cmbDocNo = new UltraCombo();
		this.label28 = new System.Windows.Forms.Label();
		this.cmbFiltersType = new UltraCombo();
		this.cmbfiltertype = new System.Windows.Forms.Label();
		this.CmbWipItem = new UltraCombo();
		this.label23 = new System.Windows.Forms.Label();
		this.CmbLotAc = new UltraCombo();
		this.label22 = new System.Windows.Forms.Label();
		this.CmbByProductionAc = new UltraCombo();
		this.label21 = new System.Windows.Forms.Label();
		this.CmbFinishGoodsAc = new UltraCombo();
		this.label20 = new System.Windows.Forms.Label();
		this.CmbWorkInProcess = new UltraCombo();
		this.label15 = new System.Windows.Forms.Label();
		this.chkPrint = new System.Windows.Forms.CheckBox();
		this.label19 = new System.Windows.Forms.Label();
		this.txtPlanTypeSrNo = new System.Windows.Forms.TextBox();
		this.cmbStatus = new UltraCombo();
		this.cmbProductionType = new UltraCombo();
		this.cmbPlantFeader = new UltraCombo();
		this.txtEndDate = new System.Windows.Forms.DateTimePicker();
		this.label18 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.txtDeliveryInstruction = new System.Windows.Forms.TextBox();
		this.txtstartDate = new System.Windows.Forms.DateTimePicker();
		this.label16 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.txtProductionInstruction = new System.Windows.Forms.TextBox();
		this.label11 = new System.Windows.Forms.Label();
		this.txtOtherInstruction = new System.Windows.Forms.TextBox();
		this.label10 = new System.Windows.Forms.Label();
		this.cmbPlanType = new UltraCombo();
		this.cmbSaleOrder = new UltraCombo();
		this.label9 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.txtPlantInstruction = new System.Windows.Forms.TextBox();
		this.label3 = new System.Windows.Forms.Label();
		this.txtOtherReference = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.txtInvoiceReference = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.txtPlanCode = new System.Windows.Forms.TextBox();
		this.txtPlanDate = new System.Windows.Forms.DateTimePicker();
		this.label5 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.Print = new System.Windows.Forms.ToolStripButton();
		this.btnattachment = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel5 = new System.Windows.Forms.Panel();
		this.panel13 = new System.Windows.Forms.Panel();
		this.grdOutPut = new GridEX();
		this.panel11 = new System.Windows.Forms.Panel();
		this.grdInput = new GridEX();
		this.panel12 = new System.Windows.Forms.Panel();
		this.DataGridHistory = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.btnloadall = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel6.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.tabPage3.SuspendLayout();
		this.panel14.SuspendLayout();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPlannedOutput).BeginInit();
		this.paneloutputgrd.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbWareHouseOutPut).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCropYearOutput).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbJobLotOutput).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbinneruom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbouteruom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPackingType).BeginInit();
		this.tabPage4.SuspendLayout();
		this.panel15.SuspendLayout();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdInputDetail).BeginInit();
		this.panelinputgrd.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbPackTypeInput).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemInput).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbwarehouseinput).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcropyearinput).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbjoblotinput).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbpacksizeinput).BeginInit();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbentrytype).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDocNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbFiltersType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWipItem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbLotAc).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbByProductionAc).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbFinishGoodsAc).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWorkInProcess).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbStatus).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbProductionType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPlantFeader).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPlanType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSaleOrder).BeginInit();
		this.panel3.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel5.SuspendLayout();
		this.panel13.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdOutPut).BeginInit();
		this.panel11.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdInput).BeginInit();
		this.panel12.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel8.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.miniToolStrip.AccessibleName = "New item selection";
		this.miniToolStrip.AccessibleRole = System.Windows.Forms.AccessibleRole.ButtonDropDown;
		this.miniToolStrip.AutoSize = false;
		this.miniToolStrip.CanOverflow = false;
		this.miniToolStrip.Dock = System.Windows.Forms.DockStyle.None;
		this.miniToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
		this.miniToolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.miniToolStrip.Location = new System.Drawing.Point(250, 4);
		this.miniToolStrip.Name = "miniToolStrip";
		this.miniToolStrip.Size = new System.Drawing.Size(1363, 27);
		this.miniToolStrip.TabIndex = 3;
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1248, 749);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1248, 749);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1240, 723);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add(this.panel6);
		this.panel2.Controls.Add(this.panel4);
		this.panel2.Controls.Add(this.panel3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1234, 717);
		this.panel2.TabIndex = 0;
		this.panel6.BackColor = System.Drawing.Color.White;
		this.panel6.Controls.Add(this.tabControl2);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(0, 314);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1234, 403);
		this.panel6.TabIndex = 61;
		this.tabControl2.Appearance = System.Windows.Forms.TabAppearance.Buttons;
		this.tabControl2.Controls.Add(this.tabPage3);
		this.tabControl2.Controls.Add(this.tabPage4);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl2.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl2.Location = new System.Drawing.Point(0, 0);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.ShowToolTips = true;
		this.tabControl2.Size = new System.Drawing.Size(1234, 403);
		this.tabControl2.TabIndex = 0;
		this.tabControl2.SelectedIndexChanged += new System.EventHandler(tabControl2_SelectedIndexChanged);
		this.tabPage3.Controls.Add(this.panel14);
		this.tabPage3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabPage3.Location = new System.Drawing.Point(4, 27);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage3.Size = new System.Drawing.Size(1226, 372);
		this.tabPage3.TabIndex = 0;
		this.tabPage3.Text = "Planned Output Requirnment";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.panel14.Controls.Add(this.panel9);
		this.panel14.Controls.Add(this.paneloutputgrd);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel14.Location = new System.Drawing.Point(3, 3);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(1220, 366);
		this.panel14.TabIndex = 0;
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.grdPlannedOutput);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel9.Location = new System.Drawing.Point(0, 108);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1220, 258);
		this.panel9.TabIndex = 206;
		((ScrollableBase)this.grdPlannedOutput).BackColor = System.Drawing.Color.White;
		this.grdPlannedOutput.ColumnAutoResize = true;
		grdPlannedOutput_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPlannedOutput.DesignTimeLayout = grdPlannedOutput_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPlannedOutput).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdPlannedOutput).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPlannedOutput.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdPlannedOutput).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdPlannedOutput).Name = "grdPlannedOutput";
		((ScrollableBase)this.grdPlannedOutput).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPlannedOutput).Size = new System.Drawing.Size(1220, 258);
		((System.Windows.Forms.Control)(object)this.grdPlannedOutput).TabIndex = 0;
		this.grdPlannedOutput.TotalRow = (InheritableBoolean)1;
		this.grdPlannedOutput.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPlannedOutput).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdPlannedOutput).DoubleClick += new System.EventHandler(grdPlannedOutput_DoubleClick);
		this.paneloutputgrd.BackColor = System.Drawing.Color.White;
		this.paneloutputgrd.Controls.Add((System.Windows.Forms.Control)(object)this.cmbWareHouseOutPut);
		this.paneloutputgrd.Controls.Add(this.label12);
		this.paneloutputgrd.Controls.Add(this.label14);
		this.paneloutputgrd.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCropYearOutput);
		this.paneloutputgrd.Controls.Add(this.label41);
		this.paneloutputgrd.Controls.Add((System.Windows.Forms.Control)(object)this.cmbJobLotOutput);
		this.paneloutputgrd.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItem);
		this.paneloutputgrd.Controls.Add(this.label37);
		this.paneloutputgrd.Controls.Add(this.label24);
		this.paneloutputgrd.Controls.Add((System.Windows.Forms.Control)(object)this.cmbinneruom);
		this.paneloutputgrd.Controls.Add(this.label25);
		this.paneloutputgrd.Controls.Add((System.Windows.Forms.Control)(object)this.cmbouteruom);
		this.paneloutputgrd.Controls.Add(this.label35);
		this.paneloutputgrd.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPackingType);
		this.paneloutputgrd.Controls.Add(this.label38);
		this.paneloutputgrd.Controls.Add(this.txtinnerqty);
		this.paneloutputgrd.Controls.Add(this.label26);
		this.paneloutputgrd.Controls.Add(this.txtOuterqty);
		this.paneloutputgrd.Controls.Add(this.label39);
		this.paneloutputgrd.Controls.Add(this.txtweight);
		this.paneloutputgrd.Controls.Add(this.label27);
		this.paneloutputgrd.Controls.Add(this.txtremarks);
		this.paneloutputgrd.Controls.Add(this.btnplus);
		this.paneloutputgrd.Controls.Add(this.btnUpdateDetail);
		this.paneloutputgrd.Controls.Add(this.btnCancelUpdateDetial);
		this.paneloutputgrd.Dock = System.Windows.Forms.DockStyle.Top;
		this.paneloutputgrd.Location = new System.Drawing.Point(0, 0);
		this.paneloutputgrd.Name = "paneloutputgrd";
		this.paneloutputgrd.Size = new System.Drawing.Size(1220, 108);
		this.paneloutputgrd.TabIndex = 0;
		this.cmbWareHouseOutPut.AllowNull = (DefaultableBoolean)1;
		this.cmbWareHouseOutPut.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbWareHouseOutPut.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbWareHouseOutPut.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbWareHouseOutPut).DisplayLayout.Appearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.cmbWareHouseOutPut).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbWareHouseOutPut).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance57).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance57).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance57).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbWareHouseOutPut).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance57;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbWareHouseOutPut).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance68;
		((SpecialBoxBase)((UltraGridBase)this.cmbWareHouseOutPut).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance168).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance168).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance168).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbWareHouseOutPut).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.cmbWareHouseOutPut).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbWareHouseOutPut).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance179).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbWareHouseOutPut).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance179;
		((AppearanceBase)appearance190).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance190).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbWareHouseOutPut).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance190;
		((UltraGridBase)this.cmbWareHouseOutPut).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbWareHouseOutPut).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance201).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbWareHouseOutPut).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance201;
		((AppearanceBase)appearance212).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance212).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbWareHouseOutPut).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance212;
		((UltraGridBase)this.cmbWareHouseOutPut).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbWareHouseOutPut).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance224).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance224).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance224).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance224).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance224).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbWareHouseOutPut).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance224;
		((AppearanceBase)appearance235).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbWareHouseOutPut).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance235;
		((UltraGridBase)this.cmbWareHouseOutPut).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbWareHouseOutPut).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance239).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance239).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbWareHouseOutPut).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance239;
		((UltraGridBase)this.cmbWareHouseOutPut).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance240).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbWareHouseOutPut).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance240;
		((UltraGridBase)this.cmbWareHouseOutPut).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbWareHouseOutPut).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbWareHouseOutPut).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbWareHouseOutPut).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbWareHouseOutPut.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbWareHouseOutPut).Location = new System.Drawing.Point(7, 71);
		((System.Windows.Forms.Control)(object)this.cmbWareHouseOutPut).Name = "cmbWareHouseOutPut";
		((System.Windows.Forms.Control)(object)this.cmbWareHouseOutPut).Size = new System.Drawing.Size(198, 26);
		((System.Windows.Forms.Control)(object)this.cmbWareHouseOutPut).TabIndex = 8;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(961, 1);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(66, 17);
		this.label12.TabIndex = 204;
		this.label12.Text = "Crop Year";
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(7, 52);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(68, 15);
		this.label14.TabIndex = 206;
		this.label14.Text = "WareHouse";
		this.cmbCropYearOutput.AllowNull = (DefaultableBoolean)1;
		this.cmbCropYearOutput.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCropYearOutput.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCropYearOutput.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance281).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance281).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCropYearOutput).DisplayLayout.Appearance = (AppearanceBase)(object)appearance281;
		((UltraGridBase)this.cmbCropYearOutput).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCropYearOutput).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance282).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance282).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance282).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance282).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCropYearOutput).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance282;
		((AppearanceBase)appearance283).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCropYearOutput).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance283;
		((SpecialBoxBase)((UltraGridBase)this.cmbCropYearOutput).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance297).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance297).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance297).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance297).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCropYearOutput).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance297;
		((UltraGridBase)this.cmbCropYearOutput).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCropYearOutput).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance298).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance298).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCropYearOutput).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance298;
		((AppearanceBase)appearance299).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance299).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCropYearOutput).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance299;
		((UltraGridBase)this.cmbCropYearOutput).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCropYearOutput).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance300).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCropYearOutput).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance300;
		((AppearanceBase)appearance301).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance301).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCropYearOutput).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance301;
		((UltraGridBase)this.cmbCropYearOutput).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCropYearOutput).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance303).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance303).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance303).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance303).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance303).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCropYearOutput).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance303;
		((AppearanceBase)appearance304).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCropYearOutput).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance304;
		((UltraGridBase)this.cmbCropYearOutput).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCropYearOutput).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance305).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance305).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCropYearOutput).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance305;
		((UltraGridBase)this.cmbCropYearOutput).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance306).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCropYearOutput).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance306;
		((UltraGridBase)this.cmbCropYearOutput).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCropYearOutput).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCropYearOutput).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCropYearOutput).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbCropYearOutput.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCropYearOutput).Location = new System.Drawing.Point(961, 22);
		((System.Windows.Forms.Control)(object)this.cmbCropYearOutput).Name = "cmbCropYearOutput";
		((System.Windows.Forms.Control)(object)this.cmbCropYearOutput).Size = new System.Drawing.Size(115, 26);
		((System.Windows.Forms.Control)(object)this.cmbCropYearOutput).TabIndex = 6;
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label41.Location = new System.Drawing.Point(1079, 1);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(52, 17);
		this.label41.TabIndex = 205;
		this.label41.Text = "Job/Lot";
		this.cmbJobLotOutput.AllowNull = (DefaultableBoolean)1;
		this.cmbJobLotOutput.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbJobLotOutput.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbJobLotOutput.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbJobLotOutput).DisplayLayout.Appearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.cmbJobLotOutput).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbJobLotOutput).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance37).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance37).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbJobLotOutput).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance37;
		((AppearanceBase)appearance38).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbJobLotOutput).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance38;
		((SpecialBoxBase)((UltraGridBase)this.cmbJobLotOutput).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance245).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance245).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance245).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance245).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbJobLotOutput).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance245;
		((UltraGridBase)this.cmbJobLotOutput).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbJobLotOutput).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance246).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance246).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbJobLotOutput).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance246;
		((AppearanceBase)appearance248).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance248).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbJobLotOutput).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance248;
		((UltraGridBase)this.cmbJobLotOutput).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbJobLotOutput).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance272).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbJobLotOutput).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance272;
		((AppearanceBase)appearance273).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance273).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbJobLotOutput).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance273;
		((UltraGridBase)this.cmbJobLotOutput).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbJobLotOutput).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance274).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance274).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance274).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance274).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance274).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbJobLotOutput).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance274;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbJobLotOutput).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.cmbJobLotOutput).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbJobLotOutput).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbJobLotOutput).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbJobLotOutput).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbJobLotOutput).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.cmbJobLotOutput).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbJobLotOutput).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbJobLotOutput).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbJobLotOutput).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbJobLotOutput.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbJobLotOutput).Location = new System.Drawing.Point(1079, 22);
		((System.Windows.Forms.Control)(object)this.cmbJobLotOutput).Name = "cmbJobLotOutput";
		((System.Windows.Forms.Control)(object)this.cmbJobLotOutput).Size = new System.Drawing.Size(100, 26);
		((System.Windows.Forms.Control)(object)this.cmbJobLotOutput).TabIndex = 7;
		this.cmbItem.AllowNull = (DefaultableBoolean)1;
		this.cmbItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.cmbItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance81).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance81).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance81;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance82;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance108).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance108).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance108).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance109).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance109;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance110).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance111;
		((AppearanceBase)appearance114).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance114).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance115).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance115).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance115).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance116).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItem).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItem).Location = new System.Drawing.Point(7, 22);
		((System.Windows.Forms.Control)(object)this.cmbItem).Name = "cmbItem";
		((System.Windows.Forms.Control)(object)this.cmbItem).Size = new System.Drawing.Size(412, 26);
		((System.Windows.Forms.Control)(object)this.cmbItem).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbItem).Leave += new System.EventHandler(cmbItem_Leave);
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(7, 2);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(72, 17);
		this.label37.TabIndex = 178;
		this.label37.Text = "Item Name";
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(634, 2);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(72, 17);
		this.label24.TabIndex = 180;
		this.label24.Text = "Inner UOM";
		this.cmbinneruom.AllowNull = (DefaultableBoolean)1;
		this.cmbinneruom.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbinneruom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbinneruom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbinneruom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.cmbinneruom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbinneruom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbinneruom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbinneruom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance67;
		((SpecialBoxBase)((UltraGridBase)this.cmbinneruom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbinneruom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.cmbinneruom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbinneruom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbinneruom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbinneruom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.cmbinneruom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbinneruom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbinneruom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance73).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbinneruom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.cmbinneruom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbinneruom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance74).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbinneruom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbinneruom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.cmbinneruom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbinneruom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbinneruom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.cmbinneruom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbinneruom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.cmbinneruom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbinneruom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbinneruom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbinneruom).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbinneruom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbinneruom).Location = new System.Drawing.Point(634, 22);
		((System.Windows.Forms.Control)(object)this.cmbinneruom).Name = "cmbinneruom";
		((System.Windows.Forms.Control)(object)this.cmbinneruom).Size = new System.Drawing.Size(118, 26);
		((System.Windows.Forms.Control)(object)this.cmbinneruom).TabIndex = 3;
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(840, 2);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(76, 17);
		this.label25.TabIndex = 180;
		this.label25.Text = "Outer UOM";
		this.cmbouteruom.AllowNull = (DefaultableBoolean)1;
		this.cmbouteruom.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbouteruom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbouteruom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance136).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbouteruom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.cmbouteruom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbouteruom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance137).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance137).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance137).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbouteruom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance137;
		((AppearanceBase)appearance138).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbouteruom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance138;
		((SpecialBoxBase)((UltraGridBase)this.cmbouteruom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance139).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance139).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance139).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbouteruom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance139;
		((UltraGridBase)this.cmbouteruom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbouteruom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance140).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbouteruom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance140;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance141).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbouteruom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance141;
		((UltraGridBase)this.cmbouteruom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbouteruom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbouteruom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance142;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance143).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbouteruom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.cmbouteruom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbouteruom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance144).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance144).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance144).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance144).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbouteruom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance144;
		((AppearanceBase)appearance145).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbouteruom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.cmbouteruom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbouteruom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance147).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbouteruom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance147;
		((UltraGridBase)this.cmbouteruom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbouteruom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance148;
		((UltraGridBase)this.cmbouteruom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbouteruom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbouteruom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbouteruom).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbouteruom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbouteruom).Location = new System.Drawing.Point(840, 22);
		((System.Windows.Forms.Control)(object)this.cmbouteruom).Name = "cmbouteruom";
		((System.Windows.Forms.Control)(object)this.cmbouteruom).Size = new System.Drawing.Size(118, 26);
		((System.Windows.Forms.Control)(object)this.cmbouteruom).TabIndex = 5;
		this.cmbouteruom.ValueChanged += new System.EventHandler(cmbouteruom_ValueChanged);
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(422, 3);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(73, 15);
		this.label35.TabIndex = 184;
		this.label35.Text = "PackingType";
		this.cmbPackingType.AllowNull = (DefaultableBoolean)1;
		this.cmbPackingType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPackingType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPackingType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance226).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance226).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance226;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance227).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance227).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance227).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance227).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPackingType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance227;
		((AppearanceBase)appearance228).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance228;
		((SpecialBoxBase)((UltraGridBase)this.cmbPackingType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance229).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance229).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance229).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance229).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance229;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance230).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance230).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance230;
		((AppearanceBase)appearance231).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance231).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance231;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance232).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance232;
		((AppearanceBase)appearance233).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance233).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance233;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance234).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance234).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance234).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance234).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance234).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance234;
		((AppearanceBase)appearance236).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance236;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance237).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance237).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance237;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance238).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance238;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPackingType).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbPackingType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPackingType).Location = new System.Drawing.Point(422, 22);
		((System.Windows.Forms.Control)(object)this.cmbPackingType).Name = "cmbPackingType";
		((System.Windows.Forms.Control)(object)this.cmbPackingType).Size = new System.Drawing.Size(124, 26);
		((System.Windows.Forms.Control)(object)this.cmbPackingType).TabIndex = 1;
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.Location = new System.Drawing.Point(549, 3);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(56, 15);
		this.label38.TabIndex = 186;
		this.label38.Text = "Inner Qty";
		this.txtinnerqty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtinnerqty.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtinnerqty.Location = new System.Drawing.Point(549, 22);
		this.txtinnerqty.Name = "txtinnerqty";
		this.txtinnerqty.Size = new System.Drawing.Size(82, 25);
		this.txtinnerqty.TabIndex = 2;
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(755, 3);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(59, 15);
		this.label26.TabIndex = 186;
		this.label26.Text = "Outer Qty";
		this.txtOuterqty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOuterqty.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOuterqty.Location = new System.Drawing.Point(755, 22);
		this.txtOuterqty.Name = "txtOuterqty";
		this.txtOuterqty.Size = new System.Drawing.Size(82, 25);
		this.txtOuterqty.TabIndex = 4;
		this.txtOuterqty.TextChanged += new System.EventHandler(txtOuterqty_TextChanged);
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label39.Location = new System.Drawing.Point(211, 52);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(45, 15);
		this.label39.TabIndex = 188;
		this.label39.Text = "Weight";
		this.txtweight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtweight.Enabled = false;
		this.txtweight.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtweight.Location = new System.Drawing.Point(208, 71);
		this.txtweight.Name = "txtweight";
		this.txtweight.Size = new System.Drawing.Size(132, 25);
		this.txtweight.TabIndex = 9;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(339, 52);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(52, 15);
		this.label27.TabIndex = 188;
		this.label27.Text = "Remarks";
		this.txtremarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtremarks.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtremarks.Location = new System.Drawing.Point(343, 71);
		this.txtremarks.Name = "txtremarks";
		this.txtremarks.Size = new System.Drawing.Size(650, 25);
		this.txtremarks.TabIndex = 10;
		this.btnplus.BackColor = System.Drawing.Color.White;
		this.btnplus.FlatAppearance.BorderSize = 0;
		this.btnplus.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnplus.Location = new System.Drawing.Point(997, 71);
		this.btnplus.Name = "btnplus";
		this.btnplus.Size = new System.Drawing.Size(39, 26);
		this.btnplus.TabIndex = 12;
		this.btnplus.Text = "+";
		this.btnplus.UseVisualStyleBackColor = false;
		this.btnplus.Click += new System.EventHandler(btnplus_Click);
		this.btnUpdateDetail.BackColor = System.Drawing.Color.White;
		this.btnUpdateDetail.FlatAppearance.BorderSize = 0;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetail.Location = new System.Drawing.Point(1037, 71);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(68, 26);
		this.btnUpdateDetail.TabIndex = 13;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Visible = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.btnCancelUpdateDetial.BackColor = System.Drawing.Color.White;
		this.btnCancelUpdateDetial.FlatAppearance.BorderSize = 0;
		this.btnCancelUpdateDetial.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelUpdateDetial.Location = new System.Drawing.Point(1109, 71);
		this.btnCancelUpdateDetial.Name = "btnCancelUpdateDetial";
		this.btnCancelUpdateDetial.Size = new System.Drawing.Size(70, 26);
		this.btnCancelUpdateDetial.TabIndex = 14;
		this.btnCancelUpdateDetial.Text = "Cancel";
		this.btnCancelUpdateDetial.UseVisualStyleBackColor = false;
		this.btnCancelUpdateDetial.Visible = false;
		this.btnCancelUpdateDetial.Click += new System.EventHandler(btnCancelUpdateDetial_Click);
		this.tabPage4.Controls.Add(this.panel15);
		this.tabPage4.Location = new System.Drawing.Point(4, 27);
		this.tabPage4.Name = "tabPage4";
		this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage4.Size = new System.Drawing.Size(1226, 372);
		this.tabPage4.TabIndex = 1;
		this.tabPage4.Text = "Input Detail";
		this.tabPage4.UseVisualStyleBackColor = true;
		this.panel15.Controls.Add(this.panel7);
		this.panel15.Controls.Add(this.panelinputgrd);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel15.Location = new System.Drawing.Point(3, 3);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(1220, 366);
		this.panel15.TabIndex = 0;
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.grdInputDetail);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(0, 112);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1220, 254);
		this.panel7.TabIndex = 207;
		((ScrollableBase)this.grdInputDetail).BackColor = System.Drawing.Color.White;
		this.grdInputDetail.ColumnAutoResize = true;
		grdInputDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdInputDetail.DesignTimeLayout = grdInputDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdInputDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdInputDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdInputDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdInputDetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdInputDetail).Name = "grdInputDetail";
		((ScrollableBase)this.grdInputDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdInputDetail).Size = new System.Drawing.Size(1220, 254);
		((System.Windows.Forms.Control)(object)this.grdInputDetail).TabIndex = 0;
		this.grdInputDetail.TotalRow = (InheritableBoolean)1;
		this.grdInputDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdInputDetail).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdInputDetail).DoubleClick += new System.EventHandler(grdInputDetail_DoubleClick);
		this.panelinputgrd.BackColor = System.Drawing.Color.White;
		this.panelinputgrd.Controls.Add(this.label42);
		this.panelinputgrd.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPackTypeInput);
		this.panelinputgrd.Controls.Add(this.txtweightinput);
		this.panelinputgrd.Controls.Add(this.label29);
		this.panelinputgrd.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemInput);
		this.panelinputgrd.Controls.Add(this.btnadddetailinputgrd);
		this.panelinputgrd.Controls.Add(this.txtremarksinput);
		this.panelinputgrd.Controls.Add(this.btnupdategrdinput);
		this.panelinputgrd.Controls.Add(this.label36);
		this.panelinputgrd.Controls.Add(this.btncancelgrdinput);
		this.panelinputgrd.Controls.Add((System.Windows.Forms.Control)(object)this.cmbwarehouseinput);
		this.panelinputgrd.Controls.Add(this.label30);
		this.panelinputgrd.Controls.Add(this.label34);
		this.panelinputgrd.Controls.Add((System.Windows.Forms.Control)(object)this.cmbcropyearinput);
		this.panelinputgrd.Controls.Add(this.label45);
		this.panelinputgrd.Controls.Add(this.label33);
		this.panelinputgrd.Controls.Add((System.Windows.Forms.Control)(object)this.cmbjoblotinput);
		this.panelinputgrd.Controls.Add(this.txtqtyinput);
		this.panelinputgrd.Controls.Add(this.label32);
		this.panelinputgrd.Controls.Add(this.label31);
		this.panelinputgrd.Controls.Add((System.Windows.Forms.Control)(object)this.cmbpacksizeinput);
		this.panelinputgrd.Dock = System.Windows.Forms.DockStyle.Top;
		this.panelinputgrd.Location = new System.Drawing.Point(0, 0);
		this.panelinputgrd.Name = "panelinputgrd";
		this.panelinputgrd.Size = new System.Drawing.Size(1220, 112);
		this.panelinputgrd.TabIndex = 206;
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label42.Location = new System.Drawing.Point(431, 4);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(73, 15);
		this.label42.TabIndex = 204;
		this.label42.Text = "PackingType";
		this.cmbPackTypeInput.AllowNull = (DefaultableBoolean)1;
		this.cmbPackTypeInput.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPackTypeInput.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPackTypeInput.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPackTypeInput).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbPackTypeInput).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPackTypeInput).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPackTypeInput).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPackTypeInput).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.cmbPackTypeInput).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPackTypeInput).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbPackTypeInput).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPackTypeInput).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPackTypeInput).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPackTypeInput).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.cmbPackTypeInput).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPackTypeInput).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPackTypeInput).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance33).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPackTypeInput).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.cmbPackTypeInput).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPackTypeInput).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPackTypeInput).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance132).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPackTypeInput).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.cmbPackTypeInput).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPackTypeInput).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPackTypeInput).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.cmbPackTypeInput).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPackTypeInput).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance134;
		((UltraGridBase)this.cmbPackTypeInput).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPackTypeInput).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPackTypeInput).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPackTypeInput).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbPackTypeInput.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPackTypeInput).Location = new System.Drawing.Point(431, 23);
		((System.Windows.Forms.Control)(object)this.cmbPackTypeInput).Name = "cmbPackTypeInput";
		((System.Windows.Forms.Control)(object)this.cmbPackTypeInput).Size = new System.Drawing.Size(124, 26);
		((System.Windows.Forms.Control)(object)this.cmbPackTypeInput).TabIndex = 1;
		this.txtweightinput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtweightinput.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtweightinput.Location = new System.Drawing.Point(985, 23);
		this.txtweightinput.Name = "txtweightinput";
		this.txtweightinput.Size = new System.Drawing.Size(120, 25);
		this.txtweightinput.TabIndex = 6;
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.Location = new System.Drawing.Point(7, 3);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(72, 17);
		this.label29.TabIndex = 178;
		this.label29.Text = "Item Name";
		this.cmbItemInput.AllowNull = (DefaultableBoolean)1;
		this.cmbItemInput.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemInput.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemInput.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.Appearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance83).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance83).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance83).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemInput).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance84;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemInput).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance85).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance85).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance85).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance86).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance86;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance87).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance88;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance89).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance91).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance91).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance91).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance91).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance91;
		((AppearanceBase)appearance92).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemInput).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance93;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance94;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemInput).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemInput).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItemInput.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemInput).Location = new System.Drawing.Point(7, 23);
		((System.Windows.Forms.Control)(object)this.cmbItemInput).Name = "cmbItemInput";
		((System.Windows.Forms.Control)(object)this.cmbItemInput).Size = new System.Drawing.Size(421, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemInput).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbItemInput).Leave += new System.EventHandler(cmbItemInput_Leave);
		this.btnadddetailinputgrd.BackColor = System.Drawing.Color.White;
		this.btnadddetailinputgrd.FlatAppearance.BorderSize = 0;
		this.btnadddetailinputgrd.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnadddetailinputgrd.Location = new System.Drawing.Point(809, 72);
		this.btnadddetailinputgrd.Name = "btnadddetailinputgrd";
		this.btnadddetailinputgrd.Size = new System.Drawing.Size(39, 26);
		this.btnadddetailinputgrd.TabIndex = 11;
		this.btnadddetailinputgrd.Text = "+";
		this.btnadddetailinputgrd.UseVisualStyleBackColor = false;
		this.btnadddetailinputgrd.Click += new System.EventHandler(btnadddetailinputgrd_Click);
		this.txtremarksinput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtremarksinput.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtremarksinput.Location = new System.Drawing.Point(207, 72);
		this.txtremarksinput.Name = "txtremarksinput";
		this.txtremarksinput.Size = new System.Drawing.Size(600, 25);
		this.txtremarksinput.TabIndex = 8;
		this.btnupdategrdinput.BackColor = System.Drawing.Color.White;
		this.btnupdategrdinput.FlatAppearance.BorderSize = 0;
		this.btnupdategrdinput.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnupdategrdinput.Location = new System.Drawing.Point(849, 72);
		this.btnupdategrdinput.Name = "btnupdategrdinput";
		this.btnupdategrdinput.Size = new System.Drawing.Size(68, 26);
		this.btnupdategrdinput.TabIndex = 13;
		this.btnupdategrdinput.Text = "Update";
		this.btnupdategrdinput.UseVisualStyleBackColor = false;
		this.btnupdategrdinput.Visible = false;
		this.btnupdategrdinput.Click += new System.EventHandler(btnupdategrdinput_Click);
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(204, 54);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(52, 15);
		this.label36.TabIndex = 202;
		this.label36.Text = "Remarks";
		this.btncancelgrdinput.BackColor = System.Drawing.Color.White;
		this.btncancelgrdinput.FlatAppearance.BorderSize = 0;
		this.btncancelgrdinput.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btncancelgrdinput.Location = new System.Drawing.Point(921, 72);
		this.btncancelgrdinput.Name = "btncancelgrdinput";
		this.btncancelgrdinput.Size = new System.Drawing.Size(68, 26);
		this.btncancelgrdinput.TabIndex = 12;
		this.btncancelgrdinput.Text = "Cancel";
		this.btncancelgrdinput.UseVisualStyleBackColor = false;
		this.btncancelgrdinput.Visible = false;
		this.btncancelgrdinput.Click += new System.EventHandler(btncancelgrdinput_Click);
		this.cmbwarehouseinput.AllowNull = (DefaultableBoolean)1;
		this.cmbwarehouseinput.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbwarehouseinput.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbwarehouseinput.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance185).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance185).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbwarehouseinput).DisplayLayout.Appearance = (AppearanceBase)(object)appearance185;
		((UltraGridBase)this.cmbwarehouseinput).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbwarehouseinput).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance186).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance186).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance186).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance186).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbwarehouseinput).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance186;
		((AppearanceBase)appearance187).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbwarehouseinput).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance187;
		((SpecialBoxBase)((UltraGridBase)this.cmbwarehouseinput).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance188).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance188).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance188).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance188).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbwarehouseinput).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance188;
		((UltraGridBase)this.cmbwarehouseinput).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbwarehouseinput).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance189).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance189).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbwarehouseinput).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance189;
		((AppearanceBase)appearance191).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance191).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbwarehouseinput).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance191;
		((UltraGridBase)this.cmbwarehouseinput).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbwarehouseinput).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance192).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbwarehouseinput).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance192;
		((AppearanceBase)appearance193).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance193).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbwarehouseinput).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance193;
		((UltraGridBase)this.cmbwarehouseinput).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbwarehouseinput).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance194).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance194).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance194).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance194).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance194).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbwarehouseinput).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance194;
		((AppearanceBase)appearance195).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbwarehouseinput).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance195;
		((UltraGridBase)this.cmbwarehouseinput).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbwarehouseinput).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance196).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance196).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbwarehouseinput).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance196;
		((UltraGridBase)this.cmbwarehouseinput).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance197).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbwarehouseinput).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance197;
		((UltraGridBase)this.cmbwarehouseinput).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbwarehouseinput).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbwarehouseinput).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbwarehouseinput).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbwarehouseinput.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbwarehouseinput).Location = new System.Drawing.Point(7, 72);
		((System.Windows.Forms.Control)(object)this.cmbwarehouseinput).Name = "cmbwarehouseinput";
		((System.Windows.Forms.Control)(object)this.cmbwarehouseinput).Size = new System.Drawing.Size(198, 26);
		((System.Windows.Forms.Control)(object)this.cmbwarehouseinput).TabIndex = 7;
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(558, 3);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(66, 17);
		this.label30.TabIndex = 190;
		this.label30.Text = "Crop Year";
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.Location = new System.Drawing.Point(7, 54);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(68, 15);
		this.label34.TabIndex = 200;
		this.label34.Text = "WareHouse";
		this.cmbcropyearinput.AllowNull = (DefaultableBoolean)1;
		this.cmbcropyearinput.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbcropyearinput.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbcropyearinput.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance198).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance198).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcropyearinput).DisplayLayout.Appearance = (AppearanceBase)(object)appearance198;
		((UltraGridBase)this.cmbcropyearinput).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcropyearinput).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance199).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance199).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance199).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance199).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcropyearinput).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance199;
		((AppearanceBase)appearance200).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcropyearinput).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance200;
		((SpecialBoxBase)((UltraGridBase)this.cmbcropyearinput).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance202).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance202).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance202).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance202).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcropyearinput).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance202;
		((UltraGridBase)this.cmbcropyearinput).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcropyearinput).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance203).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance203).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcropyearinput).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance203;
		((AppearanceBase)appearance204).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance204).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcropyearinput).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance204;
		((UltraGridBase)this.cmbcropyearinput).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcropyearinput).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance205).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcropyearinput).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance205;
		((AppearanceBase)appearance206).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance206).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcropyearinput).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance206;
		((UltraGridBase)this.cmbcropyearinput).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcropyearinput).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance207).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance207).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance207).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance207).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance207).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcropyearinput).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance207;
		((AppearanceBase)appearance208).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcropyearinput).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance208;
		((UltraGridBase)this.cmbcropyearinput).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcropyearinput).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance209).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance209).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcropyearinput).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance209;
		((UltraGridBase)this.cmbcropyearinput).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance210).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcropyearinput).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance210;
		((UltraGridBase)this.cmbcropyearinput).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcropyearinput).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcropyearinput).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcropyearinput).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbcropyearinput.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbcropyearinput).Location = new System.Drawing.Point(558, 23);
		((System.Windows.Forms.Control)(object)this.cmbcropyearinput).Name = "cmbcropyearinput";
		((System.Windows.Forms.Control)(object)this.cmbcropyearinput).Size = new System.Drawing.Size(115, 26);
		((System.Windows.Forms.Control)(object)this.cmbcropyearinput).TabIndex = 2;
		this.label45.AutoSize = true;
		this.label45.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label45.Location = new System.Drawing.Point(679, 3);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(52, 17);
		this.label45.TabIndex = 192;
		this.label45.Text = "Job/Lot";
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(988, 4);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(45, 15);
		this.label33.TabIndex = 198;
		this.label33.Text = "Weight";
		this.cmbjoblotinput.AllowNull = (DefaultableBoolean)1;
		this.cmbjoblotinput.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbjoblotinput.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbjoblotinput.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance211).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance211).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbjoblotinput).DisplayLayout.Appearance = (AppearanceBase)(object)appearance211;
		((UltraGridBase)this.cmbjoblotinput).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbjoblotinput).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance213).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance213).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance213).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance213).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbjoblotinput).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance213;
		((AppearanceBase)appearance214).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbjoblotinput).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance214;
		((SpecialBoxBase)((UltraGridBase)this.cmbjoblotinput).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance215).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance215).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance215).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance215).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbjoblotinput).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance215;
		((UltraGridBase)this.cmbjoblotinput).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbjoblotinput).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance216).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance216).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbjoblotinput).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance216;
		((AppearanceBase)appearance217).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance217).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbjoblotinput).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance217;
		((UltraGridBase)this.cmbjoblotinput).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbjoblotinput).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance218).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbjoblotinput).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance218;
		((AppearanceBase)appearance219).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance219).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbjoblotinput).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance219;
		((UltraGridBase)this.cmbjoblotinput).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbjoblotinput).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance220).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance220).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance220).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance220).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance220).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbjoblotinput).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance220;
		((AppearanceBase)appearance221).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbjoblotinput).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance221;
		((UltraGridBase)this.cmbjoblotinput).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbjoblotinput).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance222).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance222).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbjoblotinput).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance222;
		((UltraGridBase)this.cmbjoblotinput).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance225).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbjoblotinput).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance225;
		((UltraGridBase)this.cmbjoblotinput).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbjoblotinput).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbjoblotinput).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbjoblotinput).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbjoblotinput.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbjoblotinput).Location = new System.Drawing.Point(677, 23);
		((System.Windows.Forms.Control)(object)this.cmbjoblotinput).Name = "cmbjoblotinput";
		((System.Windows.Forms.Control)(object)this.cmbjoblotinput).Size = new System.Drawing.Size(100, 26);
		((System.Windows.Forms.Control)(object)this.cmbjoblotinput).TabIndex = 3;
		this.txtqtyinput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtqtyinput.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtqtyinput.Location = new System.Drawing.Point(779, 23);
		this.txtqtyinput.Name = "txtqtyinput";
		this.txtqtyinput.Size = new System.Drawing.Size(82, 25);
		this.txtqtyinput.TabIndex = 4;
		this.txtqtyinput.TextChanged += new System.EventHandler(txtqtyinput_TextChanged);
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.Location = new System.Drawing.Point(867, 3);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(61, 17);
		this.label32.TabIndex = 195;
		this.label32.Text = "Pack Size";
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.Location = new System.Drawing.Point(779, 4);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(26, 15);
		this.label31.TabIndex = 196;
		this.label31.Text = "Qty";
		this.cmbpacksizeinput.AllowNull = (DefaultableBoolean)1;
		this.cmbpacksizeinput.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbpacksizeinput.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbpacksizeinput.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbpacksizeinput).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.cmbpacksizeinput).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbpacksizeinput).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbpacksizeinput).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbpacksizeinput).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.cmbpacksizeinput).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbpacksizeinput).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbpacksizeinput).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbpacksizeinput).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbpacksizeinput).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbpacksizeinput).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbpacksizeinput).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbpacksizeinput).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbpacksizeinput).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbpacksizeinput).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbpacksizeinput).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbpacksizeinput).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbpacksizeinput).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbpacksizeinput).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbpacksizeinput).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbpacksizeinput).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbpacksizeinput).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbpacksizeinput).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbpacksizeinput).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbpacksizeinput).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbpacksizeinput).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbpacksizeinput).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbpacksizeinput).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbpacksizeinput.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbpacksizeinput).Location = new System.Drawing.Point(864, 23);
		((System.Windows.Forms.Control)(object)this.cmbpacksizeinput).Name = "cmbpacksizeinput";
		((System.Windows.Forms.Control)(object)this.cmbpacksizeinput).Size = new System.Drawing.Size(118, 26);
		((System.Windows.Forms.Control)(object)this.cmbpacksizeinput).TabIndex = 5;
		this.cmbpacksizeinput.ValueChanged += new System.EventHandler(cmbpacksizeinput_ValueChanged);
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.btnProductionPlant);
		this.panel4.Controls.Add(this.label40);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbentrytype);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDocNo);
		this.panel4.Controls.Add(this.label28);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbFiltersType);
		this.panel4.Controls.Add(this.cmbfiltertype);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWipItem);
		this.panel4.Controls.Add(this.label23);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbLotAc);
		this.panel4.Controls.Add(this.label22);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbByProductionAc);
		this.panel4.Controls.Add(this.label21);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbFinishGoodsAc);
		this.panel4.Controls.Add(this.label20);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWorkInProcess);
		this.panel4.Controls.Add(this.label15);
		this.panel4.Controls.Add(this.chkPrint);
		this.panel4.Controls.Add(this.label19);
		this.panel4.Controls.Add(this.txtPlanTypeSrNo);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbStatus);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbProductionType);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPlantFeader);
		this.panel4.Controls.Add(this.txtEndDate);
		this.panel4.Controls.Add(this.label18);
		this.panel4.Controls.Add(this.label17);
		this.panel4.Controls.Add(this.txtDeliveryInstruction);
		this.panel4.Controls.Add(this.txtstartDate);
		this.panel4.Controls.Add(this.label16);
		this.panel4.Controls.Add(this.label13);
		this.panel4.Controls.Add(this.label4);
		this.panel4.Controls.Add(this.txtProductionInstruction);
		this.panel4.Controls.Add(this.label11);
		this.panel4.Controls.Add(this.txtOtherInstruction);
		this.panel4.Controls.Add(this.label10);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPlanType);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbSaleOrder);
		this.panel4.Controls.Add(this.label9);
		this.panel4.Controls.Add(this.label6);
		this.panel4.Controls.Add(this.txtPlantInstruction);
		this.panel4.Controls.Add(this.label3);
		this.panel4.Controls.Add(this.txtOtherReference);
		this.panel4.Controls.Add(this.label1);
		this.panel4.Controls.Add(this.txtInvoiceReference);
		this.panel4.Controls.Add(this.label2);
		this.panel4.Controls.Add(this.label8);
		this.panel4.Controls.Add(this.label7);
		this.panel4.Controls.Add(this.txtPlanCode);
		this.panel4.Controls.Add(this.txtPlanDate);
		this.panel4.Controls.Add(this.label5);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 30);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1234, 284);
		this.panel4.TabIndex = 60;
		this.btnProductionPlant.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnProductionPlant.Location = new System.Drawing.Point(834, 4);
		this.btnProductionPlant.Name = "btnProductionPlant";
		this.btnProductionPlant.Size = new System.Drawing.Size(23, 23);
		this.btnProductionPlant.TabIndex = 207;
		this.btnProductionPlant.TabStop = false;
		this.btnProductionPlant.UseVisualStyleBackColor = true;
		this.btnProductionPlant.Click += new System.EventHandler(btnProductionPlant_Click);
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.Location = new System.Drawing.Point(13, 221);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(64, 17);
		this.label40.TabIndex = 204;
		this.label40.Text = "EntryType";
		this.cmbentrytype.AllowNull = (DefaultableBoolean)1;
		this.cmbentrytype.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbentrytype.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbentrytype.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance307).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance307).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbentrytype).DisplayLayout.Appearance = (AppearanceBase)(object)appearance307;
		((UltraGridBase)this.cmbentrytype).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbentrytype).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance308).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance308).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance308).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance308).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbentrytype).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance308;
		((AppearanceBase)appearance309).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbentrytype).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance309;
		((SpecialBoxBase)((UltraGridBase)this.cmbentrytype).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance310).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance310).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance310).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance310).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbentrytype).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance310;
		((UltraGridBase)this.cmbentrytype).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbentrytype).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance311).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance311).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbentrytype).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance311;
		((AppearanceBase)appearance312).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance312).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbentrytype).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance312;
		((UltraGridBase)this.cmbentrytype).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbentrytype).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbentrytype).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance3;
		((AppearanceBase)appearance4).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance4).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbentrytype).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.cmbentrytype).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbentrytype).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance5).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance5).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance5).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbentrytype).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance5;
		((AppearanceBase)appearance6).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbentrytype).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.cmbentrytype).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbentrytype).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbentrytype).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.cmbentrytype).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbentrytype).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.cmbentrytype).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbentrytype).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbentrytype).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbentrytype).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbentrytype.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbentrytype).Location = new System.Drawing.Point(16, 241);
		((System.Windows.Forms.Control)(object)this.cmbentrytype).Name = "cmbentrytype";
		((System.Windows.Forms.Control)(object)this.cmbentrytype).Size = new System.Drawing.Size(140, 26);
		((System.Windows.Forms.Control)(object)this.cmbentrytype).TabIndex = 21;
		this.cmbentrytype.ValueChanged += new System.EventHandler(cmbentrytype_ValueChanged);
		this.cmbDocNo.AllowNull = (DefaultableBoolean)1;
		this.cmbDocNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDocNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDocNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance172).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDocNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance172;
		((UltraGridBase)this.cmbDocNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDocNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance173).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance173).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance173).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance173).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDocNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance173;
		((AppearanceBase)appearance174).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDocNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance174;
		((SpecialBoxBase)((UltraGridBase)this.cmbDocNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance175).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance175).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance175).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDocNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance175;
		((UltraGridBase)this.cmbDocNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDocNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance176).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance176).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDocNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance176;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance177).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDocNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance177;
		((UltraGridBase)this.cmbDocNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDocNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDocNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance178;
		((AppearanceBase)appearance180).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance180).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDocNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance180;
		((UltraGridBase)this.cmbDocNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDocNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance181).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance181).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance181).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance181).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance181).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDocNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance181;
		((AppearanceBase)appearance182).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDocNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance182;
		((UltraGridBase)this.cmbDocNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDocNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance183).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance183).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDocNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance183;
		((UltraGridBase)this.cmbDocNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance184).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDocNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance184;
		((UltraGridBase)this.cmbDocNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDocNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDocNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDocNo).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDocNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDocNo).Location = new System.Drawing.Point(346, 241);
		((System.Windows.Forms.Control)(object)this.cmbDocNo).Name = "cmbDocNo";
		((System.Windows.Forms.Control)(object)this.cmbDocNo).Size = new System.Drawing.Size(259, 26);
		((System.Windows.Forms.Control)(object)this.cmbDocNo).TabIndex = 23;
		((System.Windows.Forms.Control)(object)this.cmbDocNo).Leave += new System.EventHandler(cmbDocNo_Leave);
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.Location = new System.Drawing.Point(351, 219);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(99, 17);
		this.label28.TabIndex = 178;
		this.label28.Text = "Doc No / Other";
		this.cmbFiltersType.AllowNull = (DefaultableBoolean)1;
		this.cmbFiltersType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbFiltersType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbFiltersType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance95).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbFiltersType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.cmbFiltersType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbFiltersType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance96).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance96).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance96).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbFiltersType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance97).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbFiltersType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance97;
		((SpecialBoxBase)((UltraGridBase)this.cmbFiltersType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance98).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance98).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance98).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbFiltersType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance98;
		((UltraGridBase)this.cmbFiltersType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbFiltersType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance99).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbFiltersType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance99;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance100).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbFiltersType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance100;
		((UltraGridBase)this.cmbFiltersType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbFiltersType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbFiltersType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance102;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance103).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbFiltersType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.cmbFiltersType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbFiltersType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance104).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance104).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance104).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance104).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbFiltersType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance104;
		((AppearanceBase)appearance105).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbFiltersType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.cmbFiltersType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbFiltersType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbFiltersType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.cmbFiltersType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbFiltersType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.cmbFiltersType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbFiltersType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbFiltersType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbFiltersType).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbFiltersType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbFiltersType).Location = new System.Drawing.Point(162, 241);
		((System.Windows.Forms.Control)(object)this.cmbFiltersType).Name = "cmbFiltersType";
		((System.Windows.Forms.Control)(object)this.cmbFiltersType).Size = new System.Drawing.Size(178, 26);
		((System.Windows.Forms.Control)(object)this.cmbFiltersType).TabIndex = 22;
		((System.Windows.Forms.Control)(object)this.cmbFiltersType).Leave += new System.EventHandler(cmbFiltersType_Leave);
		this.cmbfiltertype.AutoSize = true;
		this.cmbfiltertype.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbfiltertype.Location = new System.Drawing.Point(161, 219);
		this.cmbfiltertype.Name = "cmbfiltertype";
		this.cmbfiltertype.Size = new System.Drawing.Size(67, 17);
		this.cmbfiltertype.TabIndex = 178;
		this.cmbfiltertype.Text = "Filter Type";
		this.CmbWipItem.AllowNull = (DefaultableBoolean)1;
		this.CmbWipItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWipItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWipItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWipItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance54;
		((SpecialBoxBase)((UltraGridBase)this.CmbWipItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance60).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance61).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance149).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWipItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance149;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance150).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance150;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWipItem).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWipItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWipItem).Location = new System.Drawing.Point(935, 58);
		((System.Windows.Forms.Control)(object)this.CmbWipItem).Name = "CmbWipItem";
		((System.Windows.Forms.Control)(object)this.CmbWipItem).Size = new System.Drawing.Size(171, 24);
		((System.Windows.Forms.Control)(object)this.CmbWipItem).TabIndex = 11;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.Location = new System.Drawing.Point(859, 62);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(58, 16);
		this.label23.TabIndex = 176;
		this.label23.Text = "WIPItem";
		this.CmbLotAc.AllowNull = (DefaultableBoolean)1;
		this.CmbLotAc.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbLotAc.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbLotAc.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance159).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbLotAc).DisplayLayout.Appearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.CmbLotAc).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbLotAc).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance160).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance160).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance160).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbLotAc).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance160;
		((AppearanceBase)appearance161).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLotAc).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance161;
		((SpecialBoxBase)((UltraGridBase)this.CmbLotAc).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance162).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance162).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance162).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLotAc).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance162;
		((UltraGridBase)this.CmbLotAc).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbLotAc).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance163).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbLotAc).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance163;
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance164).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbLotAc).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.CmbLotAc).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbLotAc).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLotAc).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance165;
		((AppearanceBase)appearance166).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance166).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbLotAc).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance166;
		((UltraGridBase)this.CmbLotAc).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbLotAc).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance167).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance167).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance167).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance167).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLotAc).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance167;
		((AppearanceBase)appearance241).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbLotAc).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance241;
		((UltraGridBase)this.CmbLotAc).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbLotAc).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance242).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance242).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbLotAc).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance242;
		((UltraGridBase)this.CmbLotAc).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance243).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbLotAc).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance243;
		((UltraGridBase)this.CmbLotAc).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbLotAc).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbLotAc).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbLotAc).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbLotAc.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbLotAc).Location = new System.Drawing.Point(973, 85);
		((System.Windows.Forms.Control)(object)this.CmbLotAc).Name = "CmbLotAc";
		((System.Windows.Forms.Control)(object)this.CmbLotAc).Size = new System.Drawing.Size(133, 24);
		((System.Windows.Forms.Control)(object)this.CmbLotAc).TabIndex = 15;
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(918, 89);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(49, 16);
		this.label22.TabIndex = 174;
		this.label22.Text = "JobLot";
		this.CmbByProductionAc.AllowNull = (DefaultableBoolean)1;
		this.CmbByProductionAc.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbByProductionAc.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbByProductionAc.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance284).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance284).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbByProductionAc).DisplayLayout.Appearance = (AppearanceBase)(object)appearance284;
		((UltraGridBase)this.CmbByProductionAc).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbByProductionAc).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance285).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance285).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance285).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance285).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbByProductionAc).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance285;
		((AppearanceBase)appearance286).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbByProductionAc).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance286;
		((SpecialBoxBase)((UltraGridBase)this.CmbByProductionAc).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance287).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance287).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance287).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance287).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbByProductionAc).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance287;
		((UltraGridBase)this.CmbByProductionAc).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbByProductionAc).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance288).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance288).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbByProductionAc).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance288;
		((AppearanceBase)appearance289).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance289).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbByProductionAc).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance289;
		((UltraGridBase)this.CmbByProductionAc).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbByProductionAc).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance290).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbByProductionAc).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance290;
		((AppearanceBase)appearance292).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance292).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbByProductionAc).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance292;
		((UltraGridBase)this.CmbByProductionAc).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbByProductionAc).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance293).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance293).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance293).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance293).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance293).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbByProductionAc).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance293;
		((AppearanceBase)appearance294).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbByProductionAc).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance294;
		((UltraGridBase)this.CmbByProductionAc).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbByProductionAc).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance295).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance295).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbByProductionAc).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance295;
		((UltraGridBase)this.CmbByProductionAc).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance296).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbByProductionAc).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance296;
		((UltraGridBase)this.CmbByProductionAc).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbByProductionAc).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbByProductionAc).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbByProductionAc).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbByProductionAc.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbByProductionAc).Location = new System.Drawing.Point(731, 85);
		((System.Windows.Forms.Control)(object)this.CmbByProductionAc).Name = "CmbByProductionAc";
		((System.Windows.Forms.Control)(object)this.CmbByProductionAc).Size = new System.Drawing.Size(181, 24);
		((System.Windows.Forms.Control)(object)this.CmbByProductionAc).TabIndex = 14;
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(621, 89);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(104, 16);
		this.label21.TabIndex = 172;
		this.label21.Text = "ByProductionAc";
		this.CmbFinishGoodsAc.AllowNull = (DefaultableBoolean)1;
		this.CmbFinishGoodsAc.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbFinishGoodsAc.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbFinishGoodsAc.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbFinishGoodsAc).DisplayLayout.Appearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.CmbFinishGoodsAc).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbFinishGoodsAc).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance120).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance120).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance120).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbFinishGoodsAc).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance121).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFinishGoodsAc).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance121;
		((SpecialBoxBase)((UltraGridBase)this.CmbFinishGoodsAc).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance122).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance122).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance122).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFinishGoodsAc).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.CmbFinishGoodsAc).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbFinishGoodsAc).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance123).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbFinishGoodsAc).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance123;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbFinishGoodsAc).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.CmbFinishGoodsAc).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbFinishGoodsAc).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFinishGoodsAc).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance126;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance127).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbFinishGoodsAc).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.CmbFinishGoodsAc).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbFinishGoodsAc).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance128).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance128).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance128).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFinishGoodsAc).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance128;
		((AppearanceBase)appearance62).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbFinishGoodsAc).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.CmbFinishGoodsAc).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbFinishGoodsAc).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbFinishGoodsAc).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbFinishGoodsAc).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbFinishGoodsAc).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.CmbFinishGoodsAc).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbFinishGoodsAc).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbFinishGoodsAc).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbFinishGoodsAc).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbFinishGoodsAc.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbFinishGoodsAc).Location = new System.Drawing.Point(431, 85);
		((System.Windows.Forms.Control)(object)this.CmbFinishGoodsAc).Name = "CmbFinishGoodsAc";
		((System.Windows.Forms.Control)(object)this.CmbFinishGoodsAc).Size = new System.Drawing.Size(185, 24);
		((System.Windows.Forms.Control)(object)this.CmbFinishGoodsAc).TabIndex = 13;
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(327, 89);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(100, 16);
		this.label20.TabIndex = 170;
		this.label20.Text = "FinishGoodsAc";
		this.CmbWorkInProcess.AllowNull = (DefaultableBoolean)1;
		this.CmbWorkInProcess.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWorkInProcess.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWorkInProcess.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance271).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance271).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Appearance = (AppearanceBase)(object)appearance271;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance152).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance152).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance152).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance152;
		((AppearanceBase)appearance153).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance153;
		((SpecialBoxBase)((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance154).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance154).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance154).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance154;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance275).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance275).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance275;
		((AppearanceBase)appearance276).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance276).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance276;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance277).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance277;
		((AppearanceBase)appearance278).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance278).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance278;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance279).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance279).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance279).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance279).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance279).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance279;
		((AppearanceBase)appearance129).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWorkInProcess).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWorkInProcess.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWorkInProcess).Location = new System.Drawing.Point(145, 85);
		((System.Windows.Forms.Control)(object)this.CmbWorkInProcess).Name = "CmbWorkInProcess";
		((System.Windows.Forms.Control)(object)this.CmbWorkInProcess).Size = new System.Drawing.Size(179, 24);
		((System.Windows.Forms.Control)(object)this.CmbWorkInProcess).TabIndex = 12;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(13, 89);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(106, 16);
		this.label15.TabIndex = 168;
		this.label15.Text = "Work In Process";
		this.chkPrint.AutoSize = true;
		this.chkPrint.Location = new System.Drawing.Point(1109, 90);
		this.chkPrint.Name = "chkPrint";
		this.chkPrint.Size = new System.Drawing.Size(58, 17);
		this.chkPrint.TabIndex = 16;
		this.chkPrint.Text = "Is Print";
		this.chkPrint.UseVisualStyleBackColor = true;
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(1213, 35);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(98, 16);
		this.label19.TabIndex = 165;
		this.label19.Text = "PlanTypeSrNo";
		this.label19.Visible = false;
		this.txtPlanTypeSrNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPlanTypeSrNo.Enabled = false;
		this.txtPlanTypeSrNo.Location = new System.Drawing.Point(1216, 7);
		this.txtPlanTypeSrNo.Name = "txtPlanTypeSrNo";
		this.txtPlanTypeSrNo.Size = new System.Drawing.Size(85, 20);
		this.txtPlanTypeSrNo.TabIndex = 164;
		this.txtPlanTypeSrNo.Visible = false;
		this.cmbStatus.AllowNull = (DefaultableBoolean)1;
		this.cmbStatus.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbStatus.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbStatus.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbStatus).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbStatus).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbStatus).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance223).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbStatus).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance223;
		((SpecialBoxBase)((UltraGridBase)this.cmbStatus).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance247).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance247).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance247).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance247).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbStatus).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance247;
		((UltraGridBase)this.cmbStatus).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbStatus).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance258).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance258).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance258;
		((AppearanceBase)appearance269).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance269).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance269;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance280).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance280;
		((AppearanceBase)appearance291).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance291).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance291;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance302).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance302).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance302).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance302).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance302).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance302;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbStatus).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbStatus).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbStatus).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbStatus).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbStatus.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbStatus).Location = new System.Drawing.Point(689, 58);
		((System.Windows.Forms.Control)(object)this.cmbStatus).Name = "cmbStatus";
		((System.Windows.Forms.Control)(object)this.cmbStatus).Size = new System.Drawing.Size(168, 24);
		((System.Windows.Forms.Control)(object)this.cmbStatus).TabIndex = 10;
		this.cmbProductionType.AllowNull = (DefaultableBoolean)1;
		this.cmbProductionType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbProductionType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbProductionType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance257).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance257).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance257;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance259).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance259).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance259).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance259).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbProductionType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance259;
		((AppearanceBase)appearance260).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance260;
		((SpecialBoxBase)((UltraGridBase)this.cmbProductionType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance261).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance261).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance261).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance261).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance261;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance262).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance262).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance262;
		((AppearanceBase)appearance263).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance263).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance263;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance264).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance264;
		((AppearanceBase)appearance265).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance265).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance265;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance266).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance266).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance266).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance266).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance266).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance266;
		((AppearanceBase)appearance267).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbProductionType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance267;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance268).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance268).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance268;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance270).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance270;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbProductionType).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbProductionType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbProductionType).Location = new System.Drawing.Point(689, 31);
		((System.Windows.Forms.Control)(object)this.cmbProductionType).Name = "cmbProductionType";
		((System.Windows.Forms.Control)(object)this.cmbProductionType).Size = new System.Drawing.Size(168, 24);
		((System.Windows.Forms.Control)(object)this.cmbProductionType).TabIndex = 6;
		this.cmbPlantFeader.AllowNull = (DefaultableBoolean)1;
		this.cmbPlantFeader.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPlantFeader.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPlantFeader.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance244).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance244).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.Appearance = (AppearanceBase)(object)appearance244;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance155).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance155).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance155).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPlantFeader).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance155;
		((AppearanceBase)appearance156).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance156;
		((SpecialBoxBase)((UltraGridBase)this.cmbPlantFeader).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance158).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance158).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance158).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance158;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance249).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance249).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance249;
		((AppearanceBase)appearance250).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance250).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance250;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance251).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance251;
		((AppearanceBase)appearance252).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance252).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance252;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance253).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance253).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance253).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance253).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance253).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance253;
		((AppearanceBase)appearance254).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance254;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance255).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance255).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance255;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance256).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance256;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPlantFeader).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbPlantFeader.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPlantFeader).Location = new System.Drawing.Point(689, 4);
		((System.Windows.Forms.Control)(object)this.cmbPlantFeader).Name = "cmbPlantFeader";
		((System.Windows.Forms.Control)(object)this.cmbPlantFeader).Size = new System.Drawing.Size(144, 24);
		((System.Windows.Forms.Control)(object)this.cmbPlantFeader).TabIndex = 2;
		this.txtEndDate.CustomFormat = "dd-MMM-yy";
		this.txtEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtEndDate.Location = new System.Drawing.Point(399, 60);
		this.txtEndDate.Name = "txtEndDate";
		this.txtEndDate.Size = new System.Drawing.Size(171, 20);
		this.txtEndDate.TabIndex = 9;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(321, 62);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(64, 16);
		this.label18.TabIndex = 163;
		this.label18.Text = "End Date";
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(11, 165);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(120, 16);
		this.label17.TabIndex = 161;
		this.label17.Text = "Delivery Instruction";
		this.txtDeliveryInstruction.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDeliveryInstruction.Location = new System.Drawing.Point(145, 161);
		this.txtDeliveryInstruction.Name = "txtDeliveryInstruction";
		this.txtDeliveryInstruction.Size = new System.Drawing.Size(961, 20);
		this.txtDeliveryInstruction.TabIndex = 19;
		this.txtstartDate.CustomFormat = "dd-MMM-yy";
		this.txtstartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtstartDate.Location = new System.Drawing.Point(145, 60);
		this.txtstartDate.Name = "txtstartDate";
		this.txtstartDate.Size = new System.Drawing.Size(171, 20);
		this.txtstartDate.TabIndex = 8;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(11, 62);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(67, 16);
		this.label16.TabIndex = 159;
		this.label16.Text = "Start Date";
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(575, 35);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(107, 16);
		this.label13.TabIndex = 157;
		this.label13.Text = "Production Type";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(11, 138);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(134, 16);
		this.label4.TabIndex = 155;
		this.label4.Text = "Production Instruction";
		this.txtProductionInstruction.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtProductionInstruction.Location = new System.Drawing.Point(145, 138);
		this.txtProductionInstruction.Name = "txtProductionInstruction";
		this.txtProductionInstruction.Size = new System.Drawing.Size(961, 20);
		this.txtProductionInstruction.TabIndex = 18;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(11, 192);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(102, 16);
		this.label11.TabIndex = 153;
		this.label11.Text = "Other Instruction";
		this.txtOtherInstruction.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOtherInstruction.Location = new System.Drawing.Point(145, 188);
		this.txtOtherInstruction.Name = "txtOtherInstruction";
		this.txtOtherInstruction.Size = new System.Drawing.Size(961, 20);
		this.txtOtherInstruction.TabIndex = 20;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(575, 63);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(41, 15);
		this.label10.TabIndex = 151;
		this.label10.Text = "Status";
		this.cmbPlanType.AllowNull = (DefaultableBoolean)1;
		this.cmbPlanType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPlanType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPlanType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance169).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance169).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance169).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance169).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPlanType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance169;
		((AppearanceBase)appearance170).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance170;
		((SpecialBoxBase)((UltraGridBase)this.cmbPlanType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance171).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance171).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance171).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance171).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance171;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance101;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance113).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance124).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance124).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance124).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance124).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance124;
		((AppearanceBase)appearance135).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPlanType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPlanType).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbPlanType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPlanType).Location = new System.Drawing.Point(935, 4);
		((System.Windows.Forms.Control)(object)this.cmbPlanType).Name = "cmbPlanType";
		((System.Windows.Forms.Control)(object)this.cmbPlanType).Size = new System.Drawing.Size(171, 24);
		((System.Windows.Forms.Control)(object)this.cmbPlanType).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.cmbPlanType).Leave += new System.EventHandler(cmbPlanType_Leave);
		this.cmbSaleOrder.AllowNull = (DefaultableBoolean)1;
		this.cmbSaleOrder.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbSaleOrder.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbSaleOrder.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbSaleOrder).DisplayLayout.Appearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbSaleOrder).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbSaleOrder).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbSaleOrder).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSaleOrder).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance41;
		((SpecialBoxBase)((UltraGridBase)this.cmbSaleOrder).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSaleOrder).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbSaleOrder).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbSaleOrder).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbSaleOrder).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbSaleOrder).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.cmbSaleOrder).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbSaleOrder).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSaleOrder).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbSaleOrder).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.cmbSaleOrder).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbSaleOrder).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSaleOrder).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbSaleOrder).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.cmbSaleOrder).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbSaleOrder).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbSaleOrder).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.cmbSaleOrder).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbSaleOrder).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.cmbSaleOrder).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbSaleOrder).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbSaleOrder).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbSaleOrder).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbSaleOrder.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbSaleOrder).Location = new System.Drawing.Point(935, 31);
		((System.Windows.Forms.Control)(object)this.cmbSaleOrder).Name = "cmbSaleOrder";
		((System.Windows.Forms.Control)(object)this.cmbSaleOrder).Size = new System.Drawing.Size(171, 24);
		((System.Windows.Forms.Control)(object)this.cmbSaleOrder).TabIndex = 7;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(859, 35);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(73, 16);
		this.label9.TabIndex = 81;
		this.label9.Text = "Sale Order";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(11, 114);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(100, 16);
		this.label6.TabIndex = 79;
		this.label6.Text = "Plant Instruction";
		this.txtPlantInstruction.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPlantInstruction.Location = new System.Drawing.Point(145, 114);
		this.txtPlantInstruction.Name = "txtPlantInstruction";
		this.txtPlantInstruction.Size = new System.Drawing.Size(961, 20);
		this.txtPlantInstruction.TabIndex = 17;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(321, 35);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(64, 16);
		this.label3.TabIndex = 75;
		this.label3.Text = "Other Ref";
		this.txtOtherReference.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOtherReference.Location = new System.Drawing.Point(399, 33);
		this.txtOtherReference.Name = "txtOtherReference";
		this.txtOtherReference.Size = new System.Drawing.Size(171, 20);
		this.txtOtherReference.TabIndex = 5;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(11, 35);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(124, 16);
		this.label1.TabIndex = 73;
		this.label1.Text = "Mannual Report No";
		this.txtInvoiceReference.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtInvoiceReference.Location = new System.Drawing.Point(145, 33);
		this.txtInvoiceReference.Name = "txtInvoiceReference";
		this.txtInvoiceReference.Size = new System.Drawing.Size(171, 20);
		this.txtInvoiceReference.TabIndex = 4;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(859, 9);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(70, 16);
		this.label2.TabIndex = 71;
		this.label2.Text = "Plan Type";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(575, 9);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(92, 16);
		this.label8.TabIndex = 69;
		this.label8.Text = "Plant / Feader";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(321, 9);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(71, 16);
		this.label7.TabIndex = 67;
		this.label7.Text = "Plan Code";
		this.txtPlanCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPlanCode.Enabled = false;
		this.txtPlanCode.Location = new System.Drawing.Point(399, 7);
		this.txtPlanCode.Name = "txtPlanCode";
		this.txtPlanCode.Size = new System.Drawing.Size(171, 20);
		this.txtPlanCode.TabIndex = 1;
		this.txtPlanDate.CustomFormat = "dd-MMM-yy";
		this.txtPlanDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtPlanDate.Location = new System.Drawing.Point(145, 7);
		this.txtPlanDate.Name = "txtPlanDate";
		this.txtPlanDate.Size = new System.Drawing.Size(171, 20);
		this.txtPlanDate.TabIndex = 0;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(11, 9);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(37, 16);
		this.label5.TabIndex = 63;
		this.label5.Text = "Date";
		this.panel3.Controls.Add(this.toolStrip1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1234, 30);
		this.panel3.TabIndex = 0;
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[7] { this.toolStripButton2, this.btnnew, this.btnsave, this.btnUpdate, this.Print, this.btnattachment, this.btnRefresh });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1234, 27);
		this.toolStrip1.TabIndex = 57;
		this.toolStrip1.Text = "toolStrip1";
		this.toolStripButton2.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton2.Name = "toolStripButton2";
		this.toolStripButton2.Size = new System.Drawing.Size(200, 24);
		this.toolStripButton2.Text = "Production Job Order          ";
		this.btnnew.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = (System.Drawing.Image)resources.GetObject("btnnew.Image");
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(55, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnsave.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(55, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnUpdate.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(69, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.Print.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.Print.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.Print.Name = "Print";
		this.Print.Size = new System.Drawing.Size(56, 24);
		this.Print.Text = "&Print";
		this.Print.Click += new System.EventHandler(Print_Click);
		this.btnattachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnattachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnattachment.Name = "btnattachment";
		this.btnattachment.Size = new System.Drawing.Size(94, 24);
		this.btnattachment.Text = "&Attechment";
		this.btnattachment.Click += new System.EventHandler(btnattachment_Click);
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(50, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.tabPage2.Controls.Add(this.panel5);
		this.tabPage2.Controls.Add(this.panel12);
		this.tabPage2.Controls.Add(this.panel8);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1240, 723);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel5.Controls.Add(this.panel13);
		this.panel5.Controls.Add(this.panel11);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(3, 374);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1234, 346);
		this.panel5.TabIndex = 62;
		this.panel13.Controls.Add((System.Windows.Forms.Control)(object)this.grdOutPut);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel13.Location = new System.Drawing.Point(638, 0);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(711, 346);
		this.panel13.TabIndex = 1;
		this.grdOutPut.AllowDelete = (InheritableBoolean)1;
		grdOutPut_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdOutPut.DesignTimeLayout = grdOutPut_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdOutPut).Dock = System.Windows.Forms.DockStyle.Left;
		this.grdOutPut.DynamicFiltering = true;
		this.grdOutPut.FilterMode = (FilterMode)2;
		this.grdOutPut.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdOutPut).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdOutPut.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdOutPut).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdOutPut).Name = "grdOutPut";
		((ScrollableBase)this.grdOutPut).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdOutPut).Size = new System.Drawing.Size(706, 346);
		((System.Windows.Forms.Control)(object)this.grdOutPut).TabIndex = 62;
		this.grdOutPut.TotalRow = (InheritableBoolean)1;
		this.grdOutPut.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdOutPut).VisualStyle = (VisualStyle)4;
		this.panel11.Controls.Add((System.Windows.Forms.Control)(object)this.grdInput);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel11.Location = new System.Drawing.Point(0, 0);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(638, 346);
		this.panel11.TabIndex = 0;
		this.grdInput.AllowDelete = (InheritableBoolean)1;
		grdInput_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdInput.DesignTimeLayout = grdInput_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdInput).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdInput.DynamicFiltering = true;
		this.grdInput.FilterMode = (FilterMode)2;
		this.grdInput.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdInput).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdInput.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdInput).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdInput).Name = "grdInput";
		((ScrollableBase)this.grdInput).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdInput).Size = new System.Drawing.Size(638, 346);
		((System.Windows.Forms.Control)(object)this.grdInput).TabIndex = 62;
		this.grdInput.TotalRow = (InheritableBoolean)1;
		this.grdInput.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdInput).VisualStyle = (VisualStyle)4;
		this.panel12.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(3, 30);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1234, 344);
		this.panel12.TabIndex = 2;
		this.DataGridHistory.AllowDelete = (InheritableBoolean)1;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistory.DynamicFiltering = true;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)1;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1234, 344);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 61;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		this.DataGridHistory.LinkClicked += new ColumnActionEventHandler(DataGridHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).DoubleClick += new System.EventHandler(DataGridHistory_DoubleClick);
		this.panel8.Controls.Add(this.toolStrip2);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(3, 3);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1234, 27);
		this.panel8.TabIndex = 0;
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.toolStripButton3, this.btnNewHistory, this.toolStripSeparator1, this.toolStripSeparator2, this.btnloadall });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1234, 27);
		this.toolStrip2.TabIndex = 58;
		this.toolStrip2.Text = "toolStrip2";
		this.toolStripButton3.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton3.Name = "toolStripButton3";
		this.toolStripButton3.Size = new System.Drawing.Size(94, 24);
		this.toolStripButton3.Text = "History        ";
		this.btnNewHistory.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNewHistory.Image = (System.Drawing.Image)resources.GetObject("btnNewHistory.Image");
		this.btnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewHistory.Name = "btnNewHistory";
		this.btnNewHistory.Size = new System.Drawing.Size(55, 24);
		this.btnNewHistory.Text = "&New";
		this.btnNewHistory.Click += new System.EventHandler(btnNewHistory_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
		this.btnloadall.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnloadall.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnloadall.Name = "btnloadall";
		this.btnloadall.Size = new System.Drawing.Size(74, 24);
		this.btnloadall.Text = "Load All";
		this.btnloadall.Click += new System.EventHandler(btnloadall_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1248, 749);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmProductionJobOrder";
		this.Text = "frmProductionJobOrder";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(frmProductionJobOrder_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmProductionJobOrder_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.tabControl2.ResumeLayout(false);
		this.tabPage3.ResumeLayout(false);
		this.panel14.ResumeLayout(false);
		this.panel9.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPlannedOutput).EndInit();
		this.paneloutputgrd.ResumeLayout(false);
		this.paneloutputgrd.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbWareHouseOutPut).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCropYearOutput).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbJobLotOutput).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbinneruom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbouteruom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPackingType).EndInit();
		this.tabPage4.ResumeLayout(false);
		this.panel15.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdInputDetail).EndInit();
		this.panelinputgrd.ResumeLayout(false);
		this.panelinputgrd.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbPackTypeInput).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemInput).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbwarehouseinput).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcropyearinput).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbjoblotinput).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbpacksizeinput).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbentrytype).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDocNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbFiltersType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWipItem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbLotAc).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbByProductionAc).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbFinishGoodsAc).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWorkInProcess).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbStatus).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbProductionType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPlantFeader).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPlanType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSaleOrder).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.panel13.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdOutPut).EndInit();
		this.panel11.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdInput).EndInit();
		this.panel12.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
