// using Architecture.Reports;
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
using Architecture.BLL.Lab;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Common;
using Architecture.Model.Lab;
using Architecture.Model.Main;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Inventory_Reports;
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
namespace Architecture.WinApp.ApprovalDashboard
{

public class LabPurchaseAnalysisForApproval : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DataTable dtStatus = new DataTable();

	public int BranchesId;

	private bool FieldsChanged;

	private int txtScheduleId;

	private bool RateFromScheduleOnLabForGatePurchase;

	public bool RateApprovalGatePurchase;

	public bool WeightApprovalPurchaseOrder;

	public bool RateAppliedStatus;

	public bool WeightAppliedStatus;

	private int RefDocumentTypeId;

	private int ItemId;

	private int RecId;

	private DateTime LabDocDate = DateTime.Now;

	private IContainer components;

	private ToolStripButton btnnew;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStrip toolStrip1;

	internal GridEX grdHistory;

	private Panel panel1;

	private ToolStripButton btnApproved;

	private CtrlGrdBar ctrlGrdBar1;

	private Panel panel2;

	private Label label1;

	private Panel panel14;

	private Panel panel17;

	internal GridEX grdHistroryPurAnalysis;

	private Panel panel16;

	private TextBox txtWeightCutUom;

	private TextBox txtQtyForWtCut;

	private Label label30;

	private Label label37;

	private Label label28;

	internal UltraCombo CmbJobLot;

	private Label label36;

	internal UltraCombo CmbWarehouse;

	private Label label20;

	private TextBox txtremarks;

	private TextBox txtNetRate;

	private Label lblNetRate;

	private TextBox txtLessRate;

	private CheckBox ChkRateCutWillApply;

	private CheckBox ChkWtCutWillApply;

	internal UltraCombo CmbWtCuton;

	internal UltraCombo CmbRateUom;

	private TextBox txtWeightCut;

	private Label labeldeduciton;

	private TextBox txtMoisture;

	private Label label22;

	private TextBox txtItemRate;

	private Label lblRate;

	private TextBox txtAddRate;

	private Label lblPremium;

	private Label label29;

	private Label lblRateUom;

	private Label label7;

	private Label PaymentVoucherLable;

	private TextBox txtPurchaseType;

	private Label label33;

	private Panel panel4;

	internal GridEX grdItemLastPurchaseInfo;

	private Panel panel6;

	private Label label2;

	private Panel PanelData;

	private Button BtnUpdateOnly;

	private Label label3;

	private TextBox txtRateCut;

	private GroupBox groupBoxGP;

	private BackgroundWorker backgroundWorker1;

	private Button BtnRateApproval;

	private GroupBox groupBox2;

	private GroupBox groupBox1;

	private Label labelPoRate;

	private TextBox txtPoRate;

	public LabPurchaseAnalysisForApproval(UserAccount userAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = userAccount;
	}

