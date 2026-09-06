// using Architecture.Reports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL.Export;
using Architecture.BLL.Reports.Export;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Common;
using Architecture.WinApp.Common;
using Architecture.WinApp.Inventory_Reports;
using Architecture.WinApp.Properties;
using Architecture.WinApp.Service;
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
namespace Architecture.WinApp.ExportReports
{

public class frmExportPendingWorksRegister : Form
{
	private bool ShiftTabPressed;

	private DropDownBind DDL = new DropDownBind();

	public UserAccount UserAccount = new UserAccount();

	private DataTable Data = new DataTable();

	private DataTable DtHistory = new DataTable();

	private IContainer components;

	private Panel panel1;

	private ToolStrip tsItemType;

	private ToolStripButton btnnew;

	internal UltraCombo cmbcontractNo;

	private Label label2;

	internal UltraCombo cmbSupplierName;

	private Label label50;

	private Button btnShow;

	private Label label3;

	private DateTimePicker txtdateto;

	private Label label1;

	private DateTimePicker txtdatefrom;

	private Label label51;

	private CtrlGrdBar ctrlGrdBar1;

	private ToolStripButton BtnPrintRegister;

	internal GridEX grdContractRegister;

	internal UltraCombo cmbperemeter;

	private Label label11;

	private ToolStripButton btnShortCutKey;

	private ToolStripButton btnRefresh;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel4;

	private Panel panel7;

	private Panel panel6;

	private GroupBox groupBox1;

	private RadioButton rdAll;

	private RadioButton rdComplete;

	private RadioButton rdInProcess;

	public frmExportPendingWorksRegister(UserAccount pUserAccount)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUserAccount;
	}

