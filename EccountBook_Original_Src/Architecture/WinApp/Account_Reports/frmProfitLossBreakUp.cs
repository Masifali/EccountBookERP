using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using Architecture.BLL.Accounts;
using Architecture.BLL.Reports.Accounts;
using Architecture.Common;
using Architecture.Model;
using Architecture.Reports;
using Architecture.WinApp.Common;
using Architecture.WinApp.ExportReports;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Inventory_Definition;
using Architecture.WinApp.Inventory_Reports;
using Architecture.WinApp.Inventory_Stocks_Report;
using Architecture.WinApp.Production_Reports;
using Architecture.WinApp.Properties;
using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Account_Reports;

public class frmProfitLossBreakUp : Form
{
	public bool RequestFromAnotherForm;

	public bool FormInitialized;

	public DateTime? FromDate;

	public DateTime? ToDate;

	private bool BranchFeature;

	private bool BranchFeatureConsolidated;

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtRegister = new DataTable();

	private DataTable dtGridForLayout = new DataTable();

	private WaitForm waitForm = new WaitForm();

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel1;

	private Panel panel4;

	private Panel panel3;

	private Panel panel2;

	private ToolStrip toolStrip3;

	private ToolStripButton btnNew;

	private ToolStripButton btnPrint;

	private CtrlGrdBar ctrlGrdBar1;

	internal GridEX DataGridHistory;

	private Panel panel5;

	private Label lblCaption;

	private ToolStripButton btnRefresh;

	private ToolStripButton btnShortcut;

	private Panel panel6;

	private Label label4;

	private GroupBox groupBox1;

	private DateTimePicker datFromDate;

	private Label label1;

	private Button btnShow;

	public UltraCombo CmbParentAccount;

	private Label label12;

	private Label lblBranch;

	internal UltraCombo cmbbranch;

	private DateTimePicker datToDate;

	private Label label27;

	private RadioButton RadSummary;

	private RadioButton RadDetail;

