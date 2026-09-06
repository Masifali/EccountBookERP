// using Architecture.Reports;
using System;
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
using Architecture.WinApp.Common;
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

public class GDBreakUpandRealized_Register : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dt = new DataTable();

	private IContainer components;

	private ToolStrip tsSelectedTrialBalance;

	private ToolStripButton btnRefreshSelectedTrial;

	private ToolStripButton btnReport;

	private Panel panel3;

	private Label label23;

	private Label label5;

	private Label label4;

	private DateTimePicker datFromDateSt;

	private DateTimePicker datToDateST;

	internal UltraCombo cmbinvoice;

	private Label label9;

	private Panel panel1;

	internal UltraCombo cmbbank;

	private Label label1;

	private CheckBox ChkZkipZeroSelected;

	internal UltraCombo cmbperemeter;

	private Label label12;

	private GroupBox groupBox1;

	private Button btnShow;

	private Panel panel2;

	private Label label2;

	private CtrlGrdBar ctrlGrdBar1;

	internal GridEX DataGridHistory;

	private ToolStripButton btnRefresh;

	private ToolStripButton btnShortCutKeys;

	public GDBreakUpandRealized_Register(UserAccount user)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void GDBreakUpandRealized_Register_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(950, 600);
			ParameterFill();
			AllDropDownBind();
			cmbperemeter.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AllDropDownBind()
	{
		try
		{
			DataTable dtAll = ExImInvoice.GetDataForDropDownFromExportGdBreakup(UserAccount.OrganizationId, UserAccount.CompanyId, (string)null);
			if (dtAll.Rows.Count > 0)
			{
				EnumerableRowCollection<DataRow> rows = from r in dtAll.AsEnumerable()
					where r.Field<string>("ActivityType") == "Invoice"
					select r;
				DataTable dtInvoice = (rows.Any() ? rows.CopyToDataTable() : dtAll.Clone());
				EnumerableRowCollection<DataRow> rowsitem = from r in dtAll.AsEnumerable()
					where r.Field<string>("ActivityType") == "Bank"
					select r;
				DataTable dtBank = (rowsitem.Any() ? rowsitem.CopyToDataTable() : dtAll.Clone());
				DDL.BindDDLNew(dtInvoice, cmbinvoice, "Id", "name", "Invoice", true);
				((UltraGridBase)cmbinvoice).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				DDL.BindDDLNew(dtBank, cmbbank, "Id", "name", "Bank", true);
				((UltraGridBase)cmbbank).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
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
			((UltraGridBase)cmbperemeter).Rows[2].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Gridfill()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		try
		{
			dt.Clear();
			ReportsParameters grn = new ReportsParameters();
			grn.OrganizationId = UserAccount.OrganizationId;
			grn.CompanyId = UserAccount.CompanyId;
			grn.FromDate = Conversion.ToDateTime((object)datFromDateSt.Value);
			grn.ToDate = Conversion.ToDateTime((object)datToDateST.Value);
			grn.ExImInvoiceId = Conversion.ToInt(cmbinvoice.Value);
			grn.ExImFCBankReceiptsId = Conversion.ToInt(cmbbank.Value);
			if (ChkZkipZeroSelected.Checked)
			{
				grn.ZeroBalanceType = 1;
			}
			dt = ExportReports.GDBreakUpandRealized_Register(grn);
			if (dt.Rows.Count > 0)
			{
				DataTable dt1 = new DataTable();
				dt1.Columns.Add("Id", typeof(int));
				dt1.Columns.Add("ExImInvoiceId", typeof(int));
				dt1.Columns.Add("ShipDate", typeof(string));
				dt1.Columns.Add("Tenor", typeof(string));
				dt1.Columns.Add("DueDate", typeof(string));
				dt1.Columns.Add("PTYInvoiceNo", typeof(string));
				dt1.Columns.Add("GDNO", typeof(string));
				dt1.Columns.Add("GDDate", typeof(DateTime));
				dt1.Columns.Add("BankInvoiceNo", typeof(string));
				dt1.Columns.Add("BankInvoiceDate", typeof(DateTime));
				dt1.Columns.Add("BankName", typeof(string));
				dt1.Columns.Add("GDValue", typeof(double));
				dt1.Columns.Add("Freight", typeof(double));
				dt1.Columns.Add("FOBValue", typeof(double));
				dt1.Columns.Add("FTT%", typeof(double));
				dt1.Columns.Add("FTT/M-FORM", typeof(double));
				dt1.Columns.Add("NetToBeRealized", typeof(double));
				dt1.Columns.Add("Realized", typeof(double));
				dt1.Columns.Add("Balance", typeof(double));
				dt1.Columns.Add("ExchangeRate", typeof(double));
				dt1.Columns.Add("GdLcyAmount", typeof(double));
				dt1.Columns.Add("RealizedLcyAmount", typeof(double));
				dt1.Columns.Add("UnRealizedLcyAmount", typeof(double));
				dt1.Columns.Add("GdStatus", typeof(string));
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					DataRow newRow = dt1.NewRow();
					newRow["Id"] = dt.Rows[i]["Id"];
					newRow["ExImInvoiceId"] = dt.Rows[i]["ExImInvoiceId"];
					newRow["ShipDate"] = dt.Rows[i]["ShipDate"];
					newRow["Tenor"] = dt.Rows[i]["Tenor"];
					newRow["DueDate"] = dt.Rows[i]["DueDate"];
					newRow["PTYInvoiceNo"] = dt.Rows[i]["PTYInvoiceNo"];
					newRow["GDNO"] = dt.Rows[i]["GDNO"];
					newRow["GDDate"] = dt.Rows[i]["GDDate"];
					newRow["BankInvoiceNo"] = dt.Rows[i]["BankInvoiceNo"];
					newRow["BankInvoiceDate"] = dt.Rows[i]["BankInvoiceDate"];
					newRow["BankName"] = dt.Rows[i]["BankName"];
					newRow["GDValue"] = dt.Rows[i]["GDValue"];
					newRow["Freight"] = dt.Rows[i]["Freight"];
					newRow["FOBValue"] = dt.Rows[i]["FOBValue"];
					newRow["FTT%"] = dt.Rows[i]["FTT%"];
					newRow["FTT/M-FORM"] = dt.Rows[i]["FTT/M-FORM"];
					newRow["NetToBeRealized"] = dt.Rows[i]["NetToBeRealized"];
					newRow["Realized"] = dt.Rows[i]["Realized"];
					newRow["Balance"] = dt.Rows[i]["Balance"];
					newRow["ExchangeRate"] = dt.Rows[i]["ExchangeRate"];
					newRow["GdLcyAmount"] = dt.Rows[i]["GdLcyAmount"];
					newRow["RealizedLcyAmount"] = dt.Rows[i]["RealizedLcyAmount"];
					newRow["UnRealizedLcyAmount"] = dt.Rows[i]["UnRealizedLcyAmount"];
					newRow["GdStatus"] = dt.Rows[i]["GdStatus"];
					dt1.Rows.Add(newRow);
				}
				DataGridHistory.DataSource = dt1;
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
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataGridHistory.RootTable.Columns["Id"].Visible = false;
			DataGridHistory.RootTable.Columns["ExImInvoiceId"].Visible = false;
			foreach (GridEXColumn item in (JanusCollectionBase)DataGridHistory.RootTable.Columns)
			{
				item.HeaderStyle.TextAlignment = (TextAlignment)2;
			}
			DataGridHistory.RootTable.HeaderLines = 2;
			DataGridHistory.RootTable.Columns["ShipDate"].Width = 80;
			DataGridHistory.RootTable.Columns["Tenor"].Width = 45;
			DataGridHistory.RootTable.Columns["DueDate"].Width = 80;
			DataGridHistory.RootTable.Columns["PTYInvoiceNo"].Width = 100;
			DataGridHistory.RootTable.Columns["GDNO"].Width = 150;
			DataGridHistory.RootTable.Columns["GDDate"].Width = 80;
			DataGridHistory.RootTable.Columns["BankInvoiceNo"].Width = 110;
			DataGridHistory.RootTable.Columns["BankInvoiceDate"].Width = 85;
			DataGridHistory.RootTable.Columns["BankName"].Width = 110;
			DataGridHistory.RootTable.Columns["GDValue"].Width = 85;
			DataGridHistory.RootTable.Columns["Freight"].Width = 75;
			DataGridHistory.RootTable.Columns["FOBValue"].Width = 85;
			DataGridHistory.RootTable.Columns["FTT%"].Width = 50;
			DataGridHistory.RootTable.Columns["FTT/M-FORM"].Width = 80;
			DataGridHistory.RootTable.Columns["ExchangeRate"].Width = 70;
			DataGridHistory.RootTable.Columns["NetToBeRealized"].Width = 90;
			DataGridHistory.RootTable.Columns["Realized"].Width = 90;
			DataGridHistory.RootTable.Columns["Balance"].Width = 100;
			DataGridHistory.RootTable.Columns["GdLcyAmount"].Width = 100;
			DataGridHistory.RootTable.Columns["RealizedLcyAmount"].Width = 100;
			DataGridHistory.RootTable.Columns["UnRealizedLcyAmount"].Width = 100;
			DataGridHistory.RootTable.Columns["GdStatus"].Width = 65;
			DataGridHistory.RootTable.Columns["Realized"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["Realized"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["Realized"].FormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["Realized"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["FTT/M-FORM"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["FTT/M-FORM"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["FTT/M-FORM"].FormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["FTT/M-FORM"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["ExchangeRate"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["ExchangeRate"].FormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["ExchangeRate"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["Balance"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["Balance"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["Balance"].FormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["Balance"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["GdLcyAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["GdLcyAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["GdLcyAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["GdLcyAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["RealizedLcyAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["RealizedLcyAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["RealizedLcyAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["RealizedLcyAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["UnRealizedLcyAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["UnRealizedLcyAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["UnRealizedLcyAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["UnRealizedLcyAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["NetToBeRealized"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["NetToBeRealized"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["NetToBeRealized"].FormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["NetToBeRealized"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["FTT%"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["FTT%"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["FTT%"].FormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["FTT%"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["FOBValue"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["FOBValue"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["FOBValue"].FormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["FOBValue"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["Freight"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["Freight"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["Freight"].FormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["Freight"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["GDValue"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["GDValue"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["GDValue"].FormatString = clsGlobalVariables.stringFormatsingle;
			DataGridHistory.RootTable.Columns["GDValue"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			ctrlGrdBar1_Load(null, null);
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
			MessageBox.Show(ex.ToString());
		}
	}

	private void GDBreakUpandRealized_Register_KeyDown(object sender, KeyEventArgs e)
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
			if (e.Control && e.KeyCode == Keys.S)
			{
				btnShow_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				cmbperemeter.Focus();
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnRefreshSelectedTrial_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.R)
			{
				btnRefresh_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.P)
			{
				btnReport_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)DataGridHistory).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				cmbperemeter.Focus();
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
			dt.Rows.Add("Ctrl+S", "For Shoe Record");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Ctrl+F5", "For Focus on Combo Date");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Grid History");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Date combo in Filters Box");
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

	private void btnShortCutKeys_Click_1(object sender, EventArgs e)
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

	private void btnRefreshSelectedTrial_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)(object)cmbinvoice).Text = string.Empty;
			((Control)(object)cmbbank).Text = string.Empty;
			cmbperemeter.Focus();
			ParameterFill();
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
			Gridfill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnReport_Click(object sender, EventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.ShowReportWithDataTable(dt, "559-GDBreakUpandRealized_Register.rpt");
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
				datFromDateSt.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 2)
			{
				datFromDateSt.Value = DateTime.Today.AddDays(-7.0);
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 3)
			{
				DateTime now = DateTime.UtcNow;
				DateTime first = now.Date.AddDays(-(now.Date.Day - 1));
				datFromDateSt.Value = first;
				datToDateST.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 4)
			{
				int year = DateTime.Now.Year;
				DateTime first2 = new DateTime(year, 1, 1);
				datFromDateSt.Value = first2;
				datToDateST.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 5)
			{
				datFromDateSt.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GDBreakUpandRealized_Register_FormClosed(object sender, FormClosedEventArgs e)
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

	private void btnShow_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl.Name == "ctrlGrdBar1")
			{
				cmbperemeter.Focus();
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
			AllDropDownBind();
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
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Expected O, but got Unknown
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Expected O, but got Unknown
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Expected O, but got Unknown
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0218: Expected O, but got Unknown
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.ExportReports.GDBreakUpandRealized_Register));
		Appearance appearance1 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
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
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		this.tsSelectedTrialBalance = new System.Windows.Forms.ToolStrip();
		this.btnRefreshSelectedTrial = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnReport = new System.Windows.Forms.ToolStripButton();
		this.btnShortCutKeys = new System.Windows.Forms.ToolStripButton();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.datFromDateSt = new System.Windows.Forms.DateTimePicker();
		this.datToDateST = new System.Windows.Forms.DateTimePicker();
		this.cmbinvoice = new UltraCombo();
		this.label9 = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.btnShow = new System.Windows.Forms.Button();
		this.cmbperemeter = new UltraCombo();
		this.label12 = new System.Windows.Forms.Label();
		this.ChkZkipZeroSelected = new System.Windows.Forms.CheckBox();
		this.cmbbank = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.DataGridHistory = new GridEX();
		this.label2 = new System.Windows.Forms.Label();
		this.tsSelectedTrialBalance.SuspendLayout();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbinvoice).BeginInit();
		this.panel1.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbperemeter).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbbank).BeginInit();
		this.panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		base.SuspendLayout();
		this.tsSelectedTrialBalance.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.tsSelectedTrialBalance.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnRefreshSelectedTrial, this.btnRefresh, this.btnReport, this.btnShortCutKeys });
		this.tsSelectedTrialBalance.Location = new System.Drawing.Point(0, 0);
		this.tsSelectedTrialBalance.Name = "tsSelectedTrialBalance";
		this.tsSelectedTrialBalance.Size = new System.Drawing.Size(1118, 27);
		this.tsSelectedTrialBalance.TabIndex = 9;
		this.tsSelectedTrialBalance.Text = "toolStrip1";
		this.btnRefreshSelectedTrial.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefreshSelectedTrial.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnRefreshSelectedTrial.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshSelectedTrial.Name = "btnRefreshSelectedTrial";
		this.btnRefreshSelectedTrial.Size = new System.Drawing.Size(68, 24);
		this.btnRefreshSelectedTrial.Text = "&Reset";
		this.btnRefreshSelectedTrial.Click += new System.EventHandler(btnRefreshSelectedTrial_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "&Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnReport.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnReport.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnReport.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnReport.Name = "btnReport";
		this.btnReport.Size = new System.Drawing.Size(92, 24);
		this.btnReport.Text = "559-Print";
		this.btnReport.Click += new System.EventHandler(btnReport_Click);
		this.btnShortCutKeys.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnShortCutKeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortCutKeys.Image = (System.Drawing.Image)resources.GetObject("btnShortCutKeys.Image");
		this.btnShortCutKeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKeys.Name = "btnShortCutKeys";
		this.btnShortCutKeys.Size = new System.Drawing.Size(101, 24);
		this.btnShortCutKeys.Text = "ShortCutKeys";
		this.btnShortCutKeys.ToolTipText = "ShortCutKeys";
		this.btnShortCutKeys.Click += new System.EventHandler(btnShortCutKeys_Click_1);
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.label23);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 27);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1118, 30);
		this.panel3.TabIndex = 10;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(5, 5);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(303, 19);
		this.label23.TabIndex = 1;
		this.label23.Text = "GD Break Up and Realized_Register";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(14, 49);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(63, 15);
		this.label5.TabIndex = 149;
		this.label5.Text = "From Date";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(14, 77);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(48, 15);
		this.label4.TabIndex = 148;
		this.label4.Text = "To Date";
		this.datFromDateSt.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datFromDateSt.CustomFormat = "dd-MMM-yy";
		this.datFromDateSt.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datFromDateSt.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datFromDateSt.Location = new System.Drawing.Point(93, 45);
		this.datFromDateSt.Name = "datFromDateSt";
		this.datFromDateSt.Size = new System.Drawing.Size(161, 23);
		this.datFromDateSt.TabIndex = 1;
		this.datToDateST.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datToDateST.CustomFormat = "dd-MMM-yy";
		this.datToDateST.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datToDateST.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datToDateST.Location = new System.Drawing.Point(94, 73);
		this.datToDateST.Name = "datToDateST";
		this.datToDateST.Size = new System.Drawing.Size(160, 23);
		this.datToDateST.TabIndex = 2;
		this.cmbinvoice.AllowNull = (DefaultableBoolean)1;
		this.cmbinvoice.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbinvoice.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbinvoice.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbinvoice).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbinvoice).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbinvoice).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbinvoice).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbinvoice).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance23;
		((SpecialBoxBase)((UltraGridBase)this.cmbinvoice).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance25).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance25).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance25).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbinvoice).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbinvoice).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbinvoice).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbinvoice).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance27).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbinvoice).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.cmbinvoice).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbinvoice).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbinvoice).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance35).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbinvoice).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbinvoice).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbinvoice).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance36).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance36).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance36).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbinvoice).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbinvoice).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbinvoice).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbinvoice).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbinvoice).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbinvoice).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbinvoice).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.cmbinvoice).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbinvoice).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbinvoice).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbinvoice).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbinvoice.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbinvoice).Location = new System.Drawing.Point(331, 13);
		((System.Windows.Forms.Control)(object)this.cmbinvoice).Name = "cmbinvoice";
		((System.Windows.Forms.Control)(object)this.cmbinvoice).Size = new System.Drawing.Size(177, 26);
		((System.Windows.Forms.Control)(object)this.cmbinvoice).TabIndex = 3;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(263, 19);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(49, 15);
		this.label9.TabIndex = 150;
		this.label9.Text = "Invoice ";
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.Controls.Add(this.groupBox1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 57);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1118, 112);
		this.panel1.TabIndex = 151;
		this.groupBox1.Controls.Add(this.btnShow);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbperemeter);
		this.groupBox1.Controls.Add(this.datFromDateSt);
		this.groupBox1.Controls.Add(this.label12);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add(this.ChkZkipZeroSelected);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbbank);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbinvoice);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.datToDateST);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(4, 4);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(591, 104);
		this.groupBox1.TabIndex = 592;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.btnShow.BackColor = System.Drawing.Color.Teal;
		this.btnShow.FlatAppearance.BorderSize = 0;
		this.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShow.ForeColor = System.Drawing.Color.White;
		this.btnShow.Location = new System.Drawing.Point(452, 72);
		this.btnShow.Margin = new System.Windows.Forms.Padding(2);
		this.btnShow.Name = "btnShow";
		this.btnShow.Size = new System.Drawing.Size(56, 24);
		this.btnShow.TabIndex = 6;
		this.btnShow.Text = "Show";
		this.btnShow.UseVisualStyleBackColor = false;
		this.btnShow.Click += new System.EventHandler(btnShow_Click);
		this.btnShow.Leave += new System.EventHandler(btnShow_Leave);
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
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Location = new System.Drawing.Point(94, 13);
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Name = "cmbperemeter";
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Size = new System.Drawing.Size(160, 26);
		((System.Windows.Forms.Control)(object)this.cmbperemeter).TabIndex = 0;
		this.cmbperemeter.ValueChanged += new System.EventHandler(cmbperemeter_ValueChanged);
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(14, 19);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(60, 15);
		this.label12.TabIndex = 590;
		this.label12.Text = "Date Type";
		this.ChkZkipZeroSelected.AutoSize = true;
		this.ChkZkipZeroSelected.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkZkipZeroSelected.Location = new System.Drawing.Point(331, 76);
		this.ChkZkipZeroSelected.Margin = new System.Windows.Forms.Padding(2);
		this.ChkZkipZeroSelected.Name = "ChkZkipZeroSelected";
		this.ChkZkipZeroSelected.Size = new System.Drawing.Size(76, 19);
		this.ChkZkipZeroSelected.TabIndex = 5;
		this.ChkZkipZeroSelected.Text = "Skip Zero";
		this.ChkZkipZeroSelected.UseVisualStyleBackColor = true;
		this.cmbbank.AllowNull = (DefaultableBoolean)1;
		this.cmbbank.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbbank.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbbank.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbbank).DisplayLayout.Appearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbbank).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbbank).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbbank).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbbank).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance20;
		((SpecialBoxBase)((UltraGridBase)this.cmbbank).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbbank).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.cmbbank).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbbank).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbbank).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbbank).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbbank).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbbank).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbbank).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance28;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance29).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbbank).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbbank).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbbank).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance30).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance30).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance30).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbbank).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbbank).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.cmbbank).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbbank).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbbank).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.cmbbank).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbbank).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.cmbbank).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbbank).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbbank).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbbank).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbbank.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbbank).Location = new System.Drawing.Point(331, 43);
		((System.Windows.Forms.Control)(object)this.cmbbank).Name = "cmbbank";
		((System.Windows.Forms.Control)(object)this.cmbbank).Size = new System.Drawing.Size(177, 26);
		((System.Windows.Forms.Control)(object)this.cmbbank).TabIndex = 4;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(263, 49);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(33, 15);
		this.label1.TabIndex = 152;
		this.label1.Text = "Bank";
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.ctrlGrdBar1);
		this.panel2.Controls.Add(this.label2);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 169);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1118, 30);
		this.panel2.TabIndex = 152;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1070, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(42, 25);
		this.ctrlGrdBar1.TabIndex = 21;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.DataGridHistory.AllowEdit = (InheritableBoolean)2;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistory.DynamicFiltering = true;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(0, 199);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1118, 473);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 153;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(5, 5);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(67, 19);
		this.label2.TabIndex = 1;
		this.label2.Text = "History";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(1118, 672);
		base.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		base.Controls.Add(this.panel2);
		base.Controls.Add(this.panel1);
		base.Controls.Add(this.panel3);
		base.Controls.Add(this.tsSelectedTrialBalance);
		base.KeyPreview = true;
		base.Name = "GDBreakUpandRealized_Register";
		this.Text = "GDBreakUpandRealized_Register";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(GDBreakUpandRealized_Register_FormClosed);
		base.Load += new System.EventHandler(GDBreakUpandRealized_Register_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(GDBreakUpandRealized_Register_KeyDown);
		this.tsSelectedTrialBalance.ResumeLayout(false);
		this.tsSelectedTrialBalance.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbinvoice).EndInit();
		this.panel1.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbperemeter).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbbank).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}

}
