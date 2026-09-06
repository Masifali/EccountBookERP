using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.BLL.lgstcm;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Architecture.WinApp.Service.Definition;
using Architecture.WinApp.lgstcm.LookUp;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Service;

public class EximServicesDefine : Form
{
	private Attachment AT = new Attachment();

	private Rightsobjects formRights = new Rightsobjects();

	public UserAccount UserAccount = new UserAccount();

	private DataTable dtCatergory = new DataTable();

	private DataTable dtHistoryGrid = new DataTable();

	public bool UpdateMode;

	private bool FormInitialized;

	private int RecId;

	private string ScreenName = "EximServicesDefine";

	private IContainer components;

	private Label label13;

	private Panel panel2;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnupdate;

	private ToolStripButton btnsave;

	internal GridEX grdhistory;

	private Panel panel9;

	private Button btnsearch;

	private Label label32;

	private Panel panel10;

	private Label label31;

	private CtrlGrdBar ctrlGrdBar1;

	private ToolStripButton toolStripButton1;

	private Panel panel7;

	private CheckBox chkstatus;

	internal UltraCombo cmbPurchaseGL;

	private TextBox txtItemCode;

	private TextBox txtItemName;

	private Label label10;

	private Label label4;

	private Label label3;

	private Label label2;

	private Label label1;

	internal UltraCombo CmbUomGroup;

	internal UltraCombo cmbItemCategory;

	internal UltraCombo cmbitemcathistory;

	private GroupBox groupBox1;

	internal GridEX grdAllocation;

	private Label label11;

	private Label label9;

	private Label label8;

	private Label label7;

	private Label label33;

	private ToolStripButton btnShortCutKey;

	private ToolStripButton btnServiceCategory;

	internal UltraCombo CmbMasterItem;

	private Label label5;

	private ToolStripButton BtnDefineMasterItem;

	private ToolStripButton btnattachment;

