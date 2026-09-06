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
using Architecture.BLL.Pos;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Authentication;
using Architecture.Model.Inventory;
using Architecture.Model.POS;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Lookups;
using Architecture.WinApp.Properties;
using Infragistics.Shared;
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
namespace Architecture.WinApp.Sale
{

public class BookingOrder : Form
{
	public class ItemInfo
	{
		public int ScheduleId { get; set; }

		public int RateUomId { get; set; }

		public double ItemPrice { get; set; }

		public double RateUom { get; set; }
	}

	private int updateDetailIndex = -1;

	private int ItemId;

	private int DetailPoId;

	private int Id;

	private int DefaultDaysToLessFromHistoryFromDate;

	private bool UpdateMode;

	private bool Approved;

	private bool PreOrderExist;

	private bool SubsidiaryAccountAllownOnVouchers;

	private bool ItemSearchWithNameOrCode;

	private bool BranchImplemented;

	private double RoundingFactor;

	private string AttachmentsValues = "";

	private string CustomAttachmentsValues = "";

	private Attachment AT = new Attachment();

	private DropDownBind DDL = new DropDownBind();

	private DataTable table = new DataTable();

	private DataTable dtsuppcus = new DataTable();

	private DataTable dtitem = new DataTable();

	private DataTable dtBranch = new DataTable();

	private DataTable dtCity = new DataTable();

	private DataTable dtReferenceParty = new DataTable();

	private DataTable dtPackType = new DataTable();

	private Rightsobjects formright = new Rightsobjects();

	private LoadPreBookingOrder LoadTransactions;

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

	private Label label33;

	private Label label12;

	private TextBox txtsupprefno;

	private Label label7;

	private Label label8;

	private DateTimePicker DocDate;

	private Label label17;

	private TextBox txtdocno;

	private Label label6;

	private Label label25;

	private Label label21;

	private Label label20;

	private Label label19;

	private TextBox txtcommremarks;

	private TextBox txtCommrate;

	private Label label11;

	private TextBox txtcommamount;

	private Label label26;

	private TextBox txtduedays;

	private Label label24;

	private DateTimePicker duedate;

	private Label label32;

	private DateTimePicker deliverystartdate;

	private Label label30;

	private Label label29;

	private Label label28;

	private TextBox txtdeliverydays;

	private Panel panel5;

	private GroupBox groupBox3;

	private Button btnplus;

	private TextBox txtremarksdetail;

	private Label label47;

	private TextBox txtItemPrice;

	private TextBox txtamount;

	private TextBox txtweight;

	private TextBox txtqty;

	private Label label40;

	private Label label35;

	private Label label44;

	private Label label39;

	private Label label38;

	private Label label36;

	private Label label37;

	private Panel panel6;

	private Panel panel7;

	private Panel panel8;

	internal UltraCombo CmbCustomer;

	internal UltraCombo combsalesman;

	internal UltraCombo CmbItem;

	internal UltraCombo combitempck;

	internal UltraCombo combcityarea;

	private CtrlGrdBar ctrlGrdBar1;

	internal GridEX grd;

	private CtrlGrdBar ctrlGrdBar2;

	private Button btnUpdateDetail;

	private Button btnCancelUpdateDetial;

	private Label label3;

	internal UltraCombo CmbCropyr;

	private ToolStripButton btnRefresh;

	private CheckBox ChkBox;

	internal GridEX DataGridHistoryDetail;

	private ToolStrip toolStrip2;

	internal UltraCombo combordercat;

	internal UltraCombo combdeliverytrm;

	internal UltraCombo combpttrm;

	internal UltraCombo combruom;

	internal UltraCombo combcommtype;

	private ToolStripButton btnDelete;

	private TextBox txtcatsr;

	private TextBox txtPriceSchId;

	private TextBox txtRateUomId;

	private TextBox txtPartyDiscPrct;

	private TextBox txtNetRate;

	private Label label1;

	private Label label9;

	private TextBox txtRateUom;

	private Label label13;

	internal UltraCombo CmbOrderType;

	private Label label4;

	private GroupBox groupBox1;

	private TextBox txtNetAmount;

	private Label label10;

	private TextBox txtItemDiscPrct;

	private Label label5;

	internal UltraCombo cmbPackType;

	private Label label2;

	private TextBox txtItemDiscountAmt;

	private Label label14;

	private TextBox txtPackingAddless;

	private ToolStripButton BtnPackingPriceForm;

	private TextBox txtPartDiscAmt;

	private Label label16;

	private Label label15;

	private Label label18;

	private Label label22;

	private ToolStripSeparator toolStripSeparator1;

	private CheckBox chkCommissionOnSale;

	internal UltraCombo cmbReferenceParty;

	private Label label31;

	internal GridEX grdhistory;

	private Panel panel10;

	private Label label34;

	private Panel panel13;

	private Label label27;

	private GroupBox groupBox2;

	private Button btnShowHistory;

	private Label label50;

	private Label label51;

	private Label label52;

	private Label label53;

	private Label label54;

	internal UltraCombo CmbCustomerHistory;

	private TextBox ToDocNoHistory;

	private TextBox FromDocNoHistory;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	private ToolStripButton btnResetHistory;

	private ToolStripButton btnRefreshHistory;

	private RadioButton rdapproveddate;

	private RadioButton rdmodifydate;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	private ToolStripSeparator toolStripSeparator3;

	private ToolStripSeparator toolStripSeparator4;

	private ToolStripSeparator toolStripSeparator5;

	private ToolStripSeparator toolStripSeparator6;

	private ToolStripSeparator toolStripSeparator8;

	private ToolStripButton BtnLoader;

	private ToolStripSeparator toolStripSeparator10;

	private TextBox txtremarks;

	private RadioButton rdSearchByCode;

	private RadioButton rdSearchByName;

	private ToolStripButton BtnShortCutkeys;

	private TextBox txtBranchSrNo;

	internal UltraCombo cmbBranchName;

	private Label label41;

	private ToolStripButton btnGenerateCalculations;

