using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Architecture.BLL;
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

namespace Architecture.WinApp.Common;

public class frmAttributeVarientsExcelSheetUpload : Form
{
	private Microsoft.Office.Interop.Excel.Application xlsp = (Microsoft.Office.Interop.Excel.Application)Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("00024500-0000-0000-C000-000000000046")));

	private DataTable dt = new DataTable();

	private DataTable dtChartOfAccount = new DataTable();

	private DataTable GenericDt = new DataTable();

	public UserAccount UserAccount = new UserAccount();

	private ChartofAccount coa = new ChartofAccount();

	private DropDownBind DDL = new DropDownBind();

	private DataTableCollection TableCollection;

	private IContainer components;

	private Panel panel1;

	private Panel panel2;

	private Panel panel22;

	private RadioButton RdSingleSheet;

	private RadioButton radioButton4;

	internal UltraCombo cmbExcelSubSheetName;

	private Label label2;

	private TextBox txtAproxAcNo;

	internal GridEX grdExcelDataView;

	private Button btnUpload;

	private Button btndownload;

	private PictureBox pictureBox2;

	private PictureBox pictureBox1;

	private Label txtFileName;

	private ComboBox cmbSheet;

	internal UltraCombo CmbAttributeName;

	private Label label6;

	private Label label5;

	private Label label4;

	private TextBox txtAttributeNameText;

	private Label label7;

	private Label label3;

	private Label label8;

	private Label label1;

	private ToolStrip toolStrip1;

	private ToolStripButton btnSave;

	public frmAttributeVarientsExcelSheetUpload(UserAccount puserAccount)
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
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(331, 489);
			AttributeNameFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void AttributeNameFill()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		try
		{
			dt.Clear();
			ItemAttribute obj = new ItemAttribute();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			dt = ItemAttribute.FormHistory(obj);
			if (dt.Rows.Count > 0)
			{
				DataTable dtcombo = new DataTable();
				dtcombo.Columns.Add("Id", typeof(int));
				dtcombo.Columns.Add("AttributeDescription");
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtcombo.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["AttributeDescription"]);
				}
				if (dtcombo.Rows.Count > 0)
				{
					DDL.BindDDL(dt, CmbAttributeName, "Id", "AttributeDescription", "Attribute Description", ZeroIndex: false);
					UltraGridColumn obj2 = ((UltraGridBase)CmbAttributeName).DisplayLayout.Bands[0].Columns.Add();
					((KeyedSubObjectBase)obj2).Key = "Selected";
					((HeaderBase)obj2.Header).Caption = string.Empty;
					obj2.Header.CheckBoxVisibility = (HeaderCheckBoxVisibility)2;
					obj2.DataType = typeof(bool);
					((HeaderBase)obj2.Header).VisiblePosition = 0;
					CmbAttributeName.CheckedListSettings.CheckStateMember = "Selected";
					((EditorCheckedListSettings)CmbAttributeName.CheckedListSettings).EditorValueSource = (EditorWithComboValueSource)1;
					((EditorCheckedListSettings)CmbAttributeName.CheckedListSettings).ListSeparator = ",";
					((EditorCheckedListSettings)CmbAttributeName.CheckedListSettings).ItemCheckArea = (ItemCheckArea)1;
				}
			}
			else
			{
				((UltraGridBase)CmbAttributeName).DataSource = null;
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
			txtAttributeNameText.Text = ((Control)(object)CmbAttributeName).Text;
			txtAttributeNameText.ReadOnly = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Reset()
	{
		try
		{
			((Control)(object)CmbAttributeName).Text = string.Empty;
			txtAttributeNameText.Clear();
			txtAproxAcNo.Clear();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btndownload_Click_1(object sender, EventArgs e)
	{
		//IL_0547: Unknown result type (might be due to invalid IL or missing references)
		//IL_054e: Expected O, but got Unknown
		//IL_0319: Unknown result type (might be due to invalid IL or missing references)
		//IL_0320: Expected O, but got Unknown
		try
		{
			if (((Control)(object)CmbAttributeName).Text.Trim() == "")
			{
				throw new Exception("Please Select At Least One AttributeName To Proceed");
			}
			DataTable dtC = new DataTable();
			dtC.Columns.Add("AttributeName");
			string[] array = (Conversion.ToString((object)((Control)(object)CmbAttributeName).Text) + ",").ToString().Split(',');
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
				for (int n = 0; n < dtChartOfAccount.Rows.Count; n++)
				{
					if (Conversion.ToString(dtC.Rows[k]["AttributeName"]) == Conversion.ToString(dtChartOfAccount.Rows[n]["AccountType"]))
					{
						dts.Rows.Add(dtChartOfAccount.Rows[n]["AccountCode"], dtChartOfAccount.Rows[n]["AccountTitle"], dtChartOfAccount.Rows[n]["Account_Level"], dtChartOfAccount.Rows[n]["AccountTypeId"], dtChartOfAccount.Rows[n]["AccountType"], 4, dtChartOfAccount.Rows[n]["CompanyId"], "", "", "", "");
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
						DataTable dtByType = (from r in dts.AsEnumerable()
							where r.Field<string>("Parent_Account_Type") == Conversion.ToString(dtC.Rows[j]["AccountType"])
							select r).CopyToDataTable();
						for (int l2 = 0; l2 < dtByType.Rows.Count; l2++)
						{
							for (int l = 0; l < Conversion.ToInt((object)txtAproxAcNo.Text); l++)
							{
								for (int k2 = 0; k2 < columnNames.Length; k2++)
								{
									((ExcelRangeBase)worksheet.Cells[c, k2 + 1]).Value = dtByType.Rows[l2].ItemArray[k2];
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
						ExcelWorksheet worksheet2 = excelPackage2.Workbook.Worksheets.Add(Conversion.ToString(dtC.Rows[i]["AccountType"]));
						string[] columnNames2 = (from DataColumn column in dts.Columns
							select column.ColumnName).ToArray();
						for (int col2 = 0; col2 < columnNames2.Length; col2++)
						{
							((ExcelRangeBase)worksheet2.Cells[1, col2 + 1]).Value = columnNames2[col2];
						}
						DataTable dtByType2 = (from r in dts.AsEnumerable()
							where r.Field<string>("Parent_Account_Type") == Conversion.ToString(dtC.Rows[i]["AccountType"])
							select r).CopyToDataTable();
						for (int l3 = 0; l3 < dtByType2.Rows.Count; l3++)
						{
							for (int m = 0; m < Conversion.ToInt((object)txtAproxAcNo.Text); m++)
							{
								for (int k3 = 0; k3 < columnNames2.Length; k3++)
								{
									((ExcelRangeBase)worksheet2.Cells[m + 2, k3 + 1]).Value = dtByType2.Rows[l3].ItemArray[k3];
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
					Coa.ChartofAccountsList.Add(obj);
				}
			}
			if (ChartofAccount.SaveList(Coa) > 0)
			{
				MessageBox.Show("Accounts Uploaded Sucessfull");
				GenericDt.Rows.Clear();
				grdExcelDataView.ClearStructure();
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
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Expected O, but got Unknown
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Expected O, but got Unknown
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Expected O, but got Unknown
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Common.frmAttributeVarientsExcelSheetUpload));
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel2 = new System.Windows.Forms.Panel();
		this.grdExcelDataView = new GridEX();
		this.panel22 = new System.Windows.Forms.Panel();
		this.label6 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.cmbSheet = new System.Windows.Forms.ComboBox();
		this.txtFileName = new System.Windows.Forms.Label();
		this.cmbExcelSubSheetName = new UltraCombo();
		this.RdSingleSheet = new System.Windows.Forms.RadioButton();
		this.radioButton4 = new System.Windows.Forms.RadioButton();
		this.txtAttributeNameText = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.txtAproxAcNo = new System.Windows.Forms.TextBox();
		this.btndownload = new System.Windows.Forms.Button();
		this.btnUpload = new System.Windows.Forms.Button();
		this.CmbAttributeName = new UltraCombo();
		this.label7 = new System.Windows.Forms.Label();
		this.pictureBox2 = new System.Windows.Forms.PictureBox();
		this.pictureBox1 = new System.Windows.Forms.PictureBox();
		this.label3 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.label1 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdExcelDataView).BeginInit();
		this.panel22.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbExcelSubSheetName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAttributeName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBox2).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
		this.toolStrip1.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.label6);
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.grdExcelDataView);
		this.panel1.Controls.Add(this.label3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Controls.Add(this.panel22);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(794, 657);
		this.panel1.TabIndex = 0;
		this.panel2.BackColor = System.Drawing.Color.White;
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbExcelSubSheetName);
		this.panel2.Controls.Add(this.txtFileName);
		this.panel2.Controls.Add(this.label5);
		this.panel2.Controls.Add(this.cmbSheet);
		this.panel2.Controls.Add(this.label4);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 176);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(794, 64);
		this.panel2.TabIndex = 1;
		this.grdExcelDataView.AllowDelete = (InheritableBoolean)1;
		this.grdExcelDataView.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdExcelDataView_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdExcelDataView.DesignTimeLayout = grdExcelDataView_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdExcelDataView).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdExcelDataView.DynamicFiltering = true;
		this.grdExcelDataView.FilterMode = (FilterMode)2;
		this.grdExcelDataView.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdExcelDataView).Font = new System.Drawing.Font("Verdana", 8.25f);
		((System.Windows.Forms.Control)(object)this.grdExcelDataView).Location = new System.Drawing.Point(0, 264);
		((System.Windows.Forms.Control)(object)this.grdExcelDataView).Name = "grdExcelDataView";
		((ScrollableBase)this.grdExcelDataView).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdExcelDataView).Size = new System.Drawing.Size(794, 393);
		((System.Windows.Forms.Control)(object)this.grdExcelDataView).TabIndex = 1;
		this.grdExcelDataView.TotalRow = (InheritableBoolean)1;
		this.grdExcelDataView.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdExcelDataView).VisualStyle = (VisualStyle)4;
		this.panel22.AutoScroll = true;
		this.panel22.BackColor = System.Drawing.Color.White;
		this.panel22.Controls.Add(this.label7);
		this.panel22.Controls.Add(this.label1);
		this.panel22.Controls.Add(this.toolStrip1);
		this.panel22.Controls.Add(this.RdSingleSheet);
		this.panel22.Controls.Add(this.radioButton4);
		this.panel22.Controls.Add(this.txtAttributeNameText);
		this.panel22.Controls.Add(this.label8);
		this.panel22.Controls.Add(this.label2);
		this.panel22.Controls.Add(this.pictureBox1);
		this.panel22.Controls.Add(this.txtAproxAcNo);
		this.panel22.Controls.Add((System.Windows.Forms.Control)(object)this.CmbAttributeName);
		this.panel22.Controls.Add(this.btndownload);
		this.panel22.Controls.Add(this.pictureBox2);
		this.panel22.Controls.Add(this.btnUpload);
		this.panel22.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel22.Location = new System.Drawing.Point(0, 0);
		this.panel22.Margin = new System.Windows.Forms.Padding(2);
		this.panel22.Name = "panel22";
		this.panel22.Size = new System.Drawing.Size(794, 176);
		this.panel22.TabIndex = 0;
		this.label6.AutoSize = true;
		this.label6.BackColor = System.Drawing.Color.Teal;
		this.label6.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.ForeColor = System.Drawing.Color.White;
		this.label6.Location = new System.Drawing.Point(112, 243);
		this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(287, 16);
		this.label6.TabIndex = 13;
		this.label6.Text = "(Filter Grid Data By Changing Sheet Name ) ";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label5.Location = new System.Drawing.Point(419, 12);
		this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(52, 15);
		this.label5.TabIndex = 12;
		this.label5.Text = "File Path";
		this.label5.Visible = false;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(5, 12);
		this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(96, 15);
		this.label4.TabIndex = 10;
		this.label4.Text = "Sub Sheet Name";
		this.label4.Visible = false;
		this.cmbSheet.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbSheet.FormattingEnabled = true;
		this.cmbSheet.Location = new System.Drawing.Point(102, 35);
		this.cmbSheet.Name = "cmbSheet";
		this.cmbSheet.Size = new System.Drawing.Size(308, 24);
		this.cmbSheet.TabIndex = 2;
		this.cmbSheet.Visible = false;
		this.cmbSheet.SelectedIndexChanged += new System.EventHandler(cmbSheet_SelectedIndexChanged);
		this.txtFileName.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFileName.Location = new System.Drawing.Point(478, 6);
		this.txtFileName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.txtFileName.Name = "txtFileName";
		this.txtFileName.Size = new System.Drawing.Size(312, 53);
		this.txtFileName.TabIndex = 3;
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
		((System.Windows.Forms.Control)(object)this.cmbExcelSubSheetName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbExcelSubSheetName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbExcelSubSheetName).Location = new System.Drawing.Point(102, 6);
		((System.Windows.Forms.Control)(object)this.cmbExcelSubSheetName).Name = "cmbExcelSubSheetName";
		((System.Windows.Forms.Control)(object)this.cmbExcelSubSheetName).Size = new System.Drawing.Size(308, 26);
		((System.Windows.Forms.Control)(object)this.cmbExcelSubSheetName).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.cmbExcelSubSheetName).Visible = false;
		((System.Windows.Forms.Control)(object)this.cmbExcelSubSheetName).TextChanged += new System.EventHandler(cmbExcelSubSheetName_TextChanged);
		this.RdSingleSheet.AutoSize = true;
		this.RdSingleSheet.Checked = true;
		this.RdSingleSheet.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RdSingleSheet.Location = new System.Drawing.Point(164, 124);
		this.RdSingleSheet.Margin = new System.Windows.Forms.Padding(2);
		this.RdSingleSheet.Name = "RdSingleSheet";
		this.RdSingleSheet.Size = new System.Drawing.Size(91, 19);
		this.RdSingleSheet.TabIndex = 2;
		this.RdSingleSheet.TabStop = true;
		this.RdSingleSheet.Text = "Single Sheet";
		this.RdSingleSheet.UseVisualStyleBackColor = true;
		this.radioButton4.AutoSize = true;
		this.radioButton4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.radioButton4.Location = new System.Drawing.Point(164, 144);
		this.radioButton4.Margin = new System.Windows.Forms.Padding(2);
		this.radioButton4.Name = "radioButton4";
		this.radioButton4.Size = new System.Drawing.Size(102, 19);
		this.radioButton4.TabIndex = 3;
		this.radioButton4.Text = "Multiple Sheet";
		this.radioButton4.UseVisualStyleBackColor = true;
		this.txtAttributeNameText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAttributeNameText.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAttributeNameText.Location = new System.Drawing.Point(272, 69);
		this.txtAttributeNameText.Multiline = true;
		this.txtAttributeNameText.Name = "txtAttributeNameText";
		this.txtAttributeNameText.Size = new System.Drawing.Size(320, 100);
		this.txtAttributeNameText.TabIndex = 4;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(3, 100);
		this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(68, 15);
		this.label2.TabIndex = 4;
		this.label2.Text = "No of Rows";
		this.txtAproxAcNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAproxAcNo.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAproxAcNo.Location = new System.Drawing.Point(97, 96);
		this.txtAproxAcNo.Name = "txtAproxAcNo";
		this.txtAproxAcNo.Size = new System.Drawing.Size(169, 23);
		this.txtAproxAcNo.TabIndex = 1;
		this.btndownload.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btndownload.BackColor = System.Drawing.Color.Teal;
		this.btndownload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btndownload.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btndownload.ForeColor = System.Drawing.Color.White;
		this.btndownload.Location = new System.Drawing.Point(600, 142);
		this.btndownload.Margin = new System.Windows.Forms.Padding(2);
		this.btndownload.Name = "btndownload";
		this.btndownload.Size = new System.Drawing.Size(90, 27);
		this.btndownload.TabIndex = 5;
		this.btndownload.Text = "Download";
		this.btndownload.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
		this.btndownload.UseVisualStyleBackColor = false;
		this.btndownload.Click += new System.EventHandler(btndownload_Click_1);
		this.btnUpload.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnUpload.BackColor = System.Drawing.Color.Teal;
		this.btnUpload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpload.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpload.ForeColor = System.Drawing.Color.White;
		this.btnUpload.Location = new System.Drawing.Point(694, 142);
		this.btnUpload.Margin = new System.Windows.Forms.Padding(2);
		this.btnUpload.Name = "btnUpload";
		this.btnUpload.Size = new System.Drawing.Size(90, 27);
		this.btnUpload.TabIndex = 6;
		this.btnUpload.Text = "Upload";
		this.btnUpload.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
		this.btnUpload.UseVisualStyleBackColor = false;
		this.btnUpload.Click += new System.EventHandler(btnUpload_Click_1);
		this.CmbAttributeName.AllowNull = (DefaultableBoolean)1;
		this.CmbAttributeName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbAttributeName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbAttributeName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbAttributeName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbAttributeName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbAttributeName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbAttributeName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAttributeName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance18;
		((SpecialBoxBase)((UltraGridBase)this.CmbAttributeName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAttributeName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.CmbAttributeName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbAttributeName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbAttributeName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbAttributeName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbAttributeName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbAttributeName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAttributeName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance23).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbAttributeName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbAttributeName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbAttributeName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance24).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAttributeName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbAttributeName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbAttributeName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbAttributeName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbAttributeName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbAttributeName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbAttributeName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbAttributeName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbAttributeName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbAttributeName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbAttributeName).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbAttributeName).Location = new System.Drawing.Point(97, 69);
		((System.Windows.Forms.Control)(object)this.CmbAttributeName).Name = "CmbAttributeName";
		((System.Windows.Forms.Control)(object)this.CmbAttributeName).Size = new System.Drawing.Size(169, 24);
		((System.Windows.Forms.Control)(object)this.CmbAttributeName).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbAttributeName).Leave += new System.EventHandler(cmbaccounttitle_TextChanged);
		this.label7.AutoSize = true;
		this.label7.BackColor = System.Drawing.Color.Teal;
		this.label7.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.ForeColor = System.Drawing.Color.White;
		this.label7.Location = new System.Drawing.Point(2, 31);
		this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(185, 19);
		this.label7.TabIndex = 4;
		this.label7.Text = "Filter WorkBook Data";
		this.pictureBox2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.pictureBox2.Image = (System.Drawing.Image)resources.GetObject("pictureBox2.Image");
		this.pictureBox2.Location = new System.Drawing.Point(600, 69);
		this.pictureBox2.Margin = new System.Windows.Forms.Padding(2);
		this.pictureBox2.Name = "pictureBox2";
		this.pictureBox2.Size = new System.Drawing.Size(90, 71);
		this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.pictureBox2.TabIndex = 9;
		this.pictureBox2.TabStop = false;
		this.pictureBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.pictureBox1.Image = (System.Drawing.Image)resources.GetObject("pictureBox1.Image");
		this.pictureBox1.Location = new System.Drawing.Point(694, 69);
		this.pictureBox1.Margin = new System.Windows.Forms.Padding(2);
		this.pictureBox1.Name = "pictureBox1";
		this.pictureBox1.Size = new System.Drawing.Size(90, 71);
		this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.pictureBox1.TabIndex = 8;
		this.pictureBox1.TabStop = false;
		this.label3.BackColor = System.Drawing.Color.Teal;
		this.label3.Dock = System.Windows.Forms.DockStyle.Top;
		this.label3.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.White;
		this.label3.Location = new System.Drawing.Point(0, 240);
		this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(794, 24);
		this.label3.TabIndex = 12;
		this.label3.Text = "Sheet Data ";
		this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(3, 74);
		this.label8.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(89, 15);
		this.label8.TabIndex = 4;
		this.label8.Text = "Attribute Name";
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.btnSave });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(794, 27);
		this.toolStrip1.TabIndex = 11;
		this.toolStrip1.Text = "toolStrip1";
		this.btnSave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSave.Image = (System.Drawing.Image)resources.GetObject("btnSave.Image");
		this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(64, 24);
		this.btnSave.Text = "&Save";
		this.btnSave.Click += new System.EventHandler(btnSave_Click);
		this.label1.BackColor = System.Drawing.Color.Teal;
		this.label1.Dock = System.Windows.Forms.DockStyle.Top;
		this.label1.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(0, 27);
		this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(794, 29);
		this.label1.TabIndex = 12;
		this.label1.Text = " ";
		this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(794, 657);
		base.Controls.Add(this.panel1);
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "frmAttributeVarientsExcelSheetUpload";
		this.Text = "Excel Data Reader";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(frmExcelSheetUpload_Load);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdExcelDataView).EndInit();
		this.panel22.ResumeLayout(false);
		this.panel22.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbExcelSubSheetName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAttributeName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBox2).EndInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		base.ResumeLayout(false);
	}
}
