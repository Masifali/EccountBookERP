// using Architecture.Reports;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Export;
using Architecture.BLL.Inventory;
using Architecture.BLL.Reports.Export;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Common;
using Architecture.WinApp.Inventory_Reports;
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
namespace Architecture.WinApp.Audit_Dashboard
{

public class frmAuditByWeightReport : Form
{
	private bool ShiftTabPressed;

	public bool FormInitialized;

	private DropDownBind DDL = new DropDownBind();

	public UserAccount UserAccount = new UserAccount();

	private DataTable dtHistory = new DataTable();

	private IContainer components;

	private Panel panel1;

	private Panel panel3;

	private Panel panel2;

	private ToolStrip tsItemType;

	private ToolStripButton btnnew;

	private Button btnShow;

	private Panel panel5;

	private Label label3;

	private Label label4;

	private DateTimePicker txtdateto;

	private Label label5;

	private Label label1;

	private DateTimePicker txtdatefrom;

	private Label label51;

	private CtrlGrdBar ctrlGrdBar1;

	private ToolStripButton btnPrint;

	private Panel panel4;

	internal GridEX grd;

	internal UltraCombo cmbperemeter;

	private Label label11;

	private ToolStripButton btnShortCutKey;

	private Panel panel6;

	private Label label2;

	private Panel panel7;

	private Panel panel8;

	private Label label6;

	private CtrlGrdBar ctrlGrdBar2;

	internal GridEX grdDetail;

	private Label label8;

	private Label label7;

	internal UltraCombo CmbInvoiceNo;

	internal UltraCombo CmbContractNo;

	private RadioButton rdAll;

	private RadioButton rdCompleted;

	private RadioButton RdInProcess;

	private ToolStripButton btnPrint618_01;

