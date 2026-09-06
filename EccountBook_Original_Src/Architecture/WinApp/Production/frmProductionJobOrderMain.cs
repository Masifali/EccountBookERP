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
using Architecture.BLL.Production;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.Model.Main;
using Architecture.Model.Production;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Production;

public class frmProductionJobOrderMain : Form
{
	private Rightsobjects formright = new Rightsobjects();

	private Attachment AT = new Attachment();

	public DataTable dtViewAttachments = new DataTable();

	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private bool UpdateMode;

	private int RECID;

	private int GetLastWipAccountId;

	private int GetLastWipItemId;

	private int GetLastWipWareHouseId;

	private DataTable dtDetail = new DataTable();

	private DataTable dtByProduct = new DataTable();

	private DataTable dtFinish = new DataTable();

	private DataTable dtExportSchedule = new DataTable();

	private int UpdateDetailIndex = -1;

	private int UpdateDetailIndexForByProduct = -1;

	private int UpdateDetailIndexForFinishGoods = -1;

	private bool ByProductRateEditAbleIsAllow;

	private bool GenerateJobOrderNo;

	private LoadExportScheduleForProduction Loader;

	private IContainer components;

	private ToolStrip miniToolStrip;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private Panel panel12;

	internal GridEX DataGridHistory;

	private Panel panel8;

	private ToolStrip toolStrip2;

	private ToolStripButton btnNewHistory;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripButton toolStripButton4;

	private Panel panel2;

	private Panel panel4;

	private Button btnProductionPlant;

	internal UltraCombo CmbWipItem;

	private Label label23;

	internal UltraCombo CmbByProductionAc;

	private Label label21;

	internal UltraCombo CmbFinishGoodsAc;

	private Label label20;

	internal UltraCombo CmbWorkInProcess;

	private Label label15;

	private CheckBox chkPrint;

	private Label label19;

	private TextBox txtPlanTypeSrNo;

	internal UltraCombo cmbStatus;

	internal UltraCombo cmbProductionType;

	internal UltraCombo cmbPlantFeader;

	private DateTimePicker txtEndDate;

	private DateTimePicker txtstartDate;

	private Label label16;

	private Label label13;

	private Label label11;

	private TextBox txtOtherInstruction;

	private Label label10;

	internal UltraCombo cmbPlanType;

	private Label label3;

	private TextBox txtOtherReference;

	private Label label1;

	private TextBox txtProductionNo;

	private Label label2;

	private Label label8;

	private Label label7;

	private TextBox txtPlanCode;

	private DateTimePicker txtPlanDate;

	private Panel panel3;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnsave;

	private ToolStripButton btnUpdate;

	private ToolStripButton Print;

	private ToolStripButton btnattachment;

	private ToolStripButton btnRefresh;

	private Label label4;

	private TextBox txtAvgFinishGoodRate;

	private Button btnPlanTypeDefine;

	private Button btnProductionTypeDefine;

	private ToolStripButton btnSaveAs;

	private Button btnCancelUpdateDetial;

	private Button btnUpdateDetail;

	private Button btnplus;

	private Panel panel5;

	internal GridEX GridDetail;

	private GroupBox groupBox2;

	private TextBox txtRemarksDetail;

	private Label label6;

	private Label label5;

	private Panel panel10;

	private Panel panel9;

	private Label label9;

	private CtrlGrdBar ctrlGrdBar3;

	private CtrlGrdBar ctrlGrdBar1;

	private Panel panel6;

	private Panel panel7;

	private Panel panel11;

	private Panel panel13;

	private Panel panel15;

	private Panel panel14;

	private Label label12;

	private CtrlGrdBar ctrlGrdBar2;

	private Label label17;

	private Label label14;

	internal UltraCombo CmbItemByProduct;

	private DateTimePicker datEffectiveDateByProduct;

	private Label label22;

	private TextBox txtRateByProduct;

	private Button btnCancelByProduct;

	private Button btnAddUpdateByProduct;

	private Panel panel16;

	private Panel panel17;

	private Button btnCancelFinish;

	private Label label24;

	private TextBox txtRateFinish;

	private Button btnAddUpdateFinish;

	private Label label25;

	private Label label26;

	internal UltraCombo CmbItemFinish;

	private DateTimePicker datEffectiveDateFinish;

	private Panel panel18;

	private Label label27;

	private CtrlGrdBar ctrlGrdBar4;

	private Panel panel19;

	internal GridEX grdFinishGoods;

	internal GridEX grdByProduct;

	private Panel panel21;

	private Panel panel20;

	private Button btnshowHistory;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	private Label label31;

	internal UltraCombo xmbjoborderno;

	private Label label33;

	internal UltraCombo cmbperemeter;

	private Label label28;

	private Label label30;

	private TextBox txtOrderNoTo;

	private TextBox txtOrderNoFrom;

	private Label label29;

	private Label label32;

	private RadioButton rdapproveddate;

	private RadioButton rdmodifydate;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	private Label label18;

	private TabControl tabControl2;

	private TabPage tabPage3;

	private TabPage tabPage4;

	private Panel panel22;

	private Label label34;

	internal GridEX grdScheduleInfo;

	private Button btnLoadScheduleInfo;

	internal UltraCombo CmbProductionNo;

	private ToolStripButton btnGenerateProductionNo;

	internal UltraCombo CmbWipWarehouse;

	private Label label35;

	private Label label36;

