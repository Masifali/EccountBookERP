using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL.Export;
using Architecture.BLL.Inventory;
using Architecture.BLL.Reports.Export;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Export;
using Architecture.Reports;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.ExportReports;

public class ExportReceivableByDueDateRegisterA : Form
{
	private DropDownBind DDL = new DropDownBind();

	private DataTable dtMain = new DataTable();

	public UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel1;

	private Panel panel4;

	private Panel panel3;

	private Button button1;

	internal UltraCombo CmbInvoiceNo;

	private Label lblInvoiceNo;

	internal UltraCombo CmbCustomerName;

	private Label lblCustomerName;

	private Panel panel5;

	private CtrlGrdBar ctrlGrdBar1;

	private Label label3;

	internal UltraCombo cmbContractNo;

	private Label lblContractNo;

	internal UltraCombo cmbPaymentTerm;

	private Label lblPaymentTerm;

	internal UltraCombo cmbDeliveryTerm;

	private Label lblDeliveryTerm;

	internal UltraCombo cmbDestinationPort;

	private Label lblDestinationPort;

	private Label label1;

	public DateTimePicker datFromDate;

	public DateTimePicker datToDate;

	private Panel panel2;

	internal GridEX grdfrm;

	internal GridEX grdCustomer;

	internal GridEX grdBank;

	internal GridEX grdFCY;

	internal GridEX grdSaleMan;

	private CheckBox chkSkipZero;

	internal GridEX grdEtaDestination;

	private ToolStrip toolStrip1;

	private ToolStripButton btnNew;

	private ToolStripButton btnRefresh;

	private ToolStripButton BtnPrint;

