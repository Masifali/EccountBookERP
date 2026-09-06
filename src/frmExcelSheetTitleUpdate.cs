using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Architecture.BLL.Accounts;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Accounts;
using ExcelDataReader;
using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using Microsoft.Office.Interop.Excel;
using OfficeOpenXml;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.Common
{

public class frmExcelSheetTitleUpdate : Form
{
	private Microsoft.Office.Interop.Excel.Application xlsp = (Microsoft.Office.Interop.Excel.Application)Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("00024500-0000-0000-C000-000000000046")));

	private DataTable dtChartOfAccount = new DataTable();

	private DataTable GenericDt = new DataTable();

	public UserAccount UserAccount = new UserAccount();

	private ChartofAccount coa = new ChartofAccount();

	private DropDownBind DDL = new DropDownBind();

	private DataTableCollection TableCollection;

	private IContainer components;

	private Panel panel1;

	private Panel panel3;

	private Panel panel2;

	private RadioButton RdByParentAccountTitle;

	private RadioButton radioButton1;

	private RadioButton RdSingleSheet;

	private RadioButton radioButton4;

	private Panel panel4;

	private Label label1;

	internal GridEX grdChartofAccountsForUpdate;

	private Button btnUpload;

	private Button btndownload;

	private PictureBox pictureBox2;

	private PictureBox pictureBox1;

	internal UltraCombo cmbaccounttitle;

	private Label label3;

	private TextBox txtAccountsText;

	private Button btnSave;

	private GroupBox groupBox1;

	private Panel panel5;

	internal GridEX AccountsToBeUpdateGrid;

	private Panel panel6;

	private Panel panel7;

	private Panel panel9;

	private Panel panel8;

	private Button btnExtractAccount;

	private Label label2;

	private Label label4;

	public frmExcelSheetTitleUpdate(UserAccount puserAccount)
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Expected O, but got Unknown
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = puserAccount;
	}

	private void frmExcelSheetUpload_Load(object sender, EventArgs e)
	{
		if (RdByParentAccountTitle.Checked)
		{
			TitleFill();
		}
		GetAllChartofAccounts();
	}

	private void GetAllChartofAccounts()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			dtChartOfAccount = ChartofAccount.ReadAccountLevel(obj);
			dtChartOfAccount = (from item in dtChartOfAccount.AsEnumerable()
				where item.Field<int>("Account_Level") == 4
				select item).CopyToDataTable();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btndownload_Click_1(object sender, EventArgs e)
	{
		//IL_03c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c7: Expected O, but got Unknown
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Expected O, but got Unknown
		try
		{
			if (AccountsToBeUpdateGrid.GetCheckedRows().Count() > 0)
			{
				DataTable dts = new DataTable();
				dts.Columns.Add("Chart_Of_Account_Id", typeof(int));
				dts.Columns.Add("Account_Code", typeof(string));
				dts.Columns.Add("Account_Title", typeof(string));
				GridEXRow[] checkedRows = AccountsToBeUpdateGrid.GetCheckedRows();
				foreach (GridEXRow item in checkedRows)
				{
					dts.Rows.Add(Conversion.ToInt(item.Cells["Chart_Of_Account_Id"].Value), Conversion.ToString(item.Cells["Account_Code"].Value), Conversion.ToString(item.Cells["Account_Title"].Value));
				}
				if (RdSingleSheet.Checked)
				{
					ExcelPackage.LicenseContext = (LicenseContext)0;
					ExcelPackage excelPackage = new ExcelPackage();
					try
					{
						ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Sheet1");
						string[] columnNames = (from DataColumn column in dts.Columns
							select column.ColumnName).ToArray();
						for (int col2 = 0; col2 < columnNames.Length; col2++)
						{
							((ExcelRangeBase)worksheet.Cells[1, col2 + 1]).Value = columnNames[col2];
						}
						int c = 2;
						if (RdByParentAccountTitle.Checked)
						{
							for (int k = 0; k < dts.Rows.Count; k++)
							{
								for (int i = 0; i < columnNames.Length; i++)
								{
									((ExcelRangeBase)worksheet.Cells[c, i + 1]).Value = dts.Rows[k].ItemArray[i];
								}
								c++;
							}
						}
						else
						{
							for (int l = 0; l < dts.Rows.Count; l++)
							{
								for (int j = 0; j < columnNames.Length; j++)
								{
									((ExcelRangeBase)worksheet.Cells[c, j + 1]).Value = dts.Rows[l].ItemArray[j];
								}
								c++;
							}
						}
						((ExcelRangeBase)worksheet.Cells).AutoFitColumns();
						for (int row2 = 2; row2 <= worksheet.Dimension.End.Row; row2++)
						{
							for (int col = 1; col <= 2; col++)
							{
								((ExcelRangeBase)worksheet.Cells[row2, col]).Style.Locked = true;
							}
						}
						for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
						{
							((ExcelRangeBase)worksheet.Cells[row, 3]).Style.Locked = false;
						}
						worksheet.Protection.IsProtected = true;
						string downloadsPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
						downloadsPath = Path.Combine(downloadsPath, "Downloads");
						string fileName = "Chart_Of_Account_Updation_ExcelTemplate(@EccountBookERP).xlsx";
						string filePath = Path.Combine(downloadsPath, fileName);
						byte[] fileBytes = excelPackage.GetAsByteArray();
						if (File.Exists(filePath))
						{
							int count = Directory.GetFiles(downloadsPath, "Chart_Of_Account_Updation_ExcelTemplate(@EccountBookERP)*").Count();
							string fName = Path.GetFileNameWithoutExtension(filePath);
							filePath = Path.Combine(downloadsPath, fName + " " + count + ".xlsx");
							File.WriteAllBytes(filePath, fileBytes);
							MessageBox.Show("Excel File Created Successfully");
						}
						else
						{
							File.WriteAllBytes(filePath, fileBytes);
							MessageBox.Show("Excel File Created Successfully");
						}
						return;
					}
					finally
					{
						((IDisposable)excelPackage)?.Dispose();
					}
				}
				ExcelPackage.LicenseContext = (LicenseContext)0;
				ExcelPackage excelPackage2 = new ExcelPackage();
				try
				{
					return;
				}
				finally
				{
					((IDisposable)excelPackage2)?.Dispose();
				}
			}
			throw new Exception("No Account Selected For Update!");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void Reset()
	{
		try
		{
			((Control)(object)cmbaccounttitle).Text = string.Empty;
			AccountsToBeUpdateGrid.ClearStructure();
			grdChartofAccountsForUpdate.ClearStructure();
			txtAccountsText.Clear();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpload_Click_1(object sender, EventArgs e)
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected O, but got Unknown
		try
		{
			using OpenFileDialog file = new OpenFileDialog
			{
				Filter = " WorkBook|*.xlsx;",
				Multiselect = false
			};
			if (file.ShowDialog() == DialogResult.OK)
			{
				string desiredPattern = "Chart_Of_Account_Updation_ExcelTemplate(@EccountBookERP)";
				if (!file.FileName.Contains(desiredPattern))
				{
					throw new Exception("File Format Not Valid");
				}
				using (FileStream stream = File.Open(file.FileName, FileMode.Open, FileAccess.ReadWrite))
				{
					IExcelDataReader reader = ExcelReaderFactory.CreateReader((Stream)stream, (ExcelReaderConfiguration)null);
					try
					{
						DataSet result = ExcelDataReaderExtensions.AsDataSet(reader, new ExcelDataSetConfiguration
						{
							ConfigureDataTable = (IExcelDataReader _) => new ExcelDataTableConfiguration
							{
								UseHeaderRow = true
							}
						});
						TableCollection = result.Tables;
					}
					finally
					{
						((IDisposable)reader)?.Dispose();
					}
				}
				GenerateOneDt();
			}
			else
			{
				MessageBox.Show("please choose  .xlsx file only.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GenerateOneDt()
	{
		try
		{
			foreach (DataTable dataTable in TableCollection)
			{
				GenericDt.Merge(dataTable);
			}
			if (GenericDt.Rows.Count > 0)
			{
				grdChartofAccountsForUpdate.DataSource = GenericDt;
				grdChartofAccountsForUpdate.RetrieveStructure();
				grdChartofAccountsForUpdate.RootTable.Columns["Chart_Of_Account_Id"].Visible = false;
				grdChartofAccountsForUpdate.RootTable.Columns["Account_Title"].Width = 280;
			}
			else
			{
				grdChartofAccountsForUpdate.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbaccounttitle_TextChanged(object sender, EventArgs e)
	{
		try
		{
			txtAccountsText.Text = ((Control)(object)cmbaccounttitle).Text;
			txtAccountsText.ReadOnly = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RdByParentAccountTitle_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (RdByParentAccountTitle.Checked)
			{
				((Control)(object)cmbaccounttitle).Text = string.Empty;
				txtAccountsText.Text = string.Empty;
				((UltraGridBase)cmbaccounttitle).DataSource = null;
				AccountsToBeUpdateGrid.DataSource = null;
				TitleFill();
			}
			else
			{
				((Control)(object)cmbaccounttitle).Text = string.Empty;
				txtAccountsText.Text = string.Empty;
				((UltraGridBase)cmbaccounttitle).DataSource = null;
				AccountsToBeUpdateGrid.DataSource = null;
				AccountTypeFill();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void TitleFill()
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
			DataTable dtcoaAccountgroup = ChartofAccount.ReadAllAccountgroup(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				Account_Level = 3,
				LanguageId = 0
			});
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("AccountTitle");
			dt.Columns.Add("ParentCodeId");
			for (int i = 0; i < dtcoaAccountgroup.Rows.Count; i++)
			{
				dt.Rows.Add(dtcoaAccountgroup.Rows[i]["AccountCode"], dtcoaAccountgroup.Rows[i]["AccountTitle"], dtcoaAccountgroup.Rows[i]["Id"]);
			}
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbaccounttitle, "Id", "AccountTitle", "Account Title", ZeroIndex: false);
				((UltraGridBase)cmbaccounttitle).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				UltraGridColumn obj = ((UltraGridBase)cmbaccounttitle).DisplayLayout.Bands[0].Columns.Add();
				((KeyedSubObjectBase)obj).Key = "Selected";
				((HeaderBase)obj.Header).Caption = string.Empty;
				obj.Header.CheckBoxVisibility = (HeaderCheckBoxVisibility)2;
				obj.DataType = typeof(bool);
				((HeaderBase)obj.Header).VisiblePosition = 0;
				cmbaccounttitle.CheckedListSettings.CheckStateMember = "Selected";
				((EditorCheckedListSettings)cmbaccounttitle.CheckedListSettings).EditorValueSource = (EditorWithComboValueSource)1;
				((EditorCheckedListSettings)cmbaccounttitle.CheckedListSettings).ListSeparator = ",";
				((EditorCheckedListSettings)cmbaccounttitle.CheckedListSettings).ItemCheckArea = (ItemCheckArea)1;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AccountTypeFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		try
		{
			AccountTypes val = new AccountTypes
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			};
			DataTable dttype = new DataTable();
			dttype = AccountTypes.GetAll(val);
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("AccountType");
			for (int i = 0; i < dttype.Rows.Count; i++)
			{
				dt.Rows.Add(dttype.Rows[i]["Id"], dttype.Rows[i]["AccountType"]);
			}
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbaccounttitle, "Id", "AccountType", "Account Types", ZeroIndex: false);
			}
			UltraGridColumn obj = ((UltraGridBase)cmbaccounttitle).DisplayLayout.Bands[0].Columns.Add();
			((KeyedSubObjectBase)obj).Key = "Selected";
			((HeaderBase)obj.Header).Caption = string.Empty;
			obj.Header.CheckBoxVisibility = (HeaderCheckBoxVisibility)2;
			obj.DataType = typeof(bool);
			((HeaderBase)obj.Header).VisiblePosition = 0;
			cmbaccounttitle.CheckedListSettings.CheckStateMember = "Selected";
			((EditorCheckedListSettings)cmbaccounttitle.CheckedListSettings).EditorValueSource = (EditorWithComboValueSource)1;
			((EditorCheckedListSettings)cmbaccounttitle.CheckedListSettings).ListSeparator = ",";
			((EditorCheckedListSettings)cmbaccounttitle.CheckedListSettings).ItemCheckArea = (ItemCheckArea)1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnSave_Click(object sender, EventArgs e)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		try
		{
			if (grdChartofAccountsForUpdate.GetRows().Count() > 0)
			{
				ReportsParameters Coa = new ReportsParameters();
				Coa.ReportsParameterslist = new List<ReportsParameters>();
				GridEXRow[] rows = grdChartofAccountsForUpdate.GetRows();
				foreach (GridEXRow item in rows)
				{
					ReportsParameters obj = new ReportsParameters();
					obj.OrganizationId = UserAccount.OrganizationId;
					obj.CompanyId = UserAccount.CompanyId;
					obj.EntryUser = UserAccount.ID;
					obj.ReqType = "Update";
					obj.ChartOfAccountId = Conversion.ToInt(item.Cells["Chart_Of_Account_Id"].Value);
					obj.AccountTitle = Conversion.ToString(item.Cells["Account_Title"].Value);
					obj.OtherCode = "";
					obj.IsApproved = true;
					Coa.ReportsParameterslist.Add(obj);
				}
				ChartofAccount.DELETEANDUPDATECHARTOFACCOUNTBYID(Coa);
				MessageBox.Show("Account's UpDated Successfully");
				GenericDt.Rows.Clear();
				Reset();
				return;
			}
			throw new Exception("No Record Found For Update");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnExtractAccount_Click(object sender, EventArgs e)
	{
		try
		{
			if (((Control)(object)cmbaccounttitle).Text.Trim() == "")
			{
				throw new Exception("Please Select At Least One Parent Account Title Or Account Type To Proceed");
			}
			DataTable dtC = new DataTable();
			if (RdByParentAccountTitle.Checked)
			{
				dtC.Columns.Add("AccountTitle");
			}
			else
			{
				dtC.Columns.Add("AccountType");
			}
			string[] array = (Conversion.ToString((object)((Control)(object)cmbaccounttitle).Text) + ",").ToString().Split(',');
			foreach (string author in array)
			{
				dtC.Rows.Add(author);
			}
			DataTable dts = new DataTable();
			dts.Columns.Add("Chart_Of_Account_Id");
			dts.Columns.Add("Account_Code");
			dts.Columns.Add("Account_Title");
			for (int i = 0; i < dtC.Rows.Count - 1; i++)
			{
				for (int j = 0; j < dtChartOfAccount.Rows.Count; j++)
				{
					if (RdByParentAccountTitle.Checked)
					{
						if (Conversion.ToString(dtC.Rows[i]["AccountTitle"]) == Conversion.ToString(dtChartOfAccount.Rows[j]["ParentAccountTitle"]))
						{
							dts.Rows.Add(dtChartOfAccount.Rows[j]["Id"], dtChartOfAccount.Rows[j]["AccountCode"], dtChartOfAccount.Rows[j]["AccountTitle"]);
						}
					}
					else if (Conversion.ToString(dtC.Rows[i]["AccountType"]) == Conversion.ToString(dtChartOfAccount.Rows[j]["AccountType"]))
					{
						dts.Rows.Add(dtChartOfAccount.Rows[j]["Id"], dtChartOfAccount.Rows[j]["AccountCode"], dtChartOfAccount.Rows[j]["AccountTitle"]);
					}
				}
			}
			AccountsToBeUpdateGrid.DataSource = dts;
			AccountsToBeUpdateGrid.RetrieveStructure();
			AccountsToBeUpdateGrid.RootTable.Columns["Chart_Of_Account_Id"].Visible = false;
			AccountsToBeUpdateGrid.RootTable.Columns["Account_Title"].Width = 250;
			AccountsToBeUpdateGrid.RootTable.Columns.Add("Select");
			AccountsToBeUpdateGrid.RootTable.Columns["Select"].ActAsSelector = true;
			AccountsToBeUpdateGrid.RootTable.Columns["Select"].UseHeaderSelector = true;
			AccountsToBeUpdateGrid.RootTable.Columns["Select"].Position = 0;
			AccountsToBeUpdateGrid.RootTable.Columns["Select"].Width = 30;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmExcelSheetTitleUpdate_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				Reset();
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.U)
			{
				btnSave_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				btndownload_Click_1(null, null);
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				btnUpload_Click_1(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Expected O, but got Unknown
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Expected O, but got Unknown
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Expected O, but got Unknown
		GridEXLayout grdChartofAccountsForUpdate_DesignTimeLayout = new GridEXLayout();
		GridEXLayout AccountsToBeUpdateGrid_DesignTimeLayout = new GridEXLayout();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Common.frmExcelSheetTitleUpdate));
		Appearance appearance1 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel3 = new System.Windows.Forms.Panel();
		this.panel7 = new System.Windows.Forms.Panel();
		this.panel9 = new System.Windows.Forms.Panel();
		this.grdChartofAccountsForUpdate = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.label4 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.AccountsToBeUpdateGrid = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label2 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.btnSave = new System.Windows.Forms.Button();
		this.label3 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.btnExtractAccount = new System.Windows.Forms.Button();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.RdSingleSheet = new System.Windows.Forms.RadioButton();
		this.radioButton4 = new System.Windows.Forms.RadioButton();
		this.txtAccountsText = new System.Windows.Forms.TextBox();
		this.RdByParentAccountTitle = new System.Windows.Forms.RadioButton();
		this.btndownload = new System.Windows.Forms.Button();
		this.btnUpload = new System.Windows.Forms.Button();
		this.pictureBox2 = new System.Windows.Forms.PictureBox();
		this.cmbaccounttitle = new UltraCombo();
		this.pictureBox1 = new System.Windows.Forms.PictureBox();
		this.radioButton1 = new System.Windows.Forms.RadioButton();
		this.label1 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.panel3.SuspendLayout();
		this.panel7.SuspendLayout();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdChartofAccountsForUpdate).BeginInit();
		this.panel8.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.AccountsToBeUpdateGrid).BeginInit();
		this.panel6.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel4.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.pictureBox2).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbaccounttitle).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(994, 742);
		this.panel1.TabIndex = 0;
		this.panel3.Controls.Add(this.panel7);
		this.panel3.Controls.Add(this.panel5);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel3.Location = new System.Drawing.Point(0, 214);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(994, 528);
		this.panel3.TabIndex = 1;
		this.panel7.Controls.Add(this.panel9);
		this.panel7.Controls.Add(this.panel8);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel7.Location = new System.Drawing.Point(382, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(382, 528);
		this.panel7.TabIndex = 7;
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.grdChartofAccountsForUpdate);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel9.Location = new System.Drawing.Point(0, 31);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(382, 497);
		this.panel9.TabIndex = 2;
		this.grdChartofAccountsForUpdate.AllowDelete = (InheritableBoolean)1;
		this.grdChartofAccountsForUpdate.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdChartofAccountsForUpdate_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdChartofAccountsForUpdate.DesignTimeLayout = grdChartofAccountsForUpdate_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdChartofAccountsForUpdate).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdChartofAccountsForUpdate.DynamicFiltering = true;
		this.grdChartofAccountsForUpdate.FilterMode = (FilterMode)2;
		this.grdChartofAccountsForUpdate.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdChartofAccountsForUpdate).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdChartofAccountsForUpdate.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdChartofAccountsForUpdate).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdChartofAccountsForUpdate).Name = "grdChartofAccountsForUpdate";
		((ScrollableBase)this.grdChartofAccountsForUpdate).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdChartofAccountsForUpdate).Size = new System.Drawing.Size(382, 497);
		((System.Windows.Forms.Control)(object)this.grdChartofAccountsForUpdate).TabIndex = 5;
		this.grdChartofAccountsForUpdate.TotalRow = (InheritableBoolean)1;
		this.grdChartofAccountsForUpdate.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdChartofAccountsForUpdate).VisualStyle = (VisualStyle)4;
		this.panel8.BackColor = System.Drawing.Color.White;
		this.panel8.Controls.Add(this.label4);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.ForeColor = System.Drawing.Color.Teal;
		this.panel8.Location = new System.Drawing.Point(0, 0);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(382, 31);
		this.panel8.TabIndex = 1;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(25, 7);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(328, 16);
		this.label4.TabIndex = 1;
		this.label4.Text = "Selected Chart of Accounts Whose Title's Change";
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.AccountsToBeUpdateGrid);
		this.panel5.Controls.Add(this.panel6);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(382, 528);
		this.panel5.TabIndex = 6;
		this.AccountsToBeUpdateGrid.AllowDelete = (InheritableBoolean)1;
		this.AccountsToBeUpdateGrid.DefaultFilterRowComparison = (FilterConditionOperator)7;
		AccountsToBeUpdateGrid_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.AccountsToBeUpdateGrid.DesignTimeLayout = AccountsToBeUpdateGrid_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.AccountsToBeUpdateGrid).Dock = System.Windows.Forms.DockStyle.Fill;
		this.AccountsToBeUpdateGrid.DynamicFiltering = true;
		this.AccountsToBeUpdateGrid.FilterMode = (FilterMode)2;
		this.AccountsToBeUpdateGrid.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.AccountsToBeUpdateGrid).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.AccountsToBeUpdateGrid.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.AccountsToBeUpdateGrid).Location = new System.Drawing.Point(0, 31);
		((System.Windows.Forms.Control)(object)this.AccountsToBeUpdateGrid).Name = "AccountsToBeUpdateGrid";
		((ScrollableBase)this.AccountsToBeUpdateGrid).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.AccountsToBeUpdateGrid).Size = new System.Drawing.Size(382, 497);
		((System.Windows.Forms.Control)(object)this.AccountsToBeUpdateGrid).TabIndex = 6;
		this.AccountsToBeUpdateGrid.TotalRow = (InheritableBoolean)1;
		this.AccountsToBeUpdateGrid.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.AccountsToBeUpdateGrid).VisualStyle = (VisualStyle)4;
		this.panel6.BackColor = System.Drawing.Color.White;
		this.panel6.Controls.Add(this.label2);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.ForeColor = System.Drawing.Color.Teal;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(382, 31);
		this.panel6.TabIndex = 0;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(13, 7);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(353, 16);
		this.label2.TabIndex = 0;
		this.label2.Text = "Chart of Accounts Whose Title's you Wannna Change";
		this.panel2.AutoScroll = true;
		this.panel2.BackColor = System.Drawing.Color.White;
		this.panel2.Controls.Add(this.btnSave);
		this.panel2.Controls.Add(this.label3);
		this.panel2.Controls.Add(this.panel4);
		this.panel2.Controls.Add(this.label1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(994, 214);
		this.panel2.TabIndex = 0;
		this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnSave.BackColor = System.Drawing.Color.Green;
		this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnSave.Font = new System.Drawing.Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSave.ForeColor = System.Drawing.Color.White;
		this.btnSave.Location = new System.Drawing.Point(912, 3);
		this.btnSave.Margin = new System.Windows.Forms.Padding(2);
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(70, 28);
		this.btnSave.TabIndex = 0;
		this.btnSave.Text = "Update";
		this.btnSave.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
		this.btnSave.UseVisualStyleBackColor = false;
		this.btnSave.Click += new System.EventHandler(btnSave_Click);
		this.label3.BackColor = System.Drawing.Color.Teal;
		this.label3.Dock = System.Windows.Forms.DockStyle.Top;
		this.label3.Font = new System.Drawing.Font("Segoe UI", 16.2f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.White;
		this.label3.Location = new System.Drawing.Point(0, 182);
		this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(994, 32);
		this.label3.TabIndex = 11;
		this.label3.Text = "Accounts";
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.btnExtractAccount);
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Controls.Add(this.txtAccountsText);
		this.panel4.Controls.Add(this.RdByParentAccountTitle);
		this.panel4.Controls.Add(this.btndownload);
		this.panel4.Controls.Add(this.btnUpload);
		this.panel4.Controls.Add(this.pictureBox2);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbaccounttitle);
		this.panel4.Controls.Add(this.pictureBox1);
		this.panel4.Controls.Add(this.radioButton1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 34);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(994, 148);
		this.panel4.TabIndex = 3;
		this.btnExtractAccount.BackColor = System.Drawing.Color.Teal;
		this.btnExtractAccount.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnExtractAccount.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnExtractAccount.ForeColor = System.Drawing.Color.White;
		this.btnExtractAccount.Location = new System.Drawing.Point(360, 43);
		this.btnExtractAccount.Name = "btnExtractAccount";
		this.btnExtractAccount.Size = new System.Drawing.Size(66, 24);
		this.btnExtractAccount.TabIndex = 3;
		this.btnExtractAccount.Text = "Show";
		this.btnExtractAccount.UseVisualStyleBackColor = false;
		this.btnExtractAccount.Click += new System.EventHandler(btnExtractAccount_Click);
		this.groupBox1.Controls.Add(this.RdSingleSheet);
		this.groupBox1.Controls.Add(this.radioButton4);
		this.groupBox1.Location = new System.Drawing.Point(447, 7);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(288, 40);
		this.groupBox1.TabIndex = 11;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Sheet";
		this.groupBox1.Visible = false;
		this.RdSingleSheet.AutoSize = true;
		this.RdSingleSheet.Checked = true;
		this.RdSingleSheet.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RdSingleSheet.Location = new System.Drawing.Point(8, 14);
		this.RdSingleSheet.Margin = new System.Windows.Forms.Padding(2);
		this.RdSingleSheet.Name = "RdSingleSheet";
		this.RdSingleSheet.Size = new System.Drawing.Size(111, 23);
		this.RdSingleSheet.TabIndex = 0;
		this.RdSingleSheet.TabStop = true;
		this.RdSingleSheet.Text = "Single Sheet";
		this.RdSingleSheet.UseVisualStyleBackColor = true;
		this.radioButton4.AutoSize = true;
		this.radioButton4.Font = new System.Drawing.Font("Calibri", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.radioButton4.Location = new System.Drawing.Point(154, 14);
		this.radioButton4.Margin = new System.Windows.Forms.Padding(2);
		this.radioButton4.Name = "radioButton4";
		this.radioButton4.Size = new System.Drawing.Size(127, 23);
		this.radioButton4.TabIndex = 1;
		this.radioButton4.Text = "Multiple Sheet";
		this.radioButton4.UseVisualStyleBackColor = true;
		this.txtAccountsText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAccountsText.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAccountsText.Location = new System.Drawing.Point(12, 74);
		this.txtAccountsText.Multiline = true;
		this.txtAccountsText.Name = "txtAccountsText";
		this.txtAccountsText.Size = new System.Drawing.Size(599, 69);
		this.txtAccountsText.TabIndex = 10;
		this.RdByParentAccountTitle.AutoSize = true;
		this.RdByParentAccountTitle.Checked = true;
		this.RdByParentAccountTitle.Font = new System.Drawing.Font("Calibri", 10.2f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RdByParentAccountTitle.Location = new System.Drawing.Point(13, 10);
		this.RdByParentAccountTitle.Margin = new System.Windows.Forms.Padding(2);
		this.RdByParentAccountTitle.Name = "RdByParentAccountTitle";
		this.RdByParentAccountTitle.Size = new System.Drawing.Size(157, 21);
		this.RdByParentAccountTitle.TabIndex = 0;
		this.RdByParentAccountTitle.TabStop = true;
		this.RdByParentAccountTitle.Text = "By Parent Acount Tilte";
		this.RdByParentAccountTitle.UseVisualStyleBackColor = true;
		this.RdByParentAccountTitle.CheckedChanged += new System.EventHandler(RdByParentAccountTitle_CheckedChanged);
		this.btndownload.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btndownload.BackColor = System.Drawing.Color.Green;
		this.btndownload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btndownload.Font = new System.Drawing.Font("Segoe UI", 10.2f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btndownload.ForeColor = System.Drawing.Color.White;
		this.btndownload.Location = new System.Drawing.Point(803, 109);
		this.btndownload.Margin = new System.Windows.Forms.Padding(2);
		this.btndownload.Name = "btndownload";
		this.btndownload.Size = new System.Drawing.Size(90, 34);
		this.btndownload.TabIndex = 4;
		this.btndownload.Text = "Download";
		this.btndownload.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
		this.btndownload.UseVisualStyleBackColor = false;
		this.btndownload.Click += new System.EventHandler(btndownload_Click_1);
		this.btnUpload.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnUpload.BackColor = System.Drawing.Color.Green;
		this.btnUpload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpload.Font = new System.Drawing.Font("Segoe UI", 10.2f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpload.ForeColor = System.Drawing.Color.White;
		this.btnUpload.Location = new System.Drawing.Point(900, 109);
		this.btnUpload.Margin = new System.Windows.Forms.Padding(2);
		this.btnUpload.Name = "btnUpload";
		this.btnUpload.Size = new System.Drawing.Size(90, 34);
		this.btnUpload.TabIndex = 5;
		this.btnUpload.Text = "Upload";
		this.btnUpload.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
		this.btnUpload.UseVisualStyleBackColor = false;
		this.btnUpload.Click += new System.EventHandler(btnUpload_Click_1);
		this.pictureBox2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.pictureBox2.Image = (System.Drawing.Image)resources.GetObject("pictureBox2.Image");
		this.pictureBox2.Location = new System.Drawing.Point(803, 1);
		this.pictureBox2.Margin = new System.Windows.Forms.Padding(2);
		this.pictureBox2.Name = "pictureBox2";
		this.pictureBox2.Size = new System.Drawing.Size(90, 106);
		this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.pictureBox2.TabIndex = 9;
		this.pictureBox2.TabStop = false;
		this.cmbaccounttitle.AllowNull = (DefaultableBoolean)1;
		this.cmbaccounttitle.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbaccounttitle.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbaccounttitle.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance5).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance5).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbaccounttitle).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance5;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance6;
		((SpecialBoxBase)((UltraGridBase)this.cmbaccounttitle).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance7).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance7).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance10;
		((AppearanceBase)appearance11).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance11).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbaccounttitle).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.cmbaccounttitle).Location = new System.Drawing.Point(12, 42);
		((System.Windows.Forms.Control)(object)this.cmbaccounttitle).Name = "cmbaccounttitle";
		((System.Windows.Forms.Control)(object)this.cmbaccounttitle).Size = new System.Drawing.Size(342, 26);
		((System.Windows.Forms.Control)(object)this.cmbaccounttitle).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.cmbaccounttitle).Leave += new System.EventHandler(cmbaccounttitle_TextChanged);
		this.pictureBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.pictureBox1.Image = (System.Drawing.Image)resources.GetObject("pictureBox1.Image");
		this.pictureBox1.Location = new System.Drawing.Point(900, 0);
		this.pictureBox1.Margin = new System.Windows.Forms.Padding(2);
		this.pictureBox1.Name = "pictureBox1";
		this.pictureBox1.Size = new System.Drawing.Size(90, 107);
		this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.pictureBox1.TabIndex = 8;
		this.pictureBox1.TabStop = false;
		this.radioButton1.AutoSize = true;
		this.radioButton1.Font = new System.Drawing.Font("Calibri", 10.2f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.radioButton1.Location = new System.Drawing.Point(195, 10);
		this.radioButton1.Margin = new System.Windows.Forms.Padding(2);
		this.radioButton1.Name = "radioButton1";
		this.radioButton1.Size = new System.Drawing.Size(161, 21);
		this.radioButton1.TabIndex = 1;
		this.radioButton1.Text = "By Parent Acount Type";
		this.radioButton1.UseVisualStyleBackColor = true;
		this.label1.BackColor = System.Drawing.Color.Teal;
		this.label1.Dock = System.Windows.Forms.DockStyle.Top;
		this.label1.Font = new System.Drawing.Font("Segoe UI", 16.2f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(0, 0);
		this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(994, 34);
		this.label1.TabIndex = 0;
		this.label1.Text = "Filter WorkBook Data";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(994, 742);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "frmExcelSheetTitleUpdate";
		this.Text = "Excel Data Reader";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(frmExcelSheetUpload_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmExcelSheetTitleUpdate_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		this.panel9.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdChartofAccountsForUpdate).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.panel5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.AccountsToBeUpdateGrid).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.pictureBox2).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbaccounttitle).EndInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
		base.ResumeLayout(false);
	}
}

}
