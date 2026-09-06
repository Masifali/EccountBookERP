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

public class frmExcelSheetUpload : Form
{
	private Microsoft.Office.Interop.Excel.Application xlsp = (Microsoft.Office.Interop.Excel.Application)Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("00024500-0000-0000-C000-000000000046")));

	private DataTable dtcoaAccountgroup = new DataTable();

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

	internal UltraCombo cmbExcelSubSheetName;

	private Label label1;

	private Label label2;

	private TextBox txtAproxAcNo;

	internal GridEX grdExcelDataView;

	private Button btnUpload;

	private Button btndownload;

	private PictureBox pictureBox2;

	private PictureBox pictureBox1;

	private Label txtFileName;

	private ComboBox cmbSheet;

	internal UltraCombo cmbaccounttitle;

	private Label label6;

	private Label label5;

	private Label label4;

	private Label label3;

	private TextBox txtAccountsText;

	private Button btnSave;

	private GroupBox groupBox1;

	public frmExcelSheetUpload(UserAccount puserAccount)
	{
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Expected O, but got Unknown
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Expected O, but got Unknown
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
				where item.Field<int>("Account_Level") == 3
				select item).CopyToDataTable();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void TitleFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters coa = new ReportsParameters();
			coa.OrganizationId = UserAccount.OrganizationId;
			coa.CompanyId = UserAccount.CompanyId;
			coa.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			coa.Account_Level = 3;
			coa.LanguageId = 0;
			dtcoaAccountgroup = ChartofAccount.ReadAllAccountgroup(coa);
			DataTable dt = new DataTable();
			dt.Columns.Add("AccountCode");
			dt.Columns.Add("AccountTitle");
			dt.Columns.Add("ParentCodeId");
			for (int i = 0; i < dtcoaAccountgroup.Rows.Count; i++)
			{
				dt.Rows.Add(dtcoaAccountgroup.Rows[i]["AccountCode"], dtcoaAccountgroup.Rows[i]["AccountTitle"], dtcoaAccountgroup.Rows[i]["Id"]);
			}
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbaccounttitle, "AccountCode", "AccountTitle", "Account Title", ZeroIndex: false);
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

	private void btndownload_Click_1(object sender, EventArgs e)
	{
		//IL_074a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0751: Expected O, but got Unknown
		//IL_0497: Unknown result type (might be due to invalid IL or missing references)
		//IL_049e: Expected O, but got Unknown
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
			dts.Columns.Add("Parent_Account_Code");
			dts.Columns.Add("Parent_Account_Title");
			dts.Columns.Add("Parent_Account_Level");
			dts.Columns.Add("Parent_AccountType_Id");
			dts.Columns.Add("Parent_Account_Type");
			dts.Columns.Add("Account_Level");
			dts.Columns.Add("CompanyIds_ForAllocation");
			dts.Columns.Add("Account_Title");
			dts.Columns.Add("OtherErpCode");
			dts.Columns.Add("PhoneNumber");
			dts.Columns.Add("OpeningBalance");
			for (int k = 0; k < dtC.Rows.Count - 1; k++)
			{
				for (int j3 = 0; j3 < dtChartOfAccount.Rows.Count; j3++)
				{
					if (RdByParentAccountTitle.Checked)
					{
						if (Conversion.ToString(dtC.Rows[k]["AccountTitle"]) == Conversion.ToString(dtChartOfAccount.Rows[j3]["AccountTitle"]))
						{
							dts.Rows.Add(dtChartOfAccount.Rows[j3]["AccountCode"], dtChartOfAccount.Rows[j3]["AccountTitle"], dtChartOfAccount.Rows[j3]["Account_Level"], dtChartOfAccount.Rows[j3]["AccountTypeId"], dtChartOfAccount.Rows[j3]["AccountType"], 4, dtChartOfAccount.Rows[j3]["CompanyId"], "", "", "", "");
							break;
						}
					}
					else if (Conversion.ToString(dtC.Rows[k]["AccountType"]) == Conversion.ToString(dtChartOfAccount.Rows[j3]["AccountType"]))
					{
						dts.Rows.Add(dtChartOfAccount.Rows[j3]["AccountCode"], dtChartOfAccount.Rows[j3]["AccountTitle"], dtChartOfAccount.Rows[j3]["Account_Level"], dtChartOfAccount.Rows[j3]["AccountTypeId"], dtChartOfAccount.Rows[j3]["AccountType"], 4, dtChartOfAccount.Rows[j3]["CompanyId"], "", "", "", "");
					}
				}
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
					for (int col = 0; col < columnNames.Length; col++)
					{
						((ExcelRangeBase)worksheet.Cells[1, col + 1]).Value = columnNames[col];
					}
					int c = 2;
					int j;
					for (j = 0; j < dtC.Rows.Count - 1; j++)
					{
						if (RdByParentAccountTitle.Checked)
						{
							DataRow row = dts.AsEnumerable().SingleOrDefault((DataRow r) => r.Field<string>("Parent_Account_Title") == Conversion.ToString(dtC.Rows[j]["AccountTitle"]));
							for (int l = 0; l < Conversion.ToInt((object)txtAproxAcNo.Text); l++)
							{
								for (int k2 = 0; k2 < columnNames.Length; k2++)
								{
									((ExcelRangeBase)worksheet.Cells[c, k2 + 1]).Value = row.ItemArray[k2];
								}
								c++;
							}
							continue;
						}
						DataTable dtByType = (from r in dts.AsEnumerable()
							where r.Field<string>("Parent_Account_Type") == Conversion.ToString(dtC.Rows[j]["AccountType"])
							select r).CopyToDataTable();
						for (int l2 = 0; l2 < dtByType.Rows.Count; l2++)
						{
							for (int m = 0; m < Conversion.ToInt((object)txtAproxAcNo.Text); m++)
							{
								for (int k3 = 0; k3 < columnNames.Length; k3++)
								{
									((ExcelRangeBase)worksheet.Cells[c, k3 + 1]).Value = dtByType.Rows[l2].ItemArray[k3];
								}
								c++;
							}
						}
					}
					string downloadsPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
					downloadsPath = Path.Combine(downloadsPath, "Downloads");
					string fileName = "Chart_Of_Account_Definition_ExcelTemplate(@EccountBookERP).xlsx";
					string filePath = Path.Combine(downloadsPath, fileName);
					byte[] fileBytes = excelPackage.GetAsByteArray();
					if (File.Exists(filePath))
					{
						int count = Directory.GetFiles(downloadsPath, "Chart_Of_Account_Definition_ExcelTemplate(@EccountBookERP)*").Count();
						filePath = Path.GetFileNameWithoutExtension(filePath);
						filePath = filePath + " (" + count + ").xlsx";
						File.WriteAllBytes(filePath, fileBytes);
						MessageBox.Show("Excel File Created Sucessfully");
					}
					else
					{
						File.WriteAllBytes(filePath, fileBytes);
						MessageBox.Show("Excel File Created Sucessfully");
					}
				}
				finally
				{
					((IDisposable)excelPackage)?.Dispose();
				}
			}
			else
			{
				ExcelPackage.LicenseContext = (LicenseContext)0;
				ExcelPackage excelPackage2 = new ExcelPackage();
				try
				{
					int i;
					for (i = 0; i < dtC.Rows.Count - 1; i++)
					{
						if (RdByParentAccountTitle.Checked)
						{
							ExcelWorksheet worksheet2 = excelPackage2.Workbook.Worksheets.Add(Conversion.ToString(dtC.Rows[i]["AccountTitle"]));
							string[] columnNames2 = (from DataColumn column in dts.Columns
								select column.ColumnName).ToArray();
							for (int col2 = 0; col2 < columnNames2.Length; col2++)
							{
								((ExcelRangeBase)worksheet2.Cells[1, col2 + 1]).Value = columnNames2[col2];
							}
							DataRow row2 = dts.AsEnumerable().SingleOrDefault((DataRow r) => r.Field<string>("Parent_Account_Title") == Conversion.ToString(dtC.Rows[i]["AccountTitle"]));
							for (int n = 0; n < Conversion.ToInt((object)txtAproxAcNo.Text); n++)
							{
								for (int k4 = 0; k4 < columnNames2.Length; k4++)
								{
									((ExcelRangeBase)worksheet2.Cells[n + 2, k4 + 1]).Value = row2.ItemArray[k4];
								}
							}
							continue;
						}
						ExcelWorksheet worksheet3 = excelPackage2.Workbook.Worksheets.Add(Conversion.ToString(dtC.Rows[i]["AccountType"]));
						string[] columnNames3 = (from DataColumn column in dts.Columns
							select column.ColumnName).ToArray();
						for (int col3 = 0; col3 < columnNames3.Length; col3++)
						{
							((ExcelRangeBase)worksheet3.Cells[1, col3 + 1]).Value = columnNames3[col3];
						}
						DataTable dtByType2 = (from r in dts.AsEnumerable()
							where r.Field<string>("Parent_Account_Type") == Conversion.ToString(dtC.Rows[i]["AccountType"])
							select r).CopyToDataTable();
						for (int l3 = 0; l3 < dtByType2.Rows.Count; l3++)
						{
							for (int j2 = 0; j2 < Conversion.ToInt((object)txtAproxAcNo.Text); j2++)
							{
								for (int k5 = 0; k5 < columnNames3.Length; k5++)
								{
									((ExcelRangeBase)worksheet3.Cells[j2 + 2, k5 + 1]).Value = dtByType2.Rows[l3].ItemArray[k5];
								}
							}
						}
					}
					string downloadsPath2 = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
					downloadsPath2 = Path.Combine(downloadsPath2, "Downloads");
					string fileName2 = "Chart_Of_Account_Definition_ExcelTemplate(@EccountBookERP).xlsx";
					string filePath2 = Path.Combine(downloadsPath2, fileName2);
					byte[] fileBytes2 = excelPackage2.GetAsByteArray();
					if (File.Exists(filePath2))
					{
						int count2 = Directory.GetFiles(downloadsPath2, "Chart_Of_Account_Definition_ExcelTemplate(@EccountBookERP)*").Count();
						filePath2 = Path.GetFileNameWithoutExtension(filePath2);
						filePath2 = filePath2 + " (" + count2 + ").xlsx";
						File.WriteAllBytes(filePath2, fileBytes2);
						MessageBox.Show("Excel File Created Sucessfully");
					}
					else
					{
						File.WriteAllBytes(filePath2, fileBytes2);
						MessageBox.Show("Excel File Created Sucessfully");
					}
				}
				finally
				{
					((IDisposable)excelPackage2)?.Dispose();
				}
			}
			Reset();
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
			txtAccountsText.Clear();
			txtAproxAcNo.Clear();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpload_Click_1(object sender, EventArgs e)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Expected O, but got Unknown
		try
		{
			using OpenFileDialog file = new OpenFileDialog
			{
				Filter = " WorkBook|*.xlsx;",
				Multiselect = false
			};
			if (file.ShowDialog() == DialogResult.OK)
			{
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
						cmbSheet.Items.Clear();
						DataTable dt = new DataTable();
						foreach (DataTable table in TableCollection)
						{
							if (dt.Columns.Count == 0)
							{
								dt.Columns.Add("ParentAccountCode");
								dt.Columns.Add("ParentAccountTitle");
							}
							dt.Rows.Add(table.Rows[0]["Parent_Account_Code"], table.Rows[0]["Parent_Account_Title"]);
							cmbSheet.Items.Add(table.TableName);
						}
						DDL.BindDDL(dt, cmbExcelSubSheetName, "ParentAccountCode", "ParentAccountTitle", "Account Title", ZeroIndex: false);
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
				grdExcelDataView.DataSource = GenericDt;
				grdExcelDataView.RetrieveStructure();
				grdExcelDataView.RootTable.Groups.Add("Parent_Account_Title");
			}
			else
			{
				grdExcelDataView.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbSheet_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			DataTable dt = TableCollection[cmbSheet.SelectedItem.ToString()];
			grdExcelDataView.DataSource = dt;
			grdExcelDataView.RetrieveStructure();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void cmbExcelSubSheetName_TextChanged(object sender, EventArgs e)
	{
		try
		{
			DataTable dt = TableCollection[((Control)(object)cmbExcelSubSheetName).Text];
			grdExcelDataView.DataSource = dt;
			grdExcelDataView.RetrieveStructure();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
				((UltraGridBase)cmbaccounttitle).DataSource = null;
				TitleFill();
			}
			else
			{
				((Control)(object)cmbaccounttitle).Text = string.Empty;
				((UltraGridBase)cmbaccounttitle).DataSource = null;
				AccountTypeFill();
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
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Expected O, but got Unknown
		try
		{
			GridEXGroup groupToRemove = grdExcelDataView.RootTable.Groups["Parent_Account_Title"];
			grdExcelDataView.RootTable.Groups.Remove(groupToRemove);
			if (grdExcelDataView.GetRows().Count() <= 0)
			{
				return;
			}
			ChartofAccount Coa = new ChartofAccount();
			Coa.ChartofAccountsList = new List<ChartofAccount>();
			GridEXRow[] rows = grdExcelDataView.GetRows();
			foreach (GridEXRow item in rows)
			{
				ChartofAccount obj = new ChartofAccount();
				obj.COAAllocationList = new List<COAAllocation>();
				if (Conversion.ToString(item.Cells["Account_Title"].Value) != "")
				{
					DataRow ParentAccountObject = dtChartOfAccount.AsEnumerable().SingleOrDefault((DataRow r) => r.Field<string>("AccountCode") == Conversion.ToString(item.Cells["Parent_Account_Code"].Value));
					obj.OrganizationId = UserAccount.OrganizationId;
					obj.CompanyId = UserAccount.CompanyId;
					obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
					obj.IsActive = true;
					obj.EntryUser = UserAccount.ID;
					obj.ModifyUser = UserAccount.ID;
					obj.PostUser = UserAccount.ID;
					obj.EntryDate = DateTime.Now;
					obj.ModifyDate = DateTime.Now;
					obj.PostDate = DateTime.Now;
					obj.ParentCodeId = Conversion.ToInt((object)ParentAccountObject.Field<int>("Id"));
					obj.ParentAccountCode = Conversion.ToString((object)ParentAccountObject.Field<string>("AccountCode"));
					obj.Account_Level = 4;
					obj.AccountClass = 0;
					obj.AccountGroup = "Detail";
					obj.AccountTitle = Conversion.ToString(item.Cells["Account_Title"].Value);
					obj.AccountCode = "0";
					obj.AccountTypeId = Conversion.ToInt((object)ParentAccountObject.Field<int>("AccountTypeId"));
					obj.PLNoteId = 0;
					obj.BSNoteId = 0;
					obj.OtherErpCode = Conversion.ToString(item.Cells["OtherErpCode"].Value);
					obj.PhoneNumber = Conversion.ToString(item.Cells["PhoneNumber"].Value);
					obj.YearObDebit = ((Conversion.ToDouble(item.Cells["OpeningBalance"].Value) > 0.0) ? Conversion.ToDouble(item.Cells["OpeningBalance"].Value) : 0.0);
					obj.YearObCredit = ((Conversion.ToDouble(item.Cells["OpeningBalance"].Value) < 0.0) ? Conversion.ToDouble(item.Cells["OpeningBalance"].Value) : 0.0);
					obj.CityId = 0;
					obj.CustomerGroupId = 0;
					obj.COAAllocationList = GenerateCoaALLocationList(Conversion.ToString(item.Cells["CompanyIds_ForAllocation"].Value));
					Coa.ChartofAccountsList.Add(obj);
				}
			}
			if (ChartofAccount.SaveList(Coa) > 0)
			{
				MessageBox.Show("Accounts Uploaded Successfully");
				GenericDt.Rows.Clear();
				grdExcelDataView.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private List<COAAllocation> GenerateCoaALLocationList(string CompanyIds)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		List<COAAllocation> AllocationList = new List<COAAllocation>();
		try
		{
			string[] listOfIds = CompanyIds.Split(',');
			for (int i = 0; i < listOfIds.Length; i++)
			{
				COAAllocation cod = new COAAllocation();
				cod.OrganizationId = UserAccount.OrganizationId;
				cod.CompanyId = Conversion.ToInt((object)listOfIds[i]);
				cod.BranchId = UserAccount.BranchesId;
				cod.IsActive = true;
				AllocationList.Add(cod);
			}
			return AllocationList;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
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
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Expected O, but got Unknown
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Expected O, but got Unknown
		//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Expected O, but got Unknown
		GridEXLayout grdExcelDataView_DesignTimeLayout = new GridEXLayout();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Common.frmExcelSheetUpload));
		Appearance appearance1 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel3 = new System.Windows.Forms.Panel();
		this.grdExcelDataView = new GridEX();
		this.panel2 = new System.Windows.Forms.Panel();
		this.btnSave = new System.Windows.Forms.Button();
		this.label6 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.cmbSheet = new System.Windows.Forms.ComboBox();
		this.txtFileName = new System.Windows.Forms.Label();
		this.cmbExcelSubSheetName = new UltraCombo();
		this.panel4 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.RdSingleSheet = new System.Windows.Forms.RadioButton();
		this.radioButton4 = new System.Windows.Forms.RadioButton();
		this.txtAccountsText = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.txtAproxAcNo = new System.Windows.Forms.TextBox();
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
		((System.ComponentModel.ISupportInitialize)this.grdExcelDataView).BeginInit();
		this.panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbExcelSubSheetName).BeginInit();
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
		this.panel1.Size = new System.Drawing.Size(794, 657);
		this.panel1.TabIndex = 0;
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.grdExcelDataView);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel3.Location = new System.Drawing.Point(0, 286);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(794, 371);
		this.panel3.TabIndex = 1;
		this.grdExcelDataView.AllowDelete = (InheritableBoolean)1;
		this.grdExcelDataView.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdExcelDataView_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdExcelDataView.DesignTimeLayout = grdExcelDataView_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdExcelDataView).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdExcelDataView.DynamicFiltering = true;
		this.grdExcelDataView.FilterMode = (FilterMode)2;
		this.grdExcelDataView.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdExcelDataView).Font = new System.Drawing.Font("Verdana", 8.25f);
		((System.Windows.Forms.Control)(object)this.grdExcelDataView).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdExcelDataView).Name = "grdExcelDataView";
		((ScrollableBase)this.grdExcelDataView).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdExcelDataView).Size = new System.Drawing.Size(794, 371);
		((System.Windows.Forms.Control)(object)this.grdExcelDataView).TabIndex = 5;
		this.grdExcelDataView.TotalRow = (InheritableBoolean)1;
		this.grdExcelDataView.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdExcelDataView).VisualStyle = (VisualStyle)4;
		this.panel2.AutoScroll = true;
		this.panel2.BackColor = System.Drawing.Color.White;
		this.panel2.Controls.Add(this.btnSave);
		this.panel2.Controls.Add(this.label6);
		this.panel2.Controls.Add(this.label5);
		this.panel2.Controls.Add(this.label4);
		this.panel2.Controls.Add(this.label3);
		this.panel2.Controls.Add(this.cmbSheet);
		this.panel2.Controls.Add(this.txtFileName);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbExcelSubSheetName);
		this.panel2.Controls.Add(this.panel4);
		this.panel2.Controls.Add(this.label1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(794, 286);
		this.panel2.TabIndex = 0;
		this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnSave.BackColor = System.Drawing.Color.Green;
		this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnSave.Font = new System.Drawing.Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSave.ForeColor = System.Drawing.Color.White;
		this.btnSave.Location = new System.Drawing.Point(720, 3);
		this.btnSave.Margin = new System.Windows.Forms.Padding(2);
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(70, 28);
		this.btnSave.TabIndex = 0;
		this.btnSave.Text = "Save";
		this.btnSave.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
		this.btnSave.UseVisualStyleBackColor = false;
		this.btnSave.Click += new System.EventHandler(btnSave_Click);
		this.label6.AutoSize = true;
		this.label6.BackColor = System.Drawing.Color.Teal;
		this.label6.Font = new System.Drawing.Font("Calibri", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.ForeColor = System.Drawing.Color.White;
		this.label6.Location = new System.Drawing.Point(389, 198);
		this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(353, 23);
		this.label6.TabIndex = 13;
		this.label6.Text = "(Filter Grid Data By Changing Sheet Name ) ";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Calibri", 10.2f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(9, 268);
		this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(57, 17);
		this.label5.TabIndex = 12;
		this.label5.Text = "File Path";
		this.label5.Visible = false;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Calibri", 10.2f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(9, 230);
		this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(105, 17);
		this.label4.TabIndex = 10;
		this.label4.Text = "Sub Sheet Name";
		this.label4.Visible = false;
		this.label3.BackColor = System.Drawing.Color.Teal;
		this.label3.Dock = System.Windows.Forms.DockStyle.Top;
		this.label3.Font = new System.Drawing.Font("Segoe UI", 16.2f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.White;
		this.label3.Location = new System.Drawing.Point(0, 192);
		this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(794, 34);
		this.label3.TabIndex = 11;
		this.label3.Text = "Sheet Data ";
		this.cmbSheet.FormattingEnabled = true;
		this.cmbSheet.Location = new System.Drawing.Point(435, 230);
		this.cmbSheet.Name = "cmbSheet";
		this.cmbSheet.Size = new System.Drawing.Size(307, 21);
		this.cmbSheet.TabIndex = 10;
		this.cmbSheet.Visible = false;
		this.cmbSheet.SelectedIndexChanged += new System.EventHandler(cmbSheet_SelectedIndexChanged);
		this.txtFileName.AutoSize = true;
		this.txtFileName.Font = new System.Drawing.Font("Calibri", 10.2f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFileName.Location = new System.Drawing.Point(121, 268);
		this.txtFileName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.txtFileName.Name = "txtFileName";
		this.txtFileName.Size = new System.Drawing.Size(112, 17);
		this.txtFileName.TabIndex = 4;
		this.txtFileName.Text = "Sheet Folder Path";
		this.txtFileName.Visible = false;
		this.cmbExcelSubSheetName.AllowNull = (DefaultableBoolean)1;
		this.cmbExcelSubSheetName.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbExcelSubSheetName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbExcelSubSheetName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbExcelSubSheetName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbExcelSubSheetName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbExcelSubSheetName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbExcelSubSheetName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbExcelSubSheetName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.cmbExcelSubSheetName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbExcelSubSheetName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.cmbExcelSubSheetName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbExcelSubSheetName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbExcelSubSheetName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbExcelSubSheetName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbExcelSubSheetName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbExcelSubSheetName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbExcelSubSheetName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbExcelSubSheetName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbExcelSubSheetName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbExcelSubSheetName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbExcelSubSheetName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbExcelSubSheetName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.cmbExcelSubSheetName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbExcelSubSheetName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbExcelSubSheetName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbExcelSubSheetName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbExcelSubSheetName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmbExcelSubSheetName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbExcelSubSheetName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbExcelSubSheetName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbExcelSubSheetName).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbExcelSubSheetName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbExcelSubSheetName).Location = new System.Drawing.Point(121, 230);
		((System.Windows.Forms.Control)(object)this.cmbExcelSubSheetName).Name = "cmbExcelSubSheetName";
		((System.Windows.Forms.Control)(object)this.cmbExcelSubSheetName).Size = new System.Drawing.Size(308, 24);
		((System.Windows.Forms.Control)(object)this.cmbExcelSubSheetName).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.cmbExcelSubSheetName).Visible = false;
		((System.Windows.Forms.Control)(object)this.cmbExcelSubSheetName).TextChanged += new System.EventHandler(cmbExcelSubSheetName_TextChanged);
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Controls.Add(this.txtAccountsText);
		this.panel4.Controls.Add(this.label2);
		this.panel4.Controls.Add(this.txtAproxAcNo);
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
		this.panel4.Size = new System.Drawing.Size(794, 158);
		this.panel4.TabIndex = 3;
		this.groupBox1.Controls.Add(this.RdSingleSheet);
		this.groupBox1.Controls.Add(this.radioButton4);
		this.groupBox1.Location = new System.Drawing.Point(281, 2);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(288, 44);
		this.groupBox1.TabIndex = 11;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Sheet";
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
		this.txtAccountsText.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAccountsText.Location = new System.Drawing.Point(5, 90);
		this.txtAccountsText.Multiline = true;
		this.txtAccountsText.Name = "txtAccountsText";
		this.txtAccountsText.Size = new System.Drawing.Size(574, 49);
		this.txtAccountsText.TabIndex = 10;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Calibri", 10.2f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(299, 60);
		this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(104, 17);
		this.label2.TabIndex = 4;
		this.label2.Text = "Approx No Of Ac";
		this.txtAproxAcNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAproxAcNo.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAproxAcNo.Location = new System.Drawing.Point(419, 60);
		this.txtAproxAcNo.Name = "txtAproxAcNo";
		this.txtAproxAcNo.Size = new System.Drawing.Size(160, 23);
		this.txtAproxAcNo.TabIndex = 3;
		this.RdByParentAccountTitle.AutoSize = true;
		this.RdByParentAccountTitle.Checked = true;
		this.RdByParentAccountTitle.Font = new System.Drawing.Font("Calibri", 10.2f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RdByParentAccountTitle.Location = new System.Drawing.Point(9, 7);
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
		this.btndownload.Location = new System.Drawing.Point(603, 120);
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
		this.btnUpload.Location = new System.Drawing.Point(700, 120);
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
		this.pictureBox2.Location = new System.Drawing.Point(603, 1);
		this.pictureBox2.Margin = new System.Windows.Forms.Padding(2);
		this.pictureBox2.Name = "pictureBox2";
		this.pictureBox2.Size = new System.Drawing.Size(90, 118);
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
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbaccounttitle).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance18;
		((SpecialBoxBase)((UltraGridBase)this.cmbaccounttitle).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance23).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance24).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbaccounttitle).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance24;
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
		((System.Windows.Forms.Control)(object)this.cmbaccounttitle).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.cmbaccounttitle).Location = new System.Drawing.Point(5, 60);
		((System.Windows.Forms.Control)(object)this.cmbaccounttitle).Name = "cmbaccounttitle";
		((System.Windows.Forms.Control)(object)this.cmbaccounttitle).Size = new System.Drawing.Size(260, 24);
		((System.Windows.Forms.Control)(object)this.cmbaccounttitle).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.cmbaccounttitle).Leave += new System.EventHandler(cmbaccounttitle_TextChanged);
		this.pictureBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.pictureBox1.Image = (System.Drawing.Image)resources.GetObject("pictureBox1.Image");
		this.pictureBox1.Location = new System.Drawing.Point(700, 0);
		this.pictureBox1.Margin = new System.Windows.Forms.Padding(2);
		this.pictureBox1.Name = "pictureBox1";
		this.pictureBox1.Size = new System.Drawing.Size(90, 119);
		this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.pictureBox1.TabIndex = 8;
		this.pictureBox1.TabStop = false;
		this.radioButton1.AutoSize = true;
		this.radioButton1.Font = new System.Drawing.Font("Calibri", 10.2f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.radioButton1.Location = new System.Drawing.Point(9, 28);
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
		this.label1.Size = new System.Drawing.Size(794, 34);
		this.label1.TabIndex = 0;
		this.label1.Text = "Chart Of Account Data Upload";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(794, 657);
		base.Controls.Add(this.panel1);
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "frmExcelSheetUpload";
		this.Text = "Excel Data Reader";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(frmExcelSheetUpload_Load);
		this.panel1.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdExcelDataView).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbExcelSubSheetName).EndInit();
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
