// using Architecture.Reports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Architecture.BLL.Reports.Accounts;
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
namespace Architecture.WinApp.Account_Reports
{

public class frmProfitLossMonthWiseComparison : Form
{
	public bool RequestFromAnotherForm;

	public bool FormInitialized;

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtRegister = new DataTable();

	private DataTable dtGridForLayout = new DataTable();

	private WaitForm waitForm = new WaitForm();

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel3;

	private ToolStrip toolStrip3;

	private ToolStripButton btnNew;

	private CtrlGrdBar ctrlGrdBar1;

	private Panel panel5;

	private Label lblCaption;

	private ToolStripButton btnRefresh;

	private ToolStripButton btnShortcut;

	private Panel panel6;

	private Label label4;

	private GroupBox groupBox1;

	private Button btnShow;

	public UltraCombo CmbFromMonth;

	private Label label12;

	internal UltraCombo CmbToMonth;

	private Label label10;

	private ToolStripDropDownButton tsDrowdown;

	internal GridEX DataGridHistory;

	private RadioButton rdGroupeExpand;

	private RadioButton rdGroupCollapsed;

	private GroupBox groupBox2;

	private RadioButton rdSummary;

	private RadioButton rdDetail;

	private GroupBox groupBox3;

	private RadioButton rdGLWise;

	private RadioButton rdDocWise;

