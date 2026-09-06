// using Architecture.Reports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.BLL.Lab;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Common;
using Architecture.Model.Lab;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
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
namespace Architecture.WinApp.Lab
{

public class InvLabSampleAnalysis : Form
{
	private Attachment AT = new Attachment();

	private DropDownBind DDL = new DropDownBind();

	private Rightsobjects formrights = new Rightsobjects();

	public UserAccount UserAccount = new UserAccount();

	private DataTable grdtable = new DataTable();

	private DataTable table = new DataTable();

	private DataTable tablehistory = new DataTable();

	private int RecId;

	private bool UpdateMode;

	private bool Confiquration;

	private string saveDirectory = "";

	private string fileSavePath = "";

	private string ofd = "";

	private string fileSavePathcookingpic = "";

	private string ofdcookingpic = "";

	private List<InvLabSampleAnalysisDetail> detaillst = new List<InvLabSampleAnalysisDetail>();

	private IContainer components;

	private TabControl tabControl1;

	private TabPage Form;

	private Panel panel2;

	internal GridEX grd;

	private Panel panel5;

	private Label label6;

	private Label label8;

	private Label label1;

	private DateTimePicker txtdocdate;

	private TextBox txtremarks;

	private Label label11;

	private TextBox txtManaulReportNo;

	private TextBox txtdocno;

	private Label label10;

	private Label label9;

	private Label label2;

	private Label label42;

	private ToolStrip toolStrip2;

	private ToolStripButton BtnNew;

	private ToolStripButton BtnUpdate;

	private ToolStripButton BtnSave;

	private ToolStripButton BtnPrint;

	private ToolStripButton BtnAttachments;

	private TabPage History;

	private Panel panel8;

	internal GridEX grdhistory;

	private Panel panel3;

	private Label label15;

	private Panel panel1;

	private BackgroundWorker backgroundWorker1;

	private Label label16;

	private Label label17;

	private Button BtnBrowseCookingPic;

	private Button BtnBrowseAnaylsisPic;

	private PictureBox PicBCookingImage;

	private PictureBox PicBAnalysisImage;

	internal UltraCombo CmbSupplierName;

	internal UltraCombo CmbAnalysisGroup;

	internal UltraCombo CmbJobLot;

	internal UltraCombo CmbItemName;

	internal UltraCombo CmbCropYear;

	internal UltraCombo CmbPurchaseOrder;

	private Label label3;

	private CheckBox IsPrint;

	private Panel panel6;

	internal GridEX grdSubItems;

	private Panel panel4;

	internal UltraCombo CmbSampleLogNo;

	private Label label4;

	private Panel panel9;

	internal GridEX grdHistoryDetail;

	private Panel panel7;

	private CtrlGrdBar ctrlGrdBar1;

	internal UltraCombo CmbStatus;

	private Label label5;

	private ToolStripButton BtnRefresh;

	private TextBox txtPartyLotRefNo;

	private Label label7;

	private TextBox txtQty;

	private Label label12;

	private Panel panel12;

	private Panel panel13;

	private Panel panel11;

	private Panel panel10;

	private Panel panel14;

	private Label label18;

	private Label label14;

	private Label label13;

	private ToolStrip toolStrip1;

	private Button btnClearAnalysisPic;

	private Button btnClearCookingPic;

	private Panel panel20;

	private GroupBox groupBox2;

	private Label label19;

	private Button btnShowHistory;

	internal UltraCombo CmbCropYearHistory;

	private Label label20;

	internal UltraCombo CmbSupplierCustomerHistory;

	private Label label24;

	private Label label23;

	internal UltraCombo CmbAnalysisGroupHistory;

	internal UltraCombo CmbItemHistory;

	private Label label56;

	private Label label57;

	private TextBox ToDocNoHistory;

	private TextBox FromDocNoHistory;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	private Label label21;

	internal UltraCombo CmbJobLotHistory;

	private Label label22;

	internal UltraCombo CmbStatusHistory;

	private ToolStripButton btnNewHistory;

	private ToolStripButton btnRefreshHistory;

	private Button btnViewCookingPic;

	private Button BtnViewAnalysisPic;

	internal UltraCombo CmbCommissionAgent;

	private Label label25;

	internal UltraCombo CmbAnalysisType;

	private Label label26;

	public InvLabSampleAnalysis(UserAccount puserAccount)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = puserAccount;
	}

