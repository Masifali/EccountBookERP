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
using Architecture.BLL.Reports.Inventory;
using Architecture.BLL.WeightBridge;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Common;
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
namespace Architecture.WinApp.Inventory_Reports
{

public class WeighBridge_WeightUpdate : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private DataTable lstwb = new DataTable();

	private DataTable dtBranch = new DataTable();

	private IContainer components;

	private Panel panel1;

	private Panel panel3;

	private Panel panel2;

	private ToolStrip toolStrip3;

	private ToolStripButton btnNew;

	private ToolStripButton print;

	private Label label23;

	private Panel panel4;

	private Panel panel5;

	internal GridEX DataGridHistory;

	private DateTimePicker ToDate;

	private Label label4;

	private Label label5;

	private TextBox TxtVehicleNumber;

	private Label label2;

	private TextBox txtTicketNoTo;

	private Label label36;

	private Label label35;

	private TextBox txtTicketNoFrom;

	private DateTimePicker FromDate;

	private TextBox GpNoTo;

	private Label label3;

	private TextBox GPNoFrom;

	private Label label6;

	private CtrlGrdBar ctrlGrdBar1;

	private Button btnshow;

	internal UltraCombo CmbGatePassType;

	private Label lblLanguage;

	internal UltraCombo CmbWeighBridgeType;

	private Label label1;

	internal UltraCombo CmbBranch;

	private Label label7;

	internal UltraCombo CmbPartyName;

	private Label label8;

	private ToolStripButton toolStripButton1;

	private Panel panel6;

	private Label label9;

	private CtrlGrdBar ctrlGrdBar2;

	internal UltraCombo CmbWeighBridgeParty;

	private Label label12;

	private TextBox txtOrderNoTo;

	private Label label10;

	private TextBox txtOrderNoFrom;

	private Label label11;

	internal UltraCombo CmbDocType;

	private Label label13;

	internal UltraCombo CmbWbStatus;

	private Label label14;

	private TextBox txtComments;

	private Label label15;

	private ToolStripButton btnUpdate;

