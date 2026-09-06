// using Architecture.Reports;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Architecture.BLL.Export;
using Architecture.BLL.Reports.Export;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
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
namespace Architecture.WinApp.ExportReports
{

public class frmExImShipmentCostingSummary : Form
{
	private DropDownBind DDL = new DropDownBind();

	private DataTable dtGrid = new DataTable();

	private DataTable dtForLayout = new DataTable();

	private bool IsFormInitialized;

	private double CustomClearing;

	private double ForwarderCharges;

	private double Transportation;

	private double OtherCharges;

	private double PortAndClearing;

	private double OceanFreight;

	private double InspectionAndTesting;

	private double Insurance;

	public UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel1;

	private Panel panel4;

	private Panel panel3;

	private Panel panel2;

	private ToolStrip tsItemType;

	private ToolStripButton btnnew;

	private Label label2;

	private Button btnShow;

	private Panel panel5;

	private Label label3;

	private CtrlGrdBar ctrlGrdBar1;

	private ToolStripButton btnRegister;

	internal GridEX grdfrm;

	public UltraCombo cmbInvoiceNo;

	public UltraCombo CmbSupplierCustomer;

	private Label label1;

	public UltraCombo CmbDestinationPort;

	private Label label4;

	private ToolStripButton btnRefresh;

	public DateTimePicker DateFrom;

	private Label label6;

	public DateTimePicker DateTo;

	public frmExImShipmentCostingSummary(UserAccount pUserAccount)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUserAccount;
	}

