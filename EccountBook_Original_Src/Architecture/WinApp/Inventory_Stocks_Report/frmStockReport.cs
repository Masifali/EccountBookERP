using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Reports.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Reports;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Inventory_Stocks_Report;

public class frmStockReport : Form
{
	private WaitForm waitForm = new WaitForm();

	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtBindCombo = new DataTable();

	private DataTable dtStockSum = new DataTable();

	public int[] AryColWidth;

	private DataTable dtItemStockSummary = new DataTable();

	private DataTable dtItemandWarehouseStockSummary = new DataTable();

	private DataTable dtWarehouseandItemStockSummary = new DataTable();

	private DataTable dtItemandCropYearStockSummary = new DataTable();

	private DataTable dtItemandCropYearandWarehouseStockSummary = new DataTable();

	private DataTable dtJobLotandItemStockSummary = new DataTable();

	private DataTable dtWarehouseandJoblotandItemStockSummary = new DataTable();

	private DataTable dtItemandPackSizeStockSummary = new DataTable();

	private DataTable dtItemandPackingTypeStockSummary = new DataTable();

	private DataTable dtItemandPackSizeandPackingTypeStockSummary = new DataTable();

	private DataTable dtWIPStockPlantWiseSummary = new DataTable();

	public DateTime DateFrom;

	public DateTime DateTo;

	public string CropYear = "";

	public int PackingTypeId;

	public int ItemId;

	public int WarehouseId;

	public int JobLotId;

	public int UomId;

	public bool RequestedbyOtherForm;

	private DataTable dtMainType = new DataTable();

	private DataTable dtItem = new DataTable();

	private DataTable dtBranch = new DataTable();

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

	internal UltraCombo cmbMainCategory;

	private Label label6;

	private Label label3;

	private Label label1;

	internal UltraCombo cmbWareHouse;

	internal UltraCombo cmbCategory;

	private Button btnshow;

	private Label label9;

	internal UltraCombo cmbItem;

	private Label label4;

	internal UltraCombo cmbUOM;

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

	private Label label12;

	internal UltraCombo cmbPacktype;

	private ToolStripButton btnPrint;

	private CheckBox chkSikeZero;

	private CheckBox chkIncludeWipItem;

	private Label label2;

	internal UltraCombo CmbClassGroup;

	internal UltraCombo cmbBranchName;

	private Label label13;

	private Label label14;

	public DateTimePicker txtDateFrom;

	public DateTimePicker txtDateTo;

