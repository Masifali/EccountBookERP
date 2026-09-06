using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Architecture.BLL.IPM;
using Architecture.BLL.Reports.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.IPM;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.CommissionAgent.Helper_Methods;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.IPM;

public class frmLabActivityIPM2 : Form
{
	private Rightsobjects formright = new Rightsobjects();

	private DropDownBind DDL = new DropDownBind();

	private Attachment AT = new Attachment();

	private UserAccount UserAccount = new UserAccount();

	private DataTable dtDataFromDB = new DataTable();

	private DataTable dtGrid = new DataTable();

	private DataTable dtBindCombo = new DataTable();

	private DataTable dtCommonCombo = new DataTable();

	private int DocumentTypeId;

	private bool FormInitialized;

	private string AttachmentsValues = "";

	private string CustomAttachmentsValues = "";

	private int DefaultDaysToLessFromHistoryFromDate;

	private bool _isUpdatingGrid;

	private IContainer components;

	private Panel panel1;

	private Label label23;

	private Panel panel2;

	private Button btngrnlod;

	private DateTimePicker Todate;

	private DateTimePicker FromDate;

	private Label label1;

	private CtrlGrdBar ctrlGrdBar1;

	internal GridEX grd;

	private Label label2;

	internal UltraCombo CmbSupplier;

	private Label label4;

	internal UltraCombo CmbItemName;

	private Label label7;

	internal UltraCombo CmbCropYear;

	private Label label6;

	private Label label5;

	internal UltraCombo CmbWarehouse;

	private Label label3;

	internal UltraCombo CmbReferenceDocument;

	internal UltraCombo CmbJobLot;

	private Label label12;

	internal UltraCombo CmbItemType;

	private Label label11;

	internal UltraCombo CmbItemCategory;

	private Panel panel3;

	private ToolStrip toolStrip1;

	private ToolStripSeparator toolStripSeparator6;

	private ToolStripButton btnNew;

	private ToolStripButton btnRefresh;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripButton btnShortcutKeys;

	private ToolStripSeparator toolStripSeparator1;

	private Label label9;

	private ToolStripButton btnUpdate;

	private TextBox txtUom;

	private Label label10;

	internal UltraCombo cmbMainType;

	private Label label8;

