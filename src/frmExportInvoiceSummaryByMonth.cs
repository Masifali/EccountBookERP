// using Architecture.Reports;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL.Reports.Export;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
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

public class frmExportInvoiceSummaryByMonth : Form
{
	private bool ShiftTabPressed;

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

	internal GridEX grdContractRegister;

	internal UltraCombo cmbperemeter;

	private Label label11;

	private ToolStripButton btnShortCutKey;

	private ToolStripButton btnRefresh;

	public frmExportInvoiceSummaryByMonth(UserAccount pUserAccount)
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
			if (Conversion.ToInt(cmbperemeter.Value) == 0)
			{
				((UltraGridBase)cmbperemeter).Rows[1].Activate();
			}
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
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		try
		{
			dtHistory.Rows.Clear();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.FromDate = Conversion.ToDateTime((object)txtdatefrom.Value);
			obj.ToDate = Conversion.ToDateTime((object)txtdateto.Value);
			dtHistory = ExportReports.SpExImInvoiceSummaryByMonth(obj);
			if (dtHistory.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Customer", typeof(string));
				dt.Columns.Add("FcyCode", typeof(string));
				dt.Columns.Add("FcyJuly", typeof(double));
				dt.Columns.Add("PkrJuly", typeof(double));
				dt.Columns.Add("FcyAug", typeof(double));
				dt.Columns.Add("PkrAug", typeof(double));
				dt.Columns.Add("FcySep", typeof(double));
				dt.Columns.Add("PkrSep", typeof(double));
				dt.Columns.Add("FcyOct", typeof(double));
				dt.Columns.Add("PkrOct", typeof(double));
				dt.Columns.Add("FcyNov", typeof(double));
				dt.Columns.Add("PkrNov", typeof(double));
				dt.Columns.Add("FcyDec", typeof(double));
				dt.Columns.Add("PkrDec", typeof(double));
				dt.Columns.Add("FcyJan", typeof(double));
				dt.Columns.Add("PkrJan", typeof(double));
				dt.Columns.Add("FcyFeb", typeof(double));
				dt.Columns.Add("PkrFeb", typeof(double));
				dt.Columns.Add("FcyMar", typeof(double));
				dt.Columns.Add("PkrMar", typeof(double));
				dt.Columns.Add("FcyApr", typeof(double));
				dt.Columns.Add("PkrApr", typeof(double));
				dt.Columns.Add("FcyMay", typeof(double));
				dt.Columns.Add("PkrMay", typeof(double));
				dt.Columns.Add("FcyJune", typeof(double));
				dt.Columns.Add("PkrJune", typeof(double));
				dt.Columns.Add("FcyTotal", typeof(double));
				dt.Columns.Add("PkrTotal", typeof(double));
				for (int i = 0; i < dtHistory.Rows.Count; i++)
				{
					dt.Rows.Add(dtHistory.Rows[i]["Customer"], dtHistory.Rows[i]["FCY_CODE"], dtHistory.Rows[i]["FCY_JULY"], dtHistory.Rows[i]["PKR_JULY"], dtHistory.Rows[i]["Fcy_Aug"], dtHistory.Rows[i]["Pkr_Aug"], dtHistory.Rows[i]["Fcy_Sep"], dtHistory.Rows[i]["Pkr_Sep"], dtHistory.Rows[i]["Fcy_Oct"], dtHistory.Rows[i]["Pkr_Oct"], dtHistory.Rows[i]["Fcy_Nov"], dtHistory.Rows[i]["Pkr_Nov"], dtHistory.Rows[i]["Fcy_Dec"], dtHistory.Rows[i]["Pkr_Dec"], dtHistory.Rows[i]["Fcy_Jan"], dtHistory.Rows[i]["Pkr_Jan"], dtHistory.Rows[i]["Fcy_Feb"], dtHistory.Rows[i]["Pkr_Feb"], dtHistory.Rows[i]["Fcy_Mar"], dtHistory.Rows[i]["Pkr_Mar"], dtHistory.Rows[i]["Fcy_Apr"], dtHistory.Rows[i]["Pkr_Apr"], dtHistory.Rows[i]["Fcy_May"], dtHistory.Rows[i]["Pkr_May"], dtHistory.Rows[i]["Fcy_Jun"], dtHistory.Rows[i]["Pkr_Jun"], dtHistory.Rows[i]["Fcy_Total"], dtHistory.Rows[i]["Pkr_Total"]);
				}
				grdContractRegister.DataSource = dt;
				grdContractRegister.RetrieveStructure();
				GridSetting();
			}
			else
			{
				grdContractRegister.ClearStructure();
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
			grdContractRegister.RootTable.Columns["FcyJuly"].AggregateFunction = (AggregateFunction)2;
			grdContractRegister.RootTable.Columns["FcyJuly"].TextAlignment = (TextAlignment)3;
			grdContractRegister.RootTable.Columns["FcyJuly"].FormatString = "#,##0.###";
			grdContractRegister.RootTable.Columns["FcyJuly"].TotalFormatString = "#,##0.###";
			grdContractRegister.RootTable.Columns["PkrJuly"].AggregateFunction = (AggregateFunction)2;
			grdContractRegister.RootTable.Columns["PkrJuly"].TextAlignment = (TextAlignment)3;
			grdContractRegister.RootTable.Columns["PkrJuly"].FormatString = "#,##0.###";
			grdContractRegister.RootTable.Columns["PkrJuly"].TotalFormatString = "#,##0.###";
			grdContractRegister.RootTable.Columns["FcyAug"].AggregateFunction = (AggregateFunction)2;
			grdContractRegister.RootTable.Columns["FcyAug"].TextAlignment = (TextAlignment)3;
			grdContractRegister.RootTable.Columns["FcyAug"].FormatString = "#,##0.###";
			grdContractRegister.RootTable.Columns["FcyAug"].TotalFormatString = "#,##0.###";
			grdContractRegister.RootTable.Columns["PkrAug"].AggregateFunction = (AggregateFunction)2;
			grdContractRegister.RootTable.Columns["PkrAug"].TextAlignment = (TextAlignment)3;
			grdContractRegister.RootTable.Columns["PkrAug"].FormatString = "#,##0.###";
			grdContractRegister.RootTable.Columns["PkrAug"].TotalFormatString = "#,##0.###";
			grdContractRegister.RootTable.Columns["FcySep"].AggregateFunction = (AggregateFunction)2;
			grdContractRegister.RootTable.Columns["FcySep"].TextAlignment = (TextAlignment)3;
			grdContractRegister.RootTable.Columns["FcySep"].FormatString = "#,##0.###";
			grdContractRegister.RootTable.Columns["FcySep"].TotalFormatString = "#,##0.###";
			grdContractRegister.RootTable.Columns["PkrSep"].AggregateFunction = (AggregateFunction)2;
			grdContractRegister.RootTable.Columns["PkrSep"].TextAlignment = (TextAlignment)3;
			grdContractRegister.RootTable.Columns["PkrSep"].FormatString = "#,##0.###";
			grdContractRegister.RootTable.Columns["PkrSep"].TotalFormatString = "#,##0.###";
			grdContractRegister.RootTable.Columns["FcyOct"].AggregateFunction = (AggregateFunction)2;
			grdContractRegister.RootTable.Columns["FcyOct"].TextAlignment = (TextAlignment)3;
			grdContractRegister.RootTable.Columns["FcyOct"].FormatString = "#,##0.###";
			grdContractRegister.RootTable.Columns["FcyOct"].TotalFormatString = "#,##0.###";
			grdContractRegister.RootTable.Columns["PkrOct"].AggregateFunction = (AggregateFunction)2;
			grdContractRegister.RootTable.Columns["PkrOct"].TextAlignment = (TextAlignment)3;
			grdContractRegister.RootTable.Columns["PkrOct"].FormatString = "#,##0.###";
			grdContractRegister.RootTable.Columns["PkrOct"].TotalFormatString = "#,##0.###";
			grdContractRegister.RootTable.Columns["FcyNov"].AggregateFunction = (AggregateFunction)2;
			grdContractRegister.RootTable.Columns["FcyNov"].TextAlignment = (TextAlignment)3;
			grdContractRegister.RootTable.Columns["FcyNov"].FormatString = "#,##0.###";
			grdContractRegister.RootTable.Columns["FcyNov"].TotalFormatString = "#,##0.###";
			grdContractRegister.RootTable.Columns["PkrNov"].AggregateFunction = (AggregateFunction)2;
			grdContractRegister.RootTable.Columns["PkrNov"].TextAlignment = (TextAlignment)3;
			grdContractRegister.RootTable.Columns["PkrNov"].FormatString = "#,##0.###";
			grdContractRegister.RootTable.Columns["PkrNov"].TotalFormatString = "#,##0.###";
			grdContractRegister.RootTable.Columns["FcyDec"].AggregateFunction = (AggregateFunction)2;
			grdContractRegister.RootTable.Columns["FcyDec"].TextAlignment = (TextAlignment)3;
			grdContractRegister.RootTable.Columns["FcyDec"].FormatString = "#,##0.###";
			grdContractRegister.RootTable.Columns["FcyDec"].TotalFormatString = "#,##0.###";
			grdContractRegister.RootTable.Columns["PkrDec"].AggregateFunction = (AggregateFunction)2;
			grdContractRegister.RootTable.Columns["PkrDec"].TextAlignment = (TextAlignment)3;
			grdContractRegister.RootTable.Columns["PkrDec"].FormatString = "#,##0.###";
			grdContractRegister.RootTable.Columns["PkrDec"].TotalFormatString = "#,##0.###";
			grdContractRegister.RootTable.Columns["FcyJan"].AggregateFunction = (AggregateFunction)2;
			grdContractRegister.RootTable.Columns["FcyJan"].TextAlignment = (TextAlignment)3;
			grdContractRegister.RootTable.Columns["FcyJan"].FormatString = "#,##0.###";
			grdContractRegister.RootTable.Columns["FcyJan"].TotalFormatString = "#,##0.###";
			grdContractRegister.RootTable.Columns["PkrJan"].AggregateFunction = (AggregateFunction)2;
			grdContractRegister.RootTable.Columns["PkrJan"].TextAlignment = (TextAlignment)3;
			grdContractRegister.RootTable.Columns["PkrJan"].FormatString = "#,##0.###";
			grdContractRegister.RootTable.Columns["PkrJan"].TotalFormatString = "#,##0.###";
			grdContractRegister.RootTable.Columns["FcyFeb"].AggregateFunction = (AggregateFunction)2;
			grdContractRegister.RootTable.Columns["FcyFeb"].TextAlignment = (TextAlignment)3;
			grdContractRegister.RootTable.Columns["FcyFeb"].FormatString = "#,##0.###";
			grdContractRegister.RootTable.Columns["FcyFeb"].TotalFormatString = "#,##0.###";
			grdContractRegister.RootTable.Columns["PkrFeb"].AggregateFunction = (AggregateFunction)2;
			grdContractRegister.RootTable.Columns["PkrFeb"].TextAlignment = (TextAlignment)3;
			grdContractRegister.RootTable.Columns["PkrFeb"].FormatString = "#,##0.###";
			grdContractRegister.RootTable.Columns["PkrFeb"].TotalFormatString = "#,##0.###";
			grdContractRegister.RootTable.Columns["FcyMar"].AggregateFunction = (AggregateFunction)2;
			grdContractRegister.RootTable.Columns["FcyMar"].TextAlignment = (TextAlignment)3;
			grdContractRegister.RootTable.Columns["FcyMar"].FormatString = "#,##0.###";
			grdContractRegister.RootTable.Columns["FcyMar"].TotalFormatString = "#,##0.###";
			grdContractRegister.RootTable.Columns["PkrMar"].AggregateFunction = (AggregateFunction)2;
			grdContractRegister.RootTable.Columns["PkrMar"].TextAlignment = (TextAlignment)3;
			grdContractRegister.RootTable.Columns["PkrMar"].FormatString = "#,##0.###";
			grdContractRegister.RootTable.Columns["PkrMar"].TotalFormatString = "#,##0.###";
			grdContractRegister.RootTable.Columns["FcyApr"].AggregateFunction = (AggregateFunction)2;
			grdContractRegister.RootTable.Columns["FcyApr"].TextAlignment = (TextAlignment)3;
			grdContractRegister.RootTable.Columns["FcyApr"].FormatString = "#,##0.###";
			grdContractRegister.RootTable.Columns["FcyApr"].TotalFormatString = "#,##0.###";
			grdContractRegister.RootTable.Columns["PkrApr"].AggregateFunction = (AggregateFunction)2;
			grdContractRegister.RootTable.Columns["PkrApr"].TextAlignment = (TextAlignment)3;
			grdContractRegister.RootTable.Columns["PkrApr"].FormatString = "#,##0.###";
			grdContractRegister.RootTable.Columns["PkrApr"].TotalFormatString = "#,##0.###";
			grdContractRegister.RootTable.Columns["FcyMay"].AggregateFunction = (AggregateFunction)2;
			grdContractRegister.RootTable.Columns["FcyMay"].TextAlignment = (TextAlignment)3;
			grdContractRegister.RootTable.Columns["FcyMay"].FormatString = "#,##0.###";
			grdContractRegister.RootTable.Columns["FcyMay"].TotalFormatString = "#,##0.###";
			grdContractRegister.RootTable.Columns["PkrMay"].AggregateFunction = (AggregateFunction)2;
			grdContractRegister.RootTable.Columns["PkrMay"].TextAlignment = (TextAlignment)3;
			grdContractRegister.RootTable.Columns["PkrMay"].FormatString = "#,##0.###";
			grdContractRegister.RootTable.Columns["PkrMay"].TotalFormatString = "#,##0.###";
			grdContractRegister.RootTable.Columns["FcyJune"].AggregateFunction = (AggregateFunction)2;
			grdContractRegister.RootTable.Columns["FcyJune"].TextAlignment = (TextAlignment)3;
			grdContractRegister.RootTable.Columns["FcyJune"].FormatString = "#,##0.###";
			grdContractRegister.RootTable.Columns["FcyJune"].TotalFormatString = "#,##0.###";
			grdContractRegister.RootTable.Columns["PkrJune"].AggregateFunction = (AggregateFunction)2;
			grdContractRegister.RootTable.Columns["PkrJune"].TextAlignment = (TextAlignment)3;
			grdContractRegister.RootTable.Columns["PkrJune"].FormatString = "#,##0.###";
			grdContractRegister.RootTable.Columns["PkrJune"].TotalFormatString = "#,##0.###";
			grdContractRegister.RootTable.Columns["FcyTotal"].AggregateFunction = (AggregateFunction)2;
			grdContractRegister.RootTable.Columns["FcyTotal"].TextAlignment = (TextAlignment)3;
			grdContractRegister.RootTable.Columns["FcyTotal"].FormatString = "#,##0.###";
			grdContractRegister.RootTable.Columns["FcyTotal"].TotalFormatString = "#,##0.###";
			grdContractRegister.RootTable.Columns["PkrTotal"].AggregateFunction = (AggregateFunction)2;
			grdContractRegister.RootTable.Columns["PkrTotal"].TextAlignment = (TextAlignment)3;
			grdContractRegister.RootTable.Columns["PkrTotal"].FormatString = "#,##0.###";
			grdContractRegister.RootTable.Columns["PkrTotal"].TotalFormatString = "#,##0.###";
			CommonServices.GridAutoAdjustmentNew(grdContractRegister);
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdfrm_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Expected O, but got Unknown
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Print")
			{
				DataTable dt = new DataTable();
				dt = ExportPdfReport.ExpRptSalesContractExport_501(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					Id = Conversion.ToInt(grdContractRegister.CurrentRow.Cells["Id"].Value),
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
			dt.Rows.Add("Alt+1", "For Print 518");
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
			if (!ShiftTabPressed)
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
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Expected O, but got Unknown
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			int ID = 0;
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (grdContractRegister.GetRows().Length != 0)
			{
				ID = Conversion.ToInt(grdContractRegister.CurrentRow.Cells["Id"].Value);
			}
			if (e.Control && e.KeyCode == Keys.Space && grdContractRegister.CurrentColumn.Key == "Print")
			{
				DataTable dt = new DataTable();
				dt = ExportPdfReport.ExpRptSalesContractExport_501(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					Id = ID,
					ApprovedFilter = Conversion.ToString((object)"All")
				});
				if (dt == null || dt.Rows.Count == 0)
				{
					MessageBox.Show("No Record Found For Display");
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
			val.ShowReportWithDataTable(dtHistory, "531-ExImInvoiceSummeryByMonth.rpt");
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
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Expected O, but got Unknown
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Expected O, but got Unknown
		//IL_0354: Unknown result type (might be due to invalid IL or missing references)
		//IL_035e: Expected O, but got Unknown
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.ExportReports.frmExportInvoiceSummaryByMonth));
		this.panel1 = new System.Windows.Forms.Panel();
		this.grdContractRegister = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel3 = new System.Windows.Forms.Panel();
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
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.btnShortCutKey = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdContractRegister).BeginInit();
		this.panel4.SuspendLayout();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbperemeter).BeginInit();
		this.panel5.SuspendLayout();
		this.panel2.SuspendLayout();
		this.tsItemType.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.grdContractRegister);
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(969, 633);
		this.panel1.TabIndex = 0;
		this.grdContractRegister.AllowEdit = (InheritableBoolean)2;
		((System.Windows.Forms.Control)(object)this.grdContractRegister).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdContractRegister.DynamicFiltering = true;
		this.grdContractRegister.FilterMode = (FilterMode)2;
		this.grdContractRegister.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdContractRegister).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdContractRegister.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdContractRegister).Location = new System.Drawing.Point(0, 173);
		((System.Windows.Forms.Control)(object)this.grdContractRegister).Name = "grdContractRegister";
		((ScrollableBase)this.grdContractRegister).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdContractRegister).Size = new System.Drawing.Size(969, 460);
		((System.Windows.Forms.Control)(object)this.grdContractRegister).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.grdContractRegister).TabStop = false;
		this.grdContractRegister.TotalRow = (InheritableBoolean)1;
		this.grdContractRegister.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdContractRegister).VisualStyle = (VisualStyle)4;
		this.grdContractRegister.ColumnButtonClick += new ColumnActionEventHandler(grdfrm_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdContractRegister).KeyDown += new System.Windows.Forms.KeyEventHandler(grdContractRegister_KeyDown);
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.ctrlGrdBar1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 149);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(969, 24);
		this.panel4.TabIndex = 2;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(927, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdContractRegister;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 24);
		this.ctrlGrdBar1.TabIndex = 0;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.panel3.BackColor = System.Drawing.Color.White;
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
		this.label4.TabIndex = 576;
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
		this.label1.TabIndex = 573;
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
		this.label3.Size = new System.Drawing.Size(289, 19);
		this.label3.TabIndex = 0;
		this.label3.Text = "Export Invoice Summary By Month";
		this.btnShow.BackColor = System.Drawing.Color.Teal;
		this.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShow.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShow.ForeColor = System.Drawing.Color.White;
		this.btnShow.Location = new System.Drawing.Point(218, 90);
		this.btnShow.Name = "btnShow";
		this.btnShow.Size = new System.Drawing.Size(65, 26);
		this.btnShow.TabIndex = 3;
		this.btnShow.Text = "Show";
		this.btnShow.UseVisualStyleBackColor = false;
		this.btnShow.Click += new System.EventHandler(btnShow_Click);
		this.btnShow.Leave += new System.EventHandler(btnShow_Leave);
		this.btnShow.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(btnShow_PreviewKeyDown);
		this.panel2.BackColor = System.Drawing.Color.White;
		this.panel2.Controls.Add(this.tsItemType);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(969, 27);
		this.panel2.TabIndex = 0;
		this.tsItemType.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tsItemType.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.tsItemType.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnnew, this.btnRefresh, this.btnPrint, this.btnShortCutKey });
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
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(81, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(151, 24);
		this.btnPrint.Text = "531-Print Register";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.btnShortCutKey.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnShortCutKey.Image = (System.Drawing.Image)resources.GetObject("btnShortCutKey.Image");
		this.btnShortCutKey.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKey.Name = "btnShortCutKey";
		this.btnShortCutKey.Size = new System.Drawing.Size(107, 24);
		this.btnShortCutKey.Text = "ShortCut Keys";
		this.btnShortCutKey.Click += new System.EventHandler(btnShortCutKey_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(969, 633);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmExportInvoiceSummaryByMonth";
		this.Text = "Summary By Month";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmExportInvoiceSummaryByMonth_FormClosing);
		base.Load += new System.EventHandler(frmExportShipingLineBookingRpt_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmExportShipingLineBookingRpt_KeyDown);
		this.panel1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdContractRegister).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
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
