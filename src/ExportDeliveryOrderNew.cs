using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Export;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Authentication;
using Architecture.Model.Export;
using Architecture.Model.Inventory;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
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
namespace Architecture.WinApp.Export
{

public class ExportDeliveryOrderNew : Form
{
	private Rightsobjects formrights = new Rightsobjects();

	private List<InvDeliveryOrderdetail> lstRemoveRecord = new List<InvDeliveryOrderdetail>();

	private Attachment AT = new Attachment();

	public DataTable dtViewAttachments = new DataTable();

	public DataTable dtAttachmentsUpload = new DataTable();

	private DataTable dtInvoice = new DataTable();

	private int updateDetailIndex;

	private double NetWeight;

	private DropDownBind DDL = new DropDownBind();

	public int Id;

	private int DefaultDaysToLessFromHistoryFromDate;

	private bool IsApproved;

	private bool FormInitialized;

	private bool ContainerNoCompulsoryOnDeliveryOrderExport;

	private DataTable table = new DataTable();

	private DataTable dtWh = new DataTable();

	private DataTable dtsuppcus = new DataTable();

	private DataTable dtExport = new DataTable();

	private DataTable dtCont = new DataTable();

	private DataTable dtContFromReadById = new DataTable();

	private DataTable BranchList = new DataTable();

	private DataTable dtHistory = new DataTable();

	private int TotalDoNetWeightOfDoBeingUpdated;

	private string AttachmentsValues = "";

	private string CustomAttachmentsValues = "";

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

	private TextBox txtdocno;

	private Label label5;

	private Panel panel6;

	private CtrlGrdBar ctrlGrdBar1;

	private CtrlGrdBar ctrlGrdBar2;

	internal GridEX grdhistory;

	private ToolStripButton btnRefresh;

	private ToolStrip toolStrip2;

	internal GridEX grd;

	private Label label4;

	internal UltraCombo cmbInvoiceNo;

	private GroupBox groupBox3;

	internal UltraCombo CmbJobLot;

	private Label label8;

	internal UltraCombo CmbWareHouse;

	private Label label3;

	internal UltraCombo CmbSupplierCustomer;

	private Label label2;

	private Button btnCancelUpdateDetial;

	private Button btnUpdateDetail;

	internal UltraCombo CmbPackingType;

	internal UltraCombo CmbPackUom;

	internal UltraCombo CmbOrderNo;

	internal UltraCombo CmbItemName;

	private Button btnplus;

	private TextBox txtremarksdetail;

	private Label label47;

	private TextBox txtweight;

	private TextBox txtqty;

	private Label label35;

	private Label label45;

	private Label label39;

	private Label label38;

	private Label label36;

	private Label label37;

	private TextBox txtGrossWeight;

	private Label label7;

	private TextBox txtTotalPackingWeight;

	private Label label6;

	private TextBox txtContainerRemarks;

	private Label label10;

	private TextBox txtInspectionRemarks;

	private Label label9;

	private TextBox txtBalanceWeight;

	private Label label15;

	private TextBox txtDoWeight;

	private Label label14;

	private TextBox txtInvoiceWeight;

	private Label label12;

	private DateTimePicker txtInvoiceDate;

	private Label label11;

	internal UltraCombo CmbCropYear;

	private Label label16;

	private TextBox txtrejectedWeight;

	private Label label21;

	private TextBox txtPackingWt;

	private Label label1;

	private GroupBox groupBox1;

	private GroupBox groupBox2;

	internal GridEX grdDetail;

	private Panel panel8;

	private Panel panel7;

	private Panel panel5;

	private Label label19;

	private Label label13;

	private TextBox txtVehicleNo;

	private Label label20;

	internal UltraCombo cmbContainerNoDetail;

	private Label label22;

	private ToolStripButton btnShortCutKeys;

	private GroupBox groupBox4;

	private Label label24;

	private TextBox txtOtherRemarks;

	private TextBox txtTotalGrossWeight;

	private Label label25;

	private Label label26;

	private Label label27;

	private TextBox txtPackingWeight;

	private TextBox txtOtherWeight;

	private Label label28;

	private TextBox txtItemWeight;

	internal UltraCombo CmbBranch;

	private Label lblBranchName;

	private Panel panel9;

	private Label label18;

	private Label label17;

	private GroupBox groupBox5;

	private RadioButton rdapproveddate;

	private Button btnShowHistory;

	private RadioButton rdmodifydate;

	private Label label34;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	private Label label73;

	private Label label89;

	private Label label90;

	private Label label91;

	internal UltraCombo CmbCustomerHistory;

	private TextBox ToDocNoHistory;

	private TextBox FromDocNoHistory;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	private ToolStripButton btnResetHistory;

	private ToolStripButton btnRefreshHistory;

	internal UltraCombo cmbtransporter;

	private Label label50;

	private TextBox txtremarks;

	private Label label29;

	private Label label53;

	internal UltraCombo CmbInvoiceNoHistory;

