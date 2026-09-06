using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Export;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Export;
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
namespace Architecture.WinApp.Export
{

public class LcOrder : Form
{
	private Attachment AT = new Attachment();

	public UserAccount UserAccount = new UserAccount();

	private int RecId;

	private bool UpdateMode;

	private int updateDetailIndex;

	private DataTable dtdetail = new DataTable();

	private DropDownBind DDL = new DropDownBind();

	private IContainer components;

	internal GridEX DataGridHistory;

	private Panel panel8;

	private TabPage History;

	private Panel panel1;

	private TabControl tabControl1;

	private BackgroundWorker backgroundWorker1;

	private ComboBox cmbexporterbank;

	private Panel panel11;

	private Panel panel9;

	private Label label56;

	private TabPage Form;

	private Panel panel2;

	private Panel panel6;

	internal GridEX grdDetail;

	private Panel panel5;

	internal UltraCombo cmbstatus;

	internal UltraCombo cmbinspection;

	internal UltraCombo cmblegalization;

	internal UltraCombo cmbfc;

	internal UltraCombo cmbdestinationport;

	internal UltraCombo cmbloadingport;

	internal UltraCombo cmbExporterBanks;

	internal UltraCombo cmbimporterbank;

	internal UltraCombo cmbpaymenterm;

	internal UltraCombo cmbdeliveryterm;

	internal UltraCombo cmbcustomer;

	internal UltraCombo cmbproject;

	internal UltraCombo cmbbranch;

	private GroupBox groupBox1;

	internal UltraCombo cmbrateuom;

	internal UltraCombo cmbouterpackuom;

	internal UltraCombo cmbinnerpackuom;

	internal UltraCombo cmbinnerpacktype;

	internal UltraCombo cmbitem;

	private TextBox txtOuterQty;

	private TextBox txtinnerQty;

	private Button btnCancel;

	private Button btnGridUpdate;

	private Button btnaddnew;

	private Label label3;

	private Label label45;

	private Label label47;

	private Label label48;

	private Label label49;

	private Label label50;

	private Label label51;

	private TextBox txtamount;

	private Label label52;

	private TextBox txtrate;

	private Label label53;

	private TextBox txtweight;

	private Label label54;

	private TextBox txtdescription;

	private Label label55;

	private DateTimePicker txtexpirydate;

	private Label label12;

	private DateTimePicker txtlastshipmentdate;

	private Label label14;

	private DateTimePicker txtreferencedate;

	private Label label10;

	private DateTimePicker txtdocdate;

	private Label label46;

	private Label label5;

	private TextBox txtremarks;

	private Label label38;

	private TextBox txtexpplace;

	private TextBox txtcustomerrefno;

	private TextBox txtinquiryref;

	private TextBox txtlocalcurramount;

	private TextBox txtnetweight;

	private TextBox txtgrossweight;

	private TextBox txtexchangerate;

	private TextBox txttotalfcamount;

	private TextBox txtcustomercode;

	private TextBox txtquorationref;

	private Label label9;

	private TextBox txtshippingmark;

	private TextBox txtinspectiondesc;

	private TextBox txtlegalizationdesc;

	private TextBox txtnoofcontainer;

	private Label label41;

	private Label label44;

	private TextBox txtcommoditydesc;

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

	private Label label11;

	private Label label4;

	private Label label7;

	private Label label43;

	private Label label6;

	private Label label40;

	private Label label34;

	private Label label37;

	private Label label32;

	private Label label13;

	private Label label2;

	private Label label42;

	private Panel panel4;

	private Label label23;

	private CtrlGrdBar ctrlGrdBar1;

	private Panel panel3;

	private ToolStrip toolStrip2;

	private ToolStripButton btnNew;

	private ToolStripButton btnSave;

	private ToolStripButton btnUpdate;

	private ToolStripSeparator toolStripSeparator;

	private ToolStripButton Print;

	private CtrlGrdBar ctrlGrdBar3;

	private Label label72;

	private Label label71;

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

	private Label label76;

	private Label label75;

	private Label label74;

	private Label label73;

	internal UltraCombo cmbTransShipment;

	internal UltraCombo cmbPartialShipment;

	internal UltraCombo cmbPerformaInvoice;

	private ToolStripButton btnAttachment;

	private CtrlGrdBar ctrlGrdBar2;

	public LcOrder(UserAccount pUserAccount)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUserAccount;
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

