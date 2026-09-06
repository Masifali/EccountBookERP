using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Accounts;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Authentication;
using Architecture.Model.Inventory;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
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
namespace Architecture.WinApp.SaleTrading
{

public class SaleOrderWithQty : Form
{
	private Rightsobjects FormRights = new Rightsobjects();

	private int BaseCurrency;

	private double BaseRate;

	private Attachment AT = new Attachment();

	private bool UpdateMode;

	private bool TaxEditable;

	private bool HasMultiCurrencyFeature;

	private int updateDetailIndex = -1;

	private int updateDetailExtraIndex = -1;

	private DropDownBind DDL = new DropDownBind();

	public int RecId;

	private DataTable table = new DataTable();

	private DataTable dtExtraDetail = new DataTable();

	private DataTable dtPaymentTerm = new DataTable();

	private DataTable dtitem = new DataTable();

	private DataTable dtitemMaster = new DataTable();

	private DataTable listUOM = new DataTable();

	private DataTable dtMasterUom = new DataTable();

	private DataTable dtTerms = new DataTable();

	private List<SaleOrderDetail> lstRemoveRecord = new List<SaleOrderDetail>();

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

	private GroupBox groupBox1;

	private TextBox txtcatsr;

	private Label label33;

	private Label label2;

	private Label label12;

	private TextBox txtsupprefno;

	private Label label7;

	private Label label1;

	private Label label8;

	private Label label13;

	private Label label18;

	private DateTimePicker DocDate;

	private Label label17;

	private TextBox txtdocno;

	private Label label26;

	private TextBox txtduedays;

	private Label label24;

	private DateTimePicker duedate;

	private DateTimePicker deliverystartdate;

	private Label label30;

	private DateTimePicker validate;

	private Label label27;

	private Label label28;

	private TextBox txtdeliverydays;

	private Panel panel6;

	private Panel panel8;

	internal UltraCombo combsuppname;

	internal GridEX grd;

	private ComboBox combamounttype;

	private Label label42;

	private TextBox txtBagWeightCut;

	private Label label9;

	private TextBox txtBagPrice;

	private Label label14;

	private Label label31;

	private ToolStripButton toolStripButton1;

	private Panel panel14;

	private CheckBox ChkBoxPrint;

	internal UltraCombo CmbAgreementNo;

	private Label label4;

	private ComboBox combruom;

	private Label label25;

	private ComboBox combcommtype;

	private Label label21;

	private Label label20;

	private Label label19;

	private TextBox txtcommremarks;

	private TextBox txtcommamount;

	private Label label11;

	internal UltraCombo combsalesman;

	internal UltraCombo cmbbranch1;

	internal UltraCombo combproject1;

	internal UltraCombo combcomp1;

	internal UltraCombo combordercat;

	internal UltraCombo combpttrm;

	private ToolStrip btnLoad;

	internal UltraCombo combdeliverytrm;

	internal UltraCombo CmbStatus;

	private ToolStripButton btnDelete;

	private Panel panel5;

	private GroupBox groupBox3;

	internal UltraCombo CmbTaxType;

	private Label label43;

	private Label label45;

	private Label label48;

	private TextBox txtTaxAmount;

	private TextBox txtTotalAmount;

	private TextBox txtTaxPercent;

	private Button btnCancelUpdateDetial;

	private Button btnUpdateDetail;

	internal UltraCombo CmbJobLot;

	internal UltraCombo CmbPackUom;

	private Label label49;

	private Label label34;

	internal UltraCombo CmbItem;

	private Button btnplus;

	private TextBox txtremarksdetail;

	private Label label47;

	private TextBox txtRate;

	private TextBox txtItemAmount;

	private TextBox txtweight;

	private TextBox txtqty;

	private Label label40;

	private Label label44;

	private Label label41;

	private Label label39;

	private Label label38;

	private Label label36;

	private Label label37;

	internal UltraCombo CmbCropyr;

	internal UltraCombo CmbRateUom;

	internal UltraCombo CmbCity;

	private Label label5;

	private ToolStripButton BtnSaveAs;

	private CtrlGrdBar ctrlGrdBar1;

	private CtrlGrdBar ctrlGrdBar2;

	private Label label6;

	private Label label3;

	internal GridEX grdDetail;

	private Panel panel13;

	private Label label15;

	private Label label10;

	private Label label16;

	internal UltraCombo CmbBillType;

	private Label label22;

	private Label label29;

	private TextBox txtExchangeRate;

	private TextBox txtFcyAmount;

	private Label label32;

	internal UltraCombo cmbCurrency;

	private Label label35;

	private TextBox txtOrderQty;

	private Label label46;

	private TextBox txtOrderWeight;

	private Label label50;

	private TextBox txtOrderAmountHeader;

	private Label label51;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripSeparator toolStripSeparator3;

	private ToolStripSeparator toolStripSeparator4;

	private ToolStripSeparator toolStripSeparator5;

	private ToolStripButton btnshortcutkeys;

	private ToolStripSeparator toolStripSeparator1;

	private TextBox txtItemDescriptions;

	private Label label52;

	private TabControl tabControl2;

	private TabPage tabPageDetail;

	private TabPage tabPagePaymentDetail;

	private Panel panel11;

	private Label label57;

	internal GridEX grdPaymentTerm;

	private ToolStripButton btnCity;

	private TextBox txtremarks;

	private TabPage tabExtraItems;

	internal GridEX grdExtraItems;

	private Panel panel10;

	private Label label53;

	private CtrlGrdBar ctrlGrdBar3;

	private Panel panel12;

	private GroupBox groupBox2;

	private TextBox txtItemDescriptionInExtra;

	private Button BtnCancelInExtra;

	private Button btnAddInExtra;

	private Button BtnUpdateInExtra;

	private TextBox txtWeightInExtra;

	private Label label62;

	private TextBox txtRemarksInExtra;

	private Label label63;

	internal UltraCombo CmbPackuomInExtra;

	internal UltraCombo CmbMasterPackUomInExtra;

	private Label label55;

	private Label label65;

	private Label label66;

	private Label label67;

	private Label label68;

	private TextBox txtQtyInExtra;

	internal UltraCombo CmbJobLotInExtra;

	internal UltraCombo CmbMasterItemInExtra;

	private Label label54;

	internal UltraCombo CmbItemInExtra;

	private Label label69;

	private RadioButton rdbtnItemCode;

	private RadioButton rdbtnItemName;

	private RadioButton RadMasterItemCode;

	private RadioButton RadExtraItemCode;

	private RadioButton RadMasterItemName;

	private RadioButton RadExtraItemName;

	private Panel panel7;

	private GroupBox groupBox4;

	private Button btnShow;

	private Label label56;

	private Label label58;

	private Label label59;

	private Label label60;

	private Label label61;

	internal UltraCombo CmbCustomerHistory;

	private TextBox ToDocNoHistory;

	private TextBox FromDocNoHistory;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	private ToolStripButton btnResetHistory;

	private ToolStripButton btnRefreshHistory;

	private Panel panel9;

	private Label label64;

	internal GridEX grdhistory;

	private FlowLayoutPanel flowLayoutPanel1;

	private TextBox txtDiscountHeader;

	private Label label71;

	private TextBox txtItemNetAmountHeader;

	private Label label70;

	private TextBox txtDiscountAmount;

	private Label label72;

	private TextBox txtDiscPercent;

	private Label label73;

	internal UltraCombo CmbDiscountType;

	private Label label74;

	private TextBox txtTaxAmountHeader;

	private Label label75;

	private RadioButton rdapproveddate;