	private void frmStockWithSupplierHistory_Load(object sender, EventArgs e)
	{
		try
		{
			AttachChangeHandlers(PanelData);
			UserAccount.BranchesId = ((BranchesId > 0) ? BranchesId : UserAccount.BranchesId);
			RateFromScheduleOnLabForGatePurchase = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("GetRateFromScheduleOnLabForGatePurchase"));
			WareHouseBindFromGlobal();
			JobLotBindFromGlobal();
			WeightCutOnFill();
			StatusFill();
			GridFill();
			FieldsChanged = false;
			btnApproved.Visible = !RateApprovalGatePurchase && !WeightApprovalPurchaseOrder;
			PanelData.Enabled = RateApprovalGatePurchase || WeightApprovalPurchaseOrder;
			BtnRateApproval.Visible = RateApprovalGatePurchase || WeightApprovalPurchaseOrder;
			BtnUpdateOnly.Visible = RateApprovalGatePurchase || WeightApprovalPurchaseOrder;
			BtnRateApproval.Text = (WeightApprovalPurchaseOrder ? "Weight Cut Approval" : "Rate Approval");
			labelPoRate.Visible = RefDocumentTypeId == 41;
			txtPoRate.Visible = RefDocumentTypeId == 41;
			txtPurchaseType.Width = ((RefDocumentTypeId == 41) ? 174 : 287);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AttachChangeHandlers(Control parent)
	{
		foreach (Control ctrl in parent.Controls)
		{
			if (ctrl is TextBox txtBox)
			{
				txtBox.TextChanged += delegate
				{
					FieldsChanged = true;
				};
				continue;
			}
			if (ctrl is ComboBox comboBox)
			{
				comboBox.SelectedIndexChanged += delegate
				{
					FieldsChanged = true;
				};
				continue;
			}
			UltraCombo ultraCombo = (UltraCombo)(object)((ctrl is UltraCombo) ? ctrl : null);
			if (ultraCombo != null)
			{
				ultraCombo.ValueChanged += delegate
				{
					FieldsChanged = true;
				};
			}
			else if (ctrl is CheckBox checkBox)
			{
				checkBox.CheckedChanged += delegate
				{
					FieldsChanged = true;
				};
			}
			else if (ctrl.HasChildren)
			{
				AttachChangeHandlers(ctrl);
			}
		}
	}

	private void JobLotBindFromGlobal()
	{
		try
		{
			DataTable dt = DatatableHelper.PopulateDataTableAndReturn(clsGlobalVariables.globalJobLot, (GetJobLotGlIdsandName x) => x.Id, (GetJobLotGlIdsandName x) => x.JobLotDescription);
			InfragisticsHelper.BindAndRetainSelection(CmbJobLot, dt, "Id", "Description", "Job Lot", AllColumns: true, Conversion.ToInt(CmbJobLot.Value), insertDefaultRow: true);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void WareHouseBindFromGlobal()
	{
		try
		{
			DataTable dt = DatatableHelper.PopulateDataTableAndReturn(clsGlobalVariables.globalWarehousesWithBranches, (GlobalWarehousesWithBranch x) => x.WarehouseId, (GlobalWarehousesWithBranch x) => x.WarehouseName);
			InfragisticsHelper.BindAndRetainSelection(CmbWarehouse, dt, "Id", "Description", "Warehouse Name", AllColumns: true, Conversion.ToInt(CmbWarehouse.Value), insertDefaultRow: true);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void WeightCutOnFill()
	{
		try
		{
			DataTable dt = CommonServices.StaticColumnsService("LabWeightCutOn");
			InfragisticsHelper.BindAndRetainSelection(CmbWtCuton, dt, "Id", "type", "CutOn", AllColumns: false, Conversion.ToInt(CmbWtCuton.Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void StatusFill()
	{
		try
		{
			dtStatus.Columns.Add("Id");
			dtStatus.Columns.Add("Status");
			dtStatus.Rows.Add(1, "Accepted");
			dtStatus.Rows.Add(2, "Rejected");
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
			RefDocumentTypeId = 0;
			RecId = 0;
			ItemId = 0;
			LabDocDate = DateTime.Now;
			RateAppliedStatus = false;
			WeightAppliedStatus = false;
			txtPurchaseType.Text = "";
			txtPoRate.Text = "0";
			txtWeightCut.Text = "0";
			((Control)(object)CmbWtCuton).Text = string.Empty;
			txtWeightCutUom.Text = string.Empty;
			txtQtyForWtCut.Text = "0";
			ChkRateCutWillApply.Checked = false;
			ChkWtCutWillApply.Checked = false;
			((Control)(object)CmbRateUom).Text = string.Empty;
			((UltraGridBase)CmbRateUom).DataSource = null;
			txtItemRate.Text = "0";
			txtAddRate.Text = "0";
			txtRateCut.Text = "0";
			txtLessRate.Text = "0";
			txtNetRate.Text = "0";
			((Control)(object)CmbWarehouse).Text = string.Empty;
			((Control)(object)CmbJobLot).Text = string.Empty;
			txtMoisture.Text = "0";
			txtremarks.Text = string.Empty;
			grdHistroryPurAnalysis.DataSource = null;
			grdItemLastPurchaseInfo.DataSource = null;
			txtScheduleId = 0;
			List<string> allServices = new List<string> { "UomSchedule" };
			DatatableHelper.GlobalServicesDbCall(UserAccount, allServices);
			FieldsChanged = false;
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
			Reset();
			GridFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ResetRateControls()
	{
		txtMoisture.Text = "0";
		txtItemRate.Text = "0";
		txtAddRate.Text = "0";
		txtLessRate.Text = "0";
		txtNetRate.Text = "0";
		((Control)(object)CmbRateUom).Text = string.Empty;
		((UltraGridBase)CmbRateUom).DataSource = null;
	}

	private void GridFill()
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
			if (RateApprovalGatePurchase)
			{
				obj.ActionId = 1;
			}
			if (WeightApprovalPurchaseOrder)
			{
				obj.ActionId = 2;
			}
			DataTable dt = InvLabAnalysisPurchaseHeader.GetLabDataForApproval(obj);
			if (dt.Rows.Count > 0)
			{
				DataTable dtGrid = new DataTable();
				dtGrid.Columns.Add("Id");
				dtGrid.Columns.Add("DocumentTypeId");
				dtGrid.Columns.Add("LabStatus");
				dtGrid.Columns.Add("IsAcceptedApproved");
				dtGrid.Columns.Add("DeductionRate", typeof(double));
				dtGrid.Columns.Add("DeductionWeight", typeof(double));
				dtGrid.Columns.Add("LabType", typeof(string));
				dtGrid.Columns.Add("PurchaseType", typeof(string));
				dtGrid.Columns.Add("DocDate", typeof(DateTime));
				dtGrid.Columns.Add("DocNo", typeof(int));
				dtGrid.Columns.Add("SupplierName");
				dtGrid.Columns.Add("GpSrNo", typeof(int));
				dtGrid.Columns.Add("VehicleNo");
				dtGrid.Columns.Add("BiltyNo");
				dtGrid.Columns.Add("SampleNo");
				dtGrid.Columns.Add("ItemName");
				dtGrid.Columns.Add("CropYear");
				dtGrid.Columns.Add("AnalysisGroup");
				dtGrid.Columns.Add("AnalystName");
				dtGrid.Columns.Add("EntryDate", typeof(DateTime));
				dtGrid.Columns.Add("EntryUser");
				dtGrid.Columns.Add("NoOfAttachments");
				dtGrid.Rows.Clear();
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtGrid.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["DocumentTypeId"], dt.Rows[i]["Lab_Status"], dt.Rows[i]["IsAcceptedApproved"], dt.Rows[i]["DeductionRate"], dt.Rows[i]["DeductionWeight"], dt.Rows[i]["LabType"], dt.Rows[i]["PurchaseType"], Conversion.ToDateTime(dt.Rows[i]["DocDate"]).ToShortDateString(), dt.Rows[i]["DocNo"], dt.Rows[i]["SupplierName"], dt.Rows[i]["GpSrNo"], dt.Rows[i]["VehicleNo"], dt.Rows[i]["BiltyNo"], dt.Rows[i]["SampleNo"], dt.Rows[i]["ItemName"], dt.Rows[i]["Crop"], dt.Rows[i]["AnalysisGroupDescription"], dt.Rows[i]["AnalystName"], dt.Rows[i]["EntryDate"], dt.Rows[i]["UserName"], dt.Rows[i]["NoOfAttachments"]);
				}
				grdHistory.DataSource = dtGrid;
				grdHistory.RetrieveStructure();
				GridSetting();
			}
			else
			{
				grdHistory.DataSource = null;
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
			List<string> hiddenColumns = new List<string> { "Id", "DocumentTypeId", "DeductionRate", "DeductionWeight" };
			Dictionary<string, EditType> editableColumns = new Dictionary<string, EditType>();
			if (!RateApprovalGatePurchase && !WeightApprovalPurchaseOrder)
			{
				editableColumns = new Dictionary<string, EditType>
				{
					{
						"DeductionRate",
						(EditType)1
					},
					{
						"DeductionWeight",
						(EditType)1
					},
					{
						"IsAcceptedApproved",
						(EditType)4
					}
				};
			}
			GridEX_Helper.GridWrappingAndColumnSettings(grdHistory, 2, 2, hiddenColumns, editableColumns);
			grdHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yy hh:mm tt";
			grdHistory.RootTable.Columns["LabType"].Width = 70;
			grdHistory.RootTable.Columns["LabStatus"].Caption = "Entry Lab Status";
			grdHistory.RootTable.Columns["DocDate"].Width = Constants.InventoryConstants.DateConstant;
			grdHistory.RootTable.Columns["DocNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grdHistory.RootTable.Columns["SupplierName"].Width = Constants.InventoryConstants.SupplierName;
			grdHistory.RootTable.Columns["GpSrNo"].Width = Constants.InventoryConstants.GpNoConstant;
			grdHistory.RootTable.Columns["VehicleNo"].Width = Constants.InventoryConstants.VehicleNoConstant;
			grdHistory.RootTable.Columns["BiltyNo"].Width = Constants.InventoryConstants.BiltyNoConstant;
			grdHistory.RootTable.Columns["SampleNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grdHistory.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			grdHistory.RootTable.Columns["CropYear"].Width = Constants.InventoryConstants.CropYearConstant;
			grdHistory.RootTable.Columns["AnalysisGroup"].Width = 70;
			grdHistory.RootTable.Columns["AnalystName"].Width = Constants.InventoryConstants.StatusConstant;
			grdHistory.RootTable.Columns["DeductionRate"].Width = Constants.InventoryConstants.ExRateConstant;
			grdHistory.RootTable.Columns["DeductionWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdHistory.RootTable.Columns["LabStatus"].Width = Constants.InventoryConstants.StatusConstant;
			grdHistory.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM;
			grdHistory.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants;
			grdHistory.RootTable.Columns["IsAcceptedApproved"].Width = 80;
			grdHistory.RootTable.Columns["PurchaseType"].Width = 110;
			grdHistory.RootTable.Columns["NoOfAttachments"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants;
			if (!RateApprovalGatePurchase && !WeightApprovalPurchaseOrder)
			{
				grdHistory.RootTable.Groups.Add("PurchaseType");
				GridEX_Helper.ComboBind(grdHistory.RootTable.Columns["IsAcceptedApproved"], dtStatus, "Id", "Status", "Approval Status");
				GridEX_Helper.AddSelectButton(grdHistory, "Select", 40, 0);
			}
			GridEX_Helper.AddButton(grdHistory, "Print", "Print", 40, 1);
			grdHistory.RootTable.Columns["IsAcceptedApproved"].Position = 2;
			grdHistory.RootTable.Columns["IsAcceptedApproved"].Visible = !RateApprovalGatePurchase && !WeightApprovalPurchaseOrder;
			grdHistory.FrozenColumns = 3;
			ctrlGrdBar1_Load(null, null);
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
			int Id = Conversion.ToInt(grdHistory.CurrentRow.Cells["Id"].Value);
			if (e.Column.Key == "Print")
			{
				PrintSlip(Id);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow item = grdHistory.CurrentRow;
			if (Conversion.ToInt(item.Cells["NoOfAttachments"].Value) > 0)
			{
				CommonServices.GetNoofAttachmentsByScreenName(Conversion.ToInt(item.Cells["Id"].Value), "InvLabPurchaseAnalysis");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdHistory.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				int Id = Conversion.ToInt(item.Cells["Id"].Value);
				Conversion.ToInt(item.Cells["DocumentTypeId"].Value);
				if (FieldsChanged && RecId > 0 && RecId != Id)
				{
					string DocNo = GridEX_Helper.GetFirstOrDefaultValueOfColumn(grdHistory, "DocNo", "Id", RecId);
					if (MessageBox.Show("Detail Data has been changed For Doc No : " + DocNo + ". Do you want to discard changes and continue?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					RecId = 0;
				}
				if (!FieldsChanged || RecId == 0)
				{
					ReadById(Id);
				}
			}
			labelPoRate.Visible = RefDocumentTypeId == 41;
			txtPoRate.Visible = RefDocumentTypeId == 41;
			txtPurchaseType.Width = ((RefDocumentTypeId == 41) ? 174 : 287);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ReadById(int ID)
	{
		try
		{
			Reset();
			RecId = ID;
			InvLabAnalysisPurchaseHeader obj = InvLabAnalysisPurchaseHeader.ReadById(ID);
			if (obj != null)
			{
				PaymentVoucherLable.Text = "Purchase Analysis Parameters' Results  Analysis (No." + obj.DocNo + ")";
				RefDocumentTypeId = Conversion.ToInt((object)obj.RefDocumentTypeId);
				ItemId = Conversion.ToInt((object)obj.ItemId);
				LabDocDate = Conversion.ToDateTime((object)obj.DocDate);
				txtPurchaseType.Text = Conversion.ToString((object)obj.PurchaseType);
				txtPoRate.Text = Conversion.ToString((object)obj.OrderRate);
				CmbWtCuton.Value = Conversion.ToInt((object)obj.WeightCutOnId);
				txtWeightCutUom.Text = Conversion.ToString((object)obj.WeightCutUom);
				if (RefDocumentTypeId == 106)
				{
					UomBindByItemId(obj.GatePassInwardId, obj.ItemId);
					if (obj.RateUomId > 0)
					{
						CmbRateUom.Value = obj.RateUomId;
					}
				}
				else
				{
					((Control)(object)CmbRateUom).Text = string.Empty;
					((UltraGridBase)CmbRateUom).DataSource = null;
				}
				ChkRateCutWillApply.Checked = obj.IsRateCutCompulsory;
				ChkWtCutWillApply.Checked = obj.IsWeightCutCompulsory;
				RateAppliedStatus = obj.RateApplied;
				WeightAppliedStatus = obj.WeightApplied;
				txtremarks.Text = Conversion.ToString((object)obj.RemarksHeader);
				txtAddRate.Text = Conversion.ToString((object)obj.Premium);
				if (RefDocumentTypeId == 106)
				{
					txtLessRate.Text = obj.DeductionRate.ToString("#,##0.####");
				}
				else
				{
					txtRateCut.Text = obj.DeductionRate.ToString("#,##0.####");
				}
				txtItemRate.Text = Conversion.ToString((object)obj.ItemRate);
				txtWeightCut.Text = Conversion.ToString((object)obj.DeductionWeight);
				txtQtyForWtCut.Text = Conversion.ToString((object)obj.QtyForWtCut);
				txtScheduleId = Conversion.ToInt((object)obj.PricingScheduleId);
				CmbWarehouse.Value = obj.WarehouseId;
				if (obj.JobLotId > 0)
				{
					CmbJobLot.Value = Conversion.ToInt((object)obj.JobLotId);
				}
				NetRateCalculation();
				DataTable dtPurHistory = new DataTable();
				dtPurHistory.Columns.Add("Id", typeof(int));
				dtPurHistory.Columns.Add("AnalysisGroup");
				dtPurHistory.Columns.Add("AnalysisParameter");
				dtPurHistory.Columns.Add("MinValue");
				dtPurHistory.Columns.Add("MaxValue");
				dtPurHistory.Columns.Add("ResultValue", typeof(double));
				dtPurHistory.Columns.Add("Remarks");
				dtPurHistory.Columns.Add("ParamsMinValue", typeof(double));
				dtPurHistory.Columns.Add("ParamsMaxValue", typeof(double));
				dtPurHistory.Columns.Add("IsEditableAfterApproval", typeof(bool));
				dtPurHistory.Rows.Clear();
				if (obj.LabSampleDetail != null && obj.LabSampleDetail.Count > 0)
				{
					for (int i = 0; i < obj.LabSampleDetail.Count; i++)
					{
						dtPurHistory.Rows.Add(obj.LabSampleDetail[i].Id, obj.LabSampleDetail[i].AnalysisGroupDescription, obj.LabSampleDetail[i].AnalysisParameterDescription, obj.LabSampleDetail[i].MinValue, obj.LabSampleDetail[i].MaxValue, obj.LabSampleDetail[i].InAnalysisResult, obj.LabSampleDetail[i].RemarksDetail, obj.LabSampleDetail[i].ParamsMinValue, obj.LabSampleDetail[i].ParamsMaxValue, obj.LabSampleDetail[i].IsEditableAfterApproval);
					}
				}
				grdHistroryPurAnalysis.DataSource = dtPurHistory;
				grdHistroryPurAnalysis.RetrieveStructure();
				GrdHistoryDetailPurSetting();
				if (RefDocumentTypeId == 41)
				{
					double moisture = CommonServices.GetMoistureByItemIdAndPurchaseOrderId(ItemId, obj.PurchaseOrderId);
					txtMoisture.Text = Conversion.ToString((object)moisture);
				}
				txtItemRate.Enabled = RefDocumentTypeId == 106 && !RateFromScheduleOnLabForGatePurchase;
				txtItemRate.ReadOnly = !txtItemRate.Enabled;
				txtAddRate.Enabled = RefDocumentTypeId == 106;
				txtLessRate.Enabled = RefDocumentTypeId == 106;
				((Control)(object)CmbRateUom).Enabled = RefDocumentTypeId == 106;
				txtRateCut.Enabled = RefDocumentTypeId != 106;
				BtnRateApproval.Enabled = (RefDocumentTypeId == 106 && !obj.RateApplied) || (RefDocumentTypeId == 41 && !obj.WeightApplied);
				GridItemInfoFill(ItemId);
			}
			FieldsChanged = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdHistoryDetailPurSetting()
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "IsEditableAfterApproval" };
			GridEX_Helper.GridWrappingAndColumnSettings(grdHistroryPurAnalysis, 2, 2, hiddenColumns);
			grdHistroryPurAnalysis.RootTable.Groups.Add("AnalysisGroup");
			grdHistroryPurAnalysis.HideColumnsWhenGrouped = (InheritableBoolean)1;
			grdHistroryPurAnalysis.RootTable.Columns["AnalysisParameter"].Width = Constants.LabConstants.AnalysisParameter;
			grdHistroryPurAnalysis.RootTable.Columns["MinValue"].Width = Constants.LabConstants.MinValue;
			grdHistroryPurAnalysis.RootTable.Columns["MaxValue"].Width = Constants.LabConstants.MaxValue;
			grdHistroryPurAnalysis.RootTable.Columns["ResultValue"].Width = Constants.LabConstants.ResultValue;
			grdHistroryPurAnalysis.RootTable.Columns["ParamsMinValue"].Width = Constants.LabConstants.ResultValue;
			grdHistroryPurAnalysis.RootTable.Columns["ParamsMaxValue"].Width = Constants.LabConstants.ResultValue;
			grdHistroryPurAnalysis.RootTable.Columns["Remarks"].Width = 185;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UomBindByItemId(int GpId, int ItemId = 0)
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		try
		{
			if (ItemId == 0)
			{
				ResetRateControls();
				txtScheduleId = 0;
				return;
			}
			DataTable dtRateUom = null;
			if (RefDocumentTypeId == 106 || RefDocumentTypeId == 110)
			{
				dtRateUom = ((!RateFromScheduleOnLabForGatePurchase) ? CommonServices.dtUomFromGloablUomScheduleByItemId(ItemId) : ItemPricingScheduleForRice.GetRateUomFromItemPricingSchedule(new ItemPricingScheduleForRice
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					ItemId = ItemId
				}));
				if (dtRateUom != null && dtRateUom.Rows.Count > 0)
				{
					string valueField = (RateFromScheduleOnLabForGatePurchase ? "RateUomId" : "Id");
					string textField = (RateFromScheduleOnLabForGatePurchase ? "RateUom" : "UOMCode");
					InfragisticsHelper.BindAndRetainSelection(CmbRateUom, dtRateUom, valueField, textField, "RateUom", AllColumns: false, Conversion.ToString((object)((Control)(object)CmbRateUom).Text), insertDefaultRow: false, null, null, ActivateRow: false, 0, "UOMCode");
					((UltraGridBase)CmbRateUom).Rows[0].Activate();
					CmbRateUom_Leave(null, null);
				}
				else
				{
					ResetRateControls();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbRateUom_Leave(object sender, EventArgs e)
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Expected O, but got Unknown
		try
		{
			if (((UltraGridBase)CmbRateUom).ActiveRow != null && ItemId != 0 && RateFromScheduleOnLabForGatePurchase)
			{
				if (RefDocumentTypeId == 106 || RefDocumentTypeId == 110)
				{
					DataTable dt = ItemPricingScheduleForRice.GetItemRateByItemIdAndUomId(new ItemPricingScheduleForRice
					{
						OrganizationId = UserAccount.OrganizationId,
						CompanyId = UserAccount.CompanyId,
						ItemId = ItemId,
						RateUomId = Conversion.ToInt(CmbRateUom.Value),
						EffectiveDate = Conversion.ToDateTime((object)LabDocDate)
					});
					if (dt.Rows.Count > 0)
					{
						txtItemRate.Text = dt.Rows[0]["ItemRate"].ToString();
						txtNetRate.Text = dt.Rows[0]["ItemRate"].ToString();
						txtScheduleId = Conversion.ToInt(dt.Rows[0]["Id"]);
						NetRateCalculation();
					}
					else
					{
						txtItemRate.Text = "0";
						txtScheduleId = 0;
					}
				}
			}
			else
			{
				txtScheduleId = 0;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridItemInfoFill(int ItemId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		try
		{
			DataTable dataTable = InvPurchaseInvoice.PurchaseInvoice_GetLastRecordsByItem(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				ItemId = ItemId
			});
			if (dataTable.Rows.Count == 0)
			{
				grdItemLastPurchaseInfo.DataSource = null;
			}
			DataTable dtGrid = new DataTable();
			dtGrid.Columns.Add("Id", typeof(int));
			dtGrid.Columns.Add("DocDate", typeof(DateTime));
			dtGrid.Columns.Add("InvoiceNo", typeof(int));
			dtGrid.Columns.Add("PurchaseType", typeof(string));
			dtGrid.Columns.Add("SupplierCustomerId", typeof(int));
			dtGrid.Columns.Add("SupplierName", typeof(string));
			dtGrid.Columns.Add("ItemQty", typeof(double));
			dtGrid.Columns.Add("NetWeight", typeof(double));
			dtGrid.Columns.Add("NetRate", typeof(double));
			dtGrid.Columns.Add("ItemId", typeof(int));
			dtGrid.Columns.Add("ItemName", typeof(string));
			dtGrid.Rows.Clear();
			foreach (DataRow row in dataTable.Rows)
			{
				dtGrid.Rows.Add(Conversion.ToInt(row["Id"]), Conversion.ToDateTime(row["DocDate"]), Conversion.ToInt(row["DocNo"]), row["PurchaseAgainst"]?.ToString() ?? string.Empty, row["SupplierCustomerId"] ?? ((object)0), row["SupplierName"]?.ToString() ?? string.Empty, Conversion.ToDouble(row["ItemQty"]), Conversion.ToDouble(row["NetBillWeight"]), Conversion.ToDouble(row["ItemRate"]), row["ItemId"], row["ItemName"]?.ToString() ?? string.Empty);
			}
			grdItemLastPurchaseInfo.DataSource = dtGrid;
			grdItemLastPurchaseInfo.RetrieveStructure();
			GridItemLastPurchaseInfoSetting();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridItemLastPurchaseInfoSetting()
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "SupplierCustomerId", "ItemId", "ItemName" };
			GridEX_Helper.GridWrappingAndColumnSettings(grdItemLastPurchaseInfo, 2, 2, hiddenColumns);
			grdItemLastPurchaseInfo.RootTable.Groups.Add("ItemName");
			grdItemLastPurchaseInfo.RootTable.Columns["DocDate"].Width = Constants.InventoryConstants.DateConstant;
			grdItemLastPurchaseInfo.RootTable.Columns["PurchaseType"].Width = Constants.InventoryConstants.OperationName;
			grdItemLastPurchaseInfo.RootTable.Columns["SupplierName"].Width = Constants.InventoryConstants.SupplierName;
			grdItemLastPurchaseInfo.RootTable.Columns["ItemQty"].Width = Constants.InventoryConstants.QtyConstant;
			grdItemLastPurchaseInfo.RootTable.Columns["NetWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdItemLastPurchaseInfo.RootTable.Columns["NetRate"].Width = Constants.InventoryConstants.ExRateConstant;
			grdItemLastPurchaseInfo.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			grdItemLastPurchaseInfo.RootTable.Columns["InvoiceNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grdItemLastPurchaseInfo.RootTable.Columns["InvoiceNo"].ColumnType = (ColumnType)5;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PrintSlip(int Id)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Expected O, but got Unknown
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.Id = Id;
			DataTable dt = InvLabAnalysisPurchaseHeader.RptInvLabPurchaseAnalysisSlip653(obj);
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("Record Not Found For Display");
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(InvLabAnalysisPurchaseHeader.GetDataForLabPurchaseAnalysisSubReport(obj.Id), "LabPurchaseAnalysisSubParameterReport.rpt");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "653-RptInvLabPurchaseAnalysisSlip.rpt", lstSubReports);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool formvalidation()
	{
		if (Conversion.ToInt((object)txtWeightCut.Text) > 0 && (((UltraGridBase)CmbWtCuton).ActiveRow == null || Conversion.ToInt(CmbWtCuton.Value) == 0))
		{
			MessageBox.Show("Weight Cut On field required");
			CmbWtCuton.Focus();
			return false;
		}
		if (RefDocumentTypeId == 106)
		{
			if (((UltraGridBase)CmbRateUom).ActiveRow == null || Conversion.ToInt(CmbRateUom.Value) == 0)
			{
				MessageBox.Show("RateUom Field Required");
				CmbRateUom.Focus();
				return false;
			}
			if (Conversion.ToDouble((object)txtItemRate.Text) == 0.0)
			{
				MessageBox.Show("ItemRate Field Required");
				txtItemRate.Focus();
				return false;
			}
			if (Conversion.ToDouble((object)txtNetRate.Text) == 0.0)
			{
				MessageBox.Show("NetRate Field Required");
				txtNetRate.Focus();
				return false;
			}
		}
		if (Conversion.ToDouble((object)txtItemRate.Text) > 0.0)
		{
			if (((UltraGridBase)CmbRateUom).ActiveRow == null || Conversion.ToInt(CmbRateUom.Value) == 0)
			{
				MessageBox.Show("RateUom Field Required");
				CmbRateUom.Focus();
				return false;
			}
			if (Conversion.ToDouble((object)txtNetRate.Text) == 0.0)
			{
				MessageBox.Show("NetRate Field Required");
				txtNetRate.Focus();
				return false;
			}
		}
		if (((UltraGridBase)CmbWarehouse).ActiveRow == null || Conversion.ToInt(CmbWarehouse.Value) == 0)
		{
			MessageBox.Show("Warehouse field required");
			CmbWarehouse.Focus();
			return false;
		}
		return true;
	}

	private LabAnalysisPurchaseUpdateDuringApproval UpdateAndApproveRecord(int Id, int ActionId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			LabAnalysisPurchaseUpdateDuringApproval labheader = new LabAnalysisPurchaseUpdateDuringApproval();
			labheader.Id = Id;
			labheader.CompanyId = UserAccount.CompanyId;
			labheader.OrganizationId = UserAccount.OrganizationId;
			labheader.BranchesId = UserAccount.BranchesId;
			labheader.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			labheader.DocumentTypeId = 303;
			labheader.ApprovedUserId = UserAccount.ID;
			labheader.ModifyUser = UserAccount.ID;
			labheader.ModifyDate = DateTime.Now;
			labheader.ApprovedDate = DateTime.Now;
			labheader.ItemId = ItemId;
			labheader.RefDocumentTypeId = RefDocumentTypeId;
			labheader.ActionId = ActionId;
			labheader.DeductionWeight = Conversion.ToDouble((object)txtWeightCut.Text);
			labheader.WeightCutOnId = Conversion.ToInt(CmbWtCuton.Value);
			labheader.WeightCutUom = Conversion.ToDouble((object)txtWeightCutUom.Text);
			labheader.QtyForWtCut = Conversion.ToDouble((object)txtQtyForWtCut.Text);
			if (labheader.DeductionWeight == 0.0)
			{
				labheader.IsWeightCutCompulsory = ChkWtCutWillApply.Checked;
			}
			labheader.DeductionRate = ((RefDocumentTypeId == 106) ? Conversion.ToDouble((object)txtLessRate.Text) : Conversion.ToDouble((object)txtRateCut.Text));
			if (labheader.DeductionRate == 0.0)
			{
				labheader.IsRateCutCompulsory = ChkRateCutWillApply.Checked;
			}
			labheader.RateUomId = Conversion.ToInt(CmbRateUom.Value);
			labheader.PricingScheduleId = txtScheduleId;
			labheader.ItemRate = Conversion.ToDouble((object)txtItemRate.Text);
			labheader.Premium = Conversion.ToDouble((object)txtAddRate.Text);
			labheader.WarehouseId = Conversion.ToInt(CmbWarehouse.Value);
			labheader.JobLotId = Conversion.ToInt(CmbJobLot.Value);
			labheader.RemarksHeader = Conversion.ToString((object)txtremarks.Text);
			labheader.RateApplied = RateAppliedStatus;
			labheader.RateAppliedFromId = (RateApprovalGatePurchase ? 3 : 2);
			labheader.RateAppliedUserId = UserAccount.ID;
			labheader.WeightApplied = WeightAppliedStatus;
			labheader.WeightAppliedFromId = (WeightApprovalPurchaseOrder ? 4 : 2);
			labheader.WeightAppliedUserId = UserAccount.ID;
			return labheader;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnApproved_Click(object sender, EventArgs e)
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Expected O, but got Unknown
		try
		{
			if (grdHistory.GetCheckedRows().Count() == 0)
			{
				throw new Exception("Please select check box first");
			}
			if (MessageBox.Show("Are you sure to Only Approve Selected Records?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			LabAnalysisPurchaseUpdateDuringApproval obj = new LabAnalysisPurchaseUpdateDuringApproval();
			obj.LabAnalysisPurchaseUpdateDuringApprovalList = new List<LabAnalysisPurchaseUpdateDuringApproval>();
			GridEXRow[] checkedRows = grdHistory.GetCheckedRows();
			foreach (GridEXRow item in checkedRows)
			{
				int Id = Conversion.ToInt(item.Cells["Id"].Value);
				LabAnalysisPurchaseUpdateDuringApproval detail = UpdateAndApproveRecord(Id, 1);
				detail.DeductionRate = Conversion.ToDouble(item.Cells["DeductionRate"].Value);
				detail.DeductionWeight = Conversion.ToDouble(item.Cells["DeductionWeight"].Value);
				if (Conversion.ToInt(item.Cells["IsAcceptedApproved"].Value) == 1)
				{
					detail.IsAcceptedApproved = true;
				}
				if (Conversion.ToInt(item.Cells["IsAcceptedApproved"].Value) == 2)
				{
					detail.IsAcceptedApproved = false;
				}
				if (Conversion.ToInt(item.Cells["IsAcceptedApproved"].Value) == 0)
				{
					throw new Exception("Please Update Document Status (By Approval Person)!");
				}
				obj.LabAnalysisPurchaseUpdateDuringApprovalList.Add(detail);
			}
			InvLabAnalysisPurchaseHeader.UpdateDuringApproval(obj);
			MessageBox.Show("Record Approved Successfully");
			Reset();
			GridFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnUpdateOnly_Click(object sender, EventArgs e)
	{
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Expected O, but got Unknown
		try
		{
			if (RecId == 0)
			{
				throw new Exception("Please select record first");
			}
			if (formvalidation())
			{
				if (!FieldsChanged)
				{
					throw new Exception("No changes to update,You have Not made any changes yet");
				}
				if (MessageBox.Show("Are you sure to Update Data?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					InvLabAnalysisPurchaseHeader.UpdateDuringApproval(new LabAnalysisPurchaseUpdateDuringApproval
					{
						LabAnalysisPurchaseUpdateDuringApprovalList = new List<LabAnalysisPurchaseUpdateDuringApproval>(),
						LabAnalysisPurchaseUpdateDuringApprovalList = { UpdateAndApproveRecord(RecId, 2) }
					});
					MessageBox.Show("Record Approved Successfully");
					Reset();
					GridFill();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnRateApproval_Click(object sender, EventArgs e)
	{
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Expected O, but got Unknown
		try
		{
			if (RecId == 0)
			{
				throw new Exception("Please select record first");
			}
			if (!formvalidation())
			{
				return;
			}
			string Msg = (RateApprovalGatePurchase ? "Rate" : (WeightApprovalPurchaseOrder ? "WeightCut" : ""));
			if (MessageBox.Show("Are you sure to Approve " + Msg + "?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				int ActionId = 0;
				if (RateApprovalGatePurchase)
				{
					ActionId = 3;
					RateAppliedStatus = true;
				}
				if (WeightApprovalPurchaseOrder)
				{
					ActionId = 4;
					WeightAppliedStatus = true;
				}
				InvLabAnalysisPurchaseHeader.UpdateDuringApproval(new LabAnalysisPurchaseUpdateDuringApproval
				{
					LabAnalysisPurchaseUpdateDuringApprovalList = new List<LabAnalysisPurchaseUpdateDuringApproval>(),
					LabAnalysisPurchaseUpdateDuringApprovalList = { UpdateAndApproveRecord(RecId, ActionId) }
				});
				MessageBox.Show("Record Approved Successfully");
				Reset();
				GridFill();
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
			MessageBox.Show(ex.Message);
		}
	}

	private void txtItemRate_TextChanged(object sender, EventArgs e)
	{
		NetRateCalculation();
	}

	private void NetRateCalculation()
	{
		try
		{
			double num = Conversion.ToDouble((object)txtItemRate.Text.Trim());
			double AddRate = Conversion.ToDouble((object)txtAddRate.Text.Trim());
			double LessRate = Conversion.ToDouble((object)txtLessRate.Text.Trim());
			txtNetRate.Text = (num + AddRate - LessRate).ToString("#,#");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbWtCuton_Leave(object sender, EventArgs e)
	{
		txtWeightCutUom.Enabled = Conversion.ToInt(CmbWtCuton.Value) == 2;
		txtWeightCutUom.Text = ((Conversion.ToInt(CmbWtCuton.Value) == 2) ? txtWeightCutUom.Text : "0");
		if (Conversion.ToInt(CmbWtCuton.Value) == 2 && base.ActiveControl != null && base.ActiveControl.Name == "txtWeightCutUom")
		{
			txtWeightCutUom.Focus();
		}
	}

	private void txtWeightCutUom_KeyPress(object sender, KeyPressEventArgs e)
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

	private void grdHistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!e.Control || e.KeyCode != Keys.Space)
			{
				return;
			}
			GridEXRow item = grdHistory.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			int Id = Conversion.ToInt(item.Cells["Id"].Value);
			int DocumentTypeId = Conversion.ToInt(item.Cells["DocumentTypeId"].Value);
			if (grdHistory.CurrentColumn != null)
			{
				if (grdHistory.CurrentColumn.Key == "Print")
				{
					PrintSlip(Id);
				}
				else if (grdHistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, DocumentTypeId);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdItemLastPurchaseInfo_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdItemLastPurchaseInfo.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count != 0 && e.Column.Key == "InvoiceNo")
			{
				if (!CommonServices.CheckScreenRight("PurchaseRegisterNew"))
				{
					throw new Exception("You don't have a right of Purchase Invoice Register...");
				}
				int SupplierCustomerId = Conversion.ToInt(r.Cells["SupplierCustomerId"].Value);
				int ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
				DateTime DocDate = Conversion.ToDateTime(r.Cells["DocDate"].Value);
				PurchaseRegisterNew purchaseRegisterNew = new PurchaseRegisterNew(UserAccount);
				purchaseRegisterNew.FromDate = DocDate;
				purchaseRegisterNew.ToDate = DateTime.Now;
				purchaseRegisterNew.ItemId = ItemId;
				purchaseRegisterNew.SupplierId = SupplierCustomerId;
				purchaseRegisterNew.ShowDialog();
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
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Expected O, but got Unknown
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Expected O, but got Unknown
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Expected O, but got Unknown
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Expected O, but got Unknown
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Expected O, but got Unknown
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Expected O, but got Unknown
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Expected O, but got Unknown
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Expected O, but got Unknown
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Expected O, but got Unknown
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Expected O, but got Unknown
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Expected O, but got Unknown
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Expected O, but got Unknown
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Expected O, but got Unknown
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Expected O, but got Unknown
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Expected O, but got Unknown
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Expected O, but got Unknown
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Expected O, but got Unknown
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Expected O, but got Unknown
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Expected O, but got Unknown
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Expected O, but got Unknown
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Expected O, but got Unknown
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Expected O, but got Unknown
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Expected O, but got Unknown
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Expected O, but got Unknown
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Expected O, but got Unknown
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Expected O, but got Unknown
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Expected O, but got Unknown
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Expected O, but got Unknown
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Expected O, but got Unknown
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Expected O, but got Unknown
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Expected O, but got Unknown
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Expected O, but got Unknown
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Expected O, but got Unknown
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Expected O, but got Unknown
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Expected O, but got Unknown
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Expected O, but got Unknown
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Expected O, but got Unknown
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Expected O, but got Unknown
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Expected O, but got Unknown
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Expected O, but got Unknown
		//IL_0239: Unknown result type (might be due to invalid IL or missing references)
		//IL_0243: Expected O, but got Unknown
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0264: Expected O, but got Unknown
		//IL_0270: Unknown result type (might be due to invalid IL or missing references)
		//IL_027a: Expected O, but got Unknown
		//IL_0378: Unknown result type (might be due to invalid IL or missing references)
		//IL_0382: Expected O, but got Unknown
		//IL_03d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03da: Expected O, but got Unknown
		//IL_07d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07db: Expected O, but got Unknown
		//IL_07e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f2: Expected O, but got Unknown
		//IL_0c1c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c26: Expected O, but got Unknown
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.ApprovalDashboard.LabPurchaseAnalysisForApproval));
		GridEXLayout grdHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdItemLastPurchaseInfo_DesignTimeLayout = new GridEXLayout();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
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
		Appearance appearance1 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		GridEXLayout grdHistroryPurAnalysis_DesignTimeLayout = new GridEXLayout();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnApproved = new System.Windows.Forms.ToolStripButton();
		this.grdHistory = new GridEX();
		this.panel1 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label1 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.grdItemLastPurchaseInfo = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label2 = new System.Windows.Forms.Label();
		this.PanelData = new System.Windows.Forms.Panel();
		this.txtPurchaseType = new System.Windows.Forms.TextBox();
		this.txtPoRate = new System.Windows.Forms.TextBox();
		this.labelPoRate = new System.Windows.Forms.Label();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.CmbWarehouse = new UltraCombo();
		this.label20 = new System.Windows.Forms.Label();
		this.label36 = new System.Windows.Forms.Label();
		this.CmbJobLot = new UltraCombo();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.CmbWtCuton = new UltraCombo();
		this.txtMoisture = new System.Windows.Forms.TextBox();
		this.txtQtyForWtCut = new System.Windows.Forms.TextBox();
		this.txtWeightCut = new System.Windows.Forms.TextBox();
		this.txtRateCut = new System.Windows.Forms.TextBox();
		this.ChkRateCutWillApply = new System.Windows.Forms.CheckBox();
		this.ChkWtCutWillApply = new System.Windows.Forms.CheckBox();
		this.labeldeduciton = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label22 = new System.Windows.Forms.Label();
		this.txtWeightCutUom = new System.Windows.Forms.TextBox();
		this.label37 = new System.Windows.Forms.Label();
		this.label29 = new System.Windows.Forms.Label();
		this.label30 = new System.Windows.Forms.Label();
		this.groupBoxGP = new System.Windows.Forms.GroupBox();
		this.txtItemRate = new System.Windows.Forms.TextBox();
		this.txtLessRate = new System.Windows.Forms.TextBox();
		this.lblNetRate = new System.Windows.Forms.Label();
		this.txtNetRate = new System.Windows.Forms.TextBox();
		this.label28 = new System.Windows.Forms.Label();
		this.lblRateUom = new System.Windows.Forms.Label();
		this.lblRate = new System.Windows.Forms.Label();
		this.txtAddRate = new System.Windows.Forms.TextBox();
		this.lblPremium = new System.Windows.Forms.Label();
		this.CmbRateUom = new UltraCombo();
		this.BtnRateApproval = new System.Windows.Forms.Button();
		this.BtnUpdateOnly = new System.Windows.Forms.Button();
		this.label33 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.txtremarks = new System.Windows.Forms.TextBox();
		this.panel14 = new System.Windows.Forms.Panel();
		this.panel17 = new System.Windows.Forms.Panel();
		this.grdHistroryPurAnalysis = new GridEX();
		this.panel16 = new System.Windows.Forms.Panel();
		this.PaymentVoucherLable = new System.Windows.Forms.Label();
		this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
		this.toolStrip1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).BeginInit();
		this.panel1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdItemLastPurchaseInfo).BeginInit();
		this.panel6.SuspendLayout();
		this.PanelData.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbWarehouse).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).BeginInit();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbWtCuton).BeginInit();
		this.groupBoxGP.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbRateUom).BeginInit();
		this.panel14.SuspendLayout();
		this.panel17.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistroryPurAnalysis).BeginInit();
		this.panel16.SuspendLayout();
		base.SuspendLayout();
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.toolStripSeparator1, this.btnnew, this.btnApproved });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1234, 27);
		this.toolStrip1.TabIndex = 7;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.btnnew.Image = (System.Drawing.Image)resources.GetObject("btnnew.Image");
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(80, 24);
		this.btnnew.Text = "&Refresh";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnApproved.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.btnApproved.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnApproved.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnApproved.Name = "btnApproved";
		this.btnApproved.Size = new System.Drawing.Size(121, 24);
		this.btnApproved.Text = "Lab Approved";
		this.btnApproved.ToolTipText = "Approved All Checked Records";
		this.btnApproved.Click += new System.EventHandler(btnApproved_Click);
		grdHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistory.DesignTimeLayout = grdHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistory.DynamicFiltering = true;
		this.grdHistory.FilterMode = (FilterMode)2;
		this.grdHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdHistory).Location = new System.Drawing.Point(0, 52);
		((System.Windows.Forms.Control)(object)this.grdHistory).Name = "grdHistory";
		((ScrollableBase)this.grdHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistory).Size = new System.Drawing.Size(1234, 274);
		((System.Windows.Forms.Control)(object)this.grdHistory).TabIndex = 48;
		((System.Windows.Forms.Control)(object)this.grdHistory).TabStop = false;
		this.grdHistory.TotalRow = (InheritableBoolean)1;
		this.grdHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistory).VisualStyle = (VisualStyle)4;
		this.grdHistory.ColumnButtonClick += new ColumnActionEventHandler(grdHistory_ColumnButtonClick);
		this.grdHistory.LinkClicked += new ColumnActionEventHandler(grdHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdHistory).Click += new System.EventHandler(grdHistory_SelectionChanged);
		((System.Windows.Forms.Control)(object)this.grdHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(grdHistory_KeyDown);
		this.panel1.BackColor = System.Drawing.Color.Teal;
		this.panel1.Controls.Add(this.ctrlGrdBar1);
		this.panel1.Controls.Add(this.label1);
		this.panel1.Controls.Add(this.toolStrip1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1234, 52);
		this.panel1.TabIndex = 1;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1194, 27);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdHistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 8;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Bold);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(3, 29);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(407, 20);
		this.label1.TabIndex = 8;
		this.label1.Text = "Purchase Lab Analysis  ( Pending Records For Approval ) ";
		this.panel2.BackColor = System.Drawing.Color.White;
		this.panel2.Controls.Add(this.panel4);
		this.panel2.Controls.Add(this.PanelData);
		this.panel2.Controls.Add(this.panel14);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel2.Location = new System.Drawing.Point(0, 326);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1234, 385);
		this.panel2.TabIndex = 49;
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.grdItemLastPurchaseInfo);
		this.panel4.Controls.Add(this.panel6);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(919, 0);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(315, 385);
		this.panel4.TabIndex = 98;
		this.grdItemLastPurchaseInfo.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdItemLastPurchaseInfo).BackColor = System.Drawing.Color.White;
		grdItemLastPurchaseInfo_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdItemLastPurchaseInfo.DesignTimeLayout = grdItemLastPurchaseInfo_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdItemLastPurchaseInfo).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdItemLastPurchaseInfo.FilterMode = (FilterMode)2;
		this.grdItemLastPurchaseInfo.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdItemLastPurchaseInfo).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdItemLastPurchaseInfo.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdItemLastPurchaseInfo).Location = new System.Drawing.Point(0, 23);
		((System.Windows.Forms.Control)(object)this.grdItemLastPurchaseInfo).Name = "grdItemLastPurchaseInfo";
		((ScrollableBase)this.grdItemLastPurchaseInfo).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdItemLastPurchaseInfo).Size = new System.Drawing.Size(315, 362);
		((System.Windows.Forms.Control)(object)this.grdItemLastPurchaseInfo).TabIndex = 0;
		this.grdItemLastPurchaseInfo.TotalRow = (InheritableBoolean)1;
		this.grdItemLastPurchaseInfo.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdItemLastPurchaseInfo).VisualStyle = (VisualStyle)4;
		this.grdItemLastPurchaseInfo.LinkClicked += new ColumnActionEventHandler(grdItemLastPurchaseInfo_LinkClicked);
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.label2);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(315, 23);
		this.panel6.TabIndex = 0;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(3, 5);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(277, 14);
		this.label2.TabIndex = 2;
		this.label2.Text = "Last Purchase Information Of Selected Item";
		this.PanelData.Controls.Add(this.txtPurchaseType);
		this.PanelData.Controls.Add(this.txtPoRate);
		this.PanelData.Controls.Add(this.labelPoRate);
		this.PanelData.Controls.Add(this.groupBox2);
		this.PanelData.Controls.Add(this.groupBox1);
		this.PanelData.Controls.Add(this.groupBoxGP);
		this.PanelData.Controls.Add(this.BtnRateApproval);
		this.PanelData.Controls.Add(this.BtnUpdateOnly);
		this.PanelData.Controls.Add(this.label33);
		this.PanelData.Controls.Add(this.label7);
		this.PanelData.Controls.Add(this.txtremarks);
		this.PanelData.Dock = System.Windows.Forms.DockStyle.Left;
		this.PanelData.Location = new System.Drawing.Point(500, 0);
		this.PanelData.Name = "PanelData";
		this.PanelData.Size = new System.Drawing.Size(419, 385);
		this.PanelData.TabIndex = 0;
		this.txtPurchaseType.BackColor = System.Drawing.SystemColors.Control;
		this.txtPurchaseType.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPurchaseType.Enabled = false;
		this.txtPurchaseType.Font = new System.Drawing.Font("Verdana", 10.75f);
		this.txtPurchaseType.Location = new System.Drawing.Point(120, 5);
		this.txtPurchaseType.Name = "txtPurchaseType";
		this.txtPurchaseType.Size = new System.Drawing.Size(174, 25);
		this.txtPurchaseType.TabIndex = 0;
		this.txtPoRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPoRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPoRate.Location = new System.Drawing.Point(324, 6);
		this.txtPoRate.Name = "txtPoRate";
		this.txtPoRate.ReadOnly = true;
		this.txtPoRate.Size = new System.Drawing.Size(83, 23);
		this.txtPoRate.TabIndex = 93;
		this.labelPoRate.AutoSize = true;
		this.labelPoRate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.labelPoRate.ForeColor = System.Drawing.SystemColors.ControlText;
		this.labelPoRate.Location = new System.Drawing.Point(293, 2);
		this.labelPoRate.Name = "labelPoRate";
		this.labelPoRate.Size = new System.Drawing.Size(30, 30);
		this.labelPoRate.TabIndex = 95;
		this.labelPoRate.Text = "PO\r\nRate";
		this.labelPoRate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWarehouse);
		this.groupBox2.Controls.Add(this.label20);
		this.groupBox2.Controls.Add(this.label36);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobLot);
		this.groupBox2.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold);
		this.groupBox2.ForeColor = System.Drawing.Color.Teal;
		this.groupBox2.Location = new System.Drawing.Point(3, 222);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(410, 76);
		this.groupBox2.TabIndex = 97;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Warehouse Routing and Lot Assignment";
		this.CmbWarehouse.AllowNull = (DefaultableBoolean)1;
		this.CmbWarehouse.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWarehouse.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWarehouse.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Appearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWarehouse).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance25).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance25;
		((SpecialBoxBase)((UltraGridBase)this.CmbWarehouse).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance26).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance26).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance26).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance27).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance29;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance30).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance31).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance31).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance31).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance32).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWarehouse).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWarehouse.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWarehouse).Location = new System.Drawing.Point(123, 17);
		((System.Windows.Forms.Control)(object)this.CmbWarehouse).Name = "CmbWarehouse";
		((System.Windows.Forms.Control)(object)this.CmbWarehouse).Size = new System.Drawing.Size(281, 26);
		((System.Windows.Forms.Control)(object)this.CmbWarehouse).TabIndex = 0;
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.ForeColor = System.Drawing.SystemColors.ControlText;
		this.label20.Location = new System.Drawing.Point(6, 23);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(115, 15);
		this.label20.TabIndex = 90;
		this.label20.Text = "WareHouse (Assign)";
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.ForeColor = System.Drawing.SystemColors.ControlText;
		this.label36.Location = new System.Drawing.Point(6, 52);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(89, 15);
		this.label36.TabIndex = 91;
		this.label36.Text = "JobLot (Assign)";
		this.CmbJobLot.AllowNull = (DefaultableBoolean)1;
		this.CmbJobLot.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobLot.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobLot.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance17).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance17).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance35;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance36).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance36).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance38).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance38;
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
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Location = new System.Drawing.Point(123, 46);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Name = "CmbJobLot";
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Size = new System.Drawing.Size(281, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).TabIndex = 1;
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWtCuton);
		this.groupBox1.Controls.Add(this.txtMoisture);
		this.groupBox1.Controls.Add(this.txtQtyForWtCut);
		this.groupBox1.Controls.Add(this.txtWeightCut);
		this.groupBox1.Controls.Add(this.txtRateCut);
		this.groupBox1.Controls.Add(this.ChkRateCutWillApply);
		this.groupBox1.Controls.Add(this.ChkWtCutWillApply);
		this.groupBox1.Controls.Add(this.labeldeduciton);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.label22);
		this.groupBox1.Controls.Add(this.txtWeightCutUom);
		this.groupBox1.Controls.Add(this.label37);
		this.groupBox1.Controls.Add(this.label29);
		this.groupBox1.Controls.Add(this.label30);
		this.groupBox1.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold);
		this.groupBox1.ForeColor = System.Drawing.Color.Teal;
		this.groupBox1.Location = new System.Drawing.Point(3, 30);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(410, 120);
		this.groupBox1.TabIndex = 1;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Quality Weight Touch && Rate Cut";
		this.CmbWtCuton.AllowNull = (DefaultableBoolean)1;
		this.CmbWtCuton.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWtCuton.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWtCuton.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWtCuton).DisplayLayout.Appearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbWtCuton).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWtCuton).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance39).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance39).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWtCuton).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance39;
		((AppearanceBase)appearance40).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWtCuton).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance40;
		((SpecialBoxBase)((UltraGridBase)this.CmbWtCuton).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance41).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance41).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWtCuton).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.CmbWtCuton).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWtCuton).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWtCuton).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance42;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWtCuton).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.CmbWtCuton).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWtCuton).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWtCuton).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance5;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance6).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWtCuton).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.CmbWtCuton).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWtCuton).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance7).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance7).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance7).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWtCuton).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance7;
		((AppearanceBase)appearance44).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWtCuton).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbWtCuton).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWtCuton).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWtCuton).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbWtCuton).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWtCuton).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbWtCuton).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWtCuton).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWtCuton).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWtCuton).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWtCuton.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWtCuton).Location = new System.Drawing.Point(290, 13);
		((System.Windows.Forms.Control)(object)this.CmbWtCuton).Name = "CmbWtCuton";
		((System.Windows.Forms.Control)(object)this.CmbWtCuton).Size = new System.Drawing.Size(114, 26);
		((System.Windows.Forms.Control)(object)this.CmbWtCuton).TabIndex = 1;
		this.CmbWtCuton.ValueChanged += new System.EventHandler(CmbWtCuton_Leave);
		((System.Windows.Forms.Control)(object)this.CmbWtCuton).Leave += new System.EventHandler(CmbWtCuton_Leave);
		this.txtMoisture.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtMoisture.Enabled = false;
		this.txtMoisture.Font = new System.Drawing.Font("Verdana", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtMoisture.Location = new System.Drawing.Point(290, 66);
		this.txtMoisture.Name = "txtMoisture";
		this.txtMoisture.Size = new System.Drawing.Size(114, 27);
		this.txtMoisture.TabIndex = 5;
		this.txtMoisture.Text = "0";
		this.txtMoisture.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtQtyForWtCut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtQtyForWtCut.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtQtyForWtCut.Location = new System.Drawing.Point(290, 41);
		this.txtQtyForWtCut.Name = "txtQtyForWtCut";
		this.txtQtyForWtCut.Size = new System.Drawing.Size(114, 23);
		this.txtQtyForWtCut.TabIndex = 3;
		this.txtQtyForWtCut.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtWeightCutUom_KeyPress);
		this.txtWeightCut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtWeightCut.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtWeightCut.Location = new System.Drawing.Point(121, 15);
		this.txtWeightCut.Name = "txtWeightCut";
		this.txtWeightCut.Size = new System.Drawing.Size(80, 23);
		this.txtWeightCut.TabIndex = 0;
		this.txtWeightCut.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtWeightCutUom_KeyPress);
		this.txtRateCut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRateCut.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRateCut.Location = new System.Drawing.Point(121, 68);
		this.txtRateCut.Name = "txtRateCut";
		this.txtRateCut.Size = new System.Drawing.Size(80, 23);
		this.txtRateCut.TabIndex = 4;
		this.txtRateCut.TextChanged += new System.EventHandler(txtItemRate_TextChanged);
		this.txtRateCut.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtWeightCutUom_KeyPress);
		this.ChkRateCutWillApply.AutoSize = true;
		this.ChkRateCutWillApply.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkRateCutWillApply.ForeColor = System.Drawing.Color.FromArgb(0, 0, 192);
		this.ChkRateCutWillApply.Location = new System.Drawing.Point(274, 97);
		this.ChkRateCutWillApply.Name = "ChkRateCutWillApply";
		this.ChkRateCutWillApply.Size = new System.Drawing.Size(133, 19);
		this.ChkRateCutWillApply.TabIndex = 7;
		this.ChkRateCutWillApply.Text = "Apply Rate Cut Later";
		this.ChkRateCutWillApply.UseVisualStyleBackColor = true;
		this.ChkWtCutWillApply.AutoSize = true;
		this.ChkWtCutWillApply.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkWtCutWillApply.ForeColor = System.Drawing.Color.FromArgb(0, 0, 192);
		this.ChkWtCutWillApply.Location = new System.Drawing.Point(121, 97);
		this.ChkWtCutWillApply.Name = "ChkWtCutWillApply";
		this.ChkWtCutWillApply.Size = new System.Drawing.Size(149, 19);
		this.ChkWtCutWillApply.TabIndex = 6;
		this.ChkWtCutWillApply.Text = "Apply Weight Cut Later";
		this.ChkWtCutWillApply.UseVisualStyleBackColor = true;
		this.labeldeduciton.AutoSize = true;
		this.labeldeduciton.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.labeldeduciton.ForeColor = System.Drawing.SystemColors.ControlText;
		this.labeldeduciton.Location = new System.Drawing.Point(4, 19);
		this.labeldeduciton.Name = "labeldeduciton";
		this.labeldeduciton.Size = new System.Drawing.Size(111, 15);
		this.labeldeduciton.TabIndex = 87;
		this.labeldeduciton.Text = "Weight Cut / Touch";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.SystemColors.ControlText;
		this.label3.Location = new System.Drawing.Point(4, 72);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(51, 15);
		this.label3.TabIndex = 92;
		this.label3.Text = "Rate Cut";
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 12f, System.Drawing.FontStyle.Bold);
		this.label22.ForeColor = System.Drawing.SystemColors.ControlText;
		this.label22.Location = new System.Drawing.Point(204, 69);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(76, 21);
		this.label22.TabIndex = 86;
		this.label22.Text = "Moisture";
		this.txtWeightCutUom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtWeightCutUom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtWeightCutUom.Location = new System.Drawing.Point(121, 41);
		this.txtWeightCutUom.Name = "txtWeightCutUom";
		this.txtWeightCutUom.ShortcutsEnabled = false;
		this.txtWeightCutUom.Size = new System.Drawing.Size(80, 23);
		this.txtWeightCutUom.TabIndex = 2;
		this.txtWeightCutUom.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtWeightCutUom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtWeightCutUom_KeyPress);
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.ForeColor = System.Drawing.SystemColors.ControlText;
		this.label37.Location = new System.Drawing.Point(4, 45);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(121, 15);
		this.label37.TabIndex = 93;
		this.label37.Text = "Weight Cut Uom (Kg)";
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.ForeColor = System.Drawing.SystemColors.ControlText;
		this.label29.Location = new System.Drawing.Point(205, 19);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(86, 15);
		this.label29.TabIndex = 66;
		this.label29.Text = "Weight Cut On";
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label30.ForeColor = System.Drawing.SystemColors.ControlText;
		this.label30.Location = new System.Drawing.Point(205, 45);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(82, 15);
		this.label30.TabIndex = 94;
		this.label30.Text = "Qty For Touch";
		this.groupBoxGP.Controls.Add(this.txtItemRate);
		this.groupBoxGP.Controls.Add(this.txtLessRate);
		this.groupBoxGP.Controls.Add(this.lblNetRate);
		this.groupBoxGP.Controls.Add(this.txtNetRate);
		this.groupBoxGP.Controls.Add(this.label28);
		this.groupBoxGP.Controls.Add(this.lblRateUom);
		this.groupBoxGP.Controls.Add(this.lblRate);
		this.groupBoxGP.Controls.Add(this.txtAddRate);
		this.groupBoxGP.Controls.Add(this.lblPremium);
		this.groupBoxGP.Controls.Add((System.Windows.Forms.Control)(object)this.CmbRateUom);
		this.groupBoxGP.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBoxGP.ForeColor = System.Drawing.Color.Teal;
		this.groupBoxGP.Location = new System.Drawing.Point(3, 151);
		this.groupBoxGP.Name = "groupBoxGP";
		this.groupBoxGP.Size = new System.Drawing.Size(410, 70);
		this.groupBoxGP.TabIndex = 2;
		this.groupBoxGP.TabStop = false;
		this.groupBoxGP.Text = "Apply Rate In Case Of : Gate Purchase";
		this.txtItemRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemRate.Location = new System.Drawing.Point(67, 15);
		this.txtItemRate.Name = "txtItemRate";
		this.txtItemRate.ReadOnly = true;
		this.txtItemRate.Size = new System.Drawing.Size(80, 23);
		this.txtItemRate.TabIndex = 0;
		this.txtItemRate.TextChanged += new System.EventHandler(txtItemRate_TextChanged);
		this.txtItemRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtWeightCutUom_KeyPress);
		this.txtLessRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtLessRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtLessRate.Location = new System.Drawing.Point(209, 15);
		this.txtLessRate.Name = "txtLessRate";
		this.txtLessRate.Size = new System.Drawing.Size(60, 23);
		this.txtLessRate.TabIndex = 1;
		this.txtLessRate.TextChanged += new System.EventHandler(txtItemRate_TextChanged);
		this.txtLessRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtWeightCutUom_KeyPress);
		this.lblNetRate.AutoSize = true;
		this.lblNetRate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblNetRate.ForeColor = System.Drawing.SystemColors.ControlText;
		this.lblNetRate.Location = new System.Drawing.Point(6, 45);
		this.lblNetRate.Name = "lblNetRate";
		this.lblNetRate.Size = new System.Drawing.Size(60, 15);
		this.lblNetRate.TabIndex = 89;
		this.lblNetRate.Text = "Net / Rate";
		this.txtNetRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNetRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNetRate.Location = new System.Drawing.Point(67, 41);
		this.txtNetRate.Name = "txtNetRate";
		this.txtNetRate.ReadOnly = true;
		this.txtNetRate.Size = new System.Drawing.Size(80, 23);
		this.txtNetRate.TabIndex = 3;
		this.txtNetRate.TabStop = false;
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.ForeColor = System.Drawing.SystemColors.ControlText;
		this.label28.Location = new System.Drawing.Point(149, 19);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(50, 15);
		this.label28.TabIndex = 92;
		this.label28.Text = "Less Cut";
		this.lblRateUom.AutoSize = true;
		this.lblRateUom.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblRateUom.ForeColor = System.Drawing.SystemColors.ControlText;
		this.lblRateUom.Location = new System.Drawing.Point(149, 45);
		this.lblRateUom.Name = "lblRateUom";
		this.lblRateUom.Size = new System.Drawing.Size(59, 15);
		this.lblRateUom.TabIndex = 67;
		this.lblRateUom.Text = "Rate Uom";
		this.lblRate.AutoSize = true;
		this.lblRate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblRate.ForeColor = System.Drawing.SystemColors.ControlText;
		this.lblRate.Location = new System.Drawing.Point(6, 19);
		this.lblRate.Name = "lblRate";
		this.lblRate.Size = new System.Drawing.Size(58, 15);
		this.lblRate.TabIndex = 84;
		this.lblRate.Text = "Item Rate";
		this.txtAddRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAddRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAddRate.Location = new System.Drawing.Point(328, 15);
		this.txtAddRate.Name = "txtAddRate";
		this.txtAddRate.Size = new System.Drawing.Size(76, 23);
		this.txtAddRate.TabIndex = 2;
		this.txtAddRate.TextChanged += new System.EventHandler(txtItemRate_TextChanged);
		this.txtAddRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtWeightCutUom_KeyPress);
		this.lblPremium.AutoSize = true;
		this.lblPremium.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblPremium.ForeColor = System.Drawing.SystemColors.ControlText;
		this.lblPremium.Location = new System.Drawing.Point(273, 19);
		this.lblPremium.Name = "lblPremium";
		this.lblPremium.Size = new System.Drawing.Size(56, 15);
		this.lblPremium.TabIndex = 85;
		this.lblPremium.Text = "Premium";
		this.CmbRateUom.AllowNull = (DefaultableBoolean)1;
		this.CmbRateUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbRateUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbRateUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance11).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance11).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance11).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbRateUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance12).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance12;
		((SpecialBoxBase)((UltraGridBase)this.CmbRateUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance13).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance13).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance13).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance14).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance16;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance18).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance19).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbRateUom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Location = new System.Drawing.Point(209, 39);
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Name = "CmbRateUom";
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Size = new System.Drawing.Size(195, 26);
		((System.Windows.Forms.Control)(object)this.CmbRateUom).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Leave += new System.EventHandler(CmbRateUom_Leave);
		this.BtnRateApproval.BackColor = System.Drawing.Color.Teal;
		this.BtnRateApproval.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnRateApproval.Font = new System.Drawing.Font("Verdana", 9.25f, System.Drawing.FontStyle.Bold);
		this.BtnRateApproval.ForeColor = System.Drawing.Color.White;
		this.BtnRateApproval.Location = new System.Drawing.Point(217, 355);
		this.BtnRateApproval.Name = "BtnRateApproval";
		this.BtnRateApproval.Size = new System.Drawing.Size(190, 27);
		this.BtnRateApproval.TabIndex = 5;
		this.BtnRateApproval.Text = "Rate Approval";
		this.BtnRateApproval.UseVisualStyleBackColor = false;
		this.BtnRateApproval.Visible = false;
		this.BtnRateApproval.Click += new System.EventHandler(BtnRateApproval_Click);
		this.BtnUpdateOnly.BackColor = System.Drawing.Color.Teal;
		this.BtnUpdateOnly.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnUpdateOnly.Font = new System.Drawing.Font("Verdana", 9.25f, System.Drawing.FontStyle.Bold);
		this.BtnUpdateOnly.ForeColor = System.Drawing.Color.White;
		this.BtnUpdateOnly.Location = new System.Drawing.Point(126, 355);
		this.BtnUpdateOnly.Name = "BtnUpdateOnly";
		this.BtnUpdateOnly.Size = new System.Drawing.Size(82, 27);
		this.BtnUpdateOnly.TabIndex = 4;
		this.BtnUpdateOnly.Text = "Update This Information Only";
		this.BtnUpdateOnly.UseVisualStyleBackColor = false;
		this.BtnUpdateOnly.Visible = false;
		this.BtnUpdateOnly.Click += new System.EventHandler(BtnUpdateOnly_Click);
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI", 10f, System.Drawing.FontStyle.Bold);
		this.label33.Location = new System.Drawing.Point(3, 8);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(105, 19);
		this.label33.TabIndex = 95;
		this.label33.Text = "Purchase Type";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(3, 304);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(60, 17);
		this.label7.TabIndex = 65;
		this.label7.Text = "Remarks";
		this.txtremarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtremarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtremarks.Location = new System.Drawing.Point(126, 301);
		this.txtremarks.Multiline = true;
		this.txtremarks.Name = "txtremarks";
		this.txtremarks.Size = new System.Drawing.Size(281, 51);
		this.txtremarks.TabIndex = 3;
		this.panel14.Controls.Add(this.panel17);
		this.panel14.Controls.Add(this.panel16);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel14.Location = new System.Drawing.Point(0, 0);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(500, 385);
		this.panel14.TabIndex = 2;
		this.panel17.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistroryPurAnalysis);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel17.Location = new System.Drawing.Point(0, 23);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(500, 362);
		this.panel17.TabIndex = 1;
		this.grdHistroryPurAnalysis.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdHistroryPurAnalysis).BackColor = System.Drawing.Color.White;
		grdHistroryPurAnalysis_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistroryPurAnalysis.DesignTimeLayout = grdHistroryPurAnalysis_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistroryPurAnalysis).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistroryPurAnalysis.FilterMode = (FilterMode)2;
		this.grdHistroryPurAnalysis.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistroryPurAnalysis).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistroryPurAnalysis.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdHistroryPurAnalysis).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdHistroryPurAnalysis).Name = "grdHistroryPurAnalysis";
		((ScrollableBase)this.grdHistroryPurAnalysis).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistroryPurAnalysis).Size = new System.Drawing.Size(500, 362);
		((System.Windows.Forms.Control)(object)this.grdHistroryPurAnalysis).TabIndex = 3;
		this.grdHistroryPurAnalysis.TotalRow = (InheritableBoolean)1;
		this.grdHistroryPurAnalysis.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistroryPurAnalysis).VisualStyle = (VisualStyle)4;
		this.panel16.BackColor = System.Drawing.Color.Teal;
		this.panel16.Controls.Add(this.PaymentVoucherLable);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel16.Location = new System.Drawing.Point(0, 0);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(500, 23);
		this.panel16.TabIndex = 0;
		this.PaymentVoucherLable.AutoSize = true;
		this.PaymentVoucherLable.Font = new System.Drawing.Font("Tahoma", 11f, System.Drawing.FontStyle.Bold);
		this.PaymentVoucherLable.ForeColor = System.Drawing.Color.White;
		this.PaymentVoucherLable.Location = new System.Drawing.Point(3, 3);
		this.PaymentVoucherLable.Name = "PaymentVoucherLable";
		this.PaymentVoucherLable.Size = new System.Drawing.Size(299, 18);
		this.PaymentVoucherLable.TabIndex = 2;
		this.PaymentVoucherLable.Text = "Purchase Analysis Parameters' Results";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1234, 711);
		base.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistory);
		base.Controls.Add(this.panel2);
		base.Controls.Add(this.panel1);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Name = "LabPurchaseAnalysisForApproval";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(frmStockWithSupplierHistory_Load);
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).EndInit();
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdItemLastPurchaseInfo).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.PanelData.ResumeLayout(false);
		this.PanelData.PerformLayout();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbWarehouse).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).EndInit();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbWtCuton).EndInit();
		this.groupBoxGP.ResumeLayout(false);
		this.groupBoxGP.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbRateUom).EndInit();
		this.panel14.ResumeLayout(false);
		this.panel17.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistroryPurAnalysis).EndInit();
		this.panel16.ResumeLayout(false);
		this.panel16.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
