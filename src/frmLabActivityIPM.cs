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
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Production;
using Architecture.WinApp.Properties;
using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.IPM
{

public class frmLabActivityIPM : Form
{
	private DropDownBind DDL = new DropDownBind();

	private Rightsobjects formright = new Rightsobjects();

	private UserAccount UserAccount = new UserAccount();

	private DataTable dtFumigationDetail = new DataTable();

	private DataTable dtStockDetail = new DataTable();

	private DataTable dtHistoryForLayout = new DataTable();

	private DataTable dtCommonCombo = new DataTable();

	private DataTable dtHistoryCombo = new DataTable();

	private bool FormInitialized;

	private bool ItemSearchWithNameOrCode = true;

	private bool _isUpdatingGrid;

	private int RecId;

	private int DocumentTypeId = 2001;

	private string AttachmentsValues = "";

	private string CustomAttachmentsValues = "";

	private string ScreenName = "frmLabActivityIPM";

	private int DefaultDaysToLessFromHistoryFromDate;

	private List<labIPmActivityLog> ReadByIdList = new List<labIPmActivityLog>();

	private IContainer components;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton BtnRefresh;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripButton btnsave;

	private ToolStripButton btnUpdate;

	private ToolStripButton BtnShortCutkeys;

	private ToolStripSeparator toolStripSeparator2;

	private Panel panel3;

	private Label label23;

	private CheckBox chkPrint;

	private ToolStripButton btnPrint;

	private ToolStrip toolStrip2;

	private ToolStripButton btnNewHistory;

	private ToolStripSeparator toolStripSeparator3;

	private ToolStripButton BtnRefreshHistory;

	private Panel panel4;

	private Label label30;

	private Panel panel13;

	private GroupBox groupBox1;

	private Button btnshow;

	private DateTimePicker ToDateHistory;

	private Label label33;

	private DateTimePicker FromDateHistory;

	internal UltraCombo CmbParentItemHistory;

	private Label label36;

	private Panel panel15;

	private CtrlGrdBar ctrlGrdBar1;

	private Label label43;

	internal GridEX grdHistory;

	private Panel panel5;

	private Label label42;

	private Panel panel6;

	private Label label44;

	internal GridEX grdHistoryDetail;

	private Panel panel1;

	private Label label1;

	internal GridEX grdFumigation;

	internal GridEX grdStockDetail;

	private CtrlGrdBar ctrlGrdBar2;

	internal UltraCombo CmbWarehouse;

	private Label label12;

	internal UltraCombo CmbItemType;

	private Label label11;

	internal UltraCombo CmbItemCategory;

	private Label label7;

	internal UltraCombo CmbCropYear;

	private Label label6;

	private Label label5;

	private Label label3;

	internal UltraCombo CmbReferenceDocument;

	internal UltraCombo CmbJobLot;

	private Label label4;

	internal UltraCombo CmbItemName;

	private Label label2;

	internal UltraCombo CmbSupplier;

	private Label label8;

	private CtrlGrdBar ctrlGrdBar3;

	private RadioButton rdmodifydate;

	private RadioButton rdentrydate;

	private RadioButton drdocdate;

	private ToolStripButton btnStockLoader;

	private Label label9;

	internal UltraCombo CmbStatus;

	public frmLabActivityIPM(UserAccount user)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
		InitializeComponentCustom();
		InitializeComponentMethod();
		FormHelper.FormEventHandler(this);
	}

	private void frmLabActivityIPM_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(10, 10);
			base.AutoScrollMinSize = new Size(1100, 800);
			btnPrint.Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InitializeComponentCustom()
	{
		try
		{
			ScreenName = Conversion.ToString((object)base.Name);
			frmLabActivityIPM_Helper.InitializeFumigationTable(dtFumigationDetail);
			frmLabActivityIPM_Helper.InitializeStockTable(dtStockDetail);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private async void InitializeComponentMethod()
	{
		try
		{
			Task<Rightsobjects> rightsTask = Task.Run(() => CommonServices.SetRightsValueInRightsObject(ScreenName));
			Task<DataTable> comboTask = Task.Run(() => labIPmActivityLog.CommonCombo((string)null));
			Task historyTask = Task.Run(delegate
			{
				HistoryComboDbCall();
			});
			await Task.WhenAll(rightsTask, comboTask, historyTask);
			if (!base.IsDisposed && base.IsHandleCreated)
			{
				formright = rightsTask.Result;
				dtCommonCombo = comboTask.Result;
				btnsave.Enabled = formright.DoHaveSaveRight;
				btnUpdate.Enabled = formright.DoHaveUpdateRights;
				btnPrint.Enabled = formright.DoHavePrintRights;
				GetConfigurationsFromGlobal();
				GetIPMConfigurationsFromGlobalandBind();
				BindGrids();
				ParentCategoryComboFill();
				HistoryComboBind();
				FormInitialized = true;
				((Control)(object)grdFumigation).Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error occurred while initializing form.\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void HistoryComboDbCall()
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
			dtHistoryCombo = StocksReport.Evalaution_DropDown_ByParentCategories(obj);
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
			((UltraGridBase)CmbParentItemHistory).DataSource = null;
			DataRow[] array = dtHistoryCombo.Select("ActivityType = 'ParentCategories'");
			DataTable dtMainType = DatatableHelper.CreateTableForCombo();
			DataRow[] array2 = array;
			foreach (DataRow row in array2)
			{
				dtMainType.Rows.Add(row["Id"], row["name"]);
			}
			DDL.BindDDL(dtMainType, CmbParentItemHistory, "Id", "name", "Parent Categories", ZeroIndex: false);
			UltraGridLayout layout = ((UltraGridBase)CmbParentItemHistory).DisplayLayout;
			layout.AutoFitStyle = (AutoFitStyle)1;
			if (!((KeyedSubObjectsCollectionBase)layout.Bands[0].Columns).Exists("Selected"))
			{
				UltraGridColumn obj = layout.Bands[0].Columns.Add("Selected");
				((HeaderBase)obj.Header).Caption = string.Empty;
				obj.Header.CheckBoxVisibility = (HeaderCheckBoxVisibility)2;
				obj.DataType = typeof(bool);
				((HeaderBase)obj.Header).VisiblePosition = 0;
			}
			CmbParentItemHistory.CheckedListSettings.CheckStateMember = "Selected";
			((EditorCheckedListSettings)CmbParentItemHistory.CheckedListSettings).EditorValueSource = (EditorWithComboValueSource)1;
			((EditorCheckedListSettings)CmbParentItemHistory.CheckedListSettings).ListSeparator = ",";
			((EditorCheckedListSettings)CmbParentItemHistory.CheckedListSettings).ItemCheckArea = (ItemCheckArea)1;
			RowEnumerator enumerator = ((UltraGridBase)CmbParentItemHistory).Rows.GetEnumerator();
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

	private void HistoryComboBind()
	{
		try
		{
			DataTable dtFiltered = new DataTable();
			HashSet<string> validIds = new HashSet<string>();
			if (!string.IsNullOrWhiteSpace(((Control)(object)CmbParentItemHistory).Text))
			{
				string[] array = ((Control)(object)CmbParentItemHistory).Text.Trim().Split(',');
				Dictionary<string, string> nameToIdMap = (((UltraGridBase)CmbParentItemHistory).DataSource as DataTable).AsEnumerable().ToDictionary((DataRow row) => row["name"].ToString().Trim(), (DataRow row) => row["Id"].ToString());
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
				List<DataRow> filteredRows = (from row in dtHistoryCombo.AsEnumerable()
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

	private string LotNoDbCall()
	{
		try
		{
			return labIPmActivityLog.LabIPmActivityLog_GenerateLotNo(UserAccount.OrganizationId, UserAccount.CompanyId, UserAccount.BranchesId, clsGlobalVariables.ActiveYr.Id);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CmbParentItemHistory_Leave(object sender, EventArgs e)
	{
		try
		{
			try
			{
				if (Conversion.ToString((object)((Control)(object)CmbParentItemHistory).Text) != string.Empty)
				{
					HistoryComboBind();
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

	private void GetIPMConfigurationsFromGlobalandBind()
	{
	}

	private void BindGrids()
	{
		try
		{
			DataTable dataTable = dtFumigationDetail;
			if (dataTable != null && dataTable.Columns.Count > 0)
			{
				grdFumigation.DataSource = dtFumigationDetail;
				grdFumigation.RetrieveStructure();
				grdFumigationDetailSettings();
			}
			DataTable dataTable2 = dtStockDetail;
			if (dataTable2 != null && dataTable2.Columns.Count > 0)
			{
				grdStockDetail.DataSource = dtStockDetail;
				grdStockDetail.RetrieveStructure();
				grdStockDetailSettings(grdStockDetail);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateLotNoInFumigationGrid()
	{
		try
		{
			if (grdFumigation.RowCount == 0)
			{
				return;
			}
			int nextNo = Conversion.ToInt((object)LotNoDbCall().Replace("IPM-", ""));
			GridEXRow[] dataRows = grdFumigation.GetDataRows();
			foreach (GridEXRow row in dataRows)
			{
				if (row.Cells["MasterId"].Value != null && Conversion.ToInt(row.Cells["MasterId"].Value) == 0)
				{
					row.BeginEdit();
					row.Cells["FumigationLotNo"].Value = "IPM-" + nextNo.ToString("D2");
					row.EndEdit();
					nextNo++;
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void AddNewEmptyRowInFumigationGrid()
	{
		try
		{
			if (dtFumigationDetail != null && dtFumigationDetail.Columns.Count != 0)
			{
				DataRow newRow = dtFumigationDetail.NewRow();
				newRow["MasterId"] = 0;
				newRow["AnalysisStepId"] = 1;
				newRow["StatusAfterCompletionId"] = 1;
				newRow["StockReleaseHoldStatusId"] = 1;
				dtFumigationDetail.Rows.Add(newRow);
				grdFumigation.DataSource = dtFumigationDetail;
				GenerateLotNoInFumigationGrid();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grdFumigationDetailSettings()
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "MasterId" };
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
			GridEX_Helper.GridWrappingAndColumnSettings(grdFumigation, 3, 3, hiddenColumns, editableColumns);
			GridComboBind();
			grdFumigation.RootTable.Columns["FumigationActivity"].Caption = "Fumigation Remarks";
			GridEX_Helper.SetColumnWidth(grdFumigation, "FumigationLotNo", 80);
			GridEX_Helper.SetColumnWidth(grdFumigation, "AnalysisStepId", 90);
			GridEX_Helper.SetColumnWidth(grdFumigation, "AnalysisDate", Constants.InventoryConstants.DateConstant);
			GridEX_Helper.SetColumnWidth(grdFumigation, "FumigatedDate", Constants.InventoryConstants.DateConstant);
			GridEX_Helper.SetColumnWidth(grdFumigation, "FumigatedDays", 70);
			GridEX_Helper.SetColumnWidth(grdFumigation, "CompletionDueDate", Constants.InventoryConstants.DateConstant + 10);
			GridEX_Helper.SetColumnWidth(grdFumigation, "StatusAfterCompletionId", 180);
			GridEX_Helper.SetColumnWidth(grdFumigation, "NextFumigationPlanDate", Constants.InventoryConstants.DateConstant);
			GridEX_Helper.SetColumnWidth(grdFumigation, "FumigatedBy", 120);
			GridEX_Helper.SetColumnWidth(grdFumigation, "SupervisorCheckBy", 120);
			GridEX_Helper.SetColumnWidth(grdFumigation, "VerifiedBy", 120);
			GridEX_Helper.SetColumnWidth(grdFumigation, "StockReleaseHoldStatusId", 130);
			GridEX_Helper.SetColumnWidth(grdFumigation, "FumigationActivity", 150);
			GridEX_Helper.AddButton(grdFumigation, "Delete", "X", 20, 0);
			GridEX_Helper.AddButton(grdFumigation, "Add", "+", 20, 1);
			grdFumigation.FrozenColumns = 3;
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
					GridEX_Helper.ComboBind(grdFumigation.RootTable.Columns["AnalysisStepId"], grouped["AnalysisSteps"], "Id", "Name", "Analysis Step", 100, limitToList: true, editType: false);
				}
				if (grouped.ContainsKey("StatusAfterCompletion"))
				{
					GridEX_Helper.ComboBind(grdFumigation.RootTable.Columns["StatusAfterCompletionId"], grouped["StatusAfterCompletion"], "Id", "Name", "Status After Completion", 130);
				}
				if (grouped.ContainsKey("ReleaseStatus"))
				{
					GridEX_Helper.ComboBind(grdFumigation.RootTable.Columns["StockReleaseHoldStatusId"], grouped["ReleaseStatus"], "Id", "Name", "Stock Release Hold Status", 130);
				}
				InfragisticsHelper.BindAndRetainSelection(CmbStatus, grouped["ReleaseStatus"], "Id", "name", "Item Name", AllColumns: true, CmbStatus.Value, insertDefaultRow: false, null, null, ActivateRow: false);
				grdFumigation.RootTable.Columns["AnalysisStepId"].EditType = (EditType)0;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdFumigation_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow currentRow = grdFumigation.CurrentRow;
			if (currentRow == null || (int)currentRow.RowType != 0)
			{
				return;
			}
			GridEXColumn column = e.Column;
			string columnName = ((column != null) ? column.Key : null);
			if (!string.IsNullOrEmpty(columnName))
			{
				if (columnName == "Delete")
				{
					DeleteDetailRow(currentRow);
				}
				if (columnName == "Add")
				{
					AddDetailRow(currentRow);
				}
				GenerateLotNoInFumigationGrid();
				grdFumigation.UpdateData();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeleteDetailRow(GridEXRow currentRow)
	{
		try
		{
			_isUpdatingGrid = true;
			if (Conversion.ToInt(currentRow.Cells["MasterId"].Value) > 0)
			{
				MessageBox.Show("Cannot delete this record because it is already saved.", "Delete Blocked", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			else if (MessageBox.Show("Are you sure you want to delete this record?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
			{
				currentRow.Delete();
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

	private void AddDetailRow(GridEXRow r)
	{
		try
		{
			if (dtFumigationDetail.AsEnumerable().Any((DataRow x) => Conversion.ToInt(x["MasterId"]) == 0))
			{
				MessageBox.Show("Please save the existing new row first.");
				return;
			}
			DataRow latestRow = (from x in dtFumigationDetail.AsEnumerable()
				orderby Conversion.ToInt(x["AnalysisStepId"]) descending
				select x).FirstOrDefault();
			if (latestRow != null)
			{
				int num = Conversion.ToInt(latestRow["StockReleaseHoldStatusId"]);
				DataRow row = dtCommonCombo.AsEnumerable().FirstOrDefault((DataRow rrr) => Conversion.ToInt(rrr["Id"]) == Conversion.ToInt(latestRow["AnalysisStepId"]) && Conversion.ToString(rrr["ComboType"]) == "AnalysisSteps");
				string AnalysisStep = ((row != null) ? Convert.ToString(row["name"]) : "");
				if (num == 2)
				{
					MessageBox.Show("Stock is already released in Step [" + AnalysisStep + "]. No new step can be added.");
					return;
				}
			}
			int maxStepId = 0;
			if (dtFumigationDetail.Rows.Count > 0 && dtFumigationDetail.Columns.Contains("AnalysisStepId"))
			{
				maxStepId = (from x in dtFumigationDetail.AsEnumerable()
					where x["AnalysisStepId"] != DBNull.Value
					select x).Max((DataRow x) => Conversion.ToInt(x["AnalysisStepId"]));
			}
			if (maxStepId >= 4)
			{
				MessageBox.Show("Already on last step. No more rows can be added.");
				return;
			}
			DataRow dr = dtFumigationDetail.Rows[r.RowIndex];
			DataRow newRow = dtFumigationDetail.NewRow();
			newRow.ItemArray = (object[])dr.ItemArray.Clone();
			newRow["AnalysisStepId"] = maxStepId + 1;
			newRow["MasterId"] = 0;
			dtFumigationDetail.Rows.Add(newRow);
			grdFumigation.DataSource = dtFumigationDetail;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdFumigation_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (_isUpdatingGrid)
			{
				return;
			}
			GridEXRow currentRow = grdFumigation.CurrentRow;
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
				currentRow.Cells["StockReleaseHoldStatusId"].Value = 1;
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
					currentRow.Cells["StockReleaseHoldStatusId"].Value = 1;
				}
			}
			if (ColumnName != "JobLotId")
			{
				UpdateFumigationDataByJobLotInRows(currentRow);
			}
			grdFumigation.UpdateData();
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
		}
		finally
		{
			_isUpdatingGrid = false;
		}
	}

	private void grdFumigation_Click(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow currentRow = grdFumigation.CurrentRow;
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

	private void grdFumigation_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow currentRow = grdFumigation.CurrentRow;
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
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		if (_isUpdatingGrid || row == null || (int)row.RowType != 0)
		{
			return;
		}
		int masterId = Conversion.ToInt(row.Cells["MasterId"].Value);
		int analysisStepId = Conversion.ToInt(row.Cells["AnalysisStepId"].Value);
		int statusAfter = Conversion.ToInt(row.Cells["StatusAfterCompletionId"].Value);
		int editableStepId = -1;
		bool hasNewRow = dtFumigationDetail.AsEnumerable().Any((DataRow x) => Conversion.ToInt(x["MasterId"]) == 0);
		if (dtFumigationDetail.AsEnumerable().Any((DataRow x) => Conversion.ToInt(x["MasterId"]) > 0))
		{
			editableStepId = (from x in dtFumigationDetail.AsEnumerable()
				where Conversion.ToInt(x["MasterId"]) > 0
				select x).Max((DataRow x) => Conversion.ToInt(x["AnalysisStepId"]));
		}
		bool isEditableRow = masterId == 0 || analysisStepId == editableStepId;
		grdFumigation.AllowEdit = (InheritableBoolean)(isEditableRow ? 1 : 2);
		if (statusAfter == 2 && (!hasNewRow || masterId == 0))
		{
			grdFumigation.RootTable.Columns["StockReleaseHoldStatusId"].EditType = (EditType)4;
		}
		else
		{
			grdFumigation.RootTable.Columns["StockReleaseHoldStatusId"].EditType = (EditType)0;
		}
		if (analysisStepId <= 1 && masterId == 0)
		{
			grdFumigation.RootTable.Columns["StatusAfterCompletionId"].EditType = (EditType)0;
		}
		else
		{
			grdFumigation.RootTable.Columns["StatusAfterCompletionId"].EditType = (EditType)4;
		}
	}

	private void grdFumigation_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow currentRow = grdFumigation.CurrentRow;
			if (currentRow == null || (int)currentRow.RowType != 0 || currentRow.Cells.Count == 0)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				DeleteDetailRow(currentRow);
				GenerateLotNoInFumigationGrid();
				e.Handled = true;
			}
			else if (e.Control && e.KeyCode == Keys.D)
			{
				AddDetailRow(currentRow);
				GenerateLotNoInFumigationGrid();
				e.Handled = true;
			}
			else if (e.Control && e.KeyCode == Keys.Return)
			{
				grdFumigation.UpdateData();
				e.Handled = true;
			}
			else if (e.Control && e.KeyCode == Keys.Space)
			{
				GridEXColumn col = grdFumigation.CurrentColumn;
				if (col != null && col.Key == "Delete")
				{
					DeleteDetailRow(currentRow);
					GenerateLotNoInFumigationGrid();
					e.Handled = true;
					e.SuppressKeyPress = true;
				}
				else if (col != null && col.Key == "Add")
				{
					AddDetailRow(currentRow);
					GenerateLotNoInFumigationGrid();
					e.Handled = true;
					e.SuppressKeyPress = true;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdStockDetailSettings(GridEX gridEX)
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "RefDocumentTypeId", "RefDocIdNo", "RefDocSubIdNo", "ItemId", "SupplierCustomerId", "WarehouseId", "RefWarehouse", "ManualNo", "JobLotId" };
			GridEX_Helper.GridWrappingAndColumnSettings(gridEX, 3, 3, hiddenColumns);
			GridEX_Helper.SetColumnWidth(gridEX, "RefDocDate", Constants.InventoryConstants.DateConstant);
			GridEX_Helper.SetColumnWidth(gridEX, "RefDocNo", Constants.InventoryConstants.DocNoConstant);
			GridEX_Helper.SetColumnWidth(gridEX, "RefDocumentType", Constants.InventoryConstants.DocumentTypeCode);
			GridEX_Helper.SetColumnWidth(gridEX, "VehicleNo", Constants.InventoryConstants.VehicleNoConstant);
			GridEX_Helper.SetColumnWidth(gridEX, "SupplierCustomerName", Constants.InventoryConstants.ItemName);
			GridEX_Helper.SetColumnWidth(gridEX, "GrnNo", Constants.InventoryConstants.DocNoConstant);
			GridEX_Helper.SetColumnWidth(gridEX, "ManualNo", Constants.InventoryConstants.RecordNo);
			GridEX_Helper.SetColumnWidth(gridEX, "GpNo", Constants.InventoryConstants.GpNoConstant);
			GridEX_Helper.SetColumnWidth(gridEX, "GPDate", Constants.InventoryConstants.DateConstant);
			GridEX_Helper.SetColumnWidth(gridEX, "WareHouseCode", Constants.InventoryConstants.WareHouseName - 40);
			GridEX_Helper.SetColumnWidth(gridEX, "RefWarehouse", Constants.InventoryConstants.WareHouseName - 40);
			GridEX_Helper.SetColumnWidth(gridEX, "JobLotCode", Constants.InventoryConstants.WareHouseName - 40);
			GridEX_Helper.SetColumnWidth(gridEX, "ItemName", Constants.InventoryConstants.ItemName);
			GridEX_Helper.SetColumnWidth(gridEX, "ItemCode", Constants.InventoryConstants.ItemCode - 20);
			GridEX_Helper.SetColumnWidth(gridEX, "QtyBalance", Constants.InventoryConstants.QtyConstant);
			GridEX_Helper.SetColumnWidth(gridEX, "WeightBalance", Constants.InventoryConstants.WeightConstant);
			grdStockDetail.FrozenColumns = 3;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnStockLoader_Click(object sender, EventArgs e)
	{
		try
		{
			DataTable dataTable = dtStockDetail;
			if (dataTable != null && dataTable.AsEnumerable().Any((DataRow r) => Conversion.ToInt(r["Id"]) > 0))
			{
				MessageBox.Show("Cannot load new data. Stock already contains saved records.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			using LoadavailableTransactionsForStockReleaseFromFumigation loadFumigation = new LoadavailableTransactionsForStockReleaseFromFumigation(UserAccount);
			loadFumigation.AvailableForFumigation = 1;
			loadFumigation.ShowDialog();
			if (loadFumigation != null && loadFumigation.dtIssuance?.Rows.Count > 0)
			{
				LoadDataDetailfromFumigation(loadFumigation.dtIssuance);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadDataDetailfromFumigation(DataTable dt)
	{
		try
		{
			frmLabActivityIPM_Helper.InitializeStockTable(dtStockDetail);
			if (dt == null || dt.Rows.Count == 0)
			{
				BindGrids();
				return;
			}
			HashSet<int> ids = new HashSet<int>();
			foreach (DataRow r in dtStockDetail.Rows)
			{
				ids.Add(Conversion.ToInt(r["RefDocSubIdNo"]));
			}
			foreach (DataRow row in dt.Rows)
			{
				int refDocSubIdNo = Conversion.ToInt(row["RefDocSubIdNo"]);
				if (ids.Add(refDocSubIdNo))
				{
					int docTypeId = Conversion.ToInt(row["RefDocumentTypeId"]);
					object grnNo = ((docTypeId == 112 || docTypeId == 80) ? ((object)0) : row["GrnNo"]);
					dtStockDetail.Rows.Add(0, row["RefDocumentTypeId"], row["RefDocIdNo"], refDocSubIdNo, row["RefDocumentType"], row["DocDate"], row["DocCodeNo"], row["VehicleNo"], row["ItemId"], row["ItemCode"], row["ItemName"], row["QtyBalance"], row["WeightBalance"], grnNo, row["GpNo"], row["GPDate"], row["SupplierCustomerId"], row["SupplierCustomerName"], row["WarehouseId"], row["WareHouseCode"], row["RefWarehouse"], row["JobLotId"], row["JobLotCode"], row["ManualNo"]);
				}
			}
			BindGrids();
			if (dtFumigationDetail.Rows.Count == 0)
			{
				AddNewEmptyRowInFumigationGrid();
			}
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
			Reset();
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
			RecId = 0;
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			ReadByIdList.Clear();
			dtFumigationDetail.Clear();
			dtStockDetail.Clear();
			BindGrids();
			GetIPMConfigurationsFromGlobalandBind();
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
			dtCommonCombo = labIPmActivityLog.CommonCombo((string)null);
			GridComboBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmLabActivityIPM_FormClosing(object sender, FormClosingEventArgs e)
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
		item.stockReleaseHoldStatusId = Conversion.ToInt(r.Cells["StockReleaseHoldStatusId"].Value);
		item.qcActivityDescription = Conversion.ToString(r.Cells["FumigationActivity"].Value);
		item.attachmentsValues = "";
		item.customAttachmentsValues = "";
	}

	private void Insert()
	{
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Expected O, but got Unknown
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c2: Expected O, but got Unknown
		try
		{
			if (grdFumigation.GetDataRows().Count() == 0)
			{
				throw new Exception("No fumigation records found");
			}
			if (grdStockDetail.GetDataRows().Count() == 0)
			{
				throw new Exception("No Stock records found in grid");
			}
			string Msg = ((RecId == 0) ? "Save" : "Update");
			if (!FormHelper.ConfirmAction("Are you sure to " + Msg + "?"))
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
			obj.FumigationLotNo = LotNoDbCall();
			obj.labIPmActivityLogId = 0;
			obj.entryUserId = UserAccount.ID;
			obj.entryDate = DateTime.Now;
			obj.modifyUserId = UserAccount.ID;
			obj.modifyDate = DateTime.Now;
			obj.isApproved = false;
			int latestMasterId = obj.labIPmActivityLogMasterId;
			int nextNo = Convert.ToInt32(obj.FumigationLotNo.Replace("IPM-", ""));
			List<GridEXRow> list = grdFumigation.GetDataRows().ToList();
			List<GridEXRow> stockRows = grdStockDetail.GetDataRows().ToList();
			Dictionary<int, List<labIPmActivityLog>> groupedReadById = (from x in ReadByIdList
				group x by x.labIPmActivityLogMasterId).ToDictionary((IGrouping<int, labIPmActivityLog> g) => g.Key, (IGrouping<int, labIPmActivityLog> g) => g.ToList());
			foreach (GridEXRow fum in list)
			{
				int existingMasterId = Conversion.ToInt(fum.Cells["MasterId"].Value);
				string existingLotNo = Conversion.ToString(fum.Cells["FumigationLotNo"].Value);
				bool isUpdate = existingMasterId > 0;
				int masterId = (isUpdate ? existingMasterId : latestMasterId++);
				string lotNo = (isUpdate ? existingLotNo : $"IPM-{nextNo:D2}");
				if (!isUpdate)
				{
					nextNo++;
				}
				IEnumerable<object> sourceRows;
				if (isUpdate)
				{
					IEnumerable<object> enumerable;
					if (!groupedReadById.ContainsKey(existingMasterId))
					{
						enumerable = Enumerable.Empty<object>();
					}
					else
					{
						IEnumerable<object> enumerable2 = groupedReadById[existingMasterId];
						enumerable = enumerable2;
					}
					sourceRows = enumerable;
				}
				else
				{
					sourceRows = stockRows;
				}
				foreach (dynamic row in sourceRows)
				{
					labIPmActivityLog val = new labIPmActivityLog();
					val.labIPmActivityLogMasterId = masterId;
					val.jobLotIdIpm = masterId;
					val.FumigationLotNo = lotNo;
					val.labIPmActivityLogId = (isUpdate ? row.labIPmActivityLogId : ((object)0));
					val.actionId = ((!isUpdate) ? 1 : 2);
					labIPmActivityLog item = val;
					if (isUpdate)
					{
						item.refDocumentTypeId = row.refDocumentTypeId;
						item.refDocumentHeaderId = row.refDocumentHeaderId;
						item.refDocumentDetialId = row.refDocumentDetialId;
					}
					else
					{
						item.refDocumentTypeId = Conversion.ToInt(row.Cells["RefDocumentTypeId"].Value);
						item.refDocumentHeaderId = Conversion.ToInt(row.Cells["RefDocIdNo"].Value);
						item.refDocumentDetialId = Conversion.ToInt(row.Cells["RefDocSubIdNo"].Value);
					}
					FillDetailListCommonForInsertAndDelete(item, fum);
					ValidateItem(item, fum.RowIndex, isUpdate ? ((object)(-1)) : row.RowIndex);
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

	private void btnsave_Click(object sender, EventArgs e)
	{
		try
		{
			RecId = 0;
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdate_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId == 0)
			{
				throw new Exception("Record not update because Id not found");
			}
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ReadById(int recordId)
	{
		try
		{
			Reset();
			RecId = recordId;
			ReadByIdList = labIPmActivityLog.GetById(recordId);
			if (ReadByIdList != null)
			{
				tabControl1.SelectedIndex = 0;
				btnsave.Visible = false;
				btnUpdate.Visible = true;
				dtFumigationDetail.Clear();
				frmLabActivityIPM_Helper.FillFumigationDetailFromListByMasterId(ReadByIdList, dtFumigationDetail);
				dtStockDetail.Clear();
				frmLabActivityIPM_Helper.FillStockDetailFromListByMasterId(ReadByIdList, dtStockDetail);
				BindGrids();
				((Control)(object)grdFumigation).Focus();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ValidateItem(labIPmActivityLog item, int fumRowIndex, int stockRowIndex)
	{
		if (stockRowIndex >= 0)
		{
			FormHelper.ValidateField(item.refDocumentTypeId, "Reference Document Type", stockRowIndex);
			FormHelper.ValidateField(item.refDocumentHeaderId, "Reference Document", stockRowIndex);
		}
		FormHelper.ValidateField(item.jobLotIdIpm, "Fumigation Lot No", fumRowIndex);
		FormHelper.ValidateField(item.qcActivityStepId, "Analysis Step", fumRowIndex);
		FormHelper.ValidateField(item.qcActivityDate, "Fumigated Date", fumRowIndex);
		FormHelper.ValidateField(item.qcActivityStatusAfterCompletionId, "Status After Completion", fumRowIndex);
		FormHelper.ValidateField(item.fumigatedBy, "Fumigated By", fumRowIndex);
		FormHelper.ValidateField(item.checkBy, "Supervisor Check By", fumRowIndex);
		FormHelper.ValidateField(item.verifiedBy, "Verified By", fumRowIndex);
		FormHelper.ValidateField(item.stockReleaseHoldStatusId, "Stock Release Hold Status", fumRowIndex);
		if (item.completionDueDate < item.qcActivityDate)
		{
			throw new Exception($"Completion Due Date cannot be less than Analysis Date at Row {fumRowIndex + 1}");
		}
		if (item.labIPmActivityLogId == 0)
		{
			if (item.qcActivityStatusAfterCompletionId == 2)
			{
				throw new Exception("Pass status cannot be selected during insert. Lot No: " + item.FumigationLotNo);
			}
			if (item.stockReleaseHoldStatusId == 2)
			{
				throw new Exception("Stock cannot be released during insert. Lot No: " + item.FumigationLotNo);
			}
		}
	}

	private void btnPrint_Click(object sender, EventArgs e)
	{
	}

	private void btnNewHistory_Click(object sender, EventArgs e)
	{
	}

	private void BtnRefreshHistory_Click(object sender, EventArgs e)
	{
		try
		{
			HistoryComboDbCall();
			ParentCategoryComboFill();
			HistoryComboBind();
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
			HistoryFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.CompanyId = UserAccount.CompanyId;
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.DocumentTypeId = DocumentTypeId;
			obj.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			obj.EntryUser = UserAccount.ID;
			if (drdocdate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					obj.FromDate = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					obj.ToDate = ToDateHistory.Value;
				}
			}
			else if (rdentrydate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					obj.EntryDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					obj.EntryDateTo = ToDateHistory.Value;
				}
			}
			else if (rdmodifydate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					obj.ModifyDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					obj.ModifyDateTo = ToDateHistory.Value;
				}
			}
			string ParentIds = "";
			if (((Control)(object)CmbParentItemHistory).Text != null && ((Control)(object)CmbParentItemHistory).Text != string.Empty)
			{
				DataTable dt = (DataTable)((UltraGridBase)CmbParentItemHistory).DataSource;
				string[] array = (Conversion.ToString((object)((Control)(object)CmbParentItemHistory).Text.Trim()) + ",").Split(',');
				foreach (string author in array)
				{
					DataRow[] dr2 = dt.Select("name='" + Conversion.ToString((object)author) + "'");
					if (dr2.Length != 0)
					{
						ParentIds = ParentIds + Conversion.ToString(dr2[0].ItemArray[0]) + ",";
					}
				}
			}
			obj.ParentCategoryIds = ParentIds;
			obj.ItemId = Conversion.ToInt(CmbItemName.Value);
			obj.StatusId = Conversion.ToInt(CmbStatus.Value);
			DataTable dtHistory = labIPmActivityLog.FormHistory(obj);
			if (dtHistory.Rows.Count > 0)
			{
				if (dtHistoryForLayout.Columns.Count == 0)
				{
					dtHistoryForLayout.Columns.Add("Id", typeof(int));
					dtHistoryForLayout.Columns.Add("DocumentTypeId", typeof(int));
					dtHistoryForLayout.Columns.Add("FumigationLotNo", typeof(string));
					dtHistoryForLayout.Columns.Add("AnalysisStep", typeof(string));
					dtHistoryForLayout.Columns.Add("ActivityDate", typeof(DateTime));
					dtHistoryForLayout.Columns.Add("ActivityForDays", typeof(int));
					dtHistoryForLayout.Columns.Add("CompletionDueDate", typeof(DateTime));
					dtHistoryForLayout.Columns.Add("StatusAfterCompletionName", typeof(string));
					dtHistoryForLayout.Columns.Add("NextActivityPlanDate", typeof(DateTime));
					dtHistoryForLayout.Columns.Add("TotalQtyBalance", typeof(double));
					dtHistoryForLayout.Columns.Add("TotalWeightBalance", typeof(double));
					dtHistoryForLayout.Columns.Add("FumigatedBy", typeof(string));
					dtHistoryForLayout.Columns.Add("CheckBy", typeof(string));
					dtHistoryForLayout.Columns.Add("VerifiedBy", typeof(string));
					dtHistoryForLayout.Columns.Add("stockReleaseHoldStatusId", typeof(int));
					dtHistoryForLayout.Columns.Add("StockReleaseHoldStatus", typeof(string));
					dtHistoryForLayout.Columns.Add("FumigationRemarks", typeof(string));
					dtHistoryForLayout.Columns.Add("Remarks", typeof(string));
					dtHistoryForLayout.Columns.Add("EntryUserName", typeof(string));
					dtHistoryForLayout.Columns.Add("EntryDate", typeof(DateTime));
					dtHistoryForLayout.Columns.Add("ModifyUserName", typeof(string));
					dtHistoryForLayout.Columns.Add("ModifyDate", typeof(string));
					dtHistoryForLayout.Columns.Add("IsApproved", typeof(bool));
					dtHistoryForLayout.Columns.Add("ApprovalUserName", typeof(string));
					dtHistoryForLayout.Columns.Add("ApprovedDate", typeof(string));
					dtHistoryForLayout.Columns.Add("ApprovalRemarks", typeof(string));
				}
				dtHistoryForLayout.Clear();
				IEnumerable<DataRow> enumerable = from r in dtHistory.AsEnumerable()
					group r by new
					{
						refDocumentTypeId = Conversion.ToInt(r["refDocumentTypeId"]),
						refDocumentHeaderId = Conversion.ToInt(r["refDocumentHeaderId"]),
						refDocumentDetialId = Conversion.ToInt(r["refDocumentDetialId"])
					} into g
					select g.OrderByDescending((DataRow r) => Conversion.ToInt(r["labIPmActivityLogId"])).FirstOrDefault() into r
					where r != null
					select r;
				HashSet<int> Ids = new HashSet<int>();
				foreach (DataRow row in enumerable)
				{
					int Id = Conversion.ToInt(row["labIPmActivityLogMasterId"]);
					if (Ids.Add(Id))
					{
						DataRow dr = dtHistoryForLayout.NewRow();
						dr["Id"] = Id;
						dr["DocumentTypeId"] = Conversion.ToInt(row["documentTypeId"]);
						dr["FumigationLotNo"] = Conversion.ToString(row["FumigationLotNo"]);
						dr["AnalysisStep"] = Conversion.ToString(row["qcActivityStepName"]);
						dr["ActivityDate"] = Conversion.ToDateTime(row["qcActivityDate"]);
						dr["ActivityForDays"] = Conversion.ToInt(row["activityForDays"]);
						dr["CompletionDueDate"] = Conversion.ToDateTime(row["completionDueDate"]);
						dr["StatusAfterCompletionName"] = Conversion.ToString(row["qcActivityStatusAfterCompletionName"]);
						dr["NextActivityPlanDate"] = Conversion.ToDateTime(row["nextActivityPlanDate"]);
						dr["TotalQtyBalance"] = Conversion.ToDouble(row["TotalQtyBalance"]);
						dr["TotalWeightBalance"] = Conversion.ToDouble(row["TotalWeightBalance"]);
						dr["FumigatedBy"] = Conversion.ToString(row["fumigatedBy"]);
						dr["CheckBy"] = Conversion.ToString(row["checkBy"]);
						dr["VerifiedBy"] = Conversion.ToString(row["verifiedBy"]);
						dr["stockReleaseHoldStatusId"] = Conversion.ToInt(row["stockReleaseHoldStatusId"]);
						dr["StockReleaseHoldStatus"] = Conversion.ToString(row["stockReleaseHoldStatusName"]);
						dr["FumigationRemarks"] = Conversion.ToString(row["qcActivityDescription"]);
						dr["Remarks"] = Conversion.ToString(row["remarksHeader"]);
						dr["EntryDate"] = Conversion.ToDateTime(row["entryDate"]);
						dr["EntryUserName"] = Conversion.ToString(row["EntryUserName"]);
						dr["ModifyDate"] = Conversion.ToDateTime(row["modifyDate"]).ToShortDateString();
						dr["ModifyUserName"] = Conversion.ToString(row["ModifyUserName"]);
						dr["IsApproved"] = Conversion.ToBool(row["isApproved"]);
						dr["ApprovedDate"] = Conversion.ToDateTime(row["approvedDate"]).ToShortDateString();
						dr["ApprovalUserName"] = Conversion.ToString(row["ApprovalUserName"]);
						dr["ApprovalRemarks"] = Conversion.ToString(row["approvalRemarks"]);
						dtHistoryForLayout.Rows.Add(dr);
					}
				}
				grdHistory.DataSource = dtHistoryForLayout;
				grdHistory.RetrieveStructure();
				GridSettingHistory();
			}
			else
			{
				grdHistory.DataSource = null;
				grdHistoryDetail.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GridSettingHistory()
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "DocumentTypeId", "stockReleaseHoldStatusId" };
			GridEX_Helper.GridWrappingAndColumnSettings(grdHistory, 3, 3, hiddenColumns);
			grdHistory.RootTable.Columns["StockReleaseHoldStatus"].ColumnType = (ColumnType)0;
			grdHistory.RootTable.Columns["ActivityDate"].FormatString = "dd-MMM-yyyy";
			grdHistory.RootTable.Columns["CompletionDueDate"].FormatString = "dd-MMM-yyyy";
			grdHistory.RootTable.Columns["NextActivityPlanDate"].FormatString = "dd-MMM-yyyy";
			grdHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yy hh:mm tt";
			grdHistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yy hh:mm tt";
			grdHistory.RootTable.Columns["ApprovedDate"].FormatString = "dd-MMM-yy hh:mm tt";
			grdHistory.RootTable.Columns["FumigationLotNo"].Width = Constants.InventoryConstants.JobLotConstant;
			grdHistory.RootTable.Columns["AnalysisStep"].Width = Constants.InventoryConstants.ItemName + 20;
			grdHistory.RootTable.Columns["ActivityDate"].Width = Constants.InventoryConstants.DateConstant + 15;
			grdHistory.RootTable.Columns["ActivityForDays"].Width = 80;
			grdHistory.RootTable.Columns["CompletionDueDate"].Width = Constants.InventoryConstants.DateConstant + 15;
			grdHistory.RootTable.Columns["StatusAfterCompletionName"].Width = 150;
			grdHistory.RootTable.Columns["NextActivityPlanDate"].Width = Constants.InventoryConstants.DateConstant + 15;
			grdHistory.RootTable.Columns["FumigatedBy"].Width = Constants.InventoryConstants.EntryUserConstants + 20;
			grdHistory.RootTable.Columns["CheckBy"].Width = Constants.InventoryConstants.EntryUserConstants + 20;
			grdHistory.RootTable.Columns["VerifiedBy"].Width = Constants.InventoryConstants.EntryUserConstants + 20;
			grdHistory.RootTable.Columns["StockReleaseHoldStatus"].Width = 150;
			grdHistory.RootTable.Columns["FumigationRemarks"].Width = 200;
			grdHistory.RootTable.Columns["Remarks"].Width = 200;
			grdHistory.RootTable.Columns["EntryUserName"].Width = Constants.InventoryConstants.EntryUserConstants + 20;
			grdHistory.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			grdHistory.RootTable.Columns["ModifyUserName"].Width = Constants.InventoryConstants.ModifyUserConstants + 20;
			grdHistory.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			grdHistory.RootTable.Columns["ApprovalUserName"].Width = Constants.InventoryConstants.ModifyUserConstants + 20;
			grdHistory.RootTable.Columns["ApprovedDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			grdHistory.RootTable.Columns["ApprovalRemarks"].Width = 200;
			grdHistory.RootTable.Columns["TotalQtyBalance"].FormatString = "#,#.##";
			grdHistory.RootTable.Columns["TotalQtyBalance"].TotalFormatString = "#,#.##";
			grdHistory.RootTable.Columns["TotalQtyBalance"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["TotalWeightBalance"].FormatString = "#,#.##";
			grdHistory.RootTable.Columns["TotalWeightBalance"].TotalFormatString = "#,#.##";
			grdHistory.RootTable.Columns["TotalWeightBalance"].AggregateFunction = (AggregateFunction)2;
			GridEX_Helper.AddButton(grdHistory, "Edit", "Edit", 50, 0);
			grdHistory.FrozenColumns = 1;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grdHistory_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				ReadByIdCallFromHistory(r);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && e.Column.Key == "Edit")
			{
				ReadByIdCallFromHistory(r);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ReadByIdCallFromHistory(GridEXRow r)
	{
		try
		{
			if (Conversion.ToInt(r.Cells["stockReleaseHoldStatusId"].Value) == 2)
			{
				MessageBox.Show("This record cannot be edited because its Stock Release Hold Status is 'Released'.", "Edit Blocked", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			int Id = Conversion.ToInt(r.Cells["Id"].Value);
			ReadById(Id);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && e.Column.Key == "NoOfAttachments")
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(r.Cells["Id"].Value), DocumentTypeId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow row = grdHistory.CurrentRow;
			if (row != null && (int)row.RowType == 0 && row.Cells.Count > 0)
			{
				GetDetailGrdByHeadId(Conversion.ToInt(row.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetDetailGrdByHeadId(int Id)
	{
		try
		{
			List<labIPmActivityLog> Obj = labIPmActivityLog.GetById(Id);
			if (Obj != null && Obj.Count == 0)
			{
				grdHistoryDetail.ClearStructure();
				return;
			}
			DataTable dtDetailHistory = new DataTable();
			frmLabActivityIPM_Helper.InitializeStockTable(dtDetailHistory);
			frmLabActivityIPM_Helper.FillStockDetailFromListByMasterId(Obj, dtDetailHistory);
			grdHistoryDetail.DataSource = dtDetailHistory;
			grdHistoryDetail.RetrieveStructure();
			HistoryDetailGridSetting();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryDetailGridSetting()
	{
		try
		{
			grdStockDetailSettings(grdHistoryDetail);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar3_Load(object sender, EventArgs e)
	{
	}

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
	{
	}

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grdHistory, dtHistoryForLayout);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmLabActivityIPM_KeyDown(object sender, KeyEventArgs e)
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
					((Control)(object)grdFumigation).Focus();
				}
				else
				{
					tabControl1.SelectedIndex = 1;
					((Control)(object)grdHistory).Focus();
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
				if (e.Control && e.KeyCode == Keys.S)
				{
					if (btnsave.Visible && btnsave.Enabled)
					{
						btnsave_Click(null, null);
					}
				}
				else if (e.Control && e.KeyCode == Keys.U)
				{
					if (btnUpdate.Visible && btnUpdate.Enabled)
					{
						btnUpdate_Click(null, null);
					}
				}
				else if (e.Control && e.KeyCode == Keys.P)
				{
					if (btnPrint.Visible && btnPrint.Enabled)
					{
						btnPrint_Click(null, null);
					}
				}
				else if (e.Control && e.KeyCode == Keys.N)
				{
					btnnew_Click(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.R)
				{
					BtnRefresh_Click(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.F5)
				{
					((Control)(object)grdFumigation).Focus();
				}
				else if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grdStockDetail).Focus();
				}
				else if (e.Control && e.KeyCode == Keys.Up)
				{
					((Control)(object)grdFumigation).Focus();
				}
			}
			else if (tabControl1.SelectedIndex == 1)
			{
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grdHistory).Focus();
				}
				else if (e.Control && e.KeyCode == Keys.S)
				{
					btnshow_Click(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.Up)
				{
					FromDateHistory.Focus();
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

	private void BtnShortCutkeys_Click(object sender, EventArgs e)
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

	private void grdHistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdHistory.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count == 0)
			{
				return;
			}
			int CurrId = Conversion.ToInt(r.Cells["Id"].Value);
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdHistory.CurrentColumn.Key == "Edit")
				{
					if (!formright.DoHaveUpdateRights)
					{
						throw new Exception("you don't have update rights...");
					}
					ReadByIdCallFromHistory(r);
				}
				if (grdHistory.CurrentColumn.Key == "Print" && !formright.DoHavePrintRights)
				{
					throw new Exception("you don't have print rights...");
				}
				if (grdHistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(CurrId, DocumentTypeId);
				}
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				if (!formright.DoHaveUpdateRights)
				{
					throw new Exception("you don't have update rights...");
				}
				ReadByIdCallFromHistory(r);
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
		//IL_0388: Unknown result type (might be due to invalid IL or missing references)
		//IL_0392: Expected O, but got Unknown
		//IL_03b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03be: Expected O, but got Unknown
		//IL_047a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0484: Expected O, but got Unknown
		//IL_049b: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a5: Expected O, but got Unknown
		//IL_04dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e7: Expected O, but got Unknown
		//IL_0509: Unknown result type (might be due to invalid IL or missing references)
		//IL_0513: Expected O, but got Unknown
		//IL_051f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0529: Expected O, but got Unknown
		//IL_0535: Unknown result type (might be due to invalid IL or missing references)
		//IL_053f: Expected O, but got Unknown
		//IL_054b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0555: Expected O, but got Unknown
		//IL_0577: Unknown result type (might be due to invalid IL or missing references)
		//IL_0581: Expected O, but got Unknown
		//IL_058d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0597: Expected O, but got Unknown
		//IL_05a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ad: Expected O, but got Unknown
		//IL_05b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c3: Expected O, but got Unknown
		//IL_063d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0647: Expected O, but got Unknown
		//IL_0cfe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d08: Expected O, but got Unknown
		//IL_0d15: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d1f: Expected O, but got Unknown
		//IL_181c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1826: Expected O, but got Unknown
		//IL_184a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1854: Expected O, but got Unknown
		GridEXLayout grdStockDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdFumigation_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdHistoryDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance101 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance113 = new Appearance();
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
		Appearance appearance42 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.IPM.frmLabActivityIPM));
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.grdStockDetail = new GridEX();
		this.panel1 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label1 = new System.Windows.Forms.Label();
		this.grdFumigation = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.label42 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.chkPrint = new System.Windows.Forms.CheckBox();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.BtnRefresh = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.btnStockLoader = new System.Windows.Forms.ToolStripButton();
		this.BtnShortCutkeys = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.grdHistory = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label44 = new System.Windows.Forms.Label();
		this.grdHistoryDetail = new GridEX();
		this.panel15 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label43 = new System.Windows.Forms.Label();
		this.panel13 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.CmbReferenceDocument = new UltraCombo();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.drdocdate = new System.Windows.Forms.RadioButton();
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
		this.CmbJobLot = new UltraCombo();
		this.label4 = new System.Windows.Forms.Label();
		this.CmbItemName = new UltraCombo();
		this.label2 = new System.Windows.Forms.Label();
		this.CmbSupplier = new UltraCombo();
		this.btnshow = new System.Windows.Forms.Button();
		this.CmbParentItemHistory = new UltraCombo();
		this.label36 = new System.Windows.Forms.Label();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.label8 = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label30 = new System.Windows.Forms.Label();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.BtnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.CmbStatus = new UltraCombo();
		this.label9 = new System.Windows.Forms.Label();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdStockDetail).BeginInit();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdFumigation).BeginInit();
		this.panel5.SuspendLayout();
		this.panel3.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).BeginInit();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryDetail).BeginInit();
		this.panel15.SuspendLayout();
		this.panel13.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbReferenceDocument).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWarehouse).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemCategory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYear).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplier).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbParentItemHistory).BeginInit();
		this.panel4.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbStatus).BeginInit();
		base.SuspendLayout();
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1134, 661);
		this.tabControl1.TabIndex = 0;
		this.tabPage1.Controls.Add((System.Windows.Forms.Control)(object)this.grdStockDetail);
		this.tabPage1.Controls.Add(this.panel1);
		this.tabPage1.Controls.Add((System.Windows.Forms.Control)(object)this.grdFumigation);
		this.tabPage1.Controls.Add(this.panel5);
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Controls.Add(this.toolStrip1);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1126, 632);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.grdStockDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdStockDetail).BackColor = System.Drawing.Color.White;
		grdStockDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdStockDetail.DesignTimeLayout = grdStockDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdStockDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdStockDetail.FilterMode = (FilterMode)2;
		this.grdStockDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdStockDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdStockDetail.GroupByBoxVisible = false;
		this.grdStockDetail.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdStockDetail).Location = new System.Drawing.Point(3, 357);
		((System.Windows.Forms.Control)(object)this.grdStockDetail).Name = "grdStockDetail";
		((ScrollableBase)this.grdStockDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdStockDetail).Size = new System.Drawing.Size(1120, 272);
		((System.Windows.Forms.Control)(object)this.grdStockDetail).TabIndex = 57;
		this.grdStockDetail.TotalRow = (InheritableBoolean)1;
		this.grdStockDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdStockDetail).VisualStyle = (VisualStyle)4;
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.Controls.Add(this.ctrlGrdBar2);
		this.panel1.Controls.Add(this.label1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(3, 334);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1120, 23);
		this.panel1.TabIndex = 56;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1068, 2);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar2.MyGrid = this.grdStockDetail;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(50, 20);
		this.ctrlGrdBar2.TabIndex = 128;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.Teal;
		this.label1.Location = new System.Drawing.Point(5, 3);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(84, 16);
		this.label1.TabIndex = 127;
		this.label1.Text = "Stock Detail";
		this.grdFumigation.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdFumigation).BackColor = System.Drawing.Color.White;
		grdFumigation_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdFumigation.DesignTimeLayout = grdFumigation_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdFumigation).Dock = System.Windows.Forms.DockStyle.Top;
		this.grdFumigation.FilterMode = (FilterMode)2;
		this.grdFumigation.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdFumigation).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdFumigation.GroupByBoxVisible = false;
		this.grdFumigation.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdFumigation).Location = new System.Drawing.Point(3, 82);
		((System.Windows.Forms.Control)(object)this.grdFumigation).Name = "grdFumigation";
		((ScrollableBase)this.grdFumigation).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdFumigation).Size = new System.Drawing.Size(1120, 252);
		((System.Windows.Forms.Control)(object)this.grdFumigation).TabIndex = 2;
		this.grdFumigation.TotalRow = (InheritableBoolean)1;
		this.grdFumigation.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdFumigation).VisualStyle = (VisualStyle)4;
		this.grdFumigation.CellUpdated += new ColumnActionEventHandler(grdFumigation_CellUpdated);
		this.grdFumigation.ColumnButtonClick += new ColumnActionEventHandler(grdFumigation_ColumnButtonClick);
		this.grdFumigation.SelectionChanged += new System.EventHandler(grdFumigation_SelectionChanged);
		((System.Windows.Forms.Control)(object)this.grdFumigation).Click += new System.EventHandler(grdFumigation_Click);
		((System.Windows.Forms.Control)(object)this.grdFumigation).KeyDown += new System.Windows.Forms.KeyEventHandler(grdFumigation_KeyDown);
		this.panel5.BackColor = System.Drawing.Color.White;
		this.panel5.Controls.Add(this.ctrlGrdBar3);
		this.panel5.Controls.Add(this.label42);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(3, 59);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1120, 23);
		this.panel5.TabIndex = 55;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(1066, 0);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
		this.ctrlGrdBar3.MyGrid = this.grdFumigation;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(51, 22);
		this.ctrlGrdBar3.TabIndex = 128;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label42.ForeColor = System.Drawing.Color.Teal;
		this.label42.Location = new System.Drawing.Point(5, 3);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(117, 16);
		this.label42.TabIndex = 127;
		this.label42.Text = "Fumigation Detail";
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.label23);
		this.panel3.Controls.Add(this.chkPrint);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(3, 30);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1120, 29);
		this.panel3.TabIndex = 2;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(8, 5);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(142, 19);
		this.label23.TabIndex = 0;
		this.label23.Text = "IPM Lab Activity";
		this.chkPrint.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.chkPrint.AutoSize = true;
		this.chkPrint.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkPrint.ForeColor = System.Drawing.Color.White;
		this.chkPrint.Location = new System.Drawing.Point(1037, 6);
		this.chkPrint.Name = "chkPrint";
		this.chkPrint.Size = new System.Drawing.Size(81, 18);
		this.chkPrint.TabIndex = 2;
		this.chkPrint.Text = "Preview";
		this.chkPrint.UseVisualStyleBackColor = true;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[9] { this.btnnew, this.BtnRefresh, this.toolStripSeparator1, this.btnsave, this.btnUpdate, this.btnPrint, this.btnStockLoader, this.BtnShortCutkeys, this.toolStripSeparator2 });
		this.toolStrip1.Location = new System.Drawing.Point(3, 3);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1120, 27);
		this.toolStrip1.TabIndex = 1;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = (System.Drawing.Image)resources.GetObject("btnnew.Image");
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.BtnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnRefresh.Image = (System.Drawing.Image)resources.GetObject("BtnRefresh.Image");
		this.BtnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefresh.Name = "BtnRefresh";
		this.BtnRefresh.Size = new System.Drawing.Size(80, 24);
		this.BtnRefresh.Text = "Refresh";
		this.BtnRefresh.Click += new System.EventHandler(BtnRefresh_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnUpdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.Image = (System.Drawing.Image)resources.GetObject("btnUpdate.Image");
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(100, 24);
		this.btnPrint.Text = "1050-&Print";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.btnStockLoader.Image = Architecture.WinApp.Properties.Resources.download;
		this.btnStockLoader.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnStockLoader.Name = "btnStockLoader";
		this.btnStockLoader.Size = new System.Drawing.Size(105, 24);
		this.btnStockLoader.Text = "Load Stock";
		this.btnStockLoader.Click += new System.EventHandler(btnStockLoader_Click);
		this.BtnShortCutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShortCutkeys.Image = (System.Drawing.Image)resources.GetObject("BtnShortCutkeys.Image");
		this.BtnShortCutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnShortCutkeys.Name = "BtnShortCutkeys";
		this.BtnShortCutkeys.Size = new System.Drawing.Size(126, 24);
		this.BtnShortCutkeys.Text = "ShortCut Keys";
		this.BtnShortCutkeys.Click += new System.EventHandler(BtnShortCutkeys_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistory);
		this.tabPage2.Controls.Add(this.panel6);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistoryDetail);
		this.tabPage2.Controls.Add(this.panel15);
		this.tabPage2.Controls.Add(this.panel13);
		this.tabPage2.Controls.Add(this.panel4);
		this.tabPage2.Controls.Add(this.toolStrip2);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1126, 632);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.grdHistory.AllowEdit = (InheritableBoolean)2;
		grdHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistory.DesignTimeLayout = grdHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistory.DynamicFiltering = true;
		this.grdHistory.FilterMode = (FilterMode)2;
		this.grdHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdHistory).Location = new System.Drawing.Point(3, 181);
		((System.Windows.Forms.Control)(object)this.grdHistory).Name = "grdHistory";
		((ScrollableBase)this.grdHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistory).Size = new System.Drawing.Size(1120, 235);
		((System.Windows.Forms.Control)(object)this.grdHistory).TabIndex = 55;
		this.grdHistory.TotalRow = (InheritableBoolean)1;
		this.grdHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistory).VisualStyle = (VisualStyle)4;
		this.grdHistory.ColumnButtonClick += new ColumnActionEventHandler(grdHistory_ColumnButtonClick);
		this.grdHistory.SelectionChanged += new System.EventHandler(grdHistory_SelectionChanged);
		this.grdHistory.LinkClicked += new ColumnActionEventHandler(grdHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdHistory).DoubleClick += new System.EventHandler(grdHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(grdHistory_KeyDown);
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.label44);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel6.Location = new System.Drawing.Point(3, 416);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1120, 23);
		this.panel6.TabIndex = 57;
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label44.ForeColor = System.Drawing.Color.White;
		this.label44.Location = new System.Drawing.Point(5, 3);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(84, 16);
		this.label44.TabIndex = 127;
		this.label44.Text = "Stock Detail";
		this.grdHistoryDetail.AllowDelete = (InheritableBoolean)1;
		((ScrollableBase)this.grdHistoryDetail).BackColor = System.Drawing.Color.White;
		grdHistoryDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistoryDetail.DesignTimeLayout = grdHistoryDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Dock = System.Windows.Forms.DockStyle.Bottom;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistoryDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Location = new System.Drawing.Point(3, 439);
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Name = "grdHistoryDetail";
		((ScrollableBase)this.grdHistoryDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Size = new System.Drawing.Size(1120, 190);
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).TabIndex = 56;
		this.grdHistoryDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistoryDetail).VisualStyle = (VisualStyle)4;
		this.panel15.BackColor = System.Drawing.Color.Teal;
		this.panel15.Controls.Add(this.ctrlGrdBar1);
		this.panel15.Controls.Add(this.label43);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.Location = new System.Drawing.Point(3, 158);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(1120, 23);
		this.panel15.TabIndex = 54;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1076, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(9, 6, 9, 6);
		this.ctrlGrdBar1.MyGrid = this.grdHistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(42, 22);
		this.ctrlGrdBar1.TabIndex = 128;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label43.AutoSize = true;
		this.label43.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label43.ForeColor = System.Drawing.Color.White;
		this.label43.Location = new System.Drawing.Point(7, 3);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(112, 16);
		this.label43.TabIndex = 127;
		this.label43.Text = "Filtered Records";
		this.panel13.BackColor = System.Drawing.Color.White;
		this.panel13.Controls.Add(this.groupBox1);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel13.Location = new System.Drawing.Point(3, 59);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1120, 99);
		this.panel13.TabIndex = 6;
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbReferenceDocument);
		this.groupBox1.Controls.Add(this.rdmodifydate);
		this.groupBox1.Controls.Add(this.rdentrydate);
		this.groupBox1.Controls.Add(this.drdocdate);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWarehouse);
		this.groupBox1.Controls.Add(this.label12);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemType);
		this.groupBox1.Controls.Add(this.label11);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemCategory);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCropYear);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobLot);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemName);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbStatus);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSupplier);
		this.groupBox1.Controls.Add(this.btnshow);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbParentItemHistory);
		this.groupBox1.Controls.Add(this.label36);
		this.groupBox1.Controls.Add(this.ToDateHistory);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add(this.label33);
		this.groupBox1.Controls.Add(this.FromDateHistory);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(2, 1);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1121, 98);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.CmbReferenceDocument.AllowNull = (DefaultableBoolean)1;
		this.CmbReferenceDocument.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbReferenceDocument.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbReferenceDocument.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance101).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbReferenceDocument).DisplayLayout.Appearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.CmbReferenceDocument).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbReferenceDocument).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance102).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance102).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance102).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbReferenceDocument).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance102;
		((AppearanceBase)appearance103).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbReferenceDocument).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance103;
		((SpecialBoxBase)((UltraGridBase)this.CmbReferenceDocument).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance104).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance104).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance104).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbReferenceDocument).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.CmbReferenceDocument).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbReferenceDocument).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance105).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbReferenceDocument).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance105;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance106).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbReferenceDocument).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.CmbReferenceDocument).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbReferenceDocument).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbReferenceDocument).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance107;
		((AppearanceBase)appearance109).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance109).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbReferenceDocument).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.CmbReferenceDocument).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbReferenceDocument).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance110).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance110).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance110).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance110).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbReferenceDocument).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance110;
		((AppearanceBase)appearance111).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbReferenceDocument).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.CmbReferenceDocument).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbReferenceDocument).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbReferenceDocument).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.CmbReferenceDocument).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbReferenceDocument).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.CmbReferenceDocument).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbReferenceDocument).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbReferenceDocument).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbReferenceDocument).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbReferenceDocument.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbReferenceDocument).Location = new System.Drawing.Point(819, 12);
		((System.Windows.Forms.Control)(object)this.CmbReferenceDocument).Name = "CmbReferenceDocument";
		((System.Windows.Forms.Control)(object)this.CmbReferenceDocument).Size = new System.Drawing.Size(210, 26);
		((System.Windows.Forms.Control)(object)this.CmbReferenceDocument).TabIndex = 9;
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(1031, 48);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 15;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(1031, 31);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 14;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(1031, 14);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 13;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
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
		((System.Windows.Forms.Control)(object)this.CmbWarehouse).Location = new System.Drawing.Point(555, 12);
		((System.Windows.Forms.Control)(object)this.CmbWarehouse).Name = "CmbWarehouse";
		((System.Windows.Forms.Control)(object)this.CmbWarehouse).Size = new System.Drawing.Size(180, 26);
		((System.Windows.Forms.Control)(object)this.CmbWarehouse).TabIndex = 6;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(204, 46);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(60, 15);
		this.label12.TabIndex = 203;
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
		((AppearanceBase)appearance42).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbItemType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemType).Location = new System.Drawing.Point(286, 40);
		((System.Windows.Forms.Control)(object)this.CmbItemType).Name = "CmbItemType";
		((System.Windows.Forms.Control)(object)this.CmbItemType).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemType).TabIndex = 4;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(204, 18);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(82, 15);
		this.label11.TabIndex = 202;
		this.label11.Text = "Item Category";
		this.CmbItemCategory.AllowNull = (DefaultableBoolean)1;
		this.CmbItemCategory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemCategory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemCategory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance118).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance119).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance119).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemCategory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance119;
		((AppearanceBase)appearance120).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemCategory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance120;
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
		((System.Windows.Forms.Control)(object)this.CmbItemCategory).Location = new System.Drawing.Point(286, 12);
		((System.Windows.Forms.Control)(object)this.CmbItemCategory).Name = "CmbItemCategory";
		((System.Windows.Forms.Control)(object)this.CmbItemCategory).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemCategory).TabIndex = 3;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(489, 74);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(57, 15);
		this.label7.TabIndex = 201;
		this.label7.Text = "Crop Year";
		this.CmbCropYear.AllowNull = (DefaultableBoolean)1;
		this.CmbCropYear.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCropYear.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCropYear.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Appearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance76).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance76).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance76;
		((AppearanceBase)appearance77).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance77;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance78).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance78).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance78).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance80).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance81;
		((AppearanceBase)appearance82).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance82).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance83).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance83).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance83).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance83).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance85).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCropYear.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Location = new System.Drawing.Point(555, 68);
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Name = "CmbCropYear";
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Size = new System.Drawing.Size(180, 26);
		((System.Windows.Forms.Control)(object)this.CmbCropYear).TabIndex = 8;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(204, 74);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(67, 15);
		this.label6.TabIndex = 200;
		this.label6.Text = "Item Name";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(489, 18);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(67, 15);
		this.label5.TabIndex = 199;
		this.label5.Text = "Warehouse";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(737, 18);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(84, 15);
		this.label3.TabIndex = 198;
		this.label3.Text = "Ref Document";
		this.CmbJobLot.AllowNull = (DefaultableBoolean)1;
		this.CmbJobLot.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobLot.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobLot.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Appearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance49).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance49).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance49).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance50).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance50;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance51).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance51).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance51).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance52;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance55;
		((AppearanceBase)appearance56).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance56).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance57).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance57).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance57).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance57).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance57;
		((AppearanceBase)appearance58).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbJobLot.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Location = new System.Drawing.Point(555, 40);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Name = "CmbJobLot";
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Size = new System.Drawing.Size(180, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).TabIndex = 7;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(489, 46);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(46, 15);
		this.label4.TabIndex = 197;
		this.label4.Text = "Job Lot";
		this.CmbItemName.AllowNull = (DefaultableBoolean)1;
		this.CmbItemName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance88).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.CmbItemName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance89).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance89).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance89;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance90;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance91).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance91).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance91).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance92).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance92;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance93).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance93;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance94;
		((AppearanceBase)appearance95).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance95).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance96).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance96).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance96).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance96).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance98).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance98;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance100;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Location = new System.Drawing.Point(286, 68);
		((System.Windows.Forms.Control)(object)this.CmbItemName).Name = "CmbItemName";
		((System.Windows.Forms.Control)(object)this.CmbItemName).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemName).TabIndex = 5;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(737, 46);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(69, 15);
		this.label2.TabIndex = 196;
		this.label2.Text = "Party Name";
		this.CmbSupplier.AllowNull = (DefaultableBoolean)1;
		this.CmbSupplier.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSupplier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSupplier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance62).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance62).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance62).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance62;
		((AppearanceBase)appearance63).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance63;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance66).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance67;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance71;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance72).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance73).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance73).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance73).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance73;
		((AppearanceBase)appearance114).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSupplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Location = new System.Drawing.Point(819, 40);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Name = "CmbSupplier";
		((System.Windows.Forms.Control)(object)this.CmbSupplier).Size = new System.Drawing.Size(210, 26);
		((System.Windows.Forms.Control)(object)this.CmbSupplier).TabIndex = 10;
		this.btnshow.BackColor = System.Drawing.Color.Teal;
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(1031, 69);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(55, 25);
		this.btnshow.TabIndex = 16;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.CmbParentItemHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbParentItemHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbParentItemHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbParentItemHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance37).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance37).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance37;
		((AppearanceBase)appearance38).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance38;
		((SpecialBoxBase)((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance39).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance39).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance39).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance40).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance44).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance45).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance45).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance45).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance65).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance69).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbParentItemHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbParentItemHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbParentItemHistory).Location = new System.Drawing.Point(74, 68);
		((System.Windows.Forms.Control)(object)this.CmbParentItemHistory).Name = "CmbParentItemHistory";
		((System.Windows.Forms.Control)(object)this.CmbParentItemHistory).Size = new System.Drawing.Size(128, 26);
		((System.Windows.Forms.Control)(object)this.CmbParentItemHistory).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbParentItemHistory).Leave += new System.EventHandler(CmbParentItemHistory_Leave);
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(4, 74);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(69, 15);
		this.label36.TabIndex = 187;
		this.label36.Text = "Parent Item";
		this.ToDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9f);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(74, 42);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(128, 22);
		this.ToDateHistory.TabIndex = 1;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(5, 46);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(48, 15);
		this.label8.TabIndex = 171;
		this.label8.Text = "Date To";
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(4, 18);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(63, 15);
		this.label33.TabIndex = 171;
		this.label33.Text = "Date From";
		this.FromDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9f);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(74, 14);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(128, 22);
		this.FromDateHistory.TabIndex = 0;
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.label30);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(3, 30);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1120, 29);
		this.panel4.TabIndex = 5;
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.ForeColor = System.Drawing.Color.White;
		this.label30.Location = new System.Drawing.Point(4, 4);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(205, 19);
		this.label30.TabIndex = 0;
		this.label30.Text = "IPM Lab Activity History";
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnNewHistory, this.toolStripSeparator3, this.BtnRefreshHistory });
		this.toolStrip2.Location = new System.Drawing.Point(3, 3);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1120, 27);
		this.toolStrip2.TabIndex = 4;
		this.toolStrip2.Text = "toolStrip2";
		this.btnNewHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewHistory.Name = "btnNewHistory";
		this.btnNewHistory.Size = new System.Drawing.Size(59, 24);
		this.btnNewHistory.Text = "&New";
		this.btnNewHistory.Click += new System.EventHandler(btnNewHistory_Click);
		this.toolStripSeparator3.Name = "toolStripSeparator3";
		this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
		this.BtnRefreshHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.BtnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefreshHistory.Name = "BtnRefreshHistory";
		this.BtnRefreshHistory.Size = new System.Drawing.Size(80, 24);
		this.BtnRefreshHistory.Text = "&Refresh";
		this.BtnRefreshHistory.Click += new System.EventHandler(BtnRefreshHistory_Click);
		this.CmbStatus.AllowNull = (DefaultableBoolean)1;
		this.CmbStatus.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbStatus.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbStatus.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbStatus).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbStatus).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance33).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance33).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbStatus).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance53).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbStatus).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance53;
		((SpecialBoxBase)((UltraGridBase)this.CmbStatus).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance64).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance64).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance64).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbStatus).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.CmbStatus).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbStatus).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance75).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance75;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance86).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance97;
		((AppearanceBase)appearance108).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance108).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance117).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance117).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance117).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance117;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbStatus).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbStatus).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbStatus).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbStatus).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbStatus.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbStatus).Location = new System.Drawing.Point(819, 69);
		((System.Windows.Forms.Control)(object)this.CmbStatus).Name = "CmbStatus";
		((System.Windows.Forms.Control)(object)this.CmbStatus).Size = new System.Drawing.Size(210, 26);
		((System.Windows.Forms.Control)(object)this.CmbStatus).TabIndex = 11;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(737, 75);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(40, 15);
		this.label9.TabIndex = 196;
		this.label9.Text = "Status";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1134, 661);
		base.Controls.Add(this.tabControl1);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.Name = "frmLabActivityIPM";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmLabActivityIPM_FormClosing);
		base.Load += new System.EventHandler(frmLabActivityIPM_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmLabActivityIPM_KeyDown);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.tabPage1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdStockDetail).EndInit();
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdFumigation).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.tabPage2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryDetail).EndInit();
		this.panel15.ResumeLayout(false);
		this.panel15.PerformLayout();
		this.panel13.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbReferenceDocument).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWarehouse).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemCategory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYear).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplier).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbParentItemHistory).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbStatus).EndInit();
		base.ResumeLayout(false);
	}
}

}