	private void ExImShipmentCosting_Load(object sender, EventArgs e)
	{
		try
		{
			ComboFill();
			DateFrom.Value = clsGlobalVariables.ActiveYr.Start_Period;
			IsFormInitialized = true;
			CmbSupplierCustomer.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ComboFill()
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Expected O, but got Unknown
		try
		{
			DataTable dtInvoice = new DataTable();
			dtInvoice.Columns.Add("Id");
			dtInvoice.Columns.Add("name");
			DataTable dtCustomer = dtInvoice.Clone();
			DataTable dtPort = dtInvoice.Clone();
			DataTable Data = ExportVoucher.GetDataForDropDownFromExportInvoiceVoucher(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (Data.Rows.Count > 0)
			{
				for (int i = 0; i < Data.Rows.Count; i++)
				{
					if (Conversion.ToString(Data.Rows[i]["ActivityType"]) == "Customer")
					{
						dtCustomer.Rows.Add(Data.Rows[i]["Id"], Data.Rows[i]["name"]);
					}
					else if (Conversion.ToString(Data.Rows[i]["ActivityType"]) == "Invoice")
					{
						dtInvoice.Rows.Add(Data.Rows[i]["Id"], Data.Rows[i]["name"]);
					}
					else if (Conversion.ToString(Data.Rows[i]["ActivityType"]) == "DestinationPort")
					{
						dtPort.Rows.Add(Data.Rows[i]["Id"], Data.Rows[i]["name"]);
					}
				}
			}
			DDL.BindDDL(dtCustomer, CmbSupplierCustomer, "Id", "name", "Customer Name", ZeroIndex: false);
			((UltraGridBase)CmbSupplierCustomer).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			DDL.BindDDL(dtPort, CmbDestinationPort, "Id", "name", "Destination Port", ZeroIndex: false);
			((UltraGridBase)CmbDestinationPort).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			DDL.BindDDL(dtInvoice, cmbInvoiceNo, "Id", "name", "Invoice No", ZeroIndex: false);
			((UltraGridBase)cmbInvoiceNo).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void btnShow_Click(object sender, EventArgs e)
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

	private void Reset()
	{
		try
		{
			((Control)(object)cmbInvoiceNo).Text = string.Empty;
			cmbInvoiceNo.Focus();
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
			if (e.Control && e.KeyCode == Keys.R)
			{
				btnRefresh_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				btnShow_Click(null, null);
			}
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

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			if (IsFormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grdfrm, dtForLayout);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void GridBind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Expected O, but got Unknown
		try
		{
			dtGrid = ExportReports.ExportShipmentCosting_SummaryReport(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FromDate = DateFrom.Value,
				ToDate = DateTo.Value,
				SupplierCustomerId = Conversion.ToInt(CmbSupplierCustomer.Value),
				DestinationPortId = Conversion.ToInt(CmbDestinationPort.Value),
				ExImInvoiceId = Conversion.ToInt(cmbInvoiceNo.Value)
			})?.Tables?[0]?.Copy();
			if (dtGrid.Rows.Count > 0)
			{
				DatatableHelper.ReplaceCompLogoColumn(dtGrid, clsGlobalVariables.dtCompLogo);
				DataTable dt = new DataTable();
				dt.Columns.Add("SupplierCustomerId", typeof(int));
				dt.Columns.Add("CustomerName", typeof(string));
				dt.Columns.Add("ContractIds", typeof(string));
				dt.Columns.Add("ContractNo's", typeof(string));
				dt.Columns.Add("InvoiceId", typeof(int));
				dt.Columns.Add("InvoiceNo", typeof(string));
				dt.Columns.Add("PaymentTerms", typeof(string));
				dt.Columns.Add("DeliveryTerm", typeof(string));
				dt.Columns.Add("InvoiceDate", typeof(DateTime));
				dt.Columns.Add("InvoiceWeight", typeof(double));
				dt.Columns.Add("InvoiceMTon", typeof(double));
				dt.Columns.Add("FcyCode", typeof(string));
				dt.Columns.Add("ExchangeRate", typeof(double));
				dt.Columns.Add("InvoiceAmount", typeof(double));
				dt.Columns.Add("RealizedAmount", typeof(double));
				dt.Columns.Add("InvoiceBalance", typeof(double));
				dt.Columns.Add("SaleValue", typeof(double));
				dt.Columns.Add("CommissionValue", typeof(double));
				dt.Columns.Add("StockValue", typeof(double));
				dt.Columns.Add("PackingValue", typeof(double));
				dt.Columns.Add("LogisticsValue", typeof(double));
				dt.Columns.Add("CustomClearing", typeof(double));
				dt.Columns.Add("ForwarderCharges", typeof(double));
				dt.Columns.Add("Transportation", typeof(double));
				dt.Columns.Add("OtherCharges", typeof(double));
				dt.Columns.Add("PortAndClearing", typeof(double));
				dt.Columns.Add("OceanFreight", typeof(double));
				dt.Columns.Add("InspectionAndTesting", typeof(double));
				dt.Columns.Add("Insurance", typeof(double));
				dt.Columns.Add("GainLossValue", typeof(double));
				dt.Columns.Add("RealizedLcyAmount", typeof(double));
				dt.Columns.Add("PLValue", typeof(double));
				dt.Columns.Add("PlPercent", typeof(double));
				dt.Columns.Add("ShipmentWeight", typeof(double));
				dt.Clear();
				dtForLayout = dt.Clone();
				CustomClearing = 0.0;
				ForwarderCharges = 0.0;
				Transportation = 0.0;
				OtherCharges = 0.0;
				PortAndClearing = 0.0;
				OceanFreight = 0.0;
				InspectionAndTesting = 0.0;
				Insurance = 0.0;
				for (int i = 0; i < dtGrid.Rows.Count; i++)
				{
					CustomClearing += Conversion.ToDouble(dtGrid.Rows[i]["CustomClearing"]);
					ForwarderCharges += Conversion.ToDouble(dtGrid.Rows[i]["ForwarderCharges"]);
					Transportation += Conversion.ToDouble(dtGrid.Rows[i]["Transportation"]);
					OtherCharges += Conversion.ToDouble(dtGrid.Rows[i]["OtherCharges"]);
					PortAndClearing += Conversion.ToDouble(dtGrid.Rows[i]["PortAndClearing"]);
					OceanFreight += Conversion.ToDouble(dtGrid.Rows[i]["OceanFreight"]);
					InspectionAndTesting += Conversion.ToDouble(dtGrid.Rows[i]["InspectionAndTesting"]);
					Insurance += Conversion.ToDouble(dtGrid.Rows[i]["Insurance"]);
					dt.Rows.Add(Conversion.ToInt(dtGrid.Rows[i]["SupplierCustomerId"]), Conversion.ToString(dtGrid.Rows[i]["CustomerName"]), Conversion.ToString(dtGrid.Rows[i]["ContractIds"]), Conversion.ToString(dtGrid.Rows[i]["ContractNos"]), Conversion.ToInt(dtGrid.Rows[i]["InvoiceId"]), Conversion.ToString(dtGrid.Rows[i]["InvoiceNo"]), Conversion.ToString(dtGrid.Rows[i]["PaymentTerms"]), Conversion.ToString(dtGrid.Rows[i]["DeliveryTerm"]), Conversion.ToDateTime(dtGrid.Rows[i]["InvoiceDate"]), Conversion.ToDouble(dtGrid.Rows[i]["InvoiceNetWeight"]), Conversion.ToDouble(dtGrid.Rows[i]["InvoiceNetMTon"]), Conversion.ToString(dtGrid.Rows[i]["FcyCode"]), Conversion.ToDouble(dtGrid.Rows[i]["ExchangeRate"]), Conversion.ToDouble(dtGrid.Rows[i]["InvoiceTotalAmount"]), Conversion.ToDouble(dtGrid.Rows[i]["RealizedAmount"]), Conversion.ToDouble(dtGrid.Rows[i]["InvoiceBalance"]), Conversion.ToDouble(dtGrid.Rows[i]["SalesValueTotal"]), Conversion.ToDouble(dtGrid.Rows[i]["CommissionValueTotal"]), Conversion.ToDouble(dtGrid.Rows[i]["StockValueTotal"]), Conversion.ToDouble(dtGrid.Rows[i]["PackingMaterialValueTotal"]), Conversion.ToDouble(dtGrid.Rows[i]["LogisticsValue"]), Conversion.ToDouble(dtGrid.Rows[i]["CustomClearing"]), Conversion.ToDouble(dtGrid.Rows[i]["ForwarderCharges"]), Conversion.ToDouble(dtGrid.Rows[i]["Transportation"]), Conversion.ToDouble(dtGrid.Rows[i]["OtherCharges"]), Conversion.ToDouble(dtGrid.Rows[i]["PortAndClearing"]), Conversion.ToDouble(dtGrid.Rows[i]["OceanFreight"]), Conversion.ToDouble(dtGrid.Rows[i]["InspectionAndTesting"]), Conversion.ToDouble(dtGrid.Rows[i]["Insurance"]), Conversion.ToDouble(dtGrid.Rows[i]["GainLossValueTotal"]), Conversion.ToDouble(dtGrid.Rows[i]["RealizedLcyAmount"]), Conversion.ToDouble(dtGrid.Rows[i]["PLValue"]), Conversion.ToDouble(dtGrid.Rows[i]["PLPercent"]), Conversion.ToDouble(dtGrid.Rows[i]["ShipmentWeight"]));
				}
				grdfrm.DataSource = dt;
				grdfrm.RetrieveStructure();
				dtForLayout = dt.Copy();
				GridSetting();
			}
			else
			{
				grdfrm.ClearStructure();
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
			grdfrm.ColumnAutoResize = false;
			((ScrollableBase)grdfrm).ScrollBars = (ScrollBars)4;
			grdfrm.RootTable.Columns["SupplierCustomerId"].Visible = false;
			grdfrm.RootTable.Columns["InvoiceId"].Visible = false;
			grdfrm.RootTable.Columns["ContractIds"].Visible = false;
			grdfrm.RootTable.Columns["ShipmentWeight"].Visible = false;
			grdfrm.RootTable.Columns["InvoiceWeight"].Visible = false;
			grdfrm.RootTable.Columns["PaymentTerms"].Visible = false;
			grdfrm.RootTable.Columns["LogisticsValue"].Visible = false;
			grdfrm.RootTable.Columns["CustomClearing"].Visible = CustomClearing > 0.0;
			grdfrm.RootTable.Columns["ForwarderCharges"].Visible = ForwarderCharges > 0.0;
			grdfrm.RootTable.Columns["Transportation"].Visible = Transportation > 0.0;
			grdfrm.RootTable.Columns["OtherCharges"].Visible = OtherCharges > 0.0;
			grdfrm.RootTable.Columns["PortAndClearing"].Visible = PortAndClearing > 0.0;
			grdfrm.RootTable.Columns["OceanFreight"].Visible = OceanFreight > 0.0;
			grdfrm.RootTable.Columns["InspectionAndTesting"].Visible = InspectionAndTesting > 0.0;
			grdfrm.RootTable.Columns["Insurance"].Visible = Insurance > 0.0;
			GridEX_Helper.GridWrappingAndColumnSettings(grdfrm, 2, 3, null, null, null, ColumnSetting: false);
			grdfrm.RootTable.Columns["PLValue"].Caption = "Profit Loss Amount";
			grdfrm.RootTable.Columns["PlPercent"].Caption = "Profit Loss(%)";
			grdfrm.RootTable.Columns["InspectionAndTesting"].Caption = "Inspection & Testing";
			grdfrm.RootTable.Columns["OceanFreight"].Caption = "Ocean Freight";
			grdfrm.RootTable.Columns["PortAndClearing"].Caption = "Port & Clearing";
			grdfrm.RootTable.Columns["OtherCharges"].Caption = "Other Charges";
			grdfrm.RootTable.Columns["ForwarderCharges"].Caption = "Forwarder Charges";
			grdfrm.RootTable.Columns["CustomClearing"].Caption = "Custom Clearing";
			grdfrm.RootTable.Columns["CustomerName"].Width = Constants.ExportConstants.PartyName + 30;
			grdfrm.RootTable.Columns["ContractNo's"].Width = Constants.ExportConstants.ContractNoConstant + 50;
			grdfrm.RootTable.Columns["InvoiceNo"].Width = Constants.ExportConstants.InvoiceNoConstant;
			grdfrm.RootTable.Columns["InvoiceDate"].Width = Constants.ExportConstants.DateConstant + 15;
			grdfrm.RootTable.Columns["InvoiceWeight"].Width = Constants.ExportConstants.WeightConstant + 20;
			grdfrm.RootTable.Columns["InvoiceMTon"].Width = Constants.ExportConstants.QtyConstant + 20;
			grdfrm.RootTable.Columns["FcyCode"].Width = Constants.ExportConstants.FcyConstant;
			grdfrm.RootTable.Columns["PaymentTerms"].Width = Constants.ExportConstants.TermConstant + 50;
			grdfrm.RootTable.Columns["DeliveryTerm"].Width = Constants.ExportConstants.TermConstant;
			grdfrm.RootTable.Columns["ExchangeRate"].Width = Constants.ExportConstants.ExRateConstant;
			grdfrm.RootTable.Columns["InvoiceAmount"].Width = Constants.ExportConstants.AmountConstant + 30;
			grdfrm.RootTable.Columns["RealizedAmount"].Width = Constants.ExportConstants.AmountConstant;
			grdfrm.RootTable.Columns["InvoiceBalance"].Width = Constants.ExportConstants.AmountConstant;
			grdfrm.RootTable.Columns["SaleValue"].Width = Constants.ExportConstants.AmountConstant + 20;
			grdfrm.RootTable.Columns["CommissionValue"].Width = Constants.ExportConstants.AmountConstant;
			grdfrm.RootTable.Columns["StockValue"].Width = Constants.ExportConstants.AmountConstant + 20;
			grdfrm.RootTable.Columns["PackingValue"].Width = Constants.ExportConstants.AmountConstant;
			grdfrm.RootTable.Columns["LogisticsValue"].Width = Constants.ExportConstants.AmountConstant;
			grdfrm.RootTable.Columns["GainLossValue"].Width = Constants.ExportConstants.AmountConstant;
			grdfrm.RootTable.Columns["RealizedLcyAmount"].Width = Constants.ExportConstants.AmountConstant;
			grdfrm.RootTable.Columns["PLValue"].Width = Constants.ExportConstants.AmountConstant;
			grdfrm.RootTable.Columns["PlPercent"].Width = Constants.ExportConstants.PercentOfTotal;
			grdfrm.RootTable.Columns["ShipmentWeight"].Width = Constants.ExportConstants.WeightConstant;
			grdfrm.RootTable.Columns["InvoiceWeight"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["InvoiceWeight"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["InvoiceWeight"].FormatString = "#,##0.###";
			grdfrm.RootTable.Columns["InvoiceWeight"].TotalFormatString = "#,##0.###";
			grdfrm.RootTable.Columns["InvoiceMTon"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["InvoiceMTon"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["InvoiceMTon"].FormatString = "#,##0.###";
			grdfrm.RootTable.Columns["InvoiceMTon"].TotalFormatString = "#,##0.###";
			grdfrm.RootTable.Columns["ExchangeRate"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["ExchangeRate"].FormatString = clsGlobalVariables.DecimalFCYRateFormate;
			grdfrm.RootTable.Columns["InvoiceAmount"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["InvoiceAmount"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["InvoiceAmount"].FormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdfrm.RootTable.Columns["InvoiceAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdfrm.RootTable.Columns["RealizedAmount"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["RealizedAmount"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["RealizedAmount"].FormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdfrm.RootTable.Columns["RealizedAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdfrm.RootTable.Columns["InvoiceBalance"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["InvoiceBalance"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["InvoiceBalance"].FormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdfrm.RootTable.Columns["InvoiceBalance"].TotalFormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdfrm.RootTable.Columns["SaleValue"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["SaleValue"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["SaleValue"].FormatString = "#,##0";
			grdfrm.RootTable.Columns["SaleValue"].TotalFormatString = "#,##0";
			grdfrm.RootTable.Columns["CommissionValue"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["CommissionValue"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["CommissionValue"].FormatString = "#,##0";
			grdfrm.RootTable.Columns["CommissionValue"].TotalFormatString = "#,##0";
			grdfrm.RootTable.Columns["StockValue"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["StockValue"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["StockValue"].FormatString = "#,##0";
			grdfrm.RootTable.Columns["StockValue"].TotalFormatString = "#,##0";
			grdfrm.RootTable.Columns["PackingValue"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["PackingValue"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["PackingValue"].FormatString = "#,##0";
			grdfrm.RootTable.Columns["PackingValue"].TotalFormatString = "#,##0";
			grdfrm.RootTable.Columns["LogisticsValue"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["LogisticsValue"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["LogisticsValue"].FormatString = "#,##0";
			grdfrm.RootTable.Columns["LogisticsValue"].TotalFormatString = "#,##0";
			grdfrm.RootTable.Columns["CustomClearing"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["CustomClearing"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["CustomClearing"].FormatString = "#,##0";
			grdfrm.RootTable.Columns["CustomClearing"].TotalFormatString = "#,##0";
			grdfrm.RootTable.Columns["ForwarderCharges"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["ForwarderCharges"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["ForwarderCharges"].FormatString = "#,##0";
			grdfrm.RootTable.Columns["ForwarderCharges"].TotalFormatString = "#,##0";
			grdfrm.RootTable.Columns["Transportation"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["Transportation"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["Transportation"].FormatString = "#,##0";
			grdfrm.RootTable.Columns["Transportation"].TotalFormatString = "#,##0";
			grdfrm.RootTable.Columns["OtherCharges"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["OtherCharges"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["OtherCharges"].FormatString = "#,##0";
			grdfrm.RootTable.Columns["OtherCharges"].TotalFormatString = "#,##0";
			grdfrm.RootTable.Columns["PortAndClearing"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["PortAndClearing"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["PortAndClearing"].FormatString = "#,##0";
			grdfrm.RootTable.Columns["PortAndClearing"].TotalFormatString = "#,##0";
			grdfrm.RootTable.Columns["OceanFreight"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["OceanFreight"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["OceanFreight"].FormatString = "#,##0";
			grdfrm.RootTable.Columns["OceanFreight"].TotalFormatString = "#,##0";
			grdfrm.RootTable.Columns["InspectionAndTesting"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["InspectionAndTesting"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["InspectionAndTesting"].FormatString = "#,##0";
			grdfrm.RootTable.Columns["InspectionAndTesting"].TotalFormatString = "#,##0";
			grdfrm.RootTable.Columns["Insurance"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["Insurance"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["Insurance"].FormatString = "#,##0";
			grdfrm.RootTable.Columns["Insurance"].TotalFormatString = "#,##0";
			grdfrm.RootTable.Columns["GainLossValue"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["GainLossValue"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["GainLossValue"].FormatString = "#,##0; (#,##0)";
			grdfrm.RootTable.Columns["GainLossValue"].TotalFormatString = "#,##0; (#,##0)";
			grdfrm.RootTable.Columns["RealizedLcyAmount"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["RealizedLcyAmount"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["RealizedLcyAmount"].FormatString = "#,##0";
			grdfrm.RootTable.Columns["RealizedLcyAmount"].TotalFormatString = "#,##0";
			grdfrm.RootTable.Columns["PLValue"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["PLValue"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["PLValue"].FormatString = "#,##0;(#,##0);0";
			grdfrm.RootTable.Columns["PLValue"].TotalFormatString = "#,##0;(#,##0);0";
			grdfrm.RootTable.Columns["PlPercent"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["PlPercent"].FormatString = "#,##0.###;(#,##0.###);0";
			grdfrm.RootTable.Columns["ShipmentWeight"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["ShipmentWeight"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["ShipmentWeight"].FormatString = "#,##0.###";
			grdfrm.RootTable.Columns["ShipmentWeight"].TotalFormatString = "#,##0.###";
			grdfrm.RootTable.Columns["InvoiceNo"].ColumnType = (ColumnType)5;
			((Control)(object)grdfrm).Refresh();
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnRegister_Click(object sender, EventArgs e)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtGrid.Rows.Count == 0 || dtGrid == null)
			{
				throw new Exception("Record Not Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dtGrid, "542_01-ShipmentCostingSummary.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ExImShipmentCosting_FormClosed(object sender, FormClosedEventArgs e)
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

	private void grdfrm_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdfrm.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				int InvoiceId = Conversion.ToInt(r.Cells["InvoiceId"].Value);
				ExImShipmentCosting exImShipmentCosting = new ExImShipmentCosting(UserAccount);
				exImShipmentCosting.InvoiceId = InvoiceId;
				exImShipmentCosting.Show();
			}
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

	private void grdfrm_FormattingRow(object sender, RowLoadEventArgs e)
	{
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Expected O, but got Unknown
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Expected O, but got Unknown
		try
		{
			if (grdfrm.RowCount <= 0)
			{
				return;
			}
			GridEXColumn PLCol = grdfrm.RootTable.Columns["PLValue"];
			if (PLCol != null)
			{
				GridEXCell PLCell = e.Row.Cells[((JanusCollectionElement)PLCol).Index];
				if (Conversion.ToDouble(PLCell.Value) > 0.0)
				{
					PLCell.FormatStyle = new GridEXFormatStyle();
					PLCell.FormatStyle.ForeColor = Color.Green;
					PLCell.FormatStyle.FontBold = (TriState)2;
				}
				else if (Conversion.ToDouble(PLCell.Value) < 0.0)
				{
					PLCell.FormatStyle = new GridEXFormatStyle();
					PLCell.FormatStyle.ForeColor = Color.Red;
					PLCell.FormatStyle.FontBold = (TriState)2;
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
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Expected O, but got Unknown
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Expected O, but got Unknown
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Expected O, but got Unknown
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Expected O, but got Unknown
		//IL_0471: Unknown result type (might be due to invalid IL or missing references)
		//IL_047b: Expected O, but got Unknown
		//IL_0488: Unknown result type (might be due to invalid IL or missing references)
		//IL_0492: Expected O, but got Unknown
		GridEXLayout grdfrm_DesignTimeLayout = new GridEXLayout();
		Appearance appearance6 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance11 = new Appearance();
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
		Appearance appearance24 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.grdfrm = new GridEX();
		this.panel3 = new System.Windows.Forms.Panel();
		this.CmbDestinationPort = new UltraCombo();
		this.label4 = new System.Windows.Forms.Label();
		this.CmbSupplierCustomer = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label3 = new System.Windows.Forms.Label();
		this.btnShow = new System.Windows.Forms.Button();
		this.cmbInvoiceNo = new UltraCombo();
		this.label2 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.tsItemType = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnRegister = new System.Windows.Forms.ToolStripButton();
		this.DateFrom = new System.Windows.Forms.DateTimePicker();
		this.label6 = new System.Windows.Forms.Label();
		this.DateTo = new System.Windows.Forms.DateTimePicker();
		this.panel1.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdfrm).BeginInit();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDestinationPort).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplierCustomer).BeginInit();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbInvoiceNo).BeginInit();
		this.panel2.SuspendLayout();
		this.tsItemType.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(968, 612);
		this.panel1.TabIndex = 0;
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.grdfrm);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 119);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(968, 493);
		this.panel4.TabIndex = 2;
		this.grdfrm.AllowEdit = (InheritableBoolean)2;
		grdfrm_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdfrm.DesignTimeLayout = grdfrm_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdfrm).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdfrm.DynamicFiltering = true;
		this.grdfrm.FilterMode = (FilterMode)2;
		this.grdfrm.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdfrm).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdfrm.GroupTotals = (GroupTotals)2;
		this.grdfrm.HideColumnsWhenGrouped = (InheritableBoolean)1;
		((System.Windows.Forms.Control)(object)this.grdfrm).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdfrm).Name = "grdfrm";
		((ScrollableBase)this.grdfrm).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Size = new System.Drawing.Size(968, 493);
		((System.Windows.Forms.Control)(object)this.grdfrm).TabIndex = 47;
		this.grdfrm.TotalRow = (InheritableBoolean)1;
		this.grdfrm.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdfrm).VisualStyle = (VisualStyle)4;
		this.grdfrm.FormattingRow += new RowLoadEventHandler(grdfrm_FormattingRow);
		this.grdfrm.LinkClicked += new ColumnActionEventHandler(grdfrm_LinkClicked);
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add(this.DateFrom);
		this.panel3.Controls.Add(this.label6);
		this.panel3.Controls.Add(this.DateTo);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDestinationPort);
		this.panel3.Controls.Add(this.label4);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSupplierCustomer);
		this.panel3.Controls.Add(this.label1);
		this.panel3.Controls.Add(this.panel5);
		this.panel3.Controls.Add(this.btnShow);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbInvoiceNo);
		this.panel3.Controls.Add(this.label2);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 27);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(968, 92);
		this.panel3.TabIndex = 1;
		this.CmbDestinationPort.AllowNull = (DefaultableBoolean)1;
		this.CmbDestinationPort.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDestinationPort.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDestinationPort.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Appearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDestinationPort).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance23;
		((SpecialBoxBase)((UltraGridBase)this.CmbDestinationPort).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance31).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance31).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance7;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance26).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDestinationPort).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDestinationPort).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDestinationPort.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDestinationPort).Location = new System.Drawing.Point(493, 35);
		((System.Windows.Forms.Control)(object)this.CmbDestinationPort).Name = "CmbDestinationPort";
		((System.Windows.Forms.Control)(object)this.CmbDestinationPort).Size = new System.Drawing.Size(289, 26);
		((System.Windows.Forms.Control)(object)this.CmbDestinationPort).TabIndex = 3;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(397, 41);
		this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(93, 15);
		this.label4.TabIndex = 23;
		this.label4.Text = "Destination Port";
		this.CmbSupplierCustomer.AllowNull = (DefaultableBoolean)1;
		this.CmbSupplierCustomer.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSupplierCustomer.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSupplierCustomer.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance11).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Appearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance13).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance13).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance13;
		((AppearanceBase)appearance14).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance14;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance15).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance15).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance16;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance18;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance19).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance20).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance20).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance20).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSupplierCustomer).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSupplierCustomer.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSupplierCustomer).Location = new System.Drawing.Point(105, 62);
		((System.Windows.Forms.Control)(object)this.CmbSupplierCustomer).Name = "CmbSupplierCustomer";
		((System.Windows.Forms.Control)(object)this.CmbSupplierCustomer).Size = new System.Drawing.Size(289, 26);
		((System.Windows.Forms.Control)(object)this.CmbSupplierCustomer).TabIndex = 2;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(9, 68);
		this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(93, 15);
		this.label1.TabIndex = 21;
		this.label1.Text = "Customer Name";
		this.panel5.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel5.Controls.Add(this.ctrlGrdBar1);
		this.panel5.Controls.Add(this.label3);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(968, 31);
		this.panel5.TabIndex = 19;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(927, 4);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdfrm;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 23);
		this.ctrlGrdBar1.TabIndex = 0;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.White;
		this.label3.Location = new System.Drawing.Point(6, 5);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(233, 19);
		this.label3.TabIndex = 0;
		this.label3.Text = "Shipment Costing Summary";
		this.btnShow.BackColor = System.Drawing.Color.Teal;
		this.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShow.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShow.ForeColor = System.Drawing.Color.White;
		this.btnShow.Location = new System.Drawing.Point(783, 62);
		this.btnShow.Name = "btnShow";
		this.btnShow.Size = new System.Drawing.Size(55, 26);
		this.btnShow.TabIndex = 5;
		this.btnShow.Text = "Show";
		this.btnShow.UseVisualStyleBackColor = false;
		this.btnShow.Click += new System.EventHandler(btnShow_Click);
		this.cmbInvoiceNo.AllowNull = (DefaultableBoolean)1;
		this.cmbInvoiceNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbInvoiceNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbInvoiceNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance33;
		((SpecialBoxBase)((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance35;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance2).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance2;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance3).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance4).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance4).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance4).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance4).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbInvoiceNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbInvoiceNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbInvoiceNo).Location = new System.Drawing.Point(493, 62);
		((System.Windows.Forms.Control)(object)this.cmbInvoiceNo).Name = "cmbInvoiceNo";
		((System.Windows.Forms.Control)(object)this.cmbInvoiceNo).Size = new System.Drawing.Size(289, 26);
		((System.Windows.Forms.Control)(object)this.cmbInvoiceNo).TabIndex = 4;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(397, 68);
		this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(65, 15);
		this.label2.TabIndex = 18;
		this.label2.Text = "Invoice No";
		this.panel2.BackColor = System.Drawing.Color.White;
		this.panel2.Controls.Add(this.tsItemType);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(968, 27);
		this.panel2.TabIndex = 0;
		this.tsItemType.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.tsItemType.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnnew, this.btnRefresh, this.btnRegister });
		this.tsItemType.Location = new System.Drawing.Point(0, 0);
		this.tsItemType.Name = "tsItemType";
		this.tsItemType.Size = new System.Drawing.Size(968, 27);
		this.tsItemType.TabIndex = 3;
		this.tsItemType.Text = "toolStrip4";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnRegister.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRegister.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnRegister.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRegister.Name = "btnRegister";
		this.btnRegister.Size = new System.Drawing.Size(256, 24);
		this.btnRegister.Text = "542_01-ShipmentCostingSummary";
		this.btnRegister.Click += new System.EventHandler(btnRegister_Click);
		this.DateFrom.CustomFormat = "dd-MMM-yy";
		this.DateFrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DateFrom.Location = new System.Drawing.Point(105, 37);
		this.DateFrom.Name = "DateFrom";
		this.DateFrom.Size = new System.Drawing.Size(143, 23);
		this.DateFrom.TabIndex = 0;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(12, 41);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(91, 15);
		this.label6.TabIndex = 206;
		this.label6.Text = "From && To Date";
		this.DateTo.CustomFormat = "dd-MMM-yy";
		this.DateTo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DateTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DateTo.Location = new System.Drawing.Point(250, 37);
		this.DateTo.Name = "DateTo";
		this.DateTo.Size = new System.Drawing.Size(143, 23);
		this.DateTo.TabIndex = 1;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(968, 612);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmExImShipmentCostingSummary";
		this.Text = "Shipment Costing Register";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(ExImShipmentCosting_FormClosed);
		base.Load += new System.EventHandler(ExImShipmentCosting_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmExportShipingLineBookingRpt_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdfrm).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDestinationPort).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplierCustomer).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbInvoiceNo).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.tsItemType.ResumeLayout(false);
		this.tsItemType.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
