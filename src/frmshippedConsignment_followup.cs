// using Architecture.Reports;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Export;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Export;
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
namespace Architecture.WinApp.ExportReports
{

public class frmshippedConsignment_followup : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private bool UpdateMood;

	private int RecId;

	private bool FormInitialized;

	private DataTable dtCurrentStatus = new DataTable();

	private DataTable dtDocStatus = new DataTable();

	private DataTable GrdTableForLayout = new DataTable();

	private Rightsobjects formright = new Rightsobjects();

	private IContainer components;

	private ToolStripButton btnPrintRegister;

	private ToolStripButton btnnew;

	private ToolStrip toolStrip1;

	private ToolStripButton btnPrintSlip;

	private Panel panel2;

	internal UltraCombo InvoiceNo;

	private Label label3;

	private Panel panel5;

	private Panel panel4;

	private Panel panel1;

	private Panel panel3;

	private DateTimePicker txtDocToParty;

	private DateTimePicker txtDocToBank;

	private ToolStripButton btnSave;

	private ToolStripButton btnUpdate;

	private Label label2;

	private Label label1;

	private Label txtETDFinalDate;

	private DateTimePicker txtETDFinalDate4;

	private Label txtLoadedOnVesselDate;

	private DateTimePicker txtLoadedOnVesselDate3;

	private Label txtReachedAtLoadingPortDate;

	private DateTimePicker txtReachedAtLoadingPortDate2;

	private Label txtLoadOnTrainDate;

	public DateTimePicker txtLoadOnTrainDate1;

	private Panel panel6;

	private Label label4;

	private Panel panel7;

	private Panel panel9;

	private Panel panel8;

	private Panel panel10;

	private Label label5;

	private CtrlGrdBar ctrlGrdBar1;

	internal UltraCombo cmbcurrsatatus;

	private Label cmbcurstatus;

	internal UltraCombo cmbdocstatus;

	private Label label8;

	private ToolStripButton btnRefresh;

	private ToolStripButton btnshortcutkey;

	private ToolStripButton btnLookup;

	private Label label6;

	private TextBox txtCourierTrackingNo;

	private Label label7;

	private DateTimePicker txtReachedDestinationPortDate;

	private TextBox txtGdNo;

	private Label label9;

	private Label label10;

	private DateTimePicker txtGoodsDeclarationDate;

	private Label label11;

	private DateTimePicker txtPortPayment;

	private GroupBox groupBox1;

	internal UltraCombo cmbStatus;

	private Label label12;

	private Button btnshow;

	private Label label13;

	private Button btnUpdateStatus;

	internal GridEX grdHistory;

	private Label label14;

	private DateTimePicker datCuttOfDate;

	private Label label15;

	private TextBox txtCutOffDateDays;

	private Label label16;

	private TextBox txtETADestinationPortDays;

	private Label label17;

	private DateTimePicker txtETDLoadingPort;

	public frmshippedConsignment_followup(UserAccount userAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = userAccount;
	}

