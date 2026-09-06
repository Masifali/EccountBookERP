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
using Architecture.Model.Authentication;
using Architecture.Model.Inventory;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Contractor_Wages;
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
namespace Architecture.WinApp.Sale
{

public class frmSaleInvoiceDirect : Form
{
	private DataTable dtGrdGL = new DataTable();

	private DataTable dtInvExp = new DataTable();

	private DataTable dtGrid = new DataTable();

	private DataTable dtSupplier = new DataTable();

	private DataTable dtAccountlst = new DataTable();

	private DataTable dtOtherItem = new DataTable();

	private DataTable dtitem = new DataTable();

	private DataTable dtDiscountType = new DataTable();

	private DataTable dtPackType = new DataTable();

	private DataTable dtuomGetAll = new DataTable();

	private DataTable dtRateUom = new DataTable();

	private DataTable dtCity = new DataTable();

	private DropDownBind DDL = new DropDownBind();

	private Attachment AT = new Attachment();

	public DataTable dtViewAttachments = new DataTable();

	public DataTable dtAttachmentsUpload = new DataTable();

	private Rightsobjects formright = new Rightsobjects();

	public static int SupplierIdMain = 0;

	public static string FlagLoader = "";

	private bool WagesStatus;

	private bool IsFromLoader;

	private double SupplierGLCreditAmount;

	private double SupplierGLDebitAmount;

	private double SupplierCommissionAmount;

	private int updateDetailIndex;

	private int VoucherHeadId;

	private int RecId;

	private int getJobLotId;

	private bool Approved;

	private bool GetItemagainstwarehouseId;

	private string ScreenName = "";

	private string AttachmentsValues = "";

	private string CustomAttachmentsValues = "";

	private UserAccount UserAccount = new UserAccount();

	private LoadavailableTransactionsForIssuance LoadTransactions;

	private frmLoadGDN LoadGDN;

	private IContainer components;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel1;

	private Panel panel3;

	private ToolStrip toolStrip1;

	private ToolStripButton btnNew;

	private ToolStripButton btnUpdate;

	private ToolStripButton btnSave;

	private ToolStripButton btnPrint;

	private ToolStripButton btnAttachment;

	private TabPage tabPage2;

	private GroupBox groupBox1;

	private TextBox txtAmount;

	private Label label14;

	private Label label9;

	private Label label3;

	private Label label42;

	private Label label41;

	private TextBox txtRate;

	private Label label40;

	private TextBox txtQty;

	private Label label38;

	private Label label35;

	private TextBox txtNetBillWeight;

	private Label label36;

	private Label label32;

	private Label label31;

	private Label label28;

	private Label label22;

	private DateTimePicker txtgpdate;

	internal UltraCombo comstockparty;

	internal UltraCombo compartyreference;

	internal UltraCombo comPackUOM;

	internal UltraCombo comCropYear;

	internal UltraCombo comPackingType;

	internal UltraCombo comItem;

	internal UltraCombo comWarehouse;

	internal UltraCombo comRateUOM;

	private Label label27;

	private DateTimePicker deliverystartdate;

	private DateTimePicker validate;

	private Label label44;

	private Label label45;

	private TextBox txtdeliverydays;

	private Label label47;

	private Label label48;

	private TextBox txtvehicleno;

	private Button btnAdd;

	private TextBox txtgatepassno;

	internal UltraCombo comborderno;

	private Panel panelDetail;

	private Panel panel6;

	private Panel panel8;

	internal GridEX grdInvExp;

	internal GridEX grdGLedger;

	internal GridEX grd;

	private TextBox txtCommAgentId;

	private TextBox txtSupplierGLId;

	private CtrlGrdBar ctrlGrdBar1;

	private Button btnCancelUpdateDetial;

	private Button btnUpdateDetail;

	private ToolStripButton btnSlip;

	private CheckBox ChkBok;

	private Label label51;

	private TextBox txtBillAmount;

	private TextBox txtremarks;

	private Label label24;

	private Label label4;

	private TextBox txtduedays;

	private Label label10;

	private DateTimePicker duedate;

	private Label label29;

	internal UltraCombo comsupplier;

	private Label label7;

	private Label label6;

	private TextBox txtbillno;

	internal UltraCombo comproject;

	internal UltraCombo combranches;

	private Label label13;

	private Label label5;

	private Label label18;

	private DateTimePicker DocDate;

	private TextBox txtdocno;

	private ToolStripButton btnDelete;

	internal UltraCombo combpttrm;

	internal UltraCombo combdeliverytrm;

	private Panel panel2;

	private ToolStrip toolStrip2;

	private ToolStripButton btnRefresh;

	private CtrlGrdBar ctrlGrdBar2;

	private Panel panel10;

	internal GridEX grdDetail;

	private CheckBox ChkPrintslip;

	private CheckBox ChkResetOnSave;

	private Label lblBalance;

	private ToolStripButton btnLoadGdn;

	private ToolStripButton btn294APrint;

	internal UltraCombo txtCity;

	private Label label8;

	private Panel panel11;

	private GroupBox groupBox2;

	private Label label17;

	private Panel panel16;

	private Panel panel14;

	private Panel panel15;

	private Panel panel12;

	private Label label23;

	private Label label53;

	private Label label37;

	private CtrlGrdBar ctrlGrdBar3;

	private RadioButton rdbtnItemCode;

	private RadioButton rdbtnItemName;

	private Label label1;

	private ToolStripButton btnLoadSaleOrder;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripSeparator toolStripSeparator3;

	private ToolStripSeparator toolStripSeparator4;

	private ToolStripSeparator toolStripSeparator5;

	private ToolStripSeparator toolStripSeparator6;

	private ToolStripButton btnShortcutKeys;

	private ToolStripSeparator toolStripSeparator1;

	private Panel panel18;

	private Label label55;

	internal GridEX grdHistory;

	private Panel panel4;

	private Label label20;

	private Panel panel9;

	private Label label54;

	private GroupBox groupBox4;

	private Button btnshow;

	private Label label73;

	internal UltraCombo CmbCustomerHistory;

	private Label label75;

	internal UltraCombo cmbDateTypeHistory;

	private DateTimePicker txtToDateHistory;

	private Label label76;

	private TextBox txtToDocNoHistory;

	private DateTimePicker txtFromdateHistory;

	private Label label77;

	private TextBox txtFromNoHistory;

	private ToolStripButton BtnNewHistory;

	private ToolStripButton BtnRefreshHistory;

	private RadioButton rdapproveddate;

	private RadioButton rdmodifydate;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	private Label label2;

