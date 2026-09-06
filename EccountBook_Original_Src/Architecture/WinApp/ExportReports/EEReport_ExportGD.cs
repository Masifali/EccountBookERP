using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Export;
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

public class EEReport_ExportGD : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtBindCombo = new DataTable();

	public DateTime FromDate;

	public DateTime ToDate;

	private DataTable dtStockSum = new DataTable();

	private DataTable dtSchedule = new DataTable();

	private DataTable dtSalesTerm = new DataTable();

	private DataTable dtDateCombo = new DataTable();

	private IContainer components;

	private Panel panel1;

	private Panel panel4;

	private Panel panel3;

	private Panel panel2;

	internal GridEX grdfrm;

	private ToolStrip toolStrip1;

	private ToolStripButton btnNew;

	private Label label1;

	private Button btnshow;

	private CtrlGrdBar ctrlGrdBar1;

	private Panel panel5;

	private Label label23;

	private ToolStripButton btnPrint;

	private Panel panel6;

	private Label label2;

	private GroupBox groupBox1;

	private ToolStripButton btnshortcut;

	private DateTimePicker gdtodate;

	private Label label4;

	private DateTimePicker gdfrmdate;

	private Label label3;

	internal UltraCombo cmbperemeter;

	private ToolStripButton btnPrint567_1;

	internal UltraCombo CmbBank;

	private Label label5;

	private ToolStripButton Btnreferesh;

	private RadioButton ChkDetail;

	private RadioButton ChkSummary;

	internal UltraCombo cmbrealizedbank;

	private Label label6;

	private ToolStripButton btnprint_02;

	public EEReport_ExportGD(UserAccount userAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = userAccount;
	}

	private void frmStockReport_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(980, 550);
			if (dtSchedule.Columns.Count == 0)
			{
				dtSchedule.Columns.Add("Id", typeof(string));
				dtSchedule.Columns.Add("Name", typeof(string));
			}
			if (dtSalesTerm.Columns.Count == 0)
			{
				dtSalesTerm.Columns.Add("Id", typeof(string));
				dtSalesTerm.Columns.Add("Name", typeof(string));
			}
			if (dtDateCombo.Columns.Count == 0)
			{
				dtDateCombo.Columns.Add("MonthId", typeof(int));
				dtDateCombo.Columns.Add("YearId", typeof(int));
				dtDateCombo.Columns.Add("Month");
			}
			ParameterFill();
			RealizedBankFill();
			BankFill();
			cmbperemeter.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BankFill()
	{
		try
		{
			DataTable dt = GDBreakUpHeader.GetBanksForDropDownFromGDBreakUps(UserAccount.OrganizationId, UserAccount.CompanyId);
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbBank, "BankId", "BankName", "GD Bank", ZeroIndex: false);
				((UltraGridBase)CmbBank).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RealizedBankFill()
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		try
		{
			DataTable dtMainType = new DataTable();
			dtMainType.Columns.Add("Id");
			dtMainType.Columns.Add("name");
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			dtBindCombo = GDBreakUpHeader.RealizedBankFromDropdown(obj);
			if (dtBindCombo.Rows.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < dtBindCombo.Rows.Count; i++)
			{
				if (Conversion.ToString(dtBindCombo.Rows[i]["Activity"]) == "RealizedBank_Gd")
				{
					dtMainType.Rows.Add(dtBindCombo.Rows[i]["Id"], dtBindCombo.Rows[i]["ReferenceName"]);
				}
			}
			if (dtMainType.Rows.Count > 0)
			{
				DDL.BindDDL(dtMainType, cmbrealizedbank, "Id", "name", "Realized Bank", ZeroIndex: false);
				((UltraGridBase)cmbrealizedbank).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
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
			((UltraGridBase)cmbperemeter).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)cmbperemeter).Rows[1].Activate();
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
				gdfrmdate.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 2)
			{
				gdfrmdate.Value = DateTime.Today.AddDays(-7.0);
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 3)
			{
				DateTime now = DateTime.UtcNow;
				DateTime first = now.Date.AddDays(-(now.Date.Day - 1));
				gdfrmdate.Value = first;
				gdtodate.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 4)
			{
				int year = DateTime.Now.Year;
				DateTime first2 = new DateTime(year, 1, 1);
				gdfrmdate.Value = first2;
				gdtodate.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 5)
			{
				gdfrmdate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			}
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
			CommonServices.GetMonths();
			dtDateCombo.Clear();
			DateTime financialStartDate = clsGlobalVariables.ActiveYr.Start_Period;
			int year = ((financialStartDate.Month >= 7) ? financialStartDate.Year : (financialStartDate.Year - 1));
			DateTime dateTime = new DateTime(year, 7, 1);
			DateTime newEndDate = new DateTime(year + 1, 6, 30);
			List<DateTime> monthsList = new List<DateTime>();
			DateTime date = dateTime;
			while (date <= newEndDate)
			{
				monthsList.Add(date);
				date = date.AddMonths(1);
			}
			foreach (DateTime month in monthsList)
			{
				dtDateCombo.Rows.Add(month.Month, month.Year, month.ToString("MMM yyyy"));
			}
			grdfrm.RootTable.Columns["FEReturnMonth"].ColumnType = (ColumnType)0;
			grdfrm.RootTable.Columns["FEReturnMonth"].HasValueList = true;
			grdfrm.RootTable.Columns["FEReturnMonth"].LimitToList = true;
			grdfrm.RootTable.Columns["FEReturnMonth"].EditType = (EditType)4;
			grdfrm.RootTable.Columns["FEReturnMonth"].Caption = "FEReturnMonth";
			grdfrm.RootTable.Columns["FEReturnMonth"].ValueList.PopulateValueList((IEnumerable)dtDateCombo.DefaultView, "MonthId", "Month");
			grdfrm.RootTable.Columns["FEReturnMonth"].Width = 120;
			grdfrm.RootTable.Columns["Schedule"].ColumnType = (ColumnType)0;
			grdfrm.RootTable.Columns["Schedule"].HasValueList = true;
			grdfrm.RootTable.Columns["Schedule"].LimitToList = false;
			grdfrm.RootTable.Columns["Schedule"].EditType = (EditType)4;
			grdfrm.RootTable.Columns["Schedule"].Caption = "Schedule";
			grdfrm.RootTable.Columns["Schedule"].ValueList.PopulateValueList((IEnumerable)dtSchedule.DefaultView, "Id", "Name");
			grdfrm.RootTable.Columns["Schedule"].Width = 120;
			grdfrm.RootTable.Columns["SalesTerm"].ColumnType = (ColumnType)0;
			grdfrm.RootTable.Columns["SalesTerm"].HasValueList = true;
			grdfrm.RootTable.Columns["SalesTerm"].LimitToList = false;
			grdfrm.RootTable.Columns["SalesTerm"].EditType = (EditType)4;
			grdfrm.RootTable.Columns["SalesTerm"].Caption = "SalesTerm";
			grdfrm.RootTable.Columns["SalesTerm"].ValueList.PopulateValueList((IEnumerable)dtSalesTerm.DefaultView, "Id", "Name");
			grdfrm.RootTable.Columns["SalesTerm"].Width = 120;
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
			if (ChkDetail.Checked)
			{
				GridBind();
			}
			else
			{
				GridBindSummary();
			}
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
			obj.FromDate = Conversion.ToDateTime((object)gdfrmdate.Value);
			obj.ToDate = Conversion.ToDateTime((object)gdtodate.Value);
			obj.BankGlDrAc = Conversion.ToInt(CmbBank.Value);
			obj.ExImFCBankReceiptsId = Conversion.ToInt(cmbrealizedbank.Value);
			obj.Activity = "Detail";
			dtStockSum = ExportReports.EEReport_ExportGD(obj);
			if (dtStockSum.Rows.Count > 0)
			{
				DataTable dtItemStockSummary = new DataTable();
				dtItemStockSummary.Columns.Add("Id", typeof(int));
				dtItemStockSummary.Columns.Add("FcyBankReceiptId", typeof(int));
				dtItemStockSummary.Columns.Add("Sr.No.", typeof(string));
				dtItemStockSummary.Columns.Add("GdRefDocTypeId", typeof(int));
				dtItemStockSummary.Columns.Add("GDId", typeof(int));
				dtItemStockSummary.Columns.Add("GdNo", typeof(string));
				dtItemStockSummary.Columns.Add("Consignee", typeof(string));
				dtItemStockSummary.Columns.Add("Commodity", typeof(string));
				dtItemStockSummary.Columns.Add("HsCode", typeof(string));
				dtItemStockSummary.Columns.Add("ContractNo", typeof(string));
				dtItemStockSummary.Columns.Add("ContractDate", typeof(DateTime));
				dtItemStockSummary.Columns.Add("ContractCurrency", typeof(string));
				dtItemStockSummary.Columns.Add("ContractAmount", typeof(double));
				dtItemStockSummary.Columns.Add("DateOfShipment", typeof(DateTime));
				dtItemStockSummary.Columns.Add("DateOfNegotion", typeof(DateTime));
				dtItemStockSummary.Columns.Add("RealizedCurrency", typeof(string));
				dtItemStockSummary.Columns.Add("RealizedAmount", typeof(double));
				dtItemStockSummary.Columns.Add("FTT/Comm", typeof(double));
				dtItemStockSummary.Columns.Add("ExchangeRate", typeof(double));
				dtItemStockSummary.Columns.Add("RealizedAmountInPKR", typeof(double));
				dtItemStockSummary.Columns.Add("DateofRealization", typeof(DateTime));
				dtItemStockSummary.Columns.Add("MultiCurrencyId", typeof(int));
				dtItemStockSummary.Columns.Add("EEId", typeof(int));
				dtItemStockSummary.Columns.Add("FEReturnPageNo", typeof(int));
				dtItemStockSummary.Columns.Add("ITRSNo", typeof(int));
				dtItemStockSummary.Columns.Add("FEReturnMonth", typeof(string));
				dtItemStockSummary.Columns.Add("Schedule", typeof(string));
				dtItemStockSummary.Columns.Add("SalesTerm", typeof(string));
				dtItemStockSummary.Columns.Add("Remarks", typeof(string));
				dtItemStockSummary.Columns.Add("EERemarks", typeof(string));
				dtSalesTerm.Clear();
				dtSchedule.Clear();
				foreach (DataRow row in dtStockSum.Rows)
				{
					dtItemStockSummary.Rows.Add(Conversion.ToInt(row["Id"]), Conversion.ToInt(row["FcyBankReceiptId"]), Conversion.ToString(row["RwNo"]), Conversion.ToInt(row["GdRefDocTypeId"]), Conversion.ToInt(row["GDId"]), Conversion.ToString(row["GdNo"]), Conversion.ToString(row["Consignee"]), Conversion.ToString(row["Commodity"]), Conversion.ToString(row["HsCode"]), Conversion.ToString(row["ContractNo"]), Conversion.ToDateTime(row["ContractDate"]), Conversion.ToString(row["ContractCurrency"]), Conversion.ToDouble(row["ContractAmount"]), Conversion.ToDateTime(row["DateOfShipment"]), Conversion.ToDateTime(row["DateOfNegotion"]), Conversion.ToString(row["RealizedCurrency"]), Conversion.ToDouble(row["RealizedAmount"]), Conversion.ToDouble(row["AgencyComm"]), Conversion.ToDouble(row["ExchangeRate"]), Conversion.ToDouble(row["RealizedAmountInPKR"]), Conversion.ToDateTime(row["DateofRealization"]), Conversion.ToInt(row["MultiCurrencyId"]), Conversion.ToInt(row["EEId"]), Conversion.ToInt(row["FEReturnPageNo"]), Conversion.ToInt(row["ITRSNo"]), Conversion.ToString(row["FEReturnMonth"]), Conversion.ToString(row["Schedule"]), Conversion.ToString(row["SalesTerm"]), Conversion.ToString(row["Remarks"]), Conversion.ToString(row["EERemarks"]));
					if (!dtSalesTerm.AsEnumerable().Any((DataRow rt) => rt.Field<string>("Name") == Conversion.ToString(row["SalesTerm"])))
					{
						dtSalesTerm.Rows.Add(Conversion.ToString(row["SalesTerm"]), Conversion.ToString(row["SalesTerm"]));
					}
					if (!dtSchedule.AsEnumerable().Any((DataRow rs) => rs.Field<string>("Name") == Conversion.ToString(row["Schedule"])))
					{
						dtSchedule.Rows.Add(Conversion.ToString(row["Schedule"]), Conversion.ToString(row["Schedule"]));
					}
				}
				((Control)(object)grdfrm).Name = "EE_StockDetail";
				grdfrm.DataSource = dtItemStockSummary;
				grdfrm.RetrieveStructure();
				GridSettings();
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

	private void GridSettings()
	{
		//IL_0a90: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdfrm.AllowEdit = (InheritableBoolean)1;
			GridComboBind();
			grdfrm.RootTable.Columns["Id"].Visible = false;
			grdfrm.RootTable.Columns["EEId"].Visible = false;
			grdfrm.RootTable.Columns["FcyBankReceiptId"].Visible = false;
			grdfrm.RootTable.Columns["GdRefDocTypeId"].Visible = false;
			grdfrm.RootTable.Columns["GDId"].Visible = false;
			grdfrm.RootTable.Columns["MultiCurrencyId"].Visible = false;
			grdfrm.RootTable.Columns["FTT/Comm"].Visible = false;
			grdfrm.RootTable.Columns["Id"].EditType = (EditType)0;
			grdfrm.RootTable.Columns["FcyBankReceiptId"].EditType = (EditType)0;
			grdfrm.RootTable.Columns["Sr.No."].EditType = (EditType)0;
			grdfrm.RootTable.Columns["GdRefDocTypeId"].EditType = (EditType)0;
			grdfrm.RootTable.Columns["GDId"].EditType = (EditType)0;
			grdfrm.RootTable.Columns["GdNo"].EditType = (EditType)0;
			grdfrm.RootTable.Columns["Consignee"].EditType = (EditType)0;
			grdfrm.RootTable.Columns["Commodity"].EditType = (EditType)0;
			grdfrm.RootTable.Columns["HsCode"].EditType = (EditType)0;
			grdfrm.RootTable.Columns["ContractNo"].EditType = (EditType)0;
			grdfrm.RootTable.Columns["ContractDate"].EditType = (EditType)0;
			grdfrm.RootTable.Columns["ContractCurrency"].EditType = (EditType)0;
			grdfrm.RootTable.Columns["ContractAmount"].EditType = (EditType)0;
			grdfrm.RootTable.Columns["DateOfShipment"].EditType = (EditType)0;
			grdfrm.RootTable.Columns["DateOfNegotion"].EditType = (EditType)0;
			grdfrm.RootTable.Columns["RealizedCurrency"].EditType = (EditType)0;
			grdfrm.RootTable.Columns["RealizedAmount"].EditType = (EditType)0;
			grdfrm.RootTable.Columns["ExchangeRate"].EditType = (EditType)0;
			grdfrm.RootTable.Columns["RealizedAmountInPKR"].EditType = (EditType)0;
			grdfrm.RootTable.Columns["DateofRealization"].EditType = (EditType)0;
			grdfrm.RootTable.Columns["MultiCurrencyId"].EditType = (EditType)0;
			grdfrm.RootTable.Columns["FTT/Comm"].EditType = (EditType)0;
			grdfrm.RootTable.Columns["Remarks"].EditType = (EditType)0;
			grdfrm.RootTable.Columns["EEId"].EditType = (EditType)0;
			grdfrm.RootTable.Columns["FEReturnPageNo"].EditType = (EditType)1;
			grdfrm.RootTable.Columns["ITRSNo"].EditType = (EditType)1;
			grdfrm.RootTable.Columns["FEReturnMonth"].EditType = (EditType)4;
			grdfrm.RootTable.Columns["Schedule"].EditType = (EditType)4;
			grdfrm.RootTable.Columns["SalesTerm"].EditType = (EditType)4;
			grdfrm.RootTable.Columns["EERemarks"].EditType = (EditType)1;
			grdfrm.RootTable.Columns["Id"].FilterEditType = (FilterEditType)1;
			grdfrm.RootTable.Columns["FcyBankReceiptId"].FilterEditType = (FilterEditType)1;
			grdfrm.RootTable.Columns["Sr.No."].FilterEditType = (FilterEditType)1;
			grdfrm.RootTable.Columns["GdRefDocTypeId"].FilterEditType = (FilterEditType)1;
			grdfrm.RootTable.Columns["GDId"].FilterEditType = (FilterEditType)1;
			grdfrm.RootTable.Columns["GdNo"].FilterEditType = (FilterEditType)1;
			grdfrm.RootTable.Columns["Consignee"].FilterEditType = (FilterEditType)1;
			grdfrm.RootTable.Columns["Commodity"].FilterEditType = (FilterEditType)1;
			grdfrm.RootTable.Columns["HsCode"].FilterEditType = (FilterEditType)1;
			grdfrm.RootTable.Columns["ContractNo"].FilterEditType = (FilterEditType)1;
			grdfrm.RootTable.Columns["ContractDate"].FilterEditType = (FilterEditType)6;
			grdfrm.RootTable.Columns["ContractCurrency"].FilterEditType = (FilterEditType)1;
			grdfrm.RootTable.Columns["ContractAmount"].FilterEditType = (FilterEditType)1;
			grdfrm.RootTable.Columns["DateOfShipment"].FilterEditType = (FilterEditType)6;
			grdfrm.RootTable.Columns["DateOfNegotion"].FilterEditType = (FilterEditType)6;
			grdfrm.RootTable.Columns["RealizedCurrency"].FilterEditType = (FilterEditType)1;
			grdfrm.RootTable.Columns["RealizedAmount"].FilterEditType = (FilterEditType)1;
			grdfrm.RootTable.Columns["ExchangeRate"].FilterEditType = (FilterEditType)1;
			grdfrm.RootTable.Columns["RealizedAmountInPKR"].FilterEditType = (FilterEditType)1;
			grdfrm.RootTable.Columns["DateofRealization"].FilterEditType = (FilterEditType)6;
			grdfrm.RootTable.Columns["MultiCurrencyId"].FilterEditType = (FilterEditType)1;
			grdfrm.RootTable.Columns["FTT/Comm"].FilterEditType = (FilterEditType)1;
			grdfrm.RootTable.Columns["Remarks"].FilterEditType = (FilterEditType)1;
			grdfrm.RootTable.Columns["EEId"].FilterEditType = (FilterEditType)1;
			grdfrm.RootTable.Columns["FEReturnPageNo"].FilterEditType = (FilterEditType)1;
			grdfrm.RootTable.Columns["ITRSNo"].FilterEditType = (FilterEditType)1;
			grdfrm.RootTable.Columns["FEReturnMonth"].FilterEditType = (FilterEditType)1;
			grdfrm.RootTable.Columns["Schedule"].FilterEditType = (FilterEditType)1;
			grdfrm.RootTable.Columns["SalesTerm"].FilterEditType = (FilterEditType)1;
			grdfrm.RootTable.Columns["EERemarks"].FilterEditType = (FilterEditType)1;
			grdfrm.RootTable.Columns["RealizedAmountInPKR"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["RealizedAmountInPKR"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["RealizedAmountInPKR"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdfrm.RootTable.Columns["RealizedAmountInPKR"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdfrm.RootTable.Columns["ContractAmount"].FormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdfrm.RootTable.Columns["ExchangeRate"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["ExchangeRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdfrm.RootTable.Columns["FTT/Comm"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["FTT/Comm"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["FTT/Comm"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdfrm.RootTable.Columns["FTT/Comm"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdfrm.RootTable.Columns["RealizedAmount"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["RealizedAmount"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["RealizedAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdfrm.RootTable.Columns["RealizedAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			foreach (GridEXColumn item in (JanusCollectionBase)grdfrm.RootTable.Columns)
			{
				item.HeaderStyle.TextAlignment = (TextAlignment)2;
			}
			grdfrm.RootTable.Columns["DateOfNegotion"].FormatString = "dd-MMM-yy";
			grdfrm.RootTable.HeaderLines = 2;
			grdfrm.RootTable.Columns["Sr.No."].Width = 45;
			grdfrm.RootTable.Columns["GdNo"].Width = 192;
			grdfrm.RootTable.Columns["Consignee"].Width = 150;
			grdfrm.RootTable.Columns["Commodity"].Width = 120;
			grdfrm.RootTable.Columns["HsCode"].Width = 100;
			grdfrm.RootTable.Columns["ContractNo"].Width = 110;
			grdfrm.RootTable.Columns["ContractDate"].Width = 85;
			grdfrm.RootTable.Columns["ContractCurrency"].Width = 77;
			grdfrm.RootTable.Columns["ContractAmount"].Width = 100;
			grdfrm.RootTable.Columns["DateOfShipment"].Width = 80;
			grdfrm.RootTable.Columns["DateOfNegotion"].Width = 80;
			grdfrm.RootTable.Columns["RealizedCurrency"].Width = 73;
			grdfrm.RootTable.Columns["RealizedAmount"].Width = 100;
			grdfrm.RootTable.Columns["FTT/Comm"].Width = 90;
			grdfrm.RootTable.Columns["ExchangeRate"].Width = 75;
			grdfrm.RootTable.Columns["RealizedAmountInPKR"].Width = 110;
			grdfrm.RootTable.Columns["DateofRealization"].Width = 85;
			grdfrm.RootTable.Columns["FEReturnPageNo"].Width = 75;
			grdfrm.RootTable.Columns["ITRSNo"].Width = 70;
			grdfrm.RootTable.Columns["FEReturnMonth"].Width = 75;
			grdfrm.RootTable.Columns["Schedule"].Width = 80;
			grdfrm.RootTable.Columns["SalesTerm"].Width = 90;
			grdfrm.RootTable.Columns["Remarks"].Width = 120;
			grdfrm.RootTable.Columns["EERemarks"].Width = 150;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridBindSummary()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.FromDate = Conversion.ToDateTime((object)gdfrmdate.Value);
			obj.ToDate = Conversion.ToDateTime((object)gdtodate.Value);
			obj.BankGlDrAc = Conversion.ToInt(CmbBank.Value);
			obj.ExImFCBankReceiptsId = Conversion.ToInt(cmbrealizedbank.Value);
			obj.Activity = "Summary";
			dtStockSum = ExportReports.EEReport_ExportGD(obj);
			if (dtStockSum.Rows.Count > 0)
			{
				DataTable dtItemStockSummary = new DataTable();
				dtItemStockSummary.Columns.Add("GdRefDocTypeId", typeof(int));
				dtItemStockSummary.Columns.Add("GDId", typeof(int));
				dtItemStockSummary.Columns.Add("GdNo", typeof(string));
				dtItemStockSummary.Columns.Add("Consignee", typeof(string));
				dtItemStockSummary.Columns.Add("Commodity", typeof(string));
				dtItemStockSummary.Columns.Add("HsCode", typeof(string));
				dtItemStockSummary.Columns.Add("ContractNo", typeof(string));
				dtItemStockSummary.Columns.Add("ContractDate", typeof(DateTime));
				dtItemStockSummary.Columns.Add("ContractCurrency", typeof(string));
				dtItemStockSummary.Columns.Add("ContractAmount", typeof(double));
				dtItemStockSummary.Columns.Add("DateOfShipment", typeof(DateTime));
				dtItemStockSummary.Columns.Add("DateOfNegotion", typeof(DateTime));
				dtItemStockSummary.Columns.Add("RealizedCurrency", typeof(string));
				dtItemStockSummary.Columns.Add("RealizedAmount", typeof(double));
				dtItemStockSummary.Columns.Add("FTT/Comm", typeof(double));
				dtItemStockSummary.Columns.Add("ExchangeRate", typeof(double));
				dtItemStockSummary.Columns.Add("RealizedAmountInPKR", typeof(double));
				dtItemStockSummary.Columns.Add("DateofRealization", typeof(DateTime));
				dtItemStockSummary.Columns.Add("MultiCurrencyId", typeof(int));
				dtItemStockSummary.Columns.Add("FEReturnPageNo", typeof(int));
				dtItemStockSummary.Columns.Add("ITRSNo", typeof(int));
				dtItemStockSummary.Columns.Add("FEReturnMonth", typeof(string));
				dtItemStockSummary.Columns.Add("Schedule", typeof(string));
				dtItemStockSummary.Columns.Add("SalesTerm", typeof(string));
				dtItemStockSummary.Columns.Add("Remarks", typeof(string));
				dtItemStockSummary.Columns.Add("EERemarks", typeof(string));
				dtSalesTerm.Clear();
				dtSchedule.Clear();
				foreach (DataRow row in dtStockSum.Rows)
				{
					dtItemStockSummary.Rows.Add(Conversion.ToInt(row["GdRefDocTypeId"]), Conversion.ToInt(row["GDId"]), Conversion.ToString(row["GdNo"]), Conversion.ToString(row["Consignee"]), Conversion.ToString(row["Commodity"]), Conversion.ToString(row["HsCode"]), Conversion.ToString(row["ContractNo"]), Conversion.ToDateTime(row["ContractDate"]), Conversion.ToString(row["ContractCurrency"]), Conversion.ToDouble(row["ContractAmount"]), Conversion.ToDateTime(row["DateOfShipment"]), Conversion.ToDateTime(row["DateOfNegotion"]), Conversion.ToString(row["RealizedCurrency"]), Conversion.ToDouble(row["RealizedAmount"]), Conversion.ToDouble(row["AgencyComm"]), Conversion.ToDouble(row["ExchangeRate"]), Conversion.ToDouble(row["RealizedAmountInPKR"]), Conversion.ToDateTime(row["DateofRealization"]), Conversion.ToInt(row["MultiCurrencyId"]), Conversion.ToInt(row["FEReturnPageNo"]), Conversion.ToInt(row["ITRSNo"]), Conversion.ToString(row["FEReturnMonth"]), Conversion.ToString(row["Schedule"]), Conversion.ToString(row["SalesTerm"]), Conversion.ToString(row["Remarks"]), Conversion.ToString(row["EERemarks"]));
					if (!dtSalesTerm.AsEnumerable().Any((DataRow rt) => rt.Field<string>("Name") == Conversion.ToString(row["SalesTerm"])))
					{
						dtSalesTerm.Rows.Add(Conversion.ToString(row["SalesTerm"]), Conversion.ToString(row["SalesTerm"]));
					}
					if (!dtSchedule.AsEnumerable().Any((DataRow rs) => rs.Field<string>("Name") == Conversion.ToString(row["Schedule"])))
					{
						dtSchedule.Rows.Add(Conversion.ToString(row["Schedule"]), Conversion.ToString(row["Schedule"]));
					}
				}
				((Control)(object)grdfrm).Name = "EE_StockSummary";
				grdfrm.DataSource = dtItemStockSummary;
				grdfrm.RetrieveStructure();
				GridSettingsSunnary();
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

	private void GridSettingsSunnary()
	{
		//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdfrm.AllowEdit = (InheritableBoolean)2;
			grdfrm.RootTable.Columns["GdRefDocTypeId"].Visible = false;
			grdfrm.RootTable.Columns["GDId"].Visible = false;
			grdfrm.RootTable.Columns["MultiCurrencyId"].Visible = false;
			grdfrm.RootTable.Columns["FTT/Comm"].Visible = false;
			grdfrm.RootTable.Columns["RealizedAmountInPKR"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["RealizedAmountInPKR"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["RealizedAmountInPKR"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdfrm.RootTable.Columns["RealizedAmountInPKR"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdfrm.RootTable.Columns["ContractAmount"].FormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdfrm.RootTable.Columns["ExchangeRate"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["ExchangeRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdfrm.RootTable.Columns["FTT/Comm"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["FTT/Comm"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["FTT/Comm"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdfrm.RootTable.Columns["FTT/Comm"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdfrm.RootTable.Columns["RealizedAmount"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["RealizedAmount"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["RealizedAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdfrm.RootTable.Columns["RealizedAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			foreach (GridEXColumn item in (JanusCollectionBase)grdfrm.RootTable.Columns)
			{
				item.HeaderStyle.TextAlignment = (TextAlignment)2;
			}
			grdfrm.RootTable.HeaderLines = 2;
			grdfrm.RootTable.Columns["GdNo"].Width = 192;
			grdfrm.RootTable.Columns["Consignee"].Width = 120;
			grdfrm.RootTable.Columns["Commodity"].Width = 100;
			grdfrm.RootTable.Columns["HsCode"].Width = 90;
			grdfrm.RootTable.Columns["ContractNo"].Width = 110;
			grdfrm.RootTable.Columns["ContractDate"].Width = 80;
			grdfrm.RootTable.Columns["ContractCurrency"].Width = 75;
			grdfrm.RootTable.Columns["ContractAmount"].Width = 90;
			grdfrm.RootTable.Columns["DateOfShipment"].Width = 80;
			grdfrm.RootTable.Columns["DateOfNegotion"].Width = 80;
			grdfrm.RootTable.Columns["RealizedCurrency"].Width = 80;
			grdfrm.RootTable.Columns["RealizedAmount"].Width = 110;
			grdfrm.RootTable.Columns["FTT/Comm"].Width = 90;
			grdfrm.RootTable.Columns["ExchangeRate"].Width = 75;
			grdfrm.RootTable.Columns["RealizedAmountInPKR"].Width = 110;
			grdfrm.RootTable.Columns["DateofRealization"].Width = 80;
			grdfrm.RootTable.Columns["FEReturnPageNo"].Width = 70;
			grdfrm.RootTable.Columns["ITRSNo"].Width = 60;
			grdfrm.RootTable.Columns["FEReturnMonth"].Width = 70;
			grdfrm.RootTable.Columns["Schedule"].Width = 75;
			grdfrm.RootTable.Columns["SalesTerm"].Width = 85;
			grdfrm.RootTable.Columns["Remarks"].Width = 120;
			grdfrm.RootTable.Columns["EERemarks"].Width = 120;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdfrm_UpdatingCell(object sender, UpdatingCellEventArgs e)
	{
		try
		{
			if ((e.Column.Key == "ITRSNo" || e.Column.Key == "FEReturnPageNo") && !CommonServices.IsNumeric(e))
			{
				throw new Exception("Please Type Only Numeric Value");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void grdfrm_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Expected O, but got Unknown
		//IL_0320: Unknown result type (might be due to invalid IL or missing references)
		//IL_0331: Expected O, but got Unknown
		try
		{
			GridEXRow r = grdfrm.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0 || grdfrm.RowCount <= 0)
			{
				return;
			}
			grdfrm.UpdateData();
			if (e.Column.Key == "FEReturnPageNo" || e.Column.Key == "FEReturnMonth" || e.Column.Key == "ITRSNo" || e.Column.Key == "Schedule" || e.Column.Key == "SalesTerm" || e.Column.Key == "EERemarks")
			{
				EEStatement val = new EEStatement
				{
					EEStatementList = new List<EEStatement>()
				};
				EEStatement Lst = new EEStatement();
				Lst.OrganizationId = UserAccount.OrganizationId;
				Lst.CompanyId = UserAccount.CompanyId;
				Lst.EntryUserId = UserAccount.ID;
				Lst.ModifyUserId = UserAccount.ID;
				Lst.EntryDate = DateTime.Now;
				Lst.ModifyDate = DateTime.Now;
				Lst.Id = Conversion.ToInt(r.Cells["EEId"].Value);
				Lst.FcyReceiptId = Conversion.ToInt(r.Cells["FcyBankReceiptId"].Value);
				Lst.FcyReceiptsDetailId = Conversion.ToInt(r.Cells["Id"].Value);
				Lst.FEReturnPageNo = Conversion.ToInt(r.Cells["FEReturnPageNo"].Value);
				Lst.ITRSNo = Conversion.ToInt(r.Cells["ITRSNo"].Value);
				Lst.FEReturnMonthId = Conversion.ToInt(r.Cells["FEReturnMonth"].Value);
				if (dtDateCombo.Rows.Count > 0)
				{
					DataRow foundRow = dtDateCombo.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<int>("MonthId") == Conversion.ToInt(r.Cells["FEReturnMonth"].Value));
					Lst.FEReturnYearId = ((foundRow != null) ? Conversion.ToInt(foundRow["YearId"]) : 0);
				}
				Lst.FEReturnMonth = Conversion.ToString((object)r.Cells["FEReturnMonth"].Text);
				Lst.Remarks = Conversion.ToString(r.Cells["EERemarks"].Value);
				Lst.SalesTerm = Conversion.ToString(r.Cells["SalesTerm"].Value);
				Lst.Schedule = Conversion.ToString(r.Cells["Schedule"].Value);
				val.EEStatementList.Add(Lst);
				EEStatement.Save(val);
			}
			grdfrm.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Btnreferesh_Click(object sender, EventArgs e)
	{
		try
		{
			ParameterFill();
			BankFill();
			RealizedBankFill();
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
			if (base.ActiveControl != null && base.ActiveControl.Name == "grdfrm")
			{
				cmbperemeter.Focus();
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
			((Control)(object)cmbrealizedbank).Text = string.Empty;
			((Control)(object)CmbBank).Text = string.Empty;
			grdfrm.ClearStructure();
			cmbperemeter.Focus();
			ChkDetail.Checked = true;
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

	private void frmStockReportWithValues_FormClosing(object sender, FormClosingEventArgs e)
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

	private void btnPrint567_1_Click(object sender, EventArgs e)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtStockSum.Rows.Count == 0 || dtStockSum == null)
			{
				throw new Exception("Record Not Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dtStockSum, "567_01-EEReport_ExportGD.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrint_Click(object sender, EventArgs e)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtStockSum.Rows.Count == 0 || dtStockSum == null)
			{
				throw new Exception("Record Not Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dtStockSum, "567-EEReport_ExportGD.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnprint_02_Click(object sender, EventArgs e)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtStockSum.Rows.Count == 0 || dtStockSum == null)
			{
				throw new Exception("Record Not Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dtStockSum, "567_02-EEReport_ExportGD.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmStockReportWithValues_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			else if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
			}
			else if (e.Control && e.KeyCode == Keys.S)
			{
				btnshow_Click(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.N)
			{
				btnNew_Click(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.P)
			{
				btnPrint_Click(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.F5)
			{
				cmbperemeter.Focus();
			}
			else if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grdfrm).Focus();
			}
			else if (e.Control && e.KeyCode == Keys.Up)
			{
				cmbperemeter.Focus();
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
			dt.Rows.Add("Ctrl+S", "For show data");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Ctrl+F5", "For Focus on DateType");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Date Type");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Grid");
			ShortCutKeyPopUp shortCutKeyPopUp = new ShortCutKeyPopUp(dt);
			shortCutKeyPopUp.StartPosition = FormStartPosition.CenterScreen;
			shortCutKeyPopUp.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnshortcut_Click(object sender, EventArgs e)
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
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Expected O, but got Unknown
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Expected O, but got Unknown
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Expected O, but got Unknown
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Expected O, but got Unknown
		//IL_0504: Unknown result type (might be due to invalid IL or missing references)
		//IL_050e: Expected O, but got Unknown
		//IL_051b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0525: Expected O, but got Unknown
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
		Appearance appearance1 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.grdfrm = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label2 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.CmbBank = new UltraCombo();
		this.cmbperemeter = new UltraCombo();
		this.gdtodate = new System.Windows.Forms.DateTimePicker();
		this.label4 = new System.Windows.Forms.Label();
		this.gdfrmdate = new System.Windows.Forms.DateTimePicker();
		this.label3 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.btnshow = new System.Windows.Forms.Button();
		this.label1 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.Btnreferesh = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.btnPrint567_1 = new System.Windows.Forms.ToolStripButton();
		this.btnshortcut = new System.Windows.Forms.ToolStripButton();
		this.ChkDetail = new System.Windows.Forms.RadioButton();
		this.ChkSummary = new System.Windows.Forms.RadioButton();
		this.cmbrealizedbank = new UltraCombo();
		this.label6 = new System.Windows.Forms.Label();
		this.btnprint_02 = new System.Windows.Forms.ToolStripButton();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel1.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdfrm).BeginInit();
		this.panel6.SuspendLayout();
		this.panel3.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBank).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbperemeter).BeginInit();
		this.panel5.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbrealizedbank).BeginInit();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1136, 597);
		this.panel1.TabIndex = 0;
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.grdfrm);
		this.panel4.Controls.Add(this.panel6);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 131);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1136, 466);
		this.panel4.TabIndex = 2;
		((ScrollableBase)this.grdfrm).BackColor = System.Drawing.Color.White;
		((System.Windows.Forms.Control)(object)this.grdfrm).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdfrm.FilterMode = (FilterMode)2;
		this.grdfrm.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdfrm).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdfrm.GroupTotals = (GroupTotals)2;
		this.grdfrm.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Location = new System.Drawing.Point(0, 25);
		((System.Windows.Forms.Control)(object)this.grdfrm).Name = "grdfrm";
		((ScrollableBase)this.grdfrm).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdfrm).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdfrm).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Size = new System.Drawing.Size(1136, 441);
		((System.Windows.Forms.Control)(object)this.grdfrm).TabIndex = 0;
		this.grdfrm.TotalRow = (InheritableBoolean)1;
		this.grdfrm.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdfrm).VisualStyle = (VisualStyle)4;
		this.grdfrm.CellUpdated += new ColumnActionEventHandler(grdfrm_CellUpdated);
		this.grdfrm.UpdatingCell += new UpdatingCellEventHandler(grdfrm_UpdatingCell);
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.ctrlGrdBar1);
		this.panel6.Controls.Add(this.label2);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Margin = new System.Windows.Forms.Padding(2);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1136, 25);
		this.panel6.TabIndex = 110;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(2, 2);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(67, 19);
		this.label2.TabIndex = 1;
		this.label2.Text = "History";
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add(this.groupBox1);
		this.panel3.Controls.Add(this.panel5);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 27);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1136, 104);
		this.panel3.TabIndex = 0;
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbrealizedbank);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add(this.ChkDetail);
		this.groupBox1.Controls.Add(this.ChkSummary);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBank);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbperemeter);
		this.groupBox1.Controls.Add(this.gdtodate);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.gdfrmdate);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add(this.btnshow);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(2, 24);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(863, 74);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.CmbBank.AllowNull = (DefaultableBoolean)1;
		this.CmbBank.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBank.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBank.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBank).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbBank).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBank).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBank).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBank).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.CmbBank).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBank).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbBank).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBank).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBank).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBank).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbBank).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBank).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBank).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBank).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbBank).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBank).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBank).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBank).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbBank).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBank).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBank).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbBank).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBank).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbBank).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBank).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBank).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBank).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBank.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBank).Location = new System.Drawing.Point(463, 14);
		((System.Windows.Forms.Control)(object)this.CmbBank).Name = "CmbBank";
		((System.Windows.Forms.Control)(object)this.CmbBank).Size = new System.Drawing.Size(315, 26);
		((System.Windows.Forms.Control)(object)this.CmbBank).TabIndex = 3;
		this.CmbBank.ValueChanged += new System.EventHandler(cmbperemeter_ValueChanged);
		this.cmbperemeter.AllowNull = (DefaultableBoolean)1;
		this.cmbperemeter.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbperemeter.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbperemeter.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Appearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance20;
		((SpecialBoxBase)((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance26).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbperemeter.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Location = new System.Drawing.Point(69, 14);
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Name = "cmbperemeter";
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Size = new System.Drawing.Size(308, 26);
		((System.Windows.Forms.Control)(object)this.cmbperemeter).TabIndex = 0;
		this.cmbperemeter.ValueChanged += new System.EventHandler(cmbperemeter_ValueChanged);
		this.gdtodate.CustomFormat = "dd-MMM-yy";
		this.gdtodate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.gdtodate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.gdtodate.Location = new System.Drawing.Point(252, 44);
		this.gdtodate.Name = "gdtodate";
		this.gdtodate.Size = new System.Drawing.Size(125, 23);
		this.gdtodate.TabIndex = 2;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(198, 48);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(48, 15);
		this.label4.TabIndex = 93;
		this.label4.Text = "To Date";
		this.gdfrmdate.CustomFormat = "dd-MMM-yy";
		this.gdfrmdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.gdfrmdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.gdfrmdate.Location = new System.Drawing.Point(69, 44);
		this.gdfrmdate.Name = "gdfrmdate";
		this.gdfrmdate.Size = new System.Drawing.Size(125, 23);
		this.gdfrmdate.TabIndex = 1;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(2, 48);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(63, 15);
		this.label3.TabIndex = 92;
		this.label3.Text = "From Date";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(381, 20);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(53, 15);
		this.label5.TabIndex = 90;
		this.label5.Text = "GD Bank";
		this.btnshow.BackColor = System.Drawing.Color.Teal;
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(724, 43);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(55, 26);
		this.btnshow.TabIndex = 7;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.btnshow.Leave += new System.EventHandler(btnshow_Leave);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(2, 20);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(60, 15);
		this.label1.TabIndex = 90;
		this.label1.Text = "Date Type";
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.label23);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1136, 24);
		this.panel5.TabIndex = 109;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(2, 2);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(183, 19);
		this.label23.TabIndex = 1;
		this.label23.Text = "EE Report_Export GD";
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1136, 27);
		this.panel2.TabIndex = 0;
		this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[6] { this.btnNew, this.Btnreferesh, this.btnPrint, this.btnPrint567_1, this.btnprint_02, this.btnshortcut });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1136, 27);
		this.toolStrip1.TabIndex = 1;
		this.toolStrip1.Text = "toolStrip1";
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(60, 24);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.Btnreferesh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.Btnreferesh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.Btnreferesh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.Btnreferesh.Name = "Btnreferesh";
		this.Btnreferesh.Size = new System.Drawing.Size(81, 24);
		this.Btnreferesh.Text = "&Refresh";
		this.Btnreferesh.Click += new System.EventHandler(Btnreferesh_Click);
		this.btnPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(93, 24);
		this.btnPrint.Text = "567-Print";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.btnPrint567_1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint567_1.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint567_1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint567_1.Name = "btnPrint567_1";
		this.btnPrint567_1.Size = new System.Drawing.Size(108, 24);
		this.btnPrint567_1.Text = "567-1-Print";
		this.btnPrint567_1.Click += new System.EventHandler(btnPrint567_1_Click);
		this.btnshortcut.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshortcut.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnshortcut.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnshortcut.Name = "btnshortcut";
		this.btnshortcut.Size = new System.Drawing.Size(127, 24);
		this.btnshortcut.Text = "ShortCut Keys";
		this.btnshortcut.Click += new System.EventHandler(btnshortcut_Click);
		this.ChkDetail.AutoSize = true;
		this.ChkDetail.Checked = true;
		this.ChkDetail.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkDetail.Location = new System.Drawing.Point(784, 32);
		this.ChkDetail.Name = "ChkDetail";
		this.ChkDetail.Size = new System.Drawing.Size(56, 19);
		this.ChkDetail.TabIndex = 5;
		this.ChkDetail.TabStop = true;
		this.ChkDetail.Text = "Detail";
		this.ChkDetail.UseVisualStyleBackColor = true;
		this.ChkSummary.AutoSize = true;
		this.ChkSummary.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkSummary.Location = new System.Drawing.Point(784, 52);
		this.ChkSummary.Name = "ChkSummary";
		this.ChkSummary.Size = new System.Drawing.Size(77, 19);
		this.ChkSummary.TabIndex = 6;
		this.ChkSummary.Text = "Summary";
		this.ChkSummary.UseVisualStyleBackColor = true;
		this.cmbrealizedbank.AllowNull = (DefaultableBoolean)1;
		this.cmbrealizedbank.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbrealizedbank.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbrealizedbank.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbrealizedbank).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbrealizedbank).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbrealizedbank).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbrealizedbank).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbrealizedbank).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance23;
		((SpecialBoxBase)((UltraGridBase)this.cmbrealizedbank).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance31).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance31).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbrealizedbank).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.cmbrealizedbank).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbrealizedbank).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbrealizedbank).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbrealizedbank).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.cmbrealizedbank).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbrealizedbank).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbrealizedbank).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance35).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbrealizedbank).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbrealizedbank).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbrealizedbank).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance36).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance36).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance36).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbrealizedbank).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbrealizedbank).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbrealizedbank).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbrealizedbank).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbrealizedbank).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbrealizedbank).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbrealizedbank).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.cmbrealizedbank).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbrealizedbank).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbrealizedbank).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbrealizedbank).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbrealizedbank.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbrealizedbank).Location = new System.Drawing.Point(463, 42);
		((System.Windows.Forms.Control)(object)this.cmbrealizedbank).Name = "cmbrealizedbank";
		((System.Windows.Forms.Control)(object)this.cmbrealizedbank).Size = new System.Drawing.Size(256, 26);
		((System.Windows.Forms.Control)(object)this.cmbrealizedbank).TabIndex = 4;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(381, 48);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(80, 15);
		this.label6.TabIndex = 594;
		this.label6.Text = "Realized Bank";
		this.btnprint_02.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnprint_02.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnprint_02.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnprint_02.Name = "btnprint_02";
		this.btnprint_02.Size = new System.Drawing.Size(116, 24);
		this.btnprint_02.Text = "567-02-Print";
		this.btnprint_02.Click += new System.EventHandler(btnprint_02_Click);
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1086, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdfrm;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 23);
		this.ctrlGrdBar1.TabIndex = 0;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1136, 597);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "EEReport_ExportGD";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmStockReportWithValues_FormClosing);
		base.Load += new System.EventHandler(frmStockReport_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmStockReportWithValues_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdfrm).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBank).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbperemeter).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbrealizedbank).EndInit();
		base.ResumeLayout(false);
	}
}