	public frmProfitLossBreakUp(UserAccount userAccount)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = userAccount;
		InitializeComponentCustom();
		InitializeComponentMethod();
	}

	private void InitializeComponentCustom()
	{
	}

	private void InitializeComponentMethod()
	{
		Task.Run(() => new
		{
			ParentData = ParentAccountData()
		}).ContinueWith(t =>
		{
			if (t.IsFaulted)
			{
				MessageBox.Show("Error occurred during database call.");
			}
			else
			{
				var result = t.Result;
				BranchFeature = CommonServices.GetERPFeatureById(17);
				BranchFeatureConsolidated = CommonServices.GetERPFeatureById(18);
				if (!BranchFeature)
				{
					lblBranch.Visible = false;
					((Control)(object)cmbbranch).Visible = false;
				}
				else
				{
					BranchFill();
				}
				BindParentAccountCombo(result.ParentData);
				if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime((object)FromDate)) && !Conversion.CheckDateTimeNull(Conversion.ToDateTime((object)ToDate)))
				{
					datFromDate.Value = Conversion.ToDateTime((object)FromDate);
					datToDate.Value = Conversion.ToDateTime((object)ToDate);
					GridFill();
				}
				datFromDate.Focus();
			}
		}, TaskScheduler.FromCurrentSynchronizationContext());
	}

	private void frmEvaulationDetailSalesReports_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1230, 800);
		}
		catch (Exception ex)
		{
			waitForm.Close();
			MessageBox.Show(ex.Message);
		}
		finally
		{
			waitForm.Close();
		}
	}

	private void BranchFill()
	{
		try
		{
			DataTable dt = VoucherHead.GetBranchesFromVouchersByAccountId(UserAccount.OrganizationId, UserAccount.CompanyId, "", 0);
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbbranch, "Id", "BranchName", "Branch Name", ZeroIndex: false);
				if (BranchFeature && BranchFeatureConsolidated)
				{
					UltraGridColumn obj = ((UltraGridBase)cmbbranch).DisplayLayout.Bands[0].Columns.Add();
					((KeyedSubObjectBase)obj).Key = "Selected";
					((HeaderBase)obj.Header).Caption = string.Empty;
					obj.Header.CheckBoxVisibility = (HeaderCheckBoxVisibility)2;
					obj.DataType = typeof(bool);
					((HeaderBase)obj.Header).VisiblePosition = 0;
					cmbbranch.LimitToList = false;
					cmbbranch.CheckedListSettings.CheckStateMember = "Selected";
					((EditorCheckedListSettings)cmbbranch.CheckedListSettings).EditorValueSource = (EditorWithComboValueSource)1;
					((EditorCheckedListSettings)cmbbranch.CheckedListSettings).ListSeparator = ",";
					((EditorCheckedListSettings)cmbbranch.CheckedListSettings).ItemCheckArea = (ItemCheckArea)1;
				}
				((Control)(object)cmbbranch).Text = UserAccount.BranchName;
			}
			else
			{
				((Control)(object)cmbbranch).Text = string.Empty;
				((UltraGridBase)cmbbranch).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private DataTable ParentAccountData()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Expected O, but got Unknown
		try
		{
			return ChartofAccount.ReadAllAccountgroup(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				Account_Level = 3,
				AccountTypeId = 4
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BindParentAccountCombo(DataTable data)
	{
		try
		{
			if (data.Rows.Count > 0)
			{
				DataTable dt1 = new DataTable();
				dt1.Columns.Add("Id");
				dt1.Columns.Add("AccountTitle");
				for (int i = 0; i < data.Rows.Count; i++)
				{
					dt1.Rows.Add(data.Rows[i]["Id"], data.Rows[i]["AccountTitle"]);
				}
				DDL.BindDDL(dt1, CmbParentAccount, "Id", "AccountTitle", "Parent Account", ZeroIndex: false);
			}
			else
			{
				((Control)(object)CmbParentAccount).Text = string.Empty;
				((UltraGridBase)CmbParentAccount).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnShow_Click(object sender, EventArgs e)
	{
		try
		{
			waitForm.Show(this);
			GridFill();
		}
		catch (Exception ex)
		{
			waitForm.Close();
			MessageBox.Show(ex.Message);
		}
		finally
		{
			waitForm.Close();
		}
	}

	private void GridFill()
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		try
		{
			dtGridForLayout.Rows.Clear();
			dtRegister.Rows.Clear();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.FromDate = datFromDate.Value;
			obj.ToDate = datToDate.Value;
			obj.ParentAccountId = Conversion.ToInt(CmbParentAccount.Value);
			obj.ActionId = (RadDetail.Checked ? 1 : 2);
			dtRegister = VoucherReports.Accounts_COGS_BreakUp(obj);
			if (dtRegister.Rows.Count == 0)
			{
				DataGridHistory.ClearStructure();
				return;
			}
			DatatableHelper.ReplaceCompLogoColumn(dtRegister, clsGlobalVariables.dtCompLogo);
			DataTable dt = new DataTable();
			dt.Columns.Add("ParentCodeId", typeof(int));
			dt.Columns.Add("ParentAccount", typeof(string));
			dt.Columns.Add("AccountId", typeof(int));
			dt.Columns.Add("AccountTitle", typeof(string));
			dt.Columns.Add("ItemId", typeof(int));
			dt.Columns.Add("ItemName", typeof(string));
			dt.Columns.Add("Debit", typeof(decimal));
			dt.Columns.Add("Credit", typeof(decimal));
			dt.Columns.Add("Opening", typeof(decimal));
			dt.Columns.Add("Purchase", typeof(decimal));
			dt.Columns.Add("ProductionOutPut", typeof(decimal));
			dt.Columns.Add("ConversionOutPut", typeof(decimal));
			dt.Columns.Add("SettlementOutPut", typeof(decimal));
			dt.Columns.Add("SaleReturn", typeof(decimal));
			dt.Columns.Add("AvailableForUse", typeof(decimal));
			dt.Columns.Add("SalesExport", typeof(decimal));
			dt.Columns.Add("SalesDirect", typeof(decimal));
			dt.Columns.Add("SalesRegular", typeof(decimal));
			dt.Columns.Add("ProductionInput", typeof(decimal));
			dt.Columns.Add("ConversionInput", typeof(decimal));
			dt.Columns.Add("SettlementInPut", typeof(decimal));
			dt.Columns.Add("ClosingStock", typeof(decimal));
			dt.Columns.Add("CogsAmount", typeof(decimal));
			for (int i = 0; i < dtRegister.Rows.Count; i++)
			{
				dt.Rows.Add(dtRegister.Rows[i]["ParentCodeId"], dtRegister.Rows[i]["ParentAccount"], dtRegister.Rows[i]["AccountId"], dtRegister.Rows[i]["AccountTitle"], Conversion.ToInt(dtRegister.Rows[i]["ItemId"]), Conversion.ToString(dtRegister.Rows[i]["ItemName"]), dtRegister.Rows[i]["Debit"], dtRegister.Rows[i]["Credit"], dtRegister.Rows[i]["Opening"], dtRegister.Rows[i]["Purchase"], dtRegister.Rows[i]["ProductionOutPut"], dtRegister.Rows[i]["ConversionOutPut"], dtRegister.Rows[i]["SettlementOutPut"], dtRegister.Rows[i]["SaleReturn"], dtRegister.Rows[i]["AvailableForUse"], dtRegister.Rows[i]["SalesExport"], dtRegister.Rows[i]["SalesDirect"], dtRegister.Rows[i]["SalesRegular"], dtRegister.Rows[i]["ProductionInput"], dtRegister.Rows[i]["ConversionInput"], dtRegister.Rows[i]["SettlementInPut"], dtRegister.Rows[i]["ClosingStock"], dtRegister.Rows[i]["CogsAmount"]);
			}
			dtGridForLayout = dt.Copy();
			DataGridHistory.DataSource = dt;
			DataGridHistory.RetrieveStructure();
			DataGridHistorySetting();
		}
		catch (Exception ex)
		{
			waitForm.Close();
			MessageBox.Show(ex.Message);
		}
		finally
		{
			waitForm.Close();
		}
	}

	private void DataGridHistorySetting()
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "ItemId", "AccountId", "ParentCodeId", "Debit", "Credit" };
			List<string> WrapColumns = new List<string> { "ParentAccount", "AccountTitle", "ItemName" };
			GridEX_Helper.GridWrappingAndColumnSettings(DataGridHistory, 2, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, hiddenColumns, null, WrapColumns);
			DataGridHistory.RootTable.Columns["ItemName"].Visible = RadDetail.Checked;
			DataGridHistory.RootTable.RowHeight = 30;
			DataGridHistory.RootTable.Columns["Opening"].ColumnType = (ColumnType)5;
			DataGridHistory.RootTable.Columns["Purchase"].ColumnType = (ColumnType)5;
			DataGridHistory.RootTable.Columns["ProductionOutPut"].ColumnType = (ColumnType)5;
			DataGridHistory.RootTable.Columns["SaleReturn"].ColumnType = (ColumnType)5;
			DataGridHistory.RootTable.Columns["SalesExport"].ColumnType = (ColumnType)5;
			DataGridHistory.RootTable.Columns["SalesDirect"].ColumnType = (ColumnType)5;
			DataGridHistory.RootTable.Columns["SalesRegular"].ColumnType = (ColumnType)5;
			DataGridHistory.RootTable.Columns["ProductionInput"].ColumnType = (ColumnType)5;
			DataGridHistory.RootTable.Columns["ClosingStock"].ColumnType = (ColumnType)5;
			DataGridHistory.RootTable.Columns["ParentAccount"].Width = Constants.InventoryConstants.AccountTitle;
			DataGridHistory.RootTable.Columns["AccountTitle"].Width = Constants.InventoryConstants.AccountTitle + 20;
			DataGridHistory.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName + 20;
			DataGridHistory.RootTable.Columns["Debit"].Width = Constants.InventoryConstants.AmountConstant;
			DataGridHistory.RootTable.Columns["Credit"].Width = Constants.InventoryConstants.AmountConstant;
			DataGridHistory.RootTable.Columns["Opening"].Width = Constants.InventoryConstants.AmountConstant;
			DataGridHistory.RootTable.Columns["Purchase"].Width = Constants.InventoryConstants.AmountConstant;
			DataGridHistory.RootTable.Columns["ProductionOutPut"].Width = Constants.InventoryConstants.AmountConstant;
			DataGridHistory.RootTable.Columns["ConversionOutPut"].Width = Constants.InventoryConstants.AmountConstant;
			DataGridHistory.RootTable.Columns["SettlementOutPut"].Width = Constants.InventoryConstants.AmountConstant;
			DataGridHistory.RootTable.Columns["SaleReturn"].Width = Constants.InventoryConstants.AmountConstant;
			DataGridHistory.RootTable.Columns["AvailableForUse"].Width = Constants.InventoryConstants.AmountForReportConstant;
			DataGridHistory.RootTable.Columns["SalesExport"].Width = Constants.InventoryConstants.AmountForReportConstant;
			DataGridHistory.RootTable.Columns["SalesDirect"].Width = Constants.InventoryConstants.AmountConstant;
			DataGridHistory.RootTable.Columns["SalesRegular"].Width = Constants.InventoryConstants.AmountConstant;
			DataGridHistory.RootTable.Columns["ProductionInput"].Width = Constants.InventoryConstants.AmountConstant + 15;
			DataGridHistory.RootTable.Columns["ConversionInput"].Width = Constants.InventoryConstants.AmountConstant;
			DataGridHistory.RootTable.Columns["SettlementInPut"].Width = Constants.InventoryConstants.AmountConstant;
			DataGridHistory.RootTable.Columns["ClosingStock"].Width = Constants.InventoryConstants.AmountConstant + 15;
			DataGridHistory.RootTable.Columns["CogsAmount"].Width = Constants.InventoryConstants.AmountConstant + 20;
			((Control)(object)DataGridHistory).Refresh();
			if (RadDetail.Checked)
			{
				DataGridHistory.RootTable.Groups.Add("ParentAccount");
				DataGridHistory.HideColumnsWhenGrouped = (InheritableBoolean)1;
			}
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void DataGridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
	}

	private void DataGridHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow row = DataGridHistory.CurrentRow;
			if (row == null || (int)row.RowType != 0)
			{
				return;
			}
			int itemId = Conversion.ToInt(row.Cells["ItemId"].Value);
			int accountId = Conversion.ToInt(row.Cells["AccountId"].Value);
			DateTime fromDate = Conversion.ToDateTime((object)datFromDate.Value);
			DateTime toDate = Conversion.ToDateTime((object)datToDate.Value);
			if (e.Column.Key == "AccountTitle")
			{
				CommonServices.GoToGeneralLedgerFromLinkedEvent(accountId, clsGlobalVariables.ActiveYr.Start_Period, datFromDate.Value);
				return;
			}
			string key = e.Column.Key;
			if (key == null)
			{
				return;
			}
			switch (key.Length)
			{
			case 16:
				switch (key[0])
				{
				case 'P':
					if (key == "ProductionOutPut" && Conversion.ToDouble(row.Cells["ProductionOutPut"].Value) != 0.0)
					{
						OpenProductionRegister(fromDate, toDate, accountId, itemId, "OutPut By Product,OutPut Finish Goods");
					}
					break;
				case 'C':
					if (key == "ConversionOutPut")
					{
					}
					break;
				case 'S':
					if (key == "SettlementOutPut")
					{
					}
					break;
				}
				break;
			case 15:
				switch (key[0])
				{
				case 'P':
					if (key == "ProductionInput" && Conversion.ToDouble(row.Cells["ProductionInput"].Value) != 0.0)
					{
						OpenProductionRegister(fromDate, toDate, accountId, itemId, "Material Input");
					}
					break;
				case 'C':
					if (key == "ConversionInput")
					{
					}
					break;
				case 'S':
					if (key == "SettlementInPut")
					{
					}
					break;
				}
				break;
			case 11:
				switch (key[5])
				{
				case 'E':
					if (key == "SalesExport" && Conversion.ToDouble(row.Cells["SalesExport"].Value) != 0.0)
					{
						OpenExportRegister(fromDate, toDate, accountId, itemId);
					}
					break;
				case 'D':
					if (key == "SalesDirect" && Conversion.ToDouble(row.Cells["SalesDirect"].Value) != 0.0)
					{
						OpenSalesDirectRegister(fromDate, toDate, accountId, itemId);
					}
					break;
				}
				break;
			case 12:
				switch (key[0])
				{
				case 'S':
					if (key == "SalesRegular" && Conversion.ToDouble(row.Cells["SalesRegular"].Value) != 0.0)
					{
						OpenSalesRegularRegister(fromDate, toDate, accountId, itemId);
					}
					break;
				case 'C':
					if (key == "ClosingStock" && Conversion.ToDouble(row.Cells["ClosingStock"].Value) != 0.0)
					{
						OpenStockReport(fromDate, toDate, accountId, itemId);
					}
					break;
				}
				break;
			case 7:
				if (key == "Opening" && Conversion.ToDouble(row.Cells["Opening"].Value) != 0.0)
				{
					OpenOpeningStock(fromDate, toDate, accountId, itemId);
				}
				break;
			case 8:
				if (key == "Purchase" && Conversion.ToDouble(row.Cells["Purchase"].Value) != 0.0)
				{
					OpenPurchaseRegister(fromDate, toDate, accountId, itemId);
				}
				break;
			case 10:
				if (key == "SaleReturn" && Conversion.ToDouble(row.Cells["SaleReturn"].Value) != 0.0)
				{
					OpenSalesReturnRegister(fromDate, toDate, accountId, itemId);
				}
				break;
			case 9:
			case 13:
			case 14:
				break;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OpenProductionRegister(DateTime fromDate, DateTime toDate, int accountId, int itemId, string activities)
	{
		ProductionRegister frm = new ProductionRegister(UserAccount);
		frm.RequestFromAnotherForm = true;
		frm.FromDate = fromDate;
		frm.ToDate = toDate;
		if (accountId > 0)
		{
			frm.StockAccountId = accountId;
		}
		if (!string.IsNullOrEmpty(activities))
		{
			frm.EntryType = activities;
		}
		if (itemId > 0)
		{
			frm.ItemId = itemId;
		}
		frm.ShowDialog();
	}

	private void OpenOpeningStock(DateTime fromDate, DateTime toDate, int accountId, int itemId)
	{
		frmOpeningStockBlancing frm = new frmOpeningStockBlancing(UserAccount);
		frm.RequestFromAnotherForm = true;
		frm.FromDate = fromDate;
		frm.ToDate = toDate;
		if (accountId > 0)
		{
			frm.StockAccountId = accountId;
		}
		if (itemId > 0)
		{
			frm.ItemId = itemId;
		}
		frm.ShowDialog();
	}

	private void OpenPurchaseRegister(DateTime fromDate, DateTime toDate, int accountId, int itemId)
	{
		PurchaseRegisterNew frm = new PurchaseRegisterNew(UserAccount);
		frm.FromDate = fromDate;
		frm.ToDate = toDate;
		if (accountId > 0)
		{
			frm.StockAccountId = accountId;
		}
		if (itemId > 0)
		{
			frm.ItemId = itemId;
		}
		frm.ShowDialog();
	}

	private void OpenSalesRegularRegister(DateTime fromDate, DateTime toDate, int accountId, int itemId)
	{
		frmEvaulationDetailSalesReports frm = new frmEvaulationDetailSalesReports(UserAccount);
		frm.RequestFromAnotherForm = true;
		frm.FromDate = fromDate;
		frm.ToDate = toDate;
		if (accountId > 0)
		{
			frm.StockAccountId = accountId;
		}
		if (itemId > 0)
		{
			frm.ItemId = itemId;
		}
		frm.ShowDialog();
	}

	private void OpenSalesReturnRegister(DateTime fromDate, DateTime toDate, int accountId, int itemId)
	{
		frmSaleInvoiceReturnRegister frm = new frmSaleInvoiceReturnRegister(UserAccount);
		frm.RequestFromAnotherForm = true;
		frm.FromDate = fromDate;
		frm.ToDate = toDate;
		if (accountId > 0)
		{
			frm.StockAccountId = accountId;
		}
		if (itemId > 0)
		{
			frm.ItemId = itemId;
		}
		frm.ShowDialog();
	}

	private void OpenSalesDirectRegister(DateTime fromDate, DateTime toDate, int accountId, int itemId)
	{
		frmSaleInvoiceDirectRegister frm = new frmSaleInvoiceDirectRegister(UserAccount);
		frm.RequestFromAnotherForm = true;
		frm.FromDate = fromDate;
		frm.ToDate = toDate;
		if (accountId > 0)
		{
			frm.StockAccountId = accountId;
		}
		if (itemId > 0)
		{
			frm.ItemId = itemId;
		}
		frm.ShowDialog();
	}

	private void OpenExportRegister(DateTime fromDate, DateTime toDate, int accountId, int itemId)
	{
		ExportCustomerWiseComparisonsSummeryReport frm = new ExportCustomerWiseComparisonsSummeryReport(UserAccount);
		frm.RequestFromAnotherForm = true;
		frm.FromDate = fromDate;
		frm.ToDate = toDate;
		if (accountId > 0)
		{
			frm.StockAccountId = accountId;
		}
		if (itemId > 0)
		{
			frm.ItemId = itemId;
		}
		frm.ShowDialog();
	}

	private void OpenStockReport(DateTime fromDate, DateTime toDate, int accountId, int itemId)
	{
		frmStockReportWithValues frm = new frmStockReportWithValues(UserAccount);
		frm.RequestFromAnotherForm = true;
		frm.FromDate = fromDate;
		frm.ToDate = toDate;
		if (accountId > 0)
		{
			frm.cmbStockAccounts.Value = accountId;
		}
		if (itemId > 0)
		{
			frm.cmbItem.Value = itemId;
		}
		frm.chkSkipZero.Checked = false;
		frm.ShowDialog();
	}

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, DataGridHistory, dtGridForLayout);
			}
		}
		catch (Exception ex)
		{
			waitForm.Close();
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
			if (dtRegister.Rows.Count == 0 || dtRegister == null)
			{
				throw new Exception("Record Not Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dtRegister, "143_02_ProfitLossBreakUp.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbActivity_TextChanged(object sender, EventArgs e)
	{
		try
		{
			_ = ((Control)(object)CmbParentAccount).Text != string.Empty;
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
			datFromDate.Focus();
			DataGridHistory.ClearStructure();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmEvaulationDetailSalesReports_FormClosing(object sender, FormClosingEventArgs e)
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
			waitForm.Show(this);
			BindParentAccountCombo(ParentAccountData());
		}
		catch (Exception ex)
		{
			waitForm.Close();
			MessageBox.Show(ex.Message);
		}
		finally
		{
			waitForm.Close();
		}
	}

	private void btnShow_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl != null && base.ActiveControl.Name == "DataGridHistory")
			{
				datFromDate.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtFromDocNo_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextNumberFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmEvaulationDetailSalesReports_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				btnShow_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnNew_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.R)
			{
				btnRefresh_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				datFromDate.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				datFromDate.Focus();
			}
			if (e.Control && e.KeyCode == Keys.P)
			{
				btnPrint_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)DataGridHistory).Focus();
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
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
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Ctrl+F5", "For Focus On As On Date ");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On As On Date in Filter");
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

	private void btnShortcut_Click(object sender, EventArgs e)
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

	private void DataGridHistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = DataGridHistory.CurrentRow;
			if (item != null && (int)item.RowType == 0 && DataGridHistory.CurrentColumn != null && e.Control && e.KeyCode == Keys.Space)
			{
				CommonServices.GoToGeneralLedgerFromLinkedEvent(Conversion.ToInt(item.Cells["AccountId"].Value), clsGlobalVariables.ActiveYr.Start_Period, datFromDate.Value);
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
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Expected O, but got Unknown
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Expected O, but got Unknown
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Expected O, but got Unknown
		//IL_0475: Unknown result type (might be due to invalid IL or missing references)
		//IL_047f: Expected O, but got Unknown
		//IL_048c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0496: Expected O, but got Unknown
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
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
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.DataGridHistory = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label4 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.datToDate = new System.Windows.Forms.DateTimePicker();
		this.label27 = new System.Windows.Forms.Label();
		this.datFromDate = new System.Windows.Forms.DateTimePicker();
		this.label1 = new System.Windows.Forms.Label();
		this.btnShow = new System.Windows.Forms.Button();
		this.CmbParentAccount = new UltraCombo();
		this.label12 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.lblBranch = new System.Windows.Forms.Label();
		this.lblCaption = new System.Windows.Forms.Label();
		this.cmbbranch = new UltraCombo();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.btnShortcut = new System.Windows.Forms.ToolStripButton();
		this.RadSummary = new System.Windows.Forms.RadioButton();
		this.RadDetail = new System.Windows.Forms.RadioButton();
		this.panel1.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel6.SuspendLayout();
		this.panel3.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbParentAccount).BeginInit();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbbranch).BeginInit();
		this.panel2.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(903, 636);
		this.panel1.TabIndex = 0;
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.panel4.Controls.Add(this.panel6);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 104);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(903, 532);
		this.panel4.TabIndex = 2;
		this.DataGridHistory.AllowEdit = (InheritableBoolean)2;
		this.DataGridHistory.DefaultFilterRowComparison = (FilterConditionOperator)7;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistory.DynamicFiltering = true;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(0, 29);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(903, 503);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 1;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		this.DataGridHistory.LinkClicked += new ColumnActionEventHandler(DataGridHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(DataGridHistory_KeyDown);
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.ctrlGrdBar1);
		this.panel6.Controls.Add(this.label4);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Margin = new System.Windows.Forms.Padding(2);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(903, 29);
		this.panel6.TabIndex = 167;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(856, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(41, 24);
		this.ctrlGrdBar1.TabIndex = 9;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label4.AutoSize = true;
		this.label4.BackColor = System.Drawing.Color.Teal;
		this.label4.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label4.Location = new System.Drawing.Point(7, 5);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(142, 19);
		this.label4.TabIndex = 0;
		this.label4.Text = "Filtered Records";
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add(this.groupBox1);
		this.panel3.Controls.Add(this.panel5);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 29);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(903, 75);
		this.panel3.TabIndex = 0;
		this.groupBox1.Controls.Add(this.RadSummary);
		this.groupBox1.Controls.Add(this.datToDate);
		this.groupBox1.Controls.Add(this.RadDetail);
		this.groupBox1.Controls.Add(this.label27);
		this.groupBox1.Controls.Add(this.datFromDate);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.btnShow);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbParentAccount);
		this.groupBox1.Controls.Add(this.label12);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(6, 30);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(885, 43);
		this.groupBox1.TabIndex = 177;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.datToDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datToDate.CustomFormat = "dd-MMM-yy";
		this.datToDate.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datToDate.Location = new System.Drawing.Point(247, 15);
		this.datToDate.Name = "datToDate";
		this.datToDate.Size = new System.Drawing.Size(130, 22);
		this.datToDate.TabIndex = 1;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(197, 19);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(48, 15);
		this.label27.TabIndex = 166;
		this.label27.Text = "To Date";
		this.datFromDate.CustomFormat = "dd-MMM-yy";
		this.datFromDate.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datFromDate.Location = new System.Drawing.Point(65, 15);
		this.datFromDate.Name = "datFromDate";
		this.datFromDate.Size = new System.Drawing.Size(130, 22);
		this.datFromDate.TabIndex = 0;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(4, 19);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(63, 15);
		this.label1.TabIndex = 1;
		this.label1.Text = "From Date";
		this.btnShow.BackColor = System.Drawing.Color.Teal;
		this.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShow.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShow.ForeColor = System.Drawing.Color.White;
		this.btnShow.Location = new System.Drawing.Point(674, 13);
		this.btnShow.Name = "btnShow";
		this.btnShow.Size = new System.Drawing.Size(61, 26);
		this.btnShow.TabIndex = 3;
		this.btnShow.Text = "Show";
		this.btnShow.UseVisualStyleBackColor = false;
		this.btnShow.Click += new System.EventHandler(btnShow_Click);
		this.btnShow.Leave += new System.EventHandler(btnShow_Leave);
		this.CmbParentAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbParentAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbParentAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbParentAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbParentAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.CmbParentAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbParentAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbParentAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbParentAccount).Location = new System.Drawing.Point(469, 13);
		((System.Windows.Forms.Control)(object)this.CmbParentAccount).Name = "CmbParentAccount";
		((System.Windows.Forms.Control)(object)this.CmbParentAccount).Size = new System.Drawing.Size(201, 26);
		((System.Windows.Forms.Control)(object)this.CmbParentAccount).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbParentAccount).TextChanged += new System.EventHandler(cmbActivity_TextChanged);
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label12.Location = new System.Drawing.Point(381, 19);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(89, 15);
		this.label12.TabIndex = 163;
		this.label12.Text = "Parent Account";
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.lblBranch);
		this.panel5.Controls.Add(this.lblCaption);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbbranch);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(903, 29);
		this.panel5.TabIndex = 166;
		this.lblBranch.AutoSize = true;
		this.lblBranch.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblBranch.ForeColor = System.Drawing.Color.White;
		this.lblBranch.Location = new System.Drawing.Point(192, 8);
		this.lblBranch.Name = "lblBranch";
		this.lblBranch.Size = new System.Drawing.Size(55, 15);
		this.lblBranch.TabIndex = 165;
		this.lblBranch.Text = "Branches";
		this.lblCaption.AutoSize = true;
		this.lblCaption.BackColor = System.Drawing.Color.Teal;
		this.lblCaption.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCaption.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.lblCaption.Location = new System.Drawing.Point(5, 4);
		this.lblCaption.Name = "lblCaption";
		this.lblCaption.Size = new System.Drawing.Size(170, 19);
		this.lblCaption.TabIndex = 0;
		this.lblCaption.Text = "Profit Loss BreakUp";
		this.cmbbranch.AllowNull = (DefaultableBoolean)1;
		this.cmbbranch.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbbranch.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbbranch.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Appearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbbranch).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbbranch).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance26).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance26).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbbranch).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbbranch).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance27;
		((SpecialBoxBase)((UltraGridBase)this.cmbbranch).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance28).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance28).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbbranch).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.cmbbranch).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbbranch).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance29;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance32).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance33).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance33).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance33).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance34).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.cmbbranch).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbbranch).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbbranch).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbbranch).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbbranch.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbbranch).Location = new System.Drawing.Point(252, 2);
		((System.Windows.Forms.Control)(object)this.cmbbranch).Name = "cmbbranch";
		((System.Windows.Forms.Control)(object)this.cmbbranch).Size = new System.Drawing.Size(199, 26);
		((System.Windows.Forms.Control)(object)this.cmbbranch).TabIndex = 164;
		this.panel2.Controls.Add(this.toolStrip3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(903, 29);
		this.panel2.TabIndex = 0;
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnNew, this.btnRefresh, this.btnPrint, this.btnShortcut });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(903, 27);
		this.toolStrip3.TabIndex = 7;
		this.toolStrip3.Text = "toolStrip3";
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
		this.btnPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(116, 24);
		this.btnPrint.Text = "&Print-143_02";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.btnShortcut.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortcut.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortcut.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortcut.Name = "btnShortcut";
		this.btnShortcut.Size = new System.Drawing.Size(126, 24);
		this.btnShortcut.Text = "ShortCut Keys";
		this.btnShortcut.Click += new System.EventHandler(btnShortcut_Click);
		this.RadSummary.AutoSize = true;
		this.RadSummary.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadSummary.Location = new System.Drawing.Point(799, 16);
		this.RadSummary.Name = "RadSummary";
		this.RadSummary.Size = new System.Drawing.Size(85, 21);
		this.RadSummary.TabIndex = 179;
		this.RadSummary.Text = "Summary";
		this.RadSummary.UseVisualStyleBackColor = true;
		this.RadDetail.AutoSize = true;
		this.RadDetail.Checked = true;
		this.RadDetail.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadDetail.Location = new System.Drawing.Point(738, 16);
		this.RadDetail.Name = "RadDetail";
		this.RadDetail.Size = new System.Drawing.Size(60, 21);
		this.RadDetail.TabIndex = 178;
		this.RadDetail.TabStop = true;
		this.RadDetail.Text = "Detail";
		this.RadDetail.UseVisualStyleBackColor = true;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(903, 636);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmProfitLossBreakUp";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmEvaulationDetailSalesReports_FormClosing);
		base.Load += new System.EventHandler(frmEvaulationDetailSalesReports_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmEvaulationDetailSalesReports_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbParentAccount).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbbranch).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		base.ResumeLayout(false);
	}
}
