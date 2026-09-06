// using Architecture.Reports;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.BLL.Production;
using Architecture.BLL.Reports.Production;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.Model.Production;
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
namespace Architecture.WinApp.Production
{

public class frmFoodProduction : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private Rightsobjects formrights = new Rightsobjects();

	private bool UpdateMode;

	private int RecId;

	private int RecOverHeadId;

	private int RecpackingMaterial;

	private int RecIdOutPut;

	private int VoucherHeadId;

	private int updateDetailIndex;

	private double AvgRate;

	private double FinishGoodsNetWeight;

	private double InputNetWeight;

	private double ByProductNetWeight;

	private double DiffNetWeight;

	private double ConsumedWeight;

	private double PreWeight;

	private DataTable dtCrop = new DataTable();

	private DataTable dtJob = new DataTable();

	private DataTable dtwarehouse = new DataTable();

	private DataTable dtpacktype = new DataTable();

	private DataTable dtitem = new DataTable();

	private DataTable table = new DataTable();

	private DataTable dtuomlst = new DataTable();

	private DataTable dtSummerReport = new DataTable();

	private DataTable tableByProduct = new DataTable();

	private DataTable dtEntryType = new DataTable();

	public string GRNS = string.Empty;

	public bool GRNSelected;

	private LoadInvoices LoadGRN;

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage3;

	private Panel panel18;

	private Panel panel21;

	private Panel panel19;

	private ToolStrip toolStrip3;

	private ToolStripButton toolStripButton1;

	private ToolStripSeparator toolStripSeparator2;

	private TabPage InPut;

	private Panel panel5;

	internal GridEX grdStockConversionProductionDetail;

	private Panel panel4;

	private Button btnCancelDetail;

	private Button btnUpdateDetail;

	private TextBox txtdeailRemarks;

	private Label label21;

	internal UltraCombo cmbUOM;

	private TextBox txtNetweight;

	private Label label32;

	internal UltraCombo cmbGodown;

	internal UltraCombo cmbLot;

	internal UltraCombo cmbItem;

	internal UltraCombo CmbCropyr;

	private Button Add;

	private DateTimePicker txtDocdate;

	private Label label2;

	private Label label1;

	private Label label6;

	private Label label44;

	private Label label7;

	private TextBox txtdocnumber;

	private TextBox txtQty;

	private TextBox txtRemarks;

	private Label label18;

	private Label label5;

	private Label label12;

	internal UltraCombo cmbBagType;

	private Label label14;

	private Label label10;

	private Label label8;

	private Panel panel2;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnsave;

	private ToolStripButton btnUpdate;

	private ToolStripSeparator toolStripSeparator;

	private ToolStripButton Print;

	private TabPage tabPage2;

	private Panel panel10;

	private Panel panel13;

	private Panel panel17;

	internal GridEX grdDetail;

	private Panel panel16;

	internal GridEX DatagridHistory;

	private Panel panel15;

	private Button btnSearch;

	private Label label50;

	private TextBox txtDocNumberTo;

	private DateTimePicker txtToDate;

	private Label label52;

	private DateTimePicker txtfromDate;

	private Label label48;

	private Label label49;

	private TextBox txtDocNoFrom;

	private Panel panel11;

	private ToolStrip toolStrip2;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripButton toolStripButton4;

	private TabPage PackingMaterial;

	private Panel panel7;

	private Panel panel24;

	private Label label55;

	private Label label41;

	private TextBox txtPackingMaterialRate;

	private TextBox txtPackingMaterialQTY;

	internal UltraCombo cmbUOMPackingMaterial;

	private Label label42;

	internal UltraCombo cmbItemPackingMaterial;

	private Label label40;

	private DateTimePicker txtdocdatePackingMaterial;

	private Label label34;

	private Label label37;

	private TextBox txtDocNoPackingMaterial;

	private Panel panel22;

	private ToolStrip toolStrip4;

	private ToolStripButton btnNewpackingmaterial;

	private ToolStripButton btnsavepackingmaterial;

	private ToolStripButton btnUpdatepackingmaterial;

	private ToolStripSeparator toolStripSeparator3;

	private ToolStripButton toolStripButton9;

	private TabPage OverHead;

	private Panel panel8;

	private Panel panel27;

	private Label label58;

	private TextBox txtLedgerRemarksOverHead;

	internal UltraCombo cmbChartofAccountOverHead;

	private Label label59;

	private Label label57;

	private DateTimePicker txtDocDateOverHead;

	private Label label38;

	private Label label56;

	private TextBox txtDocNoOverHead;

	private TextBox txtExpAmountOverHead;

	private Panel panel25;

	private ToolStrip toolStrip5;

	private ToolStripButton btnNewOverHead;

	private ToolStripButton btnSaveOverHead;

	private ToolStripButton btnUpdateOverHead;

	private ToolStripSeparator toolStripSeparator4;

	private ToolStripButton btnPrintOverHead;

	private Label label63;

	private TextBox txtremarksPackingMaterial;

	private Label label62;

	private TextBox txtAmountPackingMaterial;

	private GroupBox groupBox2;

	private Panel panel9;

	internal GridEX grdPackingMaterial;

	internal GridEX grdOverHead;

	internal UltraCombo cmbJobOrderPackingMaterial;

	internal UltraCombo cmbJobOrderOverHead;

	private Label label35;

	private Label label39;

	internal UltraCombo cmbsummeryJobOrderNo;

	private Label label68;

	private ToolStripButton toolStripButton2;

	private ToolStripButton toolStripButton3;

	private ToolStripButton toolStripButton5;

	private ToolStripButton toolStripButton6;

	private ToolStripButton toolStripButton7;

	private CtrlGrdBar ctrlGrdBar1;

	private CtrlGrdBar ctrlGrdBar2;

	private TabPage OutPut;

	private Panel panel3;

	private Panel panel12;

	private Button btnCancelOutPut;

	private Button btnUpdateDetailOutPut;

	private TextBox txtRemarksOutPut;

	private Label label9;

	internal UltraCombo CmbUomOutPut;

	private TextBox txtNetWeightOutPut;

	private Label label11;

	private Label label13;

	internal UltraCombo CmbWarehouseOutput;

	internal UltraCombo CmbLotOutPut;

	internal UltraCombo CmbItemOutput;

	internal UltraCombo CmbCropYearOutPut;

	private Button btnAddOutPut;

	private DateTimePicker DocDateOutPut;

	private Label label15;

	private Label label16;

	private Label label19;

	private Label label20;

	private Label label24;

	private TextBox txtDocNoOutPut;

	private TextBox txtQtyOutPut;

	private TextBox txtRemarksHeaderOutPut;

	private Label label36;

	private Label label43;

	private Label label45;

	internal UltraCombo CmbPackTypeOutPut;

	private Label label69;

	private Label label70;

	private Panel panel6;

	private ToolStrip toolStrip6;

	private ToolStripButton toolStripButton10;

	private ToolStripButton btnSaveOutPut;

	private ToolStripButton btnUpdateOutPut;

	private ToolStripSeparator toolStripSeparator5;

	private ToolStripButton btnPrintOutPut;

	private Panel panel14;

	internal GridEX grdByProduct;

	private Button btnLoadInvoices;

	private Timer timer1;

	private Label label23;

	internal UltraCombo CmbJobOrderNo;

	private TextBox txtAmountOutPut;

	private Label lblAmountOUtPUt;

	internal UltraCombo CmbRateUomOutPut;

	private Label lblRateUOMOUtPut;

	private TextBox txtRateOutPut;

	private Label lblRateOutPUt;

	private Label label60;

	internal UltraCombo CmbJobOrderNoOutput;

	private TextBox txtAmountInput;

	private Label lblAmountInput;

	internal UltraCombo CmbRateUOMInput;

	private Label lblRateUOMInput;

	private TextBox txtRateInput;

	private Label lblRateInput;

	private TextBox txtAvgRate;

	private Label label25;

	private TextBox txtAvgStock;

	private Label label22;

	internal UltraCombo CmbEntryType;

	private Label label27;

	internal UltraCombo CmbWorkInProcess;

	private Label label33;

	internal UltraCombo CmbStockAccount;

	private Label label77;

	internal UltraCombo CmbWIPItem;

	private ToolStripButton btnVoucherReport;

	private ToolStripButton btnVoucherOutPut;

	private ToolStripButton btnVoucherPM;

	private ToolStripButton btnVoucherOH;

	private TextBox txtFGWeight;

	private Label label75;

	private TextBox txtFGRate;

	private Label label76;

	internal GridEX grdSummery;

	private Button btnApplyRate;

	private ToolStripButton btnProductionRecoveryReport;

	private Panel panel26;

	private Panel panel23;

	private Panel panel20;

	internal GridEX grdSummeryReport;

	private ToolStripLabel toolStripButton8;

	private Panel panel29;

	private Panel panel28;

	private Panel panel30;

	private Panel panel31;

	private CtrlGrdBar ctrlGrdBar3;

	private CtrlGrdBar ctrlGrdBar4;

	private CtrlGrdBar ctrlGrdBar5;

	private CtrlGrdBar ctrlGrdBar6;