	public EximServicesDefine(UserAccount pUserAccount)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUserAccount;
		InitializeComponentCustom();
		InitializeComponentMethod();
	}

	private void InitializeComponentCustom()
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(900, 500);
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
			ScreenName = base.Name;
			formRights = CommonServices.SetRightsValueInRightsObject(ScreenName);
			CatagorydtFillDBCall();
			DataTable itemGroupData = ItemGroupNameDBCall();
			DataTable companiesData = CommonServices.CompanyServiceBind();
			DataTable byLookUpTypeId = LookUps.GetByLookUpTypeId(UserAccount.OrganizationId, UserAccount.CompanyId, 3);
			return new
			{
				ItemGroupData = itemGroupData,
				CompaniesData = companiesData,
				MIData = byLookUpTypeId
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
				btnsave.Enabled = formRights.DoHaveSaveRight;
				btnupdate.Enabled = formRights.DoHaveUpdateRights;
				ctrlGrdBar1.mGridChooseFielder.Enabled = formRights.DoHaveFieldChooserRights;
				ctrlGrdBar1.mGridSaveLayouts.Enabled = formRights.DoHaveSaveLayoutRights;
				ctrlGrdBar1.mGridPrint.Enabled = formRights.DoHaveGridPrintRights;
				ctrlGrdBar1.mGridExport.Enabled = formRights.DoHaveGridExportRights;
				ctrlGrdBar1.GroupCollaps.Enabled = formRights.DoHaveGridGroupCollapseRights;
				ctrlGrdBar1.GroupExpand.Enabled = formRights.DoHaveGridGroupExpandRights;
				CatagoryBind();
				AccountlstDtFillFromGlobal();
				ItemGroupNameBind(result.ItemGroupData);
				CompaniesBindInGrid(result.CompaniesData);
				MasterItemBind(result.MIData);
				FormInitialized = true;
				btnsave.Visible = true;
				btnupdate.Visible = false;
				cmbItemCategory.Focus();
			}
		}, TaskScheduler.FromCurrentSynchronizationContext());
	}

	private void CatagorydtFillDBCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ItemCategory cat = new ItemCategory();
			cat.OrganizationId = UserAccount.OrganizationId;
			cat.CompanyId = UserAccount.CompanyId;
			cat.InventoryParentCategoriesId = 9;
			dtCatergory = ItemCategory.Getall(cat);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CatagoryBind()
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(cmbItemCategory, dtCatergory, "Id", "CategoryDescription", "Services Category", AllColumns: false, Conversion.ToInt(cmbItemCategory.Value), insertDefaultRow: true);
			InfragisticsHelper.BindAndRetainSelection(cmbitemcathistory, dtCatergory, "Id", "CategoryDescription", "Services Category", AllColumns: false, Conversion.ToInt(cmbitemcathistory.Value), insertDefaultRow: true);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void MasterItemBind(DataTable dt)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbMasterItem, dt, "Id", "LookupName", "Master Item", AllColumns: false, Conversion.ToInt(CmbMasterItem.Value), insertDefaultRow: true);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private DataTable ItemGroupNameDBCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			return ItemGroup.GetAll(new ItemGroup
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

	private void ItemGroupNameBind(DataTable lst)
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("GroupName");
			dt.Rows.Clear();
			if (lst.Rows.Count > 0)
			{
				for (int i = 0; i < lst.Rows.Count; i++)
				{
					dt.Rows.Add(lst.Rows[i]["GroupId"], lst.Rows[i]["ItemGroupName"]);
				}
			}
			InfragisticsHelper.BindAndRetainSelection(CmbUomGroup, dt, "Id", "GroupName", "Uom Group", AllColumns: true, Conversion.ToInt(CmbUomGroup.Value), insertDefaultRow: true);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AccountlstDtFillFromGlobal()
	{
		try
		{
			DataTable dtAccount = DatatableHelper.GetAccountsFromGlobalByTypeIds(new int[2] { 20, 21 });
			InfragisticsHelper.BindAndRetainSelection(cmbPurchaseGL, dtAccount, "Id", "AccountTitle", "Account Title", AllColumns: true, Conversion.ToInt(cmbPurchaseGL.Value), insertDefaultRow: true, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void cmbItemCategory_Leave(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		//IL_007d: Expected O, but got Unknown
		try
		{
			Item val = new Item
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				ItemCategoryId = Convert.ToInt32(cmbItemCategory.Value)
			};
			DataTable lst = Item.GenerateCode(val);
			if (lst.Rows.Count > 0)
			{
				txtItemCode.Text = lst.Rows[0]["ItemCode"].ToString();
			}
			DataTable dt = Item.GetGLAccountbyItemCategoryId(val);
			if (dt.Rows.Count > 0)
			{
				cmbPurchaseGL.Value = dt.Rows[0]["InventoryAccountId"].ToString();
			}
		}
		catch (Exception)
		{
			MessageBox.Show("Record Not Found");
		}
	}

	private void CompaniesBindInGrid(DataTable dt)
	{
		try
		{
			if (dt.Rows.Count > 0)
			{
				DataTable table = new DataTable();
				table.Columns.Add("Id");
				table.Columns.Add("Location");
				table.Columns.Add("Value");
				table.Rows.Clear();
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					table.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["CompName"], true);
				}
				if (table.Rows.Count > 0)
				{
					grdAllocation.DataSource = table;
					grdAllocation.RetrieveStructure();
					grdAllocation.RootTable.Columns["Id"].Visible = false;
					grdAllocation.RootTable.Columns["Location"].Width = 250;
					grdAllocation.RootTable.Columns["Value"].EditType = (EditType)8;
					grdAllocation.RootTable.Columns["Value"].FilterEditType = (FilterEditType)7;
					grdAllocation.RootTable.Columns["Value"].ColumnType = (ColumnType)3;
					grdAllocation.RootTable.Columns["Value"].CheckBoxTrueValue = "True";
					grdAllocation.RootTable.Columns["Value"].CheckBoxFalseValue = "False";
					grdAllocation.RootTable.Columns["Value"].UseHeaderSelector = true;
					grdAllocation.ColumnAutoResize = true;
				}
				else
				{
					grdAllocation.ClearStructure();
				}
			}
			else
			{
				grdAllocation.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnSave_Click(object sender, EventArgs e)
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

	private void Insert()
	{
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_0212: Expected O, but got Unknown
		//IL_0297: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02eb: Expected O, but got Unknown
		try
		{
			if (!grdAllocation.GetDataRows().Any())
			{
				throw new Exception("Company Grid Record Not Found");
			}
			List<(Control, string, FormHelper.ValidType)> controlsToValidate = new List<(Control, string, FormHelper.ValidType)>
			{
				((Control)(object)cmbItemCategory, "Services Category", FormHelper.ValidType.INT),
				(txtItemName, "Services Name", FormHelper.ValidType.String),
				((Control)(object)cmbPurchaseGL, "Services GL A/C", FormHelper.ValidType.INT)
			};
			if (RecId == 0)
			{
				controlsToValidate.Add(((Control)(object)CmbUomGroup, "UOM Group", FormHelper.ValidType.INT));
			}
			if (!FormHelper.ValidateControls(controlsToValidate) || !FormHelper.ConfirmAction((RecId == 0) ? "Are you sure to Save" : "Are you sure to Update"))
			{
				return;
			}
			Item items = new Item
			{
				Id = RecId,
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				EntryDate = DateTime.Now,
				ModifyDate = DateTime.Now,
				EntryUser = UserAccount.ID,
				ModifyUser = UserAccount.ID,
				ItemCategoryId = Conversion.ToInt(cmbItemCategory.Value),
				ItemClassId = 6,
				ItemCode = Conversion.ToString((object)txtItemCode.Text.Trim()),
				ItemName = Conversion.ToString((object)txtItemName.Text.Trim()),
				HSCode = "0",
				PackQty = 0.0,
				ItemStatus = chkstatus.Checked,
				PurchaseGLAC = Conversion.ToInt(cmbPurchaseGL.Value),
				SaleGLAC = Conversion.ToInt(cmbPurchaseGL.Value),
				COGSGLAC = Conversion.ToInt(cmbPurchaseGL.Value),
				ServicesMasterItemId = Conversion.ToInt(CmbMasterItem.Value),
				ItemAllocationlist = new List<ItemAllocation>()
			};
			if (RecId == 0)
			{
				items.ItemGroupId = Conversion.ToInt(CmbUomGroup.Value);
			}
			items.ItemAllocationlist = new List<ItemAllocation>();
			if (RecId == 0)
			{
				GridEXRow[] dataRows = grdAllocation.GetDataRows();
				int inactiveCount = 0;
				GridEXRow[] array = dataRows;
				foreach (GridEXRow row in array)
				{
					bool isActive = Conversion.ToBool(row.Cells["Value"].Value);
					if (dataRows.Count() == 1 || isActive)
					{
						items.ItemAllocationlist.Add(new ItemAllocation
						{
							CompanyId = Conversion.ToInt(row.Cells["Id"].Value),
							IsActive = true,
							BranchId = UserAccount.BranchesId,
							OrganizationId = UserAccount.OrganizationId
						});
					}
					else
					{
						inactiveCount++;
					}
				}
				if (dataRows.Count() > 1 && inactiveCount == dataRows.Count())
				{
					throw new Exception("Please select Location first");
				}
			}
			items.AttachmentsList = new List<DMSAttachments>();
			items.DeleteAttachmentsList = new List<DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<Item>(items, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
			Item.Save(items, true);
			if (RecId == 0)
			{
				MessageBox.Show("Save Successfully");
			}
			else
			{
				MessageBox.Show("Update Successfully");
				FormHelper.DeleteAllAttachmentsFromDb(AT, RecId, 0, base.Name);
			}
			FormReset();
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
				throw new Exception("Record not update because RecId not found");
			}
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ReadById(int ID)
	{
		try
		{
			FormReset();
			RecId = ID;
			Item items = Item.GetByID(RecId);
			if (items == null)
			{
				MessageBox.Show("Record Not Found");
				return;
			}
			cmbItemCategory.Value = items.ItemCategoryId;
			txtItemCode.Text = Conversion.ToString((object)items.ItemCode);
			txtItemName.Text = Conversion.ToString((object)items.ItemName);
			cmbPurchaseGL.Value = items.PurchaseGLAC;
			chkstatus.Checked = Conversion.ToBool((object)items.ItemStatus);
			CmbMasterItem.Value = Conversion.ToInt((object)items.ServicesMasterItemId);
			FormHelper.LoadAttachmentsForObject(AT, RecId, base.Name);
			btnsave.Visible = false;
			btnupdate.Visible = true;
			cmbItemCategory.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		FormReset();
		((Control)(object)CmbUomGroup).Text = string.Empty;
	}

	private void toolStripButton1_Click(object sender, EventArgs e)
	{
		try
		{
			List<string> allServices = new List<string> { "AccountsWithCustomGroupId" };
			DatatableHelper.GlobalServicesDbCall(UserAccount, allServices);
			CatagorydtFillDBCall();
			CatagoryBind();
			AccountlstDtFillFromGlobal();
			ItemGroupNameBind(ItemGroupNameDBCall());
			MasterItemBind(LookUps.GetByLookUpTypeId(UserAccount.OrganizationId, UserAccount.CompanyId, 3));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FormReset()
	{
		try
		{
			RecId = 0;
			AT = new Attachment();
			Attachment.dtAttachments.Clear();
			btnsave.Visible = true;
			btnupdate.Visible = false;
			cmbItemCategory.Focus();
			txtItemCode.Clear();
			txtItemName.Clear();
			((Control)(object)cmbPurchaseGL).Text = string.Empty;
			CompaniesBindInGrid(CommonServices.CompanyServiceBind());
			cmbItemCategory_Leave(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnsearch_Click(object sender, EventArgs e)
	{
		try
		{
			HistoryGridFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryGridFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters reports = new ReportsParameters();
			reports.OrganizationId = UserAccount.OrganizationId;
			reports.CompanyId = UserAccount.CompanyId;
			reports.ScreenName = base.Name;
			reports.CanViewAllRecord = formRights.DoHaveCanViewAllRecordRights;
			if (!reports.CanViewAllRecord)
			{
				reports.EntryUser = UserAccount.ID;
			}
			reports.ParentCategoryIds = "9";
			reports.Id = Conversion.ToInt(cmbitemcathistory.Value);
			DataTable grdlst = Item.FormHistory(reports);
			if (grdlst.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("Category", typeof(string));
				dt.Columns.Add("ServiceCode", typeof(string));
				dt.Columns.Add("ServiceName", typeof(string));
				dt.Columns.Add("GlAccount", typeof(string));
				dt.Columns.Add("MasterItem", typeof(string));
				dt.Columns.Add("Active", typeof(bool));
				dt.Columns.Add("EntryUser", typeof(string));
				dt.Columns.Add("EntryDate", typeof(DateTime));
				dt.Columns.Add("ModifyUser", typeof(string));
				dt.Columns.Add("ModifyDate", typeof(DateTime));
				dt.Columns.Add("NoOfAttachments", typeof(int));
				for (int i = 0; i < grdlst.Rows.Count; i++)
				{
					dt.Rows.Add(grdlst.Rows[i]["Id"], grdlst.Rows[i]["CategoryDescription"], grdlst.Rows[i]["ItemCode"], grdlst.Rows[i]["ItemName"], grdlst.Rows[i]["GlStockAccountTitle"], grdlst.Rows[i]["ServicesMasterItem"], grdlst.Rows[i]["ItemStatus"], grdlst.Rows[i]["EntryUserName"], Conversion.ToDateTime(grdlst.Rows[i]["EntryDate"]).ToShortDateString(), grdlst.Rows[i]["ModifyUserName"], grdlst.Rows[i]["ModifyDate"], grdlst.Rows[i]["NoOfAttachments"]);
				}
				grdhistory.DataSource = dt;
				grdhistory.RetrieveStructure();
				gridsetting();
			}
			else
			{
				grdhistory.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridsetting()
	{
		try
		{
			grdhistory.RootTable.Columns["Id"].Visible = false;
			grdhistory.RootTable.Columns["MasterItem"].Visible = false;
			GridEX_Helper.GridWrappingAndColumnSettings(grdhistory, 2, 3);
			grdhistory.RootTable.Columns["GlAccount"].Caption = "Services GL A/C";
			grdhistory.RootTable.Groups.Add("MasterItem");
			grdhistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdhistory.RootTable.Columns["EntryDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			grdhistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow row = grdhistory.CurrentRow;
			if (row != null && (int)row.RowType == 0)
			{
				if (!formRights.DoHaveUpdateRights)
				{
					throw new Exception("You don't have right to update");
				}
				ReadById(Conversion.ToInt(row.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdhistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count != 0 && e.Column.Key == "NoOfAttachments")
			{
				CommonServices.GetNoofAttachmentsByScreenName(Conversion.ToInt(r.Cells["Id"].Value), base.Name);
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
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(ScreenName, grdhistory, dtHistoryGrid);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void btnServiceCategory_Click(object sender, EventArgs e)
	{
		try
		{
			new frmServicesItemCategory(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnDefineMasterItem_Click(object sender, EventArgs e)
	{
		try
		{
			InvLookup invLookup = new InvLookup(UserAccount);
			invLookup.Show();
			invLookup.cmbProfileName.Value = 3;
			((Control)(object)invLookup.cmbProfileName).Enabled = false;
			invLookup.GridBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InvDefrmAddItem_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.KeyCode == Keys.Return)
		{
			SendKeys.Send("{TAB}");
		}
		else
		{
			if (!e.Control)
			{
				return;
			}
			switch (e.KeyCode)
			{
			case Keys.N:
				btnnew_Click(null, null);
				break;
			case Keys.R:
				toolStripButton1_Click(null, null);
				break;
			case Keys.S:
				if (btnsave.Visible && btnsave.Enabled)
				{
					BtnSave_Click(null, null);
				}
				break;
			case Keys.U:
				if (btnupdate.Visible && btnupdate.Enabled)
				{
					btnupdate_Click(null, null);
				}
				break;
			case Keys.Up:
				cmbitemcathistory.Focus();
				break;
			case Keys.Down:
				((Control)(object)grdhistory).Focus();
				break;
			case Keys.E:
				Close();
				break;
			}
			if (e.Alt)
			{
				MakeShortCutKeys();
			}
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

	private void MakeShortCutKeys()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("KeyCombination");
			dt.Columns.Add("Description");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+S", "For Save");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+F5", "For Focus on Doc Date");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On History Category Combo");
			dt.Rows.Add("Ctrl+ArrowDown", "When In History Tab For Focus On Grid history");
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

	private void grdhistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyCode == Keys.Return && !e.Control)
			{
				SendKeys.Send("{TAB}");
				return;
			}
			GridEXRow currentRow = grdhistory.CurrentRow;
			if (currentRow == null || (int)currentRow.RowType != 0)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				int CurrId = Conversion.ToInt(currentRow.Cells["Id"].Value);
				if (grdhistory.CurrentColumn.Key == "Edit")
				{
					if (!formRights.DoHaveUpdateRights)
					{
						throw new Exception("you don't have update rights...");
					}
					ReadById(CurrId);
				}
				if (grdhistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByScreenName(CurrId, base.Name);
				}
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				grdhistory_DoubleClick(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnattachment_Click(object sender, EventArgs e)
	{
		AT.Show();
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
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d2: Expected O, but got Unknown
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Expected O, but got Unknown
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Expected O, but got Unknown
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Expected O, but got Unknown
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Expected O, but got Unknown
		//IL_0307: Unknown result type (might be due to invalid IL or missing references)
		//IL_0311: Expected O, but got Unknown
		//IL_031d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0327: Expected O, but got Unknown
		//IL_25b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_25bf: Expected O, but got Unknown
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Service.EximServicesDefine));
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
		this.label13 = new System.Windows.Forms.Label();
		this.panel7 = new System.Windows.Forms.Panel();
		this.CmbMasterItem = new UltraCombo();
		this.CmbUomGroup = new UltraCombo();
		this.cmbItemCategory = new UltraCombo();
		this.cmbPurchaseGL = new UltraCombo();
		this.txtItemCode = new System.Windows.Forms.TextBox();
		this.txtItemName = new System.Windows.Forms.TextBox();
		this.label11 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.grdAllocation = new GridEX();
		this.chkstatus = new System.Windows.Forms.CheckBox();
		this.label10 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.BtnDefineMasterItem = new System.Windows.Forms.ToolStripButton();
		this.btnServiceCategory = new System.Windows.Forms.ToolStripButton();
		this.btnShortCutKey = new System.Windows.Forms.ToolStripButton();
		this.grdhistory = new GridEX();
		this.panel9 = new System.Windows.Forms.Panel();
		this.cmbitemcathistory = new UltraCombo();
		this.btnsearch = new System.Windows.Forms.Button();
		this.label32 = new System.Windows.Forms.Label();
		this.panel10 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label31 = new System.Windows.Forms.Label();
		this.btnattachment = new System.Windows.Forms.ToolStripButton();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbMasterItem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbUomGroup).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemCategory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPurchaseGL).BeginInit();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdAllocation).BeginInit();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdhistory).BeginInit();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbitemcathistory).BeginInit();
		this.panel10.SuspendLayout();
		base.SuspendLayout();
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Calibri", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.ForeColor = System.Drawing.Color.White;
		this.label13.Location = new System.Drawing.Point(4, 2);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(186, 23);
		this.label13.TabIndex = 0;
		this.label13.Text = "Define Services Item's";
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.CmbMasterItem);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.CmbUomGroup);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemCategory);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPurchaseGL);
		this.panel7.Controls.Add(this.txtItemCode);
		this.panel7.Controls.Add(this.txtItemName);
		this.panel7.Controls.Add(this.label11);
		this.panel7.Controls.Add(this.label9);
		this.panel7.Controls.Add(this.label8);
		this.panel7.Controls.Add(this.label7);
		this.panel7.Controls.Add(this.label33);
		this.panel7.Controls.Add(this.groupBox1);
		this.panel7.Controls.Add(this.chkstatus);
		this.panel7.Controls.Add(this.label10);
		this.panel7.Controls.Add(this.label5);
		this.panel7.Controls.Add(this.label4);
		this.panel7.Controls.Add(this.label3);
		this.panel7.Controls.Add(this.label2);
		this.panel7.Controls.Add(this.label1);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.panel7.Location = new System.Drawing.Point(0, 55);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(884, 168);
		this.panel7.TabIndex = 0;
		this.CmbMasterItem.AllowNull = (DefaultableBoolean)1;
		this.CmbMasterItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbMasterItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbMasterItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance36).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance36).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbMasterItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance37;
		((SpecialBoxBase)((UltraGridBase)this.CmbMasterItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance38).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance38).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance38).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance39;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance40).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance43).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance43).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance44).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance44).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance44).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance45).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbMasterItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbMasterItem).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbMasterItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbMasterItem).Location = new System.Drawing.Point(106, 138);
		((System.Windows.Forms.Control)(object)this.CmbMasterItem).Name = "CmbMasterItem";
		((System.Windows.Forms.Control)(object)this.CmbMasterItem).Size = new System.Drawing.Size(210, 26);
		((System.Windows.Forms.Control)(object)this.CmbMasterItem).TabIndex = 6;
		this.CmbUomGroup.AllowNull = (DefaultableBoolean)1;
		this.CmbUomGroup.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbUomGroup.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbUomGroup.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbUomGroup).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbUomGroup).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbUomGroup).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbUomGroup).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbUomGroup).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance30;
		((SpecialBoxBase)((UltraGridBase)this.CmbUomGroup).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance31).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance31).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbUomGroup).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbUomGroup).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbUomGroup).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbUomGroup).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbUomGroup).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbUomGroup).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbUomGroup).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbUomGroup).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance42).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbUomGroup).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbUomGroup).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbUomGroup).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbUomGroup).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbUomGroup).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbUomGroup).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbUomGroup).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbUomGroup).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbUomGroup).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbUomGroup).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbUomGroup).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbUomGroup).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbUomGroup).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbUomGroup).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbUomGroup.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbUomGroup).Location = new System.Drawing.Point(106, 109);
		((System.Windows.Forms.Control)(object)this.CmbUomGroup).Name = "CmbUomGroup";
		((System.Windows.Forms.Control)(object)this.CmbUomGroup).Size = new System.Drawing.Size(149, 26);
		((System.Windows.Forms.Control)(object)this.CmbUomGroup).TabIndex = 4;
		this.cmbItemCategory.AllowNull = (DefaultableBoolean)1;
		this.cmbItemCategory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemCategory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemCategory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance5).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance5).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemCategory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance5;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance6;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemCategory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance7).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance7).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance10;
		((AppearanceBase)appearance11).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance11).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance13).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemCategory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItemCategory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemCategory).Location = new System.Drawing.Point(106, 3);
		((System.Windows.Forms.Control)(object)this.cmbItemCategory).Name = "cmbItemCategory";
		((System.Windows.Forms.Control)(object)this.cmbItemCategory).Size = new System.Drawing.Size(210, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemCategory).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbItemCategory).Leave += new System.EventHandler(cmbItemCategory_Leave);
		this.cmbPurchaseGL.AllowNull = (DefaultableBoolean)1;
		this.cmbPurchaseGL.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPurchaseGL.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPurchaseGL.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Appearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance18).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance18).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance18;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance19;
		((SpecialBoxBase)((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance20).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance20).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance25).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance26).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance26).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance26).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPurchaseGL).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPurchaseGL).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbPurchaseGL.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPurchaseGL).Location = new System.Drawing.Point(106, 81);
		((System.Windows.Forms.Control)(object)this.cmbPurchaseGL).Name = "cmbPurchaseGL";
		((System.Windows.Forms.Control)(object)this.cmbPurchaseGL).Size = new System.Drawing.Size(210, 26);
		((System.Windows.Forms.Control)(object)this.cmbPurchaseGL).TabIndex = 3;
		this.txtItemCode.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemCode.Location = new System.Drawing.Point(106, 31);
		this.txtItemCode.Name = "txtItemCode";
		this.txtItemCode.ReadOnly = true;
		this.txtItemCode.Size = new System.Drawing.Size(210, 23);
		this.txtItemCode.TabIndex = 1;
		this.txtItemCode.TabStop = false;
		this.txtItemName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemName.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemName.Location = new System.Drawing.Point(106, 56);
		this.txtItemName.Name = "txtItemName";
		this.txtItemName.Size = new System.Drawing.Size(210, 23);
		this.txtItemName.TabIndex = 2;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.ForeColor = System.Drawing.Color.Red;
		this.label11.Location = new System.Drawing.Point(93, 34);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(14, 17);
		this.label11.TabIndex = 574;
		this.label11.Text = "*";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.ForeColor = System.Drawing.Color.Red;
		this.label9.Location = new System.Drawing.Point(93, 59);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(14, 17);
		this.label9.TabIndex = 2;
		this.label9.Text = "*";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.ForeColor = System.Drawing.Color.Red;
		this.label8.Location = new System.Drawing.Point(93, 86);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(14, 17);
		this.label8.TabIndex = 4;
		this.label8.Text = "*";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.ForeColor = System.Drawing.Color.Red;
		this.label7.Location = new System.Drawing.Point(93, 114);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(14, 17);
		this.label7.TabIndex = 3;
		this.label7.Text = "*";
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.ForeColor = System.Drawing.Color.Red;
		this.label33.Location = new System.Drawing.Point(93, 8);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(14, 17);
		this.label33.TabIndex = 570;
		this.label33.Text = "*";
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.grdAllocation);
		this.groupBox1.Location = new System.Drawing.Point(322, 3);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(500, 161);
		this.groupBox1.TabIndex = 6;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Service Allocation ";
		this.grdAllocation.AllowEdit = (InheritableBoolean)2;
		((System.Windows.Forms.Control)(object)this.grdAllocation).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdAllocation).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdAllocation.GroupByBoxVisible = false;
		this.grdAllocation.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdAllocation).Location = new System.Drawing.Point(3, 19);
		((System.Windows.Forms.Control)(object)this.grdAllocation).Name = "grdAllocation";
		((ScrollableBase)this.grdAllocation).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdAllocation).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdAllocation).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdAllocation).Size = new System.Drawing.Size(494, 139);
		((System.Windows.Forms.Control)(object)this.grdAllocation).TabIndex = 0;
		this.grdAllocation.TotalRow = (InheritableBoolean)1;
		this.grdAllocation.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdAllocation).VisualStyle = (VisualStyle)4;
		this.chkstatus.AutoSize = true;
		this.chkstatus.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.chkstatus.Checked = true;
		this.chkstatus.CheckState = System.Windows.Forms.CheckState.Checked;
		this.chkstatus.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkstatus.Location = new System.Drawing.Point(257, 113);
		this.chkstatus.Name = "chkstatus";
		this.chkstatus.Size = new System.Drawing.Size(59, 19);
		this.chkstatus.TabIndex = 5;
		this.chkstatus.Text = "Active";
		this.chkstatus.UseVisualStyleBackColor = true;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(8, 87);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(90, 15);
		this.label10.TabIndex = 9;
		this.label10.Text = "Services GL A/C";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(8, 144);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(71, 15);
		this.label5.TabIndex = 2;
		this.label5.Text = "Master Item";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(8, 9);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(54, 15);
		this.label4.TabIndex = 3;
		this.label4.Text = "Category";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(8, 115);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(69, 15);
		this.label3.TabIndex = 2;
		this.label3.Text = "Uom Group";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(8, 60);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(80, 15);
		this.label2.TabIndex = 1;
		this.label2.Text = "Service Name";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(8, 35);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(75, 15);
		this.label1.TabIndex = 0;
		this.label1.Text = "Service Code";
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.label13);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 27);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(884, 28);
		this.panel2.TabIndex = 6;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[8] { this.btnnew, this.toolStripButton1, this.btnupdate, this.btnsave, this.btnattachment, this.BtnDefineMasterItem, this.btnServiceCategory, this.btnShortCutKey });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(884, 27);
		this.toolStrip1.TabIndex = 1;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.toolStripButton1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton1.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton1.Name = "toolStripButton1";
		this.toolStripButton1.Size = new System.Drawing.Size(80, 24);
		this.toolStripButton1.Text = "Refresh";
		this.toolStripButton1.Click += new System.EventHandler(toolStripButton1_Click);
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
		this.btnsave.Click += new System.EventHandler(BtnSave_Click);
		this.BtnDefineMasterItem.Image = Architecture.WinApp.Properties.Resources.Export_Reports;
		this.BtnDefineMasterItem.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnDefineMasterItem.Name = "BtnDefineMasterItem";
		this.BtnDefineMasterItem.Size = new System.Drawing.Size(157, 24);
		this.BtnDefineMasterItem.Text = "Define Master Item";
		this.BtnDefineMasterItem.Visible = false;
		this.BtnDefineMasterItem.Click += new System.EventHandler(BtnDefineMasterItem_Click);
		this.btnServiceCategory.Image = Architecture.WinApp.Properties.Resources.Export_Reports;
		this.btnServiceCategory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnServiceCategory.Name = "btnServiceCategory";
		this.btnServiceCategory.Size = new System.Drawing.Size(145, 24);
		this.btnServiceCategory.Text = "Service Category";
		this.btnServiceCategory.Click += new System.EventHandler(btnServiceCategory_Click);
		this.btnShortCutKey.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortCutKey.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKey.Name = "btnShortCutKey";
		this.btnShortCutKey.Size = new System.Drawing.Size(126, 24);
		this.btnShortCutKey.Text = "ShortCut Keys";
		this.btnShortCutKey.Click += new System.EventHandler(btnShortCutKey_Click);
		this.grdhistory.AllowEdit = (InheritableBoolean)2;
		((System.Windows.Forms.Control)(object)this.grdhistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdhistory.FilterMode = (FilterMode)2;
		this.grdhistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdhistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdhistory.GroupTotals = (GroupTotals)2;
		this.grdhistory.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdhistory).Location = new System.Drawing.Point(0, 290);
		((System.Windows.Forms.Control)(object)this.grdhistory).Name = "grdhistory";
		((ScrollableBase)this.grdhistory).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdhistory).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdhistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdhistory).Size = new System.Drawing.Size(884, 371);
		((System.Windows.Forms.Control)(object)this.grdhistory).TabIndex = 2;
		this.grdhistory.TotalRow = (InheritableBoolean)1;
		this.grdhistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdhistory).VisualStyle = (VisualStyle)4;
		this.grdhistory.LinkClicked += new ColumnActionEventHandler(grdhistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdhistory).DoubleClick += new System.EventHandler(grdhistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdhistory).KeyDown += new System.Windows.Forms.KeyEventHandler(grdhistory_KeyDown);
		this.panel9.BackColor = System.Drawing.Color.White;
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.cmbitemcathistory);
		this.panel9.Controls.Add(this.btnsearch);
		this.panel9.Controls.Add(this.label32);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(0, 247);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(884, 43);
		this.panel9.TabIndex = 1;
		this.cmbitemcathistory.AllowNull = (DefaultableBoolean)1;
		this.cmbitemcathistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbitemcathistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbitemcathistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance49).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance49).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance49).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbitemcathistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance50).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance50;
		((SpecialBoxBase)((UltraGridBase)this.cmbitemcathistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance51).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance51).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance51).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance52;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance55;
		((AppearanceBase)appearance56).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance56).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance57).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance57).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance57).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance57).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance57;
		((AppearanceBase)appearance58).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbitemcathistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbitemcathistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbitemcathistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbitemcathistory).Location = new System.Drawing.Point(93, 9);
		((System.Windows.Forms.Control)(object)this.cmbitemcathistory).Name = "cmbitemcathistory";
		((System.Windows.Forms.Control)(object)this.cmbitemcathistory).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.cmbitemcathistory).TabIndex = 0;
		this.btnsearch.BackColor = System.Drawing.Color.Teal;
		this.btnsearch.FlatAppearance.BorderSize = 0;
		this.btnsearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnsearch.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsearch.ForeColor = System.Drawing.Color.White;
		this.btnsearch.Location = new System.Drawing.Point(316, 9);
		this.btnsearch.Name = "btnsearch";
		this.btnsearch.Size = new System.Drawing.Size(66, 26);
		this.btnsearch.TabIndex = 1;
		this.btnsearch.Text = "Search";
		this.btnsearch.UseVisualStyleBackColor = false;
		this.btnsearch.Click += new System.EventHandler(btnsearch_Click);
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.Location = new System.Drawing.Point(8, 15);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(82, 15);
		this.label32.TabIndex = 0;
		this.label32.Text = "Item Category";
		this.panel10.BackColor = System.Drawing.Color.Teal;
		this.panel10.Controls.Add(this.ctrlGrdBar1);
		this.panel10.Controls.Add(this.label31);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(0, 223);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(884, 24);
		this.panel10.TabIndex = 65;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(844, 1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdhistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 20;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.ForeColor = System.Drawing.Color.White;
		this.label31.Location = new System.Drawing.Point(3, 2);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(111, 19);
		this.label31.TabIndex = 0;
		this.label31.Text = "Item History";
		this.btnattachment.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnattachment.Image = (System.Drawing.Image)resources.GetObject("btnattachment.Image");
		this.btnattachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnattachment.Name = "btnattachment";
		this.btnattachment.Size = new System.Drawing.Size(109, 24);
		this.btnattachment.Text = "&Attachment";
		this.btnattachment.Click += new System.EventHandler(btnattachment_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(884, 661);
		base.Controls.Add((System.Windows.Forms.Control)(object)this.grdhistory);
		base.Controls.Add(this.panel9);
		base.Controls.Add(this.panel10);
		base.Controls.Add(this.panel7);
		base.Controls.Add(this.panel2);
		base.Controls.Add(this.toolStrip1);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Name = "EximServicesDefine";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(InvDefrmAddItem_KeyDown);
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbMasterItem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbUomGroup).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemCategory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPurchaseGL).EndInit();
		this.groupBox1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdAllocation).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdhistory).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbitemcathistory).EndInit();
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
