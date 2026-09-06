using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.BLL.Lab;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Common;
using Architecture.Model.Inventory;
using Architecture.Model.Lab;
using Architecture.Reports;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Lab;

public class PackingMaterialLabPurchaseAnalysis : Form
{
	private int RefDocumentTypeId;

	private PendingGPForPurchaseLab LoadGp;

	public UserAccount UserAccount = new UserAccount();

	private Attachment AT = new Attachment();

	public DataTable dtViewAttachments = new DataTable();

	private int RecId;

	private DataTable dtgpsr = new DataTable();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtgetIdbyDocNo = new DataTable();

	private DataTable dtLabStatus = new DataTable();

	private Rightsobjects formrights = new Rightsobjects();

	private List<InvLabAnalysisPurchaseDetail> detaillst = new List<InvLabAnalysisPurchaseDetail>();

	private int AnalysisGroupId;

	private int DocumentTypeId = 306;

	private IContainer components;

	private ToolStripSeparator toolStripSeparator;

	private ToolStripButton btnprint;

	private Button btnimg1;

	private ToolStripButton btnSave;

	private ToolStripButton btnUpdate;

	private ToolStripButton btnNew;

	private ToolStrip btn;

	private ToolStripButton btnattachment;

	private TextBox txtdocno;

	private Label cmpcity;

	private Label label6;

	private Label cmbsupplier;

	private Label label10;

	private Label txtsampledate;

	private CtrlGrdBar ctrlGrdBar1;

	private TabPage tabHistory;

	private Panel panel4;

	internal GridEX grdhistory;

	private Label txt;

	private Label label8;

	private TabControl tabControl1;

	private TabPage tabForm;

	private Panel PanalParamGrdsForm;

	private Panel PanelFeildForms;

	private Label label7;

	private DateTimePicker txtdocdate;

	private Label label1;

	private TextBox txtOrderNo;

	private Button btnimage2;

	private Label label4;

	private TextBox txtvehicleno;

	private TextBox txtbiltyno;

	private TextBox txtSampleId;

	private TextBox txtAnaylstName;

	private Label label12;

	internal UltraCombo cmbitem;

	internal UltraCombo cmbanalysisgroup;

	private CheckBox IsPrint;

	internal UltraCombo cmbGatePassNo;

	private TextBox txtPurchaseOrderId;

	internal UltraCombo CmbLabStatus;

	private Panel panel3;

	private Panel panel5;

	private Panel panel9;

	private TextBox txtScheduleId;

	private Label label18;

	private TextBox txtNoofBagsInspection;

	private Panel panel13;

	private Panel panel14;

	private Panel panel17;

	internal GridEX grdHistroryPurAnalysis;

	private Panel panel16;

	private Label label14;

	private Label label15;

	private CtrlGrdBar ctrlGrdBar2;

	internal UltraCombo cmbPartyName;

	private Label label19;

	private Panel panel22;

	private Label label21;

	private TextBox txtremarks;

	private ToolStripButton btnSlip257;

	private ToolStripButton BtnLoader;

	private Panel panel21;

	private ToolStrip toolStrip1;

	private ToolStripButton btnRefreshHistory;

	private Panel panel20;

	private GroupBox groupBox2;

	private Button btnShowHistory;

	private Label label53;

	private Label label56;

	private Label label57;

	internal UltraCombo CmbCustomerHistory;

	private TextBox ToDocNoHistory;

	private TextBox FromDocNoHistory;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	private Label label27;

	internal UltraCombo CmbGpNoHistory;

	private Label label26;

	internal UltraCombo CmbVehicleNoHistory;

	private Label label25;

	private Label label24;

	private Label label23;

	internal UltraCombo cmbApprovedStatusHistory;

	internal UltraCombo CmbAnalystStatusHistory;

	internal UltraCombo CmbItemHistory;

	private ToolStripButton btnNewHistory;

	private Panel panel23;

	private CheckBox chkPrintII;

	private TextBox txtCityArea;

	private Label label32;

	private Label label31;

	private DateTimePicker datBiltyDate;

	private TextBox txtApprovedBy;

	private Label label34;

	private TextBox txtApprovalStatus;

	private Label label35;

	private Panel panel2;

	private Label PaymentVoucherLable;

	private ToolStripButton btnRefresh;

	private Label label45;

	private Label label46;

	internal GridEX grd;

	private DateTimePicker datGpDate;

	private Label label2;

	public PackingMaterialLabPurchaseAnalysis(UserAccount pUserAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUserAccount;
	}