	public frmProductionJobOrderMain(UserAccount user)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void frmProductionJobOrder_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1280, 630);
			formright = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnsave.Enabled = formright.DoHaveSaveRight;
			Print.Enabled = formright.DoHavePrintRights;
			btnUpdate.Enabled = formright.DoHaveUpdateRights;
			ByProductRateEditAbleIsAllow = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("ByProductRateEditableIsAllow"));
			GenerateJobOrderNo = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("GenerateJobOrderNo"));
			btnGenerateProductionNo.Visible = GenerateJobOrderNo;
			((Control)(object)CmbProductionNo).Visible = GenerateJobOrderNo;
			txtProductionNo.Visible = !GenerateJobOrderNo;
			if (GenerateJobOrderNo)
			{
				BindJobOrderNo();
			}
			dtViewAttachments.Columns.Add("AttchmentPath");
			ParameterFill();
			JobOrderNofill();
			AccountFills();
			ItemFill();
			PlanTypeBind();
			GeneratePlanCode();
			PlantFeader();
			ProductionTypeBind();
			StatusBind();
			GetLastSaveAccount();
			ItemComboFillForDetailSchedules();
			WarehousesDtFillFromGlobalAndBind();
			if (GetLastWipAccountId > 0)
			{
				CmbWorkInProcess.Value = GetLastWipAccountId;
			}
			if (GetLastWipItemId > 0)
			{
				CmbWipItem.Value = GetLastWipItemId;
			}
			if (GetLastWipWareHouseId > 0)
			{
				CmbWipWarehouse.Value = GetLastWipWareHouseId;
			}
			txtPlanDate.Focus();
			dtDetail.Columns.Add("Id");
			dtDetail.Columns.Add("PlantId");
			dtDetail.Columns.Add("PlantName");
			dtDetail.Columns.Add("RemarksDetail");
			GridDetail.DataSource = dtDetail;
			GridDetail.RetrieveStructure();
			GridDetailSetting();
			dtByProduct.Columns.Add("Id", typeof(int));
			dtByProduct.Columns.Add("EffectiveDate", typeof(DateTime));
			dtByProduct.Columns.Add("ItemId", typeof(int));
			dtByProduct.Columns.Add("ItemName", typeof(string));
			dtByProduct.Columns.Add("Rate40Kg", typeof(double));
			grdByProduct.DataSource = dtByProduct;
			grdByProduct.RetrieveStructure();
			GridByProductSetting();
			dtFinish.Columns.Add("Id", typeof(int));
			dtFinish.Columns.Add("EffectiveDate", typeof(DateTime));
			dtFinish.Columns.Add("ItemId", typeof(int));
			dtFinish.Columns.Add("ItemName", typeof(string));
			dtFinish.Columns.Add("Rate40Kg", typeof(double));
			grdFinishGoods.DataSource = dtFinish;
			grdFinishGoods.RetrieveStructure();
			GridFinishGoodsSetting();
			dtExportSchedule.Columns.Add("Id", typeof(int));
			dtExportSchedule.Columns.Add("ContractId", typeof(int));
			dtExportSchedule.Columns.Add("SaleContract", typeof(string));
			dtExportSchedule.Columns.Add("ScheduleId", typeof(int));
			dtExportSchedule.Columns.Add("ScheduleNo", typeof(string));
			dtExportSchedule.Columns.Add("ItemId", typeof(int));
			dtExportSchedule.Columns.Add("Item", typeof(string));
			dtExportSchedule.Columns.Add("MTon", typeof(double));
			dtExportSchedule.Columns.Add("Remarks", typeof(string));
			grdScheduleInfo.DataSource = dtExportSchedule;
			grdScheduleInfo.RetrieveStructure();
			ScheduleGridSetting();
			btnplus.Visible = true;
			btnUpdate.Visible = false;
			btnSaveAs.Visible = false;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			btnAddUpdateByProduct.Visible = true;
			btnAddUpdateFinish.Visible = true;
			btnAddUpdateByProduct.Text = "Add";
			btnAddUpdateFinish.Text = "Add";
			btnCancelByProduct.Visible = false;
			btnCancelFinish.Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindJobOrderNo(string JobOrderNo = null)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		try
		{
			DataTable dt = GenerateExportInvoiceNos.GetFinalInvoiceNosForJobOrder(new GenerateExportInvoiceNos
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinalInvoiceNo = JobOrderNo,
				DocumentTypeId = 403
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbProductionNo, "Id", "FinalInvoiceNo", "JobOrderNo", false);
				((UltraGridBase)CmbProductionNo).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void WarehousesDtFillFromGlobalAndBind()
	{
		try
		{
			DataTable dt = DatatableHelper.PopulateDataTableAndReturn(clsGlobalVariables.globalWarehousesWithBranches, (GlobalWarehousesWithBranch x) => x.WarehouseId, (GlobalWarehousesWithBranch x) => x.WarehouseName);
			InfragisticsHelper.BindAndRetainSelection(CmbWipWarehouse, dt, "Id", "Description", "WareHouse Name", AllColumns: true, Conversion.ToInt(CmbWipWarehouse.Value), insertDefaultRow: true);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void JobOrderNofill()
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		try
		{
			DataTable dtorder = new DataTable();
			dtorder.Columns.Add("Id");
			dtorder.Columns.Add("name");
			DataTable dtjob = InvProductionJobOrder.JobOrderNo(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtjob.Rows.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < dtjob.Rows.Count; i++)
			{
				if (Conversion.ToString(dtjob.Rows[i]["Activity"]) == "JobOrderNo")
				{
					dtorder.Rows.Add(dtjob.Rows[i]["Id"], dtjob.Rows[i]["ReferenceName"]);
				}
			}
			if (dtorder.Rows.Count > 0)
			{
				DDL.BindDDL(dtorder, xmbjoborderno, "Id", "name", "Job Order No", ZeroIndex: false);
				((UltraGridBase)xmbjoborderno).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
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
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbperemeter, "Id", "Parameters", "Parameters", ZeroIndex: false);
				((UltraGridBase)cmbperemeter).Rows[0].Activate();
			}
			else
			{
				((Control)(object)cmbperemeter).Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetLastSaveAccount()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		try
		{
			DataTable dt = InvProductionJobOrder.GetLastWipAccount(new InvProductionJobOrder
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = 403
			});
			if (dt.Rows.Count > 0)
			{
				GetLastWipAccountId = Conversion.ToInt(dt.Rows[0]["AccountId"]);
				GetLastWipItemId = Conversion.ToInt(dt.Rows[0]["WipItemId"]);
				GetLastWipWareHouseId = Conversion.ToInt(dt.Rows[0]["WipWareHouseId"]);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PlantFeader()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dt = PlantsAllocationToBranch.GetPlantsAllocatedToBranch(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbPlantFeader, "PlantId", "PlantName", "PlantName ", ZeroIndex: false);
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
			DataTable dtItem = CommonServices.ItemGetForComboServiceBind(6);
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

	private void ItemComboFillForDetailSchedules()
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
			if (lst.Rows.Count > 0)
			{
				DDL.BindDDLNew(lst, CmbItemByProduct, "Id", "ItemName", "ItemName", false);
				((UltraGridBase)CmbItemByProduct).DisplayLayout.Bands[0].Columns[1].Width = 400;
				DDL.BindDDLNew(lst, CmbItemFinish, "Id", "ItemName", "ItemName", false);
				((UltraGridBase)CmbItemFinish).DisplayLayout.Bands[0].Columns[1].Width = 400;
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
			DataTable dtCoa = CommonServices.CoaAllocationAccountTitleByAccountTypeIds("4");
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
				DocumentTypeId = 403,
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
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Expected O, but got Unknown
		try
		{
			DataTable dtpt = ProductionPlanType.GetAll(new ProductionPlanType());
			if (dtpt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtpt, cmbPlanType, "Id", "PlanTypeDescription", "Plan Type ", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ProductionTypeBind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Expected O, but got Unknown
		try
		{
			DataTable dtprot = ProductionType.GetAll(new ProductionType());
			if (dtprot.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtprot, cmbProductionType, "Id", "ProductionTypeDescription", "Production Type ", false);
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
				((UltraGridBase)cmbStatus).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbPlanType_Leave(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		try
		{
			int code = InvProductionJobOrder.GetGenerateCodePlanType(new InvProductionJobOrder
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				PlanType = ((Control)(object)cmbPlanType).Text
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

	private void GridByProductSetting()
	{
		try
		{
			grdByProduct.RootTable.Columns["Id"].Visible = false;
			grdByProduct.RootTable.Columns["ItemId"].Visible = false;
			grdByProduct.RootTable.Columns["EffectiveDate"].Width = 100;
			grdByProduct.RootTable.Columns["ItemName"].Width = 250;
			grdByProduct.RootTable.Columns["Rate40Kg"].Width = 80;
			grdByProduct.RootTable.Columns["EffectiveDate"].FormatString = "dd-MMM-yy";
			grdByProduct.RootTable.Columns["Rate40Kg"].TextAlignment = (TextAlignment)3;
			grdByProduct.RootTable.Columns["Rate40Kg"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdByProduct.RootTable.Columns.Add("Delete");
			grdByProduct.RootTable.Columns["Delete"].Key = "Delete";
			grdByProduct.RootTable.Columns["Delete"].Caption = "X";
			grdByProduct.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdByProduct.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdByProduct.RootTable.Columns["Delete"].ButtonText = "X";
			grdByProduct.RootTable.Columns["Delete"].Width = 20;
			grdByProduct.RootTable.Columns["Delete"].Position = 0;
			grdByProduct.FrozenColumns = 1;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridFinishGoodsSetting()
	{
		try
		{
			grdFinishGoods.RootTable.Columns["Id"].Visible = false;
			grdFinishGoods.RootTable.Columns["ItemId"].Visible = false;
			grdFinishGoods.RootTable.Columns["EffectiveDate"].Width = 100;
			grdFinishGoods.RootTable.Columns["ItemName"].Width = 250;
			grdFinishGoods.RootTable.Columns["Rate40Kg"].Width = 80;
			grdFinishGoods.RootTable.Columns["Rate40Kg"].TextAlignment = (TextAlignment)3;
			grdFinishGoods.RootTable.Columns["Rate40Kg"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdFinishGoods.RootTable.Columns.Add("Delete");
			grdFinishGoods.RootTable.Columns["Delete"].Key = "Delete";
			grdFinishGoods.RootTable.Columns["Delete"].Caption = "X";
			grdFinishGoods.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdFinishGoods.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdFinishGoods.RootTable.Columns["Delete"].ButtonText = "X";
			grdFinishGoods.RootTable.Columns["Delete"].Width = 20;
			grdFinishGoods.RootTable.Columns["Delete"].Position = 0;
			grdFinishGoods.FrozenColumns = 1;
			ctrlGrdBar4_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ScheduleGridSetting()
	{
		try
		{
			grdScheduleInfo.RootTable.Columns["Id"].Visible = false;
			grdScheduleInfo.RootTable.Columns["ContractId"].Visible = false;
			grdScheduleInfo.RootTable.Columns["ScheduleId"].Visible = false;
			grdScheduleInfo.RootTable.Columns["ItemId"].Visible = false;
			grdScheduleInfo.RootTable.Columns["SaleContract"].Width = 130;
			grdScheduleInfo.RootTable.Columns["ScheduleNo"].Width = 130;
			grdScheduleInfo.RootTable.Columns["Item"].Width = 250;
			grdScheduleInfo.RootTable.Columns["MTon"].Width = 90;
			grdScheduleInfo.RootTable.Columns["Remarks"].Width = 300;
			grdScheduleInfo.RootTable.Columns["Id"].EditType = (EditType)0;
			grdScheduleInfo.RootTable.Columns["ContractId"].EditType = (EditType)0;
			grdScheduleInfo.RootTable.Columns["SaleContract"].EditType = (EditType)0;
			grdScheduleInfo.RootTable.Columns["ScheduleId"].EditType = (EditType)0;
			grdScheduleInfo.RootTable.Columns["ScheduleNo"].EditType = (EditType)0;
			grdScheduleInfo.RootTable.Columns["ItemId"].EditType = (EditType)0;
			grdScheduleInfo.RootTable.Columns["Item"].EditType = (EditType)0;
			grdScheduleInfo.RootTable.Columns["MTon"].EditType = (EditType)0;
			grdScheduleInfo.RootTable.Columns["Id"].FilterEditType = (FilterEditType)1;
			grdScheduleInfo.RootTable.Columns["ContractId"].FilterEditType = (FilterEditType)1;
			grdScheduleInfo.RootTable.Columns["SaleContract"].FilterEditType = (FilterEditType)1;
			grdScheduleInfo.RootTable.Columns["ScheduleId"].FilterEditType = (FilterEditType)1;
			grdScheduleInfo.RootTable.Columns["ScheduleNo"].FilterEditType = (FilterEditType)1;
			grdScheduleInfo.RootTable.Columns["ItemId"].FilterEditType = (FilterEditType)1;
			grdScheduleInfo.RootTable.Columns["Item"].FilterEditType = (FilterEditType)1;
			grdScheduleInfo.RootTable.Columns["MTon"].FilterEditType = (FilterEditType)1;
			grdScheduleInfo.RootTable.Columns["MTon"].AggregateFunction = (AggregateFunction)2;
			grdScheduleInfo.RootTable.Columns["MTon"].TextAlignment = (TextAlignment)3;
			grdScheduleInfo.RootTable.Columns["MTon"].FormatString = "#,##0.###";
			grdScheduleInfo.RootTable.Columns["MTon"].TotalFormatString = "#,##0.###";
			if (btnsave.Enabled && btnsave.Visible)
			{
				grdScheduleInfo.RootTable.Columns.Add("Delete");
				grdScheduleInfo.RootTable.Columns["Delete"].Key = "Delete";
				grdScheduleInfo.RootTable.Columns["Delete"].Caption = "X";
				grdScheduleInfo.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grdScheduleInfo.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
				grdScheduleInfo.RootTable.Columns["Delete"].ButtonText = "X";
				grdScheduleInfo.RootTable.Columns["Delete"].Width = 20;
				grdScheduleInfo.RootTable.Columns["Delete"].Position = 0;
				grdScheduleInfo.FrozenColumns = 1;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnplus_Click(object sender, EventArgs e)
	{
		try
		{
			if (!FormValidationDetail())
			{
				return;
			}
			if (GridDetail.GetRows().Count() > 0)
			{
				GridEXRow[] rows = GridDetail.GetRows();
				foreach (GridEXRow item in rows)
				{
					if (Conversion.ToInt(cmbPlantFeader.Value) == Conversion.ToInt(item.Cells["PlantId"].Value))
					{
						throw new Exception("Same Plant Already Exist In Detail");
					}
				}
			}
			dtDetail.Rows.Add(0, cmbPlantFeader.Value, ((Control)(object)cmbPlantFeader).Text.Trim(), txtRemarksDetail.Text.Trim());
			GridDetail.DataSource = dtDetail;
			GridDetail.RetrieveStructure();
			GridDetailSetting();
			((Control)(object)cmbPlantFeader).Text = string.Empty;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private bool FormValidationDetail()
	{
		if (((UltraGridBase)cmbPlantFeader).ActiveRow == null || ((Control)(object)cmbPlantFeader).Text.Trim() == "")
		{
			MessageBox.Show("Plant Feeder Field is Required", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			cmbPlantFeader.Focus();
			return false;
		}
		return true;
	}

	private void GridDetail_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			btnplus.Visible = false;
			btnUpdateDetail.Visible = true;
			btnCancelUpdateDetial.Visible = true;
			GridEXRow item = GridDetail.CurrentRow;
			if (item != null)
			{
				UpdateDetailIndex = item.RowIndex;
				cmbPlantFeader.Value = Conversion.ToInt(item.Cells["PlantId"].Value);
				txtRemarksDetail.Text = Conversion.ToString(item.Cells["RemarksDetail"].Value);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
			if (GridDetail.GetRows().Count() > 0)
			{
				GridEXRow[] rows = GridDetail.GetRows();
				foreach (GridEXRow item in rows)
				{
					if (UpdateDetailIndex != item.RowIndex && Conversion.ToInt(cmbPlantFeader.Value) == Conversion.ToInt(item.Cells["PlantId"].Value))
					{
						throw new Exception("Same Plant Already Exist In Detail");
					}
				}
			}
			dtDetail.Rows[UpdateDetailIndex]["PlantId"] = cmbPlantFeader.Value;
			dtDetail.Rows[UpdateDetailIndex]["PlantName"] = ((Control)(object)cmbPlantFeader).Text.Trim();
			dtDetail.Rows[UpdateDetailIndex]["RemarksDetail"] = txtRemarksDetail.Text.Trim();
			btnplus.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			((Control)(object)cmbPlantFeader).Text = string.Empty;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnCancelUpdateDetial_Click(object sender, EventArgs e)
	{
		try
		{
			btnplus.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			((Control)(object)cmbPlantFeader).Text = string.Empty;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridDetail_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Expected O, but got Unknown
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!(e.Column.Key == "Delete") || MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			GridEXRow item = GridDetail.CurrentRow;
			if (btnsave.Visible && btnsave.Enabled)
			{
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
				return;
			}
			if (!InvProductionJobOrder.CheckPlantExistinProductionagaintJobOrder(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				JobOrderId = RECID,
				PlantId = Conversion.ToInt(item.Cells["PlantId"].Value)
			}))
			{
				dtDetail.Rows[item.RowIndex].Delete();
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
				GridDetail.UpdateData();
				return;
			}
			throw new Exception("You cannot be delete " + Conversion.ToString(item.Cells["PlantName"].Value) + " because " + Conversion.ToString(item.Cells["PlantName"].Value) + " Has been refered to production");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridDetailSetting()
	{
		try
		{
			GridDetail.RootTable.Columns["Id"].Visible = false;
			GridDetail.RootTable.Columns["PlantId"].Visible = false;
			GridDetail.RootTable.Columns["PlantName"].Width = 110;
			GridDetail.RootTable.Columns["RemarksDetail"].Width = 250;
			GridDetail.RootTable.Columns.Add("Delete");
			GridDetail.RootTable.Columns["Delete"].Key = "Delete";
			GridDetail.RootTable.Columns["Delete"].Caption = "X";
			GridDetail.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			GridDetail.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			GridDetail.RootTable.Columns["Delete"].ButtonText = "X";
			GridDetail.RootTable.Columns["Delete"].Width = 20;
			GridDetail.RootTable.Columns["Delete"].Position = 0;
			GridDetail.FrozenColumns = 1;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private bool FormValidationByProduct()
	{
		if (Conversion.ToInt(CmbItemByProduct.Value) == 0)
		{
			MessageBox.Show("Item Name Field is Required", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			CmbItemByProduct.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtRateByProduct.Text) == 0.0)
		{
			MessageBox.Show("Rate40Kg Field is Required", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			txtRateByProduct.Focus();
			return false;
		}
		return true;
	}

	private void btnAddUpdateByProduct_Click(object sender, EventArgs e)
	{
		try
		{
			if (!FormValidationByProduct())
			{
				return;
			}
			if (btnAddUpdateByProduct.Text == "Add")
			{
				if (grdByProduct.RowCount > 0)
				{
					GridEXRow[] rows = grdByProduct.GetRows();
					foreach (GridEXRow obj in rows)
					{
						DateTime EffectiveDate = Conversion.ToDateTime(obj.Cells["EffectiveDate"].Value);
						int ItemId = Conversion.ToInt(obj.Cells["ItemId"].Value);
						if (EffectiveDate.Date == datEffectiveDateByProduct.Value.Date && Conversion.ToInt(CmbItemByProduct.Value) == ItemId)
						{
							MessageBox.Show("In Same Effective Date Same Item Already Exist in record. Please Check!", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							return;
						}
					}
				}
				dtByProduct.Rows.Add(0, datEffectiveDateByProduct.Value, CmbItemByProduct.Value, ((Control)(object)CmbItemByProduct).Text, Conversion.ToDouble((object)txtRateByProduct.Text));
				grdByProduct.DataSource = dtByProduct;
				grdByProduct.RetrieveStructure();
				GridByProductSetting();
			}
			else if (btnAddUpdateByProduct.Text == "Update")
			{
				if (grdByProduct.RowCount > 0)
				{
					GridEXRow[] rows = grdByProduct.GetRows();
					foreach (GridEXRow item in rows)
					{
						if (UpdateDetailIndexForByProduct != item.RowIndex)
						{
							DateTime EffectiveDate2 = Conversion.ToDateTime(item.Cells["EffectiveDate"].Value);
							int ItemId2 = Conversion.ToInt(item.Cells["ItemId"].Value);
							if (EffectiveDate2.Date == datEffectiveDateByProduct.Value.Date && Conversion.ToInt(CmbItemByProduct.Value) == ItemId2)
							{
								MessageBox.Show("In Same Effective Date Same Item Already Exist in record. Please Check!", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
								return;
							}
						}
					}
				}
				dtByProduct.Rows[UpdateDetailIndexForByProduct]["EffectiveDate"] = datEffectiveDateByProduct.Value;
				dtByProduct.Rows[UpdateDetailIndexForByProduct]["ItemId"] = CmbItemByProduct.Value;
				dtByProduct.Rows[UpdateDetailIndexForByProduct]["ItemName"] = ((Control)(object)CmbItemByProduct).Text;
				dtByProduct.Rows[UpdateDetailIndexForByProduct]["Rate40Kg"] = txtRateByProduct.Text;
				btnAddUpdateByProduct.Text = "Add";
				btnCancelByProduct.Visible = false;
			}
			((Control)(object)CmbItemByProduct).Text = string.Empty;
			txtRateByProduct.Text = string.Empty;
			datEffectiveDateByProduct.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelByProduct_Click(object sender, EventArgs e)
	{
		try
		{
			btnAddUpdateByProduct.Text = "Add";
			btnCancelByProduct.Visible = false;
			((Control)(object)CmbItemByProduct).Text = string.Empty;
			txtRateByProduct.Text = string.Empty;
			datEffectiveDateByProduct.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdByProduct_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grdByProduct.CurrentRow;
			if (item != null)
			{
				UpdateDetailIndexForByProduct = item.RowIndex;
				datEffectiveDateByProduct.Value = Conversion.ToDateTime(item.Cells["EffectiveDate"].Value);
				CmbItemByProduct.Value = Conversion.ToInt(item.Cells["ItemId"].Value);
				txtRateByProduct.Text = Conversion.ToDouble(item.Cells["Rate40Kg"].Value).ToString(clsGlobalVariables.DecimalRateFormate);
			}
			btnAddUpdateByProduct.Text = "Update";
			btnCancelByProduct.Visible = true;
			datEffectiveDateByProduct.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdByProduct_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!(e.Column.Key == "Delete") || MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			GridEXRow item = grdByProduct.CurrentRow;
			if (btnsave.Visible && btnsave.Enabled)
			{
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
				return;
			}
			InvProductionJobOrder.JoBOrderDetailScheduleDeleteById(Conversion.ToInt(item.Cells["Id"].Value));
			dtByProduct.Rows[item.RowIndex].Delete();
			if (item != null && (int)item.RowType == 0)
			{
				item.Delete();
			}
			grdByProduct.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdByProduct_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			GridEXRow item = grdByProduct.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				grdByProduct_DoubleClick(null, null);
			}
			else
			{
				if (!e.Control || e.KeyCode != Keys.Space || !(grdByProduct.CurrentColumn.Key == "Delete") || MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				if (btnsave.Visible && btnsave.Enabled)
				{
					if (item != null && (int)item.RowType == 0)
					{
						item.Delete();
					}
					return;
				}
				InvProductionJobOrder.JoBOrderDetailScheduleDeleteById(Conversion.ToInt(item.Cells["Id"].Value));
				dtByProduct.Rows[item.RowIndex].Delete();
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
				grdByProduct.UpdateData();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidationFinishGoods()
	{
		if (Conversion.ToInt(CmbItemFinish.Value) == 0)
		{
			MessageBox.Show("Item Name Field is Required", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			CmbItemFinish.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtRateFinish.Text) == 0.0)
		{
			MessageBox.Show("Rate40Kg Field is Required", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			txtRateFinish.Focus();
			return false;
		}
		return true;
	}

	private void btnAddUpdateFinish_Click(object sender, EventArgs e)
	{
		try
		{
			if (!FormValidationFinishGoods())
			{
				return;
			}
			if (btnAddUpdateFinish.Text == "Add")
			{
				if (grdFinishGoods.RowCount > 0)
				{
					GridEXRow[] rows = grdFinishGoods.GetRows();
					foreach (GridEXRow obj in rows)
					{
						DateTime EffectiveDate = Conversion.ToDateTime(obj.Cells["EffectiveDate"].Value);
						int ItemId = Conversion.ToInt(obj.Cells["ItemId"].Value);
						if (EffectiveDate.Date == datEffectiveDateFinish.Value.Date && Conversion.ToInt(CmbItemFinish.Value) == ItemId)
						{
							MessageBox.Show("In Same Effective Date Same Item Already Exist in record. Please Check!", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							return;
						}
					}
				}
				dtFinish.Rows.Add(0, datEffectiveDateFinish.Value, CmbItemFinish.Value, ((Control)(object)CmbItemFinish).Text, Conversion.ToDouble((object)txtRateFinish.Text));
				grdFinishGoods.DataSource = dtFinish;
				grdFinishGoods.RetrieveStructure();
				GridFinishGoodsSetting();
			}
			else if (btnAddUpdateFinish.Text == "Update")
			{
				if (grdFinishGoods.RowCount > 0)
				{
					GridEXRow[] rows = grdFinishGoods.GetRows();
					foreach (GridEXRow item in rows)
					{
						if (UpdateDetailIndexForFinishGoods != item.RowIndex)
						{
							DateTime EffectiveDate2 = Conversion.ToDateTime(item.Cells["EffectiveDate"].Value);
							int ItemId2 = Conversion.ToInt(item.Cells["ItemId"].Value);
							if (EffectiveDate2.Date == datEffectiveDateFinish.Value.Date && Conversion.ToInt(CmbItemFinish.Value) == ItemId2)
							{
								MessageBox.Show("In Same Effective Date Same Item Already Exist in record. Please Check!", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
								return;
							}
						}
					}
				}
				dtFinish.Rows[UpdateDetailIndexForFinishGoods]["EffectiveDate"] = datEffectiveDateFinish.Value;
				dtFinish.Rows[UpdateDetailIndexForFinishGoods]["ItemId"] = CmbItemFinish.Value;
				dtFinish.Rows[UpdateDetailIndexForFinishGoods]["ItemName"] = ((Control)(object)CmbItemFinish).Text;
				dtFinish.Rows[UpdateDetailIndexForFinishGoods]["Rate40Kg"] = txtRateFinish.Text;
				btnAddUpdateFinish.Text = "Add";
				btnCancelFinish.Visible = false;
			}
			((Control)(object)CmbItemFinish).Text = string.Empty;
			txtRateFinish.Text = string.Empty;
			datEffectiveDateFinish.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelFinish_Click(object sender, EventArgs e)
	{
		try
		{
			btnAddUpdateFinish.Text = "Add";
			btnCancelFinish.Visible = false;
			((Control)(object)CmbItemFinish).Text = string.Empty;
			txtRateFinish.Text = string.Empty;
			datEffectiveDateFinish.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdFinishGoods_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grdFinishGoods.CurrentRow;
			if (item != null)
			{
				UpdateDetailIndexForFinishGoods = item.RowIndex;
				datEffectiveDateFinish.Value = Conversion.ToDateTime(item.Cells["EffectiveDate"].Value);
				CmbItemFinish.Value = Conversion.ToInt(item.Cells["ItemId"].Value);
				txtRateFinish.Text = Conversion.ToDouble(item.Cells["Rate40Kg"].Value).ToString(clsGlobalVariables.DecimalRateFormate);
			}
			btnAddUpdateFinish.Text = "Update";
			btnCancelFinish.Visible = true;
			datEffectiveDateFinish.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdFinishGoods_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!(e.Column.Key == "Delete") || MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			GridEXRow item = grdFinishGoods.CurrentRow;
			if (btnsave.Visible && btnsave.Enabled)
			{
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
				return;
			}
			InvProductionJobOrder.JoBOrderDetailScheduleDeleteById(Conversion.ToInt(item.Cells["Id"].Value));
			dtFinish.Rows[item.RowIndex].Delete();
			if (item != null && (int)item.RowType == 0)
			{
				item.Delete();
			}
			grdFinishGoods.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdFinishGoods_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			GridEXRow item = grdFinishGoods.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				grdFinishGoods_DoubleClick(null, null);
			}
			else
			{
				if (!e.Control || e.KeyCode != Keys.Space || !(grdFinishGoods.CurrentColumn.Key == "Delete") || MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				if (btnsave.Visible && btnsave.Enabled)
				{
					if (item != null && (int)item.RowType == 0)
					{
						item.Delete();
					}
					return;
				}
				InvProductionJobOrder.JoBOrderDetailScheduleDeleteById(Conversion.ToInt(item.Cells["Id"].Value));
				dtFinish.Rows[item.RowIndex].Delete();
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
				grdFinishGoods.UpdateData();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			txtPlanDate.Value = DateTime.Now;
			txtProductionNo.Text = string.Empty;
			((Control)(object)CmbProductionNo).Text = string.Empty;
			txtOtherInstruction.Text = string.Empty;
			txtOtherReference.Text = string.Empty;
			((Control)(object)CmbWorkInProcess).Text = string.Empty;
			((Control)(object)CmbWipItem).Text = string.Empty;
			((Control)(object)CmbWipWarehouse).Text = string.Empty;
			((Control)(object)CmbFinishGoodsAc).Text = string.Empty;
			((Control)(object)CmbByProductionAc).Text = string.Empty;
			((Control)(object)cmbProductionType).Text = string.Empty;
			cmbStatus.Value = string.Empty;
			cmbPlanType.Value = string.Empty;
			btnUpdate.Visible = false;
			btnsave.Visible = true;
			UpdateMode = false;
			GeneratePlanCode();
			txtPlanDate.Focus();
			GetLastSaveAccount();
			if (GetLastWipAccountId > 0)
			{
				CmbWorkInProcess.Value = GetLastWipAccountId;
			}
			if (GetLastWipItemId > 0)
			{
				CmbWipItem.Value = GetLastWipItemId;
			}
			if (GetLastWipWareHouseId > 0)
			{
				CmbWipWarehouse.Value = GetLastWipWareHouseId;
			}
			dtDetail.Rows.Clear();
			GridDetail.ClearStructure();
			((Control)(object)cmbPlantFeader).Text = string.Empty;
			btnplus.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			dtByProduct.Rows.Clear();
			grdByProduct.DataSource = dtByProduct;
			grdByProduct.RetrieveStructure();
			GridByProductSetting();
			dtFinish.Rows.Clear();
			grdFinishGoods.DataSource = dtFinish;
			grdFinishGoods.RetrieveStructure();
			GridFinishGoodsSetting();
			dtExportSchedule.Rows.Clear();
			grdScheduleInfo.DataSource = dtExportSchedule;
			grdScheduleInfo.RetrieveStructure();
			ScheduleGridSetting();
			UpdateDetailIndexForByProduct = -1;
			UpdateDetailIndexForFinishGoods = -1;
			btnAddUpdateByProduct.Text = "Add";
			btnAddUpdateByProduct.Visible = true;
			btnCancelByProduct.Visible = false;
			btnAddUpdateFinish.Text = "Add";
			btnAddUpdateFinish.Visible = true;
			btnCancelFinish.Visible = false;
			if (GenerateJobOrderNo)
			{
				BindJobOrderNo();
			}
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

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			ByProductRateEditAbleIsAllow = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("ByProductRateEditableIsAllow"));
			GenerateJobOrderNo = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("GenerateJobOrderNo"));
			btnGenerateProductionNo.Visible = GenerateJobOrderNo;
			((Control)(object)CmbProductionNo).Visible = GenerateJobOrderNo;
			txtProductionNo.Visible = !GenerateJobOrderNo;
			if (GenerateJobOrderNo)
			{
				BindJobOrderNo();
			}
			ItemFill();
			AccountFills();
			PlanTypeBind();
			ProductionTypeBind();
			GeneratePlanCode();
			PlantFeader();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmProductionJobOrderMain_FormClosing(object sender, FormClosingEventArgs e)
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

	private bool FormValidation()
	{
		if (txtPlanCode.Text.Trim() == string.Empty || txtPlanCode.Text.Trim() == "0")
		{
			MessageBox.Show("Plan Code Field Required");
			txtPlanCode.Focus();
			return false;
		}
		if (GenerateJobOrderNo)
		{
			if (((Control)(object)CmbProductionNo).Text == string.Empty || ((Control)(object)CmbProductionNo).Text == "0")
			{
				MessageBox.Show("Mannual ReportNo Field Required");
				CmbProductionNo.Focus();
				return false;
			}
		}
		else if (txtProductionNo.Text == string.Empty || txtProductionNo.Text == "0")
		{
			MessageBox.Show("Mannual ReportNo Field Required");
			txtProductionNo.Focus();
			return false;
		}
		if (((UltraGridBase)cmbProductionType).ActiveRow == null)
		{
			MessageBox.Show("Production Type Field Required");
			cmbProductionType.Focus();
			return false;
		}
		if (((UltraGridBase)cmbPlanType).ActiveRow == null)
		{
			MessageBox.Show("Plan Type Field Required");
			cmbPlanType.Focus();
			return false;
		}
		if (((UltraGridBase)cmbStatus).ActiveRow == null)
		{
			MessageBox.Show("Status Field Required");
			cmbStatus.Focus();
			return false;
		}
		if (((UltraGridBase)CmbWorkInProcess).ActiveRow == null)
		{
			MessageBox.Show("workinprocess field required");
			CmbWorkInProcess.Focus();
			return false;
		}
		if (((UltraGridBase)CmbWipItem).ActiveRow == null)
		{
			MessageBox.Show("WIPItem Field Required");
			CmbWipItem.Focus();
			return false;
		}
		if (((UltraGridBase)CmbWipWarehouse).ActiveRow == null || Conversion.ToInt(CmbWipWarehouse.Value) == 0)
		{
			MessageBox.Show("WIP Warehouse Field Required");
			CmbWipWarehouse.Focus();
			return false;
		}
		return true;
	}

	private void Insert()
	{
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Expected O, but got Unknown
		//IL_046e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0475: Expected O, but got Unknown
		//IL_0547: Unknown result type (might be due to invalid IL or missing references)
		//IL_054e: Expected O, but got Unknown
		//IL_0642: Unknown result type (might be due to invalid IL or missing references)
		//IL_0649: Expected O, but got Unknown
		//IL_0713: Unknown result type (might be due to invalid IL or missing references)
		//IL_071a: Expected O, but got Unknown
		try
		{
			if (!FormValidation())
			{
				return;
			}
			DateTime DocDate = Conversion.ToDateTime((object)txtPlanDate.Value);
			if (DocDate.Date > Conversion.ToDateTime((object)txtstartDate.Value).Date)
			{
				MessageBox.Show("Plan Date Cannot Be Greater Than StartDate", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				txtPlanDate.Focus();
				return;
			}
			if (Conversion.ToDateTime((object)txtstartDate.Value).Date > Conversion.ToDateTime((object)txtEndDate.Value).Date)
			{
				MessageBox.Show("Start Date Cannot Be Greater Than End Date", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				txtstartDate.Focus();
				return;
			}
			if (grdByProduct.GetDataRows().Any((GridEXRow r) => Conversion.ToDateTime(r.Cells["EffectiveDate"].Value).Date < DocDate.Date))
			{
				MessageBox.Show("By Product Effective Date Cannot Be Less Than Plan Date", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				datEffectiveDateByProduct.Focus();
				return;
			}
			if (grdFinishGoods.GetDataRows().Any((GridEXRow r) => Conversion.ToDateTime(r.Cells["EffectiveDate"].Value).Date < DocDate.Date))
			{
				MessageBox.Show("Finish Goods Effective Date Cannot Be Less Than Plan Date", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				datEffectiveDateFinish.Focus();
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
			if (GridDetail.GetRows().Count() == 0)
			{
				throw new Exception("At Least One Detail Record Is Required");
			}
			invProductionJobOrder.DocumentTypeId = 403;
			invProductionJobOrder.PlanDate = txtPlanDate.Value;
			invProductionJobOrder.PlanCode = Conversion.ToInt((object)txtPlanCode.Text);
			invProductionJobOrder.InvProductionPlantId = Conversion.ToInt(cmbPlantFeader.Value);
			invProductionJobOrder.PlanType = Conversion.ToString((object)((Control)(object)cmbPlanType).Text);
			invProductionJobOrder.PlanTypeSrNo = Conversion.ToInt((object)txtPlanTypeSrNo.Text);
			if (GenerateJobOrderNo)
			{
				invProductionJobOrder.RefInvoiceNo = Conversion.ToString((object)((Control)(object)CmbProductionNo).Text);
			}
			else
			{
				invProductionJobOrder.RefInvoiceNo = Conversion.ToString((object)txtProductionNo.Text);
			}
			invProductionJobOrder.LotReference = Conversion.ToString((object)txtOtherReference.Text);
			invProductionJobOrder.ProductionType = Conversion.ToString((object)((Control)(object)cmbProductionType).Text);
			invProductionJobOrder.WorkInProccessAcId = Conversion.ToInt(CmbWorkInProcess.Value);
			invProductionJobOrder.FinishGoodsAcId = Conversion.ToInt(CmbFinishGoodsAc.Value);
			invProductionJobOrder.ByProductacId = Conversion.ToInt(CmbByProductionAc.Value);
			invProductionJobOrder.WipItemId = Conversion.ToInt(CmbWipItem.Value);
			invProductionJobOrder.WipWareHouseId = Conversion.ToInt(CmbWipWarehouse.Value);
			invProductionJobOrder.StartDate = txtstartDate.Value;
			invProductionJobOrder.EndDate = txtEndDate.Value;
			invProductionJobOrder.PlanStatus = Conversion.ToString((object)((Control)(object)cmbStatus).Text);
			invProductionJobOrder.FinishGoodRate = Conversion.ToDouble((object)txtAvgFinishGoodRate.Text.Trim());
			invProductionJobOrder.OtherInstructions = txtOtherInstruction.Text;
			invProductionJobOrder.IsApproved = false;
			invProductionJobOrder.SupplierCustomerId = 0;
			invProductionJobOrder.EntryDate = DateTime.Now;
			invProductionJobOrder.EntryUser = UserAccount.ID;
			invProductionJobOrder.ModifyUser = UserAccount.ID;
			invProductionJobOrder.ModifyDate = DateTime.Now;
			invProductionJobOrder.OrganizationId = UserAccount.OrganizationId;
			invProductionJobOrder.CompanyId = UserAccount.CompanyId;
			invProductionJobOrder.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			invProductionJobOrder.BranchesId = UserAccount.BranchesId;
			invProductionJobOrder.invProductionJobOrderInput = new List<InvProductionJobOrderInput>();
			invProductionJobOrder.invProductionJobOrderOutput = new List<InvProductionJobOrderOutput>();
			invProductionJobOrder.InvProductionJobOrderPlantslist = new List<InvProductionJobOrderPlant>();
			invProductionJobOrder.InvProductionJobOrderPackingMaterialList = new List<InvProductionJobOrderPackingMaterial>();
			invProductionJobOrder.InvProductionJobOrderOverHeadsList = new List<InvProductionJobOrderOverHeads>();
			invProductionJobOrder.JobOrderRateSchedulelist = new List<JobOrderRateSchedule>();
			invProductionJobOrder.InvProductionJobOrderAndOrderAllocationList = new List<InvProductionJobOrderAndOrderAllocation>();
			GridEXRow[] rows = GridDetail.GetRows();
			foreach (GridEXRow item4 in rows)
			{
				InvProductionJobOrderPlant plant = new InvProductionJobOrderPlant();
				plant.Id = Conversion.ToInt(item4.Cells["Id"].Value);
				plant.PlantId = Conversion.ToInt(item4.Cells["PlantId"].Value);
				plant.RemarksDetail = Conversion.ToString(item4.Cells["RemarksDetail"].Value);
				invProductionJobOrder.InvProductionJobOrderPlantslist.Add(plant);
			}
			if (!ByProductRateEditAbleIsAllow && grdByProduct.GetRows().Count() == 0)
			{
				MessageBox.Show("At Least One Record Is Required in By Product Rate Schedule", "Warning Message!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			rows = grdByProduct.GetRows();
			foreach (GridEXRow item3 in rows)
			{
				JobOrderRateSchedule Schedule3 = new JobOrderRateSchedule();
				Schedule3.TransTypeId = 1;
				Schedule3.Id = Conversion.ToInt(item3.Cells["Id"].Value);
				Schedule3.EffectiveDate = Conversion.ToDateTime(item3.Cells["EffectiveDate"].Value);
				Schedule3.ItemId = Conversion.ToInt(item3.Cells["ItemId"].Value);
				Schedule3.ItemRate = Conversion.ToDouble(item3.Cells["Rate40Kg"].Value);
				invProductionJobOrder.JobOrderRateSchedulelist.Add(Schedule3);
			}
			if (grdFinishGoods.GetRows().Count() == 0)
			{
				MessageBox.Show("At Least One Record Is Required in By Finish Goods Rate Schedule", "Warning Message!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			rows = grdFinishGoods.GetRows();
			foreach (GridEXRow item2 in rows)
			{
				JobOrderRateSchedule Schedule2 = new JobOrderRateSchedule();
				Schedule2.TransTypeId = 2;
				Schedule2.Id = Conversion.ToInt(item2.Cells["Id"].Value);
				Schedule2.EffectiveDate = Conversion.ToDateTime(item2.Cells["EffectiveDate"].Value);
				Schedule2.ItemId = Conversion.ToInt(item2.Cells["ItemId"].Value);
				Schedule2.ItemRate = Conversion.ToDouble(item2.Cells["Rate40Kg"].Value);
				invProductionJobOrder.JobOrderRateSchedulelist.Add(Schedule2);
			}
			rows = grdScheduleInfo.GetRows();
			foreach (GridEXRow item in rows)
			{
				InvProductionJobOrderAndOrderAllocation Schedule = new InvProductionJobOrderAndOrderAllocation();
				Schedule.OrderTypeId = 1;
				Schedule.Id = Conversion.ToInt(item.Cells["Id"].Value);
				Schedule.OrderId = Conversion.ToInt(item.Cells["ContractId"].Value);
				Schedule.ScheduleId = Conversion.ToInt(item.Cells["ScheduleId"].Value);
				Schedule.ItemId = Conversion.ToInt(item.Cells["ItemId"].Value);
				Schedule.MTon = Conversion.ToDouble(item.Cells["MTon"].Value);
				Schedule.Remarks = Conversion.ToString(item.Cells["Remarks"].Value);
				invProductionJobOrder.InvProductionJobOrderAndOrderAllocationList.Add(Schedule);
			}
			invProductionJobOrder.AttachmentsList = new List<DMSAttachments>();
			invProductionJobOrder.DeleteAttachmentsList = new List<DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<InvProductionJobOrder>(invProductionJobOrder, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
			int success = InvProductionJobOrder.Save(invProductionJobOrder);
			if (RECID == 0)
			{
				MessageBox.Show("Save Successfully" + invProductionJobOrder.PlanCode);
			}
			else
			{
				MessageBox.Show("Update Successfully" + invProductionJobOrder.PlanCode);
				FormHelper.DeleteAllAttachmentsFromDb(AT, RECID, invProductionJobOrder.DocumentTypeId, base.Name);
				UpdateMode = false;
			}
			Reset();
			dtDetail.Rows.Clear();
			GridDetail.ClearStructure();
			if (chkPrint.Checked)
			{
				CommonServices.PrintJobOrder620(success);
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

	private void btnSaveAs_Click(object sender, EventArgs e)
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

	public void ReadById(int ID)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			InvProductionJobOrder head = new InvProductionJobOrder();
			RECID = ID;
			head = InvProductionJobOrder.GetByID(RECID);
			if (head == null)
			{
				return;
			}
			tabControl1.SelectedIndex = 0;
			txtPlanDate.Value = head.PlanDate;
			txtPlanCode.Text = Conversion.ToString((object)head.PlanCode);
			CmbWorkInProcess.Value = Conversion.ToInt((object)head.WorkInProccessAcId);
			CmbFinishGoodsAc.Value = Conversion.ToInt((object)head.FinishGoodsAcId);
			CmbByProductionAc.Value = Conversion.ToInt((object)head.ByProductacId);
			CmbWipItem.Value = Conversion.ToInt((object)head.WipItemId);
			CmbWipWarehouse.Value = Conversion.ToInt((object)head.WipWareHouseId);
			((Control)(object)cmbPlanType).Text = Conversion.ToString((object)head.PlanType);
			txtPlanTypeSrNo.Text = Conversion.ToString((object)head.PlanTypeSrNo);
			if (GenerateJobOrderNo)
			{
				BindJobOrderNo(Conversion.ToString((object)head.RefInvoiceNo));
				((Control)(object)CmbProductionNo).Text = Conversion.ToString((object)head.RefInvoiceNo);
			}
			else
			{
				txtProductionNo.Text = Conversion.ToString((object)head.RefInvoiceNo);
			}
			txtOtherReference.Text = Conversion.ToString((object)head.LotReference);
			((Control)(object)cmbProductionType).Text = Conversion.ToString((object)head.ProductionType);
			txtstartDate.Value = Conversion.ToDateTime((object)head.StartDate);
			txtEndDate.Value = Conversion.ToDateTime((object)head.EndDate);
			((Control)(object)cmbStatus).Text = Conversion.ToString((object)head.PlanStatus);
			txtAvgFinishGoodRate.Text = Conversion.ToString((object)head.FinishGoodRate);
			txtOtherInstruction.Text = head.OtherInstructions;
			dtDetail.Rows.Clear();
			GridDetail.ClearStructure();
			for (int k = 0; k < head.InvProductionJobOrderPlantslist.Count; k++)
			{
				dtDetail.Rows.Add(head.InvProductionJobOrderPlantslist[k].Id, head.InvProductionJobOrderPlantslist[k].PlantId, head.InvProductionJobOrderPlantslist[k].PlantName, head.InvProductionJobOrderPlantslist[k].RemarksDetail);
			}
			GridDetail.DataSource = dtDetail;
			GridDetail.RetrieveStructure();
			GridDetailSetting();
			dtByProduct.Rows.Clear();
			dtFinish.Rows.Clear();
			for (int j = 0; j < head.JobOrderRateSchedulelist.Count; j++)
			{
				if (head.JobOrderRateSchedulelist[j].TransTypeId == 1)
				{
					dtByProduct.Rows.Add(head.JobOrderRateSchedulelist[j].Id, head.JobOrderRateSchedulelist[j].EffectiveDate, head.JobOrderRateSchedulelist[j].ItemId, head.JobOrderRateSchedulelist[j].ItemName, head.JobOrderRateSchedulelist[j].ItemRate);
				}
				else if (head.JobOrderRateSchedulelist[j].TransTypeId == 2)
				{
					dtFinish.Rows.Add(head.JobOrderRateSchedulelist[j].Id, head.JobOrderRateSchedulelist[j].EffectiveDate, head.JobOrderRateSchedulelist[j].ItemId, head.JobOrderRateSchedulelist[j].ItemName, head.JobOrderRateSchedulelist[j].ItemRate);
				}
			}
			grdByProduct.DataSource = dtByProduct;
			grdByProduct.RetrieveStructure();
			GridByProductSetting();
			grdFinishGoods.DataSource = dtFinish;
			grdFinishGoods.RetrieveStructure();
			GridFinishGoodsSetting();
			dtExportSchedule.Rows.Clear();
			if (head.InvProductionJobOrderAndOrderAllocationList != null && head.InvProductionJobOrderAndOrderAllocationList.Count > 0)
			{
				for (int i = 0; i < head.InvProductionJobOrderAndOrderAllocationList.Count; i++)
				{
					dtExportSchedule.Rows.Add(head.InvProductionJobOrderAndOrderAllocationList[i].Id, head.InvProductionJobOrderAndOrderAllocationList[i].OrderId, head.InvProductionJobOrderAndOrderAllocationList[i].ContractNo, head.InvProductionJobOrderAndOrderAllocationList[i].ScheduleId, head.InvProductionJobOrderAndOrderAllocationList[i].ScheduleNo, head.InvProductionJobOrderAndOrderAllocationList[i].ItemId, head.InvProductionJobOrderAndOrderAllocationList[i].ItemName, head.InvProductionJobOrderAndOrderAllocationList[i].MTon, head.InvProductionJobOrderAndOrderAllocationList[i].Remarks);
				}
			}
			grdScheduleInfo.DataSource = dtExportSchedule;
			grdScheduleInfo.RetrieveStructure();
			ScheduleGridSetting();
			FormHelper.LoadAttachmentsForObject(AT, RECID, base.Name);
			UpdateMode = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnNewHistory_Click(object sender, EventArgs e)
	{
		try
		{
			ParameterFill();
			txtOrderNoFrom.Text = string.Empty;
			txtOrderNoTo.Text = string.Empty;
			((Control)(object)xmbjoborderno).Text = string.Empty;
			cmbperemeter.Focus();
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
			btnsave.Visible = false;
			btnUpdate.Visible = true;
			ReadById(Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells["Id"].Value.ToString()));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindHistoryGrid()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters joborder = new ReportsParameters();
			joborder.OrganizationId = UserAccount.OrganizationId;
			joborder.CompanyId = UserAccount.CompanyId;
			joborder.DocumentTypeId = 403;
			joborder.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			joborder.EntryUser = UserAccount.ID;
			joborder.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			if (drdocdate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					joborder.FromDate = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					joborder.ToDate = ToDateHistory.Value;
				}
			}
			else if (rdentrydate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					joborder.EntryDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					joborder.EntryDateTo = ToDateHistory.Value;
				}
			}
			else if (rdmodifydate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					joborder.ModifyDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					joborder.ModifyDateTo = ToDateHistory.Value;
				}
			}
			else if (rdapproveddate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					joborder.ApprovedDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					joborder.ApprovedDateTo = ToDateHistory.Value;
				}
			}
			joborder.FromDocNo = Conversion.ToInt((object)txtOrderNoFrom.Text);
			joborder.ToDocNo = Conversion.ToInt((object)txtOrderNoTo.Text);
			joborder.InvJobOrderId = Conversion.ToInt(xmbjoborderno.Value);
			DataTable dtGetAll = InvProductionJobOrder.FormHistoryNew(joborder);
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("Date", typeof(DateTime));
			dt.Columns.Add("Code", typeof(int));
			dt.Columns.Add("Type");
			dt.Columns.Add("Production#");
			dt.Columns.Add("PType");
			dt.Columns.Add("StartDate", typeof(DateTime));
			dt.Columns.Add("EndDate", typeof(DateTime));
			dt.Columns.Add("PlanStatus");
			dt.Columns.Add("OtherInst");
			dt.Columns.Add("EntryUser");
			dt.Columns.Add("EntryDate", typeof(DateTime));
			dt.Columns.Add("ModifyUser");
			dt.Columns.Add("ModifyDate", typeof(DateTime));
			dt.Columns.Add("ApprovedStatus");
			dt.Columns.Add("ApprovedUser");
			dt.Columns.Add("ApprovedDate", typeof(DateTime));
			dt.Columns.Add("Attachments");
			for (int i = 0; i < dtGetAll.Rows.Count; i++)
			{
				dt.Rows.Add(dtGetAll.Rows[i]["Id"], dtGetAll.Rows[i]["PlanDate"], dtGetAll.Rows[i]["PlanCode"], dtGetAll.Rows[i]["PlanType"], dtGetAll.Rows[i]["RefInvoiceNo"], dtGetAll.Rows[i]["ProductionType"], dtGetAll.Rows[i]["StartDate"], dtGetAll.Rows[i]["EndDate"], dtGetAll.Rows[i]["PlanStatus"], dtGetAll.Rows[i]["OtherInstructions"], dtGetAll.Rows[i]["EntryUserName"], dtGetAll.Rows[i]["EntryDate"], dtGetAll.Rows[i]["ModifyUserName"], dtGetAll.Rows[i]["ModifyDate"], dtGetAll.Rows[i]["ApprovalStatus"], dtGetAll.Rows[i]["ApprovedUserName"], dtGetAll.Rows[i]["PostDate"], dtGetAll.Rows[i]["NoOfAttachments"]);
			}
			DataGridHistory.DataSource = dt;
			DataGridHistory.RetrieveStructure();
			HgridSetting();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void HgridSetting()
	{
		try
		{
			DataGridHistory.RootTable.Columns["Id"].Visible = false;
			DataGridHistory.RootTable.Columns["Date"].Width = 100;
			DataGridHistory.RootTable.Columns["Code"].Width = 50;
			DataGridHistory.RootTable.Columns["Type"].Width = 70;
			DataGridHistory.RootTable.Columns["Production#"].Width = 60;
			DataGridHistory.RootTable.Columns["PType"].Width = 80;
			DataGridHistory.RootTable.Columns["StartDate"].Width = 100;
			DataGridHistory.RootTable.Columns["EndDate"].Width = 100;
			DataGridHistory.RootTable.Columns["PlanStatus"].Width = 80;
			DataGridHistory.RootTable.Columns["OtherInst"].Width = 150;
			DataGridHistory.RootTable.Columns["Attachments"].ColumnType = (ColumnType)5;
			DataGridHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			DataGridHistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			DataGridHistory.RootTable.Columns["ApprovedDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			CommonServices.GridAutoAdjustmentNew(DataGridHistory);
			DataGridHistory.RootTable.Columns.Add("Edit");
			DataGridHistory.RootTable.Columns["Edit"].Key = "Edit";
			DataGridHistory.RootTable.Columns["Edit"].Caption = "Edit";
			DataGridHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			DataGridHistory.RootTable.Columns["Edit"].Width = 40;
			DataGridHistory.RootTable.Columns["Edit"].Position = 0;
			DataGridHistory.RootTable.Columns.Add("SaveAs");
			DataGridHistory.RootTable.Columns["SaveAs"].Key = "SaveAs";
			DataGridHistory.RootTable.Columns["SaveAs"].Caption = "SaveAs";
			DataGridHistory.RootTable.Columns["SaveAs"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["SaveAs"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["SaveAs"].ButtonText = "SaveAs";
			DataGridHistory.RootTable.Columns["SaveAs"].Width = 60;
			DataGridHistory.RootTable.Columns["SaveAs"].Position = 1;
			DataGridHistory.RootTable.Columns.Add("Print");
			DataGridHistory.RootTable.Columns["Print"].Key = "Print";
			DataGridHistory.RootTable.Columns["Print"].Caption = "Print";
			DataGridHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Print"].ButtonText = "Print";
			DataGridHistory.RootTable.Columns["Print"].Width = 50;
			DataGridHistory.RootTable.Columns["Print"].Position = 2;
			DataGridHistory.FrozenColumns = 3;
			ctrlGrdBar3_Load(null, null);
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

	private void DataGridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Edit")
			{
				btnsave.Visible = false;
				btnSaveAs.Visible = false;
				btnUpdate.Visible = true;
				ReadById(Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells["Id"].Value.ToString()));
			}
			if (e.Column.Key == "SaveAs")
			{
				btnsave.Visible = false;
				btnUpdate.Visible = false;
				btnSaveAs.Visible = true;
				ReadById(Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells["Id"].Value.ToString()));
			}
			if (e.Column.Key == "Print")
			{
				CommonServices.PrintJobOrder620(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbperemeter_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (Convert.ToInt32(cmbperemeter.Value) == 1)
			{
				FromDateHistory.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 2)
			{
				FromDateHistory.Value = DateTime.Today.AddDays(-7.0);
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 3)
			{
				DateTime now = DateTime.UtcNow;
				DateTime first = now.Date.AddDays(-(now.Date.Day - 1));
				FromDateHistory.Value = first;
				ToDateHistory.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 4)
			{
				int year = DateTime.Now.Year;
				DateTime first2 = new DateTime(year, 1, 1);
				FromDateHistory.Value = first2;
				ToDateHistory.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 5)
			{
				FromDateHistory.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
				ToDateHistory.Value = DateTime.Now;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnshowHistory_Click(object sender, EventArgs e)
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
		if (e.Control && e.KeyCode == Keys.Down)
		{
			if (((Control)(object)GridDetail).Focused)
			{
				((Control)(object)grdByProduct).Focus();
			}
			else if (((Control)(object)grdByProduct).Focused)
			{
				((Control)(object)grdFinishGoods).Focus();
			}
			else if (((Control)(object)grdFinishGoods).Focused)
			{
				((Control)(object)GridDetail).Focus();
			}
			else
			{
				((Control)(object)GridDetail).Focus();
			}
		}
		if (e.Control && e.KeyCode == Keys.Up)
		{
			if (((Control)(object)GridDetail).Focused)
			{
				cmbPlantFeader.Focus();
			}
			else if (((Control)(object)grdByProduct).Focused)
			{
				datEffectiveDateByProduct.Focus();
			}
			else if (((Control)(object)grdFinishGoods).Focused)
			{
				datEffectiveDateFinish.Focus();
			}
			else
			{
				cmbPlantFeader.Focus();
			}
		}
		else if (tabControl1.SelectedIndex == 1)
		{
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)DataGridHistory).Focus();
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				btnshowHistory_Click(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.Up)
			{
				cmbperemeter.Focus();
			}
			else if (e.Control && e.KeyCode == Keys.N)
			{
				btnNewHistory_Click(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.Right && ((Control)(object)DataGridHistory).Focused)
			{
				((Control)(object)GridDetail).Focus();
			}
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

	private void Print_Click(object sender, EventArgs e)
	{
		RECID = Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells["Id"].Value.ToString());
		CommonServices.PrintJobOrder620(RECID);
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

	private void btnProductionTypeDefine_Click(object sender, EventArgs e)
	{
		try
		{
			new DefineProductionType(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPlanTypeDefine_Click(object sender, EventArgs e)
	{
		try
		{
			new DefineProductionPlanType(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtAvgFinishGoodRate_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtOrderNoFrom_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtOrderNoTo_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtRateByProduct_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtRateFinish_KeyPress(object sender, KeyPressEventArgs e)
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

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)GridDetail).Name);
			if (fs != null)
			{
				GridDetail.LoadLayoutFile((Stream)fs);
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdByProduct).Name);
			if (fs != null)
			{
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
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlGrdBar4_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdFinishGoods).Name);
			if (fs != null)
			{
				grdFinishGoods.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void btnLoadScheduleInfo_Click(object sender, EventArgs e)
	{
		try
		{
			Loader = new LoadExportScheduleForProduction(UserAccount);
			Loader.ShowDialog();
			Loader.BringToFront();
			LoadInGrid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void LoadInGrid()
	{
		try
		{
			DataTable dt = Loader.dtLoader;
			if (dt.Rows.Count > 0)
			{
				bool flag = false;
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (grdScheduleInfo.GetRows().Count() > 0)
					{
						GridEXRow[] rows = grdScheduleInfo.GetRows();
						foreach (GridEXRow item in rows)
						{
							if (Conversion.ToInt(item.Cells["ScheduleId"].Value) == Conversion.ToInt(dt.Rows[i]["Id"]) && Conversion.ToInt(item.Cells["ItemId"].Value) == Conversion.ToInt(dt.Rows[i]["ItemId"]))
							{
								flag = true;
								break;
							}
						}
					}
					if (!flag)
					{
						dtExportSchedule.Rows.Add(0, dt.Rows[i]["ContractId"], dt.Rows[i]["ContractNo"], dt.Rows[i]["Id"], dt.Rows[i]["ScheduleCode"], dt.Rows[i]["ItemId"], dt.Rows[i]["ItemName"], dt.Rows[i]["MTon"], "");
					}
					flag = false;
				}
			}
			grdScheduleInfo.DataSource = dtExportSchedule;
			grdScheduleInfo.RetrieveStructure();
			ScheduleGridSetting();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnGenerateProductionNo_Click(object sender, EventArgs e)
	{
		try
		{
			new frmGenerateJobOrderNos(UserAccount).Show();
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
		//IL_0420: Unknown result type (might be due to invalid IL or missing references)
		//IL_0427: Expected O, but got Unknown
		//IL_0427: Unknown result type (might be due to invalid IL or missing references)
		//IL_042e: Expected O, but got Unknown
		//IL_042e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0435: Expected O, but got Unknown
		//IL_0435: Unknown result type (might be due to invalid IL or missing references)
		//IL_043c: Expected O, but got Unknown
		//IL_043c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0443: Expected O, but got Unknown
		//IL_0443: Unknown result type (might be due to invalid IL or missing references)
		//IL_044a: Expected O, but got Unknown
		//IL_044a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0451: Expected O, but got Unknown
		//IL_0451: Unknown result type (might be due to invalid IL or missing references)
		//IL_0458: Expected O, but got Unknown
		//IL_0458: Unknown result type (might be due to invalid IL or missing references)
		//IL_045f: Expected O, but got Unknown
		//IL_045f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0466: Expected O, but got Unknown
		//IL_0466: Unknown result type (might be due to invalid IL or missing references)
		//IL_046d: Expected O, but got Unknown
		//IL_046d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0474: Expected O, but got Unknown
		//IL_0474: Unknown result type (might be due to invalid IL or missing references)
		//IL_047b: Expected O, but got Unknown
		//IL_047b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0482: Expected O, but got Unknown
		//IL_0482: Unknown result type (might be due to invalid IL or missing references)
		//IL_0489: Expected O, but got Unknown
		//IL_0489: Unknown result type (might be due to invalid IL or missing references)
		//IL_0490: Expected O, but got Unknown
		//IL_0490: Unknown result type (might be due to invalid IL or missing references)
		//IL_0497: Expected O, but got Unknown
		//IL_0497: Unknown result type (might be due to invalid IL or missing references)
		//IL_049e: Expected O, but got Unknown
		//IL_049e: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a5: Expected O, but got Unknown
		//IL_04a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ac: Expected O, but got Unknown
		//IL_04ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b3: Expected O, but got Unknown
		//IL_04b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ba: Expected O, but got Unknown
		//IL_04ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c1: Expected O, but got Unknown
		//IL_04c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c8: Expected O, but got Unknown
		//IL_052c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0536: Expected O, but got Unknown
		//IL_0584: Unknown result type (might be due to invalid IL or missing references)
		//IL_058e: Expected O, but got Unknown
		//IL_05c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d0: Expected O, but got Unknown
		//IL_061e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0628: Expected O, but got Unknown
		//IL_066b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0675: Expected O, but got Unknown
		//IL_06c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06cd: Expected O, but got Unknown
		//IL_06fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0704: Expected O, but got Unknown
		//IL_073c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0746: Expected O, but got Unknown
		//IL_07b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07bf: Expected O, but got Unknown
		//IL_07f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0801: Expected O, but got Unknown
		//IL_080d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0817: Expected O, but got Unknown
		//IL_0823: Unknown result type (might be due to invalid IL or missing references)
		//IL_082d: Expected O, but got Unknown
		//IL_0844: Unknown result type (might be due to invalid IL or missing references)
		//IL_084e: Expected O, but got Unknown
		//IL_0865: Unknown result type (might be due to invalid IL or missing references)
		//IL_086f: Expected O, but got Unknown
		//IL_087b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0885: Expected O, but got Unknown
		//IL_0957: Unknown result type (might be due to invalid IL or missing references)
		//IL_0961: Expected O, but got Unknown
		//IL_09e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f0: Expected O, but got Unknown
		//IL_0a28: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a32: Expected O, but got Unknown
		//IL_0a96: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa0: Expected O, but got Unknown
		//IL_1336: Unknown result type (might be due to invalid IL or missing references)
		//IL_1340: Expected O, but got Unknown
		//IL_2021: Unknown result type (might be due to invalid IL or missing references)
		//IL_202b: Expected O, but got Unknown
		//IL_2d70: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d7a: Expected O, but got Unknown
		//IL_7807: Unknown result type (might be due to invalid IL or missing references)
		//IL_7811: Expected O, but got Unknown
		//IL_781e: Unknown result type (might be due to invalid IL or missing references)
		//IL_7828: Expected O, but got Unknown
		GridEXLayout grdFinishGoods_DesignTimeLayout = new GridEXLayout();
		Appearance appearance1 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance93 = new Appearance();
		GridEXLayout grdByProduct_DesignTimeLayout = new GridEXLayout();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		GridEXLayout GridDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance100 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance112 = new Appearance();
		GridEXLayout grdScheduleInfo_DesignTimeLayout = new GridEXLayout();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
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
		Appearance appearance153 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance157 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance165 = new Appearance();
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
		Appearance appearance124 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance168 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Production.frmProductionJobOrderMain));
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
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
		Appearance appearance26 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		this.miniToolStrip = new System.Windows.Forms.ToolStrip();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.panel16 = new System.Windows.Forms.Panel();
		this.grdFinishGoods = new GridEX();
		this.panel17 = new System.Windows.Forms.Panel();
		this.btnCancelFinish = new System.Windows.Forms.Button();
		this.label24 = new System.Windows.Forms.Label();
		this.txtRateFinish = new System.Windows.Forms.TextBox();
		this.btnAddUpdateFinish = new System.Windows.Forms.Button();
		this.label25 = new System.Windows.Forms.Label();
		this.label26 = new System.Windows.Forms.Label();
		this.CmbItemFinish = new UltraCombo();
		this.datEffectiveDateFinish = new System.Windows.Forms.DateTimePicker();
		this.panel18 = new System.Windows.Forms.Panel();
		this.label27 = new System.Windows.Forms.Label();
		this.ctrlGrdBar4 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel13 = new System.Windows.Forms.Panel();
		this.grdByProduct = new GridEX();
		this.panel15 = new System.Windows.Forms.Panel();
		this.btnCancelByProduct = new System.Windows.Forms.Button();
		this.label22 = new System.Windows.Forms.Label();
		this.txtRateByProduct = new System.Windows.Forms.TextBox();
		this.btnAddUpdateByProduct = new System.Windows.Forms.Button();
		this.label17 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.CmbItemByProduct = new UltraCombo();
		this.datEffectiveDateByProduct = new System.Windows.Forms.DateTimePicker();
		this.panel14 = new System.Windows.Forms.Panel();
		this.label12 = new System.Windows.Forms.Label();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel6 = new System.Windows.Forms.Panel();
		this.panel19 = new System.Windows.Forms.Panel();
		this.GridDetail = new GridEX();
		this.panel11 = new System.Windows.Forms.Panel();
		this.btnplus = new System.Windows.Forms.Button();
		this.txtRemarksDetail = new System.Windows.Forms.TextBox();
		this.label8 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.btnCancelUpdateDetial = new System.Windows.Forms.Button();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.cmbPlantFeader = new UltraCombo();
		this.panel7 = new System.Windows.Forms.Panel();
		this.label5 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.tabPage4 = new System.Windows.Forms.TabPage();
		this.grdScheduleInfo = new GridEX();
		this.panel22 = new System.Windows.Forms.Panel();
		this.btnLoadScheduleInfo = new System.Windows.Forms.Button();
		this.label34 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.CmbProductionNo = new UltraCombo();
		this.txtPlanDate = new System.Windows.Forms.DateTimePicker();
		this.txtPlanCode = new System.Windows.Forms.TextBox();
		this.label7 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.btnPlanTypeDefine = new System.Windows.Forms.Button();
		this.txtAvgFinishGoodRate = new System.Windows.Forms.TextBox();
		this.btnProductionTypeDefine = new System.Windows.Forms.Button();
		this.txtOtherInstruction = new System.Windows.Forms.TextBox();
		this.label23 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.CmbWipItem = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.txtProductionNo = new System.Windows.Forms.TextBox();
		this.label3 = new System.Windows.Forms.Label();
		this.CmbWorkInProcess = new UltraCombo();
		this.label15 = new System.Windows.Forms.Label();
		this.cmbStatus = new UltraCombo();
		this.txtOtherReference = new System.Windows.Forms.TextBox();
		this.cmbProductionType = new UltraCombo();
		this.txtEndDate = new System.Windows.Forms.DateTimePicker();
		this.label13 = new System.Windows.Forms.Label();
		this.cmbPlanType = new UltraCombo();
		this.txtstartDate = new System.Windows.Forms.DateTimePicker();
		this.label16 = new System.Windows.Forms.Label();
		this.CmbByProductionAc = new UltraCombo();
		this.label21 = new System.Windows.Forms.Label();
		this.CmbFinishGoodsAc = new UltraCombo();
		this.label20 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.txtPlanTypeSrNo = new System.Windows.Forms.TextBox();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label32 = new System.Windows.Forms.Label();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnSaveAs = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.Print = new System.Windows.Forms.ToolStripButton();
		this.btnattachment = new System.Windows.Forms.ToolStripButton();
		this.btnGenerateProductionNo = new System.Windows.Forms.ToolStripButton();
		this.chkPrint = new System.Windows.Forms.CheckBox();
		this.btnProductionPlant = new System.Windows.Forms.Button();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel12 = new System.Windows.Forms.Panel();
		this.panel10 = new System.Windows.Forms.Panel();
		this.DataGridHistory = new GridEX();
		this.panel21 = new System.Windows.Forms.Panel();
		this.label18 = new System.Windows.Forms.Label();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel20 = new System.Windows.Forms.Panel();
		this.rdapproveddate = new System.Windows.Forms.RadioButton();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.label30 = new System.Windows.Forms.Label();
		this.txtOrderNoTo = new System.Windows.Forms.TextBox();
		this.txtOrderNoFrom = new System.Windows.Forms.TextBox();
		this.label29 = new System.Windows.Forms.Label();
		this.cmbperemeter = new UltraCombo();
		this.label28 = new System.Windows.Forms.Label();
		this.btnshowHistory = new System.Windows.Forms.Button();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.label31 = new System.Windows.Forms.Label();
		this.xmbjoborderno = new UltraCombo();
		this.label33 = new System.Windows.Forms.Label();
		this.panel9 = new System.Windows.Forms.Panel();
		this.label9 = new System.Windows.Forms.Label();
		this.panel8 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
		this.label35 = new System.Windows.Forms.Label();
		this.CmbWipWarehouse = new UltraCombo();
		this.label36 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel4.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.tabPage3.SuspendLayout();
		this.panel16.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdFinishGoods).BeginInit();
		this.panel17.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbItemFinish).BeginInit();
		this.panel18.SuspendLayout();
		this.panel13.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdByProduct).BeginInit();
		this.panel15.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbItemByProduct).BeginInit();
		this.panel14.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel19.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridDetail).BeginInit();
		this.panel11.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbPlantFeader).BeginInit();
		this.panel7.SuspendLayout();
		this.tabPage4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdScheduleInfo).BeginInit();
		this.panel22.SuspendLayout();
		this.panel5.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbProductionNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWipItem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWorkInProcess).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbStatus).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbProductionType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPlanType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbByProductionAc).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbFinishGoodsAc).BeginInit();
		this.panel3.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel12.SuspendLayout();
		this.panel10.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel21.SuspendLayout();
		this.panel20.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbperemeter).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.xmbjoborderno).BeginInit();
		this.panel9.SuspendLayout();
		this.panel8.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbWipWarehouse).BeginInit();
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
		this.panel1.Size = new System.Drawing.Size(1306, 709);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1306, 709);
		this.tabControl1.TabIndex = 0;
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1298, 680);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add(this.panel4);
		this.panel2.Controls.Add(this.panel3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1292, 674);
		this.panel2.TabIndex = 0;
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.tabControl2);
		this.panel4.Controls.Add(this.panel5);
		this.panel4.Controls.Add(this.label19);
		this.panel4.Controls.Add(this.txtPlanTypeSrNo);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 50);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1292, 624);
		this.panel4.TabIndex = 61;
		this.tabControl2.Controls.Add(this.tabPage3);
		this.tabControl2.Controls.Add(this.tabPage4);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl2.Location = new System.Drawing.Point(0, 135);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(1292, 489);
		this.tabControl2.TabIndex = 1;
		this.tabPage3.Controls.Add(this.panel16);
		this.tabPage3.Controls.Add(this.panel13);
		this.tabPage3.Controls.Add(this.panel6);
		this.tabPage3.Location = new System.Drawing.Point(4, 25);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage3.Size = new System.Drawing.Size(1284, 460);
		this.tabPage3.TabIndex = 0;
		this.tabPage3.Text = "Job Order Detail";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.panel16.Controls.Add((System.Windows.Forms.Control)(object)this.grdFinishGoods);
		this.panel16.Controls.Add(this.panel17);
		this.panel16.Controls.Add(this.panel18);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel16.Location = new System.Drawing.Point(835, 3);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(444, 454);
		this.panel16.TabIndex = 3;
		this.grdFinishGoods.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdFinishGoods).BackColor = System.Drawing.Color.White;
		grdFinishGoods_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdFinishGoods.DesignTimeLayout = grdFinishGoods_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdFinishGoods).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdFinishGoods).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdFinishGoods.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdFinishGoods).Location = new System.Drawing.Point(0, 87);
		((System.Windows.Forms.Control)(object)this.grdFinishGoods).Name = "grdFinishGoods";
		((ScrollableBase)this.grdFinishGoods).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdFinishGoods).Size = new System.Drawing.Size(444, 367);
		((System.Windows.Forms.Control)(object)this.grdFinishGoods).TabIndex = 3;
		this.grdFinishGoods.TotalRow = (InheritableBoolean)1;
		this.grdFinishGoods.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdFinishGoods).VisualStyle = (VisualStyle)4;
		this.grdFinishGoods.ColumnButtonClick += new ColumnActionEventHandler(grdFinishGoods_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdFinishGoods).DoubleClick += new System.EventHandler(grdFinishGoods_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdFinishGoods).KeyDown += new System.Windows.Forms.KeyEventHandler(grdFinishGoods_KeyDown);
		this.panel17.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel17.Controls.Add(this.btnCancelFinish);
		this.panel17.Controls.Add(this.label24);
		this.panel17.Controls.Add(this.txtRateFinish);
		this.panel17.Controls.Add(this.btnAddUpdateFinish);
		this.panel17.Controls.Add(this.label25);
		this.panel17.Controls.Add(this.label26);
		this.panel17.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemFinish);
		this.panel17.Controls.Add(this.datEffectiveDateFinish);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel17.Location = new System.Drawing.Point(0, 24);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(444, 63);
		this.panel17.TabIndex = 2;
		this.btnCancelFinish.BackColor = System.Drawing.Color.Teal;
		this.btnCancelFinish.FlatAppearance.BorderSize = 0;
		this.btnCancelFinish.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelFinish.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelFinish.ForeColor = System.Drawing.Color.White;
		this.btnCancelFinish.Location = new System.Drawing.Point(382, 10);
		this.btnCancelFinish.Name = "btnCancelFinish";
		this.btnCancelFinish.Size = new System.Drawing.Size(56, 22);
		this.btnCancelFinish.TabIndex = 4;
		this.btnCancelFinish.Text = "Cancel";
		this.btnCancelFinish.UseVisualStyleBackColor = false;
		this.btnCancelFinish.Visible = false;
		this.btnCancelFinish.Click += new System.EventHandler(btnCancelFinish_Click);
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(309, 10);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(61, 15);
		this.label24.TabIndex = 180;
		this.label24.Text = "Rate 40Kg";
		this.txtRateFinish.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRateFinish.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRateFinish.Location = new System.Drawing.Point(309, 33);
		this.txtRateFinish.Name = "txtRateFinish";
		this.txtRateFinish.Size = new System.Drawing.Size(72, 23);
		this.txtRateFinish.TabIndex = 2;
		this.txtRateFinish.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtRateFinish.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtRateFinish_KeyPress);
		this.btnAddUpdateFinish.BackColor = System.Drawing.Color.Teal;
		this.btnAddUpdateFinish.FlatAppearance.BorderSize = 0;
		this.btnAddUpdateFinish.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAddUpdateFinish.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAddUpdateFinish.ForeColor = System.Drawing.Color.White;
		this.btnAddUpdateFinish.Location = new System.Drawing.Point(382, 33);
		this.btnAddUpdateFinish.Name = "btnAddUpdateFinish";
		this.btnAddUpdateFinish.Size = new System.Drawing.Size(56, 23);
		this.btnAddUpdateFinish.TabIndex = 3;
		this.btnAddUpdateFinish.Text = "Add";
		this.btnAddUpdateFinish.UseVisualStyleBackColor = false;
		this.btnAddUpdateFinish.Visible = false;
		this.btnAddUpdateFinish.Click += new System.EventHandler(btnAddUpdateFinish_Click);
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(117, 10);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(67, 15);
		this.label25.TabIndex = 168;
		this.label25.Text = "Item Name";
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(3, 10);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(80, 15);
		this.label26.TabIndex = 179;
		this.label26.Text = "Effective Date";
		this.CmbItemFinish.AllowNull = (DefaultableBoolean)1;
		this.CmbItemFinish.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemFinish.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemFinish.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemFinish).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbItemFinish).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemFinish).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance114).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance114).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance114).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemFinish).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance114;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemFinish).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance125;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemFinish).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance136).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance136).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance136).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemFinish).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.CmbItemFinish).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemFinish).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance147).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemFinish).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance147;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance158).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemFinish).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance158;
		((UltraGridBase)this.CmbItemFinish).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemFinish).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance2).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemFinish).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance2;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemFinish).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbItemFinish).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemFinish).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance24).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemFinish).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance84).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemFinish).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.CmbItemFinish).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemFinish).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance91).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemFinish).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.CmbItemFinish).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemFinish).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance93;
		((UltraGridBase)this.CmbItemFinish).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemFinish).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemFinish).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemFinish).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemFinish.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemFinish).Location = new System.Drawing.Point(117, 32);
		((System.Windows.Forms.Control)(object)this.CmbItemFinish).Name = "CmbItemFinish";
		((System.Windows.Forms.Control)(object)this.CmbItemFinish).Size = new System.Drawing.Size(191, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemFinish).TabIndex = 1;
		this.datEffectiveDateFinish.CustomFormat = "dd-MMM-yy";
		this.datEffectiveDateFinish.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datEffectiveDateFinish.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datEffectiveDateFinish.Location = new System.Drawing.Point(3, 34);
		this.datEffectiveDateFinish.Name = "datEffectiveDateFinish";
		this.datEffectiveDateFinish.Size = new System.Drawing.Size(112, 23);
		this.datEffectiveDateFinish.TabIndex = 0;
		this.panel18.BackColor = System.Drawing.Color.Teal;
		this.panel18.Controls.Add(this.label27);
		this.panel18.Controls.Add(this.ctrlGrdBar4);
		this.panel18.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel18.Location = new System.Drawing.Point(0, 0);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(444, 24);
		this.panel18.TabIndex = 1;
		this.label27.AutoSize = true;
		this.label27.BackColor = System.Drawing.Color.Teal;
		this.label27.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.ForeColor = System.Drawing.Color.White;
		this.label27.Location = new System.Drawing.Point(1, 4);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(198, 16);
		this.label27.TabIndex = 68;
		this.label27.Text = " (Finish Goods Rate Schedule)";
		this.ctrlGrdBar4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar4.FormName = this;
		this.ctrlGrdBar4.Location = new System.Drawing.Point(403, 2);
		this.ctrlGrdBar4.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar4.MyGrid = this.grdFinishGoods;
		this.ctrlGrdBar4.Name = "ctrlGrdBar4";
		this.ctrlGrdBar4.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar4.TabIndex = 71;
		this.ctrlGrdBar4.Load += new System.EventHandler(ctrlGrdBar4_Load);
		this.panel13.Controls.Add((System.Windows.Forms.Control)(object)this.grdByProduct);
		this.panel13.Controls.Add(this.panel15);
		this.panel13.Controls.Add(this.panel14);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel13.Location = new System.Drawing.Point(375, 3);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(460, 454);
		this.panel13.TabIndex = 2;
		this.grdByProduct.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdByProduct).BackColor = System.Drawing.Color.White;
		grdByProduct_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdByProduct.DesignTimeLayout = grdByProduct_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdByProduct).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdByProduct).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdByProduct.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdByProduct).Location = new System.Drawing.Point(0, 87);
		((System.Windows.Forms.Control)(object)this.grdByProduct).Name = "grdByProduct";
		((ScrollableBase)this.grdByProduct).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdByProduct).Size = new System.Drawing.Size(460, 367);
		((System.Windows.Forms.Control)(object)this.grdByProduct).TabIndex = 3;
		this.grdByProduct.TotalRow = (InheritableBoolean)1;
		this.grdByProduct.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdByProduct).VisualStyle = (VisualStyle)4;
		this.grdByProduct.ColumnButtonClick += new ColumnActionEventHandler(grdByProduct_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdByProduct).DoubleClick += new System.EventHandler(grdByProduct_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdByProduct).KeyDown += new System.Windows.Forms.KeyEventHandler(grdByProduct_KeyDown);
		this.panel15.Controls.Add(this.btnCancelByProduct);
		this.panel15.Controls.Add(this.label22);
		this.panel15.Controls.Add(this.txtRateByProduct);
		this.panel15.Controls.Add(this.btnAddUpdateByProduct);
		this.panel15.Controls.Add(this.label17);
		this.panel15.Controls.Add(this.label14);
		this.panel15.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemByProduct);
		this.panel15.Controls.Add(this.datEffectiveDateByProduct);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.Location = new System.Drawing.Point(0, 24);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(460, 63);
		this.panel15.TabIndex = 2;
		this.btnCancelByProduct.BackColor = System.Drawing.Color.Teal;
		this.btnCancelByProduct.FlatAppearance.BorderSize = 0;
		this.btnCancelByProduct.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelByProduct.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelByProduct.ForeColor = System.Drawing.Color.White;
		this.btnCancelByProduct.Location = new System.Drawing.Point(397, 11);
		this.btnCancelByProduct.Name = "btnCancelByProduct";
		this.btnCancelByProduct.Size = new System.Drawing.Size(56, 22);
		this.btnCancelByProduct.TabIndex = 4;
		this.btnCancelByProduct.Text = "Cancel";
		this.btnCancelByProduct.UseVisualStyleBackColor = false;
		this.btnCancelByProduct.Visible = false;
		this.btnCancelByProduct.Click += new System.EventHandler(btnCancelByProduct_Click);
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(309, 10);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(61, 15);
		this.label22.TabIndex = 180;
		this.label22.Text = "Rate 40Kg";
		this.txtRateByProduct.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRateByProduct.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRateByProduct.Location = new System.Drawing.Point(309, 33);
		this.txtRateByProduct.Name = "txtRateByProduct";
		this.txtRateByProduct.Size = new System.Drawing.Size(86, 23);
		this.txtRateByProduct.TabIndex = 2;
		this.txtRateByProduct.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtRateByProduct.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtRateByProduct_KeyPress);
		this.btnAddUpdateByProduct.BackColor = System.Drawing.Color.Teal;
		this.btnAddUpdateByProduct.FlatAppearance.BorderSize = 0;
		this.btnAddUpdateByProduct.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAddUpdateByProduct.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAddUpdateByProduct.ForeColor = System.Drawing.Color.White;
		this.btnAddUpdateByProduct.Location = new System.Drawing.Point(397, 34);
		this.btnAddUpdateByProduct.Name = "btnAddUpdateByProduct";
		this.btnAddUpdateByProduct.Size = new System.Drawing.Size(56, 23);
		this.btnAddUpdateByProduct.TabIndex = 3;
		this.btnAddUpdateByProduct.Text = "Add";
		this.btnAddUpdateByProduct.UseVisualStyleBackColor = false;
		this.btnAddUpdateByProduct.Visible = false;
		this.btnAddUpdateByProduct.Click += new System.EventHandler(btnAddUpdateByProduct_Click);
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(117, 10);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(67, 15);
		this.label17.TabIndex = 168;
		this.label17.Text = "Item Name";
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(3, 10);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(80, 15);
		this.label14.TabIndex = 179;
		this.label14.Text = "Effective Date";
		this.CmbItemByProduct.AllowNull = (DefaultableBoolean)1;
		this.CmbItemByProduct.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemByProduct.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemByProduct.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance94).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemByProduct).DisplayLayout.Appearance = (AppearanceBase)(object)appearance94;
		((UltraGridBase)this.CmbItemByProduct).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemByProduct).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance95).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance95).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance95).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemByProduct).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance95;
		((AppearanceBase)appearance96).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemByProduct).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance96;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemByProduct).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance97).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance97).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance97).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemByProduct).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.CmbItemByProduct).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemByProduct).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance98).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemByProduct).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance98;
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance99).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemByProduct).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.CmbItemByProduct).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemByProduct).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemByProduct).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance128;
		((AppearanceBase)appearance129).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance129).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemByProduct).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.CmbItemByProduct).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemByProduct).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance130).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance130).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance130).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemByProduct).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance130;
		((AppearanceBase)appearance137).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemByProduct).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance137;
		((UltraGridBase)this.CmbItemByProduct).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemByProduct).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance138).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemByProduct).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.CmbItemByProduct).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemByProduct).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance139;
		((UltraGridBase)this.CmbItemByProduct).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemByProduct).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemByProduct).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemByProduct).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemByProduct.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemByProduct).Location = new System.Drawing.Point(117, 32);
		((System.Windows.Forms.Control)(object)this.CmbItemByProduct).Name = "CmbItemByProduct";
		((System.Windows.Forms.Control)(object)this.CmbItemByProduct).Size = new System.Drawing.Size(191, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemByProduct).TabIndex = 1;
		this.datEffectiveDateByProduct.CustomFormat = "dd-MMM-yy";
		this.datEffectiveDateByProduct.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datEffectiveDateByProduct.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datEffectiveDateByProduct.Location = new System.Drawing.Point(3, 34);
		this.datEffectiveDateByProduct.Name = "datEffectiveDateByProduct";
		this.datEffectiveDateByProduct.Size = new System.Drawing.Size(112, 23);
		this.datEffectiveDateByProduct.TabIndex = 0;
		this.panel14.BackColor = System.Drawing.Color.Teal;
		this.panel14.Controls.Add(this.label12);
		this.panel14.Controls.Add(this.ctrlGrdBar2);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel14.Location = new System.Drawing.Point(0, 0);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(460, 24);
		this.panel14.TabIndex = 1;
		this.label12.AutoSize = true;
		this.label12.BackColor = System.Drawing.Color.Teal;
		this.label12.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.ForeColor = System.Drawing.Color.White;
		this.label12.Location = new System.Drawing.Point(1, 4);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(191, 16);
		this.label12.TabIndex = 68;
		this.label12.Text = " (By Product Rate Schedule)";
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(419, 2);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdByProduct;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar2.TabIndex = 71;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.panel6.Controls.Add(this.panel19);
		this.panel6.Controls.Add(this.panel11);
		this.panel6.Controls.Add(this.panel7);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel6.Location = new System.Drawing.Point(3, 3);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(372, 454);
		this.panel6.TabIndex = 1;
		this.panel19.Controls.Add((System.Windows.Forms.Control)(object)this.GridDetail);
		this.panel19.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel19.Location = new System.Drawing.Point(0, 87);
		this.panel19.Name = "panel19";
		this.panel19.Size = new System.Drawing.Size(372, 367);
		this.panel19.TabIndex = 2;
		this.GridDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.GridDetail).BackColor = System.Drawing.Color.White;
		GridDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GridDetail.DesignTimeLayout = GridDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GridDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.GridDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GridDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.GridDetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.GridDetail).Name = "GridDetail";
		((ScrollableBase)this.GridDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GridDetail).Size = new System.Drawing.Size(372, 367);
		((System.Windows.Forms.Control)(object)this.GridDetail).TabIndex = 0;
		this.GridDetail.TotalRow = (InheritableBoolean)1;
		this.GridDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GridDetail).VisualStyle = (VisualStyle)4;
		this.GridDetail.ColumnButtonClick += new ColumnActionEventHandler(GridDetail_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.GridDetail).DoubleClick += new System.EventHandler(GridDetail_DoubleClick);
		this.panel11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel11.Controls.Add(this.btnplus);
		this.panel11.Controls.Add(this.txtRemarksDetail);
		this.panel11.Controls.Add(this.label8);
		this.panel11.Controls.Add(this.label6);
		this.panel11.Controls.Add(this.btnCancelUpdateDetial);
		this.panel11.Controls.Add(this.btnUpdateDetail);
		this.panel11.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPlantFeader);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(0, 24);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(372, 63);
		this.panel11.TabIndex = 1;
		this.btnplus.BackColor = System.Drawing.Color.Teal;
		this.btnplus.FlatAppearance.BorderSize = 0;
		this.btnplus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnplus.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnplus.ForeColor = System.Drawing.Color.White;
		this.btnplus.Location = new System.Drawing.Point(311, 33);
		this.btnplus.Name = "btnplus";
		this.btnplus.Size = new System.Drawing.Size(27, 24);
		this.btnplus.TabIndex = 3;
		this.btnplus.Text = "+";
		this.btnplus.UseVisualStyleBackColor = false;
		this.btnplus.Click += new System.EventHandler(btnplus_Click);
		this.txtRemarksDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarksDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksDetail.Location = new System.Drawing.Point(131, 34);
		this.txtRemarksDetail.Name = "txtRemarksDetail";
		this.txtRemarksDetail.Size = new System.Drawing.Size(178, 23);
		this.txtRemarksDetail.TabIndex = 2;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(4, 11);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(80, 15);
		this.label8.TabIndex = 69;
		this.label8.Text = "Plant / Feader";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(131, 11);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(65, 15);
		this.label6.TabIndex = 180;
		this.label6.Text = "Comments";
		this.btnCancelUpdateDetial.BackColor = System.Drawing.Color.Teal;
		this.btnCancelUpdateDetial.FlatAppearance.BorderSize = 0;
		this.btnCancelUpdateDetial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelUpdateDetial.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelUpdateDetial.ForeColor = System.Drawing.Color.White;
		this.btnCancelUpdateDetial.Location = new System.Drawing.Point(311, 8);
		this.btnCancelUpdateDetial.Name = "btnCancelUpdateDetial";
		this.btnCancelUpdateDetial.Size = new System.Drawing.Size(56, 24);
		this.btnCancelUpdateDetial.TabIndex = 5;
		this.btnCancelUpdateDetial.Text = "Cancel";
		this.btnCancelUpdateDetial.UseVisualStyleBackColor = false;
		this.btnCancelUpdateDetial.Visible = false;
		this.btnCancelUpdateDetial.Click += new System.EventHandler(btnCancelUpdateDetial_Click);
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatAppearance.BorderSize = 0;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(311, 33);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(56, 24);
		this.btnUpdateDetail.TabIndex = 4;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Visible = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.cmbPlantFeader.AllowNull = (DefaultableBoolean)1;
		this.cmbPlantFeader.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPlantFeader.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPlantFeader.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.Appearance = (AppearanceBase)(object)appearance100;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance80).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance80).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPlantFeader).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance81;
		((SpecialBoxBase)((UltraGridBase)this.cmbPlantFeader).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance83).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance83).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance105).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance105;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance106).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance107;
		((AppearanceBase)appearance108).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance108).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance109).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance109).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance109).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance109).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance109;
		((AppearanceBase)appearance110).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance111).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPlantFeader).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPlantFeader).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbPlantFeader.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPlantFeader).Location = new System.Drawing.Point(4, 32);
		((System.Windows.Forms.Control)(object)this.cmbPlantFeader).Name = "cmbPlantFeader";
		((System.Windows.Forms.Control)(object)this.cmbPlantFeader).Size = new System.Drawing.Size(126, 26);
		((System.Windows.Forms.Control)(object)this.cmbPlantFeader).TabIndex = 0;
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.label5);
		this.panel7.Controls.Add(this.ctrlGrdBar1);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(372, 24);
		this.panel7.TabIndex = 0;
		this.label5.AutoSize = true;
		this.label5.BackColor = System.Drawing.Color.Teal;
		this.label5.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.ForeColor = System.Drawing.Color.White;
		this.label5.Location = new System.Drawing.Point(1, 4);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(272, 16);
		this.label5.TabIndex = 68;
		this.label5.Text = " (Allocate Multiple Plants To a Job Order)";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(331, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.GridDetail;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 71;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.tabPage4.Controls.Add((System.Windows.Forms.Control)(object)this.grdScheduleInfo);
		this.tabPage4.Controls.Add(this.panel22);
		this.tabPage4.Location = new System.Drawing.Point(4, 25);
		this.tabPage4.Name = "tabPage4";
		this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage4.Size = new System.Drawing.Size(1284, 484);
		this.tabPage4.TabIndex = 1;
		this.tabPage4.Text = "Export Schedule Info";
		this.tabPage4.UseVisualStyleBackColor = true;
		((ScrollableBase)this.grdScheduleInfo).BackColor = System.Drawing.Color.White;
		grdScheduleInfo_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdScheduleInfo.DesignTimeLayout = grdScheduleInfo_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdScheduleInfo).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdScheduleInfo).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdScheduleInfo.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdScheduleInfo).Location = new System.Drawing.Point(3, 31);
		((System.Windows.Forms.Control)(object)this.grdScheduleInfo).Name = "grdScheduleInfo";
		((ScrollableBase)this.grdScheduleInfo).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdScheduleInfo).Size = new System.Drawing.Size(1278, 450);
		((System.Windows.Forms.Control)(object)this.grdScheduleInfo).TabIndex = 2;
		this.grdScheduleInfo.TotalRow = (InheritableBoolean)1;
		this.grdScheduleInfo.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdScheduleInfo).VisualStyle = (VisualStyle)4;
		this.panel22.BackColor = System.Drawing.Color.Teal;
		this.panel22.Controls.Add(this.btnLoadScheduleInfo);
		this.panel22.Controls.Add(this.label34);
		this.panel22.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel22.Location = new System.Drawing.Point(3, 3);
		this.panel22.Name = "panel22";
		this.panel22.Size = new System.Drawing.Size(1278, 28);
		this.panel22.TabIndex = 1;
		this.btnLoadScheduleInfo.BackColor = System.Drawing.Color.FromArgb(0, 64, 64);
		this.btnLoadScheduleInfo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnLoadScheduleInfo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLoadScheduleInfo.ForeColor = System.Drawing.Color.White;
		this.btnLoadScheduleInfo.Image = Architecture.WinApp.Properties.Resources.download_line;
		this.btnLoadScheduleInfo.Location = new System.Drawing.Point(1076, 0);
		this.btnLoadScheduleInfo.Margin = new System.Windows.Forms.Padding(2);
		this.btnLoadScheduleInfo.Name = "btnLoadScheduleInfo";
		this.btnLoadScheduleInfo.Size = new System.Drawing.Size(197, 28);
		this.btnLoadScheduleInfo.TabIndex = 69;
		this.btnLoadScheduleInfo.Text = "Load Schedule Info";
		this.btnLoadScheduleInfo.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
		this.btnLoadScheduleInfo.UseVisualStyleBackColor = false;
		this.btnLoadScheduleInfo.Click += new System.EventHandler(btnLoadScheduleInfo_Click);
		this.label34.AutoSize = true;
		this.label34.BackColor = System.Drawing.Color.Teal;
		this.label34.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.ForeColor = System.Drawing.Color.White;
		this.label34.Location = new System.Drawing.Point(1, 4);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(207, 16);
		this.label34.TabIndex = 68;
		this.label34.Text = " (Export Schedule Information)";
		this.panel5.Controls.Add(this.groupBox2);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbByProductionAc);
		this.panel5.Controls.Add(this.label21);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbFinishGoodsAc);
		this.panel5.Controls.Add(this.label20);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1292, 135);
		this.panel5.TabIndex = 0;
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbProductionNo);
		this.groupBox2.Controls.Add(this.txtPlanDate);
		this.groupBox2.Controls.Add(this.txtPlanCode);
		this.groupBox2.Controls.Add(this.label7);
		this.groupBox2.Controls.Add(this.label4);
		this.groupBox2.Controls.Add(this.btnPlanTypeDefine);
		this.groupBox2.Controls.Add(this.txtAvgFinishGoodRate);
		this.groupBox2.Controls.Add(this.btnProductionTypeDefine);
		this.groupBox2.Controls.Add(this.txtOtherInstruction);
		this.groupBox2.Controls.Add(this.label23);
		this.groupBox2.Controls.Add(this.label11);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWipItem);
		this.groupBox2.Controls.Add(this.label1);
		this.groupBox2.Controls.Add(this.label10);
		this.groupBox2.Controls.Add(this.label2);
		this.groupBox2.Controls.Add(this.txtProductionNo);
		this.groupBox2.Controls.Add(this.label3);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWipWarehouse);
		this.groupBox2.Controls.Add(this.label35);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWorkInProcess);
		this.groupBox2.Controls.Add(this.label15);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbStatus);
		this.groupBox2.Controls.Add(this.txtOtherReference);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbProductionType);
		this.groupBox2.Controls.Add(this.txtEndDate);
		this.groupBox2.Controls.Add(this.label13);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPlanType);
		this.groupBox2.Controls.Add(this.txtstartDate);
		this.groupBox2.Controls.Add(this.label36);
		this.groupBox2.Controls.Add(this.label16);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(4, 3);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(1161, 130);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Main";
		this.CmbProductionNo.AllowNull = (DefaultableBoolean)1;
		this.CmbProductionNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbProductionNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbProductionNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbProductionNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.CmbProductionNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbProductionNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance141).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance141).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance141).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbProductionNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance141;
		((AppearanceBase)appearance142).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbProductionNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance142;
		((SpecialBoxBase)((UltraGridBase)this.CmbProductionNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance143).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance143).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance143).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbProductionNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.CmbProductionNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbProductionNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance144).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbProductionNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance144;
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance145).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbProductionNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.CmbProductionNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbProductionNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbProductionNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance146;
		((AppearanceBase)appearance148).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance148).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbProductionNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance148;
		((UltraGridBase)this.CmbProductionNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbProductionNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance149).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance149).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance149).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance149).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbProductionNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance149;
		((AppearanceBase)appearance150).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbProductionNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance150;
		((UltraGridBase)this.CmbProductionNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbProductionNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance151).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbProductionNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.CmbProductionNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbProductionNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance152;
		((UltraGridBase)this.CmbProductionNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbProductionNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbProductionNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbProductionNo).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbProductionNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbProductionNo).Location = new System.Drawing.Point(108, 45);
		((System.Windows.Forms.Control)(object)this.CmbProductionNo).Name = "CmbProductionNo";
		((System.Windows.Forms.Control)(object)this.CmbProductionNo).Size = new System.Drawing.Size(240, 26);
		((System.Windows.Forms.Control)(object)this.CmbProductionNo).TabIndex = 2;
		this.txtPlanDate.CustomFormat = "dd-MMM-yy";
		this.txtPlanDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPlanDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtPlanDate.Location = new System.Drawing.Point(212, 19);
		this.txtPlanDate.Name = "txtPlanDate";
		this.txtPlanDate.Size = new System.Drawing.Size(136, 23);
		this.txtPlanDate.TabIndex = 1;
		this.txtPlanCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPlanCode.Enabled = false;
		this.txtPlanCode.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPlanCode.Location = new System.Drawing.Point(108, 19);
		this.txtPlanCode.Name = "txtPlanCode";
		this.txtPlanCode.Size = new System.Drawing.Size(100, 23);
		this.txtPlanCode.TabIndex = 0;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(5, 23);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(100, 15);
		this.label7.TabIndex = 67;
		this.label7.Text = "Plan Code && Date";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(963, 72);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(64, 30);
		this.label4.TabIndex = 178;
		this.label4.Text = "Average\r\nFinish Rate";
		this.btnPlanTypeDefine.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPlanTypeDefine.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnPlanTypeDefine.Location = new System.Drawing.Point(596, 46);
		this.btnPlanTypeDefine.Name = "btnPlanTypeDefine";
		this.btnPlanTypeDefine.Size = new System.Drawing.Size(25, 25);
		this.btnPlanTypeDefine.TabIndex = 8;
		this.btnPlanTypeDefine.TabStop = false;
		this.btnPlanTypeDefine.UseVisualStyleBackColor = true;
		this.btnPlanTypeDefine.Click += new System.EventHandler(btnPlanTypeDefine_Click);
		this.txtAvgFinishGoodRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAvgFinishGoodRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAvgFinishGoodRate.Location = new System.Drawing.Point(1027, 76);
		this.txtAvgFinishGoodRate.Name = "txtAvgFinishGoodRate";
		this.txtAvgFinishGoodRate.Size = new System.Drawing.Size(130, 23);
		this.txtAvgFinishGoodRate.TabIndex = 15;
		this.txtAvgFinishGoodRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtAvgFinishGoodRate_KeyPress);
		this.btnProductionTypeDefine.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnProductionTypeDefine.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnProductionTypeDefine.Location = new System.Drawing.Point(596, 18);
		this.btnProductionTypeDefine.Name = "btnProductionTypeDefine";
		this.btnProductionTypeDefine.Size = new System.Drawing.Size(25, 25);
		this.btnProductionTypeDefine.TabIndex = 6;
		this.btnProductionTypeDefine.TabStop = false;
		this.btnProductionTypeDefine.UseVisualStyleBackColor = true;
		this.btnProductionTypeDefine.Click += new System.EventHandler(btnProductionTypeDefine_Click);
		this.txtOtherInstruction.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOtherInstruction.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOtherInstruction.Location = new System.Drawing.Point(107, 102);
		this.txtOtherInstruction.Name = "txtOtherInstruction";
		this.txtOtherInstruction.Size = new System.Drawing.Size(1050, 23);
		this.txtOtherInstruction.TabIndex = 4;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.Location = new System.Drawing.Point(354, 80);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(55, 15);
		this.label23.TabIndex = 176;
		this.label23.Text = "WIPItem";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(5, 106);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(98, 15);
		this.label11.TabIndex = 153;
		this.label11.Text = "Other Instruction";
		this.CmbWipItem.AllowNull = (DefaultableBoolean)1;
		this.CmbWipItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWipItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWipItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance41).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance41).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWipItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance42;
		((SpecialBoxBase)((UltraGridBase)this.CmbWipItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance43).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance43).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance46).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance47;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance48).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance49).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance49).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance49).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance49).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance74).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWipItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance75).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWipItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWipItem).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWipItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWipItem).Location = new System.Drawing.Point(421, 74);
		((System.Windows.Forms.Control)(object)this.CmbWipItem).Name = "CmbWipItem";
		((System.Windows.Forms.Control)(object)this.CmbWipItem).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.CmbWipItem).TabIndex = 9;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(5, 51);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(76, 15);
		this.label1.TabIndex = 73;
		this.label1.Text = "Production #";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(625, 80);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(40, 15);
		this.label10.TabIndex = 151;
		this.label10.Text = "Status";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(354, 51);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(58, 15);
		this.label2.TabIndex = 71;
		this.label2.Text = "Plan Type";
		this.txtProductionNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtProductionNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtProductionNo.Location = new System.Drawing.Point(108, 47);
		this.txtProductionNo.Name = "txtProductionNo";
		this.txtProductionNo.Size = new System.Drawing.Size(240, 23);
		this.txtProductionNo.TabIndex = 2;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(5, 80);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(57, 15);
		this.label3.TabIndex = 75;
		this.label3.Text = "Other Ref";
		this.CmbWorkInProcess.AllowNull = (DefaultableBoolean)1;
		this.CmbWorkInProcess.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWorkInProcess.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWorkInProcess.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWorkInProcess).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWorkInProcess).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWorkInProcess.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWorkInProcess).Location = new System.Drawing.Point(720, 45);
		((System.Windows.Forms.Control)(object)this.CmbWorkInProcess).Name = "CmbWorkInProcess";
		((System.Windows.Forms.Control)(object)this.CmbWorkInProcess).Size = new System.Drawing.Size(240, 26);
		((System.Windows.Forms.Control)(object)this.CmbWorkInProcess).TabIndex = 11;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(625, 51);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(93, 15);
		this.label15.TabIndex = 168;
		this.label15.Text = "Work In Process";
		this.cmbStatus.AllowNull = (DefaultableBoolean)1;
		this.cmbStatus.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbStatus.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbStatus.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance153).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Appearance = (AppearanceBase)(object)appearance153;
		((UltraGridBase)this.cmbStatus).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbStatus).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance154).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance154).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance154).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbStatus).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance154;
		((AppearanceBase)appearance155).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbStatus).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance155;
		((SpecialBoxBase)((UltraGridBase)this.cmbStatus).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance156).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance156).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance156).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbStatus).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance156;
		((UltraGridBase)this.cmbStatus).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbStatus).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance157).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance157;
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance159).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance160;
		((AppearanceBase)appearance161).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance161).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance161;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance162).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance162).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance162).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance162).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance162;
		((AppearanceBase)appearance163).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance163;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance164).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance165;
		((UltraGridBase)this.cmbStatus).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbStatus).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbStatus).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbStatus).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbStatus.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbStatus).Location = new System.Drawing.Point(720, 74);
		((System.Windows.Forms.Control)(object)this.cmbStatus).Name = "cmbStatus";
		((System.Windows.Forms.Control)(object)this.cmbStatus).Size = new System.Drawing.Size(240, 26);
		((System.Windows.Forms.Control)(object)this.cmbStatus).TabIndex = 12;
		this.txtOtherReference.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOtherReference.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOtherReference.Location = new System.Drawing.Point(108, 76);
		this.txtOtherReference.Name = "txtOtherReference";
		this.txtOtherReference.Size = new System.Drawing.Size(240, 23);
		this.txtOtherReference.TabIndex = 3;
		this.cmbProductionType.AllowNull = (DefaultableBoolean)1;
		this.cmbProductionType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbProductionType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbProductionType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance115).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance115).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbProductionType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance116).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance116;
		((SpecialBoxBase)((UltraGridBase)this.cmbProductionType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance117).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance117).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance117).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance118).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance118;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance119).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance121).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance121).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance122).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance122).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance122).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance122).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance122;
		((AppearanceBase)appearance123).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbProductionType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance124).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbProductionType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbProductionType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbProductionType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbProductionType).Location = new System.Drawing.Point(421, 17);
		((System.Windows.Forms.Control)(object)this.cmbProductionType).Name = "cmbProductionType";
		((System.Windows.Forms.Control)(object)this.cmbProductionType).Size = new System.Drawing.Size(174, 26);
		((System.Windows.Forms.Control)(object)this.cmbProductionType).TabIndex = 5;
		this.txtEndDate.CustomFormat = "dd-MMM-yy";
		this.txtEndDate.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtEndDate.Location = new System.Drawing.Point(1027, 40);
		this.txtEndDate.Name = "txtEndDate";
		this.txtEndDate.Size = new System.Drawing.Size(130, 22);
		this.txtEndDate.TabIndex = 14;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(354, 23);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(66, 30);
		this.label13.TabIndex = 157;
		this.label13.Text = "Production\r\nType";
		this.cmbPlanType.AllowNull = (DefaultableBoolean)1;
		this.cmbPlanType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPlanType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPlanType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance166).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance166).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance166;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance85).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance85).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance85).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPlanType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance86).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance86;
		((SpecialBoxBase)((UltraGridBase)this.cmbPlanType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance87).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance87).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance87).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance167).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance167;
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance168).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance3;
		((AppearanceBase)appearance4).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance4).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance5).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance5).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance5).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance5;
		((AppearanceBase)appearance6).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPlanType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPlanType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPlanType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbPlanType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPlanType).Location = new System.Drawing.Point(421, 45);
		((System.Windows.Forms.Control)(object)this.cmbPlanType).Name = "cmbPlanType";
		((System.Windows.Forms.Control)(object)this.cmbPlanType).Size = new System.Drawing.Size(174, 26);
		((System.Windows.Forms.Control)(object)this.cmbPlanType).TabIndex = 7;
		this.txtstartDate.CustomFormat = "dd-MMM-yy";
		this.txtstartDate.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtstartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtstartDate.Location = new System.Drawing.Point(1027, 12);
		this.txtstartDate.Name = "txtstartDate";
		this.txtstartDate.Size = new System.Drawing.Size(130, 22);
		this.txtstartDate.TabIndex = 13;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(963, 16);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(60, 15);
		this.label16.TabIndex = 159;
		this.label16.Text = "Start Date";
		this.CmbByProductionAc.AllowNull = (DefaultableBoolean)1;
		this.CmbByProductionAc.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbByProductionAc.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbByProductionAc.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance57).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbByProductionAc).DisplayLayout.Appearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.CmbByProductionAc).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbByProductionAc).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance92).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance92).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbByProductionAc).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance92;
		((AppearanceBase)appearance103).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbByProductionAc).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance103;
		((SpecialBoxBase)((UltraGridBase)this.CmbByProductionAc).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance88).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance88).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance88).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbByProductionAc).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.CmbByProductionAc).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbByProductionAc).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance89).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbByProductionAc).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance89;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbByProductionAc).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.CmbByProductionAc).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbByProductionAc).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbByProductionAc).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance101;
		((AppearanceBase)appearance102).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance102).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbByProductionAc).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.CmbByProductionAc).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbByProductionAc).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance104).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance104).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance104).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance104).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbByProductionAc).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance104;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbByProductionAc).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbByProductionAc).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbByProductionAc).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbByProductionAc).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbByProductionAc).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbByProductionAc).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbByProductionAc).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbByProductionAc).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbByProductionAc).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbByProductionAc).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbByProductionAc.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbByProductionAc).Location = new System.Drawing.Point(877, 213);
		((System.Windows.Forms.Control)(object)this.CmbByProductionAc).Name = "CmbByProductionAc";
		((System.Windows.Forms.Control)(object)this.CmbByProductionAc).Size = new System.Drawing.Size(181, 26);
		((System.Windows.Forms.Control)(object)this.CmbByProductionAc).TabIndex = 13;
		((System.Windows.Forms.Control)(object)this.CmbByProductionAc).Visible = false;
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(778, 219);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(93, 15);
		this.label21.TabIndex = 172;
		this.label21.Text = "ByProductionAc";
		this.label21.Visible = false;
		this.CmbFinishGoodsAc.AllowNull = (DefaultableBoolean)1;
		this.CmbFinishGoodsAc.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbFinishGoodsAc.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbFinishGoodsAc.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbFinishGoodsAc).DisplayLayout.Appearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.CmbFinishGoodsAc).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbFinishGoodsAc).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance60).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance60).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbFinishGoodsAc).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance60;
		((AppearanceBase)appearance61).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFinishGoodsAc).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance61;
		((SpecialBoxBase)((UltraGridBase)this.CmbFinishGoodsAc).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance62).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance62).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance62).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFinishGoodsAc).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.CmbFinishGoodsAc).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbFinishGoodsAc).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbFinishGoodsAc).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance63;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance65).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbFinishGoodsAc).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.CmbFinishGoodsAc).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbFinishGoodsAc).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFinishGoodsAc).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance67).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance67).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbFinishGoodsAc).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.CmbFinishGoodsAc).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbFinishGoodsAc).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance68).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance68).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFinishGoodsAc).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance68;
		((AppearanceBase)appearance50).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbFinishGoodsAc).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbFinishGoodsAc).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbFinishGoodsAc).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance51).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbFinishGoodsAc).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbFinishGoodsAc).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbFinishGoodsAc).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.CmbFinishGoodsAc).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbFinishGoodsAc).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbFinishGoodsAc).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbFinishGoodsAc).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbFinishGoodsAc.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbFinishGoodsAc).Location = new System.Drawing.Point(877, 245);
		((System.Windows.Forms.Control)(object)this.CmbFinishGoodsAc).Name = "CmbFinishGoodsAc";
		((System.Windows.Forms.Control)(object)this.CmbFinishGoodsAc).Size = new System.Drawing.Size(185, 26);
		((System.Windows.Forms.Control)(object)this.CmbFinishGoodsAc).TabIndex = 12;
		((System.Windows.Forms.Control)(object)this.CmbFinishGoodsAc).Visible = false;
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(778, 251);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(89, 15);
		this.label20.TabIndex = 170;
		this.label20.Text = "FinishGoodsAc";
		this.label20.Visible = false;
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(1213, 35);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(97, 16);
		this.label19.TabIndex = 165;
		this.label19.Text = "PlanTypeSrNo";
		this.label19.Visible = false;
		this.txtPlanTypeSrNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPlanTypeSrNo.Enabled = false;
		this.txtPlanTypeSrNo.Location = new System.Drawing.Point(1216, 7);
		this.txtPlanTypeSrNo.Name = "txtPlanTypeSrNo";
		this.txtPlanTypeSrNo.Size = new System.Drawing.Size(85, 23);
		this.txtPlanTypeSrNo.TabIndex = 164;
		this.txtPlanTypeSrNo.Visible = false;
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.label32);
		this.panel3.Controls.Add(this.toolStrip1);
		this.panel3.Controls.Add(this.chkPrint);
		this.panel3.Controls.Add(this.btnProductionPlant);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1292, 50);
		this.panel3.TabIndex = 0;
		this.label32.AutoSize = true;
		this.label32.BackColor = System.Drawing.Color.Teal;
		this.label32.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.ForeColor = System.Drawing.Color.White;
		this.label32.Location = new System.Drawing.Point(3, 29);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(231, 19);
		this.label32.TabIndex = 68;
		this.label32.Text = "Production Job Order          ";
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[8] { this.btnnew, this.btnRefresh, this.btnSaveAs, this.btnsave, this.btnUpdate, this.Print, this.btnattachment, this.btnGenerateProductionNo });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1292, 27);
		this.toolStrip1.TabIndex = 57;
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
		this.btnSaveAs.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSaveAs.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSaveAs.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSaveAs.Name = "btnSaveAs";
		this.btnSaveAs.Size = new System.Drawing.Size(80, 24);
		this.btnSaveAs.Text = "SaveAs";
		this.btnSaveAs.Visible = false;
		this.btnSaveAs.Click += new System.EventHandler(btnSaveAs_Click);
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
		this.Print.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.Print.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.Print.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.Print.Name = "Print";
		this.Print.Size = new System.Drawing.Size(92, 24);
		this.Print.Text = "620-&Print";
		this.Print.Click += new System.EventHandler(Print_Click);
		this.btnattachment.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnattachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnattachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnattachment.Name = "btnattachment";
		this.btnattachment.Size = new System.Drawing.Size(109, 24);
		this.btnattachment.Text = "&Attachment";
		this.btnattachment.Click += new System.EventHandler(btnattachment_Click);
		this.btnGenerateProductionNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnGenerateProductionNo.Image = Architecture.WinApp.Properties.Resources.QuickActionCardIcon;
		this.btnGenerateProductionNo.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnGenerateProductionNo.Name = "btnGenerateProductionNo";
		this.btnGenerateProductionNo.Size = new System.Drawing.Size(120, 24);
		this.btnGenerateProductionNo.Text = "&Generate Nos";
		this.btnGenerateProductionNo.Click += new System.EventHandler(btnGenerateProductionNo_Click);
		this.chkPrint.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.chkPrint.AutoSize = true;
		this.chkPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkPrint.ForeColor = System.Drawing.Color.White;
		this.chkPrint.Location = new System.Drawing.Point(1206, 29);
		this.chkPrint.Name = "chkPrint";
		this.chkPrint.Size = new System.Drawing.Size(84, 20);
		this.chkPrint.TabIndex = 13;
		this.chkPrint.Text = "Preview";
		this.chkPrint.UseVisualStyleBackColor = true;
		this.btnProductionPlant.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnProductionPlant.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnProductionPlant.Location = new System.Drawing.Point(781, 28);
		this.btnProductionPlant.Name = "btnProductionPlant";
		this.btnProductionPlant.Size = new System.Drawing.Size(23, 21);
		this.btnProductionPlant.TabIndex = 1;
		this.btnProductionPlant.TabStop = false;
		this.btnProductionPlant.UseVisualStyleBackColor = true;
		this.btnProductionPlant.Visible = false;
		this.btnProductionPlant.Click += new System.EventHandler(btnProductionPlant_Click);
		this.tabPage2.Controls.Add(this.panel12);
		this.tabPage2.Controls.Add(this.panel8);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1298, 680);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel12.Controls.Add(this.panel10);
		this.panel12.Controls.Add(this.panel9);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel12.Location = new System.Drawing.Point(3, 30);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1292, 647);
		this.panel12.TabIndex = 2;
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.panel10.Controls.Add(this.panel21);
		this.panel10.Controls.Add(this.panel20);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel10.Location = new System.Drawing.Point(0, 31);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1292, 616);
		this.panel10.TabIndex = 63;
		this.DataGridHistory.AllowEdit = (InheritableBoolean)2;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistory.DynamicFiltering = true;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(0, 92);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1292, 524);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 1;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		this.DataGridHistory.LinkClicked += new ColumnActionEventHandler(DataGridHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).DoubleClick += new System.EventHandler(DataGridHistory_DoubleClick);
		this.panel21.BackColor = System.Drawing.Color.Teal;
		this.panel21.Controls.Add(this.label18);
		this.panel21.Controls.Add(this.ctrlGrdBar3);
		this.panel21.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel21.Location = new System.Drawing.Point(0, 66);
		this.panel21.Name = "panel21";
		this.panel21.Size = new System.Drawing.Size(1292, 26);
		this.panel21.TabIndex = 63;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Tahoma", 11f, System.Drawing.FontStyle.Bold);
		this.label18.ForeColor = System.Drawing.Color.White;
		this.label18.Location = new System.Drawing.Point(5, 2);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(132, 18);
		this.label18.TabIndex = 69;
		this.label18.Text = "Filtered Records";
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(1243, 1);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(45, 23);
		this.ctrlGrdBar3.TabIndex = 70;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.panel20.BackColor = System.Drawing.Color.White;
		this.panel20.Controls.Add(this.rdapproveddate);
		this.panel20.Controls.Add(this.rdmodifydate);
		this.panel20.Controls.Add(this.drdocdate);
		this.panel20.Controls.Add(this.rdentrydate);
		this.panel20.Controls.Add(this.label30);
		this.panel20.Controls.Add(this.txtOrderNoTo);
		this.panel20.Controls.Add(this.txtOrderNoFrom);
		this.panel20.Controls.Add(this.label29);
		this.panel20.Controls.Add((System.Windows.Forms.Control)(object)this.cmbperemeter);
		this.panel20.Controls.Add(this.label28);
		this.panel20.Controls.Add(this.btnshowHistory);
		this.panel20.Controls.Add(this.ToDateHistory);
		this.panel20.Controls.Add(this.FromDateHistory);
		this.panel20.Controls.Add(this.label31);
		this.panel20.Controls.Add((System.Windows.Forms.Control)(object)this.xmbjoborderno);
		this.panel20.Controls.Add(this.label33);
		this.panel20.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel20.Location = new System.Drawing.Point(0, 0);
		this.panel20.Margin = new System.Windows.Forms.Padding(2);
		this.panel20.Name = "panel20";
		this.panel20.Size = new System.Drawing.Size(1292, 66);
		this.panel20.TabIndex = 0;
		this.rdapproveddate.AutoSize = true;
		this.rdapproveddate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdapproveddate.Location = new System.Drawing.Point(906, 39);
		this.rdapproveddate.Name = "rdapproveddate";
		this.rdapproveddate.Size = new System.Drawing.Size(105, 19);
		this.rdapproveddate.TabIndex = 10;
		this.rdapproveddate.Text = "Approved Date";
		this.rdapproveddate.UseVisualStyleBackColor = true;
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(906, 11);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 9;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(825, 11);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 7;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(825, 39);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 8;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(340, 41);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(45, 15);
		this.label30.TabIndex = 598;
		this.label30.Text = "Doc To";
		this.txtOrderNoTo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOrderNoTo.Location = new System.Drawing.Point(400, 37);
		this.txtOrderNoTo.Name = "txtOrderNoTo";
		this.txtOrderNoTo.ShortcutsEnabled = false;
		this.txtOrderNoTo.Size = new System.Drawing.Size(89, 23);
		this.txtOrderNoTo.TabIndex = 4;
		this.txtOrderNoTo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtOrderNoTo_KeyPress);
		this.txtOrderNoFrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOrderNoFrom.Location = new System.Drawing.Point(400, 9);
		this.txtOrderNoFrom.Name = "txtOrderNoFrom";
		this.txtOrderNoFrom.ShortcutsEnabled = false;
		this.txtOrderNoFrom.Size = new System.Drawing.Size(89, 23);
		this.txtOrderNoFrom.TabIndex = 3;
		this.txtOrderNoFrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtOrderNoFrom_KeyPress);
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.Location = new System.Drawing.Point(340, 13);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(60, 15);
		this.label29.TabIndex = 597;
		this.label29.Text = "Doc From";
		this.cmbperemeter.AllowNull = (DefaultableBoolean)1;
		this.cmbperemeter.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbperemeter.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbperemeter.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Appearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance28).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance28).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance28;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance29;
		((SpecialBoxBase)((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance30).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance30).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance35).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance36).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance36).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance36).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbperemeter.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Location = new System.Drawing.Point(97, 7);
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Name = "cmbperemeter";
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Size = new System.Drawing.Size(240, 26);
		((System.Windows.Forms.Control)(object)this.cmbperemeter).TabIndex = 0;
		this.cmbperemeter.ValueChanged += new System.EventHandler(cmbperemeter_ValueChanged);
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.Location = new System.Drawing.Point(4, 13);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(60, 15);
		this.label28.TabIndex = 594;
		this.label28.Text = "Date Type";
		this.btnshowHistory.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshowHistory.FlatAppearance.BorderSize = 0;
		this.btnshowHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshowHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshowHistory.ForeColor = System.Drawing.Color.White;
		this.btnshowHistory.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshowHistory.Location = new System.Drawing.Point(747, 35);
		this.btnshowHistory.Margin = new System.Windows.Forms.Padding(2);
		this.btnshowHistory.Name = "btnshowHistory";
		this.btnshowHistory.Size = new System.Drawing.Size(68, 26);
		this.btnshowHistory.TabIndex = 6;
		this.btnshowHistory.Text = "Show";
		this.btnshowHistory.UseVisualStyleBackColor = false;
		this.btnshowHistory.Click += new System.EventHandler(btnshowHistory_Click);
		this.ToDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(218, 37);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.Size = new System.Drawing.Size(119, 23);
		this.ToDateHistory.TabIndex = 2;
		this.FromDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(97, 37);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.Size = new System.Drawing.Size(119, 23);
		this.FromDateHistory.TabIndex = 1;
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.Location = new System.Drawing.Point(5, 41);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(91, 15);
		this.label31.TabIndex = 146;
		this.label31.Text = "Date From && To";
		this.xmbjoborderno.AllowNull = (DefaultableBoolean)1;
		this.xmbjoborderno.AutoCompleteMode = (AutoCompleteMode)4;
		this.xmbjoborderno.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.xmbjoborderno.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.xmbjoborderno).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.xmbjoborderno).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.xmbjoborderno).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.xmbjoborderno).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.xmbjoborderno).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance45;
		((SpecialBoxBase)((UltraGridBase)this.xmbjoborderno).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance53).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.xmbjoborderno).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.xmbjoborderno).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.xmbjoborderno).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.xmbjoborderno).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance54;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.xmbjoborderno).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.xmbjoborderno).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.xmbjoborderno).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.xmbjoborderno).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance58).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance58).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.xmbjoborderno).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.xmbjoborderno).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.xmbjoborderno).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance64).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance64).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance64).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance64).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.xmbjoborderno).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance64;
		((AppearanceBase)appearance72).TextHAlignAsString = "Left";
		((UltraGridBase)this.xmbjoborderno).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.xmbjoborderno).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.xmbjoborderno).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.xmbjoborderno).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.xmbjoborderno).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.xmbjoborderno).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.xmbjoborderno).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.xmbjoborderno).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.xmbjoborderno).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.xmbjoborderno).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.xmbjoborderno.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.xmbjoborderno).Location = new System.Drawing.Point(576, 7);
		((System.Windows.Forms.Control)(object)this.xmbjoborderno).Name = "xmbjoborderno";
		((System.Windows.Forms.Control)(object)this.xmbjoborderno).Size = new System.Drawing.Size(239, 26);
		((System.Windows.Forms.Control)(object)this.xmbjoborderno).TabIndex = 5;
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(490, 13);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(78, 15);
		this.label33.TabIndex = 144;
		this.label33.Text = "Job Order No";
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.label9);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(0, 0);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1292, 31);
		this.panel9.TabIndex = 62;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Tahoma", 12.75f, System.Drawing.FontStyle.Bold);
		this.label9.ForeColor = System.Drawing.Color.White;
		this.label9.Location = new System.Drawing.Point(5, 3);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(161, 21);
		this.label9.TabIndex = 69;
		this.label9.Text = "Job Order History";
		this.panel8.Controls.Add(this.toolStrip2);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(3, 3);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1292, 27);
		this.panel8.TabIndex = 0;
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnNewHistory, this.toolStripSeparator1, this.toolStripButton4 });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1292, 27);
		this.toolStrip2.TabIndex = 58;
		this.toolStrip2.Text = "toolStrip2";
		this.btnNewHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewHistory.Name = "btnNewHistory";
		this.btnNewHistory.Size = new System.Drawing.Size(59, 24);
		this.btnNewHistory.Text = "&New";
		this.btnNewHistory.Click += new System.EventHandler(btnNewHistory_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.toolStripButton4.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton4.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton4.Name = "toolStripButton4";
		this.toolStripButton4.Size = new System.Drawing.Size(61, 24);
		this.toolStripButton4.Text = "&Print";
		this.toolStripButton4.Visible = false;
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(625, 23);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(92, 15);
		this.label35.TabIndex = 168;
		this.label35.Text = "Wip Warehouse";
		this.CmbWipWarehouse.AllowNull = (DefaultableBoolean)1;
		this.CmbWipWarehouse.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWipWarehouse.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWipWarehouse.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWipWarehouse).DisplayLayout.Appearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.CmbWipWarehouse).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWipWarehouse).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance77).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance77).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance77).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWipWarehouse).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance77;
		((AppearanceBase)appearance78).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWipWarehouse).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance78;
		((SpecialBoxBase)((UltraGridBase)this.CmbWipWarehouse).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance79).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance79).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWipWarehouse).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.CmbWipWarehouse).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWipWarehouse).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance131).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWipWarehouse).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance131;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance132).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWipWarehouse).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.CmbWipWarehouse).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWipWarehouse).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWipWarehouse).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance133;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance134).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWipWarehouse).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance134;
		((UltraGridBase)this.CmbWipWarehouse).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWipWarehouse).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance135).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance135).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance135).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance135).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWipWarehouse).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance135;
		((AppearanceBase)appearance69).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWipWarehouse).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.CmbWipWarehouse).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWipWarehouse).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWipWarehouse).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.CmbWipWarehouse).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWipWarehouse).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbWipWarehouse).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWipWarehouse).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWipWarehouse).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWipWarehouse).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWipWarehouse.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWipWarehouse).Location = new System.Drawing.Point(720, 17);
		((System.Windows.Forms.Control)(object)this.CmbWipWarehouse).Name = "CmbWipWarehouse";
		((System.Windows.Forms.Control)(object)this.CmbWipWarehouse).Size = new System.Drawing.Size(240, 26);
		((System.Windows.Forms.Control)(object)this.CmbWipWarehouse).TabIndex = 10;
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(963, 45);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(55, 15);
		this.label36.TabIndex = 159;
		this.label36.Text = "End Date";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1306, 709);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmProductionJobOrderMain";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmProductionJobOrderMain_FormClosing);
		base.Load += new System.EventHandler(frmProductionJobOrder_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmProductionJobOrder_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.tabControl2.ResumeLayout(false);
		this.tabPage3.ResumeLayout(false);
		this.panel16.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdFinishGoods).EndInit();
		this.panel17.ResumeLayout(false);
		this.panel17.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbItemFinish).EndInit();
		this.panel18.ResumeLayout(false);
		this.panel18.PerformLayout();
		this.panel13.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdByProduct).EndInit();
		this.panel15.ResumeLayout(false);
		this.panel15.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbItemByProduct).EndInit();
		this.panel14.ResumeLayout(false);
		this.panel14.PerformLayout();
		this.panel6.ResumeLayout(false);
		this.panel19.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GridDetail).EndInit();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbPlantFeader).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.tabPage4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdScheduleInfo).EndInit();
		this.panel22.ResumeLayout(false);
		this.panel22.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbProductionNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWipItem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWorkInProcess).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbStatus).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbProductionType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPlanType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbByProductionAc).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbFinishGoodsAc).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel12.ResumeLayout(false);
		this.panel10.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel21.ResumeLayout(false);
		this.panel21.PerformLayout();
		this.panel20.ResumeLayout(false);
		this.panel20.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbperemeter).EndInit();
		((System.ComponentModel.ISupportInitialize)this.xmbjoborderno).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbWipWarehouse).EndInit();
		base.ResumeLayout(false);
	}
}