	public frmAuditByWeightReport(UserAccount pUserAccount)
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
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(980, 600);
			ParameterFill();
			ComboFill();
			if (Conversion.ToInt(cmbperemeter.Value) == 0)
			{
				((UltraGridBase)cmbperemeter).Rows[1].Activate();
			}
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
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtcombo = InvDeliveryOrder.GetDataForDropDownFromDeliveryOrder(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtcombo.Rows.Count <= 0)
			{
				return;
			}
			DataTable dtInvoice = new DataTable();
			dtInvoice.Columns.Add("Id", typeof(int));
			dtInvoice.Columns.Add("InvoiceNo", typeof(string));
			DataTable dtOrder = new DataTable();
			dtOrder.Columns.Add("Id", typeof(int));
			dtOrder.Columns.Add("ContractNo", typeof(string));
			int InvoiceId = Conversion.ToInt(CmbInvoiceNo.Value);
			int ContractId = Conversion.ToInt(CmbContractNo.Value);
			for (int i = 0; i < dtcombo.Rows.Count; i++)
			{
				if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "InvoiceNo")
				{
					dtInvoice.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
				}
				if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "ContractNo")
				{
					dtOrder.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
				}
			}
			if (dtInvoice.Rows.Count > 0)
			{
				DDL.BindDDL(dtInvoice, CmbInvoiceNo, "Id", "InvoiceNo", "Invoice No", ZeroIndex: true);
				((UltraGridBase)CmbInvoiceNo).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (dtOrder.Rows.Count > 0)
			{
				DDL.BindDDL(dtOrder, CmbContractNo, "Id", "ContractNo", "Contract No", ZeroIndex: true);
				((UltraGridBase)CmbContractNo).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (InvoiceId > 0)
			{
				if (dtInvoice.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == InvoiceId))
				{
					CmbInvoiceNo.Value = InvoiceId;
				}
				else
				{
					((Control)(object)CmbInvoiceNo).Text = string.Empty;
				}
			}
			if (ContractId > 0)
			{
				if (dtOrder.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ContractId))
				{
					CmbContractNo.Value = ContractId;
				}
				else
				{
					((Control)(object)CmbContractNo).Text = string.Empty;
				}
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
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		try
		{
			dtHistory.Rows.Clear();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.FromDate = Conversion.ToDateTime((object)txtdatefrom.Value);
			obj.ToDate = Conversion.ToDateTime((object)txtdateto.Value);
			obj.EximInvoiceId = Conversion.ToInt(CmbInvoiceNo.Value);
			obj.ExImLcOrderId = Conversion.ToInt(CmbContractNo.Value);
			obj.Status = (rdAll.Checked ? null : (RdInProcess.Checked ? "In Process" : "Completed"));
			dtHistory = ExImInvoice.ExportSalesAuditByWeight(obj);
			DataTable dataTable = dtHistory;
			if (dataTable != null && dataTable.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("SupplierCustomerId", typeof(int));
				dt.Columns.Add("CustomerName", typeof(string));
				dt.Columns.Add("ContractNo", typeof(string));
				dt.Columns.Add("InvoiceId", typeof(int));
				dt.Columns.Add("InvoiceNo", typeof(string));
				dt.Columns.Add("InvoiceDate", typeof(DateTime));
				dt.Columns.Add("Containers", typeof(int));
				dt.Columns.Add("InvoiceGrossWeight", typeof(double));
				dt.Columns.Add("DoGrossWeight", typeof(double));
				dt.Columns.Add("WbWeight", typeof(double));
				dt.Columns.Add("ExcessWeight", typeof(double));
				dt.Columns.Add("LessExcess", typeof(string));
				dt.Columns.Add("AvgLessExcess", typeof(double));
				dt.Columns.Add("LessExcessAmount", typeof(double));
				dt.Columns.Add("InvoiceNetWeight", typeof(double));
				dt.Columns.Add("DoNetWeight", typeof(double));
				dt.Columns.Add("DiffWeight", typeof(double));
				dt.Columns.Add("GpCount", typeof(int));
				dt.Columns.Add("InvoiceStatus", typeof(string));
				int i;
				for (i = 0; i < dtHistory.Rows.Count; i++)
				{
					if (!dt.AsEnumerable().Any((DataRow row) => row.Field<string>("InvoiceNo") == Conversion.ToString(dtHistory.Rows[i]["InvoiceNo"])))
					{
						int gpCount = dtHistory.AsEnumerable().Count((DataRow row) => row.Field<string>("InvoiceNo") == Conversion.ToString(dtHistory.Rows[i]["InvoiceNo"]));
						dt.Rows.Add(dtHistory.Rows[i]["SupplierCustomerId"], dtHistory.Rows[i]["CustomerName"], dtHistory.Rows[i]["LcOrderNo"], dtHistory.Rows[i]["InvoiceId"], dtHistory.Rows[i]["InvoiceNo"], Conversion.ToDateTime(dtHistory.Rows[i]["InvoiceDate"]).ToShortDateString(), dtHistory.Rows[i]["Containers"], dtHistory.Rows[i]["InvoiceGwTotal"], dtHistory.Rows[i]["DoGwTotal"], dtHistory.Rows[i]["WbGwTotal"], dtHistory.Rows[i]["ExcessWtTotal"], dtHistory.Rows[i]["LessExcess"], dtHistory.Rows[i]["AvgLessExcess"], dtHistory.Rows[i]["LessExcessAmount"], dtHistory.Rows[i]["InvoiceNwTotal"], dtHistory.Rows[i]["DoNwTotal"], dtHistory.Rows[i]["InvNwDoNwDiff"], gpCount, dtHistory.Rows[i]["InvoiceStatus"]);
					}
				}
				grd.DataSource = dt;
				grd.RetrieveStructure();
				GridSetting();
				GridColumnsColourChange();
			}
			else
			{
				grd.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSetting()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grd.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grd.RootTable.Columns)
			{
				item.HeaderAlignment = (TextAlignment)2;
			}
			grd.RootTable.Columns["SupplierCustomerId"].Visible = false;
			grd.RootTable.Columns["InvoiceId"].Visible = false;
			grd.RootTable.Columns["DoGrossWeight"].ColumnType = (ColumnType)5;
			grd.RootTable.Columns["InvoiceNo"].ColumnType = (ColumnType)5;
			grd.RootTable.Columns["InvoiceDate"].FormatString = "dd-MMM-yy";
			grd.RootTable.Columns["InvoiceDate"].Width = 73;
			grd.RootTable.Columns["CustomerName"].Width = 170;
			grd.RootTable.Columns["ContractNo"].Width = 130;
			grd.RootTable.Columns["Containers"].Width = 50;
			grd.RootTable.Columns["GpCount"].Width = 75;
			grd.RootTable.Columns["InvoiceGrossWeight"].Width = 90;
			grd.RootTable.Columns["DoGrossWeight"].Width = 90;
			grd.RootTable.Columns["WbWeight"].Width = 90;
			grd.RootTable.Columns["InvoiceNetWeight"].Width = 90;
			grd.RootTable.Columns["ExcessWeight"].Width = 90;
			grd.RootTable.Columns["LessExcessAmount"].Width = 90;
			grd.RootTable.Columns["LessExcess"].Width = 60;
			grd.RootTable.Columns["AvgLessExcess"].Width = 80;
			grd.RootTable.Columns["DiffWeight"].Width = 90;
			grd.RootTable.Columns["DoNetWeight"].Width = 90;
			grd.RootTable.Columns["AvgLessExcess"].Caption = "Avg Ls/Exc Per FCL";
			grd.RootTable.Columns["Containers"].Caption = "FCL";
			grd.RootTable.Columns["ContractNo"].Caption = "Contract Number";
			grd.RootTable.Columns["WbWeight"].Caption = "Weigh Bridge Weight";
			grd.RootTable.Columns["ExcessWeight"].Caption = "Less / Excess DO - WB";
			grd.RootTable.Columns["InvoiceStatus"].Caption = "Dispatch Status";
			grd.RootTable.Columns["DiffWeight"].Caption = "Less / Excess Invoice - DO";
			grd.RootTable.Columns["GpCount"].Caption = "No Of GatePass";
			grd.RootTable.Columns["InvoiceGrossWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["InvoiceGrossWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["InvoiceGrossWeight"].FormatString = "#,##0;(0,0);0";
			grd.RootTable.Columns["InvoiceGrossWeight"].TotalFormatString = "#,##0;(0,0);0";
			grd.RootTable.Columns["DoGrossWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["DoGrossWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["DoGrossWeight"].FormatString = "#,##0;(0,0);0";
			grd.RootTable.Columns["DoGrossWeight"].TotalFormatString = "#,##0;(0,0);0";
			grd.RootTable.Columns["WbWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["WbWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["WbWeight"].FormatString = "#,##0;(0,0);0";
			grd.RootTable.Columns["WbWeight"].TotalFormatString = "#,##0;(0,0);0";
			grd.RootTable.Columns["InvoiceNetWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["InvoiceNetWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["InvoiceNetWeight"].FormatString = "#,##0;(0,0);0";
			grd.RootTable.Columns["InvoiceNetWeight"].TotalFormatString = "#,##0;(0,0);0";
			grd.RootTable.Columns["DoNetWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["DoNetWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["DoNetWeight"].FormatString = "#,##0;(0,0);0";
			grd.RootTable.Columns["DoNetWeight"].TotalFormatString = "#,##0;(0,0);0";
			grd.RootTable.Columns["DiffWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["DiffWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["DiffWeight"].FormatString = "#,##0;(0,0);0";
			grd.RootTable.Columns["DiffWeight"].TotalFormatString = "#,##0;(0,0);0";
			grd.RootTable.Columns["Containers"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Containers"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Containers"].FormatString = "#,##0";
			grd.RootTable.Columns["Containers"].TotalFormatString = "#,##0";
			grd.RootTable.Columns["GpCount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["GpCount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["GpCount"].FormatString = "#,##0";
			grd.RootTable.Columns["GpCount"].TotalFormatString = "#,##0";
			grd.RootTable.Columns["AvgLessExcess"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["AvgLessExcess"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["AvgLessExcess"].FormatString = "#,##0";
			grd.RootTable.Columns["AvgLessExcess"].TotalFormatString = "#,##0";
			grd.RootTable.Columns["LessExcessAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["LessExcessAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["LessExcessAmount"].FormatString = "#,##0";
			grd.RootTable.Columns["LessExcessAmount"].TotalFormatString = "#,##0";
			grd.RootTable.Columns["ExcessWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ExcessWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ExcessWeight"].FormatString = "#,##0.###;(0,0.###);0";
			grd.RootTable.Columns["ExcessWeight"].TotalFormatString = "#,##0.###;(0,0.###);0";
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridColumnsColourChange()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected O, but got Unknown
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Expected O, but got Unknown
		try
		{
			GridEXColumn column = grd.RootTable.Columns["InvoiceStatus"];
			GridEXFormatCondition fc = new GridEXFormatCondition(column, (ConditionOperator)0, (object)"Completed");
			fc.TargetColumn = column;
			fc.FormatStyle.FontBold = (TriState)2;
			fc.FormatStyle.ForeColor = Color.Green;
			grd.RootTable.FormatConditions.Add(fc);
			GridEXColumn column2 = grd.RootTable.Columns["InvoiceStatus"];
			GridEXFormatCondition fc2 = new GridEXFormatCondition(column2, (ConditionOperator)0, (object)"In Process");
			fc2.TargetColumn = column2;
			fc2.FormatStyle.FontBold = (TriState)2;
			fc2.FormatStyle.ForeColor = Color.Red;
			grd.RootTable.FormatConditions.Add(fc2);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Expected O, but got Unknown
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0 && e.Column.Key == "Print")
			{
				DataTable dt = new DataTable();
				dt = ExportPdfReport.ExpRptSalesContractExport_501(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					Id = Conversion.ToInt(r.Cells["Id"].Value),
					ApprovedFilter = Conversion.ToString((object)"All")
				});
				if (dt == null || dt.Rows.Count == 0)
				{
					MessageBox.Show("Not Record Found For Display");
					return;
				}
				Reporting val = new Reporting();
				val.RptPerameter("CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dt, "501-ExpRptSalesContractExport.rpt");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Expected O, but got Unknown
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0)
			{
				return;
			}
			int InvoiceId = Conversion.ToInt(r.Cells["InvoiceId"].Value);
			Conversion.ToInt(cmbperemeter.Value);
			Conversion.ToDateTime((object)txtdatefrom.Value);
			DateTime ToDate = Conversion.ToDateTime((object)txtdateto.Value);
			if (e.Column.Key == "InvoiceNo")
			{
				DataTable dt = ExImInvoice.ExportSalesAuditByWeight(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					EximInvoiceId = InvoiceId
				});
				if (dt == null || dt.Rows.Count == 0)
				{
					MessageBox.Show("Not Record Found For Display");
				}
				else
				{
					Reporting val = new Reporting();
					val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
					val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
					val.ShowReportWithDataTable(dt, "618-DoWeightWbWeightDiff.rpt");
				}
			}
			if (e.Column.Key == "DoGrossWeight" && Conversion.ToDouble(r.Cells["DoGrossWeight"].Value) > 0.0)
			{
				DeliveryOrderHistory deliveryOrderHistory = new DeliveryOrderHistory(UserAccount);
				deliveryOrderHistory.DateTypeId = 5;
				deliveryOrderHistory.DOType = "Export";
				deliveryOrderHistory.FromDate = clsGlobalVariables.ActiveYr.Start_Period;
				deliveryOrderHistory.ToDate = ToDate;
				deliveryOrderHistory.InvoiceId = InvoiceId;
				deliveryOrderHistory.RadWithSecondWeight.Checked = true;
				deliveryOrderHistory.ShowDialog();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grd.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("GpDate", typeof(DateTime));
				dt.Columns.Add("GpId", typeof(int));
				dt.Columns.Add("GpNo", typeof(int));
				dt.Columns.Add("DOId", typeof(int));
				dt.Columns.Add("DONo", typeof(int));
				dt.Columns.Add("Container1");
				dt.Columns.Add("Container2");
				dt.Columns.Add("DoNetWeight", typeof(double));
				dt.Columns.Add("DoPackingWeight", typeof(double));
				dt.Columns.Add("DoGrossWeight", typeof(double));
				dt.Columns.Add("DoOtherWeight", typeof(double));
				dt.Columns.Add("WbWeight", typeof(double));
				dt.Columns.Add("Less/Excess", typeof(double));
				dt.Columns.Add("LessExcess", typeof(string));
				DataTable resultRows = (from row in dtHistory.AsEnumerable()
					where row.Field<string>("InvoiceNo") == Conversion.ToString(item.Cells["InvoiceNo"].Value)
					select row).CopyToDataTable();
				for (int i = 0; i < resultRows.Rows.Count; i++)
				{
					dt.Rows.Add(Conversion.ToDateTime(resultRows.Rows[i]["GpDate"]).ToShortDateString(), resultRows.Rows[i]["GpId"], resultRows.Rows[i]["GpSrNo"], resultRows.Rows[i]["DOId"], resultRows.Rows[i]["DONo"], resultRows.Rows[i]["Container1"], resultRows.Rows[i]["Container2"], resultRows.Rows[i]["doNetWeight"], resultRows.Rows[i]["doPackingWeight"], resultRows.Rows[i]["doGrossWeight"], resultRows.Rows[i]["doOtherWeight"], resultRows.Rows[i]["WbWeight"], resultRows.Rows[i]["ExcessWt"], resultRows.Rows[i]["LessExcessDoWise"]);
				}
				grdDetail.DataSource = dt;
				grdDetail.RetrieveStructure();
				GridDetailSetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridDetailSetting()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdDetail.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grdDetail.RootTable.Columns)
			{
				item.HeaderAlignment = (TextAlignment)2;
			}
			grdDetail.RootTable.Columns["GpId"].Visible = false;
			grdDetail.RootTable.Columns["DOId"].Visible = false;
			grdDetail.RootTable.Columns["DONo"].ColumnType = (ColumnType)5;
			grdDetail.RootTable.Columns["GpNo"].ColumnType = (ColumnType)5;
			grdDetail.RootTable.Columns["GpDate"].FormatString = "dd-MMM-yy";
			grdDetail.RootTable.Columns["GpDate"].Width = 73;
			grdDetail.RootTable.Columns["GpNo"].Width = 40;
			grdDetail.RootTable.Columns["DONo"].Width = 40;
			grdDetail.RootTable.Columns["Container1"].Width = 115;
			grdDetail.RootTable.Columns["Container2"].Width = 115;
			grdDetail.RootTable.Columns["DoNetWeight"].Width = 90;
			grdDetail.RootTable.Columns["DoPackingWeight"].Width = 90;
			grdDetail.RootTable.Columns["DoOtherWeight"].Width = 90;
			grdDetail.RootTable.Columns["DoGrossWeight"].Width = 90;
			grdDetail.RootTable.Columns["WbWeight"].Width = 90;
			grdDetail.RootTable.Columns["Less/Excess"].Width = 90;
			grdDetail.RootTable.Columns["Less/Excess"].Caption = "WB-DO Difference";
			grdDetail.RootTable.Columns["DONo"].Caption = "DO No";
			grdDetail.RootTable.Columns["DoNetWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["DoNetWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["DoNetWeight"].FormatString = "#,##0.###;(0,0.###);0";
			grdDetail.RootTable.Columns["DoNetWeight"].TotalFormatString = "#,##0.###;(0,0.###);0";
			grdDetail.RootTable.Columns["DoPackingWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["DoPackingWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["DoPackingWeight"].FormatString = "#,##0.###;(0,0.###);0";
			grdDetail.RootTable.Columns["DoPackingWeight"].TotalFormatString = "#,##0.###;(0,0.###);0";
			grdDetail.RootTable.Columns["DoGrossWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["DoGrossWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["DoGrossWeight"].FormatString = "#,##0.###;(0,0.###);0";
			grdDetail.RootTable.Columns["DoGrossWeight"].TotalFormatString = "#,##0.###;(0,0.###);0";
			grdDetail.RootTable.Columns["DoOtherWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["DoOtherWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["DoOtherWeight"].FormatString = "#,##0.###;(0,0.###);0";
			grdDetail.RootTable.Columns["DoOtherWeight"].TotalFormatString = "#,##0.###;(0,0.###);0";
			grdDetail.RootTable.Columns["WbWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["WbWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["WbWeight"].FormatString = "#,##0.###;(0,0.###);0";
			grdDetail.RootTable.Columns["WbWeight"].TotalFormatString = "#,##0.###;(0,0.###);0";
			grdDetail.RootTable.Columns["Less/Excess"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["Less/Excess"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["Less/Excess"].FormatString = "#,##0.###;(0,0.###);0";
			grdDetail.RootTable.Columns["Less/Excess"].TotalFormatString = "#,##0.###;(0,0.###);0";
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdDetail_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdDetail.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
			{
				if (e.Column.Key == "GpNo")
				{
					CommonServices.OutwardGatePassWithWb(Conversion.ToInt(item.Cells["GpId"].Value));
				}
				if (e.Column.Key == "DONo")
				{
					CommonServices.ExportDeliveryOrderSlip396(Conversion.ToInt(item.Cells["DOId"].Value), 84);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void frmExportInvoiceSummaryByMonth_FormClosing(object sender, FormClosingEventArgs e)
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

	public void Reset()
	{
		try
		{
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
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnnew_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				cmbperemeter.Focus();
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				btnShow_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.P)
			{
				btnPrint_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grd).Focus();
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
			dt.Rows.Add("Ctrl+F5", "For Focus on Date Type");
			dt.Rows.Add("Ctrl+S", "For Showing Data");
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Grid");
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
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grd, dtHistory);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
	{
		try
		{
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grdDetail, dtHistory);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
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

	private void btnShow_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl?.Name == "")
			{
				cmbperemeter.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control)
			{
				_ = e.KeyCode;
				_ = 32;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrint_Click(object sender, EventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtHistory == null || dtHistory.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dtHistory, "618-DoWeightWbWeightDiff.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrint618_01_Click(object sender, EventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtHistory == null || dtHistory.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dtHistory, "618_01_DoWeightWbWeightDiff.rpt");
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
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Expected O, but got Unknown
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Expected O, but got Unknown
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Expected O, but got Unknown
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Expected O, but got Unknown
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Expected O, but got Unknown
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Expected O, but got Unknown
		//IL_05a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b0: Expected O, but got Unknown
		//IL_08e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_08eb: Expected O, but got Unknown
		//IL_090f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0919: Expected O, but got Unknown
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Audit_Dashboard.frmAuditByWeightReport));
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance1 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
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
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel7 = new System.Windows.Forms.Panel();
		this.grdDetail = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.label6 = new System.Windows.Forms.Label();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel6 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label2 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel3 = new System.Windows.Forms.Panel();
		this.rdAll = new System.Windows.Forms.RadioButton();
		this.rdCompleted = new System.Windows.Forms.RadioButton();
		this.RdInProcess = new System.Windows.Forms.RadioButton();
		this.label8 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.CmbInvoiceNo = new UltraCombo();
		this.CmbContractNo = new UltraCombo();
		this.cmbperemeter = new UltraCombo();
		this.label11 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.txtdateto = new System.Windows.Forms.DateTimePicker();
		this.label5 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.txtdatefrom = new System.Windows.Forms.DateTimePicker();
		this.label51 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.label3 = new System.Windows.Forms.Label();
		this.btnShow = new System.Windows.Forms.Button();
		this.panel2 = new System.Windows.Forms.Panel();
		this.tsItemType = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.btnShortCutKey = new System.Windows.Forms.ToolStripButton();
		this.btnPrint618_01 = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel8.SuspendLayout();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel4.SuspendLayout();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbInvoiceNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbContractNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbperemeter).BeginInit();
		this.panel5.SuspendLayout();
		this.panel2.SuspendLayout();
		this.tsItemType.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.panel7);
		this.panel1.Controls.Add(this.panel6);
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(969, 648);
		this.panel1.TabIndex = 0;
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.panel7.Controls.Add(this.panel8);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(0, 471);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(969, 177);
		this.panel7.TabIndex = 4;
		this.grdDetail.AllowEdit = (InheritableBoolean)2;
		grdDetail_DesignTimeLayout.LayoutString = resources.GetString("grdDetail_DesignTimeLayout.LayoutString");
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdDetail.DynamicFiltering = true;
		this.grdDetail.FilterMode = (FilterMode)2;
		this.grdDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupByBoxVisible = false;
		this.grdDetail.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(0, 24);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(969, 153);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.grdDetail).TabStop = false;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.grdDetail.LinkClicked += new ColumnActionEventHandler(grdDetail_LinkClicked);
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.label6);
		this.panel8.Controls.Add(this.ctrlGrdBar2);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 0);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(969, 24);
		this.panel8.TabIndex = 3;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.ForeColor = System.Drawing.Color.White;
		this.label6.Location = new System.Drawing.Point(8, 2);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(355, 19);
		this.label6.TabIndex = 1;
		this.label6.Text = "Detail Information Of Above Selected Row";
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(928, 1);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdDetail;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar2.TabIndex = 0;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 173);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(969, 298);
		this.panel6.TabIndex = 3;
		this.grd.AllowEdit = (InheritableBoolean)2;
		grd_DesignTimeLayout.LayoutString = resources.GetString("grd_DesignTimeLayout.LayoutString");
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.DynamicFiltering = true;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(969, 298);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.grd).TabStop = false;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		this.grd.SelectionChanged += new System.EventHandler(grd_SelectionChanged);
		this.grd.LinkClicked += new ColumnActionEventHandler(grd_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.label2);
		this.panel4.Controls.Add(this.ctrlGrdBar1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 149);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(969, 24);
		this.panel4.TabIndex = 2;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(8, 2);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(150, 19);
		this.label2.TabIndex = 1;
		this.label2.Text = "Main Information";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(928, 1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 0;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add(this.rdAll);
		this.panel3.Controls.Add(this.rdCompleted);
		this.panel3.Controls.Add(this.RdInProcess);
		this.panel3.Controls.Add(this.label8);
		this.panel3.Controls.Add(this.label7);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbInvoiceNo);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbContractNo);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbperemeter);
		this.panel3.Controls.Add(this.label11);
		this.panel3.Controls.Add(this.label4);
		this.panel3.Controls.Add(this.txtdateto);
		this.panel3.Controls.Add(this.label5);
		this.panel3.Controls.Add(this.label1);
		this.panel3.Controls.Add(this.txtdatefrom);
		this.panel3.Controls.Add(this.label51);
		this.panel3.Controls.Add(this.panel5);
		this.panel3.Controls.Add(this.btnShow);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 27);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(969, 122);
		this.panel3.TabIndex = 0;
		this.rdAll.AutoSize = true;
		this.rdAll.Checked = true;
		this.rdAll.Location = new System.Drawing.Point(519, 92);
		this.rdAll.Name = "rdAll";
		this.rdAll.Size = new System.Drawing.Size(36, 17);
		this.rdAll.TabIndex = 590;
		this.rdAll.TabStop = true;
		this.rdAll.Text = "All";
		this.rdAll.UseVisualStyleBackColor = true;
		this.rdCompleted.AutoSize = true;
		this.rdCompleted.Location = new System.Drawing.Point(519, 66);
		this.rdCompleted.Name = "rdCompleted";
		this.rdCompleted.Size = new System.Drawing.Size(75, 17);
		this.rdCompleted.TabIndex = 590;
		this.rdCompleted.Text = "Completed";
		this.rdCompleted.UseVisualStyleBackColor = true;
		this.RdInProcess.AutoSize = true;
		this.RdInProcess.Location = new System.Drawing.Point(519, 39);
		this.RdInProcess.Name = "RdInProcess";
		this.RdInProcess.Size = new System.Drawing.Size(75, 17);
		this.RdInProcess.TabIndex = 589;
		this.RdInProcess.Text = "In Process";
		this.RdInProcess.UseVisualStyleBackColor = true;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(218, 70);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(62, 15);
		this.label8.TabIndex = 588;
		this.label8.Text = "InvoiceNo";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(218, 40);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(71, 15);
		this.label7.TabIndex = 588;
		this.label7.Text = "Contract No";
		this.CmbInvoiceNo.AllowNull = (DefaultableBoolean)1;
		this.CmbInvoiceNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbInvoiceNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbInvoiceNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance33;
		((SpecialBoxBase)((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance35;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance2).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance2;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance3).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance4).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance4).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance4).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance4).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance28).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbInvoiceNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNo).Location = new System.Drawing.Point(298, 64);
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNo).Name = "CmbInvoiceNo";
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNo).Size = new System.Drawing.Size(211, 26);
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNo).TabIndex = 4;
		this.CmbContractNo.AllowNull = (DefaultableBoolean)1;
		this.CmbContractNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbContractNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbContractNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance23).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance23).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbContractNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance31;
		((SpecialBoxBase)((UltraGridBase)this.CmbContractNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance18).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance18).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance22).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance24).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance25).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbContractNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbContractNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbContractNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbContractNo).Location = new System.Drawing.Point(298, 34);
		((System.Windows.Forms.Control)(object)this.CmbContractNo).Name = "CmbContractNo";
		((System.Windows.Forms.Control)(object)this.CmbContractNo).Size = new System.Drawing.Size(211, 26);
		((System.Windows.Forms.Control)(object)this.CmbContractNo).TabIndex = 3;
		this.cmbperemeter.AllowNull = (DefaultableBoolean)1;
		this.cmbperemeter.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbperemeter.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbperemeter.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbperemeter.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Location = new System.Drawing.Point(89, 34);
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Name = "cmbperemeter";
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Size = new System.Drawing.Size(123, 26);
		((System.Windows.Forms.Control)(object)this.cmbperemeter).TabIndex = 0;
		this.cmbperemeter.ValueChanged += new System.EventHandler(cmbperemeter_ValueChanged);
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(11, 40);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(60, 15);
		this.label11.TabIndex = 586;
		this.label11.Text = "Date Type";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.Red;
		this.label4.Location = new System.Drawing.Point(75, 95);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(14, 17);
		this.label4.TabIndex = 2;
		this.label4.Text = "*";
		this.txtdateto.CustomFormat = "dd-MMM-yy";
		this.txtdateto.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdateto.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtdateto.Location = new System.Drawing.Point(89, 92);
		this.txtdateto.Name = "txtdateto";
		this.txtdateto.Size = new System.Drawing.Size(123, 23);
		this.txtdateto.TabIndex = 2;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(11, 96);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(48, 15);
		this.label5.TabIndex = 575;
		this.label5.Text = "Date To";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.Red;
		this.label1.Location = new System.Drawing.Point(75, 67);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(14, 17);
		this.label1.TabIndex = 1;
		this.label1.Text = "*";
		this.txtdatefrom.CustomFormat = "dd-MMM-yy";
		this.txtdatefrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdatefrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtdatefrom.Location = new System.Drawing.Point(89, 64);
		this.txtdatefrom.Name = "txtdatefrom";
		this.txtdatefrom.Size = new System.Drawing.Size(123, 23);
		this.txtdatefrom.TabIndex = 1;
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label51.Location = new System.Drawing.Point(11, 68);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(63, 15);
		this.label51.TabIndex = 572;
		this.label51.Text = "Date From";
		this.panel5.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel5.Controls.Add(this.label3);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(969, 30);
		this.panel5.TabIndex = 19;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.White;
		this.label3.Location = new System.Drawing.Point(8, 5);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(257, 19);
		this.label3.TabIndex = 0;
		this.label3.Text = "Export Shipment Weight Audit";
		this.btnShow.BackColor = System.Drawing.Color.Teal;
		this.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShow.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShow.ForeColor = System.Drawing.Color.White;
		this.btnShow.Location = new System.Drawing.Point(444, 90);
		this.btnShow.Name = "btnShow";
		this.btnShow.Size = new System.Drawing.Size(65, 26);
		this.btnShow.TabIndex = 5;
		this.btnShow.Text = "Show";
		this.btnShow.UseVisualStyleBackColor = false;
		this.btnShow.Click += new System.EventHandler(btnShow_Click);
		this.btnShow.Leave += new System.EventHandler(btnShow_Leave);
		this.panel2.BackColor = System.Drawing.Color.White;
		this.panel2.Controls.Add(this.tsItemType);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(969, 27);
		this.panel2.TabIndex = 0;
		this.tsItemType.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tsItemType.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.tsItemType.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnnew, this.btnPrint, this.btnPrint618_01, this.btnShortCutKey });
		this.tsItemType.Location = new System.Drawing.Point(0, 0);
		this.tsItemType.Name = "tsItemType";
		this.tsItemType.Size = new System.Drawing.Size(969, 27);
		this.tsItemType.TabIndex = 3;
		this.tsItemType.Text = "toolStrip4";
		this.btnnew.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(58, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(92, 24);
		this.btnPrint.Text = "618-Print";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.btnShortCutKey.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnShortCutKey.Image = (System.Drawing.Image)resources.GetObject("btnShortCutKey.Image");
		this.btnShortCutKey.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKey.Name = "btnShortCutKey";
		this.btnShortCutKey.Size = new System.Drawing.Size(106, 24);
		this.btnShortCutKey.Text = "ShortCut Keys";
		this.btnShortCutKey.Click += new System.EventHandler(btnShortCutKey_Click);
		this.btnPrint618_01.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint618_01.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint618_01.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint618_01.Name = "btnPrint618_01";
		this.btnPrint618_01.Size = new System.Drawing.Size(116, 24);
		this.btnPrint618_01.Text = "618_01-Print";
		this.btnPrint618_01.Click += new System.EventHandler(btnPrint618_01_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(969, 648);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmAuditByWeightReport";
		this.Text = "Summary By Month";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmExportInvoiceSummaryByMonth_FormClosing);
		base.Load += new System.EventHandler(frmExportShipingLineBookingRpt_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmExportShipingLineBookingRpt_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.panel6.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbInvoiceNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbContractNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbperemeter).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.tsItemType.ResumeLayout(false);
		this.tsItemType.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
