using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.ContractorWages;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.ContractorWages;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Infragistics.Shared;
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
namespace Architecture.WinApp.ApprovalDashboard
{

public class PendingApprovalLabourWages : Form
{
	public int DocumentTypeId;

	private bool EnableAddLessOnWagesRegular;

	private DataTable dtHistory = new DataTable();

	public DateTime RequestedFromDate;

	public DateTime RequestedToDate;

	private DropDownBind DDL = new DropDownBind();

	public UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel1;

	private ToolStrip toolStrip3;

	private ToolStripButton btnNew;

	private Panel panel3;

	private Label lblHeading;

	private Panel panel4;

	private Button btnApproved;

	private Button btnshow;

	private DateTimePicker datToDate;

	private DateTimePicker datFromDate;

	private Label label27;

	private Label label33;

	private Panel panel2;

	private Panel panel5;

	private Label label4;

	private Panel panel6;

	private Panel panel7;

	private Label label5;

	private CtrlGrdBar ctrlGrdBar2;

	private CtrlGrdBar ctrlGrdBar1;

	internal GridEX DataGridMain;

	internal GridEX DataGridDetail;

	private ToolStripButton btnRefresh;

	private Label label7;

	internal UltraCombo cmbDocumentType;

	internal UltraCombo CmbBranchName;

	private Label label8;

	public PendingApprovalLabourWages(UserAccount Puser)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = Puser;
	}

