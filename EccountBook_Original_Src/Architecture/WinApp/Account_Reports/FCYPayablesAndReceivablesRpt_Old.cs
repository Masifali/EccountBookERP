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
using Architecture.Reports;
using Architecture.WinApp.Common;
using Architecture.WinApp.DynamicCards;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Account_Reports;

public class FCYPayablesAndReceivablesRpt_Old : Form
{
	private DropDownBind DDL = new DropDownBind();

	private DataTable dtdata = new DataTable();

	private DataTable dtCurrency = new DataTable();

	public bool RequestedByOtherDocument;

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel3;

	private Label label23;

	private Label label27;

	private Panel panel2;

	private ToolStrip toolStrip3;

	private ToolStripButton print;

	private ToolStripButton btnNew;

	private CtrlGrdBar ctrlGrdBar1;

	internal GridEX grdPayable;

	private Button btnshow;

	internal UltraCombo CmbCurrencySelection;

	private Label label2;

	private Label label3;

	private TextBox txtexchangerate;

	private GroupBox groupBox1;

	private Panel panel4;

	private Label label1;

	private FlowLayoutPanel CurrencyFlowLayout;

	private Panel panel7;

	private CtrlGrdBar ctrlGrdBar2;

	private Label label4;

	internal GridEX grdRcvAble;

	private FlowLayoutPanel RcvAbleFlowLayout;

	private ToolStripButton btn153PayablePrint;

	public DateTimePicker todate;

	public DateTimePicker fromdate;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private TabPage tabPage2;

