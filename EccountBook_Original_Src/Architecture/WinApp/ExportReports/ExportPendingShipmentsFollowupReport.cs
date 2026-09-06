using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Export;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Export;
using Architecture.Model.Inventory;
using Architecture.Reports;
using Architecture.WinApp.Common;
using Architecture.WinApp.Export;
using Architecture.WinApp.Inventory_Reports;
using Architecture.WinApp.Properties;
using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.ExportReports;

public class ExportPendingShipmentsFollowupReport : Form
{
	private DropDownBind DDL = new DropDownBind();

	private UserAccount UserAccount = new UserAccount();

	private DataTable dt1 = new DataTable();

	private DataTable grdTable = new DataTable();

	private DataTable dtStatus = new DataTable();

	private bool FormInitialized;

	private IContainer components;

	private ToolStrip toolStrip3;

	private ToolStripButton btnNew;

	private ToolStripButton btnRefresh;

	private ToolStripButton btnShortCutKeys;

	private Panel panel1;

	private Panel panel2;

	internal UltraCombo cmbsupplierCustomer;

	private Label label7;

	private Label label8;

	internal UltraCombo cmbdestinationport;

	private Panel panel8;

	private Label label9;

	private Panel panel3;

	private Label label1;

	private CtrlGrdBar ctrlGrdBar1;

	private Button btnshow;

	private GroupBox groupBox1;

	private ToolStripButton btnPrintCurrent;

	internal UltraCombo cmbStatus;

	private Label label2;

	private RadioButton rdNotRealized;

	private RadioButton rdRealized;

	private RadioButton rdAll;

	private ToolStripButton BtnShipmentForm;

	private ToolStripSeparator toolStripSeparator5;

	private ToolStripSeparator toolStripSeparator4;

	private ToolStripSeparator toolStripSeparator3;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripSeparator toolStripSeparator1;

	private RadioButton RadBlReceivedButPaymentNotShipments;

	private RadioButton RadForwardingButBLNotShipments;

	private RadioButton RadForwardingInProcessShipments;

	private RadioButton RadAllShipments;

	internal GridEX grd;

	internal GridEX grdEtaDestination;

	private ToolStripButton btnDHLTracking;

	internal UltraCombo CmbCurrentStatus;

	private Label label3;