	private void InvLabSampleAnalysis_Load(object sender, EventArgs e)
	{
		try
		{
			formrights = CommonServices.SetRightsValueInRightsObject(base.Name);
			BtnSave.Enabled = formrights.DoHaveSaveRight;
			BtnPrint.Enabled = formrights.DoHavePrintRights;
			BtnUpdate.Enabled = formrights.DoHaveUpdateRights;
			txtdocdate.Focus();
			BtnSave.Visible = true;
			BtnUpdate.Visible = false;
			itemcode();
			ItemFill();
			AnalysisTypeFill(InvLabAnalysisGroup.AnalysisType());
			cmbsupplierfill();
			CropYearFill();
			combojoblotfill();
			LabstatusFill();
			GetConfigurationsFromGlobalAndBindValuesInColumns();
			table.Clear();
			grdtable.Columns.Add("ItemId");
			grdtable.Columns.Add("AnalysisGroup");
			grdtable.Columns.Add("AnalysisItem");
			grdtable.Columns.Add("MinValue");
			grdtable.Columns.Add("MaxValue");
			grdtable.Columns.Add("ResultValue", typeof(double));
			grd.DataSource = detaillst;
			grd.RetrieveStructure();
			Grdsetting();
			FromDateHistory.Value = DateTime.Now.AddDays(-7.0);
			ToDateHistory.Value = DateTime.Now;
			HistoryComboFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryComboFill()
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		try
		{
			DataTable dtSupplier = new DataTable();
			dtSupplier.Columns.Add("Id");
			dtSupplier.Columns.Add("name");
			DataTable dtItem = dtSupplier.Clone();
			DataTable dtCrop = dtSupplier.Clone();
			DataTable dtJob = dtSupplier.Clone();
			DataTable dtStatus = dtSupplier.Clone();
			DataTable dtAnalysisGroup = dtSupplier.Clone();
			DataTable dt = InvLabSampleAnalysisHeader.GetDataForDropDownFromInvLabSampleAnalysisHeader(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeIds = "302"
			});
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToString(dt.Rows[i]["Activity"]) == "Item")
					{
						dtItem.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
					else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "Supplier")
					{
						dtSupplier.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
					else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "CropYear")
					{
						dtCrop.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
					else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "JobLot")
					{
						dtJob.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
					else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "Status")
					{
						dtStatus.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
					else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "AnalysisGroup")
					{
						dtAnalysisGroup.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
				}
			}
			DDL.BindDDL(dtSupplier, CmbSupplierCustomerHistory, "Id", "name", "Supplier Customer", ZeroIndex: false);
			DDL.BindDDL(dtItem, CmbItemHistory, "Id", "name", "Item Name", ZeroIndex: false);
			DDL.BindDDL(dtCrop, CmbCropYearHistory, "Id", "name", "Crop Year", ZeroIndex: false);
			DDL.BindDDL(dtJob, CmbJobLotHistory, "Id", "name", "Job Lot", ZeroIndex: false);
			DDL.BindDDL(dtStatus, CmbStatusHistory, "Id", "name", "Status", ZeroIndex: false);
			DDL.BindDDL(dtAnalysisGroup, CmbAnalysisGroupHistory, "Id", "name", "Analysis Group", ZeroIndex: false);
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
			int jobId = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("Job/Lot"));
			if (jobId != 0)
			{
				CmbJobLot.Value = jobId;
			}
			int cropYearId = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("Default Crop Year"));
			if (cropYearId != 0 && ((UltraGridBase)CmbCropYear).DataSource != null)
			{
				CmbCropYear.Value = cropYearId;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show("Configuration Error" + ex.Message);
		}
	}

	private void itemcode()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			int code = InvLabSampleAnalysisHeader.GenerateCode(new InvLabSampleAnalysisHeader
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (code > 0)
			{
				txtdocno.Text = code.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SampleLogNumberFill()
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		try
		{
			if (Conversion.ToInt(CmbSupplierName.Value) > 0 && Conversion.ToInt(CmbItemName.Value) > 0)
			{
				DataTable dt = InvLabSampleAnalysisHeader.GetSampleLogForSampleAnalysis(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					SupplierCustomerId = Conversion.ToInt(CmbSupplierName.Value),
					ItemId = Conversion.ToInt(CmbItemName.Value)
				});
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDLNew(dt, CmbSampleLogNo, "Id", "SampleNo", "Sample Log No", false);
					((UltraGridBase)CmbSampleLogNo).Rows[0].Activate();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbsupplierfill()
	{
		try
		{
			DataTable dt = CommonServices.SupplierCustomerGetforComboServiceBind();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbSupplierName, "Id", "CompanyName", "Supplier Customer", ZeroIndex: false);
				DDL.BindDDL(dt, CmbCommissionAgent, "Id", "CompanyName", "Commission Agent", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PurchaseOrderFill()
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Expected O, but got Unknown
		try
		{
			if (Conversion.ToInt(CmbSupplierName.Value) != 0 && Conversion.ToInt(CmbItemName.Value) != 0)
			{
				DataTable dtPurchaseOrder = PurchaseOrder.GetForComboPoBySupplierandItemId(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					SupplierCustomerId = Conversion.ToInt(CmbSupplierName.Value),
					ItemId = Conversion.ToInt(CmbItemName.Value)
				});
				if (dtPurchaseOrder.Rows.Count > 0)
				{
					DDL.BindDDLNew(dtPurchaseOrder, CmbPurchaseOrder, "Id", "DocNo", "Order No", false);
					((UltraGridBase)CmbPurchaseOrder).Rows[0].Activate();
				}
				else
				{
					((Control)(object)CmbPurchaseOrder).Text = string.Empty;
					((UltraGridBase)CmbPurchaseOrder).DataSource = null;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AnalysisTypeFill(DataTable dt)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbAnalysisType, dt, "Id", "Type", "Analysis Type", AllColumns: false, Conversion.ToInt(CmbAnalysisType.Value), insertDefaultRow: true);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private DataTable AnalysisGroupdtFill(int TypeId)
	{
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Expected O, but got Unknown
		try
		{
			DataTable dtAnalysisGroup = new DataTable();
			if (dtAnalysisGroup != null && dtAnalysisGroup.Columns.Count == 0)
			{
				dtAnalysisGroup.Columns.Add("Id", typeof(int));
				dtAnalysisGroup.Columns.Add("AnalysisGroupDescription", typeof(string));
				dtAnalysisGroup.Columns.Add("GroupTypeId", typeof(int));
				dtAnalysisGroup.Columns.Add("GroupType", typeof(string));
			}
			ReportsParameters obj = new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocDate = txtdocdate.Value
			};
			if (TypeId == 1)
			{
				DataTable allOrById = InvLabAnalysisGroup.GetAllOrById(obj);
				dtAnalysisGroup.Clear();
				foreach (DataRow row in allOrById.Rows)
				{
					dtAnalysisGroup.Rows.Add(row["Id"], row["AnalysisGroupDescription"], row["GroupType"], row["InvParentCateDescription"]);
				}
			}
			else
			{
				DataTable dataTable = LabAnalysisStandardSchedule.ComboFill(obj);
				dtAnalysisGroup.Clear();
				foreach (DataRow row2 in dataTable.Rows)
				{
					string name = Convert.ToString(row2["LabAnalysisStandardScheduleName"]);
					string date = Convert.ToDateTime(row2["EffectiveDateFrom"]).ToShortDateString();
					dtAnalysisGroup.Rows.Add(row2["LabAnalysisGroupId"], name + " - " + date, row2["GroupTypeId"], row2["GroupType"]);
				}
			}
			return dtAnalysisGroup;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void AnalysisGroupBind(DataTable dt)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbAnalysisGroup, dt, "Id", "AnalysisGroupDescription", "Analysis Group", AllColumns: true, Conversion.ToInt(CmbAnalysisGroup.Value), insertDefaultRow: true);
			if (dt != null && dt.Rows.Count > 0)
			{
				CmbAnalysisGroup_Leave(null, null);
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
			DataTable dt = CommonServices.ItemGetForComboServiceBind();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbItemName, "Id", "ItemName", "Item", ZeroIndex: false);
			}
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
			DataTable dtCropYear = CommonServices.CropYearGetAllService();
			DDL.BindDDLNew(dtCropYear, CmbCropYear, "Id", "CropYear", "Crop Year", false);
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
			DataTable dt = CommonServices.JobLotGetAllService();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbJobLot, "Id", "JobLotDescription", "JobLot", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LabstatusFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Status");
			dt.Rows.Add(1, "Accepted");
			dt.Rows.Add(2, "Rejected");
			DDL.BindDDL(dt, CmbStatus, "Id", "Status", "Analysis Status", ZeroIndex: false);
			((UltraGridBase)CmbStatus).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbPurchaseOrder_Leave(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(CmbPurchaseOrder.Value) > 0 && Conversion.ToInt(CmbAnalysisGroup.Value) > 0)
			{
				BindItemsByAnalysisGroupandOrderId(Conversion.ToInt(((UltraDropDownBase)CmbAnalysisGroup).SelectedRow.Cells["GroupTypeId"].Value), Conversion.ToInt(CmbPurchaseOrder.Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbAnalysisType_Leave(object sender, EventArgs e)
	{
		try
		{
			int Id = Conversion.ToInt(CmbAnalysisType.Value);
			AnalysisGroupBind(AnalysisGroupdtFill(Id));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbAnalysisGroup_Leave(object sender, EventArgs e)
	{
		try
		{
			int TypeId = Conversion.ToInt(CmbAnalysisType.Value);
			DetailGridFillByGroup(GetDtForDetailGrid(TypeId));
			ItemBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemBind()
	{
		try
		{
			int OrderId = Conversion.ToInt(CmbPurchaseOrder.Value);
			int GroupId = Conversion.ToInt(CmbAnalysisGroup.Value);
			UltraGridRow Row = ((UltraDropDownBase)CmbAnalysisGroup).SelectedRow;
			int GroupTypeId = 0;
			if (GroupId > 0 && Row != null)
			{
				GroupTypeId = Conversion.ToInt(Row.Cells["GroupTypeId"].Value);
			}
			if (OrderId > 0 && GroupId > 0)
			{
				BindItemsByAnalysisGroupandOrderId(GroupTypeId, OrderId);
			}
			else if (GroupId > 0)
			{
				BindItemsByAnalysisGroup(GroupTypeId);
			}
			else
			{
				ItemFill();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindItemsByAnalysisGroupandOrderId(int GroupTypeId, int OrderId)
	{
		try
		{
			DataTable dt = InvLabAnalysisGroup.GetAllItemsFromByAnalysisGroupandPurchaseOrder(UserAccount.OrganizationId, UserAccount.CompanyId, GroupTypeId, OrderId);
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbItemName, "Id", "ItemName", "Item", false);
				return;
			}
			((UltraGridBase)CmbItemName).DataSource = null;
			((Control)(object)CmbItemName).Text = string.Empty;
			CmbAnalysisGroup.Focus();
			throw new Exception("Please Select another Group Analysis,because Items not found against selected Order and analysis Group");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindItemsByAnalysisGroup(int GroupTypeId)
	{
		try
		{
			DataTable dt = InvLabAnalysisGroup.GetAllItemsFromByAnalysisGroup(UserAccount.OrganizationId, UserAccount.CompanyId, GroupTypeId);
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbItemName, "Id", "ItemName", "Item", ZeroIndex: false);
				return;
			}
			((UltraGridBase)CmbItemName).DataSource = null;
			((Control)(object)CmbItemName).Text = string.Empty;
			CmbAnalysisGroup.Focus();
			throw new Exception("Please Select another Group Analysis,because Items not found against selected analysis Group");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbitem_Leave(object sender, EventArgs e)
	{
		try
		{
			PurchaseOrderFill();
			SampleLogNumberFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbsupplier_Leave(object sender, EventArgs e)
	{
		try
		{
			PurchaseOrderFill();
			SampleLogNumberFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtdocdate_ValueChanged(object sender, EventArgs e)
	{
		CmbAnalysisType_Leave(null, null);
	}

	private bool formvalidation()
	{
		if (txtManaulReportNo.Text.Trim() == "")
		{
			MessageBox.Show("Please Report No");
			txtManaulReportNo.Focus();
			return false;
		}
		if (((Control)(object)CmbAnalysisType).Text.Trim() == "")
		{
			MessageBox.Show("Please select Analysis Type");
			CmbAnalysisType.Focus();
			return false;
		}
		if (((Control)(object)CmbAnalysisGroup).Text.Trim() == "")
		{
			MessageBox.Show("Please select Analysis Standard Group");
			CmbAnalysisGroup.Focus();
			return false;
		}
		if ((((UltraGridBase)CmbSupplierName).ActiveRow == null || Conversion.ToInt(CmbSupplierName.Value) == 0) && (((UltraGridBase)CmbCommissionAgent).ActiveRow == null || Conversion.ToInt(CmbCommissionAgent.Value) == 0))
		{
			MessageBox.Show("Please select Supplier or Commission Agent");
			CmbSupplierName.Focus();
			return false;
		}
		return true;
	}

	private DataTable GetDtForDetailGrid(int TypeId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.Id = Conversion.ToInt(CmbAnalysisGroup.Value);
			obj.DocDate = txtdocdate.Value;
			if (TypeId == 0 || obj.Id == 0)
			{
				return null;
			}
			if (TypeId == 1)
			{
				return InvLabGroupAnalysisStandards.GetParametersFromGroupStandards(obj);
			}
			return LabAnalysisStandardSchedule.GetParametersFromGroupStandardSchedule(obj);
		}
		catch (Exception)
		{
			throw;
		}
	}

	private void DetailGridFillByGroup(DataTable dtDetail)
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Expected O, but got Unknown
		detaillst.Clear();
		if (dtDetail != null && dtDetail.Rows.Count > 0)
		{
			List<InvLabSampleAnalysiSubParamsDetail> lstsub = new List<InvLabSampleAnalysiSubParamsDetail>();
			for (int j = 0; j < dtDetail.Rows.Count; j++)
			{
				if (Conversion.ToInt(dtDetail.Rows[j]["SubParamsId"]) > 0)
				{
					InvLabSampleAnalysiSubParamsDetail objsub = new InvLabSampleAnalysiSubParamsDetail();
					objsub.InvParentParameterId = Conversion.ToInt(dtDetail.Rows[j]["ParentParameterId"]);
					objsub.SubParameterId = Conversion.ToInt(dtDetail.Rows[j]["SubParamsId"]);
					objsub.SubParameterName = Conversion.ToString(dtDetail.Rows[j]["SubParameterName"]);
					objsub.ResultValue = 0.0;
					lstsub.Add(objsub);
				}
			}
			int ParamsId = 0;
			for (int i = 0; i < dtDetail.Rows.Count; i++)
			{
				InvLabSampleAnalysisDetail lstobj = new InvLabSampleAnalysisDetail();
				lstobj.InvLabSampleAnalysiSubParamsDetailsList = new List<InvLabSampleAnalysiSubParamsDetail>();
				if (ParamsId == Conversion.ToInt(dtDetail.Rows[i]["InvLabAnalysisItemsId"]))
				{
					continue;
				}
				bool flag = true;
				for (int k = 0; k < detaillst.Count; k++)
				{
					if (detaillst[k].InvLabAnalysisItemsId == Conversion.ToInt(dtDetail.Rows[i]["InvLabAnalysisItemsId"]))
					{
						flag = false;
						break;
					}
				}
				if (!flag)
				{
					break;
				}
				lstobj.InvLabAnalysisItemsId = Conversion.ToInt(dtDetail.Rows[i]["InvLabAnalysisItemsId"]);
				lstobj.AnalysisGroupDescription = Conversion.ToString(dtDetail.Rows[i]["AnalysisGroupDescription"]);
				lstobj.AnalysisParameterDescription = Conversion.ToString(dtDetail.Rows[i]["AnalysisParameterDescription"]);
				lstobj.MinValue = Conversion.ToDouble(dtDetail.Rows[i]["MinValue"]);
				lstobj.MaxValue = Conversion.ToDouble(dtDetail.Rows[i]["MaxValue"]);
				lstobj.ResultValue = 0.0;
				lstobj.InvLabSampleAnalysiSubParamsDetailsList = lstsub.FindAll((InvLabSampleAnalysiSubParamsDetail x) => lstobj.InvLabAnalysisItemsId == x.InvParentParameterId);
				detaillst.Add(lstobj);
				ParamsId = Conversion.ToInt(dtDetail.Rows[i]["InvLabAnalysisItemsId"]);
			}
		}
		BindGrid();
	}

	private void BindGrid()
	{
		try
		{
			grd.DataSource = detaillst;
			grd.RetrieveStructure();
			Grdsetting();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void Grdsetting()
	{
		try
		{
			grd.AllowDelete = (InheritableBoolean)2;
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["InvLabAnalysisItemsId"].Visible = false;
			grd.RootTable.Columns["InvLabSampleAnalysisHeaderId"].Visible = false;
			grd.RootTable.Columns["AnalysisGroupDescription"].Visible = false;
			Dictionary<string, EditType> editableColumns = new Dictionary<string, EditType>
			{
				{
					"ResultValue",
					(EditType)1
				},
				{
					"RemarksDetail",
					(EditType)1
				}
			};
			GridEX_Helper.GridWrappingAndColumnSettings(grd, 3, 3, null, editableColumns);
			grd.RootTable.Columns["AnalysisParameterDescription"].Caption = "Analysis Parameter";
			grd.RootTable.Columns["AnalysisParameterDescription"].Width = 150;
			grd.RootTable.Columns["MinValue"].Width = 55;
			grd.RootTable.Columns["MaxValue"].Width = 55;
			grd.RootTable.Columns["ResultValue"].Width = 55;
			grd.RootTable.Columns["AnalysisParameterDescription"].ColumnType = (ColumnType)5;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdSubsetting()
	{
		try
		{
			grdSubItems.AllowDelete = (InheritableBoolean)2;
			grdSubItems.ColumnAutoResize = true;
			grdSubItems.RootTable.Columns["Id"].Visible = false;
			grdSubItems.RootTable.Columns["Id"].EditType = (EditType)0;
			grdSubItems.RootTable.Columns["InvParentParameterId"].EditType = (EditType)0;
			grdSubItems.RootTable.Columns["SubParameterId"].EditType = (EditType)0;
			grdSubItems.RootTable.Columns["InvLabSampleAnalysisDetailId"].EditType = (EditType)0;
			grdSubItems.RootTable.Columns["InvLabSampleAnalysisHeaderId"].EditType = (EditType)0;
			grdSubItems.RootTable.Columns["SubParameterName"].EditType = (EditType)0;
			grdSubItems.RootTable.Columns["ParentParameterName"].EditType = (EditType)0;
			grdSubItems.RootTable.Columns["ParentParameterName"].Caption = "Parent Parameter";
			grdSubItems.RootTable.Columns["SubParameterName"].Caption = "Sub Parameter";
			grdSubItems.RootTable.Columns["InvParentParameterId"].Visible = false;
			grdSubItems.RootTable.Columns["SubParameterId"].Visible = false;
			grdSubItems.RootTable.Columns["InvLabSampleAnalysisDetailId"].Visible = false;
			grdSubItems.RootTable.Columns["InvLabSampleAnalysisHeaderId"].Visible = false;
			grdSubItems.RootTable.Columns["ResultValue"].AggregateFunction = (AggregateFunction)2;
			grdSubItems.RootTable.Columns["ResultValue"].FormatString = "#,#.##";
			grdSubItems.RootTable.Columns["ResultValue"].TotalFormatString = "#,#.##";
			grdSubItems.RootTable.Columns["ResultValue"].TextAlignment = (TextAlignment)3;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_CellUpdated_1(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "ResultValue")
			{
				GridEXRow currentRow = grd.CurrentRow;
				Conversion.ToDouble(currentRow.Cells["MinValue"].Value);
				Conversion.ToDouble(currentRow.Cells["MaxValue"].Value);
				Conversion.ToDouble(currentRow.Cells["ResultValue"].Value);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grd_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow item = grd.CurrentRow;
			grdSubItems.DataSource = detaillst[item.RowIndex].InvLabSampleAnalysiSubParamsDetailsList;
			grdSubItems.RetrieveStructure();
			GrdSubsetting();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		try
		{
			if (!formvalidation())
			{
				return;
			}
			InvLabSampleAnalysisHeader labheader = new InvLabSampleAnalysisHeader();
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
			if (detaillst.Count == 0)
			{
				throw new Exception("No detail record found");
			}
			double Result = 0.0;
			for (int i = 0; i < detaillst.Count; i++)
			{
				double TotalValueParent = detaillst[i].ResultValue;
				Result += detaillst[i].ResultValue;
				double TotalValueSub = 0.0;
				for (int j = 0; j < detaillst[i].InvLabSampleAnalysiSubParamsDetailsList?.Count; j++)
				{
					TotalValueSub += detaillst[i].InvLabSampleAnalysiSubParamsDetailsList[j].ResultValue;
				}
				if (TotalValueParent != TotalValueSub && detaillst[i].InvLabSampleAnalysiSubParamsDetailsList.Count > 0)
				{
					throw new Exception("Total Value of all sub parameters for " + detaillst[i].AnalysisParameterDescription + " must be equal to value of " + detaillst[i].AnalysisParameterDescription);
				}
			}
			if (Result == 0.0)
			{
				throw new Exception("ResultValue cannot be equal to zero");
			}
			labheader.DocumentTypeId = 302;
			labheader.DocNo = Conversion.ToInt((object)txtdocno.Text);
			labheader.DocDate = Conversion.ToDateTime((object)txtdocdate.Value);
			labheader.ApprovedDate = DateTime.Now;
			labheader.ReportNo = Conversion.ToString((object)txtManaulReportNo.Text);
			labheader.ItemId = Conversion.ToInt(CmbItemName.Value);
			labheader.SupplierCustomerId = Conversion.ToInt(CmbSupplierName.Value);
			labheader.CommissionAgentId = Conversion.ToInt(CmbCommissionAgent.Value);
			labheader.PartyLotRefNo = txtPartyLotRefNo.Text.Trim();
			labheader.NoOfBagsInspected = Conversion.ToDouble((object)txtQty.Text);
			labheader.OrderId = Conversion.ToInt(CmbPurchaseOrder.Value);
			labheader.InvLabAnalysisGroupId = Conversion.ToInt(CmbAnalysisGroup.Value);
			labheader.AnalysisTypeId = Conversion.ToInt(CmbAnalysisType.Value);
			labheader.InvLabSampleLogRegisterId = Conversion.ToInt(CmbSampleLogNo.Value);
			labheader.Crop = Conversion.ToString((object)((Control)(object)CmbCropYear).Text);
			labheader.JobLotId = Conversion.ToInt(CmbJobLot.Value);
			labheader.Remarks = Conversion.ToString((object)txtremarks.Text);
			if (((Control)(object)CmbStatus).Text == "Accepted")
			{
				labheader.IsAccepted = true;
			}
			else
			{
				labheader.IsAccepted = false;
			}
			labheader.IsApproved = false;
			labheader.AnalysisPic = Conversion.ToString((object)fileSavePath);
			labheader.CookingPic = Conversion.ToString((object)fileSavePathcookingpic);
			labheader.OrganizationId = UserAccount.OrganizationId;
			labheader.CompanyId = UserAccount.CompanyId;
			labheader.EntryUser = UserAccount.ID;
			labheader.ModifyUser = UserAccount.ID;
			labheader.LabSampleDetail = new List<InvLabSampleAnalysisDetail>();
			labheader.LabSampleDetail = detaillst;
			labheader.dmsAttachment = new List<DMSAttachments>();
			labheader.DeleteAttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<InvLabSampleAnalysisHeader>(labheader, base.Name, AT, "dmsAttachment", "DeleteAttachmentsList");
			List<string> DeleteImgList = new List<string>();
			if (Conversion.ToString(PicBAnalysisImage.Tag) != null && Conversion.ToString(PicBAnalysisImage.Tag) != string.Empty && PicBAnalysisImage.Image == null)
			{
				DeleteImgList.Add(Conversion.ToString(PicBAnalysisImage.Tag));
			}
			if (Conversion.ToString(PicBCookingImage.Tag) != null && Conversion.ToString(PicBCookingImage.Tag) != string.Empty && PicBCookingImage.Image == null)
			{
				DeleteImgList.Add(Conversion.ToString(PicBCookingImage.Tag));
			}
			int success = InvLabSampleAnalysisHeader.Save(labheader);
			if (RecId > 0)
			{
				MessageBox.Show("Update Successfully");
				FormHelper.DeleteAllAttachmentsFromDb(AT, RecId, 302, base.Name);
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
			if (ofd != null && ofd != string.Empty && fileSavePath != null && fileSavePath != string.Empty)
			{
				if (!Directory.Exists(clsGlobalVariables.AttachmentFolderPath))
				{
					throw new Exception("Attachment Folder Path Not Configure Properly");
				}
				fileSavePath = System.IO.Path.Combine(clsGlobalVariables.AttachmentFolderPath, fileSavePath);
				File.Copy(ofd, fileSavePath, overwrite: true);
				ofd = string.Empty;
				fileSavePath = string.Empty;
				PicBAnalysisImage.Image = null;
				PicBAnalysisImage.Invalidate();
			}
			if (fileSavePathcookingpic != null && fileSavePathcookingpic != string.Empty && ofdcookingpic != null && ofdcookingpic != string.Empty)
			{
				if (!Directory.Exists(clsGlobalVariables.AttachmentFolderPath))
				{
					throw new Exception("Attachment Folder Path Not Configure Properly");
				}
				fileSavePathcookingpic = System.IO.Path.Combine(clsGlobalVariables.AttachmentFolderPath, fileSavePathcookingpic);
				File.Copy(ofdcookingpic, fileSavePathcookingpic, overwrite: true);
				ofdcookingpic = string.Empty;
				fileSavePathcookingpic = string.Empty;
				PicBCookingImage.Image = null;
				PicBCookingImage.Invalidate();
			}
			refresh();
			if (IsPrint.Checked)
			{
				Print657(success);
			}
		}
		catch (Exception ex2)
		{
			MessageBox.Show(ex2.Message);
		}
	}

	private void Save_Click(object sender, EventArgs e)
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

	private void Update_Click(object sender, EventArgs e)
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

	private void ReadById(int Id)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		try
		{
			if (grdhistory.CurrentRow.RowIndex == -1)
			{
				return;
			}
			BtnSave.Visible = false;
			BtnUpdate.Visible = true;
			UpdateMode = true;
			RecId = Id;
			InvLabSampleAnalysisHeader header = new InvLabSampleAnalysisHeader();
			header = InvLabSampleAnalysisHeader.GetByID(RecId);
			if (header.LabSampleDetail.Count <= 0)
			{
				return;
			}
			detaillst.Clear();
			tabControl1.SelectedIndex = 0;
			txtdocno.Text = Conversion.ToString((object)header.DocNo);
			txtdocdate.Value = Conversion.ToDateTime((object)header.DocDate);
			txtManaulReportNo.Text = Conversion.ToString((object)header.ReportNo);
			txtPartyLotRefNo.Text = Conversion.ToString((object)header.PartyLotRefNo);
			txtQty.Text = Conversion.ToString((object)header.NoOfBagsInspected);
			if (header.SupplierCustomerId > 0)
			{
				CmbSupplierName.Value = Conversion.ToInt((object)header.SupplierCustomerId);
			}
			if (header.CommissionAgentId > 0)
			{
				CmbCommissionAgent.Value = Conversion.ToInt((object)header.CommissionAgentId);
			}
			((Control)(object)CmbCropYear).Text = Conversion.ToString((object)header.Crop);
			CmbAnalysisType.Value = Conversion.ToInt((object)header.AnalysisTypeId);
			CmbAnalysisType_Leave(null, null);
			CmbAnalysisGroup.Value = Conversion.ToInt((object)header.InvLabAnalysisGroupId);
			ItemBind();
			CmbItemName.Value = Conversion.ToInt((object)header.ItemId);
			PurchaseOrderFill();
			CmbPurchaseOrder.Value = Conversion.ToInt((object)header.OrderId);
			CmbJobLot.Value = Conversion.ToInt((object)header.JobLotId);
			if (header.IsAccepted)
			{
				((Control)(object)CmbStatus).Text = "Accepted";
			}
			else
			{
				((Control)(object)CmbStatus).Text = "Rejected";
			}
			txtremarks.Text = Conversion.ToString((object)header.Remarks);
			detaillst = header.LabSampleDetail;
			BindGrid();
			FormHelper.LoadAttachmentsForObject(AT, RecId, base.Name);
			fileSavePath = header.AnalysisPic;
			fileSavePathcookingpic = header.CookingPic;
			if (fileSavePath != null && fileSavePath != "")
			{
				string DrvPicPath2 = Path.Combine(clsGlobalVariables.AttachmentFolderPath, fileSavePath);
				if (File.Exists(DrvPicPath2))
				{
					Image image2 = Image.FromFile(DrvPicPath2);
					PicBAnalysisImage.Image = image2;
					PicBAnalysisImage.Tag = fileSavePath;
				}
				else
				{
					fileSavePath = "";
					PicBAnalysisImage.Image = null;
					PicBAnalysisImage.Invalidate();
				}
			}
			else
			{
				fileSavePath = "";
				PicBAnalysisImage.Image = null;
				PicBAnalysisImage.Invalidate();
			}
			if (fileSavePathcookingpic != null && fileSavePathcookingpic != "")
			{
				string DrvPicPath = Path.Combine(clsGlobalVariables.AttachmentFolderPath, fileSavePathcookingpic);
				if (File.Exists(DrvPicPath))
				{
					Image image = Image.FromFile(DrvPicPath);
					PicBCookingImage.Image = image;
					PicBCookingImage.Tag = fileSavePathcookingpic;
				}
				else
				{
					fileSavePathcookingpic = "";
					PicBCookingImage.Image = null;
					PicBCookingImage.Invalidate();
				}
			}
			else
			{
				fileSavePathcookingpic = "";
				PicBCookingImage.Image = null;
				PicBCookingImage.Invalidate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void New_Click(object sender, EventArgs e)
	{
		refresh();
	}

	private void refresh()
	{
		try
		{
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			txtdocdate.Focus();
			UpdateMode = false;
			BtnSave.Visible = true;
			BtnUpdate.Visible = false;
			((Control)(object)CmbItemName).Text = string.Empty;
			((Control)(object)CmbJobLot).Text = string.Empty;
			((Control)(object)CmbAnalysisGroup).Text = string.Empty;
			((Control)(object)CmbSupplierName).Text = string.Empty;
			((Control)(object)CmbCommissionAgent).Text = string.Empty;
			txtQty.Text = string.Empty;
			grdtable.Clear();
			((Control)(object)CmbCropYear).Text = string.Empty;
			txtdocno.Clear();
			((Control)(object)CmbPurchaseOrder).Text = string.Empty;
			((UltraGridBase)CmbPurchaseOrder).DataSource = null;
			txtremarks.Clear();
			txtManaulReportNo.Clear();
			txtPartyLotRefNo.Clear();
			detaillst.Clear();
			grdSubItems.ClearStructure();
			itemcode();
			grd.DataSource = null;
			PicBAnalysisImage.Image = null;
			PicBAnalysisImage.Invalidate();
			PicBAnalysisImage.Tag = "";
			PicBCookingImage.Image = null;
			PicBCookingImage.Invalidate();
			PicBCookingImage.Tag = "";
			GetConfigurationsFromGlobalAndBindValuesInColumns();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void toolStripButton2_Click(object sender, EventArgs e)
	{
		try
		{
			itemcode();
			ItemFill();
			cmbsupplierfill();
			CropYearFill();
			combojoblotfill();
			LabstatusFill();
			AnalysisTypeFill(InvLabAnalysisGroup.AnalysisType());
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void InvLabSampleAnalysis_FormClosing(object sender, FormClosingEventArgs e)
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

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			_ = tabControl1.SelectedIndex;
			_ = 1;
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
			((Control)(object)CmbSupplierCustomerHistory).Text = string.Empty;
			((Control)(object)CmbItemHistory).Text = string.Empty;
			((Control)(object)CmbCropYearHistory).Text = string.Empty;
			((Control)(object)CmbJobLotHistory).Text = string.Empty;
			((Control)(object)CmbStatusHistory).Text = string.Empty;
			((Control)(object)CmbAnalysisGroupHistory).Text = string.Empty;
			grdhistory.ClearStructure();
			grdHistoryDetail.ClearStructure();
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
			gridhistoryfill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridhistoryfill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		ReportsParameters header = new ReportsParameters();
		header.OrganizationId = UserAccount.OrganizationId;
		header.CompanyId = UserAccount.CompanyId;
		header.CanViewAllRecord = formrights.DoHaveCanViewAllRecordRights;
		if (!header.CanViewAllRecord)
		{
			header.EntryUser = UserAccount.ID;
		}
		if (FromDateHistory.Checked)
		{
			header.FromDate = FromDateHistory.Value;
		}
		if (ToDateHistory.Checked)
		{
			header.ToDate = ToDateHistory.Value;
		}
		header.FromDocNo = Conversion.ToInt((object)FromDocNoHistory.Text);
		header.ToDocNo = Conversion.ToInt((object)ToDocNoHistory.Text);
		header.SupplierCustomerId = Conversion.ToInt(CmbSupplierCustomerHistory.Value);
		header.ItemId = Conversion.ToInt(CmbItemHistory.Value);
		header.JobLotId = Conversion.ToInt(CmbJobLotHistory.Value);
		header.AnalysisGroupId = Conversion.ToInt(CmbAnalysisGroupHistory.Value);
		header.CropYear = Conversion.ToString((object)((Control)(object)CmbCropYearHistory).Text);
		header.Status = ((Control)(object)CmbStatusHistory).Text;
		DataTable dtGetAll = InvLabSampleAnalysisHeader.GetAll(header);
		if (dtGetAll.Rows.Count > 0)
		{
			tablehistory.Columns.Clear();
			tablehistory.Columns.Add("Id");
			tablehistory.Columns.Add("DocNo", typeof(int));
			tablehistory.Columns.Add("DocDate", typeof(DateTime));
			tablehistory.Columns.Add("ReportNo", typeof(string));
			tablehistory.Columns.Add("PartyLotNo", typeof(string));
			tablehistory.Columns.Add("NoOfBags", typeof(double));
			tablehistory.Columns.Add("PartyName");
			tablehistory.Columns.Add("CommissionAgent");
			tablehistory.Columns.Add("ItemName");
			tablehistory.Columns.Add("CropYear");
			tablehistory.Columns.Add("JobLot");
			tablehistory.Columns.Add("Status");
			tablehistory.Columns.Add("AnalysisGroup");
			tablehistory.Columns.Add("EntryDate", typeof(DateTime));
			tablehistory.Columns.Add("EntryUser");
			tablehistory.Columns.Add("ModifyDate", typeof(DateTime));
			tablehistory.Columns.Add("ModifyUser");
			tablehistory.Columns.Add("AnalysisPic", typeof(int));
			tablehistory.Columns.Add("CookingPic", typeof(int));
			tablehistory.Columns.Add("AnalysisPicValue");
			tablehistory.Columns.Add("CookingPicValue");
			tablehistory.Columns.Add("NoOfAttachments", typeof(int));
			tablehistory.Clear();
			for (int i = 0; i < dtGetAll.Rows.Count; i++)
			{
				tablehistory.Rows.Add(dtGetAll.Rows[i]["Id"], dtGetAll.Rows[i]["DocNo"], Conversion.ToDateTime(dtGetAll.Rows[i]["DocDate"]).ToString("dd-MMM-yyyy"), dtGetAll.Rows[i]["ReportNo"], dtGetAll.Rows[i]["PartyLotRefNo"], Conversion.ToDouble(dtGetAll.Rows[i]["NoOfBagsInspected"]), dtGetAll.Rows[i]["CompanyName"], dtGetAll.Rows[i]["CommissionAgent"], dtGetAll.Rows[i]["ItemName"], dtGetAll.Rows[i]["Crop"], dtGetAll.Rows[i]["JobLotDescription"], dtGetAll.Rows[i]["Status"], dtGetAll.Rows[i]["AnalysisGroupDescription"], dtGetAll.Rows[i]["EntryDate"], dtGetAll.Rows[i]["UserName"], dtGetAll.Rows[i]["ModifyDate"], dtGetAll.Rows[i]["ModifyUserName"], (!string.IsNullOrEmpty(Conversion.ToString(dtGetAll.Rows[i]["AnalysisPic"]))) ? 1 : 0, (!string.IsNullOrEmpty(Conversion.ToString(dtGetAll.Rows[i]["CookingPic"]))) ? 1 : 0, dtGetAll.Rows[i]["AnalysisPic"], dtGetAll.Rows[i]["CookingPic"], dtGetAll.Rows[i]["NoOfAttachments"]);
			}
			grdhistory.DataSource = tablehistory;
			grdhistory.RetrieveStructure();
			grdhistorySetting();
		}
		else
		{
			grdhistory.DataSource = null;
		}
	}

	private void grdhistorySetting()
	{
		try
		{
			grdhistory.RootTable.Columns["Id"].Visible = false;
			grdhistory.RootTable.Columns["AnalysisPicValue"].Visible = false;
			grdhistory.RootTable.Columns["CookingPicValue"].Visible = false;
			GridEX_Helper.GridWrappingAndColumnSettings(grdhistory, 2, 3, null, null, null, ColumnSetting: false);
			grdhistory.RootTable.Columns["DocNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grdhistory.RootTable.Columns["DocDate"].Width = Constants.InventoryConstants.DateConstant;
			grdhistory.RootTable.Columns["ReportNo"].Width = Constants.InventoryConstants.BiltyNoConstant;
			grdhistory.RootTable.Columns["PartyLotNo"].Width = 100;
			grdhistory.RootTable.Columns["NoOfBags"].Width = Constants.InventoryConstants.QtyConstant;
			grdhistory.RootTable.Columns["PartyName"].Width = Constants.InventoryConstants.SupplierName + 20;
			grdhistory.RootTable.Columns["CommissionAgent"].Width = Constants.InventoryConstants.SupplierName + 20;
			grdhistory.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName + 20;
			grdhistory.RootTable.Columns["CropYear"].Width = Constants.InventoryConstants.CropYearConstant;
			grdhistory.RootTable.Columns["JobLot"].Width = Constants.InventoryConstants.JobLotConstant;
			grdhistory.RootTable.Columns["Status"].Width = Constants.InventoryConstants.StatusConstant;
			grdhistory.RootTable.Columns["AnalysisGroup"].Width = 110;
			grdhistory.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			grdhistory.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants + 20;
			grdhistory.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			grdhistory.RootTable.Columns["ModifyUser"].Width = Constants.InventoryConstants.ModifyUserConstants + 20;
			grdhistory.RootTable.Columns["NoOfAttachments"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants - 10;
			grdhistory.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			grdhistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			grdhistory.RootTable.Columns["NoOfBags"].AggregateFunction = (AggregateFunction)2;
			grdhistory.RootTable.Columns["NoOfBags"].TextAlignment = (TextAlignment)3;
			grdhistory.RootTable.Columns["NoOfBags"].FormatString = "#,##0.###";
			grdhistory.RootTable.Columns["NoOfBags"].TotalFormatString = "#,##0.###";
			GridEX_Helper.AddButton(grdhistory, "Print", "Print", 40, 0);
			GridEX_Helper.AddButton(grdhistory, "Edit", "Edit", 40, 1);
			grdhistory.FrozenColumns = 2;
			GridEX_Helper.AddButton(grdhistory, "AddAttachment", "Add Attachment", 105);
			grdhistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdhistory.RootTable.Columns["AnalysisPic"].ColumnType = (ColumnType)5;
			grdhistory.RootTable.Columns["CookingPic"].ColumnType = (ColumnType)5;
			grdhistory.RootTable.Columns["AnalysisPic"].Width = 60;
			grdhistory.RootTable.Columns["CookingPic"].Width = 60;
			ctrlGrdBar1_Load(null, null);
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
					Print657(Id);
				}
				else if (e.Column.Key == "AddAttachment")
				{
					AttachmentAddingFromHistory attachmentAddingFromHistory = new AttachmentAddingFromHistory(UserAccount, Id, 302, base.Name);
					attachmentAddingFromHistory.BringToFront();
					attachmentAddingFromHistory.ShowDialog();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdhistory.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				int Id = Conversion.ToInt(r.Cells["Id"].Value);
				RecId = Id;
				ReadById(RecId);
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
				int Id = Conversion.ToInt(item.Cells["Id"].Value);
				if (e.Column.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, 302);
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

	private void grdhistory_SelectionChanged(object sender, EventArgs e)
	{
		try
		{
			if (grdhistory.CurrentRow == null || grdhistory.CurrentRow.Cells.Count <= 0)
			{
				return;
			}
			InvLabSampleAnalysisHeader objSam = InvLabSampleAnalysisHeader.GetByID(Conversion.ToInt(grdhistory.CurrentRow.Cells["Id"].Value));
			DataTable dtSamHistory = new DataTable();
			dtSamHistory.Columns.Add("Id");
			dtSamHistory.Columns.Add("AnalysisGroup");
			dtSamHistory.Columns.Add("AnalysisParameter");
			dtSamHistory.Columns.Add("MinValue");
			dtSamHistory.Columns.Add("MaxValue");
			dtSamHistory.Columns.Add("ResultValue", typeof(double));
			dtSamHistory.Columns.Add("RemarksDetail");
			dtSamHistory.Rows.Clear();
			grdHistoryDetail.ClearStructure();
			if (objSam != null)
			{
				for (int i = 0; i < objSam.LabSampleDetail.Count; i++)
				{
					dtSamHistory.Rows.Add(objSam.LabSampleDetail[i].Id, objSam.LabSampleDetail[i].AnalysisGroupDescription, objSam.LabSampleDetail[i].AnalysisParameterDescription, objSam.LabSampleDetail[i].MinValue, objSam.LabSampleDetail[i].MaxValue, objSam.LabSampleDetail[i].ResultValue, objSam.LabSampleDetail[i].RemarksDetail);
				}
				grdHistoryDetail.DataSource = dtSamHistory;
				grdHistoryDetail.RetrieveStructure();
				GrdHistoryDetailSetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdHistoryDetailSetting()
	{
		try
		{
			grdHistoryDetail.RootTable.Columns["Id"].Visible = false;
			grdHistoryDetail.RootTable.Columns["AnalysisGroup"].Width = 125;
			grdHistoryDetail.RootTable.Columns["AnalysisParameter"].Width = 125;
			grdHistoryDetail.RootTable.Columns["MinValue"].Width = 70;
			grdHistoryDetail.RootTable.Columns["MaxValue"].Width = 70;
			grdHistoryDetail.RootTable.Columns["ResultValue"].Width = 90;
			grdHistoryDetail.RootTable.Columns["ResultValue"].AggregateFunction = (AggregateFunction)2;
			grdHistoryDetail.RootTable.Columns["ResultValue"].FormatString = "#,#.##";
			grdHistoryDetail.RootTable.Columns["ResultValue"].TotalFormatString = "#,#.##";
			grdHistoryDetail.RootTable.Columns["ResultValue"].TextAlignment = (TextAlignment)3;
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
				if (PicBAnalysisImage.Image != null)
				{
					PicBAnalysisImage.Image.Dispose();
					PicBAnalysisImage.Image = null;
				}
				using (Bitmap bmp = new Bitmap(openFileDialog.FileName))
				{
					PicBAnalysisImage.Image = new Bitmap(bmp);
				}
				PicBAnalysisImage.SizeMode = PictureBoxSizeMode.Zoom;
			}
			else if (PicBAnalysisImage.Image != null)
			{
				PicBAnalysisImage.Image.Dispose();
				PicBAnalysisImage.Image = null;
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
				if (PicBCookingImage.Image != null)
				{
					PicBCookingImage.Image.Dispose();
					PicBCookingImage.Image = null;
				}
				using (Bitmap bmp = new Bitmap(openFileDialog.FileName))
				{
					PicBCookingImage.Image = new Bitmap(bmp);
				}
				PicBCookingImage.SizeMode = PictureBoxSizeMode.Zoom;
			}
			else if (PicBCookingImage.Image != null)
			{
				PicBCookingImage.Image.Dispose();
				PicBCookingImage.Image = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnClearAnalysisPic_Click(object sender, EventArgs e)
	{
		try
		{
			fileSavePath = string.Empty;
			ofd = string.Empty;
			if (PicBAnalysisImage.Image != null)
			{
				PicBAnalysisImage.Image.Dispose();
				PicBAnalysisImage.Image = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnClearCookingPic_Click(object sender, EventArgs e)
	{
		try
		{
			fileSavePathcookingpic = string.Empty;
			ofdcookingpic = string.Empty;
			if (PicBCookingImage.Image != null)
			{
				PicBCookingImage.Image.Dispose();
				PicBCookingImage.Image = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnViewAnalysisPic_Click(object sender, EventArgs e)
	{
		try
		{
			if (PicBAnalysisImage.Image != null)
			{
				LabImagesPreview labImagesPreview = new LabImagesPreview();
				labImagesPreview.ImageToShow = PicBAnalysisImage.Image;
				labImagesPreview.Show();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnViewCookingPic_Click(object sender, EventArgs e)
	{
		try
		{
			if (PicBCookingImage.Image != null)
			{
				LabImagesPreview labImagesPreview = new LabImagesPreview();
				labImagesPreview.ImageToShow = PicBCookingImage.Image;
				labImagesPreview.Show();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void toolStripButton10_Click(object sender, EventArgs e)
	{
		AT.Show();
	}

	private void Print_Click(object sender, EventArgs e)
	{
		try
		{
			Print657(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Print657(int RecordId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Expected O, but got Unknown
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.Id = RecordId;
			DataTable dt = new DataTable();
			dt = InvLabSampleAnalysisHeader.GetPrintSlipAndReport(obj);
			if (dt.Rows.Count == 0 || dt == null)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(InvLabSampleAnalysisHeader.GetDataForLabSampleAnalysisSubReport(obj.Id), "LabSampleAnalysisSubParameterReport.rpt");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "657-RptInvLabSampleAnalysisSlipA.rpt", lstSubReports);
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
			MessageBox.Show(ex.ToString());
		}
	}

	private void InvLabSampleAnalysis_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.S && !UpdateMode)
			{
				Save_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				New_Click(null, null);
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.U && UpdateMode)
			{
				Update_Click(null, null);
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
		//IL_0531: Unknown result type (might be due to invalid IL or missing references)
		//IL_0538: Expected O, but got Unknown
		//IL_0538: Unknown result type (might be due to invalid IL or missing references)
		//IL_053f: Expected O, but got Unknown
		//IL_053f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0546: Expected O, but got Unknown
		//IL_0546: Unknown result type (might be due to invalid IL or missing references)
		//IL_054d: Expected O, but got Unknown
		//IL_054d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0554: Expected O, but got Unknown
		//IL_0554: Unknown result type (might be due to invalid IL or missing references)
		//IL_055b: Expected O, but got Unknown
		//IL_055b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0562: Expected O, but got Unknown
		//IL_0562: Unknown result type (might be due to invalid IL or missing references)
		//IL_0569: Expected O, but got Unknown
		//IL_05a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ab: Expected O, but got Unknown
		//IL_05d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e2: Expected O, but got Unknown
		//IL_0604: Unknown result type (might be due to invalid IL or missing references)
		//IL_060e: Expected O, but got Unknown
		//IL_06a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b3: Expected O, but got Unknown
		//IL_06bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c9: Expected O, but got Unknown
		//IL_06d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06df: Expected O, but got Unknown
		//IL_06eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f5: Expected O, but got Unknown
		//IL_06f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0700: Expected O, but got Unknown
		//IL_0701: Unknown result type (might be due to invalid IL or missing references)
		//IL_070b: Expected O, but got Unknown
		//IL_070c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0716: Expected O, but got Unknown
		//IL_0717: Unknown result type (might be due to invalid IL or missing references)
		//IL_0721: Expected O, but got Unknown
		//IL_0722: Unknown result type (might be due to invalid IL or missing references)
		//IL_072c: Expected O, but got Unknown
		//IL_084b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0855: Expected O, but got Unknown
		//IL_086c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0876: Expected O, but got Unknown
		//IL_088d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0897: Expected O, but got Unknown
		//IL_08a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ad: Expected O, but got Unknown
		//IL_08b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c3: Expected O, but got Unknown
		//IL_08da: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e4: Expected O, but got Unknown
		//IL_08f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_08fa: Expected O, but got Unknown
		//IL_0911: Unknown result type (might be due to invalid IL or missing references)
		//IL_091b: Expected O, but got Unknown
		//IL_1287: Unknown result type (might be due to invalid IL or missing references)
		//IL_1291: Expected O, but got Unknown
		//IL_129e: Unknown result type (might be due to invalid IL or missing references)
		//IL_12a8: Expected O, but got Unknown
		//IL_5b81: Unknown result type (might be due to invalid IL or missing references)
		//IL_5b8b: Expected O, but got Unknown
		//IL_5baf: Unknown result type (might be due to invalid IL or missing references)
		//IL_5bb9: Expected O, but got Unknown
		GridEXLayout grdSubItems_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance137 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance190 = new Appearance();
		Appearance appearance191 = new Appearance();
		Appearance appearance192 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance171 = new Appearance();
		Appearance appearance182 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
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
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance136 = new Appearance();
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
		Appearance appearance151 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance157 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance163 = new Appearance();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Lab.InvLabSampleAnalysis));
		GridEXLayout grdHistoryDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdhistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance177 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance180 = new Appearance();
		Appearance appearance181 = new Appearance();
		Appearance appearance183 = new Appearance();
		Appearance appearance184 = new Appearance();
		Appearance appearance185 = new Appearance();
		Appearance appearance186 = new Appearance();
		Appearance appearance187 = new Appearance();
		Appearance appearance188 = new Appearance();
		Appearance appearance189 = new Appearance();
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
		Appearance appearance164 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance168 = new Appearance();
		Appearance appearance169 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance172 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance176 = new Appearance();
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
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.Form = new System.Windows.Forms.TabPage();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.panel12 = new System.Windows.Forms.Panel();
		this.grdSubItems = new GridEX();
		this.panel13 = new System.Windows.Forms.Panel();
		this.label18 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel11 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.panel10 = new System.Windows.Forms.Panel();
		this.label14 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.CmbCommissionAgent = new UltraCombo();
		this.label25 = new System.Windows.Forms.Label();
		this.btnViewCookingPic = new System.Windows.Forms.Button();
		this.BtnViewAnalysisPic = new System.Windows.Forms.Button();
		this.btnClearCookingPic = new System.Windows.Forms.Button();
		this.btnClearAnalysisPic = new System.Windows.Forms.Button();
		this.panel14 = new System.Windows.Forms.Panel();
		this.label13 = new System.Windows.Forms.Label();
		this.IsPrint = new System.Windows.Forms.CheckBox();
		this.label17 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.txtQty = new System.Windows.Forms.TextBox();
		this.label12 = new System.Windows.Forms.Label();
		this.txtPartyLotRefNo = new System.Windows.Forms.TextBox();
		this.label7 = new System.Windows.Forms.Label();
		this.CmbStatus = new UltraCombo();
		this.label5 = new System.Windows.Forms.Label();
		this.CmbSampleLogNo = new UltraCombo();
		this.label4 = new System.Windows.Forms.Label();
		this.CmbPurchaseOrder = new UltraCombo();
		this.label3 = new System.Windows.Forms.Label();
		this.CmbCropYear = new UltraCombo();
		this.CmbJobLot = new UltraCombo();
		this.CmbAnalysisType = new UltraCombo();
		this.CmbAnalysisGroup = new UltraCombo();
		this.CmbItemName = new UltraCombo();
		this.CmbSupplierName = new UltraCombo();
		this.BtnBrowseCookingPic = new System.Windows.Forms.Button();
		this.BtnBrowseAnaylsisPic = new System.Windows.Forms.Button();
		this.PicBCookingImage = new System.Windows.Forms.PictureBox();
		this.label26 = new System.Windows.Forms.Label();
		this.PicBAnalysisImage = new System.Windows.Forms.PictureBox();
		this.label6 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.txtdocdate = new System.Windows.Forms.DateTimePicker();
		this.txtremarks = new System.Windows.Forms.TextBox();
		this.label11 = new System.Windows.Forms.Label();
		this.txtManaulReportNo = new System.Windows.Forms.TextBox();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.label10 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label42 = new System.Windows.Forms.Label();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.BtnNew = new System.Windows.Forms.ToolStripButton();
		this.BtnRefresh = new System.Windows.Forms.ToolStripButton();
		this.BtnUpdate = new System.Windows.Forms.ToolStripButton();
		this.BtnSave = new System.Windows.Forms.ToolStripButton();
		this.BtnPrint = new System.Windows.Forms.ToolStripButton();
		this.BtnAttachments = new System.Windows.Forms.ToolStripButton();
		this.History = new System.Windows.Forms.TabPage();
		this.panel8 = new System.Windows.Forms.Panel();
		this.grdHistoryDetail = new GridEX();
		this.panel9 = new System.Windows.Forms.Panel();
		this.panel7 = new System.Windows.Forms.Panel();
		this.grdhistory = new GridEX();
		this.panel20 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.CmbItemHistory = new UltraCombo();
		this.label22 = new System.Windows.Forms.Label();
		this.CmbStatusHistory = new UltraCombo();
		this.label21 = new System.Windows.Forms.Label();
		this.CmbJobLotHistory = new UltraCombo();
		this.label19 = new System.Windows.Forms.Label();
		this.btnShowHistory = new System.Windows.Forms.Button();
		this.CmbCropYearHistory = new UltraCombo();
		this.label20 = new System.Windows.Forms.Label();
		this.CmbSupplierCustomerHistory = new UltraCombo();
		this.label24 = new System.Windows.Forms.Label();
		this.label23 = new System.Windows.Forms.Label();
		this.CmbAnalysisGroupHistory = new UltraCombo();
		this.label56 = new System.Windows.Forms.Label();
		this.label57 = new System.Windows.Forms.Label();
		this.ToDocNoHistory = new System.Windows.Forms.TextBox();
		this.FromDocNoHistory = new System.Windows.Forms.TextBox();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.panel3 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label15 = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
		this.tabControl1.SuspendLayout();
		this.Form.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel12.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdSubItems).BeginInit();
		this.panel13.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel11.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel10.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCommissionAgent).BeginInit();
		this.panel14.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbStatus).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSampleLogNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPurchaseOrder).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYear).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAnalysisType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAnalysisGroup).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplierName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.PicBCookingImage).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.PicBAnalysisImage).BeginInit();
		this.toolStrip2.SuspendLayout();
		this.History.SuspendLayout();
		this.panel8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryDetail).BeginInit();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdhistory).BeginInit();
		this.panel20.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbItemHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbStatusHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLotHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYearHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplierCustomerHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAnalysisGroupHistory).BeginInit();
		this.panel3.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.panel1.SuspendLayout();
		base.SuspendLayout();
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.Form);
		this.tabControl1.Controls.Add(this.History);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(979, 676);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.Form.Controls.Add(this.panel2);
		this.Form.Controls.Add(this.panel5);
		this.Form.Controls.Add(this.toolStrip2);
		this.Form.Location = new System.Drawing.Point(4, 4);
		this.Form.Margin = new System.Windows.Forms.Padding(2);
		this.Form.Name = "Form";
		this.Form.Padding = new System.Windows.Forms.Padding(2);
		this.Form.Size = new System.Drawing.Size(971, 647);
		this.Form.TabIndex = 0;
		this.Form.Text = "Form";
		this.Form.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add(this.panel6);
		this.panel2.Controls.Add(this.panel4);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(2, 254);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(967, 391);
		this.panel2.TabIndex = 5;
		this.panel6.Controls.Add(this.panel12);
		this.panel6.Controls.Add(this.panel13);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel6.Location = new System.Drawing.Point(460, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(476, 391);
		this.panel6.TabIndex = 2;
		this.panel12.Controls.Add((System.Windows.Forms.Control)(object)this.grdSubItems);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel12.Location = new System.Drawing.Point(0, 25);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(476, 366);
		this.panel12.TabIndex = 4;
		this.grdSubItems.AllowDelete = (InheritableBoolean)1;
		((ScrollableBase)this.grdSubItems).BackColor = System.Drawing.Color.White;
		grdSubItems_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdSubItems.DesignTimeLayout = grdSubItems_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdSubItems).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdSubItems.FilterMode = (FilterMode)2;
		this.grdSubItems.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdSubItems).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdSubItems.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdSubItems).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdSubItems).Name = "grdSubItems";
		((ScrollableBase)this.grdSubItems).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdSubItems).Size = new System.Drawing.Size(476, 366);
		((System.Windows.Forms.Control)(object)this.grdSubItems).TabIndex = 0;
		this.grdSubItems.TotalRow = (InheritableBoolean)1;
		this.grdSubItems.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdSubItems).VisualStyle = (VisualStyle)4;
		this.panel13.BackColor = System.Drawing.Color.Teal;
		this.panel13.Controls.Add(this.label18);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel13.Location = new System.Drawing.Point(0, 0);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(476, 25);
		this.panel13.TabIndex = 3;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.label18.ForeColor = System.Drawing.Color.White;
		this.label18.Location = new System.Drawing.Point(3, 4);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(459, 14);
		this.label18.TabIndex = 54;
		this.label18.Text = "User Can Add Sub Parameters Values Of Selected Analysis Parameter Here";
		this.panel4.Controls.Add(this.panel11);
		this.panel4.Controls.Add(this.panel10);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel4.Location = new System.Drawing.Point(0, 0);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(460, 391);
		this.panel4.TabIndex = 1;
		this.panel11.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel11.Location = new System.Drawing.Point(0, 25);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(460, 366);
		this.panel11.TabIndex = 2;
		this.grd.AllowDelete = (InheritableBoolean)1;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(460, 366);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.CellUpdated += new ColumnActionEventHandler(grd_CellUpdated_1);
		this.grd.LinkClicked += new ColumnActionEventHandler(grd_LinkClicked);
		this.panel10.BackColor = System.Drawing.Color.Teal;
		this.panel10.Controls.Add(this.label14);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(0, 0);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(460, 25);
		this.panel10.TabIndex = 1;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.label14.ForeColor = System.Drawing.Color.White;
		this.label14.Location = new System.Drawing.Point(4, 4);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(286, 14);
		this.label14.TabIndex = 53;
		this.label14.Text = "User Will Add Analysis Parameter Values  Here";
		this.panel5.BackColor = System.Drawing.Color.White;
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCommissionAgent);
		this.panel5.Controls.Add(this.label25);
		this.panel5.Controls.Add(this.btnViewCookingPic);
		this.panel5.Controls.Add(this.BtnViewAnalysisPic);
		this.panel5.Controls.Add(this.btnClearCookingPic);
		this.panel5.Controls.Add(this.btnClearAnalysisPic);
		this.panel5.Controls.Add(this.panel14);
		this.panel5.Controls.Add(this.txtQty);
		this.panel5.Controls.Add(this.label12);
		this.panel5.Controls.Add(this.txtPartyLotRefNo);
		this.panel5.Controls.Add(this.label7);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbStatus);
		this.panel5.Controls.Add(this.label5);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSampleLogNo);
		this.panel5.Controls.Add(this.label4);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPurchaseOrder);
		this.panel5.Controls.Add(this.label3);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCropYear);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobLot);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbAnalysisType);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbAnalysisGroup);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemName);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSupplierName);
		this.panel5.Controls.Add(this.BtnBrowseCookingPic);
		this.panel5.Controls.Add(this.BtnBrowseAnaylsisPic);
		this.panel5.Controls.Add(this.PicBCookingImage);
		this.panel5.Controls.Add(this.label26);
		this.panel5.Controls.Add(this.PicBAnalysisImage);
		this.panel5.Controls.Add(this.label6);
		this.panel5.Controls.Add(this.label8);
		this.panel5.Controls.Add(this.label1);
		this.panel5.Controls.Add(this.txtdocdate);
		this.panel5.Controls.Add(this.txtremarks);
		this.panel5.Controls.Add(this.label11);
		this.panel5.Controls.Add(this.txtManaulReportNo);
		this.panel5.Controls.Add(this.txtdocno);
		this.panel5.Controls.Add(this.label10);
		this.panel5.Controls.Add(this.label9);
		this.panel5.Controls.Add(this.label2);
		this.panel5.Controls.Add(this.label42);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(2, 29);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(967, 225);
		this.panel5.TabIndex = 0;
		this.CmbCommissionAgent.AllowNull = (DefaultableBoolean)1;
		this.CmbCommissionAgent.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCommissionAgent.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCommissionAgent.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance137).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Appearance = (AppearanceBase)(object)appearance137;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance139).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance139).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance139).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance139;
		((AppearanceBase)appearance140).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance140;
		((SpecialBoxBase)((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance141).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance141).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance141).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance141;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance142).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance142;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance143).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance144;
		((AppearanceBase)appearance145).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance145).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance146).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance146).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance146).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance146;
		((AppearanceBase)appearance147).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance147;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance148).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance148;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance150;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCommissionAgent).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCommissionAgent).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCommissionAgent.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCommissionAgent).Location = new System.Drawing.Point(116, 143);
		((System.Windows.Forms.Control)(object)this.CmbCommissionAgent).Name = "CmbCommissionAgent";
		((System.Windows.Forms.Control)(object)this.CmbCommissionAgent).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.CmbCommissionAgent).TabIndex = 7;
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(8, 149);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(108, 15);
		this.label25.TabIndex = 57;
		this.label25.Text = "Commission Agent";
		this.btnViewCookingPic.BackColor = System.Drawing.Color.Teal;
		this.btnViewCookingPic.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnViewCookingPic.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnViewCookingPic.ForeColor = System.Drawing.Color.White;
		this.btnViewCookingPic.Location = new System.Drawing.Point(851, 171);
		this.btnViewCookingPic.Name = "btnViewCookingPic";
		this.btnViewCookingPic.Size = new System.Drawing.Size(51, 27);
		this.btnViewCookingPic.TabIndex = 20;
		this.btnViewCookingPic.Text = "View";
		this.btnViewCookingPic.UseVisualStyleBackColor = false;
		this.btnViewCookingPic.Click += new System.EventHandler(btnViewCookingPic_Click);
		this.BtnViewAnalysisPic.BackColor = System.Drawing.Color.Teal;
		this.BtnViewAnalysisPic.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnViewAnalysisPic.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnViewAnalysisPic.ForeColor = System.Drawing.Color.White;
		this.BtnViewAnalysisPic.Location = new System.Drawing.Point(702, 171);
		this.BtnViewAnalysisPic.Name = "BtnViewAnalysisPic";
		this.BtnViewAnalysisPic.Size = new System.Drawing.Size(51, 27);
		this.BtnViewAnalysisPic.TabIndex = 17;
		this.BtnViewAnalysisPic.Text = "View";
		this.BtnViewAnalysisPic.UseVisualStyleBackColor = false;
		this.BtnViewAnalysisPic.Click += new System.EventHandler(btnViewAnalysisPic_Click);
		this.btnClearCookingPic.BackColor = System.Drawing.Color.Teal;
		this.btnClearCookingPic.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnClearCookingPic.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnClearCookingPic.ForeColor = System.Drawing.Color.White;
		this.btnClearCookingPic.Location = new System.Drawing.Point(902, 171);
		this.btnClearCookingPic.Name = "btnClearCookingPic";
		this.btnClearCookingPic.Size = new System.Drawing.Size(25, 27);
		this.btnClearCookingPic.TabIndex = 21;
		this.btnClearCookingPic.Text = "X";
		this.btnClearCookingPic.UseVisualStyleBackColor = false;
		this.btnClearCookingPic.Click += new System.EventHandler(btnClearCookingPic_Click);
		this.btnClearAnalysisPic.BackColor = System.Drawing.Color.Teal;
		this.btnClearAnalysisPic.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnClearAnalysisPic.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnClearAnalysisPic.ForeColor = System.Drawing.Color.White;
		this.btnClearAnalysisPic.Location = new System.Drawing.Point(753, 171);
		this.btnClearAnalysisPic.Name = "btnClearAnalysisPic";
		this.btnClearAnalysisPic.Size = new System.Drawing.Size(26, 27);
		this.btnClearAnalysisPic.TabIndex = 18;
		this.btnClearAnalysisPic.Text = "X";
		this.btnClearAnalysisPic.UseVisualStyleBackColor = false;
		this.btnClearAnalysisPic.Click += new System.EventHandler(btnClearAnalysisPic_Click);
		this.panel14.BackColor = System.Drawing.Color.Teal;
		this.panel14.Controls.Add(this.label13);
		this.panel14.Controls.Add(this.IsPrint);
		this.panel14.Controls.Add(this.label17);
		this.panel14.Controls.Add(this.label16);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel14.Location = new System.Drawing.Point(0, 0);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(967, 30);
		this.panel14.TabIndex = 51;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.ForeColor = System.Drawing.Color.White;
		this.label13.Location = new System.Drawing.Point(3, 5);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(141, 19);
		this.label13.TabIndex = 52;
		this.label13.Text = "Sample Analysis";
		this.IsPrint.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.IsPrint.AutoSize = true;
		this.IsPrint.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.IsPrint.ForeColor = System.Drawing.Color.White;
		this.IsPrint.Location = new System.Drawing.Point(896, 6);
		this.IsPrint.Name = "IsPrint";
		this.IsPrint.Size = new System.Drawing.Size(72, 19);
		this.IsPrint.TabIndex = 16;
		this.IsPrint.Text = "Preview";
		this.IsPrint.UseVisualStyleBackColor = true;
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Times New Roman", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.ForeColor = System.Drawing.Color.White;
		this.label17.Location = new System.Drawing.Point(660, 8);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(74, 15);
		this.label17.TabIndex = 38;
		this.label17.Text = "Analysis Pic";
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Times New Roman", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.ForeColor = System.Drawing.Color.White;
		this.label16.Location = new System.Drawing.Point(804, 8);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(71, 15);
		this.label16.TabIndex = 39;
		this.label16.Text = "Cooking Pic";
		this.txtQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtQty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtQty.Location = new System.Drawing.Point(261, 89);
		this.txtQty.Name = "txtQty";
		this.txtQty.Size = new System.Drawing.Size(75, 23);
		this.txtQty.TabIndex = 5;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(191, 93);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(72, 15);
		this.label12.TabIndex = 50;
		this.label12.Text = "Analysis Qty";
		this.txtPartyLotRefNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPartyLotRefNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPartyLotRefNo.Location = new System.Drawing.Point(261, 61);
		this.txtPartyLotRefNo.Name = "txtPartyLotRefNo";
		this.txtPartyLotRefNo.Size = new System.Drawing.Size(75, 23);
		this.txtPartyLotRefNo.TabIndex = 3;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(191, 65);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(61, 15);
		this.label7.TabIndex = 48;
		this.label7.Text = "Party Lot#";
		this.CmbStatus.AllowNull = (DefaultableBoolean)1;
		this.CmbStatus.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbStatus.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbStatus.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance190).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance190).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Appearance = (AppearanceBase)(object)appearance190;
		((UltraGridBase)this.CmbStatus).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbStatus).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance191).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance191).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance191).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance191).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbStatus).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance191;
		((AppearanceBase)appearance192).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbStatus).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance192;
		((SpecialBoxBase)((UltraGridBase)this.CmbStatus).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbStatus).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbStatus).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbStatus).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbStatus).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbStatus).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbStatus).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbStatus).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbStatus.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbStatus).Location = new System.Drawing.Point(422, 171);
		((System.Windows.Forms.Control)(object)this.CmbStatus).Name = "CmbStatus";
		((System.Windows.Forms.Control)(object)this.CmbStatus).Size = new System.Drawing.Size(206, 26);
		((System.Windows.Forms.Control)(object)this.CmbStatus).TabIndex = 15;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(339, 177);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(40, 15);
		this.label5.TabIndex = 47;
		this.label5.Text = "Status";
		this.CmbSampleLogNo.AllowNull = (DefaultableBoolean)1;
		this.CmbSampleLogNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSampleLogNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSampleLogNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSampleLogNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbSampleLogNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSampleLogNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSampleLogNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSampleLogNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance57;
		((SpecialBoxBase)((UltraGridBase)this.CmbSampleLogNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSampleLogNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.CmbSampleLogNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSampleLogNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSampleLogNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSampleLogNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.CmbSampleLogNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSampleLogNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSampleLogNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance101;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance106).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSampleLogNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.CmbSampleLogNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSampleLogNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance107).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance107).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance107).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance107).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSampleLogNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance107;
		((AppearanceBase)appearance108).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSampleLogNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.CmbSampleLogNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSampleLogNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance109).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSampleLogNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.CmbSampleLogNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSampleLogNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.CmbSampleLogNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSampleLogNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSampleLogNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSampleLogNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSampleLogNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSampleLogNo).Location = new System.Drawing.Point(116, 87);
		((System.Windows.Forms.Control)(object)this.CmbSampleLogNo).Name = "CmbSampleLogNo";
		((System.Windows.Forms.Control)(object)this.CmbSampleLogNo).Size = new System.Drawing.Size(75, 26);
		((System.Windows.Forms.Control)(object)this.CmbSampleLogNo).TabIndex = 4;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(8, 93);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(89, 15);
		this.label4.TabIndex = 45;
		this.label4.Text = "Sample Log No";
		this.CmbPurchaseOrder.AllowNull = (DefaultableBoolean)1;
		this.CmbPurchaseOrder.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPurchaseOrder.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPurchaseOrder.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPurchaseOrder).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbPurchaseOrder).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPurchaseOrder).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance105).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance105).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPurchaseOrder).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance105;
		((AppearanceBase)appearance116).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPurchaseOrder).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance116;
		((SpecialBoxBase)((UltraGridBase)this.CmbPurchaseOrder).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance127).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance127).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance127).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPurchaseOrder).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.CmbPurchaseOrder).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPurchaseOrder).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance138).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPurchaseOrder).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance138;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance149).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPurchaseOrder).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance149;
		((UltraGridBase)this.CmbPurchaseOrder).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPurchaseOrder).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPurchaseOrder).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance160;
		((AppearanceBase)appearance171).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance171).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPurchaseOrder).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance171;
		((UltraGridBase)this.CmbPurchaseOrder).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPurchaseOrder).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance182).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance182).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance182).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance182).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance182).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPurchaseOrder).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance182;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPurchaseOrder).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbPurchaseOrder).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPurchaseOrder).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPurchaseOrder).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbPurchaseOrder).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPurchaseOrder).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbPurchaseOrder).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPurchaseOrder).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPurchaseOrder).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPurchaseOrder).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPurchaseOrder.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPurchaseOrder).Location = new System.Drawing.Point(116, 171);
		((System.Windows.Forms.Control)(object)this.CmbPurchaseOrder).Name = "CmbPurchaseOrder";
		((System.Windows.Forms.Control)(object)this.CmbPurchaseOrder).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.CmbPurchaseOrder).TabIndex = 8;
		((System.Windows.Forms.Control)(object)this.CmbPurchaseOrder).Leave += new System.EventHandler(cmbPurchaseOrder_Leave);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(8, 177);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(88, 15);
		this.label3.TabIndex = 42;
		this.label3.Text = "Purchase Order";
		this.CmbCropYear.AllowNull = (DefaultableBoolean)1;
		this.CmbCropYear.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCropYear.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCropYear.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance111).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Appearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance113).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance113;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance114).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance114).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance114).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance115).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance117).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance118;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance119).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance120).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance120).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance120).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance120).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance121).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance122).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCropYear.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Location = new System.Drawing.Point(422, 115);
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Name = "CmbCropYear";
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Size = new System.Drawing.Size(206, 26);
		((System.Windows.Forms.Control)(object)this.CmbCropYear).TabIndex = 13;
		this.CmbJobLot.AllowNull = (DefaultableBoolean)1;
		this.CmbJobLot.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobLot.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobLot.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbJobLot.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Location = new System.Drawing.Point(422, 143);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Name = "CmbJobLot";
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Size = new System.Drawing.Size(206, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).TabIndex = 14;
		this.CmbAnalysisType.AllowNull = (DefaultableBoolean)1;
		this.CmbAnalysisType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbAnalysisType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbAnalysisType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance124).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbAnalysisType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.CmbAnalysisType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbAnalysisType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance125).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance125).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance125).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbAnalysisType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance125;
		((AppearanceBase)appearance126).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAnalysisType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance126;
		((SpecialBoxBase)((UltraGridBase)this.CmbAnalysisType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance128).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance128).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance128).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAnalysisType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.CmbAnalysisType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbAnalysisType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance129).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbAnalysisType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance129;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance130).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbAnalysisType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.CmbAnalysisType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbAnalysisType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAnalysisType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance131;
		((AppearanceBase)appearance132).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance132).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbAnalysisType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.CmbAnalysisType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbAnalysisType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance133).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance133).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance133).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAnalysisType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance133;
		((AppearanceBase)appearance134).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbAnalysisType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance134;
		((UltraGridBase)this.CmbAnalysisType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbAnalysisType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance135).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbAnalysisType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.CmbAnalysisType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbAnalysisType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.CmbAnalysisType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbAnalysisType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbAnalysisType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbAnalysisType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbAnalysisType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbAnalysisType).Location = new System.Drawing.Point(422, 31);
		((System.Windows.Forms.Control)(object)this.CmbAnalysisType).Name = "CmbAnalysisType";
		((System.Windows.Forms.Control)(object)this.CmbAnalysisType).Size = new System.Drawing.Size(206, 26);
		((System.Windows.Forms.Control)(object)this.CmbAnalysisType).TabIndex = 10;
		((System.Windows.Forms.Control)(object)this.CmbAnalysisType).Leave += new System.EventHandler(CmbAnalysisType_Leave);
		this.CmbAnalysisGroup.AllowNull = (DefaultableBoolean)1;
		this.CmbAnalysisGroup.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbAnalysisGroup.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbAnalysisGroup.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.Appearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance93).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance93).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance94;
		((SpecialBoxBase)((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance95).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance95).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance96).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance97).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance98;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance99).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance100).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance102).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbAnalysisGroup).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbAnalysisGroup.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbAnalysisGroup).Location = new System.Drawing.Point(422, 59);
		((System.Windows.Forms.Control)(object)this.CmbAnalysisGroup).Name = "CmbAnalysisGroup";
		((System.Windows.Forms.Control)(object)this.CmbAnalysisGroup).Size = new System.Drawing.Size(206, 26);
		((System.Windows.Forms.Control)(object)this.CmbAnalysisGroup).TabIndex = 11;
		((System.Windows.Forms.Control)(object)this.CmbAnalysisGroup).Leave += new System.EventHandler(CmbAnalysisGroup_Leave);
		this.CmbItemName.AllowNull = (DefaultableBoolean)1;
		this.CmbItemName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance151).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.CmbItemName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance152).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance152).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance152).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance152;
		((AppearanceBase)appearance153).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance153;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance154).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance154).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance154).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance154;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance155).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance155;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance156).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance156;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance157;
		((AppearanceBase)appearance158).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance158).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance158;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance159).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance159).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance159).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance159).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance159;
		((AppearanceBase)appearance161).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance161;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance162).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance162;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance163;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Location = new System.Drawing.Point(422, 87);
		((System.Windows.Forms.Control)(object)this.CmbItemName).Name = "CmbItemName";
		((System.Windows.Forms.Control)(object)this.CmbItemName).Size = new System.Drawing.Size(206, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemName).TabIndex = 12;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Leave += new System.EventHandler(cmbitem_Leave);
		this.CmbSupplierName.AllowNull = (DefaultableBoolean)1;
		this.CmbSupplierName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSupplierName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSupplierName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance80).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance80).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplierName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance81;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplierName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance82).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance82).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance86).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance87).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance87).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance87).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance87;
		((AppearanceBase)appearance88).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSupplierName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSupplierName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSupplierName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSupplierName).Location = new System.Drawing.Point(116, 115);
		((System.Windows.Forms.Control)(object)this.CmbSupplierName).Name = "CmbSupplierName";
		((System.Windows.Forms.Control)(object)this.CmbSupplierName).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.CmbSupplierName).TabIndex = 6;
		((System.Windows.Forms.Control)(object)this.CmbSupplierName).Leave += new System.EventHandler(cmbsupplier_Leave);
		this.BtnBrowseCookingPic.BackColor = System.Drawing.Color.Teal;
		this.BtnBrowseCookingPic.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnBrowseCookingPic.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnBrowseCookingPic.ForeColor = System.Drawing.Color.White;
		this.BtnBrowseCookingPic.Location = new System.Drawing.Point(780, 171);
		this.BtnBrowseCookingPic.Name = "BtnBrowseCookingPic";
		this.BtnBrowseCookingPic.Size = new System.Drawing.Size(71, 27);
		this.BtnBrowseCookingPic.TabIndex = 19;
		this.BtnBrowseCookingPic.Text = "Browse";
		this.BtnBrowseCookingPic.UseVisualStyleBackColor = false;
		this.BtnBrowseCookingPic.Click += new System.EventHandler(btnimage2_Click);
		this.BtnBrowseAnaylsisPic.BackColor = System.Drawing.Color.Teal;
		this.BtnBrowseAnaylsisPic.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnBrowseAnaylsisPic.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnBrowseAnaylsisPic.ForeColor = System.Drawing.Color.White;
		this.BtnBrowseAnaylsisPic.Location = new System.Drawing.Point(629, 171);
		this.BtnBrowseAnaylsisPic.Name = "BtnBrowseAnaylsisPic";
		this.BtnBrowseAnaylsisPic.Size = new System.Drawing.Size(73, 27);
		this.BtnBrowseAnaylsisPic.TabIndex = 16;
		this.BtnBrowseAnaylsisPic.Text = "Browse";
		this.BtnBrowseAnaylsisPic.UseVisualStyleBackColor = false;
		this.BtnBrowseAnaylsisPic.Click += new System.EventHandler(btnimg1_Click);
		this.PicBCookingImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.PicBCookingImage.Location = new System.Drawing.Point(780, 34);
		this.PicBCookingImage.Name = "PicBCookingImage";
		this.PicBCookingImage.Size = new System.Drawing.Size(147, 135);
		this.PicBCookingImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.PicBCookingImage.TabIndex = 36;
		this.PicBCookingImage.TabStop = false;
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(339, 37);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(78, 15);
		this.label26.TabIndex = 32;
		this.label26.Text = "Analysis Type";
		this.PicBAnalysisImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.PicBAnalysisImage.Location = new System.Drawing.Point(630, 34);
		this.PicBAnalysisImage.Name = "PicBAnalysisImage";
		this.PicBAnalysisImage.Size = new System.Drawing.Size(147, 135);
		this.PicBAnalysisImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.PicBAnalysisImage.TabIndex = 37;
		this.PicBAnalysisImage.TabStop = false;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(339, 65);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(86, 15);
		this.label6.TabIndex = 32;
		this.label6.Text = "Analysis Group";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(339, 93);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(67, 15);
		this.label8.TabIndex = 32;
		this.label8.Text = "Item Name";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(8, 121);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(86, 15);
		this.label1.TabIndex = 32;
		this.label1.Text = "Supplier Name";
		this.txtdocdate.CustomFormat = "dd-MMM-yyyy";
		this.txtdocdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtdocdate.Location = new System.Drawing.Point(194, 33);
		this.txtdocdate.Name = "txtdocdate";
		this.txtdocdate.Size = new System.Drawing.Size(142, 23);
		this.txtdocdate.TabIndex = 1;
		this.txtdocdate.ValueChanged += new System.EventHandler(txtdocdate_ValueChanged);
		this.txtremarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtremarks.Location = new System.Drawing.Point(116, 199);
		this.txtremarks.Name = "txtremarks";
		this.txtremarks.Size = new System.Drawing.Size(811, 23);
		this.txtremarks.TabIndex = 9;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(8, 203);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(52, 15);
		this.label11.TabIndex = 22;
		this.label11.Text = "Remarks";
		this.txtManaulReportNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtManaulReportNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtManaulReportNo.Location = new System.Drawing.Point(116, 61);
		this.txtManaulReportNo.Name = "txtManaulReportNo";
		this.txtManaulReportNo.Size = new System.Drawing.Size(75, 23);
		this.txtManaulReportNo.TabIndex = 2;
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Enabled = false;
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocno.Location = new System.Drawing.Point(116, 33);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.Size = new System.Drawing.Size(75, 23);
		this.txtdocno.TabIndex = 0;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(339, 149);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(46, 15);
		this.label10.TabIndex = 23;
		this.label10.Text = "Job Lot";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(339, 121);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(57, 15);
		this.label9.TabIndex = 23;
		this.label9.Text = "Crop Year";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(8, 37);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(88, 15);
		this.label2.TabIndex = 22;
		this.label2.Text = "Doc No && Date";
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label42.Location = new System.Drawing.Point(8, 65);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(104, 15);
		this.label42.TabIndex = 23;
		this.label42.Text = "Manual Report No";
		this.toolStrip2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[6] { this.BtnNew, this.BtnRefresh, this.BtnUpdate, this.BtnSave, this.BtnPrint, this.BtnAttachments });
		this.toolStrip2.Location = new System.Drawing.Point(2, 2);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(967, 27);
		this.toolStrip2.TabIndex = 1;
		this.toolStrip2.Text = "toolStrip2";
		this.BtnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.BtnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnNew.Name = "BtnNew";
		this.BtnNew.Size = new System.Drawing.Size(59, 24);
		this.BtnNew.Text = "&New";
		this.BtnNew.Click += new System.EventHandler(New_Click);
		this.BtnRefresh.Image = (System.Drawing.Image)resources.GetObject("BtnRefresh.Image");
		this.BtnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefresh.Name = "BtnRefresh";
		this.BtnRefresh.Size = new System.Drawing.Size(80, 24);
		this.BtnRefresh.Text = "Refresh";
		this.BtnRefresh.Click += new System.EventHandler(toolStripButton2_Click);
		this.BtnUpdate.Image = (System.Drawing.Image)resources.GetObject("BtnUpdate.Image");
		this.BtnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnUpdate.Name = "BtnUpdate";
		this.BtnUpdate.Size = new System.Drawing.Size(78, 24);
		this.BtnUpdate.Text = "&Update";
		this.BtnUpdate.Visible = false;
		this.BtnUpdate.Click += new System.EventHandler(Update_Click);
		this.BtnSave.Image = (System.Drawing.Image)resources.GetObject("BtnSave.Image");
		this.BtnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnSave.Name = "BtnSave";
		this.BtnSave.Size = new System.Drawing.Size(64, 24);
		this.BtnSave.Text = "&Save";
		this.BtnSave.Click += new System.EventHandler(Save_Click);
		this.BtnPrint.Image = (System.Drawing.Image)resources.GetObject("BtnPrint.Image");
		this.BtnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnPrint.Name = "BtnPrint";
		this.BtnPrint.Size = new System.Drawing.Size(92, 24);
		this.BtnPrint.Text = "657-Print";
		this.BtnPrint.Click += new System.EventHandler(Print_Click);
		this.BtnAttachments.Image = (System.Drawing.Image)resources.GetObject("BtnAttachments.Image");
		this.BtnAttachments.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnAttachments.Name = "BtnAttachments";
		this.BtnAttachments.Size = new System.Drawing.Size(109, 24);
		this.BtnAttachments.Text = "&Attachment";
		this.BtnAttachments.Click += new System.EventHandler(toolStripButton10_Click);
		this.History.Controls.Add(this.panel8);
		this.History.Location = new System.Drawing.Point(4, 4);
		this.History.Margin = new System.Windows.Forms.Padding(2);
		this.History.Name = "History";
		this.History.Padding = new System.Windows.Forms.Padding(2);
		this.History.Size = new System.Drawing.Size(971, 647);
		this.History.TabIndex = 1;
		this.History.Text = "History";
		this.History.UseVisualStyleBackColor = true;
		this.panel8.AutoScroll = true;
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistoryDetail);
		this.panel8.Controls.Add(this.panel9);
		this.panel8.Controls.Add(this.panel7);
		this.panel8.Controls.Add(this.panel3);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel8.Location = new System.Drawing.Point(2, 2);
		this.panel8.Margin = new System.Windows.Forms.Padding(2);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(967, 643);
		this.panel8.TabIndex = 0;
		this.grdHistoryDetail.AllowDelete = (InheritableBoolean)1;
		grdHistoryDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistoryDetail.DesignTimeLayout = grdHistoryDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistoryDetail.DynamicFiltering = true;
		this.grdHistoryDetail.FilterMode = (FilterMode)2;
		this.grdHistoryDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistoryDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Location = new System.Drawing.Point(0, 443);
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Name = "grdHistoryDetail";
		((ScrollableBase)this.grdHistoryDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Size = new System.Drawing.Size(967, 200);
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).TabIndex = 44;
		this.grdHistoryDetail.TotalRow = (InheritableBoolean)1;
		this.grdHistoryDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistoryDetail).VisualStyle = (VisualStyle)4;
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(0, 419);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(967, 24);
		this.panel9.TabIndex = 46;
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.grdhistory);
		this.panel7.Controls.Add(this.panel20);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 53);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(967, 366);
		this.panel7.TabIndex = 45;
		this.grdhistory.AllowEdit = (InheritableBoolean)2;
		grdhistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdhistory.DesignTimeLayout = grdhistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdhistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdhistory.DynamicFiltering = true;
		this.grdhistory.FilterMode = (FilterMode)2;
		this.grdhistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdhistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdhistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdhistory).Location = new System.Drawing.Point(0, 105);
		((System.Windows.Forms.Control)(object)this.grdhistory).Name = "grdhistory";
		((ScrollableBase)this.grdhistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdhistory).Size = new System.Drawing.Size(967, 261);
		((System.Windows.Forms.Control)(object)this.grdhistory).TabIndex = 44;
		this.grdhistory.TotalRow = (InheritableBoolean)1;
		this.grdhistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdhistory).VisualStyle = (VisualStyle)4;
		this.grdhistory.ColumnButtonClick += new ColumnActionEventHandler(grdhistory_ColumnButtonClick);
		this.grdhistory.SelectionChanged += new System.EventHandler(grdhistory_SelectionChanged);
		this.grdhistory.LinkClicked += new ColumnActionEventHandler(grdhistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdhistory).DoubleClick += new System.EventHandler(grdhistory_DoubleClick);
		this.panel20.Controls.Add(this.groupBox2);
		this.panel20.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel20.Location = new System.Drawing.Point(0, 0);
		this.panel20.Name = "panel20";
		this.panel20.Size = new System.Drawing.Size(967, 105);
		this.panel20.TabIndex = 45;
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemHistory);
		this.groupBox2.Controls.Add(this.label22);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbStatusHistory);
		this.groupBox2.Controls.Add(this.label21);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobLotHistory);
		this.groupBox2.Controls.Add(this.label19);
		this.groupBox2.Controls.Add(this.btnShowHistory);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCropYearHistory);
		this.groupBox2.Controls.Add(this.label20);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSupplierCustomerHistory);
		this.groupBox2.Controls.Add(this.label24);
		this.groupBox2.Controls.Add(this.label23);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbAnalysisGroupHistory);
		this.groupBox2.Controls.Add(this.label56);
		this.groupBox2.Controls.Add(this.label57);
		this.groupBox2.Controls.Add(this.ToDocNoHistory);
		this.groupBox2.Controls.Add(this.FromDocNoHistory);
		this.groupBox2.Controls.Add(this.ToDateHistory);
		this.groupBox2.Controls.Add(this.FromDateHistory);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(5, 2);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(961, 99);
		this.groupBox2.TabIndex = 4;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Filters";
		this.CmbItemHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbItemHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance177).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance177;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance178).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance178).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance178).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance178;
		((AppearanceBase)appearance179).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance179;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance180).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance180).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance180).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance180).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance180;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance181).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance181).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance181;
		((AppearanceBase)appearance183).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance183).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance183;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance184).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance184;
		((AppearanceBase)appearance185).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance185).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance185;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance186).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance186).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance186).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance186).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance186).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance186;
		((AppearanceBase)appearance187).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance187;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance188).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance188).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance188;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance189).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance189;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemHistory).Location = new System.Drawing.Point(455, 13);
		((System.Windows.Forms.Control)(object)this.CmbItemHistory).Name = "CmbItemHistory";
		((System.Windows.Forms.Control)(object)this.CmbItemHistory).Size = new System.Drawing.Size(218, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemHistory).TabIndex = 5;
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(678, 19);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(40, 15);
		this.label22.TabIndex = 100;
		this.label22.Text = "Status";
		this.CmbStatusHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbStatusHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbStatusHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbStatusHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbStatusHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.CmbStatusHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbStatusHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbStatusHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbStatusHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance54;
		((SpecialBoxBase)((UltraGridBase)this.CmbStatusHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbStatusHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbStatusHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbStatusHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbStatusHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbStatusHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbStatusHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbStatusHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbStatusHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance60).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbStatusHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.CmbStatusHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbStatusHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance61).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbStatusHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbStatusHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.CmbStatusHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbStatusHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbStatusHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbStatusHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbStatusHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.CmbStatusHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbStatusHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbStatusHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbStatusHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbStatusHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbStatusHistory).Location = new System.Drawing.Point(764, 13);
		((System.Windows.Forms.Control)(object)this.CmbStatusHistory).Name = "CmbStatusHistory";
		((System.Windows.Forms.Control)(object)this.CmbStatusHistory).Size = new System.Drawing.Size(191, 26);
		((System.Windows.Forms.Control)(object)this.CmbStatusHistory).TabIndex = 8;
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(389, 73);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(46, 15);
		this.label21.TabIndex = 98;
		this.label21.Text = "Job Lot";
		this.CmbJobLotHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbJobLotHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobLotHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobLotHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance164).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobLotHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.CmbJobLotHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobLotHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance165).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance165).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance165).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLotHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance165;
		((AppearanceBase)appearance166).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLotHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance166;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLotHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance167).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance167).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance167).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLotHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance167;
		((UltraGridBase)this.CmbJobLotHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobLotHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance168).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobLotHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance168;
		((AppearanceBase)appearance169).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance169).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobLotHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance169;
		((UltraGridBase)this.CmbJobLotHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobLotHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLotHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance170;
		((AppearanceBase)appearance172).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance172).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobLotHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance172;
		((UltraGridBase)this.CmbJobLotHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobLotHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance173).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance173).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance173).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance173).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance173).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLotHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance173;
		((AppearanceBase)appearance174).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobLotHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance174;
		((UltraGridBase)this.CmbJobLotHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobLotHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance175).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobLotHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance175;
		((UltraGridBase)this.CmbJobLotHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance176).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobLotHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance176;
		((UltraGridBase)this.CmbJobLotHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobLotHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobLotHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobLotHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbJobLotHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobLotHistory).Location = new System.Drawing.Point(455, 67);
		((System.Windows.Forms.Control)(object)this.CmbJobLotHistory).Name = "CmbJobLotHistory";
		((System.Windows.Forms.Control)(object)this.CmbJobLotHistory).Size = new System.Drawing.Size(218, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobLotHistory).TabIndex = 7;
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(389, 46);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(57, 15);
		this.label19.TabIndex = 96;
		this.label19.Text = "Crop Year";
		this.btnShowHistory.BackColor = System.Drawing.Color.Teal;
		this.btnShowHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShowHistory.ForeColor = System.Drawing.Color.White;
		this.btnShowHistory.Location = new System.Drawing.Point(893, 67);
		this.btnShowHistory.Margin = new System.Windows.Forms.Padding(2);
		this.btnShowHistory.Name = "btnShowHistory";
		this.btnShowHistory.Size = new System.Drawing.Size(62, 26);
		this.btnShowHistory.TabIndex = 10;
		this.btnShowHistory.Text = "Show";
		this.btnShowHistory.UseVisualStyleBackColor = false;
		this.btnShowHistory.Click += new System.EventHandler(btnShowHistory_Click);
		this.CmbCropYearHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbCropYearHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCropYearHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCropYearHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance33).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCropYearHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCropYearHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCropYearHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCropYearHistory).Location = new System.Drawing.Point(455, 40);
		((System.Windows.Forms.Control)(object)this.CmbCropYearHistory).Name = "CmbCropYearHistory";
		((System.Windows.Forms.Control)(object)this.CmbCropYearHistory).Size = new System.Drawing.Size(218, 26);
		((System.Windows.Forms.Control)(object)this.CmbCropYearHistory).TabIndex = 6;
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(6, 73);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(86, 15);
		this.label20.TabIndex = 94;
		this.label20.Text = "Supplier Name";
		this.CmbSupplierCustomerHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbSupplierCustomerHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSupplierCustomerHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSupplierCustomerHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSupplierCustomerHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbSupplierCustomerHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSupplierCustomerHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplierCustomerHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplierCustomerHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance41;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplierCustomerHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplierCustomerHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbSupplierCustomerHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSupplierCustomerHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSupplierCustomerHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSupplierCustomerHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbSupplierCustomerHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSupplierCustomerHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplierCustomerHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSupplierCustomerHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbSupplierCustomerHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSupplierCustomerHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplierCustomerHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSupplierCustomerHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbSupplierCustomerHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSupplierCustomerHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSupplierCustomerHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbSupplierCustomerHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSupplierCustomerHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbSupplierCustomerHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSupplierCustomerHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSupplierCustomerHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSupplierCustomerHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSupplierCustomerHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSupplierCustomerHistory).Location = new System.Drawing.Point(116, 67);
		((System.Windows.Forms.Control)(object)this.CmbSupplierCustomerHistory).Name = "CmbSupplierCustomerHistory";
		((System.Windows.Forms.Control)(object)this.CmbSupplierCustomerHistory).Size = new System.Drawing.Size(269, 26);
		((System.Windows.Forms.Control)(object)this.CmbSupplierCustomerHistory).TabIndex = 4;
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(678, 46);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(86, 15);
		this.label24.TabIndex = 92;
		this.label24.Text = "Analysis Group";
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.Location = new System.Drawing.Point(389, 19);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(67, 15);
		this.label23.TabIndex = 91;
		this.label23.Text = "Item Name";
		this.CmbAnalysisGroupHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbAnalysisGroupHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbAnalysisGroupHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbAnalysisGroupHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbAnalysisGroupHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.CmbAnalysisGroupHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbAnalysisGroupHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbAnalysisGroupHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAnalysisGroupHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance67;
		((SpecialBoxBase)((UltraGridBase)this.CmbAnalysisGroupHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAnalysisGroupHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.CmbAnalysisGroupHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbAnalysisGroupHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbAnalysisGroupHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbAnalysisGroupHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbAnalysisGroupHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbAnalysisGroupHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAnalysisGroupHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance73).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbAnalysisGroupHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.CmbAnalysisGroupHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbAnalysisGroupHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance74).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAnalysisGroupHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbAnalysisGroupHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.CmbAnalysisGroupHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbAnalysisGroupHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbAnalysisGroupHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.CmbAnalysisGroupHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbAnalysisGroupHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.CmbAnalysisGroupHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbAnalysisGroupHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbAnalysisGroupHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbAnalysisGroupHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbAnalysisGroupHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbAnalysisGroupHistory).Location = new System.Drawing.Point(764, 40);
		((System.Windows.Forms.Control)(object)this.CmbAnalysisGroupHistory).Name = "CmbAnalysisGroupHistory";
		((System.Windows.Forms.Control)(object)this.CmbAnalysisGroupHistory).Size = new System.Drawing.Size(191, 26);
		((System.Windows.Forms.Control)(object)this.CmbAnalysisGroupHistory).TabIndex = 9;
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
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.toolStrip1);
		this.panel3.Controls.Add(this.ctrlGrdBar1);
		this.panel3.Controls.Add(this.label15);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(967, 53);
		this.panel3.TabIndex = 4;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnNewHistory, this.btnRefreshHistory });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(967, 27);
		this.toolStrip1.TabIndex = 23;
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
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(927, 28);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdhistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 23);
		this.ctrlGrdBar1.TabIndex = 21;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label15.AutoSize = true;
		this.label15.BackColor = System.Drawing.Color.Teal;
		this.label15.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.Color.White;
		this.label15.Location = new System.Drawing.Point(3, 29);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(204, 19);
		this.label15.TabIndex = 0;
		this.label15.Text = "Sample Analysis History";
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(979, 676);
		this.panel1.TabIndex = 53;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(979, 676);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "InvLabSampleAnalysis";
		base.ShowIcon = false;
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(InvLabSampleAnalysis_FormClosing);
		base.Load += new System.EventHandler(InvLabSampleAnalysis_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(InvLabSampleAnalysis_KeyDown);
		this.tabControl1.ResumeLayout(false);
		this.Form.ResumeLayout(false);
		this.Form.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.panel12.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdSubItems).EndInit();
		this.panel13.ResumeLayout(false);
		this.panel13.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel11.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCommissionAgent).EndInit();
		this.panel14.ResumeLayout(false);
		this.panel14.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbStatus).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSampleLogNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPurchaseOrder).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYear).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAnalysisType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAnalysisGroup).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplierName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.PicBCookingImage).EndInit();
		((System.ComponentModel.ISupportInitialize)this.PicBAnalysisImage).EndInit();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		this.History.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistoryDetail).EndInit();
		this.panel7.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdhistory).EndInit();
		this.panel20.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbItemHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbStatusHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLotHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYearHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplierCustomerHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAnalysisGroupHistory).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.panel1.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}

}