	public FCYPayablesAndReceivablesRpt_Old(UserAccount user)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void VoucherValidation_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1310, 700);
			if (!RequestedByOtherDocument)
			{
				fromdate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			}
			fromdate.Focus();
			CurrencySelection();
			CurrencyFlowLayout.Visible = false;
			RcvAbleFlowLayout.Visible = false;
			btnshow_Click(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CurrencySelection()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			dtCurrency = VoucherReports.GetMultiCurrencyAndLastRate(obj);
			if (dtCurrency.Rows.Count > 0)
			{
				DDL.BindDDL(dtCurrency, CmbCurrencySelection, "DMultiCurrencyId", "CurrencyName", "Currency Name", ZeroIndex: true);
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
			reset();
		}
		catch (Exception)
		{
			MessageBox.Show("Report Not Found");
		}
	}

	private void reset()
	{
		try
		{
			CurrencyFlowLayout.Visible = false;
			fromdate.Focus();
			grdPayable.ClearStructure();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void toolStripButton17_Click(object sender, EventArgs e)
	{
		try
		{
			Close();
		}
		catch (Exception)
		{
			MessageBox.Show("Report Not Found");
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

	private void FCYPayablesAndReceivablesRpt_FormClosing(object sender, FormClosingEventArgs e)
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

	private void GridFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters vh = new ReportsParameters();
			vh.OrganizationId = UserAccount.OrganizationId;
			vh.CompanyId = UserAccount.CompanyId;
			if (fromdate.Checked)
			{
				vh.FromDate = fromdate.Value;
			}
			if (todate.Checked)
			{
				vh.ToDate = todate.Value;
			}
			dtdata = VoucherReports.FCYPayablesAndReceivables_Rpt(vh);
			if (dtdata.Rows.Count > 0)
			{
				DataTable dtPayable = new DataTable();
				dtPayable.Columns.Add("Id", typeof(int));
				dtPayable.Columns.Add("CurrencyId", typeof(int));
				dtPayable.Columns.Add("AccountTitle", typeof(string));
				dtPayable.Columns.Add("FcyCode", typeof(string));
				dtPayable.Columns.Add("Debit", typeof(double));
				dtPayable.Columns.Add("Credit", typeof(double));
				dtPayable.Columns.Add("Closing", typeof(double));
				dtPayable.Columns.Add("BaseCurrency", typeof(double));
				dtPayable.Columns.Add("LastExchRate", typeof(double));
				dtPayable.Clear();
				DataTable dtRcvable = new DataTable();
				dtRcvable.Columns.Add("Id", typeof(int));
				dtRcvable.Columns.Add("CurrencyId", typeof(int));
				dtRcvable.Columns.Add("AccountTitle", typeof(string));
				dtRcvable.Columns.Add("FcyCode", typeof(string));
				dtRcvable.Columns.Add("Debit", typeof(double));
				dtRcvable.Columns.Add("Credit", typeof(double));
				dtRcvable.Columns.Add("Closing", typeof(double));
				dtRcvable.Columns.Add("BaseCurrency", typeof(double));
				dtRcvable.Columns.Add("LastExchRate", typeof(double));
				dtRcvable.Clear();
				for (int i = 0; i < dtdata.Rows.Count; i++)
				{
					double ClosingDr = 0.0;
					double ClosingCr = 0.0;
					double BaseCurrencyDr = 0.0;
					double BaseCurrencyCr = 0.0;
					double ExchRate = 0.0;
					for (int x = 0; x < dtCurrency.Rows.Count; x++)
					{
						if (Conversion.ToInt(dtCurrency.Rows[x]["DMultiCurrencyId"]) == Conversion.ToInt(dtdata.Rows[i]["CurrencyId"]))
						{
							ExchRate = Conversion.ToDouble(dtCurrency.Rows[x]["LastExchRate"]);
							break;
						}
					}
					if (Conversion.ToDouble(dtdata.Rows[i]["FcyClosingCr"]) < 0.0)
					{
						ClosingCr = Conversion.ToDouble(dtdata.Rows[i]["FcyClosingCr"]);
						BaseCurrencyCr = Conversion.ToDouble(dtdata.Rows[i]["PkrClosingCr"]);
						dtPayable.Rows.Add(Conversion.ToInt(dtdata.Rows[i]["Id"]), Conversion.ToInt(dtdata.Rows[i]["CurrencyId"]), Conversion.ToString(dtdata.Rows[i]["AccountTitle"]), Conversion.ToString(dtdata.Rows[i]["CurrencyCode"]), Conversion.ToDouble(dtdata.Rows[i]["FcyDebitAmount"]), Conversion.ToDouble(dtdata.Rows[i]["FcyCreditAmount"]), ClosingCr, BaseCurrencyCr, ExchRate);
					}
					else if (Conversion.ToDouble(dtdata.Rows[i]["FcyClosingDr"]) > 0.0)
					{
						ClosingDr = Conversion.ToDouble(dtdata.Rows[i]["FcyClosingDr"]);
						BaseCurrencyDr = Conversion.ToDouble(dtdata.Rows[i]["PkrClosingDr"]);
						dtRcvable.Rows.Add(Conversion.ToInt(dtdata.Rows[i]["Id"]), Conversion.ToInt(dtdata.Rows[i]["CurrencyId"]), Conversion.ToString(dtdata.Rows[i]["AccountTitle"]), Conversion.ToString(dtdata.Rows[i]["CurrencyCode"]), Conversion.ToDouble(dtdata.Rows[i]["FcyDebitAmount"]), Conversion.ToDouble(dtdata.Rows[i]["FcyCreditAmount"]), ClosingDr, BaseCurrencyDr, ExchRate);
					}
				}
				CardsCreate(dtPayable, CurrencyFlowLayout);
				CardsCreate(dtRcvable, RcvAbleFlowLayout);
				grdPayable.DataSource = dtPayable;
				grdPayable.RetrieveStructure();
				GridSettingPayable();
				grdRcvAble.DataSource = dtRcvable;
				grdRcvAble.RetrieveStructure();
				GridSettingRcvAble();
			}
			else
			{
				grdPayable.ClearStructure();
				grdRcvAble.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CardsCreate(DataTable dtPayableRcvable, FlowLayoutPanel layoutPanel)
	{
		try
		{
			if (dtCurrency.Rows.Count > 0)
			{
				layoutPanel.Visible = true;
				List<DataRow> varCurrency = TotalOfAllCurrencyAmounts(dtPayableRcvable).AsEnumerable().Distinct().ToList();
				layoutPanel.Controls.Clear();
				LableWithValue[] CurrCards = new LableWithValue[varCurrency.Count];
				for (int i = 0; i < CurrCards.Length; i++)
				{
					int selectedCurrencyId = 0;
					string selectedCurrency = "";
					selectedCurrencyId = Conversion.ToInt(varCurrency[i]["CurrencyId"]);
					selectedCurrency = Conversion.ToString(varCurrency[i]["CurrencyCode"]);
					double currCodeSum = Conversion.ToDouble(varCurrency[i]["TotalAmount"]);
					if (selectedCurrencyId != 0)
					{
						layoutPanel.Text = "Total Amount in " + selectedCurrency + ": " + currCodeSum.ToString(clsGlobalVariables.stringFormatbothForFcy);
						CurrCards[i] = new LableWithValue();
						CurrCards[i].Amount = currCodeSum.ToString(clsGlobalVariables.stringFormatbothForFcy);
						CurrCards[i].Currency = selectedCurrency;
						layoutPanel.Controls.Add(CurrCards[i]);
					}
				}
				if (CurrCards.Length > 3)
				{
					layoutPanel.Height = 136;
				}
				else
				{
					layoutPanel.Height = 68;
				}
			}
			else
			{
				layoutPanel.Controls.Clear();
				layoutPanel.Visible = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSettingPayable()
	{
		try
		{
			grdPayable.RootTable.Columns["Id"].Visible = false;
			grdPayable.RootTable.Columns["CurrencyId"].Visible = false;
			grdPayable.RootTable.Columns["Debit"].AggregateFunction = (AggregateFunction)2;
			grdPayable.RootTable.Columns["Debit"].TextAlignment = (TextAlignment)3;
			grdPayable.RootTable.Columns["Debit"].FormatString = clsGlobalVariables.stringFormatbothForFcy;
			grdPayable.RootTable.Columns["Debit"].TotalFormatString = clsGlobalVariables.stringFormatbothForFcy;
			grdPayable.RootTable.Columns["Credit"].AggregateFunction = (AggregateFunction)2;
			grdPayable.RootTable.Columns["Credit"].TextAlignment = (TextAlignment)3;
			grdPayable.RootTable.Columns["Credit"].FormatString = clsGlobalVariables.stringFormatbothForFcy;
			grdPayable.RootTable.Columns["Credit"].TotalFormatString = clsGlobalVariables.stringFormatbothForFcy;
			grdPayable.RootTable.Columns["Closing"].AggregateFunction = (AggregateFunction)2;
			grdPayable.RootTable.Columns["Closing"].TextAlignment = (TextAlignment)3;
			grdPayable.RootTable.Columns["Closing"].FormatString = clsGlobalVariables.stringFormatbothForFcy;
			grdPayable.RootTable.Columns["Closing"].TotalFormatString = clsGlobalVariables.stringFormatbothForFcy;
			grdPayable.RootTable.Columns["BaseCurrency"].AggregateFunction = (AggregateFunction)2;
			grdPayable.RootTable.Columns["BaseCurrency"].TextAlignment = (TextAlignment)3;
			grdPayable.RootTable.Columns["BaseCurrency"].FormatString = clsGlobalVariables.stringFormatbothForFcy;
			grdPayable.RootTable.Columns["BaseCurrency"].TotalFormatString = clsGlobalVariables.stringFormatbothForFcy;
			grdPayable.RootTable.Columns["LastExchRate"].TextAlignment = (TextAlignment)3;
			grdPayable.RootTable.Columns["LastExchRate"].FormatString = clsGlobalVariables.DecimalFCYRateFormate;
			grdPayable.RootTable.Columns["LastExchRate"].TotalFormatString = clsGlobalVariables.DecimalFCYRateFormate;
			grdPayable.RootTable.Columns["AccountTitle"].ColumnType = (ColumnType)5;
			grdPayable.RootTable.Groups.Add("FcyCode");
			grdPayable.RootTable.HideColumnsWhenGrouped = (InheritableBoolean)1;
			grdPayable.RootTable.Columns["LastExchRate"].Caption = "Ex.Rate";
			CommonServices.GridAutoAdjustmentNew(grdPayable);
			grdPayable.RootTable.Columns["AccountTitle"].Width = 205;
			grdPayable.RootTable.Columns["LastExchRate"].Width = 70;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSettingRcvAble()
	{
		try
		{
			grdRcvAble.RootTable.Columns["Id"].Visible = false;
			grdRcvAble.RootTable.Columns["CurrencyId"].Visible = false;
			grdRcvAble.RootTable.Columns["Debit"].AggregateFunction = (AggregateFunction)2;
			grdRcvAble.RootTable.Columns["Debit"].TextAlignment = (TextAlignment)3;
			grdRcvAble.RootTable.Columns["Debit"].FormatString = clsGlobalVariables.stringFormatbothForFcy;
			grdRcvAble.RootTable.Columns["Debit"].TotalFormatString = clsGlobalVariables.stringFormatbothForFcy;
			grdRcvAble.RootTable.Columns["Credit"].AggregateFunction = (AggregateFunction)2;
			grdRcvAble.RootTable.Columns["Credit"].TextAlignment = (TextAlignment)3;
			grdRcvAble.RootTable.Columns["Credit"].FormatString = clsGlobalVariables.stringFormatbothForFcy;
			grdRcvAble.RootTable.Columns["Credit"].TotalFormatString = clsGlobalVariables.stringFormatbothForFcy;
			grdRcvAble.RootTable.Columns["Closing"].AggregateFunction = (AggregateFunction)2;
			grdRcvAble.RootTable.Columns["Closing"].TextAlignment = (TextAlignment)3;
			grdRcvAble.RootTable.Columns["Closing"].FormatString = clsGlobalVariables.stringFormatbothForFcy;
			grdRcvAble.RootTable.Columns["Closing"].TotalFormatString = clsGlobalVariables.stringFormatbothForFcy;
			grdRcvAble.RootTable.Columns["BaseCurrency"].AggregateFunction = (AggregateFunction)2;
			grdRcvAble.RootTable.Columns["BaseCurrency"].TextAlignment = (TextAlignment)3;
			grdRcvAble.RootTable.Columns["BaseCurrency"].FormatString = clsGlobalVariables.stringFormatbothForFcy;
			grdRcvAble.RootTable.Columns["BaseCurrency"].TotalFormatString = clsGlobalVariables.stringFormatbothForFcy;
			grdRcvAble.RootTable.Columns["LastExchRate"].TextAlignment = (TextAlignment)3;
			grdRcvAble.RootTable.Columns["LastExchRate"].FormatString = clsGlobalVariables.DecimalFCYRateFormate;
			grdRcvAble.RootTable.Columns["LastExchRate"].TotalFormatString = clsGlobalVariables.DecimalFCYRateFormate;
			grdRcvAble.RootTable.Columns["AccountTitle"].ColumnType = (ColumnType)5;
			grdRcvAble.RootTable.Groups.Add("FcyCode");
			grdRcvAble.RootTable.HideColumnsWhenGrouped = (InheritableBoolean)1;
			grdRcvAble.RootTable.Columns["LastExchRate"].Caption = "Ex.Rate";
			CommonServices.GridAutoAdjustmentNew(grdRcvAble);
			grdRcvAble.RootTable.Columns["AccountTitle"].Width = 205;
			grdRcvAble.RootTable.Columns["LastExchRate"].Width = 70;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdPayable.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				CommonServices.GoToFcyGeneralLedgerFromLinkedEvent(Conversion.ToInt(item.Cells["Id"].Value), fromdate.Value, todate.Value);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void print_Click_1(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = VoucherReports.FCYPayablesAndReceivables_Rpt(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FromDate = fromdate.Value,
				ToDate = todate.Value,
				ReportTypeId = 1
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "153-FCYPayablesAndReceivablesRpt.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
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

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdPayable).Name);
			if (fs != null)
			{
				grdPayable.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private DataTable TotalOfAllCurrencyAmounts(DataTable dtPayableRcvable)
	{
		DataTable dt = new DataTable();
		dt.Columns.Add("CurrencyId");
		dt.Columns.Add("CurrencyCode");
		dt.Columns.Add("TotalAmount");
		try
		{
			if (dtCurrency.Rows.Count > 0)
			{
				for (int j = 0; j < dtCurrency.Rows.Count; j++)
				{
					if (Conversion.ToInt(dtCurrency.Rows[j]["DMultiCurrencyId"]) > 0)
					{
						int currencyId = Conversion.ToInt(dtCurrency.Rows[j]["DMultiCurrencyId"]);
						string currencyCode = Conversion.ToString(dtCurrency.Rows[j]["CurrencyName"]);
						double sumOfCurrency = (from row in dtPayableRcvable.AsEnumerable()
							where row.Field<int>("CurrencyId") == currencyId
							select row).Sum((DataRow row) => row.Field<double>("Closing"));
						if (currencyId != 0 && sumOfCurrency != 0.0)
						{
							dt.Rows.Add(currencyId, currencyCode, sumOfCurrency);
						}
					}
				}
			}
			double SumOFAllAmountIntoBaseCurrency = 0.0;
			if (dt.Rows.Count > 0)
			{
				int i;
				for (i = 0; i < dt.Rows.Count; i++)
				{
					double CurrentcurrencyExchRate = (from row in dtCurrency.AsEnumerable()
						where row.Field<int>("DMultiCurrencyId") == Conversion.ToInt(dt.Rows[i]["CurrencyId"])
						select row).Sum((DataRow row) => row.Field<double>("LastExchRate"));
					SumOFAllAmountIntoBaseCurrency += Conversion.ToDouble(dt.Rows[i]["TotalAmount"]) * CurrentcurrencyExchRate;
				}
			}
			DataTable dt2 = CommonServices.GetBaseCurrencyInfo();
			double ExchRateBaseCurrency = 0.0;
			int BaseCurrencyId = Conversion.ToInt(dt2.Rows[0]["BaseCurrencyId"]);
			string BaseCurrency = "Base Currency " + Conversion.ToString(dt2.Rows[0]["CurrencyCode"]);
			if (SumOFAllAmountIntoBaseCurrency != 0.0)
			{
				dt.Rows.Add(BaseCurrencyId, BaseCurrency, SumOFAllAmountIntoBaseCurrency);
			}
			if (((UltraGridBase)CmbCurrencySelection).ActiveRow != null && Conversion.ToInt(CmbCurrencySelection.Value) != 0)
			{
				BaseCurrencyId = Conversion.ToInt(CmbCurrencySelection.Value);
				BaseCurrency = "Total In " + Conversion.ToString((object)((Control)(object)CmbCurrencySelection).Text);
				ExchRateBaseCurrency = Conversion.ToDouble(((UltraDropDownBase)CmbCurrencySelection).SelectedRow.Cells["LastExchRate"].Value);
				dt.Rows.Add(BaseCurrencyId, BaseCurrency, SumOFAllAmountIntoBaseCurrency / ExchRateBaseCurrency);
			}
			ExchRateBaseCurrency = Conversion.ToDouble(dt2.Rows[0]["BaseCurrencyRate"]);
			if (ExchRateBaseCurrency != 0.0)
			{
				txtexchangerate.Text = ExchRateBaseCurrency.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		return dt;
	}

	private void txtexchangerate_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdRcvAble).Name);
			if (fs != null)
			{
				grdRcvAble.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btn153PayablePrint_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = VoucherReports.FCYPayablesAndReceivables_Rpt(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FromDate = fromdate.Value,
				ToDate = todate.Value,
				ReportTypeId = 2
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("No Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "153-FCYPayablesAndReceivablesRpt.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdRcvAble_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdRcvAble.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				CommonServices.GoToFcyGeneralLedgerFromLinkedEvent(Conversion.ToInt(item.Cells["Id"].Value), fromdate.Value, todate.Value);
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
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Expected O, but got Unknown
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Expected O, but got Unknown
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Expected O, but got Unknown
		//IL_0e12: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e1c: Expected O, but got Unknown
		//IL_11fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1206: Expected O, but got Unknown
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
		GridEXLayout grdPayable_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdRcvAble_DesignTimeLayout = new GridEXLayout();
		this.panel3 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.label27 = new System.Windows.Forms.Label();
		this.fromdate = new System.Windows.Forms.DateTimePicker();
		this.todate = new System.Windows.Forms.DateTimePicker();
		this.label3 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.txtexchangerate = new System.Windows.Forms.TextBox();
		this.CmbCurrencySelection = new UltraCombo();
		this.btnshow = new System.Windows.Forms.Button();
		this.label23 = new System.Windows.Forms.Label();
		this.grdPayable = new GridEX();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.print = new System.Windows.Forms.ToolStripButton();
		this.btn153PayablePrint = new System.Windows.Forms.ToolStripButton();
		this.grdRcvAble = new GridEX();
		this.RcvAbleFlowLayout = new System.Windows.Forms.FlowLayoutPanel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label1 = new System.Windows.Forms.Label();
		this.CurrencyFlowLayout = new System.Windows.Forms.FlowLayoutPanel();
		this.panel7 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label4 = new System.Windows.Forms.Label();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel3.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCurrencySelection).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.grdPayable).BeginInit();
		this.panel2.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdRcvAble).BeginInit();
		this.panel4.SuspendLayout();
		this.panel7.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		base.SuspendLayout();
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add(this.groupBox1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 54);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1285, 48);
		this.panel3.TabIndex = 4;
		this.groupBox1.Controls.Add(this.fromdate);
		this.groupBox1.Controls.Add(this.todate);
		this.groupBox1.Controls.Add(this.label27);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.txtexchangerate);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCurrencySelection);
		this.groupBox1.Controls.Add(this.btnshow);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(3, 1);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1132, 46);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(5, 20);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(91, 15);
		this.label27.TabIndex = 103;
		this.label27.Text = "Date From && To";
		this.fromdate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.fromdate.CustomFormat = "dd-MMM-yy";
		this.fromdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.fromdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.fromdate.Location = new System.Drawing.Point(96, 16);
		this.fromdate.Name = "fromdate";
		this.fromdate.ShowCheckBox = true;
		this.fromdate.Size = new System.Drawing.Size(151, 23);
		this.fromdate.TabIndex = 0;
		this.todate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.todate.CustomFormat = "dd-MMM-yy";
		this.todate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.todate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.todate.Location = new System.Drawing.Point(249, 16);
		this.todate.Name = "todate";
		this.todate.ShowCheckBox = true;
		this.todate.Size = new System.Drawing.Size(153, 23);
		this.todate.TabIndex = 1;
		this.label3.AutoSize = true;
		this.label3.BackColor = System.Drawing.Color.White;
		this.label3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.Black;
		this.label3.Location = new System.Drawing.Point(738, 19);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(209, 16);
		this.label3.TabIndex = 108;
		this.label3.Text = "Base Currency Exhchange Rate";
		this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(404, 20);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(106, 15);
		this.label2.TabIndex = 106;
		this.label2.Text = "Currency Selection";
		this.txtexchangerate.Enabled = false;
		this.txtexchangerate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtexchangerate.Location = new System.Drawing.Point(952, 16);
		this.txtexchangerate.Name = "txtexchangerate";
		this.txtexchangerate.ShortcutsEnabled = false;
		this.txtexchangerate.Size = new System.Drawing.Size(111, 23);
		this.txtexchangerate.TabIndex = 3;
		this.txtexchangerate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtexchangerate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtexchangerate_KeyPress);
		this.CmbCurrencySelection.AllowNull = (DefaultableBoolean)1;
		this.CmbCurrencySelection.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCurrencySelection.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCurrencySelection.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCurrencySelection).DisplayLayout.Appearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.CmbCurrencySelection).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCurrencySelection).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance20).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance20).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCurrencySelection).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCurrencySelection).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance21;
		((SpecialBoxBase)((UltraGridBase)this.CmbCurrencySelection).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance22).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance22).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCurrencySelection).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbCurrencySelection).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCurrencySelection).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCurrencySelection).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCurrencySelection).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbCurrencySelection).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCurrencySelection).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCurrencySelection).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance26).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCurrencySelection).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbCurrencySelection).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCurrencySelection).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCurrencySelection).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCurrencySelection).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbCurrencySelection).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCurrencySelection).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCurrencySelection).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbCurrencySelection).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCurrencySelection).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbCurrencySelection).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCurrencySelection).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCurrencySelection).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCurrencySelection).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCurrencySelection.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCurrencySelection).Location = new System.Drawing.Point(512, 14);
		((System.Windows.Forms.Control)(object)this.CmbCurrencySelection).Name = "CmbCurrencySelection";
		((System.Windows.Forms.Control)(object)this.CmbCurrencySelection).Size = new System.Drawing.Size(222, 26);
		((System.Windows.Forms.Control)(object)this.CmbCurrencySelection).TabIndex = 2;
		this.btnshow.BackColor = System.Drawing.Color.Teal;
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(1068, 14);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(61, 26);
		this.btnshow.TabIndex = 4;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(3, 31);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(257, 19);
		this.label23.TabIndex = 1;
		this.label23.Text = "FCY Payables And Receivables";
		this.grdPayable.AllowEdit = (InheritableBoolean)2;
		this.grdPayable.AlternatingColors = true;
		grdPayable_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPayable.DesignTimeLayout = grdPayable_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPayable).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdPayable.DynamicFiltering = true;
		this.grdPayable.FilterMode = (FilterMode)2;
		this.grdPayable.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdPayable).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPayable.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdPayable).Location = new System.Drawing.Point(3, 97);
		((System.Windows.Forms.Control)(object)this.grdPayable).Name = "grdPayable";
		((ScrollableBase)this.grdPayable).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPayable).Size = new System.Drawing.Size(1271, 430);
		((System.Windows.Forms.Control)(object)this.grdPayable).TabIndex = 0;
		this.grdPayable.TotalRow = (InheritableBoolean)1;
		this.grdPayable.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPayable).VisualStyle = (VisualStyle)4;
		this.grdPayable.LinkClicked += new ColumnActionEventHandler(grd_LinkClicked);
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.label23);
		this.panel2.Controls.Add(this.toolStrip3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1285, 54);
		this.panel2.TabIndex = 3;
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnNew, this.print, this.btn153PayablePrint });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1285, 27);
		this.toolStrip3.TabIndex = 0;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(59, 24);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.print.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.print.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.print.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.print.Name = "print";
		this.print.Size = new System.Drawing.Size(139, 24);
		this.print.Text = "153-Receivables";
		this.print.Click += new System.EventHandler(print_Click_1);
		this.btn153PayablePrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btn153PayablePrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btn153PayablePrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btn153PayablePrint.Name = "btn153PayablePrint";
		this.btn153PayablePrint.Size = new System.Drawing.Size(120, 24);
		this.btn153PayablePrint.Text = "153-Payables";
		this.btn153PayablePrint.Click += new System.EventHandler(btn153PayablePrint_Click);
		this.grdRcvAble.AllowEdit = (InheritableBoolean)2;
		this.grdRcvAble.AlternatingColors = true;
		grdRcvAble_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdRcvAble.DesignTimeLayout = grdRcvAble_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdRcvAble).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdRcvAble.DynamicFiltering = true;
		this.grdRcvAble.FilterMode = (FilterMode)2;
		this.grdRcvAble.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdRcvAble).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdRcvAble.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdRcvAble).Location = new System.Drawing.Point(3, 97);
		((System.Windows.Forms.Control)(object)this.grdRcvAble).Name = "grdRcvAble";
		((ScrollableBase)this.grdRcvAble).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdRcvAble).Size = new System.Drawing.Size(1271, 430);
		((System.Windows.Forms.Control)(object)this.grdRcvAble).TabIndex = 1;
		this.grdRcvAble.TotalRow = (InheritableBoolean)1;
		this.grdRcvAble.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdRcvAble).VisualStyle = (VisualStyle)4;
		this.grdRcvAble.LinkClicked += new ColumnActionEventHandler(grdRcvAble_LinkClicked);
		this.RcvAbleFlowLayout.AutoScroll = true;
		this.RcvAbleFlowLayout.BackColor = System.Drawing.Color.White;
		this.RcvAbleFlowLayout.Dock = System.Windows.Forms.DockStyle.Top;
		this.RcvAbleFlowLayout.Location = new System.Drawing.Point(3, 29);
		this.RcvAbleFlowLayout.Name = "RcvAbleFlowLayout";
		this.RcvAbleFlowLayout.Size = new System.Drawing.Size(1271, 68);
		this.RcvAbleFlowLayout.TabIndex = 0;
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.ctrlGrdBar2);
		this.panel4.Controls.Add(this.label1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(3, 3);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1271, 26);
		this.panel4.TabIndex = 0;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1232, 2);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdRcvAble;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar2.TabIndex = 20;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(3, 2);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(106, 19);
		this.label1.TabIndex = 2;
		this.label1.Text = "Receivables";
		this.CurrencyFlowLayout.AutoScroll = true;
		this.CurrencyFlowLayout.BackColor = System.Drawing.Color.White;
		this.CurrencyFlowLayout.Dock = System.Windows.Forms.DockStyle.Top;
		this.CurrencyFlowLayout.Location = new System.Drawing.Point(3, 29);
		this.CurrencyFlowLayout.Name = "CurrencyFlowLayout";
		this.CurrencyFlowLayout.Size = new System.Drawing.Size(1271, 68);
		this.CurrencyFlowLayout.TabIndex = 1;
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.ctrlGrdBar1);
		this.panel7.Controls.Add(this.label4);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(3, 3);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1271, 26);
		this.panel7.TabIndex = 1;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1230, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdPayable;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 20;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.White;
		this.label4.Location = new System.Drawing.Point(3, 2);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(82, 19);
		this.label4.TabIndex = 2;
		this.label4.Text = "Payables";
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 102);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1285, 559);
		this.tabControl1.TabIndex = 115;
		this.tabPage1.Controls.Add((System.Windows.Forms.Control)(object)this.grdRcvAble);
		this.tabPage1.Controls.Add(this.RcvAbleFlowLayout);
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Location = new System.Drawing.Point(4, 25);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1277, 530);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Receivables";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.grdPayable);
		this.tabPage2.Controls.Add(this.CurrencyFlowLayout);
		this.tabPage2.Controls.Add(this.panel7);
		this.tabPage2.Location = new System.Drawing.Point(4, 25);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1277, 530);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "Payables";
		this.tabPage2.UseVisualStyleBackColor = true;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1285, 661);
		base.Controls.Add(this.tabControl1);
		base.Controls.Add(this.panel3);
		base.Controls.Add(this.panel2);
		this.DoubleBuffered = true;
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "FCYPayablesAndReceivablesRpt";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(FCYPayablesAndReceivablesRpt_FormClosing);
		base.Load += new System.EventHandler(VoucherValidation_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(VoucherValidation_KeyDown);
		this.panel3.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCurrencySelection).EndInit();
		((System.ComponentModel.ISupportInitialize)this.grdPayable).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdRcvAble).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.tabPage2.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}
