using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Export;
using Architecture.BLL.Production;
using Architecture.BLL.Reports.Production;
using Architecture.BLL.SDT;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Export;
using Architecture.Model.Production;
using Architecture.Reports;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Inventory_Stocks_Report;
using Architecture.WinApp.Properties;
using Architecture.WinApp.SDT;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Export;

public class frmSaleContractSchedule : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private Rightsobjects formright = new Rightsobjects();

	private bool UpdateModeMain;

	private bool UpdateModeDetail;

	private bool ShipmentDocumentsFeature;

	private bool skipFCLContractValidationOnSchedule;

	private bool ExportSalesContractApprovalMandatory;

	private bool PackingMaterialPlanning_MRPFeature;

	private Attachment AT = new Attachment();

	private DataTable dtHistory = new DataTable();

	private DataTable dtitem = new DataTable();

	private DataTable table = new DataTable();

	private DataTable tableSheduleMain = new DataTable();

	private DataTable tablePendingContracts = new DataTable();

	private DataTable dtDepartmentDetailGrid = new DataTable();

	private List<ExImLcOrderShipmentSchedule> LstRemoveRecordMain = new List<ExImLcOrderShipmentSchedule>();

	private List<ExImLcOrderShipmentSchedulePackingDetail> LstRemoveRecordDetail = new List<ExImLcOrderShipmentSchedulePackingDetail>();

	private int updateDetailIndex = -1;

	private int CurrContractId;

	private int RecIdDepartmentDetail;

	private double MtonByAttentiveDate;

	private DateTime LastScheduleLoadingDate = DateTime.Now;

	private DataTable dtPort = new DataTable();

	private DataTable dtCustomGroupByParty = new DataTable();

	private string AttachmentsValues = "";

	private string CustomAttachmentsValues = "";

	public int ActionIdForContractGridBid = 1;

	private IContainer components;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private Panel panel2;

	private Panel panel6;

	private Panel panel5;

	private Panel panel3;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnsave;

	private GroupBox groupBox1;

	internal UltraCombo cmbItem;

	private Label label6;

	private Label label7;

	private Label label8;

	private TextBox txtNoOfBagsDetail;

	private Label label9;

	private TextBox txtMTonDetail;

	internal GridEX grd;

	private Panel panelMainhistory;

	private Button btnAdd;

	private Button btnCancel;

	private Button btnGridUpdate;

	internal UltraCombo cmbPackingtype;

	private Panel panel8;

	private Panel panel9;

	private Panel panel4;

	private Panel panel16;

	private Panel panel15;

	internal UltraCombo cmbCropYear;

	internal UltraCombo cmbPackUom;

	private Label label16;

	private Label label17;

	internal UltraCombo cmbAttentiveLoadDateBySchedule;

	private Label label15;

	private Panel panel12;

	internal GridEX grdContractShedule;

	private Panel panel13;

	private Label label14;

	private Panel panel10;

	private Panel panel11;

	private Label label13;

	private Panel panel14;

	private ToolStrip toolStrip2;

	private ToolStripButton BtnLoadAllPendingContracts;

	private Label label12;

	private CtrlGrdBar ctrlGrdBar1;

	private CtrlGrdBar ctrlGrdBar3;

	private Label label18;

	private ToolStrip toolStrip3;

	private ToolStripButton btnNewDetailSchedule;

	private ToolStripButton btnSavedetailSchedule;

	private RadioButton rdbtnItemCode;

	private RadioButton rdbtnItemName;

	private Panel panel7;

	private Panel panel19;

	private Panel panel21;

	private Panel panel20;

	private Panel panel17;

	internal GridEX grdHistoryMainByContract;

	private Panel panel18;

	internal GridEX grdHistoryScheduleDetailByContact;

	private Panel panel23;

	internal GridEX grdHistoryScheduleByContract;

	private Panel panel22;

	private Label label2;

	private ToolStrip ToolstripHistoryContract;

	private ToolStripButton LoadAllgrdContract;

	private Label label1;

	private Label label4;

	private Label label3;

	private CtrlGrdBar ctrlGrdBar6;

	private CtrlGrdBar ctrlGrdBar5;

	private CtrlGrdBar ctrlGrdBar4;

	private TextBox txtMtonOfSelectedDate;

	private ToolStripButton BtnShortCutkeys;

	private ToolStripButton btnattachment;

	private ToolStripButton BtnNewMain;

	private TabPage tabPage3;

	internal GridEX DataGridHistory;

	private Panel panel24;

	private CtrlGrdBar ctrlGrdBar7;

	private Label label5;

	private ToolStrip toolStrip4;

	private ToolStripButton btnLoadAllHistoryNew;

	private ToolStripButton BtnHistoryPrint;

	private Panel panel25;

	private GroupBox groupBox2;

	private Label label10;

	internal UltraCombo cmbPort;

	private DateTimePicker txtdateto;

	private Label label50;

	private Label label11;

	internal UltraCombo cmbSupplierName;

	private DateTimePicker txtdatefrom;

	private Label label19;

	private Label label51;

	internal UltraCombo CmbItemHistory;

	private Button btnShow;

	private Panel panel26;

	private Panel panel27;

	private ToolStripButton btnRefreshHistoryCombos;

	private ToolStripButton BtnContractWiseSlip;

	private ToolStripButton BtnPrint551_01;

	private TabControl tabControlMain;

	private TabPage tabPageMain;

	private TabPage tabPageDepartmentStatus;

	internal GridEX grdDepartmentDetail;

	private Panel panel1;

	private Panel panel28;

	private GroupBox groupBox3;

	internal UltraCombo CmbCustomerDepartmentDetail;

	internal UltraCombo CmbPortDepartmentDetail;

	private Button BtnCancelDepartmentDetail;

	private Button BtnUpdateDepartmentDetail;

	private Label label20;

	private TextBox txtItemMtonDepartmentDetail;

	private Label label21;

	private Label label22;

	private Label label23;

	private TextBox txtFclDepartmentDetail;

	private Label label24;

	internal UltraCombo CmbContractDepartmentDetail;

	private Label label25;

	internal UltraCombo CmbScheduleNoDepartmentDetail;

	private Label label26;

	private Panel panel29;

	private ToolStrip toolStrip5;

	private ToolStripButton BtnNewDepartmentDetail;

	private ToolStripButton BtnRefreshDepartmentDetail;

	private Label label27;

	private DateTimePicker txtLoadingDateDepartmentDetail;

	private DateTimePicker txtPMDateDepartmentDetail;

	private DateTimePicker txtInspectionDateDepartmentDetail;

	private DateTimePicker txtProductionDateDepartmentDetail;

	internal UltraCombo CmbScheduleLockStatusDepartmentDetail;

	internal UltraCombo CmbItemDepartmentDetail;

	private Label label32;

	private Label label31;

	private Label label30;

	private Label label33;

	private Label label28;

	private Label label29;

	private TextBox txtScheduleMtonDepartmentDetail;

	internal UltraCombo CmbPlantDepartmentDetail;

	private Label label34;

	private Label label35;

	private TextBox txtProductionDays;

	private Label label36;

	private TextBox txtInspectionDays;

	private Label label37;

	private TextBox txtPackingMaterialScheduleDays;

	private Label label38;

	private TextBox txtInnerQty;

	private ToolStripButton BtnContractPmDetailForm;

	private ToolStripButton BtnShipmentDocumentForm;

	private Button BtnShowbyDates;

	private TextBox tztAddDays;

	private DateTimePicker txtDateFromDepart;

	private Label label39;

	private DateTimePicker txtDatetoDepart;

	private Label label40;

	private Label label41;

	private ToolStripButton BtnRefreshScheduleDetail;

	private ToolStripButton BtnRefreshScheduleMain;

	public GridEX grdPendingContract;

	public RadioButton rdAll;

	public RadioButton rdReffered;

	public RadioButton rdNotReffered;

	private Label label42;

	private TextBox txtBalMton;

	public frmSaleContractSchedule(UserAccount userAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = userAccount;
	}

	private void frmSaleContractSchedule_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1450, 700);
			formright = CommonServices.SetRightsValueInRightsObject("FrmExportSalesContractSchedule");
			btnsave.Enabled = formright.DoHaveSaveRight;
			btnSavedetailSchedule.Enabled = formright.DoHaveSaveRight;
			BtnHistoryPrint.Enabled = formright.DoHavePrintRights;
			ctrlGrdBar1.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar1.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar1.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar1.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar1.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar1.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			ctrlGrdBar3.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar3.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar3.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar3.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar3.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar3.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			ctrlGrdBar4.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar4.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar4.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar4.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar4.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar4.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			ctrlGrdBar5.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar5.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar5.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar5.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar5.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar5.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			ctrlGrdBar6.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar6.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar6.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar6.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar6.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar6.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			tabControl1.TabPages.Remove(tabPage2);
			ShipmentDocumentsFeature = CommonServices.GetERPFeatureById(12);
			PackingMaterialPlanning_MRPFeature = CommonServices.GetERPFeatureById(23);
			BtnShipmentDocumentForm.Visible = ShipmentDocumentsFeature;
			BtnContractPmDetailForm.Visible = PackingMaterialPlanning_MRPFeature;
			skipFCLContractValidationOnSchedule = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("skipFCLContractValidationOnSchedule"));
			ExportSalesContractApprovalMandatory = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("ExportSalesContractApprovalMandatory"));
			dtFilldForContractSchedule();
			dtitem.Columns.Add("Id", typeof(int));
			dtitem.Columns.Add("ItemName", typeof(string));
			dtitem.Columns.Add("ContractDetailId", typeof(int));
			dtitem.Columns.Add("RatePrice", typeof(double));
			dtitem.Columns.Add("ItemCode", typeof(string));
			dtitem.Columns.Add("CropYearId", typeof(int));
			dtitem.Columns.Add("CropYear", typeof(string));
			dtitem.Columns.Add("PackTypeId", typeof(int));
			dtitem.Columns.Add("PackType", typeof(string));
			dtitem.Columns.Add("PackUomId", typeof(int));
			dtitem.Columns.Add("PackUom", typeof(string));
			dtitem.Columns.Add("OuterQty", typeof(double));
			dtitem.Columns.Add("M_Ton", typeof(double));
			dtitem.Columns.Add("IsApproved", typeof(bool));
			tablePendingContracts.Columns.Add("Id", typeof(int));
			tablePendingContracts.Columns.Add("ContractNo", typeof(string));
			tablePendingContracts.Columns.Add("ContractDate", typeof(DateTime));
			tablePendingContracts.Columns.Add("NoOfContainers", typeof(double));
			tablePendingContracts.Columns.Add("WeightM_Ton", typeof(double));
			tablePendingContracts.Columns.Add("SupplierCustomerId", typeof(int));
			tablePendingContracts.Columns.Add("CustomerName");
			tablePendingContracts.Columns.Add("DestinationPortId");
			tablePendingContracts.Columns.Add("StartShipmentDate", typeof(DateTime));
			tablePendingContracts.Columns.Add("LastShipmentDate", typeof(DateTime));
			tablePendingContracts.Columns.Add("IsApproved");
			tablePendingContracts.Columns.Add("ApprovedStatus");
			grdPendingContract.DataSource = tablePendingContracts;
			grdPendingContract.RetrieveStructure();
			grdPendingContractSetting();
			tableSheduleMain.Columns.Add("Id", typeof(int));
			tableSheduleMain.Columns.Add("ContractId", typeof(int));
			tableSheduleMain.Columns.Add("ScheduleNo", typeof(string));
			tableSheduleMain.Columns.Add("SupplierCustomerId", typeof(int));
			tableSheduleMain.Columns.Add("CustomerName", typeof(string));
			tableSheduleMain.Columns.Add("CustomerContractNo", typeof(string));
			tableSheduleMain.Columns.Add("NoOfContainers", typeof(double));
			tableSheduleMain.Columns.Add("WeightM_Ton", typeof(double));
			tableSheduleMain.Columns.Add("AttentiveLoadingDate", typeof(DateTime));
			tableSheduleMain.Columns.Add("DestinationPortId", typeof(int));
			tableSheduleMain.Columns.Add("AttentiveProductionDate", typeof(DateTime));
			tableSheduleMain.Columns.Add("AttentiveInspectionDate", typeof(DateTime));
			tableSheduleMain.Columns.Add("PackingMaterialDate", typeof(DateTime));
			tableSheduleMain.Columns.Add("CustomGroupId", typeof(int));
			tableSheduleMain.Columns.Add("Remarks", typeof(string));
			tableSheduleMain.Columns.Add("Status", typeof(string));
			tableSheduleMain.Columns.Add("ReferredStatus", typeof(string));
			tableSheduleMain.Columns.Add("NoOfAttachments", typeof(int));
			tableSheduleMain.Columns.Add("IsApproved", typeof(bool));
			grdContractShedule.DataSource = tableSheduleMain;
			grdContractShedule.RetrieveStructure();
			grdContractSheduleSettings();
			table.Columns.Add("Id", typeof(int));
			table.Columns.Add("ScheduleId", typeof(int));
			table.Columns.Add("ContractId", typeof(int));
			table.Columns.Add("ContractDetailId", typeof(int));
			table.Columns.Add("AttentiveLoadingDate", typeof(DateTime));
			table.Columns.Add("DestinationPort", typeof(string));
			table.Columns.Add("ItemId", typeof(int));
			table.Columns.Add("Item");
			table.Columns.Add("ItemCode");
			table.Columns.Add("CropYearId", typeof(int));
			table.Columns.Add("CropYear");
			table.Columns.Add("PackingTypeId", typeof(int));
			table.Columns.Add("PackingType");
			table.Columns.Add("NoOfBags", typeof(double));
			table.Columns.Add("PackUomId", typeof(int));
			table.Columns.Add("PackUom");
			table.Columns.Add("InnerQty", typeof(double));
			table.Columns.Add("WeightM_Ton", typeof(double));
			table.Columns.Add("Status", typeof(string));
			table.Columns.Add("ReferredStatus", typeof(string));
			grd.DataSource = table;
			grd.RetrieveStructure();
			GridDetailSetting();
			dtDepartmentDetailGrid.Columns.Add("Id", typeof(int));
			dtDepartmentDetailGrid.Columns.Add("ContractScheduleId", typeof(int));
			dtDepartmentDetailGrid.Columns.Add("ScheduleCode", typeof(string));
			dtDepartmentDetailGrid.Columns.Add("ExImLcOrderId", typeof(int));
			dtDepartmentDetailGrid.Columns.Add("ContractNo", typeof(string));
			dtDepartmentDetailGrid.Columns.Add("SupplierCustomerId", typeof(int));
			dtDepartmentDetailGrid.Columns.Add("CustomerName", typeof(string));
			dtDepartmentDetailGrid.Columns.Add("DestinationPortId", typeof(int));
			dtDepartmentDetailGrid.Columns.Add("DestinationPort", typeof(string));
			dtDepartmentDetailGrid.Columns.Add("FCL", typeof(decimal));
			dtDepartmentDetailGrid.Columns.Add("ScheduleMTon", typeof(decimal));
			dtDepartmentDetailGrid.Columns.Add("LoadingDate", typeof(DateTime));
			dtDepartmentDetailGrid.Columns.Add("ContractScheduleDetailItemId", typeof(int));
			dtDepartmentDetailGrid.Columns.Add("ItemCode", typeof(string));
			dtDepartmentDetailGrid.Columns.Add("ItemName", typeof(string));
			dtDepartmentDetailGrid.Columns.Add("ItemMTon", typeof(decimal));
			dtDepartmentDetailGrid.Columns.Add("ProductionDays", typeof(int));
			dtDepartmentDetailGrid.Columns.Add("ProductionSchedule", typeof(DateTime));
			dtDepartmentDetailGrid.Columns.Add("ProductionStart", typeof(DateTime));
			dtDepartmentDetailGrid.Columns.Add("JobOrderId", typeof(int));
			dtDepartmentDetailGrid.Columns.Add("InspectionSchedule", typeof(DateTime));
			dtDepartmentDetailGrid.Columns.Add("InspectionStart", typeof(DateTime));
			dtDepartmentDetailGrid.Columns.Add("PreShipmentInspectionId", typeof(int));
			dtDepartmentDetailGrid.Columns.Add("PackMaterialSchedule", typeof(DateTime));
			dtDepartmentDetailGrid.Columns.Add("PackMaterialStart", typeof(DateTime));
			dtDepartmentDetailGrid.Columns.Add("PmStartRefDocumentTypeId", typeof(int));
			dtDepartmentDetailGrid.Columns.Add("PmStartRefId", typeof(int));
			dtDepartmentDetailGrid.Columns.Add("PlantId", typeof(int));
			dtDepartmentDetailGrid.Columns.Add("PlantName", typeof(string));
			dtDepartmentDetailGrid.Columns.Add("ScheduleLockId", typeof(int));
			dtDepartmentDetailGrid.Columns.Add("ScheduleLockStatus", typeof(string));
			grdDepartmentDetail.DataSource = dtDepartmentDetailGrid;
			GridDepartmentdetailSetting();
			PlantFeader();
			ScheduleLockStatusDepartmentDetailFill();
			CropYearFill();
			CropYearFill();
			ItemPackingTypeFill();
			int Records = ((ActionIdForContractGridBid == 1) ? 50 : 0);
			GetpendingContractsforShipmentSchedule(Records, ActionIdForContractGridBid);
			ComboBindForHistory();
			((Control)(object)grdPendingContract).Focus();
			txtDateFromDepart.Value = DateTime.Now;
			LastScheduleLoadingDate = ExImLcOrderShipmentSchedule.GetLatestAttentiveLoadDate(UserAccount.OrganizationId, UserAccount.CompanyId);
			if (LastScheduleLoadingDate > DateTime.Now)
			{
				tztAddDays.Text = (DateTime.Now - LastScheduleLoadingDate).Days.ToString();
			}
			else
			{
				txtDateFromDepart.Value = LastScheduleLoadingDate;
			}
			txtDatetoDepart.Value = LastScheduleLoadingDate;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void AttentiveLoadDateFillByContractId(int Id, string DetailRowIds = null)
	{
		try
		{
			List<ExImLcOrderShipmentSchedulePackingDetail> lstRemoveRecordDetail = LstRemoveRecordDetail;
			if (lstRemoveRecordDetail != null && lstRemoveRecordDetail.Count > 0)
			{
				string ids = string.Join(",", LstRemoveRecordDetail.Select((ExImLcOrderShipmentSchedulePackingDetail r) => r.ExImLcOrderShipmentScheduleId));
				if (!string.IsNullOrWhiteSpace(ids))
				{
					DetailRowIds = DetailRowIds + "," + ids;
				}
			}
			int id = Conversion.ToInt(cmbAttentiveLoadDateBySchedule.Value);
			((Control)(object)cmbAttentiveLoadDateBySchedule).Text = string.Empty;
			((UltraGridBase)cmbAttentiveLoadDateBySchedule).DataSource = null;
			DataTable dt = ExImLcOrderShipmentSchedule.GetAttentiveLoadDateByLcOrderId(Id, DetailRowIds);
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			DataTable dt2 = new DataTable();
			dt2.Columns.Add("ScheduleId", typeof(int));
			dt2.Columns.Add("AttentiveLoadingDate", typeof(string));
			dt2.Columns.Add("ContractId", typeof(int));
			dt2.Columns.Add("DestinationPort", typeof(string));
			dt2.Columns.Add("ScheduleNo", typeof(string));
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				dt2.Rows.Add(Conversion.ToInt(dt.Rows[i]["ScheduleId"]), Conversion.ToDateTime(dt.Rows[i]["AttentiveLoadingDate"]).ToString("dd-MMM-yyy"), Conversion.ToInt(dt.Rows[i]["ContractId"]), Conversion.ToString(dt.Rows[i]["DestinationPort"]), Conversion.ToString(dt.Rows[i]["ScheduleCode"]));
			}
			if (dt2.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDL(dt2, cmbAttentiveLoadDateBySchedule, "ScheduleId", "AttentiveLoadingDate", "Loading Date", ZeroIndex: true);
			((UltraGridBase)cmbAttentiveLoadDateBySchedule).DisplayLayout.Bands[0].Columns[2].Hidden = true;
			((UltraGridBase)cmbAttentiveLoadDateBySchedule).DisplayLayout.Bands[0].Columns[1].Width = 150;
			((UltraGridBase)cmbAttentiveLoadDateBySchedule).DisplayLayout.Bands[0].Columns[4].Width = 200;
			if (id > 0)
			{
				if (dt2.AsEnumerable().Any((DataRow row) => row.Field<int>("ScheduleId") == id))
				{
					cmbAttentiveLoadDateBySchedule.Value = id;
				}
				else
				{
					((Control)(object)cmbAttentiveLoadDateBySchedule).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ItemNameFillByContractId(int Id)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
		try
		{
			((UltraGridBase)cmbItem).DataSource = null;
			((Control)(object)cmbItem).Text = "";
			dtitem.Rows.Clear();
			DataTable dt = ExImLcOrder.GetItemsbySalesContract(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Id = Id
			});
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtitem.Rows.Add(dt.Rows[i]["ItemId"], dt.Rows[i]["ItemName"], dt.Rows[i]["ContractDetailId"], dt.Rows[i]["RatePrice"], dt.Rows[i]["ItemCode"], dt.Rows[i]["CropYearId"], dt.Rows[i]["CropYear"], dt.Rows[i]["InvPackingMaterialTypeId"], dt.Rows[i]["PackType"], dt.Rows[i]["ItemUomId"], dt.Rows[i]["PackSize"], dt.Rows[i]["Qty"], dt.Rows[i]["MTon"], dt.Rows[i]["IsApproved"]);
				}
			}
			if (dtitem.Rows.Count > 0)
			{
				if (rdbtnItemName.Checked)
				{
					DDL.BindDDL(dtitem, cmbItem, "ContractDetailId", "ItemName", "Item Name", ZeroIndex: true);
				}
				else
				{
					DDL.BindDDL(dtitem, cmbItem, "ContractDetailId", "ItemCode", "Item Name", ZeroIndex: true);
				}
				((UltraGridBase)cmbItem).DisplayLayout.Bands[0].Columns[1].Width = 300;
				((UltraGridBase)cmbItem).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)cmbItem).DisplayLayout.Bands[0].Columns[5].Hidden = true;
				((UltraGridBase)cmbItem).DisplayLayout.Bands[0].Columns[7].Hidden = true;
				((UltraGridBase)cmbItem).DisplayLayout.Bands[0].Columns[9].Hidden = true;
				((UltraGridBase)cmbItem).DisplayLayout.Bands[0].Columns[13].Hidden = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void rdbtnItemName_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (dtitem.Rows.Count > 0)
			{
				int ItemId = 0;
				if (Conversion.ToInt(cmbItem.Value) > 0)
				{
					ItemId = Conversion.ToInt(cmbItem.Value);
				}
				if (rdbtnItemName.Checked)
				{
					DDL.BindDDL(dtitem, cmbItem, "ContractDetailId", "ItemName", "Item Name", ZeroIndex: true);
				}
				else
				{
					DDL.BindDDL(dtitem, cmbItem, "ContractDetailId", "ItemCode", "Item Name", ZeroIndex: true);
				}
				cmbItem.Value = ItemId;
				cmbItem.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void bindItemPackUom()
	{
		try
		{
			string Packuom = ((Control)(object)cmbPackUom).Text;
			((Control)(object)cmbPackUom).Text = string.Empty;
			if (Conversion.ToInt(cmbItem.Value) <= 0)
			{
				return;
			}
			DataTable dtUom = CommonServices.GetUomScheduleByItemId(Conversion.ToInt(((UltraDropDownBase)cmbItem).SelectedRow.Cells["Id"].Value));
			if (dtUom.Rows.Count > 0)
			{
				DDL.BindDDL(dtUom, cmbPackUom, "Id", "UOMCode", "PackUom", ZeroIndex: true);
				if (dtUom.AsEnumerable().Any((DataRow row) => row.Field<string>("UOMCode") == Packuom))
				{
					((Control)(object)cmbPackUom).Text = Packuom;
				}
				else
				{
					((Control)(object)cmbPackUom).Text = string.Empty;
				}
			}
			else
			{
				((Control)(object)cmbPackUom).Text = string.Empty;
				((UltraGridBase)cmbPackUom).DataSource = null;
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
			bindItemPackUom();
			BindOtherDetailAuto();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ItemPackingTypeFill()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		try
		{
			int id = Conversion.ToInt(cmbPackingtype.Value);
			DataTable lst = ExImPackMaterilaType.Getall(new ExImPackMaterilaType
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (lst.Rows.Count <= 0)
			{
				return;
			}
			DataTable dtpcktype = new DataTable();
			dtpcktype.Columns.Add("Id");
			dtpcktype.Columns.Add("Description");
			dtpcktype.Clear();
			for (int i = 0; i < lst.Rows.Count; i++)
			{
				dtpcktype.Rows.Add(lst.Rows[i]["Id"], lst.Rows[i]["Description"]);
			}
			if (dtpcktype.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDL(dtpcktype, cmbPackingtype, "Id", "Description", "Packing Type", ZeroIndex: true);
			if (id > 0)
			{
				if (dtpcktype.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
				{
					cmbPackingtype.Value = id;
				}
				else
				{
					((Control)(object)cmbPackingtype).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindOtherDetailAuto()
	{
		try
		{
			if (((UltraGridBase)cmbItem).ActiveRow == null || ((UltraGridBase)cmbPackUom).DataSource == null)
			{
				return;
			}
			cmbCropYear.Value = Conversion.ToInt(((UltraDropDownBase)cmbItem).SelectedRow.Cells["CropYearId"].Value);
			cmbPackingtype.Value = Conversion.ToInt(((UltraDropDownBase)cmbItem).SelectedRow.Cells["PackTypeId"].Value);
			cmbPackUom.Value = Conversion.ToInt(((UltraDropDownBase)cmbItem).SelectedRow.Cells["PackUomId"].Value);
			double PackEquivalent = ((((UltraGridBase)cmbPackUom).ActiveRow != null) ? Conversion.ToDouble(((UltraDropDownBase)cmbPackUom).SelectedRow.Cells[2].Value) : 0.0);
			double Mtons = Conversion.ToDouble((object)txtMtonOfSelectedDate.Text);
			double mtons_In_Grid_of_SameSchedule = 0.0;
			int ContractDetailId = Conversion.ToInt(((UltraDropDownBase)cmbItem).SelectedRow.Cells["ContractDetailId"].Value);
			double ContractDetail_MTon = Conversion.ToDouble(((UltraDropDownBase)cmbItem).SelectedRow.Cells["M_Ton"].Value);
			double ContractDetailWiseMTon = 0.0;
			if (grd.GetRows().Count() > 0)
			{
				GridEXRow[] dataRows = grd.GetDataRows();
				foreach (GridEXRow row in dataRows)
				{
					if (Conversion.ToInt(row.Cells["ScheduleId"].Value) == Conversion.ToInt(cmbAttentiveLoadDateBySchedule.Value))
					{
						mtons_In_Grid_of_SameSchedule += Conversion.ToDouble(row.Cells["WeightM_Ton"].Value);
					}
					int Cont_Detail_Id = Conversion.ToInt(row.Cells["ContractDetailId"].Value);
					if (ContractDetailId == Cont_Detail_Id)
					{
						ContractDetailWiseMTon += Conversion.ToDouble(row.Cells["WeightM_Ton"].Value);
					}
				}
			}
			Mtons -= mtons_In_Grid_of_SameSchedule;
			double noOfBages = Mtons * 1000.0 / PackEquivalent;
			txtNoOfBagsDetail.Text = noOfBages.ToString("#,##0.###");
			txtBalMton.Text = (ContractDetail_MTon - ContractDetailWiseMTon).ToString("#,##0.###");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void dtFilldForContractSchedule()
	{
		try
		{
			dtPort = CommonServices.GetSeaPortForComboServiceBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CropYearFill()
	{
		try
		{
			int id = Conversion.ToInt(cmbCropYear.Value);
			DataTable lst = CommonServices.CropYearGetAllService();
			if (lst.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDLNew(lst, cmbCropYear, "Id", "CropYear", "Crop Year", true);
			if (id > 0)
			{
				if (lst.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
				{
					cmbCropYear.Value = id;
				}
				else
				{
					((Control)(object)cmbCropYear).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ComboBindForHistory()
	{
		try
		{
			DataTable dtPort = new DataTable();
			dtPort.Columns.Add("Id", typeof(int));
			dtPort.Columns.Add("Name");
			DataTable dtCustomer = new DataTable();
			dtCustomer.Columns.Add("Id", typeof(int));
			dtCustomer.Columns.Add("Name");
			DataTable dtItem = new DataTable();
			dtItem.Columns.Add("Id", typeof(int));
			dtItem.Columns.Add("Name");
			DataTable dt = ExImLcOrderShipmentScheduleHeader.GetDataForDropDownFromShipmentSchedule(UserAccount.OrganizationId, UserAccount.CompanyId, (string)null);
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToString(dt.Rows[i]["Activity"]) == "DestinationPort")
					{
						dtPort.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
					else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "Customer")
					{
						dtCustomer.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
					else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "Item")
					{
						dtItem.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
				}
			}
			DDL.BindDDL(dtPort, cmbPort, "Id", "Name", "Port Name", ZeroIndex: false);
			DDL.BindDDL(dtCustomer, cmbSupplierName, "Id", "Name", "Customer Name", ZeroIndex: false);
			DDL.BindDDL(dtItem, CmbItemHistory, "Id", "Name", "Item Name", ZeroIndex: false);
			((UltraGridBase)cmbPort).DisplayLayout.Bands[0].Columns[1].Width = 250;
			((UltraGridBase)cmbSupplierName).DisplayLayout.Bands[0].Columns[1].Width = 250;
			((UltraGridBase)CmbItemHistory).DisplayLayout.Bands[0].Columns[1].Width = 250;
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
			FormReset();
			DetailFormReset();
			LstRemoveRecordDetail.Clear();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void FormReset()
	{
		try
		{
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			AttachmentsValues = "";
			CustomAttachmentsValues = "";
			btnsave.Visible = true;
			UpdateModeMain = false;
			tableSheduleMain.Clear();
			table.Clear();
			grdContractShedule.DataSource = null;
			LstRemoveRecordMain.Clear();
			AddRowInScheduleMainGrid();
			CurrContractId = 0;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void DetailFormReset()
	{
		try
		{
			updateDetailIndex = -1;
			((Control)(object)cmbAttentiveLoadDateBySchedule).Text = string.Empty;
			((Control)(object)cmbItem).Text = string.Empty;
			((Control)(object)cmbPackUom).Text = string.Empty;
			txtNoOfBagsDetail.Text = string.Empty;
			txtMTonDetail.Text = string.Empty;
			btnAdd.Visible = true;
			btnGridUpdate.Visible = false;
			btnCancel.Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BtnNewMain_Click(object sender, EventArgs e)
	{
		try
		{
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			AttachmentsValues = "";
			CustomAttachmentsValues = "";
			FormReset();
			DetailFormReset();
			LstRemoveRecordDetail.Clear();
			((UltraGridBase)cmbItem).DataSource = null;
			((UltraGridBase)cmbPackUom).DataSource = null;
			grdContractShedule.DataSource = null;
			grd.DataSource = null;
			int ActionId = 0;
			if (rdNotReffered.Checked)
			{
				ActionId = 1;
			}
			else if (rdReffered.Checked)
			{
				ActionId = 2;
			}
			else if (rdAll.Checked)
			{
				ActionId = 0;
			}
			GetpendingContractsforShipmentSchedule(50, ActionId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BtnRefreshScheduleMain_Click(object sender, EventArgs e)
	{
		try
		{
			skipFCLContractValidationOnSchedule = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("skipFCLContractValidationOnSchedule"));
			dtFilldForContractSchedule();
			ContractScheduleGridComboFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BtnRefreshScheduleDetail_Click(object sender, EventArgs e)
	{
		try
		{
			AttentiveLoadDateFillByContractId(CurrContractId);
			ItemNameFillByContractId(CurrContractId);
			CropYearFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private bool DetailFormvalidation()
	{
		if (((Control)(object)cmbAttentiveLoadDateBySchedule).Text == string.Empty || Conversion.ToInt(cmbAttentiveLoadDateBySchedule.Value) == 0)
		{
			MessageBox.Show("Attentive Loading Date is Required!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			cmbAttentiveLoadDateBySchedule.Focus();
			return false;
		}
		if (((Control)(object)cmbItem).Text == string.Empty || Conversion.ToInt(cmbItem.Value) == 0)
		{
			MessageBox.Show("Item is Required!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			cmbItem.Focus();
			return false;
		}
		if (ExportSalesContractApprovalMandatory && Conversion.ToInt(cmbItem.Value) > 0 && !Conversion.ToBool(((UltraDropDownBase)cmbItem).SelectedRow.Cells["IsApproved"].Value))
		{
			MessageBox.Show("Contract is not approved.Please Check!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			cmbAttentiveLoadDateBySchedule.Focus();
			return false;
		}
		if (((Control)(object)cmbCropYear).Text == string.Empty || Conversion.ToInt(cmbCropYear.Value) == 0)
		{
			MessageBox.Show("CropYear is Required!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			cmbCropYear.Focus();
			return false;
		}
		if (((Control)(object)cmbPackingtype).Text == string.Empty || Conversion.ToInt(cmbPackingtype.Value) == 0)
		{
			MessageBox.Show("Packing Type is Required!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			cmbPackingtype.Focus();
			return false;
		}
		if (txtNoOfBagsDetail.Text == string.Empty || Conversion.ToDouble((object)txtNoOfBagsDetail.Text) == 0.0)
		{
			MessageBox.Show("No Of Bags Feild is Required!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			txtNoOfBagsDetail.Focus();
			return false;
		}
		if (((Control)(object)cmbPackUom).Text == string.Empty || Conversion.ToInt(cmbPackUom.Value) == 0)
		{
			MessageBox.Show("PackUom is Required!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			cmbPackUom.Focus();
			return false;
		}
		if (txtMTonDetail.Text == string.Empty || Conversion.ToDouble((object)txtMTonDetail.Text) == 0.0)
		{
			MessageBox.Show("Weight/M.Ton Feild is Required!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			txtMTonDetail.Focus();
			return false;
		}
		return true;
	}

	private void BtnLoadAllPendingContracts_Click(object sender, EventArgs e)
	{
		try
		{
			if (tabControl1.SelectedIndex == 0)
			{
				int ActionId = 0;
				if (rdNotReffered.Checked)
				{
					ActionId = 1;
				}
				else if (rdReffered.Checked)
				{
					ActionId = 2;
				}
				else if (rdAll.Checked)
				{
					ActionId = 0;
				}
				GetpendingContractsforShipmentSchedule(0, ActionId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GetpendingContractsforShipmentSchedule(int NoOfRecords = 0, int ActionId = 0)
	{
		try
		{
			DataTable dtPendingShipments = ExImLcOrderShipmentSchedule.GridContactsExistInScheduleOrNot(UserAccount.OrganizationId, UserAccount.CompanyId, ActionId, NoOfRecords);
			if (dtPendingShipments.Rows.Count > 0)
			{
				tablePendingContracts.Clear();
				for (int i = 0; i < dtPendingShipments.Rows.Count; i++)
				{
					tablePendingContracts.Rows.Add(dtPendingShipments.Rows[i]["ExImLcOrderId"], dtPendingShipments.Rows[i]["LcOrderNo"], dtPendingShipments.Rows[i]["LcOrderDate"], dtPendingShipments.Rows[i]["NoOfContainers"], Conversion.ToDouble(dtPendingShipments.Rows[i]["WeightM_Ton"]), dtPendingShipments.Rows[i]["SupCustId"], dtPendingShipments.Rows[i]["Customer"], dtPendingShipments.Rows[i]["DestinationPortId"], Conversion.ToDateTime(dtPendingShipments.Rows[i]["ShipmentStartDate"]).ToString("dd-MMM-yyyy"), Conversion.ToDateTime(dtPendingShipments.Rows[i]["LastShipmentDate"]).ToString("dd-MMM-yyyy"), dtPendingShipments.Rows[i]["IsApproved"], dtPendingShipments.Rows[i]["ApprovedStatus"]);
				}
				grdPendingContract.DataSource = tablePendingContracts;
				grdPendingContract.RetrieveStructure();
				grdPendingContractSetting();
			}
			else
			{
				grdPendingContract.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdPendingContractSetting()
	{
		try
		{
			grdPendingContract.RootTable.Columns["Id"].Visible = false;
			grdPendingContract.RootTable.Columns["LastShipmentDate"].Visible = false;
			grdPendingContract.RootTable.Columns["SupplierCustomerId"].Visible = false;
			grdPendingContract.RootTable.Columns["StartShipmentDate"].Visible = false;
			grdPendingContract.RootTable.Columns["DestinationPortId"].Visible = false;
			grdPendingContract.RootTable.Columns["IsApproved"].Visible = false;
			grdPendingContract.RootTable.Columns["ApprovedStatus"].Visible = ExportSalesContractApprovalMandatory;
			grdPendingContract.RootTable.Columns["ContractNo"].ColumnType = (ColumnType)5;
			grdPendingContract.RootTable.Columns["ContractDate"].Caption = "Date";
			grdPendingContract.RootTable.Columns["NoOfContainers"].Caption = "Fcl";
			grdPendingContract.RootTable.Columns["WeightM_Ton"].Caption = "M.ton";
			grdPendingContract.RootTable.Columns["ContractDate"].FormatString = "dd-MMM-yyyy";
			grdPendingContract.RootTable.Columns["StartShipmentDate"].FormatString = "dd-MMM-yyyy";
			grdPendingContract.RootTable.Columns["LastShipmentDate"].FormatString = "dd-MMM-yyyy";
			grdPendingContract.RootTable.Columns["NoOfContainers"].AggregateFunction = (AggregateFunction)2;
			grdPendingContract.RootTable.Columns["NoOfContainers"].TextAlignment = (TextAlignment)3;
			grdPendingContract.RootTable.Columns["NoOfContainers"].FormatString = "#,##0.##";
			grdPendingContract.RootTable.Columns["NoOfContainers"].TotalFormatString = "#,##0.##";
			grdPendingContract.RootTable.Columns["WeightM_Ton"].AggregateFunction = (AggregateFunction)2;
			grdPendingContract.RootTable.Columns["WeightM_Ton"].TextAlignment = (TextAlignment)3;
			grdPendingContract.RootTable.Columns["WeightM_Ton"].FormatString = "#,##0.##";
			grdPendingContract.RootTable.Columns["WeightM_Ton"].TotalFormatString = "#,##0.##";
			CommonServices.GridAutoAdjustmentNew(grdPendingContract);
			grdPendingContract.RootTable.Columns["ContractNo"].Width = 100;
			grdPendingContract.RootTable.Columns["CustomerName"].Width = 115;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdPendingContract_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			UpdateModeDetail = false;
			UpdateModeMain = false;
			GridEXRow r = grdPendingContract.CurrentRow;
			if (e.Column.Key == "ContractNo")
			{
				if (ExportSalesContractApprovalMandatory && !Conversion.ToBool(r.Cells["IsApproved"].Value))
				{
					throw new Exception("Please select approved contract");
				}
				grdPendingContractByContractId();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public void grdPendingContractByContractId(int ContractId = 0)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = ((ContractId == 0) ? grdPendingContract.CurrentRow : GridEX_Helper.GetRowByColumnNameAndValue(grdPendingContract, "Id", ContractId));
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			if (grd.GetRows().Count() > 0 || grdContractShedule.GetRows().Count() > 0)
			{
				if (MessageBox.Show("Are you sure to Reload Contract?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				table.Rows.Clear();
				tableSheduleMain.Rows.Clear();
			}
			CurrContractId = Conversion.ToInt(r.Cells["Id"].Value);
			ReadbyMainScheduleByContractId(CurrContractId);
			CurrContractId = Conversion.ToInt(r.Cells["Id"].Value);
			ReadScheduleDetailByContactId(CurrContractId);
			if (!UpdateModeMain && r != null && (int)r.RowType == 0)
			{
				if (tableSheduleMain.Columns.Count == 0)
				{
					tableSheduleMain.Columns.Add("Id", typeof(int));
					tableSheduleMain.Columns.Add("ContractId", typeof(int));
					tableSheduleMain.Columns.Add("ScheduleNo", typeof(string));
					tableSheduleMain.Columns.Add("SupplierCustomerId", typeof(int));
					tableSheduleMain.Columns.Add("CustomerName", typeof(string));
					tableSheduleMain.Columns.Add("CustomerContractNo", typeof(string));
					tableSheduleMain.Columns.Add("NoOfContainers", typeof(double));
					tableSheduleMain.Columns.Add("WeightM_Ton", typeof(double));
					tableSheduleMain.Columns.Add("AttentiveLoadingDate", typeof(DateTime));
					tableSheduleMain.Columns.Add("DestinationPortId", typeof(int));
					tableSheduleMain.Columns.Add("AttentiveProductionDate", typeof(DateTime));
					tableSheduleMain.Columns.Add("AttentiveInspectionDate", typeof(DateTime));
					tableSheduleMain.Columns.Add("PackingMaterialDate", typeof(DateTime));
					tableSheduleMain.Columns.Add("CustomGroupId", typeof(int));
					tableSheduleMain.Columns.Add("Remarks", typeof(string));
					tableSheduleMain.Columns.Add("Status", typeof(string));
					tableSheduleMain.Columns.Add("ReferredStatus", typeof(string));
					tableSheduleMain.Columns.Add("IsApproved", typeof(bool));
				}
				tableSheduleMain.Clear();
				tableSheduleMain.Rows.Add(0, r.Cells["Id"].Value, r.Cells["ContractNo"].Value?.ToString() + "/" + 1, r.Cells["SupplierCustomerId"].Value, r.Cells["CustomerName"].Value, "", r.Cells["NoOfContainers"].Value, r.Cells["WeightM_Ton"].Value, r.Cells["StartShipmentDate"].Value, r.Cells["DestinationPortId"].Value, DateTime.Now, DateTime.Now, DateTime.Now, 0, "", "", "", 0, false);
				grdContractShedule.DataSource = tableSheduleMain;
				grdContractShedule.RetrieveStructure();
				grdContractSheduleSettings();
			}
			if (!UpdateModeDetail)
			{
				grd.DataSource = null;
			}
			AttentiveLoadDateFillByContractId(CurrContractId);
			ItemNameFillByContractId(CurrContractId);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
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
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ContractScheduleGridComboFill()
	{
		try
		{
			grdContractShedule.RootTable.Columns["DestinationPortId"].ColumnType = (ColumnType)0;
			grdContractShedule.RootTable.Columns["DestinationPortId"].HasValueList = true;
			grdContractShedule.RootTable.Columns["DestinationPortId"].LimitToList = true;
			grdContractShedule.RootTable.Columns["DestinationPortId"].EditType = (EditType)4;
			grdContractShedule.RootTable.Columns["DestinationPortId"].Caption = "Destination Port";
			grdContractShedule.RootTable.Columns["DestinationPortId"].Width = 120;
			grdContractShedule.RootTable.Columns["DestinationPortId"].ValueList.PopulateValueList((IEnumerable)dtPort.DefaultView, "Id", "PortName");
			grdContractShedule.RootTable.Columns["CustomGroupId"].Visible = ShipmentDocumentsFeature;
			if (ShipmentDocumentsFeature)
			{
				int ID = (from r in grdContractShedule.GetDataRows().AsEnumerable()
					where Conversion.ToInt(r.Cells["SupplierCustomerId"].Value) > 0
					select Conversion.ToInt(r.Cells["SupplierCustomerId"].Value)).FirstOrDefault();
				dtCustomGroupByParty = ClientCustomGroup.GetCustomGroupAllocatedToCustomerBySupplierCustomerId(UserAccount.OrganizationId, UserAccount.CompanyId, ID);
				grdContractShedule.RootTable.Columns["CustomGroupId"].ColumnType = (ColumnType)0;
				grdContractShedule.RootTable.Columns["CustomGroupId"].HasValueList = true;
				grdContractShedule.RootTable.Columns["CustomGroupId"].LimitToList = true;
				grdContractShedule.RootTable.Columns["CustomGroupId"].EditType = (EditType)4;
				grdContractShedule.RootTable.Columns["CustomGroupId"].Caption = "Document Custom Group";
				grdContractShedule.RootTable.Columns["CustomGroupId"].Width = 100;
				grdContractShedule.RootTable.Columns["CustomGroupId"].ValueList.PopulateValueList((IEnumerable)dtCustomGroupByParty.DefaultView, "CustomGroupId", "CustomGroup");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdContractSheduleSettings()
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ContractScheduleGridComboFill();
			grdContractShedule.RootTable.HeaderLines = 3;
			foreach (GridEXColumn item in (JanusCollectionBase)grdContractShedule.RootTable.Columns)
			{
				item.HeaderAlignment = (TextAlignment)2;
			}
			grdContractShedule.RootTable.Columns["Id"].Visible = false;
			grdContractShedule.RootTable.Columns["ContractId"].Visible = false;
			grdContractShedule.RootTable.Columns["SupplierCustomerId"].Visible = false;
			grdContractShedule.RootTable.Columns["IsApproved"].Visible = false;
			grdContractShedule.RootTable.Columns["ScheduleNo"].Width = 80;
			grdContractShedule.RootTable.Columns["CustomerName"].Width = 110;
			grdContractShedule.RootTable.Columns["CustomerContractNo"].Width = 100;
			grdContractShedule.RootTable.Columns["NoOfContainers"].Caption = "FCL";
			grdContractShedule.RootTable.Columns["NoOfContainers"].Width = 40;
			grdContractShedule.RootTable.Columns["WeightM_Ton"].Caption = "M_Ton";
			grdContractShedule.RootTable.Columns["WeightM_Ton"].Width = 55;
			grdContractShedule.RootTable.Columns["AttentiveLoadingDate"].Caption = "Loading Date";
			grdContractShedule.RootTable.Columns["AttentiveLoadingDate"].Width = 90;
			grdContractShedule.RootTable.Columns["AttentiveProductionDate"].Caption = "Production Date";
			grdContractShedule.RootTable.Columns["AttentiveProductionDate"].Width = 90;
			grdContractShedule.RootTable.Columns["AttentiveInspectionDate"].Caption = "Inspection Date";
			grdContractShedule.RootTable.Columns["AttentiveInspectionDate"].Width = 90;
			grdContractShedule.RootTable.Columns["PackingMaterialDate"].Caption = "Packing Material Date";
			grdContractShedule.RootTable.Columns["PackingMaterialDate"].Width = 90;
			grdContractShedule.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants + 50;
			grdContractShedule.RootTable.Columns["Status"].Width = Constants.InventoryConstants.StatusConstant + 15;
			grdContractShedule.RootTable.Columns["ReferredStatus"].Width = Constants.InventoryConstants.StatusConstant + 15;
			grdContractShedule.RootTable.Columns["NoOfAttachments"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants - 10;
			grdContractShedule.RootTable.Columns["AttentiveLoadingDate"].FormatString = "dd-MMM-yyyy";
			grdContractShedule.RootTable.Columns["AttentiveProductionDate"].FormatString = "dd-MMM-yyyy";
			grdContractShedule.RootTable.Columns["AttentiveInspectionDate"].FormatString = "dd-MMM-yyyy";
			grdContractShedule.RootTable.Columns["PackingMaterialDate"].FormatString = "dd-MMM-yyyy";
			grdContractShedule.RootTable.Columns["Id"].EditType = (EditType)0;
			grdContractShedule.RootTable.Columns["ContractId"].EditType = (EditType)0;
			grdContractShedule.RootTable.Columns["SupplierCustomerId"].EditType = (EditType)0;
			grdContractShedule.RootTable.Columns["CustomerName"].EditType = (EditType)0;
			grdContractShedule.RootTable.Columns["Status"].EditType = (EditType)0;
			grdContractShedule.RootTable.Columns["ReferredStatus"].EditType = (EditType)0;
			grdContractShedule.RootTable.Columns["NoOfAttachments"].EditType = (EditType)0;
			grdContractShedule.RootTable.Columns["IsApproved"].EditType = (EditType)0;
			grdContractShedule.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdContractShedule.RootTable.Columns["NoOfContainers"].AggregateFunction = (AggregateFunction)2;
			grdContractShedule.RootTable.Columns["NoOfContainers"].TextAlignment = (TextAlignment)3;
			grdContractShedule.RootTable.Columns["NoOfContainers"].FormatString = "#,##0.##";
			grdContractShedule.RootTable.Columns["NoOfContainers"].TotalFormatString = "#,##0.##";
			grdContractShedule.RootTable.Columns["WeightM_Ton"].AggregateFunction = (AggregateFunction)2;
			grdContractShedule.RootTable.Columns["WeightM_Ton"].TextAlignment = (TextAlignment)3;
			grdContractShedule.RootTable.Columns["WeightM_Ton"].FormatString = "#,##0.###";
			grdContractShedule.RootTable.Columns["WeightM_Ton"].TotalFormatString = "#,##0.###";
			grdContractShedule.RootTable.Columns.Add("Delete");
			grdContractShedule.RootTable.Columns["Delete"].Key = "Delete";
			grdContractShedule.RootTable.Columns["Delete"].Caption = "X";
			grdContractShedule.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdContractShedule.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdContractShedule.RootTable.Columns["Delete"].ButtonText = "X";
			grdContractShedule.RootTable.Columns["Delete"].Width = 20;
			grdContractShedule.RootTable.Columns["Delete"].Position = 0;
			grdContractShedule.RootTable.Columns.Add("Add");
			grdContractShedule.RootTable.Columns["Add"].Key = "Add";
			grdContractShedule.RootTable.Columns["Add"].Caption = "+";
			grdContractShedule.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdContractShedule.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdContractShedule.RootTable.Columns["Add"].ButtonText = "+";
			grdContractShedule.RootTable.Columns["Add"].Width = 20;
			grdContractShedule.RootTable.Columns["Add"].Position = 1;
			GridEX_Helper.AddButton(grdContractShedule, "AddAttachment", "Add Attachment", 105);
			GridEX_Helper.AddButton(grdContractShedule, "Save", "Document Custom Group Save", 110);
			grdContractShedule.FrozenColumns = 2;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdContractShedule_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0355: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Expected O, but got Unknown
		//IL_0339: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdContractShedule.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				bool IsApproved = Conversion.ToBool(r.Cells["IsApproved"].Value);
				if (e.Column.Key == "Delete")
				{
					if (tableSheduleMain.Rows.Count <= 1)
					{
						MessageBox.Show("You Can Not Delete All rows");
						return;
					}
					string status = Conversion.ToString(r.Cells["Status"].Value);
					if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
					{
						if (status != "Pending")
						{
							throw new Exception("You can't delete this row because its detail is " + status);
						}
						if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
						{
							return;
						}
						ExImLcOrderShipmentSchedule exIm = new ExImLcOrderShipmentSchedule();
						exIm.Id = Conversion.ToInt(r.Cells["Id"].Value);
						exIm.ExImLcOrderId = Conversion.ToInt(r.Cells["ContractId"].Value);
						exIm.SupplierCustomerId = Conversion.ToInt(r.Cells["SupplierCustomerId"].Value);
						exIm.ScheduleCode = Conversion.ToString(r.Cells["ScheduleNo"].Value);
						exIm.CustomerContractNo = Conversion.ToString(r.Cells["CustomerContractNo"].Value);
						exIm.NoOfContainer = Conversion.ToInt(r.Cells["NoOfContainers"].Value);
						exIm.NetWeight = Conversion.ToDecimal(r.Cells["WeightM_Ton"].Value) * 1000m;
						exIm.AttentiveLoadingDate = Conversion.ToDateTime(r.Cells["AttentiveLoadingDate"].Value);
						exIm.DestinationPortId = Conversion.ToInt(r.Cells["DestinationPortId"].Value);
						exIm.CustomGroupId = Conversion.ToInt(r.Cells["CustomGroupId"].Value);
						exIm.AttentiveProductionDate = Conversion.ToDateTime(r.Cells["AttentiveProductionDate"].Value);
						exIm.AttentiveInspectionDate = Conversion.ToDateTime(r.Cells["AttentiveInspectionDate"].Value);
						exIm.PackingMaterialDate = Conversion.ToDateTime(r.Cells["PackingMaterialDate"].Value);
						exIm.EntryDate = DateTime.Now;
						exIm.EntryUserId = UserAccount.ID;
						exIm.ModifyDate = DateTime.Now;
						exIm.ModifyUserId = UserAccount.ID;
						exIm.OrganizationId = UserAccount.OrganizationId;
						exIm.CompanyId = UserAccount.CompanyId;
						exIm.BranchesId = UserAccount.BranchesId;
						exIm.DocumentTypeId = 244;
						exIm.ActionId = 3;
						LstRemoveRecordMain.Add(exIm);
						tableSheduleMain.Rows[r.RowIndex].Delete();
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
				else if (e.Column.Key == "Add")
				{
					if (ExportSalesContractApprovalMandatory && !IsApproved)
					{
						throw new Exception("Can't break this record because contract is not approved!");
					}
					AddRowInScheduleMainGrid();
				}
				else if (e.Column.Key == "AddAttachment")
				{
					int Id = Conversion.ToInt(r.Cells["Id"].Value);
					if (Id <= 0)
					{
						MessageBox.Show("This record is not saved so can't add attachments");
						return;
					}
					AttachmentAddingFromHistory attachmentAddingFromHistory = new AttachmentAddingFromHistory(UserAccount, Id, 244, base.Name);
					attachmentAddingFromHistory.BringToFront();
					attachmentAddingFromHistory.ShowDialog();
				}
				else if (e.Column.Key == "Save")
				{
					int Id2 = Conversion.ToInt(r.Cells["Id"].Value);
					int CustomGroupId = Conversion.ToInt(r.Cells["CustomGroupId"].Value);
					if (Id2 <= 0)
					{
						MessageBox.Show("This record is not saved so can't save custom documents");
						return;
					}
					if (CustomGroupId <= 0)
					{
						MessageBox.Show("Custom Group field is required");
						return;
					}
					ExportContractSchedule.CustomGroupDocumentSave(244, Id2, CustomGroupId);
					MessageBox.Show("Documents save successfully");
				}
			}
			grdContractShedule.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void AddRowInScheduleMainGrid()
	{
		try
		{
			if (grdContractShedule.GetRows().Count() == 0)
			{
				tableSheduleMain.Rows.Add(0, 0, "", 0, "", "", 0, 0, DateTime.Now, 0, DateTime.Now, DateTime.Now, DateTime.Now, 0, "", "", "", 0, false);
				grdContractShedule.DataSource = tableSheduleMain;
				grdContractShedule.RetrieveStructure();
				grdContractSheduleSettings();
				return;
			}
			double TotalWeightM_Ton = 0.0;
			double TotalNoOfContainers = 0.0;
			int selectedContractId = Conversion.ToInt(grdContractShedule.CurrentRow.Cells["ContractId"].Value);
			string ContractScheduleNo = "";
			GridEXRow[] rows = grdPendingContract.GetRows();
			for (int i = 0; i < rows.Length; i++)
			{
				_ = rows[i];
				DataRow[] matchingRows = tablePendingContracts.Select("Id=" + selectedContractId);
				if (matchingRows.Length != 0)
				{
					TotalWeightM_Ton += Conversion.ToDouble(matchingRows[0]["WeightM_Ton"]);
					TotalNoOfContainers += Conversion.ToDouble(matchingRows[0]["NoOfContainers"]);
					ContractScheduleNo = Conversion.ToString(matchingRows[0]["ContractNo"]);
					break;
				}
			}
			double WeightM_Ton = 0.0;
			double NoOfContainers = 0.0;
			if (grdContractShedule.GetRows().Count() > 0)
			{
				rows = grdContractShedule.GetRows();
				foreach (GridEXRow item in rows)
				{
					NoOfContainers += Conversion.ToDouble(item.Cells["NoOfContainers"].Value);
					WeightM_Ton += Conversion.ToDouble(item.Cells["WeightM_Ton"].Value);
				}
			}
			double GrossWeightM_Ton = TotalWeightM_Ton - WeightM_Ton;
			double GrossNoOfContainers = TotalNoOfContainers - NoOfContainers;
			if (GrossWeightM_Ton > 0.0 || GrossNoOfContainers > 0.0)
			{
				if (GrossWeightM_Ton > 0.0)
				{
					string updatedString = ContractScheduleNo + "/" + (tableSheduleMain.Rows.Count + 1);
					tableSheduleMain.Rows.Add(0, grdContractShedule.CurrentRow.Cells["ContractId"].Value, updatedString, grdContractShedule.CurrentRow.Cells["SupplierCustomerId"].Value, grdContractShedule.CurrentRow.Cells["CustomerName"].Value, "", GrossNoOfContainers, GrossWeightM_Ton, grdContractShedule.CurrentRow.Cells["AttentiveLoadingDate"].Value, grdContractShedule.CurrentRow.Cells["DestinationPortId"].Value, grdContractShedule.CurrentRow.Cells["AttentiveProductionDate"].Value, grdContractShedule.CurrentRow.Cells["AttentiveInspectionDate"].Value, grdContractShedule.CurrentRow.Cells["PackingMaterialDate"].Value, grdContractShedule.CurrentRow.Cells["CustomGroupId"].Value, grdContractShedule.CurrentRow.Cells["Remarks"].Value, "", "", 0, false);
					grdContractShedule.DataSource = tableSheduleMain;
				}
				else
				{
					MessageBox.Show("Please Check Grid WeightM_Ton  and TotalWeightM_Ton of Contract ");
				}
			}
			else
			{
				MessageBox.Show("Please Check Grid WeightM_Ton  and TotalWeightM_Ton of Contract ");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdContractShedule_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdContractShedule.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				double TotalWeightM_Ton = 0.0;
				double TotalNoOfContainers = 0.0;
				int selectedContractId = Conversion.ToInt(grdContractShedule.CurrentRow.Cells["ContractId"].Value);
				GridEXRow[] rows = grdPendingContract.GetRows();
				for (int i = 0; i < rows.Length; i++)
				{
					_ = rows[i];
					DataRow[] matchingRows = tablePendingContracts.Select("Id=" + selectedContractId);
					if (matchingRows.Length != 0)
					{
						TotalWeightM_Ton += Conversion.ToDouble(matchingRows[0]["WeightM_Ton"]);
						TotalNoOfContainers += Conversion.ToDouble(matchingRows[0]["NoOfContainers"]);
						break;
					}
				}
				double AllWeightM_TonExceptSelected = (from GridEXRow row in grdContractShedule.GetRows()
					where row != item
					select (double)row.Cells["WeightM_Ton"].Value).Sum();
				double AllNoOfContainersExceptSelected = (from GridEXRow row in grdContractShedule.GetRows()
					where row != item
					select (double)row.Cells["NoOfContainers"].Value).Sum();
				double RegularTotalWeightM_Ton = TotalWeightM_Ton - AllWeightM_TonExceptSelected;
				double RegularTotalNoOfContainers = TotalNoOfContainers - AllNoOfContainersExceptSelected;
				if ((e.Column.Key == "WeightM_Ton" || e.Column.Key == "NoOfContainers") && Conversion.ToDouble(item.Cells["WeightM_Ton"].Value) > 0.0 && Conversion.ToDouble(item.Cells["NoOfContainers"].Value) > 0.0)
				{
					double NoOfContainers = Conversion.ToDouble(item.Cells["NoOfContainers"].Value);
					double WeightM_Ton = Conversion.ToDouble(item.Cells["WeightM_Ton"].Value);
					if (NoOfContainers > RegularTotalNoOfContainers && !skipFCLContractValidationOnSchedule)
					{
						MessageBox.Show("FCL can not be Greater than total FCL of this Contract..");
						item.Cells["NoOfContainers"].Value = Math.Round(RegularTotalNoOfContainers, 2);
					}
					else
					{
						item.Cells["NoOfContainers"].Value = Math.Round(NoOfContainers, 2);
					}
					if (WeightM_Ton > RegularTotalWeightM_Ton)
					{
						MessageBox.Show("WeightM_Ton can not be Greater than  total WeightM_Ton of This Contract..");
						item.Cells["WeightM_Ton"].Value = Math.Round(RegularTotalWeightM_Ton, 3);
					}
					else
					{
						item.Cells["WeightM_Ton"].Value = Math.Round(WeightM_Ton, 3);
					}
				}
				_ = e.Column.Key == "AttentiveLoadingDate";
			}
			grdContractShedule.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		try
		{
			InsertScheduleMain();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnUpdate_Click(object sender, EventArgs e)
	{
		try
		{
			btnsave_Click(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void InsertScheduleMain()
	{
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected O, but got Unknown
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Expected O, but got Unknown
		try
		{
			bool Update = false;
			GridEXRow[] rows = grdContractShedule.GetRows();
			for (int j = 0; j < rows.Length; j++)
			{
				if (Conversion.ToInt(rows[j].Cells["Id"].Value) > 0)
				{
					Update = true;
					break;
				}
			}
			ExImLcOrderShipmentSchedule obj = new ExImLcOrderShipmentSchedule();
			if (Update)
			{
				if (MessageBox.Show("Are you sure to Update Main Schedule?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
			}
			else if (MessageBox.Show("Are you sure to Save Main Schedule?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			obj.EntryDate = DateTime.Now;
			obj.EntryUserId = UserAccount.ID;
			obj.ModifyDate = DateTime.Now;
			obj.ModifyUserId = UserAccount.ID;
			obj.ExImLcOrderShipmentScheduleList = new List<ExImLcOrderShipmentSchedule>();
			if (LstRemoveRecordMain.Count > 0)
			{
				for (int i = 0; i < LstRemoveRecordMain.Count; i++)
				{
					obj.ExImLcOrderShipmentScheduleList.Add(LstRemoveRecordMain[i]);
				}
			}
			if (grdContractShedule.RowCount > 0)
			{
				double Weight_Mton_Detail = 0.0;
				double Weight_MTon_Contract = 0.0;
				rows = grdContractShedule.GetRows();
				foreach (GridEXRow r2 in rows)
				{
					Conversion.ToString(r2.Cells["ReferredStatus"].Value);
					if (Conversion.CheckDateTimeNull(Conversion.ToDateTime(r2.Cells["AttentiveLoadingDate"].Value)))
					{
						MessageBox.Show("AttentiveLoadingDate cannot be null in Row No:" + (r2.RowIndex + 1), "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						return;
					}
					ExImLcOrderShipmentSchedule exIm = new ExImLcOrderShipmentSchedule();
					exIm.Id = Conversion.ToInt(r2.Cells["Id"].Value);
					exIm.ExImLcOrderId = Conversion.ToInt(r2.Cells["ContractId"].Value);
					exIm.SupplierCustomerId = Conversion.ToInt(r2.Cells["SupplierCustomerId"].Value);
					if (Conversion.ToDouble(r2.Cells["NoOfContainers"].Value) == 0.0)
					{
						MessageBox.Show("FCL Required in Row No:" + (r2.RowIndex + 1), "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						return;
					}
					exIm.NoOfContainer = Conversion.ToInt(r2.Cells["NoOfContainers"].Value);
					if (Conversion.ToDecimal(r2.Cells["WeightM_Ton"].Value) == 0m)
					{
						MessageBox.Show("WeightM_Ton Required in Row No:" + (r2.RowIndex + 1), "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						return;
					}
					Weight_Mton_Detail += Conversion.ToDouble(r2.Cells["WeightM_Ton"].Value);
					exIm.NetWeight = Conversion.ToDecimal(r2.Cells["WeightM_Ton"].Value) * 1000m;
					exIm.AttentiveLoadingDate = Conversion.ToDateTime(r2.Cells["AttentiveLoadingDate"].Value);
					if (Conversion.ToInt(r2.Cells["DestinationPortId"].Value) == 0)
					{
						MessageBox.Show("DestinationPort Required in Row No:" + (r2.RowIndex + 1), "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						return;
					}
					exIm.DestinationPortId = Conversion.ToInt(r2.Cells["DestinationPortId"].Value);
					exIm.CustomGroupId = Conversion.ToInt(r2.Cells["CustomGroupId"].Value);
					exIm.AttentiveProductionDate = Conversion.ToDateTime(r2.Cells["AttentiveProductionDate"].Value);
					exIm.AttentiveInspectionDate = Conversion.ToDateTime(r2.Cells["AttentiveInspectionDate"].Value);
					exIm.PackingMaterialDate = Conversion.ToDateTime(r2.Cells["PackingMaterialDate"].Value);
					exIm.CustomerContractNo = Conversion.ToString(r2.Cells["CustomerContractNo"].Value);
					exIm.ScheduleRemarks = Conversion.ToString(r2.Cells["Remarks"].Value);
					exIm.ScheduleCode = Conversion.ToString(r2.Cells["ScheduleNo"].Value);
					if (exIm.ScheduleCode == "")
					{
						throw new Exception("Schedule No Required in Grid");
					}
					exIm.EntryDate = DateTime.Now;
					exIm.EntryUserId = UserAccount.ID;
					exIm.ModifyDate = DateTime.Now;
					exIm.ModifyUserId = UserAccount.ID;
					exIm.OrganizationId = UserAccount.OrganizationId;
					exIm.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
					exIm.CompanyId = UserAccount.CompanyId;
					exIm.BranchesId = UserAccount.BranchesId;
					exIm.DocumentTypeId = 244;
					exIm.ActionId = ((exIm.Id <= 0) ? 1 : 2);
					obj.ExImLcOrderShipmentScheduleList.Add(exIm);
				}
				Weight_MTon_Contract = tablePendingContracts.AsEnumerable().FirstOrDefault((DataRow r) => r.Field<int>("Id") == obj.ExImLcOrderShipmentScheduleList[0].ExImLcOrderId)?.Field<double>("WeightM_Ton") ?? 0.0;
				if (Weight_Mton_Detail > Weight_MTon_Contract)
				{
					MessageBox.Show("WeightM_Ton " + Weight_Mton_Detail + " can not be Greater than total WeightM_Ton:" + Weight_MTon_Contract + " of This Contract", "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					return;
				}
				obj.AttachmentsList = new List<DMSAttachments>();
				obj.DeleteAttachmentsList = new List<DMSAttachments>();
				FormHelper.UpdateAttachmentsForObject<ExImLcOrderShipmentSchedule>(obj, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
				obj.AttachmentsValues = AttachmentsValues;
				obj.CustomAttachmentsValues = CustomAttachmentsValues;
				ExImLcOrderShipmentSchedule.Save(obj);
				if (!Update)
				{
					MessageBox.Show("Main Schedule Save SuccessFully ");
					FormReset();
					return;
				}
				MessageBox.Show("Main Schedule Update SuccessFully");
				FormHelper.DeleteAllAttachmentsFromDb(AT, obj.ExImLcOrderShipmentScheduleList[0].ExImLcOrderId, obj.DocumentTypeId, base.Name);
				FormReset();
				ReadbyMainScheduleByContractId(Conversion.ToInt(grdPendingContract.CurrentRow.Cells["Id"].Value));
				ReadScheduleDetailByContactId(Conversion.ToInt(grdPendingContract.CurrentRow.Cells["Id"].Value));
				AttentiveLoadDateFillByContractId(Conversion.ToInt(grdPendingContract.CurrentRow.Cells["Id"].Value));
				ItemNameFillByContractId(Conversion.ToInt(grdPendingContract.CurrentRow.Cells["Id"].Value));
				CurrContractId = Conversion.ToInt(grdPendingContract.CurrentRow.Cells["Id"].Value);
			}
			else
			{
				MessageBox.Show("Schedule Grid have no row", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ReadbyMainScheduleByContractId(int Id)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		try
		{
			FormReset();
			CurrContractId = Id;
			ExImLcOrderShipmentSchedule obj = new ExImLcOrderShipmentSchedule();
			obj.ExImLcOrderShipmentScheduleList = ExImLcOrderShipmentSchedule.GetScheduleByContractId(Id);
			if (obj.ExImLcOrderShipmentScheduleList == null || obj.ExImLcOrderShipmentScheduleList.Count <= 0)
			{
				return;
			}
			UpdateModeMain = true;
			tableSheduleMain.Rows.Clear();
			AttachmentsValues = obj.ExImLcOrderShipmentScheduleList[0].AttachmentsValues;
			CustomAttachmentsValues = obj.ExImLcOrderShipmentScheduleList[0].CustomAttachmentsValues;
			for (int i = 0; i < obj.ExImLcOrderShipmentScheduleList.Count; i++)
			{
				DateTime PackingMaterialDate = DateTime.Now;
				if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime((object)obj.ExImLcOrderShipmentScheduleList[i].PackingMaterialDate)))
				{
					PackingMaterialDate = obj.ExImLcOrderShipmentScheduleList[i].PackingMaterialDate;
				}
				tableSheduleMain.Rows.Add(obj.ExImLcOrderShipmentScheduleList[i].Id, obj.ExImLcOrderShipmentScheduleList[i].ExImLcOrderId, obj.ExImLcOrderShipmentScheduleList[i].ScheduleCode, obj.ExImLcOrderShipmentScheduleList[i].SupplierCustomerId, obj.ExImLcOrderShipmentScheduleList[i].CustomerName, obj.ExImLcOrderShipmentScheduleList[i].CustomerContractNo, obj.ExImLcOrderShipmentScheduleList[i].NoOfContainer, obj.ExImLcOrderShipmentScheduleList[i].NetWeight / 1000m, obj.ExImLcOrderShipmentScheduleList[i].AttentiveLoadingDate, obj.ExImLcOrderShipmentScheduleList[i].DestinationPortId, obj.ExImLcOrderShipmentScheduleList[i].AttentiveProductionDate, obj.ExImLcOrderShipmentScheduleList[i].AttentiveInspectionDate, PackingMaterialDate, obj.ExImLcOrderShipmentScheduleList[i].CustomGroupId, obj.ExImLcOrderShipmentScheduleList[i].ScheduleRemarks, obj.ExImLcOrderShipmentScheduleList[i].Status, obj.ExImLcOrderShipmentScheduleList[i].RefferedStatus, obj.ExImLcOrderShipmentScheduleList[i].NoOfAttachments, obj.ExImLcOrderShipmentScheduleList[i].ContractApproveStatus);
			}
			grdContractShedule.DataSource = tableSheduleMain;
			grdContractShedule.RetrieveStructure();
			grdContractSheduleSettings();
			FormHelper.LoadAttachmentsForObject(AT, Id, base.Name);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GetTotalMTonOfDate(int ContractId, int ScheduleId)
	{
		try
		{
			double ScheduleMTon = 0.0;
			DataTable Data = ExImLcOrderShipmentSchedule.GetNoOfContainersByScheduleAndLcOrderId(ContractId, ScheduleId);
			if (Data.Rows.Count > 0)
			{
				ScheduleMTon = Conversion.ToDouble(Data.Rows[0]["NetWeight"]) / 1000.0;
				Conversion.ToDouble(Data.Rows[0]["NoOfContainer"]);
				txtMtonOfSelectedDate.Text = ScheduleMTon.ToString();
				MtonByAttentiveDate = ScheduleMTon;
			}
			else
			{
				txtMtonOfSelectedDate.Text = 0.ToString();
				MtonByAttentiveDate = 0.0;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnAdd_Click(object sender, EventArgs e)
	{
		try
		{
			if (!DetailFormvalidation())
			{
				return;
			}
			Conversion.ToDateTime((object)((Control)(object)cmbAttentiveLoadDateBySchedule).Text);
			string DateToCompare = ((Control)(object)cmbAttentiveLoadDateBySchedule).Text;
			string DestinationPortToCompare = Conversion.ToString(((UltraDropDownBase)cmbAttentiveLoadDateBySchedule).SelectedRow.Cells["DestinationPort"].Value);
			string ScheduleNo = Conversion.ToString(((UltraDropDownBase)cmbAttentiveLoadDateBySchedule).SelectedRow.Cells["ScheduleNo"].Value);
			string ConditionToCheck = DateToCompare + " " + DestinationPortToCompare + " " + ScheduleNo;
			int ContractDetailId = Conversion.ToInt(((UltraDropDownBase)cmbItem).SelectedRow.Cells["ContractDetailId"].Value);
			decimal ContractDetail_MTon = Conversion.ToDecimal(((UltraDropDownBase)cmbItem).SelectedRow.Cells["M_Ton"].Value);
			decimal ScheduleMTon = Conversion.ToDecimal((object)MtonByAttentiveDate);
			decimal grdWeightM_Ton = default(decimal);
			decimal ContractDetailWiseMTon = default(decimal);
			if (grd.GetDataRows().Length == 0)
			{
				if (Conversion.ToDecimal((object)txtMTonDetail.Text) > ScheduleMTon)
				{
					MessageBox.Show("M.Ton Can not be Greater than Total M.Ton of this Contract against Schedule " + ConditionToCheck + ". Please Check M.Ton!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				if (Conversion.ToDecimal((object)txtMTonDetail.Text) > ContractDetail_MTon)
				{
					MessageBox.Show("M.Ton Can not be Greater than Total M.Ton of this Contract against Contract Detail MTon's" + ContractDetail_MTon + ". Please Check M.Ton!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
			}
			else if (grd.GetDataRows().Length != 0)
			{
				GridEXRow[] dataRows = grd.GetDataRows();
				foreach (GridEXRow row in dataRows)
				{
					if (Conversion.ToString(row.Cells["DestinationPort"].Value).Equals(ConditionToCheck))
					{
						grdWeightM_Ton += Conversion.ToDecimal(row.Cells["WeightM_Ton"].Value);
					}
					int Cont_Detail_Id = Conversion.ToInt(row.Cells["ContractDetailId"].Value);
					if (ContractDetailId == Cont_Detail_Id)
					{
						ContractDetailWiseMTon += Conversion.ToDecimal(row.Cells["WeightM_Ton"].Value);
					}
				}
				if (Conversion.ToDecimal((object)txtMTonDetail.Text) + grdWeightM_Ton > ScheduleMTon)
				{
					MessageBox.Show("M.Ton Can not be Greater than Total M.Ton of this Contract against Date and Port " + ConditionToCheck + ".Please Check M.Ton!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				if (Conversion.ToDecimal((object)txtMTonDetail.Text) + ContractDetailWiseMTon > ContractDetail_MTon)
				{
					MessageBox.Show("M.Ton Can not be Greater than Total M.Ton of this Contract against Contract detail MTon's " + ContractDetail_MTon + ".Please Check M.Ton!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
			}
			int contractId = Conversion.ToInt(((UltraDropDownBase)cmbAttentiveLoadDateBySchedule).SelectedRow.Cells["ContractId"].Value);
			table.Rows.Add(Conversion.ToInt((object)0), Conversion.ToInt(((UltraDropDownBase)cmbAttentiveLoadDateBySchedule).SelectedRow.Cells["ScheduleId"].Value), Conversion.ToInt(((UltraDropDownBase)cmbAttentiveLoadDateBySchedule).SelectedRow.Cells["ContractId"].Value), Conversion.ToString(((UltraDropDownBase)cmbItem).SelectedRow.Cells["ContractDetailId"].Value), Conversion.ToDateTime(((UltraDropDownBase)cmbAttentiveLoadDateBySchedule).SelectedRow.Cells["AttentiveLoadingDate"].Value), ((UltraDropDownBase)cmbAttentiveLoadDateBySchedule).SelectedRow.Cells["AttentiveLoadingDate"].Value?.ToString() + " " + Conversion.ToString(((UltraDropDownBase)cmbAttentiveLoadDateBySchedule).SelectedRow.Cells["DestinationPort"].Value) + " " + Conversion.ToString(((UltraDropDownBase)cmbAttentiveLoadDateBySchedule).SelectedRow.Cells["ScheduleNo"].Value), Conversion.ToInt(((UltraDropDownBase)cmbItem).SelectedRow.Cells["Id"].Value), Conversion.ToString((object)((UltraDropDownBase)cmbItem).SelectedRow.Cells["ItemName"].Text), Conversion.ToString((object)((UltraDropDownBase)cmbItem).SelectedRow.Cells["ItemCode"].Text), Conversion.ToInt(cmbCropYear.Value), Conversion.ToString((object)((Control)(object)cmbCropYear).Text), Conversion.ToInt(cmbPackingtype.Value), Conversion.ToString((object)((Control)(object)cmbPackingtype).Text), Conversion.ToDouble((object)txtNoOfBagsDetail.Text), Conversion.ToInt(cmbPackUom.Value), Conversion.ToString((object)((Control)(object)cmbPackUom).Text), Conversion.ToDouble((object)txtInnerQty.Text), Conversion.ToDouble((object)txtMTonDetail.Text));
			grd.DataSource = table;
			grd.RetrieveStructure();
			GridDetailSetting();
			DetailFormReset();
			cmbAttentiveLoadDateBySchedule.Focus();
			AttentiveLoadDateFillByContractId(contractId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grd_DoubleClick(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grd.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0 && Conversion.ToString(item.Cells["ReferredStatus"].Value) != "Referred")
			{
				updateDetailIndex = item.RowIndex;
				AttentiveLoadDateFillByContractId(Conversion.ToInt(item.Cells["ContractId"].Value), Conversion.ToString(item.Cells["ScheduleId"].Value));
				cmbAttentiveLoadDateBySchedule.Value = item.Cells["ScheduleId"].Value.ToString();
				cmbItem.Value = Conversion.ToInt(item.Cells["ContractDetailId"].Value);
				bindItemPackUom();
				cmbCropYear.Value = item.Cells["CropYearId"].Text.ToString();
				cmbPackingtype.Value = item.Cells["PackingTypeId"].Text.ToString();
				txtNoOfBagsDetail.Text = item.Cells["NoOfBags"].Text.ToString();
				cmbPackUom.Value = item.Cells["PackUomId"].Text.ToString();
				txtInnerQty.Text = item.Cells["InnerQty"].Text.ToString();
				txtMTonDetail.Text = item.Cells["WeightM_Ton"].Text.ToString();
				btnGridUpdate.Visible = true;
				btnCancel.Visible = true;
				btnAdd.Visible = false;
				cmbAttentiveLoadDateBySchedule.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnGridUpdate_Click(object sender, EventArgs e)
	{
		try
		{
			if (!DetailFormvalidation())
			{
				return;
			}
			Conversion.ToDateTime((object)((Control)(object)cmbAttentiveLoadDateBySchedule).Text);
			string DateToCompare = ((Control)(object)cmbAttentiveLoadDateBySchedule).Text;
			string DestinationPortToCompare = Conversion.ToString(((UltraDropDownBase)cmbAttentiveLoadDateBySchedule).SelectedRow.Cells["DestinationPort"].Value);
			string ScheduleNo = Conversion.ToString(((UltraDropDownBase)cmbAttentiveLoadDateBySchedule).SelectedRow.Cells["ScheduleNo"].Value);
			string ConditionToCheck = DateToCompare + " " + DestinationPortToCompare + " " + ScheduleNo;
			double grdWeightM_Ton = 0.0;
			double ScheduleMTon = MtonByAttentiveDate;
			int ContractDetailId = Conversion.ToInt(((UltraDropDownBase)cmbItem).SelectedRow.Cells["ContractDetailId"].Value);
			double ContractDetail_MTon = Conversion.ToDouble(((UltraDropDownBase)cmbItem).SelectedRow.Cells["M_Ton"].Value);
			double ContractDetailWiseMTon = 0.0;
			if (grd.GetDataRows().Length == 0)
			{
				if (Conversion.ToDouble((object)txtMTonDetail.Text) > ScheduleMTon)
				{
					MessageBox.Show("M.Ton Can not be Greater than Total M.Ton of this Contract.Please Check M.Ton!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				if (Conversion.ToDouble((object)txtMTonDetail.Text) > ContractDetail_MTon)
				{
					MessageBox.Show("M.Ton Can not be Greater than Total M.Ton of this Contract against Contract Detail MTon's" + ContractDetail_MTon + ".Please Check M.Ton!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
			}
			else if (grd.GetDataRows().Length != 0)
			{
				GridEXRow[] dataRows = grd.GetDataRows();
				foreach (GridEXRow row in dataRows)
				{
					if (Conversion.ToString(row.Cells["DestinationPort"].Value).Equals(ConditionToCheck) && row.RowIndex != updateDetailIndex)
					{
						grdWeightM_Ton += Conversion.ToDouble(row.Cells["WeightM_Ton"].Value);
					}
					int Cont_Detail_Id = Conversion.ToInt(row.Cells["ContractDetailId"].Value);
					if (ContractDetailId == Cont_Detail_Id && row.RowIndex != updateDetailIndex)
					{
						ContractDetailWiseMTon += Conversion.ToDouble(row.Cells["WeightM_Ton"].Value);
					}
				}
				if (Conversion.ToDouble((object)txtMTonDetail.Text) + grdWeightM_Ton > ScheduleMTon)
				{
					MessageBox.Show("M.Ton Can not be Greater than Total M.Ton of this Contract.Please Check M.Ton!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				if (Conversion.ToDouble((object)txtMTonDetail.Text) + ContractDetailWiseMTon > ContractDetail_MTon)
				{
					MessageBox.Show("M.Ton Can not be Greater than Total M.Ton of this Contract against Contract detail MTon's " + ContractDetail_MTon + ".Please Check M.Ton!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
			}
			table.Rows[updateDetailIndex]["ScheduleId"] = Conversion.ToInt(((UltraDropDownBase)cmbAttentiveLoadDateBySchedule).SelectedRow.Cells["ScheduleId"].Value);
			table.Rows[updateDetailIndex]["ContractId"] = Conversion.ToInt(((UltraDropDownBase)cmbAttentiveLoadDateBySchedule).SelectedRow.Cells["ContractId"].Value);
			table.Rows[updateDetailIndex]["ContractDetailId"] = Conversion.ToInt(((UltraDropDownBase)cmbItem).SelectedRow.Cells["ContractDetailId"].Value);
			table.Rows[updateDetailIndex]["AttentiveLoadingDate"] = Conversion.ToDateTime(((UltraDropDownBase)cmbAttentiveLoadDateBySchedule).SelectedRow.Cells["AttentiveLoadingDate"].Value);
			table.Rows[updateDetailIndex]["DestinationPort"] = ((UltraDropDownBase)cmbAttentiveLoadDateBySchedule).SelectedRow.Cells["AttentiveLoadingDate"].Value?.ToString() + " " + Conversion.ToString(((UltraDropDownBase)cmbAttentiveLoadDateBySchedule).SelectedRow.Cells["DestinationPort"].Value) + " " + Conversion.ToString(((UltraDropDownBase)cmbAttentiveLoadDateBySchedule).SelectedRow.Cells["ScheduleNo"].Value);
			table.Rows[updateDetailIndex]["ItemId"] = Conversion.ToInt(((UltraDropDownBase)cmbItem).SelectedRow.Cells["Id"].Value);
			table.Rows[updateDetailIndex]["Item"] = Conversion.ToString((object)((UltraDropDownBase)cmbItem).SelectedRow.Cells["ItemName"].Text);
			table.Rows[updateDetailIndex]["ItemCode"] = Conversion.ToString((object)((UltraDropDownBase)cmbItem).SelectedRow.Cells["ItemCode"].Text);
			table.Rows[updateDetailIndex]["CropYearId"] = Conversion.ToInt(cmbCropYear.Value);
			table.Rows[updateDetailIndex]["CropYear"] = Conversion.ToString((object)((Control)(object)cmbCropYear).Text);
			table.Rows[updateDetailIndex]["PackingTypeId"] = Conversion.ToInt(cmbPackingtype.Value);
			table.Rows[updateDetailIndex]["PackingType"] = Conversion.ToString((object)((Control)(object)cmbPackingtype).Text);
			table.Rows[updateDetailIndex]["NoOfBags"] = Conversion.ToDouble((object)txtNoOfBagsDetail.Text);
			table.Rows[updateDetailIndex]["PackUomId"] = Conversion.ToInt(cmbPackUom.Value);
			table.Rows[updateDetailIndex]["PackUom"] = Conversion.ToString((object)((Control)(object)cmbPackUom).Text);
			table.Rows[updateDetailIndex]["InnerQty"] = Conversion.ToDouble((object)txtInnerQty.Text);
			table.Rows[updateDetailIndex]["WeightM_Ton"] = Conversion.ToDouble((object)txtMTonDetail.Text);
			int ContractId = Conversion.ToInt(((UltraDropDownBase)cmbAttentiveLoadDateBySchedule).SelectedRow.Cells["ContractId"].Value);
			DetailFormReset();
			cmbAttentiveLoadDateBySchedule.Focus();
			updateDetailIndex = -1;
			AttentiveLoadDateFillByContractId(ContractId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnCancel_Click(object sender, EventArgs e)
	{
		try
		{
			int ContractId = Conversion.ToInt(((UltraDropDownBase)cmbAttentiveLoadDateBySchedule).SelectedRow.Cells["ContractId"].Value);
			DetailFormReset();
			updateDetailIndex = -1;
			AttentiveLoadDateFillByContractId(ContractId);
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
			grd.RootTable.Columns["NoOfBags"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["NoOfBags"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["NoOfBags"].FormatString = "#,##0";
			grd.RootTable.Columns["NoOfBags"].TotalFormatString = "#,##0";
			grd.RootTable.Columns["InnerQty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["InnerQty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["InnerQty"].FormatString = "#,##0";
			grd.RootTable.Columns["InnerQty"].TotalFormatString = "#,##0";
			grd.RootTable.Columns["WeightM_Ton"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["WeightM_Ton"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["WeightM_Ton"].FormatString = "#,##0.###";
			grd.RootTable.Columns["WeightM_Ton"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["AttentiveLoadingDate"].Caption = "Loading Date";
			grd.RootTable.Columns["DestinationPort"].Caption = "Loading Date / Destination Port";
			grd.RootTable.Groups.Add("DestinationPort");
			grd.RootTable.Columns["ContractDetailId"].Visible = false;
			grd.RootTable.Columns["AttentiveLoadingDate"].Visible = false;
			grd.RootTable.Columns["DestinationPort"].Visible = false;
			grd.RootTable.Columns["AttentiveLoadingDate"].FormatString = "dd-MMM-yyyy";
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["ScheduleId"].Visible = false;
			grd.RootTable.Columns["ContractId"].Visible = false;
			grd.RootTable.Columns["ItemId"].Visible = false;
			grd.RootTable.Columns["PackingTypeId"].Visible = false;
			grd.RootTable.Columns["PackUomId"].Visible = false;
			grd.RootTable.Columns["CropYearId"].Visible = false;
			CommonServices.GridAutoAdjustmentNew(grd);
			grd.RootTable.Columns.Add("Delete");
			grd.RootTable.Columns["Delete"].Key = "Delete";
			grd.RootTable.Columns["Delete"].Caption = "X";
			grd.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Delete"].ButtonText = "X";
			grd.RootTable.Columns["Delete"].Width = 20;
			grd.RootTable.Columns["Delete"].Position = 0;
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				if (e.Column.Key == "Delete")
				{
					DeleteDetailrow(r);
				}
				grd.UpdateData();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void DeleteDetailrow(GridEXRow r)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Expected O, but got Unknown
		try
		{
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			if (updateDetailIndex != -1)
			{
				throw new Exception("You can't delete this detail Record because its in update mode");
			}
			if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
			{
				if (Conversion.ToString(r.Cells["ReferredStatus"].Value) == "Referred")
				{
					throw new Exception("You can't delete this row because it is referred on invoice");
				}
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				ExImLcOrderShipmentSchedulePackingDetail exIm = new ExImLcOrderShipmentSchedulePackingDetail();
				exIm.Id = Conversion.ToInt(r.Cells["Id"].Value);
				exIm.ExImLcOrderId = Conversion.ToInt(r.Cells["ContractId"].Value);
				exIm.ExImLcOrderShipmentScheduleId = Conversion.ToInt(r.Cells["ScheduleId"].Value);
				exIm.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
				exIm.CropYearId = Conversion.ToInt(r.Cells["CropYearId"].Value);
				exIm.PackingTypeId = Conversion.ToInt(r.Cells["PackingTypeId"].Value);
				exIm.NoOfBags = Conversion.ToInt(r.Cells["NoOfBags"].Value);
				exIm.PackUomId = Conversion.ToInt(r.Cells["PackUomId"].Value);
				exIm.NetWeight = Conversion.ToDecimal(r.Cells["WeightM_Ton"].Value) * 1000m;
				exIm.InnerQty = Conversion.ToDouble(r.Cells["InnerQty"].Value);
				exIm.ActionTypeId = 3;
				LstRemoveRecordDetail.Add(exIm);
				table.Rows[r.RowIndex].Delete();
				r.Delete();
			}
			else
			{
				r.Delete();
			}
			AttentiveLoadDateFillByContractId(CurrContractId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void cmbAttentiveLoadDateBySchedule_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)cmbAttentiveLoadDateBySchedule).ActiveRow != null && Conversion.ToInt(cmbAttentiveLoadDateBySchedule.Value) > 0)
			{
				GetTotalMTonOfDate(Conversion.ToInt(((UltraDropDownBase)cmbAttentiveLoadDateBySchedule).SelectedRow.Cells["ContractId"].Value), Conversion.ToInt(((UltraDropDownBase)cmbAttentiveLoadDateBySchedule).SelectedRow.Cells["ScheduleId"].Value));
				GetMtonsFromDetail();
			}
			else
			{
				txtMtonOfSelectedDate.Text = 0.ToString();
				MtonByAttentiveDate = 0.0;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GetMtonsFromDetail()
	{
		try
		{
			double MtonOfDate = Conversion.ToDouble((object)txtMtonOfSelectedDate.Text);
			if (grd.GetDataRows().Count() <= 0 || !(MtonOfDate > 0.0))
			{
				return;
			}
			double Mton = 0.0;
			string targetDate = ((Control)(object)cmbAttentiveLoadDateBySchedule).Text;
			string destinationPort = Conversion.ToString(((UltraDropDownBase)cmbAttentiveLoadDateBySchedule).SelectedRow.Cells["DestinationPort"].Value);
			if (updateDetailIndex == -1)
			{
				GridEXRow[] dataRows = grd.GetDataRows();
				foreach (GridEXRow row in dataRows)
				{
					string obj = Conversion.ToString(row.Cells["DestinationPort"].Value);
					string TargetDestinationPort = targetDate + " " + destinationPort;
					if (obj.Equals(TargetDestinationPort))
					{
						Mton += Conversion.ToDouble(row.Cells["WeightM_Ton"].Value);
					}
				}
			}
			else
			{
				GridEXRow[] dataRows = grd.GetDataRows();
				foreach (GridEXRow row2 in dataRows)
				{
					if (Conversion.ToString(row2.Cells["DestinationPort"].Value).Equals(targetDate + " " + destinationPort) && row2.RowIndex != updateDetailIndex)
					{
						Mton += Conversion.ToDouble(row2.Cells["WeightM_Ton"].Value);
					}
				}
			}
			if (Mton > 0.0)
			{
				txtMtonOfSelectedDate.Text = (MtonOfDate - Mton).ToString("#,##0.###");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CalculateMtonDetail()
	{
		try
		{
			if (((UltraGridBase)cmbPackUom).ActiveRow != null && Conversion.ToInt(cmbPackUom.Value) != 0)
			{
				double NoOfbags = Conversion.ToDouble((object)txtNoOfBagsDetail.Text);
				double Equivalent = Conversion.ToDouble(((UltraDropDownBase)cmbPackUom).SelectedRow.Cells[2].Value);
				double QtyEquivalent = Conversion.ToDouble(((UltraDropDownBase)cmbPackUom).SelectedRow.Cells[3].Value);
				double weightMton = Conversion.ToDouble((object)txtMTonDetail.Text);
				if (base.ActiveControl != null && Conversion.ToString(base.ActiveControl.Tag) == "NoOfBags")
				{
					txtMTonDetail.Text = (NoOfbags * Equivalent / 1000.0).ToString("#,##0.###");
				}
				else if (base.ActiveControl != null && Conversion.ToString(base.ActiveControl.Tag) == "WeightMton")
				{
					txtNoOfBagsDetail.Text = (weightMton * 1000.0 / Equivalent).ToString("#,##0");
				}
				else
				{
					txtMTonDetail.Text = (NoOfbags * Equivalent / 1000.0).ToString("#,##0.###");
				}
				txtInnerQty.Text = (NoOfbags * QtyEquivalent).ToString("#,##0.###");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnAdd_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl != null && base.ActiveControl.Name == "rdbtnItemName")
			{
				cmbAttentiveLoadDateBySchedule.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InsertScheduleDetail()
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Expected O, but got Unknown
		try
		{
			bool Update = false;
			GridEXRow[] dataRows = grd.GetDataRows();
			for (int j = 0; j < dataRows.Length; j++)
			{
				if (Conversion.ToInt(dataRows[j].Cells["Id"].Value) > 0)
				{
					Update = true;
					break;
				}
			}
			ExImLcOrderShipmentSchedulePackingDetail obj = new ExImLcOrderShipmentSchedulePackingDetail();
			if (Update)
			{
				if (MessageBox.Show("Are you sure to Update Detail Schedule?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
			}
			else if (MessageBox.Show("Are you sure to Save Detail Schedule?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			obj.ExImLcOrderShipmentSchedulePackingDetailList = new List<ExImLcOrderShipmentSchedulePackingDetail>();
			if (LstRemoveRecordDetail.Count > 0)
			{
				for (int i = 0; i < LstRemoveRecordDetail.Count; i++)
				{
					obj.ExImLcOrderShipmentSchedulePackingDetailList.Add(LstRemoveRecordDetail[i]);
				}
			}
			if (grd.RowCount > 0)
			{
				dataRows = grd.GetDataRows();
				foreach (GridEXRow r in dataRows)
				{
					if (Conversion.ToString(r.Cells["ReferredStatus"].Value) != "Referred")
					{
						ExImLcOrderShipmentSchedulePackingDetail exIm = new ExImLcOrderShipmentSchedulePackingDetail();
						exIm.Id = Conversion.ToInt(r.Cells["Id"].Value);
						exIm.ExImLcOrderId = Conversion.ToInt(r.Cells["ContractId"].Value);
						exIm.ContractDetailId = Conversion.ToInt(r.Cells["ContractDetailId"].Value);
						exIm.ExImLcOrderShipmentScheduleId = Conversion.ToInt(r.Cells["ScheduleId"].Value);
						if (Conversion.ToInt(r.Cells["ItemId"].Value) == 0)
						{
							MessageBox.Show("Item Required in Row No:" + (r.RowIndex + 1), "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							return;
						}
						exIm.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
						if (Conversion.ToInt(r.Cells["CropYearId"].Value) == 0)
						{
							MessageBox.Show("CropYear Required in Row No:" + (r.RowIndex + 1), "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							return;
						}
						exIm.CropYearId = Conversion.ToInt(r.Cells["CropYearId"].Value);
						if (Conversion.ToInt(r.Cells["PackingTypeId"].Value) == 0)
						{
							MessageBox.Show("PackingType Required in Row No:" + (r.RowIndex + 1), "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							return;
						}
						exIm.PackingTypeId = Conversion.ToInt(r.Cells["PackingTypeId"].Value);
						if (Conversion.ToInt(r.Cells["NoOfBags"].Value) == 0)
						{
							MessageBox.Show("NoOfBags Required in Row No:" + (r.RowIndex + 1), "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							return;
						}
						exIm.NoOfBags = Conversion.ToInt(r.Cells["NoOfBags"].Value);
						if (Conversion.ToInt(r.Cells["PackUomId"].Value) == 0)
						{
							MessageBox.Show("PackUom Required in Row No:" + (r.RowIndex + 1), "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							return;
						}
						exIm.PackUomId = Conversion.ToInt(r.Cells["PackUomId"].Value);
						if (Conversion.ToDecimal(r.Cells["WeightM_Ton"].Value) == 0m)
						{
							MessageBox.Show("WeightM_Ton Required in Row No:" + (r.RowIndex + 1), "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							return;
						}
						exIm.NetWeight = Conversion.ToDecimal(r.Cells["WeightM_Ton"].Value) * 1000m;
						exIm.InnerQty = Conversion.ToDouble(r.Cells["InnerQty"].Value);
						exIm.ActionTypeId = ((exIm.Id <= 0) ? 1 : 2);
						obj.ExImLcOrderShipmentSchedulePackingDetailList.Add(exIm);
					}
				}
				ExImLcOrderShipmentSchedule.SaveDetail(obj);
				if (!Update)
				{
					MessageBox.Show("Detail Schedule Save SuccessFully");
				}
				else
				{
					MessageBox.Show("Detail Schedule Update SuccessFully");
				}
				DetailFormReset();
				grd.DataSource = null;
				LstRemoveRecordDetail.Clear();
				ReadbyMainScheduleByContractId(Conversion.ToInt(grdPendingContract.CurrentRow.Cells["Id"].Value));
				ReadScheduleDetailByContactId(Conversion.ToInt(grdPendingContract.CurrentRow.Cells["Id"].Value));
				AttentiveLoadDateFillByContractId(Conversion.ToInt(grdPendingContract.CurrentRow.Cells["Id"].Value));
				ItemNameFillByContractId(Conversion.ToInt(grdPendingContract.CurrentRow.Cells["Id"].Value));
				CurrContractId = Conversion.ToInt(grdPendingContract.CurrentRow.Cells["Id"].Value);
			}
			else
			{
				MessageBox.Show("Schedule Grid have no row", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnSavedetailSchedule_Click(object sender, EventArgs e)
	{
		try
		{
			InsertScheduleDetail();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnNewDetailSchedule_Click(object sender, EventArgs e)
	{
		try
		{
			DetailFormReset();
			grd.DataSource = null;
			table.Rows.Clear();
			LstRemoveRecordDetail.Clear();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ReadScheduleDetailByContactId(int Id)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		try
		{
			DetailFormReset();
			LstRemoveRecordDetail.Clear();
			ExImLcOrderShipmentSchedulePackingDetail obj = new ExImLcOrderShipmentSchedulePackingDetail();
			obj.ExImLcOrderShipmentSchedulePackingDetailList = ExImLcOrderShipmentSchedule.GetPackingDetailByContractId(Id);
			if (obj.ExImLcOrderShipmentSchedulePackingDetailList != null && obj.ExImLcOrderShipmentSchedulePackingDetailList.Count > 0)
			{
				UpdateModeDetail = true;
				table.Rows.Clear();
				for (int i = 0; i < obj.ExImLcOrderShipmentSchedulePackingDetailList.Count; i++)
				{
					table.Rows.Add(obj.ExImLcOrderShipmentSchedulePackingDetailList[i].Id, obj.ExImLcOrderShipmentSchedulePackingDetailList[i].ExImLcOrderShipmentScheduleId, obj.ExImLcOrderShipmentSchedulePackingDetailList[i].ExImLcOrderId, obj.ExImLcOrderShipmentSchedulePackingDetailList[i].ContractDetailId, obj.ExImLcOrderShipmentSchedulePackingDetailList[i].AttentiveLoadingDate, obj.ExImLcOrderShipmentSchedulePackingDetailList[i].AttentiveLoadingDate.ToString("dd-MMM-yyyy") + " " + obj.ExImLcOrderShipmentSchedulePackingDetailList[i].DestinationPort + " " + obj.ExImLcOrderShipmentSchedulePackingDetailList[i].ScheduleCode, obj.ExImLcOrderShipmentSchedulePackingDetailList[i].ItemId, obj.ExImLcOrderShipmentSchedulePackingDetailList[i].ItemName, obj.ExImLcOrderShipmentSchedulePackingDetailList[i].ItemCode, obj.ExImLcOrderShipmentSchedulePackingDetailList[i].CropYearId, obj.ExImLcOrderShipmentSchedulePackingDetailList[i].CropYear, obj.ExImLcOrderShipmentSchedulePackingDetailList[i].PackingTypeId, obj.ExImLcOrderShipmentSchedulePackingDetailList[i].PackingType, obj.ExImLcOrderShipmentSchedulePackingDetailList[i].NoOfBags, obj.ExImLcOrderShipmentSchedulePackingDetailList[i].PackUomId, obj.ExImLcOrderShipmentSchedulePackingDetailList[i].PackUom, obj.ExImLcOrderShipmentSchedulePackingDetailList[i].InnerQty, obj.ExImLcOrderShipmentSchedulePackingDetailList[i].NetWeight / 1000m, obj.ExImLcOrderShipmentSchedulePackingDetailList[i].Status, obj.ExImLcOrderShipmentSchedulePackingDetailList[i].ReferredStatus);
				}
				grd.DataSource = table;
				grd.RetrieveStructure();
				GridDetailSetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			if (tabControl1.SelectedIndex == 1)
			{
				DataGridHistoryFill(50);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void DataGridHistoryFill(int NoOfRecords = 0)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		try
		{
			dtHistory.Rows.Clear();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.FromDate = txtdatefrom.Value;
			obj.ToDate = txtdateto.Value;
			obj.DestinationPortId = Conversion.ToInt(cmbPort.Value);
			obj.SupplierCustomerId = Conversion.ToInt(cmbSupplierName.Value);
			obj.ItemId = Conversion.ToInt(CmbItemHistory.Value);
			dtHistory = ExImLcOrderShipmentSchedule.ContractSchedule_FormHistory(obj);
			if (dtHistory.Rows.Count > 0)
			{
				DataTable dtcol = new DataTable();
				dtcol.Columns.Add("RecordNo", typeof(int));
				dtcol.Columns.Add("Id", typeof(int));
				dtcol.Columns.Add("ExImLcOrderId", typeof(int));
				dtcol.Columns.Add("ContractNo", typeof(string));
				dtcol.Columns.Add("SupplierCustomerId", typeof(int));
				dtcol.Columns.Add("CustomerName", typeof(string));
				dtcol.Columns.Add("ItemName", typeof(string));
				dtcol.Columns.Add("NoOfContainer", typeof(int));
				dtcol.Columns.Add("MTon", typeof(decimal));
				dtcol.Columns.Add("Contract_MTon", typeof(decimal));
				dtcol.Columns.Add("MtonUsedInForwarding", typeof(decimal));
				dtcol.Columns.Add("ContractBalMton", typeof(decimal));
				dtcol.Columns.Add("DestinationPort", typeof(string));
				dtcol.Columns.Add("LoadingDate", typeof(DateTime));
				dtcol.Columns.Add("ProductionDate", typeof(DateTime));
				dtcol.Columns.Add("InspectionDate", typeof(DateTime));
				dtcol.Columns.Add("PackingMaterialDate", typeof(DateTime));
				dtcol.Columns.Add("Remarks", typeof(string));
				dtcol.Columns.Add("Status", typeof(string));
				dtcol.Columns.Add("EntryDate", typeof(DateTime));
				dtcol.Columns.Add("EntryUser", typeof(string));
				dtcol.Columns.Add("NoOfAttachments", typeof(int));
				dtcol.Rows.Clear();
				for (int i = 0; i < dtHistory.Rows.Count; i++)
				{
					dtcol.Rows.Add(dtHistory.Rows[i]["RecordNo"], dtHistory.Rows[i]["Id"], dtHistory.Rows[i]["ExImLcOrderId"], dtHistory.Rows[i]["ContractNo"], dtHistory.Rows[i]["SupplierCustomerId"], dtHistory.Rows[i]["CustomerName"], dtHistory.Rows[i]["ItemName"], dtHistory.Rows[i]["NoOfContainer"], dtHistory.Rows[i]["MTon"], dtHistory.Rows[i]["Contract_MTon"], dtHistory.Rows[i]["MtonUsedInForwarding"], dtHistory.Rows[i]["ContractBalMton"], dtHistory.Rows[i]["DestinationPort"], Conversion.ToDateTime(dtHistory.Rows[i]["AttentiveLoadingDate"]).ToShortDateString(), Conversion.ToDateTime(dtHistory.Rows[i]["AttentiveProductionDate"]).ToShortDateString(), Conversion.ToDateTime(dtHistory.Rows[i]["AttentiveInspectionDate"]).ToShortDateString(), Conversion.ToDateTime(dtHistory.Rows[i]["PackingMaterialDate"]).ToShortDateString(), dtHistory.Rows[i]["ScheduleRemarks"], dtHistory.Rows[i]["Status"], Conversion.ToDateTime(dtHistory.Rows[i]["EntryDate"]).ToShortDateString(), dtHistory.Rows[i]["EntryUser"], dtHistory.Rows[i]["NoOfAttachments"]);
				}
				DataGridHistory.DataSource = dtcol;
				DataGridHistory.RetrieveStructure();
				DataGridHistorySetting();
			}
			else
			{
				DataGridHistory.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void DataGridHistorySetting()
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "SupplierCustomerId", "ExImLcOrderId", "InspectionDate" };
			Dictionary<string, EditType> editableColumns = new Dictionary<string, EditType> { 
			{
				"RecordNo",
				(EditType)1
			} };
			GridEX_Helper.GridWrappingAndColumnSettings(DataGridHistory, 2, 2, hiddenColumns, editableColumns);
			DataGridHistory.RootTable.Columns["MTon"].Caption = "Schedule_MTon";
			DataGridHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			DataGridHistory.RootTable.Columns["LoadingDate"].FormatString = "dd-MMM-yyyy";
			DataGridHistory.RootTable.Columns["InspectionDate"].FormatString = "dd-MMM-yyyy";
			DataGridHistory.RootTable.Columns["PackingMaterialDate"].FormatString = "dd-MMM-yyyy";
			DataGridHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yyyy";
			DataGridHistory.RootTable.Columns["ProductionDate"].FormatString = "dd-MMM-yyyy";
			if (formright.DoHaveUpdateRights)
			{
				GridEX_Helper.AddButton(DataGridHistory, "Edit", "Edit", 40, 0);
			}
			if (formright.DoHavePrintRights)
			{
				GridEX_Helper.AddSelectButton(DataGridHistory, "Select", 30, 0);
				DataGridHistory.RootTable.Columns["RecordNo"].Visible = true;
			}
			if (formright.DoHavePrintRights && formright.DoHaveUpdateRights)
			{
				DataGridHistory.RootTable.Columns["Select"].Position = 0;
				DataGridHistory.RootTable.Columns["Edit"].Position = 1;
				DataGridHistory.RootTable.Columns["RecordNo"].Position = 2;
				DataGridHistory.FrozenColumns = 1;
			}
			else if (formright.DoHaveUpdateRights)
			{
				DataGridHistory.RootTable.Columns["Edit"].Position = 0;
				DataGridHistory.FrozenColumns = 1;
			}
			else if (formright.DoHavePrintRights)
			{
				DataGridHistory.RootTable.Columns["Select"].Position = 0;
				DataGridHistory.RootTable.Columns["RecordNo"].Position = 1;
				DataGridHistory.FrozenColumns = 2;
			}
			ctrlGrdBar7_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnLoadAllHistoryNew_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)(object)cmbPort).Text = string.Empty;
			((Control)(object)cmbSupplierName).Text = string.Empty;
			((Control)(object)CmbItemHistory).Text = string.Empty;
			DataGridHistory.ClearStructure();
			txtdatefrom.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				ReadById(Conversion.ToInt(r.Cells["ExImLcOrderId"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void ReadById(int Id)
	{
		try
		{
			tabControl1.SelectedIndex = 0;
			UpdateModeDetail = false;
			UpdateModeMain = false;
			ReadbyMainScheduleByContractId(Id);
			ReadScheduleDetailByContactId(Id);
			if (!UpdateModeMain)
			{
				grdContractShedule.DataSource = null;
			}
			if (!UpdateModeDetail)
			{
				grd.DataSource = null;
			}
			AttentiveLoadDateFillByContractId(Id);
			ItemNameFillByContractId(Id);
			CurrContractId = Id;
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
			CommonServices.GetNoofAttachmentsByScreenName(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["ExImLcOrderId"].Value), base.Name);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnHistoryPrint_Click(object sender, EventArgs e)
	{
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable selectedRows = new DataTable();
			selectedRows.Columns.Add("Id", typeof(int));
			selectedRows.Columns.Add("RecordNo", typeof(int));
			GridEXRow[] checkedRows = DataGridHistory.GetCheckedRows();
			GridEXRow[] array = ((checkedRows != null && checkedRows.Length != 0) ? checkedRows : DataGridHistory.GetDataRows());
			foreach (GridEXRow row in array)
			{
				selectedRows.Rows.Add(Conversion.ToInt(row.Cells["Id"].Value), Conversion.ToInt(row.Cells["RecordNo"].Value));
			}
			if (selectedRows.Rows.Count == 0)
			{
				MessageBox.Show("No record selected for printing.");
				return;
			}
			IEnumerable<DataRow> filteredQuery = from historyRow in dtHistory.AsEnumerable()
				join selectedRow in selectedRows.AsEnumerable() on historyRow.Field<int>("Id") equals selectedRow.Field<int>("Id")
				orderby selectedRow.Field<int>("RecordNo")
				select historyRow;
			if (!filteredQuery.Any())
			{
				MessageBox.Show("No record found for display");
				return;
			}
			DataTable reportData = filteredQuery.CopyToDataTable();
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.ShowReportWithDataTable(reportData, "551-ContractSchedule_FormHistory.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void LoadAllgrdContract_Click(object sender, EventArgs e)
	{
		try
		{
			if (tabControl1.SelectedIndex == 1)
			{
				GridHistoryMainOfContactsExistInSchedule();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridHistoryMainOfContactsExistInSchedule(int NoOfRecords = 0)
	{
		try
		{
			DataTable dtContactsExistInSchedule = ExImLcOrderShipmentSchedule.GridContactsExistInScheduleOrNot(UserAccount.OrganizationId, UserAccount.CompanyId, 2, NoOfRecords);
			if (dtContactsExistInSchedule.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("ContractNo", typeof(string));
				dt.Columns.Add("ContractDate", typeof(DateTime));
				dt.Columns.Add("SupplierCustomerId", typeof(int));
				dt.Columns.Add("CustomerName");
				dt.Columns.Add("NoOfContainers", typeof(double));
				dt.Columns.Add("WeightM_Ton", typeof(double));
				dt.Columns.Add("LastShipmentDate", typeof(DateTime));
				dt.Clear();
				for (int i = 0; i < dtContactsExistInSchedule.Rows.Count; i++)
				{
					dt.Rows.Add(dtContactsExistInSchedule.Rows[i]["ExImLcOrderId"], dtContactsExistInSchedule.Rows[i]["LcOrderNo"], dtContactsExistInSchedule.Rows[i]["LcOrderDate"], dtContactsExistInSchedule.Rows[i]["SupCustId"], dtContactsExistInSchedule.Rows[i]["Customer"], dtContactsExistInSchedule.Rows[i]["NoOfContainers"], Conversion.ToDouble(dtContactsExistInSchedule.Rows[i]["WeightM_Ton"]), Conversion.ToDateTime(dtContactsExistInSchedule.Rows[i]["LastShipmentDate"]).ToString("dd-MMM-yyyy"));
				}
				grdHistoryMainByContract.DataSource = dt;
				grdHistoryMainByContract.RetrieveStructure();
				grdHistoryMainByContractSetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdHistoryMainByContractSetting()
	{
		try
		{
			grdHistoryMainByContract.RootTable.Columns["Id"].Visible = false;
			grdHistoryMainByContract.RootTable.Columns["SupplierCustomerId"].Visible = false;
			grdHistoryMainByContract.RootTable.Columns["NoOfContainers"].AggregateFunction = (AggregateFunction)2;
			grdHistoryMainByContract.RootTable.Columns["NoOfContainers"].TextAlignment = (TextAlignment)3;
			grdHistoryMainByContract.RootTable.Columns["NoOfContainers"].FormatString = "#,##0.##";
			grdHistoryMainByContract.RootTable.Columns["NoOfContainers"].TotalFormatString = "#,##0.##";
			grdHistoryMainByContract.RootTable.Columns["WeightM_Ton"].AggregateFunction = (AggregateFunction)2;
			grdHistoryMainByContract.RootTable.Columns["WeightM_Ton"].TextAlignment = (TextAlignment)3;
			grdHistoryMainByContract.RootTable.Columns["WeightM_Ton"].FormatString = "#,##0.##";
			grdHistoryMainByContract.RootTable.Columns["WeightM_Ton"].TotalFormatString = "#,##0.##";
			CommonServices.GridAutoAdjustmentNew(grdHistoryMainByContract);
			if (formright.DoHaveUpdateRights)
			{
				grdHistoryMainByContract.RootTable.Columns.Add("Edit");
				grdHistoryMainByContract.RootTable.Columns["Edit"].Caption = "Edit";
				grdHistoryMainByContract.RootTable.Columns["Edit"].Key = "Edit";
				grdHistoryMainByContract.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grdHistoryMainByContract.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
				grdHistoryMainByContract.RootTable.Columns["Edit"].ButtonText = "Edit";
				grdHistoryMainByContract.RootTable.Columns["Edit"].Width = 50;
				grdHistoryMainByContract.RootTable.Columns["Edit"].Position = 0;
				grdHistoryMainByContract.FrozenColumns = 1;
			}
			ctrlGrdBar4_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdHistoryMainByContract_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdHistoryMainByContract.CurrentRow;
			tabControl1.SelectedIndex = 0;
			UpdateModeDetail = false;
			UpdateModeMain = false;
			ReadbyMainScheduleByContractId(Conversion.ToInt(r.Cells["Id"].Value));
			ReadScheduleDetailByContactId(Conversion.ToInt(r.Cells["Id"].Value));
			if (!UpdateModeMain && r != null && (int)r.RowType == 0)
			{
				tableSheduleMain.Clear();
				tableSheduleMain.Rows.Add(0, r.Cells["Id"].Value, r.Cells["ContractNo"].Value?.ToString() + "/" + 1, r.Cells["SupplierCustomerId"].Value, r.Cells["CustomerName"].Value, r.Cells["NoOfContainers"].Value, r.Cells["WeightM_Ton"].Value, DateTime.Now, 0, DateTime.Now, DateTime.Now, DateTime.Now, 0, "", "", "");
				grdContractShedule.DataSource = tableSheduleMain;
				grdContractSheduleSettings();
			}
			if (!UpdateModeDetail)
			{
				grd.DataSource = null;
			}
			AttentiveLoadDateFillByContractId(Conversion.ToInt(r.Cells["Id"].Value));
			ItemNameFillByContractId(Conversion.ToInt(r.Cells["Id"].Value));
			CurrContractId = Conversion.ToInt(r.Cells["Id"].Value);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdHistoryMainByContract_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdHistoryMainByContract.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				GridScheduleFillByContactId(Conversion.ToInt(r.Cells["Id"].Value));
				GridShedulePackingDetailFillByContactID(Conversion.ToInt(r.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridScheduleFillByContactId(int Id)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ExImLcOrderShipmentSchedule obj = new ExImLcOrderShipmentSchedule();
			obj.ExImLcOrderShipmentScheduleList = ExImLcOrderShipmentSchedule.GetScheduleByContractId(Id);
			if (obj.ExImLcOrderShipmentScheduleList != null)
			{
				DataTable dtcol = new DataTable();
				dtcol.Columns.Add("Id", typeof(int));
				dtcol.Columns.Add("ContactId", typeof(int));
				dtcol.Columns.Add("ContactNo", typeof(string));
				dtcol.Columns.Add("SupplierCustomerId", typeof(int));
				dtcol.Columns.Add("CustomrerName", typeof(string));
				dtcol.Columns.Add("ScheduleCode", typeof(string));
				dtcol.Columns.Add("NoOfContainer", typeof(int));
				dtcol.Columns.Add("NetWeight", typeof(decimal));
				dtcol.Columns.Add("AttentiveLoadingDate", typeof(DateTime));
				dtcol.Columns.Add("DestinationPort", typeof(string));
				dtcol.Columns.Add("AttentiveProductionDate", typeof(DateTime));
				dtcol.Columns.Add("AttentiveInspectionDate", typeof(DateTime));
				dtcol.Columns.Add("EntryDate", typeof(DateTime));
				dtcol.Columns.Add("EntryUser", typeof(string));
				dtcol.Columns.Add("ModifyDate", typeof(DateTime));
				dtcol.Columns.Add("ModifyUser", typeof(string));
				dtcol.Rows.Clear();
				for (int i = 0; i < obj.ExImLcOrderShipmentScheduleList.Count; i++)
				{
					dtcol.Rows.Add(obj.ExImLcOrderShipmentScheduleList[i].Id, obj.ExImLcOrderShipmentScheduleList[i].ExImLcOrderId, obj.ExImLcOrderShipmentScheduleList[i].LcOrderNo, obj.ExImLcOrderShipmentScheduleList[i].SupplierCustomerId, obj.ExImLcOrderShipmentScheduleList[i].CustomerName, obj.ExImLcOrderShipmentScheduleList[i].ScheduleCode, obj.ExImLcOrderShipmentScheduleList[i].NoOfContainer, obj.ExImLcOrderShipmentScheduleList[i].NetWeight / 1000m, obj.ExImLcOrderShipmentScheduleList[i].AttentiveLoadingDate, obj.ExImLcOrderShipmentScheduleList[i].DestinationPort, obj.ExImLcOrderShipmentScheduleList[i].AttentiveProductionDate, obj.ExImLcOrderShipmentScheduleList[i].AttentiveInspectionDate, obj.ExImLcOrderShipmentScheduleList[i].EntryDate, obj.ExImLcOrderShipmentScheduleList[i].EntryUser, obj.ExImLcOrderShipmentScheduleList[i].ModifyDate, obj.ExImLcOrderShipmentScheduleList[i].ModifyUser);
				}
				grdHistoryScheduleByContract.DataSource = dtcol;
				grdHistoryScheduleByContract.RetrieveStructure();
				grdHistoryScheduleByContractSetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdHistoryScheduleByContractSetting()
	{
		try
		{
			grdHistoryScheduleByContract.RootTable.Columns["Id"].Visible = false;
			grdHistoryScheduleByContract.RootTable.Columns["ContactId"].Visible = false;
			grdHistoryScheduleByContract.RootTable.Columns["SupplierCustomerId"].Visible = false;
			CommonServices.GridAutoAdjustmentNew(grdHistoryScheduleByContract);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridShedulePackingDetailFillByContactID(int Id)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ExImLcOrderShipmentSchedulePackingDetail obj = new ExImLcOrderShipmentSchedulePackingDetail();
			obj.ExImLcOrderShipmentSchedulePackingDetailList = ExImLcOrderShipmentSchedule.GetPackingDetailByContractId(Id);
			if (obj.ExImLcOrderShipmentSchedulePackingDetailList != null)
			{
				DataTable dtcol = new DataTable();
				dtcol.Columns.Add("Id", typeof(int));
				dtcol.Columns.Add("ScheduleId", typeof(int));
				dtcol.Columns.Add("ContractId", typeof(int));
				dtcol.Columns.Add("ContractDetailId", typeof(int));
				dtcol.Columns.Add("AttentiveLoadingDate", typeof(DateTime));
				dtcol.Columns.Add("ItemId", typeof(int));
				dtcol.Columns.Add("Item");
				dtcol.Columns.Add("ItemCode");
				dtcol.Columns.Add("CropYear");
				dtcol.Columns.Add("PackingType");
				dtcol.Columns.Add("NoOfBags", typeof(double));
				dtcol.Columns.Add("PackUom");
				dtcol.Columns.Add("WeightM_Ton", typeof(double));
				dtcol.Rows.Clear();
				for (int i = 0; i < obj.ExImLcOrderShipmentSchedulePackingDetailList.Count; i++)
				{
					dtcol.Rows.Add(obj.ExImLcOrderShipmentSchedulePackingDetailList[i].Id, obj.ExImLcOrderShipmentSchedulePackingDetailList[i].ExImLcOrderShipmentScheduleId, obj.ExImLcOrderShipmentSchedulePackingDetailList[i].ExImLcOrderId, obj.ExImLcOrderShipmentSchedulePackingDetailList[i].ContractDetailId, obj.ExImLcOrderShipmentSchedulePackingDetailList[i].AttentiveLoadingDate, obj.ExImLcOrderShipmentSchedulePackingDetailList[i].ItemId, obj.ExImLcOrderShipmentSchedulePackingDetailList[i].ItemName, obj.ExImLcOrderShipmentSchedulePackingDetailList[i].ItemCode, obj.ExImLcOrderShipmentSchedulePackingDetailList[i].CropYear, obj.ExImLcOrderShipmentSchedulePackingDetailList[i].PackingType, obj.ExImLcOrderShipmentSchedulePackingDetailList[i].NoOfBags, obj.ExImLcOrderShipmentSchedulePackingDetailList[i].PackUom, obj.ExImLcOrderShipmentSchedulePackingDetailList[i].NetWeight / 1000m);
				}
				grdHistoryScheduleDetailByContact.DataSource = dtcol;
				grdHistoryScheduleDetailByContact.RetrieveStructure();
				grdHistoryScheduleDetailByContactSetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdHistoryScheduleDetailByContactSetting()
	{
		try
		{
			grdHistoryScheduleDetailByContact.RootTable.Columns["NoOfBags"].AggregateFunction = (AggregateFunction)2;
			grdHistoryScheduleDetailByContact.RootTable.Columns["NoOfBags"].TextAlignment = (TextAlignment)3;
			grdHistoryScheduleDetailByContact.RootTable.Columns["NoOfBags"].FormatString = "0,0";
			grdHistoryScheduleDetailByContact.RootTable.Columns["NoOfBags"].TotalFormatString = "#,##0.##";
			grdHistoryScheduleDetailByContact.RootTable.Columns["WeightM_Ton"].AggregateFunction = (AggregateFunction)2;
			grdHistoryScheduleDetailByContact.RootTable.Columns["WeightM_Ton"].TextAlignment = (TextAlignment)3;
			grdHistoryScheduleDetailByContact.RootTable.Columns["WeightM_Ton"].FormatString = "0,0";
			grdHistoryScheduleDetailByContact.RootTable.Columns["WeightM_Ton"].TotalFormatString = "#,##0.##";
			grdHistoryScheduleDetailByContact.RootTable.Columns["Id"].Visible = false;
			grdHistoryScheduleDetailByContact.RootTable.Columns["ItemId"].Visible = false;
			grdHistoryScheduleDetailByContact.RootTable.Columns["ContractId"].Visible = false;
			grdHistoryScheduleDetailByContact.RootTable.Columns["ContractDetailId"].Visible = false;
			grdHistoryScheduleDetailByContact.RootTable.Columns["ScheduleId"].Visible = false;
			CommonServices.GridAutoAdjustmentNew(grdHistoryScheduleDetailByContact);
			ctrlGrdBar6_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdPendingContract).Name);
			if (fs != null)
			{
				grdPendingContract.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
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

	private void ctrlGrdBar4_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdHistoryMainByContract).Name);
			if (fs != null)
			{
				grdHistoryMainByContract.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdHistoryScheduleByContract).Name);
			if (fs != null)
			{
				grdHistoryScheduleByContract.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar6_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdHistoryScheduleDetailByContact).Name);
			if (fs != null)
			{
				grdHistoryScheduleDetailByContact.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtNoOfBagsDetail_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateMtonDetail();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar7_Load(object sender, EventArgs e)
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

	private void frmSaleContractSchedule_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					if (((Control)(object)grdPendingContract).Focused)
					{
						BtnNewMain_Click(null, null);
					}
					else if (((Control)(object)grdContractShedule).Focused)
					{
						btnnew_Click(null, null);
					}
					else
					{
						btnNewDetailSchedule_Click(null, null);
					}
				}
				if (tabControlMain.SelectedIndex == 1)
				{
					BtnNewDepartmentDetail_Click(null, null);
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
			if (e.Control && e.KeyCode == Keys.L)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					BtnLoadAllPendingContracts_Click(null, null);
				}
				else
				{
					LoadAllgrdContract_Click(null, null);
				}
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				if (formright.DoHaveSaveRight && tabControl1.SelectedIndex == 0)
				{
					if (((Control)(object)grd).Focused)
					{
						btnSavedetailSchedule_Click(null, null);
					}
					else if (((Control)(object)grdContractShedule).Focused)
					{
						btnsave_Click(null, null);
					}
				}
				if (tabControl1.SelectedIndex == 1)
				{
					btnShow_Click(null, null);
				}
			}
			if (e.Alt && e.KeyCode == Keys.P)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					BtnContractWiseSlip_Click(null, null);
				}
				else
				{
					BtnHistoryPrint_Click(null, null);
				}
			}
			if (tabControl1.SelectedIndex == 1)
			{
				if (e.Control && (e.KeyCode == Keys.D1 || e.KeyCode == Keys.NumPad1))
				{
					BtnHistoryPrint_Click(null, null);
				}
				else if (e.Control && (e.KeyCode == Keys.D2 || e.KeyCode == Keys.NumPad2))
				{
					BtnPrint551_01_Click(null, null);
				}
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					((Control)(object)grdPendingContract).Focus();
				}
				else
				{
					txtdatefrom.Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				cmbAttentiveLoadDateBySchedule.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					((Control)(object)grd).Focus();
				}
				else
				{
					((Control)(object)DataGridHistory).Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.Right)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					if (((Control)(object)grdPendingContract).Focused)
					{
						((Control)(object)grdContractShedule).Focus();
					}
					else if (((Control)(object)grdContractShedule).Focused)
					{
						((Control)(object)grd).Focus();
					}
					else
					{
						((Control)(object)grdPendingContract).Focus();
					}
				}
				else if (((Control)(object)grdHistoryMainByContract).Focused)
				{
					((Control)(object)grdHistoryScheduleByContract).Focus();
				}
				else if (((Control)(object)grdHistoryScheduleByContract).Focused)
				{
					((Control)(object)grdHistoryScheduleDetailByContact).Focus();
				}
				else
				{
					((Control)(object)grdHistoryMainByContract).Focus();
				}
			}
			if (e.Control)
			{
				_ = e.KeyCode;
				_ = 121;
			}
			if (e.Control && e.Alt)
			{
				BtnShortCutkeys_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.R && tabControlMain.SelectedIndex == 1)
			{
				BtnRefreshDepartmentDetail_Click(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
			dt.Rows.Add("Ctrl+S", "For Save of Focused Grid");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+N", "For New of Focused Grid");
			dt.Rows.Add("Ctrl+L", "For Load All");
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Alt+1", "For Print 551 in History");
			dt.Rows.Add("Alt+2", "For Print 551_01 in History");
			dt.Rows.Add("Ctrl+F5", "For Focus on 'Sale Contract for Schedule' Grid");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+Right", "For Move focus From One Grid to Next");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Schedule Detail Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Date Combo in Detail Box");
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

	private void grdPendingContract_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!e.Control || e.KeyCode != Keys.Space)
			{
				return;
			}
			GridEXRow r = grdPendingContract.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdPendingContract.CurrentColumn == null)
			{
				return;
			}
			if (grdPendingContract.CurrentColumn.Key == "ContractNo")
			{
				UpdateModeDetail = false;
				UpdateModeMain = false;
				ReadbyMainScheduleByContractId(Conversion.ToInt(r.Cells["Id"].Value));
				ReadScheduleDetailByContactId(Conversion.ToInt(r.Cells["Id"].Value));
				if (!UpdateModeMain && r != null && (int)r.RowType == 0)
				{
					tableSheduleMain.Clear();
					tableSheduleMain.Rows.Add(0, r.Cells["Id"].Value, r.Cells["ContractNo"].Value?.ToString() + "/" + 1, r.Cells["SupplierCustomerId"].Value, r.Cells["CustomerName"].Value, "", r.Cells["NoOfContainers"].Value, r.Cells["WeightM_Ton"].Value, DateTime.Now, 0, DateTime.Now, DateTime.Now, DateTime.Now, 0, "", "", "");
					grdContractShedule.DataSource = tableSheduleMain;
					grdContractSheduleSettings();
				}
				if (!UpdateModeDetail)
				{
					grd.DataSource = null;
				}
				AttentiveLoadDateFillByContractId(Conversion.ToInt(r.Cells["Id"].Value));
				ItemNameFillByContractId(Conversion.ToInt(r.Cells["Id"].Value));
				CurrContractId = Conversion.ToInt(r.Cells["Id"].Value);
			}
			grdPendingContract.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdContractShedule_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Expected O, but got Unknown
		//IL_033b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow row = grdContractShedule.CurrentRow;
			if (row == null || (int)row.RowType != 0 || grdContractShedule.CurrentColumn == null)
			{
				return;
			}
			_ = ((Control)(object)grdContractShedule).Focused;
			if (!e.Control || e.KeyCode != Keys.Space)
			{
				return;
			}
			if (grdContractShedule.CurrentColumn.Key == "Delete")
			{
				GridEXRow item = grdContractShedule.CurrentRow;
				if (item != null && (int)item.RowType == 0 && tableSheduleMain.Rows.Count > 1)
				{
					if (Conversion.ToInt(item.Cells["Id"].Value) == 0)
					{
						item.Delete();
						return;
					}
					string status = Conversion.ToString(item.Cells["Status"].Value);
					if (status != "Pending")
					{
						throw new Exception("You can't delete this row because its detail is " + status);
					}
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
					{
						ExImLcOrderShipmentSchedule exIm = new ExImLcOrderShipmentSchedule();
						exIm.Id = Conversion.ToInt(item.Cells["Id"].Value);
						exIm.ExImLcOrderId = Conversion.ToInt(item.Cells["ContractId"].Value);
						exIm.SupplierCustomerId = Conversion.ToInt(item.Cells["SupplierCustomerId"].Value);
						exIm.NoOfContainer = Conversion.ToInt(item.Cells["NoOfContainers"].Value);
						exIm.NetWeight = Conversion.ToDecimal(item.Cells["WeightM_Ton"].Value) * 1000m;
						exIm.AttentiveLoadingDate = Conversion.ToDateTime(item.Cells["AttentiveLoadingDate"].Value);
						exIm.DestinationPortId = Conversion.ToInt(item.Cells["DestinationPortId"].Value);
						exIm.CustomGroupId = Conversion.ToInt(item.Cells["CustomGroupId"].Value);
						exIm.AttentiveProductionDate = Conversion.ToDateTime(item.Cells["AttentiveProductionDate"].Value);
						exIm.AttentiveInspectionDate = Conversion.ToDateTime(item.Cells["AttentiveInspectionDate"].Value);
						exIm.PackingMaterialDate = Conversion.ToDateTime(item.Cells["PackingMaterialDate"].Value);
						exIm.EntryDate = DateTime.Now;
						exIm.EntryUserId = UserAccount.ID;
						exIm.ModifyDate = DateTime.Now;
						exIm.ModifyUserId = UserAccount.ID;
						exIm.OrganizationId = UserAccount.OrganizationId;
						exIm.CompanyId = UserAccount.CompanyId;
						exIm.BranchesId = UserAccount.BranchesId;
						exIm.DocumentTypeId = 244;
						exIm.ActionId = 3;
						LstRemoveRecordMain.Add(exIm);
						tableSheduleMain.Rows[item.RowIndex].Delete();
						if (item != null && (int)item.RowType == 0)
						{
							item.Delete();
						}
					}
				}
				else
				{
					MessageBox.Show("You Can Not Delete All rows");
				}
			}
			else if (grdContractShedule.CurrentColumn.Key == "Add")
			{
				AddRowInScheduleMainGrid();
			}
			else if (grdContractShedule.CurrentColumn.Key == "AddAttachment")
			{
				int Id = Conversion.ToInt(row.Cells["Id"].Value);
				if (Id > 0)
				{
					AttachmentAddingFromHistory attachmentAddingFromHistory = new AttachmentAddingFromHistory(UserAccount, Id, 244, base.Name);
					attachmentAddingFromHistory.BringToFront();
					attachmentAddingFromHistory.ShowDialog();
				}
				else
				{
					MessageBox.Show("This record is not saved so can't add attachments");
				}
			}
			else if (grdContractShedule.CurrentColumn.Key == "NoOfAttachments")
			{
				int Id2 = Conversion.ToInt(row.Cells["Id"].Value);
				if (Id2 > 0)
				{
					CommonServices.GetSaveAndDeleteAttachmentsForRecord(UserAccount, Id2, 244, "FrmExportSalesContractSchedule");
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grd_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow row = grd.CurrentRow;
			if (row != null && (int)row.RowType == 0 && grd.CurrentColumn != null)
			{
				if (e.Control && e.KeyCode == Keys.Space && grd.CurrentColumn.Key == "Delete")
				{
					DeleteDetailrow(row);
				}
				if (e.Control && e.KeyCode == Keys.Return)
				{
					grd_DoubleClick(null, null);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdHistoryMainByContract_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Control && e.KeyCode == Keys.Space)
			{
				GridEXRow r = grdHistoryMainByContract.CurrentRow;
				if (r != null && (int)r.RowType == 0 && grdHistoryMainByContract.CurrentColumn != null && grdHistoryMainByContract.CurrentColumn.Key == "Edit")
				{
					tabControl1.SelectedIndex = 0;
					UpdateModeDetail = false;
					UpdateModeMain = false;
					ReadbyMainScheduleByContractId(Conversion.ToInt(r.Cells["Id"].Value));
					ReadScheduleDetailByContactId(Conversion.ToInt(r.Cells["Id"].Value));
					if (!UpdateModeMain && r != null && (int)r.RowType == 0)
					{
						tableSheduleMain.Clear();
						tableSheduleMain.Rows.Add(0, r.Cells["Id"].Value, r.Cells["SupplierCustomerId"].Value, r.Cells["CustomerName"].Value, r.Cells["NoOfContainers"].Value, r.Cells["WeightM_Ton"].Value, DateTime.Now, 0, DateTime.Now, DateTime.Now, DateTime.Now);
						grdContractShedule.DataSource = tableSheduleMain;
						grdContractSheduleSettings();
					}
					if (!UpdateModeDetail)
					{
						grd.DataSource = null;
					}
					AttentiveLoadDateFillByContractId(Conversion.ToInt(r.Cells["Id"].Value));
					ItemNameFillByContractId(Conversion.ToInt(r.Cells["Id"].Value));
					CurrContractId = Conversion.ToInt(r.Cells["Id"].Value);
				}
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				grdHistoryMainByContract_ColumnButtonClick(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void DataGridHistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Control && e.KeyCode == Keys.Space)
			{
				GridEXRow r = DataGridHistory.CurrentRow;
				if (r != null && (int)r.RowType == 0 && DataGridHistory.CurrentColumn != null)
				{
					if (DataGridHistory.CurrentColumn.Key == "Edit" && formright.DoHaveUpdateRights)
					{
						tabControl1.SelectedIndex = 0;
						UpdateModeDetail = false;
						UpdateModeMain = false;
						ReadbyMainScheduleByContractId(Conversion.ToInt(r.Cells["ExImLcOrderId"].Value));
						ReadScheduleDetailByContactId(Conversion.ToInt(r.Cells["ExImLcOrderId"].Value));
						if (!UpdateModeMain)
						{
							grdContractShedule.DataSource = null;
						}
						if (!UpdateModeDetail)
						{
							grd.DataSource = null;
						}
						AttentiveLoadDateFillByContractId(Conversion.ToInt(r.Cells["ExImLcOrderId"].Value));
						ItemNameFillByContractId(Conversion.ToInt(r.Cells["ExImLcOrderId"].Value));
						CurrContractId = Conversion.ToInt(r.Cells["ExImLcOrderId"].Value);
					}
					if (DataGridHistory.CurrentColumn.Key == "NoOfAttachments")
					{
						DataGridHistory_LinkClicked(null, null);
					}
				}
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				DataGridHistory_ColumnButtonClick(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtMTonDetail_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateMtonDetail();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnRefreshHistoryCombos_Click(object sender, EventArgs e)
	{
		try
		{
			ComboBindForHistory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShow_Click(object sender, EventArgs e)
	{
		try
		{
			DataGridHistoryFill(50);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnContractWiseSlip_Click(object sender, EventArgs e)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (grdPendingContract.RowCount > 0)
			{
				GridEXRow r = grdPendingContract.CurrentRow;
				if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
				{
					CommonServices.LcOrderShipmentScheduleLcOrderWise_Slip(Conversion.ToInt(r.Cells["Id"].Value));
				}
				else
				{
					MessageBox.Show("First Click On any In  (Pending Grid)");
				}
			}
			else
			{
				MessageBox.Show("(Pending Grid) has no Record");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmSaleContractSchedule_FormClosed(object sender, FormClosedEventArgs e)
	{
		try
		{
			HistoryStack.Remove(this);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void rdNotReffered_Click(object sender, EventArgs e)
	{
		try
		{
			GetpendingContractsforShipmentSchedule(50, 1);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void rdReffered_Click(object sender, EventArgs e)
	{
		try
		{
			GetpendingContractsforShipmentSchedule(50, 2);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void rdAll_Click(object sender, EventArgs e)
	{
		try
		{
			GetpendingContractsforShipmentSchedule(50);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BtnPrint551_01_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Expected O, but got Unknown
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dtReport = ExImLcOrderShipmentSchedule.ContractSchedule_StatusReport(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FromDate = txtdatefrom.Value,
				ToDate = txtdateto.Value,
				DestinationPortId = Conversion.ToInt(cmbPort.Value),
				SupplierCustomerId = Conversion.ToInt(cmbSupplierName.Value),
				ItemId = Conversion.ToInt(CmbItemHistory.Value)
			});
			if (dtReport == null || dtReport.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.ShowReportWithDataTable(dtReport, "551_01_ContractSchedule_StatusReport.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BtnContractPmDetailForm_Click(object sender, EventArgs e)
	{
		try
		{
			new frmExportContractSchedulePmDetail(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnShipmentDocumentForm_Click(object sender, EventArgs e)
	{
		try
		{
			if (ShipmentDocumentsFeature)
			{
				new frmShipmentDocumentSchedule(UserAccount).Show();
				return;
			}
			throw new Exception("ShipmentDocumentsFeature is off...");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PlantFeader()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		try
		{
			int BomId = Conversion.ToInt(CmbPlantDepartmentDetail.Value);
			DataTable dtProductionplant = new DataTable();
			dtProductionplant = InvProductionPlant.GetAll(new InvProductionPlant
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtProductionplant.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtProductionplant, CmbPlantDepartmentDetail, "Id", "Description", "Plant", true);
				if (BomId > 0)
				{
					if (dtProductionplant.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == BomId))
					{
						CmbPlantDepartmentDetail.Value = BomId;
					}
					else
					{
						((Control)(object)CmbPlantDepartmentDetail).Text = string.Empty;
					}
				}
			}
			else
			{
				((UltraGridBase)CmbPlantDepartmentDetail).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ScheduleLockStatusDepartmentDetailFill()
	{
		try
		{
			DataTable dtDepartmentLockStatus = new DataTable();
			dtDepartmentLockStatus.Columns.Add("Id");
			dtDepartmentLockStatus.Columns.Add("Status");
			dtDepartmentLockStatus.Rows.Add(1, "Confirmed");
			dtDepartmentLockStatus.Rows.Add(2, "Not_Confirmed");
			DDL.BindDDL(dtDepartmentLockStatus, CmbScheduleLockStatusDepartmentDetail, "Id", "Status", "Status", ZeroIndex: false);
			((UltraGridBase)CmbScheduleLockStatusDepartmentDetail).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tabControlMain_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			if (tabControlMain.SelectedIndex == 1)
			{
				if (grdDepartmentDetail.RowCount == 0)
				{
					DepartmentDetailGridFill();
				}
			}
			else
			{
				((Control)(object)grdPendingContract).Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DepartmentDetailGridFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			if (txtDateFromDepart.Checked)
			{
				obj.FromDate = Conversion.ToDateTime((object)txtDateFromDepart.Value);
			}
			if (txtDatetoDepart.Checked)
			{
				obj.ToDate = Conversion.ToDateTime((object)txtDatetoDepart.Value);
			}
			DataTable dtDepart = ExImLcContractScheduleDepartment.GetDatForContractScheduleDepartment(obj);
			if (dtDepart.Rows.Count > 0)
			{
				dtDepartmentDetailGrid.Clear();
				foreach (DataRow row in dtDepart.Rows)
				{
					DateTime LoadingDate = Conversion.ToDateTime(row["LoadingDate"]);
					DateTime ProductionDate = Conversion.ToDateTime(row["ProductionSchedule"]);
					int Days = (int)(LoadingDate.Date - ProductionDate.Date).TotalDays;
					dtDepartmentDetailGrid.Rows.Add(row["Id"], row["ContractScheduleId"], row["ScheduleCode"], row["ExImLcOrderId"], row["ContractNo"], row["SupplierCustomerId"], row["CustomerName"], row["DestinationPortId"], row["DestinationPort"], row["FCL"], row["ScheduleMTon"], row["LoadingDate"], row["ContractScheduleDetailItemId"], row["ItemCode"], row["ItemName"], row["ItemMTon"], Days, row["ProductionSchedule"], row["ProductionStart"], row["JobOrderId"], row["InspectionSchedule"], row["InspectionStart"], row["PreShipmentInspectionId"], row["PackMaterialSchedule"], row["PackMaterialStart"], row["PmStartRefDocumentTypeId"], row["PmStartRefId"], row["PlantId"], row["PlantName"], row["ScheduleLockId"], row["ScheduleLockStatus"]);
				}
				grdDepartmentDetail.DataSource = dtDepartmentDetailGrid;
				GridDepartmentdetailSetting();
			}
			else
			{
				grdDepartmentDetail.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridDepartmentdetailSetting()
	{
		try
		{
			grdDepartmentDetail.RootTable.Columns["Id"].Visible = false;
			grdDepartmentDetail.RootTable.Columns["ContractScheduleId"].Visible = false;
			grdDepartmentDetail.RootTable.Columns["ExImLcOrderId"].Visible = false;
			grdDepartmentDetail.RootTable.Columns["SupplierCustomerId"].Visible = false;
			grdDepartmentDetail.RootTable.Columns["DestinationPortId"].Visible = false;
			grdDepartmentDetail.RootTable.Columns["ContractScheduleDetailItemId"].Visible = false;
			grdDepartmentDetail.RootTable.Columns["PlantId"].Visible = false;
			grdDepartmentDetail.RootTable.Columns["ScheduleLockId"].Visible = false;
			grdDepartmentDetail.RootTable.Columns["JobOrderId"].Visible = false;
			grdDepartmentDetail.RootTable.Columns["PreShipmentInspectionId"].Visible = false;
			grdDepartmentDetail.RootTable.Columns["PmStartRefDocumentTypeId"].Visible = false;
			grdDepartmentDetail.RootTable.Columns["PmStartRefId"].Visible = false;
			grdDepartmentDetail.RootTable.Columns["LoadingDate"].FormatString = "dd-MMM-yyyy";
			grdDepartmentDetail.RootTable.Columns["ProductionSchedule"].FormatString = "dd-MMM-yyyy";
			grdDepartmentDetail.RootTable.Columns["ProductionStart"].FormatString = "dd-MMM-yyyy";
			grdDepartmentDetail.RootTable.Columns["InspectionSchedule"].FormatString = "dd-MMM-yyyy";
			grdDepartmentDetail.RootTable.Columns["InspectionStart"].FormatString = "dd-MMM-yyyy";
			grdDepartmentDetail.RootTable.Columns["PackMaterialSchedule"].FormatString = "dd-MMM-yyyy";
			grdDepartmentDetail.RootTable.Columns["PackMaterialStart"].FormatString = "dd-MMM-yyyy";
			grdDepartmentDetail.RootTable.Columns["ScheduleCode"].ColumnType = (ColumnType)5;
			grdDepartmentDetail.RootTable.Columns["ContractNo"].ColumnType = (ColumnType)5;
			grdDepartmentDetail.RootTable.Columns["InspectionStart"].ColumnType = (ColumnType)5;
			grdDepartmentDetail.RootTable.Columns["ProductionStart"].ColumnType = (ColumnType)5;
			grdDepartmentDetail.RootTable.Columns["PackMaterialStart"].ColumnType = (ColumnType)5;
			grdDepartmentDetail.RootTable.Columns["ItemCode"].ColumnType = (ColumnType)5;
			grdDepartmentDetail.RootTable.Columns["ScheduleCode"].Width = 100;
			grdDepartmentDetail.RootTable.Columns["ContractNo"].Width = 120;
			grdDepartmentDetail.RootTable.Columns["SupplierCustomerId"].Width = 100;
			grdDepartmentDetail.RootTable.Columns["CustomerName"].Width = 170;
			grdDepartmentDetail.RootTable.Columns["DestinationPort"].Width = 120;
			grdDepartmentDetail.RootTable.Columns["FCL"].Width = 55;
			grdDepartmentDetail.RootTable.Columns["ScheduleMTon"].Width = 70;
			grdDepartmentDetail.RootTable.Columns["LoadingDate"].Width = 90;
			grdDepartmentDetail.RootTable.Columns["ItemCode"].Width = 60;
			grdDepartmentDetail.RootTable.Columns["ItemName"].Width = 170;
			grdDepartmentDetail.RootTable.Columns["ItemMTon"].Width = 60;
			grdDepartmentDetail.RootTable.Columns["ProductionDays"].Width = 70;
			grdDepartmentDetail.RootTable.Columns["ProductionSchedule"].Width = 90;
			grdDepartmentDetail.RootTable.Columns["ProductionStart"].Width = 80;
			grdDepartmentDetail.RootTable.Columns["InspectionSchedule"].Width = 90;
			grdDepartmentDetail.RootTable.Columns["InspectionStart"].Width = 80;
			grdDepartmentDetail.RootTable.Columns["PackMaterialSchedule"].Width = 90;
			grdDepartmentDetail.RootTable.Columns["PackMaterialStart"].Width = 90;
			grdDepartmentDetail.RootTable.Columns["PlantName"].Width = 90;
			grdDepartmentDetail.RootTable.Columns["ScheduleLockStatus"].Width = 100;
			grdDepartmentDetail.RootTable.Columns["ContractScheduleId"].EditType = (EditType)0;
			grdDepartmentDetail.RootTable.Columns["ScheduleCode"].EditType = (EditType)0;
			grdDepartmentDetail.RootTable.Columns["ExImLcOrderId"].EditType = (EditType)0;
			grdDepartmentDetail.RootTable.Columns["ContractNo"].EditType = (EditType)0;
			grdDepartmentDetail.RootTable.Columns["CustomerName"].EditType = (EditType)0;
			grdDepartmentDetail.RootTable.Columns["DestinationPort"].EditType = (EditType)0;
			grdDepartmentDetail.RootTable.Columns["FCL"].EditType = (EditType)0;
			grdDepartmentDetail.RootTable.Columns["ScheduleMTon"].EditType = (EditType)0;
			grdDepartmentDetail.RootTable.Columns["LoadingDate"].EditType = (EditType)0;
			grdDepartmentDetail.RootTable.Columns["ContractScheduleDetailItemId"].EditType = (EditType)0;
			grdDepartmentDetail.RootTable.Columns["ItemCode"].EditType = (EditType)0;
			grdDepartmentDetail.RootTable.Columns["ItemName"].EditType = (EditType)0;
			grdDepartmentDetail.RootTable.Columns["ItemMTon"].EditType = (EditType)0;
			grdDepartmentDetail.RootTable.Columns["ProductionSchedule"].EditType = (EditType)0;
			grdDepartmentDetail.RootTable.Columns["ProductionStart"].EditType = (EditType)0;
			grdDepartmentDetail.RootTable.Columns["InspectionSchedule"].EditType = (EditType)0;
			grdDepartmentDetail.RootTable.Columns["InspectionStart"].EditType = (EditType)0;
			grdDepartmentDetail.RootTable.Columns["PackMaterialSchedule"].EditType = (EditType)0;
			grdDepartmentDetail.RootTable.Columns["PackMaterialStart"].EditType = (EditType)0;
			grdDepartmentDetail.RootTable.Columns["PlantId"].EditType = (EditType)0;
			grdDepartmentDetail.RootTable.Columns["PlantName"].EditType = (EditType)0;
			grdDepartmentDetail.RootTable.Columns["ScheduleLockId"].EditType = (EditType)0;
			grdDepartmentDetail.RootTable.Columns["ScheduleLockStatus"].EditType = (EditType)0;
			grdDepartmentDetail.RootTable.Columns["ProductionDays"].EditType = (EditType)0;
			grdDepartmentDetail.RootTable.Columns["ContractScheduleId"].FilterEditType = (FilterEditType)1;
			grdDepartmentDetail.RootTable.Columns["ScheduleCode"].FilterEditType = (FilterEditType)1;
			grdDepartmentDetail.RootTable.Columns["ExImLcOrderId"].FilterEditType = (FilterEditType)1;
			grdDepartmentDetail.RootTable.Columns["ContractNo"].FilterEditType = (FilterEditType)1;
			grdDepartmentDetail.RootTable.Columns["CustomerName"].FilterEditType = (FilterEditType)1;
			grdDepartmentDetail.RootTable.Columns["DestinationPort"].FilterEditType = (FilterEditType)1;
			grdDepartmentDetail.RootTable.Columns["FCL"].FilterEditType = (FilterEditType)1;
			grdDepartmentDetail.RootTable.Columns["ScheduleMTon"].FilterEditType = (FilterEditType)1;
			grdDepartmentDetail.RootTable.Columns["LoadingDate"].FilterEditType = (FilterEditType)6;
			grdDepartmentDetail.RootTable.Columns["ContractScheduleDetailItemId"].FilterEditType = (FilterEditType)1;
			grdDepartmentDetail.RootTable.Columns["ItemCode"].FilterEditType = (FilterEditType)1;
			grdDepartmentDetail.RootTable.Columns["ItemName"].FilterEditType = (FilterEditType)1;
			grdDepartmentDetail.RootTable.Columns["ItemMTon"].FilterEditType = (FilterEditType)1;
			grdDepartmentDetail.RootTable.Columns["ProductionSchedule"].FilterEditType = (FilterEditType)6;
			grdDepartmentDetail.RootTable.Columns["ProductionStart"].FilterEditType = (FilterEditType)6;
			grdDepartmentDetail.RootTable.Columns["InspectionSchedule"].FilterEditType = (FilterEditType)6;
			grdDepartmentDetail.RootTable.Columns["InspectionStart"].FilterEditType = (FilterEditType)6;
			grdDepartmentDetail.RootTable.Columns["PackMaterialSchedule"].FilterEditType = (FilterEditType)6;
			grdDepartmentDetail.RootTable.Columns["PackMaterialStart"].FilterEditType = (FilterEditType)6;
			grdDepartmentDetail.RootTable.Columns["PlantId"].FilterEditType = (FilterEditType)1;
			grdDepartmentDetail.RootTable.Columns["PlantName"].FilterEditType = (FilterEditType)1;
			grdDepartmentDetail.RootTable.Columns["ScheduleLockId"].FilterEditType = (FilterEditType)1;
			grdDepartmentDetail.RootTable.Columns["ScheduleLockStatus"].FilterEditType = (FilterEditType)1;
			grdDepartmentDetail.RootTable.Columns["ProductionDays"].FilterEditType = (FilterEditType)1;
			grdDepartmentDetail.RootTable.Columns["FCL"].AggregateFunction = (AggregateFunction)2;
			grdDepartmentDetail.RootTable.Columns["FCL"].TextAlignment = (TextAlignment)3;
			grdDepartmentDetail.RootTable.Columns["FCL"].FormatString = "#,##0.##";
			grdDepartmentDetail.RootTable.Columns["FCL"].TotalFormatString = "#,##0.##";
			grdDepartmentDetail.RootTable.Columns["ScheduleMTon"].AggregateFunction = (AggregateFunction)2;
			grdDepartmentDetail.RootTable.Columns["ScheduleMTon"].TextAlignment = (TextAlignment)3;
			grdDepartmentDetail.RootTable.Columns["ScheduleMTon"].FormatString = "#,##0.##";
			grdDepartmentDetail.RootTable.Columns["ScheduleMTon"].TotalFormatString = "#,##0.##";
			grdDepartmentDetail.RootTable.Columns["ItemMTon"].AggregateFunction = (AggregateFunction)2;
			grdDepartmentDetail.RootTable.Columns["ItemMTon"].TextAlignment = (TextAlignment)3;
			grdDepartmentDetail.RootTable.Columns["ItemMTon"].FormatString = "#,##0.##";
			grdDepartmentDetail.RootTable.Columns["ItemMTon"].TotalFormatString = "#,##0.##";
			grdDepartmentDetail.RootTable.Columns["ProductionDays"].TextAlignment = (TextAlignment)3;
			grdDepartmentDetail.RootTable.Columns["ProductionDays"].FormatString = "#,##0.##";
			grdDepartmentDetail.RootTable.Columns["ProductionDays"].TotalFormatString = "#,##0.##";
			grdDepartmentDetail.RootTable.Columns.Add("Edit");
			grdDepartmentDetail.RootTable.Columns["Edit"].Caption = "Edit";
			grdDepartmentDetail.RootTable.Columns["Edit"].Key = "Edit";
			grdDepartmentDetail.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdDepartmentDetail.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdDepartmentDetail.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdDepartmentDetail.RootTable.Columns["Edit"].Width = 50;
			grdDepartmentDetail.RootTable.Columns["Edit"].Position = 0;
			grdDepartmentDetail.FrozenColumns = 1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdDepartmentDetail_CellFormatting(object sender, RowLoadEventArgs e)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		try
		{
			foreach (GridEXColumn item in (JanusCollectionBase)grdDepartmentDetail.RootTable.Columns)
			{
				GridEXColumn column = item;
				if (column.Key == "ProductionSchedule" || column.Key == "ProductionStart" || column.Key == "InspectionSchedule" || column.Key == "InspectionStart" || column.Key == "PackMaterialSchedule" || column.Key == "PackMaterialStart")
				{
					if (e.Row.Cells[column.Key].Value == DBNull.Value || e.Row.Cells[column.Key].Value == null)
					{
						e.Row.Cells[column.Key].Value = string.Empty;
					}
					else
					{
						e.Row.Cells[column.Key].Value = ((DateTime)e.Row.Cells[column.Key].Value).ToString("dd-MMM-yyyy");
					}
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdDepartmentDetail_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdDepartmentDetail.CurrentRow;
			if (r != null && (int)r.RowType == 0 && e.Column.Key == "Edit")
			{
				DepartmentDetailReadById();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDepartmentDetail_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdDepartmentDetail.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				DepartmentDetailReadById();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDepartmentDetail_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Expected O, but got Unknown
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdDepartmentDetail.CurrentRow;
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			int ContractScheduleId = Conversion.ToInt(r.Cells["ContractScheduleId"].Value);
			int ExImLcOrderId = Conversion.ToInt(r.Cells["ExImLcOrderId"].Value);
			int PreShipmentInspectionId = Conversion.ToInt(r.Cells["PreShipmentInspectionId"].Value);
			int JobOrderId = Conversion.ToInt(r.Cells["JobOrderId"].Value);
			int PmStartRefDocumentTypeId = Conversion.ToInt(r.Cells["PmStartRefDocumentTypeId"].Value);
			int PmStartRefId = Conversion.ToInt(r.Cells["PmStartRefId"].Value);
			int ItemId = Conversion.ToInt(r.Cells["ContractScheduleDetailItemId"].Value);
			if (e.Column.Key == "ContractNo")
			{
				if (ExImLcOrderId > 0)
				{
					CommonServices.SaleContractReportsSlipNew501(ExImLcOrderId);
				}
			}
			else if (e.Column.Key == "ScheduleCode")
			{
				if (ExImLcOrderId > 0 && ContractScheduleId > 0)
				{
					CommonServices.LcOrderShipmentScheduleLcOrderWise_Slip(ExImLcOrderId, ContractScheduleId);
				}
			}
			else if (e.Column.Key == "PackMaterialStart" && PmStartRefId > 0)
			{
				switch (PmStartRefDocumentTypeId)
				{
				case 700:
					CommonServices.PurchaseOrderPackingMaterialSlip215(PmStartRefId);
					break;
				case 245:
					CommonServices.PurchaseInvoiceDirectPM_PartySlip_245(PmStartRefId);
					break;
				case 807:
					CommonServices.StockTransferPackingMaterialAndStore_Slip415(PmStartRefId);
					break;
				case 111:
				{
					DataTable dt = InvFoodProductionReports.InvFoodProductionPackingAndOverHeadReportByJobOrder(new ReportsParameters
					{
						OrganizationId = UserAccount.OrganizationId,
						CompanyId = UserAccount.CompanyId,
						Id = PmStartRefId,
						ReportType = "PackingMaterial"
					});
					if (dt.Rows.Count == 0)
					{
						MessageBox.Show("Record Not Found For DisPlay");
						break;
					}
					Reporting val = new Reporting();
					val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
					val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
					val.ShowReportWithDataTable(dt, "606-InvFoodProductionPackingAndOverHeadReportByJobOrder.rpt");
					break;
				}
				}
			}
			else if (e.Column.Key == "InspectionStart")
			{
				if (PreShipmentInspectionId > 0)
				{
					CommonServices.InvLabPreProductionExportLotInspection_Slip(PreShipmentInspectionId);
				}
			}
			else if (e.Column.Key == "ProductionStart" && JobOrderId > 0)
			{
				CommonServices.ProductionRecoveryReport602(JobOrderId);
			}
			else if (e.Column.Key == "ItemCode" || e.Column.Key == "ItemName")
			{
				ShowItemReport(ItemId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ShowItemReport(int itemId)
	{
		List<tblUserRights> rights = clsGlobalVariables.ScreenViewReights.Where((tblUserRights x) => x.ScreenName.Trim() == "RptGenrateStocks" || x.ScreenName.Trim() == "frmStockReportWithValues").ToList();
		if (!rights.Any() || !rights.Any((tblUserRights r) => Conversion.ToBool((object)r.Value.ToString())))
		{
			MessageBox.Show("You don't have the right");
			return;
		}
		frmStockReportWithValues frmStockReportWithValues = new frmStockReportWithValues(UserAccount);
		frmStockReportWithValues.cmbItem.Value = itemId;
		frmStockReportWithValues.Show();
		frmStockReportWithValues.GridBind();
	}

	private void InsertDepartmentDetail()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		try
		{
			if (!DepartmentDetailFormvalidation())
			{
				return;
			}
			ExImLcContractScheduleDepartment obj = new ExImLcContractScheduleDepartment();
			if (RecIdDepartmentDetail > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				obj.Id = RecIdDepartmentDetail;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			if (Conversion.ToInt(CmbScheduleLockStatusDepartmentDetail.Value) != 1 || MessageBox.Show("You are about to update the schedule with lock status (Confirmed). Are you sure you want to save?\nRecords with a confirmed status will not be updated afterward.", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				if (Conversion.ToInt(CmbItemDepartmentDetail.Value) > 0)
				{
					obj.ContractScheduleId = Conversion.ToInt(CmbScheduleNoDepartmentDetail.Value);
				}
				obj.ContractScheduleDetailItemId = Conversion.ToInt(CmbItemDepartmentDetail.Value);
				obj.Weight = Conversion.ToDecimal((object)txtItemMtonDepartmentDetail.Text) * 1000m;
				obj.ProductionSchedule = Conversion.ToDateTime((object)txtProductionDateDepartmentDetail.Value);
				obj.Plant = Conversion.ToInt(CmbPlantDepartmentDetail.Value);
				obj.InspectionSchedule = Conversion.ToDateTime((object)txtInspectionDateDepartmentDetail.Value);
				obj.PackMaterialSchedule = Conversion.ToDateTime((object)txtPMDateDepartmentDetail.Value);
				obj.ScheduleLockId = Conversion.ToInt(CmbScheduleLockStatusDepartmentDetail.Value);
				obj.InspectionStart = DateTime.Now;
				obj.PackMaterialStart = DateTime.Now;
				obj.ProductionStart = DateTime.Now;
				ExImLcContractScheduleDepartment.Save(obj);
				if (RecIdDepartmentDetail == 0)
				{
					MessageBox.Show("Detail Save SuccessFully");
				}
				else
				{
					MessageBox.Show("Detail Update SuccessFully");
				}
				DepartmentDetailFormReset();
				DepartmentDetailGridFill();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void DepartmentDetailReadById()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdDepartmentDetail.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				RecIdDepartmentDetail = Conversion.ToInt(r.Cells["Id"].Value);
				if (Conversion.ToInt(r.Cells["ScheduleLockId"].Value) == 1)
				{
					throw new Exception("This Record has been Confirmed,So it can't be Updated...");
				}
				int ExImLcOrderId = Conversion.ToInt(r.Cells["ExImLcOrderId"].Value);
				string ContractNo = Conversion.ToString(r.Cells["ContractNo"].Value);
				if (ExImLcOrderId > 0)
				{
					DataTable dtExImLcOrderId = new DataTable();
					dtExImLcOrderId.Columns.Add("ExImLcOrderId", typeof(int));
					dtExImLcOrderId.Columns.Add("ContractNo");
					dtExImLcOrderId.Rows.Add(ExImLcOrderId, ContractNo);
					DDL.BindDDL(dtExImLcOrderId, CmbContractDepartmentDetail, "ExImLcOrderId", "ContractNo", "ContractNo", ZeroIndex: false);
					((UltraGridBase)CmbContractDepartmentDetail).Rows[0].Activate();
				}
				int ContractScheduleId = Conversion.ToInt(r.Cells["ContractScheduleId"].Value);
				string ScheduleCode = Conversion.ToString(r.Cells["ScheduleCode"].Value);
				if (ContractScheduleId > 0)
				{
					DataTable dtContractScheduleId = new DataTable();
					dtContractScheduleId.Columns.Add("ContractScheduleId", typeof(int));
					dtContractScheduleId.Columns.Add("ScheduleCode");
					dtContractScheduleId.Rows.Add(ContractScheduleId, ScheduleCode);
					DDL.BindDDL(dtContractScheduleId, CmbScheduleNoDepartmentDetail, "ContractScheduleId", "ScheduleCode", "ScheduleCode", ZeroIndex: false);
					((UltraGridBase)CmbScheduleNoDepartmentDetail).Rows[0].Activate();
				}
				int SupplierCustomerId = Conversion.ToInt(r.Cells["SupplierCustomerId"].Value);
				string CustomerName = Conversion.ToString(r.Cells["CustomerName"].Value);
				if (SupplierCustomerId > 0)
				{
					DataTable dtSupplierCustomerId = new DataTable();
					dtSupplierCustomerId.Columns.Add("SupplierCustomerId", typeof(int));
					dtSupplierCustomerId.Columns.Add("CustomerName");
					dtSupplierCustomerId.Rows.Add(SupplierCustomerId, CustomerName);
					DDL.BindDDL(dtSupplierCustomerId, CmbCustomerDepartmentDetail, "SupplierCustomerId", "CustomerName", "CustomerName", ZeroIndex: false);
					((UltraGridBase)CmbCustomerDepartmentDetail).Rows[0].Activate();
				}
				int DestinationPortId = Conversion.ToInt(r.Cells["DestinationPortId"].Value);
				string DestinationPort = Conversion.ToString(r.Cells["DestinationPort"].Value);
				if (DestinationPortId > 0)
				{
					DataTable dtDestinationPortId = new DataTable();
					dtDestinationPortId.Columns.Add("DestinationPortId", typeof(int));
					dtDestinationPortId.Columns.Add("DestinationPort");
					dtDestinationPortId.Rows.Add(DestinationPortId, DestinationPort);
					DDL.BindDDL(dtDestinationPortId, CmbPortDepartmentDetail, "DestinationPortId", "DestinationPort", "DestinationPort", ZeroIndex: false);
					((UltraGridBase)CmbPortDepartmentDetail).Rows[0].Activate();
				}
				txtFclDepartmentDetail.Text = Conversion.ToString(r.Cells["FCL"].Value).ToString();
				txtScheduleMtonDepartmentDetail.Text = Conversion.ToDecimal(r.Cells["ScheduleMTon"].Value).ToString("#,##.####");
				txtLoadingDateDepartmentDetail.Value = Conversion.ToDateTime(r.Cells["LoadingDate"].Value);
				int ContractScheduleDetailItemId = Conversion.ToInt(r.Cells["ContractScheduleDetailItemId"].Value);
				Conversion.ToString(r.Cells["ItemCode"].Value);
				string ItemName = Conversion.ToString(r.Cells["ItemName"].Value);
				if (ContractScheduleDetailItemId > 0)
				{
					DataTable dtItem = new DataTable();
					dtItem.Columns.Add("ContractScheduleDetailItemId", typeof(int));
					dtItem.Columns.Add("ItemName");
					dtItem.Rows.Add(ContractScheduleDetailItemId, ItemName);
					DDL.BindDDL(dtItem, CmbItemDepartmentDetail, "ContractScheduleDetailItemId", "ItemName", "ItemName", ZeroIndex: false);
					((UltraGridBase)CmbItemDepartmentDetail).Rows[0].Activate();
				}
				txtItemMtonDepartmentDetail.Text = Conversion.ToDecimal(r.Cells["ItemMTon"].Value).ToString("#,##.####");
				txtProductionDateDepartmentDetail.Value = Conversion.ToDateTime(r.Cells["ProductionSchedule"].Value);
				txtInspectionDateDepartmentDetail.Value = Conversion.ToDateTime(r.Cells["InspectionSchedule"].Value);
				txtPMDateDepartmentDetail.Value = Conversion.ToDateTime(r.Cells["PackMaterialSchedule"].Value);
				CmbPlantDepartmentDetail.Value = Conversion.ToInt(r.Cells["PlantId"].Value);
				CmbScheduleLockStatusDepartmentDetail.Value = Conversion.ToInt(r.Cells["ScheduleLockId"].Value);
				txtProductionDateDepartmentDetail_ValueChanged(null, null);
				txtPMDateDepartmentDetail_ValueChanged(null, null);
				txtInspectionDateDepartmentDetail_ValueChanged(null, null);
				txtProductionDays.Focus();
				BtnUpdateDepartmentDetail.Visible = true;
				BtnCancelDepartmentDetail.Visible = true;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BtnCancelDepartmentDetail_Click(object sender, EventArgs e)
	{
		try
		{
			DepartmentDetailFormReset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnUpdateDepartmentDetail_Click(object sender, EventArgs e)
	{
		try
		{
			InsertDepartmentDetail();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void DepartmentDetailFormReset()
	{
		try
		{
			RecIdDepartmentDetail = 0;
			BtnUpdateDepartmentDetail.Visible = false;
			BtnCancelDepartmentDetail.Visible = false;
			((UltraGridBase)CmbContractDepartmentDetail).DataSource = null;
			((Control)(object)CmbContractDepartmentDetail).Text = string.Empty;
			((UltraGridBase)CmbScheduleNoDepartmentDetail).DataSource = null;
			((Control)(object)CmbScheduleNoDepartmentDetail).Text = string.Empty;
			((UltraGridBase)CmbCustomerDepartmentDetail).DataSource = null;
			((Control)(object)CmbCustomerDepartmentDetail).Text = string.Empty;
			((UltraGridBase)CmbPortDepartmentDetail).DataSource = null;
			((Control)(object)CmbPortDepartmentDetail).Text = string.Empty;
			((UltraGridBase)CmbItemDepartmentDetail).DataSource = null;
			((Control)(object)CmbItemDepartmentDetail).Text = string.Empty;
			txtInspectionDays.Text = string.Empty;
			txtProductionDays.Text = string.Empty;
			txtPackingMaterialScheduleDays.Text = string.Empty;
			((Control)(object)CmbPlantDepartmentDetail).Text = string.Empty;
			((Control)(object)CmbScheduleLockStatusDepartmentDetail).Text = string.Empty;
			txtFclDepartmentDetail.Text = string.Empty;
			txtScheduleMtonDepartmentDetail.Text = string.Empty;
			txtItemMtonDepartmentDetail.Text = string.Empty;
			((Control)(object)grdDepartmentDetail).Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BtnNewDepartmentDetail_Click(object sender, EventArgs e)
	{
		try
		{
			DepartmentDetailFormReset();
			DepartmentDetailGridFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnRefreshDepartmentDetail_Click(object sender, EventArgs e)
	{
		try
		{
			PlantFeader();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool DepartmentDetailFormvalidation()
	{
		if (((Control)(object)CmbContractDepartmentDetail).Text == string.Empty || Conversion.ToInt(CmbContractDepartmentDetail.Value) == 0)
		{
			MessageBox.Show("Contract No is Required!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			CmbContractDepartmentDetail.Focus();
			return false;
		}
		if (((Control)(object)CmbScheduleNoDepartmentDetail).Text == string.Empty || Conversion.ToInt(CmbScheduleNoDepartmentDetail.Value) == 0)
		{
			MessageBox.Show("Schedule No is Required!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			CmbScheduleNoDepartmentDetail.Focus();
			return false;
		}
		if (((Control)(object)CmbCustomerDepartmentDetail).Text == string.Empty || Conversion.ToInt(CmbCustomerDepartmentDetail.Value) == 0)
		{
			MessageBox.Show("Customer is Required!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			CmbCustomerDepartmentDetail.Focus();
			return false;
		}
		if (((Control)(object)CmbPortDepartmentDetail).Text == string.Empty || Conversion.ToInt(CmbPortDepartmentDetail.Value) == 0)
		{
			MessageBox.Show("Port is Required!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			CmbPortDepartmentDetail.Focus();
			return false;
		}
		if (txtScheduleMtonDepartmentDetail.Text == string.Empty || Conversion.ToDouble((object)txtScheduleMtonDepartmentDetail.Text) == 0.0)
		{
			MessageBox.Show("Schedule Mton Field is Required!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			txtScheduleMtonDepartmentDetail.Focus();
			return false;
		}
		if (((Control)(object)CmbItemDepartmentDetail).Text == string.Empty || Conversion.ToInt(CmbItemDepartmentDetail.Value) == 0)
		{
			MessageBox.Show("Item is Required!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			CmbItemDepartmentDetail.Focus();
			return false;
		}
		if (txtItemMtonDepartmentDetail.Text == string.Empty || Conversion.ToDouble((object)txtItemMtonDepartmentDetail.Text) == 0.0)
		{
			MessageBox.Show("Item Mton Field is Required!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			txtItemMtonDepartmentDetail.Focus();
			return false;
		}
		if (txtProductionDays.Text == string.Empty || Conversion.ToInt((object)txtProductionDays.Text) == 0)
		{
			MessageBox.Show("Production Days is Required!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			txtProductionDays.Focus();
			return false;
		}
		if (((Control)(object)CmbPlantDepartmentDetail).Text == string.Empty || Conversion.ToInt(CmbPlantDepartmentDetail.Value) == 0)
		{
			MessageBox.Show("Plant is Required!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			CmbPlantDepartmentDetail.Focus();
			return false;
		}
		if (Conversion.CheckDateTimeNull(Conversion.ToDateTime((object)txtProductionDateDepartmentDetail.Value)))
		{
			MessageBox.Show("'Production Schedule Date' Is not Valid !", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			txtProductionDateDepartmentDetail.Focus();
			return false;
		}
		if (Conversion.ToDateTime((object)txtProductionDateDepartmentDetail.Value.Date) > Conversion.ToDateTime((object)txtLoadingDateDepartmentDetail.Value.Date))
		{
			MessageBox.Show("'Production Schedule Date' can not be Greater than 'Loading Date' !", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			txtProductionDateDepartmentDetail.Focus();
			return false;
		}
		if (txtInspectionDateDepartmentDetail.Checked && Conversion.CheckDateTimeNull(Conversion.ToDateTime((object)txtInspectionDateDepartmentDetail.Value)))
		{
			MessageBox.Show("'Inspection Schedule Date' Is not Valid !\nPlease Correct the Date or Un check the Field", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			txtInspectionDateDepartmentDetail.Focus();
			return false;
		}
		if (txtInspectionDateDepartmentDetail.Checked && Conversion.ToDateTime((object)txtInspectionDateDepartmentDetail.Value.Date) > Conversion.ToDateTime((object)txtLoadingDateDepartmentDetail.Value.Date))
		{
			MessageBox.Show("'Inspection Schedule Date' can not be Greater than 'Loading Date' !", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			txtInspectionDateDepartmentDetail.Focus();
			return false;
		}
		if (txtPMDateDepartmentDetail.Checked && Conversion.CheckDateTimeNull(Conversion.ToDateTime((object)txtPMDateDepartmentDetail.Value)))
		{
			MessageBox.Show("'Packing Material Schedule Date' Is not Valid !\nPlease Correct the Date or Un check the Field", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			txtPMDateDepartmentDetail.Focus();
			return false;
		}
		if (txtPMDateDepartmentDetail.Checked && Conversion.ToDateTime((object)txtPMDateDepartmentDetail.Value.Date) > Conversion.ToDateTime((object)txtLoadingDateDepartmentDetail.Value.Date))
		{
			MessageBox.Show("'Packing Material Schedule Date' can not be Greater than 'Loading Date' !", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			txtPMDateDepartmentDetail.Focus();
			return false;
		}
		if (((Control)(object)CmbScheduleLockStatusDepartmentDetail).Text == string.Empty || Conversion.ToInt(CmbScheduleLockStatusDepartmentDetail.Value) == 0)
		{
			MessageBox.Show("Schedule Lock Status is Required!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			CmbScheduleLockStatusDepartmentDetail.Focus();
			return false;
		}
		return true;
	}

	private void grdDepartmentDetail_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdDepartmentDetail.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdDepartmentDetail.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdDepartmentDetail.CurrentColumn.Key == "Edit")
				{
					DepartmentDetailReadById();
				}
				_ = grdDepartmentDetail.CurrentColumn.Key == "NoOfAttachments";
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				DepartmentDetailReadById();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtProductionDays_TextChanged(object sender, EventArgs e)
	{
		try
		{
			int Days = Conversion.ToInt((object)txtProductionDays.Text);
			DateTime LoadingDate = Conversion.ToDateTime((object)txtLoadingDateDepartmentDetail.Value);
			txtProductionDateDepartmentDetail.Value = LoadingDate.AddDays(-Days);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtProductionDateDepartmentDetail_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			DateTime ProductionDate = Conversion.ToDateTime((object)txtProductionDateDepartmentDetail.Value);
			if (!Conversion.CheckDateTimeNull(ProductionDate))
			{
				int Days = (int)(Conversion.ToDateTime((object)txtLoadingDateDepartmentDetail.Value).Date - ProductionDate.Date).TotalDays;
				txtProductionDays.Text = Days.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtInspectionDays_TextChanged(object sender, EventArgs e)
	{
		try
		{
			int Days = Conversion.ToInt((object)txtInspectionDays.Text);
			DateTime LoadingDate = Conversion.ToDateTime((object)txtLoadingDateDepartmentDetail.Value);
			txtInspectionDateDepartmentDetail.Value = LoadingDate.AddDays(-Days);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtInspectionDateDepartmentDetail_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			DateTime InspectionDate = Conversion.ToDateTime((object)txtInspectionDateDepartmentDetail.Value);
			if (!Conversion.CheckDateTimeNull(InspectionDate))
			{
				int Days = (int)(Conversion.ToDateTime((object)txtLoadingDateDepartmentDetail.Value).Date - InspectionDate.Date).TotalDays;
				txtInspectionDays.Text = Days.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtPackingMaterialScheduleDays_TextChanged(object sender, EventArgs e)
	{
		try
		{
			int Days = Conversion.ToInt((object)txtPackingMaterialScheduleDays.Text);
			DateTime LoadingDate = Conversion.ToDateTime((object)txtLoadingDateDepartmentDetail.Value);
			txtPMDateDepartmentDetail.Value = LoadingDate.AddDays(-Days);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtPMDateDepartmentDetail_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			DateTime PMDateDepartmentDetail = Conversion.ToDateTime((object)txtPMDateDepartmentDetail.Value);
			if (!Conversion.CheckDateTimeNull(PMDateDepartmentDetail))
			{
				int Days = (int)(Conversion.ToDateTime((object)txtLoadingDateDepartmentDetail.Value).Date - PMDateDepartmentDetail.Date).TotalDays;
				txtPackingMaterialScheduleDays.Text = Days.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtProductionDays_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtInspectionDays_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtPackingMaterialScheduleDays_KeyPress(object sender, KeyPressEventArgs e)
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

	private void BtnShowbyDates_Click(object sender, EventArgs e)
	{
		try
		{
			DepartmentDetailGridFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tztAddDays_KeyPress(object sender, KeyPressEventArgs e)
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

	private void tztAddDays_TextChanged(object sender, EventArgs e)
	{
		try
		{
			DateTime ToDate = Conversion.ToDateTime((object)txtDateFromDepart.Value).AddDays(Conversion.ToInt((object)tztAddDays.Text));
			txtDatetoDepart.Value = ToDate;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdContractShedule_Click(object sender, EventArgs e)
	{
		try
		{
			_ = Conversion.ToString(grdContractShedule.CurrentRow.Cells["ReferredStatus"].Value) == "Referred";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdContractShedule_UpdatingRecord(object sender, CancelEventArgs e)
	{
	}

	private void grdContractShedule_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdContractShedule.CurrentRow;
			if (r != null && (int)r.RowType == 0 && e.Column.Key == "NoOfAttachments")
			{
				int Id = Conversion.ToInt(r.Cells["Id"].Value);
				if (Id > 0)
				{
					CommonServices.GetSaveAndDeleteAttachmentsForRecord(UserAccount, Id, 244, "FrmExportSalesContractSchedule");
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
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Expected O, but got Unknown
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Expected O, but got Unknown
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Expected O, but got Unknown
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Expected O, but got Unknown
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Expected O, but got Unknown
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Expected O, but got Unknown
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Expected O, but got Unknown
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Expected O, but got Unknown
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Expected O, but got Unknown
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Expected O, but got Unknown
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Expected O, but got Unknown
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Expected O, but got Unknown
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Expected O, but got Unknown
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Expected O, but got Unknown
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Expected O, but got Unknown
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Expected O, but got Unknown
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Expected O, but got Unknown
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Expected O, but got Unknown
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0244: Expected O, but got Unknown
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Expected O, but got Unknown
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Expected O, but got Unknown
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Expected O, but got Unknown
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		//IL_0260: Expected O, but got Unknown
		//IL_0260: Unknown result type (might be due to invalid IL or missing references)
		//IL_0267: Expected O, but got Unknown
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Expected O, but got Unknown
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
		//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d0: Expected O, but got Unknown
		//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d7: Expected O, but got Unknown
		//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02de: Expected O, but got Unknown
		//IL_02de: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e5: Expected O, but got Unknown
		//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ec: Expected O, but got Unknown
		//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f3: Expected O, but got Unknown
		//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fa: Expected O, but got Unknown
		//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0301: Expected O, but got Unknown
		//IL_0301: Unknown result type (might be due to invalid IL or missing references)
		//IL_0308: Expected O, but got Unknown
		//IL_0308: Unknown result type (might be due to invalid IL or missing references)
		//IL_030f: Expected O, but got Unknown
		//IL_030f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0316: Expected O, but got Unknown
		//IL_0316: Unknown result type (might be due to invalid IL or missing references)
		//IL_031d: Expected O, but got Unknown
		//IL_031d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0324: Expected O, but got Unknown
		//IL_0324: Unknown result type (might be due to invalid IL or missing references)
		//IL_032b: Expected O, but got Unknown
		//IL_032b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0332: Expected O, but got Unknown
		//IL_0332: Unknown result type (might be due to invalid IL or missing references)
		//IL_0339: Expected O, but got Unknown
		//IL_0339: Unknown result type (might be due to invalid IL or missing references)
		//IL_0340: Expected O, but got Unknown
		//IL_0340: Unknown result type (might be due to invalid IL or missing references)
		//IL_0347: Expected O, but got Unknown
		//IL_0347: Unknown result type (might be due to invalid IL or missing references)
		//IL_034e: Expected O, but got Unknown
		//IL_034e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0355: Expected O, but got Unknown
		//IL_0355: Unknown result type (might be due to invalid IL or missing references)
		//IL_035c: Expected O, but got Unknown
		//IL_035c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0363: Expected O, but got Unknown
		//IL_0363: Unknown result type (might be due to invalid IL or missing references)
		//IL_036a: Expected O, but got Unknown
		//IL_036a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0371: Expected O, but got Unknown
		//IL_0371: Unknown result type (might be due to invalid IL or missing references)
		//IL_0378: Expected O, but got Unknown
		//IL_0378: Unknown result type (might be due to invalid IL or missing references)
		//IL_037f: Expected O, but got Unknown
		//IL_037f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0386: Expected O, but got Unknown
		//IL_0386: Unknown result type (might be due to invalid IL or missing references)
		//IL_038d: Expected O, but got Unknown
		//IL_038d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0394: Expected O, but got Unknown
		//IL_0394: Unknown result type (might be due to invalid IL or missing references)
		//IL_039b: Expected O, but got Unknown
		//IL_039b: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a2: Expected O, but got Unknown
		//IL_03a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a9: Expected O, but got Unknown
		//IL_03a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b0: Expected O, but got Unknown
		//IL_03b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b7: Expected O, but got Unknown
		//IL_03b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03be: Expected O, but got Unknown
		//IL_03be: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c5: Expected O, but got Unknown
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
		//IL_04c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cf: Expected O, but got Unknown
		//IL_04cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d6: Expected O, but got Unknown
		//IL_04d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04dd: Expected O, but got Unknown
		//IL_04dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e4: Expected O, but got Unknown
		//IL_04e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04eb: Expected O, but got Unknown
		//IL_04eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f2: Expected O, but got Unknown
		//IL_04f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f9: Expected O, but got Unknown
		//IL_04f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0500: Expected O, but got Unknown
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
		//IL_0569: Unknown result type (might be due to invalid IL or missing references)
		//IL_0573: Expected O, but got Unknown
		//IL_05ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f7: Expected O, but got Unknown
		//IL_05f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0602: Expected O, but got Unknown
		//IL_0603: Unknown result type (might be due to invalid IL or missing references)
		//IL_060d: Expected O, but got Unknown
		//IL_067c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0686: Expected O, but got Unknown
		//IL_0692: Unknown result type (might be due to invalid IL or missing references)
		//IL_069c: Expected O, but got Unknown
		//IL_0700: Unknown result type (might be due to invalid IL or missing references)
		//IL_070a: Expected O, but got Unknown
		//IL_0779: Unknown result type (might be due to invalid IL or missing references)
		//IL_0783: Expected O, but got Unknown
		//IL_0855: Unknown result type (might be due to invalid IL or missing references)
		//IL_085f: Expected O, but got Unknown
		//IL_088c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0896: Expected O, but got Unknown
		//IL_08c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_08cd: Expected O, but got Unknown
		//IL_093c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0946: Expected O, but got Unknown
		//IL_0989: Unknown result type (might be due to invalid IL or missing references)
		//IL_0993: Expected O, but got Unknown
		//IL_09b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_09bf: Expected O, but got Unknown
		//IL_09e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_09eb: Expected O, but got Unknown
		//IL_0a4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a59: Expected O, but got Unknown
		//IL_0b36: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b40: Expected O, but got Unknown
		//IL_0b41: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b4b: Expected O, but got Unknown
		//IL_0b4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b56: Expected O, but got Unknown
		//IL_0b57: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b61: Expected O, but got Unknown
		//IL_0c12: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c1c: Expected O, but got Unknown
		//IL_0c28: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c32: Expected O, but got Unknown
		//IL_0c3e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c48: Expected O, but got Unknown
		//IL_13b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_13c0: Expected O, but got Unknown
		//IL_3d82: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d8c: Expected O, but got Unknown
		//IL_3db0: Unknown result type (might be due to invalid IL or missing references)
		//IL_3dba: Expected O, but got Unknown
		//IL_3dc7: Unknown result type (might be due to invalid IL or missing references)
		//IL_3dd1: Expected O, but got Unknown
		//IL_4495: Unknown result type (might be due to invalid IL or missing references)
		//IL_449f: Expected O, but got Unknown
		//IL_5723: Unknown result type (might be due to invalid IL or missing references)
		//IL_572d: Expected O, but got Unknown
		//IL_5dc5: Unknown result type (might be due to invalid IL or missing references)
		//IL_5dcf: Expected O, but got Unknown
		//IL_5ddc: Unknown result type (might be due to invalid IL or missing references)
		//IL_5de6: Expected O, but got Unknown
		//IL_7859: Unknown result type (might be due to invalid IL or missing references)
		//IL_7863: Expected O, but got Unknown
		//IL_7870: Unknown result type (might be due to invalid IL or missing references)
		//IL_787a: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance165 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance168 = new Appearance();
		Appearance appearance169 = new Appearance();
		Appearance appearance171 = new Appearance();
		Appearance appearance172 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance180 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
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
		GridEXLayout grdContractShedule_DesignTimeLayout = new GridEXLayout();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Export.frmSaleContractSchedule));
		GridEXLayout grdPendingContract_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdHistoryScheduleDetailByContact_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdHistoryScheduleByContract_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdHistoryMainByContract_DesignTimeLayout = new GridEXLayout();
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
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
		GridEXLayout grdDepartmentDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance1 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance157 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance164 = new Appearance();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel9 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.panel16 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel15 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.label42 = new System.Windows.Forms.Label();
		this.txtInnerQty = new System.Windows.Forms.TextBox();
		this.label38 = new System.Windows.Forms.Label();
		this.txtMtonOfSelectedDate = new System.Windows.Forms.TextBox();
		this.rdbtnItemCode = new System.Windows.Forms.RadioButton();
		this.txtBalMton = new System.Windows.Forms.TextBox();
		this.rdbtnItemName = new System.Windows.Forms.RadioButton();
		this.cmbCropYear = new UltraCombo();
		this.cmbPackUom = new UltraCombo();
		this.cmbPackingtype = new UltraCombo();
		this.btnAdd = new System.Windows.Forms.Button();
		this.btnCancel = new System.Windows.Forms.Button();
		this.btnGridUpdate = new System.Windows.Forms.Button();
		this.label9 = new System.Windows.Forms.Label();
		this.txtMTonDetail = new System.Windows.Forms.TextBox();
		this.label8 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.txtNoOfBagsDetail = new System.Windows.Forms.TextBox();
		this.label7 = new System.Windows.Forms.Label();
		this.cmbAttentiveLoadDateBySchedule = new UltraCombo();
		this.label15 = new System.Windows.Forms.Label();
		this.cmbItem = new UltraCombo();
		this.label6 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNewDetailSchedule = new System.Windows.Forms.ToolStripButton();
		this.BtnRefreshScheduleDetail = new System.Windows.Forms.ToolStripButton();
		this.btnSavedetailSchedule = new System.Windows.Forms.ToolStripButton();
		this.BtnContractPmDetailForm = new System.Windows.Forms.ToolStripButton();
		this.label18 = new System.Windows.Forms.Label();
		this.panel12 = new System.Windows.Forms.Panel();
		this.grdContractShedule = new GridEX();
		this.panel13 = new System.Windows.Forms.Panel();
		this.label14 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.BtnRefreshScheduleMain = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnattachment = new System.Windows.Forms.ToolStripButton();
		this.BtnShipmentDocumentForm = new System.Windows.Forms.ToolStripButton();
		this.panel10 = new System.Windows.Forms.Panel();
		this.grdPendingContract = new GridEX();
		this.panel11 = new System.Windows.Forms.Panel();
		this.rdAll = new System.Windows.Forms.RadioButton();
		this.rdReffered = new System.Windows.Forms.RadioButton();
		this.rdNotReffered = new System.Windows.Forms.RadioButton();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label13 = new System.Windows.Forms.Label();
		this.panel14 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.BtnNewMain = new System.Windows.Forms.ToolStripButton();
		this.BtnLoadAllPendingContracts = new System.Windows.Forms.ToolStripButton();
		this.BtnContractWiseSlip = new System.Windows.Forms.ToolStripButton();
		this.BtnShortCutkeys = new System.Windows.Forms.ToolStripButton();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label12 = new System.Windows.Forms.Label();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panelMainhistory = new System.Windows.Forms.Panel();
		this.panel7 = new System.Windows.Forms.Panel();
		this.panel19 = new System.Windows.Forms.Panel();
		this.panel21 = new System.Windows.Forms.Panel();
		this.grdHistoryScheduleDetailByContact = new GridEX();
		this.panel23 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar6 = new grid.User_Conrtols.CtrlGrdBar();
		this.label4 = new System.Windows.Forms.Label();
		this.panel20 = new System.Windows.Forms.Panel();
		this.grdHistoryScheduleByContract = new GridEX();
		this.panel22 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar5 = new grid.User_Conrtols.CtrlGrdBar();
		this.label2 = new System.Windows.Forms.Label();
		this.panel17 = new System.Windows.Forms.Panel();
		this.grdHistoryMainByContract = new GridEX();
		this.panel18 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar4 = new grid.User_Conrtols.CtrlGrdBar();
		this.label3 = new System.Windows.Forms.Label();
		this.ToolstripHistoryContract = new System.Windows.Forms.ToolStrip();
		this.LoadAllgrdContract = new System.Windows.Forms.ToolStripButton();
		this.panel8 = new System.Windows.Forms.Panel();
		this.label1 = new System.Windows.Forms.Label();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.panel26 = new System.Windows.Forms.Panel();
		this.panel27 = new System.Windows.Forms.Panel();
		this.DataGridHistory = new GridEX();
		this.panel24 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar7 = new grid.User_Conrtols.CtrlGrdBar();
		this.label5 = new System.Windows.Forms.Label();
		this.panel25 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.label10 = new System.Windows.Forms.Label();
		this.cmbPort = new UltraCombo();
		this.txtdateto = new System.Windows.Forms.DateTimePicker();
		this.label50 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.cmbSupplierName = new UltraCombo();
		this.txtdatefrom = new System.Windows.Forms.DateTimePicker();
		this.label19 = new System.Windows.Forms.Label();
		this.label51 = new System.Windows.Forms.Label();
		this.CmbItemHistory = new UltraCombo();
		this.btnShow = new System.Windows.Forms.Button();
		this.toolStrip4 = new System.Windows.Forms.ToolStrip();
		this.btnLoadAllHistoryNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistoryCombos = new System.Windows.Forms.ToolStripButton();
		this.BtnHistoryPrint = new System.Windows.Forms.ToolStripButton();
		this.BtnPrint551_01 = new System.Windows.Forms.ToolStripButton();
		this.tabControlMain = new System.Windows.Forms.TabControl();
		this.tabPageMain = new System.Windows.Forms.TabPage();
		this.tabPageDepartmentStatus = new System.Windows.Forms.TabPage();
		this.grdDepartmentDetail = new GridEX();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel28 = new System.Windows.Forms.Panel();
		this.BtnShowbyDates = new System.Windows.Forms.Button();
		this.tztAddDays = new System.Windows.Forms.TextBox();
		this.txtDateFromDepart = new System.Windows.Forms.DateTimePicker();
		this.label39 = new System.Windows.Forms.Label();
		this.txtDatetoDepart = new System.Windows.Forms.DateTimePicker();
		this.label40 = new System.Windows.Forms.Label();
		this.label41 = new System.Windows.Forms.Label();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.label37 = new System.Windows.Forms.Label();
		this.txtPackingMaterialScheduleDays = new System.Windows.Forms.TextBox();
		this.label36 = new System.Windows.Forms.Label();
		this.txtInspectionDays = new System.Windows.Forms.TextBox();
		this.label35 = new System.Windows.Forms.Label();
		this.txtProductionDays = new System.Windows.Forms.TextBox();
		this.txtPMDateDepartmentDetail = new System.Windows.Forms.DateTimePicker();
		this.txtInspectionDateDepartmentDetail = new System.Windows.Forms.DateTimePicker();
		this.txtProductionDateDepartmentDetail = new System.Windows.Forms.DateTimePicker();
		this.txtLoadingDateDepartmentDetail = new System.Windows.Forms.DateTimePicker();
		this.CmbCustomerDepartmentDetail = new UltraCombo();
		this.CmbScheduleLockStatusDepartmentDetail = new UltraCombo();
		this.CmbItemDepartmentDetail = new UltraCombo();
		this.CmbPortDepartmentDetail = new UltraCombo();
		this.BtnCancelDepartmentDetail = new System.Windows.Forms.Button();
		this.BtnUpdateDepartmentDetail = new System.Windows.Forms.Button();
		this.label20 = new System.Windows.Forms.Label();
		this.label32 = new System.Windows.Forms.Label();
		this.txtItemMtonDepartmentDetail = new System.Windows.Forms.TextBox();
		this.label31 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.label30 = new System.Windows.Forms.Label();
		this.label22 = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.label28 = new System.Windows.Forms.Label();
		this.label23 = new System.Windows.Forms.Label();
		this.label29 = new System.Windows.Forms.Label();
		this.txtScheduleMtonDepartmentDetail = new System.Windows.Forms.TextBox();
		this.txtFclDepartmentDetail = new System.Windows.Forms.TextBox();
		this.label24 = new System.Windows.Forms.Label();
		this.CmbContractDepartmentDetail = new UltraCombo();
		this.label25 = new System.Windows.Forms.Label();
		this.CmbPlantDepartmentDetail = new UltraCombo();
		this.label34 = new System.Windows.Forms.Label();
		this.CmbScheduleNoDepartmentDetail = new UltraCombo();
		this.label26 = new System.Windows.Forms.Label();
		this.panel29 = new System.Windows.Forms.Panel();
		this.toolStrip5 = new System.Windows.Forms.ToolStrip();
		this.BtnNewDepartmentDetail = new System.Windows.Forms.ToolStripButton();
		this.BtnRefreshDepartmentDetail = new System.Windows.Forms.ToolStripButton();
		this.label27 = new System.Windows.Forms.Label();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel9.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel16.SuspendLayout();
		this.panel15.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbCropYear).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPackUom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPackingtype).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbAttentiveLoadDateBySchedule).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).BeginInit();
		this.panel5.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		this.panel12.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdContractShedule).BeginInit();
		this.panel13.SuspendLayout();
		this.panel3.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.panel10.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPendingContract).BeginInit();
		this.panel11.SuspendLayout();
		this.panel14.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		this.panel6.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panelMainhistory.SuspendLayout();
		this.panel7.SuspendLayout();
		this.panel19.SuspendLayout();
		this.panel21.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryScheduleDetailByContact).BeginInit();
		this.panel23.SuspendLayout();
		this.panel20.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryScheduleByContract).BeginInit();
		this.panel22.SuspendLayout();
		this.panel17.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryMainByContract).BeginInit();
		this.panel18.SuspendLayout();
		this.ToolstripHistoryContract.SuspendLayout();
		this.panel8.SuspendLayout();
		this.tabPage3.SuspendLayout();
		this.panel26.SuspendLayout();
		this.panel27.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel24.SuspendLayout();
		this.panel25.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbPort).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSupplierName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemHistory).BeginInit();
		this.toolStrip4.SuspendLayout();
		this.tabControlMain.SuspendLayout();
		this.tabPageMain.SuspendLayout();
		this.tabPageDepartmentStatus.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDepartmentDetail).BeginInit();
		this.panel28.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerDepartmentDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbScheduleLockStatusDepartmentDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemDepartmentDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPortDepartmentDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbContractDepartmentDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPlantDepartmentDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbScheduleNoDepartmentDetail).BeginInit();
		this.panel29.SuspendLayout();
		this.toolStrip5.SuspendLayout();
		base.SuspendLayout();
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Controls.Add(this.tabPage3);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(3, 3);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1605, 664);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1597, 635);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add(this.panel9);
		this.panel2.Controls.Add(this.panel6);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1591, 629);
		this.panel2.TabIndex = 0;
		this.panel9.Controls.Add(this.panel4);
		this.panel9.Controls.Add(this.panel12);
		this.panel9.Controls.Add(this.panel10);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel9.Location = new System.Drawing.Point(0, 27);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1591, 602);
		this.panel9.TabIndex = 4;
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel4.Controls.Add(this.panel16);
		this.panel4.Controls.Add(this.panel15);
		this.panel4.Controls.Add(this.panel5);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(415, 306);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1176, 296);
		this.panel4.TabIndex = 5;
		this.grd.AllowEdit = (InheritableBoolean)2;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.DynamicFiltering = true;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 156);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1176, 140);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grd_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		this.panel16.BackColor = System.Drawing.Color.Teal;
		this.panel16.Controls.Add(this.ctrlGrdBar3);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel16.Location = new System.Drawing.Point(0, 133);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(1176, 23);
		this.panel16.TabIndex = 4;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(1136, 1);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
		this.ctrlGrdBar3.MyGrid = this.grd;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar3.TabIndex = 56;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.panel15.Controls.Add(this.groupBox1);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.Location = new System.Drawing.Point(0, 50);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(1176, 83);
		this.panel15.TabIndex = 3;
		this.groupBox1.Controls.Add(this.label42);
		this.groupBox1.Controls.Add(this.txtInnerQty);
		this.groupBox1.Controls.Add(this.label38);
		this.groupBox1.Controls.Add(this.txtMtonOfSelectedDate);
		this.groupBox1.Controls.Add(this.rdbtnItemCode);
		this.groupBox1.Controls.Add(this.txtBalMton);
		this.groupBox1.Controls.Add(this.rdbtnItemName);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCropYear);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPackUom);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPackingtype);
		this.groupBox1.Controls.Add(this.btnAdd);
		this.groupBox1.Controls.Add(this.btnCancel);
		this.groupBox1.Controls.Add(this.btnGridUpdate);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Controls.Add(this.txtMTonDetail);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add(this.label16);
		this.groupBox1.Controls.Add(this.label17);
		this.groupBox1.Controls.Add(this.txtNoOfBagsDetail);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbAttentiveLoadDateBySchedule);
		this.groupBox1.Controls.Add(this.label15);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItem);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(4, 2);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1005, 74);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Detail";
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label42.Location = new System.Drawing.Point(635, 23);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(57, 15);
		this.label42.TabIndex = 132;
		this.label42.Text = "Bal MTon";
		this.txtInnerQty.Enabled = false;
		this.txtInnerQty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtInnerQty.Location = new System.Drawing.Point(794, 43);
		this.txtInnerQty.Name = "txtInnerQty";
		this.txtInnerQty.Size = new System.Drawing.Size(74, 23);
		this.txtInnerQty.TabIndex = 4;
		this.txtInnerQty.Tag = "NoOfBags";
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.Location = new System.Drawing.Point(794, 23);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(57, 15);
		this.label38.TabIndex = 67;
		this.label38.Text = "Inner Qty";
		this.txtMtonOfSelectedDate.Enabled = false;
		this.txtMtonOfSelectedDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtMtonOfSelectedDate.Location = new System.Drawing.Point(870, 15);
		this.txtMtonOfSelectedDate.Name = "txtMtonOfSelectedDate";
		this.txtMtonOfSelectedDate.Size = new System.Drawing.Size(129, 23);
		this.txtMtonOfSelectedDate.TabIndex = 4;
		this.txtMtonOfSelectedDate.WordWrap = false;
		this.rdbtnItemCode.AutoSize = true;
		this.rdbtnItemCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdbtnItemCode.Location = new System.Drawing.Point(251, 21);
		this.rdbtnItemCode.Margin = new System.Windows.Forms.Padding(2);
		this.rdbtnItemCode.Name = "rdbtnItemCode";
		this.rdbtnItemCode.Size = new System.Drawing.Size(52, 19);
		this.rdbtnItemCode.TabIndex = 130;
		this.rdbtnItemCode.Text = "Code";
		this.rdbtnItemCode.UseVisualStyleBackColor = true;
		this.txtBalMton.Enabled = false;
		this.txtBalMton.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBalMton.Location = new System.Drawing.Point(635, 43);
		this.txtBalMton.Name = "txtBalMton";
		this.txtBalMton.Size = new System.Drawing.Size(74, 23);
		this.txtBalMton.TabIndex = 131;
		this.txtBalMton.Tag = "NoOfBags";
		this.rdbtnItemName.AutoSize = true;
		this.rdbtnItemName.Checked = true;
		this.rdbtnItemName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdbtnItemName.Location = new System.Drawing.Point(198, 21);
		this.rdbtnItemName.Margin = new System.Windows.Forms.Padding(2);
		this.rdbtnItemName.Name = "rdbtnItemName";
		this.rdbtnItemName.Size = new System.Drawing.Size(57, 19);
		this.rdbtnItemName.TabIndex = 129;
		this.rdbtnItemName.TabStop = true;
		this.rdbtnItemName.Text = "Name";
		this.rdbtnItemName.UseVisualStyleBackColor = true;
		this.rdbtnItemName.CheckedChanged += new System.EventHandler(rdbtnItemName_CheckedChanged);
		this.cmbCropYear.AllowNull = (DefaultableBoolean)1;
		this.cmbCropYear.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCropYear.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCropYear.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance165).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Appearance = (AppearanceBase)(object)appearance165;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance166).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance166).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance166).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance166).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCropYear).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance166;
		((AppearanceBase)appearance167).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance167;
		((SpecialBoxBase)((UltraGridBase)this.cmbCropYear).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance168).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance168).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance168).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance169).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance169).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance169;
		((AppearanceBase)appearance171).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance171).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance171;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance172;
		((AppearanceBase)appearance173).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance173).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance173;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance174).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance174).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance174).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance174).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance174).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance174;
		((AppearanceBase)appearance175).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance175;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance176).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance176).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance176;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance177;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCropYear).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbCropYear.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCropYear).Location = new System.Drawing.Point(304, 41);
		((System.Windows.Forms.Control)(object)this.cmbCropYear).Name = "cmbCropYear";
		((System.Windows.Forms.Control)(object)this.cmbCropYear).Size = new System.Drawing.Size(87, 26);
		((System.Windows.Forms.Control)(object)this.cmbCropYear).TabIndex = 2;
		this.cmbPackUom.AllowNull = (DefaultableBoolean)1;
		this.cmbPackUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPackUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPackUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance178).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance178;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance179).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance179).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance179).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPackUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance179;
		((AppearanceBase)appearance180).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance180;
		((SpecialBoxBase)((UltraGridBase)this.cmbPackUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPackUom).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbPackUom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPackUom).Location = new System.Drawing.Point(561, 41);
		((System.Windows.Forms.Control)(object)this.cmbPackUom).Name = "cmbPackUom";
		((System.Windows.Forms.Control)(object)this.cmbPackUom).Size = new System.Drawing.Size(72, 26);
		((System.Windows.Forms.Control)(object)this.cmbPackUom).TabIndex = 5;
		((System.Windows.Forms.Control)(object)this.cmbPackUom).TextChanged += new System.EventHandler(txtNoOfBagsDetail_TextChanged);
		this.cmbPackingtype.AllowNull = (DefaultableBoolean)1;
		this.cmbPackingtype.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPackingtype.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPackingtype.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPackingtype).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.cmbPackingtype).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPackingtype).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPackingtype).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbPackingtype.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPackingtype).Location = new System.Drawing.Point(392, 41);
		((System.Windows.Forms.Control)(object)this.cmbPackingtype).Name = "cmbPackingtype";
		((System.Windows.Forms.Control)(object)this.cmbPackingtype).Size = new System.Drawing.Size(93, 26);
		((System.Windows.Forms.Control)(object)this.cmbPackingtype).TabIndex = 3;
		this.btnAdd.BackColor = System.Drawing.Color.Teal;
		this.btnAdd.FlatAppearance.BorderColor = System.Drawing.Color.Green;
		this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAdd.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAdd.ForeColor = System.Drawing.Color.White;
		this.btnAdd.Location = new System.Drawing.Point(870, 42);
		this.btnAdd.Name = "btnAdd";
		this.btnAdd.Size = new System.Drawing.Size(35, 25);
		this.btnAdd.TabIndex = 7;
		this.btnAdd.Text = "+";
		this.btnAdd.UseVisualStyleBackColor = false;
		this.btnAdd.Click += new System.EventHandler(btnAdd_Click);
		this.btnAdd.Leave += new System.EventHandler(btnAdd_Leave);
		this.btnCancel.BackColor = System.Drawing.Color.Teal;
		this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.Green;
		this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancel.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnCancel.ForeColor = System.Drawing.Color.White;
		this.btnCancel.Location = new System.Drawing.Point(937, 42);
		this.btnCancel.Name = "btnCancel";
		this.btnCancel.Size = new System.Drawing.Size(63, 25);
		this.btnCancel.TabIndex = 8;
		this.btnCancel.Text = "Cancel";
		this.btnCancel.UseVisualStyleBackColor = false;
		this.btnCancel.Visible = false;
		this.btnCancel.Click += new System.EventHandler(btnCancel_Click);
		this.btnCancel.Leave += new System.EventHandler(btnAdd_Leave);
		this.btnGridUpdate.BackColor = System.Drawing.Color.Teal;
		this.btnGridUpdate.FlatAppearance.BorderColor = System.Drawing.Color.Green;
		this.btnGridUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnGridUpdate.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnGridUpdate.ForeColor = System.Drawing.Color.White;
		this.btnGridUpdate.Location = new System.Drawing.Point(870, 42);
		this.btnGridUpdate.Name = "btnGridUpdate";
		this.btnGridUpdate.Size = new System.Drawing.Size(66, 25);
		this.btnGridUpdate.TabIndex = 7;
		this.btnGridUpdate.Text = "Update";
		this.btnGridUpdate.UseVisualStyleBackColor = false;
		this.btnGridUpdate.Visible = false;
		this.btnGridUpdate.Click += new System.EventHandler(btnGridUpdate_Click);
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(704, 23);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(91, 15);
		this.label9.TabIndex = 69;
		this.label9.Text = "Weight / M.Ton";
		this.txtMTonDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtMTonDetail.Location = new System.Drawing.Point(710, 43);
		this.txtMTonDetail.Name = "txtMTonDetail";
		this.txtMTonDetail.Size = new System.Drawing.Size(83, 23);
		this.txtMTonDetail.TabIndex = 6;
		this.txtMTonDetail.Tag = "WeightMton";
		this.txtMTonDetail.TextChanged += new System.EventHandler(txtMTonDetail_TextChanged);
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(486, 23);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(67, 15);
		this.label8.TabIndex = 67;
		this.label8.Text = "No Of Bags";
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(304, 23);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(57, 15);
		this.label16.TabIndex = 65;
		this.label16.Text = "Crop Year";
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(561, 23);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(61, 15);
		this.label17.TabIndex = 65;
		this.label17.Text = "Pack Uom";
		this.txtNoOfBagsDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNoOfBagsDetail.Location = new System.Drawing.Point(486, 43);
		this.txtNoOfBagsDetail.Name = "txtNoOfBagsDetail";
		this.txtNoOfBagsDetail.Size = new System.Drawing.Size(74, 23);
		this.txtNoOfBagsDetail.TabIndex = 4;
		this.txtNoOfBagsDetail.Tag = "NoOfBags";
		this.txtNoOfBagsDetail.TextChanged += new System.EventHandler(txtNoOfBagsDetail_TextChanged);
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(392, 23);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(77, 15);
		this.label7.TabIndex = 65;
		this.label7.Text = "Packing Type";
		this.cmbAttentiveLoadDateBySchedule.AllowNull = (DefaultableBoolean)1;
		this.cmbAttentiveLoadDateBySchedule.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbAttentiveLoadDateBySchedule.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbAttentiveLoadDateBySchedule.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbAttentiveLoadDateBySchedule).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbAttentiveLoadDateBySchedule).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbAttentiveLoadDateBySchedule).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbAttentiveLoadDateBySchedule).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbAttentiveLoadDateBySchedule).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.cmbAttentiveLoadDateBySchedule).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbAttentiveLoadDateBySchedule).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbAttentiveLoadDateBySchedule).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbAttentiveLoadDateBySchedule).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbAttentiveLoadDateBySchedule).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbAttentiveLoadDateBySchedule).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.cmbAttentiveLoadDateBySchedule).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbAttentiveLoadDateBySchedule).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbAttentiveLoadDateBySchedule).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance33).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbAttentiveLoadDateBySchedule).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.cmbAttentiveLoadDateBySchedule).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbAttentiveLoadDateBySchedule).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbAttentiveLoadDateBySchedule).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbAttentiveLoadDateBySchedule).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.cmbAttentiveLoadDateBySchedule).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbAttentiveLoadDateBySchedule).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbAttentiveLoadDateBySchedule).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.cmbAttentiveLoadDateBySchedule).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbAttentiveLoadDateBySchedule).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.cmbAttentiveLoadDateBySchedule).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbAttentiveLoadDateBySchedule).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbAttentiveLoadDateBySchedule).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbAttentiveLoadDateBySchedule).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbAttentiveLoadDateBySchedule.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbAttentiveLoadDateBySchedule).Location = new System.Drawing.Point(3, 41);
		((System.Windows.Forms.Control)(object)this.cmbAttentiveLoadDateBySchedule).Name = "cmbAttentiveLoadDateBySchedule";
		((System.Windows.Forms.Control)(object)this.cmbAttentiveLoadDateBySchedule).Size = new System.Drawing.Size(124, 26);
		((System.Windows.Forms.Control)(object)this.cmbAttentiveLoadDateBySchedule).TabIndex = 0;
		this.cmbAttentiveLoadDateBySchedule.ValueChanged += new System.EventHandler(cmbAttentiveLoadDateBySchedule_ValueChanged);
		((System.Windows.Forms.Control)(object)this.cmbAttentiveLoadDateBySchedule).Leave += new System.EventHandler(cmbAttentiveLoadDateBySchedule_ValueChanged);
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(3, 23);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(78, 15);
		this.label15.TabIndex = 65;
		this.label15.Text = "Loading Date";
		this.cmbItem.AllowNull = (DefaultableBoolean)1;
		this.cmbItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance41;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItem).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItem).Location = new System.Drawing.Point(129, 41);
		((System.Windows.Forms.Control)(object)this.cmbItem).Name = "cmbItem";
		((System.Windows.Forms.Control)(object)this.cmbItem).Size = new System.Drawing.Size(174, 26);
		((System.Windows.Forms.Control)(object)this.cmbItem).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.cmbItem).Leave += new System.EventHandler(cmbItem_Leave);
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(124, 23);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(74, 15);
		this.label6.TabIndex = 65;
		this.label6.Text = "Item / Brand";
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.toolStrip3);
		this.panel5.Controls.Add(this.label18);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1176, 50);
		this.panel5.TabIndex = 2;
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnNewDetailSchedule, this.BtnRefreshScheduleDetail, this.btnSavedetailSchedule, this.BtnContractPmDetailForm });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1176, 27);
		this.toolStrip3.TabIndex = 55;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNewDetailSchedule.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNewDetailSchedule.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewDetailSchedule.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewDetailSchedule.Name = "btnNewDetailSchedule";
		this.btnNewDetailSchedule.Size = new System.Drawing.Size(59, 24);
		this.btnNewDetailSchedule.Text = "&New";
		this.btnNewDetailSchedule.Click += new System.EventHandler(btnNewDetailSchedule_Click);
		this.BtnRefreshScheduleDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnRefreshScheduleDetail.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.BtnRefreshScheduleDetail.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefreshScheduleDetail.Name = "BtnRefreshScheduleDetail";
		this.BtnRefreshScheduleDetail.Size = new System.Drawing.Size(80, 24);
		this.BtnRefreshScheduleDetail.Text = "&Refresh";
		this.BtnRefreshScheduleDetail.Click += new System.EventHandler(BtnRefreshScheduleDetail_Click);
		this.btnSavedetailSchedule.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSavedetailSchedule.Image = (System.Drawing.Image)resources.GetObject("btnSavedetailSchedule.Image");
		this.btnSavedetailSchedule.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSavedetailSchedule.Name = "btnSavedetailSchedule";
		this.btnSavedetailSchedule.Size = new System.Drawing.Size(117, 24);
		this.btnSavedetailSchedule.Text = "&Save/Update";
		this.btnSavedetailSchedule.Click += new System.EventHandler(btnSavedetailSchedule_Click);
		this.BtnContractPmDetailForm.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.BtnContractPmDetailForm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnContractPmDetailForm.Image = (System.Drawing.Image)resources.GetObject("BtnContractPmDetailForm.Image");
		this.BtnContractPmDetailForm.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnContractPmDetailForm.Name = "BtnContractPmDetailForm";
		this.BtnContractPmDetailForm.Size = new System.Drawing.Size(172, 24);
		this.BtnContractPmDetailForm.Text = "Contract Pm Detail Form";
		this.BtnContractPmDetailForm.Click += new System.EventHandler(BtnContractPmDetailForm_Click);
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label18.ForeColor = System.Drawing.Color.White;
		this.label18.Location = new System.Drawing.Point(6, 30);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(352, 17);
		this.label18.TabIndex = 54;
		this.label18.Text = "Contract Schedule Brand Detail Loading Date wise";
		this.panel12.Controls.Add((System.Windows.Forms.Control)(object)this.grdContractShedule);
		this.panel12.Controls.Add(this.panel13);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(415, 0);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1176, 306);
		this.panel12.TabIndex = 3;
		grdContractShedule_DesignTimeLayout.LayoutString = resources.GetString("grdContractShedule_DesignTimeLayout.LayoutString");
		this.grdContractShedule.DesignTimeLayout = grdContractShedule_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdContractShedule).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdContractShedule.DynamicFiltering = true;
		this.grdContractShedule.FilterMode = (FilterMode)2;
		this.grdContractShedule.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdContractShedule).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdContractShedule.GroupByBoxVisible = false;
		this.grdContractShedule.GroupTotals = (GroupTotals)3;
		((System.Windows.Forms.Control)(object)this.grdContractShedule).Location = new System.Drawing.Point(0, 51);
		((System.Windows.Forms.Control)(object)this.grdContractShedule).Name = "grdContractShedule";
		((ScrollableBase)this.grdContractShedule).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdContractShedule).Size = new System.Drawing.Size(1176, 255);
		((System.Windows.Forms.Control)(object)this.grdContractShedule).TabIndex = 0;
		this.grdContractShedule.TotalRow = (InheritableBoolean)1;
		this.grdContractShedule.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdContractShedule).VisualStyle = (VisualStyle)4;
		this.grdContractShedule.CellUpdated += new ColumnActionEventHandler(grdContractShedule_CellUpdated);
		this.grdContractShedule.UpdatingRecord += new System.ComponentModel.CancelEventHandler(grdContractShedule_UpdatingRecord);
		this.grdContractShedule.ColumnButtonClick += new ColumnActionEventHandler(grdContractShedule_ColumnButtonClick);
		this.grdContractShedule.LinkClicked += new ColumnActionEventHandler(grdContractShedule_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdContractShedule).Click += new System.EventHandler(grdContractShedule_Click);
		((System.Windows.Forms.Control)(object)this.grdContractShedule).KeyDown += new System.Windows.Forms.KeyEventHandler(grdContractShedule_KeyDown);
		this.panel13.BackColor = System.Drawing.Color.Teal;
		this.panel13.Controls.Add(this.label14);
		this.panel13.Controls.Add(this.panel3);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel13.Location = new System.Drawing.Point(0, 0);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1176, 51);
		this.panel13.TabIndex = 6;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label14.ForeColor = System.Drawing.Color.White;
		this.label14.Location = new System.Drawing.Point(3, 30);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(135, 17);
		this.label14.TabIndex = 54;
		this.label14.Text = "Contract Schedule";
		this.panel3.Controls.Add(this.toolStrip1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1176, 27);
		this.panel3.TabIndex = 0;
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.btnnew, this.BtnRefreshScheduleMain, this.btnsave, this.btnattachment, this.BtnShipmentDocumentForm });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1176, 27);
		this.toolStrip1.TabIndex = 7;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.BtnRefreshScheduleMain.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnRefreshScheduleMain.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.BtnRefreshScheduleMain.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefreshScheduleMain.Name = "BtnRefreshScheduleMain";
		this.BtnRefreshScheduleMain.Size = new System.Drawing.Size(80, 24);
		this.BtnRefreshScheduleMain.Text = "&Refresh";
		this.BtnRefreshScheduleMain.Click += new System.EventHandler(BtnRefreshScheduleMain_Click);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(117, 24);
		this.btnsave.Text = "&Save/Update";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnattachment.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnattachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnattachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnattachment.Name = "btnattachment";
		this.btnattachment.Size = new System.Drawing.Size(109, 24);
		this.btnattachment.Text = "&Attachment";
		this.btnattachment.Visible = false;
		this.btnattachment.Click += new System.EventHandler(btnattachment_Click);
		this.BtnShipmentDocumentForm.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.BtnShipmentDocumentForm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShipmentDocumentForm.Image = (System.Drawing.Image)resources.GetObject("BtnShipmentDocumentForm.Image");
		this.BtnShipmentDocumentForm.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnShipmentDocumentForm.Name = "BtnShipmentDocumentForm";
		this.BtnShipmentDocumentForm.Size = new System.Drawing.Size(208, 24);
		this.BtnShipmentDocumentForm.Text = "Shipment Document Schedule";
		this.BtnShipmentDocumentForm.Click += new System.EventHandler(BtnShipmentDocumentForm_Click);
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.grdPendingContract);
		this.panel10.Controls.Add(this.panel11);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel10.Location = new System.Drawing.Point(0, 0);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(415, 602);
		this.panel10.TabIndex = 4;
		this.grdPendingContract.AllowEdit = (InheritableBoolean)2;
		grdPendingContract_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPendingContract.DesignTimeLayout = grdPendingContract_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPendingContract).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdPendingContract.DynamicFiltering = true;
		this.grdPendingContract.FilterMode = (FilterMode)2;
		this.grdPendingContract.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdPendingContract).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPendingContract.GroupByBoxVisible = false;
		this.grdPendingContract.GroupTotals = (GroupTotals)3;
		((System.Windows.Forms.Control)(object)this.grdPendingContract).Location = new System.Drawing.Point(0, 51);
		((System.Windows.Forms.Control)(object)this.grdPendingContract).Name = "grdPendingContract";
		((ScrollableBase)this.grdPendingContract).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPendingContract).Size = new System.Drawing.Size(415, 551);
		((System.Windows.Forms.Control)(object)this.grdPendingContract).TabIndex = 0;
		this.grdPendingContract.TotalRow = (InheritableBoolean)1;
		this.grdPendingContract.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPendingContract).VisualStyle = (VisualStyle)4;
		this.grdPendingContract.LinkClicked += new ColumnActionEventHandler(grdPendingContract_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdPendingContract).KeyDown += new System.Windows.Forms.KeyEventHandler(grdPendingContract_KeyDown);
		this.panel11.BackColor = System.Drawing.Color.Teal;
		this.panel11.Controls.Add(this.rdAll);
		this.panel11.Controls.Add(this.rdReffered);
		this.panel11.Controls.Add(this.rdNotReffered);
		this.panel11.Controls.Add(this.ctrlGrdBar1);
		this.panel11.Controls.Add(this.label13);
		this.panel11.Controls.Add(this.panel14);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(0, 0);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(415, 51);
		this.panel11.TabIndex = 4;
		this.rdAll.AutoSize = true;
		this.rdAll.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdAll.ForeColor = System.Drawing.Color.White;
		this.rdAll.Location = new System.Drawing.Point(305, 30);
		this.rdAll.Name = "rdAll";
		this.rdAll.Size = new System.Drawing.Size(39, 19);
		this.rdAll.TabIndex = 58;
		this.rdAll.Text = "All";
		this.rdAll.UseVisualStyleBackColor = true;
		this.rdAll.Click += new System.EventHandler(rdAll_Click);
		this.rdReffered.AutoSize = true;
		this.rdReffered.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdReffered.ForeColor = System.Drawing.Color.White;
		this.rdReffered.Location = new System.Drawing.Point(223, 29);
		this.rdReffered.Name = "rdReffered";
		this.rdReffered.Size = new System.Drawing.Size(76, 19);
		this.rdReffered.TabIndex = 57;
		this.rdReffered.Text = "Reffered";
		this.rdReffered.UseVisualStyleBackColor = true;
		this.rdReffered.Click += new System.EventHandler(rdReffered_Click);
		this.rdNotReffered.AutoSize = true;
		this.rdNotReffered.Checked = true;
		this.rdNotReffered.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdNotReffered.ForeColor = System.Drawing.Color.White;
		this.rdNotReffered.Location = new System.Drawing.Point(117, 29);
		this.rdNotReffered.Name = "rdNotReffered";
		this.rdNotReffered.Size = new System.Drawing.Size(100, 19);
		this.rdNotReffered.TabIndex = 56;
		this.rdNotReffered.TabStop = true;
		this.rdNotReffered.Text = "Not Reffered";
		this.rdNotReffered.UseVisualStyleBackColor = true;
		this.rdNotReffered.Click += new System.EventHandler(rdNotReffered_Click);
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(374, 28);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdPendingContract;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 55;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label13.ForeColor = System.Drawing.Color.White;
		this.label13.Location = new System.Drawing.Point(3, 30);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(110, 17);
		this.label13.TabIndex = 54;
		this.label13.Text = "Contracts Info";
		this.panel14.Controls.Add(this.toolStrip2);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel14.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.panel14.Location = new System.Drawing.Point(0, 0);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(415, 27);
		this.panel14.TabIndex = 1;
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.BtnNewMain, this.BtnLoadAllPendingContracts, this.BtnContractWiseSlip, this.BtnShortCutkeys });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(415, 27);
		this.toolStrip2.TabIndex = 7;
		this.toolStrip2.Text = "toolStrip2";
		this.BtnNewMain.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnNewMain.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.BtnNewMain.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnNewMain.Name = "BtnNewMain";
		this.BtnNewMain.Size = new System.Drawing.Size(59, 24);
		this.BtnNewMain.Text = "&New";
		this.BtnNewMain.Click += new System.EventHandler(BtnNewMain_Click);
		this.BtnLoadAllPendingContracts.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnLoadAllPendingContracts.Image = Architecture.WinApp.Properties.Resources.download_line;
		this.BtnLoadAllPendingContracts.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnLoadAllPendingContracts.Name = "BtnLoadAllPendingContracts";
		this.BtnLoadAllPendingContracts.Size = new System.Drawing.Size(82, 24);
		this.BtnLoadAllPendingContracts.Text = "Load All";
		this.BtnLoadAllPendingContracts.Click += new System.EventHandler(BtnLoadAllPendingContracts_Click);
		this.BtnContractWiseSlip.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnContractWiseSlip.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.BtnContractWiseSlip.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnContractWiseSlip.Name = "BtnContractWiseSlip";
		this.BtnContractWiseSlip.Size = new System.Drawing.Size(85, 24);
		this.BtnContractWiseSlip.Text = "395-Slip";
		this.BtnContractWiseSlip.Click += new System.EventHandler(BtnContractWiseSlip_Click);
		this.BtnShortCutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShortCutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnShortCutkeys.Name = "BtnShortCutkeys";
		this.BtnShortCutkeys.Size = new System.Drawing.Size(106, 24);
		this.BtnShortCutkeys.Text = "ShortCut Keys";
		this.BtnShortCutkeys.Click += new System.EventHandler(BtnShortCutkeys_Click);
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.label12);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1591, 27);
		this.panel6.TabIndex = 3;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.ForeColor = System.Drawing.Color.White;
		this.label12.Location = new System.Drawing.Point(4, 3);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(197, 19);
		this.label12.TabIndex = 54;
		this.label12.Text = "Sale Contract Schedule";
		this.tabPage2.Controls.Add(this.panelMainhistory);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1597, 635);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "HistoryNotInUse";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panelMainhistory.Controls.Add(this.panel7);
		this.panelMainhistory.Controls.Add(this.panel8);
		this.panelMainhistory.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panelMainhistory.Location = new System.Drawing.Point(3, 3);
		this.panelMainhistory.Name = "panelMainhistory";
		this.panelMainhistory.Size = new System.Drawing.Size(1591, 629);
		this.panelMainhistory.TabIndex = 0;
		this.panel7.Controls.Add(this.panel19);
		this.panel7.Controls.Add(this.panel17);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(0, 35);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1591, 594);
		this.panel7.TabIndex = 51;
		this.panel19.Controls.Add(this.panel21);
		this.panel19.Controls.Add(this.panel20);
		this.panel19.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel19.Location = new System.Drawing.Point(525, 0);
		this.panel19.Name = "panel19";
		this.panel19.Size = new System.Drawing.Size(1066, 594);
		this.panel19.TabIndex = 52;
		this.panel21.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistoryScheduleDetailByContact);
		this.panel21.Controls.Add(this.panel23);
		this.panel21.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel21.Location = new System.Drawing.Point(0, 291);
		this.panel21.Name = "panel21";
		this.panel21.Size = new System.Drawing.Size(1066, 303);
		this.panel21.TabIndex = 52;
		this.grdHistoryScheduleDetailByContact.AllowEdit = (InheritableBoolean)2;
		grdHistoryScheduleDetailByContact_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistoryScheduleDetailByContact.DesignTimeLayout = grdHistoryScheduleDetailByContact_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistoryScheduleDetailByContact).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistoryScheduleDetailByContact.DynamicFiltering = true;
		this.grdHistoryScheduleDetailByContact.FilterMode = (FilterMode)2;
		this.grdHistoryScheduleDetailByContact.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistoryScheduleDetailByContact).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistoryScheduleDetailByContact.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdHistoryScheduleDetailByContact).Location = new System.Drawing.Point(0, 29);
		((System.Windows.Forms.Control)(object)this.grdHistoryScheduleDetailByContact).Name = "grdHistoryScheduleDetailByContact";
		((ScrollableBase)this.grdHistoryScheduleDetailByContact).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistoryScheduleDetailByContact).Size = new System.Drawing.Size(1066, 274);
		((System.Windows.Forms.Control)(object)this.grdHistoryScheduleDetailByContact).TabIndex = 52;
		this.grdHistoryScheduleDetailByContact.TotalRow = (InheritableBoolean)1;
		this.grdHistoryScheduleDetailByContact.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistoryScheduleDetailByContact).VisualStyle = (VisualStyle)4;
		this.panel23.BackColor = System.Drawing.Color.Teal;
		this.panel23.Controls.Add(this.ctrlGrdBar6);
		this.panel23.Controls.Add(this.label4);
		this.panel23.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel23.Location = new System.Drawing.Point(0, 0);
		this.panel23.Name = "panel23";
		this.panel23.Size = new System.Drawing.Size(1066, 29);
		this.panel23.TabIndex = 51;
		this.ctrlGrdBar6.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar6.FormName = this;
		this.ctrlGrdBar6.Location = new System.Drawing.Point(1013, 3);
		this.ctrlGrdBar6.Margin = new System.Windows.Forms.Padding(9, 7, 9, 7);
		this.ctrlGrdBar6.MyGrid = this.grdHistoryScheduleDetailByContact;
		this.ctrlGrdBar6.Name = "ctrlGrdBar6";
		this.ctrlGrdBar6.Size = new System.Drawing.Size(49, 24);
		this.ctrlGrdBar6.TabIndex = 56;
		this.ctrlGrdBar6.Load += new System.EventHandler(ctrlGrdBar6_Load);
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label4.ForeColor = System.Drawing.Color.White;
		this.label4.Location = new System.Drawing.Point(2, 7);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(491, 17);
		this.label4.TabIndex = 55;
		this.label4.Text = "Sale Contract Schedule Packing Detail of Selected Row in History Main ";
		this.panel20.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistoryScheduleByContract);
		this.panel20.Controls.Add(this.panel22);
		this.panel20.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel20.Location = new System.Drawing.Point(0, 0);
		this.panel20.Name = "panel20";
		this.panel20.Size = new System.Drawing.Size(1066, 291);
		this.panel20.TabIndex = 51;
		this.grdHistoryScheduleByContract.AllowEdit = (InheritableBoolean)2;
		grdHistoryScheduleByContract_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistoryScheduleByContract.DesignTimeLayout = grdHistoryScheduleByContract_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistoryScheduleByContract).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistoryScheduleByContract.DynamicFiltering = true;
		this.grdHistoryScheduleByContract.FilterMode = (FilterMode)2;
		this.grdHistoryScheduleByContract.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistoryScheduleByContract).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistoryScheduleByContract.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdHistoryScheduleByContract).Location = new System.Drawing.Point(0, 27);
		((System.Windows.Forms.Control)(object)this.grdHistoryScheduleByContract).Name = "grdHistoryScheduleByContract";
		((ScrollableBase)this.grdHistoryScheduleByContract).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistoryScheduleByContract).Size = new System.Drawing.Size(1066, 264);
		((System.Windows.Forms.Control)(object)this.grdHistoryScheduleByContract).TabIndex = 51;
		this.grdHistoryScheduleByContract.TotalRow = (InheritableBoolean)1;
		this.grdHistoryScheduleByContract.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistoryScheduleByContract).VisualStyle = (VisualStyle)4;
		this.panel22.BackColor = System.Drawing.Color.Teal;
		this.panel22.Controls.Add(this.ctrlGrdBar5);
		this.panel22.Controls.Add(this.label2);
		this.panel22.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel22.Location = new System.Drawing.Point(0, 0);
		this.panel22.Name = "panel22";
		this.panel22.Size = new System.Drawing.Size(1066, 27);
		this.panel22.TabIndex = 50;
		this.ctrlGrdBar5.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar5.FormName = this;
		this.ctrlGrdBar5.Location = new System.Drawing.Point(1019, 2);
		this.ctrlGrdBar5.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
		this.ctrlGrdBar5.MyGrid = this.grdHistoryScheduleByContract;
		this.ctrlGrdBar5.Name = "ctrlGrdBar5";
		this.ctrlGrdBar5.Size = new System.Drawing.Size(45, 24);
		this.ctrlGrdBar5.TabIndex = 56;
		this.ctrlGrdBar5.Load += new System.EventHandler(ctrlGrdBar5_Load);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(6, 3);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(392, 17);
		this.label2.TabIndex = 55;
		this.label2.Text = "Sale Contract Schedule of Selected Row in History Main ";
		this.panel17.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistoryMainByContract);
		this.panel17.Controls.Add(this.panel18);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel17.Location = new System.Drawing.Point(0, 0);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(525, 594);
		this.panel17.TabIndex = 51;
		this.grdHistoryMainByContract.AllowEdit = (InheritableBoolean)2;
		grdHistoryMainByContract_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistoryMainByContract.DesignTimeLayout = grdHistoryMainByContract_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistoryMainByContract).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistoryMainByContract.DynamicFiltering = true;
		this.grdHistoryMainByContract.FilterMode = (FilterMode)2;
		this.grdHistoryMainByContract.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistoryMainByContract).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistoryMainByContract.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdHistoryMainByContract).Location = new System.Drawing.Point(0, 54);
		((System.Windows.Forms.Control)(object)this.grdHistoryMainByContract).Name = "grdHistoryMainByContract";
		((ScrollableBase)this.grdHistoryMainByContract).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistoryMainByContract).Size = new System.Drawing.Size(525, 540);
		((System.Windows.Forms.Control)(object)this.grdHistoryMainByContract).TabIndex = 51;
		this.grdHistoryMainByContract.TotalRow = (InheritableBoolean)1;
		this.grdHistoryMainByContract.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistoryMainByContract).VisualStyle = (VisualStyle)4;
		this.grdHistoryMainByContract.ColumnButtonClick += new ColumnActionEventHandler(grdHistoryMainByContract_ColumnButtonClick);
		this.grdHistoryMainByContract.SelectionChanged += new System.EventHandler(grdHistoryMainByContract_SelectionChanged);
		((System.Windows.Forms.Control)(object)this.grdHistoryMainByContract).KeyDown += new System.Windows.Forms.KeyEventHandler(grdHistoryMainByContract_KeyDown);
		this.panel18.BackColor = System.Drawing.Color.Teal;
		this.panel18.Controls.Add(this.ctrlGrdBar4);
		this.panel18.Controls.Add(this.label3);
		this.panel18.Controls.Add(this.ToolstripHistoryContract);
		this.panel18.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel18.Location = new System.Drawing.Point(0, 0);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(525, 54);
		this.panel18.TabIndex = 2;
		this.ctrlGrdBar4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar4.FormName = this;
		this.ctrlGrdBar4.Location = new System.Drawing.Point(480, 28);
		this.ctrlGrdBar4.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar4.MyGrid = this.grdHistoryMainByContract;
		this.ctrlGrdBar4.Name = "ctrlGrdBar4";
		this.ctrlGrdBar4.Size = new System.Drawing.Size(43, 25);
		this.ctrlGrdBar4.TabIndex = 56;
		this.ctrlGrdBar4.Load += new System.EventHandler(ctrlGrdBar4_Load);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold);
		this.label3.ForeColor = System.Drawing.Color.White;
		this.label3.Location = new System.Drawing.Point(6, 30);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(211, 19);
		this.label3.TabIndex = 55;
		this.label3.Text = "History Main By Contract";
		this.ToolstripHistoryContract.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.ToolstripHistoryContract.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.LoadAllgrdContract });
		this.ToolstripHistoryContract.Location = new System.Drawing.Point(0, 0);
		this.ToolstripHistoryContract.Name = "ToolstripHistoryContract";
		this.ToolstripHistoryContract.Size = new System.Drawing.Size(525, 27);
		this.ToolstripHistoryContract.TabIndex = 8;
		this.ToolstripHistoryContract.Text = "toolStrip4";
		this.LoadAllgrdContract.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.LoadAllgrdContract.Image = Architecture.WinApp.Properties.Resources.download_line;
		this.LoadAllgrdContract.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.LoadAllgrdContract.Name = "LoadAllgrdContract";
		this.LoadAllgrdContract.Size = new System.Drawing.Size(82, 24);
		this.LoadAllgrdContract.Text = "Load All";
		this.LoadAllgrdContract.Click += new System.EventHandler(LoadAllgrdContract_Click);
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.label1);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 0);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1591, 35);
		this.panel8.TabIndex = 1;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(5, 6);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(260, 19);
		this.label1.TabIndex = 55;
		this.label1.Text = "Sale Contract Schedule History";
		this.tabPage3.Controls.Add(this.panel26);
		this.tabPage3.Controls.Add(this.panel25);
		this.tabPage3.Controls.Add(this.toolStrip4);
		this.tabPage3.Location = new System.Drawing.Point(4, 4);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Size = new System.Drawing.Size(1597, 635);
		this.tabPage3.TabIndex = 2;
		this.tabPage3.Text = "History";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.panel26.Controls.Add(this.panel27);
		this.panel26.Controls.Add(this.panel24);
		this.panel26.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel26.Location = new System.Drawing.Point(0, 100);
		this.panel26.Name = "panel26";
		this.panel26.Size = new System.Drawing.Size(1597, 535);
		this.panel26.TabIndex = 55;
		this.panel27.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.panel27.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel27.Location = new System.Drawing.Point(0, 27);
		this.panel27.Name = "panel27";
		this.panel27.Size = new System.Drawing.Size(1597, 508);
		this.panel27.TabIndex = 54;
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
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1597, 508);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 53;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		this.DataGridHistory.LinkClicked += new ColumnActionEventHandler(DataGridHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(DataGridHistory_KeyDown);
		this.panel24.BackColor = System.Drawing.Color.Teal;
		this.panel24.Controls.Add(this.ctrlGrdBar7);
		this.panel24.Controls.Add(this.label5);
		this.panel24.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel24.Location = new System.Drawing.Point(0, 0);
		this.panel24.Name = "panel24";
		this.panel24.Size = new System.Drawing.Size(1597, 27);
		this.panel24.TabIndex = 52;
		this.ctrlGrdBar7.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar7.FormName = this;
		this.ctrlGrdBar7.Location = new System.Drawing.Point(1557, 3);
		this.ctrlGrdBar7.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar7.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar7.Name = "ctrlGrdBar7";
		this.ctrlGrdBar7.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar7.TabIndex = 56;
		this.ctrlGrdBar7.Load += new System.EventHandler(ctrlGrdBar7_Load);
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold);
		this.label5.ForeColor = System.Drawing.Color.White;
		this.label5.Location = new System.Drawing.Point(6, 3);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(67, 19);
		this.label5.TabIndex = 55;
		this.label5.Text = "History";
		this.panel25.Controls.Add(this.groupBox2);
		this.panel25.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel25.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.panel25.Location = new System.Drawing.Point(0, 27);
		this.panel25.Name = "panel25";
		this.panel25.Size = new System.Drawing.Size(1597, 73);
		this.panel25.TabIndex = 54;
		this.groupBox2.Controls.Add(this.label10);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPort);
		this.groupBox2.Controls.Add(this.txtdateto);
		this.groupBox2.Controls.Add(this.label50);
		this.groupBox2.Controls.Add(this.label11);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbSupplierName);
		this.groupBox2.Controls.Add(this.txtdatefrom);
		this.groupBox2.Controls.Add(this.label19);
		this.groupBox2.Controls.Add(this.label51);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemHistory);
		this.groupBox2.Controls.Add(this.btnShow);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(4, 3);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(1009, 67);
		this.groupBox2.TabIndex = 1;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Filters";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(330, 18);
		this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(29, 15);
		this.label10.TabIndex = 577;
		this.label10.Text = "Port";
		this.cmbPort.AllowNull = (DefaultableBoolean)1;
		this.cmbPort.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPort.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPort.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPort).DisplayLayout.Appearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.cmbPort).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPort).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPort).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPort).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance54;
		((SpecialBoxBase)((UltraGridBase)this.cmbPort).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPort).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.cmbPort).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPort).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPort).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPort).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.cmbPort).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPort).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPort).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance60).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPort).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.cmbPort).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPort).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance61).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPort).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPort).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.cmbPort).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPort).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPort).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.cmbPort).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPort).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.cmbPort).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPort).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPort).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPort).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbPort.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPort).Location = new System.Drawing.Point(330, 35);
		((System.Windows.Forms.Control)(object)this.cmbPort).Name = "cmbPort";
		((System.Windows.Forms.Control)(object)this.cmbPort).Size = new System.Drawing.Size(202, 26);
		((System.Windows.Forms.Control)(object)this.cmbPort).TabIndex = 2;
		this.txtdateto.CustomFormat = "dd-MMM-yy";
		this.txtdateto.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdateto.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtdateto.Location = new System.Drawing.Point(168, 37);
		this.txtdateto.Name = "txtdateto";
		this.txtdateto.Size = new System.Drawing.Size(160, 23);
		this.txtdateto.TabIndex = 1;
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label50.Location = new System.Drawing.Point(536, 18);
		this.label50.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(93, 15);
		this.label50.TabIndex = 16;
		this.label50.Text = "Customer Name";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(168, 18);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(91, 15);
		this.label11.TabIndex = 575;
		this.label11.Text = "loading Date To";
		this.cmbSupplierName.AllowNull = (DefaultableBoolean)1;
		this.cmbSupplierName.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbSupplierName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbSupplierName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbSupplierName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance67;
		((SpecialBoxBase)((UltraGridBase)this.cmbSupplierName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance73).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance74).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbSupplierName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbSupplierName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbSupplierName).Location = new System.Drawing.Point(534, 35);
		((System.Windows.Forms.Control)(object)this.cmbSupplierName).Name = "cmbSupplierName";
		((System.Windows.Forms.Control)(object)this.cmbSupplierName).Size = new System.Drawing.Size(211, 26);
		((System.Windows.Forms.Control)(object)this.cmbSupplierName).TabIndex = 3;
		this.txtdatefrom.CustomFormat = "dd-MMM-yy";
		this.txtdatefrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdatefrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtdatefrom.Location = new System.Drawing.Point(6, 37);
		this.txtdatefrom.Name = "txtdatefrom";
		this.txtdatefrom.Size = new System.Drawing.Size(160, 23);
		this.txtdatefrom.TabIndex = 0;
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(747, 18);
		this.label19.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(32, 15);
		this.label19.TabIndex = 18;
		this.label19.Text = "Item";
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label51.Location = new System.Drawing.Point(6, 18);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(109, 15);
		this.label51.TabIndex = 572;
		this.label51.Text = "Loading Date From";
		this.CmbItemHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbItemHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance80).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance80).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance81;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance82).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance82).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance86).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance87).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance87).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance87).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance87;
		((AppearanceBase)appearance88).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemHistory).Location = new System.Drawing.Point(747, 35);
		((System.Windows.Forms.Control)(object)this.CmbItemHistory).Name = "CmbItemHistory";
		((System.Windows.Forms.Control)(object)this.CmbItemHistory).Size = new System.Drawing.Size(195, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemHistory).TabIndex = 4;
		this.btnShow.BackColor = System.Drawing.Color.Teal;
		this.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShow.ForeColor = System.Drawing.Color.White;
		this.btnShow.Location = new System.Drawing.Point(943, 35);
		this.btnShow.Name = "btnShow";
		this.btnShow.Size = new System.Drawing.Size(58, 26);
		this.btnShow.TabIndex = 5;
		this.btnShow.Text = "Show";
		this.btnShow.UseVisualStyleBackColor = false;
		this.btnShow.Click += new System.EventHandler(btnShow_Click);
		this.toolStrip4.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip4.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnLoadAllHistoryNew, this.btnRefreshHistoryCombos, this.BtnHistoryPrint, this.BtnPrint551_01 });
		this.toolStrip4.Location = new System.Drawing.Point(0, 0);
		this.toolStrip4.Name = "toolStrip4";
		this.toolStrip4.Size = new System.Drawing.Size(1597, 27);
		this.toolStrip4.TabIndex = 8;
		this.toolStrip4.Text = "toolStrip4";
		this.btnLoadAllHistoryNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLoadAllHistoryNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnLoadAllHistoryNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadAllHistoryNew.Name = "btnLoadAllHistoryNew";
		this.btnLoadAllHistoryNew.Size = new System.Drawing.Size(59, 24);
		this.btnLoadAllHistoryNew.Text = "New";
		this.btnLoadAllHistoryNew.Click += new System.EventHandler(btnLoadAllHistoryNew_Click);
		this.btnRefreshHistoryCombos.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefreshHistoryCombos.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshHistoryCombos.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshHistoryCombos.Name = "btnRefreshHistoryCombos";
		this.btnRefreshHistoryCombos.Size = new System.Drawing.Size(80, 24);
		this.btnRefreshHistoryCombos.Text = "&Refresh";
		this.btnRefreshHistoryCombos.Click += new System.EventHandler(btnRefreshHistoryCombos_Click);
		this.BtnHistoryPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnHistoryPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.BtnHistoryPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnHistoryPrint.Name = "BtnHistoryPrint";
		this.BtnHistoryPrint.Size = new System.Drawing.Size(92, 24);
		this.BtnHistoryPrint.Text = "551-&Print";
		this.BtnHistoryPrint.Click += new System.EventHandler(BtnHistoryPrint_Click);
		this.BtnPrint551_01.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnPrint551_01.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.BtnPrint551_01.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnPrint551_01.Name = "BtnPrint551_01";
		this.BtnPrint551_01.Size = new System.Drawing.Size(116, 24);
		this.BtnPrint551_01.Text = "551_01-&Print";
		this.BtnPrint551_01.Click += new System.EventHandler(BtnPrint551_01_Click);
		this.tabControlMain.Controls.Add(this.tabPageMain);
		this.tabControlMain.Controls.Add(this.tabPageDepartmentStatus);
		this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControlMain.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControlMain.Location = new System.Drawing.Point(0, 0);
		this.tabControlMain.Name = "tabControlMain";
		this.tabControlMain.SelectedIndex = 0;
		this.tabControlMain.Size = new System.Drawing.Size(1619, 699);
		this.tabControlMain.TabIndex = 1;
		this.tabControlMain.SelectedIndexChanged += new System.EventHandler(tabControlMain_SelectedIndexChanged);
		this.tabPageMain.Controls.Add(this.tabControl1);
		this.tabPageMain.Location = new System.Drawing.Point(4, 25);
		this.tabPageMain.Name = "tabPageMain";
		this.tabPageMain.Padding = new System.Windows.Forms.Padding(3);
		this.tabPageMain.Size = new System.Drawing.Size(1611, 670);
		this.tabPageMain.TabIndex = 0;
		this.tabPageMain.Text = "Main Form";
		this.tabPageMain.UseVisualStyleBackColor = true;
		this.tabPageDepartmentStatus.Controls.Add((System.Windows.Forms.Control)(object)this.grdDepartmentDetail);
		this.tabPageDepartmentStatus.Controls.Add(this.panel1);
		this.tabPageDepartmentStatus.Controls.Add(this.panel28);
		this.tabPageDepartmentStatus.Controls.Add(this.panel29);
		this.tabPageDepartmentStatus.Location = new System.Drawing.Point(4, 25);
		this.tabPageDepartmentStatus.Name = "tabPageDepartmentStatus";
		this.tabPageDepartmentStatus.Padding = new System.Windows.Forms.Padding(3);
		this.tabPageDepartmentStatus.Size = new System.Drawing.Size(1611, 670);
		this.tabPageDepartmentStatus.TabIndex = 1;
		this.tabPageDepartmentStatus.Text = "Pre Shipment Planning";
		this.tabPageDepartmentStatus.UseVisualStyleBackColor = true;
		this.grdDepartmentDetail.AllowEdit = (InheritableBoolean)2;
		grdDepartmentDetail_DesignTimeLayout.LayoutString = resources.GetString("grdDepartmentDetail_DesignTimeLayout.LayoutString");
		this.grdDepartmentDetail.DesignTimeLayout = grdDepartmentDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDepartmentDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdDepartmentDetail.DynamicFiltering = true;
		this.grdDepartmentDetail.FilterMode = (FilterMode)2;
		this.grdDepartmentDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdDepartmentDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDepartmentDetail.GroupByBoxVisible = false;
		this.grdDepartmentDetail.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdDepartmentDetail).Location = new System.Drawing.Point(3, 194);
		((System.Windows.Forms.Control)(object)this.grdDepartmentDetail).Name = "grdDepartmentDetail";
		((ScrollableBase)this.grdDepartmentDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDepartmentDetail).Size = new System.Drawing.Size(1605, 473);
		((System.Windows.Forms.Control)(object)this.grdDepartmentDetail).TabIndex = 1;
		this.grdDepartmentDetail.TotalRow = (InheritableBoolean)1;
		this.grdDepartmentDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDepartmentDetail).VisualStyle = (VisualStyle)4;
		this.grdDepartmentDetail.ColumnButtonClick += new ColumnActionEventHandler(grdDepartmentDetail_ColumnButtonClick);
		this.grdDepartmentDetail.LinkClicked += new ColumnActionEventHandler(grdDepartmentDetail_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdDepartmentDetail).DoubleClick += new System.EventHandler(grdDepartmentDetail_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdDepartmentDetail).KeyDown += new System.Windows.Forms.KeyEventHandler(grdDepartmentDetail_KeyDown);
		this.panel1.BackColor = System.Drawing.Color.Teal;
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(3, 171);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1605, 23);
		this.panel1.TabIndex = 8;
		this.panel28.Controls.Add(this.BtnShowbyDates);
		this.panel28.Controls.Add(this.tztAddDays);
		this.panel28.Controls.Add(this.txtDateFromDepart);
		this.panel28.Controls.Add(this.label39);
		this.panel28.Controls.Add(this.txtDatetoDepart);
		this.panel28.Controls.Add(this.label40);
		this.panel28.Controls.Add(this.label41);
		this.panel28.Controls.Add(this.groupBox3);
		this.panel28.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel28.Location = new System.Drawing.Point(3, 53);
		this.panel28.Name = "panel28";
		this.panel28.Size = new System.Drawing.Size(1605, 118);
		this.panel28.TabIndex = 0;
		this.BtnShowbyDates.BackColor = System.Drawing.Color.Teal;
		this.BtnShowbyDates.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.BtnShowbyDates.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.BtnShowbyDates.ForeColor = System.Drawing.Color.White;
		this.BtnShowbyDates.Location = new System.Drawing.Point(1407, 88);
		this.BtnShowbyDates.Name = "BtnShowbyDates";
		this.BtnShowbyDates.Size = new System.Drawing.Size(55, 23);
		this.BtnShowbyDates.TabIndex = 4;
		this.BtnShowbyDates.Text = "Show";
		this.BtnShowbyDates.UseVisualStyleBackColor = false;
		this.BtnShowbyDates.Click += new System.EventHandler(BtnShowbyDates_Click);
		this.tztAddDays.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tztAddDays.Location = new System.Drawing.Point(1303, 35);
		this.tztAddDays.Name = "tztAddDays";
		this.tztAddDays.Size = new System.Drawing.Size(159, 23);
		this.tztAddDays.TabIndex = 2;
		this.tztAddDays.TextChanged += new System.EventHandler(tztAddDays_TextChanged);
		this.tztAddDays.KeyPress += new System.Windows.Forms.KeyPressEventHandler(tztAddDays_KeyPress);
		this.txtDateFromDepart.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDateFromDepart.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtDateFromDepart.Location = new System.Drawing.Point(1303, 6);
		this.txtDateFromDepart.Name = "txtDateFromDepart";
		this.txtDateFromDepart.ShowCheckBox = true;
		this.txtDateFromDepart.Size = new System.Drawing.Size(159, 23);
		this.txtDateFromDepart.TabIndex = 1;
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label39.Location = new System.Drawing.Point(1199, 10);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(109, 15);
		this.label39.TabIndex = 180;
		this.label39.Text = "From Date Loading";
		this.txtDatetoDepart.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDatetoDepart.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtDatetoDepart.Location = new System.Drawing.Point(1303, 64);
		this.txtDatetoDepart.Name = "txtDatetoDepart";
		this.txtDatetoDepart.ShowCheckBox = true;
		this.txtDatetoDepart.Size = new System.Drawing.Size(159, 23);
		this.txtDatetoDepart.TabIndex = 3;
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label40.Location = new System.Drawing.Point(1199, 68);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(94, 15);
		this.label40.TabIndex = 181;
		this.label40.Text = "To Date Loading";
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label41.Location = new System.Drawing.Point(1199, 39);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(58, 15);
		this.label41.TabIndex = 182;
		this.label41.Text = "Add Days";
		this.groupBox3.Controls.Add(this.label37);
		this.groupBox3.Controls.Add(this.txtPackingMaterialScheduleDays);
		this.groupBox3.Controls.Add(this.label36);
		this.groupBox3.Controls.Add(this.txtInspectionDays);
		this.groupBox3.Controls.Add(this.label35);
		this.groupBox3.Controls.Add(this.txtProductionDays);
		this.groupBox3.Controls.Add(this.txtPMDateDepartmentDetail);
		this.groupBox3.Controls.Add(this.txtInspectionDateDepartmentDetail);
		this.groupBox3.Controls.Add(this.txtProductionDateDepartmentDetail);
		this.groupBox3.Controls.Add(this.txtLoadingDateDepartmentDetail);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomerDepartmentDetail);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbScheduleLockStatusDepartmentDetail);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemDepartmentDetail);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPortDepartmentDetail);
		this.groupBox3.Controls.Add(this.BtnCancelDepartmentDetail);
		this.groupBox3.Controls.Add(this.BtnUpdateDepartmentDetail);
		this.groupBox3.Controls.Add(this.label20);
		this.groupBox3.Controls.Add(this.label32);
		this.groupBox3.Controls.Add(this.txtItemMtonDepartmentDetail);
		this.groupBox3.Controls.Add(this.label31);
		this.groupBox3.Controls.Add(this.label21);
		this.groupBox3.Controls.Add(this.label30);
		this.groupBox3.Controls.Add(this.label22);
		this.groupBox3.Controls.Add(this.label33);
		this.groupBox3.Controls.Add(this.label28);
		this.groupBox3.Controls.Add(this.label23);
		this.groupBox3.Controls.Add(this.label29);
		this.groupBox3.Controls.Add(this.txtScheduleMtonDepartmentDetail);
		this.groupBox3.Controls.Add(this.txtFclDepartmentDetail);
		this.groupBox3.Controls.Add(this.label24);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbContractDepartmentDetail);
		this.groupBox3.Controls.Add(this.label25);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPlantDepartmentDetail);
		this.groupBox3.Controls.Add(this.label34);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbScheduleNoDepartmentDetail);
		this.groupBox3.Controls.Add(this.label26);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(4, 1);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(1193, 114);
		this.groupBox3.TabIndex = 0;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Detail";
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(651, 64);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(55, 15);
		this.label37.TabIndex = 75;
		this.label37.Text = "P.M Days";
		this.txtPackingMaterialScheduleDays.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPackingMaterialScheduleDays.Location = new System.Drawing.Point(651, 83);
		this.txtPackingMaterialScheduleDays.Name = "txtPackingMaterialScheduleDays";
		this.txtPackingMaterialScheduleDays.Size = new System.Drawing.Size(72, 23);
		this.txtPackingMaterialScheduleDays.TabIndex = 14;
		this.txtPackingMaterialScheduleDays.Tag = "PackingMaterialScheduleDays";
		this.txtPackingMaterialScheduleDays.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtPackingMaterialScheduleDays_KeyPress);
		this.txtPackingMaterialScheduleDays.Leave += new System.EventHandler(txtPackingMaterialScheduleDays_TextChanged);
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(413, 64);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(92, 15);
		this.label36.TabIndex = 73;
		this.label36.Text = "Inspection Days";
		this.txtInspectionDays.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtInspectionDays.Location = new System.Drawing.Point(413, 83);
		this.txtInspectionDays.Name = "txtInspectionDays";
		this.txtInspectionDays.Size = new System.Drawing.Size(98, 23);
		this.txtInspectionDays.TabIndex = 12;
		this.txtInspectionDays.Tag = "InspectionDays";
		this.txtInspectionDays.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtInspectionDays_KeyPress);
		this.txtInspectionDays.Leave += new System.EventHandler(txtInspectionDays_TextChanged);
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(3, 64);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(95, 15);
		this.label35.TabIndex = 71;
		this.label35.Text = "Production Days";
		this.txtProductionDays.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtProductionDays.Location = new System.Drawing.Point(3, 83);
		this.txtProductionDays.Name = "txtProductionDays";
		this.txtProductionDays.Size = new System.Drawing.Size(98, 23);
		this.txtProductionDays.TabIndex = 9;
		this.txtProductionDays.Tag = "ProductionDays";
		this.txtProductionDays.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtProductionDays_KeyPress);
		this.txtProductionDays.Leave += new System.EventHandler(txtProductionDays_TextChanged);
		this.txtPMDateDepartmentDetail.CalendarFont = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPMDateDepartmentDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPMDateDepartmentDetail.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtPMDateDepartmentDetail.Location = new System.Drawing.Point(724, 83);
		this.txtPMDateDepartmentDetail.Name = "txtPMDateDepartmentDetail";
		this.txtPMDateDepartmentDetail.ShowCheckBox = true;
		this.txtPMDateDepartmentDetail.Size = new System.Drawing.Size(147, 23);
		this.txtPMDateDepartmentDetail.TabIndex = 15;
		this.txtPMDateDepartmentDetail.ValueChanged += new System.EventHandler(txtPMDateDepartmentDetail_ValueChanged);
		this.txtInspectionDateDepartmentDetail.CalendarFont = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtInspectionDateDepartmentDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtInspectionDateDepartmentDetail.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtInspectionDateDepartmentDetail.Location = new System.Drawing.Point(514, 83);
		this.txtInspectionDateDepartmentDetail.Name = "txtInspectionDateDepartmentDetail";
		this.txtInspectionDateDepartmentDetail.ShowCheckBox = true;
		this.txtInspectionDateDepartmentDetail.Size = new System.Drawing.Size(136, 23);
		this.txtInspectionDateDepartmentDetail.TabIndex = 13;
		this.txtInspectionDateDepartmentDetail.ValueChanged += new System.EventHandler(txtInspectionDateDepartmentDetail_ValueChanged);
		this.txtProductionDateDepartmentDetail.CalendarFont = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtProductionDateDepartmentDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtProductionDateDepartmentDetail.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtProductionDateDepartmentDetail.Location = new System.Drawing.Point(104, 83);
		this.txtProductionDateDepartmentDetail.Name = "txtProductionDateDepartmentDetail";
		this.txtProductionDateDepartmentDetail.Size = new System.Drawing.Size(125, 23);
		this.txtProductionDateDepartmentDetail.TabIndex = 10;
		this.txtProductionDateDepartmentDetail.ValueChanged += new System.EventHandler(txtProductionDateDepartmentDetail_ValueChanged);
		this.txtLoadingDateDepartmentDetail.CalendarFont = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtLoadingDateDepartmentDetail.Enabled = false;
		this.txtLoadingDateDepartmentDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtLoadingDateDepartmentDetail.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtLoadingDateDepartmentDetail.Location = new System.Drawing.Point(789, 38);
		this.txtLoadingDateDepartmentDetail.Name = "txtLoadingDateDepartmentDetail";
		this.txtLoadingDateDepartmentDetail.Size = new System.Drawing.Size(121, 23);
		this.txtLoadingDateDepartmentDetail.TabIndex = 6;
		this.CmbCustomerDepartmentDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomerDepartmentDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomerDepartmentDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomerDepartmentDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomerDepartmentDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbCustomerDepartmentDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomerDepartmentDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance93).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance93).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerDepartmentDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance104).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerDepartmentDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance104;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerDepartmentDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance115).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance115).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance115).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerDepartmentDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.CmbCustomerDepartmentDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomerDepartmentDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance126).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomerDepartmentDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance126;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance137).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomerDepartmentDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance137;
		((UltraGridBase)this.CmbCustomerDepartmentDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomerDepartmentDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerDepartmentDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance148;
		((AppearanceBase)appearance159).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance159).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomerDepartmentDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.CmbCustomerDepartmentDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomerDepartmentDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance170).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance170).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance170).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance170).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerDepartmentDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance170;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomerDepartmentDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbCustomerDepartmentDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomerDepartmentDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomerDepartmentDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbCustomerDepartmentDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomerDepartmentDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbCustomerDepartmentDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomerDepartmentDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomerDepartmentDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomerDepartmentDetail).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbCustomerDepartmentDetail).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomerDepartmentDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomerDepartmentDetail).Location = new System.Drawing.Point(276, 36);
		((System.Windows.Forms.Control)(object)this.CmbCustomerDepartmentDetail).Name = "CmbCustomerDepartmentDetail";
		((System.Windows.Forms.Control)(object)this.CmbCustomerDepartmentDetail).Size = new System.Drawing.Size(201, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomerDepartmentDetail).TabIndex = 2;
		this.CmbScheduleLockStatusDepartmentDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbScheduleLockStatusDepartmentDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbScheduleLockStatusDepartmentDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbScheduleLockStatusDepartmentDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbScheduleLockStatusDepartmentDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbScheduleLockStatusDepartmentDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbScheduleLockStatusDepartmentDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbScheduleLockStatusDepartmentDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbScheduleLockStatusDepartmentDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance57;
		((SpecialBoxBase)((UltraGridBase)this.CmbScheduleLockStatusDepartmentDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbScheduleLockStatusDepartmentDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.CmbScheduleLockStatusDepartmentDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbScheduleLockStatusDepartmentDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbScheduleLockStatusDepartmentDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbScheduleLockStatusDepartmentDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.CmbScheduleLockStatusDepartmentDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbScheduleLockStatusDepartmentDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbScheduleLockStatusDepartmentDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance92;
		((AppearanceBase)appearance94).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance94).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbScheduleLockStatusDepartmentDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance94;
		((UltraGridBase)this.CmbScheduleLockStatusDepartmentDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbScheduleLockStatusDepartmentDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance95).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance95).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance95).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance95).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbScheduleLockStatusDepartmentDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance95;
		((AppearanceBase)appearance96).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbScheduleLockStatusDepartmentDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.CmbScheduleLockStatusDepartmentDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbScheduleLockStatusDepartmentDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance97).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbScheduleLockStatusDepartmentDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.CmbScheduleLockStatusDepartmentDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbScheduleLockStatusDepartmentDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance98;
		((UltraGridBase)this.CmbScheduleLockStatusDepartmentDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbScheduleLockStatusDepartmentDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbScheduleLockStatusDepartmentDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbScheduleLockStatusDepartmentDetail).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbScheduleLockStatusDepartmentDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbScheduleLockStatusDepartmentDetail).Location = new System.Drawing.Point(873, 81);
		((System.Windows.Forms.Control)(object)this.CmbScheduleLockStatusDepartmentDetail).Name = "CmbScheduleLockStatusDepartmentDetail";
		((System.Windows.Forms.Control)(object)this.CmbScheduleLockStatusDepartmentDetail).Size = new System.Drawing.Size(181, 26);
		((System.Windows.Forms.Control)(object)this.CmbScheduleLockStatusDepartmentDetail).TabIndex = 16;
		this.CmbItemDepartmentDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbItemDepartmentDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemDepartmentDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemDepartmentDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemDepartmentDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.CmbItemDepartmentDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemDepartmentDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemDepartmentDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance101).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemDepartmentDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance101;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemDepartmentDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance102).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance102).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance102).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemDepartmentDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.CmbItemDepartmentDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemDepartmentDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance103).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemDepartmentDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance103;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance105).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemDepartmentDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.CmbItemDepartmentDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemDepartmentDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemDepartmentDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance106;
		((AppearanceBase)appearance107).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance107).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemDepartmentDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.CmbItemDepartmentDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemDepartmentDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance108).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance108).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance108).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance108).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemDepartmentDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance108;
		((AppearanceBase)appearance109).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemDepartmentDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.CmbItemDepartmentDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemDepartmentDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance110).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemDepartmentDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.CmbItemDepartmentDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemDepartmentDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.CmbItemDepartmentDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemDepartmentDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemDepartmentDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemDepartmentDetail).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbItemDepartmentDetail).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemDepartmentDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemDepartmentDetail).Location = new System.Drawing.Point(912, 36);
		((System.Windows.Forms.Control)(object)this.CmbItemDepartmentDetail).Name = "CmbItemDepartmentDetail";
		((System.Windows.Forms.Control)(object)this.CmbItemDepartmentDetail).Size = new System.Drawing.Size(196, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemDepartmentDetail).TabIndex = 7;
		this.CmbPortDepartmentDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbPortDepartmentDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPortDepartmentDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPortDepartmentDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPortDepartmentDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.CmbPortDepartmentDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPortDepartmentDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance113).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance113).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPortDepartmentDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance113;
		((AppearanceBase)appearance114).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPortDepartmentDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance114;
		((SpecialBoxBase)((UltraGridBase)this.CmbPortDepartmentDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance116).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance116).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance116).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPortDepartmentDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.CmbPortDepartmentDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPortDepartmentDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance117).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPortDepartmentDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance117;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance118).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPortDepartmentDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.CmbPortDepartmentDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPortDepartmentDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPortDepartmentDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance119;
		((AppearanceBase)appearance120).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance120).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPortDepartmentDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.CmbPortDepartmentDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPortDepartmentDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance121).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance121).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance121).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance121).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPortDepartmentDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance121;
		((AppearanceBase)appearance122).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPortDepartmentDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.CmbPortDepartmentDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPortDepartmentDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance123).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPortDepartmentDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.CmbPortDepartmentDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPortDepartmentDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.CmbPortDepartmentDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPortDepartmentDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPortDepartmentDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPortDepartmentDetail).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbPortDepartmentDetail).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPortDepartmentDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPortDepartmentDetail).Location = new System.Drawing.Point(479, 36);
		((System.Windows.Forms.Control)(object)this.CmbPortDepartmentDetail).Name = "CmbPortDepartmentDetail";
		((System.Windows.Forms.Control)(object)this.CmbPortDepartmentDetail).Size = new System.Drawing.Size(156, 26);
		((System.Windows.Forms.Control)(object)this.CmbPortDepartmentDetail).TabIndex = 3;
		this.BtnCancelDepartmentDetail.BackColor = System.Drawing.Color.Teal;
		this.BtnCancelDepartmentDetail.FlatAppearance.BorderColor = System.Drawing.Color.Green;
		this.BtnCancelDepartmentDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnCancelDepartmentDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.BtnCancelDepartmentDetail.ForeColor = System.Drawing.Color.White;
		this.BtnCancelDepartmentDetail.Location = new System.Drawing.Point(1126, 82);
		this.BtnCancelDepartmentDetail.Name = "BtnCancelDepartmentDetail";
		this.BtnCancelDepartmentDetail.Size = new System.Drawing.Size(63, 25);
		this.BtnCancelDepartmentDetail.TabIndex = 18;
		this.BtnCancelDepartmentDetail.Text = "Cancel";
		this.BtnCancelDepartmentDetail.UseVisualStyleBackColor = false;
		this.BtnCancelDepartmentDetail.Visible = false;
		this.BtnCancelDepartmentDetail.Click += new System.EventHandler(BtnCancelDepartmentDetail_Click);
		this.BtnUpdateDepartmentDetail.BackColor = System.Drawing.Color.Teal;
		this.BtnUpdateDepartmentDetail.FlatAppearance.BorderColor = System.Drawing.Color.Green;
		this.BtnUpdateDepartmentDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnUpdateDepartmentDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.BtnUpdateDepartmentDetail.ForeColor = System.Drawing.Color.White;
		this.BtnUpdateDepartmentDetail.Location = new System.Drawing.Point(1057, 82);
		this.BtnUpdateDepartmentDetail.Name = "BtnUpdateDepartmentDetail";
		this.BtnUpdateDepartmentDetail.Size = new System.Drawing.Size(66, 25);
		this.BtnUpdateDepartmentDetail.TabIndex = 17;
		this.BtnUpdateDepartmentDetail.Text = "Update";
		this.BtnUpdateDepartmentDetail.UseVisualStyleBackColor = false;
		this.BtnUpdateDepartmentDetail.Visible = false;
		this.BtnUpdateDepartmentDetail.Click += new System.EventHandler(BtnUpdateDepartmentDetail_Click);
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(1110, 19);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(69, 15);
		this.label20.TabIndex = 69;
		this.label20.Text = "Item M.Ton";
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.Location = new System.Drawing.Point(724, 64);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(147, 15);
		this.label32.TabIndex = 65;
		this.label32.Text = "Packing Material Schedule";
		this.txtItemMtonDepartmentDetail.Enabled = false;
		this.txtItemMtonDepartmentDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemMtonDepartmentDetail.Location = new System.Drawing.Point(1110, 38);
		this.txtItemMtonDepartmentDetail.Name = "txtItemMtonDepartmentDetail";
		this.txtItemMtonDepartmentDetail.Size = new System.Drawing.Size(77, 23);
		this.txtItemMtonDepartmentDetail.TabIndex = 8;
		this.txtItemMtonDepartmentDetail.Tag = "WeightMton";
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.Location = new System.Drawing.Point(514, 64);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(115, 15);
		this.label31.TabIndex = 65;
		this.label31.Text = "Inspection Schedule";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(637, 19);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(26, 15);
		this.label21.TabIndex = 67;
		this.label21.Text = "FCL";
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(104, 64);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(118, 15);
		this.label30.TabIndex = 65;
		this.label30.Text = "Production Schedule";
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(276, 19);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(93, 15);
		this.label22.TabIndex = 65;
		this.label22.Text = "Customer Name";
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(873, 64);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(120, 15);
		this.label33.TabIndex = 65;
		this.label33.Text = "Schedule Lock Status";
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.Location = new System.Drawing.Point(789, 19);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(78, 15);
		this.label28.TabIndex = 65;
		this.label28.Text = "Loading Date";
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.Location = new System.Drawing.Point(713, 19);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(46, 15);
		this.label23.TabIndex = 65;
		this.label23.Text = "M.Tons";
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.Location = new System.Drawing.Point(912, 19);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(67, 15);
		this.label29.TabIndex = 65;
		this.label29.Text = "Item Name";
		this.txtScheduleMtonDepartmentDetail.Enabled = false;
		this.txtScheduleMtonDepartmentDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtScheduleMtonDepartmentDetail.Location = new System.Drawing.Point(713, 38);
		this.txtScheduleMtonDepartmentDetail.Name = "txtScheduleMtonDepartmentDetail";
		this.txtScheduleMtonDepartmentDetail.Size = new System.Drawing.Size(74, 23);
		this.txtScheduleMtonDepartmentDetail.TabIndex = 5;
		this.txtScheduleMtonDepartmentDetail.Tag = "NoOfBags";
		this.txtFclDepartmentDetail.Enabled = false;
		this.txtFclDepartmentDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFclDepartmentDetail.Location = new System.Drawing.Point(637, 38);
		this.txtFclDepartmentDetail.Name = "txtFclDepartmentDetail";
		this.txtFclDepartmentDetail.Size = new System.Drawing.Size(74, 23);
		this.txtFclDepartmentDetail.TabIndex = 4;
		this.txtFclDepartmentDetail.Tag = "NoOfBags";
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(479, 19);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(29, 15);
		this.label24.TabIndex = 65;
		this.label24.Text = "Port";
		this.CmbContractDepartmentDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbContractDepartmentDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbContractDepartmentDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbContractDepartmentDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance125).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbContractDepartmentDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.CmbContractDepartmentDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbContractDepartmentDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance127).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance127).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbContractDepartmentDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance127;
		((AppearanceBase)appearance128).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbContractDepartmentDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance128;
		((SpecialBoxBase)((UltraGridBase)this.CmbContractDepartmentDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance129).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance129).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance129).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbContractDepartmentDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.CmbContractDepartmentDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbContractDepartmentDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance130).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbContractDepartmentDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance130;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance131).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbContractDepartmentDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.CmbContractDepartmentDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbContractDepartmentDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbContractDepartmentDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance132;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance133).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbContractDepartmentDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.CmbContractDepartmentDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbContractDepartmentDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance134).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance134).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance134).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbContractDepartmentDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance134;
		((AppearanceBase)appearance135).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbContractDepartmentDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.CmbContractDepartmentDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbContractDepartmentDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance136).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbContractDepartmentDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.CmbContractDepartmentDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbContractDepartmentDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.CmbContractDepartmentDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbContractDepartmentDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbContractDepartmentDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbContractDepartmentDetail).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbContractDepartmentDetail).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbContractDepartmentDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbContractDepartmentDetail).Location = new System.Drawing.Point(3, 36);
		((System.Windows.Forms.Control)(object)this.CmbContractDepartmentDetail).Name = "CmbContractDepartmentDetail";
		((System.Windows.Forms.Control)(object)this.CmbContractDepartmentDetail).Size = new System.Drawing.Size(134, 26);
		((System.Windows.Forms.Control)(object)this.CmbContractDepartmentDetail).TabIndex = 0;
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(3, 19);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(71, 15);
		this.label25.TabIndex = 65;
		this.label25.Text = "Contract No";
		this.CmbPlantDepartmentDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbPlantDepartmentDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPlantDepartmentDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPlantDepartmentDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance139).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPlantDepartmentDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance139;
		((UltraGridBase)this.CmbPlantDepartmentDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPlantDepartmentDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance140).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance140).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPlantDepartmentDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance140;
		((AppearanceBase)appearance141).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPlantDepartmentDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance141;
		((SpecialBoxBase)((UltraGridBase)this.CmbPlantDepartmentDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance142).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance142).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance142).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPlantDepartmentDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.CmbPlantDepartmentDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPlantDepartmentDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance143).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPlantDepartmentDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance143;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance144).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPlantDepartmentDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.CmbPlantDepartmentDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPlantDepartmentDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPlantDepartmentDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance145;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance146).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPlantDepartmentDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.CmbPlantDepartmentDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPlantDepartmentDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance147).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance147).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance147).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance147).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPlantDepartmentDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance147;
		((AppearanceBase)appearance149).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPlantDepartmentDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance149;
		((UltraGridBase)this.CmbPlantDepartmentDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPlantDepartmentDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance150).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPlantDepartmentDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance150;
		((UltraGridBase)this.CmbPlantDepartmentDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPlantDepartmentDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.CmbPlantDepartmentDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPlantDepartmentDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPlantDepartmentDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPlantDepartmentDetail).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPlantDepartmentDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPlantDepartmentDetail).Location = new System.Drawing.Point(232, 81);
		((System.Windows.Forms.Control)(object)this.CmbPlantDepartmentDetail).Name = "CmbPlantDepartmentDetail";
		((System.Windows.Forms.Control)(object)this.CmbPlantDepartmentDetail).Size = new System.Drawing.Size(179, 26);
		((System.Windows.Forms.Control)(object)this.CmbPlantDepartmentDetail).TabIndex = 11;
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.Location = new System.Drawing.Point(232, 64);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(34, 15);
		this.label34.TabIndex = 65;
		this.label34.Text = "Plant";
		this.CmbScheduleNoDepartmentDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbScheduleNoDepartmentDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbScheduleNoDepartmentDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbScheduleNoDepartmentDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance152).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbScheduleNoDepartmentDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance152;
		((UltraGridBase)this.CmbScheduleNoDepartmentDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbScheduleNoDepartmentDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance153).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance153).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance153).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbScheduleNoDepartmentDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance153;
		((AppearanceBase)appearance154).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbScheduleNoDepartmentDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance154;
		((SpecialBoxBase)((UltraGridBase)this.CmbScheduleNoDepartmentDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance155).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance155).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance155).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbScheduleNoDepartmentDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.CmbScheduleNoDepartmentDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbScheduleNoDepartmentDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance156).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbScheduleNoDepartmentDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance156;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance157).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbScheduleNoDepartmentDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.CmbScheduleNoDepartmentDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbScheduleNoDepartmentDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbScheduleNoDepartmentDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance158;
		((AppearanceBase)appearance160).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance160).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbScheduleNoDepartmentDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance160;
		((UltraGridBase)this.CmbScheduleNoDepartmentDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbScheduleNoDepartmentDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance161).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance161).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance161).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance161).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance161).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbScheduleNoDepartmentDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance161;
		((AppearanceBase)appearance162).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbScheduleNoDepartmentDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance162;
		((UltraGridBase)this.CmbScheduleNoDepartmentDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbScheduleNoDepartmentDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance163).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbScheduleNoDepartmentDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance163;
		((UltraGridBase)this.CmbScheduleNoDepartmentDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbScheduleNoDepartmentDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.CmbScheduleNoDepartmentDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbScheduleNoDepartmentDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbScheduleNoDepartmentDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbScheduleNoDepartmentDetail).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbScheduleNoDepartmentDetail).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbScheduleNoDepartmentDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbScheduleNoDepartmentDetail).Location = new System.Drawing.Point(140, 36);
		((System.Windows.Forms.Control)(object)this.CmbScheduleNoDepartmentDetail).Name = "CmbScheduleNoDepartmentDetail";
		((System.Windows.Forms.Control)(object)this.CmbScheduleNoDepartmentDetail).Size = new System.Drawing.Size(134, 26);
		((System.Windows.Forms.Control)(object)this.CmbScheduleNoDepartmentDetail).TabIndex = 1;
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(140, 19);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(75, 15);
		this.label26.TabIndex = 65;
		this.label26.Text = "Schedule No";
		this.panel29.BackColor = System.Drawing.Color.Teal;
		this.panel29.Controls.Add(this.toolStrip5);
		this.panel29.Controls.Add(this.label27);
		this.panel29.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel29.Location = new System.Drawing.Point(3, 3);
		this.panel29.Name = "panel29";
		this.panel29.Size = new System.Drawing.Size(1605, 50);
		this.panel29.TabIndex = 6;
		this.toolStrip5.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip5.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.BtnNewDepartmentDetail, this.BtnRefreshDepartmentDetail });
		this.toolStrip5.Location = new System.Drawing.Point(0, 0);
		this.toolStrip5.Name = "toolStrip5";
		this.toolStrip5.Size = new System.Drawing.Size(1605, 27);
		this.toolStrip5.TabIndex = 55;
		this.toolStrip5.Text = "toolStrip5";
		this.BtnNewDepartmentDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnNewDepartmentDetail.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.BtnNewDepartmentDetail.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnNewDepartmentDetail.Name = "BtnNewDepartmentDetail";
		this.BtnNewDepartmentDetail.Size = new System.Drawing.Size(59, 24);
		this.BtnNewDepartmentDetail.Text = "&New";
		this.BtnNewDepartmentDetail.Click += new System.EventHandler(BtnNewDepartmentDetail_Click);
		this.BtnRefreshDepartmentDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnRefreshDepartmentDetail.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.BtnRefreshDepartmentDetail.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefreshDepartmentDetail.Name = "BtnRefreshDepartmentDetail";
		this.BtnRefreshDepartmentDetail.Size = new System.Drawing.Size(80, 24);
		this.BtnRefreshDepartmentDetail.Text = "&Refresh";
		this.BtnRefreshDepartmentDetail.Click += new System.EventHandler(BtnRefreshDepartmentDetail_Click);
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label27.ForeColor = System.Drawing.Color.White;
		this.label27.Location = new System.Drawing.Point(6, 30);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(261, 17);
		this.label27.TabIndex = 54;
		this.label27.Text = "Pre Shipment Planning and FollowUp";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1619, 699);
		base.Controls.Add(this.tabControlMain);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Name = "frmSaleContractSchedule";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmSaleContractSchedule_FormClosed);
		base.Load += new System.EventHandler(frmSaleContractSchedule_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmSaleContractSchedule_KeyDown);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel9.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel16.ResumeLayout(false);
		this.panel15.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbCropYear).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPackUom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPackingtype).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbAttentiveLoadDateBySchedule).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		this.panel12.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdContractShedule).EndInit();
		this.panel13.ResumeLayout(false);
		this.panel13.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.panel10.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPendingContract).EndInit();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		this.panel14.ResumeLayout(false);
		this.panel14.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panelMainhistory.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		this.panel19.ResumeLayout(false);
		this.panel21.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistoryScheduleDetailByContact).EndInit();
		this.panel23.ResumeLayout(false);
		this.panel23.PerformLayout();
		this.panel20.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistoryScheduleByContract).EndInit();
		this.panel22.ResumeLayout(false);
		this.panel22.PerformLayout();
		this.panel17.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistoryMainByContract).EndInit();
		this.panel18.ResumeLayout(false);
		this.panel18.PerformLayout();
		this.ToolstripHistoryContract.ResumeLayout(false);
		this.ToolstripHistoryContract.PerformLayout();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.tabPage3.ResumeLayout(false);
		this.tabPage3.PerformLayout();
		this.panel26.ResumeLayout(false);
		this.panel27.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel24.ResumeLayout(false);
		this.panel24.PerformLayout();
		this.panel25.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbPort).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSupplierName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemHistory).EndInit();
		this.toolStrip4.ResumeLayout(false);
		this.toolStrip4.PerformLayout();
		this.tabControlMain.ResumeLayout(false);
		this.tabPageMain.ResumeLayout(false);
		this.tabPageDepartmentStatus.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDepartmentDetail).EndInit();
		this.panel28.ResumeLayout(false);
		this.panel28.PerformLayout();
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerDepartmentDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbScheduleLockStatusDepartmentDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemDepartmentDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPortDepartmentDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbContractDepartmentDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPlantDepartmentDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbScheduleNoDepartmentDetail).EndInit();
		this.panel29.ResumeLayout(false);
		this.panel29.PerformLayout();
		this.toolStrip5.ResumeLayout(false);
		this.toolStrip5.PerformLayout();
		base.ResumeLayout(false);
	}
}