	private void frmshippedConsignment_followup_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1200, 700);
			FormInitialized = true;
			InvoiceNoFill();
			CurrentStatusfill();
			DocStatusfill();
			GridFill();
			cmbStatus.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InvoiceNoFill()
	{
		try
		{
			int id = Conversion.ToInt(InvoiceNo.Value);
			DataTable dtInvoiceNo = CommonServices.GetExportInvoiceNo();
			if (dtInvoiceNo.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtInvoiceNo, InvoiceNo, "Id", "InvoiceNo", "Invoice No", true);
				((UltraGridBase)InvoiceNo).DisplayLayout.Bands[0].Columns[1].Width = 168;
			}
			if (id > 0)
			{
				if (dtInvoiceNo.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
				{
					InvoiceNo.Value = id;
				}
				else
				{
					((Control)(object)InvoiceNo).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DocStatusfill()
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		try
		{
			int id = Conversion.ToInt(cmbdocstatus.Value);
			int filterid = Conversion.ToInt(cmbStatus.Value);
			dtDocStatus.Rows.Clear();
			ExImLookUps obj = new ExImLookUps();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.ExImLookUptypesId = 5;
			dtDocStatus = ExImLookUps.GetByLookTypeId(obj);
			if (dtDocStatus.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtDocStatus, cmbdocstatus, "Id", "LookUpName", "Document Status", true);
				DDL.BindDDLNew(dtDocStatus, cmbStatus, "Id", "LookUpName", "Document Status", true);
				((UltraGridBase)cmbdocstatus).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				UltraGridColumn obj2 = ((UltraGridBase)cmbStatus).DisplayLayout.Bands[0].Columns.Add();
				((KeyedSubObjectBase)obj2).Key = "Selected";
				((HeaderBase)obj2.Header).Caption = string.Empty;
				obj2.Header.CheckBoxVisibility = (HeaderCheckBoxVisibility)2;
				obj2.DataType = typeof(bool);
				((HeaderBase)obj2.Header).VisiblePosition = 0;
				cmbStatus.CheckedListSettings.CheckStateMember = "Selected";
				((EditorCheckedListSettings)cmbStatus.CheckedListSettings).EditorValueSource = (EditorWithComboValueSource)1;
				((EditorCheckedListSettings)cmbStatus.CheckedListSettings).ListSeparator = ",";
				((EditorCheckedListSettings)cmbStatus.CheckedListSettings).ItemCheckArea = (ItemCheckArea)1;
				((UltraGridBase)cmbStatus).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				((UltraGridBase)cmbStatus).Rows[1].Activate();
			}
			if (id > 0)
			{
				if (dtDocStatus.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
				{
					cmbdocstatus.Value = id;
				}
				else
				{
					((Control)(object)cmbdocstatus).Text = string.Empty;
				}
			}
			if (filterid > 0)
			{
				if (dtDocStatus.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == filterid))
				{
					cmbStatus.Value = filterid;
				}
				else
				{
					((Control)(object)cmbStatus).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CurrentStatusfill()
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		try
		{
			dtCurrentStatus.Rows.Clear();
			ExImLookUps obj = new ExImLookUps();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.ExImLookUptypesId = 6;
			dtCurrentStatus = ExImLookUps.GetByLookTypeId(obj);
			if (dtCurrentStatus.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtCurrentStatus, cmbcurrsatatus, "Id", "LookUpName", "Current Status", true);
				((UltraGridBase)cmbcurrsatatus).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			int id = Conversion.ToInt(cmbcurrsatatus.Value);
			if (id > 0)
			{
				if (dtCurrentStatus.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
				{
					cmbcurrsatatus.Value = id;
				}
				else
				{
					((Control)(object)cmbcurrsatatus).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InvoiceNo_Leave(object sender, EventArgs e)
	{
		try
		{
			DataTable dt = ExImShippedConsignmentFollowUps.GetByID(Conversion.ToInt(InvoiceNo.Value));
			if (dt.Rows.Count > 0)
			{
				cmbdocstatus.Value = Conversion.ToInt(dt.Rows[0]["DocumentStatusId"]);
				cmbcurrsatatus.Value = Conversion.ToInt(dt.Rows[0]["CurrentStatusId"]);
				txtCourierTrackingNo.Text = Conversion.ToString(dt.Rows[0]["CourierTracingNo"]);
				txtGdNo.Text = Conversion.ToString(dt.Rows[0]["GdNo"]);
				if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime(dt.Rows[0]["CuttOfDate"])))
				{
					datCuttOfDate.Checked = true;
					datCuttOfDate.Value = Conversion.ToDateTime(dt.Rows[0]["CuttOfDate"]);
				}
				else
				{
					datCuttOfDate.Value = DateTime.Now;
					datCuttOfDate.Checked = false;
				}
				if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime(dt.Rows[0]["DocToBankDate"])))
				{
					txtDocToBank.Checked = true;
					txtDocToBank.Value = Conversion.ToDateTime(dt.Rows[0]["DocToBankDate"]);
				}
				else
				{
					txtDocToBank.Value = DateTime.Now;
					txtDocToBank.Checked = false;
				}
				if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime(dt.Rows[0]["DocCourierToPartyDate"])))
				{
					txtDocToParty.Value = Conversion.ToDateTime(dt.Rows[0]["DocCourierToPartyDate"]);
					txtDocToParty.Checked = true;
				}
				else
				{
					txtDocToParty.Value = DateTime.Now;
					txtDocToParty.Checked = false;
				}
				if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime(dt.Rows[0]["LoadOnTrainDate"])))
				{
					txtLoadOnTrainDate1.Value = Conversion.ToDateTime(dt.Rows[0]["LoadOnTrainDate"]);
					txtLoadOnTrainDate1.Checked = true;
				}
				else
				{
					txtLoadOnTrainDate1.Value = DateTime.Now;
					txtLoadOnTrainDate1.Checked = false;
				}
				if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime(dt.Rows[0]["ReachedAtLoadingPortDate"])))
				{
					txtReachedAtLoadingPortDate2.Checked = true;
					txtReachedAtLoadingPortDate2.Value = Conversion.ToDateTime(dt.Rows[0]["ReachedAtLoadingPortDate"]);
				}
				else
				{
					txtReachedAtLoadingPortDate2.Value = DateTime.Now;
					txtReachedAtLoadingPortDate2.Checked = false;
				}
				if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime(dt.Rows[0]["LoadedOnVesselDate"])))
				{
					txtLoadedOnVesselDate3.Checked = true;
					txtLoadedOnVesselDate3.Value = Conversion.ToDateTime(dt.Rows[0]["LoadedOnVesselDate"]);
				}
				else
				{
					txtLoadedOnVesselDate3.Value = DateTime.Now;
					txtLoadedOnVesselDate3.Checked = false;
				}
				if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime(dt.Rows[0]["ETDFinalDate"])))
				{
					txtETDFinalDate4.Value = Conversion.ToDateTime(dt.Rows[0]["ETDFinalDate"]);
					txtETDFinalDate4.Checked = true;
				}
				else
				{
					txtETDFinalDate4.Value = DateTime.Now;
					txtETDFinalDate4.Checked = false;
				}
				if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime(dt.Rows[0]["ReachedAtDestinationPort"])))
				{
					txtReachedDestinationPortDate.Value = Conversion.ToDateTime(dt.Rows[0]["ReachedAtDestinationPort"]);
					txtReachedDestinationPortDate.Checked = true;
				}
				else
				{
					txtReachedDestinationPortDate.Value = DateTime.Now;
					txtReachedDestinationPortDate.Checked = false;
				}
				if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime(dt.Rows[0]["GdDate"])))
				{
					txtGoodsDeclarationDate.Checked = true;
					txtGoodsDeclarationDate.Value = Conversion.ToDateTime(dt.Rows[0]["GdDate"]);
				}
				else
				{
					txtGoodsDeclarationDate.Value = DateTime.Now;
					txtGoodsDeclarationDate.Checked = false;
				}
				if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime(dt.Rows[0]["PortPayment"])))
				{
					txtPortPayment.Checked = true;
					txtPortPayment.Value = Conversion.ToDateTime(dt.Rows[0]["PortPayment"]);
				}
				else
				{
					txtPortPayment.Value = DateTime.Now;
					txtPortPayment.Checked = false;
				}
				if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime(dt.Rows[0]["ETDLoadingPort"])))
				{
					txtETDLoadingPort.Checked = true;
					txtETDLoadingPort.Value = Conversion.ToDateTime(dt.Rows[0]["ETDLoadingPort"]);
				}
				else
				{
					txtETDLoadingPort.Value = DateTime.Now;
					txtETDLoadingPort.Checked = false;
				}
				UpdateMood = true;
				btnUpdate.Visible = true;
				btnSave.Visible = false;
			}
			else
			{
				txtDocToBank.Value = DateTime.Now;
				txtDocToBank.Checked = false;
				txtDocToParty.Value = DateTime.Now;
				txtDocToParty.Checked = false;
				txtETDFinalDate4.Value = DateTime.Now;
				txtETDFinalDate4.Checked = false;
				txtLoadedOnVesselDate3.Value = DateTime.Now;
				txtLoadedOnVesselDate3.Checked = false;
				txtLoadOnTrainDate1.Value = DateTime.Now;
				txtLoadOnTrainDate1.Checked = false;
				txtReachedAtLoadingPortDate2.Value = DateTime.Now;
				txtReachedAtLoadingPortDate2.Checked = false;
				((Control)(object)cmbcurrsatatus).Text = string.Empty;
				((Control)(object)cmbdocstatus).Text = string.Empty;
				txtCourierTrackingNo.Text = string.Empty;
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
			if (((DataTable)((UltraGridBase)cmbStatus).DataSource).Rows.Count > 0)
			{
				((UltraGridBase)cmbStatus).Rows[1].Activate();
			}
			else
			{
				((Control)(object)cmbStatus).Text = string.Empty;
			}
			GridFill();
			((Control)(object)InvoiceNo).Text = string.Empty;
			datCuttOfDate.Value = DateTime.Now;
			txtDocToBank.Value = DateTime.Now;
			txtDocToParty.Value = DateTime.Now;
			txtETDFinalDate4.Value = DateTime.Now;
			txtLoadedOnVesselDate3.Value = DateTime.Now;
			txtLoadOnTrainDate1.Value = DateTime.Now;
			txtReachedAtLoadingPortDate2.Value = DateTime.Now;
			datCuttOfDate.Checked = false;
			txtDocToBank.Checked = false;
			txtDocToParty.Checked = false;
			txtLoadOnTrainDate1.Checked = false;
			txtReachedAtLoadingPortDate2.Checked = false;
			txtLoadedOnVesselDate3.Checked = false;
			txtETDFinalDate4.Checked = false;
			((Control)(object)cmbcurrsatatus).Text = string.Empty;
			((Control)(object)cmbdocstatus).Text = string.Empty;
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			txtCourierTrackingNo.Text = string.Empty;
			InvoiceNo.Focus();
			UpdateMood = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbcurrsatatus_Leave(object sender, EventArgs e)
	{
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			InvoiceNoFill();
			CurrentStatusfill();
			DocStatusfill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmshippedConsignment_followup_FormClosed(object sender, FormClosedEventArgs e)
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

	private void GridFill()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		try
		{
			string status = "";
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.StatusId = Conversion.ToInt(cmbStatus.Value);
			if (((Control)(object)cmbStatus).Text != null && ((Control)(object)cmbStatus).Text != string.Empty)
			{
				string[] array = (Conversion.ToString((object)((Control)(object)cmbStatus).Text.Trim()) + ",").Split(',');
				foreach (string author in array)
				{
					DataRow[] dr = dtDocStatus.Select("LookUpName='" + Conversion.ToString((object)author) + "'");
					if (dr.Length != 0)
					{
						status = status + "," + Conversion.ToString(dr[0].ItemArray[0]);
					}
				}
			}
			obj.Status = status;
			DataTable dt = ExImShippedConsignmentFollowUps.ExImShippedConsignmentFollowUps_Slipandregister(obj);
			if (dt.Rows.Count > 0)
			{
				DataTable dataTable = new DataTable();
				dataTable.Columns.Add("ShipmentId", typeof(int));
				dataTable.Columns.Add("InvoiceId", typeof(int));
				dataTable.Columns.Add("InvoiceNo", typeof(string));
				dataTable.Columns.Add("InvoiceDate", typeof(DateTime));
				dataTable.Columns.Add("Customer", typeof(string));
				dataTable.Columns.Add("DestinationPort", typeof(string));
				dataTable.Columns.Add("CRO_Number", typeof(string));
				dataTable.Columns.Add("ShippingLine", typeof(string));
				dataTable.Columns.Add("ShippingAgent", typeof(string));
				dataTable.Columns.Add("Forwarder", typeof(string));
				dataTable.Columns.Add("FclQty", typeof(double));
				dataTable.Columns.Add("MTons", typeof(double));
				dataTable.Columns.Add("FcurrencyId", typeof(int));
				dataTable.Columns.Add("PaymentStatus", typeof(string));
				dataTable.Columns.Add("DispatchedFactory", typeof(DateTime));
				dataTable.Columns.Add("LoadOnTrainDate", typeof(DateTime));
				dataTable.Columns.Add("ReachedAtLoadingPortDate", typeof(DateTime));
				dataTable.Columns.Add("LoadedOnVesselDate", typeof(DateTime));
				dataTable.Columns.Add("CuttOfDate", typeof(DateTime));
				dataTable.Columns.Add("ETDLoadingPort", typeof(DateTime));
				dataTable.Columns.Add("ETDDestinationPort", typeof(DateTime));
				dataTable.Columns.Add("DocToBank", typeof(DateTime));
				dataTable.Columns.Add("DocToParty", typeof(DateTime));
				dataTable.Columns.Add("ReachedAtDestinationPort", typeof(DateTime));
				dataTable.Columns.Add("CourierTracingNo", typeof(string));
				dataTable.Columns.Add("DocumentStatus", typeof(int));
				dataTable.Columns.Add("CurrentStatus", typeof(int));
				dataTable.Columns.Add("GoodsDeclarationNo", typeof(string));
				dataTable.Columns.Add("GoodsDeclarationDate", typeof(DateTime));
				dataTable.Columns.Add("PortPaymentDate", typeof(DateTime));
				dataTable.Columns.Add("ContainerNos", typeof(string));
				dataTable.Columns.Add("DocumentTypeId", typeof(int));
				dataTable.Columns.Add("NoOfAttachments", typeof(int));
				GrdTableForLayout = dataTable.Clone();
				dataTable.Clear();
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dataTable.Rows.Add(Conversion.ToInt(dt.Rows[i]["ShipmentId"]), Conversion.ToInt(dt.Rows[i]["InvoiceId"]), Conversion.ToString(dt.Rows[i]["InvoiceNo"]), Conversion.ToDateTime(dt.Rows[i]["InvoiceDate"]), dt.Rows[i]["CustomerName"], dt.Rows[i]["DestinationPort"], dt.Rows[i]["CRO_Number"], dt.Rows[i]["ShippingLine"], dt.Rows[i]["ShippingAgent"], dt.Rows[i]["Forwarder"], Conversion.ToDouble(dt.Rows[i]["FclQty"]), Conversion.ToDouble(dt.Rows[i]["MTons"]), Conversion.ToInt(dt.Rows[i]["FcurrencyId"]), (Conversion.ToDouble(dt.Rows[i]["RealizedAmount"]) > 0.0) ? "Received" : "Not Received", dt.Rows[i]["DispatchedFactoryDate"], dt.Rows[i]["LoadOnTrainDate"], dt.Rows[i]["ReachedAtLoadingPortDate"], dt.Rows[i]["LoadedOnVesselDate"], dt.Rows[i]["CuttOfDate"], dt.Rows[i]["ETDLoadingPort"], dt.Rows[i]["ETDDestinationPort"], dt.Rows[i]["DocToBank"], dt.Rows[i]["DocToParty"], dt.Rows[i]["ReachedAtDestinationPort"], Conversion.ToString(dt.Rows[i]["CourierTracingNo"]), Conversion.ToInt(dt.Rows[i]["DocumentStatusId"]), Conversion.ToInt(dt.Rows[i]["CurrentStatusId"]), Conversion.ToString(dt.Rows[i]["GdNo"]), dt.Rows[i]["GdDate"], dt.Rows[i]["PortPayment"], dt.Rows[i]["ContainerNos"], dt.Rows[i]["DocumentTypeId"], dt.Rows[i]["NoOfAttachments"]);
					GrdTableForLayout.ImportRow(dataTable.Rows[i]);
				}
				grdHistory.DataSource = dataTable;
				grdHistory.RetrieveStructure();
				grdsetting();
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

	private void grdsetting()
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdHistory.AllowEdit = (InheritableBoolean)1;
			grdHistory.DefaultFilterRowComparison = (FilterConditionOperator)7;
			grdHistory.RootTable.HeaderLines = 3;
			foreach (GridEXColumn item in (JanusCollectionBase)grdHistory.RootTable.Columns)
			{
				item.HeaderAlignment = (TextAlignment)2;
			}
			grdHistory.RootTable.Columns["ShipmentId"].Visible = false;
			grdHistory.RootTable.Columns["InvoiceId"].Visible = false;
			grdHistory.RootTable.Columns["FcurrencyId"].Visible = false;
			grdHistory.RootTable.Columns["ShippingAgent"].Visible = false;
			grdHistory.RootTable.Columns["DocumentTypeId"].Visible = false;
			grdHistory.RootTable.Columns["LoadOnTrainDate"].Caption = "Loaded On Train";
			grdHistory.RootTable.Columns["ReachedAtLoadingPortDate"].Caption = "Reached At Loading Port";
			grdHistory.RootTable.Columns["LoadedOnVesselDate"].Caption = "Loaded On Vessel";
			grdHistory.RootTable.Columns["ETDDestinationPort"].Caption = "ETA Destination Port";
			grdHistory.RootTable.Columns["ETDLoadingPort"].Caption = "ETD Loading Port";
			grdHistory.RootTable.Columns["DocToBank"].Caption = "Document To Bank";
			grdHistory.RootTable.Columns["DocToParty"].Caption = "Document To Party";
			grdHistory.RootTable.Columns["CuttOfDate"].Caption = "Vessel Cut Off Date";
			grdHistory.RootTable.Columns["ReachedAtDestinationPort"].Caption = "Actual Reached At Destination";
			grdHistory.RootTable.Columns["FclQty"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["FclQty"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["FclQty"].FormatString = "#,##0.##";
			grdHistory.RootTable.Columns["FclQty"].TotalFormatString = "#,##0.##";
			grdHistory.RootTable.Columns["MTons"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["MTons"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["MTons"].FormatString = "#,##0.##";
			grdHistory.RootTable.Columns["MTons"].TotalFormatString = "#,##0.##";
			grdHistory.RootTable.Columns.Add("Edit");
			grdHistory.RootTable.Columns["Edit"].Key = "Edit";
			grdHistory.RootTable.Columns["Edit"].Caption = "Edit";
			grdHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdHistory.RootTable.Columns["Edit"].Width = 35;
			grdHistory.RootTable.Columns["FclQty"].TextAlignment = (TextAlignment)2;
			grdHistory.RootTable.Columns["CRO_Number"].ColumnType = (ColumnType)5;
			grdHistory.RootTable.Columns["InvoiceNo"].ColumnType = (ColumnType)5;
			grdHistory.RootTable.Columns["InvoiceDate"].FormatString = "dd-MMM-yy";
			grdHistory.RootTable.Columns["DocToBank"].FormatString = "dd-MMM-yy";
			grdHistory.RootTable.Columns["DocToParty"].FormatString = "dd-MMM-yy";
			grdHistory.RootTable.Columns["DispatchedFactory"].FormatString = "dd-MMM-yy";
			grdHistory.RootTable.Columns["LoadOnTrainDate"].FormatString = "dd-MMM-yy";
			grdHistory.RootTable.Columns["ReachedAtLoadingPortDate"].FormatString = "dd-MMM-yy";
			grdHistory.RootTable.Columns["ETDDestinationPort"].FormatString = "dd-MMM-yy";
			grdHistory.RootTable.Columns["ETDLoadingPort"].FormatString = "dd-MMM-yy";
			grdHistory.RootTable.Columns["LoadedOnVesselDate"].FormatString = "dd-MMM-yy";
			grdHistory.RootTable.Columns["CuttOfDate"].FormatString = "dd-MMM-yy";
			grdHistory.RootTable.Columns["ReachedAtDestinationPort"].FormatString = "dd-MMM-yy";
			grdHistory.RootTable.Columns["PortPaymentDate"].FormatString = "dd-MMM-yy";
			grdHistory.RootTable.Columns["GoodsDeclarationDate"].FormatString = "dd-MMM-yy";
			grdHistory.RootTable.Columns["ShipmentId"].EditType = (EditType)0;
			grdHistory.RootTable.Columns["InvoiceId"].EditType = (EditType)0;
			grdHistory.RootTable.Columns["InvoiceNo"].EditType = (EditType)0;
			grdHistory.RootTable.Columns["InvoiceDate"].EditType = (EditType)0;
			grdHistory.RootTable.Columns["CRO_Number"].EditType = (EditType)0;
			grdHistory.RootTable.Columns["Customer"].EditType = (EditType)0;
			grdHistory.RootTable.Columns["DestinationPort"].EditType = (EditType)0;
			grdHistory.RootTable.Columns["ShippingLine"].EditType = (EditType)0;
			grdHistory.RootTable.Columns["ShippingAgent"].EditType = (EditType)0;
			grdHistory.RootTable.Columns["Forwarder"].EditType = (EditType)0;
			grdHistory.RootTable.Columns["FclQty"].EditType = (EditType)0;
			grdHistory.RootTable.Columns["MTons"].EditType = (EditType)0;
			grdHistory.RootTable.Columns["FcurrencyId"].EditType = (EditType)0;
			grdHistory.RootTable.Columns["PaymentStatus"].EditType = (EditType)0;
			grdHistory.RootTable.Columns["CourierTracingNo"].EditType = (EditType)0;
			grdHistory.RootTable.Columns["ETDLoadingPort"].EditType = (EditType)2;
			grdHistory.RootTable.Columns["GoodsDeclarationNo"].EditType = (EditType)0;
			grdHistory.RootTable.Columns["DispatchedFactory"].EditType = (EditType)0;
			grdHistory.RootTable.Columns["ContainerNos"].EditType = (EditType)0;
			grdHistory.RootTable.Columns["ShipmentId"].FilterEditType = (FilterEditType)1;
			grdHistory.RootTable.Columns["InvoiceId"].FilterEditType = (FilterEditType)1;
			grdHistory.RootTable.Columns["InvoiceNo"].FilterEditType = (FilterEditType)1;
			grdHistory.RootTable.Columns["InvoiceDate"].FilterEditType = (FilterEditType)6;
			grdHistory.RootTable.Columns["CRO_Number"].FilterEditType = (FilterEditType)1;
			grdHistory.RootTable.Columns["Customer"].FilterEditType = (FilterEditType)1;
			grdHistory.RootTable.Columns["DestinationPort"].FilterEditType = (FilterEditType)1;
			grdHistory.RootTable.Columns["ShippingLine"].FilterEditType = (FilterEditType)1;
			grdHistory.RootTable.Columns["ShippingAgent"].FilterEditType = (FilterEditType)1;
			grdHistory.RootTable.Columns["Forwarder"].FilterEditType = (FilterEditType)1;
			grdHistory.RootTable.Columns["FclQty"].FilterEditType = (FilterEditType)1;
			grdHistory.RootTable.Columns["MTons"].FilterEditType = (FilterEditType)1;
			grdHistory.RootTable.Columns["FcurrencyId"].FilterEditType = (FilterEditType)1;
			grdHistory.RootTable.Columns["PaymentStatus"].FilterEditType = (FilterEditType)1;
			grdHistory.RootTable.Columns["CourierTracingNo"].FilterEditType = (FilterEditType)1;
			grdHistory.RootTable.Columns["DocumentStatus"].FilterEditType = (FilterEditType)4;
			grdHistory.RootTable.Columns["CurrentStatus"].FilterEditType = (FilterEditType)4;
			grdHistory.RootTable.Columns["GoodsDeclarationNo"].FilterEditType = (FilterEditType)1;
			grdHistory.RootTable.Columns["ContainerNos"].FilterEditType = (FilterEditType)1;
			grdHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdHistory.RootTable.Columns["DocToBank"].EditType = (EditType)2;
			grdHistory.RootTable.Columns["DocToParty"].EditType = (EditType)2;
			grdHistory.RootTable.Columns["LoadOnTrainDate"].EditType = (EditType)2;
			grdHistory.RootTable.Columns["ReachedAtLoadingPortDate"].EditType = (EditType)2;
			grdHistory.RootTable.Columns["LoadedOnVesselDate"].EditType = (EditType)2;
			grdHistory.RootTable.Columns["CuttOfDate"].EditType = (EditType)2;
			grdHistory.RootTable.Columns["ETDDestinationPort"].EditType = (EditType)2;
			grdHistory.RootTable.Columns["ReachedAtDestinationPort"].EditType = (EditType)2;
			grdHistory.RootTable.Columns["GoodsDeclarationDate"].EditType = (EditType)2;
			grdHistory.RootTable.Columns["PortPaymentDate"].EditType = (EditType)2;
			grdHistory.RootTable.Columns["DispatchedFactory"].EditType = (EditType)2;
			grdHistory.RootTable.Columns["DocToParty"].FormatString = "dd-MMM-yy";
			grdHistory.RootTable.Columns["LoadOnTrainDate"].FormatString = "dd-MMM-yy";
			grdHistory.RootTable.Columns["ReachedAtLoadingPortDate"].FormatString = "dd-MMM-yy";
			grdHistory.RootTable.Columns["ETDDestinationPort"].FormatString = "dd-MMM-yy";
			grdHistory.RootTable.Columns["LoadedOnVesselDate"].FormatString = "dd-MMM-yy";
			grdHistory.RootTable.Columns["ReachedAtDestinationPort"].FormatString = "dd-MMM-yy";
			grdHistory.RootTable.Columns["PortPaymentDate"].FormatString = "dd-MMM-yy";
			grdHistory.RootTable.Columns["GoodsDeclarationDate"].FormatString = "dd-MMM-yy";
			grdHistory.RootTable.Columns["DispatchedFactory"].FormatString = "dd-MMM-yy";
			grdHistory.RootTable.Columns["InvoiceNo"].Width = Constants.ExportConstants.InvoiceNoConstant;
			grdHistory.RootTable.Columns["InvoiceDate"].Width = Constants.ExportConstants.DateConstant;
			grdHistory.RootTable.Columns["CRO_Number"].Width = Constants.ExportConstants.InvoiceNoConstant;
			grdHistory.RootTable.Columns["Customer"].Width = Constants.ExportConstants.StringConstant + 30;
			grdHistory.RootTable.Columns["DestinationPort"].Width = Constants.ExportConstants.PortConstant;
			grdHistory.RootTable.Columns["ShippingLine"].Width = Constants.ExportConstants.StringConstant;
			grdHistory.RootTable.Columns["ShippingAgent"].Width = Constants.ExportConstants.StringConstant;
			grdHistory.RootTable.Columns["Forwarder"].Width = Constants.ExportConstants.StringConstant;
			grdHistory.RootTable.Columns["FclQty"].Width = Constants.ExportConstants.FclConstant;
			grdHistory.RootTable.Columns["MTons"].Width = Constants.ExportConstants.FclConstant;
			grdHistory.RootTable.Columns["DocToBank"].Width = Constants.ExportConstants.DateConstant;
			grdHistory.RootTable.Columns["DocToParty"].Width = Constants.ExportConstants.DateConstant;
			grdHistory.RootTable.Columns["PaymentStatus"].Width = Constants.ExportConstants.StatusConstant;
			grdHistory.RootTable.Columns["DispatchedFactory"].Width = Constants.ExportConstants.DateConstant + 10;
			grdHistory.RootTable.Columns["LoadOnTrainDate"].Width = Constants.ExportConstants.DateConstant;
			grdHistory.RootTable.Columns["ReachedAtLoadingPortDate"].Width = Constants.ExportConstants.DateConstant;
			grdHistory.RootTable.Columns["LoadedOnVesselDate"].Width = Constants.ExportConstants.DateConstant;
			grdHistory.RootTable.Columns["CuttOfDate"].Width = Constants.ExportConstants.DateConstant + 10;
			grdHistory.RootTable.Columns["ReachedAtDestinationPort"].Width = Constants.ExportConstants.DateConstant + 5;
			grdHistory.RootTable.Columns["ETDLoadingPort"].Width = Constants.ExportConstants.DateConstant + 5;
			grdHistory.RootTable.Columns["GoodsDeclarationDate"].Width = Constants.ExportConstants.DateConstant;
			grdHistory.RootTable.Columns["PortPaymentDate"].Width = Constants.ExportConstants.DateConstant;
			grdHistory.RootTable.Columns["CourierTracingNo"].Width = Constants.ExportConstants.StringConstant - 25;
			grdHistory.RootTable.Columns["DocumentStatus"].Width = Constants.ExportConstants.DocumentStatus;
			grdHistory.RootTable.Columns["CurrentStatus"].Width = Constants.ExportConstants.CurrentStatus;
			grdHistory.RootTable.Columns["ContainerNos"].Width = 150;
			grdHistory.RootTable.Columns.Add("Update");
			grdHistory.RootTable.Columns["Update"].Key = "Update";
			grdHistory.RootTable.Columns["Update"].Caption = "Update";
			grdHistory.RootTable.Columns["Update"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["Update"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["Update"].ButtonText = "Update";
			grdHistory.RootTable.Columns["Update"].Width = 50;
			grdHistory.RootTable.Columns["CurrentStatus"].ColumnType = (ColumnType)0;
			grdHistory.RootTable.Columns["CurrentStatus"].HasValueList = true;
			grdHistory.RootTable.Columns["CurrentStatus"].LimitToList = true;
			grdHistory.RootTable.Columns["CurrentStatus"].EditType = (EditType)4;
			grdHistory.RootTable.Columns["CurrentStatus"].Caption = "Current Status";
			grdHistory.RootTable.Columns["CurrentStatus"].ValueList.PopulateValueList((IEnumerable)dtCurrentStatus.DefaultView, "Id", "LookUpName");
			grdHistory.RootTable.Columns["DocumentStatus"].ColumnType = (ColumnType)0;
			grdHistory.RootTable.Columns["DocumentStatus"].HasValueList = true;
			grdHistory.RootTable.Columns["DocumentStatus"].LimitToList = true;
			grdHistory.RootTable.Columns["DocumentStatus"].EditType = (EditType)4;
			grdHistory.RootTable.Columns["DocumentStatus"].Caption = "Document Status";
			grdHistory.RootTable.Columns["DocumentStatus"].ValueList.PopulateValueList((IEnumerable)dtDocStatus.DefaultView, "Id", "LookUpName");
			grdHistory.RootTable.Columns.Add("Select");
			grdHistory.RootTable.Columns["Select"].ActAsSelector = true;
			grdHistory.RootTable.Columns["Select"].UseHeaderSelector = true;
			grdHistory.RootTable.Columns["Select"].Width = 40;
			grdHistory.RootTable.Columns["Select"].Position = 0;
			grdHistory.RootTable.Columns["Edit"].Position = 1;
			grdHistory.RootTable.Columns["InvoiceNo"].Position = 2;
			grdHistory.FrozenColumns = 3;
			((Control)(object)grdHistory).Refresh();
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			ReadById(Conversion.ToInt(grdHistory.CurrentRow.Cells["InvoiceId"].Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void ReadById(int InvoiceId)
	{
		try
		{
			InvoiceNo.Value = Conversion.ToInt((object)InvoiceId);
			RecId = ExImShippedConsignmentFollowUps.GetIdByInvoiceId(InvoiceId);
			InvoiceNo_Leave(null, null);
			btnSave.Visible = false;
			btnUpdate.Visible = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				int CurrId = Conversion.ToInt(r.Cells["InvoiceId"].Value);
				if (e.Column.Key == "Edit")
				{
					ReadById(CurrId);
				}
				else if (e.Column.Key == "Update")
				{
					UpdateByGrid(r);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void ShowSlip()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = new DataTable();
			dt = ExImBillOfLading.ExImBillOfLading_SlipAndRegister(new ReportsParameters
			{
				EximInvoiceId = Conversion.ToInt(InvoiceNo.Value)
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "511-ExImShippedConsignmentFollowUps_Slip.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrintSlip_Click(object sender, EventArgs e)
	{
		try
		{
			ShowSlip();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void Showregister()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = new DataTable();
			dt = ExImBillOfLading.ExImBillOfLading_SlipAndRegister(new ReportsParameters
			{
				EximInvoiceId = Conversion.ToInt(InvoiceNo.Value)
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "512-ExImShippedConsignmentFollowUps_Register.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrintRegister_Click(object sender, EventArgs e)
	{
		try
		{
			Showregister();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		try
		{
			if (Conversion.ToInt(InvoiceNo.Value) == 0)
			{
				InvoiceNo.Focus();
				throw new Exception("Please Select Invoice!");
			}
			ExImShippedConsignmentFollowUps obj = new ExImShippedConsignmentFollowUps();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				obj.Id = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			obj.EntryDate = DateTime.Now;
			obj.EntryUserId = UserAccount.ID;
			obj.ModifyDate = DateTime.Now;
			obj.ModifyUserId = UserAccount.ID;
			obj.ExImInvoiceId = Conversion.ToInt(InvoiceNo.Value);
			obj.CurrentStatusId = Conversion.ToInt(cmbcurrsatatus.Value);
			obj.DocumentStatusId = Conversion.ToInt(cmbdocstatus.Value);
			obj.CourierTracingNo = Conversion.ToString((object)txtCourierTrackingNo.Text.Trim());
			obj.GdNo = Conversion.ToString((object)txtGdNo.Text.Trim());
			if (txtDocToBank.Checked)
			{
				obj.DocToBankDate = Conversion.ToDateTime((object)txtDocToBank.Value);
			}
			if (txtDocToParty.Checked)
			{
				obj.DocCourierToPartyDate = Conversion.ToDateTime((object)txtDocToParty.Value);
			}
			if (txtLoadOnTrainDate1.Checked)
			{
				obj.LoadOnTrainDate = Conversion.ToDateTime((object)txtLoadOnTrainDate1.Value);
			}
			if (txtReachedAtLoadingPortDate2.Checked)
			{
				obj.ReachedAtLoadingPortDate = Conversion.ToDateTime((object)txtReachedAtLoadingPortDate2.Value);
			}
			if (txtLoadedOnVesselDate3.Checked)
			{
				obj.LoadedOnVesselDate = Conversion.ToDateTime((object)txtLoadedOnVesselDate3.Value);
			}
			if (txtETDFinalDate4.Checked)
			{
				obj.ETDFinalDate = Conversion.ToDateTime((object)txtETDFinalDate4.Value);
			}
			if (txtReachedDestinationPortDate.Checked)
			{
				obj.ReachedAtDestinationPort = Conversion.ToDateTime((object)txtReachedDestinationPortDate.Value);
			}
			if (txtGoodsDeclarationDate.Checked)
			{
				obj.GdDate = Conversion.ToDateTime((object)txtGoodsDeclarationDate.Value);
			}
			if (txtPortPayment.Checked)
			{
				obj.PortPayment = Conversion.ToDateTime((object)txtPortPayment.Value);
			}
			if (datCuttOfDate.Checked)
			{
				obj.CuttOfDate = Conversion.ToDateTime((object)datCuttOfDate.Value);
			}
			if (txtETDLoadingPort.Checked)
			{
				obj.ETDLoadingPort = Conversion.ToDateTime((object)txtETDLoadingPort.Value);
			}
			ExImShippedConsignmentFollowUps.Save(obj);
			if (RecId > 0)
			{
				MessageBox.Show("Record Update Successfully");
			}
			else
			{
				MessageBox.Show("Record Save Successfully");
			}
			Reset();
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

	private void UpdateByGrid(GridEXRow row)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_10f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_10fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_1109: Unknown result type (might be due to invalid IL or missing references)
		//IL_111a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1125: Unknown result type (might be due to invalid IL or missing references)
		//IL_1136: Unknown result type (might be due to invalid IL or missing references)
		//IL_1156: Unknown result type (might be due to invalid IL or missing references)
		//IL_115e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1166: Unknown result type (might be due to invalid IL or missing references)
		//IL_116e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1176: Unknown result type (might be due to invalid IL or missing references)
		//IL_117e: Unknown result type (might be due to invalid IL or missing references)
		//IL_11a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_11c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_11ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_1214: Unknown result type (might be due to invalid IL or missing references)
		//IL_123b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1262: Unknown result type (might be due to invalid IL or missing references)
		//IL_1289: Unknown result type (might be due to invalid IL or missing references)
		//IL_12b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_12d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_12fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_132a: Expected O, but got Unknown
		try
		{
			if (row == null && (int)row.RowType != 0)
			{
				return;
			}
			bool StatusForUpdate = false;
			DateTime? DocToBankDate = null;
			DateTime? DocToParty = null;
			DateTime? LoadOnTrainDate = null;
			DateTime? ReachedAtLoadingPortDate = null;
			DateTime? LoadedOnVesselDate = null;
			DateTime? CuttOfDate = null;
			DateTime? ETDDestinationPortDate = null;
			DateTime? ETDLoadingPort = null;
			DateTime? ReachedAtDestinationPortDate = null;
			DateTime? GoodsDeclarationDate = null;
			DateTime? PortPaymentDate = null;
			int DocStatusId = 0;
			int CurrStatusId = 0;
			string CourierTrackingNo = "";
			string GdNo = "";
			DataTable dt = ExImShippedConsignmentFollowUps.GetByID(Conversion.ToInt(row.Cells["InvoiceId"].Value));
			if (dt.Rows.Count > 0)
			{
				DocStatusId = Conversion.ToInt(dt.Rows[0]["DocumentStatusId"]);
				CurrStatusId = Conversion.ToInt(dt.Rows[0]["CurrentStatusId"]);
				CourierTrackingNo = Conversion.ToString(dt.Rows[0]["CourierTracingNo"]);
				GdNo = Conversion.ToString(dt.Rows[0]["GdNo"]);
				if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime(dt.Rows[0]["DocToBankDate"])))
				{
					if (Conversion.ToDateTime(row.Cells["DocToBank"].Value).Date == Conversion.ToDateTime(dt.Rows[0]["DocToBankDate"]).Date)
					{
						DocToBankDate = Conversion.ToDateTime(row.Cells["DocToBank"].Value);
					}
					else
					{
						StatusForUpdate = true;
						DocToBankDate = Conversion.ToDateTime(row.Cells["DocToBank"].Value);
					}
				}
				else if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime(row.Cells["DocToBank"].Value)))
				{
					StatusForUpdate = true;
					DocToBankDate = Conversion.ToDateTime(row.Cells["DocToBank"].Value);
				}
				if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime(dt.Rows[0]["DocCourierToPartyDate"])))
				{
					if (Conversion.ToDateTime(row.Cells["DocToParty"].Value).Date == Conversion.ToDateTime(dt.Rows[0]["DocCourierToPartyDate"]).Date)
					{
						DocToParty = Conversion.ToDateTime(row.Cells["DocToParty"].Value);
					}
					else
					{
						StatusForUpdate = true;
						DocToParty = Conversion.ToDateTime(row.Cells["DocToParty"].Value);
					}
				}
				else if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime(row.Cells["DocToParty"].Value)))
				{
					StatusForUpdate = true;
					DocToParty = Conversion.ToDateTime(row.Cells["DocToParty"].Value);
				}
				if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime(dt.Rows[0]["LoadOnTrainDate"])))
				{
					if (Conversion.ToDateTime(row.Cells["LoadOnTrainDate"].Value).Date == Conversion.ToDateTime(dt.Rows[0]["LoadOnTrainDate"]).Date)
					{
						LoadOnTrainDate = Conversion.ToDateTime(row.Cells["LoadOnTrainDate"].Value);
					}
					else
					{
						StatusForUpdate = true;
						LoadOnTrainDate = Conversion.ToDateTime(row.Cells["LoadOnTrainDate"].Value);
					}
				}
				else if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime(row.Cells["LoadOnTrainDate"].Value)))
				{
					StatusForUpdate = true;
					LoadOnTrainDate = Conversion.ToDateTime(row.Cells["LoadOnTrainDate"].Value);
				}
				if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime(dt.Rows[0]["ReachedAtLoadingPortDate"])))
				{
					if (Conversion.ToDateTime(row.Cells["ReachedAtLoadingPortDate"].Value).Date == Conversion.ToDateTime(dt.Rows[0]["ReachedAtLoadingPortDate"]).Date)
					{
						ReachedAtLoadingPortDate = Conversion.ToDateTime(row.Cells["ReachedAtLoadingPortDate"].Value);
					}
					else
					{
						StatusForUpdate = true;
						ReachedAtLoadingPortDate = Conversion.ToDateTime(row.Cells["ReachedAtLoadingPortDate"].Value);
					}
				}
				else if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime(row.Cells["ReachedAtLoadingPortDate"].Value)))
				{
					StatusForUpdate = true;
					ReachedAtLoadingPortDate = Conversion.ToDateTime(row.Cells["ReachedAtLoadingPortDate"].Value);
				}
				if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime(dt.Rows[0]["LoadedOnVesselDate"])))
				{
					if (Conversion.ToDateTime(row.Cells["LoadedOnVesselDate"].Value).Date == Conversion.ToDateTime(dt.Rows[0]["LoadedOnVesselDate"]).Date)
					{
						LoadedOnVesselDate = Conversion.ToDateTime(row.Cells["LoadedOnVesselDate"].Value);
					}
					else
					{
						StatusForUpdate = true;
						LoadedOnVesselDate = Conversion.ToDateTime(row.Cells["LoadedOnVesselDate"].Value);
					}
				}
				else if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime(row.Cells["LoadedOnVesselDate"].Value)))
				{
					StatusForUpdate = true;
					LoadedOnVesselDate = Conversion.ToDateTime(row.Cells["LoadedOnVesselDate"].Value);
				}
				if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime(dt.Rows[0]["CuttOfDate"])))
				{
					if (Conversion.ToDateTime(row.Cells["CuttOfDate"].Value).Date == Conversion.ToDateTime(dt.Rows[0]["CuttOfDate"]).Date)
					{
						CuttOfDate = Conversion.ToDateTime(row.Cells["CuttOfDate"].Value);
					}
					else
					{
						StatusForUpdate = true;
						CuttOfDate = Conversion.ToDateTime(row.Cells["CuttOfDate"].Value);
					}
				}
				else if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime(row.Cells["CuttOfDate"].Value)))
				{
					StatusForUpdate = true;
					CuttOfDate = Conversion.ToDateTime(row.Cells["CuttOfDate"].Value);
				}
				if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime(dt.Rows[0]["ETDLoadingPort"])))
				{
					if (Conversion.ToDateTime(row.Cells["ETDLoadingPort"].Value).Date == Conversion.ToDateTime(dt.Rows[0]["ETDLoadingPort"]).Date)
					{
						ETDLoadingPort = Conversion.ToDateTime(row.Cells["ETDLoadingPort"].Value);
					}
					else
					{
						StatusForUpdate = true;
						ETDLoadingPort = Conversion.ToDateTime(row.Cells["ETDLoadingPort"].Value);
					}
				}
				else if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime(row.Cells["ETDLoadingPort"].Value)))
				{
					StatusForUpdate = true;
					ETDLoadingPort = Conversion.ToDateTime(row.Cells["ETDLoadingPort"].Value);
				}
				if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime(dt.Rows[0]["ETDFinalDate"])))
				{
					if (Conversion.ToDateTime(row.Cells["ETDDestinationPort"].Value).Date == Conversion.ToDateTime(dt.Rows[0]["ETDFinalDate"]).Date)
					{
						ETDDestinationPortDate = Conversion.ToDateTime(row.Cells["ETDDestinationPort"].Value);
					}
					else
					{
						StatusForUpdate = true;
						ETDDestinationPortDate = Conversion.ToDateTime(row.Cells["ETDDestinationPort"].Value);
					}
				}
				else if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime(row.Cells["ETDDestinationPort"].Value)))
				{
					StatusForUpdate = true;
					ETDDestinationPortDate = Conversion.ToDateTime(row.Cells["ETDDestinationPort"].Value);
				}
				if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime(dt.Rows[0]["ReachedAtDestinationPort"])))
				{
					if (Conversion.ToDateTime(row.Cells["ReachedAtDestinationPort"].Value).Date == Conversion.ToDateTime(dt.Rows[0]["ReachedAtDestinationPort"]).Date)
					{
						ReachedAtDestinationPortDate = Conversion.ToDateTime(row.Cells["ReachedAtDestinationPort"].Value);
					}
					else
					{
						StatusForUpdate = true;
						ReachedAtDestinationPortDate = Conversion.ToDateTime(row.Cells["ReachedAtDestinationPort"].Value);
					}
				}
				else if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime(row.Cells["ReachedAtDestinationPort"].Value)))
				{
					StatusForUpdate = true;
					ReachedAtDestinationPortDate = Conversion.ToDateTime(row.Cells["ReachedAtDestinationPort"].Value);
				}
				if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime(dt.Rows[0]["GdDate"])))
				{
					if (Conversion.ToDateTime(row.Cells["GoodsDeclarationDate"].Value).Date == Conversion.ToDateTime(dt.Rows[0]["GdDate"]).Date)
					{
						GoodsDeclarationDate = Conversion.ToDateTime(row.Cells["GoodsDeclarationDate"].Value);
					}
					else
					{
						StatusForUpdate = true;
						GoodsDeclarationDate = Conversion.ToDateTime(row.Cells["GoodsDeclarationDate"].Value);
					}
				}
				else if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime(row.Cells["GoodsDeclarationDate"].Value)))
				{
					StatusForUpdate = true;
					GoodsDeclarationDate = Conversion.ToDateTime(row.Cells["GoodsDeclarationDate"].Value);
				}
				if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime(dt.Rows[0]["PortPayment"])))
				{
					if (Conversion.ToDateTime(row.Cells["PortPaymentDate"].Value).Date == Conversion.ToDateTime(dt.Rows[0]["PortPayment"]).Date)
					{
						PortPaymentDate = Conversion.ToDateTime(row.Cells["PortPaymentDate"].Value);
					}
					else
					{
						StatusForUpdate = true;
						PortPaymentDate = Conversion.ToDateTime(row.Cells["PortPaymentDate"].Value);
					}
				}
				else if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime(row.Cells["PortPaymentDate"].Value)))
				{
					StatusForUpdate = true;
					PortPaymentDate = Conversion.ToDateTime(row.Cells["PortPaymentDate"].Value);
				}
				if (CurrStatusId > 0)
				{
					if (CurrStatusId != Conversion.ToInt(row.Cells["CurrentStatus"].Value))
					{
						StatusForUpdate = true;
						CurrStatusId = Conversion.ToInt(row.Cells["CurrentStatus"].Value);
					}
				}
				else if (Conversion.ToInt(row.Cells["CurrentStatus"].Value) > 0)
				{
					StatusForUpdate = true;
					CurrStatusId = Conversion.ToInt(row.Cells["CurrentStatus"].Value);
				}
				if (DocStatusId > 0)
				{
					if (DocStatusId != Conversion.ToInt(row.Cells["DocumentStatus"].Value))
					{
						StatusForUpdate = true;
						DocStatusId = Conversion.ToInt(row.Cells["DocumentStatus"].Value);
					}
				}
				else if (Conversion.ToInt(row.Cells["DocumentStatus"].Value) > 0)
				{
					StatusForUpdate = true;
					DocStatusId = Conversion.ToInt(row.Cells["DocumentStatus"].Value);
				}
			}
			else
			{
				if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime(row.Cells["DocToBank"].Value)))
				{
					StatusForUpdate = true;
					DocToBankDate = Conversion.ToDateTime(row.Cells["DocToBank"].Value);
				}
				if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime(row.Cells["DocToParty"].Value)))
				{
					StatusForUpdate = true;
					DocToParty = Conversion.ToDateTime(row.Cells["DocToParty"].Value);
				}
				if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime(row.Cells["LoadOnTrainDate"].Value)))
				{
					StatusForUpdate = true;
					LoadOnTrainDate = Conversion.ToDateTime(row.Cells["LoadOnTrainDate"].Value);
				}
				if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime(row.Cells["ReachedAtLoadingPortDate"].Value)))
				{
					StatusForUpdate = true;
					ReachedAtLoadingPortDate = Conversion.ToDateTime(row.Cells["ReachedAtLoadingPortDate"].Value);
				}
				if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime(row.Cells["LoadedOnVesselDate"].Value)))
				{
					StatusForUpdate = true;
					LoadedOnVesselDate = Conversion.ToDateTime(row.Cells["LoadedOnVesselDate"].Value);
				}
				if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime(row.Cells["CuttOfDate"].Value)))
				{
					StatusForUpdate = true;
					CuttOfDate = Conversion.ToDateTime(row.Cells["CuttOfDate"].Value);
				}
				if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime(row.Cells["ETDLoadingPort"].Value)))
				{
					StatusForUpdate = true;
					ETDLoadingPort = Conversion.ToDateTime(row.Cells["ETDLoadingPort"].Value);
				}
				if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime(row.Cells["ETDDestinationPort"].Value)))
				{
					StatusForUpdate = true;
					ETDDestinationPortDate = Conversion.ToDateTime(row.Cells["ETDDestinationPort"].Value);
				}
				if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime(row.Cells["ReachedAtDestinationPort"].Value)))
				{
					StatusForUpdate = true;
					ReachedAtDestinationPortDate = Conversion.ToDateTime(row.Cells["ReachedAtDestinationPort"].Value);
				}
				if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime(row.Cells["GoodsDeclarationDate"].Value)))
				{
					StatusForUpdate = true;
					GoodsDeclarationDate = Conversion.ToDateTime(row.Cells["GoodsDeclarationDate"].Value);
				}
				if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime(row.Cells["PortPaymentDate"].Value)))
				{
					StatusForUpdate = true;
					PortPaymentDate = Conversion.ToDateTime(row.Cells["PortPaymentDate"].Value);
				}
				if (Conversion.ToInt(row.Cells["CurrentStatus"].Value) > 0)
				{
					StatusForUpdate = true;
					CurrStatusId = Conversion.ToInt(row.Cells["CurrentStatus"].Value);
				}
				if (Conversion.ToInt(row.Cells["DocumentStatus"].Value) > 0)
				{
					StatusForUpdate = true;
					DocStatusId = Conversion.ToInt(row.Cells["DocumentStatus"].Value);
				}
			}
			if (StatusForUpdate)
			{
				int shipmentId = Conversion.ToInt(row.Cells["ShipmentId"].Value);
				if (shipmentId > 0)
				{
					if (MessageBox.Show("Are you sure to Update the record?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
					{
						return;
					}
				}
				else if (MessageBox.Show("Are you sure to Save new record?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				ExImShippedConsignmentFollowUps.Save(new ExImShippedConsignmentFollowUps
				{
					EntryDate = DateTime.Now,
					EntryUserId = UserAccount.ID,
					ModifyDate = DateTime.Now,
					ModifyUserId = UserAccount.ID,
					ExImInvoiceId = Conversion.ToInt(row.Cells["InvoiceId"].Value),
					Id = shipmentId,
					CurrentStatusId = CurrStatusId,
					DocumentStatusId = DocStatusId,
					CourierTracingNo = CourierTrackingNo,
					GdNo = GdNo,
					DocToBankDate = (Conversion.CheckDateTimeNull(Conversion.ToDateTime((object)DocToBankDate)) ? null : DocToBankDate),
					DocCourierToPartyDate = (Conversion.CheckDateTimeNull(Conversion.ToDateTime((object)DocToParty)) ? null : DocToParty),
					LoadOnTrainDate = (Conversion.CheckDateTimeNull(Conversion.ToDateTime((object)LoadOnTrainDate)) ? null : LoadOnTrainDate),
					ReachedAtLoadingPortDate = (Conversion.CheckDateTimeNull(Conversion.ToDateTime((object)ReachedAtLoadingPortDate)) ? null : ReachedAtLoadingPortDate),
					LoadedOnVesselDate = (Conversion.CheckDateTimeNull(Conversion.ToDateTime((object)LoadedOnVesselDate)) ? null : LoadedOnVesselDate),
					CuttOfDate = (Conversion.CheckDateTimeNull(Conversion.ToDateTime((object)CuttOfDate)) ? null : CuttOfDate),
					ETDFinalDate = (Conversion.CheckDateTimeNull(Conversion.ToDateTime((object)ETDDestinationPortDate)) ? null : ETDDestinationPortDate),
					ReachedAtDestinationPort = (Conversion.CheckDateTimeNull(Conversion.ToDateTime((object)ReachedAtDestinationPortDate)) ? null : ReachedAtDestinationPortDate),
					GdDate = (Conversion.CheckDateTimeNull(Conversion.ToDateTime((object)GoodsDeclarationDate)) ? null : GoodsDeclarationDate),
					PortPayment = (Conversion.CheckDateTimeNull(Conversion.ToDateTime((object)PortPaymentDate)) ? null : PortPaymentDate),
					ETDLoadingPort = (Conversion.CheckDateTimeNull(Conversion.ToDateTime((object)ETDLoadingPort)) ? null : ETDLoadingPort)
				});
				if (shipmentId > 0)
				{
					MessageBox.Show("Record Update Successfully");
				}
				else
				{
					MessageBox.Show("Record Save Successfully");
				}
				GridFill();
			}
			else
			{
				MessageBox.Show("No Change found. Please Change at least one date to update record");
			}
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
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grdHistory, GrdTableForLayout);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnLookup_Click(object sender, EventArgs e)
	{
		try
		{
			ExImLookups exImLookups = new ExImLookups(UserAccount);
			exImLookups.ShowDialog();
			exImLookups.BringToFront();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnshortcutkey_Click(object sender, EventArgs e)
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

	private void frmshippedConsignment_followup_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control)
			{
				switch (e.KeyCode)
				{
				case Keys.D1:
				case Keys.NumPad1:
					btnPrintSlip_Click(null, null);
					break;
				case Keys.D2:
				case Keys.NumPad2:
					btnPrintSlip_Click(null, null);
					break;
				}
			}
			if (e.Control && e.KeyCode == Keys.U && UpdateMood)
			{
				btnUpdate_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.S && !UpdateMood)
			{
				btnSave_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnnew_Click(null, null);
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				InvoiceNo.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grdHistory).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				cmbStatus.Focus();
			}
			if (e.Control && e.KeyCode == Keys.R)
			{
				btnRefresh_Click(null, null);
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
			dt.Rows.Add("Ctrl+S", "For Save");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+F5", "For Focus on Invoice No");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus on Document status for filter in Form");
			dt.Rows.Add("Ctrl+Enter", "When Focus On Any Grid For Update Record");
			ShortCutKeyPopUp shortCutKeyPopUp = new ShortCutKeyPopUp(dt);
			shortCutKeyPopUp.StartPosition = FormStartPosition.CenterScreen;
			shortCutKeyPopUp.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdHistory.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdHistory.CurrentColumn == null)
			{
				return;
			}
			int CurrId = Conversion.ToInt(r.Cells["InvoiceId"].Value);
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdHistory.CurrentColumn.Key == "Edit")
				{
					ReadById(CurrId);
				}
				if (grdHistory.CurrentColumn.Key == "Update")
				{
					UpdateByGrid(r);
				}
				if (grdHistory.CurrentColumn.Key == "CRO_Number")
				{
					PrintCro_Slip(CurrId);
				}
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				ReadById(CurrId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				int CurrId = Conversion.ToInt(r.Cells["InvoiceId"].Value);
				int InvoiceDocumentTypeId = Conversion.ToInt(r.Cells["DocumentTypeId"].Value);
				if (e.Column.Key == "CRO_Number")
				{
					PrintCro_Slip(CurrId);
				}
				else if (e.Column.Key == "InvoiceNo")
				{
					CommonServices.CommercialInvoiceSlip521(CurrId);
				}
				else if (e.Column.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(CurrId, InvoiceDocumentTypeId);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PrintCro_Slip(int InvoiceId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = ExImExportShipingLineBooking.PrintSlipandRegister(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				ExImInvoiceId = InvoiceId
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "560-ExBooking Info(CRO).rpt");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnshow_Click(object sender, EventArgs e)
	{
		try
		{
			GridFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdateStatus_Click(object sender, EventArgs e)
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_043e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0445: Expected O, but got Unknown
		try
		{
			if (grdHistory.GetCheckedRows().Count() == 0)
			{
				throw new Exception("Please Select Rows first to update document status of multi rows");
			}
			if (MessageBox.Show("Are you sure to Update the Statuses?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			ExImShippedConsignmentFollowUps obj = new ExImShippedConsignmentFollowUps();
			obj.ShippedConsignmentFollowUpList = new List<ExImShippedConsignmentFollowUps>();
			new ReportsParameters().ReportsParameterslist = new List<ReportsParameters>();
			GridEXRow[] checkedRows = grdHistory.GetCheckedRows();
			foreach (GridEXRow row in checkedRows)
			{
				if (row != null || (int)row.RowType == 0)
				{
					DateTime? DocToBankDate = null;
					DateTime? DocToParty = null;
					DateTime? LoadOnTrainDate = null;
					DateTime? ReachedAtLoadingPortDate = null;
					DateTime? LoadedOnVesselDate = null;
					DateTime? CuttOfDate = null;
					DateTime? ETDDestinationPortDate = null;
					DateTime? ReachedAtDestinationPortDate = null;
					DateTime? GoodsDeclarationDate = null;
					DateTime? PortPaymentDate = null;
					DateTime? ETDLoadingPort = null;
					int DocStatusId = 0;
					int CurrStatusId = 0;
					if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime(row.Cells["DocToBank"].Value)))
					{
						DocToBankDate = Conversion.ToDateTime(row.Cells["DocToBank"].Value);
					}
					if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime(row.Cells["DocToParty"].Value)))
					{
						DocToParty = Conversion.ToDateTime(row.Cells["DocToParty"].Value);
					}
					if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime(row.Cells["LoadOnTrainDate"].Value)))
					{
						LoadOnTrainDate = Conversion.ToDateTime(row.Cells["LoadOnTrainDate"].Value);
					}
					if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime(row.Cells["ReachedAtLoadingPortDate"].Value)))
					{
						ReachedAtLoadingPortDate = Conversion.ToDateTime(row.Cells["ReachedAtLoadingPortDate"].Value);
					}
					if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime(row.Cells["LoadedOnVesselDate"].Value)))
					{
						LoadedOnVesselDate = Conversion.ToDateTime(row.Cells["LoadedOnVesselDate"].Value);
					}
					if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime(row.Cells["CuttOfDate"].Value)))
					{
						CuttOfDate = Conversion.ToDateTime(row.Cells["CuttOfDate"].Value);
					}
					if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime(row.Cells["ETDLoadingPort"].Value)))
					{
						ETDLoadingPort = Conversion.ToDateTime(row.Cells["ETDLoadingPort"].Value);
					}
					if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime(row.Cells["ETDDestinationPort"].Value)))
					{
						ETDDestinationPortDate = Conversion.ToDateTime(row.Cells["ETDDestinationPort"].Value);
					}
					if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime(row.Cells["ReachedAtDestinationPort"].Value)))
					{
						ReachedAtDestinationPortDate = Conversion.ToDateTime(row.Cells["ReachedAtDestinationPort"].Value);
					}
					if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime(row.Cells["GoodsDeclarationDate"].Value)))
					{
						GoodsDeclarationDate = Conversion.ToDateTime(row.Cells["GoodsDeclarationDate"].Value);
					}
					if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime(row.Cells["PortPaymentDate"].Value)))
					{
						PortPaymentDate = Conversion.ToDateTime(row.Cells["PortPaymentDate"].Value);
					}
					if (Conversion.ToInt(row.Cells["CurrentStatus"].Value) > 0)
					{
						CurrStatusId = Conversion.ToInt(row.Cells["CurrentStatus"].Value);
					}
					if (Conversion.ToInt(row.Cells["DocumentStatus"].Value) > 0)
					{
						DocStatusId = Conversion.ToInt(row.Cells["DocumentStatus"].Value);
					}
					int shipmentId = Conversion.ToInt(row.Cells["ShipmentId"].Value);
					ExImShippedConsignmentFollowUps fol = new ExImShippedConsignmentFollowUps();
					fol.EntryDate = DateTime.Now;
					fol.EntryUserId = UserAccount.ID;
					fol.ModifyDate = DateTime.Now;
					fol.ModifyUserId = UserAccount.ID;
					fol.ExImInvoiceId = Conversion.ToInt(row.Cells["InvoiceId"].Value);
					fol.Id = shipmentId;
					fol.CurrentStatusId = CurrStatusId;
					fol.DocumentStatusId = DocStatusId;
					fol.CourierTracingNo = Conversion.ToString(row.Cells["CourierTracingNo"].Value);
					fol.GdNo = Conversion.ToString(row.Cells["GoodsDeclarationNo"].Value);
					fol.DocToBankDate = (Conversion.CheckDateTimeNull(Conversion.ToDateTime((object)DocToBankDate)) ? null : DocToBankDate);
					fol.DocCourierToPartyDate = (Conversion.CheckDateTimeNull(Conversion.ToDateTime((object)DocToParty)) ? null : DocToParty);
					fol.LoadOnTrainDate = (Conversion.CheckDateTimeNull(Conversion.ToDateTime((object)LoadOnTrainDate)) ? null : LoadOnTrainDate);
					fol.ReachedAtLoadingPortDate = (Conversion.CheckDateTimeNull(Conversion.ToDateTime((object)ReachedAtLoadingPortDate)) ? null : ReachedAtLoadingPortDate);
					fol.LoadedOnVesselDate = (Conversion.CheckDateTimeNull(Conversion.ToDateTime((object)LoadedOnVesselDate)) ? null : LoadedOnVesselDate);
					fol.CuttOfDate = (Conversion.CheckDateTimeNull(Conversion.ToDateTime((object)CuttOfDate)) ? null : CuttOfDate);
					fol.ETDFinalDate = (Conversion.CheckDateTimeNull(Conversion.ToDateTime((object)ETDDestinationPortDate)) ? null : ETDDestinationPortDate);
					fol.ReachedAtDestinationPort = (Conversion.CheckDateTimeNull(Conversion.ToDateTime((object)ReachedAtDestinationPortDate)) ? null : ReachedAtDestinationPortDate);
					fol.GdDate = (Conversion.CheckDateTimeNull(Conversion.ToDateTime((object)GoodsDeclarationDate)) ? null : GoodsDeclarationDate);
					fol.PortPayment = (Conversion.CheckDateTimeNull(Conversion.ToDateTime((object)PortPaymentDate)) ? null : PortPaymentDate);
					fol.ETDLoadingPort = (Conversion.CheckDateTimeNull(Conversion.ToDateTime((object)ETDLoadingPort)) ? null : ETDLoadingPort);
					obj.ShippedConsignmentFollowUpList.Add(fol);
				}
			}
			ExImShippedConsignmentFollowUps.MultiSave(obj);
			GridFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_FormattingRow(object sender, RowLoadEventArgs e)
	{
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Expected O, but got Unknown
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Expected O, but got Unknown
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Expected O, but got Unknown
		try
		{
			if (grdHistory.RowCount <= 0)
			{
				return;
			}
			DateTime CurrentDate = DateTime.Now;
			GridEXColumn CuttOfDate = grdHistory.RootTable.Columns["CuttOfDate"];
			GridEXColumn ETADestination = grdHistory.RootTable.Columns["ETDDestinationPort"];
			GridEXColumn ReachedAtDestinationPort = grdHistory.RootTable.Columns["ReachedAtDestinationPort"];
			if (CuttOfDate != null)
			{
				GridEXCell CuttOfDateCell = e.Row.Cells[((JanusCollectionElement)CuttOfDate).Index];
				DateTime date = Conversion.ToDateTime(CuttOfDateCell.Value).Date;
				DateTime currentDate3 = CurrentDate.Date;
				int daysRemaining3 = (date - currentDate3).Days;
				int Days3 = Conversion.ToInt((object)txtCutOffDateDays.Text);
				if (daysRemaining3 >= 0 && daysRemaining3 <= Days3)
				{
					CuttOfDateCell.FormatStyle = new GridEXFormatStyle();
					CuttOfDateCell.FormatStyle.ForeColor = Color.Red;
					CuttOfDateCell.FormatStyle.FontBold = (TriState)2;
				}
			}
			if (ETADestination != null)
			{
				GridEXCell ETADestinationCELL = e.Row.Cells[((JanusCollectionElement)ETADestination).Index];
				DateTime date2 = Conversion.ToDateTime(ETADestinationCELL.Value).Date;
				DateTime currentDate2 = CurrentDate.Date;
				int daysRemaining2 = (date2 - currentDate2).Days;
				int Days2 = Conversion.ToInt((object)txtETADestinationPortDays.Text);
				if (daysRemaining2 >= 0 && daysRemaining2 <= Days2)
				{
					ETADestinationCELL.FormatStyle = new GridEXFormatStyle();
					ETADestinationCELL.FormatStyle.ForeColor = Color.DarkGreen;
					ETADestinationCELL.FormatStyle.FontBold = (TriState)2;
				}
			}
			if (ReachedAtDestinationPort != null)
			{
				GridEXCell ReachedAtDestinationPortCELL = e.Row.Cells[((JanusCollectionElement)ReachedAtDestinationPort).Index];
				DateTime date3 = Conversion.ToDateTime(ReachedAtDestinationPortCELL.Value).Date;
				DateTime currentDate = CurrentDate.Date;
				int daysRemaining = (date3 - currentDate).Days;
				int Days = Conversion.ToInt((object)txtETADestinationPortDays.Text);
				if (daysRemaining >= 0 && daysRemaining <= Days)
				{
					ReachedAtDestinationPortCELL.FormatStyle = new GridEXFormatStyle();
					ReachedAtDestinationPortCELL.FormatStyle.ForeColor = Color.DarkGreen;
					ReachedAtDestinationPortCELL.FormatStyle.FontBold = (TriState)2;
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
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Expected O, but got Unknown
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Expected O, but got Unknown
		//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c4: Expected O, but got Unknown
		//IL_0354: Unknown result type (might be due to invalid IL or missing references)
		//IL_035e: Expected O, but got Unknown
		//IL_036a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0374: Expected O, but got Unknown
		//IL_11bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_11c5: Expected O, but got Unknown
		//IL_11d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_11dc: Expected O, but got Unknown
		//IL_11e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_11f3: Expected O, but got Unknown
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.ExportReports.frmshippedConsignment_followup));
		Appearance appearance18 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		GridEXLayout grdHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance1 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
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
		this.btnPrintRegister = new System.Windows.Forms.ToolStripButton();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnPrintSlip = new System.Windows.Forms.ToolStripButton();
		this.btnLookup = new System.Windows.Forms.ToolStripButton();
		this.btnshortcutkey = new System.Windows.Forms.ToolStripButton();
		this.panel2 = new System.Windows.Forms.Panel();
		this.InvoiceNo = new UltraCombo();
		this.label3 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.panel7 = new System.Windows.Forms.Panel();
		this.panel9 = new System.Windows.Forms.Panel();
		this.grdHistory = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.panel10 = new System.Windows.Forms.Panel();
		this.btnUpdateStatus = new System.Windows.Forms.Button();
		this.label13 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel3 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.label16 = new System.Windows.Forms.Label();
		this.label15 = new System.Windows.Forms.Label();
		this.txtETADestinationPortDays = new System.Windows.Forms.TextBox();
		this.txtCutOffDateDays = new System.Windows.Forms.TextBox();
		this.btnshow = new System.Windows.Forms.Button();
		this.cmbStatus = new UltraCombo();
		this.label12 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.txtPortPayment = new System.Windows.Forms.DateTimePicker();
		this.label10 = new System.Windows.Forms.Label();
		this.txtGoodsDeclarationDate = new System.Windows.Forms.DateTimePicker();
		this.txtGdNo = new System.Windows.Forms.TextBox();
		this.label9 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.datCuttOfDate = new System.Windows.Forms.DateTimePicker();
		this.txtReachedDestinationPortDate = new System.Windows.Forms.DateTimePicker();
		this.txtCourierTrackingNo = new System.Windows.Forms.TextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.cmbcurrsatatus = new UltraCombo();
		this.cmbcurstatus = new System.Windows.Forms.Label();
		this.cmbdocstatus = new UltraCombo();
		this.label8 = new System.Windows.Forms.Label();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label4 = new System.Windows.Forms.Label();
		this.txtETDFinalDate = new System.Windows.Forms.Label();
		this.txtETDFinalDate4 = new System.Windows.Forms.DateTimePicker();
		this.txtLoadedOnVesselDate = new System.Windows.Forms.Label();
		this.txtLoadedOnVesselDate3 = new System.Windows.Forms.DateTimePicker();
		this.txtReachedAtLoadingPortDate = new System.Windows.Forms.Label();
		this.txtReachedAtLoadingPortDate2 = new System.Windows.Forms.DateTimePicker();
		this.txtLoadOnTrainDate = new System.Windows.Forms.Label();
		this.txtLoadOnTrainDate1 = new System.Windows.Forms.DateTimePicker();
		this.label2 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.txtDocToParty = new System.Windows.Forms.DateTimePicker();
		this.txtDocToBank = new System.Windows.Forms.DateTimePicker();
		this.txtETDLoadingPort = new System.Windows.Forms.DateTimePicker();
		this.label17 = new System.Windows.Forms.Label();
		this.toolStrip1.SuspendLayout();
		this.panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.InvoiceNo).BeginInit();
		this.panel5.SuspendLayout();
		this.panel7.SuspendLayout();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).BeginInit();
		this.panel8.SuspendLayout();
		this.panel10.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel1.SuspendLayout();
		this.panel3.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbStatus).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcurrsatatus).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbdocstatus).BeginInit();
		this.panel6.SuspendLayout();
		base.SuspendLayout();
		this.btnPrintRegister.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrintRegister.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrintRegister.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrintRegister.Name = "btnPrintRegister";
		this.btnPrintRegister.Size = new System.Drawing.Size(150, 24);
		this.btnPrintRegister.Text = "512-Print Register";
		this.btnPrintRegister.Visible = false;
		this.btnPrintRegister.Click += new System.EventHandler(btnPrintRegister_Click);
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[8] { this.btnnew, this.btnSave, this.btnRefresh, this.btnUpdate, this.btnPrintSlip, this.btnPrintRegister, this.btnLookup, this.btnshortcutkey });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1158, 27);
		this.toolStrip1.TabIndex = 8;
		this.toolStrip1.Text = "toolStrip1";
		this.btnSave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(64, 24);
		this.btnSave.Text = "&Save";
		this.btnSave.Click += new System.EventHandler(btnSave_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "&Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnUpdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnPrintSlip.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrintSlip.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrintSlip.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrintSlip.Name = "btnPrintSlip";
		this.btnPrintSlip.Size = new System.Drawing.Size(120, 24);
		this.btnPrintSlip.Text = "511-Print Slip";
		this.btnPrintSlip.Visible = false;
		this.btnPrintSlip.Click += new System.EventHandler(btnPrintSlip_Click);
		this.btnLookup.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnLookup.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLookup.Image = (System.Drawing.Image)resources.GetObject("btnLookup.Image");
		this.btnLookup.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLookup.Name = "btnLookup";
		this.btnLookup.Size = new System.Drawing.Size(57, 24);
		this.btnLookup.Text = "Lookup";
		this.btnLookup.Click += new System.EventHandler(btnLookup_Click);
		this.btnshortcutkey.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnshortcutkey.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshortcutkey.Image = (System.Drawing.Image)resources.GetObject("btnshortcutkey.Image");
		this.btnshortcutkey.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnshortcutkey.Name = "btnshortcutkey";
		this.btnshortcutkey.Size = new System.Drawing.Size(106, 24);
		this.btnshortcutkey.Text = "ShortCut Keys";
		this.btnshortcutkey.Click += new System.EventHandler(btnshortcutkey_Click);
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1158, 30);
		this.panel2.TabIndex = 0;
		this.InvoiceNo.AllowNull = (DefaultableBoolean)1;
		this.InvoiceNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.InvoiceNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.InvoiceNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.InvoiceNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.InvoiceNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.InvoiceNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.InvoiceNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.InvoiceNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance23;
		((SpecialBoxBase)((UltraGridBase)this.InvoiceNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.InvoiceNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.InvoiceNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.InvoiceNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.InvoiceNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.InvoiceNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.InvoiceNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.InvoiceNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.InvoiceNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance22).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.InvoiceNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.InvoiceNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.InvoiceNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance24).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.InvoiceNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance25).TextHAlignAsString = "Left";
		((UltraGridBase)this.InvoiceNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.InvoiceNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.InvoiceNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.InvoiceNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.InvoiceNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.InvoiceNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.InvoiceNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.InvoiceNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.InvoiceNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.InvoiceNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.InvoiceNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.InvoiceNo).Location = new System.Drawing.Point(147, 74);
		((System.Windows.Forms.Control)(object)this.InvoiceNo).Name = "InvoiceNo";
		((System.Windows.Forms.Control)(object)this.InvoiceNo).Size = new System.Drawing.Size(141, 26);
		((System.Windows.Forms.Control)(object)this.InvoiceNo).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.InvoiceNo).Leave += new System.EventHandler(InvoiceNo_Leave);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(7, 78);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(65, 15);
		this.label3.TabIndex = 12;
		this.label3.Text = "Invoice No";
		this.panel5.Controls.Add(this.panel7);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1158, 395);
		this.panel5.TabIndex = 0;
		this.panel7.Controls.Add(this.panel9);
		this.panel7.Controls.Add(this.panel8);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1158, 395);
		this.panel7.TabIndex = 6;
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistory);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel9.Location = new System.Drawing.Point(0, 27);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1158, 368);
		this.panel9.TabIndex = 1;
		this.grdHistory.AllowEdit = (InheritableBoolean)2;
		grdHistory_DesignTimeLayout.LayoutString = resources.GetString("grdHistory_DesignTimeLayout.LayoutString");
		this.grdHistory.DesignTimeLayout = grdHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistory.FilterMode = (FilterMode)2;
		this.grdHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdHistory).Name = "grdHistory";
		((ScrollableBase)this.grdHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistory).Size = new System.Drawing.Size(1158, 368);
		((System.Windows.Forms.Control)(object)this.grdHistory).TabIndex = 5;
		this.grdHistory.TotalRow = (InheritableBoolean)1;
		this.grdHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistory).VisualStyle = (VisualStyle)4;
		this.grdHistory.FormattingRow += new RowLoadEventHandler(grdHistory_FormattingRow);
		this.grdHistory.ColumnButtonClick += new ColumnActionEventHandler(grdHistory_ColumnButtonClick);
		this.grdHistory.LinkClicked += new ColumnActionEventHandler(grdHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdHistory).DoubleClick += new System.EventHandler(grdHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(grdHistory_KeyDown);
		this.panel8.Controls.Add(this.panel10);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 0);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1158, 27);
		this.panel8.TabIndex = 0;
		this.panel10.BackColor = System.Drawing.Color.Teal;
		this.panel10.Controls.Add(this.btnUpdateStatus);
		this.panel10.Controls.Add(this.label13);
		this.panel10.Controls.Add(this.label5);
		this.panel10.Controls.Add(this.ctrlGrdBar1);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(0, 0);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1158, 27);
		this.panel10.TabIndex = 26;
		this.btnUpdateStatus.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnUpdateStatus.BackColor = System.Drawing.Color.White;
		this.btnUpdateStatus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateStatus.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateStatus.ForeColor = System.Drawing.Color.Teal;
		this.btnUpdateStatus.Location = new System.Drawing.Point(1011, 1);
		this.btnUpdateStatus.Name = "btnUpdateStatus";
		this.btnUpdateStatus.Size = new System.Drawing.Size(103, 25);
		this.btnUpdateStatus.TabIndex = 0;
		this.btnUpdateStatus.Text = "Update";
		this.btnUpdateStatus.UseVisualStyleBackColor = false;
		this.btnUpdateStatus.Click += new System.EventHandler(btnUpdateStatus_Click);
		this.label13.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(873, 6);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(132, 15);
		this.label13.TabIndex = 602;
		this.label13.Text = "Grd bar used for export ";
		this.label13.Visible = false;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.ForeColor = System.Drawing.Color.White;
		this.label5.Location = new System.Drawing.Point(4, 5);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(54, 16);
		this.label5.TabIndex = 0;
		this.label5.Text = "History";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1115, 4);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdHistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(39, 20);
		this.ctrlGrdBar1.TabIndex = 1;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.panel4.Controls.Add(this.panel5);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 214);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1158, 395);
		this.panel4.TabIndex = 2;
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1158, 609);
		this.panel1.TabIndex = 5;
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add(this.groupBox1);
		this.panel3.Controls.Add(this.label17);
		this.panel3.Controls.Add(this.label11);
		this.panel3.Controls.Add(this.txtETDLoadingPort);
		this.panel3.Controls.Add(this.txtPortPayment);
		this.panel3.Controls.Add(this.label10);
		this.panel3.Controls.Add(this.txtGoodsDeclarationDate);
		this.panel3.Controls.Add(this.txtGdNo);
		this.panel3.Controls.Add(this.label9);
		this.panel3.Controls.Add(this.label14);
		this.panel3.Controls.Add(this.label7);
		this.panel3.Controls.Add(this.datCuttOfDate);
		this.panel3.Controls.Add(this.txtReachedDestinationPortDate);
		this.panel3.Controls.Add(this.txtCourierTrackingNo);
		this.panel3.Controls.Add(this.label6);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbcurrsatatus);
		this.panel3.Controls.Add(this.cmbcurstatus);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbdocstatus);
		this.panel3.Controls.Add(this.label8);
		this.panel3.Controls.Add(this.panel6);
		this.panel3.Controls.Add(this.txtETDFinalDate);
		this.panel3.Controls.Add(this.txtETDFinalDate4);
		this.panel3.Controls.Add(this.txtLoadedOnVesselDate);
		this.panel3.Controls.Add(this.txtLoadedOnVesselDate3);
		this.panel3.Controls.Add(this.txtReachedAtLoadingPortDate);
		this.panel3.Controls.Add(this.txtReachedAtLoadingPortDate2);
		this.panel3.Controls.Add(this.txtLoadOnTrainDate);
		this.panel3.Controls.Add(this.txtLoadOnTrainDate1);
		this.panel3.Controls.Add(this.label2);
		this.panel3.Controls.Add(this.label1);
		this.panel3.Controls.Add(this.txtDocToParty);
		this.panel3.Controls.Add(this.txtDocToBank);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.InvoiceNo);
		this.panel3.Controls.Add(this.label3);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.panel3.Location = new System.Drawing.Point(0, 30);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1158, 184);
		this.panel3.TabIndex = 0;
		this.groupBox1.Controls.Add(this.label16);
		this.groupBox1.Controls.Add(this.label15);
		this.groupBox1.Controls.Add(this.txtETADestinationPortDays);
		this.groupBox1.Controls.Add(this.txtCutOffDateDays);
		this.groupBox1.Controls.Add(this.btnshow);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbStatus);
		this.groupBox1.Controls.Add(this.label12);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(4, 24);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1454, 46);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(562, 20);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(91, 15);
		this.label16.TabIndex = 40;
		this.label16.Text = "ETA Destination";
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(437, 19);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(73, 15);
		this.label15.TabIndex = 40;
		this.label15.Text = "Cut Off Date";
		this.txtETADestinationPortDays.Location = new System.Drawing.Point(658, 17);
		this.txtETADestinationPortDays.Name = "txtETADestinationPortDays";
		this.txtETADestinationPortDays.Size = new System.Drawing.Size(40, 23);
		this.txtETADestinationPortDays.TabIndex = 40;
		this.txtETADestinationPortDays.Text = "10";
		this.txtCutOffDateDays.Location = new System.Drawing.Point(516, 16);
		this.txtCutOffDateDays.Name = "txtCutOffDateDays";
		this.txtCutOffDateDays.Size = new System.Drawing.Size(40, 23);
		this.txtCutOffDateDays.TabIndex = 40;
		this.txtCutOffDateDays.Text = "3";
		this.btnshow.BackColor = System.Drawing.Color.Teal;
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(371, 15);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(59, 26);
		this.btnshow.TabIndex = 1;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.cmbStatus.AllowNull = (DefaultableBoolean)1;
		this.cmbStatus.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbStatus.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbStatus.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbStatus).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbStatus).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance44).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance44).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbStatus).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbStatus).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance45;
		((SpecialBoxBase)((UltraGridBase)this.cmbStatus).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance46).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbStatus).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.cmbStatus).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbStatus).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance47;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance2).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance2;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance3).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance4).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance4).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance4).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance4).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbStatus).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbStatus).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbStatus).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbStatus).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.cmbStatus).Location = new System.Drawing.Point(144, 15);
		((System.Windows.Forms.Control)(object)this.cmbStatus).Name = "cmbStatus";
		((System.Windows.Forms.Control)(object)this.cmbStatus).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.cmbStatus).TabIndex = 0;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(5, 20);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(100, 15);
		this.label12.TabIndex = 131;
		this.label12.Text = "Document Status";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(872, 134);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(79, 15);
		this.label11.TabIndex = 39;
		this.label11.Text = "Port Payment";
		this.txtPortPayment.Checked = false;
		this.txtPortPayment.CustomFormat = "dd-MMM-yy";
		this.txtPortPayment.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPortPayment.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtPortPayment.Location = new System.Drawing.Point(1010, 130);
		this.txtPortPayment.Name = "txtPortPayment";
		this.txtPortPayment.ShowCheckBox = true;
		this.txtPortPayment.Size = new System.Drawing.Size(141, 23);
		this.txtPortPayment.TabIndex = 14;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(872, 106);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(133, 15);
		this.label10.TabIndex = 37;
		this.label10.Text = "Goods Declaration Date";
		this.txtGoodsDeclarationDate.Checked = false;
		this.txtGoodsDeclarationDate.CustomFormat = "dd-MMM-yy";
		this.txtGoodsDeclarationDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGoodsDeclarationDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtGoodsDeclarationDate.Location = new System.Drawing.Point(1010, 102);
		this.txtGoodsDeclarationDate.Name = "txtGoodsDeclarationDate";
		this.txtGoodsDeclarationDate.ShowCheckBox = true;
		this.txtGoodsDeclarationDate.Size = new System.Drawing.Size(141, 23);
		this.txtGoodsDeclarationDate.TabIndex = 13;
		this.txtGdNo.Location = new System.Drawing.Point(1010, 75);
		this.txtGdNo.Margin = new System.Windows.Forms.Padding(2);
		this.txtGdNo.Name = "txtGdNo";
		this.txtGdNo.Size = new System.Drawing.Size(141, 23);
		this.txtGdNo.TabIndex = 12;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(872, 79);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(124, 15);
		this.label9.TabIndex = 35;
		this.label9.Text = "Goods Declaration No";
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(295, 82);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(108, 15);
		this.label14.TabIndex = 33;
		this.label14.Text = "Vessel Cut Off Date";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(566, 103);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(156, 15);
		this.label7.TabIndex = 9;
		this.label7.Text = "Reached At Destination Port";
		this.datCuttOfDate.Checked = false;
		this.datCuttOfDate.CustomFormat = "dd-MMM-yy";
		this.datCuttOfDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datCuttOfDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datCuttOfDate.Location = new System.Drawing.Point(419, 77);
		this.datCuttOfDate.Name = "datCuttOfDate";
		this.datCuttOfDate.ShowCheckBox = true;
		this.datCuttOfDate.Size = new System.Drawing.Size(141, 23);
		this.datCuttOfDate.TabIndex = 4;
		this.txtReachedDestinationPortDate.Checked = false;
		this.txtReachedDestinationPortDate.CustomFormat = "dd-MMM-yy";
		this.txtReachedDestinationPortDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtReachedDestinationPortDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtReachedDestinationPortDate.Location = new System.Drawing.Point(726, 99);
		this.txtReachedDestinationPortDate.Name = "txtReachedDestinationPortDate";
		this.txtReachedDestinationPortDate.ShowCheckBox = true;
		this.txtReachedDestinationPortDate.Size = new System.Drawing.Size(141, 23);
		this.txtReachedDestinationPortDate.TabIndex = 9;
		this.txtCourierTrackingNo.Location = new System.Drawing.Point(419, 155);
		this.txtCourierTrackingNo.Margin = new System.Windows.Forms.Padding(2);
		this.txtCourierTrackingNo.Name = "txtCourierTrackingNo";
		this.txtCourierTrackingNo.Size = new System.Drawing.Size(141, 23);
		this.txtCourierTrackingNo.TabIndex = 7;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(294, 159);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(112, 15);
		this.label6.TabIndex = 30;
		this.label6.Text = "Courier Tracking No";
		this.cmbcurrsatatus.AllowNull = (DefaultableBoolean)1;
		this.cmbcurrsatatus.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbcurrsatatus.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbcurrsatatus.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcurrsatatus).DisplayLayout.Appearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.cmbcurrsatatus).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcurrsatatus).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcurrsatatus).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcurrsatatus).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.cmbcurrsatatus).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcurrsatatus).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.cmbcurrsatatus).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcurrsatatus).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcurrsatatus).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance29;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcurrsatatus).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.cmbcurrsatatus).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcurrsatatus).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcurrsatatus).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcurrsatatus).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbcurrsatatus).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcurrsatatus).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcurrsatatus).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcurrsatatus).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.cmbcurrsatatus).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcurrsatatus).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcurrsatatus).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbcurrsatatus).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcurrsatatus).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmbcurrsatatus).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcurrsatatus).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcurrsatatus).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcurrsatatus).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbcurrsatatus.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbcurrsatatus).Location = new System.Drawing.Point(726, 152);
		((System.Windows.Forms.Control)(object)this.cmbcurrsatatus).Name = "cmbcurrsatatus";
		((System.Windows.Forms.Control)(object)this.cmbcurrsatatus).Size = new System.Drawing.Size(141, 26);
		((System.Windows.Forms.Control)(object)this.cmbcurrsatatus).TabIndex = 11;
		((System.Windows.Forms.Control)(object)this.cmbcurrsatatus).Leave += new System.EventHandler(cmbcurrsatatus_Leave);
		this.cmbcurstatus.AutoSize = true;
		this.cmbcurstatus.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbcurstatus.Location = new System.Drawing.Point(566, 158);
		this.cmbcurstatus.Name = "cmbcurstatus";
		this.cmbcurstatus.Size = new System.Drawing.Size(82, 15);
		this.cmbcurstatus.TabIndex = 11;
		this.cmbcurstatus.Text = "Current\u00a0Status";
		this.cmbdocstatus.AllowNull = (DefaultableBoolean)1;
		this.cmbdocstatus.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbdocstatus.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbdocstatus.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbdocstatus).DisplayLayout.Appearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.cmbdocstatus).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbdocstatus).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbdocstatus).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbdocstatus).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance33;
		((SpecialBoxBase)((UltraGridBase)this.cmbdocstatus).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance35).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance35).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbdocstatus).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbdocstatus).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbdocstatus).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbdocstatus).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbdocstatus).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.cmbdocstatus).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbdocstatus).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbdocstatus).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance39).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbdocstatus).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbdocstatus).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbdocstatus).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbdocstatus).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbdocstatus).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.cmbdocstatus).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbdocstatus).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbdocstatus).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbdocstatus).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbdocstatus).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.cmbdocstatus).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbdocstatus).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbdocstatus).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbdocstatus).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbdocstatus.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbdocstatus).Location = new System.Drawing.Point(726, 124);
		((System.Windows.Forms.Control)(object)this.cmbdocstatus).Name = "cmbdocstatus";
		((System.Windows.Forms.Control)(object)this.cmbdocstatus).Size = new System.Drawing.Size(141, 26);
		((System.Windows.Forms.Control)(object)this.cmbdocstatus).TabIndex = 10;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(566, 130);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(100, 15);
		this.label8.TabIndex = 10;
		this.label8.Text = "Document Status";
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.label4);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1158, 23);
		this.panel6.TabIndex = 25;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.White;
		this.label4.Location = new System.Drawing.Point(4, 0);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(270, 19);
		this.label4.TabIndex = 0;
		this.label4.Text = "Shipping Consignment FollowUp";
		this.txtETDFinalDate.AutoSize = true;
		this.txtETDFinalDate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtETDFinalDate.Location = new System.Drawing.Point(566, 77);
		this.txtETDFinalDate.Name = "txtETDFinalDate";
		this.txtETDFinalDate.Size = new System.Drawing.Size(116, 15);
		this.txtETDFinalDate.TabIndex = 8;
		this.txtETDFinalDate.Text = "ETA Destination Port";
		this.txtETDFinalDate4.Checked = false;
		this.txtETDFinalDate4.CustomFormat = "dd-MMM-yy";
		this.txtETDFinalDate4.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtETDFinalDate4.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtETDFinalDate4.Location = new System.Drawing.Point(726, 74);
		this.txtETDFinalDate4.Name = "txtETDFinalDate4";
		this.txtETDFinalDate4.ShowCheckBox = true;
		this.txtETDFinalDate4.Size = new System.Drawing.Size(141, 23);
		this.txtETDFinalDate4.TabIndex = 8;
		this.txtLoadedOnVesselDate.AutoSize = true;
		this.txtLoadedOnVesselDate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtLoadedOnVesselDate.Location = new System.Drawing.Point(7, 159);
		this.txtLoadedOnVesselDate.Name = "txtLoadedOnVesselDate";
		this.txtLoadedOnVesselDate.Size = new System.Drawing.Size(100, 15);
		this.txtLoadedOnVesselDate.TabIndex = 22;
		this.txtLoadedOnVesselDate.Text = "Loaded On Vessel";
		this.txtLoadedOnVesselDate3.Checked = false;
		this.txtLoadedOnVesselDate3.CustomFormat = "dd-MMM-yy";
		this.txtLoadedOnVesselDate3.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtLoadedOnVesselDate3.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtLoadedOnVesselDate3.Location = new System.Drawing.Point(147, 155);
		this.txtLoadedOnVesselDate3.Name = "txtLoadedOnVesselDate3";
		this.txtLoadedOnVesselDate3.ShowCheckBox = true;
		this.txtLoadedOnVesselDate3.Size = new System.Drawing.Size(141, 23);
		this.txtLoadedOnVesselDate3.TabIndex = 3;
		this.txtReachedAtLoadingPortDate.AutoSize = true;
		this.txtReachedAtLoadingPortDate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtReachedAtLoadingPortDate.Location = new System.Drawing.Point(7, 133);
		this.txtReachedAtLoadingPortDate.Name = "txtReachedAtLoadingPortDate";
		this.txtReachedAtLoadingPortDate.Size = new System.Drawing.Size(138, 15);
		this.txtReachedAtLoadingPortDate.TabIndex = 20;
		this.txtReachedAtLoadingPortDate.Text = "Reached At Loading Port";
		this.txtReachedAtLoadingPortDate2.Checked = false;
		this.txtReachedAtLoadingPortDate2.CustomFormat = "dd-MMM-yy";
		this.txtReachedAtLoadingPortDate2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtReachedAtLoadingPortDate2.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtReachedAtLoadingPortDate2.Location = new System.Drawing.Point(147, 129);
		this.txtReachedAtLoadingPortDate2.Name = "txtReachedAtLoadingPortDate2";
		this.txtReachedAtLoadingPortDate2.ShowCheckBox = true;
		this.txtReachedAtLoadingPortDate2.Size = new System.Drawing.Size(141, 23);
		this.txtReachedAtLoadingPortDate2.TabIndex = 2;
		this.txtLoadOnTrainDate.AutoSize = true;
		this.txtLoadOnTrainDate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtLoadOnTrainDate.Location = new System.Drawing.Point(7, 105);
		this.txtLoadOnTrainDate.Name = "txtLoadOnTrainDate";
		this.txtLoadOnTrainDate.Size = new System.Drawing.Size(94, 15);
		this.txtLoadOnTrainDate.TabIndex = 18;
		this.txtLoadOnTrainDate.Text = "Loaded On Train";
		this.txtLoadOnTrainDate1.Checked = false;
		this.txtLoadOnTrainDate1.CustomFormat = "dd-MMM-yy";
		this.txtLoadOnTrainDate1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtLoadOnTrainDate1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtLoadOnTrainDate1.Location = new System.Drawing.Point(147, 101);
		this.txtLoadOnTrainDate1.Name = "txtLoadOnTrainDate1";
		this.txtLoadOnTrainDate1.ShowCheckBox = true;
		this.txtLoadOnTrainDate1.Size = new System.Drawing.Size(141, 23);
		this.txtLoadOnTrainDate1.TabIndex = 1;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(295, 133);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(110, 15);
		this.label2.TabIndex = 16;
		this.label2.Text = "Document To Party";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(294, 105);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(109, 15);
		this.label1.TabIndex = 15;
		this.label1.Text = "Document To Bank";
		this.txtDocToParty.Checked = false;
		this.txtDocToParty.CustomFormat = "dd-MMM-yy";
		this.txtDocToParty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocToParty.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDocToParty.Location = new System.Drawing.Point(419, 129);
		this.txtDocToParty.Name = "txtDocToParty";
		this.txtDocToParty.ShowCheckBox = true;
		this.txtDocToParty.Size = new System.Drawing.Size(141, 23);
		this.txtDocToParty.TabIndex = 6;
		this.txtDocToBank.Checked = false;
		this.txtDocToBank.CustomFormat = "dd-MMM-yy";
		this.txtDocToBank.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocToBank.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDocToBank.Location = new System.Drawing.Point(419, 101);
		this.txtDocToBank.Name = "txtDocToBank";
		this.txtDocToBank.ShowCheckBox = true;
		this.txtDocToBank.Size = new System.Drawing.Size(141, 23);
		this.txtDocToBank.TabIndex = 5;
		this.txtETDLoadingPort.Checked = false;
		this.txtETDLoadingPort.CustomFormat = "dd-MMM-yy";
		this.txtETDLoadingPort.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtETDLoadingPort.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtETDLoadingPort.Location = new System.Drawing.Point(1010, 154);
		this.txtETDLoadingPort.Name = "txtETDLoadingPort";
		this.txtETDLoadingPort.ShowCheckBox = true;
		this.txtETDLoadingPort.Size = new System.Drawing.Size(141, 23);
		this.txtETDLoadingPort.TabIndex = 15;
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(872, 158);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(100, 15);
		this.label17.TabIndex = 39;
		this.label17.Text = "ETD Loading Port";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1158, 609);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmshippedConsignment_followup";
		this.Text = "EccountBookERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmshippedConsignment_followup_FormClosed);
		base.Load += new System.EventHandler(frmshippedConsignment_followup_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmshippedConsignment_followup_KeyDown);
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.InvoiceNo).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		this.panel9.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistory).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel1.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbStatus).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcurrsatatus).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbdocstatus).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
