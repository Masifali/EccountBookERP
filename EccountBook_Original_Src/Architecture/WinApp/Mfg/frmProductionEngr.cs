using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Inventory;
using Architecture.BLL.Mfg;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Authentication;
using Architecture.Model.Inventory;
using Architecture.Model.Main;
using Architecture.Model.Mfg;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Mfg;

public class frmProductionEngr : Form
{
	private int RecId;

	private int DefaultProductionInputItemCustomGroupId;

	private int DefaultProductionScrapItemCustomGroupId;

	private int DefaultProductionOverHeadCustomGroupId;

	private int DefaultCastingWarehouseId;

	private int DefaultProductionStageId;

	private string DefaultProductionStageName = "";

	private int DefaultDaysToLessFromHistoryFromDate;

	private string AttachmentsValues = "";

	private string CustomAttachmentsValues = "";

	private bool BranchFeature;

	private Attachment AT = new Attachment();

	private Rightsobjects formRights = new Rightsobjects();

	private DataTable dtdetailOutput = new DataTable();

	private DataTable dtdetailInput = new DataTable();

	private DataTable dtScrap = new DataTable();

	private DataTable dtOverHead = new DataTable();

	private DataTable dtitemInput = new DataTable();

	private DataTable dtitemScrap = new DataTable();

	private DataTable dtitemOutput = new DataTable();

	private DataTable dtAllUom = new DataTable();

	private DataTable dtWarehouse = new DataTable();

	private DataTable dtAccounts = new DataTable();

	private DataTable dtAllBom = new DataTable();

	private DataTable dtAllLookups = new DataTable();

	private DropDownBind DDL = new DropDownBind();

	private List<ProductionOutPutDetail> RemoveOutPutDetaillist = new List<ProductionOutPutDetail>();

	private List<ProductionInputDetail> RemoveInPutDetaillist = new List<ProductionInputDetail>();

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private Panel panel2;

	private ToolStrip toolStrip1;

	private ToolStripButton btnNew;

	private ToolStripButton btnUpdate;

	private ToolStripButton btnSave;

	private ToolStripButton btnAttachment;

	private Panel panel7;

	private Panel panel8;

	internal GridEX GrdHistory;

	private CtrlGrdBar ctrlGrdBar1;

	private ToolStripButton BtnRefresh;

	internal GridEX grdOutputHistory;

	private CtrlGrdBar ctrlGrdBar2;

	private Panel panel6;

	internal GridEX grdOutPut;

	private Panel panel4;

	private Panel panel13;

	private Label label27;

	private CheckBox ChkBox;

	private Label label24;

	private TextBox txtremarks;

	private TextBox txtProductionNo;

	private DateTimePicker DocDate;

	private Label label5;

	private TextBox txtdocno;

	private Panel panel3;

	private Button btnshow;

	private DateTimePicker datToDate;

	private Label label8;

	private DateTimePicker datFromDate;

	private Label label9;

	private Label label11;

	private Label label10;

	private TextBox txtDocNoTo;

	private TextBox txtDocNoFrom;

	private Label label23;

	private ToolStripButton btnPrint1619;

	private ToolStripButton btnshortcutkeys;

	private Label label13;

	private GroupBox groupBox1;

	private ToolStrip toolStrip2;

	private ToolStripButton BtnNewHistory;

	private ToolStripButton BtnRefreshHistory;

	private Label label15;

	private ToolStripButton BtnDelete;

	private TabControl tabControl2;

	private TabPage tabInPut;

	private TabPage tabOutPut;

	private DateTimePicker OutTime;

	private Label label20;

	private DateTimePicker InTime;

	private Label label16;

	private Label label17;

	private Label label18;

	internal GridEX grdInput;

	private Panel panel10;

	private Label label21;

	private CtrlGrdBar ctrlGrdBar3;

	internal UltraCombo CmbHeatCode;

	private Label label19;

	private TextBox txtBatchDescription;

	internal UltraCombo CmbBillOfMaterial;

	private Label label25;

	private Panel panel5;

	private Panel panel14;

	private Label label22;

	internal GridEX grdOverHead;

	private Panel panel12;

	private Panel panel15;

	private Label label1;

	internal GridEX grdScrap;

	private Button BtnLoadInputFromBom;

	private TabControl tabControl3;

	private TabPage tabPage4;

	internal GridEX grdInputHistory;

	private TabPage tabPage3;

	private TabPage tabPage5;

	internal GridEX grdScrapHistory;

	private TabPage tabPage6;

	internal GridEX grdOverHeadHistory;

	private Panel panel9;

	private Label label6;

	private TextBox txtExpectedTotalOutputValue;

	private Label label2;

	private Panel panel20;

	private Label label3;

	private TextBox txtTotalInputs;

	private Label label4;

	private Label label7;

	private Label label12;

	private Label label14;

	private TextBox txtOverHead;

	private TextBox txtLessTotalScrap;

	private Label label26;

	private TextBox txtActualtotalOutPutValue;

	private Label label28;

	internal UltraCombo CmbCastingType;

	private Label label77;

	private TextBox txtTotalTime;

	private Label label29;

	private Panel panel11;

	internal GridEX grdSummary;

	private Panel panel16;

	private Label label30;

	private CtrlGrdBar ctrlGrdBar4;

	private Label labelGainLossPercent;

	private Label labelGainLossCaption;

	private ToolStripButton BtnSaveAs;

	private CheckBox ChkSettlement;

	private ToolStripButton BtnGenerate;

	private ToolStripButton btnVoucherReport;

	internal UltraCombo CmbJobLot;

	private Button BtnLoadOutPut;

	public frmProductionEngr(UserAccount user)
	{
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Expected O, but got Unknown
		InitializeComponent();
		InitializeComponentCustom();
		InitializeComponentMethod();
		UserAccount = user;
	}