	public frmStockReport(UserAccount userAccount)
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
			dtMainType.Columns.Add("Id", typeof(int));
			dtMainType.Columns.Add("name");
			dtItem.Columns.Add("Id", typeof(int));
			dtItem.Columns.Add("name");
			bool StockByItemWarehouse = false;
			bool StockByItemWareHouseCropYear = false;
			bool StockByItemWareHouseCropYearJobLot = false;
			bool StockByItemWareHouseCropYearJobLotPackUom = false;
			bool StockByItemWareHouseCropYearJobLotPackingTypePackUom = false;
			bool FifoFeature = CommonServices.GetERPFeatureById(5);
			if (!FifoFeature && RequestedbyOtherForm)
			{
				StockByItemWarehouse = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("CheckStockItemWareHouse"));
				StockByItemWareHouseCropYear = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("CheckStockItemWareHouseCropYear"));
				StockByItemWareHouseCropYearJobLot = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("CheckStockItemWareHouseCropYearJobLot"));
				StockByItemWareHouseCropYearJobLotPackUom = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("CheckStockItemWareHouseCropYearJobLotPackUom"));
				StockByItemWareHouseCropYearJobLotPackingTypePackUom = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("CheckStockItemWareHouseCropYearJobLotPackingTypeUom"));
			}
			dtItemStockSummary.Columns.Add("ParentCategory");
			dtItemStockSummary.Columns.Add("ItemId");
			dtItemStockSummary.Columns.Add("ItemName");
			dtItemStockSummary.Columns.Add("PackUom");
			dtItemStockSummary.Columns.Add("PackingType");
			dtItemStockSummary.Columns.Add("TotalQty", typeof(double));
			dtItemStockSummary.Columns.Add("OpWeight", typeof(double));
			dtItemStockSummary.Columns.Add("WeightIn", typeof(double));
			dtItemStockSummary.Columns.Add("WeightOut", typeof(double));
			dtItemStockSummary.Columns.Add("BalWeight", typeof(double));
			dtItemandWarehouseStockSummary.Columns.Add("ParentCategory");
			dtItemandWarehouseStockSummary.Columns.Add("ItemId");
			dtItemandWarehouseStockSummary.Columns.Add("ItemName");
			dtItemandWarehouseStockSummary.Columns.Add("WareHouseName");
			dtItemandWarehouseStockSummary.Columns.Add("PackUom");
			dtItemandWarehouseStockSummary.Columns.Add("PackingType");
			dtItemandWarehouseStockSummary.Columns.Add("TotalQty", typeof(double));
			dtItemandWarehouseStockSummary.Columns.Add("OpWeight", typeof(double));
			dtItemandWarehouseStockSummary.Columns.Add("WeightIn", typeof(double));
			dtItemandWarehouseStockSummary.Columns.Add("WeightOut", typeof(double));
			dtItemandWarehouseStockSummary.Columns.Add("BalWeight", typeof(double));
			dtWarehouseandItemStockSummary.Columns.Add("ParentCategory");
			dtWarehouseandItemStockSummary.Columns.Add("ItemId");
			dtWarehouseandItemStockSummary.Columns.Add("ItemName");
			dtWarehouseandItemStockSummary.Columns.Add("WareHouseName");
			dtWarehouseandItemStockSummary.Columns.Add("PackUom");
			dtWarehouseandItemStockSummary.Columns.Add("PackingType");
			dtWarehouseandItemStockSummary.Columns.Add("TotalQty", typeof(double));
			dtWarehouseandItemStockSummary.Columns.Add("OpWeight", typeof(double));
			dtWarehouseandItemStockSummary.Columns.Add("WeightIn", typeof(double));
			dtWarehouseandItemStockSummary.Columns.Add("WeightOut", typeof(double));
			dtWarehouseandItemStockSummary.Columns.Add("BalWeight", typeof(double));
			dtItemandCropYearStockSummary.Columns.Add("ParentCategory");
			dtItemandCropYearStockSummary.Columns.Add("ItemId");
			dtItemandCropYearStockSummary.Columns.Add("ItemName");
			dtItemandCropYearStockSummary.Columns.Add("CropYear");
			dtItemandCropYearStockSummary.Columns.Add("PackUom");
			dtItemandCropYearStockSummary.Columns.Add("PackingType");
			dtItemandCropYearStockSummary.Columns.Add("TotalQty", typeof(double));
			dtItemandCropYearStockSummary.Columns.Add("OpWeight", typeof(double));
			dtItemandCropYearStockSummary.Columns.Add("WeightIn", typeof(double));
			dtItemandCropYearStockSummary.Columns.Add("WeightOut", typeof(double));
			dtItemandCropYearStockSummary.Columns.Add("BalWeight", typeof(double));
			dtItemandCropYearandWarehouseStockSummary.Columns.Add("ParentCategory");
			dtItemandCropYearandWarehouseStockSummary.Columns.Add("ItemId");
			dtItemandCropYearandWarehouseStockSummary.Columns.Add("ItemName");
			dtItemandCropYearandWarehouseStockSummary.Columns.Add("WareHouseName");
			dtItemandCropYearandWarehouseStockSummary.Columns.Add("CropYear");
			dtItemandCropYearandWarehouseStockSummary.Columns.Add("PackUom");
			dtItemandCropYearandWarehouseStockSummary.Columns.Add("PackingType");
			dtItemandCropYearandWarehouseStockSummary.Columns.Add("TotalQty", typeof(double));
			dtItemandCropYearandWarehouseStockSummary.Columns.Add("OpWeight", typeof(double));
			dtItemandCropYearandWarehouseStockSummary.Columns.Add("WeightIn", typeof(double));
			dtItemandCropYearandWarehouseStockSummary.Columns.Add("WeightOut", typeof(double));
			dtItemandCropYearandWarehouseStockSummary.Columns.Add("BalWeight", typeof(double));
			dtJobLotandItemStockSummary.Columns.Add("ParentCategory");
			dtJobLotandItemStockSummary.Columns.Add("ItemId");
			dtJobLotandItemStockSummary.Columns.Add("ItemName");
			dtJobLotandItemStockSummary.Columns.Add("JobLot");
			dtJobLotandItemStockSummary.Columns.Add("PackUom");
			dtJobLotandItemStockSummary.Columns.Add("PackingType");
			dtJobLotandItemStockSummary.Columns.Add("TotalQty", typeof(double));
			dtJobLotandItemStockSummary.Columns.Add("OpWeight", typeof(double));
			dtJobLotandItemStockSummary.Columns.Add("WeightIn", typeof(double));
			dtJobLotandItemStockSummary.Columns.Add("WeightOut", typeof(double));
			dtJobLotandItemStockSummary.Columns.Add("BalWeight", typeof(double));
			dtWarehouseandJoblotandItemStockSummary.Columns.Add("ParentCategory");
			dtWarehouseandJoblotandItemStockSummary.Columns.Add("ItemId");
			dtWarehouseandJoblotandItemStockSummary.Columns.Add("ItemName");
			dtWarehouseandJoblotandItemStockSummary.Columns.Add("WareHouseName");
			dtWarehouseandJoblotandItemStockSummary.Columns.Add("JobLot");
			dtWarehouseandJoblotandItemStockSummary.Columns.Add("PackUom");
			dtWarehouseandJoblotandItemStockSummary.Columns.Add("PackingType");
			dtWarehouseandJoblotandItemStockSummary.Columns.Add("TotalQty", typeof(double));
			dtWarehouseandJoblotandItemStockSummary.Columns.Add("OpWeight", typeof(double));
			dtWarehouseandJoblotandItemStockSummary.Columns.Add("WeightIn", typeof(double));
			dtWarehouseandJoblotandItemStockSummary.Columns.Add("WeightOut", typeof(double));
			dtWarehouseandJoblotandItemStockSummary.Columns.Add("BalWeight", typeof(double));
			dtItemandPackSizeStockSummary.Columns.Add("ParentCategory");
			dtItemandPackSizeStockSummary.Columns.Add("ItemId");
			dtItemandPackSizeStockSummary.Columns.Add("ItemName");
			dtItemandPackSizeStockSummary.Columns.Add("PackUom");
			dtItemandPackSizeStockSummary.Columns.Add("TotalQty", typeof(double));
			dtItemandPackSizeStockSummary.Columns.Add("OpWeight", typeof(double));
			dtItemandPackSizeStockSummary.Columns.Add("WeightIn", typeof(double));
			dtItemandPackSizeStockSummary.Columns.Add("WeightOut", typeof(double));
			dtItemandPackSizeStockSummary.Columns.Add("BalWeight", typeof(double));
			dtItemandPackingTypeStockSummary.Columns.Add("ParentCategory");
			dtItemandPackingTypeStockSummary.Columns.Add("ItemId");
			dtItemandPackingTypeStockSummary.Columns.Add("ItemName");
			dtItemandPackingTypeStockSummary.Columns.Add("PackingType");
			dtItemandPackingTypeStockSummary.Columns.Add("TotalQty", typeof(double));
			dtItemandPackingTypeStockSummary.Columns.Add("OpWeight", typeof(double));
			dtItemandPackingTypeStockSummary.Columns.Add("WeightIn", typeof(double));
			dtItemandPackingTypeStockSummary.Columns.Add("WeightOut", typeof(double));
			dtItemandPackingTypeStockSummary.Columns.Add("BalWeight", typeof(double));
			dtItemandPackSizeandPackingTypeStockSummary.Columns.Add("ParentCategory");
			dtItemandPackSizeandPackingTypeStockSummary.Columns.Add("ItemId");
			dtItemandPackSizeandPackingTypeStockSummary.Columns.Add("ItemName");
			dtItemandPackSizeandPackingTypeStockSummary.Columns.Add("PackUom");
			dtItemandPackSizeandPackingTypeStockSummary.Columns.Add("PackingType");
			dtItemandPackSizeandPackingTypeStockSummary.Columns.Add("TotalQty", typeof(double));
			dtItemandPackSizeandPackingTypeStockSummary.Columns.Add("OpWeight", typeof(double));
			dtItemandPackSizeandPackingTypeStockSummary.Columns.Add("WeightIn", typeof(double));
			dtItemandPackSizeandPackingTypeStockSummary.Columns.Add("WeightOut", typeof(double));
			dtItemandPackSizeandPackingTypeStockSummary.Columns.Add("BalWeight", typeof(double));
			dtWIPStockPlantWiseSummary.Columns.Add("ParentCategory");
			dtWIPStockPlantWiseSummary.Columns.Add("ItemId");
			dtWIPStockPlantWiseSummary.Columns.Add("PlantName");
			dtWIPStockPlantWiseSummary.Columns.Add("ItemName");
			dtWIPStockPlantWiseSummary.Columns.Add("TotalQty", typeof(double));
			dtWIPStockPlantWiseSummary.Columns.Add("OpWeight", typeof(double));
			dtWIPStockPlantWiseSummary.Columns.Add("WeightIn", typeof(double));
			dtWIPStockPlantWiseSummary.Columns.Add("WeightOut", typeof(double));
			dtWIPStockPlantWiseSummary.Columns.Add("BalWeight", typeof(double));
			if (Conversion.CheckDateTimeNull(DateTo))
			{
				txtDateFrom.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
				txtDateFrom.Focus();
			}
			else
			{
				txtDateFrom.Value = DateTime.Now.AddMonths(-1);
				txtDateTo.Value = DateTo;
			}
			DateTime AsOnDate = StocksReport.GetInventoryStockAsOnDate(UserAccount.OrganizationId, UserAccount.CompanyId);
			if (!Conversion.CheckDateTimeNull(AsOnDate))
			{
				txtDateFrom.Value = AsOnDate;
			}
			ReportTypeFill();
			waitForm.Show(this);
			BranchesFill();
			ParentCategoryFill();
			StockComboFill();
			if (!FifoFeature && RequestedbyOtherForm)
			{
				if (StockByItemWarehouse)
				{
					cmbItem.Value = ItemId;
					cmbWareHouse.Value = WarehouseId;
				}
				else if (StockByItemWareHouseCropYear)
				{
					cmbItem.Value = ItemId;
					cmbWareHouse.Value = WarehouseId;
					((Control)(object)cmbCropYear).Text = CropYear;
				}
				else if (StockByItemWareHouseCropYearJobLot)
				{
					cmbItem.Value = ItemId;
					cmbWareHouse.Value = WarehouseId;
					((Control)(object)cmbCropYear).Text = CropYear;
					cmbJobLot.Value = JobLotId;
				}
				else if (StockByItemWareHouseCropYearJobLotPackUom)
				{
					cmbItem.Value = ItemId;
					cmbItem_Leave(null, null);
					cmbWareHouse.Value = WarehouseId;
					((Control)(object)cmbCropYear).Text = CropYear;
					cmbJobLot.Value = JobLotId;
					cmbUOM.Value = UomId;
				}
				else if (StockByItemWareHouseCropYearJobLotPackingTypePackUom)
				{
					cmbItem.Value = ItemId;
					cmbItem_Leave(null, null);
					cmbWareHouse.Value = WarehouseId;
					((Control)(object)cmbCropYear).Text = CropYear;
					cmbJobLot.Value = JobLotId;
					cmbUOM.Value = UomId;
					cmbPacktype.Value = PackingTypeId;
				}
			}
			else if (FifoFeature && RequestedbyOtherForm)
			{
				cmbItem.Value = ItemId;
				cmbItem_Leave(null, null);
				cmbWareHouse.Value = WarehouseId;
				((Control)(object)cmbCropYear).Text = CropYear;
				cmbJobLot.Value = JobLotId;
				cmbUOM.Value = UomId;
				cmbPacktype.Value = PackingTypeId;
			}
			((UltraGridBase)cmbReportType).Rows[0].Activate();
			if (((Control)(object)cmbReportType).Text == Conversion.ToString((object)"Item Stock Summary"))
			{
				GridBind();
			}
			PrintButtonManage();
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

	private void ParentCategoryFill()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		try
		{
			((UltraGridBase)cmbMainCategory).DataSource = null;
			((Control)(object)cmbMainCategory).Text = "";
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.Ids = "1,2,3,4,6";
			dtBindCombo = StocksReport.StockReportComboByOrganizationAndCompanyId(obj);
			if (dtBindCombo.Rows.Count > 0)
			{
				for (int i = 0; i < dtBindCombo.Rows.Count; i++)
				{
					if (Conversion.ToString(dtBindCombo.Rows[i]["ActivityType"]) == "ParentCategories")
					{
						dtMainType.Rows.Add(dtBindCombo.Rows[i]["Id"], dtBindCombo.Rows[i]["name"]);
					}
				}
			}
			DDL.BindDDL(dtMainType, cmbMainCategory, "Id", "name", "Parent Category", ZeroIndex: false);
			((UltraGridBase)cmbMainCategory).DisplayLayout.Bands[0].Columns[1].Width = 227;
			UltraGridColumn obj2 = ((UltraGridBase)cmbMainCategory).DisplayLayout.Bands[0].Columns.Add();
			((KeyedSubObjectBase)obj2).Key = "Selected";
			((HeaderBase)obj2.Header).Caption = string.Empty;
			obj2.Header.CheckBoxVisibility = (HeaderCheckBoxVisibility)2;
			obj2.DataType = typeof(bool);
			((HeaderBase)obj2.Header).VisiblePosition = 0;
			cmbMainCategory.CheckedListSettings.CheckStateMember = "Selected";
			((EditorCheckedListSettings)cmbMainCategory.CheckedListSettings).EditorValueSource = (EditorWithComboValueSource)1;
			((EditorCheckedListSettings)cmbMainCategory.CheckedListSettings).ListSeparator = ",";
			((EditorCheckedListSettings)cmbMainCategory.CheckedListSettings).ItemCheckArea = (ItemCheckArea)1;
			RowEnumerator enumerator = ((UltraGridBase)cmbMainCategory).Rows.GetEnumerator();
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
			throw new Exception(ex.Message);
		}
	}

	private void StockComboFill()
	{
		try
		{
			DataTable dtFiltered = new DataTable();
			HashSet<string> validIds = new HashSet<string>();
			if (!string.IsNullOrWhiteSpace(((Control)(object)cmbMainCategory).Text))
			{
				string[] array = ((Control)(object)cmbMainCategory).Text.Trim().Split(',');
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
					where validIds.Contains(row["ParentCategoryId"].ToString())
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
					"Warehouse",
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
				},
				{
					"ItemClassGroup",
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
			DDL.BindDDL(activityTables["JobLot"], cmbJobLot, "Id", "name", "JobLot", ZeroIndex: false);
			DDL.BindDDL(activityTables["CropYear"], cmbCropYear, "Id", "name", "CropYear", ZeroIndex: false);
			DDL.BindDDL(activityTables["Warehouse"], cmbWareHouse, "Id", "name", "WarehouseName", ZeroIndex: false);
			DDL.BindDDL(activityTables["PackingType"], cmbPacktype, "Id", "name", "PackingType", ZeroIndex: false);
			DDL.BindDDL(activityTables["ItemClassGroup"], CmbClassGroup, "Id", "name", "ItemClassGroup", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
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
			DDL.BindDDL(dt, cmbUOM, "Id", "PackUom", "PackUom", ZeroIndex: false);
			((UltraGridBase)cmbUOM).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
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
			dtreporttype.Rows.Add("ItemandPackSizeStockSummary", "Item and PackSize Stock Summary");
			dtreporttype.Rows.Add("ItemandPackingTypeStockSummary", "Item and PackingType Stock Summary");
			dtreporttype.Rows.Add("ItemandPackSizeandPackingTypeStockSummary", "Item and PackSize and PackingType Stock Summary");
			dtreporttype.Rows.Add("WIPStockPlantWiseSummary", "WIP Stock Plant Wise Summary");
			DDL.BindDDL(dtreporttype, cmbReportType, "Id", "name", "Report Type", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbItem_Leave(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(cmbItem.Value) > 0)
			{
				DataTable dt = StocksReport.getPackUomByItemIdFromInventoryTransactions(UserAccount.OrganizationId, UserAccount.CompanyId, Conversion.ToInt(cmbItem.Value));
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, cmbUOM, "Id", "PackUom", "Pack Uom", ZeroIndex: false);
					return;
				}
				((Control)(object)cmbUOM).Text = string.Empty;
				((UltraGridBase)cmbUOM).DataSource = null;
			}
			else
			{
				((Control)(object)cmbUOM).Text = string.Empty;
				((UltraGridBase)cmbUOM).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridBind()
	{
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Expected O, but got Unknown
		try
		{
			string Ids = "";
			if (((Control)(object)cmbMainCategory).Text != null && ((Control)(object)cmbMainCategory).Text != string.Empty)
			{
				string[] array = (Conversion.ToString((object)((Control)(object)cmbMainCategory).Text.Trim()) + ",").Split(',');
				foreach (string author2 in array)
				{
					DataRow[] dr2 = dtMainType.Select("name='" + Conversion.ToString((object)author2) + "'");
					if (dr2.Length != 0)
					{
						Ids = Ids + Conversion.ToString(dr2[0].ItemArray[0]) + ",";
					}
				}
			}
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.FromDate = Conversion.ToDateTime((object)txtDateFrom.Value);
			obj.ToDate = Conversion.ToDateTime((object)txtDateTo.Value);
			obj.ItemClassGroupId = Conversion.ToInt(CmbClassGroup.Value);
			obj.Ids = Ids;
			obj.ItemCategoryId = Conversion.ToInt(cmbCategory.Value);
			obj.ItemTypeId = Conversion.ToInt(cmbItemType.Value);
			obj.ItemId = Conversion.ToInt(cmbItem.Value);
			obj.CropYear = Conversion.ToString((object)((Control)(object)cmbCropYear).Text);
			obj.WarehouseId = Conversion.ToInt(cmbWareHouse.Value);
			obj.PackingTypeId = Conversion.ToInt(cmbPacktype.Value);
			obj.JobLotId = Conversion.ToInt(cmbJobLot.Value);
			obj.ItemUomId = Conversion.ToInt(cmbUOM.Value);
			obj.Activity = Conversion.ToString(cmbReportType.Value);
			if (ChkItemWiseStock.Checked)
			{
				obj.stockUOM = 0;
				obj.RateUOM = 0;
			}
			else
			{
				obj.stockUOM = 1;
				obj.RateUOM = 1;
			}
			if (chkSikeZero.Checked)
			{
				obj.ZeroBalanceType = 1;
			}
			if (chkIncludeWipItem.Checked)
			{
				obj.AllowWipItem = 1;
			}
			string BranchIds = "";
			if (Conversion.ToString((object)((Control)(object)cmbBranchName).Text) != string.Empty)
			{
				string[] array = (Conversion.ToString((object)((Control)(object)cmbBranchName).Text) + ",").Split(',');
				foreach (string author in array)
				{
					DataRow[] dr = dtBranch.Select("BranchName='" + Conversion.ToString((object)author) + "'");
					if (dr.Length != 0)
					{
						BranchIds = BranchIds + "," + Conversion.ToString(dr[0].ItemArray[0]);
					}
				}
				obj.BranchesIds = BranchIds;
				dtStockSum = StocksReport.stockGeneralSummaryByWeight(obj);
				if (dtStockSum.Rows.Count > 0)
				{
					if (Conversion.ToString(cmbReportType.Value) == "ItemStockSummary")
					{
						dtItemStockSummary.Clear();
						for (int i6 = 0; i6 < dtStockSum.Rows.Count; i6++)
						{
							dtItemStockSummary.Rows.Add(dtStockSum.Rows[i6]["CategoryDescription"], dtStockSum.Rows[i6]["ItemId"], dtStockSum.Rows[i6]["ItemName"], dtStockSum.Rows[i6]["PackUom"], dtStockSum.Rows[i6]["PackingType"], Conversion.ToDouble(dtStockSum.Rows[i6]["TotalQty"]), Conversion.ToDouble(dtStockSum.Rows[i6]["OpWeight"]), Conversion.ToDouble(dtStockSum.Rows[i6]["WeightIn"]), Conversion.ToDouble(dtStockSum.Rows[i6]["WeightOut"]), Conversion.ToDouble(dtStockSum.Rows[i6]["BalWeight"]));
						}
						grdfrm.DataSource = dtItemStockSummary;
						grdfrm.RetrieveStructure();
						grdfrm.RootTable.Columns["PackUom"].Width = 70;
						grdfrm.RootTable.Columns["PackingType"].Width = 110;
						GridSettings();
					}
					if (Conversion.ToString(cmbReportType.Value) == "ItemandWarehouseStockSummary")
					{
						dtItemandWarehouseStockSummary.Clear();
						for (int i5 = 0; i5 < dtStockSum.Rows.Count; i5++)
						{
							dtItemandWarehouseStockSummary.Rows.Add(dtStockSum.Rows[i5]["CategoryDescription"], dtStockSum.Rows[i5]["ItemId"], dtStockSum.Rows[i5]["ItemName"], dtStockSum.Rows[i5]["WareHouseName"], dtStockSum.Rows[i5]["PackUom"], dtStockSum.Rows[i5]["PackingType"], Conversion.ToDouble(dtStockSum.Rows[i5]["TotalQty"]), Conversion.ToDouble(dtStockSum.Rows[i5]["OpWeight"]), Conversion.ToDouble(dtStockSum.Rows[i5]["WeightIn"]), Conversion.ToDouble(dtStockSum.Rows[i5]["WeightOut"]), Conversion.ToDouble(dtStockSum.Rows[i5]["BalWeight"]));
						}
						grdfrm.DataSource = dtItemandWarehouseStockSummary;
						grdfrm.RetrieveStructure();
						grdfrm.RootTable.Columns["WareHouseName"].Width = 120;
						grdfrm.RootTable.Columns["PackUom"].Width = 70;
						grdfrm.RootTable.Columns["PackingType"].Width = 110;
						GridSettings();
					}
					if (Conversion.ToString(cmbReportType.Value) == "ItemandWarehouseStockSummary")
					{
						dtWarehouseandItemStockSummary.Clear();
						for (int i4 = 0; i4 < dtStockSum.Rows.Count; i4++)
						{
							dtWarehouseandItemStockSummary.Rows.Add(dtStockSum.Rows[i4]["CategoryDescription"], dtStockSum.Rows[i4]["ItemId"], dtStockSum.Rows[i4]["ItemName"], dtStockSum.Rows[i4]["WareHouseName"], dtStockSum.Rows[i4]["PackUom"], dtStockSum.Rows[i4]["PackingType"], Conversion.ToDouble(dtStockSum.Rows[i4]["TotalQty"]), Conversion.ToDouble(dtStockSum.Rows[i4]["OpWeight"]), Conversion.ToDouble(dtStockSum.Rows[i4]["WeightIn"]), Conversion.ToDouble(dtStockSum.Rows[i4]["WeightOut"]), Conversion.ToDouble(dtStockSum.Rows[i4]["BalWeight"]));
						}
						grdfrm.DataSource = dtWarehouseandItemStockSummary;
						grdfrm.RetrieveStructure();
						grdfrm.RootTable.Columns["WareHouseName"].Width = 120;
						grdfrm.RootTable.Columns["PackUom"].Width = 70;
						grdfrm.RootTable.Columns["PackingType"].Width = 110;
						GridSettings();
					}
					if (Conversion.ToString(cmbReportType.Value) == "ItemandCropYearStockSummary")
					{
						dtItemandCropYearStockSummary.Clear();
						for (int i3 = 0; i3 < dtStockSum.Rows.Count; i3++)
						{
							dtItemandCropYearStockSummary.Rows.Add(dtStockSum.Rows[i3]["CategoryDescription"], dtStockSum.Rows[i3]["ItemId"], dtStockSum.Rows[i3]["ItemName"], dtStockSum.Rows[i3]["CropYear"], dtStockSum.Rows[i3]["PackUom"], dtStockSum.Rows[i3]["PackingType"], Conversion.ToDouble(dtStockSum.Rows[i3]["TotalQty"]), Conversion.ToDouble(dtStockSum.Rows[i3]["OpWeight"]), Conversion.ToDouble(dtStockSum.Rows[i3]["WeightIn"]), Conversion.ToDouble(dtStockSum.Rows[i3]["WeightOut"]), Conversion.ToDouble(dtStockSum.Rows[i3]["BalWeight"]));
						}
						grdfrm.DataSource = dtItemandCropYearStockSummary;
						grdfrm.RetrieveStructure();
						grdfrm.RootTable.Columns["CropYear"].Width = 80;
						grdfrm.RootTable.Columns["PackUom"].Width = 70;
						grdfrm.RootTable.Columns["PackingType"].Width = 110;
						GridSettings();
					}
					if (Conversion.ToString(cmbReportType.Value) == "ItemandCropYearandWarehouseStockSummary")
					{
						dtItemandCropYearandWarehouseStockSummary.Clear();
						for (int i2 = 0; i2 < dtStockSum.Rows.Count; i2++)
						{
							dtItemandCropYearandWarehouseStockSummary.Rows.Add(dtStockSum.Rows[i2]["CategoryDescription"], dtStockSum.Rows[i2]["ItemId"], dtStockSum.Rows[i2]["ItemName"], dtStockSum.Rows[i2]["WareHouseName"], dtStockSum.Rows[i2]["CropYear"], dtStockSum.Rows[i2]["PackUom"], dtStockSum.Rows[i2]["PackingType"], Conversion.ToDouble(dtStockSum.Rows[i2]["TotalQty"]), Conversion.ToDouble(dtStockSum.Rows[i2]["OpWeight"]), Conversion.ToDouble(dtStockSum.Rows[i2]["WeightIn"]), Conversion.ToDouble(dtStockSum.Rows[i2]["WeightOut"]), Conversion.ToDouble(dtStockSum.Rows[i2]["BalWeight"]));
						}
						grdfrm.DataSource = dtItemandCropYearandWarehouseStockSummary;
						grdfrm.RetrieveStructure();
						grdfrm.RootTable.Columns["WareHouseName"].Width = 120;
						grdfrm.RootTable.Columns["CropYear"].Width = 80;
						grdfrm.RootTable.Columns["PackUom"].Width = 70;
						grdfrm.RootTable.Columns["PackingType"].Width = 110;
						GridSettings();
					}
					if (Conversion.ToString(cmbReportType.Value) == "JobLotandItemStockSummary")
					{
						dtJobLotandItemStockSummary.Clear();
						for (int n = 0; n < dtStockSum.Rows.Count; n++)
						{
							dtJobLotandItemStockSummary.Rows.Add(dtStockSum.Rows[n]["CategoryDescription"], dtStockSum.Rows[n]["ItemId"], dtStockSum.Rows[n]["ItemName"], dtStockSum.Rows[n]["JobLot"], dtStockSum.Rows[n]["PackUom"], dtStockSum.Rows[n]["PackingType"], Conversion.ToDouble(dtStockSum.Rows[n]["TotalQty"]), Conversion.ToDouble(dtStockSum.Rows[n]["OpWeight"]), Conversion.ToDouble(dtStockSum.Rows[n]["WeightIn"]), Conversion.ToDouble(dtStockSum.Rows[n]["WeightOut"]), Conversion.ToDouble(dtStockSum.Rows[n]["BalWeight"]));
						}
						grdfrm.DataSource = dtJobLotandItemStockSummary;
						grdfrm.RetrieveStructure();
						grdfrm.RootTable.Columns["JobLot"].Width = 110;
						grdfrm.RootTable.Columns["PackUom"].Width = 70;
						grdfrm.RootTable.Columns["PackingType"].Width = 110;
						GridSettings();
					}
					if (Conversion.ToString(cmbReportType.Value) == "WarehouseandJoblotandItemStockSummary")
					{
						dtWarehouseandJoblotandItemStockSummary.Clear();
						for (int m = 0; m < dtStockSum.Rows.Count; m++)
						{
							dtWarehouseandJoblotandItemStockSummary.Rows.Add(dtStockSum.Rows[m]["CategoryDescription"], dtStockSum.Rows[m]["ItemId"], dtStockSum.Rows[m]["ItemName"], dtStockSum.Rows[m]["WareHouseName"], dtStockSum.Rows[m]["JobLot"], dtStockSum.Rows[m]["PackUom"], dtStockSum.Rows[m]["PackingType"], Conversion.ToDouble(dtStockSum.Rows[m]["TotalQty"]), Conversion.ToDouble(dtStockSum.Rows[m]["OpWeight"]), Conversion.ToDouble(dtStockSum.Rows[m]["WeightIn"]), Conversion.ToDouble(dtStockSum.Rows[m]["WeightOut"]), Conversion.ToDouble(dtStockSum.Rows[m]["BalWeight"]));
						}
						grdfrm.DataSource = dtWarehouseandJoblotandItemStockSummary;
						grdfrm.RetrieveStructure();
						grdfrm.RootTable.Columns["WareHouseName"].Width = 120;
						grdfrm.RootTable.Columns["JobLot"].Width = 100;
						grdfrm.RootTable.Columns["PackUom"].Width = 70;
						grdfrm.RootTable.Columns["PackingType"].Width = 110;
						GridSettings();
					}
					if (Conversion.ToString(cmbReportType.Value) == "ItemandPackSizeStockSummary")
					{
						dtItemandPackSizeStockSummary.Clear();
						for (int l = 0; l < dtStockSum.Rows.Count; l++)
						{
							dtItemandPackSizeStockSummary.Rows.Add(dtStockSum.Rows[l]["CategoryDescription"], dtStockSum.Rows[l]["ItemId"], dtStockSum.Rows[l]["ItemName"], dtStockSum.Rows[l]["PackUom"], Conversion.ToDouble(dtStockSum.Rows[l]["TotalQty"]), Conversion.ToDouble(dtStockSum.Rows[l]["OpWeight"]), Conversion.ToDouble(dtStockSum.Rows[l]["WeightIn"]), Conversion.ToDouble(dtStockSum.Rows[l]["WeightOut"]), Conversion.ToDouble(dtStockSum.Rows[l]["BalWeight"]));
						}
						grdfrm.DataSource = dtItemandPackSizeStockSummary;
						grdfrm.RetrieveStructure();
						grdfrm.RootTable.Columns["PackUom"].Width = 70;
						GridSettings();
					}
					if (Conversion.ToString(cmbReportType.Value) == "ItemandPackingTypeStockSummary")
					{
						dtItemandPackingTypeStockSummary.Clear();
						for (int k = 0; k < dtStockSum.Rows.Count; k++)
						{
							dtItemandPackingTypeStockSummary.Rows.Add(dtStockSum.Rows[k]["CategoryDescription"], dtStockSum.Rows[k]["ItemId"], dtStockSum.Rows[k]["ItemName"], dtStockSum.Rows[k]["PackingType"], Conversion.ToDouble(dtStockSum.Rows[k]["TotalQty"]), Conversion.ToDouble(dtStockSum.Rows[k]["OpWeight"]), Conversion.ToDouble(dtStockSum.Rows[k]["WeightIn"]), Conversion.ToDouble(dtStockSum.Rows[k]["WeightOut"]), Conversion.ToDouble(dtStockSum.Rows[k]["BalWeight"]));
						}
						grdfrm.DataSource = dtItemandPackingTypeStockSummary;
						grdfrm.RetrieveStructure();
						grdfrm.RootTable.Columns["PackingType"].Width = 110;
						GridSettings();
					}
					if (Conversion.ToString(cmbReportType.Value) == "ItemandPackSizeandPackingTypeStockSummary")
					{
						dtItemandPackSizeandPackingTypeStockSummary.Clear();
						for (int j = 0; j < dtStockSum.Rows.Count; j++)
						{
							dtItemandPackSizeandPackingTypeStockSummary.Rows.Add(dtStockSum.Rows[j]["CategoryDescription"], dtStockSum.Rows[j]["ItemId"], dtStockSum.Rows[j]["ItemName"], dtStockSum.Rows[j]["PackUom"], dtStockSum.Rows[j]["PackingType"], Conversion.ToDouble(dtStockSum.Rows[j]["TotalQty"]), Conversion.ToDouble(dtStockSum.Rows[j]["OpWeight"]), Conversion.ToDouble(dtStockSum.Rows[j]["WeightIn"]), Conversion.ToDouble(dtStockSum.Rows[j]["WeightOut"]), Conversion.ToDouble(dtStockSum.Rows[j]["BalWeight"]));
						}
						grdfrm.DataSource = dtItemandPackSizeandPackingTypeStockSummary;
						grdfrm.RetrieveStructure();
						grdfrm.RootTable.Columns["PackUom"].Width = 70;
						grdfrm.RootTable.Columns["PackingType"].Width = 110;
						GridSettings();
					}
					if (Conversion.ToString(cmbReportType.Value) == "WIPStockPlantWiseSummary")
					{
						dtWIPStockPlantWiseSummary.Clear();
						for (int i = 0; i < dtStockSum.Rows.Count; i++)
						{
							dtWIPStockPlantWiseSummary.Rows.Add(dtStockSum.Rows[i]["CategoryDescription"], dtStockSum.Rows[i]["ItemId"], dtStockSum.Rows[i]["WareHouseName"], dtStockSum.Rows[i]["ItemName"], Conversion.ToDouble(dtStockSum.Rows[i]["TotalQty"]), Conversion.ToDouble(dtStockSum.Rows[i]["OpWeight"]), Conversion.ToDouble(dtStockSum.Rows[i]["WeightIn"]), Conversion.ToDouble(dtStockSum.Rows[i]["WeightOut"]), Conversion.ToDouble(dtStockSum.Rows[i]["BalWeight"]));
						}
						grdfrm.DataSource = dtWIPStockPlantWiseSummary;
						grdfrm.RetrieveStructure();
						grdfrm.RootTable.Columns["WareHouseName"].Width = 120;
						GridSettings();
					}
					PrintButtonManage();
				}
				else
				{
					grdfrm.ClearStructure();
				}
				return;
			}
			cmbBranchName.Focus();
			throw new Exception("Select branch first");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GridSettings()
	{
		try
		{
			grdfrm.RootTable.Columns["ItemId"].Visible = false;
			grdfrm.RootTable.Columns["ParentCategory"].Width = 150;
			grdfrm.RootTable.Columns["ItemName"].Width = 150;
			grdfrm.RootTable.Columns["TotalQty"].Width = 85;
			grdfrm.RootTable.Columns["OpWeight"].Width = 90;
			grdfrm.RootTable.Columns["WeightIn"].Width = 90;
			grdfrm.RootTable.Columns["WeightOut"].Width = 90;
			grdfrm.RootTable.Columns["BalWeight"].Width = 95;
			grdfrm.RootTable.Columns["OpWeight"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["OpWeight"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["OpWeight"].FormatString = "#,#;(#,#);00";
			grdfrm.RootTable.Columns["OpWeight"].TotalFormatString = "#,#;(#,#);0";
			grdfrm.RootTable.Columns["WeightIn"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["WeightIn"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["WeightIn"].FormatString = "#,#;(#,#);00";
			grdfrm.RootTable.Columns["WeightIn"].TotalFormatString = "#,#;(#,#);0";
			grdfrm.RootTable.Columns["TotalQty"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["TotalQty"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["TotalQty"].FormatString = "#,#;(#,#);00";
			grdfrm.RootTable.Columns["TotalQty"].TotalFormatString = "#,#;(#,#);0";
			grdfrm.RootTable.Columns["WeightOut"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["WeightOut"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["WeightOut"].FormatString = "#,#;(#,#);00";
			grdfrm.RootTable.Columns["WeightOut"].TotalFormatString = "#,#;(#,#);0";
			grdfrm.RootTable.Columns["BalWeight"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["BalWeight"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["BalWeight"].FormatString = "#,#;(#,#);00";
			grdfrm.RootTable.Columns["BalWeight"].TotalFormatString = "#,#;(#,#);0";
			grdfrm.RootTable.Columns["ItemName"].ColumnType = (ColumnType)5;
			grdfrm.RootTable.Columns["ParentCategory"].ColumnType = (ColumnType)5;
			((JanusCollectionBase)grdfrm.RootTable.Groups).Clear();
			if (Conversion.ToString((object)((Control)(object)cmbReportType).Text) == "Warehouse and Item Stock Summary")
			{
				grdfrm.RootTable.Groups.Add("WareHouseName");
				grdfrm.RootTable.Columns["WareHouseName"].Visible = false;
				grdfrm.RootTable.GroupTotals = (GroupTotals)2;
			}
			if (Conversion.ToString((object)((Control)(object)cmbReportType).Text) == "Item and WareHouse Stock Summary")
			{
				grdfrm.RootTable.Groups.Add("ItemName");
				grdfrm.RootTable.Columns["ItemName"].Visible = false;
				grdfrm.RootTable.GroupTotals = (GroupTotals)2;
			}
			if (Conversion.ToString((object)((Control)(object)cmbReportType).Text) == "Crop Year and Item Stock Summary")
			{
				grdfrm.RootTable.Groups.Add("CropYear");
				grdfrm.RootTable.Columns["CropYear"].Visible = false;
				grdfrm.RootTable.GroupTotals = (GroupTotals)2;
			}
			if (Conversion.ToString((object)((Control)(object)cmbReportType).Text) == "Crop Year, Warehouse and Item Stock Summary")
			{
				grdfrm.RootTable.Groups.Add("CropYear");
				grdfrm.RootTable.Columns["CropYear"].Visible = false;
				grdfrm.RootTable.Groups.Add("WareHouseName");
				grdfrm.RootTable.Columns["WareHouseName"].Visible = false;
				grdfrm.RootTable.GroupTotals = (GroupTotals)2;
			}
			if (Conversion.ToString(cmbReportType.Value) == "WarehouseandJoblotandItemStockSummary")
			{
				grdfrm.RootTable.Groups.Add("WareHouseName");
				grdfrm.RootTable.Columns["WareHouseName"].Visible = false;
				grdfrm.RootTable.Groups.Add("JobLot");
				grdfrm.RootTable.Columns["JobLot"].Visible = false;
				grdfrm.RootTable.GroupTotals = (GroupTotals)2;
			}
			if (Conversion.ToString(cmbReportType.Value) == "JobLotandItemStockSummary")
			{
				grdfrm.RootTable.Groups.Add("JobLot");
				grdfrm.RootTable.Columns["JobLot"].Visible = false;
				grdfrm.RootTable.GroupTotals = (GroupTotals)2;
			}
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
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

	private void grdfrm_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdfrm.CurrentRow;
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			int Id = Conversion.ToInt(r.Cells["ItemId"].Value);
			if (e.Column.Key == "ItemName")
			{
				InventoryTransactionReport obj = new InventoryTransactionReport(UserAccount);
				obj.RequestedByOtherDocument = true;
				obj.fromdate.Value = txtDateFrom.Value;
				obj.todate.Value = txtDateTo.Value;
				obj.CmbItem.Value = Id;
				if (((UltraGridBase)cmbWareHouse).ActiveRow != null)
				{
					obj.CmbWareHouse.Value = Conversion.ToInt(cmbWareHouse.Value);
				}
				if (((UltraGridBase)cmbJobLot).ActiveRow != null)
				{
					obj.CmbJobLot.Value = Conversion.ToInt(cmbJobLot.Value);
				}
				if (((UltraGridBase)cmbItem).ActiveRow != null)
				{
					obj.CmbItem.Value = Conversion.ToInt(cmbItem.Value);
				}
				if (((UltraGridBase)cmbCropYear).ActiveRow != null)
				{
					obj.CmbCropYear.Value = Conversion.ToInt(cmbCropYear.Value);
				}
				obj.RequestedByOtherDocument = true;
				obj.ShowDialog();
			}
			else if (e.Column.Key == "ParentCategory")
			{
				ItemLedgerWithoutValue obj2 = new ItemLedgerWithoutValue(UserAccount)
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
				obj2.CmbItem.Value = Id;
				obj2.RequestedByOtherDocument = true;
				obj2.Show();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
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

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)(object)cmbCategory).Text = string.Empty;
			((Control)(object)cmbCropYear).Text = string.Empty;
			((Control)(object)cmbItem).Text = string.Empty;
			((Control)(object)cmbItemType).Text = string.Empty;
			((Control)(object)cmbJobLot).Text = string.Empty;
			((Control)(object)cmbMainCategory).Text = string.Empty;
			((Control)(object)cmbReportType).Text = string.Empty;
			((Control)(object)cmbUOM).Text = string.Empty;
			((Control)(object)cmbWareHouse).Text = string.Empty;
			txtDateFrom.Focus();
			grdfrm.ClearStructure();
			DateTime AsOnDate = StocksReport.GetInventoryStockAsOnDate(UserAccount.OrganizationId, UserAccount.CompanyId);
			if (!Conversion.CheckDateTimeNull(AsOnDate))
			{
				txtDateFrom.Value = AsOnDate;
			}
			else
			{
				txtDateFrom.Value = clsGlobalVariables.ActiveYr.Start_Period;
			}
			BranchesFill();
			ParentCategoryFill();
			StockComboFill();
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

	private void PrintButtonManage()
	{
		try
		{
			if (((Control)(object)cmbReportType).Text != null && ((Control)(object)cmbReportType).Text != string.Empty)
			{
				if (Conversion.ToString(cmbReportType.Value) == "ItemStockSummary")
				{
					btnPrint.Text = "178-ItemStockSummary";
				}
				if (Conversion.ToString((object)((Control)(object)cmbReportType).Text) == "Item and WareHouse Stock Summary")
				{
					btnPrint.Text = "179-ItemandWarehouseStockSummary";
				}
				if (Conversion.ToString((object)((Control)(object)cmbReportType).Text) == "Warehouse and Item Stock Summary")
				{
					btnPrint.Text = "180-ItemandWarehouseStockSummary";
				}
				if (Conversion.ToString(cmbReportType.Value) == "ItemandCropYearStockSummary")
				{
					btnPrint.Text = "181-ItemandCropYearStockSummary";
				}
				if (Conversion.ToString(cmbReportType.Value) == "ItemandCropYearandWarehouseStockSummary")
				{
					btnPrint.Text = "182-ItemandCropYearandWarehouseStockSummary";
				}
				if (Conversion.ToString(cmbReportType.Value) == "JobLotandItemStockSummary")
				{
					btnPrint.Text = "183-JobLotandItemStockSummary";
				}
				if (Conversion.ToString(cmbReportType.Value) == "WarehouseandJoblotandItemStockSummary")
				{
					btnPrint.Text = "184-WarehouseandJoblotandItemStockSummary";
				}
				if (Conversion.ToString(cmbReportType.Value) == "ItemandPackSizeStockSummary")
				{
					btnPrint.Text = "185-ItemandPackSizeStockSummary";
				}
				if (Conversion.ToString(cmbReportType.Value) == "ItemandPackingTypeStockSummary")
				{
					btnPrint.Text = "186-ItemandPackingTypeStockSummary";
				}
				if (Conversion.ToString(cmbReportType.Value) == "ItemandPackSizeandPackingTypeStockSummary")
				{
					btnPrint.Text = "187-ItemandPackSizeandPackingTypeStockSummary";
				}
				if (Conversion.ToString(cmbReportType.Value) == "WIPStockPlantWiseSummary")
				{
					btnPrint.Text = "198-WIPStockPlantWiseSummary";
				}
			}
			else
			{
				btnPrint.Visible = false;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void cmbBranchName_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((Control)(object)cmbBranchName).Text != string.Empty && ((UltraGridBase)cmbBranchName).ActiveRow != null)
			{
				StockComboFill();
				return;
			}
			((Control)(object)CmbClassGroup).Text = string.Empty;
			((UltraGridBase)CmbClassGroup).DataSource = null;
			((Control)(object)cmbMainCategory).Text = string.Empty;
			((UltraGridBase)cmbMainCategory).DataSource = null;
			((Control)(object)cmbCategory).Text = string.Empty;
			((UltraGridBase)cmbCategory).DataSource = null;
			((Control)(object)cmbItemType).Text = string.Empty;
			((UltraGridBase)cmbItemType).DataSource = null;
			((Control)(object)cmbPacktype).Text = string.Empty;
			((UltraGridBase)cmbPacktype).DataSource = null;
			((Control)(object)cmbCropYear).Text = string.Empty;
			((UltraGridBase)cmbCropYear).DataSource = null;
			((Control)(object)cmbItem).Text = string.Empty;
			((UltraGridBase)cmbItem).DataSource = null;
			((Control)(object)cmbWareHouse).Text = string.Empty;
			((UltraGridBase)cmbWareHouse).DataSource = null;
			((Control)(object)cmbJobLot).Text = string.Empty;
			((UltraGridBase)cmbJobLot).DataSource = null;
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
			if (btnPrint.Text == "178-ItemStockSummary")
			{
				if (dtStockSum.Rows.Count == 0 || dtStockSum == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dtStockSum, "178-ItemStockSummary.rpt");
			}
			if (btnPrint.Text == "179-ItemandWarehouseStockSummary")
			{
				if (dtStockSum.Rows.Count == 0 || dtStockSum == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val2 = new Reporting();
				val2.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val2.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val2.ShowReportWithDataTable(dtStockSum, "179-ItemandWarehouseStockSummary.rpt");
			}
			if (btnPrint.Text == "180-ItemandWarehouseStockSummary")
			{
				if (dtStockSum.Rows.Count == 0 || dtStockSum == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val3 = new Reporting();
				val3.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val3.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val3.ShowReportWithDataTable(dtStockSum, "180-ItemandWarehouseStockSummary.rpt");
			}
			if (btnPrint.Text == "181-ItemandCropYearStockSummary")
			{
				if (dtStockSum.Rows.Count == 0 || dtStockSum == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val4 = new Reporting();
				val4.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val4.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val4.ShowReportWithDataTable(dtStockSum, "181-ItemandCropYearStockSummary.rpt");
			}
			if (btnPrint.Text == "182-ItemandCropYearandWarehouseStockSummary")
			{
				if (dtStockSum.Rows.Count == 0 || dtStockSum == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val5 = new Reporting();
				val5.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val5.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val5.ShowReportWithDataTable(dtStockSum, "182-ItemandCropYearandWarehouseStockSummary.rpt");
			}
			if (btnPrint.Text == "183-JobLotandItemStockSummary")
			{
				if (dtStockSum.Rows.Count == 0 || dtStockSum == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val6 = new Reporting();
				val6.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val6.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val6.ShowReportWithDataTable(dtStockSum, "183-JobLotandItemStockSummary.rpt");
			}
			if (btnPrint.Text == "184-WarehouseandJoblotandItemStockSummary")
			{
				if (dtStockSum.Rows.Count == 0 || dtStockSum == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val7 = new Reporting();
				val7.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val7.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val7.ShowReportWithDataTable(dtStockSum, "184-WarehouseandJoblotandItemStockSummary.rpt");
			}
			if (btnPrint.Text == "185-ItemandPackSizeStockSummary")
			{
				if (dtStockSum.Rows.Count == 0 || dtStockSum == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val8 = new Reporting();
				val8.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val8.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val8.ShowReportWithDataTable(dtStockSum, "185-ItemandPackSizeStockSummary.rpt");
			}
			if (btnPrint.Text == "186-ItemandPackingTypeStockSummary")
			{
				if (dtStockSum.Rows.Count == 0 || dtStockSum == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val9 = new Reporting();
				val9.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val9.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val9.ShowReportWithDataTable(dtStockSum, "186-ItemandPackingTypeStockSummary.rpt");
			}
			if (btnPrint.Text == "187-ItemandPackSizeandPackingTypeStockSummary")
			{
				if (dtStockSum.Rows.Count == 0 || dtStockSum == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val10 = new Reporting();
				val10.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val10.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val10.ShowReportWithDataTable(dtStockSum, "187-ItemandPackSizeandPackingTypeStockSummary.rpt");
			}
			if (btnPrint.Text == "198-WIPStockPlantWiseSummary")
			{
				if (dtStockSum.Rows.Count == 0 || dtStockSum == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val11 = new Reporting();
				val11.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val11.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val11.ShowReportWithDataTable(dtStockSum, "198-WIPStockPlantWiseSummary.rpt");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmStockReport_KeyDown(object sender, KeyEventArgs e)
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

	private void frmStockReport_FormClosing(object sender, FormClosingEventArgs e)
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

	private void cmbMainCategory_Leave(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToString((object)((Control)(object)cmbMainCategory).Text) != string.Empty)
			{
				StockComboFill();
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
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Expected O, but got Unknown
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Expected O, but got Unknown
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Expected O, but got Unknown
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Expected O, but got Unknown
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Expected O, but got Unknown
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Expected O, but got Unknown
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Expected O, but got Unknown
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Expected O, but got Unknown
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Expected O, but got Unknown
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Expected O, but got Unknown
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Expected O, but got Unknown
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Expected O, but got Unknown
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Expected O, but got Unknown
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Expected O, but got Unknown
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Expected O, but got Unknown
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Expected O, but got Unknown
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Expected O, but got Unknown
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Expected O, but got Unknown
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Expected O, but got Unknown
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Expected O, but got Unknown
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Expected O, but got Unknown
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Expected O, but got Unknown
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Expected O, but got Unknown
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Expected O, but got Unknown
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Expected O, but got Unknown
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Expected O, but got Unknown
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Expected O, but got Unknown
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Expected O, but got Unknown
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Expected O, but got Unknown
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Expected O, but got Unknown
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Expected O, but got Unknown
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Expected O, but got Unknown
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Expected O, but got Unknown
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Expected O, but got Unknown
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Expected O, but got Unknown
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Expected O, but got Unknown
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Expected O, but got Unknown
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Expected O, but got Unknown
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Expected O, but got Unknown
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Expected O, but got Unknown
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Expected O, but got Unknown
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Expected O, but got Unknown
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Expected O, but got Unknown
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Expected O, but got Unknown
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Expected O, but got Unknown
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Expected O, but got Unknown
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Expected O, but got Unknown
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Expected O, but got Unknown
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Expected O, but got Unknown
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Expected O, but got Unknown
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Expected O, but got Unknown
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Expected O, but got Unknown
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Expected O, but got Unknown
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0225: Expected O, but got Unknown
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Expected O, but got Unknown
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Expected O, but got Unknown
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_023a: Expected O, but got Unknown
		//IL_023a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Expected O, but got Unknown
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Expected O, but got Unknown
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_024f: Expected O, but got Unknown
		//IL_024f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Expected O, but got Unknown
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Expected O, but got Unknown
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0264: Expected O, but got Unknown
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_026b: Expected O, but got Unknown
		//IL_026b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0272: Expected O, but got Unknown
		//IL_0272: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Expected O, but got Unknown
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Expected O, but got Unknown
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_0287: Expected O, but got Unknown
		//IL_0287: Unknown result type (might be due to invalid IL or missing references)
		//IL_028e: Expected O, but got Unknown
		//IL_028e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Expected O, but got Unknown
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Expected O, but got Unknown
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Expected O, but got Unknown
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Expected O, but got Unknown
		//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b1: Expected O, but got Unknown
		//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b8: Expected O, but got Unknown
		//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bf: Expected O, but got Unknown
		//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c6: Expected O, but got Unknown
		//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cd: Expected O, but got Unknown
		//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d4: Expected O, but got Unknown
		//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02db: Expected O, but got Unknown
		//IL_02db: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e2: Expected O, but got Unknown
		//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e9: Expected O, but got Unknown
		//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f0: Expected O, but got Unknown
		//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f7: Expected O, but got Unknown
		//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fe: Expected O, but got Unknown
		//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0305: Expected O, but got Unknown
		//IL_0305: Unknown result type (might be due to invalid IL or missing references)
		//IL_030c: Expected O, but got Unknown
		//IL_030c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0313: Expected O, but got Unknown
		//IL_0313: Unknown result type (might be due to invalid IL or missing references)
		//IL_031a: Expected O, but got Unknown
		//IL_031a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0321: Expected O, but got Unknown
		//IL_0321: Unknown result type (might be due to invalid IL or missing references)
		//IL_0328: Expected O, but got Unknown
		//IL_0328: Unknown result type (might be due to invalid IL or missing references)
		//IL_032f: Expected O, but got Unknown
		//IL_032f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0336: Expected O, but got Unknown
		//IL_0336: Unknown result type (might be due to invalid IL or missing references)
		//IL_033d: Expected O, but got Unknown
		//IL_033d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0344: Expected O, but got Unknown
		//IL_0344: Unknown result type (might be due to invalid IL or missing references)
		//IL_034b: Expected O, but got Unknown
		//IL_034b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0352: Expected O, but got Unknown
		//IL_0352: Unknown result type (might be due to invalid IL or missing references)
		//IL_0359: Expected O, but got Unknown
		//IL_0359: Unknown result type (might be due to invalid IL or missing references)
		//IL_0360: Expected O, but got Unknown
		//IL_0360: Unknown result type (might be due to invalid IL or missing references)
		//IL_0367: Expected O, but got Unknown
		//IL_0367: Unknown result type (might be due to invalid IL or missing references)
		//IL_036e: Expected O, but got Unknown
		//IL_036e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0375: Expected O, but got Unknown
		//IL_0375: Unknown result type (might be due to invalid IL or missing references)
		//IL_037c: Expected O, but got Unknown
		//IL_037c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0383: Expected O, but got Unknown
		//IL_0383: Unknown result type (might be due to invalid IL or missing references)
		//IL_038a: Expected O, but got Unknown
		//IL_038a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0391: Expected O, but got Unknown
		//IL_0391: Unknown result type (might be due to invalid IL or missing references)
		//IL_0398: Expected O, but got Unknown
		//IL_0398: Unknown result type (might be due to invalid IL or missing references)
		//IL_039f: Expected O, but got Unknown
		//IL_039f: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a6: Expected O, but got Unknown
		//IL_03a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ad: Expected O, but got Unknown
		//IL_03ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b4: Expected O, but got Unknown
		//IL_03b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bb: Expected O, but got Unknown
		//IL_03bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c2: Expected O, but got Unknown
		//IL_03c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c9: Expected O, but got Unknown
		//IL_03c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d0: Expected O, but got Unknown
		//IL_03d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d7: Expected O, but got Unknown
		//IL_03d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03de: Expected O, but got Unknown
		//IL_03de: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e5: Expected O, but got Unknown
		//IL_03e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ec: Expected O, but got Unknown
		//IL_0409: Unknown result type (might be due to invalid IL or missing references)
		//IL_0413: Expected O, but got Unknown
		//IL_042a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0434: Expected O, but got Unknown
		//IL_044b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0455: Expected O, but got Unknown
		//IL_0477: Unknown result type (might be due to invalid IL or missing references)
		//IL_0481: Expected O, but got Unknown
		//IL_04b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c3: Expected O, but got Unknown
		//IL_04cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d9: Expected O, but got Unknown
		//IL_04e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ef: Expected O, but got Unknown
		//IL_04fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0505: Expected O, but got Unknown
		//IL_0511: Unknown result type (might be due to invalid IL or missing references)
		//IL_051b: Expected O, but got Unknown
		//IL_0527: Unknown result type (might be due to invalid IL or missing references)
		//IL_0531: Expected O, but got Unknown
		//IL_053d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0547: Expected O, but got Unknown
		//IL_0574: Unknown result type (might be due to invalid IL or missing references)
		//IL_057e: Expected O, but got Unknown
		//IL_057f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0589: Expected O, but got Unknown
		//IL_087c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0886: Expected O, but got Unknown
		Appearance appearance35 = new Appearance();
		Appearance appearance46 = new Appearance();
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
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
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
		Appearance appearance57 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
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
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
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
		Appearance appearance89 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Inventory_Stocks_Report.frmStockReport));
		this.panel1 = new System.Windows.Forms.Panel();
		this.grdfrm = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel3 = new System.Windows.Forms.Panel();
		this.cmbBranchName = new UltraCombo();
		this.label13 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.CmbClassGroup = new UltraCombo();
		this.chkIncludeWipItem = new System.Windows.Forms.CheckBox();
		this.chkSikeZero = new System.Windows.Forms.CheckBox();
		this.label12 = new System.Windows.Forms.Label();
		this.cmbPacktype = new UltraCombo();
		this.panel5 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label23 = new System.Windows.Forms.Label();
		this.ChkItemWiseStock = new System.Windows.Forms.CheckBox();
		this.label11 = new System.Windows.Forms.Label();
		this.cmbReportType = new UltraCombo();
		this.label10 = new System.Windows.Forms.Label();
		this.cmbCropYear = new UltraCombo();
		this.label5 = new System.Windows.Forms.Label();
		this.cmbJobLot = new UltraCombo();
		this.label4 = new System.Windows.Forms.Label();
		this.cmbUOM = new UltraCombo();
		this.label9 = new System.Windows.Forms.Label();
		this.cmbItem = new UltraCombo();
		this.label8 = new System.Windows.Forms.Label();
		this.cmbItemType = new UltraCombo();
		this.label7 = new System.Windows.Forms.Label();
		this.cmbMainCategory = new UltraCombo();
		this.label6 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.cmbWareHouse = new UltraCombo();
		this.cmbCategory = new UltraCombo();
		this.btnshow = new System.Windows.Forms.Button();
		this.txtDateFrom = new System.Windows.Forms.DateTimePicker();
		this.txtDateTo = new System.Windows.Forms.DateTimePicker();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdfrm).BeginInit();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbClassGroup).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPacktype).BeginInit();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbReportType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCropYear).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbJobLot).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbUOM).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbMainCategory).BeginInit();
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
		this.panel1.Size = new System.Drawing.Size(1084, 649);
		this.panel1.TabIndex = 0;
		this.grdfrm.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdfrm).BackColor = System.Drawing.Color.White;
		this.grdfrm.DefaultFilterRowComparison = (FilterConditionOperator)7;
		((System.Windows.Forms.Control)(object)this.grdfrm).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdfrm.FilterMode = (FilterMode)2;
		this.grdfrm.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdfrm).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdfrm.GroupTotals = (GroupTotals)2;
		this.grdfrm.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Location = new System.Drawing.Point(0, 231);
		((System.Windows.Forms.Control)(object)this.grdfrm).Name = "grdfrm";
		((ScrollableBase)this.grdfrm).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdfrm).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdfrm).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Size = new System.Drawing.Size(1084, 418);
		((System.Windows.Forms.Control)(object)this.grdfrm).TabIndex = 0;
		this.grdfrm.TotalRow = (InheritableBoolean)1;
		this.grdfrm.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdfrm).VisualStyle = (VisualStyle)4;
		this.grdfrm.LinkClicked += new ColumnActionEventHandler(grdfrm_LinkClicked);
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.ForeColor = System.Drawing.Color.Black;
		this.panel4.Location = new System.Drawing.Point(0, 206);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1084, 25);
		this.panel4.TabIndex = 2;
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbBranchName);
		this.panel3.Controls.Add(this.label13);
		this.panel3.Controls.Add(this.label2);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbClassGroup);
		this.panel3.Controls.Add(this.chkIncludeWipItem);
		this.panel3.Controls.Add(this.chkSikeZero);
		this.panel3.Controls.Add(this.label12);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPacktype);
		this.panel3.Controls.Add(this.panel5);
		this.panel3.Controls.Add(this.ChkItemWiseStock);
		this.panel3.Controls.Add(this.label11);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbReportType);
		this.panel3.Controls.Add(this.label10);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCropYear);
		this.panel3.Controls.Add(this.label5);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbJobLot);
		this.panel3.Controls.Add(this.label4);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbUOM);
		this.panel3.Controls.Add(this.label9);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItem);
		this.panel3.Controls.Add(this.label8);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemType);
		this.panel3.Controls.Add(this.label7);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbMainCategory);
		this.panel3.Controls.Add(this.label6);
		this.panel3.Controls.Add(this.label3);
		this.panel3.Controls.Add(this.label14);
		this.panel3.Controls.Add(this.label1);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbWareHouse);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCategory);
		this.panel3.Controls.Add(this.btnshow);
		this.panel3.Controls.Add(this.txtDateFrom);
		this.panel3.Controls.Add(this.txtDateTo);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 29);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1084, 177);
		this.panel3.TabIndex = 0;
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
		((AppearanceBase)appearance52).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance52;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance53).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance54;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance58).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance58).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance59).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance59).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance59).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Location = new System.Drawing.Point(104, 35);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Name = "cmbBranchName";
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Size = new System.Drawing.Size(227, 26);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Leave += new System.EventHandler(cmbBranchName_Leave);
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(6, 41);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(79, 15);
		this.label13.TabIndex = 214;
		this.label13.Text = "Branch Name";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(6, 125);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(69, 15);
		this.label2.TabIndex = 113;
		this.label2.Text = "Class Group";
		this.CmbClassGroup.AllowNull = (DefaultableBoolean)1;
		this.CmbClassGroup.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbClassGroup.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbClassGroup.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.Appearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance77).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance77).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance77).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbClassGroup).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance77;
		((AppearanceBase)appearance78).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance78;
		((SpecialBoxBase)((UltraGridBase)this.CmbClassGroup).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance80).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance80).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance80).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance81;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance84).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance85).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance85).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance85).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance85).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance86).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbClassGroup).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbClassGroup).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbClassGroup).Location = new System.Drawing.Point(104, 119);
		((System.Windows.Forms.Control)(object)this.CmbClassGroup).Name = "CmbClassGroup";
		((System.Windows.Forms.Control)(object)this.CmbClassGroup).Size = new System.Drawing.Size(227, 26);
		((System.Windows.Forms.Control)(object)this.CmbClassGroup).TabIndex = 3;
		this.chkIncludeWipItem.AutoSize = true;
		this.chkIncludeWipItem.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkIncludeWipItem.Location = new System.Drawing.Point(908, 39);
		this.chkIncludeWipItem.Name = "chkIncludeWipItem";
		this.chkIncludeWipItem.Size = new System.Drawing.Size(120, 19);
		this.chkIncludeWipItem.TabIndex = 14;
		this.chkIncludeWipItem.Text = "Include WIP Item";
		this.chkIncludeWipItem.UseVisualStyleBackColor = true;
		this.chkSikeZero.AutoSize = true;
		this.chkSikeZero.Checked = true;
		this.chkSikeZero.CheckState = System.Windows.Forms.CheckState.Checked;
		this.chkSikeZero.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkSikeZero.Location = new System.Drawing.Point(907, 68);
		this.chkSikeZero.Name = "chkSikeZero";
		this.chkSikeZero.Size = new System.Drawing.Size(76, 19);
		this.chkSikeZero.TabIndex = 15;
		this.chkSikeZero.Text = "Skip Zero";
		this.chkSikeZero.UseVisualStyleBackColor = true;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label12.Location = new System.Drawing.Point(337, 153);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(77, 15);
		this.label12.TabIndex = 111;
		this.label12.Text = "Packing Type";
		this.cmbPacktype.AllowNull = (DefaultableBoolean)1;
		this.cmbPacktype.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPacktype.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPacktype.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance132).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.Appearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance133).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance133).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPacktype).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance133;
		((AppearanceBase)appearance135).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance135;
		((SpecialBoxBase)((UltraGridBase)this.cmbPacktype).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance136).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance136).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance136).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance137).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance137;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance138).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance139;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance140).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance141).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance141).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance141).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance141).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance141;
		((AppearanceBase)appearance142).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPacktype).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPacktype).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPacktype).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbPacktype.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPacktype).Location = new System.Drawing.Point(419, 147);
		((System.Windows.Forms.Control)(object)this.cmbPacktype).Name = "cmbPacktype";
		((System.Windows.Forms.Control)(object)this.cmbPacktype).Size = new System.Drawing.Size(153, 26);
		((System.Windows.Forms.Control)(object)this.cmbPacktype).TabIndex = 9;
		this.panel5.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel5.Controls.Add(this.ctrlGrdBar1);
		this.panel5.Controls.Add(this.label23);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1084, 31);
		this.panel5.TabIndex = 109;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1037, 3);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdfrm;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 25);
		this.ctrlGrdBar1.TabIndex = 20;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(7, 6);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(115, 19);
		this.label23.TabIndex = 1;
		this.label23.Text = "Stock Report";
		this.ChkItemWiseStock.AutoSize = true;
		this.ChkItemWiseStock.Checked = true;
		this.ChkItemWiseStock.CheckState = System.Windows.Forms.CheckState.Checked;
		this.ChkItemWiseStock.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkItemWiseStock.Location = new System.Drawing.Point(908, 95);
		this.ChkItemWiseStock.Name = "ChkItemWiseStock";
		this.ChkItemWiseStock.Size = new System.Drawing.Size(113, 19);
		this.ChkItemWiseStock.TabIndex = 16;
		this.ChkItemWiseStock.Text = "Item Wise Stock";
		this.ChkItemWiseStock.UseVisualStyleBackColor = true;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label11.Location = new System.Drawing.Point(578, 125);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(70, 15);
		this.label11.TabIndex = 107;
		this.label11.Text = "Report Type";
		this.cmbReportType.AllowNull = (DefaultableBoolean)1;
		this.cmbReportType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbReportType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbReportType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbReportType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbReportType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbReportType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbReportType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance41;
		((SpecialBoxBase)((UltraGridBase)this.cmbReportType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbReportType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbReportType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbReportType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.cmbReportType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbReportType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbReportType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbReportType).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbReportType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbReportType).Location = new System.Drawing.Point(651, 119);
		((System.Windows.Forms.Control)(object)this.cmbReportType).Name = "cmbReportType";
		((System.Windows.Forms.Control)(object)this.cmbReportType).Size = new System.Drawing.Size(250, 26);
		((System.Windows.Forms.Control)(object)this.cmbReportType).TabIndex = 13;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label10.Location = new System.Drawing.Point(337, 125);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(57, 15);
		this.label10.TabIndex = 105;
		this.label10.Text = "Crop Year";
		this.cmbCropYear.AllowNull = (DefaultableBoolean)1;
		this.cmbCropYear.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCropYear.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCropYear.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Appearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance4).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance4).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance4).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCropYear).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance5;
		((SpecialBoxBase)((UltraGridBase)this.cmbCropYear).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance7;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance10).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance11).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance11).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance11).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance11).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance12).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCropYear).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCropYear).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbCropYear.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCropYear).Location = new System.Drawing.Point(419, 119);
		((System.Windows.Forms.Control)(object)this.cmbCropYear).Name = "cmbCropYear";
		((System.Windows.Forms.Control)(object)this.cmbCropYear).Size = new System.Drawing.Size(153, 26);
		((System.Windows.Forms.Control)(object)this.cmbCropYear).TabIndex = 8;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label5.Location = new System.Drawing.Point(578, 97);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(46, 15);
		this.label5.TabIndex = 103;
		this.label5.Text = "Job Lot";
		this.cmbJobLot.AllowNull = (DefaultableBoolean)1;
		this.cmbJobLot.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbJobLot.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbJobLot.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance57).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Appearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance68).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbJobLot).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance68;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance79;
		((SpecialBoxBase)((UltraGridBase)this.cmbJobLot).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbJobLot).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbJobLot.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbJobLot).Location = new System.Drawing.Point(651, 91);
		((System.Windows.Forms.Control)(object)this.cmbJobLot).Name = "cmbJobLot";
		((System.Windows.Forms.Control)(object)this.cmbJobLot).Size = new System.Drawing.Size(250, 26);
		((System.Windows.Forms.Control)(object)this.cmbJobLot).TabIndex = 12;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label4.Location = new System.Drawing.Point(337, 101);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(61, 15);
		this.label4.TabIndex = 101;
		this.label4.Text = "Pack Uom";
		this.cmbUOM.AllowNull = (DefaultableBoolean)1;
		this.cmbUOM.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbUOM.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbUOM.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Appearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.cmbUOM).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbUOM).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance120).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance120).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance120).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbUOM).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance121).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbUOM).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance121;
		((SpecialBoxBase)((UltraGridBase)this.cmbUOM).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance122).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance122).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance122).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbUOM).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.cmbUOM).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbUOM).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance124).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance124;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance126;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance127).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance128).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance128).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance128).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance128;
		((AppearanceBase)appearance129).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.cmbUOM).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbUOM).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbUOM).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbUOM).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbUOM.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbUOM).Location = new System.Drawing.Point(419, 91);
		((System.Windows.Forms.Control)(object)this.cmbUOM).Name = "cmbUOM";
		((System.Windows.Forms.Control)(object)this.cmbUOM).Size = new System.Drawing.Size(153, 26);
		((System.Windows.Forms.Control)(object)this.cmbUOM).TabIndex = 7;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label9.Location = new System.Drawing.Point(578, 41);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(67, 15);
		this.label9.TabIndex = 99;
		this.label9.Text = "Item Name";
		this.cmbItem.AllowNull = (DefaultableBoolean)1;
		this.cmbItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.cmbItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance64).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance64).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance64).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance64;
		((AppearanceBase)appearance65).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance65;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance66).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance67;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance71).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance72).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance72).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance72).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItem).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItem).Location = new System.Drawing.Point(651, 35);
		((System.Windows.Forms.Control)(object)this.cmbItem).Name = "cmbItem";
		((System.Windows.Forms.Control)(object)this.cmbItem).Size = new System.Drawing.Size(250, 26);
		((System.Windows.Forms.Control)(object)this.cmbItem).TabIndex = 10;
		((System.Windows.Forms.Control)(object)this.cmbItem).Leave += new System.EventHandler(cmbItem_Leave);
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(337, 69);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(63, 15);
		this.label8.TabIndex = 97;
		this.label8.Text = "Item Type ";
		this.cmbItemType.AllowNull = (DefaultableBoolean)1;
		this.cmbItemType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.cmbItemType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance107).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance107).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance107).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance107;
		((AppearanceBase)appearance108).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance108;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance109).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance109).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance109).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.cmbItemType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance110).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance110;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance111).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance113;
		((AppearanceBase)appearance114).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance114).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance115).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance115).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance115).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance116).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.cmbItemType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItemType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemType).Location = new System.Drawing.Point(419, 63);
		((System.Windows.Forms.Control)(object)this.cmbItemType).Name = "cmbItemType";
		((System.Windows.Forms.Control)(object)this.cmbItemType).Size = new System.Drawing.Size(153, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemType).TabIndex = 6;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(6, 153);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(91, 15);
		this.label7.TabIndex = 96;
		this.label7.Text = "Parent Category";
		this.cmbMainCategory.AllowNull = (DefaultableBoolean)1;
		this.cmbMainCategory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbMainCategory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbMainCategory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbMainCategory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbMainCategory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbMainCategory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbMainCategory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbMainCategory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.cmbMainCategory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbMainCategory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbMainCategory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbMainCategory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbMainCategory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbMainCategory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.cmbMainCategory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbMainCategory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbMainCategory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance33).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbMainCategory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.cmbMainCategory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbMainCategory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbMainCategory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbMainCategory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.cmbMainCategory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbMainCategory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbMainCategory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.cmbMainCategory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbMainCategory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.cmbMainCategory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbMainCategory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbMainCategory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbMainCategory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.cmbMainCategory).Location = new System.Drawing.Point(104, 147);
		((System.Windows.Forms.Control)(object)this.cmbMainCategory).Name = "cmbMainCategory";
		((System.Windows.Forms.Control)(object)this.cmbMainCategory).Size = new System.Drawing.Size(227, 26);
		((System.Windows.Forms.Control)(object)this.cmbMainCategory).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.cmbMainCategory).Leave += new System.EventHandler(cmbMainCategory_Leave);
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label6.Location = new System.Drawing.Point(578, 69);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(69, 15);
		this.label6.TabIndex = 95;
		this.label6.Text = "WareHouse";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label3.Location = new System.Drawing.Point(337, 41);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(82, 15);
		this.label3.TabIndex = 92;
		this.label3.Text = "Item Category";
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(6, 97);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(48, 15);
		this.label14.TabIndex = 90;
		this.label14.Text = "Date To";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(6, 69);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(63, 15);
		this.label1.TabIndex = 90;
		this.label1.Text = "Date From";
		this.cmbWareHouse.AllowNull = (DefaultableBoolean)1;
		this.cmbWareHouse.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbWareHouse.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbWareHouse.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Appearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance91).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance91).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance91).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbWareHouse).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance91;
		((AppearanceBase)appearance92).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance92;
		((SpecialBoxBase)((UltraGridBase)this.cmbWareHouse).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance93).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance93).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance93).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance93;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance94;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance97).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance97).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance98).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance98).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance98).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance98).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance98;
		((AppearanceBase)appearance99).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance100;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbWareHouse).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbWareHouse).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbWareHouse.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbWareHouse).Location = new System.Drawing.Point(651, 63);
		((System.Windows.Forms.Control)(object)this.cmbWareHouse).Name = "cmbWareHouse";
		((System.Windows.Forms.Control)(object)this.cmbWareHouse).Size = new System.Drawing.Size(250, 26);
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
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance90).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance90).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance90).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCategory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance90;
		((AppearanceBase)appearance101).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCategory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance101;
		((SpecialBoxBase)((UltraGridBase)this.cmbCategory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance112).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCategory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.cmbCategory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCategory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance123).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance123;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance134).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance134;
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
		((AppearanceBase)appearance103).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance104).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCategory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.cmbCategory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCategory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCategory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCategory).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbCategory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCategory).Location = new System.Drawing.Point(419, 35);
		((System.Windows.Forms.Control)(object)this.cmbCategory).Name = "cmbCategory";
		((System.Windows.Forms.Control)(object)this.cmbCategory).Size = new System.Drawing.Size(153, 26);
		((System.Windows.Forms.Control)(object)this.cmbCategory).TabIndex = 5;
		this.btnshow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(841, 147);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(60, 26);
		this.btnshow.TabIndex = 17;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.txtDateFrom.CustomFormat = "dd-MMM-yy";
		this.txtDateFrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDateFrom.Location = new System.Drawing.Point(104, 65);
		this.txtDateFrom.Name = "txtDateFrom";
		this.txtDateFrom.Size = new System.Drawing.Size(227, 23);
		this.txtDateFrom.TabIndex = 1;
		this.txtDateTo.CustomFormat = "dd-MMM-yy";
		this.txtDateTo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDateTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDateTo.Location = new System.Drawing.Point(104, 93);
		this.txtDateTo.Name = "txtDateTo";
		this.txtDateTo.Size = new System.Drawing.Size(227, 23);
		this.txtDateTo.TabIndex = 2;
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1084, 29);
		this.panel2.TabIndex = 0;
		this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnRefresh, this.btnPrint });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1084, 29);
		this.toolStrip1.TabIndex = 1;
		this.toolStrip1.Text = "toolStrip1";
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 26);
		this.btnRefresh.Text = "&Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(56, 26);
		this.btnPrint.Text = "Print";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1084, 649);
		base.Controls.Add(this.panel1);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Name = "frmStockReport";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmStockReport_FormClosing);
		base.Load += new System.EventHandler(frmStockReport_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmStockReport_KeyDown);
		this.panel1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdfrm).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbClassGroup).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPacktype).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbReportType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCropYear).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbJobLot).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbUOM).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbMainCategory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbWareHouse).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCategory).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		base.ResumeLayout(false);
	}
}
