using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Inventory;
using Architecture.BLL.Main;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.Model.Main;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.StoreManagement;

public class frmStockTransferStore : Form
{
	private Rightsobjects formrights = new Rightsobjects();

	private DropDownBind DDL = new DropDownBind();

	private Attachment AT = new Attachment();

	private int updateDetailIndex = -1;

	private int DefaultDaysToLessFromHistoryFromDate;

	private bool IsApproved;

	public int RecId;

	private bool StockTransferFinancialEffectIsActive;

	private bool FifoFeature;

	private bool CGSEntryAllow;

	private bool MultiBranchFeature;

	private bool FormInitialized;

	private DataTable table = new DataTable();

	private DataTable dtExp = new DataTable();

	private DataTable dtAccountlst = new DataTable();

	private DataTable dtRefDoc = new DataTable();

	private List<RacksWithWarehouseAndItem> WarehouseAndRackFromBranchList = new List<RacksWithWarehouseAndItem>();

	private List<RacksWithWarehouseAndItem> WarehouseAndRackToBranchList = new List<RacksWithWarehouseAndItem>();

	public UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel2;

	private TabPage tabPage2;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnupdate;

	private ToolStripButton btnsave;

	private ToolStripButton btnprint;

	private Panel panel4;

	private DateTimePicker DocDate;

	private Label label17;

	private TextBox txtdocno;

	private Button btnplus;

	private TextBox txtQty;

	private Label label38;

	private Label label36;

	private Panel panel6;

	internal UltraCombo CmbPackUom;

	private CtrlGrdBar ctrlGrdBar1;

	private CtrlGrdBar ctrlGrdBar2;

	private Button btnUpdateDetail;

	private Button btnCancelUpdateDetial;

	internal GridEX grdhistory;

	private ToolStripButton btnRefresh;

	internal UltraCombo cmbItemName;

	private Label label2;

	internal UltraCombo CmbWareHouseFrom;

	private Label label3;

	internal GridEX grd;

	private TextBox txtRemarksDetail;

	private Label label12;

	internal UltraCombo cmbWareHouseTo;

	private Label label11;

	private Panel panel3;

	private Panel panel8;

	private CtrlGrdBar ctrlGrdBar4;

	private CtrlGrdBar ctrlGrdBar3;

	private CtrlGrdBar ctrlGrdBar5;

	private Label label15;

	private CheckBox ChkBoxPrintPreview;

	private TextBox txtAvailableStock;

	private TabControl tabControl2;

	private TabPage tabPage3;

	private TabPage tabPage4;

	internal GridEX grdOtherCharges;

	private ToolStripButton btnAttachment;

	private TextBox txtAvgRate;

	private Label lblAvgRate;

	private TextBox txtAmount;

	private Label lblAmount;

	private Label label24;

	private Panel panel5;

	private Panel panel11;

	private Label label23;

	private Panel panel14;

	private Panel panel13;

	private Label label22;

	private GroupBox groupBox2;

	private GroupBox groupBox1;

	private ToolStrip toolStrip2;

	private Label label25;

	internal GridEX GridDetail;

	private CtrlGrdBar ctrlGrdBar6;

	private Label label5;

	private Label lblToBranch;

	internal UltraCombo CmbToBranch;

	private Label lblFromBranch;

	internal UltraCombo CmbFromBranch;

	private ToolStripButton BtnShortCutkeys;

	private ToolStripSeparator toolStripSeparator6;

	private TextBox txtRemarksHead;

	private Panel panel12;

	private GroupBox groupBox3;

	private RadioButton rdapproveddate;

	private RadioButton rdmodifydate;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	private Button btnshow;

	private Label label27;

	private Label label43;

	private Label label62;

	private Label label63;

	private TextBox txtToDocNoHistory;

	private TextBox txtFromDocNoHistory;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	private Panel panel7;

	private Label label1;

	private ToolStripButton btnNewHistory;

	private Panel panel9;

	private Label label6;

	internal UltraCombo CmbRefDocumentType;

	private Label label28;

	internal UltraCombo CmbRefDocInvoiceNo;

	internal UltraCombo CmbRefDocNo;

	private Label label26;

	private Label label4;

	private ToolStripButton btnDelete;

	internal UltraCombo CmbItemCondition;

	private Label label14;

	internal UltraCombo CmbRackTo;

	private Label label7;

	internal UltraCombo CmbRackFrom;

	private Label label13;