	private void PackingMaterialLabPurchaseAnalysis_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1350, 750);
			PanalParamGrdsForm.AutoScroll = true;
			PanalParamGrdsForm.AutoScrollMargin = new Size(20, 20);
			PanalParamGrdsForm.AutoScrollMinSize = new Size(900, 750);
			dtViewAttachments.Columns.Add("AttchmentPath");
			formrights = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnSave.Enabled = formrights.DoHaveSaveRight;
			btnprint.Enabled = formrights.DoHavePrintRights;
			btnUpdate.Enabled = formrights.DoHaveUpdateRights;
			ctrlGrdBar1.mGridChooseFielder.Enabled = formrights.DoHaveFieldChooserRights;
			ctrlGrdBar1.mGridSaveLayouts.Enabled = formrights.DoHaveSaveLayoutRights;
			ctrlGrdBar1.mGridPrint.Enabled = formrights.DoHaveGridPrintRights;
			ctrlGrdBar1.mGridExport.Enabled = formrights.DoHaveGridExportRights;
			ctrlGrdBar1.GroupCollaps.Enabled = formrights.DoHaveGridGroupCollapseRights;
			ctrlGrdBar1.GroupExpand.Enabled = formrights.DoHaveGridGroupExpandRights;
			cmbGatePassNo.Focus();
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			GpNoFill();
			GenerateCode();
			LabstatusFill();
			GetPMLabStatus();
			FromDateHistory.Value = DateTime.Now;
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
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		try
		{
			DataTable dtSupp = new DataTable();
			dtSupp.Columns.Add("Id");
			dtSupp.Columns.Add("name");
			DataTable dtItem = dtSupp.Clone();
			DataTable dtAnalyst = dtSupp.Clone();
			DataTable dtApprovedStatus = dtSupp.Clone();
			DataTable dtGpNo = dtSupp.Clone();
			DataTable dtVehicle = dtSupp.Clone();
			DataTable dt = InvLabAnalysisPurchaseHeader.GetDataForDropDownFromLabPurchaseAnaylsis(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Activity = "",
				DocumentTypeIds = "306"
			});
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToString(dt.Rows[i]["Activity"]) == "Supplier")
					{
						dtSupp.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
					else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "Item")
					{
						dtItem.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
					else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "AnaylstStatus")
					{
						dtAnalyst.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
					else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "ApprovedStatus")
					{
						dtApprovedStatus.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
					else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "GpNo")
					{
						dtGpNo.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
					else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "VehicleNo")
					{
						dtVehicle.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
				}
			}
			DDL.BindDDL(dtSupp, CmbCustomerHistory, "Id", "name", "Supplier Name", ZeroIndex: false);
			DDL.BindDDL(dtItem, CmbItemHistory, "Id", "name", "Item Name", ZeroIndex: false);
			DDL.BindDDL(dtAnalyst, CmbAnalystStatusHistory, "Id", "name", "Analyst Status", ZeroIndex: false);
			DDL.BindDDL(dtApprovedStatus, cmbApprovedStatusHistory, "Id", "name", "Approved Status", ZeroIndex: false);
			DDL.BindDDL(dtGpNo, CmbGpNoHistory, "Id", "name", "Gp No", ZeroIndex: false);
			((UltraGridBase)CmbGpNoHistory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			DDL.BindDDL(dtVehicle, CmbVehicleNoHistory, "Id", "name", "Vahicle No", ZeroIndex: false);
			((UltraGridBase)CmbVehicleNoHistory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GpNoFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		try
		{
			DataTable dtGpNo = GatePassInward.GetGpSrNoForPurchaseAnalysis(new GatePassInward
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				BranchesId = UserAccount.BranchesId,
				RefDocumentTypeId = 700
			});
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("GpSrNo");
			dt.Columns.Add("GpDate", typeof(DateTime));
			dt.Columns.Add("VehicleNo");
			dt.Columns.Add("OrderType");
			dt.Columns.Add("SupplierName");
			dt.Columns.Add("LabType");
			dt.Columns.Add("BiltyNo");
			dt.Columns.Add("BiltyDate");
			if (dtGpNo.Rows.Count > 0)
			{
				for (int i = 0; i < dtGpNo.Rows.Count; i++)
				{
					dt.Rows.Add(dtGpNo.Rows[i]["Id"], dtGpNo.Rows[i]["GpSrNo"], dtGpNo.Rows[i]["GpDate"], dtGpNo.Rows[i]["VehicleNo"], dtGpNo.Rows[i]["OrderType"], dtGpNo.Rows[i]["SupplierName"], dtGpNo.Rows[i]["LabType"], dtGpNo.Rows[i]["BiltyNo"], dtGpNo.Rows[i]["BiltyNo"]);
				}
			}
			DDL.BindDDL(dt, cmbGatePassNo, "Id", "GpSrNo", "GpSrNo", ZeroIndex: false);
			((UltraGridBase)cmbGatePassNo).DisplayLayout.Bands[0].Columns[1].Width = 120;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateCode()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		try
		{
			int Code = InvLabAnalysisPurchaseHeader.GenerateCode(new InvLabAnalysisPurchaseHeader
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = DocumentTypeId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				BranchesId = UserAccount.BranchesId
			});
			if (Conversion.ToInt((object)Code) > 0)
			{
				txtdocno.Text = Code.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtgatepassno_Leave(object sender, EventArgs e)
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Expected O, but got Unknown
		try
		{
			dtgpsr.Rows.Clear();
			RefDocumentTypeId = 0;
			((Control)(object)cmbitem).Text = string.Empty;
			((UltraGridBase)cmbitem).DataSource = null;
			InvLabAnalysisPurchaseHeader obj = new InvLabAnalysisPurchaseHeader();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.GatePassInwardId = Conversion.ToInt(cmbGatePassNo.Value);
			dtgpsr = InvLabAnalysisPurchaseHeader.GetGpDetailByGatepassNo(obj);
			if (dtgpsr.Rows.Count > 0)
			{
				txtOrderNo.Text = dtgpsr.Rows[0]["OrderNo"].ToString();
				txtbiltyno.Text = dtgpsr.Rows[0]["BiltyNo"].ToString();
				datBiltyDate.Value = Conversion.ToDateTime(dtgpsr.Rows[0]["BiltyDate"]);
				txtvehicleno.Text = dtgpsr.Rows[0]["VehicleNo"].ToString();
				txtCityArea.Text = Conversion.ToString(dtgpsr.Rows[0]["CityName"]);
				txtNoofBagsInspection.Text = Conversion.ToString(dtgpsr.Rows[0]["GpQty"]);
				RefDocumentTypeId = Conversion.ToInt(dtgpsr.Rows[0]["RefDocumentTypeId"]);
				DataTable dtParty = new DataTable();
				dtParty.Columns.Add("Id");
				dtParty.Columns.Add("PartyName");
				dtParty.Rows.Add(dtgpsr.Rows[0]["SupplierCustomerId"], dtgpsr.Rows[0]["PartyName"]);
				DDL.BindDDL(dtParty, cmbPartyName, "Id", "PartyName", "Party Name", ZeroIndex: false);
				((UltraGridBase)cmbPartyName).Rows[0].Activate();
				txtPurchaseOrderId.Text = Conversion.ToString(dtgpsr.Rows[0]["PurchaseOrderId"]);
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("ItemName");
				dt.Columns.Add("PoDetailId");
				dt.Columns.Add("SupplierCustomerId");
				dt.Columns.Add("PartyName");
				dt.Columns.Add("ParentcategoryId");
				dt.Columns.Add("Parentcategory");
				for (int i = 0; i < dtgpsr.Rows.Count; i++)
				{
					dt.Rows.Add(dtgpsr.Rows[i]["OrderItemId"], dtgpsr.Rows[i]["ItemName"], dtgpsr.Rows[i]["PoDetailId"], dtgpsr.Rows[i]["SupplierCustomerId"], dtgpsr.Rows[i]["PartyName"], dtgpsr.Rows[i]["ParentcategoryId"], dtgpsr.Rows[i]["Parentcategory"]);
				}
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, cmbitem, "Id", "ItemName", "ItemName", ZeroIndex: false);
					((UltraGridBase)cmbitem).DisplayLayout.Bands[0].Columns[2].Hidden = true;
					((UltraGridBase)cmbitem).DisplayLayout.Bands[0].Columns[3].Hidden = true;
					((UltraGridBase)cmbitem).DisplayLayout.Bands[0].Columns[4].Hidden = true;
					((UltraGridBase)cmbitem).DisplayLayout.Bands[0].Columns[5].Hidden = true;
					((UltraGridBase)cmbitem).Rows[0].Activate();
				}
				Conversion.ToInt(cmbitem.Value);
				Conversion.ToInt((object)txtPurchaseOrderId.Text);
				if (((UltraGridBase)cmbitem).ActiveRow != null && Conversion.ToInt(cmbitem.Value) > 0)
				{
					AnalysisGroup(Conversion.ToInt(((UltraDropDownBase)cmbitem).SelectedRow.Cells["ParentcategoryId"].Value));
				}
			}
			else
			{
				txtOrderNo.Text = string.Empty;
				txtbiltyno.Text = string.Empty;
				txtvehicleno.Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AnalysisGroup(int ParentcategoryId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		try
		{
			DataTable dtLAG = InvLabAnalysisGroup.GetAllOrById(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				ParentCategoryId = ParentcategoryId
			});
			if (dtLAG.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("AnalysisGroupDescription", typeof(string));
				dt.Columns.Add("GroupTypeId", typeof(int));
				dt.Columns.Add("GroupType", typeof(string));
				for (int i = 0; i < dtLAG.Rows.Count; i++)
				{
					dt.Rows.Add(dtLAG.Rows[i]["Id"], dtLAG.Rows[i]["AnalysisGroupDescription"], dtLAG.Rows[i]["GroupType"], dtLAG.Rows[i]["InvParentCateDescription"]);
				}
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, cmbanalysisgroup, "Id", "AnalysisGroupDescription", "Analysis Group", ZeroIndex: false);
					((UltraGridBase)cmbanalysisgroup).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				}
			}
			else
			{
				((Control)(object)cmbanalysisgroup).Text = string.Empty;
				((UltraGridBase)cmbanalysisgroup).DataSource = null;
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
			DDL.BindDDL(dt, CmbLabStatus, "Id", "Status", "Analysis Status", ZeroIndex: false);
			((UltraGridBase)CmbLabStatus).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetPMLabStatus()
	{
		try
		{
			dtLabStatus = InvLabAnalysisPurchaseHeader.GetPMLabStatus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbanalysisgroup_Leave(object sender, EventArgs e)
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Expected O, but got Unknown
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Expected O, but got Unknown
		try
		{
			if (RecId > 0 && Conversion.ToInt(cmbanalysisgroup.Value) == AnalysisGroupId)
			{
				return;
			}
			detaillst.Clear();
			AnalysisGroupId = 0;
			DataTable dtDetail = InvLabGroupAnalysisStandards.GetParametersFromGroupStandards(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Id = Conversion.ToInt(cmbanalysisgroup.Value)
			});
			if (dtDetail.Rows.Count > 0)
			{
				for (int i = 0; i < dtDetail.Rows.Count; i++)
				{
					InvLabAnalysisPurchaseDetail lstobj = new InvLabAnalysisPurchaseDetail();
					lstobj.InvLabAnalysisPurchaseSubParamsDetailslist = new List<InvLabAnalysisPurchaseSubParamsDetail>();
					lstobj.InvLabAnalysisItemsId = Conversion.ToInt(dtDetail.Rows[i]["InvLabAnalysisItemsId"]);
					lstobj.InvLabGroupAnalysisStandardsId = Conversion.ToInt(dtDetail.Rows[i]["Id"]);
					lstobj.AnalysisGroupDescription = Conversion.ToString(dtDetail.Rows[i]["AnalysisGroupDescription"]);
					lstobj.AnalysisParameterDescription = Conversion.ToString(dtDetail.Rows[i]["AnalysisParameterDescription"]);
					lstobj.MinValue = Conversion.ToDouble(dtDetail.Rows[i]["MinValue"]);
					lstobj.MaxValue = Conversion.ToDouble(dtDetail.Rows[i]["MaxValue"]);
					lstobj.ParamsMinValue = Conversion.ToDouble(dtDetail.Rows[i]["ParamsMinValue"]);
					lstobj.ParamsMaxValue = Conversion.ToDouble(dtDetail.Rows[i]["ParamsMaxValue"]);
					lstobj.InAnalysisResult = 0.0;
					lstobj.AnalysisStutusId = 0;
					detaillst.Add(lstobj);
				}
				grd.DataSource = detaillst;
				grd.RetrieveStructure();
				grdsetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool formvalidation()
	{
		if (((UltraGridBase)cmbGatePassNo).ActiveRow == null || Conversion.ToInt(cmbGatePassNo.Value) == 0)
		{
			MessageBox.Show("Please GpNo GatePass No");
			cmbGatePassNo.Focus();
			return false;
		}
		if (((UltraGridBase)cmbanalysisgroup).ActiveRow == null || Conversion.ToInt(cmbanalysisgroup.Value) == 0)
		{
			MessageBox.Show("Please Select Analysis Group No");
			cmbanalysisgroup.Focus();
			return false;
		}
		if (((UltraGridBase)cmbitem).ActiveRow == null || Conversion.ToInt(cmbitem.Value) == 0)
		{
			MessageBox.Show("Item Field Required");
			cmbitem.Focus();
			return false;
		}
		if (Conversion.ToInt((object)txtNoofBagsInspection.Text) == 0)
		{
			MessageBox.Show("Analysis Qty Field is Required");
			txtNoofBagsInspection.Focus();
			return false;
		}
		if (txtAnaylstName.Text.Trim() == string.Empty)
		{
			MessageBox.Show("AnalystName field is required");
			txtAnaylstName.Focus();
			return false;
		}
		if (((UltraGridBase)CmbLabStatus).ActiveRow == null)
		{
			MessageBox.Show("Status field required");
			CmbLabStatus.Focus();
			return false;
		}
		return true;
	}

	private void grdsetting()
	{
		try
		{
			grd.AllowDelete = (InheritableBoolean)2;
			List<string> hiddenColumns = new List<string>
			{
				"Id", "InvLabGroupAnalysisStandardsId", "InvLabAnalysisPurchaseHeaderId", "InvLabAnalysisItemsId", "ParamsMinValue", "ParamsMaxValue", "ParamsMinValue", "ParamsMaxValue", "PmLabStatus", "IsEditableAfterApproval",
				"MinValue", "MaxValue"
			};
			Dictionary<string, EditType> editableColumns = new Dictionary<string, EditType>
			{
				{
					"InAnalysisResult",
					(EditType)1
				},
				{
					"RemarksDetail",
					(EditType)1
				}
			};
			List<string> NotTotalColumns = new List<string> { "ParamsMinValue", "ParamsMaxValue" };
			GridEX_Helper.GridWrappingAndColumnSettings(grd, 3, 2, hiddenColumns, editableColumns, null, ColumnSetting: true, BracketsForamt: false, NotTotalColumns);
			GridComboBind();
			grd.RootTable.Columns["AnalysisGroupDescription"].Caption = "Analysis Group";
			grd.RootTable.Columns["AnalysisParameterDescription"].Caption = "Analysis Parameter";
			grd.RootTable.Columns["InAnalysisResult"].Caption = "Analysis Result";
			grd.RootTable.Columns["IsEditableAfterApproval"].Caption = "Editable After Approval";
			grd.RootTable.Groups.Add("AnalysisGroupDescription");
			grd.HideColumnsWhenGrouped = (InheritableBoolean)1;
			grd.RootTable.Columns["AnalysisParameterDescription"].Width = Constants.LabConstants.AnalysisParameter + 40;
			grd.RootTable.Columns["MinValue"].Width = Constants.LabConstants.MinValue;
			grd.RootTable.Columns["MaxValue"].Width = Constants.LabConstants.MaxValue;
			grd.RootTable.Columns["InAnalysisResult"].Width = Constants.LabConstants.ResultValue;
			grd.RootTable.Columns["IsEditableAfterApproval"].Width = 70;
			grd.RootTable.Columns["RemarksDetail"].Width = 250;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridComboBind()
	{
		try
		{
			if (dtLabStatus != null && dtLabStatus.Rows.Count != 0)
			{
				GridEX_Helper.ComboBind(grd.RootTable.Columns["AnalysisStutusId"], dtLabStatus, "Id", "PmLabStatus", "Lab Status", 100, limitToList: true, editType: true, grd.RootTable.Columns["AnalysisParameterDescription"].Position + 1);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		grd.UpdateData();
	}

	private void grd_FormattingRow(object sender, RowLoadEventArgs e)
	{
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Expected O, but got Unknown
		try
		{
			if (grd.RowCount <= 0)
			{
				return;
			}
			GridEXColumn maxCol = grd.RootTable.Columns["MaxValue"];
			GridEXColumn minCol = grd.RootTable.Columns["MinValue"];
			GridEXColumn resultCol = grd.RootTable.Columns["InAnalysisResult"];
			GridEXColumn ParameterId = grd.RootTable.Columns["InvLabAnalysisItemsId"];
			if (maxCol != null && minCol != null && resultCol != null)
			{
				GridEXCell maxCell = e.Row.Cells[((JanusCollectionElement)maxCol).Index];
				_ = e.Row.Cells[((JanusCollectionElement)minCol).Index];
				GridEXCell ResultCell = e.Row.Cells[((JanusCollectionElement)resultCol).Index];
				_ = e.Row.Cells[((JanusCollectionElement)ParameterId).Index];
				if (Conversion.ToDouble(ResultCell.Value) > 0.0 && Conversion.ToDouble(ResultCell.Value) > Conversion.ToDouble(maxCell.Value))
				{
					ResultCell.FormatStyle = new GridEXFormatStyle();
					ResultCell.FormatStyle.ForeColor = Color.Red;
					ResultCell.FormatStyle.FontBold = (TriState)2;
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
		try
		{
			if (!formvalidation())
			{
				return;
			}
			if (grd.GetRows().Count() == 0)
			{
				throw new Exception("Grid Record Not Found");
			}
			InvLabAnalysisPurchaseHeader labheader = new InvLabAnalysisPurchaseHeader();
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
			int num = detaillst.Count((InvLabAnalysisPurchaseDetail x) => x.InAnalysisResult > 0.0);
			int countStatus = detaillst.Count((InvLabAnalysisPurchaseDetail x) => x.AnalysisStutusId > 0);
			if (num == 0 && countStatus == 0)
			{
				throw new Exception("Please enter a Result Value or select a Lab Status for at least one record.");
			}
			labheader.DocNo = Conversion.ToInt((object)txtdocno.Text);
			labheader.DocumentTypeId = DocumentTypeId;
			labheader.DocDate = Conversion.ToDateTime((object)txtdocdate.Value);
			labheader.SupplierCustomerId = Conversion.ToInt(cmbPartyName.Value);
			labheader.ApprovedDate = DateTime.Now;
			labheader.GatePassInwardId = Conversion.ToInt(cmbGatePassNo.Value);
			labheader.PurchaseOrderId = Conversion.ToInt((object)txtPurchaseOrderId.Text);
			labheader.SupCustCode = Conversion.ToString((object)txtOrderNo.Text);
			if (((Control)(object)CmbLabStatus).Text == "Accepted")
			{
				labheader.IsAccepted = true;
			}
			else
			{
				labheader.IsAccepted = false;
			}
			labheader.ItemId = Conversion.ToInt(cmbitem.Value);
			labheader.PoDetailId = Conversion.ToInt(((UltraDropDownBase)cmbitem).SelectedRow.Cells["PoDetailId"].Value);
			labheader.InvLabAnalysisGroup = Conversion.ToInt(cmbanalysisgroup.Value);
			labheader.RemarksHeader = Conversion.ToString((object)txtremarks.Text);
			labheader.NoofBagsInspection = Conversion.ToDouble((object)txtNoofBagsInspection.Text);
			labheader.PricingScheduleId = Conversion.ToInt((object)txtScheduleId.Text);
			labheader.AnalystName = txtAnaylstName.Text;
			labheader.CompanyId = UserAccount.CompanyId;
			labheader.OrganizationId = UserAccount.OrganizationId;
			labheader.BranchesId = UserAccount.BranchesId;
			labheader.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			labheader.EntryDate = DateTime.Now;
			labheader.EntryUser = UserAccount.ID;
			labheader.ModifyDate = DateTime.Now;
			labheader.ModifyUser = UserAccount.ID;
			labheader.ApprovedDate = DateTime.Now;
			labheader.IsApproved = false;
			labheader.LabSampleDetail = detaillst;
			labheader.dmsAttachment = new List<DMSAttachments>();
			labheader.DeleteAttachmentsList = new List<DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<InvLabAnalysisPurchaseHeader>(labheader, base.Name, AT, "dmsAttachment", "DeleteAttachmentsList");
			int success = InvLabAnalysisPurchaseHeader.Save(labheader);
			if (RecId > 0)
			{
				MessageBox.Show("Update Successfully");
				FormHelper.DeleteAllAttachmentsFromDb(AT, RecId, DocumentTypeId, base.Name);
			}
			else
			{
				MessageBox.Show("Save Successfully");
			}
			int GPId = Conversion.ToInt(cmbGatePassNo.Value);
			refresh();
			if (IsPrint.Checked)
			{
				LabAnalysisReport653(success);
			}
			if (chkPrintII.Checked)
			{
				CommonServices.InwardGatePassWithWbAndLabSlip(GPId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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
		try
		{
			refresh();
			btnSave.Visible = false;
			btnUpdate.Visible = true;
			((Control)(object)cmbGatePassNo).Enabled = false;
			new DataTable();
			RecId = ID;
			InvLabAnalysisPurchaseHeader obj = InvLabAnalysisPurchaseHeader.ReadById(ID);
			if (obj != null)
			{
				detaillst.Clear();
				tabControl1.SelectedIndex = 0;
				txtdocno.Text = Conversion.ToString((object)obj.DocNo);
				txtdocdate.Value = Conversion.ToDateTime((object)obj.DocDate);
				txtbiltyno.Text = Conversion.ToString((object)obj.BiltyNo);
				datBiltyDate.Value = Conversion.ToDateTime((object)obj.BiltyDate);
				txtCityArea.Text = Conversion.ToString((object)obj.CityName);
				txtApprovedBy.Text = Conversion.ToString((object)obj.UserNameAusr);
				txtApprovalStatus.Text = Conversion.ToString((object)obj.ApprovalStatus);
				txtvehicleno.Text = Conversion.ToString((object)obj.VehicleNo);
				txtPurchaseOrderId.Text = Conversion.ToString((object)obj.PurchaseOrderId);
				txtOrderNo.Text = Conversion.ToString((object)obj.SupCustCode);
				txtNoofBagsInspection.Text = Conversion.ToString((object)obj.NoofBagsInspection);
				RefDocumentTypeId = Conversion.ToInt((object)obj.RefDocumentTypeId);
				DataTable dtgp = new DataTable();
				dtgp.Columns.Add("Id");
				dtgp.Columns.Add("GpSrNo");
				dtgp.Rows.Add(Conversion.ToInt((object)obj.GatePassInwardId), Conversion.ToString((object)obj.GpSrNo));
				DDL.BindDDL(dtgp, cmbGatePassNo, "Id", "GpSrNo", "GpSrNo", ZeroIndex: false);
				((UltraGridBase)cmbGatePassNo).Rows[0].Activate();
				txtgatepassno_Leave(null, null);
				new DataTable
				{
					Columns = { "Id", "ItemName", "PoDetailId", "SupplierCustomerId", "PartyName", "ParentcategoryId", "Parentcategory" },
					Rows = { new object[7] { obj.ItemId, obj.ItemName, obj.PoDetailId, obj.SupplierCustomerId, obj.PartyName, obj.ParentcategoryId, obj.Parentcategory } }
				};
				((Control)(object)cmbitem).Text = string.Empty;
				((Control)(object)cmbPartyName).Text = string.Empty;
				cmbitem.Value = obj.ItemId;
				cmbPartyName.Value = obj.SupplierCustomerId;
				if (obj.IsAccepted)
				{
					((Control)(object)CmbLabStatus).Text = "Accepted";
				}
				else
				{
					((Control)(object)CmbLabStatus).Text = "Rejected";
				}
				if (((UltraGridBase)cmbitem).ActiveRow != null && Conversion.ToInt(cmbitem.Value) > 0)
				{
					AnalysisGroup(Conversion.ToInt(((UltraDropDownBase)cmbitem).SelectedRow.Cells["ParentcategoryId"].Value));
				}
				cmbanalysisgroup.Value = Conversion.ToInt((object)obj.InvLabAnalysisGroup);
				AnalysisGroupId = obj.InvLabAnalysisGroup;
				txtremarks.Text = Conversion.ToString((object)obj.RemarksHeader);
				txtAnaylstName.Text = Conversion.ToString((object)obj.AnalystName);
				txtScheduleId.Text = Conversion.ToString((object)obj.PricingScheduleId);
				detaillst = obj.LabSampleDetail;
				grd.DataSource = detaillst;
				grd.RetrieveStructure();
				grdsetting();
				FormHelper.LoadAttachmentsForObject(AT, RecId, base.Name);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		refresh();
	}

	private void refresh()
	{
		try
		{
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			AnalysisGroupId = 0;
			RecId = 0;
			label21.Text = "Lab Purchase Analysis";
			GenerateCode();
			GpNoFill();
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			((Control)(object)cmbGatePassNo).Text = string.Empty;
			txtbiltyno.Text = "";
			datBiltyDate.Value = DateTime.Now;
			txtCityArea.Text = string.Empty;
			txtApprovedBy.Text = string.Empty;
			txtApprovalStatus.Text = string.Empty;
			txtvehicleno.Text = "";
			txtOrderNo.Text = "";
			((Control)(object)cmbitem).Text = "";
			((Control)(object)cmbanalysisgroup).Text = "";
			txtPurchaseOrderId.Text = string.Empty;
			txtAnaylstName.Text = string.Empty;
			txtremarks.Text = string.Empty;
			txtNoofBagsInspection.Text = string.Empty;
			cmbGatePassNo.Focus();
			((Control)(object)cmbGatePassNo).Enabled = true;
			detaillst.Clear();
			dtgpsr.Rows.Clear();
			grd.ClearStructure();
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

	private void btnNewHistory_Click(object sender, EventArgs e)
	{
		try
		{
			FromDateHistory.Text = string.Empty;
			ToDateHistory.Text = string.Empty;
			FromDocNoHistory.Text = string.Empty;
			ToDocNoHistory.Text = string.Empty;
			((Control)(object)CmbCustomerHistory).Text = string.Empty;
			((Control)(object)CmbItemHistory).Text = string.Empty;
			((Control)(object)CmbAnalystStatusHistory).Text = string.Empty;
			((Control)(object)cmbApprovedStatusHistory).Text = string.Empty;
			((Control)(object)CmbGpNoHistory).Text = string.Empty;
			((Control)(object)CmbVehicleNoHistory).Text = string.Empty;
			grdhistory.ClearStructure();
			grdHistroryPurAnalysis.ClearStructure();
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

	private void gridsetting()
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id" };
			GridEX_Helper.GridWrappingAndColumnSettings(grdhistory, 3, 3, hiddenColumns);
			grdhistory.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			grdhistory.RootTable.Columns["ApprovedDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			grdhistory.RootTable.Columns["Attachments"].ColumnType = (ColumnType)5;
			grdhistory.RootTable.Columns["LabType"].Width = 80;
			grdhistory.RootTable.Columns["DocNo"].Width = 60;
			grdhistory.RootTable.Columns["AnalysisItem"].Width = 250;
			grdhistory.RootTable.Columns["GatePass#"].Width = 80;
			grdhistory.RootTable.Columns["SupplierCode"].Width = 90;
			grdhistory.RootTable.Columns["PartyName"].Width = 220;
			grdhistory.RootTable.Columns["BiltyNo"].Width = 70;
			grdhistory.RootTable.Columns["VehicleNo"].Width = 100;
			grdhistory.RootTable.Columns["Remarks"].Width = 300;
			grdhistory.RootTable.Columns["AnalystName"].Width = 200;
			grdhistory.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			grdhistory.RootTable.Columns["ApprovedDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			GridEX_Helper.AddButton(grdhistory, "Edit", "Edit", 50, 0);
			GridEX_Helper.AddButton(grdhistory, "Print", "Print", 50, 1);
			grdhistory.FrozenColumns = 2;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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
			cat.BranchesId = UserAccount.BranchesId;
			cat.DocumentTypeId = DocumentTypeId;
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
			cat.SupplierCustomerId = Conversion.ToInt(CmbCustomerHistory.Value);
			cat.ItemId = Conversion.ToInt(CmbItemHistory.Value);
			cat.GpId = Conversion.ToInt(CmbGpNoHistory.Value);
			cat.ApprovedFilter = ((Control)(object)cmbApprovedStatusHistory).Text;
			cat.AnalystStatus = ((Control)(object)CmbAnalystStatusHistory).Text;
			cat.VehicleNo = ((Control)(object)CmbVehicleNoHistory).Text;
			DataTable dtgetAll = InvLabAnalysisPurchaseHeader.GetAll(cat);
			if (dtgetAll.Rows.Count == 0)
			{
				MessageBox.Show("Record Not Found");
				grdhistory.ClearStructure();
				grdHistroryPurAnalysis.ClearStructure();
				return;
			}
			DataTable table = new DataTable();
			table.Columns.Add("Id");
			table.Columns.Add("LabType", typeof(string));
			table.Columns.Add("DocNo", typeof(int));
			table.Columns.Add("DocDate", typeof(DateTime));
			table.Columns.Add("GatePass#", typeof(int));
			table.Columns.Add("SupplierCode");
			table.Columns.Add("PartyName");
			table.Columns.Add("BiltyNo");
			table.Columns.Add("VehicleNo");
			table.Columns.Add("AnalysisItem");
			table.Columns.Add("AnalystName");
			table.Columns.Add("AnalystStatus");
			table.Columns.Add("ApprovedStatus");
			table.Columns.Add("EntryUser");
			table.Columns.Add("EntryDate", typeof(DateTime));
			table.Columns.Add("ApprovedUser");
			table.Columns.Add("ApprovedDate", typeof(DateTime));
			table.Columns.Add("Attachments", typeof(int));
			table.Columns.Add("Remarks");
			table.Clear();
			table.BeginLoadData();
			foreach (DataRow row in dtgetAll.Rows)
			{
				table.Rows.Add(row["Id"], row["LabType"], row["DocNo"], Conversion.ToDateTime(row["DocDate"]).ToShortDateString(), row["GpSrNo"], row["SupCustCode"], row["PartyName"], row["BiltyNo"], row["VehicleNo"], row["ItemName"], row["AnalystName"], row["AnalystStatus"], row["ApprovedStatus"], row["UserName"], row["EntryDate"], row["ApprovedUser"], row["ApprovedDate"], row["NoOfAttachments"], row["RemarksHeader"]);
			}
			table.EndLoadData();
			grdhistory.DataSource = table;
			grdhistory.RetrieveStructure();
			gridsetting();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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

	private void grdhistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdhistory.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				int id = Conversion.ToInt(item.Cells["Id"].Value);
				if (e.Column.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByScreenName(id, base.Name);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow row = grdhistory.CurrentRow;
			if (row != null && (int)row.RowType == 0 && row.Cells.Count != 0)
			{
				string key = e.Column.Key;
				int Id = Conversion.ToInt(row.Cells["Id"].Value);
				if (key == "Edit")
				{
					ReadById(Id);
				}
				if (key == "Print")
				{
					LabAnalysisReport653(Id);
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
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdhistory.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0)
			{
				return;
			}
			int num = Conversion.ToInt(grdhistory.CurrentRow.Cells["Id"].Value);
			int DocNo = Conversion.ToInt(grdhistory.CurrentRow.Cells["DocNo"].Value);
			label14.Text = "Purchase Analysis";
			Label label = label14;
			label.Text = label.Text + " (" + DocNo + ")";
			InvLabAnalysisPurchaseHeader obj = InvLabAnalysisPurchaseHeader.ReadById(num);
			DataTable dtPurHistory = new DataTable();
			dtPurHistory.Columns.Add("Id", typeof(int));
			dtPurHistory.Columns.Add("AnalysisGroup");
			dtPurHistory.Columns.Add("AnalysisParameter");
			dtPurHistory.Columns.Add("Lab Status", typeof(string));
			dtPurHistory.Columns.Add("ResultValue", typeof(double));
			dtPurHistory.Columns.Add("Remarks");
			dtPurHistory.Rows.Clear();
			if (obj.LabSampleDetail != null && obj.LabSampleDetail.Count > 0)
			{
				for (int i = 0; i < obj.LabSampleDetail.Count; i++)
				{
					dtPurHistory.Rows.Add(obj.LabSampleDetail[i].Id, obj.LabSampleDetail[i].AnalysisGroupDescription, obj.LabSampleDetail[i].AnalysisParameterDescription, obj.LabSampleDetail[i].PmLabStatus, obj.LabSampleDetail[i].InAnalysisResult, obj.LabSampleDetail[i].RemarksDetail);
				}
			}
			grdHistroryPurAnalysis.DataSource = dtPurHistory;
			grdHistroryPurAnalysis.RetrieveStructure();
			GrdHistoryDetailPurSetting();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdHistoryDetailPurSetting()
	{
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdHistroryPurAnalysis.AllowEdit = (InheritableBoolean)2;
			grdHistroryPurAnalysis.RootTable.Columns["Id"].Visible = false;
			grdHistroryPurAnalysis.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grdHistroryPurAnalysis.RootTable.Columns)
			{
				item.HeaderAlignment = (TextAlignment)2;
			}
			grdHistroryPurAnalysis.RootTable.Groups.Add("AnalysisGroup");
			grdHistroryPurAnalysis.HideColumnsWhenGrouped = (InheritableBoolean)1;
			grdHistroryPurAnalysis.RootTable.Columns["AnalysisParameter"].Width = Constants.LabConstants.AnalysisParameter;
			grdHistroryPurAnalysis.RootTable.Columns["ResultValue"].Width = Constants.LabConstants.ResultValue;
			grdHistroryPurAnalysis.RootTable.Columns["Remarks"].Width = 185;
			grdHistroryPurAnalysis.RootTable.Columns["ResultValue"].AggregateFunction = (AggregateFunction)2;
			grdHistroryPurAnalysis.RootTable.Columns["ResultValue"].FormatString = "#,#.##";
			grdHistroryPurAnalysis.RootTable.Columns["ResultValue"].TotalFormatString = "#,#.##";
			grdHistroryPurAnalysis.RootTable.Columns["ResultValue"].TextAlignment = (TextAlignment)3;
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

	private void btnprint_Click(object sender, EventArgs e)
	{
		try
		{
			LabAnalysisReport653(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LabAnalysisReport653(int RecId)
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
			obj.Id = RecId;
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

	private void btnSlip257_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.InwardGatePassWithWbAndLabSlip(Conversion.ToInt(cmbGatePassNo.Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnLoader_Click(object sender, EventArgs e)
	{
		try
		{
			LoadGp = new PendingGPForPurchaseLab(UserAccount);
			LoadGp.ShowDialog();
			LoadGp.BringToFront();
			if (LoadGp.GpId > 0)
			{
				cmbGatePassNo.Value = LoadGp.GpId;
				txtgatepassno_Leave(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtAddRate_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtLessRate_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtNoofBagsInspection_KeyPress(object sender, KeyPressEventArgs e)
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

	private void FromDocNoHistory_KeyPress(object sender, KeyPressEventArgs e)
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

	private void ToDocNoHistory_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtRateCut_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtQtyForWtCut_KeyPress(object sender, KeyPressEventArgs e)
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

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
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

	private void InvLabPurchaseAnalysis_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.KeyData == Keys.Return)
		{
			SendKeys.Send("{TAB}");
		}
		if (e.KeyCode == Keys.Delete && tabControl1.SelectedIndex == 1)
		{
			GridEXRow[] checkedRows = grd.GetCheckedRows();
			for (int i = 0; i < checkedRows.Length; i++)
			{
				int.Parse(checkedRows[i].Cells["Id"].Value.ToString());
			}
		}
		if (e.Control && e.KeyCode == Keys.S)
		{
			if (tabControl1.SelectedIndex == 0)
			{
				if (btnSave.Visible && btnSave.Enabled)
				{
					btnSave_Click(null, null);
				}
			}
			else if (tabControl1.SelectedIndex == 1)
			{
				btnShowHistory_Click(null, null);
			}
		}
		if (e.Control && e.KeyCode == Keys.N)
		{
			if (tabControl1.SelectedIndex == 0)
			{
				btnNew_Click(null, null);
			}
			else if (tabControl1.SelectedIndex == 1)
			{
				btnNewHistory_Click(null, null);
			}
		}
		if (e.Control && e.KeyCode == Keys.T)
		{
			if (tabControl1.SelectedIndex == 1)
			{
				tabControl1.SelectedIndex = 0;
				txtdocdate.Focus();
			}
			else
			{
				tabControl1.SelectedIndex = 1;
				FromDateHistory.Focus();
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
		if (btnprint.Visible && btnprint.Enabled && e.Control && e.KeyCode == Keys.P)
		{
			btnprint_Click(null, null);
		}
	}

	private void grdHistroryPurAnalysis_KeyDown(object sender, KeyEventArgs e)
	{
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			GpNoFill();
			GenerateCode();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PackingMaterialLabPurchaseAnalysis_FormClosing(object sender, FormClosingEventArgs e)
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
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Expected O, but got Unknown
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Expected O, but got Unknown
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Expected O, but got Unknown
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Expected O, but got Unknown
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Expected O, but got Unknown
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Expected O, but got Unknown
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Expected O, but got Unknown
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Expected O, but got Unknown
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Expected O, but got Unknown
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Expected O, but got Unknown
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Expected O, but got Unknown
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
		//IL_04a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b0: Expected O, but got Unknown
		//IL_04dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e7: Expected O, but got Unknown
		//IL_051f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0529: Expected O, but got Unknown
		//IL_0535: Unknown result type (might be due to invalid IL or missing references)
		//IL_053f: Expected O, but got Unknown
		//IL_054b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0555: Expected O, but got Unknown
		//IL_0577: Unknown result type (might be due to invalid IL or missing references)
		//IL_0581: Expected O, but got Unknown
		//IL_0582: Unknown result type (might be due to invalid IL or missing references)
		//IL_058c: Expected O, but got Unknown
		//IL_058d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0597: Expected O, but got Unknown
		//IL_065e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0668: Expected O, but got Unknown
		//IL_06e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ec: Expected O, but got Unknown
		//IL_06f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0702: Expected O, but got Unknown
		//IL_0745: Unknown result type (might be due to invalid IL or missing references)
		//IL_074f: Expected O, but got Unknown
		//IL_075b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0765: Expected O, but got Unknown
		//IL_0766: Unknown result type (might be due to invalid IL or missing references)
		//IL_0770: Expected O, but got Unknown
		//IL_1835: Unknown result type (might be due to invalid IL or missing references)
		//IL_183f: Expected O, but got Unknown
		//IL_1863: Unknown result type (might be due to invalid IL or missing references)
		//IL_186d: Expected O, but got Unknown
		//IL_42e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_42f1: Expected O, but got Unknown
		//IL_42fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_4308: Expected O, but got Unknown
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Lab.PackingMaterialLabPurchaseAnalysis));
		GridEXLayout grdHistroryPurAnalysis_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdhistory_DesignTimeLayout = new GridEXLayout();
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
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance57 = new Appearance();
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
		Appearance appearance68 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
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
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
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
		Appearance appearance1 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
		this.btnimg1 = new System.Windows.Forms.Button();
		this.btn = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.btnSlip257 = new System.Windows.Forms.ToolStripButton();
		this.btnprint = new System.Windows.Forms.ToolStripButton();
		this.btnattachment = new System.Windows.Forms.ToolStripButton();
		this.BtnLoader = new System.Windows.Forms.ToolStripButton();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.cmpcity = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.cmbsupplier = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.txtsampledate = new System.Windows.Forms.Label();
		this.tabHistory = new System.Windows.Forms.TabPage();
		this.panel13 = new System.Windows.Forms.Panel();
		this.panel14 = new System.Windows.Forms.Panel();
		this.panel17 = new System.Windows.Forms.Panel();
		this.grdHistroryPurAnalysis = new GridEX();
		this.panel16 = new System.Windows.Forms.Panel();
		this.label14 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel23 = new System.Windows.Forms.Panel();
		this.grdhistory = new GridEX();
		this.panel20 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.label46 = new System.Windows.Forms.Label();
		this.label45 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.CmbCustomerHistory = new UltraCombo();
		this.label53 = new System.Windows.Forms.Label();
		this.CmbGpNoHistory = new UltraCombo();
		this.label26 = new System.Windows.Forms.Label();
		this.CmbVehicleNoHistory = new UltraCombo();
		this.label25 = new System.Windows.Forms.Label();
		this.label24 = new System.Windows.Forms.Label();
		this.label23 = new System.Windows.Forms.Label();
		this.cmbApprovedStatusHistory = new UltraCombo();
		this.CmbAnalystStatusHistory = new UltraCombo();
		this.CmbItemHistory = new UltraCombo();
		this.btnShowHistory = new System.Windows.Forms.Button();
		this.label56 = new System.Windows.Forms.Label();
		this.label57 = new System.Windows.Forms.Label();
		this.ToDocNoHistory = new System.Windows.Forms.TextBox();
		this.FromDocNoHistory = new System.Windows.Forms.TextBox();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.panel21 = new System.Windows.Forms.Panel();
		this.label15 = new System.Windows.Forms.Label();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.txt = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabForm = new System.Windows.Forms.TabPage();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.grd = new GridEX();
		this.PanalParamGrdsForm = new System.Windows.Forms.Panel();
		this.panel9 = new System.Windows.Forms.Panel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.panel3 = new System.Windows.Forms.Panel();
		this.panel2 = new System.Windows.Forms.Panel();
		this.PaymentVoucherLable = new System.Windows.Forms.Label();
		this.PanelFeildForms = new System.Windows.Forms.Panel();
		this.txtbiltyno = new System.Windows.Forms.TextBox();
		this.datGpDate = new System.Windows.Forms.DateTimePicker();
		this.datBiltyDate = new System.Windows.Forms.DateTimePicker();
		this.txtdocdate = new System.Windows.Forms.DateTimePicker();
		this.cmbitem = new UltraCombo();
		this.txtremarks = new System.Windows.Forms.TextBox();
		this.cmbPartyName = new UltraCombo();
		this.label19 = new System.Windows.Forms.Label();
		this.txtScheduleId = new System.Windows.Forms.TextBox();
		this.txtApprovalStatus = new System.Windows.Forms.TextBox();
		this.label35 = new System.Windows.Forms.Label();
		this.txtApprovedBy = new System.Windows.Forms.TextBox();
		this.label34 = new System.Windows.Forms.Label();
		this.CmbLabStatus = new UltraCombo();
		this.txtPurchaseOrderId = new System.Windows.Forms.TextBox();
		this.cmbGatePassNo = new UltraCombo();
		this.cmbanalysisgroup = new UltraCombo();
		this.txtAnaylstName = new System.Windows.Forms.TextBox();
		this.label12 = new System.Windows.Forms.Label();
		this.txtSampleId = new System.Windows.Forms.TextBox();
		this.label18 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.txtNoofBagsInspection = new System.Windows.Forms.TextBox();
		this.txtvehicleno = new System.Windows.Forms.TextBox();
		this.txtCityArea = new System.Windows.Forms.TextBox();
		this.btnimage2 = new System.Windows.Forms.Button();
		this.txtOrderNo = new System.Windows.Forms.TextBox();
		this.label32 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label31 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.panel22 = new System.Windows.Forms.Panel();
		this.label21 = new System.Windows.Forms.Label();
		this.IsPrint = new System.Windows.Forms.CheckBox();
		this.chkPrintII = new System.Windows.Forms.CheckBox();
		this.btn.SuspendLayout();
		this.tabHistory.SuspendLayout();
		this.panel13.SuspendLayout();
		this.panel14.SuspendLayout();
		this.panel17.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistroryPurAnalysis).BeginInit();
		this.panel16.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel23.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdhistory).BeginInit();
		this.panel20.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbGpNoHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbVehicleNoHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbApprovedStatusHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAnalystStatusHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemHistory).BeginInit();
		this.panel21.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabForm.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.PanalParamGrdsForm.SuspendLayout();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		this.PanelFeildForms.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbitem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPartyName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbLabStatus).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbGatePassNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbanalysisgroup).BeginInit();
		this.panel22.SuspendLayout();
		base.SuspendLayout();
		this.toolStripSeparator.Name = "toolStripSeparator";
		this.toolStripSeparator.Size = new System.Drawing.Size(6, 27);
		this.btnimg1.Font = new System.Drawing.Font("Segoe UI", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnimg1.Location = new System.Drawing.Point(1313, 359);
		this.btnimg1.Name = "btnimg1";
		this.btnimg1.Size = new System.Drawing.Size(77, 27);
		this.btnimg1.TabIndex = 16;
		this.btnimg1.Text = "Browse";
		this.btnimg1.UseVisualStyleBackColor = true;
		this.btnimg1.Visible = false;
		this.btn.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btn.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.btn.Items.AddRange(new System.Windows.Forms.ToolStripItem[9] { this.btnNew, this.btnRefresh, this.btnUpdate, this.btnSave, this.btnSlip257, this.btnprint, this.btnattachment, this.BtnLoader, this.toolStripSeparator });
		this.btn.Location = new System.Drawing.Point(0, 0);
		this.btn.Name = "btn";
		this.btn.Size = new System.Drawing.Size(1113, 27);
		this.btn.TabIndex = 34;
		this.btn.Text = "toolStrip1";
		this.btnNew.Image = (System.Drawing.Image)resources.GetObject("btnNew.Image");
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(59, 24);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnUpdate.Image = (System.Drawing.Image)resources.GetObject("btnUpdate.Image");
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnSave.Image = (System.Drawing.Image)resources.GetObject("btnSave.Image");
		this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(64, 24);
		this.btnSave.Text = "&Save";
		this.btnSave.Click += new System.EventHandler(btnSave_Click);
		this.btnSlip257.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.btnSlip257.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnSlip257.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSlip257.Name = "btnSlip257";
		this.btnSlip257.Size = new System.Drawing.Size(102, 24);
		this.btnSlip257.Text = "257-GPSlip";
		this.btnSlip257.Click += new System.EventHandler(btnSlip257_Click);
		this.btnprint.Image = (System.Drawing.Image)resources.GetObject("btnprint.Image");
		this.btnprint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnprint.Name = "btnprint";
		this.btnprint.Size = new System.Drawing.Size(92, 24);
		this.btnprint.Text = "653-Print";
		this.btnprint.Click += new System.EventHandler(btnprint_Click);
		this.btnattachment.Image = (System.Drawing.Image)resources.GetObject("btnattachment.Image");
		this.btnattachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnattachment.Name = "btnattachment";
		this.btnattachment.Size = new System.Drawing.Size(109, 24);
		this.btnattachment.Text = "&Attachment";
		this.btnattachment.Click += new System.EventHandler(btnattachment_Click);
		this.BtnLoader.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnLoader.Name = "BtnLoader";
		this.BtnLoader.Size = new System.Drawing.Size(55, 24);
		this.BtnLoader.Text = "&Loader";
		this.BtnLoader.Click += new System.EventHandler(BtnLoader_Click);
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Enabled = false;
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocno.Location = new System.Drawing.Point(107, 4);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.Size = new System.Drawing.Size(100, 23);
		this.txtdocno.TabIndex = 0;
		this.cmpcity.AutoSize = true;
		this.cmpcity.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.cmpcity.Location = new System.Drawing.Point(7, 113);
		this.cmpcity.Name = "cmpcity";
		this.cmpcity.Size = new System.Drawing.Size(56, 15);
		this.cmpcity.TabIndex = 0;
		this.cmpcity.Text = "Order No";
		this.label6.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.ForeColor = System.Drawing.Color.Red;
		this.label6.Location = new System.Drawing.Point(7, 271);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(105, 30);
		this.label6.TabIndex = 13;
		this.label6.Text = "Status Accepted / Rejected";
		this.cmbsupplier.AutoSize = true;
		this.cmbsupplier.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbsupplier.Location = new System.Drawing.Point(7, 35);
		this.cmbsupplier.Name = "cmbsupplier";
		this.cmbsupplier.Size = new System.Drawing.Size(67, 15);
		this.cmbsupplier.TabIndex = 0;
		this.cmbsupplier.Text = "Gate Pass #";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(7, 197);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(86, 15);
		this.label10.TabIndex = 0;
		this.label10.Text = "Analysis Group";
		this.txtsampledate.AutoSize = true;
		this.txtsampledate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtsampledate.Location = new System.Drawing.Point(209, 8);
		this.txtsampledate.Name = "txtsampledate";
		this.txtsampledate.Size = new System.Drawing.Size(57, 15);
		this.txtsampledate.TabIndex = 0;
		this.txtsampledate.Text = "Doc Date";
		this.tabHistory.Controls.Add(this.panel13);
		this.tabHistory.Controls.Add(this.panel4);
		this.tabHistory.Location = new System.Drawing.Point(4, 4);
		this.tabHistory.Name = "tabHistory";
		this.tabHistory.Padding = new System.Windows.Forms.Padding(3);
		this.tabHistory.Size = new System.Drawing.Size(1119, 685);
		this.tabHistory.TabIndex = 1;
		this.tabHistory.Text = "History";
		this.tabHistory.UseVisualStyleBackColor = true;
		this.panel13.Controls.Add(this.panel14);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel13.Location = new System.Drawing.Point(3, 448);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1113, 234);
		this.panel13.TabIndex = 2;
		this.panel14.Controls.Add(this.panel17);
		this.panel14.Controls.Add(this.panel16);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel14.Location = new System.Drawing.Point(0, 0);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(1113, 234);
		this.panel14.TabIndex = 0;
		this.panel17.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistroryPurAnalysis);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel17.Location = new System.Drawing.Point(0, 32);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(1113, 202);
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
		((System.Windows.Forms.Control)(object)this.grdHistroryPurAnalysis).Size = new System.Drawing.Size(1113, 202);
		((System.Windows.Forms.Control)(object)this.grdHistroryPurAnalysis).TabIndex = 3;
		this.grdHistroryPurAnalysis.TotalRow = (InheritableBoolean)1;
		this.grdHistroryPurAnalysis.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistroryPurAnalysis).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdHistroryPurAnalysis).KeyDown += new System.Windows.Forms.KeyEventHandler(grdHistroryPurAnalysis_KeyDown);
		this.panel16.BackColor = System.Drawing.Color.Teal;
		this.panel16.Controls.Add(this.label14);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel16.Location = new System.Drawing.Point(0, 0);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(1113, 32);
		this.panel16.TabIndex = 0;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.ForeColor = System.Drawing.Color.White;
		this.label14.Location = new System.Drawing.Point(5, 4);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(257, 24);
		this.label14.TabIndex = 3;
		this.label14.Text = "Packing Material Lab Analysis";
		this.panel4.Controls.Add(this.panel23);
		this.panel4.Controls.Add(this.panel20);
		this.panel4.Controls.Add(this.panel21);
		this.panel4.Controls.Add(this.toolStrip1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(3, 3);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1113, 445);
		this.panel4.TabIndex = 1;
		this.panel23.Controls.Add((System.Windows.Forms.Control)(object)this.grdhistory);
		this.panel23.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel23.Location = new System.Drawing.Point(0, 185);
		this.panel23.Name = "panel23";
		this.panel23.Size = new System.Drawing.Size(1113, 260);
		this.panel23.TabIndex = 31;
		this.grdhistory.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdhistory).BackColor = System.Drawing.Color.White;
		grdhistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdhistory.DesignTimeLayout = grdhistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdhistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdhistory.FilterMode = (FilterMode)2;
		this.grdhistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdhistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdhistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdhistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdhistory).Name = "grdhistory";
		((ScrollableBase)this.grdhistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdhistory).Size = new System.Drawing.Size(1113, 260);
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
		this.panel20.Location = new System.Drawing.Point(0, 55);
		this.panel20.Name = "panel20";
		this.panel20.Size = new System.Drawing.Size(1113, 130);
		this.panel20.TabIndex = 30;
		this.groupBox2.Controls.Add(this.label46);
		this.groupBox2.Controls.Add(this.label45);
		this.groupBox2.Controls.Add(this.label27);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomerHistory);
		this.groupBox2.Controls.Add(this.label53);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbGpNoHistory);
		this.groupBox2.Controls.Add(this.label26);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbVehicleNoHistory);
		this.groupBox2.Controls.Add(this.label25);
		this.groupBox2.Controls.Add(this.label24);
		this.groupBox2.Controls.Add(this.label23);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbApprovedStatusHistory);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbAnalystStatusHistory);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemHistory);
		this.groupBox2.Controls.Add(this.btnShowHistory);
		this.groupBox2.Controls.Add(this.label56);
		this.groupBox2.Controls.Add(this.label57);
		this.groupBox2.Controls.Add(this.ToDocNoHistory);
		this.groupBox2.Controls.Add(this.FromDocNoHistory);
		this.groupBox2.Controls.Add(this.ToDateHistory);
		this.groupBox2.Controls.Add(this.FromDateHistory);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(5, 2);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(862, 124);
		this.groupBox2.TabIndex = 4;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Filters";
		this.label46.AutoSize = true;
		this.label46.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label46.Location = new System.Drawing.Point(7, 99);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(64, 15);
		this.label46.TabIndex = 99;
		this.label46.Text = "To Doc No";
		this.label45.AutoSize = true;
		this.label45.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label45.Location = new System.Drawing.Point(7, 45);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(48, 15);
		this.label45.TabIndex = 98;
		this.label45.Text = "To Date";
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(554, 18);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(41, 15);
		this.label27.TabIndex = 97;
		this.label27.Text = "GP No";
		this.CmbCustomerHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomerHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomerHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomerHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance69).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance70).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance70).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance70).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance71;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance72).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance72).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance72).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance73).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance73;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance74).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance75;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance76).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance77).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance77).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance77).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance77).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance77;
		((AppearanceBase)appearance78).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance79).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomerHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Location = new System.Drawing.Point(328, 12);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Name = "CmbCustomerHistory";
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Size = new System.Drawing.Size(218, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).TabIndex = 4;
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label53.Location = new System.Drawing.Point(229, 18);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(86, 15);
		this.label53.TabIndex = 87;
		this.label53.Text = "Supplier Name";
		this.CmbGpNoHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbGpNoHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbGpNoHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbGpNoHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance97).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbGpNoHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.CmbGpNoHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbGpNoHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance98).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance98).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance98).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbGpNoHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance98;
		((AppearanceBase)appearance99).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbGpNoHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance99;
		((SpecialBoxBase)((UltraGridBase)this.CmbGpNoHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance100).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbGpNoHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance100;
		((UltraGridBase)this.CmbGpNoHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbGpNoHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance102).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbGpNoHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance102;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance103).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbGpNoHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.CmbGpNoHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbGpNoHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbGpNoHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance106;
		((AppearanceBase)appearance107).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance107).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbGpNoHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.CmbGpNoHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbGpNoHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance108).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance108).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance108).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance108).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbGpNoHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance108;
		((AppearanceBase)appearance109).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbGpNoHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.CmbGpNoHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbGpNoHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance110).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbGpNoHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.CmbGpNoHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbGpNoHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.CmbGpNoHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbGpNoHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbGpNoHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbGpNoHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbGpNoHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbGpNoHistory).Location = new System.Drawing.Point(624, 12);
		((System.Windows.Forms.Control)(object)this.CmbGpNoHistory).Name = "CmbGpNoHistory";
		((System.Windows.Forms.Control)(object)this.CmbGpNoHistory).Size = new System.Drawing.Size(168, 26);
		((System.Windows.Forms.Control)(object)this.CmbGpNoHistory).TabIndex = 8;
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(554, 45);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(64, 15);
		this.label26.TabIndex = 95;
		this.label26.Text = "Vehicle No";
		this.CmbVehicleNoHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbVehicleNoHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbVehicleNoHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbVehicleNoHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance82).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbVehicleNoHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.CmbVehicleNoHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbVehicleNoHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance83).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance83).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance83).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbVehicleNoHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbVehicleNoHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance84;
		((SpecialBoxBase)((UltraGridBase)this.CmbVehicleNoHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance85).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance85).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance85).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbVehicleNoHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.CmbVehicleNoHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbVehicleNoHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance86).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbVehicleNoHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance86;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance87).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbVehicleNoHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.CmbVehicleNoHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbVehicleNoHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbVehicleNoHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance88;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance89).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbVehicleNoHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.CmbVehicleNoHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbVehicleNoHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance90).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance90).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance90).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance90).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbVehicleNoHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance90;
		((AppearanceBase)appearance91).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbVehicleNoHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.CmbVehicleNoHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbVehicleNoHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbVehicleNoHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.CmbVehicleNoHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbVehicleNoHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance93;
		((UltraGridBase)this.CmbVehicleNoHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbVehicleNoHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbVehicleNoHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbVehicleNoHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbVehicleNoHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbVehicleNoHistory).Location = new System.Drawing.Point(624, 39);
		((System.Windows.Forms.Control)(object)this.CmbVehicleNoHistory).Name = "CmbVehicleNoHistory";
		((System.Windows.Forms.Control)(object)this.CmbVehicleNoHistory).Size = new System.Drawing.Size(168, 26);
		((System.Windows.Forms.Control)(object)this.CmbVehicleNoHistory).TabIndex = 9;
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(229, 99);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(95, 15);
		this.label25.TabIndex = 93;
		this.label25.Text = "Approved Status";
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(229, 72);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(82, 15);
		this.label24.TabIndex = 92;
		this.label24.Text = "Analyst Status";
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.Location = new System.Drawing.Point(229, 45);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(67, 15);
		this.label23.TabIndex = 91;
		this.label23.Text = "Item Name";
		this.cmbApprovedStatusHistory.AllowNull = (DefaultableBoolean)1;
		this.cmbApprovedStatusHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbApprovedStatusHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbApprovedStatusHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance57).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbApprovedStatusHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.cmbApprovedStatusHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbApprovedStatusHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance58).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance58).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance58).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbApprovedStatusHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance58;
		((AppearanceBase)appearance59).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbApprovedStatusHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance59;
		((SpecialBoxBase)((UltraGridBase)this.cmbApprovedStatusHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance60).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance60).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance60).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbApprovedStatusHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.cmbApprovedStatusHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbApprovedStatusHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance61).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbApprovedStatusHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance62).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbApprovedStatusHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.cmbApprovedStatusHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbApprovedStatusHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbApprovedStatusHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance63;
		((AppearanceBase)appearance64).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance64).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbApprovedStatusHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.cmbApprovedStatusHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbApprovedStatusHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance65).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance65).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance65).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbApprovedStatusHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance65;
		((AppearanceBase)appearance66).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbApprovedStatusHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.cmbApprovedStatusHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbApprovedStatusHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance67).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbApprovedStatusHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.cmbApprovedStatusHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbApprovedStatusHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.cmbApprovedStatusHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbApprovedStatusHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbApprovedStatusHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbApprovedStatusHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbApprovedStatusHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbApprovedStatusHistory).Location = new System.Drawing.Point(328, 93);
		((System.Windows.Forms.Control)(object)this.cmbApprovedStatusHistory).Name = "cmbApprovedStatusHistory";
		((System.Windows.Forms.Control)(object)this.cmbApprovedStatusHistory).Size = new System.Drawing.Size(218, 26);
		((System.Windows.Forms.Control)(object)this.cmbApprovedStatusHistory).TabIndex = 7;
		this.CmbAnalystStatusHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbAnalystStatusHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbAnalystStatusHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbAnalystStatusHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbAnalystStatusHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.CmbAnalystStatusHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbAnalystStatusHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbAnalystStatusHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAnalystStatusHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance47;
		((SpecialBoxBase)((UltraGridBase)this.CmbAnalystStatusHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAnalystStatusHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.CmbAnalystStatusHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbAnalystStatusHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance49).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbAnalystStatusHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance50).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbAnalystStatusHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbAnalystStatusHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbAnalystStatusHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAnalystStatusHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance51;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance52).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbAnalystStatusHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.CmbAnalystStatusHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbAnalystStatusHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAnalystStatusHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbAnalystStatusHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.CmbAnalystStatusHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbAnalystStatusHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance55).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbAnalystStatusHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbAnalystStatusHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbAnalystStatusHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.CmbAnalystStatusHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbAnalystStatusHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbAnalystStatusHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbAnalystStatusHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbAnalystStatusHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbAnalystStatusHistory).Location = new System.Drawing.Point(328, 66);
		((System.Windows.Forms.Control)(object)this.CmbAnalystStatusHistory).Name = "CmbAnalystStatusHistory";
		((System.Windows.Forms.Control)(object)this.CmbAnalystStatusHistory).Size = new System.Drawing.Size(218, 26);
		((System.Windows.Forms.Control)(object)this.CmbAnalystStatusHistory).TabIndex = 6;
		this.CmbItemHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbItemHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance35;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance36).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance36).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance37;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance38).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance39;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance40).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance41).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance41).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance41).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemHistory).Location = new System.Drawing.Point(328, 39);
		((System.Windows.Forms.Control)(object)this.CmbItemHistory).Name = "CmbItemHistory";
		((System.Windows.Forms.Control)(object)this.CmbItemHistory).Size = new System.Drawing.Size(218, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemHistory).TabIndex = 5;
		this.btnShowHistory.BackColor = System.Drawing.Color.Teal;
		this.btnShowHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShowHistory.ForeColor = System.Drawing.Color.White;
		this.btnShowHistory.Location = new System.Drawing.Point(795, 39);
		this.btnShowHistory.Margin = new System.Windows.Forms.Padding(2);
		this.btnShowHistory.Name = "btnShowHistory";
		this.btnShowHistory.Size = new System.Drawing.Size(62, 26);
		this.btnShowHistory.TabIndex = 12;
		this.btnShowHistory.Text = "Show";
		this.btnShowHistory.UseVisualStyleBackColor = false;
		this.btnShowHistory.Click += new System.EventHandler(btnShowHistory_Click);
		this.label56.AutoSize = true;
		this.label56.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label56.Location = new System.Drawing.Point(7, 72);
		this.label56.Name = "label56";
		this.label56.Size = new System.Drawing.Size(79, 15);
		this.label56.TabIndex = 84;
		this.label56.Text = "From Doc No";
		this.label57.AutoSize = true;
		this.label57.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label57.Location = new System.Drawing.Point(7, 18);
		this.label57.Name = "label57";
		this.label57.Size = new System.Drawing.Size(63, 15);
		this.label57.TabIndex = 83;
		this.label57.Text = "From Date";
		this.ToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.ToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDocNoHistory.Location = new System.Drawing.Point(90, 95);
		this.ToDocNoHistory.Name = "ToDocNoHistory";
		this.ToDocNoHistory.ShortcutsEnabled = false;
		this.ToDocNoHistory.Size = new System.Drawing.Size(134, 23);
		this.ToDocNoHistory.TabIndex = 3;
		this.ToDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(ToDocNoHistory_KeyPress);
		this.FromDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.FromDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDocNoHistory.Location = new System.Drawing.Point(90, 68);
		this.FromDocNoHistory.Name = "FromDocNoHistory";
		this.FromDocNoHistory.ShortcutsEnabled = false;
		this.FromDocNoHistory.Size = new System.Drawing.Size(134, 23);
		this.FromDocNoHistory.TabIndex = 2;
		this.FromDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(FromDocNoHistory_KeyPress);
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(90, 41);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(134, 23);
		this.ToDateHistory.TabIndex = 1;
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(90, 14);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(134, 23);
		this.FromDateHistory.TabIndex = 0;
		this.panel21.BackColor = System.Drawing.Color.Teal;
		this.panel21.Controls.Add(this.label15);
		this.panel21.Controls.Add(this.ctrlGrdBar2);
		this.panel21.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel21.Location = new System.Drawing.Point(0, 27);
		this.panel21.Name = "panel21";
		this.panel21.Size = new System.Drawing.Size(1113, 28);
		this.panel21.TabIndex = 29;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.Color.White;
		this.label15.Location = new System.Drawing.Point(5, 4);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(67, 19);
		this.label15.TabIndex = 4;
		this.label15.Text = "History";
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1073, 2);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdhistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar2.TabIndex = 5;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnNewHistory, this.btnRefreshHistory });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1113, 27);
		this.toolStrip1.TabIndex = 28;
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
		this.txt.AutoSize = true;
		this.txt.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txt.Location = new System.Drawing.Point(209, 61);
		this.txt.Name = "txt";
		this.txt.Size = new System.Drawing.Size(49, 15);
		this.txt.TabIndex = 0;
		this.txt.Text = "Bilty No";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(7, 168);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(32, 15);
		this.label8.TabIndex = 0;
		this.label8.Text = "Item";
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabForm);
		this.tabControl1.Controls.Add(this.tabHistory);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1127, 711);
		this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabForm.BackColor = System.Drawing.Color.White;
		this.tabForm.Controls.Add(this.ctrlGrdBar1);
		this.tabForm.Controls.Add(this.PanalParamGrdsForm);
		this.tabForm.Controls.Add(this.PanelFeildForms);
		this.tabForm.Controls.Add(this.panel22);
		this.tabForm.Location = new System.Drawing.Point(4, 4);
		this.tabForm.Name = "tabForm";
		this.tabForm.Padding = new System.Windows.Forms.Padding(3);
		this.tabForm.Size = new System.Drawing.Size(1119, 685);
		this.tabForm.TabIndex = 0;
		this.tabForm.Text = "Form";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = null;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1168, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 26);
		this.ctrlGrdBar1.TabIndex = 35;
		this.grd.AllowDelete = (InheritableBoolean)1;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 25);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(713, 604);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.FormattingRow += new RowLoadEventHandler(grd_FormattingRow);
		this.grd.CellUpdated += new ColumnActionEventHandler(grd_CellUpdated);
		this.PanalParamGrdsForm.AutoSize = true;
		this.PanalParamGrdsForm.Controls.Add(this.panel9);
		this.PanalParamGrdsForm.Controls.Add(this.panel5);
		this.PanalParamGrdsForm.Controls.Add(this.panel3);
		this.PanalParamGrdsForm.Dock = System.Windows.Forms.DockStyle.Fill;
		this.PanalParamGrdsForm.Location = new System.Drawing.Point(405, 53);
		this.PanalParamGrdsForm.Name = "PanalParamGrdsForm";
		this.PanalParamGrdsForm.Size = new System.Drawing.Size(711, 629);
		this.PanalParamGrdsForm.TabIndex = 38;
		this.panel9.AutoSize = true;
		this.panel9.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel9.Location = new System.Drawing.Point(713, 0);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(0, 629);
		this.panel9.TabIndex = 4;
		this.panel5.AutoSize = true;
		this.panel5.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel5.Location = new System.Drawing.Point(713, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(0, 629);
		this.panel5.TabIndex = 3;
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel3.Controls.Add(this.panel2);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(713, 629);
		this.panel3.TabIndex = 2;
		this.panel2.BackColor = System.Drawing.Color.White;
		this.panel2.Controls.Add(this.PaymentVoucherLable);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(713, 25);
		this.panel2.TabIndex = 2;
		this.PaymentVoucherLable.AutoSize = true;
		this.PaymentVoucherLable.Font = new System.Drawing.Font("Tahoma", 11f, System.Drawing.FontStyle.Bold);
		this.PaymentVoucherLable.ForeColor = System.Drawing.Color.Teal;
		this.PaymentVoucherLable.Location = new System.Drawing.Point(5, 2);
		this.PaymentVoucherLable.Name = "PaymentVoucherLable";
		this.PaymentVoucherLable.Size = new System.Drawing.Size(226, 18);
		this.PaymentVoucherLable.TabIndex = 1;
		this.PaymentVoucherLable.Text = "Analysis Parameters' Results";
		this.PanelFeildForms.Controls.Add(this.txtbiltyno);
		this.PanelFeildForms.Controls.Add(this.datGpDate);
		this.PanelFeildForms.Controls.Add(this.datBiltyDate);
		this.PanelFeildForms.Controls.Add(this.txtdocdate);
		this.PanelFeildForms.Controls.Add((System.Windows.Forms.Control)(object)this.cmbitem);
		this.PanelFeildForms.Controls.Add(this.txtremarks);
		this.PanelFeildForms.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPartyName);
		this.PanelFeildForms.Controls.Add(this.label19);
		this.PanelFeildForms.Controls.Add(this.txtScheduleId);
		this.PanelFeildForms.Controls.Add(this.txtApprovalStatus);
		this.PanelFeildForms.Controls.Add(this.label35);
		this.PanelFeildForms.Controls.Add(this.txtApprovedBy);
		this.PanelFeildForms.Controls.Add(this.label34);
		this.PanelFeildForms.Controls.Add((System.Windows.Forms.Control)(object)this.CmbLabStatus);
		this.PanelFeildForms.Controls.Add(this.txtPurchaseOrderId);
		this.PanelFeildForms.Controls.Add((System.Windows.Forms.Control)(object)this.cmbGatePassNo);
		this.PanelFeildForms.Controls.Add((System.Windows.Forms.Control)(object)this.cmbanalysisgroup);
		this.PanelFeildForms.Controls.Add(this.txtAnaylstName);
		this.PanelFeildForms.Controls.Add(this.label12);
		this.PanelFeildForms.Controls.Add(this.txtSampleId);
		this.PanelFeildForms.Controls.Add(this.label18);
		this.PanelFeildForms.Controls.Add(this.label4);
		this.PanelFeildForms.Controls.Add(this.txtNoofBagsInspection);
		this.PanelFeildForms.Controls.Add(this.txtvehicleno);
		this.PanelFeildForms.Controls.Add(this.txtCityArea);
		this.PanelFeildForms.Controls.Add(this.btnimage2);
		this.PanelFeildForms.Controls.Add(this.btnimg1);
		this.PanelFeildForms.Controls.Add(this.txtOrderNo);
		this.PanelFeildForms.Controls.Add(this.txtdocno);
		this.PanelFeildForms.Controls.Add(this.cmpcity);
		this.PanelFeildForms.Controls.Add(this.label6);
		this.PanelFeildForms.Controls.Add(this.label32);
		this.PanelFeildForms.Controls.Add(this.txt);
		this.PanelFeildForms.Controls.Add(this.cmbsupplier);
		this.PanelFeildForms.Controls.Add(this.label10);
		this.PanelFeildForms.Controls.Add(this.label2);
		this.PanelFeildForms.Controls.Add(this.label31);
		this.PanelFeildForms.Controls.Add(this.txtsampledate);
		this.PanelFeildForms.Controls.Add(this.label8);
		this.PanelFeildForms.Controls.Add(this.label7);
		this.PanelFeildForms.Controls.Add(this.label1);
		this.PanelFeildForms.Dock = System.Windows.Forms.DockStyle.Left;
		this.PanelFeildForms.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.PanelFeildForms.Location = new System.Drawing.Point(3, 53);
		this.PanelFeildForms.Margin = new System.Windows.Forms.Padding(2);
		this.PanelFeildForms.Name = "PanelFeildForms";
		this.PanelFeildForms.Size = new System.Drawing.Size(402, 629);
		this.PanelFeildForms.TabIndex = 0;
		this.txtbiltyno.BackColor = System.Drawing.SystemColors.Control;
		this.txtbiltyno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtbiltyno.Enabled = false;
		this.txtbiltyno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtbiltyno.Location = new System.Drawing.Point(267, 57);
		this.txtbiltyno.Name = "txtbiltyno";
		this.txtbiltyno.Size = new System.Drawing.Size(130, 23);
		this.txtbiltyno.TabIndex = 5;
		this.datGpDate.CustomFormat = "dd-MM-yy";
		this.datGpDate.Enabled = false;
		this.datGpDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datGpDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datGpDate.Location = new System.Drawing.Point(267, 32);
		this.datGpDate.Name = "datGpDate";
		this.datGpDate.Size = new System.Drawing.Size(130, 23);
		this.datGpDate.TabIndex = 3;
		this.datBiltyDate.CustomFormat = "dd-MM-yy";
		this.datBiltyDate.Enabled = false;
		this.datBiltyDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datBiltyDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datBiltyDate.Location = new System.Drawing.Point(267, 82);
		this.datBiltyDate.Name = "datBiltyDate";
		this.datBiltyDate.Size = new System.Drawing.Size(130, 23);
		this.datBiltyDate.TabIndex = 7;
		this.txtdocdate.CustomFormat = "dd-MM-yy";
		this.txtdocdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtdocdate.Location = new System.Drawing.Point(267, 4);
		this.txtdocdate.Name = "txtdocdate";
		this.txtdocdate.Size = new System.Drawing.Size(130, 23);
		this.txtdocdate.TabIndex = 1;
		this.cmbitem.AllowNull = (DefaultableBoolean)1;
		this.cmbitem.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbitem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbitem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbitem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.cmbitem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbitem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance131).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance131).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance131).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbitem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance131;
		((AppearanceBase)appearance132).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbitem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance132;
		((SpecialBoxBase)((UltraGridBase)this.cmbitem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbitem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbitem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbitem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.cmbitem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbitem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbitem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbitem).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbitem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbitem).Location = new System.Drawing.Point(107, 162);
		((System.Windows.Forms.Control)(object)this.cmbitem).Name = "cmbitem";
		((System.Windows.Forms.Control)(object)this.cmbitem).Size = new System.Drawing.Size(290, 26);
		((System.Windows.Forms.Control)(object)this.cmbitem).TabIndex = 10;
		this.txtremarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtremarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtremarks.Location = new System.Drawing.Point(107, 356);
		this.txtremarks.Multiline = true;
		this.txtremarks.Name = "txtremarks";
		this.txtremarks.Size = new System.Drawing.Size(290, 62);
		this.txtremarks.TabIndex = 17;
		this.cmbPartyName.AllowNull = (DefaultableBoolean)1;
		this.cmbPartyName.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPartyName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPartyName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance116).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPartyName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.cmbPartyName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPartyName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance117).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance117).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPartyName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance117;
		((AppearanceBase)appearance118).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPartyName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance118;
		((SpecialBoxBase)((UltraGridBase)this.cmbPartyName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance119).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance119).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance119).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPartyName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.cmbPartyName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPartyName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance120).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPartyName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance121).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPartyName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.cmbPartyName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPartyName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPartyName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance122;
		((AppearanceBase)appearance124).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance124).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPartyName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.cmbPartyName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPartyName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance125).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance125).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance125).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance125).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPartyName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance125;
		((AppearanceBase)appearance126).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPartyName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.cmbPartyName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPartyName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPartyName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.cmbPartyName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPartyName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.cmbPartyName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPartyName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPartyName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPartyName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbPartyName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPartyName).Location = new System.Drawing.Point(107, 134);
		((System.Windows.Forms.Control)(object)this.cmbPartyName).Name = "cmbPartyName";
		((System.Windows.Forms.Control)(object)this.cmbPartyName).Size = new System.Drawing.Size(290, 26);
		((System.Windows.Forms.Control)(object)this.cmbPartyName).TabIndex = 9;
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(7, 140);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(69, 15);
		this.label19.TabIndex = 57;
		this.label19.Text = "Party Name";
		this.txtScheduleId.Location = new System.Drawing.Point(1496, 361);
		this.txtScheduleId.Name = "txtScheduleId";
		this.txtScheduleId.Size = new System.Drawing.Size(26, 25);
		this.txtScheduleId.TabIndex = 53;
		this.txtScheduleId.Visible = false;
		this.txtApprovalStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtApprovalStatus.Enabled = false;
		this.txtApprovalStatus.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtApprovalStatus.ForeColor = System.Drawing.Color.Red;
		this.txtApprovalStatus.Location = new System.Drawing.Point(107, 327);
		this.txtApprovalStatus.Name = "txtApprovalStatus";
		this.txtApprovalStatus.Size = new System.Drawing.Size(290, 23);
		this.txtApprovalStatus.TabIndex = 16;
		this.txtApprovalStatus.TabStop = false;
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.ForeColor = System.Drawing.Color.Red;
		this.label35.Location = new System.Drawing.Point(7, 331);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(91, 15);
		this.label35.TabIndex = 52;
		this.label35.Text = "Approval Status";
		this.txtApprovedBy.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtApprovedBy.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtApprovedBy.Location = new System.Drawing.Point(107, 303);
		this.txtApprovedBy.Name = "txtApprovedBy";
		this.txtApprovedBy.ReadOnly = true;
		this.txtApprovedBy.Size = new System.Drawing.Size(290, 23);
		this.txtApprovedBy.TabIndex = 15;
		this.txtApprovedBy.TabStop = false;
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.Location = new System.Drawing.Point(7, 307);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(75, 15);
		this.label34.TabIndex = 52;
		this.label34.Text = "Approved By";
		this.CmbLabStatus.AllowNull = (DefaultableBoolean)1;
		this.CmbLabStatus.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbLabStatus.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbLabStatus.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbLabStatus).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbLabStatus).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbLabStatus).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbLabStatus).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLabStatus).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.CmbLabStatus).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLabStatus).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbLabStatus).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbLabStatus).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbLabStatus).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbLabStatus).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbLabStatus).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbLabStatus).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLabStatus).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbLabStatus).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbLabStatus).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbLabStatus).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLabStatus).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbLabStatus).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbLabStatus).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbLabStatus).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbLabStatus).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbLabStatus).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbLabStatus).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbLabStatus).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbLabStatus).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbLabStatus).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbLabStatus).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbLabStatus.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbLabStatus).Location = new System.Drawing.Point(107, 274);
		((System.Windows.Forms.Control)(object)this.CmbLabStatus).Name = "CmbLabStatus";
		((System.Windows.Forms.Control)(object)this.CmbLabStatus).Size = new System.Drawing.Size(290, 26);
		((System.Windows.Forms.Control)(object)this.CmbLabStatus).TabIndex = 14;
		this.txtPurchaseOrderId.Location = new System.Drawing.Point(1468, 332);
		this.txtPurchaseOrderId.Margin = new System.Windows.Forms.Padding(2);
		this.txtPurchaseOrderId.Name = "txtPurchaseOrderId";
		this.txtPurchaseOrderId.Size = new System.Drawing.Size(26, 25);
		this.txtPurchaseOrderId.TabIndex = 42;
		this.txtPurchaseOrderId.Visible = false;
		this.cmbGatePassNo.AllowNull = (DefaultableBoolean)1;
		this.cmbGatePassNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbGatePassNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbGatePassNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbGatePassNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbGatePassNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbGatePassNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance81).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance81).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbGatePassNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance81;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbGatePassNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance94;
		((SpecialBoxBase)((UltraGridBase)this.cmbGatePassNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance95).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance95).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbGatePassNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.cmbGatePassNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbGatePassNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance96).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbGatePassNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance101).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbGatePassNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.cmbGatePassNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbGatePassNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbGatePassNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance123).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance123).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbGatePassNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.cmbGatePassNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbGatePassNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance129).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance129).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance129).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance129).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbGatePassNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance129;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbGatePassNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbGatePassNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbGatePassNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbGatePassNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbGatePassNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbGatePassNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbGatePassNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbGatePassNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbGatePassNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbGatePassNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbGatePassNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbGatePassNo).Location = new System.Drawing.Point(107, 29);
		((System.Windows.Forms.Control)(object)this.cmbGatePassNo).Name = "cmbGatePassNo";
		((System.Windows.Forms.Control)(object)this.cmbGatePassNo).Size = new System.Drawing.Size(100, 26);
		((System.Windows.Forms.Control)(object)this.cmbGatePassNo).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.cmbGatePassNo).Leave += new System.EventHandler(txtgatepassno_Leave);
		this.cmbanalysisgroup.AllowNull = (DefaultableBoolean)1;
		this.cmbanalysisgroup.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbanalysisgroup.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbanalysisgroup.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbanalysisgroup).DisplayLayout.Appearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbanalysisgroup).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbanalysisgroup).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance104).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance104).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance104).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbanalysisgroup).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance104;
		((AppearanceBase)appearance105).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbanalysisgroup).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance105;
		((SpecialBoxBase)((UltraGridBase)this.cmbanalysisgroup).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance30).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance30).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbanalysisgroup).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.cmbanalysisgroup).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbanalysisgroup).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbanalysisgroup).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbanalysisgroup).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.cmbanalysisgroup).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbanalysisgroup).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbanalysisgroup).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance27).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbanalysisgroup).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.cmbanalysisgroup).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbanalysisgroup).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance28).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance28).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance28).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbanalysisgroup).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance28;
		((AppearanceBase)appearance113).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbanalysisgroup).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.cmbanalysisgroup).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbanalysisgroup).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance114).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbanalysisgroup).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.cmbanalysisgroup).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbanalysisgroup).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.cmbanalysisgroup).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbanalysisgroup).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbanalysisgroup).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbanalysisgroup).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbanalysisgroup.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbanalysisgroup).Location = new System.Drawing.Point(107, 191);
		((System.Windows.Forms.Control)(object)this.cmbanalysisgroup).Name = "cmbanalysisgroup";
		((System.Windows.Forms.Control)(object)this.cmbanalysisgroup).Size = new System.Drawing.Size(290, 26);
		((System.Windows.Forms.Control)(object)this.cmbanalysisgroup).TabIndex = 11;
		((System.Windows.Forms.Control)(object)this.cmbanalysisgroup).Leave += new System.EventHandler(cmbanalysisgroup_Leave);
		this.txtAnaylstName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAnaylstName.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAnaylstName.Location = new System.Drawing.Point(107, 221);
		this.txtAnaylstName.Name = "txtAnaylstName";
		this.txtAnaylstName.Size = new System.Drawing.Size(290, 23);
		this.txtAnaylstName.TabIndex = 12;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(7, 225);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(78, 15);
		this.label12.TabIndex = 38;
		this.label12.Text = "AnaylstName";
		this.txtSampleId.Location = new System.Drawing.Point(1468, 361);
		this.txtSampleId.Margin = new System.Windows.Forms.Padding(2);
		this.txtSampleId.Name = "txtSampleId";
		this.txtSampleId.Size = new System.Drawing.Size(26, 25);
		this.txtSampleId.TabIndex = 35;
		this.txtSampleId.Visible = false;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(7, 86);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(72, 15);
		this.label18.TabIndex = 32;
		this.label18.Text = "Analysis Qty";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(7, 61);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(64, 15);
		this.label4.TabIndex = 32;
		this.label4.Text = "Vehicle No";
		this.txtNoofBagsInspection.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNoofBagsInspection.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNoofBagsInspection.Location = new System.Drawing.Point(107, 82);
		this.txtNoofBagsInspection.Name = "txtNoofBagsInspection";
		this.txtNoofBagsInspection.Size = new System.Drawing.Size(100, 23);
		this.txtNoofBagsInspection.TabIndex = 6;
		this.txtNoofBagsInspection.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtNoofBagsInspection_KeyPress);
		this.txtvehicleno.BackColor = System.Drawing.SystemColors.Control;
		this.txtvehicleno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtvehicleno.Enabled = false;
		this.txtvehicleno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtvehicleno.Location = new System.Drawing.Point(107, 57);
		this.txtvehicleno.Name = "txtvehicleno";
		this.txtvehicleno.Size = new System.Drawing.Size(100, 23);
		this.txtvehicleno.TabIndex = 4;
		this.txtCityArea.BackColor = System.Drawing.SystemColors.Control;
		this.txtCityArea.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCityArea.Enabled = false;
		this.txtCityArea.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCityArea.Location = new System.Drawing.Point(107, 246);
		this.txtCityArea.Name = "txtCityArea";
		this.txtCityArea.Size = new System.Drawing.Size(290, 23);
		this.txtCityArea.TabIndex = 13;
		this.btnimage2.Font = new System.Drawing.Font("Segoe UI", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnimage2.Location = new System.Drawing.Point(1390, 359);
		this.btnimage2.Name = "btnimage2";
		this.btnimage2.Size = new System.Drawing.Size(75, 27);
		this.btnimage2.TabIndex = 18;
		this.btnimage2.Text = "Browse";
		this.btnimage2.UseVisualStyleBackColor = true;
		this.btnimage2.Visible = false;
		this.txtOrderNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOrderNo.Enabled = false;
		this.txtOrderNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOrderNo.Location = new System.Drawing.Point(107, 109);
		this.txtOrderNo.Name = "txtOrderNo";
		this.txtOrderNo.Size = new System.Drawing.Size(100, 23);
		this.txtOrderNo.TabIndex = 8;
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.Location = new System.Drawing.Point(7, 250);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(54, 15);
		this.label32.TabIndex = 0;
		this.label32.Text = "City Area";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(208, 36);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(50, 15);
		this.label2.TabIndex = 0;
		this.label2.Text = "Gp Date";
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.Location = new System.Drawing.Point(209, 86);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(58, 15);
		this.label31.TabIndex = 0;
		this.label31.Text = "Bilty Date";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(6, 364);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(60, 17);
		this.label7.TabIndex = 0;
		this.label7.Text = "Remarks";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(7, 8);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(83, 15);
		this.label1.TabIndex = 0;
		this.label1.Text = "Document No";
		this.panel22.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel22.Controls.Add(this.label21);
		this.panel22.Controls.Add(this.IsPrint);
		this.panel22.Controls.Add(this.chkPrintII);
		this.panel22.Controls.Add(this.btn);
		this.panel22.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel22.Location = new System.Drawing.Point(3, 3);
		this.panel22.Name = "panel22";
		this.panel22.Size = new System.Drawing.Size(1113, 50);
		this.panel22.TabIndex = 58;
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.ForeColor = System.Drawing.Color.White;
		this.label21.Location = new System.Drawing.Point(5, 29);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(252, 19);
		this.label21.TabIndex = 1;
		this.label21.Text = "Packing Material Lab Analysis";
		this.IsPrint.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.IsPrint.AutoSize = true;
		this.IsPrint.Checked = true;
		this.IsPrint.CheckState = System.Windows.Forms.CheckState.Checked;
		this.IsPrint.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.IsPrint.ForeColor = System.Drawing.Color.White;
		this.IsPrint.Location = new System.Drawing.Point(1036, 30);
		this.IsPrint.Name = "IsPrint";
		this.IsPrint.Size = new System.Drawing.Size(75, 18);
		this.IsPrint.TabIndex = 37;
		this.IsPrint.Text = "Peview";
		this.IsPrint.UseVisualStyleBackColor = true;
		this.chkPrintII.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.chkPrintII.AutoSize = true;
		this.chkPrintII.Checked = true;
		this.chkPrintII.CheckState = System.Windows.Forms.CheckState.Checked;
		this.chkPrintII.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkPrintII.ForeColor = System.Drawing.Color.White;
		this.chkPrintII.Location = new System.Drawing.Point(923, 30);
		this.chkPrintII.Name = "chkPrintII";
		this.chkPrintII.Size = new System.Drawing.Size(106, 18);
		this.chkPrintII.TabIndex = 38;
		this.chkPrintII.Text = "257 Peview";
		this.chkPrintII.UseVisualStyleBackColor = true;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1127, 711);
		base.Controls.Add(this.tabControl1);
		base.KeyPreview = true;
		base.Name = "PackingMaterialLabPurchaseAnalysis";
		this.Text = "Packing Material Lab Analysis";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(PackingMaterialLabPurchaseAnalysis_FormClosing);
		base.Load += new System.EventHandler(PackingMaterialLabPurchaseAnalysis_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(InvLabPurchaseAnalysis_KeyDown);
		this.btn.ResumeLayout(false);
		this.btn.PerformLayout();
		this.tabHistory.ResumeLayout(false);
		this.panel13.ResumeLayout(false);
		this.panel14.ResumeLayout(false);
		this.panel17.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistroryPurAnalysis).EndInit();
		this.panel16.ResumeLayout(false);
		this.panel16.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.panel23.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdhistory).EndInit();
		this.panel20.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbGpNoHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbVehicleNoHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbApprovedStatusHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAnalystStatusHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemHistory).EndInit();
		this.panel21.ResumeLayout(false);
		this.panel21.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabControl1.ResumeLayout(false);
		this.tabForm.ResumeLayout(false);
		this.tabForm.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.PanalParamGrdsForm.ResumeLayout(false);
		this.PanalParamGrdsForm.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.PanelFeildForms.ResumeLayout(false);
		this.PanelFeildForms.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbitem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPartyName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbLabStatus).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbGatePassNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbanalysisgroup).EndInit();
		this.panel22.ResumeLayout(false);
		this.panel22.PerformLayout();
		base.ResumeLayout(false);
	}
}
