using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Accounts;
using Architecture.BLL.Inventory;
using Architecture.BLL.Reports.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.Reports;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Inventory_Stocks_Report;

public class frmStockReportWithValues : Form
{
	private WaitForm waitForm = new WaitForm();

	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtBindCombo = new DataTable();

	public DateTime FromDate;

	public DateTime ToDate;

	public string BranchesIds = "";

	public string ParentCategoryIds = "";

	private DataTable dtStockSum = new DataTable();

	private DataTable dtMainType = new DataTable();

	private DataTable dtBranch = new DataTable();

	private bool ManualRateAndAmountForItemStockSummary;

	private DataTable dtditinct = new DataTable();

	public bool RequestFromAnotherForm;

	private IContainer components;

	private Panel panel1;

	private Panel panel4;

	private Panel panel3;

	private Panel panel2;

	internal GridEX grdfrm;

	private ToolStrip toolStrip1;

	private ToolStripButton btnRefresh;

	private Label label8;

	internal UltraCombo cmbItemType;

	private Label label7;

	internal UltraCombo cmbMainType;

	private Label label6;

	private Label label3;

	private Label label1;

	internal UltraCombo cmbWareHouse;

	internal UltraCombo cmbCategory;

	private Button btnshow;

	private DateTimePicker txtDateFrom;

	private DateTimePicker txtDateTo;

	private Label label9;

	internal UltraCombo cmbItem;

	private Label label4;

	internal UltraCombo CmbClassGroup;

	private Label label5;

	internal UltraCombo cmbJobLot;

	private Label label10;

	internal UltraCombo cmbCropYear;

	private Label label11;

	internal UltraCombo cmbReportType;

	private CtrlGrdBar ctrlGrdBar1;

	private CheckBox ChkItemWiseStock;

	private Panel panel5;

	private Label label23;

	private Label lblStockAcItem;

	internal UltraCombo cmbStockAccounts;

	private Label label13;

	internal UltraCombo cmbPacktype;

	private Label label12;

	internal UltraCombo cmbPackUom;

	private ToolStripButton btnPrint;

	private CheckBox chkIncludeWipItem;

	private ToolStripButton btnPrintStockSummeryByWeight;

	private ToolStripButton btnPrintStockReport;

	internal UltraCombo cmbBranchName;

	private Label label14;

	private RadioButton RdCGSAmount;

	private RadioButton RdSaleAmount;

	private ToolStripButton BtnUpdate;

	private ToolStripButton BtnReset;

	private Label label2;

	internal UltraCombo cmbStockGroupAccount;

	private RadioButton rdAvgRatePerKg;

	private RadioButton rdAvgRate40Kg;

	public CheckBox chkSkipZero;

	private GroupBox groupBox1;

	private BackgroundWorker backgroundWorker1;

	private RadioButton rdAvgRateFourDecimals;

	private RadioButton rdAvgRateThreeDecimals;

	private RadioButton rdAvgRateTwoDecimals;