	private void cmbsupplierfill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtSupplier = SupplierCustomer.ReadByOrganizationCompanyIdForExport(new SupplierCustomer
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtSupplier.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtSupplier, cmbcustomer, "Id", "CompanyName", "Customer", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbforeigncurrency()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtForeignCurrency = MultiCurrency.GetAll(new MultiCurrency
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtForeignCurrency.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtForeignCurrency, cmbfc, "Id", "CurrencyCode", "Currency", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadingPort()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtLoadingPort = SeaPorts.Getall(new SeaPorts
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtLoadingPort.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtLoadingPort, cmbloadingport, "Id", "PortName", "Loading Port", false);
				DDL.BindDDLNew(dtLoadingPort, cmbdestinationport, "Id", "PortName", "Destination Port", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemDetailFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtItem = Item.GetExportItemsByOrganizationCompanyId(new Item
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtItem.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtItem, cmbitem, "Id", "ItemName", "Item", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemPacktype()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtItemPack = ExImPackMaterilaType.Getall(new ExImPackMaterilaType
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtItemPack.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtItemPack, cmbinnerpacktype, "Id", "Description", "Pack Type", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbitem_Leave(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		try
		{
			DataTable dtUOM = UOMSchedule.SearchByObject(new UOMSchedule
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				ItemId = Conversion.ToInt(cmbitem.Value)
			});
			if (dtUOM.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtUOM, cmbouterpackuom, "Id", "Equivalent", "Outer Pack UOM", false);
				DDL.BindDDLNew(dtUOM, cmbinnerpackuom, "Id", "Equivalent", "Inner Pack UOM", false);
				DDL.BindDDLNew(dtUOM, cmbrateuom, "Id", "Equivalent", "Rate UOM", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void importerbank()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			List<Bank> lst = Bank.GetAll(new Bank
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (lst.Count <= 0)
			{
				return;
			}
			DataTable dtForeign = new DataTable();
			dtForeign.Columns.Add("Id");
			dtForeign.Columns.Add("BranchName");
			DataTable dtHome = new DataTable();
			dtHome.Columns.Add("Id");
			dtHome.Columns.Add("BranchName");
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
			if (dtForeign.Rows.Count > 0)
			{
				DDL.BindDDL(dtForeign, cmbimporterbank, "Id", "BranchName", "ImporterBank", ZeroIndex: false);
			}
			if (dtHome.Rows.Count > 0)
			{
				DDL.BindDDL(dtHome, cmbExporterBanks, "Id", "BranchName", "ExporterBank", ZeroIndex: false);
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
			DDL.BindDDL(dt, cmblegalization, "Id", "Name", "Legalization", ZeroIndex: false);
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
			DDL.BindDDL(dt, cmbinspection, "Id", "Name", "Inspection", ZeroIndex: false);
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
			DDL.BindDDL(dt, cmbstatus, "Id", "Name", "Status", ZeroIndex: false);
			((UltraGridBase)cmbstatus).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ShipmentFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Name");
			dt.Rows.Add(1, "Allowed");
			dt.Rows.Add(2, "Not-Allowed");
			DDL.BindDDL(dt, cmbPartialShipment, "Id", "Name", "Partial Shipment", ZeroIndex: false);
			DDL.BindDDL(dt, cmbTransShipment, "Id", "Name", "Trans Shipment", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeliveryTerm()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			new ExImDeliveryTerm();
			DataTable dtDeliveryTerm = ExImDeliveryTerm.Getall();
			if (dtDeliveryTerm.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtDeliveryTerm, cmbdeliveryterm, "Id", "Code", "Delivery Term", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PaymentTerm()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtPaymentTerm = ExImLcPaymentTerm.Getall(new ExImLcPaymentTerm
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtPaymentTerm.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtPaymentTerm, cmbpaymenterm, "Id", "LcOrderTerm", "Payment Term", false);
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
		//IL_0037: Expected O, but got Unknown
		try
		{
			int code = ExImLcOrder.GenerateCode(new ExImLcOrder
			{
				CompanyId = UserAccount.CompanyId,
				OrganizationId = UserAccount.OrganizationId,
				DocumentTypeId = 201
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

	private void PerformaInvoie()
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Name");
			DataTable dtPerformaInvoice = ExImProformaInvoice.GetData(new ExImProformaInvoice
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtPerformaInvoice.Rows.Count <= 0)
			{
				return;
			}
			dt.Rows.Clear();
			((Control)(object)cmbPerformaInvoice).Text = string.Empty;
			for (int i = 0; i < dtPerformaInvoice.Rows.Count; i++)
			{
				if (Conversion.ToInt(dtPerformaInvoice.Rows[i]["SupplierCustomerId"]) == Conversion.ToInt(cmbcustomer.Value))
				{
					dt.Rows.Add(dtPerformaInvoice.Rows[i]["Id"], dtPerformaInvoice.Rows[i]["DocNo"]);
				}
			}
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbPerformaInvoice, "Id", "Name", "Performa Invoice", ZeroIndex: false);
				((UltraGridBase)cmbPerformaInvoice).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LcOrder_Load(object sender, EventArgs e)
	{
		BranchFill();
		Project();
		cmbsupplierfill();
		cmbforeigncurrency();
		LoadingPort();
		ItemDetailFill();
		ItemPacktype();
		importerbank();
		LegalizationFill();
		InspectionFill();
		StatusFill();
		DeliveryTerm();
		PaymentTerm();
		ShipmentFill();
		DocNo();
		dtdetail.Columns.Add("ItemId");
		dtdetail.Columns.Add("Item");
		dtdetail.Columns.Add("PackTypeId");
		dtdetail.Columns.Add("PackType");
		dtdetail.Columns.Add("InnerQty", typeof(double));
		dtdetail.Columns.Add("InnerPackUOMId");
		dtdetail.Columns.Add("InnerPackUOM");
		dtdetail.Columns.Add("OuterQty", typeof(double));
		dtdetail.Columns.Add("OuterPackUOMId");
		dtdetail.Columns.Add("OuterPackUOM");
		dtdetail.Columns.Add("Weigth", typeof(double));
		dtdetail.Columns.Add("Rate", typeof(double));
		dtdetail.Columns.Add("RateUOMId");
		dtdetail.Columns.Add("RateUOM");
		dtdetail.Columns.Add("Amount", typeof(double));
		dtdetail.Columns.Add("Description");
		txtdocdate.Focus();
	}

	private void btnSave_Click(object sender, EventArgs e)
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected O, but got Unknown
		//IL_0611: Unknown result type (might be due to invalid IL or missing references)
		//IL_0618: Expected O, but got Unknown
		//IL_0458: Unknown result type (might be due to invalid IL or missing references)
		//IL_045f: Expected O, but got Unknown
		try
		{
			if (RecId == 0)
			{
				if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
			}
			else if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			if (!FormValidation())
			{
				return;
			}
			ExImLcOrder exImLc = new ExImLcOrder();
			if (RecId > 0 && UpdateMode)
			{
				exImLc.Id = RecId;
			}
			exImLc.DocumentTypeId = 201;
			exImLc.OrganizationId = UserAccount.OrganizationId;
			exImLc.CompanyId = UserAccount.CompanyId;
			exImLc.LcOrderDocNo = Conversion.ToInt((object)txtdocno.Text);
			exImLc.BranchesId = Conversion.ToInt(cmbbranch.Value);
			exImLc.ExImProformaInvoice = Conversion.ToInt(cmbPerformaInvoice.Value);
			exImLc.LcOrderNo = txtdocno.Text;
			exImLc.LcOrderDate = txtdocdate.Value;
			exImLc.ExpiryDate = txtexpirydate.Value;
			exImLc.ExpiryPlace = txtexpplace.Text;
			exImLc.PartialShipment = Conversion.ToString((object)((Control)(object)cmbPartialShipment).Text);
			exImLc.TransShipment = ((Control)(object)cmbTransShipment).Text;
			exImLc.LastShipmentDate = txtlastshipmentdate.Value;
			exImLc.SupCustId = Conversion.ToInt(cmbcustomer.Value);
			exImLc.QuotReference = txtquorationref.Text;
			exImLc.InquiryReference = txtinquiryref.Text;
			exImLc.SalesContractRef = txtcustomerrefno.Text;
			exImLc.SalesContratDate = txtreferencedate.Value;
			exImLc.CustomerCode = txtcustomercode.Text;
			exImLc.LotReference = "";
			exImLc.InsepctionRequired = ((Control)(object)cmbinspection).Text;
			exImLc.InsepctionDescription = txtinspectiondesc.Text;
			exImLc.DeliveryTermId = Conversion.ToInt(cmbdeliveryterm.Value);
			exImLc.PaymentTermId = Conversion.ToInt(cmbpaymenterm.Value);
			exImLc.ImporterBankId = Conversion.ToInt(cmbimporterbank.Value);
			exImLc.ExporterBankId = Conversion.ToInt(cmbExporterBanks.Value);
			exImLc.LoadingPortId = Conversion.ToInt(cmbloadingport.Value);
			exImLc.DestinationPortId = Conversion.ToInt(cmbdestinationport.Value);
			exImLc.FcurrencyId = Conversion.ToInt(cmbfc.Value);
			exImLc.FcurrencySymbol = "";
			exImLc.FCurrencyAmount = double.Parse(txttotalfcamount.Text);
			exImLc.ConversionRate = double.Parse(txtexchangerate.Text);
			exImLc.EquivalentAmount = 0.0;
			exImLc.CommodityDetial = txtcommoditydesc.Text;
			exImLc.ShippingMarks = txtshippingmark.Text;
			exImLc.GrossWeightKgs = Conversion.ToDouble((object)txtgrossweight.Text);
			exImLc.NetWeightKgs = Conversion.ToDouble((object)txtnetweight.Text);
			exImLc.NoOfContainers = Conversion.ToDouble((object)txtnoofcontainer.Text);
			exImLc.RemarksHeader = txtremarks.Text;
			exImLc.LegalizationRequired = ((Control)(object)cmblegalization).Text;
			exImLc.LegalizationDescription = txtlegalizationdesc.Text;
			exImLc.LcOrderStatusId = 0;
			exImLc.Status = ((Control)(object)cmbstatus).Text;
			exImLc.IsApproved = false;
			exImLc.EntryDate = DateTime.Now;
			exImLc.EntryUser = UserAccount.ID;
			exImLc.ModifyDate = DateTime.Now;
			exImLc.ModifyUser = UserAccount.ID;
			exImLc.PostDate = DateTime.Now;
			exImLc.PostState = false;
			exImLc.PostUser = UserAccount.ID;
			exImLc.LcDocAttachment = "";
			exImLc.LcOrderDetail = new List<ExImLcOrderPackingDetail>();
			exImLc.ExImLcOrderOtherItemslist = new List<ExImLcOrderOtherItems>();
			exImLc.ExImLcContractPackingMaterialDetailslist = new List<ExImLcContractPackingMaterialDetail>();
			exImLc.ExImLcOrderPaymentTermsDetailslist = new List<ExImLcOrderPaymentTermsDetail>();
			if (grdDetail.RowCount > 0)
			{
				GridEXRow[] rows = grdDetail.GetRows();
				foreach (GridEXRow r in rows)
				{
					ExImLcOrderPackingDetail lcOrderPackingDetail = new ExImLcOrderPackingDetail();
					lcOrderPackingDetail.ExImItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					lcOrderPackingDetail.InvPackingMaterialTypeId = Conversion.ToInt(r.Cells["PackTypeId"].Value);
					lcOrderPackingDetail.InnerQty = Conversion.ToInt(r.Cells["InnerQty"].Value);
					lcOrderPackingDetail.UOMScheduleIdInner = Conversion.ToInt(r.Cells["InnerPackUOMId"].Value);
					lcOrderPackingDetail.OuterQty = Conversion.ToInt(r.Cells["OuterQty"].Value);
					lcOrderPackingDetail.UOMScheduleIdOuter = Conversion.ToInt(r.Cells["OuterPackUOMId"].Value);
					lcOrderPackingDetail.NetWeight = Conversion.ToInt(r.Cells["Weigth"].Value);
					lcOrderPackingDetail.RatePrice = Conversion.ToInt(r.Cells["Rate"].Value);
					lcOrderPackingDetail.UOMScheduleIdRate = Conversion.ToInt(r.Cells["RateUOMId"].Value);
					lcOrderPackingDetail.TotalAmount = Conversion.ToInt(r.Cells["Amount"].Value);
					lcOrderPackingDetail.Other_Description = Conversion.ToString((object)r.Cells["Description"].Text);
					lcOrderPackingDetail.NoOfUnit = 0.0;
					lcOrderPackingDetail.ItemBrandId = 0;
					exImLc.LcOrderDetail.Add(lcOrderPackingDetail);
				}
			}
			exImLc.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			for (int i = 0; i < Attachment.dtAttachments.Rows.Count; i++)
			{
				DMSAttachments attach = new DMSAttachments();
				attach.Attachment = Attachment.dtAttachments.Rows[i][1].ToString();
				attach.ScreenName = base.Name;
				exImLc.AttachmentsList.Add(attach);
			}
			ExImLcOrder.Save(exImLc);
			if (!UpdateMode)
			{
				MessageBox.Show("Save SuccessFully");
			}
			if (UpdateMode)
			{
				MessageBox.Show("Update SuccessFully");
			}
			FormReset();
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
				dtdetail.Rows.Add(cmbitem.Value, ((Control)(object)cmbitem).Text, cmbinnerpacktype.Value, ((Control)(object)cmbinnerpacktype).Text, txtinnerQty.Text, cmbinnerpackuom.Value, ((Control)(object)cmbinnerpackuom).Text, txtOuterQty.Text, cmbouterpackuom.Value, ((Control)(object)cmbouterpackuom).Text, txtweight.Text, txtrate.Text, cmbrateuom.Value, ((Control)(object)cmbrateuom).Text, txtamount.Text, txtdescription.Text);
				grdDetail.DataSource = dtdetail;
				grdDetail.RetrieveStructure();
				gridsetting();
				formResetDetail();
				AddNetWeightandGrossWeightInHeader();
				GetTotalAmountTotalFromGrid();
				cmbitem.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void gridsetting()
	{
		grdDetail.RootTable.Columns["InnerQty"].AggregateFunction = (AggregateFunction)2;
		grdDetail.RootTable.Columns["InnerQty"].TextAlignment = (TextAlignment)3;
		grdDetail.RootTable.Columns["InnerQty"].FormatString = "0,0";
		grdDetail.RootTable.Columns["InnerQty"].TotalFormatString = "#,##0.##";
		grdDetail.RootTable.Columns["OuterQty"].AggregateFunction = (AggregateFunction)2;
		grdDetail.RootTable.Columns["OuterQty"].TextAlignment = (TextAlignment)3;
		grdDetail.RootTable.Columns["OuterQty"].FormatString = "0,0";
		grdDetail.RootTable.Columns["OuterQty"].TotalFormatString = "#,##0.##";
		grdDetail.RootTable.Columns["Weigth"].AggregateFunction = (AggregateFunction)2;
		grdDetail.RootTable.Columns["Weigth"].TextAlignment = (TextAlignment)3;
		grdDetail.RootTable.Columns["Weigth"].FormatString = "0,0";
		grdDetail.RootTable.Columns["Weigth"].TotalFormatString = "#,##0.##";
		grdDetail.RootTable.Columns["Rate"].AggregateFunction = (AggregateFunction)3;
		grdDetail.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
		grdDetail.RootTable.Columns["Rate"].FormatString = "0,0";
		grdDetail.RootTable.Columns["Rate"].TotalFormatString = "#,##0.##";
		grdDetail.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
		grdDetail.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
		grdDetail.RootTable.Columns["Amount"].FormatString = "0,0";
		grdDetail.RootTable.Columns["Amount"].TotalFormatString = "#,##0.##";
		grdDetail.RootTable.Columns["ItemId"].Visible = false;
		grdDetail.RootTable.Columns["PackTypeId"].Visible = false;
		grdDetail.RootTable.Columns["InnerPackUOMId"].Visible = false;
		grdDetail.RootTable.Columns["OuterPackUOMId"].Visible = false;
		grdDetail.RootTable.Columns["RateUOMId"].Visible = false;
	}

	private void btnUpdate_Click(object sender, EventArgs e)
	{
		btnSave_Click(null, null);
	}

	private bool FormValidation()
	{
		if (Conversion.ToDouble(grdDetail.GetTotal(grdDetail.RootTable.Columns["Amount"], (AggregateFunction)2)) != Conversion.ToDouble((object)txttotalfcamount.Text))
		{
			MessageBox.Show("Fcy Amount and Detail Amount Should be Same", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return false;
		}
		if (Conversion.ToInt((object)txtgrossweight.Text) < Conversion.ToInt((object)txtnetweight.Text))
		{
			MessageBox.Show("Gross Weight Should Not be Less then Net Weight", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			return false;
		}
		if (((Control)(object)cmbbranch).Text == string.Empty || Conversion.ToInt(cmbbranch.Value) == 0)
		{
			MessageBox.Show("Branh Field is Required!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			cmbbranch.Focus();
			return false;
		}
		if (((Control)(object)cmbproject).Text == string.Empty || Conversion.ToInt(cmbproject.Value) == 0)
		{
			MessageBox.Show("Project Field is Required!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			cmbproject.Focus();
			return false;
		}
		if (((Control)(object)cmbcustomer).Text == string.Empty || Conversion.ToInt(cmbcustomer.Value) == 0)
		{
			MessageBox.Show("Customer Field is Required!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			cmbcustomer.Focus();
			return false;
		}
		if (((Control)(object)cmbdeliveryterm).Text == string.Empty || Conversion.ToInt(cmbdeliveryterm.Value) == 0)
		{
			MessageBox.Show("Delivery Term Field is Required!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			cmbdeliveryterm.Focus();
			return false;
		}
		if (((Control)(object)cmbpaymenterm).Text == string.Empty || Conversion.ToInt(cmbpaymenterm.Value) == 0)
		{
			MessageBox.Show("Payment Term Field is Required!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			cmbpaymenterm.Focus();
			return false;
		}
		if (((Control)(object)cmbimporterbank).Text == string.Empty || Conversion.ToInt(cmbimporterbank.Value) == 0)
		{
			MessageBox.Show("Importer Bank Field is Required!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			cmbimporterbank.Focus();
			return false;
		}
		if (((Control)(object)cmbExporterBanks).Text == string.Empty || Conversion.ToInt(cmbExporterBanks.Value) == 0)
		{
			MessageBox.Show("Exporter Bank Field is Required!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			cmbExporterBanks.Focus();
			return false;
		}
		if (((Control)(object)cmbloadingport).Text == string.Empty || Conversion.ToInt(cmbloadingport.Value) == 0)
		{
			MessageBox.Show("Loading Port Field is Required!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			cmbloadingport.Focus();
			return false;
		}
		if (((Control)(object)cmbdestinationport).Text == string.Empty || Conversion.ToInt(cmbdestinationport.Value) == 0)
		{
			MessageBox.Show("Destination Port Field is Required!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			cmbdestinationport.Focus();
			return false;
		}
		if (((Control)(object)cmbstatus).Text == string.Empty || Conversion.ToInt(cmbstatus.Value) == 0)
		{
			MessageBox.Show("Status Field is Required!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			cmbstatus.Focus();
			return false;
		}
		if (txtdocno.Text == string.Empty || Conversion.ToInt((object)txtdocno.Text) == 0)
		{
			MessageBox.Show("Doc No Field is Required!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			txtdocno.Focus();
			return false;
		}
		if (txttotalfcamount.Text == string.Empty || Conversion.ToInt((object)txttotalfcamount.Text) == 0)
		{
			MessageBox.Show("Fcy Amount Field is Required!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			txttotalfcamount.Focus();
			return false;
		}
		if (txtexchangerate.Text == string.Empty || Conversion.ToInt((object)txtexchangerate.Text) == 0)
		{
			MessageBox.Show("Exchange Rate Field is Required!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			txtexchangerate.Focus();
			return false;
		}
		if (txtlocalcurramount.Text == string.Empty || Conversion.ToInt((object)txtlocalcurramount.Text) == 0)
		{
			MessageBox.Show("Local Amount Field is Required!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			txtlocalcurramount.Focus();
			return false;
		}
		if (txtgrossweight.Text == string.Empty || Conversion.ToInt((object)txtgrossweight.Text) == 0)
		{
			MessageBox.Show("Gross Weight Field is Required!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			txtgrossweight.Focus();
			return false;
		}
		if (txtnetweight.Text == string.Empty || Conversion.ToInt((object)txtnetweight.Text) == 0)
		{
			MessageBox.Show("Net Weight Field is Required!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			txtnetweight.Focus();
			return false;
		}
		return true;
	}

	private bool DetailFormValidation()
	{
		if (((Control)(object)cmbitem).Text == string.Empty || Conversion.ToInt(cmbitem.Value) == 0)
		{
			MessageBox.Show("Item Field is Required!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			cmbitem.Focus();
			return false;
		}
		if (((Control)(object)cmbinnerpacktype).Text == string.Empty || Conversion.ToInt(cmbinnerpacktype.Value) == 0)
		{
			MessageBox.Show("Packing Type Field is Required!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			cmbinnerpacktype.Focus();
			return false;
		}
		if (txtOuterQty.Text == string.Empty || Conversion.ToInt((object)txtOuterQty.Text) == 0)
		{
			MessageBox.Show("Outer Qty Field is Required!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			txtOuterQty.Focus();
			return false;
		}
		if (((Control)(object)cmbouterpackuom).Text == string.Empty || Conversion.ToInt(cmbouterpackuom.Value) == 0)
		{
			MessageBox.Show("Outer Pack UOM Field is Required!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			cmbouterpackuom.Focus();
			return false;
		}
		if (txtweight.Text == string.Empty)
		{
			MessageBox.Show("Weight Field is Required!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			txtweight.Focus();
			return false;
		}
		if (txtrate.Text == string.Empty || Conversion.ToInt((object)txtrate.Text) == 0)
		{
			MessageBox.Show("Rate Field is Required!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			txtrate.Focus();
			return false;
		}
		if (((Control)(object)cmbrateuom).Text == string.Empty || Conversion.ToInt(cmbrateuom.Value) == 0)
		{
			MessageBox.Show("Rate UOM Field is Required!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			cmbrateuom.Focus();
			return false;
		}
		if (txtamount.Text == string.Empty || Conversion.ToInt((object)txtamount.Text) == 0)
		{
			MessageBox.Show("Amount Field is Required!", "WARNING", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			txtamount.Focus();
			return false;
		}
		return true;
	}

	private void LcOrder_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.S && tabControl1.SelectedIndex == 0 && !UpdateMode)
			{
				btnSave_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					btnNew_Click(null, null);
				}
				else
				{
					btnNew_Click(null, null);
				}
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
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.U && UpdateMode)
			{
				btnUpdate_Click(null, null);
			}
			if (e.Control)
			{
				_ = e.KeyCode;
				_ = 80;
			}
		}
		catch (Exception)
		{
			throw;
		}
	}

	private void FormReset()
	{
		btnSave.Visible = true;
		btnUpdate.Visible = false;
		UpdateMode = false;
		txtamount.Text = string.Empty;
		txtcommoditydesc.Text = string.Empty;
		txtcustomercode.Text = string.Empty;
		txtcustomerrefno.Text = string.Empty;
		txtdescription.Text = string.Empty;
		txtdocdate.Value = DateTime.Now;
		txtexchangerate.Text = string.Empty;
		txtexpirydate.Text = string.Empty;
		txtexpplace.Text = string.Empty;
		txtgrossweight.Text = string.Empty;
		txtinquiryref.Text = string.Empty;
		txtlastshipmentdate.Text = string.Empty;
		txtlegalizationdesc.Text = string.Empty;
		txtlocalcurramount.Text = string.Empty;
		txtnetweight.Text = string.Empty;
		txtnoofcontainer.Text = string.Empty;
		((Control)(object)cmbPerformaInvoice).Text = string.Empty;
		txtquorationref.Text = string.Empty;
		txtrate.Text = string.Empty;
		txtreferencedate.Value = DateTime.Now;
		txtremarks.Text = string.Empty;
		txtshippingmark.Text = string.Empty;
		txttotalfcamount.Text = string.Empty;
		txtweight.Text = string.Empty;
		((Control)(object)cmbcustomer).Text = string.Empty;
		((Control)(object)cmbdeliveryterm).Text = string.Empty;
		((Control)(object)cmbdestinationport).Text = string.Empty;
		((Control)(object)cmbExporterBanks).Text = string.Empty;
		((Control)(object)cmbfc).Text = string.Empty;
		((Control)(object)cmbimporterbank).Text = string.Empty;
		((Control)(object)cmbinnerpacktype).Text = string.Empty;
		((Control)(object)cmbinnerpackuom).Text = string.Empty;
		txtinnerQty.Text = string.Empty;
		((Control)(object)cmbinspection).Text = string.Empty;
		txtinspectiondesc.Text = string.Empty;
		((Control)(object)cmbitem).Text = string.Empty;
		((Control)(object)cmblegalization).Text = string.Empty;
		((Control)(object)cmbloadingport).Text = string.Empty;
		((Control)(object)cmbouterpackuom).Text = string.Empty;
		txtOuterQty.Text = string.Empty;
		((Control)(object)cmbPartialShipment).Text = string.Empty;
		((Control)(object)cmbpaymenterm).Text = string.Empty;
		((Control)(object)cmbstatus).Text = string.Empty;
		((Control)(object)cmbrateuom).Text = string.Empty;
		((Control)(object)cmbTransShipment).Text = string.Empty;
		txtdocdate.Focus();
		grdDetail.ClearStructure();
		dtdetail.Clear();
		DocNo();
	}

	public void formResetDetail()
	{
		((Control)(object)cmbitem).Text = string.Empty;
		txtdescription.Text = string.Empty;
		((Control)(object)cmbinnerpacktype).Text = string.Empty;
		txtinnerQty.Text = string.Empty;
		((Control)(object)cmbinnerpackuom).Text = string.Empty;
		txtOuterQty.Text = string.Empty;
		((Control)(object)cmbouterpackuom).Text = string.Empty;
		txtweight.Text = string.Empty;
		txtrate.Text = string.Empty;
		((Control)(object)cmbrateuom).Text = string.Empty;
		txtamount.Text = string.Empty;
		ItemDetailFill();
	}

	private void DataGridHistory_DoubleClick(object sender, EventArgs e)
	{
		ReadById(Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells[0].Value.ToString()));
	}

	public void ReadById(int Id)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		try
		{
			btnSave.Visible = false;
			btnUpdate.Visible = true;
			ExImLcOrder exImLc = new ExImLcOrder();
			RecId = Id;
			exImLc = ExImLcOrder.GetByID(RecId);
			tabControl1.SelectedIndex = 0;
			cmbbranch.Value = exImLc.BranchesId;
			cmbPerformaInvoice.Value = exImLc.ExImProformaInvoice.ToString();
			txtdocno.Text = exImLc.LcOrderNo;
			txtdocdate.Value = Conversion.ToDateTime((object)exImLc.LcOrderDate);
			txtexpirydate.Value = Conversion.ToDateTime((object)exImLc.ExpiryDate);
			txtexpplace.Text = exImLc.ExpiryPlace;
			((Control)(object)cmbPartialShipment).Text = Conversion.ToString((object)exImLc.PartialShipment);
			((Control)(object)cmbTransShipment).Text = exImLc.TransShipment;
			txtlastshipmentdate.Value = Conversion.ToDateTime((object)exImLc.LastShipmentDate);
			cmbcustomer.Value = exImLc.SupCustId;
			txtquorationref.Text = exImLc.QuotReference;
			txtinquiryref.Text = exImLc.InquiryReference;
			txtcustomerrefno.Text = exImLc.SalesContractRef;
			txtreferencedate.Value = Conversion.ToDateTime((object)exImLc.SalesContratDate);
			txtcustomercode.Text = exImLc.CustomerCode;
			((Control)(object)cmbinspection).Text = exImLc.InsepctionRequired;
			txtinspectiondesc.Text = exImLc.InsepctionDescription;
			cmbdeliveryterm.Value = exImLc.DeliveryTermId;
			cmbpaymenterm.Value = exImLc.PaymentTermId;
			cmbimporterbank.Value = exImLc.ImporterBankId;
			cmbExporterBanks.Value = exImLc.ExporterBankId;
			cmbloadingport.Value = exImLc.LoadingPortId;
			cmbdestinationport.Value = exImLc.DestinationPortId;
			cmbfc.Value = exImLc.FcurrencyId;
			txttotalfcamount.Text = exImLc.FCurrencyAmount.ToString();
			txtexchangerate.Text = exImLc.ConversionRate.ToString();
			txtcommoditydesc.Text = exImLc.CommodityDetial;
			txtshippingmark.Text = exImLc.ShippingMarks;
			txtgrossweight.Text = exImLc.GrossWeightKgs.ToString();
			txtnetweight.Text = exImLc.NetWeightKgs.ToString();
			txtnoofcontainer.Text = exImLc.NoOfContainers.ToString();
			txtremarks.Text = exImLc.RemarksHeader;
			((Control)(object)cmblegalization).Text = exImLc.LegalizationRequired;
			txtlegalizationdesc.Text = exImLc.LegalizationDescription;
			((Control)(object)cmbstatus).Text = exImLc.Status;
			for (int i = 0; i < exImLc.LcOrderDetail.Count; i++)
			{
				dtdetail.Rows.Add(exImLc.LcOrderDetail[i].ExImItemId, exImLc.LcOrderDetail[i].ItemName, exImLc.LcOrderDetail[i].InvPackingMaterialTypeId, exImLc.LcOrderDetail[i].ExImPackMaterialTypeName, exImLc.LcOrderDetail[i].InnerQty, exImLc.LcOrderDetail[i].UOMScheduleIdInner, exImLc.LcOrderDetail[i].UOMScheduleIdInnerName, exImLc.LcOrderDetail[i].OuterQty, exImLc.LcOrderDetail[i].UOMScheduleIdOuter, exImLc.LcOrderDetail[i].UOMScheduleIdOuterName, exImLc.LcOrderDetail[i].NetWeight, exImLc.LcOrderDetail[i].RatePrice, exImLc.LcOrderDetail[i].UOMScheduleIdRate, exImLc.LcOrderDetail[i].UOMScheduleIdRateName, exImLc.LcOrderDetail[i].TotalAmount, exImLc.LcOrderDetail[i].Other_Description);
			}
			grdDetail.DataSource = dtdetail;
			grdDetail.RetrieveStructure();
			gridsetting();
			UpdateMode = true;
			List<DMSAttachments> byID = global::Architecture.BLL.DMSAttachments.GetByID(Id, base.Name);
			AT = new Attachment();
			foreach (global::Architecture.Model.DMSAttachments Attachment in byID)
			{
				TempAttachments TA = new TempAttachments();
				TA.Id = 0;
				TA.Path = Attachment.Attachment;
				AT.lst.Add(TA);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void gridFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		try
		{
			DataTable lst = ExImLcOrder.GetData(new ExImLcOrder
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = 201
			});
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Customer");
			dt.Columns.Add("PerformaInvoice");
			dt.Columns.Add("Quotation Ref");
			dt.Columns.Add("InquiryRef");
			dt.Columns.Add("CustomerCode");
			dt.Columns.Add("ExpiryDate");
			dt.Columns.Add("LastShipmentDate");
			dt.Columns.Add("PartialShipment");
			dt.Columns.Add("LoadingPort");
			dt.Columns.Add("DestinationPort");
			dt.Columns.Add("ImporterBank");
			dt.Columns.Add("ExporterBank");
			dt.Columns.Add("ExchangeRate");
			dt.Columns.Add("GrossWeight");
			dt.Columns.Add("NoOfContainer");
			dt.Columns.Add("NoOfAttachments");
			for (int i = 0; i < lst.Rows.Count; i++)
			{
				dt.Rows.Add(lst.Rows[i]["Id"], lst.Rows[i]["CompanyName"], lst.Rows[i]["ExImProformaInvoice"], lst.Rows[i]["QuotReference"], lst.Rows[i]["InquiryReference"], lst.Rows[i]["CustomerCode"], lst.Rows[i]["ExpiryDate"], lst.Rows[i]["LastShipmentDate"], lst.Rows[i]["PartialShipment"], lst.Rows[i]["LoadingPort"], lst.Rows[i]["DestinationPort"], lst.Rows[i]["ImporterBank"], lst.Rows[i]["ExporterBank"], lst.Rows[i]["ConversionRate"], lst.Rows[i]["GrossWeightKgs"], lst.Rows[i]["NoOfContainers"], lst.Rows[i]["NoOfAttachments"]);
			}
			DataGridHistory.DataSource = dt;
			DataGridHistory.RetrieveStructure();
			GridSettingForHistory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSettingForHistory()
	{
		try
		{
			DataGridHistory.RootTable.Columns["Id"].Visible = false;
			DataGridHistory.RootTable.Columns["GrossWeight"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["GrossWeight"].TextAlignment = (TextAlignment)3;
			DataGridHistory.RootTable.Columns["GrossWeight"].FormatString = "0,0";
			DataGridHistory.RootTable.Columns["GrossWeight"].TotalFormatString = "#,##0.##";
			DataGridHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			ctrlGrdBar2_Load(null, null);
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

	private void grdDetail_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grdDetail.CurrentRow;
			if (item != null)
			{
				updateDetailIndex = item.RowIndex;
				((Control)(object)cmbitem).Text = item.Cells["Item"].Text.ToString();
				((Control)(object)cmbinnerpacktype).Text = item.Cells["PackType"].Text.ToString();
				txtinnerQty.Text = item.Cells["InnerQty"].Value.ToString();
				((Control)(object)cmbinnerpackuom).Text = item.Cells["InnerPackUOM"].Text.ToString();
				txtOuterQty.Text = item.Cells["OuterQty"].Value.ToString();
				((Control)(object)cmbouterpackuom).Text = item.Cells["OuterPackUOM"].Text.ToString();
				txtweight.Text = item.Cells["Weigth"].Value.ToString();
				txtrate.Text = item.Cells["Rate"].Value.ToString();
				((Control)(object)cmbrateuom).Text = item.Cells["RateUOM"].Text.ToString();
				txtdescription.Text = item.Cells["Description"].Text.ToString();
				btnaddnew.Visible = false;
				btnGridUpdate.Visible = true;
				btnCancel.Visible = true;
				gridsetting();
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
				dtdetail.Rows[updateDetailIndex]["ItemId"] = cmbitem.Value;
				dtdetail.Rows[updateDetailIndex]["Item"] = ((Control)(object)cmbitem).Text;
				dtdetail.Rows[updateDetailIndex]["PackTypeId"] = cmbinnerpacktype.Value;
				dtdetail.Rows[updateDetailIndex]["PackType"] = ((Control)(object)cmbinnerpacktype).Text;
				dtdetail.Rows[updateDetailIndex]["InnerQty"] = txtinnerQty.Text;
				dtdetail.Rows[updateDetailIndex]["InnerPackUOMId"] = cmbinnerpackuom.Value;
				dtdetail.Rows[updateDetailIndex]["InnerPackUOM"] = ((Control)(object)cmbinnerpackuom).Text;
				dtdetail.Rows[updateDetailIndex]["OuterQty"] = txtOuterQty.Text;
				dtdetail.Rows[updateDetailIndex]["OuterPackUOMId"] = cmbouterpackuom.Value;
				dtdetail.Rows[updateDetailIndex]["OuterPackUOM"] = ((Control)(object)cmbouterpackuom).Text;
				dtdetail.Rows[updateDetailIndex]["Weigth"] = txtweight.Text;
				dtdetail.Rows[updateDetailIndex]["Rate"] = txtrate.Text;
				dtdetail.Rows[updateDetailIndex]["RateUOMId"] = cmbrateuom.Value;
				dtdetail.Rows[updateDetailIndex]["RateUOM"] = ((Control)(object)cmbrateuom).Text;
				dtdetail.Rows[updateDetailIndex]["Amount"] = txtamount.Text;
				dtdetail.Rows[updateDetailIndex]["Description"] = txtdescription.Text;
				btnGridUpdate.Visible = false;
				btnCancel.Visible = false;
				btnaddnew.Visible = true;
				gridsetting();
				formResetDetail();
				AddNetWeightandGrossWeightInHeader();
				GetTotalAmountTotalFromGrid();
				cmbitem.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancel_Click(object sender, EventArgs e)
	{
		gridsetting();
		btnGridUpdate.Visible = false;
		btnCancel.Visible = false;
		btnaddnew.Visible = false;
		formResetDetail();
		cmbitem.Focus();
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		FormReset();
		formResetDetail();
	}

	private void CalculateLocalAmount()
	{
		try
		{
			double fcyAmount = Conversion.ToDouble((object)txttotalfcamount.Text);
			double ExchangeRate = Conversion.ToDouble((object)txtexchangerate.Text);
			if (fcyAmount > 0.0 && ExchangeRate > 0.0)
			{
				txtlocalcurramount.Text = (fcyAmount * ExchangeRate).ToString();
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
			double ItemUOM = Conversion.ToDouble((object)((Control)(object)cmbouterpackuom).Text);
			double Item = Conversion.ToDouble((object)txtOuterQty.Text);
			if (ItemUOM > 0.0 && Item > 0.0)
			{
				txtweight.Text = (ItemUOM * Item).ToString();
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
			double Weight = Conversion.ToDouble((object)txtweight.Text);
			double RateUOM = Conversion.ToDouble((object)((Control)(object)cmbrateuom).Text);
			double Rate = Conversion.ToDouble((object)txtrate.Text);
			if (Weight > 0.0 && RateUOM > 0.0 && Rate > 0.0)
			{
				double TotalAmount = Rate / RateUOM * Weight;
				txtamount.Text = TotalAmount.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		gridFill();
	}

	private void txttotalfcamount_TextChanged(object sender, EventArgs e)
	{
		CalculateLocalAmount();
	}

	private void txtexchangerate_TextChanged(object sender, EventArgs e)
	{
		CalculateLocalAmount();
	}

	private void txtOuterQty_TextChanged(object sender, EventArgs e)
	{
		CalculateWeight();
	}

	private void cmbouterpackuom_TextChanged(object sender, EventArgs e)
	{
		CalculateWeight();
	}

	private void txtweight_TextChanged(object sender, EventArgs e)
	{
		CalculateAmount();
	}

	private void txtrate_TextChanged(object sender, EventArgs e)
	{
		CalculateAmount();
	}

	private void cmbrateuom_TextChanged(object sender, EventArgs e)
	{
		CalculateAmount();
	}

	private void AddNetWeightandGrossWeightInHeader()
	{
		try
		{
			object Weight = grdDetail.GetTotal(grdDetail.RootTable.Columns["Weigth"], (AggregateFunction)2);
			txtnetweight.Text = Weight.ToString();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetTotalAmountTotalFromGrid()
	{
		try
		{
			double TotalAmount = Conversion.ToDouble(grdDetail.GetTotal(grdDetail.RootTable.Columns["Amount"], (AggregateFunction)2));
			txttotalfcamount.Text = TotalAmount.ToString();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbcustomer_Leave(object sender, EventArgs e)
	{
		try
		{
			PerformaInvoie();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txttotalfcamount_KeyPress(object sender, KeyPressEventArgs e)
	{
		e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
	}

	private void txtexchangerate_KeyPress(object sender, KeyPressEventArgs e)
	{
		e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
	}

	private void txtgrossweight_KeyPress(object sender, KeyPressEventArgs e)
	{
		e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
	}

	private void txtinnerQty_KeyPress(object sender, KeyPressEventArgs e)
	{
		e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
	}

	private void txtOuterQty_KeyPress(object sender, KeyPressEventArgs e)
	{
		e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
	}

	private void txtweight_KeyPress(object sender, KeyPressEventArgs e)
	{
		e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
	}

	private void txtrate_KeyPress(object sender, KeyPressEventArgs e)
	{
		e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
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
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value), 201);
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
		//IL_0700: Unknown result type (might be due to invalid IL or missing references)
		//IL_070a: Expected O, but got Unknown
		//IL_0779: Unknown result type (might be due to invalid IL or missing references)
		//IL_0783: Expected O, but got Unknown
		//IL_078f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0799: Expected O, but got Unknown
		//IL_079a: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a4: Expected O, but got Unknown
		//IL_07a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07af: Expected O, but got Unknown
		//IL_0860: Unknown result type (might be due to invalid IL or missing references)
		//IL_086a: Expected O, but got Unknown
		//IL_086b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0875: Expected O, but got Unknown
		//IL_0876: Unknown result type (might be due to invalid IL or missing references)
		//IL_0880: Expected O, but got Unknown
		//IL_0881: Unknown result type (might be due to invalid IL or missing references)
		//IL_088b: Expected O, but got Unknown
		//IL_088c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0896: Expected O, but got Unknown
		//IL_0897: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a1: Expected O, but got Unknown
		//IL_08a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ac: Expected O, but got Unknown
		//IL_08ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b7: Expected O, but got Unknown
		//IL_08b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c2: Expected O, but got Unknown
		//IL_08c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_08cd: Expected O, but got Unknown
		//IL_08ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d8: Expected O, but got Unknown
		//IL_08d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e3: Expected O, but got Unknown
		//IL_08e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ee: Expected O, but got Unknown
		//IL_0952: Unknown result type (might be due to invalid IL or missing references)
		//IL_095c: Expected O, but got Unknown
		//IL_095d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0967: Expected O, but got Unknown
		//IL_0968: Unknown result type (might be due to invalid IL or missing references)
		//IL_0972: Expected O, but got Unknown
		//IL_0973: Unknown result type (might be due to invalid IL or missing references)
		//IL_097d: Expected O, but got Unknown
		//IL_097e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0988: Expected O, but got Unknown
		//IL_1018: Unknown result type (might be due to invalid IL or missing references)
		//IL_1022: Expected O, but got Unknown
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance227 = new Appearance();
		Appearance appearance228 = new Appearance();
		Appearance appearance229 = new Appearance();
		Appearance appearance230 = new Appearance();
		Appearance appearance232 = new Appearance();
		Appearance appearance233 = new Appearance();
		Appearance appearance234 = new Appearance();
		Appearance appearance235 = new Appearance();
		Appearance appearance236 = new Appearance();
		Appearance appearance237 = new Appearance();
		Appearance appearance238 = new Appearance();
		Appearance appearance239 = new Appearance();
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
		Appearance appearance212 = new Appearance();
		Appearance appearance213 = new Appearance();
		Appearance appearance214 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance187 = new Appearance();
		Appearance appearance198 = new Appearance();
		Appearance appearance209 = new Appearance();
		Appearance appearance220 = new Appearance();
		Appearance appearance231 = new Appearance();
		Appearance appearance242 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
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
		Appearance appearance168 = new Appearance();
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
		Appearance appearance185 = new Appearance();
		Appearance appearance186 = new Appearance();
		Appearance appearance188 = new Appearance();
		Appearance appearance189 = new Appearance();
		Appearance appearance190 = new Appearance();
		Appearance appearance191 = new Appearance();
		Appearance appearance192 = new Appearance();
		Appearance appearance193 = new Appearance();
		Appearance appearance194 = new Appearance();
		Appearance appearance195 = new Appearance();
		Appearance appearance196 = new Appearance();
		Appearance appearance197 = new Appearance();
		Appearance appearance199 = new Appearance();
		Appearance appearance200 = new Appearance();
		Appearance appearance201 = new Appearance();
		Appearance appearance202 = new Appearance();
		Appearance appearance203 = new Appearance();
		Appearance appearance204 = new Appearance();
		Appearance appearance205 = new Appearance();
		Appearance appearance206 = new Appearance();
		Appearance appearance207 = new Appearance();
		Appearance appearance208 = new Appearance();
		Appearance appearance210 = new Appearance();
		Appearance appearance211 = new Appearance();
		Appearance appearance215 = new Appearance();
		Appearance appearance216 = new Appearance();
		Appearance appearance217 = new Appearance();
		Appearance appearance218 = new Appearance();
		Appearance appearance219 = new Appearance();
		Appearance appearance221 = new Appearance();
		Appearance appearance222 = new Appearance();
		Appearance appearance223 = new Appearance();
		Appearance appearance224 = new Appearance();
		Appearance appearance225 = new Appearance();
		Appearance appearance226 = new Appearance();
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
		Appearance appearance240 = new Appearance();
		Appearance appearance241 = new Appearance();
		Appearance appearance243 = new Appearance();
		Appearance appearance244 = new Appearance();
		Appearance appearance245 = new Appearance();
		Appearance appearance246 = new Appearance();
		Appearance appearance247 = new Appearance();
		Appearance appearance248 = new Appearance();
		Appearance appearance249 = new Appearance();
		Appearance appearance250 = new Appearance();
		Appearance appearance251 = new Appearance();
		Appearance appearance252 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance158 = new Appearance();
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
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
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
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
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
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Export.LcOrder));
		this.DataGridHistory = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.panel11 = new System.Windows.Forms.Panel();
		this.panel9 = new System.Windows.Forms.Panel();
		this.label56 = new System.Windows.Forms.Label();
		this.History = new System.Windows.Forms.TabPage();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.Form = new System.Windows.Forms.TabPage();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.grdDetail = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.cmbPerformaInvoice = new UltraCombo();
		this.cmbTransShipment = new UltraCombo();
		this.cmbPartialShipment = new UltraCombo();
		this.label72 = new System.Windows.Forms.Label();
		this.label71 = new System.Windows.Forms.Label();
		this.label70 = new System.Windows.Forms.Label();
		this.label69 = new System.Windows.Forms.Label();
		this.label68 = new System.Windows.Forms.Label();
		this.label67 = new System.Windows.Forms.Label();
		this.label66 = new System.Windows.Forms.Label();
		this.label65 = new System.Windows.Forms.Label();
		this.label64 = new System.Windows.Forms.Label();
		this.label63 = new System.Windows.Forms.Label();
		this.label62 = new System.Windows.Forms.Label();
		this.label61 = new System.Windows.Forms.Label();
		this.label60 = new System.Windows.Forms.Label();
		this.label58 = new System.Windows.Forms.Label();
		this.label57 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.cmbstatus = new UltraCombo();
		this.cmbinspection = new UltraCombo();
		this.cmblegalization = new UltraCombo();
		this.cmbfc = new UltraCombo();
		this.cmbdestinationport = new UltraCombo();
		this.cmbloadingport = new UltraCombo();
		this.cmbExporterBanks = new UltraCombo();
		this.cmbimporterbank = new UltraCombo();
		this.cmbpaymenterm = new UltraCombo();
		this.cmbdeliveryterm = new UltraCombo();
		this.cmbcustomer = new UltraCombo();
		this.cmbproject = new UltraCombo();
		this.cmbbranch = new UltraCombo();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.label80 = new System.Windows.Forms.Label();
		this.label79 = new System.Windows.Forms.Label();
		this.label78 = new System.Windows.Forms.Label();
		this.label77 = new System.Windows.Forms.Label();
		this.label76 = new System.Windows.Forms.Label();
		this.label75 = new System.Windows.Forms.Label();
		this.label74 = new System.Windows.Forms.Label();
		this.label73 = new System.Windows.Forms.Label();
		this.cmbrateuom = new UltraCombo();
		this.cmbouterpackuom = new UltraCombo();
		this.cmbinnerpackuom = new UltraCombo();
		this.cmbinnerpacktype = new UltraCombo();
		this.cmbitem = new UltraCombo();
		this.txtOuterQty = new System.Windows.Forms.TextBox();
		this.txtinnerQty = new System.Windows.Forms.TextBox();
		this.btnCancel = new System.Windows.Forms.Button();
		this.btnGridUpdate = new System.Windows.Forms.Button();
		this.btnaddnew = new System.Windows.Forms.Button();
		this.label3 = new System.Windows.Forms.Label();
		this.label45 = new System.Windows.Forms.Label();
		this.label47 = new System.Windows.Forms.Label();
		this.label48 = new System.Windows.Forms.Label();
		this.label49 = new System.Windows.Forms.Label();
		this.label50 = new System.Windows.Forms.Label();
		this.label51 = new System.Windows.Forms.Label();
		this.txtamount = new System.Windows.Forms.TextBox();
		this.label52 = new System.Windows.Forms.Label();
		this.txtrate = new System.Windows.Forms.TextBox();
		this.label53 = new System.Windows.Forms.Label();
		this.txtweight = new System.Windows.Forms.TextBox();
		this.label54 = new System.Windows.Forms.Label();
		this.txtdescription = new System.Windows.Forms.TextBox();
		this.label55 = new System.Windows.Forms.Label();
		this.txtexpirydate = new System.Windows.Forms.DateTimePicker();
		this.label12 = new System.Windows.Forms.Label();
		this.txtlastshipmentdate = new System.Windows.Forms.DateTimePicker();
		this.label14 = new System.Windows.Forms.Label();
		this.txtreferencedate = new System.Windows.Forms.DateTimePicker();
		this.label10 = new System.Windows.Forms.Label();
		this.txtdocdate = new System.Windows.Forms.DateTimePicker();
		this.label46 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.txtremarks = new System.Windows.Forms.TextBox();
		this.label38 = new System.Windows.Forms.Label();
		this.txtexpplace = new System.Windows.Forms.TextBox();
		this.txtcustomerrefno = new System.Windows.Forms.TextBox();
		this.txtinquiryref = new System.Windows.Forms.TextBox();
		this.txtlocalcurramount = new System.Windows.Forms.TextBox();
		this.txtnetweight = new System.Windows.Forms.TextBox();
		this.txtgrossweight = new System.Windows.Forms.TextBox();
		this.txtexchangerate = new System.Windows.Forms.TextBox();
		this.txttotalfcamount = new System.Windows.Forms.TextBox();
		this.txtcustomercode = new System.Windows.Forms.TextBox();
		this.txtquorationref = new System.Windows.Forms.TextBox();
		this.label9 = new System.Windows.Forms.Label();
		this.txtshippingmark = new System.Windows.Forms.TextBox();
		this.txtinspectiondesc = new System.Windows.Forms.TextBox();
		this.txtlegalizationdesc = new System.Windows.Forms.TextBox();
		this.txtnoofcontainer = new System.Windows.Forms.TextBox();
		this.label41 = new System.Windows.Forms.Label();
		this.label44 = new System.Windows.Forms.Label();
		this.txtcommoditydesc = new System.Windows.Forms.TextBox();
		this.label39 = new System.Windows.Forms.Label();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.label20 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.label24 = new System.Windows.Forms.Label();
		this.label22 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.label36 = new System.Windows.Forms.Label();
		this.label31 = new System.Windows.Forms.Label();
		this.label35 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.label26 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.label15 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label43 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.label40 = new System.Windows.Forms.Label();
		this.label34 = new System.Windows.Forms.Label();
		this.label37 = new System.Windows.Forms.Label();
		this.label32 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label42 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
		this.Print = new System.Windows.Forms.ToolStripButton();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel8.SuspendLayout();
		this.panel11.SuspendLayout();
		this.panel9.SuspendLayout();
		this.History.SuspendLayout();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.Form.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbPerformaInvoice).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbTransShipment).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPartialShipment).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbstatus).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbinspection).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmblegalization).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbfc).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbdestinationport).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbloadingport).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbExporterBanks).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbimporterbank).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbpaymenterm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbdeliveryterm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcustomer).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbproject).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbbranch).BeginInit();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbrateuom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbouterpackuom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbinnerpackuom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbinnerpacktype).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbitem).BeginInit();
		this.panel4.SuspendLayout();
		this.panel3.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.DataGridHistory.AllowDelete = (InheritableBoolean)1;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistory.DynamicFiltering = true;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1155, 629);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 43;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.LinkClicked += new ColumnActionEventHandler(DataGridHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).DoubleClick += new System.EventHandler(DataGridHistory_DoubleClick);
		this.panel8.Controls.Add(this.panel11);
		this.panel8.Controls.Add(this.panel9);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel8.Location = new System.Drawing.Point(2, 2);
		this.panel8.Margin = new System.Windows.Forms.Padding(2);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1155, 669);
		this.panel8.TabIndex = 0;
		this.panel11.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel11.Location = new System.Drawing.Point(0, 40);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1155, 629);
		this.panel11.TabIndex = 6;
		this.panel9.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel9.Controls.Add(this.ctrlGrdBar2);
		this.panel9.Controls.Add(this.label56);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(0, 0);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1155, 40);
		this.panel9.TabIndex = 4;
		this.label56.AutoSize = true;
		this.label56.Font = new System.Drawing.Font("Times New Roman", 15.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label56.ForeColor = System.Drawing.Color.White;
		this.label56.Location = new System.Drawing.Point(6, 6);
		this.label56.Name = "label56";
		this.label56.Size = new System.Drawing.Size(75, 24);
		this.label56.TabIndex = 1;
		this.label56.Text = "History";
		this.History.Controls.Add(this.panel8);
		this.History.Location = new System.Drawing.Point(4, 4);
		this.History.Margin = new System.Windows.Forms.Padding(2);
		this.History.Name = "History";
		this.History.Padding = new System.Windows.Forms.Padding(2);
		this.History.Size = new System.Drawing.Size(1159, 673);
		this.History.TabIndex = 1;
		this.History.Text = "History";
		this.History.UseVisualStyleBackColor = true;
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1167, 699);
		this.panel1.TabIndex = 50;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.Form);
		this.tabControl1.Controls.Add(this.History);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1167, 699);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.Form.Controls.Add(this.panel2);
		this.Form.Location = new System.Drawing.Point(4, 4);
		this.Form.Margin = new System.Windows.Forms.Padding(2);
		this.Form.Name = "Form";
		this.Form.Padding = new System.Windows.Forms.Padding(2);
		this.Form.Size = new System.Drawing.Size(1159, 673);
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
		this.panel2.Size = new System.Drawing.Size(1155, 669);
		this.panel2.TabIndex = 0;
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(0, 487);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1155, 182);
		this.panel6.TabIndex = 1;
		this.grdDetail.AllowEdit = (InheritableBoolean)2;
		grdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdDetail.DynamicFiltering = true;
		this.grdDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1155, 182);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 0;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdDetail).DoubleClick += new System.EventHandler(grdDetail_DoubleClick);
		this.panel5.BackColor = System.Drawing.Color.White;
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPerformaInvoice);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbTransShipment);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPartialShipment);
		this.panel5.Controls.Add(this.label72);
		this.panel5.Controls.Add(this.label71);
		this.panel5.Controls.Add(this.label70);
		this.panel5.Controls.Add(this.label69);
		this.panel5.Controls.Add(this.label68);
		this.panel5.Controls.Add(this.label67);
		this.panel5.Controls.Add(this.label66);
		this.panel5.Controls.Add(this.label65);
		this.panel5.Controls.Add(this.label64);
		this.panel5.Controls.Add(this.label63);
		this.panel5.Controls.Add(this.label62);
		this.panel5.Controls.Add(this.label61);
		this.panel5.Controls.Add(this.label60);
		this.panel5.Controls.Add(this.label58);
		this.panel5.Controls.Add(this.label57);
		this.panel5.Controls.Add(this.label1);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbstatus);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbinspection);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.cmblegalization);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbfc);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbdestinationport);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbloadingport);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbExporterBanks);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbimporterbank);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbpaymenterm);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbdeliveryterm);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbcustomer);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbproject);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbbranch);
		this.panel5.Controls.Add(this.groupBox1);
		this.panel5.Controls.Add(this.txtexpirydate);
		this.panel5.Controls.Add(this.label12);
		this.panel5.Controls.Add(this.txtlastshipmentdate);
		this.panel5.Controls.Add(this.label14);
		this.panel5.Controls.Add(this.txtreferencedate);
		this.panel5.Controls.Add(this.label10);
		this.panel5.Controls.Add(this.txtdocdate);
		this.panel5.Controls.Add(this.label46);
		this.panel5.Controls.Add(this.label5);
		this.panel5.Controls.Add(this.txtremarks);
		this.panel5.Controls.Add(this.label38);
		this.panel5.Controls.Add(this.txtexpplace);
		this.panel5.Controls.Add(this.txtcustomerrefno);
		this.panel5.Controls.Add(this.txtinquiryref);
		this.panel5.Controls.Add(this.txtlocalcurramount);
		this.panel5.Controls.Add(this.txtnetweight);
		this.panel5.Controls.Add(this.txtgrossweight);
		this.panel5.Controls.Add(this.txtexchangerate);
		this.panel5.Controls.Add(this.txttotalfcamount);
		this.panel5.Controls.Add(this.txtcustomercode);
		this.panel5.Controls.Add(this.txtquorationref);
		this.panel5.Controls.Add(this.label9);
		this.panel5.Controls.Add(this.txtshippingmark);
		this.panel5.Controls.Add(this.txtinspectiondesc);
		this.panel5.Controls.Add(this.txtlegalizationdesc);
		this.panel5.Controls.Add(this.txtnoofcontainer);
		this.panel5.Controls.Add(this.label41);
		this.panel5.Controls.Add(this.label44);
		this.panel5.Controls.Add(this.txtcommoditydesc);
		this.panel5.Controls.Add(this.label39);
		this.panel5.Controls.Add(this.txtdocno);
		this.panel5.Controls.Add(this.label20);
		this.panel5.Controls.Add(this.label16);
		this.panel5.Controls.Add(this.label24);
		this.panel5.Controls.Add(this.label22);
		this.panel5.Controls.Add(this.label19);
		this.panel5.Controls.Add(this.label21);
		this.panel5.Controls.Add(this.label17);
		this.panel5.Controls.Add(this.label36);
		this.panel5.Controls.Add(this.label31);
		this.panel5.Controls.Add(this.label35);
		this.panel5.Controls.Add(this.label18);
		this.panel5.Controls.Add(this.label26);
		this.panel5.Controls.Add(this.label8);
		this.panel5.Controls.Add(this.label25);
		this.panel5.Controls.Add(this.label15);
		this.panel5.Controls.Add(this.label11);
		this.panel5.Controls.Add(this.label4);
		this.panel5.Controls.Add(this.label7);
		this.panel5.Controls.Add(this.label43);
		this.panel5.Controls.Add(this.label6);
		this.panel5.Controls.Add(this.label40);
		this.panel5.Controls.Add(this.label34);
		this.panel5.Controls.Add(this.label37);
		this.panel5.Controls.Add(this.label32);
		this.panel5.Controls.Add(this.label13);
		this.panel5.Controls.Add(this.label2);
		this.panel5.Controls.Add(this.label42);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.panel5.Location = new System.Drawing.Point(0, 66);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1155, 421);
		this.panel5.TabIndex = 0;
		this.cmbPerformaInvoice.AllowNull = (DefaultableBoolean)1;
		this.cmbPerformaInvoice.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPerformaInvoice.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPerformaInvoice.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance227).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance227).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPerformaInvoice).DisplayLayout.Appearance = (AppearanceBase)(object)appearance227;
		((UltraGridBase)this.cmbPerformaInvoice).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPerformaInvoice).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance228).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance228).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance228).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance228).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPerformaInvoice).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance228;
		((AppearanceBase)appearance229).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPerformaInvoice).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance229;
		((SpecialBoxBase)((UltraGridBase)this.cmbPerformaInvoice).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance230).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance230).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance230).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance230).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPerformaInvoice).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance230;
		((UltraGridBase)this.cmbPerformaInvoice).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPerformaInvoice).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance232).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance232).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPerformaInvoice).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance232;
		((AppearanceBase)appearance233).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance233).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPerformaInvoice).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance233;
		((UltraGridBase)this.cmbPerformaInvoice).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPerformaInvoice).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance234).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPerformaInvoice).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance234;
		((AppearanceBase)appearance235).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance235).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPerformaInvoice).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance235;
		((UltraGridBase)this.cmbPerformaInvoice).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPerformaInvoice).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance236).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance236).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance236).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance236).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance236).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPerformaInvoice).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance236;
		((AppearanceBase)appearance237).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPerformaInvoice).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance237;
		((UltraGridBase)this.cmbPerformaInvoice).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPerformaInvoice).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance238).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance238).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPerformaInvoice).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance238;
		((UltraGridBase)this.cmbPerformaInvoice).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance239).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPerformaInvoice).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance239;
		((UltraGridBase)this.cmbPerformaInvoice).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPerformaInvoice).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPerformaInvoice).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPerformaInvoice).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbPerformaInvoice.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPerformaInvoice).Location = new System.Drawing.Point(914, 90);
		((System.Windows.Forms.Control)(object)this.cmbPerformaInvoice).Name = "cmbPerformaInvoice";
		((System.Windows.Forms.Control)(object)this.cmbPerformaInvoice).Size = new System.Drawing.Size(153, 26);
		((System.Windows.Forms.Control)(object)this.cmbPerformaInvoice).TabIndex = 15;
		this.cmbTransShipment.AllowNull = (DefaultableBoolean)1;
		this.cmbTransShipment.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbTransShipment.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbTransShipment.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbTransShipment).DisplayLayout.Appearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbTransShipment).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbTransShipment).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance4).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance4).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance4).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbTransShipment).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbTransShipment).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance5;
		((SpecialBoxBase)((UltraGridBase)this.cmbTransShipment).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbTransShipment).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.cmbTransShipment).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbTransShipment).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbTransShipment).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance7;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbTransShipment).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.cmbTransShipment).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbTransShipment).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbTransShipment).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance10).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbTransShipment).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbTransShipment).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbTransShipment).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance11).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance11).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance11).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance11).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbTransShipment).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance12).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbTransShipment).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.cmbTransShipment).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbTransShipment).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbTransShipment).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.cmbTransShipment).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbTransShipment).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.cmbTransShipment).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbTransShipment).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbTransShipment).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbTransShipment).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbTransShipment.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbTransShipment).Location = new System.Drawing.Point(653, 91);
		((System.Windows.Forms.Control)(object)this.cmbTransShipment).Name = "cmbTransShipment";
		((System.Windows.Forms.Control)(object)this.cmbTransShipment).Size = new System.Drawing.Size(153, 26);
		((System.Windows.Forms.Control)(object)this.cmbTransShipment).TabIndex = 14;
		this.cmbPartialShipment.AllowNull = (DefaultableBoolean)1;
		this.cmbPartialShipment.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPartialShipment.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPartialShipment.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance212).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance212).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPartialShipment).DisplayLayout.Appearance = (AppearanceBase)(object)appearance212;
		((UltraGridBase)this.cmbPartialShipment).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPartialShipment).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance213).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance213).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance213).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance213).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPartialShipment).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance213;
		((AppearanceBase)appearance214).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPartialShipment).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance214;
		((SpecialBoxBase)((UltraGridBase)this.cmbPartialShipment).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance53).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPartialShipment).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.cmbPartialShipment).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPartialShipment).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPartialShipment).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance54;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPartialShipment).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.cmbPartialShipment).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPartialShipment).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPartialShipment).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance139;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance140).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPartialShipment).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.cmbPartialShipment).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPartialShipment).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance141).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance141).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance141).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance141).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPartialShipment).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance141;
		((AppearanceBase)appearance142).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPartialShipment).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.cmbPartialShipment).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPartialShipment).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPartialShipment).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.cmbPartialShipment).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPartialShipment).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.cmbPartialShipment).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPartialShipment).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPartialShipment).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPartialShipment).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbPartialShipment.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPartialShipment).Location = new System.Drawing.Point(386, 91);
		((System.Windows.Forms.Control)(object)this.cmbPartialShipment).Name = "cmbPartialShipment";
		((System.Windows.Forms.Control)(object)this.cmbPartialShipment).Size = new System.Drawing.Size(153, 26);
		((System.Windows.Forms.Control)(object)this.cmbPartialShipment).TabIndex = 13;
		this.label72.AutoSize = true;
		this.label72.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label72.ForeColor = System.Drawing.Color.Red;
		this.label72.Location = new System.Drawing.Point(858, 230);
		this.label72.Name = "label72";
		this.label72.Size = new System.Drawing.Size(13, 15);
		this.label72.TabIndex = 55;
		this.label72.Text = "*";
		this.label71.AutoSize = true;
		this.label71.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label71.ForeColor = System.Drawing.Color.Red;
		this.label71.Location = new System.Drawing.Point(894, 206);
		this.label71.Name = "label71";
		this.label71.Size = new System.Drawing.Size(13, 15);
		this.label71.TabIndex = 54;
		this.label71.Text = "*";
		this.label70.AutoSize = true;
		this.label70.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label70.ForeColor = System.Drawing.Color.Red;
		this.label70.Location = new System.Drawing.Point(640, 197);
		this.label70.Name = "label70";
		this.label70.Size = new System.Drawing.Size(13, 15);
		this.label70.TabIndex = 53;
		this.label70.Text = "*";
		this.label69.AutoSize = true;
		this.label69.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label69.ForeColor = System.Drawing.Color.Red;
		this.label69.Location = new System.Drawing.Point(892, 177);
		this.label69.Name = "label69";
		this.label69.Size = new System.Drawing.Size(13, 15);
		this.label69.TabIndex = 52;
		this.label69.Text = "*";
		this.label68.AutoSize = true;
		this.label68.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label68.ForeColor = System.Drawing.Color.Red;
		this.label68.Location = new System.Drawing.Point(630, 177);
		this.label68.Name = "label68";
		this.label68.Size = new System.Drawing.Size(13, 15);
		this.label68.TabIndex = 51;
		this.label68.Text = "*";
		this.label67.AutoSize = true;
		this.label67.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label67.ForeColor = System.Drawing.Color.Red;
		this.label67.Location = new System.Drawing.Point(374, 175);
		this.label67.Name = "label67";
		this.label67.Size = new System.Drawing.Size(13, 15);
		this.label67.TabIndex = 50;
		this.label67.Text = "*";
		this.label66.AutoSize = true;
		this.label66.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label66.ForeColor = System.Drawing.Color.Red;
		this.label66.Location = new System.Drawing.Point(372, 142);
		this.label66.Name = "label66";
		this.label66.Size = new System.Drawing.Size(13, 15);
		this.label66.TabIndex = 49;
		this.label66.Text = "*";
		this.label65.AutoSize = true;
		this.label65.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label65.ForeColor = System.Drawing.Color.Red;
		this.label65.Location = new System.Drawing.Point(93, 148);
		this.label65.Name = "label65";
		this.label65.Size = new System.Drawing.Size(13, 15);
		this.label65.TabIndex = 48;
		this.label65.Text = "*";
		this.label64.AutoSize = true;
		this.label64.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label64.ForeColor = System.Drawing.Color.Red;
		this.label64.Location = new System.Drawing.Point(894, 119);
		this.label64.Name = "label64";
		this.label64.Size = new System.Drawing.Size(13, 15);
		this.label64.TabIndex = 47;
		this.label64.Text = "*";
		this.label63.AutoSize = true;
		this.label63.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label63.ForeColor = System.Drawing.Color.Red;
		this.label63.Location = new System.Drawing.Point(630, 119);
		this.label63.Name = "label63";
		this.label63.Size = new System.Drawing.Size(13, 15);
		this.label63.TabIndex = 46;
		this.label63.Text = "*";
		this.label62.AutoSize = true;
		this.label62.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label62.ForeColor = System.Drawing.Color.Red;
		this.label62.Location = new System.Drawing.Point(369, 119);
		this.label62.Name = "label62";
		this.label62.Size = new System.Drawing.Size(13, 15);
		this.label62.TabIndex = 45;
		this.label62.Text = "*";
		this.label61.AutoSize = true;
		this.label61.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label61.ForeColor = System.Drawing.Color.Red;
		this.label61.Location = new System.Drawing.Point(93, 119);
		this.label61.Name = "label61";
		this.label61.Size = new System.Drawing.Size(13, 15);
		this.label61.TabIndex = 44;
		this.label61.Text = "*";
		this.label60.AutoSize = true;
		this.label60.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label60.ForeColor = System.Drawing.Color.Red;
		this.label60.Location = new System.Drawing.Point(611, 38);
		this.label60.Name = "label60";
		this.label60.Size = new System.Drawing.Size(13, 15);
		this.label60.TabIndex = 43;
		this.label60.Text = "*";
		this.label58.AutoSize = true;
		this.label58.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label58.ForeColor = System.Drawing.Color.Red;
		this.label58.Location = new System.Drawing.Point(624, 9);
		this.label58.Name = "label58";
		this.label58.Size = new System.Drawing.Size(13, 15);
		this.label58.TabIndex = 41;
		this.label58.Text = "*";
		this.label57.AutoSize = true;
		this.label57.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label57.ForeColor = System.Drawing.Color.Red;
		this.label57.Location = new System.Drawing.Point(336, 9);
		this.label57.Name = "label57";
		this.label57.Size = new System.Drawing.Size(13, 15);
		this.label57.TabIndex = 40;
		this.label57.Text = "*";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.Red;
		this.label1.Location = new System.Drawing.Point(60, 9);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(13, 15);
		this.label1.TabIndex = 39;
		this.label1.Text = "*";
		this.cmbstatus.AllowNull = (DefaultableBoolean)1;
		this.cmbstatus.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbstatus.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbstatus.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbstatus).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbstatus).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbstatus).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbstatus).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance176).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbstatus).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance176;
		((SpecialBoxBase)((UltraGridBase)this.cmbstatus).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance187).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance187).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance187).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance187).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbstatus).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance187;
		((UltraGridBase)this.cmbstatus).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbstatus).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance198).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance198).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbstatus).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance198;
		((AppearanceBase)appearance209).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance209).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbstatus).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance209;
		((UltraGridBase)this.cmbstatus).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbstatus).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance220).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbstatus).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance220;
		((AppearanceBase)appearance231).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance231).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbstatus).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance231;
		((UltraGridBase)this.cmbstatus).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbstatus).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance242).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance242).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance242).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance242).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance242).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbstatus).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance242;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbstatus).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbstatus).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbstatus).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbstatus).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbstatus).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbstatus).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbstatus).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbstatus).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbstatus).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbstatus).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbstatus.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbstatus).Location = new System.Drawing.Point(914, 230);
		((System.Windows.Forms.Control)(object)this.cmbstatus).Name = "cmbstatus";
		((System.Windows.Forms.Control)(object)this.cmbstatus).Size = new System.Drawing.Size(153, 26);
		((System.Windows.Forms.Control)(object)this.cmbstatus).TabIndex = 35;
		this.cmbinspection.AllowNull = (DefaultableBoolean)1;
		this.cmbinspection.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbinspection.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbinspection.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbinspection).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbinspection).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbinspection).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbinspection).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbinspection).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance57;
		((SpecialBoxBase)((UltraGridBase)this.cmbinspection).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbinspection).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.cmbinspection).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbinspection).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbinspection).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbinspection).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.cmbinspection).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbinspection).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbinspection).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance101;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance113).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbinspection).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.cmbinspection).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbinspection).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance124).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance124).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance124).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance124).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbinspection).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance124;
		((AppearanceBase)appearance135).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbinspection).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.cmbinspection).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbinspection).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbinspection).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.cmbinspection).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbinspection).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.cmbinspection).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbinspection).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbinspection).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbinspection).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbinspection.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbinspection).Location = new System.Drawing.Point(653, 148);
		((System.Windows.Forms.Control)(object)this.cmbinspection).Name = "cmbinspection";
		((System.Windows.Forms.Control)(object)this.cmbinspection).Size = new System.Drawing.Size(153, 26);
		((System.Windows.Forms.Control)(object)this.cmbinspection).TabIndex = 22;
		this.cmblegalization.AllowNull = (DefaultableBoolean)1;
		this.cmblegalization.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmblegalization.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmblegalization.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance168).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmblegalization).DisplayLayout.Appearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.cmblegalization).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmblegalization).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance172).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance172).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance172).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmblegalization).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance172;
		((AppearanceBase)appearance173).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmblegalization).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance173;
		((SpecialBoxBase)((UltraGridBase)this.cmblegalization).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance174).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance174).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance174).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance174).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmblegalization).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance174;
		((UltraGridBase)this.cmblegalization).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmblegalization).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance175).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmblegalization).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance175;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance177).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmblegalization).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance177;
		((UltraGridBase)this.cmblegalization).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmblegalization).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmblegalization).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance178;
		((AppearanceBase)appearance179).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance179).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmblegalization).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance179;
		((UltraGridBase)this.cmblegalization).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmblegalization).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance180).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance180).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance180).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance180).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance180).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmblegalization).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance180;
		((AppearanceBase)appearance181).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmblegalization).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance181;
		((UltraGridBase)this.cmblegalization).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmblegalization).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance182).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance182).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmblegalization).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance182;
		((UltraGridBase)this.cmblegalization).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance183).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmblegalization).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance183;
		((UltraGridBase)this.cmblegalization).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmblegalization).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmblegalization).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmblegalization).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmblegalization.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmblegalization).Location = new System.Drawing.Point(387, 230);
		((System.Windows.Forms.Control)(object)this.cmblegalization).Name = "cmblegalization";
		((System.Windows.Forms.Control)(object)this.cmblegalization).Size = new System.Drawing.Size(153, 26);
		((System.Windows.Forms.Control)(object)this.cmblegalization).TabIndex = 33;
		this.cmbfc.AllowNull = (DefaultableBoolean)1;
		this.cmbfc.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbfc.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbfc.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance184).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance184).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbfc).DisplayLayout.Appearance = (AppearanceBase)(object)appearance184;
		((UltraGridBase)this.cmbfc).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbfc).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance185).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance185).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance185).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance185).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbfc).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance185;
		((AppearanceBase)appearance186).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbfc).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance186;
		((SpecialBoxBase)((UltraGridBase)this.cmbfc).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance188).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance188).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance188).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance188).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbfc).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance188;
		((UltraGridBase)this.cmbfc).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbfc).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance189).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance189).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbfc).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance189;
		((AppearanceBase)appearance190).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance190).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbfc).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance190;
		((UltraGridBase)this.cmbfc).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbfc).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance191).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbfc).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance191;
		((AppearanceBase)appearance192).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance192).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbfc).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance192;
		((UltraGridBase)this.cmbfc).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbfc).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance193).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance193).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance193).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance193).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance193).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbfc).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance193;
		((AppearanceBase)appearance194).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbfc).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance194;
		((UltraGridBase)this.cmbfc).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbfc).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance195).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance195).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbfc).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance195;
		((UltraGridBase)this.cmbfc).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance196).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbfc).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance196;
		((UltraGridBase)this.cmbfc).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbfc).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbfc).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbfc).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbfc.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbfc).Location = new System.Drawing.Point(125, 177);
		((System.Windows.Forms.Control)(object)this.cmbfc).Name = "cmbfc";
		((System.Windows.Forms.Control)(object)this.cmbfc).Size = new System.Drawing.Size(153, 26);
		((System.Windows.Forms.Control)(object)this.cmbfc).TabIndex = 24;
		this.cmbdestinationport.AllowNull = (DefaultableBoolean)1;
		this.cmbdestinationport.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbdestinationport.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbdestinationport.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance197).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance197).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbdestinationport).DisplayLayout.Appearance = (AppearanceBase)(object)appearance197;
		((UltraGridBase)this.cmbdestinationport).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbdestinationport).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance199).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance199).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance199).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance199).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbdestinationport).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance199;
		((AppearanceBase)appearance200).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbdestinationport).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance200;
		((SpecialBoxBase)((UltraGridBase)this.cmbdestinationport).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance201).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance201).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance201).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance201).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbdestinationport).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance201;
		((UltraGridBase)this.cmbdestinationport).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbdestinationport).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance202).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance202).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbdestinationport).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance202;
		((AppearanceBase)appearance203).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance203).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbdestinationport).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance203;
		((UltraGridBase)this.cmbdestinationport).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbdestinationport).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance204).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbdestinationport).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance204;
		((AppearanceBase)appearance205).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance205).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbdestinationport).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance205;
		((UltraGridBase)this.cmbdestinationport).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbdestinationport).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance206).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance206).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance206).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance206).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance206).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbdestinationport).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance206;
		((AppearanceBase)appearance207).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbdestinationport).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance207;
		((UltraGridBase)this.cmbdestinationport).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbdestinationport).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance208).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance208).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbdestinationport).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance208;
		((UltraGridBase)this.cmbdestinationport).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance210).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbdestinationport).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance210;
		((UltraGridBase)this.cmbdestinationport).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbdestinationport).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbdestinationport).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbdestinationport).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbdestinationport.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbdestinationport).Location = new System.Drawing.Point(387, 148);
		((System.Windows.Forms.Control)(object)this.cmbdestinationport).Name = "cmbdestinationport";
		((System.Windows.Forms.Control)(object)this.cmbdestinationport).Size = new System.Drawing.Size(153, 26);
		((System.Windows.Forms.Control)(object)this.cmbdestinationport).TabIndex = 21;
		this.cmbloadingport.AllowNull = (DefaultableBoolean)1;
		this.cmbloadingport.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbloadingport.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbloadingport.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance211).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance211).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbloadingport).DisplayLayout.Appearance = (AppearanceBase)(object)appearance211;
		((UltraGridBase)this.cmbloadingport).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbloadingport).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance215).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance215).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance215).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance215).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbloadingport).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance215;
		((AppearanceBase)appearance216).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbloadingport).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance216;
		((SpecialBoxBase)((UltraGridBase)this.cmbloadingport).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance217).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance217).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance217).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance217).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbloadingport).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance217;
		((UltraGridBase)this.cmbloadingport).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbloadingport).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance218).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance218).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbloadingport).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance218;
		((AppearanceBase)appearance219).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance219).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbloadingport).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance219;
		((UltraGridBase)this.cmbloadingport).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbloadingport).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance221).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbloadingport).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance221;
		((AppearanceBase)appearance222).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance222).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbloadingport).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance222;
		((UltraGridBase)this.cmbloadingport).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbloadingport).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance223).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance223).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance223).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance223).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance223).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbloadingport).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance223;
		((AppearanceBase)appearance224).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbloadingport).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance224;
		((UltraGridBase)this.cmbloadingport).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbloadingport).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance225).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance225).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbloadingport).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance225;
		((UltraGridBase)this.cmbloadingport).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance226).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbloadingport).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance226;
		((UltraGridBase)this.cmbloadingport).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbloadingport).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbloadingport).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbloadingport).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbloadingport.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbloadingport).Location = new System.Drawing.Point(125, 148);
		((System.Windows.Forms.Control)(object)this.cmbloadingport).Name = "cmbloadingport";
		((System.Windows.Forms.Control)(object)this.cmbloadingport).Size = new System.Drawing.Size(153, 26);
		((System.Windows.Forms.Control)(object)this.cmbloadingport).TabIndex = 20;
		this.cmbExporterBanks.AllowNull = (DefaultableBoolean)1;
		this.cmbExporterBanks.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbExporterBanks.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbExporterBanks.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance159).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbExporterBanks).DisplayLayout.Appearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.cmbExporterBanks).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbExporterBanks).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance160).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance160).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance160).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbExporterBanks).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance160;
		((AppearanceBase)appearance161).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbExporterBanks).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance161;
		((SpecialBoxBase)((UltraGridBase)this.cmbExporterBanks).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance162).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance162).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance162).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbExporterBanks).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance162;
		((UltraGridBase)this.cmbExporterBanks).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbExporterBanks).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance163).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbExporterBanks).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance163;
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance164).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbExporterBanks).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.cmbExporterBanks).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbExporterBanks).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbExporterBanks).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance165;
		((AppearanceBase)appearance166).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance166).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbExporterBanks).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance166;
		((UltraGridBase)this.cmbExporterBanks).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbExporterBanks).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance167).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance167).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance167).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance167).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbExporterBanks).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance167;
		((AppearanceBase)appearance169).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbExporterBanks).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance169;
		((UltraGridBase)this.cmbExporterBanks).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbExporterBanks).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance170).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbExporterBanks).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance170;
		((UltraGridBase)this.cmbExporterBanks).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance171).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbExporterBanks).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance171;
		((UltraGridBase)this.cmbExporterBanks).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbExporterBanks).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbExporterBanks).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbExporterBanks).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbExporterBanks.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbExporterBanks).Location = new System.Drawing.Point(914, 119);
		((System.Windows.Forms.Control)(object)this.cmbExporterBanks).Name = "cmbExporterBanks";
		((System.Windows.Forms.Control)(object)this.cmbExporterBanks).Size = new System.Drawing.Size(153, 26);
		((System.Windows.Forms.Control)(object)this.cmbExporterBanks).TabIndex = 19;
		this.cmbimporterbank.AllowNull = (DefaultableBoolean)1;
		this.cmbimporterbank.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbimporterbank.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbimporterbank.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance240).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance240).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbimporterbank).DisplayLayout.Appearance = (AppearanceBase)(object)appearance240;
		((UltraGridBase)this.cmbimporterbank).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbimporterbank).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance241).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance241).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance241).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance241).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbimporterbank).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance241;
		((AppearanceBase)appearance243).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbimporterbank).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance243;
		((SpecialBoxBase)((UltraGridBase)this.cmbimporterbank).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance244).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance244).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance244).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance244).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbimporterbank).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance244;
		((UltraGridBase)this.cmbimporterbank).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbimporterbank).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance245).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance245).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbimporterbank).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance245;
		((AppearanceBase)appearance246).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance246).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbimporterbank).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance246;
		((UltraGridBase)this.cmbimporterbank).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbimporterbank).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance247).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbimporterbank).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance247;
		((AppearanceBase)appearance248).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance248).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbimporterbank).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance248;
		((UltraGridBase)this.cmbimporterbank).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbimporterbank).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance249).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance249).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance249).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance249).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance249).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbimporterbank).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance249;
		((AppearanceBase)appearance250).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbimporterbank).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance250;
		((UltraGridBase)this.cmbimporterbank).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbimporterbank).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance251).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance251).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbimporterbank).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance251;
		((UltraGridBase)this.cmbimporterbank).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance252).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbimporterbank).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance252;
		((UltraGridBase)this.cmbimporterbank).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbimporterbank).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbimporterbank).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbimporterbank).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbimporterbank.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbimporterbank).Location = new System.Drawing.Point(653, 119);
		((System.Windows.Forms.Control)(object)this.cmbimporterbank).Name = "cmbimporterbank";
		((System.Windows.Forms.Control)(object)this.cmbimporterbank).Size = new System.Drawing.Size(153, 26);
		((System.Windows.Forms.Control)(object)this.cmbimporterbank).TabIndex = 18;
		this.cmbpaymenterm.AllowNull = (DefaultableBoolean)1;
		this.cmbpaymenterm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbpaymenterm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbpaymenterm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance145).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbpaymenterm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.cmbpaymenterm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbpaymenterm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance147).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance147).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance147).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbpaymenterm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance147;
		((AppearanceBase)appearance148).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbpaymenterm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance148;
		((SpecialBoxBase)((UltraGridBase)this.cmbpaymenterm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance149).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance149).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance149).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbpaymenterm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance149;
		((UltraGridBase)this.cmbpaymenterm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbpaymenterm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance150).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbpaymenterm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance150;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance151).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbpaymenterm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.cmbpaymenterm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbpaymenterm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbpaymenterm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance152;
		((AppearanceBase)appearance153).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance153).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbpaymenterm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance153;
		((UltraGridBase)this.cmbpaymenterm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbpaymenterm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance154).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance154).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance154).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance154).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbpaymenterm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance154;
		((AppearanceBase)appearance155).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbpaymenterm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.cmbpaymenterm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbpaymenterm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance156).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbpaymenterm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance156;
		((UltraGridBase)this.cmbpaymenterm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbpaymenterm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance158;
		((UltraGridBase)this.cmbpaymenterm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbpaymenterm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbpaymenterm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbpaymenterm).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbpaymenterm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbpaymenterm).Location = new System.Drawing.Point(387, 119);
		((System.Windows.Forms.Control)(object)this.cmbpaymenterm).Name = "cmbpaymenterm";
		((System.Windows.Forms.Control)(object)this.cmbpaymenterm).Size = new System.Drawing.Size(153, 26);
		((System.Windows.Forms.Control)(object)this.cmbpaymenterm).TabIndex = 17;
		this.cmbdeliveryterm.AllowNull = (DefaultableBoolean)1;
		this.cmbdeliveryterm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbdeliveryterm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbdeliveryterm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance17).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance17).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance18;
		((SpecialBoxBase)((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance23).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance25).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance25).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance25).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbdeliveryterm).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbdeliveryterm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbdeliveryterm).Location = new System.Drawing.Point(125, 119);
		((System.Windows.Forms.Control)(object)this.cmbdeliveryterm).Name = "cmbdeliveryterm";
		((System.Windows.Forms.Control)(object)this.cmbdeliveryterm).Size = new System.Drawing.Size(153, 26);
		((System.Windows.Forms.Control)(object)this.cmbdeliveryterm).TabIndex = 16;
		this.cmbcustomer.AllowNull = (DefaultableBoolean)1;
		this.cmbcustomer.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbcustomer.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbcustomer.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.Appearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance30).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance30).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcustomer).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance31;
		((SpecialBoxBase)((UltraGridBase)this.cmbcustomer).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance37).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance38).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance38).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance38).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcustomer).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcustomer).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcustomer).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbcustomer.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbcustomer).Location = new System.Drawing.Point(653, 36);
		((System.Windows.Forms.Control)(object)this.cmbcustomer).Name = "cmbcustomer";
		((System.Windows.Forms.Control)(object)this.cmbcustomer).Size = new System.Drawing.Size(153, 26);
		((System.Windows.Forms.Control)(object)this.cmbcustomer).TabIndex = 6;
		((System.Windows.Forms.Control)(object)this.cmbcustomer).Leave += new System.EventHandler(cmbcustomer_Leave);
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
		((System.Windows.Forms.Control)(object)this.cmbproject).Location = new System.Drawing.Point(387, 9);
		((System.Windows.Forms.Control)(object)this.cmbproject).Name = "cmbproject";
		((System.Windows.Forms.Control)(object)this.cmbproject).Size = new System.Drawing.Size(153, 26);
		((System.Windows.Forms.Control)(object)this.cmbproject).TabIndex = 1;
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
		((System.Windows.Forms.Control)(object)this.cmbbranch).Location = new System.Drawing.Point(125, 9);
		((System.Windows.Forms.Control)(object)this.cmbbranch).Name = "cmbbranch";
		((System.Windows.Forms.Control)(object)this.cmbbranch).Size = new System.Drawing.Size(153, 26);
		((System.Windows.Forms.Control)(object)this.cmbbranch).TabIndex = 0;
		this.groupBox1.Controls.Add(this.label80);
		this.groupBox1.Controls.Add(this.label79);
		this.groupBox1.Controls.Add(this.label78);
		this.groupBox1.Controls.Add(this.label77);
		this.groupBox1.Controls.Add(this.label76);
		this.groupBox1.Controls.Add(this.label75);
		this.groupBox1.Controls.Add(this.label74);
		this.groupBox1.Controls.Add(this.label73);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbrateuom);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbouterpackuom);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbinnerpackuom);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbinnerpacktype);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbitem);
		this.groupBox1.Controls.Add(this.txtOuterQty);
		this.groupBox1.Controls.Add(this.txtinnerQty);
		this.groupBox1.Controls.Add(this.btnCancel);
		this.groupBox1.Controls.Add(this.btnGridUpdate);
		this.groupBox1.Controls.Add(this.btnaddnew);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.label45);
		this.groupBox1.Controls.Add(this.label47);
		this.groupBox1.Controls.Add(this.label48);
		this.groupBox1.Controls.Add(this.label49);
		this.groupBox1.Controls.Add(this.label50);
		this.groupBox1.Controls.Add(this.label51);
		this.groupBox1.Controls.Add(this.txtamount);
		this.groupBox1.Controls.Add(this.label52);
		this.groupBox1.Controls.Add(this.txtrate);
		this.groupBox1.Controls.Add(this.label53);
		this.groupBox1.Controls.Add(this.txtweight);
		this.groupBox1.Controls.Add(this.label54);
		this.groupBox1.Controls.Add(this.txtdescription);
		this.groupBox1.Controls.Add(this.label55);
		this.groupBox1.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(7, 288);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1144, 129);
		this.groupBox1.TabIndex = 38;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Detail";
		this.label80.AutoSize = true;
		this.label80.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label80.ForeColor = System.Drawing.Color.Red;
		this.label80.Location = new System.Drawing.Point(538, 65);
		this.label80.Name = "label80";
		this.label80.Size = new System.Drawing.Size(13, 15);
		this.label80.TabIndex = 63;
		this.label80.Text = "*";
		this.label79.AutoSize = true;
		this.label79.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label79.ForeColor = System.Drawing.Color.Red;
		this.label79.Location = new System.Drawing.Point(391, 70);
		this.label79.Name = "label79";
		this.label79.Size = new System.Drawing.Size(13, 15);
		this.label79.TabIndex = 62;
		this.label79.Text = "*";
		this.label78.AutoSize = true;
		this.label78.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label78.ForeColor = System.Drawing.Color.Red;
		this.label78.Location = new System.Drawing.Point(207, 71);
		this.label78.Name = "label78";
		this.label78.Size = new System.Drawing.Size(13, 15);
		this.label78.TabIndex = 61;
		this.label78.Text = "*";
		this.label77.AutoSize = true;
		this.label77.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label77.ForeColor = System.Drawing.Color.Red;
		this.label77.Location = new System.Drawing.Point(67, 70);
		this.label77.Name = "label77";
		this.label77.Size = new System.Drawing.Size(13, 15);
		this.label77.TabIndex = 60;
		this.label77.Text = "*";
		this.label76.AutoSize = true;
		this.label76.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label76.ForeColor = System.Drawing.Color.Red;
		this.label76.Location = new System.Drawing.Point(705, 18);
		this.label76.Name = "label76";
		this.label76.Size = new System.Drawing.Size(13, 15);
		this.label76.TabIndex = 59;
		this.label76.Text = "*";
		this.label75.AutoSize = true;
		this.label75.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label75.ForeColor = System.Drawing.Color.Red;
		this.label75.Location = new System.Drawing.Point(896, 18);
		this.label75.Name = "label75";
		this.label75.Size = new System.Drawing.Size(13, 15);
		this.label75.TabIndex = 58;
		this.label75.Text = "*";
		this.label74.AutoSize = true;
		this.label74.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label74.ForeColor = System.Drawing.Color.Red;
		this.label74.Location = new System.Drawing.Point(236, 18);
		this.label74.Name = "label74";
		this.label74.Size = new System.Drawing.Size(13, 15);
		this.label74.TabIndex = 57;
		this.label74.Text = "*";
		this.label73.AutoSize = true;
		this.label73.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label73.ForeColor = System.Drawing.Color.Red;
		this.label73.Location = new System.Drawing.Point(53, 18);
		this.label73.Name = "label73";
		this.label73.Size = new System.Drawing.Size(13, 15);
		this.label73.TabIndex = 56;
		this.label73.Text = "*";
		this.cmbrateuom.AllowNull = (DefaultableBoolean)1;
		this.cmbrateuom.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbrateuom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbrateuom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbrateuom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.cmbrateuom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbrateuom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance73).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance73).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbrateuom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance73;
		((AppearanceBase)appearance74).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbrateuom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance74;
		((SpecialBoxBase)((UltraGridBase)this.cmbrateuom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance75).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance75).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance75).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbrateuom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.cmbrateuom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbrateuom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbrateuom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance76;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance77).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbrateuom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.cmbrateuom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbrateuom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbrateuom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance78;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance80).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbrateuom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.cmbrateuom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbrateuom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance81).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance81).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance81).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbrateuom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance81;
		((AppearanceBase)appearance82).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbrateuom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.cmbrateuom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbrateuom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbrateuom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.cmbrateuom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbrateuom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.cmbrateuom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbrateuom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbrateuom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbrateuom).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbrateuom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbrateuom).Location = new System.Drawing.Point(326, 89);
		((System.Windows.Forms.Control)(object)this.cmbrateuom).Name = "cmbrateuom";
		((System.Windows.Forms.Control)(object)this.cmbrateuom).Size = new System.Drawing.Size(153, 26);
		((System.Windows.Forms.Control)(object)this.cmbrateuom).TabIndex = 8;
		((System.Windows.Forms.Control)(object)this.cmbrateuom).TextChanged += new System.EventHandler(cmbrateuom_TextChanged);
		this.cmbouterpackuom.AllowNull = (DefaultableBoolean)1;
		this.cmbouterpackuom.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbouterpackuom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbouterpackuom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance85).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbouterpackuom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.cmbouterpackuom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbouterpackuom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance86).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance86).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbouterpackuom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance86;
		((AppearanceBase)appearance87).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbouterpackuom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance87;
		((SpecialBoxBase)((UltraGridBase)this.cmbouterpackuom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance88).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance88).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance88).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbouterpackuom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.cmbouterpackuom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbouterpackuom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance89).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbouterpackuom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance89;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance91).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbouterpackuom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.cmbouterpackuom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbouterpackuom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbouterpackuom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance92;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance93).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbouterpackuom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance93;
		((UltraGridBase)this.cmbouterpackuom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbouterpackuom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance94).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance94).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance94).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance94).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbouterpackuom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance94;
		((AppearanceBase)appearance95).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbouterpackuom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.cmbouterpackuom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbouterpackuom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance96).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbouterpackuom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.cmbouterpackuom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbouterpackuom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.cmbouterpackuom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbouterpackuom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbouterpackuom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbouterpackuom).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbouterpackuom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbouterpackuom).Location = new System.Drawing.Point(795, 36);
		((System.Windows.Forms.Control)(object)this.cmbouterpackuom).Name = "cmbouterpackuom";
		((System.Windows.Forms.Control)(object)this.cmbouterpackuom).Size = new System.Drawing.Size(153, 26);
		((System.Windows.Forms.Control)(object)this.cmbouterpackuom).TabIndex = 5;
		((System.Windows.Forms.Control)(object)this.cmbouterpackuom).TextChanged += new System.EventHandler(cmbouterpackuom_TextChanged);
		this.cmbinnerpackuom.AllowNull = (DefaultableBoolean)1;
		this.cmbinnerpackuom.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbinnerpackuom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbinnerpackuom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance98).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbinnerpackuom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance98;
		((UltraGridBase)this.cmbinnerpackuom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbinnerpackuom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance99).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance99).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbinnerpackuom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance99;
		((AppearanceBase)appearance100).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbinnerpackuom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance100;
		((SpecialBoxBase)((UltraGridBase)this.cmbinnerpackuom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance102).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance102).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance102).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbinnerpackuom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.cmbinnerpackuom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbinnerpackuom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance103).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbinnerpackuom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance103;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance104).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbinnerpackuom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.cmbinnerpackuom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbinnerpackuom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbinnerpackuom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance105;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance106).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbinnerpackuom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.cmbinnerpackuom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbinnerpackuom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance107).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance107).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance107).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance107).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbinnerpackuom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance107;
		((AppearanceBase)appearance108).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbinnerpackuom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.cmbinnerpackuom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbinnerpackuom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance109).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbinnerpackuom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.cmbinnerpackuom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbinnerpackuom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.cmbinnerpackuom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbinnerpackuom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbinnerpackuom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbinnerpackuom).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbinnerpackuom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbinnerpackuom).Location = new System.Drawing.Point(483, 36);
		((System.Windows.Forms.Control)(object)this.cmbinnerpackuom).Name = "cmbinnerpackuom";
		((System.Windows.Forms.Control)(object)this.cmbinnerpackuom).Size = new System.Drawing.Size(153, 26);
		((System.Windows.Forms.Control)(object)this.cmbinnerpackuom).TabIndex = 3;
		this.cmbinnerpacktype.AllowNull = (DefaultableBoolean)1;
		this.cmbinnerpacktype.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbinnerpacktype.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbinnerpacktype.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance111).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbinnerpacktype).DisplayLayout.Appearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.cmbinnerpacktype).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbinnerpacktype).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance114).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance114).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance114).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbinnerpacktype).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance114;
		((AppearanceBase)appearance115).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbinnerpacktype).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance115;
		((SpecialBoxBase)((UltraGridBase)this.cmbinnerpacktype).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance116).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance116).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance116).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbinnerpacktype).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.cmbinnerpacktype).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbinnerpacktype).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance117).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbinnerpacktype).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance117;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance118).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbinnerpacktype).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.cmbinnerpacktype).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbinnerpacktype).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbinnerpacktype).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance119;
		((AppearanceBase)appearance120).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance120).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbinnerpacktype).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.cmbinnerpacktype).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbinnerpacktype).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance121).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance121).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance121).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance121).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbinnerpacktype).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance121;
		((AppearanceBase)appearance122).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbinnerpacktype).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.cmbinnerpacktype).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbinnerpacktype).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance123).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbinnerpacktype).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.cmbinnerpacktype).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbinnerpacktype).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.cmbinnerpacktype).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbinnerpacktype).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbinnerpacktype).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbinnerpacktype).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbinnerpacktype.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbinnerpacktype).Location = new System.Drawing.Point(170, 36);
		((System.Windows.Forms.Control)(object)this.cmbinnerpacktype).Name = "cmbinnerpacktype";
		((System.Windows.Forms.Control)(object)this.cmbinnerpacktype).Size = new System.Drawing.Size(153, 26);
		((System.Windows.Forms.Control)(object)this.cmbinnerpacktype).TabIndex = 1;
		this.cmbitem.AllowNull = (DefaultableBoolean)1;
		this.cmbitem.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbitem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbitem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance126).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbitem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.cmbitem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbitem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance127).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance127).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbitem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance127;
		((AppearanceBase)appearance128).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbitem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance128;
		((SpecialBoxBase)((UltraGridBase)this.cmbitem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance129).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance129).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance129).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbitem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.cmbitem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbitem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance130).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance130;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance131).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance132;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance133).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance134).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance134).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance134).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance134;
		((AppearanceBase)appearance136).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance137).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance137;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.cmbitem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbitem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbitem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbitem).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbitem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbitem).Location = new System.Drawing.Point(15, 36);
		((System.Windows.Forms.Control)(object)this.cmbitem).Name = "cmbitem";
		((System.Windows.Forms.Control)(object)this.cmbitem).Size = new System.Drawing.Size(153, 26);
		((System.Windows.Forms.Control)(object)this.cmbitem).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbitem).Leave += new System.EventHandler(cmbitem_Leave);
		this.txtOuterQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOuterQty.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOuterQty.Location = new System.Drawing.Point(639, 36);
		this.txtOuterQty.Name = "txtOuterQty";
		this.txtOuterQty.Size = new System.Drawing.Size(153, 23);
		this.txtOuterQty.TabIndex = 4;
		this.txtOuterQty.TextChanged += new System.EventHandler(txtOuterQty_TextChanged);
		this.txtOuterQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtOuterQty_KeyPress);
		this.txtinnerQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtinnerQty.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtinnerQty.Location = new System.Drawing.Point(326, 36);
		this.txtinnerQty.Name = "txtinnerQty";
		this.txtinnerQty.Size = new System.Drawing.Size(153, 23);
		this.txtinnerQty.TabIndex = 2;
		this.txtinnerQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtinnerQty_KeyPress);
		this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.Green;
		this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancel.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancel.Location = new System.Drawing.Point(1074, 89);
		this.btnCancel.Name = "btnCancel";
		this.btnCancel.Size = new System.Drawing.Size(66, 25);
		this.btnCancel.TabIndex = 13;
		this.btnCancel.Text = "Cancel";
		this.btnCancel.UseVisualStyleBackColor = true;
		this.btnCancel.Visible = false;
		this.btnCancel.Click += new System.EventHandler(btnCancel_Click);
		this.btnGridUpdate.FlatAppearance.BorderColor = System.Drawing.Color.Green;
		this.btnGridUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnGridUpdate.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnGridUpdate.Location = new System.Drawing.Point(1002, 89);
		this.btnGridUpdate.Name = "btnGridUpdate";
		this.btnGridUpdate.Size = new System.Drawing.Size(66, 25);
		this.btnGridUpdate.TabIndex = 12;
		this.btnGridUpdate.Text = "Update";
		this.btnGridUpdate.UseVisualStyleBackColor = true;
		this.btnGridUpdate.Visible = false;
		this.btnGridUpdate.Click += new System.EventHandler(btnGridUpdate_Click);
		this.btnaddnew.FlatAppearance.BorderColor = System.Drawing.Color.Green;
		this.btnaddnew.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnaddnew.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnaddnew.Location = new System.Drawing.Point(952, 89);
		this.btnaddnew.Name = "btnaddnew";
		this.btnaddnew.Size = new System.Drawing.Size(44, 25);
		this.btnaddnew.TabIndex = 11;
		this.btnaddnew.Text = "Add";
		this.btnaddnew.UseVisualStyleBackColor = true;
		this.btnaddnew.Click += new System.EventHandler(btnaddnew_Click);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(795, 18);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(95, 15);
		this.label3.TabIndex = 45;
		this.label3.Text = "Outer Pack Uom";
		this.label45.AutoSize = true;
		this.label45.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label45.Location = new System.Drawing.Point(639, 18);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(60, 15);
		this.label45.TabIndex = 44;
		this.label45.Text = "Outer Qty";
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label47.Location = new System.Drawing.Point(483, 18);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(93, 15);
		this.label47.TabIndex = 43;
		this.label47.Text = "Inner Pack Uom";
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.Location = new System.Drawing.Point(326, 70);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(59, 15);
		this.label48.TabIndex = 41;
		this.label48.Text = "Rate Uom";
		this.label49.AutoSize = true;
		this.label49.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label49.Location = new System.Drawing.Point(326, 18);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(58, 15);
		this.label49.TabIndex = 46;
		this.label49.Text = "Inner Qty";
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label50.Location = new System.Drawing.Point(170, 18);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(60, 15);
		this.label50.TabIndex = 40;
		this.label50.Text = "Pack Type";
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label51.Location = new System.Drawing.Point(15, 18);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(37, 15);
		this.label51.TabIndex = 42;
		this.label51.Text = "Items";
		this.txtamount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtamount.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtamount.Location = new System.Drawing.Point(483, 89);
		this.txtamount.Name = "txtamount";
		this.txtamount.ReadOnly = true;
		this.txtamount.Size = new System.Drawing.Size(153, 23);
		this.txtamount.TabIndex = 9;
		this.label52.AutoSize = true;
		this.label52.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label52.Location = new System.Drawing.Point(483, 70);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(49, 15);
		this.label52.TabIndex = 36;
		this.label52.Text = "Amount";
		this.txtrate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtrate.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtrate.Location = new System.Drawing.Point(170, 89);
		this.txtrate.Name = "txtrate";
		this.txtrate.Size = new System.Drawing.Size(153, 23);
		this.txtrate.TabIndex = 7;
		this.txtrate.TextChanged += new System.EventHandler(txtrate_TextChanged);
		this.txtrate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtrate_KeyPress);
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label53.Location = new System.Drawing.Point(170, 70);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(31, 15);
		this.label53.TabIndex = 37;
		this.label53.Text = "Rate";
		this.txtweight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtweight.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtweight.Location = new System.Drawing.Point(15, 89);
		this.txtweight.Name = "txtweight";
		this.txtweight.ReadOnly = true;
		this.txtweight.Size = new System.Drawing.Size(153, 23);
		this.txtweight.TabIndex = 6;
		this.txtweight.TextChanged += new System.EventHandler(txtweight_TextChanged);
		this.txtweight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtweight_KeyPress);
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label54.Location = new System.Drawing.Point(15, 70);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(46, 15);
		this.label54.TabIndex = 38;
		this.label54.Text = "Weight";
		this.txtdescription.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdescription.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdescription.Location = new System.Drawing.Point(639, 89);
		this.txtdescription.Name = "txtdescription";
		this.txtdescription.Size = new System.Drawing.Size(309, 23);
		this.txtdescription.TabIndex = 10;
		this.label55.AutoSize = true;
		this.label55.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label55.Location = new System.Drawing.Point(639, 70);
		this.label55.Name = "label55";
		this.label55.Size = new System.Drawing.Size(70, 15);
		this.label55.TabIndex = 39;
		this.label55.Text = "Description";
		this.txtexpirydate.CustomFormat = "dd-MMM-yyyy";
		this.txtexpirydate.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtexpirydate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtexpirydate.Location = new System.Drawing.Point(653, 65);
		this.txtexpirydate.Name = "txtexpirydate";
		this.txtexpirydate.Size = new System.Drawing.Size(153, 23);
		this.txtexpirydate.TabIndex = 10;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(546, 69);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(69, 15);
		this.label12.TabIndex = 29;
		this.label12.Text = "Expiry Date";
		this.txtlastshipmentdate.CustomFormat = "dd-MMM-yyyy";
		this.txtlastshipmentdate.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtlastshipmentdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtlastshipmentdate.Location = new System.Drawing.Point(125, 91);
		this.txtlastshipmentdate.Name = "txtlastshipmentdate";
		this.txtlastshipmentdate.Size = new System.Drawing.Size(153, 23);
		this.txtlastshipmentdate.TabIndex = 12;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(8, 95);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(111, 15);
		this.label14.TabIndex = 29;
		this.label14.Text = "Last Shipment Date";
		this.txtreferencedate.CustomFormat = "dd-MMM-yyyy";
		this.txtreferencedate.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtreferencedate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtreferencedate.Location = new System.Drawing.Point(125, 65);
		this.txtreferencedate.Name = "txtreferencedate";
		this.txtreferencedate.Size = new System.Drawing.Size(153, 23);
		this.txtreferencedate.TabIndex = 8;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(8, 69);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(88, 15);
		this.label10.TabIndex = 29;
		this.label10.Text = "Reference Date";
		this.txtdocdate.CustomFormat = "dd-MMM-yyyy";
		this.txtdocdate.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtdocdate.Location = new System.Drawing.Point(914, 11);
		this.txtdocdate.Name = "txtdocdate";
		this.txtdocdate.Size = new System.Drawing.Size(153, 23);
		this.txtdocdate.TabIndex = 3;
		this.label46.AutoSize = true;
		this.label46.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label46.Location = new System.Drawing.Point(811, 15);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(90, 15);
		this.label46.TabIndex = 29;
		this.label46.Text = "Document Date";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(284, 15);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(46, 15);
		this.label5.TabIndex = 23;
		this.label5.Text = "Project";
		this.txtremarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtremarks.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtremarks.Location = new System.Drawing.Point(125, 258);
		this.txtremarks.Name = "txtremarks";
		this.txtremarks.Size = new System.Drawing.Size(942, 23);
		this.txtremarks.TabIndex = 36;
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.Location = new System.Drawing.Point(8, 262);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(54, 15);
		this.label38.TabIndex = 22;
		this.label38.Text = "Remarks";
		this.txtexpplace.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtexpplace.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtexpplace.Location = new System.Drawing.Point(914, 65);
		this.txtexpplace.Name = "txtexpplace";
		this.txtexpplace.Size = new System.Drawing.Size(153, 23);
		this.txtexpplace.TabIndex = 11;
		this.txtcustomerrefno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcustomerrefno.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcustomerrefno.Location = new System.Drawing.Point(914, 38);
		this.txtcustomerrefno.Name = "txtcustomerrefno";
		this.txtcustomerrefno.Size = new System.Drawing.Size(153, 23);
		this.txtcustomerrefno.TabIndex = 7;
		this.txtinquiryref.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtinquiryref.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtinquiryref.Location = new System.Drawing.Point(387, 38);
		this.txtinquiryref.Name = "txtinquiryref";
		this.txtinquiryref.Size = new System.Drawing.Size(153, 23);
		this.txtinquiryref.TabIndex = 5;
		this.txtlocalcurramount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtlocalcurramount.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtlocalcurramount.Location = new System.Drawing.Point(914, 179);
		this.txtlocalcurramount.Name = "txtlocalcurramount";
		this.txtlocalcurramount.ReadOnly = true;
		this.txtlocalcurramount.Size = new System.Drawing.Size(153, 23);
		this.txtlocalcurramount.TabIndex = 27;
		this.txtnetweight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtnetweight.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtnetweight.Location = new System.Drawing.Point(914, 206);
		this.txtnetweight.Name = "txtnetweight";
		this.txtnetweight.ReadOnly = true;
		this.txtnetweight.Size = new System.Drawing.Size(153, 23);
		this.txtnetweight.TabIndex = 31;
		this.txtgrossweight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtgrossweight.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtgrossweight.Location = new System.Drawing.Point(653, 206);
		this.txtgrossweight.Name = "txtgrossweight";
		this.txtgrossweight.Size = new System.Drawing.Size(153, 23);
		this.txtgrossweight.TabIndex = 30;
		this.txtgrossweight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtgrossweight_KeyPress);
		this.txtexchangerate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtexchangerate.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtexchangerate.Location = new System.Drawing.Point(653, 179);
		this.txtexchangerate.Name = "txtexchangerate";
		this.txtexchangerate.Size = new System.Drawing.Size(153, 23);
		this.txtexchangerate.TabIndex = 26;
		this.txtexchangerate.TextChanged += new System.EventHandler(txtexchangerate_TextChanged);
		this.txtexchangerate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtexchangerate_KeyPress);
		this.txttotalfcamount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txttotalfcamount.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txttotalfcamount.Location = new System.Drawing.Point(387, 179);
		this.txttotalfcamount.Name = "txttotalfcamount";
		this.txttotalfcamount.Size = new System.Drawing.Size(153, 23);
		this.txttotalfcamount.TabIndex = 25;
		this.txttotalfcamount.TextChanged += new System.EventHandler(txttotalfcamount_TextChanged);
		this.txttotalfcamount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txttotalfcamount_KeyPress);
		this.txtcustomercode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcustomercode.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcustomercode.Location = new System.Drawing.Point(387, 65);
		this.txtcustomercode.Name = "txtcustomercode";
		this.txtcustomercode.Size = new System.Drawing.Size(153, 23);
		this.txtcustomercode.TabIndex = 9;
		this.txtquorationref.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtquorationref.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtquorationref.Location = new System.Drawing.Point(125, 38);
		this.txtquorationref.Name = "txtquorationref";
		this.txtquorationref.Size = new System.Drawing.Size(153, 23);
		this.txtquorationref.TabIndex = 4;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(811, 42);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(97, 15);
		this.label9.TabIndex = 22;
		this.label9.Text = "Customer Ref No";
		this.txtshippingmark.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtshippingmark.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtshippingmark.Location = new System.Drawing.Point(387, 206);
		this.txtshippingmark.Name = "txtshippingmark";
		this.txtshippingmark.Size = new System.Drawing.Size(153, 23);
		this.txtshippingmark.TabIndex = 29;
		this.txtinspectiondesc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtinspectiondesc.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtinspectiondesc.Location = new System.Drawing.Point(914, 150);
		this.txtinspectiondesc.Name = "txtinspectiondesc";
		this.txtinspectiondesc.Size = new System.Drawing.Size(153, 23);
		this.txtinspectiondesc.TabIndex = 23;
		this.txtlegalizationdesc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtlegalizationdesc.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtlegalizationdesc.Location = new System.Drawing.Point(653, 232);
		this.txtlegalizationdesc.Name = "txtlegalizationdesc";
		this.txtlegalizationdesc.Size = new System.Drawing.Size(153, 23);
		this.txtlegalizationdesc.TabIndex = 34;
		this.txtnoofcontainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtnoofcontainer.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtnoofcontainer.Location = new System.Drawing.Point(125, 232);
		this.txtnoofcontainer.Name = "txtnoofcontainer";
		this.txtnoofcontainer.Size = new System.Drawing.Size(153, 23);
		this.txtnoofcontainer.TabIndex = 32;
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label41.Location = new System.Drawing.Point(546, 154);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(64, 15);
		this.label41.TabIndex = 23;
		this.label41.Text = "Inspection";
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label44.Location = new System.Drawing.Point(811, 236);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(41, 15);
		this.label44.TabIndex = 23;
		this.label44.Text = "Status";
		this.txtcommoditydesc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcommoditydesc.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcommoditydesc.Location = new System.Drawing.Point(125, 206);
		this.txtcommoditydesc.Name = "txtcommoditydesc";
		this.txtcommoditydesc.Size = new System.Drawing.Size(153, 23);
		this.txtcommoditydesc.TabIndex = 28;
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label39.Location = new System.Drawing.Point(284, 236);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(72, 15);
		this.label39.TabIndex = 23;
		this.label39.Text = "Legalization";
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocno.Location = new System.Drawing.Point(653, 11);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.Size = new System.Drawing.Size(153, 23);
		this.txtdocno.TabIndex = 2;
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(811, 125);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(83, 15);
		this.label20.TabIndex = 23;
		this.label20.Text = "Exporter Bank";
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(546, 95);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(91, 15);
		this.label16.TabIndex = 23;
		this.label16.Text = "Trans Shipment";
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(8, 183);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(55, 15);
		this.label24.TabIndex = 23;
		this.label24.Text = "Fcy Code";
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(8, 154);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(76, 15);
		this.label22.TabIndex = 23;
		this.label22.Text = "Loading Port";
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(546, 125);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(85, 15);
		this.label19.TabIndex = 23;
		this.label19.Text = "Importer Bank";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(284, 154);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(95, 15);
		this.label21.TabIndex = 23;
		this.label21.Text = "Destination Port";
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(8, 125);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(81, 15);
		this.label17.TabIndex = 23;
		this.label17.Text = "Delivery Term";
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(811, 210);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(87, 15);
		this.label36.TabIndex = 22;
		this.label36.Text = "Net Weight(kg)";
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.Location = new System.Drawing.Point(811, 183);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(81, 15);
		this.label31.TabIndex = 22;
		this.label31.Text = "Local Amount";
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(546, 210);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(101, 15);
		this.label35.TabIndex = 22;
		this.label35.Text = "Gross Weight(kg)";
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(284, 125);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(83, 15);
		this.label18.TabIndex = 23;
		this.label18.Text = "Payment Term";
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(546, 183);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(85, 15);
		this.label26.TabIndex = 22;
		this.label26.Text = "Exchange Rate";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(284, 42);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(67, 15);
		this.label8.TabIndex = 22;
		this.label8.Text = "Inquiry Ref";
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(284, 183);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(94, 15);
		this.label25.TabIndex = 22;
		this.label25.Text = "Total Fc Amount";
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(284, 95);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(98, 15);
		this.label15.TabIndex = 23;
		this.label15.Text = "Partial Shipment";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(284, 69);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(89, 15);
		this.label11.TabIndex = 22;
		this.label11.Text = "Customer Code";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(8, 15);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(46, 15);
		this.label4.TabIndex = 23;
		this.label4.Text = "Branch";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(8, 42);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(82, 15);
		this.label7.TabIndex = 22;
		this.label7.Text = "Quotation Ref";
		this.label43.AutoSize = true;
		this.label43.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label43.Location = new System.Drawing.Point(811, 154);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(94, 15);
		this.label43.TabIndex = 22;
		this.label43.Text = "Inspaction Desc";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(811, 95);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(101, 15);
		this.label6.TabIndex = 22;
		this.label6.Text = "Performa Invoice";
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.Location = new System.Drawing.Point(546, 236);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(101, 15);
		this.label40.TabIndex = 22;
		this.label40.Text = "Legalization Desc";
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.Location = new System.Drawing.Point(284, 210);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(93, 15);
		this.label34.TabIndex = 22;
		this.label34.Text = "Shipping Marks";
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(8, 236);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(101, 15);
		this.label37.TabIndex = 22;
		this.label37.Text = "No Of Containers";
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.Location = new System.Drawing.Point(8, 210);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(98, 15);
		this.label32.TabIndex = 22;
		this.label32.Text = "Commodity Desc";
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(811, 69);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(74, 15);
		this.label13.TabIndex = 23;
		this.label13.Text = "Expiry place";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(546, 15);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(80, 15);
		this.label2.TabIndex = 22;
		this.label2.Text = "Document No";
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label42.Location = new System.Drawing.Point(546, 42);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(59, 15);
		this.label42.TabIndex = 23;
		this.label42.Text = "Customer";
		this.panel4.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel4.Controls.Add(this.label23);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 30);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1155, 36);
		this.panel4.TabIndex = 0;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(3, 6);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(84, 24);
		this.label23.TabIndex = 0;
		this.label23.Text = "Lc Order";
		this.panel3.Controls.Add(this.ctrlGrdBar1);
		this.panel3.Controls.Add(this.toolStrip2);
		this.panel3.Controls.Add(this.ctrlGrdBar3);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1155, 30);
		this.panel3.TabIndex = 0;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1106, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdDetail;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 21);
		this.ctrlGrdBar1.TabIndex = 0;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[6] { this.btnNew, this.btnSave, this.btnUpdate, this.toolStripSeparator, this.Print, this.btnAttachment });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1155, 27);
		this.toolStrip2.TabIndex = 2;
		this.toolStrip2.Text = "toolStrip2";
		this.btnNew.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = (System.Drawing.Image)resources.GetObject("btnNew.Image");
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(55, 24);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnSave.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSave.Image = (System.Drawing.Image)resources.GetObject("btnSave.Image");
		this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(55, 24);
		this.btnSave.Text = "&Save";
		this.btnSave.Click += new System.EventHandler(btnSave_Click);
		this.btnUpdate.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(69, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.toolStripSeparator.Name = "toolStripSeparator";
		this.toolStripSeparator.Size = new System.Drawing.Size(6, 27);
		this.Print.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.Print.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.Print.Name = "Print";
		this.Print.Size = new System.Drawing.Size(56, 24);
		this.Print.Text = "&Print";
		this.ctrlGrdBar3.FormName = null;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(1379, -6);
		this.ctrlGrdBar3.MyGrid = null;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(43, 27);
		this.ctrlGrdBar3.TabIndex = 1;
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(94, 24);
		this.btnAttachment.Text = "&Attachment";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1101, 6);
		this.ctrlGrdBar2.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(48, 29);
		this.ctrlGrdBar2.TabIndex = 2;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1167, 699);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "LcOrder";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(LcOrder_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(LcOrder_KeyDown);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel11.ResumeLayout(false);
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		this.History.ResumeLayout(false);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.Form.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbPerformaInvoice).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbTransShipment).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPartialShipment).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbstatus).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbinspection).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmblegalization).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbfc).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbdestinationport).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbloadingport).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbExporterBanks).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbimporterbank).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbpaymenterm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbdeliveryterm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcustomer).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbproject).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbbranch).EndInit();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbrateuom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbouterpackuom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbinnerpackuom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbinnerpacktype).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbitem).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