	private void PendingApprovalStockTransfer_Load(object sender, EventArgs e)
	{
		try
		{
			if (DocumentTypeId == 101)
			{
				lblHeading.Text = "Labour Wages Approval";
			}
			else if (DocumentTypeId == 810)
			{
				lblHeading.Text = "Labour Wages Manual Approval";
			}
			EnableAddLessOnWagesRegular = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("EnableAddLessOnWagesRegular"));
			if (!Conversion.CheckDateTimeNull(RequestedFromDate) && !Conversion.CheckDateTimeNull(RequestedToDate))
			{
				datFromDate.Value = Convert.ToDateTime(RequestedFromDate);
				datToDate.Value = Convert.ToDateTime(RequestedToDate);
			}
			else
			{
				datFromDate.Value = DateTime.Now.AddDays(-7.0);
				datToDate.Value = DateTime.Now;
			}
			BranchesFill();
			ComboFill();
			GetPendingRecords();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BranchesFill()
	{
		try
		{
			((UltraGridBase)CmbBranchName).DataSource = null;
			DataTable dtBranch = BranchesAllocationToUser.GetBranchsAllocatedToUserFromWages(UserAccount.OrganizationId, UserAccount.CompanyId, UserAccount.ID, 0);
			if (dtBranch.Rows.Count > 0)
			{
				DDL.BindDDL(dtBranch, CmbBranchName, "BranchId", "BranchName", "Branch Name", ZeroIndex: false);
				UltraGridColumn obj = ((UltraGridBase)CmbBranchName).DisplayLayout.Bands[0].Columns.Add();
				((KeyedSubObjectBase)obj).Key = "Selected";
				((HeaderBase)obj.Header).Caption = string.Empty;
				obj.Header.CheckBoxVisibility = (HeaderCheckBoxVisibility)2;
				obj.DataType = typeof(bool);
				((HeaderBase)obj.Header).VisiblePosition = 0;
				CmbBranchName.CheckedListSettings.CheckStateMember = "Selected";
				((EditorCheckedListSettings)CmbBranchName.CheckedListSettings).EditorValueSource = (EditorWithComboValueSource)1;
				((EditorCheckedListSettings)CmbBranchName.CheckedListSettings).ListSeparator = ",";
				((EditorCheckedListSettings)CmbBranchName.CheckedListSettings).ItemCheckArea = (ItemCheckArea)1;
				((UltraGridBase)CmbBranchName).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ComboFill()
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		try
		{
			DataTable dtRefDocumentType = new DataTable();
			dtRefDocumentType.Columns.Add("Id");
			dtRefDocumentType.Columns.Add("name");
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.DocumentTypeIds = DocumentTypeId.ToString();
			obj.Activity = "RefDocumentType";
			string BranchIds = "";
			if (Conversion.ToString((object)((Control)(object)CmbBranchName).Text) != string.Empty)
			{
				string[] array = (Conversion.ToString((object)((Control)(object)CmbBranchName).Text) + ",").Split(',');
				foreach (string author in array)
				{
					DataRow[] dr = ((DataTable)((UltraGridBase)CmbBranchName).DataSource).Select("BranchName='" + Conversion.ToString((object)author) + "'");
					if (dr.Length != 0)
					{
						BranchIds = BranchIds + "," + Conversion.ToString(dr[0].ItemArray[0]);
					}
				}
				obj.BranchesIds = BranchIds;
			}
			DataTable dtBindCombo = InvContractorWagesBillHeader.ComboAgainstContractorWages(obj);
			if (dtBindCombo.Rows.Count > 0)
			{
				for (int i = 0; i < dtBindCombo.Rows.Count; i++)
				{
					dtRefDocumentType.Rows.Add(dtBindCombo.Rows[i]["Id"], dtBindCombo.Rows[i]["ReferenceName"]);
				}
				DDL.BindDDL(dtRefDocumentType, cmbDocumentType, "Id", "name", "Document Type", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)DataGridMain).Name);
			if (fs != null)
			{
				DataGridMain.LoadLayoutFile((Stream)fs);
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)DataGridDetail).Name);
			if (fs != null)
			{
				DataGridDetail.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetPendingRecords()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		try
		{
			dtHistory.Rows.Clear();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.DocumentTypeId = DocumentTypeId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.FromDate = datFromDate.Value;
			obj.ToDate = datToDate.Value;
			string BranchIds = "";
			if (Conversion.ToString((object)((Control)(object)CmbBranchName).Text) != string.Empty)
			{
				string[] array = (Conversion.ToString((object)((Control)(object)CmbBranchName).Text) + ",").Split(',');
				foreach (string author in array)
				{
					DataRow[] dr = ((DataTable)((UltraGridBase)CmbBranchName).DataSource).Select("BranchName='" + Conversion.ToString((object)author) + "'");
					if (dr.Length != 0)
					{
						BranchIds = BranchIds + "," + Conversion.ToString(dr[0].ItemArray[0]);
					}
				}
				obj.BranchesIds = BranchIds;
			}
			obj.RefDocumentTypeId = Conversion.ToInt(cmbDocumentType.Value);
			dtHistory = InvContractorWagesBillHeader.GetContractorWagesDataForApproval(obj);
			if (dtHistory.Rows.Count > 0)
			{
				DataTable dtMain = new DataTable();
				dtMain.Columns.Add("Id", typeof(int));
				dtMain.Columns.Add("DocumentTypeId", typeof(int));
				dtMain.Columns.Add("RefDocumentTypeId", typeof(int));
				dtMain.Columns.Add("DocumentType", typeof(string));
				dtMain.Columns.Add("DocNo", typeof(int));
				dtMain.Columns.Add("DocDate", typeof(DateTime));
				dtMain.Columns.Add("RefDocNoId", typeof(int));
				dtMain.Columns.Add("RefDocNo", typeof(int));
				dtMain.Columns.Add("TotalQty", typeof(double));
				dtMain.Columns.Add("TotalWeight", typeof(double));
				dtMain.Columns.Add("TotalAmount", typeof(double));
				dtMain.Columns.Add("JobOrderNo", typeof(string));
				dtMain.Columns.Add("EntryUser");
				dtMain.Columns.Add("EntryDate", typeof(DateTime));
				dtMain.Columns.Add("ModifyUser");
				dtMain.Columns.Add("ModifyDate", typeof(DateTime));
				dtMain.Clear();
				for (int i = 0; i < dtHistory.Rows.Count; i++)
				{
					dtMain.Rows.Add(dtHistory.Rows[i]["Id"], dtHistory.Rows[i]["DocumentTypeId"], dtHistory.Rows[i]["RefDocumentTypeId"], dtHistory.Rows[i]["DocumentType"], dtHistory.Rows[i]["DocNo"], Convert.ToDateTime(dtHistory.Rows[i]["DocDate"]).ToString("dd-MMM-yyyy"), dtHistory.Rows[i]["RefDocNoId"], dtHistory.Rows[i]["RefDocNo"], dtHistory.Rows[i]["TotalQty"], dtHistory.Rows[i]["TotalWeight"], dtHistory.Rows[i]["TotalAmount"], dtHistory.Rows[i]["JobOrderNo"], dtHistory.Rows[i]["EntryUser"], dtHistory.Rows[i]["EntryDate"], dtHistory.Rows[i]["ModifyUser"], dtHistory.Rows[i]["ModifyDate"]);
				}
				DataGridMain.DataSource = dtMain;
				DataGridMain.RetrieveStructure();
				GridSettings();
			}
			else
			{
				DataGridMain.ClearStructure();
				DataGridDetail.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GridSettings()
	{
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataGridMain.RootTable.Columns["Id"].Visible = false;
			DataGridMain.RootTable.Columns["DocumentTypeId"].Visible = false;
			DataGridMain.RootTable.Columns["RefDocumentTypeId"].Visible = false;
			DataGridMain.RootTable.Columns["RefDocNoId"].Visible = false;
			DataGridMain.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)DataGridMain.RootTable.Columns)
			{
				item.HeaderStyle.TextAlignment = (TextAlignment)2;
			}
			DataGridMain.RootTable.Columns["DocumentType"].Width = Constants.InventoryConstants.DocumentType;
			DataGridMain.RootTable.Columns["DocNo"].Width = Constants.InventoryConstants.DocNoConstant;
			DataGridMain.RootTable.Columns["DocDate"].Width = Constants.InventoryConstants.DateConstant;
			DataGridMain.RootTable.Columns["RefDocNo"].Width = Constants.InventoryConstants.DocNoConstant;
			DataGridMain.RootTable.Columns["TotalQty"].Width = Constants.InventoryConstants.QtyConstant;
			DataGridMain.RootTable.Columns["TotalWeight"].Width = Constants.InventoryConstants.WeightConstant;
			DataGridMain.RootTable.Columns["TotalAmount"].Width = Constants.InventoryConstants.AmountConstant;
			DataGridMain.RootTable.Columns["JobOrderNo"].Width = Constants.InventoryConstants.ProductionNo;
			DataGridMain.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants;
			DataGridMain.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM;
			DataGridMain.RootTable.Columns["ModifyUser"].Width = Constants.InventoryConstants.EntryUserConstants;
			DataGridMain.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM;
			DataGridMain.RootTable.Columns["DocDate"].FilterEditType = (FilterEditType)6;
			DataGridMain.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yy hh:mm tt";
			DataGridMain.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yy hh:mm tt";
			DataGridMain.RootTable.Columns["TotalQty"].AggregateFunction = (AggregateFunction)2;
			DataGridMain.RootTable.Columns["TotalQty"].TextAlignment = (TextAlignment)3;
			DataGridMain.RootTable.Columns["TotalQty"].FormatString = "#,##0.###";
			DataGridMain.RootTable.Columns["TotalQty"].TotalFormatString = "#,##0.###";
			DataGridMain.RootTable.Columns["TotalWeight"].AggregateFunction = (AggregateFunction)2;
			DataGridMain.RootTable.Columns["TotalWeight"].TextAlignment = (TextAlignment)3;
			DataGridMain.RootTable.Columns["TotalWeight"].FormatString = "#,##0.###";
			DataGridMain.RootTable.Columns["TotalWeight"].TotalFormatString = "#,##0.###";
			DataGridMain.RootTable.Columns["TotalAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridMain.RootTable.Columns["TotalAmount"].TextAlignment = (TextAlignment)3;
			DataGridMain.RootTable.Columns["TotalAmount"].FormatString = "#,##0.###";
			DataGridMain.RootTable.Columns["TotalAmount"].TotalFormatString = "#,##0.###";
			DataGridMain.RootTable.Columns.Add("Select");
			DataGridMain.RootTable.Columns["Select"].ActAsSelector = true;
			DataGridMain.RootTable.Columns["Select"].UseHeaderSelector = true;
			DataGridMain.RootTable.Columns["Select"].Width = 40;
			DataGridMain.RootTable.Columns["Select"].Position = 0;
			DataGridMain.RootTable.Columns.Add("IsApproved");
			DataGridMain.RootTable.Columns["IsApproved"].Caption = "Approve";
			DataGridMain.RootTable.Columns["IsApproved"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridMain.RootTable.Columns["IsApproved"].ButtonStyle = (ButtonStyle)5;
			DataGridMain.RootTable.Columns["IsApproved"].ButtonText = "Approve";
			DataGridMain.RootTable.Columns["IsApproved"].Width = 60;
			DataGridMain.RootTable.Columns["IsApproved"].Position = 1;
			DataGridMain.RootTable.Columns.Add("Slip");
			DataGridMain.RootTable.Columns["Slip"].Caption = "Slip";
			DataGridMain.RootTable.Columns["Slip"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridMain.RootTable.Columns["Slip"].ButtonStyle = (ButtonStyle)5;
			DataGridMain.RootTable.Columns["Slip"].ButtonText = "Slip";
			DataGridMain.RootTable.Columns["Slip"].Width = 40;
			DataGridMain.RootTable.Columns["Slip"].Position = 2;
			DataGridMain.FrozenColumns = 3;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridMain_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		try
		{
			GridEXRow r = DataGridMain.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				int num = Conversion.ToInt(r.Cells["Id"].Value);
				InvContractorWagesBillHeader head = new InvContractorWagesBillHeader();
				head = InvContractorWagesBillHeader.GetByID(num);
				DataTable dt = new DataTable();
				dt.Columns.Add("HeaderId");
				dt.Columns.Add("ContractorName");
				dt.Columns.Add("WagesAccount");
				dt.Columns.Add("packingType");
				dt.Columns.Add("Weight", typeof(double));
				dt.Columns.Add("PackSize");
				dt.Columns.Add("Quantity", typeof(double));
				dt.Columns.Add("WeightCut");
				dt.Columns.Add("BillWeight", typeof(double));
				dt.Columns.Add("RateWithoutAddLess");
				dt.Columns.Add("RateAddLess", typeof(double));
				dt.Columns.Add("Rate");
				dt.Columns.Add("Amount", typeof(double));
				for (int i = 0; i < head.invContractWagesBillDateil.Count; i++)
				{
					dt.Rows.Add(head.invContractWagesBillDateil[i].InvContractorWagesBillHeaderId, head.invContractWagesBillDateil[i].CompanyName, head.invContractWagesBillDateil[i].WagesAccountName, head.invContractWagesBillDateil[i].PackTypeDesc, head.invContractWagesBillDateil[i].Weight, head.invContractWagesBillDateil[i].PackSize, head.invContractWagesBillDateil[i].Qty, head.invContractWagesBillDateil[i].WeightCut, head.invContractWagesBillDateil[i].BillWeight, head.invContractWagesBillDateil[i].WageRate - head.invContractWagesBillDateil[i].RateAddLess, head.invContractWagesBillDateil[i].RateAddLess, head.invContractWagesBillDateil[i].WageRate, head.invContractWagesBillDateil[i].WagesAmount);
				}
				DataGridDetail.DataSource = dt;
				DataGridDetail.RetrieveStructure();
				GridDetailSetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridDetailSetting()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataGridDetail.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)DataGridDetail.RootTable.Columns)
			{
				item.HeaderStyle.TextAlignment = (TextAlignment)2;
			}
			DataGridDetail.RootTable.Columns["ContractorName"].Width = 150;
			DataGridDetail.RootTable.Columns["WagesAccount"].Width = 150;
			DataGridDetail.RootTable.Columns["packingType"].Width = 80;
			DataGridDetail.RootTable.Columns["packingType"].Caption = "Packing Type";
			DataGridDetail.RootTable.Columns["Weight"].Width = 80;
			DataGridDetail.RootTable.Columns["PackSize"].Width = 80;
			DataGridDetail.RootTable.Columns["Quantity"].Width = 80;
			DataGridDetail.RootTable.Columns["WeightCut"].Width = 80;
			DataGridDetail.RootTable.Columns["BillWeight"].Width = 80;
			DataGridDetail.RootTable.Columns["RateWithoutAddLess"].Width = 100;
			DataGridDetail.RootTable.Columns["RateAddLess"].Width = 80;
			DataGridDetail.RootTable.Columns["Rate"].Width = 80;
			DataGridDetail.RootTable.Columns["Amount"].Width = 80;
			DataGridDetail.RootTable.Columns["HeaderId"].Visible = false;
			DataGridDetail.RootTable.Columns["RateAddLess"].Visible = EnableAddLessOnWagesRegular;
			DataGridDetail.RootTable.Columns["RateWithoutAddLess"].Visible = EnableAddLessOnWagesRegular;
			DataGridDetail.RootTable.Columns["RateAddLess"].AggregateFunction = (AggregateFunction)2;
			DataGridDetail.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			DataGridDetail.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			DataGridDetail.RootTable.Columns["Quantity"].AggregateFunction = (AggregateFunction)2;
			DataGridDetail.RootTable.Columns["BillWeight"].AggregateFunction = (AggregateFunction)2;
			DataGridDetail.RootTable.Columns["RateWithoutAddLess"].TextAlignment = (TextAlignment)3;
			DataGridDetail.RootTable.Columns["RateAddLess"].TextAlignment = (TextAlignment)3;
			DataGridDetail.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			DataGridDetail.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			DataGridDetail.RootTable.Columns["Quantity"].TextAlignment = (TextAlignment)3;
			DataGridDetail.RootTable.Columns["BillWeight"].TextAlignment = (TextAlignment)3;
			DataGridDetail.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			DataGridDetail.RootTable.Columns["RateAddLess"].FormatString = "#,##0.##";
			DataGridDetail.RootTable.Columns["RateWithoutAddLess"].FormatString = "#,##0.##";
			DataGridDetail.RootTable.Columns["Amount"].FormatString = "0,0";
			DataGridDetail.RootTable.Columns["Weight"].FormatString = "0,0";
			DataGridDetail.RootTable.Columns["Quantity"].FormatString = "0,0";
			DataGridDetail.RootTable.Columns["BillWeight"].FormatString = "0,0";
			DataGridDetail.RootTable.Columns["RateAddLess"].TotalFormatString = "#,##0.##";
			DataGridDetail.RootTable.Columns["RateWithoutAddLess"].TotalFormatString = "#,##0.##";
			DataGridDetail.RootTable.Columns["Amount"].TotalFormatString = "0,0";
			DataGridDetail.RootTable.Columns["Weight"].TotalFormatString = "0,0";
			DataGridDetail.RootTable.Columns["Quantity"].TotalFormatString = "0,0";
			DataGridDetail.RootTable.Columns["BillWeight"].TotalFormatString = "0,0";
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnshow_Click(object sender, EventArgs e)
	{
		try
		{
			GetPendingRecords();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnApproved_Click(object sender, EventArgs e)
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Expected O, but got Unknown
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Expected O, but got Unknown
		try
		{
			if (DataGridMain.GetCheckedRows().Count() == 0)
			{
				throw new Exception("Please select check box first");
			}
			if (MessageBox.Show("Are you sure to Approve?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				ApprovalList obj = new ApprovalList();
				obj.AllApprovalLists = new List<ApprovalList>();
				GridEXRow[] checkedRows = DataGridMain.GetCheckedRows();
				foreach (GridEXRow r in checkedRows)
				{
					ApprovalList item = new ApprovalList();
					item.Id = Conversion.ToInt(r.Cells["Id"].Value);
					item.EntryUser = UserAccount.ID;
					obj.AllApprovalLists.Add(item);
				}
				InvContractorWagesBillHeader.ContractorWagesApprovalStatus_Update(obj);
				MessageBox.Show("Record Approve Successfully");
				GetPendingRecords();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridMain_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			int Id = Conversion.ToInt(DataGridMain.CurrentRow.Cells["Id"].Value);
			if (e.Column.Key == "Slip")
			{
				if (DocumentTypeId == 101)
				{
					CommonServices.ContractorWagesBill_SlipandRegister_002(Conversion.ToInt(DataGridMain.CurrentRow.Cells["Id"].Value));
				}
				else if (DocumentTypeId == 810)
				{
					CommonServices.ContractorWagesBillManualSlip004(Conversion.ToInt(DataGridMain.CurrentRow.Cells["Id"].Value));
				}
			}
			else if (e.Column.Key == "IsApproved")
			{
				ApproveRecord(Id);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ApproveRecord(int Id)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		try
		{
			if (MessageBox.Show("Are you sure to Approve?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				ApprovalList val = new ApprovalList
				{
					AllApprovalLists = new List<ApprovalList>()
				};
				ApprovalList item = new ApprovalList();
				item.Id = Id;
				item.EntryUser = UserAccount.ID;
				val.AllApprovalLists.Add(item);
				InvContractorWagesBillHeader.ContractorWagesApprovalStatus_Update(val);
				MessageBox.Show("Record Approve Successfully");
				GetPendingRecords();
			}
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
			datFromDate.Value = clsGlobalVariables.ActiveYr.Start_Period;
			datToDate.Value = DateTime.Now;
			((Control)(object)cmbDocumentType).Text = string.Empty;
			GetPendingRecords();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			BranchesFill();
			ComboFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CmbBranchName_Leave(object sender, EventArgs e)
	{
		try
		{
			ComboFill();
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
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Expected O, but got Unknown
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Expected O, but got Unknown
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Expected O, but got Unknown
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Expected O, but got Unknown
		//IL_07df: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e9: Expected O, but got Unknown
		GridEXLayout DataGridDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout DataGridMain_DesignTimeLayout = new GridEXLayout();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.DataGridDetail = new GridEX();
		this.panel7 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label5 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.DataGridMain = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label4 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.CmbBranchName = new UltraCombo();
		this.label8 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.cmbDocumentType = new UltraCombo();
		this.btnApproved = new System.Windows.Forms.Button();
		this.btnshow = new System.Windows.Forms.Button();
		this.datToDate = new System.Windows.Forms.DateTimePicker();
		this.datFromDate = new System.Windows.Forms.DateTimePicker();
		this.label27 = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.lblHeading = new System.Windows.Forms.Label();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridDetail).BeginInit();
		this.panel7.SuspendLayout();
		this.panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridMain).BeginInit();
		this.panel5.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBranchName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDocumentType).BeginInit();
		this.panel3.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		base.SuspendLayout();
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.Controls.Add(this.panel6);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.toolStrip3);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(901, 703);
		this.panel1.TabIndex = 0;
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridDetail);
		this.panel6.Controls.Add(this.panel7);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(0, 417);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(901, 286);
		this.panel6.TabIndex = 13;
		this.DataGridDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.DataGridDetail).BackColor = System.Drawing.Color.White;
		this.DataGridDetail.DefaultFilterRowComparison = (FilterConditionOperator)7;
		DataGridDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridDetail.DesignTimeLayout = DataGridDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.DataGridDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		((System.Windows.Forms.Control)(object)this.DataGridDetail).Location = new System.Drawing.Point(0, 26);
		((System.Windows.Forms.Control)(object)this.DataGridDetail).Name = "DataGridDetail";
		((ScrollableBase)this.DataGridDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridDetail).Size = new System.Drawing.Size(901, 260);
		((System.Windows.Forms.Control)(object)this.DataGridDetail).TabIndex = 13;
		this.DataGridDetail.TotalRow = (InheritableBoolean)1;
		this.DataGridDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridDetail).VisualStyle = (VisualStyle)4;
		this.panel7.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel7.Controls.Add(this.ctrlGrdBar2);
		this.panel7.Controls.Add(this.label5);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(901, 26);
		this.panel7.TabIndex = 12;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(861, 2);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
		this.ctrlGrdBar2.MyGrid = this.DataGridDetail;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar2.TabIndex = 27;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.ForeColor = System.Drawing.Color.White;
		this.label5.Location = new System.Drawing.Point(7, 3);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(353, 19);
		this.label5.TabIndex = 4;
		this.label5.Text = "Detail Information of Above Selected Row";
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridMain);
		this.panel2.Controls.Add(this.panel5);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 110);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(901, 307);
		this.panel2.TabIndex = 12;
		this.DataGridMain.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.DataGridMain).BackColor = System.Drawing.Color.White;
		this.DataGridMain.DefaultFilterRowComparison = (FilterConditionOperator)7;
		DataGridMain_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridMain.DesignTimeLayout = DataGridMain_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridMain).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridMain.FilterMode = (FilterMode)2;
		this.DataGridMain.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridMain).Font = new System.Drawing.Font("Verdana", 8.25f);
		((System.Windows.Forms.Control)(object)this.DataGridMain).Location = new System.Drawing.Point(0, 24);
		((System.Windows.Forms.Control)(object)this.DataGridMain).Name = "DataGridMain";
		((ScrollableBase)this.DataGridMain).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridMain).Size = new System.Drawing.Size(901, 283);
		((System.Windows.Forms.Control)(object)this.DataGridMain).TabIndex = 12;
		this.DataGridMain.TotalRow = (InheritableBoolean)1;
		this.DataGridMain.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridMain).VisualStyle = (VisualStyle)4;
		this.DataGridMain.ColumnButtonClick += new ColumnActionEventHandler(DataGridMain_ColumnButtonClick);
		this.DataGridMain.SelectionChanged += new System.EventHandler(DataGridMain_SelectionChanged);
		this.panel5.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel5.Controls.Add(this.ctrlGrdBar1);
		this.panel5.Controls.Add(this.label4);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(901, 24);
		this.panel5.TabIndex = 11;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(861, 1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
		this.ctrlGrdBar1.MyGrid = this.DataGridMain;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 26;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.White;
		this.label4.Location = new System.Drawing.Point(7, 2);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(150, 19);
		this.label4.TabIndex = 4;
		this.label4.Text = "Main Information";
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBranchName);
		this.panel4.Controls.Add(this.label8);
		this.panel4.Controls.Add(this.label7);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDocumentType);
		this.panel4.Controls.Add(this.btnApproved);
		this.panel4.Controls.Add(this.btnshow);
		this.panel4.Controls.Add(this.datToDate);
		this.panel4.Controls.Add(this.datFromDate);
		this.panel4.Controls.Add(this.label27);
		this.panel4.Controls.Add(this.label33);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.panel4.Location = new System.Drawing.Point(0, 59);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(901, 51);
		this.panel4.TabIndex = 11;
		this.CmbBranchName.AllowNull = (DefaultableBoolean)1;
		this.CmbBranchName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBranchName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBranchName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBranchName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance75;
		((SpecialBoxBase)((UltraGridBase)this.CmbBranchName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance76).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance76).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance76).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance77).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance77;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance78).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance80).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance81).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance81).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance81).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance81;
		((AppearanceBase)appearance82).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBranchName).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbBranchName).Location = new System.Drawing.Point(5, 21);
		((System.Windows.Forms.Control)(object)this.CmbBranchName).Name = "CmbBranchName";
		((System.Windows.Forms.Control)(object)this.CmbBranchName).Size = new System.Drawing.Size(201, 26);
		((System.Windows.Forms.Control)(object)this.CmbBranchName).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbBranchName).Leave += new System.EventHandler(CmbBranchName_Leave);
		this.label8.AutoSize = true;
		this.label8.BackColor = System.Drawing.Color.White;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(5, 4);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(79, 15);
		this.label8.TabIndex = 157;
		this.label8.Text = "Branch Name";
		this.label7.AutoSize = true;
		this.label7.BackColor = System.Drawing.Color.White;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(465, 3);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(92, 15);
		this.label7.TabIndex = 155;
		this.label7.Text = "Document Type";
		this.cmbDocumentType.AllowNull = (DefaultableBoolean)1;
		this.cmbDocumentType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDocumentType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDocumentType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDocumentType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.cmbDocumentType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDocumentType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance62).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance62).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance62).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDocumentType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance62;
		((AppearanceBase)appearance63).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDocumentType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance63;
		((SpecialBoxBase)((UltraGridBase)this.cmbDocumentType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance64).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance64).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance64).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDocumentType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.cmbDocumentType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDocumentType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDocumentType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance65;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance66).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDocumentType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.cmbDocumentType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDocumentType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDocumentType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance67;
		((AppearanceBase)appearance68).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance68).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDocumentType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.cmbDocumentType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDocumentType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance69).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance69).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDocumentType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance69;
		((AppearanceBase)appearance70).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDocumentType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.cmbDocumentType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDocumentType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDocumentType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.cmbDocumentType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDocumentType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.cmbDocumentType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDocumentType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDocumentType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDocumentType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDocumentType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDocumentType).Location = new System.Drawing.Point(465, 21);
		((System.Windows.Forms.Control)(object)this.cmbDocumentType).Name = "cmbDocumentType";
		((System.Windows.Forms.Control)(object)this.cmbDocumentType).Size = new System.Drawing.Size(218, 26);
		((System.Windows.Forms.Control)(object)this.cmbDocumentType).TabIndex = 3;
		this.btnApproved.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnApproved.FlatAppearance.BorderSize = 0;
		this.btnApproved.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnApproved.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnApproved.ForeColor = System.Drawing.Color.White;
		this.btnApproved.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnApproved.Location = new System.Drawing.Point(740, 21);
		this.btnApproved.Margin = new System.Windows.Forms.Padding(2);
		this.btnApproved.Name = "btnApproved";
		this.btnApproved.Size = new System.Drawing.Size(77, 26);
		this.btnApproved.TabIndex = 5;
		this.btnApproved.Text = "Approve";
		this.btnApproved.UseVisualStyleBackColor = false;
		this.btnApproved.Click += new System.EventHandler(btnApproved_Click);
		this.btnshow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(684, 21);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(55, 26);
		this.btnshow.TabIndex = 4;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.datToDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datToDate.CustomFormat = "dd-MMM-yyyy";
		this.datToDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datToDate.Location = new System.Drawing.Point(336, 23);
		this.datToDate.Name = "datToDate";
		this.datToDate.Size = new System.Drawing.Size(128, 23);
		this.datToDate.TabIndex = 2;
		this.datFromDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datFromDate.CustomFormat = "dd-MMM-yyyy";
		this.datFromDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datFromDate.Location = new System.Drawing.Point(207, 23);
		this.datFromDate.Name = "datFromDate";
		this.datFromDate.Size = new System.Drawing.Size(128, 23);
		this.datFromDate.TabIndex = 1;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(209, 3);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(63, 15);
		this.label27.TabIndex = 139;
		this.label27.Text = "From Date";
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(337, 3);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(48, 15);
		this.label33.TabIndex = 138;
		this.label33.Text = "To Date";
		this.panel3.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel3.Controls.Add(this.lblHeading);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 27);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(901, 32);
		this.panel3.TabIndex = 10;
		this.lblHeading.AutoSize = true;
		this.lblHeading.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblHeading.ForeColor = System.Drawing.Color.White;
		this.lblHeading.Location = new System.Drawing.Point(4, 6);
		this.lblHeading.Name = "lblHeading";
		this.lblHeading.Size = new System.Drawing.Size(202, 19);
		this.lblHeading.TabIndex = 4;
		this.lblHeading.Text = "Labour Wages Approval";
		this.toolStrip3.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnNew, this.btnRefresh });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(901, 27);
		this.toolStrip3.TabIndex = 9;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(68, 24);
		this.btnNew.Text = "&Reset";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(901, 703);
		base.Controls.Add(this.panel1);
		base.Name = "PendingApprovalLabourWages";
		this.Text = "Pending Approval Stock Transfer";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(PendingApprovalStockTransfer_Load);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.panel6.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridDetail).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.panel2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridMain).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBranchName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDocumentType).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