	public ExportReceivableByDueDateRegisterA(UserAccount pUserAccount)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUserAccount;
	}

	private void frmExportShipingLineBookingRpt_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1620, 525);
			datFromDate.Focus();
			datFromDate.Value = clsGlobalVariables.ActiveYr.Start_Period;
			datFromDate.Checked = false;
			datToDate.Checked = false;
			InvoiceNoGetAll();
			ContractNoGetAll();
			CustomerGetAll();
			SeaPortBind();
			PaymentTermsFill();
			DeliveryTermFill();
			GetExportItemsFromVouchers();
			GetExportJobLotsFromVouchers();
			GridBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InvoiceNoGetAll()
	{
		try
		{
			DataTable dtlcorder = CommonServices.GetExportInvoiceNo();
			if (dtlcorder.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtlcorder, CmbInvoiceNo, "Id", "InvoiceNo", "Invoice No", true);
				((UltraGridBase)CmbInvoiceNo).DisplayLayout.Bands[0].Columns[1].Width = 300;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SeaPortBind()
	{
		try
		{
			DataTable dt = CommonServices.GetSeaPortForComboServiceBind();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbDestinationPort, "Id", "PortName", "Port Name", true);
				((UltraGridBase)cmbDestinationPort).DisplayLayout.Bands[0].Columns[1].Width = 300;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PaymentTermsFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtDeliveryTerms = ExImLcPaymentTerm.Getall(new ExImLcPaymentTerm
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtDeliveryTerms.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtDeliveryTerms, cmbPaymentTerm, "Id", "LcOrderTerm", "PaymentTerms", true);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeliveryTermFill()
	{
		try
		{
			DataTable dtDeliveryTerms = ExImDeliveryTerm.Getall();
			DDL.BindDDL(dtDeliveryTerms, cmbDeliveryTerm, "Id", "Code", "DeliveryTerms", ZeroIndex: true);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ContractNoGetAll()
	{
		try
		{
			DataTable dtlcorder = CommonServices.GetLcOrderNo();
			new DataTable
			{
				Columns = { "Id", "LcOrderNo" }
			};
			if (dtlcorder.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtlcorder, cmbContractNo, "Id", "LcOrderNo", "Contract No", true);
				((UltraGridBase)cmbContractNo).DisplayLayout.Bands[0].Columns[1].Width = 300;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CustomerGetAll()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable Supplierlst = SupplierCustomer.GetExportCustomerFromVouchers(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (Supplierlst.Rows.Count > 0)
			{
				DDL.BindDDL(Supplierlst, CmbCustomerName, "Id", "CustomerName", "Customer Name", ZeroIndex: true);
				((UltraGridBase)CmbCustomerName).DisplayLayout.Bands[0].Columns[1].Width = 300;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetExportItemsFromVouchers()
	{
	}

	private void GetExportJobLotsFromVouchers()
	{
	}

	private void GridBind(int SortNo = 0)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_10c2: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			dtMain.Rows.Clear();
			new DataSet();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			if (datFromDate.Checked)
			{
				obj.FromDate = Conversion.ToDateTime((object)datFromDate.Value);
			}
			if (datToDate.Checked)
			{
				obj.ToDate = Conversion.ToDateTime((object)datToDate.Value);
			}
			obj.CustomerId = Conversion.ToInt(CmbCustomerName.Value);
			obj.ContractId = Conversion.ToInt(cmbContractNo.Value);
			obj.InvoiceId = Conversion.ToInt(CmbInvoiceNo.Value);
			obj.PaymentTermId = Conversion.ToInt(cmbPaymentTerm.Value);
			obj.DeliveryTermId = Conversion.ToInt(cmbDeliveryTerm.Value);
			obj.DestinationPortId = Conversion.ToInt(cmbDestinationPort.Value);
			obj.SkipZero = (chkSkipZero.Checked ? 1 : 0);
			obj.SortNo = SortNo;
			DataSet exportReceivableRegister = ExportReports.GetExportReceivableRegister(obj);
			DataTable dtR = exportReceivableRegister.Tables[0].Copy();
			dtMain = dtR.Copy();
			DataTable dtB = exportReceivableRegister.Tables[1];
			DataTable dtC = exportReceivableRegister.Tables[2];
			DataTable dtS = exportReceivableRegister.Tables[3];
			DataTable dtF = exportReceivableRegister.Tables[4];
			DataTable dtETA = exportReceivableRegister.Tables[5];
			if (dtR.Rows.Count > 0)
			{
				DataTable dtRegister = new DataTable();
				dtRegister.Columns.Add("Customer", typeof(string));
				dtRegister.Columns.Add("BankName", typeof(string));
				dtRegister.Columns.Add("ContractIds", typeof(string));
				dtRegister.Columns.Add("ContractNo", typeof(string));
				dtRegister.Columns.Add("InvoiceId", typeof(int));
				dtRegister.Columns.Add("InvoiceNo", typeof(string));
				dtRegister.Columns.Add("PaymentTerm", typeof(string));
				dtRegister.Columns.Add("Fcy", typeof(string));
				dtRegister.Columns.Add("InvoiceValue", typeof(double));
				dtRegister.Columns.Add("AdvanceReceipt", typeof(double));
				dtRegister.Columns.Add("NetReceivable", typeof(double));
				dtRegister.Columns.Add("DueDate", typeof(DateTime));
				dtRegister.Columns.Add("InvoiceDate", typeof(DateTime));
				dtRegister.Columns.Add("BolDate", typeof(DateTime));
				dtRegister.Columns.Add("DueDays", typeof(double));
				dtRegister.Columns.Add("OverDueDays", typeof(int));
				dtRegister.Columns.Add("ETADestination", typeof(DateTime));
				dtRegister.Columns.Add("ReachedAtDestination", typeof(DateTime));
				dtRegister.Columns.Add("FCL", typeof(string));
				dtRegister.Columns.Add("MTons", typeof(double));
				dtRegister.Columns.Add("SaleMan", typeof(string));
				dtRegister.Columns.Add("ShippedOnBoard", typeof(DateTime));
				dtRegister.Columns.Add("InTransitDays", typeof(double));
				dtRegister.Columns.Add("IncoTerm", typeof(string));
				dtRegister.Columns.Add("Advance", typeof(double));
				dtRegister.Columns.Add("Receipt", typeof(double));
				dtRegister.Columns.Add("DestinationPort", typeof(string));
				dtRegister.Columns.Add("ExportReceivablesAging", typeof(string));
				dtRegister.Columns.Add("InvoiceStatus", typeof(string));
				for (int n = 0; n < dtR.Rows.Count; n++)
				{
					dtRegister.Rows.Add(Conversion.ToString(dtR.Rows[n]["Customer"]), Conversion.ToString(dtR.Rows[n]["BankName"]), Conversion.ToString(dtR.Rows[n]["ContractId"]), Conversion.ToString(dtR.Rows[n]["ContractNo"]), Conversion.ToInt(dtR.Rows[n]["InvoiceId"]), Conversion.ToString(dtR.Rows[n]["InvoiceNo"]), Conversion.ToString(dtR.Rows[n]["PaymentTerm"]), Conversion.ToString(dtR.Rows[n]["Fcy"]), Conversion.ToDouble(dtR.Rows[n]["InvoiceValue"]), Conversion.ToDouble(dtR.Rows[n]["AdvanceReceipt"]), Conversion.ToDouble(dtR.Rows[n]["NetReceivable"]), dtR.Rows[n]["DueDate"], dtR.Rows[n]["InvoiceDate"], dtR.Rows[n]["BolDate"], Conversion.ToDouble(dtR.Rows[n]["DueDays"]), dtR.Rows[n]["OverDueDays"], dtR.Rows[n]["ETADestination"], dtR.Rows[n]["ReachedAtDestination"], Conversion.ToString(dtR.Rows[n]["FCL"]), Conversion.ToDouble(dtR.Rows[n]["MTons"]), Conversion.ToString(dtR.Rows[n]["SaleMan"]), dtR.Rows[n]["ShippedOnBoard"], Conversion.ToDouble(dtR.Rows[n]["InTransitDays"]), dtR.Rows[n]["IncoTerm"], Conversion.ToDouble(dtR.Rows[n]["Advance"]), Conversion.ToDouble(dtR.Rows[n]["Receipt"]), Conversion.ToString(dtR.Rows[n]["DestinationPort"]), Conversion.ToString(dtR.Rows[n]["ExportReceivablesAging"]), Conversion.ToString(dtR.Rows[n]["InvoiceStatus"]));
				}
				grdfrm.DataSource = dtRegister;
				grdfrm.RetrieveStructure();
				GridSetting();
			}
			else
			{
				grdfrm.DataSource = null;
			}
			if (dtB.Rows.Count > 0)
			{
				DataTable dtBank = new DataTable();
				dtBank.Columns.Add("BankName", typeof(string));
				dtBank.Columns.Add("Fcy", typeof(string));
				dtBank.Columns.Add("NetReceivable", typeof(double));
				dtBank.Columns.Add("TotalReceivable", typeof(double));
				dtBank.Columns.Add("PrcntOfTotal", typeof(double));
				for (int m = 0; m < dtB.Rows.Count; m++)
				{
					dtBank.Rows.Add(Conversion.ToString(dtB.Rows[m]["BankName"]), Conversion.ToString(dtB.Rows[m]["Fcy"]), Conversion.ToDouble(dtB.Rows[m]["NetReceivable"]), Conversion.ToDouble(dtB.Rows[m]["TotalReceivable"]), Conversion.ToDouble(dtB.Rows[m]["PrcntOfTotal"]));
				}
				grdBank.DataSource = dtBank;
				grdBank.RetrieveStructure();
				GridBankSetting();
			}
			else
			{
				grdBank.DataSource = null;
			}
			if (dtC.Rows.Count > 0)
			{
				DataTable dtCustomer = new DataTable();
				dtCustomer.Columns.Add("Customer", typeof(string));
				dtCustomer.Columns.Add("Fcy", typeof(string));
				dtCustomer.Columns.Add("NetReceivable", typeof(double));
				dtCustomer.Columns.Add("TotalReceivable", typeof(double));
				dtCustomer.Columns.Add("PrcntOfTotal", typeof(double));
				for (int l = 0; l < dtC.Rows.Count; l++)
				{
					dtCustomer.Rows.Add(Conversion.ToString(dtC.Rows[l]["Customer"]), Conversion.ToString(dtC.Rows[l]["Fcy"]), Conversion.ToDouble(dtC.Rows[l]["NetReceivable"]), Conversion.ToDouble(dtC.Rows[l]["TotalReceivable"]), Conversion.ToDouble(dtC.Rows[l]["PrcntOfTotal"]));
				}
				grdCustomer.DataSource = dtCustomer;
				grdCustomer.RetrieveStructure();
				GridCustomerSetting();
			}
			else
			{
				grdCustomer.DataSource = null;
			}
			if (dtS.Rows.Count > 0)
			{
				DataTable dtSaleMan = new DataTable();
				dtSaleMan.Columns.Add("SaleMan", typeof(string));
				dtSaleMan.Columns.Add("Fcy", typeof(string));
				dtSaleMan.Columns.Add("NetReceivable", typeof(double));
				dtSaleMan.Columns.Add("TotalReceivable", typeof(double));
				dtSaleMan.Columns.Add("PrcntOfTotal", typeof(double));
				for (int k = 0; k < dtS.Rows.Count; k++)
				{
					dtSaleMan.Rows.Add(Conversion.ToString(dtS.Rows[k]["SaleMan"]), Conversion.ToString(dtS.Rows[k]["Fcy"]), Conversion.ToDouble(dtS.Rows[k]["NetReceivable"]), Conversion.ToDouble(dtS.Rows[k]["TotalReceivable"]), Conversion.ToDouble(dtS.Rows[k]["PrcntOfTotal"]));
				}
				grdSaleMan.DataSource = dtSaleMan;
				grdSaleMan.RetrieveStructure();
				GridSaleManSetting();
			}
			else
			{
				grdSaleMan.DataSource = null;
			}
			if (dtF.Rows.Count > 0)
			{
				DataTable dtFcy = new DataTable();
				dtFcy.Columns.Add("Fcy", typeof(string));
				dtFcy.Columns.Add("NetReceivable", typeof(double));
				dtFcy.Columns.Add("TotalReceivable", typeof(double));
				dtFcy.Columns.Add("PrcntOfTotal", typeof(double));
				for (int j = 0; j < dtF.Rows.Count; j++)
				{
					dtFcy.Rows.Add(Conversion.ToString(dtF.Rows[j]["Fcy"]), Conversion.ToDouble(dtF.Rows[j]["NetReceivable"]), Conversion.ToDouble(dtF.Rows[j]["TotalReceivable"]), Conversion.ToDouble(dtF.Rows[j]["PrcntOfTotal"]));
				}
				grdFCY.DataSource = dtFcy;
				grdFCY.RetrieveStructure();
				GridFcySetting();
			}
			else
			{
				grdFCY.DataSource = null;
			}
			if (dtETA.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("SortNo", typeof(int));
				dt.Columns.Add("ETA-DestinationPort");
				dt.Columns.Add("FcyAmount", typeof(double));
				dt.Columns.Add("PkrAmount", typeof(double));
				for (int i = 0; i < dtETA.Rows.Count; i++)
				{
					dt.Rows.Add(dtETA.Rows[i]["SortNo"], dtETA.Rows[i]["ETADestinationPort"], dtETA.Rows[i]["FcyAmount"], dtETA.Rows[i]["PkrAmount"]);
				}
				grdEtaDestination.DataSource = dt;
				grdEtaDestination.RetrieveStructure();
				grdEtaDestination.RootTable.HeaderLines = 2;
				foreach (GridEXColumn item in (JanusCollectionBase)grdEtaDestination.RootTable.Columns)
				{
					item.HeaderAlignment = (TextAlignment)2;
				}
				grdEtaDestination.RootTable.Columns["SortNo"].Visible = false;
				grdEtaDestination.RootTable.Columns["ETA-DestinationPort"].Caption = "Export Receivables Aging";
				grdEtaDestination.RootTable.Columns["ETA-DestinationPort"].Width = 325;
				grdEtaDestination.RootTable.Columns["FcyAmount"].Width = Constants.ExportConstants.AmountConstant + 30;
				grdEtaDestination.RootTable.Columns["PkrAmount"].Width = Constants.ExportConstants.LcyAmountConstant + 10;
				grdEtaDestination.RootTable.Columns["FcyAmount"].AggregateFunction = (AggregateFunction)2;
				grdEtaDestination.RootTable.Columns["FcyAmount"].TextAlignment = (TextAlignment)3;
				grdEtaDestination.RootTable.Columns["FcyAmount"].FormatString = "#,##0.00";
				grdEtaDestination.RootTable.Columns["FcyAmount"].TotalFormatString = "#,##0.00";
				grdEtaDestination.RootTable.Columns["PkrAmount"].AggregateFunction = (AggregateFunction)2;
				grdEtaDestination.RootTable.Columns["PkrAmount"].TextAlignment = (TextAlignment)3;
				grdEtaDestination.RootTable.Columns["PkrAmount"].FormatString = "#,##0.00";
				grdEtaDestination.RootTable.Columns["PkrAmount"].TotalFormatString = "#,##0.00";
				grdEtaDestination.TotalRow = (InheritableBoolean)1;
			}
			else
			{
				grdEtaDestination.ClearStructure();
			}
			((Control)(object)grdfrm).Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSetting()
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_095c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0962: Expected O, but got Unknown
		//IL_09be: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c4: Expected O, but got Unknown
		//IL_0a22: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a29: Expected O, but got Unknown
		try
		{
			grdfrm.RootTable.HeaderLines = 3;
			foreach (GridEXColumn item in (JanusCollectionBase)grdfrm.RootTable.Columns)
			{
				item.HeaderAlignment = (TextAlignment)2;
			}
			grdfrm.RootTable.Groups.Add("ExportReceivablesAging");
			grdfrm.GroupTotals = (GroupTotals)2;
			grdfrm.RootTable.Columns["ShippedOnBoard"].FormatString = "dd-MMM-yy";
			grdfrm.RootTable.Columns["InvoiceDate"].FormatString = "dd-MMM-yy";
			grdfrm.RootTable.Columns["DueDate"].FormatString = "dd-MMM-yy";
			grdfrm.RootTable.Columns["BolDate"].FormatString = "dd-MMM-yy";
			grdfrm.RootTable.Columns["ETADestination"].FormatString = "dd-MMM-yy";
			grdfrm.RootTable.Columns["ReachedAtDestination"].FormatString = "dd-MMM-yy";
			grdfrm.RootTable.Columns["IncoTerm"].FormatString = "dd-MMM-yy";
			grdfrm.RootTable.Columns["AdvanceReceipt"].Caption = "Receipt";
			grdfrm.RootTable.Columns["ETADestination"].Caption = "ETA Destination";
			grdfrm.RootTable.Columns["InvoiceId"].Visible = false;
			grdfrm.RootTable.Columns["ContractIds"].Visible = false;
			grdfrm.RootTable.Columns["Receipt"].Visible = false;
			grdfrm.RootTable.Columns["Advance"].Visible = false;
			grdfrm.RootTable.Columns["Receipt"].Visible = false;
			grdfrm.RootTable.Columns["Advance"].Visible = false;
			grdfrm.RootTable.Columns["FCL"].TextAlignment = (TextAlignment)2;
			grdfrm.RootTable.Columns["OverDueDays"].TextAlignment = (TextAlignment)2;
			grdfrm.RootTable.Columns["ContractNo"].ColumnType = (ColumnType)5;
			grdfrm.RootTable.Columns["Customer"].Width = Constants.ExportConstants.StringConstant;
			grdfrm.RootTable.Columns["BankName"].Width = Constants.ExportConstants.StringConstant;
			grdfrm.RootTable.Columns["ContractNo"].Width = Constants.ExportConstants.ContractNoConstant;
			grdfrm.RootTable.Columns["InvoiceNo"].Width = Constants.ExportConstants.InvoiceNoConstant;
			grdfrm.RootTable.Columns["InvoiceDate"].Width = Constants.ExportConstants.DateConstant;
			grdfrm.RootTable.Columns["BolDate"].Width = Constants.ExportConstants.DateConstant;
			grdfrm.RootTable.Columns["FCL"].Width = Constants.ExportConstants.FclConstant;
			grdfrm.RootTable.Columns["DueDays"].Width = Constants.ExportConstants.DueDays;
			grdfrm.RootTable.Columns["OverDueDays"].Width = Constants.ExportConstants.OverDueDays;
			grdfrm.RootTable.Columns["PaymentTerm"].Width = Constants.ExportConstants.TermConstant;
			grdfrm.RootTable.Columns["DueDate"].Width = Constants.ExportConstants.DateConstant;
			grdfrm.RootTable.Columns["Fcy"].Width = Constants.ExportConstants.FcyConstant;
			grdfrm.RootTable.Columns["InvoiceValue"].Width = Constants.ExportConstants.AmountForReportConstant;
			grdfrm.RootTable.Columns["AdvanceReceipt"].Width = Constants.ExportConstants.AmountForReportConstant;
			grdfrm.RootTable.Columns["NetReceivable"].Width = Constants.ExportConstants.AmountForReportConstant;
			grdfrm.RootTable.Columns["MTons"].Width = Constants.ExportConstants.FclConstant + 20;
			grdfrm.RootTable.Columns["ShippedOnBoard"].Width = Constants.ExportConstants.DateConstant;
			grdfrm.RootTable.Columns["ETADestination"].Width = Constants.ExportConstants.DateConstant;
			grdfrm.RootTable.Columns["InTransitDays"].Width = Constants.ExportConstants.Days;
			grdfrm.RootTable.Columns["ReachedAtDestination"].Width = Constants.ExportConstants.DateConstant + 10;
			grdfrm.RootTable.Columns["IncoTerm"].Width = Constants.ExportConstants.FclConstant;
			grdfrm.RootTable.Columns["Advance"].Width = Constants.ExportConstants.AmountConstant;
			grdfrm.RootTable.Columns["Receipt"].Width = Constants.ExportConstants.AmountConstant;
			grdfrm.RootTable.Columns["DestinationPort"].Width = Constants.ExportConstants.StringConstant;
			grdfrm.RootTable.Columns["InvoiceStatus"].Width = Constants.ExportConstants.StatusConstant;
			grdfrm.RootTable.Columns["MTons"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["MTons"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["MTons"].FormatString = "#,##0.00";
			grdfrm.RootTable.Columns["MTons"].TotalFormatString = "#,##0.00";
			grdfrm.RootTable.Columns["DueDays"].TextAlignment = (TextAlignment)2;
			grdfrm.RootTable.Columns["Advance"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["Advance"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["Advance"].FormatString = "#,##0.000";
			grdfrm.RootTable.Columns["Advance"].TotalFormatString = "#,##0.000";
			grdfrm.RootTable.Columns["InvoiceValue"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["InvoiceValue"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["InvoiceValue"].FormatString = "#,##0.00";
			grdfrm.RootTable.Columns["InvoiceValue"].TotalFormatString = "#,##0.00";
			grdfrm.RootTable.Columns["AdvanceReceipt"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["AdvanceReceipt"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["AdvanceReceipt"].FormatString = "#,##0.00";
			grdfrm.RootTable.Columns["AdvanceReceipt"].TotalFormatString = "#,##0.00";
			grdfrm.RootTable.Columns["NetReceivable"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["NetReceivable"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["NetReceivable"].FormatString = "#,##0.00";
			grdfrm.RootTable.Columns["NetReceivable"].TotalFormatString = "#,##0.00";
			GridEXColumn column = grdfrm.RootTable.Columns["DueDays"];
			GridEXFormatCondition fc = new GridEXFormatCondition(column, (ConditionOperator)0, (object)0);
			fc.TargetColumn = column;
			fc.FormatStyle.FontBold = (TriState)2;
			fc.FormatStyle.ForeColor = Color.Red;
			grdfrm.RootTable.FormatConditions.Add(fc);
			GridEXColumn column2 = grdfrm.RootTable.Columns["OverDueDays"];
			GridEXFormatCondition fc2 = new GridEXFormatCondition(column2, (ConditionOperator)2, (object)0);
			fc2.TargetColumn = column2;
			fc2.FormatStyle.FontBold = (TriState)2;
			fc2.FormatStyle.ForeColor = Color.Red;
			grdfrm.RootTable.FormatConditions.Add(fc2);
			GridEXColumn column3 = grdfrm.RootTable.Columns["DueDate"];
			GridEXFormatCondition fc3 = new GridEXFormatCondition(column3, (ConditionOperator)13, (object)"");
			fc3.TargetColumn = column3;
			fc3.FormatStyle.FontBold = (TriState)2;
			fc3.FormatStyle.ForeColor = Color.Red;
			grdfrm.RootTable.FormatConditions.Add(fc3);
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridCustomerSetting()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdCustomer.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grdCustomer.RootTable.Columns)
			{
				item.HeaderAlignment = (TextAlignment)2;
			}
			grdCustomer.RootTable.Columns["Customer"].Width = Constants.ExportConstants.StringConstant;
			grdCustomer.RootTable.Columns["Fcy"].Width = Constants.ExportConstants.FcyConstant;
			grdCustomer.RootTable.Columns["NetReceivable"].Width = Constants.ExportConstants.AmountConstant + 15;
			grdCustomer.RootTable.Columns["TotalReceivable"].Visible = false;
			grdCustomer.RootTable.Columns["PrcntOfTotal"].Width = Constants.ExportConstants.PercentOfTotal;
			grdCustomer.RootTable.Columns["NetReceivable"].AggregateFunction = (AggregateFunction)2;
			grdCustomer.RootTable.Columns["NetReceivable"].TextAlignment = (TextAlignment)3;
			grdCustomer.RootTable.Columns["NetReceivable"].FormatString = "#,##0.00";
			grdCustomer.RootTable.Columns["NetReceivable"].TotalFormatString = "#,##0.00";
			grdCustomer.RootTable.Columns["TotalReceivable"].AggregateFunction = (AggregateFunction)2;
			grdCustomer.RootTable.Columns["TotalReceivable"].TextAlignment = (TextAlignment)3;
			grdCustomer.RootTable.Columns["TotalReceivable"].FormatString = "#,##0.000";
			grdCustomer.RootTable.Columns["TotalReceivable"].TotalFormatString = "#,##0.000";
			grdCustomer.RootTable.Columns["PrcntOfTotal"].AggregateFunction = (AggregateFunction)2;
			grdCustomer.RootTable.Columns["PrcntOfTotal"].TextAlignment = (TextAlignment)3;
			grdCustomer.RootTable.Columns["PrcntOfTotal"].FormatString = "#,##0.00";
			grdCustomer.RootTable.Columns["PrcntOfTotal"].TotalFormatString = "#,##0.00";
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridBankSetting()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdBank.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grdBank.RootTable.Columns)
			{
				item.HeaderAlignment = (TextAlignment)2;
			}
			grdBank.RootTable.Columns["BankName"].Width = Constants.ExportConstants.StringConstant;
			grdBank.RootTable.Columns["Fcy"].Width = Constants.ExportConstants.FcyConstant;
			grdBank.RootTable.Columns["NetReceivable"].Width = Constants.ExportConstants.AmountConstant + 15;
			grdBank.RootTable.Columns["TotalReceivable"].Visible = false;
			grdBank.RootTable.Columns["PrcntOfTotal"].Width = Constants.ExportConstants.PercentOfTotal;
			grdBank.RootTable.Columns["NetReceivable"].AggregateFunction = (AggregateFunction)2;
			grdBank.RootTable.Columns["NetReceivable"].TextAlignment = (TextAlignment)3;
			grdBank.RootTable.Columns["NetReceivable"].FormatString = "#,##0.00";
			grdBank.RootTable.Columns["NetReceivable"].TotalFormatString = "#,##0.00";
			grdBank.RootTable.Columns["TotalReceivable"].AggregateFunction = (AggregateFunction)2;
			grdBank.RootTable.Columns["TotalReceivable"].TextAlignment = (TextAlignment)3;
			grdBank.RootTable.Columns["TotalReceivable"].FormatString = "#,##0.000";
			grdBank.RootTable.Columns["TotalReceivable"].TotalFormatString = "#,##0.000";
			grdBank.RootTable.Columns["PrcntOfTotal"].AggregateFunction = (AggregateFunction)2;
			grdBank.RootTable.Columns["PrcntOfTotal"].TextAlignment = (TextAlignment)3;
			grdBank.RootTable.Columns["PrcntOfTotal"].FormatString = "#,##0.00";
			grdBank.RootTable.Columns["PrcntOfTotal"].TotalFormatString = "#,##0.00";
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridFcySetting()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdFCY.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grdFCY.RootTable.Columns)
			{
				item.HeaderAlignment = (TextAlignment)2;
			}
			grdFCY.RootTable.Columns["Fcy"].Width = Constants.ExportConstants.FcyConstant;
			grdFCY.RootTable.Columns["NetReceivable"].Width = Constants.ExportConstants.AmountConstant;
			grdFCY.RootTable.Columns["TotalReceivable"].Visible = false;
			grdFCY.RootTable.Columns["PrcntOfTotal"].Width = Constants.ExportConstants.PercentOfTotal;
			grdFCY.RootTable.Columns["NetReceivable"].AggregateFunction = (AggregateFunction)2;
			grdFCY.RootTable.Columns["NetReceivable"].TextAlignment = (TextAlignment)3;
			grdFCY.RootTable.Columns["NetReceivable"].FormatString = "#,##0.00";
			grdFCY.RootTable.Columns["NetReceivable"].TotalFormatString = "#,##0.00";
			grdFCY.RootTable.Columns["TotalReceivable"].AggregateFunction = (AggregateFunction)2;
			grdFCY.RootTable.Columns["TotalReceivable"].TextAlignment = (TextAlignment)3;
			grdFCY.RootTable.Columns["TotalReceivable"].FormatString = "#,##0.00";
			grdFCY.RootTable.Columns["TotalReceivable"].TotalFormatString = "#,##0.00";
			grdFCY.RootTable.Columns["PrcntOfTotal"].AggregateFunction = (AggregateFunction)2;
			grdFCY.RootTable.Columns["PrcntOfTotal"].TextAlignment = (TextAlignment)3;
			grdFCY.RootTable.Columns["PrcntOfTotal"].FormatString = "#,##0.00";
			grdFCY.RootTable.Columns["PrcntOfTotal"].TotalFormatString = "#,##0.00";
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSaleManSetting()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdSaleMan.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grdSaleMan.RootTable.Columns)
			{
				item.HeaderAlignment = (TextAlignment)2;
			}
			grdSaleMan.RootTable.Columns["SaleMan"].Width = Constants.ExportConstants.StringConstant;
			grdSaleMan.RootTable.Columns["Fcy"].Width = Constants.ExportConstants.FcyConstant;
			grdSaleMan.RootTable.Columns["NetReceivable"].Width = Constants.ExportConstants.StringConstant;
			grdSaleMan.RootTable.Columns["TotalReceivable"].Width = Constants.ExportConstants.AmountConstant;
			grdSaleMan.RootTable.Columns["PrcntOfTotal"].Width = Constants.ExportConstants.PercentOfTotal;
			grdSaleMan.RootTable.Columns["NetReceivable"].AggregateFunction = (AggregateFunction)2;
			grdSaleMan.RootTable.Columns["NetReceivable"].TextAlignment = (TextAlignment)3;
			grdSaleMan.RootTable.Columns["NetReceivable"].FormatString = "#,##0.###";
			grdSaleMan.RootTable.Columns["NetReceivable"].TotalFormatString = "#,##0.###";
			grdSaleMan.RootTable.Columns["TotalReceivable"].AggregateFunction = (AggregateFunction)2;
			grdSaleMan.RootTable.Columns["TotalReceivable"].TextAlignment = (TextAlignment)3;
			grdSaleMan.RootTable.Columns["TotalReceivable"].FormatString = "#,##0.###";
			grdSaleMan.RootTable.Columns["TotalReceivable"].TotalFormatString = "#,##0.###";
			grdSaleMan.RootTable.Columns["PrcntOfTotal"].AggregateFunction = (AggregateFunction)2;
			grdSaleMan.RootTable.Columns["PrcntOfTotal"].TextAlignment = (TextAlignment)3;
			grdSaleMan.RootTable.Columns["PrcntOfTotal"].FormatString = "#,##0.###";
			grdSaleMan.RootTable.Columns["PrcntOfTotal"].TotalFormatString = "#,##0.###";
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		Reset();
	}

	private void Reset()
	{
		try
		{
			datFromDate.Focus();
			datFromDate.Value = clsGlobalVariables.ActiveYr.Start_Period;
			datFromDate.Checked = false;
			datToDate.Checked = false;
			((Control)(object)CmbInvoiceNo).Text = string.Empty;
			((Control)(object)CmbCustomerName).Text = string.Empty;
			((Control)(object)cmbContractNo).Text = string.Empty;
			((Control)(object)cmbDeliveryTerm).Text = string.Empty;
			((Control)(object)cmbPaymentTerm).Text = string.Empty;
			((Control)(object)cmbDestinationPort).Text = string.Empty;
			grdfrm.DataSource = null;
			grdCustomer.DataSource = null;
			grdEtaDestination.DataSource = null;
			grdBank.DataSource = null;
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
			else if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			else if (e.Control && e.KeyCode == Keys.N)
			{
				btnNew_Click(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.R)
			{
				btnRefresh_Click(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.P)
			{
				BtnPrint_Click(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.S)
			{
				button1_Click(null, null);
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

	private void BtnPrint_Click(object sender, EventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtMain == null || dtMain.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dtMain, "243-ExportReceivablesDueByDate.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void button1_Click(object sender, EventArgs e)
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

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdfrm).Name);
			if (fs != null)
			{
				grdfrm.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void ExportReceivableByDueDateRegisterA_FormClosed(object sender, FormClosedEventArgs e)
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

	private void grdfrm_FormattingRow(object sender, RowLoadEventArgs e)
	{
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
				GridBind(SortNo);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdfrm_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdfrm.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				string ContractIds = Conversion.ToString(grdfrm.CurrentRow.Cells["ContractIds"].Value);
				int ContractId = 0;
				if (!string.IsNullOrEmpty(ContractIds))
				{
					ContractId = int.Parse(ContractIds.Split(',')[0]);
				}
				if (e.Column.Key == "ContractNo" && ContractId > 0)
				{
					frmContractDetailByContractId obj = new frmContractDetailByContractId(UserAccount);
					obj.ContractId = ContractId;
					obj.Show();
				}
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
			InvoiceNoGetAll();
			CustomerGetAll();
			ContractNoGetAll();
			DeliveryTermFill();
			PaymentTermsFill();
			SeaPortBind();
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
		//IL_0246: Unknown result type (might be due to invalid IL or missing references)
		//IL_0250: Expected O, but got Unknown
		//IL_025c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0266: Expected O, but got Unknown
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_0271: Expected O, but got Unknown
		//IL_0272: Unknown result type (might be due to invalid IL or missing references)
		//IL_027c: Expected O, but got Unknown
		//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02be: Expected O, but got Unknown
		//IL_02e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ea: Expected O, but got Unknown
		//IL_02eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f5: Expected O, but got Unknown
		//IL_030c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0316: Expected O, but got Unknown
		//IL_0317: Unknown result type (might be due to invalid IL or missing references)
		//IL_0321: Expected O, but got Unknown
		//IL_0338: Unknown result type (might be due to invalid IL or missing references)
		//IL_0342: Expected O, but got Unknown
		//IL_0390: Unknown result type (might be due to invalid IL or missing references)
		//IL_039a: Expected O, but got Unknown
		//IL_039b: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a5: Expected O, but got Unknown
		//IL_06d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e1: Expected O, but got Unknown
		//IL_06ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f8: Expected O, but got Unknown
		GridEXLayout grdfrm_DesignTimeLayout = new GridEXLayout();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.ExportReports.ExportReceivableByDueDateRegisterA));
		GridEXLayout grdBank_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdEtaDestination_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdCustomer_DesignTimeLayout = new GridEXLayout();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
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
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
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
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
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
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		GridEXLayout grdFCY_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdSaleMan_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.grdfrm = new GridEX();
		this.panel3 = new System.Windows.Forms.Panel();
		this.grdBank = new GridEX();
		this.grdEtaDestination = new GridEX();
		this.grdCustomer = new GridEX();
		this.panel2 = new System.Windows.Forms.Panel();
		this.chkSkipZero = new System.Windows.Forms.CheckBox();
		this.datFromDate = new System.Windows.Forms.DateTimePicker();
		this.lblInvoiceNo = new System.Windows.Forms.Label();
		this.datToDate = new System.Windows.Forms.DateTimePicker();
		this.CmbInvoiceNo = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.button1 = new System.Windows.Forms.Button();
		this.lblCustomerName = new System.Windows.Forms.Label();
		this.cmbDestinationPort = new UltraCombo();
		this.CmbCustomerName = new UltraCombo();
		this.lblDestinationPort = new System.Windows.Forms.Label();
		this.lblContractNo = new System.Windows.Forms.Label();
		this.cmbPaymentTerm = new UltraCombo();
		this.cmbContractNo = new UltraCombo();
		this.lblPaymentTerm = new System.Windows.Forms.Label();
		this.lblDeliveryTerm = new System.Windows.Forms.Label();
		this.cmbDeliveryTerm = new UltraCombo();
		this.panel5 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label3 = new System.Windows.Forms.Label();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.BtnPrint = new System.Windows.Forms.ToolStripButton();
		this.grdFCY = new GridEX();
		this.grdSaleMan = new GridEX();
		this.panel1.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdfrm).BeginInit();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdBank).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.grdEtaDestination).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.grdCustomer).BeginInit();
		this.panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbInvoiceNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDestinationPort).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPaymentTerm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbContractNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDeliveryTerm).BeginInit();
		this.panel5.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdFCY).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.grdSaleMan).BeginInit();
		base.SuspendLayout();
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel5);
		this.panel1.Controls.Add(this.toolStrip1);
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.grdFCY);
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.grdSaleMan);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1656, 599);
		this.panel1.TabIndex = 0;
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.grdfrm);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 286);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1656, 313);
		this.panel4.TabIndex = 2;
		this.grdfrm.AllowEdit = (InheritableBoolean)2;
		this.grdfrm.AlternatingColors = true;
		grdfrm_DesignTimeLayout.LayoutString = resources.GetString("grdfrm_DesignTimeLayout.LayoutString");
		this.grdfrm.DesignTimeLayout = grdfrm_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdfrm).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdfrm.DynamicFiltering = true;
		this.grdfrm.FilterMode = (FilterMode)2;
		this.grdfrm.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdfrm).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdfrm.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdfrm).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdfrm).Name = "grdfrm";
		((ScrollableBase)this.grdfrm).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Size = new System.Drawing.Size(1656, 313);
		((System.Windows.Forms.Control)(object)this.grdfrm).TabIndex = 1;
		this.grdfrm.TotalRow = (InheritableBoolean)1;
		this.grdfrm.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdfrm).VisualStyle = (VisualStyle)4;
		this.grdfrm.FormattingRow += new RowLoadEventHandler(grdfrm_FormattingRow);
		this.grdfrm.LinkClicked += new ColumnActionEventHandler(grdfrm_LinkClicked);
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.grdBank);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.grdEtaDestination);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.grdCustomer);
		this.panel3.Controls.Add(this.panel2);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 55);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1656, 231);
		this.panel3.TabIndex = 0;
		this.grdBank.AllowEdit = (InheritableBoolean)2;
		this.grdBank.AlternatingColors = true;
		grdBank_DesignTimeLayout.LayoutString = resources.GetString("grdBank_DesignTimeLayout.LayoutString");
		this.grdBank.DesignTimeLayout = grdBank_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdBank).Dock = System.Windows.Forms.DockStyle.Left;
		this.grdBank.DynamicFiltering = true;
		this.grdBank.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdBank).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdBank.GroupByBoxVisible = false;
		this.grdBank.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdBank).Location = new System.Drawing.Point(1280, 0);
		((System.Windows.Forms.Control)(object)this.grdBank).Name = "grdBank";
		((ScrollableBase)this.grdBank).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdBank).Size = new System.Drawing.Size(360, 231);
		((System.Windows.Forms.Control)(object)this.grdBank).TabIndex = 3;
		this.grdBank.TotalRow = (InheritableBoolean)1;
		this.grdBank.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdBank).VisualStyle = (VisualStyle)4;
		this.grdEtaDestination.AllowEdit = (InheritableBoolean)2;
		this.grdEtaDestination.AlternatingColors = true;
		grdEtaDestination_DesignTimeLayout.LayoutString = resources.GetString("grdEtaDestination_DesignTimeLayout.LayoutString");
		this.grdEtaDestination.DesignTimeLayout = grdEtaDestination_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdEtaDestination).Dock = System.Windows.Forms.DockStyle.Left;
		this.grdEtaDestination.DynamicFiltering = true;
		this.grdEtaDestination.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdEtaDestination).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdEtaDestination.GroupByBoxVisible = false;
		this.grdEtaDestination.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdEtaDestination).Location = new System.Drawing.Point(710, 0);
		((System.Windows.Forms.Control)(object)this.grdEtaDestination).Name = "grdEtaDestination";
		((ScrollableBase)this.grdEtaDestination).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdEtaDestination).Size = new System.Drawing.Size(570, 231);
		((System.Windows.Forms.Control)(object)this.grdEtaDestination).TabIndex = 45;
		this.grdEtaDestination.TotalRow = (InheritableBoolean)1;
		this.grdEtaDestination.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdEtaDestination).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdEtaDestination).Click += new System.EventHandler(grdEtaDestination_Click);
		this.grdCustomer.AllowEdit = (InheritableBoolean)2;
		this.grdCustomer.AlternatingColors = true;
		grdCustomer_DesignTimeLayout.LayoutString = resources.GetString("grdCustomer_DesignTimeLayout.LayoutString");
		this.grdCustomer.DesignTimeLayout = grdCustomer_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdCustomer).Dock = System.Windows.Forms.DockStyle.Left;
		this.grdCustomer.DynamicFiltering = true;
		this.grdCustomer.FilterMode = (FilterMode)2;
		this.grdCustomer.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdCustomer).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdCustomer.GroupByBoxVisible = false;
		this.grdCustomer.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdCustomer).Location = new System.Drawing.Point(335, 0);
		((System.Windows.Forms.Control)(object)this.grdCustomer).Name = "grdCustomer";
		((ScrollableBase)this.grdCustomer).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdCustomer).Size = new System.Drawing.Size(375, 231);
		((System.Windows.Forms.Control)(object)this.grdCustomer).TabIndex = 0;
		this.grdCustomer.TotalRow = (InheritableBoolean)1;
		this.grdCustomer.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdCustomer).VisualStyle = (VisualStyle)4;
		this.panel2.Controls.Add(this.chkSkipZero);
		this.panel2.Controls.Add(this.datFromDate);
		this.panel2.Controls.Add(this.lblInvoiceNo);
		this.panel2.Controls.Add(this.datToDate);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbInvoiceNo);
		this.panel2.Controls.Add(this.label1);
		this.panel2.Controls.Add(this.button1);
		this.panel2.Controls.Add(this.lblCustomerName);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDestinationPort);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomerName);
		this.panel2.Controls.Add(this.lblDestinationPort);
		this.panel2.Controls.Add(this.lblContractNo);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPaymentTerm);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbContractNo);
		this.panel2.Controls.Add(this.lblPaymentTerm);
		this.panel2.Controls.Add(this.lblDeliveryTerm);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDeliveryTerm);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(335, 231);
		this.panel2.TabIndex = 44;
		this.chkSkipZero.AutoSize = true;
		this.chkSkipZero.BackColor = System.Drawing.Color.White;
		this.chkSkipZero.Checked = true;
		this.chkSkipZero.CheckState = System.Windows.Forms.CheckState.Checked;
		this.chkSkipZero.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.chkSkipZero.ForeColor = System.Drawing.Color.Black;
		this.chkSkipZero.Location = new System.Drawing.Point(85, 201);
		this.chkSkipZero.Name = "chkSkipZero";
		this.chkSkipZero.Size = new System.Drawing.Size(119, 19);
		this.chkSkipZero.TabIndex = 8;
		this.chkSkipZero.Text = "Skip Zero Balance";
		this.chkSkipZero.UseVisualStyleBackColor = false;
		this.datFromDate.CustomFormat = "dd-MMM-yy";
		this.datFromDate.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datFromDate.Location = new System.Drawing.Point(85, 6);
		this.datFromDate.Name = "datFromDate";
		this.datFromDate.ShowCheckBox = true;
		this.datFromDate.Size = new System.Drawing.Size(123, 22);
		this.datFromDate.TabIndex = 0;
		this.lblInvoiceNo.AutoSize = true;
		this.lblInvoiceNo.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblInvoiceNo.Location = new System.Drawing.Point(5, 34);
		this.lblInvoiceNo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblInvoiceNo.Name = "lblInvoiceNo";
		this.lblInvoiceNo.Size = new System.Drawing.Size(65, 15);
		this.lblInvoiceNo.TabIndex = 24;
		this.lblInvoiceNo.Text = "Invoice No";
		this.datToDate.CustomFormat = "dd-MMM-yy";
		this.datToDate.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datToDate.Location = new System.Drawing.Point(210, 6);
		this.datToDate.Name = "datToDate";
		this.datToDate.ShowCheckBox = true;
		this.datToDate.Size = new System.Drawing.Size(123, 22);
		this.datToDate.TabIndex = 1;
		this.CmbInvoiceNo.AllowNull = (DefaultableBoolean)1;
		this.CmbInvoiceNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbInvoiceNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbInvoiceNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance9).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance9).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance9).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance10;
		((SpecialBoxBase)((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance11).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance11).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance11).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance13).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance15).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance16).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance16;
		((AppearanceBase)appearance17).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbInvoiceNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNo).Location = new System.Drawing.Point(85, 30);
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNo).Name = "CmbInvoiceNo";
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNo).Size = new System.Drawing.Size(247, 26);
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNo).TabIndex = 2;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(5, 9);
		this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(57, 15);
		this.label1.TabIndex = 41;
		this.label1.Text = "Due Date";
		this.button1.BackColor = System.Drawing.Color.Teal;
		this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.button1.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.button1.ForeColor = System.Drawing.Color.White;
		this.button1.Location = new System.Drawing.Point(273, 199);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(59, 25);
		this.button1.TabIndex = 9;
		this.button1.Text = "Show";
		this.button1.UseVisualStyleBackColor = false;
		this.button1.Click += new System.EventHandler(button1_Click);
		this.lblCustomerName.AutoSize = true;
		this.lblCustomerName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCustomerName.Location = new System.Drawing.Point(5, 64);
		this.lblCustomerName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblCustomerName.Name = "lblCustomerName";
		this.lblCustomerName.Size = new System.Drawing.Size(58, 15);
		this.lblCustomerName.TabIndex = 28;
		this.lblCustomerName.Text = "Customer";
		this.cmbDestinationPort.AllowNull = (DefaultableBoolean)1;
		this.cmbDestinationPort.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDestinationPort.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDestinationPort.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Appearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDestinationPort).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance22;
		((SpecialBoxBase)((UltraGridBase)this.cmbDestinationPort).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance23).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance23).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance25).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance27).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance28).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance28).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance28).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance28;
		((AppearanceBase)appearance29).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDestinationPort).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDestinationPort.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDestinationPort).Location = new System.Drawing.Point(85, 172);
		((System.Windows.Forms.Control)(object)this.cmbDestinationPort).Name = "cmbDestinationPort";
		((System.Windows.Forms.Control)(object)this.cmbDestinationPort).Size = new System.Drawing.Size(247, 26);
		((System.Windows.Forms.Control)(object)this.cmbDestinationPort).TabIndex = 7;
		this.CmbCustomerName.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomerName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomerName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomerName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance63).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance63).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance63;
		((AppearanceBase)appearance72).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance72;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance2).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance2).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance2).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance2).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance3;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance5;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance6).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance7).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance7).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance7).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance7;
		((AppearanceBase)appearance32).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomerName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomerName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomerName).Location = new System.Drawing.Point(85, 60);
		((System.Windows.Forms.Control)(object)this.CmbCustomerName).Name = "CmbCustomerName";
		((System.Windows.Forms.Control)(object)this.CmbCustomerName).Size = new System.Drawing.Size(247, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomerName).TabIndex = 3;
		this.lblDestinationPort.AutoSize = true;
		this.lblDestinationPort.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblDestinationPort.Location = new System.Drawing.Point(5, 176);
		this.lblDestinationPort.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblDestinationPort.Name = "lblDestinationPort";
		this.lblDestinationPort.Size = new System.Drawing.Size(71, 15);
		this.lblDestinationPort.TabIndex = 39;
		this.lblDestinationPort.Text = "Destination ";
		this.lblContractNo.AutoSize = true;
		this.lblContractNo.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblContractNo.Location = new System.Drawing.Point(5, 93);
		this.lblContractNo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblContractNo.Name = "lblContractNo";
		this.lblContractNo.Size = new System.Drawing.Size(71, 15);
		this.lblContractNo.TabIndex = 33;
		this.lblContractNo.Text = "Contract No";
		this.cmbPaymentTerm.AllowNull = (DefaultableBoolean)1;
		this.cmbPaymentTerm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPaymentTerm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPaymentTerm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance60).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance60).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance60;
		((AppearanceBase)appearance61).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance61;
		((SpecialBoxBase)((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance62).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance62).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance62).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance64).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance64;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance65).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance67).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance67).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance68).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance68).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance68;
		((AppearanceBase)appearance69).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPaymentTerm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbPaymentTerm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPaymentTerm).Location = new System.Drawing.Point(85, 145);
		((System.Windows.Forms.Control)(object)this.cmbPaymentTerm).Name = "cmbPaymentTerm";
		((System.Windows.Forms.Control)(object)this.cmbPaymentTerm).Size = new System.Drawing.Size(247, 26);
		((System.Windows.Forms.Control)(object)this.cmbPaymentTerm).TabIndex = 6;
		this.cmbContractNo.AllowNull = (DefaultableBoolean)1;
		this.cmbContractNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbContractNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbContractNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbContractNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.cmbContractNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbContractNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbContractNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbContractNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance49;
		((SpecialBoxBase)((UltraGridBase)this.cmbContractNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance50).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance50).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance50).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbContractNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.cmbContractNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbContractNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance51).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbContractNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance51;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance52).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbContractNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.cmbContractNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbContractNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbContractNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance54).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbContractNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.cmbContractNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbContractNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance55).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance55).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbContractNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance55;
		((AppearanceBase)appearance56).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbContractNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.cmbContractNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbContractNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance57).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbContractNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.cmbContractNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbContractNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.cmbContractNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbContractNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbContractNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbContractNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbContractNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbContractNo).Location = new System.Drawing.Point(85, 89);
		((System.Windows.Forms.Control)(object)this.cmbContractNo).Name = "cmbContractNo";
		((System.Windows.Forms.Control)(object)this.cmbContractNo).Size = new System.Drawing.Size(247, 26);
		((System.Windows.Forms.Control)(object)this.cmbContractNo).TabIndex = 4;
		this.lblPaymentTerm.AutoSize = true;
		this.lblPaymentTerm.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblPaymentTerm.Location = new System.Drawing.Point(5, 149);
		this.lblPaymentTerm.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblPaymentTerm.Name = "lblPaymentTerm";
		this.lblPaymentTerm.Size = new System.Drawing.Size(84, 15);
		this.lblPaymentTerm.TabIndex = 37;
		this.lblPaymentTerm.Text = "Payment Term";
		this.lblDeliveryTerm.AutoSize = true;
		this.lblDeliveryTerm.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblDeliveryTerm.Location = new System.Drawing.Point(5, 121);
		this.lblDeliveryTerm.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblDeliveryTerm.Name = "lblDeliveryTerm";
		this.lblDeliveryTerm.Size = new System.Drawing.Size(80, 15);
		this.lblDeliveryTerm.TabIndex = 35;
		this.lblDeliveryTerm.Text = "Delivery Term";
		this.cmbDeliveryTerm.AllowNull = (DefaultableBoolean)1;
		this.cmbDeliveryTerm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDeliveryTerm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDeliveryTerm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDeliveryTerm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbDeliveryTerm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDeliveryTerm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance36).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance36).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDeliveryTerm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDeliveryTerm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance37;
		((SpecialBoxBase)((UltraGridBase)this.cmbDeliveryTerm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance38).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance38).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance38).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDeliveryTerm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.cmbDeliveryTerm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDeliveryTerm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDeliveryTerm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance39;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance40).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDeliveryTerm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.cmbDeliveryTerm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDeliveryTerm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDeliveryTerm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance42).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDeliveryTerm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbDeliveryTerm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDeliveryTerm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance43).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance43).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance43).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance43).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDeliveryTerm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDeliveryTerm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.cmbDeliveryTerm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDeliveryTerm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDeliveryTerm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.cmbDeliveryTerm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDeliveryTerm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.cmbDeliveryTerm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDeliveryTerm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDeliveryTerm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDeliveryTerm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDeliveryTerm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDeliveryTerm).Location = new System.Drawing.Point(85, 117);
		((System.Windows.Forms.Control)(object)this.cmbDeliveryTerm).Name = "cmbDeliveryTerm";
		((System.Windows.Forms.Control)(object)this.cmbDeliveryTerm).Size = new System.Drawing.Size(247, 26);
		((System.Windows.Forms.Control)(object)this.cmbDeliveryTerm).TabIndex = 5;
		this.panel5.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel5.Controls.Add(this.ctrlGrdBar1);
		this.panel5.Controls.Add(this.label3);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 27);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1656, 28);
		this.panel5.TabIndex = 21;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1615, 3);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdfrm;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 22;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.White;
		this.label3.Location = new System.Drawing.Point(8, 4);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(271, 19);
		this.label3.TabIndex = 1;
		this.label3.Text = "Export Receivables Due By Date";
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnNew, this.btnRefresh, this.BtnPrint });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1656, 27);
		this.toolStrip1.TabIndex = 22;
		this.toolStrip1.Text = "toolStrip1";
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(59, 24);
		this.btnNew.Text = "&New";
		this.btnNew.ToolTipText = "Ctrl_N";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "&Refresh";
		this.btnRefresh.ToolTipText = "Alt_R";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.BtnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.BtnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnPrint.Name = "BtnPrint";
		this.BtnPrint.Size = new System.Drawing.Size(92, 24);
		this.BtnPrint.Text = "243-Print";
		this.BtnPrint.ToolTipText = "Ctrl_P";
		this.BtnPrint.Click += new System.EventHandler(BtnPrint_Click);
		this.grdFCY.AllowEdit = (InheritableBoolean)2;
		this.grdFCY.AlternatingColors = true;
		grdFCY_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><CellLayoutMode>UseColumnSets</CellLayoutMode><ColumnSetRowCount>1</ColumnSetRowCount><GroupCondition /><HeaderLines>2</HeaderLines></RootTable></GridEXLayoutData>";
		this.grdFCY.DesignTimeLayout = grdFCY_DesignTimeLayout;
		this.grdFCY.DynamicFiltering = true;
		this.grdFCY.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdFCY).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdFCY.GroupByBoxVisible = false;
		this.grdFCY.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdFCY).Location = new System.Drawing.Point(1115, 305);
		((System.Windows.Forms.Control)(object)this.grdFCY).Name = "grdFCY";
		((ScrollableBase)this.grdFCY).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdFCY).Size = new System.Drawing.Size(229, 125);
		((System.Windows.Forms.Control)(object)this.grdFCY).TabIndex = 4;
		this.grdFCY.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdFCY).VisualStyle = (VisualStyle)4;
		this.grdSaleMan.AllowEdit = (InheritableBoolean)2;
		this.grdSaleMan.AlternatingColors = true;
		grdSaleMan_DesignTimeLayout.LayoutString = resources.GetString("grdSaleMan_DesignTimeLayout.LayoutString");
		this.grdSaleMan.DesignTimeLayout = grdSaleMan_DesignTimeLayout;
		this.grdSaleMan.DynamicFiltering = true;
		this.grdSaleMan.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdSaleMan).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdSaleMan.GroupByBoxVisible = false;
		this.grdSaleMan.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdSaleMan).Location = new System.Drawing.Point(883, 305);
		((System.Windows.Forms.Control)(object)this.grdSaleMan).Name = "grdSaleMan";
		((ScrollableBase)this.grdSaleMan).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdSaleMan).Size = new System.Drawing.Size(228, 125);
		((System.Windows.Forms.Control)(object)this.grdSaleMan).TabIndex = 6;
		this.grdSaleMan.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdSaleMan).VisualStyle = (VisualStyle)4;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1656, 599);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "ExportReceivableByDueDateRegisterA";
		base.ShowIcon = false;
		this.Text = "Export Receivable Register by Due Date";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(ExportReceivableByDueDateRegisterA_FormClosed);
		base.Load += new System.EventHandler(frmExportShipingLineBookingRpt_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmExportShipingLineBookingRpt_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.panel4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdfrm).EndInit();
		this.panel3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdBank).EndInit();
		((System.ComponentModel.ISupportInitialize)this.grdEtaDestination).EndInit();
		((System.ComponentModel.ISupportInitialize)this.grdCustomer).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbInvoiceNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDestinationPort).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPaymentTerm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbContractNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDeliveryTerm).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdFCY).EndInit();
		((System.ComponentModel.ISupportInitialize)this.grdSaleMan).EndInit();
		base.ResumeLayout(false);
	}
}