	public frmStockReportWithValues(UserAccount userAccount)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = userAccount;
	}

	private void frmStockReport_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(980, 600);
			ctrlGrdBar1.mGridSaveLayouts.Visible = false;
			ctrlGrdBar1.mGridChooseFielder.Visible = false;
			ctrlGrdBar1.removeLayoutToolStripMenuItem.Visible = false;
			ctrlGrdBar1.GroupExpand.Visible = false;
			ctrlGrdBar1.GroupCollaps.Visible = false;
			dtMainType.Columns.Add("Id", typeof(int));
			dtMainType.Columns.Add("name");
			BtnUpdate.Visible = false;
			BtnUpdate.Enabled = false;
			ManualRateAndAmountForItemStockSummary = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("ManualRateAndAmountForItemStockSummary"));
			dtditinct.Columns.Add("AccountId", typeof(int));
			dtditinct.Columns.Add("AccountTitle", typeof(string));
			dtditinct.Columns.Add("ItemId", typeof(int));
			dtditinct.Columns.Add("ItemName", typeof(string));
			dtditinct.Columns.Add("PackUomId", typeof(int));
			dtditinct.Columns.Add("PackUom", typeof(string));
			dtditinct.Columns.Add("PackingTypeId", typeof(int));
			dtditinct.Columns.Add("PackingType", typeof(string));
			dtditinct.Columns.Add("WareHouseName", typeof(string));
			dtditinct.Columns.Add("CropYear", typeof(string));
			dtditinct.Columns.Add("JobLot", typeof(string));
			dtditinct.Columns.Add("OpQty", typeof(double));
			dtditinct.Columns.Add("QtyIn", typeof(double));
			dtditinct.Columns.Add("QtyOut", typeof(double));
			dtditinct.Columns.Add("BalQty", typeof(double));
			dtditinct.Columns.Add("OpWeight", typeof(double));
			dtditinct.Columns.Add("WeightIn", typeof(double));
			dtditinct.Columns.Add("WeightOut", typeof(double));
			dtditinct.Columns.Add("BalWeight", typeof(double));
			dtditinct.Columns.Add("OpAmount", typeof(double));
			dtditinct.Columns.Add("AmountIn", typeof(double));
			dtditinct.Columns.Add("AmountOut", typeof(double));
			dtditinct.Columns.Add("BalAmount", typeof(double));
			dtditinct.Columns.Add("AvgRate", typeof(double));
			dtditinct.Columns.Add("M_Rate/40Kg", typeof(double));
			dtditinct.Columns.Add("M_Amount", typeof(double));
			dtditinct.Columns.Add("Saved_M_Rate", typeof(double));
			txtDateFrom.Focus();
			if (!Conversion.CheckDateTimeNull(FromDate) && !Conversion.CheckDateTimeNull(ToDate))
			{
				txtDateFrom.Value = FromDate;
				txtDateTo.Value = ToDate;
			}
			else
			{
				txtDateFrom.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			}
			DateTime AsOnDate = StocksReport.GetInventoryStockAsOnDate(UserAccount.OrganizationId, UserAccount.CompanyId);
			if (!Conversion.CheckDateTimeNull(AsOnDate))
			{
				txtDateFrom.Value = AsOnDate;
			}
			BranchesFill();
			ParentCategoryComboFill();
			OtherComboByParentCategoryFill();
			UOMFill();
			ReportTypeFill();
			AccountFill3rdLevel();
			((UltraGridBase)cmbReportType).Rows[0].Activate();
			_ = ((Control)(object)cmbReportType).Text == Conversion.ToString((object)"Item Stock Summary");
			if (RequestFromAnotherForm)
			{
				if (BranchesIds != null && BranchesIds != string.Empty)
				{
					((Control)(object)cmbBranchName).Text = string.Empty;
					string[] valuesToMatch2 = BranchesIds.Split(',');
					RowEnumerator enumerator = ((UltraGridBase)cmbBranchName).Rows.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							UltraGridRow row2 = enumerator.Current;
							string[] array = valuesToMatch2;
							for (int i = 0; i < array.Length; i++)
							{
								if (Conversion.ToInt((object)array[i]) == Conversion.ToInt(row2.Cells["BranchId"].Value))
								{
									row2.Cells["Selected"].Value = true;
									break;
								}
							}
						}
					}
					finally
					{
						if (enumerator is IDisposable disposable)
						{
							disposable.Dispose();
						}
					}
				}
				else if (BranchesIds == null || BranchesIds == string.Empty)
				{
					RowEnumerator enumerator = ((UltraGridBase)cmbBranchName).Rows.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							enumerator.Current.Cells["Selected"].Value = true;
						}
					}
					finally
					{
						if (enumerator is IDisposable disposable2)
						{
							disposable2.Dispose();
						}
					}
				}
				if (ParentCategoryIds != null && ParentCategoryIds != string.Empty)
				{
					((Control)(object)cmbMainType).Text = string.Empty;
					string[] valuesToMatch = ParentCategoryIds.Split(',');
					RowEnumerator enumerator = ((UltraGridBase)cmbMainType).Rows.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							UltraGridRow row = enumerator.Current;
							string[] array = valuesToMatch;
							for (int i = 0; i < array.Length; i++)
							{
								if (Conversion.ToInt((object)array[i]) == Conversion.ToInt(row.Cells["Id"].Value))
								{
									row.Cells["Selected"].Value = true;
									break;
								}
							}
						}
					}
					finally
					{
						if (enumerator is IDisposable disposable3)
						{
							disposable3.Dispose();
						}
					}
				}
			}
			PrintButtonManage();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BranchesFill()
	{
		try
		{
			dtBranch = BranchesAllocationToUser.GetBranchsAllocatedToUser(UserAccount.OrganizationId, UserAccount.CompanyId, UserAccount.ID);
			if (dtBranch.Rows.Count > 0)
			{
				DDL.BindDDL(dtBranch, cmbBranchName, "BranchId", "BranchName", "Branch Name", ZeroIndex: false);
				UltraGridColumn obj = ((UltraGridBase)cmbBranchName).DisplayLayout.Bands[0].Columns.Add();
				((KeyedSubObjectBase)obj).Key = "Selected";
				((HeaderBase)obj.Header).Caption = string.Empty;
				obj.Header.CheckBoxVisibility = (HeaderCheckBoxVisibility)2;
				obj.DataType = typeof(bool);
				((HeaderBase)obj.Header).VisiblePosition = 0;
				cmbBranchName.CheckedListSettings.CheckStateMember = "Selected";
				((EditorCheckedListSettings)cmbBranchName.CheckedListSettings).EditorValueSource = (EditorWithComboValueSource)1;
				((EditorCheckedListSettings)cmbBranchName.CheckedListSettings).ListSeparator = ",";
				((EditorCheckedListSettings)cmbBranchName.CheckedListSettings).ItemCheckArea = (ItemCheckArea)1;
				((UltraGridBase)cmbBranchName).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				((Control)(object)cmbBranchName).Text = UserAccount.BranchName.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UOMFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("PackUom");
			dt.Rows.Add(1, "1KG");
			dt.Rows.Add(5, "5KG");
			dt.Rows.Add(10, "10KG");
			dt.Rows.Add(15, "15KG");
			dt.Rows.Add(20, "20KG");
			dt.Rows.Add(25, "25KG");
			dt.Rows.Add(40, "40KG");
			dt.Rows.Add(50, "50KG");
			dt.Rows.Add(60, "60KG");
			dt.Rows.Add(65, "65KG");
			dt.Rows.Add(80, "80KG");
			dt.Rows.Add(100, "100KG");
			DDL.BindDDL(dt, cmbPackUom, "Id", "PackUom", "PackUom", ZeroIndex: false);
			((UltraGridBase)cmbPackUom).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ReportTypeFill()
	{
		try
		{
			DataTable dtreporttype = new DataTable();
			dtreporttype.Columns.Add("Id");
			dtreporttype.Columns.Add("name");
			dtreporttype.Rows.Add("ItemStockSummary", "Item Stock Summary");
			dtreporttype.Rows.Add("ItemandWarehouseStockSummary", "Warehouse and Item Stock Summary");
			dtreporttype.Rows.Add("ItemandWarehouseStockSummary", "Item and WareHouse Stock Summary");
			dtreporttype.Rows.Add("ItemandCropYearStockSummary", "Crop Year and Item Stock Summary");
			dtreporttype.Rows.Add("ItemandCropYearandWarehouseStockSummary", "Crop Year, Warehouse and Item Stock Summary");
			dtreporttype.Rows.Add("JobLotandItemStockSummary", "JobLot and Item");
			dtreporttype.Rows.Add("WarehouseandJoblotandItemStockSummary", "Warehouse, Joblot and Item Stock Summary");
			dtreporttype.Rows.Add("WarehouseandJoblotandItemStockSummary", "Warehouse, Item and Joblot Stock Summary");
			dtreporttype.Rows.Add("ItemandPackSizeStockSummary", "Item and PackSize Stock Summary");
			dtreporttype.Rows.Add("ItemandPackingTypeStockSummary", "Item and PackingType Stock Summary");
			dtreporttype.Rows.Add("ItemandPackSizeandPackingTypeStockSummary", "Item and PackSize and PackingType Stock Summary");
			dtreporttype.Rows.Add("ItemWarehouseJobCropPackingTypePackSizeStockSummary", "All Filters Stock Summary");
			DDL.BindDDLNew(dtreporttype, cmbReportType, "Id", "name", "Report Type", false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbBranchName_Leave(object sender, EventArgs e)
	{
	}

	private void cmbMainType_Leave(object sender, EventArgs e)
	{
		try
		{
			try
			{
				if (Conversion.ToString((object)((Control)(object)cmbMainType).Text) != string.Empty)
				{
					OtherComboByParentCategoryFill();
					return;
				}
				((Control)(object)cmbCategory).Text = string.Empty;
				((UltraGridBase)cmbCategory).DataSource = null;
				((Control)(object)cmbItemType).Text = string.Empty;
				((UltraGridBase)cmbItemType).DataSource = null;
				((Control)(object)cmbItem).Text = string.Empty;
				((UltraGridBase)cmbItem).DataSource = null;
				((Control)(object)cmbWareHouse).Text = string.Empty;
				((UltraGridBase)cmbWareHouse).DataSource = null;
				((Control)(object)cmbStockAccounts).Text = string.Empty;
				((UltraGridBase)cmbStockAccounts).DataSource = null;
				((Control)(object)cmbCropYear).Text = string.Empty;
				((UltraGridBase)cmbCropYear).DataSource = null;
				((Control)(object)CmbClassGroup).Text = string.Empty;
				((UltraGridBase)CmbClassGroup).DataSource = null;
				((Control)(object)cmbPacktype).Text = string.Empty;
				((UltraGridBase)cmbPacktype).DataSource = null;
				((Control)(object)cmbJobLot).Text = string.Empty;
				((UltraGridBase)cmbJobLot).DataSource = null;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}
		catch (Exception ex2)
		{
			MessageBox.Show(ex2.Message);
		}
	}

	private void OtherComboByParentCategoryFill()
	{
		try
		{
			DataTable dtFiltered = new DataTable();
			HashSet<string> validIds = new HashSet<string>();
			if (!string.IsNullOrWhiteSpace(((Control)(object)cmbMainType).Text))
			{
				string[] array = ((Control)(object)cmbMainType).Text.Trim().Split(',');
				Dictionary<string, string> nameToIdMap = dtMainType.AsEnumerable().ToDictionary((DataRow row) => row["name"].ToString().Trim(), (DataRow row) => row["Id"].ToString());
				string[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					string trimmedAuthor = array2[i].Trim();
					if (nameToIdMap.ContainsKey(trimmedAuthor))
					{
						validIds.Add(nameToIdMap[trimmedAuthor]);
					}
				}
			}
			if (validIds.Count > 0)
			{
				List<DataRow> filteredRows = (from row in dtBindCombo.AsEnumerable()
					where validIds.Contains(row["InventoryParentCategoriesId"].ToString())
					select row).ToList();
				if (filteredRows.Any())
				{
					dtFiltered = filteredRows.CopyToDataTable();
				}
			}
			if (dtFiltered.Rows.Count <= 0)
			{
				return;
			}
			Dictionary<string, DataTable> activityTables = new Dictionary<string, DataTable>
			{
				{
					"ItemTypes",
					new DataTable
					{
						Columns = { "Id", "name" }
					}
				},
				{
					"ItemCategories",
					new DataTable
					{
						Columns = { "Id", "name" }
					}
				},
				{
					"Items",
					new DataTable
					{
						Columns = { "Id", "name" }
					}
				},
				{
					"Warehouse",
					new DataTable
					{
						Columns = { "Id", "name" }
					}
				},
				{
					"Stock_Account",
					new DataTable
					{
						Columns = { "Id", "name" }
					}
				},
				{
					"ItemClassGroup",
					new DataTable
					{
						Columns = { "Id", "name" }
					}
				},
				{
					"JobLot",
					new DataTable
					{
						Columns = { "Id", "name" }
					}
				},
				{
					"CropYear",
					new DataTable
					{
						Columns = { "Id", "name" }
					}
				},
				{
					"PackingType",
					new DataTable
					{
						Columns = { "Id", "name" }
					}
				}
			};
			Dictionary<string, HashSet<int>> addedIds = new Dictionary<string, HashSet<int>>();
			foreach (string activity in activityTables.Keys)
			{
				addedIds[activity] = new HashSet<int>();
			}
			foreach (DataRow row2 in dtFiltered.Rows)
			{
				string activityType = Conversion.ToString(row2["ActivityType"]);
				int id = Conversion.ToInt(row2["Id"]);
				if (activityTables.ContainsKey(activityType) && !addedIds[activityType].Contains(id))
				{
					activityTables[activityType].Rows.Add(id, row2["name"]);
					addedIds[activityType].Add(id);
				}
			}
			DDL.BindDDL(activityTables["ItemTypes"], cmbItemType, "Id", "name", "Item Type", ZeroIndex: false);
			DDL.BindDDL(activityTables["ItemCategories"], cmbCategory, "Id", "name", "Item Categories", ZeroIndex: false);
			DDL.BindDDL(activityTables["Items"], cmbItem, "Id", "name", "Item Name", ZeroIndex: false);
			DDL.BindDDL(activityTables["Warehouse"], cmbWareHouse, "Id", "name", "WarehouseName", ZeroIndex: false);
			DDL.BindDDL(activityTables["Stock_Account"], cmbStockAccounts, "Id", "name", "Stock_Account", ZeroIndex: false);
			DDL.BindDDL(activityTables["ItemClassGroup"], CmbClassGroup, "Id", "name", "ItemClassGroup", ZeroIndex: false);
			DDL.BindDDL(activityTables["JobLot"], cmbJobLot, "Id", "name", "JobLot", ZeroIndex: false);
			DDL.BindDDL(activityTables["CropYear"], cmbCropYear, "Id", "name", "CropYear", ZeroIndex: false);
			DDL.BindDDL(activityTables["PackingType"], cmbPacktype, "Id", "name", "PackingType", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ParentCategoryComboFill()
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		try
		{
			dtMainType.Clear();
			((UltraGridBase)cmbMainType).DataSource = null;
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.Ids = "1,2,3,4,6";
			dtBindCombo = StocksReport.Evalaution_DropDown_ByParentCategories(obj);
			if (dtBindCombo.Rows.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < dtBindCombo.Rows.Count; i++)
			{
				if (Conversion.ToString(dtBindCombo.Rows[i]["ActivityType"]) == "ParentCategories")
				{
					dtMainType.Rows.Add(dtBindCombo.Rows[i]["Id"], dtBindCombo.Rows[i]["name"]);
				}
			}
			DDL.BindDDL(dtMainType, cmbMainType, "Id", "name", "Parent Categories", ZeroIndex: false);
			((UltraGridBase)cmbMainType).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			UltraGridColumn obj2 = ((UltraGridBase)cmbMainType).DisplayLayout.Bands[0].Columns.Add();
			((KeyedSubObjectBase)obj2).Key = "Selected";
			((HeaderBase)obj2.Header).Caption = string.Empty;
			obj2.Header.CheckBoxVisibility = (HeaderCheckBoxVisibility)2;
			obj2.DataType = typeof(bool);
			((HeaderBase)obj2.Header).VisiblePosition = 0;
			cmbMainType.CheckedListSettings.CheckStateMember = "Selected";
			((EditorCheckedListSettings)cmbMainType.CheckedListSettings).EditorValueSource = (EditorWithComboValueSource)1;
			((EditorCheckedListSettings)cmbMainType.CheckedListSettings).ListSeparator = ",";
			((EditorCheckedListSettings)cmbMainType.CheckedListSettings).ItemCheckArea = (ItemCheckArea)1;
			RowEnumerator enumerator = ((UltraGridBase)cmbMainType).Rows.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					enumerator.Current.Cells["Selected"].Value = true;
				}
			}
			finally
			{
				if (enumerator is IDisposable disposable)
				{
					disposable.Dispose();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AccountFill3rdLevel()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Expected O, but got Unknown
		try
		{
			DataTable dtAccount = ChartofAccount.ReadAllAccountgroup(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				Account_Level = 3,
				AccountTypeId = 4,
				AccouuntClassId = 2
			});
			if (dtAccount.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("AccountTitle");
				for (int i = 0; i < dtAccount.Rows.Count; i++)
				{
					dt.Rows.Add(dtAccount.Rows[i]["Id"], dtAccount.Rows[i]["AccountTitle"]);
				}
				DDL.BindDDL(dt, cmbStockGroupAccount, "Id", "AccountTitle", "Account Title", ZeroIndex: false);
				((UltraGridBase)cmbStockGroupAccount).DisplayLayout.Bands[0].Columns[1].Width = 336;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnUpdate_Click(object sender, EventArgs e)
	{
		try
		{
			InsertInCaseOfItemSummary();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void InsertInCaseOfItemSummary()
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Expected O, but got Unknown
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Expected O, but got Unknown
		try
		{
			if (grdfrm.RowCount == 0)
			{
				MessageBox.Show("Grid Record Not Found");
			}
			else
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				StockSummaryByItem po = new StockSummaryByItem();
				po.OrganizationId = UserAccount.OrganizationId;
				po.CompanyId = UserAccount.CompanyId;
				po.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
				po.EntryUserId = UserAccount.ID;
				po.ModifyUserId = UserAccount.ID;
				po.EntryDate = DateTime.Now;
				po.ModifyDate = DateTime.Now;
				po.StockSummaryByItemsList = new List<StockSummaryByItem>();
				GridEXRow[] rows = grdfrm.GetRows();
				foreach (GridEXRow r in rows)
				{
					StockSummaryByItem vd = new StockSummaryByItem();
					if (Conversion.ToDouble(r.Cells["M_Amount"].Value) > 0.0 && Conversion.ToInt(r.Cells["M_Rate/40Kg"].Value) > 0 && Conversion.ToInt(r.Cells["M_Rate/40Kg"].Value) != Conversion.ToInt(r.Cells["Saved_M_Rate"].Value))
					{
						vd.OrganizationId = UserAccount.OrganizationId;
						vd.CompanyId = UserAccount.CompanyId;
						vd.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
						vd.EntryUserId = UserAccount.ID;
						vd.ModifyUserId = UserAccount.ID;
						vd.EntryDate = DateTime.Now;
						vd.ModifyDate = DateTime.Now;
						vd.DocDate = Conversion.ToDateTime((object)txtDateTo.Value);
						vd.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
						vd.PackingTypeId = Conversion.ToInt(r.Cells["PackingTypeId"].Value);
						vd.ItemUOMId = Conversion.ToInt(r.Cells["PackUomId"].Value);
						vd.ItemQty = Conversion.ToDouble(r.Cells["BalQty"].Value);
						vd.NetWeight = Conversion.ToDouble(r.Cells["BalWeight"].Value);
						vd.RateUOM = 40.0;
						vd.ItemRate = Conversion.ToDouble(r.Cells["M_Rate/40Kg"].Value);
						vd.Amount = Conversion.ToDouble(r.Cells["M_Amount"].Value);
						po.StockSummaryByItemsList.Add(vd);
					}
				}
				if (po.StockSummaryByItemsList.Count == 0)
				{
					throw new Exception("No Record Found For Update...");
				}
				StockSummaryByItem.Save(po);
				MessageBox.Show("Data Updated Successfully.... ");
				GridBind();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnshow_Click(object sender, EventArgs e)
	{
		try
		{
			GridBind();
			PrintButtonManage();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void GridBind()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Expected O, but got Unknown
		try
		{
			BtnUpdate.Visible = false;
			BtnUpdate.Enabled = false;
			ReportsParameters obj = new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FromDate = Conversion.ToDateTime((object)txtDateFrom.Value),
				ToDate = Conversion.ToDateTime((object)txtDateTo.Value),
				Activity = Conversion.ToString(cmbReportType.Value),
				ItemTypeId = Conversion.ToInt(cmbItemType.Value),
				ItemCategoryId = Conversion.ToInt(cmbCategory.Value),
				ItemId = Conversion.ToInt(cmbItem.Value),
				PackUomFrom = Conversion.ToDouble(cmbPackUom.Value),
				PackingTypeId = Conversion.ToInt(cmbPacktype.Value),
				WarehouseId = Conversion.ToInt(cmbWareHouse.Value),
				JobLotId = Conversion.ToInt(cmbJobLot.Value),
				PurchaseGLAC = Conversion.ToInt(cmbStockAccounts.Value),
				GroupAccountId = Conversion.ToInt(cmbStockGroupAccount.Value),
				ItemClassGroupId = Conversion.ToInt(CmbClassGroup.Value),
				CropYear = Conversion.ToString((object)((Control)(object)cmbCropYear).Text),
				ActionId = (RdSaleAmount.Checked ? 1 : 0),
				stockUOM = ((!ChkItemWiseStock.Checked) ? 1 : 0),
				RateUOM = ((!ChkItemWiseStock.Checked) ? 1 : 0),
				ZeroBalanceType = (chkSkipZero.Checked ? 1 : 0),
				AllowWipItem = (chkIncludeWipItem.Checked ? 1 : 0)
			};
			if (!string.IsNullOrWhiteSpace(((Control)(object)cmbMainType).Text))
			{
				IEnumerable<string> ids = from x in ((Control)(object)cmbMainType).Text.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries)
					select x.Trim() into x
					select dtMainType.Select("name='" + x + "'") into x
					where x.Length != 0
					select x[0][0].ToString();
				obj.Ids = string.Join(",", ids);
			}
			if (!string.IsNullOrWhiteSpace(((Control)(object)cmbBranchName).Text))
			{
				IEnumerable<string> branchIds = from x in ((Control)(object)cmbBranchName).Text.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries)
					select x.Trim() into x
					select dtBranch.Select("BranchName='" + x + "'") into x
					where x.Length != 0
					select x[0][0].ToString();
				obj.BranchesIds = string.Join(",", branchIds);
				dtStockSum = StocksReport.stockReportWithValues(obj);
				if (dtStockSum.Rows.Count <= 0)
				{
					grdfrm.DataSource = null;
					return;
				}
				if (rdAvgRatePerKg.Checked)
				{
					foreach (DataRow row2 in dtStockSum.Rows)
					{
						row2["AvgRate"] = Conversion.ToDouble(row2["AvgRate"]) / 40.0;
					}
				}
				btnPrintStockSummeryByWeight.Visible = Conversion.ToString(cmbReportType.Value) == "ItemStockSummary";
				btnPrintStockReport.Visible = Conversion.ToString(cmbReportType.Value) == "ItemStockSummary";
				dtditinct.Clear();
				dtditinct.BeginLoadData();
				foreach (DataRow row in dtStockSum.Rows)
				{
					string reportType = Conversion.ToString(cmbReportType.Value);
					string warehouse = "";
					string cropYear = "";
					string jobLot = "";
					switch (reportType)
					{
					case "ItemandWarehouseStockSummary":
					case "ItemandCropYearandWarehouseStockSummary":
					case "WarehouseandJoblotandItemStockSummary":
						warehouse = Conversion.ToString(row["WareHouseName"]);
						break;
					}
					if (reportType == "ItemandCropYearStockSummary" || reportType == "ItemandCropYearandWarehouseStockSummary")
					{
						cropYear = Conversion.ToString(row["CropYear"]);
					}
					if (reportType == "JobLotandItemStockSummary" || reportType == "WarehouseandJoblotandItemStockSummary")
					{
						jobLot = Conversion.ToString(row["JobLot"]);
					}
					if (reportType == "ItemWarehouseJobCropPackingTypePackSizeStockSummary")
					{
						warehouse = Conversion.ToString(row["WareHouseName"]);
						cropYear = Conversion.ToString(row["CropYear"]);
						jobLot = Conversion.ToString(row["JobLot"]);
					}
					bool isItemStockSummary = reportType == "ItemStockSummary";
					double manualRate = (isItemStockSummary ? Conversion.ToDouble(row["ManualRate"]) : 0.0);
					double manualAmount = (isItemStockSummary ? (Conversion.ToDouble(row["BalWeight"]) / 40.0 * manualRate) : 0.0);
					dtditinct.Rows.Add(row["AccountId"], row["AccountTitle"], row["ItemId"], row["ItemName"], isItemStockSummary ? row["PackUomId"] : ((object)0), (reportType == "ItemandPackingTypeStockSummary") ? "" : row["PackUom"], isItemStockSummary ? row["PackingTypeId"] : ((object)0), (reportType == "ItemandPackSizeStockSummary") ? "" : row["PackingType"], warehouse, cropYear, jobLot, Conversion.ToDouble(row["OpQty"]), Conversion.ToDouble(row["QtyIn"]), Conversion.ToDouble(row["QtyOut"]), Conversion.ToDouble(row["BalQty"]), Conversion.ToDouble(row["OpWeight"]), Conversion.ToDouble(row["WeightIn"]), Conversion.ToDouble(row["WeightOut"]), Conversion.ToDouble(row["BalWeight"]), Conversion.ToDouble(row["OpAmount"]), Conversion.ToDouble(row["AmountIn"]), Conversion.ToDouble(row["AmountOut"]), Conversion.ToDouble(row["BalAmount"]), Conversion.ToDouble(row["AvgRate"]), manualRate, manualAmount, manualRate);
				}
				dtditinct.EndLoadData();
				grdfrm.DataSource = dtditinct;
				GridSettings();
				return;
			}
			cmbBranchName.Focus();
			throw new Exception("Select Branch First");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSettings()
	{
		try
		{
			grdfrm.RootTable.Columns["ItemId"].Visible = false;
			grdfrm.RootTable.Columns["AccountId"].Visible = false;
			grdfrm.RootTable.Columns["PackUomId"].Visible = false;
			grdfrm.RootTable.Columns["PackingTypeId"].Visible = false;
			grdfrm.RootTable.Columns["Saved_M_Rate"].Visible = false;
			grdfrm.RootTable.Columns["AvgRate"].Caption = (rdAvgRatePerKg.Checked ? "AvgRate/1Kg" : "AvgRate/40Kg");
			grdfrm.RootTable.Columns["ItemName"].ColumnType = (ColumnType)5;
			grdfrm.RootTable.Columns["AccountTitle"].ColumnType = (ColumnType)5;
			grdfrm.RootTable.ColumnSets["Pack"].Visible = false;
			grdfrm.RootTable.ColumnSets["PackingType"].Visible = false;
			grdfrm.RootTable.ColumnSets["WareHouse"].Visible = false;
			grdfrm.RootTable.ColumnSets["Crop"].Visible = false;
			grdfrm.RootTable.ColumnSets["Job"].Visible = false;
			grdfrm.RootTable.ColumnSets["Custom"].Visible = false;
			grdfrm.RootTable.ColumnSets["Item"].Visible = true;
			grdfrm.RootTable.Columns["ItemName"].Visible = true;
			grdfrm.RootTable.Columns["AccountTitle"].Visible = true;
			grdfrm.RootTable.Columns["PackUom"].Visible = true;
			grdfrm.RootTable.Columns["PackingType"].Visible = true;
			grdfrm.RootTable.Columns["WareHouseName"].Visible = true;
			grdfrm.RootTable.Columns["CropYear"].Visible = true;
			grdfrm.RootTable.Columns["JobLot"].Visible = true;
			grdfrm.AllowEdit = (InheritableBoolean)2;
			((JanusCollectionBase)grdfrm.RootTable.Groups).Clear();
			if (Conversion.ToString(cmbReportType.Value) == "ItemStockSummary")
			{
				grdfrm.AllowEdit = (InheritableBoolean)1;
				grdfrm.RootTable.Columns["ItemId"].EditType = (EditType)0;
				grdfrm.RootTable.Columns["AccountId"].EditType = (EditType)0;
				grdfrm.RootTable.Columns["OpWeight"].EditType = (EditType)0;
				grdfrm.RootTable.Columns["WeightIn"].EditType = (EditType)0;
				grdfrm.RootTable.Columns["WeightOut"].EditType = (EditType)0;
				grdfrm.RootTable.Columns["BalWeight"].EditType = (EditType)0;
				grdfrm.RootTable.Columns["OpQty"].EditType = (EditType)0;
				grdfrm.RootTable.Columns["QtyIn"].EditType = (EditType)0;
				grdfrm.RootTable.Columns["QtyOut"].EditType = (EditType)0;
				grdfrm.RootTable.Columns["BalQty"].EditType = (EditType)0;
				grdfrm.RootTable.Columns["OpAmount"].EditType = (EditType)0;
				grdfrm.RootTable.Columns["AmountIn"].EditType = (EditType)0;
				grdfrm.RootTable.Columns["AmountOut"].EditType = (EditType)0;
				grdfrm.RootTable.Columns["AvgRate"].EditType = (EditType)0;
				grdfrm.RootTable.Columns["BalAmount"].EditType = (EditType)0;
				grdfrm.RootTable.Columns["ItemName"].EditType = (EditType)0;
				grdfrm.RootTable.Columns["PackUomId"].EditType = (EditType)0;
				grdfrm.RootTable.Columns["PackingTypeId"].EditType = (EditType)0;
				grdfrm.RootTable.Columns["PackUom"].EditType = (EditType)0;
				grdfrm.RootTable.Columns["PackingType"].EditType = (EditType)0;
				grdfrm.RootTable.Columns["Saved_M_Rate"].EditType = (EditType)0;
				grdfrm.RootTable.Columns["M_Amount"].EditType = (EditType)0;
				grdfrm.RootTable.Columns["AccountTitle"].EditType = (EditType)0;
				grdfrm.RootTable.Columns["ItemId"].FilterEditType = (FilterEditType)1;
				grdfrm.RootTable.Columns["AccountId"].FilterEditType = (FilterEditType)1;
				grdfrm.RootTable.Columns["OpWeight"].FilterEditType = (FilterEditType)1;
				grdfrm.RootTable.Columns["WeightIn"].FilterEditType = (FilterEditType)1;
				grdfrm.RootTable.Columns["WeightOut"].FilterEditType = (FilterEditType)1;
				grdfrm.RootTable.Columns["BalWeight"].FilterEditType = (FilterEditType)1;
				grdfrm.RootTable.Columns["OpQty"].FilterEditType = (FilterEditType)1;
				grdfrm.RootTable.Columns["QtyIn"].FilterEditType = (FilterEditType)1;
				grdfrm.RootTable.Columns["QtyOut"].FilterEditType = (FilterEditType)1;
				grdfrm.RootTable.Columns["BalQty"].FilterEditType = (FilterEditType)1;
				grdfrm.RootTable.Columns["OpAmount"].FilterEditType = (FilterEditType)1;
				grdfrm.RootTable.Columns["AmountIn"].FilterEditType = (FilterEditType)1;
				grdfrm.RootTable.Columns["AmountOut"].FilterEditType = (FilterEditType)1;
				grdfrm.RootTable.Columns["AvgRate"].FilterEditType = (FilterEditType)1;
				grdfrm.RootTable.Columns["BalAmount"].FilterEditType = (FilterEditType)1;
				grdfrm.RootTable.Columns["ItemName"].FilterEditType = (FilterEditType)1;
				grdfrm.RootTable.Columns["PackUomId"].FilterEditType = (FilterEditType)1;
				grdfrm.RootTable.Columns["PackingTypeId"].FilterEditType = (FilterEditType)1;
				grdfrm.RootTable.Columns["PackUom"].FilterEditType = (FilterEditType)1;
				grdfrm.RootTable.Columns["PackingType"].FilterEditType = (FilterEditType)1;
				grdfrm.RootTable.Columns["Saved_M_Rate"].FilterEditType = (FilterEditType)1;
				grdfrm.RootTable.Columns["M_Amount"].FilterEditType = (FilterEditType)1;
				grdfrm.RootTable.Columns["AccountTitle"].FilterEditType = (FilterEditType)1;
				BtnUpdate.Visible = ManualRateAndAmountForItemStockSummary && !ChkItemWiseStock.Checked;
				BtnUpdate.Enabled = ManualRateAndAmountForItemStockSummary && !ChkItemWiseStock.Checked;
				grdfrm.RootTable.Columns["M_Rate/40Kg"].Visible = ManualRateAndAmountForItemStockSummary && !ChkItemWiseStock.Checked;
				grdfrm.RootTable.Columns["M_Amount"].Visible = ManualRateAndAmountForItemStockSummary && !ChkItemWiseStock.Checked;
				grdfrm.RootTable.Columns["M_Rate/40Kg"].EditType = (EditType)((ManualRateAndAmountForItemStockSummary && !ChkItemWiseStock.Checked) ? 1 : 0);
				grdfrm.RootTable.ColumnSets["Custom"].Visible = ManualRateAndAmountForItemStockSummary && !ChkItemWiseStock.Checked;
				grdfrm.RootTable.ColumnSets["Pack"].Visible = true;
				grdfrm.RootTable.ColumnSets["PackingType"].Visible = true;
			}
			else if (Conversion.ToString((object)((Control)(object)cmbReportType).Text) == "Item and PackSize Stock Summary")
			{
				grdfrm.RootTable.ColumnSets["Pack"].Visible = true;
			}
			else if (Conversion.ToString((object)((Control)(object)cmbReportType).Text) == "Item and PackingType Stock Summary")
			{
				grdfrm.RootTable.ColumnSets["PackingType"].Visible = true;
			}
			else if (Conversion.ToString((object)((Control)(object)cmbReportType).Text) == "Item and PackSize and PackingType Stock Summary")
			{
				grdfrm.RootTable.ColumnSets["Pack"].Visible = true;
				grdfrm.RootTable.ColumnSets["PackingType"].Visible = true;
			}
			else if (Conversion.ToString((object)((Control)(object)cmbReportType).Text) == "Warehouse and Item Stock Summary")
			{
				grdfrm.RootTable.ColumnSets["Pack"].Visible = true;
				grdfrm.RootTable.ColumnSets["PackingType"].Visible = true;
				grdfrm.RootTable.Groups.Add("WareHouseName");
				grdfrm.RootTable.Columns["WareHouseName"].Visible = false;
				grdfrm.RootTable.GroupTotals = (GroupTotals)2;
			}
			else if (Conversion.ToString((object)((Control)(object)cmbReportType).Text) == "Item and WareHouse Stock Summary")
			{
				grdfrm.RootTable.ColumnSets["Pack"].Visible = true;
				grdfrm.RootTable.ColumnSets["PackingType"].Visible = true;
				grdfrm.RootTable.ColumnSets["WareHouse"].Visible = true;
				grdfrm.RootTable.ColumnSets["Item"].Visible = false;
				grdfrm.RootTable.Groups.Add("ItemName");
				grdfrm.RootTable.Columns["ItemName"].Visible = false;
				grdfrm.RootTable.GroupTotals = (GroupTotals)2;
			}
			else if (Conversion.ToString((object)((Control)(object)cmbReportType).Text) == "Crop Year and Item Stock Summary")
			{
				grdfrm.RootTable.ColumnSets["Pack"].Visible = true;
				grdfrm.RootTable.ColumnSets["PackingType"].Visible = true;
				grdfrm.RootTable.Groups.Add("CropYear");
				grdfrm.RootTable.Columns["CropYear"].Visible = false;
				grdfrm.RootTable.GroupTotals = (GroupTotals)2;
			}
			else if (Conversion.ToString((object)((Control)(object)cmbReportType).Text) == "Crop Year, Warehouse and Item Stock Summary")
			{
				grdfrm.RootTable.ColumnSets["Pack"].Visible = true;
				grdfrm.RootTable.ColumnSets["PackingType"].Visible = true;
				grdfrm.RootTable.Groups.Add("CropYear");
				grdfrm.RootTable.Columns["CropYear"].Visible = false;
				grdfrm.RootTable.Groups.Add("WareHouseName");
				grdfrm.RootTable.Columns["WareHouseName"].Visible = false;
				grdfrm.RootTable.GroupTotals = (GroupTotals)2;
			}
			else if (Conversion.ToString((object)((Control)(object)cmbReportType).Text) == "Warehouse, Item and Joblot Stock Summary")
			{
				grdfrm.RootTable.ColumnSets["Item"].Visible = false;
				grdfrm.RootTable.ColumnSets["WareHouse"].Visible = false;
				grdfrm.RootTable.ColumnSets["Pack"].Visible = true;
				grdfrm.RootTable.ColumnSets["PackingType"].Visible = true;
				grdfrm.RootTable.ColumnSets["Job"].Visible = true;
				grdfrm.RootTable.Groups.Add("WareHouseName");
				grdfrm.RootTable.Columns["WareHouseName"].Visible = false;
				grdfrm.RootTable.Groups.Add("ItemName");
				grdfrm.RootTable.Columns["ItemName"].Visible = false;
				grdfrm.RootTable.Columns["AccountTitle"].Visible = false;
				grdfrm.RootTable.GroupTotals = (GroupTotals)2;
			}
			else if (Conversion.ToString((object)((Control)(object)cmbReportType).Text) == "Warehouse, Joblot and Item Stock Summary")
			{
				grdfrm.RootTable.ColumnSets["Pack"].Visible = true;
				grdfrm.RootTable.ColumnSets["PackingType"].Visible = true;
				grdfrm.RootTable.Groups.Add("WareHouseName");
				grdfrm.RootTable.Columns["WareHouseName"].Visible = false;
				grdfrm.RootTable.Groups.Add("JobLot");
				grdfrm.RootTable.Columns["JobLot"].Visible = false;
				grdfrm.RootTable.GroupTotals = (GroupTotals)2;
			}
			else if (Conversion.ToString((object)((Control)(object)cmbReportType).Text) == "JobLot and Item")
			{
				grdfrm.RootTable.ColumnSets["Pack"].Visible = true;
				grdfrm.RootTable.ColumnSets["PackingType"].Visible = true;
				grdfrm.RootTable.Groups.Add("JobLot");
				grdfrm.RootTable.Columns["JobLot"].Visible = false;
				grdfrm.RootTable.GroupTotals = (GroupTotals)2;
			}
			else if (Conversion.ToString(cmbReportType.Value) == "ItemWarehouseJobCropPackingTypePackSizeStockSummary")
			{
				grdfrm.RootTable.ColumnSets["WareHouse"].Visible = true;
				grdfrm.RootTable.ColumnSets["PackingType"].Visible = true;
				grdfrm.RootTable.ColumnSets["Pack"].Visible = true;
				grdfrm.RootTable.ColumnSets["Crop"].Visible = true;
				grdfrm.RootTable.ColumnSets["Job"].Visible = true;
			}
			grdfrm.RootTable.ColumnSets["Item"].Width = Constants.InventoryConstants.ItemName + Constants.InventoryConstants.AccountTitle;
			grdfrm.RootTable.Columns["AccountTitle"].Width = Constants.InventoryConstants.AccountTitle;
			grdfrm.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			grdfrm.RootTable.ColumnSets["Pack"].Width = Constants.InventoryConstants.UomConstant;
			grdfrm.RootTable.Columns["PackUom"].Width = Constants.InventoryConstants.UomConstant;
			grdfrm.RootTable.ColumnSets["PackingType"].Width = Constants.InventoryConstants.PackingType;
			grdfrm.RootTable.Columns["PackingType"].Width = Constants.InventoryConstants.PackingType;
			grdfrm.RootTable.ColumnSets["WareHouse"].Width = Constants.InventoryConstants.WareHouseName;
			grdfrm.RootTable.Columns["WareHouseName"].Width = Constants.InventoryConstants.WareHouseName;
			grdfrm.RootTable.ColumnSets["Crop"].Width = Constants.InventoryConstants.CropYearConstant;
			grdfrm.RootTable.Columns["CropYear"].Width = Constants.InventoryConstants.CropYearConstant;
			grdfrm.RootTable.ColumnSets["Job"].Width = Constants.InventoryConstants.JobLotConstant;
			grdfrm.RootTable.Columns["JobLot"].Width = Constants.InventoryConstants.JobLotConstant;
			grdfrm.RootTable.ColumnSets["Qty"].Width = Constants.InventoryConstants.QtyConstant * 4;
			grdfrm.RootTable.Columns["OpQty"].Width = Constants.InventoryConstants.QtyConstant;
			grdfrm.RootTable.Columns["QtyIn"].Width = Constants.InventoryConstants.QtyConstant;
			grdfrm.RootTable.Columns["QtyOut"].Width = Constants.InventoryConstants.QtyConstant;
			grdfrm.RootTable.Columns["BalQty"].Width = Constants.InventoryConstants.QtyConstant;
			grdfrm.RootTable.ColumnSets["Weight"].Width = Constants.InventoryConstants.WeightConstant * 4;
			grdfrm.RootTable.Columns["OpWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdfrm.RootTable.Columns["WeightIn"].Width = Constants.InventoryConstants.WeightConstant;
			grdfrm.RootTable.Columns["WeightOut"].Width = Constants.InventoryConstants.WeightConstant;
			grdfrm.RootTable.Columns["BalWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdfrm.RootTable.ColumnSets["Amount"].Width = Constants.InventoryConstants.AmountForReportConstant * 4 + Constants.InventoryConstants.ExRateConstant;
			grdfrm.RootTable.Columns["OpAmount"].Width = Constants.InventoryConstants.AmountForReportConstant;
			grdfrm.RootTable.Columns["AmountIn"].Width = Constants.InventoryConstants.AmountForReportConstant;
			grdfrm.RootTable.Columns["AmountOut"].Width = Constants.InventoryConstants.AmountForReportConstant;
			grdfrm.RootTable.Columns["BalAmount"].Width = Constants.InventoryConstants.AmountForReportConstant;
			grdfrm.RootTable.Columns["AvgRate"].Width = 100;
			grdfrm.RootTable.ColumnSets["Custom"].Width = 80 + Constants.InventoryConstants.AmountForReportConstant;
			grdfrm.RootTable.Columns["M_Rate/40Kg"].Width = 80;
			grdfrm.RootTable.Columns["M_Amount"].Width = Constants.InventoryConstants.AmountForReportConstant;
			grdfrm.RootTable.Columns["OpWeight"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["OpWeight"].FormatString = "#,##0";
			grdfrm.RootTable.Columns["OpWeight"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["OpWeight"].TotalFormatString = "#,##0";
			grdfrm.RootTable.Columns["WeightIn"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["WeightIn"].FormatString = "#,##0";
			grdfrm.RootTable.Columns["WeightIn"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["WeightIn"].TotalFormatString = "#,##0";
			grdfrm.RootTable.Columns["WeightOut"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["WeightOut"].FormatString = "#,##0";
			grdfrm.RootTable.Columns["WeightOut"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["WeightOut"].TotalFormatString = "#,##0";
			grdfrm.RootTable.Columns["BalWeight"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["BalWeight"].FormatString = "#,##0";
			grdfrm.RootTable.Columns["BalWeight"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["BalWeight"].TotalFormatString = "#,##0";
			grdfrm.RootTable.Columns["OpQty"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["OpQty"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["OpQty"].FormatString = "#,##0";
			grdfrm.RootTable.Columns["OpQty"].TotalFormatString = "#,##0";
			grdfrm.RootTable.Columns["QtyIn"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["QtyIn"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["QtyIn"].FormatString = "#,##0";
			grdfrm.RootTable.Columns["QtyIn"].TotalFormatString = "#,##0";
			grdfrm.RootTable.Columns["QtyOut"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["QtyOut"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["QtyOut"].FormatString = "#,##0";
			grdfrm.RootTable.Columns["QtyOut"].TotalFormatString = "#,##0";
			grdfrm.RootTable.Columns["BalQty"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["BalQty"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["BalQty"].FormatString = "#,##0";
			grdfrm.RootTable.Columns["BalQty"].TotalFormatString = "#,##0";
			grdfrm.RootTable.Columns["OpAmount"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["OpAmount"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["OpAmount"].FormatString = "#,##0";
			grdfrm.RootTable.Columns["OpAmount"].TotalFormatString = "#,##0";
			grdfrm.RootTable.Columns["AmountIn"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["AmountIn"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["AmountIn"].FormatString = "#,##00";
			grdfrm.RootTable.Columns["AmountIn"].TotalFormatString = "#,##00";
			grdfrm.RootTable.Columns["AmountOut"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["AmountOut"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["AmountOut"].FormatString = "#,##00";
			grdfrm.RootTable.Columns["AmountOut"].TotalFormatString = "#,##00";
			grdfrm.RootTable.Columns["AvgRate"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["AvgRate"].FormatString = "#,##0.00";
			grdfrm.RootTable.Columns["AvgRate"].TotalFormatString = "#,##0.00";
			grdfrm.RootTable.Columns["M_Rate/40Kg"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["M_Rate/40Kg"].FormatString = "#,##0.##00";
			grdfrm.RootTable.Columns["M_Rate/40Kg"].TotalFormatString = "#,##0.##00";
			grdfrm.RootTable.Columns["BalAmount"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["BalAmount"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["BalAmount"].FormatString = "#,##00";
			grdfrm.RootTable.Columns["BalAmount"].TotalFormatString = "#,##00";
			grdfrm.RootTable.Columns["M_Amount"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["M_Amount"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["M_Amount"].FormatString = "#,##00";
			grdfrm.RootTable.Columns["M_Amount"].TotalFormatString = "#,##00";
			AvgRateDecimalsChange();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdfrm_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			int ItemId = Conversion.ToInt(grdfrm.CurrentRow.Cells["ItemId"].Value);
			if (e.Column.Key == "ItemName")
			{
				InventoryEvaluationItemLedger obj = new InventoryEvaluationItemLedger(UserAccount);
				obj.RequestedByOtherDocument = true;
				obj.fromdate.Value = txtDateFrom.Value;
				obj.todate.Value = txtDateTo.Value;
				((Control)(object)obj.cmbBranchName).Text = Conversion.ToString((object)((Control)(object)cmbBranchName).Text);
				((Control)(object)obj.cmbparentCategory).Text = Conversion.ToString((object)((Control)(object)cmbMainType).Text);
				if (Conversion.ToInt(CmbClassGroup.Value) > 0)
				{
					obj.cmbClassGroup.Value = Conversion.ToInt(CmbClassGroup.Value);
				}
				if (Conversion.ToInt(cmbCategory.Value) > 0)
				{
					obj.cmbItemcategory.Value = Conversion.ToInt(cmbCategory.Value);
				}
				if (Conversion.ToInt(cmbItemType.Value) > 0)
				{
					obj.cmbItemType.Value = Conversion.ToInt(cmbItemType.Value);
				}
				if (Conversion.ToInt(cmbCropYear.Value) > 0)
				{
					obj.CmbCropYear.Value = Conversion.ToInt(cmbCropYear.Value);
				}
				if (Conversion.ToInt(cmbWareHouse.Value) > 0)
				{
					obj.CmbWareHouse.Value = Conversion.ToInt(cmbWareHouse.Value);
				}
				if (Conversion.ToInt(cmbJobLot.Value) > 0)
				{
					obj.CmbJobLot.Value = Conversion.ToInt(cmbJobLot.Value);
				}
				if (Conversion.ToInt(cmbStockAccounts.Value) > 0)
				{
					obj.CmbItemStockAc.Value = Conversion.ToInt(cmbStockAccounts.Value);
				}
				obj.CmbItem.Value = ItemId;
				obj.RdSaleAmount.Checked = RdSaleAmount.Checked;
				obj.RdCGSAmount.Checked = RdCGSAmount.Checked;
				obj.Show();
			}
			if (e.Column.Key == "AccountTitle")
			{
				ItemEvaluationLedgerNew obj2 = new ItemEvaluationLedgerNew(UserAccount)
				{
					fromdate = 
					{
						Value = txtDateFrom.Value
					},
					todate = 
					{
						Value = txtDateTo.Value
					}
				};
				obj2.CmbItem.Value = ItemId;
				obj2.RequestedByOtherDocument = true;
				obj2.Show();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void grdfrm_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdfrm.UpdateData();
			GridEXRow r = grdfrm.CurrentRow;
			if (r != null && (int)r.RowType == 0 && e.Column.Key == "M_Rate/40Kg")
			{
				double Weight = Conversion.ToDouble(r.Cells["BalWeight"].Value);
				double Rate = Conversion.ToDouble(r.Cells["M_Rate/40Kg"].Value);
				int RateUOM = 40;
				if (Weight > 0.0 && Rate > 0.0)
				{
					double ItemAmount = Weight / (double)RateUOM * Rate;
					r.Cells["M_Amount"].Value = ItemAmount;
				}
				else
				{
					r.Cells["M_Amount"].Value = 0;
				}
			}
			grdfrm.UpdateData();
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
			BranchesFill();
			ParentCategoryComboFill();
			OtherComboByParentCategoryFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void frmStockReportWithValues_FormClosing(object sender, FormClosingEventArgs e)
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

	private void BtnReset_Click(object sender, EventArgs e)
	{
		try
		{
			txtDateFrom.Value = clsGlobalVariables.ActiveYr.Start_Period;
			((Control)(object)cmbCategory).Text = string.Empty;
			((Control)(object)cmbCropYear).Text = string.Empty;
			((Control)(object)cmbItem).Text = string.Empty;
			((Control)(object)cmbItemType).Text = string.Empty;
			((Control)(object)cmbJobLot).Text = string.Empty;
			((Control)(object)cmbMainType).Text = string.Empty;
			((Control)(object)cmbReportType).Text = string.Empty;
			((Control)(object)CmbClassGroup).Text = string.Empty;
			((Control)(object)cmbWareHouse).Text = string.Empty;
			txtDateFrom.Focus();
			BtnUpdate.Visible = false;
			BtnUpdate.Enabled = false;
			DateTime AsOnDate = StocksReport.GetInventoryStockAsOnDate(UserAccount.OrganizationId, UserAccount.CompanyId);
			if (!Conversion.CheckDateTimeNull(AsOnDate))
			{
				txtDateFrom.Value = AsOnDate;
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdfrm).Name);
			if (fs != null)
			{
				grdfrm.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void PrintButtonManage()
	{
		try
		{
			if (((UltraGridBase)cmbReportType).ActiveRow != null)
			{
				btnPrint.Visible = true;
				if (Conversion.ToString(cmbReportType.Value) == "ItemStockSummary")
				{
					btnPrint.Text = "188-ItemStockSummary";
				}
				if (Conversion.ToString(cmbReportType.Value) == "ItemandWarehouseStockSummary")
				{
					btnPrint.Text = "189-ItemandWarehouseStockSummary";
				}
				if (Conversion.ToString(cmbReportType.Value) == "ItemandWarehouseStockSummary")
				{
					btnPrint.Text = "190-ItemandWarehouseStockSummary";
				}
				if (Conversion.ToString(cmbReportType.Value) == "ItemandCropYearStockSummary")
				{
					btnPrint.Text = "191-ItemandCropYearStockSummary";
				}
				if (Conversion.ToString(cmbReportType.Value) == "ItemandCropYearandWarehouseStockSummary")
				{
					btnPrint.Text = "192-ItemandCropYearandWarehouseStockSummary";
				}
				if (Conversion.ToString(cmbReportType.Value) == "JobLotandItemStockSummary")
				{
					btnPrint.Text = "193-JobLotandItemStockSummary";
				}
				if (Conversion.ToString((object)((Control)(object)cmbReportType).Text) == "Warehouse, Joblot and Item Stock Summary")
				{
					btnPrint.Text = "194-WarehouseandJoblotandItemStockSummary";
				}
				if (Conversion.ToString((object)((Control)(object)cmbReportType).Text) == "Warehouse, Item and Joblot Stock Summary")
				{
					btnPrint.Text = "194A-WarehouseandItemandJoblotStockSummary";
				}
				if (Conversion.ToString(cmbReportType.Value) == "ItemandPackSizeStockSummary")
				{
					btnPrint.Text = "195-ItemandPackSizeStockSummary";
				}
				if (Conversion.ToString(cmbReportType.Value) == "ItemandPackingTypeStockSummary")
				{
					btnPrint.Text = "196-ItemandPackingTypeStockSummary";
				}
				if (Conversion.ToString(cmbReportType.Value) == "ItemandPackSizeandPackingTypeStockSummary")
				{
					btnPrint.Text = "197-ItemandPackSizeandPackingTypeStockSummary";
				}
			}
			else
			{
				btnPrint.Visible = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrint_Click(object sender, EventArgs e)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0278: Unknown result type (might be due to invalid IL or missing references)
		//IL_02df: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0366: Unknown result type (might be due to invalid IL or missing references)
		//IL_036b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0386: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_040d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0474: Unknown result type (might be due to invalid IL or missing references)
		//IL_0479: Unknown result type (might be due to invalid IL or missing references)
		//IL_0494: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0500: Unknown result type (might be due to invalid IL or missing references)
		//IL_051b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0582: Unknown result type (might be due to invalid IL or missing references)
		//IL_0587: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a2: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (btnPrint.Text == "188-ItemStockSummary")
			{
				if (dtStockSum.Rows.Count == 0 || dtStockSum == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dtStockSum, "188-ItemStockSummary.rpt");
			}
			if (btnPrint.Text == "189-ItemandWarehouseStockSummary")
			{
				if (dtStockSum.Rows.Count == 0 || dtStockSum == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val2 = new Reporting();
				val2.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val2.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val2.ShowReportWithDataTable(dtStockSum, "189-ItemandWarehouseStockSummary.rpt");
			}
			if (btnPrint.Text == "190-ItemandWarehouseStockSummary")
			{
				if (dtStockSum.Rows.Count == 0 || dtStockSum == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val3 = new Reporting();
				val3.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val3.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val3.ShowReportWithDataTable(dtStockSum, "190-ItemandWarehouseStockSummary.rpt");
			}
			if (btnPrint.Text == "191-ItemandCropYearStockSummary")
			{
				if (dtStockSum.Rows.Count == 0 || dtStockSum == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val4 = new Reporting();
				val4.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val4.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val4.ShowReportWithDataTable(dtStockSum, "191-ItemandCropYearStockSummary.rpt");
			}
			if (btnPrint.Text == "192-ItemandCropYearandWarehouseStockSummary")
			{
				if (dtStockSum.Rows.Count == 0 || dtStockSum == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val5 = new Reporting();
				val5.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val5.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val5.ShowReportWithDataTable(dtStockSum, "192-ItemandCropYearandWarehouseStockSummary.rpt");
			}
			if (btnPrint.Text == "193-JobLotandItemStockSummary")
			{
				if (dtStockSum.Rows.Count == 0 || dtStockSum == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val6 = new Reporting();
				val6.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val6.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val6.ShowReportWithDataTable(dtStockSum, "193-JobLotandItemStockSummary.rpt");
			}
			if (btnPrint.Text == "194-WarehouseandJoblotandItemStockSummary")
			{
				if (dtStockSum.Rows.Count == 0 || dtStockSum == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val7 = new Reporting();
				val7.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val7.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val7.ShowReportWithDataTable(dtStockSum, "194-WarehouseandJoblotandItemStockSummary.rpt");
			}
			if (btnPrint.Text == "194A-WarehouseandItemandJoblotStockSummary")
			{
				if (dtStockSum.Rows.Count == 0 || dtStockSum == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val8 = new Reporting();
				val8.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val8.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val8.ShowReportWithDataTable(dtStockSum, "194A-WarehouseandItemandJoblotStockSummary.rpt");
			}
			if (btnPrint.Text == "195-ItemandPackSizeStockSummary")
			{
				if (dtStockSum.Rows.Count == 0 || dtStockSum == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val9 = new Reporting();
				val9.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val9.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val9.ShowReportWithDataTable(dtStockSum, "195-ItemandPackSizeStockSummary.rpt");
			}
			if (btnPrint.Text == "196-ItemandPackingTypeStockSummary")
			{
				if (dtStockSum.Rows.Count == 0 || dtStockSum == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val10 = new Reporting();
				val10.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val10.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val10.ShowReportWithDataTable(dtStockSum, "196-ItemandPackingTypeStockSummary.rpt");
			}
			if (btnPrint.Text == "197-ItemandPackSizeandPackingTypeStockSummary")
			{
				if (dtStockSum.Rows.Count == 0 || dtStockSum == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val11 = new Reporting();
				val11.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val11.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val11.ShowReportWithDataTable(dtStockSum, "197-ItemandPackSizeandPackingTypeStockSummary.rpt");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrintStockSummeryByWeight_Click(object sender, EventArgs e)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtStockSum.Rows.Count == 0 || dtStockSum == null)
			{
				throw new Exception("Record Not Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dtStockSum, "188_01-ItemStockSummary.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrintStockReport_Click(object sender, EventArgs e)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtStockSum.Rows.Count == 0 || dtStockSum == null)
			{
				throw new Exception("Record Not Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dtStockSum, "188_02-ItemStockSummary.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmStockReportWithValues_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.P)
			{
				btnPrint_Click(null, null);
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				BtnReset_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.R)
			{
				btnRefresh_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				btnshow_Click(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AvgRateDecimalsChange()
	{
		if (rdAvgRateTwoDecimals.Checked)
		{
			grdfrm.RootTable.Columns["AvgRate"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["AvgRate"].FormatString = "#,##0.00";
			grdfrm.RootTable.Columns["M_Rate/40Kg"].FormatString = "#,##0.00";
		}
		else if (rdAvgRateThreeDecimals.Checked)
		{
			grdfrm.RootTable.Columns["AvgRate"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["AvgRate"].FormatString = "#,##0.000";
			grdfrm.RootTable.Columns["M_Rate/40Kg"].FormatString = "#,##0.000";
		}
		else if (rdAvgRateFourDecimals.Checked)
		{
			grdfrm.RootTable.Columns["AvgRate"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["AvgRate"].FormatString = "#,##0.0000";
			grdfrm.RootTable.Columns["M_Rate/40Kg"].FormatString = "#,##0.0000";
		}
	}

	private void rdAvgRateTwoDecimals_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			AvgRateDecimalsChange();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void rdAvgRateThreeDecimals_CheckedChanged(object sender, EventArgs e)
	{
		AvgRateDecimalsChange();
	}

	private void rdAvgRateFourDecimals_CheckedChanged(object sender, EventArgs e)
	{
		AvgRateDecimalsChange();
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
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Expected O, but got Unknown
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Expected O, but got Unknown
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Expected O, but got Unknown
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Expected O, but got Unknown
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Expected O, but got Unknown
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Expected O, but got Unknown
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Expected O, but got Unknown
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Expected O, but got Unknown
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Expected O, but got Unknown
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Expected O, but got Unknown
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Expected O, but got Unknown
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Expected O, but got Unknown
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Expected O, but got Unknown
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Expected O, but got Unknown
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Expected O, but got Unknown
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Expected O, but got Unknown
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Expected O, but got Unknown
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Expected O, but got Unknown
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Expected O, but got Unknown
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Expected O, but got Unknown
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Expected O, but got Unknown
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Expected O, but got Unknown
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Expected O, but got Unknown
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Expected O, but got Unknown
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Expected O, but got Unknown
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Expected O, but got Unknown
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Expected O, but got Unknown
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Expected O, but got Unknown
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Expected O, but got Unknown
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Expected O, but got Unknown
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Expected O, but got Unknown
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Expected O, but got Unknown
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Expected O, but got Unknown
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Expected O, but got Unknown
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Expected O, but got Unknown
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Expected O, but got Unknown
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Expected O, but got Unknown
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Expected O, but got Unknown
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Expected O, but got Unknown
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Expected O, but got Unknown
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Expected O, but got Unknown
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Expected O, but got Unknown
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Expected O, but got Unknown
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0244: Expected O, but got Unknown
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Expected O, but got Unknown
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Expected O, but got Unknown
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Expected O, but got Unknown
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		//IL_0260: Expected O, but got Unknown
		//IL_0260: Unknown result type (might be due to invalid IL or missing references)
		//IL_0267: Expected O, but got Unknown
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Expected O, but got Unknown
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Expected O, but got Unknown
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_027c: Expected O, but got Unknown
		//IL_027c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0283: Expected O, but got Unknown
		//IL_0283: Unknown result type (might be due to invalid IL or missing references)
		//IL_028a: Expected O, but got Unknown
		//IL_028a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0291: Expected O, but got Unknown
		//IL_0291: Unknown result type (might be due to invalid IL or missing references)
		//IL_0298: Expected O, but got Unknown
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Expected O, but got Unknown
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a6: Expected O, but got Unknown
		//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Expected O, but got Unknown
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b4: Expected O, but got Unknown
		//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bb: Expected O, but got Unknown
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c2: Expected O, but got Unknown
		//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c9: Expected O, but got Unknown
		//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d0: Expected O, but got Unknown
		//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d7: Expected O, but got Unknown
		//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02de: Expected O, but got Unknown
		//IL_02de: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e5: Expected O, but got Unknown
		//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ec: Expected O, but got Unknown
		//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f3: Expected O, but got Unknown
		//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fa: Expected O, but got Unknown
		//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0301: Expected O, but got Unknown
		//IL_0301: Unknown result type (might be due to invalid IL or missing references)
		//IL_0308: Expected O, but got Unknown
		//IL_0308: Unknown result type (might be due to invalid IL or missing references)
		//IL_030f: Expected O, but got Unknown
		//IL_030f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0316: Expected O, but got Unknown
		//IL_0316: Unknown result type (might be due to invalid IL or missing references)
		//IL_031d: Expected O, but got Unknown
		//IL_031d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0324: Expected O, but got Unknown
		//IL_0324: Unknown result type (might be due to invalid IL or missing references)
		//IL_032b: Expected O, but got Unknown
		//IL_032b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0332: Expected O, but got Unknown
		//IL_0332: Unknown result type (might be due to invalid IL or missing references)
		//IL_0339: Expected O, but got Unknown
		//IL_0339: Unknown result type (might be due to invalid IL or missing references)
		//IL_0340: Expected O, but got Unknown
		//IL_0340: Unknown result type (might be due to invalid IL or missing references)
		//IL_0347: Expected O, but got Unknown
		//IL_0347: Unknown result type (might be due to invalid IL or missing references)
		//IL_034e: Expected O, but got Unknown
		//IL_034e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0355: Expected O, but got Unknown
		//IL_0355: Unknown result type (might be due to invalid IL or missing references)
		//IL_035c: Expected O, but got Unknown
		//IL_035c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0363: Expected O, but got Unknown
		//IL_0363: Unknown result type (might be due to invalid IL or missing references)
		//IL_036a: Expected O, but got Unknown
		//IL_036a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0371: Expected O, but got Unknown
		//IL_0371: Unknown result type (might be due to invalid IL or missing references)
		//IL_0378: Expected O, but got Unknown
		//IL_0378: Unknown result type (might be due to invalid IL or missing references)
		//IL_037f: Expected O, but got Unknown
		//IL_037f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0386: Expected O, but got Unknown
		//IL_0386: Unknown result type (might be due to invalid IL or missing references)
		//IL_038d: Expected O, but got Unknown
		//IL_038d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0394: Expected O, but got Unknown
		//IL_0394: Unknown result type (might be due to invalid IL or missing references)
		//IL_039b: Expected O, but got Unknown
		//IL_039b: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a2: Expected O, but got Unknown
		//IL_03a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a9: Expected O, but got Unknown
		//IL_03a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b0: Expected O, but got Unknown
		//IL_03b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b7: Expected O, but got Unknown
		//IL_03b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03be: Expected O, but got Unknown
		//IL_03be: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c5: Expected O, but got Unknown
		//IL_03c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cc: Expected O, but got Unknown
		//IL_03cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d3: Expected O, but got Unknown
		//IL_03d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03da: Expected O, but got Unknown
		//IL_03da: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e1: Expected O, but got Unknown
		//IL_03e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e8: Expected O, but got Unknown
		//IL_03e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ef: Expected O, but got Unknown
		//IL_03ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f6: Expected O, but got Unknown
		//IL_03f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fd: Expected O, but got Unknown
		//IL_03fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0404: Expected O, but got Unknown
		//IL_0404: Unknown result type (might be due to invalid IL or missing references)
		//IL_040b: Expected O, but got Unknown
		//IL_040b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0412: Expected O, but got Unknown
		//IL_0412: Unknown result type (might be due to invalid IL or missing references)
		//IL_0419: Expected O, but got Unknown
		//IL_0419: Unknown result type (might be due to invalid IL or missing references)
		//IL_0420: Expected O, but got Unknown
		//IL_0420: Unknown result type (might be due to invalid IL or missing references)
		//IL_0427: Expected O, but got Unknown
		//IL_0427: Unknown result type (might be due to invalid IL or missing references)
		//IL_042e: Expected O, but got Unknown
		//IL_042e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0435: Expected O, but got Unknown
		//IL_0435: Unknown result type (might be due to invalid IL or missing references)
		//IL_043c: Expected O, but got Unknown
		//IL_043c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0443: Expected O, but got Unknown
		//IL_0443: Unknown result type (might be due to invalid IL or missing references)
		//IL_044a: Expected O, but got Unknown
		//IL_044a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0451: Expected O, but got Unknown
		//IL_0451: Unknown result type (might be due to invalid IL or missing references)
		//IL_0458: Expected O, but got Unknown
		//IL_0458: Unknown result type (might be due to invalid IL or missing references)
		//IL_045f: Expected O, but got Unknown
		//IL_045f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0466: Expected O, but got Unknown
		//IL_0466: Unknown result type (might be due to invalid IL or missing references)
		//IL_046d: Expected O, but got Unknown
		//IL_046d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0474: Expected O, but got Unknown
		//IL_0474: Unknown result type (might be due to invalid IL or missing references)
		//IL_047b: Expected O, but got Unknown
		//IL_047b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0482: Expected O, but got Unknown
		//IL_0482: Unknown result type (might be due to invalid IL or missing references)
		//IL_0489: Expected O, but got Unknown
		//IL_0489: Unknown result type (might be due to invalid IL or missing references)
		//IL_0490: Expected O, but got Unknown
		//IL_0490: Unknown result type (might be due to invalid IL or missing references)
		//IL_0497: Expected O, but got Unknown
		//IL_0497: Unknown result type (might be due to invalid IL or missing references)
		//IL_049e: Expected O, but got Unknown
		//IL_049e: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a5: Expected O, but got Unknown
		//IL_04a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ac: Expected O, but got Unknown
		//IL_04b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c2: Expected O, but got Unknown
		//IL_0526: Unknown result type (might be due to invalid IL or missing references)
		//IL_0530: Expected O, but got Unknown
		//IL_0531: Unknown result type (might be due to invalid IL or missing references)
		//IL_053b: Expected O, but got Unknown
		//IL_0568: Unknown result type (might be due to invalid IL or missing references)
		//IL_0572: Expected O, but got Unknown
		//IL_057e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0588: Expected O, but got Unknown
		//IL_0594: Unknown result type (might be due to invalid IL or missing references)
		//IL_059e: Expected O, but got Unknown
		//IL_05c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ca: Expected O, but got Unknown
		//IL_05f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0601: Expected O, but got Unknown
		//IL_060d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0617: Expected O, but got Unknown
		//IL_0623: Unknown result type (might be due to invalid IL or missing references)
		//IL_062d: Expected O, but got Unknown
		//IL_0639: Unknown result type (might be due to invalid IL or missing references)
		//IL_0643: Expected O, but got Unknown
		//IL_064f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0659: Expected O, but got Unknown
		//IL_0665: Unknown result type (might be due to invalid IL or missing references)
		//IL_066f: Expected O, but got Unknown
		//IL_069c: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a6: Expected O, but got Unknown
		//IL_06a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b1: Expected O, but got Unknown
		//IL_09fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a06: Expected O, but got Unknown
		//IL_0a13: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a1d: Expected O, but got Unknown
		GridEXLayout grdfrm_DesignTimeLayout = new GridEXLayout();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Inventory_Stocks_Report.frmStockReportWithValues));
		Appearance appearance100 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance157 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance168 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.grdfrm = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.rdAvgRatePerKg = new System.Windows.Forms.RadioButton();
		this.rdAvgRate40Kg = new System.Windows.Forms.RadioButton();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel3 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.rdAvgRateFourDecimals = new System.Windows.Forms.RadioButton();
		this.rdAvgRateThreeDecimals = new System.Windows.Forms.RadioButton();
		this.rdAvgRateTwoDecimals = new System.Windows.Forms.RadioButton();
		this.cmbMainType = new UltraCombo();
		this.cmbBranchName = new UltraCombo();
		this.label14 = new System.Windows.Forms.Label();
		this.chkIncludeWipItem = new System.Windows.Forms.CheckBox();
		this.chkSkipZero = new System.Windows.Forms.CheckBox();
		this.label13 = new System.Windows.Forms.Label();
		this.cmbPacktype = new UltraCombo();
		this.label12 = new System.Windows.Forms.Label();
		this.cmbPackUom = new UltraCombo();
		this.lblStockAcItem = new System.Windows.Forms.Label();
		this.cmbStockAccounts = new UltraCombo();
		this.panel5 = new System.Windows.Forms.Panel();
		this.label2 = new System.Windows.Forms.Label();
		this.RdCGSAmount = new System.Windows.Forms.RadioButton();
		this.cmbStockGroupAccount = new UltraCombo();
		this.RdSaleAmount = new System.Windows.Forms.RadioButton();
		this.label23 = new System.Windows.Forms.Label();
		this.ChkItemWiseStock = new System.Windows.Forms.CheckBox();
		this.label11 = new System.Windows.Forms.Label();
		this.cmbReportType = new UltraCombo();
		this.label10 = new System.Windows.Forms.Label();
		this.cmbCropYear = new UltraCombo();
		this.label5 = new System.Windows.Forms.Label();
		this.cmbJobLot = new UltraCombo();
		this.label4 = new System.Windows.Forms.Label();
		this.CmbClassGroup = new UltraCombo();
		this.label9 = new System.Windows.Forms.Label();
		this.cmbItem = new UltraCombo();
		this.label8 = new System.Windows.Forms.Label();
		this.cmbItemType = new UltraCombo();
		this.label7 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.cmbWareHouse = new UltraCombo();
		this.cmbCategory = new UltraCombo();
		this.btnshow = new System.Windows.Forms.Button();
		this.txtDateFrom = new System.Windows.Forms.DateTimePicker();
		this.txtDateTo = new System.Windows.Forms.DateTimePicker();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.BtnReset = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.btnPrintStockSummeryByWeight = new System.Windows.Forms.ToolStripButton();
		this.btnPrintStockReport = new System.Windows.Forms.ToolStripButton();
		this.BtnUpdate = new System.Windows.Forms.ToolStripButton();
		this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdfrm).BeginInit();
		this.panel4.SuspendLayout();
		this.panel3.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbMainType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPacktype).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPackUom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbStockAccounts).BeginInit();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbStockGroupAccount).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbReportType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCropYear).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbJobLot).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbClassGroup).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbWareHouse).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCategory).BeginInit();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.grdfrm);
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1168, 561);
		this.panel1.TabIndex = 0;
		grdfrm_DesignTimeLayout.LayoutString = resources.GetString("grdfrm_DesignTimeLayout.LayoutString");
		this.grdfrm.DesignTimeLayout = grdfrm_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdfrm).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdfrm.DynamicFiltering = true;
		this.grdfrm.FilterMode = (FilterMode)2;
		this.grdfrm.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdfrm).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdfrm.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdfrm).Location = new System.Drawing.Point(0, 229);
		((System.Windows.Forms.Control)(object)this.grdfrm).Name = "grdfrm";
		((ScrollableBase)this.grdfrm).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdfrm).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdfrm).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Size = new System.Drawing.Size(1168, 332);
		((System.Windows.Forms.Control)(object)this.grdfrm).TabIndex = 0;
		this.grdfrm.TotalRow = (InheritableBoolean)1;
		this.grdfrm.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdfrm).VisualStyle = (VisualStyle)4;
		this.grdfrm.CellUpdated += new ColumnActionEventHandler(grdfrm_CellUpdated);
		this.grdfrm.LinkClicked += new ColumnActionEventHandler(grdfrm_LinkClicked);
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.rdAvgRatePerKg);
		this.panel4.Controls.Add(this.rdAvgRate40Kg);
		this.panel4.Controls.Add(this.ctrlGrdBar1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 206);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1168, 23);
		this.panel4.TabIndex = 2;
		this.rdAvgRatePerKg.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.rdAvgRatePerKg.AutoSize = true;
		this.rdAvgRatePerKg.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdAvgRatePerKg.ForeColor = System.Drawing.Color.White;
		this.rdAvgRatePerKg.Location = new System.Drawing.Point(862, 1);
		this.rdAvgRatePerKg.Name = "rdAvgRatePerKg";
		this.rdAvgRatePerKg.Size = new System.Drawing.Size(119, 21);
		this.rdAvgRatePerKg.TabIndex = 221;
		this.rdAvgRatePerKg.Text = "Avg Rate / 1KG";
		this.rdAvgRatePerKg.UseVisualStyleBackColor = true;
		this.rdAvgRate40Kg.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.rdAvgRate40Kg.AutoSize = true;
		this.rdAvgRate40Kg.Checked = true;
		this.rdAvgRate40Kg.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdAvgRate40Kg.ForeColor = System.Drawing.Color.White;
		this.rdAvgRate40Kg.Location = new System.Drawing.Point(991, 1);
		this.rdAvgRate40Kg.Name = "rdAvgRate40Kg";
		this.rdAvgRate40Kg.Size = new System.Drawing.Size(126, 21);
		this.rdAvgRate40Kg.TabIndex = 221;
		this.rdAvgRate40Kg.TabStop = true;
		this.rdAvgRate40Kg.Text = "Avg Rate / 40KG";
		this.rdAvgRate40Kg.UseVisualStyleBackColor = true;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1123, 1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdfrm;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 0;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add(this.groupBox1);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbMainType);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbBranchName);
		this.panel3.Controls.Add(this.label14);
		this.panel3.Controls.Add(this.chkIncludeWipItem);
		this.panel3.Controls.Add(this.chkSkipZero);
		this.panel3.Controls.Add(this.label13);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPacktype);
		this.panel3.Controls.Add(this.label12);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPackUom);
		this.panel3.Controls.Add(this.lblStockAcItem);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbStockAccounts);
		this.panel3.Controls.Add(this.panel5);
		this.panel3.Controls.Add(this.ChkItemWiseStock);
		this.panel3.Controls.Add(this.label11);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbReportType);
		this.panel3.Controls.Add(this.label10);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCropYear);
		this.panel3.Controls.Add(this.label5);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbJobLot);
		this.panel3.Controls.Add(this.label4);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbClassGroup);
		this.panel3.Controls.Add(this.label9);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItem);
		this.panel3.Controls.Add(this.label8);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemType);
		this.panel3.Controls.Add(this.label7);
		this.panel3.Controls.Add(this.label6);
		this.panel3.Controls.Add(this.label3);
		this.panel3.Controls.Add(this.label1);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbWareHouse);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCategory);
		this.panel3.Controls.Add(this.btnshow);
		this.panel3.Controls.Add(this.txtDateFrom);
		this.panel3.Controls.Add(this.txtDateTo);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 27);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1168, 179);
		this.panel3.TabIndex = 0;
		this.groupBox1.Controls.Add(this.rdAvgRateFourDecimals);
		this.groupBox1.Controls.Add(this.rdAvgRateThreeDecimals);
		this.groupBox1.Controls.Add(this.rdAvgRateTwoDecimals);
		this.groupBox1.Location = new System.Drawing.Point(960, 35);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(113, 77);
		this.groupBox1.TabIndex = 1;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Avg Rate Decimals";
		this.rdAvgRateFourDecimals.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.rdAvgRateFourDecimals.AutoSize = true;
		this.rdAvgRateFourDecimals.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdAvgRateFourDecimals.ForeColor = System.Drawing.Color.Black;
		this.rdAvgRateFourDecimals.Location = new System.Drawing.Point(5, 50);
		this.rdAvgRateFourDecimals.Name = "rdAvgRateFourDecimals";
		this.rdAvgRateFourDecimals.Size = new System.Drawing.Size(49, 19);
		this.rdAvgRateFourDecimals.TabIndex = 223;
		this.rdAvgRateFourDecimals.Text = "Four";
		this.rdAvgRateFourDecimals.UseVisualStyleBackColor = true;
		this.rdAvgRateFourDecimals.CheckedChanged += new System.EventHandler(rdAvgRateFourDecimals_CheckedChanged);
		this.rdAvgRateThreeDecimals.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.rdAvgRateThreeDecimals.AutoSize = true;
		this.rdAvgRateThreeDecimals.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdAvgRateThreeDecimals.ForeColor = System.Drawing.Color.Black;
		this.rdAvgRateThreeDecimals.Location = new System.Drawing.Point(5, 32);
		this.rdAvgRateThreeDecimals.Name = "rdAvgRateThreeDecimals";
		this.rdAvgRateThreeDecimals.Size = new System.Drawing.Size(55, 19);
		this.rdAvgRateThreeDecimals.TabIndex = 222;
		this.rdAvgRateThreeDecimals.Text = "Three";
		this.rdAvgRateThreeDecimals.UseVisualStyleBackColor = true;
		this.rdAvgRateThreeDecimals.CheckedChanged += new System.EventHandler(rdAvgRateThreeDecimals_CheckedChanged);
		this.rdAvgRateTwoDecimals.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.rdAvgRateTwoDecimals.AutoSize = true;
		this.rdAvgRateTwoDecimals.Checked = true;
		this.rdAvgRateTwoDecimals.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdAvgRateTwoDecimals.ForeColor = System.Drawing.Color.Black;
		this.rdAvgRateTwoDecimals.Location = new System.Drawing.Point(5, 14);
		this.rdAvgRateTwoDecimals.Name = "rdAvgRateTwoDecimals";
		this.rdAvgRateTwoDecimals.Size = new System.Drawing.Size(47, 19);
		this.rdAvgRateTwoDecimals.TabIndex = 221;
		this.rdAvgRateTwoDecimals.TabStop = true;
		this.rdAvgRateTwoDecimals.Text = "Two";
		this.rdAvgRateTwoDecimals.UseVisualStyleBackColor = true;
		this.rdAvgRateTwoDecimals.CheckedChanged += new System.EventHandler(rdAvgRateTwoDecimals_CheckedChanged);
		this.cmbMainType.AllowNull = (DefaultableBoolean)1;
		this.cmbMainType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbMainType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbMainType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbMainType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance100;
		((UltraGridBase)this.cmbMainType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbMainType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance101).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance101).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance101).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbMainType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance101;
		((AppearanceBase)appearance102).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbMainType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance102;
		((SpecialBoxBase)((UltraGridBase)this.cmbMainType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance104).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance104).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance104).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbMainType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.cmbMainType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbMainType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance105).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbMainType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance105;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance106).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbMainType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.cmbMainType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbMainType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbMainType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance107;
		((AppearanceBase)appearance108).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance108).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbMainType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.cmbMainType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbMainType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance109).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance109).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance109).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance109).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbMainType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance109;
		((AppearanceBase)appearance110).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbMainType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.cmbMainType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbMainType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance111).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbMainType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.cmbMainType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbMainType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.cmbMainType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbMainType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbMainType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbMainType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.cmbMainType).Location = new System.Drawing.Point(93, 91);
		((System.Windows.Forms.Control)(object)this.cmbMainType).Name = "cmbMainType";
		((System.Windows.Forms.Control)(object)this.cmbMainType).Size = new System.Drawing.Size(236, 26);
		((System.Windows.Forms.Control)(object)this.cmbMainType).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.cmbMainType).Leave += new System.EventHandler(cmbMainType_Leave);
		this.cmbBranchName.AllowNull = (DefaultableBoolean)1;
		this.cmbBranchName.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbBranchName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbBranchName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance57;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance78).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance78;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance82).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance82).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance83).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance83).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance83).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance83).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance85).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Location = new System.Drawing.Point(93, 35);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Name = "cmbBranchName";
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Size = new System.Drawing.Size(236, 26);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Leave += new System.EventHandler(cmbBranchName_Leave);
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(3, 41);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(79, 15);
		this.label14.TabIndex = 218;
		this.label14.Text = "Branch Name";
		this.chkIncludeWipItem.AutoSize = true;
		this.chkIncludeWipItem.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.chkIncludeWipItem.Location = new System.Drawing.Point(960, 156);
		this.chkIncludeWipItem.Name = "chkIncludeWipItem";
		this.chkIncludeWipItem.Size = new System.Drawing.Size(120, 19);
		this.chkIncludeWipItem.TabIndex = 17;
		this.chkIncludeWipItem.Text = "Include WIP Item";
		this.chkIncludeWipItem.UseVisualStyleBackColor = true;
		this.chkSkipZero.AutoSize = true;
		this.chkSkipZero.Checked = true;
		this.chkSkipZero.CheckState = System.Windows.Forms.CheckState.Checked;
		this.chkSkipZero.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.chkSkipZero.Location = new System.Drawing.Point(961, 115);
		this.chkSkipZero.Name = "chkSkipZero";
		this.chkSkipZero.Size = new System.Drawing.Size(76, 19);
		this.chkSkipZero.TabIndex = 16;
		this.chkSkipZero.Text = "Skip Zero";
		this.chkSkipZero.UseVisualStyleBackColor = true;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label13.Location = new System.Drawing.Point(333, 69);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(77, 15);
		this.label13.TabIndex = 115;
		this.label13.Text = "Packing Type";
		this.cmbPacktype.AllowNull = (DefaultableBoolean)1;
		this.cmbPacktype.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPacktype.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPacktype.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.Appearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance4).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance4).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance4).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPacktype).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance5;
		((SpecialBoxBase)((UltraGridBase)this.cmbPacktype).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance7;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance10).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance11).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance11).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance11).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance11).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance12).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPacktype).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPacktype).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbPacktype.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPacktype).Location = new System.Drawing.Point(411, 63);
		((System.Windows.Forms.Control)(object)this.cmbPacktype).Name = "cmbPacktype";
		((System.Windows.Forms.Control)(object)this.cmbPacktype).Size = new System.Drawing.Size(157, 26);
		((System.Windows.Forms.Control)(object)this.cmbPacktype).TabIndex = 7;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label12.Location = new System.Drawing.Point(333, 97);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(61, 15);
		this.label12.TabIndex = 113;
		this.label12.Text = "Pack Uom";
		this.cmbPackUom.AllowNull = (DefaultableBoolean)1;
		this.cmbPackUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPackUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPackUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPackUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.cmbPackUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance33).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPackUom).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbPackUom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPackUom).Location = new System.Drawing.Point(411, 91);
		((System.Windows.Forms.Control)(object)this.cmbPackUom).Name = "cmbPackUom";
		((System.Windows.Forms.Control)(object)this.cmbPackUom).Size = new System.Drawing.Size(157, 26);
		((System.Windows.Forms.Control)(object)this.cmbPackUom).TabIndex = 8;
		this.lblStockAcItem.AutoSize = true;
		this.lblStockAcItem.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblStockAcItem.Location = new System.Drawing.Point(572, 97);
		this.lblStockAcItem.Name = "lblStockAcItem";
		this.lblStockAcItem.Size = new System.Drawing.Size(85, 15);
		this.lblStockAcItem.TabIndex = 111;
		this.lblStockAcItem.Text = "Stock Account";
		this.cmbStockAccounts.AllowNull = (DefaultableBoolean)1;
		this.cmbStockAccounts.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbStockAccounts.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbStockAccounts.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbStockAccounts).DisplayLayout.Appearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.cmbStockAccounts).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbStockAccounts).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbStockAccounts).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbStockAccounts).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance67;
		((SpecialBoxBase)((UltraGridBase)this.cmbStockAccounts).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbStockAccounts).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.cmbStockAccounts).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbStockAccounts).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbStockAccounts).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbStockAccounts).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.cmbStockAccounts).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbStockAccounts).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbStockAccounts).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance73).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbStockAccounts).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.cmbStockAccounts).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbStockAccounts).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance74).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbStockAccounts).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbStockAccounts).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.cmbStockAccounts).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbStockAccounts).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbStockAccounts).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.cmbStockAccounts).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbStockAccounts).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.cmbStockAccounts).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbStockAccounts).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbStockAccounts).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbStockAccounts).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbStockAccounts.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbStockAccounts).Location = new System.Drawing.Point(660, 91);
		((System.Windows.Forms.Control)(object)this.cmbStockAccounts).Name = "cmbStockAccounts";
		((System.Windows.Forms.Control)(object)this.cmbStockAccounts).Size = new System.Drawing.Size(295, 26);
		((System.Windows.Forms.Control)(object)this.cmbStockAccounts).TabIndex = 13;
		this.panel5.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel5.Controls.Add(this.label2);
		this.panel5.Controls.Add(this.RdCGSAmount);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbStockGroupAccount);
		this.panel5.Controls.Add(this.RdSaleAmount);
		this.panel5.Controls.Add(this.label23);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1168, 31);
		this.panel5.TabIndex = 109;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(222, 8);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(129, 15);
		this.label2.TabIndex = 220;
		this.label2.Text = "Stock Account (Group)";
		this.RdCGSAmount.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.RdCGSAmount.AutoSize = true;
		this.RdCGSAmount.Checked = true;
		this.RdCGSAmount.ForeColor = System.Drawing.Color.White;
		this.RdCGSAmount.Location = new System.Drawing.Point(997, 6);
		this.RdCGSAmount.Name = "RdCGSAmount";
		this.RdCGSAmount.Size = new System.Drawing.Size(77, 17);
		this.RdCGSAmount.TabIndex = 7;
		this.RdCGSAmount.TabStop = true;
		this.RdCGSAmount.Text = "CGS Value";
		this.RdCGSAmount.UseVisualStyleBackColor = true;
		this.cmbStockGroupAccount.AllowNull = (DefaultableBoolean)1;
		this.cmbStockGroupAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbStockGroupAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbStockGroupAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbStockGroupAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.cmbStockGroupAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbStockGroupAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance131).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance131).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance131).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbStockGroupAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance131;
		((AppearanceBase)appearance132).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbStockGroupAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance132;
		((SpecialBoxBase)((UltraGridBase)this.cmbStockGroupAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance133).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance133).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance133).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbStockGroupAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.cmbStockGroupAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbStockGroupAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance134).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbStockGroupAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance134;
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance135).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbStockGroupAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.cmbStockGroupAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbStockGroupAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbStockGroupAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance137;
		((AppearanceBase)appearance138).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance138).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbStockGroupAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.cmbStockGroupAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbStockGroupAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance139).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance139).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance139).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance139).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbStockGroupAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance139;
		((AppearanceBase)appearance140).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbStockGroupAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.cmbStockGroupAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbStockGroupAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance141).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbStockGroupAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance141;
		((UltraGridBase)this.cmbStockGroupAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbStockGroupAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.cmbStockGroupAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbStockGroupAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbStockGroupAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbStockGroupAccount).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbStockGroupAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbStockGroupAccount).Location = new System.Drawing.Point(355, 2);
		((System.Windows.Forms.Control)(object)this.cmbStockGroupAccount).Name = "cmbStockGroupAccount";
		((System.Windows.Forms.Control)(object)this.cmbStockGroupAccount).Size = new System.Drawing.Size(295, 26);
		((System.Windows.Forms.Control)(object)this.cmbStockGroupAccount).TabIndex = 219;
		this.RdSaleAmount.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.RdSaleAmount.AutoSize = true;
		this.RdSaleAmount.ForeColor = System.Drawing.Color.White;
		this.RdSaleAmount.Location = new System.Drawing.Point(1088, 7);
		this.RdSaleAmount.Name = "RdSaleAmount";
		this.RdSaleAmount.Size = new System.Drawing.Size(76, 17);
		this.RdSaleAmount.TabIndex = 6;
		this.RdSaleAmount.Text = "Sale Value";
		this.RdSaleAmount.UseVisualStyleBackColor = true;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(5, 5);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(209, 19);
		this.label23.TabIndex = 1;
		this.label23.Text = "Stock Report With Value";
		this.ChkItemWiseStock.AutoSize = true;
		this.ChkItemWiseStock.Checked = true;
		this.ChkItemWiseStock.CheckState = System.Windows.Forms.CheckState.Checked;
		this.ChkItemWiseStock.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.ChkItemWiseStock.Location = new System.Drawing.Point(961, 136);
		this.ChkItemWiseStock.Name = "ChkItemWiseStock";
		this.ChkItemWiseStock.Size = new System.Drawing.Size(214, 19);
		this.ChkItemWiseStock.TabIndex = 15;
		this.ChkItemWiseStock.Text = "Packing Type & Pack Size Wise Stock";
		this.ChkItemWiseStock.UseVisualStyleBackColor = true;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label11.Location = new System.Drawing.Point(572, 125);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(70, 15);
		this.label11.TabIndex = 107;
		this.label11.Text = "Report Type";
		this.cmbReportType.AllowNull = (DefaultableBoolean)1;
		this.cmbReportType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbReportType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbReportType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.cmbReportType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbReportType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbReportType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbReportType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance54;
		((SpecialBoxBase)((UltraGridBase)this.cmbReportType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbReportType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.cmbReportType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbReportType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance60).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance61).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.cmbReportType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbReportType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbReportType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbReportType).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbReportType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbReportType).Location = new System.Drawing.Point(660, 119);
		((System.Windows.Forms.Control)(object)this.cmbReportType).Name = "cmbReportType";
		((System.Windows.Forms.Control)(object)this.cmbReportType).Size = new System.Drawing.Size(295, 26);
		((System.Windows.Forms.Control)(object)this.cmbReportType).TabIndex = 14;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label10.Location = new System.Drawing.Point(333, 125);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(54, 15);
		this.label10.TabIndex = 105;
		this.label10.Text = "CropYear";
		this.cmbCropYear.AllowNull = (DefaultableBoolean)1;
		this.cmbCropYear.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCropYear.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCropYear.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Appearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCropYear).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance41;
		((SpecialBoxBase)((UltraGridBase)this.cmbCropYear).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCropYear).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbCropYear.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCropYear).Location = new System.Drawing.Point(411, 119);
		((System.Windows.Forms.Control)(object)this.cmbCropYear).Name = "cmbCropYear";
		((System.Windows.Forms.Control)(object)this.cmbCropYear).Size = new System.Drawing.Size(157, 26);
		((System.Windows.Forms.Control)(object)this.cmbCropYear).TabIndex = 9;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label5.Location = new System.Drawing.Point(333, 153);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(43, 15);
		this.label5.TabIndex = 103;
		this.label5.Text = "JobLot";
		this.cmbJobLot.AllowNull = (DefaultableBoolean)1;
		this.cmbJobLot.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbJobLot.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbJobLot.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance156).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Appearance = (AppearanceBase)(object)appearance156;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance157).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance157).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance157).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbJobLot).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance157;
		((AppearanceBase)appearance159).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance159;
		((SpecialBoxBase)((UltraGridBase)this.cmbJobLot).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance160).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance160).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance160).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance160;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance161).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance161).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance161;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance162).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance162;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance163;
		((AppearanceBase)appearance164).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance164).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance165).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance165).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance165).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance165).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance165;
		((AppearanceBase)appearance166).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance166;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance167).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance167;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbJobLot).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbJobLot.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbJobLot).Location = new System.Drawing.Point(411, 147);
		((System.Windows.Forms.Control)(object)this.cmbJobLot).Name = "cmbJobLot";
		((System.Windows.Forms.Control)(object)this.cmbJobLot).Size = new System.Drawing.Size(157, 26);
		((System.Windows.Forms.Control)(object)this.cmbJobLot).TabIndex = 10;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(3, 125);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(69, 15);
		this.label4.TabIndex = 101;
		this.label4.Text = "Class Group";
		this.CmbClassGroup.AllowNull = (DefaultableBoolean)1;
		this.CmbClassGroup.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbClassGroup.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbClassGroup.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.Appearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance144).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance144).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance144).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbClassGroup).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance144;
		((AppearanceBase)appearance145).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance145;
		((SpecialBoxBase)((UltraGridBase)this.CmbClassGroup).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance146).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance146).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance146).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance148).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance148;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance149).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance149;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance150;
		((AppearanceBase)appearance151).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance151).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance152).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance152).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance152).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance152).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance152;
		((AppearanceBase)appearance153).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance153;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance154).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance154;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbClassGroup).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbClassGroup.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbClassGroup).Location = new System.Drawing.Point(93, 119);
		((System.Windows.Forms.Control)(object)this.CmbClassGroup).Name = "CmbClassGroup";
		((System.Windows.Forms.Control)(object)this.CmbClassGroup).Size = new System.Drawing.Size(236, 26);
		((System.Windows.Forms.Control)(object)this.CmbClassGroup).TabIndex = 4;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label9.Location = new System.Drawing.Point(572, 69);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(67, 15);
		this.label9.TabIndex = 99;
		this.label9.Text = "Item Name";
		this.cmbItem.AllowNull = (DefaultableBoolean)1;
		this.cmbItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.cmbItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance88).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance88).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance88).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance88;
		((AppearanceBase)appearance89).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance89;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance90).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance90).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance91).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance91;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance93).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance93;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance94;
		((AppearanceBase)appearance95).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance95).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance96).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance96).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance96).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance96).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance97).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance98).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance98;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItem).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItem).Location = new System.Drawing.Point(660, 63);
		((System.Windows.Forms.Control)(object)this.cmbItem).Name = "cmbItem";
		((System.Windows.Forms.Control)(object)this.cmbItem).Size = new System.Drawing.Size(295, 26);
		((System.Windows.Forms.Control)(object)this.cmbItem).TabIndex = 12;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label8.Location = new System.Drawing.Point(333, 41);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(63, 15);
		this.label8.TabIndex = 97;
		this.label8.Text = "Item Type ";
		this.cmbItemType.AllowNull = (DefaultableBoolean)1;
		this.cmbItemType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.cmbItemType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance92).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance92).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance92;
		((AppearanceBase)appearance103).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance103;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbItemType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbItemType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemType).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbItemType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemType).Location = new System.Drawing.Point(411, 35);
		((System.Windows.Forms.Control)(object)this.cmbItemType).Name = "cmbItemType";
		((System.Windows.Forms.Control)(object)this.cmbItemType).Size = new System.Drawing.Size(157, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemType).TabIndex = 6;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(3, 97);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(91, 15);
		this.label7.TabIndex = 96;
		this.label7.Text = "Parent Category";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label6.Location = new System.Drawing.Point(572, 41);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(69, 15);
		this.label6.TabIndex = 95;
		this.label6.Text = "WareHouse";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(3, 153);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(82, 15);
		this.label3.TabIndex = 92;
		this.label3.Text = "Item Category";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(3, 69);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(79, 15);
		this.label1.TabIndex = 90;
		this.label1.Text = "Date From,To";
		this.cmbWareHouse.AllowNull = (DefaultableBoolean)1;
		this.cmbWareHouse.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbWareHouse.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbWareHouse.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Appearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance115).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance115).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbWareHouse).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance116).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance116;
		((SpecialBoxBase)((UltraGridBase)this.cmbWareHouse).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance117).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance117).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance117).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance118).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance118;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance119).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance121).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance121).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance122).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance122).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance122).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance122).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance122;
		((AppearanceBase)appearance123).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance124).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbWareHouse).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbWareHouse.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbWareHouse).Location = new System.Drawing.Point(660, 35);
		((System.Windows.Forms.Control)(object)this.cmbWareHouse).Name = "cmbWareHouse";
		((System.Windows.Forms.Control)(object)this.cmbWareHouse).Size = new System.Drawing.Size(295, 26);
		((System.Windows.Forms.Control)(object)this.cmbWareHouse).TabIndex = 11;
		this.cmbCategory.AllowNull = (DefaultableBoolean)1;
		this.cmbCategory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCategory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCategory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbCategory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCategory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance114).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance114).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance114).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCategory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance114;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCategory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance125;
		((SpecialBoxBase)((UltraGridBase)this.cmbCategory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance136).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance136).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance136).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCategory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.cmbCategory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCategory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance147).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance147;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance158).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance158;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance2).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance2;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance24).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance127).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.cmbCategory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCategory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCategory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCategory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbCategory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCategory).Location = new System.Drawing.Point(93, 147);
		((System.Windows.Forms.Control)(object)this.cmbCategory).Name = "cmbCategory";
		((System.Windows.Forms.Control)(object)this.cmbCategory).Size = new System.Drawing.Size(236, 26);
		((System.Windows.Forms.Control)(object)this.cmbCategory).TabIndex = 5;
		this.btnshow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(900, 147);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(55, 26);
		this.btnshow.TabIndex = 18;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.txtDateFrom.CustomFormat = "dd-MMM-yy";
		this.txtDateFrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDateFrom.Location = new System.Drawing.Point(93, 65);
		this.txtDateFrom.Name = "txtDateFrom";
		this.txtDateFrom.Size = new System.Drawing.Size(117, 23);
		this.txtDateFrom.TabIndex = 1;
		this.txtDateTo.CustomFormat = "dd-MMM-yy";
		this.txtDateTo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDateTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDateTo.Location = new System.Drawing.Point(212, 65);
		this.txtDateTo.Name = "txtDateTo";
		this.txtDateTo.Size = new System.Drawing.Size(117, 23);
		this.txtDateTo.TabIndex = 2;
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1168, 27);
		this.panel2.TabIndex = 0;
		this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[6] { this.BtnReset, this.btnRefresh, this.btnPrint, this.btnPrintStockSummeryByWeight, this.btnPrintStockReport, this.BtnUpdate });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1168, 27);
		this.toolStrip1.TabIndex = 1;
		this.toolStrip1.Text = "toolStrip1";
		this.BtnReset.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnReset.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.BtnReset.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnReset.Name = "BtnReset";
		this.BtnReset.Size = new System.Drawing.Size(68, 24);
		this.BtnReset.Text = "&Reset";
		this.BtnReset.Click += new System.EventHandler(BtnReset_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "&Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(56, 24);
		this.btnPrint.Text = "Print";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.btnPrintStockSummeryByWeight.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrintStockSummeryByWeight.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrintStockSummeryByWeight.Name = "btnPrintStockSummeryByWeight";
		this.btnPrintStockSummeryByWeight.Size = new System.Drawing.Size(96, 24);
		this.btnPrintStockSummeryByWeight.Text = "Print-188_01";
		this.btnPrintStockSummeryByWeight.Visible = false;
		this.btnPrintStockSummeryByWeight.Click += new System.EventHandler(btnPrintStockSummeryByWeight_Click);
		this.btnPrintStockReport.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrintStockReport.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrintStockReport.Name = "btnPrintStockReport";
		this.btnPrintStockReport.Size = new System.Drawing.Size(96, 24);
		this.btnPrintStockReport.Text = "Print-188_02";
		this.btnPrintStockReport.Visible = false;
		this.btnPrintStockReport.Click += new System.EventHandler(btnPrintStockReport_Click);
		this.BtnUpdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.BtnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnUpdate.Name = "BtnUpdate";
		this.BtnUpdate.Size = new System.Drawing.Size(78, 24);
		this.BtnUpdate.Text = "&Update";
		this.BtnUpdate.Visible = false;
		this.BtnUpdate.Click += new System.EventHandler(BtnUpdate_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1168, 561);
		base.Controls.Add(this.panel1);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Name = "frmStockReportWithValues";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmStockReportWithValues_FormClosing);
		base.Load += new System.EventHandler(frmStockReport_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmStockReportWithValues_KeyDown);
		this.panel1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdfrm).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbMainType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPacktype).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPackUom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbStockAccounts).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbStockGroupAccount).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbReportType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCropYear).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbJobLot).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbClassGroup).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbWareHouse).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCategory).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		base.ResumeLayout(false);
	}
}
