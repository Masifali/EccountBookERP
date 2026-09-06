using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Architecture.BLL.Lab;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Lab;
using Architecture.Model.Main;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.LabAnalysis.Helper_Methods;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Lab;

public class frmLabAnalysisScheduleItemAndPartyWise : Form
{
	private Rightsobjects formright = new Rightsobjects();

	private UserAccount UserAccount = new UserAccount();

	private DataTable dtGrid = new DataTable();

	private DataTable dtHistoryFromDb = new DataTable();

	private DataTable dtHistoryGrid = new DataTable();

	private DataTable dtSupplier = new DataTable();

	private DataTable dtItem = new DataTable();

	private string ScreenName = "frmLabAnalysisScheduleItemAndPartyWise";

	private bool UpdateMode;

	private int RecId;

	private bool FormInitialized;

	private bool FieldsChanged;

	private bool IsLoading;

	private int DefaultDaysToLessFromHistoryFromDate;

	private DataTable dtDataFromDb = new DataTable();

	private List<LabAnalysisScheduleItemAndPartyWise> lstRemoveRecordDetail = new List<LabAnalysisScheduleItemAndPartyWise>();

	private IContainer components;

	private Panel panel5;

	internal GridEX grdfrm;

	private Panel panel4;

	private Panel panel3;

	private ToolStrip tsItemTaxSchedule;

	private ToolStripButton btnnew;

	private ToolStripButton btnupdate;

	private ToolStripButton btnsave;

	private ToolStripButton btnRefresh;

	private Label label3;

	private GroupBox groupBox1;

	private Label label4;

	private TextBox txtScheduleName;

	internal UltraCombo CmbAnalysisGroup;

	private Label label44;

	private CtrlGrdBar ctrlGrdBar3;

	private RadioButton radPartyCode;

	private RadioButton RadNickName;

	private RadioButton RadBusinessName;

	internal UltraCombo CmbBuyerName;

	private RadioButton rdSearchByName;

	private RadioButton rdSearchByCode;

	private Label label9;

	internal UltraCombo CmbBrandItemName;

	private Panel panel7;

	private Label label1;

	private TabControl tabControl1;

	private TabPage tabForm;

	private TabPage tabHistory;

	internal GridEX grdHistory;

	private Panel panel8;

	private Label label2;

	internal GridEX grdSubPartyHistory;

	private Panel panel15;

	private CtrlGrdBar ctrlGrdBar1;

	private Label label43;

	private Panel panel13;

	private GroupBox groupBox2;

	private Button btnshow;

	internal UltraCombo CmbItemNameHistory;

	private Label label37;

	private RadioButton rdmodifydate;

	private RadioButton rdentrydate;

	internal UltraCombo CmbBuyerNameHistory;

	private Label label29;

	private DateTimePicker ToDateHistory;

	private Label label33;

	private DateTimePicker FromDateHistory;

	private Panel panel9;

	private Label label30;

	private ToolStrip toolStrip2;

	private ToolStripButton btnNewHistory;

	private ToolStripSeparator toolStripSeparator3;

	private ToolStripButton BtnRefreshHistory;

	internal UltraCombo CmbStandardNameHistory;

	private Label label5;

	private Label label6;

	internal UltraCombo CmbAnalysisGroupNameHistory;

	private Label label7;

	private ToolStripButton btnshortcutkeys;

