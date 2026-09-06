// using Architecture.Reports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Export;
using Architecture.BLL.Import;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Common;
using Architecture.Model.Export;
using Architecture.Model.Import;
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
namespace Architecture.WinApp.Import
{

public class ImLcOrder : Form
{
	private Rightsobjects formrights = new Rightsobjects();

	private Attachment AT = new Attachment();

	public UserAccount UserAccount = new UserAccount();

	private int RecId;

	private bool UpdateMode;

	private bool ItemSearchWithNameOrCode;

	private int updateDetailIndex = -1;

	private int updateDetailIndexPaymentDetail = -1;

	private DataTable dtdetail = new DataTable();

	private DataTable dtItem = new DataTable();

	private DataTable dtPaymnent = new DataTable();

	private DropDownBind DDL = new DropDownBind();

	private List<ImLcOrderPackingDetail> lstRemoveRecord = new List<ImLcOrderPackingDetail>();

	private IContainer components;

	private TabPage History;

	private Panel panel1;

	private TabControl tabControl1;

	private BackgroundWorker backgroundWorker1;

	private ComboBox cmbexporterbank;

	private TabPage Form;

	private Panel panel2;

	private Panel panel6;

	private Panel panel5;

	internal UltraCombo cmblegalization;

	internal UltraCombo cmbInspectionRequired;

	internal UltraCombo cmbCurreny;

	internal UltraCombo cmbDestinationPort;

	internal UltraCombo cmbLoadingPort;

	internal UltraCombo cmbExporterBanks;

	internal UltraCombo CmbImporterBank;

	internal UltraCombo cmbPaymenTerm;

	internal UltraCombo cmbDeliveryTerm;

	internal UltraCombo cmbSupplier;

	internal UltraCombo cmbproject;

	internal UltraCombo cmbbranch;

	private GroupBox groupBox1;

	internal UltraCombo cmbRateUom;

	internal UltraCombo cmbItemUom;

	internal UltraCombo cmbPackingType;

	internal UltraCombo cmbItem;

	private TextBox txtQtyDetail;

	private Button btnCancel;

	private Button btnGridUpdate;

	private Button btnaddnew;

	private Label label47;

	private Label label48;

	private Label label49;

	private Label label50;

	private TextBox txtFcyAmountDetail;

	private Label label52;

	private TextBox txtItemRate;

	private Label label53;

	private TextBox txtWeightDetail;

	private Label label54;

	private TextBox txtItemDescription;

	private Label label55;

	private DateTimePicker txtExpiryDate;

	private Label label12;

	private DateTimePicker txtLastShipmentDate;

	private Label label14;

	private DateTimePicker txtdocdate;

	private Label label5;

	private Label label38;

	private TextBox txtExpiryPlace;

	private TextBox txtSupLcOrderNo;

	private TextBox txtInquiryRef;

	private TextBox txtLcyAmountMain;

	private TextBox txtNetWeightMain;

	private TextBox txtGrossWeightMain;

	private TextBox txtExchangeRate;

	private TextBox txtTotalFcyAmountMain;

	private TextBox txtQuorationRef;

	private TextBox txtShippingMarks;

	private TextBox txtInspectionDesc;

	private TextBox txtLegalizationDesc;

	private TextBox txtNoOfContainerMain;

	private Label label41;

	private Label label44;

	private TextBox txtCommodityDescMain;

	private Label label39;

	private TextBox txtdocno;

	private Label label20;

	private Label label16;

	private Label label24;

	private Label label22;

	private Label label19;

	private Label label21;

	private Label label17;

	private Label label36;

	private Label label31;

	private Label label35;

	private Label label18;

	private Label label26;

	private Label label8;

	private Label label25;

	private Label label15;

	private Label label4;

	private Label label7;

	private Label label43;

	private Label label40;

	private Label label34;

	private Label label37;

	private Label label32;

	private Label label13;

	private Label label2;

	private Label label42;

	private Panel panel4;

	private Button btnclose;

	private Label label23;

	private CtrlGrdBar ctrlGrdBar1;

	private Panel panel3;

	private ToolStrip toolStrip2;

	private ToolStripButton btnNew;

	private ToolStripButton btnSave;

	private ToolStripButton btnUpdate;

	private ToolStripButton Print;

	private CtrlGrdBar ctrlGrdBar3;

	private TextBox cmbTransShipment;

	private TextBox cmbPartialShipment;

	private Label label72;

	private Label label70;

	private Label label69;

	private Label label68;

	private Label label67;

	private Label label66;

	private Label label65;

	private Label label64;

	private Label label63;

	private Label label62;

	private Label label61;

	private Label label60;

	private Label label58;

	private Label label57;

	private Label label1;

	private Label label80;

	private Label label79;

	private Label label78;

	private Label label77;

	private Label label74;

	internal UltraCombo cmbLegalizationRequired;

	internal UltraCombo cmbOrderStatus;

	private Label label28;

	private Label label27;

	private Label label29;

	private CtrlGrdBar ctrlGrdBar2;

	private Label label30;

	private TextBox txtRemarksDetail;

	private GroupBox groupBox2;

	private RichTextBox txtRemarksHeader;

	internal GridEX DataGridHistory;

	private Panel panel9;

	private Label label56;

	internal GridEX grdHistoryDetail;

	private Panel panel8;

	private Label label59;

	private CtrlGrdBar ctrlGrdBar4;

	private ToolStrip toolStrip1;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripButton btnLoadAllHistory;

	private Label label81;

	private Label label11;

	private DateTimePicker txtSupLcOrderDate;

	internal UltraCombo cmbShippedTo;

	private Label label82;

	internal UltraCombo cmbNotifyParty;

	private Label label9;

	internal UltraCombo cmbSalesPerson;

	private Label label83;

	private ToolStripButton btnRefresh;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripSeparator toolStripSeparator3;

	private ToolStripSeparator toolStripSeparator4;

	private ToolStripButton btnAttachment;

	private ToolStripSeparator toolStripSeparator5;

	private ToolStripButton BtnShortCutkeys;

	private ToolStripButton btnDelete;

	private ToolStripButton BtnSaveAs;

	private TabControl tabControl2;

	private TabPage Detail;

	internal GridEX grdDetail;

	private Panel panel7;

	private Label label33;

	private Panel panel10;

	private TabPage Payment;

	private Panel panel14;

	private Button btnCancelPayment;

	private Button btnUpdatePayment;

	private Button btnAddPayment;

	private Label label3;

	private TextBox txtPaymentDetailFcyAmount;

	private TextBox PercntOfTotal;

	private Label label6;

	private Label label10;

	internal UltraCombo CmbPaymentTermDetail;

	private RadioButton rdSearchByCode;

	private RadioButton rdSearchByName;

	private Label label46;

	private ToolStripButton btnSeaPortDefine;

	private ToolStripButton btnDefinePaymentTerm;

	private ToolStripButton toolStripButton2;

	internal GridEX grdPaymentTerm;

	private Panel panel11;

	private Label label45;

	public ImLcOrder(UserAccount pUserAccount)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUserAccount;
	}

