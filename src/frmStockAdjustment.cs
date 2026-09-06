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
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Production;
using Architecture.WinApp.Properties;
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
namespace Architecture.WinApp.StoreManagement
{

public class frmStockAdjustment : Form
{
	private int updateDetailIndex;

	private int RecId;

	private int VoucherHeadId;

	private bool FifoMethod;

	private bool AvgFeatureMethod;

	private DataTable dtitem = new DataTable();

	private DataTable table = new DataTable();

	private DataTable dtCrop = new DataTable();

	private DataTable dtJobLoat = new DataTable();

	private DataTable dtWh = new DataTable();

	private DataTable dtAccounts = new DataTable();

	private Attachment AT = new Attachment();

	private DropDownBind DDL = new DropDownBind();

	private Rightsobjects formright = new Rightsobjects();

	private LoadavailableTransactionsForIssuance LoadTransactions;

	public UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel2;

	private TabPage tabPage2;

	private Panel panel3;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnupdate;

	private ToolStripButton btnsave;

	private ToolStripButton btnprint;

	private ToolStripButton btnattachment;

	private Label label23;

	private Panel panel4;

	private Label label33;

	private DateTimePicker DocDate;

	private Label label17;

	private TextBox txtdocno;

	private Label label5;

	private Panel panel5;

	private Button btnplus;

	private TextBox txtremarksdetail;

	private Label label47;

	private TextBox txtweight;

	private TextBox txtqty;

	private Label label35;

	private Label label39;

	private Label label38;

	private Label label36;

	private Label label37;

	private Panel panel6;

	private Panel panel7;

	private Panel panel9;

	private Panel panel8;

	internal UltraCombo CmbItemName;

	internal UltraCombo CmbPackUom;

	internal UltraCombo CmbPackingType;

	private CtrlGrdBar ctrlGrdBar1;

	private CtrlGrdBar ctrlGrdBar2;

	private Button btnUpdateDetail;

	private Button btnCancelUpdateDetial;

	private Panel panel12;

	private ToolStripButton btnRefresh;

	private ToolStrip toolStrip2;

	private TextBox txtremarks;

	internal UltraCombo CmbWareHouse;

	private Label label3;

	internal GridEX grd;

	private Label label4;

	internal UltraCombo CmbEntryType;

	internal UltraCombo CmbJobLot;

	private Label label8;

	private CheckBox ChkPrint;

	private TextBox txtItemAmount;

	private Label label6;

	private TextBox txtItemRate;

	private Label label1;

	internal UltraCombo CmbCropYear;

	private Label label7;

	internal UltraCombo CmbRateUom;

	private Label label2;

	internal UltraCombo CmbAccountTitle;

	private Label label9;

	private ToolStripButton btnVoucher118;

	private Label lblStockBalance;

	private RadioButton rdbtnItemCode;

	private RadioButton rdbtnItemName;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripSeparator toolStripSeparator3;

	private ToolStripSeparator toolStripSeparator4;

	private ToolStripSeparator toolStripSeparator1;

	internal GridEX grdhistory;

	internal GridEX grdHistoryDetail;

	private Label label78;

	private GroupBox groupBox2;

	private Button btnShowHistory;

	private Label label50;

	private Label label51;

	private Label label52;

	private Label label53;

	private Label label54;

	internal UltraCombo CmbAdjustmentTypeHistory;

	private TextBox ToDocNoHistory;

	private TextBox FromDocNoHistory;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	private Label label11;

	private Label label10;

	private ToolStripButton btnNewHistory;

	private ToolStripButton btnRefreshHistory;

	private ToolStripButton btnshortcut;

	private RadioButton rdmodifydate;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	private ToolStripButton btnLoadAvailableStock;

	private ToolStripButton btnGenerateRate;