	public ExportDeliveryOrderNew(UserAccount user)
	{
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void PurchsaeOrder_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1250, 600);
			FormInitialized = true;
			formrights = CommonServices.SetRightsValueInRightsObject("ExportDeliveryOrder");
			btnsave.Enabled = formrights.DoHaveSaveRight;
			btnupdate.Enabled = formrights.DoHaveUpdateRights;
			dtViewAttachments.Columns.Add("AttchmentPath");
			dtCont.Columns.Add("ContainerId");
			dtCont.Columns.Add("ContainerNo");
			dtContFromReadById.Columns.Add("ContainerId");
			dtContFromReadById.Columns.Add("ContainerNo");
			table.Columns.Add("Id", typeof(int));
			table.Columns.Add("InvoiceDetailId", typeof(int));
			table.Columns.Add("InvoiceId", typeof(int));
			table.Columns.Add("InvoiceNo");
			table.Columns.Add("SupplierCustomerId", typeof(int));
			table.Columns.Add("SupplierCustomer");
			table.Columns.Add("OrderId", typeof(int));
			table.Columns.Add("OrderNo");
			table.Columns.Add("WareHouseId", typeof(int));
			table.Columns.Add("WareHouse");
			table.Columns.Add("JobLotId", typeof(int));
			table.Columns.Add("JobLot");
			table.Columns.Add("CropYearId", typeof(int));
			table.Columns.Add("CropYear");
			table.Columns.Add("ItemId", typeof(int));
			table.Columns.Add("Item");
			table.Columns.Add("ItemUOM");
			table.Columns.Add("ItemUOMId", typeof(int));
			table.Columns.Add("PackingTypeId", typeof(int));
			table.Columns.Add("PackingType");
			table.Columns.Add("QTY", typeof(double));
			table.Columns.Add("Weight", typeof(double));
			table.Columns.Add("LoadQty", typeof(double));
			table.Columns.Add("LoadWeight", typeof(double));
			table.Columns.Add("PackingWeight", typeof(double));
			table.Columns.Add("TotalPackingWeight", typeof(double));
			table.Columns.Add("GrossWeight", typeof(double));
			table.Columns.Add("OtherWeight", typeof(double));
			table.Columns.Add("ContainerId", typeof(int));
			table.Columns.Add("ContainerNo");
			table.Columns.Add("ContainerRemarks");
			table.Columns.Add("InspectionRemarks");
			table.Columns.Add("Remarks");
			grd.DataSource = table;
			if (dtWh.Columns.Count == 0)
			{
				dtWh.Columns.Add("Id");
				dtWh.Columns.Add("WareHouseName");
			}
			CmbInvoiceNoFill();
			CmbExportPackingTypeFill();
			DocumentNoFill();
			WareHouseFill();
			JobLotFill();
			DocDate.Focus();
			btnupdate.Visible = false;
			btnsave.Visible = true;
			btnplus.Visible = true;
			btnprint.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			BranchList = CommonServices.BrancheServiceBind();
			if (BranchList.Rows.Count > 1)
			{
				lblBranchName.Visible = true;
				((Control)(object)CmbBranch).Visible = true;
				((Control)(object)cmbtransporter).Size = new Size(272, 26);
			}
			else
			{
				lblBranchName.Visible = false;
				((Control)(object)CmbBranch).Visible = false;
				((Control)(object)cmbtransporter).Size = new Size(529, 26);
			}
			BranchFill();
			CmbBranch.Value = UserAccount.BranchesId;
			List<ConfigrationsAllocation> list2 = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "DefaultDaysToLessFromHistoryFromDate").ToList();
			if (list2.Count > 0)
			{
				DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)list2[0].ConfigKey.ToString());
			}
			List<ConfigrationsAllocation> list = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "ContainerNoCompulsoryOnDeliveryOrderExport").ToList();
			if (list.Count > 0)
			{
				ContainerNoCompulsoryOnDeliveryOrderExport = Conversion.ToBool((object)list[0].ConfigKey.ToString());
			}
			defaultConfiquration();
			HistoryCombosFill();
			FromDateHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (lblBranchName.Visible && ((UltraGridBase)CmbBranch).ActiveRow == null)
		{
			MessageBox.Show("Brnach field is required");
			CmbBranch.Focus();
			return false;
		}
		if (txtdocno.Text.Trim() == string.Empty || txtdocno.Text.Trim() == "0")
		{
			MessageBox.Show("DocNo Field is Required");
			txtdocno.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtItemWeight.Text) == 0.0)
		{
			MessageBox.Show("Item Weight Field is Required");
			txtItemWeight.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtPackingWeight.Text) == 0.0)
		{
			MessageBox.Show("Packing Weight Field is Required");
			txtPackingWeight.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtTotalGrossWeight.Text) == 0.0)
		{
			MessageBox.Show("Total Gross Weight Field is Required");
			txtTotalGrossWeight.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtOtherWeight.Text) > 0.0 && txtOtherRemarks.Text == string.Empty)
		{
			MessageBox.Show("Other Remarks Field is Required");
			txtOtherRemarks.Focus();
			return false;
		}
		return true;
	}

	private bool FormValidationDetail()
	{
		if (Conversion.ToInt(cmbInvoiceNo.Value) <= 0)
		{
			MessageBox.Show("InvoiceNo field is required");
			cmbInvoiceNo.Focus();
			return false;
		}
		if (Conversion.ToInt(CmbSupplierCustomer.Value) <= 0)
		{
			MessageBox.Show("Customer field is required");
			CmbSupplierCustomer.Focus();
			return false;
		}
		if (Conversion.ToInt(CmbOrderNo.Value) <= 0)
		{
			MessageBox.Show("OrderNo field is required");
			CmbOrderNo.Focus();
			return false;
		}
		if (Conversion.ToInt(CmbItemName.Value) <= 0)
		{
			MessageBox.Show("ItemName field is required");
			CmbItemName.Focus();
			return false;
		}
		if (Conversion.ToInt(CmbPackUom.Value) <= 0)
		{
			MessageBox.Show("PackUOM field is required");
			CmbPackUom.Focus();
			return false;
		}
		if (Conversion.ToInt(CmbPackingType.Value) <= 0)
		{
			MessageBox.Show("Packingtype field is required");
			CmbPackingType.Focus();
			return false;
		}
		if (Conversion.ToInt(CmbJobLot.Value) <= 0)
		{
			MessageBox.Show("JobLot field is required");
			CmbJobLot.Focus();
			return false;
		}
		if (Conversion.ToInt(CmbCropYear.Value) <= 0)
		{
			MessageBox.Show("CropYear field is required");
			CmbCropYear.Focus();
			return false;
		}
		if (Conversion.ToInt(CmbWareHouse.Value) <= 0)
		{
			MessageBox.Show("Warehouse field is required");
			CmbWareHouse.Focus();
			return false;
		}
		if (txtqty.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtqty.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Qty field is required");
			txtqty.Focus();
			return false;
		}
		if (txtweight.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtweight.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Weight field is required");
			txtweight.Focus();
			return false;
		}
		if (ContainerNoCompulsoryOnDeliveryOrderExport && (((UltraGridBase)cmbContainerNoDetail).ActiveRow == null || Conversion.ToInt(cmbContainerNoDetail.Value) == 0))
		{
			MessageBox.Show("ContainerNo field is required");
			cmbContainerNoDetail.Focus();
			return false;
		}
		return true;
	}

	private void btnplus_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValidationDetail())
			{
				int InvoiceDetailId = 0;
				if (Conversion.ToInt(CmbItemName.Value) > 0)
				{
					InvoiceDetailId = Conversion.ToInt(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["InvoiceDetailId"].Value);
				}
				int ContainerId = 0;
				string ContainerNo = "";
				if (((UltraGridBase)cmbContainerNoDetail).ActiveRow != null && Conversion.ToInt(cmbContainerNoDetail.Value) != 0)
				{
					ContainerId = Conversion.ToInt(cmbContainerNoDetail.Value);
					ContainerNo = Conversion.ToString((object)((Control)(object)cmbContainerNoDetail).Text);
				}
				table.Rows.Add(0, InvoiceDetailId, cmbInvoiceNo.Value, ((Control)(object)cmbInvoiceNo).Text.Trim(), CmbSupplierCustomer.Value, ((Control)(object)CmbSupplierCustomer).Text.Trim(), CmbOrderNo.Value, ((Control)(object)CmbOrderNo).Text.Trim(), CmbWareHouse.Value, ((Control)(object)CmbWareHouse).Text.Trim(), CmbJobLot.Value, ((Control)(object)CmbJobLot).Text.Trim(), CmbCropYear.Value, ((Control)(object)CmbCropYear).Text.Trim(), ((UltraDropDownBase)CmbItemName).SelectedRow.Cells["ItemId"].Value, ((Control)(object)CmbItemName).Text.Trim(), ((Control)(object)CmbPackUom).Text.Trim(), CmbPackUom.Value, CmbPackingType.Value, ((Control)(object)CmbPackingType).Text.Trim(), Conversion.ToDouble((object)txtqty.Text.Trim()), Conversion.ToDouble((object)txtweight.Text.Trim()), Conversion.ToDouble((object)txtqty.Text.Trim()), Conversion.ToDouble((object)txtweight.Text.Trim()), Conversion.ToDouble((object)txtPackingWt.Text), Conversion.ToDouble((object)txtTotalPackingWeight.Text), Conversion.ToDouble((object)txtGrossWeight.Text), 0, ContainerId, ContainerNo, txtContainerRemarks.Text.Trim(), txtInspectionRemarks.Text.Trim(), txtremarksdetail.Text.Trim());
				grd.DataSource = table;
				grd.RetrieveStructure();
				grdSettings();
				ResetDetail();
				CalculateTotalInformation();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateTotalInformation()
	{
		try
		{
			if (grd.GetRows().Count() > 0)
			{
				decimal ItemWeight = Conversion.ToDecimal(grd.GetTotal(grd.RootTable.Columns["LoadWeight"], (AggregateFunction)2));
				decimal PackingWeight = Conversion.ToDecimal(grd.GetTotal(grd.RootTable.Columns["TotalPackingWeight"], (AggregateFunction)2));
				decimal GrossWeight = Conversion.ToDecimal(grd.GetTotal(grd.RootTable.Columns["GrossWeight"], (AggregateFunction)2));
				if (Conversion.ToDouble((object)txtOtherWeight.Text) > 0.0)
				{
					GrossWeight += Conversion.ToDecimal((object)txtOtherWeight.Text);
				}
				txtItemWeight.Text = Math.Round(ItemWeight, 2).ToString("#,##0.##");
				txtPackingWeight.Text = Math.Round(PackingWeight, 2).ToString("#,##0.##");
				txtTotalGrossWeight.Text = Math.Round(GrossWeight, 2).ToString("#,##0.##");
			}
			else
			{
				txtItemWeight.Text = "0";
				txtPackingWeight.Text = "0";
				txtTotalGrossWeight.Text = "0";
			}
			OtherWeightProportion();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grdSettings()
	{
		try
		{
			grd.ColumnAutoResize = false;
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["InvoiceId"].Visible = false;
			grd.RootTable.Columns["InvoiceDetailId"].Visible = false;
			grd.RootTable.Columns["ItemId"].Visible = false;
			grd.RootTable.Columns["WareHouseId"].Visible = false;
			grd.RootTable.Columns["SupplierCustomerId"].Visible = false;
			grd.RootTable.Columns["OrderId"].Visible = false;
			grd.RootTable.Columns["ItemUOMId"].Visible = false;
			grd.RootTable.Columns["PackingTypeId"].Visible = false;
			grd.RootTable.Columns["JobLotId"].Visible = false;
			grd.RootTable.Columns["QTY"].Visible = false;
			grd.RootTable.Columns["Weight"].Visible = false;
			grd.RootTable.Columns["CropYearId"].Visible = false;
			grd.RootTable.Columns["ContainerId"].Visible = false;
			grd.RootTable.Columns["SupplierCustomer"].Caption = "Customer";
			CommonServices.GridWrappingAndColumnSettings(grd, 2);
			grd.RootTable.Columns.Add("Delete");
			grd.RootTable.Columns["Delete"].Key = "Delete";
			grd.RootTable.Columns["Delete"].Caption = "X";
			grd.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Delete"].ButtonText = "X";
			grd.RootTable.Columns["Delete"].Width = 20;
			grd.RootTable.Columns["Delete"].Position = 0;
			grd.FrozenColumns = 1;
			grd.RootTable.Columns.Add("Edit");
			grd.RootTable.Columns["Edit"].Key = "Edit";
			grd.RootTable.Columns["Edit"].Caption = "Edit";
			grd.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Edit"].ButtonText = "Edit";
			grd.RootTable.Columns["Edit"].Width = 50;
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
		try
		{
			GridEXRow item = grd.CurrentRow;
			if (item != null)
			{
				updateDetailIndex = item.RowIndex;
				cmbInvoiceNo.Value = Conversion.ToInt(item.Cells["InvoiceId"].Value);
				cmbInvoiceNo_Leave(null, null);
				CmbSupplierCustomer.Value = Conversion.ToInt(item.Cells["SupplierCustomerId"].Value);
				CmbOrderNo.Value = Conversion.ToInt(item.Cells["OrderId"].Value);
				CmbItemName.Value = Conversion.ToInt(item.Cells["InvoiceDetailId"].Value);
				CmbItemName_TextChanged(null, null);
				CmbPackUom.Value = item.Cells["ItemUOMId"].Value.ToString();
				CmbPackingType.Value = item.Cells["PackingTypeId"].Value.ToString();
				((Control)(object)CmbPackingType).Text = item.Cells["PackingType"].Text.ToString();
				CmbCropYear.Value = item.Cells["CropYearId"].Value.ToString();
				txtqty.Text = item.Cells["LoadQty"].Value.ToString();
				txtweight.Text = item.Cells["LoadWeight"].Value.ToString();
				txtqty.Text = item.Cells["QTY"].Value.ToString();
				txtweight.Text = item.Cells["Weight"].Value.ToString();
				txtPackingWt.Text = item.Cells["PackingWeight"].Value.ToString();
				txtTotalPackingWeight.Text = item.Cells["TotalPackingWeight"].Value.ToString();
				txtGrossWeight.Text = item.Cells["GrossWeight"].Value.ToString();
				CmbWareHouse.Value = item.Cells["WareHouseId"].Value.ToString();
				((Control)(object)CmbWareHouse).Text = item.Cells["WareHouse"].Text.ToString();
				CmbJobLot.Value = item.Cells["JobLotId"].Value.ToString();
				((Control)(object)CmbJobLot).Text = item.Cells["JobLot"].Text.ToString();
				if (Conversion.ToInt(item.Cells["ContainerId"].Value) > 0)
				{
					cmbContainerNoDetail.Value = item.Cells["ContainerId"].Value.ToString();
				}
				txtInspectionRemarks.Text = item.Cells["InspectionRemarks"].Value.ToString();
				txtremarksdetail.Text = item.Cells["Remarks"].Value.ToString();
				btnplus.Visible = false;
				btnUpdateDetail.Visible = true;
				btnCancelUpdateDetial.Visible = true;
				CmbWareHouse.Focus();
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
				int ContainerId = 0;
				string ContainerNo = "";
				if (((UltraGridBase)cmbContainerNoDetail).ActiveRow != null && Conversion.ToInt(cmbContainerNoDetail.Value) != 0)
				{
					ContainerId = Conversion.ToInt(cmbContainerNoDetail.Value);
					ContainerNo = Conversion.ToString((object)((Control)(object)cmbContainerNoDetail).Text);
				}
				table.Rows[updateDetailIndex]["InvoiceId"] = cmbInvoiceNo.Value;
				table.Rows[updateDetailIndex]["InvoiceNo"] = ((Control)(object)cmbInvoiceNo).Text;
				table.Rows[updateDetailIndex]["SupplierCustomerId"] = CmbSupplierCustomer.Value;
				table.Rows[updateDetailIndex]["SupplierCustomer"] = ((Control)(object)CmbSupplierCustomer).Text;
				table.Rows[updateDetailIndex]["InvoiceDetailId"] = ((UltraDropDownBase)CmbItemName).SelectedRow.Cells["InvoiceDetailId"].Value;
				table.Rows[updateDetailIndex]["OrderId"] = CmbOrderNo.Value;
				table.Rows[updateDetailIndex]["OrderNo"] = ((Control)(object)CmbOrderNo).Text;
				table.Rows[updateDetailIndex]["WareHouseId"] = CmbWareHouse.Value;
				table.Rows[updateDetailIndex]["WareHouse"] = ((Control)(object)CmbWareHouse).Text;
				table.Rows[updateDetailIndex]["JobLotId"] = CmbJobLot.Value;
				table.Rows[updateDetailIndex]["JobLot"] = ((Control)(object)CmbJobLot).Text.Trim();
				table.Rows[updateDetailIndex]["CropYearId"] = CmbCropYear.Value;
				table.Rows[updateDetailIndex]["CropYear"] = ((Control)(object)CmbCropYear).Text.Trim();
				table.Rows[updateDetailIndex]["ItemId"] = ((UltraDropDownBase)CmbItemName).SelectedRow.Cells["ItemId"].Value;
				table.Rows[updateDetailIndex]["Item"] = ((Control)(object)CmbItemName).Text;
				table.Rows[updateDetailIndex]["ItemUOMId"] = CmbPackUom.Value;
				table.Rows[updateDetailIndex]["ItemUOM"] = ((Control)(object)CmbPackUom).Text;
				table.Rows[updateDetailIndex]["PackingTypeId"] = CmbPackingType.Value;
				table.Rows[updateDetailIndex]["PackingType"] = ((Control)(object)CmbPackingType).Text.Trim();
				table.Rows[updateDetailIndex]["QTY"] = txtqty.Text;
				table.Rows[updateDetailIndex]["LoadQTY"] = txtqty.Text;
				table.Rows[updateDetailIndex]["Weight"] = txtweight.Text;
				table.Rows[updateDetailIndex]["LoadWeight"] = txtweight.Text;
				table.Rows[updateDetailIndex]["PackingWeight"] = txtPackingWt.Text;
				table.Rows[updateDetailIndex]["TotalPackingWeight"] = txtTotalPackingWeight.Text;
				table.Rows[updateDetailIndex]["GrossWeight"] = txtGrossWeight.Text;
				table.Rows[updateDetailIndex]["ContainerId"] = ContainerId;
				table.Rows[updateDetailIndex]["ContainerNo"] = ContainerNo;
				table.Rows[updateDetailIndex]["ContainerRemarks"] = txtContainerRemarks.Text;
				table.Rows[updateDetailIndex]["InspectionRemarks"] = txtInspectionRemarks.Text;
				table.Rows[updateDetailIndex]["Remarks"] = txtremarksdetail.Text;
				grd.DataSource = table;
				btnplus.Visible = true;
				btnUpdateDetail.Visible = false;
				btnCancelUpdateDetial.Visible = false;
				ResetDetail();
				CmbInvoiceNoFill();
				CalculateTotalInformation();
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
		CmbInvoiceNoFill();
	}

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0322: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		//IL_0303: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow r = grd.CurrentRow;
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					InvDeliveryOrderdetail vd = new InvDeliveryOrderdetail();
					vd.RefDocumentTypeId = 0;
					vd.RefDocIdNo = 0;
					vd.RefDocSubIdNo = 0;
					vd.Id = Conversion.ToInt(r.Cells["Id"].Value);
					vd.ExImInvoiceId = Conversion.ToInt(r.Cells["InvoiceId"].Value);
					vd.SupplierCustomerId = Conversion.ToInt(r.Cells["SupplierCustomerId"].Value);
					vd.SaleOrderId = Conversion.ToInt(r.Cells["OrderId"].Value);
					vd.WarehouseId = Conversion.ToInt(r.Cells["WareHouseId"].Value);
					vd.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
					vd.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					vd.CropYearId = Conversion.ToInt(r.Cells["CropYearId"].Value);
					vd.PackUomId = Conversion.ToInt(r.Cells["ItemUOMId"].Value);
					vd.InvPackingTypeId = Conversion.ToInt(r.Cells["PackingTypeId"].Value);
					vd.DoQty = Conversion.ToDouble(r.Cells["LoadQty"].Value);
					vd.DoWeight = Conversion.ToDouble(r.Cells["LoadWeight"].Value);
					vd.LoadingQty = vd.DoQty;
					vd.LoadingWeight = vd.DoWeight;
					vd.PackingWeight = Conversion.ToDouble(r.Cells["PackingWeight"].Value);
					vd.TotalPackingWeight = Conversion.ToDouble(r.Cells["TotalPackingWeight"].Value);
					vd.GrossWeight = Conversion.ToDouble(r.Cells["GrossWeight"].Value);
					vd.LoadingRemarks = Conversion.ToString(r.Cells["Remarks"].Value);
					vd.InspectionRemarks = Conversion.ToString(r.Cells["InspectionRemarks"].Value);
					vd.ContainerRemarks = Conversion.ToString(r.Cells["ContainerRemarks"].Value);
					vd.ActionTypeId = 3;
					lstRemoveRecord.Add(vd);
					table.Rows[r.RowIndex].Delete();
					if (r != null && (int)r.RowType == 0)
					{
						r.Delete();
					}
					grd.UpdateData();
				}
				else if (r != null && (int)r.RowType == 0)
				{
					r.Delete();
				}
				CalculateTotalInformation();
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

	private void OtherWeightProportion()
	{
		try
		{
			double NetWeight = ((grd.RowCount > 0) ? Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["Weight"], (AggregateFunction)2).ToString()) : 0.0);
			double OtherWeightHeader = Conversion.ToDouble((object)txtOtherWeight.Text);
			if (OtherWeightHeader > 0.0)
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					double Weight = Conversion.ToDouble((object)obj.Cells["Weight"].Value.ToString());
					obj.Cells["OtherWeight"].Value = OtherWeightHeader / NetWeight * Weight;
					obj.EndEdit();
				}
			}
			else
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj2 in rows)
				{
					obj2.BeginEdit();
					obj2.Cells["OtherWeight"].Value = 0;
					obj2.EndEdit();
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
		//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bf: Expected O, but got Unknown
		try
		{
			if (!FormValidation())
			{
				return;
			}
			if (grd.GetRows().Count() == 0)
			{
				MessageBox.Show("Grid Record Not Found");
				return;
			}
			InvDeliveryOrder po = new InvDeliveryOrder();
			if (Id > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				po.Id = Id;
				po.ModifyUser = UserAccount.ID;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			OtherWeightProportion();
			po.DocumentTypeId = 84;
			po.DocDate = DocDate.Value;
			po.DocNo = Conversion.ToInt((object)txtdocno.Text);
			po.LoadingInstructions = Conversion.ToString((object)txtremarks.Text);
			po.EximInvoiceId = Conversion.ToInt(cmbInvoiceNo.Value);
			po.OrganizationId = UserAccount.OrganizationId;
			po.CompanyId = UserAccount.CompanyId;
			po.EntryUser = UserAccount.ID;
			po.ScreenName = Conversion.ToString((object)base.Name);
			if (BranchList.Rows.Count > 1)
			{
				po.BranchesId = Conversion.ToInt(CmbBranch.Value);
			}
			else
			{
				po.BranchesId = UserAccount.BranchesId;
			}
			po.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			po.DeliveryOrderType = "Export";
			po.VehicleNo = Conversion.ToString((object)txtVehicleNo.Text.Trim());
			po.TransporterId = Conversion.ToInt(cmbtransporter.Value);
			po.NetWeight = Conversion.ToDouble((object)txtItemWeight.Text);
			po.PackingWeight = Conversion.ToDouble((object)txtPackingWeight.Text);
			po.OtherWeight = Conversion.ToDouble((object)txtOtherWeight.Text);
			po.GrossWeight = Conversion.ToDouble((object)txtTotalGrossWeight.Text);
			po.OtherWeightRemarks = Conversion.ToString((object)txtOtherRemarks.Text);
			po.InvDeliveryOrderDetailList = new List<InvDeliveryOrderdetail>();
			po.InvDeliveryOrderExpenseslist = new List<InvDeliveryOrderExpense>();
			double TotalQty = 0.0;
			double TotalNetWeight = 0.0;
			double TotalGrossWeight = 0.0;
			if (lstRemoveRecord.Count > 0)
			{
				for (int i = 0; i < lstRemoveRecord.Count; i++)
				{
					po.InvDeliveryOrderDetailList.Add(lstRemoveRecord[i]);
				}
			}
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow r in rows)
			{
				InvDeliveryOrderdetail vd = new InvDeliveryOrderdetail();
				vd.RefDocumentTypeId = 0;
				vd.RefDocIdNo = 0;
				vd.RefDocSubIdNo = 0;
				if (Id == 0)
				{
					vd.ActionTypeId = 1;
				}
				else
				{
					vd.Id = Conversion.ToInt(r.Cells["Id"].Value);
					if (vd.Id > 0)
					{
						vd.ActionTypeId = 2;
					}
					else
					{
						vd.ActionTypeId = 1;
					}
				}
				vd.ExImInvoiceId = Conversion.ToInt(r.Cells["InvoiceId"].Value);
				vd.InvoiceDetailId = Conversion.ToInt(r.Cells["InvoiceDetailId"].Value);
				vd.SupplierCustomerId = Conversion.ToInt(r.Cells["SupplierCustomerId"].Value);
				vd.SaleOrderId = Conversion.ToInt(r.Cells["OrderId"].Value);
				vd.WarehouseId = Conversion.ToInt(r.Cells["WareHouseId"].Value);
				vd.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
				vd.CropYearId = Conversion.ToInt(r.Cells["CropYearId"].Value);
				vd.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
				vd.PackUomId = Conversion.ToInt(r.Cells["ItemUOMId"].Value);
				vd.InvPackingTypeId = Conversion.ToInt(r.Cells["PackingTypeId"].Value);
				vd.InnerQty = vd.DoQty;
				vd.InnerUomId = vd.PackUomId;
				vd.InnerEbUnit = Conversion.ToDouble(r.Cells["PackingWeight"].Value);
				vd.InnerEbTotal = Conversion.ToDouble(r.Cells["TotalPackingWeight"].Value);
				vd.OuterEbTotal = Conversion.ToDouble(r.Cells["TotalPackingWeight"].Value);
				vd.DoQty = Conversion.ToDouble(r.Cells["LoadQty"].Value);
				vd.DoWeight = Conversion.ToDouble(r.Cells["LoadWeight"].Value);
				vd.LoadingQty = vd.DoQty;
				vd.LoadingWeight = vd.DoWeight;
				vd.PackingWeight = Conversion.ToDouble(r.Cells["PackingWeight"].Value);
				vd.TotalPackingWeight = Conversion.ToDouble(r.Cells["TotalPackingWeight"].Value);
				vd.GrossWeight = Conversion.ToDouble(r.Cells["GrossWeight"].Value);
				vd.OtherWeight = Conversion.ToDouble(r.Cells["OtherWeight"].Value);
				TotalQty += vd.DoQty;
				TotalNetWeight += vd.LoadingWeight;
				TotalGrossWeight += vd.GrossWeight;
				if (ContainerNoCompulsoryOnDeliveryOrderExport && Conversion.ToInt(r.Cells["ContainerId"].Value) == 0)
				{
					throw new Exception("Container No Required in Row No" + (r.RowIndex + 1));
				}
				vd.ContainerId = Conversion.ToInt(r.Cells["ContainerId"].Value);
				vd.ContainerRemarks = Conversion.ToString(r.Cells["ContainerRemarks"].Value);
				vd.InspectionRemarks = Conversion.ToString(r.Cells["InspectionRemarks"].Value);
				vd.LoadingRemarks = Conversion.ToString(r.Cells["Remarks"].Value);
				po.InvDeliveryOrderDetailList.Add(vd);
			}
			po.DoTotalQty = Conversion.ToDecimal((object)TotalQty);
			po.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			po.DeleteAttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<InvDeliveryOrder>(po, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
			po.AttachmentsValues = AttachmentsValues;
			po.CustomAttachmentsValues = CustomAttachmentsValues;
			InvDeliveryOrder.Save(po);
			if (Id > 0)
			{
				MessageBox.Show("Data Update Successfully....  " + po.DocNo);
				FormHelper.DeleteAllAttachmentsFromDb(AT, Id, po.DocumentTypeId, base.Name);
			}
			else
			{
				MessageBox.Show("Data Save Successfully....  " + po.DocNo);
			}
			Reset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		try
		{
			Id = 0;
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
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void ReadById(int ID)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		try
		{
			Reset();
			Id = ID;
			InvDeliveryOrder po = new InvDeliveryOrder();
			po = InvDeliveryOrder.GetByID(ID);
			po.Id = Id;
			tabControl1.SelectedIndex = 0;
			DocDate.Value = po.DocDate;
			txtdocno.Text = Conversion.ToString((object)po.DocNo);
			txtremarks.Text = Conversion.ToString((object)po.LoadingInstructions);
			((Control)(object)cmbInvoiceNo).Text = Conversion.ToString((object)po.EximInvoiceId);
			txtVehicleNo.Text = Conversion.ToString((object)po.VehicleNo);
			CmbBranch.Value = po.BranchesId;
			txtItemWeight.Text = po.NetWeight.ToString("#,##0.##");
			txtPackingWeight.Text = po.PackingWeight.ToString("#,##0.##");
			txtOtherWeight.Text = po.OtherWeight.ToString("#,##0.##");
			txtTotalGrossWeight.Text = po.GrossWeight.ToString("#,##0.##");
			txtOtherRemarks.Text = po.OtherWeightRemarks;
			if (po.TransporterId > 0)
			{
				cmbtransporter.Value = po.TransporterId;
			}
			AttachmentsValues = po.AttachmentsValues;
			CustomAttachmentsValues = po.CustomAttachmentsValues;
			IsApproved = po.IsApproved;
			table.Clear();
			for (int i = 0; i < po.InvDeliveryOrderDetailList.Count; i++)
			{
				if (Conversion.ToDouble((object)po.InvDeliveryOrderDetailList[i].TotalPackingWeight) == 0.0)
				{
					po.InvDeliveryOrderDetailList[i].TotalPackingWeight = po.InvDeliveryOrderDetailList[i].PackingWeight;
					if (Conversion.ToDouble((object)po.InvDeliveryOrderDetailList[i].TotalPackingWeight) > 0.0)
					{
						po.InvDeliveryOrderDetailList[i].PackingWeight = Conversion.ToDouble((object)po.InvDeliveryOrderDetailList[i].TotalPackingWeight) / Conversion.ToDouble((object)po.InvDeliveryOrderDetailList[i].LoadingQty);
					}
				}
				table.Rows.Add(po.InvDeliveryOrderDetailList[i].Id, po.InvDeliveryOrderDetailList[i].InvoiceDetailId, po.InvDeliveryOrderDetailList[i].ExImInvoiceId, po.InvDeliveryOrderDetailList[i].InvoiceNo, po.InvDeliveryOrderDetailList[i].SupplierCustomerId, po.InvDeliveryOrderDetailList[i].SupplierCustomer, po.InvDeliveryOrderDetailList[i].SaleOrderId, po.InvDeliveryOrderDetailList[i].OrderNo, po.InvDeliveryOrderDetailList[i].WarehouseId, po.InvDeliveryOrderDetailList[i].WareHouseName, po.InvDeliveryOrderDetailList[i].JobLotId, po.InvDeliveryOrderDetailList[i].JobLotDescription, po.InvDeliveryOrderDetailList[i].CropYearId, po.InvDeliveryOrderDetailList[i].CropYear, po.InvDeliveryOrderDetailList[i].ItemId, po.InvDeliveryOrderDetailList[i].ItemName, po.InvDeliveryOrderDetailList[i].PackUOM, po.InvDeliveryOrderDetailList[i].PackUomId, po.InvDeliveryOrderDetailList[i].InvPackingTypeId, po.InvDeliveryOrderDetailList[i].PackTypeDesc, po.InvDeliveryOrderDetailList[i].DoQty, po.InvDeliveryOrderDetailList[i].DoWeight, po.InvDeliveryOrderDetailList[i].LoadingQty, po.InvDeliveryOrderDetailList[i].LoadingWeight, po.InvDeliveryOrderDetailList[i].PackingWeight, po.InvDeliveryOrderDetailList[i].TotalPackingWeight, po.InvDeliveryOrderDetailList[i].GrossWeight, po.InvDeliveryOrderDetailList[i].OtherWeight, po.InvDeliveryOrderDetailList[i].ContainerId, po.InvDeliveryOrderDetailList[i].Container, po.InvDeliveryOrderDetailList[i].ContainerRemarks, po.InvDeliveryOrderDetailList[i].InspectionRemarks, po.InvDeliveryOrderDetailList[i].LoadingRemarks);
				dtContFromReadById.Rows.Add(po.InvDeliveryOrderDetailList[i].ContainerId, po.InvDeliveryOrderDetailList[i].Container);
			}
			grd.DataSource = table;
			grd.RetrieveStructure();
			grdSettings();
			CalculateTotalInformation();
			FormHelper.LoadAttachmentsForObject(AT, Id, base.Name);
			btnsave.Visible = false;
			btnupdate.Visible = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BranchFill()
	{
		try
		{
			if (BranchList.Rows.Count > 0)
			{
				DDL.BindDDLNew(BranchList, CmbBranch, "Id", "BranchName", "Branch Name", false);
				((UltraGridBase)CmbBranch).Rows[0].Activate();
			}
			if (BranchList.Rows.Count > 1)
			{
				lblBranchName.Visible = true;
				((Control)(object)CmbBranch).Visible = true;
			}
			else
			{
				lblBranchName.Visible = false;
				((Control)(object)CmbBranch).Visible = false;
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
			dtWh = CommonServices.getActiveWareHouse();
			if (dtWh.Rows.Count > 0)
			{
				DDL.BindDDL(dtWh, CmbWareHouse, "Id", "WareHouseName", "WareHouseName", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void JobLotFill()
	{
		try
		{
			DataTable dtjob = CommonServices.JobLotGetAllService();
			if (dtjob.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtjob, CmbJobLot, "Id", "JobLotDescription", "JobLot", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DocumentNoFill()
	{
		try
		{
			int code = CommonServices.DeliveryOrderGenerateCode(84);
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

	private void BindPackUomAndPackingType(int ID)
	{
		try
		{
			DataTable dt = ExImInvoice.GetInvoiceDetailForDetailId(ID);
			if (dt != null && dt.Rows.Count > 0)
			{
				txtTotalPackingWeight.Text = Conversion.ToString(dt.Rows[0]["TotalPackingWeight"]);
				txtPackingWt.Text = Conversion.ToString(dt.Rows[0]["PackingWeight"]);
				if (Conversion.ToInt(dt.Rows[0]["JobLotId"]) > 0)
				{
					CmbJobLot.Value = Conversion.ToInt(dt.Rows[0]["JobLotId"]);
				}
				DataTable dtUom = new DataTable();
				dtUom.Columns.Add("PackUomId", typeof(int));
				dtUom.Columns.Add("PackUom", typeof(string));
				dtUom.Columns.Add("Equivalent", typeof(double));
				DataTable dtpt = new DataTable();
				dtpt.Columns.Add("PackingMaterialTypeId", typeof(int));
				dtpt.Columns.Add("PackingType", typeof(string));
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtUom.Rows.Add(dt.Rows[i]["PackUomId"], dt.Rows[i]["PackUom"], dt.Rows[i]["Equivalent"]);
					dtpt.Rows.Add(dt.Rows[i]["PackingMaterialTypeId"], dt.Rows[i]["PackingType"]);
				}
				if (dtUom.Rows.Count > 0)
				{
					DDL.BindDDL(dtUom, CmbPackUom, "PackUomId", "PackUom", "PackUom", ZeroIndex: false);
					((UltraGridBase)CmbPackUom).DisplayLayout.Bands[0].Columns[2].Hidden = true;
					((UltraGridBase)CmbPackUom).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
					((UltraGridBase)CmbPackUom).Rows[0].Activate();
				}
			}
			else
			{
				((UltraGridBase)CmbPackUom).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbExportPackingTypeFill()
	{
		try
		{
			DataTable dt = InvPackingType.Getall();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbPackingType, "Id", "PackTypeDesc", "Packing Type", false);
				((UltraGridBase)CmbPackingType).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbInvoiceNoFill()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		try
		{
			DataTable dt = new DataTable();
			ExImInvoice obj = new ExImInvoice();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.Id = Id;
			dtInvoice = ExImInvoice.getOutstandingExportInvoicesForDeliveryOrder(obj);
			dt.Columns.Add("Id");
			dt.Columns.Add("InvoiceNo");
			for (int i = 0; i < dtInvoice.Rows.Count; i++)
			{
				dt.Rows.Add(dtInvoice.Rows[i]["Id"], dtInvoice.Rows[i]["InvoiceNo"]);
			}
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbInvoiceNo, "Id", "InvoiceNo", "Invoice No", false);
			}
			else
			{
				((UltraGridBase)cmbInvoiceNo).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void defaultConfiquration()
	{
		try
		{
			List<ConfigrationsAllocation> lstJob = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Job/Lot").ToList();
			if (lstJob.Count > 0)
			{
				int JobId = Conversion.ToInt((object)lstJob[0].ConfigKey.ToString());
				CmbJobLot.Value = JobId;
			}
			List<ConfigrationsAllocation> lstWareHouse = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Warehouse").ToList();
			if (lstWareHouse.Count > 0)
			{
				int WareHouseId = Conversion.ToInt((object)lstWareHouse[0].ConfigKey.ToString());
				CmbWareHouse.Value = WareHouseId;
			}
		}
		catch (Exception)
		{
			MessageBox.Show("Confiquration Error");
		}
	}

	private void GetDatabyInvoiceId()
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		try
		{
			((Control)(object)CmbSupplierCustomer).Text = string.Empty;
			((UltraGridBase)CmbSupplierCustomer).DataSource = null;
			((Control)(object)CmbOrderNo).Text = string.Empty;
			((UltraGridBase)CmbOrderNo).DataSource = null;
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.Id = Conversion.ToInt(cmbInvoiceNo.Value);
			DataTable dt = ExImInvoice.GetContractsByInvoiceId(obj);
			if (dt.Rows.Count > 0)
			{
				DataTable dtinvoice = new DataTable();
				dtinvoice.Columns.Add("OrderId");
				dtinvoice.Columns.Add("OrderNo");
				dtinvoice.Rows.Add(dt.Rows[0]["ContractId"], dt.Rows[0]["ContractNo"]);
				DDL.BindDDL(dtinvoice, CmbOrderNo, "OrderId", "OrderNo", "Sale ContractNo", ZeroIndex: false);
				((UltraGridBase)CmbOrderNo).Rows[0].Activate();
			}
			DataTable dtInvHeaderData = ExImInvoice.GetExportInvoiceHeaderDataByInvoiceId(obj);
			if (dtInvHeaderData.Rows.Count > 0)
			{
				txtInvoiceDate.Value = Conversion.ToDateTime(dtInvHeaderData.Rows[0]["DocDate"]);
				DDL.BindDDLNew(dtInvHeaderData, CmbSupplierCustomer, "Id", "CustomerName", "Customer Name", false);
				((UltraGridBase)CmbSupplierCustomer).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetInvoicewiseDoWeightandBalanceWeight()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		try
		{
			DataTable dt = ExImInvoice.GetInvoicewiseDoWeightandBalanceWeight(new ExImInvoice
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Id = Conversion.ToInt(cmbInvoiceNo.Value)
			});
			if (dt.Rows.Count > 0)
			{
				txtInvoiceWeight.Text = Conversion.ToString(dt.Rows[0]["InvoiceWeight"]);
				txtDoWeight.Text = Conversion.ToString(dt.Rows[0]["DoWeight"]);
				txtrejectedWeight.Text = Conversion.ToString(dt.Rows[0]["GpRejectedWeight"]);
				txtBalanceWeight.Text = Conversion.ToString(dt.Rows[0]["WeightAvailableForDo"]);
			}
			else
			{
				txtInvoiceWeight.Text = "0";
				txtDoWeight.Text = "0";
				txtrejectedWeight.Text = "0";
				txtBalanceWeight.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbItemName_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(CmbItemName.Value) > 0)
			{
				BindPackUomAndPackingType(Conversion.ToInt(((UltraDropDownBase)CmbItemName).SelectedRow.Cells["InvoiceDetailId"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbInvoiceNo_Leave(object sender, EventArgs e)
	{
		try
		{
			GetDatabyInvoiceId();
			GetInvoicewiseDoWeightandBalanceWeight();
			if (Conversion.ToInt(cmbInvoiceNo.Value) != 0)
			{
				GetContainerNoFromCROByInvoiceId(Conversion.ToInt(cmbInvoiceNo.Value));
			}
			else
			{
				((UltraGridBase)cmbContainerNoDetail).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbOrderNo_TextChanged(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.Id = Conversion.ToInt(cmbInvoiceNo.Value);
			obj.OrderId = Conversion.ToInt(CmbOrderNo.Value);
			if (obj.Id > 0 && obj.OrderId > 0)
			{
				DataTable dtItem = ExImInvoice.ExportInvoiceDetailByHeaderId(obj);
				if (dtItem.Rows.Count > 0)
				{
					DataTable dt = new DataTable();
					dt.Columns.Add("ItemId");
					dt.Columns.Add("ItemName");
					dt.Columns.Add("InvoiceDetailId");
					dt.Columns.Add("PackUomId");
					dt.Columns.Add("PackUom");
					DataTable dtCrop = new DataTable();
					dtCrop.Columns.Add("CropYearId");
					dtCrop.Columns.Add("CropYear");
					for (int i = 0; i < dtItem.Rows.Count; i++)
					{
						dt.Rows.Add(dtItem.Rows[i]["ItemId"], dtItem.Rows[i]["ItemName"], dtItem.Rows[i]["InvoiceDetailId"], dtItem.Rows[i]["PackUomId"], dtItem.Rows[i]["PackUom"]);
						dtCrop.Rows.Add(dtItem.Rows[i]["CropYearId"], dtItem.Rows[i]["CropYear"]);
					}
					if (dt.Rows.Count > 0)
					{
						DDL.BindDDL(dt, CmbItemName, "InvoiceDetailId", "ItemName", "Item Name", ZeroIndex: false);
						if (dt.Rows.Count == 1)
						{
							((UltraGridBase)CmbItemName).Rows[0].Activate();
						}
						((UltraGridBase)CmbItemName).DisplayLayout.Bands[0].Columns[3].Hidden = true;
					}
					if (dtCrop.Rows.Count > 0)
					{
						DDL.BindDDL(dtCrop, CmbCropYear, "CropYearId", "CropYear", "CropYear", ZeroIndex: false);
						((UltraGridBase)CmbCropYear).Rows[0].Activate();
					}
				}
				else
				{
					((Control)(object)CmbItemName).Text = string.Empty;
					((UltraGridBase)CmbItemName).DataSource = null;
					((Control)(object)CmbCropYear).Text = string.Empty;
					((UltraGridBase)CmbCropYear).DataSource = null;
				}
			}
			else
			{
				((Control)(object)CmbItemName).Text = string.Empty;
				((UltraGridBase)CmbItemName).DataSource = null;
				((Control)(object)CmbCropYear).Text = string.Empty;
				((UltraGridBase)CmbCropYear).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbInvoiceNo_TextChanged(object sender, EventArgs e)
	{
		try
		{
			GetInvoicewiseDoWeightandBalanceWeight();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetContainerNoFromCROByInvoiceId(int InvoiceId)
	{
		try
		{
			int ConNo = Conversion.ToInt(cmbContainerNoDetail.Value);
			DataTable dt = ExImInvoice.GetContainerNoFromShipingLineBookingDetailByInvoiceId(UserAccount.OrganizationId, UserAccount.CompanyId, InvoiceId, Id);
			if (dt.Rows.Count > 0)
			{
				dtCont.Clear();
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtCont.Rows.Add(dt.Rows[i]["ContainerId"], dt.Rows[i]["Container"]);
				}
				if (dtContFromReadById.Rows.Count > 0)
				{
					foreach (DataRow r in dtContFromReadById.Rows)
					{
						int containerIdClone = Conversion.ToInt(r["ContainerId"]);
						if (!dtCont.AsEnumerable().Any((DataRow contRow) => Conversion.ToInt(contRow["ContainerId"]) == containerIdClone))
						{
							dtCont.Rows.Add(r["ContainerId"], r["ContainerNo"]);
						}
					}
				}
				if (dtCont.Rows.Count > 0)
				{
					DDL.BindDDL(dtCont, cmbContainerNoDetail, "ContainerId", "ContainerNo", "Container No", ZeroIndex: true);
				}
				else
				{
					((UltraGridBase)cmbContainerNoDetail).DataSource = null;
				}
			}
			else
			{
				((UltraGridBase)cmbContainerNoDetail).DataSource = null;
			}
			if (ConNo > 0)
			{
				cmbContainerNoDetail.Value = ConNo;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Print_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.InvDeliveryOrderSlip(Id, 84);
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
			Id = 0;
			IsApproved = false;
			DocDate.Focus();
			AttachmentsValues = "";
			CustomAttachmentsValues = "";
			txtItemWeight.Text = string.Empty;
			txtPackingWeight.Text = string.Empty;
			txtOtherWeight.Text = string.Empty;
			txtTotalGrossWeight.Text = string.Empty;
			txtOtherRemarks.Text = string.Empty;
			((Control)(object)CmbItemName).Text = string.Empty;
			((Control)(object)CmbPackUom).Text = string.Empty;
			txtqty.Text = string.Empty;
			txtweight.Text = string.Empty;
			((Control)(object)CmbSupplierCustomer).Text = string.Empty;
			((Control)(object)CmbOrderNo).Text = string.Empty;
			((Control)(object)CmbPackingType).Text = string.Empty;
			txtremarks.Text = string.Empty;
			((Control)(object)cmbInvoiceNo).Text = string.Empty;
			txtVehicleNo.Text = string.Empty;
			txtInvoiceDate.Value = DateTime.Now;
			dtContFromReadById.Rows.Clear();
			((UltraGridBase)CmbSupplierCustomer).DataSource = null;
			((UltraGridBase)CmbOrderNo).DataSource = null;
			btnsave.Visible = true;
			btnupdate.Visible = false;
			btnplus.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			DocumentNoFill();
			table.Rows.Clear();
			grd.DataSource = table;
			((Control)(object)cmbInvoiceNo).Enabled = true;
			CmbInvoiceNoFill();
			NetWeight = 0.0;
			txtInvoiceWeight.Text = string.Empty;
			txtDoWeight.Text = string.Empty;
			txtBalanceWeight.Text = string.Empty;
			txtrejectedWeight.Text = string.Empty;
			defaultConfiquration();
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
			cmbInvoiceNo.Focus();
			((Control)(object)CmbItemName).Text = string.Empty;
			((UltraGridBase)CmbItemName).DataSource = null;
			((Control)(object)CmbPackUom).Text = string.Empty;
			((UltraGridBase)CmbPackUom).DataSource = null;
			txtqty.Text = string.Empty;
			txtweight.Text = string.Empty;
			((Control)(object)CmbOrderNo).Text = string.Empty;
			((Control)(object)CmbPackingType).Text = string.Empty;
			txtremarksdetail.Text = string.Empty;
			txtGrossWeight.Text = string.Empty;
			txtPackingWt.Text = string.Empty;
			txtTotalPackingWeight.Text = string.Empty;
			txtInspectionRemarks.Text = string.Empty;
			txtContainerRemarks.Text = string.Empty;
			txtGrossWeight.Text = string.Empty;
			((Control)(object)CmbSupplierCustomer).Text = string.Empty;
			((Control)(object)cmbContainerNoDetail).Text = string.Empty;
			((UltraGridBase)cmbContainerNoDetail).DataSource = null;
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
			BranchList = CommonServices.BrancheServiceBind();
			CmbExportPackingTypeFill();
			WareHouseFill();
			JobLotFill();
			CmbInvoiceNoFill();
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

	private void btnResetHistory_Click(object sender, EventArgs e)
	{
		try
		{
			FromDateHistory.Value = DateTime.Now.AddDays(-3.0);
			ToDateHistory.Value = DateTime.Now;
			FromDocNoHistory.Clear();
			ToDocNoHistory.Clear();
			((Control)(object)CmbCustomerHistory).Text = string.Empty;
			grdhistory.DataSource = null;
			grdDetail.DataSource = null;
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

	private void HistoryCombosFill()
	{
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Expected O, but got Unknown
		try
		{
			DataTable dtCustomer = new DataTable();
			dtCustomer.Columns.Add("Id", typeof(int));
			dtCustomer.Columns.Add("Customer", typeof(string));
			DataTable dtInvoice = new DataTable();
			dtInvoice.Columns.Add("Id", typeof(int));
			dtInvoice.Columns.Add("InvoiceNo", typeof(string));
			DataTable dt = InvDeliveryOrder.GetDataForDropDownFromDeliveryOrder(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				ReqType = "Export",
				DocumentTypeIds = "84"
			});
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToString(dt.Rows[i]["Activity"]) == "Customer")
					{
						dtCustomer.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
					else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "InvoiceNo")
					{
						dtInvoice.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
				}
			}
			DDL.BindDDL(dtCustomer, CmbCustomerHistory, "Id", "Customer", "Party", ZeroIndex: false);
			DDL.BindDDL(dtInvoice, CmbInvoiceNoHistory, "Id", "InvoiceNo", "Invoice No", ZeroIndex: false);
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

	private void grdhistory_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			ReadById(Conversion.ToInt((object)grdhistory.CurrentRow.Cells["Id"].Value.ToString()));
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
			obj.DocumentTypeId = 84;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
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
			obj.FromDocNo = Conversion.ToInt((object)FromDocNoHistory.Text);
			obj.ToDocNo = Conversion.ToInt((object)ToDocNoHistory.Text);
			obj.SupplierCustomerId = Conversion.ToInt(CmbCustomerHistory.Value);
			obj.ExImInvoiceId = Conversion.ToInt(CmbInvoiceNoHistory.Value);
			obj.BranchesId = UserAccount.BranchesId;
			DataTable dtHis = InvDeliveryOrder.ExportFormHistoryNew(obj);
			if (dtHis.Rows.Count > 0)
			{
				if (dtHistory.Columns.Count == 0)
				{
					dtHistory.Columns.Add("Id", typeof(int));
					dtHistory.Columns.Add("DoDate", typeof(DateTime));
					dtHistory.Columns.Add("DoNo", typeof(int));
					dtHistory.Columns.Add("InvoiceId", typeof(int));
					dtHistory.Columns.Add("InvoiceDate", typeof(DateTime));
					dtHistory.Columns.Add("InvoiceNo", typeof(string));
					dtHistory.Columns.Add("InvoiceNetWeight", typeof(double));
					dtHistory.Columns.Add("InvoiceGrossWeight", typeof(double));
					dtHistory.Columns.Add("CustomerName", typeof(string));
					dtHistory.Columns.Add("OrderNo", typeof(string));
					dtHistory.Columns.Add("GatePassId", typeof(int));
					dtHistory.Columns.Add("VehicleNo", typeof(string));
					dtHistory.Columns.Add("Transporter", typeof(string));
					dtHistory.Columns.Add("GpDate", typeof(DateTime));
					dtHistory.Columns.Add("GpSrNo", typeof(int));
					dtHistory.Columns.Add("GpStatus", typeof(string));
					dtHistory.Columns.Add("ForwardingNo", typeof(int));
					dtHistory.Columns.Add("FactoryWeight", typeof(double));
					dtHistory.Columns.Add("EntryDate", typeof(DateTime));
					dtHistory.Columns.Add("EntryUser", typeof(string));
					dtHistory.Columns.Add("ModifyDate", typeof(DateTime));
					dtHistory.Columns.Add("ModifyUser", typeof(string));
					dtHistory.Columns.Add("ApprovedDate", typeof(DateTime));
					dtHistory.Columns.Add("ApprovedUser", typeof(string));
					dtHistory.Columns.Add("BranchName", typeof(string));
					dtHistory.Columns.Add("NoOfAttachments", typeof(int));
				}
				dtHistory.Clear();
				HashSet<int> existingIds = new HashSet<int>();
				foreach (DataRow dr in dtHis.Rows)
				{
					int id = Conversion.ToInt(dr["Id"]);
					if (!existingIds.Contains(id))
					{
						dtHistory.Rows.Add(dr["Id"], Conversion.ToDateTime(dr["DocDate"]).ToShortDateString(), dr["DocNo"], dr["InvoiceId"], dr["InvoiceDate"], dr["InvoiceNo"], dr["InvoiceNetWeight"], dr["InvoiceGrossWeight"], dr["SupplierCustomer"], dr["OrderNo"], dr["GatePassId"], dr["VehicleNo"], dr["Transporter"], dr["GpDate"], dr["GpSrNo"], dr["GpStatus"], dr["ForwardingNo"], dr["FactoryWeight"], dr["EntryDate"], dr["EntryUser"], dr["ModifyDate"], dr["ModifyUser"], dr["ApprovedDate"], dr["ApprovedUser"], dr["BranchName"], dr["NoOfAttachments"]);
						existingIds.Add(id);
					}
				}
				grdhistory.DataSource = dtHistory;
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
			grdhistory.RootTable.Columns["GatePassId"].Visible = false;
			grdhistory.RootTable.Columns["InvoiceId"].Visible = false;
			grdhistory.RootTable.Columns["BranchName"].Visible = BranchList.Rows.Count > 1;
			grdhistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			CommonServices.GridWrappingAndColumnSettings(grdhistory, 2);
			grdhistory.RootTable.Columns.Add("Print");
			grdhistory.RootTable.Columns["Print"].Caption = "Print";
			grdhistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdhistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			grdhistory.RootTable.Columns["Print"].ButtonText = "Print";
			grdhistory.RootTable.Columns["Print"].Width = 50;
			grdhistory.RootTable.Columns.Add("Edit");
			grdhistory.RootTable.Columns["Edit"].Caption = "Edit";
			grdhistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdhistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdhistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdhistory.RootTable.Columns["Edit"].Width = 50;
			grdhistory.RootTable.Columns["Edit"].Position = 0;
			grdhistory.RootTable.Columns["Print"].Position = 1;
			grdhistory.FrozenColumns = 2;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(grdhistory.CurrentRow.Cells["Id"].Value), 84);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_SelectionChanged(object sender, EventArgs e)
	{
		try
		{
			GetDetailByHeaderId();
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
				int RecId = 0;
				RecId = Conversion.ToInt((object)grdhistory.CurrentRow.Cells["Id"].Value.ToString());
				ReadById(RecId);
			}
			if (e.Column.Key == "Print")
			{
				CommonServices.InvDeliveryOrderSlip(Conversion.ToInt((object)grdhistory.CurrentRow.Cells["Id"].Value.ToString()), 84);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetDetailByHeaderId()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_027d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0283: Expected O, but got Unknown
		try
		{
			if (grdhistory.CurrentRow == null || (int)grdhistory.CurrentRow.RowType != 0 || grdhistory.CurrentRow.Cells.Count <= 0)
			{
				return;
			}
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("InvocieNo", typeof(string));
			dt.Columns.Add("CustomerName", typeof(string));
			dt.Columns.Add("OrderNo", typeof(string));
			dt.Columns.Add("WareHouse", typeof(string));
			dt.Columns.Add("JobLot", typeof(string));
			dt.Columns.Add("Item", typeof(string));
			dt.Columns.Add("ItemUOM", typeof(string));
			dt.Columns.Add("PackingType", typeof(string));
			dt.Columns.Add("QTY", typeof(double));
			dt.Columns.Add("Weight", typeof(double));
			dt.Columns.Add("PackingWeight", typeof(double));
			dt.Columns.Add("TotalPackingWeight", typeof(double));
			dt.Columns.Add("GrossWeight", typeof(double));
			dt.Columns.Add("OtherWeight", typeof(double));
			dt.Columns.Add("LoadQty", typeof(double));
			dt.Columns.Add("LoadWeight", typeof(double));
			dt.Columns.Add("ContainerNo", typeof(string));
			dt.Columns.Add("ContainerRemarks", typeof(string));
			dt.Columns.Add("InspectionRemarks", typeof(string));
			dt.Columns.Add("Remarks", typeof(string));
			InvDeliveryOrder po = new InvDeliveryOrder();
			po = InvDeliveryOrder.GetByID(Conversion.ToInt(grdhistory.CurrentRow.Cells["Id"].Value));
			if (po.InvDeliveryOrderDetailList.Count > 0)
			{
				dt.Rows.Clear();
				for (int i = 0; i < po.InvDeliveryOrderDetailList.Count; i++)
				{
					dt.Rows.Add(po.InvDeliveryOrderDetailList[i].Id, po.InvDeliveryOrderDetailList[i].InvoiceNo, po.InvDeliveryOrderDetailList[i].SupplierCustomer, po.InvDeliveryOrderDetailList[i].OrderNo, po.InvDeliveryOrderDetailList[i].WareHouseName, po.InvDeliveryOrderDetailList[i].JobLotDescription, po.InvDeliveryOrderDetailList[i].ItemName, po.InvDeliveryOrderDetailList[i].PackUOM, po.InvDeliveryOrderDetailList[i].PackTypeDesc, po.InvDeliveryOrderDetailList[i].DoQty, po.InvDeliveryOrderDetailList[i].DoWeight, po.InvDeliveryOrderDetailList[i].PackingWeight, po.InvDeliveryOrderDetailList[i].TotalPackingWeight, po.InvDeliveryOrderDetailList[i].GrossWeight, po.InvDeliveryOrderDetailList[i].OtherWeight, po.InvDeliveryOrderDetailList[i].LoadingQty, po.InvDeliveryOrderDetailList[i].LoadingWeight, po.InvDeliveryOrderDetailList[i].Container, po.InvDeliveryOrderDetailList[i].ContainerRemarks, po.InvDeliveryOrderDetailList[i].InspectionRemarks, po.InvDeliveryOrderDetailList[i].LoadingRemarks);
				}
				grdDetail.DataSource = dt;
				grdDetail.RetrieveStructure();
				grdDetailSettings();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetailSettings()
	{
		try
		{
			grdDetail.RootTable.Columns["Id"].Visible = false;
			CommonServices.GridWrappingAndColumnSettings(grdDetail, 2);
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
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grd, table);
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
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grdhistory, dtHistory);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void btnattachment_Click(object sender, EventArgs e)
	{
		AT.Show();
	}

	private void CalculateWeight()
	{
		try
		{
			txtweight.Text = 0.ToString();
			txtGrossWeight.Text = 0.ToString();
			if (((UltraGridBase)CmbPackUom).ActiveRow != null && Conversion.ToDouble((object)txtqty.Text) != 0.0)
			{
				double ItemPack = Conversion.ToDouble(((UltraDropDownBase)CmbPackUom).SelectedRow.Cells[2].Value);
				double Qty = Conversion.ToDouble((object)txtqty.Text);
				NetWeight = ItemPack * Qty;
				txtweight.Text = NetWeight.ToString("#,##0.##");
				txtGrossWeight.Text = NetWeight.ToString("#,##0.##");
				txtPackingWeight_TextChanged(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculaterPackingWeight()
	{
		try
		{
			double PackingWeight = Conversion.ToDouble((object)txtPackingWt.Text);
			double TotalPackingWeight = Conversion.ToDouble((object)txtTotalPackingWeight.Text);
			double Qty = Conversion.ToDouble((object)txtqty.Text);
			if (Conversion.ToString(base.ActiveControl.Tag) == "TotalPackingWeight")
			{
				txtPackingWt.Text = (TotalPackingWeight / Qty).ToString("#,##0.##");
			}
			else if (Conversion.ToString(base.ActiveControl.Tag) == "PackingWeight")
			{
				txtTotalPackingWeight.Text = (PackingWeight * Qty).ToString("#,##0.##");
			}
			else
			{
				txtTotalPackingWeight.Text = (PackingWeight * Qty).ToString("#,##0.##");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combitempck_Leave(object sender, EventArgs e)
	{
		CalculateWeight();
	}

	private void txtqty_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculaterPackingWeight();
			CalculateWeight();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtPackingWeight_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (NetWeight > 0.0)
			{
				double GrossWeight = NetWeight + Conversion.ToDouble((object)txtTotalPackingWeight.Text);
				txtGrossWeight.Text = GrossWeight.ToString("#,##0.##");
			}
			else
			{
				txtGrossWeight.Text = NetWeight.ToString("#,##0.##");
			}
			CalculaterPackingWeight();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtPackingWt_TextChanged(object sender, EventArgs e)
	{
		CalculaterPackingWeight();
	}

	private void txtTotalQty_KeyPress(object sender, KeyPressEventArgs e)
	{
		CommonServices.OnlytextdecimelFunction(sender, e);
	}

	private void txtqty_KeyPress(object sender, KeyPressEventArgs e)
	{
		CommonServices.OnlytextdecimelFunction(sender, e);
	}

	private void txtPackingWeight_KeyPress(object sender, KeyPressEventArgs e)
	{
		CommonServices.OnlytextdecimelFunction(sender, e);
	}

	private void txtweight_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtPackingWt_KeyPress(object sender, KeyPressEventArgs e)
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

	private void ExportDeliveryOrderNew_FormClosing(object sender, FormClosingEventArgs e)
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

	private void PurchsaeOrder_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.KeyCode == Keys.Delete && tabControl1.SelectedIndex == 1)
			{
				GridEXRow[] checkedRows = grdhistory.GetCheckedRows();
				for (int i = 0; i < checkedRows.Length; i++)
				{
					Conversion.ToInt((object)checkedRows[i].Cells["Id"].Value.ToString());
				}
			}
			if (btnsave.Visible && btnsave.Enabled && e.Control && e.KeyCode == Keys.S && tabControl1.SelectedIndex == 0)
			{
				btnsave_Click(null, null);
			}
			if (btnupdate.Visible && btnupdate.Enabled && e.Control && e.KeyCode == Keys.U)
			{
				btnupdate_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.T)
			{
				if (tabControl1.SelectedIndex == 1)
				{
					tabControl1.SelectedIndex = 0;
				}
				else
				{
					tabControl1.SelectedIndex = 1;
				}
			}
			if (e.Control && e.KeyCode == Keys.N && tabControl1.SelectedIndex == 0)
			{
				btnnew_Click(null, null);
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.R)
			{
				btnRefresh_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				DocDate.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					((Control)(object)grd).Focus();
				}
				else if (tabControl1.SelectedIndex == 1)
				{
					((Control)(object)grdDetail).Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					cmbInvoiceNo.Focus();
				}
				else if (tabControl1.SelectedIndex == 1)
				{
					((Control)(object)grdhistory).Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.F10)
			{
				AT.Show();
			}
			if (e.Control && e.KeyCode == Keys.P)
			{
				CommonServices.BookOrderReports273A(Id);
			}
			if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
			}
			if (!e.Control || e.KeyCode != Keys.Return)
			{
				return;
			}
			if (tabControl1.SelectedIndex == 0)
			{
				GridEXRow item = grd.CurrentRow;
				if (item != null && (int)item.RowType == 0)
				{
					grd_DoubleClick(null, null);
				}
			}
			else if (tabControl1.SelectedIndex == 1)
			{
				Id = Conversion.ToInt((object)grdhistory.CurrentRow.Cells["Id"].Value.ToString());
				ReadById(Id);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShortCutKeys_Click(object sender, EventArgs e)
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
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Invoice no in Detail Box");
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
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_039c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Expected O, but got Unknown
		//IL_037d: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!e.Control || e.KeyCode != Keys.Space)
			{
				return;
			}
			GridEXRow item = grd.CurrentRow;
			if (item == null || (int)item.RowType != 0 || grd.CurrentColumn == null || grd.CurrentColumn == null)
			{
				return;
			}
			Conversion.ToInt((object)item.Cells["Id"].Value.ToString());
			if (grd.CurrentColumn.Key == "Delete")
			{
				GridEXRow r = grd.CurrentRow;
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					InvDeliveryOrderdetail vd = new InvDeliveryOrderdetail();
					vd.RefDocumentTypeId = 0;
					vd.RefDocIdNo = 0;
					vd.RefDocSubIdNo = 0;
					vd.Id = Conversion.ToInt(r.Cells["Id"].Value);
					vd.ExImInvoiceId = Conversion.ToInt(r.Cells["InvoiceId"].Value);
					vd.SupplierCustomerId = Conversion.ToInt(r.Cells["SupplierCustomerId"].Value);
					vd.SaleOrderId = Conversion.ToInt(r.Cells["OrderId"].Value);
					vd.WarehouseId = Conversion.ToInt(r.Cells["WareHouseId"].Value);
					vd.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
					vd.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					vd.CropYearId = Conversion.ToInt(r.Cells["CropYearId"].Value);
					vd.PackUomId = Conversion.ToInt(r.Cells["ItemUOMId"].Value);
					vd.InvPackingTypeId = Conversion.ToInt(r.Cells["PackingTypeId"].Value);
					vd.DoQty = Conversion.ToDouble(r.Cells["LoadQty"].Value);
					vd.DoWeight = Conversion.ToDouble(r.Cells["LoadWeight"].Value);
					vd.LoadingQty = vd.DoQty;
					vd.LoadingWeight = vd.DoWeight;
					vd.PackingWeight = Conversion.ToDouble(r.Cells["PackingWeight"].Value);
					vd.TotalPackingWeight = Conversion.ToDouble(r.Cells["TotalPackingWeight"].Value);
					vd.GrossWeight = Conversion.ToDouble(r.Cells["GrossWeight"].Value);
					vd.LoadingRemarks = Conversion.ToString(r.Cells["Remarks"].Value);
					vd.InspectionRemarks = Conversion.ToString(r.Cells["InspectionRemarks"].Value);
					vd.ContainerRemarks = Conversion.ToString(r.Cells["ContainerRemarks"].Value);
					vd.ActionTypeId = 3;
					lstRemoveRecord.Add(vd);
					table.Rows[r.RowIndex].Delete();
					if (r != null && (int)r.RowType == 0)
					{
						r.Delete();
					}
					grd.UpdateData();
				}
				else if (r != null && (int)r.RowType == 0)
				{
					r.Delete();
				}
			}
			if (grd.CurrentColumn.Key == "Edit")
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
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!e.Control || e.KeyCode != Keys.Space)
			{
				return;
			}
			GridEXRow r = grdhistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && grdhistory.CurrentColumn != null)
			{
				Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
				if (formrights.DoHavePrintRights && grdhistory.CurrentColumn.Key == "Print")
				{
					CommonServices.InvDeliveryOrderSlip(Conversion.ToInt((object)grdhistory.CurrentRow.Cells["Id"].Value.ToString()), 84);
				}
				if (formrights.DoHaveUpdateRights && grdhistory.CurrentColumn.Key == "Edit")
				{
					int RecId = 0;
					RecId = Conversion.ToInt((object)grdhistory.CurrentRow.Cells["Id"].Value.ToString());
					ReadById(RecId);
				}
				if (grdhistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(grdhistory.CurrentRow.Cells["Id"].Value), 84);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtOtherWeight_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtOtherWeight_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateTotalInformation();
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
		//IL_0408: Unknown result type (might be due to invalid IL or missing references)
		//IL_040f: Expected O, but got Unknown
		//IL_040f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0416: Expected O, but got Unknown
		//IL_0416: Unknown result type (might be due to invalid IL or missing references)
		//IL_041d: Expected O, but got Unknown
		//IL_041d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0424: Expected O, but got Unknown
		//IL_0424: Unknown result type (might be due to invalid IL or missing references)
		//IL_042b: Expected O, but got Unknown
		//IL_042b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0432: Expected O, but got Unknown
		//IL_0432: Unknown result type (might be due to invalid IL or missing references)
		//IL_0439: Expected O, but got Unknown
		//IL_0439: Unknown result type (might be due to invalid IL or missing references)
		//IL_0440: Expected O, but got Unknown
		//IL_0440: Unknown result type (might be due to invalid IL or missing references)
		//IL_0447: Expected O, but got Unknown
		//IL_0447: Unknown result type (might be due to invalid IL or missing references)
		//IL_044e: Expected O, but got Unknown
		//IL_044e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0455: Expected O, but got Unknown
		//IL_0455: Unknown result type (might be due to invalid IL or missing references)
		//IL_045c: Expected O, but got Unknown
		//IL_045c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0463: Expected O, but got Unknown
		//IL_0463: Unknown result type (might be due to invalid IL or missing references)
		//IL_046a: Expected O, but got Unknown
		//IL_046a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0471: Expected O, but got Unknown
		//IL_0471: Unknown result type (might be due to invalid IL or missing references)
		//IL_0478: Expected O, but got Unknown
		//IL_0478: Unknown result type (might be due to invalid IL or missing references)
		//IL_047f: Expected O, but got Unknown
		//IL_047f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0486: Expected O, but got Unknown
		//IL_0486: Unknown result type (might be due to invalid IL or missing references)
		//IL_048d: Expected O, but got Unknown
		//IL_048d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0494: Expected O, but got Unknown
		//IL_0494: Unknown result type (might be due to invalid IL or missing references)
		//IL_049b: Expected O, but got Unknown
		//IL_049b: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a2: Expected O, but got Unknown
		//IL_04a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a9: Expected O, but got Unknown
		//IL_04dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e6: Expected O, but got Unknown
		//IL_0626: Unknown result type (might be due to invalid IL or missing references)
		//IL_0630: Expected O, but got Unknown
		//IL_065d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0667: Expected O, but got Unknown
		//IL_069f: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a9: Expected O, but got Unknown
		//IL_06cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d5: Expected O, but got Unknown
		//IL_0744: Unknown result type (might be due to invalid IL or missing references)
		//IL_074e: Expected O, but got Unknown
		//IL_074f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0759: Expected O, but got Unknown
		//IL_0765: Unknown result type (might be due to invalid IL or missing references)
		//IL_076f: Expected O, but got Unknown
		//IL_077b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0785: Expected O, but got Unknown
		//IL_079c: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a6: Expected O, but got Unknown
		//IL_07a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b1: Expected O, but got Unknown
		//IL_07c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d2: Expected O, but got Unknown
		//IL_07d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_07dd: Expected O, but got Unknown
		//IL_08d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_08da: Expected O, but got Unknown
		//IL_08f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_08fb: Expected O, but got Unknown
		//IL_093e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0948: Expected O, but got Unknown
		//IL_09b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c1: Expected O, but got Unknown
		//IL_0efc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f06: Expected O, but got Unknown
		//IL_719f: Unknown result type (might be due to invalid IL or missing references)
		//IL_71a9: Expected O, but got Unknown
		//IL_71cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_71d7: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance66 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance79 = new Appearance();
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
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
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
		Appearance appearance150 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance157 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance168 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
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
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		GridEXLayout grdhistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance135 = new Appearance();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Export.ExportDeliveryOrderNew));
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.grd = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label19 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel4 = new System.Windows.Forms.Panel();
		this.groupBox4 = new System.Windows.Forms.GroupBox();
		this.txtItemWeight = new System.Windows.Forms.TextBox();
		this.label24 = new System.Windows.Forms.Label();
		this.txtOtherRemarks = new System.Windows.Forms.TextBox();
		this.txtTotalGrossWeight = new System.Windows.Forms.TextBox();
		this.label25 = new System.Windows.Forms.Label();
		this.label26 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.txtPackingWeight = new System.Windows.Forms.TextBox();
		this.txtOtherWeight = new System.Windows.Forms.TextBox();
		this.label28 = new System.Windows.Forms.Label();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.txtInvoiceDate = new System.Windows.Forms.DateTimePicker();
		this.label11 = new System.Windows.Forms.Label();
		this.txtBalanceWeight = new System.Windows.Forms.TextBox();
		this.txtrejectedWeight = new System.Windows.Forms.TextBox();
		this.label15 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.txtInvoiceWeight = new System.Windows.Forms.TextBox();
		this.txtDoWeight = new System.Windows.Forms.TextBox();
		this.label14 = new System.Windows.Forms.Label();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.label29 = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.cmbtransporter = new UltraCombo();
		this.label20 = new System.Windows.Forms.Label();
		this.txtremarks = new System.Windows.Forms.TextBox();
		this.lblBranchName = new System.Windows.Forms.Label();
		this.label50 = new System.Windows.Forms.Label();
		this.CmbBranch = new UltraCombo();
		this.DocDate = new System.Windows.Forms.DateTimePicker();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.txtVehicleNo = new System.Windows.Forms.TextBox();
		this.label5 = new System.Windows.Forms.Label();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.cmbContainerNoDetail = new UltraCombo();
		this.label22 = new System.Windows.Forms.Label();
		this.txtPackingWt = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.CmbCropYear = new UltraCombo();
		this.label16 = new System.Windows.Forms.Label();
		this.txtContainerRemarks = new System.Windows.Forms.TextBox();
		this.label10 = new System.Windows.Forms.Label();
		this.txtInspectionRemarks = new System.Windows.Forms.TextBox();
		this.label9 = new System.Windows.Forms.Label();
		this.txtGrossWeight = new System.Windows.Forms.TextBox();
		this.label7 = new System.Windows.Forms.Label();
		this.txtTotalPackingWeight = new System.Windows.Forms.TextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.CmbJobLot = new UltraCombo();
		this.cmbInvoiceNo = new UltraCombo();
		this.label8 = new System.Windows.Forms.Label();
		this.CmbWareHouse = new UltraCombo();
		this.label3 = new System.Windows.Forms.Label();
		this.CmbSupplierCustomer = new UltraCombo();
		this.label2 = new System.Windows.Forms.Label();
		this.btnCancelUpdateDetial = new System.Windows.Forms.Button();
		this.CmbPackUom = new UltraCombo();
		this.CmbPackingType = new UltraCombo();
		this.label36 = new System.Windows.Forms.Label();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.CmbOrderNo = new UltraCombo();
		this.CmbItemName = new UltraCombo();
		this.txtqty = new System.Windows.Forms.TextBox();
		this.btnplus = new System.Windows.Forms.Button();
		this.label38 = new System.Windows.Forms.Label();
		this.label35 = new System.Windows.Forms.Label();
		this.label39 = new System.Windows.Forms.Label();
		this.txtremarksdetail = new System.Windows.Forms.TextBox();
		this.txtweight = new System.Windows.Forms.TextBox();
		this.label47 = new System.Windows.Forms.Label();
		this.label45 = new System.Windows.Forms.Label();
		this.label37 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnprint = new System.Windows.Forms.ToolStripButton();
		this.btnattachment = new System.Windows.Forms.ToolStripButton();
		this.btnShortCutKeys = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.grdhistory = new GridEX();
		this.panel9 = new System.Windows.Forms.Panel();
		this.label18 = new System.Windows.Forms.Label();
		this.grdDetail = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.label17 = new System.Windows.Forms.Label();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel7 = new System.Windows.Forms.Panel();
		this.groupBox5 = new System.Windows.Forms.GroupBox();
		this.label53 = new System.Windows.Forms.Label();
		this.CmbInvoiceNoHistory = new UltraCombo();
		this.rdapproveddate = new System.Windows.Forms.RadioButton();
		this.btnShowHistory = new System.Windows.Forms.Button();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.label34 = new System.Windows.Forms.Label();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.label73 = new System.Windows.Forms.Label();
		this.label89 = new System.Windows.Forms.Label();
		this.label90 = new System.Windows.Forms.Label();
		this.label91 = new System.Windows.Forms.Label();
		this.CmbCustomerHistory = new UltraCombo();
		this.ToDocNoHistory = new System.Windows.Forms.TextBox();
		this.FromDocNoHistory = new System.Windows.Forms.TextBox();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.panel5 = new System.Windows.Forms.Panel();
		this.label13 = new System.Windows.Forms.Label();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnResetHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel6.SuspendLayout();
		this.panel4.SuspendLayout();
		this.groupBox4.SuspendLayout();
		this.groupBox2.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbtransporter).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBranch).BeginInit();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbContainerNoDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYear).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbInvoiceNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWareHouse).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplierCustomer).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackingType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbOrderNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).BeginInit();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdhistory).BeginInit();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel8.SuspendLayout();
		this.panel7.SuspendLayout();
		this.groupBox5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbInvoiceNoHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).BeginInit();
		this.panel5.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1284, 658);
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
		this.tabControl1.Size = new System.Drawing.Size(1284, 658);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.tabPage1.Controls.Add(this.panel6);
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage1.Size = new System.Drawing.Size(1276, 629);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.grd.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		this.grd.ColumnAutoResize = true;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(2, 359);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1272, 268);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 1;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grd_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.label19);
		this.panel6.Controls.Add(this.ctrlGrdBar1);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(2, 334);
		this.panel6.Margin = new System.Windows.Forms.Padding(2);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1272, 25);
		this.panel6.TabIndex = 4;
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label19.ForeColor = System.Drawing.Color.White;
		this.label19.Location = new System.Drawing.Point(6, 4);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(46, 17);
		this.label19.TabIndex = 0;
		this.label19.Text = "Detail";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1225, -1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 24);
		this.ctrlGrdBar1.TabIndex = 20;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.groupBox4);
		this.panel4.Controls.Add(this.groupBox2);
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Controls.Add(this.groupBox3);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(2, 56);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1272, 278);
		this.panel4.TabIndex = 0;
		this.groupBox4.BackColor = System.Drawing.Color.WhiteSmoke;
		this.groupBox4.Controls.Add(this.txtItemWeight);
		this.groupBox4.Controls.Add(this.label24);
		this.groupBox4.Controls.Add(this.txtOtherRemarks);
		this.groupBox4.Controls.Add(this.txtTotalGrossWeight);
		this.groupBox4.Controls.Add(this.label25);
		this.groupBox4.Controls.Add(this.label26);
		this.groupBox4.Controls.Add(this.label27);
		this.groupBox4.Controls.Add(this.txtPackingWeight);
		this.groupBox4.Controls.Add(this.txtOtherWeight);
		this.groupBox4.Controls.Add(this.label28);
		this.groupBox4.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox4.Location = new System.Drawing.Point(822, 3);
		this.groupBox4.Name = "groupBox4";
		this.groupBox4.Size = new System.Drawing.Size(444, 105);
		this.groupBox4.TabIndex = 1;
		this.groupBox4.TabStop = false;
		this.groupBox4.Text = "Weight Information";
		this.txtItemWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemWeight.Enabled = false;
		this.txtItemWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemWeight.Location = new System.Drawing.Point(104, 20);
		this.txtItemWeight.Name = "txtItemWeight";
		this.txtItemWeight.Size = new System.Drawing.Size(116, 23);
		this.txtItemWeight.TabIndex = 0;
		this.txtItemWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label24.Location = new System.Drawing.Point(4, 22);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(74, 15);
		this.label24.TabIndex = 30;
		this.label24.Text = "Item Weight";
		this.txtOtherRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOtherRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOtherRemarks.Location = new System.Drawing.Point(104, 72);
		this.txtOtherRemarks.Name = "txtOtherRemarks";
		this.txtOtherRemarks.Size = new System.Drawing.Size(331, 23);
		this.txtOtherRemarks.TabIndex = 4;
		this.txtTotalGrossWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTotalGrossWeight.Enabled = false;
		this.txtTotalGrossWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTotalGrossWeight.Location = new System.Drawing.Point(323, 45);
		this.txtTotalGrossWeight.Name = "txtTotalGrossWeight";
		this.txtTotalGrossWeight.Size = new System.Drawing.Size(112, 23);
		this.txtTotalGrossWeight.TabIndex = 3;
		this.txtTotalGrossWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label25.Location = new System.Drawing.Point(4, 76);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(85, 15);
		this.label25.TabIndex = 36;
		this.label25.Text = "Other Remarks";
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label26.Location = new System.Drawing.Point(4, 49);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(91, 15);
		this.label26.TabIndex = 32;
		this.label26.Text = "Packing Weight";
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label27.Location = new System.Drawing.Point(229, 49);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(84, 15);
		this.label27.TabIndex = 36;
		this.label27.Text = "Total Gross Wt";
		this.txtPackingWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPackingWeight.Enabled = false;
		this.txtPackingWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPackingWeight.Location = new System.Drawing.Point(104, 45);
		this.txtPackingWeight.Name = "txtPackingWeight";
		this.txtPackingWeight.Size = new System.Drawing.Size(116, 23);
		this.txtPackingWeight.TabIndex = 1;
		this.txtPackingWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtOtherWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOtherWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOtherWeight.Location = new System.Drawing.Point(323, 18);
		this.txtOtherWeight.Name = "txtOtherWeight";
		this.txtOtherWeight.Size = new System.Drawing.Size(112, 23);
		this.txtOtherWeight.TabIndex = 2;
		this.txtOtherWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtOtherWeight.TextChanged += new System.EventHandler(txtOtherWeight_TextChanged);
		this.txtOtherWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtOtherWeight_KeyPress);
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label28.Location = new System.Drawing.Point(229, 22);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(79, 15);
		this.label28.TabIndex = 34;
		this.label28.Text = "Other Weight";
		this.groupBox2.BackColor = System.Drawing.Color.WhiteSmoke;
		this.groupBox2.Controls.Add(this.txtInvoiceDate);
		this.groupBox2.Controls.Add(this.label11);
		this.groupBox2.Controls.Add(this.txtBalanceWeight);
		this.groupBox2.Controls.Add(this.txtrejectedWeight);
		this.groupBox2.Controls.Add(this.label15);
		this.groupBox2.Controls.Add(this.label12);
		this.groupBox2.Controls.Add(this.label21);
		this.groupBox2.Controls.Add(this.txtInvoiceWeight);
		this.groupBox2.Controls.Add(this.txtDoWeight);
		this.groupBox2.Controls.Add(this.label14);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(1048, 110);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(218, 166);
		this.groupBox2.TabIndex = 3;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Invoice Information";
		this.txtInvoiceDate.CustomFormat = "dd-MMM-yy";
		this.txtInvoiceDate.Enabled = false;
		this.txtInvoiceDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtInvoiceDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtInvoiceDate.Location = new System.Drawing.Point(97, 18);
		this.txtInvoiceDate.Name = "txtInvoiceDate";
		this.txtInvoiceDate.Size = new System.Drawing.Size(112, 23);
		this.txtInvoiceDate.TabIndex = 0;
		this.txtInvoiceDate.TabStop = false;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label11.Location = new System.Drawing.Point(5, 22);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(74, 15);
		this.label11.TabIndex = 30;
		this.label11.Text = "Invoice Date";
		this.txtBalanceWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBalanceWeight.Enabled = false;
		this.txtBalanceWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBalanceWeight.Location = new System.Drawing.Point(97, 76);
		this.txtBalanceWeight.Name = "txtBalanceWeight";
		this.txtBalanceWeight.Size = new System.Drawing.Size(112, 23);
		this.txtBalanceWeight.TabIndex = 2;
		this.txtBalanceWeight.TabStop = false;
		this.txtrejectedWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtrejectedWeight.Enabled = false;
		this.txtrejectedWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtrejectedWeight.Location = new System.Drawing.Point(97, 133);
		this.txtrejectedWeight.Name = "txtrejectedWeight";
		this.txtrejectedWeight.Size = new System.Drawing.Size(112, 23);
		this.txtrejectedWeight.TabIndex = 4;
		this.txtrejectedWeight.TabStop = false;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label15.Location = new System.Drawing.Point(5, 80);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(90, 15);
		this.label15.TabIndex = 36;
		this.label15.Text = "Balance Weight";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label12.Location = new System.Drawing.Point(5, 51);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(88, 15);
		this.label12.TabIndex = 32;
		this.label12.Text = "Invoice Weight";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label21.Location = new System.Drawing.Point(5, 137);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(94, 15);
		this.label21.TabIndex = 36;
		this.label21.Text = "Rejected Weight";
		this.txtInvoiceWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtInvoiceWeight.Enabled = false;
		this.txtInvoiceWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtInvoiceWeight.Location = new System.Drawing.Point(97, 47);
		this.txtInvoiceWeight.Name = "txtInvoiceWeight";
		this.txtInvoiceWeight.Size = new System.Drawing.Size(112, 23);
		this.txtInvoiceWeight.TabIndex = 1;
		this.txtInvoiceWeight.TabStop = false;
		this.txtDoWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDoWeight.Enabled = false;
		this.txtDoWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDoWeight.Location = new System.Drawing.Point(97, 104);
		this.txtDoWeight.Name = "txtDoWeight";
		this.txtDoWeight.Size = new System.Drawing.Size(112, 23);
		this.txtDoWeight.TabIndex = 3;
		this.txtDoWeight.TabStop = false;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label14.Location = new System.Drawing.Point(5, 108);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(65, 15);
		this.label14.TabIndex = 34;
		this.label14.Text = "Do Weight";
		this.groupBox1.Controls.Add(this.label29);
		this.groupBox1.Controls.Add(this.label33);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbtransporter);
		this.groupBox1.Controls.Add(this.label20);
		this.groupBox1.Controls.Add(this.txtremarks);
		this.groupBox1.Controls.Add(this.lblBranchName);
		this.groupBox1.Controls.Add(this.label50);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBranch);
		this.groupBox1.Controls.Add(this.DocDate);
		this.groupBox1.Controls.Add(this.txtdocno);
		this.groupBox1.Controls.Add(this.txtVehicleNo);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(3, 3);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(817, 105);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Main Information";
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.Location = new System.Drawing.Point(7, 50);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(57, 15);
		this.label29.TabIndex = 68;
		this.label29.Text = "Doc Date";
		this.label33.AutoSize = true;
		this.label33.BackColor = System.Drawing.Color.Transparent;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(202, 50);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(52, 15);
		this.label33.TabIndex = 8;
		this.label33.Text = "Remarks";
		this.label33.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.cmbtransporter.AllowNull = (DefaultableBoolean)1;
		this.cmbtransporter.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbtransporter.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbtransporter.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.Appearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance68).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbtransporter).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance68;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance69;
		((SpecialBoxBase)((UltraGridBase)this.cmbtransporter).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance70).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance70).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance71;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance72).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance73;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance74).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance75).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance75).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance75).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance75).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance75;
		((AppearanceBase)appearance76).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbtransporter).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance77).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbtransporter).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbtransporter).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbtransporter.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbtransporter).Location = new System.Drawing.Point(285, 17);
		((System.Windows.Forms.Control)(object)this.cmbtransporter).Name = "cmbtransporter";
		((System.Windows.Forms.Control)(object)this.cmbtransporter).Size = new System.Drawing.Size(272, 26);
		((System.Windows.Forms.Control)(object)this.cmbtransporter).TabIndex = 3;
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(7, 78);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(64, 15);
		this.label20.TabIndex = 9;
		this.label20.Text = "Vehicle No";
		this.txtremarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtremarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtremarks.Location = new System.Drawing.Point(285, 48);
		this.txtremarks.Multiline = true;
		this.txtremarks.Name = "txtremarks";
		this.txtremarks.Size = new System.Drawing.Size(529, 51);
		this.txtremarks.TabIndex = 5;
		this.lblBranchName.AutoSize = true;
		this.lblBranchName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblBranchName.Location = new System.Drawing.Point(556, 23);
		this.lblBranchName.Name = "lblBranchName";
		this.lblBranchName.Size = new System.Drawing.Size(79, 15);
		this.lblBranchName.TabIndex = 11;
		this.lblBranchName.Text = "Branch Name";
		this.lblBranchName.Visible = false;
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label50.Location = new System.Drawing.Point(202, 23);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(67, 15);
		this.label50.TabIndex = 66;
		this.label50.Text = "Transporter";
		this.CmbBranch.AllowNull = (DefaultableBoolean)1;
		this.CmbBranch.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBranch.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBranch.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Appearance = (AppearanceBase)(object)appearance100;
		((UltraGridBase)this.CmbBranch).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBranch).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance101).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance101).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance101).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBranch).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance101;
		((AppearanceBase)appearance102).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBranch).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance102;
		((SpecialBoxBase)((UltraGridBase)this.CmbBranch).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance104).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance104).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance104).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBranch).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.CmbBranch).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBranch).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance105).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance105;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance106).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance107;
		((AppearanceBase)appearance108).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance108).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance109).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance109).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance109).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance109).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance109;
		((AppearanceBase)appearance110).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance111).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBranch).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.CmbBranch).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBranch).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBranch).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBranch).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbBranch.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBranch).Location = new System.Drawing.Point(638, 17);
		((System.Windows.Forms.Control)(object)this.CmbBranch).Name = "CmbBranch";
		((System.Windows.Forms.Control)(object)this.CmbBranch).Size = new System.Drawing.Size(176, 26);
		((System.Windows.Forms.Control)(object)this.CmbBranch).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.CmbBranch).Visible = false;
		this.DocDate.CustomFormat = "dd-MMM-yy";
		this.DocDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DocDate.Location = new System.Drawing.Point(74, 46);
		this.DocDate.Name = "DocDate";
		this.DocDate.Size = new System.Drawing.Size(122, 23);
		this.DocDate.TabIndex = 1;
		this.DocDate.TabStop = false;
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Enabled = false;
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocno.Location = new System.Drawing.Point(74, 19);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.Size = new System.Drawing.Size(122, 23);
		this.txtdocno.TabIndex = 0;
		this.txtVehicleNo.Location = new System.Drawing.Point(74, 74);
		this.txtVehicleNo.Name = "txtVehicleNo";
		this.txtVehicleNo.Size = new System.Drawing.Size(122, 23);
		this.txtVehicleNo.TabIndex = 2;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(7, 23);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(48, 15);
		this.label5.TabIndex = 3;
		this.label5.Text = "Doc No";
		this.groupBox3.BackColor = System.Drawing.Color.White;
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbContainerNoDetail);
		this.groupBox3.Controls.Add(this.label22);
		this.groupBox3.Controls.Add(this.txtPackingWt);
		this.groupBox3.Controls.Add(this.label1);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCropYear);
		this.groupBox3.Controls.Add(this.label16);
		this.groupBox3.Controls.Add(this.txtContainerRemarks);
		this.groupBox3.Controls.Add(this.label10);
		this.groupBox3.Controls.Add(this.txtInspectionRemarks);
		this.groupBox3.Controls.Add(this.label9);
		this.groupBox3.Controls.Add(this.txtGrossWeight);
		this.groupBox3.Controls.Add(this.label7);
		this.groupBox3.Controls.Add(this.txtTotalPackingWeight);
		this.groupBox3.Controls.Add(this.label6);
		this.groupBox3.Controls.Add(this.label4);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobLot);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbInvoiceNo);
		this.groupBox3.Controls.Add(this.label8);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWareHouse);
		this.groupBox3.Controls.Add(this.label3);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSupplierCustomer);
		this.groupBox3.Controls.Add(this.label2);
		this.groupBox3.Controls.Add(this.btnCancelUpdateDetial);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPackUom);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPackingType);
		this.groupBox3.Controls.Add(this.label36);
		this.groupBox3.Controls.Add(this.btnUpdateDetail);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbOrderNo);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemName);
		this.groupBox3.Controls.Add(this.txtqty);
		this.groupBox3.Controls.Add(this.btnplus);
		this.groupBox3.Controls.Add(this.label38);
		this.groupBox3.Controls.Add(this.label35);
		this.groupBox3.Controls.Add(this.label39);
		this.groupBox3.Controls.Add(this.txtremarksdetail);
		this.groupBox3.Controls.Add(this.txtweight);
		this.groupBox3.Controls.Add(this.label47);
		this.groupBox3.Controls.Add(this.label45);
		this.groupBox3.Controls.Add(this.label37);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(3, 109);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(1039, 167);
		this.groupBox3.TabIndex = 2;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Detail";
		this.cmbContainerNoDetail.AllowNull = (DefaultableBoolean)1;
		this.cmbContainerNoDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbContainerNoDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbContainerNoDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbContainerNoDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.cmbContainerNoDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbContainerNoDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance41).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance41).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbContainerNoDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbContainerNoDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance42;
		((SpecialBoxBase)((UltraGridBase)this.cmbContainerNoDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance43).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance43).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbContainerNoDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.cmbContainerNoDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbContainerNoDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbContainerNoDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance46).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbContainerNoDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.cmbContainerNoDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbContainerNoDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbContainerNoDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance47;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance48).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbContainerNoDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.cmbContainerNoDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbContainerNoDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance49).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance49).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance49).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance49).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbContainerNoDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance50).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbContainerNoDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.cmbContainerNoDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbContainerNoDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance51).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbContainerNoDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.cmbContainerNoDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbContainerNoDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.cmbContainerNoDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbContainerNoDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbContainerNoDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbContainerNoDetail).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbContainerNoDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbContainerNoDetail).Location = new System.Drawing.Point(621, 89);
		((System.Windows.Forms.Control)(object)this.cmbContainerNoDetail).Name = "cmbContainerNoDetail";
		((System.Windows.Forms.Control)(object)this.cmbContainerNoDetail).Size = new System.Drawing.Size(134, 26);
		((System.Windows.Forms.Control)(object)this.cmbContainerNoDetail).TabIndex = 14;
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label22.Location = new System.Drawing.Point(621, 69);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(77, 15);
		this.label22.TabIndex = 49;
		this.label22.Text = "Container No";
		this.txtPackingWt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPackingWt.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtPackingWt.Location = new System.Drawing.Point(356, 91);
		this.txtPackingWt.Name = "txtPackingWt";
		this.txtPackingWt.Size = new System.Drawing.Size(72, 23);
		this.txtPackingWt.TabIndex = 11;
		this.txtPackingWt.Tag = "PackingWeight";
		this.txtPackingWt.TextChanged += new System.EventHandler(txtPackingWt_TextChanged);
		this.txtPackingWt.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtPackingWt_KeyPress);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label1.Location = new System.Drawing.Point(430, 69);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(83, 15);
		this.label1.TabIndex = 47;
		this.label1.Text = "T.Pack Weight";
		this.CmbCropYear.AllowNull = (DefaultableBoolean)1;
		this.CmbCropYear.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCropYear.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCropYear.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance150).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Appearance = (AppearanceBase)(object)appearance150;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance151).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance151).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance151).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance151;
		((AppearanceBase)appearance152).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance152;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance156).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance156).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance156).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance156;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance157).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance157;
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance159).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance160;
		((AppearanceBase)appearance161).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance161).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance161;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance162).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance162).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance162).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance162).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance162;
		((AppearanceBase)appearance163).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance163;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance164).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance165;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCropYear).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbCropYear.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Location = new System.Drawing.Point(759, 40);
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Name = "CmbCropYear";
		((System.Windows.Forms.Control)(object)this.CmbCropYear).Size = new System.Drawing.Size(94, 26);
		((System.Windows.Forms.Control)(object)this.CmbCropYear).TabIndex = 5;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label16.Location = new System.Drawing.Point(760, 20);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(54, 15);
		this.label16.TabIndex = 45;
		this.label16.Text = "CropYear";
		this.txtContainerRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtContainerRemarks.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtContainerRemarks.Location = new System.Drawing.Point(759, 91);
		this.txtContainerRemarks.Name = "txtContainerRemarks";
		this.txtContainerRemarks.Size = new System.Drawing.Size(277, 23);
		this.txtContainerRemarks.TabIndex = 15;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label10.Location = new System.Drawing.Point(759, 69);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(106, 15);
		this.label10.TabIndex = 43;
		this.label10.Text = "Container Remarks";
		this.txtInspectionRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtInspectionRemarks.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtInspectionRemarks.Location = new System.Drawing.Point(5, 135);
		this.txtInspectionRemarks.Name = "txtInspectionRemarks";
		this.txtInspectionRemarks.Size = new System.Drawing.Size(349, 23);
		this.txtInspectionRemarks.TabIndex = 16;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label9.Location = new System.Drawing.Point(5, 118);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(111, 15);
		this.label9.TabIndex = 41;
		this.label9.Text = "Inspection Remarks";
		this.txtGrossWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtGrossWeight.Enabled = false;
		this.txtGrossWeight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtGrossWeight.Location = new System.Drawing.Point(523, 91);
		this.txtGrossWeight.Name = "txtGrossWeight";
		this.txtGrossWeight.Size = new System.Drawing.Size(95, 23);
		this.txtGrossWeight.TabIndex = 13;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label7.Location = new System.Drawing.Point(523, 69);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(78, 15);
		this.label7.TabIndex = 39;
		this.label7.Text = "Gross Weight";
		this.txtTotalPackingWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTotalPackingWeight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtTotalPackingWeight.Location = new System.Drawing.Point(430, 91);
		this.txtTotalPackingWeight.Name = "txtTotalPackingWeight";
		this.txtTotalPackingWeight.Size = new System.Drawing.Size(91, 23);
		this.txtTotalPackingWeight.TabIndex = 12;
		this.txtTotalPackingWeight.Tag = "TotalPackingWeight";
		this.txtTotalPackingWeight.TextChanged += new System.EventHandler(txtPackingWeight_TextChanged);
		this.txtTotalPackingWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtPackingWeight_KeyPress);
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label6.Location = new System.Drawing.Point(356, 69);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(74, 15);
		this.label6.TabIndex = 37;
		this.label6.Text = "Pack Weight";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label4.Location = new System.Drawing.Point(5, 20);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(65, 15);
		this.label4.TabIndex = 27;
		this.label4.Text = "Invoice No";
		this.CmbJobLot.AllowNull = (DefaultableBoolean)1;
		this.CmbJobLot.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobLot.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobLot.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance123).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Appearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance166).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance166).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance166).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance166).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance166;
		((AppearanceBase)appearance167).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance167;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance168).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance168).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance168).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance128).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance128;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance129).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance3;
		((AppearanceBase)appearance4).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance4).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance5).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance5).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance5).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance5;
		((AppearanceBase)appearance6).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbJobLot.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Location = new System.Drawing.Point(591, 40);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Name = "CmbJobLot";
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Size = new System.Drawing.Size(165, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).TabIndex = 4;
		this.cmbInvoiceNo.AllowNull = (DefaultableBoolean)1;
		this.cmbInvoiceNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbInvoiceNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbInvoiceNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance81).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance81).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance81;
		((AppearanceBase)appearance92).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance92;
		((SpecialBoxBase)((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance103).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance103).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance103).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance114).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance114;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance136;
		((AppearanceBase)appearance147).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance147).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance147;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance158).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance158).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance158).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance158).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance158;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbInvoiceNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbInvoiceNo).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbInvoiceNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbInvoiceNo).Location = new System.Drawing.Point(5, 40);
		((System.Windows.Forms.Control)(object)this.cmbInvoiceNo).Name = "cmbInvoiceNo";
		((System.Windows.Forms.Control)(object)this.cmbInvoiceNo).Size = new System.Drawing.Size(118, 26);
		((System.Windows.Forms.Control)(object)this.cmbInvoiceNo).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbInvoiceNo).TextChanged += new System.EventHandler(cmbInvoiceNo_TextChanged);
		((System.Windows.Forms.Control)(object)this.cmbInvoiceNo).Leave += new System.EventHandler(cmbInvoiceNo_Leave);
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label8.Location = new System.Drawing.Point(591, 20);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(43, 15);
		this.label8.TabIndex = 35;
		this.label8.Text = "JobLot";
		this.CmbWareHouse.AllowNull = (DefaultableBoolean)1;
		this.CmbWareHouse.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWareHouse.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWareHouse.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Appearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance45).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance45).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWareHouse).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance56;
		((SpecialBoxBase)((UltraGridBase)this.CmbWareHouse).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance87).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance87).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance87).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance88).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance88;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance89).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance90;
		((AppearanceBase)appearance91).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance91).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance93).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance93).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance93).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance94).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance94;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance95).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWareHouse).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbWareHouse.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).Location = new System.Drawing.Point(423, 40);
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).Name = "CmbWareHouse";
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).Size = new System.Drawing.Size(165, 26);
		((System.Windows.Forms.Control)(object)this.CmbWareHouse).TabIndex = 3;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label3.Location = new System.Drawing.Point(423, 20);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(69, 15);
		this.label3.TabIndex = 33;
		this.label3.Text = "WareHouse";
		this.CmbSupplierCustomer.AllowNull = (DefaultableBoolean)1;
		this.CmbSupplierCustomer.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSupplierCustomer.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSupplierCustomer.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Appearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance54).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance54).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance54).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance54;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance55;
		((SpecialBoxBase)((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance57).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance57).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance58;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance59).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance60;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance61).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance62).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance62).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance62).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance62).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance62;
		((AppearanceBase)appearance63).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance64).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSupplierCustomer).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSupplierCustomer).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbSupplierCustomer.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSupplierCustomer).Location = new System.Drawing.Point(125, 40);
		((System.Windows.Forms.Control)(object)this.CmbSupplierCustomer).Name = "CmbSupplierCustomer";
		((System.Windows.Forms.Control)(object)this.CmbSupplierCustomer).Size = new System.Drawing.Size(194, 26);
		((System.Windows.Forms.Control)(object)this.CmbSupplierCustomer).TabIndex = 1;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label2.Location = new System.Drawing.Point(125, 20);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(93, 15);
		this.label2.TabIndex = 31;
		this.label2.Text = "Customer Name";
		this.btnCancelUpdateDetial.BackColor = System.Drawing.Color.Teal;
		this.btnCancelUpdateDetial.FlatAppearance.BorderSize = 0;
		this.btnCancelUpdateDetial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelUpdateDetial.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelUpdateDetial.ForeColor = System.Drawing.Color.White;
		this.btnCancelUpdateDetial.Location = new System.Drawing.Point(970, 133);
		this.btnCancelUpdateDetial.Name = "btnCancelUpdateDetial";
		this.btnCancelUpdateDetial.Size = new System.Drawing.Size(66, 26);
		this.btnCancelUpdateDetial.TabIndex = 20;
		this.btnCancelUpdateDetial.Text = "Cancel";
		this.btnCancelUpdateDetial.UseVisualStyleBackColor = false;
		this.btnCancelUpdateDetial.Visible = false;
		this.btnCancelUpdateDetial.Click += new System.EventHandler(btnCancelUpdateDetial_Click);
		this.CmbPackUom.AllowNull = (DefaultableBoolean)1;
		this.CmbPackUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance67).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance67).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance67).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance67;
		((AppearanceBase)appearance78).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance78;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance80).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance80).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance80).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance82;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance84;
		((AppearanceBase)appearance85).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance85).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance86).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance86).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance86).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance86;
		((AppearanceBase)appearance153).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance153;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance154).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance154;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbPackUom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Location = new System.Drawing.Point(5, 89);
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Name = "CmbPackUom";
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Size = new System.Drawing.Size(79, 26);
		((System.Windows.Forms.Control)(object)this.CmbPackUom).TabIndex = 7;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Leave += new System.EventHandler(combitempck_Leave);
		this.CmbPackingType.AllowNull = (DefaultableBoolean)1;
		this.CmbPackingType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackingType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackingType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance33).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbPackingType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Location = new System.Drawing.Point(86, 89);
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Name = "CmbPackingType";
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Size = new System.Drawing.Size(106, 26);
		((System.Windows.Forms.Control)(object)this.CmbPackingType).TabIndex = 8;
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label36.Location = new System.Drawing.Point(5, 69);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(63, 15);
		this.label36.TabIndex = 8;
		this.label36.Text = "Pack UOM";
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatAppearance.BorderSize = 0;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(897, 133);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(70, 26);
		this.btnUpdateDetail.TabIndex = 19;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Visible = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.CmbOrderNo.AllowNull = (DefaultableBoolean)1;
		this.CmbOrderNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbOrderNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbOrderNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance137).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance137;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance138).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance138).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance138).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbOrderNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance138;
		((AppearanceBase)appearance139).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance139;
		((SpecialBoxBase)((UltraGridBase)this.CmbOrderNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance140).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance140).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance140).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance141).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance141;
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance142).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance143;
		((AppearanceBase)appearance144).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance144).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance145).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance145).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance145).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance145).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance145;
		((AppearanceBase)appearance146).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance148).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance148;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance149;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbOrderNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbOrderNo).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbOrderNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbOrderNo).Location = new System.Drawing.Point(322, 40);
		((System.Windows.Forms.Control)(object)this.CmbOrderNo).Name = "CmbOrderNo";
		((System.Windows.Forms.Control)(object)this.CmbOrderNo).Size = new System.Drawing.Size(98, 26);
		((System.Windows.Forms.Control)(object)this.CmbOrderNo).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbOrderNo).TextChanged += new System.EventHandler(CmbOrderNo_TextChanged);
		this.CmbItemName.AllowNull = (DefaultableBoolean)1;
		this.CmbItemName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance97).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.CmbItemName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance98).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance98).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance98).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance98;
		((AppearanceBase)appearance99).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance99;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance113).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance113).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance113).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance115).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance116).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance117;
		((AppearanceBase)appearance118).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance118).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance119).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance119).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance119).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance119;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbItemName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Location = new System.Drawing.Point(856, 40);
		((System.Windows.Forms.Control)(object)this.CmbItemName).Name = "CmbItemName";
		((System.Windows.Forms.Control)(object)this.CmbItemName).Size = new System.Drawing.Size(180, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemName).TabIndex = 6;
		((System.Windows.Forms.Control)(object)this.CmbItemName).Leave += new System.EventHandler(CmbItemName_TextChanged);
		this.txtqty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtqty.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtqty.Location = new System.Drawing.Point(195, 91);
		this.txtqty.Name = "txtqty";
		this.txtqty.Size = new System.Drawing.Size(67, 23);
		this.txtqty.TabIndex = 9;
		this.txtqty.TextChanged += new System.EventHandler(txtqty_TextChanged);
		this.txtqty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtqty_KeyPress);
		this.btnplus.BackColor = System.Drawing.Color.Teal;
		this.btnplus.FlatAppearance.BorderSize = 0;
		this.btnplus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnplus.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnplus.ForeColor = System.Drawing.Color.White;
		this.btnplus.Location = new System.Drawing.Point(855, 133);
		this.btnplus.Name = "btnplus";
		this.btnplus.Size = new System.Drawing.Size(39, 26);
		this.btnplus.TabIndex = 18;
		this.btnplus.Text = "+";
		this.btnplus.UseVisualStyleBackColor = false;
		this.btnplus.Click += new System.EventHandler(btnplus_Click);
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label38.Location = new System.Drawing.Point(195, 69);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(26, 15);
		this.label38.TabIndex = 13;
		this.label38.Text = "Qty";
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label35.Location = new System.Drawing.Point(86, 69);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(74, 15);
		this.label35.TabIndex = 25;
		this.label35.Text = "PackingType";
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label39.Location = new System.Drawing.Point(264, 69);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(46, 15);
		this.label39.TabIndex = 15;
		this.label39.Text = "Weight";
		this.txtremarksdetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtremarksdetail.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtremarksdetail.Location = new System.Drawing.Point(356, 135);
		this.txtremarksdetail.Name = "txtremarksdetail";
		this.txtremarksdetail.Size = new System.Drawing.Size(497, 23);
		this.txtremarksdetail.TabIndex = 17;
		this.txtweight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtweight.Enabled = false;
		this.txtweight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtweight.Location = new System.Drawing.Point(264, 91);
		this.txtweight.Name = "txtweight";
		this.txtweight.Size = new System.Drawing.Size(90, 23);
		this.txtweight.TabIndex = 10;
		this.txtweight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtweight_KeyPress);
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label47.Location = new System.Drawing.Point(356, 118);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(65, 15);
		this.label47.TabIndex = 25;
		this.label47.Text = "Comments";
		this.label45.AutoSize = true;
		this.label45.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label45.Location = new System.Drawing.Point(321, 20);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(96, 15);
		this.label45.TabIndex = 23;
		this.label45.Text = "Sale Contract No";
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label37.Location = new System.Drawing.Point(856, 20);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(67, 15);
		this.label37.TabIndex = 9;
		this.label37.Text = "Item Name";
		this.panel3.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel3.Controls.Add(this.label23);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(2, 27);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1272, 29);
		this.panel3.TabIndex = 1;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(8, 5);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(186, 19);
		this.label23.TabIndex = 0;
		this.label23.Text = "Delivery Order Export";
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(2, 2);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1272, 25);
		this.panel2.TabIndex = 0;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[7] { this.btnnew, this.btnRefresh, this.btnupdate, this.btnsave, this.btnprint, this.btnattachment, this.btnShortCutKeys });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1272, 27);
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
		this.btnprint.Text = "262-Print";
		this.btnprint.Visible = false;
		this.btnprint.Click += new System.EventHandler(Print_Click);
		this.btnattachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnattachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnattachment.Name = "btnattachment";
		this.btnattachment.Size = new System.Drawing.Size(109, 24);
		this.btnattachment.Text = "&Attachment";
		this.btnattachment.Click += new System.EventHandler(btnattachment_Click);
		this.btnShortCutKeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortCutKeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKeys.Name = "btnShortCutKeys";
		this.btnShortCutKeys.Size = new System.Drawing.Size(106, 24);
		this.btnShortCutKeys.Text = "ShortCut Keys";
		this.btnShortCutKeys.Click += new System.EventHandler(btnShortCutKeys_Click);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.grdhistory);
		this.tabPage2.Controls.Add(this.panel9);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.tabPage2.Controls.Add(this.panel8);
		this.tabPage2.Controls.Add(this.panel7);
		this.tabPage2.Controls.Add(this.panel5);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage2.Size = new System.Drawing.Size(1276, 629);
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
		((System.Windows.Forms.Control)(object)this.grdhistory).Location = new System.Drawing.Point(2, 150);
		((System.Windows.Forms.Control)(object)this.grdhistory).Name = "grdhistory";
		((ScrollableBase)this.grdhistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdhistory).Size = new System.Drawing.Size(1272, 236);
		((System.Windows.Forms.Control)(object)this.grdhistory).TabIndex = 1;
		this.grdhistory.TotalRow = (InheritableBoolean)1;
		this.grdhistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdhistory).VisualStyle = (VisualStyle)4;
		this.grdhistory.ColumnButtonClick += new ColumnActionEventHandler(grdhistory_ColumnButtonClick);
		this.grdhistory.SelectionChanged += new System.EventHandler(grdhistory_SelectionChanged);
		this.grdhistory.LinkClicked += new ColumnActionEventHandler(grdhistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdhistory).DoubleClick += new System.EventHandler(grdhistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdhistory).KeyDown += new System.Windows.Forms.KeyEventHandler(grdhistory_KeyDown);
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.label18);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel9.Location = new System.Drawing.Point(2, 386);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1272, 20);
		this.panel9.TabIndex = 51;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label18.ForeColor = System.Drawing.Color.White;
		this.label18.Location = new System.Drawing.Point(6, 2);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(228, 17);
		this.label18.TabIndex = 23;
		this.label18.Text = "Detail of the selected above row";
		this.grdDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdDetail).BackColor = System.Drawing.Color.White;
		grdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Bottom;
		this.grdDetail.DynamicFiltering = true;
		this.grdDetail.FilterMode = (FilterMode)2;
		this.grdDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(2, 406);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1272, 221);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 2;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.label17);
		this.panel8.Controls.Add(this.ctrlGrdBar2);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(2, 127);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1272, 23);
		this.panel8.TabIndex = 48;
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.ForeColor = System.Drawing.Color.White;
		this.label17.Location = new System.Drawing.Point(3, 2);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(142, 19);
		this.label17.TabIndex = 22;
		this.label17.Text = "Filtered Records";
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1230, 0);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdhistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(40, 21);
		this.ctrlGrdBar2.TabIndex = 21;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.panel7.BackColor = System.Drawing.Color.White;
		this.panel7.Controls.Add(this.groupBox5);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(2, 53);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1272, 74);
		this.panel7.TabIndex = 0;
		this.groupBox5.Controls.Add(this.label53);
		this.groupBox5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbInvoiceNoHistory);
		this.groupBox5.Controls.Add(this.rdapproveddate);
		this.groupBox5.Controls.Add(this.btnShowHistory);
		this.groupBox5.Controls.Add(this.rdmodifydate);
		this.groupBox5.Controls.Add(this.label34);
		this.groupBox5.Controls.Add(this.drdocdate);
		this.groupBox5.Controls.Add(this.rdentrydate);
		this.groupBox5.Controls.Add(this.label73);
		this.groupBox5.Controls.Add(this.label89);
		this.groupBox5.Controls.Add(this.label90);
		this.groupBox5.Controls.Add(this.label91);
		this.groupBox5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomerHistory);
		this.groupBox5.Controls.Add(this.ToDocNoHistory);
		this.groupBox5.Controls.Add(this.FromDocNoHistory);
		this.groupBox5.Controls.Add(this.ToDateHistory);
		this.groupBox5.Controls.Add(this.FromDateHistory);
		this.groupBox5.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox5.Location = new System.Drawing.Point(3, 1);
		this.groupBox5.Name = "groupBox5";
		this.groupBox5.Size = new System.Drawing.Size(923, 71);
		this.groupBox5.TabIndex = 0;
		this.groupBox5.TabStop = false;
		this.groupBox5.Text = "Filters";
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label53.Location = new System.Drawing.Point(374, 47);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(46, 15);
		this.label53.TabIndex = 91;
		this.label53.Text = "Invoice";
		this.CmbInvoiceNoHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbInvoiceNoHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbInvoiceNoHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbInvoiceNoHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance120).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance121).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance121).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance121).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance121;
		((AppearanceBase)appearance122).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance122;
		((SpecialBoxBase)((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance124).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance124).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance124).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance126).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance126;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance127).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance130;
		((AppearanceBase)appearance131).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance131).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance132).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance132).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance132).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance132).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance132;
		((AppearanceBase)appearance133).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance134;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbInvoiceNoHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNoHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbInvoiceNoHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNoHistory).Location = new System.Drawing.Point(434, 41);
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNoHistory).Name = "CmbInvoiceNoHistory";
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNoHistory).Size = new System.Drawing.Size(238, 26);
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNoHistory).TabIndex = 5;
		this.rdapproveddate.AutoSize = true;
		this.rdapproveddate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdapproveddate.Location = new System.Drawing.Point(816, 44);
		this.rdapproveddate.Name = "rdapproveddate";
		this.rdapproveddate.Size = new System.Drawing.Size(105, 19);
		this.rdapproveddate.TabIndex = 10;
		this.rdapproveddate.Text = "Approved Date";
		this.rdapproveddate.UseVisualStyleBackColor = true;
		this.btnShowHistory.BackColor = System.Drawing.Color.Teal;
		this.btnShowHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShowHistory.ForeColor = System.Drawing.Color.White;
		this.btnShowHistory.Location = new System.Drawing.Point(668, 41);
		this.btnShowHistory.Margin = new System.Windows.Forms.Padding(2);
		this.btnShowHistory.Name = "btnShowHistory";
		this.btnShowHistory.Size = new System.Drawing.Size(62, 26);
		this.btnShowHistory.TabIndex = 6;
		this.btnShowHistory.Text = "Show";
		this.btnShowHistory.UseVisualStyleBackColor = false;
		this.btnShowHistory.Click += new System.EventHandler(btnShowHistory_Click);
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(816, 18);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 9;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.Location = new System.Drawing.Point(374, 20);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(58, 15);
		this.label34.TabIndex = 87;
		this.label34.Text = "Customer";
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(735, 18);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 7;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(735, 44);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 8;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.label73.AutoSize = true;
		this.label73.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label73.Location = new System.Drawing.Point(213, 47);
		this.label73.Name = "label73";
		this.label73.Size = new System.Drawing.Size(64, 15);
		this.label73.TabIndex = 86;
		this.label73.Text = "To Doc No";
		this.label89.AutoSize = true;
		this.label89.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label89.Location = new System.Drawing.Point(213, 20);
		this.label89.Name = "label89";
		this.label89.Size = new System.Drawing.Size(79, 15);
		this.label89.TabIndex = 85;
		this.label89.Text = "From Doc No";
		this.label90.AutoSize = true;
		this.label90.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label90.Location = new System.Drawing.Point(3, 47);
		this.label90.Name = "label90";
		this.label90.Size = new System.Drawing.Size(48, 15);
		this.label90.TabIndex = 84;
		this.label90.Text = "To Date";
		this.label91.AutoSize = true;
		this.label91.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label91.Location = new System.Drawing.Point(3, 20);
		this.label91.Name = "label91";
		this.label91.Size = new System.Drawing.Size(63, 15);
		this.label91.TabIndex = 83;
		this.label91.Text = "From Date";
		this.CmbCustomerHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomerHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomerHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomerHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomerHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Location = new System.Drawing.Point(434, 14);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Name = "CmbCustomerHistory";
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Size = new System.Drawing.Size(296, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).TabIndex = 4;
		this.ToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.ToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDocNoHistory.Location = new System.Drawing.Point(294, 43);
		this.ToDocNoHistory.Name = "ToDocNoHistory";
		this.ToDocNoHistory.ShortcutsEnabled = false;
		this.ToDocNoHistory.Size = new System.Drawing.Size(78, 23);
		this.ToDocNoHistory.TabIndex = 3;
		this.ToDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(ToDocNoHistory_KeyPress);
		this.FromDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.FromDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDocNoHistory.Location = new System.Drawing.Point(294, 16);
		this.FromDocNoHistory.Name = "FromDocNoHistory";
		this.FromDocNoHistory.ShortcutsEnabled = false;
		this.FromDocNoHistory.Size = new System.Drawing.Size(78, 23);
		this.FromDocNoHistory.TabIndex = 2;
		this.FromDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(FromDocNoHistory_KeyPress);
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(69, 43);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(142, 23);
		this.ToDateHistory.TabIndex = 1;
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(69, 16);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(142, 23);
		this.FromDateHistory.TabIndex = 0;
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.label13);
		this.panel5.Controls.Add(this.toolStrip2);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(2, 2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1272, 51);
		this.panel5.TabIndex = 46;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.ForeColor = System.Drawing.Color.White;
		this.label13.Location = new System.Drawing.Point(3, 29);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(249, 19);
		this.label13.TabIndex = 22;
		this.label13.Text = "Delivery Order Export History";
		this.toolStrip2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnResetHistory, this.btnRefreshHistory });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1272, 27);
		this.toolStrip2.TabIndex = 1;
		this.toolStrip2.Text = "toolStrip2";
		this.btnResetHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnResetHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnResetHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnResetHistory.Name = "btnResetHistory";
		this.btnResetHistory.Size = new System.Drawing.Size(59, 24);
		this.btnResetHistory.Text = "&New";
		this.btnResetHistory.Click += new System.EventHandler(btnResetHistory_Click);
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
		base.ClientSize = new System.Drawing.Size(1284, 658);
		base.Controls.Add(this.panel1);
		base.HelpButton = true;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "ExportDeliveryOrderNew";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(ExportDeliveryOrderNew_FormClosing);
		base.Load += new System.EventHandler(PurchsaeOrder_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(PurchsaeOrder_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.groupBox4.ResumeLayout(false);
		this.groupBox4.PerformLayout();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbtransporter).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBranch).EndInit();
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbContainerNoDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropYear).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbInvoiceNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWareHouse).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSupplierCustomer).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackingType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbOrderNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemName).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdhistory).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.panel7.ResumeLayout(false);
		this.groupBox5.ResumeLayout(false);
		this.groupBox5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbInvoiceNoHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
