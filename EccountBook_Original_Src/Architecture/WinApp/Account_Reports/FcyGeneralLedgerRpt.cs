using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Reports.Accounts;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Authentication;
using Architecture.Reports;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Account_Reports;

public class FcyGeneralLedgerRpt : Form
{
	private DataTable dtData = new DataTable();

	public DateTime FromDate;

	public DateTime ToDate;

	public int AccountId;

	private bool AllowEditOnVoucherNoInReports;

	private DropDownBind DDL = new DropDownBind();

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel3;

	private Label label23;

	private Panel panel4;

	private DateTimePicker todate;

	private DateTimePicker fromdate;

	private Label label33;

	private Label label27;

	private Panel panel1;

	private Panel panel2;

	private ToolStrip toolStrip3;

	private ToolStripButton print;

	private ToolStripButton btnNew;

	private Panel panel5;

	private Panel panel6;

	private CtrlGrdBar ctrlGrdBar1;

	internal GridEX grd;

	private Label label2;

	internal UltraCombo cmbAccountTitle;

	private Button btnshow;

	private RadioButton rdFromAccounts;

	private RadioButton rdFromExports;

	private ToolStripButton btnPrint105C;

	private ToolStripButton btnPrint105CII;

	public FcyGeneralLedgerRpt(UserAccount user)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void VoucherValidation_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.P)
			{
				print_Click_1(null, null);
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnNew_Click(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AccountTitleFill(int CostCenterId = 0)
	{
		try
		{
			DataTable dt = CommonServices.CoaAllocationAccountTitleByAccountTypeIds("22", null, CostCenterId);
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbAccountTitle, "Id", "AccountTitle", "Account Title", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void VoucherValidation_Load(object sender, EventArgs e)
	{
		try
		{
			ConfigrationsAllocation obj = clsGlobalVariables.configrationsAllocation.FirstOrDefault((ConfigrationsAllocation r) => r.ConfigDescription == "AllowEditOnVoucherNoinReports");
			AllowEditOnVoucherNoInReports = Conversion.ToBool((object)((obj != null) ? obj.ConfigKey : null));
			AccountTitleFill();
			fromdate.Focus();
			if (AccountId > 0)
			{
				cmbAccountTitle.Value = AccountId;
				fromdate.Value = FromDate;
				todate.Value = ToDate;
				btnshow_Click(null, null);
			}
			else
			{
				fromdate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void reset()
	{
		try
		{
			((Control)(object)cmbAccountTitle).Text = string.Empty;
			fromdate.Focus();
			grd.ClearStructure();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void print_Click_1(object sender, EventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtData == null || dtData.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dtData, "156-FcyGeneralLedger.rpt");
		}
		catch (Exception)
		{
			MessageBox.Show("Report Not Found");
		}
	}

	private void toolStripButton17_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		reset();
	}

	private void btnshow_Click(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		try
		{
			dtData.Rows.Clear();
			ReportsParameters vh = new ReportsParameters();
			vh.OrganizationId = UserAccount.OrganizationId;
			vh.CompanyId = UserAccount.CompanyId;
			vh.AccountId = Conversion.ToInt(cmbAccountTitle.Value);
			if (vh.AccountId == 0)
			{
				MessageBox.Show("Account Title Required", "Warning Message!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				cmbAccountTitle.Focus();
				return;
			}
			vh.FromDate = fromdate.Value;
			vh.ToDate = todate.Value;
			if (rdFromExports.Checked)
			{
				print.Visible = true;
				btnPrint105C.Visible = false;
				btnPrint105CII.Visible = false;
				((Control)(object)grd).Name = "FcyLedgerFromExports";
				dtData = VoucherReports.FCY_LedgerAgainstAccountId(vh);
				if (dtData.Rows.Count > 0)
				{
					DataTable dt = new DataTable();
					dt.Columns.Add("AccountTitle", typeof(string));
					dt.Columns.Add("VoucherDate", typeof(DateTime));
					dt.Columns.Add("VoucherType", typeof(string));
					dt.Columns.Add("VoucherCode", typeof(int));
					dt.Columns.Add("ContractNo", typeof(string));
					dt.Columns.Add("InvoiceNo", typeof(string));
					dt.Columns.Add("M_Tons", typeof(double));
					dt.Columns.Add("FcyCode", typeof(string));
					dt.Columns.Add("ExchangeRate", typeof(double));
					dt.Columns.Add("FcyOpening", typeof(double));
					dt.Columns.Add("FcyDebit", typeof(double));
					dt.Columns.Add("FcyCredit", typeof(double));
					dt.Columns.Add("FcyBalance", typeof(double));
					dt.Columns.Add("PkrOpening", typeof(double));
					dt.Columns.Add("PkrDebit", typeof(double));
					dt.Columns.Add("PkrCredit", typeof(double));
					dt.Columns.Add("PkrBalance", typeof(double));
					dt.Clear();
					for (int i = 0; i < dtData.Rows.Count; i++)
					{
						dt.Rows.Add(Conversion.ToString(dtData.Rows[i]["AccountTitle"]), Conversion.ToDateTime(dtData.Rows[i]["VoucherDate"]), Conversion.ToString(dtData.Rows[i]["DocType"]), Conversion.ToInt(dtData.Rows[i]["VoucherCode"]), Conversion.ToString(dtData.Rows[i]["ContractNo"]), Conversion.ToString(dtData.Rows[i]["InvoiceNo"]), Conversion.ToDouble(dtData.Rows[i]["M_Tons"]), Conversion.ToString(dtData.Rows[i]["CurrencyCode"]), Conversion.ToDouble(dtData.Rows[i]["ExchangeRate"]), Conversion.ToDouble(dtData.Rows[i]["FcyOpening"]), Conversion.ToDouble(dtData.Rows[i]["FcyDebit"]), Conversion.ToDouble(dtData.Rows[i]["FcyCredit"]), Conversion.ToDouble(dtData.Rows[i]["FcyBalance"]), Conversion.ToDouble(dtData.Rows[i]["PkrOpening"]), Conversion.ToDouble(dtData.Rows[i]["PkrDebit"]), Conversion.ToDouble(dtData.Rows[i]["PkrCredit"]), Conversion.ToDouble(dtData.Rows[i]["PkrBalance"]));
					}
					grd.DataSource = dt;
					grd.RetrieveStructure();
					GridSetting();
				}
				else
				{
					grd.ClearStructure();
				}
			}
			else if (rdFromAccounts.Checked)
			{
				((Control)(object)grd).Name = "FcyLedgerFromAccounts";
				print.Visible = false;
				btnPrint105C.Visible = true;
				btnPrint105CII.Visible = true;
				vh.ApprovedFilter = "All";
				dtData = VoucherReports.Accounts_FcyGeneralLedger_Rpt(vh);
				if (dtData.Rows.Count <= 0)
				{
					return;
				}
				DataTable dt2 = new DataTable();
				dt2.Columns.Add("VoucherDate", typeof(DateTime));
				dt2.Columns.Add("DocType");
				dt2.Columns.Add("VoucherNo");
				dt2.Columns.Add("AccountCode");
				dt2.Columns.Add("OffSetTitle");
				dt2.Columns.Add("FcyCode");
				dt2.Columns.Add("ExchangeRate", typeof(double));
				dt2.Columns.Add("FcyDebit", typeof(double));
				dt2.Columns.Add("FcyCredit", typeof(double));
				dt2.Columns.Add("FcyBalance", typeof(double));
				dt2.Columns.Add("BalType");
				dt2.Columns.Add("LcyDebit", typeof(double));
				dt2.Columns.Add("LcyCredit", typeof(double));
				dt2.Columns.Add("LcyBalance", typeof(double));
				dt2.Columns.Add("LcyBalType");
				dt2.Columns.Add("Comments");
				dt2.Columns.Add("MannualNo");
				dt2.Columns.Add("NoOfAttachments", typeof(double));
				dt2.Columns.Add("VoucherHeadId", typeof(int));
				dt2.Columns.Add("DocumentTypeId", typeof(int));
				dt2.Columns.Add("DocumentTypeSrNo", typeof(int));
				for (int j = 0; j < dtData.Rows.Count; j++)
				{
					string type = "Nill";
					if (Conversion.ToDouble(dtData.Rows[j]["FcyBalance"]) > 0.0)
					{
						type = "Dr";
					}
					else if (Conversion.ToDouble(dtData.Rows[j]["FcyBalance"]) < 0.0)
					{
						type = "Cr";
					}
					string typeLcy = "Nill";
					if (Conversion.ToDouble(dtData.Rows[j]["Balance"]) > 0.0)
					{
						typeLcy = "Dr";
					}
					else if (Conversion.ToDouble(dtData.Rows[j]["Balance"]) < 0.0)
					{
						typeLcy = "Cr";
					}
					dt2.Rows.Add(Conversion.ToDateTime(dtData.Rows[j]["VoucherDate"]).ToShortDateString(), dtData.Rows[j]["DocTypeCode"], dtData.Rows[j]["VoucherCode"], dtData.Rows[j]["AccountCode"], dtData.Rows[j]["OffsetAccountTitle"], dtData.Rows[j]["FcyCode"], dtData.Rows[j]["ExRateCal"], dtData.Rows[j]["FcyDebit"], dtData.Rows[j]["FcyCredit"], dtData.Rows[j]["FcyBalance"], type, dtData.Rows[j]["DebitAmount"], dtData.Rows[j]["CreditAmount"], dtData.Rows[j]["Balance"], typeLcy, dtData.Rows[j]["Comments"], dtData.Rows[j]["MannualNo"], dtData.Rows[j]["NoOfAttachments"], dtData.Rows[j]["VoucherHeadId"], dtData.Rows[j]["DocumentTypeId"], dtData.Rows[j]["DocumentTypeSrNo"]);
				}
				grd.DataSource = dt2;
				grd.RetrieveStructure();
				grdFcyLedgerFromAccountsSetting();
			}
			else
			{
				grd.ClearStructure();
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
			grd.AutomaticSort = false;
			List<string> hiddenColumns = new List<string> { "AccountTitle" };
			GridEX_Helper.GridWrappingAndColumnSettings(grd, 2, 3, hiddenColumns, null, null, ColumnSetting: false);
			grd.RootTable.Columns["AccountTitle"].Width = 200;
			grd.RootTable.Columns["VoucherDate"].Width = 75;
			grd.RootTable.Columns["VoucherType"].Width = 70;
			grd.RootTable.Columns["VoucherCode"].Width = 65;
			grd.RootTable.Columns["ContractNo"].Width = 90;
			grd.RootTable.Columns["InvoiceNo"].Width = 90;
			grd.RootTable.Columns["M_Tons"].Width = 70;
			grd.RootTable.Columns["FcyCode"].Width = 70;
			grd.RootTable.Columns["ExchangeRate"].Width = 70;
			grd.RootTable.Columns["FcyOpening"].Width = 110;
			grd.RootTable.Columns["FcyDebit"].Width = 110;
			grd.RootTable.Columns["FcyCredit"].Width = 110;
			grd.RootTable.Columns["FcyBalance"].Width = 110;
			grd.RootTable.Columns["PkrOpening"].Width = 115;
			grd.RootTable.Columns["PkrDebit"].Width = 110;
			grd.RootTable.Columns["PkrCredit"].Width = 110;
			grd.RootTable.Columns["PkrBalance"].Width = 130;
			grd.RootTable.Columns["M_Tons"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["M_Tons"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["M_Tons"].FormatString = "#,##0.###";
			grd.RootTable.Columns["M_Tons"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["ExchangeRate"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ExchangeRate"].FormatString = "#,##0.###";
			grd.RootTable.Columns["FcyOpening"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["FcyOpening"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["FcyOpening"].FormatString = clsGlobalVariables.stringFormatbothForFcy;
			grd.RootTable.Columns["FcyOpening"].TotalFormatString = clsGlobalVariables.stringFormatbothForFcy;
			grd.RootTable.Columns["FcyDebit"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["FcyDebit"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["FcyDebit"].FormatString = clsGlobalVariables.stringFormatbothForFcy;
			grd.RootTable.Columns["FcyDebit"].TotalFormatString = clsGlobalVariables.stringFormatbothForFcy;
			grd.RootTable.Columns["FcyCredit"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["FcyCredit"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["FcyCredit"].FormatString = clsGlobalVariables.stringFormatbothForFcy;
			grd.RootTable.Columns["FcyCredit"].TotalFormatString = clsGlobalVariables.stringFormatbothForFcy;
			grd.RootTable.Columns["FcyBalance"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["FcyBalance"].FormatString = clsGlobalVariables.stringFormatbothForFcy;
			grd.RootTable.Columns["FcyBalance"].TotalFormatString = clsGlobalVariables.stringFormatbothForFcy;
			grd.RootTable.Columns["PkrOpening"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["PkrOpening"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["PkrOpening"].FormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["PkrOpening"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["PkrDebit"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["PkrDebit"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["PkrDebit"].FormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["PkrDebit"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["PkrCredit"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["PkrCredit"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["PkrCredit"].FormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["PkrCredit"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["PkrBalance"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["PkrBalance"].FormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["PkrBalance"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdFcyLedgerFromAccountsSetting()
	{
		try
		{
			grd.AutomaticSort = false;
			List<string> hiddenColumns = new List<string> { "AccountTitle", "VoucherHeadId", "DocumentTypeId", "DocumentTypeSrNo" };
			GridEX_Helper.GridWrappingAndColumnSettings(grd, 2, 3, hiddenColumns, null, null, ColumnSetting: false);
			grd.RootTable.Columns["MannualNo"].Caption = "Manual No";
			grd.RootTable.Columns["VoucherDate"].Width = 75;
			grd.RootTable.Columns["DocType"].Width = 70;
			grd.RootTable.Columns["VoucherNo"].Width = 65;
			grd.RootTable.Columns["AccountCode"].Width = 75;
			grd.RootTable.Columns["OffSetTitle"].Width = 180;
			grd.RootTable.Columns["FcyCode"].Width = 55;
			grd.RootTable.Columns["ExchangeRate"].Width = 70;
			grd.RootTable.Columns["FcyDebit"].Width = 110;
			grd.RootTable.Columns["FcyCredit"].Width = 110;
			grd.RootTable.Columns["FcyBalance"].Width = 110;
			grd.RootTable.Columns["BalType"].Width = 50;
			grd.RootTable.Columns["LcyDebit"].Width = 110;
			grd.RootTable.Columns["LcyCredit"].Width = 110;
			grd.RootTable.Columns["LcyBalance"].Width = 110;
			grd.RootTable.Columns["LcyBalType"].Width = 60;
			grd.RootTable.Columns["Comments"].Width = 130;
			grd.RootTable.Columns["MannualNo"].Width = 70;
			grd.RootTable.Columns["NoOfAttachments"].Width = 85;
			grd.RootTable.Columns["ExchangeRate"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ExchangeRate"].FormatString = clsGlobalVariables.DecimalFCYRateFormate;
			grd.RootTable.Columns["FcyDebit"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["FcyDebit"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["FcyDebit"].FormatString = clsGlobalVariables.stringFormatbothForFcy;
			grd.RootTable.Columns["FcyDebit"].TotalFormatString = clsGlobalVariables.stringFormatbothForFcy;
			grd.RootTable.Columns["FcyCredit"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["FcyCredit"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["FcyCredit"].FormatString = clsGlobalVariables.stringFormatbothForFcy;
			grd.RootTable.Columns["FcyCredit"].TotalFormatString = clsGlobalVariables.stringFormatbothForFcy;
			grd.RootTable.Columns["FcyBalance"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["FcyBalance"].FormatString = clsGlobalVariables.stringFormatbothForFcy;
			grd.RootTable.Columns["FcyBalance"].TotalFormatString = clsGlobalVariables.stringFormatbothForFcy;
			grd.RootTable.Columns["LcyDebit"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["LcyDebit"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["LcyDebit"].FormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["LcyDebit"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["LcyCredit"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["LcyCredit"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["LcyCredit"].FormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["LcyCredit"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["LcyBalance"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["LcyBalance"].FormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["LcyBalance"].TotalFormatString = clsGlobalVariables.stringFormatboth;
			grd.RootTable.Columns["DocType"].ColumnType = (ColumnType)5;
			grd.RootTable.Columns["VoucherNo"].ColumnType = (ColumnType)((UserAccount.RoleName == "Admin") ? 5 : (AllowEditOnVoucherNoInReports ? 5 : 0));
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grd).Name);
			if (fs != null)
			{
				grd.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void FcyGeneralLedgerRpt_FormClosing(object sender, FormClosingEventArgs e)
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

	private void btnPrint105C_Click(object sender, EventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtData == null || dtData.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dtData, "105C-AcRptFcyGeneralLedger.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrint105CII_Click(object sender, EventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtData == null || dtData.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dtData, "105D-AcRptFcyGeneralLedger.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow currentRow = grd.CurrentRow;
			int Id = Conversion.ToInt(currentRow.Cells["VoucherHeadId"].Value);
			int DocumentTypeId = Conversion.ToInt(currentRow.Cells["DocumentTypeId"].Value);
			int DocumentTypeIdSrNo = Conversion.ToInt(currentRow.Cells["DocumentTypeSrNo"].Value);
			Conversion.ToDateTime(currentRow.Cells["VoucherDate"].Value);
			if (e.Column.Key == "VoucherNo" || e.Column.Key == "VoucherCode")
			{
				CommonServices.EditMethodFromLinked(DocumentTypeId, Id, DocumentTypeIdSrNo);
			}
			else if (e.Column.Key == "DocType" || e.Column.Key == "VoucherType")
			{
				CommonServices.ANewAcRptPaymentReceiptsVoucherSlip_102(Id);
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
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Expected O, but got Unknown
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Expected O, but got Unknown
		//IL_0405: Unknown result type (might be due to invalid IL or missing references)
		//IL_040f: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		this.panel3 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.grd = new GridEX();
		this.label23 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.rdFromAccounts = new System.Windows.Forms.RadioButton();
		this.rdFromExports = new System.Windows.Forms.RadioButton();
		this.btnshow = new System.Windows.Forms.Button();
		this.cmbAccountTitle = new UltraCombo();
		this.todate = new System.Windows.Forms.DateTimePicker();
		this.fromdate = new System.Windows.Forms.DateTimePicker();
		this.label2 = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.print = new System.Windows.Forms.ToolStripButton();
		this.btnPrint105C = new System.Windows.Forms.ToolStripButton();
		this.btnPrint105CII = new System.Windows.Forms.ToolStripButton();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel4.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbAccountTitle).BeginInit();
		this.panel1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		base.SuspendLayout();
		this.panel3.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel3.Controls.Add(this.ctrlGrdBar1);
		this.panel3.Controls.Add(this.label23);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 27);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(962, 26);
		this.panel3.TabIndex = 4;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(920, 3);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 20;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.grd.AllowEdit = (InheritableBoolean)2;
		this.grd.AlternatingColors = true;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.DynamicFiltering = true;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(962, 495);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.LinkClicked += new ColumnActionEventHandler(grd_LinkClicked);
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(6, 3);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(101, 19);
		this.label23.TabIndex = 1;
		this.label23.Text = "FCY Ledger";
		this.panel4.Controls.Add(this.panel6);
		this.panel4.Controls.Add(this.panel5);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 53);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(962, 554);
		this.panel4.TabIndex = 5;
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(0, 59);
		this.panel6.Margin = new System.Windows.Forms.Padding(2);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(962, 495);
		this.panel6.TabIndex = 108;
		this.panel5.BackColor = System.Drawing.Color.White;
		this.panel5.Controls.Add(this.rdFromAccounts);
		this.panel5.Controls.Add(this.rdFromExports);
		this.panel5.Controls.Add(this.btnshow);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbAccountTitle);
		this.panel5.Controls.Add(this.todate);
		this.panel5.Controls.Add(this.fromdate);
		this.panel5.Controls.Add(this.label2);
		this.panel5.Controls.Add(this.label33);
		this.panel5.Controls.Add(this.label27);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(962, 59);
		this.panel5.TabIndex = 107;
		this.rdFromAccounts.AutoSize = true;
		this.rdFromAccounts.Checked = true;
		this.rdFromAccounts.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdFromAccounts.Location = new System.Drawing.Point(593, 32);
		this.rdFromAccounts.Name = "rdFromAccounts";
		this.rdFromAccounts.Size = new System.Drawing.Size(106, 19);
		this.rdFromAccounts.TabIndex = 109;
		this.rdFromAccounts.TabStop = true;
		this.rdFromAccounts.Text = "From Accounts";
		this.rdFromAccounts.UseVisualStyleBackColor = true;
		this.rdFromExports.AutoSize = true;
		this.rdFromExports.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdFromExports.Location = new System.Drawing.Point(593, 12);
		this.rdFromExports.Name = "rdFromExports";
		this.rdFromExports.Size = new System.Drawing.Size(95, 19);
		this.rdFromExports.TabIndex = 108;
		this.rdFromExports.Text = "From Exports";
		this.rdFromExports.UseVisualStyleBackColor = true;
		this.btnshow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(523, 25);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(61, 26);
		this.btnshow.TabIndex = 6;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.cmbAccountTitle.AllowNull = (DefaultableBoolean)1;
		this.cmbAccountTitle.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbAccountTitle.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbAccountTitle.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Appearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance15).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance15).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbAccountTitle).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance15;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance16;
		((SpecialBoxBase)((UltraGridBase)this.cmbAccountTitle).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance23).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance24).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance13).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbAccountTitle).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbAccountTitle).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbAccountTitle.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbAccountTitle).Location = new System.Drawing.Point(238, 25);
		((System.Windows.Forms.Control)(object)this.cmbAccountTitle).Name = "cmbAccountTitle";
		((System.Windows.Forms.Control)(object)this.cmbAccountTitle).Size = new System.Drawing.Size(283, 26);
		((System.Windows.Forms.Control)(object)this.cmbAccountTitle).TabIndex = 5;
		this.todate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.todate.CustomFormat = "dd-MMM-yy";
		this.todate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.todate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.todate.Location = new System.Drawing.Point(125, 27);
		this.todate.Name = "todate";
		this.todate.Size = new System.Drawing.Size(111, 23);
		this.todate.TabIndex = 1;
		this.fromdate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.fromdate.CustomFormat = "dd-MMM-yy";
		this.fromdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.fromdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.fromdate.Location = new System.Drawing.Point(11, 27);
		this.fromdate.Name = "fromdate";
		this.fromdate.Size = new System.Drawing.Size(111, 23);
		this.fromdate.TabIndex = 0;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(238, 6);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(78, 15);
		this.label2.TabIndex = 105;
		this.label2.Text = "Account Title";
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(125, 6);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(48, 15);
		this.label33.TabIndex = 102;
		this.label33.Text = "To Date";
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(11, 6);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(63, 15);
		this.label27.TabIndex = 103;
		this.label27.Text = "From Date";
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(962, 607);
		this.panel1.TabIndex = 0;
		this.panel2.Controls.Add(this.toolStrip3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(962, 27);
		this.panel2.TabIndex = 3;
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnNew, this.print, this.btnPrint105C, this.btnPrint105CII });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(962, 27);
		this.toolStrip3.TabIndex = 0;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(68, 24);
		this.btnNew.Text = "&Reset";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.print.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.print.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.print.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.print.Name = "print";
		this.print.Size = new System.Drawing.Size(92, 24);
		this.print.Text = "156-Print";
		this.print.Click += new System.EventHandler(print_Click_1);
		this.btnPrint105C.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint105C.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint105C.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint105C.Name = "btnPrint105C";
		this.btnPrint105C.Size = new System.Drawing.Size(101, 24);
		this.btnPrint105C.Text = "105C-Print";
		this.btnPrint105C.Visible = false;
		this.btnPrint105C.Click += new System.EventHandler(btnPrint105C_Click);
		this.btnPrint105CII.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint105CII.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint105CII.Name = "btnPrint105CII";
		this.btnPrint105CII.Size = new System.Drawing.Size(98, 24);
		this.btnPrint105CII.Text = "105C-Print-II";
		this.btnPrint105CII.Visible = false;
		this.btnPrint105CII.Click += new System.EventHandler(btnPrint105CII_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(962, 607);
		base.Controls.Add(this.panel1);
		this.DoubleBuffered = true;
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "FcyGeneralLedgerRpt";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(FcyGeneralLedgerRpt_FormClosing);
		base.Load += new System.EventHandler(VoucherValidation_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(VoucherValidation_KeyDown);
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbAccountTitle).EndInit();
		this.panel1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		base.ResumeLayout(false);
	}
}