	private void InitializeComponentCustom()
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(10, 10);
			base.AutoScrollMinSize = new Size(1180, 625);
			dtAccounts.Columns.Add("Id", typeof(int));
			dtAccounts.Columns.Add("AccountTitle");
			dtAccounts.Columns.Add("AccountCode");
			dtitemInput.Columns.Add("Id", typeof(int));
			dtitemInput.Columns.Add("ItemName");
			dtitemInput.Columns.Add("ItemCode");
			dtitemScrap.Columns.Add("Id", typeof(int));
			dtitemScrap.Columns.Add("ItemName");
			dtitemScrap.Columns.Add("ItemCode");
			dtitemOutput.Columns.Add("Id", typeof(int));
			dtitemOutput.Columns.Add("ItemName");
			dtitemOutput.Columns.Add("ItemCode");
			dtitemOutput.Columns.Add("SampleNo", typeof(string));
			dtitemOutput.Columns.Add("ItemCategoryId", typeof(int));
			dtdetailInput.Columns.Add("Id", typeof(int));
			dtdetailInput.Columns.Add("BomHeaderId", typeof(int));
			dtdetailInput.Columns.Add("BomInputDetailId", typeof(int));
			dtdetailInput.Columns.Add("WarehouseId", typeof(int));
			dtdetailInput.Columns.Add("ItemId", typeof(int));
			dtdetailInput.Columns.Add("ItemName", typeof(string));
			dtdetailInput.Columns.Add("PackUomId", typeof(int));
			dtdetailInput.Columns.Add("PackUom", typeof(string));
			dtdetailInput.Columns.Add("PackEquivalent", typeof(double));
			dtdetailInput.Columns.Add("NetWeight", typeof(double));
			dtdetailInput.Columns.Add("Rate", typeof(double));
			dtdetailInput.Columns.Add("RateUomId", typeof(double));
			dtdetailInput.Columns.Add("RateUom", typeof(string));
			dtdetailInput.Columns.Add("RateEquivalent", typeof(double));
			dtdetailInput.Columns.Add("Amount", typeof(double));
			dtdetailOutput.Columns.Add("Id", typeof(int));
			dtdetailOutput.Columns.Add("WorkOrderId", typeof(int));
			dtdetailOutput.Columns.Add("workOrderItemDetailId", typeof(int));
			dtdetailOutput.Columns.Add("WorkOrderNo", typeof(string));
			dtdetailOutput.Columns.Add("WarehouseId", typeof(int));
			dtdetailOutput.Columns.Add("ItemId", typeof(int));
			dtdetailOutput.Columns.Add("ItemName", typeof(string));
			dtdetailOutput.Columns.Add("PackUomId", typeof(int));
			dtdetailOutput.Columns.Add("PackUom", typeof(string));
			dtdetailOutput.Columns.Add("PackEquivalent", typeof(string));
			dtdetailOutput.Columns.Add("ModalId", typeof(string));
			dtdetailOutput.Columns.Add("ModalDescription", typeof(string));
			dtdetailOutput.Columns.Add("ProcessStatusId", typeof(string));
			dtdetailOutput.Columns.Add("ProcessStatus", typeof(string));
			dtdetailOutput.Columns.Add("NetWeight", typeof(double));
			dtdetailOutput.Columns.Add("BalQty", typeof(double));
			dtdetailOutput.Columns.Add("WeightPerQty", typeof(decimal));
			dtdetailOutput.Columns.Add("CastingWeight", typeof(decimal));
			dtdetailOutput.Columns.Add("Rate", typeof(double));
			dtdetailOutput.Columns.Add("RateUomId", typeof(double));
			dtdetailOutput.Columns.Add("RateUom", typeof(string));
			dtdetailOutput.Columns.Add("RateEquivalent", typeof(string));
			dtdetailOutput.Columns.Add("Amount", typeof(double));
			dtScrap.Columns.Add("Id", typeof(int));
			dtScrap.Columns.Add("WarehouseId", typeof(int));
			dtScrap.Columns.Add("ItemId", typeof(int));
			dtScrap.Columns.Add("ItemName", typeof(string));
			dtScrap.Columns.Add("PackUomId", typeof(int));
			dtScrap.Columns.Add("PackUom", typeof(string));
			dtScrap.Columns.Add("PackEquivalent", typeof(decimal));
			dtScrap.Columns.Add("NetWeight", typeof(decimal));
			dtScrap.Columns.Add("Rate", typeof(decimal));
			dtScrap.Columns.Add("RateUomId", typeof(int));
			dtScrap.Columns.Add("RateUom", typeof(string));
			dtScrap.Columns.Add("RateEquivalent", typeof(decimal));
			dtScrap.Columns.Add("Amount", typeof(decimal));
			dtOverHead.Columns.Add("Id", typeof(int));
			dtOverHead.Columns.Add("OverHeadAccountId", typeof(int));
			dtOverHead.Columns.Add("PercentOfTotalInput", typeof(decimal));
			dtOverHead.Columns.Add("Qty", typeof(decimal));
			dtOverHead.Columns.Add("Rate", typeof(decimal));
			dtOverHead.Columns.Add("Amount", typeof(decimal));
			dtOverHead.Columns.Add("OhRemarks", typeof(string));
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			BtnSaveAs.Visible = false;
			BtnDelete.Visible = false;
			DocDate.Focus();
			datFromDate.Value = DateTime.Now.AddDays(-3.0);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InitializeComponentMethod()
	{
		Task.Run(delegate
		{
			formRights = CommonServices.SetRightsValueInRightsObject(base.Name);
			BranchFeature = CommonServices.GetERPFeatureById(11);
			ConfigrationsAllocation obj2 = clsGlobalVariables.configrationsAllocation.FirstOrDefault((ConfigrationsAllocation r) => r.ConfigDescription == "DefaultProductionInputItemCustomGroupId");
			DefaultProductionInputItemCustomGroupId = Conversion.ToInt((object)((obj2 != null) ? obj2.ConfigKey : null));
			ConfigrationsAllocation obj3 = clsGlobalVariables.configrationsAllocation.FirstOrDefault((ConfigrationsAllocation r) => r.ConfigDescription == "DefaultProductionScrapItemCustomGroupId");
			DefaultProductionScrapItemCustomGroupId = Conversion.ToInt((object)((obj3 != null) ? obj3.ConfigKey : null));
			ConfigrationsAllocation obj4 = clsGlobalVariables.configrationsAllocation.FirstOrDefault((ConfigrationsAllocation r) => r.ConfigDescription == "DefaultProductionOverHeadCustomGroupId");
			DefaultProductionOverHeadCustomGroupId = Conversion.ToInt((object)((obj4 != null) ? obj4.ConfigKey : null));
			ConfigrationsAllocation obj5 = clsGlobalVariables.configrationsAllocation.FirstOrDefault((ConfigrationsAllocation r) => r.ConfigDescription == "DefaultCastingWarehouse");
			DefaultCastingWarehouseId = Conversion.ToInt((object)((obj5 != null) ? obj5.ConfigKey : null));
			ConfigrationsAllocation obj6 = clsGlobalVariables.configrationsAllocation.FirstOrDefault((ConfigrationsAllocation r) => r.ConfigDescription == "DefaultDaysToLessFromHistoryFromDate");
			DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)((obj6 != null) ? obj6.ConfigKey : null));
			ConfigrationsAllocation obj7 = clsGlobalVariables.configrationsAllocation.FirstOrDefault((ConfigrationsAllocation r) => r.ConfigDescription == "DefaultProductionStageId");
			DefaultProductionStageId = Conversion.ToInt((object)((obj7 != null) ? obj7.ConfigKey : null));
			LookUps obj8 = clsGlobalVariables.GetAllLookUps_Engr.FirstOrDefault((LookUps r) => r.Id == DefaultProductionStageId);
			DefaultProductionStageName = Conversion.ToString((object)((obj8 != null) ? obj8.LookupName : null));
			string documentNoData = GenerateCode();
			DataTable heatCode = GetHeatCode();
			DataTable uomData = UomGetAll();
			DataTable itemDataForOutPut = GetItemDataForOutPut();
			DataTable jobLotData = BindJobLot();
			GetWarehouses();
			GetAllBomHeader();
			return new
			{
				formRights = formRights,
				documentNoData = documentNoData,
				HeatCodeData = heatCode,
				UomData = uomData,
				ItemDataForOutput = itemDataForOutPut,
				JobLotData = jobLotData
			};
		}).ContinueWith(t =>
		{
			if (t.IsFaulted)
			{
				MessageBox.Show("Error occurred during database call.");
			}
			else
			{
				var result = t.Result;
				btnSave.Enabled = formRights.DoHaveSaveRight;
				BtnSaveAs.Enabled = formRights.DoHaveSaveRight;
				btnUpdate.Enabled = formRights.DoHaveUpdateRights;
				BtnDelete.Enabled = formRights.DoHaveCanDelete;
				btnPrint1619.Enabled = formRights.DoHavePrintRights;
				ChkBox.Checked = formRights.DoHavePrintRights;
				ctrlGrdBar1.mGridChooseFielder.Enabled = formRights.DoHaveFieldChooserRights;
				ctrlGrdBar1.mGridSaveLayouts.Enabled = formRights.DoHaveSaveLayoutRights;
				ctrlGrdBar1.mGridPrint.Enabled = formRights.DoHaveGridPrintRights;
				ctrlGrdBar1.mGridExport.Enabled = formRights.DoHaveGridExportRights;
				ctrlGrdBar1.GroupCollaps.Enabled = formRights.DoHaveGridGroupCollapseRights;
				ctrlGrdBar1.GroupExpand.Enabled = formRights.DoHaveGridGroupExpandRights;
				ctrlGrdBar2.mGridChooseFielder.Enabled = formRights.DoHaveFieldChooserRights;
				ctrlGrdBar2.mGridSaveLayouts.Enabled = formRights.DoHaveSaveLayoutRights;
				ctrlGrdBar2.mGridPrint.Enabled = formRights.DoHaveGridPrintRights;
				ctrlGrdBar2.mGridExport.Enabled = formRights.DoHaveGridExportRights;
				ctrlGrdBar2.GroupCollaps.Enabled = formRights.DoHaveGridGroupCollapseRights;
				ctrlGrdBar2.GroupExpand.Enabled = formRights.DoHaveGridGroupExpandRights;
				ctrlGrdBar3.mGridChooseFielder.Enabled = formRights.DoHaveFieldChooserRights;
				ctrlGrdBar3.mGridSaveLayouts.Enabled = formRights.DoHaveSaveLayoutRights;
				ctrlGrdBar3.mGridPrint.Enabled = formRights.DoHaveGridPrintRights;
				ctrlGrdBar3.mGridExport.Enabled = formRights.DoHaveGridExportRights;
				ctrlGrdBar3.GroupCollaps.Enabled = formRights.DoHaveGridGroupCollapseRights;
				ctrlGrdBar3.GroupExpand.Enabled = formRights.DoHaveGridGroupExpandRights;
				if (RecId == 0)
				{
					UpdateDocumentNoUI(result.documentNoData);
				}
				BindHeatCode(result.HeatCodeData);
				dtAllUom = result.UomData;
				BindItemInputScrap();
				BindOverHeadAccounts();
				BindItemForOutPut(result.ItemDataForOutput);
				BindCastingType();
				Joblotfill(result.JobLotData);
				if (dtdetailInput != null && dtdetailInput.Columns.Count > 0)
				{
					AddRowInInputGrid();
					grdInput.RetrieveStructure();
					grdInputSettings();
				}
				if (dtdetailOutput != null && dtdetailOutput.Columns.Count > 0)
				{
					AddRowInOutputGrid();
					grdOutPut.RetrieveStructure();
					grdOutPutSettings();
					GridEXRow[] dataRows = grdOutPut.GetDataRows();
					foreach (GridEXRow obj in dataRows)
					{
						obj.BeginEdit();
						obj.Cells["WareHouseId"].Value = DefaultCastingWarehouseId;
						obj.EndEdit();
					}
				}
				if (dtScrap != null && dtScrap.Columns.Count > 0)
				{
					AddRowInScrapGrid();
					grdScrap.RetrieveStructure();
					grdScrapSettings();
				}
				if (dtOverHead != null && dtOverHead.Columns.Count > 0)
				{
					AddRowInOverHeadGrid();
					grdOverHead.RetrieveStructure();
					grdOverHeadGridSetting();
				}
			}
		}, TaskScheduler.FromCurrentSynchronizationContext());
	}

	private DataTable BindJobLot()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		try
		{
			return JobLotsAllocationToBranch.GetJobLotsAllocatedToBranchByBranchId(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void Joblotfill(DataTable dt)
	{
		try
		{
			int ID = Conversion.ToInt(CmbJobLot.Value);
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbJobLot, "Id", "JobLotDescription", "JobLot", true);
				((UltraGridBase)CmbJobLot).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				if (ID > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID))
					{
						CmbJobLot.Value = ID;
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
			throw new Exception(ex.Message);
		}
	}

	private void BindCastingType()
	{
		try
		{
			List<LookUps> lst = clsGlobalVariables.GetAllLookUps_Engr;
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("CastingType", typeof(string));
			if (lst != null && lst.Count > 0)
			{
				List<LookUps> filteredList = lst.Where((LookUps r) => r.LookUpTypeId == 1).ToList();
				for (int i = 0; i < filteredList.Count; i++)
				{
					dt.Rows.Add(filteredList[i].Id, filteredList[i].LookupName);
				}
			}
			int DetailId = Conversion.ToInt(CmbCastingType.Value);
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbCastingType, "Id", "CastingType", "Casting Type", ZeroIndex: true);
				((UltraGridBase)CmbCastingType).DisplayLayout.Bands[0].Columns[1].Width = 350;
				if (DetailId > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == DetailId))
					{
						CmbCastingType.Value = DetailId;
					}
					else
					{
						((Control)(object)CmbCastingType).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbCastingType).Text = string.Empty;
				((UltraGridBase)CmbCastingType).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private string GenerateCode()
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
			return Conversion.ToString((object)ProductionHeader.GenerateCode(new ProductionHeader
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				DocumentTypeId = 1670,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			}));
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void UpdateDocumentNoUI(string documentData)
	{
		txtdocno.Text = documentData;
	}

	private DataTable GetHeatCode()
	{
		try
		{
			return CommonServices.StaticColumnsService("ProductionHeatCodeEngr");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BindHeatCode(DataTable dt)
	{
		try
		{
			int Id = Conversion.ToInt(CmbHeatCode.Value);
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbHeatCode, "Id", "type", "Heat Code", ZeroIndex: false);
				if (Id > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
					{
						CmbHeatCode.Value = Id;
					}
					else
					{
						((Control)(object)CmbHeatCode).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbHeatCode).Text = string.Empty;
				((UltraGridBase)CmbHeatCode).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GetAllBomHeader()
	{
		try
		{
			dtAllBom.Clear();
			dtAllBom = BomHeader.BOMNameForProduction(UserAccount.OrganizationId, UserAccount.CompanyId, RecId, 0);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BindBomHeader(int CastingTypeId)
	{
		try
		{
			int Id = Conversion.ToInt(CmbBillOfMaterial.Value);
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("BomName", typeof(string));
			dt.Columns.Add("EffectedDate", typeof(DateTime));
			if (dtAllBom != null && dtAllBom.Rows.Count > 0)
			{
				dt.Clear();
				foreach (DataRow r in dtAllBom.Rows)
				{
					if (Conversion.ToInt(r["CastingTypeId"]) == CastingTypeId)
					{
						dt.Rows.Add(r["Id"], r["BomName"], r["EffectedDate"]);
					}
				}
			}
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbBillOfMaterial, "Id", "BomName", "Bill Of Material", ZeroIndex: false);
				if (Id > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
					{
						CmbBillOfMaterial.Value = Id;
					}
					else
					{
						((Control)(object)CmbBillOfMaterial).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbBillOfMaterial).Text = string.Empty;
				((UltraGridBase)CmbBillOfMaterial).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private DataTable UomGetAll()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			return UOMSchedule.Getall(new UOMSchedule
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BindItemInputScrap()
	{
		try
		{
			List<getGlobalAllItemsWithCustomGroup> lst = clsGlobalVariables.AllItemsWithCustomGroupId;
			dtitemInput.Rows.Clear();
			dtitemScrap.Rows.Clear();
			if (lst == null || lst.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < lst.Count; i++)
			{
				if (DefaultProductionInputItemCustomGroupId == lst[i].CustomGroupId)
				{
					dtitemInput.Rows.Add(lst[i].ItemId, lst[i].ItemName, lst[i].ItemCode);
				}
				if (DefaultProductionScrapItemCustomGroupId == lst[i].CustomGroupId)
				{
					dtitemScrap.Rows.Add(lst[i].ItemId, lst[i].ItemName, lst[i].ItemCode);
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private DataTable GetItemDataForOutPut()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		try
		{
			dtitemOutput.Clear();
			return Item.AllItemsBindForEngrWithModalOnly(new Item
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BindItemForOutPut(DataTable dt)
	{
		try
		{
			dtitemOutput.Clear();
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtitemOutput.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ItemName"], dt.Rows[i]["ItemCodeNew"], dt.Rows[i]["SampleNo"], dt.Rows[i]["ItemCategoryId"]);
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GetWarehouses()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		try
		{
			dtWarehouse.Rows.Clear();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = (BranchFeature ? UserAccount.BranchesId : 0);
			dtWarehouse = WarehousesAllocationToBranch.GetWarehousesAllocatedToBranchByBranchId(obj);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private DataTable dtUomByItemId(int ItemId)
	{
		try
		{
			DataTable dtUom = new DataTable();
			dtUom.Columns.Add("UomId", typeof(int));
			dtUom.Columns.Add("Uom", typeof(string));
			dtUom.Columns.Add("UomEquivalent", typeof(double));
			dtUom.Clear();
			if (dtAllUom.Rows.Count > 0)
			{
				foreach (DataRow r in dtAllUom.Rows)
				{
					if (Conversion.ToInt(r["ItemId"]) == Conversion.ToInt((object)ItemId))
					{
						dtUom.Rows.Add(r["Id"], r["UOMCode"], r["Equivalent"]);
					}
				}
			}
			return dtUom;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BindOverHeadAccounts()
	{
		try
		{
			dtAccounts.Clear();
			List<getGlobalAllAccountsWithCustomGroup> dt = clsGlobalVariables.AllAccountsWithCustomGroupId.Where((getGlobalAllAccountsWithCustomGroup r) => r.ExistsInCustomGroup).ToList();
			if (dt == null || dt.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < dt.Count; i++)
			{
				if (DefaultProductionOverHeadCustomGroupId == dt[i].CustomGroupId)
				{
					dtAccounts.Rows.Add(dt[i].ChartOfAccountId, dt[i].AccountTitle, dt[i].AccountCode);
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CmbCastingType_Leave(object sender, EventArgs e)
	{
		try
		{
			int CastingTypeId = Conversion.ToInt(CmbCastingType.Value);
			if (((UltraGridBase)CmbCastingType).ActiveRow != null && CastingTypeId > 0)
			{
				BindBomHeader(CastingTypeId);
				return;
			}
			((Control)(object)CmbBillOfMaterial).Text = string.Empty;
			((UltraGridBase)CmbBillOfMaterial).DataSource = null;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (Conversion.ToInt((object)txtdocno.Text) == 0 || txtdocno.Text.Trim() == "" || txtdocno.Text.Trim() == "0" || txtdocno.Text.Trim() == null)
		{
			MessageBox.Show("DocNo Field is Required");
			txtdocno.Focus();
			return false;
		}
		if (txtProductionNo.Text.Trim() == "" || txtProductionNo.Text.Trim() == "0" || txtProductionNo.Text.Trim() == null)
		{
			MessageBox.Show("Production No Field is Required");
			txtProductionNo.Focus();
			return false;
		}
		if (((Control)(object)CmbJobLot).Text.Trim() == "" || ((Control)(object)CmbJobLot).Text.Trim() == "0" || ((Control)(object)CmbJobLot).Text.Trim() == null)
		{
			MessageBox.Show("Batch Code Field is Required");
			CmbJobLot.Focus();
			return false;
		}
		if (((UltraGridBase)CmbJobLot).ActiveRow == null || Conversion.ToInt(CmbJobLot.Value) == 0)
		{
			MessageBox.Show("Batch Code Field is Required");
			CmbJobLot.Focus();
			return false;
		}
		if (((UltraGridBase)CmbHeatCode).ActiveRow == null || Conversion.ToInt(CmbHeatCode.Value) == 0)
		{
			MessageBox.Show("Heat Code Field is Required");
			CmbHeatCode.Focus();
			return false;
		}
		if (Conversion.ToDateTime((object)InTime.Value) >= Conversion.ToDateTime((object)OutTime.Value))
		{
			MessageBox.Show("In time cannot be equal to or greater than out time.");
			InTime.Focus();
			return false;
		}
		return true;
	}

	private void BtnLoadInputFromBom_Click(object sender, EventArgs e)
	{
		try
		{
			int BOMId = Conversion.ToInt(CmbBillOfMaterial.Value);
			if (((UltraGridBase)CmbBillOfMaterial).ActiveRow == null || BOMId == 0)
			{
				MessageBox.Show("BillOfMaterial Field is Required");
				CmbBillOfMaterial.Focus();
			}
			else
			{
				FillInputDetailForMultiBom(BOMId);
				Calculation();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillInputDetailForMultiBom(int BOMId)
	{
		try
		{
			if (grdInput.GetDataRows().Count() > 0 && MessageBox.Show("Input Grid already have Records.Do you want To Refresh Data?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			int warehouseId = 0;
			if (dtWarehouse.Rows.Count > 0)
			{
				DataRow firstRow = (from row in dtWarehouse.AsEnumerable()
					where row.Field<int>("Id") > 0
					select row).FirstOrDefault();
				if (firstRow != null)
				{
					warehouseId = Conversion.ToInt(firstRow["Id"]);
				}
			}
			BomHeader bOMHeader = BomHeader.ReadById(BOMId);
			if (bOMHeader != null && bOMHeader.BomInputDetailList.Count > 0)
			{
				bool flag = false;
				for (int i = 0; i < bOMHeader.BomInputDetailList.Count; i++)
				{
					if (grdInput.GetDataRows().Length != 0)
					{
						GridEXRow[] dataRows = grdInput.GetDataRows();
						for (int j = 0; j < dataRows.Length; j++)
						{
							if (Conversion.ToInt(dataRows[j].Cells["BomInputDetailId"].Value) == Conversion.ToInt((object)bOMHeader.BomInputDetailList[i].BomInputDetailId))
							{
								flag = true;
								break;
							}
						}
					}
					if (!flag)
					{
						dtdetailInput.Rows.Add(0, bOMHeader.BomInputDetailList[i].BomHeaderId, bOMHeader.BomInputDetailList[i].BomInputDetailId, warehouseId, bOMHeader.BomInputDetailList[i].ItemId, bOMHeader.BomInputDetailList[i].ItemName, bOMHeader.BomInputDetailList[i].ItemUomId, bOMHeader.BomInputDetailList[i].PackUom, bOMHeader.BomInputDetailList[i].PackUomEquivalent, bOMHeader.BomInputDetailList[i].WeightKg, bOMHeader.BomInputDetailList[i].Rate, bOMHeader.BomInputDetailList[i].RateUomId, bOMHeader.BomInputDetailList[i].RateUom, bOMHeader.BomInputDetailList[i].RateUomEquivalent, bOMHeader.BomInputDetailList[i].Amount);
					}
					flag = false;
				}
				grdInput.DataSource = dtdetailInput;
				grdInput.RetrieveStructure();
				grdInputSettings();
			}
			else
			{
				grdInput.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GridComboBind(GridEX grd)
	{
		try
		{
			grd.RootTable.Columns["WarehouseId"].ColumnType = (ColumnType)0;
			grd.RootTable.Columns["WarehouseId"].HasValueList = true;
			grd.RootTable.Columns["WarehouseId"].EditType = (EditType)4;
			grd.RootTable.Columns["WarehouseId"].LimitToList = true;
			grd.RootTable.Columns["WarehouseId"].ValueList.PopulateValueList((IEnumerable)dtWarehouse.DefaultView, "Id", "WareHouseName");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInInputGrid()
	{
		dtdetailInput.Rows.Add(0, 0, 0, DefaultCastingWarehouseId, 0, "", 0, "", 0, 0, 0, 0, "", 0, 0);
		grdInput.DataSource = dtdetailInput;
	}

	private void grdInputSettings()
	{
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Expected O, but got Unknown
		try
		{
			grdInput.RootTable.Columns["Id"].Visible = false;
			grdInput.RootTable.Columns["BomHeaderId"].Visible = false;
			grdInput.RootTable.Columns["ItemId"].Visible = false;
			grdInput.RootTable.Columns["BomInputDetailId"].Visible = false;
			grdInput.RootTable.Columns["PackUomId"].Visible = false;
			grdInput.RootTable.Columns["PackEquivalent"].Visible = false;
			grdInput.RootTable.Columns["RateUomId"].Visible = false;
			grdInput.RootTable.Columns["RateEquivalent"].Visible = false;
			grdInput.RootTable.Columns["WarehouseId"].Caption = "Ware House";
			grdInput.RootTable.Columns["PackUom"].Caption = "Uom";
			grdInput.RootTable.Columns["NetWeight"].Caption = "Qty";
			grdInput.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grdInput.RootTable.Columns)
			{
				GridEXColumn column = item;
				column.HeaderAlignment = (TextAlignment)2;
				CommonServices.GridColumnSettings(column, 2);
				if (column.Key == "Rate" || column.Key == "NetWeight")
				{
					column.EditType = (EditType)1;
				}
				else
				{
					column.EditType = (EditType)0;
				}
				column.FilterEditType = (FilterEditType)1;
			}
			GridComboBind(grdInput);
			grdInput.RootTable.Columns["WarehouseId"].Width = Constants.InventoryConstants.WareHouseName;
			grdInput.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			grdInput.RootTable.Columns["PackUom"].Width = Constants.InventoryConstants.UomConstant;
			grdInput.RootTable.Columns["NetWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdInput.RootTable.Columns["Rate"].Width = Constants.InventoryConstants.ExRateConstant;
			grdInput.RootTable.Columns["RateUom"].Width = Constants.InventoryConstants.UomConstant;
			grdInput.RootTable.Columns["Amount"].Width = Constants.InventoryConstants.AmountConstant;
			grdInput.RootTable.Columns.Add("Delete");
			grdInput.RootTable.Columns["Delete"].Key = "Delete";
			grdInput.RootTable.Columns["Delete"].Caption = "X";
			grdInput.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdInput.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdInput.RootTable.Columns["Delete"].ButtonText = "X";
			grdInput.RootTable.Columns["Delete"].Width = 20;
			grdInput.RootTable.Columns["Delete"].Position = 0;
			grdInput.RootTable.Columns.Add("Add");
			grdInput.RootTable.Columns["Add"].Key = "Add";
			grdInput.RootTable.Columns["Add"].Caption = "+";
			grdInput.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdInput.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdInput.RootTable.Columns["Add"].ButtonText = "+";
			grdInput.RootTable.Columns["Add"].Width = 20;
			grdInput.RootTable.Columns["Add"].Position = 1;
			grdInput.FrozenColumns = 2;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInput_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdInput.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				if (e.Column.Key == "Delete")
				{
					DeleteInputDetailRow(r);
				}
				if (e.Column.Key == "Add")
				{
					AddRowInInputGrid();
				}
				Calculation();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeleteInputDetailRow(GridEXRow r)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		try
		{
			if (r != null && (int)r.RowType == 0)
			{
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					ProductionInputDetail detail = new ProductionInputDetail();
					detail.productionInputDetailId = Conversion.ToInt(r.Cells["Id"].Value);
					detail.BomHeaderId = Conversion.ToInt(r.Cells["BomHeaderId"].Value);
					detail.BomInputDetailId = Conversion.ToInt(r.Cells["BomInputDetailId"].Value);
					detail.WareHouseId = Conversion.ToInt(r.Cells["WarehouseId"].Value);
					detail.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					detail.ItemUomId = Conversion.ToInt(r.Cells["PackUomId"].Value);
					detail.Qty = Conversion.ToDecimal(r.Cells["NetWeight"].Value);
					detail.Weight = Conversion.ToDecimal(r.Cells["NetWeight"].Value);
					detail.ItemRate = Conversion.ToDecimal(r.Cells["Rate"].Value);
					detail.RateUomId = Conversion.ToInt((object)r.Cells["RateUomId"].Text);
					detail.Amount = Conversion.ToDecimal(r.Cells["Amount"].Value);
					detail.RemarksDetail = "";
					detail.ActionTypeId = 3;
					detail.HeatCode = Conversion.ToInt(CmbHeatCode.Value);
					detail.timeStart = Conversion.ToDateTime((object)InTime.Value);
					detail.timeOut = Conversion.ToDateTime((object)OutTime.Value);
					RemoveInPutDetaillist.Add(detail);
					dtdetailInput.Rows[r.RowIndex].Delete();
					r.Delete();
				}
				else
				{
					dtdetailInput.Rows[r.RowIndex].Delete();
					r.Delete();
				}
				if (dtdetailInput.Rows.Count == 0)
				{
					AddRowInInputGrid();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInput_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdInput.UpdateData();
			GridEXRow item = grdInput.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				if (e.Column.Key == "NetWeight" || e.Column.Key == "Rate" || e.Column.Key == "RateUomId" || e.Column.Key == "RateUom")
				{
					decimal NetWeight = Conversion.ToDecimal((object)item.Cells["NetWeight"].Value.ToString());
					decimal rate = Conversion.ToDecimal((object)item.Cells["Rate"].Value.ToString());
					decimal Ruom = Conversion.ToDecimal((object)item.Cells["RateEquivalent"].Value.ToString());
					decimal Amount = ((Ruom > 0m) ? (NetWeight / Ruom * rate) : 0m);
					item.Cells["Amount"].Value = Amount;
				}
				grdInput.UpdateData();
				Calculation();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInOverHeadGrid()
	{
		dtOverHead.Rows.Add(0, 0, 0, 0, 0, 0, "");
		grdOverHead.DataSource = dtOverHead;
	}

	private void grdOverHeadComboFill()
	{
		try
		{
			if (dtAccounts.Rows.Count > 0)
			{
				grdOverHead.RootTable.Columns["OverHeadAccountId"].ColumnType = (ColumnType)0;
				grdOverHead.RootTable.Columns["OverHeadAccountId"].LimitToList = true;
				grdOverHead.RootTable.Columns["OverHeadAccountId"].HasValueList = true;
				grdOverHead.RootTable.Columns["OverHeadAccountId"].EditType = (EditType)4;
				grdOverHead.RootTable.Columns["OverHeadAccountId"].ValueList.PopulateValueList((IEnumerable)dtAccounts.DefaultView, "Id", "AccountTitle");
				grdOverHead.RootTable.Columns["OverHeadAccountId"].Width = 200;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOverHeadGridSetting()
	{
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Expected O, but got Unknown
		try
		{
			grdOverHead.RootTable.Columns["Id"].Visible = false;
			grdOverHead.RootTable.Columns["PercentOfTotalInput"].Caption = "Percentage";
			grdOverHead.RootTable.Columns["OverHeadAccountId"].Caption = "AccountTitle";
			grdOverHead.RootTable.Columns["Id"].EditType = (EditType)0;
			grdOverHead.RootTable.Columns["Qty"].EditType = (EditType)1;
			grdOverHead.RootTable.Columns["Rate"].EditType = (EditType)1;
			grdOverHead.RootTable.Columns["PercentOfTotalInput"].EditType = (EditType)1;
			grdOverHead.RootTable.Columns["Amount"].EditType = (EditType)1;
			grdOverHead.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grdOverHead.RootTable.Columns)
			{
				item.HeaderStyle.TextAlignment = (TextAlignment)2;
				CommonServices.GridColumnSettings(item);
			}
			grdOverHead.RootTable.Columns["OverHeadAccountId"].Width = 200;
			grdOverHead.RootTable.Columns["PercentOfTotalInput"].Width = 90;
			grdOverHead.RootTable.Columns["Qty"].Width = 60;
			grdOverHead.RootTable.Columns["Rate"].Width = 65;
			grdOverHead.RootTable.Columns["Amount"].Width = 90;
			grdOverHead.RootTable.Columns["OhRemarks"].Width = 250;
			grdOverHeadComboFill();
			grdOverHead.RootTable.Columns.Add("Delete");
			grdOverHead.RootTable.Columns["Delete"].Key = "Delete";
			grdOverHead.RootTable.Columns["Delete"].Caption = "X";
			grdOverHead.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdOverHead.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdOverHead.RootTable.Columns["Delete"].ButtonText = "X";
			grdOverHead.RootTable.Columns["Delete"].Width = 25;
			grdOverHead.RootTable.Columns["Delete"].Position = 0;
			grdOverHead.RootTable.Columns.Add("Add");
			grdOverHead.RootTable.Columns["Add"].Key = "Add";
			grdOverHead.RootTable.Columns["Add"].Caption = "+";
			grdOverHead.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdOverHead.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdOverHead.RootTable.Columns["Add"].ButtonText = "+";
			grdOverHead.RootTable.Columns["Add"].Width = 25;
			grdOverHead.RootTable.Columns["Add"].Position = 1;
			grdOverHead.FrozenColumns = 2;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOverHead_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdOverHead.UpdateData();
			GridEXRow item = grdOverHead.CurrentRow;
			if ((e.Column.Key == "Qty" || e.Column.Key == "Rate") && item != null && (int)item.RowType == 0)
			{
				decimal Amount = default(decimal);
				decimal Iuom = default(decimal);
				if (!(item.Cells["Qty"].Value.ToString() != string.Empty) || !(item.Cells["Rate"].Value.ToString() != string.Empty))
				{
					throw new Exception("Qty And Rate Should be greater than zero...");
				}
				decimal num = Conversion.ToDecimal((object)item.Cells["Qty"].Value.ToString());
				Iuom = Conversion.ToDecimal((object)item.Cells["Rate"].Text.ToString());
				Amount = num * Iuom;
				item.Cells["Amount"].Value = Amount;
			}
			if (e.Column.Key == "PercentOfTotalInput" && item != null && (int)item.RowType == 0)
			{
				double TotalPercentage = 0.0;
				GridEXRow[] rows = grdOverHead.GetRows();
				foreach (GridEXRow r in rows)
				{
					TotalPercentage += Conversion.ToDouble(r.Cells["PercentOfTotalInput"].Value);
				}
				if (TotalPercentage > 100.0)
				{
					double Percent = Conversion.ToDouble(grdOverHead.CurrentRow.Cells["PercentOfTotalInput"].Value);
					double i = TotalPercentage - 100.0;
					grdOverHead.CurrentRow.Cells["PercentOfTotalInput"].Value = Percent - i;
					MessageBox.Show("TotalPercentage Can not be Greater than 100...");
					return;
				}
				if (item.Cells["PercentOfTotalInput"].Value.ToString() != string.Empty)
				{
					double num2 = Conversion.ToDouble((object)grdInput.GetTotal(grdInput.RootTable.Columns["Amount"], (AggregateFunction)2).ToString());
					double Percentage = Conversion.ToDouble((object)item.Cells["PercentOfTotalInput"].Value.ToString());
					double TotlaPercentage = num2 / 100.0 * Percentage;
					if (TotlaPercentage > 0.0)
					{
						item.Cells["Amount"].Value = Math.Round(TotlaPercentage, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
					}
					else
					{
						item.Cells["Amount"].Value = 0;
					}
					item.Cells["Qty"].Value = 0;
					item.Cells["Rate"].Value = 0;
				}
			}
			if (e.Column.Key == "Amount")
			{
				item.Cells["Qty"].Value = 0;
				item.Cells["Rate"].Value = 0;
			}
			grdScrap.UpdateData();
			Calculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOverHead_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdOverHead.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			if (e.Column.Key == "Delete")
			{
				dtOverHead.Rows[item.RowIndex].Delete();
				item.Delete();
				if (dtOverHead.Rows.Count == 0)
				{
					AddRowInOverHeadGrid();
				}
			}
			if (e.Column.Key == "Add")
			{
				AddRowInOverHeadGrid();
			}
			grdOverHead.UpdateData();
			Calculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInOutputGrid()
	{
		grdOutPut.DataSource = dtdetailOutput;
	}

	private void grdOutPutSettings()
	{
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Expected O, but got Unknown
		try
		{
			grdOutPut.RootTable.Columns["Id"].Visible = false;
			grdOutPut.RootTable.Columns["ItemId"].Visible = false;
			grdOutPut.RootTable.Columns["ModalId"].Visible = false;
			grdOutPut.RootTable.Columns["PackUomId"].Visible = false;
			grdOutPut.RootTable.Columns["ProcessStatusId"].Visible = false;
			grdOutPut.RootTable.Columns["PackEquivalent"].Visible = false;
			grdOutPut.RootTable.Columns["RateUomId"].Visible = false;
			grdOutPut.RootTable.Columns["RateEquivalent"].Visible = false;
			grdOutPut.RootTable.Columns["WorkOrderId"].Visible = false;
			grdOutPut.RootTable.Columns["workOrderItemDetailId"].Visible = false;
			grdOutPut.RootTable.Columns["WarehouseId"].Caption = "Ware House";
			grdOutPut.RootTable.Columns["PackUom"].Caption = "Uom";
			grdOutPut.RootTable.Columns["NetWeight"].Caption = "Qty";
			grdOutPut.RootTable.Columns["ProcessStatus"].Caption = "Production Stage";
			grdOutPut.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grdOutPut.RootTable.Columns)
			{
				GridEXColumn column = item;
				column.HeaderAlignment = (TextAlignment)2;
				CommonServices.GridColumnSettings(column, 2);
				if (column.Key == "Rate" || column.Key == "NetWeight")
				{
					column.EditType = (EditType)1;
				}
				else
				{
					column.EditType = (EditType)0;
				}
				column.FilterEditType = (FilterEditType)1;
			}
			GridComboBind(grdOutPut);
			grdOutPut.RootTable.Columns["WarehouseId"].EditType = (EditType)0;
			grdOutPut.RootTable.Columns["ModalDescription"].Width = Constants.InventoryConstants.OtherDescription;
			grdOutPut.RootTable.Columns["WarehouseId"].Width = Constants.InventoryConstants.WareHouseName;
			grdOutPut.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			grdOutPut.RootTable.Columns["PackUom"].Width = Constants.InventoryConstants.UomConstant;
			grdOutPut.RootTable.Columns["NetWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdOutPut.RootTable.Columns["BalQty"].Width = Constants.InventoryConstants.WeightConstant;
			grdOutPut.RootTable.Columns["Rate"].Width = Constants.InventoryConstants.ExRateConstant;
			grdOutPut.RootTable.Columns["RateUom"].Width = Constants.InventoryConstants.UomConstant;
			grdOutPut.RootTable.Columns["Amount"].Width = Constants.InventoryConstants.AmountConstant;
			grdOutPut.RootTable.Columns["WorkOrderNo"].Width = Constants.InventoryConstants.OtherDescription;
			grdOutPut.RootTable.Columns["WeightPerQty"].Width = Constants.InventoryConstants.WeightConstant;
			grdOutPut.RootTable.Columns["CastingWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdOutPut.RootTable.Columns.Add("Delete");
			grdOutPut.RootTable.Columns["Delete"].Key = "Delete";
			grdOutPut.RootTable.Columns["Delete"].Caption = "X";
			grdOutPut.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdOutPut.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdOutPut.RootTable.Columns["Delete"].ButtonText = "X";
			grdOutPut.RootTable.Columns["Delete"].Width = 20;
			grdOutPut.RootTable.Columns["Delete"].Position = 0;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOutPut_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdOutPut.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				if (e.Column.Key == "Delete")
				{
					DeleteOutputDetailRow(r);
				}
				if (e.Column.Key == "Add")
				{
					AddRowInOutputGrid();
				}
				Calculation();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeleteOutputDetailRow(GridEXRow r)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		try
		{
			if (r != null && (int)r.RowType == 0)
			{
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					ProductionOutPutDetail detail = new ProductionOutPutDetail();
					detail.ProductionOutPutDetailId = Conversion.ToInt(r.Cells["Id"].Value);
					detail.OutPutTypeId = 1;
					detail.SaleOrderId = 0;
					detail.WorkOrderId = Conversion.ToInt(r.Cells["WorkOrderId"].Value);
					detail.workOrderItemDetailId = Conversion.ToInt(r.Cells["workOrderItemDetailId"].Value);
					detail.WareHouseId = Conversion.ToInt(r.Cells["WarehouseId"].Value);
					detail.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					detail.JobLotId = 0;
					detail.ItemUomId = Conversion.ToInt(r.Cells["PackUomId"].Value);
					detail.ItemVariantId = Conversion.ToInt(r.Cells["ModalId"].Value);
					detail.ProcessStatusId = Conversion.ToInt(r.Cells["ProcessStatusId"].Value);
					detail.ItemQty = Conversion.ToDecimal(r.Cells["NetWeight"].Value);
					detail.NetWeight = Conversion.ToDecimal(r.Cells["NetWeight"].Value);
					detail.ItemRate = Conversion.ToDecimal(r.Cells["Rate"].Value);
					detail.RateUomId = Conversion.ToInt(r.Cells["RateUomId"].Value);
					detail.Amount = Conversion.ToDecimal(r.Cells["Amount"].Value);
					detail.RemarksDetail = "";
					detail.ActionTypeId = 3;
					RemoveOutPutDetaillist.Add(detail);
					dtdetailOutput.Rows[r.RowIndex].Delete();
					r.Delete();
				}
				else
				{
					dtdetailOutput.Rows[r.RowIndex].Delete();
					r.Delete();
				}
				if (dtdetailOutput.Rows.Count == 0)
				{
					AddRowInOutputGrid();
				}
			}
			((Control)(object)CmbCastingType).Enabled = !grdOutPut.GetRows().Any((GridEXRow rrr) => Convert.ToInt32(rrr.Cells["WorkOrderId"].Value) > 0);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOutPut_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdOutPut.UpdateData();
			GridEXRow item = grdOutPut.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				if (e.Column.Key == "NetWeight")
				{
					decimal NetWeight2 = Conversion.ToDecimal((object)item.Cells["NetWeight"].Value.ToString());
					decimal WeightPerQty = Conversion.ToDecimal(item.Cells["WeightPerQty"].Value);
					item.Cells["CastingWeight"].Value = (NetWeight2 * WeightPerQty).ToString("#,##.##");
				}
				if (e.Column.Key == "NetWeight" || e.Column.Key == "Rate" || e.Column.Key == "RateUomId" || e.Column.Key == "RateUom")
				{
					decimal NetWeight = Conversion.ToDecimal((object)item.Cells["NetWeight"].Value.ToString());
					decimal rate = Conversion.ToDecimal((object)item.Cells["Rate"].Value.ToString());
					decimal Ruom = Conversion.ToDecimal((object)item.Cells["RateEquivalent"].Value.ToString());
					decimal Amount = ((Ruom > 0m) ? (NetWeight / Ruom * rate) : 0m);
					item.Cells["Amount"].Value = Amount;
				}
				grdOutPut.UpdateData();
				Calculation();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInScrapGrid()
	{
		dtScrap.Rows.Add(0, 0, 0, "", 0, "", 0, 0, 0, 0, "", 0, 0);
		grdScrap.DataSource = dtScrap;
	}

	private void grdScrapSettings()
	{
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Expected O, but got Unknown
		try
		{
			grdScrap.RootTable.Columns["Id"].Visible = false;
			grdScrap.RootTable.Columns["ItemId"].Visible = false;
			grdScrap.RootTable.Columns["PackUomId"].Visible = false;
			grdScrap.RootTable.Columns["PackEquivalent"].Visible = false;
			grdScrap.RootTable.Columns["RateUomId"].Visible = false;
			grdScrap.RootTable.Columns["RateEquivalent"].Visible = false;
			grdScrap.RootTable.Columns["WarehouseId"].Caption = "Ware House";
			grdScrap.RootTable.Columns["PackUom"].Caption = "Uom";
			grdScrap.RootTable.Columns["NetWeight"].Caption = "Qty";
			grdScrap.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grdScrap.RootTable.Columns)
			{
				GridEXColumn column = item;
				column.HeaderAlignment = (TextAlignment)2;
				CommonServices.GridColumnSettings(column, 2);
				if (column.Key == "Rate" || column.Key == "NetWeight")
				{
					column.EditType = (EditType)1;
				}
				else
				{
					column.EditType = (EditType)0;
				}
				column.FilterEditType = (FilterEditType)1;
			}
			GridComboBind(grdScrap);
			grdScrap.RootTable.Columns["WarehouseId"].Width = Constants.InventoryConstants.WareHouseName;
			grdScrap.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			grdScrap.RootTable.Columns["PackUom"].Width = Constants.InventoryConstants.UomConstant;
			grdScrap.RootTable.Columns["NetWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdScrap.RootTable.Columns["Rate"].Width = Constants.InventoryConstants.ExRateConstant;
			grdScrap.RootTable.Columns["RateUom"].Width = Constants.InventoryConstants.UomConstant;
			grdScrap.RootTable.Columns["Amount"].Width = Constants.InventoryConstants.AmountConstant;
			grdScrap.RootTable.Columns.Add("Delete");
			grdScrap.RootTable.Columns["Delete"].Key = "Delete";
			grdScrap.RootTable.Columns["Delete"].Caption = "X";
			grdScrap.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdScrap.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdScrap.RootTable.Columns["Delete"].ButtonText = "X";
			grdScrap.RootTable.Columns["Delete"].Width = 20;
			grdScrap.RootTable.Columns["Delete"].Position = 0;
			grdScrap.RootTable.Columns.Add("Add");
			grdScrap.RootTable.Columns["Add"].Key = "Add";
			grdScrap.RootTable.Columns["Add"].Caption = "+";
			grdScrap.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdScrap.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdScrap.RootTable.Columns["Add"].ButtonText = "+";
			grdScrap.RootTable.Columns["Add"].Width = 20;
			grdScrap.RootTable.Columns["Add"].Position = 1;
			grdScrap.FrozenColumns = 2;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdScrap_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdScrap.UpdateData();
			GridEXRow item = grdScrap.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				if (e.Column.Key == "NetWeight" || e.Column.Key == "Rate" || e.Column.Key == "RateUomId" || e.Column.Key == "RateUom")
				{
					decimal NetWeight = Conversion.ToDecimal((object)item.Cells["NetWeight"].Value.ToString());
					decimal rate = Conversion.ToDecimal((object)item.Cells["Rate"].Value.ToString());
					decimal Ruom = Conversion.ToDecimal((object)item.Cells["RateEquivalent"].Value.ToString());
					decimal Amount = ((Ruom > 0m) ? (NetWeight / Ruom * rate) : 0m);
					item.Cells["Amount"].Value = Amount;
				}
				grdScrap.UpdateData();
				Calculation();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdScrap_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdScrap.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				if (e.Column.Key == "Delete")
				{
					DeleteScrapDetailRow(r);
				}
				if (e.Column.Key == "Add")
				{
					AddRowInScrapGrid();
				}
				Calculation();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeleteScrapDetailRow(GridEXRow r)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		try
		{
			if (r != null && (int)r.RowType == 0)
			{
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					ProductionOutPutDetail detail = new ProductionOutPutDetail();
					detail.ProductionOutPutDetailId = Conversion.ToInt(r.Cells["Id"].Value);
					detail.OutPutTypeId = 2;
					detail.WorkOrderId = 0;
					detail.SaleOrderId = 0;
					detail.WareHouseId = Conversion.ToInt(r.Cells["WarehouseId"].Value);
					detail.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					detail.JobLotId = 0;
					detail.ItemUomId = Conversion.ToInt(r.Cells["PackUomId"].Value);
					detail.ItemVariantId = 0;
					detail.ProcessStatusId = 0;
					detail.ItemQty = Conversion.ToDecimal(r.Cells["NetWeight"].Value);
					detail.NetWeight = Conversion.ToDecimal(r.Cells["NetWeight"].Value);
					detail.ItemRate = Conversion.ToDecimal(r.Cells["Rate"].Value);
					detail.RateUomId = Conversion.ToInt((object)r.Cells["RateUomId"].Text);
					detail.Amount = Conversion.ToDecimal(r.Cells["Amount"].Value);
					detail.RemarksDetail = "";
					detail.ActionTypeId = 3;
					RemoveOutPutDetaillist.Add(detail);
					dtScrap.Rows[r.RowIndex].Delete();
					r.Delete();
				}
				else
				{
					dtScrap.Rows[r.RowIndex].Delete();
					r.Delete();
				}
				if (dtScrap.Rows.Count == 0)
				{
					AddRowInScrapGrid();
				}
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
		//IL_038e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0398: Expected O, but got Unknown
		//IL_0968: Unknown result type (might be due to invalid IL or missing references)
		//IL_096f: Expected O, but got Unknown
		//IL_0cee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cf5: Expected O, but got Unknown
		//IL_10cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_10d3: Expected O, but got Unknown
		try
		{
			if (!FormValidation())
			{
				return;
			}
			if (grdInput.RowCount == 0)
			{
				MessageBox.Show("Grid Record Not Found,Input Grid Should have a row");
				return;
			}
			ProductionHeader obj = new ProductionHeader();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				obj.ProductionHeaderId = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			if (ChkSettlement.Checked)
			{
				double sumoftotalOutPut = Conversion.ToDouble((object)grdOutPut.GetTotal(grdOutPut.RootTable.Columns["Amount"], (AggregateFunction)2).ToString());
				double TotalOutPutAutoRatedAmount = (from GridEXRow row in grdSummary.GetDataRows()
					where Conversion.ToInt(row.Cells["Id"].Value) == 5
					select (double)row.Cells["Amount"].Value).Sum();
				if (Math.Round(TotalOutPutAutoRatedAmount, 2) != Math.Round(sumoftotalOutPut, 2))
				{
					throw new Exception($"Net Vale {TotalOutPutAutoRatedAmount} In Header and Total OutPut Amount {sumoftotalOutPut} are not equal");
				}
			}
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.DocumentTypeId = 1670;
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			obj.ApprovedDate = DateTime.Now;
			obj.IsApproved = false;
			obj.EntryUserId = UserAccount.ID;
			obj.ModifyUserId = UserAccount.ID;
			obj.ApprovedUserId = UserAccount.ID;
			obj.DocDate = DocDate.Value;
			obj.DocNo = Conversion.ToInt((object)txtdocno.Text.Trim());
			obj.ProductionNo = txtProductionNo.Text.Trim();
			obj.JobLotId = Conversion.ToInt(CmbJobLot.Value);
			obj.batchCode = ((Control)(object)CmbJobLot).Text.Trim();
			obj.heatCode = Conversion.ToInt(CmbHeatCode.Value);
			obj.inTime = Conversion.ToDateTime((object)InTime.Value);
			obj.outTime = Conversion.ToDateTime((object)OutTime.Value);
			obj.CastingTypeId = Conversion.ToInt(CmbCastingType.Value);
			obj.BomHeaderId = Conversion.ToInt(CmbBillOfMaterial.Value);
			obj.RemarksHeader = txtremarks.Text.Trim();
			obj.IsSettled = (ChkSettlement.Checked ? 1 : 0);
			obj.ProductionOutPutDetailslist = new List<ProductionOutPutDetail>();
			obj.ProductionInputDetailslist = new List<ProductionInputDetail>();
			obj.ProductionOverHeadsDetailList = new List<ProductionOverHeadsDetail>();
			if (RemoveOutPutDetaillist.Count > 0 && RecId > 0)
			{
				for (int i = 0; i < RemoveOutPutDetaillist.Count; i++)
				{
					obj.ProductionOutPutDetailslist.Add(RemoveOutPutDetaillist[i]);
				}
			}
			GridEXRow[] rows = grdOutPut.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				ProductionOutPutDetail detail = new ProductionOutPutDetail();
				detail.ProductionOutPutDetailId = ((RecId != 0) ? Conversion.ToInt(r2.Cells["Id"].Value) : 0);
				detail.ActionTypeId = ((detail.ProductionOutPutDetailId <= 0) ? 1 : 2);
				detail.OutPutTypeId = 1;
				detail.SaleOrderId = 0;
				detail.WorkOrderId = Conversion.ToInt(r2.Cells["WorkOrderId"].Value);
				detail.workOrderItemDetailId = Conversion.ToInt(r2.Cells["workOrderItemDetailId"].Value);
				detail.WareHouseId = Conversion.ToInt(r2.Cells["WarehouseId"].Value);
				detail.ItemId = Conversion.ToInt(r2.Cells["ItemId"].Value);
				detail.JobLotId = 0;
				detail.ItemUomId = Conversion.ToInt(r2.Cells["PackUomId"].Value);
				detail.ItemVariantId = Conversion.ToInt(r2.Cells["ModalId"].Value);
				detail.ProcessStatusId = Conversion.ToInt(r2.Cells["ProcessStatusId"].Value);
				detail.ItemQty = Conversion.ToDecimal(r2.Cells["NetWeight"].Value);
				detail.NetWeight = Conversion.ToDecimal(r2.Cells["NetWeight"].Value);
				detail.ItemRate = Conversion.ToDecimal(r2.Cells["Rate"].Value);
				detail.RateUomId = Conversion.ToInt(r2.Cells["RateUomId"].Value);
				detail.Amount = Conversion.ToDecimal(r2.Cells["Amount"].Value);
				detail.RemarksDetail = "";
				if (detail.WorkOrderId == 0)
				{
					throw new Exception("WorkOrder Required In Row #" + (r2.RowIndex + 1) + " In OutPut Grid");
				}
				if (detail.WareHouseId == 0)
				{
					throw new Exception("WareHouse Required In Row #" + (r2.RowIndex + 1) + " In OutPut Grid");
				}
				if (detail.ItemId == 0)
				{
					throw new Exception("Item Required In Row #" + (r2.RowIndex + 1) + " In OutPut Grid");
				}
				if (detail.ItemUomId == 0)
				{
					throw new Exception("Uom Required In Row #" + (r2.RowIndex + 1) + " In OutPut Grid");
				}
				if (detail.ProcessStatusId == 0)
				{
					throw new Exception("Production Stage Required In Row #" + (r2.RowIndex + 1) + " In OutPut Grid");
				}
				if (detail.NetWeight == 0m)
				{
					throw new Exception("Qty Required In Row #" + (r2.RowIndex + 1) + " In OutPut Grid");
				}
				if (detail.ItemRate == 0m)
				{
					throw new Exception("Rate Required In Row #" + (r2.RowIndex + 1) + " In OutPut Grid");
				}
				if (detail.RateUomId == 0)
				{
					throw new Exception("RateUom Required In Row #" + (r2.RowIndex + 1) + " In OutPut Grid");
				}
				if (detail.Amount == 0m)
				{
					throw new Exception("Amount Required In Row #" + (r2.RowIndex + 1) + " In OutPut Grid");
				}
				double BalQtyByWorkOrder = (from GridEXRow row in grdOutPut.GetDataRows()
					where Conversion.ToInt(row.Cells["WorkOrderId"].Value) > 0 && Conversion.ToInt(row.Cells["Id"].Value) != detail.ProductionOutPutDetailId && Conversion.ToInt(row.Cells["WorkOrderId"].Value) == detail.WorkOrderId
					select (double)row.Cells["NetWeight"].Value).Sum();
				decimal ValuetoVarify = Conversion.ToDecimal(r2.Cells["BalQty"].Value) - Conversion.ToDecimal((object)BalQtyByWorkOrder);
				if (detail.WorkOrderId > 0 && detail.NetWeight > ValuetoVarify)
				{
					throw new Exception($"Qty '{detail.NetWeight}' Can not Greater than Bal Qty '{ValuetoVarify}' of Selected WorkOder " + "against CastingType '" + Conversion.ToString((object)((Control)(object)CmbCastingType).Text) + "' In Row # " + (r2.RowIndex + 1) + " In OutPut Grid");
				}
				obj.ProductionOutPutDetailslist.Add(detail);
			}
			rows = grdScrap.GetRows();
			foreach (GridEXRow r4 in rows)
			{
				if (Conversion.ToInt(r4.Cells["ItemId"].Value) > 0)
				{
					ProductionOutPutDetail detail3 = new ProductionOutPutDetail();
					detail3.ProductionOutPutDetailId = ((RecId != 0) ? Conversion.ToInt(r4.Cells["Id"].Value) : 0);
					detail3.ActionTypeId = ((detail3.ProductionOutPutDetailId <= 0) ? 1 : 2);
					detail3.OutPutTypeId = 2;
					detail3.WorkOrderId = 0;
					detail3.SaleOrderId = 0;
					detail3.WareHouseId = Conversion.ToInt(r4.Cells["WarehouseId"].Value);
					detail3.ItemId = Conversion.ToInt(r4.Cells["ItemId"].Value);
					detail3.JobLotId = 0;
					detail3.ItemUomId = Conversion.ToInt(r4.Cells["PackUomId"].Value);
					detail3.ItemVariantId = 0;
					detail3.ProcessStatusId = 0;
					detail3.ItemQty = Conversion.ToDecimal(r4.Cells["NetWeight"].Value);
					detail3.NetWeight = Conversion.ToDecimal(r4.Cells["NetWeight"].Value);
					detail3.ItemRate = Conversion.ToDecimal(r4.Cells["Rate"].Value);
					detail3.RateUomId = Conversion.ToInt(r4.Cells["RateUomId"].Value);
					detail3.Amount = Conversion.ToDecimal(r4.Cells["Amount"].Value);
					detail3.RemarksDetail = "";
					if (detail3.WareHouseId == 0)
					{
						throw new Exception("WareHouse Required In Row #" + (r4.RowIndex + 1) + " In Scrap Grid");
					}
					if (detail3.ItemId == 0)
					{
						throw new Exception("Item Required In Row #" + (r4.RowIndex + 1) + " In Scrap Grid");
					}
					if (detail3.ItemUomId == 0)
					{
						throw new Exception("Uom Required In Row #" + (r4.RowIndex + 1) + " In Scrap Grid");
					}
					if (detail3.NetWeight == 0m)
					{
						throw new Exception("Weight Required In Row #" + (r4.RowIndex + 1) + " In Scrap Grid");
					}
					if (detail3.ItemRate == 0m)
					{
						throw new Exception("Rate Required In Row #" + (r4.RowIndex + 1) + " In Scrap Grid");
					}
					if (detail3.RateUomId == 0)
					{
						throw new Exception("RateUom Required In Row #" + (r4.RowIndex + 1) + " In Scrap Grid");
					}
					if (detail3.Amount == 0m)
					{
						throw new Exception("Amount Required In Row #" + (r4.RowIndex + 1) + " In Scrap Grid");
					}
					obj.ProductionOutPutDetailslist.Add(detail3);
				}
			}
			if (RemoveInPutDetaillist.Count > 0 && RecId > 0)
			{
				for (int j = 0; j < RemoveInPutDetaillist.Count; j++)
				{
					obj.ProductionInputDetailslist.Add(RemoveInPutDetaillist[j]);
				}
			}
			rows = grdInput.GetRows();
			foreach (GridEXRow r3 in rows)
			{
				ProductionInputDetail detail2 = new ProductionInputDetail();
				detail2.productionInputDetailId = ((RecId != 0) ? Conversion.ToInt(r3.Cells["Id"].Value) : 0);
				detail2.ActionTypeId = ((detail2.productionInputDetailId <= 0) ? 1 : 2);
				detail2.BomHeaderId = Conversion.ToInt(r3.Cells["BomHeaderId"].Value);
				detail2.BomInputDetailId = Conversion.ToInt(r3.Cells["BomInputDetailId"].Value);
				detail2.WareHouseId = Conversion.ToInt(r3.Cells["WarehouseId"].Value);
				detail2.ItemId = Conversion.ToInt(r3.Cells["ItemId"].Value);
				detail2.ItemUomId = Conversion.ToInt(r3.Cells["PackUomId"].Value);
				detail2.Qty = Conversion.ToDecimal(r3.Cells["NetWeight"].Value);
				detail2.Weight = Conversion.ToDecimal(r3.Cells["NetWeight"].Value);
				detail2.ItemRate = Conversion.ToDecimal(r3.Cells["Rate"].Value);
				detail2.RateUomId = Conversion.ToInt(r3.Cells["RateUomId"].Value);
				detail2.Amount = Conversion.ToDecimal(r3.Cells["Amount"].Value);
				detail2.HeatCode = Conversion.ToInt(CmbHeatCode.Value);
				detail2.timeStart = Conversion.ToDateTime((object)InTime.Value);
				detail2.timeOut = Conversion.ToDateTime((object)OutTime.Value);
				detail2.RemarksDetail = "";
				if (detail2.WareHouseId == 0)
				{
					throw new Exception("WareHouse Required In Row #" + (r3.RowIndex + 1) + " In Input Grid");
				}
				if (detail2.ItemId == 0)
				{
					throw new Exception("Item Required In Row #" + (r3.RowIndex + 1) + " In Input Grid");
				}
				if (detail2.ItemUomId == 0)
				{
					throw new Exception("Uom Required In Row #" + (r3.RowIndex + 1) + " In Input Grid");
				}
				if (detail2.Weight == 0m)
				{
					throw new Exception("Weight Required In Row #" + (r3.RowIndex + 1) + " In Input Grid");
				}
				if (detail2.ItemRate == 0m)
				{
					throw new Exception("Rate Required In Row #" + (r3.RowIndex + 1) + " In Input Grid");
				}
				if (detail2.RateUomId == 0)
				{
					throw new Exception("RateUom Required In Row #" + (r3.RowIndex + 1) + " In Input Grid");
				}
				if (detail2.Amount == 0m)
				{
					throw new Exception("Amount Required In Row #" + (r3.RowIndex + 1) + " In Input Grid");
				}
				obj.ProductionInputDetailslist.Add(detail2);
			}
			if (obj.ProductionInputDetailslist.Count == 0)
			{
				MessageBox.Show("Grid Record Not Found,Input Grid Should have a row");
				return;
			}
			rows = grdOverHead.GetDataRows();
			foreach (GridEXRow r in rows)
			{
				if (Conversion.ToInt(r.Cells["OverHeadAccountId"].Value) > 0)
				{
					ProductionOverHeadsDetail d = new ProductionOverHeadsDetail();
					d.ProductionOverHeadsDetailId = Conversion.ToInt(r.Cells["Id"].Value);
					d.OverHeadAccountId = Conversion.ToInt(r.Cells["OverHeadAccountId"].Value);
					d.OverHeadAccountTitle = Conversion.ToString((object)r.Cells["OverHeadAccountId"].Text);
					d.PercentOfTotalInput = Conversion.ToDecimal((object)r.Cells["PercentOfTotalInput"].Text);
					d.Qty = Conversion.ToDecimal((object)r.Cells["Qty"].Text);
					d.Rate = Conversion.ToDecimal((object)r.Cells["Rate"].Text);
					d.Amount = Conversion.ToDecimal((object)r.Cells["Amount"].Text);
					d.Remarks = Conversion.ToString((object)r.Cells["OhRemarks"].Text);
					if (d.OverHeadAccountId == 0)
					{
						MessageBox.Show("OverHeadAccount Field Required in Grid Over Head Row No: " + (r.RowIndex + 1));
						return;
					}
					if (d.Amount == 0m)
					{
						MessageBox.Show("Amount Field Required in Grid Over Head Row No: " + (r.RowIndex + 1));
						return;
					}
					obj.ProductionOverHeadsDetailList.Add(d);
				}
			}
			obj.AttachmentsList = new List<DMSAttachments>();
			obj.DeleteAttachmentsList = new List<DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<ProductionHeader>(obj, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
			obj.AttachmentsValues = AttachmentsValues;
			obj.CustomAttachmentsValues = CustomAttachmentsValues;
			int success = ProductionHeader.Save(obj);
			if (RecId > 0)
			{
				MessageBox.Show("Record Update Successfully [" + obj.DocNo + "]");
				FormHelper.DeleteAllAttachmentsFromDb(AT, RecId, 1670, base.Name);
			}
			else
			{
				MessageBox.Show("Record Save Successfully [" + obj.DocNo + "]");
			}
			reset();
			if (ChkBox.Checked)
			{
				CommonServices.ProductionSlipEngr_1670(success);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
			if (RecId == 0)
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
		try
		{
			reset();
			RecId = ID;
			ProductionHeader obj = ProductionHeader.ReadById(RecId);
			if (obj == null)
			{
				return;
			}
			btnSave.Visible = false;
			BtnSaveAs.Visible = false;
			btnUpdate.Visible = true;
			BtnDelete.Visible = true;
			tabControl1.SelectedIndex = 0;
			txtdocno.Text = obj.DocNo.ToString();
			DocDate.Value = obj.DocDate;
			txtProductionNo.Text = obj.ProductionNo;
			CmbJobLot.Value = obj.JobLotId;
			CmbHeatCode.Value = obj.heatCode;
			InTime.Value = obj.inTime;
			OutTime.Value = obj.outTime;
			CmbCastingType.Value = obj.CastingTypeId;
			GetAllBomHeader();
			CmbCastingType_Leave(null, null);
			CmbBillOfMaterial.Value = obj.BomHeaderId;
			txtremarks.Text = Conversion.ToString((object)obj.RemarksHeader);
			AttachmentsValues = obj.AttachmentsValues;
			CustomAttachmentsValues = obj.CustomAttachmentsValues;
			dtdetailOutput.Rows.Clear();
			dtScrap.Rows.Clear();
			for (int k = 0; k < obj.ProductionOutPutDetailslist.Count; k++)
			{
				if (obj.ProductionOutPutDetailslist[k].OutPutTypeId == 1)
				{
					dtdetailOutput.Rows.Add(obj.ProductionOutPutDetailslist[k].ProductionOutPutDetailId, obj.ProductionOutPutDetailslist[k].WorkOrderId, obj.ProductionOutPutDetailslist[k].workOrderItemDetailId, obj.ProductionOutPutDetailslist[k].WorkOrderNo, obj.ProductionOutPutDetailslist[k].WareHouseId, obj.ProductionOutPutDetailslist[k].ItemId, obj.ProductionOutPutDetailslist[k].ItemName, obj.ProductionOutPutDetailslist[k].ItemUomId, obj.ProductionOutPutDetailslist[k].PackUom, obj.ProductionOutPutDetailslist[k].PackEquivalent, obj.ProductionOutPutDetailslist[k].ItemVariantId, obj.ProductionOutPutDetailslist[k].VarientDescription, obj.ProductionOutPutDetailslist[k].ProcessStatusId, obj.ProductionOutPutDetailslist[k].ProcessStatus, obj.ProductionOutPutDetailslist[k].NetWeight, obj.ProductionOutPutDetailslist[k].WorkOrderBalQty, obj.ProductionOutPutDetailslist[k].ItemWeight, obj.ProductionOutPutDetailslist[k].NetWeight * Conversion.ToDecimal((object)obj.ProductionOutPutDetailslist[k].ItemWeight), obj.ProductionOutPutDetailslist[k].ItemRate, obj.ProductionOutPutDetailslist[k].RateUomId, obj.ProductionOutPutDetailslist[k].RateUom, obj.ProductionOutPutDetailslist[k].RateEquivalent, obj.ProductionOutPutDetailslist[k].Amount);
				}
				else
				{
					dtScrap.Rows.Add(obj.ProductionOutPutDetailslist[k].ProductionOutPutDetailId, obj.ProductionOutPutDetailslist[k].WareHouseId, obj.ProductionOutPutDetailslist[k].ItemId, obj.ProductionOutPutDetailslist[k].ItemName, obj.ProductionOutPutDetailslist[k].ItemUomId, obj.ProductionOutPutDetailslist[k].PackUom, obj.ProductionOutPutDetailslist[k].PackEquivalent, obj.ProductionOutPutDetailslist[k].NetWeight, obj.ProductionOutPutDetailslist[k].ItemRate, obj.ProductionOutPutDetailslist[k].RateUomId, obj.ProductionOutPutDetailslist[k].RateUom, obj.ProductionOutPutDetailslist[k].RateEquivalent, obj.ProductionOutPutDetailslist[k].Amount);
				}
			}
			if (dtdetailOutput.Rows.Count == 0)
			{
				AddRowInOutputGrid();
			}
			grdOutPut.DataSource = dtdetailOutput;
			grdOutPut.RetrieveStructure();
			grdOutPutSettings();
			if (dtScrap.Rows.Count == 0)
			{
				AddRowInScrapGrid();
			}
			grdScrap.DataSource = dtScrap;
			grdScrap.RetrieveStructure();
			grdScrapSettings();
			dtdetailInput.Rows.Clear();
			for (int j = 0; j < obj.ProductionInputDetailslist.Count; j++)
			{
				dtdetailInput.Rows.Add(obj.ProductionInputDetailslist[j].productionInputDetailId, obj.ProductionInputDetailslist[j].BomHeaderId, obj.ProductionInputDetailslist[j].BomInputDetailId, obj.ProductionInputDetailslist[j].WareHouseId, obj.ProductionInputDetailslist[j].ItemId, obj.ProductionInputDetailslist[j].ItemName, obj.ProductionInputDetailslist[j].ItemUomId, obj.ProductionInputDetailslist[j].PackUom, obj.ProductionInputDetailslist[j].PackEquivalent, obj.ProductionInputDetailslist[j].Weight, obj.ProductionInputDetailslist[j].ItemRate, obj.ProductionInputDetailslist[j].RateUomId, obj.ProductionInputDetailslist[j].RateUom, obj.ProductionInputDetailslist[j].RateEquivalent, obj.ProductionInputDetailslist[j].Amount);
			}
			if (dtdetailInput.Rows.Count == 0)
			{
				AddRowInInputGrid();
			}
			grdInput.DataSource = dtdetailInput;
			grdInput.RetrieveStructure();
			grdInputSettings();
			dtOverHead.Rows.Clear();
			for (int i = 0; i < obj.ProductionOverHeadsDetailList.Count; i++)
			{
				dtOverHead.Rows.Add(obj.ProductionOverHeadsDetailList[i].ProductionOverHeadsDetailId, obj.ProductionOverHeadsDetailList[i].OverHeadAccountId, obj.ProductionOverHeadsDetailList[i].PercentOfTotalInput, obj.ProductionOverHeadsDetailList[i].Qty, obj.ProductionOverHeadsDetailList[i].Rate, obj.ProductionOverHeadsDetailList[i].Amount, obj.ProductionOverHeadsDetailList[i].Remarks);
			}
			if (dtOverHead.Rows.Count == 0)
			{
				AddRowInOverHeadGrid();
			}
			grdOverHead.DataSource = dtOverHead;
			grdOverHead.RetrieveStructure();
			grdOverHeadGridSetting();
			FormHelper.LoadAttachmentsForObject(AT, RecId, base.Name);
			Calculation();
			DocDate.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnDelete_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					ProductionHeader.DeleteByID(UserAccount.OrganizationId, UserAccount.CompanyId, 1670, UserAccount.ID, RecId);
					MessageBox.Show("Delete Record Successfully");
					reset();
				}
			}
			else
			{
				MessageBox.Show("No record found to Delete");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnSaveAs_Click(object sender, EventArgs e)
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

	private void reset()
	{
		try
		{
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			RemoveOutPutDetaillist.Clear();
			RemoveInPutDetaillist.Clear();
			RecId = 0;
			AttachmentsValues = "";
			CustomAttachmentsValues = "";
			txtProductionNo.Text = string.Empty;
			txtremarks.Text = string.Empty;
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			BtnSaveAs.Visible = false;
			BtnDelete.Visible = false;
			grdOutPut.DataSource = null;
			dtdetailOutput.Clear();
			AddRowInOutputGrid();
			grdOutPut.RetrieveStructure();
			grdOutPutSettings();
			grdInput.DataSource = null;
			dtdetailInput.Clear();
			AddRowInInputGrid();
			grdInput.RetrieveStructure();
			grdInputSettings();
			grdSummary.DataSource = null;
			labelGainLossCaption.Text = "Gain/Loss";
			labelGainLossPercent.Text = "%";
			labelGainLossCaption.ForeColor = Color.Black;
			labelGainLossPercent.ForeColor = Color.Black;
			grdScrap.DataSource = null;
			dtScrap.Clear();
			AddRowInScrapGrid();
			grdScrap.RetrieveStructure();
			grdScrapSettings();
			grdOverHead.DataSource = null;
			dtOverHead.Clear();
			AddRowInOverHeadGrid();
			grdOverHead.RetrieveStructure();
			grdOverHeadGridSetting();
			((Control)(object)CmbCastingType).Text = string.Empty;
			((Control)(object)CmbBillOfMaterial).Text = string.Empty;
			((UltraGridBase)CmbBillOfMaterial).DataSource = null;
			txtTotalInputs.Text = "";
			txtOverHead.Text = "";
			txtLessTotalScrap.Text = "";
			txtExpectedTotalOutputValue.Text = "";
			txtActualtotalOutPutValue.Text = "";
			DocDate.Focus();
			UpdateDocumentNoUI(GenerateCode());
			((Control)(object)CmbCastingType).Enabled = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		reset();
	}

	private void BtnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			ConfigrationsAllocation obj = clsGlobalVariables.configrationsAllocation.FirstOrDefault((ConfigrationsAllocation r) => r.ConfigDescription == "DefaultProductionInputItemCustomGroupId");
			DefaultProductionInputItemCustomGroupId = Conversion.ToInt((object)((obj != null) ? obj.ConfigKey : null));
			ConfigrationsAllocation obj2 = clsGlobalVariables.configrationsAllocation.FirstOrDefault((ConfigrationsAllocation r) => r.ConfigDescription == "DefaultProductionScrapItemCustomGroupId");
			DefaultProductionScrapItemCustomGroupId = Conversion.ToInt((object)((obj2 != null) ? obj2.ConfigKey : null));
			ConfigrationsAllocation obj3 = clsGlobalVariables.configrationsAllocation.FirstOrDefault((ConfigrationsAllocation r) => r.ConfigDescription == "DefaultProductionOverHeadCustomGroupId");
			DefaultProductionOverHeadCustomGroupId = Conversion.ToInt((object)((obj3 != null) ? obj3.ConfigKey : null));
			ConfigrationsAllocation obj4 = clsGlobalVariables.configrationsAllocation.FirstOrDefault((ConfigrationsAllocation r) => r.ConfigDescription == "DefaultCastingWarehouse");
			DefaultCastingWarehouseId = Conversion.ToInt((object)((obj4 != null) ? obj4.ConfigKey : null));
			ConfigrationsAllocation obj5 = clsGlobalVariables.configrationsAllocation.FirstOrDefault((ConfigrationsAllocation r) => r.ConfigDescription == "DefaultDaysToLessFromHistoryFromDate");
			DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)((obj5 != null) ? obj5.ConfigKey : null));
			ConfigrationsAllocation obj6 = clsGlobalVariables.configrationsAllocation.FirstOrDefault((ConfigrationsAllocation r) => r.ConfigDescription == "DefaultProductionStageId");
			DefaultProductionStageId = Conversion.ToInt((object)((obj6 != null) ? obj6.ConfigKey : null));
			LookUps obj7 = clsGlobalVariables.GetAllLookUps_Engr.FirstOrDefault((LookUps r) => r.Id == DefaultProductionStageId);
			DefaultProductionStageName = Conversion.ToString((object)((obj7 != null) ? obj7.LookupName : null));
			List<string> allServices = new List<string> { "AccountsWithCustomGroupId", "ItemsWithCustomGroupId", "LookUpsEngr" };
			DatatableHelper.GlobalServicesDbCall(UserAccount, allServices);
			BindItemInputScrap();
			BindOverHeadAccounts();
			BindCastingType();
			GetAllBomHeader();
			CmbCastingType_Leave(null, null);
			dtitemOutput.Rows.Clear();
			BindItemForOutPut(GetItemDataForOutPut());
			BindHeatCode(GetHeatCode());
			Joblotfill(BindJobLot());
			GetWarehouses();
			dtAllUom.Rows.Clear();
			dtAllUom = UomGetAll();
			GridComboBind(grdInput);
			GridComboBind(grdOutPut);
			GridComboBind(grdScrap);
			grdOverHeadComboFill();
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

	private void GRNDirectAgainstOrder_FormClosing(object sender, FormClosingEventArgs e)
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

	public void Calculation()
	{
		try
		{
			double TotalInputQty = 0.0;
			double TotalInputAmount = 0.0;
			double TotalOHQty = 0.0;
			double TotalOHAmount = 0.0;
			double TotalScrapQty = 0.0;
			double TotalScrapAmount = 0.0;
			double TotalOutputQty = 0.0;
			double TotalOutputAmount = 0.0;
			if (grdOutPut.GetRows().Count() > 0)
			{
				TotalOutputQty = Convert.ToDouble(grdOutPut.GetTotal(grdOutPut.RootTable.Columns["NetWeight"], (AggregateFunction)2));
				TotalOutputAmount = Convert.ToDouble(grdOutPut.GetTotal(grdOutPut.RootTable.Columns["Amount"], (AggregateFunction)2));
			}
			if (grdInput.GetRows().Count() > 0)
			{
				TotalInputQty = Convert.ToDouble(grdInput.GetTotal(grdInput.RootTable.Columns["NetWeight"], (AggregateFunction)2));
				TotalInputAmount = Convert.ToDouble(grdInput.GetTotal(grdInput.RootTable.Columns["Amount"], (AggregateFunction)2));
			}
			if (grdOverHead.GetRows().Count() > 0)
			{
				TotalOHQty = Convert.ToDouble(grdOverHead.GetTotal(grdOverHead.RootTable.Columns["Qty"], (AggregateFunction)2));
				TotalOHAmount = Convert.ToDouble(grdOverHead.GetTotal(grdOverHead.RootTable.Columns["Amount"], (AggregateFunction)2));
			}
			if (grdScrap.GetRows().Count() > 0)
			{
				TotalScrapQty = Convert.ToDouble(grdScrap.GetTotal(grdScrap.RootTable.Columns["NetWeight"], (AggregateFunction)2));
				TotalScrapAmount = Convert.ToDouble(grdScrap.GetTotal(grdScrap.RootTable.Columns["Amount"], (AggregateFunction)2));
			}
			double ScrapPercentage = ((TotalInputAmount > 0.0) ? (TotalScrapAmount / TotalInputAmount * 100.0) : 0.0);
			double OutputPercentage = ((TotalInputAmount > 0.0) ? (TotalOutputAmount / TotalInputAmount * 100.0) : 0.0);
			double OverheadPercentage = ((TotalInputAmount > 0.0) ? (TotalOHAmount / TotalInputAmount * 100.0) : 0.0);
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("Description", typeof(string));
			dt.Columns.Add("Qty", typeof(double));
			dt.Columns.Add("Amount", typeof(double));
			dt.Columns.Add("%", typeof(string));
			dt.Rows.Add(1, "Total Inputs", TotalInputQty, TotalInputAmount, 100.ToString("#,##.##"));
			dt.Rows.Add(2, "Total OverHead", TotalOHQty, TotalOHAmount, OverheadPercentage.ToString("#,##.##"));
			dt.Rows.Add(3, "Total Scrap", TotalScrapQty, TotalScrapAmount, ScrapPercentage.ToString("#,##.##"));
			dt.Rows.Add(4, "Total Output", TotalOutputQty, TotalOutputAmount, OutputPercentage.ToString("#,##.##"));
			double ExpectedTotalOutputAmount = TotalInputAmount + TotalOHAmount - TotalScrapAmount;
			double ExpectedOutputPercentage = ((TotalInputAmount > 0.0) ? (ExpectedTotalOutputAmount / TotalInputAmount * 100.0) : 0.0);
			dt.Rows.Add(5, "Total Expected Output", TotalOutputQty, ExpectedTotalOutputAmount, ExpectedOutputPercentage.ToString("#,##.##"));
			grdSummary.DataSource = dt;
			grdSummary.RetrieveStructure();
			CommonServices.GridWrappingAndColumnSettings(grdSummary, 1);
			grdSummary.RootTable.Columns["Id"].Visible = false;
			grdSummary.RootTable.Columns["Description"].Width = Constants.InventoryConstants.OtherDescription;
			grdSummary.RootTable.Columns["Qty"].Width = Constants.InventoryConstants.QtyConstant;
			grdSummary.RootTable.Columns["Amount"].Width = Constants.InventoryConstants.AmountConstant;
			grdSummary.RootTable.Columns["%"].Width = Constants.InventoryConstants.PercentOfTotal;
			double GainLoss = TotalScrapAmount + TotalOutputAmount - TotalInputAmount;
			if (GainLoss >= 0.0)
			{
				double GainLossPercentage = ((TotalInputAmount > 0.0) ? (GainLoss / TotalInputAmount * 100.0) : 0.0);
				labelGainLossCaption.Text = "Gain";
				labelGainLossCaption.ForeColor = Color.Green;
				labelGainLossPercent.Text = GainLossPercentage.ToString("#,##0.000") + " %";
				labelGainLossPercent.ForeColor = Color.Green;
			}
			else
			{
				double GainLossPercentage2 = ((TotalInputAmount > 0.0) ? (GainLoss / TotalInputAmount * 100.0) : 0.0);
				labelGainLossCaption.Text = "Loss";
				labelGainLossCaption.ForeColor = Color.Red;
				labelGainLossPercent.Text = GainLossPercentage2.ToString("#,##0.000") + " %";
				labelGainLossPercent.ForeColor = Color.Red;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnGenerate_Click(object sender, EventArgs e)
	{
		try
		{
			if (grdOutPut.GetDataRows().Count() == 0)
			{
				throw new Exception("Output Grid Record not Found");
			}
			UpdateOutputRates();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UpdateOutputRates()
	{
		try
		{
			List<GridEXRow> outputRows = grdOutPut.GetDataRows().ToList();
			if (outputRows.Count == 0)
			{
				throw new Exception("Output Grid Record not Found");
			}
			if (outputRows.Count == 1 && outputRows.FirstOrDefault((GridEXRow rr) => IsValidRow(rr)) == null)
			{
				throw new Exception("Output Grid Record not Filled Completely");
			}
			Calculation();
			double num = grdInput.GetDataRows().Sum((GridEXRow row) => (double)row.Cells["Amount"].Value);
			double sumOfOverhead = Conversion.ToDouble((object)grdOverHead.GetDataRows().Sum((GridEXRow row) => (decimal)row.Cells["Amount"].Value));
			double sumOfScrap = Conversion.ToDouble((object)grdScrap.GetDataRows().Sum((GridEXRow row) => (decimal)row.Cells["Amount"].Value));
			double netValue = num + sumOfOverhead - sumOfScrap;
			double itemQty = outputRows.Sum((GridEXRow row) => Conversion.ToDouble(row.Cells["NetWeight"].Value));
			foreach (GridEXRow item in outputRows)
			{
				item.BeginEdit();
				double totalCurrentRowQty = Conversion.ToDouble(item.Cells["NetWeight"].Value);
				double currentRowAmount = ((itemQty > 0.0) ? (netValue / itemQty * totalCurrentRowQty) : 0.0);
				item.Cells["Amount"].Value = currentRowAmount;
				double currentRowRate = ((totalCurrentRowQty > 0.0) ? (currentRowAmount / totalCurrentRowQty) : 0.0);
				item.Cells["Rate"].Value = currentRowRate;
				item.EndEdit();
			}
			Calculation();
		}
		catch (Exception ex)
		{
			throw new Exception("Error in UpdateOutputRates: " + ex.Message);
		}
	}

	private bool IsValidRow(GridEXRow row)
	{
		if (Conversion.ToInt(row.Cells["WareHouseId"].Value) <= 0 && Conversion.ToInt(row.Cells["ItemId"].Value) <= 0 && !(Conversion.ToDouble(row.Cells["ItemQty"].Value) > 0.0))
		{
			return Conversion.ToDouble(row.Cells["Amount"].Value) > 0.0;
		}
		return true;
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			if (tabControl1.SelectedIndex == 0)
			{
				DocDate.Focus();
			}
			else
			{
				datFromDate.Focus();
			}
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
			datFromDate.Value = DateTime.Now.AddDays(-3.0);
			datToDate.Value = DateTime.Now;
			txtDocNoFrom.Clear();
			txtDocNoTo.Clear();
			GrdHistory.DataSource = null;
			grdOutputHistory.DataSource = null;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BtnRefreshHistory_Click(object sender, EventArgs e)
	{
	}

	private void btnshow_Click(object sender, EventArgs e)
	{
		try
		{
			HistoryGridFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryGridFill()
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
			obj.DocumentTypeId = 1670;
			if (datFromDate.Checked)
			{
				obj.FromDate = datFromDate.Value;
			}
			if (datToDate.Checked)
			{
				obj.ToDate = datToDate.Value;
			}
			obj.FromDocNo = Conversion.ToDouble((object)txtDocNoFrom.Text.Trim());
			obj.ToDocNo = Conversion.ToDouble((object)txtDocNoTo.Text.Trim());
			obj.CanViewAllRecord = formRights.DoHaveCanViewAllRecordRights;
			if (!obj.CanViewAllRecord)
			{
				obj.EntryUser = UserAccount.ID;
			}
			DataTable dthis = ProductionHeader.FormHistory(obj);
			if (dthis.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("DocDate", typeof(DateTime));
				dt.Columns.Add("DocNo", typeof(int));
				dt.Columns.Add("DocumentTypeId", typeof(int));
				dt.Columns.Add("ProductionNo", typeof(string));
				dt.Columns.Add("BatchCode", typeof(string));
				dt.Columns.Add("heatCodeId", typeof(string));
				dt.Columns.Add("heatCode", typeof(string));
				dt.Columns.Add("InTime", typeof(DateTime));
				dt.Columns.Add("OutTime", typeof(DateTime));
				dt.Columns.Add("TotalTime", typeof(string));
				dt.Columns.Add("CastingType", typeof(string));
				dt.Columns.Add("BomHeaderId", typeof(int));
				dt.Columns.Add("BomNo", typeof(string));
				dt.Columns.Add("IsSettled", typeof(string));
				dt.Columns.Add("SettlementStatus", typeof(string));
				dt.Columns.Add("RemarksHeader", typeof(string));
				dt.Columns.Add("EntryDate", typeof(DateTime));
				dt.Columns.Add("EntryUser", typeof(string));
				dt.Columns.Add("ModifyDate", typeof(DateTime));
				dt.Columns.Add("ModifyUser", typeof(string));
				dt.Columns.Add("NoOfAttachments", typeof(int));
				for (int i = 0; i < dthis.Rows.Count; i++)
				{
					dt.Rows.Add(dthis.Rows[i]["Id"], dthis.Rows[i]["DocDate"], dthis.Rows[i]["DocNo"], dthis.Rows[i]["DocumentTypeId"], dthis.Rows[i]["ProductionNo"], dthis.Rows[i]["batchCode"], dthis.Rows[i]["heatCodeId"], dthis.Rows[i]["heatCode"], dthis.Rows[i]["inTime"], dthis.Rows[i]["outTime"], Conversion.ToString((object)(Conversion.ToDateTime(dthis.Rows[i]["outTime"]) - Conversion.ToDateTime(dthis.Rows[i]["inTime"]))), dthis.Rows[i]["CastingType"], dthis.Rows[i]["bomHeaderId"], dthis.Rows[i]["BomNo"], dthis.Rows[i]["IsSettled"], (Conversion.ToInt(dthis.Rows[i]["IsSettled"]) == 1) ? "Settled" : "Not Settled", dthis.Rows[i]["RemarksHeader"], dthis.Rows[i]["EntryDate"], dthis.Rows[i]["EntryUserName"], dthis.Rows[i]["ModifyDate"], dthis.Rows[i]["ModifyUserName"], dthis.Rows[i]["NoOfAttachments"]);
				}
				GrdHistory.DataSource = dt;
				GrdHistory.RetrieveStructure();
				HistoryGridSettings();
			}
			else
			{
				GrdHistory.ClearStructure();
				grdOutputHistory.DataSource = null;
				grdInputHistory.DataSource = null;
				grdScrapHistory.DataSource = null;
				grdOverHeadHistory.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryGridSettings()
	{
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Expected O, but got Unknown
		try
		{
			GrdHistory.RootTable.Columns["DocumentTypeId"].Visible = false;
			GrdHistory.RootTable.Columns["heatCodeId"].Visible = false;
			GrdHistory.RootTable.Columns["BomHeaderId"].Visible = false;
			GrdHistory.RootTable.Columns["Id"].Visible = false;
			GrdHistory.RootTable.Columns["IsSettled"].Visible = false;
			GrdHistory.RootTable.Columns["ProductionNo"].Caption = "Manual No";
			GrdHistory.RootTable.Columns["InTime"].FormatString = "dd-MMM-yy hh:mm tt";
			GrdHistory.RootTable.Columns["OutTime"].FormatString = "dd-MMM-yy hh:mm tt";
			GrdHistory.RootTable.Columns["TotalTime"].FormatString = "hh:mm";
			GrdHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yy hh:mm tt";
			GrdHistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yy hh:mm tt";
			GrdHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			GrdHistory.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)GrdHistory.RootTable.Columns)
			{
				item.HeaderAlignment = (TextAlignment)2;
				CommonServices.GridColumnSettings(item, 2);
			}
			GrdHistory.RootTable.Columns["DocDate"].Width = Constants.InventoryConstants.DateConstant;
			GrdHistory.RootTable.Columns["DocNo"].Width = Constants.InventoryConstants.DocNoConstant;
			GrdHistory.RootTable.Columns["ProductionNo"].Width = Constants.InventoryConstants.ProductionNo;
			GrdHistory.RootTable.Columns["BatchCode"].Width = Constants.InventoryConstants.OtherDescription;
			GrdHistory.RootTable.Columns["heatCode"].Width = Constants.InventoryConstants.OtherDescription;
			GrdHistory.RootTable.Columns["InTime"].Width = Constants.InventoryConstants.DateTimeConstantddMMM;
			GrdHistory.RootTable.Columns["OutTime"].Width = Constants.InventoryConstants.DateTimeConstantddMMM;
			GrdHistory.RootTable.Columns["TotalTime"].Width = 80;
			GrdHistory.RootTable.Columns["CastingType"].Width = Constants.InventoryConstants.CastingType;
			GrdHistory.RootTable.Columns["BomNo"].Width = Constants.InventoryConstants.ProductionNo;
			GrdHistory.RootTable.Columns["RemarksHeader"].Width = Constants.InventoryConstants.RemarksConstants;
			GrdHistory.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM;
			GrdHistory.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants;
			GrdHistory.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM;
			GrdHistory.RootTable.Columns["ModifyUser"].Width = Constants.InventoryConstants.ModifyUserConstants;
			GrdHistory.RootTable.Columns["NoOfAttachments"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants;
			GrdHistory.RootTable.Columns.Add("Edit");
			GrdHistory.RootTable.Columns["Edit"].Caption = "Edit";
			GrdHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			GrdHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			GrdHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			GrdHistory.RootTable.Columns["Edit"].Width = 50;
			GrdHistory.RootTable.Columns.Add("SaveAs");
			GrdHistory.RootTable.Columns["SaveAs"].Caption = "SaveAs";
			GrdHistory.RootTable.Columns["SaveAs"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			GrdHistory.RootTable.Columns["SaveAs"].ButtonStyle = (ButtonStyle)5;
			GrdHistory.RootTable.Columns["SaveAs"].ButtonText = "SaveAs";
			GrdHistory.RootTable.Columns["SaveAs"].Width = 60;
			GrdHistory.RootTable.Columns.Add("Print");
			GrdHistory.RootTable.Columns["Print"].Caption = "Print";
			GrdHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			GrdHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			GrdHistory.RootTable.Columns["Print"].ButtonText = "Print";
			GrdHistory.RootTable.Columns["Print"].Width = 50;
			GrdHistory.RootTable.Columns.Add("Voucher");
			GrdHistory.RootTable.Columns["Voucher"].Key = "Voucher";
			GrdHistory.RootTable.Columns["Voucher"].Caption = "Voucher";
			GrdHistory.RootTable.Columns["Voucher"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			GrdHistory.RootTable.Columns["Voucher"].ButtonStyle = (ButtonStyle)5;
			GrdHistory.RootTable.Columns["Voucher"].ButtonText = "Voucher";
			GrdHistory.RootTable.Columns["Voucher"].Width = 70;
			GrdHistory.RootTable.Columns["Edit"].Position = 0;
			GrdHistory.RootTable.Columns["SaveAs"].Position = 1;
			GrdHistory.RootTable.Columns["Print"].Position = 2;
			GrdHistory.RootTable.Columns["Voucher"].Position = 3;
			GrdHistory.FrozenColumns = 3;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdHistory_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = GrdHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				RecId = Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
				ReadById(RecId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = GrdHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0 && e.Column.Key == "NoOfAttachments")
			{
				int id = Conversion.ToInt(r.Cells["Id"].Value);
				int DocumentTypeId = Conversion.ToInt(r.Cells["DocumentTypeId"].Value);
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(id, DocumentTypeId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = GrdHistory.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0)
			{
				return;
			}
			int CurrId = Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
			if (e.Column.Key == "Edit")
			{
				RecId = CurrId;
				ReadById(RecId);
			}
			else if (e.Column.Key == "Print")
			{
				CommonServices.ProductionSlipEngr_1670(CurrId);
			}
			if (e.Column.Key == "Voucher")
			{
				if (Conversion.ToInt(r.Cells["IsSettled"].Value) != 1)
				{
					throw new Exception("This Document Is not settled yet");
				}
				VoucherReprot(CurrId);
			}
			else if (e.Column.Key == "SaveAs")
			{
				RecId = CurrId;
				ReadById(RecId);
				btnSave.Visible = false;
				btnUpdate.Visible = false;
				BtnSaveAs.Visible = true;
				grdOutPut.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdHistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdOutputHistory.DataSource = null;
			grdInputHistory.DataSource = null;
			grdScrapHistory.DataSource = null;
			grdOverHeadHistory.DataSource = null;
			GridEXRow r = GrdHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				int CurrId = Conversion.ToInt(r.Cells["Id"].Value);
				if (CurrId > 0)
				{
					BindAllDetailHistories(CurrId);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindAllDetailHistories(int CurrId)
	{
		try
		{
			ProductionHeader obj = ProductionHeader.ReadById(CurrId);
			if (obj == null)
			{
				return;
			}
			DataTable dtInputHistory = new DataTable();
			dtInputHistory.Columns.Add("Id", typeof(int));
			dtInputHistory.Columns.Add("WarehouseName", typeof(string));
			dtInputHistory.Columns.Add("ItemId", typeof(int));
			dtInputHistory.Columns.Add("ItemName", typeof(string));
			dtInputHistory.Columns.Add("PackUom", typeof(string));
			dtInputHistory.Columns.Add("NetWeight", typeof(double));
			dtInputHistory.Columns.Add("Rate", typeof(double));
			dtInputHistory.Columns.Add("RateUom", typeof(string));
			dtInputHistory.Columns.Add("Amount", typeof(double));
			DataTable dtOutputHistory = new DataTable();
			dtOutputHistory.Columns.Add("Id", typeof(int));
			dtOutputHistory.Columns.Add("WorkOrderNo", typeof(string));
			dtOutputHistory.Columns.Add("WarehouseName", typeof(string));
			dtOutputHistory.Columns.Add("ItemId", typeof(int));
			dtOutputHistory.Columns.Add("ItemName", typeof(string));
			dtOutputHistory.Columns.Add("PackUom", typeof(string));
			dtOutputHistory.Columns.Add("ModalDescription", typeof(string));
			dtOutputHistory.Columns.Add("ProcessStatus", typeof(string));
			dtOutputHistory.Columns.Add("NetWeight", typeof(double));
			dtOutputHistory.Columns.Add("WeightPerQty", typeof(decimal));
			dtOutputHistory.Columns.Add("CastingWeight", typeof(decimal));
			dtOutputHistory.Columns.Add("Rate", typeof(double));
			dtOutputHistory.Columns.Add("RateUom", typeof(string));
			dtOutputHistory.Columns.Add("Amount", typeof(double));
			DataTable dtScrapHistory = new DataTable();
			dtScrapHistory.Columns.Add("Id", typeof(int));
			dtScrapHistory.Columns.Add("WarehouseName", typeof(string));
			dtScrapHistory.Columns.Add("ItemId", typeof(int));
			dtScrapHistory.Columns.Add("ItemName", typeof(string));
			dtScrapHistory.Columns.Add("PackUom", typeof(string));
			dtScrapHistory.Columns.Add("NetWeight", typeof(decimal));
			dtScrapHistory.Columns.Add("Rate", typeof(decimal));
			dtScrapHistory.Columns.Add("RateUom", typeof(string));
			dtScrapHistory.Columns.Add("Amount", typeof(decimal));
			DataTable dtOhHistory = new DataTable();
			dtOhHistory.Columns.Add("Id", typeof(int));
			dtOhHistory.Columns.Add("OverHeadAccount", typeof(string));
			dtOhHistory.Columns.Add("PercentOfTotalInput", typeof(decimal));
			dtOhHistory.Columns.Add("Qty", typeof(decimal));
			dtOhHistory.Columns.Add("Rate", typeof(decimal));
			dtOhHistory.Columns.Add("Amount", typeof(decimal));
			dtOhHistory.Columns.Add("ohRemarks", typeof(string));
			for (int k = 0; k < obj.ProductionOutPutDetailslist.Count; k++)
			{
				if (obj.ProductionOutPutDetailslist[k].OutPutTypeId == 1)
				{
					dtOutputHistory.Rows.Add(obj.ProductionOutPutDetailslist[k].ProductionOutPutDetailId, obj.ProductionOutPutDetailslist[k].WorkOrderNo, obj.ProductionOutPutDetailslist[k].WareHouseName, obj.ProductionOutPutDetailslist[k].ItemId, obj.ProductionOutPutDetailslist[k].ItemName, obj.ProductionOutPutDetailslist[k].PackUom, obj.ProductionOutPutDetailslist[k].VarientDescription, obj.ProductionOutPutDetailslist[k].ProcessStatus, obj.ProductionOutPutDetailslist[k].NetWeight, obj.ProductionOutPutDetailslist[k].ItemWeight, obj.ProductionOutPutDetailslist[k].NetWeight * Conversion.ToDecimal((object)obj.ProductionOutPutDetailslist[k].ItemWeight), obj.ProductionOutPutDetailslist[k].ItemRate, obj.ProductionOutPutDetailslist[k].RateUom, obj.ProductionOutPutDetailslist[k].Amount);
				}
				else
				{
					dtScrapHistory.Rows.Add(obj.ProductionOutPutDetailslist[k].ProductionOutPutDetailId, obj.ProductionOutPutDetailslist[k].WareHouseName, obj.ProductionOutPutDetailslist[k].ItemId, obj.ProductionOutPutDetailslist[k].ItemName, obj.ProductionOutPutDetailslist[k].PackUom, obj.ProductionOutPutDetailslist[k].NetWeight, obj.ProductionOutPutDetailslist[k].ItemRate, obj.ProductionOutPutDetailslist[k].RateUom, obj.ProductionOutPutDetailslist[k].Amount);
				}
			}
			grdOutputHistory.DataSource = dtOutputHistory;
			grdOutputHistory.RetrieveStructure();
			grdOutputHistorySettings();
			grdScrapHistory.DataSource = dtScrapHistory;
			grdScrapHistory.RetrieveStructure();
			grdScrapHistorySettings();
			for (int j = 0; j < obj.ProductionInputDetailslist.Count; j++)
			{
				dtInputHistory.Rows.Add(obj.ProductionInputDetailslist[j].productionInputDetailId, obj.ProductionInputDetailslist[j].WareHouseName, obj.ProductionInputDetailslist[j].ItemId, obj.ProductionInputDetailslist[j].ItemName, obj.ProductionInputDetailslist[j].PackUom, obj.ProductionInputDetailslist[j].Weight, obj.ProductionInputDetailslist[j].ItemRate, obj.ProductionInputDetailslist[j].RateUom, obj.ProductionInputDetailslist[j].Amount);
			}
			grdInputHistory.DataSource = dtInputHistory;
			grdInputHistory.RetrieveStructure();
			grdInputHistorySettings();
			for (int i = 0; i < obj.ProductionOverHeadsDetailList.Count; i++)
			{
				dtOhHistory.Rows.Add(obj.ProductionOverHeadsDetailList[i].ProductionOverHeadsDetailId, obj.ProductionOverHeadsDetailList[i].OverHeadAccountTitle, obj.ProductionOverHeadsDetailList[i].PercentOfTotalInput, obj.ProductionOverHeadsDetailList[i].Qty, obj.ProductionOverHeadsDetailList[i].Rate, obj.ProductionOverHeadsDetailList[i].Amount, obj.ProductionOverHeadsDetailList[i].Remarks);
			}
			grdOverHeadHistory.DataSource = dtOhHistory;
			grdOverHeadHistory.RetrieveStructure();
			grdOverHeadHistoryGridSetting();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInputHistorySettings()
	{
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Expected O, but got Unknown
		try
		{
			grdInputHistory.RootTable.Columns["Id"].Visible = false;
			grdInputHistory.RootTable.Columns["ItemId"].Visible = false;
			grdInputHistory.RootTable.Columns["PackUom"].Caption = "Uom";
			grdInputHistory.RootTable.Columns["NetWeight"].Caption = "Qty";
			grdInputHistory.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grdInputHistory.RootTable.Columns)
			{
				item.HeaderAlignment = (TextAlignment)2;
				CommonServices.GridColumnSettings(item, 2);
			}
			grdInputHistory.RootTable.Columns["WarehouseName"].Width = Constants.InventoryConstants.WareHouseName;
			grdInputHistory.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			grdInputHistory.RootTable.Columns["PackUom"].Width = Constants.InventoryConstants.UomConstant;
			grdInputHistory.RootTable.Columns["NetWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdInputHistory.RootTable.Columns["Rate"].Width = Constants.InventoryConstants.ExRateConstant;
			grdInputHistory.RootTable.Columns["RateUom"].Width = Constants.InventoryConstants.UomConstant;
			grdInputHistory.RootTable.Columns["Amount"].Width = Constants.InventoryConstants.AmountConstant;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOutputHistorySettings()
	{
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Expected O, but got Unknown
		try
		{
			grdOutputHistory.RootTable.Columns["Id"].Visible = false;
			grdOutputHistory.RootTable.Columns["ItemId"].Visible = false;
			grdOutputHistory.RootTable.Columns["PackUom"].Caption = "Uom";
			grdOutputHistory.RootTable.Columns["NetWeight"].Caption = "Qty";
			grdOutputHistory.RootTable.Columns["ProcessStatus"].Caption = "Production Stage";
			grdOutputHistory.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grdOutputHistory.RootTable.Columns)
			{
				item.HeaderAlignment = (TextAlignment)2;
				CommonServices.GridColumnSettings(item, 2);
			}
			grdOutputHistory.RootTable.Columns["ModalDescription"].Width = Constants.InventoryConstants.OtherDescription;
			grdOutputHistory.RootTable.Columns["WarehouseName"].Width = Constants.InventoryConstants.WareHouseName;
			grdOutputHistory.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			grdOutputHistory.RootTable.Columns["PackUom"].Width = Constants.InventoryConstants.UomConstant;
			grdOutputHistory.RootTable.Columns["NetWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdOutputHistory.RootTable.Columns["Rate"].Width = Constants.InventoryConstants.ExRateConstant;
			grdOutputHistory.RootTable.Columns["RateUom"].Width = Constants.InventoryConstants.UomConstant;
			grdOutputHistory.RootTable.Columns["Amount"].Width = Constants.InventoryConstants.AmountConstant;
			grdOutputHistory.RootTable.Columns["WeightPerQty"].Width = Constants.InventoryConstants.WeightConstant;
			grdOutputHistory.RootTable.Columns["CastingWeight"].Width = Constants.InventoryConstants.WeightConstant;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdScrapHistorySettings()
	{
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Expected O, but got Unknown
		try
		{
			grdScrapHistory.RootTable.Columns["Id"].Visible = false;
			grdScrapHistory.RootTable.Columns["ItemId"].Visible = false;
			grdScrapHistory.RootTable.Columns["PackUom"].Caption = "Uom";
			grdScrapHistory.RootTable.Columns["NetWeight"].Caption = "Weight";
			grdScrapHistory.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grdScrapHistory.RootTable.Columns)
			{
				item.HeaderAlignment = (TextAlignment)2;
				CommonServices.GridColumnSettings(item, 2);
			}
			grdScrapHistory.RootTable.Columns["WarehouseName"].Width = Constants.InventoryConstants.WareHouseName;
			grdScrapHistory.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			grdScrapHistory.RootTable.Columns["PackUom"].Width = Constants.InventoryConstants.UomConstant;
			grdScrapHistory.RootTable.Columns["NetWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdScrapHistory.RootTable.Columns["Rate"].Width = Constants.InventoryConstants.ExRateConstant;
			grdScrapHistory.RootTable.Columns["RateUom"].Width = Constants.InventoryConstants.UomConstant;
			grdScrapHistory.RootTable.Columns["Amount"].Width = Constants.InventoryConstants.AmountConstant;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOverHeadHistoryGridSetting()
	{
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Expected O, but got Unknown
		try
		{
			grdOverHeadHistory.RootTable.Columns["Id"].Visible = false;
			grdOverHeadHistory.RootTable.Columns["PercentOfTotalInput"].Caption = "Percentage";
			grdOverHeadHistory.RootTable.Columns["OverHeadAccount"].Caption = "AccountTitle";
			grdOverHeadHistory.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grdOverHeadHistory.RootTable.Columns)
			{
				item.HeaderStyle.TextAlignment = (TextAlignment)2;
				CommonServices.GridColumnSettings(item);
			}
			grdOverHeadHistory.RootTable.Columns["OverHeadAccount"].Width = 200;
			grdOverHeadHistory.RootTable.Columns["PercentOfTotalInput"].Width = 90;
			grdOverHeadHistory.RootTable.Columns["Qty"].Width = 60;
			grdOverHeadHistory.RootTable.Columns["Rate"].Width = 65;
			grdOverHeadHistory.RootTable.Columns["Amount"].Width = 90;
			grdOverHeadHistory.RootTable.Columns["OhRemarks"].Width = 250;
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)GrdHistory).Name);
			if (fs != null)
			{
				GrdHistory.LoadLayoutFile((Stream)fs);
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdOutPut).Name);
			if (fs != null)
			{
				grdOutPut.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
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

	private void txtDocNoFrom_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtDocNoTo_KeyPress(object sender, KeyPressEventArgs e)
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

	private void VoucherReprot(int Id)
	{
		try
		{
			CommonServices.VoucherReport_118(Conversion.ToInt((object)CommonServices.VoucherHeadIdGet(Id, 1670)), 1670);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnPrint1619_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId > 0)
			{
				CommonServices.ProductionSlipEngr_1670(RecId);
				return;
			}
			throw new Exception("No Data Found");
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
			VoucherReprot(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmProductionEngr_KeyDown(object sender, KeyEventArgs e)
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
			if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
			}
			if (tabControl1.SelectedIndex == 0)
			{
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnNew_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.R)
				{
					BtnRefresh_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.S && btnSave.Visible && btnSave.Enabled)
				{
					btnSave_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.U && btnUpdate.Visible && btnUpdate.Enabled)
				{
					btnUpdate_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F12 && BtnSaveAs.Visible && BtnSaveAs.Enabled)
				{
					BtnSaveAs_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.P && formRights.DoHavePrintRights)
				{
					btnPrint1619_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					DocDate.Focus();
				}
				if (e.Control && e.KeyCode == Keys.F10)
				{
					btnAttachment_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grdOutPut).Focus();
				}
				if (e.Control && e.KeyCode != Keys.Up)
				{
				}
			}
			else if (tabControl1.SelectedIndex == 1)
			{
				if (e.Control && e.KeyCode == Keys.N)
				{
					BtnNewHistory_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.R)
				{
					BtnRefreshHistory_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.S)
				{
					btnshow_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					datFromDate.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grdOutputHistory).Focus();
				}
				if (e.Control && e.KeyCode == Keys.Up)
				{
					((Control)(object)GrdHistory).Focus();
				}
			}
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

	private void MakeShortCutKeys()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("KeyCombination");
			dt.Columns.Add("Description");
			dt.Rows.Add("Ctrl+S", "For Save When on Entry form and For Show Data when on History Form");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+Shift+Delete", "For Delete");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Ctrl+F5", "For Focus on Doc Date");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+Delete", "For Deleting an row of Focused Grid");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid when in Form And On History Grid When On History");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Column Of Detail(Item Name)");
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

	private void grdOutPut_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_032f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0334: Unknown result type (might be due to invalid IL or missing references)
		//IL_0345: Unknown result type (might be due to invalid IL or missing references)
		//IL_0356: Unknown result type (might be due to invalid IL or missing references)
		//IL_036c: Unknown result type (might be due to invalid IL or missing references)
		//IL_038b: Unknown result type (might be due to invalid IL or missing references)
		//IL_039d: Expected O, but got Unknown
		try
		{
			GridEXRow r = grdOutPut.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdOutPut.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdOutPut.CurrentColumn != null && grdOutPut.CurrentColumn.Key == "Delete")
				{
					DeleteOutputDetailRow(r);
				}
				if (grdOutPut.CurrentColumn != null && grdOutPut.CurrentColumn.Key == "Add")
				{
					AddRowInOutputGrid();
				}
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				DeleteOutputDetailRow(r);
			}
			if (e.Control && e.KeyCode == Keys.D)
			{
				AddRowInOutputGrid();
			}
			if (e.KeyCode == Keys.F1)
			{
				int Id = 0;
				string NameCode = "";
				if (grdOutPut.CurrentColumn.Key == "RateUomId" || grdOutPut.CurrentColumn.Key == "RateUom" || grdOutPut.CurrentColumn.Key == "RateEquivalent")
				{
					DataTable dataTable = dtUomByItemId(Conversion.ToInt(r.Cells["ItemId"].Value));
					GrdPopUp obj2 = new GrdPopUp(dataTable, "UomId", "Uom");
					obj2.StartPosition = FormStartPosition.CenterScreen;
					obj2.ShowDialog();
					obj2.BringToFront();
					Id = Conversion.ToInt((object)obj2.ReturnId);
					NameCode = Conversion.ToString((object)obj2.ReturnName);
					DataRow foundRow = dataTable.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<int>("UomId") == Id);
					double Equivalent = ((foundRow != null) ? Conversion.ToDouble(foundRow["UomEquivalent"]) : 0.0);
					r.Cells["RateUomId"].Value = Id;
					r.Cells["RateUom"].Value = NameCode;
					r.Cells["RateEquivalent"].Value = Equivalent;
					decimal NetWeight = Conversion.ToDecimal((object)r.Cells["NetWeight"].Value.ToString());
					decimal rate = Conversion.ToDecimal((object)r.Cells["Rate"].Value.ToString());
					decimal Ruom = Conversion.ToDecimal((object)r.Cells["RateEquivalent"].Value.ToString());
					decimal Amount = ((Ruom > 0m) ? (NetWeight / Ruom * rate) : 0m);
					r.Cells["Amount"].Value = Amount;
				}
				if (grdOutPut.CurrentColumn != null && (grdOutPut.CurrentColumn.Key == "WorkOrderId" || grdOutPut.CurrentColumn.Key == "WorkOrderNo"))
				{
					ReportsParameters val = new ReportsParameters
					{
						OrganizationId = UserAccount.OrganizationId,
						CompanyId = UserAccount.CompanyId,
						CastingTypeId = Conversion.ToInt(CmbCastingType.Value)
					};
					if (val.CastingTypeId == 0)
					{
						CmbCastingType.Focus();
						throw new Exception("Casting type Field Required");
					}
					val.RecId = RecId;
					DataTable dt = ProductionHeader.WorkOrderSemiFinishForProductionOutput(val);
					List<string> columnsToHide = new List<string> { "WorkOrderId", "workOrderItemDetailId", "ItemId", "UomId", "ItemEquivalent", "ItemVariantId", "CastingTypeId" };
					GrdPopUpToReturndt obj = new GrdPopUpToReturndt(dt, selection: true, returnFullGrdDt: false, columnsToHide);
					obj.StartPosition = FormStartPosition.CenterScreen;
					obj.WindowState = FormWindowState.Maximized;
					obj.ShowDialog();
					obj.BringToFront();
					if (obj.dtReturn.Rows.Count > 0)
					{
						if (Conversion.ToInt(r.Cells["ItemId"].Value) == 0 && Conversion.ToInt(r.Cells["WorkOrderId"].Value) == 0)
						{
							r.Delete();
						}
						foreach (DataRow dr in obj.dtReturn.Rows)
						{
							bool flag = false;
							if (grdOutPut.GetRows().Length != 0)
							{
								GridEXRow[] rows = grdOutPut.GetRows();
								for (int i = 0; i < rows.Length; i++)
								{
									if (Conversion.ToInt(rows[i].Cells["workOrderItemDetailId"].Value) == Conversion.ToInt(dr["workOrderItemDetailId"]))
									{
										flag = true;
										break;
									}
								}
							}
							if (!flag)
							{
								dtdetailOutput.Rows.Add(0, Conversion.ToInt(dr["WorkOrderId"]), Conversion.ToInt(dr["workOrderItemDetailId"]), Conversion.ToString(dr["WorkOrderNo"]), DefaultCastingWarehouseId, Conversion.ToInt(dr["ItemId"]), Conversion.ToString(dr["ItemName"]), Conversion.ToInt(dr["UomId"]), Conversion.ToString(dr["ItemUom"]), Conversion.ToDouble(dr["ItemEquivalent"]), Conversion.ToInt(dr["ItemVariantId"]), Conversion.ToString(dr["ModelDescription"]), DefaultProductionStageId, DefaultProductionStageName, Conversion.ToDouble(dr["BalQty"]), Conversion.ToDouble(dr["BalQty"]), Conversion.ToDouble(dr["ItemWeight"]), Conversion.ToDouble(dr["BalQty"]) * Conversion.ToDouble(dr["ItemWeight"]), 0, Conversion.ToInt(dr["UomId"]), Conversion.ToString(dr["ItemUom"]), Conversion.ToDouble(dr["ItemEquivalent"]), 0);
							}
						}
						grdOutPut.DataSource = dtdetailOutput;
						grdOutPut.RetrieveStructure();
						grdOutPutSettings();
						((Control)(object)CmbCastingType).Enabled = !grdOutPut.GetRows().Any((GridEXRow rrr) => Convert.ToInt32(rrr.Cells["WorkOrderId"].Value) > 0);
					}
				}
				grdOutPut.UpdateData();
			}
			Calculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInput_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdInput.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdInput.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdInput.CurrentColumn != null && grdInput.CurrentColumn.Key == "Delete")
				{
					DeleteInputDetailRow(r);
				}
				if (grdInput.CurrentColumn != null && grdInput.CurrentColumn.Key == "Add")
				{
					AddRowInInputGrid();
				}
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				DeleteInputDetailRow(r);
			}
			if (e.Control && e.KeyCode == Keys.D)
			{
				AddRowInInputGrid();
			}
			if (e.KeyCode == Keys.F1)
			{
				int Id = 0;
				string NameCode = "";
				if (grdInput.CurrentColumn != null && (grdInput.CurrentColumn.Key == "WarehouseId" || grdInput.CurrentColumn.Key == "WarehouseName"))
				{
					GrdPopUp obj4 = new GrdPopUp(dtWarehouse, "Id", "WarehouseName");
					obj4.StartPosition = FormStartPosition.CenterScreen;
					obj4.ShowDialog();
					obj4.BringToFront();
					Id = Conversion.ToInt((object)obj4.ReturnId);
					NameCode = Conversion.ToString((object)obj4.ReturnName);
					r.Cells["WarehouseId"].Value = Id;
				}
				if (grdInput.CurrentColumn != null && (grdInput.CurrentColumn.Key == "ItemId" || grdInput.CurrentColumn.Key == "ItemName"))
				{
					GrdPopUpMultiSelection obj3 = new GrdPopUpMultiSelection(dtitemScrap, "Id", "ItemName");
					obj3.StartPosition = FormStartPosition.CenterScreen;
					obj3.ShowDialog();
					obj3.BringToFront();
					if (obj3.dtReturn.Rows.Count > 0)
					{
						if (obj3.dtReturn.Rows.Count == 1)
						{
							r.Cells["ItemId"].Value = Conversion.ToInt(obj3.dtReturn.Rows[0]["ReturnId"]);
							r.Cells["ItemName"].Value = Conversion.ToString(obj3.dtReturn.Rows[0]["ReturnName"]);
						}
						else
						{
							if (Conversion.ToInt(r.Cells["ItemId"].Value) == 0 && Conversion.ToDouble(r.Cells["NetWeight"].Value) == 0.0)
							{
								r.Delete();
							}
							foreach (DataRow rr in obj3.dtReturn.Rows)
							{
								if (dtdetailInput.AsEnumerable().FirstOrDefault((DataRow row) => Conversion.ToInt((object)row.Field<int>("ItemId")) == Conversion.ToInt(rr["ReturnId"])) == null)
								{
									DataRow dr = dtdetailInput.NewRow();
									dr["ItemId"] = Conversion.ToInt(rr["ReturnId"]);
									dr["ItemName"] = Conversion.ToString(rr["ReturnName"]);
									dtdetailInput.Rows.Add(dr);
								}
							}
							grdInput.DataSource = dtdetailInput;
							grdInput.RetrieveStructure();
							grdInputSettings();
						}
					}
				}
				if (grdInput.CurrentColumn != null && (grdInput.CurrentColumn.Key == "PackUomId" || grdInput.CurrentColumn.Key == "PackUom" || grdInput.CurrentColumn.Key == "PackEquivalent"))
				{
					DataTable dataTable = dtUomByItemId(Conversion.ToInt(r.Cells["ItemId"].Value));
					GrdPopUp obj2 = new GrdPopUp(dataTable, "UomId", "Uom");
					obj2.StartPosition = FormStartPosition.CenterScreen;
					obj2.ShowDialog();
					obj2.BringToFront();
					Id = Conversion.ToInt((object)obj2.ReturnId);
					NameCode = Conversion.ToString((object)obj2.ReturnName);
					DataRow foundRow2 = dataTable.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<int>("UomId") == Id);
					double Equivalent2 = ((foundRow2 != null) ? Conversion.ToDouble(foundRow2["UomEquivalent"]) : 0.0);
					r.Cells["PackUomId"].Value = Id;
					r.Cells["PackUom"].Value = NameCode;
					r.Cells["PackEquivalent"].Value = Equivalent2;
				}
				if (grdInput.CurrentColumn != null && (grdInput.CurrentColumn.Key == "RateUomId" || grdInput.CurrentColumn.Key == "RateUom" || grdInput.CurrentColumn.Key == "RateEquivalent"))
				{
					DataTable dataTable2 = dtUomByItemId(Conversion.ToInt(r.Cells["ItemId"].Value));
					GrdPopUp obj = new GrdPopUp(dataTable2, "UomId", "Uom");
					obj.StartPosition = FormStartPosition.CenterScreen;
					obj.ShowDialog();
					obj.BringToFront();
					Id = Conversion.ToInt((object)obj.ReturnId);
					NameCode = Conversion.ToString((object)obj.ReturnName);
					DataRow foundRow = dataTable2.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<int>("UomId") == Id);
					double Equivalent = ((foundRow != null) ? Conversion.ToDouble(foundRow["UomEquivalent"]) : 0.0);
					r.Cells["RateUomId"].Value = Id;
					r.Cells["RateUom"].Value = NameCode;
					r.Cells["RateEquivalent"].Value = Equivalent;
					decimal NetWeight = Conversion.ToDecimal((object)r.Cells["NetWeight"].Value.ToString());
					decimal rate = Conversion.ToDecimal((object)r.Cells["Rate"].Value.ToString());
					decimal Ruom = Conversion.ToDecimal((object)r.Cells["RateEquivalent"].Value.ToString());
					decimal Amount = ((Ruom > 0m) ? (NetWeight / Ruom * rate) : 0m);
					r.Cells["Amount"].Value = Amount;
				}
				grdInput.UpdateData();
			}
			Calculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdScrap_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdScrap.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdScrap.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdScrap.CurrentColumn != null && grdScrap.CurrentColumn.Key == "Delete")
				{
					DeleteScrapDetailRow(r);
				}
				if (grdScrap.CurrentColumn != null && grdScrap.CurrentColumn.Key == "Add")
				{
					AddRowInScrapGrid();
				}
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				DeleteScrapDetailRow(r);
			}
			if (e.Control && e.KeyCode == Keys.D)
			{
				AddRowInScrapGrid();
			}
			if (e.KeyCode == Keys.F1)
			{
				int Id = 0;
				string NameCode = "";
				if (grdScrap.CurrentColumn != null && (grdScrap.CurrentColumn.Key == "WarehouseId" || grdScrap.CurrentColumn.Key == "WarehouseName"))
				{
					GrdPopUp obj4 = new GrdPopUp(dtWarehouse, "Id", "WarehouseName");
					obj4.StartPosition = FormStartPosition.CenterScreen;
					obj4.ShowDialog();
					obj4.BringToFront();
					Id = Conversion.ToInt((object)obj4.ReturnId);
					NameCode = Conversion.ToString((object)obj4.ReturnName);
					r.Cells["WarehouseId"].Value = Id;
				}
				if (grdScrap.CurrentColumn != null && (grdScrap.CurrentColumn.Key == "ItemId" || grdScrap.CurrentColumn.Key == "ItemName"))
				{
					GrdPopUpMultiSelection obj3 = new GrdPopUpMultiSelection(dtitemScrap, "Id", "ItemName");
					obj3.StartPosition = FormStartPosition.CenterScreen;
					obj3.ShowDialog();
					obj3.BringToFront();
					if (obj3.dtReturn.Rows.Count > 0)
					{
						if (obj3.dtReturn.Rows.Count == 1)
						{
							r.Cells["ItemId"].Value = Conversion.ToInt(obj3.dtReturn.Rows[0]["ReturnId"]);
							r.Cells["ItemName"].Value = Conversion.ToString(obj3.dtReturn.Rows[0]["ReturnName"]);
						}
						else
						{
							if (Conversion.ToInt(r.Cells["ItemId"].Value) == 0 && Conversion.ToDouble(r.Cells["NetWeight"].Value) == 0.0)
							{
								r.Delete();
							}
							foreach (DataRow rr in obj3.dtReturn.Rows)
							{
								if (dtScrap.AsEnumerable().FirstOrDefault((DataRow row) => Conversion.ToInt((object)row.Field<int>("ItemId")) == Conversion.ToInt(rr["ReturnId"])) == null)
								{
									DataRow dr = dtScrap.NewRow();
									dr["ItemId"] = Conversion.ToInt(rr["ReturnId"]);
									dr["ItemName"] = Conversion.ToString(rr["ReturnName"]);
									dtScrap.Rows.Add(dr);
								}
							}
							grdScrap.DataSource = dtScrap;
							grdScrap.RetrieveStructure();
							grdScrapSettings();
						}
					}
				}
				if (grdScrap.CurrentColumn != null && (grdScrap.CurrentColumn.Key == "PackUomId" || grdScrap.CurrentColumn.Key == "PackUom" || grdScrap.CurrentColumn.Key == "PackEquivalent"))
				{
					DataTable dataTable = dtUomByItemId(Conversion.ToInt(r.Cells["ItemId"].Value));
					GrdPopUp obj2 = new GrdPopUp(dataTable, "UomId", "Uom");
					obj2.StartPosition = FormStartPosition.CenterScreen;
					obj2.ShowDialog();
					obj2.BringToFront();
					Id = Conversion.ToInt((object)obj2.ReturnId);
					NameCode = Conversion.ToString((object)obj2.ReturnName);
					DataRow foundRow2 = dataTable.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<int>("UomId") == Id);
					double Equivalent2 = ((foundRow2 != null) ? Conversion.ToDouble(foundRow2["UomEquivalent"]) : 0.0);
					r.Cells["PackUomId"].Value = Id;
					r.Cells["PackUom"].Value = NameCode;
					r.Cells["PackEquivalent"].Value = Equivalent2;
				}
				if (grdScrap.CurrentColumn != null && (grdScrap.CurrentColumn.Key == "RateUomId" || grdScrap.CurrentColumn.Key == "RateUom" || grdScrap.CurrentColumn.Key == "RateEquivalent"))
				{
					DataTable dataTable2 = dtUomByItemId(Conversion.ToInt(r.Cells["ItemId"].Value));
					GrdPopUp obj = new GrdPopUp(dataTable2, "UomId", "Uom");
					obj.StartPosition = FormStartPosition.CenterScreen;
					obj.ShowDialog();
					obj.BringToFront();
					Id = Conversion.ToInt((object)obj.ReturnId);
					NameCode = Conversion.ToString((object)obj.ReturnName);
					DataRow foundRow = dataTable2.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<int>("UomId") == Id);
					double Equivalent = ((foundRow != null) ? Conversion.ToDouble(foundRow["UomEquivalent"]) : 0.0);
					r.Cells["RateUomId"].Value = Id;
					r.Cells["RateUom"].Value = NameCode;
					r.Cells["RateEquivalent"].Value = Equivalent;
					decimal NetWeight = Conversion.ToDecimal((object)r.Cells["NetWeight"].Value.ToString());
					decimal rate = Conversion.ToDecimal((object)r.Cells["Rate"].Value.ToString());
					decimal Ruom = Conversion.ToDecimal((object)r.Cells["RateEquivalent"].Value.ToString());
					decimal Amount = ((Ruom > 0m) ? (NetWeight / Ruom * rate) : 0m);
					r.Cells["Amount"].Value = Amount;
				}
				grdScrap.UpdateData();
			}
			Calculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOverHead_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdOverHead.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdOverHead.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
				if (grdOverHead.CurrentColumn.Key == "Add")
				{
					AddRowInOverHeadGrid();
				}
				if (grdOverHead.CurrentColumn.Key == "Delete")
				{
					dtOverHead.Rows[r.RowIndex].Delete();
					if (r != null && (int)r.RowType == 0)
					{
						r.Delete();
					}
					if (dtOverHead.Rows.Count == 0)
					{
						AddRowInOverHeadGrid();
					}
				}
				grdOverHead.UpdateData();
			}
			if (e.Control && e.KeyCode == Keys.D)
			{
				AddRowInOverHeadGrid();
				grdOverHead.UpdateData();
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				dtOverHead.Rows[r.RowIndex].Delete();
				if (r != null && (int)r.RowType == 0)
				{
					r.Delete();
				}
				if (dtOverHead.Rows.Count == 0)
				{
					AddRowInOverHeadGrid();
				}
				grdOverHead.UpdateData();
			}
			if (e.KeyCode == Keys.F1 && grdOverHead.CurrentColumn != null && grdOverHead.CurrentColumn.Key == "OverHeadAccountId")
			{
				GrdPopUpMultiSelection obj = new GrdPopUpMultiSelection(dtAccounts, "Id", "AccountTitle");
				obj.StartPosition = FormStartPosition.CenterScreen;
				obj.ShowDialog();
				obj.BringToFront();
				if (obj.dtReturn.Rows.Count > 0)
				{
					if (obj.dtReturn.Rows.Count == 1)
					{
						r.Cells["OverHeadAccountId"].Value = Conversion.ToInt(obj.dtReturn.Rows[0]["ReturnId"]);
					}
					else
					{
						if (Conversion.ToInt(r.Cells["OverHeadAccountId"].Value) == 0 && Conversion.ToInt(r.Cells["Amount"].Value) == 0)
						{
							r.Delete();
						}
						foreach (DataRow rr in obj.dtReturn.Rows)
						{
							if (dtOverHead.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<int>("OverHeadAccountId") == Conversion.ToInt(rr["ReturnId"])) == null)
							{
								DataRow dr = dtOverHead.NewRow();
								dr["OverHeadAccountId"] = Conversion.ToInt(rr["ReturnId"]);
								dtOverHead.Rows.Add(dr);
							}
						}
					}
				}
			}
			Calculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdHistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			GridEXRow item = GrdHistory.CurrentRow;
			if (item == null || (int)item.RowType != 0 || item.Cells.Count <= 0)
			{
				return;
			}
			int ID = 0;
			int documentTypeId = 0;
			ID = Conversion.ToInt(item.Cells["Id"].Value);
			documentTypeId = Conversion.ToInt(item.Cells["DocumentTypeId"].Value);
			if (e.Control && e.KeyCode == Keys.Return)
			{
				ReadById(ID);
			}
			if (e.Control && e.KeyCode == Keys.F12)
			{
				RecId = ID;
				ReadById(RecId);
				BtnSaveAs.Visible = false;
				btnUpdate.Visible = false;
				BtnSaveAs.Visible = true;
			}
			else
			{
				if (!e.Control || e.KeyCode != Keys.Space)
				{
					return;
				}
				if (GrdHistory.CurrentColumn.Key == "Edit")
				{
					ReadById(ID);
				}
				if (GrdHistory.CurrentColumn.Key == "Voucher")
				{
					if (Conversion.ToInt(item.Cells["IsSettled"].Value) != 1)
					{
						throw new Exception("This Document Is not settled yet");
					}
					VoucherReprot(ID);
				}
				else if (GrdHistory.CurrentColumn.Key == "Print")
				{
					if (formRights.DoHavePrintRights && GrdHistory.RowCount > 0)
					{
						CommonServices.ProductionSlipEngr_1670(ID);
					}
				}
				else if (GrdHistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(ID, documentTypeId);
				}
				else if (GrdHistory.CurrentColumn.Key == "SaveAs")
				{
					RecId = ID;
					ReadById(RecId);
					BtnSaveAs.Visible = false;
					btnUpdate.Visible = false;
					BtnSaveAs.Visible = true;
					grdOutPut.DataSource = null;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InTime_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			TotalTime();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OutTime_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			TotalTime();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TotalTime()
	{
		try
		{
			if (!Conversion.CheckDateTimeNull(InTime.Value) && !Conversion.CheckDateTimeNull(OutTime.Value))
			{
				DateTime InTim = InTime.Value;
				TimeSpan totalTime = OutTime.Value - InTim;
				int days = totalTime.Days;
				int hours = totalTime.Hours;
				int minutes = totalTime.Minutes;
				txtTotalTime.Text = $"{days} Days And {hours:D2}:{minutes:D2}";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error calculating total time: " + ex.Message);
		}
	}

	private void BtnLoadOutPut_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		try
		{
			ReportsParameters val = new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				CastingTypeId = Conversion.ToInt(CmbCastingType.Value)
			};
			if (val.CastingTypeId == 0)
			{
				CmbCastingType.Focus();
				throw new Exception("Casting type Field Required");
			}
			val.RecId = RecId;
			DataTable dt = ProductionHeader.WorkOrderSemiFinishForProductionOutput(val);
			List<string> columnsToHide = new List<string> { "WorkOrderId", "workOrderItemDetailId", "ItemId", "UomId", "ItemEquivalent", "ItemVariantId", "CastingTypeId" };
			GrdPopUpToReturndt obj = new GrdPopUpToReturndt(dt, selection: true, returnFullGrdDt: false, columnsToHide);
			obj.StartPosition = FormStartPosition.CenterScreen;
			obj.WindowState = FormWindowState.Maximized;
			obj.ShowDialog();
			obj.BringToFront();
			if (obj.dtReturn.Rows.Count <= 0)
			{
				return;
			}
			foreach (DataRow dr in obj.dtReturn.Rows)
			{
				bool flag = false;
				if (grdOutPut.GetRows().Length != 0)
				{
					GridEXRow[] rows = grdOutPut.GetRows();
					for (int i = 0; i < rows.Length; i++)
					{
						if (Conversion.ToInt(rows[i].Cells["workOrderItemDetailId"].Value) == Conversion.ToInt(dr["workOrderItemDetailId"]))
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					dtdetailOutput.Rows.Add(0, Conversion.ToInt(dr["WorkOrderId"]), Conversion.ToInt(dr["workOrderItemDetailId"]), Conversion.ToString(dr["WorkOrderNo"]), DefaultCastingWarehouseId, Conversion.ToInt(dr["ItemId"]), Conversion.ToString(dr["ItemName"]), Conversion.ToInt(dr["UomId"]), Conversion.ToString(dr["ItemUom"]), Conversion.ToDouble(dr["ItemEquivalent"]), Conversion.ToInt(dr["ItemVariantId"]), Conversion.ToString(dr["ModelDescription"]), DefaultProductionStageId, DefaultProductionStageName, Conversion.ToDouble(dr["BalQty"]), Conversion.ToDouble(dr["BalQty"]), Conversion.ToDouble(dr["ItemWeight"]), Conversion.ToDouble(dr["BalQty"]) * Conversion.ToDouble(dr["ItemWeight"]), 0, Conversion.ToInt(dr["UomId"]), Conversion.ToString(dr["ItemUom"]), Conversion.ToDouble(dr["ItemEquivalent"]), 0);
				}
			}
			grdOutPut.DataSource = dtdetailOutput;
			grdOutPut.RetrieveStructure();
			grdOutPutSettings();
			((Control)(object)CmbCastingType).Enabled = !grdOutPut.GetRows().Any((GridEXRow rrr) => Convert.ToInt32(rrr.Cells["WorkOrderId"].Value) > 0);
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
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e5: Expected O, but got Unknown
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Expected O, but got Unknown
		//IL_023e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Expected O, but got Unknown
		//IL_0254: Unknown result type (might be due to invalid IL or missing references)
		//IL_025e: Expected O, but got Unknown
		//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c1: Expected O, but got Unknown
		//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0303: Expected O, but got Unknown
		//IL_03ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d4: Expected O, but got Unknown
		//IL_03f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0400: Expected O, but got Unknown
		//IL_0401: Unknown result type (might be due to invalid IL or missing references)
		//IL_040b: Expected O, but got Unknown
		//IL_056c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0576: Expected O, but got Unknown
		//IL_058d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0597: Expected O, but got Unknown
		//IL_05a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ad: Expected O, but got Unknown
		//IL_05b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c3: Expected O, but got Unknown
		//IL_05cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d9: Expected O, but got Unknown
		//IL_0d14: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d1e: Expected O, but got Unknown
		//IL_0d2b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d35: Expected O, but got Unknown
		//IL_0eaf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eb9: Expected O, but got Unknown
		//IL_0ec6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ed0: Expected O, but got Unknown
		//IL_1344: Unknown result type (might be due to invalid IL or missing references)
		//IL_134e: Expected O, but got Unknown
		//IL_135b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1365: Expected O, but got Unknown
		//IL_14e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_14ea: Expected O, but got Unknown
		//IL_14f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1501: Expected O, but got Unknown
		//IL_4f0e: Unknown result type (might be due to invalid IL or missing references)
		//IL_4f18: Expected O, but got Unknown
		//IL_4f3c: Unknown result type (might be due to invalid IL or missing references)
		//IL_4f46: Expected O, but got Unknown
		GridEXLayout grdInput_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdOverHead_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdOutPut_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdScrap_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdSummary_DesignTimeLayout = new GridEXLayout();
		Appearance appearance5 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance13 = new Appearance();
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
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		GridEXLayout GrdHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdInputHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdOutputHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdScrapHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdOverHeadHistory_DesignTimeLayout = new GridEXLayout();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Mfg.frmProductionEngr));
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.tabInPut = new System.Windows.Forms.TabPage();
		this.grdInput = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.grdOverHead = new GridEX();
		this.panel14 = new System.Windows.Forms.Panel();
		this.label22 = new System.Windows.Forms.Label();
		this.panel10 = new System.Windows.Forms.Panel();
		this.label21 = new System.Windows.Forms.Label();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.tabOutPut = new System.Windows.Forms.TabPage();
		this.grdOutPut = new GridEX();
		this.panel12 = new System.Windows.Forms.Panel();
		this.grdScrap = new GridEX();
		this.panel15 = new System.Windows.Forms.Panel();
		this.label1 = new System.Windows.Forms.Label();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label15 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.BtnLoadOutPut = new System.Windows.Forms.Button();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel11 = new System.Windows.Forms.Panel();
		this.grdSummary = new GridEX();
		this.panel16 = new System.Windows.Forms.Panel();
		this.label30 = new System.Windows.Forms.Label();
		this.ctrlGrdBar4 = new grid.User_Conrtols.CtrlGrdBar();
		this.labelGainLossPercent = new System.Windows.Forms.Label();
		this.labelGainLossCaption = new System.Windows.Forms.Label();
		this.CmbCastingType = new UltraCombo();
		this.label77 = new System.Windows.Forms.Label();
		this.panel9 = new System.Windows.Forms.Panel();
		this.label6 = new System.Windows.Forms.Label();
		this.txtActualtotalOutPutValue = new System.Windows.Forms.TextBox();
		this.txtExpectedTotalOutputValue = new System.Windows.Forms.TextBox();
		this.label28 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.panel20 = new System.Windows.Forms.Panel();
		this.label3 = new System.Windows.Forms.Label();
		this.txtTotalInputs = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.txtOverHead = new System.Windows.Forms.TextBox();
		this.txtLessTotalScrap = new System.Windows.Forms.TextBox();
		this.label26 = new System.Windows.Forms.Label();
		this.BtnLoadInputFromBom = new System.Windows.Forms.Button();
		this.CmbBillOfMaterial = new UltraCombo();
		this.label25 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.txtBatchDescription = new System.Windows.Forms.TextBox();
		this.CmbJobLot = new UltraCombo();
		this.CmbHeatCode = new UltraCombo();
		this.txtTotalTime = new System.Windows.Forms.TextBox();
		this.label29 = new System.Windows.Forms.Label();
		this.OutTime = new System.Windows.Forms.DateTimePicker();
		this.label20 = new System.Windows.Forms.Label();
		this.InTime = new System.Windows.Forms.DateTimePicker();
		this.label16 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.panel13 = new System.Windows.Forms.Panel();
		this.label27 = new System.Windows.Forms.Label();
		this.ChkSettlement = new System.Windows.Forms.CheckBox();
		this.ChkBox = new System.Windows.Forms.CheckBox();
		this.label24 = new System.Windows.Forms.Label();
		this.txtremarks = new System.Windows.Forms.TextBox();
		this.txtProductionNo = new System.Windows.Forms.TextBox();
		this.DocDate = new System.Windows.Forms.DateTimePicker();
		this.label18 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.BtnRefresh = new System.Windows.Forms.ToolStripButton();
		this.BtnSaveAs = new System.Windows.Forms.ToolStripButton();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.BtnDelete = new System.Windows.Forms.ToolStripButton();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.btnPrint1619 = new System.Windows.Forms.ToolStripButton();
		this.btnVoucherReport = new System.Windows.Forms.ToolStripButton();
		this.btnshortcutkeys = new System.Windows.Forms.ToolStripButton();
		this.BtnGenerate = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.GrdHistory = new GridEX();
		this.tabControl3 = new System.Windows.Forms.TabControl();
		this.tabPage4 = new System.Windows.Forms.TabPage();
		this.grdInputHistory = new GridEX();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.grdOutputHistory = new GridEX();
		this.tabPage5 = new System.Windows.Forms.TabPage();
		this.grdScrapHistory = new GridEX();
		this.tabPage6 = new System.Windows.Forms.TabPage();
		this.grdOverHeadHistory = new GridEX();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label13 = new System.Windows.Forms.Label();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel8 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.label9 = new System.Windows.Forms.Label();
		this.datFromDate = new System.Windows.Forms.DateTimePicker();
		this.btnshow = new System.Windows.Forms.Button();
		this.txtDocNoTo = new System.Windows.Forms.TextBox();
		this.label11 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.txtDocNoFrom = new System.Windows.Forms.TextBox();
		this.datToDate = new System.Windows.Forms.DateTimePicker();
		this.panel7 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.BtnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.BtnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.tabInPut.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdInput).BeginInit();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdOverHead).BeginInit();
		this.panel14.SuspendLayout();
		this.panel10.SuspendLayout();
		this.tabOutPut.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdOutPut).BeginInit();
		this.panel12.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdScrap).BeginInit();
		this.panel15.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel11.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdSummary).BeginInit();
		this.panel16.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCastingType).BeginInit();
		this.panel9.SuspendLayout();
		this.panel20.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBillOfMaterial).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbHeatCode).BeginInit();
		this.panel13.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdHistory).BeginInit();
		this.tabControl3.SuspendLayout();
		this.tabPage4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdInputHistory).BeginInit();
		this.tabPage3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdOutputHistory).BeginInit();
		this.tabPage5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdScrapHistory).BeginInit();
		this.tabPage6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdOverHeadHistory).BeginInit();
		this.panel3.SuspendLayout();
		this.panel8.SuspendLayout();
		this.groupBox1.SuspendLayout();
		this.panel7.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(964, 611);
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
		this.tabControl1.Size = new System.Drawing.Size(964, 611);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.BackColor = System.Drawing.Color.White;
		this.tabPage1.Controls.Add(this.tabControl2);
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage1.Size = new System.Drawing.Size(956, 582);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabControl2.Controls.Add(this.tabInPut);
		this.tabControl2.Controls.Add(this.tabOutPut);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl2.Location = new System.Drawing.Point(2, 228);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(952, 352);
		this.tabControl2.TabIndex = 6;
		this.tabInPut.Controls.Add((System.Windows.Forms.Control)(object)this.grdInput);
		this.tabInPut.Controls.Add(this.panel5);
		this.tabInPut.Controls.Add(this.panel10);
		this.tabInPut.Location = new System.Drawing.Point(4, 25);
		this.tabInPut.Name = "tabInPut";
		this.tabInPut.Padding = new System.Windows.Forms.Padding(3);
		this.tabInPut.Size = new System.Drawing.Size(944, 323);
		this.tabInPut.TabIndex = 1;
		this.tabInPut.Text = "Input && Overhead";
		this.tabInPut.UseVisualStyleBackColor = true;
		((ScrollableBase)this.grdInput).BackColor = System.Drawing.Color.White;
		grdInput_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdInput.DesignTimeLayout = grdInput_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdInput).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdInput.FilterRowButtonStyle = (FilterRowButtonStyle)1;
		((System.Windows.Forms.Control)(object)this.grdInput).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdInput.GroupByBoxFormatStyle.BackColorGradient = System.Drawing.Color.White;
		this.grdInput.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdInput).Location = new System.Drawing.Point(3, 28);
		((System.Windows.Forms.Control)(object)this.grdInput).Name = "grdInput";
		((ScrollableBase)this.grdInput).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdInput).Size = new System.Drawing.Size(938, 110);
		((System.Windows.Forms.Control)(object)this.grdInput).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.grdInput).TabStop = false;
		this.grdInput.TotalRow = (InheritableBoolean)1;
		this.grdInput.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdInput).VisualStyle = (VisualStyle)4;
		this.grdInput.CellUpdated += new ColumnActionEventHandler(grdInput_CellUpdated);
		this.grdInput.ColumnButtonClick += new ColumnActionEventHandler(grdInput_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdInput).KeyDown += new System.Windows.Forms.KeyEventHandler(grdInput_KeyDown);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.grdOverHead);
		this.panel5.Controls.Add(this.panel14);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel5.Location = new System.Drawing.Point(3, 138);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(938, 182);
		this.panel5.TabIndex = 6;
		((ScrollableBase)this.grdOverHead).BackColor = System.Drawing.Color.White;
		grdOverHead_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdOverHead.DesignTimeLayout = grdOverHead_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdOverHead).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdOverHead).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdOverHead.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdOverHead).Location = new System.Drawing.Point(0, 26);
		((System.Windows.Forms.Control)(object)this.grdOverHead).Name = "grdOverHead";
		((ScrollableBase)this.grdOverHead).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdOverHead).Size = new System.Drawing.Size(938, 156);
		((System.Windows.Forms.Control)(object)this.grdOverHead).TabIndex = 0;
		this.grdOverHead.TotalRow = (InheritableBoolean)1;
		this.grdOverHead.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdOverHead).VisualStyle = (VisualStyle)4;
		this.grdOverHead.CellUpdated += new ColumnActionEventHandler(grdOverHead_CellUpdated);
		this.grdOverHead.ColumnButtonClick += new ColumnActionEventHandler(grdOverHead_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdOverHead).KeyDown += new System.Windows.Forms.KeyEventHandler(grdOverHead_KeyDown);
		this.panel14.BackColor = System.Drawing.Color.Teal;
		this.panel14.Controls.Add(this.label22);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel14.Location = new System.Drawing.Point(0, 0);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(938, 26);
		this.panel14.TabIndex = 13;
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.ForeColor = System.Drawing.Color.White;
		this.label22.Location = new System.Drawing.Point(6, 5);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(71, 16);
		this.label22.TabIndex = 126;
		this.label22.Text = "OverHead";
		this.panel10.BackColor = System.Drawing.Color.Teal;
		this.panel10.Controls.Add(this.label21);
		this.panel10.Controls.Add(this.ctrlGrdBar3);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(3, 3);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(938, 25);
		this.panel10.TabIndex = 5;
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label21.ForeColor = System.Drawing.Color.White;
		this.label21.Location = new System.Drawing.Point(5, 3);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(90, 17);
		this.label21.TabIndex = 24;
		this.label21.Text = "Input Detail";
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(892, 2);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.grdInput;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar3.TabIndex = 1;
		this.tabOutPut.Controls.Add((System.Windows.Forms.Control)(object)this.grdOutPut);
		this.tabOutPut.Controls.Add(this.panel12);
		this.tabOutPut.Controls.Add(this.panel6);
		this.tabOutPut.Location = new System.Drawing.Point(4, 25);
		this.tabOutPut.Name = "tabOutPut";
		this.tabOutPut.Padding = new System.Windows.Forms.Padding(3);
		this.tabOutPut.Size = new System.Drawing.Size(944, 323);
		this.tabOutPut.TabIndex = 0;
		this.tabOutPut.Text = "OutPut && Scrap";
		this.tabOutPut.UseVisualStyleBackColor = true;
		((ScrollableBase)this.grdOutPut).BackColor = System.Drawing.Color.White;
		grdOutPut_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdOutPut.DesignTimeLayout = grdOutPut_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdOutPut).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdOutPut.FilterRowButtonStyle = (FilterRowButtonStyle)1;
		((System.Windows.Forms.Control)(object)this.grdOutPut).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdOutPut.GroupByBoxFormatStyle.BackColorGradient = System.Drawing.Color.White;
		this.grdOutPut.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdOutPut).Location = new System.Drawing.Point(3, 28);
		((System.Windows.Forms.Control)(object)this.grdOutPut).Name = "grdOutPut";
		((ScrollableBase)this.grdOutPut).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdOutPut).Size = new System.Drawing.Size(938, 109);
		((System.Windows.Forms.Control)(object)this.grdOutPut).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.grdOutPut).TabStop = false;
		this.grdOutPut.TotalRow = (InheritableBoolean)1;
		this.grdOutPut.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdOutPut).VisualStyle = (VisualStyle)4;
		this.grdOutPut.CellUpdated += new ColumnActionEventHandler(grdOutPut_CellUpdated);
		this.grdOutPut.ColumnButtonClick += new ColumnActionEventHandler(grdOutPut_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdOutPut).KeyDown += new System.Windows.Forms.KeyEventHandler(grdOutPut_KeyDown);
		this.panel12.Controls.Add((System.Windows.Forms.Control)(object)this.grdScrap);
		this.panel12.Controls.Add(this.panel15);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel12.Location = new System.Drawing.Point(3, 137);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(938, 183);
		this.panel12.TabIndex = 3;
		((ScrollableBase)this.grdScrap).BackColor = System.Drawing.Color.White;
		grdScrap_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdScrap.DesignTimeLayout = grdScrap_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdScrap).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdScrap).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdScrap.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdScrap).Location = new System.Drawing.Point(0, 27);
		((System.Windows.Forms.Control)(object)this.grdScrap).Name = "grdScrap";
		((ScrollableBase)this.grdScrap).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdScrap).Size = new System.Drawing.Size(938, 156);
		((System.Windows.Forms.Control)(object)this.grdScrap).TabIndex = 11;
		this.grdScrap.TotalRow = (InheritableBoolean)1;
		this.grdScrap.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdScrap).VisualStyle = (VisualStyle)4;
		this.grdScrap.CellUpdated += new ColumnActionEventHandler(grdScrap_CellUpdated);
		this.grdScrap.ColumnButtonClick += new ColumnActionEventHandler(grdScrap_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdScrap).KeyDown += new System.Windows.Forms.KeyEventHandler(grdScrap_KeyDown);
		this.panel15.BackColor = System.Drawing.Color.Teal;
		this.panel15.Controls.Add(this.label1);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.Location = new System.Drawing.Point(0, 0);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(938, 27);
		this.panel15.TabIndex = 10;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(7, 6);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(172, 16);
		this.label1.TabIndex = 126;
		this.label1.Text = "OutPut by Product/Scrap";
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.label15);
		this.panel6.Controls.Add(this.ctrlGrdBar1);
		this.panel6.Controls.Add(this.BtnLoadOutPut);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(3, 3);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(938, 25);
		this.panel6.TabIndex = 2;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label15.ForeColor = System.Drawing.Color.White;
		this.label15.Location = new System.Drawing.Point(5, 3);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(100, 17);
		this.label15.TabIndex = 24;
		this.label15.Text = "Output Detail";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(952, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdOutPut;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 1;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.BtnLoadOutPut.BackColor = System.Drawing.Color.FromArgb(0, 64, 64);
		this.BtnLoadOutPut.FlatAppearance.BorderSize = 0;
		this.BtnLoadOutPut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnLoadOutPut.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.BtnLoadOutPut.ForeColor = System.Drawing.Color.White;
		this.BtnLoadOutPut.Location = new System.Drawing.Point(111, 1);
		this.BtnLoadOutPut.Name = "BtnLoadOutPut";
		this.BtnLoadOutPut.Size = new System.Drawing.Size(141, 23);
		this.BtnLoadOutPut.TabIndex = 11;
		this.BtnLoadOutPut.Text = "Loader For Ouput";
		this.BtnLoadOutPut.UseVisualStyleBackColor = false;
		this.BtnLoadOutPut.Click += new System.EventHandler(BtnLoadOutPut_Click);
		this.panel4.Controls.Add(this.panel11);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCastingType);
		this.panel4.Controls.Add(this.label77);
		this.panel4.Controls.Add(this.panel9);
		this.panel4.Controls.Add(this.BtnLoadInputFromBom);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBillOfMaterial);
		this.panel4.Controls.Add(this.label25);
		this.panel4.Controls.Add(this.label19);
		this.panel4.Controls.Add(this.txtBatchDescription);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobLot);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbHeatCode);
		this.panel4.Controls.Add(this.txtTotalTime);
		this.panel4.Controls.Add(this.label29);
		this.panel4.Controls.Add(this.OutTime);
		this.panel4.Controls.Add(this.label20);
		this.panel4.Controls.Add(this.InTime);
		this.panel4.Controls.Add(this.label16);
		this.panel4.Controls.Add(this.label17);
		this.panel4.Controls.Add(this.panel13);
		this.panel4.Controls.Add(this.label24);
		this.panel4.Controls.Add(this.txtremarks);
		this.panel4.Controls.Add(this.txtProductionNo);
		this.panel4.Controls.Add(this.DocDate);
		this.panel4.Controls.Add(this.label18);
		this.panel4.Controls.Add(this.label5);
		this.panel4.Controls.Add(this.txtdocno);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(2, 33);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(952, 195);
		this.panel4.TabIndex = 0;
		this.panel11.Controls.Add((System.Windows.Forms.Control)(object)this.grdSummary);
		this.panel11.Controls.Add(this.panel16);
		this.panel11.Location = new System.Drawing.Point(544, 28);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(393, 162);
		this.panel11.TabIndex = 605;
		this.grdSummary.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdSummary).BackColor = System.Drawing.Color.White;
		grdSummary_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdSummary.DesignTimeLayout = grdSummary_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdSummary).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdSummary.FilterRowButtonStyle = (FilterRowButtonStyle)1;
		((System.Windows.Forms.Control)(object)this.grdSummary).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdSummary.GroupByBoxFormatStyle.BackColorGradient = System.Drawing.Color.White;
		this.grdSummary.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdSummary).Location = new System.Drawing.Point(0, 25);
		((System.Windows.Forms.Control)(object)this.grdSummary).Name = "grdSummary";
		((ScrollableBase)this.grdSummary).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdSummary).Size = new System.Drawing.Size(393, 137);
		((System.Windows.Forms.Control)(object)this.grdSummary).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.grdSummary).TabStop = false;
		this.grdSummary.TotalRow = (InheritableBoolean)1;
		this.grdSummary.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdSummary).VisualStyle = (VisualStyle)4;
		this.panel16.BackColor = System.Drawing.Color.White;
		this.panel16.Controls.Add(this.label30);
		this.panel16.Controls.Add(this.ctrlGrdBar4);
		this.panel16.Controls.Add(this.labelGainLossPercent);
		this.panel16.Controls.Add(this.labelGainLossCaption);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel16.Location = new System.Drawing.Point(0, 0);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(393, 25);
		this.panel16.TabIndex = 4;
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label30.ForeColor = System.Drawing.Color.Teal;
		this.label30.Location = new System.Drawing.Point(5, 3);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(74, 17);
		this.label30.TabIndex = 24;
		this.label30.Text = "Summary";
		this.ctrlGrdBar4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar4.FormName = this;
		this.ctrlGrdBar4.Location = new System.Drawing.Point(407, 2);
		this.ctrlGrdBar4.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar4.MyGrid = this.grdSummary;
		this.ctrlGrdBar4.Name = "ctrlGrdBar4";
		this.ctrlGrdBar4.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar4.TabIndex = 1;
		this.labelGainLossPercent.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.labelGainLossPercent.AutoSize = true;
		this.labelGainLossPercent.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.labelGainLossPercent.ForeColor = System.Drawing.Color.Teal;
		this.labelGainLossPercent.Location = new System.Drawing.Point(298, 6);
		this.labelGainLossPercent.Name = "labelGainLossPercent";
		this.labelGainLossPercent.Size = new System.Drawing.Size(22, 14);
		this.labelGainLossPercent.TabIndex = 601;
		this.labelGainLossPercent.Text = "%";
		this.labelGainLossCaption.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.labelGainLossCaption.AutoSize = true;
		this.labelGainLossCaption.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.labelGainLossCaption.ForeColor = System.Drawing.Color.Teal;
		this.labelGainLossCaption.Location = new System.Drawing.Point(210, 6);
		this.labelGainLossCaption.Name = "labelGainLossCaption";
		this.labelGainLossCaption.Size = new System.Drawing.Size(83, 14);
		this.labelGainLossCaption.TabIndex = 601;
		this.labelGainLossCaption.Text = "Gain / Loss";
		this.CmbCastingType.AllowNull = (DefaultableBoolean)1;
		this.CmbCastingType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCastingType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCastingType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCastingType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbCastingType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCastingType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance9).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance9).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance9).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCastingType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCastingType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance10;
		((SpecialBoxBase)((UltraGridBase)this.CmbCastingType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance11).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance11).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance11).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCastingType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbCastingType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCastingType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCastingType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance13;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance14).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCastingType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.CmbCastingType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCastingType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCastingType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance15;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance16).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCastingType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbCastingType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCastingType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance17).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance17).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance17).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCastingType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCastingType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbCastingType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCastingType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCastingType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.CmbCastingType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCastingType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbCastingType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCastingType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCastingType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCastingType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCastingType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCastingType).Location = new System.Drawing.Point(95, 137);
		((System.Windows.Forms.Control)(object)this.CmbCastingType).Name = "CmbCastingType";
		((System.Windows.Forms.Control)(object)this.CmbCastingType).Size = new System.Drawing.Size(181, 26);
		((System.Windows.Forms.Control)(object)this.CmbCastingType).TabIndex = 5;
		((System.Windows.Forms.Control)(object)this.CmbCastingType).Leave += new System.EventHandler(CmbCastingType_Leave);
		this.label77.AutoSize = true;
		this.label77.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label77.Location = new System.Drawing.Point(6, 141);
		this.label77.Name = "label77";
		this.label77.Size = new System.Drawing.Size(74, 15);
		this.label77.TabIndex = 604;
		this.label77.Text = "Casting Type";
		this.panel9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel9.Controls.Add(this.label6);
		this.panel9.Controls.Add(this.txtActualtotalOutPutValue);
		this.panel9.Controls.Add(this.txtExpectedTotalOutputValue);
		this.panel9.Controls.Add(this.label28);
		this.panel9.Controls.Add(this.label2);
		this.panel9.Controls.Add(this.panel20);
		this.panel9.Controls.Add(this.txtTotalInputs);
		this.panel9.Controls.Add(this.label4);
		this.panel9.Controls.Add(this.label7);
		this.panel9.Controls.Add(this.label12);
		this.panel9.Controls.Add(this.label14);
		this.panel9.Controls.Add(this.txtOverHead);
		this.panel9.Controls.Add(this.txtLessTotalScrap);
		this.panel9.Controls.Add(this.label26);
		this.panel9.Location = new System.Drawing.Point(1003, 126);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(124, 61);
		this.panel9.TabIndex = 602;
		this.panel9.Visible = false;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(5, 30);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(80, 17);
		this.label6.TabIndex = 91;
		this.label6.Text = "Total Inputs";
		this.txtActualtotalOutPutValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtActualtotalOutPutValue.Font = new System.Drawing.Font("Verdana", 10.5f, System.Drawing.FontStyle.Bold);
		this.txtActualtotalOutPutValue.Location = new System.Drawing.Point(150, 135);
		this.txtActualtotalOutPutValue.Name = "txtActualtotalOutPutValue";
		this.txtActualtotalOutPutValue.ReadOnly = true;
		this.txtActualtotalOutPutValue.Size = new System.Drawing.Size(147, 25);
		this.txtActualtotalOutPutValue.TabIndex = 3;
		this.txtExpectedTotalOutputValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtExpectedTotalOutputValue.Font = new System.Drawing.Font("Verdana", 10.5f, System.Drawing.FontStyle.Bold);
		this.txtExpectedTotalOutputValue.Location = new System.Drawing.Point(150, 109);
		this.txtExpectedTotalOutputValue.Name = "txtExpectedTotalOutputValue";
		this.txtExpectedTotalOutputValue.ReadOnly = true;
		this.txtExpectedTotalOutputValue.Size = new System.Drawing.Size(147, 25);
		this.txtExpectedTotalOutputValue.TabIndex = 3;
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.Location = new System.Drawing.Point(5, 138);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(130, 17);
		this.label28.TabIndex = 97;
		this.label28.Text = "Actual OutPut Value";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(5, 112);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(147, 17);
		this.label2.TabIndex = 97;
		this.label2.Text = "Expected OutPut Value";
		this.panel20.BackColor = System.Drawing.Color.Teal;
		this.panel20.Controls.Add(this.label3);
		this.panel20.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel20.Location = new System.Drawing.Point(0, 0);
		this.panel20.Name = "panel20";
		this.panel20.Size = new System.Drawing.Size(122, 23);
		this.panel20.TabIndex = 1;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.White;
		this.label3.Location = new System.Drawing.Point(5, 4);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(141, 16);
		this.label3.TabIndex = 0;
		this.label3.Text = "Calculation Summery";
		this.txtTotalInputs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTotalInputs.Font = new System.Drawing.Font("Verdana", 10.5f, System.Drawing.FontStyle.Bold);
		this.txtTotalInputs.Location = new System.Drawing.Point(150, 25);
		this.txtTotalInputs.Name = "txtTotalInputs";
		this.txtTotalInputs.ReadOnly = true;
		this.txtTotalInputs.Size = new System.Drawing.Size(147, 25);
		this.txtTotalInputs.TabIndex = 0;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(5, 56);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(63, 17);
		this.label4.TabIndex = 95;
		this.label4.Text = "Total O.H";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(14, 99);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(18, 16);
		this.label7.TabIndex = 93;
		this.label7.Text = "=";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(14, 72);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(14, 16);
		this.label12.TabIndex = 93;
		this.label12.Text = "-";
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(14, 42);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(18, 16);
		this.label14.TabIndex = 93;
		this.label14.Text = "+";
		this.txtOverHead.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOverHead.Font = new System.Drawing.Font("Verdana", 10.5f, System.Drawing.FontStyle.Bold);
		this.txtOverHead.Location = new System.Drawing.Point(150, 52);
		this.txtOverHead.Name = "txtOverHead";
		this.txtOverHead.ReadOnly = true;
		this.txtOverHead.Size = new System.Drawing.Size(147, 25);
		this.txtOverHead.TabIndex = 1;
		this.txtLessTotalScrap.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtLessTotalScrap.Font = new System.Drawing.Font("Verdana", 10.5f, System.Drawing.FontStyle.Bold);
		this.txtLessTotalScrap.Location = new System.Drawing.Point(150, 80);
		this.txtLessTotalScrap.Name = "txtLessTotalScrap";
		this.txtLessTotalScrap.ReadOnly = true;
		this.txtLessTotalScrap.Size = new System.Drawing.Size(147, 25);
		this.txtLessTotalScrap.TabIndex = 2;
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(5, 85);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(41, 17);
		this.label26.TabIndex = 93;
		this.label26.Text = "Scrap";
		this.BtnLoadInputFromBom.BackColor = System.Drawing.Color.FromArgb(0, 64, 64);
		this.BtnLoadInputFromBom.FlatAppearance.BorderSize = 0;
		this.BtnLoadInputFromBom.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnLoadInputFromBom.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.BtnLoadInputFromBom.ForeColor = System.Drawing.Color.White;
		this.BtnLoadInputFromBom.Location = new System.Drawing.Point(368, 137);
		this.BtnLoadInputFromBom.Name = "BtnLoadInputFromBom";
		this.BtnLoadInputFromBom.Size = new System.Drawing.Size(173, 26);
		this.BtnLoadInputFromBom.TabIndex = 11;
		this.BtnLoadInputFromBom.Text = "Load Input From BOM";
		this.BtnLoadInputFromBom.UseVisualStyleBackColor = false;
		this.BtnLoadInputFromBom.Click += new System.EventHandler(BtnLoadInputFromBom_Click);
		this.CmbBillOfMaterial.AllowNull = (DefaultableBoolean)1;
		this.CmbBillOfMaterial.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBillOfMaterial.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBillOfMaterial.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBillOfMaterial).DisplayLayout.Appearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbBillOfMaterial).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBillOfMaterial).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance23).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance23).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBillOfMaterial).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBillOfMaterial).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance24;
		((SpecialBoxBase)((UltraGridBase)this.CmbBillOfMaterial).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance25).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance25).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance25).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBillOfMaterial).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbBillOfMaterial).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBillOfMaterial).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance27).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBillOfMaterial).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBillOfMaterial).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbBillOfMaterial).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBillOfMaterial).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBillOfMaterial).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance29;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance30).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBillOfMaterial).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbBillOfMaterial).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBillOfMaterial).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance31).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance31).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance31).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBillOfMaterial).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance32).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBillOfMaterial).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.CmbBillOfMaterial).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBillOfMaterial).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBillOfMaterial).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbBillOfMaterial).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBillOfMaterial).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.CmbBillOfMaterial).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBillOfMaterial).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBillOfMaterial).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBillOfMaterial).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBillOfMaterial.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBillOfMaterial).Location = new System.Drawing.Point(368, 108);
		((System.Windows.Forms.Control)(object)this.CmbBillOfMaterial).Name = "CmbBillOfMaterial";
		((System.Windows.Forms.Control)(object)this.CmbBillOfMaterial).Size = new System.Drawing.Size(173, 26);
		((System.Windows.Forms.Control)(object)this.CmbBillOfMaterial).TabIndex = 10;
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(281, 114);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(85, 15);
		this.label25.TabIndex = 601;
		this.label25.Text = "Bill Of Material";
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(6, 86);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(67, 15);
		this.label19.TabIndex = 600;
		this.label19.Text = "Batch Code";
		this.txtBatchDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBatchDescription.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBatchDescription.Location = new System.Drawing.Point(281, 139);
		this.txtBatchDescription.Name = "txtBatchDescription";
		this.txtBatchDescription.Size = new System.Drawing.Size(85, 23);
		this.txtBatchDescription.TabIndex = 3;
		this.txtBatchDescription.Visible = false;
		this.txtBatchDescription.WordWrap = false;
		this.CmbJobLot.AllowNull = (DefaultableBoolean)1;
		this.CmbJobLot.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobLot.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobLot.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance35;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance43).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance43).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbJobLot.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Location = new System.Drawing.Point(95, 80);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Name = "CmbJobLot";
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Size = new System.Drawing.Size(181, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).TabIndex = 3;
		this.CmbHeatCode.AllowNull = (DefaultableBoolean)1;
		this.CmbHeatCode.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbHeatCode.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbHeatCode.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbHeatCode).DisplayLayout.Appearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.CmbHeatCode).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbHeatCode).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance7).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance7).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbHeatCode).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance7;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbHeatCode).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance8;
		((SpecialBoxBase)((UltraGridBase)this.CmbHeatCode).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbHeatCode).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbHeatCode).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbHeatCode).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbHeatCode).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbHeatCode).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbHeatCode).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbHeatCode).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbHeatCode).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance37;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance38).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbHeatCode).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbHeatCode).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbHeatCode).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance39).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance39).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance39).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbHeatCode).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance39;
		((AppearanceBase)appearance40).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbHeatCode).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.CmbHeatCode).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbHeatCode).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbHeatCode).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.CmbHeatCode).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbHeatCode).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbHeatCode).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbHeatCode).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbHeatCode).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbHeatCode).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbHeatCode.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbHeatCode).Location = new System.Drawing.Point(95, 108);
		((System.Windows.Forms.Control)(object)this.CmbHeatCode).Name = "CmbHeatCode";
		((System.Windows.Forms.Control)(object)this.CmbHeatCode).Size = new System.Drawing.Size(181, 26);
		((System.Windows.Forms.Control)(object)this.CmbHeatCode).TabIndex = 4;
		this.txtTotalTime.Enabled = false;
		this.txtTotalTime.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtTotalTime.Location = new System.Drawing.Point(368, 82);
		this.txtTotalTime.Name = "txtTotalTime";
		this.txtTotalTime.Size = new System.Drawing.Size(173, 22);
		this.txtTotalTime.TabIndex = 9;
		this.txtTotalTime.TabStop = false;
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.Location = new System.Drawing.Point(281, 86);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(63, 15);
		this.label29.TabIndex = 596;
		this.label29.Text = "Total Time";
		this.OutTime.CustomFormat = "dd-MMM-yy hh:mm tt";
		this.OutTime.Font = new System.Drawing.Font("Verdana", 9f);
		this.OutTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.OutTime.Location = new System.Drawing.Point(368, 55);
		this.OutTime.Name = "OutTime";
		this.OutTime.Size = new System.Drawing.Size(173, 22);
		this.OutTime.TabIndex = 8;
		this.OutTime.ValueChanged += new System.EventHandler(OutTime_ValueChanged);
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(281, 59);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(57, 15);
		this.label20.TabIndex = 596;
		this.label20.Text = "Out Time";
		this.InTime.CustomFormat = "dd-MMM-yy hh:mm tt";
		this.InTime.Font = new System.Drawing.Font("Verdana", 9f);
		this.InTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.InTime.Location = new System.Drawing.Point(368, 30);
		this.InTime.Name = "InTime";
		this.InTime.Size = new System.Drawing.Size(173, 22);
		this.InTime.TabIndex = 7;
		this.InTime.ValueChanged += new System.EventHandler(InTime_ValueChanged);
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(6, 114);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(62, 15);
		this.label16.TabIndex = 594;
		this.label16.Text = "Heat Code";
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(281, 34);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(48, 15);
		this.label17.TabIndex = 595;
		this.label17.Text = "In Time";
		this.panel13.BackColor = System.Drawing.Color.Teal;
		this.panel13.Controls.Add(this.label27);
		this.panel13.Controls.Add(this.ChkSettlement);
		this.panel13.Controls.Add(this.ChkBox);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel13.Location = new System.Drawing.Point(0, 0);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(952, 26);
		this.panel13.TabIndex = 584;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.ForeColor = System.Drawing.Color.White;
		this.label27.Location = new System.Drawing.Point(5, 3);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(97, 19);
		this.label27.TabIndex = 24;
		this.label27.Text = "Production";
		this.ChkSettlement.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkSettlement.AutoSize = true;
		this.ChkSettlement.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkSettlement.ForeColor = System.Drawing.Color.White;
		this.ChkSettlement.Location = new System.Drawing.Point(621, 3);
		this.ChkSettlement.Margin = new System.Windows.Forms.Padding(2);
		this.ChkSettlement.Name = "ChkSettlement";
		this.ChkSettlement.Size = new System.Drawing.Size(175, 20);
		this.ChkSettlement.TabIndex = 0;
		this.ChkSettlement.TabStop = false;
		this.ChkSettlement.Text = "Settlement Required";
		this.ChkSettlement.UseVisualStyleBackColor = true;
		this.ChkBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkBox.AutoSize = true;
		this.ChkBox.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkBox.ForeColor = System.Drawing.Color.White;
		this.ChkBox.Location = new System.Drawing.Point(800, 3);
		this.ChkBox.Margin = new System.Windows.Forms.Padding(2);
		this.ChkBox.Name = "ChkBox";
		this.ChkBox.Size = new System.Drawing.Size(148, 20);
		this.ChkBox.TabIndex = 0;
		this.ChkBox.TabStop = false;
		this.ChkBox.Text = "Preview On Save";
		this.ChkBox.UseVisualStyleBackColor = true;
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5f, System.Drawing.FontStyle.Bold);
		this.label24.Location = new System.Drawing.Point(6, 170);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(60, 17);
		this.label24.TabIndex = 0;
		this.label24.Text = "Remarks";
		this.txtremarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtremarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtremarks.Location = new System.Drawing.Point(95, 167);
		this.txtremarks.Name = "txtremarks";
		this.txtremarks.Size = new System.Drawing.Size(446, 23);
		this.txtremarks.TabIndex = 6;
		this.txtProductionNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtProductionNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtProductionNo.Location = new System.Drawing.Point(95, 55);
		this.txtProductionNo.Name = "txtProductionNo";
		this.txtProductionNo.Size = new System.Drawing.Size(181, 23);
		this.txtProductionNo.TabIndex = 2;
		this.DocDate.CustomFormat = "dd-MMM-yy";
		this.DocDate.Font = new System.Drawing.Font("Verdana", 9f);
		this.DocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DocDate.Location = new System.Drawing.Point(165, 30);
		this.DocDate.Name = "DocDate";
		this.DocDate.Size = new System.Drawing.Size(111, 22);
		this.DocDate.TabIndex = 1;
		this.DocDate.TabStop = false;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(6, 59);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(66, 15);
		this.label18.TabIndex = 3;
		this.label18.Text = "Manual No";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(6, 34);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(85, 15);
		this.label5.TabIndex = 3;
		this.label5.Text = "DocNo && Date";
		this.txtdocno.BackColor = System.Drawing.Color.White;
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocno.Location = new System.Drawing.Point(95, 30);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.Size = new System.Drawing.Size(69, 23);
		this.txtdocno.TabIndex = 0;
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(2, 2);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(952, 31);
		this.panel2.TabIndex = 0;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[11]
		{
			this.btnNew, this.BtnRefresh, this.BtnSaveAs, this.btnSave, this.btnUpdate, this.BtnDelete, this.btnAttachment, this.btnPrint1619, this.btnVoucherReport, this.btnshortcutkeys,
			this.BtnGenerate
		});
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(952, 27);
		this.toolStrip1.TabIndex = 0;
		this.toolStrip1.Text = "toolStrip1";
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(59, 24);
		this.btnNew.Text = "&New";
		this.btnNew.ToolTipText = "Ctrl_N";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.BtnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.BtnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefresh.Name = "BtnRefresh";
		this.BtnRefresh.Size = new System.Drawing.Size(80, 24);
		this.BtnRefresh.Text = "&Refresh";
		this.BtnRefresh.ToolTipText = "Alt_R";
		this.BtnRefresh.Click += new System.EventHandler(BtnRefresh_Click);
		this.BtnSaveAs.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.BtnSaveAs.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnSaveAs.Name = "BtnSaveAs";
		this.BtnSaveAs.Size = new System.Drawing.Size(85, 24);
		this.BtnSaveAs.Text = "&Save As";
		this.BtnSaveAs.ToolTipText = "Ctrl_F12";
		this.BtnSaveAs.Visible = false;
		this.BtnSaveAs.Click += new System.EventHandler(BtnSaveAs_Click);
		this.btnSave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(64, 24);
		this.btnSave.Text = "&Save";
		this.btnSave.ToolTipText = "Ctrl_S";
		this.btnSave.Click += new System.EventHandler(btnSave_Click);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.ToolTipText = "Ctrl_U";
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.BtnDelete.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.BtnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnDelete.Name = "BtnDelete";
		this.BtnDelete.Size = new System.Drawing.Size(73, 24);
		this.BtnDelete.Text = "&Delete";
		this.BtnDelete.ToolTipText = "Ctrl_S";
		this.BtnDelete.Visible = false;
		this.BtnDelete.Click += new System.EventHandler(BtnDelete_Click);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(109, 24);
		this.btnAttachment.Text = "&Attachment";
		this.btnAttachment.ToolTipText = "Alt_A";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.btnPrint1619.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint1619.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint1619.Name = "btnPrint1619";
		this.btnPrint1619.Size = new System.Drawing.Size(100, 24);
		this.btnPrint1619.Text = "1670-Print";
		this.btnPrint1619.Click += new System.EventHandler(btnPrint1619_Click);
		this.btnVoucherReport.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnVoucherReport.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnVoucherReport.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnVoucherReport.Name = "btnVoucherReport";
		this.btnVoucherReport.Size = new System.Drawing.Size(115, 24);
		this.btnVoucherReport.Text = "118-Voucher";
		this.btnVoucherReport.Click += new System.EventHandler(btnVoucherReport_Click);
		this.btnshortcutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshortcutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnshortcutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnshortcutkeys.Name = "btnshortcutkeys";
		this.btnshortcutkeys.Size = new System.Drawing.Size(126, 24);
		this.btnshortcutkeys.Text = "ShortCut Keys";
		this.btnshortcutkeys.Click += new System.EventHandler(btnshortcutkeys_Click);
		this.BtnGenerate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnGenerate.Image = Architecture.WinApp.Properties.Resources.Account_Transaction;
		this.BtnGenerate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnGenerate.Name = "BtnGenerate";
		this.BtnGenerate.Size = new System.Drawing.Size(184, 24);
		this.BtnGenerate.Text = "Generate Output Rates";
		this.BtnGenerate.Click += new System.EventHandler(BtnGenerate_Click);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.GrdHistory);
		this.tabPage2.Controls.Add(this.tabControl3);
		this.tabPage2.Controls.Add(this.panel3);
		this.tabPage2.Controls.Add(this.panel8);
		this.tabPage2.Controls.Add(this.panel7);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage2.Size = new System.Drawing.Size(956, 582);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.GrdHistory.AllowEdit = (InheritableBoolean)2;
		GrdHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GrdHistory.DesignTimeLayout = GrdHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GrdHistory.DynamicFiltering = true;
		this.GrdHistory.FilterMode = (FilterMode)2;
		this.GrdHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GrdHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdHistory.FrozenColumns = 3;
		this.GrdHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.GrdHistory).Location = new System.Drawing.Point(2, 126);
		((System.Windows.Forms.Control)(object)this.GrdHistory).Name = "GrdHistory";
		((ScrollableBase)this.GrdHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdHistory).Size = new System.Drawing.Size(952, 193);
		((System.Windows.Forms.Control)(object)this.GrdHistory).TabIndex = 0;
		this.GrdHistory.TotalRow = (InheritableBoolean)1;
		this.GrdHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdHistory).VisualStyle = (VisualStyle)4;
		this.GrdHistory.ColumnButtonClick += new ColumnActionEventHandler(GrdHistory_ColumnButtonClick);
		this.GrdHistory.SelectionChanged += new System.EventHandler(GrdHistory_SelectionChanged);
		this.GrdHistory.LinkClicked += new ColumnActionEventHandler(GrdHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.GrdHistory).DoubleClick += new System.EventHandler(GrdHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.GrdHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(GrdHistory_KeyDown);
		this.tabControl3.Controls.Add(this.tabPage4);
		this.tabControl3.Controls.Add(this.tabPage3);
		this.tabControl3.Controls.Add(this.tabPage5);
		this.tabControl3.Controls.Add(this.tabPage6);
		this.tabControl3.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.tabControl3.Location = new System.Drawing.Point(2, 319);
		this.tabControl3.Name = "tabControl3";
		this.tabControl3.SelectedIndex = 0;
		this.tabControl3.Size = new System.Drawing.Size(952, 261);
		this.tabControl3.TabIndex = 48;
		this.tabPage4.Controls.Add((System.Windows.Forms.Control)(object)this.grdInputHistory);
		this.tabPage4.Location = new System.Drawing.Point(4, 25);
		this.tabPage4.Name = "tabPage4";
		this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage4.Size = new System.Drawing.Size(944, 232);
		this.tabPage4.TabIndex = 1;
		this.tabPage4.Text = "Input";
		this.tabPage4.UseVisualStyleBackColor = true;
		this.grdInputHistory.AllowEdit = (InheritableBoolean)2;
		grdInputHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdInputHistory.DesignTimeLayout = grdInputHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdInputHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdInputHistory.DynamicFiltering = true;
		this.grdInputHistory.FilterMode = (FilterMode)2;
		this.grdInputHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdInputHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdInputHistory.GroupByBoxVisible = false;
		this.grdInputHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdInputHistory).Location = new System.Drawing.Point(3, 3);
		((System.Windows.Forms.Control)(object)this.grdInputHistory).Name = "grdInputHistory";
		((ScrollableBase)this.grdInputHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdInputHistory).Size = new System.Drawing.Size(938, 226);
		((System.Windows.Forms.Control)(object)this.grdInputHistory).TabIndex = 2;
		this.grdInputHistory.TotalRow = (InheritableBoolean)1;
		this.grdInputHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdInputHistory).VisualStyle = (VisualStyle)4;
		this.tabPage3.Controls.Add((System.Windows.Forms.Control)(object)this.grdOutputHistory);
		this.tabPage3.Location = new System.Drawing.Point(4, 25);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage3.Size = new System.Drawing.Size(944, 232);
		this.tabPage3.TabIndex = 0;
		this.tabPage3.Text = "OutPut";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.grdOutputHistory.AllowEdit = (InheritableBoolean)2;
		grdOutputHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdOutputHistory.DesignTimeLayout = grdOutputHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdOutputHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdOutputHistory.DynamicFiltering = true;
		this.grdOutputHistory.FilterMode = (FilterMode)2;
		this.grdOutputHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdOutputHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdOutputHistory.GroupByBoxVisible = false;
		this.grdOutputHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdOutputHistory).Location = new System.Drawing.Point(3, 3);
		((System.Windows.Forms.Control)(object)this.grdOutputHistory).Name = "grdOutputHistory";
		((ScrollableBase)this.grdOutputHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdOutputHistory).Size = new System.Drawing.Size(938, 226);
		((System.Windows.Forms.Control)(object)this.grdOutputHistory).TabIndex = 1;
		this.grdOutputHistory.TotalRow = (InheritableBoolean)1;
		this.grdOutputHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdOutputHistory).VisualStyle = (VisualStyle)4;
		this.tabPage5.Controls.Add((System.Windows.Forms.Control)(object)this.grdScrapHistory);
		this.tabPage5.Location = new System.Drawing.Point(4, 25);
		this.tabPage5.Name = "tabPage5";
		this.tabPage5.Size = new System.Drawing.Size(944, 232);
		this.tabPage5.TabIndex = 2;
		this.tabPage5.Text = "Scrap";
		this.tabPage5.UseVisualStyleBackColor = true;
		this.grdScrapHistory.AllowEdit = (InheritableBoolean)2;
		grdScrapHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdScrapHistory.DesignTimeLayout = grdScrapHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdScrapHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdScrapHistory.DynamicFiltering = true;
		this.grdScrapHistory.FilterMode = (FilterMode)2;
		this.grdScrapHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdScrapHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdScrapHistory.GroupByBoxVisible = false;
		this.grdScrapHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdScrapHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdScrapHistory).Name = "grdScrapHistory";
		((ScrollableBase)this.grdScrapHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdScrapHistory).Size = new System.Drawing.Size(944, 232);
		((System.Windows.Forms.Control)(object)this.grdScrapHistory).TabIndex = 2;
		this.grdScrapHistory.TotalRow = (InheritableBoolean)1;
		this.grdScrapHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdScrapHistory).VisualStyle = (VisualStyle)4;
		this.tabPage6.Controls.Add((System.Windows.Forms.Control)(object)this.grdOverHeadHistory);
		this.tabPage6.Location = new System.Drawing.Point(4, 25);
		this.tabPage6.Name = "tabPage6";
		this.tabPage6.Size = new System.Drawing.Size(944, 232);
		this.tabPage6.TabIndex = 3;
		this.tabPage6.Text = "Over Heads";
		this.tabPage6.UseVisualStyleBackColor = true;
		this.grdOverHeadHistory.AllowEdit = (InheritableBoolean)2;
		grdOverHeadHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdOverHeadHistory.DesignTimeLayout = grdOverHeadHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdOverHeadHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdOverHeadHistory.DynamicFiltering = true;
		this.grdOverHeadHistory.FilterMode = (FilterMode)2;
		this.grdOverHeadHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdOverHeadHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdOverHeadHistory.GroupByBoxVisible = false;
		this.grdOverHeadHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdOverHeadHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdOverHeadHistory).Name = "grdOverHeadHistory";
		((ScrollableBase)this.grdOverHeadHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdOverHeadHistory).Size = new System.Drawing.Size(944, 232);
		((System.Windows.Forms.Control)(object)this.grdOverHeadHistory).TabIndex = 2;
		this.grdOverHeadHistory.TotalRow = (InheritableBoolean)1;
		this.grdOverHeadHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdOverHeadHistory).VisualStyle = (VisualStyle)4;
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.label13);
		this.panel3.Controls.Add(this.ctrlGrdBar2);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(2, 101);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(952, 25);
		this.panel3.TabIndex = 45;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.ForeColor = System.Drawing.Color.White;
		this.label13.Location = new System.Drawing.Point(4, 2);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(133, 18);
		this.label13.TabIndex = 1;
		this.label13.Text = "Filtered Records";
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(911, 1);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.GrdHistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(37, 21);
		this.ctrlGrdBar2.TabIndex = 23;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.panel8.Controls.Add(this.groupBox1);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(2, 51);
		this.panel8.Margin = new System.Windows.Forms.Padding(2);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(952, 50);
		this.panel8.TabIndex = 0;
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Controls.Add(this.datFromDate);
		this.groupBox1.Controls.Add(this.btnshow);
		this.groupBox1.Controls.Add(this.txtDocNoTo);
		this.groupBox1.Controls.Add(this.label11);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add(this.label10);
		this.groupBox1.Controls.Add(this.txtDocNoFrom);
		this.groupBox1.Controls.Add(this.datToDate);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(2, 1);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(878, 47);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(5, 19);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(63, 15);
		this.label9.TabIndex = 108;
		this.label9.Text = "From Date";
		this.datFromDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datFromDate.CustomFormat = "dd-MMM-yy";
		this.datFromDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datFromDate.Location = new System.Drawing.Point(74, 15);
		this.datFromDate.Name = "datFromDate";
		this.datFromDate.ShowCheckBox = true;
		this.datFromDate.Size = new System.Drawing.Size(159, 23);
		this.datFromDate.TabIndex = 0;
		this.btnshow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(800, 14);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(66, 25);
		this.btnshow.TabIndex = 4;
		this.btnshow.Text = "Show";
		this.btnshow.TextAlign = System.Drawing.ContentAlignment.TopCenter;
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.txtDocNoTo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNoTo.Location = new System.Drawing.Point(698, 15);
		this.txtDocNoTo.Name = "txtDocNoTo";
		this.txtDocNoTo.ShortcutsEnabled = false;
		this.txtDocNoTo.Size = new System.Drawing.Size(100, 23);
		this.txtDocNoTo.TabIndex = 3;
		this.txtDocNoTo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtDocNoTo_KeyPress);
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(635, 19);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(61, 15);
		this.label11.TabIndex = 110;
		this.label11.Text = "DocNo To";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(237, 19);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(48, 15);
		this.label8.TabIndex = 107;
		this.label8.Text = "To Date";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(453, 19);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(76, 15);
		this.label10.TabIndex = 110;
		this.label10.Text = "DocNo From";
		this.txtDocNoFrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNoFrom.Location = new System.Drawing.Point(531, 15);
		this.txtDocNoFrom.Name = "txtDocNoFrom";
		this.txtDocNoFrom.ShortcutsEnabled = false;
		this.txtDocNoFrom.Size = new System.Drawing.Size(100, 23);
		this.txtDocNoFrom.TabIndex = 2;
		this.txtDocNoFrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtDocNoFrom_KeyPress);
		this.datToDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datToDate.CustomFormat = "dd-MMM-yy";
		this.datToDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datToDate.Location = new System.Drawing.Point(290, 15);
		this.datToDate.Name = "datToDate";
		this.datToDate.ShowCheckBox = true;
		this.datToDate.Size = new System.Drawing.Size(159, 23);
		this.datToDate.TabIndex = 1;
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.label23);
		this.panel7.Controls.Add(this.toolStrip2);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(2, 2);
		this.panel7.Margin = new System.Windows.Forms.Padding(2);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(952, 49);
		this.panel7.TabIndex = 0;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(4, 29);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(149, 18);
		this.label23.TabIndex = 1;
		this.label23.Text = "Production History";
		this.toolStrip2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.BtnNewHistory, this.BtnRefreshHistory });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(952, 27);
		this.toolStrip2.TabIndex = 1;
		this.toolStrip2.Text = "toolStrip2";
		this.BtnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.BtnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnNewHistory.Name = "BtnNewHistory";
		this.BtnNewHistory.Size = new System.Drawing.Size(59, 24);
		this.BtnNewHistory.Text = "&New";
		this.BtnNewHistory.ToolTipText = "Ctrl_N";
		this.BtnNewHistory.Click += new System.EventHandler(BtnNewHistory_Click);
		this.BtnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.BtnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefreshHistory.Name = "BtnRefreshHistory";
		this.BtnRefreshHistory.Size = new System.Drawing.Size(80, 24);
		this.BtnRefreshHistory.Text = "&Refresh";
		this.BtnRefreshHistory.ToolTipText = "Ctrl_R";
		this.BtnRefreshHistory.Visible = false;
		this.BtnRefreshHistory.Click += new System.EventHandler(BtnRefreshHistory_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(964, 611);
		base.Controls.Add(this.panel1);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "frmProductionEngr";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(GRNDirectAgainstOrder_FormClosing);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmProductionEngr_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.tabControl2.ResumeLayout(false);
		this.tabInPut.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdInput).EndInit();
		this.panel5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdOverHead).EndInit();
		this.panel14.ResumeLayout(false);
		this.panel14.PerformLayout();
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		this.tabOutPut.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdOutPut).EndInit();
		this.panel12.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdScrap).EndInit();
		this.panel15.ResumeLayout(false);
		this.panel15.PerformLayout();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.panel11.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdSummary).EndInit();
		this.panel16.ResumeLayout(false);
		this.panel16.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCastingType).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		this.panel20.ResumeLayout(false);
		this.panel20.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBillOfMaterial).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbHeatCode).EndInit();
		this.panel13.ResumeLayout(false);
		this.panel13.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GrdHistory).EndInit();
		this.tabControl3.ResumeLayout(false);
		this.tabPage4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdInputHistory).EndInit();
		this.tabPage3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdOutputHistory).EndInit();
		this.tabPage5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdScrapHistory).EndInit();
		this.tabPage6.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdOverHeadHistory).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel8.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}