	public frmStockAdjustment(UserAccount user)
	{
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void frmStockAdjustment_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(10, 10);
			base.AutoScrollMinSize = new Size(1000, 625);
			formright = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnsave.Enabled = formright.DoHaveSaveRight;
			btnupdate.Enabled = formright.DoHaveUpdateRights;
			btnprint.Enabled = formright.DoHavePrintRights;
			ChkPrint.Checked = formright.DoHavePrintRights;
			ctrlGrdBar1.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar1.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar1.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar1.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar1.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar1.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			ctrlGrdBar2.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar2.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar2.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar2.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar2.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar2.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			FifoMethod = CommonServices.GetERPFeatureById(5);
			AvgFeatureMethod = ((CommonServices.GetERPFeatureById(1) || CommonServices.GetERPFeatureById(2) || CommonServices.GetERPFeatureById(19)) ? true : false);
			dtitem.Columns.Add("Id", typeof(int));
			dtitem.Columns.Add("ItemName", typeof(string));
			dtitem.Columns.Add("ItemCode", typeof(string));
			table.Columns.Add("Id", typeof(int));
			table.Columns.Add("RefDocumentTypeId", typeof(int));
			table.Columns.Add("RefDocNoId", typeof(int));
			table.Columns.Add("RefDocSubIdNo", typeof(int));
			table.Columns.Add("RefDocumentType", typeof(string));
			table.Columns.Add("RefDocDate", typeof(DateTime));
			table.Columns.Add("RefDocNo", typeof(int));
			table.Columns.Add("WareHouseId", typeof(int));
			table.Columns.Add("WareHouse");
			table.Columns.Add("ItemId", typeof(int));
			table.Columns.Add("Item");
			table.Columns.Add("ItemCode");
			table.Columns.Add("CropYearId", typeof(int));
			table.Columns.Add("CropYear");
			table.Columns.Add("JobLotId", typeof(int));
			table.Columns.Add("JobLot");
			table.Columns.Add("PackingTypeId", typeof(int));
			table.Columns.Add("PackingType");
			table.Columns.Add("ItemUOMId", typeof(int));
			table.Columns.Add("ItemUOM");
			table.Columns.Add("ItemUOMEquivalent", typeof(double));
			table.Columns.Add("ItemQty", typeof(double));
			table.Columns.Add("Weight", typeof(double));
			table.Columns.Add("ItemRate", typeof(double));
			table.Columns.Add("RateUomId", typeof(int));
			table.Columns.Add("RateUom");
			table.Columns.Add("RateUomEquivalent", typeof(double));
			table.Columns.Add("ItemAmount", typeof(double));
			table.Columns.Add("AccountId", typeof(int));
			table.Columns.Add("Comments");
			grd.DataSource = table;
			grd.RetrieveStructure();
			grdSettings();
			DocumentNoFill();
			EntryTypeBind();
			WareHouseFill();
			ItemBind();
			CropYearFill();
			JobLotFill();
			CmbPackingTypeFill();
			HistoryCombosFill();
			DocDate.Focus();
			btnupdate.Visible = false;
			btnsave.Visible = true;
			btnplus.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
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
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected O, but got Unknown
		try
		{
			int code = InvStockAdjustment.GenerateCode(new InvStockAdjustment
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = 70,
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

	private void WareHouseFill()
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		try
		{
			dtWh.Clear();
			int ItId1 = Conversion.ToInt(CmbWareHouse.Value);
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			dtWh = WarehousesAllocationToBranch.GetWarehousesAllocatedToBranchByBranchId(obj);
			if (dtWh.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtWh, CmbWareHouse, "Id", "WareHouseName", "WareHouseName", true);
				if (ItId1 > 0)
				{
					if (dtWh.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ItId1))
					{
						CmbWareHouse.Value = ItId1;
					}
					else
					{
						((Control)(object)CmbWareHouse).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbWareHouse).Text = string.Empty;
				((UltraGridBase)CmbWareHouse).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void JobLotFill()
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		try
		{
			dtJobLoat.Clear();
			int ItId1 = Conversion.ToInt(CmbJobLot.Value);
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			dtJobLoat = JobLotsAllocationToBranch.GetJobLotsAllocatedToBranchByBranchId(obj);
			if (dtJobLoat.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtJobLoat, CmbJobLot, "Id", "JobLotDescription", "JobLotDescription", true);
				if (ItId1 > 0)
				{
					if (dtJobLoat.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ItId1))
					{
						CmbJobLot.Value = ItId1;
					}
					else
					{
						((Control)(object)CmbJobLot).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbJobLot).Text = string.Empty;
				((UltraGridBase)CmbJobLot).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CropYearFill()
	{
		try
		{
			dtCrop.Clear();
			int ItId1 = Conversion.ToInt(CmbCropYear.Value);
			dtCrop = CommonServices.CropYearGetAllService();
			if (dtCrop.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtCrop, CmbCropYear, "Id", "CropYear", "CropYear", true);
				if (ItId1 > 0)
				{
					if (dtCrop.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ItId1))
					{
						CmbCropYear.Value = ItId1;
					}
					else
					{
						((Control)(object)CmbCropYear).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbCropYear).Text = string.Empty;
				((UltraGridBase)CmbCropYear).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemBind()
	{
		try
		{
			int ItId1 = Conversion.ToInt(CmbItemName.Value);
			DataTable dt = CommonServices.ReadAllItems();
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtitem.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ItemName"], dt.Rows[i]["ItemCodeNew"]);
				}
				if (dtitem.Rows.Count > 0)
				{
					if (rdbtnItemName.Checked)
					{
						DDL.BindDDL(dtitem, CmbItemName, "Id", "ItemName", "Item Name", ZeroIndex: true);
					}
					else
					{
						DDL.BindDDL(dtitem, CmbItemName, "Id", "ItemCode", "Item Name", ZeroIndex: true);
					}
					((UltraGridBase)CmbItemName).DisplayLayout.Bands[0].Columns[1].Width = 300;
					if (ItId1 > 0)
					{
						if (dtitem.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ItId1))
						{
							CmbItemName.Value = ItId1;
						}
						else
						{
							((Control)(object)CmbItemName).Text = string.Empty;
						}
					}
				}
				else
				{
					((Control)(object)CmbItemName).Text = string.Empty;
					((UltraGridBase)CmbItemName).DataSource = null;
				}
			}
			else
			{
				((Control)(object)CmbItemName).Text = string.Empty;
				((UltraGridBase)CmbItemName).DataSource = null;
			}
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
				if (Conversion.ToInt(CmbItemName.Value) > 0)
				{
					ItemId = Conversion.ToInt(CmbItemName.Value);
				}
				if (rdbtnItemName.Checked)
				{
					DDL.BindDDL(dtitem, CmbItemName, "Id", "ItemName", "Item Name", ZeroIndex: true);
				}
				else
				{
					DDL.BindDDL(dtitem, CmbItemName, "Id", "ItemCode", "Item Name", ZeroIndex: true);
				}
				CmbItemName.Value = ItemId;
				CmbItemName.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void bindRateUomAndItemPackUom(int ID)
	{
		try
		{
			string PackUom = Conversion.ToString((object)((Control)(object)CmbPackUom).Text.Trim());
			string RateUom = Conversion.ToString((object)((Control)(object)CmbRateUom).Text.Trim());
			DataTable dtuomsch = CommonServices.GetUomScheduleByItemId(ID);
			if (dtuomsch.Rows.Count > 0)
			{
				DDL.BindDDL(dtuomsch, CmbPackUom, "Id", "UOMCode", "PackUom", ZeroIndex: true);
				DDL.BindDDL(dtuomsch, CmbRateUom, "Id", "UOMCode", "RateUom", ZeroIndex: true);
				((UltraGridBase)CmbPackUom).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)CmbRateUom).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((Control)(object)CmbPackUom).Text = PackUom;
				((Control)(object)CmbRateUom).Text = RateUom;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbPackingTypeFill()
	{
		try
		{
			int ItId1 = Conversion.ToInt(CmbPackingType.Value);
			DataTable dtPackingType = InvPackingType.Getall();
			if (dtPackingType.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtPackingType, CmbPackingType, "Id", "PackTypeDesc", "Packing Type", true);
				if (ItId1 > 0)
				{
					if (dtPackingType.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ItId1))
					{
						CmbPackingType.Value = ItId1;
					}
					else
					{
						((Control)(object)CmbPackingType).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbPackingType).Text = string.Empty;
				((UltraGridBase)CmbPackingType).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void EntryTypeBind()
	{
		try
		{
			int ItId1 = Conversion.ToInt(CmbEntryType.Value);
			DataTable dt = CommonServices.StaticColumnsService("StockAdjustmentType");
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbEntryType, "Id", "Type", "Entry Type", true);
				if (ItId1 > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ItId1))
					{
						CmbEntryType.Value = ItId1;
					}
					else
					{
						((Control)(object)CmbEntryType).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbEntryType).Text = string.Empty;
				((UltraGridBase)CmbEntryType).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbEntryType_Leave(object sender, EventArgs e)
	{
		try
		{
			int ItId1 = Conversion.ToInt(CmbAccountTitle.Value);
			dtAccounts.Clear();
			if (Conversion.ToInt(CmbEntryType.Value) == 1)
			{
				dtAccounts = CommonServices.CoaAllocationAccountTitleByAccountTypeIds("10,12");
			}
			else
			{
				dtAccounts = CommonServices.CoaAllocationAccountTitleByAccountTypeIds("11,12,13,20,21");
			}
			if (dtAccounts.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtAccounts, CmbAccountTitle, "Id", "AccountTitle", "Account Title", true);
				if (ItId1 > 0)
				{
					if (dtAccounts.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ItId1))
					{
						CmbAccountTitle.Value = ItId1;
					}
					else
					{
						((Control)(object)CmbAccountTitle).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbAccountTitle).Text = string.Empty;
				((UltraGridBase)CmbAccountTitle).DataSource = null;
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
		if (((UltraGridBase)CmbEntryType).ActiveRow == null || Conversion.ToInt(CmbEntryType.Value) == 0)
		{
			MessageBox.Show("Entry Type Field is Required");
			CmbEntryType.Focus();
			return false;
		}
		return true;
	}

	private bool FormValidationDetail()
	{
		if (((UltraGridBase)CmbWareHouse).ActiveRow == null || Conversion.ToInt(CmbWareHouse.Value) == 0)
		{
			MessageBox.Show("WareHouseName field is required");
			CmbWareHouse.Focus();
			return false;
		}
		if (((UltraGridBase)CmbItemName).ActiveRow == null || Conversion.ToInt(CmbItemName.Value) == 0)
		{
			MessageBox.Show("ItemName field is required");
			CmbItemName.Focus();
			return false;
		}
		if (((UltraGridBase)CmbCropYear).ActiveRow == null || Conversion.ToInt(CmbCropYear.Value) == 0)
		{
			MessageBox.Show("CropYear field is required");
			CmbCropYear.Focus();
			return false;
		}
		if (((UltraGridBase)CmbJobLot).ActiveRow == null || Conversion.ToInt(CmbJobLot.Value) == 0)
		{
			MessageBox.Show("JobLot field is required");
			CmbJobLot.Focus();
			return false;
		}
		if (((UltraGridBase)CmbPackingType).ActiveRow == null || Conversion.ToInt(CmbPackingType.Value) == 0)
		{
			MessageBox.Show("PackingType field is required");
			CmbPackingType.Focus();
			return false;
		}
		if (((UltraGridBase)CmbPackUom).ActiveRow == null || Conversion.ToInt(CmbPackUom.Value) == 0)
		{
			MessageBox.Show("PackUOM field is required");
			CmbPackUom.Focus();
			return false;
		}
		if (Conversion.ToInt(CmbEntryType.Value) != 2 && Conversion.ToDouble((object)txtqty.Text) == 0.0)
		{
			MessageBox.Show("ItemQty field is required");
			txtqty.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtweight.Text) == 0.0)
		{
			MessageBox.Show("Weight field is required");
			txtweight.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtItemRate.Text) == 0.0)
		{
			MessageBox.Show("ItemRate field is required");
			txtItemRate.Focus();
			return false;
		}
		if (((UltraGridBase)CmbRateUom).ActiveRow == null || Conversion.ToInt(CmbRateUom.Value) == 0)
		{
			MessageBox.Show("RateUom field is required");
			CmbRateUom.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtItemAmount.Text) == 0.0)
		{
			MessageBox.Show("ItemAmount field is required");
			txtItemAmount.Focus();
			return false;
		}
		if (((UltraGridBase)CmbAccountTitle).ActiveRow == null || Conversion.ToInt(CmbAccountTitle.Value) == 0)
		{
			MessageBox.Show("AccountTitle field is required");
			CmbAccountTitle.Focus();
			return false;
		}
		return true;
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		Reset();
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
			VoucherHeadId = 0;
			((Control)(object)CmbEntryType).Enabled = true;
			txtItemRate.Enabled = true;
			DocDate.Focus();
			((Control)(object)CmbItemName).Text = string.Empty;
			((Control)(object)CmbPackUom).Text = string.Empty;
			txtqty.Text = string.Empty;
			txtweight.Text = string.Empty;
			((Control)(object)CmbPackingType).Text = string.Empty;
			txtremarks.Text = string.Empty;
			((Control)(object)CmbEntryType).Text = string.Empty;
			txtItemRate.Text = "0";
			txtItemAmount.Text = "0";
			btnsave.Visible = true;
			btnupdate.Visible = false;
			btnplus.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			DocumentNoFill();
			table.Rows.Clear();
			grd.DataSource = table;
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
			((Control)(object)CmbItemName).Text = string.Empty;
			((Control)(object)CmbPackUom).Text = string.Empty;
			txtqty.Text = string.Empty;
			txtweight.Text = string.Empty;
			txtItemRate.Text = "0";
			txtItemAmount.Text = "0";
			((Control)(object)CmbPackingType).Text = string.Empty;
			txtremarksdetail.Text = string.Empty;
			((Control)(object)CmbWareHouse).Text = string.Empty;
			((Control)(object)CmbJobLot).Text = string.Empty;
			((Control)(object)CmbAccountTitle).Text = string.Empty;
			btnplus.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
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
			bindRateUomAndItemPackUom(Conversion.ToInt(CmbItemName.Value));
			CmbPackingTypeFill();
			WareHouseFill();
			JobLotFill();
			ItemBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmStockAdjustment_FormClosing(object sender, FormClosingEventArgs e)
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

	private void btnplus_Click(object sender, EventArgs e)
	{
		try
		{
			if (grd.GetDataRows().Count() > 0)
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow item in rows)
				{
					if (Conversion.ToInt(item.Cells["RefDocumentTypeId"].Value) != 0 || Conversion.ToInt(item.Cells["RefDocNoId"].Value) != 0 || Conversion.ToInt(item.Cells["RefDocSubIdNo"].Value) != 0)
					{
						throw new Exception("A record From loader exists in the grid, so you can't add a manual record.");
					}
				}
			}
			if (FormValidationDetail())
			{
				table.Rows.Add(0, 0, 0, 0, "", DateTime.Now, 0, Conversion.ToInt(CmbWareHouse.Value), Conversion.ToString((object)((Control)(object)CmbWareHouse).Text), Conversion.ToInt(CmbItemName.Value), Conversion.ToString(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["ItemName"].Value), Conversion.ToString(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["ItemCode"].Value), Conversion.ToInt(CmbCropYear.Value), Conversion.ToString((object)((Control)(object)CmbCropYear).Text.Trim()), Conversion.ToInt(CmbJobLot.Value), Conversion.ToString((object)((Control)(object)CmbJobLot).Text), Conversion.ToInt(CmbPackingType.Value), Conversion.ToString((object)((Control)(object)CmbPackingType).Text), Conversion.ToInt(CmbPackUom.Value), Conversion.ToString((object)((Control)(object)CmbPackUom).Text.Trim()), Conversion.ToDouble(((UltraDropDownBase)CmbPackUom).SelectedRow.Cells[2].Value), Conversion.ToDouble((object)txtqty.Text.Trim()), Conversion.ToDouble((object)txtweight.Text.Trim()), Conversion.ToDouble((object)txtItemRate.Text.Trim()), Conversion.ToInt(CmbRateUom.Value), Conversion.ToString((object)((Control)(object)CmbRateUom).Text), Conversion.ToDouble(((UltraDropDownBase)CmbRateUom).SelectedRow.Cells[2].Value), Conversion.ToDouble((object)txtItemAmount.Text.Trim()), Conversion.ToInt(CmbAccountTitle.Value), Conversion.ToString((object)txtremarksdetail.Text.Trim()));
				grd.DataSource = table;
				grd.RetrieveStructure();
				grdSettings();
				ResetDetail();
				((Control)(object)CmbEntryType).Enabled = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettings()
	{
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grd.AllowEdit = (InheritableBoolean)1;
			grd.AllowDelete = (InheritableBoolean)2;
			grd.ColumnAutoResize = false;
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["RefDocumentTypeId"].Visible = false;
			grd.RootTable.Columns["RefDocNoId"].Visible = false;
			grd.RootTable.Columns["RefDocSubIdNo"].Visible = false;
			grd.RootTable.Columns["ItemId"].Visible = false;
			grd.RootTable.Columns["CropYearId"].Visible = false;
			grd.RootTable.Columns["JobLotId"].Visible = false;
			grd.RootTable.Columns["PackingTypeId"].Visible = false;
			grd.RootTable.Columns["ItemUOMId"].Visible = false;
			grd.RootTable.Columns["WareHouseId"].Visible = false;
			grd.RootTable.Columns["RateUomId"].Visible = false;
			grd.RootTable.Columns["ItemUOMEquivalent"].Visible = false;
			grd.RootTable.Columns["RateUomEquivalent"].Visible = false;
			grd.RootTable.Columns["RefDocNo"].Visible = false;
			grd.RootTable.Columns["RefDocDate"].Visible = false;
			grd.RootTable.Columns["RefDocumentType"].Visible = false;
			GridEXRow r = grd.GetDataRows().FirstOrDefault((GridEXRow rrr) => Conversion.ToInt(rrr.Cells["RefDocumentTypeId"].Value) > 0);
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				grd.RootTable.Columns["RefDocNo"].Visible = true;
				grd.RootTable.Columns["RefDocDate"].Visible = true;
				grd.RootTable.Columns["RefDocumentType"].Visible = true;
			}
			grd.RootTable.Columns["Id"].EditType = (EditType)0;
			grd.RootTable.Columns["RefDocumentTypeId"].EditType = (EditType)0;
			grd.RootTable.Columns["RefDocNoId"].EditType = (EditType)0;
			grd.RootTable.Columns["RefDocSubIdNo"].EditType = (EditType)0;
			grd.RootTable.Columns["RefDocumentType"].EditType = (EditType)0;
			grd.RootTable.Columns["RefDocDate"].EditType = (EditType)0;
			grd.RootTable.Columns["RefDocNo"].EditType = (EditType)0;
			grd.RootTable.Columns["WareHouseId"].EditType = (EditType)0;
			grd.RootTable.Columns["WareHouse"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemId"].EditType = (EditType)0;
			grd.RootTable.Columns["Item"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemCode"].EditType = (EditType)0;
			grd.RootTable.Columns["CropYearId"].EditType = (EditType)0;
			grd.RootTable.Columns["CropYear"].EditType = (EditType)0;
			grd.RootTable.Columns["JobLotId"].EditType = (EditType)0;
			grd.RootTable.Columns["JobLot"].EditType = (EditType)0;
			grd.RootTable.Columns["PackingTypeId"].EditType = (EditType)0;
			grd.RootTable.Columns["PackingType"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemUOMId"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemUOM"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemUOMEquivalent"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemRate"].EditType = (EditType)0;
			grd.RootTable.Columns["RateUomId"].EditType = (EditType)0;
			grd.RootTable.Columns["RateUom"].EditType = (EditType)0;
			grd.RootTable.Columns["RateUomEquivalent"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemAmount"].EditType = (EditType)0;
			grd.RootTable.Columns["AccountId"].ColumnType = (ColumnType)0;
			grd.RootTable.Columns["AccountId"].HasValueList = true;
			grd.RootTable.Columns["AccountId"].LimitToList = true;
			grd.RootTable.Columns["AccountId"].EditType = (EditType)4;
			grd.RootTable.Columns["AccountId"].Caption = "Account Title";
			grd.RootTable.Columns["AccountId"].ValueList.PopulateValueList((IEnumerable)dtAccounts.DefaultView, "Id", "AccountTitle");
			grd.RootTable.Columns["AccountId"].Width = 250;
			grd.RootTable.Columns["ItemQty"].EditType = (EditType)1;
			grd.RootTable.Columns["Weight"].EditType = (EditType)1;
			grd.RootTable.Columns["Comments"].EditType = (EditType)1;
			grd.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemQty"].FormatString = "#,#.##";
			grd.RootTable.Columns["Weight"].FormatString = "#,#.##";
			grd.RootTable.Columns["ItemRate"].FormatString = "#,#.###";
			grd.RootTable.Columns["ItemAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["ItemQty"].TotalFormatString = "#,#.##";
			grd.RootTable.Columns["Weight"].TotalFormatString = "#,#.##";
			grd.RootTable.Columns["ItemAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustmentNew(grd);
			grd.RootTable.Columns.Add("Delete");
			grd.RootTable.Columns["Delete"].Caption = "Delete";
			grd.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Delete"].ButtonText = "Delete";
			grd.RootTable.Columns["Delete"].Width = 60;
			grd.RootTable.Columns.Add("Edit");
			grd.RootTable.Columns["Edit"].Caption = "Edit";
			grd.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Edit"].ButtonText = "Edit";
			grd.RootTable.Columns["Edit"].Width = 50;
			grd.RootTable.Columns["Delete"].Position = 0;
			grd.RootTable.Columns["Edit"].Position = 1;
			grd.FrozenColumns = 2;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_DoubleClick(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grd.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
			{
				if (Conversion.ToInt(item.Cells["RefDocumentTypeId"].Value) > 0 || Conversion.ToInt(item.Cells["RefDocNoId"].Value) > 0 || Conversion.ToInt(item.Cells["RefDocSubIdNo"].Value) > 0)
				{
					throw new Exception("The record is from the loader, so you can't update it from the fields. You can change the Qty,Weight, and Account_Title in the grid.");
				}
				updateDetailIndex = item.RowIndex;
				CmbWareHouse.Value = item.Cells["WareHouseId"].Value.ToString();
				CmbItemName.Value = item.Cells["ItemId"].Value.ToString();
				bindRateUomAndItemPackUom(Conversion.ToInt(CmbItemName.Value));
				CmbPackUom.Value = item.Cells["ItemUOMId"].Value.ToString();
				((Control)(object)CmbPackUom).Text = item.Cells["ItemUOM"].Text.ToString();
				CmbPackingType.Value = item.Cells["PackingTypeId"].Value.ToString();
				CmbCropYear.Value = item.Cells["CropYearId"].Value.ToString();
				txtqty.Text = item.Cells["ItemQty"].Value.ToString();
				txtweight.Text = item.Cells["Weight"].Value.ToString();
				txtItemRate.Text = item.Cells["ItemRate"].Value.ToString();
				txtItemAmount.Text = item.Cells["ItemAmount"].Value.ToString();
				CmbJobLot.Value = item.Cells["JobLotId"].Value.ToString();
				CmbRateUom.Value = item.Cells["RateUomId"].Value.ToString();
				CmbAccountTitle.Value = item.Cells["AccountId"].Value.ToString();
				txtremarksdetail.Text = item.Cells["Comments"].Value.ToString();
				btnplus.Visible = false;
				btnUpdateDetail.Visible = true;
				btnCancelUpdateDetial.Visible = true;
			}
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
				table.Rows[updateDetailIndex]["WareHouseId"] = Conversion.ToInt(CmbWareHouse.Value);
				table.Rows[updateDetailIndex]["WareHouse"] = Conversion.ToString((object)((Control)(object)CmbWareHouse).Text);
				table.Rows[updateDetailIndex]["ItemId"] = Conversion.ToInt(CmbItemName.Value);
				table.Rows[updateDetailIndex]["Item"] = Conversion.ToString(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["ItemName"].Value);
				table.Rows[updateDetailIndex]["ItemCode"] = Conversion.ToString(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["ItemCode"].Value);
				table.Rows[updateDetailIndex]["ItemUOMId"] = Conversion.ToInt(CmbPackUom.Value);
				table.Rows[updateDetailIndex]["ItemUOM"] = Conversion.ToString((object)((Control)(object)CmbPackUom).Text);
				table.Rows[updateDetailIndex]["PackingTypeId"] = Conversion.ToInt(CmbPackingType.Value);
				table.Rows[updateDetailIndex]["PackingType"] = Conversion.ToString((object)((Control)(object)CmbPackingType).Text);
				table.Rows[updateDetailIndex]["CropYearId"] = Conversion.ToInt(CmbCropYear.Value);
				table.Rows[updateDetailIndex]["CropYear"] = Conversion.ToString((object)((Control)(object)CmbCropYear).Text);
				table.Rows[updateDetailIndex]["ItemQty"] = Conversion.ToDouble((object)txtqty.Text);
				table.Rows[updateDetailIndex]["ItemRate"] = Conversion.ToDouble((object)txtItemRate.Text);
				table.Rows[updateDetailIndex]["ItemAmount"] = Conversion.ToDouble((object)txtItemAmount.Text);
				table.Rows[updateDetailIndex]["Weight"] = Conversion.ToDouble((object)txtweight.Text);
				table.Rows[updateDetailIndex]["JobLotId"] = Conversion.ToInt(CmbJobLot.Value);
				table.Rows[updateDetailIndex]["JobLot"] = Conversion.ToString((object)((Control)(object)CmbJobLot).Text);
				table.Rows[updateDetailIndex]["RateUomId"] = Conversion.ToInt(CmbRateUom.Value);
				table.Rows[updateDetailIndex]["RateUom"] = Conversion.ToString((object)((Control)(object)CmbRateUom).Text);
				table.Rows[updateDetailIndex]["AccountId"] = Conversion.ToInt(CmbAccountTitle.Value);
				table.Rows[updateDetailIndex]["Comments"] = Conversion.ToString((object)txtremarksdetail.Text);
				grd.DataSource = table;
				btnplus.Visible = true;
				btnUpdateDetail.Visible = false;
				btnCancelUpdateDetial.Visible = false;
				ResetDetail();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelUpdateDetial_Click(object sender, EventArgs e)
	{
		ResetDetail();
	}

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0)
			{
				return;
			}
			Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
			if (e.Column.Key == "Delete")
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				r.Delete();
				grd.UpdateData();
				if (grd.RowCount == 0)
				{
					((Control)(object)CmbEntryType).Enabled = true;
				}
			}
			if (e.Column.Key == "Edit")
			{
				grd_DoubleClick(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		try
		{
			grd.UpdateData();
			GridEXRow item = grd.CurrentRow;
			if (e.Column.Key == "ItemQty")
			{
				double Qty = Conversion.ToDouble((object)item.Cells["ItemQty"].Value.ToString());
				double Uom = Conversion.ToDouble((object)item.Cells["ItemUOMEquivalent"].Value.ToString());
				double Weight2 = Qty * Uom;
				item.Cells["Weight"].Value = Weight2;
				double Rate2 = Conversion.ToDouble((object)item.Cells["ItemRate"].Value.ToString());
				double RateUOM2 = Conversion.ToDouble(item.Cells["RateUomEquivalent"].Value);
				double Amount2 = Weight2 / RateUOM2 * Rate2;
				item.Cells["ItemAmount"].Value = Amount2;
			}
			if (e.Column.Key == "Weight")
			{
				double Weight = Conversion.ToDouble((object)item.Cells["Weight"].Value.ToString());
				double Rate = Conversion.ToDouble(item.Cells["ItemRate"].Value);
				double RateUOM = Conversion.ToDouble(item.Cells["RateUomEquivalent"].Value);
				double Amount = Weight / RateUOM * Rate;
				item.Cells["ItemAmount"].Value = Math.Round(Amount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
			}
			if (!(e.Column.Key == "ItemRate") && !(e.Column.Key == "RateUom"))
			{
				_ = e.Column.Key == "RateUomEquivalent";
			}
			grd.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Expected O, but got Unknown
		try
		{
			if (grd.GetRows().Count() == 0)
			{
				throw new Exception("Grid Record Not Found");
			}
			if (!FormValidation())
			{
				return;
			}
			InvStockAdjustment obj = new InvStockAdjustment();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				obj.Id = RecId;
				obj.ModifyUserId = UserAccount.ID;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.DocumentTypeId = 70;
			obj.DocDate = DocDate.Value;
			obj.DocNo = Conversion.ToInt((object)txtdocno.Text);
			obj.RemarksHeader = Conversion.ToString((object)txtremarks.Text);
			obj.AdjustmentTypeId = Conversion.ToInt(CmbEntryType.Value);
			obj.EnteryUserId = UserAccount.ID;
			obj.ModifyUserId = UserAccount.ID;
			obj.ApprovalUserId = UserAccount.ID;
			obj.EnteryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			obj.ApprovedDate = DateTime.Now;
			obj.InvStockAdjustmentDetailslist = new List<InvStockAdjustmentDetail>();
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow r in rows)
			{
				InvStockAdjustmentDetail vd = new InvStockAdjustmentDetail();
				vd.Id = Conversion.ToInt(r.Cells["Id"].Value);
				if (Conversion.ToInt(r.Cells["WareHouseId"].Value) == 0)
				{
					throw new Exception($"Warehouse Not found In Grid Row No : {r.RowIndex + 1}");
				}
				if (Conversion.ToInt(r.Cells["ItemId"].Value) == 0)
				{
					throw new Exception($"Item Not found In Grid Row No : {r.RowIndex + 1}");
				}
				if (Conversion.ToInt(r.Cells["CropYearId"].Value) == 0)
				{
					throw new Exception($"CropYear Not found In Grid Row No : {r.RowIndex + 1}");
				}
				if (Conversion.ToInt(r.Cells["JobLotId"].Value) == 0)
				{
					throw new Exception($"JobLot Not found In Grid Row No : {r.RowIndex + 1}");
				}
				if (Conversion.ToInt(r.Cells["PackingTypeId"].Value) == 0)
				{
					throw new Exception($"PackingType Not found In Grid Row No : {r.RowIndex + 1}");
				}
				if (Conversion.ToInt(r.Cells["ItemUOMId"].Value) == 0)
				{
					throw new Exception($"ItemUOM Not found In Grid Row No : {r.RowIndex + 1}");
				}
				if (Conversion.ToInt(CmbEntryType.Value) != 2 && Conversion.ToDouble(r.Cells["ItemQty"].Value) == 0.0)
				{
					throw new Exception($"ItemQty Not found In Grid Row No : {r.RowIndex + 1}");
				}
				if (Conversion.ToDouble(r.Cells["Weight"].Value) == 0.0)
				{
					throw new Exception($"Weight Not found In Grid Row No : {r.RowIndex + 1}");
				}
				if (Conversion.ToInt(r.Cells["RateUomId"].Value) == 0)
				{
					throw new Exception($"RateUom Not found In Grid Row No : {r.RowIndex + 1}");
				}
				if (Conversion.ToDouble(r.Cells["ItemAmount"].Value) == 0.0)
				{
					throw new Exception($"ItemAmount Not found In Grid Row No : {r.RowIndex + 1}");
				}
				if (Conversion.ToInt(r.Cells["AccountId"].Value) == 0)
				{
					throw new Exception($"Account Not found In Grid Row No : {r.RowIndex + 1}");
				}
				vd.RefDocumentTypeId = Conversion.ToInt(r.Cells["RefDocumentTypeId"].Value);
				vd.RefDocNoId = Conversion.ToInt(r.Cells["RefDocNoId"].Value);
				vd.RefDocSubIdNo = Conversion.ToInt(r.Cells["RefDocSubIdNo"].Value);
				vd.WarehouseId = Conversion.ToInt(r.Cells["WareHouseId"].Value);
				vd.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
				vd.CropYearId = Conversion.ToInt(r.Cells["CropYearId"].Value);
				vd.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
				vd.PackingTypeId = Conversion.ToInt(r.Cells["PackingTypeId"].Value);
				vd.PackUomId = Conversion.ToInt(r.Cells["ItemUOMId"].Value);
				vd.Qty = Conversion.ToDecimal(r.Cells["ItemQty"].Value);
				vd.NetWeight = Conversion.ToDecimal(r.Cells["Weight"].Value);
				vd.ItemRate = Conversion.ToDecimal(r.Cells["ItemRate"].Value);
				vd.RateUomId = Conversion.ToInt(r.Cells["RateUomId"].Value);
				vd.Amount = Conversion.ToDouble(r.Cells["ItemAmount"].Value);
				vd.RemarksDetail = Conversion.ToString(r.Cells["Comments"].Value);
				if (obj.AdjustmentTypeId == 2)
				{
					vd.DebitAccountId = Conversion.ToInt(r.Cells["AccountId"].Value);
				}
				else
				{
					vd.CreditAccountId = Conversion.ToInt(r.Cells["AccountId"].Value);
				}
				vd.RateUom = Conversion.ToString((object)r.Cells["RateUom"].Text);
				obj.InvStockAdjustmentDetailslist.Add(vd);
			}
			obj.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			obj.DeleteAttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<InvStockAdjustment>(obj, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
			int success = InvStockAdjustment.Save(obj);
			if (RecId > 0)
			{
				MessageBox.Show("Data Update Successfully....  " + obj.DocNo);
				FormHelper.DeleteAllAttachmentsFromDb(AT, RecId, 70, base.Name);
			}
			else
			{
				MessageBox.Show("Data Save Successfully....  " + obj.DocNo);
			}
			Reset();
			if (ChkPrint.Checked)
			{
				GenerateReport(Conversion.ToInt((object)success));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
			if (RecId == 0)
			{
				throw new Exception("Record Not Update because RecId Not Found");
			}
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public void ReadById(int ID)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown
		try
		{
			RecId = ID;
			InvStockAdjustment obj = new InvStockAdjustment();
			obj = InvStockAdjustment.GetByID(ID);
			if (obj != null)
			{
				((Control)(object)CmbEntryType).Enabled = false;
				tabControl1.SelectedIndex = 0;
				DocDate.Value = obj.DocDate;
				txtdocno.Text = Conversion.ToString((object)obj.DocNo);
				txtremarks.Text = Conversion.ToString((object)obj.RemarksHeader);
				CmbEntryType.Value = Conversion.ToInt((object)obj.AdjustmentTypeId);
				CmbEntryType_Leave(null, null);
				table.Rows.Clear();
				for (int i = 0; i < obj.InvStockAdjustmentDetailslist.Count; i++)
				{
					int AccountId = 0;
					AccountId = ((obj.AdjustmentTypeId != 2) ? obj.InvStockAdjustmentDetailslist[i].CreditAccountId : obj.InvStockAdjustmentDetailslist[i].DebitAccountId);
					table.Rows.Add(obj.InvStockAdjustmentDetailslist[i].Id, obj.InvStockAdjustmentDetailslist[i].RefDocumentTypeId, obj.InvStockAdjustmentDetailslist[i].RefDocNoId, obj.InvStockAdjustmentDetailslist[i].RefDocSubIdNo, obj.InvStockAdjustmentDetailslist[i].RefDocumentType, obj.InvStockAdjustmentDetailslist[i].RefDocDate, obj.InvStockAdjustmentDetailslist[i].RefDocNo, obj.InvStockAdjustmentDetailslist[i].WarehouseId, obj.InvStockAdjustmentDetailslist[i].WareHouseName, obj.InvStockAdjustmentDetailslist[i].ItemId, obj.InvStockAdjustmentDetailslist[i].ItemName, obj.InvStockAdjustmentDetailslist[i].ItemCode, obj.InvStockAdjustmentDetailslist[i].CropYearId, obj.InvStockAdjustmentDetailslist[i].CropYear, obj.InvStockAdjustmentDetailslist[i].JobLotId, obj.InvStockAdjustmentDetailslist[i].JobLot, obj.InvStockAdjustmentDetailslist[i].PackingTypeId, obj.InvStockAdjustmentDetailslist[i].PackingType, obj.InvStockAdjustmentDetailslist[i].PackUomId, obj.InvStockAdjustmentDetailslist[i].PackUom, obj.InvStockAdjustmentDetailslist[i].PackUomEquivalent, obj.InvStockAdjustmentDetailslist[i].Qty, obj.InvStockAdjustmentDetailslist[i].NetWeight, obj.InvStockAdjustmentDetailslist[i].ItemRate, obj.InvStockAdjustmentDetailslist[i].RateUomId, obj.InvStockAdjustmentDetailslist[i].RateUom, obj.InvStockAdjustmentDetailslist[i].RateUomEquivalent, obj.InvStockAdjustmentDetailslist[i].Amount, AccountId, obj.InvStockAdjustmentDetailslist[i].RemarksDetail);
				}
				grd.DataSource = table;
				grd.RetrieveStructure();
				grdSettings();
				btnsave.Visible = false;
				btnupdate.Visible = true;
				FormHelper.LoadAttachmentsForObject(AT, RecId, base.Name);
			}
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
			FromDateHistory.Focus();
		}
		else
		{
			DocDate.Focus();
		}
	}

	private void ResetHistory()
	{
		try
		{
			FromDateHistory.Value = DateTime.Now.AddDays(-3.0);
			ToDateHistory.Value = DateTime.Now;
			FromDocNoHistory.Clear();
			ToDocNoHistory.Clear();
			((Control)(object)CmbAdjustmentTypeHistory).Text = string.Empty;
			grdhistory.DataSource = null;
			grdHistoryDetail.DataSource = null;
			FromDateHistory.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void HistoryCombosFill()
	{
		try
		{
			DataTable dt = InvStockAdjustment.GetDataForDropDownFromStockAdjustment(UserAccount.OrganizationId, UserAccount.CompanyId, "StockAdjustmentType");
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbAdjustmentTypeHistory, "Id", "ReferenceName", "StockAdjustmentType", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnRefreshHistory_Click(object sender, EventArgs e)
	{
		try
		{
			HistoryCombosFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnShowHistory_Click(object sender, EventArgs e)
	{
		try
		{
			gridhistoryfill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnNewHistory_Click(object sender, EventArgs e)
	{
		try
		{
			ResetHistory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
			obj.DocumentTypeId = 70;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			if (!obj.CanViewAllRecord)
			{
				obj.EntryUser = clsGlobalVariables.UserAccount.ID;
			}
			if (FromDateHistory.Checked)
			{
				obj.FromDate = Conversion.ToDateTime((object)FromDateHistory.Value);
			}
			if (ToDateHistory.Checked)
			{
				obj.ToDate = Conversion.ToDateTime((object)ToDateHistory.Value);
			}
			obj.FromDocNo = Conversion.ToInt((object)FromDocNoHistory.Text);
			obj.ToDocNo = Conversion.ToInt((object)ToDocNoHistory.Text);
			obj.AdjustmentTypeId = Conversion.ToInt(CmbAdjustmentTypeHistory.Value);
			DataTable dthistoryfill = InvStockAdjustment.FormHistory(obj);
			if (dthistoryfill.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("RecordNo", typeof(int));
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("VoucherHeadId", typeof(int));
				dt.Columns.Add("DocDate", typeof(DateTime));
				dt.Columns.Add("DocNo", typeof(int));
				dt.Columns.Add("RemarksHeader", typeof(string));
				dt.Columns.Add("EntryType", typeof(string));
				dt.Columns.Add("EnteryDate", typeof(DateTime));
				dt.Columns.Add("EntryUser", typeof(string));
				dt.Columns.Add("ModifyDate", typeof(DateTime));
				dt.Columns.Add("ModifyUser", typeof(string));
				dt.Columns.Add("NoOfAttachments", typeof(int));
				foreach (DataRow row in dthistoryfill.Rows)
				{
					dt.Rows.Add(row["RecordNo"], row["Id"], row["VoucherHeadId"], row["DocDate"], row["DocNo"], row["RemarksHeader"], row["EntryType"], row["EnteryDate"], row["EntryUser"], row["ModifyDate"], row["ModifyUser"], row["NoOfAttachments"]);
				}
				grdhistory.DataSource = dt;
				grdhistory.RetrieveStructure();
				HistoryGridSettings();
			}
			else
			{
				grdhistory.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryGridSettings()
	{
		try
		{
			grdhistory.RootTable.Columns["Id"].Visible = false;
			grdhistory.RootTable.Columns["RecordNo"].Visible = false;
			grdhistory.RootTable.Columns["VoucherHeadId"].Visible = false;
			GridEX_Helper.GridWrappingAndColumnSettings(grdhistory, 2, 3, null, null, null, ColumnSetting: false);
			grdhistory.RootTable.Columns["DocDate"].Width = Constants.InventoryConstants.DateConstant;
			grdhistory.RootTable.Columns["DocNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grdhistory.RootTable.Columns["RemarksHeader"].Width = Constants.InventoryConstants.RemarksConstants;
			grdhistory.RootTable.Columns["EntryType"].Width = 70;
			grdhistory.RootTable.Columns["EnteryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			grdhistory.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants + 20;
			grdhistory.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			grdhistory.RootTable.Columns["ModifyUser"].Width = Constants.InventoryConstants.ModifyUserConstants + 20;
			grdhistory.RootTable.Columns["NoOfAttachments"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants - 10;
			grdhistory.RootTable.Columns["EnteryDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			grdhistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			grdhistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdhistory.RootTable.Columns.Add("Print");
			grdhistory.RootTable.Columns["Print"].Caption = "Print";
			grdhistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdhistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			grdhistory.RootTable.Columns["Print"].ButtonText = "Print";
			grdhistory.RootTable.Columns["Print"].Width = 45;
			grdhistory.RootTable.Columns.Add("Voucher");
			grdhistory.RootTable.Columns["Voucher"].Caption = "Voucher";
			grdhistory.RootTable.Columns["Voucher"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdhistory.RootTable.Columns["Voucher"].ButtonStyle = (ButtonStyle)5;
			grdhistory.RootTable.Columns["Voucher"].ButtonText = "Voucher";
			grdhistory.RootTable.Columns["Voucher"].Width = 70;
			grdhistory.RootTable.Columns.Add("Edit");
			grdhistory.RootTable.Columns["Edit"].Caption = "Edit";
			grdhistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdhistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdhistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdhistory.RootTable.Columns["Edit"].Width = 40;
			grdhistory.RootTable.Columns["Edit"].Position = 0;
			grdhistory.RootTable.Columns["Print"].Position = 1;
			grdhistory.RootTable.Columns["Voucher"].Position = 2;
			grdhistory.FrozenColumns = 4;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void grdhistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "NoOfAttachments")
			{
				GridEXRow item = grdhistory.CurrentRow;
				if (item != null && (int)item.RowType == 0)
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(item.Cells["Id"].Value), 70);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Edit")
			{
				RecId = Conversion.ToInt(grdhistory.CurrentRow.Cells["Id"].Value);
				VoucherHeadId = Conversion.ToInt(grdhistory.CurrentRow.Cells["VoucherHeadId"].Value);
				ReadById(RecId);
			}
			if (e.Column.Key == "Print")
			{
				GenerateReport(Conversion.ToInt(grdhistory.CurrentRow.Cells["Id"].Value));
			}
			if (e.Column.Key == "Voucher")
			{
				CommonServices.VoucherReport_118(Conversion.ToInt(grdhistory.CurrentRow.Cells["VoucherHeadId"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			RecId = Conversion.ToInt(grdhistory.CurrentRow.Cells["Id"].Value);
			VoucherHeadId = Conversion.ToInt(grdhistory.CurrentRow.Cells["VoucherHeadId"].Value);
			ReadById(RecId);
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
			GridEXRow r = grdhistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				GridDetailBind(Conversion.ToInt(r.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridDetailBind(int CurrId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			InvStockAdjustment obj = new InvStockAdjustment();
			obj = InvStockAdjustment.GetByID(CurrId);
			if (obj != null)
			{
				DataTable dtdetailhistory = new DataTable();
				dtdetailhistory.Columns.Add("Id", typeof(int));
				dtdetailhistory.Columns.Add("RefDocumentType", typeof(string));
				dtdetailhistory.Columns.Add("RefDocDate", typeof(DateTime));
				dtdetailhistory.Columns.Add("RefDocNo", typeof(int));
				dtdetailhistory.Columns.Add("WareHouse");
				dtdetailhistory.Columns.Add("ItemId");
				dtdetailhistory.Columns.Add("Item");
				dtdetailhistory.Columns.Add("ItemCode");
				dtdetailhistory.Columns.Add("CropYear");
				dtdetailhistory.Columns.Add("JobLot");
				dtdetailhistory.Columns.Add("PackingType");
				dtdetailhistory.Columns.Add("ItemUOM");
				dtdetailhistory.Columns.Add("ItemQty", typeof(double));
				dtdetailhistory.Columns.Add("Weight", typeof(double));
				dtdetailhistory.Columns.Add("ItemRate", typeof(double));
				dtdetailhistory.Columns.Add("RateUom");
				dtdetailhistory.Columns.Add("ItemAmount", typeof(double));
				dtdetailhistory.Columns.Add("AccountId", typeof(int));
				dtdetailhistory.Columns.Add("AccountTitle");
				dtdetailhistory.Columns.Add("Comments");
				table.Rows.Clear();
				for (int i = 0; i < obj.InvStockAdjustmentDetailslist.Count; i++)
				{
					int AccountId = 0;
					AccountId = ((obj.AdjustmentTypeId != 2) ? obj.InvStockAdjustmentDetailslist[i].CreditAccountId : obj.InvStockAdjustmentDetailslist[i].DebitAccountId);
					dtdetailhistory.Rows.Add(obj.InvStockAdjustmentDetailslist[i].Id, obj.InvStockAdjustmentDetailslist[i].RefDocumentType, obj.InvStockAdjustmentDetailslist[i].RefDocDate, obj.InvStockAdjustmentDetailslist[i].RefDocNo, obj.InvStockAdjustmentDetailslist[i].WareHouseName, obj.InvStockAdjustmentDetailslist[i].ItemId, obj.InvStockAdjustmentDetailslist[i].ItemName, obj.InvStockAdjustmentDetailslist[i].ItemCode, obj.InvStockAdjustmentDetailslist[i].CropYear, obj.InvStockAdjustmentDetailslist[i].JobLot, obj.InvStockAdjustmentDetailslist[i].PackingType, obj.InvStockAdjustmentDetailslist[i].PackUom, obj.InvStockAdjustmentDetailslist[i].Qty, obj.InvStockAdjustmentDetailslist[i].NetWeight, obj.InvStockAdjustmentDetailslist[i].ItemRate, obj.InvStockAdjustmentDetailslist[i].RateUom, obj.InvStockAdjustmentDetailslist[i].Amount, AccountId, obj.InvStockAdjustmentDetailslist[i].AccountTitle, obj.InvStockAdjustmentDetailslist[i].RemarksDetail);
				}
				grdHistoryDetail.DataSource = dtdetailhistory;
				grdHistoryDetail.RetrieveStructure();
				DataGridHistoryDetailSetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistoryDetailSetting()
	{
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdHistoryDetail.RootTable.Columns["Id"].Visible = false;
			grdHistoryDetail.RootTable.Columns["ItemId"].Visible = false;
			grdHistoryDetail.RootTable.Columns["AccountId"].Visible = false;
			grdHistoryDetail.RootTable.Columns["RefDocNo"].Visible = false;
			grdHistoryDetail.RootTable.Columns["RefDocDate"].Visible = false;
			grdHistoryDetail.RootTable.Columns["RefDocumentType"].Visible = false;
			GridEXRow r = grdHistoryDetail.GetDataRows().FirstOrDefault((GridEXRow rrr) => Conversion.ToInt(rrr.Cells["RefDocNo"].Value) > 0);
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				grdHistoryDetail.RootTable.Columns["RefDocNo"].Visible = true;
				grdHistoryDetail.RootTable.Columns["RefDocDate"].Visible = true;
				grdHistoryDetail.RootTable.Columns["RefDocumentType"].Visible = true;
			}
			GridEX_Helper.GridWrappingAndColumnSettings(grdHistoryDetail, 2, 3, null, null, null, ColumnSetting: false);
			grdHistoryDetail.RootTable.Columns["RefDocumentType"].Width = Constants.InventoryConstants.DocumentType;
			grdHistoryDetail.RootTable.Columns["RefDocDate"].Width = Constants.InventoryConstants.DateConstant;
			grdHistoryDetail.RootTable.Columns["RefDocNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grdHistoryDetail.RootTable.Columns["WareHouse"].Width = Constants.InventoryConstants.WareHouseName;
			grdHistoryDetail.RootTable.Columns["Item"].Width = Constants.InventoryConstants.ItemName;
			grdHistoryDetail.RootTable.Columns["ItemCode"].Width = Constants.InventoryConstants.ItemCode;
			grdHistoryDetail.RootTable.Columns["CropYear"].Width = Constants.InventoryConstants.CropYearConstant;
			grdHistoryDetail.RootTable.Columns["JobLot"].Width = Constants.InventoryConstants.JobLotConstant;
			grdHistoryDetail.RootTable.Columns["PackingType"].Width = Constants.InventoryConstants.PackingType;
			grdHistoryDetail.RootTable.Columns["ItemUOM"].Width = Constants.InventoryConstants.UomConstant;
			grdHistoryDetail.RootTable.Columns["ItemQty"].Width = Constants.InventoryConstants.QtyConstant;
			grdHistoryDetail.RootTable.Columns["Weight"].Width = Constants.InventoryConstants.WeightConstant;
			grdHistoryDetail.RootTable.Columns["ItemRate"].Width = Constants.InventoryConstants.ExRateConstant;
			grdHistoryDetail.RootTable.Columns["RateUom"].Width = Constants.InventoryConstants.UomConstant;
			grdHistoryDetail.RootTable.Columns["ItemAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdHistoryDetail.RootTable.Columns["AccountTitle"].Width = Constants.InventoryConstants.AccountTitle;
			grdHistoryDetail.RootTable.Columns["Comments"].Width = Constants.InventoryConstants.RemarksConstants;
			grdHistoryDetail.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdHistoryDetail.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdHistoryDetail.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.##";
			grdHistoryDetail.RootTable.Columns["ItemQty"].FormatString = "#,##0.##";
			grdHistoryDetail.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			grdHistoryDetail.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			grdHistoryDetail.RootTable.Columns["Weight"].FormatString = "#,##0.##";
			grdHistoryDetail.RootTable.Columns["Weight"].TotalFormatString = "#,##0.##";
			grdHistoryDetail.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			grdHistoryDetail.RootTable.Columns["ItemRate"].FormatString = "#,##0.###";
			grdHistoryDetail.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			grdHistoryDetail.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			grdHistoryDetail.RootTable.Columns["ItemAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdHistoryDetail.RootTable.Columns["ItemAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnprint_Click(object sender, EventArgs e)
	{
		try
		{
			GenerateReport(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateReport(int PrintId)
	{
		try
		{
			CommonServices.StockAdjustmentSlipAndRegister409(PrintId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnVoucher118_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.VoucherReport_118(Conversion.ToInt((object)VoucherHeadId));
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

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grd).Name);
			if (fs != null)
			{
				grd.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
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

	private void FromDocNoHistory_KeyPress(object sender, KeyPressEventArgs e)
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

	private void ToDocNoHistory_KeyPress(object sender, KeyPressEventArgs e)
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
			if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
			}
			if (tabControl1.SelectedIndex == 0)
			{
				if (e.Control && e.KeyCode == Keys.S && btnsave.Visible && btnsave.Enabled)
				{
					btnsave_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.U && btnupdate.Visible && btnupdate.Enabled)
				{
					btnupdate_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.P && btnprint.Visible && btnprint.Enabled)
				{
					btnprint_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnnew_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.L)
				{
					btnLoadAvailableStock_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefresh_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F10)
				{
					btnattachment_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					DocDate.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Up)
				{
					CmbWareHouse.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grd).Focus();
				}
			}
			else if (tabControl1.SelectedIndex == 1)
			{
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grdHistoryDetail).Focus();
				}
				if (e.Control && e.KeyCode == Keys.S)
				{
					btnShowHistory_Click(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.Up)
				{
					((Control)(object)grdhistory).Focus();
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
			dt.Rows.Add("Ctrl+L", "For Loader");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Ctrl+F5", "For Focus on Doc Date");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+Delete", "For Deleting an row of Focused Grid");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On on Warehouse Combo");
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
				Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
				if (grd.CurrentColumn.Key == "Delete")
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					r.Delete();
					grd.UpdateData();
					if (grd.RowCount == 0)
					{
						((Control)(object)CmbEntryType).Enabled = true;
					}
				}
				if (grd.CurrentColumn.Key == "Edit")
				{
					grd_DoubleClick(null, null);
				}
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				r.Delete();
				grd.UpdateData();
				if (grd.RowCount == 0)
				{
					((Control)(object)CmbEntryType).Enabled = true;
				}
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
			int Id = Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdhistory.CurrentColumn.Key == "Edit")
				{
					RecId = Conversion.ToInt((object)grdhistory.CurrentRow.Cells["Id"].Value.ToString());
					VoucherHeadId = Conversion.ToInt((object)grdhistory.CurrentRow.Cells["VoucherHeadId"].Value.ToString());
					ReadById(RecId);
				}
				if (grdhistory.CurrentColumn.Key == "Print")
				{
					GenerateReport(Conversion.ToInt((object)grdhistory.CurrentRow.Cells["Id"].Value.ToString()));
				}
				if (grdhistory.CurrentColumn.Key == "Voucher")
				{
					CommonServices.VoucherReport_118(Conversion.ToInt((object)grdhistory.CurrentRow.Cells["VoucherHeadId"].Value.ToString()));
				}
				if (grdhistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, 70);
				}
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				if (!formright.DoHaveUpdateRights)
				{
					throw new Exception("you don't have update rights...");
				}
				RecId = Conversion.ToInt((object)grdhistory.CurrentRow.Cells["Id"].Value.ToString());
				VoucherHeadId = Conversion.ToInt((object)grdhistory.CurrentRow.Cells["VoucherHeadId"].Value.ToString());
				ReadById(RecId);
			}
			if (e.Control && e.KeyCode == Keys.P)
			{
				GenerateReport(Conversion.ToInt((object)grdhistory.CurrentRow.Cells["Id"].Value.ToString()));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnshortcut_Click(object sender, EventArgs e)
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

	private void GetAvgRate()
	{
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		try
		{
			if (Conversion.ToInt(CmbEntryType.Value) == 2)
			{
				double rate = Conversion.ToDouble((object)txtItemRate.Text);
				if (FifoMethod)
				{
					txtItemRate.Text = "0";
					txtItemRate.Enabled = true;
					ReportsParameters obj = new ReportsParameters();
					obj.OrganizationId = UserAccount.OrganizationId;
					obj.CompanyId = UserAccount.CompanyId;
					obj.DocDate = Conversion.ToDateTime((object)DocDate.Value);
					obj.stockUOM = Conversion.ToInt(CmbPackUom.Value);
					obj.ItemId = Conversion.ToInt(CmbItemName.Value);
					obj.WarehouseId = Conversion.ToInt(CmbWareHouse.Value);
					obj.PackingTypeId = Conversion.ToInt(CmbPackingType.Value);
					obj.JobLotId = Conversion.ToInt(CmbJobLot.Value);
					obj.CropYearId = Conversion.ToInt(CmbCropYear.Value);
					obj.ItemQty = Conversion.ToDouble((object)txtqty.Text);
					obj.NetWeight = Conversion.ToDouble((object)txtweight.Text);
					if (RecId > 0)
					{
						obj.DocumentTypeId = 70;
						obj.Id = RecId;
					}
					rate = Math.Round(CommonServices.GetAvgRateFromFIFOMethod(obj), 3);
				}
				if (AvgFeatureMethod)
				{
					txtItemRate.Text = "0";
					txtItemRate.Enabled = true;
					rate = CommonServices.AvgRateOnlyForCGS(Conversion.ToInt(CmbItemName.Value), Conversion.ToDateTime((object)DocDate.Value), 70, RecId, Conversion.ToInt(CmbJobLot.Value), Conversion.ToInt(CmbCropYear.Value), null, Conversion.ToInt(CmbWareHouse.Value));
				}
				if (rate > 0.0)
				{
					txtItemRate.Text = Conversion.ToString((object)rate);
					txtItemRate.Enabled = false;
				}
			}
			else
			{
				txtItemRate.Enabled = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateWeight()
	{
		try
		{
			if (txtweight.Text != string.Empty && txtItemRate.Text != string.Empty && ((UltraGridBase)CmbRateUom).ActiveRow != null)
			{
				double Rate = Conversion.ToDouble((object)txtItemRate.Text.Trim());
				double num = Conversion.ToDouble((object)txtweight.Text.Trim());
				double RateUom = Conversion.ToDouble(((UltraDropDownBase)CmbRateUom).SelectedRow.Cells[2].Value);
				double Amount = num / RateUom * Rate;
				txtItemAmount.Text = Amount.ToString();
			}
			else
			{
				txtItemAmount.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combitempck_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)CmbPackUom).ActiveRow != null)
			{
				double ItemPack = Conversion.ToDouble(((UltraDropDownBase)CmbPackUom).SelectedRow.Cells[2].Value);
				double Qty = Conversion.ToDouble((object)txtqty.Text);
				txtweight.Text = (ItemPack * Qty).ToString();
			}
			CalculateWeight();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtqty_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)CmbPackUom).ActiveRow != null)
			{
				double ItemPack = Conversion.ToDouble(((UltraDropDownBase)CmbPackUom).SelectedRow.Cells[2].Value);
				double Qty = Conversion.ToDouble((object)txtqty.Text);
				txtweight.Text = (ItemPack * Qty).ToString();
			}
			CalculateWeight();
			GetAvgRate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtPackSize_TextChanged(object sender, EventArgs e)
	{
		CalculateWeight();
	}

	private void txtweight_TextChanged(object sender, EventArgs e)
	{
		CalculateWeight();
		GetAvgRate();
	}

	private void txtItemRate_TextChanged(object sender, EventArgs e)
	{
		CalculateWeight();
	}

	private void CmbRateUom_TextChanged(object sender, EventArgs e)
	{
		CalculateWeight();
	}

	private void AvailableStock()
	{
		try
		{
			double Balance = CommonServices.GetStockInHandFromInventoryTrasactions(Conversion.ToInt(CmbItemName.Value), DocDate.Value, Conversion.ToInt(CmbJobLot.Value), Conversion.ToInt(CmbWareHouse.Value), Conversion.ToString((object)((Control)(object)CmbCropYear).Text));
			if (Balance > 0.0)
			{
				lblStockBalance.Visible = true;
				lblStockBalance.Text = Balance.ToString("#,#.##");
			}
			else
			{
				lblStockBalance.Visible = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbJobLot_Leave(object sender, EventArgs e)
	{
		AvailableStock();
		GetAvgRate();
	}

	private void CmbWareHouse_Leave(object sender, EventArgs e)
	{
		AvailableStock();
		GetAvgRate();
	}

	private void CmbCropYear_Leave(object sender, EventArgs e)
	{
		AvailableStock();
		GetAvgRate();
	}

	private void CmbItemName_Leave(object sender, EventArgs e)
	{
		try
		{
			bindRateUomAndItemPackUom(Conversion.ToInt(CmbItemName.Value));
			AvailableStock();
			GetAvgRate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnLoadAvailableStock_Click(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToString((object)((Control)(object)CmbEntryType).Text) == "Loss")
			{
				if (grd.GetDataRows().Count() > 0)
				{
					GridEXRow[] rows = grd.GetRows();
					foreach (GridEXRow item in rows)
					{
						if (Conversion.ToInt(item.Cells["RefDocumentTypeId"].Value) == 0 || Conversion.ToInt(item.Cells["RefDocNoId"].Value) == 0 || Conversion.ToInt(item.Cells["RefDocSubIdNo"].Value) == 0)
						{
							throw new Exception("A record without the loader exists in the grid, so you can't add a record from the loader.");
						}
					}
				}
				LoadTransactions = new LoadavailableTransactionsForIssuance(UserAccount);
				LoadTransactions.ShowDialog();
				LoadTransactions.BringToFront();
				LoadDetailDataFromavailableTransactions();
				return;
			}
			CmbEntryType.Focus();
			throw new Exception("You can Only Load Data Against EntryType 'Loss'");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadDetailDataFromavailableTransactions()
	{
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dtAT = LoadTransactions.dtIssuance;
			if (dtAT.Rows.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < dtAT.Rows.Count; i++)
			{
				bool flag = false;
				if (grd.RowCount > 0)
				{
					GridEXRow[] rows = grd.GetRows();
					foreach (GridEXRow item in rows)
					{
						if (item != null && (int)item.RowType == 0)
						{
							if (Conversion.ToInt(item.Cells["RefDocumentTypeId"].Value) == 0 || Conversion.ToInt(item.Cells["RefDocNoId"].Value) == 0)
							{
								item.Delete();
							}
							if (grd.GetRows().Length != 0 && Conversion.ToInt(dtAT.Rows[i]["RefDocumentTypeId"]) == Conversion.ToInt(item.Cells["RefDocumentTypeId"].Value) && Conversion.ToInt(dtAT.Rows[i]["RefDocIdNo"]) == Conversion.ToInt(item.Cells["RefDocNoId"].Value) && Conversion.ToInt(dtAT.Rows[i]["RefDocSubIdNo"]) == Conversion.ToInt(item.Cells["RefDocSubIdNo"].Value))
							{
								flag = true;
							}
						}
					}
				}
				if (!flag && !flag)
				{
					table.Rows.Add(0, dtAT.Rows[i]["RefDocumentTypeId"], dtAT.Rows[i]["RefDocIdNo"], dtAT.Rows[i]["RefDocSubIdNo"], dtAT.Rows[i]["RefDocumentType"], dtAT.Rows[i]["DocDate"], dtAT.Rows[i]["DocNo"], dtAT.Rows[i]["WareHouseId"], dtAT.Rows[i]["WareHouse"], dtAT.Rows[i]["ItemId"], dtAT.Rows[i]["ItemName"], dtAT.Rows[i]["ItemCode"], dtAT.Rows[i]["CropYearId"], dtAT.Rows[i]["CropYear"], dtAT.Rows[i]["JobLotId"], dtAT.Rows[i]["JobLotCode"], dtAT.Rows[i]["PackingTypeId"], dtAT.Rows[i]["PackTypeCode"], dtAT.Rows[i]["ItemUOMId"], dtAT.Rows[i]["PackUom"], dtAT.Rows[i]["ItemUOM"], dtAT.Rows[i]["ItemQty"], dtAT.Rows[i]["Weight"], dtAT.Rows[i]["ItemRate"], dtAT.Rows[i]["RateUOMId"], dtAT.Rows[i]["RateUOM"], dtAT.Rows[i]["Equivalent"], dtAT.Rows[i]["ItemAmount"], 0, "");
				}
			}
			grd.DataSource = table;
			grd.RetrieveStructure();
			grdSettings();
			((Control)(object)CmbEntryType).Enabled = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AvgRateUpdateOnDocDateChange()
	{
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Expected O, but got Unknown
		try
		{
			if (grd.RowCount <= 0 || Conversion.ToInt(CmbEntryType.Value) != 2)
			{
				return;
			}
			GridEXRow[] dataRows = grd.GetDataRows();
			foreach (GridEXRow r in dataRows)
			{
				r.BeginEdit();
				int RefDocumentTypeId = Conversion.ToInt(r.Cells["RefDocumentTypeId"].Value);
				int RefDocNoId = Conversion.ToInt(r.Cells["RefDocNoId"].Value);
				int RefDocSubIdNo = Conversion.ToInt(r.Cells["RefDocSubIdNo"].Value);
				int ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
				int WareHouseId = Conversion.ToInt(r.Cells["WareHouseId"].Value);
				int CropYearId = Conversion.ToInt(r.Cells["CropYearId"].Value);
				int JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
				int PackingTypeId = Conversion.ToInt(r.Cells["PackingTypeId"].Value);
				int ItemUOMId = Conversion.ToInt(r.Cells["ItemUOMId"].Value);
				double Qty = Conversion.ToDouble(r.Cells["ItemQty"].Value);
				double Weight = Conversion.ToDouble(r.Cells["Weight"].Value);
				double RateUomEquivalent = Conversion.ToDouble(r.Cells["RateUomEquivalent"].Value);
				double rate = 0.0;
				if (RefDocumentTypeId != 0 || RefDocNoId != 0 || RefDocSubIdNo != 0)
				{
					break;
				}
				if (FifoMethod)
				{
					ReportsParameters obj = new ReportsParameters
					{
						OrganizationId = UserAccount.OrganizationId,
						CompanyId = UserAccount.CompanyId,
						DocDate = Conversion.ToDateTime((object)DocDate.Value),
						stockUOM = ItemUOMId,
						ItemId = ItemId,
						WarehouseId = WareHouseId,
						PackingTypeId = PackingTypeId,
						JobLotId = JobLotId,
						CropYearId = CropYearId,
						ItemQty = Qty,
						NetWeight = Weight
					};
					if (RecId > 0)
					{
						obj.DocumentTypeId = 70;
						obj.Id = RecId;
					}
					rate = Math.Round(CommonServices.GetAvgRateFromFIFOMethod(obj), 3);
				}
				else if (AvgFeatureMethod)
				{
					rate = CommonServices.AvgRateOnlyForCGS(ItemId, Conversion.ToDateTime((object)DocDate.Value), 70, RecId, JobLotId, CropYearId, null, WareHouseId);
				}
				if (rate > 0.0)
				{
					r.Cells["ItemRate"].Value = rate;
					r.Cells["ItemAmount"].Value = Weight / RateUomEquivalent * rate;
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

	private void DocDate_ValueChanged(object sender, EventArgs e)
	{
	}

	private void btnGenerateRate_Click(object sender, EventArgs e)
	{
		try
		{
			AvgRateUpdateOnDocDateChange();
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
		//IL_037b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0385: Expected O, but got Unknown
		//IL_03d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03dd: Expected O, but got Unknown
		//IL_03de: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e8: Expected O, but got Unknown
		//IL_0462: Unknown result type (might be due to invalid IL or missing references)
		//IL_046c: Expected O, but got Unknown
		//IL_046d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0477: Expected O, but got Unknown
		//IL_0478: Unknown result type (might be due to invalid IL or missing references)
		//IL_0482: Expected O, but got Unknown
		//IL_048e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0498: Expected O, but got Unknown
		//IL_0499: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a3: Expected O, but got Unknown
		//IL_04c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cf: Expected O, but got Unknown
		//IL_051d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0527: Expected O, but got Unknown
		//IL_0646: Unknown result type (might be due to invalid IL or missing references)
		//IL_0650: Expected O, but got Unknown
		//IL_0701: Unknown result type (might be due to invalid IL or missing references)
		//IL_070b: Expected O, but got Unknown
		//IL_0738: Unknown result type (might be due to invalid IL or missing references)
		//IL_0742: Expected O, but got Unknown
		//IL_0c23: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c2d: Expected O, but got Unknown
		//IL_0c3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c44: Expected O, but got Unknown
		//IL_5257: Unknown result type (might be due to invalid IL or missing references)
		//IL_5261: Expected O, but got Unknown
		//IL_5285: Unknown result type (might be due to invalid IL or missing references)
		//IL_528f: Expected O, but got Unknown
		//IL_6464: Unknown result type (might be due to invalid IL or missing references)
		//IL_646e: Expected O, but got Unknown
		//IL_647b: Unknown result type (might be due to invalid IL or missing references)
		//IL_6485: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
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
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
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
		GridEXLayout grdhistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance78 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance90 = new Appearance();
		GridEXLayout grdHistoryDetail_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.grd = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel5 = new System.Windows.Forms.Panel();
		this.btnplus = new System.Windows.Forms.Button();
		this.rdbtnItemCode = new System.Windows.Forms.RadioButton();
		this.rdbtnItemName = new System.Windows.Forms.RadioButton();
		this.lblStockBalance = new System.Windows.Forms.Label();
		this.CmbRateUom = new UltraCombo();
		this.CmbCropYear = new UltraCombo();
		this.label2 = new System.Windows.Forms.Label();
		this.label37 = new System.Windows.Forms.Label();
		this.label36 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label35 = new System.Windows.Forms.Label();
		this.txtItemAmount = new System.Windows.Forms.TextBox();
		this.label47 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.txtremarksdetail = new System.Windows.Forms.TextBox();
		this.txtItemRate = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.CmbItemName = new UltraCombo();
		this.CmbJobLot = new UltraCombo();
		this.CmbPackUom = new UltraCombo();
		this.label8 = new System.Windows.Forms.Label();
		this.CmbPackingType = new UltraCombo();
		this.CmbAccountTitle = new UltraCombo();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.label9 = new System.Windows.Forms.Label();
		this.label38 = new System.Windows.Forms.Label();
		this.CmbWareHouse = new UltraCombo();
		this.btnCancelUpdateDetial = new System.Windows.Forms.Button();
		this.label3 = new System.Windows.Forms.Label();
		this.txtqty = new System.Windows.Forms.TextBox();
		this.txtweight = new System.Windows.Forms.TextBox();
		this.label39 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label4 = new System.Windows.Forms.Label();
		this.CmbEntryType = new UltraCombo();
		this.txtremarks = new System.Windows.Forms.TextBox();
		this.DocDate = new System.Windows.Forms.DateTimePicker();
		this.label33 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.label5 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.ChkPrint = new System.Windows.Forms.CheckBox();
		this.label23 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.btnattachment = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
		this.btnprint = new System.Windows.Forms.ToolStripButton();
		this.btnVoucher118 = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.btnLoadAvailableStock = new System.Windows.Forms.ToolStripButton();
		this.btnshortcut = new System.Windows.Forms.ToolStripButton();
		this.btnGenerateRate = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.grdhistory = new GridEX();
		this.panel12 = new System.Windows.Forms.Panel();
		this.label11 = new System.Windows.Forms.Label();
		this.panel7 = new System.Windows.Forms.Panel();
		this.label78 = new System.Windows.Forms.Label();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel9 = new System.Windows.Forms.Panel();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.btnShowHistory = new System.Windows.Forms.Button();
		this.label50 = new System.Windows.Forms.Label();
		this.label51 = new System.Windows.Forms.Label();
		this.label52 = new System.Windows.Forms.Label();
		this.label53 = new System.Windows.Forms.Label();
		this.label54 = new System.Windows.Forms.Label();
		this.CmbAdjustmentTypeHistory = new UltraCombo();
		this.ToDocNoHistory = new System.Windows.Forms.TextBox();
		this.FromDocNoHistory = new System.Windows.Forms.TextBox();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.grdHistoryDetail = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.label10 = new System.Windows.Forms.Label();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel6.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbRateUom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYear).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackingType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAccountTitle).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWareHouse).BeginInit();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbEntryType).BeginInit();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdhistory).BeginInit();
		this.panel12.SuspendLayout();
		this.panel7.SuspendLayout();
		this.panel9.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbAdjustmentTypeHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryDetail).BeginInit();
		this.panel8.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1079, 586);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1079, 586);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.tabPage1.Controls.Add(this.panel6);
		this.tabPage1.Controls.Add(this.panel5);
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage1.Size = new System.Drawing.Size(1071, 557);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.grd.AllowDelete = (InheritableBoolean)1;
		this.grd.AutomaticSort = false;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		this.grd.ColumnAutoResize = true;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(2, 269);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1067, 286);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 2;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.CellUpdated += new ColumnActionEventHandler(grd_CellUpdated);
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grd_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.ctrlGrdBar1);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(2, 247);
		this.panel6.Margin = new System.Windows.Forms.Padding(2);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1067, 22);
		this.panel6.TabIndex = 4;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1327, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(59, 26);
		this.ctrlGrdBar1.TabIndex = 20;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.panel5.BackColor = System.Drawing.Color.White;
		this.panel5.Controls.Add(this.btnplus);
		this.panel5.Controls.Add(this.rdbtnItemCode);
		this.panel5.Controls.Add(this.rdbtnItemName);
		this.panel5.Controls.Add(this.lblStockBalance);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbRateUom);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCropYear);
		this.panel5.Controls.Add(this.label2);
		this.panel5.Controls.Add(this.label37);
		this.panel5.Controls.Add(this.label36);
		this.panel5.Controls.Add(this.label7);
		this.panel5.Controls.Add(this.label35);
		this.panel5.Controls.Add(this.txtItemAmount);
		this.panel5.Controls.Add(this.label47);
		this.panel5.Controls.Add(this.label6);
		this.panel5.Controls.Add(this.txtremarksdetail);
		this.panel5.Controls.Add(this.txtItemRate);
		this.panel5.Controls.Add(this.label1);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemName);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobLot);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPackUom);
		this.panel5.Controls.Add(this.label8);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPackingType);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbAccountTitle);
		this.panel5.Controls.Add(this.btnUpdateDetail);
		this.panel5.Controls.Add(this.label9);
		this.panel5.Controls.Add(this.label38);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWareHouse);
		this.panel5.Controls.Add(this.btnCancelUpdateDetial);
		this.panel5.Controls.Add(this.label3);
		this.panel5.Controls.Add(this.txtqty);
		this.panel5.Controls.Add(this.txtweight);
		this.panel5.Controls.Add(this.label39);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(2, 143);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1067, 104);
		this.panel5.TabIndex = 1;
		this.btnplus.BackColor = System.Drawing.Color.Teal;
		this.btnplus.FlatAppearance.BorderSize = 0;
		this.btnplus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnplus.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnplus.ForeColor = System.Drawing.Color.White;
		this.btnplus.Location = new System.Drawing.Point(811, 73);
		this.btnplus.Name = "btnplus";
		this.btnplus.Size = new System.Drawing.Size(39, 26);
		this.btnplus.TabIndex = 13;
		this.btnplus.Text = "+";
		this.btnplus.UseVisualStyleBackColor = false;
		this.btnplus.Click += new System.EventHandler(btnplus_Click);
		this.rdbtnItemCode.AutoSize = true;
		this.rdbtnItemCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdbtnItemCode.Location = new System.Drawing.Point(275, 5);
		this.rdbtnItemCode.Margin = new System.Windows.Forms.Padding(2);
		this.rdbtnItemCode.Name = "rdbtnItemCode";
		this.rdbtnItemCode.Size = new System.Drawing.Size(52, 19);
		this.rdbtnItemCode.TabIndex = 129;
		this.rdbtnItemCode.Text = "Code";
		this.rdbtnItemCode.UseVisualStyleBackColor = true;
		this.rdbtnItemName.AutoSize = true;
		this.rdbtnItemName.Checked = true;
		this.rdbtnItemName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdbtnItemName.Location = new System.Drawing.Point(222, 5);
		this.rdbtnItemName.Margin = new System.Windows.Forms.Padding(2);
		this.rdbtnItemName.Name = "rdbtnItemName";
		this.rdbtnItemName.Size = new System.Drawing.Size(57, 19);
		this.rdbtnItemName.TabIndex = 128;
		this.rdbtnItemName.TabStop = true;
		this.rdbtnItemName.Text = "Name";
		this.rdbtnItemName.UseVisualStyleBackColor = true;
		this.rdbtnItemName.CheckedChanged += new System.EventHandler(rdbtnItemName_CheckedChanged);
		this.lblStockBalance.AutoSize = true;
		this.lblStockBalance.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblStockBalance.Location = new System.Drawing.Point(332, 7);
		this.lblStockBalance.Name = "lblStockBalance";
		this.lblStockBalance.Size = new System.Drawing.Size(48, 15);
		this.lblStockBalance.TabIndex = 49;
		this.lblStockBalance.Text = "Balance";
		this.CmbRateUom.AllowNull = (DefaultableBoolean)1;
		this.CmbRateUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbRateUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbRateUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance93).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance93).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbRateUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance94;
		((SpecialBoxBase)((UltraGridBase)this.CmbRateUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance95).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance95).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance96).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance97).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance98;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance99).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance100).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance101).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbRateUom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Location = new System.Drawing.Point(187, 73);
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Name = "CmbRateUom";
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Size = new System.Drawing.Size(83, 26);
		((System.Windows.Forms.Control)(object)this.CmbRateUom).TabIndex = 9;
		((System.Windows.Forms.Control)(object)this.CmbRateUom).TextChanged += new System.EventHandler(CmbRateUom_TextChanged);
		this.CmbCropYear.AllowNull = (DefaultableBoolean)1;
		this.CmbCropYear.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCropYear.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCropYear.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Appearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance106).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance106).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance106;
		((AppearanceBase)appearance107).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance107;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance108).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance108).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance108).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance109).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance109;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance111).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance113).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance114).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance114).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance114).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance114).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance114;
		((AppearanceBase)appearance115).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance116).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbCropYear.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Location = new System.Drawing.Point(426, 26);
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Name = "CmbCropYear";
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Size = new System.Drawing.Size(94, 26);
		((System.Windows.Forms.Control)(object)this.CmbCropYear).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Leave += new System.EventHandler(CmbCropYear_Leave);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label2.Location = new System.Drawing.Point(187, 56);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(59, 15);
		this.label2.TabIndex = 48;
		this.label2.Text = "Rate Uom";
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label37.Location = new System.Drawing.Point(188, 7);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(32, 15);
		this.label37.TabIndex = 9;
		this.label37.Text = "Item";
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label36.Location = new System.Drawing.Point(781, 7);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(63, 15);
		this.label36.TabIndex = 8;
		this.label36.Text = "Pack UOM";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label7.Location = new System.Drawing.Point(427, 7);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(54, 15);
		this.label7.TabIndex = 41;
		this.label7.Text = "CropYear";
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label35.Location = new System.Drawing.Point(675, 7);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(74, 15);
		this.label35.TabIndex = 25;
		this.label35.Text = "PackingType";
		this.txtItemAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtItemAmount.Location = new System.Drawing.Point(273, 75);
		this.txtItemAmount.Name = "txtItemAmount";
		this.txtItemAmount.ReadOnly = true;
		this.txtItemAmount.Size = new System.Drawing.Size(107, 23);
		this.txtItemAmount.TabIndex = 10;
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label47.Location = new System.Drawing.Point(549, 56);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(65, 15);
		this.label47.TabIndex = 25;
		this.label47.Text = "Comments";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label6.Location = new System.Drawing.Point(273, 56);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(79, 15);
		this.label6.TabIndex = 39;
		this.label6.Text = "Item Amount";
		this.txtremarksdetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtremarksdetail.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtremarksdetail.Location = new System.Drawing.Point(552, 75);
		this.txtremarksdetail.Name = "txtremarksdetail";
		this.txtremarksdetail.Size = new System.Drawing.Size(256, 23);
		this.txtremarksdetail.TabIndex = 12;
		this.txtItemRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemRate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtItemRate.Location = new System.Drawing.Point(104, 75);
		this.txtItemRate.Name = "txtItemRate";
		this.txtItemRate.Size = new System.Drawing.Size(80, 23);
		this.txtItemRate.TabIndex = 8;
		this.txtItemRate.TextChanged += new System.EventHandler(txtItemRate_TextChanged);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label1.Location = new System.Drawing.Point(104, 56);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(58, 15);
		this.label1.TabIndex = 37;
		this.label1.Text = "Item Rate";
		this.CmbItemName.AllowNull = (DefaultableBoolean)1;
		this.CmbItemName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.CmbItemName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance62).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance62).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance62).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance62;
		((AppearanceBase)appearance63).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance63;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance71).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance71).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance73).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance73;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance74).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance75;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance76).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance77).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance77).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance77).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance77).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance77;
		((AppearanceBase)appearance68).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance69).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbItemName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Location = new System.Drawing.Point(187, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemName).Name = "CmbItemName";
		((System.Windows.Forms.Control)(object)this.CmbItemName).Size = new System.Drawing.Size(237, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemName).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Leave += new System.EventHandler(CmbItemName_Leave);
		this.CmbJobLot.AllowNull = (DefaultableBoolean)1;
		this.CmbJobLot.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobLot.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobLot.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Appearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance29;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance30;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance31).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance31).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance35;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance36).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance37).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance37).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance37).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance37;
		((AppearanceBase)appearance38).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbJobLot.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Location = new System.Drawing.Point(522, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Name = "CmbJobLot";
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Size = new System.Drawing.Size(150, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Leave += new System.EventHandler(CmbJobLot_Leave);
		this.CmbPackUom.AllowNull = (DefaultableBoolean)1;
		this.CmbPackUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance72).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance72).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance82;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance91).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance91).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance91).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance102).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance102;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance110).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance2).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance12).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance13).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance13).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance13).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance13;
		((AppearanceBase)appearance23).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbPackUom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Location = new System.Drawing.Point(781, 26);
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Name = "CmbPackUom";
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Size = new System.Drawing.Size(96, 26);
		((System.Windows.Forms.Control)(object)this.CmbPackUom).TabIndex = 5;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Leave += new System.EventHandler(combitempck_Leave);
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label8.Location = new System.Drawing.Point(527, 7);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(43, 15);
		this.label8.TabIndex = 35;
		this.label8.Text = "JobLot";
		this.CmbPackingType.AllowNull = (DefaultableBoolean)1;
		this.CmbPackingType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackingType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackingType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance15).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance15).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance15;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance16;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance17).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance17).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance18;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance21).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance22).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance22).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance22).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance22).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance24).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbPackingType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Location = new System.Drawing.Point(675, 26);
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Name = "CmbPackingType";
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Size = new System.Drawing.Size(103, 26);
		((System.Windows.Forms.Control)(object)this.CmbPackingType).TabIndex = 4;
		this.CmbAccountTitle.AllowNull = (DefaultableBoolean)1;
		this.CmbAccountTitle.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbAccountTitle.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbAccountTitle.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Appearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance33).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance33).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbAccountTitle).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance41;
		((SpecialBoxBase)((UltraGridBase)this.CmbAccountTitle).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance51).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance51).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance51).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance52;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance53).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance54;
		((AppearanceBase)appearance55).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance55).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance57).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance57).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance57).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance57).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance57;
		((AppearanceBase)appearance58).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbAccountTitle).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbAccountTitle).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbAccountTitle.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbAccountTitle).Location = new System.Drawing.Point(383, 73);
		((System.Windows.Forms.Control)(object)this.CmbAccountTitle).Name = "CmbAccountTitle";
		((System.Windows.Forms.Control)(object)this.CmbAccountTitle).Size = new System.Drawing.Size(166, 26);
		((System.Windows.Forms.Control)(object)this.CmbAccountTitle).TabIndex = 11;
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatAppearance.BorderSize = 0;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(811, 73);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(78, 26);
		this.btnUpdateDetail.TabIndex = 14;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Visible = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label9.Location = new System.Drawing.Point(380, 56);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(78, 15);
		this.label9.TabIndex = 33;
		this.label9.Text = "Account Title";
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label38.Location = new System.Drawing.Point(879, 7);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(26, 15);
		this.label38.TabIndex = 13;
		this.label38.Text = "Qty";
		this.CmbWareHouse.AllowNull = (DefaultableBoolean)1;
		this.CmbWareHouse.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWareHouse.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWareHouse.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Appearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance56).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance56).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance56).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWareHouse).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance67;
		((SpecialBoxBase)((UltraGridBase)this.CmbWareHouse).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance49).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance50).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance50).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance50).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance50;
		((AppearanceBase)appearance64).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbWareHouse.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).Location = new System.Drawing.Point(7, 26);
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).Name = "CmbWareHouse";
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).Size = new System.Drawing.Size(177, 26);
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).Leave += new System.EventHandler(CmbWareHouse_Leave);
		this.btnCancelUpdateDetial.BackColor = System.Drawing.Color.Teal;
		this.btnCancelUpdateDetial.FlatAppearance.BorderSize = 0;
		this.btnCancelUpdateDetial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelUpdateDetial.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelUpdateDetial.ForeColor = System.Drawing.Color.White;
		this.btnCancelUpdateDetial.Location = new System.Drawing.Point(892, 73);
		this.btnCancelUpdateDetial.Name = "btnCancelUpdateDetial";
		this.btnCancelUpdateDetial.Size = new System.Drawing.Size(70, 26);
		this.btnCancelUpdateDetial.TabIndex = 15;
		this.btnCancelUpdateDetial.Text = "Cancel";
		this.btnCancelUpdateDetial.UseVisualStyleBackColor = false;
		this.btnCancelUpdateDetial.Visible = false;
		this.btnCancelUpdateDetial.Click += new System.EventHandler(btnCancelUpdateDetial_Click);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label3.Location = new System.Drawing.Point(7, 7);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(69, 15);
		this.label3.TabIndex = 33;
		this.label3.Text = "WareHouse";
		this.txtqty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtqty.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtqty.Location = new System.Drawing.Point(880, 28);
		this.txtqty.Name = "txtqty";
		this.txtqty.Size = new System.Drawing.Size(82, 23);
		this.txtqty.TabIndex = 6;
		this.txtqty.TextChanged += new System.EventHandler(txtqty_TextChanged);
		this.txtqty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtqty_KeyPress);
		this.txtweight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtweight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtweight.Location = new System.Drawing.Point(7, 75);
		this.txtweight.Name = "txtweight";
		this.txtweight.Size = new System.Drawing.Size(96, 23);
		this.txtweight.TabIndex = 7;
		this.txtweight.TextChanged += new System.EventHandler(txtweight_TextChanged);
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label39.Location = new System.Drawing.Point(7, 56);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(46, 15);
		this.label39.TabIndex = 15;
		this.label39.Text = "Weight";
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.label4);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbEntryType);
		this.panel4.Controls.Add(this.txtremarks);
		this.panel4.Controls.Add(this.DocDate);
		this.panel4.Controls.Add(this.label33);
		this.panel4.Controls.Add(this.label17);
		this.panel4.Controls.Add(this.txtdocno);
		this.panel4.Controls.Add(this.label5);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(2, 55);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1067, 88);
		this.panel4.TabIndex = 0;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(7, 64);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(62, 15);
		this.label4.TabIndex = 27;
		this.label4.Text = "Entry Type";
		this.CmbEntryType.AllowNull = (DefaultableBoolean)1;
		this.CmbEntryType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbEntryType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbEntryType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance118).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance119).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance119).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbEntryType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance119;
		((AppearanceBase)appearance120).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance120;
		((SpecialBoxBase)((UltraGridBase)this.CmbEntryType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbEntryType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbEntryType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbEntryType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbEntryType).Location = new System.Drawing.Point(71, 58);
		((System.Windows.Forms.Control)(object)this.CmbEntryType).Name = "CmbEntryType";
		((System.Windows.Forms.Control)(object)this.CmbEntryType).Size = new System.Drawing.Size(166, 26);
		((System.Windows.Forms.Control)(object)this.CmbEntryType).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbEntryType).Leave += new System.EventHandler(CmbEntryType_Leave);
		this.txtremarks.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtremarks.Location = new System.Drawing.Point(242, 32);
		this.txtremarks.Multiline = true;
		this.txtremarks.Name = "txtremarks";
		this.txtremarks.Size = new System.Drawing.Size(720, 52);
		this.txtremarks.TabIndex = 3;
		this.DocDate.CustomFormat = "dd-MMM-yy";
		this.DocDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DocDate.Location = new System.Drawing.Point(71, 32);
		this.DocDate.Name = "DocDate";
		this.DocDate.Size = new System.Drawing.Size(166, 23);
		this.DocDate.TabIndex = 1;
		this.DocDate.TabStop = false;
		this.DocDate.ValueChanged += new System.EventHandler(DocDate_ValueChanged);
		this.label33.BackColor = System.Drawing.Color.LightGray;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 11f, System.Drawing.FontStyle.Bold);
		this.label33.Location = new System.Drawing.Point(242, 6);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(720, 23);
		this.label33.TabIndex = 8;
		this.label33.Text = "Remarks";
		this.label33.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(7, 10);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(48, 15);
		this.label17.TabIndex = 3;
		this.label17.Text = "Doc No";
		this.txtdocno.BackColor = System.Drawing.Color.White;
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocno.Location = new System.Drawing.Point(71, 6);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.Size = new System.Drawing.Size(166, 23);
		this.txtdocno.TabIndex = 0;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(7, 36);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(57, 15);
		this.label5.TabIndex = 3;
		this.label5.Text = "Doc Date";
		this.panel3.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel3.Controls.Add(this.ChkPrint);
		this.panel3.Controls.Add(this.label23);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(2, 27);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1067, 28);
		this.panel3.TabIndex = 1;
		this.ChkPrint.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkPrint.AutoSize = true;
		this.ChkPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkPrint.ForeColor = System.Drawing.Color.White;
		this.ChkPrint.Location = new System.Drawing.Point(964, 4);
		this.ChkPrint.Name = "ChkPrint";
		this.ChkPrint.Size = new System.Drawing.Size(102, 20);
		this.ChkPrint.TabIndex = 0;
		this.ChkPrint.Text = "Is Preview";
		this.ChkPrint.UseVisualStyleBackColor = true;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(8, 4);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(154, 19);
		this.label23.TabIndex = 0;
		this.label23.Text = "Stock Adjustment";
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(2, 2);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1067, 25);
		this.panel2.TabIndex = 0;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[14]
		{
			this.btnnew, this.btnRefresh, this.toolStripSeparator2, this.btnupdate, this.btnsave, this.toolStripSeparator3, this.btnattachment, this.toolStripSeparator4, this.btnprint, this.btnVoucher118,
			this.toolStripSeparator1, this.btnLoadAvailableStock, this.btnshortcut, this.btnGenerateRate
		});
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1067, 27);
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
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
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
		this.toolStripSeparator3.Name = "toolStripSeparator3";
		this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
		this.btnattachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnattachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnattachment.Name = "btnattachment";
		this.btnattachment.Size = new System.Drawing.Size(109, 24);
		this.btnattachment.Text = "&Attachment";
		this.btnattachment.Click += new System.EventHandler(btnattachment_Click);
		this.toolStripSeparator4.Name = "toolStripSeparator4";
		this.toolStripSeparator4.Size = new System.Drawing.Size(6, 27);
		this.btnprint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnprint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnprint.Name = "btnprint";
		this.btnprint.Size = new System.Drawing.Size(92, 24);
		this.btnprint.Text = "409-Print";
		this.btnprint.Click += new System.EventHandler(btnprint_Click);
		this.btnVoucher118.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnVoucher118.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnVoucher118.Name = "btnVoucher118";
		this.btnVoucher118.Size = new System.Drawing.Size(115, 24);
		this.btnVoucher118.Text = "Voucher-118";
		this.btnVoucher118.Click += new System.EventHandler(btnVoucher118_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.btnLoadAvailableStock.Image = Architecture.WinApp.Properties.Resources.download;
		this.btnLoadAvailableStock.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadAvailableStock.Name = "btnLoadAvailableStock";
		this.btnLoadAvailableStock.Size = new System.Drawing.Size(75, 24);
		this.btnLoadAvailableStock.Text = "Loader";
		this.btnLoadAvailableStock.Click += new System.EventHandler(btnLoadAvailableStock_Click);
		this.btnshortcut.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnshortcut.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnshortcut.Name = "btnshortcut";
		this.btnshortcut.Size = new System.Drawing.Size(126, 24);
		this.btnshortcut.Text = "ShortCut Keys";
		this.btnshortcut.Click += new System.EventHandler(btnshortcut_Click);
		this.btnGenerateRate.Image = Architecture.WinApp.Properties.Resources.Taxation;
		this.btnGenerateRate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnGenerateRate.Name = "btnGenerateRate";
		this.btnGenerateRate.Size = new System.Drawing.Size(126, 24);
		this.btnGenerateRate.Text = "Generate Rate";
		this.btnGenerateRate.Click += new System.EventHandler(btnGenerateRate_Click);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.grdhistory);
		this.tabPage2.Controls.Add(this.panel12);
		this.tabPage2.Controls.Add(this.panel7);
		this.tabPage2.Controls.Add(this.panel9);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistoryDetail);
		this.tabPage2.Controls.Add(this.panel8);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage2.Size = new System.Drawing.Size(976, 557);
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
		((System.Windows.Forms.Control)(object)this.grdhistory).Location = new System.Drawing.Point(2, 148);
		((System.Windows.Forms.Control)(object)this.grdhistory).Name = "grdhistory";
		((ScrollableBase)this.grdhistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdhistory).Size = new System.Drawing.Size(972, 212);
		((System.Windows.Forms.Control)(object)this.grdhistory).TabIndex = 1;
		this.grdhistory.TotalRow = (InheritableBoolean)1;
		this.grdhistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdhistory).VisualStyle = (VisualStyle)4;
		this.grdhistory.ColumnButtonClick += new ColumnActionEventHandler(grdhistory_ColumnButtonClick);
		this.grdhistory.SelectionChanged += new System.EventHandler(grdhistory_SelectionChanged);
		this.grdhistory.LinkClicked += new ColumnActionEventHandler(grdhistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdhistory).DoubleClick += new System.EventHandler(grdhistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdhistory).KeyDown += new System.Windows.Forms.KeyEventHandler(grdhistory_KeyDown);
		this.panel12.BackColor = System.Drawing.Color.Teal;
		this.panel12.Controls.Add(this.label11);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel12.Location = new System.Drawing.Point(2, 360);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(972, 18);
		this.panel12.TabIndex = 46;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.label11.ForeColor = System.Drawing.Color.White;
		this.label11.Location = new System.Drawing.Point(0, 2);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(212, 14);
		this.label11.TabIndex = 36;
		this.label11.Text = "Detail of The Selected Above row";
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.label78);
		this.panel7.Controls.Add(this.ctrlGrdBar2);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(2, 125);
		this.panel7.Margin = new System.Windows.Forms.Padding(2);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(972, 23);
		this.panel7.TabIndex = 0;
		this.label78.AutoSize = true;
		this.label78.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label78.ForeColor = System.Drawing.Color.White;
		this.label78.Location = new System.Drawing.Point(2, 2);
		this.label78.Name = "label78";
		this.label78.Size = new System.Drawing.Size(40, 17);
		this.label78.TabIndex = 36;
		this.label78.Text = "Main";
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(928, 1);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdhistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(40, 21);
		this.ctrlGrdBar2.TabIndex = 21;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.panel9.BackColor = System.Drawing.Color.White;
		this.panel9.Controls.Add(this.rdmodifydate);
		this.panel9.Controls.Add(this.drdocdate);
		this.panel9.Controls.Add(this.rdentrydate);
		this.panel9.Controls.Add(this.groupBox2);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(2, 50);
		this.panel9.Margin = new System.Windows.Forms.Padding(2);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(972, 75);
		this.panel9.TabIndex = 0;
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(759, 48);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 8;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.rdmodifydate.Visible = false;
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(759, 9);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 6;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.drdocdate.Visible = false;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(759, 28);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 7;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.rdentrydate.Visible = false;
		this.groupBox2.Controls.Add(this.btnShowHistory);
		this.groupBox2.Controls.Add(this.label50);
		this.groupBox2.Controls.Add(this.label51);
		this.groupBox2.Controls.Add(this.label52);
		this.groupBox2.Controls.Add(this.label53);
		this.groupBox2.Controls.Add(this.label54);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbAdjustmentTypeHistory);
		this.groupBox2.Controls.Add(this.ToDocNoHistory);
		this.groupBox2.Controls.Add(this.FromDocNoHistory);
		this.groupBox2.Controls.Add(this.ToDateHistory);
		this.groupBox2.Controls.Add(this.FromDateHistory);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(4, 2);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(752, 71);
		this.groupBox2.TabIndex = 3;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Filters";
		this.btnShowHistory.BackColor = System.Drawing.Color.Teal;
		this.btnShowHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShowHistory.ForeColor = System.Drawing.Color.White;
		this.btnShowHistory.Location = new System.Drawing.Point(685, 40);
		this.btnShowHistory.Margin = new System.Windows.Forms.Padding(2);
		this.btnShowHistory.Name = "btnShowHistory";
		this.btnShowHistory.Size = new System.Drawing.Size(62, 26);
		this.btnShowHistory.TabIndex = 5;
		this.btnShowHistory.Text = "Show";
		this.btnShowHistory.UseVisualStyleBackColor = false;
		this.btnShowHistory.Click += new System.EventHandler(btnShowHistory_Click);
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label50.Location = new System.Drawing.Point(404, 20);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(97, 15);
		this.label50.TabIndex = 87;
		this.label50.Text = "Adjustment Type";
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label51.Location = new System.Drawing.Point(215, 46);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(64, 15);
		this.label51.TabIndex = 86;
		this.label51.Text = "To Doc No";
		this.label52.AutoSize = true;
		this.label52.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label52.Location = new System.Drawing.Point(215, 20);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(79, 15);
		this.label52.TabIndex = 85;
		this.label52.Text = "From Doc No";
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label53.Location = new System.Drawing.Point(3, 46);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(48, 15);
		this.label53.TabIndex = 84;
		this.label53.Text = "To Date";
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label54.Location = new System.Drawing.Point(3, 20);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(63, 15);
		this.label54.TabIndex = 83;
		this.label54.Text = "From Date";
		this.CmbAdjustmentTypeHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbAdjustmentTypeHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbAdjustmentTypeHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbAdjustmentTypeHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbAdjustmentTypeHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.CmbAdjustmentTypeHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbAdjustmentTypeHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance79).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance79).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance79).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbAdjustmentTypeHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance80).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAdjustmentTypeHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance80;
		((SpecialBoxBase)((UltraGridBase)this.CmbAdjustmentTypeHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance81).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance81).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAdjustmentTypeHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.CmbAdjustmentTypeHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbAdjustmentTypeHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbAdjustmentTypeHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbAdjustmentTypeHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.CmbAdjustmentTypeHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbAdjustmentTypeHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAdjustmentTypeHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance86).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbAdjustmentTypeHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.CmbAdjustmentTypeHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbAdjustmentTypeHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance87).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance87).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance87).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAdjustmentTypeHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance87;
		((AppearanceBase)appearance88).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbAdjustmentTypeHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.CmbAdjustmentTypeHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbAdjustmentTypeHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbAdjustmentTypeHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.CmbAdjustmentTypeHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbAdjustmentTypeHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.CmbAdjustmentTypeHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbAdjustmentTypeHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbAdjustmentTypeHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbAdjustmentTypeHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbAdjustmentTypeHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbAdjustmentTypeHistory).Location = new System.Drawing.Point(504, 14);
		((System.Windows.Forms.Control)(object)this.CmbAdjustmentTypeHistory).Name = "CmbAdjustmentTypeHistory";
		((System.Windows.Forms.Control)(object)this.CmbAdjustmentTypeHistory).Size = new System.Drawing.Size(244, 26);
		((System.Windows.Forms.Control)(object)this.CmbAdjustmentTypeHistory).TabIndex = 4;
		this.ToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.ToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDocNoHistory.Location = new System.Drawing.Point(298, 42);
		this.ToDocNoHistory.Name = "ToDocNoHistory";
		this.ToDocNoHistory.ShortcutsEnabled = false;
		this.ToDocNoHistory.Size = new System.Drawing.Size(103, 23);
		this.ToDocNoHistory.TabIndex = 3;
		this.ToDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(ToDocNoHistory_KeyPress);
		this.FromDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.FromDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDocNoHistory.Location = new System.Drawing.Point(298, 16);
		this.FromDocNoHistory.Name = "FromDocNoHistory";
		this.FromDocNoHistory.ShortcutsEnabled = false;
		this.FromDocNoHistory.Size = new System.Drawing.Size(103, 23);
		this.FromDocNoHistory.TabIndex = 2;
		this.FromDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(FromDocNoHistory_KeyPress);
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(69, 42);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(144, 23);
		this.ToDateHistory.TabIndex = 1;
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(69, 16);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(144, 23);
		this.FromDateHistory.TabIndex = 0;
		this.grdHistoryDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdHistoryDetail).BackColor = System.Drawing.Color.White;
		grdHistoryDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistoryDetail.DesignTimeLayout = grdHistoryDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Dock = System.Windows.Forms.DockStyle.Bottom;
		this.grdHistoryDetail.DynamicFiltering = true;
		this.grdHistoryDetail.FilterMode = (FilterMode)2;
		this.grdHistoryDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistoryDetail.GroupByBoxVisible = false;
		this.grdHistoryDetail.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Location = new System.Drawing.Point(2, 378);
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Name = "grdHistoryDetail";
		((ScrollableBase)this.grdHistoryDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Size = new System.Drawing.Size(972, 177);
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).TabIndex = 2;
		this.grdHistoryDetail.TotalRow = (InheritableBoolean)1;
		this.grdHistoryDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistoryDetail).VisualStyle = (VisualStyle)4;
		this.grdHistoryDetail.ColumnButtonClick += new ColumnActionEventHandler(grdhistory_ColumnButtonClick);
		this.grdHistoryDetail.LinkClicked += new ColumnActionEventHandler(grdhistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).DoubleClick += new System.EventHandler(grdhistory_DoubleClick);
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.label10);
		this.panel8.Controls.Add(this.toolStrip2);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(2, 2);
		this.panel8.Margin = new System.Windows.Forms.Padding(2);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(972, 48);
		this.panel8.TabIndex = 0;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.ForeColor = System.Drawing.Color.White;
		this.label10.Location = new System.Drawing.Point(5, 28);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(222, 19);
		this.label10.TabIndex = 36;
		this.label10.Text = "Stock Adjustment History ";
		this.toolStrip2.BackColor = System.Drawing.Color.White;
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnNewHistory, this.btnRefreshHistory });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(972, 27);
		this.toolStrip2.TabIndex = 1;
		this.toolStrip2.Text = "toolStrip2";
		this.btnNewHistory.Font = new System.Drawing.Font("Verdana", 10f);
		this.btnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewHistory.Name = "btnNewHistory";
		this.btnNewHistory.Size = new System.Drawing.Size(61, 24);
		this.btnNewHistory.Text = "&New";
		this.btnNewHistory.Click += new System.EventHandler(btnNewHistory_Click);
		this.btnRefreshHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshHistory.Name = "btnRefreshHistory";
		this.btnRefreshHistory.Size = new System.Drawing.Size(80, 24);
		this.btnRefreshHistory.Text = "Refresh";
		this.btnRefreshHistory.Click += new System.EventHandler(btnRefreshHistory_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(1079, 586);
		base.Controls.Add(this.panel1);
		base.HelpButton = true;
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "frmStockAdjustment";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmStockAdjustment_FormClosing);
		base.Load += new System.EventHandler(frmStockAdjustment_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(PurchsaeOrder_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbRateUom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYear).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackingType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAccountTitle).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWareHouse).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbEntryType).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdhistory).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbAdjustmentTypeHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryDetail).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