	public frmStockTransferStore(UserAccount user)
	{
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void PurchsaeOrder_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(850, 600);
			FormInitialized = true;
			formrights = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnsave.Enabled = formrights.DoHaveSaveRight;
			btnupdate.Enabled = formrights.DoHaveUpdateRights;
			btnprint.Enabled = formrights.DoHaveUpdateRights;
			btnDelete.Enabled = formrights.DoHaveCanDelete;
			btnDelete.Visible = false;
			ChkBoxPrintPreview.Enabled = formrights.DoHaveUpdateRights;
			ChkBoxPrintPreview.Checked = formrights.DoHaveUpdateRights;
			tabControl2.TabPages.Remove(tabPage4);
			lblAvgRate.Visible = false;
			lblAmount.Visible = false;
			txtAvgRate.Visible = false;
			txtAmount.Visible = false;
			CGSEntryAllow = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("CGSEntryAllow"));
			FifoFeature = CommonServices.GetERPFeatureById(5);
			if (FifoFeature)
			{
				StockTransferFinancialEffectIsActive = true;
			}
			else
			{
				StockTransferFinancialEffectIsActive = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("StockTransferFinancialEffectIsActive"));
			}
			if (StockTransferFinancialEffectIsActive)
			{
				tabControl2.TabPages.Add(tabPage4);
				lblAvgRate.Visible = true;
				lblAmount.Visible = true;
				txtAvgRate.Visible = true;
				txtAmount.Visible = true;
			}
			else
			{
				label28.Location = new Point(2, 63);
				((Control)(object)CmbRefDocumentType).Location = new Point(2, 80);
				((Control)(object)CmbRefDocumentType).Size = new Size(350, 26);
			}
			txtAvgRate.Enabled = ((!FifoFeature && !CGSEntryAllow) ? true : false);
			MultiBranchFeature = CommonServices.GetERPFeatureById(11);
			if (MultiBranchFeature)
			{
				BranchesFill();
				lblFromBranch.Visible = true;
				((Control)(object)CmbFromBranch).Visible = true;
				lblToBranch.Visible = true;
				((Control)(object)CmbToBranch).Visible = true;
			}
			else
			{
				lblFromBranch.Visible = false;
				((Control)(object)CmbFromBranch).Visible = false;
				lblToBranch.Visible = false;
				((Control)(object)CmbToBranch).Visible = false;
			}
			table.Columns.Add("Id", typeof(int));
			table.Columns.Add("ItemId", typeof(int));
			table.Columns.Add("ItemName");
			table.Columns.Add("WareHouseFromId", typeof(int));
			table.Columns.Add("WareHouseFrom");
			table.Columns.Add("RackFromId", typeof(int));
			table.Columns.Add("RackFrom");
			table.Columns.Add("WareHouseToId", typeof(int));
			table.Columns.Add("WareHouseTo");
			table.Columns.Add("RackToId", typeof(int));
			table.Columns.Add("RackTo");
			table.Columns.Add("ItemConditionId", typeof(int));
			table.Columns.Add("ItemCondition");
			table.Columns.Add("PackUOMId", typeof(int));
			table.Columns.Add("PackUOM");
			table.Columns.Add("QTY", typeof(double));
			table.Columns.Add("ItemRate");
			table.Columns.Add("ItemAmount", typeof(double));
			table.Columns.Add("Expense", typeof(double));
			table.Columns.Add("RefDocumentTypeId", typeof(int));
			table.Columns.Add("RefDocumentType", typeof(string));
			table.Columns.Add("RefDocId", typeof(int));
			table.Columns.Add("RefDocNo", typeof(string));
			table.Columns.Add("RefDocInvoiceId", typeof(int));
			table.Columns.Add("RefDocInvoiceNo", typeof(string));
			table.Columns.Add("Remarks");
			grd.DataSource = table;
			grd.RetrieveStructure();
			grdSettings();
			dtAccountlst.Columns.Add("Id");
			dtAccountlst.Columns.Add("AccountTitle");
			AccountsFill();
			dtExp.Columns.Add("Account");
			dtExp.Columns.Add("Percentage");
			dtExp.Columns.Add("Qty", typeof(double));
			dtExp.Columns.Add("Rate", typeof(double));
			dtExp.Columns.Add("Amount", typeof(double));
			dtExp.Columns.Add("Remarks");
			AddRowInExpenseGrid();
			grdOtherCharges.RetrieveStructure();
			grdExpenseSettings();
			DocumentNoFill();
			ItemConditionBindFromGlobal();
			ItemDetailFill();
			DocumentTypeFill();
			DocDate.Focus();
			btnupdate.Visible = false;
			btnsave.Visible = true;
			btnplus.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			ChkBoxPrintPreview.Checked = true;
			DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("DefaultDaysToLessFromHistoryFromDate"));
			FromDateHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
			ToDateHistory.Value = DateTime.Now;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DocumentNoFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		try
		{
			int code = InvStockTransferHeader.GenerateCode(new InvStockTransferHeader
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				DocumentTypeId = 807,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			if (code > 0)
			{
				txtdocno.Text = code.ToString();
			}
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
			DataTable dtBranch = DatatableHelper.PopulateDataTableAndReturn(clsGlobalVariables.globalBranchesAllocateToUser, (getGlobalBranchesAllocateToUser x) => x.BranchId, (getGlobalBranchesAllocateToUser x) => x.BranchName);
			InfragisticsHelper.BindAndRetainSelection(CmbFromBranch, dtBranch, "Id", "Description", "From Branch", AllColumns: true, Conversion.ToInt(CmbFromBranch.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			InfragisticsHelper.BindAndRetainSelection(CmbToBranch, dtBranch, "Id", "Description", "To Branch", AllColumns: true, Conversion.ToInt(CmbToBranch.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			if (dtBranch.Rows.Count > 0)
			{
				((Control)(object)CmbFromBranch).Text = UserAccount.BranchName.ToString();
				CmbFromBranch_Leave(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void WareHouseFill()
	{
		try
		{
			DataTable dt = DatatableHelper.PopulateDataTableAndReturn(clsGlobalVariables.globalWarehousesWithBranches, (GlobalWarehousesWithBranch x) => x.WarehouseId, (GlobalWarehousesWithBranch x) => x.WarehouseName);
			int Id = Conversion.ToInt(CmbWareHouseFrom.Value);
			int IdTo = Conversion.ToInt(cmbWareHouseTo.Value);
			InfragisticsHelper.BindAndRetainSelection(CmbWareHouseFrom, dt, "Id", "Description", "WareHouse From", AllColumns: true, Id, insertDefaultRow: true);
			InfragisticsHelper.BindAndRetainSelection(cmbWareHouseTo, dt, "Id", "Description", "WareHouse To", AllColumns: true, IdTo, insertDefaultRow: true);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemDetailFill()
	{
		try
		{
			DataTable dtItem = new DataTable();
			dtItem.Columns.Add("Id", typeof(int));
			dtItem.Columns.Add("ItemName", typeof(string));
			dtItem.Columns.Add("ItemCode", typeof(string));
			dtItem.Columns.Add("ParentCategoryId", typeof(int));
			List<getGlobalAllItems> allItems = clsGlobalVariables.getGlobalAllItems.Where((getGlobalAllItems item) => item.ItemTypeOfTypeId == 14 || item.ItemTypeOfTypeId == 17).ToList();
			if (allItems == null || !allItems.Any())
			{
				return;
			}
			foreach (getGlobalAllItems row in allItems)
			{
				dtItem.Rows.Add(row.Id, row.ItemName, row.ItemCode, row.InventoryParentCategoriesId);
			}
			int Id = Conversion.ToInt(cmbItemName.Value);
			InfragisticsHelper.BindAndRetainSelection(cmbItemName, dtItem, "Id", "ItemName", "Item Name", AllColumns: true, Id, insertDefaultRow: true, null, null, ActivateRow: false);
			if (dtItem.Rows.Count > 0)
			{
				((UltraGridBase)cmbItemName).DisplayLayout.Bands[0].Columns[3].Hidden = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void bindRateUomAndItemPackUom()
	{
		try
		{
			string Packuom = ((Control)(object)CmbPackUom).Text;
			DataTable dtUOM = CommonServices.dtUomFromGloablUomScheduleByItemId(Conversion.ToInt(cmbItemName.Value));
			InfragisticsHelper.BindAndRetainSelection(CmbPackUom, dtUOM, "Id", "UOMCode", "Pack UOM", AllColumns: true, Packuom, insertDefaultRow: true, null, null, ActivateRow: true, 0, "UOMCode");
			if (Conversion.ToInt(CmbPackUom.Value) != 0)
			{
				return;
			}
			int rowCount = dtUOM?.Rows.Count ?? 0;
			if (rowCount == 1)
			{
				CmbPackUom.Value = dtUOM.Rows[0]["Id"];
			}
			else
			{
				if (rowCount <= 1)
				{
					return;
				}
				DataRow eqRow = dtUOM.AsEnumerable().FirstOrDefault((DataRow r) => Conversion.ToDecimal(r["Equivalent"]) == 1m);
				if (eqRow != null)
				{
					int eqId = Conversion.ToInt(eqRow["Id"]);
					UltraGridRow obj = ((IEnumerable)((UltraGridBase)CmbPackUom).Rows).Cast<UltraGridRow>().FirstOrDefault((UltraGridRow r) => Conversion.ToInt(r.Cells["Id"].Value) == eqId);
					if (obj != null)
					{
						obj.Activate();
					}
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ItemConditionBindFromGlobal()
	{
		try
		{
			List<GlobalItemCondition> list = clsGlobalVariables.globalItemConditions.ToList();
			int Id = Conversion.ToInt(CmbItemCondition.Value);
			DataTable dt = DatatableHelper.PopulateDataTableAndReturn(list, (GlobalItemCondition x) => x.Id, (GlobalItemCondition x) => x.ConditionStatus);
			InfragisticsHelper.BindAndRetainSelection(CmbItemCondition, dt, "Id", "Description", "Item Condition", AllColumns: true, Id, insertDefaultRow: true);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbFromBranch_Leave(object sender, EventArgs e)
	{
		try
		{
			int FromId = Conversion.ToInt(CmbFromBranch.Value);
			int ToId = Conversion.ToInt(CmbToBranch.Value);
			if (FromId > 0 && ToId > 0 && FromId == ToId)
			{
				((Control)(object)CmbFromBranch).Text = string.Empty;
				MessageBox.Show("From Branch And To Branch Can't be same", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			WarehouseAndRackFromBranchList = GlobalServicesMethods.GetRacksWithWarehouseByItemId(UserAccount.OrganizationId, UserAccount.CompanyId, Conversion.ToInt(CmbFromBranch.Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FromWarehouseBind()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		try
		{
			if (Conversion.ToInt(CmbFromBranch.Value) > 0)
			{
				DataTable dt = WarehousesAllocationToBranch.GetWarehousesAllocatedToBranchByBranchId(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					BranchesId = Conversion.ToInt(CmbFromBranch.Value)
				});
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, CmbWareHouseFrom, "Id", "WareHouseName", "Warehouse From", ZeroIndex: false);
				}
			}
			else
			{
				((Control)(object)CmbWareHouseFrom).Text = string.Empty;
				((UltraGridBase)CmbWareHouseFrom).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ToWarehouseBind()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		try
		{
			if (Conversion.ToInt(CmbToBranch.Value) > 0)
			{
				DataTable dt = WarehousesAllocationToBranch.GetWarehousesAllocatedToBranchByBranchId(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					BranchesId = Conversion.ToInt(CmbToBranch.Value)
				});
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, cmbWareHouseTo, "Id", "WareHouseName", "Warehouse To", ZeroIndex: false);
				}
			}
			else
			{
				((Control)(object)cmbWareHouseTo).Text = string.Empty;
				((UltraGridBase)cmbWareHouseTo).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbToBranch_Leave(object sender, EventArgs e)
	{
		try
		{
			int FromId = Conversion.ToInt(CmbFromBranch.Value);
			int ToId = Conversion.ToInt(CmbToBranch.Value);
			if (ToId > 0 && FromId > 0 && FromId == ToId)
			{
				((Control)(object)CmbToBranch).Text = string.Empty;
				MessageBox.Show("From Branch And To Branch Can't be same", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			WarehouseAndRackToBranchList = GlobalServicesMethods.GetRacksWithWarehouseByItemId(UserAccount.OrganizationId, UserAccount.CompanyId, Conversion.ToInt(CmbToBranch.Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbRefDocumentType_Leave(object sender, EventArgs e)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		try
		{
			dtRefDoc.Clear();
			int TypeId = Conversion.ToInt(CmbRefDocumentType.Value);
			if (TypeId > 0)
			{
				ReportsParameters obj = new ReportsParameters();
				obj.OrganizationId = UserAccount.OrganizationId;
				obj.CompanyId = UserAccount.CompanyId;
				obj.DocumentTypeId = 245;
				obj.RefDocumentTypeId = TypeId;
				obj.Id = RecId;
				dtRefDoc = PurchaseOrder.GetContractScheduleAndInvoiceDataForPM(obj);
				BindRefInvoiceNo();
				BindRefDocNo();
			}
			else
			{
				((UltraGridBase)CmbRefDocNo).DataSource = null;
				((Control)(object)CmbRefDocNo).Text = string.Empty;
				((UltraGridBase)CmbRefDocInvoiceNo).DataSource = null;
				((Control)(object)CmbRefDocInvoiceNo).Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbRefDocNo_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)CmbRefDocNo).ActiveRow == null || Conversion.ToInt(CmbRefDocNo.Value) <= 0)
			{
				return;
			}
			int Id = Conversion.ToInt(((UltraDropDownBase)CmbRefDocNo).SelectedRow.Cells[2].Value);
			if (((UltraGridBase)CmbRefDocInvoiceNo).DataSource is DataTable dt)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
				{
					CmbRefDocInvoiceNo.Value = Id;
				}
				else
				{
					((Control)(object)CmbRefDocInvoiceNo).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbRefDocInvoiceNo_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)CmbRefDocInvoiceNo).ActiveRow == null || Conversion.ToInt(CmbRefDocInvoiceNo.Value) <= 0)
			{
				return;
			}
			int Id = Conversion.ToInt(((UltraDropDownBase)CmbRefDocInvoiceNo).SelectedRow.Cells[2].Value);
			if (((UltraGridBase)CmbRefDocNo).DataSource is DataTable dt)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
				{
					CmbRefDocNo.Value = Id;
				}
				else
				{
					((Control)(object)CmbRefDocNo).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindRefInvoiceNo()
	{
		try
		{
			int ComboId = Conversion.ToInt((object)Conversion.ToInt(CmbRefDocInvoiceNo.Value));
			if (dtRefDoc.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("No", typeof(string));
				dt.Columns.Add("RefDocId", typeof(int));
				foreach (DataRow r in dtRefDoc.Rows)
				{
					if (Conversion.ToInt(r["RefDocInvoiceId"]) > 0 && !dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Conversion.ToInt(r["RefDocInvoiceId"])))
					{
						dt.Rows.Add(r["RefDocInvoiceId"], r["RefDocInvoiceNo"], r["RefDocId"]);
					}
				}
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, CmbRefDocInvoiceNo, "Id", "No", "Invoice No", ZeroIndex: true);
					((UltraGridBase)CmbRefDocInvoiceNo).DisplayLayout.Bands[0].Columns[2].Hidden = true;
					if (ComboId > 0)
					{
						if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ComboId))
						{
							CmbRefDocInvoiceNo.Value = ComboId;
						}
						else
						{
							((Control)(object)CmbRefDocInvoiceNo).Text = string.Empty;
						}
					}
				}
				else
				{
					((UltraGridBase)CmbRefDocInvoiceNo).DataSource = null;
					((Control)(object)CmbRefDocInvoiceNo).Text = string.Empty;
				}
			}
			else
			{
				((UltraGridBase)CmbRefDocInvoiceNo).DataSource = null;
				((Control)(object)CmbRefDocInvoiceNo).Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindRefDocNo()
	{
		try
		{
			int ComboId = Conversion.ToInt((object)Conversion.ToInt(CmbRefDocNo.Value));
			if (dtRefDoc.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("No", typeof(string));
				dt.Columns.Add("InvoiceId", typeof(int));
				foreach (DataRow r in dtRefDoc.Rows)
				{
					if (Conversion.ToInt(r["RefDocId"]) > 0 && !dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Conversion.ToInt(r["RefDocId"])))
					{
						dt.Rows.Add(r["RefDocId"], r["RefDocNo"], r["RefDocInvoiceId"]);
					}
				}
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, CmbRefDocNo, "Id", "No", "RefDoc No", ZeroIndex: true);
					((UltraGridBase)CmbRefDocNo).DisplayLayout.Bands[0].Columns[2].Hidden = true;
					if (ComboId > 0)
					{
						if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ComboId))
						{
							CmbRefDocNo.Value = ComboId;
						}
						else
						{
							((Control)(object)CmbRefDocNo).Text = string.Empty;
						}
					}
				}
				else
				{
					((UltraGridBase)CmbRefDocNo).DataSource = null;
					((Control)(object)CmbRefDocNo).Text = string.Empty;
				}
			}
			else
			{
				((UltraGridBase)CmbRefDocNo).DataSource = null;
				((Control)(object)CmbRefDocNo).Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DocumentTypeFill()
	{
		try
		{
			int ID = Conversion.ToInt(CmbRefDocumentType.Value);
			DataTable dt = CommonServices.StaticColumnsService("RefDocumentTypeForPackingMaterial");
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbRefDocumentType, "Id", "type", "Document Type", ZeroIndex: true);
				if (ID > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID))
					{
						CmbRefDocumentType.Value = ID;
					}
					else
					{
						((Control)(object)CmbRefDocumentType).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbRefDocumentType).Text = string.Empty;
				((UltraGridBase)CmbRefDocumentType).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (txtdocno.Text.Trim() == string.Empty || txtdocno.Text.Trim() == "0")
		{
			MessageBox.Show("DocNo Field is Required");
			txtdocno.Focus();
			return false;
		}
		if (MultiBranchFeature)
		{
			if (((UltraGridBase)CmbFromBranch).ActiveRow == null || Conversion.ToInt(CmbFromBranch.Value) == 0)
			{
				MessageBox.Show("From Branch Field is Required");
				CmbFromBranch.Focus();
				return false;
			}
			if (((UltraGridBase)CmbToBranch).ActiveRow == null || Conversion.ToInt(CmbToBranch.Value) == 0)
			{
				MessageBox.Show("To Branch Field is Required");
				CmbToBranch.Focus();
				return false;
			}
			if (Conversion.ToInt(CmbFromBranch.Value) == Conversion.ToInt(CmbToBranch.Value))
			{
				MessageBox.Show("From Branch And To Branch can't be same");
				CmbToBranch.Focus();
				return false;
			}
			return true;
		}
		return true;
	}

	private bool FormValidationDetail()
	{
		if (((UltraGridBase)cmbItemName).ActiveRow == null || Conversion.ToInt(cmbItemName.Value) == 0)
		{
			MessageBox.Show("Item Name field is required");
			cmbItemName.Focus();
			return false;
		}
		if (((UltraGridBase)CmbWareHouseFrom).ActiveRow == null || Conversion.ToInt(CmbWareHouseFrom.Value) == 0)
		{
			MessageBox.Show("Warehouse From field is required");
			CmbWareHouseFrom.Focus();
			return false;
		}
		if (((UltraGridBase)CmbRackFrom).ActiveRow == null || Conversion.ToInt(CmbRackFrom.Value) == 0)
		{
			MessageBox.Show("Rack From field is required");
			CmbRackFrom.Focus();
			return false;
		}
		if (((UltraGridBase)cmbWareHouseTo).ActiveRow == null || Conversion.ToInt(cmbWareHouseTo.Value) == 0)
		{
			MessageBox.Show("Warehouse To field is required");
			cmbWareHouseTo.Focus();
			return false;
		}
		if (((UltraGridBase)CmbRackTo).ActiveRow == null || Conversion.ToInt(CmbRackTo.Value) == 0)
		{
			MessageBox.Show("Rack To field is required");
			CmbRackTo.Focus();
			return false;
		}
		if (Conversion.ToInt(CmbRackFrom.Value) == Conversion.ToInt(CmbRackTo.Value))
		{
			MessageBox.Show("Rack From and Rack To cannot be the same. Please select a different rack.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			CmbRackTo.Focus();
			return false;
		}
		if (((UltraGridBase)CmbItemCondition).ActiveRow == null || Conversion.ToInt(CmbItemCondition.Value) == 0)
		{
			MessageBox.Show("Item Condition field is required");
			CmbItemCondition.Focus();
			return false;
		}
		if (((UltraGridBase)CmbPackUom).ActiveRow == null || Conversion.ToInt(CmbPackUom.Value) == 0)
		{
			MessageBox.Show("PackUOM field is required");
			CmbPackUom.Focus();
			return false;
		}
		if (StockTransferFinancialEffectIsActive)
		{
			if (txtAvgRate.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtAvgRate.Text) == 0.0)
			{
				MessageBox.Show("AvgRate field is required");
				txtAvgRate.Focus();
				return false;
			}
			if (txtAmount.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtAmount.Text) == 0.0)
			{
				MessageBox.Show("Amount field is required");
				txtAmount.Focus();
				return false;
			}
		}
		if (txtQty.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtQty.Text) == 0.0)
		{
			MessageBox.Show("Qty field is required");
			txtQty.Focus();
			return false;
		}
		return true;
	}

	private void AccountsFill()
	{
		try
		{
			DataTable dtcoa = CommonServices.CoaAllocationGetAllServiceBind();
			if (dtcoa.Rows.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < dtcoa.Rows.Count; i++)
			{
				if (Convert.ToInt32(dtcoa.Rows[i]["AccountTypeId"]) != 2 && Convert.ToInt32(dtcoa.Rows[i]["AccountTypeId"]) != 11 && Convert.ToInt32(dtcoa.Rows[i]["AccountTypeId"]) != 12 && Convert.ToInt32(dtcoa.Rows[i]["AccountTypeId"]) != 15)
				{
					dtAccountlst.Rows.Add(dtcoa.Rows[i]["Id"], dtcoa.Rows[i]["AccountTitle"]);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInExpenseGrid()
	{
		dtExp.Rows.Add(0, 0, 0, 0, 0, "");
		grdOtherCharges.DataSource = dtExp;
	}

	private void grdExpenseSettings()
	{
		try
		{
			grdOtherCharges.RootTable.Columns["Amount"].Caption = "Credit";
			grdOtherCharges.RootTable.Columns["Account"].Caption = "AccountTitle";
			grdOtherCharges.RootTable.Columns["Percentage"].Caption = "%";
			grdOtherCharges.RootTable.Columns["Account"].ColumnType = (ColumnType)0;
			grdOtherCharges.RootTable.Columns["Account"].HasValueList = true;
			grdOtherCharges.RootTable.Columns["Account"].LimitToList = true;
			grdOtherCharges.RootTable.Columns["Account"].EditType = (EditType)4;
			grdOtherCharges.RootTable.Columns["Account"].ValueList.PopulateValueList((IEnumerable)dtAccountlst.DefaultView, "Id", "AccountTitle");
			grdOtherCharges.RootTable.Columns["Account"].Width = 150;
			grdOtherCharges.RootTable.Columns["Remarks"].Width = 120;
			grdOtherCharges.RootTable.Columns["Amount"].Width = 80;
			grdOtherCharges.RootTable.Columns["Percentage"].Width = 50;
			grdOtherCharges.RootTable.Columns.Add("Delete");
			grdOtherCharges.RootTable.Columns["Delete"].Key = "Delete";
			grdOtherCharges.RootTable.Columns["Delete"].Caption = "X";
			grdOtherCharges.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdOtherCharges.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdOtherCharges.RootTable.Columns["Delete"].ButtonText = "X";
			grdOtherCharges.RootTable.Columns["Delete"].Width = 20;
			grdOtherCharges.RootTable.Columns.Add("Add");
			grdOtherCharges.RootTable.Columns["Add"].Key = "Add";
			grdOtherCharges.RootTable.Columns["Add"].Caption = "+";
			grdOtherCharges.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdOtherCharges.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdOtherCharges.RootTable.Columns["Add"].ButtonText = "+";
			grdOtherCharges.RootTable.Columns["Add"].Width = 20;
			grdOtherCharges.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdOtherCharges.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdOtherCharges.RootTable.Columns["Amount"].FormatString = "#,#.###";
			grdOtherCharges.RootTable.Columns["Amount"].TotalFormatString = "#,#.###";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OtherExpensesProportion()
	{
		try
		{
			if (grd.GetRows().Length != 0)
			{
				double TQty = Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["QTY"], (AggregateFunction)2).ToString());
				double CreditForExpense = Conversion.ToDouble((object)grdOtherCharges.GetTotal(grdOtherCharges.RootTable.Columns["Amount"], (AggregateFunction)2).ToString());
				if (CreditForExpense > 0.0)
				{
					GridEXRow[] rows = grd.GetRows();
					foreach (GridEXRow obj in rows)
					{
						obj.BeginEdit();
						double qty = Conversion.ToDouble((object)obj.Cells["QTY"].Value.ToString());
						obj.Cells["Expense"].Value = CreditForExpense / TQty * qty;
					}
				}
				else
				{
					GridEXRow[] rows = grd.GetRows();
					foreach (GridEXRow obj2 in rows)
					{
						obj2.BeginEdit();
						obj2.Cells["Expense"].Value = 0;
					}
				}
			}
			grd.UpdateData();
			grdOtherCharges.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OtherChargesAmountUpdate()
	{
		try
		{
			GridEXRow[] rows = grdOtherCharges.GetRows();
			foreach (GridEXRow item in rows)
			{
				if (Conversion.ToDouble(item.Cells["Amount"].Value) > 0.0 && item.Cells["Percentage"].Value.ToString() != string.Empty)
				{
					item.BeginEdit();
					double num = Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["ItemAmount"], (AggregateFunction)2).ToString());
					double Percentage = Conversion.ToDouble((object)item.Cells["Percentage"].Value.ToString());
					double TotlaPercentage = num / 100.0 * Percentage;
					if (TotlaPercentage > 0.0)
					{
						item.Cells["Amount"].Value = Conversion.ToDouble((object)TotlaPercentage);
					}
					item.Cells["Qty"].Value = 0;
					item.Cells["Rate"].Value = 0;
					item.EndEdit();
				}
			}
			grdOtherCharges.UpdateData();
			OtherExpensesProportion();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOtherCharges_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdOtherCharges.UpdateData();
			GridEXRow item = grdOtherCharges.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				if ((e.Column.Key == "Qty" || e.Column.Key == "Rate") && item.Cells["Qty"].Value.ToString() != string.Empty && item.Cells["Rate"].Value.ToString() != string.Empty)
				{
					double Qty = Conversion.ToDouble((object)item.Cells["Qty"].Value.ToString());
					double Rate = Conversion.ToDouble((object)item.Cells["Rate"].Value.ToString());
					item.Cells["Amount"].Value = Qty * Rate;
					item.Cells["Percentage"].Value = 0;
				}
				if (e.Column.Key == "Percentage" && item.Cells["Percentage"].Value.ToString() != string.Empty)
				{
					double num = Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["ItemAmount"], (AggregateFunction)2).ToString());
					double Percentage = Conversion.ToDouble((object)item.Cells["Percentage"].Value.ToString());
					double TotlaPercentage = num / 100.0 * Percentage;
					if (TotlaPercentage > 0.0)
					{
						item.Cells["Amount"].Value = Conversion.ToDouble((object)TotlaPercentage);
					}
					item.Cells["Qty"].Value = 0;
					item.Cells["Rate"].Value = 0;
				}
			}
			grdOtherCharges.UpdateData();
			OtherExpensesProportion();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOtherCharges_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdOtherCharges.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				if (e.Column.Key == "Delete")
				{
					DeleteOtherDetailRow(r);
				}
				if (e.Column.Key == "Add")
				{
					AddRowInExpenseGrid();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeleteOtherDetailRow(GridEXRow r)
	{
		try
		{
			dtExp.Rows[r.RowIndex].Delete();
			r.Delete();
			if (dtExp.Rows.Count == 0)
			{
				AddRowInExpenseGrid();
			}
			grdOtherCharges.UpdateData();
			OtherExpensesProportion();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelUpdateDetial_Click(object sender, EventArgs e)
	{
		try
		{
			ResetDetail();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdateDetail_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValidationDetail())
			{
				int RefDocumentTypeId = Conversion.ToInt(CmbRefDocumentType.Value);
				string RefDocumentType = Conversion.ToString((object)((Control)(object)CmbRefDocumentType).Text);
				int RefDocId = 0;
				string RefDocNo = "";
				int RefDocInvoiceId = 0;
				string RefDocInvoiceNo = "";
				if (RefDocumentTypeId > 0)
				{
					RefDocId = Conversion.ToInt(CmbRefDocNo.Value);
					RefDocNo = Conversion.ToString((object)((Control)(object)CmbRefDocNo).Text);
					RefDocInvoiceId = Conversion.ToInt(CmbRefDocInvoiceNo.Value);
					RefDocInvoiceNo = Conversion.ToString((object)((Control)(object)CmbRefDocInvoiceNo).Text);
				}
				table.Rows[updateDetailIndex]["ItemId"] = cmbItemName.Value;
				table.Rows[updateDetailIndex]["ItemName"] = ((Control)(object)cmbItemName).Text;
				table.Rows[updateDetailIndex]["WareHouseFromId"] = CmbWareHouseFrom.Value;
				table.Rows[updateDetailIndex]["WareHouseFrom"] = ((Control)(object)CmbWareHouseFrom).Text;
				table.Rows[updateDetailIndex]["RackFromId"] = CmbRackFrom.Value;
				table.Rows[updateDetailIndex]["RackFrom"] = ((Control)(object)CmbRackFrom).Text;
				table.Rows[updateDetailIndex]["WareHouseToId"] = cmbWareHouseTo.Value;
				table.Rows[updateDetailIndex]["WareHouseTo"] = ((Control)(object)cmbWareHouseTo).Text;
				table.Rows[updateDetailIndex]["RackToId"] = CmbRackTo.Value;
				table.Rows[updateDetailIndex]["RackTo"] = ((Control)(object)CmbRackTo).Text;
				table.Rows[updateDetailIndex]["QTY"] = Conversion.ToDouble((object)txtQty.Text);
				table.Rows[updateDetailIndex]["ItemConditionId"] = CmbItemCondition.Value;
				table.Rows[updateDetailIndex]["ItemCondition"] = ((Control)(object)CmbItemCondition).Text;
				table.Rows[updateDetailIndex]["PackUOMId"] = CmbPackUom.Value;
				table.Rows[updateDetailIndex]["PackUOM"] = ((Control)(object)CmbPackUom).Text;
				table.Rows[updateDetailIndex]["ItemRate"] = Conversion.ToDouble((object)txtAvgRate.Text);
				table.Rows[updateDetailIndex]["ItemAmount"] = Conversion.ToDouble((object)txtAmount.Text);
				table.Rows[updateDetailIndex]["RefDocumentTypeId"] = RefDocumentTypeId;
				table.Rows[updateDetailIndex]["RefDocumentType"] = ((RefDocumentTypeId > 0) ? RefDocumentType : "");
				table.Rows[updateDetailIndex]["RefDocId"] = RefDocId;
				table.Rows[updateDetailIndex]["RefDocNo"] = RefDocNo;
				table.Rows[updateDetailIndex]["RefDocInvoiceId"] = RefDocInvoiceId;
				table.Rows[updateDetailIndex]["RefDocInvoiceNo"] = RefDocInvoiceNo;
				table.Rows[updateDetailIndex]["Remarks"] = txtRemarksDetail.Text;
				grd.DataSource = table;
				ResetDetail();
				OtherExpensesProportion();
				OtherChargesAmountUpdate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grd.CurrentRow;
			if (item != null)
			{
				updateDetailIndex = item.RowIndex;
				cmbItemName.Value = item.Cells["ItemId"].Value.ToString();
				cmbItemName_Leave(null, null);
				CmbWareHouseFrom.Value = item.Cells["WareHouseFromId"].Value.ToString();
				CmbRackFrom.Value = item.Cells["RackFromId"].Value.ToString();
				cmbWareHouseTo.Value = item.Cells["WareHouseToId"].Value.ToString();
				CmbRackTo.Value = item.Cells["RackToId"].Value.ToString();
				CmbItemCondition.Value = Conversion.ToInt(item.Cells["ItemConditionId"].Value);
				CmbPackUom.Value = item.Cells["PackUOMId"].Value.ToString();
				((Control)(object)CmbPackUom).Text = item.Cells["PackUOM"].Value.ToString();
				txtQty.Text = Conversion.ToDouble(item.Cells["QTY"].Value).ToString("#,##0.###");
				txtAvgRate.Text = Conversion.ToDouble(item.Cells["ItemRate"].Value).ToString(clsGlobalVariables.DecimalRateFormate);
				txtAmount.Text = Conversion.ToDouble(item.Cells["ItemAmount"].Value).ToString(clsGlobalVariables.stringFormatsingle);
				CmbRefDocumentType.Value = item.Cells["RefDocumentTypeId"].Value.ToString();
				CmbRefDocumentType_Leave(null, null);
				CmbRefDocNo.Value = item.Cells["RefDocId"].Value.ToString();
				CmbRefDocInvoiceNo.Value = item.Cells["RefDocInvoiceId"].Value.ToString();
				txtRemarksDetail.Text = item.Cells["Remarks"].Value.ToString();
				btnplus.Visible = false;
				btnUpdateDetail.Visible = true;
				btnCancelUpdateDetial.Visible = true;
				CmbWareHouseFrom.Focus();
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
			grd.ColumnAutoResize = false;
			List<string> hiddenColumns = new List<string>
			{
				"Id", "WareHouseFromId", "RackFromId", "WareHouseToId", "RackToId", "ItemId", "PackUOMId", "ItemConditionId", "RefDocInvoiceId", "RefDocId",
				"RefDocumentTypeId"
			};
			if (!StockTransferFinancialEffectIsActive)
			{
				hiddenColumns.AddRange(new List<string> { "ItemRate", "ItemAmount", "Expense" });
			}
			GridEX_Helper.GridWrappingAndColumnSettings(grd, 2, 3, hiddenColumns);
			grd.RootTable.Columns["WareHouseFrom"].Width = Constants.InventoryConstants.WareHouseName;
			grd.RootTable.Columns["RackFrom"].Width = Constants.InventoryConstants.StringConstant;
			grd.RootTable.Columns["WareHouseTo"].Width = Constants.InventoryConstants.WareHouseName;
			grd.RootTable.Columns["RackTo"].Width = Constants.InventoryConstants.StringConstant;
			grd.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			grd.RootTable.Columns["ItemCondition"].Width = Constants.InventoryConstants.StringConstant;
			grd.RootTable.Columns["PackUOM"].Width = Constants.InventoryConstants.UomConstant;
			grd.RootTable.Columns["QTY"].Width = Constants.InventoryConstants.QtyConstant;
			grd.RootTable.Columns["ItemRate"].Width = Constants.InventoryConstants.ExRateConstant;
			grd.RootTable.Columns["ItemAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["Expense"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["RefDocumentType"].Width = Constants.InventoryConstants.DocumentType;
			grd.RootTable.Columns["RefDocNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grd.RootTable.Columns["RefDocInvoiceNo"].Width = Constants.InventoryConstants.DocNoConstant + 20;
			grd.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants;
			GridEX_Helper.AddButton(grd, "Delete", "X", 20, 0);
			GridEX_Helper.AddButton(grd, "Edit", "Edit", 35, 1);
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ConfigureNumericalColumn(GridEXColumn column)
	{
		if (column.Key.Contains("Amount"))
		{
			column.AggregateFunction = (AggregateFunction)2;
			column.TextAlignment = (TextAlignment)3;
			column.FormatString = clsGlobalVariables.stringFormatsingle;
			column.TotalFormatString = clsGlobalVariables.stringFormatsingle;
		}
		else if (column.Key.Contains("Rate"))
		{
			column.TextAlignment = (TextAlignment)3;
			column.FormatString = clsGlobalVariables.DecimalRateFormate;
		}
		else
		{
			column.AggregateFunction = (AggregateFunction)2;
			column.TextAlignment = (TextAlignment)3;
			column.FormatString = "#,##0.##";
			column.TotalFormatString = "#,##0.##";
		}
	}

	private void btnplus_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValidationDetail())
			{
				int RefDocumentTypeId = Conversion.ToInt(CmbRefDocumentType.Value);
				string RefDocumentType = Conversion.ToString((object)((Control)(object)CmbRefDocumentType).Text);
				int RefDocId = 0;
				string RefDocNo = "";
				int RefDocInvoiceId = 0;
				string RefDocInvoiceNo = "";
				if (RefDocumentTypeId > 0)
				{
					RefDocId = Conversion.ToInt(CmbRefDocNo.Value);
					RefDocNo = Conversion.ToString((object)((Control)(object)CmbRefDocNo).Text);
					RefDocInvoiceId = Conversion.ToInt(CmbRefDocInvoiceNo.Value);
					RefDocInvoiceNo = Conversion.ToString((object)((Control)(object)CmbRefDocInvoiceNo).Text);
				}
				table.Rows.Add(0, Conversion.ToInt(cmbItemName.Value), ((Control)(object)cmbItemName).Text.Trim(), Conversion.ToInt(CmbWareHouseFrom.Value), ((Control)(object)CmbWareHouseFrom).Text.Trim(), Conversion.ToInt(CmbRackFrom.Value), ((Control)(object)CmbRackFrom).Text.Trim(), Conversion.ToInt(cmbWareHouseTo.Value), ((Control)(object)cmbWareHouseTo).Text.Trim(), Conversion.ToInt(CmbRackTo.Value), ((Control)(object)CmbRackTo).Text.Trim(), Conversion.ToInt(CmbItemCondition.Value), ((Control)(object)CmbItemCondition).Text.Trim(), Conversion.ToInt(CmbPackUom.Value), ((Control)(object)CmbPackUom).Text.Trim(), Conversion.ToDouble((object)txtQty.Text.Trim()), Conversion.ToDouble((object)txtAvgRate.Text), Conversion.ToDouble((object)txtAmount.Text), 0, RefDocumentTypeId, (RefDocumentTypeId > 0) ? RefDocumentType : "", RefDocId, RefDocNo, RefDocInvoiceId, RefDocInvoiceNo, txtRemarksDetail.Text.Trim());
				grd.DataSource = table;
				grd.RetrieveStructure();
				grdSettings();
				OtherExpensesProportion();
				ResetDetail();
				if (MultiBranchFeature)
				{
					ToggleBranchControls();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void ToggleBranchControls()
	{
		try
		{
			bool hasRows = false;
			if (grd.DataSource is DataTable dt)
			{
				hasRows = dt.Rows.Count > 0;
			}
			else if (grd.GetRows().Length != 0)
			{
				hasRows = true;
			}
			((Control)(object)CmbFromBranch).Enabled = !hasRows;
			((Control)(object)CmbToBranch).Enabled = !hasRows;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeleteDetailRow(GridEXRow r)
	{
		try
		{
			if (updateDetailIndex == -1)
			{
				table.Rows[r.RowIndex].Delete();
				r.Delete();
				grd.UpdateData();
				OtherExpensesProportion();
				if (MultiBranchFeature)
				{
					ToggleBranchControls();
				}
				return;
			}
			throw new Exception("Please Reset the detail first...");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				if (e.Column.Key == "Delete")
				{
					DeleteDetailRow(r);
				}
				if (e.Column.Key == "Edit")
				{
					grd_DoubleClick(null, null);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected O, but got Unknown
		//IL_0322: Unknown result type (might be due to invalid IL or missing references)
		//IL_0329: Expected O, but got Unknown
		//IL_08d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_08da: Expected O, but got Unknown
		try
		{
			if (grd.GetRows().Count() == 0)
			{
				MessageBox.Show("Grid Record Not Found");
			}
			else
			{
				if (!FormValidation())
				{
					return;
				}
				InvStockTransferHeader sh = new InvStockTransferHeader();
				if (RecId > 0)
				{
					if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					sh.Id = RecId;
					sh.ModifyUser = UserAccount.ID;
				}
				else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				GridEXRow[] rows = grdOtherCharges.GetRows();
				foreach (GridEXRow r3 in rows)
				{
					if (!(Conversion.ToDouble(r3.Cells["Amount"].Value) > 0.0))
					{
						continue;
					}
					if (Conversion.ToInt(r3.Cells["Account"].Value) == 0)
					{
						throw new Exception("Please Select an Account Against OtherCharges Amount First");
					}
					if (Conversion.ToDouble(r3.Cells["Amount"].Value) > 0.0)
					{
						string FrRemarks = Conversion.ToString(r3.Cells["Remarks"].Value);
						if (FrRemarks == string.Empty || FrRemarks == null)
						{
							tabControl2.SelectedIndex = 1;
							((Control)(object)grdOtherCharges).Focus();
							throw new Exception("Grid Charge to Product Remarks required Please Check");
						}
					}
				}
				sh.OrganizationId = UserAccount.OrganizationId;
				sh.CompanyId = UserAccount.CompanyId;
				sh.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
				sh.BranchesId = UserAccount.BranchesId;
				sh.ProjectsId = UserAccount.BranchesId;
				sh.DocumentTypeId = 807;
				sh.EntryUser = UserAccount.ID;
				sh.ModifyUser = UserAccount.ID;
				sh.ApprovedUserId = UserAccount.ID;
				sh.EntryDate = DateTime.Now;
				sh.ModifyDate = DateTime.Now;
				sh.ApprovedDate = DateTime.Now;
				sh.DocNo = Conversion.ToInt((object)txtdocno.Text);
				sh.DocDate = Conversion.ToDateTime((object)DocDate.Value);
				sh.TransferType = "MoveOrder";
				sh.RemarksHeader = Conversion.ToString((object)txtRemarksHead.Text);
				if (MultiBranchFeature)
				{
					DataTable dt = (DataTable)((UltraGridBase)CmbFromBranch).DataSource;
					sh.NoOfBranches = dt.Rows.Count;
					sh.FromBranchId = Conversion.ToInt(CmbFromBranch.Value);
					sh.ToBranchId = Conversion.ToInt(CmbToBranch.Value);
				}
				else
				{
					sh.NoOfBranches = 0;
					sh.FromBranchId = 0;
					sh.ToBranchId = 0;
				}
				sh.InvStockTransferDetail = new List<InvStockTransferDetail>();
				sh.InvStockTransferExpense = new List<InvStockTransferExpense>();
				rows = grd.GetRows();
				foreach (GridEXRow r2 in rows)
				{
					InvStockTransferDetail vd = new InvStockTransferDetail();
					if (Conversion.ToInt(r2.Cells["WareHouseFromId"].Value) == 0)
					{
						throw new Exception($"FromWarehouse Field is Required.in Detail Grid Row No : {r2.RowIndex + 1}");
					}
					if (Conversion.ToInt(r2.Cells["WareHouseToId"].Value) == 0)
					{
						throw new Exception($"WarehouseTo Field is Required.in Detail Grid Row No : {r2.RowIndex + 1}");
					}
					if (Conversion.ToInt(r2.Cells["RackFromId"].Value) == 0)
					{
						throw new Exception($"Rack From Field is Required.in Detail Grid Row No : {r2.RowIndex + 1}");
					}
					if (Conversion.ToInt(r2.Cells["RackToId"].Value) == 0)
					{
						throw new Exception($"Rack To Field is Required.in Detail Grid Row No : {r2.RowIndex + 1}");
					}
					if (Conversion.ToInt(r2.Cells["ItemId"].Value) == 0)
					{
						throw new Exception("ItemId not found");
					}
					if (Conversion.ToInt(r2.Cells["PackUOMId"].Value) == 0)
					{
						throw new Exception($"Pack Uom  not Found in Detail Grid Row No : {r2.RowIndex + 1}");
					}
					if (Conversion.ToInt(r2.Cells["ItemConditionId"].Value) == 0)
					{
						throw new Exception($"Item Condition  not Found in Detail Grid Row No : {r2.RowIndex + 1}");
					}
					if (Conversion.ToInt(r2.Cells["QTY"].Value) == 0)
					{
						throw new Exception($"To QTY not Found in Detail Grid Row No : {r2.RowIndex + 1}");
					}
					if (StockTransferFinancialEffectIsActive)
					{
						if (Conversion.ToDouble(r2.Cells["ItemRate"].Value) <= 0.0)
						{
							throw new Exception($"AvgRate field Required.in Detail Grid Row No : {r2.RowIndex + 1}");
						}
						if (Conversion.ToDouble(r2.Cells["ItemAmount"].Value) <= 0.0)
						{
							throw new Exception($"Amount field Required.in Detail Grid Row No : {r2.RowIndex + 1}");
						}
					}
					if (Conversion.ToInt(r2.Cells["RefDocumentTypeId"].Value) > 0 && Conversion.ToInt(r2.Cells["RefDocId"].Value) == 0 && Conversion.ToInt(r2.Cells["RefDocInvoiceId"].Value) == 0)
					{
						MessageBox.Show($"RefDocNo or RefDocInvoiceNo field Required in Detail grid Row No : {r2.RowIndex + 1}");
						return;
					}
					vd.Id = Conversion.ToInt(r2.Cells["Id"].Value);
					vd.ItemId = Conversion.ToInt(r2.Cells["ItemId"].Value);
					vd.FromWarehouseId = Conversion.ToInt(r2.Cells["WareHouseFromId"].Value);
					vd.RackId = Conversion.ToInt(r2.Cells["RackFromId"].Value);
					vd.rackName = Conversion.ToString(r2.Cells["RackFrom"].Value);
					vd.ToWarehouseId = Conversion.ToInt(r2.Cells["WareHouseToId"].Value);
					vd.ToRackId = Conversion.ToInt(r2.Cells["RackToId"].Value);
					vd.TorackName = Conversion.ToString(r2.Cells["RackTo"].Value);
					vd.ItemConditionId = Conversion.ToInt(r2.Cells["ItemConditionId"].Value);
					vd.PackUomId = Conversion.ToInt(r2.Cells["PackUOMId"].Value);
					vd.Qty = Conversion.ToDouble(r2.Cells["QTY"].Value);
					vd.GrossWeight = vd.Qty;
					vd.NetWeight = vd.Qty;
					vd.ItemRate = Conversion.ToDouble(r2.Cells["ItemRate"].Value);
					vd.ItemAmount = Conversion.ToDouble(r2.Cells["ItemAmount"].Value);
					vd.ExpenseAmount = Conversion.ToDouble(r2.Cells["Expense"].Value);
					vd.RefDocumentTypeId = Conversion.ToInt(r2.Cells["RefDocumentTypeId"].Value);
					vd.RefDocNoId = Conversion.ToInt(r2.Cells["RefDocId"].Value);
					vd.RefDocInvoiceId = Conversion.ToInt(r2.Cells["RefDocInvoiceId"].Value);
					vd.RemarksDetail = Conversion.ToString((object)r2.Cells["Remarks"].Text);
					sh.InvStockTransferDetail.Add(vd);
				}
				rows = grdOtherCharges.GetRows();
				foreach (GridEXRow r in rows)
				{
					if (Conversion.ToInt((object)r.Cells["Account"].Value.ToString()) != 0)
					{
						InvStockTransferExpense PF = new InvStockTransferExpense();
						PF.AccountId = Conversion.ToInt(r.Cells["Account"].Value);
						PF.Percentage = Conversion.ToDouble(r.Cells["Percentage"].Value);
						PF.Qty = Conversion.ToDouble(r.Cells["Qty"].Value);
						PF.Rate = Conversion.ToDouble(r.Cells["Rate"].Value);
						PF.Amount = Conversion.ToDouble(r.Cells["Amount"].Value);
						PF.Remarks = Conversion.ToString(r.Cells["Remarks"].Value);
						sh.InvStockTransferExpense.Add(PF);
					}
				}
				sh.AttachmentsList = new List<DMSAttachments>();
				sh.DeleteAttachmentsList = new List<DMSAttachments>();
				FormHelper.UpdateAttachmentsForObject<InvStockTransferHeader>(sh, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
				int success = InvStockTransferHeader.Save(sh);
				if (RecId > 0)
				{
					MessageBox.Show("Data Update Successfully....  " + sh.DocNo);
					FormHelper.DeleteAllAttachmentsFromDb(AT, RecId, 807, base.Name);
				}
				else
				{
					MessageBox.Show("Data Save Successfully....  " + sh.DocNo);
				}
				Reset();
				if (ChkBoxPrintPreview.Checked)
				{
					GeneratePrint(success);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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

	private void btnupdate_Click(object sender, EventArgs e)
	{
		try
		{
			if (IsApproved)
			{
				MessageBox.Show("Approved Record Can't Update");
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

	public void ReadById(int ID)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown
		try
		{
			RecId = ID;
			InvStockTransferHeader Stock = new InvStockTransferHeader();
			Stock = InvStockTransferHeader.GetByID(ID);
			if (Stock != null && Stock.InvStockTransferDetail.Count > 0)
			{
				tabControl1.SelectedIndex = 0;
				btnsave.Visible = false;
				btnupdate.Visible = true;
				btnDelete.Visible = formrights.DoHaveCanDelete;
				DocDate.Value = Conversion.ToDateTime((object)Stock.DocDate);
				txtdocno.Text = Conversion.ToString((object)Stock.DocNo);
				txtRemarksHead.Text = Conversion.ToString((object)Stock.RemarksHeader);
				IsApproved = Stock.IsApproved;
				if (Stock.FromBranchId > 0)
				{
					CmbFromBranch.Value = Stock.FromBranchId;
					CmbFromBranch_Leave(null, null);
				}
				if (Stock.ToBranchId > 0)
				{
					CmbToBranch.Value = Stock.ToBranchId;
					CmbToBranch_Leave(null, null);
				}
				table.Clear();
				for (int j = 0; j < Stock.InvStockTransferDetail.Count; j++)
				{
					table.Rows.Add(Stock.InvStockTransferDetail[j].Id, Stock.InvStockTransferDetail[j].ItemId, Stock.InvStockTransferDetail[j].ItemName, Stock.InvStockTransferDetail[j].FromWarehouseId, Stock.InvStockTransferDetail[j].FromWareHouseName, Stock.InvStockTransferDetail[j].RackId, Stock.InvStockTransferDetail[j].rackName, Stock.InvStockTransferDetail[j].ToWarehouseId, Stock.InvStockTransferDetail[j].ToWareHouseName, Stock.InvStockTransferDetail[j].ToRackId, Stock.InvStockTransferDetail[j].TorackName, Stock.InvStockTransferDetail[j].ItemConditionId, Stock.InvStockTransferDetail[j].ItemCondition, Stock.InvStockTransferDetail[j].PackUomId, Stock.InvStockTransferDetail[j].PackUom, Stock.InvStockTransferDetail[j].Qty, Stock.InvStockTransferDetail[j].ItemRate, Stock.InvStockTransferDetail[j].ItemAmount, Stock.InvStockTransferDetail[j].ExpenseAmount, Stock.InvStockTransferDetail[j].RefDocumentTypeId, Stock.InvStockTransferDetail[j].RefDocumentType, Stock.InvStockTransferDetail[j].RefDocNoId, Stock.InvStockTransferDetail[j].RefDocNo, Stock.InvStockTransferDetail[j].RefDocInvoiceId, Stock.InvStockTransferDetail[j].RefDocInvoiceNo, Stock.InvStockTransferDetail[j].RemarksDetail);
				}
				grd.DataSource = table;
				grd.RetrieveStructure();
				grdSettings();
				dtExp.Rows.Clear();
				for (int i = 0; i < Stock.InvStockTransferExpense.Count; i++)
				{
					dtExp.Rows.Add(Stock.InvStockTransferExpense[i].AccountId, Stock.InvStockTransferExpense[i].Percentage, Stock.InvStockTransferExpense[i].Qty, Stock.InvStockTransferExpense[i].Rate, Stock.InvStockTransferExpense[i].Amount, Stock.InvStockTransferExpense[i].Remarks);
				}
				if (dtExp.Rows.Count == 0)
				{
					AddRowInExpenseGrid();
				}
				grdOtherCharges.DataSource = dtExp;
				grdOtherCharges.RetrieveStructure();
				grdExpenseSettings();
				FormHelper.LoadAttachmentsForObject(AT, RecId, base.Name);
				if (MultiBranchFeature)
				{
					ToggleBranchControls();
				}
				return;
			}
			Reset();
			throw new Exception("Record not found");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtdocno_Leave(object sender, EventArgs e)
	{
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			List<string> allServices = new List<string> { "BranchesAllocatedToUser", "RacksWithWarehouseAndItem", "WarehousesWithBranches", "UomSchedule", "AllItems", "ItemConditions" };
			DatatableHelper.GlobalServicesDbCall(UserAccount, allServices);
			BranchesFill();
			ItemDetailFill();
			ItemConditionBindFromGlobal();
			DocumentTypeFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		Reset();
		ResetDetail();
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void Reset()
	{
		try
		{
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			RecId = 0;
			IsApproved = false;
			txtRemarksHead.Text = string.Empty;
			((Control)(object)CmbToBranch).Text = string.Empty;
			txtAvgRate.Text = string.Empty;
			txtAmount.Text = string.Empty;
			btnsave.Visible = true;
			btnupdate.Visible = false;
			btnplus.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			btnDelete.Visible = false;
			dtExp.Rows.Clear();
			AddRowInExpenseGrid();
			grdOtherCharges.RetrieveStructure();
			grdExpenseSettings();
			((Control)(object)cmbItemName).Enabled = true;
			((Control)(object)CmbWareHouseFrom).Enabled = true;
			((Control)(object)CmbWareHouseFrom).Text = string.Empty;
			((UltraGridBase)CmbWareHouseFrom).DataSource = null;
			((Control)(object)CmbRackFrom).Text = string.Empty;
			((UltraGridBase)CmbRackFrom).DataSource = null;
			((Control)(object)cmbWareHouseTo).Text = string.Empty;
			((UltraGridBase)cmbWareHouseTo).DataSource = null;
			((Control)(object)CmbRackTo).Text = string.Empty;
			((UltraGridBase)CmbRackTo).DataSource = null;
			((Control)(object)CmbPackUom).Enabled = true;
			((Control)(object)CmbPackUom).Text = string.Empty;
			((Control)(object)CmbRefDocumentType).Text = string.Empty;
			((Control)(object)CmbRefDocNo).Text = string.Empty;
			((Control)(object)CmbRefDocInvoiceNo).Text = string.Empty;
			((UltraGridBase)CmbRefDocNo).DataSource = null;
			((Control)(object)CmbRefDocNo).Text = string.Empty;
			((UltraGridBase)CmbRefDocInvoiceNo).DataSource = null;
			((Control)(object)CmbRefDocInvoiceNo).Text = string.Empty;
			DocumentNoFill();
			table.Rows.Clear();
			grd.DataSource = table;
			updateDetailIndex = -1;
			if (MultiBranchFeature)
			{
				ToggleBranchControls();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ResetDetail()
	{
		try
		{
			updateDetailIndex = -1;
			cmbItemName.Focus();
			((Control)(object)cmbItemName).Text = string.Empty;
			((Control)(object)CmbWareHouseFrom).Text = string.Empty;
			((UltraGridBase)CmbWareHouseFrom).DataSource = null;
			((Control)(object)cmbWareHouseTo).Text = string.Empty;
			((UltraGridBase)cmbWareHouseTo).DataSource = null;
			((Control)(object)CmbRackFrom).Text = string.Empty;
			((UltraGridBase)CmbRackFrom).DataSource = null;
			((Control)(object)CmbRackTo).Text = string.Empty;
			((UltraGridBase)CmbRackTo).DataSource = null;
			((Control)(object)CmbPackUom).Text = string.Empty;
			txtQty.Text = string.Empty;
			txtRemarksDetail.Text = string.Empty;
			txtAvgRate.Text = string.Empty;
			txtAmount.Text = string.Empty;
			((Control)(object)CmbRefDocumentType).Text = string.Empty;
			((Control)(object)CmbRefDocNo).Text = string.Empty;
			((Control)(object)CmbRefDocInvoiceNo).Text = string.Empty;
			((UltraGridBase)CmbRefDocNo).DataSource = null;
			((Control)(object)CmbRefDocNo).Text = string.Empty;
			((UltraGridBase)CmbRefDocInvoiceNo).DataSource = null;
			((Control)(object)CmbRefDocInvoiceNo).Text = string.Empty;
			btnplus.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmStockTransferStore_FormClosing(object sender, FormClosingEventArgs e)
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

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			if (tabControl1.SelectedIndex == 1)
			{
				FromDateHistory.Focus();
			}
			else
			{
				DocDate.Focus();
			}
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
			FromDateHistory.Value = DateTime.Now;
			ToDateHistory.Value = DateTime.Now;
			txtFromDocNoHistory.Text = string.Empty;
			txtToDocNoHistory.Text = string.Empty;
			grdhistory.ClearStructure();
			GridDetail.ClearStructure();
			drdocdate.Checked = true;
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
			gridhistoryfill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridhistoryfill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.DocumentTypeId = 807;
			obj.CanViewAllRecord = formrights.DoHaveCanViewAllRecordRights;
			if (!obj.CanViewAllRecord)
			{
				obj.EntryUser = UserAccount.ID;
			}
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
			else if (rdapproveddate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					obj.ApprovedDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					obj.ApprovedDateTo = ToDateHistory.Value;
				}
			}
			obj.FromDocNo = Conversion.ToInt((object)txtFromDocNoHistory.Text);
			obj.ToDocNo = Conversion.ToInt((object)txtToDocNoHistory.Text);
			DataTable dthistoryfill = InvStockTransferHeader.FormHistory(obj);
			if (dthistoryfill.Rows.Count > 0)
			{
				DataTable dtGridHistory = new DataTable();
				dtGridHistory.Columns.Add("Id", typeof(int));
				dtGridHistory.Columns.Add("DocDate", typeof(DateTime));
				dtGridHistory.Columns.Add("DocNo", typeof(int));
				dtGridHistory.Columns.Add("TransType");
				dtGridHistory.Columns.Add("FromBranchName");
				dtGridHistory.Columns.Add("ToBranchName");
				dtGridHistory.Columns.Add("Remarks");
				dtGridHistory.Columns.Add("EntryDate", typeof(DateTime));
				dtGridHistory.Columns.Add("EntryUser");
				dtGridHistory.Columns.Add("ModifyDate", typeof(DateTime));
				dtGridHistory.Columns.Add("ModifyUser");
				dtGridHistory.Columns.Add("NoOfAttachments", typeof(int));
				dtGridHistory.Clear();
				for (int i = 0; i < dthistoryfill.Rows.Count; i++)
				{
					dtGridHistory.Rows.Add(dthistoryfill.Rows[i]["Id"], Conversion.ToDateTime(dthistoryfill.Rows[i]["DocDate"]).ToShortDateString(), dthistoryfill.Rows[i]["DocNo"], dthistoryfill.Rows[i]["TransferType"], dthistoryfill.Rows[i]["FromBranchName"], dthistoryfill.Rows[i]["ToBranchName"], dthistoryfill.Rows[i]["RemarksHeader"], Conversion.ToDateTime(dthistoryfill.Rows[i]["EntryDate"]).ToShortDateString(), dthistoryfill.Rows[i]["EntryUserName"], Conversion.ToDateTime(dthistoryfill.Rows[i]["ModifyDate"]).ToShortDateString(), dthistoryfill.Rows[i]["ModifyUserName"], dthistoryfill.Rows[i]["NoOfAttachments"]);
				}
				grdhistory.DataSource = dtGridHistory;
				grdhistory.RetrieveStructure();
				HistoryGridSettings();
			}
			else
			{
				grdhistory.ClearStructure();
				GridDetail.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryGridSettings()
	{
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdhistory.RootTable.Columns["Id"].Visible = false;
			grdhistory.RootTable.Columns["FromBranchName"].Visible = MultiBranchFeature;
			grdhistory.RootTable.Columns["ToBranchName"].Visible = MultiBranchFeature;
			grdhistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdhistory.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yy hh:mm tt";
			grdhistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yy hh:mm tt";
			CommonServices.GridAutoAdjustmentNew(grdhistory);
			grdhistory.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grdhistory.RootTable.Columns)
			{
				item.HeaderAlignment = (TextAlignment)2;
			}
			grdhistory.RootTable.Columns.Add("Edit");
			grdhistory.RootTable.Columns["Edit"].Caption = "Edit";
			grdhistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdhistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdhistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdhistory.RootTable.Columns["Edit"].Width = 40;
			grdhistory.RootTable.Columns["Edit"].Position = 0;
			grdhistory.RootTable.Columns.Add("Print");
			grdhistory.RootTable.Columns["Print"].Caption = "Print";
			grdhistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdhistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			grdhistory.RootTable.Columns["Print"].ButtonText = "Print";
			grdhistory.RootTable.Columns["Print"].Width = 40;
			grdhistory.RootTable.Columns["Print"].Position = 1;
			grdhistory.FrozenColumns = 2;
			grdhistory.ColumnAutoResize = false;
			ctrlGrdBar3_Load(null, null);
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
			GridEXRow item = grdhistory.CurrentRow;
			if (item != null && (int)item.RowType == 0 && e.Column.Key == "NoOfAttachments")
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(item.Cells["Id"].Value), 807);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdhistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				int CurrId = Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
				if (e.Column.Key == "Edit")
				{
					Reset();
					ReadById(CurrId);
				}
				if (e.Column.Key == "Print")
				{
					GeneratePrint(CurrId);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_DoubleClick_1(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdhistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				RecId = Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
				ReadById(RecId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow row = grdhistory.CurrentRow;
			if (row != null && (int)row.RowType == 0 && row.Cells.Count > 0)
			{
				GetDetailByHeaderId(Conversion.ToInt(row.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GetDetailByHeaderId(int Id)
	{
		//IL_03ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c1: Expected O, but got Unknown
		try
		{
			InvStockTransferHeader obj = InvStockTransferHeader.GetByID(Id);
			if (obj == null || obj.InvStockTransferDetail.Count <= 0)
			{
				return;
			}
			DataTable dt = new DataTable();
			dt.Columns.Add("ItemId");
			dt.Columns.Add("ItemName");
			dt.Columns.Add("WarehouseFrom");
			dt.Columns.Add("RackFrom");
			dt.Columns.Add("WarehouseTo");
			dt.Columns.Add("RackTo");
			dt.Columns.Add("ItemCondition");
			dt.Columns.Add("PackUOM");
			dt.Columns.Add("QTY", typeof(double));
			dt.Columns.Add("ItemRate");
			dt.Columns.Add("ItemAmount", typeof(double));
			dt.Columns.Add("Expense", typeof(double));
			dt.Columns.Add("RefDocumentType", typeof(string));
			dt.Columns.Add("RfDocNo", typeof(string));
			dt.Columns.Add("RefDocInvoiceNo", typeof(string));
			dt.Columns.Add("Remarks");
			for (int i = 0; i < obj.InvStockTransferDetail.Count; i++)
			{
				dt.Rows.Add(obj.InvStockTransferDetail[i].ItemId, obj.InvStockTransferDetail[i].ItemName, obj.InvStockTransferDetail[i].FromWareHouseName, obj.InvStockTransferDetail[i].rackName, obj.InvStockTransferDetail[i].ToWareHouseName, obj.InvStockTransferDetail[i].TorackName, obj.InvStockTransferDetail[i].ItemCondition, obj.InvStockTransferDetail[i].PackUom, obj.InvStockTransferDetail[i].Qty, obj.InvStockTransferDetail[i].ItemRate, obj.InvStockTransferDetail[i].ItemAmount, obj.InvStockTransferDetail[i].ExpenseAmount, obj.InvStockTransferDetail[i].RefDocumentType, obj.InvStockTransferDetail[i].RefDocNo, obj.InvStockTransferDetail[i].RefDocInvoiceNo, obj.InvStockTransferDetail[i].RemarksDetail);
			}
			GridDetail.DataSource = dt;
			GridDetail.RetrieveStructure();
			GridDetail.RootTable.Columns["ItemId"].Visible = false;
			GridDetail.RootTable.Columns["ItemRate"].Visible = StockTransferFinancialEffectIsActive;
			GridDetail.RootTable.Columns["ItemAmount"].Visible = StockTransferFinancialEffectIsActive;
			CommonServices.GridAutoAdjustmentNew(GridDetail);
			GridDetail.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)GridDetail.RootTable.Columns)
			{
				GridEXColumn column = item;
				column.HeaderAlignment = (TextAlignment)2;
				if (column.Type == typeof(double) || column.Type == typeof(decimal))
				{
					ConfigureNumericalColumn(column);
				}
			}
			ctrlGrdBar6_Load(null, null);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grd, table);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar4_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdOtherCharges).Name);
			if (fs != null)
			{
				grdOtherCharges.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlGrdBar3_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdhistory).Name);
			if (fs != null)
			{
				grdhistory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlGrdBar6_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)GridDetail).Name);
			if (fs != null)
			{
				GridDetail.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void btnprint_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId > 0)
			{
				GeneratePrint(RecId);
				return;
			}
			throw new Exception("No Record Selected");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GeneratePrint(int Id)
	{
		try
		{
			CommonServices.StockTransferPackingMaterialAndStore_Slip415(Id);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AvailableStockGetByItem()
	{
		try
		{
			DateTime Date = DocDate.Value;
			int itemId = Conversion.ToInt(cmbItemName.Value);
			int WarehouseId = Conversion.ToInt(CmbWareHouseFrom.Value);
			int rackId = Conversion.ToInt(CmbRackFrom.Value);
			int ItemConditionId = Conversion.ToInt(CmbItemCondition.Value);
			DataTable dtStock = CommonServices.GetAvgRateQtyAndStockInHand(itemId, Date, ItemConditionId, RecId, (RecId > 0) ? 807 : 0, WarehouseId, rackId);
			double Stock = ((dtStock.Rows.Count > 0) ? Math.Round(Conversion.ToDouble(dtStock.Rows[0]["QtyInHand"]), 2) : 0.0);
			if (Stock > 0.0)
			{
				txtAvailableStock.Text = Stock.ToString("#,##0.###");
			}
			else
			{
				txtAvailableStock.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetAvgRate()
	{
		try
		{
			DataTable dt = CommonServices.GetAvgRateQtyAndStockInHand(Conversion.ToInt(cmbItemName.Value), Conversion.ToDateTime((object)DocDate.Value), Conversion.ToInt(CmbItemCondition.Value), RecId, (RecId > 0) ? 807 : 0);
			double rate = ((dt.Rows.Count > 0) ? Math.Round(Conversion.ToDouble(dt.Rows[0]["AvgRate"]), 3) : 0.0);
			if (rate > 0.0)
			{
				txtAvgRate.Text = Conversion.ToString((object)rate);
			}
			else
			{
				txtAvgRate.Text = 0.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AvgAmountCalculation()
	{
		try
		{
			if (Conversion.ToDouble((object)txtQty.Text) > 0.0 && Conversion.ToDouble((object)txtAvgRate.Text) > 0.0)
			{
				double rate = Conversion.ToDouble((object)txtAvgRate.Text);
				double Amount = Conversion.ToDouble((object)txtQty.Text) * rate;
				txtAmount.Text = Amount.ToString(clsGlobalVariables.stringFormatsingle);
			}
			else
			{
				txtAmount.Text = 0.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemAmountCalculation()
	{
		try
		{
			if (grd.GetRows().Length != 0)
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow item in rows)
				{
					item.BeginEdit();
					double NetWeight = Conversion.ToDouble(item.Cells["Qty"].Value);
					double RateUOM = Conversion.ToDouble(item.Cells["RateUOM"].Value);
					double ItemRate = Conversion.ToDouble(item.Cells["ItemRate"].Value);
					double ItemAmount = NetWeight / RateUOM * ItemRate;
					if (NetWeight > 0.0 && RateUOM > 0.0 && ItemRate > 0.0)
					{
						item.Cells["ItemAmount"].Value = ItemAmount.ToString();
					}
					else
					{
						item.Cells["ItemAmount"].Value = 0;
					}
					item.BeginEdit();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		grd.UpdateData();
	}

	private void CmbWareHouseFrom_TextChanged(object sender, EventArgs e)
	{
		try
		{
			AvailableStockGetByItem();
			BindRacks(Conversion.ToInt(cmbItemName.Value), Conversion.ToInt(CmbWareHouseFrom.Value), isFrom: true);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbWareHouseTo_Leave(object sender, EventArgs e)
	{
		try
		{
			BindRacks(Conversion.ToInt(cmbItemName.Value), Conversion.ToInt(cmbWareHouseTo.Value), isFrom: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbRackFrom_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			int rackId = Conversion.ToInt(CmbRackFrom.Value);
			int currentWarehouseId = Conversion.ToInt(CmbWareHouseFrom.Value);
			RacksWithWarehouseAndItem rack = (MultiBranchFeature ? WarehouseAndRackFromBranchList : clsGlobalVariables.racksWithWarehouseAndItems).FirstOrDefault((RacksWithWarehouseAndItem r) => r.Id == rackId);
			if (rack == null)
			{
				return;
			}
			if (currentWarehouseId == 0 || rack.WarehouseId != currentWarehouseId)
			{
				DataTable dtW = (DataTable)((UltraGridBase)CmbWareHouseFrom).DataSource;
				if (dtW != null && dtW.Rows.Count > 0)
				{
					CmbWareHouseFrom.Value = rack.WarehouseId;
				}
			}
			AvailableStockGetByItem();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbRackTo_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			int rackId = Conversion.ToInt(CmbRackTo.Value);
			int currentWarehouseId = Conversion.ToInt(cmbWareHouseTo.Value);
			RacksWithWarehouseAndItem rack = (MultiBranchFeature ? WarehouseAndRackToBranchList : clsGlobalVariables.racksWithWarehouseAndItems).FirstOrDefault((RacksWithWarehouseAndItem r) => r.Id == rackId);
			if (rack != null && (currentWarehouseId == 0 || rack.WarehouseId != currentWarehouseId))
			{
				DataTable dtW = (DataTable)((UltraGridBase)cmbWareHouseTo).DataSource;
				if (dtW != null && dtW.Rows.Count > 0)
				{
					cmbWareHouseTo.Value = rack.WarehouseId;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbItemName_TextChanged(object sender, EventArgs e)
	{
		try
		{
			GetAvgRate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtAvgRate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			AvgAmountCalculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtQty_TextChanged(object sender, EventArgs e)
	{
		try
		{
			AvgAmountCalculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbPackUom_TextChanged(object sender, EventArgs e)
	{
	}

	private void cmbItemName_Leave(object sender, EventArgs e)
	{
		try
		{
			int itemId = Conversion.ToInt(cmbItemName.Value);
			if (BindWarehouses(itemId))
			{
				BindRacks(itemId, Conversion.ToInt(CmbWareHouseFrom.Value), isFrom: true);
				BindRacks(itemId, Conversion.ToInt(cmbWareHouseTo.Value), isFrom: false);
				bindRateUomAndItemPackUom();
				AvailableStockGetByItem();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool BindWarehouses(int itemId)
	{
		try
		{
			((UltraGridBase)CmbWareHouseFrom).DataSource = null;
			((UltraGridBase)cmbWareHouseTo).DataSource = null;
			if (MultiBranchFeature)
			{
				if (((UltraGridBase)CmbFromBranch).ActiveRow == null || Conversion.ToInt(CmbFromBranch.Value) == 0)
				{
					MessageBox.Show("Please select From Branch first.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					CmbFromBranch.Focus();
					return false;
				}
				if (((UltraGridBase)CmbToBranch).ActiveRow == null || Conversion.ToInt(CmbToBranch.Value) == 0)
				{
					MessageBox.Show("Please select To Branch first.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					CmbToBranch.Focus();
					return false;
				}
				BindWarehouseDropdown(CmbWareHouseFrom, WarehouseAndRackFromBranchList, itemId);
				BindWarehouseDropdown(cmbWareHouseTo, WarehouseAndRackToBranchList, itemId);
			}
			else
			{
				BindWarehouseDropdown(CmbWareHouseFrom, clsGlobalVariables.racksWithWarehouseAndItems, itemId);
				BindWarehouseDropdown(cmbWareHouseTo, clsGlobalVariables.racksWithWarehouseAndItems, itemId);
			}
			return true;
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error loading WareHouse: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			return false;
		}
	}

	private void BindWarehouseDropdown(UltraCombo combo, List<RacksWithWarehouseAndItem> source, int itemId)
	{
		List<RacksWithWarehouseAndItem> list = (from x in source
			where x.ItemId == itemId
			group x by x.WarehouseId into g
			select g.First()).ToList();
		DataTable dt = new DataTable();
		dt.Columns.Add("Id", typeof(int));
		dt.Columns.Add("Warehouse", typeof(string));
		dt.BeginLoadData();
		foreach (RacksWithWarehouseAndItem w in list)
		{
			dt.Rows.Add(w.WarehouseId, w.WareHouseName);
		}
		dt.EndLoadData();
		InfragisticsHelper.BindAndRetainSelection(combo, dt, "Id", "Warehouse", ((Control)(object)combo).Tag?.ToString(), AllColumns: true, Conversion.ToInt(combo.Value), insertDefaultRow: true);
		if (Conversion.ToInt(combo.Value) != 0)
		{
			return;
		}
		if (dt.Rows.Count == 1)
		{
			combo.Value = dt.Rows[0]["Id"];
		}
		else if (dt.Rows.Count > 1)
		{
			int parentCategoryId = 0;
			UltraCombo obj = cmbItemName;
			if (((obj != null) ? ((UltraDropDownBase)obj).SelectedRow : null) != null)
			{
				parentCategoryId = Conversion.ToInt(((UltraDropDownBase)cmbItemName).SelectedRow.Cells[3].Value);
			}
			int configWarehouseId = 0;
			switch (parentCategoryId)
			{
			case 7:
				configWarehouseId = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("PackingMaterialDefaultWarehouse"));
				break;
			case 8:
				configWarehouseId = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("DefaultWarehouseForStoreFlow"));
				break;
			}
			if (configWarehouseId != 0 && dt.AsEnumerable().Any((DataRow row) => Conversion.ToInt(row["Id"]) == configWarehouseId))
			{
				combo.Value = configWarehouseId;
			}
		}
	}

	private void BindRacks(int itemId, int warehouseId, bool isFrom)
	{
		try
		{
			List<RacksWithWarehouseAndItem> list = (from x in (!MultiBranchFeature) ? clsGlobalVariables.racksWithWarehouseAndItems : (isFrom ? WarehouseAndRackFromBranchList : WarehouseAndRackToBranchList)
				where x.ItemId == itemId && (warehouseId == 0 || x.WarehouseId == warehouseId)
				group x by x.Id into g
				select g.First()).ToList();
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("RackName", typeof(string));
			dt.Columns.Add("WarehouseId", typeof(int));
			dt.Columns.Add("WarehouseName", typeof(string));
			dt.Columns.Add("BaseRackId", typeof(int));
			dt.BeginLoadData();
			foreach (RacksWithWarehouseAndItem r in list)
			{
				dt.Rows.Add(r.Id, r.RackName, r.WarehouseId, r.WareHouseName, r.BaseRackId);
			}
			dt.EndLoadData();
			UltraCombo combo = (isFrom ? CmbRackFrom : CmbRackTo);
			InfragisticsHelper.BindAndRetainSelection(combo, dt, "Id", "RackName", "Rack", AllColumns: true, Conversion.ToInt(combo.Value), insertDefaultRow: true);
			if (dt.Rows.Count > 0)
			{
				((UltraGridBase)combo).DisplayLayout.Bands[0].Columns["WarehouseId"].Hidden = true;
				((UltraGridBase)combo).DisplayLayout.Bands[0].Columns["BaseRackId"].Hidden = true;
			}
			if (Conversion.ToInt(combo.Value) != 0)
			{
				return;
			}
			if (dt.Rows.Count == 1)
			{
				combo.Value = dt.Rows[0]["Id"];
			}
			else if (dt.Rows.Count > 1)
			{
				DataRow baseRackRow = dt.AsEnumerable().FirstOrDefault((DataRow row) => Conversion.ToInt(row["BaseRackId"]) > 0);
				if (baseRackRow != null)
				{
					combo.Value = baseRackRow["Id"];
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error loading Rack: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CmbItemCondition_Leave(object sender, EventArgs e)
	{
		try
		{
			AvailableStockGetByItem();
			GetAvgRate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAttachment_Click(object sender, EventArgs e)
	{
		try
		{
			AT.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtqty_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtAvailableStock_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtAvgRate_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtWorkingReportNo_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextNumberFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtFromDocNoHistory_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextNumberFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtToDocNoHistory_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextNumberFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PurchsaeOrder_KeyDown(object sender, KeyEventArgs e)
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
					DocDate.Focus();
				}
				else
				{
					tabControl1.SelectedIndex = 1;
					FromDateHistory.Focus();
				}
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (tabControl1.SelectedIndex == 0)
			{
				if (e.Control && e.KeyCode == Keys.S && btnsave.Visible && btnsave.Enabled)
				{
					btnsave_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnnew_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.U && btnupdate.Visible && btnupdate.Enabled)
				{
					btnupdate_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					DocDate.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Up)
				{
					cmbWareHouseTo.Focus();
				}
			}
			else
			{
				if (e.Control && e.KeyCode == Keys.S)
				{
					btnshow_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnNewHistory_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F5)
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

	private void MakeShortCutKeys()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("KeyCombination");
			dt.Columns.Add("Description");
			dt.Rows.Add("Ctrl+S", "For Save");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Ctrl+F5", "For Focus on Doc Date");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On WareHouse To Combo in Detail Box");
			dt.Rows.Add("Ctrl+Enter", "For Update Record When Focus On Any Grid ");
			dt.Rows.Add("Ctrl+Space", "To Call Function's On Button Or Link When Focus On Any Grid ");
			ShortCutKeyPopUp shortCutKeyPopUp = new ShortCutKeyPopUp(dt);
			shortCutKeyPopUp.StartPosition = FormStartPosition.CenterScreen;
			shortCutKeyPopUp.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grd.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grd.CurrentColumn.Key == "Delete")
				{
					DeleteOtherDetailRow(r);
				}
				if (grd.CurrentColumn.Key == "Edit")
				{
					grd_DoubleClick(null, null);
				}
				grd.UpdateData();
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				DeleteDetailRow(r);
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				grd_DoubleClick(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOtherCharges_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdOtherCharges.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdOtherCharges.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdOtherCharges.CurrentColumn.Key == "Delete")
				{
					DeleteOtherDetailRow(r);
				}
				if (grdOtherCharges.CurrentColumn.Key == "Add")
				{
					AddRowInExpenseGrid();
				}
				grdOtherCharges.UpdateData();
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				DeleteOtherDetailRow(r);
			}
			if (e.Control && e.KeyCode == Keys.D)
			{
				AddRowInExpenseGrid();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdhistory.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdhistory.CurrentColumn == null)
			{
				return;
			}
			int CurrId = Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdhistory.CurrentColumn.Key == "Edit" && formrights.DoHaveUpdateRights)
				{
					ReadById(CurrId);
				}
				if (grdhistory.CurrentColumn.Key == "Print" && formrights.DoHavePrintRights)
				{
					CommonServices.StockTransferSlip406(CurrId);
				}
				if (grdhistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByScreenName(CurrId, base.Name);
				}
			}
			if (e.Control && e.KeyCode == Keys.Return && formrights.DoHaveUpdateRights)
			{
				ReadById(CurrId);
			}
			if (e.Control && e.KeyCode == Keys.P && formrights.DoHavePrintRights)
			{
				CommonServices.StockTransferSlip406(CurrId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnDelete_Click(object sender, EventArgs e)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Expected O, but got Unknown
		try
		{
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					InvPurchaseInvoice.RemoveByID(new InvPurchaseInvoice
					{
						OrganizationId = UserAccount.OrganizationId,
						CompanyId = UserAccount.CompanyId,
						DocumentTypeId = 807,
						Id = RecId,
						EntryUser = UserAccount.ID
					});
					MessageBox.Show("Delete Record Successfully");
					Reset();
				}
				return;
			}
			throw new Exception("RecordId Not Found.....");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AvgRateUpdateOnDocDateChange()
	{
		try
		{
			if (grd.RowCount <= 0)
			{
				return;
			}
			GridEXRow[] dataRows = grd.GetDataRows();
			foreach (GridEXRow r in dataRows)
			{
				r.BeginEdit();
				int ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
				int ItemConditionId = Conversion.ToInt(r.Cells["ItemConditionId"].Value);
				double Qty = Conversion.ToDouble(r.Cells["QTY"].Value);
				double rate = 0.0;
				DataTable dt = CommonServices.GetAvgRateQtyAndStockInHand(ItemId, DocDate.Value, ItemConditionId, RecId, (RecId > 0) ? 807 : 0);
				if (dt.Rows.Count > 0)
				{
					rate = Math.Round(Conversion.ToDouble(dt.Rows[0]["AvgRate"]), 3);
				}
				if (rate > 0.0)
				{
					r.Cells["ItemRate"].Value = rate;
					double Amount = Qty * rate;
					r.Cells["ItemAmount"].Value = Amount;
				}
				else
				{
					r.Cells["ItemRate"].Value = 0;
					r.Cells["ItemAmount"].Value = 0;
				}
				r.EndEdit();
			}
			grd.UpdateData();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void DocDate_Leave(object sender, EventArgs e)
	{
		try
		{
			if (StockTransferFinancialEffectIsActive || CGSEntryAllow)
			{
				AvgRateUpdateOnDocDateChange();
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
		//IL_03ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f3: Expected O, but got Unknown
		//IL_03f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fa: Expected O, but got Unknown
		//IL_03fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0401: Expected O, but got Unknown
		//IL_0401: Unknown result type (might be due to invalid IL or missing references)
		//IL_0408: Expected O, but got Unknown
		//IL_0461: Unknown result type (might be due to invalid IL or missing references)
		//IL_046b: Expected O, but got Unknown
		//IL_04a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ad: Expected O, but got Unknown
		//IL_04e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ef: Expected O, but got Unknown
		//IL_04f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fa: Expected O, but got Unknown
		//IL_053d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0547: Expected O, but got Unknown
		//IL_0553: Unknown result type (might be due to invalid IL or missing references)
		//IL_055d: Expected O, but got Unknown
		//IL_0574: Unknown result type (might be due to invalid IL or missing references)
		//IL_057e: Expected O, but got Unknown
		//IL_057f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0589: Expected O, but got Unknown
		//IL_05a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05aa: Expected O, but got Unknown
		//IL_05cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d6: Expected O, but got Unknown
		//IL_05d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e1: Expected O, but got Unknown
		//IL_063a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0644: Expected O, but got Unknown
		//IL_0671: Unknown result type (might be due to invalid IL or missing references)
		//IL_067b: Expected O, but got Unknown
		//IL_0687: Unknown result type (might be due to invalid IL or missing references)
		//IL_0691: Expected O, but got Unknown
		//IL_0758: Unknown result type (might be due to invalid IL or missing references)
		//IL_0762: Expected O, but got Unknown
		//IL_0784: Unknown result type (might be due to invalid IL or missing references)
		//IL_078e: Expected O, but got Unknown
		//IL_101a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1024: Expected O, but got Unknown
		//IL_1418: Unknown result type (might be due to invalid IL or missing references)
		//IL_1422: Expected O, but got Unknown
		//IL_142f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1439: Expected O, but got Unknown
		//IL_66d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_66dc: Expected O, but got Unknown
		//IL_6700: Unknown result type (might be due to invalid IL or missing references)
		//IL_670a: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdOtherCharges_DesignTimeLayout = new GridEXLayout();
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
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		GridEXLayout grdhistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GridDetail_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel6 = new System.Windows.Forms.Panel();
		this.panel3 = new System.Windows.Forms.Panel();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.panel5 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.panel11 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar5 = new grid.User_Conrtols.CtrlGrdBar();
		this.label23 = new System.Windows.Forms.Label();
		this.tabPage4 = new System.Windows.Forms.TabPage();
		this.panel14 = new System.Windows.Forms.Panel();
		this.grdOtherCharges = new GridEX();
		this.panel13 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar4 = new grid.User_Conrtols.CtrlGrdBar();
		this.label22 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.CmbItemCondition = new UltraCombo();
		this.CmbRackTo = new UltraCombo();
		this.label38 = new System.Windows.Forms.Label();
		this.txtAvailableStock = new System.Windows.Forms.TextBox();
		this.txtQty = new System.Windows.Forms.TextBox();
		this.label14 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label36 = new System.Windows.Forms.Label();
		this.CmbRackFrom = new UltraCombo();
		this.label13 = new System.Windows.Forms.Label();
		this.CmbPackUom = new UltraCombo();
		this.txtAvgRate = new System.Windows.Forms.TextBox();
		this.lblAvgRate = new System.Windows.Forms.Label();
		this.CmbWareHouseFrom = new UltraCombo();
		this.cmbWareHouseTo = new UltraCombo();
		this.label3 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.CmbRefDocInvoiceNo = new UltraCombo();
		this.lblAmount = new System.Windows.Forms.Label();
		this.label26 = new System.Windows.Forms.Label();
		this.txtAmount = new System.Windows.Forms.TextBox();
		this.CmbRefDocumentType = new UltraCombo();
		this.CmbRefDocNo = new UltraCombo();
		this.label28 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.btnplus = new System.Windows.Forms.Button();
		this.label2 = new System.Windows.Forms.Label();
		this.btnCancelUpdateDetial = new System.Windows.Forms.Button();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.txtRemarksDetail = new System.Windows.Forms.TextBox();
		this.label12 = new System.Windows.Forms.Label();
		this.cmbItemName = new UltraCombo();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.DocDate = new System.Windows.Forms.DateTimePicker();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.lblToBranch = new System.Windows.Forms.Label();
		this.CmbToBranch = new UltraCombo();
		this.lblFromBranch = new System.Windows.Forms.Label();
		this.CmbFromBranch = new UltraCombo();
		this.label5 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.label15 = new System.Windows.Forms.Label();
		this.txtRemarksHead = new System.Windows.Forms.TextBox();
		this.panel2 = new System.Windows.Forms.Panel();
		this.label24 = new System.Windows.Forms.Label();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnprint = new System.Windows.Forms.ToolStripButton();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.BtnShortCutkeys = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.ChkBoxPrintPreview = new System.Windows.Forms.CheckBox();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.grdhistory = new GridEX();
		this.panel9 = new System.Windows.Forms.Panel();
		this.label6 = new System.Windows.Forms.Label();
		this.ctrlGrdBar6 = new grid.User_Conrtols.CtrlGrdBar();
		this.GridDetail = new GridEX();
		this.panel7 = new System.Windows.Forms.Panel();
		this.label1 = new System.Windows.Forms.Label();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel12 = new System.Windows.Forms.Panel();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.rdapproveddate = new System.Windows.Forms.RadioButton();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.btnshow = new System.Windows.Forms.Button();
		this.label27 = new System.Windows.Forms.Label();
		this.label43 = new System.Windows.Forms.Label();
		this.label62 = new System.Windows.Forms.Label();
		this.label63 = new System.Windows.Forms.Label();
		this.txtToDocNoHistory = new System.Windows.Forms.TextBox();
		this.txtFromDocNoHistory = new System.Windows.Forms.TextBox();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.panel8 = new System.Windows.Forms.Panel();
		this.label25 = new System.Windows.Forms.Label();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel3.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.tabPage3.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel11.SuspendLayout();
		this.tabPage4.SuspendLayout();
		this.panel14.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdOtherCharges).BeginInit();
		this.panel13.SuspendLayout();
		this.panel4.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbItemCondition).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRackTo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRackFrom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWareHouseFrom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbWareHouseTo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRefDocInvoiceNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRefDocumentType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRefDocNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemName).BeginInit();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbToBranch).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbFromBranch).BeginInit();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdhistory).BeginInit();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridDetail).BeginInit();
		this.panel7.SuspendLayout();
		this.panel12.SuspendLayout();
		this.groupBox3.SuspendLayout();
		this.panel8.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1099, 626);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1099, 626);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel6);
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage1.Size = new System.Drawing.Size(1091, 597);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel6.Controls.Add(this.panel3);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(2, 245);
		this.panel6.Margin = new System.Windows.Forms.Padding(2);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1087, 350);
		this.panel6.TabIndex = 4;
		this.panel3.Controls.Add(this.tabControl2);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1087, 350);
		this.panel3.TabIndex = 1;
		this.tabControl2.Controls.Add(this.tabPage3);
		this.tabControl2.Controls.Add(this.tabPage4);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl2.Location = new System.Drawing.Point(0, 0);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(1087, 350);
		this.tabControl2.TabIndex = 0;
		this.tabPage3.Controls.Add(this.panel5);
		this.tabPage3.Controls.Add(this.panel11);
		this.tabPage3.Font = new System.Drawing.Font("Segoe UI", 10.75f);
		this.tabPage3.Location = new System.Drawing.Point(4, 25);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage3.Size = new System.Drawing.Size(1079, 321);
		this.tabPage3.TabIndex = 0;
		this.tabPage3.Text = "Detail";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(3, 29);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1073, 289);
		this.panel5.TabIndex = 54;
		this.grd.AllowDelete = (InheritableBoolean)1;
		this.grd.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		this.grd.ColumnAutoResize = true;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1073, 289);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grd_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		this.panel11.BackColor = System.Drawing.Color.Teal;
		this.panel11.Controls.Add(this.ctrlGrdBar5);
		this.panel11.Controls.Add(this.label23);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(3, 3);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1073, 26);
		this.panel11.TabIndex = 53;
		this.ctrlGrdBar5.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar5.FormName = this;
		this.ctrlGrdBar5.Location = new System.Drawing.Point(1029, 1);
		this.ctrlGrdBar5.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
		this.ctrlGrdBar5.MyGrid = this.grd;
		this.ctrlGrdBar5.Name = "ctrlGrdBar5";
		this.ctrlGrdBar5.Size = new System.Drawing.Size(40, 24);
		this.ctrlGrdBar5.TabIndex = 25;
		this.ctrlGrdBar5.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(5, 5);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(46, 17);
		this.label23.TabIndex = 35;
		this.label23.Text = "Detail";
		this.tabPage4.Controls.Add(this.panel14);
		this.tabPage4.Controls.Add(this.panel13);
		this.tabPage4.Font = new System.Drawing.Font("Segoe UI", 10.75f);
		this.tabPage4.Location = new System.Drawing.Point(4, 25);
		this.tabPage4.Name = "tabPage4";
		this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage4.Size = new System.Drawing.Size(1079, 321);
		this.tabPage4.TabIndex = 1;
		this.tabPage4.Text = "Other Charges";
		this.tabPage4.UseVisualStyleBackColor = true;
		this.panel14.Controls.Add((System.Windows.Forms.Control)(object)this.grdOtherCharges);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel14.Location = new System.Drawing.Point(3, 29);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(1073, 289);
		this.panel14.TabIndex = 54;
		((ScrollableBase)this.grdOtherCharges).BackColor = System.Drawing.Color.White;
		this.grdOtherCharges.ColumnAutoResize = true;
		grdOtherCharges_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdOtherCharges.DesignTimeLayout = grdOtherCharges_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdOtherCharges).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdOtherCharges).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdOtherCharges.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdOtherCharges).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdOtherCharges).Name = "grdOtherCharges";
		((ScrollableBase)this.grdOtherCharges).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdOtherCharges).Size = new System.Drawing.Size(1073, 289);
		((System.Windows.Forms.Control)(object)this.grdOtherCharges).TabIndex = 0;
		this.grdOtherCharges.TotalRow = (InheritableBoolean)1;
		this.grdOtherCharges.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdOtherCharges).VisualStyle = (VisualStyle)4;
		this.grdOtherCharges.CellUpdated += new ColumnActionEventHandler(grdOtherCharges_CellUpdated);
		this.grdOtherCharges.ColumnButtonClick += new ColumnActionEventHandler(grdOtherCharges_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdOtherCharges).KeyDown += new System.Windows.Forms.KeyEventHandler(grdOtherCharges_KeyDown);
		this.panel13.BackColor = System.Drawing.Color.Teal;
		this.panel13.Controls.Add(this.ctrlGrdBar4);
		this.panel13.Controls.Add(this.label22);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel13.Location = new System.Drawing.Point(3, 3);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1073, 26);
		this.panel13.TabIndex = 53;
		this.ctrlGrdBar4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar4.FormName = this;
		this.ctrlGrdBar4.Location = new System.Drawing.Point(1025, 0);
		this.ctrlGrdBar4.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
		this.ctrlGrdBar4.MyGrid = this.grdOtherCharges;
		this.ctrlGrdBar4.Name = "ctrlGrdBar4";
		this.ctrlGrdBar4.Size = new System.Drawing.Size(45, 25);
		this.ctrlGrdBar4.TabIndex = 24;
		this.ctrlGrdBar4.Load += new System.EventHandler(ctrlGrdBar4_Load);
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label22.ForeColor = System.Drawing.Color.White;
		this.label22.Location = new System.Drawing.Point(5, 5);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(151, 17);
		this.label22.TabIndex = 35;
		this.label22.Text = "Other Charges Detail";
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.groupBox2);
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(2, 53);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1087, 192);
		this.panel4.TabIndex = 0;
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemCondition);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbRackTo);
		this.groupBox2.Controls.Add(this.label38);
		this.groupBox2.Controls.Add(this.txtAvailableStock);
		this.groupBox2.Controls.Add(this.txtQty);
		this.groupBox2.Controls.Add(this.label14);
		this.groupBox2.Controls.Add(this.label7);
		this.groupBox2.Controls.Add(this.label36);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbRackFrom);
		this.groupBox2.Controls.Add(this.label13);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPackUom);
		this.groupBox2.Controls.Add(this.txtAvgRate);
		this.groupBox2.Controls.Add(this.lblAvgRate);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWareHouseFrom);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbWareHouseTo);
		this.groupBox2.Controls.Add(this.label3);
		this.groupBox2.Controls.Add(this.label11);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbRefDocInvoiceNo);
		this.groupBox2.Controls.Add(this.lblAmount);
		this.groupBox2.Controls.Add(this.label26);
		this.groupBox2.Controls.Add(this.txtAmount);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbRefDocumentType);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbRefDocNo);
		this.groupBox2.Controls.Add(this.label28);
		this.groupBox2.Controls.Add(this.label4);
		this.groupBox2.Controls.Add(this.btnplus);
		this.groupBox2.Controls.Add(this.label2);
		this.groupBox2.Controls.Add(this.btnCancelUpdateDetial);
		this.groupBox2.Controls.Add(this.btnUpdateDetail);
		this.groupBox2.Controls.Add(this.txtRemarksDetail);
		this.groupBox2.Controls.Add(this.label12);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemName);
		this.groupBox2.Font = new System.Drawing.Font("Verdana", 8f, System.Drawing.FontStyle.Bold);
		this.groupBox2.Location = new System.Drawing.Point(3, 75);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(1060, 112);
		this.groupBox2.TabIndex = 1;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Detail";
		this.CmbItemCondition.AllowNull = (DefaultableBoolean)1;
		this.CmbItemCondition.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemCondition.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemCondition.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemCondition).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemCondition).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemCondition).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemCondition.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemCondition).Location = new System.Drawing.Point(756, 33);
		((System.Windows.Forms.Control)(object)this.CmbItemCondition).Name = "CmbItemCondition";
		((System.Windows.Forms.Control)(object)this.CmbItemCondition).Size = new System.Drawing.Size(133, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemCondition).TabIndex = 5;
		((System.Windows.Forms.Control)(object)this.CmbItemCondition).Leave += new System.EventHandler(CmbItemCondition_Leave);
		this.CmbRackTo.AllowNull = (DefaultableBoolean)1;
		this.CmbRackTo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbRackTo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbRackTo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbRackTo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.CmbRackTo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbRackTo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance71).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance71).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbRackTo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance71;
		((AppearanceBase)appearance72).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRackTo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance72;
		((SpecialBoxBase)((UltraGridBase)this.CmbRackTo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance73).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance73).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance73).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRackTo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.CmbRackTo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbRackTo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance74).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbRackTo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance75).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbRackTo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.CmbRackTo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbRackTo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRackTo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance76;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance81).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbRackTo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.CmbRackTo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbRackTo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance82).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance82).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance82).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance82).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRackTo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance82;
		((AppearanceBase)appearance83).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbRackTo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.CmbRackTo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbRackTo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance84).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbRackTo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.CmbRackTo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbRackTo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.CmbRackTo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbRackTo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbRackTo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbRackTo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbRackTo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbRackTo).Location = new System.Drawing.Point(631, 33);
		((System.Windows.Forms.Control)(object)this.CmbRackTo).Name = "CmbRackTo";
		((System.Windows.Forms.Control)(object)this.CmbRackTo).Size = new System.Drawing.Size(124, 26);
		((System.Windows.Forms.Control)(object)this.CmbRackTo).TabIndex = 4;
		this.CmbRackTo.ValueChanged += new System.EventHandler(CmbRackTo_ValueChanged);
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.Location = new System.Drawing.Point(975, 15);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(70, 15);
		this.label38.TabIndex = 13;
		this.label38.Text = "Qty/Weight";
		this.txtAvailableStock.BackColor = System.Drawing.Color.White;
		this.txtAvailableStock.BorderStyle = System.Windows.Forms.BorderStyle.None;
		this.txtAvailableStock.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtAvailableStock.Location = new System.Drawing.Point(101, 14);
		this.txtAvailableStock.Name = "txtAvailableStock";
		this.txtAvailableStock.ReadOnly = true;
		this.txtAvailableStock.Size = new System.Drawing.Size(102, 16);
		this.txtAvailableStock.TabIndex = 6;
		this.txtAvailableStock.TabStop = false;
		this.txtAvailableStock.Text = "0";
		this.txtAvailableStock.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtAvailableStock.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtAvailableStock_KeyPress);
		this.txtQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtQty.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtQty.Location = new System.Drawing.Point(975, 35);
		this.txtQty.Name = "txtQty";
		this.txtQty.ShortcutsEnabled = false;
		this.txtQty.Size = new System.Drawing.Size(81, 23);
		this.txtQty.TabIndex = 7;
		this.txtQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtQty.TextChanged += new System.EventHandler(txtQty_TextChanged);
		this.txtQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtqty_KeyPress);
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(756, 15);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(87, 15);
		this.label14.TabIndex = 58;
		this.label14.Text = "Item Condition";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(631, 15);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(48, 15);
		this.label7.TabIndex = 245;
		this.label7.Text = "Rack To";
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(890, 15);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(35, 15);
		this.label36.TabIndex = 8;
		this.label36.Text = "UOM";
		this.CmbRackFrom.AllowNull = (DefaultableBoolean)1;
		this.CmbRackFrom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbRackFrom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbRackFrom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance104).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbRackFrom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.CmbRackFrom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbRackFrom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance105).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance105).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbRackFrom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance105;
		((AppearanceBase)appearance106).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRackFrom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance106;
		((SpecialBoxBase)((UltraGridBase)this.CmbRackFrom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance107).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance107).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance107).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRackFrom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.CmbRackFrom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbRackFrom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance108).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbRackFrom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance108;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance109).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbRackFrom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.CmbRackFrom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbRackFrom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRackFrom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance110;
		((AppearanceBase)appearance111).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance111).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbRackFrom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.CmbRackFrom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbRackFrom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance113).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance113).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance113).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRackFrom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance113;
		((AppearanceBase)appearance117).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbRackFrom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.CmbRackFrom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbRackFrom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance118).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbRackFrom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.CmbRackFrom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbRackFrom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.CmbRackFrom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbRackFrom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbRackFrom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbRackFrom).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbRackFrom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbRackFrom).Location = new System.Drawing.Point(355, 33);
		((System.Windows.Forms.Control)(object)this.CmbRackFrom).Name = "CmbRackFrom";
		((System.Windows.Forms.Control)(object)this.CmbRackFrom).Size = new System.Drawing.Size(132, 26);
		((System.Windows.Forms.Control)(object)this.CmbRackFrom).TabIndex = 2;
		this.CmbRackFrom.ValueChanged += new System.EventHandler(CmbRackFrom_ValueChanged);
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(355, 15);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(63, 15);
		this.label13.TabIndex = 243;
		this.label13.Text = "Rack From";
		this.CmbPackUom.AllowNull = (DefaultableBoolean)1;
		this.CmbPackUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance58).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance58).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance58).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance58;
		((AppearanceBase)appearance59).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance59;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance2).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance2).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance2).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance2).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance13;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance60;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance61).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance62).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance62).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance62).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance62).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance62;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbPackUom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Location = new System.Drawing.Point(890, 33);
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Name = "CmbPackUom";
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Size = new System.Drawing.Size(84, 26);
		((System.Windows.Forms.Control)(object)this.CmbPackUom).TabIndex = 6;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).TextChanged += new System.EventHandler(CmbPackUom_TextChanged);
		this.txtAvgRate.BackColor = System.Drawing.Color.White;
		this.txtAvgRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAvgRate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtAvgRate.Location = new System.Drawing.Point(2, 82);
		this.txtAvgRate.Name = "txtAvgRate";
		this.txtAvgRate.ShortcutsEnabled = false;
		this.txtAvgRate.Size = new System.Drawing.Size(88, 23);
		this.txtAvgRate.TabIndex = 8;
		this.txtAvgRate.Text = "0";
		this.txtAvgRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtAvgRate.TextChanged += new System.EventHandler(txtAvgRate_TextChanged);
		this.txtAvgRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtAvgRate_KeyPress);
		this.lblAvgRate.AutoSize = true;
		this.lblAvgRate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblAvgRate.Location = new System.Drawing.Point(2, 63);
		this.lblAvgRate.Name = "lblAvgRate";
		this.lblAvgRate.Size = new System.Drawing.Size(54, 15);
		this.lblAvgRate.TabIndex = 49;
		this.lblAvgRate.Text = "Avg Rate";
		this.CmbWareHouseFrom.AllowNull = (DefaultableBoolean)1;
		this.CmbWareHouseFrom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWareHouseFrom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWareHouseFrom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance57).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance57).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance57).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance57;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance68;
		((SpecialBoxBase)((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance79).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance79).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance90;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance101).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance43).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance43).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance54).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance54).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance54).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance54).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance54;
		((AppearanceBase)appearance112).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance123).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance134;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWareHouseFrom).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbWareHouseFrom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWareHouseFrom).Location = new System.Drawing.Point(207, 33);
		((System.Windows.Forms.Control)(object)this.CmbWareHouseFrom).Name = "CmbWareHouseFrom";
		((System.Windows.Forms.Control)(object)this.CmbWareHouseFrom).Size = new System.Drawing.Size(147, 26);
		((System.Windows.Forms.Control)(object)this.CmbWareHouseFrom).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.CmbWareHouseFrom).Leave += new System.EventHandler(CmbWareHouseFrom_TextChanged);
		this.cmbWareHouseTo.AllowNull = (DefaultableBoolean)1;
		this.cmbWareHouseTo.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbWareHouseTo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbWareHouseTo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance28).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance28).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance28;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance32;
		((SpecialBoxBase)((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance63).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance63).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance63).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance64).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance64;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance65).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance67).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance67).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance69).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance69).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance69;
		((AppearanceBase)appearance94).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance94;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance95).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbWareHouseTo).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbWareHouseTo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbWareHouseTo).Location = new System.Drawing.Point(488, 33);
		((System.Windows.Forms.Control)(object)this.cmbWareHouseTo).Name = "cmbWareHouseTo";
		((System.Windows.Forms.Control)(object)this.cmbWareHouseTo).Size = new System.Drawing.Size(142, 26);
		((System.Windows.Forms.Control)(object)this.cmbWareHouseTo).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.cmbWareHouseTo).Leave += new System.EventHandler(cmbWareHouseTo_Leave);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(207, 15);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(100, 15);
		this.label3.TabIndex = 0;
		this.label3.Text = "WareHouse From";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(488, 15);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(85, 15);
		this.label11.TabIndex = 43;
		this.label11.Text = "WareHouse To";
		this.CmbRefDocInvoiceNo.AllowNull = (DefaultableBoolean)1;
		this.CmbRefDocInvoiceNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbRefDocInvoiceNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbRefDocInvoiceNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance129).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbRefDocInvoiceNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.CmbRefDocInvoiceNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbRefDocInvoiceNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance130).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance130).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbRefDocInvoiceNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance130;
		((AppearanceBase)appearance131).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRefDocInvoiceNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance131;
		((SpecialBoxBase)((UltraGridBase)this.CmbRefDocInvoiceNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance132).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance132).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance132).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRefDocInvoiceNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.CmbRefDocInvoiceNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbRefDocInvoiceNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance133).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbRefDocInvoiceNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance133;
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance135).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbRefDocInvoiceNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.CmbRefDocInvoiceNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbRefDocInvoiceNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRefDocInvoiceNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance136;
		((AppearanceBase)appearance137).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance137).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbRefDocInvoiceNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance137;
		((UltraGridBase)this.CmbRefDocInvoiceNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbRefDocInvoiceNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance138).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance138).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance138).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance138).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRefDocInvoiceNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance138;
		((AppearanceBase)appearance139).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbRefDocInvoiceNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance139;
		((UltraGridBase)this.CmbRefDocInvoiceNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbRefDocInvoiceNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbRefDocInvoiceNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.CmbRefDocInvoiceNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbRefDocInvoiceNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance141;
		((UltraGridBase)this.CmbRefDocInvoiceNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbRefDocInvoiceNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbRefDocInvoiceNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbRefDocInvoiceNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbRefDocInvoiceNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbRefDocInvoiceNo).Location = new System.Drawing.Point(541, 80);
		((System.Windows.Forms.Control)(object)this.CmbRefDocInvoiceNo).Name = "CmbRefDocInvoiceNo";
		((System.Windows.Forms.Control)(object)this.CmbRefDocInvoiceNo).Size = new System.Drawing.Size(170, 26);
		((System.Windows.Forms.Control)(object)this.CmbRefDocInvoiceNo).TabIndex = 12;
		((System.Windows.Forms.Control)(object)this.CmbRefDocInvoiceNo).Leave += new System.EventHandler(CmbRefDocInvoiceNo_Leave);
		this.lblAmount.AutoSize = true;
		this.lblAmount.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblAmount.Location = new System.Drawing.Point(91, 63);
		this.lblAmount.Name = "lblAmount";
		this.lblAmount.Size = new System.Drawing.Size(51, 15);
		this.lblAmount.TabIndex = 51;
		this.lblAmount.Text = "Amount";
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(541, 63);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(110, 15);
		this.label26.TabIndex = 26;
		this.label26.Text = "Ref Doc Invoice No";
		this.txtAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtAmount.Location = new System.Drawing.Point(91, 82);
		this.txtAmount.Name = "txtAmount";
		this.txtAmount.ReadOnly = true;
		this.txtAmount.Size = new System.Drawing.Size(115, 23);
		this.txtAmount.TabIndex = 9;
		this.txtAmount.TabStop = false;
		this.txtAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.CmbRefDocumentType.AllowNull = (DefaultableBoolean)1;
		this.CmbRefDocumentType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbRefDocumentType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbRefDocumentType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance126).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance127).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance127).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance127;
		((AppearanceBase)appearance128).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance128;
		((SpecialBoxBase)((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance142).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance142).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance142).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance143).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance143;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance144).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance3;
		((AppearanceBase)appearance4).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance4).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance5).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance5).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance5).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance5;
		((AppearanceBase)appearance6).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbRefDocumentType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbRefDocumentType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbRefDocumentType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbRefDocumentType).Location = new System.Drawing.Point(207, 80);
		((System.Windows.Forms.Control)(object)this.CmbRefDocumentType).Name = "CmbRefDocumentType";
		((System.Windows.Forms.Control)(object)this.CmbRefDocumentType).Size = new System.Drawing.Size(147, 26);
		((System.Windows.Forms.Control)(object)this.CmbRefDocumentType).TabIndex = 10;
		((System.Windows.Forms.Control)(object)this.CmbRefDocumentType).Leave += new System.EventHandler(CmbRefDocumentType_Leave);
		this.CmbRefDocNo.AllowNull = (DefaultableBoolean)1;
		this.CmbRefDocNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbRefDocNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbRefDocNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance91).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbRefDocNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.CmbRefDocNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbRefDocNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance92).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance92).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbRefDocNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance92;
		((AppearanceBase)appearance93).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRefDocNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance93;
		((SpecialBoxBase)((UltraGridBase)this.CmbRefDocNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance86).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance86).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance86).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRefDocNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.CmbRefDocNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbRefDocNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance87).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbRefDocNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance87;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance88).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbRefDocNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.CmbRefDocNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbRefDocNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRefDocNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance116;
		((AppearanceBase)appearance120).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance120).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbRefDocNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.CmbRefDocNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbRefDocNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance121).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance121).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance121).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance121).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRefDocNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance121;
		((AppearanceBase)appearance122).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbRefDocNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.CmbRefDocNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbRefDocNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance124).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbRefDocNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.CmbRefDocNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbRefDocNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.CmbRefDocNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbRefDocNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbRefDocNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbRefDocNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbRefDocNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbRefDocNo).Location = new System.Drawing.Point(355, 80);
		((System.Windows.Forms.Control)(object)this.CmbRefDocNo).Name = "CmbRefDocNo";
		((System.Windows.Forms.Control)(object)this.CmbRefDocNo).Size = new System.Drawing.Size(185, 26);
		((System.Windows.Forms.Control)(object)this.CmbRefDocNo).TabIndex = 11;
		((System.Windows.Forms.Control)(object)this.CmbRefDocNo).Leave += new System.EventHandler(CmbRefDocNo_Leave);
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.Location = new System.Drawing.Point(208, 63);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(112, 15);
		this.label28.TabIndex = 28;
		this.label28.Text = "Ref Document Type";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(355, 63);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(68, 15);
		this.label4.TabIndex = 27;
		this.label4.Text = "Ref Doc No";
		this.btnplus.BackColor = System.Drawing.Color.Teal;
		this.btnplus.FlatAppearance.BorderSize = 0;
		this.btnplus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnplus.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnplus.ForeColor = System.Drawing.Color.White;
		this.btnplus.Location = new System.Drawing.Point(969, 82);
		this.btnplus.Name = "btnplus";
		this.btnplus.Size = new System.Drawing.Size(39, 23);
		this.btnplus.TabIndex = 14;
		this.btnplus.Text = "+";
		this.btnplus.UseVisualStyleBackColor = false;
		this.btnplus.Click += new System.EventHandler(btnplus_Click);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(4, 15);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(67, 15);
		this.label2.TabIndex = 0;
		this.label2.Text = "Item Name";
		this.btnCancelUpdateDetial.BackColor = System.Drawing.Color.Teal;
		this.btnCancelUpdateDetial.FlatAppearance.BorderSize = 0;
		this.btnCancelUpdateDetial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelUpdateDetial.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelUpdateDetial.ForeColor = System.Drawing.Color.White;
		this.btnCancelUpdateDetial.Location = new System.Drawing.Point(969, 60);
		this.btnCancelUpdateDetial.Name = "btnCancelUpdateDetial";
		this.btnCancelUpdateDetial.Size = new System.Drawing.Size(87, 21);
		this.btnCancelUpdateDetial.TabIndex = 16;
		this.btnCancelUpdateDetial.Text = "Cancel";
		this.btnCancelUpdateDetial.UseVisualStyleBackColor = false;
		this.btnCancelUpdateDetial.Visible = false;
		this.btnCancelUpdateDetial.Click += new System.EventHandler(btnCancelUpdateDetial_Click);
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatAppearance.BorderSize = 0;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(969, 82);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(87, 23);
		this.btnUpdateDetail.TabIndex = 15;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Visible = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.txtRemarksDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksDetail.Location = new System.Drawing.Point(712, 82);
		this.txtRemarksDetail.Name = "txtRemarksDetail";
		this.txtRemarksDetail.Size = new System.Drawing.Size(255, 23);
		this.txtRemarksDetail.TabIndex = 13;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label12.Location = new System.Drawing.Point(712, 63);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(52, 15);
		this.label12.TabIndex = 16;
		this.label12.Text = "Remarks";
		this.cmbItemName.AllowNull = (DefaultableBoolean)1;
		this.cmbItemName.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbItemName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance30).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance30).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance31;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance33).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance33).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.cmbItemName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance37).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance38).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance38).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance38).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.cmbItemName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemName).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbItemName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemName).Location = new System.Drawing.Point(4, 33);
		((System.Windows.Forms.Control)(object)this.cmbItemName).Name = "cmbItemName";
		((System.Windows.Forms.Control)(object)this.cmbItemName).Size = new System.Drawing.Size(202, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemName).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbItemName).TextChanged += new System.EventHandler(cmbItemName_TextChanged);
		((System.Windows.Forms.Control)(object)this.cmbItemName).Leave += new System.EventHandler(cmbItemName_Leave);
		this.groupBox1.Controls.Add(this.DocDate);
		this.groupBox1.Controls.Add(this.txtdocno);
		this.groupBox1.Controls.Add(this.lblToBranch);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbToBranch);
		this.groupBox1.Controls.Add(this.lblFromBranch);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbFromBranch);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add(this.label17);
		this.groupBox1.Controls.Add(this.label15);
		this.groupBox1.Controls.Add(this.txtRemarksHead);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(3, 2);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1060, 72);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Main";
		this.DocDate.CustomFormat = "dd-MMM-yy";
		this.DocDate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.DocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DocDate.Location = new System.Drawing.Point(62, 43);
		this.DocDate.Name = "DocDate";
		this.DocDate.Size = new System.Drawing.Size(126, 23);
		this.DocDate.TabIndex = 1;
		this.DocDate.TabStop = false;
		this.DocDate.Leave += new System.EventHandler(DocDate_Leave);
		this.txtdocno.BackColor = System.Drawing.Color.White;
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtdocno.Location = new System.Drawing.Point(62, 15);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.Size = new System.Drawing.Size(126, 23);
		this.txtdocno.TabIndex = 0;
		this.txtdocno.Leave += new System.EventHandler(txtdocno_Leave);
		this.lblToBranch.AutoSize = true;
		this.lblToBranch.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblToBranch.Location = new System.Drawing.Point(760, 47);
		this.lblToBranch.Name = "lblToBranch";
		this.lblToBranch.Size = new System.Drawing.Size(60, 15);
		this.lblToBranch.TabIndex = 30;
		this.lblToBranch.Text = "To Branch";
		this.CmbToBranch.AllowNull = (DefaultableBoolean)1;
		this.CmbToBranch.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbToBranch.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbToBranch.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbToBranch).DisplayLayout.Appearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbToBranch).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbToBranch).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance44).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance44).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbToBranch).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbToBranch).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance45;
		((SpecialBoxBase)((UltraGridBase)this.CmbToBranch).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance46).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbToBranch).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.CmbToBranch).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbToBranch).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbToBranch).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance47;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbToBranch).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.CmbToBranch).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbToBranch).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbToBranch).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance50).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbToBranch).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbToBranch).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbToBranch).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance51).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance51).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance51).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance51).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbToBranch).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance51;
		((AppearanceBase)appearance52).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbToBranch).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.CmbToBranch).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbToBranch).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbToBranch).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.CmbToBranch).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbToBranch).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbToBranch).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbToBranch).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbToBranch).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbToBranch).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbToBranch.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbToBranch).Location = new System.Drawing.Point(839, 41);
		((System.Windows.Forms.Control)(object)this.CmbToBranch).Name = "CmbToBranch";
		((System.Windows.Forms.Control)(object)this.CmbToBranch).Size = new System.Drawing.Size(217, 26);
		((System.Windows.Forms.Control)(object)this.CmbToBranch).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.CmbToBranch).TabStop = false;
		((System.Windows.Forms.Control)(object)this.CmbToBranch).Leave += new System.EventHandler(CmbToBranch_Leave);
		this.lblFromBranch.AutoSize = true;
		this.lblFromBranch.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblFromBranch.Location = new System.Drawing.Point(760, 19);
		this.lblFromBranch.Name = "lblFromBranch";
		this.lblFromBranch.Size = new System.Drawing.Size(75, 15);
		this.lblFromBranch.TabIndex = 29;
		this.lblFromBranch.Text = "From Branch";
		this.CmbFromBranch.AllowNull = (DefaultableBoolean)1;
		this.CmbFromBranch.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbFromBranch.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbFromBranch.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance97).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbFromBranch).DisplayLayout.Appearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.CmbFromBranch).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbFromBranch).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance98).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance98).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance98).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbFromBranch).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance98;
		((AppearanceBase)appearance89).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFromBranch).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance89;
		((SpecialBoxBase)((UltraGridBase)this.CmbFromBranch).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance114).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance114).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance114).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFromBranch).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.CmbFromBranch).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbFromBranch).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance115).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbFromBranch).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance99).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbFromBranch).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.CmbFromBranch).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbFromBranch).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFromBranch).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance77).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance77).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbFromBranch).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.CmbFromBranch).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbFromBranch).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance78).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance78).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance78).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFromBranch).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance78;
		((AppearanceBase)appearance80).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbFromBranch).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.CmbFromBranch).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbFromBranch).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance102).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbFromBranch).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.CmbFromBranch).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbFromBranch).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.CmbFromBranch).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbFromBranch).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbFromBranch).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbFromBranch).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbFromBranch.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbFromBranch).Location = new System.Drawing.Point(839, 13);
		((System.Windows.Forms.Control)(object)this.CmbFromBranch).Name = "CmbFromBranch";
		((System.Windows.Forms.Control)(object)this.CmbFromBranch).Size = new System.Drawing.Size(217, 26);
		((System.Windows.Forms.Control)(object)this.CmbFromBranch).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.CmbFromBranch).TabStop = false;
		((System.Windows.Forms.Control)(object)this.CmbFromBranch).Leave += new System.EventHandler(CmbFromBranch_Leave);
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label5.Location = new System.Drawing.Point(4, 47);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(57, 15);
		this.label5.TabIndex = 18;
		this.label5.Text = "Doc Date";
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label17.Location = new System.Drawing.Point(4, 19);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(48, 15);
		this.label17.TabIndex = 3;
		this.label17.Text = "Doc No";
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label15.Location = new System.Drawing.Point(192, 19);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(52, 15);
		this.label15.TabIndex = 17;
		this.label15.Text = "Remarks";
		this.txtRemarksHead.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksHead.Location = new System.Drawing.Point(250, 15);
		this.txtRemarksHead.Multiline = true;
		this.txtRemarksHead.Name = "txtRemarksHead";
		this.txtRemarksHead.Size = new System.Drawing.Size(505, 51);
		this.txtRemarksHead.TabIndex = 2;
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.label24);
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Controls.Add(this.ChkBoxPrintPreview);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(2, 2);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1087, 51);
		this.panel2.TabIndex = 0;
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold);
		this.label24.ForeColor = System.Drawing.Color.White;
		this.label24.Location = new System.Drawing.Point(2, 28);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(128, 19);
		this.label24.TabIndex = 35;
		this.label24.Text = "Stock Transfer";
		this.toolStrip1.BackColor = System.Drawing.Color.White;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[9] { this.btnnew, this.btnRefresh, this.btnupdate, this.btnsave, this.btnprint, this.btnAttachment, this.BtnShortCutkeys, this.toolStripSeparator6, this.btnDelete });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1087, 27);
		this.toolStrip1.TabIndex = 0;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnupdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnupdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnupdate.Name = "btnupdate";
		this.btnupdate.Size = new System.Drawing.Size(78, 24);
		this.btnupdate.Text = "&Update";
		this.btnupdate.Click += new System.EventHandler(btnupdate_Click);
		this.btnsave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnprint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnprint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnprint.Name = "btnprint";
		this.btnprint.Size = new System.Drawing.Size(92, 24);
		this.btnprint.Text = "415-Print";
		this.btnprint.Click += new System.EventHandler(btnprint_Click);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(109, 24);
		this.btnAttachment.Text = "Attachment";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.BtnShortCutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShortCutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.BtnShortCutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnShortCutkeys.Name = "BtnShortCutkeys";
		this.BtnShortCutkeys.Size = new System.Drawing.Size(126, 24);
		this.BtnShortCutkeys.Text = "ShortCut Keys";
		this.BtnShortCutkeys.Click += new System.EventHandler(BtnShortCutkeys_Click);
		this.toolStripSeparator6.Name = "toolStripSeparator6";
		this.toolStripSeparator6.Size = new System.Drawing.Size(6, 27);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(73, 24);
		this.btnDelete.Text = "Delete";
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.ChkBoxPrintPreview.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkBoxPrintPreview.AutoSize = true;
		this.ChkBoxPrintPreview.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkBoxPrintPreview.ForeColor = System.Drawing.Color.White;
		this.ChkBoxPrintPreview.Location = new System.Drawing.Point(998, 29);
		this.ChkBoxPrintPreview.Name = "ChkBoxPrintPreview";
		this.ChkBoxPrintPreview.Size = new System.Drawing.Size(84, 20);
		this.ChkBoxPrintPreview.TabIndex = 3;
		this.ChkBoxPrintPreview.Text = "Preview";
		this.ChkBoxPrintPreview.UseVisualStyleBackColor = true;
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.grdhistory);
		this.tabPage2.Controls.Add(this.panel9);
		this.tabPage2.Controls.Add(this.panel7);
		this.tabPage2.Controls.Add(this.panel12);
		this.tabPage2.Controls.Add(this.panel8);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.GridDetail);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage2.Size = new System.Drawing.Size(1091, 597);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.grdhistory.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdhistory).BackColor = System.Drawing.Color.White;
		grdhistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdhistory.DesignTimeLayout = grdhistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdhistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdhistory.DynamicFiltering = true;
		this.grdhistory.FilterMode = (FilterMode)2;
		this.grdhistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdhistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdhistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdhistory).Location = new System.Drawing.Point(2, 142);
		((System.Windows.Forms.Control)(object)this.grdhistory).Name = "grdhistory";
		((ScrollableBase)this.grdhistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdhistory).Size = new System.Drawing.Size(1087, 259);
		((System.Windows.Forms.Control)(object)this.grdhistory).TabIndex = 45;
		this.grdhistory.TotalRow = (InheritableBoolean)1;
		this.grdhistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdhistory).VisualStyle = (VisualStyle)4;
		this.grdhistory.ColumnButtonClick += new ColumnActionEventHandler(grdhistory_ColumnButtonClick);
		this.grdhistory.SelectionChanged += new System.EventHandler(grdhistory_SelectionChanged);
		this.grdhistory.LinkClicked += new ColumnActionEventHandler(grdhistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdhistory).DoubleClick += new System.EventHandler(grdhistory_DoubleClick_1);
		((System.Windows.Forms.Control)(object)this.grdhistory).KeyDown += new System.Windows.Forms.KeyEventHandler(grdhistory_KeyDown);
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.label6);
		this.panel9.Controls.Add(this.ctrlGrdBar6);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel9.Location = new System.Drawing.Point(2, 401);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1087, 24);
		this.panel9.TabIndex = 52;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Tahoma", 11f, System.Drawing.FontStyle.Bold);
		this.label6.ForeColor = System.Drawing.Color.White;
		this.label6.Location = new System.Drawing.Point(3, 3);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(146, 18);
		this.label6.TabIndex = 0;
		this.label6.Text = "Detail Information";
		this.ctrlGrdBar6.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar6.FormName = this;
		this.ctrlGrdBar6.Location = new System.Drawing.Point(1041, 2);
		this.ctrlGrdBar6.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar6.MyGrid = this.GridDetail;
		this.ctrlGrdBar6.Name = "ctrlGrdBar6";
		this.ctrlGrdBar6.Size = new System.Drawing.Size(43, 20);
		this.ctrlGrdBar6.TabIndex = 48;
		this.ctrlGrdBar6.Load += new System.EventHandler(ctrlGrdBar6_Load);
		this.GridDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.GridDetail).BackColor = System.Drawing.Color.White;
		GridDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GridDetail.DesignTimeLayout = GridDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GridDetail).Dock = System.Windows.Forms.DockStyle.Bottom;
		this.GridDetail.DynamicFiltering = true;
		this.GridDetail.FilterMode = (FilterMode)2;
		this.GridDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GridDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GridDetail.GroupByBoxVisible = false;
		this.GridDetail.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.GridDetail).Location = new System.Drawing.Point(2, 425);
		((System.Windows.Forms.Control)(object)this.GridDetail).Name = "GridDetail";
		((ScrollableBase)this.GridDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GridDetail).Size = new System.Drawing.Size(1087, 170);
		((System.Windows.Forms.Control)(object)this.GridDetail).TabIndex = 49;
		this.GridDetail.TotalRow = (InheritableBoolean)1;
		this.GridDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GridDetail).VisualStyle = (VisualStyle)4;
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.label1);
		this.panel7.Controls.Add(this.ctrlGrdBar3);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(2, 118);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1087, 24);
		this.panel7.TabIndex = 51;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Tahoma", 11f, System.Drawing.FontStyle.Bold);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(3, 3);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(132, 18);
		this.label1.TabIndex = 0;
		this.label1.Text = "Filtered Records";
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(1046, 2);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.grdhistory;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(37, 19);
		this.ctrlGrdBar3.TabIndex = 23;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.panel12.BackColor = System.Drawing.Color.White;
		this.panel12.Controls.Add(this.groupBox3);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(2, 52);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1087, 66);
		this.panel12.TabIndex = 50;
		this.groupBox3.Controls.Add(this.rdapproveddate);
		this.groupBox3.Controls.Add(this.rdmodifydate);
		this.groupBox3.Controls.Add(this.drdocdate);
		this.groupBox3.Controls.Add(this.rdentrydate);
		this.groupBox3.Controls.Add(this.btnshow);
		this.groupBox3.Controls.Add(this.label27);
		this.groupBox3.Controls.Add(this.label43);
		this.groupBox3.Controls.Add(this.label62);
		this.groupBox3.Controls.Add(this.label63);
		this.groupBox3.Controls.Add(this.txtToDocNoHistory);
		this.groupBox3.Controls.Add(this.txtFromDocNoHistory);
		this.groupBox3.Controls.Add(this.ToDateHistory);
		this.groupBox3.Controls.Add(this.FromDateHistory);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(2, 1);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(822, 64);
		this.groupBox3.TabIndex = 1;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Filters";
		this.rdapproveddate.AutoSize = true;
		this.rdapproveddate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdapproveddate.Location = new System.Drawing.Point(595, 37);
		this.rdapproveddate.Name = "rdapproveddate";
		this.rdapproveddate.Size = new System.Drawing.Size(105, 19);
		this.rdapproveddate.TabIndex = 9;
		this.rdapproveddate.Text = "Approved Date";
		this.rdapproveddate.UseVisualStyleBackColor = true;
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(595, 16);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 8;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(514, 16);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 6;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(514, 37);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 7;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.btnshow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(446, 36);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(57, 23);
		this.btnshow.TabIndex = 5;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label27.Location = new System.Drawing.Point(360, 17);
		this.label27.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(64, 15);
		this.label27.TabIndex = 141;
		this.label27.Text = "To Doc No";
		this.label43.AutoSize = true;
		this.label43.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label43.Location = new System.Drawing.Point(276, 17);
		this.label43.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(79, 15);
		this.label43.TabIndex = 140;
		this.label43.Text = "From Doc No";
		this.label62.AutoSize = true;
		this.label62.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label62.Location = new System.Drawing.Point(143, 17);
		this.label62.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label62.Name = "label62";
		this.label62.Size = new System.Drawing.Size(48, 15);
		this.label62.TabIndex = 139;
		this.label62.Text = "To Date";
		this.label63.AutoSize = true;
		this.label63.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label63.Location = new System.Drawing.Point(5, 17);
		this.label63.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label63.Name = "label63";
		this.label63.Size = new System.Drawing.Size(63, 15);
		this.label63.TabIndex = 13;
		this.label63.Text = "From Date";
		this.txtToDocNoHistory.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDocNoHistory.Location = new System.Drawing.Point(360, 36);
		this.txtToDocNoHistory.Name = "txtToDocNoHistory";
		this.txtToDocNoHistory.ShortcutsEnabled = false;
		this.txtToDocNoHistory.Size = new System.Drawing.Size(81, 23);
		this.txtToDocNoHistory.TabIndex = 3;
		this.txtToDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtToDocNoHistory_KeyPress);
		this.txtFromDocNoHistory.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDocNoHistory.Location = new System.Drawing.Point(276, 36);
		this.txtFromDocNoHistory.Name = "txtFromDocNoHistory";
		this.txtFromDocNoHistory.ShortcutsEnabled = false;
		this.txtFromDocNoHistory.Size = new System.Drawing.Size(81, 23);
		this.txtFromDocNoHistory.TabIndex = 2;
		this.txtFromDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFromDocNoHistory_KeyPress);
		this.ToDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(143, 36);
		this.ToDateHistory.Margin = new System.Windows.Forms.Padding(4);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(131, 23);
		this.ToDateHistory.TabIndex = 1;
		this.FromDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(5, 36);
		this.FromDateHistory.Margin = new System.Windows.Forms.Padding(4);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(137, 23);
		this.FromDateHistory.TabIndex = 0;
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.label25);
		this.panel8.Controls.Add(this.toolStrip2);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(2, 2);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1087, 50);
		this.panel8.TabIndex = 46;
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold);
		this.label25.ForeColor = System.Drawing.Color.White;
		this.label25.Location = new System.Drawing.Point(1, 29);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(191, 19);
		this.label25.TabIndex = 36;
		this.label25.Text = "Stock Transfer History";
		this.toolStrip2.BackColor = System.Drawing.Color.White;
		this.toolStrip2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.btnNewHistory });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1087, 27);
		this.toolStrip2.TabIndex = 0;
		this.toolStrip2.Text = "toolStrip2";
		this.btnNewHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewHistory.Name = "btnNewHistory";
		this.btnNewHistory.Size = new System.Drawing.Size(68, 24);
		this.btnNewHistory.Text = "&Reset";
		this.btnNewHistory.Click += new System.EventHandler(btnNewHistory_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(1099, 626);
		base.Controls.Add(this.panel1);
		base.HelpButton = true;
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "frmStockTransferStore";
		this.Text = "Stock Transfer";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmStockTransferStore_FormClosing);
		base.Load += new System.EventHandler(PurchsaeOrder_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(PurchsaeOrder_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		this.tabControl2.ResumeLayout(false);
		this.tabPage3.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		this.tabPage4.ResumeLayout(false);
		this.panel14.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdOtherCharges).EndInit();
		this.panel13.ResumeLayout(false);
		this.panel13.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbItemCondition).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRackTo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRackFrom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWareHouseFrom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbWareHouseTo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRefDocInvoiceNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRefDocumentType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRefDocNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemName).EndInit();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbToBranch).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbFromBranch).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdhistory).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.GridDetail).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.panel12.ResumeLayout(false);
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}