	public frmLabActivityIPM2(UserAccount user)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
		InitializeComponentMethod();
	}

	private void LoadInvoices_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1310, 830);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private async void InitializeComponentMethod()
	{
		Task.Run(delegate
		{
			formright = CommonServices.SetRightsValueInRightsObject(base.Name);
			ComboDbCall();
			dtCommonCombo = labIPmActivityLog.CommonCombo((string)null);
		}).ContinueWith(delegate(Task t)
		{
			if (!base.IsDisposed)
			{
				if (t.IsFaulted)
				{
					MessageBox.Show("Error occurred during database call.");
				}
				else
				{
					btnUpdate.Enabled = formright.DoHaveSaveRight || formright.DoHaveUpdateRights;
					GetConfigurationsFromGlobal();
					ParentCategoryComboFill();
					ComboBind();
					FromDate.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
					Todate.Value = DateTime.Now;
					FormInitialized = true;
					FromDate.Focus();
				}
			}
		}, TaskScheduler.FromCurrentSynchronizationContext());
	}

	private void GetConfigurationsFromGlobal()
	{
		try
		{
			DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("DefaultDaysToLessFromHistoryFromDate"));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ComboDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			};
			obj.Ids = "1,2,3,4,6";
			dtBindCombo = StocksReport.Evalaution_DropDown_ByParentCategories(obj);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ParentCategoryComboFill()
	{
		try
		{
			((UltraGridBase)cmbMainType).DataSource = null;
			DataRow[] array = dtBindCombo.Select("ActivityType = 'ParentCategories'");
			DataTable dtMainType = DatatableHelper.CreateTableForCombo();
			DataRow[] array2 = array;
			foreach (DataRow row in array2)
			{
				dtMainType.Rows.Add(row["Id"], row["name"]);
			}
			DDL.BindDDL(dtMainType, cmbMainType, "Id", "name", "Parent Categories", ZeroIndex: false);
			UltraGridLayout layout = ((UltraGridBase)cmbMainType).DisplayLayout;
			layout.AutoFitStyle = (AutoFitStyle)1;
			if (!((KeyedSubObjectsCollectionBase)layout.Bands[0].Columns).Exists("Selected"))
			{
				UltraGridColumn obj = layout.Bands[0].Columns.Add("Selected");
				((HeaderBase)obj.Header).Caption = string.Empty;
				obj.Header.CheckBoxVisibility = (HeaderCheckBoxVisibility)2;
				obj.DataType = typeof(bool);
				((HeaderBase)obj.Header).VisiblePosition = 0;
			}
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

	private void ComboBind()
	{
		try
		{
			DataTable dtFiltered = new DataTable();
			HashSet<string> validIds = new HashSet<string>();
			if (!string.IsNullOrWhiteSpace(((Control)(object)cmbMainType).Text))
			{
				string[] array = ((Control)(object)cmbMainType).Text.Trim().Split(',');
				Dictionary<string, string> nameToIdMap = (((UltraGridBase)cmbMainType).DataSource as DataTable).AsEnumerable().ToDictionary((DataRow row) => row["name"].ToString().Trim(), (DataRow row) => row["Id"].ToString());
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
			Dictionary<string, DataTable> tables = new Dictionary<string, DataTable>
			{
				{
					"ItemTypes",
					DatatableHelper.CreateTableForCombo()
				},
				{
					"ItemCategories",
					DatatableHelper.CreateTableForCombo()
				},
				{
					"Items",
					DatatableHelper.CreateTableForCombo()
				},
				{
					"Warehouse",
					DatatableHelper.CreateTableForCombo()
				},
				{
					"DocumentType",
					DatatableHelper.CreateTableForCombo()
				},
				{
					"ItemClassGroup",
					DatatableHelper.CreateTableForCombo()
				},
				{
					"JobLot",
					DatatableHelper.CreateTableForCombo()
				},
				{
					"CropYear",
					DatatableHelper.CreateTableForCombo()
				},
				{
					"PackingType",
					DatatableHelper.CreateTableForCombo()
				},
				{
					"Supplier_Customer",
					DatatableHelper.CreateTableForCombo()
				}
			};
			if (dtFiltered.Rows.Count > 0)
			{
				Dictionary<string, HashSet<int>> addedIds = new Dictionary<string, HashSet<int>>();
				foreach (string activity in tables.Keys)
				{
					addedIds[activity] = new HashSet<int>();
				}
				foreach (DataRow row2 in dtFiltered.Rows)
				{
					string activityType = Conversion.ToString(row2["ActivityType"]);
					int id = Conversion.ToInt(row2["Id"]);
					if (tables.ContainsKey(activityType) && !addedIds[activityType].Contains(id))
					{
						tables[activityType].Rows.Add(id, row2["name"]);
						addedIds[activityType].Add(id);
					}
				}
			}
			InfragisticsHelper.BindAndRetainSelection(CmbItemCategory, tables["ItemCategories"], "Id", "name", "Item Category", AllColumns: true, SafeValue(CmbItemCategory.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbItemType, tables["ItemTypes"], "Id", "name", "Item Type", AllColumns: true, SafeValue(CmbItemType.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbJobLot, tables["JobLot"], "Id", "name", "JobLot", AllColumns: true, SafeValue(CmbJobLot.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbCropYear, tables["CropYear"], "Id", "name", "CropYear", AllColumns: true, SafeValue(CmbCropYear.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbWarehouse, tables["Warehouse"], "Id", "name", "WarehouseName", AllColumns: true, SafeValue(CmbWarehouse.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbReferenceDocument, tables["DocumentType"], "Id", "name", "Document Type", AllColumns: true, SafeValue(CmbReferenceDocument.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbSupplier, tables["Supplier_Customer"], "Id", "name", "Party Name", AllColumns: true, SafeValue(CmbSupplier.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbItemName, tables["Items"], "Id", "name", "Item Name", AllColumns: true, SafeValue(CmbItemName.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		static int SafeValue(object val)
		{
			if (val != null)
			{
				return Conversion.ToInt(val);
			}
			return 0;
		}
	}

	private void cmbMainType_Leave(object sender, EventArgs e)
	{
		try
		{
			try
			{
				if (Conversion.ToString((object)((Control)(object)cmbMainType).Text) != string.Empty)
				{
					ComboBind();
					return;
				}
				((Control)(object)CmbItemCategory).Text = string.Empty;
				((UltraGridBase)CmbItemCategory).DataSource = null;
				((Control)(object)CmbItemType).Text = string.Empty;
				((UltraGridBase)CmbItemType).DataSource = null;
				((Control)(object)CmbItemName).Text = string.Empty;
				((UltraGridBase)CmbItemName).DataSource = null;
				((Control)(object)CmbWarehouse).Text = string.Empty;
				((UltraGridBase)CmbWarehouse).DataSource = null;
				((Control)(object)CmbJobLot).Text = string.Empty;
				((UltraGridBase)CmbJobLot).DataSource = null;
				((Control)(object)CmbCropYear).Text = string.Empty;
				((UltraGridBase)CmbCropYear).DataSource = null;
				((Control)(object)CmbReferenceDocument).Text = string.Empty;
				((UltraGridBase)CmbReferenceDocument).DataSource = null;
				((Control)(object)CmbSupplier).Text = string.Empty;
				((UltraGridBase)CmbSupplier).DataSource = null;
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

	private void BtnShow_Click(object sender, EventArgs e)
	{
		try
		{
			AvailableStockDataDbCall();
			AvailableStockDataGridFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AvailableStockDataDbCall()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		try
		{
			dtDataFromDB.Clear();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.FromDate = Convert.ToDateTime(FromDate.Value);
			obj.ToDate = Convert.ToDateTime(Todate.Value);
			string Ids = "";
			if (((Control)(object)cmbMainType).Text != null && ((Control)(object)cmbMainType).Text != string.Empty)
			{
				string[] array = (Conversion.ToString((object)((Control)(object)cmbMainType).Text.Trim()) + ",").Split(',');
				DataTable dtMainType = ((UltraGridBase)cmbMainType).DataSource as DataTable;
				string[] array2 = array;
				foreach (string author in array2)
				{
					DataRow[] dr = dtMainType.Select("name='" + Conversion.ToString((object)author) + "'");
					if (dr.Length != 0)
					{
						Ids = Ids + Conversion.ToString(dr[0].ItemArray[0]) + ",";
					}
				}
				obj.ParentCategoryIds = Ids;
				obj.ItemCategoryId = Conversion.ToInt(CmbItemCategory.Value);
				obj.ItemTypeId = Conversion.ToInt(CmbItemType.Value);
				obj.JobLotId = Conversion.ToInt(CmbJobLot.Value);
				obj.CropYear = Conversion.ToString((object)((Control)(object)CmbCropYear).Text);
				obj.WarehouseId = Conversion.ToInt(CmbWarehouse.Value);
				obj.RefDocumentTypeId = Conversion.ToInt(CmbReferenceDocument.Value);
				obj.SupplierCustomerId = Conversion.ToInt(CmbSupplier.Value);
				obj.ItemId = Conversion.ToInt(CmbItemName.Value);
				dtDataFromDB = labIPmActivityLog.HoldStockForFumigation(obj);
				return;
			}
			cmbMainType.Focus();
			throw new Exception("Select Parent Category First");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AvailableStockDataGridFill()
	{
		try
		{
			DataTable dataTable = dtDataFromDB;
			if (dataTable != null && dataTable.Rows.Count == 0)
			{
				grd.DataSource = null;
			}
			double Uom = Conversion.ToDouble((object)txtUom.Text);
			DataTable dataTable2 = dtGrid;
			if (dataTable2 != null && dataTable2.Columns.Count == 0)
			{
				dtGrid.Columns.Add("Id", typeof(int));
				dtGrid.Columns.Add("MasterId", typeof(int));
				dtGrid.Columns.Add("RefDocumentTypeId", typeof(int));
				dtGrid.Columns.Add("RefDocIdNo", typeof(int));
				dtGrid.Columns.Add("RefDocSubIdNo", typeof(int));
				dtGrid.Columns.Add("RefDocumentType", typeof(string));
				dtGrid.Columns.Add("RefDocDate", typeof(DateTime));
				dtGrid.Columns.Add("RefDocNo", typeof(int));
				dtGrid.Columns.Add("VehicleNo", typeof(string));
				dtGrid.Columns.Add("ItemId", typeof(int));
				dtGrid.Columns.Add("ItemCode", typeof(string));
				dtGrid.Columns.Add("ItemName", typeof(string));
				dtGrid.Columns.Add("QtyBalance", typeof(decimal));
				dtGrid.Columns.Add("WeightBalance", typeof(decimal));
				dtGrid.Columns.Add($"QtyPer{Uom}Kg", typeof(decimal));
				dtGrid.Columns.Add("JobLotId", typeof(int));
				dtGrid.Columns.Add("AnalysisStepId", typeof(int));
				dtGrid.Columns.Add("FumigatedDate", typeof(DateTime));
				dtGrid.Columns.Add("FumigatedDays", typeof(int));
				dtGrid.Columns.Add("CompletionDueDate", typeof(DateTime));
				dtGrid.Columns.Add("StatusAfterCompletionId", typeof(int));
				dtGrid.Columns.Add("NextFumigationPlanDate", typeof(DateTime));
				dtGrid.Columns.Add("FumigatedBy", typeof(string));
				dtGrid.Columns.Add("SupervisorCheckBy", typeof(string));
				dtGrid.Columns.Add("VerifiedBy", typeof(string));
				dtGrid.Columns.Add("StockRelaseHoldStatusId", typeof(int));
				dtGrid.Columns.Add("FumigationActivity", typeof(string));
				dtGrid.Columns.Add("FumigationModeId", typeof(int));
				dtGrid.Columns.Add("GrnNo", typeof(int));
				dtGrid.Columns.Add("GpNo", typeof(string));
				dtGrid.Columns.Add("SupplierCustomerId", typeof(int));
				dtGrid.Columns.Add("SupplierCustomerName", typeof(string));
				dtGrid.Columns.Add("WarehouseId", typeof(int));
				dtGrid.Columns.Add("WareHouseCode", typeof(string));
				dtGrid.Columns.Add("RefWarehouse", typeof(string));
				dtGrid.Columns.Add("ManualNo", typeof(string));
			}
			dtGrid.Rows.Clear();
			foreach (DataRow row in dtDataFromDB.Rows)
			{
				int Id = Conversion.ToInt(row["labIPmActivityLogId"]);
				int MasterId = Conversion.ToInt(row["labIPmActivityLogMasterId"]);
				int ActivityStepId = Conversion.ToInt(row["qcActivityStepId"]);
				if (Id > 0)
				{
					ActivityStepId++;
				}
				double WeightBalance = Conversion.ToDouble(row["WeightBalance"]);
				double QtyPer65Kg = ((Uom > 0.0) ? (WeightBalance / Uom) : 0.0);
				dtGrid.Rows.Add(Id, MasterId, row["RefDocumentTypeId"], row["RefDocIdNo"], row["RefDocSubIdNo"], row["RefDocumentType"], row["DocDate"], row["DocCodeNo"], row["VehicleNo"], row["ItemId"], row["ItemCode"], row["ItemName"], row["QtyBalance"], row["WeightBalance"], QtyPer65Kg, row["jobLotIdIpm"], ActivityStepId, row["qcActivityDate"], row["activityForDays"], row["completionDueDate"], row["qcActivityStatusAfterCompletionId"], row["nextActivityPlanDate"], row["fumigatedBy"], row["checkBy"], row["verifiedBy"], row["stockReleaseHoldStatusId"], row["qcActivityDescription"], 1, (Conversion.ToInt(row["RefDocumentTypeId"]) == 112) ? ((object)0) : ((Conversion.ToInt(row["RefDocumentTypeId"]) == 80) ? ((object)0) : row["GrnNo"]), row["GpNo"], row["SupplierCustomerId"], row["SupplierCustomerName"], row["WarehouseId"], row["WareHouseCode"], row["RefWarehouse"], row["ManualNo"]);
			}
			BindGrid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindGrid()
	{
		try
		{
			DataTable dataTable = dtGrid;
			if (dataTable != null && dataTable.Columns.Count > 0)
			{
				grd.DataSource = dtGrid;
				grd.RetrieveStructure();
				grdSettings();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettings()
	{
		try
		{
			double Uom = Conversion.ToDouble((object)txtUom.Text);
			List<string> hiddenColumns = new List<string> { "Id", "MasterId", "RefDocumentTypeId", "RefDocIdNo", "RefDocSubIdNo", "ItemId", "SupplierCustomerId", "WarehouseId" };
			Dictionary<string, EditType> editableColumns = new Dictionary<string, EditType>
			{
				{
					"FumigatedDate",
					(EditType)2
				},
				{
					"FumigatedDays",
					(EditType)1
				},
				{
					"NextFumigationPlanDate",
					(EditType)2
				},
				{
					"FumigationActivity",
					(EditType)1
				},
				{
					"FumigatedBy",
					(EditType)1
				},
				{
					"SupervisorCheckBy",
					(EditType)1
				},
				{
					"VerifiedBy",
					(EditType)1
				}
			};
			GridEX_Helper.GridWrappingAndColumnSettings(grd, 3, 3, hiddenColumns, editableColumns);
			GridComboBind();
			GridEX_Helper.SetColumnWidth(grd, "RefDocDate", Constants.InventoryConstants.DateConstant);
			GridEX_Helper.SetColumnWidth(grd, "RefDocNo", Constants.InventoryConstants.DocNoConstant);
			GridEX_Helper.SetColumnWidth(grd, "RefDocumentType", Constants.InventoryConstants.DocumentTypeCode);
			GridEX_Helper.SetColumnWidth(grd, "VehicleNo", Constants.InventoryConstants.VehicleNoConstant);
			GridEX_Helper.SetColumnWidth(grd, "SupplierCustomerName", Constants.InventoryConstants.ItemName);
			GridEX_Helper.SetColumnWidth(grd, "GrnNo", Constants.InventoryConstants.DocNoConstant);
			GridEX_Helper.SetColumnWidth(grd, "ManualNo", Constants.InventoryConstants.RecordNo);
			GridEX_Helper.SetColumnWidth(grd, "GpNo", Constants.InventoryConstants.GpNoConstant);
			GridEX_Helper.SetColumnWidth(grd, "WareHouseCode", Constants.InventoryConstants.WareHouseName - 40);
			GridEX_Helper.SetColumnWidth(grd, "RefWarehouse", Constants.InventoryConstants.WareHouseName - 40);
			GridEX_Helper.SetColumnWidth(grd, "ItemName", Constants.InventoryConstants.ItemName);
			GridEX_Helper.SetColumnWidth(grd, "ItemCode", Constants.InventoryConstants.ItemCode - 20);
			GridEX_Helper.SetColumnWidth(grd, "QtyBalance", Constants.InventoryConstants.QtyConstant);
			GridEX_Helper.SetColumnWidth(grd, "WeightBalance", Constants.InventoryConstants.WeightConstant);
			GridEX_Helper.SetColumnWidth(grd, $"QtyPer{Uom}Kg", Constants.InventoryConstants.QtyConstant);
			GridEX_Helper.SetColumnWidth(grd, "FumigationLotNo", 80);
			GridEX_Helper.SetColumnWidth(grd, "AnalysisStepId", 90);
			GridEX_Helper.SetColumnWidth(grd, "AnalysisDate", Constants.InventoryConstants.DateConstant);
			GridEX_Helper.SetColumnWidth(grd, "FumigatedDate", Constants.InventoryConstants.DateConstant);
			GridEX_Helper.SetColumnWidth(grd, "FumigatedDays", 70);
			GridEX_Helper.SetColumnWidth(grd, "CompletionDueDate", Constants.InventoryConstants.DateConstant + 10);
			GridEX_Helper.SetColumnWidth(grd, "StatusAfterCompletionId", 180);
			GridEX_Helper.SetColumnWidth(grd, "NextFumigationPlanDate", Constants.InventoryConstants.DateConstant);
			GridEX_Helper.SetColumnWidth(grd, "FumigatedBy", 120);
			GridEX_Helper.SetColumnWidth(grd, "SupervisorCheckBy", 120);
			GridEX_Helper.SetColumnWidth(grd, "VerifiedBy", 120);
			GridEX_Helper.SetColumnWidth(grd, "StockReleaseHoldStatusId", 130);
			GridEX_Helper.SetColumnWidth(grd, "FumigationActivity", 150);
			grd.FrozenColumns = 3;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridComboBind()
	{
		try
		{
			if (dtCommonCombo != null && dtCommonCombo.Rows.Count != 0)
			{
				Dictionary<string, DataTable> grouped = (from x in dtCommonCombo.AsEnumerable()
					group x by x.Field<string>("ComboType")).ToDictionary((IGrouping<string, DataRow> g) => g.Key, (IGrouping<string, DataRow> g) => g.CopyToDataTable());
				if (grouped.ContainsKey("AnalysisSteps"))
				{
					GridEX_Helper.ComboBind(grd.RootTable.Columns["AnalysisStepId"], grouped["AnalysisSteps"], "Id", "Name", "Analysis Step", 100);
				}
				if (grouped.ContainsKey("StatusAfterCompletion"))
				{
					GridEX_Helper.ComboBind(grd.RootTable.Columns["StatusAfterCompletionId"], grouped["StatusAfterCompletion"], "Id", "Name", "Status After Completion", 130);
				}
				if (grouped.ContainsKey("ReleaseStatus"))
				{
					GridEX_Helper.ComboBind(grd.RootTable.Columns["StockRelaseHoldStatusId"], grouped["ReleaseStatus"], "Id", "Name", "Stock Relase Hold Status", 130);
				}
				DataTable dtjobLot = CommonBindings.JoblotBindFromGlobal();
				GridEX_Helper.ComboBind(grd.RootTable.Columns["JobLotId"], dtjobLot, "Id", "JobLot", "Fumigation Lot No", 130);
				DataTable dtFumigationMode = DatatableHelper.CreateTableForCombo();
				dtFumigationMode.Rows.Add(1, "Insert");
				dtFumigationMode.Rows.Add(2, "Update");
				GridEX_Helper.ComboBind(grd.RootTable.Columns["FumigationModeId"], dtFumigationMode, "Id", "name", "Fumigation Mode", 80);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_ColumnHeaderClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow currentRow = grd.CurrentRow;
			if (currentRow != null && (int)currentRow.RowType == 0 && currentRow.Cells.Count != 0 && e.Column != null)
			{
				_ = e.Column.Key == "AddLot";
				grd.UpdateData();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_CellValueChanged(object sender, ColumnActionEventArgs e)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (_isUpdatingGrid)
			{
				return;
			}
			GridEXRow currentRow = grd.CurrentRow;
			if (currentRow == null || (int)currentRow.RowType != 0 || currentRow.Cells.Count == 0 || e.Column == null)
			{
				return;
			}
			string ColumnName = e.Column.Key;
			if (ColumnName == "JobLotId")
			{
				UpdateFumigationDataByJobLot(currentRow);
			}
			if (ColumnName == "FumigatedDate" || ColumnName == "FumigatedDays")
			{
				currentRow.Cells["CompletionDueDate"].Value = Conversion.ToDateTime(currentRow.Cells["FumigatedDate"].Value).AddDays(Conversion.ToInt(currentRow.Cells["FumigatedDays"].Value));
			}
			if (ColumnName == "StatusAfterCompletionId" && Conversion.ToInt(currentRow.Cells["StatusAfterCompletionId"].Value) != 2)
			{
				currentRow.Cells["StockRelaseHoldStatusId"].Value = 1;
			}
			if (ColumnName == "AnalysisStepId")
			{
				int AnalysisStepId = Conversion.ToInt(currentRow.Cells["AnalysisStepId"].Value);
				if (AnalysisStepId == 1 && Conversion.ToInt(currentRow.Cells["Id"].Value) > 0)
				{
					AnalysisStepId = 2;
					currentRow.Cells["AnalysisStepId"].Value = AnalysisStepId;
				}
				if (AnalysisStepId == 1)
				{
					currentRow.Cells["StockRelaseHoldStatusId"].Value = 1;
				}
			}
			if (ColumnName != "JobLotId")
			{
				UpdateFumigationDataByJobLotInRows(currentRow);
			}
			grd.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UpdateFumigationDataByJobLotInRows(GridEXRow r)
	{
		try
		{
			_isUpdatingGrid = true;
			int jobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
			if (jobLotId <= 0)
			{
				return;
			}
			foreach (GridEXRow row in from x in grd.GetDataRows()
				where Conversion.ToInt(x.Cells["JobLotId"].Value) == jobLotId
				select x)
			{
				if (row.RowIndex != r.RowIndex)
				{
					row.BeginEdit();
					row.Cells["FumigationModeId"].Value = r.Cells["FumigationModeId"].Value;
					row.Cells["AnalysisStepId"].Value = r.Cells["AnalysisStepId"].Value;
					row.Cells["FumigatedDate"].Value = r.Cells["FumigatedDate"].Value;
					row.Cells["FumigatedDays"].Value = r.Cells["FumigatedDays"].Value;
					row.Cells["CompletionDueDate"].Value = r.Cells["CompletionDueDate"].Value;
					row.Cells["StatusAfterCompletionId"].Value = r.Cells["StatusAfterCompletionId"].Value;
					row.Cells["NextFumigationPlanDate"].Value = r.Cells["NextFumigationPlanDate"].Value;
					row.Cells["FumigatedBy"].Value = r.Cells["FumigatedBy"].Value;
					row.Cells["SupervisorCheckBy"].Value = r.Cells["SupervisorCheckBy"].Value;
					row.Cells["VerifiedBy"].Value = r.Cells["VerifiedBy"].Value;
					row.Cells["StockRelaseHoldStatusId"].Value = r.Cells["StockRelaseHoldStatusId"].Value;
					row.Cells["FumigationActivity"].Value = r.Cells["FumigationActivity"].Value;
					row.EndEdit();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		finally
		{
			_isUpdatingGrid = false;
		}
	}

	private void UpdateFumigationDataByJobLot(GridEXRow r)
	{
		try
		{
			_isUpdatingGrid = true;
			int jobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
			if (jobLotId > 0)
			{
				GridEXRow row = (from rrr in grd.GetDataRows()
					where Conversion.ToInt(rrr.Cells["JobLotId"].Value) == jobLotId && rrr.RowIndex != r.RowIndex
					select rrr).FirstOrDefault();
				if (row != null)
				{
					r.Cells["AnalysisStepId"].Value = ((Conversion.ToInt(r.Cells["Id"].Value) == 0) ? ((object)1) : row.Cells["AnalysisStepId"].Value);
					r.Cells["FumigatedDate"].Value = row.Cells["FumigatedDate"].Value;
					r.Cells["FumigatedDays"].Value = row.Cells["FumigatedDays"].Value;
					r.Cells["CompletionDueDate"].Value = row.Cells["CompletionDueDate"].Value;
					r.Cells["StatusAfterCompletionId"].Value = row.Cells["StatusAfterCompletionId"].Value;
					r.Cells["NextFumigationPlanDate"].Value = row.Cells["NextFumigationPlanDate"].Value;
					r.Cells["FumigatedBy"].Value = row.Cells["FumigatedBy"].Value;
					r.Cells["SupervisorCheckBy"].Value = row.Cells["SupervisorCheckBy"].Value;
					r.Cells["VerifiedBy"].Value = row.Cells["VerifiedBy"].Value;
					r.Cells["StockRelaseHoldStatusId"].Value = row.Cells["StockRelaseHoldStatusId"].Value;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		finally
		{
			_isUpdatingGrid = false;
		}
	}

	private void grd_Click(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow currentRow = grd.CurrentRow;
			if (currentRow != null && (int)currentRow.RowType == 0 && currentRow.Cells.Count != 0)
			{
				ApplyRowWiseEditRules(currentRow);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow currentRow = grd.CurrentRow;
			if (currentRow != null && (int)currentRow.RowType == 0 && currentRow.Cells.Count != 0)
			{
				ApplyRowWiseEditRules(currentRow);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ApplyRowWiseEditRules(GridEXRow row)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		if (row != null && (int)row.RowType == 0)
		{
			int num = Conversion.ToInt(row.Cells["Id"].Value);
			int analysisStepId = Conversion.ToInt(row.Cells["AnalysisStepId"].Value);
			int statusAfter = Conversion.ToInt(row.Cells["StatusAfterCompletionId"].Value);
			if (num == 0)
			{
				grd.RootTable.Columns["AnalysisStepId"].EditType = (EditType)0;
				grd.RootTable.Columns["FumigationModeId"].EditType = (EditType)0;
			}
			else
			{
				grd.RootTable.Columns["AnalysisStepId"].EditType = (EditType)4;
				grd.RootTable.Columns["FumigationModeId"].EditType = (EditType)4;
			}
			if (statusAfter == 2)
			{
				grd.RootTable.Columns["StockRelaseHoldStatusId"].EditType = (EditType)4;
			}
			else
			{
				grd.RootTable.Columns["StockRelaseHoldStatusId"].EditType = (EditType)0;
			}
			if (analysisStepId <= 1)
			{
				grd.RootTable.Columns["StatusAfterCompletionId"].EditType = (EditType)0;
			}
			else
			{
				grd.RootTable.Columns["StatusAfterCompletionId"].EditType = (EditType)4;
			}
		}
	}

	private void btnUpdate_Click(object sender, EventArgs e)
	{
		Insert();
	}

	private void FillDetailListCommonForInsertAndDelete(labIPmActivityLog item, GridEXRow r)
	{
		item.organizationId = UserAccount.OrganizationId;
		item.companyId = UserAccount.CompanyId;
		item.branchId = UserAccount.BranchesId;
		item.financialYearId = clsGlobalVariables.ActiveYr.Id;
		item.documentTypeId = DocumentTypeId;
		item.entryUserId = UserAccount.ID;
		item.entryDate = DateTime.Now;
		item.modifyUserId = UserAccount.ID;
		item.modifyDate = DateTime.Now;
		item.isApproved = false;
		item.refDocumentTypeId = Conversion.ToInt(r.Cells["RefDocumentTypeId"].Value);
		item.refDocumentHeaderId = Conversion.ToInt(r.Cells["RefDocIdNo"].Value);
		item.refDocumentDetialId = Conversion.ToInt(r.Cells["RefDocSubIdNo"].Value);
		item.FumigationLotNo = Conversion.ToString((object)r.Cells["JobLotId"].Text);
		item.qcActivityStepId = Conversion.ToInt(r.Cells["AnalysisStepId"].Value);
		item.qcActivityDate = Conversion.ToDateTime(r.Cells["FumigatedDate"].Value);
		item.activityForDays = Conversion.ToInt(r.Cells["FumigatedDays"].Value);
		item.completionDueDate = Conversion.ToDateTime(r.Cells["CompletionDueDate"].Value);
		item.qcActivityStatusAfterCompletionId = Conversion.ToInt(r.Cells["StatusAfterCompletionId"].Value);
		item.nextActivityPlanDate = Conversion.ToDateTime(r.Cells["NextFumigationPlanDate"].Value);
		item.fumigatedBy = Conversion.ToString(r.Cells["FumigatedBy"].Value);
		item.checkBy = Conversion.ToString(r.Cells["SupervisorCheckBy"].Value);
		item.verifiedBy = Conversion.ToString(r.Cells["VerifiedBy"].Value);
		item.qcActivityStatus = 1;
		item.stockReleaseHoldStatusId = Conversion.ToInt(r.Cells["StockRelaseHoldStatusId"].Value);
		item.qcActivityDescription = Conversion.ToString(r.Cells["FumigationActivity"].Value);
		item.attachmentsValues = "";
		item.customAttachmentsValues = "";
	}

	private void Insert()
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected O, but got Unknown
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Expected O, but got Unknown
		try
		{
			if (grd.GetDataRows().Count() == 0)
			{
				throw new Exception("No records found in grid");
			}
			if (!FormHelper.ConfirmAction("Are you sure to Update?"))
			{
				return;
			}
			labIPmActivityLog obj = new labIPmActivityLog();
			obj.organizationId = UserAccount.OrganizationId;
			obj.companyId = UserAccount.CompanyId;
			obj.branchId = UserAccount.BranchesId;
			obj.financialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.documentTypeId = DocumentTypeId;
			obj.labIPmActivityLogMasterId = labIPmActivityLog.GenerateMasterId(obj.organizationId, obj.companyId, obj.branchId, obj.financialYearId);
			obj.labIPmActivityLogId = 0;
			obj.entryUserId = UserAccount.ID;
			obj.entryDate = DateTime.Now;
			obj.modifyUserId = UserAccount.ID;
			obj.modifyDate = DateTime.Now;
			obj.isApproved = false;
			int LatestMasterId = obj.labIPmActivityLogMasterId;
			foreach (IGrouping<int, GridEXRow> item2 in from r in (from r in grd.GetDataRows()
					where Conversion.ToInt(r.Cells["JobLotId"].Value) > 0
					select r).ToList()
				group r by Conversion.ToInt(r.Cells["JobLotId"].Value))
			{
				int jobLotId = item2.Key;
				int existingMasterId = Conversion.ToInt(item2.First().Cells["MasterId"].Value);
				int existReleaseStatusId = Conversion.ToInt(item2.First().Cells["StockRelaseHoldStatusId"].Value);
				int FumigationModeId = Conversion.ToInt(item2.First().Cells["FumigationModeId"].Value);
				FumigationModeId = ((existReleaseStatusId == 2) ? existReleaseStatusId : FumigationModeId);
				bool isUpdate = existingMasterId > 0 && FumigationModeId == 2;
				int masterIdForThisJobLot = (isUpdate ? existingMasterId : LatestMasterId++);
				foreach (GridEXRow r2 in item2)
				{
					labIPmActivityLog item = new labIPmActivityLog();
					item.labIPmActivityLogMasterId = masterIdForThisJobLot;
					bool isUpdateDetail = isUpdate && Conversion.ToInt(r2.Cells["Id"].Value) > 0;
					item.labIPmActivityLogId = (isUpdateDetail ? Conversion.ToInt(r2.Cells["Id"].Value) : 0);
					item.actionId = ((!isUpdateDetail) ? 1 : 2);
					item.jobLotIdIpm = jobLotId;
					FillDetailListCommonForInsertAndDelete(item, r2);
					FormHelper.ValidateField(item.refDocumentTypeId, "Reference Document Type", r2.RowIndex);
					FormHelper.ValidateField(item.refDocumentHeaderId, "Reference Document", r2.RowIndex);
					FormHelper.ValidateField(item.jobLotIdIpm, "Fumigation Lot No", r2.RowIndex);
					FormHelper.ValidateField(item.qcActivityStepId, "Analysis Step", r2.RowIndex);
					FormHelper.ValidateField(item.qcActivityStatusAfterCompletionId, "Status After Completion", r2.RowIndex);
					FormHelper.ValidateField(item.nextActivityPlanDate, "Next Fumigation Plan Date", r2.RowIndex);
					FormHelper.ValidateField(item.fumigatedBy, "Fumigated By", r2.RowIndex);
					FormHelper.ValidateField(item.checkBy, "Supervisor Check By", r2.RowIndex);
					FormHelper.ValidateField(item.verifiedBy, "Verified By", r2.RowIndex);
					FormHelper.ValidateField(item.stockReleaseHoldStatusId, "Stock Release Hold Status", r2.RowIndex);
					if (item.completionDueDate < item.qcActivityDate)
					{
						throw new Exception($"Completion Due Date cannot be less than Analysis Date at Row {r2.RowIndex + 1}");
					}
					if (item.nextActivityPlanDate < item.qcActivityDate)
					{
						throw new Exception($"Next Fumigation Plan Date cannot be less than Analysis Date at Row {r2.RowIndex + 1}");
					}
					if (item.activityForDays <= 0)
					{
						throw new Exception($"Fumigated Days must be greater than zero at Row {r2.RowIndex + 1}");
					}
					obj.LabIPmActivityLogList.Add(item);
				}
			}
			labIPmActivityLog.save(obj);
			MessageBox.Show("Updated Successfully");
			Reset();
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
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grd, dtGrid);
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
			Reset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Reset()
	{
		cmbMainType.Focus();
		((Control)(object)CmbSupplier).Text = string.Empty;
		((Control)(object)CmbItemName).Text = string.Empty;
		grd.DataSource = null;
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			dtCommonCombo = labIPmActivityLog.CommonCombo((string)null);
			ComboDbCall();
			ParentCategoryComboFill();
			ComboBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtUom_KeyPress(object sender, KeyPressEventArgs e)
	{
		CommonServices.OnlytextNumberFunction(sender, e);
	}

	private void frmLabActivityIPM_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				if (btnUpdate.Visible && btnUpdate.Enabled)
				{
					btnUpdate_Click(null, null);
				}
			}
			else if (e.Control && e.KeyCode == Keys.U)
			{
				if (btnUpdate.Visible && btnUpdate.Enabled)
				{
					btnUpdate_Click(null, null);
				}
			}
			else
			{
				if (e.Control && e.KeyCode == Keys.P)
				{
					return;
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnNew_Click(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefresh_Click(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.F5)
				{
					FromDate.Focus();
				}
				else
				{
					if (e.Control && e.KeyCode == Keys.F10)
					{
						return;
					}
					if (e.Control && e.KeyCode == Keys.Down)
					{
						((Control)(object)grd).Focus();
					}
					else if (e.Control && e.KeyCode == Keys.Right)
					{
						if (((Control)(object)grd).Focused)
						{
							((Control)(object)grd).Focus();
						}
					}
					else if (e.Control && e.KeyCode == Keys.Up)
					{
						FromDate.Focus();
					}
				}
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
			dt.Rows.Add("Ctrl+S", "For Save When on Entry form and For Show Data when on History Form");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+Shift+Delete", "For Delete");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Ctrl+F5", "For Focus on Inquiry Date");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Parent Item");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On on Inquiry Date");
			dt.Rows.Add("Ctrl+Enter", "When Focus On Any Grid For Update Record");
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

	private void btnShortcutKeys_Click(object sender, EventArgs e)
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
		//IL_031f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0329: Expected O, but got Unknown
		//IL_0340: Unknown result type (might be due to invalid IL or missing references)
		//IL_034a: Expected O, but got Unknown
		//IL_0361: Unknown result type (might be due to invalid IL or missing references)
		//IL_036b: Expected O, but got Unknown
		//IL_0377: Unknown result type (might be due to invalid IL or missing references)
		//IL_0381: Expected O, but got Unknown
		//IL_038d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0397: Expected O, but got Unknown
		//IL_03a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ad: Expected O, but got Unknown
		//IL_03cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d9: Expected O, but got Unknown
		//IL_03da: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e4: Expected O, but got Unknown
		//IL_03f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fa: Expected O, but got Unknown
		//IL_0411: Unknown result type (might be due to invalid IL or missing references)
		//IL_041b: Expected O, but got Unknown
		//IL_0754: Unknown result type (might be due to invalid IL or missing references)
		//IL_075e: Expected O, but got Unknown
		//IL_076b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0775: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance12 = new Appearance();
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
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.IPM.frmLabActivityIPM2));
		this.panel1 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.grd = new GridEX();
		this.label23 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.cmbMainType = new UltraCombo();
		this.label8 = new System.Windows.Forms.Label();
		this.txtUom = new System.Windows.Forms.TextBox();
		this.CmbWarehouse = new UltraCombo();
		this.label12 = new System.Windows.Forms.Label();
		this.CmbItemType = new UltraCombo();
		this.label11 = new System.Windows.Forms.Label();
		this.CmbItemCategory = new UltraCombo();
		this.label7 = new System.Windows.Forms.Label();
		this.CmbCropYear = new UltraCombo();
		this.label6 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.CmbReferenceDocument = new UltraCombo();
		this.CmbJobLot = new UltraCombo();
		this.label4 = new System.Windows.Forms.Label();
		this.CmbItemName = new UltraCombo();
		this.label10 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.CmbSupplier = new UltraCombo();
		this.btngrnlod = new System.Windows.Forms.Button();
		this.Todate = new System.Windows.Forms.DateTimePicker();
		this.FromDate = new System.Windows.Forms.DateTimePicker();
		this.label9 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.btnShortcutKeys = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbMainType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWarehouse).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemCategory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYear).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbReferenceDocument).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplier).BeginInit();
		this.panel3.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		base.SuspendLayout();
		this.panel1.BackColor = System.Drawing.Color.Teal;
		this.panel1.Controls.Add(this.ctrlGrdBar1);
		this.panel1.Location = new System.Drawing.Point(0, 143);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1079, 27);
		this.panel1.TabIndex = 0;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1031, 1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 24);
		this.ctrlGrdBar1.TabIndex = 10;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.grd.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 170);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(291, 124);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 1;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.CellUpdated += new ColumnActionEventHandler(grd_CellValueChanged);
		this.grd.ColumnHeaderClick += new ColumnActionEventHandler(grd_ColumnHeaderClick);
		this.grd.SelectionChanged += new System.EventHandler(grd_SelectionChanged);
		((System.Windows.Forms.Control)(object)this.grd).Click += new System.EventHandler(grd_Click);
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(3, 28);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(142, 19);
		this.label23.TabIndex = 24;
		this.label23.Text = "IPM Lab Activity";
		this.panel2.BackColor = System.Drawing.Color.White;
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbMainType);
		this.panel2.Controls.Add(this.label8);
		this.panel2.Controls.Add(this.txtUom);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWarehouse);
		this.panel2.Controls.Add(this.label12);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemType);
		this.panel2.Controls.Add(this.label11);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemCategory);
		this.panel2.Controls.Add(this.label7);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCropYear);
		this.panel2.Controls.Add(this.label6);
		this.panel2.Controls.Add(this.label5);
		this.panel2.Controls.Add(this.label3);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbReferenceDocument);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobLot);
		this.panel2.Controls.Add(this.label4);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemName);
		this.panel2.Controls.Add(this.label10);
		this.panel2.Controls.Add(this.label2);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSupplier);
		this.panel2.Controls.Add(this.btngrnlod);
		this.panel2.Controls.Add(this.Todate);
		this.panel2.Controls.Add(this.FromDate);
		this.panel2.Controls.Add(this.label9);
		this.panel2.Controls.Add(this.label1);
		this.panel2.Location = new System.Drawing.Point(0, 50);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1079, 93);
		this.panel2.TabIndex = 0;
		this.cmbMainType.AllowNull = (DefaultableBoolean)1;
		this.cmbMainType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbMainType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbMainType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbMainType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.cmbMainType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbMainType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbMainType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbMainType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance54;
		((SpecialBoxBase)((UltraGridBase)this.cmbMainType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance56).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance56).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbMainType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.cmbMainType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbMainType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbMainType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance57;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbMainType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.cmbMainType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbMainType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbMainType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance60).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbMainType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.cmbMainType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbMainType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance61).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbMainType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbMainType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.cmbMainType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbMainType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbMainType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.cmbMainType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbMainType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.cmbMainType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbMainType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbMainType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbMainType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.cmbMainType).Location = new System.Drawing.Point(96, 61);
		((System.Windows.Forms.Control)(object)this.cmbMainType).Name = "cmbMainType";
		((System.Windows.Forms.Control)(object)this.cmbMainType).Size = new System.Drawing.Size(140, 26);
		((System.Windows.Forms.Control)(object)this.cmbMainType).TabIndex = 97;
		((System.Windows.Forms.Control)(object)this.cmbMainType).Leave += new System.EventHandler(cmbMainType_Leave);
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(5, 67);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(91, 15);
		this.label8.TabIndex = 98;
		this.label8.Text = "Parent Category";
		this.txtUom.Font = new System.Drawing.Font("Verdana", 11.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtUom.Location = new System.Drawing.Point(945, 61);
		this.txtUom.Name = "txtUom";
		this.txtUom.Size = new System.Drawing.Size(105, 26);
		this.txtUom.TabIndex = 51;
		this.txtUom.Tag = "double";
		this.txtUom.Text = "65";
		this.txtUom.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtUom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtUom_KeyPress);
		this.CmbWarehouse.AllowNull = (DefaultableBoolean)1;
		this.CmbWarehouse.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWarehouse.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWarehouse.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWarehouse).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.CmbWarehouse).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWarehouse).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWarehouse.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWarehouse).Location = new System.Drawing.Point(623, 3);
		((System.Windows.Forms.Control)(object)this.CmbWarehouse).Name = "CmbWarehouse";
		((System.Windows.Forms.Control)(object)this.CmbWarehouse).Size = new System.Drawing.Size(230, 26);
		((System.Windows.Forms.Control)(object)this.CmbWarehouse).TabIndex = 6;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(240, 38);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(60, 15);
		this.label12.TabIndex = 50;
		this.label12.Text = "Item Type";
		this.CmbItemType.AllowNull = (DefaultableBoolean)1;
		this.CmbItemType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbItemType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbItemType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance34).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance35).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance35).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance35).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance35;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbItemType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemType).Location = new System.Drawing.Point(322, 32);
		((System.Windows.Forms.Control)(object)this.CmbItemType).Name = "CmbItemType";
		((System.Windows.Forms.Control)(object)this.CmbItemType).Size = new System.Drawing.Size(230, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemType).TabIndex = 4;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(240, 9);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(82, 15);
		this.label11.TabIndex = 48;
		this.label11.Text = "Item Category";
		this.CmbItemCategory.AllowNull = (DefaultableBoolean)1;
		this.CmbItemCategory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemCategory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemCategory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance107).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance107).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance107).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemCategory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance107;
		((AppearanceBase)appearance108).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance108;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemCategory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemCategory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemCategory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemCategory).Location = new System.Drawing.Point(322, 3);
		((System.Windows.Forms.Control)(object)this.CmbItemCategory).Name = "CmbItemCategory";
		((System.Windows.Forms.Control)(object)this.CmbItemCategory).Size = new System.Drawing.Size(230, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemCategory).TabIndex = 3;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(557, 67);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(57, 15);
		this.label7.TabIndex = 36;
		this.label7.Text = "Crop Year";
		this.CmbCropYear.AllowNull = (DefaultableBoolean)1;
		this.CmbCropYear.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCropYear.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCropYear.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Appearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance67).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance67).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance67).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance67;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance68;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance73).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance74).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCropYear.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Location = new System.Drawing.Point(623, 61);
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Name = "CmbCropYear";
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Size = new System.Drawing.Size(230, 26);
		((System.Windows.Forms.Control)(object)this.CmbCropYear).TabIndex = 8;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(240, 67);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(67, 15);
		this.label6.TabIndex = 34;
		this.label6.Text = "Item Name";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(557, 9);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(67, 15);
		this.label5.TabIndex = 33;
		this.label5.Text = "Warehouse";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(858, 9);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(84, 15);
		this.label3.TabIndex = 31;
		this.label3.Text = "Ref Document";
		this.CmbReferenceDocument.AllowNull = (DefaultableBoolean)1;
		this.CmbReferenceDocument.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbReferenceDocument.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbReferenceDocument.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbReferenceDocument).DisplayLayout.Appearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.CmbReferenceDocument).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbReferenceDocument).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance93).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance93).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbReferenceDocument).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbReferenceDocument).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance94;
		((SpecialBoxBase)((UltraGridBase)this.CmbReferenceDocument).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance95).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance95).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbReferenceDocument).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.CmbReferenceDocument).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbReferenceDocument).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance96).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbReferenceDocument).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance97).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbReferenceDocument).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.CmbReferenceDocument).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbReferenceDocument).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbReferenceDocument).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance98;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance100).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbReferenceDocument).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance100;
		((UltraGridBase)this.CmbReferenceDocument).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbReferenceDocument).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance101).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance101).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance101).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance101).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbReferenceDocument).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance101;
		((AppearanceBase)appearance102).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbReferenceDocument).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.CmbReferenceDocument).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbReferenceDocument).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbReferenceDocument).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.CmbReferenceDocument).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbReferenceDocument).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.CmbReferenceDocument).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbReferenceDocument).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbReferenceDocument).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbReferenceDocument).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbReferenceDocument.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbReferenceDocument).Location = new System.Drawing.Point(945, 3);
		((System.Windows.Forms.Control)(object)this.CmbReferenceDocument).Name = "CmbReferenceDocument";
		((System.Windows.Forms.Control)(object)this.CmbReferenceDocument).Size = new System.Drawing.Size(228, 26);
		((System.Windows.Forms.Control)(object)this.CmbReferenceDocument).TabIndex = 9;
		this.CmbJobLot.AllowNull = (DefaultableBoolean)1;
		this.CmbJobLot.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobLot.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobLot.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Appearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance41;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbJobLot.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Location = new System.Drawing.Point(623, 32);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Name = "CmbJobLot";
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Size = new System.Drawing.Size(230, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).TabIndex = 7;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(557, 38);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(46, 15);
		this.label4.TabIndex = 28;
		this.label4.Text = "Job Lot";
		this.CmbItemName.AllowNull = (DefaultableBoolean)1;
		this.CmbItemName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance79).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.CmbItemName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance80).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance80).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance81;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance82).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance82).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance86).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance87).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance87).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance87).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance87;
		((AppearanceBase)appearance89).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance90).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Location = new System.Drawing.Point(322, 61);
		((System.Windows.Forms.Control)(object)this.CmbItemName).Name = "CmbItemName";
		((System.Windows.Forms.Control)(object)this.CmbItemName).Size = new System.Drawing.Size(230, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemName).TabIndex = 5;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 11f, System.Drawing.FontStyle.Bold);
		this.label10.ForeColor = System.Drawing.Color.Blue;
		this.label10.Location = new System.Drawing.Point(851, 64);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(97, 20);
		this.label10.TabIndex = 24;
		this.label10.Text = "Uom For Qty";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(858, 38);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(69, 15);
		this.label2.TabIndex = 24;
		this.label2.Text = "Party Name";
		this.CmbSupplier.AllowNull = (DefaultableBoolean)1;
		this.CmbSupplier.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSupplier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSupplier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance33).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance33).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance44;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance66).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance77).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance88;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance99).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance105).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance105).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance105).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance105;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Location = new System.Drawing.Point(945, 32);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Name = "CmbSupplier";
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Size = new System.Drawing.Size(228, 26);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).TabIndex = 10;
		this.btngrnlod.BackColor = System.Drawing.Color.Teal;
		this.btngrnlod.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btngrnlod.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btngrnlod.ForeColor = System.Drawing.Color.White;
		this.btngrnlod.Location = new System.Drawing.Point(1108, 61);
		this.btngrnlod.Name = "btngrnlod";
		this.btngrnlod.Size = new System.Drawing.Size(65, 26);
		this.btngrnlod.TabIndex = 11;
		this.btngrnlod.Text = "Show";
		this.btngrnlod.UseVisualStyleBackColor = false;
		this.btngrnlod.Click += new System.EventHandler(BtnShow_Click);
		this.Todate.CustomFormat = "dd-MMM-yy";
		this.Todate.Font = new System.Drawing.Font("Verdana", 9f);
		this.Todate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.Todate.Location = new System.Drawing.Point(96, 34);
		this.Todate.Name = "Todate";
		this.Todate.Size = new System.Drawing.Size(140, 22);
		this.Todate.TabIndex = 1;
		this.FromDate.CustomFormat = "dd-MMM-yy";
		this.FromDate.Font = new System.Drawing.Font("Verdana", 9f);
		this.FromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDate.Location = new System.Drawing.Point(96, 5);
		this.FromDate.Name = "FromDate";
		this.FromDate.Size = new System.Drawing.Size(140, 22);
		this.FromDate.TabIndex = 0;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(5, 38);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(48, 15);
		this.label9.TabIndex = 12;
		this.label9.Text = "To Date";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(5, 9);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(63, 15);
		this.label1.TabIndex = 12;
		this.label1.Text = "From Date";
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.toolStrip1);
		this.panel3.Controls.Add(this.label23);
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1079, 50);
		this.panel3.TabIndex = 31;
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[7] { this.toolStripSeparator6, this.btnNew, this.btnRefresh, this.btnUpdate, this.toolStripSeparator2, this.btnShortcutKeys, this.toolStripSeparator1 });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1079, 25);
		this.toolStrip1.TabIndex = 28;
		this.toolStrip1.Text = "toolStrip1";
		this.toolStripSeparator6.Name = "toolStripSeparator6";
		this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(55, 22);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(76, 22);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnUpdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(60, 22);
		this.btnUpdate.Text = "Save";
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
		this.btnShortcutKeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortcutKeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortcutKeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortcutKeys.Name = "btnShortcutKeys";
		this.btnShortcutKeys.Size = new System.Drawing.Size(122, 22);
		this.btnShortcutKeys.Text = "ShortCut Keys";
		this.btnShortcutKeys.Click += new System.EventHandler(btnShortcutKeys_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1184, 461);
		base.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		base.Controls.Add(this.panel1);
		base.Controls.Add(this.panel2);
		base.Controls.Add(this.panel3);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Name = "frmLabActivityIPM2";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(LoadInvoices_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmLabActivityIPM_KeyDown);
		this.panel1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbMainType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWarehouse).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemCategory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYear).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbReferenceDocument).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplier).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		base.ResumeLayout(false);
	}
}