	public ExportPendingShipmentsFollowupReport(UserAccount user)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void ExportPendingShipmentsFollowupReport_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1000, 550);
			panel2.Height = 152;
			FormInitialized = true;
			AllcomboBind();
			Statusfill();
			CurrentStatusfill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Statusfill()
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		try
		{
			dtStatus.Rows.Clear();
			int id = Conversion.ToInt(cmbStatus.Value);
			ExImLookUps obj = new ExImLookUps();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.ExImLookUptypesId = 5;
			dtStatus = ExImLookUps.GetByLookTypeId(obj);
			if (dtStatus.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtStatus, cmbStatus, "Id", "LookUpName", "Document Status", false);
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
			}
			if (id > 0)
			{
				if (dtStatus.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
				{
					cmbStatus.Value = id;
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
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		try
		{
			DataTable dt = ExImLookUps.GetByLookTypeId(new ExImLookUps
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				ExImLookUptypesId = 6
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbCurrentStatus, "Id", "LookUpName", "Current Status", true);
				UltraGridColumn obj = ((UltraGridBase)CmbCurrentStatus).DisplayLayout.Bands[0].Columns.Add();
				((KeyedSubObjectBase)obj).Key = "Selected";
				((HeaderBase)obj.Header).Caption = string.Empty;
				obj.Header.CheckBoxVisibility = (HeaderCheckBoxVisibility)2;
				obj.DataType = typeof(bool);
				((HeaderBase)obj.Header).VisiblePosition = 0;
				CmbCurrentStatus.CheckedListSettings.CheckStateMember = "Selected";
				((EditorCheckedListSettings)CmbCurrentStatus.CheckedListSettings).EditorValueSource = (EditorWithComboValueSource)1;
				((EditorCheckedListSettings)CmbCurrentStatus.CheckedListSettings).ListSeparator = ",";
				((EditorCheckedListSettings)CmbCurrentStatus.CheckedListSettings).ItemCheckArea = (ItemCheckArea)1;
				((UltraGridBase)CmbCurrentStatus).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			else
			{
				((Control)(object)CmbCurrentStatus).Text = string.Empty;
				((UltraGridBase)CmbCurrentStatus).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AllcomboBind()
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		try
		{
			int Id = Conversion.ToInt(cmbsupplierCustomer.Value);
			int Id2 = Conversion.ToInt(cmbdestinationport.Value);
			DataTable dt = ExImLcOrder.GetDataForDropDownFromExportInvoice(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			DataTable dtSupplier = new DataTable();
			dtSupplier.Columns.Add("Id", typeof(int));
			dtSupplier.Columns.Add("Customer");
			DataTable dtPort = new DataTable();
			dtPort.Columns.Add("Id", typeof(int));
			dtPort.Columns.Add("Port");
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToString(dt.Rows[i]["ActivityType"]) == "Customer")
					{
						dtSupplier.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["name"]);
					}
					else if (Conversion.ToString(dt.Rows[i]["ActivityType"]) == "DestinationPort")
					{
						dtPort.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["name"]);
					}
				}
			}
			DDL.BindDDL(dtSupplier, cmbsupplierCustomer, "Id", "Customer", "Customer", ZeroIndex: false);
			((UltraGridBase)cmbsupplierCustomer).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			DDL.BindDDL(dtPort, cmbdestinationport, "Id", "Port", "Destination Port", ZeroIndex: false);
			((UltraGridBase)cmbsupplierCustomer).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			if (Id > 0)
			{
				if (dtSupplier.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
				{
					cmbsupplierCustomer.Value = Id;
				}
				else
				{
					((Control)(object)cmbsupplierCustomer).Text = string.Empty;
				}
			}
			if (Id2 > 0)
			{
				if (dtPort.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id2))
				{
					cmbdestinationport.Value = Id2;
				}
				else
				{
					((Control)(object)cmbdestinationport).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Distinationportfill()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		try
		{
			int id = Conversion.ToInt(cmbdestinationport.Value);
			DataTable dtDestinationPort = SeaPorts.Getall(new SeaPorts
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtDestinationPort.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtDestinationPort, cmbdestinationport, "Id", "PortName", "Destination Port", true);
				((UltraGridBase)cmbdestinationport).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				((UltraGridBase)cmbdestinationport).Rows[0].Activate();
			}
			if (id > 0)
			{
				if (dtDestinationPort.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
				{
					cmbdestinationport.Value = id;
				}
				else
				{
					((Control)(object)cmbdestinationport).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Gridfill(int SortNo = 0)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		try
		{
			string status = "";
			dt1.Rows.Clear();
			grdTable.Clear();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.SupplierCustomerId = Conversion.ToInt(cmbsupplierCustomer.Value);
			obj.DestinationPortId = Conversion.ToInt(cmbdestinationport.Value);
			if (((Control)(object)cmbStatus).Text != null && ((Control)(object)cmbStatus).Text != string.Empty)
			{
				string[] array = (Conversion.ToString((object)((Control)(object)cmbStatus).Text.Trim()) + ",").Split(',');
				foreach (string author in array)
				{
					DataRow[] dr = dtStatus.Select("LookUpName='" + Conversion.ToString((object)author) + "'");
					if (dr.Length != 0)
					{
						status = status + "," + Conversion.ToString(dr[0].ItemArray[0]);
					}
				}
			}
			obj.Status = status;
			string CurrentStatusIds = InfragisticsHelper.GetSelectedIdsFromMultiSelectionCombo(((Control)(object)CmbCurrentStatus).Text, (DataTable)((UltraGridBase)CmbCurrentStatus).DataSource, "LookUpName");
			obj.ActivityId = (rdNotRealized.Checked ? 1 : (rdRealized.Checked ? 2 : obj.ActivityId));
			obj.StatusId = ((!RadAllShipments.Checked) ? (RadForwardingInProcessShipments.Checked ? 1 : (RadForwardingButBLNotShipments.Checked ? 2 : (RadBlReceivedButPaymentNotShipments.Checked ? 3 : 0))) : 0);
			obj.SortNo = SortNo;
			obj.CurrentStatusIds = CurrentStatusIds;
			DataSet ds = ExImShippedConsignmentFollowUps.ExportPendingShipmentsFollowup(obj);
			if (ds.Tables.Count > 0)
			{
				dt1 = ds.Tables[0];
				if (dt1.Rows.Count > 0)
				{
					DataTable dt = new DataTable();
					dt.Columns.Add("InvoiceDocumentTypeId", typeof(int));
					dt.Columns.Add("InvoiceId", typeof(int));
					dt.Columns.Add("ContractId", typeof(int));
					dt.Columns.Add("Customer", typeof(string));
					dt.Columns.Add("LcOrderNo", typeof(string));
					dt.Columns.Add("InvoiceNo", typeof(string));
					dt.Columns.Add("InvoiceDate", typeof(DateTime));
					dt.Columns.Add("FclQty", typeof(double));
					dt.Columns.Add("Mtons", typeof(double));
					dt.Columns.Add("PortName", typeof(string));
					dt.Columns.Add("InspectionDate", typeof(DateTime));
					dt.Columns.Add("CroBooking", typeof(DateTime));
					dt.Columns.Add("ETADate", typeof(DateTime));
					dt.Columns.Add("ETDDate", typeof(DateTime));
					dt.Columns.Add("CutOffDate", typeof(DateTime));
					dt.Columns.Add("LoadingStart", typeof(DateTime));
					dt.Columns.Add("LoadingEnd", typeof(DateTime));
					dt.Columns.Add("LoadOnTrainDate", typeof(DateTime));
					dt.Columns.Add("ReachedAtLoadingPortDate", typeof(DateTime));
					dt.Columns.Add("LoadedOnVesselDate", typeof(DateTime));
					dt.Columns.Add("BLDate", typeof(DateTime));
					dt.Columns.Add("DocumentStatus", typeof(string));
					dt.Columns.Add("DocToBank", typeof(DateTime));
					dt.Columns.Add("DocToParty", typeof(DateTime));
					dt.Columns.Add("CurrentStatus", typeof(string));
					dt.Columns.Add("ETADestination", typeof(DateTime));
					dt.Columns.Add("ExpectedInTransitDays", typeof(int));
					dt.Columns.Add("InTransitDays", typeof(int));
					dt.Columns.Add("ETDFinalDate", typeof(DateTime));
					dt.Columns.Add("FcyId", typeof(int));
					dt.Columns.Add("FcyCode", typeof(string));
					dt.Columns.Add("InvoiceValue", typeof(double));
					dt.Columns.Add("InvoiceStatus", typeof(string));
					dt.Columns.Add("RealizedAmount", typeof(double));
					dt.Columns.Add("BalAmount", typeof(double));
					dt.Columns.Add("ShippingLine", typeof(string));
					dt.Columns.Add("BLNumber", typeof(string));
					dt.Columns.Add("NoOfAttachments", typeof(int));
					grdTable = dt.Clone();
					for (int j = 0; j < dt1.Rows.Count; j++)
					{
						dt.Rows.Add(Conversion.ToInt(dt1.Rows[j]["InvoiceDocumentTypeId"]), Conversion.ToInt(dt1.Rows[j]["InvoiceId"]), Conversion.ToInt(dt1.Rows[j]["ContractId"]), Conversion.ToString(dt1.Rows[j]["Customer"]), Conversion.ToString(dt1.Rows[j]["LcOrderNo"]), Conversion.ToString(dt1.Rows[j]["InvoiceNo"]), Conversion.ToDateTime(dt1.Rows[j]["InvoiceDate"]).ToShortDateString(), Conversion.ToDouble(dt1.Rows[j]["FclQty"]), Conversion.ToDouble(dt1.Rows[j]["Mtons"]), Conversion.ToString(dt1.Rows[j]["PortName"]), dt1.Rows[j]["InspectionDate"], dt1.Rows[j]["CroBooking"], dt1.Rows[j]["ETADestination"], dt1.Rows[j]["ETDDate"], dt1.Rows[j]["CutOffDate"], dt1.Rows[j]["LoadingStart"], dt1.Rows[j]["LoadingEnd"], dt1.Rows[j]["LoadOnTrainDate"], dt1.Rows[j]["ReachedAtLoadingPortDate"], dt1.Rows[j]["LoadedOnVesselDate"], dt1.Rows[j]["BLDate"], Conversion.ToString(dt1.Rows[j]["DocumentStatus"]), dt1.Rows[j]["DocToBank"], dt1.Rows[j]["DocToParty"], Conversion.ToString(dt1.Rows[j]["CurrentStatus"]), dt1.Rows[j]["ETADestination"], Conversion.ToInt(dt1.Rows[j]["ExpectedInTransitDays"]), Conversion.ToInt(dt1.Rows[j]["InTransitDays"]), dt1.Rows[j]["ETDFinalDate"], Conversion.ToString(dt1.Rows[j]["FcyId"]), Conversion.ToString(dt1.Rows[j]["CurrencyCode"]), Conversion.ToDouble(dt1.Rows[j]["InvoiceValue"]), Conversion.ToString(dt1.Rows[j]["InvoiceStatus"]), Conversion.ToDouble(dt1.Rows[j]["RealizedAmount"]), Conversion.ToDouble(dt1.Rows[j]["BalAmount"]), Conversion.ToString(dt1.Rows[j]["ShippingLine"]), Conversion.ToString(dt1.Rows[j]["BLNumber"]), Conversion.ToInt(dt1.Rows[j]["NoOfAttachments"]));
						grdTable.ImportRow(dt.Rows[j]);
					}
					grd.DataSource = dt;
					grd.RetrieveStructure();
					grdsetting();
				}
				else
				{
					grd.DataSource = null;
				}
				DataTable dtEta = new DataTable();
				dtEta.Columns.Add("SortNo", typeof(int));
				dtEta.Columns.Add("ETA-DestinationPort");
				dtEta.Columns.Add("M.Tons", typeof(double));
				dtEta.Columns.Add("FCL", typeof(double));
				DataTable dt2 = ((ds.Tables[1].Rows.Count > 0) ? ds.Tables[1] : null);
				if (dt2.Rows.Count > 0)
				{
					for (int i = 0; i < dt2.Rows.Count; i++)
					{
						dtEta.Rows.Add(dt2.Rows[i]["SortNo"], dt2.Rows[i]["ETADestinationPort"], dt2.Rows[i]["Mtons"], dt2.Rows[i]["FCL"]);
					}
					grdEtaDestination.DataSource = dtEta;
					grdEtaDestination.RetrieveStructure();
					grdEtaDestination.RootTable.Columns["SortNo"].Visible = false;
					grdEtaDestination.RootTable.Columns["ETA-DestinationPort"].Width = 325;
					grdEtaDestination.RootTable.Columns["M.Tons"].Width = 70;
					grdEtaDestination.RootTable.Columns["FCL"].Width = 65;
					grdEtaDestination.RootTable.Columns["M.Tons"].AggregateFunction = (AggregateFunction)2;
					grdEtaDestination.RootTable.Columns["M.Tons"].TextAlignment = (TextAlignment)3;
					grdEtaDestination.RootTable.Columns["M.Tons"].FormatString = "#,##0.###";
					grdEtaDestination.RootTable.Columns["M.Tons"].TotalFormatString = "#,##0.###";
					grdEtaDestination.RootTable.Columns["FCL"].AggregateFunction = (AggregateFunction)2;
					grdEtaDestination.RootTable.Columns["FCL"].TextAlignment = (TextAlignment)3;
					grdEtaDestination.RootTable.Columns["FCL"].FormatString = "#,##0.###";
					grdEtaDestination.RootTable.Columns["FCL"].TotalFormatString = "#,##0.###";
					grdEtaDestination.TotalRow = (InheritableBoolean)1;
				}
				else
				{
					grdEtaDestination.ClearStructure();
				}
			}
			else
			{
				grd.DataSource = null;
				grdEtaDestination.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grdsetting()
	{
		//IL_04e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c16: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c1c: Expected O, but got Unknown
		//IL_0c78: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c7e: Expected O, but got Unknown
		try
		{
			grd.DefaultFilterRowComparison = (FilterConditionOperator)7;
			grd.RootTable.Columns["InvoiceDocumentTypeId"].Visible = false;
			grd.RootTable.Columns["InvoiceId"].Visible = false;
			grd.RootTable.Columns["ContractId"].Visible = false;
			grd.RootTable.Columns["FcyId"].Visible = false;
			grd.RootTable.Columns["InvoiceStatus"].Visible = false;
			grd.RootTable.Columns["RealizedAmount"].Visible = false;
			grd.RootTable.Columns["InvoiceValue"].Visible = false;
			grd.RootTable.Columns["BalAmount"].Visible = false;
			grd.RootTable.Columns["ShippingLine"].Visible = false;
			grd.RootTable.Columns["BLNumber"].Visible = false;
			grd.RootTable.Columns["FcyCode"].Visible = false;
			grd.RootTable.Columns["InvoiceNo"].ColumnType = (ColumnType)5;
			grd.RootTable.Columns["CroBooking"].ColumnType = (ColumnType)5;
			grd.RootTable.Columns["LoadingStart"].ColumnType = (ColumnType)5;
			grd.RootTable.Columns["LoadingEnd"].ColumnType = (ColumnType)5;
			grd.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grd.RootTable.Columns["LcOrderNo"].Caption = "Contract";
			grd.RootTable.Columns["InvoiceNo"].Caption = "Invoice";
			grd.RootTable.Columns["FclQty"].Caption = "FCL";
			grd.RootTable.Columns["Mtons"].Caption = "M.Tons";
			grd.RootTable.Columns["PortName"].Caption = "Destination Port";
			grd.RootTable.Columns["CroBooking"].Caption = "CRO Booked";
			grd.RootTable.Columns["ETADate"].Caption = "Loading Port E.T.A";
			grd.RootTable.Columns["ETDDate"].Caption = "Loading Port E.T.D";
			grd.RootTable.Columns["ETADestination"].Caption = "ETA Destination";
			grd.RootTable.Columns["LoadingStart"].Caption = "Dispatch Started";
			grd.RootTable.Columns["LoadingEnd"].Caption = "Dispatch Completed";
			grd.RootTable.Columns["LoadOnTrainDate"].Caption = "Load On Train";
			grd.RootTable.Columns["ReachedAtLoadingPortDate"].Caption = "Reached At Port";
			grd.RootTable.Columns["LoadedOnVesselDate"].Caption = "Loaded On Vessel";
			grd.RootTable.Columns["DocToBank"].Caption = "Doc To Bank Date";
			grd.RootTable.Columns["DocToParty"].Caption = "Doc To Party Date";
			grd.RootTable.Columns["ETDFinalDate"].Caption = "Reached At Destination Port";
			grd.RootTable.Columns["FcyCode"].Caption = "Fcy";
			grd.RootTable.Columns["BalAmount"].Caption = "Balance Amount";
			grd.RootTable.HeaderLines = 3;
			foreach (GridEXColumn item in (JanusCollectionBase)grd.RootTable.Columns)
			{
				item.HeaderStyle.TextAlignment = (TextAlignment)2;
			}
			grd.RootTable.Columns["InvoiceDate"].FormatString = "dd-MMM-yy";
			grd.RootTable.Columns["InspectionDate"].FormatString = "dd-MMM-yy";
			grd.RootTable.Columns["CroBooking"].FormatString = "dd-MMM-yy";
			grd.RootTable.Columns["ETADate"].FormatString = "dd-MMM-yy";
			grd.RootTable.Columns["ETDDate"].FormatString = "dd-MMM-yy";
			grd.RootTable.Columns["LoadingStart"].FormatString = "dd-MMM-yy";
			grd.RootTable.Columns["LoadingEnd"].FormatString = "dd-MMM-yy";
			grd.RootTable.Columns["LoadOnTrainDate"].FormatString = "dd-MMM-yy";
			grd.RootTable.Columns["ReachedAtLoadingPortDate"].FormatString = "dd-MMM-yy";
			grd.RootTable.Columns["CutOffDate"].FormatString = "dd-MMM-yy";
			grd.RootTable.Columns["LoadedOnVesselDate"].FormatString = "dd-MMM-yy";
			grd.RootTable.Columns["BLDate"].FormatString = "dd-MMM-yy";
			grd.RootTable.Columns["DocToBank"].FormatString = "dd-MMM-yy";
			grd.RootTable.Columns["DocToParty"].FormatString = "dd-MMM-yy";
			grd.RootTable.Columns["ETADestination"].FormatString = "dd-MMM-yy";
			grd.RootTable.Columns["ETDFinalDate"].FormatString = "dd-MMM-yy";
			grd.RootTable.Columns["LcOrderNo"].Width = Constants.ExportConstants.ContractNoConstant;
			grd.RootTable.Columns["InvoiceNo"].Width = Constants.ExportConstants.InvoiceNoConstant;
			grd.RootTable.Columns["InvoiceDate"].Width = Constants.ExportConstants.DateConstant;
			grd.RootTable.Columns["Customer"].Width = Constants.ExportConstants.StringConstant;
			grd.RootTable.Columns["PortName"].Width = Constants.ExportConstants.PortConstant;
			grd.RootTable.Columns["FcyCode"].Width = Constants.ExportConstants.FcyConstant;
			grd.RootTable.Columns["FcyCode"].TextAlignment = (TextAlignment)2;
			grd.RootTable.Columns["InvoiceValue"].Width = Constants.ExportConstants.AmountConstant;
			grd.RootTable.Columns["InvoiceStatus"].Width = Constants.ExportConstants.StatusConstant;
			grd.RootTable.Columns["RealizedAmount"].Width = Constants.ExportConstants.AmountConstant;
			grd.RootTable.Columns["BalAmount"].Width = Constants.ExportConstants.AmountConstant;
			grd.RootTable.Columns["FclQty"].Width = Constants.ExportConstants.FclConstant;
			grd.RootTable.Columns["Mtons"].Width = Constants.ExportConstants.FclConstant;
			grd.RootTable.Columns["InspectionDate"].Width = Constants.ExportConstants.DateConstant;
			grd.RootTable.Columns["CroBooking"].Width = Constants.ExportConstants.DateConstant;
			grd.RootTable.Columns["ShippingLine"].Width = Constants.ExportConstants.DateConstant;
			grd.RootTable.Columns["ETADate"].Width = Constants.ExportConstants.DateConstant;
			grd.RootTable.Columns["ETDDate"].Width = Constants.ExportConstants.DateConstant;
			grd.RootTable.Columns["LoadingStart"].Width = Constants.ExportConstants.DateConstant;
			grd.RootTable.Columns["LoadingEnd"].Width = Constants.ExportConstants.DateConstant;
			grd.RootTable.Columns["LoadOnTrainDate"].Width = Constants.ExportConstants.DateConstant;
			grd.RootTable.Columns["ReachedAtLoadingPortDate"].Width = Constants.ExportConstants.DateConstant;
			grd.RootTable.Columns["CutOffDate"].Width = Constants.ExportConstants.DateConstant;
			grd.RootTable.Columns["LoadedOnVesselDate"].Width = Constants.ExportConstants.DateConstant;
			grd.RootTable.Columns["BLDate"].Width = Constants.ExportConstants.DateConstant;
			grd.RootTable.Columns["BLNumber"].Width = Constants.ExportConstants.DateConstant;
			grd.RootTable.Columns["DocToBank"].Width = Constants.ExportConstants.DateConstant;
			grd.RootTable.Columns["DocToParty"].Width = Constants.ExportConstants.DateConstant;
			grd.RootTable.Columns["ExpectedInTransitDays"].Width = Constants.ExportConstants.DateConstant;
			grd.RootTable.Columns["InTransitDays"].Width = Constants.ExportConstants.DateConstant;
			grd.RootTable.Columns["ETDFinalDate"].Width = 100;
			grd.RootTable.Columns["DocumentStatus"].Width = 120;
			grd.RootTable.Columns["CurrentStatus"].Width = 150;
			GridEXColumn column = grd.RootTable.Columns["ETDFinalDate"];
			GridEXFormatCondition fc = new GridEXFormatCondition(column, (ConditionOperator)13, (object)"");
			fc.TargetColumn = column;
			fc.FormatStyle.FontBold = (TriState)2;
			fc.FormatStyle.ForeColor = Color.DarkOrange;
			grd.RootTable.FormatConditions.Add(fc);
			GridEXColumn column2 = grd.RootTable.Columns["ETADestination"];
			GridEXFormatCondition fc2 = new GridEXFormatCondition(column2, (ConditionOperator)13, (object)"");
			fc2.TargetColumn = column2;
			fc2.FormatStyle.FontBold = (TriState)2;
			fc2.FormatStyle.ForeColor = Color.DarkOrange;
			grd.RootTable.FormatConditions.Add(fc2);
			grd.RootTable.Columns["InvoiceValue"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["InvoiceValue"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["InvoiceValue"].FormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grd.RootTable.Columns["InvoiceValue"].TotalFormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grd.RootTable.Columns["RealizedAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["RealizedAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["RealizedAmount"].FormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grd.RootTable.Columns["RealizedAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grd.RootTable.Columns["BalAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["BalAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["BalAmount"].FormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grd.RootTable.Columns["BalAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grd.RootTable.Columns["Mtons"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Mtons"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Mtons"].FormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grd.RootTable.Columns["Mtons"].TotalFormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grd.RootTable.Columns["FclQty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["FclQty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["FclQty"].FormatString = "#,##0.##";
			grd.RootTable.Columns["FclQty"].TotalFormatString = "#,##0.##";
			((Control)(object)grd).Refresh();
			ctrlGrdBar1_Load(null, null);
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
			Gridfill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Expected O, but got Unknown
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0)
			{
				return;
			}
			int InvoiceDocumentTypeId = Conversion.ToInt(r.Cells["InvoiceDocumentTypeId"].Value);
			int InvoiceId = Conversion.ToInt(r.Cells["InvoiceId"].Value);
			Conversion.ToInt(r.Cells["ContractId"].Value);
			if (e.Column.Key == "InvoiceNo")
			{
				CommonServices.CommercialInvoiceSlip521(InvoiceId);
			}
			if (e.Column.Key == "CroBooking")
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
			if (e.Column.Key == "LoadingEnd" && Conversion.ToString(r.Cells["LoadingEnd"].Value) != string.Empty)
			{
				ExImForwardingHistory obj = new ExImForwardingHistory(UserAccount)
				{
					txtdatefrom = 
					{
						Value = clsGlobalVariables.ActiveYr.Start_Period
					},
					txtdateto = 
					{
						Value = DateTime.Now
					}
				};
				obj.cmbInvoiceNo.Value = InvoiceId;
				obj.btnShow_Click(null, null);
				obj.Show();
			}
			if (e.Column.Key == "LoadingStart")
			{
				DeliveryOrderHistory deliveryOrderHistory = new DeliveryOrderHistory(UserAccount);
				deliveryOrderHistory.DOType = "Export";
				deliveryOrderHistory.FromDate = clsGlobalVariables.ActiveYr.Start_Period;
				deliveryOrderHistory.ToDate = DateTime.Now;
				deliveryOrderHistory.InvoiceId = InvoiceId;
				deliveryOrderHistory.ShowDialog();
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

	private void Reset()
	{
		try
		{
			((Control)(object)cmbStatus).Text = string.Empty;
			((Control)(object)cmbdestinationport).Text = string.Empty;
			((Control)(object)cmbsupplierCustomer).Text = string.Empty;
			cmbStatus.Focus();
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
			Reset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ExportPendingShipmentsFollowupReport_FormClosed(object sender, FormClosedEventArgs e)
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

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			Statusfill();
			CurrentStatusfill();
			AllcomboBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnshow_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl != null && base.ActiveControl.Name == "ctrlGrdBar1")
			{
				cmbStatus.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ExportPendingShipmentsFollowupReport_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				btnshow_Click(null, null);
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.R)
			{
				btnRefresh_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnNew_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.P)
			{
				btnPrintCurrent_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grd).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				cmbsupplierCustomer.Focus();
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				cmbsupplierCustomer.Focus();
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
			dt.Rows.Add("Ctrl+S", "For Show");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+F5", "For Focus on Supplier Customer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Supplier Customer in Filter");
			ShortCutKeyPopUp shortCutKeyPopUp = new ShortCutKeyPopUp(dt);
			shortCutKeyPopUp.StartPosition = FormStartPosition.CenterScreen;
			shortCutKeyPopUp.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShortCutKeys_Click(object sender, EventArgs e)
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

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grd, grdTable);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrintCurrent_Click(object sender, EventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dt1 == null || dt1.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt1, "552-ExportPendingShippmentFollowUp.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnShipmentForm_Click(object sender, EventArgs e)
	{
		try
		{
			if (UserAccount.RoleName != "Admin")
			{
				if (CommonServices.SetRightsValueInRightsObject("frmshippedConsignment_followup").DoHaveViewRight)
				{
					new frmshippedConsignment_followup(UserAccount).Show();
				}
				else
				{
					MessageBox.Show("You don't have rights to view Of This Form..");
				}
			}
			else
			{
				new frmshippedConsignment_followup(UserAccount).Show();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Delete")
			{
				MessageBox.Show("this Button functionality is revoked");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdEtaDestination_Click(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdEtaDestination.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				int SortNo = Conversion.ToInt(r.Cells["SortNo"].Value);
				Gridfill(SortNo);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnDHLTracking_Click(object sender, EventArgs e)
	{
		try
		{
			new frmDhlTracking(UserAccount).Show();
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
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Expected O, but got Unknown
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Expected O, but got Unknown
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Expected O, but got Unknown
		//IL_0274: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Expected O, but got Unknown
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Expected O, but got Unknown
		//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b5: Expected O, but got Unknown
		//IL_0a61: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a6b: Expected O, but got Unknown
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.ExportReports.ExportPendingShipmentsFollowupReport));
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdEtaDestination_DesignTimeLayout = new GridEXLayout();
		Appearance appearance1 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
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
		Appearance appearance12 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance32 = new Appearance();
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
		Appearance appearance45 = new Appearance();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
		this.btnPrintCurrent = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.btnShortCutKeys = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.BtnShipmentForm = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.btnDHLTracking = new System.Windows.Forms.ToolStripButton();
		this.panel1 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.panel3 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label1 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.grdEtaDestination = new GridEX();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.CmbCurrentStatus = new UltraCombo();
		this.label3 = new System.Windows.Forms.Label();
		this.rdNotRealized = new System.Windows.Forms.RadioButton();
		this.rdRealized = new System.Windows.Forms.RadioButton();
		this.cmbStatus = new UltraCombo();
		this.rdAll = new System.Windows.Forms.RadioButton();
		this.label2 = new System.Windows.Forms.Label();
		this.cmbsupplierCustomer = new UltraCombo();
		this.btnshow = new System.Windows.Forms.Button();
		this.cmbdestinationport = new UltraCombo();
		this.label8 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.RadBlReceivedButPaymentNotShipments = new System.Windows.Forms.RadioButton();
		this.RadForwardingButBLNotShipments = new System.Windows.Forms.RadioButton();
		this.RadForwardingInProcessShipments = new System.Windows.Forms.RadioButton();
		this.RadAllShipments = new System.Windows.Forms.RadioButton();
		this.panel8 = new System.Windows.Forms.Panel();
		this.label9 = new System.Windows.Forms.Label();
		this.toolStrip3.SuspendLayout();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdEtaDestination).BeginInit();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCurrentStatus).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbStatus).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbsupplierCustomer).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbdestinationport).BeginInit();
		this.panel8.SuspendLayout();
		base.SuspendLayout();
		this.toolStrip3.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[11]
		{
			this.toolStripSeparator5, this.btnNew, this.btnRefresh, this.toolStripSeparator4, this.btnPrintCurrent, this.toolStripSeparator3, this.btnShortCutKeys, this.toolStripSeparator2, this.BtnShipmentForm, this.toolStripSeparator1,
			this.btnDHLTracking
		});
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1539, 27);
		this.toolStrip3.TabIndex = 1;
		this.toolStrip3.Text = "toolStrip3";
		this.toolStripSeparator5.Name = "toolStripSeparator5";
		this.toolStripSeparator5.Size = new System.Drawing.Size(6, 27);
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(68, 24);
		this.btnNew.Text = "&Reset";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "&Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.toolStripSeparator4.Name = "toolStripSeparator4";
		this.toolStripSeparator4.Size = new System.Drawing.Size(6, 27);
		this.btnPrintCurrent.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrintCurrent.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrintCurrent.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrintCurrent.Name = "btnPrintCurrent";
		this.btnPrintCurrent.Size = new System.Drawing.Size(92, 24);
		this.btnPrintCurrent.Text = "552-Print";
		this.btnPrintCurrent.ToolTipText = "Print";
		this.btnPrintCurrent.Click += new System.EventHandler(btnPrintCurrent_Click);
		this.toolStripSeparator3.Name = "toolStripSeparator3";
		this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
		this.btnShortCutKeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortCutKeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortCutKeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKeys.Name = "btnShortCutKeys";
		this.btnShortCutKeys.Size = new System.Drawing.Size(126, 24);
		this.btnShortCutKeys.Text = "ShortCut Keys";
		this.btnShortCutKeys.Click += new System.EventHandler(btnShortCutKeys_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
		this.BtnShipmentForm.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.BtnShipmentForm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShipmentForm.Image = (System.Drawing.Image)resources.GetObject("BtnShipmentForm.Image");
		this.BtnShipmentForm.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnShipmentForm.Name = "BtnShipmentForm";
		this.BtnShipmentForm.Size = new System.Drawing.Size(140, 24);
		this.BtnShipmentForm.Text = "Shipment Follow UP";
		this.BtnShipmentForm.Click += new System.EventHandler(BtnShipmentForm_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.btnDHLTracking.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnDHLTracking.Image = (System.Drawing.Image)resources.GetObject("btnDHLTracking.Image");
		this.btnDHLTracking.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDHLTracking.Name = "btnDHLTracking";
		this.btnDHLTracking.Size = new System.Drawing.Size(96, 24);
		this.btnDHLTracking.Text = "DHL Tracking";
		this.btnDHLTracking.Click += new System.EventHandler(btnDHLTracking_Click);
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Controls.Add(this.panel8);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 27);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1539, 534);
		this.panel1.TabIndex = 2;
		this.grd.AllowEdit = (InheritableBoolean)2;
		this.grd.AlternatingColors = true;
		grd_DesignTimeLayout.LayoutString = resources.GetString("grd_DesignTimeLayout.LayoutString");
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.DynamicFiltering = true;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 206);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1539, 328);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 130;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.LinkClicked += new ColumnActionEventHandler(grd_LinkClicked);
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.ctrlGrdBar1);
		this.panel3.Controls.Add(this.label1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 181);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1539, 25);
		this.panel3.TabIndex = 129;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1495, 1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 22);
		this.ctrlGrdBar1.TabIndex = 21;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(6, 2);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(142, 19);
		this.label1.TabIndex = 1;
		this.label1.Text = "Filtered Records";
		this.panel2.BackColor = System.Drawing.Color.White;
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.grdEtaDestination);
		this.panel2.Controls.Add(this.groupBox1);
		this.panel2.Controls.Add(this.RadBlReceivedButPaymentNotShipments);
		this.panel2.Controls.Add(this.RadForwardingButBLNotShipments);
		this.panel2.Controls.Add(this.RadForwardingInProcessShipments);
		this.panel2.Controls.Add(this.RadAllShipments);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 30);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1539, 151);
		this.panel2.TabIndex = 0;
		this.grdEtaDestination.AllowEdit = (InheritableBoolean)2;
		this.grdEtaDestination.AlternatingColors = true;
		grdEtaDestination_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdEtaDestination.DesignTimeLayout = grdEtaDestination_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdEtaDestination).Dock = System.Windows.Forms.DockStyle.Left;
		this.grdEtaDestination.DynamicFiltering = true;
		this.grdEtaDestination.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdEtaDestination).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdEtaDestination.GroupByBoxVisible = false;
		this.grdEtaDestination.GroupTotals = (GroupTotals)3;
		((System.Windows.Forms.Control)(object)this.grdEtaDestination).Location = new System.Drawing.Point(350, 0);
		((System.Windows.Forms.Control)(object)this.grdEtaDestination).Name = "grdEtaDestination";
		((ScrollableBase)this.grdEtaDestination).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdEtaDestination).Size = new System.Drawing.Size(483, 151);
		((System.Windows.Forms.Control)(object)this.grdEtaDestination).TabIndex = 6;
		this.grdEtaDestination.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdEtaDestination).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdEtaDestination).Click += new System.EventHandler(grdEtaDestination_Click);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCurrentStatus);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.rdNotRealized);
		this.groupBox1.Controls.Add(this.rdRealized);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbStatus);
		this.groupBox1.Controls.Add(this.rdAll);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbsupplierCustomer);
		this.groupBox1.Controls.Add(this.btnshow);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbdestinationport);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(0, 0);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(350, 151);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.CmbCurrentStatus.AllowNull = (DefaultableBoolean)1;
		this.CmbCurrentStatus.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCurrentStatus.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCurrentStatus.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCurrentStatus).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbCurrentStatus).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCurrentStatus).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance33).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance33).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCurrentStatus).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCurrentStatus).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance44;
		((SpecialBoxBase)((UltraGridBase)this.CmbCurrentStatus).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance46).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCurrentStatus).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.CmbCurrentStatus).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCurrentStatus).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCurrentStatus).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance47;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCurrentStatus).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.CmbCurrentStatus).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCurrentStatus).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance2).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCurrentStatus).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance2;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance3).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCurrentStatus).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbCurrentStatus).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCurrentStatus).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance4).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance4).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance4).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance4).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCurrentStatus).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCurrentStatus).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbCurrentStatus).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCurrentStatus).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCurrentStatus).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbCurrentStatus).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCurrentStatus).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbCurrentStatus).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCurrentStatus).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCurrentStatus).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCurrentStatus).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbCurrentStatus).Location = new System.Drawing.Point(108, 97);
		((System.Windows.Forms.Control)(object)this.CmbCurrentStatus).Name = "CmbCurrentStatus";
		((System.Windows.Forms.Control)(object)this.CmbCurrentStatus).Size = new System.Drawing.Size(235, 26);
		((System.Windows.Forms.Control)(object)this.CmbCurrentStatus).TabIndex = 3;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(6, 103);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(82, 15);
		this.label3.TabIndex = 131;
		this.label3.Text = "Current Status";
		this.rdNotRealized.AutoSize = true;
		this.rdNotRealized.Checked = true;
		this.rdNotRealized.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdNotRealized.Location = new System.Drawing.Point(204, 128);
		this.rdNotRealized.Name = "rdNotRealized";
		this.rdNotRealized.Size = new System.Drawing.Size(75, 18);
		this.rdNotRealized.TabIndex = 6;
		this.rdNotRealized.TabStop = true;
		this.rdNotRealized.Text = "Pending";
		this.rdNotRealized.UseVisualStyleBackColor = true;
		this.rdRealized.AutoSize = true;
		this.rdRealized.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdRealized.Location = new System.Drawing.Point(120, 128);
		this.rdRealized.Name = "rdRealized";
		this.rdRealized.Size = new System.Drawing.Size(83, 18);
		this.rdRealized.TabIndex = 5;
		this.rdRealized.Text = "Complete";
		this.rdRealized.UseVisualStyleBackColor = true;
		this.cmbStatus.AllowNull = (DefaultableBoolean)1;
		this.cmbStatus.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbStatus.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbStatus.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Appearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbStatus).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbStatus).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbStatus).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbStatus).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance20;
		((SpecialBoxBase)((UltraGridBase)this.cmbStatus).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbStatus).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.cmbStatus).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbStatus).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance26).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.cmbStatus).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbStatus).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbStatus).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbStatus).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.cmbStatus).Location = new System.Drawing.Point(108, 70);
		((System.Windows.Forms.Control)(object)this.cmbStatus).Name = "cmbStatus";
		((System.Windows.Forms.Control)(object)this.cmbStatus).Size = new System.Drawing.Size(235, 26);
		((System.Windows.Forms.Control)(object)this.cmbStatus).TabIndex = 2;
		this.rdAll.AutoSize = true;
		this.rdAll.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdAll.Location = new System.Drawing.Point(78, 128);
		this.rdAll.Name = "rdAll";
		this.rdAll.Size = new System.Drawing.Size(40, 18);
		this.rdAll.TabIndex = 4;
		this.rdAll.Text = "All";
		this.rdAll.UseVisualStyleBackColor = true;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(6, 76);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(100, 15);
		this.label2.TabIndex = 129;
		this.label2.Text = "Document Status";
		this.cmbsupplierCustomer.AllowNull = (DefaultableBoolean)1;
		this.cmbsupplierCustomer.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbsupplierCustomer.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbsupplierCustomer.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbsupplierCustomer).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.cmbsupplierCustomer).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbsupplierCustomer).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance23).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance23).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbsupplierCustomer).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbsupplierCustomer).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance31;
		((SpecialBoxBase)((UltraGridBase)this.cmbsupplierCustomer).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbsupplierCustomer).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.cmbsupplierCustomer).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbsupplierCustomer).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbsupplierCustomer).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance7;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbsupplierCustomer).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.cmbsupplierCustomer).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbsupplierCustomer).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbsupplierCustomer).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbsupplierCustomer).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbsupplierCustomer).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbsupplierCustomer).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbsupplierCustomer).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbsupplierCustomer).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.cmbsupplierCustomer).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbsupplierCustomer).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbsupplierCustomer).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbsupplierCustomer).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbsupplierCustomer).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmbsupplierCustomer).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbsupplierCustomer).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbsupplierCustomer).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbsupplierCustomer).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.cmbsupplierCustomer).Location = new System.Drawing.Point(108, 16);
		((System.Windows.Forms.Control)(object)this.cmbsupplierCustomer).Name = "cmbsupplierCustomer";
		((System.Windows.Forms.Control)(object)this.cmbsupplierCustomer).Size = new System.Drawing.Size(235, 26);
		((System.Windows.Forms.Control)(object)this.cmbsupplierCustomer).TabIndex = 0;
		this.btnshow.BackColor = System.Drawing.Color.Teal;
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(284, 124);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(59, 26);
		this.btnshow.TabIndex = 7;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.btnshow.Leave += new System.EventHandler(btnshow_Leave);
		this.cmbdestinationport.AllowNull = (DefaultableBoolean)1;
		this.cmbdestinationport.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbdestinationport.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbdestinationport.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbdestinationport).DisplayLayout.Appearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.cmbdestinationport).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbdestinationport).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbdestinationport).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbdestinationport).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance35;
		((SpecialBoxBase)((UltraGridBase)this.cmbdestinationport).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance36).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance36).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbdestinationport).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.cmbdestinationport).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbdestinationport).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbdestinationport).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance37;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance38).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbdestinationport).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.cmbdestinationport).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbdestinationport).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbdestinationport).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance39;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance40).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbdestinationport).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.cmbdestinationport).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbdestinationport).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance41).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance41).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance41).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbdestinationport).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbdestinationport).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbdestinationport).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbdestinationport).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbdestinationport).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.cmbdestinationport).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbdestinationport).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.cmbdestinationport).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbdestinationport).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbdestinationport).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbdestinationport).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.cmbdestinationport).Location = new System.Drawing.Point(108, 43);
		((System.Windows.Forms.Control)(object)this.cmbdestinationport).Name = "cmbdestinationport";
		((System.Windows.Forms.Control)(object)this.cmbdestinationport).Size = new System.Drawing.Size(235, 26);
		((System.Windows.Forms.Control)(object)this.cmbdestinationport).TabIndex = 1;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(6, 22);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(58, 15);
		this.label8.TabIndex = 125;
		this.label8.Text = "Customer";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(6, 49);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(93, 15);
		this.label7.TabIndex = 127;
		this.label7.Text = "Destination Port";
		this.RadBlReceivedButPaymentNotShipments.AutoSize = true;
		this.RadBlReceivedButPaymentNotShipments.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadBlReceivedButPaymentNotShipments.Location = new System.Drawing.Point(839, 69);
		this.RadBlReceivedButPaymentNotShipments.Name = "RadBlReceivedButPaymentNotShipments";
		this.RadBlReceivedButPaymentNotShipments.Size = new System.Drawing.Size(273, 18);
		this.RadBlReceivedButPaymentNotShipments.TabIndex = 5;
		this.RadBlReceivedButPaymentNotShipments.Text = "B/L Received But Payment Not Received";
		this.RadBlReceivedButPaymentNotShipments.UseVisualStyleBackColor = true;
		this.RadForwardingButBLNotShipments.AutoSize = true;
		this.RadForwardingButBLNotShipments.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadForwardingButBLNotShipments.Location = new System.Drawing.Point(839, 46);
		this.RadForwardingButBLNotShipments.Name = "RadForwardingButBLNotShipments";
		this.RadForwardingButBLNotShipments.Size = new System.Drawing.Size(230, 18);
		this.RadForwardingButBLNotShipments.TabIndex = 4;
		this.RadForwardingButBLNotShipments.Text = "Forwarding But B/L Not Received";
		this.RadForwardingButBLNotShipments.UseVisualStyleBackColor = true;
		this.RadForwardingInProcessShipments.AutoSize = true;
		this.RadForwardingInProcessShipments.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadForwardingInProcessShipments.Location = new System.Drawing.Point(839, 24);
		this.RadForwardingInProcessShipments.Name = "RadForwardingInProcessShipments";
		this.RadForwardingInProcessShipments.Size = new System.Drawing.Size(218, 18);
		this.RadForwardingInProcessShipments.TabIndex = 3;
		this.RadForwardingInProcessShipments.Text = "Forwarding Dispatch In Process";
		this.RadForwardingInProcessShipments.UseVisualStyleBackColor = true;
		this.RadAllShipments.AutoSize = true;
		this.RadAllShipments.Checked = true;
		this.RadAllShipments.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadAllShipments.Location = new System.Drawing.Point(839, 3);
		this.RadAllShipments.Name = "RadAllShipments";
		this.RadAllShipments.Size = new System.Drawing.Size(163, 18);
		this.RadAllShipments.TabIndex = 2;
		this.RadAllShipments.TabStop = true;
		this.RadAllShipments.Text = "All Pending Shipments";
		this.RadAllShipments.UseVisualStyleBackColor = true;
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.label9);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 0);
		this.panel8.Margin = new System.Windows.Forms.Padding(2);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1539, 30);
		this.panel8.TabIndex = 123;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.ForeColor = System.Drawing.Color.White;
		this.label9.Location = new System.Drawing.Point(7, 5);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(363, 19);
		this.label9.TabIndex = 1;
		this.label9.Text = "Export Pending Shipments Followup Report";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1539, 561);
		base.Controls.Add(this.panel1);
		base.Controls.Add(this.toolStrip3);
		base.KeyPreview = true;
		base.Name = "ExportPendingShipmentsFollowupReport";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(ExportPendingShipmentsFollowupReport_FormClosed);
		base.Load += new System.EventHandler(ExportPendingShipmentsFollowupReport_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(ExportPendingShipmentsFollowupReport_KeyDown);
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		this.panel1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdEtaDestination).EndInit();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCurrentStatus).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbStatus).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbsupplierCustomer).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbdestinationport).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
