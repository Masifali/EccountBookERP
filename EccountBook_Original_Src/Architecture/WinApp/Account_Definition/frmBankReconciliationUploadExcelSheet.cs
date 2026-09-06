using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using Architecture.BLL.Accounts;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using ExcelDataReader;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using Microsoft.Office.Interop.Excel;
using OfficeOpenXml;
using grid.User_Conrtols;

namespace Architecture.WinApp.Account_Definition;

public class frmBankReconciliationUploadExcelSheet : Form
{
	private int DocumentTypeId = 920;

	private int DefaultDaysToLessFromHistoryFromDate;

	private bool FormInitialized;

	private bool UpdateMode;

	private DataTable dtExpectedSchema = new DataTable();

	private DataTable dtGridHistory = new DataTable();

	private DataTable dtGridAccountHistory = new DataTable();

	private DataTableCollection TableCollection;

	private Microsoft.Office.Interop.Excel.Application xlsp = (Microsoft.Office.Interop.Excel.Application)Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("00024500-0000-0000-C000-000000000046")));

	private UserAccount UserAccount = new UserAccount();

	private List<BankReconciliation> lstRemoveRecord = new List<BankReconciliation>();

	private IContainer components;

	private Panel panel1;

	private Panel panel3;

	private Panel panel2;

	internal GridEX grdExcelDataView;

	private PictureBox btndownload;

	private PictureBox btnUpload;

	internal UltraCombo cmbaccounttitle;

	private Label label7;

	private Panel panel4;

	private Label label4;

	private Label label2;

	private ToolStrip toolstrip;

	private ToolStripButton btnnew;

	private ToolStripButton BtnRefresh;

	private ToolStripButton btnSave;

	private ToolStripButton btnShortCutKey;

	private Label label1;

	private Panel panel5;

	private TabControl tabControl1;

	private TabPage tabForm;

	private TabPage tabhistory;

	private Panel panel6;

	private Label label3;

	internal GridEX grdAccountHistory;

	private Button BtnShow;

	internal GridEX grdHistory;

	private Panel panel8;

	private CtrlGrdBar ctrlGrdBar2;

	private Label label78;

	private Panel panel7;

	private GroupBox groupBox4;

	private RadioButton rdmodifydate;

	private RadioButton rdentrydate;

	private Button BtnShowhistory;

	private Label label73;

	internal UltraCombo CmbCustomerHistory;

	private Label label75;

	internal UltraCombo cmbDateTypeHistory;

	private DateTimePicker txtToDateHistory;

	private Label label5;

	private DateTimePicker txtFromdateHistory;

	private Panel panel9;

	private Label label54;

	private ToolStrip toolStrip2;

	private ToolStripButton BtnNewHistory;

	private ToolStripButton BtnRefreshHistory;

	private Button btnUpdate;

	private ToolStripButton btnUpdateRecords;

	public frmBankReconciliationUploadExcelSheet(UserAccount puserAccount)
	{
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = puserAccount;
		InitializeComponentCustom();
		InitializeComponentMethod();
	}

	private void InitializeComponentCustom()
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(10, 10);
			base.AutoScrollMinSize = new Size(1050, 600);
			if (dtExpectedSchema.Columns.Count == 0)
			{
				dtExpectedSchema.Columns.Add("TransactionDate", typeof(DateTime));
				dtExpectedSchema.Columns.Add("ChequeNo");
				dtExpectedSchema.Columns.Add("Particulars");
				dtExpectedSchema.Columns.Add("Debit", typeof(double));
				dtExpectedSchema.Columns.Add("Credit", typeof(double));
				dtExpectedSchema.Columns.Add("Id", typeof(int));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InitializeComponentMethod()
	{
		Task.Run(() => new
		{
			HcomboData = BankReconciliation.GetDataForDropDownFromBankReconciliation(UserAccount.OrganizationId, UserAccount.CompanyId, 0, "BankAccount")
		}).ContinueWith(t =>
		{
			DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("DefaultDaysToLessFromHistoryFromDate"));
			if (t.IsFaulted)
			{
				MessageBox.Show("Error occurred during database call.");
			}
			else
			{
				var result = t.Result;
				BankGlAccountBindFromGlobal();
				ParameterFill();
				HistoryComboBind(result.HcomboData);
				txtFromdateHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
				txtToDateHistory.Value = DateTime.Now;
				FormInitialized = true;
				cmbaccounttitle.Focus();
			}
		}, TaskScheduler.FromCurrentSynchronizationContext());
	}

	private void BankGlAccountBindFromGlobal()
	{
		try
		{
			DataTable dt = DatatableHelper.GetAccountsFromGlobalByTypeIds(new int[1] { 15 });
			InfragisticsHelper.BindAndRetainSelection(cmbaccounttitle, dt, "Id", "AccountTitle", "AccountTitle", AllColumns: true, Conversion.ToInt(cmbaccounttitle.Value), insertDefaultRow: true, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnDownloadPic_Click(object sender, EventArgs e)
	{
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		try
		{
			if (grdExcelDataView.DataSource != null && grdExcelDataView.RowCount > 0 && MessageBox.Show("Downloading the template will clear the current data in the grid. Do you want to continue?", "Confirm Download", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
			{
				return;
			}
			ExcelPackage.LicenseContext = (LicenseContext)0;
			ExcelPackage excelPackage = new ExcelPackage();
			try
			{
				List<string> excludedColumns = new List<string> { "Id" };
				ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Sheet1");
				for (int col = 0; col < dtExpectedSchema.Columns.Count; col++)
				{
					if (!excludedColumns.Contains(dtExpectedSchema.Columns[col].ColumnName))
					{
						((ExcelRangeBase)worksheet.Cells[1, col + 1]).Value = dtExpectedSchema.Columns[col].ColumnName;
					}
				}
				((ExcelRangeBase)worksheet.Cells[2, 1]).Value = DateTime.Today;
				string systemDateFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
				worksheet.Column(1).Style.Numberformat.Format = systemDateFormat;
				string downloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
				string baseFileName = "BankReconciliation_ExcelTemplate(AccountBookERP)";
				string fileName = baseFileName + ".xlsx";
				string filePath = Path.Combine(downloadsPath, fileName);
				if (File.Exists(filePath))
				{
					int count = Directory.GetFiles(downloadsPath, baseFileName + "*").Length;
					filePath = Path.Combine(downloadsPath, $"{baseFileName} ({count}).xlsx");
				}
				File.WriteAllBytes(filePath, excelPackage.GetAsByteArray());
				MessageBox.Show("Excel file created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
			finally
			{
				((IDisposable)excelPackage)?.Dispose();
			}
			grdExcelDataView.DataSource = null;
			cmbaccounttitle.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BtnUploadPic_Click(object sender, EventArgs e)
	{
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Expected O, but got Unknown
		try
		{
			if (grdExcelDataView.DataSource != null && grdExcelDataView.RowCount > 0 && MessageBox.Show("Uploading the template will clear the current data in the grid and then load new data if found. Do you want to continue?", "Confirm Download", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
			{
				return;
			}
			dtExpectedSchema.Clear();
			OpenFileDialog fileDialog = new OpenFileDialog
			{
				Filter = "Excel Workbook (*.xlsx)|*.xlsx",
				Multiselect = false
			};
			if (fileDialog.ShowDialog() != DialogResult.OK)
			{
				MessageBox.Show("Please choose a valid .xlsx file.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			TableCollection = null;
			using (FileStream stream = File.Open(fileDialog.FileName, FileMode.Open, FileAccess.Read))
			{
				IExcelDataReader reader = ExcelReaderFactory.CreateReader((Stream)stream, (ExcelReaderConfiguration)null);
				try
				{
					DataSet excelData = ExcelDataReaderExtensions.AsDataSet(reader, new ExcelDataSetConfiguration
					{
						ConfigureDataTable = (IExcelDataReader _) => new ExcelDataTableConfiguration
						{
							UseHeaderRow = true
						}
					});
					TableCollection = excelData.Tables;
				}
				finally
				{
					((IDisposable)reader)?.Dispose();
				}
			}
			DataTable combinedData = new DataTable();
			foreach (DataTable sheet in TableCollection)
			{
				List<string> first = (from DataColumn c in dtExpectedSchema.Columns
					select c.ColumnName.Trim().ToLower()).ToList();
				List<string> uploadedCols = (from DataColumn c in sheet.Columns
					select c.ColumnName.Trim().ToLower()).ToList();
				List<string> missingCols = first.Except(uploadedCols).ToList();
				if (missingCols.Any() && !missingCols.Contains("id"))
				{
					MessageBox.Show("The uploaded Excel file is missing the following required columns:\n" + string.Join(", ", missingCols), "Column Mismatch", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					return;
				}
				foreach (DataRow row in sheet.Rows)
				{
					DataRow newRow = dtExpectedSchema.NewRow();
					foreach (DataColumn col in dtExpectedSchema.Columns)
					{
						object value = ((col.ColumnName == "Id") ? ((object)0) : row[col.ColumnName]);
						if (col.ColumnName == "Id")
						{
							newRow[col.ColumnName] = 0;
						}
						else if (col.ColumnName == "TransactionDate")
						{
							newRow[col.ColumnName] = Conversion.ToDateTime(value);
						}
						else
						{
							newRow[col.ColumnName] = value;
						}
					}
					dtExpectedSchema.Rows.Add(newRow);
				}
				combinedData.Merge(dtExpectedSchema);
			}
			if (combinedData.Rows.Count > 0)
			{
				FillExcelGrid(combinedData);
				return;
			}
			grdExcelDataView.ClearStructure();
			MessageBox.Show("No data found in the uploaded Excel file.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}
		catch (Exception ex)
		{
			MessageBox.Show("Failed to upload Excel file.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void FillExcelGrid(DataTable dt)
	{
		try
		{
			grdExcelDataView.DataSource = dt;
			grdExcelDataView.RetrieveStructure();
			grdExcelDataView.RootTable.SortKeys.Add(grdExcelDataView.RootTable.Columns["TransactionDate"], (SortOrder)1);
			Dictionary<string, EditType> editCol = new Dictionary<string, EditType>
			{
				{
					"TransactionDate",
					(EditType)2
				},
				{
					"Particulars",
					(EditType)1
				},
				{
					"ChequeNo",
					(EditType)1
				},
				{
					"Debit",
					(EditType)1
				},
				{
					"Credit",
					(EditType)1
				}
			};
			GridEX_Helper.GridWrappingAndColumnSettings(grdExcelDataView, 2, 3, null, editCol);
			grdExcelDataView.RootTable.Columns["Id"].Caption = "RNo";
			grdExcelDataView.RootTable.Columns["TransactionDate"].Width = Constants.InventoryConstants.DateConstant + 10;
			grdExcelDataView.RootTable.Columns["Particulars"].Width = 140;
			grdExcelDataView.RootTable.Columns["ChequeNo"].Width = 80;
			grdExcelDataView.RootTable.Columns["Debit"].Width = Constants.InventoryConstants.AmountConstant;
			grdExcelDataView.RootTable.Columns["Credit"].Width = Constants.InventoryConstants.AmountConstant;
			GridEX_Helper.AddButton(grdExcelDataView, "Delete", "X", 20, 0);
			GridEX_Helper.AddButton(grdExcelDataView, "Add", "+", 20, 1);
			grdExcelDataView.RootTable.Columns["Id"].Position = 2;
			grdExcelDataView.FrozenColumns = 3;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grdExcelDataView_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdExcelDataView.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				if (e.Column.Key == "Delete")
				{
					DeleteDetailRow(r);
				}
				if (e.Column.Key == "Add")
				{
					AddRowInGrid();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInGrid()
	{
		dtExpectedSchema.Rows.Add(DateTime.Now, "", "", 0, 0, 0);
		grdExcelDataView.DataSource = dtExpectedSchema;
	}

	private void DeleteDetailRow(GridEXRow r)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Expected O, but got Unknown
		try
		{
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0 && MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					BankReconciliation obj = new BankReconciliation();
					obj.EntryDate = DateTime.Now;
					obj.ModifyDate = DateTime.Now;
					obj.ApprovedDate = DateTime.Now;
					obj.OrganizationId = UserAccount.OrganizationId;
					obj.CompanyId = UserAccount.CompanyId;
					obj.EntryUserId = UserAccount.ID;
					obj.ModifyUserId = UserAccount.ID;
					obj.ApprovedUserId = UserAccount.ID;
					obj.BankAccountId = Conversion.ToInt(cmbaccounttitle.Value);
					obj.Id = Conversion.ToInt(r.Cells["Id"].Value);
					obj.ActionId = 3;
					obj.TransactionDate = Conversion.ToDateTime(r.Cells["TransactionDate"].Value);
					obj.ChequeNo = Conversion.ToString(r.Cells["ChequeNo"].Value);
					obj.Particulars = Conversion.ToString(r.Cells["Particulars"].Value);
					obj.Debit = Conversion.ToDecimal(r.Cells["Debit"].Value);
					obj.Credit = Conversion.ToDecimal(r.Cells["Credit"].Value);
					lstRemoveRecord.Add(obj);
					dtExpectedSchema.Rows[r.RowIndex].Delete();
					r.Delete();
				}
				else
				{
					r.Delete();
				}
				grdExcelDataView.UpdateData();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnSave_Click(object sender, EventArgs e)
	{
		try
		{
			UpdateMode = false;
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdateRecords_Click(object sender, EventArgs e)
	{
		try
		{
			UpdateMode = true;
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Expected O, but got Unknown
		try
		{
			if (((Control)(object)cmbaccounttitle).Text.Trim() == "")
			{
				cmbaccounttitle.Focus();
				throw new Exception("Please select an account title.");
			}
			if (UpdateMode)
			{
				if (!FormHelper.ConfirmAction("Are you sure to Update"))
				{
					return;
				}
			}
			else if (!FormHelper.ConfirmAction("Are you sure to Save"))
			{
				return;
			}
			if (grdExcelDataView.GetDataRows().Count() <= 0)
			{
				return;
			}
			BankReconciliation Coa = new BankReconciliation();
			Coa.BankReconciliationList = new List<BankReconciliation>();
			GridEXRow[] rows = grdExcelDataView.GetRows();
			foreach (GridEXRow item in rows)
			{
				BankReconciliation obj = new BankReconciliation();
				obj.OrganizationId = UserAccount.OrganizationId;
				obj.CompanyId = UserAccount.CompanyId;
				obj.BranchesId = UserAccount.BranchesId;
				obj.ProjectsId = UserAccount.BranchesId;
				obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
				obj.DocumentTypeId = 920;
				obj.EntryUserId = UserAccount.ID;
				obj.ModifyUserId = UserAccount.ID;
				obj.ApprovedUserId = UserAccount.ID;
				obj.EntryDate = DateTime.Now;
				obj.ModifyDate = DateTime.Now;
				obj.ApprovedDate = DateTime.Now;
				obj.BankAccountId = Conversion.ToInt(cmbaccounttitle.Value);
				obj.Id = Conversion.ToInt(item.Cells["Id"].Value);
				obj.ActionId = ((obj.Id == 0) ? 1 : 2);
				obj.TransactionDate = Conversion.ToDateTime(item.Cells["TransactionDate"].Value);
				obj.ChequeNo = Conversion.ToString(item.Cells["ChequeNo"].Value);
				obj.Particulars = Conversion.ToString(item.Cells["Particulars"].Value);
				obj.Debit = Conversion.ToDecimal(item.Cells["Debit"].Value);
				obj.Credit = Conversion.ToDecimal(item.Cells["Credit"].Value);
				FormHelper.ValidateField(obj.Particulars, "Particulars", item.RowIndex, "Excel Data");
				if ((obj.Debit <= 0m && obj.Credit <= 0m) || (obj.Debit > 0m && obj.Credit > 0m))
				{
					throw new Exception($"Either Debit or Credit must be greater than zero but not both In Row No {item.RowIndex + 1} in Excel Data");
				}
				obj.IsReconciled = false;
				Coa.BankReconciliationList.Add(obj);
			}
			if (UpdateMode && lstRemoveRecord.Count > 0)
			{
				for (int i = 0; i < lstRemoveRecord.Count; i++)
				{
					Coa.BankReconciliationList.Add(lstRemoveRecord[i]);
				}
			}
			if (BankReconciliation.SaveList(Coa) > 0)
			{
				MessageBox.Show("Record's Uploaded Successfully");
				grdExcelDataView.ClearStructure();
			}
			btnnew_Click(null, null);
		}
		catch (Exception ex)
		{
			throw new Exception("Exception From Insert Method:" + ex.Message);
		}
	}

	private void BtnShow_Click(object sender, EventArgs e)
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Expected O, but got Unknown
		try
		{
			if (((Control)(object)cmbaccounttitle).Text.Trim() == "")
			{
				cmbaccounttitle.Focus();
				throw new Exception("Please select an account title.");
			}
			ReportsParameters PI = new ReportsParameters();
			PI.OrganizationId = UserAccount.OrganizationId;
			PI.CompanyId = UserAccount.CompanyId;
			PI.BranchesId = UserAccount.BranchesId;
			PI.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			PI.DocumentTypeId = DocumentTypeId;
			PI.AccountId = Conversion.ToInt(cmbaccounttitle.Value);
			grdHistorySetting(PI, grdAccountHistory, dtGridAccountHistory);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			if (tabControl1.SelectedIndex == 1)
			{
				cmbDateTypeHistory.Focus();
			}
			else
			{
				cmbaccounttitle.Focus();
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
			InfragisticsHelper.BindComboDateType(cmbDateTypeHistory, 0);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbDateTypeHistory_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (Convert.ToInt32(cmbDateTypeHistory.Value) == 1)
			{
				txtFromdateHistory.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbDateTypeHistory.Value) == 2)
			{
				txtFromdateHistory.Value = DateTime.Today.AddDays(-7.0);
			}
			else if (Convert.ToInt32(cmbDateTypeHistory.Value) == 3)
			{
				DateTime now = DateTime.UtcNow;
				DateTime first = now.Date.AddDays(-(now.Date.Day - 1));
				txtFromdateHistory.Value = first;
				txtToDateHistory.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbDateTypeHistory.Value) == 4)
			{
				int year = DateTime.Now.Year;
				DateTime first2 = new DateTime(year, 1, 1);
				txtFromdateHistory.Value = first2;
				txtToDateHistory.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbDateTypeHistory.Value) == 5)
			{
				txtFromdateHistory.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryComboBind(DataTable dtSupplier)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbCustomerHistory, dtSupplier, "Id", "name", "Bank Account", AllColumns: false, Conversion.ToInt(CmbCustomerHistory.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Resethistory()
	{
		try
		{
			cmbDateTypeHistory.Value = 3;
			((Control)(object)CmbCustomerHistory).Text = string.Empty;
			cmbDateTypeHistory.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnNewHistory_Click(object sender, EventArgs e)
	{
		try
		{
			Resethistory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnRefreshHistory_Click(object sender, EventArgs e)
	{
		try
		{
			HistoryComboBind(BankReconciliation.GetDataForDropDownFromBankReconciliation(UserAccount.OrganizationId, UserAccount.CompanyId, 0, "BankAccount"));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnshow_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters PI = new ReportsParameters();
			PI.OrganizationId = UserAccount.OrganizationId;
			PI.CompanyId = UserAccount.CompanyId;
			PI.BranchesId = UserAccount.BranchesId;
			PI.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			PI.DocumentTypeId = DocumentTypeId;
			if (rdentrydate.Checked)
			{
				if (txtFromdateHistory.Checked)
				{
					PI.EntryDateFrom = txtFromdateHistory.Value;
				}
				if (txtToDateHistory.Checked)
				{
					PI.EntryDateTo = txtToDateHistory.Value;
				}
			}
			else if (rdmodifydate.Checked)
			{
				if (txtFromdateHistory.Checked)
				{
					PI.ModifyDateFrom = txtFromdateHistory.Value;
				}
				if (txtToDateHistory.Checked)
				{
					PI.ModifyDateTo = txtToDateHistory.Value;
				}
			}
			PI.AccountId = Conversion.ToInt(CmbCustomerHistory.Value);
			grdHistorySetting(PI, grdHistory, dtGridHistory);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistorySetting(ReportsParameters PI, GridEX grid, DataTable dtHistory)
	{
		try
		{
			PI.CanViewAllRecord = true;
			DataTable invPI = BankReconciliation.FormHistory(PI);
			if (invPI != null && invPI.Rows.Count > 0)
			{
				if (dtHistory.Columns.Count == 0)
				{
					dtHistory.Columns.Add("Id", typeof(int));
					dtHistory.Columns.Add("DocumentTypeId", typeof(int));
					dtHistory.Columns.Add("BankAccountId", typeof(int));
					dtHistory.Columns.Add("AccountTitle", typeof(string));
					dtHistory.Columns.Add("AccountCode", typeof(string));
					dtHistory.Columns.Add("TransactionDate", typeof(DateTime));
					dtHistory.Columns.Add("ChequeNo", typeof(string));
					dtHistory.Columns.Add("Particulars", typeof(string));
					dtHistory.Columns.Add("Debit", typeof(decimal));
					dtHistory.Columns.Add("Credit", typeof(decimal));
					dtHistory.Columns.Add("IsReconciled", typeof(bool));
					dtHistory.Columns.Add("Remarks", typeof(string));
					dtHistory.Columns.Add("EntryDate", typeof(DateTime));
					dtHistory.Columns.Add("EntryUser", typeof(string));
					dtHistory.Columns.Add("ModifyDate", typeof(DateTime));
					dtHistory.Columns.Add("ModifyUser", typeof(string));
					dtHistory.Columns.Add("IsApproved", typeof(bool));
					dtHistory.Columns.Add("ApprovedDate", typeof(DateTime));
					dtHistory.Columns.Add("ApprovedUser", typeof(string));
					dtHistory.Columns.Add("NoOfAttachments", typeof(int));
				}
				dtHistory.Clear();
				foreach (DataRow dataRow in invPI.Rows)
				{
					dtHistory.Rows.Add(dataRow["Id"], dataRow["DocumentTypeId"], dataRow["BankAccountId"], dataRow["AccountTitle"], dataRow["AccountCode"], dataRow["TransactionDate"], dataRow["ChequeNo"], dataRow["Particulars"], dataRow["Debit"], dataRow["Credit"], dataRow["IsReconciled"], dataRow["Remarks"], dataRow["EntryDate"], dataRow["EntryUser"], dataRow["ModifyDate"], dataRow["ModifyUser"], dataRow["IsApproved"], dataRow["ApprovedDate"], dataRow["ApprovedUser"], dataRow["NoOfAttachments"]);
				}
				grid.DataSource = dtHistory;
				grid.RetrieveStructure();
				grdHistorySetting(grid);
			}
			else
			{
				grid.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistorySetting(GridEX grd)
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "DocumentTypeId", "BankAccountId", "NoOfAttachments", "IsApproved", "ApprovedUser", "ApprovedDate" };
			GridEX_Helper.GridWrappingAndColumnSettings(grd, 3, 3, hiddenColumns);
			grd.RootTable.Columns["EntryDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			grd.RootTable.Columns["ModifyDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			grd.RootTable.Columns["ApprovedDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			if (((Control)(object)grd).Name == "grdAccountHistory")
			{
				grd.RootTable.Columns["AccountTitle"].Visible = false;
				grd.RootTable.Columns["AccountCode"].Visible = false;
			}
			grd.RootTable.Columns["AccountTitle"].Width = Constants.InventoryConstants.AccountTitle;
			grd.RootTable.Columns["AccountCode"].Width = Constants.InventoryConstants.AccountCode;
			grd.RootTable.Columns["TransactionDate"].Width = Constants.InventoryConstants.DateConstant + 10;
			grd.RootTable.Columns["ChequeNo"].Width = 120;
			grd.RootTable.Columns["Particulars"].Width = 180;
			grd.RootTable.Columns["Debit"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["Credit"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["IsReconciled"].Width = 80;
			grd.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants;
			grd.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM;
			grd.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants;
			grd.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM;
			grd.RootTable.Columns["ModifyUser"].Width = Constants.InventoryConstants.ModifyUserConstants;
			grd.RootTable.Columns["IsApproved"].Width = 80;
			grd.RootTable.Columns["ApprovedDate"].Width = Constants.InventoryConstants.DateConstant;
			grd.RootTable.Columns["ApprovedUser"].Width = Constants.InventoryConstants.EntryUserConstants;
			grd.RootTable.Columns["NoOfAttachments"].Width = 80;
			grd.FrozenColumns = 1;
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
			lstRemoveRecord.Clear();
			btnSave.Visible = true;
			btnUpdateRecords.Visible = false;
			((Control)(object)cmbaccounttitle).Text = string.Empty;
			grdExcelDataView.DataSource = null;
			grdAccountHistory.DataSource = null;
			cmbaccounttitle.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			List<string> allServices = new List<string> { "AccountsWithCustomGroupId" };
			DatatableHelper.GlobalServicesDbCall(UserAccount, allServices);
			BankGlAccountBindFromGlobal();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmBankReconciliationUploadExcelSheet_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.T)
			{
				if (tabControl1.SelectedIndex == 1)
				{
					tabControl1.SelectedIndex = 0;
					((Control)(object)grdExcelDataView).Focus();
				}
				else
				{
					tabControl1.SelectedIndex = 1;
					cmbDateTypeHistory.Focus();
				}
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
			}
			if (tabControl1.SelectedIndex == 0)
			{
				if (e.Control && e.KeyCode == Keys.S && btnSave.Visible && btnSave.Enabled)
				{
					btnSave_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnnew_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.R)
				{
					BtnRefresh_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					cmbaccounttitle.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grdExcelDataView).Focus();
				}
				if (e.Control && e.KeyCode == Keys.Right)
				{
					if (((Control)(object)grdExcelDataView).Focused)
					{
						((Control)(object)grdAccountHistory).Focus();
					}
					else
					{
						((Control)(object)grdExcelDataView).Focus();
					}
				}
				if (e.Control && e.KeyCode == Keys.Up)
				{
					cmbaccounttitle.Focus();
				}
				return;
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				((Control)(object)grdHistory).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grdHistory).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Right)
			{
				if (((Control)(object)grdHistory).Focused)
				{
					((Control)(object)grdHistory).Focus();
				}
				else
				{
					((Control)(object)grdHistory).Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				((Control)(object)grdHistory).Focus();
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
			dt.Rows.Add("Ctrl+S", "For Save");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+Shift+Delete", "For Delete");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+F5", "For Focus on Account");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+D", "For Adding an row in Focused Grid");
			dt.Rows.Add("Ctrl+Delete", "For Deleting an row of Focused Grid");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
			dt.Rows.Add("Ctrl+ArrowRight", "For Change Focus from one Grid To another Grid");
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

	private void grdExcelDataView_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdExcelDataView.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdExcelDataView.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdExcelDataView.CurrentColumn.Key == "Delete")
				{
					DeleteDetailRow(r);
				}
				if (grdExcelDataView.CurrentColumn.Key == "Add")
				{
					AddRowInGrid();
				}
			}
			if (e.Control && e.KeyCode == Keys.D)
			{
				AddRowInGrid();
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				DeleteDetailRow(r);
			}
			((Control)(object)grdExcelDataView).Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdate_Click(object sender, EventArgs e)
	{
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Expected O, but got Unknown
		try
		{
			if (((Control)(object)cmbaccounttitle).Text.Trim() == "")
			{
				cmbaccounttitle.Focus();
				throw new Exception("Please select an account title.");
			}
			if (grdExcelDataView.GetRows().Count() > 0 && !FormHelper.ConfirmAction("Are you sure to Edit! If yes then the data in sheet data grid will be clear"))
			{
				return;
			}
			lstRemoveRecord.Clear();
			dtExpectedSchema.Clear();
			DataTable dt = BankReconciliation.FormHistory(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeId = DocumentTypeId,
				AccountId = Conversion.ToInt(cmbaccounttitle.Value),
				CanViewAllRecord = true
			});
			if (dt != null && dt.Rows.Count > 0)
			{
				foreach (DataRow dataRow in dt.Rows)
				{
					dtExpectedSchema.Rows.Add(dataRow["TransactionDate"], dataRow["ChequeNo"], dataRow["Particulars"], dataRow["Debit"], dataRow["Credit"], dataRow["Id"]);
				}
				FillExcelGrid(dtExpectedSchema);
				btnSave.Visible = false;
				btnUpdateRecords.Visible = true;
			}
			else
			{
				grdExcelDataView.DataSource = null;
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
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Expected O, but got Unknown
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Expected O, but got Unknown
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Expected O, but got Unknown
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Expected O, but got Unknown
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Expected O, but got Unknown
		//IL_0274: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Expected O, but got Unknown
		//IL_08bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c9: Expected O, but got Unknown
		GridEXLayout grdAccountHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdExcelDataView_DesignTimeLayout = new GridEXLayout();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Account_Definition.frmBankReconciliationUploadExcelSheet));
		Appearance appearance1 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance11 = new Appearance();
		GridEXLayout grdHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.grdAccountHistory = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.BtnShow = new System.Windows.Forms.Button();
		this.label3 = new System.Windows.Forms.Label();
		this.grdExcelDataView = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label1 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.label7 = new System.Windows.Forms.Label();
		this.btndownload = new System.Windows.Forms.PictureBox();
		this.cmbaccounttitle = new UltraCombo();
		this.btnUpload = new System.Windows.Forms.PictureBox();
		this.panel3 = new System.Windows.Forms.Panel();
		this.toolstrip = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.BtnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.btnShortCutKey = new System.Windows.Forms.ToolStripButton();
		this.label2 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabForm = new System.Windows.Forms.TabPage();
		this.tabhistory = new System.Windows.Forms.TabPage();
		this.grdHistory = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.label78 = new System.Windows.Forms.Label();
		this.panel7 = new System.Windows.Forms.Panel();
		this.groupBox4 = new System.Windows.Forms.GroupBox();
		this.CmbCustomerHistory = new UltraCombo();
		this.cmbDateTypeHistory = new UltraCombo();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.BtnShowhistory = new System.Windows.Forms.Button();
		this.label73 = new System.Windows.Forms.Label();
		this.label75 = new System.Windows.Forms.Label();
		this.txtToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.label5 = new System.Windows.Forms.Label();
		this.txtFromdateHistory = new System.Windows.Forms.DateTimePicker();
		this.panel9 = new System.Windows.Forms.Panel();
		this.label54 = new System.Windows.Forms.Label();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.BtnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.BtnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.Button();
		this.btnUpdateRecords = new System.Windows.Forms.ToolStripButton();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdAccountHistory).BeginInit();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdExcelDataView).BeginInit();
		this.panel4.SuspendLayout();
		this.panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.btndownload).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbaccounttitle).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.btnUpload).BeginInit();
		this.panel3.SuspendLayout();
		this.toolstrip.SuspendLayout();
		this.panel5.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabForm.SuspendLayout();
		this.tabhistory.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).BeginInit();
		this.panel8.SuspendLayout();
		this.panel7.SuspendLayout();
		this.groupBox4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDateTypeHistory).BeginInit();
		this.panel9.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.grdAccountHistory);
		this.panel1.Controls.Add(this.panel6);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(590, 52);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(914, 543);
		this.panel1.TabIndex = 0;
		this.grdAccountHistory.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdAccountHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdAccountHistory.DesignTimeLayout = grdAccountHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdAccountHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdAccountHistory.DynamicFiltering = true;
		this.grdAccountHistory.FilterMode = (FilterMode)2;
		this.grdAccountHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdAccountHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdAccountHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdAccountHistory).Location = new System.Drawing.Point(0, 29);
		((System.Windows.Forms.Control)(object)this.grdAccountHistory).Name = "grdAccountHistory";
		((ScrollableBase)this.grdAccountHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdAccountHistory).Size = new System.Drawing.Size(914, 514);
		((System.Windows.Forms.Control)(object)this.grdAccountHistory).TabIndex = 6;
		this.grdAccountHistory.TotalRow = (InheritableBoolean)1;
		this.grdAccountHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdAccountHistory).VisualStyle = (VisualStyle)4;
		this.panel6.Controls.Add(this.btnUpdate);
		this.panel6.Controls.Add(this.BtnShow);
		this.panel6.Controls.Add(this.label3);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(914, 29);
		this.panel6.TabIndex = 0;
		this.BtnShow.BackColor = System.Drawing.Color.Teal;
		this.BtnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnShow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShow.ForeColor = System.Drawing.Color.White;
		this.BtnShow.Location = new System.Drawing.Point(242, 2);
		this.BtnShow.Name = "BtnShow";
		this.BtnShow.Size = new System.Drawing.Size(58, 25);
		this.BtnShow.TabIndex = 12;
		this.BtnShow.Text = "Show";
		this.BtnShow.UseVisualStyleBackColor = false;
		this.BtnShow.Click += new System.EventHandler(BtnShow_Click);
		this.label3.AutoSize = true;
		this.label3.BackColor = System.Drawing.Color.White;
		this.label3.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold);
		this.label3.ForeColor = System.Drawing.Color.Teal;
		this.label3.Location = new System.Drawing.Point(5, 5);
		this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(227, 19);
		this.label3.TabIndex = 11;
		this.label3.Text = "History Of Selected Record";
		this.grdExcelDataView.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdExcelDataView_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdExcelDataView.DesignTimeLayout = grdExcelDataView_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdExcelDataView).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdExcelDataView.DynamicFiltering = true;
		this.grdExcelDataView.FilterMode = (FilterMode)2;
		this.grdExcelDataView.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdExcelDataView).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdExcelDataView.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdExcelDataView).Location = new System.Drawing.Point(0, 108);
		((System.Windows.Forms.Control)(object)this.grdExcelDataView).Name = "grdExcelDataView";
		((ScrollableBase)this.grdExcelDataView).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdExcelDataView).Size = new System.Drawing.Size(587, 435);
		((System.Windows.Forms.Control)(object)this.grdExcelDataView).TabIndex = 5;
		this.grdExcelDataView.TotalRow = (InheritableBoolean)1;
		this.grdExcelDataView.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdExcelDataView).VisualStyle = (VisualStyle)4;
		this.grdExcelDataView.ColumnButtonClick += new ColumnActionEventHandler(grdExcelDataView_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdExcelDataView).KeyDown += new System.Windows.Forms.KeyEventHandler(grdExcelDataView_KeyDown);
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.label1);
		this.panel4.Controls.Add(this.label4);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 76);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(587, 32);
		this.panel4.TabIndex = 6;
		this.label1.BackColor = System.Drawing.Color.Teal;
		this.label1.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.label1.ForeColor = System.Drawing.Color.Cyan;
		this.label1.Location = new System.Drawing.Point(118, 2);
		this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(410, 30);
		this.label1.TabIndex = 11;
		this.label1.Text = "Click 'Download Pic' to download the template, or 'Upload Pic' to upload the sheet.";
		this.label4.AutoSize = true;
		this.label4.BackColor = System.Drawing.Color.Teal;
		this.label4.Font = new System.Drawing.Font("Tahoma", 12.25f, System.Drawing.FontStyle.Bold);
		this.label4.ForeColor = System.Drawing.Color.White;
		this.label4.Location = new System.Drawing.Point(5, 6);
		this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(109, 21);
		this.label4.TabIndex = 11;
		this.label4.Text = "Sheet Data ";
		this.panel2.BackColor = System.Drawing.Color.White;
		this.panel2.Controls.Add(this.label7);
		this.panel2.Controls.Add(this.btndownload);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbaccounttitle);
		this.panel2.Controls.Add(this.btnUpload);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(587, 76);
		this.panel2.TabIndex = 0;
		this.label7.BackColor = System.Drawing.Color.LightGray;
		this.label7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.label7.Font = new System.Drawing.Font("Calibri", 15f, System.Drawing.FontStyle.Bold);
		this.label7.Location = new System.Drawing.Point(4, 10);
		this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(377, 30);
		this.label7.TabIndex = 4;
		this.label7.Text = "Bank Account";
		this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.btndownload.Image = (System.Drawing.Image)resources.GetObject("btndownload.Image");
		this.btndownload.Location = new System.Drawing.Point(397, 5);
		this.btndownload.Margin = new System.Windows.Forms.Padding(2);
		this.btndownload.Name = "btndownload";
		this.btndownload.Size = new System.Drawing.Size(86, 67);
		this.btndownload.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.btndownload.TabIndex = 9;
		this.btndownload.TabStop = false;
		this.btndownload.Click += new System.EventHandler(BtnDownloadPic_Click);
		this.cmbaccounttitle.AllowNull = (DefaultableBoolean)1;
		this.cmbaccounttitle.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbaccounttitle.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbaccounttitle.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbaccounttitle).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance18;
		((SpecialBoxBase)((UltraGridBase)this.cmbaccounttitle).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance23).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance23).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance27).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance32).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance33).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance33).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance33).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbaccounttitle).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.cmbaccounttitle).Location = new System.Drawing.Point(4, 44);
		((System.Windows.Forms.Control)(object)this.cmbaccounttitle).Name = "cmbaccounttitle";
		((System.Windows.Forms.Control)(object)this.cmbaccounttitle).Size = new System.Drawing.Size(377, 26);
		((System.Windows.Forms.Control)(object)this.cmbaccounttitle).TabIndex = 2;
		this.btnUpload.Image = (System.Drawing.Image)resources.GetObject("btnUpload.Image");
		this.btnUpload.Location = new System.Drawing.Point(494, 5);
		this.btnUpload.Margin = new System.Windows.Forms.Padding(2);
		this.btnUpload.Name = "btnUpload";
		this.btnUpload.Size = new System.Drawing.Size(86, 67);
		this.btnUpload.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.btnUpload.TabIndex = 8;
		this.btnUpload.TabStop = false;
		this.btnUpload.Click += new System.EventHandler(BtnUploadPic_Click);
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.toolstrip);
		this.panel3.Controls.Add(this.label2);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(3, 3);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1501, 49);
		this.panel3.TabIndex = 1;
		this.toolstrip.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolstrip.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolstrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.btnnew, this.BtnRefresh, this.btnSave, this.btnUpdateRecords, this.btnShortCutKey });
		this.toolstrip.Location = new System.Drawing.Point(0, 0);
		this.toolstrip.Name = "toolstrip";
		this.toolstrip.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
		this.toolstrip.Size = new System.Drawing.Size(1501, 27);
		this.toolstrip.TabIndex = 12;
		this.toolstrip.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.BtnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.BtnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefresh.Name = "BtnRefresh";
		this.BtnRefresh.Size = new System.Drawing.Size(80, 24);
		this.BtnRefresh.Text = "Refresh";
		this.BtnRefresh.Click += new System.EventHandler(BtnRefresh_Click);
		this.btnSave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSave.Image = (System.Drawing.Image)resources.GetObject("btnSave.Image");
		this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(64, 24);
		this.btnSave.Text = "&Save";
		this.btnSave.Click += new System.EventHandler(btnSave_Click);
		this.btnShortCutKey.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortCutKey.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortCutKey.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKey.Name = "btnShortCutKey";
		this.btnShortCutKey.Size = new System.Drawing.Size(126, 24);
		this.btnShortCutKey.Text = "ShortCut Keys";
		this.btnShortCutKey.Click += new System.EventHandler(btnShortCutKey_Click);
		this.label2.AutoSize = true;
		this.label2.BackColor = System.Drawing.Color.Teal;
		this.label2.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(2, 27);
		this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(253, 19);
		this.label2.TabIndex = 11;
		this.label2.Text = "Chart Of Account Data Upload";
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.grdExcelDataView);
		this.panel5.Controls.Add(this.panel4);
		this.panel5.Controls.Add(this.panel2);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel5.Location = new System.Drawing.Point(3, 52);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(587, 543);
		this.panel5.TabIndex = 7;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabForm);
		this.tabControl1.Controls.Add(this.tabhistory);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1515, 627);
		this.tabControl1.TabIndex = 1;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabForm.Controls.Add(this.panel1);
		this.tabForm.Controls.Add(this.panel5);
		this.tabForm.Controls.Add(this.panel3);
		this.tabForm.Location = new System.Drawing.Point(4, 4);
		this.tabForm.Name = "tabForm";
		this.tabForm.Padding = new System.Windows.Forms.Padding(3);
		this.tabForm.Size = new System.Drawing.Size(1507, 598);
		this.tabForm.TabIndex = 0;
		this.tabForm.Text = "Form";
		this.tabForm.UseVisualStyleBackColor = true;
		this.tabhistory.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistory);
		this.tabhistory.Controls.Add(this.panel8);
		this.tabhistory.Controls.Add(this.panel7);
		this.tabhistory.Controls.Add(this.panel9);
		this.tabhistory.Controls.Add(this.toolStrip2);
		this.tabhistory.Location = new System.Drawing.Point(4, 4);
		this.tabhistory.Name = "tabhistory";
		this.tabhistory.Padding = new System.Windows.Forms.Padding(3);
		this.tabhistory.Size = new System.Drawing.Size(1026, 532);
		this.tabhistory.TabIndex = 1;
		this.tabhistory.Text = "History";
		this.tabhistory.UseVisualStyleBackColor = true;
		this.grdHistory.AllowEdit = (InheritableBoolean)2;
		grdHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistory.DesignTimeLayout = grdHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistory.DynamicFiltering = true;
		this.grdHistory.FilterMode = (FilterMode)2;
		this.grdHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistory.GroupByBoxVisible = false;
		this.grdHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdHistory).Location = new System.Drawing.Point(3, 133);
		((System.Windows.Forms.Control)(object)this.grdHistory).Name = "grdHistory";
		((ScrollableBase)this.grdHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistory).Size = new System.Drawing.Size(1020, 396);
		((System.Windows.Forms.Control)(object)this.grdHistory).TabIndex = 0;
		this.grdHistory.TotalRow = (InheritableBoolean)1;
		this.grdHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistory).VisualStyle = (VisualStyle)4;
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.ctrlGrdBar2);
		this.panel8.Controls.Add(this.label78);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(3, 106);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1020, 27);
		this.panel8.TabIndex = 53;
		this.label78.AutoSize = true;
		this.label78.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label78.ForeColor = System.Drawing.Color.White;
		this.label78.Location = new System.Drawing.Point(5, 3);
		this.label78.Name = "label78";
		this.label78.Size = new System.Drawing.Size(142, 19);
		this.label78.TabIndex = 35;
		this.label78.Text = "Filtered Records";
		this.panel7.Controls.Add(this.groupBox4);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(3, 55);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1020, 51);
		this.panel7.TabIndex = 49;
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomerHistory);
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory);
		this.groupBox4.Controls.Add(this.rdmodifydate);
		this.groupBox4.Controls.Add(this.rdentrydate);
		this.groupBox4.Controls.Add(this.BtnShowhistory);
		this.groupBox4.Controls.Add(this.label73);
		this.groupBox4.Controls.Add(this.label75);
		this.groupBox4.Controls.Add(this.txtToDateHistory);
		this.groupBox4.Controls.Add(this.label5);
		this.groupBox4.Controls.Add(this.txtFromdateHistory);
		this.groupBox4.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox4.Location = new System.Drawing.Point(1, 2);
		this.groupBox4.Name = "groupBox4";
		this.groupBox4.Size = new System.Drawing.Size(949, 48);
		this.groupBox4.TabIndex = 0;
		this.groupBox4.TabStop = false;
		this.groupBox4.Text = "Filters";
		this.CmbCustomerHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomerHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomerHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomerHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance25).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance25).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance26;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance5;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance7;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance8).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance9).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance9).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance9).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance9).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomerHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Location = new System.Drawing.Point(606, 16);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Name = "CmbCustomerHistory";
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Size = new System.Drawing.Size(190, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).TabIndex = 3;
		this.cmbDateTypeHistory.AllowNull = (DefaultableBoolean)1;
		this.cmbDateTypeHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDateTypeHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDateTypeHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance35).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance35).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance35;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance36;
		((SpecialBoxBase)((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance13).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance13).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance13).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance14).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance16;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance17).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance19).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDateTypeHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Location = new System.Drawing.Point(63, 16);
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Name = "cmbDateTypeHistory";
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Size = new System.Drawing.Size(130, 26);
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).TabIndex = 0;
		this.cmbDateTypeHistory.ValueChanged += new System.EventHandler(cmbDateTypeHistory_ValueChanged);
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(855, 27);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 6;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Checked = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(855, 11);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 5;
		this.rdentrydate.TabStop = true;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.BtnShowhistory.BackColor = System.Drawing.Color.Teal;
		this.BtnShowhistory.FlatAppearance.BorderSize = 0;
		this.BtnShowhistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnShowhistory.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.BtnShowhistory.ForeColor = System.Drawing.Color.White;
		this.BtnShowhistory.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.BtnShowhistory.Location = new System.Drawing.Point(797, 17);
		this.BtnShowhistory.Margin = new System.Windows.Forms.Padding(2);
		this.BtnShowhistory.Name = "BtnShowhistory";
		this.BtnShowhistory.Size = new System.Drawing.Size(54, 25);
		this.BtnShowhistory.TabIndex = 4;
		this.BtnShowhistory.Text = "Show";
		this.BtnShowhistory.UseVisualStyleBackColor = false;
		this.BtnShowhistory.Click += new System.EventHandler(btnshow_Click);
		this.label73.AutoSize = true;
		this.label73.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label73.Location = new System.Drawing.Point(528, 22);
		this.label73.Name = "label73";
		this.label73.Size = new System.Drawing.Size(81, 15);
		this.label73.TabIndex = 178;
		this.label73.Text = "Bank Account";
		this.label75.AutoSize = true;
		this.label75.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label75.Location = new System.Drawing.Point(4, 22);
		this.label75.Name = "label75";
		this.label75.Size = new System.Drawing.Size(60, 15);
		this.label75.TabIndex = 174;
		this.label75.Text = "Date Type";
		this.txtToDateHistory.CalendarFont = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDateHistory.Checked = false;
		this.txtToDateHistory.CustomFormat = "dd-MMM-yy";
		this.txtToDateHistory.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtToDateHistory.Location = new System.Drawing.Point(402, 18);
		this.txtToDateHistory.Name = "txtToDateHistory";
		this.txtToDateHistory.ShowCheckBox = true;
		this.txtToDateHistory.Size = new System.Drawing.Size(125, 22);
		this.txtToDateHistory.TabIndex = 2;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(195, 22);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(79, 15);
		this.label5.TabIndex = 171;
		this.label5.Text = "Date From,To";
		this.txtFromdateHistory.CalendarFont = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromdateHistory.Checked = false;
		this.txtFromdateHistory.CustomFormat = "dd-MMM-yy";
		this.txtFromdateHistory.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtFromdateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtFromdateHistory.Location = new System.Drawing.Point(276, 18);
		this.txtFromdateHistory.Name = "txtFromdateHistory";
		this.txtFromdateHistory.ShowCheckBox = true;
		this.txtFromdateHistory.Size = new System.Drawing.Size(125, 22);
		this.txtFromdateHistory.TabIndex = 1;
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.label54);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(3, 28);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1020, 27);
		this.panel9.TabIndex = 52;
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label54.ForeColor = System.Drawing.Color.White;
		this.label54.Location = new System.Drawing.Point(5, 3);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(360, 19);
		this.label54.TabIndex = 35;
		this.label54.Text = "Bank Reconciliation With Vouchers History ";
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.BtnNewHistory, this.BtnRefreshHistory });
		this.toolStrip2.Location = new System.Drawing.Point(3, 3);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1020, 25);
		this.toolStrip2.TabIndex = 51;
		this.toolStrip2.Text = "toolStrip2";
		this.BtnNewHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.BtnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnNewHistory.Name = "BtnNewHistory";
		this.BtnNewHistory.Size = new System.Drawing.Size(55, 22);
		this.BtnNewHistory.Text = "New";
		this.BtnNewHistory.Click += new System.EventHandler(BtnNewHistory_Click);
		this.BtnRefreshHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.BtnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefreshHistory.Name = "BtnRefreshHistory";
		this.BtnRefreshHistory.Size = new System.Drawing.Size(76, 22);
		this.BtnRefreshHistory.Text = "Refresh";
		this.BtnRefreshHistory.Click += new System.EventHandler(BtnRefreshHistory_Click);
		this.btnUpdate.BackColor = System.Drawing.Color.Teal;
		this.btnUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.ForeColor = System.Drawing.Color.White;
		this.btnUpdate.Location = new System.Drawing.Point(300, 2);
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(58, 25);
		this.btnUpdate.TabIndex = 13;
		this.btnUpdate.Text = "Edit";
		this.btnUpdate.UseVisualStyleBackColor = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnUpdateRecords.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdateRecords.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdateRecords.Name = "btnUpdateRecords";
		this.btnUpdateRecords.Size = new System.Drawing.Size(78, 24);
		this.btnUpdateRecords.Text = "Update";
		this.btnUpdateRecords.Visible = false;
		this.btnUpdateRecords.Click += new System.EventHandler(btnUpdateRecords_Click);
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = null;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(967, 0);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar2.MyGrid = this.grdHistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(49, 25);
		this.ctrlGrdBar2.TabIndex = 34;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1515, 627);
		base.Controls.Add(this.tabControl1);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "frmBankReconciliationUploadExcelSheet";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmBankReconciliationUploadExcelSheet_KeyDown);
		this.panel1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdAccountHistory).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdExcelDataView).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.btndownload).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbaccounttitle).EndInit();
		((System.ComponentModel.ISupportInitialize)this.btnUpload).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolstrip.ResumeLayout(false);
		this.toolstrip.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabForm.ResumeLayout(false);
		this.tabhistory.ResumeLayout(false);
		this.tabhistory.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.panel7.ResumeLayout(false);
		this.groupBox4.ResumeLayout(false);
		this.groupBox4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDateTypeHistory).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}