	private void LcOrder_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1200, 700);
			formrights = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnSave.Enabled = formrights.DoHaveSaveRight;
			btnUpdate.Enabled = formrights.DoHaveUpdateRights;
			Print.Enabled = formrights.DoHavePrintRights;
			ctrlGrdBar1.mGridChooseFielder.Enabled = formrights.DoHaveFieldChooserRights;
			ctrlGrdBar1.mGridSaveLayouts.Enabled = formrights.DoHaveSaveLayoutRights;
			ctrlGrdBar1.mGridPrint.Enabled = formrights.DoHaveGridPrintRights;
			ctrlGrdBar1.mGridExport.Enabled = formrights.DoHaveGridExportRights;
			ctrlGrdBar1.GroupCollaps.Enabled = formrights.DoHaveGridGroupCollapseRights;
			ctrlGrdBar1.GroupExpand.Enabled = formrights.DoHaveGridGroupExpandRights;
			ctrlGrdBar2.mGridChooseFielder.Enabled = formrights.DoHaveFieldChooserRights;
			ctrlGrdBar2.mGridSaveLayouts.Enabled = formrights.DoHaveSaveLayoutRights;
			ctrlGrdBar2.mGridPrint.Enabled = formrights.DoHaveGridPrintRights;
			ctrlGrdBar2.mGridExport.Enabled = formrights.DoHaveGridExportRights;
			ctrlGrdBar2.GroupCollaps.Enabled = formrights.DoHaveGridGroupCollapseRights;
			ctrlGrdBar2.GroupExpand.Enabled = formrights.DoHaveGridGroupExpandRights;
			ctrlGrdBar3.mGridChooseFielder.Enabled = formrights.DoHaveFieldChooserRights;
			ctrlGrdBar3.mGridSaveLayouts.Enabled = formrights.DoHaveSaveLayoutRights;
			ctrlGrdBar3.mGridPrint.Enabled = formrights.DoHaveGridPrintRights;
			ctrlGrdBar3.mGridExport.Enabled = formrights.DoHaveGridExportRights;
			ctrlGrdBar3.GroupCollaps.Enabled = formrights.DoHaveGridGroupCollapseRights;
			ctrlGrdBar3.GroupExpand.Enabled = formrights.DoHaveGridGroupExpandRights;
			dtItem.Columns.Add("Id", typeof(int));
			dtItem.Columns.Add("ItemName", typeof(string));
			dtItem.Columns.Add("ItemCode", typeof(string));
			dtPaymnent.Columns.Add("Id", typeof(int));
			dtPaymnent.Columns.Add("PaymentTermId", typeof(int));
			dtPaymnent.Columns.Add("PaymentTerm", typeof(string));
			dtPaymnent.Columns.Add("%ofTotal", typeof(double));
			dtPaymnent.Columns.Add("FcyAmount", typeof(double));
			grdPaymentTerm.DataSource = dtPaymnent;
			grdPaymentTerm.RetrieveStructure();
			grdSettingsPaymentTerm();
			dtdetail.Columns.Add("Id", typeof(int));
			dtdetail.Columns.Add("ItemId", typeof(int));
			dtdetail.Columns.Add("ItemName");
			dtdetail.Columns.Add("ItemCode");
			dtdetail.Columns.Add("Description");
			dtdetail.Columns.Add("PackingTypeId", typeof(int));
			dtdetail.Columns.Add("PackingType");
			dtdetail.Columns.Add("ItemUomId", typeof(int));
			dtdetail.Columns.Add("ItemUom");
			dtdetail.Columns.Add("Qty", typeof(decimal));
			dtdetail.Columns.Add("NetWeight", typeof(decimal));
			dtdetail.Columns.Add("ItemRate", typeof(decimal));
			dtdetail.Columns.Add("RateUomId", typeof(int));
			dtdetail.Columns.Add("RateUom");
			dtdetail.Columns.Add("FcyAmount", typeof(decimal));
			dtdetail.Columns.Add("LcyAmount", typeof(decimal));
			dtdetail.Columns.Add("RemarksDetail");
			grdDetail.DataSource = dtdetail;
			grdDetail.RetrieveStructure();
			GrdSetting();
			DocNo();
			BranchFill();
			Project();
			SupplierFill();
			salesPersonFill();
			LoadingPortFill();
			ImporterandExportBankFill();
			LegalizationFill();
			InspectionFill();
			PaymentTermsFill();
			MultiCurrencyfill();
			DeliveryTermFill();
			StatusFill();
			ItemDetailFill();
			ItemPacktype();
			ItemSearchWithNameOrCode = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("ItemSearchByCode"));
			if (ItemSearchWithNameOrCode)
			{
				rdSearchByCode.Checked = true;
			}
			else
			{
				rdSearchByName.Checked = true;
			}
			txtdocdate.Focus();
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
			DataTable dtBranch = CommonServices.BrancheServiceBind();
			if (dtBranch.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtBranch, cmbbranch, "Id", "BranchName", "Branch", false);
				((UltraGridBase)cmbbranch).Rows[0].Activate();
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
			DataTable dtProject = CommonServices.ProjectServiceBind();
			if (dtProject.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtProject, cmbproject, "Id", "ProjectName", "Project", false);
				((UltraGridBase)cmbproject).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DocNo()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		try
		{
			int code = ImLcOrder.GenerateCode(new ImLcOrder
			{
				CompanyId = UserAccount.CompanyId,
				OrganizationId = UserAccount.OrganizationId,
				BranchesId = UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeId = 231
			});
			if (Conversion.ToString((object)code) != null || code != 0)
			{
				txtdocno.Text = code.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SupplierFill()
	{
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Expected O, but got Unknown
		try
		{
			int customerId = Conversion.ToInt(cmbSupplier.Value);
			int NotifyId = Conversion.ToInt(cmbNotifyParty.Value);
			int ShippedTo = Conversion.ToInt(cmbShippedTo.Value);
			DataTable dtSupplier = SupplierCustomer.ReadByOrganizationCompanyIdForExport(new SupplierCustomer
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			DDL.BindDDLNew(dtSupplier, cmbSupplier, "Id", "CompanyName", "Customer", true);
			DDL.BindDDLNew(dtSupplier, cmbNotifyParty, "Id", "CompanyName", "NotifyParty", true);
			DDL.BindDDLNew(dtSupplier, cmbShippedTo, "Id", "CompanyName", "NotifyParty", true);
			((UltraGridBase)cmbSupplier).DisplayLayout.Bands[0].Columns[1].Width = 300;
			((UltraGridBase)cmbNotifyParty).DisplayLayout.Bands[0].Columns[1].Width = 300;
			((UltraGridBase)cmbShippedTo).DisplayLayout.Bands[0].Columns[1].Width = 300;
			if (dtSupplier.Rows.Count <= 0)
			{
				return;
			}
			if (customerId > 0)
			{
				if (dtSupplier.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == customerId))
				{
					cmbSupplier.Value = customerId;
				}
				else
				{
					((Control)(object)cmbSupplier).Text = string.Empty;
				}
			}
			if (NotifyId > 0)
			{
				if (dtSupplier.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == NotifyId))
				{
					cmbNotifyParty.Value = NotifyId;
				}
				else
				{
					((Control)(object)cmbNotifyParty).Text = string.Empty;
				}
			}
			if (ShippedTo > 0)
			{
				if (dtSupplier.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ShippedTo))
				{
					cmbShippedTo.Value = ShippedTo;
				}
				else
				{
					((Control)(object)cmbShippedTo).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void salesPersonFill()
	{
		try
		{
			int id = Conversion.ToInt(cmbSalesPerson.Value);
			DataTable dtSalePerson = CommonServices.GetSupplierustomerByCustomerGroupId("9");
			DDL.BindDDL(dtSalePerson, cmbSalesPerson, "Id", "CompanyName", "Sales Person", ZeroIndex: true);
			((UltraGridBase)cmbSalesPerson).DisplayLayout.Bands[0].Columns[1].Width = 300;
			if (dtSalePerson.Rows.Count > 0 && id > 0)
			{
				if (dtSalePerson.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
				{
					cmbSalesPerson.Value = id;
				}
				else
				{
					((Control)(object)cmbSalesPerson).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeliveryTermFill()
	{
		try
		{
			int id = Conversion.ToInt(cmbDeliveryTerm.Value);
			DataTable dt = ExImDeliveryTerm.Getall();
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDLNew(dt, cmbDeliveryTerm, "Id", "Code", "Delivery Term", true);
			((UltraGridBase)cmbDeliveryTerm).Rows[1].Activate();
			if (dt.Rows.Count > 0 && id > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
				{
					cmbDeliveryTerm.Value = id;
				}
				else
				{
					((Control)(object)cmbDeliveryTerm).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PaymentTermsFill()
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		try
		{
			int termId = Conversion.ToInt(cmbPaymenTerm.Value);
			int termIdDetail = Conversion.ToInt(CmbPaymentTermDetail.Value);
			DataTable dtDeliveryTerms = ExImLcPaymentTerm.Getall(new ExImLcPaymentTerm
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtDeliveryTerms.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDLNew(dtDeliveryTerms, cmbPaymenTerm, "Id", "lcOrderTerm", "Payment Term", true);
			DDL.BindDDLNew(dtDeliveryTerms, CmbPaymentTermDetail, "Id", "lcOrderTerm", "Payment Term", true);
			if (dtDeliveryTerms.Rows.Count <= 0)
			{
				return;
			}
			if (termId > 0)
			{
				if (dtDeliveryTerms.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == termId))
				{
					cmbPaymenTerm.Value = termId;
				}
				else
				{
					((Control)(object)cmbPaymenTerm).Text = string.Empty;
				}
			}
			if (termIdDetail > 0)
			{
				if (dtDeliveryTerms.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == termIdDetail))
				{
					CmbPaymentTermDetail.Value = termIdDetail;
				}
				else
				{
					((Control)(object)CmbPaymentTermDetail).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadingPortFill()
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		try
		{
			int loadId = Conversion.ToInt(cmbLoadingPort.Value);
			int destId = Conversion.ToInt(cmbDestinationPort.Value);
			DataTable dt = SeaPorts.Getall(new SeaPorts
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDLNew(dt, cmbLoadingPort, "Id", "PortName", "Loading Port", true);
			DDL.BindDDLNew(dt, cmbDestinationPort, "Id", "PortName", "Destination Port", true);
			((UltraGridBase)cmbDestinationPort).Rows[1].Activate();
			((UltraGridBase)cmbLoadingPort).Rows[1].Activate();
			if (loadId > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == loadId))
				{
					cmbLoadingPort.Value = loadId;
				}
				else
				{
					((Control)(object)cmbLoadingPort).Text = string.Empty;
				}
			}
			if (destId > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == destId))
				{
					cmbDestinationPort.Value = destId;
				}
				else
				{
					((Control)(object)cmbDestinationPort).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void MultiCurrencyfill()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		try
		{
			int id = Conversion.ToInt(cmbCurreny.Value);
			DataTable dt = MultiCurrency.GetAll(new MultiCurrency
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDLNew(dt, cmbCurreny, "Id", "CurrencyCode", "Curreny Code", true);
			((UltraGridBase)cmbCurreny).Rows[1].Activate();
			if (dt.Rows.Count > 0 && id > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
				{
					cmbCurreny.Value = id;
				}
				else
				{
					((Control)(object)cmbCurreny).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ImporterandExportBankFill()
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		try
		{
			int foreign = Conversion.ToInt(CmbImporterBank.Value);
			int home = Conversion.ToInt(cmbExporterBanks.Value);
			List<Bank> lst = Bank.GetAll(new Bank
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			DataTable dtForeign = new DataTable();
			dtForeign.Columns.Add("Id", typeof(int));
			dtForeign.Columns.Add("BranchName");
			DataTable dtHome = new DataTable();
			dtHome.Columns.Add("Id", typeof(int));
			dtHome.Columns.Add("BranchName");
			if (lst.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < lst.Count; i++)
			{
				if (lst[i].IsHomeland == "Foreign Country")
				{
					dtForeign.Rows.Add(lst[i].Id, lst[i].BranchName);
				}
				else if (lst[i].IsHomeland == "Home Country")
				{
					dtHome.Rows.Add(lst[i].Id, lst[i].BranchName);
				}
			}
			DDL.BindDDL(dtForeign, CmbImporterBank, "Id", "BranchName", "Importer Bank", ZeroIndex: true);
			if (dtForeign.Rows.Count > 0 && foreign > 0)
			{
				if (dtForeign.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == foreign))
				{
					CmbImporterBank.Value = foreign;
				}
				else
				{
					((Control)(object)CmbImporterBank).Text = string.Empty;
				}
			}
			DDL.BindDDL(dtHome, cmbExporterBanks, "Id", "BranchName", "Exporter Bank", ZeroIndex: true);
			if (dtHome.Rows.Count > 0 && home > 0)
			{
				if (dtHome.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == home))
				{
					cmbExporterBanks.Value = home;
				}
				else
				{
					((Control)(object)cmbExporterBanks).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LegalizationFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Name");
			dt.Rows.Add(1, "Required");
			dt.Rows.Add(2, "Non-Required");
			DDL.BindDDL(dt, cmbLegalizationRequired, "Id", "Name", "Legalization", ZeroIndex: true);
			((UltraGridBase)cmbLegalizationRequired).Rows[1].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InspectionFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Name");
			dt.Rows.Add(1, "Required");
			dt.Rows.Add(2, "Non-Required");
			DDL.BindDDL(dt, cmbInspectionRequired, "Id", "Name", "Inspection", ZeroIndex: true);
			((UltraGridBase)cmbInspectionRequired).Rows[1].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void StatusFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Name");
			dt.Rows.Add(1, "Open");
			dt.Rows.Add(2, "Complete");
			dt.Rows.Add(3, "Cancel");
			DDL.BindDDL(dt, cmbOrderStatus, "Id", "Name", "Status", ZeroIndex: true);
			((UltraGridBase)cmbOrderStatus).Rows[1].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void bindRateUomAndItemPackUom()
	{
		try
		{
			int Packuom = Conversion.ToInt(cmbItemUom.Value);
			int Rateuom = Conversion.ToInt(cmbRateUom.Value);
			((Control)(object)cmbItemUom).Text = string.Empty;
			((Control)(object)cmbRateUom).Text = string.Empty;
			DataTable dtUom = CommonServices.GetUomScheduleByItemId(Conversion.ToInt(cmbItem.Value));
			if (dtUom.Rows.Count > 0)
			{
				DDL.BindDDL(dtUom, cmbItemUom, "Id", "UOMCode", "PackUom", ZeroIndex: true);
				DDL.BindDDL(dtUom, cmbRateUom, "Id", "UOMCode", "RateUom", ZeroIndex: true);
				if (Packuom > 0)
				{
					if (dtUom.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Packuom))
					{
						cmbItemUom.Value = Packuom;
					}
					else
					{
						((Control)(object)cmbItemUom).Text = string.Empty;
					}
				}
				if (Rateuom > 0)
				{
					if (dtUom.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Rateuom))
					{
						cmbRateUom.Value = Rateuom;
					}
					else
					{
						((Control)(object)cmbRateUom).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)cmbItemUom).Text = string.Empty;
				((UltraGridBase)cmbItemUom).DataSource = null;
				((Control)(object)cmbRateUom).Text = string.Empty;
				((UltraGridBase)cmbRateUom).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemDetailFill()
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		try
		{
			int id = Conversion.ToInt(cmbItem.Value);
			dtItem.Rows.Clear();
			DataTable dt = new DataTable();
			dt = Item.ReadAllForExportCombo(new Item
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				dtItem.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ItemName"], dt.Rows[i]["ItemCode"]);
			}
			if (rdSearchByName.Checked)
			{
				DDL.BindDDL(dtItem, cmbItem, "Id", "ItemName", "Item Name", ZeroIndex: true);
			}
			else
			{
				DDL.BindDDL(dtItem, cmbItem, "Id", "ItemCode", "Item Code", ZeroIndex: true);
			}
			((UltraGridBase)cmbItem).DisplayLayout.Bands[0].Columns[1].Width = 400;
			if (id > 0)
			{
				if (dtItem.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
				{
					cmbItem.Value = id;
				}
				else
				{
					((Control)(object)cmbItem).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbitem_Leave(object sender, EventArgs e)
	{
		try
		{
			bindRateUomAndItemPackUom();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemPacktype()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		try
		{
			int id = Conversion.ToInt(cmbPackingType.Value);
			DataTable dtItemPack = ExImPackMaterilaType.Getall(new ExImPackMaterilaType
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtItemPack.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtItemPack, cmbPackingType, "Id", "Description", "Packing Type", true);
				if (id > 0)
				{
					cmbPackingType.Value = id;
				}
				else
				{
					((Control)(object)cmbPackingType).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void FormReset()
	{
		try
		{
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			RecId = 0;
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			UpdateMode = false;
			txtFcyAmountDetail.Text = string.Empty;
			txtCommodityDescMain.Text = string.Empty;
			txtSupLcOrderNo.Text = string.Empty;
			txtItemDescription.Text = string.Empty;
			txtdocdate.Value = DateTime.Now;
			txtSupLcOrderDate.Value = DateTime.Now;
			txtdocno.Text = string.Empty;
			txtExchangeRate.Text = string.Empty;
			txtExpiryDate.Text = string.Empty;
			txtExpiryPlace.Text = string.Empty;
			txtGrossWeightMain.Text = string.Empty;
			txtInquiryRef.Text = string.Empty;
			txtLastShipmentDate.Text = string.Empty;
			txtLegalizationDesc.Text = string.Empty;
			txtLcyAmountMain.Text = string.Empty;
			txtNetWeightMain.Text = string.Empty;
			txtNoOfContainerMain.Text = string.Empty;
			txtQuorationRef.Text = string.Empty;
			txtItemRate.Text = string.Empty;
			txtRemarksHeader.Text = string.Empty;
			txtShippingMarks.Text = string.Empty;
			txtTotalFcyAmountMain.Text = string.Empty;
			txtWeightDetail.Text = string.Empty;
			((Control)(object)cmbSupplier).Text = string.Empty;
			((Control)(object)cmbSalesPerson).Text = string.Empty;
			((Control)(object)cmbShippedTo).Text = string.Empty;
			((Control)(object)cmbNotifyParty).Text = string.Empty;
			((Control)(object)cmbDeliveryTerm).Text = string.Empty;
			((Control)(object)cmbDestinationPort).Text = string.Empty;
			((Control)(object)cmbExporterBanks).Text = string.Empty;
			((Control)(object)cmbCurreny).Text = string.Empty;
			((Control)(object)CmbImporterBank).Text = string.Empty;
			((Control)(object)cmbPackingType).Text = string.Empty;
			((Control)(object)cmbItemUom).Text = string.Empty;
			txtQtyDetail.Text = string.Empty;
			((Control)(object)cmbInspectionRequired).Text = string.Empty;
			txtInspectionDesc.Text = string.Empty;
			cmbTransShipment.Text = string.Empty;
			txtdocdate.Focus();
			cmbPartialShipment.Text = string.Empty;
			((Control)(object)cmbPaymenTerm).Text = string.Empty;
			((Control)(object)cmbLegalizationRequired).Text = string.Empty;
			((Control)(object)cmbLoadingPort).Text = string.Empty;
			((Control)(object)cmbOrderStatus).Text = string.Empty;
			FormResetDetail();
			grdDetail.ClearStructure();
			dtdetail.Clear();
			ResetPaymentDetail();
			grdPaymentTerm.ClearStructure();
			dtPaymnent.Clear();
			DocNo();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FormResetDetail()
	{
		try
		{
			updateDetailIndex = -1;
			((Control)(object)cmbItem).Text = string.Empty;
			txtItemDescription.Text = string.Empty;
			((Control)(object)cmbPackingType).Text = string.Empty;
			txtQtyDetail.Text = string.Empty;
			((Control)(object)cmbItemUom).Text = string.Empty;
			txtWeightDetail.Text = string.Empty;
			txtItemRate.Text = string.Empty;
			((Control)(object)cmbRateUom).Text = string.Empty;
			txtFcyAmountDetail.Text = string.Empty;
			btnaddnew.Visible = true;
			btnGridUpdate.Visible = false;
			btnCancel.Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		try
		{
			FormReset();
			FormResetDetail();
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
			BranchFill();
			Project();
			SupplierFill();
			salesPersonFill();
			LoadingPortFill();
			ImporterandExportBankFill();
			LegalizationFill();
			InspectionFill();
			PaymentTermsFill();
			MultiCurrencyfill();
			DeliveryTermFill();
			StatusFill();
			ItemDetailFill();
			ItemPacktype();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void ResetPaymentDetail()
	{
		try
		{
			updateDetailIndexPaymentDetail = -1;
			((Control)(object)CmbPaymentTermDetail).Text = string.Empty;
			PercntOfTotal.Text = string.Empty;
			txtPaymentDetailFcyAmount.Text = string.Empty;
			btnAddPayment.Visible = true;
			btnUpdatePayment.Visible = false;
			btnCancelPayment.Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ImLcOrder_FormClosed(object sender, FormClosedEventArgs e)
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

	private bool FormValidation()
	{
		if (txtdocno.Text == string.Empty || Conversion.ToInt((object)txtdocno.Text) == 0)
		{
			MessageBox.Show("Doc No Field is Required!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			txtdocno.Focus();
			return false;
		}
		if (((UltraGridBase)cmbSupplier).ActiveRow == null || Conversion.ToInt(cmbSupplier.Value) == 0)
		{
			MessageBox.Show("Supplier Field is Required!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			cmbSupplier.Focus();
			return false;
		}
		if (txtSupLcOrderNo.Text == string.Empty || txtSupLcOrderNo.Text == "0")
		{
			MessageBox.Show("Supplier Order No Field is Required!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			cmbSupplier.Focus();
			return false;
		}
		if (((UltraGridBase)cmbSalesPerson).ActiveRow == null || Conversion.ToInt(cmbSalesPerson.Value) == 0)
		{
			MessageBox.Show("SalesPerson Field is Required!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			cmbSalesPerson.Focus();
			return false;
		}
		if (((UltraGridBase)cmbLoadingPort).ActiveRow == null || Conversion.ToInt(cmbLoadingPort.Value) == 0)
		{
			MessageBox.Show("Loading Port Field is Required!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			cmbLoadingPort.Focus();
			return false;
		}
		if (((UltraGridBase)cmbDestinationPort).ActiveRow == null || Conversion.ToInt(cmbDestinationPort.Value) == 0)
		{
			MessageBox.Show("Destination Port Field is Required!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			cmbDestinationPort.Focus();
			return false;
		}
		if (((UltraGridBase)CmbImporterBank).ActiveRow == null || Conversion.ToInt(CmbImporterBank.Value) == 0)
		{
			MessageBox.Show("Importer Bank Field is Required!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			CmbImporterBank.Focus();
			return false;
		}
		if (((UltraGridBase)cmbExporterBanks).ActiveRow == null || Conversion.ToInt(cmbExporterBanks.Value) == 0)
		{
			MessageBox.Show("Exporter Bank Field is Required!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			cmbExporterBanks.Focus();
			return false;
		}
		if (((UltraGridBase)cmbPaymenTerm).ActiveRow == null || Conversion.ToInt(cmbPaymenTerm.Value) == 0)
		{
			MessageBox.Show("Payment Term Field is Required!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			cmbPaymenTerm.Focus();
			return false;
		}
		if (txtTotalFcyAmountMain.Text == string.Empty || Conversion.ToDecimal((object)txtTotalFcyAmountMain.Text) == 0m)
		{
			MessageBox.Show("Fcy Amount Field is Required!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			txtTotalFcyAmountMain.Focus();
			return false;
		}
		if (((UltraGridBase)cmbCurreny).ActiveRow == null || Conversion.ToInt(cmbCurreny.Value) == 0)
		{
			MessageBox.Show("Fcy Code Field is Required!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			cmbCurreny.Focus();
			return false;
		}
		if (txtExchangeRate.Text == string.Empty || Conversion.ToDecimal((object)txtExchangeRate.Text) == 0m)
		{
			MessageBox.Show("Exchange Rate Field is Required!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			txtExchangeRate.Focus();
			return false;
		}
		if (txtLcyAmountMain.Text == string.Empty || Conversion.ToDecimal((object)txtLcyAmountMain.Text) == 0m)
		{
			MessageBox.Show("Local Amount Field is Required!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			txtLcyAmountMain.Focus();
			return false;
		}
		if (((UltraGridBase)cmbDeliveryTerm).ActiveRow == null || Conversion.ToInt(cmbDeliveryTerm.Value) == 0)
		{
			MessageBox.Show("Delivery Term Field is Required!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			cmbDeliveryTerm.Focus();
			return false;
		}
		if (txtGrossWeightMain.Text == string.Empty || Conversion.ToDecimal((object)txtGrossWeightMain.Text) == 0m)
		{
			MessageBox.Show("Gross Weight Field is Required!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			txtGrossWeightMain.Focus();
			return false;
		}
		if (txtNetWeightMain.Text == string.Empty || Conversion.ToDecimal((object)txtNetWeightMain.Text) == 0m)
		{
			MessageBox.Show("Net Weight Field is Required!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			txtNetWeightMain.Focus();
			return false;
		}
		if (Conversion.ToInt((object)txtGrossWeightMain.Text) < Conversion.ToInt((object)txtNetWeightMain.Text))
		{
			MessageBox.Show("Gross Weight Must Be Greater then Net Weight", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return false;
		}
		if (((UltraGridBase)cmbOrderStatus).ActiveRow == null || Conversion.ToInt(cmbOrderStatus.Value) == 0)
		{
			MessageBox.Show("Status Field is Required!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			cmbOrderStatus.Focus();
			return false;
		}
		return true;
	}

	private bool DetailFormValidation()
	{
		if (((UltraGridBase)cmbItem).ActiveRow == null || Conversion.ToInt(cmbItem.Value) == 0)
		{
			MessageBox.Show("Item Field is Required!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			cmbItem.Focus();
			return false;
		}
		if (((UltraGridBase)cmbPackingType).ActiveRow == null || Conversion.ToInt(cmbPackingType.Value) == 0)
		{
			MessageBox.Show("Packing Type Field is Required!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			cmbPackingType.Focus();
			return false;
		}
		if (txtQtyDetail.Text == string.Empty || Conversion.ToDecimal((object)txtQtyDetail.Text) == 0m)
		{
			MessageBox.Show("Inner Qty Field is Required!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			txtQtyDetail.Focus();
			return false;
		}
		if (((UltraGridBase)cmbItemUom).ActiveRow == null || Conversion.ToInt(cmbItemUom.Value) == 0)
		{
			MessageBox.Show("Pack UOM Field is Required!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			cmbItemUom.Focus();
			return false;
		}
		if (Conversion.ToDecimal((object)txtWeightDetail.Text) == 0m)
		{
			MessageBox.Show("Net Weight Field is Required!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			txtWeightDetail.Focus();
			return false;
		}
		if (txtItemRate.Text == string.Empty || Conversion.ToDecimal((object)txtItemRate.Text) == 0m)
		{
			MessageBox.Show("Rate Field is Required!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			txtItemRate.Focus();
			return false;
		}
		if (((UltraGridBase)cmbRateUom).ActiveRow == null || Conversion.ToInt(cmbRateUom.Value) == 0)
		{
			MessageBox.Show("Rate UOM Field is Required!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			cmbRateUom.Focus();
			return false;
		}
		if (txtFcyAmountDetail.Text == string.Empty || Conversion.ToDecimal((object)txtFcyAmountDetail.Text) == 0m)
		{
			MessageBox.Show("Amount Field is Required!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			txtFcyAmountDetail.Focus();
			return false;
		}
		return true;
	}

	private bool PaymentFormValidation()
	{
		if (Conversion.ToInt(CmbPaymentTermDetail.Value) == 0)
		{
			MessageBox.Show("Payment Term field required");
			CmbPaymentTermDetail.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)PercntOfTotal.Text.Trim()) == 0.0)
		{
			MessageBox.Show("% of Total field required");
			PercntOfTotal.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtPaymentDetailFcyAmount.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Fcy Amount field required");
			txtPaymentDetailFcyAmount.Focus();
			return false;
		}
		return true;
	}

	private void btnAddPayment_Click(object sender, EventArgs e)
	{
		try
		{
			if (!PaymentFormValidation())
			{
				return;
			}
			double prcntOfTotal = Conversion.ToDouble((object)PercntOfTotal.Text);
			if (prcntOfTotal > 100.0)
			{
				MessageBox.Show("Percent can't be Greater than 100%", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				PercntOfTotal.Focus();
				return;
			}
			if (grdPaymentTerm.GetRows().Count() > 0)
			{
				GridEXRow[] rows = grdPaymentTerm.GetRows();
				for (int i = 0; i < rows.Length; i++)
				{
					if (Conversion.ToInt(rows[i].Cells["PaymentTermId"].Value) == Conversion.ToInt(CmbPaymentTermDetail.Value))
					{
						MessageBox.Show("You Can't Add Same Payment Term", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						CmbPaymentTermDetail.Focus();
						return;
					}
				}
				double TotalPaymentPrcnt = Conversion.ToDouble(grdPaymentTerm.GetTotal(grdPaymentTerm.RootTable.Columns["%ofTotal"], (AggregateFunction)2));
				if (TotalPaymentPrcnt + prcntOfTotal > 100.0)
				{
					MessageBox.Show("Toatl Percent can't be Greater than 100%.Remaing " + (100.0 - TotalPaymentPrcnt), "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					PercntOfTotal.Text = (100.0 - TotalPaymentPrcnt).ToString();
					return;
				}
			}
			dtPaymnent.Rows.Add(0, CmbPaymentTermDetail.Value, ((Control)(object)CmbPaymentTermDetail).Text, PercntOfTotal.Text, txtPaymentDetailFcyAmount.Text);
			grdPaymentTerm.DataSource = dtPaymnent;
			grdPaymentTerm.RetrieveStructure();
			grdSettingsPaymentTerm();
			ResetPaymentDetail();
			CmbPaymentTermDetail.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdatePayment_Click(object sender, EventArgs e)
	{
		try
		{
			if (!PaymentFormValidation())
			{
				return;
			}
			double prcntOfTotal = Conversion.ToDouble((object)PercntOfTotal.Text);
			if (prcntOfTotal > 100.0)
			{
				MessageBox.Show("Percent can't be Greater than 100%", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				PercntOfTotal.Focus();
				return;
			}
			if (grdPaymentTerm.GetRows().Count() > 0)
			{
				GridEXRow[] rows = grdPaymentTerm.GetRows();
				foreach (GridEXRow item in rows)
				{
					if (item.RowIndex != updateDetailIndexPaymentDetail && Conversion.ToInt(item.Cells["PaymentTermId"].Value) == Conversion.ToInt(CmbPaymentTermDetail.Value))
					{
						MessageBox.Show("You Can't Add Same Payment Term Twice", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						return;
					}
				}
				GridEXRow curr = grdPaymentTerm.GetRow(updateDetailIndexPaymentDetail);
				double num = Conversion.ToDouble(grdPaymentTerm.GetTotal(grdPaymentTerm.RootTable.Columns["%ofTotal"], (AggregateFunction)2));
				double currentRowPrcnt = Conversion.ToDouble(curr.Cells["%ofTotal"].Value);
				double t = Math.Abs(num - currentRowPrcnt);
				if (t + prcntOfTotal > 100.0)
				{
					MessageBox.Show("Toatl Percent can't be Greater than 100%.Remaing " + (100.0 - t), "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					PercntOfTotal.Text = (100.0 - t).ToString();
					return;
				}
			}
			dtPaymnent.Rows[updateDetailIndexPaymentDetail]["PaymentTermId"] = CmbPaymentTermDetail.Value;
			dtPaymnent.Rows[updateDetailIndexPaymentDetail]["PaymentTerm"] = ((Control)(object)CmbPaymentTermDetail).Text;
			dtPaymnent.Rows[updateDetailIndexPaymentDetail]["%ofTotal"] = PercntOfTotal.Text;
			dtPaymnent.Rows[updateDetailIndexPaymentDetail]["FcyAmount"] = txtPaymentDetailFcyAmount.Text;
			grdPaymentTerm.DataSource = dtPaymnent;
			btnAddPayment.Visible = true;
			btnUpdatePayment.Visible = false;
			btnCancelPayment.Visible = false;
			ResetPaymentDetail();
			CmbPaymentTermDetail.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelPayment_Click(object sender, EventArgs e)
	{
		try
		{
			ResetPaymentDetail();
			btnAddPayment.Visible = true;
			btnUpdatePayment.Visible = false;
			btnCancelPayment.Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAddPayment_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl.Name == "grdPaymentTerm")
			{
				CmbPaymentTermDetail.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelPayment_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl.Name == "grdPaymentTerm")
			{
				CmbPaymentTermDetail.Focus();
			}
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
			grdPaymentTerm.RootTable.Columns["Id"].Visible = false;
			grdPaymentTerm.RootTable.Columns["PaymentTermId"].Visible = false;
			grdPaymentTerm.RootTable.Columns["%ofTotal"].TextAlignment = (TextAlignment)3;
			grdPaymentTerm.RootTable.Columns["%ofTotal"].FormatString = "#,##0.###";
			grdPaymentTerm.RootTable.Columns["FcyAmount"].AggregateFunction = (AggregateFunction)2;
			grdPaymentTerm.RootTable.Columns["FcyAmount"].TextAlignment = (TextAlignment)3;
			grdPaymentTerm.RootTable.Columns["FcyAmount"].FormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdPaymentTerm.RootTable.Columns["FcyAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingleForFcy;
			CommonServices.GridAutoAdjustmentNew(grdPaymentTerm);
			grdPaymentTerm.RootTable.Columns.Add("Delete");
			grdPaymentTerm.RootTable.Columns["Delete"].Key = "Delete";
			grdPaymentTerm.RootTable.Columns["Delete"].Caption = "X";
			grdPaymentTerm.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdPaymentTerm.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdPaymentTerm.RootTable.Columns["Delete"].ButtonText = "X";
			grdPaymentTerm.RootTable.Columns["Delete"].Width = 20;
			grdPaymentTerm.RootTable.Columns["Delete"].Position = 0;
			grdPaymentTerm.FrozenColumns = 1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPaymentTerm_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grdPaymentTerm.CurrentRow;
			if (item != null)
			{
				updateDetailIndexPaymentDetail = item.RowIndex;
				CmbPaymentTermDetail.Value = item.Cells["PaymentTermId"].Value.ToString();
				PercntOfTotal.Text = Conversion.ToDouble(item.Cells["%ofTotal"].Value).ToString("#,##0.###");
				txtPaymentDetailFcyAmount.Text = Conversion.ToDouble(item.Cells["FcyAmount"].Value).ToString(clsGlobalVariables.stringFormatsingleForFcy);
				btnAddPayment.Visible = false;
				btnUpdatePayment.Visible = true;
				btnCancelPayment.Visible = true;
				CmbPaymentTermDetail.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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
				dtPaymnent.Rows[item.RowIndex].Delete();
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
			}
			grdPaymentTerm.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PercntOfTotal_KeyPress(object sender, KeyPressEventArgs e)
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

	private void PercntOfTotal_TextChanged(object sender, EventArgs e)
	{
		try
		{
			double prcnt = Conversion.ToDouble((object)PercntOfTotal.Text);
			double fcyAmount = Conversion.ToDouble((object)txtTotalFcyAmountMain.Text);
			if (prcnt > 100.0)
			{
				PercntOfTotal.Text = "0";
				txtPaymentDetailFcyAmount.Text = "0";
				MessageBox.Show("%age Can't be Greater Than 100", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			else if (fcyAmount > 0.0)
			{
				double amount = prcnt * fcyAmount / 100.0;
				txtPaymentDetailFcyAmount.Text = Math.Round(amount, clsGlobalVariables.DefaultNoofDecimalPointsForFcyAmount, MidpointRounding.AwayFromZero).ToString(clsGlobalVariables.stringFormatsingle);
			}
			else
			{
				txtPaymentDetailFcyAmount.Text = "0";
				MessageBox.Show("Fcy Amount is Zero", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnaddnew_Click(object sender, EventArgs e)
	{
		try
		{
			if (DetailFormValidation())
			{
				dtdetail.Rows.Add(0, Conversion.ToInt(cmbItem.Value), Conversion.ToString(((UltraDropDownBase)cmbItem).SelectedRow.Cells["ItemName"].Value), Conversion.ToString(((UltraDropDownBase)cmbItem).SelectedRow.Cells["ItemCode"].Value), Conversion.ToString((object)txtItemDescription.Text), Conversion.ToInt(cmbPackingType.Value), Conversion.ToString((object)((Control)(object)cmbPackingType).Text), Conversion.ToInt(cmbItemUom.Value), Conversion.ToString((object)((Control)(object)cmbItemUom).Text), Conversion.ToDecimal((object)txtQtyDetail.Text), Conversion.ToDecimal((object)txtWeightDetail.Text), Conversion.ToDecimal((object)txtItemRate.Text), Conversion.ToInt(cmbRateUom.Value), Conversion.ToString((object)((Control)(object)cmbRateUom).Text), Conversion.ToDecimal((object)txtFcyAmountDetail.Text), Conversion.ToDecimal((object)txtFcyAmountDetail.Text), Conversion.ToString((object)txtRemarksDetail.Text));
				grdDetail.DataSource = dtdetail;
				grdDetail.RetrieveStructure();
				GrdSetting();
				FormResetDetail();
				cmbItem.Focus();
				CalculateLcyAmountInDetail();
				CalculateNetWeightAndFcyAmountInMain();
			}
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
				cmbItem.Value = item.Cells["ItemId"].Value.ToString();
				cmbitem_Leave(null, null);
				txtItemDescription.Text = item.Cells["Description"].Text.ToString();
				cmbPackingType.Value = item.Cells["PackingType"].Value.ToString();
				((Control)(object)cmbItemUom).Text = item.Cells["ItemUomId"].Text.ToString();
				txtQtyDetail.Text = item.Cells["Qty"].Text.ToString();
				txtWeightDetail.Text = item.Cells["NetWeight"].Text.ToString();
				txtItemRate.Text = item.Cells["ItemRate"].Text.ToString();
				((Control)(object)cmbRateUom).Text = item.Cells["RateUom"].Text.ToString();
				txtFcyAmountDetail.Text = item.Cells["FcyAmount"].Text.ToString();
				txtRemarksDetail.Text = item.Cells["RemarksDetail"].Text.ToString();
				btnaddnew.Visible = false;
				btnGridUpdate.Visible = true;
				btnCancel.Visible = true;
				cmbItem.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnGridUpdate_Click(object sender, EventArgs e)
	{
		try
		{
			if (DetailFormValidation())
			{
				dtdetail.Rows[updateDetailIndex]["ItemId"] = Conversion.ToInt(cmbItem.Value);
				dtdetail.Rows[updateDetailIndex]["ItemName"] = Conversion.ToString(((UltraDropDownBase)cmbItem).SelectedRow.Cells["ItemName"].Value);
				dtdetail.Rows[updateDetailIndex]["ItemCode"] = Conversion.ToString(((UltraDropDownBase)cmbItem).SelectedRow.Cells["ItemCode"].Value);
				dtdetail.Rows[updateDetailIndex]["Description"] = Conversion.ToString((object)txtItemDescription.Text);
				dtdetail.Rows[updateDetailIndex]["PackingTypeId"] = Conversion.ToInt(cmbPackingType.Value);
				dtdetail.Rows[updateDetailIndex]["PackingType"] = Conversion.ToString((object)((Control)(object)cmbPackingType).Text);
				dtdetail.Rows[updateDetailIndex]["ItemUomId"] = Conversion.ToInt(cmbItemUom.Value);
				dtdetail.Rows[updateDetailIndex]["ItemUom"] = Conversion.ToString((object)((Control)(object)cmbItemUom).Text);
				dtdetail.Rows[updateDetailIndex]["Qty"] = Conversion.ToDecimal((object)txtQtyDetail.Text);
				dtdetail.Rows[updateDetailIndex]["NetWeight"] = Conversion.ToDecimal((object)txtWeightDetail.Text);
				dtdetail.Rows[updateDetailIndex]["ItemRate"] = Conversion.ToDecimal((object)txtItemRate.Text);
				dtdetail.Rows[updateDetailIndex]["RateUomId"] = Conversion.ToInt(cmbRateUom.Value);
				dtdetail.Rows[updateDetailIndex]["RateUom"] = Conversion.ToString((object)((Control)(object)cmbRateUom).Text);
				dtdetail.Rows[updateDetailIndex]["FcyAmount"] = Conversion.ToDecimal((object)txtFcyAmountDetail.Text);
				dtdetail.Rows[updateDetailIndex]["RemarksDetail"] = Conversion.ToString((object)txtRemarksDetail.Text);
				FormResetDetail();
				CalculateLcyAmountInDetail();
				CalculateNetWeightAndFcyAmountInMain();
				cmbItem.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancel_Click(object sender, EventArgs e)
	{
		try
		{
			FormResetDetail();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdSetting()
	{
		try
		{
			grdDetail.RootTable.Columns["Id"].Visible = false;
			grdDetail.RootTable.Columns["ItemId"].Visible = false;
			grdDetail.RootTable.Columns["PackingTypeId"].Visible = false;
			grdDetail.RootTable.Columns["ItemUomId"].Visible = false;
			grdDetail.RootTable.Columns["RateUomId"].Visible = false;
			grdDetail.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["Qty"].FormatString = "#,##.##";
			grdDetail.RootTable.Columns["Qty"].TotalFormatString = "#,##.##";
			grdDetail.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["NetWeight"].FormatString = "#,##.##";
			grdDetail.RootTable.Columns["NetWeight"].TotalFormatString = "#,##.##";
			grdDetail.RootTable.Columns["ItemRate"].AggregateFunction = (AggregateFunction)3;
			grdDetail.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["ItemRate"].FormatString = clsGlobalVariables.DecimalFCYRateFormate;
			grdDetail.RootTable.Columns["ItemRate"].TotalFormatString = clsGlobalVariables.DecimalFCYRateFormate;
			grdDetail.RootTable.Columns["FcyAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["FcyAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["FcyAmount"].FormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdDetail.RootTable.Columns["FcyAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdDetail.RootTable.Columns["LcyAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["LcyAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["LcyAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["LcyAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustmentNew(grdDetail);
			grdDetail.RootTable.Columns.Add("Edit");
			grdDetail.RootTable.Columns["Edit"].Key = "Edit";
			grdDetail.RootTable.Columns["Edit"].Caption = "Edit";
			grdDetail.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdDetail.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdDetail.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdDetail.RootTable.Columns["Edit"].Width = 50;
			grdDetail.RootTable.Columns["Edit"].Position = 0;
			grdDetail.RootTable.Columns.Add("Delete");
			grdDetail.RootTable.Columns["Delete"].Key = "Delete";
			grdDetail.RootTable.Columns["Delete"].Caption = "X";
			grdDetail.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdDetail.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdDetail.RootTable.Columns["Delete"].ButtonText = "X";
			grdDetail.RootTable.Columns["Delete"].Width = 50;
			grdDetail.RootTable.Columns["Delete"].Position = 1;
			grdDetail.FrozenColumns = 2;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetail_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_02d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Expected O, but got Unknown
		//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Edit")
			{
				grdDetail_DoubleClick(null, null);
			}
			if (e.Column.Key == "Delete")
			{
				if (updateDetailIndex > -1)
				{
					MessageBox.Show("Please Reset Detail Box Data If You want to Delete Any row from the Grid....");
					return;
				}
				GridEXRow r = grdDetail.CurrentRow;
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					ImLcOrderPackingDetail vd = new ImLcOrderPackingDetail();
					vd.Id = Conversion.ToInt(r.Cells["Id"].Value);
					vd.ImItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					vd.ItemBrandId = Conversion.ToInt(r.Cells["ItemId"].Value);
					vd.ItemDescription = Conversion.ToString((object)r.Cells["Description"].Text);
					vd.InvPackingTypeId = Conversion.ToInt(r.Cells["PackingTypeId"].Value);
					vd.ItemUomId = Conversion.ToInt(r.Cells["ItemUomId"].Value);
					vd.Qty = Conversion.ToDecimal(r.Cells["Qty"].Value);
					vd.NetWeight = Conversion.ToDecimal(r.Cells["NetWeight"].Value);
					vd.ItemRate = Conversion.ToDecimal(r.Cells["ItemRate"].Value);
					vd.RateUomId = Conversion.ToInt(r.Cells["RateUomId"].Value);
					vd.FcyAmount = Conversion.ToDecimal(r.Cells["FcyAmount"].Value);
					vd.CurrencyId = Conversion.ToInt(cmbCurreny.Value);
					vd.ExchangeRate = Conversion.ToDecimal((object)txtExchangeRate.Text);
					vd.LcyAmount = Conversion.ToDecimal(r.Cells["LcyAmount"].Value);
					vd.ItemAmount = Conversion.ToDecimal(r.Cells["LcyAmount"].Value);
					vd.RemarksDetail = Conversion.ToString((object)r.Cells["RemarksDetail"].Text);
					vd.ActionTypeId = 3;
					lstRemoveRecord.Add(vd);
					dtdetail.Rows[r.RowIndex].Delete();
					if (r != null && (int)r.RowType == 0)
					{
						r.Delete();
					}
					grdDetail.UpdateData();
				}
				else if (r != null && (int)r.RowType == 0)
				{
					r.Delete();
				}
			}
			CalculateLcyAmountInDetail();
			CalculateNetWeightAndFcyAmountInMain();
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
		//IL_04b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bc: Expected O, but got Unknown
		//IL_076b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0772: Expected O, but got Unknown
		//IL_081f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0826: Expected O, but got Unknown
		try
		{
			if (grdDetail.RowCount == 0)
			{
				throw new Exception("Please Check Detail Grid");
			}
			if (!FormValidation())
			{
				return;
			}
			ImLcOrder exImLc = new ImLcOrder();
			if (RecId == 0)
			{
				if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
			}
			else
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				exImLc.Id = RecId;
			}
			if (grdPaymentTerm.RowCount > 0)
			{
				decimal TotalFcyAmount = Conversion.ToDecimal((object)txtTotalFcyAmountMain.Text);
				decimal PaymentGridFcyAmount = Conversion.ToDecimal(grdPaymentTerm.GetTotal(grdPaymentTerm.RootTable.Columns["FcyAmount"], (AggregateFunction)2));
				if (TotalFcyAmount != PaymentGridFcyAmount)
				{
					throw new Exception($"Payment Term Grid Total Amount not Equal To Total FcyAmount.. PaymentGridAmount is = '{PaymentGridFcyAmount}' and TotalFcyAmount is = '{TotalFcyAmount}'");
				}
				CalculateLcyAmountInDetail();
				CalculateLocalAmount();
				exImLc.DocumentTypeId = 231;
				exImLc.OrganizationId = UserAccount.OrganizationId;
				exImLc.CompanyId = UserAccount.CompanyId;
				exImLc.BranchesId = Conversion.ToInt((object)UserAccount.BranchesId);
				exImLc.FinancialYearId = Conversion.ToInt((object)clsGlobalVariables.ActiveYr.Id);
				exImLc.DocNo = Conversion.ToInt((object)txtdocno.Text);
				exImLc.DocDate = txtdocdate.Value;
				exImLc.SupplierCustomerId = Conversion.ToInt(cmbSupplier.Value);
				exImLc.LcOrderNo = txtSupLcOrderNo.Text;
				exImLc.LcOrderDate = txtSupLcOrderDate.Value;
				exImLc.SalesPersonId = Conversion.ToInt(cmbSalesPerson.Value);
				exImLc.NotifyPartyId = Conversion.ToInt(cmbNotifyParty.Value);
				exImLc.ShipedToId = Conversion.ToInt(cmbShippedTo.Value);
				exImLc.PartialShipment = Conversion.ToString((object)cmbPartialShipment.Text);
				exImLc.TransShipment = cmbTransShipment.Text;
				exImLc.LastShipmentDate = txtLastShipmentDate.Value;
				exImLc.QuotReference = txtQuorationRef.Text;
				exImLc.InquiryReference = txtInquiryRef.Text;
				exImLc.ExpiryDate = txtExpiryDate.Value;
				exImLc.ExpiryPlace = txtExpiryPlace.Text;
				exImLc.LoadingPortId = Conversion.ToInt(cmbLoadingPort.Value);
				exImLc.DestinationPortId = Conversion.ToInt(cmbDestinationPort.Value);
				exImLc.LegalizationRequired = ((Control)(object)cmbLegalizationRequired).Text;
				exImLc.InsepctionRequired = ((Control)(object)cmbInspectionRequired).Text;
				exImLc.ImporterBankId = Conversion.ToInt(CmbImporterBank.Value);
				exImLc.ExporterBankId = Conversion.ToInt(cmbExporterBanks.Value);
				exImLc.PaymentTermId = Conversion.ToInt(cmbPaymenTerm.Value);
				exImLc.FcyAmount = Conversion.ToDecimal((object)txtTotalFcyAmountMain.Text);
				exImLc.FcurrencyId = Conversion.ToInt(cmbCurreny.Value);
				exImLc.ExchangeRate = Conversion.ToDecimal((object)txtExchangeRate.Text);
				exImLc.LcyAmount = Conversion.ToDecimal((object)txtLcyAmountMain.Text);
				exImLc.DeliveryTermId = Conversion.ToInt(cmbDeliveryTerm.Value);
				exImLc.GrossWeightKgs = Conversion.ToDecimal((object)txtGrossWeightMain.Text);
				exImLc.NetWeightKgs = Conversion.ToDecimal((object)txtNetWeightMain.Text);
				exImLc.NoOfContainers = Conversion.ToInt((object)txtNoOfContainerMain.Text);
				exImLc.Status = ((Control)(object)cmbOrderStatus).Text;
				exImLc.RemarksHeader = txtRemarksHeader.Text;
				exImLc.CommodityDetial = txtCommodityDescMain.Text;
				exImLc.LegalizationDescription = txtLegalizationDesc.Text;
				exImLc.InsepctionDescription = txtInspectionDesc.Text;
				exImLc.ShippingMarks = txtShippingMarks.Text;
				exImLc.IsApproved = false;
				exImLc.EntryDate = DateTime.Now;
				exImLc.EntryUserId = UserAccount.ID;
				exImLc.ModifyDate = DateTime.Now;
				exImLc.ModifyUserId = UserAccount.ID;
				exImLc.ApprovedDate = DateTime.Now;
				exImLc.ApprovedUserId = UserAccount.ID;
				exImLc.ImLcOrderPackingDetailList = new List<ImLcOrderPackingDetail>();
				exImLc.ImLcOrderPaymnetTermList = new List<ImLcOrderPaymnetTerm>();
				exImLc.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
				GridEXRow[] rows = grdDetail.GetRows();
				foreach (GridEXRow r2 in rows)
				{
					ImLcOrderPackingDetail vh = new ImLcOrderPackingDetail();
					if (RecId > 0)
					{
						vh.Id = Conversion.ToInt(r2.Cells["Id"].Value);
					}
					else
					{
						vh.Id = 0;
					}
					if (vh.Id == 0)
					{
						vh.ActionTypeId = 1;
					}
					else
					{
						vh.ActionTypeId = 2;
					}
					vh.ImItemId = Conversion.ToInt(r2.Cells["ItemId"].Value);
					vh.ItemBrandId = Conversion.ToInt(r2.Cells["ItemId"].Value);
					vh.ItemDescription = Conversion.ToString((object)r2.Cells["Description"].Text);
					vh.InvPackingTypeId = Conversion.ToInt(r2.Cells["PackingTypeId"].Value);
					vh.ItemUomId = Conversion.ToInt(r2.Cells["ItemUomId"].Value);
					vh.Qty = Conversion.ToDecimal(r2.Cells["Qty"].Value);
					vh.NetWeight = Conversion.ToDecimal(r2.Cells["NetWeight"].Value);
					vh.ItemRate = Conversion.ToDecimal(r2.Cells["ItemRate"].Value);
					vh.RateUomId = Conversion.ToInt(r2.Cells["RateUomId"].Value);
					vh.FcyAmount = Conversion.ToDecimal(r2.Cells["FcyAmount"].Value);
					vh.CurrencyId = Conversion.ToInt(cmbCurreny.Value);
					vh.ExchangeRate = Conversion.ToDecimal((object)txtExchangeRate.Text);
					vh.LcyAmount = Conversion.ToDecimal(r2.Cells["LcyAmount"].Value);
					vh.ItemAmount = Conversion.ToDecimal(r2.Cells["LcyAmount"].Value);
					vh.RemarksDetail = Conversion.ToString((object)r2.Cells["RemarksDetail"].Text);
					exImLc.ImLcOrderPackingDetailList.Add(vh);
				}
				if (lstRemoveRecord.Count > 0)
				{
					for (int j = 0; j < lstRemoveRecord.Count; j++)
					{
						exImLc.ImLcOrderPackingDetailList.Add(lstRemoveRecord[j]);
					}
				}
				rows = grdPaymentTerm.GetRows();
				foreach (GridEXRow r in rows)
				{
					ImLcOrderPaymnetTerm vd = new ImLcOrderPaymnetTerm();
					vd.Id = Conversion.ToInt(r.Cells["Id"].Value);
					vd.PaymentTermId = Conversion.ToInt(r.Cells["PaymentTermId"].Value);
					vd.PrcntOfTotal = Conversion.ToDecimal(r.Cells["%ofTotal"].Value);
					vd.FcyAmount = Conversion.ToDecimal(r.Cells["FcyAmount"].Value);
					exImLc.ImLcOrderPaymnetTermList.Add(vd);
				}
				for (int i = 0; i < Attachment.dtAttachments.Rows.Count; i++)
				{
					DMSAttachments attach = new DMSAttachments();
					attach.Attachment = Path.GetFileName(Attachment.dtAttachments.Rows[i][1].ToString());
					attach.ScreenName = base.Name;
					exImLc.AttachmentsList.Add(attach);
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
				ImLcOrder.Save(exImLc);
				if (!UpdateMode)
				{
					MessageBox.Show("Save SuccessFully");
				}
				if (UpdateMode)
				{
					MessageBox.Show("Update SuccessFully");
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
				FormReset();
				return;
			}
			throw new Exception("Payment Term Grid record not found");
		}
		catch (Exception ex2)
		{
			MessageBox.Show(ex2.Message);
		}
	}

	private void btnSave_Click(object sender, EventArgs e)
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

	private void ReadById(int Id)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ImLcOrder exImLc = new ImLcOrder();
			RecId = Id;
			exImLc = ImLcOrder.ReadById(RecId);
			if (exImLc != null)
			{
				tabControl1.SelectedIndex = 0;
				cmbbranch.Value = exImLc.BranchesId;
				txtdocno.Text = Conversion.ToString((object)exImLc.DocNo);
				txtdocdate.Value = Conversion.ToDateTime((object)exImLc.LcOrderDate);
				cmbSupplier.Value = exImLc.SupplierCustomerId;
				txtSupLcOrderNo.Text = Conversion.ToString((object)exImLc.LcOrderNo);
				txtSupLcOrderDate.Value = Conversion.ToDateTime((object)exImLc.LcOrderDate);
				cmbSalesPerson.Value = exImLc.SalesPersonId;
				cmbNotifyParty.Value = exImLc.NotifyPartyId;
				cmbShippedTo.Value = exImLc.ShipedToId;
				cmbPartialShipment.Text = Conversion.ToString((object)exImLc.PartialShipment);
				cmbTransShipment.Text = Conversion.ToString((object)exImLc.TransShipment);
				txtLastShipmentDate.Value = Conversion.ToDateTime((object)exImLc.LastShipmentDate);
				txtQuorationRef.Text = Conversion.ToString((object)exImLc.QuotReference);
				txtInquiryRef.Text = Conversion.ToString((object)exImLc.InquiryReference);
				txtExpiryDate.Value = Conversion.ToDateTime((object)exImLc.ExpiryDate);
				txtExpiryPlace.Text = exImLc.ExpiryPlace;
				cmbLoadingPort.Value = exImLc.LoadingPortId;
				cmbDestinationPort.Value = exImLc.DestinationPortId;
				((Control)(object)cmbInspectionRequired).Text = Conversion.ToString((object)exImLc.InsepctionRequired);
				((Control)(object)cmbLegalizationRequired).Text = Conversion.ToString((object)exImLc.LegalizationRequired);
				CmbImporterBank.Value = exImLc.ImporterBankId;
				cmbExporterBanks.Value = exImLc.ExporterBankId;
				cmbPaymenTerm.Value = exImLc.PaymentTermId;
				cmbCurreny.Value = exImLc.FcurrencyId;
				txtTotalFcyAmountMain.Text = Conversion.ToString((object)exImLc.FcyAmount);
				txtExchangeRate.Text = Conversion.ToString((object)exImLc.ExchangeRate);
				txtLcyAmountMain.Text = Conversion.ToString((object)exImLc.LcyAmount);
				cmbDeliveryTerm.Value = exImLc.DeliveryTermId;
				txtGrossWeightMain.Text = Conversion.ToString((object)exImLc.GrossWeightKgs);
				txtNetWeightMain.Text = Conversion.ToString((object)exImLc.NetWeightKgs);
				txtNoOfContainerMain.Text = Conversion.ToString((object)exImLc.NoOfContainers);
				((Control)(object)cmbOrderStatus).Text = exImLc.Status;
				txtRemarksHeader.Text = Conversion.ToString((object)exImLc.RemarksHeader);
				txtCommodityDescMain.Text = Conversion.ToString((object)exImLc.CommodityDetial);
				txtInspectionDesc.Text = Conversion.ToString((object)exImLc.InsepctionDescription);
				txtLegalizationDesc.Text = Conversion.ToString((object)exImLc.LegalizationDescription);
				txtShippingMarks.Text = Conversion.ToString((object)exImLc.ShippingMarks);
				if (exImLc.ImLcOrderPackingDetailList.Count > 0)
				{
					dtdetail.Rows.Clear();
					for (int j = 0; j < exImLc.ImLcOrderPackingDetailList.Count; j++)
					{
						dtdetail.Rows.Add(exImLc.ImLcOrderPackingDetailList[j].Id, exImLc.ImLcOrderPackingDetailList[j].ImItemId, exImLc.ImLcOrderPackingDetailList[j].ItemName, exImLc.ImLcOrderPackingDetailList[j].ItemCode, exImLc.ImLcOrderPackingDetailList[j].ItemDescription, exImLc.ImLcOrderPackingDetailList[j].InvPackingTypeId, exImLc.ImLcOrderPackingDetailList[j].PackingType, exImLc.ImLcOrderPackingDetailList[j].ItemUomId, exImLc.ImLcOrderPackingDetailList[j].PackUom, exImLc.ImLcOrderPackingDetailList[j].Qty, exImLc.ImLcOrderPackingDetailList[j].NetWeight, exImLc.ImLcOrderPackingDetailList[j].ItemRate, exImLc.ImLcOrderPackingDetailList[j].RateUomId, exImLc.ImLcOrderPackingDetailList[j].RateUom, exImLc.ImLcOrderPackingDetailList[j].FcyAmount, exImLc.ImLcOrderPackingDetailList[j].LcyAmount);
					}
					grdDetail.DataSource = dtdetail;
					grdDetail.RetrieveStructure();
					GrdSetting();
					if (exImLc.ImLcOrderPaymnetTermList.Count > 0)
					{
						dtPaymnent.Rows.Clear();
						for (int i = 0; i < exImLc.ImLcOrderPaymnetTermList.Count; i++)
						{
							dtPaymnent.Rows.Add(exImLc.ImLcOrderPaymnetTermList[i].Id, exImLc.ImLcOrderPaymnetTermList[i].PaymentTermId, exImLc.ImLcOrderPaymnetTermList[i].PaymentTerm, exImLc.ImLcOrderPaymnetTermList[i].PrcntOfTotal, exImLc.ImLcOrderPaymnetTermList[i].FcyAmount);
						}
						grdPaymentTerm.DataSource = dtPaymnent;
						grdPaymentTerm.RetrieveStructure();
						grdSettingsPaymentTerm();
					}
					CalculateLcyAmountInDetail();
					CalculateLocalAmount();
					UpdateMode = true;
					btnSave.Visible = false;
					BtnSaveAs.Visible = false;
					btnUpdate.Visible = true;
					btnDelete.Visible = true;
				}
				else
				{
					MessageBox.Show("Detail Record No found....");
				}
			}
			else
			{
				MessageBox.Show($"Record No found against Current Id{RecId}....");
			}
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
					ImLcOrder.DeleteByID(UserAccount.ID, RecId);
					MessageBox.Show("Delete Record Seccessfully");
					FormReset();
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

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			if (tabControl1.SelectedIndex == 1)
			{
				GridHistoryFill(50);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridHistoryFill(int NoOfRecords = 0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Expected O, but got Unknown
		try
		{
			DataTable lst = ImLcOrder.FormHistory(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				DocumentTypeId = 231,
				EntryUser = UserAccount.ID,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				NoOfRecords = NoOfRecords,
				CanViewAllRecord = formrights.DoHaveCanViewAllRecordRights
			});
			if (lst.Rows.Count > 0)
			{
				DataTable dtcol = new DataTable();
				dtcol.Columns.Add("RecordNo", typeof(int));
				dtcol.Columns.Add("Id", typeof(int));
				dtcol.Columns.Add("DocumentTypeId", typeof(int));
				dtcol.Columns.Add("DocNo", typeof(string));
				dtcol.Columns.Add("DocDate", typeof(DateTime));
				dtcol.Columns.Add("SupOrderNo", typeof(string));
				dtcol.Columns.Add("SupOrderDate", typeof(DateTime));
				dtcol.Columns.Add("SupplierCustomerId", typeof(int));
				dtcol.Columns.Add("SupplierName", typeof(string));
				dtcol.Columns.Add("NotifyParty", typeof(string));
				dtcol.Columns.Add("ExpiryDate", typeof(DateTime));
				dtcol.Columns.Add("ExpiryPlace", typeof(string));
				dtcol.Columns.Add("PartialShipment", typeof(string));
				dtcol.Columns.Add("TransShipment", typeof(string));
				dtcol.Columns.Add("LastShipmentDate", typeof(DateTime));
				dtcol.Columns.Add("QuotReference", typeof(string));
				dtcol.Columns.Add("InquiryReference", typeof(string));
				dtcol.Columns.Add("ImporterBank", typeof(string));
				dtcol.Columns.Add("ExporterBank", typeof(string));
				dtcol.Columns.Add("LoadingPort", typeof(string));
				dtcol.Columns.Add("DestinationPort", typeof(string));
				dtcol.Columns.Add("PaymentTerm", typeof(string));
				dtcol.Columns.Add("FcyAmount", typeof(decimal));
				dtcol.Columns.Add("CurrencyCode", typeof(string));
				dtcol.Columns.Add("ExchangeRate", typeof(decimal));
				dtcol.Columns.Add("LcyAmount", typeof(decimal));
				dtcol.Columns.Add("GrossWeightKgs", typeof(decimal));
				dtcol.Columns.Add("NetWeightKgs", typeof(decimal));
				dtcol.Columns.Add("NoOfContainers", typeof(int));
				dtcol.Columns.Add("Status", typeof(string));
				dtcol.Columns.Add("CommodityDetial", typeof(string));
				dtcol.Columns.Add("RemarksHeader", typeof(string));
				dtcol.Columns.Add("EntryDate", typeof(DateTime));
				dtcol.Columns.Add("EntryUserName", typeof(string));
				dtcol.Columns.Add("NoOfAttachments", typeof(int));
				dtcol.Rows.Clear();
				for (int i = 0; i < lst.Rows.Count; i++)
				{
					dtcol.Rows.Add(lst.Rows[i]["RecordNo"], lst.Rows[i]["Id"], lst.Rows[i]["DocumentTypeId"], lst.Rows[i]["DocNo"], lst.Rows[i]["DocDate"], lst.Rows[i]["LcOrderNo"], lst.Rows[i]["LcOrderDate"], lst.Rows[i]["SupplierCustomerId"], lst.Rows[i]["SupplierName"], lst.Rows[i]["NotifyParty"], lst.Rows[i]["ExpiryDate"], lst.Rows[i]["ExpiryPlace"], lst.Rows[i]["PartialShipment"], lst.Rows[i]["TransShipment"], lst.Rows[i]["LastShipmentDate"], lst.Rows[i]["QuotReference"], lst.Rows[i]["InquiryReference"], lst.Rows[i]["ImporterBank"], lst.Rows[i]["ExporterBank"], lst.Rows[i]["LoadingPort"], lst.Rows[i]["DestinationPort"], lst.Rows[i]["lcOrderTermDesc"], lst.Rows[i]["FcyAmount"], lst.Rows[i]["CurrencySymbol"], lst.Rows[i]["ExchangeRate"], lst.Rows[i]["LcyAmount"], lst.Rows[i]["GrossWeightKgs"], lst.Rows[i]["NetWeightKgs"], lst.Rows[i]["NoOfContainers"], lst.Rows[i]["Status"], lst.Rows[i]["CommodityDetial"], lst.Rows[i]["RemarksHeader"], lst.Rows[i]["EntryDate"], lst.Rows[i]["EntryUserName"], lst.Rows[i]["NoOfAttachments"]);
				}
				DataGridHistory.DataSource = dtcol;
				DataGridHistory.RetrieveStructure();
				DataGridHistorySetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistorySetting()
	{
		try
		{
			DataGridHistory.RootTable.Columns["RecordNo"].Visible = false;
			DataGridHistory.RootTable.Columns["DocumentTypeId"].Visible = false;
			DataGridHistory.RootTable.Columns["SupplierCustomerId"].Visible = false;
			DataGridHistory.RootTable.Columns["Id"].Visible = false;
			DataGridHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			SetColumnAggregateFormat(DataGridHistory, "NoOfContainers", "#,##0.###", (AggregateFunction)2);
			SetColumnAggregateFormat(DataGridHistory, "GrossWeightKgs", "#,##0.###", (AggregateFunction)2);
			SetColumnAggregateFormat(DataGridHistory, "NetWeightKgs", "#,##0.###", (AggregateFunction)2);
			SetColumnAggregateFormat(DataGridHistory, "FcyAmount", clsGlobalVariables.stringFormatsingleForFcy, (AggregateFunction)2);
			SetColumnAggregateFormat(DataGridHistory, "LcyAmount", clsGlobalVariables.stringFormatsingle, (AggregateFunction)2);
			CommonServices.GridAutoAdjustmentNew(DataGridHistory);
			ConfigureUserRightsColumns();
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SetColumnAggregateFormat(GridEX grid, string columnName, string formatString, AggregateFunction aggregateFunction)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		grid.RootTable.Columns[columnName].AggregateFunction = aggregateFunction;
		grid.RootTable.Columns[columnName].TextAlignment = (TextAlignment)3;
		grid.RootTable.Columns[columnName].FormatString = formatString;
		grid.RootTable.Columns[columnName].TotalFormatString = formatString;
	}

	private void ConfigureUserRightsColumns()
	{
		if (formrights.DoHaveUpdateRights)
		{
			AddButtonColumn("Edit", "Edit", "Edit", 50, 0);
		}
		if (formrights.DoHaveSaveRight)
		{
			AddButtonColumn("SaveAs", "SaveAs", "SaveAs", 50, 1);
		}
		if (formrights.DoHavePrintRights)
		{
			AddButtonColumn("Print", "Print", "Print", 50, 2);
		}
		SetColumnPositionAndFrozen();
	}

	private void AddButtonColumn(string key, string caption, string buttonText, int width, int position)
	{
		DataGridHistory.RootTable.Columns.Add(key);
		DataGridHistory.RootTable.Columns[key].Key = key;
		DataGridHistory.RootTable.Columns[key].Caption = caption;
		DataGridHistory.RootTable.Columns[key].ButtonDisplayMode = (CellButtonDisplayMode)3;
		DataGridHistory.RootTable.Columns[key].ButtonStyle = (ButtonStyle)5;
		DataGridHistory.RootTable.Columns[key].ButtonText = buttonText;
		DataGridHistory.RootTable.Columns[key].Width = width;
		DataGridHistory.RootTable.Columns[key].Position = position;
	}

	private void SetColumnPositionAndFrozen()
	{
		int frozenColumns = 0;
		if (formrights.DoHaveUpdateRights)
		{
			DataGridHistory.RootTable.Columns["Edit"].Position = frozenColumns++;
		}
		if (formrights.DoHaveSaveRight)
		{
			DataGridHistory.RootTable.Columns["SaveAs"].Position = frozenColumns++;
		}
		if (formrights.DoHavePrintRights)
		{
			DataGridHistory.RootTable.Columns["Print"].Position = frozenColumns++;
		}
		DataGridHistory.FrozenColumns = frozenColumns;
	}

	private void DataGridHistory_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			if (formrights.DoHaveUpdateRights)
			{
				RecId = Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells["Id"].Value.ToString());
				ReadById(RecId);
			}
			else
			{
				MessageBox.Show("you Don't Have Update right....");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = DataGridHistory.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt((object)item.Cells["Id"].Value.ToString().ToString()), 231);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnLoadAllHistory_Click(object sender, EventArgs e)
	{
		try
		{
			GridHistoryFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				GridHistoryDetailBind(Conversion.ToInt(r.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridHistoryDetailBind(int Id)
	{
		try
		{
			DataTable dtdeta = ImLcOrder.FormHistoryDetail(Id);
			if (dtdeta.Rows.Count > 0)
			{
				DataTable dtcol = new DataTable();
				dtcol.Columns.Add("Id", typeof(int));
				dtcol.Columns.Add("ImLcOrderId", typeof(int));
				dtcol.Columns.Add("ImItemId", typeof(int));
				dtcol.Columns.Add("ItemName", typeof(string));
				dtcol.Columns.Add("ItemCode", typeof(string));
				dtcol.Columns.Add("ItemDescription", typeof(string));
				dtcol.Columns.Add("PackingType", typeof(string));
				dtcol.Columns.Add("PackUom", typeof(string));
				dtcol.Columns.Add("Qty", typeof(decimal));
				dtcol.Columns.Add("NetWeight", typeof(decimal));
				dtcol.Columns.Add("RateUom", typeof(string));
				dtcol.Columns.Add("ItemRate", typeof(decimal));
				dtcol.Columns.Add("FcyAmount", typeof(decimal));
				dtcol.Columns.Add("LcyAmount", typeof(decimal));
				dtcol.Columns.Add("RemarksDetail", typeof(string));
				dtcol.Rows.Clear();
				for (int i = 0; i < dtdeta.Rows.Count; i++)
				{
					dtcol.Rows.Add(dtdeta.Rows[i]["Id"], dtdeta.Rows[i]["ImLcOrderId"], dtdeta.Rows[i]["ImItemId"], dtdeta.Rows[i]["ItemName"], dtdeta.Rows[i]["ItemCode"], dtdeta.Rows[i]["ItemDescription"], dtdeta.Rows[i]["PackingType"], dtdeta.Rows[i]["PackUom"], dtdeta.Rows[i]["Qty"], dtdeta.Rows[i]["NetWeight"], dtdeta.Rows[i]["RateUom"], dtdeta.Rows[i]["ItemRate"], dtdeta.Rows[i]["FcyAmount"], dtdeta.Rows[i]["LcyAmount"], dtdeta.Rows[i]["RemarksDetail"]);
				}
				grdHistoryDetail.DataSource = dtcol;
				grdHistoryDetail.RetrieveStructure();
				grddetailsetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grddetailsetting()
	{
		try
		{
			grdHistoryDetail.RootTable.Columns["Id"].Visible = false;
			grdHistoryDetail.RootTable.Columns["ImLcOrderId"].Visible = false;
			grdHistoryDetail.RootTable.Columns["ImItemId"].Visible = false;
			grdHistoryDetail.RootTable.Columns["FcyAmount"].AggregateFunction = (AggregateFunction)2;
			grdHistoryDetail.RootTable.Columns["FcyAmount"].TextAlignment = (TextAlignment)3;
			grdHistoryDetail.RootTable.Columns["FcyAmount"].FormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdHistoryDetail.RootTable.Columns["FcyAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdHistoryDetail.RootTable.Columns["LcyAmount"].AggregateFunction = (AggregateFunction)2;
			grdHistoryDetail.RootTable.Columns["LcyAmount"].TextAlignment = (TextAlignment)3;
			grdHistoryDetail.RootTable.Columns["LcyAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdHistoryDetail.RootTable.Columns["LcyAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdHistoryDetail.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)3;
			grdHistoryDetail.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdHistoryDetail.RootTable.Columns["Qty"].FormatString = "#,##0.###";
			grdHistoryDetail.RootTable.Columns["Qty"].TotalFormatString = "#,##0.###";
			grdHistoryDetail.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)3;
			grdHistoryDetail.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			grdHistoryDetail.RootTable.Columns["NetWeight"].FormatString = "#,##0.###";
			grdHistoryDetail.RootTable.Columns["NetWeight"].TotalFormatString = "#,##0.###";
			grdHistoryDetail.RootTable.Columns["ItemRate"].AggregateFunction = (AggregateFunction)3;
			grdHistoryDetail.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			grdHistoryDetail.RootTable.Columns["ItemRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdHistoryDetail.RootTable.Columns["ItemRate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			CommonServices.GridAutoAdjustmentNew(grdHistoryDetail);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Edit")
			{
				if (!formrights.DoHaveUpdateRights && !formrights.DoHaveCanDelete)
				{
					MessageBox.Show("you Don't Have Update right....");
					return;
				}
				RecId = Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells["Id"].Value.ToString());
				ReadById(RecId);
				txtdocdate.Focus();
				btnDelete.Visible = true;
			}
			if (e.Column.Key == "SaveAs")
			{
				if (!formrights.DoHaveSaveRight && !formrights.DoHaveCanDelete)
				{
					MessageBox.Show("you Don't Have Update right....");
					return;
				}
				RecId = Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells["Id"].Value.ToString());
				ReadById(RecId);
				txtdocdate.Focus();
				BtnSaveAs.Visible = true;
				btnDelete.Visible = true;
				btnSave.Visible = false;
				btnUpdate.Visible = false;
			}
			if (e.Column.Key == "Print" && formrights.DoHavePrintRights)
			{
				RecId = Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells["Id"].Value.ToString());
				GenerateSlip(RecId);
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
			decimal ItemUOM = default(decimal);
			decimal Qty = default(decimal);
			if (((UltraGridBase)cmbItemUom).ActiveRow != null && Conversion.ToInt(cmbItemUom.Value) > 0)
			{
				ItemUOM = Conversion.ToDecimal(((UltraDropDownBase)cmbItemUom).SelectedRow.Cells[2].Value);
				Qty = Conversion.ToDecimal((object)txtQtyDetail.Text);
				if (ItemUOM > 0m && Qty > 0m)
				{
					decimal Weight = ItemUOM * Qty;
					txtWeightDetail.Text = Weight.ToString("##,#.##");
				}
				else
				{
					txtWeightDetail.Text = 0.ToString("##,#.##");
				}
			}
			else
			{
				txtWeightDetail.Text = 0.ToString("##,#.##");
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
			decimal RateUOM = default(decimal);
			decimal Weight = default(decimal);
			decimal Rate = default(decimal);
			if (((UltraGridBase)cmbRateUom).ActiveRow != null && Conversion.ToInt(cmbRateUom.Value) > 0)
			{
				RateUOM = Conversion.ToDecimal(((UltraDropDownBase)cmbRateUom).SelectedRow.Cells[2].Value);
				Weight = Conversion.ToDecimal((object)txtWeightDetail.Text);
				Rate = Conversion.ToDecimal((object)txtItemRate.Text);
				if (Weight > 0m && RateUOM > 0m && Rate > 0m)
				{
					decimal TotalAmount = Rate / RateUOM * Weight;
					txtFcyAmountDetail.Text = TotalAmount.ToString();
				}
				else
				{
					txtFcyAmountDetail.Text = 0.ToString("##,#.##");
				}
			}
			else
			{
				txtFcyAmountDetail.Text = 0.ToString("##,#.##");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateNetWeightAndFcyAmountInMain()
	{
		try
		{
			if (grdDetail.RowCount > 0)
			{
				decimal Weight = Conversion.ToDecimal(grdDetail.GetTotal(grdDetail.RootTable.Columns["NetWeight"], (AggregateFunction)2));
				decimal FcyAmount = Conversion.ToDecimal(grdDetail.GetTotal(grdDetail.RootTable.Columns["FcyAmount"], (AggregateFunction)2));
				txtTotalFcyAmountMain.Text = FcyAmount.ToString(clsGlobalVariables.stringFormatsingleForFcy);
				txtNetWeightMain.Text = Weight.ToString("##,#.##");
			}
			else
			{
				txtTotalFcyAmountMain.Text = 0.ToString(clsGlobalVariables.stringFormatsingleForFcy);
				txtNetWeightMain.Text = 0.ToString("##,#.##");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateLcyAmountInDetail()
	{
		try
		{
			if (grdDetail.RowCount <= 0)
			{
				return;
			}
			decimal ExchangeRate = Conversion.ToDecimal((object)txtExchangeRate.Text);
			GridEXRow[] rows;
			if (ExchangeRate > 0m)
			{
				rows = grdDetail.GetRows();
				foreach (GridEXRow r in rows)
				{
					decimal FcyAmount = Conversion.ToDecimal(r.Cells["FcyAmount"].Value);
					if (FcyAmount != 0m)
					{
						r.BeginEdit();
						r.Cells["LcyAmount"].Value = FcyAmount * ExchangeRate;
						r.EndEdit();
					}
					else
					{
						r.BeginEdit();
						r.Cells["LcyAmount"].Value = 0;
						r.EndEdit();
					}
				}
				return;
			}
			rows = grdDetail.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				decimal FcyAmount2 = Conversion.ToDecimal(r2.Cells["FcyAmount"].Value);
				if (FcyAmount2 != 0m)
				{
					r2.BeginEdit();
					r2.Cells["LcyAmount"].Value = FcyAmount2;
					r2.EndEdit();
				}
				else
				{
					r2.BeginEdit();
					r2.Cells["LcyAmount"].Value = 0;
					r2.EndEdit();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateLocalAmount()
	{
		try
		{
			CalculateNetWeightAndFcyAmountInMain();
			decimal fcyAmount = Conversion.ToDecimal((object)txtTotalFcyAmountMain.Text);
			decimal ExchangeRate = Conversion.ToDecimal((object)txtExchangeRate.Text);
			if (fcyAmount > 0m && ExchangeRate > 0m)
			{
				decimal LocalAmount = fcyAmount * ExchangeRate;
				txtLcyAmountMain.Text = LocalAmount.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txttotalfcamount_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateLocalAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtexchangerate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateLocalAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtOuterQty_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateWeight();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbouterpackuom_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateWeight();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtweight_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtrate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbrateuom_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbItemUom_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateWeight();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtQtyDetail_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateWeight();
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
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)DataGridHistory).Name);
			if (fs != null)
			{
				DataGridHistory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void LcOrder_KeyDown(object sender, KeyEventArgs e)
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
					((Control)(object)DataGridHistory).Focus();
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
					if (formrights.DoHaveCanDelete && btnDelete.Enabled)
					{
						btnDelete_Click(null, null);
					}
				}
				else if (e.Control && e.KeyCode == Keys.S)
				{
					if (btnSave.Visible && btnSave.Enabled)
					{
						btnSave_Click(null, null);
					}
				}
				else if (e.Control && e.KeyCode == Keys.F12)
				{
					if (BtnSaveAs.Visible && BtnSaveAs.Enabled)
					{
						BtnSaveAs_Click(null, null);
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
					if (Print.Visible && !Print.Enabled)
					{
					}
				}
				else if (e.Control && e.KeyCode == Keys.N)
				{
					if (tabControl1.SelectedIndex == 0)
					{
						btnNew_Click(null, null);
					}
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
				else if (e.Control && e.KeyCode == Keys.Down)
				{
					if (tabControl2.SelectedIndex == 0)
					{
						((Control)(object)grdDetail).Focus();
					}
					else
					{
						((Control)(object)grdPaymentTerm).Focus();
					}
				}
				else if (e.Control && e.KeyCode == Keys.Up)
				{
					if (tabControl2.SelectedIndex == 0)
					{
						cmbItem.Focus();
					}
					else
					{
						CmbPaymentTermDetail.Focus();
					}
				}
				else if (e.Control && e.KeyCode == Keys.Right)
				{
					if (tabControl2.SelectedIndex == 0)
					{
						tabControl2.SelectedIndex = 1;
						((Control)(object)grdPaymentTerm).Focus();
					}
					else
					{
						tabControl2.SelectedIndex = 0;
						((Control)(object)grdDetail).Focus();
					}
				}
				else if (e.Control && e.KeyCode == Keys.F1)
				{
					btnSeaPortDefine_Click(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.F1)
				{
					btnPaymentTerms_Click(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.F1)
				{
					btnDeliveryTerm_Click(null, null);
				}
			}
			else if (tabControl1.SelectedIndex == 1)
			{
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grdHistoryDetail).Focus();
				}
				else if (e.Control && e.KeyCode == Keys.Up)
				{
					((Control)(object)DataGridHistory).Focus();
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
			dt.Rows.Add("Ctrl+S", "For Save");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+Shift+Delete", "For Delete");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Ctrl+F5", "For Focus on Doc Date");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("F12", "For Save As");
			dt.Rows.Add("Ctrl+F1", "For Sea Port Define");
			dt.Rows.Add("Ctrl+F2", "For Payment Term Define");
			dt.Rows.Add("Ctrl+F3", "For Delivery Term Define");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+D", "For Adding an row in Focused Grid");
			dt.Rows.Add("Ctrl+Delete", "For Deleting an row of Focused Grid");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Item in Detail Box");
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
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_034b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Expected O, but got Unknown
		//IL_032c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			GridEXRow item = grdDetail.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				grdDetail_DoubleClick(null, null);
			}
			else
			{
				if (!e.Control || e.KeyCode != Keys.Space)
				{
					return;
				}
				if (grdDetail.CurrentColumn.Key == "Edit")
				{
					grdDetail_DoubleClick(null, null);
				}
				if (grdDetail.CurrentColumn.Key == "Delete")
				{
					if (updateDetailIndex > -1)
					{
						MessageBox.Show("Please Reset Detail Box Data If You want to Delete Any row from the Grid....");
						return;
					}
					GridEXRow r = grdDetail.CurrentRow;
					if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
					{
						if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
						{
							return;
						}
						ImLcOrderPackingDetail vd = new ImLcOrderPackingDetail();
						vd.Id = Conversion.ToInt(r.Cells["Id"].Value);
						vd.ImItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
						vd.ItemBrandId = Conversion.ToInt(r.Cells["ItemId"].Value);
						vd.ItemDescription = Conversion.ToString((object)r.Cells["Description"].Text);
						vd.InvPackingTypeId = Conversion.ToInt(r.Cells["PackingTypeId"].Value);
						vd.ItemUomId = Conversion.ToInt(r.Cells["ItemUomId"].Value);
						vd.Qty = Conversion.ToDecimal(r.Cells["Qty"].Value);
						vd.NetWeight = Conversion.ToDecimal(r.Cells["NetWeight"].Value);
						vd.ItemRate = Conversion.ToDecimal(r.Cells["ItemRate"].Value);
						vd.RateUomId = Conversion.ToInt(r.Cells["RateUomId"].Value);
						vd.FcyAmount = Conversion.ToDecimal(r.Cells["FcyAmount"].Value);
						vd.CurrencyId = Conversion.ToInt(cmbCurreny.Value);
						vd.ExchangeRate = Conversion.ToDecimal((object)txtExchangeRate.Text);
						vd.LcyAmount = Conversion.ToDecimal(r.Cells["LcyAmount"].Value);
						vd.ItemAmount = Conversion.ToDecimal(r.Cells["LcyAmount"].Value);
						vd.RemarksDetail = Conversion.ToString((object)r.Cells["RemarksDetail"].Text);
						vd.ActionTypeId = 3;
						lstRemoveRecord.Add(vd);
						dtdetail.Rows[r.RowIndex].Delete();
						if (r != null && (int)r.RowType == 0)
						{
							r.Delete();
						}
						grdDetail.UpdateData();
					}
					else if (r != null && (int)r.RowType == 0)
					{
						r.Delete();
					}
				}
				CalculateLcyAmountInDetail();
				CalculateNetWeightAndFcyAmountInMain();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPaymentTerm_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			GridEXRow item = grdPaymentTerm.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				grdPaymentTerm_DoubleClick(null, null);
			}
			else
			{
				if (!e.Control || e.KeyCode != Keys.Space)
				{
					return;
				}
				if (grdPaymentTerm.CurrentColumn.Key == "Delete")
				{
					dtPaymnent.Rows[item.RowIndex].Delete();
					if (item != null && (int)item.RowType == 0)
					{
						item.Delete();
					}
				}
				grdPaymentTerm.UpdateData();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = DataGridHistory.CurrentRow;
			if (item == null || (int)item.RowType != 0 || DataGridHistory.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				int Id = Conversion.ToInt((object)item.Cells["Id"].Value.ToString());
				if (DataGridHistory.CurrentColumn.Key == "Edit")
				{
					if (!formrights.DoHaveUpdateRights && !formrights.DoHaveCanDelete)
					{
						MessageBox.Show("you Don't Have Update right....");
						return;
					}
					RecId = Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells["Id"].Value.ToString());
					ReadById(RecId);
					txtdocdate.Focus();
					btnDelete.Visible = true;
				}
				if (DataGridHistory.CurrentColumn.Key == "SaveAs")
				{
					if (!formrights.DoHaveSaveRight && !formrights.DoHaveCanDelete)
					{
						MessageBox.Show("you Don't Have Update right....");
						return;
					}
					RecId = Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells["Id"].Value.ToString());
					ReadById(RecId);
					txtdocdate.Focus();
					BtnSaveAs.Visible = true;
					btnDelete.Visible = true;
					btnSave.Visible = false;
					btnUpdate.Visible = false;
				}
				if (DataGridHistory.CurrentColumn.Key == "Print")
				{
					Id = Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells["Id"].Value.ToString());
					GenerateSlip(Id);
				}
				if (DataGridHistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt((object)Id.ToString()), 231);
				}
			}
			else if (e.Control && e.KeyCode == Keys.Return)
			{
				if (formrights.DoHaveUpdateRights || formrights.DoHaveCanDelete)
				{
					RecId = Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells["Id"].Value.ToString());
					ReadById(RecId);
					txtdocdate.Focus();
					btnDelete.Visible = true;
				}
				else
				{
					MessageBox.Show("you Don't Have Update right....");
				}
			}
			else if (e.Control && e.KeyCode == Keys.P && formrights.DoHavePrintRights)
			{
				RecId = Conversion.ToInt((object)item.Cells["Id"].Value.ToString());
				GenerateSlip(RecId);
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
			GenerateSlip(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateSlip(int PrintId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Expected O, but got Unknown
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.Id = PrintId;
			obj.DocumentTypeId = 231;
			DataTable dt = ImLcOrder.ImLcOrder_SlipAndRegister(obj);
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("Record Not Found For Display");
			}
			List<SubReportObject> lstSubReports = new List<SubReportObject>();
			SubReportObject subReportObject = new SubReportObject(ImLcOrder.ImLcOrderPaymentDetail_SubReport(obj.Id), "ImLcOrderPaymentDetail_SubReport.rpt");
			lstSubReports.Add(subReportObject);
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTableSubReprt(dt, "231-ImLcOrder-Slip.rpt", lstSubReports);
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

	private void txtExchangeRate_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtGrossWeightMain_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtNoOfContainerMain_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtItemRate_KeyPress(object sender, KeyPressEventArgs e)
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

	private void btnDeliveryTerm_Click(object sender, EventArgs e)
	{
		try
		{
			new DeliveryTerms(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPaymentTerms_Click(object sender, EventArgs e)
	{
		try
		{
			new PaymentTerms(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnSeaPortDefine_Click(object sender, EventArgs e)
	{
		try
		{
			new SeaPortsDefine(UserAccount).Show();
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
		//IL_070a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0713: Expected O, but got Unknown
		//IL_0721: Unknown result type (might be due to invalid IL or missing references)
		//IL_072b: Expected O, but got Unknown
		//IL_074d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0757: Expected O, but got Unknown
		//IL_07e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f1: Expected O, but got Unknown
		//IL_08ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b7: Expected O, but got Unknown
		//IL_08b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c2: Expected O, but got Unknown
		//IL_08c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_08cd: Expected O, but got Unknown
		//IL_08ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d8: Expected O, but got Unknown
		//IL_09f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a01: Expected O, but got Unknown
		//IL_0a65: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a6f: Expected O, but got Unknown
		//IL_0a7b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a85: Expected O, but got Unknown
		//IL_0a86: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a90: Expected O, but got Unknown
		//IL_0aa7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab1: Expected O, but got Unknown
		//IL_0ac8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad2: Expected O, but got Unknown
		//IL_0ade: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ae8: Expected O, but got Unknown
		//IL_0b0a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b14: Expected O, but got Unknown
		//IL_0b20: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b2a: Expected O, but got Unknown
		//IL_0b6d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b77: Expected O, but got Unknown
		//IL_0b83: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b8d: Expected O, but got Unknown
		//IL_0baf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bb9: Expected O, but got Unknown
		//IL_0c33: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c3d: Expected O, but got Unknown
		//IL_0c5f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c69: Expected O, but got Unknown
		//IL_0ccd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cd7: Expected O, but got Unknown
		//IL_0dd5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ddf: Expected O, but got Unknown
		//IL_0de0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dea: Expected O, but got Unknown
		//IL_0f14: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f1e: Expected O, but got Unknown
		//IL_15fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_1604: Expected O, but got Unknown
		//IL_1628: Unknown result type (might be due to invalid IL or missing references)
		//IL_1632: Expected O, but got Unknown
		//IL_1f02: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f0c: Expected O, but got Unknown
		//IL_cf9a: Unknown result type (might be due to invalid IL or missing references)
		//IL_cfa4: Expected O, but got Unknown
		GridEXLayout grdHistoryDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance224 = new Appearance();
		Appearance appearance225 = new Appearance();
		Appearance appearance226 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance237 = new Appearance();
		Appearance appearance238 = new Appearance();
		Appearance appearance239 = new Appearance();
		Appearance appearance240 = new Appearance();
		Appearance appearance241 = new Appearance();
		Appearance appearance243 = new Appearance();
		Appearance appearance244 = new Appearance();
		Appearance appearance245 = new Appearance();
		Appearance appearance246 = new Appearance();
		Appearance appearance247 = new Appearance();
		Appearance appearance248 = new Appearance();
		Appearance appearance249 = new Appearance();
		Appearance appearance209 = new Appearance();
		Appearance appearance211 = new Appearance();
		Appearance appearance212 = new Appearance();
		Appearance appearance213 = new Appearance();
		Appearance appearance214 = new Appearance();
		Appearance appearance215 = new Appearance();
		Appearance appearance216 = new Appearance();
		Appearance appearance217 = new Appearance();
		Appearance appearance218 = new Appearance();
		Appearance appearance219 = new Appearance();
		Appearance appearance220 = new Appearance();
		Appearance appearance222 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance185 = new Appearance();
		Appearance appearance186 = new Appearance();
		Appearance appearance187 = new Appearance();
		Appearance appearance189 = new Appearance();
		Appearance appearance190 = new Appearance();
		Appearance appearance191 = new Appearance();
		Appearance appearance192 = new Appearance();
		Appearance appearance193 = new Appearance();
		Appearance appearance194 = new Appearance();
		Appearance appearance195 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance157 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance168 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance196 = new Appearance();
		Appearance appearance197 = new Appearance();
		Appearance appearance198 = new Appearance();
		Appearance appearance200 = new Appearance();
		Appearance appearance201 = new Appearance();
		Appearance appearance202 = new Appearance();
		Appearance appearance203 = new Appearance();
		Appearance appearance204 = new Appearance();
		Appearance appearance205 = new Appearance();
		Appearance appearance206 = new Appearance();
		Appearance appearance207 = new Appearance();
		Appearance appearance208 = new Appearance();
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
		Appearance appearance1 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance188 = new Appearance();
		Appearance appearance199 = new Appearance();
		Appearance appearance210 = new Appearance();
		Appearance appearance221 = new Appearance();
		Appearance appearance232 = new Appearance();
		Appearance appearance236 = new Appearance();
		Appearance appearance242 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance223 = new Appearance();
		Appearance appearance227 = new Appearance();
		Appearance appearance228 = new Appearance();
		Appearance appearance229 = new Appearance();
		Appearance appearance230 = new Appearance();
		Appearance appearance231 = new Appearance();
		Appearance appearance233 = new Appearance();
		Appearance appearance234 = new Appearance();
		Appearance appearance235 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance169 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance171 = new Appearance();
		Appearance appearance172 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance180 = new Appearance();
		Appearance appearance181 = new Appearance();
		Appearance appearance182 = new Appearance();
		Appearance appearance183 = new Appearance();
		Appearance appearance184 = new Appearance();
		Appearance appearance250 = new Appearance();
		Appearance appearance251 = new Appearance();
		Appearance appearance252 = new Appearance();
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
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Import.ImLcOrder));
		GridEXLayout grdPaymentTerm_DesignTimeLayout = new GridEXLayout();
		this.History = new System.Windows.Forms.TabPage();
		this.grdHistoryDetail = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.label59 = new System.Windows.Forms.Label();
		this.ctrlGrdBar4 = new grid.User_Conrtols.CtrlGrdBar();
		this.DataGridHistory = new GridEX();
		this.panel9 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.btnLoadAllHistory = new System.Windows.Forms.ToolStripButton();
		this.label56 = new System.Windows.Forms.Label();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.Form = new System.Windows.Forms.TabPage();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.Detail = new System.Windows.Forms.TabPage();
		this.grdDetail = new GridEX();
		this.panel7 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label33 = new System.Windows.Forms.Label();
		this.panel10 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.rdSearchByCode = new System.Windows.Forms.RadioButton();
		this.rdSearchByName = new System.Windows.Forms.RadioButton();
		this.label46 = new System.Windows.Forms.Label();
		this.label28 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.label80 = new System.Windows.Forms.Label();
		this.txtQtyDetail = new System.Windows.Forms.TextBox();
		this.label49 = new System.Windows.Forms.Label();
		this.label79 = new System.Windows.Forms.Label();
		this.label78 = new System.Windows.Forms.Label();
		this.label77 = new System.Windows.Forms.Label();
		this.label74 = new System.Windows.Forms.Label();
		this.cmbRateUom = new UltraCombo();
		this.cmbItemUom = new UltraCombo();
		this.cmbPackingType = new UltraCombo();
		this.cmbItem = new UltraCombo();
		this.btnCancel = new System.Windows.Forms.Button();
		this.btnGridUpdate = new System.Windows.Forms.Button();
		this.btnaddnew = new System.Windows.Forms.Button();
		this.label47 = new System.Windows.Forms.Label();
		this.label48 = new System.Windows.Forms.Label();
		this.label50 = new System.Windows.Forms.Label();
		this.txtFcyAmountDetail = new System.Windows.Forms.TextBox();
		this.label81 = new System.Windows.Forms.Label();
		this.label52 = new System.Windows.Forms.Label();
		this.txtItemRate = new System.Windows.Forms.TextBox();
		this.label53 = new System.Windows.Forms.Label();
		this.txtWeightDetail = new System.Windows.Forms.TextBox();
		this.label54 = new System.Windows.Forms.Label();
		this.txtItemDescription = new System.Windows.Forms.TextBox();
		this.label55 = new System.Windows.Forms.Label();
		this.txtRemarksDetail = new System.Windows.Forms.TextBox();
		this.Payment = new System.Windows.Forms.TabPage();
		this.panel14 = new System.Windows.Forms.Panel();
		this.btnCancelPayment = new System.Windows.Forms.Button();
		this.btnUpdatePayment = new System.Windows.Forms.Button();
		this.btnAddPayment = new System.Windows.Forms.Button();
		this.label3 = new System.Windows.Forms.Label();
		this.txtPaymentDetailFcyAmount = new System.Windows.Forms.TextBox();
		this.PercntOfTotal = new System.Windows.Forms.TextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.CmbPaymentTermDetail = new UltraCombo();
		this.panel5 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.txtRemarksHeader = new System.Windows.Forms.RichTextBox();
		this.label11 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.txtSupLcOrderDate = new System.Windows.Forms.DateTimePicker();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.txtdocdate = new System.Windows.Forms.DateTimePicker();
		this.txtQuorationRef = new System.Windows.Forms.TextBox();
		this.cmbOrderStatus = new UltraCombo();
		this.label7 = new System.Windows.Forms.Label();
		this.cmbLegalizationRequired = new UltraCombo();
		this.cmbShippedTo = new UltraCombo();
		this.label82 = new System.Windows.Forms.Label();
		this.txtInquiryRef = new System.Windows.Forms.TextBox();
		this.cmbNotifyParty = new UltraCombo();
		this.label9 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.cmbSalesPerson = new UltraCombo();
		this.label83 = new System.Windows.Forms.Label();
		this.cmbSupplier = new UltraCombo();
		this.label42 = new System.Windows.Forms.Label();
		this.txtSupLcOrderNo = new System.Windows.Forms.TextBox();
		this.txtExpiryDate = new System.Windows.Forms.DateTimePicker();
		this.cmbInspectionRequired = new UltraCombo();
		this.label12 = new System.Windows.Forms.Label();
		this.cmbCurreny = new UltraCombo();
		this.label44 = new System.Windows.Forms.Label();
		this.txtExpiryPlace = new System.Windows.Forms.TextBox();
		this.label38 = new System.Windows.Forms.Label();
		this.label31 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.label26 = new System.Windows.Forms.Label();
		this.cmbPaymenTerm = new UltraCombo();
		this.label25 = new System.Windows.Forms.Label();
		this.cmbDestinationPort = new UltraCombo();
		this.label24 = new System.Windows.Forms.Label();
		this.cmbTransShipment = new System.Windows.Forms.TextBox();
		this.txtTotalFcyAmountMain = new System.Windows.Forms.TextBox();
		this.cmbLoadingPort = new UltraCombo();
		this.txtExchangeRate = new System.Windows.Forms.TextBox();
		this.txtShippingMarks = new System.Windows.Forms.TextBox();
		this.txtLcyAmountMain = new System.Windows.Forms.TextBox();
		this.label16 = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.txtInspectionDesc = new System.Windows.Forms.TextBox();
		this.txtNetWeightMain = new System.Windows.Forms.TextBox();
		this.label19 = new System.Windows.Forms.Label();
		this.label15 = new System.Windows.Forms.Label();
		this.txtLegalizationDesc = new System.Windows.Forms.TextBox();
		this.cmbExporterBanks = new UltraCombo();
		this.label14 = new System.Windows.Forms.Label();
		this.label41 = new System.Windows.Forms.Label();
		this.txtLastShipmentDate = new System.Windows.Forms.DateTimePicker();
		this.CmbImporterBank = new UltraCombo();
		this.cmbPartialShipment = new System.Windows.Forms.TextBox();
		this.txtGrossWeightMain = new System.Windows.Forms.TextBox();
		this.txtNoOfContainerMain = new System.Windows.Forms.TextBox();
		this.txtCommodityDescMain = new System.Windows.Forms.TextBox();
		this.label39 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.label43 = new System.Windows.Forms.Label();
		this.label40 = new System.Windows.Forms.Label();
		this.label34 = new System.Windows.Forms.Label();
		this.cmbDeliveryTerm = new UltraCombo();
		this.label32 = new System.Windows.Forms.Label();
		this.label36 = new System.Windows.Forms.Label();
		this.label37 = new System.Windows.Forms.Label();
		this.label35 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.label22 = new System.Windows.Forms.Label();
		this.label30 = new System.Windows.Forms.Label();
		this.label29 = new System.Windows.Forms.Label();
		this.label72 = new System.Windows.Forms.Label();
		this.label70 = new System.Windows.Forms.Label();
		this.label60 = new System.Windows.Forms.Label();
		this.label69 = new System.Windows.Forms.Label();
		this.label68 = new System.Windows.Forms.Label();
		this.label67 = new System.Windows.Forms.Label();
		this.label66 = new System.Windows.Forms.Label();
		this.label65 = new System.Windows.Forms.Label();
		this.label64 = new System.Windows.Forms.Label();
		this.label63 = new System.Windows.Forms.Label();
		this.label62 = new System.Windows.Forms.Label();
		this.label61 = new System.Windows.Forms.Label();
		this.label58 = new System.Windows.Forms.Label();
		this.label57 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.cmbproject = new UltraCombo();
		this.cmbbranch = new UltraCombo();
		this.label5 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.btnclose = new System.Windows.Forms.Button();
		this.label23 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.BtnSaveAs = new System.Windows.Forms.ToolStripButton();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.Print = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
		this.BtnShortCutkeys = new System.Windows.Forms.ToolStripButton();
		this.btnSeaPortDefine = new System.Windows.Forms.ToolStripButton();
		this.btnDefinePaymentTerm = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
		this.panel11 = new System.Windows.Forms.Panel();
		this.label45 = new System.Windows.Forms.Label();
		this.grdPaymentTerm = new GridEX();
		this.History.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryDetail).BeginInit();
		this.panel8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel9.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.Form.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel6.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.Detail.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel7.SuspendLayout();
		this.panel10.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbRateUom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemUom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPackingType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).BeginInit();
		this.Payment.SuspendLayout();
		this.panel14.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPaymentTermDetail).BeginInit();
		this.panel5.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbOrderStatus).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbLegalizationRequired).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbShippedTo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbNotifyParty).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSalesPerson).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSupplier).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbInspectionRequired).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCurreny).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPaymenTerm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDestinationPort).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbLoadingPort).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbExporterBanks).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbImporterBank).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDeliveryTerm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbproject).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbbranch).BeginInit();
		this.panel4.SuspendLayout();
		this.panel3.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		this.panel11.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPaymentTerm).BeginInit();
		base.SuspendLayout();
		this.History.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistoryDetail);
		this.History.Controls.Add(this.panel8);
		this.History.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.History.Controls.Add(this.panel9);
		this.History.Location = new System.Drawing.Point(4, 4);
		this.History.Margin = new System.Windows.Forms.Padding(2);
		this.History.Name = "History";
		this.History.Padding = new System.Windows.Forms.Padding(2);
		this.History.Size = new System.Drawing.Size(1176, 682);
		this.History.TabIndex = 1;
		this.History.Text = "History";
		this.History.UseVisualStyleBackColor = true;
		this.grdHistoryDetail.AllowEdit = (InheritableBoolean)2;
		grdHistoryDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistoryDetail.DesignTimeLayout = grdHistoryDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistoryDetail.DynamicFiltering = true;
		this.grdHistoryDetail.FilterMode = (FilterMode)2;
		this.grdHistoryDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistoryDetail.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Location = new System.Drawing.Point(2, 459);
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Name = "grdHistoryDetail";
		((ScrollableBase)this.grdHistoryDetail).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdHistoryDetail).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdHistoryDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Size = new System.Drawing.Size(1172, 221);
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).TabIndex = 47;
		this.grdHistoryDetail.TotalRow = (InheritableBoolean)1;
		this.grdHistoryDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistoryDetail).VisualStyle = (VisualStyle)4;
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.label59);
		this.panel8.Controls.Add(this.ctrlGrdBar4);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(2, 432);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1172, 27);
		this.panel8.TabIndex = 46;
		this.label59.AutoSize = true;
		this.label59.BackColor = System.Drawing.Color.Teal;
		this.label59.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label59.ForeColor = System.Drawing.Color.White;
		this.label59.Location = new System.Drawing.Point(6, 4);
		this.label59.Name = "label59";
		this.label59.Size = new System.Drawing.Size(159, 17);
		this.label59.TabIndex = 1;
		this.label59.Text = "Detail of selected Row";
		this.ctrlGrdBar4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar4.FormName = this;
		this.ctrlGrdBar4.Location = new System.Drawing.Point(1128, 2);
		this.ctrlGrdBar4.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar4.MyGrid = this.grdHistoryDetail;
		this.ctrlGrdBar4.Name = "ctrlGrdBar4";
		this.ctrlGrdBar4.Size = new System.Drawing.Size(44, 25);
		this.ctrlGrdBar4.TabIndex = 2;
		this.DataGridHistory.AllowEdit = (InheritableBoolean)2;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Top;
		this.DataGridHistory.DynamicFiltering = true;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(2, 55);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.DataGridHistory).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1172, 377);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 45;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		this.DataGridHistory.SelectionChanged += new System.EventHandler(DataGridHistory_SelectionChanged);
		this.DataGridHistory.LinkClicked += new ColumnActionEventHandler(DataGridHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).DoubleClick += new System.EventHandler(DataGridHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(DataGridHistory_KeyDown);
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.toolStrip1);
		this.panel9.Controls.Add(this.label56);
		this.panel9.Controls.Add(this.ctrlGrdBar2);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(2, 2);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1172, 53);
		this.panel9.TabIndex = 44;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.toolStripSeparator1, this.btnLoadAllHistory });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1172, 27);
		this.toolStrip1.TabIndex = 3;
		this.toolStrip1.Text = "toolStrip1";
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.btnLoadAllHistory.Image = Architecture.WinApp.Properties.Resources.download_line;
		this.btnLoadAllHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadAllHistory.Name = "btnLoadAllHistory";
		this.btnLoadAllHistory.Size = new System.Drawing.Size(82, 24);
		this.btnLoadAllHistory.Text = "&Load All";
		this.btnLoadAllHistory.Click += new System.EventHandler(btnLoadAllHistory_Click);
		this.label56.AutoSize = true;
		this.label56.BackColor = System.Drawing.Color.Teal;
		this.label56.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label56.ForeColor = System.Drawing.Color.White;
		this.label56.Location = new System.Drawing.Point(6, 29);
		this.label56.Name = "label56";
		this.label56.Size = new System.Drawing.Size(231, 19);
		this.label56.TabIndex = 1;
		this.label56.Text = "Import: Lc Contract History";
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1120, 28);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(45, 24);
		this.ctrlGrdBar2.TabIndex = 2;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1184, 711);
		this.panel1.TabIndex = 50;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.Form);
		this.tabControl1.Controls.Add(this.History);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1184, 711);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.Form.Controls.Add(this.panel2);
		this.Form.Location = new System.Drawing.Point(4, 4);
		this.Form.Margin = new System.Windows.Forms.Padding(2);
		this.Form.Name = "Form";
		this.Form.Padding = new System.Windows.Forms.Padding(2);
		this.Form.Size = new System.Drawing.Size(1176, 682);
		this.Form.TabIndex = 0;
		this.Form.Text = "Form";
		this.Form.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add(this.panel6);
		this.panel2.Controls.Add(this.panel5);
		this.panel2.Controls.Add(this.panel4);
		this.panel2.Controls.Add(this.panel3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(2, 2);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1172, 678);
		this.panel2.TabIndex = 0;
		this.panel6.Controls.Add(this.tabControl2);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(0, 397);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1172, 281);
		this.panel6.TabIndex = 1;
		this.tabControl2.Controls.Add(this.Detail);
		this.tabControl2.Controls.Add(this.Payment);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl2.Location = new System.Drawing.Point(0, 0);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(1172, 281);
		this.tabControl2.TabIndex = 0;
		this.Detail.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.Detail.Controls.Add(this.panel7);
		this.Detail.Controls.Add(this.panel10);
		this.Detail.Location = new System.Drawing.Point(4, 25);
		this.Detail.Name = "Detail";
		this.Detail.Padding = new System.Windows.Forms.Padding(3);
		this.Detail.Size = new System.Drawing.Size(1164, 252);
		this.Detail.TabIndex = 0;
		this.Detail.Text = "Detail";
		this.Detail.UseVisualStyleBackColor = true;
		this.grdDetail.AllowEdit = (InheritableBoolean)2;
		grdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdDetail.DynamicFiltering = true;
		this.grdDetail.FilterMode = (FilterMode)2;
		this.grdDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(3, 144);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdDetail).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1158, 105);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 1;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.grdDetail.ColumnButtonClick += new ColumnActionEventHandler(grdDetail_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdDetail).DoubleClick += new System.EventHandler(grdDetail_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdDetail).KeyDown += new System.Windows.Forms.KeyEventHandler(grdDetail_KeyDown);
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.ctrlGrdBar1);
		this.panel7.Controls.Add(this.label33);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(3, 121);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1158, 23);
		this.panel7.TabIndex = 3;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1107, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdDetail;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 21);
		this.ctrlGrdBar1.TabIndex = 0;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label33.ForeColor = System.Drawing.Color.White;
		this.label33.Location = new System.Drawing.Point(8, 2);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(46, 17);
		this.label33.TabIndex = 0;
		this.label33.Text = "Detail";
		this.panel10.Controls.Add(this.groupBox1);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(3, 3);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1158, 118);
		this.panel10.TabIndex = 0;
		this.groupBox1.Controls.Add(this.rdSearchByCode);
		this.groupBox1.Controls.Add(this.rdSearchByName);
		this.groupBox1.Controls.Add(this.label46);
		this.groupBox1.Controls.Add(this.label28);
		this.groupBox1.Controls.Add(this.label27);
		this.groupBox1.Controls.Add(this.label80);
		this.groupBox1.Controls.Add(this.txtQtyDetail);
		this.groupBox1.Controls.Add(this.label49);
		this.groupBox1.Controls.Add(this.label79);
		this.groupBox1.Controls.Add(this.label78);
		this.groupBox1.Controls.Add(this.label77);
		this.groupBox1.Controls.Add(this.label74);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbRateUom);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemUom);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPackingType);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItem);
		this.groupBox1.Controls.Add(this.btnCancel);
		this.groupBox1.Controls.Add(this.btnGridUpdate);
		this.groupBox1.Controls.Add(this.btnaddnew);
		this.groupBox1.Controls.Add(this.label47);
		this.groupBox1.Controls.Add(this.label48);
		this.groupBox1.Controls.Add(this.label50);
		this.groupBox1.Controls.Add(this.txtFcyAmountDetail);
		this.groupBox1.Controls.Add(this.label81);
		this.groupBox1.Controls.Add(this.label52);
		this.groupBox1.Controls.Add(this.txtItemRate);
		this.groupBox1.Controls.Add(this.label53);
		this.groupBox1.Controls.Add(this.txtWeightDetail);
		this.groupBox1.Controls.Add(this.label54);
		this.groupBox1.Controls.Add(this.txtItemDescription);
		this.groupBox1.Controls.Add(this.label55);
		this.groupBox1.Controls.Add(this.txtRemarksDetail);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(6, 5);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1156, 115);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Detail";
		this.rdSearchByCode.AutoSize = true;
		this.rdSearchByCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdSearchByCode.Location = new System.Drawing.Point(154, 18);
		this.rdSearchByCode.Name = "rdSearchByCode";
		this.rdSearchByCode.Size = new System.Drawing.Size(52, 19);
		this.rdSearchByCode.TabIndex = 495;
		this.rdSearchByCode.Text = "Code";
		this.rdSearchByCode.UseVisualStyleBackColor = true;
		this.rdSearchByName.AutoSize = true;
		this.rdSearchByName.Checked = true;
		this.rdSearchByName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdSearchByName.Location = new System.Drawing.Point(76, 18);
		this.rdSearchByName.Name = "rdSearchByName";
		this.rdSearchByName.Size = new System.Drawing.Size(57, 19);
		this.rdSearchByName.TabIndex = 494;
		this.rdSearchByName.TabStop = true;
		this.rdSearchByName.Text = "Name";
		this.rdSearchByName.UseVisualStyleBackColor = true;
		this.label46.AutoSize = true;
		this.label46.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label46.Location = new System.Drawing.Point(6, 20);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(35, 15);
		this.label46.TabIndex = 493;
		this.label46.Text = "Item ";
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.ForeColor = System.Drawing.Color.Red;
		this.label28.Location = new System.Drawing.Point(862, 20);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(12, 15);
		this.label28.TabIndex = 65;
		this.label28.Text = "*";
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.ForeColor = System.Drawing.Color.Red;
		this.label27.Location = new System.Drawing.Point(961, 20);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(12, 15);
		this.label27.TabIndex = 64;
		this.label27.Text = "*";
		this.label80.AutoSize = true;
		this.label80.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label80.ForeColor = System.Drawing.Color.Red;
		this.label80.Location = new System.Drawing.Point(302, 67);
		this.label80.Name = "label80";
		this.label80.Size = new System.Drawing.Size(12, 15);
		this.label80.TabIndex = 63;
		this.label80.Text = "*";
		this.txtQtyDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtQtyDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtQtyDetail.Location = new System.Drawing.Point(903, 39);
		this.txtQtyDetail.Name = "txtQtyDetail";
		this.txtQtyDetail.Size = new System.Drawing.Size(103, 23);
		this.txtQtyDetail.TabIndex = 4;
		this.txtQtyDetail.TextChanged += new System.EventHandler(txtQtyDetail_TextChanged);
		this.txtQtyDetail.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtQtyDetail_KeyPress);
		this.label49.AutoSize = true;
		this.label49.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label49.Location = new System.Drawing.Point(906, 20);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(26, 15);
		this.label49.TabIndex = 46;
		this.label49.Text = "Qty";
		this.label79.AutoSize = true;
		this.label79.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label79.ForeColor = System.Drawing.Color.Red;
		this.label79.Location = new System.Drawing.Point(184, 67);
		this.label79.Name = "label79";
		this.label79.Size = new System.Drawing.Size(12, 15);
		this.label79.TabIndex = 62;
		this.label79.Text = "*";
		this.label78.AutoSize = true;
		this.label78.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label78.ForeColor = System.Drawing.Color.Red;
		this.label78.Location = new System.Drawing.Point(93, 67);
		this.label78.Name = "label78";
		this.label78.Size = new System.Drawing.Size(12, 15);
		this.label78.TabIndex = 61;
		this.label78.Text = "*";
		this.label77.AutoSize = true;
		this.label77.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label77.ForeColor = System.Drawing.Color.Red;
		this.label77.Location = new System.Drawing.Point(1092, 20);
		this.label77.Name = "label77";
		this.label77.Size = new System.Drawing.Size(12, 15);
		this.label77.TabIndex = 60;
		this.label77.Text = "*";
		this.label74.AutoSize = true;
		this.label74.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label74.ForeColor = System.Drawing.Color.Red;
		this.label74.Location = new System.Drawing.Point(721, 20);
		this.label74.Name = "label74";
		this.label74.Size = new System.Drawing.Size(12, 15);
		this.label74.TabIndex = 57;
		this.label74.Text = "*";
		this.cmbRateUom.AllowNull = (DefaultableBoolean)1;
		this.cmbRateUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbRateUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbRateUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance224).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance224).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbRateUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance224;
		((UltraGridBase)this.cmbRateUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbRateUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance225).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance225).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance225).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance225).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbRateUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance225;
		((AppearanceBase)appearance226).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbRateUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance226;
		((SpecialBoxBase)((UltraGridBase)this.cmbRateUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance53).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbRateUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.cmbRateUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbRateUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbRateUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance54;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbRateUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.cmbRateUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbRateUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbRateUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance76).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbRateUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.cmbRateUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbRateUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance77).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance77).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance77).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance77).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbRateUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance77;
		((AppearanceBase)appearance78).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbRateUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.cmbRateUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbRateUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbRateUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.cmbRateUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbRateUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.cmbRateUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbRateUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbRateUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbRateUom).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbRateUom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbRateUom).Location = new System.Drawing.Point(119, 84);
		((System.Windows.Forms.Control)(object)this.cmbRateUom).Name = "cmbRateUom";
		((System.Windows.Forms.Control)(object)this.cmbRateUom).Size = new System.Drawing.Size(100, 26);
		((System.Windows.Forms.Control)(object)this.cmbRateUom).TabIndex = 7;
		this.cmbRateUom.ValueChanged += new System.EventHandler(cmbrateuom_TextChanged);
		this.cmbItemUom.AllowNull = (DefaultableBoolean)1;
		this.cmbItemUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance95).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.cmbItemUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance101).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance101;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance102).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance102).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance102).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.cmbItemUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance103).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance103;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance104).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.cmbItemUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance105;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance106).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.cmbItemUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance107).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance107).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance107).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance107).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance107;
		((AppearanceBase)appearance108).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.cmbItemUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance114).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.cmbItemUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.cmbItemUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemUom).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItemUom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemUom).Location = new System.Drawing.Point(798, 37);
		((System.Windows.Forms.Control)(object)this.cmbItemUom).Name = "cmbItemUom";
		((System.Windows.Forms.Control)(object)this.cmbItemUom).Size = new System.Drawing.Size(102, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemUom).TabIndex = 3;
		this.cmbItemUom.ValueChanged += new System.EventHandler(cmbItemUom_ValueChanged);
		this.cmbPackingType.AllowNull = (DefaultableBoolean)1;
		this.cmbPackingType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPackingType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPackingType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance116).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance117).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance117).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPackingType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance117;
		((AppearanceBase)appearance118).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance118;
		((SpecialBoxBase)((UltraGridBase)this.cmbPackingType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance119).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance119).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance119).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance120).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance122).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance123;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance127).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance128).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance128).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance128).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance128;
		((AppearanceBase)appearance129).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPackingType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPackingType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbPackingType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPackingType).Location = new System.Drawing.Point(642, 37);
		((System.Windows.Forms.Control)(object)this.cmbPackingType).Name = "cmbPackingType";
		((System.Windows.Forms.Control)(object)this.cmbPackingType).Size = new System.Drawing.Size(153, 26);
		((System.Windows.Forms.Control)(object)this.cmbPackingType).TabIndex = 2;
		this.cmbItem.AllowNull = (DefaultableBoolean)1;
		this.cmbItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.cmbItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance134).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance134).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance134;
		((AppearanceBase)appearance135).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance135;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance136).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance136).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance136).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance137).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance137;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance138).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance139;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance140).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance141).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance141).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance141).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance141).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance141;
		((AppearanceBase)appearance142).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance144).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItem).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItem).Location = new System.Drawing.Point(6, 37);
		((System.Windows.Forms.Control)(object)this.cmbItem).Name = "cmbItem";
		((System.Windows.Forms.Control)(object)this.cmbItem).Size = new System.Drawing.Size(213, 26);
		((System.Windows.Forms.Control)(object)this.cmbItem).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbItem).Leave += new System.EventHandler(cmbitem_Leave);
		this.btnCancel.BackColor = System.Drawing.Color.Teal;
		this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.Green;
		this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancel.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancel.ForeColor = System.Drawing.Color.White;
		this.btnCancel.Location = new System.Drawing.Point(1082, 85);
		this.btnCancel.Name = "btnCancel";
		this.btnCancel.Size = new System.Drawing.Size(68, 25);
		this.btnCancel.TabIndex = 11;
		this.btnCancel.Text = "Cancel";
		this.btnCancel.UseVisualStyleBackColor = false;
		this.btnCancel.Visible = false;
		this.btnCancel.Click += new System.EventHandler(btnCancel_Click);
		this.btnGridUpdate.BackColor = System.Drawing.Color.Teal;
		this.btnGridUpdate.FlatAppearance.BorderColor = System.Drawing.Color.Green;
		this.btnGridUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnGridUpdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnGridUpdate.ForeColor = System.Drawing.Color.White;
		this.btnGridUpdate.Location = new System.Drawing.Point(1009, 85);
		this.btnGridUpdate.Name = "btnGridUpdate";
		this.btnGridUpdate.Size = new System.Drawing.Size(71, 25);
		this.btnGridUpdate.TabIndex = 10;
		this.btnGridUpdate.Text = "Update";
		this.btnGridUpdate.UseVisualStyleBackColor = false;
		this.btnGridUpdate.Visible = false;
		this.btnGridUpdate.Click += new System.EventHandler(btnGridUpdate_Click);
		this.btnaddnew.BackColor = System.Drawing.Color.Teal;
		this.btnaddnew.FlatAppearance.BorderColor = System.Drawing.Color.Green;
		this.btnaddnew.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnaddnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnaddnew.ForeColor = System.Drawing.Color.White;
		this.btnaddnew.Location = new System.Drawing.Point(1009, 85);
		this.btnaddnew.Name = "btnaddnew";
		this.btnaddnew.Size = new System.Drawing.Size(41, 25);
		this.btnaddnew.TabIndex = 10;
		this.btnaddnew.Text = "+";
		this.btnaddnew.UseVisualStyleBackColor = false;
		this.btnaddnew.Click += new System.EventHandler(btnaddnew_Click);
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label47.Location = new System.Drawing.Point(798, 20);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(33, 15);
		this.label47.TabIndex = 43;
		this.label47.Text = "Uom";
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label48.Location = new System.Drawing.Point(119, 67);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(59, 15);
		this.label48.TabIndex = 41;
		this.label48.Text = "Rate Uom";
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label50.Location = new System.Drawing.Point(644, 20);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(77, 15);
		this.label50.TabIndex = 40;
		this.label50.Text = "Packing Type";
		this.txtFcyAmountDetail.BackColor = System.Drawing.Color.White;
		this.txtFcyAmountDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFcyAmountDetail.Enabled = false;
		this.txtFcyAmountDetail.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtFcyAmountDetail.Location = new System.Drawing.Point(223, 86);
		this.txtFcyAmountDetail.Name = "txtFcyAmountDetail";
		this.txtFcyAmountDetail.ReadOnly = true;
		this.txtFcyAmountDetail.Size = new System.Drawing.Size(120, 23);
		this.txtFcyAmountDetail.TabIndex = 8;
		this.label81.AutoSize = true;
		this.label81.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label81.Location = new System.Drawing.Point(343, 67);
		this.label81.Name = "label81";
		this.label81.Size = new System.Drawing.Size(52, 15);
		this.label81.TabIndex = 36;
		this.label81.Text = "Remarks";
		this.label52.AutoSize = true;
		this.label52.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label52.Location = new System.Drawing.Point(223, 67);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(72, 15);
		this.label52.TabIndex = 36;
		this.label52.Text = "Fcy Amount";
		this.txtItemRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemRate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtItemRate.Location = new System.Drawing.Point(6, 86);
		this.txtItemRate.Name = "txtItemRate";
		this.txtItemRate.Size = new System.Drawing.Size(111, 23);
		this.txtItemRate.TabIndex = 6;
		this.txtItemRate.TextChanged += new System.EventHandler(txtrate_TextChanged);
		this.txtItemRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtItemRate_KeyPress);
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label53.Location = new System.Drawing.Point(6, 67);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(87, 15);
		this.label53.TabIndex = 37;
		this.label53.Text = "Item Rate (Fcy)";
		this.txtWeightDetail.BackColor = System.Drawing.Color.White;
		this.txtWeightDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtWeightDetail.Enabled = false;
		this.txtWeightDetail.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtWeightDetail.Location = new System.Drawing.Point(1008, 39);
		this.txtWeightDetail.Name = "txtWeightDetail";
		this.txtWeightDetail.ReadOnly = true;
		this.txtWeightDetail.Size = new System.Drawing.Size(137, 23);
		this.txtWeightDetail.TabIndex = 5;
		this.txtWeightDetail.TextChanged += new System.EventHandler(txtweight_TextChanged);
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label54.Location = new System.Drawing.Point(1008, 20);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(68, 15);
		this.label54.TabIndex = 38;
		this.label54.Text = "Net Weight";
		this.txtItemDescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemDescription.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemDescription.Location = new System.Drawing.Point(223, 39);
		this.txtItemDescription.Name = "txtItemDescription";
		this.txtItemDescription.Size = new System.Drawing.Size(413, 23);
		this.txtItemDescription.TabIndex = 1;
		this.label55.AutoSize = true;
		this.label55.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label55.Location = new System.Drawing.Point(223, 20);
		this.label55.Name = "label55";
		this.label55.Size = new System.Drawing.Size(68, 15);
		this.label55.TabIndex = 39;
		this.label55.Text = "Description";
		this.txtRemarksDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarksDetail.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtRemarksDetail.Location = new System.Drawing.Point(346, 86);
		this.txtRemarksDetail.Name = "txtRemarksDetail";
		this.txtRemarksDetail.Size = new System.Drawing.Size(660, 23);
		this.txtRemarksDetail.TabIndex = 9;
		this.Payment.Controls.Add((System.Windows.Forms.Control)(object)this.grdPaymentTerm);
		this.Payment.Controls.Add(this.panel11);
		this.Payment.Controls.Add(this.panel14);
		this.Payment.Location = new System.Drawing.Point(4, 25);
		this.Payment.Name = "Payment";
		this.Payment.Padding = new System.Windows.Forms.Padding(3);
		this.Payment.Size = new System.Drawing.Size(1164, 252);
		this.Payment.TabIndex = 1;
		this.Payment.Text = "Payment";
		this.Payment.UseVisualStyleBackColor = true;
		this.panel14.Controls.Add(this.btnCancelPayment);
		this.panel14.Controls.Add(this.btnUpdatePayment);
		this.panel14.Controls.Add(this.btnAddPayment);
		this.panel14.Controls.Add(this.label3);
		this.panel14.Controls.Add(this.txtPaymentDetailFcyAmount);
		this.panel14.Controls.Add(this.PercntOfTotal);
		this.panel14.Controls.Add(this.label6);
		this.panel14.Controls.Add(this.label10);
		this.panel14.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPaymentTermDetail);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel14.Location = new System.Drawing.Point(3, 3);
		this.panel14.Margin = new System.Windows.Forms.Padding(2);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(1158, 63);
		this.panel14.TabIndex = 506;
		this.btnCancelPayment.BackColor = System.Drawing.Color.Teal;
		this.btnCancelPayment.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelPayment.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelPayment.ForeColor = System.Drawing.Color.White;
		this.btnCancelPayment.Location = new System.Drawing.Point(519, 28);
		this.btnCancelPayment.Margin = new System.Windows.Forms.Padding(2);
		this.btnCancelPayment.Name = "btnCancelPayment";
		this.btnCancelPayment.Size = new System.Drawing.Size(69, 24);
		this.btnCancelPayment.TabIndex = 5;
		this.btnCancelPayment.Text = "Cancel";
		this.btnCancelPayment.UseVisualStyleBackColor = false;
		this.btnCancelPayment.Click += new System.EventHandler(btnCancelPayment_Click);
		this.btnCancelPayment.Leave += new System.EventHandler(btnCancelPayment_Leave);
		this.btnUpdatePayment.BackColor = System.Drawing.Color.Teal;
		this.btnUpdatePayment.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdatePayment.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdatePayment.ForeColor = System.Drawing.Color.White;
		this.btnUpdatePayment.Location = new System.Drawing.Point(450, 28);
		this.btnUpdatePayment.Margin = new System.Windows.Forms.Padding(2);
		this.btnUpdatePayment.Name = "btnUpdatePayment";
		this.btnUpdatePayment.Size = new System.Drawing.Size(70, 24);
		this.btnUpdatePayment.TabIndex = 4;
		this.btnUpdatePayment.Text = "Update";
		this.btnUpdatePayment.UseVisualStyleBackColor = false;
		this.btnUpdatePayment.Click += new System.EventHandler(btnUpdatePayment_Click);
		this.btnAddPayment.BackColor = System.Drawing.Color.Teal;
		this.btnAddPayment.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAddPayment.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAddPayment.ForeColor = System.Drawing.Color.White;
		this.btnAddPayment.Location = new System.Drawing.Point(420, 28);
		this.btnAddPayment.Margin = new System.Windows.Forms.Padding(2);
		this.btnAddPayment.Name = "btnAddPayment";
		this.btnAddPayment.Size = new System.Drawing.Size(31, 24);
		this.btnAddPayment.TabIndex = 3;
		this.btnAddPayment.Text = "+";
		this.btnAddPayment.UseVisualStyleBackColor = false;
		this.btnAddPayment.Click += new System.EventHandler(btnAddPayment_Click);
		this.btnAddPayment.Leave += new System.EventHandler(btnAddPayment_Leave);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(289, 6);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(72, 15);
		this.label3.TabIndex = 562;
		this.label3.Text = "Fcy Amount";
		this.txtPaymentDetailFcyAmount.Enabled = false;
		this.txtPaymentDetailFcyAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPaymentDetailFcyAmount.Location = new System.Drawing.Point(287, 28);
		this.txtPaymentDetailFcyAmount.Name = "txtPaymentDetailFcyAmount";
		this.txtPaymentDetailFcyAmount.Size = new System.Drawing.Size(131, 23);
		this.txtPaymentDetailFcyAmount.TabIndex = 2;
		this.txtPaymentDetailFcyAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.PercntOfTotal.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.PercntOfTotal.Location = new System.Drawing.Point(183, 28);
		this.PercntOfTotal.Name = "PercntOfTotal";
		this.PercntOfTotal.Size = new System.Drawing.Size(100, 23);
		this.PercntOfTotal.TabIndex = 1;
		this.PercntOfTotal.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.PercntOfTotal.TextChanged += new System.EventHandler(PercntOfTotal_TextChanged);
		this.PercntOfTotal.KeyPress += new System.Windows.Forms.KeyPressEventHandler(PercntOfTotal_KeyPress);
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(193, 6);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(62, 15);
		this.label6.TabIndex = 503;
		this.label6.Text = "% Of Total";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(7, 6);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(84, 15);
		this.label10.TabIndex = 502;
		this.label10.Text = "Payment Term";
		this.CmbPaymentTermDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbPaymentTermDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPaymentTermDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPaymentTermDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance237).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance237).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPaymentTermDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance237;
		((UltraGridBase)this.CmbPaymentTermDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPaymentTermDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance238).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance238).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance238).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance238).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPaymentTermDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance238;
		((AppearanceBase)appearance239).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPaymentTermDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance239;
		((SpecialBoxBase)((UltraGridBase)this.CmbPaymentTermDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance240).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance240).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance240).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance240).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPaymentTermDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance240;
		((UltraGridBase)this.CmbPaymentTermDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPaymentTermDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance241).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance241).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPaymentTermDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance241;
		((AppearanceBase)appearance243).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance243).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPaymentTermDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance243;
		((UltraGridBase)this.CmbPaymentTermDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPaymentTermDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance244).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPaymentTermDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance244;
		((AppearanceBase)appearance245).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance245).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPaymentTermDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance245;
		((UltraGridBase)this.CmbPaymentTermDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPaymentTermDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance246).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance246).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance246).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance246).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance246).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPaymentTermDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance246;
		((AppearanceBase)appearance247).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPaymentTermDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance247;
		((UltraGridBase)this.CmbPaymentTermDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPaymentTermDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance248).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance248).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPaymentTermDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance248;
		((UltraGridBase)this.CmbPaymentTermDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance249).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPaymentTermDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance249;
		((UltraGridBase)this.CmbPaymentTermDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPaymentTermDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPaymentTermDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermDetail).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPaymentTermDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermDetail).Location = new System.Drawing.Point(7, 28);
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermDetail).Name = "CmbPaymentTermDetail";
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermDetail).Size = new System.Drawing.Size(171, 26);
		((System.Windows.Forms.Control)(object)this.CmbPaymentTermDetail).TabIndex = 0;
		this.panel5.BackColor = System.Drawing.Color.White;
		this.panel5.Controls.Add(this.groupBox2);
		this.panel5.Controls.Add(this.label30);
		this.panel5.Controls.Add(this.label29);
		this.panel5.Controls.Add(this.label72);
		this.panel5.Controls.Add(this.label70);
		this.panel5.Controls.Add(this.label60);
		this.panel5.Controls.Add(this.label69);
		this.panel5.Controls.Add(this.label68);
		this.panel5.Controls.Add(this.label67);
		this.panel5.Controls.Add(this.label66);
		this.panel5.Controls.Add(this.label65);
		this.panel5.Controls.Add(this.label64);
		this.panel5.Controls.Add(this.label63);
		this.panel5.Controls.Add(this.label62);
		this.panel5.Controls.Add(this.label61);
		this.panel5.Controls.Add(this.label58);
		this.panel5.Controls.Add(this.label57);
		this.panel5.Controls.Add(this.label1);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbproject);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbbranch);
		this.panel5.Controls.Add(this.label5);
		this.panel5.Controls.Add(this.label4);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.panel5.Location = new System.Drawing.Point(0, 55);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1172, 342);
		this.panel5.TabIndex = 0;
		this.groupBox2.Controls.Add(this.txtRemarksHeader);
		this.groupBox2.Controls.Add(this.label11);
		this.groupBox2.Controls.Add(this.label2);
		this.groupBox2.Controls.Add(this.txtSupLcOrderDate);
		this.groupBox2.Controls.Add(this.txtdocno);
		this.groupBox2.Controls.Add(this.txtdocdate);
		this.groupBox2.Controls.Add(this.txtQuorationRef);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbOrderStatus);
		this.groupBox2.Controls.Add(this.label7);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbLegalizationRequired);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbShippedTo);
		this.groupBox2.Controls.Add(this.label82);
		this.groupBox2.Controls.Add(this.txtInquiryRef);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbNotifyParty);
		this.groupBox2.Controls.Add(this.label9);
		this.groupBox2.Controls.Add(this.label8);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbSalesPerson);
		this.groupBox2.Controls.Add(this.label83);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbSupplier);
		this.groupBox2.Controls.Add(this.label42);
		this.groupBox2.Controls.Add(this.txtSupLcOrderNo);
		this.groupBox2.Controls.Add(this.txtExpiryDate);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbInspectionRequired);
		this.groupBox2.Controls.Add(this.label12);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCurreny);
		this.groupBox2.Controls.Add(this.label44);
		this.groupBox2.Controls.Add(this.txtExpiryPlace);
		this.groupBox2.Controls.Add(this.label38);
		this.groupBox2.Controls.Add(this.label31);
		this.groupBox2.Controls.Add(this.label13);
		this.groupBox2.Controls.Add(this.label26);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPaymenTerm);
		this.groupBox2.Controls.Add(this.label25);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDestinationPort);
		this.groupBox2.Controls.Add(this.label24);
		this.groupBox2.Controls.Add(this.cmbTransShipment);
		this.groupBox2.Controls.Add(this.txtTotalFcyAmountMain);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbLoadingPort);
		this.groupBox2.Controls.Add(this.txtExchangeRate);
		this.groupBox2.Controls.Add(this.txtShippingMarks);
		this.groupBox2.Controls.Add(this.txtLcyAmountMain);
		this.groupBox2.Controls.Add(this.label16);
		this.groupBox2.Controls.Add(this.label20);
		this.groupBox2.Controls.Add(this.label18);
		this.groupBox2.Controls.Add(this.txtInspectionDesc);
		this.groupBox2.Controls.Add(this.txtNetWeightMain);
		this.groupBox2.Controls.Add(this.label19);
		this.groupBox2.Controls.Add(this.label15);
		this.groupBox2.Controls.Add(this.txtLegalizationDesc);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbExporterBanks);
		this.groupBox2.Controls.Add(this.label14);
		this.groupBox2.Controls.Add(this.label41);
		this.groupBox2.Controls.Add(this.txtLastShipmentDate);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbImporterBank);
		this.groupBox2.Controls.Add(this.cmbPartialShipment);
		this.groupBox2.Controls.Add(this.txtGrossWeightMain);
		this.groupBox2.Controls.Add(this.txtNoOfContainerMain);
		this.groupBox2.Controls.Add(this.txtCommodityDescMain);
		this.groupBox2.Controls.Add(this.label39);
		this.groupBox2.Controls.Add(this.label17);
		this.groupBox2.Controls.Add(this.label43);
		this.groupBox2.Controls.Add(this.label40);
		this.groupBox2.Controls.Add(this.label34);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDeliveryTerm);
		this.groupBox2.Controls.Add(this.label32);
		this.groupBox2.Controls.Add(this.label36);
		this.groupBox2.Controls.Add(this.label37);
		this.groupBox2.Controls.Add(this.label35);
		this.groupBox2.Controls.Add(this.label21);
		this.groupBox2.Controls.Add(this.label22);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(6, 5);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(1163, 333);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Main Information";
		this.txtRemarksHeader.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksHeader.Location = new System.Drawing.Point(823, 45);
		this.txtRemarksHeader.Name = "txtRemarksHeader";
		this.txtRemarksHeader.Size = new System.Drawing.Size(331, 55);
		this.txtRemarksHeader.TabIndex = 31;
		this.txtRemarksHeader.Text = "";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label11.Location = new System.Drawing.Point(5, 80);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(86, 15);
		this.label11.TabIndex = 22;
		this.label11.Text = "Ord.No && Date";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label2.Location = new System.Drawing.Point(5, 22);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(88, 15);
		this.label2.TabIndex = 22;
		this.label2.Text = "Doc No && Date";
		this.txtSupLcOrderDate.CustomFormat = "dd-MMM-yyyy";
		this.txtSupLcOrderDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSupLcOrderDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtSupLcOrderDate.Location = new System.Drawing.Point(207, 76);
		this.txtSupLcOrderDate.Name = "txtSupLcOrderDate";
		this.txtSupLcOrderDate.Size = new System.Drawing.Size(127, 23);
		this.txtSupLcOrderDate.TabIndex = 4;
		this.txtdocno.BackColor = System.Drawing.Color.White;
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocno.Location = new System.Drawing.Point(100, 18);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.Size = new System.Drawing.Size(105, 23);
		this.txtdocno.TabIndex = 0;
		this.txtdocdate.CustomFormat = "dd-MMM-yyyy";
		this.txtdocdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtdocdate.Location = new System.Drawing.Point(207, 18);
		this.txtdocdate.Name = "txtdocdate";
		this.txtdocdate.Size = new System.Drawing.Size(127, 23);
		this.txtdocdate.TabIndex = 1;
		this.txtQuorationRef.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtQuorationRef.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtQuorationRef.Location = new System.Drawing.Point(100, 275);
		this.txtQuorationRef.Name = "txtQuorationRef";
		this.txtQuorationRef.Size = new System.Drawing.Size(234, 23);
		this.txtQuorationRef.TabIndex = 11;
		this.cmbOrderStatus.AllowNull = (DefaultableBoolean)1;
		this.cmbOrderStatus.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbOrderStatus.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbOrderStatus.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance209).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance209).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbOrderStatus).DisplayLayout.Appearance = (AppearanceBase)(object)appearance209;
		((UltraGridBase)this.cmbOrderStatus).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbOrderStatus).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance211).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance211).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance211).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance211).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbOrderStatus).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance211;
		((AppearanceBase)appearance212).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbOrderStatus).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance212;
		((SpecialBoxBase)((UltraGridBase)this.cmbOrderStatus).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance213).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance213).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance213).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance213).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbOrderStatus).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance213;
		((UltraGridBase)this.cmbOrderStatus).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbOrderStatus).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance214).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance214).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbOrderStatus).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance214;
		((AppearanceBase)appearance215).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance215).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbOrderStatus).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance215;
		((UltraGridBase)this.cmbOrderStatus).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbOrderStatus).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance216).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbOrderStatus).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance216;
		((AppearanceBase)appearance217).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance217).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbOrderStatus).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance217;
		((UltraGridBase)this.cmbOrderStatus).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbOrderStatus).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance218).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance218).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance218).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance218).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance218).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbOrderStatus).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance218;
		((AppearanceBase)appearance219).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbOrderStatus).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance219;
		((UltraGridBase)this.cmbOrderStatus).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbOrderStatus).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance220).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance220).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbOrderStatus).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance220;
		((UltraGridBase)this.cmbOrderStatus).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance222).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbOrderStatus).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance222;
		((UltraGridBase)this.cmbOrderStatus).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbOrderStatus).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbOrderStatus).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbOrderStatus).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbOrderStatus.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbOrderStatus).Location = new System.Drawing.Point(675, 302);
		((System.Windows.Forms.Control)(object)this.cmbOrderStatus).Name = "cmbOrderStatus";
		((System.Windows.Forms.Control)(object)this.cmbOrderStatus).Size = new System.Drawing.Size(142, 26);
		((System.Windows.Forms.Control)(object)this.cmbOrderStatus).TabIndex = 30;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label7.Location = new System.Drawing.Point(5, 279);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(81, 15);
		this.label7.TabIndex = 22;
		this.label7.Text = "Quotation Ref";
		this.cmbLegalizationRequired.AllowNull = (DefaultableBoolean)1;
		this.cmbLegalizationRequired.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbLegalizationRequired.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbLegalizationRequired.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance165).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbLegalizationRequired).DisplayLayout.Appearance = (AppearanceBase)(object)appearance165;
		((UltraGridBase)this.cmbLegalizationRequired).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbLegalizationRequired).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance176).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance176).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance176).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance176).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbLegalizationRequired).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance176;
		((AppearanceBase)appearance185).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLegalizationRequired).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance185;
		((SpecialBoxBase)((UltraGridBase)this.cmbLegalizationRequired).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance186).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance186).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance186).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance186).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLegalizationRequired).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance186;
		((UltraGridBase)this.cmbLegalizationRequired).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbLegalizationRequired).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance187).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance187).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbLegalizationRequired).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance187;
		((AppearanceBase)appearance189).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance189).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbLegalizationRequired).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance189;
		((UltraGridBase)this.cmbLegalizationRequired).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbLegalizationRequired).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance190).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLegalizationRequired).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance190;
		((AppearanceBase)appearance191).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance191).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbLegalizationRequired).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance191;
		((UltraGridBase)this.cmbLegalizationRequired).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbLegalizationRequired).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance192).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance192).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance192).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance192).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance192).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLegalizationRequired).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance192;
		((AppearanceBase)appearance193).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbLegalizationRequired).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance193;
		((UltraGridBase)this.cmbLegalizationRequired).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbLegalizationRequired).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance194).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance194).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbLegalizationRequired).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance194;
		((UltraGridBase)this.cmbLegalizationRequired).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance195).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbLegalizationRequired).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance195;
		((UltraGridBase)this.cmbLegalizationRequired).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbLegalizationRequired).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbLegalizationRequired).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbLegalizationRequired).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbLegalizationRequired.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbLegalizationRequired).Location = new System.Drawing.Point(429, 103);
		((System.Windows.Forms.Control)(object)this.cmbLegalizationRequired).Name = "cmbLegalizationRequired";
		((System.Windows.Forms.Control)(object)this.cmbLegalizationRequired).Size = new System.Drawing.Size(388, 26);
		((System.Windows.Forms.Control)(object)this.cmbLegalizationRequired).TabIndex = 18;
		this.cmbShippedTo.AllowNull = (DefaultableBoolean)1;
		this.cmbShippedTo.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbShippedTo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbShippedTo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbShippedTo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbShippedTo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbShippedTo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance30).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance30).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbShippedTo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbShippedTo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance31;
		((SpecialBoxBase)((UltraGridBase)this.cmbShippedTo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbShippedTo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.cmbShippedTo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbShippedTo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbShippedTo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbShippedTo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.cmbShippedTo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbShippedTo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbShippedTo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance37).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbShippedTo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.cmbShippedTo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbShippedTo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance38).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance38).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance38).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbShippedTo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbShippedTo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbShippedTo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbShippedTo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbShippedTo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.cmbShippedTo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbShippedTo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.cmbShippedTo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbShippedTo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbShippedTo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbShippedTo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbShippedTo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbShippedTo).Location = new System.Drawing.Point(100, 161);
		((System.Windows.Forms.Control)(object)this.cmbShippedTo).Name = "cmbShippedTo";
		((System.Windows.Forms.Control)(object)this.cmbShippedTo).Size = new System.Drawing.Size(234, 26);
		((System.Windows.Forms.Control)(object)this.cmbShippedTo).TabIndex = 7;
		this.label82.AutoSize = true;
		this.label82.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label82.Location = new System.Drawing.Point(5, 167);
		this.label82.Name = "label82";
		this.label82.Size = new System.Drawing.Size(67, 15);
		this.label82.TabIndex = 23;
		this.label82.Text = "Shipped To";
		this.txtInquiryRef.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtInquiryRef.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtInquiryRef.Location = new System.Drawing.Point(100, 304);
		this.txtInquiryRef.Name = "txtInquiryRef";
		this.txtInquiryRef.Size = new System.Drawing.Size(234, 23);
		this.txtInquiryRef.TabIndex = 12;
		this.cmbNotifyParty.AllowNull = (DefaultableBoolean)1;
		this.cmbNotifyParty.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbNotifyParty.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbNotifyParty.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance124).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbNotifyParty).DisplayLayout.Appearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.cmbNotifyParty).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbNotifyParty).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance125).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance125).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance125).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbNotifyParty).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance125;
		((AppearanceBase)appearance126).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbNotifyParty).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance126;
		((SpecialBoxBase)((UltraGridBase)this.cmbNotifyParty).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance146).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance146).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance146).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbNotifyParty).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.cmbNotifyParty).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbNotifyParty).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance147).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbNotifyParty).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance147;
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance148).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbNotifyParty).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance148;
		((UltraGridBase)this.cmbNotifyParty).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbNotifyParty).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbNotifyParty).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance149;
		((AppearanceBase)appearance150).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance150).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbNotifyParty).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance150;
		((UltraGridBase)this.cmbNotifyParty).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbNotifyParty).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance151).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance151).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance151).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance151).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbNotifyParty).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance151;
		((AppearanceBase)appearance152).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbNotifyParty).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance152;
		((UltraGridBase)this.cmbNotifyParty).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbNotifyParty).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance153).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbNotifyParty).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance153;
		((UltraGridBase)this.cmbNotifyParty).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbNotifyParty).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.cmbNotifyParty).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbNotifyParty).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbNotifyParty).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbNotifyParty).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbNotifyParty.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbNotifyParty).Location = new System.Drawing.Point(100, 132);
		((System.Windows.Forms.Control)(object)this.cmbNotifyParty).Name = "cmbNotifyParty";
		((System.Windows.Forms.Control)(object)this.cmbNotifyParty).Size = new System.Drawing.Size(234, 26);
		((System.Windows.Forms.Control)(object)this.cmbNotifyParty).TabIndex = 6;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label9.Location = new System.Drawing.Point(5, 138);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(70, 15);
		this.label9.TabIndex = 23;
		this.label9.Text = "Notify Party";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label8.Location = new System.Drawing.Point(5, 308);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(65, 15);
		this.label8.TabIndex = 22;
		this.label8.Text = "Inquiry Ref";
		this.cmbSalesPerson.AllowNull = (DefaultableBoolean)1;
		this.cmbSalesPerson.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbSalesPerson.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbSalesPerson.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbSalesPerson).DisplayLayout.Appearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.cmbSalesPerson).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbSalesPerson).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbSalesPerson).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSalesPerson).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance75;
		((SpecialBoxBase)((UltraGridBase)this.cmbSalesPerson).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance83).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance83).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSalesPerson).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.cmbSalesPerson).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbSalesPerson).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbSalesPerson).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance84;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance85).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbSalesPerson).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.cmbSalesPerson).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbSalesPerson).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSalesPerson).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance97).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance97).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbSalesPerson).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.cmbSalesPerson).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbSalesPerson).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance99).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance99).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance99).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSalesPerson).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance99;
		((AppearanceBase)appearance111).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbSalesPerson).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.cmbSalesPerson).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbSalesPerson).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbSalesPerson).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.cmbSalesPerson).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbSalesPerson).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.cmbSalesPerson).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbSalesPerson).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbSalesPerson).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbSalesPerson).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbSalesPerson.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbSalesPerson).Location = new System.Drawing.Point(100, 103);
		((System.Windows.Forms.Control)(object)this.cmbSalesPerson).Name = "cmbSalesPerson";
		((System.Windows.Forms.Control)(object)this.cmbSalesPerson).Size = new System.Drawing.Size(234, 26);
		((System.Windows.Forms.Control)(object)this.cmbSalesPerson).TabIndex = 5;
		this.label83.AutoSize = true;
		this.label83.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label83.Location = new System.Drawing.Point(5, 109);
		this.label83.Name = "label83";
		this.label83.Size = new System.Drawing.Size(73, 15);
		this.label83.TabIndex = 23;
		this.label83.Text = "Sales Person";
		this.cmbSupplier.AllowNull = (DefaultableBoolean)1;
		this.cmbSupplier.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbSupplier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbSupplier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance156).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance156;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance157).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance157).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance157).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbSupplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance157;
		((AppearanceBase)appearance158).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance158;
		((SpecialBoxBase)((UltraGridBase)this.cmbSupplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance159).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance159).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance159).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance160).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance160;
		((AppearanceBase)appearance161).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance161).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance161;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance162;
		((AppearanceBase)appearance163).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance163).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance163;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance164).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance164).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance164).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance164).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance164;
		((AppearanceBase)appearance166).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance166;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance167).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance167;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbSupplier).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbSupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbSupplier).Location = new System.Drawing.Point(100, 45);
		((System.Windows.Forms.Control)(object)this.cmbSupplier).Name = "cmbSupplier";
		((System.Windows.Forms.Control)(object)this.cmbSupplier).Size = new System.Drawing.Size(234, 26);
		((System.Windows.Forms.Control)(object)this.cmbSupplier).TabIndex = 2;
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label42.Location = new System.Drawing.Point(5, 51);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(51, 15);
		this.label42.TabIndex = 23;
		this.label42.Text = "Supplier";
		this.txtSupLcOrderNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSupLcOrderNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSupLcOrderNo.Location = new System.Drawing.Point(100, 76);
		this.txtSupLcOrderNo.Name = "txtSupLcOrderNo";
		this.txtSupLcOrderNo.Size = new System.Drawing.Size(105, 23);
		this.txtSupLcOrderNo.TabIndex = 3;
		this.txtExpiryDate.CustomFormat = "dd-MMM-yyyy";
		this.txtExpiryDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtExpiryDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtExpiryDate.Location = new System.Drawing.Point(429, 18);
		this.txtExpiryDate.Name = "txtExpiryDate";
		this.txtExpiryDate.Size = new System.Drawing.Size(142, 23);
		this.txtExpiryDate.TabIndex = 13;
		this.cmbInspectionRequired.AllowNull = (DefaultableBoolean)1;
		this.cmbInspectionRequired.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbInspectionRequired.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbInspectionRequired.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbInspectionRequired).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbInspectionRequired).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbInspectionRequired).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbInspectionRequired).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbInspectionRequired).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance57;
		((SpecialBoxBase)((UltraGridBase)this.cmbInspectionRequired).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbInspectionRequired).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.cmbInspectionRequired).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbInspectionRequired).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbInspectionRequired).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance87).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbInspectionRequired).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.cmbInspectionRequired).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbInspectionRequired).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbInspectionRequired).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance98;
		((AppearanceBase)appearance110).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance110).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbInspectionRequired).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.cmbInspectionRequired).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbInspectionRequired).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance121).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance121).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance121).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance121).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbInspectionRequired).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance121;
		((AppearanceBase)appearance132).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbInspectionRequired).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.cmbInspectionRequired).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbInspectionRequired).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbInspectionRequired).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.cmbInspectionRequired).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbInspectionRequired).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance154;
		((UltraGridBase)this.cmbInspectionRequired).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbInspectionRequired).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbInspectionRequired).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbInspectionRequired).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbInspectionRequired.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbInspectionRequired).Location = new System.Drawing.Point(429, 74);
		((System.Windows.Forms.Control)(object)this.cmbInspectionRequired).Name = "cmbInspectionRequired";
		((System.Windows.Forms.Control)(object)this.cmbInspectionRequired).Size = new System.Drawing.Size(388, 26);
		((System.Windows.Forms.Control)(object)this.cmbInspectionRequired).TabIndex = 17;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label12.Location = new System.Drawing.Point(341, 22);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(67, 15);
		this.label12.TabIndex = 29;
		this.label12.Text = "Expiry Date";
		this.cmbCurreny.AllowNull = (DefaultableBoolean)1;
		this.cmbCurreny.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCurreny.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCurreny.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance196).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance196).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCurreny).DisplayLayout.Appearance = (AppearanceBase)(object)appearance196;
		((UltraGridBase)this.cmbCurreny).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCurreny).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance197).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance197).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance197).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance197).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCurreny).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance197;
		((AppearanceBase)appearance198).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCurreny).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance198;
		((SpecialBoxBase)((UltraGridBase)this.cmbCurreny).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance200).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance200).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance200).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance200).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCurreny).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance200;
		((UltraGridBase)this.cmbCurreny).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCurreny).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance201).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance201).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCurreny).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance201;
		((AppearanceBase)appearance202).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance202).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCurreny).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance202;
		((UltraGridBase)this.cmbCurreny).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCurreny).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance203).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCurreny).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance203;
		((AppearanceBase)appearance204).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance204).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCurreny).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance204;
		((UltraGridBase)this.cmbCurreny).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCurreny).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance205).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance205).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance205).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance205).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance205).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCurreny).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance205;
		((AppearanceBase)appearance206).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCurreny).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance206;
		((UltraGridBase)this.cmbCurreny).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCurreny).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance207).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance207).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCurreny).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance207;
		((UltraGridBase)this.cmbCurreny).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance208).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCurreny).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance208;
		((UltraGridBase)this.cmbCurreny).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCurreny).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCurreny).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCurreny).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbCurreny.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCurreny).Location = new System.Drawing.Point(429, 247);
		((System.Windows.Forms.Control)(object)this.cmbCurreny).Name = "cmbCurreny";
		((System.Windows.Forms.Control)(object)this.cmbCurreny).Size = new System.Drawing.Size(142, 26);
		((System.Windows.Forms.Control)(object)this.cmbCurreny).TabIndex = 23;
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label44.Location = new System.Drawing.Point(577, 308);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(73, 15);
		this.label44.TabIndex = 23;
		this.label44.Text = "Order Status";
		this.txtExpiryPlace.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtExpiryPlace.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtExpiryPlace.Location = new System.Drawing.Point(429, 47);
		this.txtExpiryPlace.Name = "txtExpiryPlace";
		this.txtExpiryPlace.Size = new System.Drawing.Size(142, 23);
		this.txtExpiryPlace.TabIndex = 14;
		this.label38.BackColor = System.Drawing.Color.LightGray;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 11f, System.Drawing.FontStyle.Bold);
		this.label38.Location = new System.Drawing.Point(823, 16);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(331, 26);
		this.label38.TabIndex = 22;
		this.label38.Text = "Remarks";
		this.label38.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label31.Location = new System.Drawing.Point(341, 308);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(72, 15);
		this.label31.TabIndex = 22;
		this.label31.Text = "Lcy Amount";
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label13.Location = new System.Drawing.Point(341, 51);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(70, 15);
		this.label13.TabIndex = 23;
		this.label13.Text = "Expiry Place";
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label26.Location = new System.Drawing.Point(341, 279);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(84, 15);
		this.label26.TabIndex = 22;
		this.label26.Text = "Exchange Rate";
		this.cmbPaymenTerm.AllowNull = (DefaultableBoolean)1;
		this.cmbPaymenTerm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPaymenTerm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPaymenTerm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPaymenTerm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbPaymenTerm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPaymenTerm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance4).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance4).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance4).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPaymenTerm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPaymenTerm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance5;
		((SpecialBoxBase)((UltraGridBase)this.cmbPaymenTerm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPaymenTerm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.cmbPaymenTerm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPaymenTerm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPaymenTerm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance7;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPaymenTerm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.cmbPaymenTerm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPaymenTerm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPaymenTerm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance10).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPaymenTerm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbPaymenTerm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPaymenTerm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance11).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance11).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance11).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance11).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPaymenTerm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance12).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPaymenTerm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.cmbPaymenTerm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPaymenTerm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPaymenTerm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.cmbPaymenTerm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPaymenTerm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.cmbPaymenTerm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPaymenTerm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPaymenTerm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPaymenTerm).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbPaymenTerm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPaymenTerm).Location = new System.Drawing.Point(429, 190);
		((System.Windows.Forms.Control)(object)this.cmbPaymenTerm).Name = "cmbPaymenTerm";
		((System.Windows.Forms.Control)(object)this.cmbPaymenTerm).Size = new System.Drawing.Size(142, 26);
		((System.Windows.Forms.Control)(object)this.cmbPaymenTerm).TabIndex = 21;
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label25.Location = new System.Drawing.Point(341, 225);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(72, 15);
		this.label25.TabIndex = 22;
		this.label25.Text = "Fcy Amount";
		this.cmbDestinationPort.AllowNull = (DefaultableBoolean)1;
		this.cmbDestinationPort.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDestinationPort.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDestinationPort.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance109).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance109).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance109).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDestinationPort).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance109;
		((AppearanceBase)appearance188).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance188;
		((SpecialBoxBase)((UltraGridBase)this.cmbDestinationPort).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance199).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance199).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance199).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance199).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance199;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance210).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance210).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance210;
		((AppearanceBase)appearance221).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance221).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance221;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance232).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance232;
		((AppearanceBase)appearance236).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance236).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance236;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance242).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance242).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance242).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance242).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance242).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance242;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDestinationPort).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbDestinationPort.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDestinationPort).Location = new System.Drawing.Point(675, 45);
		((System.Windows.Forms.Control)(object)this.cmbDestinationPort).Name = "cmbDestinationPort";
		((System.Windows.Forms.Control)(object)this.cmbDestinationPort).Size = new System.Drawing.Size(142, 26);
		((System.Windows.Forms.Control)(object)this.cmbDestinationPort).TabIndex = 16;
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label24.Location = new System.Drawing.Point(341, 253);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(55, 15);
		this.label24.TabIndex = 23;
		this.label24.Text = "Lcy Code";
		this.cmbTransShipment.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.cmbTransShipment.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbTransShipment.Location = new System.Drawing.Point(100, 221);
		this.cmbTransShipment.Name = "cmbTransShipment";
		this.cmbTransShipment.Size = new System.Drawing.Size(234, 23);
		this.cmbTransShipment.TabIndex = 9;
		this.txtTotalFcyAmountMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTotalFcyAmountMain.Enabled = false;
		this.txtTotalFcyAmountMain.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtTotalFcyAmountMain.Location = new System.Drawing.Point(429, 221);
		this.txtTotalFcyAmountMain.Name = "txtTotalFcyAmountMain";
		this.txtTotalFcyAmountMain.Size = new System.Drawing.Size(142, 23);
		this.txtTotalFcyAmountMain.TabIndex = 22;
		this.txtTotalFcyAmountMain.TextChanged += new System.EventHandler(txttotalfcamount_TextChanged);
		this.cmbLoadingPort.AllowNull = (DefaultableBoolean)1;
		this.cmbLoadingPort.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbLoadingPort.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbLoadingPort.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance223).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance223).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Appearance = (AppearanceBase)(object)appearance223;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance227).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance227).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance227).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance227).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbLoadingPort).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance227;
		((AppearanceBase)appearance228).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance228;
		((SpecialBoxBase)((UltraGridBase)this.cmbLoadingPort).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance229).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance229).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance229).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance229).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance229;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance230).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance230).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance230;
		((AppearanceBase)appearance231).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance231).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance231;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance233).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance233;
		((AppearanceBase)appearance234).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance234).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance234;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance235).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance235).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance235).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance235).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance235).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance235;
		((AppearanceBase)appearance82).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbLoadingPort).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbLoadingPort.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbLoadingPort).Location = new System.Drawing.Point(675, 16);
		((System.Windows.Forms.Control)(object)this.cmbLoadingPort).Name = "cmbLoadingPort";
		((System.Windows.Forms.Control)(object)this.cmbLoadingPort).Size = new System.Drawing.Size(142, 26);
		((System.Windows.Forms.Control)(object)this.cmbLoadingPort).TabIndex = 15;
		this.txtExchangeRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtExchangeRate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtExchangeRate.Location = new System.Drawing.Point(429, 275);
		this.txtExchangeRate.Name = "txtExchangeRate";
		this.txtExchangeRate.Size = new System.Drawing.Size(142, 23);
		this.txtExchangeRate.TabIndex = 24;
		this.txtExchangeRate.TextChanged += new System.EventHandler(txtexchangerate_TextChanged);
		this.txtExchangeRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtExchangeRate_KeyPress);
		this.txtShippingMarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtShippingMarks.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtShippingMarks.Location = new System.Drawing.Point(823, 304);
		this.txtShippingMarks.Name = "txtShippingMarks";
		this.txtShippingMarks.Size = new System.Drawing.Size(331, 23);
		this.txtShippingMarks.TabIndex = 35;
		this.txtLcyAmountMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtLcyAmountMain.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtLcyAmountMain.Location = new System.Drawing.Point(429, 304);
		this.txtLcyAmountMain.Name = "txtLcyAmountMain";
		this.txtLcyAmountMain.ReadOnly = true;
		this.txtLcyAmountMain.Size = new System.Drawing.Size(142, 23);
		this.txtLcyAmountMain.TabIndex = 25;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label16.Location = new System.Drawing.Point(5, 225);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(90, 15);
		this.label16.TabIndex = 23;
		this.label16.Text = "Trans Shipment";
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label20.Location = new System.Drawing.Point(341, 167);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(80, 15);
		this.label20.TabIndex = 23;
		this.label20.Text = "Exporter Bank";
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label18.Location = new System.Drawing.Point(341, 196);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(84, 15);
		this.label18.TabIndex = 23;
		this.label18.Text = "Payment Term";
		this.txtInspectionDesc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtInspectionDesc.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtInspectionDesc.Location = new System.Drawing.Point(823, 192);
		this.txtInspectionDesc.Name = "txtInspectionDesc";
		this.txtInspectionDesc.Size = new System.Drawing.Size(331, 23);
		this.txtInspectionDesc.TabIndex = 33;
		this.txtNetWeightMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNetWeightMain.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtNetWeightMain.Location = new System.Drawing.Point(675, 249);
		this.txtNetWeightMain.Name = "txtNetWeightMain";
		this.txtNetWeightMain.ReadOnly = true;
		this.txtNetWeightMain.Size = new System.Drawing.Size(142, 23);
		this.txtNetWeightMain.TabIndex = 28;
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label19.Location = new System.Drawing.Point(341, 138);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(83, 15);
		this.label19.TabIndex = 23;
		this.label19.Text = "Importer Bank";
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label15.Location = new System.Drawing.Point(5, 196);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(95, 15);
		this.label15.TabIndex = 23;
		this.label15.Text = "Partial Shipment";
		this.txtLegalizationDesc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtLegalizationDesc.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtLegalizationDesc.Location = new System.Drawing.Point(823, 249);
		this.txtLegalizationDesc.Name = "txtLegalizationDesc";
		this.txtLegalizationDesc.Size = new System.Drawing.Size(331, 23);
		this.txtLegalizationDesc.TabIndex = 34;
		this.cmbExporterBanks.AllowNull = (DefaultableBoolean)1;
		this.cmbExporterBanks.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbExporterBanks.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbExporterBanks.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbExporterBanks).DisplayLayout.Appearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.cmbExporterBanks).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbExporterBanks).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance90).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance90).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance90).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbExporterBanks).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance90;
		((AppearanceBase)appearance91).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbExporterBanks).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance91;
		((SpecialBoxBase)((UltraGridBase)this.cmbExporterBanks).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance92).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance92).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance92).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbExporterBanks).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.cmbExporterBanks).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbExporterBanks).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance93).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbExporterBanks).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbExporterBanks).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance94;
		((UltraGridBase)this.cmbExporterBanks).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbExporterBanks).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance169).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbExporterBanks).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance169;
		((AppearanceBase)appearance170).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance170).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbExporterBanks).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance170;
		((UltraGridBase)this.cmbExporterBanks).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbExporterBanks).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance171).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance171).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance171).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance171).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance171).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbExporterBanks).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance171;
		((AppearanceBase)appearance172).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbExporterBanks).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance172;
		((UltraGridBase)this.cmbExporterBanks).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbExporterBanks).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance173).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance173).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbExporterBanks).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance173;
		((UltraGridBase)this.cmbExporterBanks).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance174).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbExporterBanks).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance174;
		((UltraGridBase)this.cmbExporterBanks).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbExporterBanks).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbExporterBanks).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbExporterBanks).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbExporterBanks.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbExporterBanks).Location = new System.Drawing.Point(429, 161);
		((System.Windows.Forms.Control)(object)this.cmbExporterBanks).Name = "cmbExporterBanks";
		((System.Windows.Forms.Control)(object)this.cmbExporterBanks).Size = new System.Drawing.Size(388, 26);
		((System.Windows.Forms.Control)(object)this.cmbExporterBanks).TabIndex = 20;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label14.Location = new System.Drawing.Point(5, 253);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(83, 15);
		this.label14.TabIndex = 29;
		this.label14.Text = "Last Ship Date";
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label41.Location = new System.Drawing.Point(341, 80);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(63, 15);
		this.label41.TabIndex = 23;
		this.label41.Text = "Inspection";
		this.txtLastShipmentDate.CustomFormat = "dd-MMM-yyyy";
		this.txtLastShipmentDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtLastShipmentDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtLastShipmentDate.Location = new System.Drawing.Point(100, 249);
		this.txtLastShipmentDate.Name = "txtLastShipmentDate";
		this.txtLastShipmentDate.Size = new System.Drawing.Size(234, 23);
		this.txtLastShipmentDate.TabIndex = 10;
		this.CmbImporterBank.AllowNull = (DefaultableBoolean)1;
		this.CmbImporterBank.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbImporterBank.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbImporterBank.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance175).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.Appearance = (AppearanceBase)(object)appearance175;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance177).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance177).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance177).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbImporterBank).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance177;
		((AppearanceBase)appearance178).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance178;
		((SpecialBoxBase)((UltraGridBase)this.CmbImporterBank).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance179).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance179).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance179).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance179;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance180).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance180).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance180;
		((AppearanceBase)appearance181).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance181).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance181;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance182).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance182;
		((AppearanceBase)appearance183).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance183).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance183;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance184).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance184).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance184).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance184).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance184).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance184;
		((AppearanceBase)appearance250).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance250;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance251).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance251).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance251;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance252).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance252;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbImporterBank).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbImporterBank).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbImporterBank.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbImporterBank).Location = new System.Drawing.Point(429, 132);
		((System.Windows.Forms.Control)(object)this.CmbImporterBank).Name = "CmbImporterBank";
		((System.Windows.Forms.Control)(object)this.CmbImporterBank).Size = new System.Drawing.Size(388, 26);
		((System.Windows.Forms.Control)(object)this.CmbImporterBank).TabIndex = 19;
		this.cmbPartialShipment.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.cmbPartialShipment.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbPartialShipment.Location = new System.Drawing.Point(100, 192);
		this.cmbPartialShipment.Name = "cmbPartialShipment";
		this.cmbPartialShipment.Size = new System.Drawing.Size(234, 23);
		this.cmbPartialShipment.TabIndex = 8;
		this.txtGrossWeightMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtGrossWeightMain.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtGrossWeightMain.Location = new System.Drawing.Point(675, 221);
		this.txtGrossWeightMain.Name = "txtGrossWeightMain";
		this.txtGrossWeightMain.Size = new System.Drawing.Size(142, 23);
		this.txtGrossWeightMain.TabIndex = 27;
		this.txtGrossWeightMain.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtGrossWeightMain_KeyPress);
		this.txtNoOfContainerMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNoOfContainerMain.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtNoOfContainerMain.Location = new System.Drawing.Point(675, 275);
		this.txtNoOfContainerMain.Name = "txtNoOfContainerMain";
		this.txtNoOfContainerMain.Size = new System.Drawing.Size(142, 23);
		this.txtNoOfContainerMain.TabIndex = 29;
		this.txtNoOfContainerMain.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtNoOfContainerMain_KeyPress);
		this.txtCommodityDescMain.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCommodityDescMain.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtCommodityDescMain.Location = new System.Drawing.Point(823, 134);
		this.txtCommodityDescMain.Name = "txtCommodityDescMain";
		this.txtCommodityDescMain.Size = new System.Drawing.Size(331, 23);
		this.txtCommodityDescMain.TabIndex = 32;
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label39.Location = new System.Drawing.Point(341, 109);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(71, 15);
		this.label39.TabIndex = 23;
		this.label39.Text = "Legalization";
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(577, 196);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(80, 15);
		this.label17.TabIndex = 23;
		this.label17.Text = "Delivery Term";
		this.label43.BackColor = System.Drawing.Color.LightGray;
		this.label43.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.label43.Location = new System.Drawing.Point(823, 161);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(331, 26);
		this.label43.TabIndex = 22;
		this.label43.Text = "Inspaction Desc";
		this.label43.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label40.BackColor = System.Drawing.Color.LightGray;
		this.label40.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.label40.Location = new System.Drawing.Point(823, 221);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(331, 23);
		this.label40.TabIndex = 22;
		this.label40.Text = "Legalization Desc";
		this.label40.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label34.BackColor = System.Drawing.Color.LightGray;
		this.label34.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.label34.Location = new System.Drawing.Point(823, 275);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(331, 23);
		this.label34.TabIndex = 22;
		this.label34.Text = "Shipping Marks";
		this.label34.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.cmbDeliveryTerm.AllowNull = (DefaultableBoolean)1;
		this.cmbDeliveryTerm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDeliveryTerm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDeliveryTerm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDeliveryTerm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbDeliveryTerm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDeliveryTerm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance17).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance17).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDeliveryTerm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDeliveryTerm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance18;
		((SpecialBoxBase)((UltraGridBase)this.cmbDeliveryTerm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDeliveryTerm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.cmbDeliveryTerm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDeliveryTerm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDeliveryTerm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDeliveryTerm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.cmbDeliveryTerm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDeliveryTerm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDeliveryTerm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance23).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDeliveryTerm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbDeliveryTerm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDeliveryTerm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance25).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance25).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance25).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDeliveryTerm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDeliveryTerm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbDeliveryTerm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDeliveryTerm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDeliveryTerm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.cmbDeliveryTerm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDeliveryTerm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.cmbDeliveryTerm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDeliveryTerm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDeliveryTerm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDeliveryTerm).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbDeliveryTerm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDeliveryTerm).Location = new System.Drawing.Point(675, 190);
		((System.Windows.Forms.Control)(object)this.cmbDeliveryTerm).Name = "cmbDeliveryTerm";
		((System.Windows.Forms.Control)(object)this.cmbDeliveryTerm).Size = new System.Drawing.Size(142, 26);
		((System.Windows.Forms.Control)(object)this.cmbDeliveryTerm).TabIndex = 26;
		this.label32.BackColor = System.Drawing.Color.LightGray;
		this.label32.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.label32.Location = new System.Drawing.Point(823, 103);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(331, 26);
		this.label32.TabIndex = 22;
		this.label32.Text = "Commodity Desc";
		this.label32.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(577, 253);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(89, 15);
		this.label36.TabIndex = 22;
		this.label36.Text = "Net Weight(kg)";
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(577, 279);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(98, 15);
		this.label37.TabIndex = 22;
		this.label37.Text = "No Of Containers";
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(577, 225);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(99, 15);
		this.label35.TabIndex = 22;
		this.label35.Text = "Gross Weight(kg)";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(577, 51);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(93, 15);
		this.label21.TabIndex = 23;
		this.label21.Text = "Destination Port";
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(577, 22);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(75, 15);
		this.label22.TabIndex = 23;
		this.label22.Text = "Loading Port";
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.ForeColor = System.Drawing.Color.Red;
		this.label30.Location = new System.Drawing.Point(1560, 128);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(13, 15);
		this.label30.TabIndex = 57;
		this.label30.Text = "*";
		this.label30.Visible = false;
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.ForeColor = System.Drawing.Color.Red;
		this.label29.Location = new System.Drawing.Point(1560, 153);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(13, 15);
		this.label29.TabIndex = 56;
		this.label29.Text = "*";
		this.label29.Visible = false;
		this.label72.AutoSize = true;
		this.label72.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label72.ForeColor = System.Drawing.Color.Red;
		this.label72.Location = new System.Drawing.Point(1560, 265);
		this.label72.Name = "label72";
		this.label72.Size = new System.Drawing.Size(13, 15);
		this.label72.TabIndex = 55;
		this.label72.Text = "*";
		this.label72.Visible = false;
		this.label70.AutoSize = true;
		this.label70.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label70.ForeColor = System.Drawing.Color.Red;
		this.label70.Location = new System.Drawing.Point(1560, 231);
		this.label70.Name = "label70";
		this.label70.Size = new System.Drawing.Size(13, 15);
		this.label70.TabIndex = 53;
		this.label70.Text = "*";
		this.label70.Visible = false;
		this.label60.AutoSize = true;
		this.label60.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label60.ForeColor = System.Drawing.Color.Red;
		this.label60.Location = new System.Drawing.Point(1560, 185);
		this.label60.Name = "label60";
		this.label60.Size = new System.Drawing.Size(13, 15);
		this.label60.TabIndex = 43;
		this.label60.Text = "*";
		this.label60.Visible = false;
		this.label69.AutoSize = true;
		this.label69.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label69.ForeColor = System.Drawing.Color.Red;
		this.label69.Location = new System.Drawing.Point(1560, 234);
		this.label69.Name = "label69";
		this.label69.Size = new System.Drawing.Size(13, 15);
		this.label69.TabIndex = 52;
		this.label69.Text = "*";
		this.label69.Visible = false;
		this.label68.AutoSize = true;
		this.label68.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label68.ForeColor = System.Drawing.Color.Red;
		this.label68.Location = new System.Drawing.Point(1560, 110);
		this.label68.Name = "label68";
		this.label68.Size = new System.Drawing.Size(13, 15);
		this.label68.TabIndex = 51;
		this.label68.Text = "*";
		this.label68.Visible = false;
		this.label67.AutoSize = true;
		this.label67.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label67.ForeColor = System.Drawing.Color.Red;
		this.label67.Location = new System.Drawing.Point(1560, 209);
		this.label67.Name = "label67";
		this.label67.Size = new System.Drawing.Size(13, 15);
		this.label67.TabIndex = 50;
		this.label67.Text = "*";
		this.label67.Visible = false;
		this.label66.AutoSize = true;
		this.label66.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label66.ForeColor = System.Drawing.Color.Red;
		this.label66.Location = new System.Drawing.Point(1560, 176);
		this.label66.Name = "label66";
		this.label66.Size = new System.Drawing.Size(13, 15);
		this.label66.TabIndex = 49;
		this.label66.Text = "*";
		this.label66.Visible = false;
		this.label65.AutoSize = true;
		this.label65.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label65.ForeColor = System.Drawing.Color.Red;
		this.label65.Location = new System.Drawing.Point(1560, 244);
		this.label65.Name = "label65";
		this.label65.Size = new System.Drawing.Size(13, 15);
		this.label65.TabIndex = 48;
		this.label65.Text = "*";
		this.label65.Visible = false;
		this.label64.AutoSize = true;
		this.label64.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label64.ForeColor = System.Drawing.Color.Red;
		this.label64.Location = new System.Drawing.Point(1560, 153);
		this.label64.Name = "label64";
		this.label64.Size = new System.Drawing.Size(13, 15);
		this.label64.TabIndex = 47;
		this.label64.Text = "*";
		this.label64.Visible = false;
		this.label63.AutoSize = true;
		this.label63.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label63.ForeColor = System.Drawing.Color.Red;
		this.label63.Location = new System.Drawing.Point(1560, 185);
		this.label63.Name = "label63";
		this.label63.Size = new System.Drawing.Size(13, 15);
		this.label63.TabIndex = 46;
		this.label63.Text = "*";
		this.label63.Visible = false;
		this.label62.AutoSize = true;
		this.label62.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label62.ForeColor = System.Drawing.Color.Red;
		this.label62.Location = new System.Drawing.Point(1560, 153);
		this.label62.Name = "label62";
		this.label62.Size = new System.Drawing.Size(13, 15);
		this.label62.TabIndex = 45;
		this.label62.Text = "*";
		this.label62.Visible = false;
		this.label61.AutoSize = true;
		this.label61.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label61.ForeColor = System.Drawing.Color.Red;
		this.label61.Location = new System.Drawing.Point(1560, 215);
		this.label61.Name = "label61";
		this.label61.Size = new System.Drawing.Size(13, 15);
		this.label61.TabIndex = 44;
		this.label61.Text = "*";
		this.label61.Visible = false;
		this.label58.AutoSize = true;
		this.label58.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label58.ForeColor = System.Drawing.Color.Red;
		this.label58.Location = new System.Drawing.Point(1560, 153);
		this.label58.Name = "label58";
		this.label58.Size = new System.Drawing.Size(13, 15);
		this.label58.TabIndex = 41;
		this.label58.Text = "*";
		this.label58.Visible = false;
		this.label57.AutoSize = true;
		this.label57.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label57.ForeColor = System.Drawing.Color.Red;
		this.label57.Location = new System.Drawing.Point(1540, 46);
		this.label57.Name = "label57";
		this.label57.Size = new System.Drawing.Size(13, 15);
		this.label57.TabIndex = 40;
		this.label57.Text = "*";
		this.label57.Visible = false;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.Red;
		this.label1.Location = new System.Drawing.Point(1540, 11);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(13, 15);
		this.label1.TabIndex = 39;
		this.label1.Text = "*";
		this.label1.Visible = false;
		this.cmbproject.AllowNull = (DefaultableBoolean)1;
		this.cmbproject.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbproject.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbproject.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbproject).DisplayLayout.Appearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbproject).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbproject).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance43).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance43).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance43).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbproject).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbproject).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance44;
		((SpecialBoxBase)((UltraGridBase)this.cmbproject).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance45).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance45).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbproject).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.cmbproject).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbproject).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance47;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance50).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance51).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance51).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance51).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance51).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance51;
		((AppearanceBase)appearance52).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.cmbproject).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbproject).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbproject).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbproject).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbproject.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbproject).Location = new System.Drawing.Point(1554, 40);
		((System.Windows.Forms.Control)(object)this.cmbproject).Name = "cmbproject";
		((System.Windows.Forms.Control)(object)this.cmbproject).Size = new System.Drawing.Size(67, 26);
		((System.Windows.Forms.Control)(object)this.cmbproject).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.cmbproject).Visible = false;
		this.cmbbranch.AllowNull = (DefaultableBoolean)1;
		this.cmbbranch.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbbranch.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbbranch.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Appearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.cmbbranch).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbbranch).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance60).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance60).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbbranch).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance60;
		((AppearanceBase)appearance61).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbbranch).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance61;
		((SpecialBoxBase)((UltraGridBase)this.cmbbranch).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance62).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance62).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance62).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbbranch).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.cmbbranch).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbbranch).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance63;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance64).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance65;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance66).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance67).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance67).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance67).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance67).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance67;
		((AppearanceBase)appearance69).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbbranch).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.cmbbranch).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbbranch).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbbranch).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbbranch).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbbranch.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbbranch).Location = new System.Drawing.Point(1554, 5);
		((System.Windows.Forms.Control)(object)this.cmbbranch).Name = "cmbbranch";
		((System.Windows.Forms.Control)(object)this.cmbbranch).Size = new System.Drawing.Size(67, 26);
		((System.Windows.Forms.Control)(object)this.cmbbranch).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbbranch).Visible = false;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(1494, 46);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(46, 15);
		this.label5.TabIndex = 23;
		this.label5.Text = "Project";
		this.label5.Visible = false;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(1494, 11);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(46, 15);
		this.label4.TabIndex = 23;
		this.label4.Text = "Branch";
		this.label4.Visible = false;
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.btnclose);
		this.panel4.Controls.Add(this.label23);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 30);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1172, 25);
		this.panel4.TabIndex = 0;
		this.btnclose.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnclose.BackColor = System.Drawing.Color.Teal;
		this.btnclose.FlatAppearance.BorderSize = 0;
		this.btnclose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnclose.Image = (System.Drawing.Image)resources.GetObject("btnclose.Image");
		this.btnclose.Location = new System.Drawing.Point(1124, 2);
		this.btnclose.Margin = new System.Windows.Forms.Padding(2);
		this.btnclose.Name = "btnclose";
		this.btnclose.Size = new System.Drawing.Size(44, 22);
		this.btnclose.TabIndex = 0;
		this.btnclose.UseVisualStyleBackColor = false;
		this.btnclose.Click += new System.EventHandler(btnclose_Click);
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(5, 3);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(149, 19);
		this.label23.TabIndex = 0;
		this.label23.Text = "Import : Lc Order";
		this.panel3.Controls.Add(this.toolStrip2);
		this.panel3.Controls.Add(this.ctrlGrdBar3);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1172, 30);
		this.panel3.TabIndex = 0;
		this.toolStrip2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[16]
		{
			this.btnNew, this.btnRefresh, this.toolStripSeparator2, this.BtnSaveAs, this.btnSave, this.btnDelete, this.btnUpdate, this.toolStripSeparator3, this.Print, this.toolStripSeparator4,
			this.btnAttachment, this.toolStripSeparator5, this.BtnShortCutkeys, this.btnSeaPortDefine, this.btnDefinePaymentTerm, this.toolStripButton2
		});
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1172, 27);
		this.toolStrip2.TabIndex = 2;
		this.toolStrip2.Text = "toolStrip2";
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(59, 24);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "&Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
		this.BtnSaveAs.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnSaveAs.Image = (System.Drawing.Image)resources.GetObject("BtnSaveAs.Image");
		this.BtnSaveAs.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnSaveAs.Name = "BtnSaveAs";
		this.BtnSaveAs.Size = new System.Drawing.Size(85, 24);
		this.BtnSaveAs.Text = "&Save As";
		this.BtnSaveAs.Visible = false;
		this.BtnSaveAs.Click += new System.EventHandler(BtnSaveAs_Click);
		this.btnSave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSave.Image = (System.Drawing.Image)resources.GetObject("btnSave.Image");
		this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(64, 24);
		this.btnSave.Text = "&Save";
		this.btnSave.Click += new System.EventHandler(btnSave_Click);
		this.btnDelete.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(73, 24);
		this.btnDelete.Text = "&Delete";
		this.btnDelete.Visible = false;
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.btnUpdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.toolStripSeparator3.Name = "toolStripSeparator3";
		this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
		this.Print.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.Print.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.Print.Name = "Print";
		this.Print.Size = new System.Drawing.Size(92, 24);
		this.Print.Text = "231-&Print";
		this.Print.Click += new System.EventHandler(Print_Click);
		this.toolStripSeparator4.Name = "toolStripSeparator4";
		this.toolStripSeparator4.Size = new System.Drawing.Size(6, 27);
		this.btnAttachment.Font = new System.Drawing.Font("Verdana", 10f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(114, 24);
		this.btnAttachment.Text = "&Attachment";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.toolStripSeparator5.Name = "toolStripSeparator5";
		this.toolStripSeparator5.Size = new System.Drawing.Size(6, 27);
		this.BtnShortCutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShortCutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnShortCutkeys.Name = "BtnShortCutkeys";
		this.BtnShortCutkeys.Size = new System.Drawing.Size(106, 24);
		this.BtnShortCutkeys.Text = "ShortCut Keys";
		this.BtnShortCutkeys.Click += new System.EventHandler(btnshortcutkeys_Click);
		this.btnSeaPortDefine.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSeaPortDefine.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSeaPortDefine.Name = "btnSeaPortDefine";
		this.btnSeaPortDefine.Size = new System.Drawing.Size(68, 24);
		this.btnSeaPortDefine.Text = "Sea Port";
		this.btnSeaPortDefine.Click += new System.EventHandler(btnSeaPortDefine_Click);
		this.btnDefinePaymentTerm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnDefinePaymentTerm.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDefinePaymentTerm.Name = "btnDefinePaymentTerm";
		this.btnDefinePaymentTerm.Size = new System.Drawing.Size(105, 24);
		this.btnDefinePaymentTerm.Text = "Payment Term";
		this.btnDefinePaymentTerm.Click += new System.EventHandler(btnPaymentTerms_Click);
		this.toolStripButton2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton2.Name = "toolStripButton2";
		this.toolStripButton2.Size = new System.Drawing.Size(100, 24);
		this.toolStripButton2.Text = "Delivery Term";
		this.toolStripButton2.Click += new System.EventHandler(btnDeliveryTerm_Click);
		this.ctrlGrdBar3.FormName = null;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(1379, -6);
		this.ctrlGrdBar3.MyGrid = null;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(43, 27);
		this.ctrlGrdBar3.TabIndex = 1;
		this.panel11.BackColor = System.Drawing.Color.Teal;
		this.panel11.Controls.Add(this.label45);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(3, 66);
		this.panel11.Margin = new System.Windows.Forms.Padding(2);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1158, 26);
		this.panel11.TabIndex = 508;
		this.label45.AutoSize = true;
		this.label45.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label45.ForeColor = System.Drawing.Color.White;
		this.label45.Location = new System.Drawing.Point(3, 4);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(135, 19);
		this.label45.TabIndex = 561;
		this.label45.Text = "Payment Detail";
		this.grdPaymentTerm.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdPaymentTerm).BackColor = System.Drawing.Color.White;
		this.grdPaymentTerm.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdPaymentTerm_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPaymentTerm.DesignTimeLayout = grdPaymentTerm_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPaymentTerm).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdPaymentTerm.FilterMode = (FilterMode)2;
		((System.Windows.Forms.Control)(object)this.grdPaymentTerm).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPaymentTerm.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdPaymentTerm).Location = new System.Drawing.Point(3, 92);
		((System.Windows.Forms.Control)(object)this.grdPaymentTerm).Name = "grdPaymentTerm";
		((ScrollableBase)this.grdPaymentTerm).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPaymentTerm).Size = new System.Drawing.Size(1158, 157);
		((System.Windows.Forms.Control)(object)this.grdPaymentTerm).TabIndex = 509;
		this.grdPaymentTerm.TotalRow = (InheritableBoolean)1;
		this.grdPaymentTerm.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPaymentTerm).VisualStyle = (VisualStyle)4;
		this.grdPaymentTerm.ColumnButtonClick += new ColumnActionEventHandler(grdPaymentTerm_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdPaymentTerm).DoubleClick += new System.EventHandler(grdPaymentTerm_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdPaymentTerm).KeyDown += new System.Windows.Forms.KeyEventHandler(grdPaymentTerm_KeyDown);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1184, 711);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "ImLcOrder";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(ImLcOrder_FormClosed);
		base.Load += new System.EventHandler(LcOrder_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(LcOrder_KeyDown);
		this.History.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistoryDetail).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.Form.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.tabControl2.ResumeLayout(false);
		this.Detail.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.panel10.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbRateUom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemUom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPackingType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).EndInit();
		this.Payment.ResumeLayout(false);
		this.panel14.ResumeLayout(false);
		this.panel14.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPaymentTermDetail).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbOrderStatus).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbLegalizationRequired).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbShippedTo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbNotifyParty).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSalesPerson).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSupplier).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbInspectionRequired).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCurreny).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPaymenTerm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDestinationPort).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbLoadingPort).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbExporterBanks).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbImporterBank).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDeliveryTerm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbproject).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbbranch).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPaymentTerm).EndInit();
		base.ResumeLayout(false);
	}
}

}
