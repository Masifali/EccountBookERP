using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Inventory;
using Architecture.BLL.Mfg;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.Model.Mfg;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Inventory_Definition;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;
using grid.User_Conrtols;
using InheritableBoolean = Janus.Windows.GridEX.InheritableBoolean;
using GroupTotals = Janus.Windows.GridEX.GroupTotals;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.Mfg
{

public class frmWorkOrderForSemiFinish : Form
{
	private Attachment AT = new Attachment();

	private Rightsobjects formright = new Rightsobjects();

	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtOperationProcess = new DataTable();

	private DataTable dtHistory = new DataTable();

	private DataTable dtitem = new DataTable();

	private DataTable dtSampleModal = new DataTable();

	private string AttachmentsValues = "";

	private string CustomAttachmentsValues = "";

	private bool UpdateMode;

	private int RecId;

	private int updateDetailIndex = -1;

	private List<workOrderItemDetail> lstRemoveRecordModalDetail = new List<workOrderItemDetail>();

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel2;

	private Panel panel3;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnsave;

	private ToolStripButton btnUpdate;

	private ToolStripSeparator toolStripSeparator;

	private ToolStripButton btnPrint;

	private Label label12;

	private TabPage tabPage3;

	private ToolStrip toolStrip2;

	private ToolStripButton btnNewHistory;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripButton btnRegister;

	private GroupBox groupBox2;

	private Label label21;

	private Panel panel5;

	private Label label37;

	private DateTimePicker txtdocdate;

	private TextBox txtdocno;

	private ToolStripButton btnRefresh;

	private ToolStripButton btnDelete;

	private ToolStripButton btnshortcutkeys;

	private Panel panel38;

	private Label label32;

	internal GridEX grdhistoryDetail;

	private Panel panel9;

	internal GridEX DatagridHistory;

	private Panel panel15;

	private Label label17;

	private Panel panel13;

	private GroupBox groupBox1;

	private DateTimePicker txtToDateHistory;

	private Label label28;

	private TextBox txtToDocNoHistory;

	private DateTimePicker txtFromdateHistory;

	private Label label35;

	private TextBox txtFromNoHistory;

	private Button btnshow;

	private CtrlGrdBar ctrlGrdBar1;

	internal GridEX grdDetail;

	private Panel panel14;

	private Label label25;

	private Label label23;

	private DateTimePicker txtDeadLineDateMain;

	private DateTimePicker txtEndDate;

	private DateTimePicker txtStartdate;

	private Button BtnCancelOp;

	private Button BtnUpdateOp;

	private Button BtnAddOp;

	private CheckBox ChkPreview;

	private TextBox txtRemarksHeader;

	private Label label8;

	private TextBox txtBatchDesc;

	private Label label13;

	private Label label15;

	private Label label9;

	private ToolStripButton btnSaveAs;

	private ToolStripButton btnAttachment;

	private TextBox txtLeadTimeHeader;

	private Label label24;

	internal UltraCombo CmbOrderNoDetail;

	private Label label2;

	private Label label33;

	private Label label31;

	private TextBox txtQtyDetaill;

	internal UltraCombo CmbCastingTypeDetail;

	private Label label34;

	internal UltraCombo CmbVariantDescription;

	private TextBox txtWorkOrderNo;

	internal UltraCombo CmbItem;

	private Label label26;

	internal UltraCombo CmbUomDetail;

	private Label label1;

	private Label label3;

	private Label label5;

	private Label label4;

	private TextBox txtCastingWeight;

	private TextBox txtItemWeight;

	private RadioButton rdbtnItemCode;

	private RadioButton rdbtnItemName;

	internal UltraCombo CmbCategory;

	private Label label52;

	private Label label6;

	private Button BtnDefineVariant;

	private ToolStripButton BtnDefineItem;

	internal UltraCombo CmbSampleModal;

	private Label label29;

	public frmWorkOrderForSemiFinish(UserAccount user)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void frmBillOfMaterial_Manufacturing_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(10, 10);
			base.AutoScrollMinSize = new Size(1100, 650);
			formright = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnsave.Enabled = formright.DoHaveSaveRight;
			btnSaveAs.Enabled = formright.DoHaveSaveRight;
			btnUpdate.Enabled = formright.DoHaveUpdateRights;
			btnPrint.Enabled = formright.DoHavePrintRights;
			btnDelete.Enabled = formright.DoHaveCanDelete;
			btnDelete.Visible = false;
			ctrlGrdBar1.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar1.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar1.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar1.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar1.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar1.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			dtitem.Columns.Add("Id", typeof(int));
			dtitem.Columns.Add("ItemName");
			dtitem.Columns.Add("ItemCode");
			dtitem.Columns.Add("ItemCategoryId", typeof(int));
			dtitem.Columns.Add("itemModalId", typeof(int));
			dtitem.Columns.Add("SandWeight", typeof(double));
			dtitem.Columns.Add("DyeWeight", typeof(double));
			if (dtSampleModal.Columns.Count == 0)
			{
				dtSampleModal.Columns.AddRange(new DataColumn[5]
				{
					new DataColumn("Id", typeof(int)),
					new DataColumn("SampleNo"),
					new DataColumn("ItemModal"),
					new DataColumn("ItemCategoryId", typeof(int)),
					new DataColumn("ItemId", typeof(int))
				});
			}
			DocumentNoFill();
			GetItemCategory();
			SampleModaldBCall();
			ItemModalBind();
			ItemdtFill();
			ItemDetailBind();
			CastingType();
			dtOperationProcess.Columns.Add("Id", typeof(int));
			dtOperationProcess.Columns.Add("OrderId", typeof(int));
			dtOperationProcess.Columns.Add("OrderNo", typeof(int));
			dtOperationProcess.Columns.Add("ItemId", typeof(int));
			dtOperationProcess.Columns.Add("ItemCode", typeof(string));
			dtOperationProcess.Columns.Add("ItemName", typeof(string));
			dtOperationProcess.Columns.Add("UomId", typeof(int));
			dtOperationProcess.Columns.Add("Uom", typeof(string));
			dtOperationProcess.Columns.Add("VariantId", typeof(int));
			dtOperationProcess.Columns.Add("VariantDescription", typeof(string));
			dtOperationProcess.Columns.Add("CastingTypeId", typeof(int));
			dtOperationProcess.Columns.Add("CastingType", typeof(string));
			dtOperationProcess.Columns.Add("Qty", typeof(decimal));
			dtOperationProcess.Columns.Add("WeightPerQty", typeof(decimal));
			dtOperationProcess.Columns.Add("CastingWeight", typeof(decimal));
			grdDetail.DataSource = dtOperationProcess;
			grdDetail.RetrieveStructure();
			grdDetailSetting();
			txtdocdate.Focus();
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
			int code = WorkOrder.GenerateCode(new WorkOrder
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				DocumentTypeId = 1621,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			txtdocno.Text = code.ToString();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetItemCategory()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		try
		{
			int SupId = Conversion.ToInt(CmbCategory.Value);
			DataTable dtItemCategory = ItemCategory.Getall(new ItemCategory
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtItemCategory.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtItemCategory, CmbCategory, "Id", "CategoryDescription", "ItemCategory", false);
				if (SupId > 0)
				{
					if (dtItemCategory.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == SupId))
					{
						CmbCategory.Value = SupId;
					}
					else
					{
						((Control)(object)CmbCategory).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbCategory).Text = string.Empty;
				((UltraGridBase)CmbCategory).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SampleModaldBCall()
	{
		try
		{
			dtSampleModal.Rows.Clear();
			DataTable dt = ItemModel.ItemModel_WithItemId(UserAccount.OrganizationId, UserAccount.CompanyId);
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtSampleModal.Rows.Add(dt.Rows[i]["itemModalId"], dt.Rows[i]["ItemSampleNo"], dt.Rows[i]["ItemModal"], dt.Rows[i]["ItemCategoryId"], dt.Rows[i]["ItemId"]);
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ItemModalBind(int CategoryId = 0, int itemId = 0)
	{
		try
		{
			int Id = Conversion.ToInt(CmbSampleModal.Value);
			DataTable FilterItems = new DataTable();
			if (itemId > 0)
			{
				DataRow[] filteredRows = dtSampleModal.Select($"ItemId = {itemId}");
				FilterItems = (filteredRows.Any() ? filteredRows.CopyToDataTable() : new DataTable());
			}
			else if (CategoryId > 0)
			{
				DataRow[] filteredRows2 = dtSampleModal.Select($"ItemCategoryId = {CategoryId}");
				FilterItems = (filteredRows2.Any() ? filteredRows2.CopyToDataTable() : new DataTable());
			}
			else
			{
				FilterItems = dtSampleModal;
			}
			if (FilterItems.Rows.Count > 0)
			{
				DDL.BindDDL(FilterItems, CmbSampleModal, "Id", "SampleNo", "SampleNo", ZeroIndex: true);
				((UltraGridBase)CmbSampleModal).DisplayLayout.Bands[0].Columns[1].Width = 300;
				((UltraGridBase)CmbSampleModal).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				((UltraGridBase)CmbSampleModal).DisplayLayout.Bands[0].Columns[4].Hidden = true;
				((UltraGridBase)CmbSampleModal).Rows[1].Activate();
				if (Id > 0)
				{
					if (FilterItems.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
					{
						CmbSampleModal.Value = Id;
					}
					else
					{
						((Control)(object)CmbSampleModal).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbSampleModal).Text = string.Empty;
				((UltraGridBase)CmbSampleModal).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ItemdtFill()
	{
		try
		{
			dtitem.Rows.Clear();
			DataTable dt = CommonServices.ItemGetForComboServiceBindForEngr();
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtitem.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ItemName"], dt.Rows[i]["ItemCodeNew"], dt.Rows[i]["ItemCategoryId"], dt.Rows[i]["itemModalId"], dt.Rows[i]["SandWeight"], dt.Rows[i]["DyeWeight"]);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemDetailBind(int CategoryId = 0, int itemModalId = 0)
	{
		try
		{
			int Id = Conversion.ToInt(CmbItem.Value);
			DataTable FilterItems = new DataTable();
			if (itemModalId > 0)
			{
				DataRow[] filteredRows = dtitem.Select($"itemModalId = {itemModalId}");
				FilterItems = (filteredRows.Any() ? filteredRows.CopyToDataTable() : new DataTable());
			}
			else if (CategoryId > 0)
			{
				DataRow[] filteredRows2 = dtitem.Select($"ItemCategoryId = {CategoryId}");
				FilterItems = (filteredRows2.Any() ? filteredRows2.CopyToDataTable() : new DataTable());
			}
			else
			{
				FilterItems = dtitem;
			}
			if (FilterItems.Rows.Count > 0)
			{
				if (rdbtnItemName.Checked)
				{
					DDL.BindDDL(FilterItems, CmbItem, "Id", "ItemName", "Item Name", ZeroIndex: true);
				}
				else
				{
					DDL.BindDDL(FilterItems, CmbItem, "Id", "ItemCode", "Item Name", ZeroIndex: true);
				}
				((UltraGridBase)CmbItem).DisplayLayout.Bands[0].Columns[1].Width = 300;
				((UltraGridBase)CmbItem).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				((UltraGridBase)CmbItem).DisplayLayout.Bands[0].Columns[4].Hidden = true;
				if (Id > 0)
				{
					if (FilterItems.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
					{
						CmbItem.Value = Id;
					}
					else
					{
						((UltraGridBase)CmbItem).Rows[1].Activate();
					}
				}
				else
				{
					((UltraGridBase)CmbItem).Rows[1].Activate();
				}
			}
			else
			{
				((Control)(object)CmbItem).Text = string.Empty;
				((UltraGridBase)CmbItem).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CastingType()
	{
		try
		{
			int DetailId = Conversion.ToInt(CmbCastingTypeDetail.Value);
			DataTable dt = LookUps.GetDataByTypeId(1);
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbCastingTypeDetail, "Id", "LookupName", "Casting Type", true);
				((UltraGridBase)CmbCastingTypeDetail).DisplayLayout.Bands[0].Columns[1].Width = 350;
				if (DetailId > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == DetailId))
					{
						CmbCastingTypeDetail.Value = DetailId;
					}
					else
					{
						((Control)(object)CmbCastingTypeDetail).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbCastingTypeDetail).Text = string.Empty;
				((UltraGridBase)CmbCastingTypeDetail).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillItemFinishWeight(int Id)
	{
		try
		{
			double FinishWeight = 0.0;
			if (((UltraGridBase)CmbItem).ActiveRow != null && Id > 0)
			{
				FinishWeight = ((Conversion.ToInt(CmbCastingTypeDetail.Value) == 1) ? Conversion.ToDouble(((UltraDropDownBase)CmbItem).SelectedRow.Cells["SandWeight"].Value) : Conversion.ToDouble(((UltraDropDownBase)CmbItem).SelectedRow.Cells["DyeWeight"].Value));
			}
			txtItemWeight.Text = FinishWeight.ToString("#,##.##");
			txtCastingWeight.Text = (Conversion.ToDouble((object)txtQtyDetaill.Text) * FinishWeight).ToString("#,##.##");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbCategory_Leave(object sender, EventArgs e)
	{
		try
		{
			ItemDetailBind(Conversion.ToInt(CmbCategory.Value));
			ItemModalBind(Conversion.ToInt(CmbCategory.Value));
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
			int ItemId = Conversion.ToInt(CmbItem.Value);
			ItemUomBind(ItemId);
			bindvarientunit(ItemId);
			ItemModalBind(0, ItemId);
			if (((UltraGridBase)CmbItem).ActiveRow != null && ItemId > 0)
			{
				CmbCategory.Value = Conversion.ToInt(((UltraDropDownBase)CmbItem).SelectedRow.Cells["ItemCategoryId"].Value);
			}
			FillItemFinishWeight(Conversion.ToInt(CmbItem.Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemUomBind(int ItemId)
	{
		try
		{
			int Id = Conversion.ToInt(CmbUomDetail.Value);
			((Control)(object)CmbUomDetail).Text = string.Empty;
			DataTable dtuomheader = CommonServices.GetUomScheduleByItemId(ItemId);
			if (dtuomheader.Rows.Count > 0)
			{
				DDL.BindDDL(dtuomheader, CmbUomDetail, "Id", "UOMCode", "UOM", ZeroIndex: false);
				((UltraGridBase)CmbUomDetail).DisplayLayout.Bands[0].Columns[1].Width = 193;
				((UltraGridBase)CmbUomDetail).Rows[0].Activate();
				if (Id > 0)
				{
					if (dtuomheader.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
					{
						CmbUomDetail.Value = Id;
					}
					else
					{
						((UltraGridBase)CmbUomDetail).Rows[0].Activate();
					}
				}
			}
			else
			{
				((Control)(object)CmbUomDetail).Text = string.Empty;
				((UltraGridBase)CmbUomDetail).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void bindvarientunit(int ID)
	{
		try
		{
			string ItId = Conversion.ToString((object)((Control)(object)CmbVariantDescription).Text);
			DataTable dtunit = ItemAttributeVarient.GetAllForCombo(UserAccount.OrganizationId, UserAccount.CompanyId, ID);
			if (dtunit.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("VariantId", typeof(int));
				dt.Columns.Add("VariantDescription");
				dt.Columns.Add("VariantEquivalent", typeof(double));
				dt.Clear();
				for (int i = 0; i < dtunit.Rows.Count; i++)
				{
					dt.Rows.Add(dtunit.Rows[i]["ItemAttributeVarientId"], dtunit.Rows[i]["VarientDescription"], dtunit.Rows[i]["VarientEquivalent"]);
				}
				DDL.BindDDL(dt, CmbVariantDescription, "VariantId", "VariantDescription", "VariantDescription", ZeroIndex: true);
				((UltraGridBase)CmbVariantDescription).Rows[1].Activate();
				if (ItId != null && ItId != string.Empty)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<string>("VariantDescription") == ItId))
					{
						((Control)(object)CmbVariantDescription).Text = ItId;
					}
					else
					{
						((UltraGridBase)CmbVariantDescription).Rows[1].Activate();
					}
				}
			}
			else
			{
				((Control)(object)CmbVariantDescription).Text = string.Empty;
				((UltraGridBase)CmbVariantDescription).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbCastingTypeDetail_Leave(object sender, EventArgs e)
	{
		try
		{
			FillItemFinishWeight(Conversion.ToInt(CmbItem.Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtQtyDetaill_TextChanged(object sender, EventArgs e)
	{
		try
		{
			double FinishWeight = Conversion.ToDouble((object)txtItemWeight.Text);
			double QtyDetaill = Conversion.ToDouble((object)txtQtyDetaill.Text);
			txtCastingWeight.Text = (QtyDetaill * FinishWeight).ToString("#,##.##");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void rdbtnItemName_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (dtitem.Rows.Count > 0)
			{
				int ItemId = 0;
				if (Conversion.ToInt(CmbItem.Value) > 0)
				{
					ItemId = Conversion.ToInt(CmbItem.Value);
				}
				if (rdbtnItemName.Checked)
				{
					DDL.BindDDL(dtitem, CmbItem, "Id", "ItemName", "Item Name", ZeroIndex: true);
				}
				else
				{
					DDL.BindDDL(dtitem, CmbItem, "Id", "ItemCode", "Item Name", ZeroIndex: true);
				}
				if (ItemId > 0)
				{
					CmbItem.Value = ItemId;
					CmbItem.Focus();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbSampleModal_Leave(object sender, EventArgs e)
	{
		try
		{
			int Id = Conversion.ToInt(CmbSampleModal.Value);
			if (((UltraGridBase)CmbSampleModal).ActiveRow != null && Id > 0)
			{
				CmbCategory.Value = Conversion.ToInt(((UltraDropDownBase)CmbSampleModal).SelectedRow.Cells["ItemCategoryId"].Value);
			}
			ItemDetailBind(0, Conversion.ToInt(CmbSampleModal.Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (txtdocno.Text.Trim() == string.Empty || txtdocno.Text == "0")
		{
			MessageBox.Show("doc No Field Required");
			txtdocno.Focus();
			return false;
		}
		if (txtWorkOrderNo.Text.Trim() == string.Empty || txtWorkOrderNo.Text == "0")
		{
			MessageBox.Show("WorkOrder No Field Required");
			txtWorkOrderNo.Focus();
			return false;
		}
		if (Conversion.ToDateTime((object)txtStartdate.Value).Date > Conversion.ToDateTime((object)txtEndDate.Value).Date)
		{
			MessageBox.Show("Start date cannot be greater than End Date");
			txtStartdate.Focus();
			return false;
		}
		if (Conversion.ToDateTime((object)txtDeadLineDateMain.Value).Date < Conversion.ToDateTime((object)txtEndDate.Value).Date)
		{
			MessageBox.Show("Dead Line date cannot be less than End Date");
			txtStartdate.Focus();
			return false;
		}
		return true;
	}

	private bool FormValidationOperationDetail()
	{
		if (((UltraGridBase)CmbItem).ActiveRow == null || Conversion.ToInt(CmbItem.Value) == 0)
		{
			MessageBox.Show("Item / Modal Field is Required");
			CmbItem.Focus();
			return false;
		}
		if (((UltraGridBase)CmbUomDetail).ActiveRow == null || Conversion.ToInt(CmbUomDetail.Value) == 0)
		{
			MessageBox.Show("Uom Field is Required");
			CmbUomDetail.Focus();
			return false;
		}
		if (((UltraGridBase)CmbCastingTypeDetail).ActiveRow == null || Conversion.ToInt(CmbCastingTypeDetail.Value) == 0)
		{
			MessageBox.Show("Casting Type Field is Required");
			CmbCastingTypeDetail.Focus();
			return false;
		}
		if (txtQtyDetaill.Text.Trim() == string.Empty || txtQtyDetaill.Text.Trim() == "0")
		{
			MessageBox.Show("Qty Field is Required");
			txtQtyDetaill.Focus();
			return false;
		}
		return true;
	}

	private void BtnAddOp_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValidationOperationDetail())
			{
				dtOperationProcess.Rows.Add(0, Conversion.ToInt(CmbOrderNoDetail.Value), Conversion.ToInt((object)((Control)(object)CmbOrderNoDetail).Text), Conversion.ToInt(CmbItem.Value), Conversion.ToString(((UltraDropDownBase)CmbItem).SelectedRow.Cells["ItemCode"].Value), Conversion.ToString(((UltraDropDownBase)CmbItem).SelectedRow.Cells["ItemName"].Value), Conversion.ToInt(CmbUomDetail.Value), Conversion.ToString((object)((Control)(object)CmbUomDetail).Text), Conversion.ToInt(CmbVariantDescription.Value), Conversion.ToString((object)((Control)(object)CmbVariantDescription).Text), Conversion.ToInt(CmbCastingTypeDetail.Value), Conversion.ToString((object)((Control)(object)CmbCastingTypeDetail).Text), Conversion.ToDouble((object)txtQtyDetaill.Text), Conversion.ToDouble((object)txtItemWeight.Text), Conversion.ToDouble((object)txtCastingWeight.Text));
				grdDetail.DataSource = dtOperationProcess;
				grdDetail.RetrieveStructure();
				grdDetailSetting();
				FromResetDetail();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnUpdateOp_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValidationOperationDetail())
			{
				dtOperationProcess.Rows[updateDetailIndex]["OrderId"] = Conversion.ToInt(CmbOrderNoDetail.Value);
				dtOperationProcess.Rows[updateDetailIndex]["OrderNo"] = Conversion.ToInt((object)((Control)(object)CmbOrderNoDetail).Text);
				dtOperationProcess.Rows[updateDetailIndex]["ItemId"] = Conversion.ToInt(CmbItem.Value);
				dtOperationProcess.Rows[updateDetailIndex]["ItemCode"] = Conversion.ToString(((UltraDropDownBase)CmbItem).SelectedRow.Cells["ItemCode"].Value);
				dtOperationProcess.Rows[updateDetailIndex]["ItemName"] = Conversion.ToString(((UltraDropDownBase)CmbItem).SelectedRow.Cells["ItemName"].Value);
				dtOperationProcess.Rows[updateDetailIndex]["UomId"] = Conversion.ToInt(CmbUomDetail.Value);
				dtOperationProcess.Rows[updateDetailIndex]["Uom"] = Conversion.ToString((object)((Control)(object)CmbUomDetail).Text);
				dtOperationProcess.Rows[updateDetailIndex]["VariantId"] = Conversion.ToInt(CmbVariantDescription.Value);
				dtOperationProcess.Rows[updateDetailIndex]["VariantDescription"] = Conversion.ToString((object)((Control)(object)CmbVariantDescription).Text);
				dtOperationProcess.Rows[updateDetailIndex]["CastingTypeId"] = Conversion.ToInt(CmbCastingTypeDetail.Value);
				dtOperationProcess.Rows[updateDetailIndex]["CastingType"] = Conversion.ToString((object)((Control)(object)CmbCastingTypeDetail).Text);
				dtOperationProcess.Rows[updateDetailIndex]["Qty"] = Conversion.ToDouble((object)txtQtyDetaill.Text);
				dtOperationProcess.Rows[updateDetailIndex]["WeightPerQty"] = Conversion.ToDouble((object)txtItemWeight.Text);
				dtOperationProcess.Rows[updateDetailIndex]["CastingWeight"] = Conversion.ToDouble((object)txtCastingWeight.Text);
				grdDetail.DataSource = dtOperationProcess;
				FromResetDetail();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnCancelOp_Click(object sender, EventArgs e)
	{
		try
		{
			FromResetDetail();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetail_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grdDetail.CurrentRow;
			if (item != null)
			{
				updateDetailIndex = item.RowIndex;
				((Control)(object)CmbCategory).Text = "";
				CmbCategory_Leave(null, null);
				CmbOrderNoDetail.Value = Conversion.ToInt(item.Cells["OrderId"].Value);
				CmbItem.Value = Conversion.ToInt(item.Cells["ItemId"].Value);
				cmbItem_Leave(null, null);
				CmbUomDetail.Value = Conversion.ToInt(item.Cells["UomId"].Value);
				CmbVariantDescription.Value = Conversion.ToInt(item.Cells["VariantId"].Value);
				CmbCastingTypeDetail.Value = Conversion.ToInt(item.Cells["CastingTypeId"].Value);
				CmbCastingTypeDetail_Leave(null, null);
				txtQtyDetaill.Text = Conversion.ToDouble(item.Cells["Qty"].Value).ToString("#,##.###");
				BtnAddOp.Visible = false;
				BtnUpdateOp.Visible = true;
				BtnCancelOp.Visible = true;
				CmbItem.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetailSetting()
	{
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Expected O, but got Unknown
		try
		{
			grdDetail.RootTable.Columns["Id"].Visible = false;
			grdDetail.RootTable.Columns["OrderId"].Visible = false;
			grdDetail.RootTable.Columns["ItemId"].Visible = false;
			grdDetail.RootTable.Columns["UomId"].Visible = false;
			grdDetail.RootTable.Columns["VariantId"].Visible = false;
			grdDetail.RootTable.Columns["CastingTypeId"].Visible = false;
			grdDetail.RootTable.Columns["VariantDescription"].Caption = "Modal Description";
			grdDetail.RootTable.HeaderLines = 3;
			foreach (GridEXColumn item in (JanusCollectionBase)grdDetail.RootTable.Columns)
			{
				item.HeaderAlignment = (TextAlignment)2;
				item.EditType = (EditType)0;
				item.FilterEditType = (FilterEditType)1;
				CommonServices.GridColumnSettings(item);
			}
			grdDetail.RootTable.Columns["OrderNo"].Width = Constants.InventoryConstants.OrderNo;
			grdDetail.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			grdDetail.RootTable.Columns["ItemCode"].Width = Constants.InventoryConstants.ItemCode;
			grdDetail.RootTable.Columns["Uom"].Width = Constants.InventoryConstants.UomConstant;
			grdDetail.RootTable.Columns["VariantDescription"].Width = Constants.InventoryConstants.VariantEquivalent + 10;
			grdDetail.RootTable.Columns["CastingType"].Width = Constants.InventoryConstants.ItemType;
			grdDetail.RootTable.Columns["Qty"].Width = Constants.InventoryConstants.QtyConstant;
			grdDetail.RootTable.Columns["WeightPerQty"].Width = Constants.InventoryConstants.WeightConstant;
			grdDetail.RootTable.Columns["CastingWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdDetail.RootTable.Columns.Add("Delete");
			grdDetail.RootTable.Columns["Delete"].Caption = "X";
			grdDetail.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdDetail.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdDetail.RootTable.Columns["Delete"].ButtonText = "X";
			grdDetail.RootTable.Columns["Delete"].Width = 25;
			grdDetail.RootTable.Columns["Delete"].Position = 0;
			grdDetail.RootTable.Columns.Add("Edit");
			grdDetail.RootTable.Columns["Edit"].Caption = "Edit";
			grdDetail.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdDetail.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdDetail.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdDetail.RootTable.Columns["Edit"].Width = 40;
			grdDetail.RootTable.Columns["Edit"].Position = 1;
			grdDetail.FrozenColumns = 2;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetail_CellUpdated(object sender, ColumnActionEventArgs e)
	{
	}

	private void grdDetail_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow r = grdDetail.CurrentRow;
			if (e.Column.Key == "Delete")
			{
				DeleteOperationDetailRow(r);
			}
			if (e.Column.Key == "Edit")
			{
				grdDetail_DoubleClick(null, null);
			}
			grdDetail.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeleteOperationDetailRow(GridEXRow r)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
		try
		{
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			if (updateDetailIndex == -1)
			{
				if (Conversion.ToInt((object)r.Cells["Id"].Value.ToString()) != 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
					{
						workOrderItemDetail d1 = new workOrderItemDetail();
						d1.workOrderItemDetailId = Conversion.ToInt(r.Cells["Id"].Value);
						d1.OrderId = Conversion.ToInt(r.Cells["OrderId"].Value);
						d1.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
						d1.UomId = Conversion.ToInt(r.Cells["UomId"].Value);
						d1.ItemVariantId = Conversion.ToInt(r.Cells["VariantId"].Value);
						d1.CastingTypeId = Conversion.ToInt(r.Cells["CastingTypeId"].Value);
						d1.Qty = Conversion.ToDecimal(r.Cells["Qty"].Value);
						d1.ActionTypeId = 3;
						lstRemoveRecordModalDetail.Add(d1);
						dtOperationProcess.Rows[r.RowIndex].Delete();
						r.Delete();
					}
				}
				else
				{
					dtOperationProcess.Rows[r.RowIndex].Delete();
					r.Delete();
				}
				return;
			}
			throw new Exception("Please Reset the Detail first..");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		FromReset();
	}

	private void FromReset()
	{
		try
		{
			RecId = 0;
			AT = new Attachment();
			Attachment.dtAttachments.Clear();
			AttachmentsValues = "";
			CustomAttachmentsValues = "";
			updateDetailIndex = -1;
			lstRemoveRecordModalDetail.Clear();
			DocumentNoFill();
			txtdocdate.Value = DateTime.Now;
			txtWorkOrderNo.Text = string.Empty;
			txtRemarksHeader.Text = string.Empty;
			FromResetDetail();
			dtOperationProcess.Rows.Clear();
			grdDetail.ClearStructure();
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			btnSaveAs.Visible = false;
			btnDelete.Visible = false;
			UpdateMode = false;
			txtdocdate.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FromResetDetail()
	{
		try
		{
			updateDetailIndex = -1;
			((Control)(object)CmbOrderNoDetail).Text = string.Empty;
			((Control)(object)CmbItem).Text = string.Empty;
			((Control)(object)CmbUomDetail).Text = string.Empty;
			((Control)(object)CmbVariantDescription).Text = string.Empty;
			txtItemWeight.Text = string.Empty;
			txtCastingWeight.Text = string.Empty;
			((Control)(object)CmbCastingTypeDetail).Text = string.Empty;
			txtQtyDetaill.Text = string.Empty;
			BtnAddOp.Visible = true;
			BtnCancelOp.Visible = false;
			BtnUpdateOp.Visible = false;
			CmbItem.Focus();
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
			GetItemCategory();
			SampleModaldBCall();
			ItemModalBind();
			ItemdtFill();
			ItemDetailBind();
			CastingType();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmWorkOrderForSemiFinish_FormClosed(object sender, FormClosedEventArgs e)
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

	private void BtnCancelOp_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl != null && base.ActiveControl.Name == "tabControl1")
			{
				CmbItem.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnAddOp_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl != null && base.ActiveControl.Name == "CmbItemDetail")
			{
				CmbItem.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbOrderNoDetail_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl != null && base.ActiveControl.Name == "grdDetail")
			{
				txtRemarksHeader.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtRemarksHeader_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl != null && base.ActiveControl.Name == "ChkPreview")
			{
				CmbOrderNoDetail.Focus();
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
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Expected O, but got Unknown
		try
		{
			if (!FormValidation())
			{
				return;
			}
			if (grdDetail.GetRows().Count() == 0)
			{
				MessageBox.Show("Detail Grid Record Not Found...\nGrid Should have atleast one record");
				return;
			}
			WorkOrder Or = new WorkOrder();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				Or.WorkOrderId = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			Or.CompanyId = UserAccount.CompanyId;
			Or.BranchesId = UserAccount.BranchesId;
			Or.ProjectsId = UserAccount.BranchesId;
			Or.OrganizationId = UserAccount.OrganizationId;
			Or.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			Or.DocumentTypeId = 1621;
			Or.DocDate = Conversion.ToDateTime((object)txtdocdate.Value);
			Or.DocNo = Conversion.ToInt((object)txtdocno.Text);
			Or.ManualDocCode = txtWorkOrderNo.Text;
			Or.StartDate = Conversion.ToDateTime((object)txtStartdate.Value);
			Or.EndDate = Conversion.ToDateTime((object)txtEndDate.Value);
			Or.DeadLine = Conversion.ToDateTime((object)txtDeadLineDateMain.Value);
			Or.BatchDesc = txtBatchDesc.Text;
			Or.RemarksHeader = Conversion.ToString((object)txtRemarksHeader.Text);
			Or.EntryUserId = UserAccount.ID;
			Or.EntryDate = DateTime.Now;
			Or.ModifyDate = DateTime.Now;
			Or.ModifyUserId = UserAccount.ID;
			Or.ApprovedUserId = UserAccount.ID;
			Or.ApprovedDate = DateTime.Now;
			Or.IsApproved = false;
			Or.WorkOrderCustomerList = new List<WorkOrderCustomer>();
			Or.WorkOrderOperationList = new List<WorkOrderOperation>();
			Or.WorkOrderMaterialList = new List<workOrderMaterial>();
			Or.workOrderItemDetailList = new List<workOrderItemDetail>();
			if (lstRemoveRecordModalDetail.Count > 0 && RecId > 0)
			{
				for (int i = 0; i < lstRemoveRecordModalDetail.Count; i++)
				{
					Or.workOrderItemDetailList.Add(lstRemoveRecordModalDetail[i]);
				}
			}
			GridEXRow[] rows = grdDetail.GetRows();
			foreach (GridEXRow r in rows)
			{
				if (Conversion.ToInt((object)r.Cells["ItemId"].Value.ToString()) != 0)
				{
					workOrderItemDetail d1 = new workOrderItemDetail();
					if (r.Cells["ItemId"].Text == string.Empty)
					{
						MessageBox.Show("ItemModal Field Required in Grid Detail Row No: " + (r.RowIndex + 1));
						return;
					}
					if (r.Cells["UomId"].Text == string.Empty)
					{
						MessageBox.Show("Uom Field Required in Grid Detail Row No: " + (r.RowIndex + 1));
						return;
					}
					if (r.Cells["VariantId"].Text == string.Empty)
					{
						MessageBox.Show("VariantDescription Field Required in Grid Detail Row No: " + (r.RowIndex + 1));
						return;
					}
					if (r.Cells["CastingTypeId"].Text == string.Empty)
					{
						MessageBox.Show("CastingType Field Required in Grid Detail Row No: " + (r.RowIndex + 1));
						return;
					}
					if (Conversion.ToDouble(r.Cells["Qty"].Value) == 0.0)
					{
						MessageBox.Show("Qty Field Required in Grid Detail Row No: " + (r.RowIndex + 1));
						return;
					}
					d1.workOrderItemDetailId = ((RecId != 0) ? Conversion.ToInt(r.Cells["Id"].Value) : 0);
					d1.ActionTypeId = ((d1.workOrderItemDetailId <= 0) ? 1 : 2);
					d1.OrderId = Conversion.ToInt(r.Cells["OrderId"].Value);
					d1.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					d1.UomId = Conversion.ToInt(r.Cells["UomId"].Value);
					d1.ItemVariantId = Conversion.ToInt(r.Cells["VariantId"].Value);
					d1.CastingTypeId = Conversion.ToInt(r.Cells["CastingTypeId"].Value);
					d1.Qty = Conversion.ToDecimal(r.Cells["Qty"].Value);
					Or.workOrderItemDetailList.Add(d1);
				}
			}
			Or.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			Or.DeleteAttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<WorkOrder>(Or, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
			Or.AttachmentsValues = AttachmentsValues;
			Or.CustomAttachmentsValues = CustomAttachmentsValues;
			int success = WorkOrder.Save(Or);
			Or.WorkOrderId = success;
			if (RecId > 0)
			{
				MessageBox.Show("Data Update Successfully....  " + Or.DocNo);
				FormHelper.DeleteAllAttachmentsFromDb(AT, RecId, 1621, base.Name);
			}
			else
			{
				MessageBox.Show("Data Save Successfully....  " + Or.DocNo);
			}
			FromReset();
			if (ChkPreview.Checked && formright.DoHavePrintRights)
			{
				WorkOrderSlip(Or.WorkOrderId);
			}
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
			Insert();
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
			dtOperationProcess.Clear();
			grdDetail.ClearStructure();
			RecId = Id;
			lstRemoveRecordModalDetail.Clear();
			WorkOrder Or = WorkOrder.ReadById(RecId);
			if (Or == null)
			{
				return;
			}
			tabControl1.SelectedIndex = 0;
			UpdateMode = true;
			btnsave.Visible = false;
			btnSaveAs.Visible = false;
			btnUpdate.Visible = true;
			btnDelete.Visible = true;
			txtdocdate.Value = Or.DocDate;
			txtdocno.Text = Conversion.ToString((object)Or.DocNo);
			txtWorkOrderNo.Text = Or.ManualDocCode;
			txtStartdate.Value = Or.StartDate;
			txtEndDate.Value = Or.EndDate;
			txtDeadLineDateMain.Value = Or.DeadLine;
			txtRemarksHeader.Text = Conversion.ToString((object)Or.RemarksHeader);
			txtBatchDesc.Text = Conversion.ToString((object)Or.BatchDesc);
			AttachmentsValues = Or.AttachmentsValues;
			CustomAttachmentsValues = Or.CustomAttachmentsValues;
			if (Or.workOrderItemDetailList.Count > 0)
			{
				for (int i = 0; i < Or.workOrderItemDetailList.Count; i++)
				{
					decimal ItemWeight = default(decimal);
					ItemWeight = ((Or.workOrderItemDetailList[i].CastingTypeId != 1) ? Conversion.ToDecimal((object)Or.workOrderItemDetailList[i].DyeWeight) : Conversion.ToDecimal((object)Or.workOrderItemDetailList[i].SandWeight));
					dtOperationProcess.Rows.Add(Or.workOrderItemDetailList[i].workOrderItemDetailId, Or.workOrderItemDetailList[i].OrderId, Or.workOrderItemDetailList[i].OrderNo, Or.workOrderItemDetailList[i].ItemId, Or.workOrderItemDetailList[i].ItemCode, Or.workOrderItemDetailList[i].ItemName, Or.workOrderItemDetailList[i].UomId, Or.workOrderItemDetailList[i].Uom, Or.workOrderItemDetailList[i].ItemVariantId, Or.workOrderItemDetailList[i].VariantDescription, Or.workOrderItemDetailList[i].CastingTypeId, Or.workOrderItemDetailList[i].CastingType, Or.workOrderItemDetailList[i].Qty, ItemWeight, ItemWeight * Or.workOrderItemDetailList[i].Qty);
				}
			}
			else
			{
				grdDetail.ClearStructure();
			}
			grdDetail.DataSource = dtOperationProcess;
			grdDetail.RetrieveStructure();
			grdDetailSetting();
			FormHelper.LoadAttachmentsForObject(AT, RecId, base.Name);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnDelete_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					WorkOrder.DeleteByID(UserAccount.ID, RecId);
					MessageBox.Show("Delete Record Successfully");
					FromReset();
				}
			}
			else
			{
				MessageBox.Show("No record found to Delete");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnSaveAs_Click(object sender, EventArgs e)
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

	private void txtQtyDetail_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtLeadTimeHeader_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtFromNoHistory_KeyPress(object sender, KeyPressEventArgs e)
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

	private void btnPrint_Click(object sender, EventArgs e)
	{
		try
		{
			WorkOrderSlip(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void WorkOrderSlip(int PrintId)
	{
		try
		{
			CommonServices.WorkOrderSemiFinish_Slip1621(PrintId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (tabControl1.SelectedIndex == 1)
		{
			txtFromdateHistory.Focus();
		}
		else
		{
			txtdocdate.Focus();
		}
	}

	private void btnshow_Click(object sender, EventArgs e)
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
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		try
		{
			dtHistory.Clear();
			ReportsParameters header = new ReportsParameters();
			header.CompanyId = UserAccount.CompanyId;
			header.OrganizationId = UserAccount.OrganizationId;
			header.BranchesId = UserAccount.BranchesId;
			header.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			header.DocumentTypeId = 1621;
			header.FromDocNo = Conversion.ToInt((object)txtFromNoHistory.Text.Trim());
			header.ToDocNo = Conversion.ToInt((object)txtToDocNoHistory.Text.Trim());
			if (txtFromdateHistory.Checked)
			{
				header.FromDate = Conversion.ToDateTime((object)txtFromdateHistory.Value);
			}
			if (txtToDateHistory.Checked)
			{
				header.ToDate = Conversion.ToDateTime((object)txtToDateHistory.Value);
			}
			header.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			dtHistory = WorkOrder.FormHistory(header);
			if (dtHistory.Rows.Count > 0)
			{
				DataTable dtcol = new DataTable();
				dtcol.Columns.Add("RecordNo", typeof(int));
				dtcol.Columns.Add("Id", typeof(int));
				dtcol.Columns.Add("DocumentTypeId", typeof(int));
				dtcol.Columns.Add("DocNo", typeof(string));
				dtcol.Columns.Add("DocDate", typeof(DateTime));
				dtcol.Columns.Add("ManualDocCode", typeof(string));
				dtcol.Columns.Add("StartDate", typeof(DateTime));
				dtcol.Columns.Add("EndDate", typeof(DateTime));
				dtcol.Columns.Add("DeadLine", typeof(string));
				dtcol.Columns.Add("BatchDesc", typeof(string));
				dtcol.Columns.Add("RemarksHeader", typeof(string));
				dtcol.Columns.Add("EntryUserName", typeof(string));
				dtcol.Columns.Add("EntryDate", typeof(DateTime));
				dtcol.Columns.Add("ModifyUserName", typeof(string));
				dtcol.Columns.Add("ModifyDate", typeof(DateTime));
				dtcol.Columns.Add("IsApproved", typeof(bool));
				dtcol.Columns.Add("NoOfAttachments", typeof(int));
				dtcol.Rows.Clear();
				foreach (DataRow row in dtHistory.Rows)
				{
					dtcol.Rows.Add(row["RecordNo"], row["Id"], row["DocumentTypeId"], row["DocNo"], row["DocDate"], row["ManualDocCode"], row["StartDate"], row["EndDate"], row["DeadLine"], row["BatchDesc"], row["RemarksHeader"], row["EntryUserName"], row["EntryDate"], row["ModifyUserName"], row["ModifyDate"], row["IsApproved"], row["NoOfAttachments"]);
				}
				DatagridHistory.DataSource = dtcol;
				DatagridHistory.RetrieveStructure();
				HistoryGridSetting();
			}
			else
			{
				DatagridHistory.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryGridSetting()
	{
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Expected O, but got Unknown
		try
		{
			DatagridHistory.RootTable.Columns["RecordNo"].Visible = false;
			DatagridHistory.RootTable.Columns["Id"].Visible = false;
			DatagridHistory.RootTable.Columns["DocumentTypeId"].Visible = false;
			DatagridHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			DatagridHistory.RootTable.HeaderLines = 3;
			foreach (GridEXColumn item in (JanusCollectionBase)DatagridHistory.RootTable.Columns)
			{
				item.HeaderAlignment = (TextAlignment)2;
				item.EditType = (EditType)0;
				item.FilterEditType = (FilterEditType)1;
				CommonServices.GridColumnSettings(item);
			}
			DatagridHistory.RootTable.Columns["RecordNo"].Width = Constants.InventoryConstants.RecordNo;
			DatagridHistory.RootTable.Columns["DocNo"].Width = Constants.InventoryConstants.DocNoConstant;
			DatagridHistory.RootTable.Columns["DocDate"].Width = Constants.InventoryConstants.DateConstant;
			DatagridHistory.RootTable.Columns["ManualDocCode"].Width = Constants.InventoryConstants.RecordNo;
			DatagridHistory.RootTable.Columns["StartDate"].Width = Constants.InventoryConstants.DateConstant;
			DatagridHistory.RootTable.Columns["EndDate"].Width = Constants.InventoryConstants.DateConstant;
			DatagridHistory.RootTable.Columns["DeadLine"].Width = Constants.InventoryConstants.DateConstant;
			DatagridHistory.RootTable.Columns["BatchDesc"].Width = Constants.InventoryConstants.OtherDescription;
			DatagridHistory.RootTable.Columns["RemarksHeader"].Width = Constants.InventoryConstants.RemarksConstants;
			DatagridHistory.RootTable.Columns["EntryUserName"].Width = Constants.InventoryConstants.EntryUserConstants;
			DatagridHistory.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateConstant;
			DatagridHistory.RootTable.Columns["ModifyUserName"].Width = Constants.InventoryConstants.ModifyUserConstants;
			DatagridHistory.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateConstant;
			DatagridHistory.RootTable.Columns["IsApproved"].Width = Constants.InventoryConstants.IsApproved;
			DatagridHistory.RootTable.Columns["NoOfAttachments"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants;
			DatagridHistory.RootTable.Columns.Add("Print");
			DatagridHistory.RootTable.Columns["Print"].Key = "Print";
			DatagridHistory.RootTable.Columns["Print"].Caption = "Print";
			DatagridHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DatagridHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			DatagridHistory.RootTable.Columns["Print"].ButtonText = "Print";
			DatagridHistory.RootTable.Columns["Print"].Width = 50;
			DatagridHistory.RootTable.Columns["Print"].Position = 0;
			DatagridHistory.RootTable.Columns.Add("Edit");
			DatagridHistory.RootTable.Columns["Edit"].Key = "Edit";
			DatagridHistory.RootTable.Columns["Edit"].Caption = "Edit";
			DatagridHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DatagridHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			DatagridHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			DatagridHistory.RootTable.Columns["Edit"].Width = 40;
			DatagridHistory.RootTable.Columns["Edit"].Position = 1;
			DatagridHistory.RootTable.Columns.Add("SaveAs");
			DatagridHistory.RootTable.Columns["SaveAs"].Key = "SaveAs";
			DatagridHistory.RootTable.Columns["SaveAs"].Caption = "SaveAs";
			DatagridHistory.RootTable.Columns["SaveAs"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DatagridHistory.RootTable.Columns["SaveAs"].ButtonStyle = (ButtonStyle)5;
			DatagridHistory.RootTable.Columns["SaveAs"].ButtonText = "SaveAs";
			DatagridHistory.RootTable.Columns["SaveAs"].Width = 60;
			DatagridHistory.RootTable.Columns["SaveAs"].Position = 2;
			DatagridHistory.FrozenColumns = 3;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DatagridHistory_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			if (formright.DoHaveUpdateRights)
			{
				ReadById(Conversion.ToInt((object)DatagridHistory.CurrentRow.Cells["Id"].Value.ToString()));
				return;
			}
			throw new Exception("ypu don't have update rights...");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DatagridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DatagridHistory.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0)
			{
				return;
			}
			if (e.Column.Key == "Print")
			{
				WorkOrderSlip(Conversion.ToInt((object)r.Cells["Id"].Value.ToString()));
			}
			if (e.Column.Key == "Edit")
			{
				if (!formright.DoHaveUpdateRights)
				{
					throw new Exception("you don't have update rights...");
				}
				ReadById(Conversion.ToInt((object)r.Cells["Id"].Value.ToString()));
			}
			if (e.Column.Key == "SaveAs")
			{
				DataGridHistory_SaveAs(Conversion.ToInt(r.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DatagridHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DatagridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0 && e.Column.Key == "NoOfAttachments")
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(r.Cells["Id"].Value), 1621);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_SaveAs(int CurrId)
	{
		try
		{
			ReadById(CurrId);
			btnsave.Visible = false;
			btnDelete.Visible = false;
			btnUpdate.Visible = false;
			btnSaveAs.Visible = true;
			txtdocdate.Value = DateTime.Now;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DatagridHistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DatagridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				DatagridHistoryDetailbind(Conversion.ToInt(r.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DatagridHistoryDetailbind(int Id)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		try
		{
			grdhistoryDetail.ClearStructure();
			WorkOrder Or = new WorkOrder();
			Or = WorkOrder.ReadById(Id);
			if (Or == null)
			{
				return;
			}
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("OrderId", typeof(int));
			dt.Columns.Add("OrderNo", typeof(int));
			dt.Columns.Add("ItemId", typeof(int));
			dt.Columns.Add("ItemName", typeof(string));
			dt.Columns.Add("UomId", typeof(int));
			dt.Columns.Add("Uom", typeof(string));
			dt.Columns.Add("VariantId", typeof(int));
			dt.Columns.Add("VariantDescription", typeof(string));
			dt.Columns.Add("CastingTypeId", typeof(int));
			dt.Columns.Add("CastingType", typeof(string));
			dt.Columns.Add("Qty", typeof(decimal));
			dt.Columns.Add("WeightPerQty", typeof(decimal));
			dt.Columns.Add("CastingWeight", typeof(decimal));
			if (Or.workOrderItemDetailList.Count > 0)
			{
				for (int i = 0; i < Or.workOrderItemDetailList.Count; i++)
				{
					decimal ItemWeight = default(decimal);
					ItemWeight = ((Or.workOrderItemDetailList[i].CastingTypeId != 1) ? Conversion.ToDecimal((object)Or.workOrderItemDetailList[i].DyeWeight) : Conversion.ToDecimal((object)Or.workOrderItemDetailList[i].SandWeight));
					dt.Rows.Add(Or.workOrderItemDetailList[i].workOrderItemDetailId, Or.workOrderItemDetailList[i].OrderId, Or.workOrderItemDetailList[i].OrderNo, Or.workOrderItemDetailList[i].ItemId, Or.workOrderItemDetailList[i].ItemName, Or.workOrderItemDetailList[i].UomId, Or.workOrderItemDetailList[i].Uom, Or.workOrderItemDetailList[i].ItemVariantId, Or.workOrderItemDetailList[i].VariantDescription, Or.workOrderItemDetailList[i].CastingTypeId, Or.workOrderItemDetailList[i].CastingType, Or.workOrderItemDetailList[i].Qty, ItemWeight, ItemWeight * Or.workOrderItemDetailList[i].Qty);
				}
			}
			else
			{
				grdhistoryDetail.ClearStructure();
			}
			grdhistoryDetail.DataSource = dt;
			grdhistoryDetail.RetrieveStructure();
			grdHistoryDetailetting();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistoryDetailetting()
	{
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Expected O, but got Unknown
		try
		{
			grdhistoryDetail.RootTable.Columns["Id"].Visible = false;
			grdhistoryDetail.RootTable.Columns["OrderId"].Visible = false;
			grdhistoryDetail.RootTable.Columns["ItemId"].Visible = false;
			grdhistoryDetail.RootTable.Columns["UomId"].Visible = false;
			grdhistoryDetail.RootTable.Columns["CastingTypeId"].Visible = false;
			grdhistoryDetail.RootTable.Columns["VariantId"].Visible = false;
			grdhistoryDetail.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grdhistoryDetail.RootTable.Columns)
			{
				item.HeaderAlignment = (TextAlignment)2;
				item.EditType = (EditType)0;
				item.FilterEditType = (FilterEditType)1;
				CommonServices.GridColumnSettings(item);
			}
			grdhistoryDetail.RootTable.Columns["OrderNo"].Width = Constants.InventoryConstants.OrderNo;
			grdhistoryDetail.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			grdhistoryDetail.RootTable.Columns["Uom"].Width = Constants.InventoryConstants.UomConstant;
			grdhistoryDetail.RootTable.Columns["VariantDescription"].Width = Constants.InventoryConstants.VariantEquivalent;
			grdhistoryDetail.RootTable.Columns["CastingType"].Width = Constants.InventoryConstants.ItemType;
			grdhistoryDetail.RootTable.Columns["Qty"].Width = Constants.InventoryConstants.QtyConstant;
			grdhistoryDetail.RootTable.Columns["WeightPerQty"].Width = Constants.InventoryConstants.WeightConstant;
			grdhistoryDetail.RootTable.Columns["CastingWeight"].Width = Constants.InventoryConstants.WeightConstant;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnHistoryRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			grdhistoryDetail.ClearStructure();
			txtFromdateHistory.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmBillOfMaterial_Manufacturing_KeyDown(object sender, KeyEventArgs e)
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
					txtdocdate.Focus();
				}
				else
				{
					tabControl1.SelectedIndex = 1;
					((Control)(object)DatagridHistory).Focus();
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
				if (e.Control && e.Shift && e.KeyCode == Keys.Delete)
				{
					if (formright.DoHaveCanDelete && btnDelete.Enabled)
					{
						btnDelete_Click(null, null);
					}
				}
				else if (e.Control && e.KeyCode == Keys.S)
				{
					if (btnsave.Visible && btnsave.Enabled)
					{
						btnsave_Click(null, null);
					}
				}
				else if (e.Control && e.KeyCode == Keys.U)
				{
					if (btnUpdate.Visible && btnUpdate.Enabled && UpdateMode)
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
					btnRefresh_Click(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.F5)
				{
					txtdocdate.Focus();
				}
				else if (e.Control && e.KeyCode == Keys.F10)
				{
					AT.Show();
				}
				else if (e.Control && e.KeyCode == Keys.F12)
				{
					if (btnSaveAs.Visible && btnSaveAs.Enabled)
					{
						btnSaveAs_Click(null, null);
					}
				}
				else if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grdDetail).Focus();
				}
				else if (e.Control && e.KeyCode == Keys.Up)
				{
					txtdocdate.Focus();
				}
				else if (e.Control && e.KeyCode != Keys.Right)
				{
				}
			}
			else if (tabControl1.SelectedIndex == 1)
			{
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grdhistoryDetail).Focus();
				}
				else if (e.Control && e.KeyCode == Keys.S)
				{
					btnshow_Click(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.Up)
				{
					((Control)(object)DatagridHistory).Focus();
				}
				else if (e.Control)
				{
					_ = e.KeyCode;
					_ = 39;
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

	private void grdDetail_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdDetail.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdDetail.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
				if (grdDetail.CurrentColumn.Key == "Delete")
				{
					DeleteOperationDetailRow(r);
				}
				if (grdDetail.CurrentColumn.Key == "Edit")
				{
					grdDetail_DoubleClick(null, null);
				}
				grdDetail.UpdateData();
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				DeleteOperationDetailRow(r);
				grdDetail.UpdateData();
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				grdDetail_DoubleClick(null, null);
			}
			if (e.KeyCode == Keys.F1 && grdDetail.CurrentColumn != null)
			{
				_ = grdDetail.CurrentColumn.Key == "OverHeadAccountId";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DatagridHistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DatagridHistory.CurrentRow;
			if (r == null || (int)r.RowType != 0 || DatagridHistory.CurrentColumn == null)
			{
				return;
			}
			int CurrId = Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (DatagridHistory.CurrentColumn.Key == "Edit")
				{
					if (!formright.DoHaveUpdateRights)
					{
						throw new Exception("you don't have update rights...");
					}
					ReadById(Conversion.ToInt((object)CurrId));
				}
				if (DatagridHistory.CurrentColumn.Key == "Print")
				{
					WorkOrderSlip(CurrId);
				}
				if (DatagridHistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(CurrId, 1621);
				}
				if (DatagridHistory.CurrentColumn.Key == "SaveAs")
				{
					DataGridHistory_SaveAs(CurrId);
				}
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				if (!formright.DoHaveUpdateRights)
				{
					throw new Exception("you don't have update rights...");
				}
				ReadById(Conversion.ToInt((object)CurrId));
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)DatagridHistory).Name);
			if (fs != null)
			{
				DatagridHistory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
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

	private void BtnDefineVariant_Click(object sender, EventArgs e)
	{
		try
		{
			ItemAttributeVarient_Engr itemAttributeVarient_Engr = new ItemAttributeVarient_Engr(UserAccount);
			itemAttributeVarient_Engr.CmbItem.Value = Conversion.ToInt(CmbItem.Value);
			itemAttributeVarient_Engr.Show();
			bindvarientunit(Conversion.ToInt(CmbItem.Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnDefineItem_Click(object sender, EventArgs e)
	{
		try
		{
			new frmAddItem_Engr(UserAccount).Show();
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
		//IL_027f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0289: Expected O, but got Unknown
		//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Expected O, but got Unknown
		//IL_02d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e1: Expected O, but got Unknown
		//IL_02f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0302: Expected O, but got Unknown
		//IL_030e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0318: Expected O, but got Unknown
		//IL_0324: Unknown result type (might be due to invalid IL or missing references)
		//IL_032e: Expected O, but got Unknown
		//IL_032f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0339: Expected O, but got Unknown
		//IL_0345: Unknown result type (might be due to invalid IL or missing references)
		//IL_034f: Expected O, but got Unknown
		//IL_053f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0549: Expected O, but got Unknown
		//IL_0560: Unknown result type (might be due to invalid IL or missing references)
		//IL_056a: Expected O, but got Unknown
		//IL_0a66: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a70: Expected O, but got Unknown
		//IL_0a7d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a87: Expected O, but got Unknown
		//IL_4af0: Unknown result type (might be due to invalid IL or missing references)
		//IL_4afa: Expected O, but got Unknown
		//IL_4b1e: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b28: Expected O, but got Unknown
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance1 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
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
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		GridEXLayout DatagridHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdhistoryDetail_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.grdDetail = new GridEX();
		this.panel14 = new System.Windows.Forms.Panel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.CmbSampleModal = new UltraCombo();
		this.label29 = new System.Windows.Forms.Label();
		this.BtnDefineVariant = new System.Windows.Forms.Button();
		this.rdbtnItemCode = new System.Windows.Forms.RadioButton();
		this.rdbtnItemName = new System.Windows.Forms.RadioButton();
		this.CmbCategory = new UltraCombo();
		this.label52 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.CmbItem = new UltraCombo();
		this.label26 = new System.Windows.Forms.Label();
		this.CmbUomDetail = new UltraCombo();
		this.BtnAddOp = new System.Windows.Forms.Button();
		this.CmbCastingTypeDetail = new UltraCombo();
		this.CmbVariantDescription = new UltraCombo();
		this.label34 = new System.Windows.Forms.Label();
		this.CmbOrderNoDetail = new UltraCombo();
		this.label2 = new System.Windows.Forms.Label();
		this.BtnCancelOp = new System.Windows.Forms.Button();
		this.label5 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.label31 = new System.Windows.Forms.Label();
		this.BtnUpdateOp = new System.Windows.Forms.Button();
		this.txtCastingWeight = new System.Windows.Forms.TextBox();
		this.txtItemWeight = new System.Windows.Forms.TextBox();
		this.txtQtyDetaill = new System.Windows.Forms.TextBox();
		this.panel2 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.txtEndDate = new System.Windows.Forms.DateTimePicker();
		this.txtDeadLineDateMain = new System.Windows.Forms.DateTimePicker();
		this.txtRemarksHeader = new System.Windows.Forms.TextBox();
		this.txtWorkOrderNo = new System.Windows.Forms.TextBox();
		this.txtLeadTimeHeader = new System.Windows.Forms.TextBox();
		this.txtStartdate = new System.Windows.Forms.DateTimePicker();
		this.txtdocdate = new System.Windows.Forms.DateTimePicker();
		this.txtBatchDesc = new System.Windows.Forms.TextBox();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.label25 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.label23 = new System.Windows.Forms.Label();
		this.label37 = new System.Windows.Forms.Label();
		this.label24 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnSaveAs = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.btnshortcutkeys = new System.Windows.Forms.ToolStripButton();
		this.BtnDefineItem = new System.Windows.Forms.ToolStripButton();
		this.label21 = new System.Windows.Forms.Label();
		this.ChkPreview = new System.Windows.Forms.CheckBox();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.DatagridHistory = new GridEX();
		this.panel9 = new System.Windows.Forms.Panel();
		this.label15 = new System.Windows.Forms.Label();
		this.grdhistoryDetail = new GridEX();
		this.panel15 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label17 = new System.Windows.Forms.Label();
		this.panel13 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.btnshow = new System.Windows.Forms.Button();
		this.txtToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.label1 = new System.Windows.Forms.Label();
		this.label28 = new System.Windows.Forms.Label();
		this.txtToDocNoHistory = new System.Windows.Forms.TextBox();
		this.txtFromdateHistory = new System.Windows.Forms.DateTimePicker();
		this.label3 = new System.Windows.Forms.Label();
		this.label35 = new System.Windows.Forms.Label();
		this.txtFromNoHistory = new System.Windows.Forms.TextBox();
		this.panel38 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.btnRegister = new System.Windows.Forms.ToolStripButton();
		this.label32 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbSampleModal).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCategory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbUomDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCastingTypeDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbVariantDescription).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbOrderNoDetail).BeginInit();
		this.panel2.SuspendLayout();
		this.groupBox2.SuspendLayout();
		this.panel3.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DatagridHistory).BeginInit();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdhistoryDetail).BeginInit();
		this.panel15.SuspendLayout();
		this.panel13.SuspendLayout();
		this.groupBox1.SuspendLayout();
		this.panel38.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(934, 595);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage3);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(934, 595);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.tabPage1.Controls.Add(this.panel14);
		this.tabPage1.Controls.Add(this.panel5);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(926, 566);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.grdDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdDetail).BackColor = System.Drawing.Color.White;
		grdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(3, 280);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(920, 283);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 1;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.grdDetail.CellUpdated += new ColumnActionEventHandler(grdDetail_CellUpdated);
		this.grdDetail.ColumnButtonClick += new ColumnActionEventHandler(grdDetail_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdDetail).DoubleClick += new System.EventHandler(grdDetail_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdDetail).KeyDown += new System.Windows.Forms.KeyEventHandler(grdDetail_KeyDown);
		this.panel14.BackColor = System.Drawing.Color.Teal;
		this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel14.Location = new System.Drawing.Point(3, 257);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(920, 23);
		this.panel14.TabIndex = 4;
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSampleModal);
		this.panel5.Controls.Add(this.label29);
		this.panel5.Controls.Add(this.BtnDefineVariant);
		this.panel5.Controls.Add(this.rdbtnItemCode);
		this.panel5.Controls.Add(this.rdbtnItemName);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCategory);
		this.panel5.Controls.Add(this.label52);
		this.panel5.Controls.Add(this.label6);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItem);
		this.panel5.Controls.Add(this.label26);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbUomDetail);
		this.panel5.Controls.Add(this.BtnAddOp);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCastingTypeDetail);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbVariantDescription);
		this.panel5.Controls.Add(this.label34);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbOrderNoDetail);
		this.panel5.Controls.Add(this.label2);
		this.panel5.Controls.Add(this.BtnCancelOp);
		this.panel5.Controls.Add(this.label5);
		this.panel5.Controls.Add(this.label4);
		this.panel5.Controls.Add(this.label33);
		this.panel5.Controls.Add(this.label31);
		this.panel5.Controls.Add(this.BtnUpdateOp);
		this.panel5.Controls.Add(this.txtCastingWeight);
		this.panel5.Controls.Add(this.txtItemWeight);
		this.panel5.Controls.Add(this.txtQtyDetaill);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(3, 156);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(920, 101);
		this.panel5.TabIndex = 0;
		this.CmbSampleModal.AllowNull = (DefaultableBoolean)1;
		this.CmbSampleModal.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSampleModal.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSampleModal.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSampleModal).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbSampleModal).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSampleModal).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance45).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance45).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSampleModal).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance66).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSampleModal).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance66;
		((SpecialBoxBase)((UltraGridBase)this.CmbSampleModal).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance67).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance67).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSampleModal).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.CmbSampleModal).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSampleModal).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSampleModal).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance68;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSampleModal).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.CmbSampleModal).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSampleModal).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSampleModal).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance71).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSampleModal).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbSampleModal).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSampleModal).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance74).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSampleModal).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSampleModal).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbSampleModal).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSampleModal).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSampleModal).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbSampleModal).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSampleModal).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbSampleModal).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSampleModal).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSampleModal).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSampleModal).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSampleModal.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSampleModal).Location = new System.Drawing.Point(288, 25);
		((System.Windows.Forms.Control)(object)this.CmbSampleModal).Name = "CmbSampleModal";
		((System.Windows.Forms.Control)(object)this.CmbSampleModal).Size = new System.Drawing.Size(114, 26);
		((System.Windows.Forms.Control)(object)this.CmbSampleModal).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbSampleModal).Leave += new System.EventHandler(CmbSampleModal_Leave);
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label29.Location = new System.Drawing.Point(291, 7);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(84, 15);
		this.label29.TabIndex = 601;
		this.label29.Text = "Modal Sample";
		this.BtnDefineVariant.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.BtnDefineVariant.Location = new System.Drawing.Point(887, 25);
		this.BtnDefineVariant.Name = "BtnDefineVariant";
		this.BtnDefineVariant.Size = new System.Drawing.Size(29, 26);
		this.BtnDefineVariant.TabIndex = 6;
		this.BtnDefineVariant.TabStop = false;
		this.BtnDefineVariant.UseVisualStyleBackColor = true;
		this.BtnDefineVariant.Click += new System.EventHandler(BtnDefineVariant_Click);
		this.rdbtnItemCode.AutoSize = true;
		this.rdbtnItemCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdbtnItemCode.Location = new System.Drawing.Point(505, 5);
		this.rdbtnItemCode.Margin = new System.Windows.Forms.Padding(2);
		this.rdbtnItemCode.Name = "rdbtnItemCode";
		this.rdbtnItemCode.Size = new System.Drawing.Size(52, 19);
		this.rdbtnItemCode.TabIndex = 150;
		this.rdbtnItemCode.Text = "Code";
		this.rdbtnItemCode.UseVisualStyleBackColor = true;
		this.rdbtnItemName.AutoSize = true;
		this.rdbtnItemName.Checked = true;
		this.rdbtnItemName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdbtnItemName.Location = new System.Drawing.Point(444, 5);
		this.rdbtnItemName.Margin = new System.Windows.Forms.Padding(2);
		this.rdbtnItemName.Name = "rdbtnItemName";
		this.rdbtnItemName.Size = new System.Drawing.Size(57, 19);
		this.rdbtnItemName.TabIndex = 149;
		this.rdbtnItemName.TabStop = true;
		this.rdbtnItemName.Text = "Name";
		this.rdbtnItemName.UseVisualStyleBackColor = true;
		this.rdbtnItemName.CheckedChanged += new System.EventHandler(rdbtnItemName_CheckedChanged);
		this.CmbCategory.AllowNull = (DefaultableBoolean)1;
		this.CmbCategory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCategory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCategory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance54).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.CmbCategory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCategory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance55).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCategory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance55;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCategory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance56;
		((SpecialBoxBase)((UltraGridBase)this.CmbCategory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance57).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance57).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCategory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.CmbCategory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCategory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance58;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance59).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance60;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance61).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance62).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance62).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance62).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance62).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance62;
		((AppearanceBase)appearance63).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance64).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.CmbCategory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCategory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCategory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCategory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCategory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCategory).Location = new System.Drawing.Point(103, 25);
		((System.Windows.Forms.Control)(object)this.CmbCategory).Name = "CmbCategory";
		((System.Windows.Forms.Control)(object)this.CmbCategory).Size = new System.Drawing.Size(183, 26);
		((System.Windows.Forms.Control)(object)this.CmbCategory).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.CmbCategory).Leave += new System.EventHandler(CmbCategory_Leave);
		this.label52.AutoSize = true;
		this.label52.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label52.Location = new System.Drawing.Point(103, 7);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(82, 15);
		this.label52.TabIndex = 147;
		this.label52.Text = "Item Category";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(406, 7);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(32, 15);
		this.label6.TabIndex = 148;
		this.label6.Text = "Item";
		this.CmbItem.AllowNull = (DefaultableBoolean)1;
		this.CmbItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance4).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance4).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance4).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance5;
		((SpecialBoxBase)((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.CmbItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance7;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance10).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance11).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance11).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance11).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance11).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance12).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItem).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItem).Location = new System.Drawing.Point(403, 26);
		((System.Windows.Forms.Control)(object)this.CmbItem).Name = "CmbItem";
		((System.Windows.Forms.Control)(object)this.CmbItem).Size = new System.Drawing.Size(213, 25);
		((System.Windows.Forms.Control)(object)this.CmbItem).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.CmbItem).Leave += new System.EventHandler(cmbItem_Leave);
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(617, 7);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(35, 15);
		this.label26.TabIndex = 144;
		this.label26.Text = "UOM";
		this.CmbUomDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbUomDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbUomDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbUomDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbUomDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.CmbUomDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbUomDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance73).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance73).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbUomDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance73;
		((AppearanceBase)appearance75).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbUomDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance75;
		((SpecialBoxBase)((UltraGridBase)this.CmbUomDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance76).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance76).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance76).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbUomDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.CmbUomDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbUomDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance77).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbUomDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance77;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance78).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbUomDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.CmbUomDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbUomDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbUomDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance80).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbUomDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.CmbUomDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbUomDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance81).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance81).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance81).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbUomDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance81;
		((AppearanceBase)appearance82).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbUomDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.CmbUomDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbUomDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbUomDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.CmbUomDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbUomDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.CmbUomDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbUomDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbUomDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbUomDetail).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbUomDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbUomDetail).Location = new System.Drawing.Point(618, 26);
		((System.Windows.Forms.Control)(object)this.CmbUomDetail).Name = "CmbUomDetail";
		((System.Windows.Forms.Control)(object)this.CmbUomDetail).Size = new System.Drawing.Size(81, 25);
		((System.Windows.Forms.Control)(object)this.CmbUomDetail).TabIndex = 4;
		this.BtnAddOp.BackColor = System.Drawing.Color.Teal;
		this.BtnAddOp.FlatAppearance.BorderSize = 0;
		this.BtnAddOp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnAddOp.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnAddOp.ForeColor = System.Drawing.Color.White;
		this.BtnAddOp.Location = new System.Drawing.Point(566, 73);
		this.BtnAddOp.Name = "BtnAddOp";
		this.BtnAddOp.Size = new System.Drawing.Size(39, 22);
		this.BtnAddOp.TabIndex = 11;
		this.BtnAddOp.Text = "+";
		this.BtnAddOp.UseVisualStyleBackColor = false;
		this.BtnAddOp.Click += new System.EventHandler(BtnAddOp_Click);
		this.BtnAddOp.Leave += new System.EventHandler(BtnAddOp_Leave);
		this.CmbCastingTypeDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbCastingTypeDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCastingTypeDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCastingTypeDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCastingTypeDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbCastingTypeDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCastingTypeDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCastingTypeDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance42;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCastingTypeDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance43;
		((SpecialBoxBase)((UltraGridBase)this.CmbCastingTypeDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance44).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance44).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCastingTypeDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbCastingTypeDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCastingTypeDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance46).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCastingTypeDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCastingTypeDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbCastingTypeDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCastingTypeDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCastingTypeDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance49).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCastingTypeDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbCastingTypeDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCastingTypeDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance50).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance50).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance50).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCastingTypeDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance50;
		((AppearanceBase)appearance51).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCastingTypeDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbCastingTypeDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCastingTypeDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCastingTypeDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.CmbCastingTypeDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCastingTypeDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.CmbCastingTypeDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCastingTypeDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCastingTypeDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCastingTypeDetail).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCastingTypeDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCastingTypeDetail).Location = new System.Drawing.Point(3, 71);
		((System.Windows.Forms.Control)(object)this.CmbCastingTypeDetail).Name = "CmbCastingTypeDetail";
		((System.Windows.Forms.Control)(object)this.CmbCastingTypeDetail).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.CmbCastingTypeDetail).TabIndex = 7;
		((System.Windows.Forms.Control)(object)this.CmbCastingTypeDetail).Leave += new System.EventHandler(CmbCastingTypeDetail_Leave);
		this.CmbVariantDescription.AllowNull = (DefaultableBoolean)1;
		this.CmbVariantDescription.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbVariantDescription.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbVariantDescription.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbVariantDescription).DisplayLayout.Appearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbVariantDescription).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbVariantDescription).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance31).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance31).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbVariantDescription).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbVariantDescription).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance32;
		((SpecialBoxBase)((UltraGridBase)this.CmbVariantDescription).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance33).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance33).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbVariantDescription).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbVariantDescription).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbVariantDescription).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbVariantDescription).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbVariantDescription).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbVariantDescription).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbVariantDescription).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbVariantDescription).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance37).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbVariantDescription).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbVariantDescription).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbVariantDescription).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance38).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance38).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance38).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbVariantDescription).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbVariantDescription).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbVariantDescription).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbVariantDescription).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbVariantDescription).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.CmbVariantDescription).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbVariantDescription).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.CmbVariantDescription).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbVariantDescription).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbVariantDescription).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbVariantDescription).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbVariantDescription.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbVariantDescription).Location = new System.Drawing.Point(700, 25);
		((System.Windows.Forms.Control)(object)this.CmbVariantDescription).Name = "CmbVariantDescription";
		((System.Windows.Forms.Control)(object)this.CmbVariantDescription).Size = new System.Drawing.Size(186, 26);
		((System.Windows.Forms.Control)(object)this.CmbVariantDescription).TabIndex = 5;
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label34.Location = new System.Drawing.Point(3, 54);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(74, 15);
		this.label34.TabIndex = 19;
		this.label34.Text = "Casting Type";
		this.CmbOrderNoDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbOrderNoDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbOrderNoDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbOrderNoDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbOrderNoDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbOrderNoDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbOrderNoDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance18).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance18).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbOrderNoDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance18;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOrderNoDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance19;
		((SpecialBoxBase)((UltraGridBase)this.CmbOrderNoDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance20).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance20).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOrderNoDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbOrderNoDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbOrderNoDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbOrderNoDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbOrderNoDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbOrderNoDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbOrderNoDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOrderNoDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance24).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbOrderNoDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbOrderNoDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbOrderNoDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance25).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance25).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance25).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOrderNoDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance27).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbOrderNoDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.CmbOrderNoDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbOrderNoDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbOrderNoDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbOrderNoDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbOrderNoDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbOrderNoDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbOrderNoDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbOrderNoDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbOrderNoDetail).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbOrderNoDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbOrderNoDetail).Location = new System.Drawing.Point(3, 25);
		((System.Windows.Forms.Control)(object)this.CmbOrderNoDetail).Name = "CmbOrderNoDetail";
		((System.Windows.Forms.Control)(object)this.CmbOrderNoDetail).Size = new System.Drawing.Size(98, 26);
		((System.Windows.Forms.Control)(object)this.CmbOrderNoDetail).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbOrderNoDetail).Leave += new System.EventHandler(CmbOrderNoDetail_Leave);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label2.Location = new System.Drawing.Point(3, 7);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(56, 15);
		this.label2.TabIndex = 19;
		this.label2.Text = "Order No";
		this.BtnCancelOp.BackColor = System.Drawing.Color.Teal;
		this.BtnCancelOp.FlatAppearance.BorderSize = 0;
		this.BtnCancelOp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnCancelOp.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnCancelOp.ForeColor = System.Drawing.Color.White;
		this.BtnCancelOp.Location = new System.Drawing.Point(632, 73);
		this.BtnCancelOp.Name = "BtnCancelOp";
		this.BtnCancelOp.Size = new System.Drawing.Size(64, 22);
		this.BtnCancelOp.TabIndex = 12;
		this.BtnCancelOp.Text = "Cancel";
		this.BtnCancelOp.UseVisualStyleBackColor = false;
		this.BtnCancelOp.Visible = false;
		this.BtnCancelOp.Click += new System.EventHandler(BtnCancelOp_Click);
		this.BtnCancelOp.Leave += new System.EventHandler(BtnCancelOp_Leave);
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label5.Location = new System.Drawing.Point(459, 54);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(88, 15);
		this.label5.TabIndex = 25;
		this.label5.Text = "Casting Weight";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label4.Location = new System.Drawing.Point(353, 54);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(88, 15);
		this.label4.TabIndex = 25;
		this.label4.Text = "Weight Per Qty";
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label33.Location = new System.Drawing.Point(225, 54);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(26, 15);
		this.label33.TabIndex = 25;
		this.label33.Text = "Qty";
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.Location = new System.Drawing.Point(700, 7);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(105, 15);
		this.label31.TabIndex = 87;
		this.label31.Text = "Modal Description";
		this.BtnUpdateOp.BackColor = System.Drawing.Color.Teal;
		this.BtnUpdateOp.FlatAppearance.BorderSize = 0;
		this.BtnUpdateOp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnUpdateOp.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnUpdateOp.ForeColor = System.Drawing.Color.White;
		this.BtnUpdateOp.Location = new System.Drawing.Point(566, 73);
		this.BtnUpdateOp.Name = "BtnUpdateOp";
		this.BtnUpdateOp.Size = new System.Drawing.Size(64, 22);
		this.BtnUpdateOp.TabIndex = 11;
		this.BtnUpdateOp.Text = "Update";
		this.BtnUpdateOp.UseVisualStyleBackColor = false;
		this.BtnUpdateOp.Visible = false;
		this.BtnUpdateOp.Click += new System.EventHandler(BtnUpdateOp_Click);
		this.txtCastingWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCastingWeight.Enabled = false;
		this.txtCastingWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCastingWeight.Location = new System.Drawing.Point(459, 73);
		this.txtCastingWeight.Name = "txtCastingWeight";
		this.txtCastingWeight.Size = new System.Drawing.Size(104, 23);
		this.txtCastingWeight.TabIndex = 10;
		this.txtCastingWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtQtyDetail_KeyPress);
		this.txtItemWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemWeight.Enabled = false;
		this.txtItemWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemWeight.Location = new System.Drawing.Point(353, 73);
		this.txtItemWeight.Name = "txtItemWeight";
		this.txtItemWeight.Size = new System.Drawing.Size(104, 23);
		this.txtItemWeight.TabIndex = 9;
		this.txtItemWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtQtyDetail_KeyPress);
		this.txtQtyDetaill.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtQtyDetaill.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtQtyDetaill.Location = new System.Drawing.Point(225, 73);
		this.txtQtyDetaill.Name = "txtQtyDetaill";
		this.txtQtyDetaill.Size = new System.Drawing.Size(125, 23);
		this.txtQtyDetaill.TabIndex = 8;
		this.txtQtyDetaill.TextChanged += new System.EventHandler(txtQtyDetaill_TextChanged);
		this.txtQtyDetaill.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtQtyDetail_KeyPress);
		this.panel2.Controls.Add(this.groupBox2);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 58);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(920, 98);
		this.panel2.TabIndex = 0;
		this.groupBox2.Controls.Add(this.txtEndDate);
		this.groupBox2.Controls.Add(this.txtDeadLineDateMain);
		this.groupBox2.Controls.Add(this.txtRemarksHeader);
		this.groupBox2.Controls.Add(this.txtWorkOrderNo);
		this.groupBox2.Controls.Add(this.txtLeadTimeHeader);
		this.groupBox2.Controls.Add(this.txtStartdate);
		this.groupBox2.Controls.Add(this.txtdocdate);
		this.groupBox2.Controls.Add(this.txtBatchDesc);
		this.groupBox2.Controls.Add(this.txtdocno);
		this.groupBox2.Controls.Add(this.label25);
		this.groupBox2.Controls.Add(this.label9);
		this.groupBox2.Controls.Add(this.label23);
		this.groupBox2.Controls.Add(this.label37);
		this.groupBox2.Controls.Add(this.label24);
		this.groupBox2.Controls.Add(this.label8);
		this.groupBox2.Controls.Add(this.label12);
		this.groupBox2.Controls.Add(this.label13);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.groupBox2.Location = new System.Drawing.Point(0, 0);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(917, 93);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.txtEndDate.CustomFormat = "dd-MMM-yy";
		this.txtEndDate.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtEndDate.Location = new System.Drawing.Point(357, 41);
		this.txtEndDate.Name = "txtEndDate";
		this.txtEndDate.Size = new System.Drawing.Size(107, 22);
		this.txtEndDate.TabIndex = 5;
		this.txtDeadLineDateMain.CustomFormat = "dd-MMM-yy";
		this.txtDeadLineDateMain.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtDeadLineDateMain.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDeadLineDateMain.Location = new System.Drawing.Point(357, 66);
		this.txtDeadLineDateMain.Name = "txtDeadLineDateMain";
		this.txtDeadLineDateMain.Size = new System.Drawing.Size(107, 22);
		this.txtDeadLineDateMain.TabIndex = 6;
		this.txtRemarksHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarksHeader.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksHeader.Location = new System.Drawing.Point(536, 41);
		this.txtRemarksHeader.Multiline = true;
		this.txtRemarksHeader.Name = "txtRemarksHeader";
		this.txtRemarksHeader.Size = new System.Drawing.Size(379, 47);
		this.txtRemarksHeader.TabIndex = 8;
		this.txtRemarksHeader.Leave += new System.EventHandler(txtRemarksHeader_Leave);
		this.txtWorkOrderNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtWorkOrderNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtWorkOrderNo.Location = new System.Drawing.Point(76, 41);
		this.txtWorkOrderNo.Name = "txtWorkOrderNo";
		this.txtWorkOrderNo.Size = new System.Drawing.Size(216, 23);
		this.txtWorkOrderNo.TabIndex = 2;
		this.txtLeadTimeHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtLeadTimeHeader.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtLeadTimeHeader.Location = new System.Drawing.Point(76, 66);
		this.txtLeadTimeHeader.Name = "txtLeadTimeHeader";
		this.txtLeadTimeHeader.Size = new System.Drawing.Size(216, 23);
		this.txtLeadTimeHeader.TabIndex = 3;
		this.txtLeadTimeHeader.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtLeadTimeHeader_KeyPress);
		this.txtStartdate.CustomFormat = "dd-MMM-yy";
		this.txtStartdate.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtStartdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtStartdate.Location = new System.Drawing.Point(357, 16);
		this.txtStartdate.Name = "txtStartdate";
		this.txtStartdate.Size = new System.Drawing.Size(107, 22);
		this.txtStartdate.TabIndex = 4;
		this.txtdocdate.CustomFormat = "dd-MMM-yy";
		this.txtdocdate.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtdocdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtdocdate.Location = new System.Drawing.Point(185, 16);
		this.txtdocdate.Name = "txtdocdate";
		this.txtdocdate.Size = new System.Drawing.Size(107, 22);
		this.txtdocdate.TabIndex = 1;
		this.txtBatchDesc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBatchDesc.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBatchDesc.Location = new System.Drawing.Point(536, 16);
		this.txtBatchDesc.Name = "txtBatchDesc";
		this.txtBatchDesc.Size = new System.Drawing.Size(379, 23);
		this.txtBatchDesc.TabIndex = 7;
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocno.Location = new System.Drawing.Point(76, 16);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.Size = new System.Drawing.Size(107, 23);
		this.txtdocno.TabIndex = 0;
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(295, 70);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(60, 15);
		this.label25.TabIndex = 138;
		this.label25.Text = "Dead Line";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(295, 45);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(55, 15);
		this.label9.TabIndex = 138;
		this.label9.Text = "End Date";
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.Location = new System.Drawing.Point(295, 20);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(60, 15);
		this.label23.TabIndex = 138;
		this.label23.Text = "Start Date";
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(1, 20);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(76, 15);
		this.label37.TabIndex = 138;
		this.label37.Text = "Doc No,Date";
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(1, 70);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(62, 15);
		this.label24.TabIndex = 131;
		this.label24.Text = "Lead Time";
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(468, 44);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(66, 39);
		this.label8.TabIndex = 131;
		this.label8.Text = "Remarks / Instructions";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(1, 45);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(70, 15);
		this.label12.TabIndex = 131;
		this.label12.Text = "W.Order No";
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(468, 20);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(66, 15);
		this.label13.TabIndex = 87;
		this.label13.Text = "Batch Desc";
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.toolStrip1);
		this.panel3.Controls.Add(this.label21);
		this.panel3.Controls.Add(this.ChkPreview);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(3, 3);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(920, 55);
		this.panel3.TabIndex = 0;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[11]
		{
			this.btnnew, this.btnRefresh, this.btnsave, this.btnSaveAs, this.btnUpdate, this.btnDelete, this.toolStripSeparator, this.btnAttachment, this.btnPrint, this.btnshortcutkeys,
			this.BtnDefineItem
		});
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(920, 27);
		this.toolStrip1.TabIndex = 2;
		this.toolStrip1.Text = "toolStrip1";
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
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnSaveAs.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSaveAs.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSaveAs.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSaveAs.Name = "btnSaveAs";
		this.btnSaveAs.Size = new System.Drawing.Size(80, 24);
		this.btnSaveAs.Text = "SaveAs";
		this.btnSaveAs.Visible = false;
		this.btnSaveAs.Click += new System.EventHandler(btnSaveAs_Click);
		this.btnUpdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnDelete.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(73, 24);
		this.btnDelete.Text = "&Delete";
		this.btnDelete.Visible = false;
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.toolStripSeparator.Name = "toolStripSeparator";
		this.toolStripSeparator.Size = new System.Drawing.Size(6, 27);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(109, 24);
		this.btnAttachment.Text = "&Attachment";
		this.btnAttachment.ToolTipText = "Alt_A";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.btnPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(100, 24);
		this.btnPrint.Text = "1621-&Print";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.btnshortcutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshortcutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnshortcutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnshortcutkeys.Name = "btnshortcutkeys";
		this.btnshortcutkeys.Size = new System.Drawing.Size(126, 24);
		this.btnshortcutkeys.Text = "ShortCut Keys";
		this.btnshortcutkeys.Click += new System.EventHandler(btnshortcutkeys_Click);
		this.BtnDefineItem.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnDefineItem.Name = "BtnDefineItem";
		this.BtnDefineItem.Size = new System.Drawing.Size(87, 24);
		this.BtnDefineItem.Text = "Define Item";
		this.BtnDefineItem.Click += new System.EventHandler(BtnDefineItem_Click);
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.ForeColor = System.Drawing.Color.White;
		this.label21.Location = new System.Drawing.Point(7, 32);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(353, 19);
		this.label21.TabIndex = 126;
		this.label21.Text = "Work Order For Semi Finish - Melting Heat";
		this.ChkPreview.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkPreview.AutoSize = true;
		this.ChkPreview.Checked = true;
		this.ChkPreview.CheckState = System.Windows.Forms.CheckState.Checked;
		this.ChkPreview.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkPreview.ForeColor = System.Drawing.Color.White;
		this.ChkPreview.Location = new System.Drawing.Point(839, 30);
		this.ChkPreview.Name = "ChkPreview";
		this.ChkPreview.Size = new System.Drawing.Size(78, 20);
		this.ChkPreview.TabIndex = 12;
		this.ChkPreview.Text = "Preview";
		this.ChkPreview.UseVisualStyleBackColor = true;
		this.tabPage3.Controls.Add((System.Windows.Forms.Control)(object)this.DatagridHistory);
		this.tabPage3.Controls.Add(this.panel9);
		this.tabPage3.Controls.Add((System.Windows.Forms.Control)(object)this.grdhistoryDetail);
		this.tabPage3.Controls.Add(this.panel15);
		this.tabPage3.Controls.Add(this.panel13);
		this.tabPage3.Controls.Add(this.panel38);
		this.tabPage3.Location = new System.Drawing.Point(4, 4);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Size = new System.Drawing.Size(926, 566);
		this.tabPage3.TabIndex = 2;
		this.tabPage3.Text = "History";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.DatagridHistory.AllowEdit = (InheritableBoolean)2;
		DatagridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DatagridHistory.DesignTimeLayout = DatagridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DatagridHistory.DynamicFiltering = true;
		this.DatagridHistory.FilterMode = (FilterMode)2;
		this.DatagridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DatagridHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Location = new System.Drawing.Point(0, 124);
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Name = "DatagridHistory";
		((ScrollableBase)this.DatagridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Size = new System.Drawing.Size(926, 259);
		((System.Windows.Forms.Control)(object)this.DatagridHistory).TabIndex = 0;
		this.DatagridHistory.TotalRow = (InheritableBoolean)1;
		this.DatagridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DatagridHistory).VisualStyle = (VisualStyle)4;
		this.DatagridHistory.ColumnButtonClick += new ColumnActionEventHandler(DatagridHistory_ColumnButtonClick);
		this.DatagridHistory.SelectionChanged += new System.EventHandler(DatagridHistory_SelectionChanged);
		this.DatagridHistory.LinkClicked += new ColumnActionEventHandler(DatagridHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.DatagridHistory).DoubleClick += new System.EventHandler(DatagridHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.DatagridHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(DatagridHistory_KeyDown);
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.label15);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel9.Location = new System.Drawing.Point(0, 383);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(926, 24);
		this.panel9.TabIndex = 51;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.Color.White;
		this.label15.Location = new System.Drawing.Point(4, 5);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(266, 16);
		this.label15.TabIndex = 129;
		this.label15.Text = "Detail Detail of the Above Selected Row";
		this.grdhistoryDetail.AllowEdit = (InheritableBoolean)2;
		grdhistoryDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdhistoryDetail.DesignTimeLayout = grdhistoryDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdhistoryDetail).Dock = System.Windows.Forms.DockStyle.Bottom;
		this.grdhistoryDetail.DynamicFiltering = true;
		this.grdhistoryDetail.FilterMode = (FilterMode)2;
		this.grdhistoryDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdhistoryDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdhistoryDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdhistoryDetail).Location = new System.Drawing.Point(0, 407);
		((System.Windows.Forms.Control)(object)this.grdhistoryDetail).Name = "grdhistoryDetail";
		((ScrollableBase)this.grdhistoryDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdhistoryDetail).Size = new System.Drawing.Size(926, 159);
		((System.Windows.Forms.Control)(object)this.grdhistoryDetail).TabIndex = 1;
		this.grdhistoryDetail.TotalRow = (InheritableBoolean)1;
		this.grdhistoryDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdhistoryDetail).VisualStyle = (VisualStyle)4;
		this.panel15.BackColor = System.Drawing.Color.Teal;
		this.panel15.Controls.Add(this.ctrlGrdBar1);
		this.panel15.Controls.Add(this.label17);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.Location = new System.Drawing.Point(0, 101);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(926, 23);
		this.panel15.TabIndex = 53;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(879, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(9, 6, 9, 6);
		this.ctrlGrdBar1.MyGrid = this.DatagridHistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(42, 22);
		this.ctrlGrdBar1.TabIndex = 128;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.ForeColor = System.Drawing.Color.White;
		this.label17.Location = new System.Drawing.Point(8, 3);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(112, 16);
		this.label17.TabIndex = 127;
		this.label17.Text = "Filtered Records";
		this.panel13.BackColor = System.Drawing.Color.White;
		this.panel13.Controls.Add(this.groupBox1);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel13.Location = new System.Drawing.Point(0, 52);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(926, 49);
		this.panel13.TabIndex = 0;
		this.groupBox1.Controls.Add(this.btnshow);
		this.groupBox1.Controls.Add(this.txtToDateHistory);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.label28);
		this.groupBox1.Controls.Add(this.txtToDocNoHistory);
		this.groupBox1.Controls.Add(this.txtFromdateHistory);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.label35);
		this.groupBox1.Controls.Add(this.txtFromNoHistory);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(5, 1);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(795, 47);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.btnshow.BackColor = System.Drawing.Color.Teal;
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(691, 17);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(55, 25);
		this.btnshow.TabIndex = 5;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.txtToDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDateHistory.CustomFormat = "dd-MMM-yy";
		this.txtToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtToDateHistory.Location = new System.Drawing.Point(269, 18);
		this.txtToDateHistory.Name = "txtToDateHistory";
		this.txtToDateHistory.ShowCheckBox = true;
		this.txtToDateHistory.Size = new System.Drawing.Size(144, 23);
		this.txtToDateHistory.TabIndex = 2;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(219, 22);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(48, 15);
		this.label1.TabIndex = 171;
		this.label1.Text = "Date To";
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.Location = new System.Drawing.Point(5, 22);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(63, 15);
		this.label28.TabIndex = 171;
		this.label28.Text = "Date From";
		this.txtToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDocNoHistory.Location = new System.Drawing.Point(610, 18);
		this.txtToDocNoHistory.Name = "txtToDocNoHistory";
		this.txtToDocNoHistory.Size = new System.Drawing.Size(78, 23);
		this.txtToDocNoHistory.TabIndex = 4;
		this.txtToDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtToDocNoHistory_KeyPress);
		this.txtFromdateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromdateHistory.CustomFormat = "dd-MMM-yy";
		this.txtFromdateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromdateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtFromdateHistory.Location = new System.Drawing.Point(70, 18);
		this.txtFromdateHistory.Name = "txtFromdateHistory";
		this.txtFromdateHistory.ShowCheckBox = true;
		this.txtFromdateHistory.Size = new System.Drawing.Size(144, 23);
		this.txtFromdateHistory.TabIndex = 1;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(561, 22);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(45, 15);
		this.label3.TabIndex = 173;
		this.label3.Text = "Doc To";
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(417, 22);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(60, 15);
		this.label35.TabIndex = 173;
		this.label35.Text = "Doc From";
		this.txtFromNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFromNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromNoHistory.Location = new System.Drawing.Point(479, 18);
		this.txtFromNoHistory.Name = "txtFromNoHistory";
		this.txtFromNoHistory.Size = new System.Drawing.Size(78, 23);
		this.txtFromNoHistory.TabIndex = 3;
		this.txtFromNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFromNoHistory_KeyPress);
		this.panel38.BackColor = System.Drawing.Color.Teal;
		this.panel38.Controls.Add(this.toolStrip2);
		this.panel38.Controls.Add(this.label32);
		this.panel38.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel38.Location = new System.Drawing.Point(0, 0);
		this.panel38.Name = "panel38";
		this.panel38.Size = new System.Drawing.Size(926, 52);
		this.panel38.TabIndex = 48;
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnNewHistory, this.toolStripSeparator1, this.btnRegister });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(926, 27);
		this.toolStrip2.TabIndex = 3;
		this.toolStrip2.Text = "toolStrip2";
		this.btnNewHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewHistory.Name = "btnNewHistory";
		this.btnNewHistory.Size = new System.Drawing.Size(59, 24);
		this.btnNewHistory.Text = "&New";
		this.btnNewHistory.Click += new System.EventHandler(btnHistoryRefresh_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.btnRegister.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRegister.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnRegister.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRegister.Name = "btnRegister";
		this.btnRegister.Size = new System.Drawing.Size(61, 24);
		this.btnRegister.Text = "&Print";
		this.btnRegister.Visible = false;
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.ForeColor = System.Drawing.Color.White;
		this.label32.Location = new System.Drawing.Point(5, 31);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(301, 19);
		this.label32.TabIndex = 127;
		this.label32.Text = "Work Order For Semi finish History  ";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(934, 595);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmWorkOrderForSemiFinish";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmWorkOrderForSemiFinish_FormClosed);
		base.Load += new System.EventHandler(frmBillOfMaterial_Manufacturing_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmBillOfMaterial_Manufacturing_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbSampleModal).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCategory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbUomDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCastingTypeDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbVariantDescription).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbOrderNoDetail).EndInit();
		this.panel2.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DatagridHistory).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdhistoryDetail).EndInit();
		this.panel15.ResumeLayout(false);
		this.panel15.PerformLayout();
		this.panel13.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		this.panel38.ResumeLayout(false);
		this.panel38.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