	public frmSaleInvoiceDirect(UserAccount user)
	{
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void frmSaleInvoiceDirectFlourAutoRated_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(10, 10);
			base.AutoScrollMinSize = new Size(1000, 650);
			ConfigurationDefault();
			dtViewAttachments.Columns.Add("AttchmentPath");
			ScreenName = base.Name;
			formright = CommonServices.SetRightsValueInRightsObject(ScreenName);
			btnSave.Enabled = formright.DoHaveSaveRight;
			btnPrint.Enabled = formright.DoHavePrintRights;
			btnSlip.Enabled = formright.DoHavePrintRights;
			btn294APrint.Enabled = formright.DoHavePrintRights;
			btnUpdate.Enabled = formright.DoHaveUpdateRights;
			btnDelete.Enabled = formright.DoHaveCanDelete;
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
			ctrlGrdBar3.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar3.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar3.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar3.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar3.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar3.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			AccountsFill();
			OtherItemsBind();
			dtRateUom.Columns.Add("RateUomId", typeof(int));
			dtRateUom.Columns.Add("RateUOM", typeof(string));
			dtRateUom.Columns.Add("Equivalent", typeof(double));
			dtitem.Columns.Add("Id", typeof(int));
			dtitem.Columns.Add("ItemName", typeof(string));
			dtitem.Columns.Add("ItemCode", typeof(string));
			dtGrid.Columns.Add("Id", typeof(int));
			dtGrid.Columns.Add("RefDocumentTypeId", typeof(int));
			dtGrid.Columns.Add("RefDocIdNo", typeof(int));
			dtGrid.Columns.Add("RefDocSubIdNo", typeof(int));
			dtGrid.Columns.Add("SaleOrderId", typeof(int));
			dtGrid.Columns.Add("SaleOrderDetailId", typeof(int));
			dtGrid.Columns.Add("SaleOrderNo", typeof(int));
			dtGrid.Columns.Add("InvGdnId", typeof(int));
			dtGrid.Columns.Add("InvGdnDetailId", typeof(int));
			dtGrid.Columns.Add("GdnNo", typeof(int));
			dtGrid.Columns.Add("WarehouseId", typeof(int));
			dtGrid.Columns.Add("Warehouse", typeof(string));
			dtGrid.Columns.Add("ItemId", typeof(int));
			dtGrid.Columns.Add("Item", typeof(string));
			dtGrid.Columns.Add("ItemCode", typeof(string));
			dtGrid.Columns.Add("CropYear", typeof(string));
			dtGrid.Columns.Add("PackingTypeId", typeof(int));
			dtGrid.Columns.Add("PackingType", typeof(string));
			dtGrid.Columns.Add("PackUOMId", typeof(int));
			dtGrid.Columns.Add("PackUOMEquivalent", typeof(double));
			dtGrid.Columns.Add("ItemQty", typeof(double));
			dtGrid.Columns.Add("BalQty", typeof(double));
			dtGrid.Columns.Add("NetBillWeight", typeof(double));
			dtGrid.Columns.Add("BalWeight", typeof(double));
			dtGrid.Columns.Add("Rate", typeof(double));
			dtGrid.Columns.Add("CgsRate", typeof(double));
			dtGrid.Columns.Add("RateUOMId", typeof(int));
			dtGrid.Columns.Add("RateEquivalent", typeof(double));
			dtGrid.Columns.Add("ItemAmount", typeof(double));
			dtGrid.Columns.Add("GpDate", typeof(DateTime));
			dtGrid.Columns.Add("GpNo", typeof(int));
			dtGrid.Columns.Add("VehicleNo", typeof(string));
			dtGrid.Columns.Add("BillAmount", typeof(double));
			dtGrid.Columns.Add("Expense", typeof(double));
			dtGrid.Columns.Add("Journal", typeof(double));
			dtGrid.Columns.Add("CityId", typeof(int));
			dtGrid.Columns.Add("CityName", typeof(string));
			grd.DataSource = dtGrid;
			grd.RetrieveStructure();
			grdSettings();
			dtInvExp.Columns.Add("ItemId", typeof(int));
			dtInvExp.Columns.Add("Qty", typeof(double));
			dtInvExp.Columns.Add("Rate", typeof(double));
			dtInvExp.Columns.Add("Amount", typeof(double));
			dtInvExp.Columns.Add("Remarks");
			AddRowInvExpGrid();
			grdInvExp.RetrieveStructure();
			grdInvExpSettings();
			dtGrdGL.Columns.Add("AccountId", typeof(int));
			dtGrdGL.Columns.Add("Remarks");
			dtGrdGL.Columns.Add("Percentage", typeof(double));
			dtGrdGL.Columns.Add("Qty", typeof(double));
			dtGrdGL.Columns.Add("Rate", typeof(double));
			dtGrdGL.Columns.Add("Debit", typeof(double));
			dtGrdGL.Columns.Add("Credit", typeof(double));
			AddRowInGLGrid();
			grdGLedger.RetrieveStructure();
			gridGLSettings();
			bindWareHouse();
			BinCity();
			suppliercustomer();
			PaymentTerms();
			DeliveryTerm();
			ItemNameFill();
			Project();
			PackingType();
			braches();
			cropyear();
			DocumentNo();
			((UltraGridBase)comproject).Rows[0].Activate();
			((UltraGridBase)combranches).Rows[0].Activate();
			DocDate.Focus();
			ChkPrintslip.Checked = true;
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			btnDelete.Visible = false;
			btnAdd.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			txtduedays.Enabled = true;
			if (Conversion.ToInt(combpttrm.Value) == 1)
			{
				txtduedays.Text = "";
				txtduedays.Enabled = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void bindWareHouse()
	{
		try
		{
			int id = Conversion.ToInt(comWarehouse.Value);
			DataTable dt = CommonServices.getActiveWareHouse();
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDL(dt, comWarehouse, "Id", "WareHouseName", "Ware House ", ZeroIndex: true);
			if (id > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
				{
					comWarehouse.Value = id;
				}
				else
				{
					((Control)(object)comWarehouse).Text = string.Empty;
				}
			}
		}
		catch (Exception)
		{
			MessageBox.Show("WareHouse Not Found");
		}
	}

	private void BinCity()
	{
		try
		{
			int id = Conversion.ToInt(txtCity.Value);
			dtCity = CommonServices.CityGetAllService();
			if (dtCity.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDLNew(dtCity, txtCity, "Id", "CityName", "City Name ", true);
			if (id > 0)
			{
				if (dtCity.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
				{
					txtCity.Value = id;
				}
				else
				{
					((Control)(object)txtCity).Text = string.Empty;
				}
			}
		}
		catch (Exception)
		{
			MessageBox.Show("City Not Found");
		}
	}

	private void suppliercustomer()
	{
		try
		{
			int id = Conversion.ToInt(comsupplier.Value);
			dtSupplier = CommonServices.SupplierCustomerGetAllServiceBind();
			if (dtSupplier.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDLNew(dtSupplier, comsupplier, "Id", "CompanyName", "Customer Name ", true);
			((UltraGridBase)comsupplier).DisplayLayout.Bands[0].Columns[1].Width = 300;
			if (id > 0)
			{
				if (dtSupplier.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
				{
					comsupplier.Value = id;
				}
				else
				{
					((Control)(object)comsupplier).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Project()
	{
		try
		{
			DataTable dtproject = CommonServices.ProjectServiceBind();
			if (dtproject.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtproject, comproject, "Id", "ProjectName", "Project Name", false);
				((UltraGridBase)comproject).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void braches()
	{
		try
		{
			DataTable BranchList = CommonServices.BrancheServiceBind();
			if (BranchList.Rows.Count > 0)
			{
				DDL.BindDDLNew(BranchList, combranches, "Id", "BranchName", "Branch Name", false);
				((UltraGridBase)combranches).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PaymentTerms()
	{
		try
		{
			int id = Conversion.ToInt(combpttrm.Value);
			DataTable dtTerm = CommonServices.GetDueTermServiceBind();
			if (dtTerm.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDLNew(dtTerm, combpttrm, "Id", "TermsDescription", "Payment Term", true);
			((UltraGridBase)combpttrm).Rows[1].Activate();
			if (id > 0)
			{
				if (dtTerm.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
				{
					combpttrm.Value = id;
				}
				else
				{
					((Control)(object)combpttrm).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeliveryTerm()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("DeliveryTerm");
			dt.Rows.Add(1, "Load");
			dt.Rows.Add(2, "Ponch");
			DDL.BindDDL(dt, combdeliverytrm, "Id", "DeliveryTerm", "Delivery Term", ZeroIndex: false);
			((UltraGridBase)combdeliverytrm).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PackingType()
	{
		try
		{
			int id = Conversion.ToInt(comItem.Value);
			dtPackType.Clear();
			dtPackType = InvPackingType.Getall();
			if (dtPackType.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDLNew(dtPackType, comPackingType, "Id", "PackTypeDesc", "Pack Type ", true);
			if (id > 0)
			{
				if (dtPackType.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
				{
					comPackingType.Value = id;
				}
				else
				{
					((Control)(object)comPackingType).Text = string.Empty;
				}
			}
			else if (dtPackType.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == 2))
			{
				comPackingType.Value = 2;
			}
			else
			{
				((Control)(object)comPackingType).Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AccountsFill()
	{
		try
		{
			dtAccountlst = CommonServices.CoaAllocationGetAllServiceBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PackUOM()
	{
		try
		{
			string Packuom = ((Control)(object)comPackUOM).Text;
			string Rateuom = ((Control)(object)comRateUOM).Text;
			((Control)(object)comPackUOM).Text = string.Empty;
			((Control)(object)comRateUOM).Text = string.Empty;
			dtuomGetAll = CommonServices.GetUomScheduleByItemId(Conversion.ToInt(comItem.Value));
			if (dtuomGetAll.Rows.Count > 0)
			{
				DDL.BindDDL(dtuomGetAll, comPackUOM, "Id", "Equivalent", "PackUOM", ZeroIndex: false);
				DDL.BindDDL(dtuomGetAll, comRateUOM, "Id", "Equivalent", "RateUOM", ZeroIndex: false);
				((UltraGridBase)comPackUOM).DisplayLayout.Bands[0].Columns[1].Width = 70;
				((UltraGridBase)comPackUOM).DisplayLayout.Bands[0].Columns[2].Width = 85;
				((UltraGridBase)comRateUOM).DisplayLayout.Bands[0].Columns[1].Width = 70;
				((UltraGridBase)comRateUOM).DisplayLayout.Bands[0].Columns[1].Width = 85;
				if (dtuomGetAll.AsEnumerable().Any((DataRow row) => Conversion.ToString((object)row.Field<double>("Equivalent")) == Packuom))
				{
					((Control)(object)comPackUOM).Text = Packuom;
				}
				else
				{
					((Control)(object)comPackUOM).Text = string.Empty;
				}
				if (dtuomGetAll.AsEnumerable().Any((DataRow row) => Conversion.ToString((object)row.Field<double>("Equivalent")) == Rateuom))
				{
					((Control)(object)comRateUOM).Text = Rateuom;
				}
				else
				{
					((Control)(object)comRateUOM).Text = string.Empty;
				}
			}
			else
			{
				((Control)(object)comPackUOM).Text = string.Empty;
				((Control)(object)comRateUOM).Text = string.Empty;
				((UltraGridBase)comPackUOM).DataSource = null;
				((UltraGridBase)comRateUOM).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cropyear()
	{
		try
		{
			int id = Conversion.ToInt(comCropYear.Value);
			DataTable lst = CommonServices.CropYearGetAllService();
			if (lst.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDLNew(lst, comCropYear, "Id", "CropYear", "Crop Year", true);
			if (id > 0)
			{
				if (lst.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
				{
					comCropYear.Value = id;
				}
				else
				{
					((Control)(object)comCropYear).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DocumentNo()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		try
		{
			int Code = InvSaleInvoice.GenerateInvSaleInvoiceCode(new InvSaleInvoice
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = 186,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			if (Code > 0)
			{
				txtdocno.Text = Code.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemNameFill()
	{
		try
		{
			int id = Conversion.ToInt(comWarehouse.Value);
			DataTable dt = CommonServices.ItemGetAllServiceBind();
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtitem.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ItemName"], dt.Rows[i]["ItemCode"]);
				}
			}
			if (dtitem.Rows.Count <= 0)
			{
				return;
			}
			if (rdbtnItemName.Checked)
			{
				DDL.BindDDL(dtitem, comItem, "Id", "ItemName", "Item Name", ZeroIndex: true);
			}
			else
			{
				DDL.BindDDL(dtitem, comItem, "Id", "ItemCode", "Item Name", ZeroIndex: true);
			}
			((UltraGridBase)comItem).DisplayLayout.Bands[0].Columns[1].Width = 300;
			if (id > 0)
			{
				if (dtitem.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
				{
					comItem.Value = id;
				}
				else
				{
					((Control)(object)comItem).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void VoucherHeadIdGet()
	{
		try
		{
			VoucherHeadId = CommonServices.VoucherHeadIdGet(RecId, 186);
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
				if (Conversion.ToInt(comItem.Value) > 0)
				{
					ItemId = Conversion.ToInt(comItem.Value);
				}
				if (rdbtnItemName.Checked)
				{
					DDL.BindDDL(dtitem, comItem, "Id", "ItemName", "Item Name", ZeroIndex: true);
				}
				else
				{
					DDL.BindDDL(dtitem, comItem, "Id", "ItemCode", "Item Name", ZeroIndex: true);
				}
				comItem.Value = ItemId;
				comItem.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AvailableStockGetByItem()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Expected O, but got Unknown
		try
		{
			double CurrStock = InventoryStockEvalautionDetail.GetCurrentStockByItemId(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				WarehouseId = Conversion.ToInt(comWarehouse.Value),
				ItemId = Conversion.ToInt(comItem.Value),
				CropYear = Conversion.ToString((object)((Control)(object)comCropYear).Text.Trim()),
				DocDate = Conversion.ToDateTime((object)DocDate.Value),
				PackingTypeId = Conversion.ToInt(comPackingType.Value),
				stockUOM = Conversion.ToInt(comPackUOM.Value)
			});
			if (CurrStock > 0.0)
			{
				lblBalance.Text = CurrStock.ToString();
			}
			else
			{
				lblBalance.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void comItem_Leave(object sender, EventArgs e)
	{
		PackUOM();
		AvailableStockGetByItem();
	}

	private void comborderno_Leave(object sender, EventArgs e)
	{
		try
		{
			if (comborderno.Value != null && Conversion.ToInt(comborderno.Value) != 0)
			{
				ItemNameFill();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void comWarehouse_TextChanged(object sender, EventArgs e)
	{
		AvailableStockGetByItem();
	}

	private void comCropYear_Leave(object sender, EventArgs e)
	{
		AvailableStockGetByItem();
	}

	private void comjobLot_Leave(object sender, EventArgs e)
	{
		AvailableStockGetByItem();
	}

	private void comPackingType_Leave(object sender, EventArgs e)
	{
		AvailableStockGetByItem();
	}

	private void ConfigurationDefault()
	{
		try
		{
			List<ConfigrationsAllocation> ItemStatus = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "GetItemsagainstWarehouse").ToList();
			if (ItemStatus.Count > 0)
			{
				GetItemagainstwarehouseId = Conversion.ToBool((object)ItemStatus[0].ConfigKey.ToString());
			}
			List<ConfigrationsAllocation> lstCrop = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Default Crop Year").ToList();
			if (lstCrop.Count > 0)
			{
				int CropYearId = Conversion.ToInt((object)lstCrop[0].ConfigKey.ToString());
				comCropYear.Value = CropYearId;
			}
			List<ConfigrationsAllocation> lstPackingType = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Paking Type").ToList();
			if (lstPackingType.Count > 0)
			{
				int PackingTypeId = Conversion.ToInt((object)lstPackingType[0].ConfigKey.ToString());
				comPackingType.Value = PackingTypeId;
			}
			List<ConfigrationsAllocation> lstWareHouse = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Warehouse").ToList();
			if (lstWareHouse.Count > 0)
			{
				int WareHouseId = Conversion.ToInt((object)lstWareHouse[0].ConfigKey.ToString());
				comWarehouse.Value = WareHouseId;
			}
			List<ConfigrationsAllocation> lstJobLot = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Job/Lot").ToList();
			if (lstJobLot.Count > 0)
			{
				getJobLotId = Conversion.ToInt((object)lstJobLot[0].ConfigKey.ToString());
				return;
			}
			throw new Exception("JobLot config not configure properly pls check");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (((UltraGridBase)combranches).ActiveRow == null || Conversion.ToInt(combranches.Value) == 0)
		{
			MessageBox.Show("Branch Field is Required");
			combranches.Focus();
			return false;
		}
		if (((UltraGridBase)comproject).ActiveRow == null || Conversion.ToInt(comproject.Value) == 0)
		{
			MessageBox.Show("Project Field is Required");
			comproject.Focus();
			return false;
		}
		if (((UltraGridBase)comsupplier).ActiveRow == null || Conversion.ToInt(comsupplier.Value) == 0)
		{
			MessageBox.Show("CustomerName Field is Required");
			comsupplier.Focus();
			return false;
		}
		if (txtdocno.Text.Trim() == "" || txtdocno.Text.Trim() == "0" || txtdocno.Text.Trim() == null)
		{
			MessageBox.Show("DocNo Field is Required");
			txtdocno.Focus();
			return false;
		}
		if (Conversion.ToString((object)((Control)(object)combpttrm).Text) == "Credit")
		{
			if (txtduedays.Text.Trim() == "" || txtduedays.Text.Trim() == null)
			{
				MessageBox.Show("Due Days Field is Required");
				txtduedays.Focus();
				return false;
			}
			return true;
		}
		return true;
	}

	private bool FormValidationDetila()
	{
		if (((UltraGridBase)comWarehouse).ActiveRow == null || Conversion.ToInt(comWarehouse.Value) == 0)
		{
			MessageBox.Show("Warehouse Field is Required");
			comWarehouse.Focus();
			return false;
		}
		if (((UltraGridBase)comItem).ActiveRow == null || Conversion.ToInt(comItem.Value) == 0)
		{
			MessageBox.Show("Item Name Field is Required");
			comItem.Focus();
			return false;
		}
		if (((UltraGridBase)comCropYear).ActiveRow == null || Conversion.ToInt(comCropYear.Value) == 0)
		{
			MessageBox.Show("Crop Year Field is Required");
			comCropYear.Focus();
			return false;
		}
		if (((UltraGridBase)comPackingType).ActiveRow == null || Conversion.ToInt(comPackingType.Value) == 0)
		{
			MessageBox.Show("Packing Type Field is Required");
			comPackingType.Focus();
			return false;
		}
		if (((UltraGridBase)comPackUOM).ActiveRow == null || Conversion.ToInt(comPackUOM.Value) == 0)
		{
			MessageBox.Show("Pack Unit Field is Required");
			comPackUOM.Focus();
			return false;
		}
		if (txtQty.Text.Trim() == "" || txtQty.Text.Trim() == "0")
		{
			MessageBox.Show("Qty Field is Required");
			txtQty.Focus();
			return false;
		}
		if (txtNetBillWeight.Text.Trim() == "" || txtNetBillWeight.Text.Trim() == "0")
		{
			MessageBox.Show("Net Bill Weight Field is Required");
			txtNetBillWeight.Focus();
			return false;
		}
		if (txtRate.Text.Trim() == "" || txtRate.Text.Trim() == "0")
		{
			MessageBox.Show("Rate Field is Required");
			txtRate.Focus();
			return false;
		}
		if (((UltraGridBase)comRateUOM).ActiveRow == null || Conversion.ToInt(comRateUOM.Value) == 0)
		{
			MessageBox.Show("Rate UOM Field is Required");
			comRateUOM.Focus();
			return false;
		}
		if (txtAmount.Text.Trim() == "" || txtAmount.Text.Trim() == "0")
		{
			MessageBox.Show("Item Amount Field is Required");
			txtAmount.Focus();
			return false;
		}
		return true;
	}

	private void DisableFields()
	{
		try
		{
			((Control)(object)comWarehouse).Enabled = false;
			((Control)(object)comItem).Enabled = false;
			((Control)(object)comCropYear).Enabled = false;
			((Control)(object)comPackUOM).Enabled = false;
			((Control)(object)comPackingType).Enabled = false;
			txtQty.Enabled = false;
			txtNetBillWeight.Enabled = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void EnablesFields()
	{
		try
		{
			((Control)(object)comWarehouse).Enabled = true;
			((Control)(object)comItem).Enabled = true;
			((Control)(object)comCropYear).Enabled = true;
			((Control)(object)comPackUOM).Enabled = true;
			((Control)(object)comPackingType).Enabled = true;
			txtQty.Enabled = true;
			txtNetBillWeight.Enabled = true;
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
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			AttachmentsValues = "";
			CustomAttachmentsValues = "";
			EnablesFields();
			FlagLoader = "";
			RecId = 0;
			VoucherHeadId = 0;
			Approved = false;
			AT = new Attachment();
			((Control)(object)comstockparty).Text = string.Empty;
			((Control)(object)compartyreference).Text = string.Empty;
			txtbillno.Text = string.Empty;
			txtremarks.Text = string.Empty;
			txtdeliverydays.Text = string.Empty;
			txtduedays.Text = "0";
			((Control)(object)comItem).Text = string.Empty;
			((Control)(object)comPackUOM).Text = string.Empty;
			txtQty.Text = string.Empty;
			txtNetBillWeight.Text = string.Empty;
			txtRate.Text = string.Empty;
			((Control)(object)comRateUOM).Text = string.Empty;
			txtAmount.Text = string.Empty;
			txtBillAmount.Text = string.Empty;
			DocDate.Focus();
			dtInvExp.Rows.Clear();
			dtGrdGL.Rows.Clear();
			dtGrid.Rows.Clear();
			AddRowInGLGrid();
			grdGLedger.RetrieveStructure();
			gridGLSettings();
			AddRowInvExpGrid();
			grdInvExp.RetrieveStructure();
			grdInvExpSettings();
			DocumentNo();
			if (ChkResetOnSave.Checked)
			{
				OptionResetFields();
			}
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			btnDelete.Visible = false;
			panelDetail.Visible = true;
			IsFromLoader = false;
			btnAdd.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			txtduedays.Enabled = true;
			if (Conversion.ToInt(combpttrm.Value) == 1)
			{
				txtduedays.Text = "";
				txtduedays.Enabled = false;
			}
			ConfigurationDefault();
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
			((Control)(object)comPackUOM).Text = string.Empty;
			txtQty.Text = string.Empty;
			txtNetBillWeight.Text = string.Empty;
			txtRate.Text = string.Empty;
			((Control)(object)comRateUOM).Text = string.Empty;
			txtAmount.Text = string.Empty;
			if (ChkResetOnSave.Checked)
			{
				OptionResetFields();
			}
			if (dtPackType.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == 2))
			{
				comPackingType.Value = 2;
			}
			else
			{
				((Control)(object)comPackingType).Text = string.Empty;
			}
			comItem.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OptionResetFields()
	{
		((Control)(object)comborderno).Text = string.Empty;
		((Control)(object)comItem).Text = string.Empty;
		((Control)(object)comCropYear).Text = string.Empty;
		((Control)(object)comPackingType).Text = string.Empty;
		((Control)(object)comWarehouse).Text = string.Empty;
		txtgatepassno.Text = string.Empty;
		txtvehicleno.Text = string.Empty;
		((Control)(object)txtCity).Text = string.Empty;
		comItem.Focus();
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			ConfigurationDefault();
			OtherItemsBind();
			AccountsFill();
			GridComboBind();
			bindWareHouse();
			BinCity();
			suppliercustomer();
			PaymentTerms();
			DeliveryTerm();
			ItemNameFill();
			Project();
			PackingType();
			braches();
			cropyear();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		Reset();
	}

	private void frmSaleInvoiceDirectFlourAutoRated_FormClosing(object sender, FormClosingEventArgs e)
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

	private void combpttrm_Leave(object sender, EventArgs e)
	{
		try
		{
			txtduedays.Enabled = true;
			if (Conversion.ToInt(combpttrm.Value) == 1)
			{
				txtduedays.Text = "";
				txtduedays.Enabled = false;
			}
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
			grdInvExp.RootTable.Columns["ItemId"].ColumnType = (ColumnType)0;
			grdInvExp.RootTable.Columns["ItemId"].HasValueList = true;
			grdInvExp.RootTable.Columns["ItemId"].LimitToList = true;
			grdInvExp.RootTable.Columns["ItemId"].EditType = (EditType)4;
			grdInvExp.RootTable.Columns["ItemId"].Caption = "Item";
			grdInvExp.RootTable.Columns["ItemId"].ValueList.PopulateValueList((IEnumerable)dtOtherItem.DefaultView, "Id", "OtherItemName");
			grdInvExp.RootTable.Columns["ItemId"].Width = 250;
			grdGLedger.RootTable.Columns["AccountId"].ColumnType = (ColumnType)0;
			grdGLedger.RootTable.Columns["AccountId"].HasValueList = true;
			grdGLedger.RootTable.Columns["AccountId"].LimitToList = true;
			grdGLedger.RootTable.Columns["AccountId"].EditType = (EditType)4;
			grdGLedger.RootTable.Columns["AccountId"].Caption = "Account";
			grdGLedger.RootTable.Columns["AccountId"].ValueList.PopulateValueList((IEnumerable)dtAccountlst.DefaultView, "Id", "AccountTitle");
			grdGLedger.RootTable.Columns["AccountId"].Width = 250;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OtherItemsBind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			InventoryItemsOther obj = new InventoryItemsOther();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			dtOtherItem = InventoryItemsOther.GetAll(obj);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInvExpGrid()
	{
		dtInvExp.Rows.Add(0, 0, 0, 0, "");
		grdInvExp.DataSource = dtInvExp;
	}

	private void grdInvExpSettings()
	{
		try
		{
			grdInvExp.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdInvExp.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdInvExp.RootTable.Columns["Qty"].FormatString = "#,##0.##";
			grdInvExp.RootTable.Columns["Qty"].TotalFormatString = "#,##0.##";
			grdInvExp.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdInvExp.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdInvExp.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdInvExp.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdInvExp.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdInvExp.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdInvExp.RootTable.Columns["ItemId"].ColumnType = (ColumnType)0;
			grdInvExp.RootTable.Columns["ItemId"].HasValueList = true;
			grdInvExp.RootTable.Columns["ItemId"].LimitToList = true;
			grdInvExp.RootTable.Columns["ItemId"].EditType = (EditType)4;
			grdInvExp.RootTable.Columns["ItemId"].Caption = "Item";
			grdInvExp.RootTable.Columns["ItemId"].ValueList.PopulateValueList((IEnumerable)dtOtherItem.DefaultView, "Id", "OtherItemName");
			grdInvExp.RootTable.Columns["ItemId"].Width = 250;
			grdInvExp.RootTable.Columns.Add("Delete");
			grdInvExp.RootTable.Columns["Delete"].Key = "Delete";
			grdInvExp.RootTable.Columns["Delete"].Caption = "Delete";
			grdInvExp.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdInvExp.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdInvExp.RootTable.Columns["Delete"].ButtonText = "Delete";
			grdInvExp.RootTable.Columns["Delete"].Position = 0;
			grdInvExp.RootTable.Columns.Add("Add");
			grdInvExp.RootTable.Columns["Add"].Key = "Add";
			grdInvExp.RootTable.Columns["Add"].Caption = "Add New";
			grdInvExp.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdInvExp.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdInvExp.RootTable.Columns["Add"].ButtonText = "Add";
			grdInvExp.RootTable.Columns["Add"].Position = 1;
			grdInvExp.FrozenColumns = 2;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInvExp_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow item = grdInvExp.CurrentRow;
				dtInvExp.Rows[item.RowIndex].Delete();
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
				if (dtInvExp.Rows.Count == 0)
				{
					AddRowInvExpGrid();
				}
			}
			if (e.Column.Key == "Add")
			{
				AddRowInvExpGrid();
			}
			grdInvExp.UpdateData();
			BillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInvExp_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdInvExp.UpdateData();
			GridEXRow item = grdInvExp.CurrentRow;
			if (e.Column.Key == "Qty" || e.Column.Key == "Rate")
			{
				if (item != null && (int)item.RowType == 0 && item.Cells["Qty"].Value.ToString() != string.Empty && item.Cells["Rate"].Value.ToString() != string.Empty)
				{
					double num = Conversion.ToDouble((object)item.Cells["Qty"].Value.ToString());
					double Rate = Conversion.ToDouble((object)item.Cells["Rate"].Value.ToString());
					double Amount = num * Rate;
					item.Cells["Amount"].Value = Math.Round(Amount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				}
			}
			else if (e.Column.Key == "Amount")
			{
				item.Cells["Amount"].Value = Math.Round(Conversion.ToDouble(item.Cells["Amount"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
			}
			grdInvExp.UpdateData();
			BillAmount();
			ExpProportion();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInvExp_UpdatingCell(object sender, UpdatingCellEventArgs e)
	{
		try
		{
			if ((e.Column.Key == "Qty" || e.Column.Key == "Rate" || e.Column.Key == "Amount") && !CommonServices.IsNumeric(e))
			{
				throw new Exception("Please Type Only Numeric Value");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void AddRowInGLGrid()
	{
		dtGrdGL.Rows.Add(0, "", 0, 0, 0, 0, 0);
		grdGLedger.DataSource = dtGrdGL;
	}

	private void gridGLSettings()
	{
		try
		{
			grdGLedger.RootTable.Columns["AccountId"].ColumnType = (ColumnType)0;
			grdGLedger.RootTable.Columns["AccountId"].HasValueList = true;
			grdGLedger.RootTable.Columns["AccountId"].LimitToList = true;
			grdGLedger.RootTable.Columns["AccountId"].EditType = (EditType)4;
			grdGLedger.RootTable.Columns["AccountId"].Caption = "Account";
			grdGLedger.RootTable.Columns["AccountId"].ValueList.PopulateValueList((IEnumerable)dtAccountlst.DefaultView, "Id", "AccountTitle");
			grdGLedger.RootTable.Columns["AccountId"].Width = 250;
			grdGLedger.RootTable.Columns.Add("Delete");
			grdGLedger.RootTable.Columns["Delete"].Key = "Delete";
			grdGLedger.RootTable.Columns["Delete"].Caption = "Delete";
			grdGLedger.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdGLedger.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdGLedger.RootTable.Columns["Delete"].ButtonText = "Delete";
			grdGLedger.RootTable.Columns.Add("Add");
			grdGLedger.RootTable.Columns["Add"].Key = "Add";
			grdGLedger.RootTable.Columns["Add"].Caption = "Add New";
			grdGLedger.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdGLedger.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdGLedger.RootTable.Columns["Add"].ButtonText = "Add";
			grdGLedger.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdGLedger.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdGLedger.RootTable.Columns["Qty"].FormatString = "#,##0.##";
			grdGLedger.RootTable.Columns["Qty"].TotalFormatString = "#,##0.##";
			grdGLedger.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdGLedger.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdGLedger.RootTable.Columns["Credit"].AggregateFunction = (AggregateFunction)2;
			grdGLedger.RootTable.Columns["Credit"].TextAlignment = (TextAlignment)3;
			grdGLedger.RootTable.Columns["Credit"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdGLedger.RootTable.Columns["Credit"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdGLedger.RootTable.Columns["Debit"].AggregateFunction = (AggregateFunction)2;
			grdGLedger.RootTable.Columns["Debit"].TextAlignment = (TextAlignment)3;
			grdGLedger.RootTable.Columns["Debit"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdGLedger.RootTable.Columns["Debit"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdGLedger_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow item = grdGLedger.CurrentRow;
				dtGrdGL.Rows[item.RowIndex].Delete();
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
				if (dtGrdGL.Rows.Count == 0)
				{
					AddRowInGLGrid();
				}
			}
			if (e.Column.Key == "Add")
			{
				AddRowInGLGrid();
			}
			grdGLedger.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		BillAmount();
	}

	private void grdGLedger_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_02eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a5: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdGLedger.UpdateData();
			GridEXRow item = grdGLedger.CurrentRow;
			if ((e.Column.Key == "Qty" || e.Column.Key == "Rate") && item != null && (int)item.RowType == 0 && item.Cells["Qty"].Value.ToString() != string.Empty && item.Cells["Rate"].Value.ToString() != string.Empty)
			{
				double Qty = Conversion.ToDouble((object)item.Cells["Qty"].Value.ToString());
				double Rate = Conversion.ToDouble((object)item.Cells["Rate"].Value.ToString());
				item.Cells["Credit"].Value = Math.Round(Qty * Rate, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				item.Cells["Debit"].Value = 0;
				item.Cells["Percentage"].Value = 0;
			}
			if (e.Column.Key == "Percentage" && item != null && (int)item.RowType == 0 && item.Cells["Percentage"].Value.ToString() != string.Empty)
			{
				double num = Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["ItemAmount"], (AggregateFunction)2).ToString());
				double Percentage = Conversion.ToDouble((object)item.Cells["Percentage"].Value.ToString());
				double TotlaPercentage = num / 100.0 * Percentage;
				if (TotlaPercentage > 0.0)
				{
					item.Cells["Credit"].Value = Math.Round(TotlaPercentage, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
					item.Cells["Debit"].Value = 0;
				}
				else
				{
					item.Cells["Debit"].Value = Math.Abs(Math.Round(TotlaPercentage, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero));
					item.Cells["Credit"].Value = 0;
				}
				item.Cells["Qty"].Value = 0;
				item.Cells["Rate"].Value = 0;
			}
			if (e.Column.Key == "Credit" && item != null && (int)item.RowType == 0)
			{
				item.Cells["Credit"].Value = Math.Round(Conversion.ToDouble(item.Cells["Credit"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				if (Conversion.ToDouble((object)item.Cells["Debit"].Value.ToString()) > 0.0)
				{
					item.Cells["Credit"].Value = 0;
					MessageBox.Show("Debit Side is aleady added");
				}
			}
			if (e.Column.Key == "Debit" && item != null && (int)item.RowType == 0)
			{
				item.Cells["Debit"].Value = Math.Round(Conversion.ToDouble(item.Cells["Debit"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				if (Conversion.ToDouble((object)item.Cells["Credit"].Value.ToString()) > 0.0)
				{
					item.Cells["Debit"].Value = 0;
					MessageBox.Show("Credit Side is aleady added");
				}
			}
			grdGLedger.UpdateData();
			BillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdGLedger_UpdatingCell(object sender, UpdatingCellEventArgs e)
	{
		try
		{
			if ((e.Column.Key == "Percentage" || e.Column.Key == "Qty" || e.Column.Key == "Rate" || e.Column.Key == "Debit" || e.Column.Key == "Credit") && !CommonServices.IsNumeric(e))
			{
				throw new Exception("Please Type Only Numeric Value");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void btnAdd_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValidationDetila())
			{
				if (grd.RowCount > 0 && IsFromLoader)
				{
					throw new Exception("You can not Add Manual Record Becuase Record from Loader Exist in grd");
				}
				dtGrid.Rows.Add(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, Conversion.ToInt(comWarehouse.Value), Conversion.ToString((object)((Control)(object)comWarehouse).Text), Conversion.ToInt(comItem.Value), Conversion.ToString(((UltraDropDownBase)comItem).SelectedRow.Cells["ItemName"].Value), Conversion.ToString(((UltraDropDownBase)comItem).SelectedRow.Cells["ItemCode"].Value), Conversion.ToString((object)((Control)(object)comCropYear).Text), Conversion.ToInt(comPackingType.Value), Conversion.ToString((object)((Control)(object)comPackingType).Text), Conversion.ToInt(comPackUOM.Value), Conversion.ToDouble(((UltraDropDownBase)comPackUOM).SelectedRow.Cells[2].Value), Conversion.ToDouble((object)txtQty.Text), Conversion.ToDouble((object)txtQty.Text), Conversion.ToDouble((object)txtNetBillWeight.Text), Conversion.ToDouble((object)txtNetBillWeight.Text), Conversion.ToDouble((object)txtRate.Text), Conversion.ToDouble((object)txtRate.Text), Conversion.ToInt(comRateUOM.Value), Conversion.ToDouble(((UltraDropDownBase)comRateUOM).SelectedRow.Cells[2].Value), Conversion.ToDouble((object)txtAmount.Text), Conversion.ToDateTime((object)txtgpdate.Value).ToShortDateString(), Conversion.ToInt((object)txtgatepassno.Text), Conversion.ToString((object)txtvehicleno.Text), 0, 0, 0, Conversion.ToInt(txtCity.Value), (Conversion.ToInt(txtCity.Value) > 0) ? Conversion.ToString((object)((Control)(object)txtCity).Text) : "");
				grd.DataSource = dtGrid;
				grd.RetrieveStructure();
				grdSettings();
				IsFromLoader = false;
				ExpProportion();
				BillAmount();
				ResetDetail();
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
			if (IsFromLoader)
			{
				return;
			}
			GridEXRow r = grd.CurrentRow;
			if (r != null)
			{
				updateDetailIndex = r.RowIndex;
				if (Conversion.ToInt(r.Cells["InvGdnId"].Value) > 0)
				{
					DisableFields();
				}
				comWarehouse.Value = r.Cells["WareHouseId"].Value.ToString();
				comItem.Value = Conversion.ToInt(r.Cells["ItemId"].Value);
				PackUOM();
				((Control)(object)comCropYear).Text = r.Cells["CropYear"].Text.ToString();
				comPackingType.Value = Conversion.ToInt(r.Cells["PackingTypeId"].Value);
				comPackUOM.Value = Conversion.ToInt(r.Cells["PackUOMId"].Value);
				txtQty.Text = Conversion.ToDouble(r.Cells["ItemQty"].Value).ToString("#,##0.##");
				txtNetBillWeight.Text = Conversion.ToDouble(r.Cells["NetBillWeight"].Value).ToString("#,##0.##");
				txtRate.Text = Conversion.ToDouble(r.Cells["Rate"].Value).ToString(clsGlobalVariables.DecimalRateFormate);
				if (Conversion.ToInt(r.Cells["RateUOMId"].Value) > 0)
				{
					comRateUOM.Value = Conversion.ToInt(r.Cells["RateUOMId"].Value);
				}
				txtAmount.Text = Conversion.ToDouble(r.Cells["ItemAmount"].Value).ToString(clsGlobalVariables.stringFormatsingle);
				txtgpdate.Text = r.Cells["GpDate"].Value.ToString();
				txtgatepassno.Text = r.Cells["GpNo"].Value.ToString();
				txtvehicleno.Text = r.Cells["VehicleNo"].Value.ToString();
				txtCity.Value = Conversion.ToInt(r.Cells["CityId"].Value);
				btnAdd.Visible = false;
				btnUpdateDetail.Visible = true;
				btnCancelUpdateDetial.Visible = true;
				comWarehouse.Focus();
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
			if (FormValidationDetila())
			{
				dtGrid.Rows[updateDetailIndex]["WarehouseId"] = Conversion.ToInt(comWarehouse.Value);
				dtGrid.Rows[updateDetailIndex]["Warehouse"] = Conversion.ToString((object)((Control)(object)comWarehouse).Text);
				dtGrid.Rows[updateDetailIndex]["ItemId"] = Conversion.ToInt(comItem.Value);
				dtGrid.Rows[updateDetailIndex]["Item"] = Conversion.ToString(((UltraDropDownBase)comItem).SelectedRow.Cells["ItemName"].Value);
				dtGrid.Rows[updateDetailIndex]["ItemCode"] = Conversion.ToString(((UltraDropDownBase)comItem).SelectedRow.Cells["ItemCode"].Value);
				dtGrid.Rows[updateDetailIndex]["CropYear"] = Conversion.ToString((object)((Control)(object)comCropYear).Text);
				dtGrid.Rows[updateDetailIndex]["PackingTypeId"] = Conversion.ToInt(comPackingType.Value);
				dtGrid.Rows[updateDetailIndex]["PackingType"] = Conversion.ToString((object)((Control)(object)comPackingType).Text);
				dtGrid.Rows[updateDetailIndex]["ItemQty"] = Conversion.ToDouble((object)txtQty.Text);
				dtGrid.Rows[updateDetailIndex]["PackUOMId"] = Conversion.ToInt(comPackUOM.Value);
				dtGrid.Rows[updateDetailIndex]["PackUOMEquivalent"] = Conversion.ToDouble(((UltraDropDownBase)comPackUOM).SelectedRow.Cells[2].Value);
				dtGrid.Rows[updateDetailIndex]["NetBillWeight"] = Conversion.ToDouble((object)txtNetBillWeight.Text);
				dtGrid.Rows[updateDetailIndex]["Rate"] = Conversion.ToDouble((object)txtRate.Text);
				dtGrid.Rows[updateDetailIndex]["RateUOMId"] = Conversion.ToInt(comRateUOM.Value);
				dtGrid.Rows[updateDetailIndex]["RateEquivalent"] = Conversion.ToDouble(((UltraDropDownBase)comRateUOM).SelectedRow.Cells[2].Value);
				dtGrid.Rows[updateDetailIndex]["ItemAmount"] = Conversion.ToDouble((object)txtAmount.Text);
				dtGrid.Rows[updateDetailIndex]["CityId"] = Conversion.ToInt(txtCity.Value);
				dtGrid.Rows[updateDetailIndex]["CityName"] = Conversion.ToString((object)((Control)(object)txtCity).Text);
				dtGrid.Rows[updateDetailIndex]["GpDate"] = Conversion.ToDateTime((object)txtgpdate.Value);
				dtGrid.Rows[updateDetailIndex]["GpNo"] = Conversion.ToInt((object)txtgatepassno.Text);
				dtGrid.Rows[updateDetailIndex]["VehicleNo"] = Conversion.ToString((object)txtvehicleno.Text);
				btnAdd.Visible = true;
				btnUpdateDetail.Visible = false;
				btnCancelUpdateDetial.Visible = false;
				ExpProportion();
				BillAmount();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelUpdateDetial_Click(object sender, EventArgs e)
	{
		btnAdd.Visible = true;
		btnUpdateDetail.Visible = false;
		btnCancelUpdateDetial.Visible = false;
	}

	private void grdSettings()
	{
		try
		{
			if (IsFromLoader)
			{
				grd.RootTable.Columns["ItemQty"].EditType = (EditType)1;
			}
			else
			{
				panelDetail.Visible = true;
				grd.RootTable.Columns["RateEquivalent"].EditType = (EditType)0;
				grd.RootTable.Columns["ItemQty"].EditType = (EditType)0;
			}
			grd.RootTable.Columns["CropYear"].EditType = (EditType)0;
			grd.RootTable.Columns["RateEquivalent"].EditType = (EditType)0;
			grd.RootTable.Columns["Id"].EditType = (EditType)0;
			grd.RootTable.Columns["RefDocumentTypeId"].EditType = (EditType)0;
			grd.RootTable.Columns["RefDocIdNo"].EditType = (EditType)0;
			grd.RootTable.Columns["RefDocSubIdNo"].EditType = (EditType)0;
			grd.RootTable.Columns["SaleOrderId"].EditType = (EditType)0;
			grd.RootTable.Columns["SaleOrderDetailId"].EditType = (EditType)0;
			grd.RootTable.Columns["SaleOrderNo"].EditType = (EditType)0;
			grd.RootTable.Columns["InvGdnId"].EditType = (EditType)0;
			grd.RootTable.Columns["InvGdnDetailId"].EditType = (EditType)0;
			grd.RootTable.Columns["GdnNo"].EditType = (EditType)0;
			grd.RootTable.Columns["WarehouseId"].EditType = (EditType)0;
			grd.RootTable.Columns["Warehouse"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemId"].EditType = (EditType)0;
			grd.RootTable.Columns["Item"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemCode"].EditType = (EditType)0;
			grd.RootTable.Columns["PackingTypeId"].EditType = (EditType)0;
			grd.RootTable.Columns["PackingType"].EditType = (EditType)0;
			grd.RootTable.Columns["PackUOMId"].EditType = (EditType)0;
			grd.RootTable.Columns["PackUOMEquivalent"].EditType = (EditType)0;
			grd.RootTable.Columns["RateUOMId"].EditType = (EditType)0;
			grd.RootTable.Columns["CityId"].EditType = (EditType)0;
			grd.RootTable.Columns["BalWeight"].EditType = (EditType)0;
			grd.RootTable.Columns["BalQty"].EditType = (EditType)0;
			grd.RootTable.Columns["NetBillWeight"].EditType = (EditType)0;
			grd.RootTable.Columns["Rate"].EditType = (EditType)0;
			grd.RootTable.Columns["CgsRate"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemAmount"].EditType = (EditType)0;
			grd.RootTable.Columns["BillAmount"].EditType = (EditType)0;
			grd.RootTable.Columns["Expense"].EditType = (EditType)0;
			grd.RootTable.Columns["Journal"].EditType = (EditType)0;
			grd.RootTable.Columns["CityId"].EditType = (EditType)0;
			grd.RootTable.Columns["CityName"].EditType = (EditType)0;
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["RefDocumentTypeId"].Visible = false;
			grd.RootTable.Columns["RefDocIdNo"].Visible = false;
			grd.RootTable.Columns["RefDocSubIdNo"].Visible = false;
			grd.RootTable.Columns["SaleOrderId"].Visible = false;
			grd.RootTable.Columns["SaleOrderDetailId"].Visible = false;
			grd.RootTable.Columns["SaleOrderNo"].Visible = false;
			grd.RootTable.Columns["InvGdnId"].Visible = false;
			grd.RootTable.Columns["InvGdnDetailId"].Visible = false;
			grd.RootTable.Columns["GdnNo"].Visible = false;
			grd.RootTable.Columns["WarehouseId"].Visible = false;
			grd.RootTable.Columns["ItemId"].Visible = false;
			grd.RootTable.Columns["PackingTypeId"].Visible = false;
			grd.RootTable.Columns["PackUOMId"].Visible = false;
			grd.RootTable.Columns["RateUOMId"].Visible = false;
			grd.RootTable.Columns["CityId"].Visible = false;
			grd.RootTable.Columns["BalWeight"].Visible = false;
			grd.RootTable.Columns["BalQty"].Visible = false;
			grd.RootTable.Columns["RateEquivalent"].Caption = "Rate Uom";
			grd.RootTable.Columns["PackUOMEquivalent"].Caption = "Pack Uom";
			grd.RootTable.Columns["BillAmount"].Caption = "Item Net Amount";
			grd.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemQty"].FormatString = "#,##0.##";
			grd.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["NetBillWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["NetBillWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["NetBillWeight"].FormatString = "#,##0.##";
			grd.RootTable.Columns["NetBillWeight"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grd.RootTable.Columns["CgsRate"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["CgsRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grd.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["ItemAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["BillAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["BillAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["BillAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["BillAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["Expense"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Expense"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Expense"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["Expense"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["Journal"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Journal"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Journal"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["Journal"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustmentNew(grd);
			grd.RootTable.Columns.Add("Delete");
			grd.RootTable.Columns["Delete"].Key = "Delete";
			grd.RootTable.Columns["Delete"].Caption = "X";
			grd.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Delete"].ButtonText = "X";
			grd.RootTable.Columns["Delete"].Width = 20;
			grd.RootTable.Columns["Delete"].Position = 0;
			grd.FrozenColumns = 1;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		grd.UpdateData();
		GridEXRow item = grd.CurrentRow;
		if (IsFromLoader && item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
		{
			if (e.Column.Key == "ItemQty")
			{
				double Qty = Conversion.ToDouble(item.Cells["ItemQty"].Value);
				double BalQty = Conversion.ToDouble(item.Cells["BalQty"].Value);
				double BalWt = Conversion.ToDouble(item.Cells["BalWeight"].Value);
				double NetWt2 = 0.0;
				double Amount2 = 0.0;
				if (Qty > BalQty)
				{
					MessageBox.Show("Item Cannot be greater than Balance Qty " + BalQty + " Of this row");
					item.Cells["ItemQty"].Value = BalQty;
					NetWt2 = BalWt;
					item.Cells["NetBillWeight"].Value = NetWt2;
				}
				else
				{
					NetWt2 = Conversion.ToDouble(item.Cells["PackUOMEquivalent"].Value) * Qty;
					item.Cells["NetBillWeight"].Value = NetWt2;
					if (NetWt2 > BalWt)
					{
						NetWt2 = BalWt;
						item.Cells["NetBillWeight"].Value = NetWt2;
					}
				}
				if (NetWt2 > 0.0)
				{
					double IRate2 = Conversion.ToDouble(item.Cells["Rate"].Value);
					double RUom2 = Conversion.ToDouble(item.Cells["RateEquivalent"].Value);
					Amount2 = NetWt2 / RUom2 * IRate2;
					item.Cells["ItemAmount"].Value = Amount2;
				}
				else
				{
					Amount2 = 0.0;
					item.Cells["ItemAmount"].Value = Amount2;
				}
			}
			if (e.Column.Key == "RateEquivalent")
			{
				double NetWt = Conversion.ToDouble(item.Cells["NetBillWeight"].Value);
				double Amount = 0.0;
				if (NetWt > 0.0)
				{
					double IRate = Conversion.ToDouble(item.Cells["Rate"].Value);
					double RUom = Conversion.ToDouble(item.Cells["RateEquivalent"].Value);
					Amount = NetWt / RUom * IRate;
					item.Cells["ItemAmount"].Value = Amount;
				}
				else
				{
					Amount = 0.0;
					item.Cells["ItemAmount"].Value = Amount;
				}
			}
		}
		grd.UpdateData();
		ExpProportion();
		BillAmount();
	}

	private void grd_EditingCell(object sender, EditingCellEventArgs e)
	{
	}

	private void grd_RecordsDeleted(object sender, EventArgs e)
	{
		try
		{
			grd.UpdateData();
			ExpProportion();
			BillAmount();
			grd.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grd.UpdateData();
			if (e.Column.Key == "Delete")
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				GridEXRow item = grd.CurrentRow;
				dtGrid.Rows[item.RowIndex].Delete();
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
			}
			ExpProportion();
			BillAmount();
			grd.UpdateData();
			if (grd.RowCount == 0)
			{
				IsFromLoader = false;
			}
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
		//IL_037c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0383: Expected O, but got Unknown
		//IL_0b01: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b08: Expected O, but got Unknown
		//IL_0caa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cb1: Expected O, but got Unknown
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
			InvSaleInvoice obj = new InvSaleInvoice();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				obj.Id = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			GridEXRow[] rows = grdInvExp.GetRows();
			foreach (GridEXRow r3 in rows)
			{
				if (Conversion.ToDouble(r3.Cells["Amount"].Value) > 0.0 && Conversion.ToInt(r3.Cells["ItemId"].Value) == 0)
				{
					MessageBox.Show("Please Select an Item Against Expense First");
					return;
				}
			}
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.BranchesId = Conversion.ToInt((object)UserAccount.BranchesId);
			obj.ProjectsId = Conversion.ToInt(comproject.Value);
			obj.DocumentTypeId = 186;
			obj.DocNo = Conversion.ToInt((object)txtdocno.Text);
			obj.DocDate = DocDate.Value;
			obj.SupplierCustomerId = Conversion.ToInt(comsupplier.Value);
			obj.ManualBillNo = txtbillno.Text.Trim();
			obj.DeliveryTerm = Conversion.ToString((object)((Control)(object)combdeliverytrm).Text);
			obj.PaymentTermId = Conversion.ToInt(combpttrm.Value);
			obj.DueDate = duedate.Value;
			obj.DueDays = Conversion.ToInt((object)txtduedays.Text);
			obj.BillAmount = Conversion.ToDouble((object)txtBillAmount.Text.Trim());
			obj.RemarksHeader = txtremarks.Text.Trim();
			obj.CommAmount = 0.0;
			obj.CommissionAgentId = 0;
			obj.CommissionRemarks = "";
			obj.CommissionType = "";
			obj.CommRate = 0.0;
			obj.UomScheduleIdCmRate = "";
			obj.ScreenName = Conversion.ToString((object)ScreenName);
			obj.ReferencePartyId = Conversion.ToInt(compartyreference.Value);
			obj.StockPartyId = Conversion.ToInt(comstockparty.Value);
			obj.SupplierInvoiceDate = duedate.Value;
			obj.SupplierInvoiceNo = Conversion.ToInt((object)txtduedays.Text);
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			obj.EntryUser = UserAccount.ID;
			obj.ModifyUser = UserAccount.ID;
			obj.IsReserve = false;
			obj.InvSaleInvoiceDetaillist = new List<InvSaleInvoiceDetail>();
			obj.InvSaleInvoiceCommission = new List<InvSaleInvoiceCommission>();
			obj.InvSaleInvoiceFreightList = new List<InvSaleInvoiceFreight>();
			obj.InvSaleInvoiceExpenseList = new List<InvSaleInvoiceExpense>();
			obj.InvSaleInvoiceJournalList = new List<InvSaleInvoiceJournal>();
			double GrossWt = 0.0;
			rows = grd.GetRows();
			foreach (GridEXRow r4 in rows)
			{
				InvSaleInvoiceDetail detail = new InvSaleInvoiceDetail();
				detail.LineId = r4.RowIndex + 1;
				detail.Id = Conversion.ToInt(r4.Cells["Id"].Value);
				detail.RefRefDocumentTypeId = Conversion.ToInt(r4.Cells["RefDocumentTypeId"].Value);
				detail.RefRefDocIdNo = Conversion.ToInt(r4.Cells["RefDocIdNo"].Value);
				detail.RefDocSubId = Conversion.ToInt(r4.Cells["RefDocSubIdNo"].Value);
				detail.SaleOrderId = Conversion.ToInt(r4.Cells["SaleOrderId"].Value);
				detail.SaleOrderDetailId = Conversion.ToInt(r4.Cells["SaleOrderDetailId"].Value);
				detail.InvGdnId = Conversion.ToInt(r4.Cells["InvGdnId"].Value);
				detail.InvGdnDetailId = Conversion.ToInt(r4.Cells["InvGdnDetailId"].Value);
				if (Conversion.ToInt(r4.Cells["WarehouseId"].Value) == 0)
				{
					MessageBox.Show("Warehouse is required in Detail Grid in Row No: " + (r4.RowIndex + 1) + "Please Check...");
					return;
				}
				detail.WarehouseId = Conversion.ToInt(r4.Cells["WarehouseId"].Value);
				if (Conversion.ToInt(r4.Cells["ItemId"].Value) == 0)
				{
					MessageBox.Show("ItemId is required in Detail Grid in Row No: " + (r4.RowIndex + 1) + "Please Check...");
					return;
				}
				detail.ItemId = Conversion.ToInt(r4.Cells["ItemId"].Value);
				detail.CropYear = Conversion.ToString(r4.Cells["CropYear"].Value);
				if (Conversion.ToInt(r4.Cells["PackingTypeId"].Value) == 0)
				{
					MessageBox.Show("PackingType is required in Detail Grid in Row No: " + (r4.RowIndex + 1) + "Please Check...");
					return;
				}
				detail.PackingTypeId = Conversion.ToInt(r4.Cells["PackingTypeId"].Value);
				if (Conversion.ToInt(r4.Cells["PackUOMId"].Value) == 0)
				{
					MessageBox.Show("PackUOM is required in Detail Grid in Row No: " + (r4.RowIndex + 1) + "Please Check...");
					return;
				}
				detail.ItemUOMId = Conversion.ToInt(r4.Cells["PackUOMId"].Value);
				if (Conversion.ToDouble(r4.Cells["ItemQty"].Value) == 0.0)
				{
					MessageBox.Show("ItemQty is required in Detail Grid in Row No: " + (r4.RowIndex + 1) + "Please Check...");
					return;
				}
				detail.ItemQty = Conversion.ToDouble(r4.Cells["ItemQty"].Value);
				if (Conversion.ToDouble(r4.Cells["NetBillWeight"].Value) == 0.0)
				{
					MessageBox.Show("NetBillWeight is required in Detail Grid in Row No: " + (r4.RowIndex + 1) + "Please Check...");
					return;
				}
				detail.GrossWeight = Conversion.ToDouble(r4.Cells["NetBillWeight"].Value);
				GrossWt += detail.GrossWeight;
				detail.NetBillWeight = Conversion.ToDouble(r4.Cells["NetBillWeight"].Value);
				detail.NetStockWeight = Conversion.ToDouble(r4.Cells["NetBillWeight"].Value);
				if (Conversion.ToDouble(r4.Cells["Rate"].Value) == 0.0)
				{
					MessageBox.Show("Rate is required in Detail Grid in Row No: " + (r4.RowIndex + 1) + "Please Check...");
					return;
				}
				detail.ItemRate = Conversion.ToDouble(r4.Cells["Rate"].Value);
				detail.ItemRateWOExp = Conversion.ToDecimal(r4.Cells["Rate"].Value);
				if (Conversion.ToDouble(r4.Cells["CgsRate"].Value) == 0.0 && IsFromLoader)
				{
					MessageBox.Show("CgsRate is required in Detail Grid in Row No: " + (r4.RowIndex + 1) + "Please Check...");
					return;
				}
				detail.ItemCgsRate = Conversion.ToDouble(r4.Cells["CgsRate"].Value);
				detail.UomScheduleIdRate = Conversion.ToInt(r4.Cells["RateUOMId"].Value);
				if (Conversion.ToDouble(r4.Cells["ItemAmount"].Value) == 0.0)
				{
					MessageBox.Show("ItemAmount is required in Detail Grid in Row No: " + (r4.RowIndex + 1) + "Please Check...");
					return;
				}
				detail.ItemAmount = Conversion.ToDouble(r4.Cells["ItemAmount"].Value);
				detail.BillAmount = Conversion.ToDouble(r4.Cells["ItemAmount"].Value);
				detail.GpDate = Conversion.ToDateTime(r4.Cells["GpDate"].Value);
				detail.GpNo = Conversion.ToInt(r4.Cells["GpNo"].Value);
				detail.VehicleNo = Conversion.ToString(r4.Cells["VehicleNo"].Value);
				detail.ExpenseAmount = Conversion.ToDouble(r4.Cells["Expense"].Value);
				detail.JournalAmount = Conversion.ToDouble(r4.Cells["Journal"].Value);
				detail.BillAmount = Math.Round(Conversion.ToDouble(r4.Cells["BillAmount"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				detail.CityId = Conversion.ToInt(r4.Cells["CityId"].Value);
				detail.JobLotId = getJobLotId;
				detail.ReserveWareHouse = 0;
				obj.InvSaleInvoiceDetaillist.Add(detail);
			}
			rows = grdInvExp.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				if (Conversion.ToInt((object)r2.Cells["ItemId"].Value.ToString()) != 0 || Conversion.ToDouble(r2.Cells["Amount"].Value) != 0.0)
				{
					InvSaleInvoiceExpense PE = new InvSaleInvoiceExpense();
					if (Conversion.ToInt(r2.Cells["ItemId"].Value) == 0)
					{
						MessageBox.Show("Item is required in Expense Grid in Row No: " + (r2.RowIndex + 1) + "Please Check...");
						return;
					}
					PE.InvRevExpItemId = Conversion.ToInt(r2.Cells["ItemId"].Value);
					PE.Qty = Conversion.ToInt(r2.Cells["Qty"].Value);
					PE.Rate = Conversion.ToDouble(r2.Cells["Rate"].Value);
					if (Conversion.ToDouble(r2.Cells["Amount"].Value) == 0.0)
					{
						MessageBox.Show("Amount is required in Expense Grid in Row No: " + (r2.RowIndex + 1) + "Please Check...");
						return;
					}
					PE.Amount = Conversion.ToDouble(r2.Cells["Amount"].Value);
					PE.Remarks = Conversion.ToString(r2.Cells["Remarks"].Value);
					obj.InvSaleInvoiceExpenseList.Add(PE);
				}
			}
			rows = grdGLedger.GetRows();
			foreach (GridEXRow r in rows)
			{
				if (Conversion.ToInt((object)r.Cells["AccountId"].Value.ToString()) != 0)
				{
					InvSaleInvoiceJournal PJ = new InvSaleInvoiceJournal();
					PJ.ChartofAccountId = Conversion.ToInt(r.Cells["AccountId"].Value);
					PJ.JvRemarks = Conversion.ToString(r.Cells["Remarks"].Value);
					PJ.JvPrcnt = Conversion.ToDouble(r.Cells["Percentage"].Value);
					PJ.JvQty = Conversion.ToDouble(r.Cells["Qty"].Value);
					PJ.JvRate = Conversion.ToDouble(r.Cells["Rate"].Value);
					PJ.JvDebit = Conversion.ToDouble(r.Cells["Debit"].Value);
					PJ.JvCredit = Conversion.ToDouble(r.Cells["Credit"].Value);
					obj.InvSaleInvoiceJournalList.Add(PJ);
				}
			}
			obj.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			obj.DeleteAttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<InvSaleInvoice>(obj, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
			obj.AttachmentsValues = AttachmentsValues;
			obj.CustomAttachmentsValues = CustomAttachmentsValues;
			int sucess = InvSaleInvoice.Save(obj);
			if (RecId == 0)
			{
				MessageBox.Show("Record Saved Successfully [" + obj.DocNo + "] ");
			}
			else
			{
				MessageBox.Show("Record Update Successfully [" + obj.DocNo + "] ");
				if (AT.RemovedAttachmentListInUpdateCase.Count > 0 && AT.lst.Count == 0)
				{
					global::Architecture.BLL.DMSAttachments.RemoveByIdAndNames(RecId, base.Name, 186, 0);
				}
			}
			if (WagesStatus)
			{
				frmwagesBillHeader frmwagesBillHeader = new frmwagesBillHeader(UserAccount);
				frmwagesBillHeader.RefDocTypeId = 186;
				frmwagesBillHeader.RefDocId = sucess;
				frmwagesBillHeader.GrossWeightTotal = GrossWt;
				frmwagesBillHeader.ShowDialog();
				frmwagesBillHeader.BringToFront();
			}
			Reset();
			if (formright.DoHavePrintRights)
			{
				if (ChkBok.Checked)
				{
					RecId = sucess;
					VoucherHeadIdGet();
					RecId = 0;
					CommonServices.VoucherReport_118(VoucherHeadId, 186);
				}
				if (ChkPrintslip.Checked)
				{
					CommonServices.SaleInvoice_DirectFlour282(sucess);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void saveToolStripButton_Click(object sender, EventArgs e)
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

	public void ReadById(int ID)
	{
		try
		{
			RecId = Conversion.ToInt((object)ID);
			InvSaleInvoice invPI = InvSaleInvoice.GetByID(RecId);
			combranches.Value = invPI.BranchesId;
			comproject.Value = invPI.ProjectsId;
			txtdocno.Text = invPI.DocNo.ToString();
			DocDate.Value = invPI.DocDate;
			tabControl1.SelectedIndex = 0;
			comsupplier.Value = invPI.SupplierCustomerId;
			compartyreference.Value = invPI.ReferencePartyId;
			comstockparty.Value = invPI.StockPartyId;
			txtbillno.Text = invPI.ManualBillNo;
			txtremarks.Text = Conversion.ToString((object)invPI.RemarksHeader);
			txtBillAmount.Text = invPI.BillAmount.ToString(clsGlobalVariables.stringFormatsingle);
			txtduedays.Text = Conversion.ToString((object)invPI.SupplierInvoiceNo);
			duedate.Value = invPI.SupplierInvoiceDate;
			Approved = invPI.IsApproved;
			AttachmentsValues = invPI.AttachmentsValues;
			CustomAttachmentsValues = invPI.CustomAttachmentsValues;
			VoucherHeadIdGet();
			dtGrid.Rows.Clear();
			for (int k = 0; k < invPI.InvSaleInvoiceDetaillist.Count; k++)
			{
				dtGrid.Rows.Add(invPI.InvSaleInvoiceDetaillist[k].Id, invPI.InvSaleInvoiceDetaillist[k].RefRefDocumentTypeId, invPI.InvSaleInvoiceDetaillist[k].RefRefDocIdNo, invPI.InvSaleInvoiceDetaillist[k].RefDocSubId, invPI.InvSaleInvoiceDetaillist[k].SaleOrderId, invPI.InvSaleInvoiceDetaillist[k].SaleOrderDetailId, invPI.InvSaleInvoiceDetaillist[k].SaleOrder, invPI.InvSaleInvoiceDetaillist[k].InvGdnId, invPI.InvSaleInvoiceDetaillist[k].InvGdnDetailId, invPI.InvSaleInvoiceDetaillist[k].ForwardingDocNo, invPI.InvSaleInvoiceDetaillist[k].WarehouseId, invPI.InvSaleInvoiceDetaillist[k].WareHouseName, invPI.InvSaleInvoiceDetaillist[k].ItemId, invPI.InvSaleInvoiceDetaillist[k].ItemName, invPI.InvSaleInvoiceDetaillist[k].ItemCode, invPI.InvSaleInvoiceDetaillist[k].CropYear, invPI.InvSaleInvoiceDetaillist[k].PackingTypeId, invPI.InvSaleInvoiceDetaillist[k].PackTypeDesc, invPI.InvSaleInvoiceDetaillist[k].ItemUOMId, invPI.InvSaleInvoiceDetaillist[k].PackUom, invPI.InvSaleInvoiceDetaillist[k].ItemQty, invPI.InvSaleInvoiceDetaillist[k].ItemQty, invPI.InvSaleInvoiceDetaillist[k].NetBillWeight, invPI.InvSaleInvoiceDetaillist[k].NetBillWeight, invPI.InvSaleInvoiceDetaillist[k].ItemRate, invPI.InvSaleInvoiceDetaillist[k].ItemCgsRate, invPI.InvSaleInvoiceDetaillist[k].UomScheduleIdRate, invPI.InvSaleInvoiceDetaillist[k].RateUOM, invPI.InvSaleInvoiceDetaillist[k].ItemAmount, invPI.InvSaleInvoiceDetaillist[k].GpDate, invPI.InvSaleInvoiceDetaillist[k].GpNo, invPI.InvSaleInvoiceDetaillist[k].VehicleNo, invPI.InvSaleInvoiceDetaillist[k].BillAmount, invPI.InvSaleInvoiceDetaillist[k].ExpenseAmount, invPI.InvSaleInvoiceDetaillist[k].JournalAmount, invPI.InvSaleInvoiceDetaillist[k].CityId, invPI.InvSaleInvoiceDetaillist[k].CityName);
				if (invPI.InvSaleInvoiceDetaillist[k].RefRefDocumentTypeId > 0 || invPI.InvSaleInvoiceDetaillist[k].RefDocSubId > 0 || invPI.InvSaleInvoiceDetaillist[k].RefRefDocIdNo > 0)
				{
					IsFromLoader = true;
				}
				else
				{
					IsFromLoader = false;
				}
			}
			dtInvExp.Rows.Clear();
			for (int j = 0; j < invPI.InvSaleInvoiceExpenseList.Count; j++)
			{
				dtInvExp.Rows.Add(invPI.InvSaleInvoiceExpenseList[j].InvRevExpItemId, invPI.InvSaleInvoiceExpenseList[j].Qty, invPI.InvSaleInvoiceExpenseList[j].Rate, invPI.InvSaleInvoiceExpenseList[j].Amount, invPI.InvSaleInvoiceExpenseList[j].Remarks);
			}
			dtGrdGL.Rows.Clear();
			for (int i = 0; i < invPI.InvSaleInvoiceJournalList.Count; i++)
			{
				dtGrdGL.Rows.Add(invPI.InvSaleInvoiceJournalList[i].ChartofAccountId, invPI.InvSaleInvoiceJournalList[i].JvRemarks, invPI.InvSaleInvoiceJournalList[i].JvPrcnt, invPI.InvSaleInvoiceJournalList[i].JvQty, invPI.InvSaleInvoiceJournalList[i].JvRate, invPI.InvSaleInvoiceJournalList[i].JvDebit, invPI.InvSaleInvoiceJournalList[i].JvCredit);
			}
			if (dtInvExp.Rows.Count == 0)
			{
				AddRowInvExpGrid();
			}
			grdInvExp.DataSource = dtInvExp;
			grdInvExp.RetrieveStructure();
			if (dtGrdGL.Rows.Count == 0)
			{
				AddRowInGLGrid();
			}
			grdGLedger.DataSource = dtGrdGL;
			grdGLedger.RetrieveStructure();
			grd.DataSource = dtGrid;
			grd.RetrieveStructure();
			grdSettings();
			grdInvExpSettings();
			gridGLSettings();
			ExpProportion();
			BillAmount();
			tabControl1.SelectedIndex = 0;
			FormHelper.LoadAttachmentsForObject(AT, RecId, base.Name);
			btnSave.Visible = false;
			btnUpdate.Visible = true;
			btnDelete.Visible = true;
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
			if (Approved)
			{
				throw new Exception("Record Not Update because Record has approved");
			}
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnDelete_Click(object sender, EventArgs e)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Expected O, but got Unknown
		try
		{
			if (Approved)
			{
				throw new Exception("Record Not Delete because Record has approved");
			}
			if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				InvSaleInvoice.RemoveByID(new InvSaleInvoice
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					DocumentTypeId = 186,
					Id = RecId,
					EntryUser = UserAccount.ID
				});
				MessageBox.Show("Delete Voucher Successfully");
				Reset();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtdocno_TextChanged(object sender, EventArgs e)
	{
	}

	public void BillAmount()
	{
		try
		{
			double BillAmount = 0.0;
			double ItemAmount = Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["ItemAmount"], (AggregateFunction)2).ToString());
			double ExpAmount = Conversion.ToDouble((object)grdInvExp.GetTotal(grdInvExp.RootTable.Columns["Amount"], (AggregateFunction)2).ToString());
			double JournalDebit = 0.0;
			double JournalCredit = 0.0;
			GridEXRow[] rows = grdGLedger.GetRows();
			foreach (GridEXRow r in rows)
			{
				if (Conversion.ToInt((object)r.Cells["AccountId"].Value.ToString()) > 0)
				{
					JournalDebit += Conversion.ToDouble((object)r.Cells["Debit"].Value.ToString());
					JournalCredit += Conversion.ToDouble((object)r.Cells["Credit"].Value.ToString());
					SupplierGLDebitAmount = JournalDebit;
					SupplierGLCreditAmount = JournalCredit;
				}
			}
			ItemAmount = Math.Round(ItemAmount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
			ExpAmount = Math.Round(ExpAmount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
			JournalDebit = Math.Round(JournalDebit, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
			JournalCredit = Math.Round(JournalCredit, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
			BillAmount = ItemAmount + ExpAmount;
			BillAmount = BillAmount + JournalCredit - JournalDebit;
			SupplierCommissionAmount = 0.0;
			txtBillAmount.Text = Math.Round(BillAmount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero).ToString(clsGlobalVariables.stringFormatsingle);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ExpProportion()
	{
		try
		{
			double TotalExpAmount = Conversion.ToDouble((object)grdInvExp.GetTotal(grdInvExp.RootTable.Columns["Amount"], (AggregateFunction)2).ToString());
			double NetWeight = Conversion.ToDouble((object)grd.GetTotal(grd.RootTable.Columns["NetBillWeight"], (AggregateFunction)2).ToString());
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow obj in rows)
			{
				obj.BeginEdit();
				double Weight = Conversion.ToDouble((object)obj.Cells["NetBillWeight"].Value.ToString());
				obj.Cells["Expense"].Value = Math.Round(TotalExpAmount / NetWeight * Weight, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
			}
			BillProportion();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BillProportion()
	{
		try
		{
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow r in rows)
			{
				double itemAmount = Math.Round(Conversion.ToDouble(r.Cells["ItemAmount"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				Math.Round(Conversion.ToDouble(r.Cells["Journal"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				double Expense = Math.Round(Conversion.ToDouble(r.Cells["Expense"].Value), clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				r.BeginEdit();
				List<ConfigrationsAllocation> lst = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "CreditAmountInItemSaleGL").ToList();
				if (lst.Count > 0)
				{
					if (!Conversion.ToBool((object)lst[0].ConfigKey))
					{
						r.Cells["BillAmount"].Value = itemAmount;
					}
					else
					{
						r.Cells["BillAmount"].Value = Math.Round(itemAmount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
					}
				}
				else
				{
					r.Cells["BillAmount"].Value = Math.Round(itemAmount + Expense, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				}
				r.EndEdit();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Total()
	{
		try
		{
			double PackUOM = 0.0;
			double Qty = 0.0;
			double NetWeight = 0.0;
			if (((Control)(object)comPackUOM).Text != string.Empty || Conversion.ToInt(comPackUOM.Value) != 0)
			{
				PackUOM = Conversion.ToDouble(((UltraDropDownBase)comPackUOM).SelectedRow.Cells[2].Value);
			}
			if (txtQty.Text != string.Empty && txtQty.Text != null)
			{
				Qty = Conversion.ToDouble((object)txtQty.Text.Trim().ToString());
			}
			if (PackUOM > 0.0 && Qty > 0.0)
			{
				txtNetBillWeight.Text = (PackUOM * Qty).ToString("##,#.##");
			}
			else
			{
				txtNetBillWeight.Text = 0.ToString("##,#.##");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DetailAmountCaluculation()
	{
		try
		{
			double RateUOM = 0.0;
			double Rate = 0.0;
			double NetWeight = 0.0;
			double ItemAmount = 0.0;
			RateUOM = ((((UltraGridBase)comRateUOM).ActiveRow == null) ? 0.0 : Conversion.ToDouble(((UltraDropDownBase)comRateUOM).SelectedRow.Cells[2].Value));
			if (Conversion.ToDouble((object)txtRate.Text) != 0.0)
			{
				Rate = Conversion.ToDouble((object)txtRate.Text);
			}
			if (Conversion.ToDouble((object)txtNetBillWeight.Text) != 0.0)
			{
				NetWeight = Conversion.ToDouble((object)txtNetBillWeight.Text);
			}
			if (NetWeight > 0.0 && RateUOM > 0.0 && Rate > 0.0)
			{
				ItemAmount = NetWeight / RateUOM * Rate;
				ItemAmount = Math.Round(ItemAmount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
				txtAmount.Text = ItemAmount.ToString(clsGlobalVariables.stringFormatsingle);
			}
			else
			{
				txtAmount.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private int GetCursorOffset(string formattedText, string originalText, int originalCursorPosition)
	{
		int length = formattedText.Length;
		int originalLength = originalText.Length;
		int cursorOffset = length - originalLength;
		if (originalCursorPosition == originalLength)
		{
			return cursorOffset;
		}
		int originalCursorOffset = originalLength - originalCursorPosition;
		return Math.Max(cursorOffset - originalCursorOffset, 0);
	}

	private void comPackUOM_Leave(object sender, EventArgs e)
	{
		try
		{
			Total();
			DetailAmountCaluculation();
			BillAmount();
			AvailableStockGetByItem();
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
			CommonServices.CommasApplyWhileTyping(sender, e);
			Total();
			DetailAmountCaluculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void comRateUOM_Leave(object sender, EventArgs e)
	{
		DetailAmountCaluculation();
	}

	private void txtRate_TextChanged(object sender, EventArgs e)
	{
		CommonServices.CommasApplyWhileTyping(sender, e);
		DetailAmountCaluculation();
	}

	private void comsupplier_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			BillAmount();
			if (dtSupplier.Rows.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < dtSupplier.Rows.Count; i++)
			{
				if (Conversion.ToInt(dtSupplier.Rows[i]["Id"]) == Conversion.ToInt(comsupplier.Value))
				{
					txtSupplierGLId.Text = dtSupplier.Rows[i]["GlAccountId"].ToString();
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
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Expected O, but got Unknown
		try
		{
			int Id = Conversion.ToInt(CmbCustomerHistory.Value);
			DataTable dt = InvSaleInvoice.AllComboBindAgainstSaleInvoice(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				AppId = UserAccount.AppId,
				UserId = UserAccount.ID,
				DocumentTypeIds = "186"
			});
			if (dt.Rows.Count > 0)
			{
				DataTable dtCustomer = new DataTable();
				dtCustomer.Columns.Add("Id", typeof(int));
				dtCustomer.Columns.Add("CustomerName");
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToString(dt.Rows[i]["Activity"]) == "Supplier")
					{
						dtCustomer.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
				}
				if (dtCustomer.Rows.Count > 0)
				{
					DDL.BindDDL(dtCustomer, CmbCustomerHistory, "Id", "CustomerName", "Customer Name", ZeroIndex: true);
					if (Id > 0)
					{
						if (dtCustomer.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
						{
							CmbCustomerHistory.Value = Id;
						}
						else
						{
							((Control)(object)CmbCustomerHistory).Text = string.Empty;
						}
					}
				}
				else
				{
					((UltraGridBase)CmbCustomerHistory).DataSource = null;
				}
			}
			else
			{
				((UltraGridBase)CmbCustomerHistory).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ParameterFill()
	{
		try
		{
			DataTable dt = CommonServices.DateType();
			DDL.BindDDL(dt, cmbDateTypeHistory, "Id", "Parameters", "Parameters", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbDateTypeHistory_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (Convert.ToInt32(cmbDateTypeHistory.Value) == 1)
			{
				txtFromdateHistory.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbDateTypeHistory.Value) == 2)
			{
				txtFromdateHistory.Value = DateTime.Today.AddDays(-7.0);
			}
			else if (Convert.ToInt32(cmbDateTypeHistory.Value) == 3)
			{
				DateTime now = DateTime.UtcNow;
				DateTime first = now.Date.AddDays(-(now.Date.Day - 1));
				txtFromdateHistory.Value = first;
				txtToDateHistory.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbDateTypeHistory.Value) == 4)
			{
				int year = DateTime.Now.Year;
				DateTime first2 = new DateTime(year, 1, 1);
				txtFromdateHistory.Value = first2;
				txtToDateHistory.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbDateTypeHistory.Value) == 5)
			{
				txtFromdateHistory.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			}
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
			GetAll();
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
			HistoryComboBind();
			ParameterFill();
			cmbDateTypeHistory.Focus();
			if (Conversion.ToInt(cmbDateTypeHistory.Value) == 0)
			{
				((UltraGridBase)cmbDateTypeHistory).Rows[0].Activate();
			}
		}
	}

	private void Resethistory()
	{
		try
		{
			cmbDateTypeHistory.Value = 3;
			txtFromNoHistory.Text = string.Empty;
			txtToDocNoHistory.Text = string.Empty;
			((Control)(object)CmbCustomerHistory).Text = string.Empty;
			cmbDateTypeHistory.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnLoadAll_Click(object sender, EventArgs e)
	{
		try
		{
			Resethistory();
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
			HistoryComboBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetAll()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters PI = new ReportsParameters();
			PI.OrganizationId = UserAccount.OrganizationId;
			PI.CompanyId = UserAccount.CompanyId;
			PI.BranchesId = UserAccount.BranchesId;
			PI.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			PI.DocumentTypeId = 186;
			PI.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			if (drdocdate.Checked)
			{
				if (txtFromdateHistory.Checked)
				{
					PI.FromDate = txtFromdateHistory.Value;
				}
				if (txtToDateHistory.Checked)
				{
					PI.ToDate = txtToDateHistory.Value;
				}
			}
			else if (rdentrydate.Checked)
			{
				if (txtFromdateHistory.Checked)
				{
					PI.EntryDateFrom = txtFromdateHistory.Value;
				}
				if (txtToDateHistory.Checked)
				{
					PI.EntryDateTo = txtToDateHistory.Value;
				}
			}
			else if (rdmodifydate.Checked)
			{
				if (txtFromdateHistory.Checked)
				{
					PI.ModifyDateFrom = txtFromdateHistory.Value;
				}
				if (txtToDateHistory.Checked)
				{
					PI.ModifyDateTo = txtToDateHistory.Value;
				}
			}
			else if (rdapproveddate.Checked)
			{
				if (txtFromdateHistory.Checked)
				{
					PI.ApprovedDateFrom = txtFromdateHistory.Value;
				}
				if (txtToDateHistory.Checked)
				{
					PI.ApprovedDateTo = txtToDateHistory.Value;
				}
			}
			PI.FromDocNo = Conversion.ToInt((object)txtFromNoHistory.Text);
			PI.ToDocNo = Conversion.ToInt((object)txtToDocNoHistory.Text);
			PI.SupplierCustomerId = Conversion.ToInt(CmbCustomerHistory.Value);
			if (!PI.CanViewAllRecord)
			{
				PI.EntryUser = UserAccount.ID;
			}
			DataTable dthistory = InvSaleInvoice.FormHistory(PI);
			if (dthistory.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("VoucherHeadId", typeof(int));
				dt.Columns.Add("DocNo", typeof(int));
				dt.Columns.Add("DocDate", typeof(DateTime));
				dt.Columns.Add("SupplierCustomerId", typeof(int));
				dt.Columns.Add("CustomerName", typeof(string));
				dt.Columns.Add("ManualBillNo", typeof(string));
				dt.Columns.Add("DeliveryTerm", typeof(string));
				dt.Columns.Add("PaymentTerm", typeof(string));
				dt.Columns.Add("DueDays", typeof(double));
				dt.Columns.Add("DueDate", typeof(DateTime));
				dt.Columns.Add("BillAmount", typeof(double));
				dt.Columns.Add("EntryDate", typeof(DateTime));
				dt.Columns.Add("EntryUser", typeof(string));
				dt.Columns.Add("ModifyDate", typeof(DateTime));
				dt.Columns.Add("ModifyUser", typeof(string));
				dt.Columns.Add("NoOfAttachments", typeof(int));
				dt.Columns.Add("Remarks", typeof(string));
				for (int i = 0; i < dthistory.Rows.Count; i++)
				{
					dt.Rows.Add(dthistory.Rows[i]["Id"], dthistory.Rows[i]["VoucherHeadId"], dthistory.Rows[i]["DocNo"], Conversion.ToDateTime(dthistory.Rows[i]["DocDate"]).ToShortDateString(), dthistory.Rows[i]["SupplierCustomerId"], dthistory.Rows[i]["CustomerName"], dthistory.Rows[i]["ManualBillNo"], dthistory.Rows[i]["DeliveryTerm"], dthistory.Rows[i]["PaymentTerm"], dthistory.Rows[i]["DueDays"], Conversion.ToDateTime(dthistory.Rows[i]["DueDate"]).ToShortDateString(), dthistory.Rows[i]["BillAmount"], Conversion.ToDateTime(dthistory.Rows[i]["EntryDate"]).ToShortDateString(), dthistory.Rows[i]["UserName"], Conversion.ToDateTime(dthistory.Rows[i]["ModifyDate"]).ToShortDateString(), dthistory.Rows[i]["ModifyUserName"], dthistory.Rows[i]["NoOfAttachments"], dthistory.Rows[i]["RemarksHeader"]);
				}
				grdHistory.DataSource = dt;
				grdHistory.RetrieveStructure();
				HistoryGridSettings();
			}
			else
			{
				grdHistory.ClearStructure();
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
			grdHistory.AllowEdit = (InheritableBoolean)2;
			grdHistory.AllowDelete = (InheritableBoolean)2;
			grdHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdHistory.RootTable.Columns["Id"].Visible = false;
			grdHistory.RootTable.Columns["VoucherHeadId"].Visible = false;
			grdHistory.RootTable.Columns["SupplierCustomerId"].Visible = false;
			grdHistory.RootTable.Columns["BillAmount"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["BillAmount"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["BillAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdHistory.RootTable.Columns["BillAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustmentNew(grdHistory);
			grdHistory.RootTable.Columns.Add("View");
			grdHistory.RootTable.Columns["View"].Caption = "Slip_282";
			grdHistory.RootTable.Columns["View"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["View"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["View"].ButtonText = "Slip_282";
			grdHistory.RootTable.Columns["View"].Width = 70;
			grdHistory.RootTable.Columns.Add("Edit");
			grdHistory.RootTable.Columns["Edit"].Caption = "Edit";
			grdHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdHistory.RootTable.Columns["Edit"].Width = 50;
			grdHistory.RootTable.Columns.Add("Voucher");
			grdHistory.RootTable.Columns["Voucher"].Caption = "Voucher";
			grdHistory.RootTable.Columns["Voucher"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["Voucher"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["Voucher"].ButtonText = "Voucher";
			grdHistory.RootTable.Columns["Voucher"].Width = 60;
			grdHistory.RootTable.Columns.Add("PrintTwo");
			grdHistory.RootTable.Columns["PrintTwo"].Key = "PrintTwo";
			grdHistory.RootTable.Columns["PrintTwo"].Caption = "Slip_282A";
			grdHistory.RootTable.Columns["PrintTwo"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["PrintTwo"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["PrintTwo"].ButtonText = "Slip_282A";
			grdHistory.RootTable.Columns["PrintTwo"].Width = 70;
			grdHistory.RootTable.Columns["View"].Position = 0;
			grdHistory.RootTable.Columns["PrintTwo"].Position = 1;
			grdHistory.RootTable.Columns["Voucher"].Position = 2;
			grdHistory.RootTable.Columns["Edit"].Position = 3;
			grdHistory.FrozenColumns = 4;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void grdHistory_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			RecId = Conversion.ToInt(grdHistory.CurrentRow.Cells["Id"].Value);
			ReadById(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdHistory.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(item.Cells["Id"].Value), 186);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Edit")
			{
				Reset();
				RecId = Conversion.ToInt(grdHistory.CurrentRow.Cells["Id"].Value);
				ReadById(RecId);
			}
			if (e.Column.Key == "View")
			{
				CommonServices.SaleInvoice_DirectFlour282(Conversion.ToInt(grdHistory.CurrentRow.Cells["Id"].Value));
			}
			if (e.Column.Key == "PrintTwo")
			{
				CommonServices.SaleInvoice_DirectFlour_282A(Conversion.ToInt(grdHistory.CurrentRow.Cells["Id"].Value));
			}
			if (e.Column.Key == "Voucher")
			{
				CommonServices.AcRptPurchaseSalesVoucherSlip_103(Conversion.ToInt(grdHistory.CurrentRow.Cells["VoucherHeadId"].Value), 186);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdHistory_SelectionChanged(object sender, EventArgs e)
	{
		try
		{
			GetDetailGrdByHeadId();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetDetailGrdByHeadId()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		try
		{
			if (grdHistory.CurrentRow == null || (int)grdHistory.CurrentRow.RowType != 0 || grdHistory.CurrentRow.Cells.Count <= 0)
			{
				return;
			}
			InvSaleInvoice obj = new InvSaleInvoice();
			obj = InvSaleInvoice.GetByID(Conversion.ToInt(grdHistory.CurrentRow.Cells["Id"].Value));
			if (obj != null && obj.InvSaleInvoiceDetaillist.Count > 0)
			{
				DataTable dtdetail = new DataTable();
				dtdetail.Columns.Add("Id", typeof(int));
				dtdetail.Columns.Add("SaleOrderNo", typeof(int));
				dtdetail.Columns.Add("GdnNo", typeof(int));
				dtdetail.Columns.Add("Warehouse", typeof(string));
				dtdetail.Columns.Add("ItemId", typeof(int));
				dtdetail.Columns.Add("PurchaseGLAC", typeof(int));
				dtdetail.Columns.Add("Item", typeof(string));
				dtdetail.Columns.Add("ItemCode", typeof(string));
				dtdetail.Columns.Add("CropYear", typeof(string));
				dtdetail.Columns.Add("PackingType", typeof(string));
				dtdetail.Columns.Add("PackUOM", typeof(string));
				dtdetail.Columns.Add("ItemQty", typeof(double));
				dtdetail.Columns.Add("NetBillWeight", typeof(double));
				dtdetail.Columns.Add("Rate", typeof(double));
				dtdetail.Columns.Add("RateUOM", typeof(string));
				dtdetail.Columns.Add("ItemAmount", typeof(double));
				dtdetail.Columns.Add("GpDate", typeof(DateTime));
				dtdetail.Columns.Add("GpNo", typeof(int));
				dtdetail.Columns.Add("VehicleNo", typeof(string));
				dtdetail.Columns.Add("BillAmount", typeof(double));
				dtdetail.Columns.Add("Expense", typeof(double));
				dtdetail.Columns.Add("Journal", typeof(double));
				dtdetail.Columns.Add("CityName", typeof(string));
				dtdetail.Rows.Clear();
				for (int i = 0; i < obj.InvSaleInvoiceDetaillist.Count; i++)
				{
					dtdetail.Rows.Add(obj.InvSaleInvoiceDetaillist[i].Id, obj.InvSaleInvoiceDetaillist[i].ForwardingDocNo, obj.InvSaleInvoiceDetaillist[i].SaleOrder, obj.InvSaleInvoiceDetaillist[i].WareHouseName, obj.InvSaleInvoiceDetaillist[i].ItemId, obj.InvSaleInvoiceDetaillist[i].SaleGLAC, obj.InvSaleInvoiceDetaillist[i].ItemName, obj.InvSaleInvoiceDetaillist[i].ItemCode, obj.InvSaleInvoiceDetaillist[i].CropYear, obj.InvSaleInvoiceDetaillist[i].PackTypeDesc, obj.InvSaleInvoiceDetaillist[i].UOMCodeItem, obj.InvSaleInvoiceDetaillist[i].ItemQty, obj.InvSaleInvoiceDetaillist[i].NetBillWeight, obj.InvSaleInvoiceDetaillist[i].ItemRate, obj.InvSaleInvoiceDetaillist[i].UOMCodeRate, obj.InvSaleInvoiceDetaillist[i].ItemAmount, obj.InvSaleInvoiceDetaillist[i].GpDate, obj.InvSaleInvoiceDetaillist[i].GpNo, obj.InvSaleInvoiceDetaillist[i].VehicleNo, obj.InvSaleInvoiceDetaillist[i].BillAmount, obj.InvSaleInvoiceDetaillist[i].ExpenseAmount, obj.InvSaleInvoiceDetaillist[i].JournalAmount, obj.InvSaleInvoiceDetaillist[i].CityName);
				}
				grdDetail.DataSource = dtdetail;
				grdDetail.RetrieveStructure();
				DetailGridSetting();
			}
			else
			{
				grdDetail.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DetailGridSetting()
	{
		try
		{
			grdDetail.RootTable.Columns["Id"].Visible = false;
			grdDetail.RootTable.Columns["ItemId"].Visible = false;
			grdDetail.RootTable.Columns["GdnNo"].Visible = false;
			grdDetail.RootTable.Columns["SaleOrderNo"].Visible = false;
			grdDetail.RootTable.Columns["PurchaseGLAC"].Visible = false;
			grdDetail.RootTable.Columns["BillAmount"].Caption = "Item Net Amount";
			grdDetail.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["ItemQty"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["NetBillWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["NetBillWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["NetBillWeight"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["NetBillWeight"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdDetail.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["ItemAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["ItemAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["BillAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["BillAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["BillAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["BillAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["Expense"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["Expense"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["Expense"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["Expense"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["Journal"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["Journal"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["Journal"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["Journal"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustmentNew(grdDetail);
			ctrlGrdBar3_Load(null, null);
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
			CommonServices.AcRptPurchaseSalesVoucherSlip_103(VoucherHeadId, 186);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnSlip_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.SaleInvoice_DirectFlour282(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btn294APrint_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.SaleInvoice_DirectFlour_282A(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAttachment_Click(object sender, EventArgs e)
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdHistory).Name);
			if (fs != null)
			{
				grdHistory.LoadLayoutFile((Stream)fs);
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdDetail).Name);
			if (fs != null)
			{
				grdDetail.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmSaleInvoiceDirectFlourAutoRated_KeyDown(object sender, KeyEventArgs e)
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
			if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
			}
			if (tabControl1.SelectedIndex == 0)
			{
				if (e.Control && e.KeyCode == Keys.S && btnSave.Visible && btnSave.Enabled)
				{
					saveToolStripButton_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.L)
				{
					btnLoadSaleOrder_Click(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.U)
				{
					if (btnUpdate.Visible && btnUpdate.Enabled)
					{
						btnUpdate_Click(null, null);
					}
				}
				else if (e.Control && e.KeyCode == Keys.N)
				{
					btnNew_Click(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefresh_Click(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.F5)
				{
					DocDate.Focus();
				}
				else if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grd).Focus();
				}
				else if (e.Control && e.KeyCode == Keys.Left)
				{
					if (((Control)(object)grd).Focused)
					{
						((Control)(object)grdInvExp).Focus();
					}
					else if (((Control)(object)grdInvExp).Focused)
					{
						((Control)(object)grd).Focus();
					}
				}
				else if (e.Control && e.KeyCode == Keys.Right)
				{
					if (((Control)(object)grd).Focused)
					{
						((Control)(object)grdInvExp).Focus();
					}
					else if (((Control)(object)grdInvExp).Focused)
					{
						((Control)(object)grd).Focus();
					}
				}
				else if (e.Control && e.KeyCode == Keys.Up)
				{
					comWarehouse.Focus();
				}
				else if (e.Control && e.KeyCode == Keys.F10)
				{
					AT.Show();
				}
				else if (e.Alt && (e.KeyCode == Keys.NumPad1 || e.KeyCode == Keys.D1))
				{
					btnSlip_Click(null, null);
				}
				else if (e.Alt && (e.KeyCode == Keys.NumPad2 || e.KeyCode == Keys.D2))
				{
					btn294APrint_Click(null, null);
				}
				else if (e.Alt && (e.KeyCode == Keys.NumPad3 || e.KeyCode == Keys.D3))
				{
					btnPrint_Click(null, null);
				}
			}
			else if (e.Control && e.KeyCode == Keys.F5)
			{
				((Control)(object)grdHistory).Focus();
			}
			else if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grdHistory).Focus();
			}
			else if (e.Control && e.KeyCode == Keys.Right)
			{
				if (((Control)(object)grdDetail).Focused)
				{
					((Control)(object)grdHistory).Focus();
				}
				else
				{
					((Control)(object)grdHistory).Focus();
				}
			}
			else if (e.Control && e.KeyCode == Keys.Up)
			{
				((Control)(object)grdHistory).Focus();
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
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+L", "To Open Load From Issuance");
			dt.Rows.Add("Ctrl+S", "For Save");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+Shift+Delete", "For Delete");
			dt.Rows.Add("Alt+1", "For Print Slip 282");
			dt.Rows.Add("Alt+2", "For Print Slip 282A");
			dt.Rows.Add("Alt+3", "For Print Voucher 103");
			dt.Rows.Add("Ctrl+F5", "For Focus on Doc Date");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+D", "For Adding an row in Focused Grid");
			dt.Rows.Add("Ctrl+Delete", "For Deleting an row of Focused Grid");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On WareHouse Name in Detail Box");
			dt.Rows.Add("Ctrl+ArrowRight", "For Change Focus from one Grid To another Grid");
			dt.Rows.Add("Ctrl+ArrowLeft", "For Change Focus from one Grid To another Grid");
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

	private void grd_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
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
					dtGrid.Rows[r.RowIndex].Delete();
					if (r != null && (int)r.RowType == 0)
					{
						r.Delete();
					}
				}
				ExpProportion();
				BillAmount();
				grd.UpdateData();
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				dtGrid.Rows[r.RowIndex].Delete();
				if (r != null && (int)r.RowType == 0)
				{
					r.Delete();
				}
				ExpProportion();
				BillAmount();
				grd.UpdateData();
			}
			if (e.KeyCode == Keys.F1)
			{
				int Id = 0;
				_ = grd.CurrentColumn.Key == "RateEquivalent";
				if (grd.CurrentColumn.Key == "DiscountType")
				{
					GrdPopUp grdPopUp = new GrdPopUp(dtDiscountType, "Id", "DiscountType");
					grdPopUp.StartPosition = FormStartPosition.CenterScreen;
					grdPopUp.ShowDialog();
					grdPopUp.BringToFront();
					Id = Conversion.ToInt((object)grdPopUp.ReturnId);
					string IName2 = Conversion.ToString((object)grdPopUp.ReturnName);
					r.Cells["DiscountTypeId"].Value = Id;
					r.Cells["DiscountTypeId"].Text = Conversion.ToString((object)Id);
					r.Cells["DiscountType"].Value = IName2;
					r.Cells["DiscountType"].Text = IName2;
					double AmtWithoutDisc = Conversion.ToDouble(r.Cells["ItemAmountWithoutDisc"].Value);
					double DisRate = 0.0;
					double DisAmt = 0.0;
					if (Conversion.ToInt(r.Cells["DiscountTypeId"].Value) != 0)
					{
						DisRate = Conversion.ToDouble(r.Cells["DiscountRate"].Value);
						if (Conversion.ToInt(r.Cells["DiscountTypeId"].Value) == 1)
						{
							DisAmt = DisRate;
							r.Cells["DiscountAmount"].Value = Math.Round(DisAmt, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
						}
						else if (Conversion.ToInt(r.Cells["DiscountTypeId"].Value) == 2)
						{
							if (DisRate >= 100.0)
							{
								MessageBox.Show("DiscountRate Cannot be greater than 99 when Discount Type is Percent");
								DisRate = 99.0;
								r.Cells["DiscountRate"].Value = DisRate;
							}
							DisAmt = AmtWithoutDisc * DisRate / 100.0;
							r.Cells["DiscountAmount"].Value = Math.Round(DisAmt, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
						}
					}
					else
					{
						r.Cells["DiscountAmount"].Value = 0;
					}
					if (AmtWithoutDisc > 0.0 && DisAmt > 0.0)
					{
						r.Cells["ItemAmount"].Value = Math.Round(AmtWithoutDisc - DisAmt, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
					}
					else if (AmtWithoutDisc > 0.0)
					{
						r.Cells["ItemAmount"].Value = Math.Round(AmtWithoutDisc, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero);
					}
					else
					{
						r.Cells["ItemAmount"].Value = 0;
					}
					grd.UpdateData();
					ExpProportion();
					BillAmount();
				}
				if (grd.CurrentColumn.Key == "CityName")
				{
					GrdPopUp grdPopUp2 = new GrdPopUp(dtCity, "Id", "CityName");
					grdPopUp2.StartPosition = FormStartPosition.CenterScreen;
					grdPopUp2.ShowDialog();
					grdPopUp2.BringToFront();
					Id = Conversion.ToInt((object)grdPopUp2.ReturnId);
					string IName = Conversion.ToString((object)grdPopUp2.ReturnName);
					r.Cells["CityName"].Value = IName;
					r.Cells["CityId"].Value = Id;
				}
			}
			if (grd.RowCount == 0)
			{
				IsFromLoader = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdInvExp_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdInvExp.CurrentRow;
			if (item != null && (int)item.RowType == 0 && grdInvExp.CurrentColumn != null)
			{
				if (e.Control && e.KeyCode == Keys.Space)
				{
					if (grdInvExp.CurrentColumn.Key == "Delete")
					{
						dtInvExp.Rows[item.RowIndex].Delete();
						if (item != null && (int)item.RowType == 0)
						{
							item.Delete();
						}
						if (dtInvExp.Rows.Count == 0)
						{
							AddRowInvExpGrid();
						}
					}
					if (grdInvExp.CurrentColumn.Key == "Add")
					{
						AddRowInvExpGrid();
					}
				}
				if (e.Control && e.KeyCode == Keys.Delete)
				{
					dtInvExp.Rows[item.RowIndex].Delete();
					if (item != null && (int)item.RowType == 0)
					{
						item.Delete();
					}
					if (dtInvExp.Rows.Count == 0)
					{
						AddRowInvExpGrid();
					}
				}
			}
			if (e.Control && e.KeyCode == Keys.D)
			{
				AddRowInvExpGrid();
			}
			ExpProportion();
			BillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdHistory.CurrentRow;
			if (item == null || (int)item.RowType != 0 || grdHistory.CurrentColumn == null)
			{
				return;
			}
			int Id = Conversion.ToInt((object)grdHistory.CurrentRow.Cells["Id"].Value.ToString());
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdHistory.CurrentColumn.Key == "Edit")
				{
					RecId = Conversion.ToInt((object)grdHistory.CurrentRow.Cells["Id"].Value.ToString());
					ReadById(RecId);
					DocDate.Focus();
				}
				if (grdHistory.CurrentColumn.Key == "View" && formright.DoHavePrintRights)
				{
					CommonServices.SaleInvoice_DirectFlour282(Id);
				}
				if (grdHistory.CurrentColumn.Key == "PrintTwo" && formright.DoHavePrintRights)
				{
					CommonServices.SaleInvoice_DirectFlour_282A(RecId);
				}
				if (grdHistory.CurrentColumn.Key == "Voucher" && formright.DoHavePrintRights)
				{
					VoucherHeadId = Conversion.ToInt((object)CommonServices.VoucherHeadIdGet(Id, 186));
					CommonServices.VoucherReport_118(VoucherHeadId, 186);
				}
				if (grdHistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt((object)Id.ToString()), 186);
				}
			}
			else if (e.Control && e.KeyCode == Keys.Return)
			{
				if (formright.DoHaveUpdateRights)
				{
					RecId = Conversion.ToInt((object)grdHistory.CurrentRow.Cells["Id"].Value.ToString());
					ReadById(RecId);
					DocDate.Focus();
				}
			}
			else if (e.Control && e.KeyCode == Keys.P && formright.DoHavePrintRights)
			{
				CommonServices.SaleInvoice_DirectFlour282(Id);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtduedays_TextChanged(object sender, EventArgs e)
	{
		try
		{
			DueDateGenerate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DueDateGenerate()
	{
		try
		{
			if (txtduedays.Text.Trim() != string.Empty && txtduedays.Text != null)
			{
				duedate.Value = DocDate.Value.AddDays(Conversion.ToDouble((object)txtduedays.Text.Trim()));
			}
			else
			{
				duedate.Value = DocDate.Value;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DocDate_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			DueDateGenerate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtQty_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtRate_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtduedays_KeyPress(object sender, KeyPressEventArgs e)
	{
		e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
	}

	private void txtDiscountRate_KeyPress(object sender, KeyPressEventArgs e)
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
		e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
	}

	private void txtToDocNoHistory_KeyPress(object sender, KeyPressEventArgs e)
	{
		e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
	}

	private void btnLoadGdn_Click(object sender, EventArgs e)
	{
	}

	private void LoadInGridDetail()
	{
		try
		{
			dtGrid.Rows.Clear();
			string GdnIds = "";
			for (int i = 0; i < LoadGDN.dtGDNS.Rows.Count; i++)
			{
				GdnIds = GdnIds + "," + Conversion.ToInt((object)LoadGDN.dtGDNS.Rows[i][0].ToString());
			}
			LoadDataDetailGridAgainstGP(GdnIds);
			grd.DataSource = dtGrid;
			grd.RetrieveStructure();
			grdSettings();
			dtInvExp.Clear();
			LoadExpData(GdnIds);
			if (dtInvExp.Rows.Count == 0 || dtInvExp == null)
			{
				AddRowInvExpGrid();
			}
			grdInvExp.RetrieveStructure();
			grdInvExpSettings();
			ExpProportion();
			BillProportion();
			BillAmount();
			GdnIds = "";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadDataDetailGridAgainstGP(string IDs)
	{
		try
		{
			DataTable invGdn = InvSaleInvoice.GdnLoadForSaleInvoiceDirect(IDs);
			if (invGdn.Rows.Count > 0)
			{
				DisableFields();
				comsupplier.Value = invGdn.Rows[0]["SupplierCustomerId"];
				((Control)(object)comsupplier).Text = Conversion.ToString(invGdn.Rows[0]["CustomerName"]);
				txtremarks.Text = invGdn.Rows[0]["RemarksHeader"].ToString();
				for (int i = 0; i < invGdn.Rows.Count; i++)
				{
					dtGrid.Rows.Add(0, 0, 0, 0, 0, 0, 0, invGdn.Rows[i]["InvGdnId"], invGdn.Rows[i]["Id"], invGdn.Rows[i]["DocNo"], invGdn.Rows[i]["WarehouseId"], invGdn.Rows[i]["WareHouseName"], invGdn.Rows[i]["ItemId"], invGdn.Rows[i]["ItemName"], invGdn.Rows[i]["ItemCode"], invGdn.Rows[i]["CropYear"], invGdn.Rows[i]["PackingTypeId"], invGdn.Rows[i]["PackTypeDesc"], invGdn.Rows[i]["ItemUomId"], invGdn.Rows[i]["UOMCodeItem"], invGdn.Rows[i]["UOMCodeItem"], invGdn.Rows[i]["ItemQty"], invGdn.Rows[i]["ItemQty"], invGdn.Rows[i]["NetBillWeight"], invGdn.Rows[i]["NetBillWeight"], 0, 0, 0, 0, 0, DateTime.Now, 0, "", 0, 0, 0, invGdn.Rows[i]["CityId"], invGdn.Rows[i]["CityName"]);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadExpData(string ID)
	{
		DataTable item = InvSaleInvoice.GetItemQtyAndItemRateFromSO(ID);
		if (item.Rows.Count > 0)
		{
			for (int i = 0; i < item.Rows.Count; i++)
			{
				dtInvExp.Rows.Add(0, item.Rows[i]["ItemQty"], item.Rows[i]["BagPrice"], Conversion.ToDouble(item.Rows[i]["BagPrice"]), 0);
			}
		}
	}

	private void btnLoadSaleOrder_Click(object sender, EventArgs e)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (grd.GetRows().Count() > 0)
			{
				GridEXRow item = grd.CurrentRow;
				if (item != null && item.Cells.Count > 0 && (int)item.RowType == 0 && !IsFromLoader)
				{
					MessageBox.Show("You cannot Load Data from Loader when Manual record Exist in grd");
					return;
				}
			}
			LoadTransactions = new LoadavailableTransactionsForIssuance(UserAccount);
			LoadTransactions.CmbWarehouse.Value = Conversion.ToInt(comWarehouse.Value);
			LoadTransactions.ShowDialog();
			LoadTransactions.BringToFront();
			IsFromLoader = true;
			LoadDataDetailfromPurchaseInvoivce();
			ExpProportion();
			BillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadDataDetailfromPurchaseInvoivce()
	{
		try
		{
			DataTable dtpurchaseinvoice = LoadTransactions.dtIssuance;
			if (dtpurchaseinvoice.Rows.Count > 0)
			{
				for (int i = 0; i < dtpurchaseinvoice.Rows.Count; i++)
				{
					dtGrid.Rows.Add(0, dtpurchaseinvoice.Rows[i]["RefDocumentTypeId"], dtpurchaseinvoice.Rows[i]["RefDocIdNo"], dtpurchaseinvoice.Rows[i]["RefDocSubIdNo"], 0, 0, 0, 0, 0, 0, dtpurchaseinvoice.Rows[i]["WareHouseId"], dtpurchaseinvoice.Rows[i]["WareHouse"], dtpurchaseinvoice.Rows[i]["ItemId"], dtpurchaseinvoice.Rows[i]["ItemName"], dtpurchaseinvoice.Rows[i]["ItemCode"], dtpurchaseinvoice.Rows[i]["CropYear"], dtpurchaseinvoice.Rows[i]["PackingTypeId"], dtpurchaseinvoice.Rows[i]["PackTypeCode"], dtpurchaseinvoice.Rows[i]["ItemUOMId"], dtpurchaseinvoice.Rows[i]["ItemUOM"], dtpurchaseinvoice.Rows[i]["ItemQty"], dtpurchaseinvoice.Rows[i]["ItemQty"], dtpurchaseinvoice.Rows[i]["Weight"], dtpurchaseinvoice.Rows[i]["Weight"], dtpurchaseinvoice.Rows[i]["ItemRate"], dtpurchaseinvoice.Rows[i]["ItemRate"], dtpurchaseinvoice.Rows[i]["RateUOMId"], dtpurchaseinvoice.Rows[i]["Equivalent"], dtpurchaseinvoice.Rows[i]["ItemAmount"], DateTime.Now, 0, "", 0, 0, 0, 0, "");
				}
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
		//IL_04a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b0: Expected O, but got Unknown
		//IL_04b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b7: Expected O, but got Unknown
		//IL_04b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04be: Expected O, but got Unknown
		//IL_04be: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c5: Expected O, but got Unknown
		//IL_04c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cc: Expected O, but got Unknown
		//IL_04cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d3: Expected O, but got Unknown
		//IL_04d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04da: Expected O, but got Unknown
		//IL_04da: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e1: Expected O, but got Unknown
		//IL_04e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e8: Expected O, but got Unknown
		//IL_04e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ef: Expected O, but got Unknown
		//IL_04ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f6: Expected O, but got Unknown
		//IL_04f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fd: Expected O, but got Unknown
		//IL_04fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0504: Expected O, but got Unknown
		//IL_0504: Unknown result type (might be due to invalid IL or missing references)
		//IL_050b: Expected O, but got Unknown
		//IL_050b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0512: Expected O, but got Unknown
		//IL_0512: Unknown result type (might be due to invalid IL or missing references)
		//IL_0519: Expected O, but got Unknown
		//IL_0519: Unknown result type (might be due to invalid IL or missing references)
		//IL_0520: Expected O, but got Unknown
		//IL_0520: Unknown result type (might be due to invalid IL or missing references)
		//IL_0527: Expected O, but got Unknown
		//IL_0527: Unknown result type (might be due to invalid IL or missing references)
		//IL_052e: Expected O, but got Unknown
		//IL_052e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0535: Expected O, but got Unknown
		//IL_0535: Unknown result type (might be due to invalid IL or missing references)
		//IL_053c: Expected O, but got Unknown
		//IL_053c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0543: Expected O, but got Unknown
		//IL_0543: Unknown result type (might be due to invalid IL or missing references)
		//IL_054a: Expected O, but got Unknown
		//IL_054a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0551: Expected O, but got Unknown
		//IL_0551: Unknown result type (might be due to invalid IL or missing references)
		//IL_0558: Expected O, but got Unknown
		//IL_0558: Unknown result type (might be due to invalid IL or missing references)
		//IL_055f: Expected O, but got Unknown
		//IL_055f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0566: Expected O, but got Unknown
		//IL_0566: Unknown result type (might be due to invalid IL or missing references)
		//IL_056d: Expected O, but got Unknown
		//IL_056d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0574: Expected O, but got Unknown
		//IL_0574: Unknown result type (might be due to invalid IL or missing references)
		//IL_057b: Expected O, but got Unknown
		//IL_057b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0582: Expected O, but got Unknown
		//IL_0582: Unknown result type (might be due to invalid IL or missing references)
		//IL_0589: Expected O, but got Unknown
		//IL_0589: Unknown result type (might be due to invalid IL or missing references)
		//IL_0590: Expected O, but got Unknown
		//IL_0590: Unknown result type (might be due to invalid IL or missing references)
		//IL_0597: Expected O, but got Unknown
		//IL_0597: Unknown result type (might be due to invalid IL or missing references)
		//IL_059e: Expected O, but got Unknown
		//IL_059e: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a5: Expected O, but got Unknown
		//IL_05a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ac: Expected O, but got Unknown
		//IL_05ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b3: Expected O, but got Unknown
		//IL_05d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05df: Expected O, but got Unknown
		//IL_0617: Unknown result type (might be due to invalid IL or missing references)
		//IL_0621: Expected O, but got Unknown
		//IL_0664: Unknown result type (might be due to invalid IL or missing references)
		//IL_066e: Expected O, but got Unknown
		//IL_069b: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a5: Expected O, but got Unknown
		//IL_06a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b0: Expected O, but got Unknown
		//IL_06b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06bb: Expected O, but got Unknown
		//IL_06bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c6: Expected O, but got Unknown
		//IL_06c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d1: Expected O, but got Unknown
		//IL_06d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06dc: Expected O, but got Unknown
		//IL_07e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ef: Expected O, but got Unknown
		//IL_083d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0847: Expected O, but got Unknown
		//IL_087f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0889: Expected O, but got Unknown
		//IL_08cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d6: Expected O, but got Unknown
		//IL_0950: Unknown result type (might be due to invalid IL or missing references)
		//IL_095a: Expected O, but got Unknown
		//IL_0966: Unknown result type (might be due to invalid IL or missing references)
		//IL_0970: Expected O, but got Unknown
		//IL_0971: Unknown result type (might be due to invalid IL or missing references)
		//IL_097b: Expected O, but got Unknown
		//IL_0987: Unknown result type (might be due to invalid IL or missing references)
		//IL_0991: Expected O, but got Unknown
		//IL_09b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_09bd: Expected O, but got Unknown
		//IL_0aa5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aaf: Expected O, but got Unknown
		//IL_0ad1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0adb: Expected O, but got Unknown
		//IL_0b55: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b5f: Expected O, but got Unknown
		//IL_0b6b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b75: Expected O, but got Unknown
		//IL_1114: Unknown result type (might be due to invalid IL or missing references)
		//IL_111e: Expected O, but got Unknown
		//IL_112b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1135: Expected O, but got Unknown
		//IL_1159: Unknown result type (might be due to invalid IL or missing references)
		//IL_1163: Expected O, but got Unknown
		//IL_1521: Unknown result type (might be due to invalid IL or missing references)
		//IL_152b: Expected O, but got Unknown
		//IL_1538: Unknown result type (might be due to invalid IL or missing references)
		//IL_1542: Expected O, but got Unknown
		//IL_154f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1559: Expected O, but got Unknown
		//IL_4ae8: Unknown result type (might be due to invalid IL or missing references)
		//IL_4af2: Expected O, but got Unknown
		//IL_4aff: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b09: Expected O, but got Unknown
		//IL_4b16: Unknown result type (might be due to invalid IL or missing references)
		//IL_4b20: Expected O, but got Unknown
		//IL_8e6a: Unknown result type (might be due to invalid IL or missing references)
		//IL_8e74: Expected O, but got Unknown
		//IL_8e98: Unknown result type (might be due to invalid IL or missing references)
		//IL_8ea2: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdInvExp_DesignTimeLayout = new GridEXLayout();
		Appearance appearance136 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
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
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
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
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
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
		GridEXLayout grdGLedger_DesignTimeLayout = new GridEXLayout();
		Appearance Appearance1 = new Appearance();
		Appearance Appearance5 = new Appearance();
		Appearance Appearance6 = new Appearance();
		Appearance Appearance7 = new Appearance();
		Appearance Appearance8 = new Appearance();
		Appearance Appearance9 = new Appearance();
		Appearance Appearance10 = new Appearance();
		Appearance Appearance11 = new Appearance();
		Appearance Appearance12 = new Appearance();
		Appearance Appearance2 = new Appearance();
		Appearance Appearance3 = new Appearance();
		Appearance Appearance4 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance168 = new Appearance();
		Appearance appearance169 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance171 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance181 = new Appearance();
		Appearance appearance182 = new Appearance();
		Appearance appearance183 = new Appearance();
		Appearance appearance184 = new Appearance();
		Appearance appearance185 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance187 = new Appearance();
		Appearance appearance188 = new Appearance();
		Appearance appearance189 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance172 = new Appearance();
		Appearance appearance180 = new Appearance();
		Appearance appearance186 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance157 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance135 = new Appearance();
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance146 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance190 = new Appearance();
		Appearance appearance191 = new Appearance();
		Appearance appearance192 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel1 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel16 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.grdInvExp = new GridEX();
		this.panel12 = new System.Windows.Forms.Panel();
		this.label37 = new System.Windows.Forms.Label();
		this.panelDetail = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.rdbtnItemCode = new System.Windows.Forms.RadioButton();
		this.rdbtnItemName = new System.Windows.Forms.RadioButton();
		this.txtCity = new UltraCombo();
		this.label8 = new System.Windows.Forms.Label();
		this.lblBalance = new System.Windows.Forms.Label();
		this.btnCancelUpdateDetial = new System.Windows.Forms.Button();
		this.btnAdd = new System.Windows.Forms.Button();
		this.comWarehouse = new UltraCombo();
		this.comRateUOM = new UltraCombo();
		this.comPackUOM = new UltraCombo();
		this.comCropYear = new UltraCombo();
		this.comPackingType = new UltraCombo();
		this.comItem = new UltraCombo();
		this.txtQty = new System.Windows.Forms.TextBox();
		this.label38 = new System.Windows.Forms.Label();
		this.txtNetBillWeight = new System.Windows.Forms.TextBox();
		this.label36 = new System.Windows.Forms.Label();
		this.label42 = new System.Windows.Forms.Label();
		this.label41 = new System.Windows.Forms.Label();
		this.label40 = new System.Windows.Forms.Label();
		this.label32 = new System.Windows.Forms.Label();
		this.label31 = new System.Windows.Forms.Label();
		this.txtRate = new System.Windows.Forms.TextBox();
		this.label28 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.label22 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.txtAmount = new System.Windows.Forms.TextBox();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.txtgpdate = new System.Windows.Forms.DateTimePicker();
		this.label35 = new System.Windows.Forms.Label();
		this.label47 = new System.Windows.Forms.Label();
		this.txtvehicleno = new System.Windows.Forms.TextBox();
		this.label48 = new System.Windows.Forms.Label();
		this.txtgatepassno = new System.Windows.Forms.TextBox();
		this.panel3 = new System.Windows.Forms.Panel();
		this.panel14 = new System.Windows.Forms.Panel();
		this.grdGLedger = new GridEX();
		this.panel15 = new System.Windows.Forms.Panel();
		this.label53 = new System.Windows.Forms.Label();
		this.txtdeliverydays = new System.Windows.Forms.TextBox();
		this.label45 = new System.Windows.Forms.Label();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.txtBillAmount = new System.Windows.Forms.TextBox();
		this.duedate = new System.Windows.Forms.DateTimePicker();
		this.combpttrm = new UltraCombo();
		this.txtduedays = new System.Windows.Forms.TextBox();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.DocDate = new System.Windows.Forms.DateTimePicker();
		this.label5 = new System.Windows.Forms.Label();
		this.txtbillno = new System.Windows.Forms.TextBox();
		this.combdeliverytrm = new UltraCombo();
		this.label6 = new System.Windows.Forms.Label();
		this.label51 = new System.Windows.Forms.Label();
		this.txtremarks = new System.Windows.Forms.TextBox();
		this.label24 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.comsupplier = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.label29 = new System.Windows.Forms.Label();
		this.label44 = new System.Windows.Forms.Label();
		this.validate = new System.Windows.Forms.DateTimePicker();
		this.panel11 = new System.Windows.Forms.Panel();
		this.label17 = new System.Windows.Forms.Label();
		this.ChkPrintslip = new System.Windows.Forms.CheckBox();
		this.ChkBok = new System.Windows.Forms.CheckBox();
		this.ChkResetOnSave = new System.Windows.Forms.CheckBox();
		this.deliverystartdate = new System.Windows.Forms.DateTimePicker();
		this.comproject = new UltraCombo();
		this.label27 = new System.Windows.Forms.Label();
		this.combranches = new UltraCombo();
		this.comstockparty = new UltraCombo();
		this.label13 = new System.Windows.Forms.Label();
		this.compartyreference = new UltraCombo();
		this.label18 = new System.Windows.Forms.Label();
		this.txtCommAgentId = new System.Windows.Forms.TextBox();
		this.txtSupplierGLId = new System.Windows.Forms.TextBox();
		this.comborderno = new UltraCombo();
		this.label3 = new System.Windows.Forms.Label();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
		this.btnSlip = new System.Windows.Forms.ToolStripButton();
		this.btn294APrint = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
		this.btnLoadGdn = new System.Windows.Forms.ToolStripButton();
		this.btnLoadSaleOrder = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
		this.btnShortcutKeys = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.grdDetail = new GridEX();
		this.panel18 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.label55 = new System.Windows.Forms.Label();
		this.grdHistory = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label20 = new System.Windows.Forms.Label();
		this.panel10 = new System.Windows.Forms.Panel();
		this.groupBox4 = new System.Windows.Forms.GroupBox();
		this.rdapproveddate = new System.Windows.Forms.RadioButton();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.btnshow = new System.Windows.Forms.Button();
		this.label73 = new System.Windows.Forms.Label();
		this.CmbCustomerHistory = new UltraCombo();
		this.label75 = new System.Windows.Forms.Label();
		this.cmbDateTypeHistory = new UltraCombo();
		this.txtToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.label2 = new System.Windows.Forms.Label();
		this.label76 = new System.Windows.Forms.Label();
		this.txtToDocNoHistory = new System.Windows.Forms.TextBox();
		this.txtFromdateHistory = new System.Windows.Forms.DateTimePicker();
		this.label77 = new System.Windows.Forms.Label();
		this.txtFromNoHistory = new System.Windows.Forms.TextBox();
		this.panel9 = new System.Windows.Forms.Panel();
		this.label54 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.BtnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.BtnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel8.SuspendLayout();
		this.panel16.SuspendLayout();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdInvExp).BeginInit();
		this.panel12.SuspendLayout();
		this.panelDetail.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.txtCity).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comWarehouse).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comRateUOM).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comPackUOM).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comCropYear).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comPackingType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comItem).BeginInit();
		this.panel3.SuspendLayout();
		this.panel14.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdGLedger).BeginInit();
		this.panel15.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.combpttrm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combdeliverytrm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comsupplier).BeginInit();
		this.panel11.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.comproject).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combranches).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comstockparty).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.compartyreference).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comborderno).BeginInit();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel18.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).BeginInit();
		this.panel4.SuspendLayout();
		this.panel10.SuspendLayout();
		this.groupBox4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDateTypeHistory).BeginInit();
		this.panel9.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(984, 611);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel1);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(976, 582);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel1.Controls.Add(this.panel8);
		this.panel1.Controls.Add(this.panel16);
		this.panel1.Controls.Add(this.panelDetail);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.toolStrip1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(3, 3);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(970, 576);
		this.panel1.TabIndex = 7;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 455);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(970, 121);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.EditingCell += new EditingCellEventHandler(grd_EditingCell);
		this.grd.CellUpdated += new ColumnActionEventHandler(grd_CellUpdated);
		this.grd.RecordsDeleted += new System.EventHandler(grd_RecordsDeleted);
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grd_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.label23);
		this.panel8.Controls.Add(this.ctrlGrdBar1);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 432);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(970, 23);
		this.panel8.TabIndex = 32;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(5, 5);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(46, 17);
		this.label23.TabIndex = 35;
		this.label23.Text = "Detail";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(925, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(37, 22);
		this.ctrlGrdBar1.TabIndex = 33;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.panel16.BackColor = System.Drawing.Color.White;
		this.panel16.Controls.Add(this.panel6);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel16.Location = new System.Drawing.Point(0, 283);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(970, 149);
		this.panel16.TabIndex = 33;
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.grdInvExp);
		this.panel6.Controls.Add(this.panel12);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(970, 149);
		this.panel6.TabIndex = 0;
		((ScrollableBase)this.grdInvExp).BackColor = System.Drawing.Color.White;
		grdInvExp_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdInvExp.DesignTimeLayout = grdInvExp_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdInvExp.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Location = new System.Drawing.Point(0, 29);
		((System.Windows.Forms.Control)(object)this.grdInvExp).Name = "grdInvExp";
		((ScrollableBase)this.grdInvExp).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdInvExp).Size = new System.Drawing.Size(970, 120);
		((System.Windows.Forms.Control)(object)this.grdInvExp).TabIndex = 0;
		this.grdInvExp.TotalRow = (InheritableBoolean)1;
		this.grdInvExp.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdInvExp).VisualStyle = (VisualStyle)4;
		this.grdInvExp.CellUpdated += new ColumnActionEventHandler(grdInvExp_CellUpdated);
		this.grdInvExp.UpdatingCell += new UpdatingCellEventHandler(grdInvExp_UpdatingCell);
		this.grdInvExp.ColumnButtonClick += new ColumnActionEventHandler(grdInvExp_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdInvExp).KeyDown += new System.Windows.Forms.KeyEventHandler(grdInvExp_KeyDown);
		this.panel12.BackColor = System.Drawing.Color.Teal;
		this.panel12.Controls.Add(this.label37);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(0, 0);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(970, 29);
		this.panel12.TabIndex = 31;
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label37.ForeColor = System.Drawing.Color.White;
		this.label37.Location = new System.Drawing.Point(5, 5);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(98, 17);
		this.label37.TabIndex = 36;
		this.label37.Text = "Expense Grid";
		this.panelDetail.Controls.Add(this.groupBox1);
		this.panelDetail.Controls.Add(this.txtgpdate);
		this.panelDetail.Controls.Add(this.label35);
		this.panelDetail.Controls.Add(this.label47);
		this.panelDetail.Controls.Add(this.txtvehicleno);
		this.panelDetail.Controls.Add(this.label48);
		this.panelDetail.Controls.Add(this.txtgatepassno);
		this.panelDetail.Dock = System.Windows.Forms.DockStyle.Top;
		this.panelDetail.Location = new System.Drawing.Point(0, 158);
		this.panelDetail.Name = "panelDetail";
		this.panelDetail.Size = new System.Drawing.Size(970, 125);
		this.panelDetail.TabIndex = 31;
		this.groupBox1.BackColor = System.Drawing.Color.White;
		this.groupBox1.Controls.Add(this.rdbtnItemCode);
		this.groupBox1.Controls.Add(this.rdbtnItemName);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.txtCity);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add(this.lblBalance);
		this.groupBox1.Controls.Add(this.btnCancelUpdateDetial);
		this.groupBox1.Controls.Add(this.btnAdd);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.comWarehouse);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.comRateUOM);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.comPackUOM);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.comCropYear);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.comPackingType);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.comItem);
		this.groupBox1.Controls.Add(this.txtQty);
		this.groupBox1.Controls.Add(this.label38);
		this.groupBox1.Controls.Add(this.txtNetBillWeight);
		this.groupBox1.Controls.Add(this.label36);
		this.groupBox1.Controls.Add(this.label42);
		this.groupBox1.Controls.Add(this.label41);
		this.groupBox1.Controls.Add(this.label40);
		this.groupBox1.Controls.Add(this.label32);
		this.groupBox1.Controls.Add(this.label31);
		this.groupBox1.Controls.Add(this.txtRate);
		this.groupBox1.Controls.Add(this.label28);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Controls.Add(this.label22);
		this.groupBox1.Controls.Add(this.label14);
		this.groupBox1.Controls.Add(this.txtAmount);
		this.groupBox1.Controls.Add(this.btnUpdateDetail);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(0, 3);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(967, 116);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Detail";
		this.rdbtnItemCode.AutoSize = true;
		this.rdbtnItemCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdbtnItemCode.Location = new System.Drawing.Point(336, 17);
		this.rdbtnItemCode.Margin = new System.Windows.Forms.Padding(2);
		this.rdbtnItemCode.Name = "rdbtnItemCode";
		this.rdbtnItemCode.Size = new System.Drawing.Size(52, 19);
		this.rdbtnItemCode.TabIndex = 125;
		this.rdbtnItemCode.Text = "Code";
		this.rdbtnItemCode.UseVisualStyleBackColor = true;
		this.rdbtnItemName.AutoSize = true;
		this.rdbtnItemName.Checked = true;
		this.rdbtnItemName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdbtnItemName.Location = new System.Drawing.Point(283, 17);
		this.rdbtnItemName.Margin = new System.Windows.Forms.Padding(2);
		this.rdbtnItemName.Name = "rdbtnItemName";
		this.rdbtnItemName.Size = new System.Drawing.Size(57, 19);
		this.rdbtnItemName.TabIndex = 124;
		this.rdbtnItemName.TabStop = true;
		this.rdbtnItemName.Text = "Name";
		this.rdbtnItemName.UseVisualStyleBackColor = true;
		this.rdbtnItemName.CheckedChanged += new System.EventHandler(rdbtnItemName_CheckedChanged);
		this.txtCity.AllowNull = (DefaultableBoolean)1;
		this.txtCity.AutoCompleteMode = (AutoCompleteMode)4;
		this.txtCity.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.txtCity.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance136).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.txtCity).DisplayLayout.Appearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.txtCity).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.txtCity).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance137).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance137).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance137).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.txtCity).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance137;
		((AppearanceBase)appearance138).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.txtCity).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance138;
		((SpecialBoxBase)((UltraGridBase)this.txtCity).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance140).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance140).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance140).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.txtCity).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.txtCity).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.txtCity).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance141).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance141;
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance142).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance143;
		((AppearanceBase)appearance144).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance144).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance145).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance145).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance145).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance145).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance145;
		((AppearanceBase)appearance27).TextHAlignAsString = "Left";
		((UltraGridBase)this.txtCity).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.txtCity).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.txtCity).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.txtCity).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.txtCity).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtCity.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.txtCity).Location = new System.Drawing.Point(278, 85);
		((System.Windows.Forms.Control)(object)this.txtCity).Name = "txtCity";
		((System.Windows.Forms.Control)(object)this.txtCity).Size = new System.Drawing.Size(195, 26);
		((System.Windows.Forms.Control)(object)this.txtCity).TabIndex = 10;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(278, 68);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(62, 15);
		this.label8.TabIndex = 84;
		this.label8.Text = "City Name";
		this.lblBalance.AutoSize = true;
		this.lblBalance.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.lblBalance.Location = new System.Drawing.Point(394, 19);
		this.lblBalance.Name = "lblBalance";
		this.lblBalance.Size = new System.Drawing.Size(14, 15);
		this.lblBalance.TabIndex = 80;
		this.lblBalance.Text = "0";
		this.btnCancelUpdateDetial.BackColor = System.Drawing.Color.Teal;
		this.btnCancelUpdateDetial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelUpdateDetial.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelUpdateDetial.ForeColor = System.Drawing.Color.White;
		this.btnCancelUpdateDetial.Location = new System.Drawing.Point(546, 85);
		this.btnCancelUpdateDetial.Margin = new System.Windows.Forms.Padding(2);
		this.btnCancelUpdateDetial.Name = "btnCancelUpdateDetial";
		this.btnCancelUpdateDetial.Size = new System.Drawing.Size(66, 27);
		this.btnCancelUpdateDetial.TabIndex = 13;
		this.btnCancelUpdateDetial.Text = "Cancel";
		this.btnCancelUpdateDetial.UseVisualStyleBackColor = false;
		this.btnCancelUpdateDetial.Visible = false;
		this.btnCancelUpdateDetial.Click += new System.EventHandler(btnCancelUpdateDetial_Click);
		this.btnAdd.BackColor = System.Drawing.Color.Teal;
		this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAdd.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAdd.ForeColor = System.Drawing.Color.White;
		this.btnAdd.Location = new System.Drawing.Point(476, 85);
		this.btnAdd.Margin = new System.Windows.Forms.Padding(2);
		this.btnAdd.Name = "btnAdd";
		this.btnAdd.Size = new System.Drawing.Size(29, 27);
		this.btnAdd.TabIndex = 11;
		this.btnAdd.Text = "+";
		this.btnAdd.UseVisualStyleBackColor = false;
		this.btnAdd.Click += new System.EventHandler(btnAdd_Click);
		this.comWarehouse.AllowNull = (DefaultableBoolean)1;
		this.comWarehouse.AutoCompleteMode = (AutoCompleteMode)4;
		this.comWarehouse.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comWarehouse.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Appearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.comWarehouse).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comWarehouse).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comWarehouse).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance101).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comWarehouse).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance101;
		((SpecialBoxBase)((UltraGridBase)this.comWarehouse).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance103).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance103).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance103).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comWarehouse).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.comWarehouse).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comWarehouse).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance104).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance104;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance105).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance106;
		((AppearanceBase)appearance107).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance107).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance108).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance108).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance108).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance108).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance108;
		((AppearanceBase)appearance109).TextHAlignAsString = "Left";
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance110).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comWarehouse).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.comWarehouse).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comWarehouse).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comWarehouse).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comWarehouse).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.comWarehouse.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comWarehouse).Location = new System.Drawing.Point(4, 38);
		((System.Windows.Forms.Control)(object)this.comWarehouse).Name = "comWarehouse";
		((System.Windows.Forms.Control)(object)this.comWarehouse).Size = new System.Drawing.Size(237, 26);
		((System.Windows.Forms.Control)(object)this.comWarehouse).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.comWarehouse).Leave += new System.EventHandler(comWarehouse_TextChanged);
		this.comRateUOM.AllowNull = (DefaultableBoolean)1;
		this.comRateUOM.AutoCompleteMode = (AutoCompleteMode)4;
		this.comRateUOM.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comRateUOM.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Appearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.comRateUOM).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comRateUOM).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comRateUOM).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comRateUOM).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance45;
		((SpecialBoxBase)((UltraGridBase)this.comRateUOM).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance53).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comRateUOM).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.comRateUOM).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comRateUOM).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance55;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance66).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance71;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance80).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance91).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance91).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance91).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance91).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance91;
		((AppearanceBase)appearance102).TextHAlignAsString = "Left";
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comRateUOM).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.comRateUOM).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comRateUOM).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comRateUOM).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comRateUOM).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.comRateUOM.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comRateUOM).Location = new System.Drawing.Point(8, 85);
		((System.Windows.Forms.Control)(object)this.comRateUOM).Name = "comRateUOM";
		((System.Windows.Forms.Control)(object)this.comRateUOM).Size = new System.Drawing.Size(75, 26);
		((System.Windows.Forms.Control)(object)this.comRateUOM).TabIndex = 7;
		((System.Windows.Forms.Control)(object)this.comRateUOM).Leave += new System.EventHandler(comRateUOM_Leave);
		this.comPackUOM.AllowNull = (DefaultableBoolean)1;
		this.comPackUOM.AutoCompleteMode = (AutoCompleteMode)4;
		this.comPackUOM.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comPackUOM.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Appearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.comPackUOM).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comPackUOM).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance73).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance73).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comPackUOM).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance73;
		((AppearanceBase)appearance74).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comPackUOM).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance74;
		((SpecialBoxBase)((UltraGridBase)this.comPackUOM).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance75).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance75).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance75).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comPackUOM).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.comPackUOM).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comPackUOM).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance76;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance77).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance78;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance81).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance82).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance82).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance82).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance82).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance82;
		((AppearanceBase)appearance83).TextHAlignAsString = "Left";
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance84).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comPackUOM).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.comPackUOM).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comPackUOM).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comPackUOM).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comPackUOM).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.comPackUOM.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comPackUOM).Location = new System.Drawing.Point(772, 38);
		((System.Windows.Forms.Control)(object)this.comPackUOM).Name = "comPackUOM";
		((System.Windows.Forms.Control)(object)this.comPackUOM).Size = new System.Drawing.Size(75, 26);
		((System.Windows.Forms.Control)(object)this.comPackUOM).TabIndex = 5;
		((System.Windows.Forms.Control)(object)this.comPackUOM).Leave += new System.EventHandler(comPackUOM_Leave);
		this.comCropYear.AllowNull = (DefaultableBoolean)1;
		this.comCropYear.AutoCompleteMode = (AutoCompleteMode)4;
		this.comCropYear.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comCropYear.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comCropYear).DisplayLayout.Appearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.comCropYear).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comCropYear).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance15).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance15).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comCropYear).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance15;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comCropYear).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance16;
		((SpecialBoxBase)((UltraGridBase)this.comCropYear).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance17).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance17).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comCropYear).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.comCropYear).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comCropYear).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comCropYear).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance18;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comCropYear).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.comCropYear).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comCropYear).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comCropYear).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance21).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comCropYear).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.comCropYear).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comCropYear).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance22).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance22).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance22).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance22).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comCropYear).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance24).TextHAlignAsString = "Left";
		((UltraGridBase)this.comCropYear).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.comCropYear).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comCropYear).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comCropYear).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.comCropYear).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comCropYear).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.comCropYear).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comCropYear).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comCropYear).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comCropYear).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.comCropYear.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comCropYear).Location = new System.Drawing.Point(476, 38);
		((System.Windows.Forms.Control)(object)this.comCropYear).Name = "comCropYear";
		((System.Windows.Forms.Control)(object)this.comCropYear).Size = new System.Drawing.Size(94, 26);
		((System.Windows.Forms.Control)(object)this.comCropYear).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.comCropYear).Leave += new System.EventHandler(comCropYear_Leave);
		this.comPackingType.AllowNull = (DefaultableBoolean)1;
		this.comPackingType.AutoCompleteMode = (AutoCompleteMode)4;
		this.comPackingType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comPackingType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comPackingType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.comPackingType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comPackingType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance87).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance87).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comPackingType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance87;
		((AppearanceBase)appearance88).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comPackingType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance88;
		((SpecialBoxBase)((UltraGridBase)this.comPackingType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance89).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance89).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance89).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comPackingType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.comPackingType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comPackingType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comPackingType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance90;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance92).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comPackingType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.comPackingType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comPackingType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comPackingType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance94).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance94).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comPackingType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance94;
		((UltraGridBase)this.comPackingType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comPackingType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance95).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance95).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance95).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance95).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comPackingType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance95;
		((AppearanceBase)appearance96).TextHAlignAsString = "Left";
		((UltraGridBase)this.comPackingType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.comPackingType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comPackingType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance97).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comPackingType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.comPackingType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comPackingType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance98;
		((UltraGridBase)this.comPackingType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comPackingType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comPackingType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comPackingType).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.comPackingType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comPackingType).Location = new System.Drawing.Point(572, 38);
		((System.Windows.Forms.Control)(object)this.comPackingType).Name = "comPackingType";
		((System.Windows.Forms.Control)(object)this.comPackingType).Size = new System.Drawing.Size(112, 26);
		((System.Windows.Forms.Control)(object)this.comPackingType).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.comPackingType).Leave += new System.EventHandler(comPackingType_Leave);
		this.comItem.AllowNull = (DefaultableBoolean)1;
		this.comItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.comItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.comItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance41).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance41).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance42;
		((SpecialBoxBase)((UltraGridBase)this.comItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance43).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance43).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.comItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance46).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.comItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance47;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance48).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.comItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance49).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance49).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance49).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance49).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance50).TextHAlignAsString = "Left";
		((UltraGridBase)this.comItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.comItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance51).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.comItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.comItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comItem).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.comItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comItem).Location = new System.Drawing.Point(244, 38);
		((System.Windows.Forms.Control)(object)this.comItem).Name = "comItem";
		((System.Windows.Forms.Control)(object)this.comItem).Size = new System.Drawing.Size(229, 26);
		((System.Windows.Forms.Control)(object)this.comItem).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.comItem).Leave += new System.EventHandler(comItem_Leave);
		this.txtQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtQty.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtQty.Location = new System.Drawing.Point(686, 40);
		this.txtQty.Name = "txtQty";
		this.txtQty.Size = new System.Drawing.Size(84, 23);
		this.txtQty.TabIndex = 4;
		this.txtQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtQty.TextChanged += new System.EventHandler(txtQty_TextChanged);
		this.txtQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtQty_KeyPress);
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label38.Location = new System.Drawing.Point(686, 19);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(26, 15);
		this.label38.TabIndex = 47;
		this.label38.Text = "Qty";
		this.txtNetBillWeight.BackColor = System.Drawing.Color.White;
		this.txtNetBillWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNetBillWeight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtNetBillWeight.Location = new System.Drawing.Point(849, 40);
		this.txtNetBillWeight.Name = "txtNetBillWeight";
		this.txtNetBillWeight.ReadOnly = true;
		this.txtNetBillWeight.Size = new System.Drawing.Size(115, 23);
		this.txtNetBillWeight.TabIndex = 6;
		this.txtNetBillWeight.TabStop = false;
		this.txtNetBillWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(849, 21);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(87, 15);
		this.label36.TabIndex = 43;
		this.label36.Text = "Net Bill Weight";
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label42.Location = new System.Drawing.Point(85, 68);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(30, 15);
		this.label42.TabIndex = 55;
		this.label42.Text = "Rate";
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label41.Location = new System.Drawing.Point(476, 19);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(57, 15);
		this.label41.TabIndex = 53;
		this.label41.Text = "Crop Year";
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.Location = new System.Drawing.Point(682, 359);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(39, 18);
		this.label40.TabIndex = 51;
		this.label40.Text = "Rate";
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label32.Location = new System.Drawing.Point(244, 19);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(32, 15);
		this.label32.TabIndex = 39;
		this.label32.Text = "Item";
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label31.Location = new System.Drawing.Point(4, 19);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(69, 15);
		this.label31.TabIndex = 33;
		this.label31.Text = "WareHouse";
		this.txtRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtRate.Location = new System.Drawing.Point(85, 87);
		this.txtRate.Name = "txtRate";
		this.txtRate.Size = new System.Drawing.Size(78, 23);
		this.txtRate.TabIndex = 8;
		this.txtRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtRate.TextChanged += new System.EventHandler(txtRate_TextChanged);
		this.txtRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtRate_KeyPress);
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label28.Location = new System.Drawing.Point(772, 19);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(63, 15);
		this.label28.TabIndex = 29;
		this.label28.Text = "Pack UOM";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label9.Location = new System.Drawing.Point(572, 19);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(77, 15);
		this.label9.TabIndex = 19;
		this.label9.Text = "Packing Type";
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(8, 68);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(61, 15);
		this.label22.TabIndex = 27;
		this.label22.Text = "Rate UOM";
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(165, 68);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(51, 15);
		this.label14.TabIndex = 21;
		this.label14.Text = "Amount";
		this.txtAmount.BackColor = System.Drawing.Color.White;
		this.txtAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtAmount.Location = new System.Drawing.Point(165, 87);
		this.txtAmount.Name = "txtAmount";
		this.txtAmount.ReadOnly = true;
		this.txtAmount.Size = new System.Drawing.Size(111, 23);
		this.txtAmount.TabIndex = 9;
		this.txtAmount.TabStop = false;
		this.txtAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(476, 85);
		this.btnUpdateDetail.Margin = new System.Windows.Forms.Padding(2);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(70, 27);
		this.btnUpdateDetail.TabIndex = 12;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Visible = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.txtgpdate.CustomFormat = "dd-MMM-yy";
		this.txtgpdate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtgpdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtgpdate.Location = new System.Drawing.Point(619, 200);
		this.txtgpdate.Name = "txtgpdate";
		this.txtgpdate.Size = new System.Drawing.Size(114, 23);
		this.txtgpdate.TabIndex = 15;
		this.txtgpdate.TabStop = false;
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(619, 180);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(50, 15);
		this.label35.TabIndex = 45;
		this.label35.Text = "GP Date";
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label47.Location = new System.Drawing.Point(735, 180);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(32, 15);
		this.label47.TabIndex = 60;
		this.label47.Text = "GP #";
		this.txtvehicleno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtvehicleno.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtvehicleno.Location = new System.Drawing.Point(791, 200);
		this.txtvehicleno.Name = "txtvehicleno";
		this.txtvehicleno.Size = new System.Drawing.Size(80, 23);
		this.txtvehicleno.TabIndex = 17;
		this.txtvehicleno.TabStop = false;
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.Location = new System.Drawing.Point(791, 180);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(64, 15);
		this.label48.TabIndex = 70;
		this.label48.Text = "Vehicle No";
		this.txtgatepassno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtgatepassno.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtgatepassno.Location = new System.Drawing.Point(735, 200);
		this.txtgatepassno.Name = "txtgatepassno";
		this.txtgatepassno.Size = new System.Drawing.Size(55, 23);
		this.txtgatepassno.TabIndex = 16;
		this.txtgatepassno.TabStop = false;
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add(this.panel14);
		this.panel3.Controls.Add(this.txtdeliverydays);
		this.panel3.Controls.Add(this.label45);
		this.panel3.Controls.Add(this.groupBox2);
		this.panel3.Controls.Add(this.label44);
		this.panel3.Controls.Add(this.validate);
		this.panel3.Controls.Add(this.panel11);
		this.panel3.Controls.Add(this.deliverystartdate);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.comproject);
		this.panel3.Controls.Add(this.label27);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.combranches);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.comstockparty);
		this.panel3.Controls.Add(this.label13);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.compartyreference);
		this.panel3.Controls.Add(this.label18);
		this.panel3.Controls.Add(this.txtCommAgentId);
		this.panel3.Controls.Add(this.txtSupplierGLId);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.comborderno);
		this.panel3.Controls.Add(this.label3);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.panel3.Location = new System.Drawing.Point(0, 25);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(970, 133);
		this.panel3.TabIndex = 0;
		this.panel14.Controls.Add((System.Windows.Forms.Control)(object)this.grdGLedger);
		this.panel14.Controls.Add(this.panel15);
		this.panel14.Location = new System.Drawing.Point(1439, 139);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(152, 51);
		this.panel14.TabIndex = 34;
		this.panel14.Visible = false;
		((ScrollableBase)this.grdGLedger).BackColor = System.Drawing.Color.White;
		this.grdGLedger.ColumnAutoResize = true;
		grdGLedger_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdGLedger.DesignTimeLayout = grdGLedger_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdGLedger).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdGLedger.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdGLedger).Location = new System.Drawing.Point(3, 19);
		((System.Windows.Forms.Control)(object)this.grdGLedger).Name = "grdGLedger";
		((ScrollableBase)this.grdGLedger).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdGLedger).Size = new System.Drawing.Size(149, 32);
		((System.Windows.Forms.Control)(object)this.grdGLedger).TabIndex = 0;
		this.grdGLedger.TotalRow = (InheritableBoolean)1;
		this.grdGLedger.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdGLedger).VisualStyle = (VisualStyle)4;
		this.grdGLedger.CellUpdated += new ColumnActionEventHandler(grdGLedger_CellUpdated);
		this.grdGLedger.UpdatingCell += new UpdatingCellEventHandler(grdGLedger_UpdatingCell);
		this.grdGLedger.ColumnButtonClick += new ColumnActionEventHandler(grdGLedger_ColumnButtonClick);
		this.panel15.BackColor = System.Drawing.Color.Teal;
		this.panel15.Controls.Add(this.label53);
		this.panel15.Location = new System.Drawing.Point(3, 3);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(146, 20);
		this.panel15.TabIndex = 33;
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Tahoma", 6f, System.Drawing.FontStyle.Bold);
		this.label53.ForeColor = System.Drawing.Color.White;
		this.label53.Location = new System.Drawing.Point(3, 3);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(84, 10);
		this.label53.TabIndex = 36;
		this.label53.Text = "Party Add/Less Grid";
		this.txtdeliverydays.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdeliverydays.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdeliverydays.Location = new System.Drawing.Point(1355, 107);
		this.txtdeliverydays.Multiline = true;
		this.txtdeliverydays.Name = "txtdeliverydays";
		this.txtdeliverydays.Size = new System.Drawing.Size(78, 26);
		this.txtdeliverydays.TabIndex = 1;
		this.txtdeliverydays.Visible = false;
		this.label45.AutoSize = true;
		this.label45.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label45.Location = new System.Drawing.Point(1265, 80);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(58, 13);
		this.label45.TabIndex = 45;
		this.label45.Text = "Start Date";
		this.label45.Visible = false;
		this.groupBox2.Controls.Add(this.txtBillAmount);
		this.groupBox2.Controls.Add(this.duedate);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.combpttrm);
		this.groupBox2.Controls.Add(this.txtduedays);
		this.groupBox2.Controls.Add(this.txtdocno);
		this.groupBox2.Controls.Add(this.DocDate);
		this.groupBox2.Controls.Add(this.label5);
		this.groupBox2.Controls.Add(this.txtbillno);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.combdeliverytrm);
		this.groupBox2.Controls.Add(this.label6);
		this.groupBox2.Controls.Add(this.label51);
		this.groupBox2.Controls.Add(this.txtremarks);
		this.groupBox2.Controls.Add(this.label24);
		this.groupBox2.Controls.Add(this.label7);
		this.groupBox2.Controls.Add(this.label4);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.comsupplier);
		this.groupBox2.Controls.Add(this.label1);
		this.groupBox2.Controls.Add(this.label10);
		this.groupBox2.Controls.Add(this.label29);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(2, 33);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(965, 97);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Main";
		this.txtBillAmount.Font = new System.Drawing.Font("Verdana", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBillAmount.Location = new System.Drawing.Point(813, 41);
		this.txtBillAmount.Name = "txtBillAmount";
		this.txtBillAmount.ReadOnly = true;
		this.txtBillAmount.Size = new System.Drawing.Size(150, 27);
		this.txtBillAmount.TabIndex = 9;
		this.txtBillAmount.TabStop = false;
		this.txtBillAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.duedate.CustomFormat = "dd-MM-yy";
		this.duedate.Enabled = false;
		this.duedate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.duedate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.duedate.Location = new System.Drawing.Point(813, 15);
		this.duedate.Name = "duedate";
		this.duedate.Size = new System.Drawing.Size(150, 23);
		this.duedate.TabIndex = 8;
		this.combpttrm.AllowNull = (DefaultableBoolean)1;
		this.combpttrm.AutoCompleteMode = (AutoCompleteMode)4;
		this.combpttrm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combpttrm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)Appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)Appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combpttrm).DisplayLayout.Appearance = (AppearanceBase)(object)Appearance1;
		((UltraGridBase)this.combpttrm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combpttrm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)Appearance5).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)Appearance5).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)Appearance5).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)Appearance5).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combpttrm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)Appearance5;
		((AppearanceBase)Appearance6).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combpttrm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)Appearance6;
		((SpecialBoxBase)((UltraGridBase)this.combpttrm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)Appearance7).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)Appearance7).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)Appearance7).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)Appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combpttrm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)Appearance7;
		((UltraGridBase)this.combpttrm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combpttrm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)Appearance8).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)Appearance8).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)Appearance8;
		((AppearanceBase)Appearance9).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)Appearance9).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)Appearance9;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)Appearance10).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)Appearance10;
		((AppearanceBase)Appearance11).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)Appearance11).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)Appearance11;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)Appearance12).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)Appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)Appearance12).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)Appearance12).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)Appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)Appearance12;
		((AppearanceBase)Appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)Appearance2;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)Appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)Appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)Appearance3;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)Appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)Appearance4;
		((UltraGridBase)this.combpttrm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combpttrm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combpttrm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combpttrm).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.combpttrm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combpttrm).Location = new System.Drawing.Point(593, 13);
		((System.Windows.Forms.Control)(object)this.combpttrm).Name = "combpttrm";
		((System.Windows.Forms.Control)(object)this.combpttrm).Size = new System.Drawing.Size(134, 26);
		((System.Windows.Forms.Control)(object)this.combpttrm).TabIndex = 6;
		((System.Windows.Forms.Control)(object)this.combpttrm).Leave += new System.EventHandler(combpttrm_Leave);
		this.txtduedays.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtduedays.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtduedays.Location = new System.Drawing.Point(593, 43);
		this.txtduedays.Name = "txtduedays";
		this.txtduedays.Size = new System.Drawing.Size(134, 23);
		this.txtduedays.TabIndex = 7;
		this.txtduedays.Text = "0";
		this.txtduedays.TextChanged += new System.EventHandler(txtduedays_TextChanged);
		this.txtduedays.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtduedays_KeyPress);
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocno.Location = new System.Drawing.Point(89, 15);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.Size = new System.Drawing.Size(95, 23);
		this.txtdocno.TabIndex = 0;
		this.txtdocno.Leave += new System.EventHandler(txtdocno_TextChanged);
		this.DocDate.CustomFormat = "dd-MMM-yy";
		this.DocDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DocDate.Location = new System.Drawing.Point(186, 15);
		this.DocDate.Name = "DocDate";
		this.DocDate.Size = new System.Drawing.Size(122, 23);
		this.DocDate.TabIndex = 1;
		this.DocDate.ValueChanged += new System.EventHandler(DocDate_ValueChanged);
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label5.Location = new System.Drawing.Point(3, 19);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(88, 15);
		this.label5.TabIndex = 88;
		this.label5.Text = "Doc No && Date";
		this.txtbillno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtbillno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtbillno.Location = new System.Drawing.Point(393, 15);
		this.txtbillno.Name = "txtbillno";
		this.txtbillno.Size = new System.Drawing.Size(110, 23);
		this.txtbillno.TabIndex = 4;
		this.combdeliverytrm.AllowNull = (DefaultableBoolean)1;
		this.combdeliverytrm.AutoCompleteMode = (AutoCompleteMode)4;
		this.combdeliverytrm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combdeliverytrm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance31).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance31).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combdeliverytrm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance32;
		((SpecialBoxBase)((UltraGridBase)this.combdeliverytrm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.combdeliverytrm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).Location = new System.Drawing.Point(393, 41);
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).Name = "combdeliverytrm";
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).Size = new System.Drawing.Size(110, 26);
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).TabIndex = 5;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label6.Location = new System.Drawing.Point(310, 19);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(85, 15);
		this.label6.TabIndex = 64;
		this.label6.Text = "Manual Bill No";
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.label51.Location = new System.Drawing.Point(727, 45);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(83, 19);
		this.label51.TabIndex = 103;
		this.label51.Text = "Bill Amount";
		this.txtremarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtremarks.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtremarks.Location = new System.Drawing.Point(89, 69);
		this.txtremarks.Name = "txtremarks";
		this.txtremarks.Size = new System.Drawing.Size(874, 23);
		this.txtremarks.TabIndex = 3;
		this.label24.AutoSize = true;
		this.label24.BackColor = System.Drawing.Color.White;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label24.Location = new System.Drawing.Point(3, 73);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(89, 15);
		this.label24.TabIndex = 102;
		this.label24.Text = "Invoice Remark";
		this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label7.Location = new System.Drawing.Point(3, 47);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(69, 15);
		this.label7.TabIndex = 65;
		this.label7.Text = "Party Name";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label4.Location = new System.Drawing.Point(507, 19);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(89, 15);
		this.label4.TabIndex = 100;
		this.label4.Text = "Payment Terms";
		this.comsupplier.AllowNull = (DefaultableBoolean)1;
		this.comsupplier.AutoCompleteMode = (AutoCompleteMode)4;
		this.comsupplier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comsupplier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance163).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comsupplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance163;
		((UltraGridBase)this.comsupplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comsupplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance33).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance33).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comsupplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comsupplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance35;
		((SpecialBoxBase)((UltraGridBase)this.comsupplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance166).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance166).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance166).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance166).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comsupplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance166;
		((UltraGridBase)this.comsupplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comsupplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance167).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance167;
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance168).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance169).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance169;
		((AppearanceBase)appearance170).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance170).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance170;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance171).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance171).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance171).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance171).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance171).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance171;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.comsupplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comsupplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comsupplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comsupplier).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.comsupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comsupplier).Location = new System.Drawing.Point(89, 41);
		((System.Windows.Forms.Control)(object)this.comsupplier).Name = "comsupplier";
		((System.Windows.Forms.Control)(object)this.comsupplier).Size = new System.Drawing.Size(219, 26);
		((System.Windows.Forms.Control)(object)this.comsupplier).TabIndex = 2;
		this.comsupplier.ValueChanged += new System.EventHandler(comsupplier_ValueChanged);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label1.Location = new System.Drawing.Point(727, 19);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(57, 15);
		this.label1.TabIndex = 98;
		this.label1.Text = "Due Date";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label10.Location = new System.Drawing.Point(507, 47);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(58, 15);
		this.label10.TabIndex = 98;
		this.label10.Text = "Due Days";
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label29.Location = new System.Drawing.Point(310, 47);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(80, 15);
		this.label29.TabIndex = 101;
		this.label29.Text = "Delivery Term";
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label44.Location = new System.Drawing.Point(1265, 57);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(72, 13);
		this.label44.TabIndex = 44;
		this.label44.Text = "Validite Date";
		this.label44.Visible = false;
		this.validate.CustomFormat = "dd-MM-yy";
		this.validate.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.validate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.validate.Location = new System.Drawing.Point(1355, 49);
		this.validate.Name = "validate";
		this.validate.Size = new System.Drawing.Size(78, 21);
		this.validate.TabIndex = 19;
		this.validate.Visible = false;
		this.panel11.BackColor = System.Drawing.Color.Teal;
		this.panel11.Controls.Add(this.label17);
		this.panel11.Controls.Add(this.ChkPrintslip);
		this.panel11.Controls.Add(this.ChkBok);
		this.panel11.Controls.Add(this.ChkResetOnSave);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(0, 0);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(970, 31);
		this.panel11.TabIndex = 1;
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.ForeColor = System.Drawing.Color.White;
		this.label17.Location = new System.Drawing.Point(4, 6);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(228, 19);
		this.label17.TabIndex = 34;
		this.label17.Text = "SALE DIRECT INVOICE       ";
		this.ChkPrintslip.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkPrintslip.AutoSize = true;
		this.ChkPrintslip.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkPrintslip.ForeColor = System.Drawing.Color.White;
		this.ChkPrintslip.Location = new System.Drawing.Point(543, 6);
		this.ChkPrintslip.Margin = new System.Windows.Forms.Padding(2);
		this.ChkPrintslip.Name = "ChkPrintslip";
		this.ChkPrintslip.Size = new System.Drawing.Size(90, 20);
		this.ChkPrintslip.TabIndex = 0;
		this.ChkPrintslip.TabStop = false;
		this.ChkPrintslip.Text = "Print Slip";
		this.ChkPrintslip.UseVisualStyleBackColor = true;
		this.ChkBok.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkBok.AutoSize = true;
		this.ChkBok.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkBok.ForeColor = System.Drawing.Color.White;
		this.ChkBok.Location = new System.Drawing.Point(633, 6);
		this.ChkBok.Margin = new System.Windows.Forms.Padding(2);
		this.ChkBok.Name = "ChkBok";
		this.ChkBok.Size = new System.Drawing.Size(124, 20);
		this.ChkBok.TabIndex = 1;
		this.ChkBok.TabStop = false;
		this.ChkBok.Text = "Print Voucher";
		this.ChkBok.UseVisualStyleBackColor = true;
		this.ChkResetOnSave.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkResetOnSave.AutoSize = true;
		this.ChkResetOnSave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkResetOnSave.ForeColor = System.Drawing.Color.White;
		this.ChkResetOnSave.Location = new System.Drawing.Point(764, 6);
		this.ChkResetOnSave.Margin = new System.Windows.Forms.Padding(2);
		this.ChkResetOnSave.Name = "ChkResetOnSave";
		this.ChkResetOnSave.Size = new System.Drawing.Size(198, 20);
		this.ChkResetOnSave.TabIndex = 2;
		this.ChkResetOnSave.TabStop = false;
		this.ChkResetOnSave.Text = "Reset On Add and  Save";
		this.ChkResetOnSave.TextAlign = System.Drawing.ContentAlignment.TopLeft;
		this.ChkResetOnSave.UseVisualStyleBackColor = true;
		this.deliverystartdate.CustomFormat = "dd-MM-yy";
		this.deliverystartdate.Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.deliverystartdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.deliverystartdate.Location = new System.Drawing.Point(1355, 80);
		this.deliverystartdate.Name = "deliverystartdate";
		this.deliverystartdate.Size = new System.Drawing.Size(78, 21);
		this.deliverystartdate.TabIndex = 21;
		this.deliverystartdate.Visible = false;
		this.comproject.AllowNull = (DefaultableBoolean)1;
		this.comproject.AutoCompleteMode = (AutoCompleteMode)4;
		this.comproject.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comproject.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance173).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance173).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comproject).DisplayLayout.Appearance = (AppearanceBase)(object)appearance173;
		((UltraGridBase)this.comproject).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comproject).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance174).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance174).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance174).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance174).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comproject).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance174;
		((AppearanceBase)appearance175).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comproject).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance175;
		((SpecialBoxBase)((UltraGridBase)this.comproject).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance176).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance176).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance176).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance176).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comproject).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance176;
		((UltraGridBase)this.comproject).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comproject).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance177).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comproject).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance177;
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance178).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comproject).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance178;
		((UltraGridBase)this.comproject).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comproject).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance179;
		((AppearanceBase)appearance181).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance181).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance181;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance182).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance182).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance182).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance182).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance182).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comproject).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance182;
		((AppearanceBase)appearance183).TextHAlignAsString = "Left";
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance183;
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance184).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance184).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comproject).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance184;
		((UltraGridBase)this.comproject).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance185).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comproject).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance185;
		((UltraGridBase)this.comproject).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comproject).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comproject).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comproject).Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.comproject.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comproject).Location = new System.Drawing.Point(1513, 79);
		((System.Windows.Forms.Control)(object)this.comproject).Name = "comproject";
		((System.Windows.Forms.Control)(object)this.comproject).Size = new System.Drawing.Size(78, 23);
		((System.Windows.Forms.Control)(object)this.comproject).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.comproject).TabStop = false;
		((System.Windows.Forms.Control)(object)this.comproject).Visible = false;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(1265, 108);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(77, 13);
		this.label27.TabIndex = 49;
		this.label27.Text = "Delivery Days";
		this.label27.Visible = false;
		this.combranches.AllowNull = (DefaultableBoolean)1;
		this.combranches.AutoCompleteMode = (AutoCompleteMode)4;
		this.combranches.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combranches.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combranches).DisplayLayout.Appearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.combranches).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combranches).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance54).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance54).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance54).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combranches).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance54;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combranches).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance56;
		((SpecialBoxBase)((UltraGridBase)this.combranches).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance57).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance57).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combranches).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.combranches).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combranches).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combranches).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance58;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance59).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combranches).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.combranches).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combranches).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance60;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance61).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance62).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance62).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance62).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance62).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combranches).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance62;
		((AppearanceBase)appearance187).TextHAlignAsString = "Left";
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance187;
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance188).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance188).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combranches).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance188;
		((UltraGridBase)this.combranches).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance189).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combranches).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance189;
		((UltraGridBase)this.combranches).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combranches).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combranches).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combranches).Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combranches.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combranches).Location = new System.Drawing.Point(1513, 111);
		((System.Windows.Forms.Control)(object)this.combranches).Name = "combranches";
		((System.Windows.Forms.Control)(object)this.combranches).Size = new System.Drawing.Size(78, 23);
		((System.Windows.Forms.Control)(object)this.combranches).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.combranches).TabStop = false;
		((System.Windows.Forms.Control)(object)this.combranches).Visible = false;
		this.comstockparty.AllowNull = (DefaultableBoolean)1;
		this.comstockparty.AutoCompleteMode = (AutoCompleteMode)4;
		this.comstockparty.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comstockparty.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comstockparty).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.comstockparty).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comstockparty).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance79).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance79).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance79).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comstockparty).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance124).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comstockparty).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance124;
		((SpecialBoxBase)((UltraGridBase)this.comstockparty).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance139).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance139).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance139).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comstockparty).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance139;
		((UltraGridBase)this.comstockparty).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comstockparty).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance150).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comstockparty).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance150;
		((AppearanceBase)appearance161).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance161).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comstockparty).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance161;
		((UltraGridBase)this.comstockparty).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comstockparty).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comstockparty).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance172;
		((AppearanceBase)appearance180).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance180).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comstockparty).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance180;
		((UltraGridBase)this.comstockparty).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comstockparty).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance186).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance186).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance186).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance186).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance186).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comstockparty).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance186;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.comstockparty).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.comstockparty).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comstockparty).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comstockparty).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.comstockparty).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comstockparty).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.comstockparty).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comstockparty).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comstockparty).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comstockparty).Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.comstockparty.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comstockparty).Location = new System.Drawing.Point(1355, 167);
		((System.Windows.Forms.Control)(object)this.comstockparty).Name = "comstockparty";
		((System.Windows.Forms.Control)(object)this.comstockparty).Size = new System.Drawing.Size(78, 23);
		((System.Windows.Forms.Control)(object)this.comstockparty).TabIndex = 7;
		((System.Windows.Forms.Control)(object)this.comstockparty).Visible = false;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(1438, 117);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(74, 13);
		this.label13.TabIndex = 86;
		this.label13.Text = "Branch Name";
		this.label13.Visible = false;
		this.compartyreference.AllowNull = (DefaultableBoolean)1;
		this.compartyreference.AutoCompleteMode = (AutoCompleteMode)4;
		this.compartyreference.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.compartyreference.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.compartyreference).DisplayLayout.Appearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.compartyreference).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.compartyreference).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance64).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance64).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance64).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.compartyreference).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance64;
		((AppearanceBase)appearance65).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.compartyreference).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance65;
		((SpecialBoxBase)((UltraGridBase)this.compartyreference).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance67).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance67).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.compartyreference).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.compartyreference).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.compartyreference).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.compartyreference).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance68;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.compartyreference).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.compartyreference).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.compartyreference).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.compartyreference).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance156;
		((AppearanceBase)appearance157).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance157).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.compartyreference).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.compartyreference).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.compartyreference).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance158).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance158).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance158).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance158).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.compartyreference).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance158;
		((AppearanceBase)appearance70).TextHAlignAsString = "Left";
		((UltraGridBase)this.compartyreference).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.compartyreference).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.compartyreference).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance160).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.compartyreference).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance160;
		((UltraGridBase)this.compartyreference).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.compartyreference).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance162;
		((UltraGridBase)this.compartyreference).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.compartyreference).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.compartyreference).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.compartyreference).Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.compartyreference.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.compartyreference).Location = new System.Drawing.Point(1355, 139);
		((System.Windows.Forms.Control)(object)this.compartyreference).Name = "compartyreference";
		((System.Windows.Forms.Control)(object)this.compartyreference).Size = new System.Drawing.Size(78, 23);
		((System.Windows.Forms.Control)(object)this.compartyreference).TabIndex = 6;
		((System.Windows.Forms.Control)(object)this.compartyreference).Visible = false;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(1438, 85);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(75, 13);
		this.label18.TabIndex = 87;
		this.label18.Text = "Project Name";
		this.label18.Visible = false;
		this.txtCommAgentId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCommAgentId.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCommAgentId.Location = new System.Drawing.Point(1315, 139);
		this.txtCommAgentId.Multiline = true;
		this.txtCommAgentId.Name = "txtCommAgentId";
		this.txtCommAgentId.Size = new System.Drawing.Size(16, 26);
		this.txtCommAgentId.TabIndex = 62;
		this.txtCommAgentId.Visible = false;
		this.txtSupplierGLId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSupplierGLId.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSupplierGLId.Location = new System.Drawing.Point(1335, 139);
		this.txtSupplierGLId.Multiline = true;
		this.txtSupplierGLId.Name = "txtSupplierGLId";
		this.txtSupplierGLId.Size = new System.Drawing.Size(16, 26);
		this.txtSupplierGLId.TabIndex = 61;
		this.txtSupplierGLId.Visible = false;
		this.comborderno.AllowNull = (DefaultableBoolean)1;
		this.comborderno.AutoCompleteMode = (AutoCompleteMode)4;
		this.comborderno.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comborderno.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance114).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comborderno).DisplayLayout.Appearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.comborderno).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comborderno).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance115).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance115).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comborderno).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance116).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comborderno).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance116;
		((SpecialBoxBase)((UltraGridBase)this.comborderno).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance117).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance117).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance117).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comborderno).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.comborderno).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comborderno).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance118).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comborderno).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance118;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comborderno).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.comborderno).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comborderno).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comborderno).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance130;
		((AppearanceBase)appearance131).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance131).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comborderno).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.comborderno).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comborderno).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance132).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance132).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance132).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance132).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comborderno).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance132;
		((AppearanceBase)appearance133).TextHAlignAsString = "Left";
		((UltraGridBase)this.comborderno).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.comborderno).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comborderno).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comborderno).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance134;
		((UltraGridBase)this.comborderno).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comborderno).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.comborderno).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comborderno).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comborderno).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comborderno).Font = new System.Drawing.Font("Verdana", 8.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.comborderno.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comborderno).Location = new System.Drawing.Point(1513, 47);
		((System.Windows.Forms.Control)(object)this.comborderno).Name = "comborderno";
		((System.Windows.Forms.Control)(object)this.comborderno).Size = new System.Drawing.Size(78, 23);
		((System.Windows.Forms.Control)(object)this.comborderno).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.comborderno).TabStop = false;
		((System.Windows.Forms.Control)(object)this.comborderno).Visible = false;
		((System.Windows.Forms.Control)(object)this.comborderno).Leave += new System.EventHandler(comborderno_Leave);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(1457, 55);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(30, 13);
		this.label3.TabIndex = 17;
		this.label3.Text = "Po #";
		this.label3.Visible = false;
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[18]
		{
			this.btnNew, this.btnRefresh, this.toolStripSeparator2, this.btnSave, this.btnUpdate, this.toolStripSeparator3, this.btnDelete, this.btnAttachment, this.toolStripSeparator4, this.btnSlip,
			this.btn294APrint, this.btnPrint, this.toolStripSeparator5, this.btnLoadGdn, this.btnLoadSaleOrder, this.toolStripSeparator6, this.btnShortcutKeys, this.toolStripSeparator1
		});
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(970, 25);
		this.toolStrip1.TabIndex = 26;
		this.toolStrip1.Text = "toolStrip1";
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
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
		this.btnSave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(60, 22);
		this.btnSave.Text = "&Save";
		this.btnSave.Click += new System.EventHandler(saveToolStripButton_Click);
		this.btnUpdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(74, 22);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.toolStripSeparator3.Name = "toolStripSeparator3";
		this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
		this.btnDelete.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(69, 22);
		this.btnDelete.Text = "Delete";
		this.btnDelete.Visible = false;
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.btnAttachment.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(105, 22);
		this.btnAttachment.Text = "&Attachment";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.toolStripSeparator4.Name = "toolStripSeparator4";
		this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
		this.btnSlip.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSlip.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnSlip.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSlip.Name = "btnSlip";
		this.btnSlip.Size = new System.Drawing.Size(88, 22);
		this.btnSlip.Text = "282-Print";
		this.btnSlip.Click += new System.EventHandler(btnSlip_Click);
		this.btn294APrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btn294APrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btn294APrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btn294APrint.Name = "btn294APrint";
		this.btn294APrint.Size = new System.Drawing.Size(97, 22);
		this.btn294APrint.Text = "282A-Print";
		this.btn294APrint.Click += new System.EventHandler(btn294APrint_Click);
		this.btnPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(111, 22);
		this.btnPrint.Text = "103-Voucher";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.toolStripSeparator5.Name = "toolStripSeparator5";
		this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
		this.btnLoadGdn.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLoadGdn.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadGdn.Name = "btnLoadGdn";
		this.btnLoadGdn.Size = new System.Drawing.Size(72, 22);
		this.btnLoadGdn.Text = "Load Gdn";
		this.btnLoadGdn.Visible = false;
		this.btnLoadGdn.Click += new System.EventHandler(btnLoadGdn_Click);
		this.btnLoadSaleOrder.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLoadSaleOrder.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadSaleOrder.Name = "btnLoadSaleOrder";
		this.btnLoadSaleOrder.Size = new System.Drawing.Size(143, 20);
		this.btnLoadSaleOrder.Text = "Load From Issuance";
		this.btnLoadSaleOrder.Click += new System.EventHandler(btnLoadSaleOrder_Click);
		this.toolStripSeparator6.Name = "toolStripSeparator6";
		this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
		this.btnShortcutKeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortcutKeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortcutKeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortcutKeys.Name = "btnShortcutKeys";
		this.btnShortcutKeys.Size = new System.Drawing.Size(122, 20);
		this.btnShortcutKeys.Text = "ShortCut Keys";
		this.btnShortcutKeys.Click += new System.EventHandler(btnShortcutKeys_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.tabPage2.Controls.Add(this.panel18);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistory);
		this.tabPage2.Controls.Add(this.panel4);
		this.tabPage2.Controls.Add(this.panel10);
		this.tabPage2.Controls.Add(this.panel9);
		this.tabPage2.Controls.Add(this.panel2);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(976, 582);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.grdDetail.AllowDelete = (InheritableBoolean)1;
		grdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdDetail.DynamicFiltering = true;
		this.grdDetail.FilterMode = (FilterMode)2;
		this.grdDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(3, 477);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(970, 102);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 2;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.panel18.BackColor = System.Drawing.Color.Teal;
		this.panel18.Controls.Add(this.ctrlGrdBar3);
		this.panel18.Controls.Add(this.label55);
		this.panel18.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel18.Location = new System.Drawing.Point(3, 456);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(970, 21);
		this.panel18.TabIndex = 52;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(929, 0);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.grdDetail;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(37, 21);
		this.ctrlGrdBar3.TabIndex = 34;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.label55.AutoSize = true;
		this.label55.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label55.ForeColor = System.Drawing.Color.White;
		this.label55.Location = new System.Drawing.Point(5, 3);
		this.label55.Name = "label55";
		this.label55.Size = new System.Drawing.Size(297, 17);
		this.label55.TabIndex = 35;
		this.label55.Text = "Detail Information of Above Selected Row";
		this.grdHistory.AllowDelete = (InheritableBoolean)1;
		grdHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistory.DesignTimeLayout = grdHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistory).Dock = System.Windows.Forms.DockStyle.Top;
		this.grdHistory.DynamicFiltering = true;
		this.grdHistory.FilterMode = (FilterMode)2;
		this.grdHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdHistory).Location = new System.Drawing.Point(3, 184);
		((System.Windows.Forms.Control)(object)this.grdHistory).Name = "grdHistory";
		((ScrollableBase)this.grdHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistory).Size = new System.Drawing.Size(970, 272);
		((System.Windows.Forms.Control)(object)this.grdHistory).TabIndex = 1;
		this.grdHistory.TotalRow = (InheritableBoolean)1;
		this.grdHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistory).VisualStyle = (VisualStyle)4;
		this.grdHistory.ColumnButtonClick += new ColumnActionEventHandler(grdHistory_ColumnButtonClick);
		this.grdHistory.SelectionChanged += new System.EventHandler(grdHistory_SelectionChanged);
		this.grdHistory.LinkClicked += new ColumnActionEventHandler(grdHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdHistory).DoubleClick += new System.EventHandler(grdHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(grdHistory_KeyDown);
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.ctrlGrdBar2);
		this.panel4.Controls.Add(this.label20);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(3, 162);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(970, 22);
		this.panel4.TabIndex = 50;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(929, 0);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = null;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(37, 21);
		this.ctrlGrdBar2.TabIndex = 34;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.ForeColor = System.Drawing.Color.White;
		this.label20.Location = new System.Drawing.Point(5, 1);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(176, 19);
		this.label20.TabIndex = 35;
		this.label20.Text = "Sale Invoice History ";
		this.panel10.Controls.Add(this.groupBox4);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(3, 57);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(970, 105);
		this.panel10.TabIndex = 0;
		this.groupBox4.Controls.Add(this.rdapproveddate);
		this.groupBox4.Controls.Add(this.rdmodifydate);
		this.groupBox4.Controls.Add(this.drdocdate);
		this.groupBox4.Controls.Add(this.rdentrydate);
		this.groupBox4.Controls.Add(this.btnshow);
		this.groupBox4.Controls.Add(this.label73);
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomerHistory);
		this.groupBox4.Controls.Add(this.label75);
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory);
		this.groupBox4.Controls.Add(this.txtToDateHistory);
		this.groupBox4.Controls.Add(this.label2);
		this.groupBox4.Controls.Add(this.label76);
		this.groupBox4.Controls.Add(this.txtToDocNoHistory);
		this.groupBox4.Controls.Add(this.txtFromdateHistory);
		this.groupBox4.Controls.Add(this.label77);
		this.groupBox4.Controls.Add(this.txtFromNoHistory);
		this.groupBox4.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox4.Location = new System.Drawing.Point(6, 3);
		this.groupBox4.Name = "groupBox4";
		this.groupBox4.Size = new System.Drawing.Size(748, 98);
		this.groupBox4.TabIndex = 0;
		this.groupBox4.TabStop = false;
		this.groupBox4.Text = "Filters";
		this.rdapproveddate.AutoSize = true;
		this.rdapproveddate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdapproveddate.Location = new System.Drawing.Point(642, 45);
		this.rdapproveddate.Name = "rdapproveddate";
		this.rdapproveddate.Size = new System.Drawing.Size(105, 19);
		this.rdapproveddate.TabIndex = 10;
		this.rdapproveddate.Text = "Approved Date";
		this.rdapproveddate.UseVisualStyleBackColor = true;
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(642, 17);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 9;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(561, 17);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 7;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(561, 45);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 8;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.btnshow.BackColor = System.Drawing.Color.Teal;
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 10.2f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(487, 69);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(68, 25);
		this.btnshow.TabIndex = 6;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.label73.AutoSize = true;
		this.label73.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label73.Location = new System.Drawing.Point(219, 47);
		this.label73.Name = "label73";
		this.label73.Size = new System.Drawing.Size(93, 15);
		this.label73.TabIndex = 178;
		this.label73.Text = "Customer Name";
		this.CmbCustomerHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomerHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomerHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomerHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance147).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance147).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance147).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance147;
		((AppearanceBase)appearance148).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance148;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance149).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance149).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance149).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance149;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance151).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance151;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance152).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance152;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance153;
		((AppearanceBase)appearance154).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance154).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance154;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance155).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance155).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance155).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance155).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance155;
		((AppearanceBase)appearance159).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance164).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance165;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomerHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Location = new System.Drawing.Point(314, 41);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Name = "CmbCustomerHistory";
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Size = new System.Drawing.Size(241, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).TabIndex = 5;
		this.label75.AutoSize = true;
		this.label75.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label75.Location = new System.Drawing.Point(5, 19);
		this.label75.Name = "label75";
		this.label75.Size = new System.Drawing.Size(60, 15);
		this.label75.TabIndex = 174;
		this.label75.Text = "Date Type";
		this.cmbDateTypeHistory.AllowNull = (DefaultableBoolean)1;
		this.cmbDateTypeHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDateTypeHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDateTypeHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance190).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance190).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance190;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance191).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance191).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance191).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance191).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance191;
		((AppearanceBase)appearance192).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance192;
		((SpecialBoxBase)((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance119).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance119).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance119).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance120).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance121).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance122;
		((AppearanceBase)appearance123).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance123).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance126).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance126).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance126).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance126).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance126;
		((AppearanceBase)appearance127).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDateTypeHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Location = new System.Drawing.Point(72, 13);
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Name = "cmbDateTypeHistory";
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Size = new System.Drawing.Size(142, 26);
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).TabIndex = 0;
		this.cmbDateTypeHistory.ValueChanged += new System.EventHandler(cmbDateTypeHistory_ValueChanged);
		this.txtToDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDateHistory.CustomFormat = "dd-MMM-yy";
		this.txtToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtToDateHistory.Location = new System.Drawing.Point(72, 70);
		this.txtToDateHistory.Name = "txtToDateHistory";
		this.txtToDateHistory.ShowCheckBox = true;
		this.txtToDateHistory.Size = new System.Drawing.Size(142, 23);
		this.txtToDateHistory.TabIndex = 2;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(5, 74);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(48, 15);
		this.label2.TabIndex = 171;
		this.label2.Text = "Date To";
		this.label76.AutoSize = true;
		this.label76.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label76.Location = new System.Drawing.Point(5, 47);
		this.label76.Name = "label76";
		this.label76.Size = new System.Drawing.Size(63, 15);
		this.label76.TabIndex = 171;
		this.label76.Text = "Date From";
		this.txtToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDocNoHistory.Location = new System.Drawing.Point(435, 15);
		this.txtToDocNoHistory.Name = "txtToDocNoHistory";
		this.txtToDocNoHistory.Size = new System.Drawing.Size(120, 23);
		this.txtToDocNoHistory.TabIndex = 4;
		this.txtToDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtToDocNoHistory_KeyPress);
		this.txtFromdateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromdateHistory.CustomFormat = "dd-MMM-yy";
		this.txtFromdateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromdateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtFromdateHistory.Location = new System.Drawing.Point(72, 43);
		this.txtFromdateHistory.Name = "txtFromdateHistory";
		this.txtFromdateHistory.ShowCheckBox = true;
		this.txtFromdateHistory.Size = new System.Drawing.Size(142, 23);
		this.txtFromdateHistory.TabIndex = 1;
		this.label77.AutoSize = true;
		this.label77.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label77.Location = new System.Drawing.Point(219, 19);
		this.label77.Name = "label77";
		this.label77.Size = new System.Drawing.Size(60, 15);
		this.label77.TabIndex = 173;
		this.label77.Text = "Doc From";
		this.txtFromNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFromNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromNoHistory.Location = new System.Drawing.Point(314, 15);
		this.txtFromNoHistory.Name = "txtFromNoHistory";
		this.txtFromNoHistory.Size = new System.Drawing.Size(120, 23);
		this.txtFromNoHistory.TabIndex = 3;
		this.txtFromNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFromNoHistory_KeyPress);
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.label54);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(3, 30);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(970, 27);
		this.panel9.TabIndex = 47;
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label54.ForeColor = System.Drawing.Color.White;
		this.label54.Location = new System.Drawing.Point(5, 3);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(176, 19);
		this.label54.TabIndex = 35;
		this.label54.Text = "Sale Invoice History ";
		this.panel2.Controls.Add(this.toolStrip2);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(970, 27);
		this.panel2.TabIndex = 46;
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.BtnNewHistory, this.BtnRefreshHistory });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(970, 25);
		this.toolStrip2.TabIndex = 27;
		this.toolStrip2.Text = "toolStrip2";
		this.BtnNewHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.BtnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnNewHistory.Name = "BtnNewHistory";
		this.BtnNewHistory.Size = new System.Drawing.Size(55, 22);
		this.BtnNewHistory.Text = "New";
		this.BtnNewHistory.Click += new System.EventHandler(btnLoadAll_Click);
		this.BtnRefreshHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.BtnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefreshHistory.Name = "BtnRefreshHistory";
		this.BtnRefreshHistory.Size = new System.Drawing.Size(76, 22);
		this.BtnRefreshHistory.Text = "Refresh";
		this.BtnRefreshHistory.Click += new System.EventHandler(BtnRefreshHistory_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(984, 611);
		base.Controls.Add(this.tabControl1);
		base.KeyPreview = true;
		base.Name = "frmSaleInvoiceDirect";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmSaleInvoiceDirectFlourAutoRated_FormClosing);
		base.Load += new System.EventHandler(frmSaleInvoiceDirectFlourAutoRated_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmSaleInvoiceDirectFlourAutoRated_KeyDown);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.panel16.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdInvExp).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		this.panelDetail.ResumeLayout(false);
		this.panelDetail.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.txtCity).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comWarehouse).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comRateUOM).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comPackUOM).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comCropYear).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comPackingType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comItem).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel14.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdGLedger).EndInit();
		this.panel15.ResumeLayout(false);
		this.panel15.PerformLayout();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.combpttrm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combdeliverytrm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comsupplier).EndInit();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.comproject).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combranches).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comstockparty).EndInit();
		((System.ComponentModel.ISupportInitialize)this.compartyreference).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comborderno).EndInit();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel18.ResumeLayout(false);
		this.panel18.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.panel10.ResumeLayout(false);
		this.groupBox4.ResumeLayout(false);
		this.groupBox4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDateTypeHistory).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