	public WeighBridge_WeightUpdate(UserAccount user)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void frmWeightBridgeHistory_Load(object sender, EventArgs e)
	{
		try
		{
			BranchesFill();
			FromDate.Value = DateTime.Now.AddDays(-7.0);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GatePassTypeFill()
	{
		try
		{
			DataTable dt = CommonServices.GatePassReferenceTypes();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbGatePassType, "Id", "Name", "GatePassType", ZeroIndex: false);
				((UltraGridBase)CmbGatePassType).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				UltraGridColumn obj = ((UltraGridBase)CmbGatePassType).DisplayLayout.Bands[0].Columns.Add();
				((KeyedSubObjectBase)obj).Key = "Selected";
				((HeaderBase)obj.Header).Caption = string.Empty;
				obj.Header.CheckBoxVisibility = (HeaderCheckBoxVisibility)2;
				obj.DataType = typeof(bool);
				((HeaderBase)obj.Header).VisiblePosition = 0;
				CmbGatePassType.CheckedListSettings.CheckStateMember = "Selected";
				((EditorCheckedListSettings)CmbGatePassType.CheckedListSettings).EditorValueSource = (EditorWithComboValueSource)1;
				((EditorCheckedListSettings)CmbGatePassType.CheckedListSettings).ListSeparator = ",";
				((EditorCheckedListSettings)CmbGatePassType.CheckedListSettings).ItemCheckArea = (ItemCheckArea)1;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BranchesFill()
	{
		try
		{
			dtBranch = Branches.GetBranchesAllocatedToUserFromWeighBridge(UserAccount.OrganizationId, UserAccount.CompanyId, UserAccount.ID, 0);
			if (dtBranch.Rows.Count > 0)
			{
				DDL.BindDDL(dtBranch, CmbBranch, "BranchId", "BranchName", "Branch Name", ZeroIndex: false);
				UltraGridColumn obj = ((UltraGridBase)CmbBranch).DisplayLayout.Bands[0].Columns.Add();
				((KeyedSubObjectBase)obj).Key = "Selected";
				((HeaderBase)obj.Header).Caption = string.Empty;
				obj.Header.CheckBoxVisibility = (HeaderCheckBoxVisibility)2;
				obj.DataType = typeof(bool);
				((HeaderBase)obj.Header).VisiblePosition = 0;
				CmbBranch.CheckedListSettings.CheckStateMember = "Selected";
				((EditorCheckedListSettings)CmbBranch.CheckedListSettings).EditorValueSource = (EditorWithComboValueSource)1;
				((EditorCheckedListSettings)CmbBranch.CheckedListSettings).ListSeparator = ",";
				((EditorCheckedListSettings)CmbBranch.CheckedListSettings).ItemCheckArea = (ItemCheckArea)1;
				((UltraGridBase)CmbBranch).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				((Control)(object)CmbBranch).Text = UserAccount.BranchName.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void WbType()
	{
		try
		{
			DataTable dt = CommonServices.GatePassTypeFill("WeighBridge");
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbWeighBridgeType, "Id", "GpTypeDescription", "WeighBridge Type", ZeroIndex: false);
				((UltraGridBase)CmbWeighBridgeType).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PartyNameFill()
	{
		try
		{
			DataTable dt = SupplierCustomer.GetPartiesFromWbTransactions(UserAccount.OrganizationId, UserAccount.CompanyId);
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbPartyName, "Id", "CompanyName", "PartyName", ZeroIndex: false);
				((UltraGridBase)CmbPartyName).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void WbPartyFill()
	{
		try
		{
			DataTable dt = WbPartyandItemsDefine.GetWbParties(UserAccount.OrganizationId, UserAccount.CompanyId);
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbWeighBridgeParty, "Id", "PartyName", "Wb Party", ZeroIndex: false);
				((UltraGridBase)CmbWeighBridgeParty).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DocTypeFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("DocType");
			dt.Rows.Add(102, "Auto");
			dt.Rows.Add(113, "Manual");
			DDL.BindDDL(dt, CmbDocType, "Id", "DocType", "Doc Type", ZeroIndex: false);
			((UltraGridBase)CmbDocType).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void WbStatusFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Value");
			dt.Rows.Add(1, "Open");
			dt.Rows.Add(2, "Accepted");
			DDL.BindDDL(dt, CmbDocType, "Id", "Value", "Wb Status", ZeroIndex: false);
			((UltraGridBase)CmbDocType).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdweightBridge()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.FromDocNo = Conversion.ToInt((object)txtTicketNoFrom.Text.Trim());
			obj.ToDocNo = Conversion.ToInt((object)txtTicketNoTo.Text.Trim());
			obj.GpSrNoF = Conversion.ToInt((object)GPNoFrom.Text.Trim());
			obj.GpSrNoT = Conversion.ToInt((object)GpNoTo.Text.Trim());
			obj.FromDate = Conversion.ToDateTime((object)FromDate.Value);
			obj.ToDate = Conversion.ToDateTime((object)ToDate.Value);
			obj.VehicleNo = Conversion.ToString((object)TxtVehicleNumber.Text.Trim());
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.Activity = Conversion.ToString((object)((Control)(object)CmbWeighBridgeType).Text);
			obj.BranchesId = Conversion.ToInt(CmbBranch.Value);
			obj.SupplierCustomerId = Conversion.ToInt(CmbPartyName.Value);
			obj.OrderNoFrom = Conversion.ToInt((object)txtOrderNoFrom.Text);
			obj.OrderNoTo = Conversion.ToInt((object)txtOrderNoTo.Text);
			obj.DocumentTypeId = Conversion.ToInt(CmbDocType.Value);
			obj.CompanyName = Conversion.ToString((object)((Control)(object)CmbWeighBridgeParty).Text);
			obj.Status = "Open";
			string BranchIds = string.Empty;
			if (((Control)(object)CmbBranch).Text != string.Empty)
			{
				string[] array = (Conversion.ToString((object)((Control)(object)CmbBranch).Text) + ",").ToString().Split(',');
				foreach (string author in array)
				{
					DataRow[] dr = dtBranch.Select("BranchName = '" + Conversion.ToString((object)author) + "'");
					if (dr.Length != 0)
					{
						BranchIds = BranchIds + "," + Conversion.ToString(dr[0].ItemArray[0]);
					}
				}
				obj.BranchesIds = BranchIds;
			}
			lstwb = WbTransactionsReports.WbTransactionHistory_Updating(obj);
			if (lstwb.Rows.Count > 0)
			{
				DataTable dtWb = new DataTable();
				dtWb.Columns.Add("Id", typeof(int));
				dtWb.Columns.Add("DocTypeId", typeof(int));
				dtWb.Columns.Add("ReferenceDocTypeId", typeof(int));
				dtWb.Columns.Add("ReferenceDocNoId", typeof(int));
				dtWb.Columns.Add("BranchName", typeof(string));
				dtWb.Columns.Add("DocumentTypeDescription", typeof(string));
				dtWb.Columns.Add("DocDate", typeof(DateTime));
				dtWb.Columns.Add("TicketNo.", typeof(int));
				dtWb.Columns.Add("OrderNo", typeof(int));
				dtWb.Columns.Add("GpNo", typeof(int));
				dtWb.Columns.Add("SupplierCustomerId", typeof(int));
				dtWb.Columns.Add("PartyName", typeof(string));
				dtWb.Columns.Add("VehicleNo", typeof(string));
				dtWb.Columns.Add("BiltyNo", typeof(int));
				dtWb.Columns.Add("SupplierWeight", typeof(double));
				dtWb.Columns.Add("FirstWeight", typeof(double));
				dtWb.Columns.Add("SecondWeight", typeof(double));
				dtWb.Columns.Add("NetWbWeight", typeof(double));
				dtWb.Columns.Add("FirstDateTime", typeof(DateTime));
				dtWb.Columns.Add("SecondDateTime", typeof(DateTime));
				dtWb.Columns.Add("ItemDescription", typeof(string));
				dtWb.Columns.Add("ItemQty", typeof(double));
				dtWb.Columns.Add("WeighBridgeType", typeof(string));
				dtWb.Columns.Add("WbCharges", typeof(double));
				dtWb.Columns.Add("WbRemarks", typeof(string));
				dtWb.Columns.Add("EntryUser", typeof(string));
				dtWb.Columns.Add("EntryDate", typeof(DateTime));
				dtWb.Columns.Add("IsApproved", typeof(bool));
				dtWb.Columns.Add("ApprovedUser", typeof(string));
				dtWb.Columns.Add("ApprovedDate", typeof(DateTime));
				dtWb.Clear();
				for (int i = 0; i < lstwb.Rows.Count; i++)
				{
					dtWb.Rows.Add(Conversion.ToInt(lstwb.Rows[i]["Id"]), Conversion.ToInt(lstwb.Rows[i]["DocTypeId"]), Conversion.ToInt(lstwb.Rows[i]["ReferenceDocTypeId"]), Conversion.ToInt(lstwb.Rows[i]["ReferenceDocNoId"]), Conversion.ToString(lstwb.Rows[i]["BranchName"]), Conversion.ToString(lstwb.Rows[i]["DocumentTypeDescription"]), Conversion.ToDateTime(lstwb.Rows[i]["DocDate"]), Conversion.ToInt(lstwb.Rows[i]["TicketNo"]), Conversion.ToInt(lstwb.Rows[i]["OrderNo"]), Conversion.ToInt(lstwb.Rows[i]["GpSrNo"]), Conversion.ToInt(lstwb.Rows[i]["SupplierCustomerId"]), Conversion.ToString(lstwb.Rows[i]["PartyName"]), Conversion.ToString(lstwb.Rows[i]["VehicleNo"]), Conversion.ToInt(lstwb.Rows[i]["BiltyNo"]), Conversion.ToDouble(lstwb.Rows[i]["SupplierWeight"]), Conversion.ToDouble(lstwb.Rows[i]["FirstWeight"]), Conversion.ToDouble(lstwb.Rows[i]["SecondWeight"]), Conversion.ToDouble(lstwb.Rows[i]["NetWbWeight"]), lstwb.Rows[i]["FirstDateTime"], lstwb.Rows[i]["SecondDateTime"], Conversion.ToString(lstwb.Rows[i]["ItemDescription"]), Conversion.ToDouble(lstwb.Rows[i]["ItemQty"]), Conversion.ToString(lstwb.Rows[i]["WeighBridgeType"]), Conversion.ToDouble(lstwb.Rows[i]["WbCharges"]), Conversion.ToString(lstwb.Rows[i]["WbRemarks"]), Conversion.ToString(lstwb.Rows[i]["FirstNameEusr"]), lstwb.Rows[i]["EntryDate"], Conversion.ToBool(lstwb.Rows[i]["IsApproved"]), Conversion.ToString(lstwb.Rows[i]["FirstNameAusr"]), Conversion.ToDateTime(lstwb.Rows[i]["ApprovedDateTime"]));
				}
				DataGridHistory.DataSource = dtWb;
				DataGridHistory.RetrieveStructure();
				GridSetting();
			}
			else
			{
				DataGridHistory.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSetting()
	{
		//IL_08c7: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataGridHistory.AllowEdit = (InheritableBoolean)1;
			DataGridHistory.AllowDelete = (InheritableBoolean)2;
			DataGridHistory.RootTable.Columns["Id"].Visible = false;
			DataGridHistory.RootTable.Columns["DocTypeId"].Visible = false;
			DataGridHistory.RootTable.Columns["ReferenceDocNoId"].Visible = false;
			DataGridHistory.RootTable.Columns["ReferenceDocTypeId"].Visible = false;
			DataGridHistory.RootTable.Columns["SupplierCustomerId"].Visible = false;
			DataGridHistory.RootTable.Columns["IsApproved"].Visible = false;
			DataGridHistory.RootTable.Columns["ApprovedUser"].Visible = false;
			DataGridHistory.RootTable.Columns["ApprovedDate"].Visible = false;
			DataGridHistory.RootTable.Columns["Id"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["DocTypeId"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["ReferenceDocTypeId"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["ReferenceDocNoId"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["BranchName"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["DocumentTypeDescription"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["DocDate"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["TicketNo."].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["OrderNo"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["GpNo"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["SupplierCustomerId"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["PartyName"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["VehicleNo"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["BiltyNo"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["SupplierWeight"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["FirstWeight"].EditType = (EditType)1;
			DataGridHistory.RootTable.Columns["SecondWeight"].EditType = (EditType)1;
			DataGridHistory.RootTable.Columns["NetWbWeight"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["FirstDateTime"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["SecondDateTime"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["ItemDescription"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["ItemQty"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["WeighBridgeType"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["WbCharges"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["WbRemarks"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["EntryUser"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["EntryDate"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["IsApproved"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["ApprovedUser"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["ApprovedDate"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["Id"].FilterEditType = (FilterEditType)1;
			DataGridHistory.RootTable.Columns["DocTypeId"].FilterEditType = (FilterEditType)1;
			DataGridHistory.RootTable.Columns["ReferenceDocTypeId"].FilterEditType = (FilterEditType)1;
			DataGridHistory.RootTable.Columns["ReferenceDocNoId"].FilterEditType = (FilterEditType)1;
			DataGridHistory.RootTable.Columns["BranchName"].FilterEditType = (FilterEditType)1;
			DataGridHistory.RootTable.Columns["DocumentTypeDescription"].FilterEditType = (FilterEditType)1;
			DataGridHistory.RootTable.Columns["DocDate"].FilterEditType = (FilterEditType)6;
			DataGridHistory.RootTable.Columns["TicketNo."].FilterEditType = (FilterEditType)1;
			DataGridHistory.RootTable.Columns["OrderNo"].FilterEditType = (FilterEditType)1;
			DataGridHistory.RootTable.Columns["GpNo"].FilterEditType = (FilterEditType)1;
			DataGridHistory.RootTable.Columns["SupplierCustomerId"].FilterEditType = (FilterEditType)1;
			DataGridHistory.RootTable.Columns["PartyName"].FilterEditType = (FilterEditType)1;
			DataGridHistory.RootTable.Columns["VehicleNo"].FilterEditType = (FilterEditType)1;
			DataGridHistory.RootTable.Columns["BiltyNo"].FilterEditType = (FilterEditType)1;
			DataGridHistory.RootTable.Columns["SupplierWeight"].FilterEditType = (FilterEditType)1;
			DataGridHistory.RootTable.Columns["FirstWeight"].FilterEditType = (FilterEditType)1;
			DataGridHistory.RootTable.Columns["SecondWeight"].FilterEditType = (FilterEditType)1;
			DataGridHistory.RootTable.Columns["NetWbWeight"].FilterEditType = (FilterEditType)1;
			DataGridHistory.RootTable.Columns["FirstDateTime"].FilterEditType = (FilterEditType)6;
			DataGridHistory.RootTable.Columns["SecondDateTime"].FilterEditType = (FilterEditType)6;
			DataGridHistory.RootTable.Columns["ItemDescription"].FilterEditType = (FilterEditType)1;
			DataGridHistory.RootTable.Columns["ItemQty"].FilterEditType = (FilterEditType)1;
			DataGridHistory.RootTable.Columns["WeighBridgeType"].FilterEditType = (FilterEditType)1;
			DataGridHistory.RootTable.Columns["WbCharges"].FilterEditType = (FilterEditType)1;
			DataGridHistory.RootTable.Columns["WbRemarks"].FilterEditType = (FilterEditType)1;
			DataGridHistory.RootTable.Columns["EntryUser"].FilterEditType = (FilterEditType)1;
			DataGridHistory.RootTable.Columns["EntryDate"].FilterEditType = (FilterEditType)6;
			DataGridHistory.RootTable.Columns["IsApproved"].FilterEditType = (FilterEditType)1;
			DataGridHistory.RootTable.Columns["ApprovedUser"].FilterEditType = (FilterEditType)1;
			DataGridHistory.RootTable.Columns["ApprovedDate"].FilterEditType = (FilterEditType)6;
			DataGridHistory.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)DataGridHistory.RootTable.Columns)
			{
				item.HeaderStyle.TextAlignment = (TextAlignment)2;
			}
			DataGridHistory.RootTable.Columns["FirstWeight"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["FirstWeight"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["FirstWeight"].FormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["FirstWeight"].TotalFormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["SecondWeight"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["SecondWeight"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["SecondWeight"].FormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["SecondWeight"].TotalFormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["NetWbWeight"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["NetWbWeight"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["NetWbWeight"].FormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["NetWbWeight"].TotalFormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["ItemQty"].FormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["WbCharges"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["WbCharges"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["WbCharges"].FormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["WbCharges"].TotalFormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["SupplierWeight"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["SupplierWeight"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["SupplierWeight"].FormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["SupplierWeight"].TotalFormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["DocumentTypeDescription"].Width = Constants.InventoryConstants.StringConstant;
			DataGridHistory.RootTable.Columns["WeighBridgeType"].Width = Constants.InventoryConstants.PackingType;
			DataGridHistory.RootTable.Columns["BranchName"].Width = 80;
			DataGridHistory.RootTable.Columns["DocDate"].Width = Constants.InventoryConstants.DateConstant;
			DataGridHistory.RootTable.Columns["TicketNo."].Width = Constants.InventoryConstants.DocNoConstant;
			DataGridHistory.RootTable.Columns["OrderNo"].Width = Constants.InventoryConstants.OrderNo;
			DataGridHistory.RootTable.Columns["GpNo"].Width = Constants.InventoryConstants.GpNoConstant;
			DataGridHistory.RootTable.Columns["PartyName"].Width = Constants.InventoryConstants.StringConstant;
			DataGridHistory.RootTable.Columns["VehicleNo"].Width = Constants.InventoryConstants.VehicleNoConstant;
			DataGridHistory.RootTable.Columns["BiltyNo"].Width = Constants.InventoryConstants.BiltyNoConstant;
			DataGridHistory.RootTable.Columns["ItemDescription"].Width = Constants.InventoryConstants.StringConstant;
			DataGridHistory.RootTable.Columns["ItemQty"].Width = Constants.InventoryConstants.QtyConstant;
			DataGridHistory.RootTable.Columns["SupplierWeight"].Width = Constants.InventoryConstants.WeightConstant;
			DataGridHistory.RootTable.Columns["WbCharges"].Width = Constants.InventoryConstants.AmountConstant;
			DataGridHistory.RootTable.Columns["FirstWeight"].Width = Constants.InventoryConstants.WeightConstant;
			DataGridHistory.RootTable.Columns["SecondWeight"].Width = Constants.InventoryConstants.WeightConstant;
			DataGridHistory.RootTable.Columns["NetWbWeight"].Width = Constants.InventoryConstants.WeightConstant;
			DataGridHistory.RootTable.Columns["FirstDateTime"].Width = Constants.InventoryConstants.DateTimeConstantddMMM;
			DataGridHistory.RootTable.Columns["SecondDateTime"].Width = Constants.InventoryConstants.DateTimeConstantddMMM;
			DataGridHistory.RootTable.Columns["WbRemarks"].Width = Constants.InventoryConstants.StringConstant;
			DataGridHistory.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants;
			DataGridHistory.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM;
			DataGridHistory.RootTable.Columns["IsApproved"].Width = Constants.InventoryConstants.IsApproved;
			DataGridHistory.RootTable.Columns["ApprovedUser"].Width = Constants.InventoryConstants.ApprovedByConstant;
			DataGridHistory.RootTable.Columns["ApprovedDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM;
			DataGridHistory.RootTable.Columns["FirstDateTime"].FormatString = "dd-MMM-yy hh:mm tt";
			DataGridHistory.RootTable.Columns["SecondDateTime"].FormatString = "dd-MMM-yy hh:mm tt";
			DataGridHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yy hh:mm tt";
			DataGridHistory.RootTable.Columns["ApprovedDate"].FormatString = "dd-MMM-yy hh:mm tt";
			DataGridHistory.RootTable.Columns.Add("Select");
			DataGridHistory.RootTable.Columns["Select"].ActAsSelector = true;
			DataGridHistory.RootTable.Columns["Select"].UseHeaderSelector = true;
			DataGridHistory.RootTable.Columns["Select"].Width = 25;
			DataGridHistory.RootTable.Columns["Select"].Position = 0;
			DataGridHistory.RootTable.Columns.Add("Update");
			DataGridHistory.RootTable.Columns["Update"].Caption = "Update";
			DataGridHistory.RootTable.Columns["Update"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Update"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Update"].ButtonText = "Update";
			DataGridHistory.RootTable.Columns["Update"].Width = 50;
			DataGridHistory.RootTable.Columns["Update"].Position = 1;
			DataGridHistory.RootTable.Columns.Add("Print");
			DataGridHistory.RootTable.Columns["Print"].Caption = "Print";
			DataGridHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Print"].ButtonText = "Print";
			DataGridHistory.RootTable.Columns["Print"].Width = 50;
			DataGridHistory.RootTable.Columns["Print"].Position = 1;
			DataGridHistory.FrozenColumns = 3;
			ctrlGrdBar2_Load(null, null);
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
			grdweightBridge();
			Reset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void Reset()
	{
		try
		{
			txtTicketNoFrom.Text = "0";
			txtTicketNoTo.Text = string.Empty;
			TxtVehicleNumber.Text = string.Empty;
			GPNoFrom.Text = string.Empty;
			GpNoTo.Text = string.Empty;
			FromDate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			ToDate.Value = DateTime.Now;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void print_Click(object sender, EventArgs e)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (lstwb.Rows.Count == 0 || lstwb == null)
			{
				throw new Exception("Record Not Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(lstwb, "281-InvRptWeighBridgeRegister.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		try
		{
			Close();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmWeightBridgeHistory_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.P)
			{
				print_Click(null, null);
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnNew_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				FromDate.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnshow_Click(object sender, EventArgs e)
	{
		try
		{
			grdweightBridge();
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

	private void DataGridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Expected O, but got Unknown
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			if (e.Column.Key == "Print")
			{
				DataTable dt = new DataTable();
				dt = WbTransactionsReports.WbTransactionSlip280(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					Id = Convert.ToInt32(r.Cells["Id"].Value),
					DocumentTypeId = Convert.ToInt32(r.Cells["DocTypeId"].Value)
				});
				if (dt == null || dt.Rows.Count == 0)
				{
					MessageBox.Show("Not Record Found For Display");
					return;
				}
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val.ShowReportWithDataTable(dt, "280-InvRptWeighBridgeSlip.rpt");
			}
			else if (e.Column.Key == "Update")
			{
				UpdateSingleRecord();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void toolStripButton1_Click(object sender, EventArgs e)
	{
		try
		{
			BranchesFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GPNoFrom_KeyPress(object sender, KeyPressEventArgs e)
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

	private void GpNoTo_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtTicketNoFrom_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtTicketNoTo_KeyPress(object sender, KeyPressEventArgs e)
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

	private void DataGridHistory_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataGridHistory.UpdateData();
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && (e.Column.Key == "FirstWeight" || e.Column.Key == "SecondWeight"))
			{
				double num = Conversion.ToDouble(r.Cells["FirstWeight"].Value);
				double SecondWeight = Conversion.ToDouble(r.Cells["SecondWeight"].Value);
				double NetWeight = Math.Abs(num - SecondWeight);
				r.Cells["NetWbWeight"].Value = NetWeight;
			}
			DataGridHistory.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UpdateSingleRecord()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Expected O, but got Unknown
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Expected O, but got Unknown
		try
		{
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			ReportsParameters val = new ReportsParameters
			{
				ReportsParameterslist = new List<ReportsParameters>()
			};
			int ActionId = 0;
			int WbId = Conversion.ToInt(r.Cells["Id"].Value);
			double FirstWeight = Conversion.ToDouble(r.Cells["FirstWeight"].Value);
			double SecondWeight = Conversion.ToDouble(r.Cells["SecondWeight"].Value);
			DataRow originalRow = lstwb.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<int>("Id") == WbId);
			if (originalRow != null)
			{
				bool firstWeightUpdated = false;
				bool secondWeightUpdated = false;
				if (FirstWeight != originalRow.Field<double>("FirstWeight"))
				{
					firstWeightUpdated = true;
				}
				if (SecondWeight != originalRow.Field<double>("SecondWeight") && originalRow.Field<double>("SecondWeight") != 0.0)
				{
					secondWeightUpdated = true;
				}
				if (firstWeightUpdated && secondWeightUpdated)
				{
					ActionId = 0;
				}
				else if (firstWeightUpdated)
				{
					ActionId = 1;
				}
				else
				{
					if (!secondWeightUpdated)
					{
						ActionId = 3;
						throw new Exception("No Change Found!");
					}
					ActionId = 2;
				}
			}
			if (txtComments.Text == string.Empty)
			{
				txtComments.Focus();
				throw new Exception("Comments Field Required");
			}
			ReportsParameters update = new ReportsParameters();
			update.ActionId = ActionId;
			update.FirstWeight = FirstWeight;
			update.SecondWeight = SecondWeight;
			update.Id = WbId;
			update.Remarks = txtComments.Text;
			val.ReportsParameterslist.Add(update);
			WbTransactions.WbTransactions_WeightUpdation(val);
			MessageBox.Show("Update completed Successfully!");
			grdweightBridge();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdate_Click(object sender, EventArgs e)
	{
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Expected O, but got Unknown
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Expected O, but got Unknown
		try
		{
			if (DataGridHistory.GetCheckedRows().Count() == 0)
			{
				throw new Exception("Checked Rows First then update!");
			}
			if (txtComments.Text == string.Empty)
			{
				txtComments.Focus();
				throw new Exception("Comments Field Required");
			}
			ReportsParameters obj = new ReportsParameters();
			obj.ReportsParameterslist = new List<ReportsParameters>();
			string TicketNos = "";
			GridEXRow[] checkedRows = DataGridHistory.GetCheckedRows();
			foreach (GridEXRow r in checkedRows)
			{
				int ActionId = -1;
				int WbId = Conversion.ToInt(r.Cells["Id"].Value);
				Conversion.ToInt(r.Cells["TicketNo."].Value);
				double FirstWeight = Conversion.ToDouble(r.Cells["FirstWeight"].Value);
				double SecondWeight = Conversion.ToDouble(r.Cells["SecondWeight"].Value);
				DataRow originalRow = lstwb.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<int>("Id") == WbId);
				if (originalRow != null)
				{
					bool firstWeightUpdated = false;
					bool secondWeightUpdated = false;
					if (FirstWeight != originalRow.Field<double>("FirstWeight"))
					{
						firstWeightUpdated = true;
					}
					if (SecondWeight != originalRow.Field<double>("SecondWeight") && originalRow.Field<double>("SecondWeight") != 0.0)
					{
						secondWeightUpdated = true;
					}
					if (firstWeightUpdated && secondWeightUpdated)
					{
						ActionId = 0;
					}
					else if (firstWeightUpdated)
					{
						ActionId = 1;
					}
					else if (secondWeightUpdated)
					{
						ActionId = 2;
					}
					else
					{
						ActionId = 3;
						TicketNos += ",";
					}
				}
				if (ActionId != 3)
				{
					ReportsParameters update = new ReportsParameters();
					update.ActionId = ActionId;
					update.Id = WbId;
					update.FirstWeight = FirstWeight;
					update.SecondWeight = SecondWeight;
					update.Remarks = txtComments.Text;
					obj.ReportsParameterslist.Add(update);
				}
			}
			if (obj.ReportsParameterslist.Count > 0)
			{
				WbTransactions.WbTransactions_WeightUpdation(obj);
				grdweightBridge();
				if (TicketNos != string.Empty)
				{
					MessageBox.Show("Update completed successfully! However, some rows with Ticket Number's(" + TicketNos + ") were not updated because no changes were detected.");
				}
				else
				{
					MessageBox.Show("Update completed Successfully!");
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_UpdatingCell(object sender, UpdatingCellEventArgs e)
	{
		try
		{
			if ((e.Column.Key == "FirstWeight" || e.Column.Key == "SecondWeight") && !CommonServices.IsNumeric(e))
			{
				throw new Exception("Please Type Only Numeric Value");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_EditingCell(object sender, EditingCellEventArgs e)
	{
		try
		{
			if (e.Column.Key == "SecondWeight")
			{
				Conversion.ToInt(e.Value);
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
		//IL_0266: Unknown result type (might be due to invalid IL or missing references)
		//IL_0270: Expected O, but got Unknown
		//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bd: Expected O, but got Unknown
		//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d3: Expected O, but got Unknown
		//IL_02df: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e9: Expected O, but got Unknown
		//IL_0321: Unknown result type (might be due to invalid IL or missing references)
		//IL_032b: Expected O, but got Unknown
		//IL_0337: Unknown result type (might be due to invalid IL or missing references)
		//IL_0341: Expected O, but got Unknown
		//IL_034d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0357: Expected O, but got Unknown
		//IL_0363: Unknown result type (might be due to invalid IL or missing references)
		//IL_036d: Expected O, but got Unknown
		//IL_075b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0765: Expected O, but got Unknown
		//IL_0772: Unknown result type (might be due to invalid IL or missing references)
		//IL_077c: Expected O, but got Unknown
		//IL_0789: Unknown result type (might be due to invalid IL or missing references)
		//IL_0793: Expected O, but got Unknown
		//IL_07a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07aa: Expected O, but got Unknown
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance44 = new Appearance();
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
		Appearance appearance57 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.DataGridHistory = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label9 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.txtComments = new System.Windows.Forms.TextBox();
		this.label15 = new System.Windows.Forms.Label();
		this.CmbWbStatus = new UltraCombo();
		this.label14 = new System.Windows.Forms.Label();
		this.CmbDocType = new UltraCombo();
		this.label13 = new System.Windows.Forms.Label();
		this.CmbWeighBridgeParty = new UltraCombo();
		this.label12 = new System.Windows.Forms.Label();
		this.txtOrderNoTo = new System.Windows.Forms.TextBox();
		this.label10 = new System.Windows.Forms.Label();
		this.txtOrderNoFrom = new System.Windows.Forms.TextBox();
		this.label11 = new System.Windows.Forms.Label();
		this.CmbBranch = new UltraCombo();
		this.label7 = new System.Windows.Forms.Label();
		this.CmbPartyName = new UltraCombo();
		this.label8 = new System.Windows.Forms.Label();
		this.CmbWeighBridgeType = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.CmbGatePassType = new UltraCombo();
		this.lblLanguage = new System.Windows.Forms.Label();
		this.btnshow = new System.Windows.Forms.Button();
		this.GpNoTo = new System.Windows.Forms.TextBox();
		this.label3 = new System.Windows.Forms.Label();
		this.GPNoFrom = new System.Windows.Forms.TextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.ToDate = new System.Windows.Forms.DateTimePicker();
		this.label4 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.TxtVehicleNumber = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.txtTicketNoTo = new System.Windows.Forms.TextBox();
		this.label36 = new System.Windows.Forms.Label();
		this.label35 = new System.Windows.Forms.Label();
		this.txtTicketNoFrom = new System.Windows.Forms.TextBox();
		this.FromDate = new System.Windows.Forms.DateTimePicker();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.print = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel6.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbWbStatus).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDocType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWeighBridgeParty).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBranch).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPartyName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWeighBridgeType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbGatePassType).BeginInit();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.panel5);
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1199, 749);
		this.panel1.TabIndex = 0;
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.panel5.Controls.Add(this.panel6);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(0, 148);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1199, 601);
		this.panel5.TabIndex = 3;
		this.DataGridHistory.AllowEdit = (InheritableBoolean)2;
		this.DataGridHistory.DefaultFilterRowComparison = (FilterConditionOperator)7;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistory.DynamicFiltering = true;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(0, 30);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1199, 571);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 0;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.EditingCell += new EditingCellEventHandler(DataGridHistory_EditingCell);
		this.DataGridHistory.CellUpdated += new ColumnActionEventHandler(DataGridHistory_CellUpdated);
		this.DataGridHistory.UpdatingCell += new UpdatingCellEventHandler(DataGridHistory_UpdatingCell);
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.ctrlGrdBar2);
		this.panel6.Controls.Add(this.label9);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1199, 30);
		this.panel6.TabIndex = 2;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1152, 3);
		this.ctrlGrdBar2.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(42, 24);
		this.ctrlGrdBar2.TabIndex = 5;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.ForeColor = System.Drawing.Color.White;
		this.label9.Location = new System.Drawing.Point(9, 6);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(67, 19);
		this.label9.TabIndex = 4;
		this.label9.Text = "History";
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.txtComments);
		this.panel4.Controls.Add(this.label15);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWbStatus);
		this.panel4.Controls.Add(this.label14);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDocType);
		this.panel4.Controls.Add(this.label13);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWeighBridgeParty);
		this.panel4.Controls.Add(this.label12);
		this.panel4.Controls.Add(this.txtOrderNoTo);
		this.panel4.Controls.Add(this.label10);
		this.panel4.Controls.Add(this.txtOrderNoFrom);
		this.panel4.Controls.Add(this.label11);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBranch);
		this.panel4.Controls.Add(this.label7);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPartyName);
		this.panel4.Controls.Add(this.label8);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWeighBridgeType);
		this.panel4.Controls.Add(this.label1);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbGatePassType);
		this.panel4.Controls.Add(this.lblLanguage);
		this.panel4.Controls.Add(this.btnshow);
		this.panel4.Controls.Add(this.GpNoTo);
		this.panel4.Controls.Add(this.label3);
		this.panel4.Controls.Add(this.GPNoFrom);
		this.panel4.Controls.Add(this.label6);
		this.panel4.Controls.Add(this.ToDate);
		this.panel4.Controls.Add(this.label4);
		this.panel4.Controls.Add(this.label5);
		this.panel4.Controls.Add(this.TxtVehicleNumber);
		this.panel4.Controls.Add(this.label2);
		this.panel4.Controls.Add(this.txtTicketNoTo);
		this.panel4.Controls.Add(this.label36);
		this.panel4.Controls.Add(this.label35);
		this.panel4.Controls.Add(this.txtTicketNoFrom);
		this.panel4.Controls.Add(this.FromDate);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 55);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1199, 93);
		this.panel4.TabIndex = 0;
		this.txtComments.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtComments.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtComments.Location = new System.Drawing.Point(744, 35);
		this.txtComments.Multiline = true;
		this.txtComments.Name = "txtComments";
		this.txtComments.Size = new System.Drawing.Size(301, 53);
		this.txtComments.TabIndex = 17;
		this.label15.BackColor = System.Drawing.Color.Silver;
		this.label15.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(744, 9);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(301, 24);
		this.label15.TabIndex = 193;
		this.label15.Text = "Comments For Updation";
		this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.CmbWbStatus.AllowNull = (DefaultableBoolean)1;
		this.CmbWbStatus.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWbStatus.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWbStatus.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWbStatus).DisplayLayout.Appearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbWbStatus).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWbStatus).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWbStatus).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWbStatus).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance20;
		((SpecialBoxBase)((UltraGridBase)this.CmbWbStatus).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWbStatus).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbWbStatus).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWbStatus).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWbStatus).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWbStatus).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbWbStatus).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWbStatus).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWbStatus).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance26).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWbStatus).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbWbStatus).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWbStatus).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWbStatus).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWbStatus).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbWbStatus).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWbStatus).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWbStatus).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbWbStatus).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWbStatus).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbWbStatus).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWbStatus).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWbStatus).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWbStatus).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWbStatus.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWbStatus).Location = new System.Drawing.Point(882, 454);
		((System.Windows.Forms.Control)(object)this.CmbWbStatus).Name = "CmbWbStatus";
		((System.Windows.Forms.Control)(object)this.CmbWbStatus).Size = new System.Drawing.Size(154, 26);
		((System.Windows.Forms.Control)(object)this.CmbWbStatus).TabIndex = 15;
		((System.Windows.Forms.Control)(object)this.CmbWbStatus).Visible = false;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(770, 460);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(62, 15);
		this.label14.TabIndex = 191;
		this.label14.Text = "Wb Status";
		this.label14.Visible = false;
		this.CmbDocType.AllowNull = (DefaultableBoolean)1;
		this.CmbDocType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDocType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDocType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDocType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbDocType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDocType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance72).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance72).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDocType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDocType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance73;
		((SpecialBoxBase)((UltraGridBase)this.CmbDocType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDocType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.CmbDocType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDocType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance75).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDocType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance75;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance76).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDocType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.CmbDocType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDocType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDocType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance77;
		((AppearanceBase)appearance79).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance79).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDocType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.CmbDocType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDocType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance80).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance80).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance80).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDocType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance81).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDocType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.CmbDocType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDocType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance82).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDocType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.CmbDocType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDocType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.CmbDocType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDocType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDocType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDocType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDocType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDocType).Location = new System.Drawing.Point(882, 427);
		((System.Windows.Forms.Control)(object)this.CmbDocType).Name = "CmbDocType";
		((System.Windows.Forms.Control)(object)this.CmbDocType).Size = new System.Drawing.Size(211, 26);
		((System.Windows.Forms.Control)(object)this.CmbDocType).TabIndex = 14;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(770, 433);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(57, 15);
		this.label13.TabIndex = 189;
		this.label13.Text = "Doc Type";
		this.CmbWeighBridgeParty.AllowNull = (DefaultableBoolean)1;
		this.CmbWeighBridgeParty.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWeighBridgeParty.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWeighBridgeParty.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance58).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWeighBridgeParty).DisplayLayout.Appearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbWeighBridgeParty).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWeighBridgeParty).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance59).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance59).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWeighBridgeParty).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWeighBridgeParty).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance60;
		((SpecialBoxBase)((UltraGridBase)this.CmbWeighBridgeParty).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWeighBridgeParty).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.CmbWeighBridgeParty).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWeighBridgeParty).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance62).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWeighBridgeParty).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance62;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance63).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWeighBridgeParty).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbWeighBridgeParty).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWeighBridgeParty).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWeighBridgeParty).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance64;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance65).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWeighBridgeParty).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.CmbWeighBridgeParty).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWeighBridgeParty).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWeighBridgeParty).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance68).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWeighBridgeParty).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.CmbWeighBridgeParty).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWeighBridgeParty).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance69).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWeighBridgeParty).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.CmbWeighBridgeParty).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWeighBridgeParty).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.CmbWeighBridgeParty).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWeighBridgeParty).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWeighBridgeParty).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWeighBridgeParty).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWeighBridgeParty.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWeighBridgeParty).Location = new System.Drawing.Point(538, 394);
		((System.Windows.Forms.Control)(object)this.CmbWeighBridgeParty).Name = "CmbWeighBridgeParty";
		((System.Windows.Forms.Control)(object)this.CmbWeighBridgeParty).Size = new System.Drawing.Size(211, 26);
		((System.Windows.Forms.Control)(object)this.CmbWeighBridgeParty).TabIndex = 13;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(426, 400);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(56, 15);
		this.label12.TabIndex = 187;
		this.label12.Text = "Wb Party";
		this.txtOrderNoTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOrderNoTo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOrderNoTo.Location = new System.Drawing.Point(298, 424);
		this.txtOrderNoTo.Name = "txtOrderNoTo";
		this.txtOrderNoTo.ShortcutsEnabled = false;
		this.txtOrderNoTo.Size = new System.Drawing.Size(110, 23);
		this.txtOrderNoTo.TabIndex = 8;
		this.txtOrderNoTo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtOrderNoTo_KeyPress);
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(240, 428);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(44, 15);
		this.label10.TabIndex = 185;
		this.label10.Text = "Po# To";
		this.txtOrderNoFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOrderNoFrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOrderNoFrom.Location = new System.Drawing.Point(125, 424);
		this.txtOrderNoFrom.Name = "txtOrderNoFrom";
		this.txtOrderNoFrom.ShortcutsEnabled = false;
		this.txtOrderNoFrom.Size = new System.Drawing.Size(109, 23);
		this.txtOrderNoFrom.TabIndex = 7;
		this.txtOrderNoFrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtOrderNoFrom_KeyPress);
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(54, 428);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(59, 15);
		this.label11.TabIndex = 184;
		this.label11.Text = "Po# From";
		this.CmbBranch.AllowNull = (DefaultableBoolean)1;
		this.CmbBranch.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBranch.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBranch.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbBranch).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBranch).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBranch).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBranch).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.CmbBranch).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBranch).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbBranch).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBranch).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbBranch).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBranch).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBranch).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBranch).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBranch.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBranch).Location = new System.Drawing.Point(87, 7);
		((System.Windows.Forms.Control)(object)this.CmbBranch).Name = "CmbBranch";
		((System.Windows.Forms.Control)(object)this.CmbBranch).Size = new System.Drawing.Size(283, 26);
		((System.Windows.Forms.Control)(object)this.CmbBranch).TabIndex = 0;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(16, 13);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(44, 15);
		this.label7.TabIndex = 181;
		this.label7.Text = "Branch";
		this.CmbPartyName.AllowNull = (DefaultableBoolean)1;
		this.CmbPartyName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPartyName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPartyName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPartyName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance47;
		((SpecialBoxBase)((UltraGridBase)this.CmbPartyName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance49).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance50).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance51;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance52).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance55).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPartyName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPartyName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPartyName).Location = new System.Drawing.Point(125, 449);
		((System.Windows.Forms.Control)(object)this.CmbPartyName).Name = "CmbPartyName";
		((System.Windows.Forms.Control)(object)this.CmbPartyName).Size = new System.Drawing.Size(283, 26);
		((System.Windows.Forms.Control)(object)this.CmbPartyName).TabIndex = 9;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(54, 455);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(69, 15);
		this.label8.TabIndex = 179;
		this.label8.Text = "Party Name";
		this.CmbWeighBridgeType.AllowNull = (DefaultableBoolean)1;
		this.CmbWeighBridgeType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWeighBridgeType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWeighBridgeType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWeighBridgeType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbWeighBridgeType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWeighBridgeType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWeighBridgeType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWeighBridgeType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance33;
		((SpecialBoxBase)((UltraGridBase)this.CmbWeighBridgeType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance35).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance35).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWeighBridgeType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbWeighBridgeType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWeighBridgeType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWeighBridgeType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWeighBridgeType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbWeighBridgeType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWeighBridgeType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWeighBridgeType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance39).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWeighBridgeType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbWeighBridgeType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWeighBridgeType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWeighBridgeType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWeighBridgeType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.CmbWeighBridgeType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWeighBridgeType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWeighBridgeType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbWeighBridgeType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWeighBridgeType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.CmbWeighBridgeType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWeighBridgeType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWeighBridgeType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWeighBridgeType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWeighBridgeType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWeighBridgeType).Location = new System.Drawing.Point(538, 454);
		((System.Windows.Forms.Control)(object)this.CmbWeighBridgeType).Name = "CmbWeighBridgeType";
		((System.Windows.Forms.Control)(object)this.CmbWeighBridgeType).Size = new System.Drawing.Size(211, 26);
		((System.Windows.Forms.Control)(object)this.CmbWeighBridgeType).TabIndex = 12;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(426, 460);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(104, 15);
		this.label1.TabIndex = 179;
		this.label1.Text = "WeighBridge Type";
		this.CmbGatePassType.AllowNull = (DefaultableBoolean)1;
		this.CmbGatePassType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbGatePassType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbGatePassType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbGatePassType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbGatePassType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbGatePassType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbGatePassType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbGatePassType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance23;
		((SpecialBoxBase)((UltraGridBase)this.CmbGatePassType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbGatePassType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.CmbGatePassType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbGatePassType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbGatePassType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbGatePassType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.CmbGatePassType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbGatePassType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbGatePassType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance67;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance78).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbGatePassType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.CmbGatePassType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbGatePassType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance84).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance84).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance84).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance84).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbGatePassType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance84;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbGatePassType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbGatePassType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbGatePassType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbGatePassType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbGatePassType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbGatePassType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbGatePassType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbGatePassType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbGatePassType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbGatePassType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbGatePassType).Location = new System.Drawing.Point(538, 426);
		((System.Windows.Forms.Control)(object)this.CmbGatePassType).Name = "CmbGatePassType";
		((System.Windows.Forms.Control)(object)this.CmbGatePassType).Size = new System.Drawing.Size(211, 26);
		((System.Windows.Forms.Control)(object)this.CmbGatePassType).TabIndex = 11;
		this.lblLanguage.AutoSize = true;
		this.lblLanguage.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblLanguage.Location = new System.Drawing.Point(426, 432);
		this.lblLanguage.Name = "lblLanguage";
		this.lblLanguage.Size = new System.Drawing.Size(82, 15);
		this.lblLanguage.TabIndex = 177;
		this.lblLanguage.Text = "GatePass Type";
		this.btnshow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(618, 32);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(54, 26);
		this.btnshow.TabIndex = 16;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.GpNoTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.GpNoTo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.GpNoTo.Location = new System.Drawing.Point(618, 6);
		this.GpNoTo.Name = "GpNoTo";
		this.GpNoTo.ShortcutsEnabled = false;
		this.GpNoTo.Size = new System.Drawing.Size(110, 23);
		this.GpNoTo.TabIndex = 6;
		this.GpNoTo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(GpNoTo_KeyPress);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(560, 10);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(38, 15);
		this.label3.TabIndex = 175;
		this.label3.Text = "Gp To";
		this.GPNoFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.GPNoFrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.GPNoFrom.Location = new System.Drawing.Point(445, 6);
		this.GPNoFrom.Name = "GPNoFrom";
		this.GPNoFrom.ShortcutsEnabled = false;
		this.GPNoFrom.Size = new System.Drawing.Size(109, 23);
		this.GPNoFrom.TabIndex = 5;
		this.GPNoFrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(GPNoFrom_KeyPress);
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(374, 10);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(53, 15);
		this.label6.TabIndex = 174;
		this.label6.Text = "Gp From";
		this.ToDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDate.CustomFormat = "dd-MMM-yy";
		this.ToDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDate.Location = new System.Drawing.Point(259, 37);
		this.ToDate.Name = "ToDate";
		this.ToDate.Size = new System.Drawing.Size(110, 23);
		this.ToDate.TabIndex = 2;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(16, 41);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(63, 15);
		this.label4.TabIndex = 171;
		this.label4.Text = "From Date";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(201, 41);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(48, 15);
		this.label5.TabIndex = 170;
		this.label5.Text = "To Date";
		this.TxtVehicleNumber.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.TxtVehicleNumber.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.TxtVehicleNumber.Location = new System.Drawing.Point(443, 32);
		this.TxtVehicleNumber.Name = "TxtVehicleNumber";
		this.TxtVehicleNumber.Size = new System.Drawing.Size(111, 23);
		this.TxtVehicleNumber.TabIndex = 10;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(374, 36);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(67, 15);
		this.label2.TabIndex = 169;
		this.label2.Text = "Vehicle No.";
		this.txtTicketNoTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTicketNoTo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTicketNoTo.Location = new System.Drawing.Point(260, 65);
		this.txtTicketNoTo.Name = "txtTicketNoTo";
		this.txtTicketNoTo.ShortcutsEnabled = false;
		this.txtTicketNoTo.Size = new System.Drawing.Size(110, 23);
		this.txtTicketNoTo.TabIndex = 4;
		this.txtTicketNoTo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtTicketNoTo_KeyPress);
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(202, 69);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(55, 15);
		this.label36.TabIndex = 168;
		this.label36.Text = "Ticket To";
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(16, 69);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(70, 15);
		this.label35.TabIndex = 167;
		this.label35.Text = "Ticket From";
		this.txtTicketNoFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTicketNoFrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTicketNoFrom.Location = new System.Drawing.Point(87, 65);
		this.txtTicketNoFrom.Name = "txtTicketNoFrom";
		this.txtTicketNoFrom.ShortcutsEnabled = false;
		this.txtTicketNoFrom.Size = new System.Drawing.Size(109, 23);
		this.txtTicketNoFrom.TabIndex = 3;
		this.txtTicketNoFrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtTicketNoFrom_KeyPress);
		this.FromDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDate.CustomFormat = "dd-MMM-yy";
		this.FromDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDate.Location = new System.Drawing.Point(87, 37);
		this.FromDate.Name = "FromDate";
		this.FromDate.Size = new System.Drawing.Size(109, 23);
		this.FromDate.TabIndex = 1;
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.label23);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 25);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1199, 30);
		this.panel3.TabIndex = 1;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(9, 6);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(181, 19);
		this.label23.TabIndex = 4;
		this.label23.Text = "Weigh Bridge History";
		this.panel2.Controls.Add(this.toolStrip3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1199, 25);
		this.panel2.TabIndex = 0;
		this.toolStrip3.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnNew, this.toolStripButton1, this.print, this.btnUpdate });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1199, 27);
		this.toolStrip3.TabIndex = 7;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(59, 24);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.toolStripButton1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton1.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton1.Name = "toolStripButton1";
		this.toolStripButton1.Size = new System.Drawing.Size(80, 24);
		this.toolStripButton1.Text = "&Refresh";
		this.toolStripButton1.Click += new System.EventHandler(toolStripButton1_Click);
		this.print.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.print.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.print.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.print.Name = "print";
		this.print.Size = new System.Drawing.Size(115, 24);
		this.print.Text = "281-Register";
		this.print.Click += new System.EventHandler(print_Click);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.check_line;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "Update";
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1199, 749);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "WeighBridge_WeightUpdate";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(frmWeightBridgeHistory_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmWeightBridgeHistory_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbWbStatus).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDocType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWeighBridgeParty).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBranch).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPartyName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWeighBridgeType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbGatePassType).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