	private void frmExportShipingLineBookingRpt_Load(object sender, EventArgs e)
	{
		try
		{
			ctrlGrdBar1.mGridChooseFielder.Visible = false;
			ctrlGrdBar1.mGridSaveLayouts.Visible = false;
			ctrlGrdBar1.removeLayoutToolStripMenuItem.Visible = false;
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(980, 600);
			ParameterFill();
			ComboFill();
			GridBind();
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
			DDL.BindDDL(dt, cmbperemeter, "Id", "Parameters", "Parameters", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ComboFill()
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Expected O, but got Unknown
		try
		{
			DataTable dtContractNo = new DataTable();
			dtContractNo.Columns.Add("Id");
			dtContractNo.Columns.Add("name");
			DataTable dtCustomer = new DataTable();
			dtCustomer.Columns.Add("Id");
			dtCustomer.Columns.Add("name");
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			Data = ExImLcOrder.GetDataForDropDownFromExportContract(obj);
			if (Data.Rows.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < Data.Rows.Count; i++)
			{
				if (Conversion.ToString(Data.Rows[i]["ActivityType"]) == "ContractNo")
				{
					dtContractNo.Rows.Add(Data.Rows[i]["Id"], Data.Rows[i]["name"]);
				}
				if (Conversion.ToString(Data.Rows[i]["ActivityType"]) == "Customer")
				{
					dtCustomer.Rows.Add(Data.Rows[i]["Id"], Data.Rows[i]["name"]);
				}
			}
			if (dtContractNo.Rows.Count > 0)
			{
				DDL.BindDDL(dtContractNo, cmbcontractNo, "Id", "name", "Contract No", ZeroIndex: false);
				((UltraGridBase)cmbcontractNo).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (dtCustomer.Rows.Count > 0)
			{
				DDL.BindDDL(dtCustomer, cmbSupplierName, "Id", "name", "Customer Name", ZeroIndex: false);
				((UltraGridBase)cmbSupplierName).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
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
			GridBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridBind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.ExImLcOrderId = Conversion.ToInt(cmbcontractNo.Value);
			if (txtdatefrom.Checked)
			{
				obj.FromDate = Conversion.ToDateTime((object)txtdatefrom.Value);
			}
			obj.ToDate = Conversion.ToDateTime((object)txtdateto.Value);
			obj.SupplierCustomerId = Conversion.ToInt(cmbSupplierName.Value);
			if (rdInProcess.Checked)
			{
				obj.ActionId = 1;
			}
			else if (rdComplete.Checked)
			{
				obj.ActionId = 2;
			}
			else if (rdAll.Checked)
			{
				obj.ActionId = 0;
			}
			DtHistory = ExImLcOrder.PendingWorkExportRegister(obj);
			if (DtHistory.Rows.Count > 0)
			{
				DataTable dtcol = new DataTable();
				dtcol.Columns.Add("InvoiceDocumentTypeId", typeof(int));
				dtcol.Columns.Add("InvoiceId", typeof(int));
				dtcol.Columns.Add("InvoiceNo", typeof(string));
				dtcol.Columns.Add("ContractId", typeof(int));
				dtcol.Columns.Add("ContractScheduleId", typeof(int));
				dtcol.Columns.Add("ContractNo", typeof(string));
				dtcol.Columns.Add("SupplierCustomerId", typeof(int));
				dtcol.Columns.Add("CustomerName", typeof(string));
				dtcol.Columns.Add("Product", typeof(string));
				dtcol.Columns.Add("Fcl", typeof(int));
				dtcol.Columns.Add("MTons", typeof(double));
				dtcol.Columns.Add("InspectionId", typeof(int));
				dtcol.Columns.Add("Inspection", typeof(string));
				dtcol.Columns.Add("CROBooking", typeof(string));
				dtcol.Columns.Add("DeliveryOrder", typeof(string));
				dtcol.Columns.Add("Forwarding", typeof(string));
				dtcol.Columns.Add("BillOfLading", typeof(string));
				dtcol.Columns.Add("DocToParty", typeof(string));
				dtcol.Columns.Add("GoodsDeclaration", typeof(string));
				dtcol.Columns.Add("PortPayment", typeof(string));
				dtcol.Columns.Add("PackingMaterial", typeof(string));
				dtcol.Columns.Add("ShipmentExpenses", typeof(string));
				dtcol.Columns.Add("ShipmentClose", typeof(string));
				dtcol.Columns.Add("InvoiceStatus", typeof(string));
				dtcol.Columns.Add("ExportVoucherId", typeof(int));
				dtcol.Columns.Add("NoOfAttachments", typeof(int));
				dtcol.Rows.Clear();
				foreach (DataRow row in DtHistory.Rows)
				{
					dtcol.Rows.Add(row["InvoiceDocumentTypeId"], row["InvoiceId"], row["InvoiceNo"], row["ContractId"], row["ContractScheduleId"], row["ContractNo"], row["SupplierCustomerId"], row["CustomerName"], row["Product"], row["NoOfContainer"], row["MTon"], row["InspectionId"], row["PreShipmentInspectionStatus"], row["BookingInfoStatus"], row["DOStatus"], row["ForwardingStatus"], row["BillOfLadingStatus"], row["DocToParty"], row["GoodsDeclaration"], row["PortPayment"], row["PackingMateialStatus"], row["LogasticsStatus"], row["ShipmentClose"], row["FinalInvoiceStatus"], row["ExportVoucherId"], row["NoOfAttachments"]);
				}
				grdContractRegister.DataSource = dtcol;
				grdContractRegister.RetrieveStructure();
				GridSetting();
			}
			else
			{
				grdContractRegister.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSetting()
	{
		//IL_0722: Unknown result type (might be due to invalid IL or missing references)
		//IL_0728: Expected O, but got Unknown
		//IL_078e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0794: Expected O, but got Unknown
		//IL_07fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0800: Expected O, but got Unknown
		//IL_0866: Unknown result type (might be due to invalid IL or missing references)
		//IL_086c: Expected O, but got Unknown
		//IL_08d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d9: Expected O, but got Unknown
		//IL_0942: Unknown result type (might be due to invalid IL or missing references)
		//IL_0949: Expected O, but got Unknown
		//IL_09b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b9: Expected O, but got Unknown
		//IL_0a22: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a29: Expected O, but got Unknown
		//IL_0a92: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a99: Expected O, but got Unknown
		//IL_0b02: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b09: Expected O, but got Unknown
		//IL_0b72: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b79: Expected O, but got Unknown
		//IL_0be2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be9: Expected O, but got Unknown
		//IL_0c52: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c59: Expected O, but got Unknown
		//IL_0cc2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cc9: Expected O, but got Unknown
		//IL_0d32: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d39: Expected O, but got Unknown
		//IL_0da2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0da9: Expected O, but got Unknown
		//IL_0e12: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e19: Expected O, but got Unknown
		//IL_0e82: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e89: Expected O, but got Unknown
		//IL_0ef2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ef9: Expected O, but got Unknown
		//IL_0f62: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f69: Expected O, but got Unknown
		//IL_0fd2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fd9: Expected O, but got Unknown
		//IL_1042: Unknown result type (might be due to invalid IL or missing references)
		//IL_1049: Expected O, but got Unknown
		//IL_10b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_10b9: Expected O, but got Unknown
		//IL_1122: Unknown result type (might be due to invalid IL or missing references)
		//IL_1129: Expected O, but got Unknown
		//IL_1192: Unknown result type (might be due to invalid IL or missing references)
		//IL_1199: Expected O, but got Unknown
		//IL_1202: Unknown result type (might be due to invalid IL or missing references)
		//IL_1209: Expected O, but got Unknown
		try
		{
			GridEX_Helper.GridWrappingAndColumnSettings(grdContractRegister, 2);
			grdContractRegister.RootTable.Columns["Inspection"].ColumnType = (ColumnType)5;
			grdContractRegister.RootTable.Columns["CROBooking"].ColumnType = (ColumnType)5;
			grdContractRegister.RootTable.Columns["GoodsDeclaration"].ColumnType = (ColumnType)5;
			grdContractRegister.RootTable.Columns["PortPayment"].ColumnType = (ColumnType)5;
			grdContractRegister.RootTable.Columns["DeliveryOrder"].ColumnType = (ColumnType)5;
			grdContractRegister.RootTable.Columns["Forwarding"].ColumnType = (ColumnType)5;
			grdContractRegister.RootTable.Columns["BillOfLading"].ColumnType = (ColumnType)5;
			grdContractRegister.RootTable.Columns["DocToParty"].ColumnType = (ColumnType)5;
			grdContractRegister.RootTable.Columns["PackingMaterial"].ColumnType = (ColumnType)5;
			grdContractRegister.RootTable.Columns["ShipmentClose"].ColumnType = (ColumnType)5;
			grdContractRegister.RootTable.Columns["ShipmentExpenses"].ColumnType = (ColumnType)5;
			grdContractRegister.RootTable.Columns["InvoiceStatus"].ColumnType = (ColumnType)5;
			grdContractRegister.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdContractRegister.RootTable.Columns["InvoiceNo"].Width = Constants.ExportConstants.InvoiceNoConstant;
			grdContractRegister.RootTable.Columns["ContractNo"].Width = Constants.ExportConstants.ContractNoConstant;
			grdContractRegister.RootTable.Columns["CustomerName"].Width = Constants.ExportConstants.StringConstant;
			grdContractRegister.RootTable.Columns["Product"].Width = Constants.ExportConstants.StringConstant;
			grdContractRegister.RootTable.Columns["Fcl"].Width = Constants.ExportConstants.FclConstant;
			grdContractRegister.RootTable.Columns["MTons"].Width = Constants.ExportConstants.FclConstant + 8;
			grdContractRegister.RootTable.Columns["Inspection"].Width = Constants.ExportConstants.StatusConstant;
			grdContractRegister.RootTable.Columns["CROBooking"].Width = Constants.ExportConstants.StatusConstant;
			grdContractRegister.RootTable.Columns["GoodsDeclaration"].Width = Constants.ExportConstants.StatusConstant;
			grdContractRegister.RootTable.Columns["PortPayment"].Width = Constants.ExportConstants.StatusConstant;
			grdContractRegister.RootTable.Columns["DeliveryOrder"].Width = Constants.ExportConstants.StatusConstant;
			grdContractRegister.RootTable.Columns["Forwarding"].Width = Constants.ExportConstants.StatusConstant;
			grdContractRegister.RootTable.Columns["BillOfLading"].Width = Constants.ExportConstants.StatusConstant;
			grdContractRegister.RootTable.Columns["DocToParty"].Width = Constants.ExportConstants.StatusConstant;
			grdContractRegister.RootTable.Columns["PackingMaterial"].Width = Constants.ExportConstants.StatusConstant;
			grdContractRegister.RootTable.Columns["ShipmentClose"].Width = Constants.ExportConstants.StatusConstant;
			grdContractRegister.RootTable.Columns["ShipmentExpenses"].Width = Constants.ExportConstants.StatusConstant;
			grdContractRegister.RootTable.Columns["InvoiceStatus"].Width = Constants.ExportConstants.StatusConstant;
			grdContractRegister.RootTable.Columns["InvoiceDocumentTypeId"].Visible = false;
			grdContractRegister.RootTable.Columns["InvoiceId"].Visible = false;
			grdContractRegister.RootTable.Columns["InspectionId"].Visible = false;
			grdContractRegister.RootTable.Columns["SupplierCustomerId"].Visible = false;
			grdContractRegister.RootTable.Columns["ContractScheduleId"].Visible = false;
			grdContractRegister.RootTable.Columns["ContractId"].Visible = false;
			grdContractRegister.RootTable.Columns["PortPayment"].Visible = false;
			grdContractRegister.RootTable.Columns["ExportVoucherId"].Visible = false;
			grdContractRegister.RootTable.Columns["GoodsDeclaration"].Caption = "GD/FI";
			grdContractRegister.RootTable.Columns["CROBooking"].Caption = "CRO Booking";
			grdContractRegister.RootTable.Columns["ShipmentClose"].Caption = "Reached At Destination Port";
			grdContractRegister.RootTable.Columns["DocToParty"].Caption = "Document To Party";
			grdContractRegister.RootTable.Columns["InvoiceStatus"].Caption = "Export Voucher";
			grdContractRegister.RootTable.Columns["MTons"].AggregateFunction = (AggregateFunction)2;
			grdContractRegister.RootTable.Columns["MTons"].TextAlignment = (TextAlignment)3;
			grdContractRegister.RootTable.Columns["MTons"].FormatString = "#,#.###";
			grdContractRegister.RootTable.Columns["MTons"].TotalFormatString = "#,#.###";
			grdContractRegister.RootTable.Columns["Fcl"].AggregateFunction = (AggregateFunction)2;
			grdContractRegister.RootTable.Columns["Fcl"].TextAlignment = (TextAlignment)3;
			grdContractRegister.RootTable.Columns["Fcl"].FormatString = "#,#.###";
			grdContractRegister.RootTable.Columns["Fcl"].TotalFormatString = "#,#.###";
			GridEXFormatCondition Inspection = new GridEXFormatCondition(grdContractRegister.RootTable.Columns["Inspection"], (ConditionOperator)0, (object)"Pending");
			Inspection.TargetColumn = grdContractRegister.RootTable.Columns["Inspection"];
			Inspection.FormatStyle.ForeColor = Color.Red;
			grdContractRegister.RootTable.FormatConditions.Add(Inspection);
			GridEXFormatCondition Inspection2 = new GridEXFormatCondition(grdContractRegister.RootTable.Columns["Inspection"], (ConditionOperator)0, (object)"Complete");
			Inspection2.TargetColumn = grdContractRegister.RootTable.Columns["Inspection"];
			Inspection2.FormatStyle.ForeColor = Color.DarkGreen;
			grdContractRegister.RootTable.FormatConditions.Add(Inspection2);
			GridEXFormatCondition CROBooking = new GridEXFormatCondition(grdContractRegister.RootTable.Columns["CROBooking"], (ConditionOperator)0, (object)"Pending");
			CROBooking.TargetColumn = grdContractRegister.RootTable.Columns["CROBooking"];
			CROBooking.FormatStyle.ForeColor = Color.Red;
			grdContractRegister.RootTable.FormatConditions.Add(CROBooking);
			GridEXFormatCondition CROBooking2 = new GridEXFormatCondition(grdContractRegister.RootTable.Columns["CROBooking"], (ConditionOperator)0, (object)"Complete");
			CROBooking2.TargetColumn = grdContractRegister.RootTable.Columns["CROBooking"];
			CROBooking2.FormatStyle.ForeColor = Color.DarkGreen;
			grdContractRegister.RootTable.FormatConditions.Add(CROBooking2);
			GridEXFormatCondition BillOfLading = new GridEXFormatCondition(grdContractRegister.RootTable.Columns["BillOfLading"], (ConditionOperator)0, (object)"Pending");
			BillOfLading.TargetColumn = grdContractRegister.RootTable.Columns["BillOfLading"];
			BillOfLading.FormatStyle.ForeColor = Color.Red;
			grdContractRegister.RootTable.FormatConditions.Add(BillOfLading);
			GridEXFormatCondition BillOfLading2 = new GridEXFormatCondition(grdContractRegister.RootTable.Columns["BillOfLading"], (ConditionOperator)0, (object)"Complete");
			BillOfLading2.TargetColumn = grdContractRegister.RootTable.Columns["BillOfLading"];
			BillOfLading2.FormatStyle.ForeColor = Color.DarkGreen;
			grdContractRegister.RootTable.FormatConditions.Add(BillOfLading2);
			GridEXFormatCondition DeliveryOrder = new GridEXFormatCondition(grdContractRegister.RootTable.Columns["DeliveryOrder"], (ConditionOperator)0, (object)"Pending");
			DeliveryOrder.TargetColumn = grdContractRegister.RootTable.Columns["DeliveryOrder"];
			DeliveryOrder.FormatStyle.ForeColor = Color.Red;
			grdContractRegister.RootTable.FormatConditions.Add(DeliveryOrder);
			GridEXFormatCondition DeliveryOrder2 = new GridEXFormatCondition(grdContractRegister.RootTable.Columns["DeliveryOrder"], (ConditionOperator)0, (object)"Complete");
			DeliveryOrder2.TargetColumn = grdContractRegister.RootTable.Columns["DeliveryOrder"];
			DeliveryOrder2.FormatStyle.ForeColor = Color.DarkGreen;
			grdContractRegister.RootTable.FormatConditions.Add(DeliveryOrder2);
			GridEXFormatCondition DeliveryOrder3 = new GridEXFormatCondition(grdContractRegister.RootTable.Columns["DeliveryOrder"], (ConditionOperator)0, (object)"InProcess");
			DeliveryOrder3.TargetColumn = grdContractRegister.RootTable.Columns["DeliveryOrder"];
			DeliveryOrder3.FormatStyle.ForeColor = Color.Orange;
			grdContractRegister.RootTable.FormatConditions.Add(DeliveryOrder3);
			GridEXFormatCondition Forwarding = new GridEXFormatCondition(grdContractRegister.RootTable.Columns["Forwarding"], (ConditionOperator)0, (object)"Pending");
			Forwarding.TargetColumn = grdContractRegister.RootTable.Columns["Forwarding"];
			Forwarding.FormatStyle.ForeColor = Color.Red;
			grdContractRegister.RootTable.FormatConditions.Add(Forwarding);
			GridEXFormatCondition Forwarding2 = new GridEXFormatCondition(grdContractRegister.RootTable.Columns["Forwarding"], (ConditionOperator)0, (object)"Complete");
			Forwarding2.TargetColumn = grdContractRegister.RootTable.Columns["Forwarding"];
			Forwarding2.FormatStyle.ForeColor = Color.DarkGreen;
			grdContractRegister.RootTable.FormatConditions.Add(Forwarding2);
			GridEXFormatCondition Forwarding3 = new GridEXFormatCondition(grdContractRegister.RootTable.Columns["Forwarding"], (ConditionOperator)0, (object)"InProcess");
			Forwarding3.TargetColumn = grdContractRegister.RootTable.Columns["Forwarding"];
			Forwarding3.FormatStyle.ForeColor = Color.Orange;
			grdContractRegister.RootTable.FormatConditions.Add(Forwarding3);
			GridEXFormatCondition logistics = new GridEXFormatCondition(grdContractRegister.RootTable.Columns["ShipmentExpenses"], (ConditionOperator)0, (object)"pending");
			logistics.TargetColumn = grdContractRegister.RootTable.Columns["ShipmentExpenses"];
			logistics.FormatStyle.ForeColor = Color.Red;
			grdContractRegister.RootTable.FormatConditions.Add(logistics);
			GridEXFormatCondition Logistics1 = new GridEXFormatCondition(grdContractRegister.RootTable.Columns["ShipmentExpenses"], (ConditionOperator)0, (object)"Complete");
			Logistics1.TargetColumn = grdContractRegister.RootTable.Columns["ShipmentExpenses"];
			Logistics1.FormatStyle.ForeColor = Color.DarkGreen;
			grdContractRegister.RootTable.FormatConditions.Add(Logistics1);
			GridEXFormatCondition PackingMaterial = new GridEXFormatCondition(grdContractRegister.RootTable.Columns["PackingMaterial"], (ConditionOperator)0, (object)"Pending");
			PackingMaterial.TargetColumn = grdContractRegister.RootTable.Columns["PackingMaterial"];
			PackingMaterial.FormatStyle.ForeColor = Color.Red;
			grdContractRegister.RootTable.FormatConditions.Add(PackingMaterial);
			GridEXFormatCondition PackingMaterial2 = new GridEXFormatCondition(grdContractRegister.RootTable.Columns["PackingMaterial"], (ConditionOperator)0, (object)"Complete");
			PackingMaterial2.TargetColumn = grdContractRegister.RootTable.Columns["PackingMaterial"];
			PackingMaterial2.FormatStyle.ForeColor = Color.DarkGreen;
			grdContractRegister.RootTable.FormatConditions.Add(PackingMaterial2);
			GridEXFormatCondition GoodsDeclaration = new GridEXFormatCondition(grdContractRegister.RootTable.Columns["GoodsDeclaration"], (ConditionOperator)0, (object)"Pending");
			GoodsDeclaration.TargetColumn = grdContractRegister.RootTable.Columns["GoodsDeclaration"];
			GoodsDeclaration.FormatStyle.ForeColor = Color.Red;
			grdContractRegister.RootTable.FormatConditions.Add(GoodsDeclaration);
			GridEXFormatCondition GoodsDeclaration2 = new GridEXFormatCondition(grdContractRegister.RootTable.Columns["GoodsDeclaration"], (ConditionOperator)0, (object)"Complete");
			GoodsDeclaration2.TargetColumn = grdContractRegister.RootTable.Columns["GoodsDeclaration"];
			GoodsDeclaration2.FormatStyle.ForeColor = Color.DarkGreen;
			grdContractRegister.RootTable.FormatConditions.Add(GoodsDeclaration2);
			GridEXFormatCondition PortPayment = new GridEXFormatCondition(grdContractRegister.RootTable.Columns["PortPayment"], (ConditionOperator)0, (object)"Pending");
			PortPayment.TargetColumn = grdContractRegister.RootTable.Columns["PortPayment"];
			PortPayment.FormatStyle.ForeColor = Color.Red;
			grdContractRegister.RootTable.FormatConditions.Add(PortPayment);
			GridEXFormatCondition PortPayment2 = new GridEXFormatCondition(grdContractRegister.RootTable.Columns["PortPayment"], (ConditionOperator)0, (object)"Complete");
			PortPayment2.TargetColumn = grdContractRegister.RootTable.Columns["PortPayment"];
			PortPayment2.FormatStyle.ForeColor = Color.DarkGreen;
			grdContractRegister.RootTable.FormatConditions.Add(PortPayment2);
			GridEXFormatCondition InvoiceStatus2 = new GridEXFormatCondition(grdContractRegister.RootTable.Columns["InvoiceStatus"], (ConditionOperator)0, (object)"Pending");
			InvoiceStatus2.TargetColumn = grdContractRegister.RootTable.Columns["InvoiceStatus"];
			InvoiceStatus2.FormatStyle.ForeColor = Color.Red;
			grdContractRegister.RootTable.FormatConditions.Add(InvoiceStatus2);
			GridEXFormatCondition InvoiceStatus = new GridEXFormatCondition(grdContractRegister.RootTable.Columns["InvoiceStatus"], (ConditionOperator)0, (object)"Complete");
			InvoiceStatus.TargetColumn = grdContractRegister.RootTable.Columns["InvoiceStatus"];
			InvoiceStatus.FormatStyle.ForeColor = Color.DarkGreen;
			grdContractRegister.RootTable.FormatConditions.Add(InvoiceStatus);
			GridEXFormatCondition DocToParty = new GridEXFormatCondition(grdContractRegister.RootTable.Columns["DocToParty"], (ConditionOperator)0, (object)"Pending");
			DocToParty.TargetColumn = grdContractRegister.RootTable.Columns["DocToParty"];
			DocToParty.FormatStyle.ForeColor = Color.Red;
			grdContractRegister.RootTable.FormatConditions.Add(DocToParty);
			GridEXFormatCondition DocToParty2 = new GridEXFormatCondition(grdContractRegister.RootTable.Columns["DocToParty"], (ConditionOperator)0, (object)"Complete");
			DocToParty2.TargetColumn = grdContractRegister.RootTable.Columns["DocToParty"];
			DocToParty2.FormatStyle.ForeColor = Color.DarkGreen;
			grdContractRegister.RootTable.FormatConditions.Add(DocToParty2);
			GridEXFormatCondition ShipmentClose = new GridEXFormatCondition(grdContractRegister.RootTable.Columns["ShipmentClose"], (ConditionOperator)0, (object)"Open");
			ShipmentClose.TargetColumn = grdContractRegister.RootTable.Columns["ShipmentClose"];
			ShipmentClose.FormatStyle.ForeColor = Color.Red;
			grdContractRegister.RootTable.FormatConditions.Add(ShipmentClose);
			GridEXFormatCondition ShipmentClose2 = new GridEXFormatCondition(grdContractRegister.RootTable.Columns["ShipmentClose"], (ConditionOperator)0, (object)"In-Transit");
			ShipmentClose2.TargetColumn = grdContractRegister.RootTable.Columns["ShipmentClose"];
			ShipmentClose2.FormatStyle.ForeColor = Color.DarkGreen;
			grdContractRegister.RootTable.FormatConditions.Add(ShipmentClose2);
			ctrlGrdBar1_Load(null, null);
			if (UserAccount.RoleName == "Admin")
			{
				GridEX_Helper.AddButton(grdContractRegister, "CompleteStatus", "Complete Status", 120);
				GridEX_Helper.AddButton(grdContractRegister, "OpenStatus", "Open Status", 120);
				GridEX_Helper.AddButton(grdContractRegister, "Print", "Print", 50);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdfrm_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Expected O, but got Unknown
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Expected O, but got Unknown
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Expected O, but got Unknown
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdContractRegister.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0)
			{
				return;
			}
			if (e.Column.Key == "Print")
			{
				DataTable dt = new DataTable();
				ReportsParameters obj = new ReportsParameters();
				obj.OrganizationId = UserAccount.OrganizationId;
				obj.CompanyId = UserAccount.CompanyId;
				obj.Id = Conversion.ToInt(r.Cells["ContractId"].Value);
				obj.ApprovedFilter = "All";
				dt = SaleContractReports.SaleContractReports501(obj);
				if (dt == null || dt.Rows.Count == 0)
				{
					MessageBox.Show("No Record Found For Display");
				}
				else
				{
					List<SubReportObject> lstSubReports = new List<SubReportObject>();
					SubReportObject subReportObject1 = new SubReportObject(SaleContractReports.LcorderOtherItem_SubRpt501(obj.Id), "LcorderOtherItem_SubRpt.rpt");
					lstSubReports.Add(subReportObject1);
					SubReportObject subReportObject2 = new SubReportObject(SaleContractReports.ExImLcOrderPaymentTermsDetail_SubRpt501(obj.Id), "ExImLcOrderPaymentTermsDetail_SubRpt.rpt");
					lstSubReports.Add(subReportObject2);
					SubReportObject subReportObject3 = new SubReportObject(SaleContractReports.ExImLcContractPackingMaterialDetail_SubRpt501(obj.Id), "ExImLcContractPackingMaterialDetail_SubRpt.rpt");
					lstSubReports.Add(subReportObject3);
					Reporting val = new Reporting();
					val.RptPerameter("CompanyAddress", UserAccount.CompAddress.ToString());
					val.RptPerameter("CompanyName", UserAccount.CompName.ToString());
					val.ShowReportWithDataTableSubReprt(dt, "501-ExportSalesContractExportNew.rpt", lstSubReports);
				}
			}
			if (e.Column.Key == "CompleteStatus")
			{
				int num = Conversion.ToInt(r.Cells["InvoiceId"].Value);
				string status2 = "Complete";
				int userid2 = UserAccount.ID;
				ExImInvoice.ExportInvoice_StatusUpdate(num, userid2, status2);
				MessageBox.Show("Update Complete successfully...");
				GridBind();
			}
			if (e.Column.Key == "OpenStatus")
			{
				int num2 = Conversion.ToInt(r.Cells["InvoiceId"].Value);
				string status = "Open";
				int userid = UserAccount.ID;
				ExImInvoice.ExportInvoice_StatusUpdate(num2, userid, status);
				MessageBox.Show("Update Open successfully...");
				GridBind();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdContractRegister_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Expected O, but got Unknown
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdContractRegister.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0)
			{
				return;
			}
			int InvoiceId = Conversion.ToInt(r.Cells["InvoiceId"].Value);
			int InvoiceDocumentTypeId = Conversion.ToInt(r.Cells["InvoiceDocumentTypeId"].Value);
			int InspectionId = Conversion.ToInt(r.Cells["InspectionId"].Value);
			Conversion.ToInt(r.Cells["ContractId"].Value);
			int ExportVoucherId = Conversion.ToInt(r.Cells["ExportVoucherId"].Value);
			int DateTypeId = Conversion.ToInt(cmbperemeter.Value);
			DateTime FromDate = Conversion.ToDateTime((object)txtdatefrom.Value);
			DateTime ToDate = Conversion.ToDateTime((object)txtdateto.Value);
			if (e.Column.Key == "Inspection" && Conversion.ToString(r.Cells["Inspection"].Value) != "Pending")
			{
				CommonServices.InvLabPreProductionExportLotInspection_Slip(InspectionId);
			}
			if (e.Column.Key == "CROBooking" && Conversion.ToString(r.Cells["CROBooking"].Value) != "Pending")
			{
				DataTable dt = new DataTable();
				dt = ExImExportShipingLineBooking.PrintSlipandRegister(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					ExImInvoiceId = InvoiceId
				});
				if (dt == null || dt.Rows.Count == 0)
				{
					MessageBox.Show("No Record Found For Display");
				}
				else
				{
					Reporting val = new Reporting();
					val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
					val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
					val.ShowReportWithDataTable(dt, "560-ExBooking Info(CRO).rpt");
				}
			}
			if (e.Column.Key == "BillOfLading" && Conversion.ToString(r.Cells["BillOfLading"].Value) != "Pending")
			{
				CommonServices.ExImBillOfLading_Slip(InvoiceId);
			}
			if (e.Column.Key == "DeliveryOrder" && Conversion.ToString(r.Cells["DeliveryOrder"].Value) != "Pending")
			{
				DeliveryOrderHistory deliveryOrderHistory = new DeliveryOrderHistory(UserAccount);
				deliveryOrderHistory.DateTypeId = DateTypeId;
				deliveryOrderHistory.DOType = "Export";
				deliveryOrderHistory.FromDate = FromDate;
				deliveryOrderHistory.ToDate = ToDate;
				deliveryOrderHistory.InvoiceId = InvoiceId;
				deliveryOrderHistory.ShowDialog();
			}
			if (e.Column.Key == "Forwarding" && Conversion.ToString(r.Cells["Forwarding"].Value) != "Pending")
			{
				ExImForwardingHistory obj = new ExImForwardingHistory(UserAccount)
				{
					txtdatefrom = 
					{
						Value = FromDate
					},
					txtdateto = 
					{
						Value = ToDate
					}
				};
				obj.cmbInvoiceNo.Value = InvoiceId;
				obj.btnShow_Click(null, null);
				obj.Show();
			}
			if (e.Column.Key == "ShipmentExpenses" && Conversion.ToString(r.Cells["ShipmentExpenses"].Value) != "Pending" && CommonServices.SetRightsValueInRightsObject("frmServiceBillHistory").DoHaveViewRight)
			{
				frmServiceBillHistory frmServiceBillHistory = new frmServiceBillHistory(UserAccount);
				frmServiceBillHistory.cmbperemeter.Value = DateTypeId;
				frmServiceBillHistory.datFromDate.Value = FromDate;
				frmServiceBillHistory.dateToDate.Value = ToDate;
				frmServiceBillHistory.cmbInvoiceNo.Value = InvoiceId;
				frmServiceBillHistory.btnshow_Click(null, null);
				frmServiceBillHistory.Show();
			}
			if (e.Column.Key == "PackingMaterial" && Conversion.ToString(r.Cells["PackingMaterial"].Value) != "Pending")
			{
				CommonServices.StoreIssuanceHeader_Slip475(0, InvoiceId);
			}
			if (e.Column.Key == "InvoiceStatus" && Conversion.ToString(r.Cells["InvoiceStatus"].Value) != "Pending" && UserAccount.RoleName == "Admin")
			{
				CommonServices.ExportInvoiceVoucher_Slip(ExportVoucherId);
			}
			if (e.Column.Key == "NoOfAttachments")
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(InvoiceId, InvoiceDocumentTypeId);
			}
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
			((Control)(object)cmbcontractNo).Text = string.Empty;
			((Control)(object)cmbSupplierName).Text = string.Empty;
			txtdatefrom.Focus();
			GridBind();
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
			ComboFill();
			Reset();
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

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			ComboFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShortCutKey_Click(object sender, EventArgs e)
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

	private void frmExportShipingLineBookingRpt_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnnew_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.R)
			{
				btnRefresh_Click(null, null);
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				cmbperemeter.Focus();
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				btnShow_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grdContractRegister).Focus();
			}
			if (e.Control && e.KeyCode == Keys.P)
			{
				BtnPrintRegister_Click(null, null);
			}
			if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
			}
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
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+F5", "For Focus on Date Type");
			dt.Rows.Add("Ctrl+S", "For Showing Data");
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Grid");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
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

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdContractRegister).Name);
			if (fs != null)
			{
				grdContractRegister.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void BtnPrintRegister_Click(object sender, EventArgs e)
	{
		try
		{
			ShowRegister();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void ShowRegister()
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (DtHistory == null || DtHistory.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.RptPerameter("@PrintedBy", UserAccount.UserName.ToString());
			val.ShowReportWithDataTable(DtHistory, "555-PendingWorkExportRegister.rpt");
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
				txtdatefrom.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 2)
			{
				txtdatefrom.Value = DateTime.Today.AddDays(-7.0);
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 3)
			{
				DateTime now = DateTime.UtcNow;
				DateTime first = now.Date.AddDays(-(now.Date.Day - 1));
				txtdatefrom.Value = first;
				txtdateto.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 4)
			{
				int year = DateTime.Now.Year;
				DateTime first2 = new DateTime(year, 1, 1);
				txtdatefrom.Value = first2;
				txtdateto.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 5)
			{
				txtdatefrom.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShow_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
	{
		try
		{
			if (e.Shift && e.KeyCode == Keys.Tab)
			{
				ShiftTabPressed = true;
			}
			else
			{
				ShiftTabPressed = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShow_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl != null && base.ActiveControl.Name == "cmbperemeter")
			{
				cmbperemeter.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdContractRegister_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Expected O, but got Unknown
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Expected O, but got Unknown
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Expected O, but got Unknown
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Expected O, but got Unknown
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdContractRegister.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0)
			{
				return;
			}
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.Space && grdContractRegister.CurrentColumn.Key == "Print")
			{
				int Id = Conversion.ToInt(grdContractRegister.CurrentRow.Cells["ContractNo"].Value);
				DataTable dt = new DataTable();
				ReportsParameters obj = new ReportsParameters();
				obj.OrganizationId = UserAccount.OrganizationId;
				obj.CompanyId = UserAccount.CompanyId;
				obj.Id = Id;
				obj.ApprovedFilter = Conversion.ToString((object)"All");
				dt = ExportPdfReport.ExpRptSalesContractExport_501(obj);
				if (dt == null || dt.Rows.Count == 0)
				{
					MessageBox.Show("No Record Found For Display");
					return;
				}
				List<SubReportObject> lstSubReports = new List<SubReportObject>();
				SubReportObject subReportObject1 = new SubReportObject(SaleContractReports.LcorderOtherItem_SubRpt501(obj.Id), "LcorderOtherItem_SubRpt.rpt");
				lstSubReports.Add(subReportObject1);
				SubReportObject subReportObject2 = new SubReportObject(SaleContractReports.ExImLcOrderPaymentTermsDetail_SubRpt501(obj.Id), "ExImLcOrderPaymentTermsDetail_SubRpt.rpt");
				lstSubReports.Add(subReportObject2);
				SubReportObject subReportObject3 = new SubReportObject(SaleContractReports.ExImLcContractPackingMaterialDetail_SubRpt501(obj.Id), "ExImLcContractPackingMaterialDetail_SubRpt.rpt");
				lstSubReports.Add(subReportObject3);
				Reporting val = new Reporting();
				val.RptPerameter("CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTableSubReprt(dt, "501-ExportSalesContractExportNew.rpt", lstSubReports);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmExportPendingWorksRegister_FormClosed(object sender, FormClosedEventArgs e)
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
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
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
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Expected O, but got Unknown
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Expected O, but got Unknown
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Expected O, but got Unknown
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Expected O, but got Unknown
		//IL_05b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c2: Expected O, but got Unknown
		//IL_05cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d9: Expected O, but got Unknown
		GridEXLayout grdContractRegister_DesignTimeLayout = new GridEXLayout();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.ExportReports.frmExportPendingWorksRegister));
		Appearance appearance13 = new Appearance();
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
		Appearance appearance24 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
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
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.grdContractRegister = new GridEX();
		this.panel7 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel6 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.rdAll = new System.Windows.Forms.RadioButton();
		this.rdComplete = new System.Windows.Forms.RadioButton();
		this.rdInProcess = new System.Windows.Forms.RadioButton();
		this.cmbperemeter = new UltraCombo();
		this.label50 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.cmbSupplierName = new UltraCombo();
		this.txtdateto = new System.Windows.Forms.DateTimePicker();
		this.label2 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.cmbcontractNo = new UltraCombo();
		this.txtdatefrom = new System.Windows.Forms.DateTimePicker();
		this.btnShow = new System.Windows.Forms.Button();
		this.label51 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label3 = new System.Windows.Forms.Label();
		this.tsItemType = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.BtnPrintRegister = new System.Windows.Forms.ToolStripButton();
		this.btnShortCutKey = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdContractRegister).BeginInit();
		this.panel7.SuspendLayout();
		this.panel6.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbperemeter).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSupplierName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcontractNo).BeginInit();
		this.panel4.SuspendLayout();
		this.tsItemType.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1017, 633);
		this.panel1.TabIndex = 0;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1017, 633);
		this.tabControl1.TabIndex = 3;
		this.tabPage1.Controls.Add((System.Windows.Forms.Control)(object)this.grdContractRegister);
		this.tabPage1.Controls.Add(this.panel7);
		this.tabPage1.Controls.Add(this.panel6);
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Location = new System.Drawing.Point(4, 25);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1009, 604);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Pending Work Register";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.grdContractRegister.AllowEdit = (InheritableBoolean)2;
		grdContractRegister_DesignTimeLayout.LayoutString = resources.GetString("grdContractRegister_DesignTimeLayout.LayoutString");
		this.grdContractRegister.DesignTimeLayout = grdContractRegister_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdContractRegister).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdContractRegister.DynamicFiltering = true;
		this.grdContractRegister.FilterMode = (FilterMode)2;
		this.grdContractRegister.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdContractRegister).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdContractRegister.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdContractRegister).Location = new System.Drawing.Point(3, 177);
		((System.Windows.Forms.Control)(object)this.grdContractRegister).Name = "grdContractRegister";
		((ScrollableBase)this.grdContractRegister).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdContractRegister).Size = new System.Drawing.Size(1003, 424);
		((System.Windows.Forms.Control)(object)this.grdContractRegister).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.grdContractRegister).TabStop = false;
		this.grdContractRegister.TotalRow = (InheritableBoolean)1;
		this.grdContractRegister.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdContractRegister).VisualStyle = (VisualStyle)4;
		this.grdContractRegister.ColumnButtonClick += new ColumnActionEventHandler(grdfrm_ColumnButtonClick);
		this.grdContractRegister.LinkClicked += new ColumnActionEventHandler(grdContractRegister_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdContractRegister).KeyDown += new System.Windows.Forms.KeyEventHandler(grdContractRegister_KeyDown);
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.ctrlGrdBar1);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(3, 149);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1003, 28);
		this.panel7.TabIndex = 4;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(947, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar1.MyGrid = this.grdContractRegister;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(49, 23);
		this.ctrlGrdBar1.TabIndex = 0;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.panel6.Controls.Add(this.groupBox1);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(3, 60);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1003, 89);
		this.panel6.TabIndex = 3;
		this.groupBox1.Controls.Add(this.rdAll);
		this.groupBox1.Controls.Add(this.rdComplete);
		this.groupBox1.Controls.Add(this.rdInProcess);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbperemeter);
		this.groupBox1.Controls.Add(this.label50);
		this.groupBox1.Controls.Add(this.label11);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbSupplierName);
		this.groupBox1.Controls.Add(this.txtdateto);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbcontractNo);
		this.groupBox1.Controls.Add(this.txtdatefrom);
		this.groupBox1.Controls.Add(this.btnShow);
		this.groupBox1.Controls.Add(this.label51);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(2, 3);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(970, 83);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.rdAll.AutoSize = true;
		this.rdAll.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdAll.Location = new System.Drawing.Point(919, 20);
		this.rdAll.Name = "rdAll";
		this.rdAll.Size = new System.Drawing.Size(39, 19);
		this.rdAll.TabIndex = 589;
		this.rdAll.Text = "All";
		this.rdAll.UseVisualStyleBackColor = true;
		this.rdComplete.AutoSize = true;
		this.rdComplete.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdComplete.Location = new System.Drawing.Point(841, 20);
		this.rdComplete.Name = "rdComplete";
		this.rdComplete.Size = new System.Drawing.Size(76, 19);
		this.rdComplete.TabIndex = 588;
		this.rdComplete.Text = "Complete";
		this.rdComplete.UseVisualStyleBackColor = true;
		this.rdInProcess.AutoSize = true;
		this.rdInProcess.Checked = true;
		this.rdInProcess.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdInProcess.Location = new System.Drawing.Point(762, 20);
		this.rdInProcess.Name = "rdInProcess";
		this.rdInProcess.Size = new System.Drawing.Size(76, 19);
		this.rdInProcess.TabIndex = 587;
		this.rdInProcess.TabStop = true;
		this.rdInProcess.Text = "InProcess";
		this.rdInProcess.UseVisualStyleBackColor = true;
		this.cmbperemeter.AllowNull = (DefaultableBoolean)1;
		this.cmbperemeter.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbperemeter.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbperemeter.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Appearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbperemeter.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Location = new System.Drawing.Point(106, 16);
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Name = "cmbperemeter";
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Size = new System.Drawing.Size(246, 26);
		((System.Windows.Forms.Control)(object)this.cmbperemeter).TabIndex = 0;
		this.cmbperemeter.ValueChanged += new System.EventHandler(cmbperemeter_ValueChanged);
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label50.Location = new System.Drawing.Point(355, 50);
		this.label50.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(93, 15);
		this.label50.TabIndex = 16;
		this.label50.Text = "Customer Name";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(3, 22);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(60, 15);
		this.label11.TabIndex = 586;
		this.label11.Text = "Date Type";
		this.cmbSupplierName.AllowNull = (DefaultableBoolean)1;
		this.cmbSupplierName.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbSupplierName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbSupplierName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance44).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance44).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbSupplierName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance45;
		((SpecialBoxBase)((UltraGridBase)this.cmbSupplierName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance46).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance47;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance26).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbSupplierName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbSupplierName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbSupplierName).Location = new System.Drawing.Point(449, 44);
		((System.Windows.Forms.Control)(object)this.cmbSupplierName).Name = "cmbSupplierName";
		((System.Windows.Forms.Control)(object)this.cmbSupplierName).Size = new System.Drawing.Size(306, 26);
		((System.Windows.Forms.Control)(object)this.cmbSupplierName).TabIndex = 4;
		this.txtdateto.CustomFormat = "dd-MMM-yy";
		this.txtdateto.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdateto.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtdateto.Location = new System.Drawing.Point(230, 46);
		this.txtdateto.Name = "txtdateto";
		this.txtdateto.Size = new System.Drawing.Size(122, 23);
		this.txtdateto.TabIndex = 2;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(355, 22);
		this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(71, 15);
		this.label2.TabIndex = 18;
		this.label2.Text = "Contract No";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.Red;
		this.label1.Location = new System.Drawing.Point(92, 49);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(14, 17);
		this.label1.TabIndex = 573;
		this.label1.Text = "*";
		this.cmbcontractNo.AllowNull = (DefaultableBoolean)1;
		this.cmbcontractNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbcontractNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbcontractNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcontractNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.cmbcontractNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcontractNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcontractNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcontractNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance33;
		((SpecialBoxBase)((UltraGridBase)this.cmbcontractNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcontractNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.cmbcontractNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcontractNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcontractNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance35;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcontractNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.cmbcontractNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcontractNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcontractNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance37;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance38).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcontractNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.cmbcontractNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcontractNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance39).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance39).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance39).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcontractNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance39;
		((AppearanceBase)appearance40).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcontractNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.cmbcontractNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcontractNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcontractNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.cmbcontractNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcontractNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbcontractNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcontractNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcontractNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcontractNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbcontractNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbcontractNo).Location = new System.Drawing.Point(449, 16);
		((System.Windows.Forms.Control)(object)this.cmbcontractNo).Name = "cmbcontractNo";
		((System.Windows.Forms.Control)(object)this.cmbcontractNo).Size = new System.Drawing.Size(306, 26);
		((System.Windows.Forms.Control)(object)this.cmbcontractNo).TabIndex = 3;
		this.txtdatefrom.Checked = false;
		this.txtdatefrom.CustomFormat = "dd-MMM-yy";
		this.txtdatefrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdatefrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtdatefrom.Location = new System.Drawing.Point(106, 46);
		this.txtdatefrom.Name = "txtdatefrom";
		this.txtdatefrom.ShowCheckBox = true;
		this.txtdatefrom.Size = new System.Drawing.Size(122, 23);
		this.txtdatefrom.TabIndex = 1;
		this.btnShow.BackColor = System.Drawing.Color.Teal;
		this.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShow.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShow.ForeColor = System.Drawing.Color.White;
		this.btnShow.Location = new System.Drawing.Point(761, 44);
		this.btnShow.Name = "btnShow";
		this.btnShow.Size = new System.Drawing.Size(65, 26);
		this.btnShow.TabIndex = 5;
		this.btnShow.Text = "Show";
		this.btnShow.UseVisualStyleBackColor = false;
		this.btnShow.Click += new System.EventHandler(btnShow_Click);
		this.btnShow.Leave += new System.EventHandler(btnShow_Leave);
		this.btnShow.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(btnShow_PreviewKeyDown);
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label51.Location = new System.Drawing.Point(3, 50);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(91, 15);
		this.label51.TabIndex = 572;
		this.label51.Text = "Date From && To";
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.label3);
		this.panel4.Controls.Add(this.tsItemType);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(3, 3);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1003, 57);
		this.panel4.TabIndex = 2;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.White;
		this.label3.Location = new System.Drawing.Point(5, 32);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(180, 19);
		this.label3.TabIndex = 0;
		this.label3.Text = "Pending Work Status";
		this.tsItemType.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tsItemType.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.tsItemType.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnnew, this.btnRefresh, this.BtnPrintRegister, this.btnShortCutKey });
		this.tsItemType.Location = new System.Drawing.Point(0, 0);
		this.tsItemType.Name = "tsItemType";
		this.tsItemType.Size = new System.Drawing.Size(1003, 27);
		this.tsItemType.TabIndex = 3;
		this.tsItemType.Text = "toolStrip4";
		this.btnnew.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(58, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.BtnPrintRegister.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnPrintRegister.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.BtnPrintRegister.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnPrintRegister.Name = "BtnPrintRegister";
		this.BtnPrintRegister.Size = new System.Drawing.Size(150, 24);
		this.BtnPrintRegister.Text = "555-Print Register";
		this.BtnPrintRegister.Click += new System.EventHandler(BtnPrintRegister_Click);
		this.btnShortCutKey.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnShortCutKey.Image = (System.Drawing.Image)resources.GetObject("btnShortCutKey.Image");
		this.btnShortCutKey.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKey.Name = "btnShortCutKey";
		this.btnShortCutKey.Size = new System.Drawing.Size(106, 24);
		this.btnShortCutKey.Text = "ShortCut Keys";
		this.btnShortCutKey.Click += new System.EventHandler(btnShortCutKey_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1017, 633);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmExportPendingWorksRegister";
		this.Text = "Sale Contract Register";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmExportPendingWorksRegister_FormClosed);
		base.Load += new System.EventHandler(frmExportShipingLineBookingRpt_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmExportShipingLineBookingRpt_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdContractRegister).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbperemeter).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSupplierName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcontractNo).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.tsItemType.ResumeLayout(false);
		this.tsItemType.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