	public frmFoodProduction(UserAccount user)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void frmFoodProduction_Load(object sender, EventArgs e)
	{
		try
		{
			formrights = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnsave.Enabled = formrights.DoHaveSaveRight;
			btnSaveOutPut.Enabled = formrights.DoHaveSaveRight;
			btnUpdate.Enabled = formrights.DoHaveUpdateRights;
			btnUpdateOutPut.Enabled = formrights.DoHaveUpdateRights;
			Print.Enabled = formrights.DoHavePrintRights;
			btnVoucherReport.Enabled = formrights.DoHavePrintRights;
			btnVoucherOutPut.Enabled = formrights.DoHavePrintRights;
			btnPrintOutPut.Enabled = formrights.DoHavePrintRights;
			ctrlGrdBar3.mGridSaveLayouts.Enabled = formrights.DoHaveSaveLayoutRights;
			ctrlGrdBar3.mGridPrint.Enabled = formrights.DoHaveGridPrintRights;
			ctrlGrdBar3.mGridExport.Enabled = formrights.DoHaveGridExportRights;
			ctrlGrdBar3.GroupCollaps.Enabled = formrights.DoHaveGridGroupCollapseRights;
			ctrlGrdBar3.GroupExpand.Enabled = formrights.DoHaveGridGroupExpandRights;
			ctrlGrdBar4.mGridSaveLayouts.Enabled = formrights.DoHaveSaveLayoutRights;
			ctrlGrdBar4.mGridPrint.Enabled = formrights.DoHaveGridPrintRights;
			ctrlGrdBar4.mGridExport.Enabled = formrights.DoHaveGridExportRights;
			ctrlGrdBar4.GroupCollaps.Enabled = formrights.DoHaveGridGroupCollapseRights;
			ctrlGrdBar4.GroupExpand.Enabled = formrights.DoHaveGridGroupExpandRights;
			if (!formrights.DoHaveCanRateandAmount)
			{
				ctrlGrdBar3.mGridChooseFielder.Visible = false;
				ctrlGrdBar4.mGridChooseFielder.Visible = false;
			}
			dtitem.Columns.Add("Id");
			dtitem.Columns.Add("ItemName");
			dtCrop.Columns.Add("Id");
			dtCrop.Columns.Add("CropYear");
			dtJob.Columns.Add("Id");
			dtJob.Columns.Add("JobLotDescription");
			dtwarehouse.Columns.Add("Id");
			dtwarehouse.Columns.Add("WareHouseName");
			dtpacktype.Columns.Add("Id");
			dtpacktype.Columns.Add("PackingType");
			table.Columns.Add("Id");
			table.Columns.Add("DocDate");
			table.Columns.Add("DocNo");
			table.Columns.Add("BillNo");
			table.Columns.Add("WareHouseId");
			table.Columns.Add("WareHouse");
			table.Columns.Add("ItemId");
			table.Columns.Add("Item");
			table.Columns.Add("ItemUOMId");
			table.Columns.Add("ItemUOM");
			table.Columns.Add("CropYearId");
			table.Columns.Add("CropYear");
			table.Columns.Add("JobLotId");
			table.Columns.Add("JobLot");
			table.Columns.Add("PackingTypeId");
			table.Columns.Add("PackingType");
			table.Columns.Add("Quantity", typeof(double));
			table.Columns.Add("Weight", typeof(double));
			table.Columns.Add("Rate");
			table.Columns.Add("RateUOMId");
			table.Columns.Add("RateUOM");
			table.Columns.Add("ItemAmount", typeof(double));
			table.Columns.Add("Remarks");
			tableByProduct.Columns.Add("Id");
			tableByProduct.Columns.Add("EntryType");
			tableByProduct.Columns.Add("WareHouseId");
			tableByProduct.Columns.Add("WareHouse");
			tableByProduct.Columns.Add("ItemId");
			tableByProduct.Columns.Add("Item");
			tableByProduct.Columns.Add("ItemUOMId");
			tableByProduct.Columns.Add("ItemUOM");
			tableByProduct.Columns.Add("CropYearId");
			tableByProduct.Columns.Add("CropYear");
			tableByProduct.Columns.Add("JobLotId");
			tableByProduct.Columns.Add("JobLot");
			tableByProduct.Columns.Add("PackingTypeId");
			tableByProduct.Columns.Add("PackingType");
			tableByProduct.Columns.Add("Quantity", typeof(double));
			tableByProduct.Columns.Add("Weight", typeof(double));
			tableByProduct.Columns.Add("Rate");
			tableByProduct.Columns.Add("RateUOMId");
			tableByProduct.Columns.Add("RateUOM");
			tableByProduct.Columns.Add("ItemAmount", typeof(double));
			tableByProduct.Columns.Add("Remarks");
			tableByProduct.Columns.Add("StockAcId");
			tableByProduct.Columns.Add("StockAc");
			EntryTypeFill();
			stockAccoutnFill();
			UOMFill();
			ItemFill();
			combojoblotfill();
			CropYear();
			Warehouse();
			bagType();
			JobOrderNoFill();
			GenerateDocNumberInput();
			GenerateDocNumberOutPut();
			btnUpdateDetail.Visible = false;
			btnCancelDetail.Visible = false;
			btnUpdateDetailOutPut.Visible = false;
			btnCancelOutPut.Visible = false;
			ItemFillPackingMaterial();
			ChartOfAccountOverHead();
			GenerateCodeofOverHead();
			GenerateCodeOfPackingMaterial();
			BindGrid();
			BindGridOfOverHead();
			if (!formrights.DoHaveCanRateandAmount)
			{
				txtRateInput.Visible = false;
				((Control)(object)CmbRateUOMInput).Visible = false;
				txtAmountInput.Visible = false;
				lblRateInput.Visible = false;
				lblRateUOMInput.Visible = false;
				lblAmountInput.Visible = false;
				txtdeailRemarks.Width = 750;
				txtRemarksOutPut.Width = 680;
				txtRateOutPut.Visible = false;
				((Control)(object)CmbRateUomOutPut).Visible = false;
				txtAmountOutPut.Visible = false;
				lblRateOutPUt.Visible = false;
				lblRateUOMOUtPut.Visible = false;
				lblAmountOUtPUt.Visible = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateDocNumberInput()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected O, but got Unknown
		try
		{
			int code = InvFoodProduction.GetSerialNumber(new InvFoodProduction
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = 80,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			if (code > 0)
			{
				txtdocnumber.Text = code.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateDocNumberOutPut()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected O, but got Unknown
		try
		{
			int code = InvFoodProduction.GetSerialNumber(new InvFoodProduction
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = 112,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			if (code > 0)
			{
				txtDocNoOutPut.Text = code.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Warehouse()
	{
		try
		{
			DataTable dtware = CommonServices.getActiveWareHouse();
			dtwarehouse.Clear();
			for (int i = 0; i < dtware.Rows.Count; i++)
			{
				dtwarehouse.Rows.Add(dtware.Rows[i]["Id"], dtware.Rows[i]["WareHouseName"]);
			}
			if (dtwarehouse.Rows.Count > 0)
			{
				DDL.BindDDL(dtwarehouse, cmbGodown, "Id", "WareHouseName", "Issue From ", ZeroIndex: false);
				DDL.BindDDL(dtwarehouse, CmbWarehouseOutput, "Id", "WareHouseName", "Issue From ", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void stockAccoutnFill()
	{
		try
		{
			DataTable dtCoa = CommonServices.CoaAllocationGetForComboServiceBind();
			if (dtCoa.Rows.Count > 0)
			{
				DDL.BindDDL(dtCoa, CmbStockAccount, "Id", "AccountTitle", "Stock Account ", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combojoblotfill()
	{
		try
		{
			DataTable dtjoblot = CommonServices.JobLotGetAllService();
			dtJob.Clear();
			for (int i = 0; i < dtjoblot.Rows.Count; i++)
			{
				dtJob.Rows.Add(dtjoblot.Rows[i]["Id"], dtjoblot.Rows[i]["JobLotDescription"]);
			}
			if (dtJob.Rows.Count > 0)
			{
				DDL.BindDDL(dtJob, cmbLot, "Id", "JobLotDescription", "JobLot Description", ZeroIndex: false);
				DDL.BindDDL(dtJob, CmbLotOutPut, "Id", "JobLotDescription", "JobLot Description", ZeroIndex: false);
			}
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
			dtCrop = CommonServices.CropYearGetAllService();
			if (dtCrop.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtCrop, CmbCropyr, "Id", "CropYear", "Crop Year", false);
				DDL.BindDDLNew(dtCrop, CmbCropYearOutPut, "Id", "CropYear", "Crop Year", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void bagType()
	{
		try
		{
			DataTable dtpackingtype = new DataTable();
			dtpackingtype = InvPackingType.Getall();
			dtpacktype.Clear();
			for (int i = 0; i < dtpackingtype.Rows.Count; i++)
			{
				dtpacktype.Rows.Add(dtpackingtype.Rows[i]["Id"], dtpackingtype.Rows[i]["PackTypeDesc"]);
			}
			if (dtpacktype.Rows.Count > 0)
			{
				DDL.BindDDL(dtpacktype, cmbBagType, "Id", "PackingType", "Packing Type", ZeroIndex: false);
				DDL.BindDDL(dtpacktype, CmbPackTypeOutPut, "Id", "PackingType", "Packing Type", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable lst = Item.ReadAllItems(new Item
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			dtitem.Rows.Clear();
			if (lst.Rows.Count > 0)
			{
				DDL.BindDDL(lst, cmbItem, "Id", "ItemName", "ItemCode", ZeroIndex: false);
				DDL.BindDDL(lst, CmbItemOutput, "Id", "ItemName", "ItemCode", ZeroIndex: false);
				((UltraGridBase)cmbItem).DisplayLayout.Bands[0].Columns[1].Width = 400;
				((UltraGridBase)CmbItemOutput).DisplayLayout.Bands[0].Columns[1].Width = 400;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void JobOrderNoFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		try
		{
			ReportsParameters val = new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = 401,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			};
			DataTable dtPJO = new DataTable();
			dtPJO = InvProductionJobOrder.GetJobOrderNoForInvFoodProduction(val);
			if (dtPJO.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("PlanCode");
				for (int i = 0; i < dtPJO.Rows.Count; i++)
				{
					dt.Rows.Add(dtPJO.Rows[i]["Id"], dtPJO.Rows[i]["PlanCode"]);
				}
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, cmbJobOrderPackingMaterial, "Id", "PlanCode", "JobOrderNo", ZeroIndex: false);
					DDL.BindDDL(dt, cmbJobOrderOverHead, "Id", "PlanCode", "JobOrderNo", ZeroIndex: false);
					DDL.BindDDL(dt, CmbJobOrderNo, "Id", "PlanCode", "JobOrderNo", ZeroIndex: false);
					DDL.BindDDL(dt, CmbJobOrderNoOutput, "Id", "PlanCode", "JobOrderNo", ZeroIndex: false);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void JobOrderNoFillForSummery()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		try
		{
			ReportsParameters val = new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			};
			DataTable dtPJO = new DataTable();
			dtPJO = InvProductionJobOrder.GetJobOrderAll(val);
			if (dtPJO.Rows.Count > 0)
			{
				DDL.BindDDL(dtPJO, cmbsummeryJobOrderNo, "Id", "PlanCode", "JobOrderNo", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UOMFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			UOMSchedule obj = new UOMSchedule();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			dtuomlst = UOMSchedule.Getall(obj);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void bindUomInput()
	{
		try
		{
			if (dtuomlst.Rows.Count <= 0)
			{
				return;
			}
			DataTable dtUom = new DataTable();
			dtUom.Columns.Add("Id");
			dtUom.Columns.Add("Equivalent");
			for (int i = 0; i < dtuomlst.Rows.Count; i++)
			{
				if (Convert.ToInt32(dtuomlst.Rows[i]["ItemId"]) == Convert.ToInt32(cmbItem.Value))
				{
					dtUom.Rows.Add(dtuomlst.Rows[i]["Id"], dtuomlst.Rows[i]["Equivalent"]);
				}
			}
			if (dtUom.Rows.Count > 0)
			{
				DDL.BindDDL(dtUom, cmbUOM, "Id", "Equivalent", "UOM", ZeroIndex: false);
				DDL.BindDDL(dtUom, CmbRateUOMInput, "Id", "Equivalent", "UOM", ZeroIndex: false);
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
			bindUomInput();
			GetAvgRate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetAvgRate()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Expected O, but got Unknown
		try
		{
			ReportsParameters val = new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				ItemId = Conversion.ToInt(cmbItem.Value),
				CropYear = ((Control)(object)CmbCropyr).Text.ToString(),
				JobLotId = Conversion.ToInt(cmbLot.Value),
				ToDate = txtDocdate.Value
			};
			DataTable dtang = new DataTable();
			dtang = InvPurchaseInvoice.GetInventoryStockEvalautionDetail_GetAvgRates(val);
			if (dtang.Rows.Count > 0)
			{
				txtRateInput.Text = Math.Round(Convert.ToDouble(dtang.Rows[0]["AvgRate"]), 2).ToString();
				txtAvgRate.Text = Math.Round(Convert.ToDouble(dtang.Rows[0]["AvgRate"]), 2).ToString();
				txtAvgStock.Text = Math.Round(Convert.ToDouble(dtang.Rows[0]["rBalWeight"]), 2).ToString();
			}
			else
			{
				txtRateInput.Text = "0";
				txtAvgRate.Text = "0";
				txtAvgStock.Text = "0";
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
			dtEntryType.Columns.Add("Id");
			dtEntryType.Columns.Add("Name");
			dtEntryType.Rows.Add(1, "ByProduct");
			dtEntryType.Rows.Add(2, "FinishGoods");
			DDL.BindDDL(dtEntryType, CmbEntryType, "Id", "Name", "EntryType", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		reset();
		resetDetail();
	}

	private void InsertInput()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Expected O, but got Unknown
		try
		{
			if (!FormValidation())
			{
				return;
			}
			InvFoodProduction invFoodProduction = new InvFoodProduction();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				invFoodProduction.Id = RecId;
			}
			else
			{
				if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				GenerateDocNumberInput();
			}
			invFoodProduction.RefDocumentTypeId = 1;
			invFoodProduction.DocumentTypeId = 80;
			invFoodProduction.DocCode = Conversion.ToInt((object)txtdocnumber.Text);
			invFoodProduction.DocDate = Conversion.ToDateTime((object)txtDocdate.Value);
			invFoodProduction.MainRemarks = txtRemarks.Text.ToString();
			invFoodProduction.EntryType = "Input";
			invFoodProduction.InvFoodProductionPlanId = 0;
			invFoodProduction.WIPAccountId = Conversion.ToInt(CmbWorkInProcess.Value);
			invFoodProduction.WIPItemId = Conversion.ToInt(CmbWIPItem.Value);
			invFoodProduction.InvJobOrderId = Conversion.ToInt(CmbJobOrderNo.Value);
			invFoodProduction.InvJobOrderNo = Conversion.ToString((object)((Control)(object)CmbJobOrderNo).Text.Trim());
			invFoodProduction.OrganizationId = UserAccount.OrganizationId;
			invFoodProduction.CompanyId = UserAccount.CompanyId;
			invFoodProduction.EntryDate = DateTime.Now;
			invFoodProduction.ModifyDate = DateTime.Now;
			invFoodProduction.EntryUser = UserAccount.ID;
			invFoodProduction.ModifyUser = UserAccount.ID;
			invFoodProduction.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			invFoodProduction.invFoodProductionDetails = new List<InvFoodProductionDetail>();
			if (grdStockConversionProductionDetail.RowCount > 0)
			{
				GridEXRow[] rows = grdStockConversionProductionDetail.GetRows();
				foreach (GridEXRow r in rows)
				{
					if (Conversion.ToDouble((object)r.Cells["Quantity"].Text) > 0.0 && Conversion.ToDouble((object)r.Cells["Weight"].Text) > 0.0)
					{
						InvFoodProductionDetail Conversiondetail = new InvFoodProductionDetail();
						Conversiondetail.EntryType = Conversion.ToString((object)"Issue");
						Conversiondetail.RefDocNoId = Conversion.ToInt(r.Cells["Id"].Value);
						Conversiondetail.Id = Conversion.ToInt(r.Cells["Id"].Value);
						if (r.Cells["WareHouseId"].Text == string.Empty || r.Cells["WareHouseId"].Text == "0")
						{
							MessageBox.Show("WareHouse Field Require in Input Detail");
							return;
						}
						Conversiondetail.WarehouseId = Conversion.ToInt(r.Cells["WareHouseId"].Value);
						if (r.Cells["ItemId"].Text == string.Empty || r.Cells["ItemId"].Text == "0")
						{
							MessageBox.Show("Item Field Require in Input Detail");
							return;
						}
						Conversiondetail.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
						Conversiondetail.ItemUomId = Conversion.ToInt(r.Cells["ItemUOMId"].Value);
						if (r.Cells["CropYear"].Text == string.Empty || r.Cells["CropYear"].Text == "0")
						{
							MessageBox.Show("CropYear Field Require in Input Detail");
							return;
						}
						Conversiondetail.CropBatch = Conversion.ToString((object)r.Cells["CropYear"].Text);
						if (r.Cells["JobLotId"].Text == string.Empty)
						{
							MessageBox.Show("JobLot Field Require in Input Detail");
							return;
						}
						Conversiondetail.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
						Conversiondetail.PackingtypeId = Conversion.ToInt(r.Cells["PackingTypeId"].Value);
						Conversiondetail.Qty = Conversion.ToDouble((object)r.Cells["Quantity"].Text);
						Conversiondetail.Weight = Conversion.ToDouble(r.Cells["Weight"].Value);
						Conversiondetail.Rate = Conversion.ToDouble(r.Cells["Rate"].Value);
						Conversiondetail.InvProductionJobOrderId = Conversion.ToInt(CmbJobOrderNo.Value);
						Conversiondetail.Amount = Conversion.ToInt(r.Cells["ItemAmount"].Value);
						Conversiondetail.RateUOMId = Conversion.ToInt(r.Cells["RateUOMId"].Value);
						Conversiondetail.VoucherHeadId = 0;
						Conversiondetail.Remarks = Conversion.ToString((object)r.Cells["Remarks"].Text);
						invFoodProduction.invFoodProductionDetails.Add(Conversiondetail);
					}
				}
				if (invFoodProduction.invFoodProductionDetails.Count == 0)
				{
					throw new Exception("Item Qty and Weight Required");
				}
				InvFoodProduction.Save(invFoodProduction);
				if (RecId > 0)
				{
					MessageBox.Show("Record Update Successfully" + invFoodProduction.DocCode);
				}
				else
				{
					MessageBox.Show("Record Save Successfully" + invFoodProduction.DocCode);
				}
				reset();
				resetDetail();
			}
			else
			{
				MessageBox.Show("Enter Detail First ...");
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
			RecId = 0;
			InsertInput();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Add_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValidationOfDetailPortion())
			{
				double amount = 0.0;
				double Qty = 0.0;
				double Rate = 0.0;
				double Weight = 0.0;
				if (txtAmountInput.Text != string.Empty)
				{
					amount = Conversion.ToDouble((object)txtAmountInput.Text.Trim());
				}
				if (txtRateInput.Text != string.Empty)
				{
					Rate = Conversion.ToDouble((object)txtRateInput.Text.Trim());
				}
				if (txtQty.Text != string.Empty)
				{
					Qty = Conversion.ToDouble((object)txtQty.Text.Trim());
				}
				if (txtNetweight.Text != string.Empty)
				{
					Weight = Conversion.ToDouble((object)txtNetweight.Text.Trim());
				}
				table.Rows.Add(0, "", 0, 0, cmbGodown.Value, ((Control)(object)cmbGodown).Text, cmbItem.Value, ((Control)(object)cmbItem).Text, cmbUOM.Value, ((Control)(object)cmbUOM).Text, ((Control)(object)CmbCropyr).Text, cmbLot.Value, ((Control)(object)cmbLot).Text, cmbBagType.Value, ((Control)(object)cmbBagType).Text, Qty, Weight, Rate, CmbRateUOMInput.Value, Conversion.ToDouble((object)((Control)(object)CmbRateUOMInput).Text), amount, txtdeailRemarks.Text);
				grdStockConversionProductionDetail.DataSource = table;
				grdStockConversionProductionDetail.RetrieveStructure();
				resetDetail();
				gridsettings();
				cmbGodown.Focus();
			}
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
			InsertInput();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ReadByIdInput(int ID)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Expected O, but got Unknown
		try
		{
			btnsave.Visible = false;
			btnUpdate.Visible = true;
			InvFoodProduction SC = new InvFoodProduction();
			SC = InvFoodProduction.GetByID(ID);
			if (SC.EntryType == "Input")
			{
				txtdocnumber.Text = Conversion.ToString((object)SC.DocCode);
				txtDocdate.Value = Conversion.ToDateTime((object)SC.DocDate);
				txtRemarks.Text = Conversion.ToString((object)SC.MainRemarks);
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("JobOrderNo");
				dt.Rows.Add(SC.InvJobOrderId, SC.InvJobOrderNo);
				DDL.BindDDL(dt, CmbJobOrderNo, "Id", "JobOrderNo", "JobOrderNo", ZeroIndex: false);
				((UltraGridBase)CmbJobOrderNo).Rows[0].Activate();
				DataTable dtac = InvProductionJobOrder.GetGlAccountsByJobOrderId(new InvProductionJobOrder
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					Id = Conversion.ToInt(CmbJobOrderNo.Value)
				});
				if (dtac.Rows.Count > 0)
				{
					DDL.BindDDLNew(dtac, CmbWorkInProcess, "WorkInProccessAcId", "WorkInProcessAc", "WorkInProcessAc", false);
					DDL.BindDDLNew(dtac, CmbWIPItem, "WipItemId", "ItemName", "ItemName", false);
					((UltraGridBase)CmbWorkInProcess).Rows[0].Activate();
					((UltraGridBase)CmbWIPItem).Rows[0].Activate();
				}
			}
			table.Rows.Clear();
			for (int i = 0; i < SC.invFoodProductionDetails.Count; i++)
			{
				if (SC.invFoodProductionDetails[i].EntryType == "Issue")
				{
					tabControl1.SelectedIndex = 0;
					table.Rows.Add(SC.invFoodProductionDetails[i].Id, Convert.ToDateTime(SC.invFoodProductionDetails[i].DocDate).ToString("dd-MMM-yy"), SC.invFoodProductionDetails[i].DocNo, SC.invFoodProductionDetails[i].ManualBillNo, SC.invFoodProductionDetails[i].WarehouseId, SC.invFoodProductionDetails[i].WareHouseName, SC.invFoodProductionDetails[i].ItemId, SC.invFoodProductionDetails[i].ItemName, SC.invFoodProductionDetails[i].ItemUomId, SC.invFoodProductionDetails[i].Equivalent, SC.invFoodProductionDetails[i].CropBatch, SC.invFoodProductionDetails[i].CropBatch, SC.invFoodProductionDetails[i].JobLotId, SC.invFoodProductionDetails[i].JobLotDescription, SC.invFoodProductionDetails[i].PackingtypeId, SC.invFoodProductionDetails[i].PackTypeDesc, SC.invFoodProductionDetails[i].Qty, SC.invFoodProductionDetails[i].Weight, Math.Round(SC.invFoodProductionDetails[i].Rate, 2), SC.invFoodProductionDetails[i].RateUOMId, SC.invFoodProductionDetails[i].RateUOM, SC.invFoodProductionDetails[i].Amount, SC.invFoodProductionDetails[i].Remarks);
				}
				grdStockConversionProductionDetail.DataSource = table;
				grdStockConversionProductionDetail.RetrieveStructure();
				gridsettings();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ReadByIdOutPut(int ID)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Expected O, but got Unknown
		try
		{
			btnSaveOutPut.Visible = false;
			btnUpdateOutPut.Visible = true;
			InvFoodProduction SC = new InvFoodProduction();
			SC = InvFoodProduction.GetByID(ID);
			if (SC.EntryType != "Input")
			{
				txtDocNoOutPut.Text = Conversion.ToString((object)SC.DocCode);
				DocDateOutPut.Value = Conversion.ToDateTime((object)SC.DocDate);
				txtRemarksHeaderOutPut.Text = Conversion.ToString((object)SC.MainRemarks);
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("JobOrderNo");
				dt.Rows.Add(SC.InvJobOrderId, SC.InvJobOrderNo);
				DDL.BindDDL(dt, CmbJobOrderNoOutput, "Id", "JobOrderNo", "JobOrderNo", ZeroIndex: false);
				((UltraGridBase)CmbJobOrderNoOutput).Rows[0].Activate();
				DataTable dtWIP = InvProductionJobOrder.GetGlAccountsByJobOrderId(new InvProductionJobOrder
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					Id = Conversion.ToInt(CmbJobOrderNoOutput.Value)
				});
				if (dtWIP.Rows.Count > 0)
				{
					DDL.BindDDLNew(dtWIP, CmbWorkInProcess, "WorkInProccessAcId", "WorkInProcessAc", "WorkInProcessAc", false);
					DDL.BindDDLNew(dtWIP, CmbWIPItem, "WipItemId", "ItemName", "ItemName", false);
					((UltraGridBase)CmbWorkInProcess).Rows[0].Activate();
					((UltraGridBase)CmbWIPItem).Rows[0].Activate();
				}
			}
			tableByProduct.Clear();
			for (int i = 0; i < SC.invFoodProductionDetails.Count; i++)
			{
				if (SC.invFoodProductionDetails[i].EntryType != "Issue")
				{
					tabControl1.SelectedIndex = 1;
					tableByProduct.Rows.Add(SC.invFoodProductionDetails[i].Id, SC.invFoodProductionDetails[i].EntryType, SC.invFoodProductionDetails[i].WarehouseId, SC.invFoodProductionDetails[i].WareHouseName, SC.invFoodProductionDetails[i].ItemId, SC.invFoodProductionDetails[i].ItemName, SC.invFoodProductionDetails[i].ItemUomId, SC.invFoodProductionDetails[i].Equivalent, SC.invFoodProductionDetails[i].CropBatch, SC.invFoodProductionDetails[i].CropBatch, SC.invFoodProductionDetails[i].JobLotId, SC.invFoodProductionDetails[i].JobLotDescription, SC.invFoodProductionDetails[i].PackingtypeId, SC.invFoodProductionDetails[i].PackTypeDesc, SC.invFoodProductionDetails[i].Qty, SC.invFoodProductionDetails[i].Weight, Math.Round(SC.invFoodProductionDetails[i].Rate, 2), SC.invFoodProductionDetails[i].RateUOMId, SC.invFoodProductionDetails[i].RateUOM, SC.invFoodProductionDetails[i].Amount, SC.invFoodProductionDetails[i].Remarks, SC.invFoodProductionDetails[i].StockAcId, SC.invFoodProductionDetails[i].AccountTitle);
				}
				grdByProduct.DataSource = tableByProduct;
				grdByProduct.RetrieveStructure();
				gridsettingsOutPut();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (txtdocnumber.Text.Trim() == string.Empty || txtdocnumber.Text == "0")
		{
			MessageBox.Show("document Number Field Required");
			txtdocnumber.Focus();
			return false;
		}
		if (((UltraGridBase)CmbJobOrderNo).ActiveRow == null)
		{
			MessageBox.Show("Job Order Number Field Required");
			CmbJobOrderNo.Focus();
			return false;
		}
		if (((UltraGridBase)CmbWIPItem).ActiveRow == null)
		{
			MessageBox.Show("WIP Item Field Required");
			CmbWIPItem.Focus();
			return false;
		}
		return true;
	}

	private bool FormValidationOutPut()
	{
		if (txtDocNoOutPut.Text.Trim() == string.Empty || txtDocNoOutPut.Text == "0")
		{
			MessageBox.Show("DocNo Field Required");
			txtDocNoOutPut.Focus();
			return false;
		}
		if (((UltraGridBase)CmbJobOrderNoOutput).ActiveRow == null)
		{
			MessageBox.Show("Job Order Number Field Required");
			CmbJobOrderNoOutput.Focus();
			return false;
		}
		if (((UltraGridBase)CmbWIPItem).ActiveRow == null)
		{
			MessageBox.Show("WIP Item Field Required");
			CmbWIPItem.Focus();
			return false;
		}
		return true;
	}

	private bool FormValidationOfDetailPortion()
	{
		if (((UltraGridBase)cmbGodown).ActiveRow == null)
		{
			MessageBox.Show("Ware house Field Required");
			cmbGodown.Focus();
			return false;
		}
		if (((UltraGridBase)cmbItem).ActiveRow == null)
		{
			MessageBox.Show("Item Field Field Required");
			cmbItem.Focus();
			return false;
		}
		if (((UltraGridBase)cmbUOM).ActiveRow == null)
		{
			MessageBox.Show("UOM Field Required");
			cmbUOM.Focus();
			return false;
		}
		if (((UltraGridBase)cmbBagType).ActiveRow == null)
		{
			MessageBox.Show("Bag Type Field Required");
			cmbBagType.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtQty.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Bag Quantity Field Required");
			txtQty.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtNetweight.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Net Weight Field Required");
			txtNetweight.Focus();
			return false;
		}
		return true;
	}

	private bool FormValidationOfDetailPortionOutPut()
	{
		if (((UltraGridBase)CmbWarehouseOutput).ActiveRow == null)
		{
			MessageBox.Show("Ware house Field Required");
			CmbWarehouseOutput.Focus();
			return false;
		}
		if (((UltraGridBase)CmbItemOutput).ActiveRow == null)
		{
			MessageBox.Show("Item Field Field Required");
			CmbItemOutput.Focus();
			return false;
		}
		if (((UltraGridBase)CmbUomOutPut).ActiveRow == null)
		{
			MessageBox.Show("UOM Field Required");
			CmbUomOutPut.Focus();
			return false;
		}
		if (((UltraGridBase)CmbPackTypeOutPut).ActiveRow == null)
		{
			MessageBox.Show("Bag Type Field Required");
			CmbPackTypeOutPut.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtQtyOutPut.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Bag Quantity Field Required");
			txtQtyOutPut.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtNetWeightOutPut.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Unit Weight Field Required");
			txtNetWeightOutPut.Focus();
			return false;
		}
		return true;
	}

	private void reset()
	{
		try
		{
			RecId = 0;
			VoucherHeadId = 0;
			txtAvgRate.Text = "0";
			txtAvgStock.Text = "0";
			txtRemarks.Text = string.Empty;
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			UpdateMode = false;
			((Control)(object)CmbJobOrderNo).Enabled = true;
			table.Clear();
			grdStockConversionProductionDetail.ClearStructure();
			GenerateDocNumberInput();
			((UltraGridBase)CmbJobOrderNo).DataSource = null;
			((Control)(object)CmbJobOrderNo).Text = string.Empty;
			JobOrderNoFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void resetOutPut()
	{
		try
		{
			DiffNetWeight = 0.0;
			ConsumedWeight = 0.0;
			RecIdOutPut = 0;
			VoucherHeadId = 0;
			btnSaveOutPut.Visible = true;
			btnUpdateOutPut.Visible = false;
			((Control)(object)CmbJobOrderNoOutput).Enabled = true;
			tableByProduct.Clear();
			grdByProduct.ClearStructure();
			GenerateDocNumberOutPut();
			((UltraGridBase)CmbJobOrderNoOutput).DataSource = null;
			((Control)(object)CmbJobOrderNoOutput).Text = string.Empty;
			JobOrderNoFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void resetDetail()
	{
		try
		{
			txtRateInput.Text = string.Empty;
			txtAmountInput.Text = string.Empty;
			((Control)(object)CmbRateUOMInput).Text = string.Empty;
			cmbItem.Value = "";
			cmbUOM.Value = "";
			txtQty.Text = "";
			cmbGodown.Value = "";
			CmbCropyr.Value = "";
			cmbLot.Value = "";
			cmbBagType.Value = "";
			txtNetweight.Text = string.Empty;
			txtdeailRemarks.Text = string.Empty;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void resetDetailOutPut()
	{
		try
		{
			AvgRate = 0.0;
			txtRateOutPut.Text = string.Empty;
			txtAmountOutPut.Text = string.Empty;
			((Control)(object)CmbRateUomOutPut).Text = string.Empty;
			CmbItemOutput.Value = "";
			CmbUomOutPut.Value = "";
			txtQtyOutPut.Text = "";
			CmbWarehouseOutput.Value = "";
			CmbCropYearOutPut.Value = "";
			CmbPackTypeOutPut.Value = "";
			txtNetWeightOutPut.Text = string.Empty;
			txtRemarksOutPut.Text = string.Empty;
			txtRateOutPut.Text = string.Empty;
			txtAmountOutPut.Text = string.Empty;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void gridsettings()
	{
		try
		{
			if (!formrights.DoHaveCanRateandAmount)
			{
				grdStockConversionProductionDetail.RootTable.Columns["Rate"].Visible = false;
				grdStockConversionProductionDetail.RootTable.Columns["RateUOM"].Visible = false;
				grdStockConversionProductionDetail.RootTable.Columns["ItemAmount"].Visible = false;
			}
			grdStockConversionProductionDetail.RootTable.Columns["WareHouse"].EditType = (EditType)0;
			grdStockConversionProductionDetail.RootTable.Columns["Item"].EditType = (EditType)0;
			grdStockConversionProductionDetail.RootTable.Columns["ItemUOM"].EditType = (EditType)0;
			grdStockConversionProductionDetail.RootTable.Columns["CropYear"].EditType = (EditType)0;
			grdStockConversionProductionDetail.RootTable.Columns["JobLot"].EditType = (EditType)0;
			grdStockConversionProductionDetail.RootTable.Columns["PackingType"].EditType = (EditType)0;
			grdStockConversionProductionDetail.RootTable.Columns["Rate"].EditType = (EditType)0;
			grdStockConversionProductionDetail.RootTable.Columns["RateUOM"].EditType = (EditType)0;
			grdStockConversionProductionDetail.RootTable.Columns["ItemAmount"].EditType = (EditType)0;
			grdStockConversionProductionDetail.RootTable.Columns["Remarks"].EditType = (EditType)0;
			grdStockConversionProductionDetail.RootTable.Columns["Id"].Visible = false;
			grdStockConversionProductionDetail.RootTable.Columns["DocDate"].Visible = false;
			grdStockConversionProductionDetail.RootTable.Columns["DocNo"].Visible = false;
			grdStockConversionProductionDetail.RootTable.Columns["BillNo"].Visible = false;
			grdStockConversionProductionDetail.RootTable.Columns["ItemId"].Visible = false;
			grdStockConversionProductionDetail.RootTable.Columns["JobLotId"].Visible = false;
			grdStockConversionProductionDetail.RootTable.Columns["CropYearId"].Visible = false;
			grdStockConversionProductionDetail.RootTable.Columns["WarehouseId"].Visible = false;
			grdStockConversionProductionDetail.RootTable.Columns["PackingTypeId"].Visible = false;
			grdStockConversionProductionDetail.RootTable.Columns["ItemUOMId"].Visible = false;
			grdStockConversionProductionDetail.RootTable.Columns["RateUOMId"].Visible = false;
			grdStockConversionProductionDetail.RootTable.Columns["Quantity"].AggregateFunction = (AggregateFunction)2;
			grdStockConversionProductionDetail.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			grdStockConversionProductionDetail.RootTable.Columns["Quantity"].FormatString = "0,0";
			grdStockConversionProductionDetail.RootTable.Columns["Quantity"].TotalFormatString = "#,##0.##";
			grdStockConversionProductionDetail.RootTable.Columns["Weight"].FormatString = "0,0";
			grdStockConversionProductionDetail.RootTable.Columns["Weight"].TotalFormatString = "#,##0.##";
			grdStockConversionProductionDetail.RootTable.Columns["Quantity"].TextAlignment = (TextAlignment)3;
			grdStockConversionProductionDetail.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void gridsettingsOutPut()
	{
		try
		{
			if (!formrights.DoHaveCanRateandAmount)
			{
				grdByProduct.RootTable.Columns["Rate"].Visible = false;
				grdByProduct.RootTable.Columns["RateUOM"].Visible = false;
				grdByProduct.RootTable.Columns["ItemAmount"].Visible = false;
			}
			grdByProduct.RootTable.Columns["Id"].Visible = false;
			grdByProduct.RootTable.Columns["StockAcId"].Visible = false;
			grdByProduct.RootTable.Columns["CropYearId"].Visible = false;
			grdByProduct.RootTable.Columns["WareHouseId"].Visible = false;
			grdByProduct.RootTable.Columns["ItemId"].Visible = false;
			grdByProduct.RootTable.Columns["ItemUOMId"].Visible = false;
			grdByProduct.RootTable.Columns["JobLotId"].Visible = false;
			grdByProduct.RootTable.Columns["PackingTypeId"].Visible = false;
			grdByProduct.RootTable.Columns["RateUOMId"].Visible = false;
			grdByProduct.RootTable.Columns["Id"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["EntryType"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["WareHouse"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["Item"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["ItemUOM"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["CropYear"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["JobLot"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["PackingType"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["Rate"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["RateUOM"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["ItemAmount"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["Remarks"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["StockAc"].EditType = (EditType)0;
			grdByProduct.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			grdByProduct.RootTable.Columns["Quantity"].AggregateFunction = (AggregateFunction)2;
			grdByProduct.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			grdByProduct.RootTable.Columns["Weight"].FormatString = "0,0";
			grdByProduct.RootTable.Columns["Quantity"].FormatString = "0,0";
			grdByProduct.RootTable.Columns["ItemAmount"].FormatString = "0,0";
			grdByProduct.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			grdByProduct.RootTable.Columns["Quantity"].TextAlignment = (TextAlignment)3;
			grdByProduct.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			grdByProduct.RootTable.Columns["EntryType"].ColumnType = (ColumnType)0;
			grdByProduct.RootTable.Columns["EntryType"].HasValueList = true;
			grdByProduct.RootTable.Columns["EntryType"].LimitToList = true;
			grdByProduct.RootTable.Columns["EntryType"].EditType = (EditType)4;
			grdByProduct.RootTable.Columns["EntryType"].Caption = "EntryType";
			grdByProduct.RootTable.Columns["EntryType"].ValueList.PopulateValueList((IEnumerable)dtEntryType.DefaultView, "Id", "Name");
			grdByProduct.RootTable.Columns["EntryType"].Width = 150;
			ctrlGrdBar4_Load(null, null);
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

	private void GridHistory()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		try
		{
			InvFoodProduction val = new InvFoodProduction
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			};
			DataTable dthis = new DataTable();
			dthis = InvFoodProduction.GetAll(val);
			if (dthis.Rows.Count > 0)
			{
				DataTable dtHistory = new DataTable();
				dtHistory.Columns.Add("Id");
				dtHistory.Columns.Add("DocSrNo");
				dtHistory.Columns.Add("DocDate");
				dtHistory.Columns.Add("JobOrderNo");
				dtHistory.Columns.Add("EntryType");
				dtHistory.Columns.Add("Remarks");
				for (int i = 0; i < dthis.Rows.Count; i++)
				{
					dtHistory.Rows.Add(dthis.Rows[i]["Id"], dthis.Rows[i]["DocCode"], Conversion.ToDateTime(dthis.Rows[i]["DocDate"]).ToString("dd-MMM-yyyy"), dthis.Rows[i]["InvJobOrderNo"], dthis.Rows[i]["EntryType"], dthis.Rows[i]["MainRemarks"]);
				}
				DatagridHistory.DataSource = dtHistory;
				DatagridHistory.RetrieveStructure();
				GridSetting();
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
			DatagridHistory.RootTable.Columns["Id"].Visible = false;
			DatagridHistory.RootTable.Columns["DocSrNo"].Width = 120;
			DatagridHistory.RootTable.Columns["DocDate"].Width = 100;
			DatagridHistory.RootTable.Columns["Remarks"].Width = 250;
			DatagridHistory.RootTable.Columns["JobOrderNo"].Width = 120;
			DatagridHistory.RootTable.Columns["EntryType"].Width = 150;
			DatagridHistory.RootTable.Columns.Add("Edit");
			DatagridHistory.RootTable.Columns["Edit"].Key = "Edit";
			DatagridHistory.RootTable.Columns["Edit"].Caption = "Edit";
			DatagridHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DatagridHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			DatagridHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			DatagridHistory.RootTable.Columns["Edit"].Width = 40;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void button1_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void btnUpdateDetail_Click(object sender, EventArgs e)
	{
	}

	private void btnCancelDetail_Click(object sender, EventArgs e)
	{
		Add.Visible = true;
		btnUpdateDetail.Visible = false;
		btnCancelDetail.Visible = false;
		resetDetail();
	}

	private void grdStockConversionProductionDetail_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grdStockConversionProductionDetail.CurrentRow;
			if (Convert.ToInt32(item.Cells[0].Value) == 0)
			{
				if (item != null)
				{
					updateDetailIndex = item.RowIndex;
					cmbGodown.Value = item.Cells["WareHouseId"].Value.ToString();
					((Control)(object)cmbGodown).Text = item.Cells["WareHouse"].Text.ToString();
					cmbLot.Value = item.Cells["JobLotId"].Value.ToString();
					((Control)(object)cmbLot).Text = item.Cells["JobLot"].Text.ToString();
					cmbBagType.Value = item.Cells["PackingTypeId"].Value.ToString();
					((Control)(object)cmbBagType).Text = item.Cells["PackingType"].Text.ToString();
					cmbItem.Value = item.Cells["ItemId"].Value.ToString();
					((Control)(object)cmbItem).Text = item.Cells["Item"].Text.ToString();
					cmbUOM.Value = item.Cells["ItemUOMId"].Value.ToString();
					((Control)(object)cmbUOM).Text = item.Cells["ItemUOM"].Text.ToString();
					((Control)(object)CmbCropyr).Text = item.Cells["CropYear"].Text.ToString();
					txtQty.Text = item.Cells["Quantity"].Text.ToString();
					txtNetweight.Text = item.Cells["Weight"].Text.ToString();
					txtRateInput.Text = item.Cells["Rate"].Value.ToString();
					CmbRateUOMInput.Value = item.Cells["RateUOMId"].Value.ToString();
					((Control)(object)CmbRateUOMInput).Text = item.Cells["RateUOM"].Text.ToString();
					txtAmountInput.Text = item.Cells["ItemAmount"].Value.ToString();
					txtdeailRemarks.Text = item.Cells["Remarks"].Text.ToString();
					Add.Visible = false;
					btnUpdateDetail.Visible = true;
					btnCancelDetail.Visible = true;
				}
			}
			else
			{
				MessageBox.Show("Record Not Update beacuse invoice No exist");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TotalWeightInput()
	{
		try
		{
			if (((Control)(object)cmbUOM).Text != string.Empty && txtQty.Text != string.Empty)
			{
				double UOM = 0.0;
				double NetWeight = 0.0;
				double num = double.Parse(txtQty.Text.Trim());
				UOM = double.Parse(((Control)(object)cmbUOM).Text.Trim());
				txtNetweight.Text = (num * UOM).ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TotalAmountInput()
	{
		try
		{
			if (((Control)(object)cmbUOM).Text != string.Empty && txtQty.Text != string.Empty && txtNetweight.Text != string.Empty && txtRateInput.Text != string.Empty && ((Control)(object)CmbRateUOMInput).Text != string.Empty)
			{
				double Rate = 0.0;
				double RateUOM = 0.0;
				double Amount = 0.0;
				Rate = double.Parse(txtRateInput.Text.Trim());
				double num = double.Parse(txtNetweight.Text.Trim());
				RateUOM = double.Parse(((Control)(object)CmbRateUOMInput).Text.Trim());
				Amount = num / RateUOM * Rate;
				txtAmountInput.Text = Amount.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidationOfPackingMaterial()
	{
		if (((UltraGridBase)cmbJobOrderPackingMaterial).ActiveRow == null)
		{
			MessageBox.Show("Product No. Field Required");
			cmbJobOrderPackingMaterial.Focus();
			return false;
		}
		if (((UltraGridBase)cmbItemPackingMaterial).ActiveRow == null)
		{
			MessageBox.Show(" Item Field Required");
			cmbItemPackingMaterial.Focus();
			return false;
		}
		if (((UltraGridBase)cmbUOMPackingMaterial).ActiveRow == null)
		{
			MessageBox.Show("UOM  Field Required");
			cmbUOMPackingMaterial.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtPackingMaterialQTY.Text.Trim()) == 0.0)
		{
			MessageBox.Show(" Qty Field Required");
			txtPackingMaterialQTY.Focus();
			return false;
		}
		return true;
	}

	private void ItemFillPackingMaterial()
	{
		try
		{
			DataTable lst = CommonServices.ItemGetForComboServiceBind();
			if (lst.Rows.Count > 0)
			{
				DDL.BindDDL(lst, cmbItemPackingMaterial, "Id", "ItemName", "ItemName", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbItemPackingMaterial_Leave(object sender, EventArgs e)
	{
		try
		{
			if (dtuomlst.Rows.Count <= 0)
			{
				return;
			}
			DataTable dtUomPackingMaterial = new DataTable();
			dtUomPackingMaterial.Columns.Add("Id");
			dtUomPackingMaterial.Columns.Add("Equivalent");
			for (int i = 0; i < dtuomlst.Rows.Count; i++)
			{
				if (Convert.ToInt32(dtuomlst.Rows[i]["ItemId"]) == Convert.ToInt32(cmbItemPackingMaterial.Value))
				{
					dtUomPackingMaterial.Rows.Add(dtuomlst.Rows[i]["Id"], dtuomlst.Rows[i]["Equivalent"]);
				}
			}
			if (dtUomPackingMaterial.Rows.Count > 0)
			{
				DDL.BindDDL(dtUomPackingMaterial, cmbUOMPackingMaterial, "Id", "Equivalent", "UOM", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PackingMaterialInsert()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		try
		{
			if (FormValidationOfPackingMaterial())
			{
				InvFoodProductionPackingMaterial invFoodProductionPacking = new InvFoodProductionPackingMaterial();
				if (RecpackingMaterial > 0)
				{
					invFoodProductionPacking.Id = RecpackingMaterial;
				}
				invFoodProductionPacking.DocDate = txtdocdatePackingMaterial.Value;
				invFoodProductionPacking.DocNo = Conversion.ToInt((object)txtDocNoPackingMaterial.Text);
				invFoodProductionPacking.InvProductionJobOrderId = Conversion.ToInt(cmbJobOrderPackingMaterial.Value);
				invFoodProductionPacking.ItemId = Conversion.ToInt(cmbItemPackingMaterial.Value);
				invFoodProductionPacking.itemUomSchId = Conversion.ToInt(cmbUOMPackingMaterial.Value);
				invFoodProductionPacking.Qty = Conversion.ToDouble((object)txtPackingMaterialQTY.Text);
				invFoodProductionPacking.Rate = Conversion.ToInt((object)txtPackingMaterialRate.Text);
				invFoodProductionPacking.Amount = Conversion.ToDouble((object)txtAmountPackingMaterial.Text);
				invFoodProductionPacking.pmRemarks = Conversion.ToString((object)txtremarksPackingMaterial.Text);
				invFoodProductionPacking.DocumentTypeId = 111;
				invFoodProductionPacking.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
				InvFoodProductionPackingMaterial.Save(invFoodProductionPacking);
				if (RecpackingMaterial > 0)
				{
					MessageBox.Show("Update Successfully");
				}
				else
				{
					MessageBox.Show("Save Successfully");
				}
				FormResetOfPackingMaterial();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnsavepackingmaterial_Click(object sender, EventArgs e)
	{
		try
		{
			RecpackingMaterial = 0;
			PackingMaterialInsert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void BindGrid()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		try
		{
			DataTable dtpm = InvFoodProductionPackingMaterial.GetAll(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			DataTable dt = new DataTable();
			dt.Clear();
			dt.Columns.Add("Id");
			dt.Columns.Add("DNo");
			dt.Columns.Add("DocDate");
			dt.Columns.Add("JOrder");
			dt.Columns.Add("Item");
			dt.Columns.Add("UOM");
			dt.Columns.Add("Qty", typeof(double));
			dt.Columns.Add("Rate", typeof(double));
			dt.Columns.Add("Amount", typeof(double));
			dt.Columns.Add("Remarks");
			for (int i = 0; i < dtpm.Rows.Count; i++)
			{
				dt.Rows.Add(dtpm.Rows[i]["Id"], dtpm.Rows[i]["DocNo"], Conversion.ToDateTime(dtpm.Rows[i]["DocDate"]).ToString("dd-MMM-yyyy"), dtpm.Rows[i]["InvProductionJobOrderId"], dtpm.Rows[i]["ItemName"], dtpm.Rows[i]["UOMDescription"], dtpm.Rows[i]["Qty"], dtpm.Rows[i]["Rate"], dtpm.Rows[i]["Amount"], dtpm.Rows[i]["pmRemarks"]);
			}
			grdPackingMaterial.DataSource = dt;
			grdPackingMaterial.RetrieveStructure();
			GridSettingOfPackingmaterial();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void GridSettingOfPackingmaterial()
	{
		try
		{
			grdPackingMaterial.RootTable.Columns["Id"].Visible = false;
			grdPackingMaterial.RootTable.Columns["DNo"].Width = 50;
			grdPackingMaterial.RootTable.Columns["DocDate"].Width = 100;
			grdPackingMaterial.RootTable.Columns["JOrder"].Width = 50;
			grdPackingMaterial.RootTable.Columns["Item"].Width = 200;
			grdPackingMaterial.RootTable.Columns["UOM"].Width = 100;
			grdPackingMaterial.RootTable.Columns["Qty"].Width = 60;
			grdPackingMaterial.RootTable.Columns["Rate"].Width = 60;
			grdPackingMaterial.RootTable.Columns["Amount"].Width = 80;
			grdPackingMaterial.RootTable.Columns["Remarks"].Width = 150;
			grdPackingMaterial.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdPackingMaterial.RootTable.Columns["Rate"].AggregateFunction = (AggregateFunction)2;
			grdPackingMaterial.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdPackingMaterial.RootTable.Columns["Qty"].FormatString = "0,0";
			grdPackingMaterial.RootTable.Columns["Rate"].FormatString = "0,0";
			grdPackingMaterial.RootTable.Columns["Amount"].FormatString = "0,0";
			grdPackingMaterial.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdPackingMaterial.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdPackingMaterial.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPackingMaterial_DoubleClick(object sender, EventArgs e)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		try
		{
			btnsavepackingmaterial.Visible = false;
			btnUpdatepackingmaterial.Visible = true;
			InvFoodProductionPackingMaterial SC = new InvFoodProductionPackingMaterial();
			RecpackingMaterial = Conversion.ToInt((object)grdPackingMaterial.CurrentRow.Cells[0].Value.ToString());
			SC = InvFoodProductionPackingMaterial.GetByID(RecpackingMaterial);
			if (SC != null)
			{
				txtdocdatePackingMaterial.Value = SC.DocDate;
				txtDocNoPackingMaterial.Text = SC.DocNo.ToString();
				txtPackingMaterialQTY.Text = SC.Qty.ToString();
				txtPackingMaterialRate.Text = SC.Rate.ToString();
				txtremarksPackingMaterial.Text = Conversion.ToString((object)SC.pmRemarks);
				txtAmountPackingMaterial.Text = SC.Amount.ToString();
				cmbItemPackingMaterial.Value = SC.ItemId;
				cmbUOMPackingMaterial.Value = SC.itemUomSchId;
				cmbJobOrderPackingMaterial.Value = SC.InvProductionJobOrderId.ToString();
				cmbItemPackingMaterial.Focus();
				UpdateMode = true;
				VoucherHeadId = CommonServices.VoucherHeadIdGet(RecpackingMaterial, 111);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdatepackingmaterial_Click(object sender, EventArgs e)
	{
		try
		{
			PackingMaterialInsert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void FormResetOfPackingMaterial()
	{
		try
		{
			RecpackingMaterial = 0;
			VoucherHeadId = 0;
			btnsavepackingmaterial.Visible = true;
			btnUpdatepackingmaterial.Visible = false;
			UpdateMode = false;
			BindGrid();
			GenerateCodeOfPackingMaterial();
			((Control)(object)cmbItemPackingMaterial).Text = string.Empty;
			((Control)(object)cmbUOMPackingMaterial).Text = string.Empty;
			txtdocdatePackingMaterial.Value = DateTime.Now;
			((Control)(object)cmbJobOrderPackingMaterial).Text = string.Empty;
			txtPackingMaterialQTY.Text = string.Empty;
			txtPackingMaterialRate.Text = string.Empty;
			txtAmountPackingMaterial.Text = string.Empty;
			txtremarksPackingMaterial.Text = string.Empty;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void GenerateCodeOfPackingMaterial()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		try
		{
			int code = InvFoodProductionPackingMaterial.GenerateCode(new InvFoodProduction
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			if (code > 0)
			{
				txtDocNoPackingMaterial.Text = code.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnNewpackingmaterial_Click(object sender, EventArgs e)
	{
		FormResetOfPackingMaterial();
	}

	private void txtPackingMaterialRate_TextChanged(object sender, EventArgs e)
	{
		CaluculatonofPackingMaterial();
	}

	public void CaluculatonofPackingMaterial()
	{
		try
		{
			if (txtPackingMaterialQTY.Text != string.Empty && txtPackingMaterialRate.Text != string.Empty)
			{
				double fAmount = double.Parse(txtPackingMaterialRate.Text) * double.Parse(txtPackingMaterialQTY.Text);
				txtAmountPackingMaterial.Text = Math.Round(fAmount).ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtPackingMaterialQTY_TextChanged(object sender, EventArgs e)
	{
		CaluculatonofPackingMaterial();
	}

	private void ChartOfAccountOverHead()
	{
		try
		{
			DataTable CoaList = CommonServices.CoaAllocationGetAllServiceBind();
			if (CoaList.Rows.Count <= 0)
			{
				return;
			}
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("AccountTitle");
			for (int i = 0; i < CoaList.Rows.Count; i++)
			{
				if (Convert.ToInt32(CoaList.Rows[i]["AccountTypeId"]) != 2 && Convert.ToInt32(CoaList.Rows[i]["AccountTypeId"]) != 11 && Convert.ToInt32(CoaList.Rows[i]["AccountTypeId"]) != 2)
				{
					dt.Rows.Add(CoaList.Rows[i]["Id"], CoaList.Rows[i]["AccountTitle"]);
				}
			}
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbChartofAccountOverHead, "Id", "AccountTitle", "Account Title", ZeroIndex: true);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OverHeadInsert()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		try
		{
			if (FormValidationOfOverHead())
			{
				InvFoodProductionOverHeads invFoodProductionOverHeads = new InvFoodProductionOverHeads();
				if (RecOverHeadId > 0)
				{
					invFoodProductionOverHeads.Id = RecOverHeadId;
				}
				invFoodProductionOverHeads.DocDate = txtDocDateOverHead.Value;
				invFoodProductionOverHeads.DocNo = Conversion.ToInt((object)txtDocNoOverHead.Text);
				invFoodProductionOverHeads.InvProductionJobOrderId = Conversion.ToInt(cmbJobOrderOverHead.Value);
				invFoodProductionOverHeads.CharOfAccountId = Conversion.ToInt(cmbChartofAccountOverHead.Value);
				invFoodProductionOverHeads.ohRemarks = Conversion.ToString((object)txtLedgerRemarksOverHead.Text);
				invFoodProductionOverHeads.Amount = Conversion.ToDouble((object)txtExpAmountOverHead.Text);
				invFoodProductionOverHeads.DocumentTypeId = 110;
				invFoodProductionOverHeads.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
				InvFoodProductionOverHeads.Save(invFoodProductionOverHeads);
				if (RecOverHeadId > 0)
				{
					MessageBox.Show("Update Successfully");
				}
				else
				{
					MessageBox.Show("Save Successfully");
				}
				FormResetOverHead();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnSaveOverHead_Click(object sender, EventArgs e)
	{
		try
		{
			RecOverHeadId = 0;
			OverHeadInsert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdateOverHead_Click(object sender, EventArgs e)
	{
		try
		{
			OverHeadInsert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOverHead_DoubleClick(object sender, EventArgs e)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		try
		{
			btnSaveOverHead.Visible = false;
			btnUpdateOverHead.Visible = true;
			InvFoodProductionOverHeads SC = new InvFoodProductionOverHeads();
			RecOverHeadId = Conversion.ToInt((object)grdOverHead.CurrentRow.Cells[0].Value.ToString());
			SC = InvFoodProductionOverHeads.GetByID(RecOverHeadId);
			txtDocDateOverHead.Value = SC.DocDate;
			txtDocNoOverHead.Text = SC.DocNo.ToString();
			cmbJobOrderOverHead.Value = SC.InvProductionJobOrderId.ToString();
			cmbChartofAccountOverHead.Value = SC.CharOfAccountId;
			txtLedgerRemarksOverHead.Text = SC.ohRemarks;
			txtExpAmountOverHead.Text = SC.Amount.ToString();
			cmbChartofAccountOverHead.Focus();
			UpdateMode = true;
			VoucherHeadId = CommonServices.VoucherHeadIdGet(RecOverHeadId, 110);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FormResetOverHead()
	{
		try
		{
			RecOverHeadId = 0;
			VoucherHeadId = 0;
			btnUpdateOverHead.Visible = false;
			btnSaveOverHead.Visible = true;
			UpdateMode = false;
			txtDocDateOverHead.Text = string.Empty;
			((Control)(object)cmbJobOrderOverHead).Text = string.Empty;
			txtLedgerRemarksOverHead.Text = string.Empty;
			txtExpAmountOverHead.Text = string.Empty;
			BindGridOfOverHead();
			GenerateCodeofOverHead();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateCodeofOverHead()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		try
		{
			int code = InvFoodProductionOverHeads.GenerateCode(new InvFoodProduction
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			if (code > 0)
			{
				txtDocNoOverHead.Text = code.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindGridOfOverHead()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		try
		{
			InvFoodProduction val = new InvFoodProduction
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			};
			DataTable dtover = new DataTable();
			dtover = InvFoodProductionOverHeads.GetAll(val);
			DataTable dt = new DataTable();
			dt.Clear();
			dt.Columns.Add("Id");
			dt.Columns.Add("DNo");
			dt.Columns.Add("DocDate");
			dt.Columns.Add("PCode");
			dt.Columns.Add("GLAccount");
			dt.Columns.Add("Remarks");
			dt.Columns.Add("Expense", typeof(double));
			for (int i = 0; i < dtover.Rows.Count; i++)
			{
				dt.Rows.Add(dtover.Rows[i]["Id"], dtover.Rows[i]["DocNo"], Conversion.ToDateTime(dtover.Rows[i]["DocDate"]).ToString("dd-MMM-yyyy"), dtover.Rows[i]["PlanCode"], dtover.Rows[i]["AccountTitle"], dtover.Rows[i]["ohRemarks"], dtover.Rows[i]["Amount"]);
			}
			grdOverHead.DataSource = dt;
			grdOverHead.RetrieveStructure();
			OverHeadGridSetting();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OverHeadGridSetting()
	{
		grdOverHead.RootTable.Columns["Id"].Visible = false;
		grdOverHead.RootTable.Columns["DNo"].EditType = (EditType)0;
		grdOverHead.RootTable.Columns["DocDate"].EditType = (EditType)0;
		grdOverHead.RootTable.Columns["PCode"].EditType = (EditType)0;
		grdOverHead.RootTable.Columns["GLAccount"].EditType = (EditType)0;
		grdOverHead.RootTable.Columns["Remarks"].EditType = (EditType)0;
		grdOverHead.RootTable.Columns["Expense"].EditType = (EditType)0;
		grdOverHead.RootTable.Columns["DNo"].FilterEditType = (FilterEditType)1;
		grdOverHead.RootTable.Columns["DocDate"].FilterEditType = (FilterEditType)1;
		grdOverHead.RootTable.Columns["PCode"].FilterEditType = (FilterEditType)1;
		grdOverHead.RootTable.Columns["GLAccount"].FilterEditType = (FilterEditType)1;
		grdOverHead.RootTable.Columns["Remarks"].FilterEditType = (FilterEditType)1;
		grdOverHead.RootTable.Columns["Expense"].FilterEditType = (FilterEditType)1;
		grdOverHead.RootTable.Columns["Expense"].AggregateFunction = (AggregateFunction)2;
		grdOverHead.RootTable.Columns["Expense"].TextAlignment = (TextAlignment)3;
		grdOverHead.RootTable.Columns["Expense"].FormatString = "0,0";
		grdOverHead.RootTable.Columns["DNo"].Width = 50;
		grdOverHead.RootTable.Columns["DocDate"].Width = 100;
		grdOverHead.RootTable.Columns["PCode"].Width = 60;
		grdOverHead.RootTable.Columns["GLAccount"].Width = 150;
		grdOverHead.RootTable.Columns["Remarks"].Width = 200;
		grdOverHead.RootTable.Columns["Expense"].Width = 60;
	}

	private void btnNewOverHead_Click(object sender, EventArgs e)
	{
		FormResetOverHead();
	}

	private bool FormValidationOfOverHead()
	{
		if (txtDocNoOverHead.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Doc No. Field Required");
			txtDocNoOverHead.Focus();
			return false;
		}
		if (((UltraGridBase)cmbJobOrderOverHead).ActiveRow == null)
		{
			MessageBox.Show("Product No. Field Required");
			cmbJobOrderOverHead.Focus();
			return false;
		}
		if (((UltraGridBase)cmbChartofAccountOverHead).ActiveRow == null)
		{
			MessageBox.Show(" Chart Of Account Field Required");
			cmbChartofAccountOverHead.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtExpAmountOverHead.Text.Trim()) == 0.0)
		{
			MessageBox.Show(" Exp Field Required");
			txtExpAmountOverHead.Focus();
			return false;
		}
		return true;
	}

	private void button3_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void frmFoodProduction_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void button2_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void cmbsummeryJobOrderNo_TextChanged(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.JobOrderId = Conversion.ToInt(cmbsummeryJobOrderNo.Value);
			dtSummerReport = InvFoodProductionReports.InvFoodProductionRecoverySummeryReport(obj);
			if (dtSummerReport.Rows.Count > 0 && dtCrop != null)
			{
				grdSummeryReport.DataSource = dtSummerReport;
				grdSummeryReport.RetrieveStructure();
				SummeryHistoryGridSetting();
			}
			DataTable dt = InvFoodProduction.GetSummeryValues(new ReportsParameters
			{
				JobOrderId = Conversion.ToInt(cmbsummeryJobOrderNo.Value)
			});
			if (dt.Rows.Count > 0 && dt != null)
			{
				grdSummery.DataSource = dt;
				grdSummery.RetrieveStructure();
				grdSummery.RootTable.Columns["TranType"].Width = 130;
				grdSummery.RootTable.Columns["SortNo"].Width = 80;
				grdSummery.RootTable.Columns["NetQty"].TextAlignment = (TextAlignment)3;
				grdSummery.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
				grdSummery.RootTable.Columns["NetQty"].AggregateFunction = (AggregateFunction)2;
				grdSummery.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
				grdSummery.RootTable.Columns["NetQty"].FormatString = "#,##";
				grdSummery.RootTable.Columns["NetWeight"].FormatString = "#,##";
				grdSummery.RootTable.Columns["NetQty"].TotalFormatString = "#,##";
				grdSummery.RootTable.Columns["NetWeight"].TotalFormatString = "#,##";
				if (!formrights.DoHaveCanRateandAmount)
				{
					grdSummery.RootTable.Columns["AvgRate"].Visible = false;
					grdSummery.RootTable.Columns["Amount"].Visible = false;
				}
			}
			else
			{
				grdSummery.DataSource = null;
			}
			ctrlGrdBar5_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SummeryHistoryGridSetting()
	{
		try
		{
			if (!formrights.DoHaveCanRateandAmount)
			{
				grdSummeryReport.RootTable.Columns["TranRate"].Visible = false;
				grdSummeryReport.RootTable.Columns["TotalAmountBySortNo"].Visible = false;
				grdSummeryReport.RootTable.Columns["Amount"].Visible = false;
			}
			grdSummeryReport.RootTable.Columns["TotalAmountBySortNo"].Visible = false;
			grdSummeryReport.RootTable.Columns["TranRate"].Visible = false;
			grdSummeryReport.RootTable.Columns["TotalWeightBySortNo"].Visible = false;
			grdSummeryReport.RootTable.Columns["RmWeightTotal"].Visible = false;
			grdSummeryReport.RootTable.Columns["BpWeightTotal"].Visible = false;
			grdSummeryReport.RootTable.Columns["FgWeightTotal"].Visible = false;
			grdSummeryReport.RootTable.Columns["SortNo"].Width = 80;
			grdSummeryReport.RootTable.Columns["TranType"].Width = 200;
			grdSummeryReport.RootTable.Columns["ItemName"].Width = 350;
			grdSummeryReport.RootTable.Columns["NetQty"].Width = 100;
			grdSummeryReport.RootTable.Columns["NetWeight"].Width = 150;
			grdSummeryReport.RootTable.Columns["Amount"].Width = 150;
			grdSummeryReport.RootTable.Columns["NetQty"].AggregateFunction = (AggregateFunction)2;
			grdSummeryReport.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			grdSummeryReport.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdSummeryReport.RootTable.Columns["OutPutPrcnt"].AggregateFunction = (AggregateFunction)2;
			grdSummeryReport.RootTable.Columns["NetQty"].FormatString = "0,0";
			grdSummeryReport.RootTable.Columns["NetWeight"].FormatString = "0,0";
			grdSummeryReport.RootTable.Columns["Amount"].FormatString = "0,0";
			grdSummeryReport.RootTable.Columns["OutPutPrcnt"].FormatString = "#,0.##";
			grdSummeryReport.RootTable.Columns["OutPutPrcnt"].TotalFormatString = "#,0.##";
			grdSummeryReport.RootTable.Columns["NetQty"].TotalFormatString = "#,0.##";
			grdSummeryReport.RootTable.Columns["NetWeight"].TotalFormatString = "#,0.##";
			grdSummeryReport.RootTable.Columns["Amount"].TotalFormatString = "#,0.##";
			grdSummeryReport.RootTable.Columns["NetQty"].TextAlignment = (TextAlignment)3;
			grdSummeryReport.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			grdSummeryReport.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdSummeryReport.RootTable.Columns["OutPutPrcnt"].TextAlignment = (TextAlignment)3;
			ctrlGrdBar6_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Print_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = InvFoodProductionReports.InvFoodProductionSlip(new ReportsParameters
			{
				Id = RecId
			});
			if (dt.Rows.Count == 0 || dt == null)
			{
				MessageBox.Show("Record Not Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "601-InvFoodProductionSlip.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DatagridHistory_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			InvFoodProduction SC = new InvFoodProduction();
			if (Conversion.ToInt((object)DatagridHistory.CurrentRow.Cells[0].Value.ToString()) <= 0)
			{
				return;
			}
			SC = InvFoodProduction.GetByID(Conversion.ToInt((object)DatagridHistory.CurrentRow.Cells[0].Value.ToString()));
			if (SC != null)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("EntryType");
				dt.Columns.Add("WareHouse");
				dt.Columns.Add("Item");
				dt.Columns.Add("JobLot");
				dt.Columns.Add("CropBatch");
				dt.Columns.Add("PackType");
				dt.Columns.Add("Qty", typeof(double));
				dt.Columns.Add("Weight", typeof(double));
				dt.Columns.Add("Order");
				dt.Columns.Add("Remarks");
				for (int i = 0; i < SC.invFoodProductionDetails.Count; i++)
				{
					dt.Rows.Add(SC.invFoodProductionDetails[i].EntryType, SC.invFoodProductionDetails[i].WareHouseName, SC.invFoodProductionDetails[i].ItemName, SC.invFoodProductionDetails[i].JobLotCode, SC.invFoodProductionDetails[i].CropBatch, SC.invFoodProductionDetails[i].PackTypeCode, SC.invFoodProductionDetails[i].Qty, SC.invFoodProductionDetails[i].Weight, SC.invFoodProductionDetails[i].InvProductionJobOrderNo, SC.invFoodProductionDetails[i].Remarks);
				}
				grdDetail.DataSource = dt;
				grdDetail.RetrieveStructure();
				DetailHistoryGridSetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DetailHistoryGridSetting()
	{
		try
		{
			grdDetail.RootTable.Columns["EntryType"].Width = 130;
			grdDetail.RootTable.Columns["WareHouse"].Width = 100;
			grdDetail.RootTable.Columns["Item"].Width = 200;
			grdDetail.RootTable.Columns["JobLot"].Width = 80;
			grdDetail.RootTable.Columns["CropBatch"].Width = 80;
			grdDetail.RootTable.Columns["PackType"].Width = 80;
			grdDetail.RootTable.Columns["Qty"].Width = 60;
			grdDetail.RootTable.Columns["Weight"].Width = 60;
			grdDetail.RootTable.Columns["Order"].Width = 40;
			grdDetail.RootTable.Columns["Remarks"].Width = 150;
			grdDetail.RootTable.Columns["Qty"].FormatString = "#0,0##";
			grdDetail.RootTable.Columns["Weight"].FormatString = "#0,0##";
			grdDetail.RootTable.Columns["Qty"].TotalFormatString = "#0,0##";
			grdDetail.RootTable.Columns["Weight"].TotalFormatString = "#0,0##";
			grdDetail.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			ctrlGrdBar2_Load(null, null);
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
			if (File.Exists(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)DatagridHistory).Name))
			{
				FileStream fs = new FileStream(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)DatagridHistory).Name, FileMode.OpenOrCreate, FileAccess.ReadWrite);
				DatagridHistory.LoadLayoutFile((Stream)fs);
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

	private void btnLoadInvoices_Click(object sender, EventArgs e)
	{
		try
		{
			GRNSelected = true;
			timer1.Enabled = true;
			LoadGRN = new LoadInvoices(UserAccount);
			LoadGRN.ShowDialog();
			LoadGRN.BringToFront();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadDataDetailfromPurchaseInvoivce()
	{
		try
		{
			DataTable dtpurchaseinvoice = LoadGRN.dtGRNS;
			if (dtpurchaseinvoice.Rows.Count > 0)
			{
				for (int i = 0; i < dtpurchaseinvoice.Rows.Count; i++)
				{
					table.Rows.Add(dtpurchaseinvoice.Rows[i]["Id"], Conversion.ToDateTime(dtpurchaseinvoice.Rows[i]["DocDate"]).ToString("dd-MMM-yyyy"), dtpurchaseinvoice.Rows[i]["DocNo"], dtpurchaseinvoice.Rows[i]["ManualBillNo"], dtpurchaseinvoice.Rows[i]["WarehouseId"], dtpurchaseinvoice.Rows[i]["WareHouseName"], dtpurchaseinvoice.Rows[i]["ItemId"], dtpurchaseinvoice.Rows[i]["ItemName"], dtpurchaseinvoice.Rows[i]["ItemUomId"], dtpurchaseinvoice.Rows[i]["UOMCodeItem"], 0, dtpurchaseinvoice.Rows[i]["CropYear"], dtpurchaseinvoice.Rows[i]["JobLotId"], dtpurchaseinvoice.Rows[i]["JobLotCode"], dtpurchaseinvoice.Rows[i]["PackingTypeId"], dtpurchaseinvoice.Rows[i]["PackTypeCode"], dtpurchaseinvoice.Rows[i]["ItemQty"], dtpurchaseinvoice.Rows[i]["NetBillWeight"], dtpurchaseinvoice.Rows[i]["ItemRate"], dtpurchaseinvoice.Rows[i]["RateUOMId"], dtpurchaseinvoice.Rows[i]["RateUOM"], dtpurchaseinvoice.Rows[i]["ItemAmount"], "");
				}
				grdStockConversionProductionDetail.DataSource = table;
				grdStockConversionProductionDetail.RetrieveStructure();
				gridsettings();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void timer1_Tick_1(object sender, EventArgs e)
	{
		try
		{
			if (GRNSelected && LoadGRN.dtGRNS != null && LoadGRN.dtGRNS.Rows.Count > 0)
			{
				Refresh();
				LoadDataDetailfromPurchaseInvoivce();
				LoadGRN.dtGRNS.Rows.Clear();
				timer1.Enabled = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OutPutInsert()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Expected O, but got Unknown
		try
		{
			if (!FormValidationOutPut())
			{
				return;
			}
			InvFoodProduction invFoodProduction = new InvFoodProduction();
			if (RecIdOutPut > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				invFoodProduction.Id = RecIdOutPut;
			}
			else
			{
				if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				GenerateDocNumberOutPut();
			}
			invFoodProduction.RefDocumentTypeId = 1;
			invFoodProduction.DocCode = Conversion.ToInt((object)txtDocNoOutPut.Text);
			invFoodProduction.DocDate = Conversion.ToDateTime((object)DocDateOutPut.Value);
			invFoodProduction.MainRemarks = txtRemarksHeaderOutPut.Text.ToString();
			invFoodProduction.InvFoodProductionPlanId = 0;
			invFoodProduction.DocumentTypeId = 112;
			invFoodProduction.EntryType = "Output";
			invFoodProduction.WIPAccountId = Conversion.ToInt(CmbWorkInProcess.Value);
			invFoodProduction.WIPItemId = Conversion.ToInt(CmbWIPItem.Value);
			invFoodProduction.InvJobOrderId = Conversion.ToInt(CmbJobOrderNoOutput.Value);
			invFoodProduction.InvJobOrderNo = Conversion.ToString((object)((Control)(object)CmbJobOrderNoOutput).Text.Trim());
			invFoodProduction.OrganizationId = UserAccount.OrganizationId;
			invFoodProduction.CompanyId = UserAccount.CompanyId;
			invFoodProduction.EntryDate = DateTime.Now;
			invFoodProduction.ModifyDate = DateTime.Now;
			invFoodProduction.EntryUser = UserAccount.ID;
			invFoodProduction.ModifyUser = UserAccount.ID;
			invFoodProduction.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			invFoodProduction.invFoodProductionDetails = new List<InvFoodProductionDetail>();
			if (grdByProduct.RowCount > 0)
			{
				GridEXRow[] rows = grdByProduct.GetRows();
				foreach (GridEXRow r in rows)
				{
					InvFoodProductionDetail Conversiondetail = new InvFoodProductionDetail();
					if (Conversion.ToInt(r.Cells["Quantity"].Value) > 0 && Conversion.ToDouble(r.Cells["Weight"].Value) > 0.0)
					{
						if (Conversion.ToString((object)r.Cells["EntryType"].Text.Trim()) == string.Empty)
						{
							MessageBox.Show("EntryType Field Require in Output Detail");
							return;
						}
						Conversiondetail.EntryType = Conversion.ToString((object)r.Cells["EntryType"].Text);
						Conversiondetail.Id = Conversion.ToInt(r.Cells["Id"].Value);
						Conversiondetail.RefDocNoId = Conversion.ToInt((object)0);
						if (r.Cells["WareHouseId"].Text == string.Empty || Conversion.ToInt(r.Cells["WareHouseId"].Value) == 0)
						{
							MessageBox.Show("WareHouse Field Require in Output Detail");
							return;
						}
						Conversiondetail.WarehouseId = Conversion.ToInt(r.Cells["WareHouseId"].Value);
						if (r.Cells["ItemId"].Text == string.Empty || Conversion.ToInt(r.Cells["ItemId"].Value) == 0)
						{
							MessageBox.Show("Item Field Require in Output Detail");
							return;
						}
						Conversiondetail.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
						Conversiondetail.ItemUomId = Conversion.ToInt(r.Cells["ItemUOMId"].Value);
						Conversiondetail.CropBatch = Conversion.ToString((object)r.Cells["CropYear"].Text);
						if (r.Cells["JobLotId"].Text == string.Empty || r.Cells["JobLotId"].Text == "0")
						{
							MessageBox.Show("JobLot Field Require in Output Detail");
							return;
						}
						Conversiondetail.JobLotId = Conversion.ToInt((object)r.Cells["JobLotId"].Text);
						Conversiondetail.PackingtypeId = Conversion.ToInt(r.Cells["PackingTypeId"].Value);
						Conversiondetail.Qty = Conversion.ToDouble((object)r.Cells["Quantity"].Text);
						Conversiondetail.Weight = Conversion.ToDouble(r.Cells["Weight"].Value);
						Conversiondetail.Rate = Conversion.ToDouble(r.Cells["Rate"].Value);
						Conversiondetail.RateUOMId = Conversion.ToInt(r.Cells["RateUOMId"].Value);
						Conversiondetail.InvProductionJobOrderId = Conversion.ToInt(CmbJobOrderNoOutput.Value);
						Conversiondetail.Amount = Conversion.ToInt(r.Cells["ItemAmount"].Value);
						Conversiondetail.VoucherHeadId = 0;
						Conversiondetail.Remarks = Conversion.ToString((object)r.Cells["Remarks"].Text);
						Conversiondetail.StockAcId = Conversion.ToInt(r.Cells["StockAcId"].Value);
						invFoodProduction.invFoodProductionDetails.Add(Conversiondetail);
					}
				}
				if (invFoodProduction.invFoodProductionDetails.Count == 0)
				{
					throw new Exception("Item Qty and Weight must be grater than zero");
				}
				InvFoodProduction.Save(invFoodProduction);
				if (RecIdOutPut > 0)
				{
					MessageBox.Show("Record Update Successfully" + invFoodProduction.DocCode);
				}
				else
				{
					MessageBox.Show("Record Save Successfully" + invFoodProduction.DocCode);
				}
				resetOutPut();
				resetDetailOutPut();
			}
			else
			{
				MessageBox.Show("Enter Detail First ...");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void bindUomOutPut()
	{
		try
		{
			if (dtuomlst.Rows.Count <= 0)
			{
				return;
			}
			DataTable dtUom = new DataTable();
			dtUom.Columns.Add("Id");
			dtUom.Columns.Add("Equivalent");
			for (int i = 0; i < dtuomlst.Rows.Count; i++)
			{
				if (Convert.ToInt32(dtuomlst.Rows[i]["ItemId"]) == Convert.ToInt32(CmbItemOutput.Value))
				{
					dtUom.Rows.Add(dtuomlst.Rows[i]["Id"], dtuomlst.Rows[i]["Equivalent"]);
				}
			}
			if (dtUom.Rows.Count > 0)
			{
				DDL.BindDDL(dtUom, CmbUomOutPut, "Id", "Equivalent", "UOM", ZeroIndex: false);
				DDL.BindDDL(dtUom, CmbRateUomOutPut, "Id", "Equivalent", "UOM", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAddOutPut_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValidationOfDetailPortionOutPut())
			{
				tableByProduct.Rows.Add(((Control)(object)CmbEntryType).Text, CmbWarehouseOutput.Value, ((Control)(object)CmbWarehouseOutput).Text, CmbItemOutput.Value, ((Control)(object)CmbItemOutput).Text, CmbUomOutPut.Value, ((Control)(object)CmbUomOutPut).Text, ((Control)(object)CmbCropYearOutPut).Text, CmbLotOutPut.Value, ((Control)(object)CmbLotOutPut).Text, CmbPackTypeOutPut.Value, ((Control)(object)CmbPackTypeOutPut).Text, txtQtyOutPut.Text, txtNetWeightOutPut.Text, Conversion.ToDouble((object)txtRateOutPut.Text.Trim()), Conversion.ToInt(CmbRateUomOutPut.Value), Conversion.ToDouble((object)((Control)(object)CmbRateUomOutPut).Text), Conversion.ToDouble((object)txtAmountOutPut.Text.Trim()), txtRemarksOutPut.Text, CmbStockAccount.Value, ((Control)(object)CmbStockAccount).Text);
				grdByProduct.DataSource = tableByProduct;
				grdByProduct.RetrieveStructure();
				resetDetailOutPut();
				gridsettingsOutPut();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnSaveOutPut_Click(object sender, EventArgs e)
	{
		try
		{
			RecIdOutPut = 0;
			OutPutInsert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdateOutPut_Click(object sender, EventArgs e)
	{
		try
		{
			OutPutInsert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbItemOutput_Leave(object sender, EventArgs e)
	{
		try
		{
			bindUomOutPut();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Total()
	{
		try
		{
			if (((Control)(object)CmbUomOutPut).Text != string.Empty && txtQtyOutPut.Text != string.Empty)
			{
				double UOM = 0.0;
				double NetWeight = 0.0;
				double num = double.Parse(txtQtyOutPut.Text.Trim());
				UOM = double.Parse(((Control)(object)CmbUomOutPut).Text.Trim());
				txtNetWeightOutPut.Text = (num * UOM).ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TotalAmount()
	{
		try
		{
			if (((Control)(object)CmbUomOutPut).Text != string.Empty && txtQtyOutPut.Text != string.Empty && txtNetWeightOutPut.Text != string.Empty && txtRateOutPut.Text != string.Empty && ((Control)(object)CmbRateUomOutPut).Text != string.Empty)
			{
				double Rate = 0.0;
				double RateUOM = 0.0;
				double Amount = 0.0;
				Rate = double.Parse(txtRateOutPut.Text.Trim());
				double num = double.Parse(txtNetWeightOutPut.Text.Trim());
				RateUOM = double.Parse(((Control)(object)CmbRateUomOutPut).Text.Trim());
				Amount = num / RateUOM * Rate;
				txtAmountOutPut.Text = Amount.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbUomOutPut_Leave(object sender, EventArgs e)
	{
		Total();
		TotalAmount();
	}

	private void txtQtyOutPut_TextChanged(object sender, EventArgs e)
	{
		Total();
		TotalAmount();
	}

	private void txtRateOutPut_TextChanged(object sender, EventArgs e)
	{
	}

	private void CmbRateUomOutPut_Leave(object sender, EventArgs e)
	{
	}

	private void txtQtyOutPut_KeyPress(object sender, KeyPressEventArgs e)
	{
		e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
	}

	private void toolStripButton10_Click(object sender, EventArgs e)
	{
		resetOutPut();
		resetDetailOutPut();
	}

	private void txtNetWeightOutPut_KeyPress(object sender, KeyPressEventArgs e)
	{
		e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
	}

	private void txtRateOutPut_KeyPress(object sender, KeyPressEventArgs e)
	{
		e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
	}

	private void grdByProduct_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grdByProduct.CurrentRow;
			if (item != null)
			{
				updateDetailIndex = item.RowIndex;
				((Control)(object)CmbEntryType).Text = item.Cells["EntryType"].Text.ToString();
				CmbWarehouseOutput.Value = item.Cells["WareHouseId"].Value.ToString();
				((Control)(object)CmbWarehouseOutput).Text = item.Cells["WareHouse"].Text.ToString();
				CmbLotOutPut.Value = item.Cells["JobLotId"].Value.ToString();
				((Control)(object)CmbLotOutPut).Text = item.Cells["JobLot"].Text.ToString();
				CmbPackTypeOutPut.Value = item.Cells["PackingTypeId"].Value.ToString();
				((Control)(object)CmbPackTypeOutPut).Text = item.Cells["PackingType"].Text.ToString();
				CmbItemOutput.Value = item.Cells["ItemId"].Value.ToString();
				((Control)(object)CmbItemOutput).Text = item.Cells["Item"].Text.ToString();
				CmbUomOutPut.Value = item.Cells["ItemUOMId"].Value.ToString();
				((Control)(object)CmbUomOutPut).Text = item.Cells["ItemUOM"].Text.ToString();
				((Control)(object)CmbCropYearOutPut).Text = item.Cells["CropYear"].Text.ToString();
				txtQtyOutPut.Text = item.Cells["Quantity"].Text.ToString();
				txtNetWeightOutPut.Text = item.Cells["Weight"].Text.ToString();
				PreWeight = Conversion.ToDouble((object)txtNetWeightOutPut.Text);
				txtRateOutPut.Text = item.Cells["Rate"].Value.ToString();
				CmbRateUomOutPut.Value = item.Cells["RateUOMId"].Value.ToString();
				((Control)(object)CmbRateUomOutPut).Text = item.Cells["RateUOM"].Text.ToString();
				txtAmountOutPut.Text = item.Cells["ItemAmount"].Value.ToString();
				txtRemarksOutPut.Text = item.Cells["Remarks"].Text.ToString();
				CmbStockAccount.Value = item.Cells["StockAcId"].Value.ToString();
				((Control)(object)CmbStockAccount).Text = item.Cells["StockAc"].Text.ToString();
				btnAddOutPut.Visible = false;
				btnUpdateDetailOutPut.Visible = true;
				btnCancelOutPut.Visible = true;
				CmbEntryType.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdateDetailOutPut_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValidationOfDetailPortionOutPut())
			{
				tableByProduct.Rows[updateDetailIndex]["EntryType"] = ((Control)(object)CmbEntryType).Text.Trim();
				tableByProduct.Rows[updateDetailIndex]["WareHouseId"] = CmbWarehouseOutput.Value;
				tableByProduct.Rows[updateDetailIndex]["WareHouse"] = ((Control)(object)CmbWarehouseOutput).Text.Trim();
				tableByProduct.Rows[updateDetailIndex]["JobLotId"] = CmbLotOutPut.Value;
				tableByProduct.Rows[updateDetailIndex]["JobLot"] = ((Control)(object)CmbLotOutPut).Text.Trim();
				tableByProduct.Rows[updateDetailIndex]["PackingTypeId"] = CmbPackTypeOutPut.Value;
				tableByProduct.Rows[updateDetailIndex]["PackingType"] = ((Control)(object)CmbPackTypeOutPut).Text.Trim();
				tableByProduct.Rows[updateDetailIndex]["ItemId"] = CmbItemOutput.Value;
				tableByProduct.Rows[updateDetailIndex]["Item"] = ((Control)(object)CmbItemOutput).Text.Trim();
				tableByProduct.Rows[updateDetailIndex]["ItemUOMId"] = CmbUomOutPut.Value;
				tableByProduct.Rows[updateDetailIndex]["ItemUOM"] = ((Control)(object)CmbUomOutPut).Text.Trim();
				tableByProduct.Rows[updateDetailIndex]["CropYear"] = ((Control)(object)CmbCropYearOutPut).Text.Trim();
				tableByProduct.Rows[updateDetailIndex]["Quantity"] = txtQtyOutPut.Text.Trim();
				tableByProduct.Rows[updateDetailIndex]["Weight"] = txtNetWeightOutPut.Text.Trim();
				if (DiffNetWeight < ConsumedWeight + Conversion.ToDouble((object)txtNetWeightOutPut.Text) - PreWeight)
				{
					MessageBox.Show("Please Check Input Weight / " + DiffNetWeight);
					return;
				}
				ConsumedWeight = ConsumedWeight + Conversion.ToDouble((object)txtNetWeightOutPut.Text) - PreWeight;
				PreWeight = 0.0;
				tableByProduct.Rows[updateDetailIndex]["Rate"] = txtRateOutPut.Text.Trim();
				tableByProduct.Rows[updateDetailIndex]["RateUOM"] = ((Control)(object)CmbRateUomOutPut).Text.Trim();
				tableByProduct.Rows[updateDetailIndex]["RateUOMId"] = CmbRateUomOutPut.Value;
				tableByProduct.Rows[updateDetailIndex]["ItemAmount"] = txtAmountOutPut.Text.Trim();
				tableByProduct.Rows[updateDetailIndex]["Remarks"] = txtRemarksOutPut.Text.Trim();
				tableByProduct.Rows[updateDetailIndex]["StockAcId"] = CmbStockAccount.Value;
				tableByProduct.Rows[updateDetailIndex]["StockAc"] = ((Control)(object)CmbStockAccount).Text.Trim();
				btnUpdateDetailOutPut.Visible = false;
				btnCancelOutPut.Visible = false;
				btnAddOutPut.Visible = true;
				resetDetailOutPut();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbUOM_Leave(object sender, EventArgs e)
	{
		TotalWeightInput();
		TotalAmountInput();
	}

	private void txtQty_TextChanged(object sender, EventArgs e)
	{
		if (btnsave.Visible)
		{
			TotalWeightInput();
			TotalAmountInput();
		}
		else
		{
			TotalAmountInput();
		}
	}

	private void txtRateInput_TextChanged(object sender, EventArgs e)
	{
		TotalWeightInput();
		TotalAmountInput();
	}

	private void CmbRateUOMInput_Leave(object sender, EventArgs e)
	{
		TotalWeightInput();
		TotalAmountInput();
	}

	private void txtNetweight_KeyPress(object sender, KeyPressEventArgs e)
	{
		e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
	}

	private void txtRateInput_KeyPress(object sender, KeyPressEventArgs e)
	{
		e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
	}

	private void grdStockConversionProductionDetail_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		try
		{
			grdStockConversionProductionDetail.UpdateData();
			GridEXRow item = grdStockConversionProductionDetail.CurrentRow;
			if (e.Column.Key == "Quantity")
			{
				double Qty = Conversion.ToDouble((object)item.Cells["Quantity"].Value.ToString());
				double Uom = Conversion.ToDouble((object)item.Cells["ItemUOM"].Value.ToString());
				item.Cells["Weight"].Value = Qty * Uom;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtdocnumber_Leave(object sender, EventArgs e)
	{
	}

	private void txtDocNoOutPut_Leave(object sender, EventArgs e)
	{
	}

	private void txtAmountInput_KeyPress(object sender, KeyPressEventArgs e)
	{
		e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
	}

	private void CmbCropyr_Leave(object sender, EventArgs e)
	{
		GetAvgRate();
	}

	private void cmbLot_Leave(object sender, EventArgs e)
	{
		GetAvgRate();
	}

	private void btnPrintOutPut_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.ProductionInPutAndOutPut601(RecIdOutPut);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbJobOrderNo_Leave(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		try
		{
			DataTable dt = InvProductionJobOrder.GetGlAccountsByJobOrderId(new InvProductionJobOrder
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Id = Conversion.ToInt(CmbJobOrderNo.Value)
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbWorkInProcess, "WorkInProccessAcId", "WorkInProcessAc", "WorkInProcessAc", false);
				DDL.BindDDLNew(dt, CmbWIPItem, "WipItemId", "ItemName", "ItemName", false);
				((UltraGridBase)CmbWorkInProcess).Rows[0].Activate();
				((UltraGridBase)CmbWIPItem).Rows[0].Activate();
			}
			if (btnsave.Visible && btnsave.Enabled)
			{
				GetItemsbyJobOrderId();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetItemsbyJobOrderId()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		try
		{
			DataTable dt = InvProductionJobOrder.GetItemsByJobOrderId(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				JobOrderId = Conversion.ToInt(CmbJobOrderNo.Value)
			});
			if (dt.Rows.Count > 0)
			{
				table.Rows.Clear();
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToString(dt.Rows[i]["type"]) == "Input")
					{
						table.Rows.Add(0, 0, 0, 0, dt.Rows[i]["WarehouseId"], dt.Rows[i]["WareHouseName"], dt.Rows[i]["ItemId"], dt.Rows[i]["ItemName"], dt.Rows[i]["UomSchIdQty"], dt.Rows[i]["Equivalent"], dt.Rows[i]["CropYearId"], dt.Rows[i]["CropYear"], dt.Rows[i]["JobLotId"], dt.Rows[i]["JobLotDescription"], dt.Rows[i]["PackingTypeId"], dt.Rows[i]["PackTypeDesc"], dt.Rows[i]["Qty"], dt.Rows[i]["NetWeight"], 0, 0, 0, 0, dt.Rows[i]["RemarksInputDt"]);
					}
				}
				grdStockConversionProductionDetail.DataSource = table;
				grdStockConversionProductionDetail.RetrieveStructure();
				gridsettings();
			}
			else
			{
				grdStockConversionProductionDetail.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbJobOrderNoOutput_Leave(object sender, EventArgs e)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Expected O, but got Unknown
		try
		{
			if (btnSaveOutPut.Visible && btnSaveOutPut.Enabled)
			{
				GetItemsbyJobOrderIdOutput();
			}
			DataTable dtSummeryValue = InvFoodProduction.GetSummeryValues(new ReportsParameters
			{
				JobOrderId = Conversion.ToInt(CmbJobOrderNoOutput.Value),
				NetWeight = Conversion.ToDouble((object)txtFGWeight.Text.Trim())
			});
			if (dtSummeryValue.Rows.Count > 0 && dtSummeryValue != null)
			{
				for (int i = 0; i < dtSummeryValue.Rows.Count; i++)
				{
					if (dtSummeryValue.Rows[i]["TranType"].ToString() == "Output Finish Goods")
					{
						FinishGoodsNetWeight = Conversion.ToDouble(dtSummeryValue.Rows[i]["NetWeight"]);
					}
					if (dtSummeryValue.Rows[i]["TranType"].ToString() == "OutPut By Product")
					{
						ByProductNetWeight = Conversion.ToDouble(dtSummeryValue.Rows[i]["NetWeight"]);
					}
					if (dtSummeryValue.Rows[i]["TranType"].ToString() == "Input Raw Material")
					{
						InputNetWeight = Conversion.ToDouble(dtSummeryValue.Rows[i]["NetWeight"]);
					}
					DiffNetWeight = InputNetWeight - ByProductNetWeight - FinishGoodsNetWeight;
				}
			}
			DataTable dtWIP = InvProductionJobOrder.GetGlAccountsByJobOrderId(new InvProductionJobOrder
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Id = Conversion.ToInt(CmbJobOrderNoOutput.Value)
			});
			if (dtWIP.Rows.Count > 0)
			{
				CmbStockAccount.Value = Conversion.ToInt(dtWIP.Rows[0]["ByProductacId"]);
				((Control)(object)CmbStockAccount).Text = Conversion.ToString(dtWIP.Rows[0]["ByProductionAc"]);
				DDL.BindDDLNew(dtWIP, CmbWorkInProcess, "WorkInProccessAcId", "WorkInProcessAc", "WorkInProcessAc", false);
				DDL.BindDDLNew(dtWIP, CmbWIPItem, "WipItemId", "ItemName", "ItemName", false);
				((UltraGridBase)CmbWorkInProcess).Rows[0].Activate();
				((UltraGridBase)CmbWIPItem).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetItemsbyJobOrderIdOutput()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		try
		{
			DataTable dt = InvProductionJobOrder.GetItemsByJobOrderId(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				JobOrderId = Conversion.ToInt(CmbJobOrderNoOutput.Value)
			});
			if (dt.Rows.Count > 0)
			{
				tableByProduct.Rows.Clear();
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToString(dt.Rows[i]["type"]) == "Output")
					{
						tableByProduct.Rows.Add(0, 1, dt.Rows[i]["WarehouseId"], dt.Rows[i]["WareHouseName"], dt.Rows[i]["ItemId"], dt.Rows[i]["ItemName"], dt.Rows[i]["UomSchIdQty"], dt.Rows[i]["Equivalent"], dt.Rows[i]["CropYearId"], dt.Rows[i]["CropYear"], dt.Rows[i]["JobLotId"], dt.Rows[i]["JobLotDescription"], dt.Rows[i]["PackingTypeId"], dt.Rows[i]["PackTypeDesc"], dt.Rows[i]["Qty"], dt.Rows[i]["NetWeight"], 0, 0, 0, 0, dt.Rows[i]["RemarksInputDt"]);
					}
				}
				grdByProduct.DataSource = tableByProduct;
				grdByProduct.RetrieveStructure();
				gridsettingsOutPut();
			}
			else
			{
				grdByProduct.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (tabControl1.SelectedIndex == 4)
		{
			GridHistory();
		}
		if (tabControl1.SelectedIndex == 5)
		{
			JobOrderNoFillForSummery();
		}
	}

	private void VoucherReprot(int Id)
	{
		try
		{
			CommonServices.VoucherReport_118(Id);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnVoucherReport_Click(object sender, EventArgs e)
	{
		try
		{
			VoucherReprot(VoucherHeadId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnVoucherOutPut_Click(object sender, EventArgs e)
	{
		try
		{
			VoucherReprot(VoucherHeadId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnVoucherOH_Click(object sender, EventArgs e)
	{
		try
		{
			VoucherReprot(VoucherHeadId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnVoucherOH_Click_1(object sender, EventArgs e)
	{
		try
		{
			VoucherReprot(VoucherHeadId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtFGWeight_TextChanged(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Expected O, but got Unknown
		try
		{
			DataTable dt = InvFoodProduction.GetSummeryValues(new ReportsParameters
			{
				JobOrderId = Conversion.ToInt(CmbJobOrderNoOutput.Value),
				NetWeight = Conversion.ToDouble((object)txtFGWeight.Text.Trim())
			});
			if (dt.Rows.Count > 0 && dt != null)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (dt.Rows[i]["TranType"].ToString() == "Output Finish Goods")
					{
						AvgRate = Conversion.ToDouble(dt.Rows[i]["AvgRate"]);
						txtFGRate.Text = Math.Round(Conversion.ToDouble(dt.Rows[i]["AvgRate"]), 3).ToString();
					}
				}
			}
			else
			{
				txtFGRate.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ProporationFinishGoods()
	{
		try
		{
			if (txtFGWeight.Text == string.Empty || txtFGWeight.Text == "0")
			{
				MessageBox.Show("Please Enter Finish Goods Weight");
				return;
			}
			grdByProduct.UpdateData();
			GridEXRow[] rows = grdByProduct.GetRows();
			foreach (GridEXRow r in rows)
			{
				r.BeginEdit();
				if (r.Cells["EntryType"].Value.ToString() == "FinishGoods")
				{
					double num = Conversion.ToDouble((object)r.Cells["Weight"].Value.ToString());
					double num2 = Conversion.ToDouble((object)AvgRate);
					double RateUOM = Conversion.ToDouble((object)r.Cells["RateUOM"].Value.ToString());
					double FgRate = num2 * RateUOM;
					r.Cells["Rate"].Value = FgRate;
					double Amount = num / RateUOM * Conversion.ToDouble((object)r.Cells["Rate"].Value.ToString());
					r.Cells["ItemAmount"].Value = Math.Round(Amount, 2).ToString();
					grdByProduct.UpdateData();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnApplyRate_Click(object sender, EventArgs e)
	{
		try
		{
			ProporationFinishGoods();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdByProduct_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		try
		{
			grdByProduct.UpdateData();
			GridEXRow item = grdByProduct.CurrentRow;
			if (e.Column.Key == "Quantity")
			{
				double Qty = Conversion.ToDouble((object)item.Cells["Quantity"].Value.ToString());
				double Uom = Conversion.ToDouble((object)item.Cells["ItemUOM"].Value.ToString());
				item.Cells["Weight"].Value = Qty * Uom;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdByProduct_CellEdited(object sender, ColumnActionEventArgs e)
	{
	}

	private void btnProductionRecoveryReport_Click(object sender, EventArgs e)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtSummerReport.Rows.Count > 0 && dtSummerReport != null)
			{
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dtSummerReport, "602-InvRptProductionSummery.rpt");
			}
			else
			{
				MessageBox.Show("Record Not Found For DisPlay");
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
			if (File.Exists(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)grdStockConversionProductionDetail).Name))
			{
				FileStream fs = new FileStream(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)grdStockConversionProductionDetail).Name, FileMode.OpenOrCreate, FileAccess.ReadWrite);
				grdStockConversionProductionDetail.LoadLayoutFile((Stream)fs);
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
			if (File.Exists(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)grdByProduct).Name))
			{
				FileStream fs = new FileStream(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)grdByProduct).Name, FileMode.OpenOrCreate, FileAccess.ReadWrite);
				grdByProduct.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void DatagridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Edit")
			{
				if (DatagridHistory.CurrentRow.Cells[4].Value.ToString() == "Input")
				{
					((Control)(object)CmbJobOrderNo).Enabled = false;
					RecId = Conversion.ToInt((object)DatagridHistory.CurrentRow.Cells[0].Value.ToString());
					ReadByIdInput(RecId);
				}
				else
				{
					((Control)(object)CmbJobOrderNoOutput).Enabled = false;
					RecIdOutPut = Conversion.ToInt((object)DatagridHistory.CurrentRow.Cells[0].Value.ToString());
					ReadByIdOutPut(RecIdOutPut);
				}
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
			if (File.Exists(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)grdSummery).Name))
			{
				FileStream fs = new FileStream(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)grdSummery).Name, FileMode.OpenOrCreate, FileAccess.ReadWrite);
				grdSummery.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlGrdBar6_Load(object sender, EventArgs e)
	{
		try
		{
			if (File.Exists(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)grdSummeryReport).Name))
			{
				FileStream fs = new FileStream(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)grdSummeryReport).Name, FileMode.OpenOrCreate, FileAccess.ReadWrite);
				grdSummeryReport.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
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
		//IL_06fc: Expected O, but got Unknown
		//IL_06fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0703: Expected O, but got Unknown
		//IL_0703: Unknown result type (might be due to invalid IL or missing references)
		//IL_070a: Expected O, but got Unknown
		//IL_070a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0713: Expected O, but got Unknown
		//IL_0715: Unknown result type (might be due to invalid IL or missing references)
		//IL_071e: Expected O, but got Unknown
		//IL_0720: Unknown result type (might be due to invalid IL or missing references)
		//IL_0729: Expected O, but got Unknown
		//IL_072b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0734: Expected O, but got Unknown
		//IL_0736: Unknown result type (might be due to invalid IL or missing references)
		//IL_073f: Expected O, but got Unknown
		//IL_0741: Unknown result type (might be due to invalid IL or missing references)
		//IL_074a: Expected O, but got Unknown
		//IL_074c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0755: Expected O, but got Unknown
		//IL_0757: Unknown result type (might be due to invalid IL or missing references)
		//IL_0760: Expected O, but got Unknown
		//IL_0762: Unknown result type (might be due to invalid IL or missing references)
		//IL_076b: Expected O, but got Unknown
		//IL_076d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0776: Expected O, but got Unknown
		//IL_0778: Unknown result type (might be due to invalid IL or missing references)
		//IL_0781: Expected O, but got Unknown
		//IL_0783: Unknown result type (might be due to invalid IL or missing references)
		//IL_078c: Expected O, but got Unknown
		//IL_078e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0797: Expected O, but got Unknown
		//IL_0799: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a2: Expected O, but got Unknown
		//IL_07a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ad: Expected O, but got Unknown
		//IL_07af: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b8: Expected O, but got Unknown
		//IL_07ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c3: Expected O, but got Unknown
		//IL_07c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ce: Expected O, but got Unknown
		//IL_07d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d9: Expected O, but got Unknown
		//IL_07db: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e4: Expected O, but got Unknown
		//IL_07e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ef: Expected O, but got Unknown
		//IL_07f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07fa: Expected O, but got Unknown
		//IL_07fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0805: Expected O, but got Unknown
		//IL_0807: Unknown result type (might be due to invalid IL or missing references)
		//IL_0810: Expected O, but got Unknown
		//IL_0812: Unknown result type (might be due to invalid IL or missing references)
		//IL_081b: Expected O, but got Unknown
		//IL_081d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0826: Expected O, but got Unknown
		//IL_0828: Unknown result type (might be due to invalid IL or missing references)
		//IL_0831: Expected O, but got Unknown
		//IL_0833: Unknown result type (might be due to invalid IL or missing references)
		//IL_083c: Expected O, but got Unknown
		//IL_083e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0847: Expected O, but got Unknown
		//IL_0849: Unknown result type (might be due to invalid IL or missing references)
		//IL_0852: Expected O, but got Unknown
		//IL_0854: Unknown result type (might be due to invalid IL or missing references)
		//IL_085d: Expected O, but got Unknown
		//IL_085f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0868: Expected O, but got Unknown
		//IL_086a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0873: Expected O, but got Unknown
		//IL_0875: Unknown result type (might be due to invalid IL or missing references)
		//IL_087e: Expected O, but got Unknown
		//IL_0880: Unknown result type (might be due to invalid IL or missing references)
		//IL_0889: Expected O, but got Unknown
		//IL_088b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0894: Expected O, but got Unknown
		//IL_0896: Unknown result type (might be due to invalid IL or missing references)
		//IL_089f: Expected O, but got Unknown
		//IL_08a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_08aa: Expected O, but got Unknown
		//IL_08ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b5: Expected O, but got Unknown
		//IL_08b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c0: Expected O, but got Unknown
		//IL_08c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_08cb: Expected O, but got Unknown
		//IL_08cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d6: Expected O, but got Unknown
		//IL_08d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e1: Expected O, but got Unknown
		//IL_08e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ec: Expected O, but got Unknown
		//IL_08ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f7: Expected O, but got Unknown
		//IL_08f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0902: Expected O, but got Unknown
		//IL_0904: Unknown result type (might be due to invalid IL or missing references)
		//IL_090d: Expected O, but got Unknown
		//IL_090f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0918: Expected O, but got Unknown
		//IL_091a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0923: Expected O, but got Unknown
		//IL_0925: Unknown result type (might be due to invalid IL or missing references)
		//IL_092e: Expected O, but got Unknown
		//IL_0930: Unknown result type (might be due to invalid IL or missing references)
		//IL_0939: Expected O, but got Unknown
		//IL_093b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0944: Expected O, but got Unknown
		//IL_0946: Unknown result type (might be due to invalid IL or missing references)
		//IL_094f: Expected O, but got Unknown
		//IL_0951: Unknown result type (might be due to invalid IL or missing references)
		//IL_095a: Expected O, but got Unknown
		//IL_095c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0965: Expected O, but got Unknown
		//IL_0967: Unknown result type (might be due to invalid IL or missing references)
		//IL_0970: Expected O, but got Unknown
		//IL_0972: Unknown result type (might be due to invalid IL or missing references)
		//IL_097b: Expected O, but got Unknown
		//IL_097d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0986: Expected O, but got Unknown
		//IL_0988: Unknown result type (might be due to invalid IL or missing references)
		//IL_0991: Expected O, but got Unknown
		//IL_0993: Unknown result type (might be due to invalid IL or missing references)
		//IL_099c: Expected O, but got Unknown
		//IL_099e: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a7: Expected O, but got Unknown
		//IL_09a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b2: Expected O, but got Unknown
		//IL_09b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_09bd: Expected O, but got Unknown
		//IL_09bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c8: Expected O, but got Unknown
		//IL_09ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d3: Expected O, but got Unknown
		//IL_09d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_09de: Expected O, but got Unknown
		//IL_0a0d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a17: Expected O, but got Unknown
		//IL_0a2e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a38: Expected O, but got Unknown
		//IL_0a9c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa6: Expected O, but got Unknown
		//IL_0abd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac7: Expected O, but got Unknown
		//IL_0ad3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0add: Expected O, but got Unknown
		//IL_0aff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b09: Expected O, but got Unknown
		//IL_0b2b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b35: Expected O, but got Unknown
		//IL_0b4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b56: Expected O, but got Unknown
		//IL_0bdb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be5: Expected O, but got Unknown
		//IL_0bf1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bfb: Expected O, but got Unknown
		//IL_0bfc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c06: Expected O, but got Unknown
		//IL_0cb7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cc1: Expected O, but got Unknown
		//IL_0d04: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d0e: Expected O, but got Unknown
		//IL_0d0f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d19: Expected O, but got Unknown
		//IL_0d1a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d24: Expected O, but got Unknown
		//IL_0d5c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d66: Expected O, but got Unknown
		//IL_0dbf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dc9: Expected O, but got Unknown
		//IL_0dd5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ddf: Expected O, but got Unknown
		//IL_0de0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dea: Expected O, but got Unknown
		//IL_0df6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e00: Expected O, but got Unknown
		//IL_0e01: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e0b: Expected O, but got Unknown
		//IL_0eb1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ebb: Expected O, but got Unknown
		//IL_0f56: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f60: Expected O, but got Unknown
		//IL_0f82: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f8c: Expected O, but got Unknown
		//IL_101c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1026: Expected O, but got Unknown
		//IL_1027: Unknown result type (might be due to invalid IL or missing references)
		//IL_1031: Expected O, but got Unknown
		//IL_10b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_10c0: Expected O, but got Unknown
		//IL_10ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_10f7: Expected O, but got Unknown
		//IL_10f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_1102: Expected O, but got Unknown
		//IL_11df: Unknown result type (might be due to invalid IL or missing references)
		//IL_11e9: Expected O, but got Unknown
		//IL_11f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_11ff: Expected O, but got Unknown
		//IL_12e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_12f1: Expected O, but got Unknown
		//IL_12fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_1307: Expected O, but got Unknown
		//IL_1313: Unknown result type (might be due to invalid IL or missing references)
		//IL_131d: Expected O, but got Unknown
		//IL_1a69: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a73: Expected O, but got Unknown
		//IL_60b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_60c0: Expected O, but got Unknown
		//IL_60cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_60d7: Expected O, but got Unknown
		//IL_dc9a: Unknown result type (might be due to invalid IL or missing references)
		//IL_dca4: Expected O, but got Unknown
		this.components = new System.ComponentModel.Container();
		GridEXLayout grdStockConversionProductionDetail_DesignTimeLayout = new GridEXLayout();
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
		Appearance appearance145 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
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
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Production.frmFoodProduction));
		GridEXLayout grdByProduct_DesignTimeLayout = new GridEXLayout();
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
		Appearance appearance35 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance157 = new Appearance();
		Appearance appearance168 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance190 = new Appearance();
		Appearance appearance201 = new Appearance();
		Appearance appearance212 = new Appearance();
		Appearance appearance224 = new Appearance();
		Appearance appearance235 = new Appearance();
		Appearance appearance239 = new Appearance();
		Appearance appearance240 = new Appearance();
		Appearance appearance241 = new Appearance();
		Appearance appearance242 = new Appearance();
		Appearance appearance243 = new Appearance();
		Appearance appearance310 = new Appearance();
		Appearance appearance311 = new Appearance();
		Appearance appearance312 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance297 = new Appearance();
		Appearance appearance298 = new Appearance();
		Appearance appearance299 = new Appearance();
		Appearance appearance300 = new Appearance();
		Appearance appearance301 = new Appearance();
		Appearance appearance303 = new Appearance();
		Appearance appearance304 = new Appearance();
		Appearance appearance305 = new Appearance();
		Appearance appearance306 = new Appearance();
		Appearance appearance307 = new Appearance();
		Appearance appearance308 = new Appearance();
		Appearance appearance309 = new Appearance();
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
		Appearance appearance271 = new Appearance();
		Appearance appearance272 = new Appearance();
		Appearance appearance273 = new Appearance();
		Appearance appearance274 = new Appearance();
		Appearance appearance275 = new Appearance();
		Appearance appearance276 = new Appearance();
		Appearance appearance277 = new Appearance();
		Appearance appearance278 = new Appearance();
		Appearance appearance279 = new Appearance();
		Appearance appearance281 = new Appearance();
		Appearance appearance282 = new Appearance();
		Appearance appearance283 = new Appearance();
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
		Appearance appearance245 = new Appearance();
		Appearance appearance246 = new Appearance();
		Appearance appearance248 = new Appearance();
		Appearance appearance249 = new Appearance();
		Appearance appearance250 = new Appearance();
		Appearance appearance251 = new Appearance();
		Appearance appearance252 = new Appearance();
		Appearance appearance253 = new Appearance();
		Appearance appearance254 = new Appearance();
		Appearance appearance255 = new Appearance();
		Appearance appearance256 = new Appearance();
		GridEXLayout grdPackingMaterial_DesignTimeLayout = new GridEXLayout();
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
		Appearance appearance159 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance169 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance171 = new Appearance();
		GridEXLayout grdOverHead_DesignTimeLayout = new GridEXLayout();
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
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout DatagridHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdSummeryReport_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdSummery_DesignTimeLayout = new GridEXLayout();
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
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.InPut = new System.Windows.Forms.TabPage();
		this.panel5 = new System.Windows.Forms.Panel();
		this.grdStockConversionProductionDetail = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel29 = new System.Windows.Forms.Panel();
		this.cmbUOM = new UltraCombo();
		this.label21 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.txtdeailRemarks = new System.Windows.Forms.TextBox();
		this.label8 = new System.Windows.Forms.Label();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.label10 = new System.Windows.Forms.Label();
		this.txtNetweight = new System.Windows.Forms.TextBox();
		this.label14 = new System.Windows.Forms.Label();
		this.btnCancelDetail = new System.Windows.Forms.Button();
		this.cmbBagType = new UltraCombo();
		this.label32 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.cmbGodown = new UltraCombo();
		this.label18 = new System.Windows.Forms.Label();
		this.cmbLot = new UltraCombo();
		this.txtQty = new System.Windows.Forms.TextBox();
		this.lblRateInput = new System.Windows.Forms.Label();
		this.txtAmountInput = new System.Windows.Forms.TextBox();
		this.cmbItem = new UltraCombo();
		this.label7 = new System.Windows.Forms.Label();
		this.txtRateInput = new System.Windows.Forms.TextBox();
		this.lblAmountInput = new System.Windows.Forms.Label();
		this.CmbCropyr = new UltraCombo();
		this.label44 = new System.Windows.Forms.Label();
		this.lblRateUOMInput = new System.Windows.Forms.Label();
		this.CmbRateUOMInput = new UltraCombo();
		this.Add = new System.Windows.Forms.Button();
		this.panel28 = new System.Windows.Forms.Panel();
		this.txtdocnumber = new System.Windows.Forms.TextBox();
		this.label77 = new System.Windows.Forms.Label();
		this.txtRemarks = new System.Windows.Forms.TextBox();
		this.btnLoadInvoices = new System.Windows.Forms.Button();
		this.label6 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.txtAvgStock = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.txtDocdate = new System.Windows.Forms.DateTimePicker();
		this.label22 = new System.Windows.Forms.Label();
		this.CmbJobOrderNo = new UltraCombo();
		this.label23 = new System.Windows.Forms.Label();
		this.CmbWIPItem = new UltraCombo();
		this.CmbWorkInProcess = new UltraCombo();
		this.txtAvgRate = new System.Windows.Forms.TextBox();
		this.label25 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.toolStripButton5 = new System.Windows.Forms.ToolStripButton();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
		this.Print = new System.Windows.Forms.ToolStripButton();
		this.btnVoucherReport = new System.Windows.Forms.ToolStripButton();
		this.OutPut = new System.Windows.Forms.TabPage();
		this.panel3 = new System.Windows.Forms.Panel();
		this.panel14 = new System.Windows.Forms.Panel();
		this.grdByProduct = new GridEX();
		this.panel12 = new System.Windows.Forms.Panel();
		this.panel31 = new System.Windows.Forms.Panel();
		this.txtRemarksOutPut = new System.Windows.Forms.TextBox();
		this.label70 = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.label69 = new System.Windows.Forms.Label();
		this.CmbStockAccount = new UltraCombo();
		this.CmbPackTypeOutPut = new UltraCombo();
		this.CmbEntryType = new UltraCombo();
		this.label45 = new System.Windows.Forms.Label();
		this.txtAmountOutPut = new System.Windows.Forms.TextBox();
		this.label43 = new System.Windows.Forms.Label();
		this.lblAmountOUtPUt = new System.Windows.Forms.Label();
		this.label36 = new System.Windows.Forms.Label();
		this.CmbRateUomOutPut = new UltraCombo();
		this.txtQtyOutPut = new System.Windows.Forms.TextBox();
		this.lblRateUOMOUtPut = new System.Windows.Forms.Label();
		this.label24 = new System.Windows.Forms.Label();
		this.txtRateOutPut = new System.Windows.Forms.TextBox();
		this.label20 = new System.Windows.Forms.Label();
		this.lblRateOutPUt = new System.Windows.Forms.Label();
		this.btnAddOutPut = new System.Windows.Forms.Button();
		this.btnCancelOutPut = new System.Windows.Forms.Button();
		this.CmbCropYearOutPut = new UltraCombo();
		this.btnUpdateDetailOutPut = new System.Windows.Forms.Button();
		this.CmbItemOutput = new UltraCombo();
		this.CmbLotOutPut = new UltraCombo();
		this.label9 = new System.Windows.Forms.Label();
		this.CmbWarehouseOutput = new UltraCombo();
		this.CmbUomOutPut = new UltraCombo();
		this.label13 = new System.Windows.Forms.Label();
		this.txtNetWeightOutPut = new System.Windows.Forms.TextBox();
		this.label11 = new System.Windows.Forms.Label();
		this.panel30 = new System.Windows.Forms.Panel();
		this.txtDocNoOutPut = new System.Windows.Forms.TextBox();
		this.btnApplyRate = new System.Windows.Forms.Button();
		this.txtRemarksHeaderOutPut = new System.Windows.Forms.TextBox();
		this.txtFGRate = new System.Windows.Forms.TextBox();
		this.label19 = new System.Windows.Forms.Label();
		this.label76 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.txtFGWeight = new System.Windows.Forms.TextBox();
		this.label15 = new System.Windows.Forms.Label();
		this.label75 = new System.Windows.Forms.Label();
		this.DocDateOutPut = new System.Windows.Forms.DateTimePicker();
		this.CmbJobOrderNoOutput = new UltraCombo();
		this.label60 = new System.Windows.Forms.Label();
		this.panel6 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar4 = new grid.User_Conrtols.CtrlGrdBar();
		this.toolStrip6 = new System.Windows.Forms.ToolStrip();
		this.toolStripButton8 = new System.Windows.Forms.ToolStripLabel();
		this.toolStripButton10 = new System.Windows.Forms.ToolStripButton();
		this.btnSaveOutPut = new System.Windows.Forms.ToolStripButton();
		this.btnUpdateOutPut = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
		this.btnPrintOutPut = new System.Windows.Forms.ToolStripButton();
		this.btnVoucherOutPut = new System.Windows.Forms.ToolStripButton();
		this.PackingMaterial = new System.Windows.Forms.TabPage();
		this.panel7 = new System.Windows.Forms.Panel();
		this.panel9 = new System.Windows.Forms.Panel();
		this.grdPackingMaterial = new GridEX();
		this.panel24 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.label35 = new System.Windows.Forms.Label();
		this.cmbJobOrderPackingMaterial = new UltraCombo();
		this.txtremarksPackingMaterial = new System.Windows.Forms.TextBox();
		this.label63 = new System.Windows.Forms.Label();
		this.txtDocNoPackingMaterial = new System.Windows.Forms.TextBox();
		this.label62 = new System.Windows.Forms.Label();
		this.label37 = new System.Windows.Forms.Label();
		this.txtAmountPackingMaterial = new System.Windows.Forms.TextBox();
		this.label55 = new System.Windows.Forms.Label();
		this.label34 = new System.Windows.Forms.Label();
		this.label41 = new System.Windows.Forms.Label();
		this.txtdocdatePackingMaterial = new System.Windows.Forms.DateTimePicker();
		this.txtPackingMaterialRate = new System.Windows.Forms.TextBox();
		this.label40 = new System.Windows.Forms.Label();
		this.txtPackingMaterialQTY = new System.Windows.Forms.TextBox();
		this.cmbItemPackingMaterial = new UltraCombo();
		this.cmbUOMPackingMaterial = new UltraCombo();
		this.label42 = new System.Windows.Forms.Label();
		this.panel22 = new System.Windows.Forms.Panel();
		this.toolStrip4 = new System.Windows.Forms.ToolStrip();
		this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
		this.btnNewpackingmaterial = new System.Windows.Forms.ToolStripButton();
		this.btnsavepackingmaterial = new System.Windows.Forms.ToolStripButton();
		this.btnUpdatepackingmaterial = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.toolStripButton9 = new System.Windows.Forms.ToolStripButton();
		this.btnVoucherPM = new System.Windows.Forms.ToolStripButton();
		this.OverHead = new System.Windows.Forms.TabPage();
		this.panel8 = new System.Windows.Forms.Panel();
		this.grdOverHead = new GridEX();
		this.panel27 = new System.Windows.Forms.Panel();
		this.label39 = new System.Windows.Forms.Label();
		this.label58 = new System.Windows.Forms.Label();
		this.txtLedgerRemarksOverHead = new System.Windows.Forms.TextBox();
		this.cmbJobOrderOverHead = new UltraCombo();
		this.cmbChartofAccountOverHead = new UltraCombo();
		this.label59 = new System.Windows.Forms.Label();
		this.label57 = new System.Windows.Forms.Label();
		this.txtDocDateOverHead = new System.Windows.Forms.DateTimePicker();
		this.label38 = new System.Windows.Forms.Label();
		this.label56 = new System.Windows.Forms.Label();
		this.txtDocNoOverHead = new System.Windows.Forms.TextBox();
		this.txtExpAmountOverHead = new System.Windows.Forms.TextBox();
		this.panel25 = new System.Windows.Forms.Panel();
		this.toolStrip5 = new System.Windows.Forms.ToolStrip();
		this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
		this.btnNewOverHead = new System.Windows.Forms.ToolStripButton();
		this.btnSaveOverHead = new System.Windows.Forms.ToolStripButton();
		this.btnUpdateOverHead = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
		this.btnPrintOverHead = new System.Windows.Forms.ToolStripButton();
		this.btnVoucherOH = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel10 = new System.Windows.Forms.Panel();
		this.panel13 = new System.Windows.Forms.Panel();
		this.panel17 = new System.Windows.Forms.Panel();
		this.grdDetail = new GridEX();
		this.panel16 = new System.Windows.Forms.Panel();
		this.DatagridHistory = new GridEX();
		this.panel15 = new System.Windows.Forms.Panel();
		this.btnSearch = new System.Windows.Forms.Button();
		this.label50 = new System.Windows.Forms.Label();
		this.txtDocNumberTo = new System.Windows.Forms.TextBox();
		this.txtToDate = new System.Windows.Forms.DateTimePicker();
		this.label52 = new System.Windows.Forms.Label();
		this.txtfromDate = new System.Windows.Forms.DateTimePicker();
		this.label48 = new System.Windows.Forms.Label();
		this.label49 = new System.Windows.Forms.Label();
		this.txtDocNoFrom = new System.Windows.Forms.TextBox();
		this.panel11 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.toolStripButton7 = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.panel18 = new System.Windows.Forms.Panel();
		this.panel21 = new System.Windows.Forms.Panel();
		this.panel26 = new System.Windows.Forms.Panel();
		this.grdSummeryReport = new GridEX();
		this.panel23 = new System.Windows.Forms.Panel();
		this.grdSummery = new GridEX();
		this.panel20 = new System.Windows.Forms.Panel();
		this.cmbsummeryJobOrderNo = new UltraCombo();
		this.label68 = new System.Windows.Forms.Label();
		this.panel19 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar5 = new grid.User_Conrtols.CtrlGrdBar();
		this.ctrlGrdBar6 = new grid.User_Conrtols.CtrlGrdBar();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.toolStripButton6 = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.btnProductionRecoveryReport = new System.Windows.Forms.ToolStripButton();
		this.timer1 = new System.Windows.Forms.Timer(this.components);
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.InPut.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdStockConversionProductionDetail).BeginInit();
		this.panel4.SuspendLayout();
		this.panel29.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbUOM).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbBagType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbGodown).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbLot).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropyr).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRateUOMInput).BeginInit();
		this.panel28.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbJobOrderNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWIPItem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWorkInProcess).BeginInit();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.OutPut.SuspendLayout();
		this.panel3.SuspendLayout();
		this.panel14.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdByProduct).BeginInit();
		this.panel12.SuspendLayout();
		this.panel31.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbStockAccount).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackTypeOutPut).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbEntryType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRateUomOutPut).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYearOutPut).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemOutput).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbLotOutPut).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWarehouseOutput).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbUomOutPut).BeginInit();
		this.panel30.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbJobOrderNoOutput).BeginInit();
		this.panel6.SuspendLayout();
		this.toolStrip6.SuspendLayout();
		this.PackingMaterial.SuspendLayout();
		this.panel7.SuspendLayout();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPackingMaterial).BeginInit();
		this.panel24.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbJobOrderPackingMaterial).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemPackingMaterial).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbUOMPackingMaterial).BeginInit();
		this.panel22.SuspendLayout();
		this.toolStrip4.SuspendLayout();
		this.OverHead.SuspendLayout();
		this.panel8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdOverHead).BeginInit();
		this.panel27.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbJobOrderOverHead).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbChartofAccountOverHead).BeginInit();
		this.panel25.SuspendLayout();
		this.toolStrip5.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel10.SuspendLayout();
		this.panel13.SuspendLayout();
		this.panel17.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel16.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DatagridHistory).BeginInit();
		this.panel15.SuspendLayout();
		this.panel11.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		this.tabPage3.SuspendLayout();
		this.panel18.SuspendLayout();
		this.panel21.SuspendLayout();
		this.panel26.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdSummeryReport).BeginInit();
		this.panel23.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdSummery).BeginInit();
		this.panel20.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbsummeryJobOrderNo).BeginInit();
		this.panel19.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1239, 745);
		this.panel1.TabIndex = 0;
		this.tabControl1.Controls.Add(this.InPut);
		this.tabControl1.Controls.Add(this.OutPut);
		this.tabControl1.Controls.Add(this.PackingMaterial);
		this.tabControl1.Controls.Add(this.OverHead);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Controls.Add(this.tabPage3);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1239, 745);
		this.tabControl1.TabIndex = 1;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.InPut.BackColor = System.Drawing.Color.White;
		this.InPut.Controls.Add(this.panel5);
		this.InPut.Controls.Add(this.panel4);
		this.InPut.Controls.Add(this.panel2);
		this.InPut.Location = new System.Drawing.Point(4, 22);
		this.InPut.Name = "InPut";
		this.InPut.Padding = new System.Windows.Forms.Padding(3);
		this.InPut.Size = new System.Drawing.Size(1231, 719);
		this.InPut.TabIndex = 0;
		this.InPut.Text = "Input";
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.grdStockConversionProductionDetail);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(3, 207);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1225, 509);
		this.panel5.TabIndex = 3;
		((ScrollableBase)this.grdStockConversionProductionDetail).BackColor = System.Drawing.Color.White;
		grdStockConversionProductionDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdStockConversionProductionDetail.DesignTimeLayout = grdStockConversionProductionDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdStockConversionProductionDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdStockConversionProductionDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdStockConversionProductionDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdStockConversionProductionDetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdStockConversionProductionDetail).Name = "grdStockConversionProductionDetail";
		((ScrollableBase)this.grdStockConversionProductionDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdStockConversionProductionDetail).Size = new System.Drawing.Size(1225, 509);
		((System.Windows.Forms.Control)(object)this.grdStockConversionProductionDetail).TabIndex = 0;
		this.grdStockConversionProductionDetail.TotalRow = (InheritableBoolean)1;
		this.grdStockConversionProductionDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdStockConversionProductionDetail).VisualStyle = (VisualStyle)4;
		this.grdStockConversionProductionDetail.CellUpdated += new ColumnActionEventHandler(grdStockConversionProductionDetail_CellUpdated);
		((System.Windows.Forms.Control)(object)this.grdStockConversionProductionDetail).DoubleClick += new System.EventHandler(grdStockConversionProductionDetail_DoubleClick);
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.panel29);
		this.panel4.Controls.Add(this.panel28);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(3, 33);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1225, 174);
		this.panel4.TabIndex = 0;
		this.panel29.Controls.Add((System.Windows.Forms.Control)(object)this.cmbUOM);
		this.panel29.Controls.Add(this.label21);
		this.panel29.Controls.Add(this.label5);
		this.panel29.Controls.Add(this.txtdeailRemarks);
		this.panel29.Controls.Add(this.label8);
		this.panel29.Controls.Add(this.btnUpdateDetail);
		this.panel29.Controls.Add(this.label10);
		this.panel29.Controls.Add(this.txtNetweight);
		this.panel29.Controls.Add(this.label14);
		this.panel29.Controls.Add(this.btnCancelDetail);
		this.panel29.Controls.Add((System.Windows.Forms.Control)(object)this.cmbBagType);
		this.panel29.Controls.Add(this.label32);
		this.panel29.Controls.Add(this.label12);
		this.panel29.Controls.Add((System.Windows.Forms.Control)(object)this.cmbGodown);
		this.panel29.Controls.Add(this.label18);
		this.panel29.Controls.Add((System.Windows.Forms.Control)(object)this.cmbLot);
		this.panel29.Controls.Add(this.txtQty);
		this.panel29.Controls.Add(this.lblRateInput);
		this.panel29.Controls.Add(this.txtAmountInput);
		this.panel29.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItem);
		this.panel29.Controls.Add(this.label7);
		this.panel29.Controls.Add(this.txtRateInput);
		this.panel29.Controls.Add(this.lblAmountInput);
		this.panel29.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCropyr);
		this.panel29.Controls.Add(this.label44);
		this.panel29.Controls.Add(this.lblRateUOMInput);
		this.panel29.Controls.Add((System.Windows.Forms.Control)(object)this.CmbRateUOMInput);
		this.panel29.Controls.Add(this.Add);
		this.panel29.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel29.Location = new System.Drawing.Point(0, 60);
		this.panel29.Name = "panel29";
		this.panel29.Size = new System.Drawing.Size(1225, 114);
		this.panel29.TabIndex = 175;
		this.panel29.Visible = false;
		this.cmbUOM.AllowNull = (DefaultableBoolean)1;
		this.cmbUOM.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbUOM.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbUOM.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Appearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.cmbUOM).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbUOM).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance80).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance80).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbUOM).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbUOM).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance81;
		((SpecialBoxBase)((UltraGridBase)this.cmbUOM).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance82).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance82).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbUOM).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.cmbUOM).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbUOM).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance86).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance87).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance87).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance87).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance87;
		((AppearanceBase)appearance88).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.cmbUOM).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbUOM).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbUOM).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbUOM).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbUOM.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbUOM).Location = new System.Drawing.Point(580, 30);
		((System.Windows.Forms.Control)(object)this.cmbUOM).Name = "cmbUOM";
		((System.Windows.Forms.Control)(object)this.cmbUOM).Size = new System.Drawing.Size(87, 24);
		((System.Windows.Forms.Control)(object)this.cmbUOM).TabIndex = 8;
		((System.Windows.Forms.Control)(object)this.cmbUOM).Leave += new System.EventHandler(cmbUOM_Leave);
		this.label21.AutoSize = true;
		this.label21.Location = new System.Drawing.Point(18, 63);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(49, 13);
		this.label21.TabIndex = 113;
		this.label21.Text = "Remarks";
		this.label5.AutoSize = true;
		this.label5.Location = new System.Drawing.Point(18, 11);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(64, 13);
		this.label5.TabIndex = 67;
		this.label5.Text = "WareHouse";
		this.txtdeailRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdeailRemarks.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdeailRemarks.Location = new System.Drawing.Point(18, 84);
		this.txtdeailRemarks.Name = "txtdeailRemarks";
		this.txtdeailRemarks.Size = new System.Drawing.Size(497, 22);
		this.txtdeailRemarks.TabIndex = 17;
		this.label8.AutoSize = true;
		this.label8.Location = new System.Drawing.Point(672, 6);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(54, 13);
		this.label8.TabIndex = 70;
		this.label8.Text = "Crop Year";
		this.btnUpdateDetail.Location = new System.Drawing.Point(806, 84);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(61, 23);
		this.btnUpdateDetail.TabIndex = 19;
		this.btnUpdateDetail.Text = "Update ";
		this.btnUpdateDetail.UseVisualStyleBackColor = true;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.label10.AutoSize = true;
		this.label10.Location = new System.Drawing.Point(779, 5);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(22, 13);
		this.label10.TabIndex = 73;
		this.label10.Text = "Lot";
		this.txtNetweight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNetweight.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNetweight.Location = new System.Drawing.Point(1122, 31);
		this.txtNetweight.Name = "txtNetweight";
		this.txtNetweight.Size = new System.Drawing.Size(92, 22);
		this.txtNetweight.TabIndex = 13;
		this.txtNetweight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtNetweight_KeyPress);
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.ForeColor = System.Drawing.Color.Red;
		this.label14.Location = new System.Drawing.Point(941, 6);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(15, 20);
		this.label14.TabIndex = 77;
		this.label14.Text = "*";
		this.btnCancelDetail.Location = new System.Drawing.Point(868, 84);
		this.btnCancelDetail.Name = "btnCancelDetail";
		this.btnCancelDetail.Size = new System.Drawing.Size(55, 23);
		this.btnCancelDetail.TabIndex = 20;
		this.btnCancelDetail.Text = "Cancel";
		this.btnCancelDetail.UseVisualStyleBackColor = true;
		this.btnCancelDetail.Click += new System.EventHandler(btnCancelDetail_Click);
		this.cmbBagType.AllowNull = (DefaultableBoolean)1;
		this.cmbBagType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbBagType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbBagType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance145).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbBagType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.cmbBagType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbBagType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance147).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance147).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance147).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbBagType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance147;
		((AppearanceBase)appearance148).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBagType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance148;
		((SpecialBoxBase)((UltraGridBase)this.cmbBagType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance149).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance149).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance149).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBagType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance149;
		((UltraGridBase)this.cmbBagType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbBagType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance150).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbBagType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance150;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance151).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbBagType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.cmbBagType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbBagType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBagType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance152;
		((AppearanceBase)appearance153).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance153).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbBagType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance153;
		((UltraGridBase)this.cmbBagType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbBagType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance154).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance154).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance154).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance154).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBagType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance154;
		((AppearanceBase)appearance155).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbBagType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.cmbBagType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbBagType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance156).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbBagType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance156;
		((UltraGridBase)this.cmbBagType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbBagType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance158;
		((UltraGridBase)this.cmbBagType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbBagType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbBagType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbBagType).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbBagType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbBagType).Location = new System.Drawing.Point(897, 30);
		((System.Windows.Forms.Control)(object)this.cmbBagType).Name = "cmbBagType";
		((System.Windows.Forms.Control)(object)this.cmbBagType).Size = new System.Drawing.Size(135, 24);
		((System.Windows.Forms.Control)(object)this.cmbBagType).TabIndex = 11;
		this.label32.AutoSize = true;
		this.label32.Location = new System.Drawing.Point(1131, 10);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(61, 13);
		this.label32.TabIndex = 107;
		this.label32.Text = "Net Weight";
		this.label12.AutoSize = true;
		this.label12.Location = new System.Drawing.Point(892, 6);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(59, 13);
		this.label12.TabIndex = 76;
		this.label12.Text = "Pack Type";
		this.cmbGodown.AllowNull = (DefaultableBoolean)1;
		this.cmbGodown.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbGodown.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbGodown.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbGodown).DisplayLayout.Appearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.cmbGodown).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbGodown).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance93).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance93).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbGodown).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbGodown).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance94;
		((SpecialBoxBase)((UltraGridBase)this.cmbGodown).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance95).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance95).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbGodown).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.cmbGodown).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbGodown).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance96).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbGodown).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance97).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbGodown).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.cmbGodown).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbGodown).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbGodown).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance98;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance99).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbGodown).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.cmbGodown).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbGodown).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance100).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbGodown).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance102).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbGodown).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.cmbGodown).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbGodown).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbGodown).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.cmbGodown).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbGodown).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.cmbGodown).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbGodown).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbGodown).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbGodown).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbGodown.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbGodown).Location = new System.Drawing.Point(18, 30);
		((System.Windows.Forms.Control)(object)this.cmbGodown).Name = "cmbGodown";
		((System.Windows.Forms.Control)(object)this.cmbGodown).Size = new System.Drawing.Size(173, 24);
		((System.Windows.Forms.Control)(object)this.cmbGodown).TabIndex = 6;
		this.label18.AutoSize = true;
		this.label18.Location = new System.Drawing.Point(1033, 6);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(46, 13);
		this.label18.TabIndex = 79;
		this.label18.Text = "Quantity";
		this.cmbLot.AllowNull = (DefaultableBoolean)1;
		this.cmbLot.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbLot.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbLot.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbLot).DisplayLayout.Appearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.cmbLot).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbLot).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance106).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance106).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbLot).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance106;
		((AppearanceBase)appearance107).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLot).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance107;
		((SpecialBoxBase)((UltraGridBase)this.cmbLot).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance108).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance108).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance108).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLot).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.cmbLot).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbLot).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance109).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance109;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance110).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance111;
		((AppearanceBase)appearance114).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance114).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance115).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance115).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance115).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance116).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.cmbLot).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbLot).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbLot).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbLot).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbLot.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbLot).Location = new System.Drawing.Point(786, 30);
		((System.Windows.Forms.Control)(object)this.cmbLot).Name = "cmbLot";
		((System.Windows.Forms.Control)(object)this.cmbLot).Size = new System.Drawing.Size(109, 24);
		((System.Windows.Forms.Control)(object)this.cmbLot).TabIndex = 10;
		((System.Windows.Forms.Control)(object)this.cmbLot).Leave += new System.EventHandler(cmbLot_Leave);
		this.txtQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtQty.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtQty.Location = new System.Drawing.Point(1034, 31);
		this.txtQty.Name = "txtQty";
		this.txtQty.Size = new System.Drawing.Size(85, 22);
		this.txtQty.TabIndex = 12;
		this.txtQty.TextChanged += new System.EventHandler(txtQty_TextChanged);
		this.lblRateInput.AutoSize = true;
		this.lblRateInput.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblRateInput.Location = new System.Drawing.Point(519, 64);
		this.lblRateInput.Name = "lblRateInput";
		this.lblRateInput.Size = new System.Drawing.Size(33, 15);
		this.lblRateInput.TabIndex = 161;
		this.lblRateInput.Text = "Rate";
		this.txtAmountInput.BackColor = System.Drawing.Color.White;
		this.txtAmountInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAmountInput.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAmountInput.Location = new System.Drawing.Point(684, 85);
		this.txtAmountInput.Name = "txtAmountInput";
		this.txtAmountInput.ReadOnly = true;
		this.txtAmountInput.Size = new System.Drawing.Size(94, 21);
		this.txtAmountInput.TabIndex = 16;
		this.txtAmountInput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtAmountInput_KeyPress);
		this.cmbItem.AllowNull = (DefaultableBoolean)1;
		this.cmbItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.cmbItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance120).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance120).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance120).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance121).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance121;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance122).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance122).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance122).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance123).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance123;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance126;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance127).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance128).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance128).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance128).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance128;
		((AppearanceBase)appearance129).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItem).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItem).Location = new System.Drawing.Point(195, 30);
		((System.Windows.Forms.Control)(object)this.cmbItem).Name = "cmbItem";
		((System.Windows.Forms.Control)(object)this.cmbItem).Size = new System.Drawing.Size(379, 24);
		((System.Windows.Forms.Control)(object)this.cmbItem).TabIndex = 7;
		((System.Windows.Forms.Control)(object)this.cmbItem).Leave += new System.EventHandler(cmbItem_Leave);
		this.label7.AutoSize = true;
		this.label7.Location = new System.Drawing.Point(198, 6);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(27, 13);
		this.label7.TabIndex = 26;
		this.label7.Text = "Item";
		this.txtRateInput.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRateInput.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRateInput.Location = new System.Drawing.Point(520, 85);
		this.txtRateInput.Name = "txtRateInput";
		this.txtRateInput.Size = new System.Drawing.Size(70, 21);
		this.txtRateInput.TabIndex = 14;
		this.txtRateInput.TextChanged += new System.EventHandler(txtRateInput_TextChanged);
		this.txtRateInput.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtRateInput_KeyPress);
		this.lblAmountInput.AutoSize = true;
		this.lblAmountInput.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblAmountInput.Location = new System.Drawing.Point(683, 64);
		this.lblAmountInput.Name = "lblAmountInput";
		this.lblAmountInput.Size = new System.Drawing.Size(49, 15);
		this.lblAmountInput.TabIndex = 165;
		this.lblAmountInput.Text = "Amount";
		this.CmbCropyr.AllowNull = (DefaultableBoolean)1;
		this.CmbCropyr.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCropyr.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCropyr.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance132).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Appearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance133).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance133).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropyr).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance133;
		((AppearanceBase)appearance134).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance134;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropyr).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance136).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance136).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance136).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance137).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance137;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance138).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance139;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance140).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance141).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance141).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance141).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance141).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance141;
		((AppearanceBase)appearance142).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCropyr).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCropyr.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCropyr).Location = new System.Drawing.Point(669, 30);
		((System.Windows.Forms.Control)(object)this.CmbCropyr).Name = "CmbCropyr";
		((System.Windows.Forms.Control)(object)this.CmbCropyr).Size = new System.Drawing.Size(116, 24);
		((System.Windows.Forms.Control)(object)this.CmbCropyr).TabIndex = 9;
		((System.Windows.Forms.Control)(object)this.CmbCropyr).Leave += new System.EventHandler(CmbCropyr_Leave);
		this.label44.AutoSize = true;
		this.label44.Location = new System.Drawing.Point(584, 7);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(32, 13);
		this.label44.TabIndex = 26;
		this.label44.Text = "UOM";
		this.lblRateUOMInput.AutoSize = true;
		this.lblRateUOMInput.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblRateUOMInput.Location = new System.Drawing.Point(594, 64);
		this.lblRateUOMInput.Name = "lblRateUOMInput";
		this.lblRateUOMInput.Size = new System.Drawing.Size(65, 15);
		this.lblRateUOMInput.TabIndex = 163;
		this.lblRateUOMInput.Text = "Rate UOM";
		this.CmbRateUOMInput.AllowNull = (DefaultableBoolean)1;
		this.CmbRateUOMInput.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbRateUOMInput.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbRateUOMInput.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbRateUOMInput).DisplayLayout.Appearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.CmbRateUOMInput).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbRateUOMInput).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbRateUOMInput).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRateUOMInput).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance54;
		((SpecialBoxBase)((UltraGridBase)this.CmbRateUOMInput).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRateUOMInput).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbRateUOMInput).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbRateUOMInput).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbRateUOMInput).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbRateUOMInput).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbRateUOMInput).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbRateUOMInput).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRateUOMInput).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance60).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbRateUOMInput).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.CmbRateUOMInput).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbRateUOMInput).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance61).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRateUOMInput).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbRateUOMInput).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.CmbRateUOMInput).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbRateUOMInput).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbRateUOMInput).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbRateUOMInput).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbRateUOMInput).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.CmbRateUOMInput).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbRateUOMInput).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbRateUOMInput).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbRateUOMInput).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbRateUOMInput.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbRateUOMInput).Location = new System.Drawing.Point(594, 83);
		((System.Windows.Forms.Control)(object)this.CmbRateUOMInput).Name = "CmbRateUOMInput";
		((System.Windows.Forms.Control)(object)this.CmbRateUOMInput).Size = new System.Drawing.Size(87, 24);
		((System.Windows.Forms.Control)(object)this.CmbRateUOMInput).TabIndex = 15;
		((System.Windows.Forms.Control)(object)this.CmbRateUOMInput).Leave += new System.EventHandler(CmbRateUOMInput_Leave);
		this.Add.BackColor = System.Drawing.Color.White;
		this.Add.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.Add.Location = new System.Drawing.Point(779, 84);
		this.Add.Margin = new System.Windows.Forms.Padding(2);
		this.Add.Name = "Add";
		this.Add.Size = new System.Drawing.Size(26, 23);
		this.Add.TabIndex = 18;
		this.Add.Text = "+";
		this.Add.UseVisualStyleBackColor = false;
		this.Add.Click += new System.EventHandler(Add_Click);
		this.panel28.Controls.Add(this.txtdocnumber);
		this.panel28.Controls.Add(this.label77);
		this.panel28.Controls.Add(this.txtRemarks);
		this.panel28.Controls.Add(this.btnLoadInvoices);
		this.panel28.Controls.Add(this.label6);
		this.panel28.Controls.Add(this.label1);
		this.panel28.Controls.Add(this.txtAvgStock);
		this.panel28.Controls.Add(this.label2);
		this.panel28.Controls.Add(this.txtDocdate);
		this.panel28.Controls.Add(this.label22);
		this.panel28.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobOrderNo);
		this.panel28.Controls.Add(this.label23);
		this.panel28.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWIPItem);
		this.panel28.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWorkInProcess);
		this.panel28.Controls.Add(this.txtAvgRate);
		this.panel28.Controls.Add(this.label25);
		this.panel28.Controls.Add(this.label27);
		this.panel28.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel28.Location = new System.Drawing.Point(0, 0);
		this.panel28.Name = "panel28";
		this.panel28.Size = new System.Drawing.Size(1225, 60);
		this.panel28.TabIndex = 174;
		this.txtdocnumber.BackColor = System.Drawing.Color.White;
		this.txtdocnumber.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocnumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocnumber.Location = new System.Drawing.Point(96, 6);
		this.txtdocnumber.Name = "txtdocnumber";
		this.txtdocnumber.ReadOnly = true;
		this.txtdocnumber.Size = new System.Drawing.Size(122, 22);
		this.txtdocnumber.TabIndex = 0;
		this.txtdocnumber.Leave += new System.EventHandler(txtdocnumber_Leave);
		this.label77.AutoSize = true;
		this.label77.Location = new System.Drawing.Point(761, 36);
		this.label77.Name = "label77";
		this.label77.Size = new System.Drawing.Size(51, 13);
		this.label77.TabIndex = 173;
		this.label77.Text = "WIP Item";
		this.txtRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarks.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarks.Location = new System.Drawing.Point(96, 32);
		this.txtRemarks.Name = "txtRemarks";
		this.txtRemarks.Size = new System.Drawing.Size(579, 22);
		this.txtRemarks.TabIndex = 5;
		this.btnLoadInvoices.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLoadInvoices.Location = new System.Drawing.Point(680, 6);
		this.btnLoadInvoices.Name = "btnLoadInvoices";
		this.btnLoadInvoices.Size = new System.Drawing.Size(48, 24);
		this.btnLoadInvoices.TabIndex = 4;
		this.btnLoadInvoices.Text = "Load";
		this.btnLoadInvoices.UseVisualStyleBackColor = true;
		this.btnLoadInvoices.Visible = false;
		this.btnLoadInvoices.Click += new System.EventHandler(btnLoadInvoices_Click);
		this.label6.AutoSize = true;
		this.label6.Location = new System.Drawing.Point(18, 36);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(49, 13);
		this.label6.TabIndex = 59;
		this.label6.Text = "Remarks";
		this.label1.AutoSize = true;
		this.label1.Location = new System.Drawing.Point(18, 11);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(73, 13);
		this.label1.TabIndex = 25;
		this.label1.Text = "Document No";
		this.txtAvgStock.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAvgStock.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAvgStock.Location = new System.Drawing.Point(1112, 25);
		this.txtAvgStock.Name = "txtAvgStock";
		this.txtAvgStock.ReadOnly = true;
		this.txtAvgStock.Size = new System.Drawing.Size(103, 20);
		this.txtAvgStock.TabIndex = 166;
		this.txtAvgStock.TabStop = false;
		this.label2.AutoSize = true;
		this.label2.Location = new System.Drawing.Point(226, 11);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(82, 13);
		this.label2.TabIndex = 33;
		this.label2.Text = "Document Date";
		this.txtDocdate.CustomFormat = "dd-MMM-yy";
		this.txtDocdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDocdate.Location = new System.Drawing.Point(319, 7);
		this.txtDocdate.Name = "txtDocdate";
		this.txtDocdate.Size = new System.Drawing.Size(106, 22);
		this.txtDocdate.TabIndex = 1;
		this.label22.AutoSize = true;
		this.label22.Location = new System.Drawing.Point(1047, 29);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(59, 13);
		this.label22.TabIndex = 167;
		this.label22.Text = "AvgStocks";
		this.CmbJobOrderNo.AllowNull = (DefaultableBoolean)1;
		this.CmbJobOrderNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobOrderNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobOrderNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance67;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance73).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance74).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobOrderNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNo).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbJobOrderNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNo).Location = new System.Drawing.Point(510, 6);
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNo).Name = "CmbJobOrderNo";
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNo).Size = new System.Drawing.Size(165, 24);
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNo).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNo).Leave += new System.EventHandler(CmbJobOrderNo_Leave);
		this.label23.AutoSize = true;
		this.label23.Location = new System.Drawing.Point(431, 11);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(70, 13);
		this.label23.TabIndex = 151;
		this.label23.Text = "Job Order No";
		this.CmbWIPItem.AllowNull = (DefaultableBoolean)1;
		this.CmbWIPItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWIPItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWIPItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWIPItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.CmbWIPItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance33).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWIPItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWIPItem).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWIPItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWIPItem).Location = new System.Drawing.Point(841, 30);
		((System.Windows.Forms.Control)(object)this.CmbWIPItem).Name = "CmbWIPItem";
		((System.Windows.Forms.Control)(object)this.CmbWIPItem).Size = new System.Drawing.Size(200, 24);
		((System.Windows.Forms.Control)(object)this.CmbWIPItem).TabIndex = 172;
		((System.Windows.Forms.Control)(object)this.CmbWIPItem).TabStop = false;
		this.CmbWorkInProcess.AllowNull = (DefaultableBoolean)1;
		this.CmbWorkInProcess.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWorkInProcess.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWorkInProcess.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Appearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance41;
		((SpecialBoxBase)((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWorkInProcess).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWorkInProcess.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWorkInProcess).Location = new System.Drawing.Point(841, 3);
		((System.Windows.Forms.Control)(object)this.CmbWorkInProcess).Name = "CmbWorkInProcess";
		((System.Windows.Forms.Control)(object)this.CmbWorkInProcess).Size = new System.Drawing.Size(200, 24);
		((System.Windows.Forms.Control)(object)this.CmbWorkInProcess).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.CmbWorkInProcess).TabStop = false;
		this.txtAvgRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAvgRate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAvgRate.Location = new System.Drawing.Point(1112, 3);
		this.txtAvgRate.Name = "txtAvgRate";
		this.txtAvgRate.ReadOnly = true;
		this.txtAvgRate.Size = new System.Drawing.Size(103, 20);
		this.txtAvgRate.TabIndex = 168;
		this.txtAvgRate.TabStop = false;
		this.label25.AutoSize = true;
		this.label25.Location = new System.Drawing.Point(1047, 7);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(49, 13);
		this.label25.TabIndex = 169;
		this.label25.Text = "AvgRate";
		this.label27.AutoSize = true;
		this.label27.Location = new System.Drawing.Point(761, 9);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(71, 13);
		this.label27.TabIndex = 171;
		this.label27.Text = "WIP Account";
		this.panel2.Controls.Add(this.ctrlGrdBar3);
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1225, 30);
		this.panel2.TabIndex = 0;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(1176, 2);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.grdStockConversionProductionDetail;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(45, 21);
		this.ctrlGrdBar3.TabIndex = 20;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[7] { this.toolStripButton5, this.btnnew, this.btnsave, this.btnUpdate, this.toolStripSeparator, this.Print, this.btnVoucherReport });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1225, 27);
		this.toolStrip1.TabIndex = 1;
		this.toolStrip1.Text = "toolStrip1";
		this.toolStripButton5.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton5.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton5.Name = "toolStripButton5";
		this.toolStripButton5.Size = new System.Drawing.Size(133, 24);
		this.toolStripButton5.Text = "Production           ";
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
		this.toolStripSeparator.Name = "toolStripSeparator";
		this.toolStripSeparator.Size = new System.Drawing.Size(6, 27);
		this.Print.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.Print.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.Print.Name = "Print";
		this.Print.Size = new System.Drawing.Size(56, 24);
		this.Print.Text = "&Print";
		this.Print.Click += new System.EventHandler(Print_Click);
		this.btnVoucherReport.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnVoucherReport.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnVoucherReport.Name = "btnVoucherReport";
		this.btnVoucherReport.Size = new System.Drawing.Size(97, 24);
		this.btnVoucherReport.Text = "118-Voucher";
		this.btnVoucherReport.Click += new System.EventHandler(btnVoucherReport_Click);
		this.OutPut.Controls.Add(this.panel3);
		this.OutPut.Location = new System.Drawing.Point(4, 22);
		this.OutPut.Name = "OutPut";
		this.OutPut.Size = new System.Drawing.Size(1231, 719);
		this.OutPut.TabIndex = 5;
		this.OutPut.Text = "OutPut";
		this.OutPut.UseVisualStyleBackColor = true;
		this.panel3.Controls.Add(this.panel14);
		this.panel3.Controls.Add(this.panel12);
		this.panel3.Controls.Add(this.panel6);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1231, 719);
		this.panel3.TabIndex = 0;
		this.panel14.Controls.Add((System.Windows.Forms.Control)(object)this.grdByProduct);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel14.Location = new System.Drawing.Point(0, 189);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(1231, 530);
		this.panel14.TabIndex = 2;
		((ScrollableBase)this.grdByProduct).BackColor = System.Drawing.Color.White;
		grdByProduct_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdByProduct.DesignTimeLayout = grdByProduct_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdByProduct).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdByProduct).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdByProduct.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdByProduct).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdByProduct).Name = "grdByProduct";
		((ScrollableBase)this.grdByProduct).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdByProduct).Size = new System.Drawing.Size(1231, 530);
		((System.Windows.Forms.Control)(object)this.grdByProduct).TabIndex = 13;
		this.grdByProduct.TotalRow = (InheritableBoolean)1;
		this.grdByProduct.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdByProduct).VisualStyle = (VisualStyle)4;
		this.grdByProduct.CellEdited += new ColumnActionEventHandler(grdByProduct_CellEdited);
		this.grdByProduct.CellUpdated += new ColumnActionEventHandler(grdByProduct_CellUpdated);
		((System.Windows.Forms.Control)(object)this.grdByProduct).DoubleClick += new System.EventHandler(grdByProduct_DoubleClick);
		this.panel12.Controls.Add(this.panel31);
		this.panel12.Controls.Add(this.panel30);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(0, 30);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1231, 159);
		this.panel12.TabIndex = 1;
		this.panel31.Controls.Add(this.txtRemarksOutPut);
		this.panel31.Controls.Add(this.label70);
		this.panel31.Controls.Add(this.label33);
		this.panel31.Controls.Add(this.label69);
		this.panel31.Controls.Add((System.Windows.Forms.Control)(object)this.CmbStockAccount);
		this.panel31.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPackTypeOutPut);
		this.panel31.Controls.Add((System.Windows.Forms.Control)(object)this.CmbEntryType);
		this.panel31.Controls.Add(this.label45);
		this.panel31.Controls.Add(this.txtAmountOutPut);
		this.panel31.Controls.Add(this.label43);
		this.panel31.Controls.Add(this.lblAmountOUtPUt);
		this.panel31.Controls.Add(this.label36);
		this.panel31.Controls.Add((System.Windows.Forms.Control)(object)this.CmbRateUomOutPut);
		this.panel31.Controls.Add(this.txtQtyOutPut);
		this.panel31.Controls.Add(this.lblRateUOMOUtPut);
		this.panel31.Controls.Add(this.label24);
		this.panel31.Controls.Add(this.txtRateOutPut);
		this.panel31.Controls.Add(this.label20);
		this.panel31.Controls.Add(this.lblRateOutPUt);
		this.panel31.Controls.Add(this.btnAddOutPut);
		this.panel31.Controls.Add(this.btnCancelOutPut);
		this.panel31.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCropYearOutPut);
		this.panel31.Controls.Add(this.btnUpdateDetailOutPut);
		this.panel31.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemOutput);
		this.panel31.Controls.Add((System.Windows.Forms.Control)(object)this.CmbLotOutPut);
		this.panel31.Controls.Add(this.label9);
		this.panel31.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWarehouseOutput);
		this.panel31.Controls.Add((System.Windows.Forms.Control)(object)this.CmbUomOutPut);
		this.panel31.Controls.Add(this.label13);
		this.panel31.Controls.Add(this.txtNetWeightOutPut);
		this.panel31.Controls.Add(this.label11);
		this.panel31.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel31.Location = new System.Drawing.Point(0, 57);
		this.panel31.Name = "panel31";
		this.panel31.Size = new System.Drawing.Size(1231, 100);
		this.panel31.TabIndex = 180;
		this.panel31.Visible = false;
		this.txtRemarksOutPut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarksOutPut.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksOutPut.Location = new System.Drawing.Point(12, 76);
		this.txtRemarksOutPut.Name = "txtRemarksOutPut";
		this.txtRemarksOutPut.Size = new System.Drawing.Size(438, 21);
		this.txtRemarksOutPut.TabIndex = 14;
		this.label70.AutoSize = true;
		this.label70.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label70.Location = new System.Drawing.Point(726, 3);
		this.label70.Name = "label70";
		this.label70.Size = new System.Drawing.Size(61, 15);
		this.label70.TabIndex = 141;
		this.label70.Text = "Crop Year";
		this.label33.AutoSize = true;
		this.label33.Location = new System.Drawing.Point(712, 55);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(51, 13);
		this.label33.TabIndex = 173;
		this.label33.Text = "Stock Ac";
		this.label69.AutoSize = true;
		this.label69.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label69.Location = new System.Drawing.Point(833, 3);
		this.label69.Name = "label69";
		this.label69.Size = new System.Drawing.Size(24, 15);
		this.label69.TabIndex = 142;
		this.label69.Text = "Lot";
		this.CmbStockAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbStockAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbStockAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbStockAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbStockAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance223).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance223;
		((SpecialBoxBase)((UltraGridBase)this.CmbStockAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance247).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance247).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance247).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance247).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance247;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance258).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance258).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance258;
		((AppearanceBase)appearance269).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance269).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance269;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance280).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance280;
		((AppearanceBase)appearance291).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance291).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance291;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance302).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance302).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance302).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance302).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance302).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance302;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbStockAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbStockAccount).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbStockAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbStockAccount).Location = new System.Drawing.Point(712, 74);
		((System.Windows.Forms.Control)(object)this.CmbStockAccount).Name = "CmbStockAccount";
		((System.Windows.Forms.Control)(object)this.CmbStockAccount).Size = new System.Drawing.Size(242, 24);
		((System.Windows.Forms.Control)(object)this.CmbStockAccount).TabIndex = 18;
		this.CmbPackTypeOutPut.AllowNull = (DefaultableBoolean)1;
		this.CmbPackTypeOutPut.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackTypeOutPut.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackTypeOutPut.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackTypeOutPut).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackTypeOutPut).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPackTypeOutPut.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackTypeOutPut).Location = new System.Drawing.Point(949, 23);
		((System.Windows.Forms.Control)(object)this.CmbPackTypeOutPut).Name = "CmbPackTypeOutPut";
		((System.Windows.Forms.Control)(object)this.CmbPackTypeOutPut).Size = new System.Drawing.Size(90, 24);
		((System.Windows.Forms.Control)(object)this.CmbPackTypeOutPut).TabIndex = 10;
		this.CmbEntryType.AllowNull = (DefaultableBoolean)1;
		this.CmbEntryType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbEntryType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbEntryType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbEntryType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance57;
		((SpecialBoxBase)((UltraGridBase)this.CmbEntryType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance101;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance113).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance124).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance124).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance124).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance124).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance124;
		((AppearanceBase)appearance135).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbEntryType).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbEntryType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbEntryType).Location = new System.Drawing.Point(10, 21);
		((System.Windows.Forms.Control)(object)this.CmbEntryType).Name = "CmbEntryType";
		((System.Windows.Forms.Control)(object)this.CmbEntryType).Size = new System.Drawing.Size(151, 24);
		((System.Windows.Forms.Control)(object)this.CmbEntryType).TabIndex = 4;
		this.label45.AutoSize = true;
		this.label45.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label45.Location = new System.Drawing.Point(946, 3);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(63, 15);
		this.label45.TabIndex = 143;
		this.label45.Text = "Pack Type";
		this.txtAmountOutPut.BackColor = System.Drawing.Color.White;
		this.txtAmountOutPut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAmountOutPut.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAmountOutPut.Location = new System.Drawing.Point(615, 76);
		this.txtAmountOutPut.Name = "txtAmountOutPut";
		this.txtAmountOutPut.ReadOnly = true;
		this.txtAmountOutPut.Size = new System.Drawing.Size(94, 21);
		this.txtAmountOutPut.TabIndex = 17;
		this.label43.AutoSize = true;
		this.label43.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label43.Location = new System.Drawing.Point(181, 3);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(72, 15);
		this.label43.TabIndex = 140;
		this.label43.Text = "WareHouse";
		this.lblAmountOUtPUt.AutoSize = true;
		this.lblAmountOUtPUt.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblAmountOUtPUt.Location = new System.Drawing.Point(616, 54);
		this.lblAmountOUtPUt.Name = "lblAmountOUtPUt";
		this.lblAmountOUtPUt.Size = new System.Drawing.Size(49, 15);
		this.lblAmountOUtPUt.TabIndex = 159;
		this.lblAmountOUtPUt.Text = "Amount";
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(1041, 3);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(51, 15);
		this.label36.TabIndex = 145;
		this.label36.Text = "Quantity";
		this.CmbRateUomOutPut.AllowNull = (DefaultableBoolean)1;
		this.CmbRateUomOutPut.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbRateUomOutPut.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbRateUomOutPut.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance168).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.Appearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance179).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance179).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance179).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance179;
		((AppearanceBase)appearance190).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance190;
		((SpecialBoxBase)((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance201).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance201).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance201).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance201).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance201;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance212).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance212).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance212;
		((AppearanceBase)appearance224).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance224).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance224;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance235).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance235;
		((AppearanceBase)appearance239).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance239).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance239;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance240).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance240).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance240).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance240).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance240).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance240;
		((AppearanceBase)appearance241).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance241;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance242).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance242).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance242;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance243).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance243;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbRateUomOutPut).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbRateUomOutPut).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbRateUomOutPut.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbRateUomOutPut).Location = new System.Drawing.Point(526, 74);
		((System.Windows.Forms.Control)(object)this.CmbRateUomOutPut).Name = "CmbRateUomOutPut";
		((System.Windows.Forms.Control)(object)this.CmbRateUomOutPut).Size = new System.Drawing.Size(87, 24);
		((System.Windows.Forms.Control)(object)this.CmbRateUomOutPut).TabIndex = 16;
		((System.Windows.Forms.Control)(object)this.CmbRateUomOutPut).Leave += new System.EventHandler(CmbRateUomOutPut_Leave);
		this.txtQtyOutPut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtQtyOutPut.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtQtyOutPut.Location = new System.Drawing.Point(1042, 24);
		this.txtQtyOutPut.Name = "txtQtyOutPut";
		this.txtQtyOutPut.Size = new System.Drawing.Size(85, 21);
		this.txtQtyOutPut.TabIndex = 11;
		this.txtQtyOutPut.TextChanged += new System.EventHandler(txtQtyOutPut_TextChanged);
		this.txtQtyOutPut.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtQtyOutPut_KeyPress);
		this.lblRateUOMOUtPut.AutoSize = true;
		this.lblRateUOMOUtPut.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblRateUOMOUtPut.Location = new System.Drawing.Point(527, 54);
		this.lblRateUOMOUtPut.Name = "lblRateUOMOUtPut";
		this.lblRateUOMOUtPut.Size = new System.Drawing.Size(65, 15);
		this.lblRateUOMOUtPut.TabIndex = 157;
		this.lblRateUOMOUtPut.Text = "Rate UOM";
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(329, 3);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(31, 15);
		this.label24.TabIndex = 135;
		this.label24.Text = "Item";
		this.txtRateOutPut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRateOutPut.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRateOutPut.Location = new System.Drawing.Point(452, 76);
		this.txtRateOutPut.Name = "txtRateOutPut";
		this.txtRateOutPut.Size = new System.Drawing.Size(70, 21);
		this.txtRateOutPut.TabIndex = 15;
		this.txtRateOutPut.TextChanged += new System.EventHandler(txtRateOutPut_TextChanged);
		this.txtRateOutPut.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtRateOutPut_KeyPress);
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(638, 3);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(36, 15);
		this.label20.TabIndex = 136;
		this.label20.Text = "UOM";
		this.lblRateOutPUt.AutoSize = true;
		this.lblRateOutPUt.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblRateOutPUt.Location = new System.Drawing.Point(453, 54);
		this.lblRateOutPUt.Name = "lblRateOutPUt";
		this.lblRateOutPUt.Size = new System.Drawing.Size(33, 15);
		this.lblRateOutPUt.TabIndex = 155;
		this.lblRateOutPUt.Text = "Rate";
		this.btnAddOutPut.BackColor = System.Drawing.Color.White;
		this.btnAddOutPut.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAddOutPut.Location = new System.Drawing.Point(956, 74);
		this.btnAddOutPut.Margin = new System.Windows.Forms.Padding(2);
		this.btnAddOutPut.Name = "btnAddOutPut";
		this.btnAddOutPut.Size = new System.Drawing.Size(26, 24);
		this.btnAddOutPut.TabIndex = 19;
		this.btnAddOutPut.Text = "+";
		this.btnAddOutPut.UseVisualStyleBackColor = false;
		this.btnAddOutPut.Click += new System.EventHandler(btnAddOutPut_Click);
		this.btnCancelOutPut.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelOutPut.Location = new System.Drawing.Point(1046, 75);
		this.btnCancelOutPut.Name = "btnCancelOutPut";
		this.btnCancelOutPut.Size = new System.Drawing.Size(55, 23);
		this.btnCancelOutPut.TabIndex = 21;
		this.btnCancelOutPut.Text = "Cancel";
		this.btnCancelOutPut.UseVisualStyleBackColor = true;
		this.CmbCropYearOutPut.AllowNull = (DefaultableBoolean)1;
		this.CmbCropYearOutPut.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCropYearOutPut.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCropYearOutPut.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance310).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance310).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.Appearance = (AppearanceBase)(object)appearance310;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance311).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance311).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance311).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance311).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance311;
		((AppearanceBase)appearance312).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance312;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCropYearOutPut).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCropYearOutPut).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCropYearOutPut.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCropYearOutPut).Location = new System.Drawing.Point(730, 22);
		((System.Windows.Forms.Control)(object)this.CmbCropYearOutPut).Name = "CmbCropYearOutPut";
		((System.Windows.Forms.Control)(object)this.CmbCropYearOutPut).Size = new System.Drawing.Size(102, 24);
		((System.Windows.Forms.Control)(object)this.CmbCropYearOutPut).TabIndex = 8;
		this.btnUpdateDetailOutPut.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetailOutPut.Location = new System.Drawing.Point(986, 75);
		this.btnUpdateDetailOutPut.Name = "btnUpdateDetailOutPut";
		this.btnUpdateDetailOutPut.Size = new System.Drawing.Size(56, 23);
		this.btnUpdateDetailOutPut.TabIndex = 20;
		this.btnUpdateDetailOutPut.Text = "Update ";
		this.btnUpdateDetailOutPut.UseVisualStyleBackColor = true;
		this.btnUpdateDetailOutPut.Click += new System.EventHandler(btnUpdateDetailOutPut_Click);
		this.CmbItemOutput.AllowNull = (DefaultableBoolean)1;
		this.CmbItemOutput.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemOutput.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemOutput.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance297).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance297).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.Appearance = (AppearanceBase)(object)appearance297;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance298).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance298).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance298).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance298).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemOutput).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance298;
		((AppearanceBase)appearance299).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance299;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemOutput).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance300).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance300).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance300).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance300).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance300;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance301).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance301).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance301;
		((AppearanceBase)appearance303).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance303).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance303;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance304).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance304;
		((AppearanceBase)appearance305).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance305).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance305;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance306).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance306).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance306).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance306).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance306).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance306;
		((AppearanceBase)appearance307).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance307;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance308).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance308).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance308;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance309).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance309;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemOutput).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemOutput).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemOutput.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemOutput).Location = new System.Drawing.Point(325, 22);
		((System.Windows.Forms.Control)(object)this.CmbItemOutput).Name = "CmbItemOutput";
		((System.Windows.Forms.Control)(object)this.CmbItemOutput).Size = new System.Drawing.Size(307, 24);
		((System.Windows.Forms.Control)(object)this.CmbItemOutput).TabIndex = 6;
		((System.Windows.Forms.Control)(object)this.CmbItemOutput).Leave += new System.EventHandler(CmbItemOutput_Leave);
		this.CmbLotOutPut.AllowNull = (DefaultableBoolean)1;
		this.CmbLotOutPut.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbLotOutPut.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbLotOutPut.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance284).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance284).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.Appearance = (AppearanceBase)(object)appearance284;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance285).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance285).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance285).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance285).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbLotOutPut).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance285;
		((AppearanceBase)appearance286).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance286;
		((SpecialBoxBase)((UltraGridBase)this.CmbLotOutPut).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance287).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance287).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance287).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance287).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance287;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance288).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance288).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance288;
		((AppearanceBase)appearance289).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance289).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance289;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance290).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance290;
		((AppearanceBase)appearance292).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance292).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance292;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance293).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance293).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance293).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance293).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance293).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance293;
		((AppearanceBase)appearance294).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance294;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance295).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance295).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance295;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance296).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance296;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbLotOutPut).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbLotOutPut).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbLotOutPut.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbLotOutPut).Location = new System.Drawing.Point(836, 22);
		((System.Windows.Forms.Control)(object)this.CmbLotOutPut).Name = "CmbLotOutPut";
		((System.Windows.Forms.Control)(object)this.CmbLotOutPut).Size = new System.Drawing.Size(109, 24);
		((System.Windows.Forms.Control)(object)this.CmbLotOutPut).TabIndex = 9;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(14, 54);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(57, 15);
		this.label9.TabIndex = 149;
		this.label9.Text = "Remarks";
		this.CmbWarehouseOutput.AllowNull = (DefaultableBoolean)1;
		this.CmbWarehouseOutput.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWarehouseOutput.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWarehouseOutput.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance271).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance271).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.Appearance = (AppearanceBase)(object)appearance271;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance272).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance272).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance272).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance272).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance272;
		((AppearanceBase)appearance273).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance273;
		((SpecialBoxBase)((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance274).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance274).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance274).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance274).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance274;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance275).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance275).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance275;
		((AppearanceBase)appearance276).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance276).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance276;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance277).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance277;
		((AppearanceBase)appearance278).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance278).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance278;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance279).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance279).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance279).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance279).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance279).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance279;
		((AppearanceBase)appearance281).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance281;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance282).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance282).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance282;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance283).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance283;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWarehouseOutput).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWarehouseOutput).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWarehouseOutput.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWarehouseOutput).Location = new System.Drawing.Point(165, 22);
		((System.Windows.Forms.Control)(object)this.CmbWarehouseOutput).Name = "CmbWarehouseOutput";
		((System.Windows.Forms.Control)(object)this.CmbWarehouseOutput).Size = new System.Drawing.Size(158, 24);
		((System.Windows.Forms.Control)(object)this.CmbWarehouseOutput).TabIndex = 5;
		this.CmbUomOutPut.AllowNull = (DefaultableBoolean)1;
		this.CmbUomOutPut.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbUomOutPut.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbUomOutPut.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance257).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance257).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.Appearance = (AppearanceBase)(object)appearance257;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance259).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance259).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance259).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance259).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbUomOutPut).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance259;
		((AppearanceBase)appearance260).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance260;
		((SpecialBoxBase)((UltraGridBase)this.CmbUomOutPut).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance261).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance261).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance261).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance261).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance261;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance262).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance262).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance262;
		((AppearanceBase)appearance263).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance263).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance263;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance264).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance264;
		((AppearanceBase)appearance265).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance265).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance265;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance266).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance266).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance266).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance266).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance266).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance266;
		((AppearanceBase)appearance267).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance267;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance268).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance268).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance268;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance270).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance270;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbUomOutPut).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbUomOutPut).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbUomOutPut.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbUomOutPut).Location = new System.Drawing.Point(638, 22);
		((System.Windows.Forms.Control)(object)this.CmbUomOutPut).Name = "CmbUomOutPut";
		((System.Windows.Forms.Control)(object)this.CmbUomOutPut).Size = new System.Drawing.Size(87, 24);
		((System.Windows.Forms.Control)(object)this.CmbUomOutPut).TabIndex = 7;
		((System.Windows.Forms.Control)(object)this.CmbUomOutPut).Leave += new System.EventHandler(CmbUomOutPut_Leave);
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(7, 3);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(63, 15);
		this.label13.TabIndex = 147;
		this.label13.Text = "Entry Type";
		this.txtNetWeightOutPut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNetWeightOutPut.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNetWeightOutPut.Location = new System.Drawing.Point(1129, 24);
		this.txtNetWeightOutPut.Name = "txtNetWeightOutPut";
		this.txtNetWeightOutPut.Size = new System.Drawing.Size(93, 21);
		this.txtNetWeightOutPut.TabIndex = 13;
		this.txtNetWeightOutPut.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtNetWeightOutPut_KeyPress);
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(1134, 3);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(67, 15);
		this.label11.TabIndex = 148;
		this.label11.Text = "Net Weight";
		this.panel30.Controls.Add(this.txtDocNoOutPut);
		this.panel30.Controls.Add(this.btnApplyRate);
		this.panel30.Controls.Add(this.txtRemarksHeaderOutPut);
		this.panel30.Controls.Add(this.txtFGRate);
		this.panel30.Controls.Add(this.label19);
		this.panel30.Controls.Add(this.label76);
		this.panel30.Controls.Add(this.label16);
		this.panel30.Controls.Add(this.txtFGWeight);
		this.panel30.Controls.Add(this.label15);
		this.panel30.Controls.Add(this.label75);
		this.panel30.Controls.Add(this.DocDateOutPut);
		this.panel30.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobOrderNoOutput);
		this.panel30.Controls.Add(this.label60);
		this.panel30.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel30.Location = new System.Drawing.Point(0, 0);
		this.panel30.Name = "panel30";
		this.panel30.Size = new System.Drawing.Size(1231, 57);
		this.panel30.TabIndex = 179;
		this.txtDocNoOutPut.BackColor = System.Drawing.Color.White;
		this.txtDocNoOutPut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDocNoOutPut.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNoOutPut.Location = new System.Drawing.Point(8, 28);
		this.txtDocNoOutPut.Name = "txtDocNoOutPut";
		this.txtDocNoOutPut.ReadOnly = true;
		this.txtDocNoOutPut.Size = new System.Drawing.Size(122, 21);
		this.txtDocNoOutPut.TabIndex = 0;
		this.txtDocNoOutPut.Leave += new System.EventHandler(txtDocNoOutPut_Leave);
		this.btnApplyRate.BackColor = System.Drawing.Color.White;
		this.btnApplyRate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnApplyRate.Location = new System.Drawing.Point(1145, 25);
		this.btnApplyRate.Margin = new System.Windows.Forms.Padding(2);
		this.btnApplyRate.Name = "btnApplyRate";
		this.btnApplyRate.Size = new System.Drawing.Size(74, 24);
		this.btnApplyRate.TabIndex = 178;
		this.btnApplyRate.Text = "ApplyRate";
		this.btnApplyRate.UseVisualStyleBackColor = false;
		this.btnApplyRate.Click += new System.EventHandler(btnApplyRate_Click);
		this.txtRemarksHeaderOutPut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarksHeaderOutPut.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksHeaderOutPut.Location = new System.Drawing.Point(344, 28);
		this.txtRemarksHeaderOutPut.Name = "txtRemarksHeaderOutPut";
		this.txtRemarksHeaderOutPut.Size = new System.Drawing.Size(566, 21);
		this.txtRemarksHeaderOutPut.TabIndex = 3;
		this.txtFGRate.BackColor = System.Drawing.Color.White;
		this.txtFGRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFGRate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFGRate.Location = new System.Drawing.Point(1044, 27);
		this.txtFGRate.Name = "txtFGRate";
		this.txtFGRate.ReadOnly = true;
		this.txtFGRate.Size = new System.Drawing.Size(93, 21);
		this.txtFGRate.TabIndex = 176;
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(346, 8);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(57, 15);
		this.label19.TabIndex = 138;
		this.label19.Text = "Remarks";
		this.label76.AutoSize = true;
		this.label76.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label76.Location = new System.Drawing.Point(1046, 6);
		this.label76.Name = "label76";
		this.label76.Size = new System.Drawing.Size(52, 15);
		this.label76.TabIndex = 177;
		this.label76.Text = "FG Rate";
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(9, 8);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(48, 15);
		this.label16.TabIndex = 134;
		this.label16.Text = "Doc No";
		this.txtFGWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFGWeight.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFGWeight.Location = new System.Drawing.Point(949, 27);
		this.txtFGWeight.Name = "txtFGWeight";
		this.txtFGWeight.Size = new System.Drawing.Size(93, 21);
		this.txtFGWeight.TabIndex = 174;
		this.txtFGWeight.TextChanged += new System.EventHandler(txtFGWeight_TextChanged);
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(134, 8);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(58, 15);
		this.label15.TabIndex = 137;
		this.label15.Text = "Doc Date";
		this.label75.AutoSize = true;
		this.label75.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label75.Location = new System.Drawing.Point(951, 4);
		this.label75.Name = "label75";
		this.label75.Size = new System.Drawing.Size(64, 15);
		this.label75.TabIndex = 175;
		this.label75.Text = "FG Weight";
		this.DocDateOutPut.CustomFormat = "dd-MMM-yy";
		this.DocDateOutPut.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DocDateOutPut.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DocDateOutPut.Location = new System.Drawing.Point(133, 28);
		this.DocDateOutPut.Name = "DocDateOutPut";
		this.DocDateOutPut.Size = new System.Drawing.Size(107, 21);
		this.DocDateOutPut.TabIndex = 1;
		this.CmbJobOrderNoOutput.AllowNull = (DefaultableBoolean)1;
		this.CmbJobOrderNoOutput.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobOrderNoOutput.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobOrderNoOutput.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance244).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance244).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.Appearance = (AppearanceBase)(object)appearance244;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance245).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance245).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance245).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance245).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance245;
		((AppearanceBase)appearance246).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance246;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance248).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance248).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance248).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance248).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance248;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance249).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance249).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance249;
		((AppearanceBase)appearance250).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance250).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance250;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance251).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance251;
		((AppearanceBase)appearance252).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance252).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance252;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance253).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance253).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance253).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance253).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance253).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance253;
		((AppearanceBase)appearance254).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance254;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance255).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance255).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance255;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance256).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance256;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobOrderNoOutput).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNoOutput).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbJobOrderNoOutput.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNoOutput).Location = new System.Drawing.Point(243, 25);
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNoOutput).Name = "CmbJobOrderNoOutput";
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNoOutput).Size = new System.Drawing.Size(98, 24);
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNoOutput).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbJobOrderNoOutput).Leave += new System.EventHandler(CmbJobOrderNoOutput_Leave);
		this.label60.AutoSize = true;
		this.label60.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label60.Location = new System.Drawing.Point(246, 5);
		this.label60.Name = "label60";
		this.label60.Size = new System.Drawing.Size(80, 15);
		this.label60.TabIndex = 153;
		this.label60.Text = "Job Order No";
		this.panel6.Controls.Add(this.ctrlGrdBar4);
		this.panel6.Controls.Add(this.toolStrip6);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1231, 30);
		this.panel6.TabIndex = 0;
		this.ctrlGrdBar4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar4.FormName = this;
		this.ctrlGrdBar4.Location = new System.Drawing.Point(1181, 3);
		this.ctrlGrdBar4.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar4.MyGrid = this.grdByProduct;
		this.ctrlGrdBar4.Name = "ctrlGrdBar4";
		this.ctrlGrdBar4.Size = new System.Drawing.Size(45, 21);
		this.ctrlGrdBar4.TabIndex = 21;
		this.ctrlGrdBar4.Load += new System.EventHandler(ctrlGrdBar4_Load);
		this.toolStrip6.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip6.Items.AddRange(new System.Windows.Forms.ToolStripItem[7] { this.toolStripButton8, this.toolStripButton10, this.btnSaveOutPut, this.btnUpdateOutPut, this.toolStripSeparator5, this.btnPrintOutPut, this.btnVoucherOutPut });
		this.toolStrip6.Location = new System.Drawing.Point(0, 0);
		this.toolStrip6.Name = "toolStrip6";
		this.toolStrip6.Size = new System.Drawing.Size(1231, 27);
		this.toolStrip6.TabIndex = 2;
		this.toolStrip6.Text = "toolStrip6";
		this.toolStripButton8.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton8.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton8.Name = "toolStripButton8";
		this.toolStripButton8.Size = new System.Drawing.Size(179, 24);
		this.toolStripButton8.Text = "Production  Output         ";
		this.toolStripButton10.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton10.Image = (System.Drawing.Image)resources.GetObject("toolStripButton10.Image");
		this.toolStripButton10.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton10.Name = "toolStripButton10";
		this.toolStripButton10.Size = new System.Drawing.Size(55, 24);
		this.toolStripButton10.Text = "&New";
		this.toolStripButton10.Click += new System.EventHandler(toolStripButton10_Click);
		this.btnSaveOutPut.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSaveOutPut.Image = (System.Drawing.Image)resources.GetObject("btnSaveOutPut.Image");
		this.btnSaveOutPut.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSaveOutPut.Name = "btnSaveOutPut";
		this.btnSaveOutPut.Size = new System.Drawing.Size(55, 24);
		this.btnSaveOutPut.Text = "&Save";
		this.btnSaveOutPut.Click += new System.EventHandler(btnSaveOutPut_Click);
		this.btnUpdateOutPut.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateOutPut.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdateOutPut.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdateOutPut.Name = "btnUpdateOutPut";
		this.btnUpdateOutPut.Size = new System.Drawing.Size(69, 24);
		this.btnUpdateOutPut.Text = "&Update";
		this.btnUpdateOutPut.Visible = false;
		this.btnUpdateOutPut.Click += new System.EventHandler(btnUpdateOutPut_Click);
		this.toolStripSeparator5.Name = "toolStripSeparator5";
		this.toolStripSeparator5.Size = new System.Drawing.Size(6, 27);
		this.btnPrintOutPut.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrintOutPut.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrintOutPut.Name = "btnPrintOutPut";
		this.btnPrintOutPut.Size = new System.Drawing.Size(56, 24);
		this.btnPrintOutPut.Text = "&Print";
		this.btnPrintOutPut.Click += new System.EventHandler(btnPrintOutPut_Click);
		this.btnVoucherOutPut.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnVoucherOutPut.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnVoucherOutPut.Name = "btnVoucherOutPut";
		this.btnVoucherOutPut.Size = new System.Drawing.Size(97, 24);
		this.btnVoucherOutPut.Text = "118-Voucher";
		this.btnVoucherOutPut.Click += new System.EventHandler(btnVoucherOutPut_Click);
		this.PackingMaterial.Controls.Add(this.panel7);
		this.PackingMaterial.Location = new System.Drawing.Point(4, 22);
		this.PackingMaterial.Name = "PackingMaterial";
		this.PackingMaterial.Size = new System.Drawing.Size(1231, 719);
		this.PackingMaterial.TabIndex = 3;
		this.PackingMaterial.Text = "Packing Material";
		this.PackingMaterial.UseVisualStyleBackColor = true;
		this.panel7.Controls.Add(this.panel9);
		this.panel7.Controls.Add(this.panel24);
		this.panel7.Controls.Add(this.panel22);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1231, 719);
		this.panel7.TabIndex = 0;
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.grdPackingMaterial);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel9.Location = new System.Drawing.Point(0, 127);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1231, 592);
		this.panel9.TabIndex = 3;
		this.grdPackingMaterial.AllowEdit = (InheritableBoolean)2;
		grdPackingMaterial_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPackingMaterial.DesignTimeLayout = grdPackingMaterial_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPackingMaterial).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdPackingMaterial.DynamicFiltering = true;
		this.grdPackingMaterial.FilterMode = (FilterMode)2;
		this.grdPackingMaterial.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdPackingMaterial).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPackingMaterial.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdPackingMaterial).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdPackingMaterial).Name = "grdPackingMaterial";
		((ScrollableBase)this.grdPackingMaterial).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPackingMaterial).Size = new System.Drawing.Size(1231, 592);
		((System.Windows.Forms.Control)(object)this.grdPackingMaterial).TabIndex = 45;
		this.grdPackingMaterial.TotalRow = (InheritableBoolean)1;
		this.grdPackingMaterial.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPackingMaterial).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdPackingMaterial).DoubleClick += new System.EventHandler(grdPackingMaterial_DoubleClick);
		this.panel24.Controls.Add(this.groupBox2);
		this.panel24.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel24.Location = new System.Drawing.Point(0, 30);
		this.panel24.Name = "panel24";
		this.panel24.Size = new System.Drawing.Size(1231, 97);
		this.panel24.TabIndex = 2;
		this.groupBox2.BackColor = System.Drawing.Color.White;
		this.groupBox2.Controls.Add(this.label35);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbJobOrderPackingMaterial);
		this.groupBox2.Controls.Add(this.txtremarksPackingMaterial);
		this.groupBox2.Controls.Add(this.label63);
		this.groupBox2.Controls.Add(this.txtDocNoPackingMaterial);
		this.groupBox2.Controls.Add(this.label62);
		this.groupBox2.Controls.Add(this.label37);
		this.groupBox2.Controls.Add(this.txtAmountPackingMaterial);
		this.groupBox2.Controls.Add(this.label55);
		this.groupBox2.Controls.Add(this.label34);
		this.groupBox2.Controls.Add(this.label41);
		this.groupBox2.Controls.Add(this.txtdocdatePackingMaterial);
		this.groupBox2.Controls.Add(this.txtPackingMaterialRate);
		this.groupBox2.Controls.Add(this.label40);
		this.groupBox2.Controls.Add(this.txtPackingMaterialQTY);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemPackingMaterial);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbUOMPackingMaterial);
		this.groupBox2.Controls.Add(this.label42);
		this.groupBox2.Location = new System.Drawing.Point(17, 6);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(1220, 85);
		this.groupBox2.TabIndex = 64;
		this.groupBox2.TabStop = false;
		this.label35.AutoSize = true;
		this.label35.Location = new System.Drawing.Point(451, 26);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(70, 13);
		this.label35.TabIndex = 116;
		this.label35.Text = "Job Order No";
		this.cmbJobOrderPackingMaterial.AllowNull = (DefaultableBoolean)1;
		this.cmbJobOrderPackingMaterial.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbJobOrderPackingMaterial.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbJobOrderPackingMaterial.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance211).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance211).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbJobOrderPackingMaterial).DisplayLayout.Appearance = (AppearanceBase)(object)appearance211;
		((UltraGridBase)this.cmbJobOrderPackingMaterial).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbJobOrderPackingMaterial).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance213).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance213).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance213).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance213).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbJobOrderPackingMaterial).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance213;
		((AppearanceBase)appearance214).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbJobOrderPackingMaterial).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance214;
		((SpecialBoxBase)((UltraGridBase)this.cmbJobOrderPackingMaterial).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance215).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance215).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance215).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance215).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbJobOrderPackingMaterial).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance215;
		((UltraGridBase)this.cmbJobOrderPackingMaterial).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbJobOrderPackingMaterial).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance216).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance216).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbJobOrderPackingMaterial).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance216;
		((AppearanceBase)appearance217).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance217).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbJobOrderPackingMaterial).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance217;
		((UltraGridBase)this.cmbJobOrderPackingMaterial).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbJobOrderPackingMaterial).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance218).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbJobOrderPackingMaterial).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance218;
		((AppearanceBase)appearance219).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance219).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbJobOrderPackingMaterial).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance219;
		((UltraGridBase)this.cmbJobOrderPackingMaterial).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbJobOrderPackingMaterial).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance220).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance220).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance220).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance220).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance220).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbJobOrderPackingMaterial).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance220;
		((AppearanceBase)appearance221).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbJobOrderPackingMaterial).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance221;
		((UltraGridBase)this.cmbJobOrderPackingMaterial).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbJobOrderPackingMaterial).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance222).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance222).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbJobOrderPackingMaterial).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance222;
		((UltraGridBase)this.cmbJobOrderPackingMaterial).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance225).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbJobOrderPackingMaterial).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance225;
		((UltraGridBase)this.cmbJobOrderPackingMaterial).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbJobOrderPackingMaterial).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbJobOrderPackingMaterial).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbJobOrderPackingMaterial).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbJobOrderPackingMaterial.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbJobOrderPackingMaterial).Location = new System.Drawing.Point(530, 17);
		((System.Windows.Forms.Control)(object)this.cmbJobOrderPackingMaterial).Name = "cmbJobOrderPackingMaterial";
		((System.Windows.Forms.Control)(object)this.cmbJobOrderPackingMaterial).Size = new System.Drawing.Size(113, 26);
		((System.Windows.Forms.Control)(object)this.cmbJobOrderPackingMaterial).TabIndex = 2;
		this.txtremarksPackingMaterial.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtremarksPackingMaterial.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtremarksPackingMaterial.Location = new System.Drawing.Point(706, 50);
		this.txtremarksPackingMaterial.Name = "txtremarksPackingMaterial";
		this.txtremarksPackingMaterial.Size = new System.Drawing.Size(413, 22);
		this.txtremarksPackingMaterial.TabIndex = 8;
		this.label63.AutoSize = true;
		this.label63.Location = new System.Drawing.Point(651, 55);
		this.label63.Name = "label63";
		this.label63.Size = new System.Drawing.Size(49, 13);
		this.label63.TabIndex = 63;
		this.label63.Text = "Remarks";
		this.txtDocNoPackingMaterial.BackColor = System.Drawing.Color.White;
		this.txtDocNoPackingMaterial.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDocNoPackingMaterial.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNoPackingMaterial.Location = new System.Drawing.Point(103, 21);
		this.txtDocNoPackingMaterial.Name = "txtDocNoPackingMaterial";
		this.txtDocNoPackingMaterial.ReadOnly = true;
		this.txtDocNoPackingMaterial.Size = new System.Drawing.Size(122, 22);
		this.txtDocNoPackingMaterial.TabIndex = 0;
		this.label62.AutoSize = true;
		this.label62.Location = new System.Drawing.Point(451, 55);
		this.label62.Name = "label62";
		this.label62.Size = new System.Drawing.Size(43, 13);
		this.label62.TabIndex = 61;
		this.label62.Text = "Amount";
		this.label37.AutoSize = true;
		this.label37.Location = new System.Drawing.Point(13, 26);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(73, 13);
		this.label37.TabIndex = 49;
		this.label37.Text = "Document No";
		this.txtAmountPackingMaterial.BackColor = System.Drawing.Color.White;
		this.txtAmountPackingMaterial.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAmountPackingMaterial.Enabled = false;
		this.txtAmountPackingMaterial.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAmountPackingMaterial.Location = new System.Drawing.Point(530, 50);
		this.txtAmountPackingMaterial.Name = "txtAmountPackingMaterial";
		this.txtAmountPackingMaterial.ReadOnly = true;
		this.txtAmountPackingMaterial.Size = new System.Drawing.Size(113, 22);
		this.txtAmountPackingMaterial.TabIndex = 7;
		this.label55.AutoSize = true;
		this.label55.Location = new System.Drawing.Point(251, 55);
		this.label55.Name = "label55";
		this.label55.Size = new System.Drawing.Size(50, 13);
		this.label55.TabIndex = 59;
		this.label55.Text = "ItemRate";
		this.label34.AutoSize = true;
		this.label34.Location = new System.Drawing.Point(229, 26);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(82, 13);
		this.label34.TabIndex = 50;
		this.label34.Text = "Document Date";
		this.label41.AutoSize = true;
		this.label41.Location = new System.Drawing.Point(19, 55);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(46, 13);
		this.label41.TabIndex = 59;
		this.label41.Text = "Item Qty";
		this.txtdocdatePackingMaterial.CustomFormat = "dd-MMM-yy";
		this.txtdocdatePackingMaterial.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtdocdatePackingMaterial.Location = new System.Drawing.Point(318, 22);
		this.txtdocdatePackingMaterial.Name = "txtdocdatePackingMaterial";
		this.txtdocdatePackingMaterial.Size = new System.Drawing.Size(125, 20);
		this.txtdocdatePackingMaterial.TabIndex = 1;
		this.txtPackingMaterialRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPackingMaterialRate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPackingMaterialRate.Location = new System.Drawing.Point(318, 50);
		this.txtPackingMaterialRate.Name = "txtPackingMaterialRate";
		this.txtPackingMaterialRate.Size = new System.Drawing.Size(125, 22);
		this.txtPackingMaterialRate.TabIndex = 6;
		this.txtPackingMaterialRate.TextChanged += new System.EventHandler(txtPackingMaterialRate_TextChanged);
		this.label40.AutoSize = true;
		this.label40.Location = new System.Drawing.Point(651, 26);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(27, 13);
		this.label40.TabIndex = 55;
		this.label40.Text = "Item";
		this.txtPackingMaterialQTY.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPackingMaterialQTY.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPackingMaterialQTY.Location = new System.Drawing.Point(103, 50);
		this.txtPackingMaterialQTY.Name = "txtPackingMaterialQTY";
		this.txtPackingMaterialQTY.Size = new System.Drawing.Size(122, 22);
		this.txtPackingMaterialQTY.TabIndex = 5;
		this.txtPackingMaterialQTY.TextChanged += new System.EventHandler(txtPackingMaterialQTY_TextChanged);
		this.cmbItemPackingMaterial.AllowNull = (DefaultableBoolean)1;
		this.cmbItemPackingMaterial.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemPackingMaterial.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemPackingMaterial.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance226).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance226).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.Appearance = (AppearanceBase)(object)appearance226;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance227).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance227).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance227).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance227).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance227;
		((AppearanceBase)appearance228).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance228;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance229).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance229).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance229).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance229).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance229;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance230).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance230).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance230;
		((AppearanceBase)appearance231).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance231).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance231;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance232).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance232;
		((AppearanceBase)appearance233).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance233).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance233;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance234).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance234).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance234).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance234).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance234).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance234;
		((AppearanceBase)appearance236).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance236;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance237).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance237).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance237;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance238).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance238;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemPackingMaterial).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemPackingMaterial).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItemPackingMaterial.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemPackingMaterial).Location = new System.Drawing.Point(706, 19);
		((System.Windows.Forms.Control)(object)this.cmbItemPackingMaterial).Name = "cmbItemPackingMaterial";
		((System.Windows.Forms.Control)(object)this.cmbItemPackingMaterial).Size = new System.Drawing.Size(265, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemPackingMaterial).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.cmbItemPackingMaterial).Leave += new System.EventHandler(cmbItemPackingMaterial_Leave);
		this.cmbUOMPackingMaterial.AllowNull = (DefaultableBoolean)1;
		this.cmbUOMPackingMaterial.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbUOMPackingMaterial.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbUOMPackingMaterial.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance159).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbUOMPackingMaterial).DisplayLayout.Appearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.cmbUOMPackingMaterial).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbUOMPackingMaterial).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance160).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance160).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance160).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbUOMPackingMaterial).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance160;
		((AppearanceBase)appearance161).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbUOMPackingMaterial).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance161;
		((SpecialBoxBase)((UltraGridBase)this.cmbUOMPackingMaterial).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance162).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance162).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance162).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbUOMPackingMaterial).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance162;
		((UltraGridBase)this.cmbUOMPackingMaterial).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbUOMPackingMaterial).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance163).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbUOMPackingMaterial).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance163;
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance164).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbUOMPackingMaterial).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.cmbUOMPackingMaterial).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbUOMPackingMaterial).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbUOMPackingMaterial).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance165;
		((AppearanceBase)appearance166).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance166).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbUOMPackingMaterial).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance166;
		((UltraGridBase)this.cmbUOMPackingMaterial).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbUOMPackingMaterial).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance167).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance167).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance167).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance167).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbUOMPackingMaterial).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance167;
		((AppearanceBase)appearance169).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbUOMPackingMaterial).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance169;
		((UltraGridBase)this.cmbUOMPackingMaterial).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbUOMPackingMaterial).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance170).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbUOMPackingMaterial).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance170;
		((UltraGridBase)this.cmbUOMPackingMaterial).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance171).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbUOMPackingMaterial).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance171;
		((UltraGridBase)this.cmbUOMPackingMaterial).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbUOMPackingMaterial).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbUOMPackingMaterial).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbUOMPackingMaterial).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbUOMPackingMaterial.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbUOMPackingMaterial).Location = new System.Drawing.Point(1015, 19);
		((System.Windows.Forms.Control)(object)this.cmbUOMPackingMaterial).Name = "cmbUOMPackingMaterial";
		((System.Windows.Forms.Control)(object)this.cmbUOMPackingMaterial).Size = new System.Drawing.Size(104, 26);
		((System.Windows.Forms.Control)(object)this.cmbUOMPackingMaterial).TabIndex = 4;
		this.label42.AutoSize = true;
		this.label42.Location = new System.Drawing.Point(977, 26);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(32, 13);
		this.label42.TabIndex = 57;
		this.label42.Text = "UOM";
		this.panel22.Controls.Add(this.toolStrip4);
		this.panel22.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel22.Location = new System.Drawing.Point(0, 0);
		this.panel22.Name = "panel22";
		this.panel22.Size = new System.Drawing.Size(1231, 30);
		this.panel22.TabIndex = 0;
		this.toolStrip4.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip4.Items.AddRange(new System.Windows.Forms.ToolStripItem[7] { this.toolStripButton3, this.btnNewpackingmaterial, this.btnsavepackingmaterial, this.btnUpdatepackingmaterial, this.toolStripSeparator3, this.toolStripButton9, this.btnVoucherPM });
		this.toolStrip4.Location = new System.Drawing.Point(0, 0);
		this.toolStrip4.Name = "toolStrip4";
		this.toolStrip4.Size = new System.Drawing.Size(1231, 27);
		this.toolStrip4.TabIndex = 58;
		this.toolStrip4.Text = "toolStrip4";
		this.toolStripButton3.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton3.Name = "toolStripButton3";
		this.toolStripButton3.Size = new System.Drawing.Size(164, 24);
		this.toolStripButton3.Text = "Packing Material         ";
		this.btnNewpackingmaterial.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNewpackingmaterial.Image = (System.Drawing.Image)resources.GetObject("btnNewpackingmaterial.Image");
		this.btnNewpackingmaterial.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewpackingmaterial.Name = "btnNewpackingmaterial";
		this.btnNewpackingmaterial.Size = new System.Drawing.Size(55, 24);
		this.btnNewpackingmaterial.Text = "&New";
		this.btnNewpackingmaterial.Click += new System.EventHandler(btnNewpackingmaterial_Click);
		this.btnsavepackingmaterial.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsavepackingmaterial.Image = (System.Drawing.Image)resources.GetObject("btnsavepackingmaterial.Image");
		this.btnsavepackingmaterial.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsavepackingmaterial.Name = "btnsavepackingmaterial";
		this.btnsavepackingmaterial.Size = new System.Drawing.Size(55, 24);
		this.btnsavepackingmaterial.Text = "&Save";
		this.btnsavepackingmaterial.Click += new System.EventHandler(btnsavepackingmaterial_Click);
		this.btnUpdatepackingmaterial.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdatepackingmaterial.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdatepackingmaterial.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdatepackingmaterial.Name = "btnUpdatepackingmaterial";
		this.btnUpdatepackingmaterial.Size = new System.Drawing.Size(69, 24);
		this.btnUpdatepackingmaterial.Text = "&Update";
		this.btnUpdatepackingmaterial.Visible = false;
		this.btnUpdatepackingmaterial.Click += new System.EventHandler(btnUpdatepackingmaterial_Click);
		this.toolStripSeparator3.Name = "toolStripSeparator3";
		this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
		this.toolStripButton9.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.toolStripButton9.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton9.Name = "toolStripButton9";
		this.toolStripButton9.Size = new System.Drawing.Size(56, 24);
		this.toolStripButton9.Text = "&Print";
		this.btnVoucherPM.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnVoucherPM.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnVoucherPM.Name = "btnVoucherPM";
		this.btnVoucherPM.Size = new System.Drawing.Size(97, 24);
		this.btnVoucherPM.Text = "118-Voucher";
		this.btnVoucherPM.Click += new System.EventHandler(btnVoucherOH_Click);
		this.OverHead.Controls.Add(this.panel8);
		this.OverHead.Location = new System.Drawing.Point(4, 22);
		this.OverHead.Name = "OverHead";
		this.OverHead.Size = new System.Drawing.Size(1231, 719);
		this.OverHead.TabIndex = 4;
		this.OverHead.Text = "Over Head";
		this.OverHead.UseVisualStyleBackColor = true;
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.grdOverHead);
		this.panel8.Controls.Add(this.panel27);
		this.panel8.Controls.Add(this.panel25);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel8.Location = new System.Drawing.Point(0, 0);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1231, 719);
		this.panel8.TabIndex = 0;
		this.grdOverHead.AllowDelete = (InheritableBoolean)1;
		grdOverHead_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdOverHead.DesignTimeLayout = grdOverHead_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdOverHead).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdOverHead.DynamicFiltering = true;
		this.grdOverHead.FilterMode = (FilterMode)2;
		this.grdOverHead.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdOverHead).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdOverHead.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdOverHead).Location = new System.Drawing.Point(0, 103);
		((System.Windows.Forms.Control)(object)this.grdOverHead).Name = "grdOverHead";
		((ScrollableBase)this.grdOverHead).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdOverHead).Size = new System.Drawing.Size(1231, 616);
		((System.Windows.Forms.Control)(object)this.grdOverHead).TabIndex = 46;
		this.grdOverHead.TotalRow = (InheritableBoolean)1;
		this.grdOverHead.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdOverHead).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdOverHead).DoubleClick += new System.EventHandler(grdOverHead_DoubleClick);
		this.panel27.BackColor = System.Drawing.Color.White;
		this.panel27.Controls.Add(this.label39);
		this.panel27.Controls.Add(this.label58);
		this.panel27.Controls.Add(this.txtLedgerRemarksOverHead);
		this.panel27.Controls.Add((System.Windows.Forms.Control)(object)this.cmbJobOrderOverHead);
		this.panel27.Controls.Add((System.Windows.Forms.Control)(object)this.cmbChartofAccountOverHead);
		this.panel27.Controls.Add(this.label59);
		this.panel27.Controls.Add(this.label57);
		this.panel27.Controls.Add(this.txtDocDateOverHead);
		this.panel27.Controls.Add(this.label38);
		this.panel27.Controls.Add(this.label56);
		this.panel27.Controls.Add(this.txtDocNoOverHead);
		this.panel27.Controls.Add(this.txtExpAmountOverHead);
		this.panel27.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel27.Location = new System.Drawing.Point(0, 28);
		this.panel27.Name = "panel27";
		this.panel27.Size = new System.Drawing.Size(1231, 75);
		this.panel27.TabIndex = 2;
		this.label39.AutoSize = true;
		this.label39.Location = new System.Drawing.Point(405, 18);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(70, 13);
		this.label39.TabIndex = 116;
		this.label39.Text = "Job Order No";
		this.label58.AutoSize = true;
		this.label58.Location = new System.Drawing.Point(15, 45);
		this.label58.Name = "label58";
		this.label58.Size = new System.Drawing.Size(85, 13);
		this.label58.TabIndex = 62;
		this.label58.Text = "Ledger Remarks";
		this.txtLedgerRemarksOverHead.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtLedgerRemarksOverHead.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtLedgerRemarksOverHead.Location = new System.Drawing.Point(105, 41);
		this.txtLedgerRemarksOverHead.Name = "txtLedgerRemarksOverHead";
		this.txtLedgerRemarksOverHead.Size = new System.Drawing.Size(458, 22);
		this.txtLedgerRemarksOverHead.TabIndex = 4;
		this.cmbJobOrderOverHead.AllowNull = (DefaultableBoolean)1;
		this.cmbJobOrderOverHead.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbJobOrderOverHead.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbJobOrderOverHead.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance172).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbJobOrderOverHead).DisplayLayout.Appearance = (AppearanceBase)(object)appearance172;
		((UltraGridBase)this.cmbJobOrderOverHead).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbJobOrderOverHead).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance173).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance173).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance173).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance173).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbJobOrderOverHead).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance173;
		((AppearanceBase)appearance174).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbJobOrderOverHead).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance174;
		((SpecialBoxBase)((UltraGridBase)this.cmbJobOrderOverHead).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance175).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance175).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance175).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbJobOrderOverHead).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance175;
		((UltraGridBase)this.cmbJobOrderOverHead).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbJobOrderOverHead).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance176).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance176).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbJobOrderOverHead).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance176;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance177).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbJobOrderOverHead).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance177;
		((UltraGridBase)this.cmbJobOrderOverHead).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbJobOrderOverHead).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbJobOrderOverHead).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance178;
		((AppearanceBase)appearance180).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance180).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbJobOrderOverHead).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance180;
		((UltraGridBase)this.cmbJobOrderOverHead).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbJobOrderOverHead).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance181).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance181).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance181).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance181).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance181).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbJobOrderOverHead).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance181;
		((AppearanceBase)appearance182).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbJobOrderOverHead).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance182;
		((UltraGridBase)this.cmbJobOrderOverHead).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbJobOrderOverHead).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance183).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance183).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbJobOrderOverHead).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance183;
		((UltraGridBase)this.cmbJobOrderOverHead).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance184).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbJobOrderOverHead).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance184;
		((UltraGridBase)this.cmbJobOrderOverHead).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbJobOrderOverHead).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbJobOrderOverHead).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbJobOrderOverHead).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbJobOrderOverHead.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbJobOrderOverHead).Location = new System.Drawing.Point(486, 12);
		((System.Windows.Forms.Control)(object)this.cmbJobOrderOverHead).Name = "cmbJobOrderOverHead";
		((System.Windows.Forms.Control)(object)this.cmbJobOrderOverHead).Size = new System.Drawing.Size(77, 26);
		((System.Windows.Forms.Control)(object)this.cmbJobOrderOverHead).TabIndex = 2;
		this.cmbChartofAccountOverHead.AllowNull = (DefaultableBoolean)1;
		this.cmbChartofAccountOverHead.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbChartofAccountOverHead.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbChartofAccountOverHead.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance185).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance185).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.Appearance = (AppearanceBase)(object)appearance185;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance186).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance186).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance186).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance186).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance186;
		((AppearanceBase)appearance187).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance187;
		((SpecialBoxBase)((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance188).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance188).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance188).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance188).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance188;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance189).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance189).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance189;
		((AppearanceBase)appearance191).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance191).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance191;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance192).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance192;
		((AppearanceBase)appearance193).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance193).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance193;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance194).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance194).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance194).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance194).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance194).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance194;
		((AppearanceBase)appearance195).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance195;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance196).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance196).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance196;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance197).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance197;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbChartofAccountOverHead).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbChartofAccountOverHead).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbChartofAccountOverHead.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbChartofAccountOverHead).Location = new System.Drawing.Point(678, 12);
		((System.Windows.Forms.Control)(object)this.cmbChartofAccountOverHead).Name = "cmbChartofAccountOverHead";
		((System.Windows.Forms.Control)(object)this.cmbChartofAccountOverHead).Size = new System.Drawing.Size(245, 26);
		((System.Windows.Forms.Control)(object)this.cmbChartofAccountOverHead).TabIndex = 3;
		this.label59.AutoSize = true;
		this.label59.Location = new System.Drawing.Point(569, 45);
		this.label59.Name = "label59";
		this.label59.Size = new System.Drawing.Size(43, 13);
		this.label59.TabIndex = 60;
		this.label59.Text = "Amount";
		this.label57.AutoSize = true;
		this.label57.Location = new System.Drawing.Point(569, 19);
		this.label57.Name = "label57";
		this.label57.Size = new System.Drawing.Size(99, 13);
		this.label57.TabIndex = 60;
		this.label57.Text = "OverHead Account";
		this.txtDocDateOverHead.CustomFormat = "dd-MMM-yy";
		this.txtDocDateOverHead.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDocDateOverHead.Location = new System.Drawing.Point(274, 15);
		this.txtDocDateOverHead.Name = "txtDocDateOverHead";
		this.txtDocDateOverHead.Size = new System.Drawing.Size(125, 20);
		this.txtDocDateOverHead.TabIndex = 1;
		this.label38.AutoSize = true;
		this.label38.Location = new System.Drawing.Point(185, 19);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(82, 13);
		this.label38.TabIndex = 57;
		this.label38.Text = "Document Date";
		this.label56.AutoSize = true;
		this.label56.Location = new System.Drawing.Point(15, 19);
		this.label56.Name = "label56";
		this.label56.Size = new System.Drawing.Size(73, 13);
		this.label56.TabIndex = 56;
		this.label56.Text = "Document No";
		this.txtDocNoOverHead.BackColor = System.Drawing.Color.White;
		this.txtDocNoOverHead.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDocNoOverHead.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNoOverHead.Location = new System.Drawing.Point(105, 14);
		this.txtDocNoOverHead.Name = "txtDocNoOverHead";
		this.txtDocNoOverHead.ReadOnly = true;
		this.txtDocNoOverHead.Size = new System.Drawing.Size(74, 22);
		this.txtDocNoOverHead.TabIndex = 0;
		this.txtExpAmountOverHead.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtExpAmountOverHead.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtExpAmountOverHead.Location = new System.Drawing.Point(678, 41);
		this.txtExpAmountOverHead.Name = "txtExpAmountOverHead";
		this.txtExpAmountOverHead.Size = new System.Drawing.Size(245, 22);
		this.txtExpAmountOverHead.TabIndex = 5;
		this.panel25.Controls.Add(this.toolStrip5);
		this.panel25.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel25.Location = new System.Drawing.Point(0, 0);
		this.panel25.Name = "panel25";
		this.panel25.Size = new System.Drawing.Size(1231, 28);
		this.panel25.TabIndex = 0;
		this.toolStrip5.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip5.Items.AddRange(new System.Windows.Forms.ToolStripItem[7] { this.toolStripButton2, this.btnNewOverHead, this.btnSaveOverHead, this.btnUpdateOverHead, this.toolStripSeparator4, this.btnPrintOverHead, this.btnVoucherOH });
		this.toolStrip5.Location = new System.Drawing.Point(0, 0);
		this.toolStrip5.Name = "toolStrip5";
		this.toolStrip5.Size = new System.Drawing.Size(1231, 27);
		this.toolStrip5.TabIndex = 58;
		this.toolStrip5.Text = "toolStrip5";
		this.toolStripButton2.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton2.Name = "toolStripButton2";
		this.toolStripButton2.Size = new System.Drawing.Size(129, 24);
		this.toolStripButton2.Text = "Over Head           ";
		this.btnNewOverHead.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNewOverHead.Image = (System.Drawing.Image)resources.GetObject("btnNewOverHead.Image");
		this.btnNewOverHead.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewOverHead.Name = "btnNewOverHead";
		this.btnNewOverHead.Size = new System.Drawing.Size(55, 24);
		this.btnNewOverHead.Text = "&New";
		this.btnNewOverHead.Click += new System.EventHandler(btnNewOverHead_Click);
		this.btnSaveOverHead.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSaveOverHead.Image = (System.Drawing.Image)resources.GetObject("btnSaveOverHead.Image");
		this.btnSaveOverHead.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSaveOverHead.Name = "btnSaveOverHead";
		this.btnSaveOverHead.Size = new System.Drawing.Size(55, 24);
		this.btnSaveOverHead.Text = "&Save";
		this.btnSaveOverHead.Click += new System.EventHandler(btnSaveOverHead_Click);
		this.btnUpdateOverHead.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateOverHead.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdateOverHead.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdateOverHead.Name = "btnUpdateOverHead";
		this.btnUpdateOverHead.Size = new System.Drawing.Size(69, 24);
		this.btnUpdateOverHead.Text = "&Update";
		this.btnUpdateOverHead.Visible = false;
		this.btnUpdateOverHead.Click += new System.EventHandler(btnUpdateOverHead_Click);
		this.toolStripSeparator4.Name = "toolStripSeparator4";
		this.toolStripSeparator4.Size = new System.Drawing.Size(6, 27);
		this.btnPrintOverHead.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrintOverHead.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrintOverHead.Name = "btnPrintOverHead";
		this.btnPrintOverHead.Size = new System.Drawing.Size(56, 24);
		this.btnPrintOverHead.Text = "&Print";
		this.btnVoucherOH.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnVoucherOH.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnVoucherOH.Name = "btnVoucherOH";
		this.btnVoucherOH.Size = new System.Drawing.Size(97, 24);
		this.btnVoucherOH.Text = "118-Voucher";
		this.btnVoucherOH.Click += new System.EventHandler(btnVoucherOH_Click_1);
		this.tabPage2.Controls.Add(this.panel10);
		this.tabPage2.Location = new System.Drawing.Point(4, 22);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1231, 719);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "Transaction History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel10.Controls.Add(this.panel13);
		this.panel10.Controls.Add(this.panel11);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel10.Location = new System.Drawing.Point(3, 3);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1225, 713);
		this.panel10.TabIndex = 0;
		this.panel13.Controls.Add(this.panel17);
		this.panel13.Controls.Add(this.panel16);
		this.panel13.Controls.Add(this.panel15);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel13.Location = new System.Drawing.Point(0, 26);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1225, 687);
		this.panel13.TabIndex = 2;
		this.panel17.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel17.Location = new System.Drawing.Point(0, 294);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(1225, 393);
		this.panel17.TabIndex = 2;
		grdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdDetail.DynamicFiltering = true;
		this.grdDetail.FilterMode = (FilterMode)2;
		this.grdDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1225, 393);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 45;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.panel16.Controls.Add((System.Windows.Forms.Control)(object)this.DatagridHistory);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel16.Location = new System.Drawing.Point(0, 10);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(1225, 284);
		this.panel16.TabIndex = 1;
		DatagridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DatagridHistory.DesignTimeLayout = DatagridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DatagridHistory.DynamicFiltering = true;
		this.DatagridHistory.FilterMode = (FilterMode)2;
		this.DatagridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DatagridHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Name = "DatagridHistory";
		((ScrollableBase)this.DatagridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Size = new System.Drawing.Size(1225, 284);
		((System.Windows.Forms.Control)(object)this.DatagridHistory).TabIndex = 44;
		this.DatagridHistory.TotalRow = (InheritableBoolean)1;
		this.DatagridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DatagridHistory).VisualStyle = (VisualStyle)4;
		this.DatagridHistory.ColumnButtonClick += new ColumnActionEventHandler(DatagridHistory_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Click += new System.EventHandler(DatagridHistory_Click);
		this.panel15.Controls.Add(this.btnSearch);
		this.panel15.Controls.Add(this.label50);
		this.panel15.Controls.Add(this.txtDocNumberTo);
		this.panel15.Controls.Add(this.txtToDate);
		this.panel15.Controls.Add(this.label52);
		this.panel15.Controls.Add(this.txtfromDate);
		this.panel15.Controls.Add(this.label48);
		this.panel15.Controls.Add(this.label49);
		this.panel15.Controls.Add(this.txtDocNoFrom);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.Location = new System.Drawing.Point(0, 0);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(1225, 10);
		this.panel15.TabIndex = 0;
		this.btnSearch.Location = new System.Drawing.Point(864, 16);
		this.btnSearch.Name = "btnSearch";
		this.btnSearch.Size = new System.Drawing.Size(75, 23);
		this.btnSearch.TabIndex = 55;
		this.btnSearch.Text = "Search";
		this.btnSearch.UseVisualStyleBackColor = true;
		this.label50.AutoSize = true;
		this.label50.Location = new System.Drawing.Point(660, 23);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(60, 13);
		this.label50.TabIndex = 54;
		this.label50.Text = "Doc No To";
		this.txtDocNumberTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDocNumberTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNumberTo.Location = new System.Drawing.Point(726, 18);
		this.txtDocNumberTo.Name = "txtDocNumberTo";
		this.txtDocNumberTo.Size = new System.Drawing.Size(113, 22);
		this.txtDocNumberTo.TabIndex = 53;
		this.txtToDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtToDate.Location = new System.Drawing.Point(298, 19);
		this.txtToDate.Name = "txtToDate";
		this.txtToDate.Size = new System.Drawing.Size(153, 20);
		this.txtToDate.TabIndex = 51;
		this.label52.AutoSize = true;
		this.label52.Location = new System.Drawing.Point(246, 23);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(46, 13);
		this.label52.TabIndex = 52;
		this.label52.Text = "Date To";
		this.txtfromDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtfromDate.Location = new System.Drawing.Point(87, 19);
		this.txtfromDate.Name = "txtfromDate";
		this.txtfromDate.Size = new System.Drawing.Size(153, 20);
		this.txtfromDate.TabIndex = 45;
		this.label48.AutoSize = true;
		this.label48.Location = new System.Drawing.Point(25, 23);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(56, 13);
		this.label48.TabIndex = 47;
		this.label48.Text = "Date From";
		this.label49.AutoSize = true;
		this.label49.Location = new System.Drawing.Point(462, 23);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(70, 13);
		this.label49.TabIndex = 49;
		this.label49.Text = "Doc No From";
		this.txtDocNoFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDocNoFrom.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNoFrom.Location = new System.Drawing.Point(538, 18);
		this.txtDocNoFrom.Name = "txtDocNoFrom";
		this.txtDocNoFrom.Size = new System.Drawing.Size(113, 22);
		this.txtDocNoFrom.TabIndex = 46;
		this.panel11.Controls.Add(this.ctrlGrdBar2);
		this.panel11.Controls.Add(this.ctrlGrdBar1);
		this.panel11.Controls.Add(this.toolStrip2);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(0, 0);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1225, 26);
		this.panel11.TabIndex = 0;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1134, 0);
		this.ctrlGrdBar2.MyGrid = this.grdDetail;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(41, 24);
		this.ctrlGrdBar2.TabIndex = 5;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1181, 0);
		this.ctrlGrdBar1.MyGrid = this.DatagridHistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(41, 24);
		this.ctrlGrdBar1.TabIndex = 4;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.toolStripSeparator1, this.toolStripButton7, this.toolStripButton4 });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1225, 27);
		this.toolStrip2.TabIndex = 2;
		this.toolStrip2.Text = "toolStrip2";
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.toolStripButton7.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton7.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton7.Name = "toolStripButton7";
		this.toolStripButton7.Size = new System.Drawing.Size(78, 24);
		this.toolStripButton7.Text = "History    ";
		this.toolStripButton4.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton4.Name = "toolStripButton4";
		this.toolStripButton4.Size = new System.Drawing.Size(56, 24);
		this.toolStripButton4.Text = "&Print";
		this.tabPage3.Controls.Add(this.panel18);
		this.tabPage3.Location = new System.Drawing.Point(4, 22);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Size = new System.Drawing.Size(1231, 719);
		this.tabPage3.TabIndex = 2;
		this.tabPage3.Text = "Summery";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.panel18.Controls.Add(this.panel21);
		this.panel18.Controls.Add(this.panel19);
		this.panel18.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel18.Location = new System.Drawing.Point(0, 0);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(1231, 719);
		this.panel18.TabIndex = 0;
		this.panel21.BackColor = System.Drawing.Color.White;
		this.panel21.Controls.Add(this.panel26);
		this.panel21.Controls.Add(this.panel23);
		this.panel21.Controls.Add(this.panel20);
		this.panel21.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel21.Location = new System.Drawing.Point(0, 30);
		this.panel21.Name = "panel21";
		this.panel21.Size = new System.Drawing.Size(1231, 685);
		this.panel21.TabIndex = 2;
		this.panel26.Controls.Add((System.Windows.Forms.Control)(object)this.grdSummeryReport);
		this.panel26.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel26.Location = new System.Drawing.Point(0, 274);
		this.panel26.Name = "panel26";
		this.panel26.Size = new System.Drawing.Size(1231, 411);
		this.panel26.TabIndex = 122;
		this.grdSummeryReport.AllowEdit = (InheritableBoolean)2;
		grdSummeryReport_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdSummeryReport.DesignTimeLayout = grdSummeryReport_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdSummeryReport).Dock = System.Windows.Forms.DockStyle.Top;
		this.grdSummeryReport.DynamicFiltering = true;
		this.grdSummeryReport.FilterRowButtonStyle = (FilterRowButtonStyle)1;
		((System.Windows.Forms.Control)(object)this.grdSummeryReport).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdSummeryReport.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdSummeryReport).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdSummeryReport).Name = "grdSummeryReport";
		((ScrollableBase)this.grdSummeryReport).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdSummeryReport).Size = new System.Drawing.Size(1231, 406);
		((System.Windows.Forms.Control)(object)this.grdSummeryReport).TabIndex = 119;
		this.grdSummeryReport.TotalRow = (InheritableBoolean)1;
		this.grdSummeryReport.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdSummeryReport).VisualStyle = (VisualStyle)4;
		this.panel23.Controls.Add((System.Windows.Forms.Control)(object)this.grdSummery);
		this.panel23.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel23.Location = new System.Drawing.Point(0, 41);
		this.panel23.Name = "panel23";
		this.panel23.Size = new System.Drawing.Size(1231, 233);
		this.panel23.TabIndex = 121;
		this.grdSummery.AllowEdit = (InheritableBoolean)2;
		grdSummery_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdSummery.DesignTimeLayout = grdSummery_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdSummery).Dock = System.Windows.Forms.DockStyle.Left;
		this.grdSummery.DynamicFiltering = true;
		this.grdSummery.FilterRowButtonStyle = (FilterRowButtonStyle)1;
		((System.Windows.Forms.Control)(object)this.grdSummery).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdSummery.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdSummery).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdSummery).Name = "grdSummery";
		((ScrollableBase)this.grdSummery).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdSummery).Size = new System.Drawing.Size(1237, 233);
		((System.Windows.Forms.Control)(object)this.grdSummery).TabIndex = 118;
		this.grdSummery.TotalRow = (InheritableBoolean)1;
		this.grdSummery.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdSummery).VisualStyle = (VisualStyle)4;
		this.panel20.Controls.Add((System.Windows.Forms.Control)(object)this.cmbsummeryJobOrderNo);
		this.panel20.Controls.Add(this.label68);
		this.panel20.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel20.Location = new System.Drawing.Point(0, 0);
		this.panel20.Name = "panel20";
		this.panel20.Size = new System.Drawing.Size(1231, 41);
		this.panel20.TabIndex = 120;
		this.cmbsummeryJobOrderNo.AllowNull = (DefaultableBoolean)1;
		this.cmbsummeryJobOrderNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbsummeryJobOrderNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbsummeryJobOrderNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance198).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance198).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbsummeryJobOrderNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance198;
		((UltraGridBase)this.cmbsummeryJobOrderNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbsummeryJobOrderNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance199).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance199).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance199).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance199).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbsummeryJobOrderNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance199;
		((AppearanceBase)appearance200).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbsummeryJobOrderNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance200;
		((SpecialBoxBase)((UltraGridBase)this.cmbsummeryJobOrderNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance202).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance202).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance202).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance202).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbsummeryJobOrderNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance202;
		((UltraGridBase)this.cmbsummeryJobOrderNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbsummeryJobOrderNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance203).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance203).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbsummeryJobOrderNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance203;
		((AppearanceBase)appearance204).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance204).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbsummeryJobOrderNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance204;
		((UltraGridBase)this.cmbsummeryJobOrderNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbsummeryJobOrderNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance205).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbsummeryJobOrderNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance205;
		((AppearanceBase)appearance206).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance206).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbsummeryJobOrderNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance206;
		((UltraGridBase)this.cmbsummeryJobOrderNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbsummeryJobOrderNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance207).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance207).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance207).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance207).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance207).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbsummeryJobOrderNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance207;
		((AppearanceBase)appearance208).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbsummeryJobOrderNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance208;
		((UltraGridBase)this.cmbsummeryJobOrderNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbsummeryJobOrderNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance209).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance209).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbsummeryJobOrderNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance209;
		((UltraGridBase)this.cmbsummeryJobOrderNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance210).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbsummeryJobOrderNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance210;
		((UltraGridBase)this.cmbsummeryJobOrderNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbsummeryJobOrderNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbsummeryJobOrderNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbsummeryJobOrderNo).Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbsummeryJobOrderNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbsummeryJobOrderNo).Location = new System.Drawing.Point(82, 10);
		((System.Windows.Forms.Control)(object)this.cmbsummeryJobOrderNo).Name = "cmbsummeryJobOrderNo";
		((System.Windows.Forms.Control)(object)this.cmbsummeryJobOrderNo).Size = new System.Drawing.Size(145, 25);
		((System.Windows.Forms.Control)(object)this.cmbsummeryJobOrderNo).TabIndex = 116;
		((System.Windows.Forms.Control)(object)this.cmbsummeryJobOrderNo).Leave += new System.EventHandler(cmbsummeryJobOrderNo_TextChanged);
		this.label68.AutoSize = true;
		this.label68.Location = new System.Drawing.Point(6, 16);
		this.label68.Name = "label68";
		this.label68.Size = new System.Drawing.Size(70, 13);
		this.label68.TabIndex = 117;
		this.label68.Text = "Job Order No";
		this.panel19.Controls.Add(this.ctrlGrdBar5);
		this.panel19.Controls.Add(this.ctrlGrdBar6);
		this.panel19.Controls.Add(this.toolStrip3);
		this.panel19.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel19.Location = new System.Drawing.Point(0, 0);
		this.panel19.Name = "panel19";
		this.panel19.Size = new System.Drawing.Size(1231, 30);
		this.panel19.TabIndex = 0;
		this.ctrlGrdBar5.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar5.FormName = this;
		this.ctrlGrdBar5.Location = new System.Drawing.Point(1137, 3);
		this.ctrlGrdBar5.MyGrid = this.grdSummery;
		this.ctrlGrdBar5.Name = "ctrlGrdBar5";
		this.ctrlGrdBar5.Size = new System.Drawing.Size(41, 24);
		this.ctrlGrdBar5.TabIndex = 60;
		this.ctrlGrdBar5.Load += new System.EventHandler(ctrlGrdBar5_Load);
		this.ctrlGrdBar6.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar6.FormName = this;
		this.ctrlGrdBar6.Location = new System.Drawing.Point(1184, 3);
		this.ctrlGrdBar6.MyGrid = this.grdSummeryReport;
		this.ctrlGrdBar6.Name = "ctrlGrdBar6";
		this.ctrlGrdBar6.Size = new System.Drawing.Size(41, 24);
		this.ctrlGrdBar6.TabIndex = 59;
		this.ctrlGrdBar6.Load += new System.EventHandler(ctrlGrdBar6_Load);
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.toolStripButton6, this.toolStripButton1, this.toolStripSeparator2, this.btnProductionRecoveryReport });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1231, 27);
		this.toolStrip3.TabIndex = 58;
		this.toolStrip3.Text = "toolStrip3";
		this.toolStripButton6.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton6.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton6.Name = "toolStripButton6";
		this.toolStripButton6.Size = new System.Drawing.Size(118, 24);
		this.toolStripButton6.Text = "Summery          ";
		this.toolStripButton1.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton1.Image = (System.Drawing.Image)resources.GetObject("toolStripButton1.Image");
		this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton1.Name = "toolStripButton1";
		this.toolStripButton1.Size = new System.Drawing.Size(55, 24);
		this.toolStripButton1.Text = "&New";
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
		this.btnProductionRecoveryReport.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnProductionRecoveryReport.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnProductionRecoveryReport.Name = "btnProductionRecoveryReport";
		this.btnProductionRecoveryReport.Size = new System.Drawing.Size(137, 24);
		this.btnProductionRecoveryReport.Text = "602-RecoveryReport";
		this.btnProductionRecoveryReport.Click += new System.EventHandler(btnProductionRecoveryReport_Click);
		this.timer1.Tick += new System.EventHandler(timer1_Tick_1);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1239, 745);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmFoodProduction";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(frmFoodProduction_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmFoodProduction_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.InPut.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdStockConversionProductionDetail).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel29.ResumeLayout(false);
		this.panel29.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbUOM).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbBagType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbGodown).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbLot).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropyr).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRateUOMInput).EndInit();
		this.panel28.ResumeLayout(false);
		this.panel28.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbJobOrderNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWIPItem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWorkInProcess).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.OutPut.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		this.panel14.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdByProduct).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel31.ResumeLayout(false);
		this.panel31.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbStockAccount).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackTypeOutPut).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbEntryType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRateUomOutPut).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYearOutPut).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemOutput).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbLotOutPut).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWarehouseOutput).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbUomOutPut).EndInit();
		this.panel30.ResumeLayout(false);
		this.panel30.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbJobOrderNoOutput).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.toolStrip6.ResumeLayout(false);
		this.toolStrip6.PerformLayout();
		this.PackingMaterial.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		this.panel9.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPackingMaterial).EndInit();
		this.panel24.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbJobOrderPackingMaterial).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemPackingMaterial).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbUOMPackingMaterial).EndInit();
		this.panel22.ResumeLayout(false);
		this.panel22.PerformLayout();
		this.toolStrip4.ResumeLayout(false);
		this.toolStrip4.PerformLayout();
		this.OverHead.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdOverHead).EndInit();
		this.panel27.ResumeLayout(false);
		this.panel27.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbJobOrderOverHead).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbChartofAccountOverHead).EndInit();
		this.panel25.ResumeLayout(false);
		this.panel25.PerformLayout();
		this.toolStrip5.ResumeLayout(false);
		this.toolStrip5.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel10.ResumeLayout(false);
		this.panel13.ResumeLayout(false);
		this.panel17.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel16.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DatagridHistory).EndInit();
		this.panel15.ResumeLayout(false);
		this.panel15.PerformLayout();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		this.tabPage3.ResumeLayout(false);
		this.panel18.ResumeLayout(false);
		this.panel21.ResumeLayout(false);
		this.panel26.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdSummeryReport).EndInit();
		this.panel23.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdSummery).EndInit();
		this.panel20.ResumeLayout(false);
		this.panel20.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbsummeryJobOrderNo).EndInit();
		this.panel19.ResumeLayout(false);
		this.panel19.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
