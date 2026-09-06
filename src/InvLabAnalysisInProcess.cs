using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Architecture.BLL.Lab;
using Architecture.BLL.Production;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Authentication;
using Architecture.Model.Lab;
using Architecture.Model.Production;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;
using grid.User_Conrtols;
using InheritableBoolean = Janus.Windows.GridEX.InheritableBoolean;
using GroupTotals = Janus.Windows.GridEX.GroupTotals;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.Lab
{

public class InvLabAnalysisInProcess : Form
{
	private Attachment AT = new Attachment();

	private DropDownBind DDL = new DropDownBind();

	private Rightsobjects formrights = new Rightsobjects();

	public UserAccount UserAccount = new UserAccount();

	private int RecId;

	private bool UpdateMode;

	public static Image imgpreview;

	private bool Confiquration;

	private string saveDirectory = "";

	private string fileSavePath = "";

	private string ofd = "";

	private string fileSavePathcookingpic = "";

	private string ofdcookingpic = "";

	private DataTable grdtable = new DataTable();

	private DataTable GetByPlantdt = new DataTable();

	private DataTable GetByAnalysisGroupdt = new DataTable();

	private IContainer components;

	private ToolStripButton btnprint;

	private ToolStripButton btnSave;

	private ToolStripButton btnUpdate;

	private ToolStripButton btnNew;

	private ToolStrip btn;

	private ToolStripButton btnattachment;

	internal GridEX grdAnalysisGroup;

	private TabPage tabHistory;

	private Panel panel4;

	internal GridEX grdhistory;

	private TabControl tabControl1;

	private TabPage tabForm;

	private Panel panel1;

	private Panel panel7;

	private Button btnimgprevcookingpic;

	private Button btnprv;

	private Label label3;

	private Label label2;

	private Button btnimage2;

	private Button btnimg1;

	private PictureBox cookingpic;

	private PictureBox sampleanalysispicture;

	private TextBox txtremarks;

	private TextBox txtdocno;

	private Label label10;

	private Label label12;

	private Label txtsampledate;

	private Label label8;

	private Label label7;

	private DateTimePicker txtanalysistime;

	private DateTimePicker txtdocdate;

	private Label label1;

	private Label label13;

	private TextBox txtAnalystPerson;

	private Label label15;

	private Label label5;

	internal UltraCombo cmbproductionNo;

	internal UltraCombo cmbCropYear;

	internal UltraCombo cmbitem;

	internal UltraCombo cmbanalysisGroup;

	private Panel panel2;

	private Panel panel3;

	internal GridEX grdStepAnalysis;

	internal UltraCombo cmbPlantName;

	private Label label4;

	private ToolStripButton BtnRefresh;

	private Panel panel8;

	internal GridEX grdGroupAnalysisHistory;

	private Panel panel6;

	internal GridEX grdStepAnalysisHistory;

	private Panel panel5;

	private Panel panel9;

	private Label label6;

	private Panel panel10;

	private Panel panel12;

	private Label label11;

	private Panel panel11;

	private Label label9;

	private CtrlGrdBar crtgrdHistory;

	private CheckBox chkPreview;

	private Label label14;

	private Panel panel14;

	private Label label16;

	private Panel panel13;

	private CtrlGrdBar ctrlGrdBar3;

	private CtrlGrdBar ctrlGrdBar2;

	private Panel panel15;

	private Label label17;

	private Button btnCookingPicClear;

	private Button btnAnalysisPictureClear;

	private Panel panel16;

	private ToolStrip toolStrip1;

	private ToolStripButton btnNewHistory;

	private ToolStripButton btnRefreshHistory;

	private Panel panel20;

	private GroupBox groupBox2;

	private Label label24;

	private Label label23;

	internal UltraCombo CmbAnalystHistory;

	internal UltraCombo CmbItemHistory;

	private Button btnShowHistory;

	private Label label56;

	private Label label57;

	private TextBox ToDocNoHistory;

	private TextBox FromDocNoHistory;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	private Label label19;

	internal UltraCombo CmbPlantHistory;

	private Label label18;

	internal UltraCombo CmbJobOrderHistory;

	public InvLabAnalysisInProcess(UserAccount pUserAccount)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUserAccount;
	}

	private void InvLabPurchaseAnalysis_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1200, 650);
			formrights = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnSave.Enabled = formrights.DoHaveSaveRight;
			btnprint.Enabled = formrights.DoHavePrintRights;
			btnUpdate.Enabled = formrights.DoHaveUpdateRights;
			ctrlGrdBar2.mGridChooseFielder.Enabled = formrights.DoHaveFieldChooserRights;
			ctrlGrdBar2.mGridSaveLayouts.Enabled = formrights.DoHaveSaveLayoutRights;
			ctrlGrdBar2.mGridPrint.Enabled = formrights.DoHaveGridPrintRights;
			ctrlGrdBar2.mGridExport.Enabled = formrights.DoHaveGridExportRights;
			ctrlGrdBar2.GroupCollaps.Enabled = formrights.DoHaveGridGroupCollapseRights;
			ctrlGrdBar2.GroupExpand.Enabled = formrights.DoHaveGridGroupExpandRights;
			PlantFill();
			JobOrderNoFill();
			GenerateCode();
			AnalysisGroup();
			CropYearFill();
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			chkPreview.Checked = true;
			GetByAnalysisGroupdt.Columns.Add("Id");
			GetByAnalysisGroupdt.Columns.Add("AnalysisGroupId");
			GetByAnalysisGroupdt.Columns.Add("AnalysisGroupName");
			GetByAnalysisGroupdt.Columns.Add("AnalysisParameterId");
			GetByAnalysisGroupdt.Columns.Add("AnalysisParameter");
			GetByAnalysisGroupdt.Columns.Add("MinValue");
			GetByAnalysisGroupdt.Columns.Add("MaxValue");
			GetByAnalysisGroupdt.Columns.Add("ResultValue", typeof(double));
			GetByAnalysisGroupdt.Columns.Add("RemarksDetail");
			grdAnalysisGroup.DataSource = GetByAnalysisGroupdt;
			grdAnalysisGroup.RetrieveStructure();
			grdAnalysisGroupSetting();
			GetByPlantdt.Columns.Add("Id");
			GetByPlantdt.Columns.Add("PlantId");
			GetByPlantdt.Columns.Add("PlantName");
			GetByPlantdt.Columns.Add("SortNo");
			GetByPlantdt.Columns.Add("StepId");
			GetByPlantdt.Columns.Add("StepName");
			GetByPlantdt.Columns.Add("AnalysisParameterId");
			GetByPlantdt.Columns.Add("AnalysisParameter");
			GetByPlantdt.Columns.Add("ResultValue", typeof(double));
			GetByPlantdt.Columns.Add("Caption1", typeof(double));
			GetByPlantdt.Columns.Add("Caption2", typeof(double));
			GetByPlantdt.Columns.Add("Caption3", typeof(double));
			GetByPlantdt.Columns.Add("Caption4", typeof(double));
			GetByPlantdt.Columns.Add("Caption5", typeof(double));
			GetByPlantdt.Columns.Add("Average", typeof(double));
			GetByPlantdt.Columns.Add("RemarksDetail");
			GetConfigurationsFromGlobalAndBindValuesInColumns();
			FromDateHistory.Value = DateTime.Now.AddDays(-7.0);
			ToDateHistory.Value = DateTime.Now;
			HistoryComboFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetConfigurationsFromGlobalAndBindValuesInColumns()
	{
		try
		{
			int cropYearId = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("Default Crop Year"));
			if (cropYearId != 0 && ((UltraGridBase)cmbCropYear).DataSource != null)
			{
				cmbCropYear.Value = cropYearId;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show("Configuration Error" + ex.Message);
		}
	}

	private void HistoryComboFill()
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		try
		{
			DataTable dtJobOrder = new DataTable();
			dtJobOrder.Columns.Add("Id");
			dtJobOrder.Columns.Add("name");
			DataTable dtPlant = dtJobOrder.Clone();
			DataTable dtItem = dtJobOrder.Clone();
			DataTable dtAnalyst = dtJobOrder.Clone();
			DataTable dt = LabInProcessAnalysisRawHeader.GetDataForDropDownFromLabInProcessAnalysisRawHeader(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeIds = "306"
			});
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToString(dt.Rows[i]["Activity"]) == "Item")
					{
						dtItem.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
					else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "AnalystPerson")
					{
						dtAnalyst.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
					else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "JobOrder")
					{
						dtJobOrder.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
					else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "Plant")
					{
						dtPlant.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
				}
			}
			DDL.BindDDL(dtItem, CmbItemHistory, "Id", "name", "Item Name", ZeroIndex: false);
			DDL.BindDDL(dtAnalyst, CmbAnalystHistory, "Id", "name", "Analyst Person", ZeroIndex: false);
			DDL.BindDDL(dtJobOrder, CmbJobOrderHistory, "Id", "name", "Job Order", ZeroIndex: false);
			DDL.BindDDL(dtPlant, CmbPlantHistory, "Id", "name", "Plant", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateCode()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			LabInProcessAnalysisRawHeader code = new LabInProcessAnalysisRawHeader();
			txtdocno.Text = LabInProcessAnalysisRawHeader.GenerateCode(code).ToString();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PlantFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		try
		{
			InvProductionPlant val = new InvProductionPlant
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			};
			DataTable dtpp = new DataTable();
			dtpp = InvProductionPlant.GetAll(val);
			if (dtpp.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtpp, cmbPlantName, "Id", "Description", "Plant", false);
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
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		try
		{
			DataTable dtPJO = InvProductionJobOrder.InvProductionJobOrder_GetForInProcessLab(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				Status = "In Process",
				RecId = RecId
			});
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
					DDL.BindDDL(dt, cmbproductionNo, "Id", "PlanCode", "JobOrderNo", ZeroIndex: false);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private DataTable GetItemsBindByJobOrderDbCall(int JobOrderId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		try
		{
			return InvFoodProduction.InvFoodProduction_GetBrandItemsByJobOrder(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				JobOrderId = JobOrderId,
				DocumentTypeIds = "80,112"
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ItemNameBind(DataTable lst)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(cmbitem, lst, "Id", "ItemName", "Item Name", AllColumns: true, Conversion.ToInt(cmbitem.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CropYearFill()
	{
		try
		{
			DataTable dt = CommonServices.CropYearGetAllService();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbCropYear, "Id", "CropYear", "Crop Year", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AnalysisGroup()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		try
		{
			ReportsParameters val = new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			};
			DataTable dtLAG = new DataTable();
			dtLAG = InvLabAnalysisGroup.GetAllOrById(val);
			if (dtLAG.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtLAG, cmbanalysisGroup, "Id", "AnalysisGroupDescription", "Analysis Group", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbproductionNo_Leave(object sender, EventArgs e)
	{
		try
		{
			ItemNameBind(GetItemsBindByJobOrderDbCall(Conversion.ToInt(cmbproductionNo.Value)));
			TopFiveStepAnalysisResultsAgainstPlantJobOrderAndItem();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbanalysisGroup_Leave(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		try
		{
			GetByAnalysisGroupdt.Rows.Clear();
			DataTable GroupAnalysisdt = LabInProcessAnalysisRawHeader.GetByAnalysisGroupId(new LabInProcessAnalysisRawHeader
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				AnalysisGroupId = Conversion.ToInt(cmbanalysisGroup.Value)
			});
			if (GroupAnalysisdt.Rows.Count > 0)
			{
				for (int i = 0; i < GroupAnalysisdt.Rows.Count; i++)
				{
					GetByAnalysisGroupdt.Rows.Add(GroupAnalysisdt.Rows[i]["Id"], GroupAnalysisdt.Rows[i]["InvLabAnalysisGroup"], GroupAnalysisdt.Rows[i]["AnalysisGroupDescription"], GroupAnalysisdt.Rows[i]["InvLabAnalysisItems"], GroupAnalysisdt.Rows[i]["AnalysisParameterCode"], GroupAnalysisdt.Rows[i]["MinValue"], GroupAnalysisdt.Rows[i]["MaxValue"], Conversion.ToDouble(GroupAnalysisdt.Rows[i]["InAnalysisResult"]), GroupAnalysisdt.Rows[i]["RemarksDetail"]);
				}
			}
			grdAnalysisGroup.DataSource = GetByAnalysisGroupdt;
			grdAnalysisGroup.RetrieveStructure();
			grdAnalysisGroupSetting();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdAnalysisGroupSetting()
	{
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdAnalysisGroup.AllowDelete = (InheritableBoolean)2;
			grdAnalysisGroup.AllowEdit = (InheritableBoolean)1;
			grdAnalysisGroup.RootTable.Columns["Id"].EditType = (EditType)0;
			grdAnalysisGroup.RootTable.Columns["AnalysisGroupId"].EditType = (EditType)0;
			grdAnalysisGroup.RootTable.Columns["AnalysisParameterId"].EditType = (EditType)0;
			grdAnalysisGroup.RootTable.Columns["AnalysisGroupName"].EditType = (EditType)0;
			grdAnalysisGroup.RootTable.Columns["MinValue"].EditType = (EditType)0;
			grdAnalysisGroup.RootTable.Columns["MaxValue"].EditType = (EditType)0;
			grdAnalysisGroup.RootTable.Columns["AnalysisParameter"].EditType = (EditType)0;
			grdAnalysisGroup.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grdAnalysisGroup.RootTable.Columns)
			{
				item.HeaderAlignment = (TextAlignment)2;
			}
			grdAnalysisGroup.RootTable.Columns["AnalysisParameter"].FilterEditType = (FilterEditType)1;
			grdAnalysisGroup.RootTable.Columns["MinValue"].FilterEditType = (FilterEditType)1;
			grdAnalysisGroup.RootTable.Columns["MaxValue"].FilterEditType = (FilterEditType)1;
			grdAnalysisGroup.RootTable.Columns["Id"].Visible = false;
			grdAnalysisGroup.RootTable.Columns["AnalysisGroupId"].Visible = false;
			grdAnalysisGroup.RootTable.Columns["AnalysisParameterId"].Visible = false;
			grdAnalysisGroup.RootTable.Columns["AnalysisGroupName"].Visible = false;
			grdAnalysisGroup.RootTable.Columns["AnalysisParameter"].Width = 150;
			grdAnalysisGroup.RootTable.Columns["AnalysisGroupName"].Width = 140;
			grdAnalysisGroup.RootTable.Columns["RemarksDetail"].Width = 100;
			grdAnalysisGroup.RootTable.Columns["MinValue"].Width = 65;
			grdAnalysisGroup.RootTable.Columns["MaxValue"].Width = 65;
			grdAnalysisGroup.RootTable.Columns["ResultValue"].Width = 65;
			grdAnalysisGroup.RootTable.Columns["MaxValue"].TextAlignment = (TextAlignment)3;
			grdAnalysisGroup.RootTable.Columns["MaxValue"].FormatString = "#,##0.####";
			grdAnalysisGroup.RootTable.Columns["MinValue"].TextAlignment = (TextAlignment)3;
			grdAnalysisGroup.RootTable.Columns["MinValue"].FormatString = "#,##0.####";
			grdAnalysisGroup.RootTable.Columns["ResultValue"].TextAlignment = (TextAlignment)3;
			grdAnalysisGroup.RootTable.Columns["ResultValue"].FormatString = "#,##0.####";
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		grdAnalysisGroup.UpdateData();
	}

	private void cmbPlantName_Leave(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		try
		{
			GetByPlantdt.Rows.Clear();
			DataTable StepAnalysisdt = LabInProcessAnalysisRawHeader.GetByPlantId(new LabInProcessAnalysisRawHeader
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				PlantId = Conversion.ToInt(cmbPlantName.Value)
			});
			if (StepAnalysisdt.Rows.Count > 0)
			{
				for (int i = 0; i < StepAnalysisdt.Rows.Count; i++)
				{
					GetByPlantdt.Rows.Add(StepAnalysisdt.Rows[i]["Id"], StepAnalysisdt.Rows[i]["PlantId"], StepAnalysisdt.Rows[i]["PlantName"], StepAnalysisdt.Rows[i]["SortNo"], StepAnalysisdt.Rows[i]["LabInProcessAnalysisStepId"], StepAnalysisdt.Rows[i]["ProcessStepName"], StepAnalysisdt.Rows[i]["AnalysisParameterId"], StepAnalysisdt.Rows[i]["AnalysisParameterCode"], Conversion.ToDouble(StepAnalysisdt.Rows[i]["ResultValue"]), 0, 0, 0, 0, 0, 0, StepAnalysisdt.Rows[i]["RemarkDetail"]);
				}
			}
			grdStepAnalysis.DataSource = GetByPlantdt;
			grdStepAnalysis.RetrieveStructure();
			grdStepAnalysisSetting();
			TopFiveStepAnalysisResultsAgainstPlantJobOrderAndItem();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdStepAnalysisSetting()
	{
		//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdStepAnalysis.RootTable.Columns["Id"].EditType = (EditType)0;
			grdStepAnalysis.RootTable.Columns["PlantId"].EditType = (EditType)0;
			grdStepAnalysis.RootTable.Columns["StepId"].EditType = (EditType)0;
			grdStepAnalysis.RootTable.Columns["AnalysisParameterId"].EditType = (EditType)0;
			grdStepAnalysis.RootTable.Columns["SortNo"].EditType = (EditType)0;
			grdStepAnalysis.RootTable.Columns["PlantName"].EditType = (EditType)0;
			grdStepAnalysis.RootTable.Columns["StepName"].EditType = (EditType)0;
			grdStepAnalysis.RootTable.Columns["Caption1"].EditType = (EditType)0;
			grdStepAnalysis.RootTable.Columns["Caption2"].EditType = (EditType)0;
			grdStepAnalysis.RootTable.Columns["Caption3"].EditType = (EditType)0;
			grdStepAnalysis.RootTable.Columns["Caption4"].EditType = (EditType)0;
			grdStepAnalysis.RootTable.Columns["Caption5"].EditType = (EditType)0;
			grdStepAnalysis.RootTable.Columns["Average"].EditType = (EditType)0;
			grdStepAnalysis.RootTable.Columns["AnalysisParameter"].EditType = (EditType)0;
			grdStepAnalysis.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grdStepAnalysis.RootTable.Columns)
			{
				item.HeaderAlignment = (TextAlignment)2;
			}
			grdStepAnalysis.RootTable.Groups.Add("StepName");
			grdStepAnalysis.HideColumnsWhenGrouped = (InheritableBoolean)1;
			grdStepAnalysis.GroupTotals = (GroupTotals)2;
			grdStepAnalysis.RootTable.Columns["StepName"].FilterEditType = (FilterEditType)1;
			grdStepAnalysis.RootTable.Columns["AnalysisParameter"].FilterEditType = (FilterEditType)1;
			grdStepAnalysis.RootTable.Columns["Id"].Visible = false;
			grdStepAnalysis.RootTable.Columns["StepId"].Visible = false;
			grdStepAnalysis.RootTable.Columns["PlantId"].Visible = false;
			grdStepAnalysis.RootTable.Columns["PlantName"].Visible = false;
			grdStepAnalysis.RootTable.Columns["AnalysisParameterId"].Visible = false;
			grdStepAnalysis.RootTable.Columns["AnalysisParameter"].Width = 220;
			grdStepAnalysis.RootTable.Columns["SortNo"].Width = 60;
			grdStepAnalysis.RootTable.Columns["ResultValue"].Width = 60;
			grdStepAnalysis.RootTable.Columns["RemarksDetail"].Width = 230;
			grdStepAnalysis.RootTable.Columns["Average"].Width = 60;
			grdStepAnalysis.RootTable.Columns["Caption1"].TextAlignment = (TextAlignment)2;
			grdStepAnalysis.RootTable.Columns["Caption1"].FormatString = "#,##0.####";
			grdStepAnalysis.RootTable.Columns["Caption2"].TextAlignment = (TextAlignment)2;
			grdStepAnalysis.RootTable.Columns["Caption2"].FormatString = "#,##0.####";
			grdStepAnalysis.RootTable.Columns["Caption3"].TextAlignment = (TextAlignment)2;
			grdStepAnalysis.RootTable.Columns["Caption3"].FormatString = "#,##0.####";
			grdStepAnalysis.RootTable.Columns["Caption4"].TextAlignment = (TextAlignment)2;
			grdStepAnalysis.RootTable.Columns["Caption4"].FormatString = "#,##0.####";
			grdStepAnalysis.RootTable.Columns["Caption5"].TextAlignment = (TextAlignment)2;
			grdStepAnalysis.RootTable.Columns["Caption5"].FormatString = "#,##0.####";
			grdStepAnalysis.RootTable.Columns["Average"].TextAlignment = (TextAlignment)2;
			grdStepAnalysis.RootTable.Columns["Average"].FormatString = "#,##0.####";
			grdStepAnalysis.RootTable.Columns["ResultValue"].TextAlignment = (TextAlignment)2;
			grdStepAnalysis.RootTable.Columns["ResultValue"].FormatString = "#,##0.####";
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void TopFiveStepAnalysisResultsAgainstPlantJobOrderAndItem()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		try
		{
			DataTable dtTop = LabInProcessAnalysisRawHeader.Top_InProcessAnalysisDataWithAnalysistimeAndResult(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				PlantId = Conversion.ToInt(cmbPlantName.Value),
				JobOrderId = Conversion.ToInt(cmbproductionNo.Value)
			});
			if (dtTop.Rows.Count > 0)
			{
				string Caption1 = Conversion.ToDateTime(dtTop.Rows[0]["dtm_01"]).ToString("dd-MMM-yyyy hh:mm tt");
				string Caption2 = Conversion.ToDateTime(dtTop.Rows[0]["dtm_02"]).ToString("dd-MMM-yyyy hh:mm tt");
				string Caption3 = Conversion.ToDateTime(dtTop.Rows[0]["dtm_03"]).ToString("dd-MMM-yyyy hh:mm tt");
				string Caption4 = Conversion.ToDateTime(dtTop.Rows[0]["dtm_04"]).ToString("dd-MMM-yyyy hh:mm tt");
				string Caption5 = Conversion.ToDateTime(dtTop.Rows[0]["dtm_05"]).ToString("dd-MMM-yyyy hh:mm tt");
				if (GetByPlantdt.Rows.Count > 0)
				{
					foreach (DataRow row in GetByPlantdt.Rows)
					{
						int stepId = Conversion.ToInt(row["StepId"]);
						int paramId = Conversion.ToInt(row["AnalysisParameterId"]);
						for (int i = 0; i < dtTop.Rows.Count; i++)
						{
							int TopStepId = Conversion.ToInt(dtTop.Rows[i]["StepId"]);
							int TopParamId = Conversion.ToInt(dtTop.Rows[i]["AnalysisParameterId"]);
							if (stepId == TopStepId && paramId == TopParamId)
							{
								row["Caption1"] = ((Conversion.ToDouble(dtTop.Rows[i]["Value_01"]) > 0.0) ? Conversion.ToDouble(dtTop.Rows[i]["Value_01"]) : 0.0);
								row["Caption2"] = ((Conversion.ToDouble(dtTop.Rows[i]["Value_02"]) > 0.0) ? Conversion.ToDouble(dtTop.Rows[i]["Value_02"]) : 0.0);
								row["Caption3"] = ((Conversion.ToDouble(dtTop.Rows[i]["Value_03"]) > 0.0) ? Conversion.ToDouble(dtTop.Rows[i]["Value_03"]) : 0.0);
								row["Caption4"] = ((Conversion.ToDouble(dtTop.Rows[i]["Value_04"]) > 0.0) ? Conversion.ToDouble(dtTop.Rows[i]["Value_04"]) : 0.0);
								row["Caption5"] = ((Conversion.ToDouble(dtTop.Rows[i]["Value_05"]) > 0.0) ? Conversion.ToDouble(dtTop.Rows[i]["Value_05"]) : 0.0);
								row["Average"] = ((Conversion.ToDouble(dtTop.Rows[i]["Average"]) > 0.0) ? Conversion.ToDouble(dtTop.Rows[i]["Average"]) : 0.0);
							}
						}
					}
				}
				grdStepAnalysis.RootTable.Columns["Caption1"].Caption = ((!Conversion.CheckDateTimeNull(Conversion.ToDateTime((object)Caption1)) && Caption1 != string.Empty) ? Caption1 : DateTime.Now.ToString("dd-MMM-yyyy HH:mm tt"));
				grdStepAnalysis.RootTable.Columns["Caption2"].Caption = ((!Conversion.CheckDateTimeNull(Conversion.ToDateTime((object)Caption2)) && Caption2 != string.Empty) ? Caption2 : DateTime.Now.ToString("dd-MMM-yyyy HH:mm tt"));
				grdStepAnalysis.RootTable.Columns["Caption3"].Caption = ((!Conversion.CheckDateTimeNull(Conversion.ToDateTime((object)Caption3)) && Caption3 != string.Empty) ? Caption3 : DateTime.Now.ToString("dd-MMM-yyyy HH:mm tt"));
				grdStepAnalysis.RootTable.Columns["Caption4"].Caption = ((!Conversion.CheckDateTimeNull(Conversion.ToDateTime((object)Caption4)) && Caption4 != string.Empty) ? Caption4 : DateTime.Now.ToString("dd-MMM-yyyy HH:mm tt"));
				grdStepAnalysis.RootTable.Columns["Caption5"].Caption = ((!Conversion.CheckDateTimeNull(Conversion.ToDateTime((object)Caption5)) && Caption5 != string.Empty) ? Caption5 : DateTime.Now.ToString("dd-MMM-yyyy HH:mm tt"));
				return;
			}
			if (GetByPlantdt.Rows.Count > 0)
			{
				foreach (DataRow row2 in GetByPlantdt.Rows)
				{
					row2["Caption1"] = 0;
					row2["Caption2"] = 0;
					row2["Caption3"] = 0;
					row2["Caption4"] = 0;
					row2["Caption5"] = 0;
				}
			}
			grdStepAnalysis.RootTable.Columns["Caption1"].Caption = "Caption1";
			grdStepAnalysis.RootTable.Columns["Caption2"].Caption = "Caption2";
			grdStepAnalysis.RootTable.Columns["Caption3"].Caption = "Caption3";
			grdStepAnalysis.RootTable.Columns["Caption4"].Caption = "Caption4";
			grdStepAnalysis.RootTable.Columns["Caption5"].Caption = "Caption5";
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private bool formvalidation()
	{
		if (((Control)(object)cmbPlantName).Text.Trim() == "")
		{
			MessageBox.Show("Please Select Plant Name");
			cmbPlantName.Focus();
			return false;
		}
		if (((Control)(object)cmbproductionNo).Text.Trim() == "")
		{
			MessageBox.Show("Please Select Job Order No");
			cmbproductionNo.Focus();
			return false;
		}
		if (((UltraGridBase)cmbitem).ActiveRow == null)
		{
			MessageBox.Show("Please Select Item");
			cmbitem.Focus();
			return false;
		}
		if (((UltraGridBase)cmbCropYear).ActiveRow == null)
		{
			MessageBox.Show("Please Select Crop Year");
			cmbCropYear.Focus();
			return false;
		}
		if (((UltraGridBase)cmbanalysisGroup).ActiveRow == null)
		{
			MessageBox.Show("Please Select Analysis Group");
			cmbanalysisGroup.Focus();
			return false;
		}
		if (txtAnalystPerson.Text.Trim() == "")
		{
			MessageBox.Show("Please Enter Analyst Person");
			txtAnalystPerson.Focus();
			return false;
		}
		return true;
	}

	private void Insert()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0306: Expected O, but got Unknown
		//IL_03dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e3: Expected O, but got Unknown
		try
		{
			if (!formvalidation())
			{
				return;
			}
			LabInProcessAnalysisRawHeader labheader = new LabInProcessAnalysisRawHeader();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				labheader.Id = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			if (grdAnalysisGroup.RowCount > 0 && grdStepAnalysis.RowCount > 0)
			{
				GridEXRow[] dataRows = grdStepAnalysis.GetDataRows();
				foreach (GridEXRow row2 in dataRows)
				{
					if (Conversion.ToString(row2.Cells["ResultValue"].Value) == string.Empty)
					{
						MessageBox.Show("PLease Enter Result Value In Step Analysis Grid...    In Row No " + (row2.RowIndex + 1));
						return;
					}
				}
				dataRows = grdAnalysisGroup.GetDataRows();
				foreach (GridEXRow row in dataRows)
				{
					if (Conversion.ToString(row.Cells["ResultValue"].Value) == string.Empty)
					{
						MessageBox.Show("PLease Enter Result Value In Group Analysis Grid...    In Row No " + (row.RowIndex + 1) + " " + Conversion.ToString(row.Cells["AnalysisParameterCode"].Value));
						return;
					}
				}
				labheader.DocumentTypeId = 306;
				labheader.DocNo = Conversion.ToInt((object)txtdocno.Text);
				labheader.DocDate = Conversion.ToDateTime((object)txtdocdate.Value);
				labheader.AnalysisTime = Conversion.ToDateTime((object)txtanalysistime.Value);
				labheader.PlantId = Conversion.ToInt(cmbPlantName.Value);
				labheader.JobOrderId = Conversion.ToInt(cmbproductionNo.Value);
				labheader.AnalystPerson = Conversion.ToString((object)txtAnalystPerson.Text);
				labheader.CropYearId = Conversion.ToInt(cmbCropYear.Value);
				labheader.IsApproved = false;
				labheader.ItemId = Conversion.ToInt(cmbitem.Value);
				labheader.AnalysisGroupId = Conversion.ToInt(cmbanalysisGroup.Value);
				labheader.RemarksHeader = Conversion.ToString((object)txtremarks.Text);
				labheader.OrganizationId = UserAccount.OrganizationId;
				labheader.CompanyId = UserAccount.CompanyId;
				labheader.EntryUser = UserAccount.ID;
				labheader.ModifyUser = UserAccount.ID;
				labheader.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
				labheader.AnalysisInProcessDetail = new List<InvLabAnalysisInProcessDetail>();
				labheader.LabInProcessAnalysisRawDetail = new List<LabInProcessAnalysisRawDetail>();
				dataRows = grdAnalysisGroup.GetDataRows();
				foreach (GridEXRow r2 in dataRows)
				{
					InvLabAnalysisInProcessDetail labdetail = new InvLabAnalysisInProcessDetail();
					labdetail.InvLabGroupAnalysisStandardsId = Conversion.ToInt(r2.Cells["AnalysisGroupId"].Value);
					labdetail.InAnalysisResult = Conversion.ToDouble(r2.Cells["ResultValue"].Value);
					labdetail.RemarksDetail = Conversion.ToString((object)r2.Cells["RemarksDetail"].Text);
					labdetail.AnalysisParameterID = Conversion.ToInt((object)r2.Cells["AnalysisParameterId"].Text);
					labdetail.Analysistime = Conversion.ToDateTime((object)txtanalysistime.Text);
					labheader.AnalysisInProcessDetail.Add(labdetail);
				}
				dataRows = grdStepAnalysis.GetDataRows();
				foreach (GridEXRow r in dataRows)
				{
					LabInProcessAnalysisRawDetail labdetailRaw = new LabInProcessAnalysisRawDetail();
					labdetailRaw.ParameterId = Conversion.ToInt(r.Cells["AnalysisParameterId"].Value);
					labdetailRaw.StepId = Conversion.ToInt(r.Cells["StepId"].Value);
					labdetailRaw.ResultValue = Conversion.ToDecimal(r.Cells["ResultValue"].Value);
					labdetailRaw.RemarkDetail = Conversion.ToString((object)r.Cells["RemarksDetail"].Text);
					labdetailRaw.Analysistime = Conversion.ToDateTime((object)txtanalysistime.Text);
					labheader.LabInProcessAnalysisRawDetail.Add(labdetailRaw);
				}
				labheader.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
				labheader.DeleteAttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
				FormHelper.UpdateAttachmentsForObject<LabInProcessAnalysisRawHeader>(labheader, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
				labheader.AnalysisPic = fileSavePath;
				labheader.CookingPic = fileSavePathcookingpic;
				List<string> DeleteImgList = new List<string>();
				if (Conversion.ToString(sampleanalysispicture.Tag) != null && Conversion.ToString(sampleanalysispicture.Tag) != string.Empty && sampleanalysispicture.Image == null)
				{
					DeleteImgList.Add(Conversion.ToString(sampleanalysispicture.Tag));
				}
				if (Conversion.ToString(cookingpic.Tag) != null && Conversion.ToString(cookingpic.Tag) != string.Empty && cookingpic.Image == null)
				{
					DeleteImgList.Add(Conversion.ToString(cookingpic.Tag));
				}
				int success = LabInProcessAnalysisRawHeader.Save(labheader);
				if (RecId > 0)
				{
					MessageBox.Show("Update Successfully");
					FormHelper.DeleteAllAttachmentsFromDb(AT, RecId, 306, base.Name);
					foreach (string item in DeleteImgList)
					{
						string Path = clsGlobalVariables.AttachmentFolderPath + "\\" + item;
						if (!File.Exists(Path))
						{
							continue;
						}
						using (FileStream fs = new FileStream(Path, FileMode.Open, FileAccess.ReadWrite))
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
								File.Delete(Path);
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
				else
				{
					MessageBox.Show("Save Successfully");
				}
				if (ofd != null && ofd != string.Empty)
				{
					if (!Directory.Exists(clsGlobalVariables.AttachmentFolderPath))
					{
						throw new Exception("Attachment Folder Path Not Configure Properly");
					}
					fileSavePath = System.IO.Path.Combine(clsGlobalVariables.AttachmentFolderPath, fileSavePath);
					File.Copy(ofd, fileSavePath, overwrite: true);
					ofd = string.Empty;
					fileSavePath = string.Empty;
					sampleanalysispicture.Image = null;
					sampleanalysispicture.Invalidate();
				}
				if (ofdcookingpic != null && ofdcookingpic != string.Empty)
				{
					if (!Directory.Exists(clsGlobalVariables.AttachmentFolderPath))
					{
						throw new Exception("Attachment Folder Path Not Configure Properly");
					}
					fileSavePathcookingpic = System.IO.Path.Combine(clsGlobalVariables.AttachmentFolderPath, fileSavePathcookingpic);
					File.Copy(ofdcookingpic, fileSavePathcookingpic, overwrite: true);
					ofdcookingpic = string.Empty;
					fileSavePathcookingpic = string.Empty;
					cookingpic.Image = null;
					cookingpic.Invalidate();
				}
				refresh();
				if (chkPreview.Checked)
				{
					CommonServices.LabAnalysisReport659(success);
				}
			}
			else
			{
				MessageBox.Show("Grid Record Not Found");
			}
		}
		catch (Exception ex2)
		{
			MessageBox.Show(ex2.Message);
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

	private void ReadById(int ID)
	{
		//IL_0396: Unknown result type (might be due to invalid IL or missing references)
		//IL_039b: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ce: Expected O, but got Unknown
		try
		{
			RecId = ID;
			DataTable dtgetbyId = LabInProcessAnalysisRawHeader.GetById(RecId);
			if (dtgetbyId.Rows.Count <= 0)
			{
				return;
			}
			tabControl1.SelectedIndex = 0;
			txtdocno.Text = Conversion.ToString(dtgetbyId.Rows[0]["DocNo"]);
			txtdocdate.Text = Conversion.ToString(dtgetbyId.Rows[0]["DocDate"]);
			cmbCropYear.Value = Conversion.ToInt(dtgetbyId.Rows[0]["CropYearId"]);
			cmbproductionNo.Value = Conversion.ToInt(dtgetbyId.Rows[0]["JobOrderId"]);
			ItemNameBind(GetItemsBindByJobOrderDbCall(Conversion.ToInt(cmbproductionNo.Value)));
			cmbanalysisGroup.Value = Conversion.ToInt(dtgetbyId.Rows[0]["AnalysisGroupId"]);
			cmbPlantName.Value = Conversion.ToInt(dtgetbyId.Rows[0]["PlantId"]);
			cmbitem.Value = Conversion.ToInt(dtgetbyId.Rows[0]["ItemId"]);
			txtanalysistime.Text = Conversion.ToString(dtgetbyId.Rows[0]["AnalysisTime"]);
			txtremarks.Text = Conversion.ToString(dtgetbyId.Rows[0]["RemarksHeader"]);
			txtAnalystPerson.Text = Conversion.ToString(dtgetbyId.Rows[0]["AnalystPerson"]);
			btnSave.Visible = false;
			btnUpdate.Visible = true;
			((Control)(object)cmbanalysisGroup).Enabled = false;
			fileSavePath = Conversion.ToString(dtgetbyId.Rows[0]["AnalysisPic"]);
			fileSavePathcookingpic = Conversion.ToString(dtgetbyId.Rows[0]["CookingPic"]);
			if (fileSavePath != null && fileSavePath != "")
			{
				string DrvPicPath2 = clsGlobalVariables.AttachmentFolderPath + "\\" + fileSavePath;
				if (File.Exists(DrvPicPath2))
				{
					Image image2 = Image.FromFile(DrvPicPath2);
					sampleanalysispicture.Image = image2;
					sampleanalysispicture.Tag = fileSavePath;
				}
				else
				{
					fileSavePath = "";
					sampleanalysispicture.Image = null;
					sampleanalysispicture.Invalidate();
				}
			}
			else
			{
				fileSavePath = "";
				sampleanalysispicture.Image = null;
				sampleanalysispicture.Invalidate();
			}
			if (fileSavePathcookingpic != null && fileSavePathcookingpic != "")
			{
				string DrvPicPath = clsGlobalVariables.AttachmentFolderPath + "\\" + fileSavePathcookingpic;
				if (File.Exists(DrvPicPath))
				{
					Image image = Image.FromFile(DrvPicPath);
					cookingpic.Image = image;
					cookingpic.Tag = fileSavePathcookingpic;
				}
				else
				{
					fileSavePathcookingpic = "";
					cookingpic.Image = null;
					cookingpic.Invalidate();
				}
			}
			else
			{
				fileSavePathcookingpic = "";
				cookingpic.Image = null;
				cookingpic.Invalidate();
			}
			DataTable GetByIdStepAnalysisdt = LabInProcessAnalysisRawHeader.BindPlantStepAnalysisHistory(new LabInProcessAnalysisRawHeader
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Id = RecId
			});
			GetByPlantdt.Rows.Clear();
			if (GetByIdStepAnalysisdt.Rows.Count > 0)
			{
				for (int j = 0; j < GetByIdStepAnalysisdt.Rows.Count; j++)
				{
					GetByPlantdt.Rows.Add(GetByIdStepAnalysisdt.Rows[j]["Id"], GetByIdStepAnalysisdt.Rows[j]["PlantId"], GetByIdStepAnalysisdt.Rows[j]["PlantName"], GetByIdStepAnalysisdt.Rows[j]["SortNo"], GetByIdStepAnalysisdt.Rows[j]["StepId"], GetByIdStepAnalysisdt.Rows[j]["ProcessStepName"], GetByIdStepAnalysisdt.Rows[j]["ParameterId"], GetByIdStepAnalysisdt.Rows[j]["AnalysisParameterCode"], Conversion.ToDouble(GetByIdStepAnalysisdt.Rows[j]["ResultValue"]), 0, 0, 0, 0, 0, 0, GetByIdStepAnalysisdt.Rows[j]["RemarkDetail"]);
				}
			}
			if (GetByPlantdt.Rows.Count > 0)
			{
				grdStepAnalysis.DataSource = GetByPlantdt;
				grdStepAnalysis.RetrieveStructure();
				grdStepAnalysisSetting();
			}
			else
			{
				grdStepAnalysis.ClearStructure();
			}
			GetByAnalysisGroupdt.Rows.Clear();
			DataTable GroupAnalysisdt = LabInProcessAnalysisRawHeader.BindAnalysisGroupHistory(RecId);
			if (GroupAnalysisdt.Rows.Count > 0)
			{
				for (int i = 0; i < GroupAnalysisdt.Rows.Count; i++)
				{
					GetByAnalysisGroupdt.Rows.Add(GroupAnalysisdt.Rows[i]["Id"], GroupAnalysisdt.Rows[i]["InvLabGroupAnalysisStandardsId"], GroupAnalysisdt.Rows[i]["AnalysisGroupDescription"], GroupAnalysisdt.Rows[i]["AnalysisParameterID"], GroupAnalysisdt.Rows[i]["AnalysisParameterDescription"], GroupAnalysisdt.Rows[i]["MinValue"], GroupAnalysisdt.Rows[i]["MaxValue"], Conversion.ToDouble(GroupAnalysisdt.Rows[i]["InAnalysisResult"]), GroupAnalysisdt.Rows[i]["RemarksDetail"]);
				}
			}
			if (GetByAnalysisGroupdt.Rows.Count > 0)
			{
				grdAnalysisGroup.DataSource = GetByAnalysisGroupdt;
				grdAnalysisGroup.RetrieveStructure();
				grdAnalysisGroupSetting();
			}
			else
			{
				grdAnalysisGroup.ClearStructure();
			}
			FormHelper.LoadAttachmentsForObject(AT, RecId, base.Name);
			JobOrderNoFill();
			TopFiveStepAnalysisResultsAgainstPlantJobOrderAndItem();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void refresh()
	{
		try
		{
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			RecId = 0;
			GenerateCode();
			UpdateMode = false;
			grdtable.Clear();
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			((Control)(object)cmbitem).Text = "";
			((Control)(object)cmbproductionNo).Text = "";
			txtAnalystPerson.Text = "";
			((Control)(object)cmbitem).Text = "";
			((Control)(object)cmbanalysisGroup).Text = "";
			((Control)(object)cmbCropYear).Text = "";
			txtremarks.Text = "";
			GetByPlantdt.Rows.Clear();
			GetByAnalysisGroupdt.Rows.Clear();
			sampleanalysispicture.Image = null;
			sampleanalysispicture.Tag = "";
			sampleanalysispicture.Invalidate();
			cookingpic.Image = null;
			cookingpic.Tag = "";
			cookingpic.Invalidate();
			((Control)(object)cmbCropYear).Text = string.Empty;
			((Control)(object)cmbanalysisGroup).Enabled = true;
			JobOrderNoFill();
			AnalysisGroup();
			CropYearFill();
			PlantFill();
			grdAnalysisGroup.ClearStructure();
			grdStepAnalysis.ClearStructure();
			GetConfigurationsFromGlobalAndBindValuesInColumns();
			JobOrderNoFill();
			cmbPlantName.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Refresh1()
	{
		try
		{
			JobOrderNoFill();
			AnalysisGroup();
			CropYearFill();
			PlantFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnRefresh_Click(object sender, EventArgs e)
	{
		Refresh1();
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		refresh();
	}

	private void InvLabAnalysisInProcess_FormClosing(object sender, FormClosingEventArgs e)
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

	private void btnAnalysisPictureClear_Click(object sender, EventArgs e)
	{
		try
		{
			fileSavePath = string.Empty;
			ofd = string.Empty;
			if (sampleanalysispicture.Image != null)
			{
				sampleanalysispicture.Image.Dispose();
				sampleanalysispicture.Image = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCookingPicClear_Click(object sender, EventArgs e)
	{
		try
		{
			fileSavePathcookingpic = string.Empty;
			ofdcookingpic = string.Empty;
			if (cookingpic.Image != null)
			{
				cookingpic.Image.Dispose();
				cookingpic.Image = null;
			}
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
			FromDateHistory.Text = string.Empty;
			ToDateHistory.Text = string.Empty;
			FromDocNoHistory.Text = string.Empty;
			ToDocNoHistory.Text = string.Empty;
			((Control)(object)CmbJobOrderHistory).Text = string.Empty;
			((Control)(object)CmbPlantHistory).Text = string.Empty;
			((Control)(object)CmbItemHistory).Text = string.Empty;
			((Control)(object)CmbAnalystHistory).Text = string.Empty;
			grdhistory.ClearStructure();
			grdStepAnalysisHistory.ClearStructure();
			grdGroupAnalysisHistory.ClearStructure();
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
			HistoryComboFill();
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
			historygridfill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdhistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				HistoryStepAnalysisGridFill();
				HistoryGroupAnalysisGridFill();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdhistory.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				if (e.Column.Key == "NoOfAttachments")
				{
					int id = Conversion.ToInt(item.Cells["Id"].Value);
					int DocumentTypeId = Conversion.ToInt(item.Cells["DocumentTypeId"].Value);
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(id, DocumentTypeId);
				}
				if (e.Column.Key == "AnalysisPic")
				{
					string AnalysisPic = Conversion.ToString(item.Cells["AnalysisPicValue"].Value);
					FormHelper.ShowLabImagesPreview(Path.Combine(clsGlobalVariables.AttachmentFolderPath, AnalysisPic));
				}
				if (e.Column.Key == "CookingPic")
				{
					string CookingPic = Conversion.ToString(item.Cells["CookingPicValue"].Value);
					FormHelper.ShowLabImagesPreview(Path.Combine(clsGlobalVariables.AttachmentFolderPath, CookingPic));
				}
			}
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
	}

	private void historygridfill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters cat = new ReportsParameters();
			cat.OrganizationId = UserAccount.OrganizationId;
			cat.CompanyId = UserAccount.CompanyId;
			cat.DocumentTypeId = 306;
			cat.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			cat.CanViewAllRecord = formrights.DoHaveCanViewAllRecordRights;
			if (!cat.CanViewAllRecord)
			{
				cat.EntryUser = UserAccount.ID;
			}
			if (FromDateHistory.Checked)
			{
				cat.FromDate = FromDateHistory.Value;
			}
			if (ToDateHistory.Checked)
			{
				cat.ToDate = ToDateHistory.Value;
			}
			cat.FromDocNo = Conversion.ToInt((object)FromDocNoHistory.Text);
			cat.ToDocNo = Conversion.ToInt((object)ToDocNoHistory.Text);
			cat.JobOrderId = Conversion.ToInt(CmbJobOrderHistory.Value);
			cat.PlantId = Conversion.ToInt(CmbPlantHistory.Value);
			cat.ItemId = Conversion.ToInt(CmbItemHistory.Value);
			cat.AnalystPerson = ((Control)(object)CmbAnalystHistory).Text;
			DataTable dtHis = LabInProcessAnalysisRawHeader.GetAll(cat);
			if (dtHis.Rows.Count > 0)
			{
				DataTable table = new DataTable();
				table.Columns.Add("RecordNo", typeof(int));
				table.Columns.Add("Id", typeof(int));
				table.Columns.Add("DocNo", typeof(int));
				table.Columns.Add("DocDate", typeof(DateTime));
				table.Columns.Add("DocumentTypeId", typeof(int));
				table.Columns.Add("PlantName", typeof(string));
				table.Columns.Add("JobOrder", typeof(string));
				table.Columns.Add("ItemName", typeof(string));
				table.Columns.Add("CropYear", typeof(string));
				table.Columns.Add("AnalysisGroup", typeof(string));
				table.Columns.Add("AnalystPerson", typeof(string));
				table.Columns.Add("AnalysisTime", typeof(DateTime));
				table.Columns.Add("EntryDate", typeof(DateTime));
				table.Columns.Add("EntryUser", typeof(string));
				table.Columns.Add("ModifyDate", typeof(DateTime));
				table.Columns.Add("ModifyUser", typeof(string));
				table.Columns.Add("AnalysisPic", typeof(int));
				table.Columns.Add("CookingPic", typeof(int));
				table.Columns.Add("AnalysisPicValue");
				table.Columns.Add("CookingPicValue");
				table.Columns.Add("NoOfAttachments", typeof(int));
				table.Columns.Add("RemarksHeader", typeof(string));
				table.Clear();
				foreach (DataRow row in dtHis.Rows)
				{
					table.Rows.Add(row["RecordNo"], row["Id"], row["DocNo"], Convert.ToDateTime(row["DocDate"]), row["DocumentTypeId"], row["PlantName"], row["JobOrder"], row["ItemName"], row["CropYear"], row["AnalysisGroupDescription"], row["AnalystPerson"], row["AnalysisTime"], row["EntryDate"], row["UserName"], row["ModifyDate"], row["ModifyUserName"], (!string.IsNullOrEmpty(Conversion.ToString(row["AnalysisPic"]))) ? 1 : 0, (!string.IsNullOrEmpty(Conversion.ToString(row["CookingPic"]))) ? 1 : 0, row["AnalysisPic"], row["CookingPic"], row["NoOfAttachments"], row["RemarksHeader"]);
				}
				grdhistory.DataSource = table;
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
			grdhistory.RootTable.Columns["DocumentTypeId"].Visible = false;
			grdhistory.RootTable.Columns["RecordNo"].Visible = false;
			grdhistory.RootTable.Columns["AnalysisPicValue"].Visible = false;
			grdhistory.RootTable.Columns["CookingPicValue"].Visible = false;
			GridEX_Helper.GridWrappingAndColumnSettings(grdhistory, 2, 3, null, null, null, ColumnSetting: false);
			grdhistory.GroupByBoxVisible = true;
			grdhistory.RootTable.Columns["DocNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grdhistory.RootTable.Columns["DocDate"].Width = Constants.InventoryConstants.DateConstant;
			grdhistory.RootTable.Columns["PlantName"].Width = Constants.InventoryConstants.PlantName;
			grdhistory.RootTable.Columns["JobOrder"].Width = 120;
			grdhistory.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName + 20;
			grdhistory.RootTable.Columns["CropYear"].Width = Constants.InventoryConstants.CropYearConstant;
			grdhistory.RootTable.Columns["AnalysisGroup"].Width = 85;
			grdhistory.RootTable.Columns["AnalystPerson"].Width = 100;
			grdhistory.RootTable.Columns["AnalysisTime"].Width = 110;
			grdhistory.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			grdhistory.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants + 20;
			grdhistory.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			grdhistory.RootTable.Columns["ModifyUser"].Width = Constants.InventoryConstants.ModifyUserConstants + 20;
			grdhistory.RootTable.Columns["NoOfAttachments"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants - 10;
			grdhistory.RootTable.Columns["RemarksHeader"].Width = Constants.InventoryConstants.RemarksConstants + 50;
			grdhistory.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			grdhistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			grdhistory.RootTable.Columns["AnalysisTime"].FormatString = "hh:mmm:ss tt";
			grdhistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			GridEX_Helper.AddButton(grdhistory, "AddAttachment", "Add Attachment", 105);
			grdhistory.RootTable.Columns["AnalysisPic"].ColumnType = (ColumnType)5;
			grdhistory.RootTable.Columns["CookingPic"].ColumnType = (ColumnType)5;
			grdhistory.RootTable.Columns["AnalysisPic"].Width = 60;
			grdhistory.RootTable.Columns["CookingPic"].Width = 60;
			((Control)(object)grdhistory).Refresh();
			GridEX_Helper.AddButton(grdhistory, "Print", "Print", 40, 0);
			GridEX_Helper.AddButton(grdhistory, "Edit", "Edit", 40, 1);
			grdhistory.FrozenColumns = 2;
			crtgrdHistory_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			RecId = Conversion.ToInt(grdhistory.CurrentRow.Cells["Id"].Value);
			ReadById(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdhistory.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				int Id = Conversion.ToInt(grdhistory.CurrentRow.Cells["Id"].Value);
				if (e.Column.Key == "Edit")
				{
					RecId = Id;
					ReadById(RecId);
				}
				else if (e.Column.Key == "Print")
				{
					CommonServices.LabAnalysisReport659(Id);
				}
				else if (e.Column.Key == "AddAttachment")
				{
					AttachmentAddingFromHistory attachmentAddingFromHistory = new AttachmentAddingFromHistory(UserAccount, Id, 306, base.Name);
					attachmentAddingFromHistory.BringToFront();
					attachmentAddingFromHistory.ShowDialog();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void HistoryStepAnalysisGridFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Expected O, but got Unknown
		try
		{
			DataTable GetByAnalysisGroupHistorydt = LabInProcessAnalysisRawHeader.BindPlantStepAnalysisHistory(new LabInProcessAnalysisRawHeader
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Id = Conversion.ToInt(grdhistory.CurrentRow.Cells["Id"].Value)
			});
			if (GetByAnalysisGroupHistorydt.Rows.Count > 0)
			{
				grdStepAnalysisHistory.DataSource = GetByAnalysisGroupHistorydt;
				grdStepAnalysisHistory.RetrieveStructure();
				GridEX_Helper.GridWrappingAndColumnSettings(grdStepAnalysisHistory, 2, 3, null, null, null, ColumnSetting: false);
				grdStepAnalysisHistory.RootTable.Columns["Id"].Visible = false;
				grdStepAnalysisHistory.RootTable.Columns["PlantId"].Visible = false;
				grdStepAnalysisHistory.RootTable.Columns["StepId"].Visible = false;
				grdStepAnalysisHistory.RootTable.Columns["ParameterId"].Visible = false;
				grdStepAnalysisHistory.RootTable.Columns["SortNo"].Visible = false;
				grdStepAnalysisHistory.RootTable.Columns["ProcessStepName"].Caption = "Process Step";
				grdStepAnalysisHistory.RootTable.Columns["ResultValue"].FormatString = "#,##0.####";
			}
			else
			{
				grdStepAnalysisHistory.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryGroupAnalysisGridFill()
	{
		try
		{
			DataTable dtBindAnalysisGroupHistory = LabInProcessAnalysisRawHeader.BindAnalysisGroupHistory(Conversion.ToInt(grdhistory.CurrentRow.Cells["Id"].Value));
			grdGroupAnalysisHistory.ClearStructure();
			if (dtBindAnalysisGroupHistory.Rows.Count > 0)
			{
				grdGroupAnalysisHistory.DataSource = dtBindAnalysisGroupHistory;
				grdGroupAnalysisHistory.RetrieveStructure();
				GridEX_Helper.GridWrappingAndColumnSettings(grdGroupAnalysisHistory, 2, 3, null, null, null, ColumnSetting: false);
				grdGroupAnalysisHistory.RootTable.Columns["Id"].Visible = false;
				grdGroupAnalysisHistory.RootTable.Columns["InvLabGroupAnalysisStandardsId"].Visible = false;
				grdGroupAnalysisHistory.RootTable.Columns["AnalysisParameterID"].Visible = false;
				grdGroupAnalysisHistory.RootTable.Columns["AnalysisParameterDescription"].Width = 150;
			}
			else
			{
				grdGroupAnalysisHistory.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InvLabPurchaseAnalysis_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.KeyData == Keys.Return)
		{
			SendKeys.Send("{TAB}");
		}
		if (btnSave.Visible && btnSave.Enabled && e.Control && e.KeyCode == Keys.S && tabControl1.SelectedIndex == 0)
		{
			btnSave_Click(null, null);
		}
		if (e.Control && e.KeyCode == Keys.N && tabControl1.SelectedIndex == 0)
		{
			btnNew_Click(null, null);
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
		if (btnUpdate.Visible && btnUpdate.Enabled && tabControl1.SelectedIndex == 0 && e.Control && e.KeyCode == Keys.U)
		{
			btnUpdate_Click(null, null);
		}
		if (e.Control && e.KeyCode == Keys.P)
		{
			btnprint_Click(null, null);
		}
	}

	public void definition()
	{
		try
		{
			List<ConfigrationsAllocation> list = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Attachment Folder Path").ToList();
			if (list.Count > 0)
			{
				string value = list[0].ConfigKey.ToString();
				if (value != null)
				{
					saveDirectory = value;
					Confiquration = true;
				}
			}
			else
			{
				MessageBox.Show("Please Map the Path in Configuration");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnimg1_Click(object sender, EventArgs e)
	{
		try
		{
			fileSavePath = string.Empty;
			ofd = string.Empty;
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "(*.jpg; *.jpeg; *.gif; *.PNG; *.png)| *.jpg; *.jpeg; *.gif; *.PNG;*.png; ";
			openFileDialog.Title = "Select an image";
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				fileSavePath = $"{Guid.NewGuid():N}{Path.GetExtension(openFileDialog.FileName)}";
				ofd = openFileDialog.FileName;
				Path.GetFileName(openFileDialog.FileName);
				if (sampleanalysispicture.Image != null)
				{
					sampleanalysispicture.Image.Dispose();
					sampleanalysispicture.Image = null;
				}
				using (Bitmap bmp = new Bitmap(openFileDialog.FileName))
				{
					sampleanalysispicture.Image = new Bitmap(bmp);
				}
				sampleanalysispicture.SizeMode = PictureBoxSizeMode.Zoom;
			}
			else if (sampleanalysispicture.Image != null)
			{
				sampleanalysispicture.Image.Dispose();
				sampleanalysispicture.Image = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnimage2_Click(object sender, EventArgs e)
	{
		try
		{
			fileSavePathcookingpic = string.Empty;
			ofdcookingpic = string.Empty;
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "(*.jpg; *.jpeg; *.gif; *.PNG; *.png)| *.jpg; *.jpeg; *.gif; *.PNG;*.png; ";
			openFileDialog.Title = "Select an image";
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				fileSavePathcookingpic = $"{Guid.NewGuid():N}{Path.GetExtension(openFileDialog.FileName)}";
				ofdcookingpic = openFileDialog.FileName;
				Path.GetFileName(openFileDialog.FileName);
				if (cookingpic.Image != null)
				{
					cookingpic.Image.Dispose();
					cookingpic.Image = null;
				}
				using (Bitmap bmp = new Bitmap(openFileDialog.FileName))
				{
					cookingpic.Image = new Bitmap(bmp);
				}
				cookingpic.SizeMode = PictureBoxSizeMode.Zoom;
			}
			else if (cookingpic.Image != null)
			{
				cookingpic.Image.Dispose();
				cookingpic.Image = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnprv_Click(object sender, EventArgs e)
	{
		LabImagesPreview labImagesPreview = new LabImagesPreview();
		imgpreview = sampleanalysispicture.Image;
		labImagesPreview.Show();
	}

	private void btnimgprevcookingpic_Click(object sender, EventArgs e)
	{
		LabImagesPreview labImagesPreview = new LabImagesPreview();
		imgpreview = cookingpic.Image;
		labImagesPreview.Show();
	}

	private void btnattachment_Click(object sender, EventArgs e)
	{
		AT.Show();
	}

	private void btnprint_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.LabAnalysisReport659(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void crtgrdHistory_Load(object sender, EventArgs e)
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

	private void crtgrdStepAnalysis_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdStepAnalysisHistory).Name);
			if (fs != null)
			{
				grdStepAnalysisHistory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void crtgrdgroupAnalysis_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdGroupAnalysisHistory).Name);
			if (fs != null)
			{
				grdGroupAnalysisHistory.LoadLayoutFile((Stream)fs);
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdStepAnalysis).Name);
			if (fs != null)
			{
				grdStepAnalysis.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ctrlGrdBar3_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdAnalysisGroup).Name);
			if (fs != null)
			{
				grdAnalysisGroup.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
		//IL_0361: Unknown result type (might be due to invalid IL or missing references)
		//IL_036b: Expected O, but got Unknown
		//IL_038d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0397: Expected O, but got Unknown
		//IL_03a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ad: Expected O, but got Unknown
		//IL_03f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fa: Expected O, but got Unknown
		//IL_0427: Unknown result type (might be due to invalid IL or missing references)
		//IL_0431: Expected O, but got Unknown
		//IL_043d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0447: Expected O, but got Unknown
		//IL_045e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0468: Expected O, but got Unknown
		//IL_0469: Unknown result type (might be due to invalid IL or missing references)
		//IL_0473: Expected O, but got Unknown
		//IL_053a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0544: Expected O, but got Unknown
		//IL_05df: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e9: Expected O, but got Unknown
		//IL_05f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ff: Expected O, but got Unknown
		//IL_0600: Unknown result type (might be due to invalid IL or missing references)
		//IL_060a: Expected O, but got Unknown
		//IL_060b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0615: Expected O, but got Unknown
		//IL_0616: Unknown result type (might be due to invalid IL or missing references)
		//IL_0620: Expected O, but got Unknown
		//IL_0d7f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d89: Expected O, but got Unknown
		//IL_15bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_15c6: Expected O, but got Unknown
		//IL_15ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_15f4: Expected O, but got Unknown
		GridEXLayout grdAnalysisGroup_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdGroupAnalysisHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdStepAnalysisHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdhistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
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
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance66 = new Appearance();
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
		GridEXLayout grdStepAnalysis_DesignTimeLayout = new GridEXLayout();
		Appearance appearance67 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		this.btn = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.BtnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.btnprint = new System.Windows.Forms.ToolStripButton();
		this.btnattachment = new System.Windows.Forms.ToolStripButton();
		this.grdAnalysisGroup = new GridEX();
		this.tabHistory = new System.Windows.Forms.TabPage();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel10 = new System.Windows.Forms.Panel();
		this.grdGroupAnalysisHistory = new GridEX();
		this.panel9 = new System.Windows.Forms.Panel();
		this.grdStepAnalysisHistory = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.panel12 = new System.Windows.Forms.Panel();
		this.label11 = new System.Windows.Forms.Label();
		this.panel11 = new System.Windows.Forms.Panel();
		this.label9 = new System.Windows.Forms.Label();
		this.panel6 = new System.Windows.Forms.Panel();
		this.grdhistory = new GridEX();
		this.panel20 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.label19 = new System.Windows.Forms.Label();
		this.btnShowHistory = new System.Windows.Forms.Button();
		this.CmbPlantHistory = new UltraCombo();
		this.label18 = new System.Windows.Forms.Label();
		this.CmbJobOrderHistory = new UltraCombo();
		this.label24 = new System.Windows.Forms.Label();
		this.label23 = new System.Windows.Forms.Label();
		this.CmbAnalystHistory = new UltraCombo();
		this.CmbItemHistory = new UltraCombo();
		this.label56 = new System.Windows.Forms.Label();
		this.label57 = new System.Windows.Forms.Label();
		this.ToDocNoHistory = new System.Windows.Forms.TextBox();
		this.FromDocNoHistory = new System.Windows.Forms.TextBox();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.panel16 = new System.Windows.Forms.Panel();
		this.crtgrdHistory = new grid.User_Conrtols.CtrlGrdBar();
		this.label6 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabForm = new System.Windows.Forms.TabPage();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel3 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.grdStepAnalysis = new GridEX();
		this.panel13 = new System.Windows.Forms.Panel();
		this.label14 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel14 = new System.Windows.Forms.Panel();
		this.label16 = new System.Windows.Forms.Label();
		this.panel7 = new System.Windows.Forms.Panel();
		this.btnCookingPicClear = new System.Windows.Forms.Button();
		this.chkPreview = new System.Windows.Forms.CheckBox();
		this.btnAnalysisPictureClear = new System.Windows.Forms.Button();
		this.panel15 = new System.Windows.Forms.Panel();
		this.label17 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.cmbPlantName = new UltraCombo();
		this.label4 = new System.Windows.Forms.Label();
		this.cmbitem = new UltraCombo();
		this.cmbCropYear = new UltraCombo();
		this.cmbanalysisGroup = new UltraCombo();
		this.cmbproductionNo = new UltraCombo();
		this.txtAnalystPerson = new System.Windows.Forms.TextBox();
		this.label15 = new System.Windows.Forms.Label();
		this.btnimgprevcookingpic = new System.Windows.Forms.Button();
		this.btnprv = new System.Windows.Forms.Button();
		this.btnimage2 = new System.Windows.Forms.Button();
		this.btnimg1 = new System.Windows.Forms.Button();
		this.cookingpic = new System.Windows.Forms.PictureBox();
		this.sampleanalysispicture = new System.Windows.Forms.PictureBox();
		this.txtremarks = new System.Windows.Forms.TextBox();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.label13 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.txtsampledate = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.txtanalysistime = new System.Windows.Forms.DateTimePicker();
		this.txtdocdate = new System.Windows.Forms.DateTimePicker();
		this.label1 = new System.Windows.Forms.Label();
		this.btn.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdAnalysisGroup).BeginInit();
		this.tabHistory.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel10.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdGroupAnalysisHistory).BeginInit();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdStepAnalysisHistory).BeginInit();
		this.panel8.SuspendLayout();
		this.panel12.SuspendLayout();
		this.panel11.SuspendLayout();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdhistory).BeginInit();
		this.panel20.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPlantHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobOrderHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAnalystHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemHistory).BeginInit();
		this.panel16.SuspendLayout();
		this.panel5.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabForm.SuspendLayout();
		this.panel1.SuspendLayout();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdStepAnalysis).BeginInit();
		this.panel13.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel14.SuspendLayout();
		this.panel7.SuspendLayout();
		this.panel15.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbPlantName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbitem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCropYear).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbanalysisGroup).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbproductionNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cookingpic).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.sampleanalysispicture).BeginInit();
		base.SuspendLayout();
		this.btn.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.btn.Items.AddRange(new System.Windows.Forms.ToolStripItem[6] { this.btnNew, this.BtnRefresh, this.btnUpdate, this.btnSave, this.btnprint, this.btnattachment });
		this.btn.Location = new System.Drawing.Point(3, 3);
		this.btn.Name = "btn";
		this.btn.Size = new System.Drawing.Size(1170, 27);
		this.btn.TabIndex = 34;
		this.btn.Text = "toolStrip1";
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(59, 24);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.BtnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.BtnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefresh.Name = "BtnRefresh";
		this.BtnRefresh.Size = new System.Drawing.Size(80, 24);
		this.BtnRefresh.Text = "Refresh";
		this.BtnRefresh.Click += new System.EventHandler(BtnRefresh_Click);
		this.btnUpdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnSave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(64, 24);
		this.btnSave.Text = "&Save";
		this.btnSave.Click += new System.EventHandler(btnSave_Click);
		this.btnprint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnprint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnprint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnprint.Name = "btnprint";
		this.btnprint.Size = new System.Drawing.Size(92, 24);
		this.btnprint.Text = "&Print-659";
		this.btnprint.Click += new System.EventHandler(btnprint_Click);
		this.btnattachment.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnattachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnattachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnattachment.Name = "btnattachment";
		this.btnattachment.Size = new System.Drawing.Size(109, 24);
		this.btnattachment.Text = "&Attachment";
		this.btnattachment.Click += new System.EventHandler(btnattachment_Click);
		this.grdAnalysisGroup.AllowDelete = (InheritableBoolean)1;
		((ScrollableBase)this.grdAnalysisGroup).BackColor = System.Drawing.Color.White;
		grdAnalysisGroup_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdAnalysisGroup.DesignTimeLayout = grdAnalysisGroup_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdAnalysisGroup).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdAnalysisGroup).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdAnalysisGroup.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdAnalysisGroup).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdAnalysisGroup).Name = "grdAnalysisGroup";
		((ScrollableBase)this.grdAnalysisGroup).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdAnalysisGroup).Size = new System.Drawing.Size(460, 325);
		((System.Windows.Forms.Control)(object)this.grdAnalysisGroup).TabIndex = 0;
		this.grdAnalysisGroup.TotalRow = (InheritableBoolean)1;
		this.grdAnalysisGroup.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdAnalysisGroup).VisualStyle = (VisualStyle)4;
		this.grdAnalysisGroup.CellUpdated += new ColumnActionEventHandler(grd_CellUpdated);
		this.tabHistory.Controls.Add(this.panel4);
		this.tabHistory.Location = new System.Drawing.Point(4, 4);
		this.tabHistory.Name = "tabHistory";
		this.tabHistory.Padding = new System.Windows.Forms.Padding(3);
		this.tabHistory.Size = new System.Drawing.Size(1176, 582);
		this.tabHistory.TabIndex = 1;
		this.tabHistory.Text = "History";
		this.tabHistory.UseVisualStyleBackColor = true;
		this.panel4.Controls.Add(this.panel10);
		this.panel4.Controls.Add(this.panel9);
		this.panel4.Controls.Add(this.panel8);
		this.panel4.Controls.Add(this.panel6);
		this.panel4.Controls.Add(this.panel5);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(3, 3);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1170, 576);
		this.panel4.TabIndex = 1;
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.grdGroupAnalysisHistory);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel10.Location = new System.Drawing.Point(576, 482);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(594, 94);
		this.panel10.TabIndex = 7;
		this.grdGroupAnalysisHistory.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdGroupAnalysisHistory).BackColor = System.Drawing.Color.White;
		grdGroupAnalysisHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdGroupAnalysisHistory.DesignTimeLayout = grdGroupAnalysisHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdGroupAnalysisHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdGroupAnalysisHistory.FilterMode = (FilterMode)2;
		this.grdGroupAnalysisHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdGroupAnalysisHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdGroupAnalysisHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdGroupAnalysisHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdGroupAnalysisHistory).Name = "grdGroupAnalysisHistory";
		((ScrollableBase)this.grdGroupAnalysisHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdGroupAnalysisHistory).Size = new System.Drawing.Size(594, 94);
		((System.Windows.Forms.Control)(object)this.grdGroupAnalysisHistory).TabIndex = 3;
		this.grdGroupAnalysisHistory.TotalRow = (InheritableBoolean)1;
		this.grdGroupAnalysisHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdGroupAnalysisHistory).VisualStyle = (VisualStyle)4;
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.grdStepAnalysisHistory);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel9.Location = new System.Drawing.Point(0, 482);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(576, 94);
		this.panel9.TabIndex = 6;
		this.grdStepAnalysisHistory.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdStepAnalysisHistory).BackColor = System.Drawing.Color.White;
		grdStepAnalysisHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdStepAnalysisHistory.DesignTimeLayout = grdStepAnalysisHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdStepAnalysisHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdStepAnalysisHistory.FilterMode = (FilterMode)2;
		this.grdStepAnalysisHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdStepAnalysisHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdStepAnalysisHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdStepAnalysisHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdStepAnalysisHistory).Name = "grdStepAnalysisHistory";
		((ScrollableBase)this.grdStepAnalysisHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdStepAnalysisHistory).Size = new System.Drawing.Size(576, 94);
		((System.Windows.Forms.Control)(object)this.grdStepAnalysisHistory).TabIndex = 3;
		this.grdStepAnalysisHistory.TotalRow = (InheritableBoolean)1;
		this.grdStepAnalysisHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdStepAnalysisHistory).VisualStyle = (VisualStyle)4;
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.panel12);
		this.panel8.Controls.Add(this.panel11);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 461);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1170, 21);
		this.panel8.TabIndex = 5;
		this.panel12.Controls.Add(this.label11);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel12.Location = new System.Drawing.Point(576, 0);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(594, 21);
		this.panel12.TabIndex = 1;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label11.ForeColor = System.Drawing.Color.White;
		this.label11.Location = new System.Drawing.Point(6, 2);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(110, 17);
		this.label11.TabIndex = 42;
		this.label11.Text = "Group Analysis";
		this.panel11.Controls.Add(this.label9);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel11.Location = new System.Drawing.Point(0, 0);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(576, 21);
		this.panel11.TabIndex = 0;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label9.ForeColor = System.Drawing.Color.White;
		this.label9.Location = new System.Drawing.Point(5, 2);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(138, 17);
		this.label9.TabIndex = 42;
		this.label9.Text = "Plant Step Analysis";
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.grdhistory);
		this.panel6.Controls.Add(this.panel20);
		this.panel6.Controls.Add(this.panel16);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 30);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1170, 431);
		this.panel6.TabIndex = 4;
		this.grdhistory.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdhistory).BackColor = System.Drawing.Color.White;
		grdhistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdhistory.DesignTimeLayout = grdhistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdhistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdhistory.FilterMode = (FilterMode)2;
		this.grdhistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdhistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdhistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdhistory).Location = new System.Drawing.Point(0, 138);
		((System.Windows.Forms.Control)(object)this.grdhistory).Name = "grdhistory";
		((ScrollableBase)this.grdhistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdhistory).Size = new System.Drawing.Size(1170, 293);
		((System.Windows.Forms.Control)(object)this.grdhistory).TabIndex = 2;
		this.grdhistory.TotalRow = (InheritableBoolean)1;
		this.grdhistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdhistory).VisualStyle = (VisualStyle)4;
		this.grdhistory.ColumnButtonClick += new ColumnActionEventHandler(grdhistory_ColumnButtonClick);
		this.grdhistory.SelectionChanged += new System.EventHandler(grdhistory_SelectionChanged);
		this.grdhistory.LinkClicked += new ColumnActionEventHandler(grdhistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdhistory).DoubleClick += new System.EventHandler(grdhistory_DoubleClick);
		this.panel20.Controls.Add(this.groupBox2);
		this.panel20.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel20.Location = new System.Drawing.Point(0, 33);
		this.panel20.Name = "panel20";
		this.panel20.Size = new System.Drawing.Size(1170, 105);
		this.panel20.TabIndex = 31;
		this.groupBox2.Controls.Add(this.label19);
		this.groupBox2.Controls.Add(this.btnShowHistory);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPlantHistory);
		this.groupBox2.Controls.Add(this.label18);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobOrderHistory);
		this.groupBox2.Controls.Add(this.label24);
		this.groupBox2.Controls.Add(this.label23);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbAnalystHistory);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemHistory);
		this.groupBox2.Controls.Add(this.label56);
		this.groupBox2.Controls.Add(this.label57);
		this.groupBox2.Controls.Add(this.ToDocNoHistory);
		this.groupBox2.Controls.Add(this.FromDocNoHistory);
		this.groupBox2.Controls.Add(this.ToDateHistory);
		this.groupBox2.Controls.Add(this.FromDateHistory);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(5, 2);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(765, 99);
		this.groupBox2.TabIndex = 4;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Filters";
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(389, 19);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(69, 15);
		this.label19.TabIndex = 96;
		this.label19.Text = "Plant Name";
		this.btnShowHistory.BackColor = System.Drawing.Color.Teal;
		this.btnShowHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShowHistory.ForeColor = System.Drawing.Color.White;
		this.btnShowHistory.Location = new System.Drawing.Point(693, 67);
		this.btnShowHistory.Margin = new System.Windows.Forms.Padding(2);
		this.btnShowHistory.Name = "btnShowHistory";
		this.btnShowHistory.Size = new System.Drawing.Size(62, 26);
		this.btnShowHistory.TabIndex = 8;
		this.btnShowHistory.Text = "Show";
		this.btnShowHistory.UseVisualStyleBackColor = false;
		this.btnShowHistory.Click += new System.EventHandler(btnShowHistory_Click);
		this.CmbPlantHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbPlantHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPlantHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPlantHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPlantHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.CmbPlantHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPlantHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance28).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance28).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPlantHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance28;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPlantHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance29;
		((SpecialBoxBase)((UltraGridBase)this.CmbPlantHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance30).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance30).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPlantHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbPlantHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPlantHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPlantHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPlantHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbPlantHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPlantHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPlantHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance35).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPlantHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbPlantHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPlantHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance36).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance36).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance36).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPlantHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPlantHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbPlantHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPlantHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPlantHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbPlantHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPlantHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbPlantHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPlantHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPlantHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPlantHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPlantHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPlantHistory).Location = new System.Drawing.Point(472, 13);
		((System.Windows.Forms.Control)(object)this.CmbPlantHistory).Name = "CmbPlantHistory";
		((System.Windows.Forms.Control)(object)this.CmbPlantHistory).Size = new System.Drawing.Size(218, 26);
		((System.Windows.Forms.Control)(object)this.CmbPlantHistory).TabIndex = 5;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(6, 73);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(59, 15);
		this.label18.TabIndex = 94;
		this.label18.Text = "Job Order";
		this.CmbJobOrderHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbJobOrderHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobOrderHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobOrderHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobOrderHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.CmbJobOrderHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobOrderHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance41).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance41).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobOrderHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobOrderHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance42;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobOrderHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance44).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance44).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobOrderHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbJobOrderHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobOrderHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobOrderHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance46).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobOrderHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.CmbJobOrderHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobOrderHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobOrderHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance47;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance48).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobOrderHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.CmbJobOrderHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobOrderHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance49).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance49).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance49).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance49).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobOrderHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance50).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobOrderHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbJobOrderHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobOrderHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance51).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobOrderHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbJobOrderHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobOrderHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.CmbJobOrderHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobOrderHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobOrderHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobOrderHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbJobOrderHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobOrderHistory).Location = new System.Drawing.Point(116, 67);
		((System.Windows.Forms.Control)(object)this.CmbJobOrderHistory).Name = "CmbJobOrderHistory";
		((System.Windows.Forms.Control)(object)this.CmbJobOrderHistory).Size = new System.Drawing.Size(269, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobOrderHistory).TabIndex = 4;
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(389, 73);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(81, 15);
		this.label24.TabIndex = 92;
		this.label24.Text = "Analyst Name";
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.Location = new System.Drawing.Point(389, 46);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(67, 15);
		this.label23.TabIndex = 91;
		this.label23.Text = "Item Name";
		this.CmbAnalystHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbAnalystHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbAnalystHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbAnalystHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbAnalystHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.CmbAnalystHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbAnalystHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance55).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbAnalystHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance55;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAnalystHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance56;
		((SpecialBoxBase)((UltraGridBase)this.CmbAnalystHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance57).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance57).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAnalystHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.CmbAnalystHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbAnalystHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbAnalystHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance58;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance59).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbAnalystHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.CmbAnalystHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbAnalystHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAnalystHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance60;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance61).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbAnalystHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.CmbAnalystHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbAnalystHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance62).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance62).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance62).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance62).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAnalystHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance62;
		((AppearanceBase)appearance63).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbAnalystHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbAnalystHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbAnalystHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance64).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbAnalystHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.CmbAnalystHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbAnalystHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.CmbAnalystHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbAnalystHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbAnalystHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbAnalystHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbAnalystHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbAnalystHistory).Location = new System.Drawing.Point(472, 67);
		((System.Windows.Forms.Control)(object)this.CmbAnalystHistory).Name = "CmbAnalystHistory";
		((System.Windows.Forms.Control)(object)this.CmbAnalystHistory).Size = new System.Drawing.Size(218, 26);
		((System.Windows.Forms.Control)(object)this.CmbAnalystHistory).TabIndex = 7;
		this.CmbItemHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbItemHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance15).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance15).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance15;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance16;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance17).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance17).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance18;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance22).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance22).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance23).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance23).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance23).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance24).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemHistory).Location = new System.Drawing.Point(472, 40);
		((System.Windows.Forms.Control)(object)this.CmbItemHistory).Name = "CmbItemHistory";
		((System.Windows.Forms.Control)(object)this.CmbItemHistory).Size = new System.Drawing.Size(218, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemHistory).TabIndex = 6;
		this.label56.AutoSize = true;
		this.label56.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label56.Location = new System.Drawing.Point(6, 46);
		this.label56.Name = "label56";
		this.label56.Size = new System.Drawing.Size(107, 15);
		this.label56.TabIndex = 84;
		this.label56.Text = "From && To Doc No";
		this.label57.AutoSize = true;
		this.label57.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label57.Location = new System.Drawing.Point(6, 19);
		this.label57.Name = "label57";
		this.label57.Size = new System.Drawing.Size(91, 15);
		this.label57.TabIndex = 83;
		this.label57.Text = "From && To Date";
		this.ToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.ToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDocNoHistory.Location = new System.Drawing.Point(251, 42);
		this.ToDocNoHistory.Name = "ToDocNoHistory";
		this.ToDocNoHistory.ShortcutsEnabled = false;
		this.ToDocNoHistory.Size = new System.Drawing.Size(134, 23);
		this.ToDocNoHistory.TabIndex = 3;
		this.FromDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.FromDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDocNoHistory.Location = new System.Drawing.Point(116, 42);
		this.FromDocNoHistory.Name = "FromDocNoHistory";
		this.FromDocNoHistory.ShortcutsEnabled = false;
		this.FromDocNoHistory.Size = new System.Drawing.Size(134, 23);
		this.FromDocNoHistory.TabIndex = 2;
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(251, 15);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(134, 23);
		this.ToDateHistory.TabIndex = 1;
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(116, 15);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(134, 23);
		this.FromDateHistory.TabIndex = 0;
		this.panel16.BackColor = System.Drawing.Color.Teal;
		this.panel16.Controls.Add(this.crtgrdHistory);
		this.panel16.Controls.Add(this.label6);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel16.Location = new System.Drawing.Point(0, 0);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(1170, 33);
		this.panel16.TabIndex = 3;
		this.crtgrdHistory.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.crtgrdHistory.FormName = this;
		this.crtgrdHistory.Location = new System.Drawing.Point(1128, 5);
		this.crtgrdHistory.Margin = new System.Windows.Forms.Padding(4);
		this.crtgrdHistory.MyGrid = this.grdhistory;
		this.crtgrdHistory.Name = "crtgrdHistory";
		this.crtgrdHistory.Size = new System.Drawing.Size(38, 23);
		this.crtgrdHistory.TabIndex = 40;
		this.crtgrdHistory.Load += new System.EventHandler(crtgrdHistory_Load);
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.75f, System.Drawing.FontStyle.Bold);
		this.label6.ForeColor = System.Drawing.Color.White;
		this.label6.Location = new System.Drawing.Point(1, 3);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(74, 24);
		this.label6.TabIndex = 0;
		this.label6.Text = "History";
		this.panel5.BackColor = System.Drawing.Color.White;
		this.panel5.Controls.Add(this.toolStrip1);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1170, 30);
		this.panel5.TabIndex = 3;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnNewHistory, this.btnRefreshHistory });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1170, 27);
		this.toolStrip1.TabIndex = 29;
		this.toolStrip1.Text = "toolStrip1";
		this.btnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewHistory.Name = "btnNewHistory";
		this.btnNewHistory.Size = new System.Drawing.Size(59, 24);
		this.btnNewHistory.Text = "&New";
		this.btnNewHistory.Click += new System.EventHandler(btnNewHistory_Click);
		this.btnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshHistory.Name = "btnRefreshHistory";
		this.btnRefreshHistory.Size = new System.Drawing.Size(80, 24);
		this.btnRefreshHistory.Text = "&Refresh";
		this.btnRefreshHistory.Click += new System.EventHandler(btnRefreshHistory_Click);
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabForm);
		this.tabControl1.Controls.Add(this.tabHistory);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1184, 611);
		this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabForm.BackColor = System.Drawing.Color.White;
		this.tabForm.Controls.Add(this.panel1);
		this.tabForm.Controls.Add(this.panel7);
		this.tabForm.Controls.Add(this.btn);
		this.tabForm.Location = new System.Drawing.Point(4, 4);
		this.tabForm.Name = "tabForm";
		this.tabForm.Padding = new System.Windows.Forms.Padding(3);
		this.tabForm.Size = new System.Drawing.Size(1176, 582);
		this.tabForm.TabIndex = 0;
		this.tabForm.Text = "Form";
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(3, 229);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1170, 350);
		this.panel1.TabIndex = 38;
		this.panel3.Controls.Add(this.ctrlGrdBar2);
		this.panel3.Controls.Add(this.panel13);
		this.panel3.Controls.Add(this.label14);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(710, 350);
		this.panel3.TabIndex = 2;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(671, 1);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdStepAnalysis;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(38, 23);
		this.ctrlGrdBar2.TabIndex = 42;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.grdStepAnalysis.AllowDelete = (InheritableBoolean)1;
		((ScrollableBase)this.grdStepAnalysis).BackColor = System.Drawing.Color.White;
		grdStepAnalysis_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdStepAnalysis.DesignTimeLayout = grdStepAnalysis_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdStepAnalysis).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdStepAnalysis).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdStepAnalysis.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdStepAnalysis).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdStepAnalysis).Name = "grdStepAnalysis";
		((ScrollableBase)this.grdStepAnalysis).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdStepAnalysis).Size = new System.Drawing.Size(710, 325);
		((System.Windows.Forms.Control)(object)this.grdStepAnalysis).TabIndex = 0;
		this.grdStepAnalysis.TotalRow = (InheritableBoolean)1;
		this.grdStepAnalysis.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdStepAnalysis).VisualStyle = (VisualStyle)4;
		this.panel13.Controls.Add((System.Windows.Forms.Control)(object)this.grdStepAnalysis);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel13.Location = new System.Drawing.Point(0, 25);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(710, 325);
		this.panel13.TabIndex = 2;
		this.label14.BackColor = System.Drawing.Color.Teal;
		this.label14.Dock = System.Windows.Forms.DockStyle.Top;
		this.label14.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.ForeColor = System.Drawing.Color.White;
		this.label14.Location = new System.Drawing.Point(0, 0);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(710, 25);
		this.label14.TabIndex = 1;
		this.label14.Text = "Step Analysis Grid";
		this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.panel2.Controls.Add(this.ctrlGrdBar3);
		this.panel2.Controls.Add(this.panel14);
		this.panel2.Controls.Add(this.label16);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
		this.panel2.Location = new System.Drawing.Point(710, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(460, 350);
		this.panel2.TabIndex = 3;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(420, 1);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.grdAnalysisGroup;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(38, 23);
		this.ctrlGrdBar3.TabIndex = 43;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.panel14.Controls.Add((System.Windows.Forms.Control)(object)this.grdAnalysisGroup);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel14.Location = new System.Drawing.Point(0, 25);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(460, 325);
		this.panel14.TabIndex = 3;
		this.label16.BackColor = System.Drawing.Color.Teal;
		this.label16.Dock = System.Windows.Forms.DockStyle.Top;
		this.label16.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.ForeColor = System.Drawing.Color.White;
		this.label16.Location = new System.Drawing.Point(0, 0);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(460, 25);
		this.label16.TabIndex = 2;
		this.label16.Text = "Analysis Group Grid";
		this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.panel7.Controls.Add(this.btnCookingPicClear);
		this.panel7.Controls.Add(this.btnAnalysisPictureClear);
		this.panel7.Controls.Add(this.panel15);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPlantName);
		this.panel7.Controls.Add(this.label4);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.cmbitem);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCropYear);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.cmbanalysisGroup);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.cmbproductionNo);
		this.panel7.Controls.Add(this.txtAnalystPerson);
		this.panel7.Controls.Add(this.label15);
		this.panel7.Controls.Add(this.btnimgprevcookingpic);
		this.panel7.Controls.Add(this.btnprv);
		this.panel7.Controls.Add(this.btnimage2);
		this.panel7.Controls.Add(this.btnimg1);
		this.panel7.Controls.Add(this.cookingpic);
		this.panel7.Controls.Add(this.sampleanalysispicture);
		this.panel7.Controls.Add(this.txtremarks);
		this.panel7.Controls.Add(this.txtdocno);
		this.panel7.Controls.Add(this.label13);
		this.panel7.Controls.Add(this.label5);
		this.panel7.Controls.Add(this.label10);
		this.panel7.Controls.Add(this.label12);
		this.panel7.Controls.Add(this.txtsampledate);
		this.panel7.Controls.Add(this.label8);
		this.panel7.Controls.Add(this.label7);
		this.panel7.Controls.Add(this.txtanalysistime);
		this.panel7.Controls.Add(this.txtdocdate);
		this.panel7.Controls.Add(this.label1);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.panel7.Location = new System.Drawing.Point(3, 30);
		this.panel7.Margin = new System.Windows.Forms.Padding(2);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1170, 199);
		this.panel7.TabIndex = 0;
		this.btnCookingPicClear.BackColor = System.Drawing.Color.Teal;
		this.btnCookingPicClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCookingPicClear.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCookingPicClear.ForeColor = System.Drawing.Color.White;
		this.btnCookingPicClear.Location = new System.Drawing.Point(1000, 170);
		this.btnCookingPicClear.Name = "btnCookingPicClear";
		this.btnCookingPicClear.Size = new System.Drawing.Size(66, 25);
		this.btnCookingPicClear.TabIndex = 14;
		this.btnCookingPicClear.Text = "Clear";
		this.btnCookingPicClear.UseVisualStyleBackColor = false;
		this.btnCookingPicClear.Click += new System.EventHandler(btnCookingPicClear_Click);
		this.chkPreview.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.chkPreview.AutoSize = true;
		this.chkPreview.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkPreview.ForeColor = System.Drawing.Color.White;
		this.chkPreview.Location = new System.Drawing.Point(1089, 7);
		this.chkPreview.Name = "chkPreview";
		this.chkPreview.Size = new System.Drawing.Size(81, 18);
		this.chkPreview.TabIndex = 10;
		this.chkPreview.Text = "Preview";
		this.chkPreview.UseVisualStyleBackColor = true;
		this.btnAnalysisPictureClear.BackColor = System.Drawing.Color.Teal;
		this.btnAnalysisPictureClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAnalysisPictureClear.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAnalysisPictureClear.ForeColor = System.Drawing.Color.White;
		this.btnAnalysisPictureClear.Location = new System.Drawing.Point(782, 170);
		this.btnAnalysisPictureClear.Name = "btnAnalysisPictureClear";
		this.btnAnalysisPictureClear.Size = new System.Drawing.Size(66, 25);
		this.btnAnalysisPictureClear.TabIndex = 11;
		this.btnAnalysisPictureClear.Text = "Clear";
		this.btnAnalysisPictureClear.UseVisualStyleBackColor = false;
		this.btnAnalysisPictureClear.Click += new System.EventHandler(btnAnalysisPictureClear_Click);
		this.panel15.BackColor = System.Drawing.Color.Teal;
		this.panel15.Controls.Add(this.label17);
		this.panel15.Controls.Add(this.chkPreview);
		this.panel15.Controls.Add(this.label3);
		this.panel15.Controls.Add(this.label2);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.Location = new System.Drawing.Point(0, 0);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(1170, 31);
		this.panel15.TabIndex = 52;
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.ForeColor = System.Drawing.Color.White;
		this.label17.Location = new System.Drawing.Point(4, 6);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(167, 19);
		this.label17.TabIndex = 52;
		this.label17.Text = "In-Process Analysis";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.White;
		this.label3.Location = new System.Drawing.Point(983, 9);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(95, 15);
		this.label3.TabIndex = 29;
		this.label3.Text = "Cooking Picture";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(772, 9);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(93, 15);
		this.label2.TabIndex = 28;
		this.label2.Text = "Analysis Picture";
		this.cmbPlantName.AllowNull = (DefaultableBoolean)1;
		this.cmbPlantName.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPlantName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPlantName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance67).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPlantName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.cmbPlantName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPlantName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance68).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPlantName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance68;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPlantName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance69;
		((SpecialBoxBase)((UltraGridBase)this.cmbPlantName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance70).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance70).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPlantName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.cmbPlantName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPlantName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPlantName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance71;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance72).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPlantName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.cmbPlantName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPlantName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPlantName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance73;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance74).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPlantName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.cmbPlantName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPlantName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance75).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance75).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance75).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance75).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPlantName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance75;
		((AppearanceBase)appearance77).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPlantName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.cmbPlantName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPlantName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPlantName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.cmbPlantName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPlantName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.cmbPlantName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPlantName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPlantName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPlantName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbPlantName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPlantName).Location = new System.Drawing.Point(97, 62);
		((System.Windows.Forms.Control)(object)this.cmbPlantName).Name = "cmbPlantName";
		((System.Windows.Forms.Control)(object)this.cmbPlantName).Size = new System.Drawing.Size(249, 26);
		((System.Windows.Forms.Control)(object)this.cmbPlantName).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.cmbPlantName).Leave += new System.EventHandler(cmbPlantName_Leave);
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(7, 68);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(69, 15);
		this.label4.TabIndex = 41;
		this.label4.Text = "Plant Name";
		this.cmbitem.AllowNull = (DefaultableBoolean)1;
		this.cmbitem.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbitem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbitem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbitem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.cmbitem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbitem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance81).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance81).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbitem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance81;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbitem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance82;
		((SpecialBoxBase)((UltraGridBase)this.cmbitem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance83).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance83).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbitem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.cmbitem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbitem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance84;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance85).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance86;
		((AppearanceBase)appearance88).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance88).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance89).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance89).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance89).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance89;
		((AppearanceBase)appearance90).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance91).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.cmbitem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbitem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbitem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbitem).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbitem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbitem).Location = new System.Drawing.Point(97, 116);
		((System.Windows.Forms.Control)(object)this.cmbitem).Name = "cmbitem";
		((System.Windows.Forms.Control)(object)this.cmbitem).Size = new System.Drawing.Size(249, 26);
		((System.Windows.Forms.Control)(object)this.cmbitem).TabIndex = 4;
		this.cmbCropYear.AllowNull = (DefaultableBoolean)1;
		this.cmbCropYear.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCropYear.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCropYear.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCropYear).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance32;
		((SpecialBoxBase)((UltraGridBase)this.cmbCropYear).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance43).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance43).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance54;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance65).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance76;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance87).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance98).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance98).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance98).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance98).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance98;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCropYear).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbCropYear.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCropYear).Location = new System.Drawing.Point(457, 35);
		((System.Windows.Forms.Control)(object)this.cmbCropYear).Name = "cmbCropYear";
		((System.Windows.Forms.Control)(object)this.cmbCropYear).Size = new System.Drawing.Size(249, 26);
		((System.Windows.Forms.Control)(object)this.cmbCropYear).TabIndex = 6;
		this.cmbanalysisGroup.AllowNull = (DefaultableBoolean)1;
		this.cmbanalysisGroup.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbanalysisGroup.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbanalysisGroup.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbanalysisGroup).DisplayLayout.Appearance = (AppearanceBase)(object)appearance93;
		((UltraGridBase)this.cmbanalysisGroup).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbanalysisGroup).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance94).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance94).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance94).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbanalysisGroup).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance94;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbanalysisGroup).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance95;
		((SpecialBoxBase)((UltraGridBase)this.cmbanalysisGroup).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance96).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance96).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance96).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbanalysisGroup).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.cmbanalysisGroup).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbanalysisGroup).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance97).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbanalysisGroup).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance97;
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance99).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbanalysisGroup).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.cmbanalysisGroup).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbanalysisGroup).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbanalysisGroup).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance101).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance101).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbanalysisGroup).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.cmbanalysisGroup).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbanalysisGroup).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance102).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance102).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance102).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance102).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbanalysisGroup).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance102;
		((AppearanceBase)appearance103).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbanalysisGroup).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.cmbanalysisGroup).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbanalysisGroup).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance104).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbanalysisGroup).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.cmbanalysisGroup).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbanalysisGroup).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.cmbanalysisGroup).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbanalysisGroup).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbanalysisGroup).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbanalysisGroup).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbanalysisGroup.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbanalysisGroup).Location = new System.Drawing.Point(457, 62);
		((System.Windows.Forms.Control)(object)this.cmbanalysisGroup).Name = "cmbanalysisGroup";
		((System.Windows.Forms.Control)(object)this.cmbanalysisGroup).Size = new System.Drawing.Size(249, 26);
		((System.Windows.Forms.Control)(object)this.cmbanalysisGroup).TabIndex = 7;
		((System.Windows.Forms.Control)(object)this.cmbanalysisGroup).Leave += new System.EventHandler(cmbanalysisGroup_Leave);
		this.cmbproductionNo.AllowNull = (DefaultableBoolean)1;
		this.cmbproductionNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbproductionNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbproductionNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbproductionNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.cmbproductionNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbproductionNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance107).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance107).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance107).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbproductionNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance107;
		((AppearanceBase)appearance108).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbproductionNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance108;
		((SpecialBoxBase)((UltraGridBase)this.cmbproductionNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbproductionNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbproductionNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbproductionNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbproductionNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbproductionNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbproductionNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbproductionNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbproductionNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbproductionNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.cmbproductionNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbproductionNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbproductionNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbproductionNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.cmbproductionNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbproductionNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbproductionNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbproductionNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbproductionNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.cmbproductionNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbproductionNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbproductionNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbproductionNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbproductionNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbproductionNo).Location = new System.Drawing.Point(97, 89);
		((System.Windows.Forms.Control)(object)this.cmbproductionNo).Name = "cmbproductionNo";
		((System.Windows.Forms.Control)(object)this.cmbproductionNo).Size = new System.Drawing.Size(249, 26);
		((System.Windows.Forms.Control)(object)this.cmbproductionNo).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.cmbproductionNo).Leave += new System.EventHandler(cmbproductionNo_Leave);
		this.txtAnalystPerson.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAnalystPerson.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAnalystPerson.Location = new System.Drawing.Point(457, 91);
		this.txtAnalystPerson.Name = "txtAnalystPerson";
		this.txtAnalystPerson.Size = new System.Drawing.Size(249, 23);
		this.txtAnalystPerson.TabIndex = 8;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(351, 95);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(85, 15);
		this.label15.TabIndex = 40;
		this.label15.Text = "Analyst Person";
		this.btnimgprevcookingpic.BackColor = System.Drawing.Color.Teal;
		this.btnimgprevcookingpic.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnimgprevcookingpic.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnimgprevcookingpic.ForeColor = System.Drawing.Color.White;
		this.btnimgprevcookingpic.Location = new System.Drawing.Point(1070, 170);
		this.btnimgprevcookingpic.Name = "btnimgprevcookingpic";
		this.btnimgprevcookingpic.Size = new System.Drawing.Size(73, 25);
		this.btnimgprevcookingpic.TabIndex = 15;
		this.btnimgprevcookingpic.Text = "Preview";
		this.btnimgprevcookingpic.UseVisualStyleBackColor = false;
		this.btnimgprevcookingpic.Click += new System.EventHandler(btnimgprevcookingpic_Click);
		this.btnprv.BackColor = System.Drawing.Color.Teal;
		this.btnprv.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnprv.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnprv.ForeColor = System.Drawing.Color.White;
		this.btnprv.Location = new System.Drawing.Point(852, 170);
		this.btnprv.Name = "btnprv";
		this.btnprv.Size = new System.Drawing.Size(70, 25);
		this.btnprv.TabIndex = 12;
		this.btnprv.Text = "Preview";
		this.btnprv.UseVisualStyleBackColor = false;
		this.btnprv.Click += new System.EventHandler(btnprv_Click);
		this.btnimage2.BackColor = System.Drawing.Color.Teal;
		this.btnimage2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnimage2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnimage2.ForeColor = System.Drawing.Color.White;
		this.btnimage2.Location = new System.Drawing.Point(931, 170);
		this.btnimage2.Name = "btnimage2";
		this.btnimage2.Size = new System.Drawing.Size(65, 25);
		this.btnimage2.TabIndex = 13;
		this.btnimage2.Text = "Browse";
		this.btnimage2.UseVisualStyleBackColor = false;
		this.btnimage2.Click += new System.EventHandler(btnimage2_Click);
		this.btnimg1.BackColor = System.Drawing.Color.Teal;
		this.btnimg1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnimg1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnimg1.ForeColor = System.Drawing.Color.White;
		this.btnimg1.Location = new System.Drawing.Point(714, 170);
		this.btnimg1.Name = "btnimg1";
		this.btnimg1.Size = new System.Drawing.Size(64, 25);
		this.btnimg1.TabIndex = 10;
		this.btnimg1.Text = "Browse";
		this.btnimg1.UseVisualStyleBackColor = false;
		this.btnimg1.Click += new System.EventHandler(btnimg1_Click);
		this.cookingpic.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.cookingpic.Location = new System.Drawing.Point(930, 36);
		this.cookingpic.Name = "cookingpic";
		this.cookingpic.Size = new System.Drawing.Size(212, 133);
		this.cookingpic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.cookingpic.TabIndex = 27;
		this.cookingpic.TabStop = false;
		this.sampleanalysispicture.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.sampleanalysispicture.Location = new System.Drawing.Point(712, 36);
		this.sampleanalysispicture.Name = "sampleanalysispicture";
		this.sampleanalysispicture.Size = new System.Drawing.Size(211, 133);
		this.sampleanalysispicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.sampleanalysispicture.TabIndex = 27;
		this.sampleanalysispicture.TabStop = false;
		this.txtremarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtremarks.Location = new System.Drawing.Point(97, 145);
		this.txtremarks.Multiline = true;
		this.txtremarks.Name = "txtremarks";
		this.txtremarks.Size = new System.Drawing.Size(609, 48);
		this.txtremarks.TabIndex = 5;
		this.txtdocno.Enabled = false;
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocno.Location = new System.Drawing.Point(97, 37);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.Size = new System.Drawing.Size(73, 23);
		this.txtdocno.TabIndex = 0;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(7, 95);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(78, 15);
		this.label13.TabIndex = 0;
		this.label13.Text = "Job Order No";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(351, 41);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(57, 15);
		this.label5.TabIndex = 0;
		this.label5.Text = "Crop Year";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(351, 68);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(86, 15);
		this.label10.TabIndex = 0;
		this.label10.Text = "Analysis Group";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(458, 120);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(101, 19);
		this.label12.TabIndex = 0;
		this.label12.Text = "Analysis Time";
		this.txtsampledate.AutoSize = true;
		this.txtsampledate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtsampledate.Location = new System.Drawing.Point(172, 41);
		this.txtsampledate.Name = "txtsampledate";
		this.txtsampledate.Size = new System.Drawing.Size(57, 15);
		this.txtsampledate.TabIndex = 0;
		this.txtsampledate.Text = "Doc Date";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(7, 122);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(67, 15);
		this.label8.TabIndex = 0;
		this.label8.Text = "Item Name";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(7, 149);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(52, 15);
		this.label7.TabIndex = 0;
		this.label7.Text = "Remarks";
		this.txtanalysistime.CustomFormat = "hhh:mmm:ss tt";
		this.txtanalysistime.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtanalysistime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtanalysistime.Location = new System.Drawing.Point(574, 116);
		this.txtanalysistime.Name = "txtanalysistime";
		this.txtanalysistime.Size = new System.Drawing.Size(132, 27);
		this.txtanalysistime.TabIndex = 9;
		this.txtdocdate.CustomFormat = "dd-MM-yy";
		this.txtdocdate.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtdocdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtdocdate.Location = new System.Drawing.Point(230, 37);
		this.txtdocdate.Name = "txtdocdate";
		this.txtdocdate.Size = new System.Drawing.Size(116, 22);
		this.txtdocdate.TabIndex = 1;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(7, 41);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(83, 15);
		this.label1.TabIndex = 0;
		this.label1.Text = "Document No";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1184, 611);
		base.Controls.Add(this.tabControl1);
		base.KeyPreview = true;
		base.Name = "InvLabAnalysisInProcess";
		this.Text = "InvLabPurchaseAnalysis";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(InvLabAnalysisInProcess_FormClosing);
		base.Load += new System.EventHandler(InvLabPurchaseAnalysis_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(InvLabPurchaseAnalysis_KeyDown);
		this.btn.ResumeLayout(false);
		this.btn.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdAnalysisGroup).EndInit();
		this.tabHistory.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		this.panel10.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdGroupAnalysisHistory).EndInit();
		this.panel9.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdStepAnalysisHistory).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		this.panel6.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdhistory).EndInit();
		this.panel20.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPlantHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobOrderHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAnalystHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemHistory).EndInit();
		this.panel16.ResumeLayout(false);
		this.panel16.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabControl1.ResumeLayout(false);
		this.tabForm.ResumeLayout(false);
		this.tabForm.PerformLayout();
		this.panel1.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdStepAnalysis).EndInit();
		this.panel13.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel14.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.panel15.ResumeLayout(false);
		this.panel15.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbPlantName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbitem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCropYear).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbanalysisGroup).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbproductionNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cookingpic).EndInit();
		((System.ComponentModel.ISupportInitialize)this.sampleanalysispicture).EndInit();
		base.ResumeLayout(false);
	}
}

}