	public frmLabAnalysisScheduleItemAndPartyWise(UserAccount pUserAccount)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUserAccount;
		AttachChangeHandlers((Control)(object)grdfrm);
		InitializeComponentCustom();
		InitializeComponentMethod();
		FormHelper.FormEventHandler(this);
	}

	private void InvLabAnalysisGroup_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(10, 10);
			base.AutoScrollMinSize = new Size(900, 650);
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
			if (dtSupplier.Columns.Count == 0)
			{
				dtSupplier.Columns.Add("Id", typeof(int));
				dtSupplier.Columns.Add("CompanyName", typeof(string));
				dtSupplier.Columns.Add("PartyCode", typeof(string));
				dtSupplier.Columns.Add("GlAccountId", typeof(int));
				dtSupplier.Columns.Add("CityName", typeof(string));
				dtSupplier.Columns.Add("MobileNo", typeof(string));
			}
			if (dtItem.Columns.Count == 0)
			{
				dtItem.Columns.Add("Id", typeof(int));
				dtItem.Columns.Add("ItemName");
				dtItem.Columns.Add("ItemCode");
				dtItem.Columns.Add("InventoryParentCategoriesId");
				dtItem.Columns.Add("ItemCategoryId");
				dtItem.Columns.Add("ItemCategory");
				dtItem.Columns.Add("ItemTypeId");
				dtItem.Columns.Add("ItemType");
			}
			LabAnalysisSchedule_Helper.InitializeDetailTable(dtGrid);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InitializeComponentMethod()
	{
		Task.Run(delegate
		{
			formright = CommonServices.SetRightsValueInRightsObject(ScreenName);
			DataTable historyComboData = HistoryComboDbCall();
			DataTable data = AnalysisGroupDbCall();
			LabGroupAnalysisStandardsDbCall();
			return new
			{
				Data = data,
				HistoryComboData = historyComboData
			};
		}).ContinueWith(t =>
		{
			if (t.IsFaulted)
			{
				MessageBox.Show("Error occurred during database call.");
			}
			else
			{
				var result = t.Result;
				btnsave.Enabled = formright.DoHaveSaveRight;
				btnupdate.Enabled = formright.DoHaveUpdateRights;
				FormHelper.SetCtrlGrdBarRight(ctrlGrdBar3, formright);
				ctrlGrdBar3.mGridSaveLayouts.Visible = false;
				ctrlGrdBar3.removeLayoutToolStripMenuItem.Visible = false;
				_ = RecId;
				SupplierDtFillFromGlobal();
				BindSupplierName();
				ItemDtsFillFromGlobal();
				ItemNameBind();
				AnalysisGroupBind(result.Data);
				BindGrids();
				HistoryComboBind(result.HistoryComboData);
				FromDateHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
				ToDateHistory.Value = DateTime.Now;
				FormInitialized = true;
				FieldsChanged = false;
				IsLoading = false;
				CmbAnalysisGroup.Focus();
			}
		}, TaskScheduler.FromCurrentSynchronizationContext());
	}

	private void SupplierDtFillFromGlobal()
	{
		try
		{
			dtSupplier.Clear();
			HashSet<int> excludedIds = new HashSet<int> { 14, 15, 17, 13, 12, 11, 10, 8 };
			List<getGlobalAllSupplierCustomer> customers = clsGlobalVariables.globalAllSupplierCustomer.Where((getGlobalAllSupplierCustomer r) => !excludedIds.Contains(r.CustomerGroupId)).ToList();
			if (customers == null || customers.Count == 0)
			{
				return;
			}
			bool useNickName = RadNickName.Checked;
			Dictionary<int, getGlobalAllSupplierCustomer> customerLookup = customers.ToDictionary((getGlobalAllSupplierCustomer x) => x.Id);
			dtSupplier.BeginLoadData();
			foreach (getGlobalAllSupplierCustomer item in customers)
			{
				string name = (useNickName ? (item.NickName ?? string.Empty) : (item.CompanyName ?? string.Empty));
				if (item.IsSubSupCust && item.ParentsSupCustId > 0 && customerLookup.TryGetValue(item.ParentsSupCustId, out var parent))
				{
					name = (useNickName ? (parent.NickName ?? string.Empty) : (parent.CompanyName ?? string.Empty)) + " / " + name;
				}
				dtSupplier.Rows.Add(item.Id, name, item.PartyCode, item.GlAccountId, item.CityName, item.MobilePersonal);
			}
			dtSupplier.EndLoadData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindSupplierName()
	{
		try
		{
			string ValueMember = (radPartyCode.Checked ? "PartyCode" : "CompanyName");
			string DisplayMember = (radPartyCode.Checked ? "Party Code" : "Party Name");
			InfragisticsHelper.BindAndRetainSelection(CmbBuyerName, dtSupplier, "Id", ValueMember, DisplayMember, AllColumns: true, Conversion.ToInt(CmbBuyerName.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			if (dtSupplier.Rows.Count > 0)
			{
				((UltraGridBase)CmbBuyerName).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				((UltraGridBase)CmbBuyerName).DisplayLayout.Bands[0].Columns[4].Hidden = true;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ItemDtsFillFromGlobal()
	{
		try
		{
			dtItem.Clear();
			IEnumerable<getGlobalAllItems> allItems = clsGlobalVariables.getGlobalAllItems.Where((getGlobalAllItems item) => item.ItemTypeOfTypeId != 14 && item.ItemTypeOfTypeId != 17);
			if (allItems == null || !allItems.Any())
			{
				return;
			}
			foreach (getGlobalAllItems row in allItems)
			{
				dtItem.Rows.Add(row.Id, row.ItemName, row.ItemCode, row.InventoryParentCategoriesId, row.ItemCategoryId, row.ItemCategory, row.ItemTypeId, row.ItemType);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error loading items: " + ex.Message);
		}
	}

	private void ItemNameBind()
	{
		try
		{
			int Id = Conversion.ToInt(CmbBrandItemName.Value);
			string ValueMember = (rdSearchByName.Checked ? "ItemName" : "ItemCode");
			string DisplayMember = (rdSearchByName.Checked ? "Item Name" : "Item Code");
			InfragisticsHelper.BindAndRetainSelection(CmbBrandItemName, dtItem, "Id", ValueMember, DisplayMember, AllColumns: true, Id, insertDefaultRow: true, null, null, ActivateRow: false);
			if (dtItem.Rows.Count > 0)
			{
				((UltraGridBase)CmbBrandItemName).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				((UltraGridBase)CmbBrandItemName).DisplayLayout.Bands[0].Columns[4].Hidden = true;
				((UltraGridBase)CmbBrandItemName).DisplayLayout.Bands[0].Columns[6].Hidden = true;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void rdSearchByName_CheckedChanged(object sender, EventArgs e)
	{
		ItemNameBind();
	}

	private void RadBusinessName_CheckedChanged(object sender, EventArgs e)
	{
		SupplierDtFillFromGlobal();
		BindSupplierName();
	}

	private DataTable AnalysisGroupDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			return InvLabAnalysisGroup.GetAllOrById(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void AnalysisGroupBind(DataTable dt)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbAnalysisGroup, dt, "Id", "AnalysisGroupDescription", "Analysis Group", AllColumns: false, Conversion.ToInt(CmbAnalysisGroup.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbAnalysisGroup_Leave(object sender, EventArgs e)
	{
		try
		{
			int Id = Conversion.ToInt(CmbAnalysisGroup.Value);
			string GroupName = Conversion.ToString((object)((Control)(object)CmbAnalysisGroup).Text);
			if (grdfrm.RowCount > 0)
			{
				string GridGroup = GridEX_Helper.GetFirstOrDefaultValueOfColumn(grdfrm, "AnalysisGroup");
				if (FieldsChanged && !string.IsNullOrEmpty(GridGroup) && GridGroup != GroupName)
				{
					if (MessageBox.Show("Analysis Group : " + GridGroup + " has been changed to " + GroupName + ". Do you want to discard changes and continue?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						int previousValue = Conversion.ToInt((object)GridEX_Helper.GetFirstOrDefaultValueOfColumn(grdfrm, "AnalysisGroupId"));
						CmbAnalysisGroup.Value = previousValue;
					}
					else
					{
						LoadGroup(Id);
					}
				}
				else
				{
					bool PreviousFieldsChanged = FieldsChanged;
					LoadGroup(Id);
					FieldsChanged = PreviousFieldsChanged;
				}
			}
			else
			{
				LoadGroup(Id);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AttachChangeHandlers(Control parent)
	{
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Expected O, but got Unknown
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Expected O, but got Unknown
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Expected O, but got Unknown
		foreach (Control ctrl in parent.Controls)
		{
			if (ctrl is TextBox txtBox)
			{
				txtBox.TextChanged += delegate
				{
					FieldsChanged = true;
				};
			}
			else if (ctrl is DateTimePicker dateTime)
			{
				dateTime.ValueChanged += delegate
				{
					FieldsChanged = true;
				};
			}
			else
			{
				GridEX grid = (GridEX)(object)((ctrl is GridEX) ? ctrl : null);
				if (grid != null)
				{
					grid.CellValueChanged += (ColumnActionEventHandler)delegate
					{
						if (!IsLoading)
						{
							FieldsChanged = true;
						}
					};
					grid.CellEdited += (ColumnActionEventHandler)delegate
					{
						if (!IsLoading)
						{
							FieldsChanged = true;
						}
					};
					grid.UpdatingRecord += delegate
					{
						if (!IsLoading)
						{
							FieldsChanged = true;
						}
					};
					grid.AddingRecord += delegate
					{
						if (!IsLoading)
						{
							FieldsChanged = true;
						}
					};
					grid.DeletingRecord += (RowActionCancelEventHandler)delegate
					{
						if (!IsLoading)
						{
							FieldsChanged = true;
						}
					};
				}
			}
			if (ctrl.HasChildren)
			{
				AttachChangeHandlers(ctrl);
			}
		}
	}

	private void LoadGroup(int id)
	{
		try
		{
			IsLoading = true;
			GridFillByAnalysisGroup(id);
			FieldsChanged = false;
		}
		finally
		{
			IsLoading = false;
		}
	}

	private void Insert()
	{
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Expected O, but got Unknown
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Expected O, but got Unknown
		try
		{
			if (grdfrm.RowCount == 0)
			{
				MessageBox.Show("At least one record is required in grid.");
			}
			else
			{
				if (!FormHelper.ValidateControls(new List<(Control, string, FormHelper.ValidType)>
				{
					((Control)(object)CmbAnalysisGroup, "Analysis Group", FormHelper.ValidType.INT),
					(txtScheduleName, "Schedule Name", FormHelper.ValidType.String),
					((Control)(object)CmbBrandItemName, "Brand Item", FormHelper.ValidType.INT)
				}) || !FormHelper.ConfirmAction((RecId == 0) ? "Are you sure to Save" : "Are you sure to Update"))
				{
					return;
				}
				LabAnalysisScheduleItemAndPartyWise Or = new LabAnalysisScheduleItemAndPartyWise();
				Or.LabAnalysisScheduleItemAndPartyWiseId = RecId;
				Or.OrganizationId = UserAccount.OrganizationId;
				Or.CompanyId = UserAccount.CompanyId;
				Or.EntryUserId = UserAccount.ID;
				Or.EntryDate = DateTime.Now;
				Or.ModifyDate = DateTime.Now;
				Or.ModifyUserId = UserAccount.ID;
				Or.LabAnalysisScheduleItemAndPartyWiseList = new List<LabAnalysisScheduleItemAndPartyWise>();
				if (lstRemoveRecordDetail.Count > 0 && RecId > 0)
				{
					for (int i = 0; i < lstRemoveRecordDetail.Count; i++)
					{
						Or.LabAnalysisScheduleItemAndPartyWiseList.Add(lstRemoveRecordDetail[i]);
					}
				}
				bool hasActiveRow = false;
				int MainId = Conversion.ToInt((object)LabAnalysisScheduleItemAndPartyWise.GetMainId(UserAccount.OrganizationId, UserAccount.CompanyId));
				GridEXRow[] rows = grdfrm.GetRows();
				foreach (GridEXRow r in rows)
				{
					LabAnalysisScheduleItemAndPartyWise vd = new LabAnalysisScheduleItemAndPartyWise();
					vd.LabAnalysisScheduleItemAndPartyWiseId = ((RecId != 0) ? Conversion.ToInt(r.Cells["Id"].Value) : 0);
					vd.ActionId = ((vd.LabAnalysisScheduleItemAndPartyWiseId <= 0) ? 1 : 2);
					vd.MainId = MainId;
					FillLabGroupAnalysisStandardsFromGrid(vd, r);
					FormHelper.ValidateField(vd.LabAnalysisGroupId, "Analysis Group", r.RowIndex);
					FormHelper.ValidateField(vd.LabAnalysisItemId, "Analysis Item", r.RowIndex);
					FormHelper.ValidateField(vd.MaxValue, "MaxValue", r.RowIndex);
					Or.LabAnalysisScheduleItemAndPartyWiseList.Add(vd);
					if (vd.Active)
					{
						hasActiveRow = true;
					}
				}
				if (!hasActiveRow)
				{
					throw new ArgumentException("At least one row must have Active = 1.");
				}
				int success = LabAnalysisScheduleItemAndPartyWise.Save(Or);
				Or.LabAnalysisScheduleItemAndPartyWiseId = success;
				if (RecId > 0)
				{
					MessageBox.Show("Data Update Successfully....  " + Or.ScheduleName);
				}
				else
				{
					MessageBox.Show("Data Save Successfully....  " + Or.ScheduleName);
				}
				Reset();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillLabGroupAnalysisStandardsFromGrid(LabAnalysisScheduleItemAndPartyWise standards, GridEXRow r)
	{
		standards.ScheduleName = Conversion.ToString((object)txtScheduleName.Text);
		standards.BrandItemId = Conversion.ToInt(CmbBrandItemName.Value);
		standards.BrandItemName = Conversion.ToString(((UltraDropDownBase)CmbBrandItemName).SelectedRow.Cells["ItemName"].Value);
		standards.BrandItemCode = Conversion.ToString(((UltraDropDownBase)CmbBrandItemName).SelectedRow.Cells["ItemCode"].Value);
		standards.SupplierCustomerId = Conversion.ToInt(CmbBuyerName.Value);
		standards.PartyName = Conversion.ToString((object)((Control)(object)CmbBuyerName).Text);
		standards.LabAnalysisGroupId = Conversion.ToInt(r.Cells["AnalysisGroupId"].Value);
		standards.LabAnalysisGroupDescription = Conversion.ToString(r.Cells["AnalysisGroup"].Value);
		standards.LabAnalysisItemId = Conversion.ToInt(r.Cells["AnalysisItemId"].Value);
		standards.LabAnalysisItemName = Conversion.ToString(r.Cells["AnalysisItem"].Value);
		standards.MinValue = Conversion.ToDecimal(r.Cells["MinValue"].Value);
		standards.MaxValue = Conversion.ToDecimal(r.Cells["MaxValue"].Value);
		standards.Active = Conversion.ToBool(r.Cells["Active"].Value);
		standards.OrganizationId = UserAccount.OrganizationId;
		standards.CompanyId = UserAccount.CompanyId;
		standards.EntryUserId = UserAccount.ID;
		standards.EntryDate = DateTime.Now;
		standards.ModifyDate = DateTime.Now;
		standards.ModifyUserId = UserAccount.ID;
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

	private void btnupdate_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId == 0)
			{
				MessageBox.Show("No record selected to update.");
			}
			else
			{
				Insert();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void ReadById(int Id)
	{
		try
		{
			Reset();
			RecId = Id;
			LabAnalysisScheduleItemAndPartyWise obj = LabAnalysisScheduleItemAndPartyWise.ReadByMainId(RecId);
			if (obj == null || obj.LabAnalysisScheduleItemAndPartyWiseList.Count == 0)
			{
				MessageBox.Show("No Record Found");
				return;
			}
			tabControl1.SelectedIndex = 0;
			UpdateMode = true;
			btnsave.Visible = false;
			btnupdate.Visible = true;
			LabAnalysisScheduleItemAndPartyWise item = obj.LabAnalysisScheduleItemAndPartyWiseList[0];
			CmbAnalysisGroup.Value = Conversion.ToInt((object)item.LabAnalysisGroupId);
			txtScheduleName.Text = Conversion.ToString((object)item.ScheduleName);
			CmbBuyerName.Value = Conversion.ToInt((object)item.SupplierCustomerId);
			CmbBrandItemName.Value = Conversion.ToInt((object)item.BrandItemId);
			dtGrid.Rows.Clear();
			LabAnalysisSchedule_Helper.FillDetailFromList(obj, dtGrid);
			BindGrids();
			txtScheduleName.Focus();
			FieldsChanged = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindGrids()
	{
		try
		{
			DataTable dataTable = dtGrid;
			if (dataTable != null && dataTable.Columns.Count > 0)
			{
				grdfrm.DataSource = dtGrid;
				grdfrm.RetrieveStructure();
				grdfrmSetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LabGroupAnalysisStandardsDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			dtDataFromDb = InvLabGroupAnalysisStandards.GetAllOrById(obj);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeleteAllGridRows()
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		GridEXRow[] dataRows = grdfrm.GetDataRows();
		foreach (GridEXRow row in dataRows)
		{
			int id = Conversion.ToInt(row.Cells["Id"].Value);
			if (id != 0)
			{
				LabAnalysisScheduleItemAndPartyWise d = new LabAnalysisScheduleItemAndPartyWise
				{
					LabAnalysisScheduleItemAndPartyWiseId = id,
					ActionId = 3
				};
				FillLabGroupAnalysisStandardsFromGrid(d, row);
				lstRemoveRecordDetail.Add(d);
			}
			dtGrid.Rows[row.RowIndex].Delete();
		}
	}

	private void GridFillByAnalysisGroup(int GroupId)
	{
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Expected O, but got Unknown
		try
		{
			if (GroupId <= 0)
			{
				DeleteAllGridRows();
				BindGrids();
				return;
			}
			List<DataRow> list = (from r in dtDataFromDb.AsEnumerable()
				where Conversion.ToInt(r["InvLabAnalysisGroup"]) == GroupId
				select r).ToList();
			if (list.Count == 0)
			{
				MessageBox.Show("No data found.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				return;
			}
			HashSet<string> sourceKeys = new HashSet<string>(list.Count);
			foreach (DataRow r4 in list)
			{
				sourceKeys.Add(string.Concat(r4["InvLabAnalysisGroup"], "_", r4["InvLabAnalysisItems"]));
			}
			for (int i = dtGrid.Rows.Count - 1; i >= 0; i--)
			{
				DataRow row2 = dtGrid.Rows[i];
				string key2 = string.Concat(row2["AnalysisGroupId"], "_", row2["AnalysisItemId"]);
				if (!sourceKeys.Contains(key2))
				{
					int id = Conversion.ToInt(row2["Id"]);
					if (id != 0)
					{
						LabAnalysisScheduleItemAndPartyWise d = new LabAnalysisScheduleItemAndPartyWise
						{
							LabAnalysisScheduleItemAndPartyWiseId = id,
							ActionId = 3
						};
						GridEXRow r3 = GridEX_Helper.GetRowByColumnNameAndValue(grdfrm, "Id", id);
						FillLabGroupAnalysisStandardsFromGrid(d, r3);
						lstRemoveRecordDetail.Add(d);
					}
					dtGrid.Rows.RemoveAt(i);
				}
			}
			HashSet<string> existingKeys = new HashSet<string>();
			foreach (DataRow row in dtGrid.Rows)
			{
				existingKeys.Add(string.Concat(row["AnalysisGroupId"], "_", row["AnalysisItemId"]));
			}
			dtGrid.BeginLoadData();
			foreach (DataRow r2 in list)
			{
				string key = string.Concat(r2["InvLabAnalysisGroup"], "_", r2["InvLabAnalysisItems"]);
				if (!existingKeys.Contains(key))
				{
					dtGrid.Rows.Add(0, r2["InvLabAnalysisGroup"], r2["AnalysisGroupDescription"], r2["InvLabAnalysisItems"], r2["AnalysisParameterDescription"], r2["MinValue"], r2["MaxValue"], false);
				}
			}
			dtGrid.EndLoadData();
			BindGrids();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grdfrmSetting()
	{
		try
		{
			Dictionary<string, EditType> editableColumns = new Dictionary<string, EditType>
			{
				{
					"MinValue",
					(EditType)1
				},
				{
					"MaxValue",
					(EditType)1
				},
				{
					"Active",
					(EditType)8
				},
				{
					"Remarks",
					(EditType)1
				}
			};
			LabAnalysisSchedule_Helper.DetailGridCommonSetting(grdfrm, editableColumns);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdfrm_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			GridEXRow item = grdfrm.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				Conversion.ToInt(item.Cells["Id"].Value);
				if (e.Control)
				{
					_ = e.KeyCode;
					_ = 13;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdfrm_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow r = grdfrm.CurrentRow;
			if (r != null)
			{
				if (e.Column.Key == "Delete")
				{
					DeleteCustomerDetailRow(r);
				}
				grdfrm.UpdateData();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeleteCustomerDetailRow(GridEXRow r)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected O, but got Unknown
		try
		{
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			if (Conversion.ToInt((object)r.Cells["Id"].Value.ToString()) != 0)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					LabAnalysisScheduleItemAndPartyWise d = new LabAnalysisScheduleItemAndPartyWise();
					d.LabAnalysisScheduleItemAndPartyWiseId = Conversion.ToInt(r.Cells["Id"].Value);
					FillLabGroupAnalysisStandardsFromGrid(d, r);
					d.ActionId = 3;
					lstRemoveRecordDetail.Add(d);
					dtGrid.Rows[r.RowIndex].Delete();
					r.Delete();
				}
			}
			else
			{
				dtGrid.Rows[r.RowIndex].Delete();
				r.Delete();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		Reset();
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		List<string> allServices = new List<string> { "SupplierCustomerLists", "AllItems" };
		DatatableHelper.GlobalServicesDbCall(UserAccount, allServices);
		SupplierDtFillFromGlobal();
		BindSupplierName();
		ItemDtsFillFromGlobal();
		ItemNameBind();
		AnalysisGroupBind(AnalysisGroupDbCall());
		LabGroupAnalysisStandardsDbCall();
	}

	private void frmLabAnalysisScheduleItemAndPartyWise_FormClosing(object sender, FormClosingEventArgs e)
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

	private void Reset()
	{
		try
		{
			RecId = 0;
			lstRemoveRecordDetail.Clear();
			btnsave.Visible = true;
			btnupdate.Visible = false;
			CmbAnalysisGroup.Value = null;
			txtScheduleName.Text = string.Empty;
			CmbBrandItemName.Value = null;
			dtGrid.Clear();
			BindGrids();
			txtScheduleName.Focus();
			FieldsChanged = false;
			IsLoading = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnNewHistory_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)(object)CmbItemNameHistory).Text = string.Empty;
			((Control)(object)CmbStandardNameHistory).Text = string.Empty;
			((Control)(object)CmbBuyerNameHistory).Text = string.Empty;
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
			HistoryComboBind(HistoryComboDbCall());
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private DataTable HistoryComboDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			return LabAnalysisScheduleItemAndPartyWise.GetDataForDropDown(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void HistoryComboBind(DataTable dtcombo)
	{
		try
		{
			DataTable dtAG = new DataTable();
			dtAG.Columns.Add("Id", typeof(int));
			dtAG.Columns.Add("Name");
			DataTable dtScheduleNo = dtAG.Clone();
			DataTable dtPartyName = dtAG.Clone();
			DataTable dtBrandItem = dtAG.Clone();
			if (dtcombo.Rows.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < dtcombo.Rows.Count; i++)
			{
				if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "AnalysisGroup")
				{
					dtAG.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["Name"]);
				}
				else if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "ScheduleName")
				{
					dtScheduleNo.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["Name"]);
				}
				else if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "PartyName")
				{
					dtPartyName.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["Name"]);
				}
				else if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "BrandItem")
				{
					dtBrandItem.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["Name"]);
				}
			}
			InfragisticsHelper.BindAndRetainSelection(CmbBuyerNameHistory, dtPartyName, "Id", "Name", "Party Name", AllColumns: true, Conversion.ToInt(CmbBuyerNameHistory.Value), insertDefaultRow: true, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbItemNameHistory, dtBrandItem, "Id", "Name", "Brand Item", AllColumns: true, Conversion.ToInt(CmbItemNameHistory.Value), insertDefaultRow: true, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbAnalysisGroupNameHistory, dtAG, "Id", "Name", "Analysis Group", AllColumns: true, Conversion.ToInt(CmbAnalysisGroupNameHistory.Value), insertDefaultRow: true, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbStandardNameHistory, dtScheduleNo, "Id", "Name", "Schedule Name", AllColumns: true, Conversion.ToInt(CmbStandardNameHistory.Value), insertDefaultRow: true, null, null, ActivateRow: false);
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
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		try
		{
			dtHistoryFromDb.Clear();
			ReportsParameters obj = new ReportsParameters();
			obj.CompanyId = UserAccount.CompanyId;
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			obj.EntryUser = UserAccount.ID;
			if (rdentrydate.Checked)
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
			obj.ItemId = Conversion.ToInt(CmbItemNameHistory.Value);
			obj.SupplierCustomerId = Conversion.ToInt(CmbBuyerNameHistory.Value);
			obj.Id = Conversion.ToInt(CmbStandardNameHistory.Value);
			dtHistoryFromDb = LabAnalysisScheduleItemAndPartyWise.FormHistory(obj);
			DataTable dataTable = dtHistoryFromDb;
			if (dataTable == null || dataTable.Rows.Count <= 0)
			{
				return;
			}
			DataTable dataTable2 = dtHistoryGrid;
			if (dataTable2 != null && dataTable2.Columns.Count == 0)
			{
				dtHistoryGrid.Columns.Add("Id", typeof(int));
				dtHistoryGrid.Columns.Add("MainId", typeof(int));
				dtHistoryGrid.Columns.Add("LabAnalysisGroup", typeof(string));
				dtHistoryGrid.Columns.Add("ScheduleName", typeof(string));
				dtHistoryGrid.Columns.Add("BrandItemName", typeof(string));
				dtHistoryGrid.Columns.Add("BrandItemCode", typeof(string));
				dtHistoryGrid.Columns.Add("PartyName", typeof(string));
				dtHistoryGrid.Columns.Add("EntryUser", typeof(string));
				dtHistoryGrid.Columns.Add("EntryDate", typeof(DateTime));
				dtHistoryGrid.Columns.Add("ModifyUser", typeof(string));
				dtHistoryGrid.Columns.Add("ModifyDate", typeof(DateTime));
			}
			dtHistoryGrid.Clear();
			HashSet<int> Ids = new HashSet<int>();
			foreach (DataRow row in dtHistoryFromDb.Rows)
			{
				int MainId = Conversion.ToInt(row["MainId"]);
				if (Ids.Add(MainId))
				{
					DataRow dr = dtHistoryGrid.NewRow();
					int Id = Conversion.ToInt(row["LabAnalysisScheduleItemAndPartyWiseId"]);
					dr["Id"] = Id;
					dr["MainId"] = MainId;
					dr["ScheduleName"] = Conversion.ToString(row["ScheduleName"]);
					dr["PartyName"] = Conversion.ToString(row["PartyName"]);
					dr["BrandItemName"] = Conversion.ToString(row["BrandItemName"]);
					dr["BrandItemCode"] = Conversion.ToString(row["BrandItemCode"]);
					dr["LabAnalysisGroup"] = Conversion.ToString(row["LabAnalysisGroupDescription"]);
					dr["EntryUser"] = Conversion.ToString(row["EntryUserName"]);
					dr["EntryDate"] = Conversion.ToDateTime(row["EntryDate"]);
					dr["ModifyUser"] = Conversion.ToString(row["ModifyUserName"]);
					dr["ModifyDate"] = ((row["ModifyDate"] == DBNull.Value) ? DBNull.Value : ((object)Conversion.ToDateTime(row["ModifyDate"])));
					dtHistoryGrid.Rows.Add(dr);
				}
			}
			grdHistory.DataSource = dtHistoryGrid;
			grdHistory.RetrieveStructure();
			GridSettingHistory();
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
			List<string> hiddenColumns = new List<string> { "Id", "MainId" };
			List<string> NotTotalColumns = new List<string> { "MinValue", "MaxValue" };
			GridEX_Helper.GridWrappingAndColumnSettings(grdHistory, 3, 3, hiddenColumns, null, null, ColumnSetting: true, BracketsForamt: false, NotTotalColumns);
			grdHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yy hh:mm tt";
			grdHistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yy hh:mm tt";
			grdHistory.RootTable.Columns["LabAnalysisGroup"].Width = 150;
			grdHistory.RootTable.Columns["ScheduleName"].Width = 150;
			grdHistory.RootTable.Columns["BrandItemName"].Width = Constants.InventoryConstants.ItemName;
			grdHistory.RootTable.Columns["BrandItemCode"].Width = 100;
			grdHistory.RootTable.Columns["PartyName"].Width = Constants.InventoryConstants.SupplierName;
			grdHistory.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants + 20;
			grdHistory.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			grdHistory.RootTable.Columns["ModifyUser"].Width = Constants.InventoryConstants.ModifyUserConstants + 20;
			grdHistory.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			GridEX_Helper.AddButton(grdHistory, "Edit", "Edit", 40, 0);
			grdHistory.FrozenColumns = 3;
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
				int Id = Conversion.ToInt(r.Cells["MainId"].Value);
				ReadById(Id);
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
			if (r != null && (int)r.RowType == 0)
			{
				int Id = Conversion.ToInt(r.Cells["MainId"].Value);
				if (e.Column.Key == "Edit")
				{
					ReadById(Id);
				}
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
				GetDetailGrdByHeadId(Conversion.ToInt(row.Cells["MainId"].Value));
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
			if (Id == 0)
			{
				grdSubPartyHistory.ClearStructure();
				return;
			}
			DataTable dtData = (from r in dtHistoryFromDb.AsEnumerable()
				where Conversion.ToInt(r["MainId"]) == Id
				select r).CopyToDataTable();
			DataTable dtDetailHistory = dtGrid.Clone();
			LabAnalysisSchedule_Helper.FillDetailFromDataTable(dtData, dtDetailHistory);
			grdSubPartyHistory.DataSource = dtDetailHistory;
			grdSubPartyHistory.RetrieveStructure();
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
			LabAnalysisSchedule_Helper.DetailGridCommonSetting(grdSubPartyHistory);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
	}

	private void ctrlGrdBar3_Load(object sender, EventArgs e)
	{
	}

	private void frmLabAnalysisScheduleItemAndPartyWise_KeyDown(object sender, KeyEventArgs e)
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
					CmbAnalysisGroup.Focus();
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
					if (btnupdate.Visible && btnupdate.Enabled && UpdateMode)
					{
						btnupdate_Click(null, null);
					}
				}
				else if (e.Control && e.KeyCode == Keys.N)
				{
					btnnew_Click(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefresh_Click(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.F5)
				{
					CmbAnalysisGroup.Focus();
				}
				else if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grdfrm).Focus();
				}
				else if (e.Control && e.KeyCode == Keys.Up)
				{
					CmbAnalysisGroup.Focus();
				}
			}
			else if (tabControl1.SelectedIndex == 1)
			{
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grdSubPartyHistory).Focus();
				}
				else if (e.Control && e.KeyCode == Keys.S)
				{
					btnshow_Click(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.Up)
				{
					((Control)(object)grdHistory).Focus();
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
			dt.Rows.Add("Ctrl+F5", "For Focus on Doc Date");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("F1", "For Combo Lookup on Current Column of any Focused Grid");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+D", "For Adding an row in Focused Grid");
			dt.Rows.Add("Ctrl+Delete", "For Deleting an row of Focused Grid");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On on Doc Date");
			dt.Rows.Add("Ctrl+ArrowRight", "to change focus from one grid to another");
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

	private void btnshortcutkeys_Click(object sender, EventArgs e)
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
			int CurrId = Conversion.ToInt(r.Cells["MainId"].Value);
			if (e.Control && e.KeyCode == Keys.Space && grdHistory.CurrentColumn.Key == "Edit")
			{
				if (!formright.DoHaveUpdateRights)
				{
					throw new Exception("you don't have update rights...");
				}
				ReadById(CurrId);
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				if (!formright.DoHaveUpdateRights)
				{
					throw new Exception("you don't have update rights...");
				}
				ReadById(CurrId);
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
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_0272: Expected O, but got Unknown
		//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0301: Expected O, but got Unknown
		//IL_0302: Unknown result type (might be due to invalid IL or missing references)
		//IL_030c: Expected O, but got Unknown
		//IL_030d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0317: Expected O, but got Unknown
		//IL_03b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bc: Expected O, but got Unknown
		//IL_03d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03dd: Expected O, but got Unknown
		//IL_0436: Unknown result type (might be due to invalid IL or missing references)
		//IL_0440: Expected O, but got Unknown
		//IL_044c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0456: Expected O, but got Unknown
		//IL_0462: Unknown result type (might be due to invalid IL or missing references)
		//IL_046c: Expected O, but got Unknown
		//IL_0478: Unknown result type (might be due to invalid IL or missing references)
		//IL_0482: Expected O, but got Unknown
		//IL_0726: Unknown result type (might be due to invalid IL or missing references)
		//IL_0730: Expected O, but got Unknown
		//IL_250f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2519: Expected O, but got Unknown
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Lab.frmLabAnalysisScheduleItemAndPartyWise));
		GridEXLayout grdHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdSubPartyHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance79 = new Appearance();
		this.grdfrm = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel4 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.panel7 = new System.Windows.Forms.Panel();
		this.RadBusinessName = new System.Windows.Forms.RadioButton();
		this.RadNickName = new System.Windows.Forms.RadioButton();
		this.radPartyCode = new System.Windows.Forms.RadioButton();
		this.label1 = new System.Windows.Forms.Label();
		this.rdSearchByName = new System.Windows.Forms.RadioButton();
		this.rdSearchByCode = new System.Windows.Forms.RadioButton();
		this.label9 = new System.Windows.Forms.Label();
		this.CmbBrandItemName = new UltraCombo();
		this.CmbBuyerName = new UltraCombo();
		this.CmbAnalysisGroup = new UltraCombo();
		this.label44 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.txtScheduleName = new System.Windows.Forms.TextBox();
		this.label3 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.tsItemTaxSchedule = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnshortcutkeys = new System.Windows.Forms.ToolStripButton();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabForm = new System.Windows.Forms.TabPage();
		this.tabHistory = new System.Windows.Forms.TabPage();
		this.grdHistory = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.label2 = new System.Windows.Forms.Label();
		this.grdSubPartyHistory = new GridEX();
		this.panel15 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label43 = new System.Windows.Forms.Label();
		this.panel13 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.btnshow = new System.Windows.Forms.Button();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.CmbItemNameHistory = new UltraCombo();
		this.label37 = new System.Windows.Forms.Label();
		this.CmbAnalysisGroupNameHistory = new UltraCombo();
		this.label7 = new System.Windows.Forms.Label();
		this.CmbStandardNameHistory = new UltraCombo();
		this.label5 = new System.Windows.Forms.Label();
		this.CmbBuyerNameHistory = new UltraCombo();
		this.label29 = new System.Windows.Forms.Label();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.label6 = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.panel9 = new System.Windows.Forms.Panel();
		this.label30 = new System.Windows.Forms.Label();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.BtnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		((System.ComponentModel.ISupportInitialize)this.grdfrm).BeginInit();
		this.panel5.SuspendLayout();
		this.panel4.SuspendLayout();
		this.groupBox1.SuspendLayout();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBrandItemName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBuyerName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAnalysisGroup).BeginInit();
		this.panel3.SuspendLayout();
		this.tsItemTaxSchedule.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabForm.SuspendLayout();
		this.tabHistory.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).BeginInit();
		this.panel8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdSubPartyHistory).BeginInit();
		this.panel15.SuspendLayout();
		this.panel13.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbItemNameHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAnalysisGroupNameHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbStandardNameHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBuyerNameHistory).BeginInit();
		this.panel9.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.grdfrm.AllowEdit = (InheritableBoolean)2;
		((System.Windows.Forms.Control)(object)this.grdfrm).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdfrm.FilterMode = (FilterMode)2;
		this.grdfrm.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdfrm).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdfrm.GroupByBoxVisible = false;
		this.grdfrm.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Location = new System.Drawing.Point(3, 146);
		((System.Windows.Forms.Control)(object)this.grdfrm).Name = "grdfrm";
		((ScrollableBase)this.grdfrm).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdfrm).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdfrm).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Size = new System.Drawing.Size(870, 354);
		((System.Windows.Forms.Control)(object)this.grdfrm).TabIndex = 1;
		this.grdfrm.TotalRow = (InheritableBoolean)1;
		this.grdfrm.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdfrm).VisualStyle = (VisualStyle)4;
		this.grdfrm.ColumnButtonClick += new ColumnActionEventHandler(grdfrm_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdfrm).KeyDown += new System.Windows.Forms.KeyEventHandler(grdfrm_KeyDown);
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.ctrlGrdBar3);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(3, 125);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(870, 21);
		this.panel5.TabIndex = 80;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(827, 0);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar3.MyGrid = this.grdfrm;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(43, 20);
		this.ctrlGrdBar3.TabIndex = 128;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(3, 56);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(870, 69);
		this.panel4.TabIndex = 0;
		this.groupBox1.Controls.Add(this.panel7);
		this.groupBox1.Controls.Add(this.rdSearchByName);
		this.groupBox1.Controls.Add(this.rdSearchByCode);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBrandItemName);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBuyerName);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbAnalysisGroup);
		this.groupBox1.Controls.Add(this.label44);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.txtScheduleName);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(3, 3);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(827, 67);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Main Information";
		this.panel7.Controls.Add(this.RadBusinessName);
		this.panel7.Controls.Add(this.RadNickName);
		this.panel7.Controls.Add(this.radPartyCode);
		this.panel7.Controls.Add(this.label1);
		this.panel7.Location = new System.Drawing.Point(498, 13);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(326, 22);
		this.panel7.TabIndex = 124;
		this.RadBusinessName.AutoSize = true;
		this.RadBusinessName.Checked = true;
		this.RadBusinessName.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadBusinessName.Location = new System.Drawing.Point(45, 3);
		this.RadBusinessName.Name = "RadBusinessName";
		this.RadBusinessName.Size = new System.Drawing.Size(123, 17);
		this.RadBusinessName.TabIndex = 63;
		this.RadBusinessName.TabStop = true;
		this.RadBusinessName.Text = "Business Name";
		this.RadBusinessName.UseVisualStyleBackColor = true;
		this.RadBusinessName.CheckedChanged += new System.EventHandler(RadBusinessName_CheckedChanged);
		this.RadNickName.AutoSize = true;
		this.RadNickName.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadNickName.Location = new System.Drawing.Point(174, 3);
		this.RadNickName.Name = "RadNickName";
		this.RadNickName.Size = new System.Drawing.Size(94, 17);
		this.RadNickName.TabIndex = 65;
		this.RadNickName.Text = "Nick Name";
		this.RadNickName.UseVisualStyleBackColor = true;
		this.RadNickName.CheckedChanged += new System.EventHandler(RadBusinessName_CheckedChanged);
		this.radPartyCode.AutoSize = true;
		this.radPartyCode.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.radPartyCode.Location = new System.Drawing.Point(269, 4);
		this.radPartyCode.Name = "radPartyCode";
		this.radPartyCode.Size = new System.Drawing.Size(57, 17);
		this.radPartyCode.TabIndex = 65;
		this.radPartyCode.Text = "Code";
		this.radPartyCode.UseVisualStyleBackColor = true;
		this.radPartyCode.CheckedChanged += new System.EventHandler(RadBusinessName_CheckedChanged);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Bold);
		this.label1.Location = new System.Drawing.Point(3, 4);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(42, 13);
		this.label1.TabIndex = 121;
		this.label1.Text = "Party";
		this.rdSearchByName.AutoSize = true;
		this.rdSearchByName.Checked = true;
		this.rdSearchByName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdSearchByName.Location = new System.Drawing.Point(329, 15);
		this.rdSearchByName.Name = "rdSearchByName";
		this.rdSearchByName.Size = new System.Drawing.Size(57, 19);
		this.rdSearchByName.TabIndex = 123;
		this.rdSearchByName.TabStop = true;
		this.rdSearchByName.Text = "Name";
		this.rdSearchByName.UseVisualStyleBackColor = true;
		this.rdSearchByName.CheckedChanged += new System.EventHandler(rdSearchByName_CheckedChanged);
		this.rdSearchByCode.AutoSize = true;
		this.rdSearchByCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdSearchByCode.Location = new System.Drawing.Point(386, 15);
		this.rdSearchByCode.Name = "rdSearchByCode";
		this.rdSearchByCode.Size = new System.Drawing.Size(52, 19);
		this.rdSearchByCode.TabIndex = 122;
		this.rdSearchByCode.Text = "Code";
		this.rdSearchByCode.UseVisualStyleBackColor = true;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(297, 17);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(32, 15);
		this.label9.TabIndex = 121;
		this.label9.Text = "Item";
		this.CmbBrandItemName.AllowNull = (DefaultableBoolean)1;
		this.CmbBrandItemName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBrandItemName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBrandItemName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBrandItemName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.CmbBrandItemName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBrandItemName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance54).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance54).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance54).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrandItemName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance54;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrandItemName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance55;
		((SpecialBoxBase)((UltraGridBase)this.CmbBrandItemName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance23).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance23).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBrandItemName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbBrandItemName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBrandItemName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance59).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBrandItemName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBrandItemName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbBrandItemName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBrandItemName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrandItemName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance26).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBrandItemName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbBrandItemName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBrandItemName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBrandItemName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance29).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBrandItemName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbBrandItemName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBrandItemName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBrandItemName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbBrandItemName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBrandItemName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbBrandItemName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBrandItemName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBrandItemName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBrandItemName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBrandItemName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBrandItemName).Location = new System.Drawing.Point(296, 36);
		((System.Windows.Forms.Control)(object)this.CmbBrandItemName).Name = "CmbBrandItemName";
		((System.Windows.Forms.Control)(object)this.CmbBrandItemName).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.CmbBrandItemName).TabIndex = 2;
		this.CmbBuyerName.AllowNull = (DefaultableBoolean)1;
		this.CmbBuyerName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBuyerName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBuyerName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBuyerName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance49;
		((SpecialBoxBase)((UltraGridBase)this.CmbBuyerName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance50).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance50).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance50).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance51).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance51;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance52).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance15).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance28).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance28).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance28).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance28;
		((AppearanceBase)appearance17).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBuyerName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBuyerName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBuyerName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBuyerName).Location = new System.Drawing.Point(498, 36);
		((System.Windows.Forms.Control)(object)this.CmbBuyerName).Name = "CmbBuyerName";
		((System.Windows.Forms.Control)(object)this.CmbBuyerName).Size = new System.Drawing.Size(326, 26);
		((System.Windows.Forms.Control)(object)this.CmbBuyerName).TabIndex = 3;
		this.CmbAnalysisGroup.AllowNull = (DefaultableBoolean)1;
		this.CmbAnalysisGroup.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbAnalysisGroup.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbAnalysisGroup.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.Appearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance9).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance9).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance9).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance10;
		((SpecialBoxBase)((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance11).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance11).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance11).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance13).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance16).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance18).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance18).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance18).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance18;
		((AppearanceBase)appearance20).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbAnalysisGroup).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbAnalysisGroup).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbAnalysisGroup.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbAnalysisGroup).Location = new System.Drawing.Point(2, 36);
		((System.Windows.Forms.Control)(object)this.CmbAnalysisGroup).Name = "CmbAnalysisGroup";
		((System.Windows.Forms.Control)(object)this.CmbAnalysisGroup).Size = new System.Drawing.Size(150, 26);
		((System.Windows.Forms.Control)(object)this.CmbAnalysisGroup).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbAnalysisGroup).TextChanged += new System.EventHandler(CmbAnalysisGroup_Leave);
		((System.Windows.Forms.Control)(object)this.CmbAnalysisGroup).Leave += new System.EventHandler(CmbAnalysisGroup_Leave);
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label44.Location = new System.Drawing.Point(6, 17);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(86, 15);
		this.label44.TabIndex = 28;
		this.label44.Text = "Analysis Group";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(157, 17);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(90, 15);
		this.label4.TabIndex = 19;
		this.label4.Text = "Standard Name";
		this.txtScheduleName.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtScheduleName.Location = new System.Drawing.Point(154, 38);
		this.txtScheduleName.Name = "txtScheduleName";
		this.txtScheduleName.Size = new System.Drawing.Size(140, 23);
		this.txtScheduleName.TabIndex = 1;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label3.ForeColor = System.Drawing.Color.White;
		this.label3.Location = new System.Drawing.Point(0, 30);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(314, 17);
		this.label3.TabIndex = 22;
		this.label3.Text = "Lab Analysis Standards Item And Party Wise";
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.label3);
		this.panel3.Controls.Add(this.tsItemTaxSchedule);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(3, 3);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(870, 53);
		this.panel3.TabIndex = 0;
		this.tsItemTaxSchedule.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tsItemTaxSchedule.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.tsItemTaxSchedule.Items.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.btnnew, this.btnRefresh, this.btnupdate, this.btnsave, this.btnshortcutkeys });
		this.tsItemTaxSchedule.Location = new System.Drawing.Point(0, 0);
		this.tsItemTaxSchedule.Name = "tsItemTaxSchedule";
		this.tsItemTaxSchedule.Size = new System.Drawing.Size(870, 27);
		this.tsItemTaxSchedule.TabIndex = 78;
		this.tsItemTaxSchedule.Text = "toolStrip3";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnupdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnupdate.Image = (System.Drawing.Image)resources.GetObject("btnupdate.Image");
		this.btnupdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnupdate.Name = "btnupdate";
		this.btnupdate.Size = new System.Drawing.Size(78, 24);
		this.btnupdate.Text = "&Update";
		this.btnupdate.Visible = false;
		this.btnupdate.Click += new System.EventHandler(btnupdate_Click);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnshortcutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshortcutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnshortcutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnshortcutkeys.Name = "btnshortcutkeys";
		this.btnshortcutkeys.Size = new System.Drawing.Size(126, 24);
		this.btnshortcutkeys.Text = "ShortCut Keys";
		this.btnshortcutkeys.Click += new System.EventHandler(btnshortcutkeys_Click);
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabForm);
		this.tabControl1.Controls.Add(this.tabHistory);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(884, 532);
		this.tabControl1.TabIndex = 2;
		this.tabForm.Controls.Add((System.Windows.Forms.Control)(object)this.grdfrm);
		this.tabForm.Controls.Add(this.panel5);
		this.tabForm.Controls.Add(this.panel4);
		this.tabForm.Controls.Add(this.panel3);
		this.tabForm.Location = new System.Drawing.Point(4, 4);
		this.tabForm.Name = "tabForm";
		this.tabForm.Padding = new System.Windows.Forms.Padding(3);
		this.tabForm.Size = new System.Drawing.Size(876, 503);
		this.tabForm.TabIndex = 0;
		this.tabForm.Text = "Form";
		this.tabForm.UseVisualStyleBackColor = true;
		this.tabHistory.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistory);
		this.tabHistory.Controls.Add(this.panel8);
		this.tabHistory.Controls.Add((System.Windows.Forms.Control)(object)this.grdSubPartyHistory);
		this.tabHistory.Controls.Add(this.panel15);
		this.tabHistory.Controls.Add(this.panel13);
		this.tabHistory.Controls.Add(this.panel9);
		this.tabHistory.Controls.Add(this.toolStrip2);
		this.tabHistory.Location = new System.Drawing.Point(4, 4);
		this.tabHistory.Name = "tabHistory";
		this.tabHistory.Padding = new System.Windows.Forms.Padding(3);
		this.tabHistory.Size = new System.Drawing.Size(876, 503);
		this.tabHistory.TabIndex = 1;
		this.tabHistory.Text = "History";
		this.tabHistory.UseVisualStyleBackColor = true;
		this.grdHistory.AllowEdit = (InheritableBoolean)2;
		grdHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistory.DesignTimeLayout = grdHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistory.DynamicFiltering = true;
		this.grdHistory.FilterMode = (FilterMode)2;
		this.grdHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdHistory).Location = new System.Drawing.Point(3, 153);
		((System.Windows.Forms.Control)(object)this.grdHistory).Name = "grdHistory";
		((ScrollableBase)this.grdHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistory).Size = new System.Drawing.Size(870, 134);
		((System.Windows.Forms.Control)(object)this.grdHistory).TabIndex = 0;
		this.grdHistory.TotalRow = (InheritableBoolean)1;
		this.grdHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistory).VisualStyle = (VisualStyle)4;
		this.grdHistory.ColumnButtonClick += new ColumnActionEventHandler(grdHistory_ColumnButtonClick);
		this.grdHistory.SelectionChanged += new System.EventHandler(grdHistory_SelectionChanged);
		((System.Windows.Forms.Control)(object)this.grdHistory).DoubleClick += new System.EventHandler(grdHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(grdHistory_KeyDown);
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.label2);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel8.Location = new System.Drawing.Point(3, 287);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(870, 23);
		this.panel8.TabIndex = 64;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(5, 3);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(153, 16);
		this.label2.TabIndex = 127;
		this.label2.Text = "Detail of Selected Row";
		this.grdSubPartyHistory.AllowDelete = (InheritableBoolean)1;
		((ScrollableBase)this.grdSubPartyHistory).BackColor = System.Drawing.Color.White;
		grdSubPartyHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdSubPartyHistory.DesignTimeLayout = grdSubPartyHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdSubPartyHistory).Dock = System.Windows.Forms.DockStyle.Bottom;
		((System.Windows.Forms.Control)(object)this.grdSubPartyHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdSubPartyHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdSubPartyHistory).Location = new System.Drawing.Point(3, 310);
		((System.Windows.Forms.Control)(object)this.grdSubPartyHistory).Name = "grdSubPartyHistory";
		((ScrollableBase)this.grdSubPartyHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdSubPartyHistory).Size = new System.Drawing.Size(870, 190);
		((System.Windows.Forms.Control)(object)this.grdSubPartyHistory).TabIndex = 1;
		this.grdSubPartyHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdSubPartyHistory).VisualStyle = (VisualStyle)4;
		this.panel15.BackColor = System.Drawing.Color.Teal;
		this.panel15.Controls.Add(this.ctrlGrdBar1);
		this.panel15.Controls.Add(this.label43);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.Location = new System.Drawing.Point(3, 130);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(870, 23);
		this.panel15.TabIndex = 61;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(826, 0);
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
		this.panel13.Controls.Add(this.groupBox2);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel13.Location = new System.Drawing.Point(3, 59);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(870, 71);
		this.panel13.TabIndex = 60;
		this.groupBox2.Controls.Add(this.btnshow);
		this.groupBox2.Controls.Add(this.rdentrydate);
		this.groupBox2.Controls.Add(this.rdmodifydate);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemNameHistory);
		this.groupBox2.Controls.Add(this.label37);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbAnalysisGroupNameHistory);
		this.groupBox2.Controls.Add(this.label7);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbStandardNameHistory);
		this.groupBox2.Controls.Add(this.label5);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBuyerNameHistory);
		this.groupBox2.Controls.Add(this.label29);
		this.groupBox2.Controls.Add(this.ToDateHistory);
		this.groupBox2.Controls.Add(this.label6);
		this.groupBox2.Controls.Add(this.label33);
		this.groupBox2.Controls.Add(this.FromDateHistory);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(4, 1);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(866, 69);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Filters";
		this.btnshow.BackColor = System.Drawing.Color.Teal;
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(777, 43);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(52, 21);
		this.btnshow.TabIndex = 8;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Checked = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(777, 9);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 6;
		this.rdentrydate.TabStop = true;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(777, 25);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 7;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.CmbItemNameHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbItemNameHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemNameHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemNameHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance57).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance57).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance57).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance57;
		((AppearanceBase)appearance66).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance66;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance67).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance67).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance68;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance71;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance72).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance73).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance73).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance73).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance73;
		((AppearanceBase)appearance74).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance75).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemNameHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemNameHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemNameHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemNameHistory).Location = new System.Drawing.Point(280, 12);
		((System.Windows.Forms.Control)(object)this.CmbItemNameHistory).Name = "CmbItemNameHistory";
		((System.Windows.Forms.Control)(object)this.CmbItemNameHistory).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemNameHistory).TabIndex = 2;
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(211, 18);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(67, 15);
		this.label37.TabIndex = 189;
		this.label37.Text = "Item Name";
		this.CmbAnalysisGroupNameHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbAnalysisGroupNameHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbAnalysisGroupNameHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbAnalysisGroupNameHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbAnalysisGroupNameHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.CmbAnalysisGroupNameHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbAnalysisGroupNameHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance60).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance60).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbAnalysisGroupNameHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance60;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAnalysisGroupNameHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance83;
		((SpecialBoxBase)((UltraGridBase)this.CmbAnalysisGroupNameHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance84).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance84).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAnalysisGroupNameHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.CmbAnalysisGroupNameHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbAnalysisGroupNameHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbAnalysisGroupNameHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance3;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance61).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbAnalysisGroupNameHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.CmbAnalysisGroupNameHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbAnalysisGroupNameHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAnalysisGroupNameHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance62;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance63).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbAnalysisGroupNameHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbAnalysisGroupNameHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbAnalysisGroupNameHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance64).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance64).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance64).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance64).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAnalysisGroupNameHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance64;
		((AppearanceBase)appearance65).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbAnalysisGroupNameHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.CmbAnalysisGroupNameHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbAnalysisGroupNameHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbAnalysisGroupNameHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.CmbAnalysisGroupNameHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbAnalysisGroupNameHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbAnalysisGroupNameHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbAnalysisGroupNameHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbAnalysisGroupNameHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbAnalysisGroupNameHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbAnalysisGroupNameHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbAnalysisGroupNameHistory).Location = new System.Drawing.Point(592, 12);
		((System.Windows.Forms.Control)(object)this.CmbAnalysisGroupNameHistory).Name = "CmbAnalysisGroupNameHistory";
		((System.Windows.Forms.Control)(object)this.CmbAnalysisGroupNameHistory).Size = new System.Drawing.Size(180, 26);
		((System.Windows.Forms.Control)(object)this.CmbAnalysisGroupNameHistory).TabIndex = 4;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(504, 18);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(86, 15);
		this.label7.TabIndex = 182;
		this.label7.Text = "Analysis Group";
		this.CmbStandardNameHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbStandardNameHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbStandardNameHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbStandardNameHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbStandardNameHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbStandardNameHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbStandardNameHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance47).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance47).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbStandardNameHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance47;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbStandardNameHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance58;
		((SpecialBoxBase)((UltraGridBase)this.CmbStandardNameHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbStandardNameHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.CmbStandardNameHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbStandardNameHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance80).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbStandardNameHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbStandardNameHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.CmbStandardNameHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbStandardNameHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance2).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbStandardNameHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance2;
		((AppearanceBase)appearance4).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance4).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbStandardNameHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbStandardNameHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbStandardNameHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance5).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance5).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance5).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbStandardNameHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance5;
		((AppearanceBase)appearance40).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbStandardNameHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.CmbStandardNameHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbStandardNameHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbStandardNameHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbStandardNameHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbStandardNameHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.CmbStandardNameHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbStandardNameHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbStandardNameHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbStandardNameHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbStandardNameHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbStandardNameHistory).Location = new System.Drawing.Point(592, 40);
		((System.Windows.Forms.Control)(object)this.CmbStandardNameHistory).Name = "CmbStandardNameHistory";
		((System.Windows.Forms.Control)(object)this.CmbStandardNameHistory).Size = new System.Drawing.Size(180, 26);
		((System.Windows.Forms.Control)(object)this.CmbStandardNameHistory).TabIndex = 5;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(504, 46);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(90, 15);
		this.label5.TabIndex = 182;
		this.label5.Text = "Standard Name";
		this.CmbBuyerNameHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbBuyerNameHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBuyerNameHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBuyerNameHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance35;
		((SpecialBoxBase)((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance36).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance36).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance37;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance38).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance42).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance43).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance43).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance43).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance43).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance77).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBuyerNameHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBuyerNameHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBuyerNameHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBuyerNameHistory).Location = new System.Drawing.Point(280, 40);
		((System.Windows.Forms.Control)(object)this.CmbBuyerNameHistory).Name = "CmbBuyerNameHistory";
		((System.Windows.Forms.Control)(object)this.CmbBuyerNameHistory).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.CmbBuyerNameHistory).TabIndex = 3;
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.Location = new System.Drawing.Point(211, 46);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(69, 15);
		this.label29.TabIndex = 182;
		this.label29.Text = "Party Name";
		this.ToDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9f);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(67, 42);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(140, 22);
		this.ToDateHistory.TabIndex = 1;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(3, 46);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(48, 15);
		this.label6.TabIndex = 171;
		this.label6.Text = "To Date";
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(3, 18);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(63, 15);
		this.label33.TabIndex = 171;
		this.label33.Text = "From Date";
		this.FromDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9f);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(67, 14);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(140, 22);
		this.FromDateHistory.TabIndex = 0;
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.label30);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(3, 30);
		this.panel9.Margin = new System.Windows.Forms.Padding(2);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(870, 29);
		this.panel9.TabIndex = 59;
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.ForeColor = System.Drawing.Color.White;
		this.label30.Location = new System.Drawing.Point(4, 4);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(434, 19);
		this.label30.TabIndex = 0;
		this.label30.Text = "Lab Analysis Standards Item And Party Wise History";
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnNewHistory, this.toolStripSeparator3, this.BtnRefreshHistory });
		this.toolStrip2.Location = new System.Drawing.Point(3, 3);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(870, 27);
		this.toolStrip2.TabIndex = 58;
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
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(884, 532);
		base.Controls.Add(this.tabControl1);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Name = "frmLabAnalysisScheduleItemAndPartyWise";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmLabAnalysisScheduleItemAndPartyWise_FormClosing);
		base.Load += new System.EventHandler(InvLabAnalysisGroup_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmLabAnalysisScheduleItemAndPartyWise_KeyDown);
		((System.ComponentModel.ISupportInitialize)this.grdfrm).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBrandItemName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBuyerName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAnalysisGroup).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.tsItemTaxSchedule.ResumeLayout(false);
		this.tsItemTaxSchedule.PerformLayout();
		this.tabControl1.ResumeLayout(false);
		this.tabForm.ResumeLayout(false);
		this.tabHistory.ResumeLayout(false);
		this.tabHistory.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdSubPartyHistory).EndInit();
		this.panel15.ResumeLayout(false);
		this.panel15.PerformLayout();
		this.panel13.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbItemNameHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAnalysisGroupNameHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbStandardNameHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBuyerNameHistory).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}