	public frmProfitLossMonthWiseComparison(UserAccount userAccount)
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
			Months = GlobalVariables_Helper.GetMonthListByAllFinancialYEarFromGlobal()
		}).ContinueWith(t =>
		{
			if (t.IsFaulted)
			{
				MessageBox.Show("Error occurred during database call.");
			}
			else
			{
				var result = t.Result;
				BindMonths(result.Months);
				CmbFromMonth.Focus();
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
			ReportsLoad();
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

	private void ReportsLoad()
	{
		try
		{
			string[] filesSelected = CommonServices.DynamicReportsLoad("ProfitLossMonthWiseComparison");
			if (filesSelected != null && filesSelected.Length != 0)
			{
				for (int i = 0; i < filesSelected.Length; i++)
				{
					tsDrowdown.DropDownItems.Add(Path.GetFileNameWithoutExtension(filesSelected[i]));
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindMonths(List<DateTime> monthList)
	{
		try
		{
			if (monthList.Count <= 0)
			{
				return;
			}
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Month");
			foreach (DateTime month in monthList)
			{
				dt.Rows.Add(month.ToString("MMM-yy"), month.ToString("MMM-yy"));
			}
			DDL.BindDDL(dt, CmbFromMonth, "Id", "Month", "From Month", ZeroIndex: false);
			DDL.BindDDL(dt, CmbToMonth, "Id", "Month", "To Month", ZeroIndex: false);
			((UltraGridBase)CmbFromMonth).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)CmbToMonth).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
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
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Expected O, but got Unknown
		try
		{
			dtGridForLayout.Rows.Clear();
			dtRegister.Rows.Clear();
			if (((UltraGridBase)CmbFromMonth).ActiveRow == null || ((Control)(object)CmbFromMonth).Text == string.Empty)
			{
				throw new Exception("From Month Required");
			}
			if (((UltraGridBase)CmbToMonth).ActiveRow == null || ((Control)(object)CmbToMonth).Text == string.Empty)
			{
				throw new Exception("To Month Required");
			}
			ReportsParameters val = new ReportsParameters
			{
				CompanyId = UserAccount.CompanyId
			};
			DateTime parsedDate = DateTime.ParseExact(Conversion.ToString(CmbFromMonth.Value), "MMM-yy", CultureInfo.InvariantCulture);
			DateTime firstDayOfMonth = new DateTime(parsedDate.Year, parsedDate.Month, 1);
			DateTime parsedToDate = DateTime.ParseExact(Conversion.ToString(CmbToMonth.Value), "MMM-yy", CultureInfo.InvariantCulture);
			DateTime lastDayOfMonth = new DateTime(parsedToDate.Year, parsedToDate.Month, 1).AddMonths(1).AddDays(-1.0);
			val.FromDate = firstDayOfMonth;
			val.ToDate = lastDayOfMonth;
			val.ActionId = (rdDocWise.Checked ? 1 : 0);
			DataSet ds = VoucherReports.GetMonthWiseProfitLoss(val);
			dtRegister = ds?.Tables?[0].Copy();
			if (dtRegister.Rows.Count == 0)
			{
				DataGridHistory.ClearStructure();
				return;
			}
			DatatableHelper.ReplaceCompLogoColumn(dtRegister, (ds.Tables[1].Rows.Count > 0) ? ds.Tables[1] : null);
			DataTable dt = new DataTable();
			dt.Columns.Add("ParentAccountId", typeof(int));
			dt.Columns.Add("ParentPLNoteId", typeof(int));
			dt.Columns.Add("PLNoteId", typeof(int));
			dt.Columns.Add("NoteTitle", typeof(string));
			dt.Columns.Add("ParentAccount", typeof(string));
			if (rdDetail.Checked)
			{
				dt.Columns.Add("AccountId", typeof(int));
				dt.Columns.Add("AccountCode", typeof(string));
				dt.Columns.Add("AccountTitle", typeof(string));
			}
			List<string> MonthColumns = new List<string>();
			foreach (DataColumn col7 in dtRegister.Columns)
			{
				if (col7.ColumnName.Contains("Caption") && Conversion.ToString(dtRegister.Rows[0][col7.ColumnName]) != string.Empty)
				{
					MonthColumns.Add(Conversion.ToString(dtRegister.Rows[0][col7.ColumnName]));
				}
			}
			foreach (string item in MonthColumns)
			{
				dt.Columns.Add(item, typeof(double));
			}
			dt.Columns.Add("SortOrder", typeof(int));
			dt.Columns.Add("TotalAmount", typeof(double));
			HashSet<int> PlIds = new HashSet<int>();
			object Value = DBNull.Value;
			foreach (DataRow regRow2 in dtRegister.Rows)
			{
				int plNoteId = Conversion.ToInt(regRow2["PLNoteId"]);
				if (!PlIds.Contains(plNoteId))
				{
					PlIds.Add(plNoteId);
					DataRow newRow3 = dt.NewRow();
					newRow3["ParentAccountId"] = 0;
					newRow3["ParentPLNoteId"] = Value;
					newRow3["PLNoteId"] = plNoteId;
					newRow3["NoteTitle"] = regRow2["NoteTitle"];
					newRow3["ParentAccount"] = Value;
					for (int j = 0; j < MonthColumns.Count; j++)
					{
						newRow3[MonthColumns[j]] = 0;
					}
					newRow3["SortOrder"] = GetSortOrder(plNoteId);
					newRow3["TotalAmount"] = 0;
					dt.Rows.Add(newRow3);
				}
			}
			DataRow gpRow = dt.NewRow();
			gpRow["ParentAccountId"] = 0;
			gpRow["ParentPLNoteId"] = Value;
			gpRow["PLNoteId"] = 100;
			gpRow["NoteTitle"] = "GROSS PROFIT";
			gpRow["ParentAccount"] = Value;
			foreach (string col5 in MonthColumns)
			{
				gpRow[col5] = 0;
			}
			gpRow["SortOrder"] = 4;
			gpRow["TotalAmount"] = 0;
			dt.Rows.Add(gpRow);
			DataRow npRow = dt.NewRow();
			npRow["ParentAccountId"] = 0;
			npRow["ParentPLNoteId"] = Value;
			npRow["PLNoteId"] = 101;
			npRow["NoteTitle"] = "NET PROFIT";
			npRow["ParentAccount"] = Value;
			foreach (string col4 in MonthColumns)
			{
				npRow[col4] = 0;
			}
			npRow["SortOrder"] = 10;
			npRow["TotalAmount"] = 0;
			dt.Rows.Add(npRow);
			int startIndex = dtRegister.Columns.Cast<DataColumn>().Select((DataColumn col, int index) => new { col, index }).FirstOrDefault(x => x.col.ColumnName.StartsWith("Column"))?.index ?? (-1);
			if (rdSummary.Checked)
			{
				foreach (IGrouping<int, DataRow> group in from r in dtRegister.AsEnumerable()
					group r by Conversion.ToInt(r["ParentAccountId"]))
				{
					int parentAccountId = group.Key;
					double rowWiseSum = 0.0;
					DataRow firstRow = group.First();
					DataRow newRow = dt.NewRow();
					newRow["ParentAccountId"] = parentAccountId;
					newRow["ParentPLNoteId"] = firstRow["PLNoteId"];
					newRow["PLNoteId"] = -parentAccountId;
					newRow["NoteTitle"] = firstRow["NoteTitle"];
					newRow["ParentAccount"] = firstRow["ParentAccount"];
					int k;
					for (k = 0; k < MonthColumns.Count; k++)
					{
						double sum4 = group.Sum((DataRow r) => Conversion.ToDouble(r[startIndex + k]));
						newRow[MonthColumns[k]] = sum4;
						rowWiseSum += sum4;
					}
					newRow["SortOrder"] = GetSortOrder(Conversion.ToInt(firstRow["PLNoteId"]));
					newRow["TotalAmount"] = rowWiseSum;
					dt.Rows.Add(newRow);
				}
			}
			else if (rdDetail.Checked)
			{
				double RowWiseSum = 0.0;
				foreach (DataRow regRow in dtRegister.Rows)
				{
					int ParentAcId = Conversion.ToInt(regRow["ParentAccountId"]);
					int AccountId = Conversion.ToInt(regRow["AccountId"]);
					DataRow newRow2 = dt.NewRow();
					newRow2["ParentAccountId"] = ParentAcId;
					newRow2["ParentPLNoteId"] = regRow["PLNoteId"];
					newRow2["PLNoteId"] = -AccountId;
					newRow2["NoteTitle"] = regRow["NoteTitle"];
					newRow2["ParentAccount"] = regRow["ParentAccount"];
					newRow2["AccountId"] = AccountId;
					newRow2["AccountCode"] = regRow["AccountCode"];
					newRow2["AccountTitle"] = regRow["AccountTitle"];
					for (int i = 0; i < MonthColumns.Count; i++)
					{
						newRow2[MonthColumns[i]] = Conversion.ToDouble(regRow[startIndex + i]);
						RowWiseSum += Conversion.ToDouble(newRow2[MonthColumns[i]]);
					}
					newRow2["SortOrder"] = GetSortOrder(Conversion.ToInt(regRow["PLNoteId"]));
					newRow2["TotalAmount"] = RowWiseSum;
					dt.Rows.Add(newRow2);
				}
			}
			foreach (DataRow parentRow in from r in dt.AsEnumerable()
				where Conversion.ToInt(r["ParentAccountId"]) == 0
				select r)
			{
				int PLNoteId = Conversion.ToInt(parentRow["PLNoteId"]);
				EnumerableRowCollection<DataRow> childRows = from r in dt.AsEnumerable()
					where Conversion.ToInt(r["ParentPLNoteId"]) == PLNoteId
					select r;
				double rowTotal = 0.0;
				foreach (string col6 in MonthColumns)
				{
					double sum3 = childRows.Sum((DataRow r) => (r[col6] != DBNull.Value) ? Conversion.ToDouble(r[col6]) : 0.0);
					parentRow[col6] = sum3;
					rowTotal += sum3;
				}
				parentRow["TotalAmount"] = rowTotal;
			}
			DataRow grossProfitRow = dt.AsEnumerable().First((DataRow r) => Conversion.ToInt(r["PLNoteId"]) == 100);
			int[] gpSortIds = new int[3] { 1, 2, 3 };
			double gpTotal = 0.0;
			foreach (string col3 in MonthColumns)
			{
				double sum2 = (from r in dt.AsEnumerable()
					where gpSortIds.Contains(Conversion.ToInt(r["SortOrder"])) && Conversion.ToInt(r["ParentAccountId"]) != 0
					select r).Sum((DataRow r) => (r[col3] != DBNull.Value) ? Conversion.ToDouble(r[col3]) : 0.0);
				grossProfitRow[col3] = sum2;
				gpTotal += sum2;
			}
			grossProfitRow["TotalAmount"] = gpTotal;
			DataRow NetProfitRow = dt.AsEnumerable().First((DataRow r) => Conversion.ToInt(r["PLNoteId"]) == 101);
			int[] NpSortIds = new int[5] { 5, 6, 7, 8, 9 };
			double npTotal = 0.0;
			foreach (string col2 in MonthColumns)
			{
				double sum = (from r in dt.AsEnumerable()
					where NpSortIds.Contains(Conversion.ToInt(r["SortOrder"])) && Conversion.ToInt(r["ParentAccountId"]) != 0
					select r).Sum((DataRow r) => (r[col2] != DBNull.Value) ? Conversion.ToDouble(r[col2]) : 0.0);
				NetProfitRow[col2] = sum;
				npTotal += sum;
			}
			NetProfitRow["TotalAmount"] = gpTotal;
			dtGridForLayout = dt.Copy();
			DataGridHistory.DataSource = dt;
			DataGridHistory.RetrieveStructure();
			DataGridHistorySetting(MonthColumns);
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

	private int GetSortOrder(int noteTitleId)
	{
		return noteTitleId switch
		{
			2 => 1, 
			4 => 2, 
			32 => 3, 
			7 => 5, 
			8 => 6, 
			3 => 7, 
			9 => 8, 
			6 => 9, 
			_ => 999, 
		};
	}

	private void DataGridHistorySetting(List<string> MonthColumns)
	{
		try
		{
			DataGridHistory.RootTable.Columns["ParentAccountId"].Visible = false;
			DataGridHistory.RootTable.Columns["SortOrder"].Visible = false;
			CommonServices.GridWrappingAndColumnSettings(DataGridHistory, 2, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, "false", null, NegativeFormat: true);
			if (rdDetail.Checked)
			{
				DataGridHistory.RootTable.Columns["AccountId"].Visible = false;
				DataGridHistory.RootTable.Columns["AccountCode"].Width = Constants.InventoryConstants.AccountCode;
				DataGridHistory.RootTable.Columns["AccountCode"].ColumnType = (ColumnType)(rdGLWise.Checked ? 5 : 0);
				DataGridHistory.RootTable.Columns["AccountTitle"].Width = Constants.InventoryConstants.AccountTitle + 40;
			}
			DataGridHistory.RootTable.Columns["ParentAccount"].ColumnType = (ColumnType)(rdGLWise.Checked ? 5 : 0);
			DataGridHistory.RootTable.Columns["NoteTitle"].Width = Constants.InventoryConstants.AccountTitle + 40;
			DataGridHistory.RootTable.Columns["ParentAccount"].Width = Constants.InventoryConstants.AccountTitle + 40;
			DataGridHistory.RootTable.Columns["TotalAmount"].Width = Constants.InventoryConstants.AmountForReportConstant;
			((Control)(object)DataGridHistory).Refresh();
			for (int i = 0; i < MonthColumns.Count; i++)
			{
				DataGridHistory.RootTable.Columns[MonthColumns[i]].Width = Constants.InventoryConstants.AmountForReportConstant;
			}
			DataGridHistory.GridLines = (GridLines)1;
			DataGridHistory.HideSelection = (HideSelection)2;
			DataGridHistory.AllowEdit = (InheritableBoolean)2;
			DataGridHistory.Hierarchical = true;
			DataGridHistory.RootTable.HierarchicalMode = (HierarchicalMode)1;
			DataGridHistory.RootTable.SelfReferencingSettings.ParentDataMember = "PLNoteId";
			DataGridHistory.RootTable.SelfReferencingSettings.ChildDataMember = "ParentPLNoteId";
			((JanusCollectionBase)DataGridHistory.RootTable.SortKeys).Clear();
			DataGridHistory.RootTable.SortKeys.Add(DataGridHistory.RootTable.Columns["SortOrder"], (SortOrder)1);
			DataGridHistory.RootTable.SelfReferencingSettings.ExpandColumn = DataGridHistory.RootTable.Columns["NoteTitle"];
			DataGridHistory.RootTable.SelfReferencingSettings.AggregationMode = (SelfReferencingAggregationMode)1;
			if (rdGroupCollapsed.Checked)
			{
				DataGridHistory.CollapseRecords();
			}
			else
			{
				DataGridHistory.ExpandRecords();
			}
			DataGridHistory.RootTable.Columns["PLNoteId"].Visible = false;
			DataGridHistory.RootTable.Columns["ParentPLNoteId"].Visible = false;
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

	private void tsDrowdown_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
	{
		//IL_04d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f7: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtGridForLayout.Rows.Count > 0 && dtGridForLayout != null)
			{
				List<string> MonthColumns = new List<string>();
				foreach (DataColumn col3 in dtRegister.Columns)
				{
					if (col3.ColumnName.Contains("Caption") && Conversion.ToString(dtRegister.Rows[0][col3.ColumnName]) != string.Empty)
					{
						MonthColumns.Add(Conversion.ToString(dtRegister.Rows[0][col3.ColumnName]));
					}
				}
				HashSet<string> excluded = MonthColumns.Select((string c) => c).ToHashSet();
				List<DataColumn> fixedColumns = (from DataColumn c in dtGridForLayout.Columns
					where !excluded.Contains(c.ColumnName)
					select c).ToList();
				DataTable dtOriginal = new DataTable();
				foreach (DataColumn col2 in fixedColumns)
				{
					dtOriginal.Columns.Add(col2.ColumnName, col2.DataType);
				}
				for (int j = 0; j < MonthColumns.Count; j++)
				{
					dtOriginal.Columns.Add($"Column{j + 1}", typeof(double));
					dtOriginal.Columns.Add($"Caption{j + 1}", typeof(string));
				}
				foreach (DataRow srcRow in dtGridForLayout.Rows)
				{
					DataRow newRow2 = dtOriginal.NewRow();
					foreach (DataColumn col in fixedColumns)
					{
						newRow2[col.ColumnName] = ((srcRow[col.ColumnName] == DBNull.Value) ? DBNull.Value : srcRow[col.ColumnName]);
					}
					for (int i = 0; i < MonthColumns.Count; i++)
					{
						newRow2[$"Column{i + 1}"] = ((srcRow[MonthColumns[i]] == DBNull.Value) ? 0.0 : Conversion.ToDouble(srcRow[MonthColumns[i]]));
						newRow2[$"Caption{i + 1}"] = MonthColumns[i];
					}
					dtOriginal.Rows.Add(newRow2);
				}
				string[] globalColumns = new string[11]
				{
					"CompCountry", "CompContactPerson", "CompMobileA", "CompMobileB", "CompMobileC", "CompEmailA", "CompEmailB", "CompLogoImage", "CompanyWebsite", "CompanyFaxNo",
					"OrgReportingRemarks"
				};
				string[] array = globalColumns;
				foreach (string colName2 in array)
				{
					if (!dtOriginal.Columns.Contains(colName2))
					{
						Type colType = (dtRegister.Columns.Contains(colName2) ? dtRegister.Columns[colName2].DataType : typeof(string));
						dtOriginal.Columns.Add(colName2, colType);
					}
				}
				DataRow globalRow = dtRegister.Rows[0];
				foreach (DataRow newRow in dtOriginal.Rows)
				{
					array = globalColumns;
					foreach (string colName in array)
					{
						newRow[colName] = (dtRegister.Columns.Contains(colName) ? (globalRow[colName] ?? DBNull.Value) : DBNull.Value);
					}
				}
				if (dtOriginal.Columns.Contains("SortOrder"))
				{
					DataView defaultView = dtOriginal.DefaultView;
					defaultView.Sort = "SortOrder ASC";
					dtOriginal = defaultView.ToTable();
				}
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dtOriginal, e.ClickedItem.Text + ".rpt");
				return;
			}
			throw new Exception("Record not found for Display");
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
			_ = ((Control)(object)CmbFromMonth).Text != string.Empty;
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
			((Control)(object)CmbFromMonth).Text = string.Empty;
			((Control)(object)CmbToMonth).Text = string.Empty;
			CmbFromMonth.Focus();
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
				CmbFromMonth.Focus();
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
				CmbFromMonth.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				CmbFromMonth.Focus();
			}
			if (e.Control)
			{
				_ = e.KeyCode;
				_ = 80;
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
				CommonServices.GoToGeneralLedgerFromLinkedEvent(Conversion.ToInt(item.Cells["AccountId"].Value), clsGlobalVariables.ActiveYr.Start_Period, DateTime.Now);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = DataGridHistory.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				int ParentAccountId = Conversion.ToInt(item.Cells["ParentAccountId"].Value);
				DateTime parsedDate = DateTime.ParseExact(Conversion.ToString(CmbFromMonth.Value), "MMM-yy", CultureInfo.InvariantCulture);
				DateTime firstDayOfMonth = new DateTime(parsedDate.Year, parsedDate.Month, 1);
				DateTime parsedToDate = DateTime.ParseExact(Conversion.ToString(CmbToMonth.Value), "MMM-yy", CultureInfo.InvariantCulture);
				DateTime lastDayOfMonth = new DateTime(parsedToDate.Year, parsedToDate.Month, 1).AddMonths(1).AddDays(-1.0);
				if (e.Column.Key == "ParentAccount")
				{
					SelectedTrialBalance selectedTrialBalance = new SelectedTrialBalance(UserAccount);
					SelectedTrialBalance.FromDate = firstDayOfMonth;
					SelectedTrialBalance.ToDate = lastDayOfMonth;
					SelectedTrialBalance.GroupId = ParentAccountId;
					selectedTrialBalance.BringToFront();
					selectedTrialBalance.ShowDialog();
				}
				else if (e.Column.Key == "AccountCode")
				{
					CommonServices.GoToGeneralLedgerFromLinkedEvent(Conversion.ToInt(item.Cells["AccountId"].Value), firstDayOfMonth, lastDayOfMonth);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_FormattingRow(object sender, RowLoadEventArgs e)
	{
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		try
		{
			if (DataGridHistory.RowCount <= 0)
			{
				return;
			}
			GridEXColumn ParentAccountId = DataGridHistory.RootTable.Columns["ParentAccountId"];
			if (ParentAccountId != null)
			{
				GridEXCell ParentCell = e.Row.Cells[((JanusCollectionElement)ParentAccountId).Index];
				if (Conversion.ToInt(ParentCell.Value) == 0)
				{
					ParentCell.FormatStyle = new GridEXFormatStyle();
					ParentCell.FormatStyle.FontBold = (TriState)2;
					ParentCell.FormatStyle.ForeColor = Color.Red;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void rdGroupCollapsed_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (rdGroupeExpand.Checked)
			{
				DataGridHistory.ExpandRecords();
			}
			else
			{
				DataGridHistory.CollapseRecords();
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
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Expected O, but got Unknown
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Expected O, but got Unknown
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Expected O, but got Unknown
		//IL_19ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_19f6: Expected O, but got Unknown
		//IL_1a03: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a0d: Expected O, but got Unknown
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
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Account_Reports.frmProfitLossMonthWiseComparison));
		this.panel6 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label4 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.rdGLWise = new System.Windows.Forms.RadioButton();
		this.rdDocWise = new System.Windows.Forms.RadioButton();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.rdSummary = new System.Windows.Forms.RadioButton();
		this.rdDetail = new System.Windows.Forms.RadioButton();
		this.rdGroupeExpand = new System.Windows.Forms.RadioButton();
		this.rdGroupCollapsed = new System.Windows.Forms.RadioButton();
		this.CmbToMonth = new UltraCombo();
		this.label10 = new System.Windows.Forms.Label();
		this.btnShow = new System.Windows.Forms.Button();
		this.CmbFromMonth = new UltraCombo();
		this.label12 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.lblCaption = new System.Windows.Forms.Label();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.tsDrowdown = new System.Windows.Forms.ToolStripDropDownButton();
		this.btnShortcut = new System.Windows.Forms.ToolStripButton();
		this.DataGridHistory = new GridEX();
		this.panel6.SuspendLayout();
		this.panel3.SuspendLayout();
		this.groupBox1.SuspendLayout();
		this.groupBox3.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbToMonth).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbFromMonth).BeginInit();
		this.panel5.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		base.SuspendLayout();
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.ctrlGrdBar1);
		this.panel6.Controls.Add(this.label4);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 129);
		this.panel6.Margin = new System.Windows.Forms.Padding(2);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1042, 29);
		this.panel6.TabIndex = 167;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(995, 2);
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
		this.panel3.Location = new System.Drawing.Point(0, 27);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1042, 102);
		this.panel3.TabIndex = 0;
		this.groupBox1.Controls.Add(this.groupBox3);
		this.groupBox1.Controls.Add(this.groupBox2);
		this.groupBox1.Controls.Add(this.rdGroupeExpand);
		this.groupBox1.Controls.Add(this.rdGroupCollapsed);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbToMonth);
		this.groupBox1.Controls.Add(this.label10);
		this.groupBox1.Controls.Add(this.btnShow);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbFromMonth);
		this.groupBox1.Controls.Add(this.label12);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(4, 30);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(685, 70);
		this.groupBox1.TabIndex = 177;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.groupBox3.Controls.Add(this.rdGLWise);
		this.groupBox3.Controls.Add(this.rdDocWise);
		this.groupBox3.Location = new System.Drawing.Point(517, 12);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(162, 51);
		this.groupBox3.TabIndex = 6;
		this.groupBox3.TabStop = false;
		this.rdGLWise.AutoSize = true;
		this.rdGLWise.Checked = true;
		this.rdGLWise.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdGLWise.Location = new System.Drawing.Point(5, 10);
		this.rdGLWise.Name = "rdGLWise";
		this.rdGLWise.Size = new System.Drawing.Size(74, 21);
		this.rdGLWise.TabIndex = 1;
		this.rdGLWise.TabStop = true;
		this.rdGLWise.Text = "GL Wise";
		this.rdGLWise.UseVisualStyleBackColor = true;
		this.rdDocWise.AutoSize = true;
		this.rdDocWise.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdDocWise.Location = new System.Drawing.Point(5, 27);
		this.rdDocWise.Name = "rdDocWise";
		this.rdDocWise.Size = new System.Drawing.Size(154, 21);
		this.rdDocWise.TabIndex = 0;
		this.rdDocWise.Text = "Document Type Wise";
		this.rdDocWise.UseVisualStyleBackColor = true;
		this.groupBox2.Controls.Add(this.rdSummary);
		this.groupBox2.Controls.Add(this.rdDetail);
		this.groupBox2.Location = new System.Drawing.Point(422, 13);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(92, 51);
		this.groupBox2.TabIndex = 5;
		this.groupBox2.TabStop = false;
		this.rdSummary.AutoSize = true;
		this.rdSummary.Checked = true;
		this.rdSummary.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdSummary.Location = new System.Drawing.Point(5, 10);
		this.rdSummary.Name = "rdSummary";
		this.rdSummary.Size = new System.Drawing.Size(85, 21);
		this.rdSummary.TabIndex = 1;
		this.rdSummary.TabStop = true;
		this.rdSummary.Text = "Summary";
		this.rdSummary.UseVisualStyleBackColor = true;
		this.rdDetail.AutoSize = true;
		this.rdDetail.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdDetail.Location = new System.Drawing.Point(5, 27);
		this.rdDetail.Name = "rdDetail";
		this.rdDetail.Size = new System.Drawing.Size(60, 21);
		this.rdDetail.TabIndex = 0;
		this.rdDetail.Text = "Detail";
		this.rdDetail.UseVisualStyleBackColor = true;
		this.rdGroupeExpand.AutoSize = true;
		this.rdGroupeExpand.Checked = true;
		this.rdGroupeExpand.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdGroupeExpand.Location = new System.Drawing.Point(345, 40);
		this.rdGroupeExpand.Name = "rdGroupeExpand";
		this.rdGroupeExpand.Size = new System.Drawing.Size(71, 21);
		this.rdGroupeExpand.TabIndex = 4;
		this.rdGroupeExpand.TabStop = true;
		this.rdGroupeExpand.Text = "Expand";
		this.rdGroupeExpand.UseVisualStyleBackColor = true;
		this.rdGroupCollapsed.AutoSize = true;
		this.rdGroupCollapsed.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdGroupCollapsed.Location = new System.Drawing.Point(345, 23);
		this.rdGroupCollapsed.Name = "rdGroupCollapsed";
		this.rdGroupCollapsed.Size = new System.Drawing.Size(76, 21);
		this.rdGroupCollapsed.TabIndex = 3;
		this.rdGroupCollapsed.Text = "Collapse";
		this.rdGroupCollapsed.UseVisualStyleBackColor = true;
		this.rdGroupCollapsed.CheckedChanged += new System.EventHandler(rdGroupCollapsed_CheckedChanged);
		this.CmbToMonth.AllowNull = (DefaultableBoolean)1;
		this.CmbToMonth.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbToMonth.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbToMonth.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbToMonth).DisplayLayout.Appearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbToMonth).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbToMonth).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance26).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance26).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbToMonth).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbToMonth).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance27;
		((SpecialBoxBase)((UltraGridBase)this.CmbToMonth).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance28).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance28).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbToMonth).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbToMonth).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbToMonth).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbToMonth).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance29;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbToMonth).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbToMonth).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbToMonth).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbToMonth).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance32).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbToMonth).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.CmbToMonth).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbToMonth).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance33).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance33).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance33).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbToMonth).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance34).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbToMonth).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.CmbToMonth).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbToMonth).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbToMonth).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbToMonth).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbToMonth).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbToMonth).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbToMonth).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbToMonth).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbToMonth).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbToMonth.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbToMonth).Location = new System.Drawing.Point(144, 36);
		((System.Windows.Forms.Control)(object)this.CmbToMonth).Name = "CmbToMonth";
		((System.Windows.Forms.Control)(object)this.CmbToMonth).Size = new System.Drawing.Size(136, 26);
		((System.Windows.Forms.Control)(object)this.CmbToMonth).TabIndex = 1;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(144, 17);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(59, 15);
		this.label10.TabIndex = 623;
		this.label10.Text = "To Month";
		this.btnShow.BackColor = System.Drawing.Color.Teal;
		this.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShow.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShow.ForeColor = System.Drawing.Color.White;
		this.btnShow.Location = new System.Drawing.Point(281, 36);
		this.btnShow.Name = "btnShow";
		this.btnShow.Size = new System.Drawing.Size(61, 26);
		this.btnShow.TabIndex = 2;
		this.btnShow.Text = "Show";
		this.btnShow.UseVisualStyleBackColor = false;
		this.btnShow.Click += new System.EventHandler(btnShow_Click);
		this.btnShow.Leave += new System.EventHandler(btnShow_Leave);
		this.CmbFromMonth.AllowNull = (DefaultableBoolean)1;
		this.CmbFromMonth.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbFromMonth.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbFromMonth.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbFromMonth).DisplayLayout.Appearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbFromMonth).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbFromMonth).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbFromMonth).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFromMonth).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.CmbFromMonth).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFromMonth).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbFromMonth).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbFromMonth).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbFromMonth).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbFromMonth).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbFromMonth).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbFromMonth).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFromMonth).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbFromMonth).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbFromMonth).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbFromMonth).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFromMonth).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbFromMonth).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbFromMonth).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbFromMonth).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbFromMonth).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbFromMonth).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbFromMonth).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbFromMonth).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbFromMonth).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbFromMonth).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbFromMonth).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbFromMonth.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbFromMonth).Location = new System.Drawing.Point(7, 36);
		((System.Windows.Forms.Control)(object)this.CmbFromMonth).Name = "CmbFromMonth";
		((System.Windows.Forms.Control)(object)this.CmbFromMonth).Size = new System.Drawing.Size(136, 26);
		((System.Windows.Forms.Control)(object)this.CmbFromMonth).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbFromMonth).TextChanged += new System.EventHandler(cmbActivity_TextChanged);
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label12.Location = new System.Drawing.Point(7, 17);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(74, 15);
		this.label12.TabIndex = 163;
		this.label12.Text = "From Month";
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.lblCaption);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1042, 29);
		this.panel5.TabIndex = 166;
		this.lblCaption.AutoSize = true;
		this.lblCaption.BackColor = System.Drawing.Color.Teal;
		this.lblCaption.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCaption.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.lblCaption.Location = new System.Drawing.Point(5, 4);
		this.lblCaption.Name = "lblCaption";
		this.lblCaption.Size = new System.Drawing.Size(195, 19);
		this.lblCaption.TabIndex = 0;
		this.lblCaption.Text = "Profit Loss Month Wise";
		this.toolStrip3.BackColor = System.Drawing.Color.White;
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnNew, this.btnRefresh, this.tsDrowdown, this.btnShortcut });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1042, 27);
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
		this.tsDrowdown.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tsDrowdown.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.tsDrowdown.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.tsDrowdown.Name = "tsDrowdown";
		this.tsDrowdown.Size = new System.Drawing.Size(70, 24);
		this.tsDrowdown.Text = "Print";
		this.tsDrowdown.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(tsDrowdown_DropDownItemClicked);
		this.btnShortcut.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortcut.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortcut.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortcut.Name = "btnShortcut";
		this.btnShortcut.Size = new System.Drawing.Size(126, 24);
		this.btnShortcut.Text = "ShortCut Keys";
		this.btnShortcut.Click += new System.EventHandler(btnShortcut_Click);
		this.DataGridHistory.AlternatingColors = true;
		this.DataGridHistory.DefaultFilterRowComparison = (FilterConditionOperator)7;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistory.DynamicFiltering = true;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupByBoxVisible = false;
		this.DataGridHistory.GroupTotals = (GroupTotals)2;
		this.DataGridHistory.HeaderFormatStyle.LineAlignment = (TextAlignment)2;
		this.DataGridHistory.HeaderFormatStyle.TextAlignment = (TextAlignment)2;
		this.DataGridHistory.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(0, 158);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1042, 464);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 168;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.FormattingRow += new RowLoadEventHandler(DataGridHistory_FormattingRow);
		this.DataGridHistory.LinkClicked += new ColumnActionEventHandler(DataGridHistory_LinkClicked);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1042, 622);
		base.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		base.Controls.Add(this.panel6);
		base.Controls.Add(this.panel3);
		base.Controls.Add(this.toolStrip3);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Name = "frmProfitLossMonthWiseComparison";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmEvaulationDetailSalesReports_FormClosing);
		base.Load += new System.EventHandler(frmEvaulationDetailSalesReports_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmEvaulationDetailSalesReports_KeyDown);
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbToMonth).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbFromMonth).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}

}