	public BookingOrder(UserAccount user)
	{
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void BookingOrder_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(10, 10);
			base.AutoScrollMinSize = new Size(1250, 600);
			List<ConfigrationsAllocation> list = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "RoundingForItemPricing").ToList();
			if (list.Count > 0)
			{
				RoundingFactor = Conversion.ToDouble((object)list[0].ConfigKey.ToString());
			}
			formright = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnsave.Enabled = formright.DoHaveSaveRight;
			btnprint.Enabled = formright.DoHavePrintRights;
			btnDelete.Enabled = formright.DoHaveCanDelete;
			btnupdate.Enabled = formright.DoHaveUpdateRights;
			ChkBox.Checked = formright.DoHavePrintRights;
			ctrlGrdBar1.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar1.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar2.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar2.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar2.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar2.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar2.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar2.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			SubsidiaryAccountAllownOnVouchers = CommonServices.GetERPFeatureById(4);
			ItemSearchWithNameOrCode = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("ItemSearchByCode"));
			if (ItemSearchWithNameOrCode)
			{
				rdSearchByCode.Checked = true;
			}
			else
			{
				rdSearchByName.Checked = true;
			}
			BranchImplemented = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("SaleOrderBranchWise"));
			dtBranch.Columns.Add("Id", typeof(int));
			dtBranch.Columns.Add("BranchName", typeof(string));
			dtitem.Columns.Add("ItemId", typeof(int));
			dtitem.Columns.Add("ItemName");
			dtitem.Columns.Add("ItemPrice", typeof(double));
			dtitem.Columns.Add("RateUomId", typeof(int));
			dtitem.Columns.Add("ScheduleId", typeof(int));
			dtitem.Columns.Add("ItemCode");
			BranchSrNoFill();
			DocumentNoFill();
			OrderCatagoryfill();
			SupplierNameFill();
			OrderType();
			PaymentTerms();
			DeliveryTerms();
			CommissionTypeFill();
			CommissionUOMFill();
			ItemDetailFill();
			PackingTypeFill();
			cmbCityFill();
			ReferencePartiesFill();
			ConfigurationDefault();
			GenerateOrderCategoryNo();
			table.Columns.Add("Id", typeof(int));
			table.Columns.Add("PreOrderId", typeof(int));
			table.Columns.Add("PreOrderDetailId", typeof(int));
			table.Columns.Add("PreOrder", typeof(int));
			table.Columns.Add("ScheduleId", typeof(int));
			table.Columns.Add("ItemId", typeof(int));
			table.Columns.Add("ItemCode");
			table.Columns.Add("Item");
			table.Columns.Add("CropYearID", typeof(string));
			table.Columns.Add("CropYear");
			table.Columns.Add("PackingTypeID", typeof(int));
			table.Columns.Add("PackUomId", typeof(int));
			table.Columns.Add("PackScheduleUnitId", typeof(int));
			table.Columns.Add("PackUom");
			table.Columns.Add("QTY", typeof(double));
			table.Columns.Add("Weight", typeof(double));
			table.Columns.Add("ItemPrice", typeof(double));
			table.Columns.Add("PackingAddLess", typeof(double));
			table.Columns.Add("RateDiscount", typeof(double));
			table.Columns.Add("RateDiscountAmt", typeof(double));
			table.Columns.Add("Rate", typeof(double));
			table.Columns.Add("RateUomId", typeof(int));
			table.Columns.Add("RateUom");
			table.Columns.Add("Amount", typeof(double));
			table.Columns.Add("ItemDiscPrct", typeof(double));
			table.Columns.Add("ItemDiscAmt", typeof(double));
			table.Columns.Add("TotalAmount", typeof(double));
			table.Columns.Add("CityID", typeof(int));
			table.Columns.Add("RefPartyId", typeof(int));
			table.Columns.Add("Remarks");
			table.Columns.Add("CommOnSale", typeof(bool));
			table.Columns.Add("CostCenterId", typeof(int));
			grd.DataSource = table;
			grd.RetrieveStructure();
			grdSettings();
			btnupdate.Visible = false;
			btnsave.Visible = true;
			btnplus.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			txtPackingAddless.Enabled = false;
			CmbCustomer.Focus();
			DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("DefaultDaysToLessFromHistoryFromDate"));
			HistoryBranchComboFill();
			HistoryCombosFill();
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
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected O, but got Unknown
		try
		{
			int code = SaleOrder.GeneratePurchaseOrderCodeByDocId(new SaleOrder
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = 127,
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

	private void BranchSrNoFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		try
		{
			int code = SaleOrder.GenerateSaleOrderBranchCodeByDocId(new SaleOrder
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				DocumentTypeId = 127,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			if (code > 0)
			{
				txtBranchSrNo.Text = code.ToString();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void OrderCatagoryfill()
	{
		try
		{
			int ID = Conversion.ToInt(combordercat.Value);
			DataTable dt = InvOrderCategory.GetAll();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, combordercat, "Id", "OrderCategoryName", "OrderCategoryName", true);
				((UltraGridBase)combordercat).Rows[3].Activate();
				if (ID > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID))
					{
						combordercat.Value = ID;
					}
					else
					{
						((Control)(object)combordercat).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)combordercat).Text = string.Empty;
				((UltraGridBase)combordercat).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateOrderCategoryNo()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		try
		{
			int code = InvOrderCategory.GenerateSaleOrderCategoryCodebyId(new InvOrderCategory
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Id = Conversion.ToInt(combordercat.Value),
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			if (code > 0)
			{
				txtcatsr.Text = code.ToString();
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
			int ID = Conversion.ToInt(combpttrm.Value);
			DataTable dt = CommonServices.GetDueTermServiceBind();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, combpttrm, "Id", "TermsDescription", "Payment Term", true);
				((UltraGridBase)combpttrm).Rows[2].Activate();
				if (ID > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID))
					{
						combpttrm.Value = ID;
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

	private void DeliveryTerms()
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

	private void OrderType()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("OrderType");
			dt.Rows.Add(1, "Fixed");
			dt.Rows.Add(2, "Open");
			DDL.BindDDL(dt, CmbOrderType, "Id", "OrderType", "Order Type", ZeroIndex: false);
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
			int SupId = Conversion.ToInt(CmbCustomer.Value);
			int SalId = Conversion.ToInt(combsalesman.Value);
			DataTable dtsuppcus = new DataTable();
			if (SubsidiaryAccountAllownOnVouchers)
			{
				dtsuppcus = CommonServices.GetVendorsAndCustomers(2);
				if (dtsuppcus.Rows.Count > 0)
				{
					DDL.BindDDL(dtsuppcus, CmbCustomer, "Id", "CompanyName", "Customer Name", ZeroIndex: true);
					DDL.BindDDL(dtsuppcus, combsalesman, "Id", "CompanyName", "SalesMan Name", ZeroIndex: true);
				}
				((UltraGridBase)CmbCustomer).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				((UltraGridBase)combsalesman).DisplayLayout.Bands[0].Columns[3].Hidden = true;
			}
			else
			{
				dtsuppcus = CommonServices.SupplierCustomerGetforComboServiceBind();
				if (dtsuppcus.Rows.Count > 0)
				{
					DDL.BindDDL(dtsuppcus, CmbCustomer, "Id", "CompanyName", "Customer Name", ZeroIndex: true);
					DDL.BindDDL(dtsuppcus, combsalesman, "Id", "CompanyName", "SalesMan Name", ZeroIndex: true);
				}
			}
			if (SupId > 0)
			{
				if (dtsuppcus.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == SupId))
				{
					CmbCustomer.Value = SupId;
				}
				else
				{
					((Control)(object)CmbCustomer).Text = string.Empty;
				}
			}
			if (SalId > 0)
			{
				if (dtsuppcus.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == SalId))
				{
					combsalesman.Value = SalId;
				}
				else
				{
					((Control)(object)combsalesman).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CommissionTypeFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("CommissionType");
			dt.Rows.Add(1, "Flat");
			dt.Rows.Add(2, "Percent");
			dt.Rows.Add(3, "Comm Weight");
			DDL.BindDDL(dt, combcommtype, "Id", "CommissionType", "CommissionType", ZeroIndex: false);
			((UltraGridBase)combcommtype).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CommissionUOMFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("UOM");
			dt.Rows.Add(1, "40");
			dt.Rows.Add(2, "50");
			dt.Rows.Add(3, "60");
			dt.Rows.Add(4, "100");
			DDL.BindDDL(dt, combruom, "Id", "UOM", "UOM", ZeroIndex: false);
			((UltraGridBase)combruom).Rows[0].Activate();
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
			int ID = Conversion.ToInt(CmbItem.Value);
			dtitem.Rows.Clear();
			DataTable data = CommonServices.ItemBindFromPricingSchedule(6);
			if (data.Rows.Count > 0)
			{
				for (int i = 0; i < data.Rows.Count; i++)
				{
					dtitem.Rows.Add(data.Rows[i]["ItemId"], data.Rows[i]["ItemName"], data.Rows[i]["ItemPrice"], data.Rows[i]["RateUomId"], data.Rows[i]["Id"], data.Rows[i]["ItemCode"]);
				}
			}
			if (dtitem.Rows.Count > 0)
			{
				if (rdSearchByName.Checked)
				{
					DDL.BindDDL(dtitem, CmbItem, "ItemId", "ItemName", "Item", ZeroIndex: true);
				}
				else
				{
					DDL.BindDDL(dtitem, CmbItem, "ItemId", "ItemCode", "Item", ZeroIndex: true);
				}
				((UltraGridBase)CmbItem).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)CmbItem).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				((UltraGridBase)CmbItem).DisplayLayout.Bands[0].Columns[4].Hidden = true;
				((UltraGridBase)CmbItem).DisplayLayout.Bands[0].Columns[1].Width = 500;
				if (ID > 0)
				{
					if (dtitem.AsEnumerable().Any((DataRow row) => row.Field<int>("ItemId") == ID))
					{
						CmbItem.Value = ID;
					}
					else
					{
						((Control)(object)CmbItem).Text = string.Empty;
					}
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
			MessageBox.Show(ex.Message);
		}
	}

	public void bindRateUomAndItemPackUom(int ID)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		try
		{
			double Packuom = Conversion.ToDouble((object)((Control)(object)combitempck).Text);
			((Control)(object)combitempck).Text = string.Empty;
			DataTable dtuomsch = UOMSchedule.SearchByObject(new UOMSchedule
			{
				ItemId = Conversion.ToInt((object)ID),
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtuomsch.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("ScheduleId", typeof(int));
				dt.Columns.Add("Equivalent", typeof(double));
				for (int i = 0; i < dtuomsch.Rows.Count; i++)
				{
					dt.Rows.Add(dtuomsch.Rows[i]["Id"], dtuomsch.Rows[i]["ScheduleUnitId"], dtuomsch.Rows[i]["Equivalent"]);
				}
				DDL.BindDDL(dt, combitempck, "Id", "Equivalent", "PackUom", ZeroIndex: false);
				((UltraGridBase)combitempck).DisplayLayout.Bands[0].Columns[1].Hidden = true;
				if (Packuom > 0.0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<double>("Equivalent") == Packuom))
					{
						((Control)(object)combitempck).Text = Packuom.ToString();
					}
					else
					{
						((Control)(object)combitempck).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)combitempck).Text = string.Empty;
				((UltraGridBase)combitempck).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbCityFill()
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		try
		{
			int ID = Conversion.ToInt(combcityarea.Value);
			dtCity.Rows.Clear();
			City obj = new City();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			dtCity = City.GetAll(obj);
			if (dtCity.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtCity, combcityarea, "Id", "CityName", "City Name", true);
				((UltraGridBase)combcityarea).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				if (ID > 0)
				{
					if (dtCity.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID))
					{
						combcityarea.Value = ID;
					}
					else
					{
						((Control)(object)combcityarea).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)combcityarea).Text = string.Empty;
				((UltraGridBase)combcityarea).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ReferencePartiesFill()
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		try
		{
			DataTable dtRefParty = new DataTable();
			dtRefParty.Columns.Add("Id", typeof(int));
			dtRefParty.Columns.Add("ReferencePartyName");
			dtReferenceParty.Rows.Clear();
			ReferenceParties obj = new ReferenceParties();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			dtReferenceParty = ReferenceParties.GetAll(obj);
			if (dtReferenceParty.Rows.Count > 0)
			{
				for (int i = 0; i < dtReferenceParty.Rows.Count; i++)
				{
					dtRefParty.Rows.Add(dtReferenceParty.Rows[i]["Id"], dtReferenceParty.Rows[i]["ReferencePartyName"]);
				}
			}
			DDL.BindDDL(dtRefParty, cmbReferenceParty, "Id", "ReferencePartyName", "Reference Party", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CropYear()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Expected O, but got Unknown
		try
		{
			DataTable dtCrop = ItemPricingSchedule.getCropYearFromPricingScheduleByItemId(new ItemPricingSchedule
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				PriceTypeId = 6,
				ItemId = Conversion.ToInt(CmbItem.Value),
				EffectedDate = Conversion.ToDateTime((object)DocDate.Value)
			});
			string ID = Conversion.ToString((object)((Control)(object)CmbCropyr).Text);
			if (dtCrop != null && dtCrop.Rows.Count > 0)
			{
				DDL.BindDDL(dtCrop, CmbCropyr, "Id", "CropYear", "Crop Year", ZeroIndex: false);
				if (!string.IsNullOrEmpty(ID))
				{
					if (dtCrop.AsEnumerable().Any((DataRow row) => row.Field<string>("CropYear") == ID))
					{
						((Control)(object)CmbCropyr).Text = ID;
					}
					else
					{
						((UltraGridBase)CmbCropyr).Rows[0].Activate();
					}
				}
				else
				{
					((UltraGridBase)CmbCropyr).Rows[0].Activate();
				}
			}
			else
			{
				((Control)(object)CmbCropyr).Text = string.Empty;
				((UltraGridBase)CmbCropyr).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PackingTypeFill()
	{
		try
		{
			int Id = Conversion.ToInt(cmbPackType.Value);
			dtPackType = InvPackingType.Getall();
			if (dtPackType.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtPackType, cmbPackType, "Id", "PackTypeDesc", "PackTypeDesc", true);
				if (Id > 0)
				{
					if (dtPackType.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
					{
						cmbPackType.Value = Id;
					}
					else
					{
						((Control)(object)cmbPackType).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)cmbPackType).Text = string.Empty;
				((UltraGridBase)cmbPackType).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetCustomerDiscount()
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Expected O, but got Unknown
		try
		{
			if (((UltraGridBase)CmbItem).ActiveRow != null && ((UltraGridBase)CmbCustomer).ActiveRow != null)
			{
				double Discount = CustomerDiscountPolicy.GetCustomerDiscountByEffectiveDate(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					SupplierCustomerId = Conversion.ToInt(CmbCustomer.Value),
					EffectedDate = Conversion.ToDateTime((object)DocDate.Value),
					ItemId = Conversion.ToInt(CmbItem.Value)
				});
				if (Discount > 0.0)
				{
					txtPartyDiscPrct.Text = Discount.ToString();
				}
				else
				{
					txtPartyDiscPrct.Text = "0";
				}
			}
			TotalAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combordercat_Leave(object sender, EventArgs e)
	{
		GenerateOrderCategoryNo();
	}

	private void combsalesman_Leave(object sender, EventArgs e)
	{
		try
		{
			((Control)(object)combcommtype).Enabled = true;
			txtCommrate.Enabled = true;
			((Control)(object)combruom).Enabled = true;
			txtcommamount.Enabled = true;
			txtcommremarks.Enabled = true;
			if (base.ActiveControl != null && base.ActiveControl.Name == "combcommtype")
			{
				combcommtype.Focus();
			}
			TotalCommissionAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combsuppname_Leave(object sender, EventArgs e)
	{
		try
		{
			GetCustomerDiscount();
			GetPackingAddLess();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combitem_Leave(object sender, EventArgs e)
	{
		try
		{
			CropYear();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void rdSearchByName_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			DataTable dataTable = dtitem;
			if (dataTable != null && dataTable.Rows.Count > 0)
			{
				int ItemId = 0;
				if (Conversion.ToInt(CmbItem.Value) > 0)
				{
					ItemId = Conversion.ToInt(CmbItem.Value);
				}
				if (rdSearchByName.Checked)
				{
					DDL.BindDDL(dtitem, CmbItem, "ItemId", "ItemName", "Item Name", ZeroIndex: true);
				}
				else
				{
					DDL.BindDDL(dtitem, CmbItem, "ItemId", "ItemCode", "Item Name", ZeroIndex: true);
				}
				CmbItem.Value = ItemId;
				CmbItem.Focus();
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
				combcityarea.Value = CityId;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (txtdocno.Text.Trim() == string.Empty || txtdocno.Text.Trim() == "0" || txtdocno.Text.Trim() == null)
		{
			MessageBox.Show("Doc No Field is Required");
			txtdocno.Focus();
			return false;
		}
		if (((UltraGridBase)combordercat).ActiveRow == null || Conversion.ToInt(combordercat.Value) == 0)
		{
			MessageBox.Show("Order Category Field is Required");
			combordercat.Focus();
			return false;
		}
		if (((UltraGridBase)CmbCustomer).ActiveRow == null || Conversion.ToInt(CmbCustomer.Value) == 0)
		{
			MessageBox.Show("Customer Name Field is Required");
			CmbCustomer.Focus();
			return false;
		}
		if (((UltraGridBase)CmbOrderType).ActiveRow == null || Conversion.ToInt(CmbOrderType.Value) == 0)
		{
			MessageBox.Show("OrderType Field is Required");
			CmbOrderType.Focus();
			return false;
		}
		if (((UltraGridBase)combpttrm).ActiveRow == null || Conversion.ToInt(combpttrm.Value) == 0)
		{
			MessageBox.Show("Payment Term Field is Required");
			combpttrm.Focus();
			return false;
		}
		if (((UltraGridBase)combdeliverytrm).ActiveRow == null || Conversion.ToInt(combdeliverytrm.Value) == 0)
		{
			MessageBox.Show("Delivery Term Field is Required");
			combdeliverytrm.Focus();
			return false;
		}
		return true;
	}

	private bool FormValidationDetail()
	{
		if (((UltraGridBase)CmbItem).ActiveRow == null || Conversion.ToInt(CmbItem.Value) == 0)
		{
			MessageBox.Show("ItemName Field is Required");
			CmbItem.Focus();
			return false;
		}
		if (((UltraGridBase)CmbCustomer).ActiveRow == null || Conversion.ToInt(CmbCustomer.Value) == 0)
		{
			MessageBox.Show("Customer Name Field is Required");
			CmbCustomer.Focus();
			return false;
		}
		if (((UltraGridBase)combitempck).ActiveRow == null || Conversion.ToInt(combitempck.Value) == 0)
		{
			MessageBox.Show("PackUom Field is Required");
			combitempck.Focus();
			return false;
		}
		if (((UltraGridBase)CmbCropyr).ActiveRow == null || Conversion.ToInt(CmbCropyr.Value) == 0)
		{
			MessageBox.Show("Crop Year Field is Required");
			CmbCropyr.Focus();
			return false;
		}
		if (((UltraGridBase)cmbPackType).ActiveRow == null || Conversion.ToInt(cmbPackType.Value) == 0)
		{
			MessageBox.Show("Pack Type Field is Required");
			cmbPackType.Focus();
			return false;
		}
		if (txtqty.Text.Trim() == string.Empty || txtqty.Text.Trim() == "0")
		{
			MessageBox.Show("Qty Field is Required");
			txtqty.Focus();
			return false;
		}
		if (txtweight.Text.Trim() == string.Empty || txtweight.Text.Trim() == "0")
		{
			MessageBox.Show("Weight Field is Required");
			txtweight.Focus();
			return false;
		}
		if (txtItemPrice.Text.Trim() == string.Empty || txtItemPrice.Text.Trim() == "0")
		{
			MessageBox.Show("ItemPrice Field is Required");
			txtItemPrice.Focus();
			return false;
		}
		if (txtNetRate.Text.Trim() == string.Empty || txtNetRate.Text.Trim() == "0")
		{
			MessageBox.Show("NetRate Field is Required");
			txtNetRate.Focus();
			return false;
		}
		if (txtamount.Text.Trim() == string.Empty || txtamount.Text.Trim() == "0")
		{
			MessageBox.Show("Amount Field is Required");
			txtamount.Focus();
			return false;
		}
		return true;
	}

	private void ChkCommission_Click(object sender, EventArgs e)
	{
	}

	private void btnplus_Click(object sender, EventArgs e)
	{
		try
		{
			if (grd.RowCount > 0)
			{
				GridEXRow item = grd.GetRow(0);
				if (item != null && Conversion.ToInt(item.Cells["PreOrderId"].Value) > 0)
				{
					throw new Exception("You can not add manual Record because record against Pre order exist in Grid");
				}
			}
			if (FormValidationDetail())
			{
				table.Rows.Add(0, 0, 0, 0, Conversion.ToInt((object)txtPriceSchId.Text), Conversion.ToInt(CmbItem.Value), Conversion.ToString((object)((UltraDropDownBase)CmbItem).SelectedRow.Cells["ItemCode"].Text), Conversion.ToString((object)((UltraDropDownBase)CmbItem).SelectedRow.Cells["ItemName"].Text), Conversion.ToString(CmbCropyr.Value), Conversion.ToString((object)((Control)(object)CmbCropyr).Text.Trim()), Conversion.ToInt(cmbPackType.Value), Conversion.ToInt(combitempck.Value), Conversion.ToInt(((UltraDropDownBase)combitempck).SelectedRow.Cells[1].Value), ((Control)(object)combitempck).Text.Trim(), Conversion.ToDouble((object)txtqty.Text.Trim()), Conversion.ToDouble((object)txtweight.Text.Trim()), Conversion.ToDouble((object)txtItemPrice.Text.Trim()), Conversion.ToDouble((object)txtPackingAddless.Text.Trim()), Conversion.ToDouble((object)txtPartyDiscPrct.Text.Trim()), Conversion.ToDouble((object)txtPartDiscAmt.Text.Trim()), Conversion.ToDouble((object)txtNetRate.Text.Trim()), Conversion.ToInt((object)txtRateUomId.Text), txtRateUom.Text.Trim(), Conversion.ToDouble((object)txtamount.Text), Conversion.ToDouble((object)txtItemDiscPrct.Text), Conversion.ToDouble((object)txtItemDiscountAmt.Text.Trim()), Conversion.ToDouble((object)txtNetAmount.Text), Conversion.ToInt(combcityarea.Value), Conversion.ToInt(cmbReferenceParty.Value), Conversion.ToString((object)txtremarksdetail.Text.Trim()), chkCommissionOnSale.Checked, 0);
				grd.DataSource = table;
				grd.RetrieveStructure();
				grdSettings();
				((Control)(object)CmbCropyr).Text = string.Empty;
				((UltraGridBase)CmbCropyr).DataSource = null;
				CmbItem.Focus();
				ResetDetail();
				TotalCommissionAmount();
				((Control)(object)CmbCustomer).Enabled = false;
				DocDate.Enabled = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettings()
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DetailCombosInGrid();
			grd.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grd.RootTable.Columns)
			{
				item.HeaderStyle.TextAlignment = (TextAlignment)2;
			}
			grd.RootTable.Columns["Id"].EditType = (EditType)0;
			grd.RootTable.Columns["PreOrderId"].EditType = (EditType)0;
			grd.RootTable.Columns["PreOrderDetailId"].EditType = (EditType)0;
			grd.RootTable.Columns["PreOrder"].EditType = (EditType)0;
			grd.RootTable.Columns["ScheduleId"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemId"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemCode"].EditType = (EditType)0;
			grd.RootTable.Columns["Item"].EditType = (EditType)0;
			grd.RootTable.Columns["CropYearID"].EditType = (EditType)0;
			grd.RootTable.Columns["CropYear"].EditType = (EditType)0;
			grd.RootTable.Columns["PackUomId"].EditType = (EditType)0;
			grd.RootTable.Columns["PackScheduleUnitId"].EditType = (EditType)0;
			grd.RootTable.Columns["PackUom"].EditType = (EditType)0;
			grd.RootTable.Columns["QTY"].EditType = (EditType)(PreOrderExist ? 1 : 0);
			grd.RootTable.Columns["Weight"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemPrice"].EditType = (EditType)0;
			grd.RootTable.Columns["PackingAddLess"].EditType = (EditType)0;
			grd.RootTable.Columns["RateDiscount"].EditType = (EditType)0;
			grd.RootTable.Columns["RateDiscountAmt"].EditType = (EditType)0;
			grd.RootTable.Columns["Rate"].EditType = (EditType)0;
			grd.RootTable.Columns["RateUomId"].EditType = (EditType)0;
			grd.RootTable.Columns["RateUom"].EditType = (EditType)0;
			grd.RootTable.Columns["Amount"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemDiscPrct"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemDiscAmt"].EditType = (EditType)0;
			grd.RootTable.Columns["TotalAmount"].EditType = (EditType)0;
			grd.RootTable.Columns["Remarks"].EditType = (EditType)0;
			grd.RootTable.Columns["CommOnSale"].EditType = (EditType)0;
			grd.RootTable.Columns["CostCenterId"].EditType = (EditType)0;
			grd.RootTable.Columns["PreOrder"].Width = Constants.InventoryConstants.OrderNo;
			grd.RootTable.Columns["ItemCode"].Width = Constants.InventoryConstants.ItemCode;
			grd.RootTable.Columns["Item"].Width = Constants.InventoryConstants.ItemName;
			grd.RootTable.Columns["CropYear"].Width = Constants.InventoryConstants.CropYearConstant;
			grd.RootTable.Columns["PackingTypeID"].Width = Constants.InventoryConstants.PackingType;
			grd.RootTable.Columns["PackUom"].Width = Constants.InventoryConstants.UomConstant;
			grd.RootTable.Columns["QTY"].Width = Constants.InventoryConstants.QtyConstant;
			grd.RootTable.Columns["Weight"].Width = Constants.InventoryConstants.WeightConstant;
			grd.RootTable.Columns["ItemPrice"].Width = Constants.InventoryConstants.ExRateConstant;
			grd.RootTable.Columns["PackingAddLess"].Width = Constants.InventoryConstants.AddLessConstant;
			grd.RootTable.Columns["RateDiscount"].Width = Constants.InventoryConstants.ExRateConstant;
			grd.RootTable.Columns["RateDiscountAmt"].Width = Constants.InventoryConstants.ExRateConstant;
			grd.RootTable.Columns["Rate"].Width = Constants.InventoryConstants.ExRateConstant;
			grd.RootTable.Columns["RateUom"].Width = Constants.InventoryConstants.UomConstant;
			grd.RootTable.Columns["Amount"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["ItemDiscPrct"].Width = Constants.InventoryConstants.ExRateConstant;
			grd.RootTable.Columns["ItemDiscAmt"].Width = Constants.InventoryConstants.RangeConstant;
			grd.RootTable.Columns["TotalAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["CityID"].Width = Constants.InventoryConstants.CityName;
			grd.RootTable.Columns["RefPartyId"].Width = Constants.InventoryConstants.ReferenceParty;
			grd.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants;
			grd.RootTable.Columns["CommOnSale"].Width = 60;
			grd.RootTable.Columns["Item"].Caption = "Item Name";
			grd.RootTable.Columns["Rate"].Caption = "Net Rate";
			grd.RootTable.Columns["PackingAddLess"].Caption = "Packing Exp/Disc";
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["PreOrder"].Visible = PreOrderExist;
			grd.RootTable.Columns["PreOrderId"].Visible = false;
			grd.RootTable.Columns["PreOrderDetailId"].Visible = false;
			grd.RootTable.Columns["ScheduleId"].Visible = false;
			grd.RootTable.Columns["ItemId"].Visible = false;
			grd.RootTable.Columns["PackUomId"].Visible = false;
			grd.RootTable.Columns["CropYearID"].Visible = false;
			grd.RootTable.Columns["RateUomId"].Visible = false;
			grd.RootTable.Columns["PackScheduleUnitId"].Visible = false;
			grd.RootTable.Columns["CommOnsale"].Visible = false;
			grd.RootTable.Columns["CostCenterId"].Visible = false;
			grd.RootTable.Columns["QTY"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["RateDiscountAmt"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ItemDiscAmt"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["TotalAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["QTY"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["RateDiscountAmt"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemDiscAmt"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["TotalAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemPrice"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemPrice"].FormatString = "#,#";
			grd.RootTable.Columns["QTY"].FormatString = "#,#";
			grd.RootTable.Columns["Weight"].FormatString = "#,#";
			grd.RootTable.Columns["Amount"].FormatString = "#,#";
			grd.RootTable.Columns["RateDiscountAmt"].FormatString = "#,#";
			grd.RootTable.Columns["ItemDiscAmt"].FormatString = "#,#";
			grd.RootTable.Columns["TotalAmount"].FormatString = "#,#";
			grd.RootTable.Columns["QTY"].TotalFormatString = "#,#";
			grd.RootTable.Columns["Weight"].TotalFormatString = "#,#";
			grd.RootTable.Columns["Amount"].TotalFormatString = "#,#";
			grd.RootTable.Columns["RateDiscountAmt"].TotalFormatString = "#,#";
			grd.RootTable.Columns["ItemDiscAmt"].TotalFormatString = "#,#";
			grd.RootTable.Columns["TotalAmount"].TotalFormatString = "#,#";
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

	private void DetailCombosInGrid()
	{
		try
		{
			grd.RootTable.Columns["PackingTypeID"].ColumnType = (ColumnType)0;
			grd.RootTable.Columns["PackingTypeID"].EditType = (EditType)5;
			grd.RootTable.Columns["PackingTypeID"].HasValueList = true;
			grd.RootTable.Columns["PackingTypeID"].LimitToList = true;
			grd.RootTable.Columns["PackingTypeID"].Caption = "Packing Type";
			grd.RootTable.Columns["PackingTypeID"].ValueList.PopulateValueList((IEnumerable)dtPackType.DefaultView, "Id", "PackTypeDesc");
			grd.RootTable.Columns["CityID"].ColumnType = (ColumnType)0;
			grd.RootTable.Columns["CityID"].EditType = (EditType)5;
			grd.RootTable.Columns["CityID"].HasValueList = true;
			grd.RootTable.Columns["CityID"].LimitToList = true;
			grd.RootTable.Columns["CityID"].Caption = "City Name";
			grd.RootTable.Columns["CityID"].ValueList.PopulateValueList((IEnumerable)dtCity.DefaultView, "Id", "CityName");
			grd.RootTable.Columns["RefPartyId"].ColumnType = (ColumnType)0;
			grd.RootTable.Columns["RefPartyId"].EditType = (EditType)5;
			grd.RootTable.Columns["RefPartyId"].HasValueList = true;
			grd.RootTable.Columns["RefPartyId"].LimitToList = true;
			grd.RootTable.Columns["RefPartyId"].Caption = "Reference Party";
			grd.RootTable.Columns["RefPartyId"].ValueList.PopulateValueList((IEnumerable)dtReferenceParty.DefaultView, "Id", "ReferencePartyName");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Expected O, but got Unknown
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r == null || (int)r.RowType != 0 || !(e.Column.Key == "Delete"))
			{
				return;
			}
			if (updateDetailIndex == -1)
			{
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					SaleOrderDetail vd = new SaleOrderDetail();
					vd.Id = Conversion.ToInt(r.Cells["Id"].Value);
					vd.RefDocId = Conversion.ToInt(r.Cells["PreOrderId"].Value);
					vd.RefDocDetailId = Conversion.ToInt(r.Cells["PreOrderDetailId"].Value);
					vd.PriceScheduleId = Conversion.ToInt(r.Cells["ScheduleId"].Value);
					vd.RateDiscount = Conversion.ToDouble(r.Cells["RateDiscount"].Value);
					vd.ItemPrice = Conversion.ToDouble(r.Cells["ItemPrice"].Value);
					vd.OrderItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					vd.OrderItemUOMId = Conversion.ToInt(r.Cells["PackUomId"].Value);
					vd.OrderItemQty = Conversion.ToDouble(r.Cells["QTY"].Value);
					vd.NetWeight = Conversion.ToDouble(r.Cells["Weight"].Value);
					vd.OrderItemRate = Conversion.ToDouble(r.Cells["Rate"].Value);
					vd.OrderItemRateUOMId = Conversion.ToInt(r.Cells["RateUomId"].Value);
					vd.Amount = Conversion.ToDouble(r.Cells["Amount"].Value);
					vd.CityArea = Conversion.ToString((object)r.Cells["CityID"].Text);
					vd.CityId = Conversion.ToInt(r.Cells["CityID"].Value);
					vd.Crop = Conversion.ToString((object)r.Cells["CropYear"].Text);
					vd.OrderRemarks = Conversion.ToString(r.Cells["Remarks"].Value);
					vd.CostCenterId = Conversion.ToInt(r.Cells["CostCenterId"].Value);
					vd.ActionTypeId = 3;
					lstRemoveRecord.Add(vd);
					table.Rows[r.RowIndex].Delete();
					r.Delete();
					grd.UpdateData();
					TotalCommissionAmount();
				}
				else
				{
					r.Delete();
				}
				if (grd.GetRows().Length == 0)
				{
					PreOrderExist = false;
					((Control)(object)CmbCustomer).Enabled = true;
					((Control)(object)CmbItem).Enabled = true;
					CmbItem.ReadOnly = false;
					DocDate.Enabled = true;
				}
				return;
			}
			throw new Exception("Reset the Detail Record First");
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
				updateDetailIndex = item.RowIndex;
				DetailPoId = Conversion.ToInt(item.Cells["Id"].Value);
				txtPriceSchId.Text = Conversion.ToString(item.Cells["ScheduleId"].Value);
				CmbItem.Value = item.Cells["ItemId"].Value.ToString();
				ItemId = Conversion.ToInt((object)item.Cells["ItemId"].Value.ToString());
				CropYear();
				cmbPackType.Value = item.Cells["PackingTypeID"].Value.ToString();
				combitempck.Value = item.Cells["PackUomId"].Value.ToString();
				txtqty.Text = item.Cells["QTY"].Value.ToString();
				txtweight.Text = item.Cells["Weight"].Value.ToString();
				txtItemPrice.Text = item.Cells["ItemPrice"].Value.ToString();
				txtPackingAddless.Text = item.Cells["PackingAddLess"].Value.ToString();
				txtPartyDiscPrct.Text = item.Cells["RateDiscount"].Value.ToString();
				txtPartDiscAmt.Text = item.Cells["RateDiscountAmt"].Value.ToString();
				txtNetRate.Text = item.Cells["Rate"].Value.ToString();
				txtRateUomId.Text = item.Cells["RateUomId"].Value.ToString();
				txtRateUom.Text = item.Cells["RateUom"].Value.ToString();
				txtamount.Text = item.Cells["Amount"].Value.ToString();
				txtItemDiscPrct.Text = item.Cells["ItemDiscPrct"].Value.ToString();
				txtItemDiscountAmt.Text = item.Cells["ItemDiscAmt"].Value.ToString();
				txtNetAmount.Text = item.Cells["TotalAmount"].Value.ToString();
				combcityarea.Value = item.Cells["CityID"].Value.ToString();
				((Control)(object)CmbCropyr).Text = item.Cells["CropYear"].Value.ToString();
				if (Conversion.ToInt(item.Cells["RefPartyId"].Value) > 0)
				{
					cmbReferenceParty.Value = Conversion.ToInt(item.Cells["RefPartyId"].Value);
				}
				txtremarksdetail.Text = item.Cells["Remarks"].Value.ToString();
				chkCommissionOnSale.Checked = Conversion.ToBool(item.Cells["CommOnSale"].Value);
				btnplus.Visible = false;
				btnUpdateDetail.Visible = true;
				btnCancelUpdateDetial.Visible = true;
				CmbItem.ReadOnly = PreOrderExist;
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
			if (FormValidationDetail())
			{
				ItemId = 0;
				table.Rows[updateDetailIndex]["Id"] = Conversion.ToInt((object)DetailPoId);
				table.Rows[updateDetailIndex]["ScheduleId"] = Conversion.ToInt((object)txtPriceSchId.Text);
				table.Rows[updateDetailIndex]["ItemId"] = Conversion.ToInt(CmbItem.Value);
				table.Rows[updateDetailIndex]["ItemCode"] = Conversion.ToString((object)((UltraDropDownBase)CmbItem).SelectedRow.Cells["ItemCode"].Text);
				table.Rows[updateDetailIndex]["Item"] = Conversion.ToString((object)((UltraDropDownBase)CmbItem).SelectedRow.Cells["ItemName"].Text);
				table.Rows[updateDetailIndex]["PackingTypeID"] = Conversion.ToInt(cmbPackType.Value);
				table.Rows[updateDetailIndex]["PackUomId"] = Conversion.ToInt(combitempck.Value);
				table.Rows[updateDetailIndex]["PackScheduleUnitId"] = Conversion.ToInt(((UltraDropDownBase)combitempck).SelectedRow.Cells[1].Value);
				table.Rows[updateDetailIndex]["PackUom"] = ((Control)(object)combitempck).Text;
				table.Rows[updateDetailIndex]["QTY"] = Conversion.ToDouble((object)txtqty.Text);
				table.Rows[updateDetailIndex]["Weight"] = Conversion.ToDouble((object)txtweight.Text);
				table.Rows[updateDetailIndex]["ItemPrice"] = Conversion.ToDouble((object)txtItemPrice.Text);
				table.Rows[updateDetailIndex]["PackingAddLess"] = Conversion.ToDouble((object)txtPackingAddless.Text);
				table.Rows[updateDetailIndex]["RateDiscount"] = Conversion.ToDouble((object)txtPartyDiscPrct.Text);
				table.Rows[updateDetailIndex]["RateDiscountAmt"] = Conversion.ToDouble((object)txtPartDiscAmt.Text);
				table.Rows[updateDetailIndex]["Rate"] = Conversion.ToDouble((object)txtNetRate.Text);
				table.Rows[updateDetailIndex]["RateUomId"] = Conversion.ToInt((object)txtRateUomId.Text);
				table.Rows[updateDetailIndex]["RateUom"] = txtRateUom.Text;
				table.Rows[updateDetailIndex]["Amount"] = Conversion.ToDouble((object)txtamount.Text);
				table.Rows[updateDetailIndex]["ItemDiscPrct"] = Conversion.ToDouble((object)txtItemDiscPrct.Text);
				table.Rows[updateDetailIndex]["ItemDiscAmt"] = Conversion.ToDouble((object)txtItemDiscountAmt.Text);
				table.Rows[updateDetailIndex]["TotalAmount"] = Conversion.ToDouble((object)txtNetAmount.Text);
				table.Rows[updateDetailIndex]["CityID"] = Conversion.ToInt(combcityarea.Value);
				table.Rows[updateDetailIndex]["CropYearID"] = CmbCropyr.Value;
				table.Rows[updateDetailIndex]["CropYear"] = ((Control)(object)CmbCropyr).Text;
				table.Rows[updateDetailIndex]["RefPartyId"] = Conversion.ToInt(cmbReferenceParty.Value);
				table.Rows[updateDetailIndex]["Remarks"] = txtremarksdetail.Text;
				table.Rows[updateDetailIndex]["CommOnSale"] = chkCommissionOnSale.Checked;
				grd.DataSource = table;
				btnplus.Visible = true;
				btnUpdateDetail.Visible = false;
				btnCancelUpdateDetial.Visible = false;
				ResetDetail();
				CmbItem.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combitem_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			bindRateUomAndItemPackUom(Conversion.ToInt(CmbItem.Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelUpdateDetial_Click(object sender, EventArgs e)
	{
		btnplus.Visible = true;
		btnUpdateDetail.Visible = false;
		btnCancelUpdateDetial.Visible = false;
		ResetDetail();
	}

	private void Insert()
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Expected O, but got Unknown
		//IL_03f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fd: Expected O, but got Unknown
		//IL_0aa1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0adb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae7: Expected O, but got Unknown
		try
		{
			if (grd.GetRows().Length == 0)
			{
				throw new Exception("Grid Record Not Found");
			}
			double BillAmount = 0.0;
			if (!FormValidation())
			{
				return;
			}
			SaleOrder po = new SaleOrder();
			if (Id > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				po.Id = Id;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			int AgentId = Conversion.ToInt(combsalesman.Value);
			double CommAmount = Conversion.ToDouble((object)txtcommamount.Text.Trim());
			double CommRate = Conversion.ToDouble((object)txtCommrate.Text);
			if ((CommAmount > 0.0 || CommRate > 0.0) && AgentId == 0)
			{
				throw new Exception("Please Select Commission Agent Required when Commission Amount or Rate is Present...");
			}
			if (Conversion.ToInt(combsalesman.Value) > 0 && (CommAmount == 0.0 || CommRate == 0.0))
			{
				if (CommAmount == 0.0)
				{
					throw new Exception("Commission Amount Required when Commission Agent is Selected...");
				}
				if (CommRate == 0.0)
				{
					throw new Exception("Commission Rate Required when Commission Agent is Selected...");
				}
			}
			po.OrganizationId = UserAccount.OrganizationId;
			po.CompanyId = UserAccount.CompanyId;
			po.BranchesId = UserAccount.BranchesId;
			po.ProjectsId = UserAccount.BranchesId;
			po.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			po.DocumentTypeId = 127;
			po.EntryUser = UserAccount.ID;
			po.ModifyUser = UserAccount.ID;
			po.DocDate = DocDate.Value;
			po.DocNo = Conversion.ToInt((object)txtdocno.Text);
			po.BranchSrNo = Conversion.ToInt((object)txtBranchSrNo.Text);
			po.OrderCatagoryId = Conversion.ToInt(combordercat.Value);
			po.CatagorySrNo = Conversion.ToInt((object)txtcatsr.Text);
			po.OrderSupCustId = Conversion.ToInt(CmbCustomer.Value);
			po.SupplierRefNo = Conversion.ToString((object)txtsupprefno.Text);
			po.RemarksHeader = Conversion.ToString((object)txtremarks.Text);
			po.PaymentTermsId = Conversion.ToInt(combpttrm.Value);
			po.OrderDueDays = Conversion.ToInt((object)txtduedays.Text);
			po.OrderDueDate = duedate.Value;
			po.OrderExpiryDate = DateTime.Now;
			po.OrderType = Conversion.ToString((object)((Control)(object)CmbOrderType).Text);
			po.OrderStatus = "Open";
			po.DeliveryTerm = Conversion.ToString((object)((Control)(object)combdeliverytrm).Text);
			po.DeliveryStartDate = deliverystartdate.Value;
			po.DeliveryDays = Conversion.ToInt((object)txtdeliverydays.Text);
			if (((UltraGridBase)combsalesman).ActiveRow != null && Conversion.ToInt(combsalesman.Value) > 0)
			{
				po.BrokerAgentSupCustId = Conversion.ToInt(combsalesman.Value);
				po.CommissionType = Conversion.ToString((object)((Control)(object)combcommtype).Text);
				po.CommRate = Conversion.ToDouble((object)txtCommrate.Text);
				po.UomScheduleIdCmRate = Conversion.ToInt((object)((Control)(object)combruom).Text);
				po.CommAmount = Conversion.ToDouble((object)txtcommamount.Text.Trim());
				po.CommissionRemarks = Conversion.ToString((object)txtcommremarks.Text);
				if (po.BrokerAgentSupCustId == po.OrderSupCustId)
				{
					BillAmount = po.CommAmount;
				}
			}
			po.SaleOrderDetailList = new List<SaleOrderDetail>();
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow r in rows)
			{
				SaleOrderDetail vd = new SaleOrderDetail();
				vd.Id = Conversion.ToInt(r.Cells["Id"].Value);
				if (vd.Id > 0)
				{
					vd.ActionTypeId = 2;
				}
				else
				{
					vd.ActionTypeId = 1;
				}
				vd.RefDocId = Conversion.ToInt(r.Cells["PreOrderId"].Value);
				vd.RefDocDetailId = Conversion.ToInt(r.Cells["PreOrderDetailId"].Value);
				vd.PriceScheduleId = Conversion.ToInt(r.Cells["ScheduleId"].Value);
				if (vd.PriceScheduleId == 0)
				{
					throw new Exception($"PriceScheduleId Field Required in Detail Grid Row No: {r.RowIndex + 1}");
				}
				vd.RateDiscount = Conversion.ToDouble(r.Cells["RateDiscount"].Value);
				vd.ItemPrice = Conversion.ToDouble(r.Cells["ItemPrice"].Value);
				if (vd.ItemPrice == 0.0)
				{
					throw new Exception($"Item Price Field Required in Detail Grid Row No: {r.RowIndex + 1}");
				}
				if (Conversion.ToInt(r.Cells["ItemId"].Value) > 0)
				{
					vd.OrderItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					if (Conversion.ToInt(r.Cells["PackingTypeID"].Value) > 0)
					{
						vd.PackingTypeID = Conversion.ToInt(r.Cells["PackingTypeID"].Value);
						if (Conversion.ToInt(r.Cells["PackUomId"].Value) > 0)
						{
							vd.OrderItemUOMId = Conversion.ToInt(r.Cells["PackUomId"].Value);
							if (Conversion.ToDouble(r.Cells["Qty"].Value) > 0.0)
							{
								vd.OrderItemQty = Conversion.ToDouble(r.Cells["Qty"].Value);
								if (Conversion.ToDouble(r.Cells["Weight"].Value) > 0.0)
								{
									vd.NetWeight = Conversion.ToDouble(r.Cells["Weight"].Value);
									if (Conversion.ToDouble(r.Cells["Rate"].Value) > 0.0)
									{
										vd.OrderItemRate = Conversion.ToDouble(r.Cells["Rate"].Value);
										vd.OrderItemRateUOMId = Conversion.ToInt(r.Cells["RateUomId"].Value);
										if (vd.OrderItemRateUOMId == 0)
										{
											throw new Exception($"RateUom Field Required in Detail Grid Row No: {r.RowIndex + 1}");
										}
										vd.PackingAddLessOnRate = Conversion.ToDouble(r.Cells["PackingAddLess"].Value);
										vd.ItemDiscount = Conversion.ToDouble(r.Cells["ItemDiscPrct"].Value);
										vd.ItemDiscountAmount = Conversion.ToDouble(r.Cells["ItemDiscAmt"].Value);
										vd.TotalAmount = Conversion.ToDouble(r.Cells["TotalAmount"].Value);
										if (Conversion.ToDouble(r.Cells["Amount"].Value) > 0.0)
										{
											vd.Amount = Conversion.ToDouble(r.Cells["Amount"].Value);
											BillAmount += vd.Amount;
											vd.CityArea = Conversion.ToString((object)r.Cells["CityID"].Text);
											vd.CityId = Conversion.ToInt(r.Cells["CityID"].Value);
											vd.Crop = Conversion.ToString((object)r.Cells["CropYear"].Text);
											vd.ReferencePartyId = Conversion.ToInt(r.Cells["RefPartyId"].Value);
											vd.OrderRemarks = Conversion.ToString(r.Cells["Remarks"].Value);
											vd.CommOnSale = Conversion.ToBool(r.Cells["CommOnSale"].Value);
											vd.CostCenterId = Conversion.ToInt(r.Cells["CostCenterId"].Value);
											po.SaleOrderDetailList.Add(vd);
											continue;
										}
										throw new Exception($"Amount Field Required in Detail Grid Row No: {r.RowIndex + 1}");
									}
									throw new Exception($"Rate Field Required in Detail Grid Row No: {r.RowIndex + 1}");
								}
								throw new Exception($"Weight Field Required in Detail Grid Row No: {r.RowIndex + 1}");
							}
							throw new Exception($"Item Qty Field Required in Detail Grid Row No: {r.RowIndex + 1}");
						}
						throw new Exception($"Item UOM Field Required in Detail Grid Row No: {r.RowIndex + 1}");
					}
					throw new Exception($"PackingType Field Required in Detail Grid Row No: {r.RowIndex + 1}");
				}
				throw new Exception($"Item Field Required in Detail Grid Row No: {r.RowIndex + 1}");
			}
			if (lstRemoveRecord.Count > 0)
			{
				for (int i = 0; i < lstRemoveRecord.Count; i++)
				{
					po.SaleOrderDetailList.Add(lstRemoveRecord[i]);
				}
			}
			po.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			po.DeleteAttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<SaleOrder>(po, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
			po.AttachmentsValues = AttachmentsValues;
			po.CustomAttachmentsValues = CustomAttachmentsValues;
			string IsExist = "";
			IsExist = SaleOrder.CheckOrderExist(po);
			if (IsExist != "" && MessageBox.Show(IsExist, "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
			{
				return;
			}
			double AvailableLimit = SaleOrder.CHECKSUPPLIERCUSTOMERLIMITS(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				SupplierCustomerId = po.OrderSupCustId,
				Amount = BillAmount,
				ActionId = 2
			});
			if (AvailableLimit > 0.0)
			{
				if (MessageBox.Show("Debit Limit of Customer Exceeds Define Debit Limit & remaining limit value is   " + Conversion.ToString((object)AvailableLimit), "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
				{
					return;
				}
			}
			else if (AvailableLimit < 0.0 && MessageBox.Show("Debit Limit of Customer already Exceeds Define Debit Limit", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
			{
				return;
			}
			int success = (po.Id = SaleOrder.Save(po));
			if (Id > 0)
			{
				MessageBox.Show("Data Update Successfully....  " + po.DocNo);
				if (AT.RemovedAttachmentListInUpdateCase.Count > 0 && AT.lst.Count == 0)
				{
					global::Architecture.BLL.DMSAttachments.RemoveByIdAndNames(Id, base.Name, 127, 0);
				}
			}
			else
			{
				MessageBox.Show("Data Save Successfully....  " + po.DocNo);
			}
			Reset();
			if (ChkBox.Checked)
			{
				CommonServices.BookOrderReports273A(success);
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
			Approved = false;
			Id = 0;
			Insert();
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
			Approved = false;
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
			if (Id == 0)
			{
				throw new Exception("Record Not Update because RecId Not Found");
			}
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public void ReadById(int ID)
	{
		try
		{
			Id = Conversion.ToInt((object)ID);
			SaleOrder po = SaleOrder.GetByID(Id);
			if (po != null && po.SaleOrderDetailList.Count > 0)
			{
				tabControl1.SelectedIndex = 0;
				DocDate.Value = po.DocDate;
				txtdocno.Text = Conversion.ToString((object)po.DocNo);
				txtBranchSrNo.Text = Conversion.ToString((object)po.BranchSrNo);
				combordercat.Value = Conversion.ToInt((object)po.OrderCatagoryId);
				txtcatsr.Text = po.CatagorySrNo.ToString();
				CmbCustomer.Value = Conversion.ToInt((object)po.OrderSupCustId);
				txtsupprefno.Text = Conversion.ToString((object)po.SupplierRefNo);
				txtremarks.Text = Conversion.ToString((object)po.RemarksHeader);
				((Control)(object)CmbOrderType).Text = Conversion.ToString((object)po.OrderType);
				combpttrm.Value = Conversion.ToInt((object)po.PaymentTermsId);
				txtduedays.Text = Conversion.ToString((object)po.OrderDueDays);
				duedate.Value = po.OrderDueDate;
				((Control)(object)combdeliverytrm).Text = Conversion.ToString((object)po.DeliveryTerm);
				deliverystartdate.Value = po.DeliveryStartDate;
				txtdeliverydays.Text = Conversion.ToString((object)po.DeliveryDays);
				combsalesman.Value = Conversion.ToInt((object)po.BrokerAgentSupCustId);
				((Control)(object)combcommtype).Text = Conversion.ToString((object)po.CommissionType);
				txtCommrate.Text = Conversion.ToString((object)po.CommRate);
				((Control)(object)combruom).Text = Conversion.ToString((object)po.UomScheduleIdCmRate);
				txtcommamount.Text = Conversion.ToString((object)po.CommAmount);
				txtcommremarks.Text = Conversion.ToString((object)po.CommissionRemarks);
				Approved = Conversion.ToBool((object)po.IsAproved);
				AttachmentsValues = po.AttachmentsValues;
				CustomAttachmentsValues = po.CustomAttachmentsValues;
				lstRemoveRecord.Clear();
				table.Clear();
				for (int i = 0; i < po.SaleOrderDetailList.Count; i++)
				{
					((Control)(object)CmbCustomer).Enabled = false;
					DocDate.Enabled = false;
					table.Rows.Add(po.SaleOrderDetailList[i].Id, po.SaleOrderDetailList[i].RefDocId, po.SaleOrderDetailList[i].RefDocDetailId, po.SaleOrderDetailList[i].RefDocNo, po.SaleOrderDetailList[i].PriceScheduleId, po.SaleOrderDetailList[i].OrderItemId, po.SaleOrderDetailList[i].ItemCodeNew, po.SaleOrderDetailList[i].ItemName, po.SaleOrderDetailList[i].CropYearId, po.SaleOrderDetailList[i].Crop, po.SaleOrderDetailList[i].PackingTypeID, po.SaleOrderDetailList[i].OrderItemUOMId, po.SaleOrderDetailList[i].PackScheduleUnitId, po.SaleOrderDetailList[i].UOMDescription, po.SaleOrderDetailList[i].OrderItemQty, po.SaleOrderDetailList[i].NetWeight, po.SaleOrderDetailList[i].ItemPrice, po.SaleOrderDetailList[i].PackingAddLessOnRate, po.SaleOrderDetailList[i].RateDiscount, po.SaleOrderDetailList[i].ItemPrice * po.SaleOrderDetailList[i].RateDiscount / 100.0, po.SaleOrderDetailList[i].OrderItemRate, po.SaleOrderDetailList[i].OrderItemRateUOMId, po.SaleOrderDetailList[i].EquivalentRate, po.SaleOrderDetailList[i].Amount, po.SaleOrderDetailList[i].ItemDiscount, po.SaleOrderDetailList[i].ItemDiscountAmount, po.SaleOrderDetailList[i].TotalAmount, po.SaleOrderDetailList[i].CityId, po.SaleOrderDetailList[i].ReferencePartyId, po.SaleOrderDetailList[i].OrderRemarks, po.SaleOrderDetailList[i].CommOnSale, po.SaleOrderDetailList[i].CostCenterId);
					PreOrderExist = Conversion.ToInt((object)po.SaleOrderDetailList[i].RefDocId) > 0;
				}
				grd.DataSource = table;
				grd.RetrieveStructure();
				grdSettings();
				FormHelper.LoadAttachmentsForObject(AT, Id, base.Name);
				btnsave.Visible = false;
				btnupdate.Visible = true;
				return;
			}
			throw new Exception("Record Not Found");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnDelete_Click(object sender, EventArgs e)
	{
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			OrderCatagoryfill();
			SupplierNameFill();
			DeliveryTerms();
			ItemDetailFill();
			cmbCityFill();
			PackingTypeFill();
			PaymentTerms();
			CommissionTypeFill();
			ReferencePartiesFill();
			DetailCombosInGrid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		Reset();
		combordercat_Leave(null, null);
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
			AttachmentsValues = "";
			CustomAttachmentsValues = "";
			btnGenerateCalculations.Visible = false;
			updateDetailIndex = -1;
			lstRemoveRecord.Clear();
			((Control)(object)CmbCustomer).Enabled = true;
			DocDate.Enabled = true;
			Approved = false;
			PreOrderExist = false;
			Id = 0;
			ItemId = 0;
			DetailPoId = 0;
			txtPriceSchId.Text = string.Empty;
			txtRateUomId.Text = string.Empty;
			txtRateUom.Text = string.Empty;
			((Control)(object)CmbCustomer).Text = string.Empty;
			((Control)(object)combsalesman).Text = string.Empty;
			txtsupprefno.Text = string.Empty;
			txtduedays.Text = string.Empty;
			txtdeliverydays.Text = string.Empty;
			txtdeliverydays.Text = string.Empty;
			((Control)(object)combcommtype).Text = string.Empty;
			txtCommrate.Text = string.Empty;
			txtcommamount.Text = string.Empty;
			txtcommremarks.Text = string.Empty;
			CmbItem.ReadOnly = false;
			CmbCustomer.Focus();
			((Control)(object)CmbItem).Text = string.Empty;
			((Control)(object)combitempck).Text = string.Empty;
			txtqty.Text = string.Empty;
			txtweight.Text = string.Empty;
			txtItemPrice.Text = string.Empty;
			txtamount.Text = string.Empty;
			txtremarks.Text = string.Empty;
			btnsave.Visible = true;
			btnupdate.Visible = false;
			btnplus.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			((UltraGridBase)combordercat).Rows[2].Activate();
			GenerateOrderCategoryNo();
			DocumentNoFill();
			BranchSrNoFill();
			table.Clear();
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
			updateDetailIndex = -1;
			chkCommissionOnSale.Checked = false;
			((Control)(object)CmbItem).Text = string.Empty;
			((Control)(object)combitempck).Text = string.Empty;
			txtqty.Text = string.Empty;
			txtweight.Text = string.Empty;
			txtItemPrice.Text = string.Empty;
			txtNetRate.Text = string.Empty;
			txtRateUom.Text = string.Empty;
			txtamount.Text = string.Empty;
			txtPackingAddless.Text = string.Empty;
			txtPartyDiscPrct.Text = string.Empty;
			txtPartDiscAmt.Text = string.Empty;
			txtItemDiscountAmt.Text = string.Empty;
			txtItemDiscPrct.Text = string.Empty;
			txtNetAmount.Text = string.Empty;
			txtremarksdetail.Text = string.Empty;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BookingOrder_FormClosing(object sender, FormClosingEventArgs e)
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

	private void txtduedays_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (txtduedays.Text.Trim() != string.Empty && txtduedays.Text != null)
			{
				duedate.Value = DateTime.Now.AddDays(Conversion.ToDouble((object)txtduedays.Text.Trim()));
			}
			else
			{
				duedate.Value = DateTime.Now;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetRegularItemDiscount()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Expected O, but got Unknown
		try
		{
			if (((UltraGridBase)CmbItem).ActiveRow != null)
			{
				DataTable dt = ItemDiscounting.GetRegularItemDiscount(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					CurrentDate = Conversion.ToDateTime((object)DocDate.Value),
					ItemId = Conversion.ToInt(CmbItem.Value),
					PriceTypeId = 6,
					MasterDiscountTypeId = 19
				});
				if (dt.Rows.Count > 0)
				{
					txtItemDiscPrct.Text = Conversion.ToString(dt.Rows[0]["DiscountValue"]);
				}
				else
				{
					txtItemDiscPrct.Text = "";
				}
			}
			else
			{
				txtItemDiscPrct.Text = "";
			}
			TotalAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetPackingAddLess()
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Expected O, but got Unknown
		try
		{
			double PackingAddless = 0.0;
			if (((UltraGridBase)combitempck).ActiveRow != null)
			{
				DataTable dt = PackingChangePriceSchedule.GetPackingAddLess(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					SupplierCustomerId = Conversion.ToInt(CmbCustomer.Value),
					ItemUomId = Conversion.ToInt(((UltraDropDownBase)combitempck).SelectedRow.Cells[1].Value),
					EffectedDate = Conversion.ToDateTime((object)DocDate.Value)
				});
				if (dt.Rows.Count > 0)
				{
					PackingAddless = Conversion.ToDouble(dt.Rows[0]["PackingAddLess"]);
					txtPackingAddless.Text = Conversion.ToString((object)PackingAddless.ToString("0,0.00"));
				}
				else
				{
					txtPackingAddless.Text = "";
				}
			}
			if (PackingAddless != 0.0)
			{
				txtPackingAddless.Text = Conversion.ToString((object)PackingAddless);
			}
			else
			{
				txtPackingAddless.Text = "";
			}
			TotalAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PackingAddLess()
	{
	}

	private void DocDate_Leave(object sender, EventArgs e)
	{
		try
		{
			GetCustomerDiscount();
			GetPackingAddLess();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtPackingAddless_TextChanged(object sender, EventArgs e)
	{
		try
		{
			decimal PackingAddless = Conversion.ToDecimal((object)txtPackingAddless.Text.Trim());
			if (PackingAddless != 0m)
			{
				if (Conversion.ToInt(cmbPackType.Value) == 2 && (Conversion.ToInt((object)((Control)(object)combitempck).Text) == 50 || Conversion.ToInt((object)((Control)(object)combitempck).Text) == 25))
				{
					txtPackingAddless.Text = Conversion.ToString((object)("-" + Math.Abs(PackingAddless)));
				}
				else
				{
					txtPackingAddless.Text = Conversion.ToString((object)PackingAddless);
				}
			}
			TotalAmount();
			TotalCommissionAmount();
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
			if (((UltraGridBase)combpttrm).ActiveRow != null)
			{
				if (Conversion.ToInt(combpttrm.Value) == 1)
				{
					txtduedays.Text = "0";
					duedate.Value = DateTime.Now;
					txtduedays.Enabled = false;
					duedate.Enabled = false;
				}
				else
				{
					txtduedays.Enabled = true;
					duedate.Enabled = true;
				}
			}
			else
			{
				txtduedays.Enabled = true;
				duedate.Enabled = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbPackType_Leave(object sender, EventArgs e)
	{
		try
		{
			GetPackingAddLess();
			CalculateWeight();
			TotalAmount();
			TotalCommissionAmount();
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
			if (((Control)(object)combitempck).Text != string.Empty && txtqty.Text != string.Empty)
			{
				double ItemPack = Conversion.ToDouble((object)((Control)(object)combitempck).Text);
				double Qty = Conversion.ToDouble((object)txtqty.Text);
				txtweight.Text = (ItemPack * Qty).ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combitempck_Leave(object sender, EventArgs e)
	{
		GetPackingAddLess();
		CalculateWeight();
		TotalAmount();
		TotalCommissionAmount();
	}

	private void txtqty_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateWeight();
			TotalAmount();
			TotalCommissionAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TotalAmount()
	{
		try
		{
			double RateUOM = 0.0;
			double PackUOM = 0.0;
			double ItemRate = 0.0;
			double NetRate = 0.0;
			double TotalAmount = 0.0;
			double DiscountAmt = 0.0;
			double RoundingFactorHalf = Math.Round(RoundingFactor / 2.0, 2);
			if (txtqty.Text != string.Empty && txtweight.Text != string.Empty && txtItemPrice.Text != string.Empty && txtRateUom.Text != string.Empty)
			{
				double num = Conversion.ToDouble((object)txtweight.Text);
				RateUOM = Conversion.ToDouble((object)txtRateUom.Text);
				PackUOM = Conversion.ToDouble((object)((Control)(object)combitempck).Text);
				ItemRate = Conversion.ToDouble((object)txtItemPrice.Text);
				double Discount = Conversion.ToDouble((object)txtPartyDiscPrct.Text);
				if (Discount > 0.0 && ItemRate > 0.0)
				{
					DiscountAmt = ItemRate * Discount / 100.0;
				}
				NetRate = ItemRate - DiscountAmt;
				if (RoundingFactor > 0.0)
				{
					double num2 = NetRate % 100.0;
					double lastTwoDigCompare = Math.Round(num2 / RoundingFactor, 0) * RoundingFactor;
					double lastTwoDigdiff = num2 - lastTwoDigCompare;
					NetRate = ((!(lastTwoDigdiff > 0.0)) ? UltimateRoundingFunctionCeiling(NetRate, RoundingFactor, 0.0) : ((!(lastTwoDigdiff >= RoundingFactorHalf)) ? UltimateRoundingFunctionFloor(NetRate, RoundingFactor, 0.0) : UltimateRoundingFunctionCeiling(NetRate, RoundingFactor, 0.0)));
				}
				double PackingAddless = Conversion.ToDouble((object)txtPackingAddless.Text);
				NetRate = (NetRate / RateUOM * PackUOM + PackingAddless) / PackUOM * RateUOM;
				txtNetRate.Text = NetRate.ToString("#,#");
				txtPartDiscAmt.Text = DiscountAmt.ToString("#,#.##");
				TotalAmount = num / RateUOM * NetRate;
				txtamount.Text = TotalAmount.ToString("#,#");
			}
			else
			{
				txtamount.Text = "0";
			}
			ItemDiscountAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemDiscountAmount()
	{
		try
		{
			double DiscountAmt = 0.0;
			double TotalAmount = 0.0;
			double ItemAmount = Conversion.ToDouble((object)txtamount.Text);
			double ItemDisPrct = Conversion.ToDouble((object)txtItemDiscPrct.Text);
			if (ItemDisPrct > 0.0 && ItemAmount > 0.0)
			{
				DiscountAmt = ItemAmount * ItemDisPrct / 100.0;
				txtItemDiscountAmt.Text = DiscountAmt.ToString("#,#");
			}
			if (DiscountAmt > 0.0)
			{
				TotalAmount = ItemAmount - DiscountAmt;
				txtNetAmount.Text = TotalAmount.ToString("#,#");
			}
			else
			{
				txtNetAmount.Text = txtamount.Text;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private double UltimateRoundingFunctionCeiling(double amountToRound, double nearstOf, double fairness)
	{
		return Math.Ceiling(amountToRound / nearstOf + fairness) * nearstOf;
	}

	private double UltimateRoundingFunctionFloor(double amountToRound, double nearstOf, double fairness)
	{
		return Math.Floor(amountToRound / nearstOf + fairness) * nearstOf;
	}

	private void TotalCommissionAmount()
	{
		try
		{
			if (!(txtCommrate.Text != string.Empty))
			{
				return;
			}
			if (((Control)(object)combcommtype).Text == "Flat")
			{
				double CommRate = Conversion.ToDouble((object)txtCommrate.Text.Trim());
				txtcommamount.Text = CommRate.ToString();
			}
			if (((Control)(object)combcommtype).Text == "Percent")
			{
				double CommRate3 = Conversion.ToDouble((object)txtCommrate.Text.Trim());
				object total = grd.GetTotal(grd.RootTable.Columns["Amount"], (AggregateFunction)2);
				if (total != null)
				{
					double CommAmt2 = Conversion.ToDouble((object)total.ToString()) * CommRate3 / 100.0;
					txtcommamount.Text = Math.Round(CommAmt2, 0, MidpointRounding.AwayFromZero).ToString("#,##0.###");
				}
			}
			if (((Control)(object)combcommtype).Text == "Comm Weight")
			{
				double CommRate2 = Conversion.ToDouble((object)txtCommrate.Text.Trim());
				double UOM = Conversion.ToDouble((object)((Control)(object)combruom).Text.Trim());
				double Weight = Conversion.ToDouble(grd.GetTotal(grd.RootTable.Columns["Weight"], (AggregateFunction)2));
				if (Weight > 0.0 && CommRate2 > 0.0 && UOM > 0.0)
				{
					double CommAmt = Weight / UOM * CommRate2;
					txtcommamount.Text = Math.Round(CommAmt, 0, MidpointRounding.AwayFromZero).ToString("#,##0.###");
				}
				else
				{
					txtcommamount.Text = 0.ToString("#,##0.###");
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combamounttype_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateWeight();
			TotalAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtRate_TextChanged(object sender, EventArgs e)
	{
		CalculateWeight();
		TotalAmount();
		TotalCommissionAmount();
	}

	private void combrateuom_SelectedIndexChanged(object sender, EventArgs e)
	{
		CalculateWeight();
		TotalAmount();
		TotalCommissionAmount();
	}

	private void txtCommrate_TextChanged(object sender, EventArgs e)
	{
		TotalCommissionAmount();
	}

	private void combcommtype_Leave(object sender, EventArgs e)
	{
		TotalCommissionAmount();
		((UltraGridBase)combruom).Rows[0].Activate();
	}

	private void combruom_Leave(object sender, EventArgs e)
	{
		TotalCommissionAmount();
	}

	private void txtdocno_TextChanged(object sender, EventArgs e)
	{
	}

	private void combrateuom_TextChanged(object sender, EventArgs e)
	{
		TotalAmount();
		TotalCommissionAmount();
	}

	private void txtweight_TextChanged(object sender, EventArgs e)
	{
		CalculateWeight();
		TotalAmount();
		TotalCommissionAmount();
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
			DataGridHistoryDetail.DataSource = null;
			drdocdate.Checked = true;
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
			HistoryBranchComboFill();
			HistoryCombosFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void HistoryBranchComboFill()
	{
		try
		{
			dtBranch.Clear();
			((UltraGridBase)cmbBranchName).DataSource = null;
			if (BranchImplemented)
			{
				dtBranch.Rows.Add(UserAccount.BranchesId, UserAccount.BranchName);
			}
			else
			{
				DataTable dt = SaleOrder.GetBranchesAllocatedToUserFromSaleOrder(UserAccount.OrganizationId, UserAccount.CompanyId, UserAccount.ID, 127);
				if (dt.Rows.Count > 0)
				{
					for (int i = 0; i < dt.Rows.Count; i++)
					{
						dtBranch.Rows.Add(dt.Rows[i]["BranchId"], dt.Rows[i]["BranchName"]);
					}
				}
			}
			if (dtBranch.Rows.Count > 0)
			{
				DDL.BindDDL(dtBranch, cmbBranchName, "Id", "BranchName", "Branch Name", ZeroIndex: true);
				UltraGridColumn obj = ((UltraGridBase)cmbBranchName).DisplayLayout.Bands[0].Columns.Add();
				((KeyedSubObjectBase)obj).Key = "Selected";
				((HeaderBase)obj.Header).Caption = string.Empty;
				obj.Header.CheckBoxVisibility = (HeaderCheckBoxVisibility)2;
				obj.DataType = typeof(bool);
				((HeaderBase)obj.Header).VisiblePosition = 0;
				cmbBranchName.CheckedListSettings.CheckStateMember = "Selected";
				((EditorCheckedListSettings)cmbBranchName.CheckedListSettings).EditorValueSource = (EditorWithComboValueSource)1;
				((EditorCheckedListSettings)cmbBranchName.CheckedListSettings).ListSeparator = ",";
				((EditorCheckedListSettings)cmbBranchName.CheckedListSettings).ItemCheckArea = (ItemCheckArea)1;
				((UltraGridBase)cmbBranchName).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				((Control)(object)cmbBranchName).Text = UserAccount.BranchName.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryCombosFill()
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		try
		{
			DataTable dtCustomer = new DataTable();
			dtCustomer.Columns.Add("Id", typeof(int));
			dtCustomer.Columns.Add("Customer", typeof(string));
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.AppId = UserAccount.AppId;
			obj.UserId = UserAccount.ID;
			obj.DocumentTypeIds = "127";
			string BranchIds = "";
			if (Conversion.ToString((object)((Control)(object)cmbBranchName).Text) != string.Empty && Conversion.ToString((object)((Control)(object)cmbBranchName).Text) != "0")
			{
				string[] array = (Conversion.ToString((object)((Control)(object)cmbBranchName).Text) + ",").Split(',');
				foreach (string author in array)
				{
					DataRow[] dr = dtBranch.Select("BranchName='" + Conversion.ToString((object)author) + "'");
					if (dr.Length != 0)
					{
						BranchIds = BranchIds + "," + Conversion.ToString(dr[0].ItemArray[0]);
					}
				}
				obj.BranchesIds = BranchIds;
				DataTable dt = SaleOrder.GetDataForDropDownFromSaleOrder(obj);
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
				return;
			}
			cmbBranchName.Focus();
			throw new Exception("Select branch first");
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

	private void gridhistoryfill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters reports = new ReportsParameters();
			reports.OrganizationId = UserAccount.OrganizationId;
			reports.CompanyId = UserAccount.CompanyId;
			reports.DocumentTypeId = 127;
			reports.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			reports.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			if (!reports.CanViewAllRecord)
			{
				reports.EntryUser = UserAccount.ID;
			}
			if (drdocdate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					reports.FromDate = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					reports.ToDate = ToDateHistory.Value;
				}
			}
			else if (rdentrydate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					reports.EntryDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					reports.EntryDateTo = ToDateHistory.Value;
				}
			}
			else if (rdmodifydate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					reports.ModifyDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					reports.ModifyDateTo = ToDateHistory.Value;
				}
			}
			else if (rdapproveddate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					reports.ApprovedDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					reports.ApprovedDateTo = ToDateHistory.Value;
				}
			}
			reports.FromDocNo = Conversion.ToInt((object)FromDocNoHistory.Text);
			reports.ToDocNo = Conversion.ToInt((object)ToDocNoHistory.Text);
			reports.SupplierCustomerId = Conversion.ToInt(CmbCustomerHistory.Value);
			string BranchIds = "";
			if (Conversion.ToString((object)((Control)(object)cmbBranchName).Text) != string.Empty)
			{
				string[] array = (Conversion.ToString((object)((Control)(object)cmbBranchName).Text) + ",").Split(',');
				foreach (string author in array)
				{
					DataRow[] dr = dtBranch.Select("BranchName='" + Conversion.ToString((object)author) + "'");
					if (dr.Length != 0)
					{
						BranchIds = BranchIds + "," + Conversion.ToString(dr[0].ItemArray[0]);
					}
				}
				reports.BranchesIds = BranchIds;
				DataTable dthistoryfill = SaleOrder.GetHisoty(reports);
				if (dthistoryfill.Rows.Count > 0)
				{
					DataTable dt = new DataTable();
					dt.Columns.Add("Id");
					dt.Columns.Add("BranchName", typeof(string));
					dt.Columns.Add("BranchSrNo", typeof(int));
					dt.Columns.Add("DocNo");
					dt.Columns.Add("DocDate");
					dt.Columns.Add("CustomerName");
					dt.Columns.Add("DueDate");
					dt.Columns.Add("OrderExpiryDate");
					dt.Columns.Add("PaymentTerm");
					dt.Columns.Add("DeliveryTerm");
					dt.Columns.Add("OrderStatus");
					dt.Columns.Add("UserName");
					dt.Columns.Add("EntryDate");
					dt.Columns.Add("ModifyUser");
					dt.Columns.Add("ModifyDate");
					dt.Columns.Add("ApprovedUser");
					dt.Columns.Add("ApprovedDate");
					dt.Columns.Add("NoOfAttachments");
					for (int i = 0; i < dthistoryfill.Rows.Count; i++)
					{
						dt.Rows.Add(dthistoryfill.Rows[i]["Id"], Conversion.ToString(dthistoryfill.Rows[i]["BranchName"]), Conversion.ToInt(dthistoryfill.Rows[i]["BranchSrNo"]), dthistoryfill.Rows[i]["DocNo"], dthistoryfill.Rows[i]["DocDate"], dthistoryfill.Rows[i]["CustomerName"], dthistoryfill.Rows[i]["DueDate"], dthistoryfill.Rows[i]["OrderExpiryDate"], dthistoryfill.Rows[i]["TermsDescription"], dthistoryfill.Rows[i]["DeliveryTerm"], dthistoryfill.Rows[i]["OrderStatus"], dthistoryfill.Rows[i]["UserName"], dthistoryfill.Rows[i]["EntryDate"], dthistoryfill.Rows[i]["ModifyUserName"], dthistoryfill.Rows[i]["ModifyDate"], dthistoryfill.Rows[i]["ApprovedUser"], dthistoryfill.Rows[i]["PostDate"], dthistoryfill.Rows[i]["NoOfAttachments"]);
					}
					grdhistory.DataSource = dt;
					grdhistory.RetrieveStructure();
					HistoryGridSettings();
				}
				else
				{
					grdhistory.ClearStructure();
					DataGridHistoryDetail.ClearStructure();
				}
				return;
			}
			cmbBranchName.Focus();
			throw new Exception("Select branch first");
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
			grdhistory.AllowEdit = (InheritableBoolean)2;
			grdhistory.AllowDelete = (InheritableBoolean)2;
			grdhistory.RootTable.Columns["Id"].Visible = false;
			grdhistory.RootTable.Columns["BranchName"].Visible = !BranchImplemented;
			grdhistory.RootTable.Columns["BranchSrNo"].Visible = !BranchImplemented;
			grdhistory.RootTable.Columns["EntryDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			grdhistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			grdhistory.RootTable.Columns["ApprovedDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			grdhistory.RootTable.Columns.Add("Edit");
			grdhistory.RootTable.Columns["Edit"].Caption = "Edit";
			grdhistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdhistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdhistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdhistory.RootTable.Columns["Edit"].Width = 50;
			grdhistory.RootTable.Columns.Add("Print");
			grdhistory.RootTable.Columns["Print"].Caption = "Print";
			grdhistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdhistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			grdhistory.RootTable.Columns["Print"].ButtonText = "Print";
			grdhistory.RootTable.Columns["Print"].Width = 50;
			grdhistory.RootTable.Columns["Edit"].Position = 0;
			grdhistory.RootTable.Columns["Print"].Position = 1;
			grdhistory.FrozenColumns = 2;
			grdhistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			CommonServices.GridAutoAdjustmentNew(grdhistory);
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_DoubleClick(object sender, EventArgs e)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdhistory.CurrentRow;
			if (r != null && r.Cells.Count > 0 && (int)r.RowType == 0)
			{
				int CuurID = Conversion.ToInt(r.Cells["Id"].Value);
				Id = CuurID;
				ReadById(Id);
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
			if (item != null && (int)item.RowType == 0)
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(item.Cells["Id"].Value), 127);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdhistory.CurrentRow;
			if (r != null && r.Cells.Count > 0 && (int)r.RowType == 0)
			{
				int CuurID = Conversion.ToInt(r.Cells["Id"].Value);
				if (e.Column.Key == "Edit")
				{
					Reset();
					Id = CuurID;
					ReadById(Id);
				}
				if (e.Column.Key == "Print")
				{
					CommonServices.BookOrderReports273A(CuurID);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdhistory.CurrentRow;
			if (r != null && r.Cells.Count > 0 && (int)r.RowType == 0)
			{
				GridDetailBind(Conversion.ToInt(r.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridDetailBind(int Id)
	{
		try
		{
			SaleOrder po = SaleOrder.GetByID(Id);
			if (po != null)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("PreOrder");
				dt.Columns.Add("ItemCode");
				dt.Columns.Add("ItemName");
				dt.Columns.Add("CropYear");
				dt.Columns.Add("PackUom");
				dt.Columns.Add("QTY", typeof(double));
				dt.Columns.Add("Weight", typeof(double));
				dt.Columns.Add("ItemPrice");
				dt.Columns.Add("PackingAddLess");
				dt.Columns.Add("PartyDiscount");
				dt.Columns.Add("NetRate");
				dt.Columns.Add("RateUOM");
				dt.Columns.Add("Amount", typeof(double));
				dt.Columns.Add("ItemDiscPrct", typeof(double));
				dt.Columns.Add("ItemDiscAmount", typeof(double));
				dt.Columns.Add("TotalAmount", typeof(double));
				dt.Columns.Add("City");
				dt.Columns.Add("ReferenceParty");
				dt.Columns.Add("Remarks");
				dt.Columns.Add("CommOnSale");
				for (int i = 0; i < po.SaleOrderDetailList.Count; i++)
				{
					dt.Rows.Add(po.SaleOrderDetailList[i].RefDocNo, po.SaleOrderDetailList[i].ItemCodeNew, po.SaleOrderDetailList[i].ItemName, po.SaleOrderDetailList[i].Crop, po.SaleOrderDetailList[i].UOMDescription, po.SaleOrderDetailList[i].OrderItemQty, po.SaleOrderDetailList[i].NetWeight, po.SaleOrderDetailList[i].ItemPrice, po.SaleOrderDetailList[i].PackingAddLessOnRate, po.SaleOrderDetailList[i].RateDiscount, po.SaleOrderDetailList[i].OrderItemRate, po.SaleOrderDetailList[i].EquivalentRate, po.SaleOrderDetailList[i].Amount, po.SaleOrderDetailList[i].ItemDiscount, po.SaleOrderDetailList[i].ItemDiscountAmount, po.SaleOrderDetailList[i].TotalAmount, po.SaleOrderDetailList[i].CityArea, po.SaleOrderDetailList[i].ReferencePartyName, po.SaleOrderDetailList[i].OrderRemarks, po.SaleOrderDetailList[i].CommOnSale);
				}
				DataGridHistoryDetail.DataSource = dt;
				DataGridHistoryDetail.RetrieveStructure();
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
		try
		{
			DataGridHistoryDetail.AllowDelete = (InheritableBoolean)2;
			DataGridHistoryDetail.AllowEdit = (InheritableBoolean)2;
			DataGridHistoryDetail.RootTable.Columns["PackingAddLess"].Caption = "Packing Exp/Disc";
			DataGridHistoryDetail.RootTable.Columns["TotalAmount"].Caption = "NetAmount";
			DataGridHistoryDetail.RootTable.Columns["City"].Caption = "CityName";
			DataGridHistoryDetail.RootTable.Columns["QTY"].AggregateFunction = (AggregateFunction)2;
			DataGridHistoryDetail.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistoryDetail.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			DataGridHistoryDetail.RootTable.Columns["ItemDiscAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistoryDetail.RootTable.Columns["TotalAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistoryDetail.RootTable.Columns["QTY"].TextAlignment = (TextAlignment)3;
			DataGridHistoryDetail.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			DataGridHistoryDetail.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			DataGridHistoryDetail.RootTable.Columns["ItemDiscAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistoryDetail.RootTable.Columns["TotalAmount"].TextAlignment = (TextAlignment)3;
			DataGridHistoryDetail.RootTable.Columns["QTY"].FormatString = "0,0";
			DataGridHistoryDetail.RootTable.Columns["Amount"].FormatString = "0,0";
			DataGridHistoryDetail.RootTable.Columns["Weight"].FormatString = "0,0";
			DataGridHistoryDetail.RootTable.Columns["ItemDiscAmount"].FormatString = "0,0";
			DataGridHistoryDetail.RootTable.Columns["TotalAmount"].FormatString = "0,0";
			DataGridHistoryDetail.RootTable.Columns["QTY"].TotalFormatString = "0,0";
			DataGridHistoryDetail.RootTable.Columns["Amount"].TotalFormatString = "0,0";
			DataGridHistoryDetail.RootTable.Columns["Weight"].TotalFormatString = "0,0";
			DataGridHistoryDetail.RootTable.Columns["ItemDiscAmount"].TotalFormatString = "0,0";
			DataGridHistoryDetail.RootTable.Columns["TotalAmount"].TotalFormatString = "0,0";
			CommonServices.GridAutoAdjustmentNew(DataGridHistoryDetail);
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
			CommonServices.BookOrderReports273A(Id);
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
			MessageBox.Show(ex.Message);
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
			MessageBox.Show(ex.Message);
		}
	}

	private void btnattachment_Click(object sender, EventArgs e)
	{
		AT.Show();
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

	private void txtCommrate_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtdeliverydays_KeyPress(object sender, KeyPressEventArgs e)
	{
		e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
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

	private void BtnPackingPriceForm_Click(object sender, EventArgs e)
	{
		try
		{
			PackingChangePriceSchedule packingChangePriceSchedule = new PackingChangePriceSchedule(UserAccount);
			packingChangePriceSchedule.ShowDialog();
			packingChangePriceSchedule.BringToFront();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnLoader_Click(object sender, EventArgs e)
	{
		try
		{
			if (grd.GetRows().Count() > 0)
			{
				GridEXRow item = grd.CurrentRow;
				if (item != null && Conversion.ToInt(item.Cells["PreOrderId"].Value) == 0)
				{
					MessageBox.Show("You cannot load Order because a record exists without Order in the grid");
					return;
				}
			}
			LoadTransactions = new LoadPreBookingOrder(UserAccount);
			LoadTransactions.ShowDialog();
			LoadTransactions.BringToFront();
			LoadInGridDetail();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadInGridDetail()
	{
		try
		{
			DataTable dtData = LoadTransactions.dtLoader;
			if (dtData.Rows.Count <= 0)
			{
				return;
			}
			PreOrderExist = true;
			DocDate.Enabled = false;
			if (grd.GetRows().Count() > 0 && Conversion.ToInt(CmbCustomer.Value) > 0 && Conversion.ToInt(CmbCustomer.Value) != Conversion.ToInt(dtData.Rows[0]["OrderSupCustId"]))
			{
				MessageBox.Show("Data against another Customer Already Exist in Detail");
				return;
			}
			((Control)(object)CmbCustomer).Enabled = false;
			CmbCustomer.Value = Conversion.ToInt(dtData.Rows[0]["OrderSupCustId"]);
			bool flag = false;
			for (int i = 0; i < dtData.Rows.Count; i++)
			{
				if (grd.GetRows().Length != 0)
				{
					GridEXRow[] rows = grd.GetRows();
					for (int j = 0; j < rows.Length; j++)
					{
						if (Conversion.ToInt(rows[j].Cells["PreOrderDetailId"].Value) == Conversion.ToInt(dtData.Rows[i]["DetailId"]))
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					table.Rows.Add(0, dtData.Rows[i]["PreBookingOrderId"], dtData.Rows[i]["DetailId"], dtData.Rows[i]["DocNo"], 0, dtData.Rows[i]["OrderItemId"], dtData.Rows[i]["ItemCode"], dtData.Rows[i]["ItemName"], dtData.Rows[i]["CropYearId"], dtData.Rows[i]["CropYear"], 0, dtData.Rows[i]["OrderItemUOMId"], dtData.Rows[i]["ScheduleUnitId"], dtData.Rows[i]["PackEquivalent"], dtData.Rows[i]["BalQty"], dtData.Rows[i]["BalWeight"], dtData.Rows[i]["OrderItemRate"], 0, 0, 0, dtData.Rows[i]["OrderItemRate"], dtData.Rows[i]["OrderItemRateUOMId"], dtData.Rows[i]["RateEquivalent"], dtData.Rows[i]["BalAmount"], 0, 0, dtData.Rows[i]["BalAmount"], 0, 0, "", false, dtData.Rows[i]["CostCenterId"]);
				}
				flag = false;
			}
			grd.DataSource = table;
			grd.RetrieveStructure();
			grdSettings();
			btnGenerateCalculations.Visible = true;
			UpdateGridDataAgainstPreOrder();
			TotalCommissionAmount();
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
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
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
			if (tabControl1.SelectedIndex == 0)
			{
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnnew_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.S && btnsave.Visible && btnsave.Enabled)
				{
					btnsave_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefresh_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.U && btnupdate.Visible && btnupdate.Enabled)
				{
					btnupdate_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					DocDate.Focus();
				}
				if (e.Control && e.KeyCode == Keys.F10)
				{
					btnattachment_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grd).Focus();
				}
				if (e.Control && e.KeyCode == Keys.Up)
				{
					CmbItem.Focus();
				}
				if (e.Control && e.KeyCode == Keys.P && formright.DoHavePrintRights)
				{
					btnprint_Click(null, null);
				}
				if (e.Alt && (e.KeyCode == Keys.D1 || e.KeyCode == Keys.NumPad1) && formright.DoHavePrintRights)
				{
					btnprint_Click(null, null);
				}
			}
			else
			{
				if (e.Control && e.KeyCode == Keys.S)
				{
					btnShowHistory_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnResetHistory_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefreshHistory_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					FromDateHistory.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grdhistory).Focus();
				}
				if (e.Control && e.KeyCode == Keys.Up)
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
			dt.Rows.Add("Ctrl+S", "For Save in Form Tab and For Show History in History Tab");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+P", "For Print 273A");
			dt.Rows.Add("Alt+1", "For Print 273A");
			dt.Rows.Add("Ctrl+F5", "For Focus on Doc Date");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid when focus in form tab and for focus on history grid when in history tab");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus on Item in Detail Grid when in Form tab and For focus on FromDate in history tab");
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
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Expected O, but got Unknown
		//IL_041e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0424: Expected O, but got Unknown
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			GridEXRow r = grd.CurrentRow;
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				grd_DoubleClick(null, null);
			}
			else if (e.Control && e.KeyCode == Keys.Space)
			{
				if (!(grd.CurrentColumn.Key == "Delete"))
				{
					return;
				}
				if (updateDetailIndex != -1)
				{
					throw new Exception("Please Reset the Detail First...");
				}
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					SaleOrderDetail vd = new SaleOrderDetail();
					vd.Id = Conversion.ToInt(r.Cells["Id"].Value);
					vd.RefDocId = Conversion.ToInt(r.Cells["PreOrderId"].Value);
					vd.RefDocDetailId = Conversion.ToInt(r.Cells["PreOrderDetailId"].Value);
					vd.PriceScheduleId = Conversion.ToInt(r.Cells["ScheduleId"].Value);
					vd.RateDiscount = Conversion.ToDouble(r.Cells["RateDiscount"].Value);
					vd.ItemPrice = Conversion.ToDouble(r.Cells["ItemPrice"].Value);
					vd.OrderItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					vd.OrderItemUOMId = Conversion.ToInt(r.Cells["PackUomId"].Value);
					vd.OrderItemQty = Conversion.ToDouble(r.Cells["QTY"].Value);
					vd.NetWeight = Conversion.ToDouble(r.Cells["Weight"].Value);
					vd.OrderItemRate = Conversion.ToDouble(r.Cells["Rate"].Value);
					vd.OrderItemRateUOMId = Conversion.ToInt(r.Cells["RateUomId"].Value);
					vd.Amount = Conversion.ToDouble(r.Cells["Amount"].Value);
					vd.CityArea = Conversion.ToString((object)r.Cells["CityID"].Text);
					vd.CityId = Conversion.ToInt(r.Cells["CityID"].Value);
					vd.Crop = Conversion.ToString((object)r.Cells["CropYear"].Text);
					vd.OrderRemarks = Conversion.ToString(r.Cells["Remarks"].Value);
					vd.CostCenterId = Conversion.ToInt(r.Cells["CostCenterId"].Value);
					vd.ActionTypeId = 3;
					lstRemoveRecord.Add(vd);
					table.Rows[r.RowIndex].Delete();
					r.Delete();
					grd.UpdateData();
					TotalCommissionAmount();
				}
				else
				{
					r.Delete();
				}
				if (grd.GetRows().Length == 0)
				{
					PreOrderExist = false;
					((Control)(object)CmbCustomer).Enabled = true;
					((Control)(object)CmbItem).Enabled = true;
					CmbItem.ReadOnly = false;
					DocDate.Enabled = true;
				}
			}
			else
			{
				if (!e.Control || !e.Shift || e.KeyCode != Keys.Delete)
				{
					return;
				}
				if (updateDetailIndex != -1)
				{
					throw new Exception("Please Reset the Detail First...");
				}
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					SaleOrderDetail vd2 = new SaleOrderDetail();
					vd2.Id = Conversion.ToInt(r.Cells["Id"].Value);
					vd2.RefDocId = Conversion.ToInt(r.Cells["PreOrderId"].Value);
					vd2.RefDocDetailId = Conversion.ToInt(r.Cells["PreOrderDetailId"].Value);
					vd2.PriceScheduleId = Conversion.ToInt(r.Cells["ScheduleId"].Value);
					vd2.RateDiscount = Conversion.ToDouble(r.Cells["RateDiscount"].Value);
					vd2.ItemPrice = Conversion.ToDouble(r.Cells["ItemPrice"].Value);
					vd2.OrderItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					vd2.OrderItemUOMId = Conversion.ToInt(r.Cells["PackUomId"].Value);
					vd2.OrderItemQty = Conversion.ToDouble(r.Cells["QTY"].Value);
					vd2.NetWeight = Conversion.ToDouble(r.Cells["Weight"].Value);
					vd2.OrderItemRate = Conversion.ToDouble(r.Cells["Rate"].Value);
					vd2.OrderItemRateUOMId = Conversion.ToInt(r.Cells["RateUomId"].Value);
					vd2.Amount = Conversion.ToDouble(r.Cells["Amount"].Value);
					vd2.CityArea = Conversion.ToString((object)r.Cells["CityID"].Text);
					vd2.CityId = Conversion.ToInt(r.Cells["CityID"].Value);
					vd2.Crop = Conversion.ToString((object)r.Cells["CropYear"].Text);
					vd2.OrderRemarks = Conversion.ToString(r.Cells["Remarks"].Value);
					vd2.CostCenterId = Conversion.ToInt(r.Cells["CostCenterId"].Value);
					vd2.ActionTypeId = 3;
					lstRemoveRecord.Add(vd2);
					table.Rows[r.RowIndex].Delete();
					r.Delete();
					grd.UpdateData();
					TotalCommissionAmount();
				}
				else
				{
					r.Delete();
				}
				if (grd.GetRows().Length == 0)
				{
					PreOrderExist = false;
					((Control)(object)CmbCustomer).Enabled = true;
					((Control)(object)CmbItem).Enabled = true;
					CmbItem.ReadOnly = false;
					DocDate.Enabled = true;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			GridEXRow item = grdhistory.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			int CuurID = Conversion.ToInt(item.Cells["Id"].Value);
			if (e.Control && e.KeyCode == Keys.Return)
			{
				Reset();
				ReadById(CuurID);
			}
			else if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdhistory.CurrentColumn.Key == "Edit")
				{
					Reset();
					ReadById(CuurID);
				}
				else if (grdhistory.CurrentColumn.Key == "Print")
				{
					CommonServices.BookOrderReports273A(CuurID);
				}
				else if (grdhistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(CuurID, 127);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbBranchName_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)cmbBranchName).ActiveRow != null)
			{
				HistoryCombosFill();
				return;
			}
			((Control)(object)CmbCustomerHistory).Text = string.Empty;
			((UltraGridBase)CmbCustomerHistory).DataSource = null;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbCropyr_Leave(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Expected O, but got Unknown
		try
		{
			DataTable dt = ItemPricingSchedule.GetItemRateAndUomIdByEffectedDate(new ItemPricingSchedule
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				PriceTypeId = 6,
				ItemId = Conversion.ToInt(CmbItem.Value),
				CropYearId = Conversion.ToInt(CmbCropyr.Value),
				EffectedDate = Conversion.ToDateTime((object)DocDate.Value)
			});
			if (dt.Rows.Count > 0)
			{
				GetCustomerDiscount();
				txtPriceSchId.Text = Conversion.ToString(dt.Rows[0]["Id"]);
				txtRateUomId.Text = Conversion.ToString(dt.Rows[0]["RateUomId"]);
				double ItemRate = Conversion.ToDouble(dt.Rows[0]["ItemPrice"]);
				txtItemPrice.Text = ItemRate.ToString("#,#");
				txtRateUom.Text = Conversion.ToString(dt.Rows[0]["PackingUom"]);
			}
			else
			{
				txtPriceSchId.Text = "0";
				txtRateUomId.Text = "0";
				txtItemPrice.Text = "0";
				txtRateUom.Text = "0";
				txtNetRate.Text = "0";
				txtPartyDiscPrct.Text = "0";
			}
			GetRegularItemDiscount();
			chkCommissionOnSale.Checked = Conversion.ToBool((object)CommonServices.CheckCommissionOnSaleByItem(Conversion.ToInt(CmbItem.Value)));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private double GetPackingAddLessAgainstPackUom(int PackUomId)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		try
		{
			double PackingAddless = 0.0;
			DataTable dt = PackingChangePriceSchedule.GetPackingAddLess(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				SupplierCustomerId = Conversion.ToInt(CmbCustomer.Value),
				ItemUomId = PackUomId,
				EffectedDate = Conversion.ToDateTime((object)DocDate.Value)
			});
			if (dt.Rows.Count > 0)
			{
				return Conversion.ToDouble(dt.Rows[0]["PackingAddLess"]);
			}
			return 0.0;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private ItemInfo GetRateAgainstItemCrop(int ItemId, int CropYearId)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Expected O, but got Unknown
		try
		{
			ItemInfo info = new ItemInfo();
			DataTable dt = ItemPricingSchedule.GetItemRateAndUomIdByEffectedDate(new ItemPricingSchedule
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				PriceTypeId = 6,
				ItemId = ItemId,
				CropYearId = CropYearId,
				EffectedDate = Conversion.ToDateTime((object)DocDate.Value)
			});
			if (dt.Rows.Count > 0)
			{
				info.ScheduleId = Conversion.ToInt(dt.Rows[0]["Id"]);
				info.RateUomId = Conversion.ToInt(dt.Rows[0]["RateUomId"]);
				double ItemRate = Conversion.ToDouble(dt.Rows[0]["ItemPrice"]);
				info.ItemPrice = ItemRate;
				info.RateUom = Conversion.ToDouble(dt.Rows[0]["PackingUom"]);
			}
			return (dt.Rows.Count > 0) ? info : null;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private double GetCustomerDiscountAgainstItemId(int ItemId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		try
		{
			return CustomerDiscountPolicy.GetCustomerDiscountByEffectiveDate(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				SupplierCustomerId = Conversion.ToInt(CmbCustomer.Value),
				EffectedDate = Conversion.ToDateTime((object)DocDate.Value),
				ItemId = ItemId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private double GetRegularItemDiscountAgainstItem(int ItemId)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Expected O, but got Unknown
		try
		{
			double Discount = 0.0;
			DataTable dt = ItemDiscounting.GetRegularItemDiscount(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				CurrentDate = Conversion.ToDateTime((object)DocDate.Value),
				ItemId = ItemId,
				PriceTypeId = 6,
				MasterDiscountTypeId = 19
			});
			if (dt.Rows.Count > 0)
			{
				Discount = Conversion.ToDouble(dt.Rows[0]["DiscountValue"]);
			}
			return Discount;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void UpdateGridDataAgainstPreOrder()
	{
		try
		{
			if (grd.GetRows().Count() <= 0)
			{
				return;
			}
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow row in rows)
			{
				double NetRate = 0.0;
				double ItemAmount = 0.0;
				double DiscountAmount = 0.0;
				double ItemDiscountAmount = 0.0;
				int ScheduleId = 0;
				int RateUomId = 0;
				double ItemRate = 0.0;
				double RateUOM = 0.0;
				int ItemId = Conversion.ToInt(row.Cells["ItemId"].Value);
				int CropYearId = Conversion.ToInt(row.Cells["CropYearID"].Value);
				int PackScheduleUnitId = Conversion.ToInt(row.Cells["PackScheduleUnitId"].Value);
				double Weight = Conversion.ToDouble(row.Cells["Weight"].Value);
				double PackUOM = Conversion.ToDouble(row.Cells["PackUom"].Value);
				double PackingAddLess = GetPackingAddLessAgainstPackUom(PackScheduleUnitId);
				double PartyDiscount = GetCustomerDiscountAgainstItemId(ItemId);
				double ItemDisPrct = GetRegularItemDiscountAgainstItem(ItemId);
				ItemInfo info = new ItemInfo();
				info = GetRateAgainstItemCrop(ItemId, CropYearId);
				if (info != null)
				{
					ScheduleId = info.ScheduleId;
					RateUomId = info.RateUomId;
					RateUOM = info.RateUom;
					ItemRate = info.ItemPrice;
				}
				else
				{
					ScheduleId = Conversion.ToInt(row.Cells["ScheduleId"].Value);
					ItemRate = Conversion.ToDouble(row.Cells["ItemPrice"].Value);
					RateUOM = Conversion.ToDouble(row.Cells["RateUom"].Value);
					RateUomId = Conversion.ToInt(row.Cells["RateUomId"].Value);
				}
				double RoundingFactorHalf = Math.Round(RoundingFactor / 2.0, 2);
				if (PartyDiscount > 0.0 && ItemRate > 0.0)
				{
					DiscountAmount = ItemRate * PartyDiscount / 100.0;
				}
				NetRate = ItemRate - DiscountAmount;
				if (RoundingFactor > 0.0)
				{
					double num = NetRate % 100.0;
					double lastTwoDigCompare = Math.Round(num / RoundingFactor, 0) * RoundingFactor;
					double lastTwoDigdiff = num - lastTwoDigCompare;
					NetRate = ((!(lastTwoDigdiff > 0.0)) ? UltimateRoundingFunctionCeiling(NetRate, RoundingFactor, 0.0) : ((!(lastTwoDigdiff >= RoundingFactorHalf)) ? UltimateRoundingFunctionFloor(NetRate, RoundingFactor, 0.0) : UltimateRoundingFunctionCeiling(NetRate, RoundingFactor, 0.0)));
				}
				NetRate = (NetRate / RateUOM * PackUOM + PackingAddLess) / PackUOM * RateUOM;
				ItemAmount = Weight / RateUOM * NetRate;
				double NetAmount = 0.0;
				if (ItemDisPrct > 0.0 && ItemAmount > 0.0)
				{
					ItemDiscountAmount = ItemAmount * ItemDisPrct / 100.0;
				}
				NetAmount = ((!(ItemDiscountAmount > 0.0)) ? ItemAmount : (ItemAmount - ItemDiscountAmount));
				row.BeginEdit();
				row.Cells["ScheduleId"].Value = ScheduleId;
				row.Cells["ItemPrice"].Value = ItemRate;
				row.Cells["RateUomId"].Value = RateUomId;
				row.Cells["RateUom"].Value = RateUOM;
				row.Cells["PackingAddLess"].Value = PackingAddLess;
				row.Cells["RateDiscount"].Value = PartyDiscount;
				row.Cells["RateDiscountAmt"].Value = DiscountAmount;
				row.Cells["Rate"].Value = NetRate;
				row.Cells["Amount"].Value = ItemAmount;
				row.Cells["ItemDiscPrct"].Value = ItemDisPrct;
				row.Cells["ItemDiscAmt"].Value = ItemDiscountAmount;
				row.Cells["TotalAmount"].Value = NetAmount;
				row.Cells["CommOnSale"].Value = Conversion.ToBool((object)CommonServices.CheckCommissionOnSaleByItem(ItemId));
				row.EndEdit();
			}
			grd.UpdateData();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnGenerateCalculations_Click(object sender, EventArgs e)
	{
		try
		{
			UpdateGridDataAgainstPreOrder();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grd.UpdateData();
			GridEXRow row = grd.CurrentRow;
			if (row != null && (int)row.RowType == 0 && e.Column.Key == "QTY")
			{
				double PackUom = Conversion.ToDouble(row.Cells["PackUom"].Value);
				double Weight = Conversion.ToDouble(row.Cells["QTY"].Value) * PackUom;
				double NetRate = Conversion.ToDouble(row.Cells["Rate"].Value);
				double RateUOM = Conversion.ToDouble(row.Cells["RateUom"].Value);
				double ItemDisPrct = Conversion.ToDouble(row.Cells["ItemDiscPrct"].Value);
				double ItemAmount = Weight / RateUOM * NetRate;
				double ItemDiscountAmount = 0.0;
				double NetAmount = 0.0;
				if (ItemDisPrct > 0.0 && ItemAmount > 0.0)
				{
					ItemDiscountAmount = ItemAmount * ItemDisPrct / 100.0;
				}
				NetAmount = ((!(ItemDiscountAmount > 0.0)) ? ItemAmount : (ItemAmount - ItemDiscountAmount));
				row.BeginEdit();
				row.Cells["Weight"].Value = Weight;
				row.Cells["Amount"].Value = ItemAmount;
				row.Cells["ItemDiscAmt"].Value = ItemDiscountAmount;
				row.Cells["TotalAmount"].Value = NetAmount;
				row.EndEdit();
			}
			grd.UpdateData();
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
		//IL_0584: Unknown result type (might be due to invalid IL or missing references)
		//IL_058e: Expected O, but got Unknown
		//IL_05c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d0: Expected O, but got Unknown
		//IL_05e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f1: Expected O, but got Unknown
		//IL_0676: Unknown result type (might be due to invalid IL or missing references)
		//IL_0680: Expected O, but got Unknown
		//IL_06e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ee: Expected O, but got Unknown
		//IL_06fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0704: Expected O, but got Unknown
		//IL_0705: Unknown result type (might be due to invalid IL or missing references)
		//IL_070f: Expected O, but got Unknown
		//IL_07e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07eb: Expected O, but got Unknown
		//IL_0802: Unknown result type (might be due to invalid IL or missing references)
		//IL_080c: Expected O, but got Unknown
		//IL_0823: Unknown result type (might be due to invalid IL or missing references)
		//IL_082d: Expected O, but got Unknown
		//IL_082e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0838: Expected O, but got Unknown
		//IL_085a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0864: Expected O, but got Unknown
		//IL_089c: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a6: Expected O, but got Unknown
		//IL_08f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_08fe: Expected O, but got Unknown
		//IL_090a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0914: Expected O, but got Unknown
		//IL_0a75: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a7f: Expected O, but got Unknown
		//IL_0ac2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0acc: Expected O, but got Unknown
		//IL_0b46: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b50: Expected O, but got Unknown
		//IL_0bb4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bbe: Expected O, but got Unknown
		//IL_10c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_10ca: Expected O, but got Unknown
		//IL_10d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_10e1: Expected O, but got Unknown
		//IL_8789: Unknown result type (might be due to invalid IL or missing references)
		//IL_8793: Expected O, but got Unknown
		//IL_87b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_87c1: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
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
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
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
		Appearance appearance174 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance179 = new Appearance();
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
		Appearance appearance134 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance171 = new Appearance();
		Appearance appearance182 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance168 = new Appearance();
		Appearance appearance169 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance172 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance180 = new Appearance();
		Appearance appearance181 = new Appearance();
		Appearance appearance183 = new Appearance();
		Appearance appearance184 = new Appearance();
		Appearance appearance185 = new Appearance();
		Appearance appearance186 = new Appearance();
		Appearance appearance187 = new Appearance();
		Appearance appearance188 = new Appearance();
		Appearance appearance189 = new Appearance();
		Appearance appearance190 = new Appearance();
		Appearance appearance191 = new Appearance();
		Appearance appearance192 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
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
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		GridEXLayout grdhistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance35 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance157 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance159 = new Appearance();
		GridEXLayout DataGridHistoryDetail_DesignTimeLayout = new GridEXLayout();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Sale.BookingOrder));
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.grd = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label18 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.btnCancelUpdateDetial = new System.Windows.Forms.Button();
		this.CmbItem = new UltraCombo();
		this.rdSearchByCode = new System.Windows.Forms.RadioButton();
		this.rdSearchByName = new System.Windows.Forms.RadioButton();
		this.cmbReferenceParty = new UltraCombo();
		this.label31 = new System.Windows.Forms.Label();
		this.chkCommissionOnSale = new System.Windows.Forms.CheckBox();
		this.txtPartDiscAmt = new System.Windows.Forms.TextBox();
		this.label16 = new System.Windows.Forms.Label();
		this.label15 = new System.Windows.Forms.Label();
		this.txtPackingAddless = new System.Windows.Forms.TextBox();
		this.txtItemDiscountAmt = new System.Windows.Forms.TextBox();
		this.label14 = new System.Windows.Forms.Label();
		this.txtNetAmount = new System.Windows.Forms.TextBox();
		this.label10 = new System.Windows.Forms.Label();
		this.txtItemDiscPrct = new System.Windows.Forms.TextBox();
		this.label5 = new System.Windows.Forms.Label();
		this.cmbPackType = new UltraCombo();
		this.label2 = new System.Windows.Forms.Label();
		this.txtPartyDiscPrct = new System.Windows.Forms.TextBox();
		this.txtRateUom = new System.Windows.Forms.TextBox();
		this.txtRateUomId = new System.Windows.Forms.TextBox();
		this.label13 = new System.Windows.Forms.Label();
		this.txtNetRate = new System.Windows.Forms.TextBox();
		this.txtamount = new System.Windows.Forms.TextBox();
		this.label9 = new System.Windows.Forms.Label();
		this.txtPriceSchId = new System.Windows.Forms.TextBox();
		this.CmbCropyr = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.combcityarea = new UltraCombo();
		this.combitempck = new UltraCombo();
		this.btnplus = new System.Windows.Forms.Button();
		this.txtremarksdetail = new System.Windows.Forms.TextBox();
		this.label47 = new System.Windows.Forms.Label();
		this.txtItemPrice = new System.Windows.Forms.TextBox();
		this.txtweight = new System.Windows.Forms.TextBox();
		this.txtqty = new System.Windows.Forms.TextBox();
		this.label40 = new System.Windows.Forms.Label();
		this.label35 = new System.Windows.Forms.Label();
		this.label44 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label39 = new System.Windows.Forms.Label();
		this.label38 = new System.Windows.Forms.Label();
		this.label36 = new System.Windows.Forms.Label();
		this.label37 = new System.Windows.Forms.Label();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.panel4 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.txtBranchSrNo = new System.Windows.Forms.TextBox();
		this.txtremarks = new System.Windows.Forms.TextBox();
		this.combpttrm = new UltraCombo();
		this.duedate = new System.Windows.Forms.DateTimePicker();
		this.txtduedays = new System.Windows.Forms.TextBox();
		this.CmbOrderType = new UltraCombo();
		this.DocDate = new System.Windows.Forms.DateTimePicker();
		this.label33 = new System.Windows.Forms.Label();
		this.combordercat = new UltraCombo();
		this.combsalesman = new UltraCombo();
		this.txtCommrate = new System.Windows.Forms.TextBox();
		this.label29 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.CmbCustomer = new UltraCombo();
		this.label19 = new System.Windows.Forms.Label();
		this.txtcatsr = new System.Windows.Forms.TextBox();
		this.txtcommamount = new System.Windows.Forms.TextBox();
		this.label11 = new System.Windows.Forms.Label();
		this.txtdeliverydays = new System.Windows.Forms.TextBox();
		this.combruom = new UltraCombo();
		this.label20 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.label30 = new System.Windows.Forms.Label();
		this.txtcommremarks = new System.Windows.Forms.TextBox();
		this.label24 = new System.Windows.Forms.Label();
		this.combcommtype = new UltraCombo();
		this.txtsupprefno = new System.Windows.Forms.TextBox();
		this.combdeliverytrm = new UltraCombo();
		this.label28 = new System.Windows.Forms.Label();
		this.label26 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.deliverystartdate = new System.Windows.Forms.DateTimePicker();
		this.label4 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.label32 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.ChkBox = new System.Windows.Forms.CheckBox();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
		this.btnprint = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
		this.btnattachment = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.BtnPackingPriceForm = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
		this.BtnLoader = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
		this.BtnShortCutkeys = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.grdhistory = new GridEX();
		this.panel10 = new System.Windows.Forms.Panel();
		this.label34 = new System.Windows.Forms.Label();
		this.panel13 = new System.Windows.Forms.Panel();
		this.label27 = new System.Windows.Forms.Label();
		this.panel7 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.cmbBranchName = new UltraCombo();
		this.label41 = new System.Windows.Forms.Label();
		this.rdapproveddate = new System.Windows.Forms.RadioButton();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.btnShowHistory = new System.Windows.Forms.Button();
		this.label50 = new System.Windows.Forms.Label();
		this.label51 = new System.Windows.Forms.Label();
		this.label52 = new System.Windows.Forms.Label();
		this.label53 = new System.Windows.Forms.Label();
		this.label54 = new System.Windows.Forms.Label();
		this.CmbCustomerHistory = new UltraCombo();
		this.ToDocNoHistory = new System.Windows.Forms.TextBox();
		this.FromDocNoHistory = new System.Windows.Forms.TextBox();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.panel8 = new System.Windows.Forms.Panel();
		this.label22 = new System.Windows.Forms.Label();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnResetHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.DataGridHistoryDetail = new GridEX();
		this.btnGenerateCalculations = new System.Windows.Forms.ToolStripButton();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel6.SuspendLayout();
		this.panel5.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbItem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbReferenceParty).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPackType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropyr).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combcityarea).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combitempck).BeginInit();
		this.panel4.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.combpttrm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbOrderType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combordercat).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combsalesman).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomer).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combruom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combcommtype).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combdeliverytrm).BeginInit();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdhistory).BeginInit();
		this.panel10.SuspendLayout();
		this.panel13.SuspendLayout();
		this.panel7.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).BeginInit();
		this.panel8.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistoryDetail).BeginInit();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1234, 561);
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
		this.tabControl1.Size = new System.Drawing.Size(1234, 561);
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
		this.tabPage1.Size = new System.Drawing.Size(1226, 532);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(2, 327);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1222, 203);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.CellUpdated += new ColumnActionEventHandler(grd_CellUpdated);
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grd_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.label18);
		this.panel6.Controls.Add(this.ctrlGrdBar1);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(2, 306);
		this.panel6.Margin = new System.Windows.Forms.Padding(2);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1222, 21);
		this.panel6.TabIndex = 4;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label18.ForeColor = System.Drawing.Color.White;
		this.label18.Location = new System.Drawing.Point(4, 3);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(151, 17);
		this.label18.TabIndex = 1;
		this.label18.Text = "Booking Order Detail";
		this.panel5.BackColor = System.Drawing.Color.White;
		this.panel5.Controls.Add(this.groupBox3);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(2, 189);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1222, 117);
		this.panel5.TabIndex = 3;
		this.groupBox3.BackColor = System.Drawing.Color.White;
		this.groupBox3.Controls.Add(this.btnCancelUpdateDetial);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItem);
		this.groupBox3.Controls.Add(this.rdSearchByCode);
		this.groupBox3.Controls.Add(this.rdSearchByName);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbReferenceParty);
		this.groupBox3.Controls.Add(this.label31);
		this.groupBox3.Controls.Add(this.chkCommissionOnSale);
		this.groupBox3.Controls.Add(this.txtPartDiscAmt);
		this.groupBox3.Controls.Add(this.label16);
		this.groupBox3.Controls.Add(this.label15);
		this.groupBox3.Controls.Add(this.txtPackingAddless);
		this.groupBox3.Controls.Add(this.txtItemDiscountAmt);
		this.groupBox3.Controls.Add(this.label14);
		this.groupBox3.Controls.Add(this.txtNetAmount);
		this.groupBox3.Controls.Add(this.label10);
		this.groupBox3.Controls.Add(this.txtItemDiscPrct);
		this.groupBox3.Controls.Add(this.label5);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPackType);
		this.groupBox3.Controls.Add(this.label2);
		this.groupBox3.Controls.Add(this.txtPartyDiscPrct);
		this.groupBox3.Controls.Add(this.txtRateUom);
		this.groupBox3.Controls.Add(this.txtRateUomId);
		this.groupBox3.Controls.Add(this.label13);
		this.groupBox3.Controls.Add(this.txtNetRate);
		this.groupBox3.Controls.Add(this.txtamount);
		this.groupBox3.Controls.Add(this.label9);
		this.groupBox3.Controls.Add(this.txtPriceSchId);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCropyr);
		this.groupBox3.Controls.Add(this.label1);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.combcityarea);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.combitempck);
		this.groupBox3.Controls.Add(this.btnplus);
		this.groupBox3.Controls.Add(this.txtremarksdetail);
		this.groupBox3.Controls.Add(this.label47);
		this.groupBox3.Controls.Add(this.txtItemPrice);
		this.groupBox3.Controls.Add(this.txtweight);
		this.groupBox3.Controls.Add(this.txtqty);
		this.groupBox3.Controls.Add(this.label40);
		this.groupBox3.Controls.Add(this.label35);
		this.groupBox3.Controls.Add(this.label44);
		this.groupBox3.Controls.Add(this.label3);
		this.groupBox3.Controls.Add(this.label39);
		this.groupBox3.Controls.Add(this.label38);
		this.groupBox3.Controls.Add(this.label36);
		this.groupBox3.Controls.Add(this.label37);
		this.groupBox3.Controls.Add(this.btnUpdateDetail);
		this.groupBox3.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(3, 3);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(1209, 111);
		this.groupBox3.TabIndex = 0;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Detail";
		this.btnCancelUpdateDetial.BackColor = System.Drawing.Color.Teal;
		this.btnCancelUpdateDetial.FlatAppearance.BorderSize = 0;
		this.btnCancelUpdateDetial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelUpdateDetial.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.btnCancelUpdateDetial.ForeColor = System.Drawing.Color.White;
		this.btnCancelUpdateDetial.Location = new System.Drawing.Point(1142, 80);
		this.btnCancelUpdateDetial.Name = "btnCancelUpdateDetial";
		this.btnCancelUpdateDetial.Size = new System.Drawing.Size(62, 26);
		this.btnCancelUpdateDetial.TabIndex = 22;
		this.btnCancelUpdateDetial.Text = "Cancel";
		this.btnCancelUpdateDetial.UseVisualStyleBackColor = false;
		this.btnCancelUpdateDetial.Visible = false;
		this.btnCancelUpdateDetial.Click += new System.EventHandler(btnCancelUpdateDetial_Click);
		this.CmbItem.AllowNull = (DefaultableBoolean)1;
		this.CmbItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance51).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance51).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance51).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance51;
		((AppearanceBase)appearance52).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance52;
		((SpecialBoxBase)((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance54).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance54).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.CmbItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance55;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance57;
		((AppearanceBase)appearance58).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance58).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance59).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance59).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance59).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.CmbItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItem).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItem).Location = new System.Drawing.Point(3, 35);
		((System.Windows.Forms.Control)(object)this.CmbItem).Name = "CmbItem";
		((System.Windows.Forms.Control)(object)this.CmbItem).Size = new System.Drawing.Size(223, 26);
		((System.Windows.Forms.Control)(object)this.CmbItem).TabIndex = 0;
		this.CmbItem.ValueChanged += new System.EventHandler(combitem_ValueChanged);
		((System.Windows.Forms.Control)(object)this.CmbItem).Leave += new System.EventHandler(combitem_Leave);
		this.rdSearchByCode.AutoSize = true;
		this.rdSearchByCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdSearchByCode.Location = new System.Drawing.Point(122, 16);
		this.rdSearchByCode.Name = "rdSearchByCode";
		this.rdSearchByCode.Size = new System.Drawing.Size(52, 19);
		this.rdSearchByCode.TabIndex = 54;
		this.rdSearchByCode.TabStop = true;
		this.rdSearchByCode.Text = "Code";
		this.rdSearchByCode.UseVisualStyleBackColor = true;
		this.rdSearchByName.AutoSize = true;
		this.rdSearchByName.Checked = true;
		this.rdSearchByName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdSearchByName.Location = new System.Drawing.Point(63, 16);
		this.rdSearchByName.Name = "rdSearchByName";
		this.rdSearchByName.Size = new System.Drawing.Size(57, 19);
		this.rdSearchByName.TabIndex = 53;
		this.rdSearchByName.TabStop = true;
		this.rdSearchByName.Text = "Name";
		this.rdSearchByName.UseVisualStyleBackColor = true;
		this.rdSearchByName.CheckedChanged += new System.EventHandler(rdSearchByName_CheckedChanged);
		this.cmbReferenceParty.AllowNull = (DefaultableBoolean)1;
		this.cmbReferenceParty.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbReferenceParty.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbReferenceParty.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbReferenceParty).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbReferenceParty).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbReferenceParty).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbReferenceParty).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbReferenceParty).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.cmbReferenceParty).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbReferenceParty).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbReferenceParty).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbReferenceParty).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbReferenceParty).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbReferenceParty).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.cmbReferenceParty).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbReferenceParty).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbReferenceParty).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance33).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbReferenceParty).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.cmbReferenceParty).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbReferenceParty).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbReferenceParty).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbReferenceParty).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.cmbReferenceParty).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbReferenceParty).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbReferenceParty).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.cmbReferenceParty).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbReferenceParty).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.cmbReferenceParty).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbReferenceParty).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbReferenceParty).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbReferenceParty).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbReferenceParty.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbReferenceParty).Location = new System.Drawing.Point(614, 80);
		((System.Windows.Forms.Control)(object)this.cmbReferenceParty).Name = "cmbReferenceParty";
		((System.Windows.Forms.Control)(object)this.cmbReferenceParty).Size = new System.Drawing.Size(162, 26);
		((System.Windows.Forms.Control)(object)this.cmbReferenceParty).TabIndex = 17;
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label31.Location = new System.Drawing.Point(617, 64);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(89, 15);
		this.label31.TabIndex = 51;
		this.label31.Text = "Reference Party";
		this.chkCommissionOnSale.AutoSize = true;
		this.chkCommissionOnSale.Enabled = false;
		this.chkCommissionOnSale.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.chkCommissionOnSale.Location = new System.Drawing.Point(1079, 62);
		this.chkCommissionOnSale.Name = "chkCommissionOnSale";
		this.chkCommissionOnSale.Size = new System.Drawing.Size(106, 19);
		this.chkCommissionOnSale.TabIndex = 19;
		this.chkCommissionOnSale.Text = "Comm On Sale";
		this.chkCommissionOnSale.UseVisualStyleBackColor = true;
		this.txtPartDiscAmt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPartDiscAmt.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtPartDiscAmt.Location = new System.Drawing.Point(1006, 37);
		this.txtPartDiscAmt.Name = "txtPartDiscAmt";
		this.txtPartDiscAmt.ReadOnly = true;
		this.txtPartDiscAmt.Size = new System.Drawing.Size(105, 23);
		this.txtPartDiscAmt.TabIndex = 9;
		this.txtPartDiscAmt.TabStop = false;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(1008, 19);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(86, 15);
		this.label16.TabIndex = 48;
		this.label16.Text = "Party Disc Amt";
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(825, 19);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(99, 15);
		this.label15.TabIndex = 46;
		this.label15.Text = "Packing Exp/Disc";
		this.txtPackingAddless.Enabled = false;
		this.txtPackingAddless.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtPackingAddless.Location = new System.Drawing.Point(825, 37);
		this.txtPackingAddless.Name = "txtPackingAddless";
		this.txtPackingAddless.Size = new System.Drawing.Size(105, 23);
		this.txtPackingAddless.TabIndex = 7;
		this.txtPackingAddless.TextChanged += new System.EventHandler(txtPackingAddless_TextChanged);
		this.txtItemDiscountAmt.BackColor = System.Drawing.SystemColors.Control;
		this.txtItemDiscountAmt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemDiscountAmt.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtItemDiscountAmt.Location = new System.Drawing.Point(264, 82);
		this.txtItemDiscountAmt.Name = "txtItemDiscountAmt";
		this.txtItemDiscountAmt.ReadOnly = true;
		this.txtItemDiscountAmt.Size = new System.Drawing.Size(87, 23);
		this.txtItemDiscountAmt.TabIndex = 14;
		this.txtItemDiscountAmt.TabStop = false;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(267, 64);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(77, 15);
		this.label14.TabIndex = 44;
		this.label14.Text = "Disc Amount";
		this.txtNetAmount.BackColor = System.Drawing.SystemColors.Control;
		this.txtNetAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNetAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtNetAmount.Location = new System.Drawing.Point(352, 82);
		this.txtNetAmount.Name = "txtNetAmount";
		this.txtNetAmount.ReadOnly = true;
		this.txtNetAmount.Size = new System.Drawing.Size(125, 23);
		this.txtNetAmount.TabIndex = 15;
		this.txtNetAmount.TabStop = false;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(357, 64);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(73, 15);
		this.label10.TabIndex = 42;
		this.label10.Text = "Net Amount";
		this.txtItemDiscPrct.BackColor = System.Drawing.SystemColors.Control;
		this.txtItemDiscPrct.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemDiscPrct.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtItemDiscPrct.Location = new System.Drawing.Point(189, 82);
		this.txtItemDiscPrct.Name = "txtItemDiscPrct";
		this.txtItemDiscPrct.ReadOnly = true;
		this.txtItemDiscPrct.Size = new System.Drawing.Size(74, 23);
		this.txtItemDiscPrct.TabIndex = 13;
		this.txtItemDiscPrct.TabStop = false;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(192, 64);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(71, 15);
		this.label5.TabIndex = 40;
		this.label5.Text = "Item Disc %";
		this.cmbPackType.AllowNull = (DefaultableBoolean)1;
		this.cmbPackType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPackType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPackType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance174).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance174).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance174;
		((UltraGridBase)this.cmbPackType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPackType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance175).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance175).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance175).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPackType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance175;
		((AppearanceBase)appearance176).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPackType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance176;
		((SpecialBoxBase)((UltraGridBase)this.cmbPackType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance39).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance39).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance39).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPackType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbPackType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPackType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance40).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance42;
		((AppearanceBase)appearance43).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance43).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance44).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance44).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance44).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance177).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance177;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance178).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance178;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance179;
		((UltraGridBase)this.cmbPackType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPackType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPackType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPackType).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbPackType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPackType).Location = new System.Drawing.Point(352, 35);
		((System.Windows.Forms.Control)(object)this.cmbPackType).Name = "cmbPackType";
		((System.Windows.Forms.Control)(object)this.cmbPackType).Size = new System.Drawing.Size(125, 26);
		((System.Windows.Forms.Control)(object)this.cmbPackType).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.cmbPackType).Leave += new System.EventHandler(cmbPackType_Leave);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(352, 19);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(60, 15);
		this.label2.TabIndex = 38;
		this.label2.Text = "Pack Type";
		this.txtPartyDiscPrct.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPartyDiscPrct.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtPartyDiscPrct.Location = new System.Drawing.Point(931, 37);
		this.txtPartyDiscPrct.Name = "txtPartyDiscPrct";
		this.txtPartyDiscPrct.ReadOnly = true;
		this.txtPartyDiscPrct.Size = new System.Drawing.Size(74, 23);
		this.txtPartyDiscPrct.TabIndex = 8;
		this.txtPartyDiscPrct.TabStop = false;
		this.txtRateUom.BackColor = System.Drawing.SystemColors.Control;
		this.txtRateUom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRateUom.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtRateUom.Location = new System.Drawing.Point(3, 82);
		this.txtRateUom.Name = "txtRateUom";
		this.txtRateUom.ReadOnly = true;
		this.txtRateUom.Size = new System.Drawing.Size(70, 23);
		this.txtRateUom.TabIndex = 11;
		this.txtRateUom.TabStop = false;
		this.txtRateUomId.Location = new System.Drawing.Point(1279, 95);
		this.txtRateUomId.Name = "txtRateUomId";
		this.txtRateUomId.Size = new System.Drawing.Size(45, 25);
		this.txtRateUomId.TabIndex = 31;
		this.txtRateUomId.Visible = false;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(4, 64);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(59, 15);
		this.label13.TabIndex = 35;
		this.label13.Text = "Rate Uom";
		this.txtNetRate.BackColor = System.Drawing.SystemColors.Control;
		this.txtNetRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNetRate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtNetRate.Location = new System.Drawing.Point(1113, 37);
		this.txtNetRate.Name = "txtNetRate";
		this.txtNetRate.ReadOnly = true;
		this.txtNetRate.Size = new System.Drawing.Size(87, 23);
		this.txtNetRate.TabIndex = 10;
		this.txtNetRate.TabStop = false;
		this.txtamount.BackColor = System.Drawing.SystemColors.Control;
		this.txtamount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtamount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtamount.Location = new System.Drawing.Point(74, 82);
		this.txtamount.Name = "txtamount";
		this.txtamount.ReadOnly = true;
		this.txtamount.Size = new System.Drawing.Size(114, 23);
		this.txtamount.TabIndex = 12;
		this.txtamount.TabStop = false;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(78, 64);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(51, 15);
		this.label9.TabIndex = 36;
		this.label9.Text = "Amount";
		this.txtPriceSchId.Location = new System.Drawing.Point(1279, 68);
		this.txtPriceSchId.Name = "txtPriceSchId";
		this.txtPriceSchId.Size = new System.Drawing.Size(45, 25);
		this.txtPriceSchId.TabIndex = 30;
		this.txtPriceSchId.Visible = false;
		this.CmbCropyr.AllowNull = (DefaultableBoolean)1;
		this.CmbCropyr.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCropyr.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCropyr.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropyr).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.CmbCropyr).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCropyr).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCropyr).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbCropyr.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCropyr).Location = new System.Drawing.Point(227, 35);
		((System.Windows.Forms.Control)(object)this.CmbCropyr).Name = "CmbCropyr";
		((System.Windows.Forms.Control)(object)this.CmbCropyr).Size = new System.Drawing.Size(124, 26);
		((System.Windows.Forms.Control)(object)this.CmbCropyr).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.CmbCropyr).Leave += new System.EventHandler(CmbCropyr_Leave);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(1117, 19);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(52, 15);
		this.label1.TabIndex = 35;
		this.label1.Text = "Net Rate";
		this.combcityarea.AllowNull = (DefaultableBoolean)1;
		this.combcityarea.AutoCompleteMode = (AutoCompleteMode)4;
		this.combcityarea.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combcityarea.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combcityarea).DisplayLayout.Appearance = (AppearanceBase)(object)appearance134;
		((UltraGridBase)this.combcityarea).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combcityarea).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance135).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance135).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance135).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combcityarea).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance135;
		((AppearanceBase)appearance136).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combcityarea).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance136;
		((SpecialBoxBase)((UltraGridBase)this.combcityarea).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance161).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance161).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance161).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance161).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combcityarea).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance161;
		((UltraGridBase)this.combcityarea).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combcityarea).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance152).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combcityarea).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance152;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance153).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combcityarea).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance153;
		((UltraGridBase)this.combcityarea).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combcityarea).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combcityarea).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance162;
		((AppearanceBase)appearance163).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance163).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combcityarea).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance163;
		((UltraGridBase)this.combcityarea).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combcityarea).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance164).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance164).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance164).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance164).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combcityarea).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance164;
		((AppearanceBase)appearance165).TextHAlignAsString = "Left";
		((UltraGridBase)this.combcityarea).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance165;
		((UltraGridBase)this.combcityarea).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combcityarea).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance166).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance166).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combcityarea).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance166;
		((UltraGridBase)this.combcityarea).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combcityarea).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance167;
		((UltraGridBase)this.combcityarea).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combcityarea).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combcityarea).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combcityarea).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.combcityarea.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combcityarea).Location = new System.Drawing.Point(478, 80);
		((System.Windows.Forms.Control)(object)this.combcityarea).Name = "combcityarea";
		((System.Windows.Forms.Control)(object)this.combcityarea).Size = new System.Drawing.Size(135, 26);
		((System.Windows.Forms.Control)(object)this.combcityarea).TabIndex = 16;
		this.combitempck.AllowNull = (DefaultableBoolean)1;
		this.combitempck.AutoCompleteMode = (AutoCompleteMode)4;
		this.combitempck.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combitempck.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combitempck).DisplayLayout.Appearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.combitempck).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combitempck).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance47).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance47).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combitempck).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance47;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combitempck).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance48;
		((SpecialBoxBase)((UltraGridBase)this.combitempck).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance124).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance124).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance124).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combitempck).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.combitempck).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combitempck).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance125;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance126).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance128;
		((AppearanceBase)appearance129).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance129).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance130).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance130).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance130).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance130;
		((AppearanceBase)appearance131).TextHAlignAsString = "Left";
		((UltraGridBase)this.combitempck).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance132).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combitempck).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.combitempck).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combitempck).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combitempck).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combitempck).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.combitempck.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combitempck).Location = new System.Drawing.Point(479, 35);
		((System.Windows.Forms.Control)(object)this.combitempck).Name = "combitempck";
		((System.Windows.Forms.Control)(object)this.combitempck).Size = new System.Drawing.Size(76, 26);
		((System.Windows.Forms.Control)(object)this.combitempck).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.combitempck).Leave += new System.EventHandler(combitempck_Leave);
		this.btnplus.BackColor = System.Drawing.Color.Teal;
		this.btnplus.FlatAppearance.BorderSize = 0;
		this.btnplus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnplus.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnplus.ForeColor = System.Drawing.Color.White;
		this.btnplus.Location = new System.Drawing.Point(1082, 80);
		this.btnplus.Name = "btnplus";
		this.btnplus.Size = new System.Drawing.Size(29, 26);
		this.btnplus.TabIndex = 20;
		this.btnplus.Text = "+";
		this.btnplus.UseVisualStyleBackColor = false;
		this.btnplus.Click += new System.EventHandler(btnplus_Click);
		this.txtremarksdetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtremarksdetail.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtremarksdetail.Location = new System.Drawing.Point(777, 82);
		this.txtremarksdetail.Name = "txtremarksdetail";
		this.txtremarksdetail.Size = new System.Drawing.Size(300, 23);
		this.txtremarksdetail.TabIndex = 18;
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label47.Location = new System.Drawing.Point(780, 64);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(52, 15);
		this.label47.TabIndex = 25;
		this.label47.Text = "Remarks";
		this.txtItemPrice.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemPrice.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtItemPrice.Location = new System.Drawing.Point(733, 37);
		this.txtItemPrice.Name = "txtItemPrice";
		this.txtItemPrice.ReadOnly = true;
		this.txtItemPrice.Size = new System.Drawing.Size(89, 23);
		this.txtItemPrice.TabIndex = 6;
		this.txtItemPrice.TabStop = false;
		this.txtItemPrice.Text = "0";
		this.txtItemPrice.TextChanged += new System.EventHandler(txtRate_TextChanged);
		this.txtItemPrice.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtRate_KeyPress);
		this.txtweight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtweight.Enabled = false;
		this.txtweight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtweight.Location = new System.Drawing.Point(641, 37);
		this.txtweight.Name = "txtweight";
		this.txtweight.ReadOnly = true;
		this.txtweight.Size = new System.Drawing.Size(90, 23);
		this.txtweight.TabIndex = 5;
		this.txtweight.TabStop = false;
		this.txtweight.TextChanged += new System.EventHandler(txtweight_TextChanged);
		this.txtqty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtqty.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtqty.Location = new System.Drawing.Point(557, 37);
		this.txtqty.Name = "txtqty";
		this.txtqty.Size = new System.Drawing.Size(82, 23);
		this.txtqty.TabIndex = 4;
		this.txtqty.TextChanged += new System.EventHandler(txtqty_TextChanged);
		this.txtqty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtqty_KeyPress);
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.Location = new System.Drawing.Point(735, 19);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(61, 15);
		this.label40.TabIndex = 17;
		this.label40.Text = "Item Price";
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label35.Location = new System.Drawing.Point(481, 64);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(62, 15);
		this.label35.TabIndex = 25;
		this.label35.Text = "City Name";
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label44.Location = new System.Drawing.Point(931, 19);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(73, 15);
		this.label44.TabIndex = 23;
		this.label44.Text = "Party Disc %";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(231, 19);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(57, 15);
		this.label3.TabIndex = 19;
		this.label3.Text = "Crop Year";
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label39.Location = new System.Drawing.Point(644, 19);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(46, 15);
		this.label39.TabIndex = 15;
		this.label39.Text = "Weight";
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.Location = new System.Drawing.Point(560, 19);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(26, 15);
		this.label38.TabIndex = 13;
		this.label38.Text = "Qty";
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(481, 19);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(61, 15);
		this.label36.TabIndex = 8;
		this.label36.Text = "Pack Uom";
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(6, 18);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(32, 15);
		this.label37.TabIndex = 9;
		this.label37.Text = "Item";
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatAppearance.BorderSize = 0;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(1078, 80);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(62, 26);
		this.btnUpdateDetail.TabIndex = 21;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Visible = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(2, 50);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1222, 139);
		this.panel4.TabIndex = 0;
		this.groupBox1.Controls.Add(this.txtBranchSrNo);
		this.groupBox1.Controls.Add(this.txtremarks);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.combpttrm);
		this.groupBox1.Controls.Add(this.duedate);
		this.groupBox1.Controls.Add(this.txtduedays);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbOrderType);
		this.groupBox1.Controls.Add(this.DocDate);
		this.groupBox1.Controls.Add(this.label33);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.combordercat);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.combsalesman);
		this.groupBox1.Controls.Add(this.txtCommrate);
		this.groupBox1.Controls.Add(this.label29);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomer);
		this.groupBox1.Controls.Add(this.label19);
		this.groupBox1.Controls.Add(this.txtcatsr);
		this.groupBox1.Controls.Add(this.txtcommamount);
		this.groupBox1.Controls.Add(this.label11);
		this.groupBox1.Controls.Add(this.txtdeliverydays);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.combruom);
		this.groupBox1.Controls.Add(this.label20);
		this.groupBox1.Controls.Add(this.label25);
		this.groupBox1.Controls.Add(this.label12);
		this.groupBox1.Controls.Add(this.label21);
		this.groupBox1.Controls.Add(this.label30);
		this.groupBox1.Controls.Add(this.txtcommremarks);
		this.groupBox1.Controls.Add(this.label24);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.combcommtype);
		this.groupBox1.Controls.Add(this.txtsupprefno);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.combdeliverytrm);
		this.groupBox1.Controls.Add(this.label28);
		this.groupBox1.Controls.Add(this.label26);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add(this.txtdocno);
		this.groupBox1.Controls.Add(this.deliverystartdate);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add(this.label17);
		this.groupBox1.Controls.Add(this.label32);
		this.groupBox1.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(3, 2);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1208, 132);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Main";
		this.txtBranchSrNo.BackColor = System.Drawing.Color.White;
		this.txtBranchSrNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBranchSrNo.Enabled = false;
		this.txtBranchSrNo.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtBranchSrNo.Location = new System.Drawing.Point(200, 18);
		this.txtBranchSrNo.Name = "txtBranchSrNo";
		this.txtBranchSrNo.Size = new System.Drawing.Size(56, 23);
		this.txtBranchSrNo.TabIndex = 1;
		this.txtBranchSrNo.TabStop = false;
		this.txtremarks.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtremarks.Location = new System.Drawing.Point(463, 103);
		this.txtremarks.Name = "txtremarks";
		this.txtremarks.Size = new System.Drawing.Size(382, 23);
		this.txtremarks.TabIndex = 19;
		this.combpttrm.AllowNull = (DefaultableBoolean)1;
		this.combpttrm.AutoCompleteMode = (AutoCompleteMode)4;
		this.combpttrm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combpttrm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combpttrm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.combpttrm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combpttrm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance90).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance90).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance90).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combpttrm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance90;
		((AppearanceBase)appearance116).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combpttrm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance116;
		((SpecialBoxBase)((UltraGridBase)this.combpttrm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance127).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance127).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance127).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combpttrm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.combpttrm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combpttrm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance138).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance138;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance149).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance149;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance160;
		((AppearanceBase)appearance171).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance171).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance171;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance182).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance182).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance182).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance182).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance182).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance182;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.combpttrm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combpttrm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combpttrm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combpttrm).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.combpttrm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combpttrm).Location = new System.Drawing.Point(463, 16);
		((System.Windows.Forms.Control)(object)this.combpttrm).Name = "combpttrm";
		((System.Windows.Forms.Control)(object)this.combpttrm).Size = new System.Drawing.Size(133, 26);
		((System.Windows.Forms.Control)(object)this.combpttrm).TabIndex = 8;
		((System.Windows.Forms.Control)(object)this.combpttrm).Leave += new System.EventHandler(combpttrm_Leave);
		this.duedate.CustomFormat = "dd-MMM-yy";
		this.duedate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.duedate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.duedate.Location = new System.Drawing.Point(463, 74);
		this.duedate.Name = "duedate";
		this.duedate.Size = new System.Drawing.Size(133, 23);
		this.duedate.TabIndex = 10;
		this.txtduedays.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtduedays.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtduedays.Location = new System.Drawing.Point(463, 46);
		this.txtduedays.Name = "txtduedays";
		this.txtduedays.Size = new System.Drawing.Size(133, 23);
		this.txtduedays.TabIndex = 9;
		this.txtduedays.TextChanged += new System.EventHandler(txtduedays_TextChanged);
		this.txtduedays.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtduedays_KeyPress);
		this.CmbOrderType.AllowNull = (DefaultableBoolean)1;
		this.CmbOrderType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbOrderType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbOrderType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance137).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbOrderType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance137;
		((UltraGridBase)this.CmbOrderType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbOrderType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance139).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance139).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance139).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbOrderType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance139;
		((AppearanceBase)appearance140).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOrderType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance140;
		((SpecialBoxBase)((UltraGridBase)this.CmbOrderType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance141).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance141).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance141).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOrderType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance141;
		((UltraGridBase)this.CmbOrderType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbOrderType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance142).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbOrderType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance142;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance143).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbOrderType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.CmbOrderType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbOrderType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOrderType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance168;
		((AppearanceBase)appearance169).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance169).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbOrderType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance169;
		((UltraGridBase)this.CmbOrderType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbOrderType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance170).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance170).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance170).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance170).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOrderType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance170;
		((AppearanceBase)appearance147).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbOrderType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance147;
		((UltraGridBase)this.CmbOrderType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbOrderType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance172).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbOrderType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance172;
		((UltraGridBase)this.CmbOrderType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance173).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbOrderType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance173;
		((UltraGridBase)this.CmbOrderType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbOrderType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbOrderType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbOrderType).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbOrderType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbOrderType).Location = new System.Drawing.Point(273, 101);
		((System.Windows.Forms.Control)(object)this.CmbOrderType).Name = "CmbOrderType";
		((System.Windows.Forms.Control)(object)this.CmbOrderType).Size = new System.Drawing.Size(97, 26);
		((System.Windows.Forms.Control)(object)this.CmbOrderType).TabIndex = 6;
		this.DocDate.CustomFormat = "dd-MMM-yy";
		this.DocDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DocDate.Location = new System.Drawing.Point(257, 18);
		this.DocDate.Name = "DocDate";
		this.DocDate.Size = new System.Drawing.Size(113, 23);
		this.DocDate.TabIndex = 2;
		this.DocDate.TabStop = false;
		this.DocDate.Leave += new System.EventHandler(DocDate_Leave);
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(375, 107);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(52, 15);
		this.label33.TabIndex = 8;
		this.label33.Text = "Remarks";
		this.combordercat.AllowNull = (DefaultableBoolean)1;
		this.combordercat.AutoCompleteMode = (AutoCompleteMode)4;
		this.combordercat.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combordercat.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combordercat).DisplayLayout.Appearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.combordercat).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combordercat).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance93).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance93).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combordercat).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combordercat).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance94;
		((SpecialBoxBase)((UltraGridBase)this.combordercat).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance95).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance95).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combordercat).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.combordercat).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combordercat).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance96).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combordercat).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance98).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combordercat).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance98;
		((UltraGridBase)this.combordercat).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combordercat).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combordercat).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance99;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance100).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combordercat).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance100;
		((UltraGridBase)this.combordercat).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combordercat).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance101).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance101).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance101).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance101).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combordercat).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance101;
		((AppearanceBase)appearance102).TextHAlignAsString = "Left";
		((UltraGridBase)this.combordercat).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.combordercat).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combordercat).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combordercat).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.combordercat).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combordercat).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.combordercat).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combordercat).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combordercat).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combordercat).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combordercat.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combordercat).Location = new System.Drawing.Point(121, 44);
		((System.Windows.Forms.Control)(object)this.combordercat).Name = "combordercat";
		((System.Windows.Forms.Control)(object)this.combordercat).Size = new System.Drawing.Size(170, 26);
		((System.Windows.Forms.Control)(object)this.combordercat).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.combordercat).TabStop = false;
		((System.Windows.Forms.Control)(object)this.combordercat).Leave += new System.EventHandler(combordercat_Leave);
		this.combsalesman.AllowNull = (DefaultableBoolean)1;
		this.combsalesman.AutoCompleteMode = (AutoCompleteMode)4;
		this.combsalesman.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combsalesman.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combsalesman).DisplayLayout.Appearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.combsalesman).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combsalesman).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance4).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance4).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance4).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combsalesman).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combsalesman).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance5;
		((SpecialBoxBase)((UltraGridBase)this.combsalesman).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combsalesman).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.combsalesman).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combsalesman).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combsalesman).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance7;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combsalesman).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.combsalesman).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combsalesman).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combsalesman).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance10).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combsalesman).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.combsalesman).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combsalesman).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance11).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance11).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance11).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance11).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combsalesman).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance105).TextHAlignAsString = "Left";
		((UltraGridBase)this.combsalesman).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.combsalesman).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combsalesman).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combsalesman).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.combsalesman).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combsalesman).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.combsalesman).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combsalesman).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combsalesman).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combsalesman).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.combsalesman.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combsalesman).Location = new System.Drawing.Point(958, 16);
		((System.Windows.Forms.Control)(object)this.combsalesman).Name = "combsalesman";
		((System.Windows.Forms.Control)(object)this.combsalesman).Size = new System.Drawing.Size(246, 26);
		((System.Windows.Forms.Control)(object)this.combsalesman).TabIndex = 14;
		((System.Windows.Forms.Control)(object)this.combsalesman).Leave += new System.EventHandler(combsalesman_Leave);
		this.txtCommrate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCommrate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtCommrate.Location = new System.Drawing.Point(1117, 46);
		this.txtCommrate.Name = "txtCommrate";
		this.txtCommrate.Size = new System.Drawing.Size(87, 23);
		this.txtCommrate.TabIndex = 16;
		this.txtCommrate.TextChanged += new System.EventHandler(txtCommrate_TextChanged);
		this.txtCommrate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtCommrate_KeyPress);
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.Location = new System.Drawing.Point(375, 78);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(57, 15);
		this.label29.TabIndex = 3;
		this.label29.Text = "Due Date";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(849, 22);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(108, 15);
		this.label6.TabIndex = 0;
		this.label6.Text = "Commission Agent";
		this.CmbCustomer.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomer.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomer.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomer.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance111).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Appearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomer).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance113).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance113;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomer).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance114).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance114).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance114).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance115).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance117).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance118;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance119).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance120).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance120).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance120).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance120).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance121).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance122).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomer).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbCustomer.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomer).Location = new System.Drawing.Point(121, 72);
		((System.Windows.Forms.Control)(object)this.CmbCustomer).Name = "CmbCustomer";
		((System.Windows.Forms.Control)(object)this.CmbCustomer).Size = new System.Drawing.Size(249, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomer).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.CmbCustomer).Leave += new System.EventHandler(combsuppname_Leave);
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(1051, 78);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(51, 15);
		this.label19.TabIndex = 0;
		this.label19.Text = "Amount";
		this.txtcatsr.BackColor = System.Drawing.Color.White;
		this.txtcatsr.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcatsr.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtcatsr.Location = new System.Drawing.Point(292, 46);
		this.txtcatsr.Name = "txtcatsr";
		this.txtcatsr.ReadOnly = true;
		this.txtcatsr.Size = new System.Drawing.Size(75, 23);
		this.txtcatsr.TabIndex = 3;
		this.txtcatsr.TabStop = false;
		this.txtcommamount.BackColor = System.Drawing.Color.White;
		this.txtcommamount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcommamount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtcommamount.Location = new System.Drawing.Point(1117, 74);
		this.txtcommamount.Name = "txtcommamount";
		this.txtcommamount.ReadOnly = true;
		this.txtcommamount.Size = new System.Drawing.Size(87, 23);
		this.txtcommamount.TabIndex = 18;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(1051, 50);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(69, 15);
		this.label11.TabIndex = 0;
		this.label11.Text = "Comm Rate";
		this.txtdeliverydays.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdeliverydays.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtdeliverydays.Location = new System.Drawing.Point(712, 74);
		this.txtdeliverydays.Name = "txtdeliverydays";
		this.txtdeliverydays.Size = new System.Drawing.Size(133, 23);
		this.txtdeliverydays.TabIndex = 13;
		this.txtdeliverydays.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtdeliverydays_KeyPress);
		this.combruom.AllowNull = (DefaultableBoolean)1;
		this.combruom.AutoCompleteMode = (AutoCompleteMode)4;
		this.combruom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combruom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance180).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance180).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combruom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance180;
		((UltraGridBase)this.combruom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combruom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance181).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance181).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance181).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance181).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combruom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance181;
		((AppearanceBase)appearance183).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combruom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance183;
		((SpecialBoxBase)((UltraGridBase)this.combruom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance184).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance184).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance184).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance184).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combruom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance184;
		((UltraGridBase)this.combruom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combruom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance185).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance185).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combruom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance185;
		((AppearanceBase)appearance186).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance186).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combruom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance186;
		((UltraGridBase)this.combruom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combruom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance187).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combruom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance187;
		((AppearanceBase)appearance188).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance188).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combruom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance188;
		((UltraGridBase)this.combruom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combruom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance189).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance189).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance189).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance189).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance189).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combruom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance189;
		((AppearanceBase)appearance190).TextHAlignAsString = "Left";
		((UltraGridBase)this.combruom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance190;
		((UltraGridBase)this.combruom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combruom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance191).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance191).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combruom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance191;
		((UltraGridBase)this.combruom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance192).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combruom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance192;
		((UltraGridBase)this.combruom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combruom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combruom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combruom).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.combruom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combruom).Location = new System.Drawing.Point(958, 72);
		((System.Windows.Forms.Control)(object)this.combruom).Name = "combruom";
		((System.Windows.Forms.Control)(object)this.combruom).Size = new System.Drawing.Size(90, 26);
		((System.Windows.Forms.Control)(object)this.combruom).TabIndex = 17;
		((System.Windows.Forms.Control)(object)this.combruom).Leave += new System.EventHandler(combruom_Leave);
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(849, 50);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(71, 15);
		this.label20.TabIndex = 3;
		this.label20.Text = "Comm Type";
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(849, 107);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(91, 15);
		this.label25.TabIndex = 3;
		this.label25.Text = "Comm Remarks";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(4, 78);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(93, 15);
		this.label12.TabIndex = 0;
		this.label12.Text = "Customer Name";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(849, 78);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(61, 15);
		this.label21.TabIndex = 8;
		this.label21.Text = "Rate UOM";
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(603, 22);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(80, 15);
		this.label30.TabIndex = 8;
		this.label30.Text = "Delivery Term";
		this.txtcommremarks.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtcommremarks.Location = new System.Drawing.Point(958, 103);
		this.txtcommremarks.Name = "txtcommremarks";
		this.txtcommremarks.Size = new System.Drawing.Size(246, 23);
		this.txtcommremarks.TabIndex = 19;
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(375, 50);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(58, 15);
		this.label24.TabIndex = 0;
		this.label24.Text = "Due Days";
		this.combcommtype.AllowNull = (DefaultableBoolean)1;
		this.combcommtype.AutoCompleteMode = (AutoCompleteMode)4;
		this.combcommtype.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combcommtype.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combcommtype).DisplayLayout.Appearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.combcommtype).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combcommtype).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance65).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance65).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combcommtype).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance65;
		((AppearanceBase)appearance66).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combcommtype).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance66;
		((SpecialBoxBase)((UltraGridBase)this.combcommtype).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance67).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance67).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combcommtype).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.combcommtype).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combcommtype).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combcommtype).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance68;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combcommtype).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.combcommtype).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combcommtype).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combcommtype).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance71).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combcommtype).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.combcommtype).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combcommtype).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance72).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance72).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance72).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combcommtype).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).TextHAlignAsString = "Left";
		((UltraGridBase)this.combcommtype).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.combcommtype).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combcommtype).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combcommtype).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.combcommtype).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combcommtype).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.combcommtype).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combcommtype).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combcommtype).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combcommtype).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.combcommtype.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combcommtype).Location = new System.Drawing.Point(958, 44);
		((System.Windows.Forms.Control)(object)this.combcommtype).Name = "combcommtype";
		((System.Windows.Forms.Control)(object)this.combcommtype).Size = new System.Drawing.Size(90, 26);
		((System.Windows.Forms.Control)(object)this.combcommtype).TabIndex = 15;
		((System.Windows.Forms.Control)(object)this.combcommtype).Leave += new System.EventHandler(combcommtype_Leave);
		this.txtsupprefno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtsupprefno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtsupprefno.Location = new System.Drawing.Point(121, 103);
		this.txtsupprefno.Name = "txtsupprefno";
		this.txtsupprefno.Size = new System.Drawing.Size(88, 23);
		this.txtsupprefno.TabIndex = 5;
		this.combdeliverytrm.AllowNull = (DefaultableBoolean)1;
		this.combdeliverytrm.AutoCompleteMode = (AutoCompleteMode)4;
		this.combdeliverytrm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combdeliverytrm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance77).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance78).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance78).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combdeliverytrm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance78;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance79;
		((SpecialBoxBase)((UltraGridBase)this.combdeliverytrm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance80).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance80).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance80).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance81;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance84).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance85).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance85).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance85).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance85).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance87).TextHAlignAsString = "Left";
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance88).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.combdeliverytrm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).Location = new System.Drawing.Point(712, 16);
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).Name = "combdeliverytrm";
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).Size = new System.Drawing.Size(133, 26);
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).TabIndex = 11;
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.Location = new System.Drawing.Point(603, 50);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(106, 15);
		this.label28.TabIndex = 0;
		this.label28.Text = "Delivery Start Date";
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(375, 22);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(89, 15);
		this.label26.TabIndex = 8;
		this.label26.Text = "Payment Terms";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(4, 50);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(118, 15);
		this.label7.TabIndex = 0;
		this.label7.Text = "Order Catagory && No";
		this.txtdocno.BackColor = System.Drawing.Color.White;
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtdocno.Location = new System.Drawing.Point(121, 18);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.Size = new System.Drawing.Size(78, 23);
		this.txtdocno.TabIndex = 0;
		this.txtdocno.TabStop = false;
		this.txtdocno.Leave += new System.EventHandler(txtdocno_TextChanged);
		this.deliverystartdate.CustomFormat = "dd-MMM-yy";
		this.deliverystartdate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.deliverystartdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.deliverystartdate.Location = new System.Drawing.Point(712, 46);
		this.deliverystartdate.Name = "deliverystartdate";
		this.deliverystartdate.Size = new System.Drawing.Size(133, 23);
		this.deliverystartdate.TabIndex = 12;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(210, 107);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(65, 15);
		this.label4.TabIndex = 8;
		this.label4.Text = "Order Type";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(4, 107);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(97, 15);
		this.label8.TabIndex = 3;
		this.label8.Text = "Customer Ref No";
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(4, 22);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(96, 15);
		this.label17.TabIndex = 3;
		this.label17.Text = "Order No && Date";
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.Location = new System.Drawing.Point(603, 78);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(79, 15);
		this.label32.TabIndex = 3;
		this.label32.Text = "Delivery Days";
		this.panel3.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel3.Controls.Add(this.label23);
		this.panel3.Controls.Add(this.ChkBox);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(2, 27);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1222, 23);
		this.panel3.TabIndex = 1;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(8, 2);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(126, 19);
		this.label23.TabIndex = 0;
		this.label23.Text = "Booking Order";
		this.ChkBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkBox.AutoSize = true;
		this.ChkBox.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkBox.ForeColor = System.Drawing.Color.White;
		this.ChkBox.Location = new System.Drawing.Point(1133, 2);
		this.ChkBox.Margin = new System.Windows.Forms.Padding(2);
		this.ChkBox.Name = "ChkBox";
		this.ChkBox.Size = new System.Drawing.Size(84, 20);
		this.ChkBox.TabIndex = 20;
		this.ChkBox.Text = "Preview";
		this.ChkBox.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(2, 2);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1222, 25);
		this.panel2.TabIndex = 0;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[18]
		{
			this.toolStripSeparator3, this.btnnew, this.btnRefresh, this.toolStripSeparator4, this.btnupdate, this.btnsave, this.btnDelete, this.toolStripSeparator5, this.btnprint, this.toolStripSeparator6,
			this.btnattachment, this.toolStripSeparator1, this.BtnPackingPriceForm, this.toolStripSeparator8, this.BtnLoader, this.toolStripSeparator10, this.BtnShortCutkeys, this.btnGenerateCalculations
		});
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1222, 27);
		this.toolStrip1.TabIndex = 0;
		this.toolStrip1.Text = "toolStrip1";
		this.toolStripSeparator3.Name = "toolStripSeparator3";
		this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
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
		this.toolStripSeparator4.Name = "toolStripSeparator4";
		this.toolStripSeparator4.Size = new System.Drawing.Size(6, 27);
		this.btnupdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnupdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnupdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnupdate.Name = "btnupdate";
		this.btnupdate.Size = new System.Drawing.Size(78, 24);
		this.btnupdate.Text = "&Update";
		this.btnupdate.Visible = false;
		this.btnupdate.Click += new System.EventHandler(btnupdate_Click);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnDelete.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.icons8_delete_bin_16;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(73, 24);
		this.btnDelete.Text = "Delete";
		this.btnDelete.Visible = false;
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.toolStripSeparator5.Name = "toolStripSeparator5";
		this.toolStripSeparator5.Size = new System.Drawing.Size(6, 27);
		this.btnprint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnprint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnprint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnprint.Name = "btnprint";
		this.btnprint.Size = new System.Drawing.Size(101, 24);
		this.btnprint.Text = "273A-Print";
		this.btnprint.Click += new System.EventHandler(btnprint_Click);
		this.toolStripSeparator6.Name = "toolStripSeparator6";
		this.toolStripSeparator6.Size = new System.Drawing.Size(6, 27);
		this.btnattachment.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnattachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnattachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnattachment.Name = "btnattachment";
		this.btnattachment.Size = new System.Drawing.Size(109, 24);
		this.btnattachment.Text = "&Attachment";
		this.btnattachment.Click += new System.EventHandler(btnattachment_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.BtnPackingPriceForm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnPackingPriceForm.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnPackingPriceForm.Name = "BtnPackingPriceForm";
		this.BtnPackingPriceForm.Size = new System.Drawing.Size(98, 24);
		this.BtnPackingPriceForm.Text = "Packing Price";
		this.BtnPackingPriceForm.Click += new System.EventHandler(BtnPackingPriceForm_Click);
		this.toolStripSeparator8.Name = "toolStripSeparator8";
		this.toolStripSeparator8.Size = new System.Drawing.Size(6, 27);
		this.BtnLoader.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnLoader.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnLoader.Name = "BtnLoader";
		this.BtnLoader.Size = new System.Drawing.Size(164, 24);
		this.BtnLoader.Text = "Load Pre Booking Order";
		this.BtnLoader.Click += new System.EventHandler(BtnLoader_Click);
		this.toolStripSeparator10.Name = "toolStripSeparator10";
		this.toolStripSeparator10.Size = new System.Drawing.Size(6, 27);
		this.BtnShortCutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShortCutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.BtnShortCutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnShortCutkeys.Name = "BtnShortCutkeys";
		this.BtnShortCutkeys.Size = new System.Drawing.Size(126, 24);
		this.BtnShortCutkeys.Text = "ShortCut Keys";
		this.BtnShortCutkeys.Click += new System.EventHandler(BtnShortCutkeys_Click);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.grdhistory);
		this.tabPage2.Controls.Add(this.panel10);
		this.tabPage2.Controls.Add(this.panel13);
		this.tabPage2.Controls.Add(this.panel7);
		this.tabPage2.Controls.Add(this.panel8);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage2.Size = new System.Drawing.Size(1226, 532);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.grdhistory.AllowDelete = (InheritableBoolean)1;
		((ScrollableBase)this.grdhistory).BackColor = System.Drawing.Color.White;
		grdhistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdhistory.DesignTimeLayout = grdhistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdhistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdhistory.DynamicFiltering = true;
		this.grdhistory.FilterMode = (FilterMode)2;
		this.grdhistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdhistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdhistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdhistory).Location = new System.Drawing.Point(2, 147);
		((System.Windows.Forms.Control)(object)this.grdhistory).Name = "grdhistory";
		((ScrollableBase)this.grdhistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdhistory).Size = new System.Drawing.Size(1222, 177);
		((System.Windows.Forms.Control)(object)this.grdhistory).TabIndex = 0;
		this.grdhistory.TotalRow = (InheritableBoolean)1;
		this.grdhistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdhistory).VisualStyle = (VisualStyle)4;
		this.grdhistory.ColumnButtonClick += new ColumnActionEventHandler(grdhistory_ColumnButtonClick);
		this.grdhistory.SelectionChanged += new System.EventHandler(grdhistory_SelectionChanged);
		this.grdhistory.LinkClicked += new ColumnActionEventHandler(grdhistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdhistory).DoubleClick += new System.EventHandler(grdhistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdhistory).KeyDown += new System.Windows.Forms.KeyEventHandler(grdhistory_KeyDown);
		this.panel10.BackColor = System.Drawing.Color.Teal;
		this.panel10.Controls.Add(this.ctrlGrdBar2);
		this.panel10.Controls.Add(this.label34);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(2, 125);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1222, 22);
		this.panel10.TabIndex = 49;
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label34.ForeColor = System.Drawing.Color.White;
		this.label34.Location = new System.Drawing.Point(5, 3);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(40, 17);
		this.label34.TabIndex = 1;
		this.label34.Text = "Main";
		this.panel13.BackColor = System.Drawing.Color.Teal;
		this.panel13.Controls.Add(this.label27);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel13.Location = new System.Drawing.Point(2, 324);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1222, 22);
		this.panel13.TabIndex = 48;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label27.ForeColor = System.Drawing.Color.White;
		this.label27.Location = new System.Drawing.Point(6, 2);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(211, 17);
		this.label27.TabIndex = 1;
		this.label27.Text = "Detail Of Above Selected Row";
		this.panel7.Controls.Add(this.groupBox2);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(2, 53);
		this.panel7.Margin = new System.Windows.Forms.Padding(2);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1222, 72);
		this.panel7.TabIndex = 0;
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbBranchName);
		this.groupBox2.Controls.Add(this.label41);
		this.groupBox2.Controls.Add(this.rdapproveddate);
		this.groupBox2.Controls.Add(this.rdmodifydate);
		this.groupBox2.Controls.Add(this.drdocdate);
		this.groupBox2.Controls.Add(this.rdentrydate);
		this.groupBox2.Controls.Add(this.btnShowHistory);
		this.groupBox2.Controls.Add(this.label50);
		this.groupBox2.Controls.Add(this.label51);
		this.groupBox2.Controls.Add(this.label52);
		this.groupBox2.Controls.Add(this.label53);
		this.groupBox2.Controls.Add(this.label54);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomerHistory);
		this.groupBox2.Controls.Add(this.ToDocNoHistory);
		this.groupBox2.Controls.Add(this.FromDocNoHistory);
		this.groupBox2.Controls.Add(this.ToDateHistory);
		this.groupBox2.Controls.Add(this.FromDateHistory);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(3, 2);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(1212, 68);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Filters";
		this.cmbBranchName.AllowNull = (DefaultableBoolean)1;
		this.cmbBranchName.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbBranchName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbBranchName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance49).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance49;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance53).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance64).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance64;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance75).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance86;
		((AppearanceBase)appearance91).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance91).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance97).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance97).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance97).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance97).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance97;
		((AppearanceBase)appearance108).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance109).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbBranchName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Location = new System.Drawing.Point(5, 36);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Name = "cmbBranchName";
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Size = new System.Drawing.Size(212, 26);
		((System.Windows.Forms.Control)(object)this.cmbBranchName).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbBranchName).Leave += new System.EventHandler(cmbBranchName_Leave);
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label41.Location = new System.Drawing.Point(5, 19);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(79, 15);
		this.label41.TabIndex = 3;
		this.label41.Text = "Branch Name";
		this.rdapproveddate.AutoSize = true;
		this.rdapproveddate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdapproveddate.Location = new System.Drawing.Point(1108, 40);
		this.rdapproveddate.Name = "rdapproveddate";
		this.rdapproveddate.Size = new System.Drawing.Size(105, 19);
		this.rdapproveddate.TabIndex = 10;
		this.rdapproveddate.Text = "Approved Date";
		this.rdapproveddate.UseVisualStyleBackColor = true;
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(1108, 17);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 9;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(1027, 17);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 7;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(1027, 40);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 8;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.btnShowHistory.BackColor = System.Drawing.Color.Teal;
		this.btnShowHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShowHistory.ForeColor = System.Drawing.Color.White;
		this.btnShowHistory.Location = new System.Drawing.Point(960, 36);
		this.btnShowHistory.Margin = new System.Windows.Forms.Padding(2);
		this.btnShowHistory.Name = "btnShowHistory";
		this.btnShowHistory.Size = new System.Drawing.Size(62, 26);
		this.btnShowHistory.TabIndex = 6;
		this.btnShowHistory.Text = "Show";
		this.btnShowHistory.UseVisualStyleBackColor = false;
		this.btnShowHistory.Click += new System.EventHandler(btnShowHistory_Click);
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label50.Location = new System.Drawing.Point(702, 19);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(58, 15);
		this.label50.TabIndex = 87;
		this.label50.Text = "Customer";
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label51.Location = new System.Drawing.Point(601, 19);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(64, 15);
		this.label51.TabIndex = 86;
		this.label51.Text = "To Doc No";
		this.label52.AutoSize = true;
		this.label52.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label52.Location = new System.Drawing.Point(496, 19);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(79, 15);
		this.label52.TabIndex = 85;
		this.label52.Text = "From Doc No";
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label53.Location = new System.Drawing.Point(358, 19);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(48, 15);
		this.label53.TabIndex = 84;
		this.label53.Text = "To Date";
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label54.Location = new System.Drawing.Point(217, 19);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(63, 15);
		this.label54.TabIndex = 83;
		this.label54.Text = "From Date";
		this.CmbCustomerHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomerHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomerHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomerHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance144).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance145).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance145).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance145).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance145;
		((AppearanceBase)appearance146).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance146;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance148).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance148).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance148).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance148;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance150).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance150;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance151).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance154;
		((AppearanceBase)appearance155).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance155).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance156).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance156).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance156).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance156).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance156;
		((AppearanceBase)appearance157).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance158).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance158;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomerHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Location = new System.Drawing.Point(705, 36);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Name = "CmbCustomerHistory";
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Size = new System.Drawing.Size(252, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).TabIndex = 5;
		this.ToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.ToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDocNoHistory.Location = new System.Drawing.Point(601, 38);
		this.ToDocNoHistory.Name = "ToDocNoHistory";
		this.ToDocNoHistory.ShortcutsEnabled = false;
		this.ToDocNoHistory.Size = new System.Drawing.Size(102, 23);
		this.ToDocNoHistory.TabIndex = 4;
		this.ToDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(ToDocNoHistory_KeyPress);
		this.FromDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.FromDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDocNoHistory.Location = new System.Drawing.Point(496, 38);
		this.FromDocNoHistory.Name = "FromDocNoHistory";
		this.FromDocNoHistory.ShortcutsEnabled = false;
		this.FromDocNoHistory.Size = new System.Drawing.Size(102, 23);
		this.FromDocNoHistory.TabIndex = 3;
		this.FromDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(FromDocNoHistory_KeyPress);
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(358, 38);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(136, 23);
		this.ToDateHistory.TabIndex = 2;
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(220, 38);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(136, 23);
		this.FromDateHistory.TabIndex = 1;
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.label22);
		this.panel8.Controls.Add(this.toolStrip2);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(2, 2);
		this.panel8.Margin = new System.Windows.Forms.Padding(2);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1222, 51);
		this.panel8.TabIndex = 0;
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.ForeColor = System.Drawing.Color.White;
		this.label22.Location = new System.Drawing.Point(6, 29);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(189, 19);
		this.label22.TabIndex = 22;
		this.label22.Text = "Booking Order History";
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnResetHistory, this.btnRefreshHistory });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1222, 27);
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
		this.DataGridHistoryDetail.AllowDelete = (InheritableBoolean)1;
		((ScrollableBase)this.DataGridHistoryDetail).BackColor = System.Drawing.Color.White;
		DataGridHistoryDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistoryDetail.DesignTimeLayout = DataGridHistoryDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail).Dock = System.Windows.Forms.DockStyle.Bottom;
		this.DataGridHistoryDetail.DynamicFiltering = true;
		this.DataGridHistoryDetail.FilterMode = (FilterMode)2;
		this.DataGridHistoryDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistoryDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail).Location = new System.Drawing.Point(2, 346);
		((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail).Name = "DataGridHistoryDetail";
		((ScrollableBase)this.DataGridHistoryDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail).Size = new System.Drawing.Size(1222, 184);
		((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail).TabIndex = 1;
		this.DataGridHistoryDetail.TotalRow = (InheritableBoolean)1;
		this.DataGridHistoryDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistoryDetail).VisualStyle = (VisualStyle)4;
		this.btnGenerateCalculations.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnGenerateCalculations.Image = (System.Drawing.Image)resources.GetObject("btnGenerateCalculations.Image");
		this.btnGenerateCalculations.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnGenerateCalculations.Name = "btnGenerateCalculations";
		this.btnGenerateCalculations.Size = new System.Drawing.Size(71, 24);
		this.btnGenerateCalculations.Text = "Generate";
		this.btnGenerateCalculations.Visible = false;
		this.btnGenerateCalculations.Click += new System.EventHandler(btnGenerateCalculations_Click);
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1171, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 21);
		this.ctrlGrdBar1.TabIndex = 20;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1173, 1);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdhistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(43, 23);
		this.ctrlGrdBar2.TabIndex = 21;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(1234, 561);
		base.Controls.Add(this.panel1);
		base.HelpButton = true;
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "BookingOrder";
		this.Text = "Sale Order";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(BookingOrder_FormClosing);
		base.Load += new System.EventHandler(BookingOrder_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(PurchsaeOrder_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbItem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbReferenceParty).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPackType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCropyr).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combcityarea).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combitempck).EndInit();
		this.panel4.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.combpttrm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbOrderType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combordercat).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combsalesman).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomer).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combruom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combcommtype).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combdeliverytrm).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdhistory).EndInit();
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		this.panel13.ResumeLayout(false);
		this.panel13.PerformLayout();
		this.panel7.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbBranchName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistoryDetail).EndInit();
		base.ResumeLayout(false);
	}
}

}