	private RadioButton rdmodifydate;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	public SaleOrderWithQty(UserAccount user)
	{
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void PurchsaeOrder_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(10, 10);
			base.AutoScrollMinSize = new Size(1180, 625);
			FormRights = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnsave.Enabled = FormRights.DoHaveSaveRight;
			btnprint.Enabled = FormRights.DoHavePrintRights;
			btnupdate.Enabled = FormRights.DoHaveUpdateRights;
			ctrlGrdBar1.mGridChooseFielder.Enabled = FormRights.DoHaveFieldChooserRights;
			ctrlGrdBar1.mGridSaveLayouts.Enabled = FormRights.DoHaveSaveLayoutRights;
			ctrlGrdBar1.mGridPrint.Enabled = FormRights.DoHaveGridPrintRights;
			ctrlGrdBar1.mGridExport.Enabled = FormRights.DoHaveGridExportRights;
			ctrlGrdBar1.GroupCollaps.Enabled = FormRights.DoHaveGridGroupCollapseRights;
			ctrlGrdBar1.GroupExpand.Enabled = FormRights.DoHaveGridGroupExpandRights;
			ctrlGrdBar2.mGridChooseFielder.Enabled = FormRights.DoHaveFieldChooserRights;
			ctrlGrdBar2.mGridSaveLayouts.Enabled = FormRights.DoHaveSaveLayoutRights;
			ctrlGrdBar2.mGridPrint.Enabled = FormRights.DoHaveGridPrintRights;
			ctrlGrdBar2.mGridExport.Enabled = FormRights.DoHaveGridExportRights;
			ctrlGrdBar2.GroupCollaps.Enabled = FormRights.DoHaveGridGroupCollapseRights;
			ctrlGrdBar2.GroupExpand.Enabled = FormRights.DoHaveGridGroupExpandRights;
			ctrlGrdBar3.mGridChooseFielder.Enabled = FormRights.DoHaveFieldChooserRights;
			ctrlGrdBar3.mGridSaveLayouts.Enabled = FormRights.DoHaveSaveLayoutRights;
			ctrlGrdBar3.mGridPrint.Enabled = FormRights.DoHaveGridPrintRights;
			ctrlGrdBar3.mGridExport.Enabled = FormRights.DoHaveGridExportRights;
			ctrlGrdBar3.GroupCollaps.Enabled = FormRights.DoHaveGridGroupCollapseRights;
			ctrlGrdBar3.GroupExpand.Enabled = FormRights.DoHaveGridGroupExpandRights;
			TaxEditable = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("TaxPercentEditable"));
			CurrencyFill();
			MultiCurrencyFeature();
			dtitem.Columns.Add("Id", typeof(int));
			dtitem.Columns.Add("ItemName");
			dtitem.Columns.Add("ItemCode");
			dtitemMaster.Columns.Add("Id", typeof(int));
			dtitemMaster.Columns.Add("ItemName");
			dtitemMaster.Columns.Add("ItemCode");
			dtMasterUom.Columns.Add("MasterPackUomId", typeof(int));
			dtMasterUom.Columns.Add("MasterPackUom", typeof(string));
			dtMasterUom.Columns.Add("MasterItemId", typeof(int));
			table.Columns.Add("Id", typeof(int));
			table.Columns.Add("ItemId", typeof(int));
			table.Columns.Add("ItemCode", typeof(string));
			table.Columns.Add("ItemName", typeof(string));
			table.Columns.Add("ItemDescription", typeof(string));
			table.Columns.Add("Remarks", typeof(string));
			table.Columns.Add("JobLotId", typeof(int));
			table.Columns.Add("JobLot", typeof(string));
			table.Columns.Add("PackUomId", typeof(int));
			table.Columns.Add("PackUom", typeof(string));
			table.Columns.Add("ItemQty", typeof(double));
			table.Columns.Add("Weight", typeof(double));
			table.Columns.Add("ItemRate", typeof(double));
			table.Columns.Add("RateUomId", typeof(int));
			table.Columns.Add("RateUom", typeof(string));
			table.Columns.Add("DiscountType", typeof(int));
			table.Columns.Add("Discount%", typeof(double));
			table.Columns.Add("DiscountAmount", typeof(double));
			table.Columns.Add("Amount", typeof(double));
			table.Columns.Add("FcyAmount", typeof(double));
			table.Columns.Add("TaxNameId", typeof(int));
			table.Columns.Add("TaxName", typeof(string));
			table.Columns.Add("TaxPercent", typeof(double));
			table.Columns.Add("TaxAmount", typeof(double));
			table.Columns.Add("TotalAmount", typeof(double));
			table.Columns.Add("CityId", typeof(int));
			table.Columns.Add("CityName", typeof(string));
			grd.DataSource = table;
			grd.RetrieveStructure();
			grdSettings();
			dtExtraDetail.Columns.Add("Id", typeof(int));
			dtExtraDetail.Columns.Add("MasterItemId", typeof(int));
			dtExtraDetail.Columns.Add("MasterItemCode", typeof(string));
			dtExtraDetail.Columns.Add("MasterItemName", typeof(string));
			dtExtraDetail.Columns.Add("MasterPackUomId", typeof(int));
			dtExtraDetail.Columns.Add("MasterPackUom", typeof(string));
			dtExtraDetail.Columns.Add("ItemId", typeof(int));
			dtExtraDetail.Columns.Add("ItemCode", typeof(string));
			dtExtraDetail.Columns.Add("ItemName", typeof(string));
			dtExtraDetail.Columns.Add("ItemDescription", typeof(string));
			dtExtraDetail.Columns.Add("Remarks", typeof(string));
			dtExtraDetail.Columns.Add("JobLotId", typeof(int));
			dtExtraDetail.Columns.Add("JobLot", typeof(string));
			dtExtraDetail.Columns.Add("PackUomId", typeof(int));
			dtExtraDetail.Columns.Add("PackUom", typeof(string));
			dtExtraDetail.Columns.Add("ItemQty", typeof(double));
			dtExtraDetail.Columns.Add("Weight", typeof(double));
			grdExtraItems.DataSource = dtExtraDetail;
			grdExtraItems.RetrieveStructure();
			grdExtraItemsSettings();
			ChkBoxPrint.Checked = true;
			DiscountTypeFill();
			CompanyFill();
			BranchFill();
			ProjectFill();
			DocumentNoFill();
			SupplierNameFill();
			PaymentTerms();
			DeliveryTerm();
			OrderStatus();
			ItemDetailFill();
			combojoblotfill();
			CityFill();
			BillTypeComboFill();
			ConfigurationDefault();
			DocDate.Focus();
			combamounttype.SelectedIndex = 0;
			btnupdate.Visible = false;
			btnsave.Visible = true;
			btnplus.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			combcommtype.Enabled = false;
			((UltraGridBase)CmbStatus).Rows[0].Activate();
			((Control)(object)CmbStatus).Enabled = false;
			txtTaxPercent.Enabled = TaxEditable;
			dtPaymentTerm.Columns.Add("PaymentTermId", typeof(int));
			dtPaymentTerm.Columns.Add("%ofTotal", typeof(double));
			dtPaymentTerm.Columns.Add("Amount", typeof(double));
			dtPaymentTerm.Columns.Add("DueDays", typeof(int));
			dtPaymentTerm.Columns.Add("DueDate", typeof(DateTime));
			dtPaymentTerm.Columns.Add("Remarks");
			AddRowInPaymentGrid();
			grdPaymentTerm.RetrieveStructure();
			grdSettingsPaymentTerm();
			HistoryCombosFill();
			FromDateHistory.Value = DateTime.Now.AddDays(-3.0);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CompanyFill()
	{
		try
		{
			DataTable dt = CommonServices.CompanyServiceBind();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, combcomp1, "Id", "CompName", "Company Name", false);
			}
			((UltraGridBase)combcomp1).Rows[0].Activate();
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
			DataTable dtbran = CommonServices.BrancheServiceBind();
			if (dtbran.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtbran, cmbbranch1, "Id", "BranchName", "Branch Name", false);
				((UltraGridBase)cmbbranch1).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ProjectFill()
	{
		try
		{
			DataTable dt = CommonServices.ProjectServiceBind();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, combproject1, "Id", "ProjectName", "Project Name", false);
			}
			((UltraGridBase)combproject1).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SupplierNameFill()
	{
		try
		{
			int Id = Conversion.ToInt(combsuppname.Value);
			int Id2 = Conversion.ToInt(CmbCustomerHistory.Value);
			DataTable dtsupcustomer = CommonServices.SupplierCustomerGetforComboServiceBind();
			if (dtsupcustomer.Rows.Count > 0)
			{
				DDL.BindDDL(dtsupcustomer, combsuppname, "Id", "CompanyName", "Customer Name", ZeroIndex: true);
				DDL.BindDDL(dtsupcustomer, CmbCustomerHistory, "Id", "CompanyName", "Party", ZeroIndex: true);
				if (Id > 0)
				{
					if (dtsupcustomer.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
					{
						combsuppname.Value = Id;
					}
					else
					{
						((Control)(object)combsuppname).Text = string.Empty;
					}
				}
				if (Id2 > 0)
				{
					if (dtsupcustomer.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id2))
					{
						CmbCustomerHistory.Value = Id2;
					}
					else
					{
						((Control)(object)CmbCustomerHistory).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)combsuppname).Text = string.Empty;
				((UltraGridBase)combsuppname).DataSource = null;
				((Control)(object)CmbCustomerHistory).Text = string.Empty;
				((UltraGridBase)CmbCustomerHistory).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ComboAgreementNoFill()
	{
	}

	private void PaymentTerms()
	{
		try
		{
			int Id = Conversion.ToInt(combpttrm.Value);
			dtTerms.Rows.Clear();
			dtTerms = CommonServices.GetDueTermServiceBind();
			if (dtTerms.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtTerms, combpttrm, "Id", "TermsDescription", "Payment Term", true);
				((UltraGridBase)combpttrm).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				((UltraGridBase)combpttrm).Rows[1].Activate();
				if (Id > 0)
				{
					if (dtTerms.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
					{
						combpttrm.Value = Id;
					}
					else
					{
						((Control)(object)combpttrm).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)combpttrm).Text = string.Empty;
				((UltraGridBase)combpttrm).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DocumentNoFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			SaleOrder sup = new SaleOrder();
			sup.OrganizationId = UserAccount.OrganizationId;
			sup.CompanyId = UserAccount.CompanyId;
			sup.DocumentTypeId = 1605;
			sup.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			txtdocno.Text = SaleOrder.GeneratePurchaseOrderCodeByDocId(sup).ToString();
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
			int Id = Conversion.ToInt(CmbItem.Value);
			Conversion.ToInt(CmbItemInExtra.Value);
			((Control)(object)CmbItem).Text = string.Empty;
			((Control)(object)CmbItemInExtra).Text = string.Empty;
			DataTable dt = CommonServices.ItemGetForComboServiceBindForEngr();
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtitem.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ItemName"], dt.Rows[i]["ItemCodeNew"]);
				}
			}
			if (dtitem.Rows.Count > 0)
			{
				if (rdbtnItemName.Checked)
				{
					DDL.BindDDL(dtitem, CmbItem, "Id", "ItemName", "Item Name", ZeroIndex: true);
				}
				else
				{
					DDL.BindDDL(dtitem, CmbItem, "Id", "ItemCode", "Item Name", ZeroIndex: true);
				}
				((UltraGridBase)CmbItem).DisplayLayout.Bands[0].Columns[1].Width = 300;
				if (RadExtraItemName.Checked)
				{
					DDL.BindDDL(dtitem, CmbItemInExtra, "Id", "ItemName", "Item Name", ZeroIndex: true);
				}
				else
				{
					DDL.BindDDL(dtitem, CmbItemInExtra, "Id", "ItemCode", "Item Name", ZeroIndex: true);
				}
				((UltraGridBase)CmbItemInExtra).DisplayLayout.Bands[0].Columns[1].Width = 300;
			}
			else
			{
				((Control)(object)CmbItem).Text = string.Empty;
				((UltraGridBase)CmbItem).DataSource = null;
				((Control)(object)CmbItemInExtra).Text = string.Empty;
				((UltraGridBase)CmbItemInExtra).DataSource = null;
			}
			if (Id > 0)
			{
				if (dtitem.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
				{
					CmbItem.Value = Id;
				}
				else
				{
					((Control)(object)CmbItem).Text = string.Empty;
				}
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

	private void RadExtraItemName_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (dtitem.Rows.Count > 0)
			{
				int ItemId = 0;
				if (Conversion.ToInt(CmbItemInExtra.Value) > 0)
				{
					ItemId = Conversion.ToInt(CmbItemInExtra.Value);
				}
				if (RadExtraItemName.Checked)
				{
					DDL.BindDDL(dtitem, CmbItemInExtra, "Id", "ItemName", "Item Name", ZeroIndex: true);
				}
				else
				{
					DDL.BindDDL(dtitem, CmbItemInExtra, "Id", "ItemCode", "Item Name", ZeroIndex: true);
				}
				if (ItemId > 0)
				{
					CmbItemInExtra.Value = ItemId;
					CmbItemInExtra.Focus();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void bindRateUomAndItemPackUom(int ID)
	{
		try
		{
			string PackUom = ((Control)(object)CmbPackUom).Text;
			string RateUom = ((Control)(object)CmbRateUom).Text;
			((Control)(object)CmbPackUom).Text = string.Empty;
			((Control)(object)CmbRateUom).Text = string.Empty;
			DataTable dtUom = CommonServices.GetUomScheduleByItemId(Conversion.ToInt(CmbItem.Value));
			if (dtUom.Rows.Count > 0)
			{
				DDL.BindDDL(dtUom, CmbPackUom, "Id", "UomCode", "Uom", ZeroIndex: false);
				((UltraGridBase)CmbPackUom).DisplayLayout.Bands[0].Columns[1].Width = 75;
				((UltraGridBase)CmbPackUom).DisplayLayout.Bands[0].Columns[2].Width = 85;
				DDL.BindDDL(dtUom, CmbRateUom, "Id", "UomCode", "RateUom", ZeroIndex: false);
				((UltraGridBase)CmbRateUom).DisplayLayout.Bands[0].Columns[1].Width = 75;
				((UltraGridBase)CmbRateUom).DisplayLayout.Bands[0].Columns[2].Width = 85;
				if (dtUom.AsEnumerable().Any((DataRow row) => row.Field<string>("UomCode") == PackUom))
				{
					((Control)(object)CmbPackUom).Text = PackUom;
				}
				else
				{
					((Control)(object)CmbPackUom).Text = string.Empty;
				}
				if (dtUom.AsEnumerable().Any((DataRow row) => row.Field<string>("UomCode") == RateUom))
				{
					((Control)(object)CmbRateUom).Text = RateUom;
				}
				else
				{
					((Control)(object)CmbRateUom).Text = string.Empty;
				}
			}
			else
			{
				((Control)(object)CmbPackUom).Text = string.Empty;
				((Control)(object)CmbRateUom).Text = string.Empty;
				((UltraGridBase)CmbPackUom).DataSource = null;
				((UltraGridBase)CmbRateUom).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public void bindPackUomInExtra(int ID)
	{
		try
		{
			string PackUom = ((Control)(object)CmbPackuomInExtra).Text;
			((Control)(object)CmbPackuomInExtra).Text = string.Empty;
			DataTable dtUom = CommonServices.GetUomScheduleByItemId(ID);
			if (dtUom.Rows.Count > 0)
			{
				DDL.BindDDL(dtUom, CmbPackuomInExtra, "Id", "UomCode", "Uom", ZeroIndex: false);
				((UltraGridBase)CmbPackuomInExtra).DisplayLayout.Bands[0].Columns[1].Width = 75;
				((UltraGridBase)CmbPackuomInExtra).DisplayLayout.Bands[0].Columns[2].Width = 85;
				if (dtUom.AsEnumerable().Any((DataRow row) => row.Field<string>("UomCode") == PackUom))
				{
					((Control)(object)CmbPackuomInExtra).Text = PackUom;
				}
				else
				{
					((Control)(object)CmbPackuomInExtra).Text = string.Empty;
				}
			}
			else
			{
				((Control)(object)CmbPackuomInExtra).Text = string.Empty;
				((UltraGridBase)CmbPackuomInExtra).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void combojoblotfill()
	{
		try
		{
			int Id = Conversion.ToInt(CmbJobLot.Value);
			int Id2 = Conversion.ToInt(CmbJobLotInExtra.Value);
			DataTable dtJob = CommonServices.JobLotGetAllService();
			if (dtJob.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtJob, CmbJobLot, "Id", "JobLotDescription", "Job Lot", true);
				DDL.BindDDLNew(dtJob, CmbJobLotInExtra, "Id", "JobLotDescription", "Job Lot", true);
				if (Id > 0)
				{
					if (dtJob.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
					{
						CmbJobLot.Value = Id;
					}
					else
					{
						((Control)(object)CmbJobLot).Text = string.Empty;
					}
				}
				if (Id2 > 0)
				{
					if (dtJob.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id2))
					{
						CmbJobLotInExtra.Value = Id2;
					}
					else
					{
						((Control)(object)CmbJobLotInExtra).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbJobLot).Text = string.Empty;
				((UltraGridBase)CmbJobLot).DataSource = null;
				((Control)(object)CmbJobLotInExtra).Text = string.Empty;
				((UltraGridBase)CmbJobLotInExtra).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CityFill()
	{
		try
		{
			int Id = Conversion.ToInt(CmbCity.Value);
			DataTable dt = CommonServices.CityGetAllService();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbCity, "Id", "CityName", "City Name", true);
				((UltraGridBase)CmbCity).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				if (Id > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
					{
						CmbCity.Value = Id;
					}
					else
					{
						((Control)(object)CmbCity).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbCity).Text = string.Empty;
				((UltraGridBase)CmbCity).DataSource = null;
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

	private void OrderStatus()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Status");
			dt.Rows.Add(1, "Open");
			dt.Rows.Add(2, "Complete");
			dt.Rows.Add(3, "Cancel");
			DDL.BindDDL(dt, CmbStatus, "Id", "Status", "Order Status", ZeroIndex: false);
			((UltraGridBase)CmbStatus).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BillTypeComboFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Type");
			dt.Rows.Add(1, "On Weight");
			dt.Rows.Add(2, "On Qty");
			DDL.BindDDL(dt, CmbBillType, "Id", "Type", "Bill Type", ZeroIndex: false);
			((UltraGridBase)CmbBillType).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbMasterItemInExtra_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)CmbMasterItemInExtra).ActiveRow == null || Conversion.ToInt(CmbMasterItemInExtra.Value) <= 0)
			{
				return;
			}
			if (dtMasterUom.Rows.Count > 0)
			{
				string PackUom = ((Control)(object)CmbMasterPackUomInExtra).Text;
				((Control)(object)CmbMasterPackUomInExtra).Text = string.Empty;
				DataTable dtmatchingRows = dtMasterUom.Select("MasterItemId=" + Conversion.ToInt(CmbMasterItemInExtra.Value)).CopyToDataTable();
				if (dtmatchingRows.Rows.Count > 0)
				{
					DDL.BindDDL(dtmatchingRows, CmbMasterPackUomInExtra, "MasterPackUomId", "MasterPackUom", "MasterUom", ZeroIndex: false);
					((UltraGridBase)CmbMasterPackUomInExtra).DisplayLayout.Bands[0].Columns[1].Width = 75;
					if (dtmatchingRows.AsEnumerable().Any((DataRow row) => row.Field<string>("MasterPackUom") == PackUom))
					{
						((Control)(object)CmbMasterPackUomInExtra).Text = PackUom;
					}
					else
					{
						((Control)(object)CmbMasterPackUomInExtra).Text = string.Empty;
					}
				}
				else
				{
					((Control)(object)CmbMasterPackUomInExtra).Text = string.Empty;
					((UltraGridBase)CmbMasterPackUomInExtra).DataSource = null;
				}
			}
			else
			{
				((Control)(object)CmbMasterPackUomInExtra).Text = string.Empty;
				((UltraGridBase)CmbMasterPackUomInExtra).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RadMasterItemName_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (dtitemMaster.Rows.Count > 0)
			{
				int ItemId = 0;
				if (Conversion.ToInt(CmbMasterItemInExtra.Value) > 0)
				{
					ItemId = Conversion.ToInt(CmbMasterItemInExtra.Value);
				}
				if (RadMasterItemName.Checked)
				{
					DDL.BindDDL(dtitemMaster, CmbMasterItemInExtra, "Id", "ItemName", "Item Name", ZeroIndex: true);
				}
				else
				{
					DDL.BindDDL(dtitemMaster, CmbMasterItemInExtra, "Id", "ItemCode", "Item Name", ZeroIndex: true);
				}
				if (ItemId > 0)
				{
					CmbMasterItemInExtra.Value = ItemId;
					CmbMasterItemInExtra.Focus();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbItemInExtra_Leave(object sender, EventArgs e)
	{
		try
		{
			bindPackUomInExtra(Conversion.ToInt(CmbItemInExtra.Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combitem_Leave_1(object sender, EventArgs e)
	{
		try
		{
			bindRateUomAndItemPackUom(Conversion.ToInt(CmbItem.Value));
			List<ConfigrationsAllocation> lstJob = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Job/Lot").ToList();
			if (lstJob.Count > 0)
			{
				int JobId = Conversion.ToInt((object)lstJob[0].ConfigKey.ToString());
				CmbJobLot.Value = JobId;
			}
			GetTaxTypeIdAndTaxPercent();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetTaxTypeIdAndTaxPercent()
	{
		try
		{
			DataTable dt = CommonServices.GetTaxTypeIdAndPercentByItemId(Conversion.ToInt(CmbItem.Value), DocDate.Value);
			if (dt.Rows.Count > 0)
			{
				DataTable dtTax = new DataTable();
				dtTax.Columns.Add("Id");
				dtTax.Columns.Add("TaxType");
				dtTax.Columns.Add("TaxPrcnt");
				dtTax.Rows.Add(dt.Rows[0]["TaxNameId"], dt.Rows[0]["TaxName"], dt.Rows[0]["TaxPercent"]);
				DDL.BindDDL(dtTax, CmbTaxType, "Id", "TaxType", "TaxType", ZeroIndex: false);
				((UltraGridBase)CmbTaxType).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)CmbTaxType).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			else
			{
				((Control)(object)CmbTaxType).Text = string.Empty;
				((UltraGridBase)CmbTaxType).DataSource = null;
				txtTaxPercent.Text = "0";
				txtTaxAmount.Text = "0";
			}
			if (updateDetailIndex > -1 && Conversion.ToInt(grd.CurrentRow.Cells["TaxNameId"].Value) > 0)
			{
				CmbTaxType.Value = Conversion.ToInt(grd.CurrentRow.Cells["TaxNameId"].Value);
			}
			CalculateTaxAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DiscountTypeFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("Name");
			dt.Rows.Add(1, "Flat");
			dt.Rows.Add(2, "Percent");
			DDL.BindDDL(dt, CmbDiscountType, "Id", "Name", "Discount Type", ZeroIndex: false);
			((UltraGridBase)CmbDiscountType).Rows[1].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CurrencyFill()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		try
		{
			int id = Conversion.ToInt(cmbCurrency.Value);
			DataTable dt = MultiCurrency.GetAll(new MultiCurrency
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDLNew(dt, cmbCurrency, "Id", "CurrencyCode", "Currency", true);
			((UltraGridBase)cmbCurrency).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			if (id > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
				{
					cmbCurrency.Value = id;
				}
				else
				{
					((Control)(object)cmbCurrency).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void MultiCurrencyFeature()
	{
		try
		{
			HasMultiCurrencyFeature = CommonServices.GetERPFeatureById(6);
			if (HasMultiCurrencyFeature)
			{
				((Control)(object)cmbCurrency).Visible = true;
				txtExchangeRate.Visible = true;
				txtFcyAmount.Visible = true;
				label35.Visible = true;
				label32.Visible = true;
				label29.Visible = true;
				txtremarks.Size = new Size(614, 48);
			}
			else
			{
				((Control)(object)cmbCurrency).Visible = false;
				txtExchangeRate.Visible = false;
				txtFcyAmount.Visible = false;
				label35.Visible = false;
				label32.Visible = false;
				label29.Visible = false;
				txtremarks.Size = new Size(827, 48);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ConfigurationDefault()
	{
		try
		{
			List<ConfigrationsAllocation> list = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "City Area").ToList();
			if (list.Count > 0)
			{
				int CityId = Conversion.ToInt((object)list[0].ConfigKey.ToString());
				if (Conversion.ToInt(CmbCity.Value) == 0)
				{
					CmbCity.Value = CityId;
				}
			}
			List<ConfigrationsAllocation> lstJob = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Job/Lot").ToList();
			if (lstJob.Count > 0)
			{
				int JobId = Conversion.ToInt((object)lstJob[0].ConfigKey.ToString());
				if (Conversion.ToInt(CmbJobLot.Value) == 0)
				{
					CmbJobLot.Value = JobId;
				}
			}
			List<ConfigrationsAllocation> lstCrop = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Default Crop Year").ToList();
			if (lstCrop.Count > 0)
			{
				int CropYearId = Conversion.ToInt((object)lstCrop[0].ConfigKey.ToString());
				if (Conversion.ToInt(CmbCropyr.Value) == 0)
				{
					CmbCropyr.Value = CropYearId;
				}
			}
			List<ConfigrationsAllocation> BaseCurrencylist = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Base Currency").ToList();
			if (BaseCurrencylist.Count > 0)
			{
				int Currency = Conversion.ToInt((object)BaseCurrencylist[0].ConfigKey.ToString());
				BaseCurrency = Currency;
				if (Conversion.ToInt(cmbCurrency.Value) == 0)
				{
					cmbCurrency.Value = BaseCurrency;
				}
			}
			List<ConfigrationsAllocation> BaseRatelist = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "BaseCurrencyRate").ToList();
			if (BaseRatelist.Count > 0)
			{
				double BaseCurrencyRate = (BaseRate = Conversion.ToDouble((object)BaseRatelist[0].ConfigKey.ToString()));
				if (Conversion.ToDouble((object)txtExchangeRate.Text) == 0.0)
				{
					txtExchangeRate.Text = BaseCurrencyRate.ToString(clsGlobalVariables.DecimalRateFormate);
				}
			}
		}
		catch (Exception)
		{
			MessageBox.Show("Confiquration list Error");
		}
	}

	private void cmbCurrency_Leave(object sender, EventArgs e)
	{
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Expected O, but got Unknown
		try
		{
			if (Conversion.ToInt(cmbCurrency.Value) == 0 || Conversion.ToDouble((object)txtExchangeRate.Text) != 0.0)
			{
				return;
			}
			if (Conversion.ToInt(cmbCurrency.Value) != BaseCurrency)
			{
				DataTable dt = global::Architecture.BLL.Accounts.VoucherHead.GetLastExchangeRateAndCurrencyOfVoucher(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					DocumentTypeIds = "1605",
					Ids = Conversion.ToString(cmbCurrency.Value)
				});
				if (dt.Rows.Count > 0)
				{
					txtExchangeRate.Text = Conversion.ToDouble(dt.Rows[0]["LastExchRate"]).ToString(clsGlobalVariables.DecimalRateFormate);
				}
				else
				{
					txtExchangeRate.Text = "0";
				}
			}
			else
			{
				txtExchangeRate.Text = BaseRate.ToString(clsGlobalVariables.DecimalRateFormate);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtExchangeRate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (grd.GetRows().Count() > 0 && Conversion.ToDecimal((object)txtExchangeRate.Text) > 0m)
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					double amount = Conversion.ToDouble(obj.Cells["Amount"].Value) / Conversion.ToDouble((object)txtExchangeRate.Text);
					obj.Cells["FcyAmount"].Value = amount;
					obj.EndEdit();
				}
			}
			else
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj2 in rows)
				{
					obj2.BeginEdit();
					obj2.Cells["FcyAmount"].Value = 0;
					obj2.EndEdit();
				}
			}
			CalculateTotalInformation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtExchangeRate_Leave(object sender, EventArgs e)
	{
		try
		{
			txtExchangeRate.Text = Conversion.ToDecimal((object)txtExchangeRate.Text).ToString(clsGlobalVariables.DecimalRateFormate);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CalculateTotalInformation()
	{
		try
		{
			if (grd.GetRows().Count() > 0)
			{
				decimal TotalQty = Conversion.ToDecimal(grd.GetTotal(grd.RootTable.Columns["ItemQty"], (AggregateFunction)2));
				decimal TotalWeight = Conversion.ToDecimal(grd.GetTotal(grd.RootTable.Columns["Weight"], (AggregateFunction)2));
				decimal TotalItemAmount = Conversion.ToDecimal(grd.GetTotal(grd.RootTable.Columns["Amount"], (AggregateFunction)2));
				decimal TotalTaxAmount = Conversion.ToDecimal(grd.GetTotal(grd.RootTable.Columns["TaxAmount"], (AggregateFunction)2));
				decimal TotalFcyAmount = Conversion.ToDecimal(grd.GetTotal(grd.RootTable.Columns["FcyAmount"], (AggregateFunction)2));
				decimal OrderNetAmount = TotalItemAmount;
				if (((UltraGridBase)combsalesman).ActiveRow != null && Conversion.ToDecimal((object)txtcommamount.Text) > 0m && Conversion.ToInt(combsuppname.Value) == Conversion.ToInt(combsalesman.Value))
				{
					OrderNetAmount -= Conversion.ToDecimal((object)txtcommamount.Text);
				}
				txtOrderQty.Text = Math.Round(TotalQty, 2).ToString("#,##0.##");
				txtOrderWeight.Text = Math.Round(TotalWeight, 2).ToString("#,##0.##");
				txtItemNetAmountHeader.Text = TotalItemAmount.ToString(clsGlobalVariables.stringFormatsingle);
				txtTaxAmountHeader.Text = TotalTaxAmount.ToString(clsGlobalVariables.stringFormatsingle);
				decimal DiscountAmountHeader = Conversion.ToDecimal((object)txtDiscountHeader.Text);
				OrderNetAmount -= DiscountAmountHeader;
				txtOrderAmountHeader.Text = OrderNetAmount.ToString(clsGlobalVariables.stringFormatsingle);
				txtFcyAmount.Text = TotalFcyAmount.ToString(clsGlobalVariables.stringFormatsingleForFcy);
			}
			else
			{
				txtOrderQty.Text = "0";
				txtOrderWeight.Text = "0";
				txtItemNetAmountHeader.Text = "0";
				txtTaxAmountHeader.Text = "0";
				txtOrderAmountHeader.Text = "0";
				txtFcyAmount.Text = "0";
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void txtDiscountHeader_TextChanged(object sender, EventArgs e)
	{
		try
		{
			txtExchangeRate_TextChanged(null, null);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (((Control)(object)cmbbranch1).Text.Trim() == string.Empty || Conversion.ToInt(cmbbranch1.Value) == 0)
		{
			MessageBox.Show("branch field is required");
			cmbbranch1.Focus();
			return false;
		}
		if (((Control)(object)combproject1).Text.Trim() == string.Empty || Conversion.ToInt(combproject1.Value) == 0)
		{
			MessageBox.Show("Project Field is Required");
			combproject1.Focus();
			return false;
		}
		if (((Control)(object)combsuppname).Text.Trim() == string.Empty || Conversion.ToInt(combsuppname.Value) == 0)
		{
			MessageBox.Show("Supplier Name Field is Required");
			combsuppname.Focus();
			return false;
		}
		if (((UltraGridBase)combpttrm).ActiveRow == null)
		{
			MessageBox.Show("Payment Term Field is Required");
			combpttrm.Focus();
			return false;
		}
		if (((Control)(object)combpttrm).Text == "Credit" && Conversion.ToInt((object)txtduedays.Text) == 0)
		{
			MessageBox.Show("Due Days Field is Required");
			txtduedays.Focus();
			return false;
		}
		if (((UltraGridBase)combdeliverytrm).ActiveRow == null)
		{
			MessageBox.Show("Delivery Term Field is Required");
			combdeliverytrm.Focus();
			return false;
		}
		if (HasMultiCurrencyFeature)
		{
			if (((UltraGridBase)cmbCurrency).ActiveRow == null || Conversion.ToInt(cmbCurrency.Value) == 0)
			{
				MessageBox.Show("Fcy Code Field is Required");
				cmbCurrency.Focus();
				return false;
			}
			if (txtExchangeRate.Text.Trim() == string.Empty || txtExchangeRate.Text.Trim() == "0")
			{
				MessageBox.Show("Exchange Rate Field is Required");
				txtExchangeRate.Focus();
				return false;
			}
			if (txtFcyAmount.Text.Trim() == string.Empty || txtFcyAmount.Text.Trim() == "0")
			{
				MessageBox.Show("Fcy Amount Rate Field is Required");
				txtFcyAmount.Focus();
				return false;
			}
		}
		else
		{
			if (((UltraGridBase)cmbCurrency).ActiveRow == null || Conversion.ToInt(cmbCurrency.Value) == 0)
			{
				MessageBox.Show("Please Configure Your Base Currency In configurations");
				cmbCurrency.Focus();
				return false;
			}
			if (txtExchangeRate.Text.Trim() == string.Empty || txtExchangeRate.Text.Trim() == "0")
			{
				MessageBox.Show("Please Configure Your Base Currency Rate In configurations");
				txtExchangeRate.Focus();
				return false;
			}
		}
		if (((UltraGridBase)CmbBillType).ActiveRow == null)
		{
			MessageBox.Show("Bill Type Field is Required");
			CmbBillType.Focus();
			return false;
		}
		return true;
	}

	private bool FormValidationDetails()
	{
		if (((UltraGridBase)CmbItem).ActiveRow == null)
		{
			MessageBox.Show("Item Name Field is Required");
			CmbItem.Focus();
			return false;
		}
		if (((UltraGridBase)CmbPackUom).ActiveRow == null)
		{
			MessageBox.Show("Pack Uom Field is Required");
			CmbPackUom.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtqty.Text) == 0.0)
		{
			MessageBox.Show("Item Qty Field is Required");
			txtqty.Focus();
			return false;
		}
		if (txtRate.Text == string.Empty)
		{
			MessageBox.Show("Item Rate Field is Required");
			txtRate.Focus();
			return false;
		}
		if (((UltraGridBase)CmbRateUom).ActiveRow == null)
		{
			MessageBox.Show("Rate UOM Field is Required");
			CmbRateUom.Focus();
			return false;
		}
		if (((UltraGridBase)CmbCity).ActiveRow == null)
		{
			MessageBox.Show("City Field is Required");
			CmbCity.Focus();
			return false;
		}
		return true;
	}

	private bool FormValidationExtraDetail()
	{
		if (((UltraGridBase)CmbMasterItemInExtra).ActiveRow != null && Conversion.ToInt(CmbMasterItemInExtra.Value) != 0 && Conversion.ToInt(CmbMasterPackUomInExtra.Value) == 0)
		{
			MessageBox.Show("Master Uom Field is Required when Master Item Is Selected");
			CmbMasterPackUomInExtra.Focus();
			return false;
		}
		if ((((UltraGridBase)CmbMasterItemInExtra).ActiveRow == null || Conversion.ToInt(CmbMasterItemInExtra.Value) == 0) && Conversion.ToInt(CmbMasterPackUomInExtra.Value) != 0)
		{
			MessageBox.Show("Master Item Field is Required when Master Uom Is Selected");
			CmbMasterItemInExtra.Focus();
			return false;
		}
		if (((UltraGridBase)CmbItemInExtra).ActiveRow == null || Conversion.ToInt(CmbItemInExtra.Value) == 0)
		{
			MessageBox.Show("Item Field is Required");
			CmbItemInExtra.Focus();
			return false;
		}
		if (((UltraGridBase)CmbPackuomInExtra).ActiveRow == null || Conversion.ToInt(CmbPackuomInExtra.Value) == 0)
		{
			MessageBox.Show("Pack Uom Field is Required");
			CmbPackuomInExtra.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtQtyInExtra.Text) == 0.0)
		{
			MessageBox.Show("Item Qty Field is Required");
			txtQtyInExtra.Focus();
			return false;
		}
		if (((UltraGridBase)CmbJobLotInExtra).ActiveRow == null || Conversion.ToInt(CmbJobLotInExtra.Value) == 0)
		{
			MessageBox.Show("JobLot Field is Required");
			CmbJobLotInExtra.Focus();
			return false;
		}
		return true;
	}

	private void btnplus_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValidationDetails())
			{
				double TaxPercent = 0.0;
				double TaxAmount = 0.0;
				if (txtTaxPercent.Text != string.Empty)
				{
					TaxPercent = Conversion.ToDouble((object)txtTaxPercent.Text);
				}
				if (txtTaxAmount.Text != string.Empty)
				{
					TaxAmount = Conversion.ToDouble((object)txtTaxAmount.Text);
				}
				table.Rows.Add(0, Conversion.ToInt(CmbItem.Value), Conversion.ToString(((UltraDropDownBase)CmbItem).SelectedRow.Cells["ItemCode"].Value), Conversion.ToString(((UltraDropDownBase)CmbItem).SelectedRow.Cells["ItemName"].Value), Conversion.ToString((object)txtItemDescriptions.Text), Conversion.ToString((object)txtremarksdetail.Text), Conversion.ToInt(CmbJobLot.Value), Conversion.ToString((object)((Control)(object)CmbJobLot).Text), Conversion.ToInt(CmbPackUom.Value), Conversion.ToString((object)((Control)(object)CmbPackUom).Text), Conversion.ToDouble((object)txtqty.Text), Conversion.ToDouble((object)txtweight.Text), Conversion.ToDouble((object)txtRate.Text), Conversion.ToInt(CmbRateUom.Value), Conversion.ToString((object)((Control)(object)CmbRateUom).Text), Conversion.ToInt(CmbDiscountType.Value), Conversion.ToDouble((object)txtDiscPercent.Text), Conversion.ToDouble((object)txtDiscountAmount.Text), Conversion.ToDouble((object)txtItemAmount.Text), (Conversion.ToDecimal((object)txtExchangeRate.Text) > 0m && Conversion.ToDecimal((object)txtItemAmount.Text) > 0m) ? (Conversion.ToDecimal((object)txtItemAmount.Text) / Conversion.ToDecimal((object)txtExchangeRate.Text)) : 0m, Conversion.ToInt(CmbTaxType.Value), Conversion.ToString((object)((Control)(object)CmbTaxType).Text), Conversion.ToDouble((object)TaxPercent), Conversion.ToDouble((object)TaxAmount), Conversion.ToDouble((object)txtTotalAmount.Text), Conversion.ToInt(CmbCity.Value), Conversion.ToString((object)((Control)(object)CmbCity).Text));
				grd.DataSource = table;
				grd.RetrieveStructure();
				grdSettings();
				((Control)(object)CmbItem).Text = string.Empty;
				txtItemDescriptions.Text = string.Empty;
				txtremarksdetail.Text = string.Empty;
				((Control)(object)CmbPackUom).Text = string.Empty;
				txtqty.Text = string.Empty;
				txtweight.Text = string.Empty;
				((Control)(object)CmbRateUom).Text = string.Empty;
				txtRate.Text = string.Empty;
				txtItemAmount.Text = string.Empty;
				((Control)(object)CmbTaxType).Text = string.Empty;
				txtTaxPercent.Text = string.Empty;
				txtTaxAmount.Text = string.Empty;
				txtTotalAmount.Text = string.Empty;
				CmbItem.Focus();
				txtExchangeRate_TextChanged(null, null);
				FillMasterItemsFromDetail();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Expected O, but got Unknown
		//IL_035f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0 || !(e.Column.Key == "Delete") || MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No || !ValidateDetailItemExistInExtra(Conversion.ToInt(r.Cells["ItemId"].Value)))
			{
				return;
			}
			if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
			{
				SaleOrderDetail vd = new SaleOrderDetail();
				vd.Id = Conversion.ToInt(r.Cells["Id"].Value);
				vd.OrderItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
				vd.ItemDiscription = Conversion.ToString(r.Cells["ItemDescription"].Value);
				vd.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
				vd.OrderItemUOMId = Conversion.ToInt(r.Cells["PackUomId"].Value);
				vd.OrderItemQty = Conversion.ToDouble(r.Cells["ItemQty"].Value);
				vd.NetWeight = Conversion.ToDouble(r.Cells["Weight"].Value);
				vd.OrderItemRate = Conversion.ToDouble(r.Cells["ItemRate"].Value);
				vd.OrderItemRateUOMId = Conversion.ToInt(r.Cells["RateUomId"].Value);
				vd.DiscountTypeId = Conversion.ToInt(r.Cells["DiscountType"].Value);
				vd.ItemDiscount = Conversion.ToDouble(r.Cells["Discount%"].Value);
				vd.ItemDiscountAmount = Conversion.ToDouble(r.Cells["DiscountAmount"].Value);
				vd.Amount = Conversion.ToDouble(r.Cells["Amount"].Value);
				vd.CityId = Conversion.ToInt(r.Cells["CityId"].Value);
				vd.CityArea = Conversion.ToString(r.Cells["CityName"].Value);
				vd.OrderRemarks = Conversion.ToString(r.Cells["Remarks"].Value);
				vd.TaxNameId = Conversion.ToInt(r.Cells["TaxNameId"].Value);
				vd.TaxPercent = Conversion.ToDouble(r.Cells["TaxPercent"].Value);
				vd.TaxAmount = Conversion.ToDouble(r.Cells["TaxAmount"].Value);
				vd.TotalAmount = Conversion.ToDouble(r.Cells["TotalAmount"].Value);
				vd.ActionTypeId = 3;
				lstRemoveRecord.Add(vd);
				table.Rows[r.RowIndex].Delete();
				if (r != null && (int)r.RowType == 0)
				{
					r.Delete();
				}
				grd.UpdateData();
			}
			else
			{
				r.Delete();
			}
			FillMasterItemsFromDetail();
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
				CmbItem.Value = item.Cells["ItemId"].Value.ToString();
				combitem_Leave_1(null, null);
				txtItemDescriptions.Text = item.Cells["ItemDescription"].Value.ToString();
				CmbJobLot.Value = item.Cells["JobLotId"].Value.ToString();
				CmbPackUom.Value = item.Cells["PackUomId"].Value.ToString();
				txtqty.Text = Conversion.ToDouble(item.Cells["ItemQty"].Value).ToString("#,##0.###");
				txtweight.Text = Conversion.ToDouble(item.Cells["Weight"].Value).ToString("#,##0.###");
				txtRate.Text = Conversion.ToDouble(item.Cells["ItemRate"].Value).ToString(clsGlobalVariables.DecimalRateFormate);
				CmbRateUom.Value = item.Cells["RateUomId"].Value.ToString();
				CmbRateUom.Value = item.Cells["RateUom"].Value.ToString();
				CmbDiscountType.Value = item.Cells["DiscountType"].Value;
				txtDiscPercent.Text = Conversion.ToDouble(item.Cells["Discount%"].Value).ToString("#,##0.###");
				txtDiscountAmount.Text = Conversion.ToDouble(item.Cells["DiscountAmount"].Value).ToString(clsGlobalVariables.stringFormatsingle);
				txtItemAmount.Text = Conversion.ToDouble(item.Cells["Amount"].Value).ToString(clsGlobalVariables.stringFormatsingle);
				CmbTaxType.Value = item.Cells["TaxNameId"].Value.ToString();
				((Control)(object)CmbTaxType).Text = item.Cells["TaxName"].Value.ToString();
				txtTaxPercent.Text = Conversion.ToDouble(item.Cells["TaxPercent"].Value).ToString("#,##0.####");
				txtTaxAmount.Text = Conversion.ToDouble(item.Cells["TaxAmount"].Value).ToString(clsGlobalVariables.stringFormatsingle);
				txtTotalAmount.Text = Conversion.ToDouble(item.Cells["TotalAmount"].Value).ToString(clsGlobalVariables.stringFormatsingle);
				txtremarksdetail.Text = item.Cells["Remarks"].Value.ToString();
				CmbCity.Value = Conversion.ToInt(item.Cells["CityId"].Value);
				btnplus.Visible = false;
				btnUpdateDetail.Visible = true;
				btnCancelUpdateDetial.Visible = true;
				CmbItem.Focus();
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
			if (FormValidationDetails())
			{
				table.Rows[updateDetailIndex]["ItemId"] = Conversion.ToInt(CmbItem.Value);
				table.Rows[updateDetailIndex]["ItemName"] = Conversion.ToString(((UltraDropDownBase)CmbItem).SelectedRow.Cells["ItemName"].Value);
				table.Rows[updateDetailIndex]["ItemCode"] = Conversion.ToString(((UltraDropDownBase)CmbItem).SelectedRow.Cells["ItemCode"].Value);
				table.Rows[updateDetailIndex]["ItemDescription"] = Conversion.ToString((object)txtItemDescriptions.Text);
				table.Rows[updateDetailIndex]["Remarks"] = Conversion.ToString((object)txtremarksdetail.Text);
				table.Rows[updateDetailIndex]["JobLotId"] = Conversion.ToInt(CmbJobLot.Value);
				table.Rows[updateDetailIndex]["JobLot"] = Conversion.ToString((object)((Control)(object)CmbJobLot).Text);
				table.Rows[updateDetailIndex]["PackUomId"] = Conversion.ToInt(CmbPackUom.Value);
				table.Rows[updateDetailIndex]["PackUom"] = Conversion.ToString((object)((Control)(object)CmbPackUom).Text);
				table.Rows[updateDetailIndex]["ItemQty"] = Conversion.ToDouble((object)txtqty.Text);
				table.Rows[updateDetailIndex]["Weight"] = Conversion.ToDouble((object)txtweight.Text);
				table.Rows[updateDetailIndex]["ItemRate"] = Conversion.ToDouble((object)txtRate.Text);
				table.Rows[updateDetailIndex]["RateUomId"] = Conversion.ToInt(CmbRateUom.Value);
				table.Rows[updateDetailIndex]["RateUom"] = Conversion.ToString((object)((Control)(object)CmbRateUom).Text);
				table.Rows[updateDetailIndex]["DiscountType"] = Conversion.ToInt(CmbDiscountType.Value);
				table.Rows[updateDetailIndex]["Discount%"] = Conversion.ToDouble((object)txtDiscPercent.Text);
				table.Rows[updateDetailIndex]["DiscountAmount"] = Conversion.ToDouble((object)txtDiscountAmount.Text);
				table.Rows[updateDetailIndex]["Amount"] = Conversion.ToDouble((object)txtItemAmount.Text);
				table.Rows[updateDetailIndex]["TaxNameId"] = Conversion.ToInt(CmbTaxType.Value);
				table.Rows[updateDetailIndex]["TaxName"] = Conversion.ToString((object)((Control)(object)CmbTaxType).Text);
				table.Rows[updateDetailIndex]["TaxPercent"] = Conversion.ToDouble((object)txtTaxPercent.Text);
				table.Rows[updateDetailIndex]["TaxAmount"] = Conversion.ToDouble((object)txtTaxAmount.Text);
				table.Rows[updateDetailIndex]["TotalAmount"] = Conversion.ToDouble((object)txtTotalAmount.Text);
				table.Rows[updateDetailIndex]["CityId"] = Conversion.ToInt(CmbCity.Value);
				table.Rows[updateDetailIndex]["CityName"] = Conversion.ToString((object)((Control)(object)CmbCity).Text);
				grd.DataSource = table;
				btnplus.Visible = true;
				btnUpdateDetail.Visible = false;
				btnCancelUpdateDetial.Visible = false;
				((Control)(object)CmbItem).Text = string.Empty;
				txtItemDescriptions.Text = string.Empty;
				txtremarksdetail.Text = string.Empty;
				((Control)(object)CmbPackUom).Text = string.Empty;
				txtqty.Text = string.Empty;
				txtweight.Text = string.Empty;
				((Control)(object)CmbRateUom).Text = string.Empty;
				txtRate.Text = string.Empty;
				txtItemAmount.Text = string.Empty;
				((Control)(object)CmbTaxType).Text = string.Empty;
				txtTaxPercent.Text = string.Empty;
				txtTaxAmount.Text = string.Empty;
				txtTotalAmount.Text = string.Empty;
				updateDetailIndex = -1;
				CmbItem.Focus();
				txtExchangeRate_TextChanged(null, null);
				FillMasterItemsFromDetail();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelUpdateDetial_Click(object sender, EventArgs e)
	{
		((Control)(object)CmbItem).Text = string.Empty;
		txtItemDescriptions.Text = string.Empty;
		txtremarksdetail.Text = string.Empty;
		((Control)(object)CmbPackUom).Text = string.Empty;
		txtqty.Text = string.Empty;
		txtweight.Text = string.Empty;
		((Control)(object)CmbRateUom).Text = string.Empty;
		txtRate.Text = string.Empty;
		txtItemAmount.Text = string.Empty;
		((Control)(object)CmbTaxType).Text = string.Empty;
		txtTaxPercent.Text = string.Empty;
		txtTaxAmount.Text = string.Empty;
		txtTotalAmount.Text = string.Empty;
		CmbItem.Focus();
		btnplus.Visible = true;
		btnUpdateDetail.Visible = false;
		btnCancelUpdateDetial.Visible = false;
	}

	private void grd_CellUpdated(object sender, ColumnActionEventArgs e)
	{
	}

	public void grdSettings()
	{
		try
		{
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["ItemId"].Visible = false;
			grd.RootTable.Columns["JobLotId"].Visible = false;
			grd.RootTable.Columns["PackUomId"].Visible = false;
			grd.RootTable.Columns["RateUomId"].Visible = false;
			grd.RootTable.Columns["TaxNameId"].Visible = false;
			grd.RootTable.Columns["CityId"].Visible = false;
			grd.RootTable.Columns["FcyAmount"].Visible = HasMultiCurrencyFeature;
			grd.RootTable.Columns["ItemQty"].Caption = "Qty";
			grd.RootTable.Columns["Remarks"].Caption = "Specification/Remarks";
			grd.RootTable.Columns["Discount%"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Discount%"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grd.RootTable.Columns["DiscountAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["DiscountAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["DiscountAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["DiscountAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemQty"].FormatString = "#,##0.###";
			grd.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Weight"].FormatString = "#,##0.###";
			grd.RootTable.Columns["Weight"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grd.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["TotalAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["TotalAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["TotalAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["TotalAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["TaxAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["TaxAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["TaxAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["TaxAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["FcyAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["FcyAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["FcyAmount"].FormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grd.RootTable.Columns["FcyAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingleForFcy;
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("Name");
			dt.Rows.Add(1, "Flat");
			dt.Rows.Add(2, "Percent");
			grd.RootTable.Columns["DiscountType"].ColumnType = (ColumnType)0;
			grd.RootTable.Columns["DiscountType"].HasValueList = true;
			grd.RootTable.Columns["DiscountType"].LimitToList = true;
			grd.RootTable.Columns["DiscountType"].EditType = (EditType)4;
			grd.RootTable.Columns["DiscountType"].ValueList.PopulateValueList((IEnumerable)dt.DefaultView, "Id", "Name");
			grd.RootTable.Columns["DiscountType"].Width = 60;
			CommonServices.GridAutoAdjustmentNew(grd);
			grd.RootTable.Columns["ItemDescription"].Width = 200;
			grd.RootTable.Columns["Remarks"].Width = 200;
			grd.RootTable.Columns.Add("Delete");
			grd.RootTable.Columns["Delete"].Key = "Delete";
			grd.RootTable.Columns["Delete"].Caption = "X ";
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

	private bool ValidateDetailItemExistInExtra(int ItemId)
	{
		bool IsExist = false;
		try
		{
			if (dtExtraDetail.Rows.Count > 0 && ItemId > 0)
			{
				if (dtExtraDetail.AsEnumerable().Any((DataRow row) => row.Field<int>("MasterItemId") == ItemId))
				{
					if (MessageBox.Show("The item you are trying to delete exists in the Extra Items grid as a master item.\nif You Delete this row, The rows containing that item in Extra Item Grid will also be deleted.\nAre you sure you want to delete it?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						IsExist = false;
					}
					else
					{
						GridEXRow[] rows = grdExtraItems.GetRows();
						foreach (GridEXRow r in rows)
						{
							if (Conversion.ToInt(r.Cells["MasterItemId"].Value) == ItemId)
							{
								dtExtraDetail.Rows[r.RowIndex].Delete();
							}
						}
						grdExtraItems.DataSource = dtExtraDetail;
						IsExist = true;
					}
				}
				else
				{
					IsExist = true;
				}
			}
			else
			{
				IsExist = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		return IsExist;
	}

	private void FillMasterItemsFromDetail()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grd.CurrentRow;
			if (item == null || (int)item.RowType != 0 || item.Cells.Count <= 0)
			{
				return;
			}
			int Id = Conversion.ToInt(CmbMasterItemInExtra.Value);
			dtitemMaster.Clear();
			dtMasterUom.Clear();
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow r in rows)
			{
				if (!dtitemMaster.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Conversion.ToInt(r.Cells["ItemId"].Value)))
				{
					dtitemMaster.Rows.Add(Conversion.ToInt(r.Cells["ItemId"].Value), Conversion.ToString(r.Cells["ItemName"].Value), Conversion.ToString(r.Cells["ItemCode"].Value));
				}
				if (!dtMasterUom.AsEnumerable().Any((DataRow row) => row.Field<int>("MasterPackUomId") == Conversion.ToInt(r.Cells["PackUomId"].Value) && row.Field<int>("MasterItemId") == Conversion.ToInt(r.Cells["ItemId"].Value)))
				{
					dtMasterUom.Rows.Add(Conversion.ToInt(r.Cells["PackUomId"].Value), Conversion.ToString(r.Cells["PackUom"].Value), Conversion.ToInt(r.Cells["ItemId"].Value));
				}
			}
			if (dtitemMaster.Rows.Count > 0)
			{
				if (RadMasterItemName.Checked)
				{
					DDL.BindDDL(dtitemMaster, CmbMasterItemInExtra, "Id", "ItemName", "Item Name", ZeroIndex: true);
				}
				else
				{
					DDL.BindDDL(dtitemMaster, CmbMasterItemInExtra, "Id", "ItemCode", "Item Name", ZeroIndex: true);
				}
				((UltraGridBase)CmbMasterItemInExtra).DisplayLayout.Bands[0].Columns[1].Width = 300;
				if (Id > 0)
				{
					if (dtitemMaster.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
					{
						CmbMasterItemInExtra.Value = Id;
					}
					else
					{
						((Control)(object)CmbMasterItemInExtra).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbMasterItemInExtra).Text = string.Empty;
				((UltraGridBase)CmbMasterItemInExtra).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAddInExtra_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValidationExtraDetail())
			{
				string MasterItemName = "";
				string MasterItemCode = "";
				if (Conversion.ToInt(CmbMasterItemInExtra.Value) > 0)
				{
					MasterItemName = Conversion.ToString(((UltraDropDownBase)CmbMasterItemInExtra).SelectedRow.Cells["ItemName"].Value);
					MasterItemCode = Conversion.ToString(((UltraDropDownBase)CmbMasterItemInExtra).SelectedRow.Cells["ItemCode"].Value);
				}
				dtExtraDetail.Rows.Add(0, Conversion.ToInt(CmbMasterItemInExtra.Value), MasterItemCode, MasterItemName, Conversion.ToInt(CmbMasterPackUomInExtra.Value), Conversion.ToString((object)((Control)(object)CmbMasterPackUomInExtra).Text), Conversion.ToInt(CmbItemInExtra.Value), Conversion.ToString(((UltraDropDownBase)CmbItemInExtra).SelectedRow.Cells["ItemCode"].Value), Conversion.ToString(((UltraDropDownBase)CmbItemInExtra).SelectedRow.Cells["ItemName"].Value), Conversion.ToString((object)txtItemDescriptionInExtra.Text), Conversion.ToString((object)txtRemarksInExtra.Text), Conversion.ToInt(CmbJobLotInExtra.Value), Conversion.ToString((object)((Control)(object)CmbJobLotInExtra).Text), Conversion.ToInt(CmbPackuomInExtra.Value), Conversion.ToString((object)((Control)(object)CmbPackuomInExtra).Text), Conversion.ToDouble((object)txtQtyInExtra.Text), Conversion.ToDouble((object)txtWeightInExtra.Text));
				grdExtraItems.DataSource = dtExtraDetail;
				grdExtraItems.RetrieveStructure();
				grdExtraItemsSettings();
				ResetExtraDetail();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdExtraItems_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdExtraItems.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0)
			{
				return;
			}
			if (e.Column.Key == "Delete")
			{
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					dtExtraDetail.Rows[r.RowIndex].Delete();
					r.Delete();
					grdExtraItems.UpdateData();
				}
				else
				{
					r.Delete();
					grdExtraItems.UpdateData();
				}
			}
			if (e.Column.Key == "Edit")
			{
				grdExtraItems_DoubleClick(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdExtraItems_DoubleClick(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdExtraItems.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				updateDetailExtraIndex = r.RowIndex;
				FillMasterItemsFromDetail();
				CmbMasterItemInExtra.Value = r.Cells["MasterItemId"].Value.ToString();
				CmbMasterItemInExtra_Leave(null, null);
				CmbMasterPackUomInExtra.Value = r.Cells["MasterPackUomId"].Value.ToString();
				CmbItemInExtra.Value = r.Cells["ItemId"].Value.ToString();
				CmbItemInExtra_Leave(null, null);
				CmbPackuomInExtra.Value = r.Cells["PackUomId"].Value.ToString();
				txtItemDescriptionInExtra.Text = r.Cells["ItemDescription"].Value.ToString();
				txtRemarksInExtra.Text = r.Cells["Remarks"].Value.ToString();
				CmbJobLotInExtra.Value = r.Cells["JobLotId"].Value.ToString();
				txtQtyInExtra.Text = Conversion.ToDouble(r.Cells["ItemQty"].Value).ToString("#,##0.###");
				txtWeightInExtra.Text = Conversion.ToDouble(r.Cells["Weight"].Value).ToString("#,##0.###");
				btnAddInExtra.Visible = false;
				BtnUpdateInExtra.Visible = true;
				BtnCancelInExtra.Visible = true;
				CmbMasterItemInExtra.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnUpdateInExtra_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValidationExtraDetail())
			{
				string MasterItemName = "";
				string MasterItemCode = "";
				if (Conversion.ToInt(CmbMasterItemInExtra.Value) > 0)
				{
					MasterItemName = Conversion.ToString(((UltraDropDownBase)CmbMasterItemInExtra).SelectedRow.Cells["ItemName"].Value);
					MasterItemCode = Conversion.ToString(((UltraDropDownBase)CmbMasterItemInExtra).SelectedRow.Cells["ItemCode"].Value);
				}
				dtExtraDetail.Rows[updateDetailExtraIndex]["MasterItemId"] = Conversion.ToInt(CmbMasterItemInExtra.Value);
				dtExtraDetail.Rows[updateDetailExtraIndex]["MasterItemName"] = MasterItemName;
				dtExtraDetail.Rows[updateDetailExtraIndex]["MasterItemCode"] = MasterItemCode;
				dtExtraDetail.Rows[updateDetailExtraIndex]["MasterPackUomId"] = Conversion.ToInt(CmbMasterPackUomInExtra.Value);
				dtExtraDetail.Rows[updateDetailExtraIndex]["MasterPackUom"] = Conversion.ToString((object)((Control)(object)CmbMasterPackUomInExtra).Text);
				dtExtraDetail.Rows[updateDetailExtraIndex]["ItemId"] = Conversion.ToInt(CmbItemInExtra.Value);
				dtExtraDetail.Rows[updateDetailExtraIndex]["ItemName"] = Conversion.ToString(((UltraDropDownBase)CmbItemInExtra).SelectedRow.Cells["ItemName"].Value);
				dtExtraDetail.Rows[updateDetailExtraIndex]["ItemCode"] = Conversion.ToString(((UltraDropDownBase)CmbItemInExtra).SelectedRow.Cells["ItemCode"].Value);
				dtExtraDetail.Rows[updateDetailExtraIndex]["ItemDescription"] = Conversion.ToString((object)txtItemDescriptionInExtra.Text);
				dtExtraDetail.Rows[updateDetailExtraIndex]["Remarks"] = Conversion.ToString((object)txtremarksdetail.Text);
				dtExtraDetail.Rows[updateDetailExtraIndex]["JobLotId"] = Conversion.ToInt(CmbJobLotInExtra.Value);
				dtExtraDetail.Rows[updateDetailExtraIndex]["JobLot"] = Conversion.ToString((object)((Control)(object)CmbJobLotInExtra).Text);
				dtExtraDetail.Rows[updateDetailExtraIndex]["PackUomId"] = Conversion.ToInt(CmbPackuomInExtra.Value);
				dtExtraDetail.Rows[updateDetailExtraIndex]["PackUom"] = Conversion.ToString((object)((Control)(object)CmbPackuomInExtra).Text);
				dtExtraDetail.Rows[updateDetailExtraIndex]["ItemQty"] = Conversion.ToDouble((object)txtQtyInExtra.Text);
				dtExtraDetail.Rows[updateDetailExtraIndex]["Weight"] = Conversion.ToDouble((object)txtWeightInExtra.Text);
				grdExtraItems.DataSource = dtExtraDetail;
				ResetExtraDetail();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnCancelInExtra_Click(object sender, EventArgs e)
	{
		try
		{
			ResetExtraDetail();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdExtraItems_CellUpdated(object sender, ColumnActionEventArgs e)
	{
	}

	public void grdExtraItemsSettings()
	{
		try
		{
			grdExtraItems.RootTable.Columns["Id"].Visible = false;
			grdExtraItems.RootTable.Columns["MasterItemId"].Visible = false;
			grdExtraItems.RootTable.Columns["ItemId"].Visible = false;
			grdExtraItems.RootTable.Columns["JobLotId"].Visible = false;
			grdExtraItems.RootTable.Columns["MasterPackUomId"].Visible = false;
			grdExtraItems.RootTable.Columns["PackUomId"].Visible = false;
			grdExtraItems.RootTable.Columns["ItemQty"].Caption = "Qty";
			grdExtraItems.RootTable.Columns["Remarks"].Caption = "Specification/Remarks";
			grdExtraItems.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdExtraItems.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdExtraItems.RootTable.Columns["ItemQty"].FormatString = "#,##0.###";
			grdExtraItems.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.###";
			grdExtraItems.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			grdExtraItems.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			grdExtraItems.RootTable.Columns["Weight"].FormatString = "#,##0.###";
			grdExtraItems.RootTable.Columns["Weight"].TotalFormatString = "#,##0.###";
			CommonServices.GridAutoAdjustmentNew(grdExtraItems);
			grdExtraItems.RootTable.Columns["ItemDescription"].Width = 200;
			grdExtraItems.RootTable.Columns["Remarks"].Width = 200;
			grdExtraItems.RootTable.Columns.Add("Delete");
			grdExtraItems.RootTable.Columns["Delete"].Key = "Delete";
			grdExtraItems.RootTable.Columns["Delete"].Caption = "X ";
			grdExtraItems.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdExtraItems.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdExtraItems.RootTable.Columns["Delete"].ButtonText = "X";
			grdExtraItems.RootTable.Columns["Delete"].Width = 20;
			grdExtraItems.RootTable.Columns["Delete"].Position = 0;
			grdExtraItems.RootTable.Columns.Add("Edit");
			grdExtraItems.RootTable.Columns["Edit"].Key = "Edit";
			grdExtraItems.RootTable.Columns["Edit"].Caption = "Edit";
			grdExtraItems.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdExtraItems.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdExtraItems.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdExtraItems.RootTable.Columns["Edit"].Width = 50;
			grdExtraItems.RootTable.Columns["Edit"].Position = 1;
			grdExtraItems.FrozenColumns = 2;
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInPaymentGrid()
	{
		try
		{
			dtPaymentTerm.Rows.Add(0, 0, 0, 0, DateTime.Now, "");
			grdPaymentTerm.DataSource = dtPaymentTerm;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettingsPaymentTerm()
	{
		try
		{
			grdPaymentTerm.RootTable.Columns["PaymentTermId"].ColumnType = (ColumnType)0;
			grdPaymentTerm.RootTable.Columns["PaymentTermId"].HasValueList = true;
			grdPaymentTerm.RootTable.Columns["PaymentTermId"].LimitToList = true;
			grdPaymentTerm.RootTable.Columns["PaymentTermId"].EditType = (EditType)4;
			grdPaymentTerm.RootTable.Columns["PaymentTermId"].Caption = "Payment Term";
			grdPaymentTerm.RootTable.Columns["PaymentTermId"].ValueList.PopulateValueList((IEnumerable)dtTerms.DefaultView, "Id", "TermsDescription");
			grdPaymentTerm.RootTable.Columns["PaymentTermId"].Width = 120;
			grdPaymentTerm.RootTable.Columns["DueDate"].EditType = (EditType)2;
			grdPaymentTerm.RootTable.Columns["DueDate"].FormatString = "dd-MMM-yyyy";
			grdPaymentTerm.RootTable.Columns["%ofTotal"].AggregateFunction = (AggregateFunction)2;
			grdPaymentTerm.RootTable.Columns["%ofTotal"].TextAlignment = (TextAlignment)3;
			grdPaymentTerm.RootTable.Columns["%ofTotal"].FormatString = "#,##0.####";
			grdPaymentTerm.RootTable.Columns["%ofTotal"].TotalFormatString = "#,##0.####";
			grdPaymentTerm.RootTable.Columns["DueDays"].AggregateFunction = (AggregateFunction)2;
			grdPaymentTerm.RootTable.Columns["DueDays"].TextAlignment = (TextAlignment)3;
			grdPaymentTerm.RootTable.Columns["DueDays"].FormatString = "#,##0.###";
			grdPaymentTerm.RootTable.Columns["DueDays"].TotalFormatString = "#,##0.###";
			grdPaymentTerm.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdPaymentTerm.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdPaymentTerm.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdPaymentTerm.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingleForFcy;
			CommonServices.GridAutoAdjustmentNew(grdPaymentTerm);
			grdPaymentTerm.RootTable.Columns.Add("Delete");
			grdPaymentTerm.RootTable.Columns["Delete"].Key = "Delete";
			grdPaymentTerm.RootTable.Columns["Delete"].Caption = "X";
			grdPaymentTerm.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdPaymentTerm.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdPaymentTerm.RootTable.Columns["Delete"].ButtonText = "X";
			grdPaymentTerm.RootTable.Columns["Delete"].Width = 20;
			grdPaymentTerm.RootTable.Columns.Add("Add");
			grdPaymentTerm.RootTable.Columns["Add"].Key = "Add";
			grdPaymentTerm.RootTable.Columns["Add"].Caption = "+";
			grdPaymentTerm.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdPaymentTerm.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdPaymentTerm.RootTable.Columns["Add"].ButtonText = "+";
			grdPaymentTerm.RootTable.Columns["Add"].Width = 20;
			grdPaymentTerm.RootTable.Columns["Add"].Position = 0;
			grdPaymentTerm.RootTable.Columns["Delete"].Position = 1;
			grdPaymentTerm.FrozenColumns = 2;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPaymentTerm_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdPaymentTerm.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				if (e.Column.Key == "%ofTotal" && grd.GetRows().Count() > 0)
				{
					double num = Conversion.ToDouble((object)txtOrderAmountHeader.Text);
					double prcnt = Conversion.ToDouble(item.Cells["%ofTotal"].Value);
					if (prcnt > 100.0)
					{
						MessageBox.Show("%of Total Can't Greater than 100", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						item.Cells["%ofTotal"].Value = 100;
						prcnt = 100.0;
					}
					double amount2 = num * prcnt / 100.0;
					item.Cells["Amount"].Value = Math.Round(amount2, 4);
				}
				if (e.Column.Key == "Amount")
				{
					if (grd.GetRows().Count() <= 0)
					{
						throw new Exception("No Detail Record Found");
					}
					double billAmount = Conversion.ToDouble((object)txtOrderAmountHeader.Text);
					double amount = Conversion.ToDouble(item.Cells["Amount"].Value);
					if (billAmount < amount)
					{
						MessageBox.Show("Amount Cant be Greater than Order Amount", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						item.Cells["Amount"].Value = billAmount;
						amount = billAmount;
					}
					double prcnt2 = amount * 100.0 / billAmount;
					item.Cells["%ofTotal"].Value = Math.Round(prcnt2, 8);
				}
				if (e.Column.Key == "DueDays")
				{
					int days2 = Conversion.ToInt(item.Cells["DueDays"].Value);
					item.Cells["DueDate"].Value = DocDate.Value.AddDays(days2);
				}
				if (e.Column.Key == "DueDate")
				{
					DateTime date = Conversion.ToDateTime(item.Cells["DueDate"].Value);
					if (date.Date < DocDate.Value.Date)
					{
						item.Cells["DueDate"].Value = DocDate.Value;
						throw new Exception("Due Date Can't less Than DocDate");
					}
					int days = (date.Date - DocDate.Value.Date).Days;
					item.Cells["DueDays"].Value = days;
				}
			}
			grdPaymentTerm.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPaymentTerm_UpdatingCell(object sender, UpdatingCellEventArgs e)
	{
		try
		{
			if (e.Column.Key == "%ofTotal" && !CommonServices.IsNumeric(e))
			{
				throw new Exception("Please Type Only Numeric Value");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void grdPaymentTerm_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow item = grdPaymentTerm.CurrentRow;
				dtPaymentTerm.Rows[item.RowIndex].Delete();
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
				if (dtPaymentTerm.Rows.Count == 0)
				{
					AddRowInPaymentGrid();
				}
			}
			if (e.Column.Key == "Add")
			{
				AddRowInPaymentGrid();
			}
			grdPaymentTerm.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		//IL_0409: Unknown result type (might be due to invalid IL or missing references)
		//IL_0410: Expected O, but got Unknown
		//IL_08be: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c5: Expected O, but got Unknown
		//IL_0b10: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b17: Expected O, but got Unknown
		//IL_0d6a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d71: Expected O, but got Unknown
		try
		{
			if (grd.RowCount == 0)
			{
				MessageBox.Show("Grid Record Not Found");
			}
			if (!FormValidation())
			{
				return;
			}
			SaleOrder So = new SaleOrder();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				So.Id = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			So.DocumentTypeId = 1605;
			So.OrganizationId = UserAccount.OrganizationId;
			So.CompanyId = UserAccount.CompanyId;
			So.ActionId = 1;
			So.EntryUser = UserAccount.ID;
			So.ModifyUser = UserAccount.ID;
			So.EntryDate = DateTime.Now;
			So.ModifyDate = DateTime.Now;
			So.BranchesId = Conversion.ToInt(cmbbranch1.Value);
			So.ProjectsId = Conversion.ToInt(combproject1.Value);
			So.DocDate = Conversion.ToDateTime((object)DocDate.Value);
			So.DocNo = Conversion.ToInt((object)txtdocno.Text);
			So.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			So.OrderCatagoryId = Conversion.ToInt(combordercat.Value);
			So.CatagorySrNo = Conversion.ToInt((object)txtcatsr.Text);
			So.OrderSupCustId = Conversion.ToInt(combsuppname.Value);
			So.SupplierRefNo = Conversion.ToString((object)txtsupprefno.Text);
			So.RemarksHeader = Conversion.ToString((object)txtremarks.Text);
			So.ItemAmountHeader = Conversion.ToDouble((object)txtItemNetAmountHeader.Text);
			So.TaxAmountHeader = Conversion.ToDouble((object)txtTaxAmountHeader.Text);
			So.DiscountAmountHeader = Conversion.ToDouble((object)txtDiscountHeader.Text);
			So.OrderAmount = Conversion.ToDouble((object)txtOrderAmountHeader.Text);
			So.ExchangeRate = Conversion.ToDouble((object)txtExchangeRate.Text);
			So.CurrencyId = Conversion.ToInt(cmbCurrency.Value);
			So.OrderQty = Conversion.ToDouble((object)txtOrderQty.Text);
			So.OrderWeight = Conversion.ToDouble((object)txtOrderWeight.Text);
			So.FcyAmount = Conversion.ToDouble((object)txtFcyAmount.Text);
			So.PaymentTermsId = Conversion.ToInt(combpttrm.Value);
			So.RefrenenceParty = Conversion.ToInt(CmbAgreementNo.Value);
			So.OrderDueDays = Conversion.ToInt((object)txtduedays.Text);
			So.OrderDueDate = duedate.Value;
			So.OrderExpiryDate = Conversion.ToDateTime((object)validate.Value);
			So.DeliveryTerm = Conversion.ToString((object)((Control)(object)combdeliverytrm).Text);
			So.DeliveryStartDate = Conversion.ToDateTime((object)deliverystartdate.Value);
			So.DeliveryDays = Conversion.ToInt((object)txtdeliverydays.Text);
			So.OrderStatus = ((Control)(object)CmbStatus).Text;
			So.BrokerAgentSupCustId = Conversion.ToInt(combsalesman.Value);
			So.BillCalculateTypeId = Conversion.ToInt(CmbBillType.Value);
			So.SaleOrderDetailList = new List<SaleOrderDetail>();
			So.SaleOrderExtraItemsDetaillist = new List<SaleOrderExtraItemsDetail>();
			So.SaleOrderPaymentTermsDetailslist = new List<SaleOrderPaymentTermsDetail>();
			So.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			if (lstRemoveRecord.Count > 0)
			{
				for (int j = 0; j < lstRemoveRecord.Count; j++)
				{
					So.SaleOrderDetailList.Add(lstRemoveRecord[j]);
				}
			}
			decimal PaymentDetailAmount = default(decimal);
			decimal PrcntOfTotal = default(decimal);
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				SaleOrderDetail vd2 = new SaleOrderDetail();
				vd2.Id = ((RecId > 0) ? Conversion.ToInt(r2.Cells["Id"].Value) : 0);
				vd2.OrderItemId = Conversion.ToInt(r2.Cells["ItemId"].Value);
				vd2.ItemName = Conversion.ToString((object)r2.Cells["ItemName"].Text);
				vd2.ItemDiscription = Conversion.ToString((object)r2.Cells["ItemDescription"].Text);
				vd2.JobLotId = Conversion.ToInt(r2.Cells["JobLotId"].Value);
				vd2.OrderItemUOMId = Conversion.ToInt(r2.Cells["PackUomId"].Value);
				vd2.OrderItemQty = Conversion.ToDouble(r2.Cells["itemQty"].Value);
				vd2.NetWeight = Conversion.ToDouble(r2.Cells["Weight"].Value);
				vd2.OrderItemRate = Conversion.ToDouble(r2.Cells["ItemRate"].Value);
				vd2.OrderItemRateUOMId = Conversion.ToInt(r2.Cells["RateUomId"].Value);
				vd2.DiscountTypeId = Conversion.ToInt(r2.Cells["DiscountType"].Value);
				vd2.ItemDiscount = Conversion.ToDouble(r2.Cells["Discount%"].Value);
				vd2.ItemDiscountAmount = Conversion.ToDouble(r2.Cells["DiscountAmount"].Value);
				if (vd2.DiscountTypeId > 0 && vd2.ItemDiscount <= 0.0 && vd2.ItemDiscountAmount <= 0.0)
				{
					vd2.DiscountTypeId = 0;
				}
				if (vd2.DiscountTypeId > 0 && vd2.ItemDiscount > 0.0 && vd2.ItemDiscountAmount <= 0.0)
				{
					throw new Exception("Discount Amount Required In Row#" + (r2.RowIndex + 1) + " In Detail Grid");
				}
				vd2.Amount = Conversion.ToDouble(r2.Cells["Amount"].Value);
				vd2.ExchangeRate = Conversion.ToDouble((object)txtExchangeRate.Text);
				vd2.CurrencyId = Conversion.ToInt(cmbCurrency.Value);
				vd2.FcyAmount = Conversion.ToDouble(r2.Cells["FcyAmount"].Value);
				vd2.TaxNameId = Conversion.ToInt(r2.Cells["TaxNameId"].Value);
				if (vd2.TaxNameId > 0)
				{
					vd2.TaxPercent = Conversion.ToDouble(r2.Cells["TaxPercent"].Value);
					vd2.TaxAmount = Conversion.ToDouble(r2.Cells["TaxAmount"].Value);
				}
				if (vd2.TaxNameId > 0 && vd2.TaxPercent > 0.0 && vd2.TaxAmount <= 0.0)
				{
					throw new Exception("TaxAmount Required In Row#" + (r2.RowIndex + 1) + " In Detail Grid");
				}
				vd2.TotalAmount = Conversion.ToDouble(r2.Cells["TotalAmount"].Value);
				vd2.OrderRemarks = Conversion.ToString(r2.Cells["Remarks"].Value);
				vd2.CityId = Conversion.ToInt(r2.Cells["CityId"].Value);
				vd2.CityArea = Conversion.ToString(r2.Cells["CityName"].Value);
				vd2.ActionTypeId = ((vd2.Id <= 0) ? 1 : 2);
				vd2.TaxableStatus = ((vd2.TaxNameId > 0) ? ((vd2.TaxPercent > 0.0) ? ((vd2.TaxAmount > 0.0) ? "True" : "False") : "False") : "False");
				So.SaleOrderDetailList.Add(vd2);
			}
			rows = grdExtraItems.GetRows();
			foreach (GridEXRow r in rows)
			{
				SaleOrderExtraItemsDetail vd = new SaleOrderExtraItemsDetail();
				vd.Id = ((RecId > 0) ? Conversion.ToInt(r.Cells["Id"].Value) : 0);
				vd.MasterItemId = Conversion.ToInt(r.Cells["MasterItemId"].Value);
				vd.MasterItemName = Conversion.ToString((object)r.Cells["MasterItemName"].Text);
				vd.MasterPackUomId = Conversion.ToInt(r.Cells["MasterPackUomId"].Value);
				vd.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
				vd.ItemName = Conversion.ToString((object)r.Cells["ItemName"].Text);
				vd.ItemDiscription = Conversion.ToString((object)r.Cells["ItemDescription"].Text);
				vd.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
				vd.PackUomId = Conversion.ToInt(r.Cells["PackUomId"].Value);
				vd.ItemQty = Conversion.ToDecimal(r.Cells["itemQty"].Value);
				vd.NetWeight = Conversion.ToDecimal(r.Cells["Weight"].Value);
				vd.Remarks = Conversion.ToString(r.Cells["Remarks"].Value);
				So.SaleOrderExtraItemsDetaillist.Add(vd);
			}
			if (grdPaymentTerm.GetRows().Count() <= 0)
			{
				tabControl2.SelectedIndex = 2;
				((Control)(object)grdPaymentTerm).Focus();
				MessageBox.Show("Payment Detail Record Not Found", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return;
			}
			rows = grdPaymentTerm.GetRows();
			foreach (GridEXRow row in rows)
			{
				if (Conversion.ToDecimal(row.Cells["Amount"].Value) > 0m)
				{
					SaleOrderPaymentTermsDetail obj = new SaleOrderPaymentTermsDetail();
					obj.PaymentTermId = Conversion.ToInt(row.Cells["PaymentTermId"].Value);
					obj.PrcntOfTotal = Conversion.ToDecimal(row.Cells["%ofTotal"].Value);
					obj.Amount = Conversion.ToDecimal(row.Cells["Amount"].Value);
					PrcntOfTotal += obj.PrcntOfTotal;
					PaymentDetailAmount += obj.Amount;
					obj.DueDays = Conversion.ToInt(row.Cells["DueDays"].Value);
					obj.DueDate = Conversion.ToDateTime(row.Cells["DueDate"].Value);
					obj.PaymentRemarks = Conversion.ToString(row.Cells["Remarks"].Value);
					if (obj.PaymentTermId <= 0)
					{
						MessageBox.Show("Payment Term Required in row#" + (row.RowIndex + 1), "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
						((Control)(object)grdPaymentTerm).Focus();
						return;
					}
					if (obj.PaymentTermId == 2 && obj.DueDays <= 0)
					{
						MessageBox.Show("Due Days Required In case Of Credit row in row#" + (row.RowIndex + 1), "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
						tabControl2.SelectedIndex = 2;
						((Control)(object)grdPaymentTerm).Focus();
						return;
					}
					So.SaleOrderPaymentTermsDetailslist.Add(obj);
				}
			}
			if (PaymentDetailAmount != Conversion.ToDecimal((object)So.OrderAmount))
			{
				MessageBox.Show("Payment Detail Amount " + PaymentDetailAmount.ToString("#,##0.####") + " Not Equal to Total Order Amount " + So.OrderAmount.ToString("#,##0.####"), "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				tabControl2.SelectedIndex = 2;
				((Control)(object)grdPaymentTerm).Focus();
				return;
			}
			if (PrcntOfTotal != 100m)
			{
				MessageBox.Show("Payment Detail Total% not equal to 100", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				((Control)(object)grdPaymentTerm).Focus();
				return;
			}
			for (int i = 0; i < Attachment.dtAttachments.Rows.Count; i++)
			{
				DMSAttachments attach = new DMSAttachments();
				attach.Attachment = Path.GetFileName(Attachment.dtAttachments.Rows[i][1].ToString());
				attach.ScreenName = base.Name;
				So.AttachmentsList.Add(attach);
				if (!Directory.Exists(clsGlobalVariables.AttachmentFolderPath))
				{
					throw new Exception("Attachment Folder Path Not Configure Properly");
				}
				string fileSavePath = Path.Combine(clsGlobalVariables.AttachmentFolderPath, attach.Attachment);
				if (!File.Exists(fileSavePath))
				{
					File.Copy(Attachment.dtAttachments.Rows[i][1].ToString(), fileSavePath, overwrite: true);
				}
			}
			int success = SaleOrder.Save(So);
			if (RecId > 0)
			{
				MessageBox.Show("Data Update Successfully.");
				if (AT.RemovedAttachmentListInUpdateCase.Count > 0)
				{
					for (int k = 0; k < AT.RemovedAttachmentListInUpdateCase.Count; k++)
					{
						using (FileStream fs = new FileStream(AT.RemovedAttachmentListInUpdateCase[k].Path, FileMode.Open, FileAccess.ReadWrite))
						{
							fs.Flush();
							fs.Close();
							fs.Dispose();
						}
						bool fileDeleted = false;
						int attempts = 0;
						while (!fileDeleted && attempts < 5)
						{
							try
							{
								File.Delete(AT.RemovedAttachmentListInUpdateCase[k].Path);
								fileDeleted = true;
							}
							catch (IOException)
							{
								attempts++;
								Thread.Sleep(500);
							}
						}
						if (!fileDeleted)
						{
							throw new Exception("Unable to delete file. It is in use by another process.");
						}
					}
				}
			}
			else
			{
				MessageBox.Show("Data Save Successfully.");
			}
			Reset();
			if (ChkBoxPrint.Checked)
			{
				CommonServices.SaleOrderSlipEngr_1605(success);
			}
		}
		catch (Exception ex2)
		{
			MessageBox.Show(ex2.Message);
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
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		try
		{
			if (SaleOrder.ReadSaleOrderIdbyDocNo(new SaleOrder
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocNo = Conversion.ToInt((object)txtdocno.Text.Trim()),
				DocumentTypeId = 1605
			}) == 0)
			{
				throw new Exception("Record Not Exist against this Number");
			}
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnSaveAs_Click(object sender, EventArgs e)
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
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			SaleOrder So = new SaleOrder();
			So = SaleOrder.GetByID(ID);
			if (So == null)
			{
				return;
			}
			cmbbranch1.Value = Conversion.ToInt((object)So.BranchesId);
			combproject1.Value = Conversion.ToInt((object)So.ProjectsId);
			tabControl1.SelectedIndex = 0;
			DocDate.Value = Conversion.ToDateTime((object)So.DocDate);
			txtdocno.Text = Conversion.ToString((object)So.DocNo);
			combordercat.Value = Conversion.ToInt((object)So.OrderCatagoryId);
			txtcatsr.Text = So.CatagorySrNo.ToString();
			combsuppname.Value = Conversion.ToInt((object)So.OrderSupCustId);
			txtsupprefno.Text = Conversion.ToString((object)So.SupplierRefNo);
			txtremarks.Text = Conversion.ToString((object)So.RemarksHeader);
			cmbCurrency.Value = So.CurrencyId;
			txtOrderQty.Text = So.OrderQty.ToString("#,##0.##");
			txtOrderWeight.Text = So.OrderWeight.ToString("#,##0.##");
			txtItemNetAmountHeader.Text = So.ItemAmountHeader.ToString(clsGlobalVariables.stringFormatsingle);
			txtTaxAmountHeader.Text = So.TaxAmountHeader.ToString(clsGlobalVariables.stringFormatsingle);
			txtDiscountAmount.Text = So.DiscountAmountHeader.ToString(clsGlobalVariables.stringFormatsingle);
			txtOrderAmountHeader.Text = So.OrderAmount.ToString(clsGlobalVariables.stringFormatsingle);
			txtFcyAmount.Text = So.FcyAmount.ToString(clsGlobalVariables.stringFormatsingleForFcy);
			combpttrm.Value = Conversion.ToInt((object)So.PaymentTermsId);
			txtduedays.Text = Conversion.ToString((object)So.OrderDueDays);
			duedate.Value = Conversion.ToDateTime((object)So.OrderDueDate);
			validate.Value = Conversion.ToDateTime((object)So.OrderExpiryDate);
			((Control)(object)combdeliverytrm).Text = Conversion.ToString((object)So.DeliveryTerm);
			deliverystartdate.Value = So.DeliveryStartDate;
			txtdeliverydays.Text = Conversion.ToString((object)So.DeliveryDays);
			((Control)(object)CmbStatus).Text = Conversion.ToString((object)So.OrderStatus);
			CmbBillType.Value = So.BillCalculateTypeId;
			((Control)(object)CmbStatus).Enabled = true;
			if (So.CurrencyId > 0)
			{
				txtExchangeRate.Text = Conversion.ToString((object)So.ExchangeRate);
			}
			else if (So.CurrencyId == 0)
			{
				ConfigurationDefault();
			}
			table.Clear();
			for (int k = 0; k < So.SaleOrderDetailList.Count; k++)
			{
				table.Rows.Add(So.SaleOrderDetailList[k].Id, So.SaleOrderDetailList[k].OrderItemId, So.SaleOrderDetailList[k].ItemCodeNew, So.SaleOrderDetailList[k].ItemName, So.SaleOrderDetailList[k].ItemDiscription, So.SaleOrderDetailList[k].OrderRemarks, So.SaleOrderDetailList[k].JobLotId, So.SaleOrderDetailList[k].JobLotDescription, So.SaleOrderDetailList[k].OrderItemUOMId, So.SaleOrderDetailList[k].UOMCode, So.SaleOrderDetailList[k].OrderItemQty, So.SaleOrderDetailList[k].NetWeight, So.SaleOrderDetailList[k].OrderItemRate, So.SaleOrderDetailList[k].OrderItemRateUOMId, So.SaleOrderDetailList[k].RateUom, So.SaleOrderDetailList[k].DiscountTypeId, So.SaleOrderDetailList[k].ItemDiscount, So.SaleOrderDetailList[k].ItemDiscountAmount, So.SaleOrderDetailList[k].Amount, So.SaleOrderDetailList[k].FcyAmount, So.SaleOrderDetailList[k].TaxNameId, So.SaleOrderDetailList[k].TaxName, So.SaleOrderDetailList[k].TaxPercent, So.SaleOrderDetailList[k].TaxAmount, So.SaleOrderDetailList[k].TotalAmount, So.SaleOrderDetailList[k].CityId, So.SaleOrderDetailList[k].CityArea);
			}
			grd.DataSource = table;
			grd.RetrieveStructure();
			grdSettings();
			dtExtraDetail.Clear();
			for (int j = 0; j < So.SaleOrderExtraItemsDetaillist.Count; j++)
			{
				dtExtraDetail.Rows.Add(So.SaleOrderExtraItemsDetaillist[j].Id, So.SaleOrderExtraItemsDetaillist[j].MasterItemId, So.SaleOrderExtraItemsDetaillist[j].MasterItemCode, So.SaleOrderExtraItemsDetaillist[j].MasterItemName, So.SaleOrderExtraItemsDetaillist[j].MasterPackUomId, So.SaleOrderExtraItemsDetaillist[j].MasterPackUomCode, So.SaleOrderExtraItemsDetaillist[j].ItemId, So.SaleOrderExtraItemsDetaillist[j].ItemCode, So.SaleOrderExtraItemsDetaillist[j].ItemName, So.SaleOrderExtraItemsDetaillist[j].ItemDiscription, So.SaleOrderExtraItemsDetaillist[j].Remarks, So.SaleOrderExtraItemsDetaillist[j].JobLotId, So.SaleOrderExtraItemsDetaillist[j].JobLotDescription, So.SaleOrderExtraItemsDetaillist[j].PackUomId, So.SaleOrderExtraItemsDetaillist[j].PackUomCode, So.SaleOrderExtraItemsDetaillist[j].ItemQty, So.SaleOrderExtraItemsDetaillist[j].NetWeight);
			}
			grdExtraItems.DataSource = dtExtraDetail;
			grdExtraItems.RetrieveStructure();
			grdExtraItemsSettings();
			dtPaymentTerm.Rows.Clear();
			if (So.SaleOrderPaymentTermsDetailslist != null && So.SaleOrderPaymentTermsDetailslist.Count > 0)
			{
				for (int i = 0; i < So.SaleOrderPaymentTermsDetailslist.Count; i++)
				{
					dtPaymentTerm.Rows.Add(So.SaleOrderPaymentTermsDetailslist[i].PaymentTermId, So.SaleOrderPaymentTermsDetailslist[i].PrcntOfTotal, So.SaleOrderPaymentTermsDetailslist[i].Amount, So.SaleOrderPaymentTermsDetailslist[i].DueDays, So.SaleOrderPaymentTermsDetailslist[i].DueDate, So.SaleOrderPaymentTermsDetailslist[i].PaymentRemarks);
				}
			}
			if (dtPaymentTerm.Rows.Count == 0)
			{
				AddRowInPaymentGrid();
			}
			grdPaymentTerm.DataSource = dtPaymentTerm;
			grdPaymentTerm.RetrieveStructure();
			grdSettingsPaymentTerm();
			List<DMSAttachments> byID = global::Architecture.BLL.DMSAttachments.GetByID(RecId, base.Name);
			AT = new Attachment();
			foreach (global::Architecture.Model.DMSAttachments Attachment in byID)
			{
				TempAttachments TA = new TempAttachments();
				TA.Id = 0;
				TA.Path = Attachment.Attachment;
				AT.lst.Add(TA);
			}
			btnsave.Visible = false;
			BtnSaveAs.Visible = false;
			btnupdate.Visible = true;
			txtExchangeRate_TextChanged(null, null);
			FillMasterItemsFromDetail();
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

	private void toolStripButton1_Click(object sender, EventArgs e)
	{
		CompanyFill();
		BranchFill();
		ProjectFill();
		SupplierNameFill();
		PaymentTerms();
		ItemDetailFill();
		combojoblotfill();
		CityFill();
		CurrencyFill();
		ConfigurationDefault();
	}

	private void SaleOrderWithQty_FormClosing(object sender, FormClosingEventArgs e)
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

	private void btnclose_Click(object sender, EventArgs e)
	{
		try
		{
			Close();
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
			lstRemoveRecord.Clear();
			RecId = 0;
			updateDetailExtraIndex = -1;
			updateDetailIndex = -1;
			((Control)(object)combsuppname).Text = string.Empty;
			txtsupprefno.Text = string.Empty;
			txtduedays.Text = string.Empty;
			((Control)(object)CmbBillType).Text = string.Empty;
			txtItemNetAmountHeader.Text = string.Empty;
			txtDiscountHeader.Text = string.Empty;
			txtTaxAmountHeader.Text = string.Empty;
			txtDiscPercent.Text = string.Empty;
			txtDiscountAmount.Text = string.Empty;
			((Control)(object)CmbItem).Text = string.Empty;
			((Control)(object)CmbPackUom).Text = string.Empty;
			txtqty.Text = string.Empty;
			txtweight.Text = string.Empty;
			((Control)(object)CmbRateUom).Text = string.Empty;
			txtRate.Text = string.Empty;
			txtItemAmount.Text = string.Empty;
			txtTaxPercent.Text = string.Empty;
			((Control)(object)CmbTaxType).Text = string.Empty;
			txtTaxAmount.Text = string.Empty;
			((Control)(object)CmbJobLot).Text = string.Empty;
			btnsave.Visible = true;
			btnupdate.Visible = false;
			btnplus.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			txtTotalAmount.Text = string.Empty;
			txtFcyAmount.Text = string.Empty;
			txtOrderQty.Clear();
			txtOrderWeight.Clear();
			txtOrderAmountHeader.Clear();
			ConfigurationDefault();
			DocumentNoFill();
			table.Clear();
			grd.DataSource = table;
			DocDate.Focus();
			dtPaymentTerm.Clear();
			AddRowInPaymentGrid();
			grdPaymentTerm.RetrieveStructure();
			grdSettingsPaymentTerm();
			ResetExtraDetail();
			dtExtraDetail.Clear();
			grdExtraItems.RetrieveStructure();
			grdExtraItemsSettings();
			BtnSaveAs.Visible = false;
			((Control)(object)CmbStatus).Enabled = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ResetExtraDetail()
	{
		try
		{
			((Control)(object)CmbMasterItemInExtra).Text = string.Empty;
			((Control)(object)CmbMasterPackUomInExtra).Text = string.Empty;
			((Control)(object)CmbItemInExtra).Text = string.Empty;
			((Control)(object)CmbPackuomInExtra).Text = string.Empty;
			txtQtyInExtra.Text = string.Empty;
			txtWeightInExtra.Text = string.Empty;
			((Control)(object)CmbJobLotInExtra).Text = string.Empty;
			btnAddInExtra.Visible = true;
			BtnUpdateInExtra.Visible = false;
			BtnCancelInExtra.Visible = false;
			CmbMasterItemInExtra.Focus();
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
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnShow_Click(object sender, EventArgs e)
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
			grdhistory.DataSource = null;
			drdocdate.Checked = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void HistoryCombosFill()
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Expected O, but got Unknown
		try
		{
			DataTable dtCustomer = new DataTable();
			dtCustomer.Columns.Add("Id", typeof(int));
			dtCustomer.Columns.Add("Customer", typeof(string));
			DataTable dt = SaleOrder.GetDataForDropDownFromSaleOrder(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				AppId = UserAccount.AppId,
				UserId = UserAccount.ID
			});
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToString(dt.Rows[i]["Activity"]) == "Customer")
					{
						dtCustomer.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
				}
			}
			DDL.BindDDL(dtCustomer, CmbCustomerHistory, "Id", "Customer", "Party", ZeroIndex: false);
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

	private void grdhistory_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			RecId = Conversion.ToInt(grdhistory.CurrentRow.Cells["Id"].Value);
			ReadById(RecId);
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
			ReportsParameters order = new ReportsParameters();
			order.OrganizationId = UserAccount.OrganizationId;
			order.CompanyId = UserAccount.CompanyId;
			order.BranchesId = UserAccount.BranchesId;
			order.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			order.DocumentTypeId = 1605;
			order.ActionId = 1;
			if (drdocdate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					order.FromDate = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					order.ToDate = ToDateHistory.Value;
				}
			}
			else if (rdentrydate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					order.EntryDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					order.EntryDateTo = ToDateHistory.Value;
				}
			}
			else if (rdmodifydate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					order.ModifyDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					order.ModifyDateTo = ToDateHistory.Value;
				}
			}
			else if (rdapproveddate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					order.ApprovedDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					order.ApprovedDateTo = ToDateHistory.Value;
				}
			}
			order.FromDocNo = Conversion.ToInt((object)FromDocNoHistory.Text);
			order.ToDocNo = Conversion.ToInt((object)ToDocNoHistory.Text);
			order.SupplierCustomerId = Conversion.ToInt(CmbCustomerHistory.Value);
			order.CanViewAllRecord = FormRights.DoHaveCanViewAllRecordRights;
			DataTable dtgetall = SaleOrder.GetHisoty(order);
			if (dtgetall.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("DocNo", typeof(int));
				dt.Columns.Add("DocDate", typeof(DateTime));
				dt.Columns.Add("CustomerName");
				dt.Columns.Add("ItemNetAmount", typeof(double));
				dt.Columns.Add("TaxAmount", typeof(double));
				dt.Columns.Add("DiscountAmount", typeof(double));
				dt.Columns.Add("OrderNetAmount", typeof(double));
				dt.Columns.Add("PaymentTerms", typeof(string));
				dt.Columns.Add("DueDays", typeof(int));
				dt.Columns.Add("DueDate", typeof(DateTime));
				dt.Columns.Add("DeliveryTerm", typeof(string));
				dt.Columns.Add("DeliveryDays", typeof(int));
				dt.Columns.Add("OrderStatus", typeof(string));
				dt.Columns.Add("OrderExpiryDate", typeof(DateTime));
				dt.Columns.Add("RemarksHeader", typeof(string));
				dt.Columns.Add("EntryUser", typeof(string));
				dt.Columns.Add("EntryDate", typeof(DateTime));
				dt.Columns.Add("ModifyUser", typeof(string));
				dt.Columns.Add("ModifyDate", typeof(DateTime));
				dt.Columns.Add("ApprovedUser", typeof(string));
				dt.Columns.Add("ApprovedDate", typeof(DateTime));
				dt.Columns.Add("NoOfAttachments", typeof(int));
				for (int i = 0; i < dtgetall.Rows.Count; i++)
				{
					dt.Rows.Add(Conversion.ToInt(dtgetall.Rows[i]["Id"]), Conversion.ToInt(dtgetall.Rows[i]["DocNo"]), Conversion.ToDateTime(dtgetall.Rows[i]["DocDate"]).ToShortDateString(), Conversion.ToString(dtgetall.Rows[i]["CustomerName"]), Conversion.ToDouble(dtgetall.Rows[i]["ItemAmountHeader"]), Conversion.ToDouble(dtgetall.Rows[i]["TaxAmountHeader"]), Conversion.ToDouble(dtgetall.Rows[i]["DiscountAmountHeader"]), Conversion.ToDouble(dtgetall.Rows[i]["OrderAmount"]), Conversion.ToString(dtgetall.Rows[i]["TermsDescription"]), Conversion.ToInt(dtgetall.Rows[i]["DueDays"]), Conversion.ToDateTime(dtgetall.Rows[i]["DueDate"]).ToShortDateString(), Conversion.ToString(dtgetall.Rows[i]["DeliveryTerm"]), Conversion.ToInt(dtgetall.Rows[i]["DeliveryDays"]), Conversion.ToString(dtgetall.Rows[i]["OrderStatus"]), Conversion.ToDateTime(dtgetall.Rows[i]["OrderExpiryDate"]).ToShortDateString(), Conversion.ToString(dtgetall.Rows[i]["RemarksHeader"]), Conversion.ToString(dtgetall.Rows[i]["UserName"]), dtgetall.Rows[i]["EntryDate"], Conversion.ToString(dtgetall.Rows[i]["ModifyUserName"]), dtgetall.Rows[i]["ModifyDate"], dtgetall.Rows[i]["ApprovedUser"], dtgetall.Rows[i]["PostDate"], Conversion.ToInt(dtgetall.Rows[i]["NoOfAttachments"]));
				}
				grdhistory.DataSource = dt;
				grdhistory.RetrieveStructure();
				HistoryGridSettings();
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
			grdhistory.RootTable.Columns["EntryDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			grdhistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			grdhistory.RootTable.Columns["ApprovedDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			grdhistory.RootTable.Columns["ItemNetAmount"].AggregateFunction = (AggregateFunction)2;
			grdhistory.RootTable.Columns["ItemNetAmount"].TextAlignment = (TextAlignment)3;
			grdhistory.RootTable.Columns["ItemNetAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdhistory.RootTable.Columns["ItemNetAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdhistory.RootTable.Columns["TaxAmount"].AggregateFunction = (AggregateFunction)2;
			grdhistory.RootTable.Columns["TaxAmount"].TextAlignment = (TextAlignment)3;
			grdhistory.RootTable.Columns["TaxAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdhistory.RootTable.Columns["TaxAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdhistory.RootTable.Columns["DiscountAmount"].AggregateFunction = (AggregateFunction)2;
			grdhistory.RootTable.Columns["DiscountAmount"].TextAlignment = (TextAlignment)3;
			grdhistory.RootTable.Columns["DiscountAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdhistory.RootTable.Columns["DiscountAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdhistory.RootTable.Columns["OrderNetAmount"].AggregateFunction = (AggregateFunction)2;
			grdhistory.RootTable.Columns["OrderNetAmount"].TextAlignment = (TextAlignment)3;
			grdhistory.RootTable.Columns["OrderNetAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdhistory.RootTable.Columns["OrderNetAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustmentNew(grdhistory);
			grdhistory.RootTable.Columns["RemarksHeader"].Width = 200;
			grdhistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdhistory.RootTable.Columns.Add("Edit");
			grdhistory.RootTable.Columns["Edit"].Caption = "Edit";
			grdhistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdhistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdhistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdhistory.RootTable.Columns["Edit"].Position = 0;
			grdhistory.RootTable.Columns["Edit"].Width = 40;
			grdhistory.RootTable.Columns.Add("Print");
			grdhistory.RootTable.Columns["Print"].Caption = "Print";
			grdhistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdhistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			grdhistory.RootTable.Columns["Print"].ButtonText = "Print";
			grdhistory.RootTable.Columns["Print"].Position = 1;
			grdhistory.RootTable.Columns["Print"].Width = 40;
			grdhistory.RootTable.Columns.Add("SaveAs");
			grdhistory.RootTable.Columns["SaveAs"].Caption = "SaveAs";
			grdhistory.RootTable.Columns["SaveAs"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdhistory.RootTable.Columns["SaveAs"].ButtonStyle = (ButtonStyle)5;
			grdhistory.RootTable.Columns["SaveAs"].ButtonText = "SaveAs";
			grdhistory.RootTable.Columns["SaveAs"].Position = 2;
			grdhistory.RootTable.Columns["SaveAs"].Width = 60;
			grdhistory.FrozenColumns = 3;
			ctrlGrdBar2_Load(null, null);
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

	private void grdhistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdhistory.CurrentRow;
			if (item == null || (int)item.RowType != 0 || item.Cells.Count <= 0)
			{
				return;
			}
			if (e.Column.Key == "Edit")
			{
				RecId = Conversion.ToInt(grdhistory.CurrentRow.Cells["Id"].Value);
				ReadById(RecId);
			}
			else if (e.Column.Key == "Print")
			{
				GeneratePrint(Conversion.ToInt(grdhistory.CurrentRow.Cells["Id"].Value));
			}
			else
			{
				if (!(e.Column.Key == "SaveAs"))
				{
					return;
				}
				RecId = Conversion.ToInt(grdhistory.CurrentRow.Cells["Id"].Value);
				ReadById(RecId);
				btnsave.Visible = false;
				btnupdate.Visible = false;
				BtnSaveAs.Visible = true;
				if (grd.RowCount > 0)
				{
					GridEXRow[] rows = grd.GetRows();
					foreach (GridEXRow obj in rows)
					{
						obj.BeginEdit();
						obj.Cells["Id"].Value = 0;
						obj.EndEdit();
					}
				}
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
			GridEXRow item = grdhistory.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(item.Cells["Id"].Value), 1605);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected O, but got Unknown
		try
		{
			GridEXRow item = grdhistory.CurrentRow;
			if (item == null || (int)item.RowType != 0 || item.Cells.Count <= 0)
			{
				return;
			}
			SaleOrder So = new SaleOrder();
			So = SaleOrder.GetByID(Conversion.ToInt(item.Cells["Id"].Value));
			if (So != null)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("ItemCode", typeof(string));
				dt.Columns.Add("ItemName", typeof(string));
				dt.Columns.Add("ItemDescription", typeof(string));
				dt.Columns.Add("Specification/Remarks", typeof(string));
				dt.Columns.Add("JobLot", typeof(string));
				dt.Columns.Add("PackUom", typeof(string));
				dt.Columns.Add("ItemQty", typeof(double));
				dt.Columns.Add("Weight", typeof(double));
				dt.Columns.Add("ItemRate", typeof(double));
				dt.Columns.Add("RateUom");
				dt.Columns.Add("DiscountType", typeof(string));
				dt.Columns.Add("Discount%", typeof(double));
				dt.Columns.Add("DiscountAmount", typeof(double));
				dt.Columns.Add("Amount", typeof(double));
				dt.Columns.Add("FcyAmount", typeof(double));
				dt.Columns.Add("TaxName", typeof(string));
				dt.Columns.Add("TaxPercent", typeof(double));
				dt.Columns.Add("TaxAmount", typeof(double));
				dt.Columns.Add("CityName", typeof(string));
				dt.Clear();
				for (int i = 0; i < So.SaleOrderDetailList.Count; i++)
				{
					dt.Rows.Add(So.SaleOrderDetailList[i].ItemCodeNew, So.SaleOrderDetailList[i].ItemName, So.SaleOrderDetailList[i].ItemDiscription, So.SaleOrderDetailList[i].OrderRemarks, So.SaleOrderDetailList[i].JobLotDescription, So.SaleOrderDetailList[i].UOMCode, So.SaleOrderDetailList[i].OrderItemQty, So.SaleOrderDetailList[i].NetWeight, So.SaleOrderDetailList[i].OrderItemRate, So.SaleOrderDetailList[i].RateUom, So.SaleOrderDetailList[i].DiscountType, So.SaleOrderDetailList[i].ItemDiscount, So.SaleOrderDetailList[i].ItemDiscountAmount, So.SaleOrderDetailList[i].Amount, So.SaleOrderDetailList[i].FcyAmount, So.SaleOrderDetailList[i].TaxName, So.SaleOrderDetailList[i].TaxPercent, So.SaleOrderDetailList[i].TaxAmount, So.SaleOrderDetailList[i].CityArea);
				}
				grdDetail.DataSource = dt;
				grdDetail.RetrieveStructure();
				grdDetailSettings();
			}
			else
			{
				grdDetail.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void grdDetailSettings()
	{
		try
		{
			grdDetail.RootTable.Columns["FcyAmount"].Visible = HasMultiCurrencyFeature;
			grdDetail.RootTable.Columns["PackUom"].Caption = "Uom";
			grdDetail.RootTable.Columns["ItemQty"].Caption = "Qty";
			grdDetail.RootTable.Columns["Amount"].Caption = "ItemAmount";
			grdDetail.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["ItemQty"].FormatString = "#,##0.###";
			grdDetail.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.###";
			grdDetail.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["Weight"].FormatString = "#,##0.###";
			grdDetail.RootTable.Columns["Weight"].TotalFormatString = "#,##0.###";
			grdDetail.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["ItemRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdDetail.RootTable.Columns["TaxPercent"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["TaxPercent"].FormatString = "#,##0.###";
			grdDetail.RootTable.Columns["Discount%"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["Discount%"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdDetail.RootTable.Columns["DiscountAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["DiscountAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["DiscountAmount"].FormatString = "#,##0.###";
			grdDetail.RootTable.Columns["DiscountAmount"].TotalFormatString = "#,##0.###";
			grdDetail.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["TaxAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["TaxAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["TaxAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["TaxAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["FcyAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["FcyAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["FcyAmount"].FormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdDetail.RootTable.Columns["FcyAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingleForFcy;
			CommonServices.GridAutoAdjustmentNew(grdDetail);
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

	private void ctrlGrdBar3_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdExtraItems).Name);
			if (fs != null)
			{
				grdExtraItems.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	public void CalculateWeight()
	{
		try
		{
			if (((UltraGridBase)CmbPackUom).ActiveRow != null && Conversion.ToDouble((object)txtqty.Text) > 0.0)
			{
				double equivalent = Conversion.ToDouble(((UltraDropDownBase)CmbPackUom).SelectedRow.Cells[2].Value);
				double ItemQty = Conversion.ToDouble((object)txtqty.Text);
				txtweight.Text = (equivalent * ItemQty).ToString("#,##0.###");
			}
			else
			{
				txtweight.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void CalculateTaxAmount()
	{
		try
		{
			double ItemAmount = Conversion.ToDouble((object)txtItemAmount.Text);
			if (((UltraGridBase)CmbTaxType).ActiveRow != null && Conversion.ToInt(CmbTaxType.Value) > 0)
			{
				double TaxPercent = 0.0;
				if (!(Conversion.ToDouble((object)txtTaxPercent.Text) > 0.0))
				{
					txtTaxPercent.Text = Conversion.ToDouble(((UltraDropDownBase)CmbTaxType).SelectedRow.Cells[2].Value).ToString("#,##0.###");
					TaxPercent = Conversion.ToDouble(((UltraDropDownBase)CmbTaxType).SelectedRow.Cells[2].Value);
				}
				else
				{
					TaxPercent = Conversion.ToDouble((object)txtTaxPercent.Text);
				}
				double TaxAmt = ItemAmount * TaxPercent / 100.0;
				txtTaxAmount.Text = TaxAmt.ToString("#,##0.####");
				double TotalAmount = ItemAmount + TaxAmt;
				txtTotalAmount.Text = TotalAmount.ToString("#,##0.####");
			}
			else if (btnsave.Enabled && btnsave.Visible)
			{
				txtTaxPercent.Text = "0";
				txtTaxAmount.Text = "0";
				txtTotalAmount.Text = Conversion.ToDouble((object)txtItemAmount.Text).ToString("#,##0.####");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateAmount()
	{
		try
		{
			if (((UltraGridBase)CmbRateUom).ActiveRow != null && Conversion.ToDouble(CmbRateUom.Value) > 0.0)
			{
				double RateEquivalent = ((((UltraGridBase)CmbRateUom).ActiveRow != null) ? Conversion.ToDouble(((UltraDropDownBase)CmbRateUom).SelectedRow.Cells[2].Value) : 0.0);
				double Qty = Conversion.ToDouble((object)txtqty.Text);
				double NetRate = Conversion.ToDouble((object)txtRate.Text);
				double discountPrcnt = Conversion.ToDouble((object)txtDiscPercent.Text);
				double discountAmount = 0.0;
				if (Qty > 0.0 && NetRate > 0.0 && RateEquivalent > 0.0)
				{
					double Amount = Qty / RateEquivalent * NetRate;
					if (Conversion.ToInt(CmbDiscountType.Value) == 1)
					{
						discountAmount = ((discountPrcnt > 0.0) ? discountPrcnt : 0.0);
					}
					else if (Conversion.ToInt(CmbDiscountType.Value) == 2)
					{
						if (discountPrcnt > 100.0)
						{
							MessageBox.Show("Discount Percent Can't Greater Than 100.", "WARNING!!!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							txtDiscPercent.Text = 100.ToString();
							discountPrcnt = 100.0;
						}
						discountAmount = ((discountPrcnt > 0.0) ? (Amount * discountPrcnt / 100.0) : 0.0);
					}
					else
					{
						discountAmount = 0.0;
						txtDiscPercent.Text = "0";
						txtDiscountAmount.Text = "0";
					}
					txtDiscountAmount.Text = discountAmount.ToString("#,##0.###");
					Amount -= discountAmount;
					txtItemAmount.Text = Math.Round(Amount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero).ToString(clsGlobalVariables.stringFormatsingle);
					txtTotalAmount.Text = Math.Round(Amount, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero).ToString(clsGlobalVariables.stringFormatsingle);
				}
				else
				{
					txtItemAmount.Text = "0";
					txtTotalAmount.Text = "0";
				}
			}
			else
			{
				txtItemAmount.Text = "0";
				txtTotalAmount.Text = "0";
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
			CalculateWeight();
			CalculateAmount();
			CalculateTaxAmount();
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
			CalculateWeight();
			CalculateAmount();
			CalculateTaxAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtRate_TextChanged(object sender, EventArgs e)
	{
		CommonServices.CommasApplyWhileTyping(sender, e);
		CalculateAmount();
		CalculateTaxAmount();
	}

	private void combamounttype_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			if (combamounttype.Text == "QTY")
			{
				((Control)(object)CmbRateUom).Text = "1";
				((Control)(object)CmbRateUom).Enabled = false;
			}
			else
			{
				((Control)(object)CmbRateUom).Enabled = true;
			}
			CalculateWeight();
			CalculateAmount();
			CalculateTaxAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combsuppname_Leave(object sender, EventArgs e)
	{
		ComboAgreementNoFill();
	}

	private void combrateuom_TextChanged(object sender, EventArgs e)
	{
		CalculateAmount();
		CalculateTaxAmount();
	}

	private void combrateuom_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateAmount();
			CalculateTaxAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtDiscPercent_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateAmount();
			CalculateTaxAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbDiscountType_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateAmount();
			CalculateTaxAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void CalculateWeightExtra()
	{
		try
		{
			if (((UltraGridBase)CmbPackuomInExtra).ActiveRow != null && Conversion.ToInt(CmbPackuomInExtra.Value) > 0 && Conversion.ToDouble((object)txtQtyInExtra.Text) > 0.0)
			{
				double equivalent = Conversion.ToDouble(((UltraDropDownBase)CmbPackuomInExtra).SelectedRow.Cells[2].Value);
				double ItemQty = Conversion.ToDouble((object)txtQtyInExtra.Text);
				txtWeightInExtra.Text = (equivalent * ItemQty).ToString("#,##0.###");
			}
			else
			{
				txtWeightInExtra.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbPackuomInExtra_Leave(object sender, EventArgs e)
	{
		try
		{
			CalculateWeightExtra();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtQtyInExtra_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateWeightExtra();
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

	private void combcommtype_Leave(object sender, EventArgs e)
	{
	}

	private void combruom_Leave(object sender, EventArgs e)
	{
	}

	private void txtdocno_TextChanged(object sender, EventArgs e)
	{
	}

	private void btnDelete_Click(object sender, EventArgs e)
	{
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
			CommonServices.SaleOrderSlipEngr_1605(Id);
		}
		catch (Exception)
		{
			throw;
		}
	}

	private void txtduedays_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (txtduedays.Text != string.Empty && txtduedays.Text != null)
			{
				duedate.Value = DocDate.Value.AddDays(Conversion.ToDouble((object)txtduedays.Text));
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

	private void combpttrm_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (((Control)(object)combpttrm).Text == "Cash")
			{
				txtduedays.Enabled = false;
				duedate.Enabled = false;
				txtduedays.Text = string.Empty;
				duedate.Value = DocDate.Value;
			}
			else
			{
				txtduedays.Enabled = true;
				duedate.Enabled = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtduedays_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextNumberFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void DocDate_Leave(object sender, EventArgs e)
	{
		try
		{
			if (grd.RowCount > 0)
			{
				string ItemIds = "";
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow r in rows)
				{
					ItemIds = ItemIds + "," + r.Cells["ItemId"].Value;
				}
				DataTable dt = ChangedTaxPercentFromGridByDatechange(ItemIds);
				rows = grd.GetRows();
				foreach (GridEXRow item in rows)
				{
					item.BeginEdit();
					double TaxPercent = 0.0;
					double ItemAmount = 0.0;
					double TaxAmount = 0.0;
					double TotalAmount = 0.0;
					bool flag = false;
					for (int i = 0; i < dt.Rows.Count; i++)
					{
						if (Conversion.ToInt(dt.Rows[i]["ItemId"]) == Conversion.ToInt(item.Cells["ItemId"].Value))
						{
							flag = true;
							item.Cells["TaxNameId"].Value = Conversion.ToInt(dt.Rows[i]["TaxNameId"]);
							item.Cells["TaxName"].Value = Conversion.ToString(dt.Rows[i]["TaxName"]);
							TaxPercent = Conversion.ToDouble(dt.Rows[i]["TaxPercent"]);
							item.Cells["TaxPercent"].Value = TaxPercent;
							ItemAmount = Conversion.ToDouble(item.Cells["Amount"].Value);
							if (ItemAmount > 0.0 && TaxPercent > 0.0)
							{
								TaxAmount = ItemAmount * TaxPercent / 100.0;
								item.Cells["TaxAmount"].Value = TaxAmount;
							}
							TotalAmount = ItemAmount + TaxAmount;
							item.Cells["TotalAmount"].Value = TotalAmount;
						}
					}
					if (!flag)
					{
						item.Cells["TaxNameId"].Value = 0;
						item.Cells["TaxName"].Value = 0;
						item.Cells["TaxPercent"].Value = 0;
						item.Cells["TaxAmount"].Value = 0;
						item.Cells["TotalAmount"].Value = item.Cells["Amount"].Value;
					}
					item.EndEdit();
				}
			}
			if (deliverystartdate.Value.Date < DocDate.Value.Date)
			{
				deliverystartdate.Value = DocDate.Value;
				MessageBox.Show("Delivery Start Date Can't Be Less Than Doc Date");
			}
			else
			{
				deliverystartdate.Value = DocDate.Value;
			}
			txtduedays_TextChanged(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void deliverystartdate_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (deliverystartdate.Value.Date < DocDate.Value.Date)
			{
				deliverystartdate.Value = DocDate.Value;
				MessageBox.Show("Delivery Start Date Can't Be Less Than Doc Date");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void duedate_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (duedate.Value.Date < DocDate.Value.Date)
			{
				duedate.Value = DocDate.Value;
			}
			int days = (duedate.Value.Date - DocDate.Value.Date).Days;
			txtduedays.Text = days.ToString();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtdeliverydays_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextNumberFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtExchangeRate_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtItemAmount_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtContractDiscount_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextNumberFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtDiscPercent_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextNumberFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private DataTable ChangedTaxPercentFromGridByDatechange(string ItemIds)
	{
		DataTable dt = new DataTable();
		try
		{
			dt = CommonServices.GetTaxScheduleDetailbyItemIds(ItemIds, DocDate.Value);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		return dt;
	}

	private void CmbTaxType_TextChanged(object sender, EventArgs e)
	{
		CalculateTaxAmount();
	}

	private void txtTaxPercent_TextChanged(object sender, EventArgs e)
	{
		CalculateTaxAmount();
	}

	private void CmbTaxType_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)CmbTaxType).ActiveRow != null)
			{
				CalculateTaxAmount();
				return;
			}
			txtTaxPercent.Text = "0";
			txtTaxAmount.Text = "0";
			txtTotalAmount.Text = Conversion.ToDouble((object)txtItemAmount.Text).ToString(clsGlobalVariables.stringFormatsingle);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbWholeSale_SelectedIndexChanged(object sender, EventArgs e)
	{
		CalculateTaxAmount();
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
					((Control)(object)grdhistory).Focus();
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
					if (FormRights.DoHaveCanDelete && btnDelete.Enabled)
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
					if (btnupdate.Visible && btnupdate.Enabled)
					{
						btnupdate_Click(null, null);
					}
				}
				else if (e.Control && e.KeyCode == Keys.P)
				{
					if (btnprint.Visible && btnprint.Enabled)
					{
						btnprint_Click(null, null);
					}
				}
				else if (e.Control && e.KeyCode == Keys.N)
				{
					btnnew_Click(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.R)
				{
					toolStripButton1_Click(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.F5)
				{
					DocDate.Focus();
				}
				else if (e.Control && e.KeyCode == Keys.F10)
				{
					AT.Show();
				}
				else if (e.Control && e.KeyCode == Keys.F12)
				{
					BtnSaveAs_Click(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.Down)
				{
					if (tabControl2.SelectedIndex == 0)
					{
						((Control)(object)grd).Focus();
					}
					else if (tabControl2.SelectedIndex == 1)
					{
						((Control)(object)grdPaymentTerm).Focus();
					}
					else
					{
						((Control)(object)grd).Focus();
					}
				}
				else if (e.Control && e.KeyCode == Keys.Up)
				{
					CmbItem.Focus();
				}
				else if (e.Control && e.KeyCode == Keys.Right)
				{
					if (tabControl2.SelectedIndex == 0)
					{
						tabControl2.SelectedIndex = 1;
						((Control)(object)grdPaymentTerm).Focus();
					}
					else if (tabControl2.SelectedIndex == 1)
					{
						tabControl2.SelectedIndex = 0;
						CmbItem.Focus();
					}
				}
			}
			else
			{
				if (tabControl1.SelectedIndex != 1)
				{
					return;
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					if (((Control)(object)grd).Focused)
					{
						((Control)(object)grdDetail).Focus();
					}
					else if (((Control)(object)grdDetail).Focused)
					{
						((Control)(object)grdhistory).Focus();
					}
				}
				else if (e.Control && e.KeyCode == Keys.S)
				{
					btnShow_Click(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.N)
				{
					btnResetHistory_Click(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefreshHistory_Click(null, null);
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
			dt.Rows.Add("Ctrl+S", "For Save and When on Entry form and For Show Data when on History Form");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Ctrl+F5", "For Focus on Doc Date");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+D", "For Adding an row in Focused Grid");
			dt.Rows.Add("Ctrl+Delete", "For Deleting an row of Focused Grid");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On on Detail Entry First Column");
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

	private void grdExtraItems_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdExtraItems.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdExtraItems.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
				if (grdExtraItems.CurrentColumn != null && grdExtraItems.CurrentColumn.Key == "Delete")
				{
					if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
					{
						if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
						{
							return;
						}
						r.Delete();
						grdExtraItems.UpdateData();
					}
					else
					{
						r.Delete();
					}
					grdExtraItems.UpdateData();
				}
				if (grdExtraItems.CurrentColumn != null && grdExtraItems.CurrentColumn.Key == "Edit")
				{
					grdExtraItems_DoubleClick(null, null);
				}
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				grdExtraItems_DoubleClick(null, null);
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					r.Delete();
					grdExtraItems.UpdateData();
				}
				else
				{
					r.Delete();
				}
				grdExtraItems.UpdateData();
			}
			if (e.KeyCode == Keys.F1 && grdExtraItems.CurrentColumn != null)
			{
				_ = grdExtraItems.CurrentColumn.Key == "";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0439: Unknown result type (might be due to invalid IL or missing references)
		//IL_043f: Expected O, but got Unknown
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Expected O, but got Unknown
		//IL_06f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_039b: Unknown result type (might be due to invalid IL or missing references)
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
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No || !ValidateDetailItemExistInExtra(Conversion.ToInt(r.Cells["ItemId"].Value)))
					{
						return;
					}
					if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
					{
						SaleOrderDetail vd2 = new SaleOrderDetail();
						vd2.Id = Conversion.ToInt(r.Cells["Id"].Value);
						vd2.OrderItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
						vd2.ItemDiscription = Conversion.ToString(r.Cells["ItemDescription"].Value);
						vd2.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
						vd2.OrderItemUOMId = Conversion.ToInt(r.Cells["PackUomId"].Value);
						vd2.OrderItemQty = Conversion.ToDouble(r.Cells["ItemQty"].Value);
						vd2.NetWeight = Conversion.ToDouble(r.Cells["Weight"].Value);
						vd2.OrderItemRate = Conversion.ToDouble(r.Cells["ItemRate"].Value);
						vd2.OrderItemRateUOMId = Conversion.ToInt(r.Cells["RateUomId"].Value);
						vd2.DiscountTypeId = Conversion.ToInt(r.Cells["DiscountType"].Value);
						vd2.ItemDiscount = Conversion.ToDouble(r.Cells["Discount%"].Value);
						vd2.ItemDiscountAmount = Conversion.ToDouble(r.Cells["DiscountAmount"].Value);
						vd2.Amount = Conversion.ToDouble(r.Cells["Amount"].Value);
						vd2.CityId = Conversion.ToInt(r.Cells["CityId"].Value);
						vd2.CityArea = Conversion.ToString(r.Cells["CityName"].Value);
						vd2.OrderRemarks = Conversion.ToString(r.Cells["Remarks"].Value);
						vd2.TaxNameId = Conversion.ToInt(r.Cells["TaxNameId"].Value);
						vd2.TaxPercent = Conversion.ToDouble(r.Cells["TaxPercent"].Value);
						vd2.TaxAmount = Conversion.ToDouble(r.Cells["TaxAmount"].Value);
						vd2.TotalAmount = Conversion.ToDouble(r.Cells["TotalAmount"].Value);
						vd2.ActionTypeId = 3;
						lstRemoveRecord.Add(vd2);
						table.Rows[r.RowIndex].Delete();
						if (r != null && (int)r.RowType == 0)
						{
							r.Delete();
						}
						grd.UpdateData();
					}
					else
					{
						r.Delete();
					}
					FillMasterItemsFromDetail();
				}
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No || !ValidateDetailItemExistInExtra(Conversion.ToInt(r.Cells["ItemId"].Value)))
				{
					return;
				}
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					SaleOrderDetail vd = new SaleOrderDetail();
					vd.Id = Conversion.ToInt(r.Cells["Id"].Value);
					vd.OrderItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					vd.ItemDiscription = Conversion.ToString(r.Cells["ItemDescription"].Value);
					vd.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
					vd.OrderItemUOMId = Conversion.ToInt(r.Cells["PackUomId"].Value);
					vd.OrderItemQty = Conversion.ToDouble(r.Cells["ItemQty"].Value);
					vd.NetWeight = Conversion.ToDouble(r.Cells["Weight"].Value);
					vd.OrderItemRate = Conversion.ToDouble(r.Cells["ItemRate"].Value);
					vd.OrderItemRateUOMId = Conversion.ToInt(r.Cells["RateUomId"].Value);
					vd.DiscountTypeId = Conversion.ToInt(r.Cells["DiscountType"].Value);
					vd.ItemDiscount = Conversion.ToDouble(r.Cells["Discount%"].Value);
					vd.ItemDiscountAmount = Conversion.ToDouble(r.Cells["DiscountAmount"].Value);
					vd.Amount = Conversion.ToDouble(r.Cells["Amount"].Value);
					vd.CityId = Conversion.ToInt(r.Cells["CityId"].Value);
					vd.CityArea = Conversion.ToString(r.Cells["CityName"].Value);
					vd.OrderRemarks = Conversion.ToString(r.Cells["Remarks"].Value);
					vd.TaxNameId = Conversion.ToInt(r.Cells["TaxNameId"].Value);
					vd.TaxPercent = Conversion.ToDouble(r.Cells["TaxPercent"].Value);
					vd.TaxAmount = Conversion.ToDouble(r.Cells["TaxAmount"].Value);
					vd.TotalAmount = Conversion.ToDouble(r.Cells["TotalAmount"].Value);
					vd.ActionTypeId = 3;
					lstRemoveRecord.Add(vd);
					table.Rows[r.RowIndex].Delete();
					if (r != null && (int)r.RowType == 0)
					{
						r.Delete();
					}
					grd.UpdateData();
				}
				else
				{
					r.Delete();
				}
				FillMasterItemsFromDetail();
			}
			if (e.KeyCode == Keys.F1 && grd.CurrentColumn != null)
			{
				_ = grd.CurrentColumn.Key == "OverHeadAccountId";
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
					if (!FormRights.DoHaveUpdateRights)
					{
						throw new Exception("ypu don't have updae rights...");
					}
					RecId = Conversion.ToInt(grdhistory.CurrentRow.Cells["Id"].Value);
					ReadById(Conversion.ToInt((object)Id));
				}
				else if (grdhistory.CurrentColumn.Key == "Print")
				{
					GeneratePrint(Conversion.ToInt(grdhistory.CurrentRow.Cells["Id"].Value));
				}
				else if (grdhistory.CurrentColumn.Key == "SaveAs")
				{
					RecId = Conversion.ToInt(grdhistory.CurrentRow.Cells["Id"].Value);
					ReadById(RecId);
					btnsave.Visible = false;
					btnupdate.Visible = false;
					BtnSaveAs.Visible = true;
				}
				if (grdhistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, 1605);
				}
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				if (!FormRights.DoHaveUpdateRights)
				{
					throw new Exception("ypu don't have updae rights...");
				}
				ReadById(Conversion.ToInt((object)Id));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPaymentTerm_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdPaymentTerm.CurrentRow;
			if (item == null || (int)item.RowType != 0 || grdPaymentTerm.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdPaymentTerm.CurrentColumn.Key == "Delete")
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					dtPaymentTerm.Rows[item.RowIndex].Delete();
					if (item != null && (int)item.RowType == 0)
					{
						item.Delete();
					}
					if (dtPaymentTerm.Rows.Count == 0)
					{
						AddRowInPaymentGrid();
					}
				}
				if (grdPaymentTerm.CurrentColumn.Key == "Add")
				{
					AddRowInPaymentGrid();
				}
				grdPaymentTerm.UpdateData();
			}
			else if (e.Control && e.KeyCode == Keys.Delete)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					dtPaymentTerm.Rows[item.RowIndex].Delete();
					if (item != null && (int)item.RowType == 0)
					{
						item.Delete();
					}
					if (dtPaymentTerm.Rows.Count == 0)
					{
						AddRowInPaymentGrid();
					}
				}
			}
			else if (e.Control && e.KeyCode == Keys.D)
			{
				AddRowInPaymentGrid();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCity_Click(object sender, EventArgs e)
	{
		try
		{
			new DefineCity(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShow_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl.Name != null && base.ActiveControl.Name == "ctrlGrdBar2")
			{
				FromDateHistory.Focus();
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
		//IL_05b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ba: Expected O, but got Unknown
		//IL_05ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c1: Expected O, but got Unknown
		//IL_05c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c8: Expected O, but got Unknown
		//IL_05c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cf: Expected O, but got Unknown
		//IL_05cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d6: Expected O, but got Unknown
		//IL_05d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05dd: Expected O, but got Unknown
		//IL_05dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e4: Expected O, but got Unknown
		//IL_05e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05eb: Expected O, but got Unknown
		//IL_05eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f2: Expected O, but got Unknown
		//IL_05f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f9: Expected O, but got Unknown
		//IL_05f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0600: Expected O, but got Unknown
		//IL_0600: Unknown result type (might be due to invalid IL or missing references)
		//IL_0607: Expected O, but got Unknown
		//IL_0607: Unknown result type (might be due to invalid IL or missing references)
		//IL_060e: Expected O, but got Unknown
		//IL_060e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0615: Expected O, but got Unknown
		//IL_0615: Unknown result type (might be due to invalid IL or missing references)
		//IL_061c: Expected O, but got Unknown
		//IL_061c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0623: Expected O, but got Unknown
		//IL_0623: Unknown result type (might be due to invalid IL or missing references)
		//IL_062a: Expected O, but got Unknown
		//IL_062a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0631: Expected O, but got Unknown
		//IL_0631: Unknown result type (might be due to invalid IL or missing references)
		//IL_0638: Expected O, but got Unknown
		//IL_0638: Unknown result type (might be due to invalid IL or missing references)
		//IL_063f: Expected O, but got Unknown
		//IL_063f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0646: Expected O, but got Unknown
		//IL_0646: Unknown result type (might be due to invalid IL or missing references)
		//IL_064d: Expected O, but got Unknown
		//IL_064d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0654: Expected O, but got Unknown
		//IL_0654: Unknown result type (might be due to invalid IL or missing references)
		//IL_065b: Expected O, but got Unknown
		//IL_065b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0662: Expected O, but got Unknown
		//IL_0662: Unknown result type (might be due to invalid IL or missing references)
		//IL_0669: Expected O, but got Unknown
		//IL_0669: Unknown result type (might be due to invalid IL or missing references)
		//IL_0670: Expected O, but got Unknown
		//IL_0670: Unknown result type (might be due to invalid IL or missing references)
		//IL_0677: Expected O, but got Unknown
		//IL_0677: Unknown result type (might be due to invalid IL or missing references)
		//IL_067e: Expected O, but got Unknown
		//IL_067e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0685: Expected O, but got Unknown
		//IL_0685: Unknown result type (might be due to invalid IL or missing references)
		//IL_068c: Expected O, but got Unknown
		//IL_068c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0693: Expected O, but got Unknown
		//IL_0693: Unknown result type (might be due to invalid IL or missing references)
		//IL_069a: Expected O, but got Unknown
		//IL_069a: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a1: Expected O, but got Unknown
		//IL_06a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a8: Expected O, but got Unknown
		//IL_06a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06af: Expected O, but got Unknown
		//IL_06af: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b6: Expected O, but got Unknown
		//IL_06b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_06bd: Expected O, but got Unknown
		//IL_06bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c4: Expected O, but got Unknown
		//IL_06c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06cb: Expected O, but got Unknown
		//IL_06cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d2: Expected O, but got Unknown
		//IL_06d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d9: Expected O, but got Unknown
		//IL_06d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e0: Expected O, but got Unknown
		//IL_06e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e7: Expected O, but got Unknown
		//IL_06e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ee: Expected O, but got Unknown
		//IL_06ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f5: Expected O, but got Unknown
		//IL_06f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fe: Expected O, but got Unknown
		//IL_0700: Unknown result type (might be due to invalid IL or missing references)
		//IL_0709: Expected O, but got Unknown
		//IL_070b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0714: Expected O, but got Unknown
		//IL_0716: Unknown result type (might be due to invalid IL or missing references)
		//IL_071f: Expected O, but got Unknown
		//IL_0721: Unknown result type (might be due to invalid IL or missing references)
		//IL_072a: Expected O, but got Unknown
		//IL_072c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0735: Expected O, but got Unknown
		//IL_0737: Unknown result type (might be due to invalid IL or missing references)
		//IL_0740: Expected O, but got Unknown
		//IL_0742: Unknown result type (might be due to invalid IL or missing references)
		//IL_074b: Expected O, but got Unknown
		//IL_074d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0756: Expected O, but got Unknown
		//IL_0758: Unknown result type (might be due to invalid IL or missing references)
		//IL_0761: Expected O, but got Unknown
		//IL_0763: Unknown result type (might be due to invalid IL or missing references)
		//IL_076c: Expected O, but got Unknown
		//IL_076e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0777: Expected O, but got Unknown
		//IL_0779: Unknown result type (might be due to invalid IL or missing references)
		//IL_0782: Expected O, but got Unknown
		//IL_0784: Unknown result type (might be due to invalid IL or missing references)
		//IL_078d: Expected O, but got Unknown
		//IL_078f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0798: Expected O, but got Unknown
		//IL_079a: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a3: Expected O, but got Unknown
		//IL_07a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ae: Expected O, but got Unknown
		//IL_07b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b9: Expected O, but got Unknown
		//IL_07bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c4: Expected O, but got Unknown
		//IL_07c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_07cf: Expected O, but got Unknown
		//IL_07d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07da: Expected O, but got Unknown
		//IL_07dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e5: Expected O, but got Unknown
		//IL_07e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f0: Expected O, but got Unknown
		//IL_07f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_07fb: Expected O, but got Unknown
		//IL_07fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0806: Expected O, but got Unknown
		//IL_0808: Unknown result type (might be due to invalid IL or missing references)
		//IL_0811: Expected O, but got Unknown
		//IL_0813: Unknown result type (might be due to invalid IL or missing references)
		//IL_081c: Expected O, but got Unknown
		//IL_081e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0827: Expected O, but got Unknown
		//IL_0829: Unknown result type (might be due to invalid IL or missing references)
		//IL_0832: Expected O, but got Unknown
		//IL_0834: Unknown result type (might be due to invalid IL or missing references)
		//IL_083d: Expected O, but got Unknown
		//IL_083f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0848: Expected O, but got Unknown
		//IL_084a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0853: Expected O, but got Unknown
		//IL_0855: Unknown result type (might be due to invalid IL or missing references)
		//IL_085e: Expected O, but got Unknown
		//IL_0860: Unknown result type (might be due to invalid IL or missing references)
		//IL_0869: Expected O, but got Unknown
		//IL_086b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0874: Expected O, but got Unknown
		//IL_0876: Unknown result type (might be due to invalid IL or missing references)
		//IL_087f: Expected O, but got Unknown
		//IL_0881: Unknown result type (might be due to invalid IL or missing references)
		//IL_088a: Expected O, but got Unknown
		//IL_088c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0895: Expected O, but got Unknown
		//IL_0897: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a0: Expected O, but got Unknown
		//IL_08a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ab: Expected O, but got Unknown
		//IL_08ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b6: Expected O, but got Unknown
		//IL_08b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c1: Expected O, but got Unknown
		//IL_08c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_08cc: Expected O, but got Unknown
		//IL_08ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d7: Expected O, but got Unknown
		//IL_08d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e2: Expected O, but got Unknown
		//IL_08e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ed: Expected O, but got Unknown
		//IL_08ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f8: Expected O, but got Unknown
		//IL_08fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0903: Expected O, but got Unknown
		//IL_0905: Unknown result type (might be due to invalid IL or missing references)
		//IL_090e: Expected O, but got Unknown
		//IL_0910: Unknown result type (might be due to invalid IL or missing references)
		//IL_0919: Expected O, but got Unknown
		//IL_091b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0924: Expected O, but got Unknown
		//IL_0926: Unknown result type (might be due to invalid IL or missing references)
		//IL_092f: Expected O, but got Unknown
		//IL_0931: Unknown result type (might be due to invalid IL or missing references)
		//IL_093a: Expected O, but got Unknown
		//IL_093c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0945: Expected O, but got Unknown
		//IL_0947: Unknown result type (might be due to invalid IL or missing references)
		//IL_0950: Expected O, but got Unknown
		//IL_0952: Unknown result type (might be due to invalid IL or missing references)
		//IL_095b: Expected O, but got Unknown
		//IL_095d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0966: Expected O, but got Unknown
		//IL_0968: Unknown result type (might be due to invalid IL or missing references)
		//IL_0971: Expected O, but got Unknown
		//IL_0973: Unknown result type (might be due to invalid IL or missing references)
		//IL_097c: Expected O, but got Unknown
		//IL_097e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0987: Expected O, but got Unknown
		//IL_0989: Unknown result type (might be due to invalid IL or missing references)
		//IL_0992: Expected O, but got Unknown
		//IL_0994: Unknown result type (might be due to invalid IL or missing references)
		//IL_099d: Expected O, but got Unknown
		//IL_09f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a02: Expected O, but got Unknown
		//IL_0a92: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a9c: Expected O, but got Unknown
		//IL_0abe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac8: Expected O, but got Unknown
		//IL_0ac9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad3: Expected O, but got Unknown
		//IL_0ad4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ade: Expected O, but got Unknown
		//IL_0adf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae9: Expected O, but got Unknown
		//IL_0aea: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af4: Expected O, but got Unknown
		//IL_0b21: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b2b: Expected O, but got Unknown
		//IL_0bf2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bfc: Expected O, but got Unknown
		//IL_0c81: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c8b: Expected O, but got Unknown
		//IL_0c97: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ca1: Expected O, but got Unknown
		//IL_0d10: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d1a: Expected O, but got Unknown
		//IL_0d5d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d67: Expected O, but got Unknown
		//IL_0d7e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d88: Expected O, but got Unknown
		//IL_0dcb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dd5: Expected O, but got Unknown
		//IL_0dd6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0de0: Expected O, but got Unknown
		//IL_0df7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e01: Expected O, but got Unknown
		//IL_0ebd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ec7: Expected O, but got Unknown
		//IL_0ec8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ed2: Expected O, but got Unknown
		//IL_0f15: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f1f: Expected O, but got Unknown
		//IL_0f20: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f2a: Expected O, but got Unknown
		//IL_0f36: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f40: Expected O, but got Unknown
		//IL_0f57: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f61: Expected O, but got Unknown
		//IL_0f99: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fa3: Expected O, but got Unknown
		//IL_0fd0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fda: Expected O, but got Unknown
		//IL_0ff1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ffb: Expected O, but got Unknown
		//IL_117d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1187: Expected O, but got Unknown
		//IL_1222: Unknown result type (might be due to invalid IL or missing references)
		//IL_122c: Expected O, but got Unknown
		//IL_12a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_12b0: Expected O, but got Unknown
		//IL_12b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_12bb: Expected O, but got Unknown
		//IL_12bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_12c6: Expected O, but got Unknown
		//IL_54a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_54aa: Expected O, but got Unknown
		//IL_54b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_54c1: Expected O, but got Unknown
		//IL_8d54: Unknown result type (might be due to invalid IL or missing references)
		//IL_8d5e: Expected O, but got Unknown
		//IL_8d6b: Unknown result type (might be due to invalid IL or missing references)
		//IL_8d75: Expected O, but got Unknown
		//IL_b452: Unknown result type (might be due to invalid IL or missing references)
		//IL_b45c: Expected O, but got Unknown
		//IL_b469: Unknown result type (might be due to invalid IL or missing references)
		//IL_b473: Expected O, but got Unknown
		//IL_b480: Unknown result type (might be due to invalid IL or missing references)
		//IL_b48a: Expected O, but got Unknown
		//IL_ddca: Unknown result type (might be due to invalid IL or missing references)
		//IL_ddd4: Expected O, but got Unknown
		//IL_ddf8: Unknown result type (might be due to invalid IL or missing references)
		//IL_de02: Expected O, but got Unknown
		Appearance appearance179 = new Appearance();
		Appearance appearance272 = new Appearance();
		Appearance appearance273 = new Appearance();
		Appearance appearance190 = new Appearance();
		Appearance appearance201 = new Appearance();
		Appearance appearance298 = new Appearance();
		Appearance appearance299 = new Appearance();
		Appearance appearance300 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance281 = new Appearance();
		Appearance appearance282 = new Appearance();
		Appearance appearance283 = new Appearance();
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
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
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
		Appearance appearance89 = new Appearance();
		Appearance appearance91 = new Appearance();
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
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance244 = new Appearance();
		Appearance appearance245 = new Appearance();
		Appearance appearance246 = new Appearance();
		Appearance appearance248 = new Appearance();
		Appearance appearance249 = new Appearance();
		Appearance appearance250 = new Appearance();
		Appearance appearance251 = new Appearance();
		Appearance appearance252 = new Appearance();
		Appearance appearance253 = new Appearance();
		Appearance appearance226 = new Appearance();
		Appearance appearance227 = new Appearance();
		Appearance appearance228 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance223 = new Appearance();
		Appearance appearance247 = new Appearance();
		Appearance appearance258 = new Appearance();
		Appearance appearance269 = new Appearance();
		Appearance appearance280 = new Appearance();
		Appearance appearance291 = new Appearance();
		Appearance appearance302 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
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
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance145 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance254 = new Appearance();
		Appearance appearance255 = new Appearance();
		Appearance appearance256 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance229 = new Appearance();
		Appearance appearance230 = new Appearance();
		Appearance appearance231 = new Appearance();
		Appearance appearance232 = new Appearance();
		Appearance appearance233 = new Appearance();
		Appearance appearance234 = new Appearance();
		Appearance appearance236 = new Appearance();
		Appearance appearance237 = new Appearance();
		Appearance appearance238 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance169 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance171 = new Appearance();
		Appearance appearance172 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance180 = new Appearance();
		Appearance appearance181 = new Appearance();
		Appearance appearance182 = new Appearance();
		Appearance appearance183 = new Appearance();
		Appearance appearance184 = new Appearance();
		Appearance appearance185 = new Appearance();
		Appearance appearance186 = new Appearance();
		Appearance appearance187 = new Appearance();
		Appearance appearance188 = new Appearance();
		Appearance appearance189 = new Appearance();
		Appearance appearance191 = new Appearance();
		Appearance appearance192 = new Appearance();
		Appearance appearance193 = new Appearance();
		Appearance appearance194 = new Appearance();
		Appearance appearance195 = new Appearance();
		Appearance appearance196 = new Appearance();
		Appearance appearance197 = new Appearance();
		Appearance appearance198 = new Appearance();
		Appearance appearance199 = new Appearance();
		Appearance appearance200 = new Appearance();
		Appearance appearance202 = new Appearance();
		Appearance appearance203 = new Appearance();
		Appearance appearance204 = new Appearance();
		Appearance appearance205 = new Appearance();
		Appearance appearance206 = new Appearance();
		Appearance appearance207 = new Appearance();
		Appearance appearance208 = new Appearance();
		Appearance appearance209 = new Appearance();
		Appearance appearance210 = new Appearance();
		Appearance appearance211 = new Appearance();
		Appearance appearance213 = new Appearance();
		Appearance appearance214 = new Appearance();
		Appearance appearance215 = new Appearance();
		Appearance appearance216 = new Appearance();
		Appearance appearance217 = new Appearance();
		Appearance appearance218 = new Appearance();
		Appearance appearance219 = new Appearance();
		Appearance appearance220 = new Appearance();
		GridEXLayout grdExtraItems_DesignTimeLayout = new GridEXLayout();
		Appearance appearance284 = new Appearance();
		Appearance appearance285 = new Appearance();
		Appearance appearance286 = new Appearance();
		Appearance appearance287 = new Appearance();
		Appearance appearance288 = new Appearance();
		Appearance appearance289 = new Appearance();
		Appearance appearance290 = new Appearance();
		Appearance appearance292 = new Appearance();
		Appearance appearance293 = new Appearance();
		Appearance appearance294 = new Appearance();
		Appearance appearance295 = new Appearance();
		Appearance appearance296 = new Appearance();
		Appearance appearance168 = new Appearance();
		Appearance appearance212 = new Appearance();
		Appearance appearance224 = new Appearance();
		Appearance appearance235 = new Appearance();
		Appearance appearance239 = new Appearance();
		Appearance appearance240 = new Appearance();
		Appearance appearance241 = new Appearance();
		Appearance appearance242 = new Appearance();
		Appearance appearance243 = new Appearance();
		Appearance appearance297 = new Appearance();
		Appearance appearance301 = new Appearance();
		Appearance appearance303 = new Appearance();
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
		Appearance appearance257 = new Appearance();
		Appearance appearance259 = new Appearance();
		Appearance appearance260 = new Appearance();
		Appearance appearance261 = new Appearance();
		Appearance appearance262 = new Appearance();
		Appearance appearance263 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance221 = new Appearance();
		Appearance appearance267 = new Appearance();
		Appearance appearance222 = new Appearance();
		Appearance appearance225 = new Appearance();
		Appearance appearance304 = new Appearance();
		Appearance appearance305 = new Appearance();
		Appearance appearance306 = new Appearance();
		Appearance appearance307 = new Appearance();
		Appearance appearance308 = new Appearance();
		Appearance appearance309 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		GridEXLayout grdPaymentTerm_DesignTimeLayout = new GridEXLayout();
		Appearance appearance310 = new Appearance();
		Appearance appearance311 = new Appearance();
		Appearance appearance312 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance157 = new Appearance();
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
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		GridEXLayout grdhistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance264 = new Appearance();
		Appearance appearance265 = new Appearance();
		Appearance appearance266 = new Appearance();
		Appearance appearance268 = new Appearance();
		Appearance appearance270 = new Appearance();
		Appearance appearance271 = new Appearance();
		Appearance appearance274 = new Appearance();
		Appearance appearance275 = new Appearance();
		Appearance appearance276 = new Appearance();
		Appearance appearance277 = new Appearance();
		Appearance appearance278 = new Appearance();
		Appearance appearance279 = new Appearance();
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
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
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.label29 = new System.Windows.Forms.Label();
		this.txtExchangeRate = new System.Windows.Forms.TextBox();
		this.txtFcyAmount = new System.Windows.Forms.TextBox();
		this.label32 = new System.Windows.Forms.Label();
		this.cmbCurrency = new UltraCombo();
		this.label35 = new System.Windows.Forms.Label();
		this.txtOrderQty = new System.Windows.Forms.TextBox();
		this.label46 = new System.Windows.Forms.Label();
		this.txtOrderWeight = new System.Windows.Forms.TextBox();
		this.label50 = new System.Windows.Forms.Label();
		this.txtTaxAmountHeader = new System.Windows.Forms.TextBox();
		this.label75 = new System.Windows.Forms.Label();
		this.txtDiscountHeader = new System.Windows.Forms.TextBox();
		this.label71 = new System.Windows.Forms.Label();
		this.txtItemNetAmountHeader = new System.Windows.Forms.TextBox();
		this.label70 = new System.Windows.Forms.Label();
		this.txtOrderAmountHeader = new System.Windows.Forms.TextBox();
		this.label51 = new System.Windows.Forms.Label();
		this.CmbBillType = new UltraCombo();
		this.label22 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.CmbStatus = new UltraCombo();
		this.combdeliverytrm = new UltraCombo();
		this.combpttrm = new UltraCombo();
		this.cmbbranch1 = new UltraCombo();
		this.combproject1 = new UltraCombo();
		this.label26 = new System.Windows.Forms.Label();
		this.label31 = new System.Windows.Forms.Label();
		this.txtduedays = new System.Windows.Forms.TextBox();
		this.label33 = new System.Windows.Forms.Label();
		this.combsuppname = new UltraCombo();
		this.label24 = new System.Windows.Forms.Label();
		this.deliverystartdate = new System.Windows.Forms.DateTimePicker();
		this.duedate = new System.Windows.Forms.DateTimePicker();
		this.label30 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.label28 = new System.Windows.Forms.Label();
		this.txtdeliverydays = new System.Windows.Forms.TextBox();
		this.label13 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.txtsupprefno = new System.Windows.Forms.TextBox();
		this.DocDate = new System.Windows.Forms.DateTimePicker();
		this.label8 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.txtremarks = new System.Windows.Forms.TextBox();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.panel5 = new System.Windows.Forms.Panel();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.tabPageDetail = new System.Windows.Forms.TabPage();
		this.grd = new GridEX();
		this.panel14 = new System.Windows.Forms.Panel();
		this.label16 = new System.Windows.Forms.Label();
		this.panel6 = new System.Windows.Forms.Panel();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.txtDiscountAmount = new System.Windows.Forms.TextBox();
		this.label72 = new System.Windows.Forms.Label();
		this.rdbtnItemCode = new System.Windows.Forms.RadioButton();
		this.rdbtnItemName = new System.Windows.Forms.RadioButton();
		this.txtDiscPercent = new System.Windows.Forms.TextBox();
		this.txtItemDescriptions = new System.Windows.Forms.TextBox();
		this.label73 = new System.Windows.Forms.Label();
		this.btnCancelUpdateDetial = new System.Windows.Forms.Button();
		this.CmbDiscountType = new UltraCombo();
		this.label74 = new System.Windows.Forms.Label();
		this.CmbCity = new UltraCombo();
		this.btnplus = new System.Windows.Forms.Button();
		this.label5 = new System.Windows.Forms.Label();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.txtTotalAmount = new System.Windows.Forms.TextBox();
		this.label49 = new System.Windows.Forms.Label();
		this.txtItemAmount = new System.Windows.Forms.TextBox();
		this.label44 = new System.Windows.Forms.Label();
		this.txtTaxAmount = new System.Windows.Forms.TextBox();
		this.label48 = new System.Windows.Forms.Label();
		this.txtTaxPercent = new System.Windows.Forms.TextBox();
		this.CmbRateUom = new UltraCombo();
		this.txtweight = new System.Windows.Forms.TextBox();
		this.label43 = new System.Windows.Forms.Label();
		this.label41 = new System.Windows.Forms.Label();
		this.label45 = new System.Windows.Forms.Label();
		this.label39 = new System.Windows.Forms.Label();
		this.txtremarksdetail = new System.Windows.Forms.TextBox();
		this.CmbTaxType = new UltraCombo();
		this.label47 = new System.Windows.Forms.Label();
		this.label40 = new System.Windows.Forms.Label();
		this.CmbPackUom = new UltraCombo();
		this.txtRate = new System.Windows.Forms.TextBox();
		this.label52 = new System.Windows.Forms.Label();
		this.label36 = new System.Windows.Forms.Label();
		this.label38 = new System.Windows.Forms.Label();
		this.label34 = new System.Windows.Forms.Label();
		this.txtqty = new System.Windows.Forms.TextBox();
		this.CmbJobLot = new UltraCombo();
		this.CmbItem = new UltraCombo();
		this.label37 = new System.Windows.Forms.Label();
		this.tabExtraItems = new System.Windows.Forms.TabPage();
		this.grdExtraItems = new GridEX();
		this.panel10 = new System.Windows.Forms.Panel();
		this.label53 = new System.Windows.Forms.Label();
		this.panel12 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
		this.RadMasterItemName = new System.Windows.Forms.RadioButton();
		this.RadMasterItemCode = new System.Windows.Forms.RadioButton();
		this.RadExtraItemCode = new System.Windows.Forms.RadioButton();
		this.RadExtraItemName = new System.Windows.Forms.RadioButton();
		this.txtItemDescriptionInExtra = new System.Windows.Forms.TextBox();
		this.BtnCancelInExtra = new System.Windows.Forms.Button();
		this.btnAddInExtra = new System.Windows.Forms.Button();
		this.BtnUpdateInExtra = new System.Windows.Forms.Button();
		this.txtWeightInExtra = new System.Windows.Forms.TextBox();
		this.label62 = new System.Windows.Forms.Label();
		this.txtRemarksInExtra = new System.Windows.Forms.TextBox();
		this.label63 = new System.Windows.Forms.Label();
		this.CmbPackuomInExtra = new UltraCombo();
		this.CmbMasterPackUomInExtra = new UltraCombo();
		this.label55 = new System.Windows.Forms.Label();
		this.label65 = new System.Windows.Forms.Label();
		this.label66 = new System.Windows.Forms.Label();
		this.label67 = new System.Windows.Forms.Label();
		this.label68 = new System.Windows.Forms.Label();
		this.txtQtyInExtra = new System.Windows.Forms.TextBox();
		this.CmbJobLotInExtra = new UltraCombo();
		this.CmbMasterItemInExtra = new UltraCombo();
		this.label54 = new System.Windows.Forms.Label();
		this.CmbItemInExtra = new UltraCombo();
		this.label69 = new System.Windows.Forms.Label();
		this.tabPagePaymentDetail = new System.Windows.Forms.TabPage();
		this.grdPaymentTerm = new GridEX();
		this.panel11 = new System.Windows.Forms.Panel();
		this.label57 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.txtBagPrice = new System.Windows.Forms.TextBox();
		this.combruom = new System.Windows.Forms.ComboBox();
		this.combordercat = new UltraCombo();
		this.label14 = new System.Windows.Forms.Label();
		this.combamounttype = new System.Windows.Forms.ComboBox();
		this.label25 = new System.Windows.Forms.Label();
		this.label42 = new System.Windows.Forms.Label();
		this.combcomp1 = new UltraCombo();
		this.combcommtype = new System.Windows.Forms.ComboBox();
		this.label21 = new System.Windows.Forms.Label();
		this.CmbAgreementNo = new UltraCombo();
		this.label9 = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.txtBagWeightCut = new System.Windows.Forms.TextBox();
		this.label19 = new System.Windows.Forms.Label();
		this.txtcommremarks = new System.Windows.Forms.TextBox();
		this.txtcatsr = new System.Windows.Forms.TextBox();
		this.txtcommamount = new System.Windows.Forms.TextBox();
		this.validate = new System.Windows.Forms.DateTimePicker();
		this.label11 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.ChkBoxPrint = new System.Windows.Forms.CheckBox();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.BtnSaveAs = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.btnattachment = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
		this.btnprint = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
		this.btnshortcutkeys = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.btnCity = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.grdhistory = new GridEX();
		this.panel9 = new System.Windows.Forms.Panel();
		this.label64 = new System.Windows.Forms.Label();
		this.panel7 = new System.Windows.Forms.Panel();
		this.groupBox4 = new System.Windows.Forms.GroupBox();
		this.rdapproveddate = new System.Windows.Forms.RadioButton();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.btnShow = new System.Windows.Forms.Button();
		this.label56 = new System.Windows.Forms.Label();
		this.label58 = new System.Windows.Forms.Label();
		this.label59 = new System.Windows.Forms.Label();
		this.label60 = new System.Windows.Forms.Label();
		this.label61 = new System.Windows.Forms.Label();
		this.CmbCustomerHistory = new UltraCombo();
		this.ToDocNoHistory = new System.Windows.Forms.TextBox();
		this.FromDocNoHistory = new System.Windows.Forms.TextBox();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.panel8 = new System.Windows.Forms.Panel();
		this.btnLoad = new System.Windows.Forms.ToolStrip();
		this.btnResetHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.label15 = new System.Windows.Forms.Label();
		this.panel13 = new System.Windows.Forms.Panel();
		this.label10 = new System.Windows.Forms.Label();
		this.grdDetail = new GridEX();
		this.combsalesman = new UltraCombo();
		this.CmbCropyr = new UltraCombo();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbCurrency).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBillType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbStatus).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combdeliverytrm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combpttrm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbbranch1).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combproject1).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combsuppname).BeginInit();
		this.panel5.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.tabPageDetail.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel14.SuspendLayout();
		this.panel6.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDiscountType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCity).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRateUom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTaxType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItem).BeginInit();
		this.tabExtraItems.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdExtraItems).BeginInit();
		this.panel10.SuspendLayout();
		this.panel12.SuspendLayout();
		this.groupBox2.SuspendLayout();
		this.flowLayoutPanel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPackuomInExtra).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbMasterPackUomInExtra).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLotInExtra).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbMasterItemInExtra).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemInExtra).BeginInit();
		this.tabPagePaymentDetail.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPaymentTerm).BeginInit();
		this.panel11.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.combordercat).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combcomp1).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAgreementNo).BeginInit();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdhistory).BeginInit();
		this.panel9.SuspendLayout();
		this.panel7.SuspendLayout();
		this.groupBox4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).BeginInit();
		this.panel8.SuspendLayout();
		this.btnLoad.SuspendLayout();
		this.panel13.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combsalesman).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropyr).BeginInit();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1236, 611);
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
		this.tabControl1.Size = new System.Drawing.Size(1236, 611);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.groupBox1);
		this.tabPage1.Controls.Add(this.panel5);
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage1.Size = new System.Drawing.Size(1228, 582);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.groupBox1.BackColor = System.Drawing.Color.White;
		this.groupBox1.Controls.Add(this.label29);
		this.groupBox1.Controls.Add(this.txtExchangeRate);
		this.groupBox1.Controls.Add(this.txtFcyAmount);
		this.groupBox1.Controls.Add(this.label32);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCurrency);
		this.groupBox1.Controls.Add(this.label35);
		this.groupBox1.Controls.Add(this.txtOrderQty);
		this.groupBox1.Controls.Add(this.label46);
		this.groupBox1.Controls.Add(this.txtOrderWeight);
		this.groupBox1.Controls.Add(this.label50);
		this.groupBox1.Controls.Add(this.txtTaxAmountHeader);
		this.groupBox1.Controls.Add(this.label75);
		this.groupBox1.Controls.Add(this.txtDiscountHeader);
		this.groupBox1.Controls.Add(this.label71);
		this.groupBox1.Controls.Add(this.txtItemNetAmountHeader);
		this.groupBox1.Controls.Add(this.label70);
		this.groupBox1.Controls.Add(this.txtOrderAmountHeader);
		this.groupBox1.Controls.Add(this.label51);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBillType);
		this.groupBox1.Controls.Add(this.label22);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbStatus);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.combdeliverytrm);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.combpttrm);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbbranch1);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.combproject1);
		this.groupBox1.Controls.Add(this.label26);
		this.groupBox1.Controls.Add(this.label31);
		this.groupBox1.Controls.Add(this.txtduedays);
		this.groupBox1.Controls.Add(this.label33);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.combsuppname);
		this.groupBox1.Controls.Add(this.label24);
		this.groupBox1.Controls.Add(this.deliverystartdate);
		this.groupBox1.Controls.Add(this.duedate);
		this.groupBox1.Controls.Add(this.label30);
		this.groupBox1.Controls.Add(this.label12);
		this.groupBox1.Controls.Add(this.label28);
		this.groupBox1.Controls.Add(this.txtdeliverydays);
		this.groupBox1.Controls.Add(this.label13);
		this.groupBox1.Controls.Add(this.label18);
		this.groupBox1.Controls.Add(this.txtsupprefno);
		this.groupBox1.Controls.Add(this.DocDate);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add(this.label17);
		this.groupBox1.Controls.Add(this.txtremarks);
		this.groupBox1.Controls.Add(this.txtdocno);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(7, 54);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1163, 178);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Main";
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label29.Location = new System.Drawing.Point(713, 156);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(72, 15);
		this.label29.TabIndex = 55;
		this.label29.Text = "Fcy Amount";
		this.txtExchangeRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtExchangeRate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtExchangeRate.Location = new System.Drawing.Point(799, 126);
		this.txtExchangeRate.Name = "txtExchangeRate";
		this.txtExchangeRate.Size = new System.Drawing.Size(125, 23);
		this.txtExchangeRate.TabIndex = 18;
		this.txtExchangeRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtExchangeRate.TextChanged += new System.EventHandler(txtExchangeRate_TextChanged);
		this.txtExchangeRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtExchangeRate_KeyPress);
		this.txtExchangeRate.Leave += new System.EventHandler(txtExchangeRate_Leave);
		this.txtFcyAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFcyAmount.Enabled = false;
		this.txtFcyAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtFcyAmount.Location = new System.Drawing.Point(799, 152);
		this.txtFcyAmount.Name = "txtFcyAmount";
		this.txtFcyAmount.Size = new System.Drawing.Size(125, 23);
		this.txtFcyAmount.TabIndex = 19;
		this.txtFcyAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label32.Location = new System.Drawing.Point(713, 130);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(84, 15);
		this.label32.TabIndex = 54;
		this.label32.Text = "Exchange Rate";
		this.cmbCurrency.AllowNull = (DefaultableBoolean)1;
		this.cmbCurrency.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCurrency.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCurrency.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance179).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Appearance = (AppearanceBase)(object)appearance179;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance272).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance272).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance272).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance272).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance272;
		((AppearanceBase)appearance273).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance273;
		((SpecialBoxBase)((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance190).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance190).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance190).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance190).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance190;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance201).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance201).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance201;
		((AppearanceBase)appearance298).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance298).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance298;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance299).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance299;
		((AppearanceBase)appearance300).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance300).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance300;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance106).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance106).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance106).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance106;
		((AppearanceBase)appearance281).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance281;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance282).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance282).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance282;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance283).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance283;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbCurrency.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Location = new System.Drawing.Point(799, 98);
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Name = "cmbCurrency";
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Size = new System.Drawing.Size(125, 26);
		((System.Windows.Forms.Control)(object)this.cmbCurrency).TabIndex = 17;
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Leave += new System.EventHandler(cmbCurrency_Leave);
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(713, 104);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(55, 15);
		this.label35.TabIndex = 48;
		this.label35.Text = "Fcy Code";
		this.txtOrderQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOrderQty.Enabled = false;
		this.txtOrderQty.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtOrderQty.Location = new System.Drawing.Point(799, 45);
		this.txtOrderQty.Name = "txtOrderQty";
		this.txtOrderQty.Size = new System.Drawing.Size(125, 23);
		this.txtOrderQty.TabIndex = 15;
		this.txtOrderQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label46.AutoSize = true;
		this.label46.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label46.Location = new System.Drawing.Point(713, 49);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(59, 15);
		this.label46.TabIndex = 51;
		this.label46.Text = "Order Qty";
		this.txtOrderWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOrderWeight.Enabled = false;
		this.txtOrderWeight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtOrderWeight.Location = new System.Drawing.Point(799, 72);
		this.txtOrderWeight.Name = "txtOrderWeight";
		this.txtOrderWeight.Size = new System.Drawing.Size(125, 23);
		this.txtOrderWeight.TabIndex = 16;
		this.txtOrderWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label50.Location = new System.Drawing.Point(713, 76);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(79, 15);
		this.label50.TabIndex = 50;
		this.label50.Text = "Order Weight";
		this.txtTaxAmountHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTaxAmountHeader.Enabled = false;
		this.txtTaxAmountHeader.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtTaxAmountHeader.Location = new System.Drawing.Point(1031, 45);
		this.txtTaxAmountHeader.Name = "txtTaxAmountHeader";
		this.txtTaxAmountHeader.Size = new System.Drawing.Size(127, 23);
		this.txtTaxAmountHeader.TabIndex = 21;
		this.txtTaxAmountHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtTaxAmountHeader.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtContractDiscount_KeyPress);
		this.label75.AutoSize = true;
		this.label75.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label75.Location = new System.Drawing.Point(925, 49);
		this.label75.Name = "label75";
		this.label75.Size = new System.Drawing.Size(72, 15);
		this.label75.TabIndex = 49;
		this.label75.Text = "Tax Amount";
		this.txtDiscountHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDiscountHeader.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtDiscountHeader.Location = new System.Drawing.Point(1031, 72);
		this.txtDiscountHeader.Name = "txtDiscountHeader";
		this.txtDiscountHeader.Size = new System.Drawing.Size(127, 23);
		this.txtDiscountHeader.TabIndex = 22;
		this.txtDiscountHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtDiscountHeader.TextChanged += new System.EventHandler(txtDiscountHeader_TextChanged);
		this.txtDiscountHeader.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtContractDiscount_KeyPress);
		this.label71.AutoSize = true;
		this.label71.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label71.Location = new System.Drawing.Point(925, 76);
		this.label71.Name = "label71";
		this.label71.Size = new System.Drawing.Size(88, 15);
		this.label71.TabIndex = 49;
		this.label71.Text = "Order Discount";
		this.txtItemNetAmountHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemNetAmountHeader.Enabled = false;
		this.txtItemNetAmountHeader.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtItemNetAmountHeader.Location = new System.Drawing.Point(1031, 17);
		this.txtItemNetAmountHeader.Name = "txtItemNetAmountHeader";
		this.txtItemNetAmountHeader.Size = new System.Drawing.Size(127, 23);
		this.txtItemNetAmountHeader.TabIndex = 20;
		this.txtItemNetAmountHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label70.AutoSize = true;
		this.label70.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label70.Location = new System.Drawing.Point(925, 21);
		this.label70.Name = "label70";
		this.label70.Size = new System.Drawing.Size(101, 15);
		this.label70.TabIndex = 49;
		this.label70.Text = "Item Net Amount";
		this.txtOrderAmountHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOrderAmountHeader.Enabled = false;
		this.txtOrderAmountHeader.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtOrderAmountHeader.Location = new System.Drawing.Point(1031, 100);
		this.txtOrderAmountHeader.Name = "txtOrderAmountHeader";
		this.txtOrderAmountHeader.Size = new System.Drawing.Size(127, 23);
		this.txtOrderAmountHeader.TabIndex = 23;
		this.txtOrderAmountHeader.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label51.Location = new System.Drawing.Point(925, 104);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(106, 15);
		this.label51.TabIndex = 49;
		this.label51.Text = "Order Net Amount";
		this.CmbBillType.AllowNull = (DefaultableBoolean)1;
		this.CmbBillType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBillType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBillType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBillType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.CmbBillType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBillType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance93).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance93).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBillType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBillType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance94;
		((SpecialBoxBase)((UltraGridBase)this.CmbBillType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance95).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance95).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBillType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.CmbBillType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBillType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance96).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBillType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance97).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBillType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.CmbBillType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBillType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBillType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance98;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance99).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBillType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.CmbBillType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBillType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance100).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBillType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance102).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBillType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.CmbBillType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBillType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBillType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.CmbBillType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBillType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.CmbBillType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBillType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBillType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBillType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBillType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBillType).Location = new System.Drawing.Point(799, 15);
		((System.Windows.Forms.Control)(object)this.CmbBillType).Name = "CmbBillType";
		((System.Windows.Forms.Control)(object)this.CmbBillType).Size = new System.Drawing.Size(125, 26);
		((System.Windows.Forms.Control)(object)this.CmbBillType).TabIndex = 14;
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(713, 21);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(51, 15);
		this.label22.TabIndex = 17;
		this.label22.Text = "Bill Type";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(505, 76);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(74, 15);
		this.label6.TabIndex = 16;
		this.label6.Text = "D. Start Date";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(302, 104);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(57, 15);
		this.label3.TabIndex = 15;
		this.label3.Text = "Due Date";
		this.CmbStatus.AllowNull = (DefaultableBoolean)1;
		this.CmbStatus.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbStatus.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbStatus.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Appearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.CmbStatus).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbStatus).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance120).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance120).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance120).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbStatus).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance121).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbStatus).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance121;
		((SpecialBoxBase)((UltraGridBase)this.CmbStatus).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance122).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance122).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance122).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbStatus).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.CmbStatus).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbStatus).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance123).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance123;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance126;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance127).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance128).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance128).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance128).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance128;
		((AppearanceBase)appearance129).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.CmbStatus).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbStatus).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbStatus).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbStatus).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbStatus.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbStatus).Location = new System.Drawing.Point(588, 98);
		((System.Windows.Forms.Control)(object)this.CmbStatus).Name = "CmbStatus";
		((System.Windows.Forms.Control)(object)this.CmbStatus).Size = new System.Drawing.Size(123, 26);
		((System.Windows.Forms.Control)(object)this.CmbStatus).TabIndex = 13;
		this.combdeliverytrm.AllowNull = (DefaultableBoolean)1;
		this.combdeliverytrm.AutoCompleteMode = (AutoCompleteMode)4;
		this.combdeliverytrm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combdeliverytrm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance80).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance80).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combdeliverytrm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance81;
		((SpecialBoxBase)((UltraGridBase)this.combdeliverytrm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance82).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance82).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance86).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance87).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance87).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance87).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance87;
		((AppearanceBase)appearance88).TextHAlignAsString = "Left";
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combdeliverytrm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).Location = new System.Drawing.Point(588, 15);
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).Name = "combdeliverytrm";
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).Size = new System.Drawing.Size(123, 26);
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).TabIndex = 10;
		this.combpttrm.AllowNull = (DefaultableBoolean)1;
		this.combpttrm.AutoCompleteMode = (AutoCompleteMode)4;
		this.combpttrm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combpttrm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combpttrm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.combpttrm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combpttrm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combpttrm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combpttrm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance67;
		((SpecialBoxBase)((UltraGridBase)this.combpttrm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combpttrm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.combpttrm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combpttrm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance73).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance74).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).TextHAlignAsString = "Left";
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.combpttrm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combpttrm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combpttrm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combpttrm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combpttrm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combpttrm).Location = new System.Drawing.Point(386, 43);
		((System.Windows.Forms.Control)(object)this.combpttrm).Name = "combpttrm";
		((System.Windows.Forms.Control)(object)this.combpttrm).Size = new System.Drawing.Size(119, 26);
		((System.Windows.Forms.Control)(object)this.combpttrm).TabIndex = 7;
		((System.Windows.Forms.Control)(object)this.combpttrm).TextChanged += new System.EventHandler(combpttrm_TextChanged);
		this.cmbbranch1.AllowNull = (DefaultableBoolean)1;
		this.cmbbranch1.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbbranch1.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbbranch1.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance244).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance244).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.Appearance = (AppearanceBase)(object)appearance244;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance245).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance245).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance245).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance245).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbbranch1).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance245;
		((AppearanceBase)appearance246).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance246;
		((SpecialBoxBase)((UltraGridBase)this.cmbbranch1).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance248).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance248).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance248).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance248).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance248;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance249).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance249).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance249;
		((AppearanceBase)appearance250).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance250).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance250;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance251).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance251;
		((AppearanceBase)appearance252).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance252).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance252;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance253).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance253).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance253).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance253).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance253).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance253;
		((AppearanceBase)appearance226).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbbranch1).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance226;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance227).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance227).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance227;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance228).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance228;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbbranch1).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbbranch1.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbbranch1).Location = new System.Drawing.Point(97, 15);
		((System.Windows.Forms.Control)(object)this.cmbbranch1).Name = "cmbbranch1";
		((System.Windows.Forms.Control)(object)this.cmbbranch1).Size = new System.Drawing.Size(202, 26);
		((System.Windows.Forms.Control)(object)this.cmbbranch1).TabIndex = 0;
		this.combproject1.AllowNull = (DefaultableBoolean)1;
		this.combproject1.AutoCompleteMode = (AutoCompleteMode)4;
		this.combproject1.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combproject1.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combproject1).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.combproject1).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combproject1).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combproject1).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance223).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combproject1).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance223;
		((SpecialBoxBase)((UltraGridBase)this.combproject1).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance247).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance247).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance247).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance247).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combproject1).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance247;
		((UltraGridBase)this.combproject1).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combproject1).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance258).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance258).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combproject1).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance258;
		((AppearanceBase)appearance269).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance269).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combproject1).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance269;
		((UltraGridBase)this.combproject1).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combproject1).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance280).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combproject1).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance280;
		((AppearanceBase)appearance291).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance291).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combproject1).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance291;
		((UltraGridBase)this.combproject1).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combproject1).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance302).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance302).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance302).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance302).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance302).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combproject1).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance302;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.combproject1).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.combproject1).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combproject1).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combproject1).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.combproject1).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combproject1).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.combproject1).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combproject1).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combproject1).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combproject1).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combproject1.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combproject1).Location = new System.Drawing.Point(97, 43);
		((System.Windows.Forms.Control)(object)this.combproject1).Name = "combproject1";
		((System.Windows.Forms.Control)(object)this.combproject1).Size = new System.Drawing.Size(202, 26);
		((System.Windows.Forms.Control)(object)this.combproject1).TabIndex = 1;
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(302, 49);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(84, 15);
		this.label26.TabIndex = 8;
		this.label26.Text = "Payment Term";
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.Location = new System.Drawing.Point(505, 104);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(73, 15);
		this.label31.TabIndex = 8;
		this.label31.Text = "Order Status";
		this.txtduedays.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtduedays.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtduedays.Location = new System.Drawing.Point(386, 72);
		this.txtduedays.Name = "txtduedays";
		this.txtduedays.Size = new System.Drawing.Size(119, 23);
		this.txtduedays.TabIndex = 8;
		this.txtduedays.TextChanged += new System.EventHandler(txtduedays_TextChanged);
		this.txtduedays.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtduedays_KeyPress);
		this.label33.AutoSize = true;
		this.label33.BackColor = System.Drawing.Color.White;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label33.Location = new System.Drawing.Point(5, 131);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(52, 15);
		this.label33.TabIndex = 8;
		this.label33.Text = "Remarks";
		this.label33.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.combsuppname.AllowNull = (DefaultableBoolean)1;
		this.combsuppname.AutoCompleteMode = (AutoCompleteMode)4;
		this.combsuppname.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combsuppname.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combsuppname).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.combsuppname).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combsuppname).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combsuppname).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combsuppname).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.combsuppname).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combsuppname).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.combsuppname).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combsuppname).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combsuppname).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.combsuppname).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combsuppname).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combsuppname).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combsuppname).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combsuppname.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combsuppname).Location = new System.Drawing.Point(97, 98);
		((System.Windows.Forms.Control)(object)this.combsuppname).Name = "combsuppname";
		((System.Windows.Forms.Control)(object)this.combsuppname).Size = new System.Drawing.Size(202, 26);
		((System.Windows.Forms.Control)(object)this.combsuppname).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.combsuppname).Leave += new System.EventHandler(combsuppname_Leave);
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(302, 76);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(58, 15);
		this.label24.TabIndex = 0;
		this.label24.Text = "Due Days";
		this.deliverystartdate.CustomFormat = "dd-MMM-yy";
		this.deliverystartdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.deliverystartdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.deliverystartdate.Location = new System.Drawing.Point(588, 72);
		this.deliverystartdate.Name = "deliverystartdate";
		this.deliverystartdate.Size = new System.Drawing.Size(123, 23);
		this.deliverystartdate.TabIndex = 12;
		this.deliverystartdate.ValueChanged += new System.EventHandler(deliverystartdate_ValueChanged);
		this.duedate.CustomFormat = "dd-MMM-yy";
		this.duedate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.duedate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.duedate.Location = new System.Drawing.Point(386, 100);
		this.duedate.Name = "duedate";
		this.duedate.Size = new System.Drawing.Size(119, 23);
		this.duedate.TabIndex = 9;
		this.duedate.ValueChanged += new System.EventHandler(duedate_ValueChanged);
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(505, 21);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(80, 15);
		this.label30.TabIndex = 8;
		this.label30.Text = "Delivery Term";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(5, 104);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(93, 15);
		this.label12.TabIndex = 0;
		this.label12.Text = "Customer Name";
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.Location = new System.Drawing.Point(505, 49);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(82, 15);
		this.label28.TabIndex = 0;
		this.label28.Text = "Delivery Days ";
		this.txtdeliverydays.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdeliverydays.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdeliverydays.Location = new System.Drawing.Point(588, 45);
		this.txtdeliverydays.Name = "txtdeliverydays";
		this.txtdeliverydays.Size = new System.Drawing.Size(123, 23);
		this.txtdeliverydays.TabIndex = 11;
		this.txtdeliverydays.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtdeliverydays_KeyPress);
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(5, 21);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(79, 15);
		this.label13.TabIndex = 0;
		this.label13.Text = "Branch Name";
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(5, 49);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(79, 15);
		this.label18.TabIndex = 0;
		this.label18.Text = "Project Name";
		this.txtsupprefno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtsupprefno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtsupprefno.Location = new System.Drawing.Point(386, 17);
		this.txtsupprefno.Name = "txtsupprefno";
		this.txtsupprefno.Size = new System.Drawing.Size(119, 23);
		this.txtsupprefno.TabIndex = 6;
		this.DocDate.CustomFormat = "dd-MMM-yy";
		this.DocDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DocDate.Location = new System.Drawing.Point(171, 72);
		this.DocDate.Name = "DocDate";
		this.DocDate.Size = new System.Drawing.Size(127, 23);
		this.DocDate.TabIndex = 3;
		this.DocDate.TabStop = false;
		this.DocDate.Leave += new System.EventHandler(DocDate_Leave);
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(302, 21);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(78, 15);
		this.label8.TabIndex = 3;
		this.label8.Text = "Reference No";
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(5, 76);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(88, 15);
		this.label17.TabIndex = 3;
		this.label17.Text = "Doc No && Date";
		this.txtremarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtremarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtremarks.Location = new System.Drawing.Point(97, 127);
		this.txtremarks.Multiline = true;
		this.txtremarks.Name = "txtremarks";
		this.txtremarks.Size = new System.Drawing.Size(614, 48);
		this.txtremarks.TabIndex = 5;
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Enabled = false;
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocno.Location = new System.Drawing.Point(97, 72);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.Size = new System.Drawing.Size(72, 23);
		this.txtdocno.TabIndex = 2;
		this.txtdocno.Leave += new System.EventHandler(txtdocno_TextChanged);
		this.panel5.Controls.Add(this.tabControl2);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(2, 234);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1224, 346);
		this.panel5.TabIndex = 3;
		this.tabControl2.Controls.Add(this.tabPageDetail);
		this.tabControl2.Controls.Add(this.tabExtraItems);
		this.tabControl2.Controls.Add(this.tabPagePaymentDetail);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl2.Location = new System.Drawing.Point(0, 0);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(1224, 346);
		this.tabControl2.TabIndex = 0;
		this.tabPageDetail.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.tabPageDetail.Controls.Add(this.panel14);
		this.tabPageDetail.Controls.Add(this.panel6);
		this.tabPageDetail.Location = new System.Drawing.Point(4, 25);
		this.tabPageDetail.Name = "tabPageDetail";
		this.tabPageDetail.Padding = new System.Windows.Forms.Padding(3);
		this.tabPageDetail.Size = new System.Drawing.Size(1216, 317);
		this.tabPageDetail.TabIndex = 0;
		this.tabPageDetail.Text = "Detail";
		this.tabPageDetail.UseVisualStyleBackColor = true;
		this.grd.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(3, 186);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1210, 128);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.grd).TabStop = false;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.CellUpdated += new ColumnActionEventHandler(grd_CellUpdated);
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grd_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		this.panel14.BackColor = System.Drawing.Color.Teal;
		this.panel14.Controls.Add(this.label16);
		this.panel14.Controls.Add(this.ctrlGrdBar1);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel14.Location = new System.Drawing.Point(3, 161);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(1210, 25);
		this.panel14.TabIndex = 0;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Tahoma", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.ForeColor = System.Drawing.Color.White;
		this.label16.Location = new System.Drawing.Point(3, 3);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(53, 18);
		this.label16.TabIndex = 26;
		this.label16.Text = "Detail";
		this.panel6.Controls.Add(this.groupBox3);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(3, 3);
		this.panel6.Margin = new System.Windows.Forms.Padding(2);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1210, 158);
		this.panel6.TabIndex = 4;
		this.groupBox3.BackColor = System.Drawing.Color.White;
		this.groupBox3.Controls.Add(this.txtDiscountAmount);
		this.groupBox3.Controls.Add(this.label72);
		this.groupBox3.Controls.Add(this.rdbtnItemCode);
		this.groupBox3.Controls.Add(this.rdbtnItemName);
		this.groupBox3.Controls.Add(this.txtDiscPercent);
		this.groupBox3.Controls.Add(this.txtItemDescriptions);
		this.groupBox3.Controls.Add(this.label73);
		this.groupBox3.Controls.Add(this.btnCancelUpdateDetial);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDiscountType);
		this.groupBox3.Controls.Add(this.label74);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCity);
		this.groupBox3.Controls.Add(this.btnplus);
		this.groupBox3.Controls.Add(this.label5);
		this.groupBox3.Controls.Add(this.btnUpdateDetail);
		this.groupBox3.Controls.Add(this.txtTotalAmount);
		this.groupBox3.Controls.Add(this.label49);
		this.groupBox3.Controls.Add(this.txtItemAmount);
		this.groupBox3.Controls.Add(this.label44);
		this.groupBox3.Controls.Add(this.txtTaxAmount);
		this.groupBox3.Controls.Add(this.label48);
		this.groupBox3.Controls.Add(this.txtTaxPercent);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbRateUom);
		this.groupBox3.Controls.Add(this.txtweight);
		this.groupBox3.Controls.Add(this.label43);
		this.groupBox3.Controls.Add(this.label41);
		this.groupBox3.Controls.Add(this.label45);
		this.groupBox3.Controls.Add(this.label39);
		this.groupBox3.Controls.Add(this.txtremarksdetail);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbTaxType);
		this.groupBox3.Controls.Add(this.label47);
		this.groupBox3.Controls.Add(this.label40);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPackUom);
		this.groupBox3.Controls.Add(this.txtRate);
		this.groupBox3.Controls.Add(this.label52);
		this.groupBox3.Controls.Add(this.label36);
		this.groupBox3.Controls.Add(this.label38);
		this.groupBox3.Controls.Add(this.label34);
		this.groupBox3.Controls.Add(this.txtqty);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobLot);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItem);
		this.groupBox3.Controls.Add(this.label37);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(2, 0);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(1159, 156);
		this.groupBox3.TabIndex = 0;
		this.groupBox3.TabStop = false;
		this.txtDiscountAmount.BackColor = System.Drawing.Color.White;
		this.txtDiscountAmount.Enabled = false;
		this.txtDiscountAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDiscountAmount.Location = new System.Drawing.Point(878, 79);
		this.txtDiscountAmount.Name = "txtDiscountAmount";
		this.txtDiscountAmount.Size = new System.Drawing.Size(113, 23);
		this.txtDiscountAmount.TabIndex = 11;
		this.txtDiscountAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label72.AutoSize = true;
		this.label72.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label72.Location = new System.Drawing.Point(877, 59);
		this.label72.Name = "label72";
		this.label72.Size = new System.Drawing.Size(77, 15);
		this.label72.TabIndex = 148;
		this.label72.Text = "Disc Amount";
		this.rdbtnItemCode.AutoSize = true;
		this.rdbtnItemCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdbtnItemCode.Location = new System.Drawing.Point(102, 11);
		this.rdbtnItemCode.Margin = new System.Windows.Forms.Padding(2);
		this.rdbtnItemCode.Name = "rdbtnItemCode";
		this.rdbtnItemCode.Size = new System.Drawing.Size(52, 19);
		this.rdbtnItemCode.TabIndex = 129;
		this.rdbtnItemCode.Text = "Code";
		this.rdbtnItemCode.UseVisualStyleBackColor = true;
		this.rdbtnItemName.AutoSize = true;
		this.rdbtnItemName.Checked = true;
		this.rdbtnItemName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdbtnItemName.Location = new System.Drawing.Point(41, 11);
		this.rdbtnItemName.Margin = new System.Windows.Forms.Padding(2);
		this.rdbtnItemName.Name = "rdbtnItemName";
		this.rdbtnItemName.Size = new System.Drawing.Size(57, 19);
		this.rdbtnItemName.TabIndex = 128;
		this.rdbtnItemName.TabStop = true;
		this.rdbtnItemName.Text = "Name";
		this.rdbtnItemName.UseVisualStyleBackColor = true;
		this.rdbtnItemName.CheckedChanged += new System.EventHandler(rdbtnItemName_CheckedChanged);
		this.txtDiscPercent.BackColor = System.Drawing.Color.White;
		this.txtDiscPercent.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDiscPercent.Location = new System.Drawing.Point(796, 79);
		this.txtDiscPercent.Name = "txtDiscPercent";
		this.txtDiscPercent.Size = new System.Drawing.Size(80, 23);
		this.txtDiscPercent.TabIndex = 10;
		this.txtDiscPercent.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtDiscPercent.TextChanged += new System.EventHandler(txtDiscPercent_TextChanged);
		this.txtDiscPercent.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtDiscPercent_KeyPress);
		this.txtItemDescriptions.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemDescriptions.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemDescriptions.Location = new System.Drawing.Point(215, 33);
		this.txtItemDescriptions.Name = "txtItemDescriptions";
		this.txtItemDescriptions.Size = new System.Drawing.Size(362, 23);
		this.txtItemDescriptions.TabIndex = 1;
		this.label73.AutoSize = true;
		this.label73.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label73.Location = new System.Drawing.Point(798, 59);
		this.label73.Name = "label73";
		this.label73.Size = new System.Drawing.Size(43, 15);
		this.label73.TabIndex = 147;
		this.label73.Text = "Disc %";
		this.btnCancelUpdateDetial.BackColor = System.Drawing.Color.Teal;
		this.btnCancelUpdateDetial.FlatAppearance.BorderSize = 0;
		this.btnCancelUpdateDetial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelUpdateDetial.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelUpdateDetial.ForeColor = System.Drawing.Color.White;
		this.btnCancelUpdateDetial.Location = new System.Drawing.Point(900, 127);
		this.btnCancelUpdateDetial.Name = "btnCancelUpdateDetial";
		this.btnCancelUpdateDetial.Size = new System.Drawing.Size(59, 23);
		this.btnCancelUpdateDetial.TabIndex = 20;
		this.btnCancelUpdateDetial.Text = "Cancel";
		this.btnCancelUpdateDetial.UseVisualStyleBackColor = false;
		this.btnCancelUpdateDetial.Visible = false;
		this.btnCancelUpdateDetial.Click += new System.EventHandler(btnCancelUpdateDetial_Click);
		this.CmbDiscountType.AllowNull = (DefaultableBoolean)1;
		this.CmbDiscountType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDiscountType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDiscountType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance145).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance147).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance147).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance147).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDiscountType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance147;
		((AppearanceBase)appearance148).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance148;
		((SpecialBoxBase)((UltraGridBase)this.CmbDiscountType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance149).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance149).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance149).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance149;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance150).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance150;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance151).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance152;
		((AppearanceBase)appearance153).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance153).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance153;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance154).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance154).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance154).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance154).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance154;
		((AppearanceBase)appearance254).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance254;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance255).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance255).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance255;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance256).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance256;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDiscountType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDiscountType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDiscountType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDiscountType).Location = new System.Drawing.Point(679, 77);
		((System.Windows.Forms.Control)(object)this.CmbDiscountType).Name = "CmbDiscountType";
		((System.Windows.Forms.Control)(object)this.CmbDiscountType).Size = new System.Drawing.Size(114, 26);
		((System.Windows.Forms.Control)(object)this.CmbDiscountType).TabIndex = 9;
		((System.Windows.Forms.Control)(object)this.CmbDiscountType).TextChanged += new System.EventHandler(CmbDiscountType_TextChanged);
		this.label74.AutoSize = true;
		this.label74.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label74.Location = new System.Drawing.Point(680, 59);
		this.label74.Name = "label74";
		this.label74.Size = new System.Drawing.Size(83, 15);
		this.label74.TabIndex = 146;
		this.label74.Text = "Discount Type";
		this.CmbCity.AllowNull = (DefaultableBoolean)1;
		this.CmbCity.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCity.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCity.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance116).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCity).DisplayLayout.Appearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.CmbCity).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCity).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance117).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance117).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCity).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance117;
		((AppearanceBase)appearance118).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCity).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance118;
		((SpecialBoxBase)((UltraGridBase)this.CmbCity).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance229).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance229).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance229).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance229).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCity).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance229;
		((UltraGridBase)this.CmbCity).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCity).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance230).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance230).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance230;
		((AppearanceBase)appearance231).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance231).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance231;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance232).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance232;
		((AppearanceBase)appearance233).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance233).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance233;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance234).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance234).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance234).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance234).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance234).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance234;
		((AppearanceBase)appearance236).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance236;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance237).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance237).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance237;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance238).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance238;
		((UltraGridBase)this.CmbCity).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCity).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCity).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCity).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCity.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCity).Location = new System.Drawing.Point(488, 125);
		((System.Windows.Forms.Control)(object)this.CmbCity).Name = "CmbCity";
		((System.Windows.Forms.Control)(object)this.CmbCity).Size = new System.Drawing.Size(305, 26);
		((System.Windows.Forms.Control)(object)this.CmbCity).TabIndex = 17;
		this.btnplus.BackColor = System.Drawing.Color.Teal;
		this.btnplus.FlatAppearance.BorderSize = 0;
		this.btnplus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnplus.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnplus.ForeColor = System.Drawing.Color.White;
		this.btnplus.Location = new System.Drawing.Point(798, 127);
		this.btnplus.Name = "btnplus";
		this.btnplus.Size = new System.Drawing.Size(33, 23);
		this.btnplus.TabIndex = 18;
		this.btnplus.Text = "+";
		this.btnplus.UseVisualStyleBackColor = false;
		this.btnplus.Click += new System.EventHandler(btnplus_Click);
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(488, 107);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(62, 15);
		this.label5.TabIndex = 39;
		this.label5.Text = "City Name";
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatAppearance.BorderSize = 0;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(833, 127);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(65, 23);
		this.btnUpdateDetail.TabIndex = 19;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Visible = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.txtTotalAmount.BackColor = System.Drawing.Color.White;
		this.txtTotalAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTotalAmount.Enabled = false;
		this.txtTotalAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTotalAmount.Location = new System.Drawing.Point(352, 127);
		this.txtTotalAmount.Name = "txtTotalAmount";
		this.txtTotalAmount.ReadOnly = true;
		this.txtTotalAmount.Size = new System.Drawing.Size(134, 23);
		this.txtTotalAmount.TabIndex = 16;
		this.txtTotalAmount.TabStop = false;
		this.txtTotalAmount.Text = "0";
		this.txtTotalAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label49.AutoSize = true;
		this.label49.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label49.Location = new System.Drawing.Point(351, 107);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(80, 15);
		this.label49.TabIndex = 35;
		this.label49.Text = "Total Amount";
		this.txtItemAmount.BackColor = System.Drawing.Color.White;
		this.txtItemAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemAmount.Enabled = false;
		this.txtItemAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemAmount.Location = new System.Drawing.Point(993, 79);
		this.txtItemAmount.Name = "txtItemAmount";
		this.txtItemAmount.ReadOnly = true;
		this.txtItemAmount.Size = new System.Drawing.Size(135, 23);
		this.txtItemAmount.TabIndex = 12;
		this.txtItemAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtItemAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtItemAmount_KeyPress);
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label44.Location = new System.Drawing.Point(994, 59);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(79, 15);
		this.label44.TabIndex = 23;
		this.label44.Text = "Item Amount";
		this.txtTaxAmount.BackColor = System.Drawing.Color.White;
		this.txtTaxAmount.Enabled = false;
		this.txtTaxAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTaxAmount.Location = new System.Drawing.Point(226, 127);
		this.txtTaxAmount.Name = "txtTaxAmount";
		this.txtTaxAmount.ReadOnly = true;
		this.txtTaxAmount.Size = new System.Drawing.Size(123, 23);
		this.txtTaxAmount.TabIndex = 15;
		this.txtTaxAmount.TabStop = false;
		this.txtTaxAmount.Text = "0";
		this.txtTaxAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.Location = new System.Drawing.Point(226, 107);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(72, 15);
		this.label48.TabIndex = 33;
		this.label48.Text = "Tax Amount";
		this.txtTaxPercent.BackColor = System.Drawing.Color.White;
		this.txtTaxPercent.Enabled = false;
		this.txtTaxPercent.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTaxPercent.Location = new System.Drawing.Point(144, 127);
		this.txtTaxPercent.Name = "txtTaxPercent";
		this.txtTaxPercent.Size = new System.Drawing.Size(81, 23);
		this.txtTaxPercent.TabIndex = 14;
		this.txtTaxPercent.TabStop = false;
		this.txtTaxPercent.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtTaxPercent.TextChanged += new System.EventHandler(txtTaxPercent_TextChanged);
		this.CmbRateUom.AllowNull = (DefaultableBoolean)1;
		this.CmbRateUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbRateUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbRateUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance155).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance156).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance156).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance156).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbRateUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance156;
		((AppearanceBase)appearance158).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance158;
		((SpecialBoxBase)((UltraGridBase)this.CmbRateUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance159).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance159).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance159).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance160).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance160;
		((AppearanceBase)appearance161).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance161).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance161;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance162;
		((AppearanceBase)appearance163).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance163).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance163;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance164).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance164).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance164).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance164).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance164;
		((AppearanceBase)appearance165).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance165;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance166).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance166).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance166;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance167;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbRateUom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Location = new System.Drawing.Point(579, 77);
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Name = "CmbRateUom";
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Size = new System.Drawing.Size(97, 26);
		((System.Windows.Forms.Control)(object)this.CmbRateUom).TabIndex = 8;
		((System.Windows.Forms.Control)(object)this.CmbRateUom).TextChanged += new System.EventHandler(combrateuom_TextChanged);
		this.txtweight.Enabled = false;
		this.txtweight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtweight.Location = new System.Drawing.Point(400, 79);
		this.txtweight.Name = "txtweight";
		this.txtweight.ShortcutsEnabled = false;
		this.txtweight.Size = new System.Drawing.Size(86, 23);
		this.txtweight.TabIndex = 6;
		this.txtweight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label43.AutoSize = true;
		this.label43.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label43.Location = new System.Drawing.Point(144, 107);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(38, 15);
		this.label43.TabIndex = 31;
		this.label43.Text = "Tax %";
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label41.Location = new System.Drawing.Point(579, 59);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(59, 15);
		this.label41.TabIndex = 19;
		this.label41.Text = "Rate Uom";
		this.label45.AutoSize = true;
		this.label45.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label45.Location = new System.Drawing.Point(7, 107);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(53, 15);
		this.label45.TabIndex = 23;
		this.label45.Text = "Tax Type";
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label39.Location = new System.Drawing.Point(401, 59);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(46, 15);
		this.label39.TabIndex = 15;
		this.label39.Text = "Weight";
		this.txtremarksdetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtremarksdetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtremarksdetail.Location = new System.Drawing.Point(579, 33);
		this.txtremarksdetail.Name = "txtremarksdetail";
		this.txtremarksdetail.Size = new System.Drawing.Size(549, 23);
		this.txtremarksdetail.TabIndex = 2;
		this.CmbTaxType.AllowNull = (DefaultableBoolean)1;
		this.CmbTaxType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbTaxType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbTaxType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance169).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance169).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance169;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance170).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance170).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance170).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbTaxType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance170;
		((AppearanceBase)appearance171).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance171;
		((SpecialBoxBase)((UltraGridBase)this.CmbTaxType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance172).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance172).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance172).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance172;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance173).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance173).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance173;
		((AppearanceBase)appearance174).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance174).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance174;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance175;
		((AppearanceBase)appearance176).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance176).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance176;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance177).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance177).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance177).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance177).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance177;
		((AppearanceBase)appearance178).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance178;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance180).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance180).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance180;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance181).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance181;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbTaxType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbTaxType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbTaxType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbTaxType).Location = new System.Drawing.Point(6, 125);
		((System.Windows.Forms.Control)(object)this.CmbTaxType).Name = "CmbTaxType";
		((System.Windows.Forms.Control)(object)this.CmbTaxType).Size = new System.Drawing.Size(134, 26);
		((System.Windows.Forms.Control)(object)this.CmbTaxType).TabIndex = 13;
		((System.Windows.Forms.Control)(object)this.CmbTaxType).TextChanged += new System.EventHandler(CmbTaxType_TextChanged);
		((System.Windows.Forms.Control)(object)this.CmbTaxType).Leave += new System.EventHandler(CmbTaxType_Leave);
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label47.Location = new System.Drawing.Point(578, 13);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(132, 15);
		this.label47.TabIndex = 25;
		this.label47.Text = "Specification / Remarks";
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.Location = new System.Drawing.Point(486, 59);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(58, 15);
		this.label40.TabIndex = 17;
		this.label40.Text = "Item Rate";
		this.CmbPackUom.AllowNull = (DefaultableBoolean)1;
		this.CmbPackUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance182).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance182).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance182;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance183).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance183).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance183).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance183).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance183;
		((AppearanceBase)appearance184).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance184;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance185).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance185).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance185).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance185).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance185;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance186).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance186).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance186;
		((AppearanceBase)appearance187).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance187).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance187;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance188).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance188;
		((AppearanceBase)appearance189).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance189).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance189;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance191).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance191).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance191).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance191).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance191).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance191;
		((AppearanceBase)appearance192).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance192;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance193).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance193).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance193;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance194).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance194;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPackUom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Location = new System.Drawing.Point(215, 77);
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Name = "CmbPackUom";
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Size = new System.Drawing.Size(100, 26);
		((System.Windows.Forms.Control)(object)this.CmbPackUom).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Leave += new System.EventHandler(combitempck_Leave);
		this.txtRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRate.Location = new System.Drawing.Point(488, 79);
		this.txtRate.Name = "txtRate";
		this.txtRate.ShortcutsEnabled = false;
		this.txtRate.Size = new System.Drawing.Size(89, 23);
		this.txtRate.TabIndex = 7;
		this.txtRate.Text = "0";
		this.txtRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtRate.TextChanged += new System.EventHandler(txtRate_TextChanged);
		this.txtRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtRate_KeyPress);
		this.label52.AutoSize = true;
		this.label52.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label52.Location = new System.Drawing.Point(219, 13);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(96, 15);
		this.label52.TabIndex = 27;
		this.label52.Text = "Item Description";
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(215, 59);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(33, 15);
		this.label36.TabIndex = 8;
		this.label36.Text = "Uom";
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.Location = new System.Drawing.Point(319, 59);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(26, 15);
		this.label38.TabIndex = 13;
		this.label38.Text = "Qty";
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.Location = new System.Drawing.Point(4, 59);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(46, 15);
		this.label34.TabIndex = 23;
		this.label34.Text = "Job Lot";
		this.txtqty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtqty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtqty.Location = new System.Drawing.Point(318, 79);
		this.txtqty.Name = "txtqty";
		this.txtqty.ShortcutsEnabled = false;
		this.txtqty.Size = new System.Drawing.Size(81, 23);
		this.txtqty.TabIndex = 5;
		this.txtqty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtqty.TextChanged += new System.EventHandler(txtqty_TextChanged);
		this.txtqty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtqty_KeyPress);
		this.CmbJobLot.AllowNull = (DefaultableBoolean)1;
		this.CmbJobLot.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobLot.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobLot.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance195).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance195).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Appearance = (AppearanceBase)(object)appearance195;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance196).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance196).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance196).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance196).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance196;
		((AppearanceBase)appearance197).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance197;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance198).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance198).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance198).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance198).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance198;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance199).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance199).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance199;
		((AppearanceBase)appearance200).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance200).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance200;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance202).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance202;
		((AppearanceBase)appearance203).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance203).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance203;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance204).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance204).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance204).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance204).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance204).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance204;
		((AppearanceBase)appearance205).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance205;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance206).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance206).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance206;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance207).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance207;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbJobLot.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Location = new System.Drawing.Point(4, 77);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Name = "CmbJobLot";
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Size = new System.Drawing.Size(209, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).TabIndex = 3;
		this.CmbItem.AllowNull = (DefaultableBoolean)1;
		this.CmbItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance208).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance208).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance208;
		((UltraGridBase)this.CmbItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance209).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance209).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance209).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance209).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance209;
		((AppearanceBase)appearance210).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance210;
		((SpecialBoxBase)((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance211).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance211).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance211).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance211).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance211;
		((UltraGridBase)this.CmbItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance213).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance213).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance213;
		((AppearanceBase)appearance214).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance214).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance214;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance215).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance215;
		((AppearanceBase)appearance216).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance216).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance216;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance217).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance217).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance217).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance217).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance217).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance217;
		((AppearanceBase)appearance218).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance218;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance219).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance219).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance219;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance220).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance220;
		((UltraGridBase)this.CmbItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItem).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItem).Location = new System.Drawing.Point(4, 31);
		((System.Windows.Forms.Control)(object)this.CmbItem).Name = "CmbItem";
		((System.Windows.Forms.Control)(object)this.CmbItem).Size = new System.Drawing.Size(209, 26);
		((System.Windows.Forms.Control)(object)this.CmbItem).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbItem).Leave += new System.EventHandler(combitem_Leave_1);
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(4, 13);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(32, 15);
		this.label37.TabIndex = 9;
		this.label37.Text = "Item";
		this.tabExtraItems.Controls.Add((System.Windows.Forms.Control)(object)this.grdExtraItems);
		this.tabExtraItems.Controls.Add(this.panel10);
		this.tabExtraItems.Controls.Add(this.panel12);
		this.tabExtraItems.Location = new System.Drawing.Point(4, 25);
		this.tabExtraItems.Name = "tabExtraItems";
		this.tabExtraItems.Size = new System.Drawing.Size(1216, 317);
		this.tabExtraItems.TabIndex = 2;
		this.tabExtraItems.Text = "Extra Items Detail";
		this.tabExtraItems.UseVisualStyleBackColor = true;
		this.grdExtraItems.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdExtraItems).BackColor = System.Drawing.Color.White;
		grdExtraItems_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdExtraItems.DesignTimeLayout = grdExtraItems_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdExtraItems).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdExtraItems.FilterMode = (FilterMode)2;
		this.grdExtraItems.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdExtraItems).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdExtraItems.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdExtraItems).Location = new System.Drawing.Point(0, 141);
		((System.Windows.Forms.Control)(object)this.grdExtraItems).Name = "grdExtraItems";
		((ScrollableBase)this.grdExtraItems).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdExtraItems).Size = new System.Drawing.Size(1216, 176);
		((System.Windows.Forms.Control)(object)this.grdExtraItems).TabIndex = 5;
		((System.Windows.Forms.Control)(object)this.grdExtraItems).TabStop = false;
		this.grdExtraItems.TotalRow = (InheritableBoolean)1;
		this.grdExtraItems.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdExtraItems).VisualStyle = (VisualStyle)4;
		this.grdExtraItems.CellUpdated += new ColumnActionEventHandler(grdExtraItems_CellUpdated);
		this.grdExtraItems.ColumnButtonClick += new ColumnActionEventHandler(grdExtraItems_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdExtraItems).DoubleClick += new System.EventHandler(grdExtraItems_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdExtraItems).KeyDown += new System.Windows.Forms.KeyEventHandler(grdExtraItems_KeyDown);
		this.panel10.BackColor = System.Drawing.Color.Teal;
		this.panel10.Controls.Add(this.label53);
		this.panel10.Controls.Add(this.ctrlGrdBar3);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(0, 116);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1216, 25);
		this.panel10.TabIndex = 6;
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Tahoma", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label53.ForeColor = System.Drawing.Color.White;
		this.label53.Location = new System.Drawing.Point(3, 3);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(53, 18);
		this.label53.TabIndex = 26;
		this.label53.Text = "Detail";
		this.panel12.Controls.Add(this.groupBox2);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(0, 0);
		this.panel12.Margin = new System.Windows.Forms.Padding(2);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1216, 116);
		this.panel12.TabIndex = 7;
		this.groupBox2.BackColor = System.Drawing.Color.White;
		this.groupBox2.Controls.Add(this.flowLayoutPanel1);
		this.groupBox2.Controls.Add(this.RadExtraItemCode);
		this.groupBox2.Controls.Add(this.RadExtraItemName);
		this.groupBox2.Controls.Add(this.txtItemDescriptionInExtra);
		this.groupBox2.Controls.Add(this.BtnCancelInExtra);
		this.groupBox2.Controls.Add(this.btnAddInExtra);
		this.groupBox2.Controls.Add(this.BtnUpdateInExtra);
		this.groupBox2.Controls.Add(this.txtWeightInExtra);
		this.groupBox2.Controls.Add(this.label62);
		this.groupBox2.Controls.Add(this.txtRemarksInExtra);
		this.groupBox2.Controls.Add(this.label63);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPackuomInExtra);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbMasterPackUomInExtra);
		this.groupBox2.Controls.Add(this.label55);
		this.groupBox2.Controls.Add(this.label65);
		this.groupBox2.Controls.Add(this.label66);
		this.groupBox2.Controls.Add(this.label67);
		this.groupBox2.Controls.Add(this.label68);
		this.groupBox2.Controls.Add(this.txtQtyInExtra);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobLotInExtra);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbMasterItemInExtra);
		this.groupBox2.Controls.Add(this.label54);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemInExtra);
		this.groupBox2.Controls.Add(this.label69);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(2, 2);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(1135, 111);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.flowLayoutPanel1.Controls.Add(this.RadMasterItemName);
		this.flowLayoutPanel1.Controls.Add(this.RadMasterItemCode);
		this.flowLayoutPanel1.Location = new System.Drawing.Point(82, 10);
		this.flowLayoutPanel1.Name = "flowLayoutPanel1";
		this.flowLayoutPanel1.Size = new System.Drawing.Size(125, 23);
		this.flowLayoutPanel1.TabIndex = 0;
		this.RadMasterItemName.AutoSize = true;
		this.RadMasterItemName.Checked = true;
		this.RadMasterItemName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadMasterItemName.Location = new System.Drawing.Point(2, 2);
		this.RadMasterItemName.Margin = new System.Windows.Forms.Padding(2);
		this.RadMasterItemName.Name = "RadMasterItemName";
		this.RadMasterItemName.Size = new System.Drawing.Size(57, 19);
		this.RadMasterItemName.TabIndex = 130;
		this.RadMasterItemName.TabStop = true;
		this.RadMasterItemName.Text = "Name";
		this.RadMasterItemName.UseVisualStyleBackColor = true;
		this.RadMasterItemName.CheckedChanged += new System.EventHandler(RadMasterItemName_CheckedChanged);
		this.RadMasterItemCode.AutoSize = true;
		this.RadMasterItemCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadMasterItemCode.Location = new System.Drawing.Point(63, 2);
		this.RadMasterItemCode.Margin = new System.Windows.Forms.Padding(2);
		this.RadMasterItemCode.Name = "RadMasterItemCode";
		this.RadMasterItemCode.Size = new System.Drawing.Size(52, 19);
		this.RadMasterItemCode.TabIndex = 131;
		this.RadMasterItemCode.Text = "Code";
		this.RadMasterItemCode.UseVisualStyleBackColor = true;
		this.RadExtraItemCode.AutoSize = true;
		this.RadExtraItemCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadExtraItemCode.Location = new System.Drawing.Point(490, 12);
		this.RadExtraItemCode.Margin = new System.Windows.Forms.Padding(2);
		this.RadExtraItemCode.Name = "RadExtraItemCode";
		this.RadExtraItemCode.Size = new System.Drawing.Size(52, 19);
		this.RadExtraItemCode.TabIndex = 131;
		this.RadExtraItemCode.Text = "Code";
		this.RadExtraItemCode.UseVisualStyleBackColor = true;
		this.RadExtraItemName.AutoSize = true;
		this.RadExtraItemName.Checked = true;
		this.RadExtraItemName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadExtraItemName.Location = new System.Drawing.Point(429, 12);
		this.RadExtraItemName.Margin = new System.Windows.Forms.Padding(2);
		this.RadExtraItemName.Name = "RadExtraItemName";
		this.RadExtraItemName.Size = new System.Drawing.Size(57, 19);
		this.RadExtraItemName.TabIndex = 130;
		this.RadExtraItemName.TabStop = true;
		this.RadExtraItemName.Text = "Name";
		this.RadExtraItemName.UseVisualStyleBackColor = true;
		this.RadExtraItemName.CheckedChanged += new System.EventHandler(RadExtraItemName_CheckedChanged);
		this.txtItemDescriptionInExtra.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemDescriptionInExtra.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemDescriptionInExtra.Location = new System.Drawing.Point(596, 37);
		this.txtItemDescriptionInExtra.Name = "txtItemDescriptionInExtra";
		this.txtItemDescriptionInExtra.Size = new System.Drawing.Size(222, 23);
		this.txtItemDescriptionInExtra.TabIndex = 1;
		this.BtnCancelInExtra.BackColor = System.Drawing.Color.Teal;
		this.BtnCancelInExtra.FlatAppearance.BorderSize = 0;
		this.BtnCancelInExtra.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnCancelInExtra.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnCancelInExtra.ForeColor = System.Drawing.Color.White;
		this.BtnCancelInExtra.Location = new System.Drawing.Point(671, 82);
		this.BtnCancelInExtra.Name = "BtnCancelInExtra";
		this.BtnCancelInExtra.Size = new System.Drawing.Size(61, 23);
		this.BtnCancelInExtra.TabIndex = 16;
		this.BtnCancelInExtra.Text = "Cancel";
		this.BtnCancelInExtra.UseVisualStyleBackColor = false;
		this.BtnCancelInExtra.Visible = false;
		this.BtnCancelInExtra.Click += new System.EventHandler(BtnCancelInExtra_Click);
		this.btnAddInExtra.BackColor = System.Drawing.Color.Teal;
		this.btnAddInExtra.FlatAppearance.BorderSize = 0;
		this.btnAddInExtra.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAddInExtra.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAddInExtra.ForeColor = System.Drawing.Color.White;
		this.btnAddInExtra.Location = new System.Drawing.Point(597, 82);
		this.btnAddInExtra.Name = "btnAddInExtra";
		this.btnAddInExtra.Size = new System.Drawing.Size(35, 23);
		this.btnAddInExtra.TabIndex = 15;
		this.btnAddInExtra.Text = "+";
		this.btnAddInExtra.UseVisualStyleBackColor = false;
		this.btnAddInExtra.Click += new System.EventHandler(btnAddInExtra_Click);
		this.BtnUpdateInExtra.BackColor = System.Drawing.Color.Teal;
		this.BtnUpdateInExtra.FlatAppearance.BorderSize = 0;
		this.BtnUpdateInExtra.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnUpdateInExtra.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnUpdateInExtra.ForeColor = System.Drawing.Color.White;
		this.BtnUpdateInExtra.Location = new System.Drawing.Point(597, 82);
		this.BtnUpdateInExtra.Name = "BtnUpdateInExtra";
		this.BtnUpdateInExtra.Size = new System.Drawing.Size(71, 23);
		this.BtnUpdateInExtra.TabIndex = 15;
		this.BtnUpdateInExtra.Text = "Update";
		this.BtnUpdateInExtra.UseVisualStyleBackColor = false;
		this.BtnUpdateInExtra.Visible = false;
		this.BtnUpdateInExtra.Click += new System.EventHandler(BtnUpdateInExtra_Click);
		this.txtWeightInExtra.Enabled = false;
		this.txtWeightInExtra.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtWeightInExtra.Location = new System.Drawing.Point(477, 82);
		this.txtWeightInExtra.Name = "txtWeightInExtra";
		this.txtWeightInExtra.ShortcutsEnabled = false;
		this.txtWeightInExtra.Size = new System.Drawing.Size(117, 23);
		this.txtWeightInExtra.TabIndex = 6;
		this.txtWeightInExtra.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label62.AutoSize = true;
		this.label62.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label62.Location = new System.Drawing.Point(477, 64);
		this.label62.Name = "label62";
		this.label62.Size = new System.Drawing.Size(46, 15);
		this.label62.TabIndex = 15;
		this.label62.Text = "Weight";
		this.txtRemarksInExtra.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarksInExtra.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksInExtra.Location = new System.Drawing.Point(821, 37);
		this.txtRemarksInExtra.Name = "txtRemarksInExtra";
		this.txtRemarksInExtra.Size = new System.Drawing.Size(308, 23);
		this.txtRemarksInExtra.TabIndex = 2;
		this.label63.AutoSize = true;
		this.label63.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label63.Location = new System.Drawing.Point(825, 14);
		this.label63.Name = "label63";
		this.label63.Size = new System.Drawing.Size(132, 15);
		this.label63.TabIndex = 25;
		this.label63.Text = "Specification / Remarks";
		this.CmbPackuomInExtra.AllowNull = (DefaultableBoolean)1;
		this.CmbPackuomInExtra.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackuomInExtra.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackuomInExtra.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance284).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance284).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackuomInExtra).DisplayLayout.Appearance = (AppearanceBase)(object)appearance284;
		((UltraGridBase)this.CmbPackuomInExtra).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackuomInExtra).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance285).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance285).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance285).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance285).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackuomInExtra).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance285;
		((AppearanceBase)appearance286).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackuomInExtra).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance286;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackuomInExtra).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance287).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance287).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance287).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance287).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackuomInExtra).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance287;
		((UltraGridBase)this.CmbPackuomInExtra).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackuomInExtra).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance288).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance288).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackuomInExtra).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance288;
		((AppearanceBase)appearance289).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance289).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackuomInExtra).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance289;
		((UltraGridBase)this.CmbPackuomInExtra).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackuomInExtra).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance290).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackuomInExtra).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance290;
		((AppearanceBase)appearance292).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance292).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackuomInExtra).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance292;
		((UltraGridBase)this.CmbPackuomInExtra).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackuomInExtra).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance293).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance293).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance293).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance293).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance293).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackuomInExtra).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance293;
		((AppearanceBase)appearance294).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackuomInExtra).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance294;
		((UltraGridBase)this.CmbPackuomInExtra).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackuomInExtra).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance295).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance295).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackuomInExtra).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance295;
		((UltraGridBase)this.CmbPackuomInExtra).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance296).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackuomInExtra).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance296;
		((UltraGridBase)this.CmbPackuomInExtra).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackuomInExtra).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackuomInExtra).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackuomInExtra).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPackuomInExtra.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackuomInExtra).Location = new System.Drawing.Point(245, 80);
		((System.Windows.Forms.Control)(object)this.CmbPackuomInExtra).Name = "CmbPackuomInExtra";
		((System.Windows.Forms.Control)(object)this.CmbPackuomInExtra).Size = new System.Drawing.Size(109, 26);
		((System.Windows.Forms.Control)(object)this.CmbPackuomInExtra).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.CmbPackuomInExtra).Leave += new System.EventHandler(CmbPackuomInExtra_Leave);
		this.CmbMasterPackUomInExtra.AllowNull = (DefaultableBoolean)1;
		this.CmbMasterPackUomInExtra.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbMasterPackUomInExtra.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbMasterPackUomInExtra.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance168).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbMasterPackUomInExtra).DisplayLayout.Appearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.CmbMasterPackUomInExtra).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbMasterPackUomInExtra).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance212).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance212).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance212).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance212).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbMasterPackUomInExtra).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance212;
		((AppearanceBase)appearance224).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbMasterPackUomInExtra).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance224;
		((SpecialBoxBase)((UltraGridBase)this.CmbMasterPackUomInExtra).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance235).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance235).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance235).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance235).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbMasterPackUomInExtra).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance235;
		((UltraGridBase)this.CmbMasterPackUomInExtra).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbMasterPackUomInExtra).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance239).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance239).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbMasterPackUomInExtra).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance239;
		((AppearanceBase)appearance240).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance240).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbMasterPackUomInExtra).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance240;
		((UltraGridBase)this.CmbMasterPackUomInExtra).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbMasterPackUomInExtra).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance241).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbMasterPackUomInExtra).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance241;
		((AppearanceBase)appearance242).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance242).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbMasterPackUomInExtra).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance242;
		((UltraGridBase)this.CmbMasterPackUomInExtra).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbMasterPackUomInExtra).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance243).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance243).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance243).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance243).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance243).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbMasterPackUomInExtra).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance243;
		((AppearanceBase)appearance297).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbMasterPackUomInExtra).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance297;
		((UltraGridBase)this.CmbMasterPackUomInExtra).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbMasterPackUomInExtra).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance301).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance301).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbMasterPackUomInExtra).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance301;
		((UltraGridBase)this.CmbMasterPackUomInExtra).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance303).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbMasterPackUomInExtra).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance303;
		((UltraGridBase)this.CmbMasterPackUomInExtra).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbMasterPackUomInExtra).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbMasterPackUomInExtra).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbMasterPackUomInExtra).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbMasterPackUomInExtra.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbMasterPackUomInExtra).Location = new System.Drawing.Point(245, 35);
		((System.Windows.Forms.Control)(object)this.CmbMasterPackUomInExtra).Name = "CmbMasterPackUomInExtra";
		((System.Windows.Forms.Control)(object)this.CmbMasterPackUomInExtra).Size = new System.Drawing.Size(109, 26);
		((System.Windows.Forms.Control)(object)this.CmbMasterPackUomInExtra).TabIndex = 4;
		this.label55.AutoSize = true;
		this.label55.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label55.Location = new System.Drawing.Point(245, 64);
		this.label55.Name = "label55";
		this.label55.Size = new System.Drawing.Size(33, 15);
		this.label55.TabIndex = 8;
		this.label55.Text = "Uom";
		this.label65.AutoSize = true;
		this.label65.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label65.Location = new System.Drawing.Point(600, 14);
		this.label65.Name = "label65";
		this.label65.Size = new System.Drawing.Size(96, 15);
		this.label65.TabIndex = 27;
		this.label65.Text = "Item Description";
		this.label66.AutoSize = true;
		this.label66.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label66.Location = new System.Drawing.Point(245, 14);
		this.label66.Name = "label66";
		this.label66.Size = new System.Drawing.Size(72, 15);
		this.label66.TabIndex = 8;
		this.label66.Text = "Master Uom";
		this.label67.AutoSize = true;
		this.label67.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label67.Location = new System.Drawing.Point(357, 64);
		this.label67.Name = "label67";
		this.label67.Size = new System.Drawing.Size(26, 15);
		this.label67.TabIndex = 13;
		this.label67.Text = "Qty";
		this.label68.AutoSize = true;
		this.label68.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label68.Location = new System.Drawing.Point(5, 64);
		this.label68.Name = "label68";
		this.label68.Size = new System.Drawing.Size(46, 15);
		this.label68.TabIndex = 23;
		this.label68.Text = "Job Lot";
		this.txtQtyInExtra.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtQtyInExtra.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtQtyInExtra.Location = new System.Drawing.Point(357, 82);
		this.txtQtyInExtra.Name = "txtQtyInExtra";
		this.txtQtyInExtra.ShortcutsEnabled = false;
		this.txtQtyInExtra.Size = new System.Drawing.Size(118, 23);
		this.txtQtyInExtra.TabIndex = 5;
		this.txtQtyInExtra.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtQtyInExtra.TextChanged += new System.EventHandler(txtQtyInExtra_TextChanged);
		this.CmbJobLotInExtra.AllowNull = (DefaultableBoolean)1;
		this.CmbJobLotInExtra.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobLotInExtra.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobLotInExtra.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobLotInExtra).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbJobLotInExtra).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobLotInExtra).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLotInExtra).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLotInExtra).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLotInExtra).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLotInExtra).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbJobLotInExtra).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobLotInExtra).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobLotInExtra).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobLotInExtra).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbJobLotInExtra).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobLotInExtra).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLotInExtra).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance33).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobLotInExtra).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbJobLotInExtra).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobLotInExtra).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLotInExtra).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobLotInExtra).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbJobLotInExtra).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobLotInExtra).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobLotInExtra).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbJobLotInExtra).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobLotInExtra).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbJobLotInExtra).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobLotInExtra).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobLotInExtra).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobLotInExtra).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbJobLotInExtra.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobLotInExtra).Location = new System.Drawing.Point(5, 80);
		((System.Windows.Forms.Control)(object)this.CmbJobLotInExtra).Name = "CmbJobLotInExtra";
		((System.Windows.Forms.Control)(object)this.CmbJobLotInExtra).Size = new System.Drawing.Size(237, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobLotInExtra).TabIndex = 3;
		this.CmbMasterItemInExtra.AllowNull = (DefaultableBoolean)1;
		this.CmbMasterItemInExtra.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbMasterItemInExtra.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbMasterItemInExtra.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance257).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance257).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbMasterItemInExtra).DisplayLayout.Appearance = (AppearanceBase)(object)appearance257;
		((UltraGridBase)this.CmbMasterItemInExtra).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbMasterItemInExtra).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance259).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance259).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance259).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance259).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbMasterItemInExtra).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance259;
		((AppearanceBase)appearance260).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbMasterItemInExtra).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance260;
		((SpecialBoxBase)((UltraGridBase)this.CmbMasterItemInExtra).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance261).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance261).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance261).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance261).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbMasterItemInExtra).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance261;
		((UltraGridBase)this.CmbMasterItemInExtra).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbMasterItemInExtra).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance262).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance262).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbMasterItemInExtra).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance262;
		((AppearanceBase)appearance263).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance263).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbMasterItemInExtra).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance263;
		((UltraGridBase)this.CmbMasterItemInExtra).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbMasterItemInExtra).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbMasterItemInExtra).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance107;
		((AppearanceBase)appearance108).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance108).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbMasterItemInExtra).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.CmbMasterItemInExtra).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbMasterItemInExtra).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance221).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance221).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance221).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance221).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance221).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbMasterItemInExtra).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance221;
		((AppearanceBase)appearance267).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbMasterItemInExtra).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance267;
		((UltraGridBase)this.CmbMasterItemInExtra).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbMasterItemInExtra).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance222).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance222).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbMasterItemInExtra).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance222;
		((UltraGridBase)this.CmbMasterItemInExtra).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance225).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbMasterItemInExtra).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance225;
		((UltraGridBase)this.CmbMasterItemInExtra).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbMasterItemInExtra).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbMasterItemInExtra).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbMasterItemInExtra).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbMasterItemInExtra.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbMasterItemInExtra).Location = new System.Drawing.Point(5, 35);
		((System.Windows.Forms.Control)(object)this.CmbMasterItemInExtra).Name = "CmbMasterItemInExtra";
		((System.Windows.Forms.Control)(object)this.CmbMasterItemInExtra).Size = new System.Drawing.Size(237, 26);
		((System.Windows.Forms.Control)(object)this.CmbMasterItemInExtra).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbMasterItemInExtra).Leave += new System.EventHandler(CmbMasterItemInExtra_Leave);
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label54.Location = new System.Drawing.Point(5, 14);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(71, 15);
		this.label54.TabIndex = 9;
		this.label54.Text = "Master Item";
		this.CmbItemInExtra.AllowNull = (DefaultableBoolean)1;
		this.CmbItemInExtra.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemInExtra.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemInExtra.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance304).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance304).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemInExtra).DisplayLayout.Appearance = (AppearanceBase)(object)appearance304;
		((UltraGridBase)this.CmbItemInExtra).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemInExtra).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance305).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance305).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance305).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance305).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemInExtra).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance305;
		((AppearanceBase)appearance306).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemInExtra).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance306;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemInExtra).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance307).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance307).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance307).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance307).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemInExtra).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance307;
		((UltraGridBase)this.CmbItemInExtra).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemInExtra).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance308).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance308).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemInExtra).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance308;
		((AppearanceBase)appearance309).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance309).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemInExtra).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance309;
		((UltraGridBase)this.CmbItemInExtra).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemInExtra).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemInExtra).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance105;
		((AppearanceBase)appearance109).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance109).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemInExtra).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.CmbItemInExtra).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemInExtra).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance110).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance110).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance110).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance110).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemInExtra).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance110;
		((AppearanceBase)appearance111).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemInExtra).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.CmbItemInExtra).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemInExtra).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance114).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemInExtra).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.CmbItemInExtra).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemInExtra).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.CmbItemInExtra).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemInExtra).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemInExtra).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemInExtra).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemInExtra.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemInExtra).Location = new System.Drawing.Point(357, 35);
		((System.Windows.Forms.Control)(object)this.CmbItemInExtra).Name = "CmbItemInExtra";
		((System.Windows.Forms.Control)(object)this.CmbItemInExtra).Size = new System.Drawing.Size(237, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemInExtra).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbItemInExtra).Leave += new System.EventHandler(CmbItemInExtra_Leave);
		this.label69.AutoSize = true;
		this.label69.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label69.Location = new System.Drawing.Point(357, 14);
		this.label69.Name = "label69";
		this.label69.Size = new System.Drawing.Size(67, 15);
		this.label69.TabIndex = 9;
		this.label69.Text = "Item Name";
		this.tabPagePaymentDetail.Controls.Add((System.Windows.Forms.Control)(object)this.grdPaymentTerm);
		this.tabPagePaymentDetail.Controls.Add(this.panel11);
		this.tabPagePaymentDetail.Location = new System.Drawing.Point(4, 25);
		this.tabPagePaymentDetail.Name = "tabPagePaymentDetail";
		this.tabPagePaymentDetail.Padding = new System.Windows.Forms.Padding(3);
		this.tabPagePaymentDetail.Size = new System.Drawing.Size(1216, 317);
		this.tabPagePaymentDetail.TabIndex = 1;
		this.tabPagePaymentDetail.Text = "PaymentDetail";
		this.tabPagePaymentDetail.UseVisualStyleBackColor = true;
		((ScrollableBase)this.grdPaymentTerm).BackColor = System.Drawing.Color.White;
		this.grdPaymentTerm.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdPaymentTerm_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPaymentTerm.DesignTimeLayout = grdPaymentTerm_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPaymentTerm).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdPaymentTerm).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPaymentTerm.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdPaymentTerm).Location = new System.Drawing.Point(3, 29);
		((System.Windows.Forms.Control)(object)this.grdPaymentTerm).Name = "grdPaymentTerm";
		((ScrollableBase)this.grdPaymentTerm).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPaymentTerm).Size = new System.Drawing.Size(1210, 285);
		((System.Windows.Forms.Control)(object)this.grdPaymentTerm).TabIndex = 2;
		this.grdPaymentTerm.TotalRow = (InheritableBoolean)1;
		this.grdPaymentTerm.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPaymentTerm).VisualStyle = (VisualStyle)4;
		this.grdPaymentTerm.CellUpdated += new ColumnActionEventHandler(grdPaymentTerm_CellUpdated);
		this.grdPaymentTerm.UpdatingCell += new UpdatingCellEventHandler(grdPaymentTerm_UpdatingCell);
		this.grdPaymentTerm.ColumnButtonClick += new ColumnActionEventHandler(grdPaymentTerm_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdPaymentTerm).KeyDown += new System.Windows.Forms.KeyEventHandler(grdPaymentTerm_KeyDown);
		this.panel11.BackColor = System.Drawing.Color.Teal;
		this.panel11.Controls.Add(this.label57);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(3, 3);
		this.panel11.Margin = new System.Windows.Forms.Padding(2);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1210, 26);
		this.panel11.TabIndex = 1;
		this.label57.AutoSize = true;
		this.label57.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label57.ForeColor = System.Drawing.Color.White;
		this.label57.Location = new System.Drawing.Point(3, 3);
		this.label57.Name = "label57";
		this.label57.Size = new System.Drawing.Size(135, 19);
		this.label57.TabIndex = 561;
		this.label57.Text = "Payment Detail";
		this.panel4.Controls.Add(this.txtBagPrice);
		this.panel4.Controls.Add(this.combruom);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.combordercat);
		this.panel4.Controls.Add(this.label14);
		this.panel4.Controls.Add(this.combamounttype);
		this.panel4.Controls.Add(this.label25);
		this.panel4.Controls.Add(this.label42);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.combcomp1);
		this.panel4.Controls.Add(this.combcommtype);
		this.panel4.Controls.Add(this.label21);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbAgreementNo);
		this.panel4.Controls.Add(this.label9);
		this.panel4.Controls.Add(this.label20);
		this.panel4.Controls.Add(this.txtBagWeightCut);
		this.panel4.Controls.Add(this.label19);
		this.panel4.Controls.Add(this.txtcommremarks);
		this.panel4.Controls.Add(this.txtcatsr);
		this.panel4.Controls.Add(this.txtcommamount);
		this.panel4.Controls.Add(this.validate);
		this.panel4.Controls.Add(this.label11);
		this.panel4.Controls.Add(this.label27);
		this.panel4.Controls.Add(this.label1);
		this.panel4.Controls.Add(this.label2);
		this.panel4.Controls.Add(this.label4);
		this.panel4.Controls.Add(this.label7);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(2, 53);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1224, 181);
		this.panel4.TabIndex = 2;
		this.txtBagPrice.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBagPrice.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBagPrice.Location = new System.Drawing.Point(1649, 192);
		this.txtBagPrice.Name = "txtBagPrice";
		this.txtBagPrice.Size = new System.Drawing.Size(74, 25);
		this.txtBagPrice.TabIndex = 9;
		this.txtBagPrice.Visible = false;
		this.combruom.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
		this.combruom.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
		this.combruom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.combruom.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combruom.FormattingEnabled = true;
		this.combruom.Items.AddRange(new object[4] { "40", "50", "60", "100" });
		this.combruom.Location = new System.Drawing.Point(1491, 59);
		this.combruom.Name = "combruom";
		this.combruom.Size = new System.Drawing.Size(52, 25);
		this.combruom.TabIndex = 4;
		this.combruom.Visible = false;
		this.combruom.Leave += new System.EventHandler(combruom_Leave);
		this.combordercat.AllowNull = (DefaultableBoolean)1;
		this.combordercat.AutoCompleteMode = (AutoCompleteMode)4;
		this.combordercat.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combordercat.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance310).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance310).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combordercat).DisplayLayout.Appearance = (AppearanceBase)(object)appearance310;
		((UltraGridBase)this.combordercat).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combordercat).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance311).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance311).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance311).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance311).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combordercat).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance311;
		((AppearanceBase)appearance312).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combordercat).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance312;
		((SpecialBoxBase)((UltraGridBase)this.combordercat).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combordercat).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.combordercat).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combordercat).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combordercat).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combordercat).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.combordercat).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combordercat).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combordercat).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combordercat).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.combordercat).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combordercat).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combordercat).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.combordercat).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.combordercat).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combordercat).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combordercat).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.combordercat).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combordercat).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.combordercat).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combordercat).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combordercat).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combordercat).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combordercat.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combordercat).Location = new System.Drawing.Point(1336, 5);
		((System.Windows.Forms.Control)(object)this.combordercat).Name = "combordercat";
		((System.Windows.Forms.Control)(object)this.combordercat).Size = new System.Drawing.Size(51, 24);
		((System.Windows.Forms.Control)(object)this.combordercat).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.combordercat).TabStop = false;
		((System.Windows.Forms.Control)(object)this.combordercat).Visible = false;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(1647, 170);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(62, 17);
		this.label14.TabIndex = 32;
		this.label14.Text = "Bag Price";
		this.label14.Visible = false;
		this.combamounttype.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
		this.combamounttype.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
		this.combamounttype.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.combamounttype.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combamounttype.FormattingEnabled = true;
		this.combamounttype.Items.AddRange(new object[2] { "Weight", "Qty" });
		this.combamounttype.Location = new System.Drawing.Point(1666, 140);
		this.combamounttype.Name = "combamounttype";
		this.combamounttype.Size = new System.Drawing.Size(119, 25);
		this.combamounttype.TabIndex = 7;
		this.combamounttype.Visible = false;
		this.combamounttype.SelectedIndexChanged += new System.EventHandler(combamounttype_SelectedIndexChanged);
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(1389, 122);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(100, 17);
		this.label25.TabIndex = 3;
		this.label25.Text = "Comm Remarks";
		this.label25.Visible = false;
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label42.Location = new System.Drawing.Point(1663, 117);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(84, 17);
		this.label42.TabIndex = 27;
		this.label42.Text = "Amount Type";
		this.label42.Visible = false;
		this.combcomp1.AllowNull = (DefaultableBoolean)1;
		this.combcomp1.AutoCompleteMode = (AutoCompleteMode)4;
		this.combcomp1.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combcomp1.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combcomp1).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.combcomp1).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combcomp1).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combcomp1).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combcomp1).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance57;
		((SpecialBoxBase)((UltraGridBase)this.combcomp1).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combcomp1).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.combcomp1).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combcomp1).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combcomp1).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combcomp1).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.combcomp1).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combcomp1).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combcomp1).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance101;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance113).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combcomp1).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.combcomp1).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combcomp1).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance124).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance124).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance124).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance124).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combcomp1).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance124;
		((AppearanceBase)appearance135).TextHAlignAsString = "Left";
		((UltraGridBase)this.combcomp1).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.combcomp1).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combcomp1).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combcomp1).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.combcomp1).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combcomp1).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.combcomp1).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combcomp1).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combcomp1).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combcomp1).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combcomp1.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combcomp1).Location = new System.Drawing.Point(1336, 60);
		((System.Windows.Forms.Control)(object)this.combcomp1).Name = "combcomp1";
		((System.Windows.Forms.Control)(object)this.combcomp1).Size = new System.Drawing.Size(51, 24);
		((System.Windows.Forms.Control)(object)this.combcomp1).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.combcomp1).TabStop = false;
		((System.Windows.Forms.Control)(object)this.combcomp1).Visible = false;
		this.combcommtype.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
		this.combcommtype.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
		this.combcommtype.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.combcommtype.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combcommtype.FormattingEnabled = true;
		this.combcommtype.Items.AddRange(new object[3] { "Flat", "Percent", "Comm Weight" });
		this.combcommtype.Location = new System.Drawing.Point(1491, 1);
		this.combcommtype.Name = "combcommtype";
		this.combcommtype.Size = new System.Drawing.Size(52, 25);
		this.combcommtype.TabIndex = 2;
		this.combcommtype.Visible = false;
		this.combcommtype.Leave += new System.EventHandler(combcommtype_Leave);
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(1389, 63);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(69, 17);
		this.label21.TabIndex = 8;
		this.label21.Text = "Rate UOM";
		this.label21.Visible = false;
		this.CmbAgreementNo.AllowNull = (DefaultableBoolean)1;
		this.CmbAgreementNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbAgreementNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbAgreementNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbAgreementNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.CmbAgreementNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbAgreementNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbAgreementNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAgreementNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance54;
		((SpecialBoxBase)((UltraGridBase)this.CmbAgreementNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAgreementNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbAgreementNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbAgreementNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbAgreementNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbAgreementNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbAgreementNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbAgreementNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAgreementNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance60).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbAgreementNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.CmbAgreementNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbAgreementNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance61).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAgreementNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbAgreementNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.CmbAgreementNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbAgreementNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbAgreementNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbAgreementNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbAgreementNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.CmbAgreementNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbAgreementNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbAgreementNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbAgreementNo).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbAgreementNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbAgreementNo).Location = new System.Drawing.Point(1336, 90);
		((System.Windows.Forms.Control)(object)this.CmbAgreementNo).Name = "CmbAgreementNo";
		((System.Windows.Forms.Control)(object)this.CmbAgreementNo).Size = new System.Drawing.Size(51, 24);
		((System.Windows.Forms.Control)(object)this.CmbAgreementNo).TabIndex = 9;
		((System.Windows.Forms.Control)(object)this.CmbAgreementNo).TabStop = false;
		((System.Windows.Forms.Control)(object)this.CmbAgreementNo).Visible = false;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(1728, 170);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(47, 17);
		this.label9.TabIndex = 13;
		this.label9.Text = "Wt Cut";
		this.label9.Visible = false;
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(1389, 5);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(77, 17);
		this.label20.TabIndex = 3;
		this.label20.Text = "Comm Type";
		this.label20.Visible = false;
		this.txtBagWeightCut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBagWeightCut.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBagWeightCut.Location = new System.Drawing.Point(1725, 192);
		this.txtBagWeightCut.Name = "txtBagWeightCut";
		this.txtBagWeightCut.Size = new System.Drawing.Size(74, 25);
		this.txtBagWeightCut.TabIndex = 10;
		this.txtBagWeightCut.Visible = false;
		this.txtBagWeightCut.TextChanged += new System.EventHandler(txtqty_TextChanged);
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(1389, 92);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(95, 17);
		this.label19.TabIndex = 0;
		this.label19.Text = "Comm Amount";
		this.label19.Visible = false;
		this.txtcommremarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcommremarks.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcommremarks.Location = new System.Drawing.Point(1491, 117);
		this.txtcommremarks.Name = "txtcommremarks";
		this.txtcommremarks.Size = new System.Drawing.Size(52, 23);
		this.txtcommremarks.TabIndex = 6;
		this.txtcommremarks.Visible = false;
		this.txtcatsr.BackColor = System.Drawing.Color.White;
		this.txtcatsr.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcatsr.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcatsr.Location = new System.Drawing.Point(1336, 33);
		this.txtcatsr.Name = "txtcatsr";
		this.txtcatsr.ReadOnly = true;
		this.txtcatsr.Size = new System.Drawing.Size(51, 21);
		this.txtcatsr.TabIndex = 4;
		this.txtcatsr.TabStop = false;
		this.txtcatsr.Visible = false;
		this.txtcommamount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcommamount.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcommamount.Location = new System.Drawing.Point(1491, 88);
		this.txtcommamount.Name = "txtcommamount";
		this.txtcommamount.ReadOnly = true;
		this.txtcommamount.Size = new System.Drawing.Size(52, 23);
		this.txtcommamount.TabIndex = 5;
		this.txtcommamount.Visible = false;
		this.validate.CustomFormat = "dd-MMM-yyyy";
		this.validate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.validate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.validate.Location = new System.Drawing.Point(1336, 120);
		this.validate.Name = "validate";
		this.validate.Size = new System.Drawing.Size(51, 21);
		this.validate.TabIndex = 15;
		this.validate.TabStop = false;
		this.validate.Visible = false;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(1389, 34);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(76, 17);
		this.label11.TabIndex = 0;
		this.label11.Text = "Comm Rate";
		this.label11.Visible = false;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(1246, 123);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(72, 15);
		this.label27.TabIndex = 0;
		this.label27.Text = "Validite Date";
		this.label27.Visible = false;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(1246, 64);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(59, 15);
		this.label1.TabIndex = 0;
		this.label1.Text = "Company";
		this.label1.Visible = false;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(1246, 35);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(68, 15);
		this.label2.TabIndex = 8;
		this.label2.Text = "Catagory Sr";
		this.label2.Visible = false;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(1246, 95);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(82, 15);
		this.label4.TabIndex = 0;
		this.label4.Text = "AgreementNo";
		this.label4.Visible = false;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(1246, 8);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(88, 15);
		this.label7.TabIndex = 0;
		this.label7.Text = "Order Catagory";
		this.label7.Visible = false;
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.label23);
		this.panel3.Controls.Add(this.ChkBoxPrint);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(2, 27);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1224, 26);
		this.panel3.TabIndex = 1;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(3, 3);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(95, 19);
		this.label23.TabIndex = 0;
		this.label23.Text = "Sale Order";
		this.ChkBoxPrint.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkBoxPrint.AutoSize = true;
		this.ChkBoxPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkBoxPrint.ForeColor = System.Drawing.Color.White;
		this.ChkBoxPrint.Location = new System.Drawing.Point(1137, 3);
		this.ChkBoxPrint.Name = "ChkBoxPrint";
		this.ChkBoxPrint.Size = new System.Drawing.Size(85, 20);
		this.ChkBoxPrint.TabIndex = 21;
		this.ChkBoxPrint.Text = "Preview";
		this.ChkBoxPrint.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(2, 2);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1224, 25);
		this.panel2.TabIndex = 0;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[15]
		{
			this.btnnew, this.toolStripButton1, this.toolStripSeparator2, this.btnupdate, this.btnDelete, this.BtnSaveAs, this.btnsave, this.toolStripSeparator3, this.btnattachment, this.toolStripSeparator4,
			this.btnprint, this.toolStripSeparator5, this.btnshortcutkeys, this.toolStripSeparator1, this.btnCity
		});
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1224, 27);
		this.toolStrip1.TabIndex = 0;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(60, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.toolStripButton1.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton1.Name = "toolStripButton1";
		this.toolStripButton1.Size = new System.Drawing.Size(81, 24);
		this.toolStripButton1.Text = "Refresh";
		this.toolStripButton1.Click += new System.EventHandler(toolStripButton1_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
		this.btnupdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnupdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnupdate.Name = "btnupdate";
		this.btnupdate.Size = new System.Drawing.Size(79, 24);
		this.btnupdate.Text = "&Update";
		this.btnupdate.Click += new System.EventHandler(btnupdate_Click);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(74, 24);
		this.btnDelete.Text = "Delete";
		this.btnDelete.Visible = false;
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.BtnSaveAs.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.BtnSaveAs.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnSaveAs.Name = "BtnSaveAs";
		this.BtnSaveAs.Size = new System.Drawing.Size(86, 24);
		this.BtnSaveAs.Text = "&Save As";
		this.BtnSaveAs.Visible = false;
		this.BtnSaveAs.Click += new System.EventHandler(BtnSaveAs_Click);
		this.btnsave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(65, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.toolStripSeparator3.Name = "toolStripSeparator3";
		this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
		this.btnattachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnattachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnattachment.Name = "btnattachment";
		this.btnattachment.Size = new System.Drawing.Size(110, 24);
		this.btnattachment.Text = "&Attachment";
		this.btnattachment.Click += new System.EventHandler(btnattachment_Click);
		this.toolStripSeparator4.Name = "toolStripSeparator4";
		this.toolStripSeparator4.Size = new System.Drawing.Size(6, 27);
		this.btnprint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnprint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnprint.Name = "btnprint";
		this.btnprint.Size = new System.Drawing.Size(101, 24);
		this.btnprint.Text = "1605-Print";
		this.btnprint.Click += new System.EventHandler(btnprint_Click);
		this.toolStripSeparator5.Name = "toolStripSeparator5";
		this.toolStripSeparator5.Size = new System.Drawing.Size(6, 27);
		this.btnshortcutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshortcutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnshortcutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnshortcutkeys.Name = "btnshortcutkeys";
		this.btnshortcutkeys.Size = new System.Drawing.Size(127, 24);
		this.btnshortcutkeys.Text = "ShortCut Keys";
		this.btnshortcutkeys.Click += new System.EventHandler(btnshortcutkeys_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.btnCity.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnCity.Name = "btnCity";
		this.btnCity.Size = new System.Drawing.Size(84, 24);
		this.btnCity.Text = "Define City";
		this.btnCity.Click += new System.EventHandler(btnCity_Click);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.grdhistory);
		this.tabPage2.Controls.Add(this.panel9);
		this.tabPage2.Controls.Add(this.panel7);
		this.tabPage2.Controls.Add(this.panel8);
		this.tabPage2.Controls.Add(this.panel13);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage2.Size = new System.Drawing.Size(1228, 582);
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
		((System.Windows.Forms.Control)(object)this.grdhistory).Size = new System.Drawing.Size(1224, 225);
		((System.Windows.Forms.Control)(object)this.grdhistory).TabIndex = 52;
		this.grdhistory.TotalRow = (InheritableBoolean)1;
		this.grdhistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdhistory).VisualStyle = (VisualStyle)4;
		this.grdhistory.ColumnButtonClick += new ColumnActionEventHandler(grdhistory_ColumnButtonClick);
		this.grdhistory.SelectionChanged += new System.EventHandler(grdhistory_SelectionChanged);
		this.grdhistory.LinkClicked += new ColumnActionEventHandler(grdhistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdhistory).DoubleClick += new System.EventHandler(grdhistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdhistory).KeyDown += new System.Windows.Forms.KeyEventHandler(grdhistory_KeyDown);
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.ctrlGrdBar2);
		this.panel9.Controls.Add(this.label64);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(2, 124);
		this.panel9.Margin = new System.Windows.Forms.Padding(2);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1224, 26);
		this.panel9.TabIndex = 51;
		this.label64.AutoSize = true;
		this.label64.BackColor = System.Drawing.Color.Teal;
		this.label64.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label64.ForeColor = System.Drawing.Color.White;
		this.label64.Location = new System.Drawing.Point(3, 2);
		this.label64.Name = "label64";
		this.label64.Size = new System.Drawing.Size(134, 19);
		this.label64.TabIndex = 1;
		this.label64.Text = "Filtered Record";
		this.panel7.Controls.Add(this.groupBox4);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(2, 53);
		this.panel7.Margin = new System.Windows.Forms.Padding(2);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1224, 71);
		this.panel7.TabIndex = 50;
		this.groupBox4.Controls.Add(this.rdapproveddate);
		this.groupBox4.Controls.Add(this.rdmodifydate);
		this.groupBox4.Controls.Add(this.drdocdate);
		this.groupBox4.Controls.Add(this.rdentrydate);
		this.groupBox4.Controls.Add(this.btnShow);
		this.groupBox4.Controls.Add(this.label56);
		this.groupBox4.Controls.Add(this.label58);
		this.groupBox4.Controls.Add(this.label59);
		this.groupBox4.Controls.Add(this.label60);
		this.groupBox4.Controls.Add(this.label61);
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomerHistory);
		this.groupBox4.Controls.Add(this.ToDocNoHistory);
		this.groupBox4.Controls.Add(this.FromDocNoHistory);
		this.groupBox4.Controls.Add(this.ToDateHistory);
		this.groupBox4.Controls.Add(this.FromDateHistory);
		this.groupBox4.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox4.Location = new System.Drawing.Point(7, 3);
		this.groupBox4.Name = "groupBox4";
		this.groupBox4.Size = new System.Drawing.Size(1211, 60);
		this.groupBox4.TabIndex = 1;
		this.groupBox4.TabStop = false;
		this.groupBox4.Text = "Filters";
		this.rdapproveddate.AutoSize = true;
		this.rdapproveddate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdapproveddate.Location = new System.Drawing.Point(1105, 33);
		this.rdapproveddate.Name = "rdapproveddate";
		this.rdapproveddate.Size = new System.Drawing.Size(105, 19);
		this.rdapproveddate.TabIndex = 8;
		this.rdapproveddate.Text = "Approved Date";
		this.rdapproveddate.UseVisualStyleBackColor = true;
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(1105, 11);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 7;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(1024, 11);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 5;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(1024, 33);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 6;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.btnShow.BackColor = System.Drawing.Color.Teal;
		this.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShow.ForeColor = System.Drawing.Color.White;
		this.btnShow.Location = new System.Drawing.Point(960, 22);
		this.btnShow.Margin = new System.Windows.Forms.Padding(2);
		this.btnShow.Name = "btnShow";
		this.btnShow.Size = new System.Drawing.Size(58, 25);
		this.btnShow.TabIndex = 9;
		this.btnShow.Text = "Show";
		this.btnShow.UseVisualStyleBackColor = false;
		this.btnShow.Click += new System.EventHandler(btnShow_Click);
		this.btnShow.Leave += new System.EventHandler(btnShow_Leave);
		this.label56.AutoSize = true;
		this.label56.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label56.Location = new System.Drawing.Point(709, 27);
		this.label56.Name = "label56";
		this.label56.Size = new System.Drawing.Size(34, 15);
		this.label56.TabIndex = 87;
		this.label56.Text = "Party";
		this.label58.AutoSize = true;
		this.label58.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label58.Location = new System.Drawing.Point(557, 27);
		this.label58.Name = "label58";
		this.label58.Size = new System.Drawing.Size(64, 15);
		this.label58.TabIndex = 86;
		this.label58.Text = "To Doc No";
		this.label59.AutoSize = true;
		this.label59.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label59.Location = new System.Drawing.Point(395, 27);
		this.label59.Name = "label59";
		this.label59.Size = new System.Drawing.Size(79, 15);
		this.label59.TabIndex = 85;
		this.label59.Text = "From Doc No";
		this.label60.AutoSize = true;
		this.label60.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label60.Location = new System.Drawing.Point(205, 27);
		this.label60.Name = "label60";
		this.label60.Size = new System.Drawing.Size(48, 15);
		this.label60.TabIndex = 84;
		this.label60.Text = "To Date";
		this.label61.AutoSize = true;
		this.label61.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label61.Location = new System.Drawing.Point(5, 27);
		this.label61.Name = "label61";
		this.label61.Size = new System.Drawing.Size(63, 15);
		this.label61.TabIndex = 83;
		this.label61.Text = "From Date";
		this.CmbCustomerHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomerHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomerHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomerHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance264).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance264).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance264;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance265).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance265).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance265).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance265).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance265;
		((AppearanceBase)appearance266).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance266;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance268).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance268).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance268).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance268).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance268;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance270).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance270).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance270;
		((AppearanceBase)appearance271).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance271).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance271;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance274).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance274;
		((AppearanceBase)appearance275).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance275).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance275;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance276).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance276).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance276).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance276).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance276).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance276;
		((AppearanceBase)appearance277).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance277;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance278).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance278).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance278;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance279).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance279;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomerHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Location = new System.Drawing.Point(745, 21);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Name = "CmbCustomerHistory";
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Size = new System.Drawing.Size(210, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).TabIndex = 4;
		this.ToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.ToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDocNoHistory.Location = new System.Drawing.Point(627, 23);
		this.ToDocNoHistory.Name = "ToDocNoHistory";
		this.ToDocNoHistory.ShortcutsEnabled = false;
		this.ToDocNoHistory.Size = new System.Drawing.Size(78, 23);
		this.ToDocNoHistory.TabIndex = 3;
		this.ToDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(ToDocNoHistory_KeyPress);
		this.FromDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.FromDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDocNoHistory.Location = new System.Drawing.Point(476, 23);
		this.FromDocNoHistory.Name = "FromDocNoHistory";
		this.FromDocNoHistory.ShortcutsEnabled = false;
		this.FromDocNoHistory.Size = new System.Drawing.Size(78, 23);
		this.FromDocNoHistory.TabIndex = 2;
		this.FromDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(FromDocNoHistory_KeyPress);
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(259, 23);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(132, 23);
		this.ToDateHistory.TabIndex = 1;
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(71, 23);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(132, 23);
		this.FromDateHistory.TabIndex = 0;
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.btnLoad);
		this.panel8.Controls.Add(this.label15);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(2, 2);
		this.panel8.Margin = new System.Windows.Forms.Padding(2);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1224, 51);
		this.panel8.TabIndex = 0;
		this.btnLoad.Font = new System.Drawing.Font("Verdana", 11.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLoad.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.btnLoad.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnResetHistory, this.btnRefreshHistory });
		this.btnLoad.Location = new System.Drawing.Point(0, 0);
		this.btnLoad.Name = "btnLoad";
		this.btnLoad.Size = new System.Drawing.Size(1224, 27);
		this.btnLoad.TabIndex = 23;
		this.btnLoad.Text = "toolStrip2";
		this.btnResetHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnResetHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnResetHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnResetHistory.Name = "btnResetHistory";
		this.btnResetHistory.Size = new System.Drawing.Size(60, 24);
		this.btnResetHistory.Text = "New";
		this.btnResetHistory.Click += new System.EventHandler(btnResetHistory_Click);
		this.btnRefreshHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshHistory.Name = "btnRefreshHistory";
		this.btnRefreshHistory.Size = new System.Drawing.Size(81, 24);
		this.btnRefreshHistory.Text = "Refresh";
		this.btnRefreshHistory.Click += new System.EventHandler(btnRefreshHistory_Click);
		this.label15.AutoSize = true;
		this.label15.BackColor = System.Drawing.Color.Teal;
		this.label15.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.Color.White;
		this.label15.Location = new System.Drawing.Point(5, 29);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(158, 19);
		this.label15.TabIndex = 1;
		this.label15.Text = "Sale Order History";
		this.panel13.BackColor = System.Drawing.Color.Teal;
		this.panel13.Controls.Add(this.label10);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel13.Location = new System.Drawing.Point(2, 375);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1224, 21);
		this.panel13.TabIndex = 48;
		this.label10.AutoSize = true;
		this.label10.BackColor = System.Drawing.Color.Teal;
		this.label10.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label10.ForeColor = System.Drawing.Color.White;
		this.label10.Location = new System.Drawing.Point(2, 3);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(236, 17);
		this.label10.TabIndex = 1;
		this.label10.Text = "Detail of the Above Selected Row";
		this.grdDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdDetail).BackColor = System.Drawing.Color.White;
		grdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Bottom;
		this.grdDetail.DynamicFiltering = true;
		this.grdDetail.FilterMode = (FilterMode)2;
		this.grdDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(2, 396);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1224, 184);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 49;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.combsalesman.AllowNull = (DefaultableBoolean)1;
		this.combsalesman.AutoCompleteMode = (AutoCompleteMode)4;
		this.combsalesman.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combsalesman.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combsalesman).DisplayLayout.Appearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.combsalesman).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combsalesman).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combsalesman).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combsalesman).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance41;
		((SpecialBoxBase)((UltraGridBase)this.combsalesman).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combsalesman).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.combsalesman).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combsalesman).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combsalesman).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combsalesman).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.combsalesman).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combsalesman).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combsalesman).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combsalesman).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.combsalesman).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combsalesman).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combsalesman).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).TextHAlignAsString = "Left";
		((UltraGridBase)this.combsalesman).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.combsalesman).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combsalesman).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combsalesman).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.combsalesman).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combsalesman).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.combsalesman).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combsalesman).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combsalesman).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combsalesman).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combsalesman.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combsalesman).Location = new System.Drawing.Point(117, 13);
		((System.Windows.Forms.Control)(object)this.combsalesman).Name = "combsalesman";
		((System.Windows.Forms.Control)(object)this.combsalesman).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.combsalesman).TabIndex = 1;
		this.CmbCropyr.AllowNull = (DefaultableBoolean)1;
		this.CmbCropyr.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCropyr.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCropyr.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance132).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Appearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance133).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance133).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropyr).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance133;
		((AppearanceBase)appearance134).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance134;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropyr).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance136).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance136).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance136).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance137).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance137;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance138).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance139;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance140).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance141).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance141).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance141).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance141).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance141;
		((AppearanceBase)appearance142).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCropyr).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCropyr.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCropyr).Location = new System.Drawing.Point(309, 191);
		((System.Windows.Forms.Control)(object)this.CmbCropyr).Name = "CmbCropyr";
		((System.Windows.Forms.Control)(object)this.CmbCropyr).Size = new System.Drawing.Size(115, 24);
		((System.Windows.Forms.Control)(object)this.CmbCropyr).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.CmbCropyr).TabStop = false;
		((System.Windows.Forms.Control)(object)this.CmbCropyr).Visible = false;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1171, 2);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 1;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(1151, 2);
		this.ctrlGrdBar3.MyGrid = this.grdExtraItems;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar3.TabIndex = 1;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1181, 0);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.ctrlGrdBar2.MyGrid = this.grdhistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(37, 25);
		this.ctrlGrdBar2.TabIndex = 24;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(1236, 611);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "SaleOrderWithQty";
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "SaleOrder";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(SaleOrderWithQty_FormClosing);
		base.Load += new System.EventHandler(PurchsaeOrder_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(PurchsaeOrder_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbCurrency).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBillType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbStatus).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combdeliverytrm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combpttrm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbbranch1).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combproject1).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combsuppname).EndInit();
		this.panel5.ResumeLayout(false);
		this.tabControl2.ResumeLayout(false);
		this.tabPageDetail.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel14.ResumeLayout(false);
		this.panel14.PerformLayout();
		this.panel6.ResumeLayout(false);
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDiscountType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCity).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRateUom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTaxType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItem).EndInit();
		this.tabExtraItems.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdExtraItems).EndInit();
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		this.panel12.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		this.flowLayoutPanel1.ResumeLayout(false);
		this.flowLayoutPanel1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPackuomInExtra).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbMasterPackUomInExtra).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLotInExtra).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbMasterItemInExtra).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItemInExtra).EndInit();
		this.tabPagePaymentDetail.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPaymentTerm).EndInit();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.combordercat).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combcomp1).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAgreementNo).EndInit();
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
		this.panel7.ResumeLayout(false);
		this.groupBox4.ResumeLayout(false);
		this.groupBox4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.btnLoad.ResumeLayout(false);
		this.btnLoad.PerformLayout();
		this.panel13.ResumeLayout(false);
		this.panel13.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combsalesman).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropyr).EndInit();
		base.ResumeLayout(false);
	}
}

}
