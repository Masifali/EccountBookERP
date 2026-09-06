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

namespace Architecture.WinApp.Export;

public class EximInvoice : Form
{
	private DropDownBind DDL = new DropDownBind();

	private Attachment AT = new Attachment();

	private UserAccount UserAccount = new UserAccount();

	private int RecId;

	private bool UpdateMode;

	private int updateDetailIndex;

	private DataTable dtdetail = new DataTable();

	private DataTable dtInvoice = new DataTable();

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private Panel panel5;

	private Panel panel4;

	private Panel panel2;

	private ToolStrip toolStrip1;

	private ToolStripButton BtnNew;

	private ToolStripButton btnSave;

	private ToolStripButton btnUpdate;

	private ToolStripSeparator toolStripSeparator;

	private ToolStripButton btnAttachment;

	private ToolStripButton toolStripButton3;

	private ToolStripButton helpToolStripButton;

	private Panel panel3;

	private Panel panel7;

	private Label label1;

	private TextBox txtBuyerLedgerValue;

	private Panel panel6;

	internal GridEX grdDetail;

	private Panel panel8;

	private Panel panel11;

	private Panel panel10;

	private Panel panel9;

	private ToolStrip toolStrip2;

	private ToolStripButton toolStripButton1;

	private ToolStripButton toolStripButton6;

	private ToolStripSeparator toolStripSeparator1;

	private Label label37;

	internal GridEX DataGridHistory;

	internal UltraCombo cmbImporterBank;

	internal UltraCombo cmbExporterBank;

	internal UltraCombo cmbCarierType;

	private Label label24;

	private TextBox txtRemarks;

	internal UltraCombo cmbDestinationPort;

	private Label label27;

	private Label label23;

	private TextBox txtGrossWeight;

	internal UltraCombo cmbLoadingPort;

	internal UltraCombo CmbLCContractor;

	private Label label22;

	private TextBox txtLocalAmt;

	private Label label19;

	private Label label7;

	private Label label17;

	private Label label6;

	private TextBox txtNetWeight;

	private Label label8;

	internal UltraCombo CmbCustomer;

	private Label label5;

	private Label label21;

	private TextBox txtinvno;

	private Label label4;

	private DateTimePicker invoicedate;

	private Label label10;

	private Label label20;

	internal UltraCombo comproject;

	private TextBox txtNoOfContainer;

	private Label label9;

	private Label label18;

	private Label label16;

	internal UltraCombo combranches;

	private Label label13;

	private TextBox txtConversionRate;

	private Label label15;

	internal UltraCombo cmbPaymentTerm;

	private TextBox txtTotalFcAmount;

	private Label label11;

	private Label label14;

	private Label label12;

	internal UltraCombo cmbFCurrencr;

	private GroupBox groupBox1;

	private Button btnEformNo;

	private TextBox textBox1;

	internal GridEX grdEform;

	private Label label25;

	internal UltraCombo cmbEFormNo;

	private TextBox textBox2;

	private Label label26;

	private Label label3;

	private TextBox txtdocno;

	private Label label42;

	private Panel panel12;

	private GroupBox groupBox5;

	private Label label41;

	private TextBox OuterDesc;

	private TextBox txtweightDetail;

	private Label label40;

	private Label label39;

	private Label label38;

	internal UltraCombo cmbOuterUOM;

	internal UltraCombo cmbInnerUOM;

	private Button btnCancel;

	private Button btnGridUpdate;

	private Button btnAddinGrid;

	private TextBox txtAmountFC;

	private Label label36;

	private TextBox txtrate;

	private Label label35;

	private TextBox txtItemDesc;

	private Label label34;

	internal UltraCombo cmbRateUOM;

	private Label label33;

	private TextBox txtOuterUnit;

	private Label label32;

	internal UltraCombo cmbPackUOM;

	private Label label31;

	private TextBox txtInnerUnit;

	private Label label30;

	internal UltraCombo cmbPackType;

	private Label label29;

	internal UltraCombo cmbItem;

	private Label label28;

	private Label label50;

	private Label label49;

	private Label label48;

	private Label label47;

	private Label label46;

	private Label label45;

	private Label label44;

	private Label label43;

	private Label label2;

	internal UltraCombo cmbdeliveryterm;

	private Label label53;

	private Label label52;

	private TextBox txtFOBValue;

	private Label label51;

	private CtrlGrdBar ctrlGrdBar1;

	private CtrlGrdBar ctrlGrdBar2;

	public EximInvoice(UserAccount user)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void Project()
	{
		try
		{
			DataTable dtproject = CommonServices.ProjectServiceBind();
			if (dtproject.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtproject, comproject, "Id", "ProjectName", "Project Name", false);
			}
		}
		catch (Exception)
		{
			throw;
		}
	}

	public void braches()
	{
	}

	public void Eform()
	{
	}

	private void suppliercustomer()
	{
		try
		{
			DataTable dtsuppliercustomer = CommonServices.SupplierCustomerGetAllServiceBind();
			if (dtsuppliercustomer.Rows.Count <= 0)
			{
				return;
			}
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("CompanyName");
			for (int i = 0; i < dtsuppliercustomer.Rows.Count; i++)
			{
				if (Convert.ToInt32(dtsuppliercustomer.Rows[i]["CustomerGroupId"]) == 7)
				{
					dt.Rows.Add(dtsuppliercustomer.Rows[i]["Id"], dtsuppliercustomer.Rows[i]["CompanyName"]);
				}
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, CmbCustomer, "Id", "CompanyName", "Customer Name", ZeroIndex: false);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbCustomer_Leave(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		try
		{
			ExImLcOrder val = new ExImLcOrder
			{
				CompanyId = UserAccount.CompanyId,
				OrganizationId = UserAccount.OrganizationId
			};
			int customerId = Conversion.ToInt(CmbCustomer.Value);
			DataTable dtLc = ExImLcOrder.GetData(val);
			if (dtLc.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("LcOrderNo");
				for (int i = 0; i < dtLc.Rows.Count; i++)
				{
					if (Convert.ToInt32(dtLc.Rows[i]["SupCustId"]) == customerId)
					{
						dt.Rows.Add(dtLc.Rows[i]["Id"], dtLc.Rows[i]["LcOrderNo"]);
					}
					if (dt.Rows.Count > 0)
					{
						DDL.BindDDL(dt, CmbLCContractor, "Id", "LcOrderNo", "LcOrderNo", ZeroIndex: false);
					}
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		EFormNo();
	}

	private void CmbLCContractor_Leave(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ExImLcOrder lcOrder = new ExImLcOrder();
			lcOrder.CompanyId = UserAccount.CompanyId;
			lcOrder.OrganizationId = UserAccount.OrganizationId;
			int LcId = Conversion.ToInt(CmbLCContractor.Value);
			if (LcId <= 0)
			{
				return;
			}
			DataTable dteform = ExImLcOrder.GetData(lcOrder);
			if (dteform.Rows.Count <= 0)
			{
				return;
			}
			DataTable dtimporterbank = new DataTable();
			dtimporterbank.Columns.Add("ImporterBankId");
			dtimporterbank.Columns.Add("ImporterBank");
			DataTable dtexporterbank = new DataTable();
			dtexporterbank.Columns.Add("ExporterBankId");
			dtexporterbank.Columns.Add("ExporterBank");
			DataTable dtDestinationPort = new DataTable();
			dtDestinationPort.Columns.Add("DestinationPortId");
			dtDestinationPort.Columns.Add("DestinationPort");
			DataTable dtLoadingPort = new DataTable();
			dtLoadingPort.Columns.Add("LoadingPortId");
			dtLoadingPort.Columns.Add("LoadingPort");
			for (int i = 0; i < dteform.Rows.Count; i++)
			{
				if (Convert.ToInt32(dteform.Rows[i]["Id"]) == LcId)
				{
					dtimporterbank.Rows.Add(dteform.Rows[i]["ImporterBankId"], dteform.Rows[i]["ImporterBank"]);
					dtexporterbank.Rows.Add(dteform.Rows[i]["ExporterBankId"], dteform.Rows[i]["ExporterBank"]);
					dtDestinationPort.Rows.Add(dteform.Rows[i]["DestinationPortId"], dteform.Rows[i]["DestinationPort"]);
					dtLoadingPort.Rows.Add(dteform.Rows[i]["LoadingPortId"], dteform.Rows[i]["LoadingPort"]);
				}
				if (dteform.Rows.Count > 0)
				{
					DDL.BindDDL(dtimporterbank, cmbImporterBank, "ImporterBankId", "ImporterBank", "Importer Bank", ZeroIndex: false);
					DDL.BindDDL(dtexporterbank, cmbExporterBank, "ExporterBankId", "ExporterBank", "Exporter Bank", ZeroIndex: false);
					DDL.BindDDL(dtDestinationPort, cmbLoadingPort, "DestinationPortId", "DestinationPort", "DestinationPort", ZeroIndex: false);
					DDL.BindDDL(dtLoadingPort, cmbDestinationPort, "LoadingPortId", "LoadingPort", "LoadingPort", ZeroIndex: false);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void EFormNo()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		try
		{
			ExImEFormRegistration val = new ExImEFormRegistration
			{
				CompanyId = UserAccount.CompanyId,
				OrganizationId = UserAccount.OrganizationId
			};
			int customerId = Conversion.ToInt(CmbCustomer.Value);
			DataTable dteform = ExImEFormRegistration.GetData(val);
			if (dteform.Rows.Count <= 0)
			{
				return;
			}
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("EFormNo");
			for (int i = 0; i < dteform.Rows.Count; i++)
			{
				if (Convert.ToInt32(dteform.Rows[i]["ConsigneeId"]) == customerId)
				{
					dt.Rows.Add(dteform.Rows[i]["Id"], dteform.Rows[i]["EFormNo"]);
				}
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, cmbEFormNo, "Id", "EFormNo", "EFormNo", ZeroIndex: false);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void bindcmbLoadingPort()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtseaport = SeaPorts.Getall(new SeaPorts
			{
				CompanyId = UserAccount.CompanyId,
				OrganizationId = UserAccount.OrganizationId
			});
			if (dtseaport.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtseaport, cmbLoadingPort, "Id", "PortName", "Port Name", false);
				DDL.BindDDLNew(dtseaport, cmbDestinationPort, "Id", "PortName", "Port Name", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void item()
	{
		try
		{
			DataTable dtItem = CommonServices.ItemGetForComboServiceBind();
			if (dtItem.Rows.Count > 0)
			{
				DDL.BindDDL(dtItem, cmbItem, "Id", "ItemName", "Item", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PackUOM()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		try
		{
			List<UOMSchedule> lst = UOMSchedule.SearchByObjectList(new UOMSchedule
			{
				CompanyId = UserAccount.CompanyId,
				OrganizationId = UserAccount.OrganizationId,
				ItemId = Conversion.ToInt(cmbItem.Value)
			});
			DataTable dtCOMMUOM = new DataTable();
			dtCOMMUOM.Columns.Add("Id");
			dtCOMMUOM.Columns.Add("Equivalent");
			for (int i = 0; i < lst.Count; i++)
			{
				dtCOMMUOM.Rows.Add(lst[i].Id, lst[i].Equivalent);
			}
			if (dtCOMMUOM.Rows.Count > 0)
			{
				DDL.BindDDL(dtCOMMUOM, cmbPackUOM, "Id", "Equivalent", "Pack UOM", ZeroIndex: false);
				DDL.BindDDL(dtCOMMUOM, cmbRateUOM, "Id", "Equivalent", "Rate UOM ", ZeroIndex: false);
				DDL.BindDDL(dtCOMMUOM, cmbInnerUOM, "Id", "Equivalent", "Inner UOM ", ZeroIndex: false);
				DDL.BindDDL(dtCOMMUOM, cmbOuterUOM, "Id", "Equivalent", "Outer UOM ", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbItem_Leave(object sender, EventArgs e)
	{
		PackUOM();
	}

	public void PaymentTerm()
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
				DDL.BindDDLNew(dtPaymentTerm, cmbPaymentTerm, "Id", "LcOrderTerm", "Payment Term", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void DeliveryTerm()
	{
		try
		{
			DataTable dtdeliveryTerm = ExImDeliveryTerm.Getall();
			if (dtdeliveryTerm.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtdeliveryTerm, cmbdeliveryterm, "Id", "Code", "Delivery Term", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void Fcurrency()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtmc = MultiCurrency.GetAll(new MultiCurrency
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtmc.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtmc, cmbFCurrencr, "Id", "CurrencyCode", "CurrencyCode", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void PackingType()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dt = ExImPackMaterilaType.Getall(new ExImPackMaterilaType
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbPackType, "Id", "Description", "Pack Type ", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void ImporterBank()
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
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("ImporterBankBranch");
			for (int i = 0; i < lst.Count; i++)
			{
				if (lst[i].IsHomeland == "Foreign Country")
				{
					dt.Rows.Add(lst[i].Id, lst[i].BranchName);
				}
			}
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbImporterBank, "Id", "ImporterBankBranch", "Importer Bank", ZeroIndex: false);
				DDL.BindDDL(dt, cmbExporterBank, "Id", "ImporterBankBranch", "Exporter Bank", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateCode()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		try
		{
			int Code = ExImInvoice.GenerateCode(new ExImInvoice
			{
				DocumentTypeId = 204
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

	private void ExpfrmCommercialinvoice_Load(object sender, EventArgs e)
	{
		txtinvno.Focus();
		GenerateCode();
		braches();
		Project();
		suppliercustomer();
		bindcmbLoadingPort();
		item();
		PackingType();
		Fcurrency();
		PaymentTerm();
		DeliveryTerm();
		ImporterBank();
		Eform();
		dtInvoice.Columns.Add("RegistrationFormNoId");
		dtInvoice.Columns.Add("EFormNo");
		dtdetail.Columns.Add("ItemId");
		dtdetail.Columns.Add("Item");
		dtdetail.Columns.Add("PackUOMId");
		dtdetail.Columns.Add("PackUOM");
		dtdetail.Columns.Add("ItemDesc");
		dtdetail.Columns.Add("PackTypeId");
		dtdetail.Columns.Add("PackType");
		dtdetail.Columns.Add("InnerQty");
		dtdetail.Columns.Add("InnerUOMId");
		dtdetail.Columns.Add("InnerUOM");
		dtdetail.Columns.Add("OuterQty");
		dtdetail.Columns.Add("OuterUOMId");
		dtdetail.Columns.Add("OuterUOM");
		dtdetail.Columns.Add("OuterDesc");
		dtdetail.Columns.Add("Weight", typeof(double));
		dtdetail.Columns.Add("Rate", typeof(double));
		dtdetail.Columns.Add("RateUOMId");
		dtdetail.Columns.Add("RateUOM");
		dtdetail.Columns.Add("Fcy-Amount", typeof(double));
		btnAddinGrid.Visible = true;
		btnGridUpdate.Visible = false;
		btnCancel.Visible = false;
	}

	public void FormReset()
	{
		btnSave.Visible = true;
		btnUpdate.Visible = false;
		GenerateCode();
		txtAmountFC.Text = string.Empty;
		txtBuyerLedgerValue.Text = string.Empty;
		txtConversionRate.Text = string.Empty;
		((Control)(object)cmbdeliveryterm).Text = string.Empty;
		txtLocalAmt.Text = string.Empty;
		((Control)(object)cmbExporterBank).Text = string.Empty;
		txtGrossWeight.Text = string.Empty;
		((Control)(object)cmbImporterBank).Text = string.Empty;
		txtInnerUnit.Text = string.Empty;
		txtinvno.Text = string.Empty;
		txtNetWeight.Text = string.Empty;
		txtNoOfContainer.Text = string.Empty;
		txtOuterUnit.Text = string.Empty;
		txtrate.Text = string.Empty;
		txtRemarks.Text = string.Empty;
		txtTotalFcAmount.Text = string.Empty;
		txtItemDesc.Text = string.Empty;
		((Control)(object)cmbCarierType).Text = string.Empty;
		((Control)(object)cmbDestinationPort).Text = string.Empty;
		((Control)(object)cmbEFormNo).Text = string.Empty;
		((Control)(object)cmbFCurrencr).Text = string.Empty;
		((Control)(object)cmbItem).Text = string.Empty;
		((Control)(object)cmbLoadingPort).Text = string.Empty;
		((Control)(object)cmbPackType).Text = string.Empty;
		((Control)(object)cmbPackUOM).Text = string.Empty;
		((Control)(object)cmbPaymentTerm).Text = string.Empty;
		((Control)(object)cmbRateUOM).Text = string.Empty;
		((Control)(object)CmbCustomer).Text = string.Empty;
		((Control)(object)CmbLCContractor).Text = string.Empty;
		((Control)(object)cmbCarierType).Text = string.Empty;
		((Control)(object)cmbInnerUOM).Text = string.Empty;
		((Control)(object)cmbOuterUOM).Text = string.Empty;
		((Control)(object)cmbImporterBank).Text = string.Empty;
		((Control)(object)cmbExporterBank).Text = string.Empty;
		txtdocno.Text = string.Empty;
		braches();
		Project();
		suppliercustomer();
		bindcmbLoadingPort();
		item();
		PackingType();
		Fcurrency();
		PaymentTerm();
		DeliveryTerm();
		ImporterBank();
		dtdetail.Clear();
		dtInvoice.Clear();
		grdDetail.ClearStructure();
		grdEform.ClearStructure();
		AT = new Attachment();
		GridFill();
	}

	public void DetailFormReset()
	{
		((Control)(object)cmbItem).Text = string.Empty;
		((Control)(object)cmbPackType).Text = string.Empty;
		((Control)(object)cmbPackUOM).Text = string.Empty;
		((Control)(object)cmbInnerUOM).Text = string.Empty;
		((Control)(object)cmbOuterUOM).Text = string.Empty;
		((Control)(object)cmbRateUOM).Text = string.Empty;
		txtItemDesc.Text = string.Empty;
		txtInnerUnit.Text = string.Empty;
		txtOuterUnit.Text = string.Empty;
		txtrate.Text = string.Empty;
		txtweightDetail.Text = string.Empty;
		OuterDesc.Text = string.Empty;
		txtRemarks.Text = string.Empty;
		OuterDesc.Text = string.Empty;
		txtAmountFC.Text = string.Empty;
		item();
		btnAddinGrid.Visible = true;
		btnGridUpdate.Visible = false;
		btnCancel.Visible = false;
	}

	private bool formvalidation()
	{
		if (txtinvno.Text == "")
		{
			MessageBox.Show("Please Check InvoiceNo");
			txtinvno.Focus();
			return false;
		}
		if (txtdocno.Text.Trim() == "")
		{
			MessageBox.Show("Please Insert DocNo");
			txtdocno.Focus();
			return false;
		}
		if (((Control)(object)CmbCustomer).Text.Trim() == "")
		{
			MessageBox.Show("Please select Customer");
			CmbCustomer.Focus();
			return false;
		}
		if (txtGrossWeight.Text.Trim() == "")
		{
			MessageBox.Show("Please Insert Gross Weight");
			txtGrossWeight.Focus();
			return false;
		}
		if (((Control)(object)cmbPaymentTerm).Text.Trim() == "")
		{
			MessageBox.Show("Please Select Payment Terms");
			cmbPaymentTerm.Focus();
			return false;
		}
		if (((Control)(object)cmbEFormNo).Text.Trim() == "")
		{
			MessageBox.Show("Please Select EForm First");
			cmbEFormNo.Focus();
			return false;
		}
		if (txtFOBValue.Text.Trim() == "")
		{
			MessageBox.Show("Please Insert FOB Value");
			txtFOBValue.Focus();
			return false;
		}
		return true;
	}

	private void btnSave_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0324: Unknown result type (might be due to invalid IL or missing references)
		//IL_032a: Expected O, but got Unknown
		//IL_0570: Unknown result type (might be due to invalid IL or missing references)
		//IL_0577: Expected O, but got Unknown
		//IL_0377: Unknown result type (might be due to invalid IL or missing references)
		//IL_037e: Expected O, but got Unknown
		try
		{
			ExImInvoice exImInvoice = new ExImInvoice();
			if (RecId > 0 && UpdateMode)
			{
				exImInvoice.Id = RecId;
			}
			if (!formvalidation())
			{
				return;
			}
			exImInvoice.OrganizationId = UserAccount.OrganizationId;
			exImInvoice.CompanyId = UserAccount.CompanyId;
			exImInvoice.BranchesId = Conversion.ToInt(combranches.Value);
			exImInvoice.ProjectsId = Conversion.ToInt(comproject.Value);
			exImInvoice.InvoiceNo = txtinvno.Text;
			exImInvoice.DocumentTypeId = 204;
			exImInvoice.DocDate = invoicedate.Value;
			exImInvoice.DocCode = Conversion.ToInt((object)txtdocno.Text);
			exImInvoice.SupplierCustomerId = Conversion.ToInt(CmbCustomer.Value);
			exImInvoice.LcOrderNoId = Conversion.ToInt(CmbLCContractor.Value);
			exImInvoice.GrossWeight = Conversion.ToDouble((object)txtGrossWeight.Text.Trim());
			exImInvoice.NetWeight = Conversion.ToDouble((object)txtNetWeight.Text.Trim());
			exImInvoice.NoOfContainers = Conversion.ToInt((object)txtNoOfContainer.Text);
			exImInvoice.PaymentTermId = Conversion.ToInt((object)((Control)(object)cmbPaymentTerm).Text);
			exImInvoice.DeliveryTermId = Conversion.ToInt((object)((Control)(object)cmbdeliveryterm).Text);
			exImInvoice.FcurrencyId = Conversion.ToInt(cmbFCurrencr.Value);
			exImInvoice.FCurrencyAmount = Conversion.ToDouble((object)txtTotalFcAmount.Text.Trim());
			exImInvoice.ConversionRate = Conversion.ToDouble((object)txtConversionRate.Text.Trim());
			exImInvoice.EquivalentAmount = Conversion.ToDouble((object)txtLocalAmt.Text.Trim());
			exImInvoice.LoadingPortId = Conversion.ToInt(cmbLoadingPort.Value);
			exImInvoice.DestinationPortId = Conversion.ToInt(cmbDestinationPort.Value);
			exImInvoice.CarierType = ((Control)(object)cmbCarierType).Text;
			exImInvoice.RemarksHeader = txtRemarks.Text;
			exImInvoice.ImporterBankId = Conversion.ToInt(cmbImporterBank.Value);
			exImInvoice.ExporteBankId = Conversion.ToInt(cmbExporterBank.Value);
			exImInvoice.BuyerGLValueRs = Conversion.ToInt((object)txtBuyerLedgerValue.Text.Trim());
			exImInvoice.FobValue = Conversion.ToDouble((object)txtFOBValue.Text.Trim());
			exImInvoice.EFormId = Conversion.ToInt(cmbEFormNo.Value);
			exImInvoice.LotNoRef = "";
			exImInvoice.InvoiceCommercialValue = 0.0;
			exImInvoice.OtherInstructions = "";
			exImInvoice.InvStatus = "";
			exImInvoice.IsApproved = false;
			exImInvoice.ApprovedDate = DateTime.Now;
			exImInvoice.EntryDate = DateTime.Now;
			exImInvoice.EntryUser = UserAccount.ID;
			exImInvoice.ModifyDate = DateTime.Now;
			exImInvoice.ModifyUser = UserAccount.ID;
			exImInvoice.ExImInvoiceDetail = new List<ExImInvoicePackingDetail>();
			exImInvoice.ExImInvoicePaymentTermsDetail = new List<ExImInvoicePaymentTermsDetail>();
			ExImInvoicePaymentTermsDetail exImInvoicePayment = new ExImInvoicePaymentTermsDetail();
			exImInvoicePayment.ExImEFormRegistrationId = Conversion.ToInt(cmbEFormNo.Value);
			exImInvoice.ExImInvoicePaymentTermsDetail.Add(exImInvoicePayment);
			if (grdDetail.RowCount > 0)
			{
				GridEXRow[] rows = grdDetail.GetRows();
				foreach (GridEXRow r in rows)
				{
					ExImInvoicePackingDetail exIm = new ExImInvoicePackingDetail();
					exIm.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					exIm.ItemUOMId = Conversion.ToInt(r.Cells["PackUOMId"].Value);
					exIm.ItemDescriptionManual = Conversion.ToString((object)r.Cells["ItemDesc"].Text);
					exIm.PackingMaterialTypeId = Conversion.ToInt(r.Cells["PackTypeId"].Value);
					exIm.InnerQty = Conversion.ToInt((object)r.Cells["InnerQty"].Text);
					exIm.InnerQtyUomId = Conversion.ToInt(r.Cells["InnerUOMId"].Value);
					exIm.OuterQty = Conversion.ToInt((object)r.Cells["OuterQty"].Text);
					exIm.OuterQtyUomId = Conversion.ToInt((object)r.Cells["OuterUOMId"].Text);
					exIm.OuterPackDescription = Conversion.ToString((object)r.Cells["OuterDesc"].Text);
					exIm.NetWeight = Conversion.ToDouble((object)r.Cells["Weight"].Text);
					exIm.RatePrice = Conversion.ToDouble((object)r.Cells["Rate"].Text);
					exIm.RateUomId = Conversion.ToInt(r.Cells["RateUOMId"].Value);
					exIm.FcAmount = Conversion.ToInt(r.Cells["Fcy-Amount"].Value);
					exIm.BrandId = 0;
					exImInvoice.ExImInvoiceDetail.Add(exIm);
				}
			}
			exImInvoice.AttachmentsList = new List<DMSAttachments>();
			for (int i = 0; i < Attachment.dtAttachments.Rows.Count; i++)
			{
				DMSAttachments attach = new DMSAttachments();
				attach.Attachment = Attachment.dtAttachments.Rows[i][1].ToString();
				attach.ScreenName = base.Name;
				exImInvoice.AttachmentsList.Add(attach);
			}
			int success = ExImInvoice.Save(exImInvoice);
			if (!UpdateMode && success > 0)
			{
				MessageBox.Show("Save SuccessFully");
				FormReset();
				GenerateCode();
				txtinvno.Focus();
			}
			if (UpdateMode && success > 0)
			{
				MessageBox.Show("Update SuccessFully");
				FormReset();
				GenerateCode();
				txtinvno.Focus();
			}
			FormReset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAddinGrid_Click(object sender, EventArgs e)
	{
		try
		{
			if (((Control)(object)cmbItem).Text.Trim() == "")
			{
				MessageBox.Show("Please Select item");
				cmbItem.Focus();
				return;
			}
			if (((Control)(object)cmbPackUOM).Text.Trim() == "")
			{
				MessageBox.Show("Please Pack Uom");
				cmbPackUOM.Focus();
				return;
			}
			if (((Control)(object)cmbPackType).Text.Trim() == "")
			{
				MessageBox.Show("Please Select PackType");
				cmbPackType.Focus();
				return;
			}
			if (txtInnerUnit.Text.Trim() == "")
			{
				MessageBox.Show("Please Insert InnerUnit");
				txtInnerUnit.Focus();
				return;
			}
			if (((Control)(object)cmbInnerUOM).Text.Trim() == "")
			{
				MessageBox.Show("Please Select Uom");
				cmbInnerUOM.Focus();
				return;
			}
			if (txtOuterUnit.Text.Trim() == "")
			{
				MessageBox.Show("Please Enter Uom");
				txtOuterUnit.Focus();
				return;
			}
			if (((Control)(object)cmbOuterUOM).Text.Trim() == "")
			{
				MessageBox.Show("Please Select OuterUom");
				cmbOuterUOM.Focus();
				return;
			}
			if (OuterDesc.Text.Trim() == "")
			{
				MessageBox.Show("Please Insert Outer Description");
				OuterDesc.Focus();
				return;
			}
			if (txtweightDetail.Text.Trim() == "")
			{
				MessageBox.Show("Please Insert Weight");
				txtweightDetail.Focus();
				return;
			}
			if (txtrate.Text.Trim() == "")
			{
				MessageBox.Show("Please Enter Rate");
				txtrate.Focus();
				return;
			}
			if (((Control)(object)cmbRateUOM).Text.Trim() == "")
			{
				MessageBox.Show("Please Select Rate Uom");
				cmbRateUOM.Focus();
				return;
			}
			if (txtAmountFC.Text.Trim() == "")
			{
				MessageBox.Show("Please Enter Fc Amount");
				txtAmountFC.Focus();
				return;
			}
			dtdetail.Rows.Add(cmbItem.Value, ((Control)(object)cmbItem).Text, cmbPackUOM.Value, ((Control)(object)cmbPackUOM).Text, txtItemDesc.Text, cmbPackType.Value, ((Control)(object)cmbPackType).Text, txtInnerUnit.Text, cmbInnerUOM.Value, ((Control)(object)cmbInnerUOM).Text, txtOuterUnit.Text, cmbOuterUOM.Value, ((Control)(object)cmbOuterUOM).Text, OuterDesc.Text, txtweightDetail.Text, txtrate.Text, cmbRateUOM.Value, ((Control)(object)cmbRateUOM).Text, txtAmountFC.Text);
			grdDetail.DataSource = dtdetail;
			grdDetail.RetrieveStructure();
			GridDetailSetting();
			DetailFormReset();
			cmbItem.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdate_Click(object sender, EventArgs e)
	{
		btnSave_Click(null, null);
		DetailFormReset();
		GenerateCode();
	}

	private void BtnNew_Click(object sender, EventArgs e)
	{
		FormReset();
		DetailFormReset();
		GenerateCode();
	}

	private void btnEformNo_Click(object sender, EventArgs e)
	{
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
			ExImInvoice exImLc = new ExImInvoice();
			RecId = Id;
			exImLc = ExImInvoice.GetByID(RecId);
			tabControl1.SelectedIndex = 0;
			combranches.Value = exImLc.BranchesId;
			comproject.Value = exImLc.ProjectsId;
			txtinvno.Text = exImLc.InvoiceNo;
			invoicedate.Value = exImLc.DocDate;
			CmbCustomer.Value = exImLc.SupplierCustomerId;
			CmbLCContractor.Value = exImLc.LcOrderNoId;
			txtGrossWeight.Text = exImLc.GrossWeight.ToString();
			txtNetWeight.Text = exImLc.NetWeight.ToString();
			txtNoOfContainer.Text = exImLc.NoOfContainers.ToString();
			cmbFCurrencr.Value = exImLc.FcurrencyId;
			txtTotalFcAmount.Text = exImLc.FCurrencyAmount.ToString();
			txtConversionRate.Text = exImLc.ConversionRate.ToString();
			txtLocalAmt.Text = exImLc.EquivalentAmount.ToString();
			cmbLoadingPort.Value = exImLc.LoadingPortId;
			cmbDestinationPort.Value = exImLc.DestinationPortId;
			((Control)(object)cmbCarierType).Text = exImLc.CarierType;
			txtRemarks.Text = exImLc.RemarksHeader;
			cmbImporterBank.Value = Conversion.ToInt((object)exImLc.ImporterBankId);
			cmbExporterBank.Value = Conversion.ToInt((object)exImLc.ExporteBankId);
			cmbPaymentTerm.Value = Conversion.ToInt((object)exImLc.PaymentTermId);
			cmbdeliveryterm.Value = Conversion.ToInt((object)exImLc.DeliveryTermId);
			cmbEFormNo.Value = Conversion.ToInt((object)exImLc.EFormId);
			txtdocno.Text = Conversion.ToString((object)exImLc.DocCode);
			dtdetail.Clear();
			grdDetail.ClearStructure();
			for (int i = 0; i < exImLc.ExImInvoiceDetail.Count; i++)
			{
				dtdetail.Rows.Add(exImLc.ExImInvoiceDetail[i].ItemId, exImLc.ExImInvoiceDetail[i].ItemName, exImLc.ExImInvoiceDetail[i].ItemUOMId, exImLc.ExImInvoiceDetail[i].ItemUOM, exImLc.ExImInvoiceDetail[i].ItemDescriptionManual, exImLc.ExImInvoiceDetail[i].PackingMaterialTypeId, exImLc.ExImInvoiceDetail[i].PackMaterilaType, exImLc.ExImInvoiceDetail[i].InnerQty, exImLc.ExImInvoiceDetail[i].InnerQtyUomId, exImLc.ExImInvoiceDetail[i].InnerUOM, exImLc.ExImInvoiceDetail[i].OuterQty, exImLc.ExImInvoiceDetail[i].OuterQtyUomId, exImLc.ExImInvoiceDetail[i].OuterUOM, exImLc.ExImInvoiceDetail[i].OuterPackDescription, exImLc.ExImInvoiceDetail[i].NetWeight, exImLc.ExImInvoiceDetail[i].RatePrice, exImLc.ExImInvoiceDetail[i].RateUomId, exImLc.ExImInvoiceDetail[i].RateUOM, exImLc.ExImInvoiceDetail[i].FcAmount);
			}
			grdDetail.DataSource = dtdetail;
			grdDetail.RetrieveStructure();
			GridDetailSetting();
			DetailFormReset();
			UpdateMode = true;
			List<DMSAttachments> byID = DMSAttachments.GetByID(Id, base.Name);
			AT = new Attachment();
			foreach (DMSAttachments Attachment in byID)
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

	public void GridDetailSetting()
	{
		grdDetail.RootTable.Columns["ItemId"].Visible = false;
		grdDetail.RootTable.Columns["PackUOMId"].Visible = false;
		grdDetail.RootTable.Columns["PackTypeId"].Visible = false;
		grdDetail.RootTable.Columns["InnerUOMId"].Visible = false;
		grdDetail.RootTable.Columns["OuterUOMId"].Visible = false;
		grdDetail.RootTable.Columns["RateUOMId"].Visible = false;
		grdDetail.RootTable.Columns["Item"].EditType = (EditType)0;
		grdDetail.RootTable.Columns["PackUOM"].EditType = (EditType)0;
		grdDetail.RootTable.Columns["ItemDesc"].EditType = (EditType)0;
		grdDetail.RootTable.Columns["PackType"].EditType = (EditType)0;
		grdDetail.RootTable.Columns["InnerQty"].EditType = (EditType)0;
		grdDetail.RootTable.Columns["InnerUOM"].EditType = (EditType)0;
		grdDetail.RootTable.Columns["OuterQty"].EditType = (EditType)0;
		grdDetail.RootTable.Columns["OuterDesc"].EditType = (EditType)0;
		grdDetail.RootTable.Columns["Weight"].EditType = (EditType)0;
		grdDetail.RootTable.Columns["Rate"].EditType = (EditType)0;
		grdDetail.RootTable.Columns["Fcy-Amount"].EditType = (EditType)0;
		grdDetail.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
		grdDetail.RootTable.Columns["Rate"].AggregateFunction = (AggregateFunction)2;
		grdDetail.RootTable.Columns["Fcy-Amount"].AggregateFunction = (AggregateFunction)2;
		grdDetail.RootTable.Columns["Weight"].FormatString = "0,0";
		grdDetail.RootTable.Columns["Rate"].FormatString = "0,0";
		grdDetail.RootTable.Columns["Fcy-Amount"].FormatString = "0,0";
		ctrlGrdBar2_Load(null, null);
	}

	private void grdDetail_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grdDetail.CurrentRow;
			if (item != null)
			{
				updateDetailIndex = item.RowIndex;
				((Control)(object)cmbItem).Text = item.Cells["ItemId"].Text.ToString();
				((Control)(object)cmbPackType).Text = item.Cells["PackType"].Text.ToString();
				txtItemDesc.Text = item.Cells["ItemDesc"].Text.ToString();
				((Control)(object)cmbRateUOM).Text = item.Cells["ItemId"].Text.ToString();
				((Control)(object)cmbPackUOM).Text = item.Cells["PackUOM"].Text.ToString();
				txtInnerUnit.Text = item.Cells["InnerQty"].Text.ToString();
				((Control)(object)cmbInnerUOM).Text = item.Cells["InnerUOMId"].Text.ToString();
				txtOuterUnit.Text = item.Cells["OuterQty"].Text.ToString();
				((Control)(object)cmbOuterUOM).Text = item.Cells["OuterUOM"].Text.ToString();
				OuterDesc.Text = item.Cells["OuterDesc"].Text.ToString();
				txtweightDetail.Text = item.Cells["Weight"].Text.ToString();
				txtrate.Text = item.Cells["Rate"].Text.ToString();
				((Control)(object)cmbRateUOM).Text = item.Cells["RateUOM"].Text.ToString();
				txtAmountFC.Text = item.Cells["Fcy-Amount"].Text.ToString();
				btnGridUpdate.Visible = true;
				btnCancel.Visible = true;
				btnAddinGrid.Visible = false;
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
			dtdetail.Rows[updateDetailIndex]["ItemId"] = cmbItem.Value;
			dtdetail.Rows[updateDetailIndex]["Item"] = ((Control)(object)cmbItem).Text;
			dtdetail.Rows[updateDetailIndex]["PackUOMId"] = cmbPackUOM.Value;
			dtdetail.Rows[updateDetailIndex]["PackUOM"] = ((Control)(object)cmbPackUOM).Text;
			dtdetail.Rows[updateDetailIndex]["ItemDesc"] = txtItemDesc.Text;
			dtdetail.Rows[updateDetailIndex]["PackTypeId"] = cmbPackType.Value;
			dtdetail.Rows[updateDetailIndex]["PackType"] = ((Control)(object)cmbPackType).Text;
			dtdetail.Rows[updateDetailIndex]["InnerQty"] = txtInnerUnit.Text;
			dtdetail.Rows[updateDetailIndex]["InnerUOMId"] = cmbInnerUOM.Value;
			dtdetail.Rows[updateDetailIndex]["InnerUOMId"] = ((Control)(object)cmbInnerUOM).Text;
			dtdetail.Rows[updateDetailIndex]["OuterQty"] = txtOuterUnit.Text;
			dtdetail.Rows[updateDetailIndex]["OuterUOMId"] = cmbOuterUOM.Value;
			dtdetail.Rows[updateDetailIndex]["OuterUOM"] = ((Control)(object)cmbOuterUOM).Text;
			dtdetail.Rows[updateDetailIndex]["OuterDesc"] = OuterDesc.Text;
			dtdetail.Rows[updateDetailIndex]["Weight"] = txtweightDetail.Text;
			dtdetail.Rows[updateDetailIndex]["Rate"] = txtrate.Text;
			dtdetail.Rows[updateDetailIndex]["RateUOMId"] = cmbRateUOM.Value;
			dtdetail.Rows[updateDetailIndex]["RateUOM"] = ((Control)(object)cmbRateUOM).Text;
			dtdetail.Rows[updateDetailIndex]["Fcy-Amount"] = txtAmountFC.Text;
			btnGridUpdate.Visible = false;
			btnCancel.Visible = false;
			btnAddinGrid.Visible = true;
			DetailFormReset();
			cmbItem.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancel_Click(object sender, EventArgs e)
	{
		DetailFormReset();
	}

	private void ExpfrmCommercialinvoice_KeyDown(object sender, KeyEventArgs e)
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
					BtnNew_Click(null, null);
				}
				else
				{
					BtnNew_Click(null, null);
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
			if (e.Control && e.KeyCode == Keys.F10)
			{
				AT.Show();
			}
		}
		catch (Exception)
		{
			throw;
		}
	}

	private void txtConversionRate_TextChanged(object sender, EventArgs e)
	{
		CurrencyCalculation();
	}

	private void CurrencyCalculation()
	{
		try
		{
			if (txtTotalFcAmount.Text != string.Empty && txtConversionRate.Text != string.Empty)
			{
				double num = Conversion.ToDouble((object)txtTotalFcAmount.Text);
				double b = Conversion.ToDouble((object)txtConversionRate.Text);
				txtLocalAmt.Text = (num * b).ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void WeightCalculation()
	{
		try
		{
			if (txtOuterUnit.Text != string.Empty && ((Control)(object)cmbOuterUOM).Text != string.Empty)
			{
				double num = Conversion.ToDouble((object)txtOuterUnit.Text);
				double b = Conversion.ToDouble((object)((Control)(object)cmbOuterUOM).Text);
				txtweightDetail.Text = (num * b).ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbOuterUOM_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			WeightCalculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AmountCalculation()
	{
		try
		{
			if (txtweightDetail.Text != string.Empty && txtrate.Text != string.Empty)
			{
				double num = Conversion.ToDouble((object)txtrate.Text);
				double c = Conversion.ToDouble((object)((Control)(object)cmbRateUOM).Text);
				double a = Conversion.ToDouble((object)txtweightDetail.Text);
				double d = num / c * a;
				txtAmountFC.Text = d.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbRateUOM_ValueChanged(object sender, EventArgs e)
	{
		AmountCalculation();
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			GridFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void GridFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		try
		{
			DataTable dtgrd = ExImInvoice.GetData(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Ids = "204"
			});
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("InvoiceNo");
			dt.Columns.Add("DocDate");
			dt.Columns.Add("Customer");
			dt.Columns.Add("GrossWeight", typeof(double));
			dt.Columns.Add("NetWeight", typeof(double));
			dt.Columns.Add("NoOfContainer");
			dt.Columns.Add("TotalFcAmount", typeof(double));
			dt.Columns.Add("ImporterBank");
			dt.Columns.Add("ExporterBank");
			dt.Columns.Add("LoadingPort");
			dt.Columns.Add("DestinationPort");
			dt.Columns.Add("NoOfAttachments");
			for (int i = 0; i < dtgrd.Rows.Count; i++)
			{
				dt.Rows.Add(dtgrd.Rows[i]["Id"], dtgrd.Rows[i]["InvoiceNo"], dtgrd.Rows[i]["DocDate"], dtgrd.Rows[i]["Customer"], dtgrd.Rows[i]["GrossWeight"], dtgrd.Rows[i]["NetWeight"], dtgrd.Rows[i]["NoOfContainers"], dtgrd.Rows[i]["FCurrencyAmount"], dtgrd.Rows[i]["ImporterBank"], dtgrd.Rows[i]["ExporterBank"], dtgrd.Rows[i]["LoadingPort"], dtgrd.Rows[i]["DestinationPort"], dtgrd.Rows[i]["NoOfAttachments"]);
			}
			DataGridHistory.DataSource = dt;
			DataGridHistory.RetrieveStructure();
			HistoryGridSettings();
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
			DataGridHistory.RootTable.Columns["Id"].Visible = false;
			DataGridHistory.RootTable.Columns["Id"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["InvoiceNo"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["DocDate"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["Customer"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["GrossWeight"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["NetWeight"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["TotalFcAmount"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["ImporterBank"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["ExporterBank"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["LoadingPort"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["DestinationPort"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["NoOfAttachments"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["InvoiceNo"].Width = 100;
			DataGridHistory.RootTable.Columns["DocDate"].Width = 100;
			DataGridHistory.RootTable.Columns["Customer"].Width = 250;
			DataGridHistory.RootTable.Columns["GrossWeight"].Width = 100;
			DataGridHistory.RootTable.Columns["NetWeight"].Width = 100;
			DataGridHistory.RootTable.Columns["NoOfContainer"].Width = 100;
			DataGridHistory.RootTable.Columns["TotalFcAmount"].Width = 130;
			DataGridHistory.RootTable.Columns["ImporterBank"].Width = 150;
			DataGridHistory.RootTable.Columns["ExporterBank"].Width = 150;
			DataGridHistory.RootTable.Columns["LoadingPort"].Width = 150;
			DataGridHistory.RootTable.Columns["DestinationPort"].Width = 150;
			DataGridHistory.RootTable.Columns["InvoiceNo"].FilterEditType = (FilterEditType)1;
			DataGridHistory.RootTable.Columns["DocDate"].FilterEditType = (FilterEditType)6;
			DataGridHistory.RootTable.Columns["Customer"].FilterEditType = (FilterEditType)1;
			DataGridHistory.RootTable.Columns["GrossWeight"].FilterEditType = (FilterEditType)1;
			DataGridHistory.RootTable.Columns["NetWeight"].FilterEditType = (FilterEditType)1;
			DataGridHistory.RootTable.Columns["TotalFcAmount"].FilterEditType = (FilterEditType)1;
			DataGridHistory.RootTable.Columns["ImporterBank"].FilterEditType = (FilterEditType)1;
			DataGridHistory.RootTable.Columns["ExporterBank"].FilterEditType = (FilterEditType)1;
			DataGridHistory.RootTable.Columns["LoadingPort"].FilterEditType = (FilterEditType)1;
			DataGridHistory.RootTable.Columns["DestinationPort"].FilterEditType = (FilterEditType)1;
			DataGridHistory.RootTable.Columns["GrossWeight"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["TotalFcAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["GrossWeight"].FormatString = "0,0";
			DataGridHistory.RootTable.Columns["NetWeight"].FormatString = "0,0";
			DataGridHistory.RootTable.Columns["TotalFcAmount"].FormatString = "0,0";
			DataGridHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
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

	private void DataGridHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value), 204);
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

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
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
		//IL_068b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0695: Expected O, but got Unknown
		//IL_06f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0703: Expected O, but got Unknown
		//IL_0704: Unknown result type (might be due to invalid IL or missing references)
		//IL_070e: Expected O, but got Unknown
		//IL_0772: Unknown result type (might be due to invalid IL or missing references)
		//IL_077c: Expected O, but got Unknown
		//IL_079e: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a8: Expected O, but got Unknown
		//IL_07ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d4: Expected O, but got Unknown
		//IL_07e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ea: Expected O, but got Unknown
		//IL_082d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0837: Expected O, but got Unknown
		//IL_0859: Unknown result type (might be due to invalid IL or missing references)
		//IL_0863: Expected O, but got Unknown
		//IL_08d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_08dc: Expected O, but got Unknown
		//IL_0914: Unknown result type (might be due to invalid IL or missing references)
		//IL_091e: Expected O, but got Unknown
		//IL_091f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0929: Expected O, but got Unknown
		//IL_092a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0934: Expected O, but got Unknown
		//IL_094b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0955: Expected O, but got Unknown
		//IL_0977: Unknown result type (might be due to invalid IL or missing references)
		//IL_0981: Expected O, but got Unknown
		//IL_0982: Unknown result type (might be due to invalid IL or missing references)
		//IL_098c: Expected O, but got Unknown
		//IL_09f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_09fa: Expected O, but got Unknown
		//IL_0a48: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a52: Expected O, but got Unknown
		//IL_0a7f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a89: Expected O, but got Unknown
		//IL_0aab: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab5: Expected O, but got Unknown
		//IL_0ae2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aec: Expected O, but got Unknown
		//IL_0b92: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b9c: Expected O, but got Unknown
		//IL_97f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_97ff: Expected O, but got Unknown
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
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
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
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
		Appearance appearance147 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance157 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance159 = new Appearance();
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
		Appearance appearance55 = new Appearance();
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
		Appearance appearance200 = new Appearance();
		Appearance appearance201 = new Appearance();
		Appearance appearance202 = new Appearance();
		Appearance appearance203 = new Appearance();
		Appearance appearance204 = new Appearance();
		Appearance appearance205 = new Appearance();
		Appearance appearance206 = new Appearance();
		Appearance appearance208 = new Appearance();
		Appearance appearance209 = new Appearance();
		Appearance appearance210 = new Appearance();
		Appearance appearance211 = new Appearance();
		Appearance appearance212 = new Appearance();
		GridEXLayout grdEform_DesignTimeLayout = new GridEXLayout();
		Appearance appearance164 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance168 = new Appearance();
		Appearance appearance169 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance171 = new Appearance();
		Appearance appearance172 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance190 = new Appearance();
		Appearance appearance191 = new Appearance();
		Appearance appearance192 = new Appearance();
		Appearance appearance193 = new Appearance();
		Appearance appearance194 = new Appearance();
		Appearance appearance195 = new Appearance();
		Appearance appearance197 = new Appearance();
		Appearance appearance198 = new Appearance();
		Appearance appearance199 = new Appearance();
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
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance226 = new Appearance();
		Appearance appearance227 = new Appearance();
		Appearance appearance228 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance180 = new Appearance();
		Appearance appearance181 = new Appearance();
		Appearance appearance182 = new Appearance();
		Appearance appearance183 = new Appearance();
		Appearance appearance184 = new Appearance();
		Appearance appearance186 = new Appearance();
		Appearance appearance187 = new Appearance();
		Appearance appearance188 = new Appearance();
		Appearance appearance189 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance185 = new Appearance();
		Appearance appearance196 = new Appearance();
		Appearance appearance207 = new Appearance();
		Appearance appearance218 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance213 = new Appearance();
		Appearance appearance214 = new Appearance();
		Appearance appearance215 = new Appearance();
		Appearance appearance216 = new Appearance();
		Appearance appearance217 = new Appearance();
		Appearance appearance219 = new Appearance();
		Appearance appearance220 = new Appearance();
		Appearance appearance221 = new Appearance();
		Appearance appearance222 = new Appearance();
		Appearance appearance223 = new Appearance();
		Appearance appearance224 = new Appearance();
		Appearance appearance225 = new Appearance();
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
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Export.EximInvoice));
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel6 = new System.Windows.Forms.Panel();
		this.grdDetail = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.panel12 = new System.Windows.Forms.Panel();
		this.groupBox5 = new System.Windows.Forms.GroupBox();
		this.label41 = new System.Windows.Forms.Label();
		this.OuterDesc = new System.Windows.Forms.TextBox();
		this.txtweightDetail = new System.Windows.Forms.TextBox();
		this.label40 = new System.Windows.Forms.Label();
		this.label39 = new System.Windows.Forms.Label();
		this.label38 = new System.Windows.Forms.Label();
		this.cmbOuterUOM = new UltraCombo();
		this.cmbInnerUOM = new UltraCombo();
		this.btnCancel = new System.Windows.Forms.Button();
		this.btnGridUpdate = new System.Windows.Forms.Button();
		this.btnAddinGrid = new System.Windows.Forms.Button();
		this.txtAmountFC = new System.Windows.Forms.TextBox();
		this.label36 = new System.Windows.Forms.Label();
		this.txtrate = new System.Windows.Forms.TextBox();
		this.label35 = new System.Windows.Forms.Label();
		this.txtItemDesc = new System.Windows.Forms.TextBox();
		this.label34 = new System.Windows.Forms.Label();
		this.cmbRateUOM = new UltraCombo();
		this.label33 = new System.Windows.Forms.Label();
		this.txtOuterUnit = new System.Windows.Forms.TextBox();
		this.label32 = new System.Windows.Forms.Label();
		this.cmbPackUOM = new UltraCombo();
		this.label31 = new System.Windows.Forms.Label();
		this.txtInnerUnit = new System.Windows.Forms.TextBox();
		this.label30 = new System.Windows.Forms.Label();
		this.cmbPackType = new UltraCombo();
		this.label29 = new System.Windows.Forms.Label();
		this.cmbItem = new UltraCombo();
		this.label28 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label53 = new System.Windows.Forms.Label();
		this.label52 = new System.Windows.Forms.Label();
		this.txtFOBValue = new System.Windows.Forms.TextBox();
		this.label51 = new System.Windows.Forms.Label();
		this.cmbdeliveryterm = new UltraCombo();
		this.label50 = new System.Windows.Forms.Label();
		this.label49 = new System.Windows.Forms.Label();
		this.label48 = new System.Windows.Forms.Label();
		this.cmbEFormNo = new UltraCombo();
		this.label47 = new System.Windows.Forms.Label();
		this.label46 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label45 = new System.Windows.Forms.Label();
		this.label44 = new System.Windows.Forms.Label();
		this.label43 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.btnEformNo = new System.Windows.Forms.Button();
		this.textBox1 = new System.Windows.Forms.TextBox();
		this.grdEform = new GridEX();
		this.label25 = new System.Windows.Forms.Label();
		this.textBox2 = new System.Windows.Forms.TextBox();
		this.label26 = new System.Windows.Forms.Label();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.label42 = new System.Windows.Forms.Label();
		this.cmbImporterBank = new UltraCombo();
		this.cmbExporterBank = new UltraCombo();
		this.cmbCarierType = new UltraCombo();
		this.label24 = new System.Windows.Forms.Label();
		this.txtRemarks = new System.Windows.Forms.TextBox();
		this.cmbDestinationPort = new UltraCombo();
		this.label27 = new System.Windows.Forms.Label();
		this.label23 = new System.Windows.Forms.Label();
		this.txtGrossWeight = new System.Windows.Forms.TextBox();
		this.cmbLoadingPort = new UltraCombo();
		this.CmbLCContractor = new UltraCombo();
		this.label22 = new System.Windows.Forms.Label();
		this.txtLocalAmt = new System.Windows.Forms.TextBox();
		this.label19 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.txtBuyerLedgerValue = new System.Windows.Forms.TextBox();
		this.label17 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.txtNetWeight = new System.Windows.Forms.TextBox();
		this.label8 = new System.Windows.Forms.Label();
		this.CmbCustomer = new UltraCombo();
		this.label5 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.txtinvno = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.invoicedate = new System.Windows.Forms.DateTimePicker();
		this.label10 = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.comproject = new UltraCombo();
		this.txtNoOfContainer = new System.Windows.Forms.TextBox();
		this.label9 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.combranches = new UltraCombo();
		this.label13 = new System.Windows.Forms.Label();
		this.txtConversionRate = new System.Windows.Forms.TextBox();
		this.label15 = new System.Windows.Forms.Label();
		this.cmbPaymentTerm = new UltraCombo();
		this.txtTotalFcAmount = new System.Windows.Forms.TextBox();
		this.label11 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.cmbFCurrencr = new UltraCombo();
		this.panel3 = new System.Windows.Forms.Panel();
		this.panel7 = new System.Windows.Forms.Panel();
		this.label1 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.BtnNew = new System.Windows.Forms.ToolStripButton();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
		this.helpToolStripButton = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel8 = new System.Windows.Forms.Panel();
		this.panel11 = new System.Windows.Forms.Panel();
		this.DataGridHistory = new GridEX();
		this.panel10 = new System.Windows.Forms.Panel();
		this.label37 = new System.Windows.Forms.Label();
		this.panel9 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton6 = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel5.SuspendLayout();
		this.panel12.SuspendLayout();
		this.groupBox5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbOuterUOM).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbInnerUOM).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbRateUOM).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPackUOM).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPackType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).BeginInit();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbdeliveryterm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbEFormNo).BeginInit();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdEform).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbImporterBank).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbExporterBank).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCarierType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDestinationPort).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbLoadingPort).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbLCContractor).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomer).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comproject).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combranches).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPaymentTerm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbFCurrencr).BeginInit();
		this.panel3.SuspendLayout();
		this.panel7.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel8.SuspendLayout();
		this.panel11.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel10.SuspendLayout();
		this.panel9.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1265, 766);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1265, 766);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel6);
		this.tabPage1.Controls.Add(this.panel5);
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1257, 740);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(3, 480);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1251, 257);
		this.panel6.TabIndex = 3;
		this.grdDetail.AllowDelete = (InheritableBoolean)1;
		((ScrollableBase)this.grdDetail).BackColor = System.Drawing.Color.White;
		grdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1251, 257);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 0;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdDetail).DoubleClick += new System.EventHandler(grdDetail_DoubleClick);
		this.panel5.BackColor = System.Drawing.Color.White;
		this.panel5.Controls.Add(this.panel12);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(3, 338);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1251, 142);
		this.panel5.TabIndex = 2;
		this.panel12.Controls.Add(this.groupBox5);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(0, 0);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1251, 142);
		this.panel12.TabIndex = 1;
		this.groupBox5.BackColor = System.Drawing.Color.White;
		this.groupBox5.Controls.Add(this.label41);
		this.groupBox5.Controls.Add(this.OuterDesc);
		this.groupBox5.Controls.Add(this.txtweightDetail);
		this.groupBox5.Controls.Add(this.label40);
		this.groupBox5.Controls.Add(this.label39);
		this.groupBox5.Controls.Add(this.label38);
		this.groupBox5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbOuterUOM);
		this.groupBox5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbInnerUOM);
		this.groupBox5.Controls.Add(this.btnCancel);
		this.groupBox5.Controls.Add(this.btnGridUpdate);
		this.groupBox5.Controls.Add(this.btnAddinGrid);
		this.groupBox5.Controls.Add(this.txtAmountFC);
		this.groupBox5.Controls.Add(this.label36);
		this.groupBox5.Controls.Add(this.txtrate);
		this.groupBox5.Controls.Add(this.label35);
		this.groupBox5.Controls.Add(this.txtItemDesc);
		this.groupBox5.Controls.Add(this.label34);
		this.groupBox5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbRateUOM);
		this.groupBox5.Controls.Add(this.label33);
		this.groupBox5.Controls.Add(this.txtOuterUnit);
		this.groupBox5.Controls.Add(this.label32);
		this.groupBox5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPackUOM);
		this.groupBox5.Controls.Add(this.label31);
		this.groupBox5.Controls.Add(this.txtInnerUnit);
		this.groupBox5.Controls.Add(this.label30);
		this.groupBox5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPackType);
		this.groupBox5.Controls.Add(this.label29);
		this.groupBox5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItem);
		this.groupBox5.Controls.Add(this.label28);
		this.groupBox5.Location = new System.Drawing.Point(5, 6);
		this.groupBox5.Name = "groupBox5";
		this.groupBox5.Size = new System.Drawing.Size(1211, 133);
		this.groupBox5.TabIndex = 0;
		this.groupBox5.TabStop = false;
		this.groupBox5.Text = "Detail";
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label41.Location = new System.Drawing.Point(1089, 21);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(111, 17);
		this.label41.TabIndex = 58;
		this.label41.Text = "Outer Description";
		this.OuterDesc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.OuterDesc.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.OuterDesc.Location = new System.Drawing.Point(1094, 44);
		this.OuterDesc.Name = "OuterDesc";
		this.OuterDesc.Size = new System.Drawing.Size(108, 22);
		this.OuterDesc.TabIndex = 8;
		this.txtweightDetail.BackColor = System.Drawing.Color.White;
		this.txtweightDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtweightDetail.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtweightDetail.Location = new System.Drawing.Point(14, 96);
		this.txtweightDetail.Name = "txtweightDetail";
		this.txtweightDetail.ReadOnly = true;
		this.txtweightDetail.Size = new System.Drawing.Size(98, 22);
		this.txtweightDetail.TabIndex = 9;
		this.txtweightDetail.TabStop = false;
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.Location = new System.Drawing.Point(989, 22);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(76, 17);
		this.label40.TabIndex = 55;
		this.label40.Text = "Outer UOM";
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label39.Location = new System.Drawing.Point(807, 23);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(72, 17);
		this.label39.TabIndex = 54;
		this.label39.Text = "Inner UOM";
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.Location = new System.Drawing.Point(300, 22);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(103, 17);
		this.label38.TabIndex = 53;
		this.label38.Text = "Item Description";
		this.cmbOuterUOM.AllowNull = (DefaultableBoolean)1;
		this.cmbOuterUOM.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbOuterUOM.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbOuterUOM.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbOuterUOM).DisplayLayout.Appearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbOuterUOM).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbOuterUOM).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance43).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance43).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance43).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbOuterUOM).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbOuterUOM).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance44;
		((SpecialBoxBase)((UltraGridBase)this.cmbOuterUOM).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance45).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance45).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbOuterUOM).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.cmbOuterUOM).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbOuterUOM).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbOuterUOM).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance47;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbOuterUOM).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.cmbOuterUOM).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbOuterUOM).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbOuterUOM).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance50).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbOuterUOM).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.cmbOuterUOM).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbOuterUOM).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance51).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance51).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance51).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance51).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbOuterUOM).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance51;
		((AppearanceBase)appearance52).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbOuterUOM).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.cmbOuterUOM).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbOuterUOM).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbOuterUOM).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.cmbOuterUOM).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbOuterUOM).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.cmbOuterUOM).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbOuterUOM).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbOuterUOM).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		this.cmbOuterUOM.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbOuterUOM).Location = new System.Drawing.Point(992, 45);
		((System.Windows.Forms.Control)(object)this.cmbOuterUOM).Name = "cmbOuterUOM";
		((System.Windows.Forms.Control)(object)this.cmbOuterUOM).Size = new System.Drawing.Size(98, 22);
		((System.Windows.Forms.Control)(object)this.cmbOuterUOM).TabIndex = 7;
		this.cmbOuterUOM.ValueChanged += new System.EventHandler(cmbOuterUOM_ValueChanged);
		this.cmbInnerUOM.AllowNull = (DefaultableBoolean)1;
		this.cmbInnerUOM.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbInnerUOM.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbInnerUOM.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbInnerUOM).DisplayLayout.Appearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.cmbInnerUOM).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbInnerUOM).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance120).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance120).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance120).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbInnerUOM).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance121).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbInnerUOM).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance121;
		((SpecialBoxBase)((UltraGridBase)this.cmbInnerUOM).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance122).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance122).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance122).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbInnerUOM).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.cmbInnerUOM).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbInnerUOM).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance123).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbInnerUOM).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance123;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbInnerUOM).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.cmbInnerUOM).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbInnerUOM).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbInnerUOM).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance126;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance127).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbInnerUOM).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.cmbInnerUOM).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbInnerUOM).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance128).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance128).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance128).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbInnerUOM).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance128;
		((AppearanceBase)appearance129).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbInnerUOM).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.cmbInnerUOM).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbInnerUOM).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbInnerUOM).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.cmbInnerUOM).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbInnerUOM).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.cmbInnerUOM).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbInnerUOM).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbInnerUOM).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		this.cmbInnerUOM.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbInnerUOM).Location = new System.Drawing.Point(807, 46);
		((System.Windows.Forms.Control)(object)this.cmbInnerUOM).Name = "cmbInnerUOM";
		((System.Windows.Forms.Control)(object)this.cmbInnerUOM).Size = new System.Drawing.Size(98, 22);
		((System.Windows.Forms.Control)(object)this.cmbInnerUOM).TabIndex = 5;
		this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.Green;
		this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancel.Location = new System.Drawing.Point(482, 104);
		this.btnCancel.Name = "btnCancel";
		this.btnCancel.Size = new System.Drawing.Size(56, 25);
		this.btnCancel.TabIndex = 15;
		this.btnCancel.Text = "Cancel";
		this.btnCancel.UseVisualStyleBackColor = true;
		this.btnCancel.Click += new System.EventHandler(btnCancel_Click);
		this.btnGridUpdate.FlatAppearance.BorderColor = System.Drawing.Color.Green;
		this.btnGridUpdate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnGridUpdate.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnGridUpdate.Location = new System.Drawing.Point(482, 73);
		this.btnGridUpdate.Name = "btnGridUpdate";
		this.btnGridUpdate.Size = new System.Drawing.Size(56, 25);
		this.btnGridUpdate.TabIndex = 14;
		this.btnGridUpdate.Text = "Update";
		this.btnGridUpdate.UseVisualStyleBackColor = true;
		this.btnGridUpdate.Click += new System.EventHandler(btnGridUpdate_Click);
		this.btnAddinGrid.Location = new System.Drawing.Point(439, 95);
		this.btnAddinGrid.Name = "btnAddinGrid";
		this.btnAddinGrid.Size = new System.Drawing.Size(27, 22);
		this.btnAddinGrid.TabIndex = 13;
		this.btnAddinGrid.UseVisualStyleBackColor = true;
		this.btnAddinGrid.Click += new System.EventHandler(btnAddinGrid_Click);
		this.txtAmountFC.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAmountFC.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAmountFC.Location = new System.Drawing.Point(316, 96);
		this.txtAmountFC.Name = "txtAmountFC";
		this.txtAmountFC.Size = new System.Drawing.Size(117, 22);
		this.txtAmountFC.TabIndex = 12;
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(314, 73);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(75, 17);
		this.label36.TabIndex = 16;
		this.label36.Text = "Amount Fcy";
		this.txtrate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtrate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtrate.Location = new System.Drawing.Point(118, 96);
		this.txtrate.Name = "txtrate";
		this.txtrate.Size = new System.Drawing.Size(88, 22);
		this.txtrate.TabIndex = 10;
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(116, 73);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(34, 17);
		this.label35.TabIndex = 14;
		this.label35.Text = "Rate";
		this.txtItemDesc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemDesc.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemDesc.Location = new System.Drawing.Point(298, 46);
		this.txtItemDesc.Name = "txtItemDesc";
		this.txtItemDesc.Size = new System.Drawing.Size(273, 22);
		this.txtItemDesc.TabIndex = 2;
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.Location = new System.Drawing.Point(12, 73);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(74, 17);
		this.label34.TabIndex = 12;
		this.label34.Text = "Weight KGs";
		this.cmbRateUOM.AllowNull = (DefaultableBoolean)1;
		this.cmbRateUOM.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbRateUOM.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbRateUOM.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance147).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbRateUOM).DisplayLayout.Appearance = (AppearanceBase)(object)appearance147;
		((UltraGridBase)this.cmbRateUOM).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbRateUOM).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance148).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance148).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance148).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbRateUOM).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance148;
		((AppearanceBase)appearance149).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbRateUOM).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance149;
		((SpecialBoxBase)((UltraGridBase)this.cmbRateUOM).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance150).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance150).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance150).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbRateUOM).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance150;
		((UltraGridBase)this.cmbRateUOM).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbRateUOM).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance151).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbRateUOM).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance151;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance153).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbRateUOM).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance153;
		((UltraGridBase)this.cmbRateUOM).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbRateUOM).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbRateUOM).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance154;
		((AppearanceBase)appearance155).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance155).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbRateUOM).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.cmbRateUOM).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbRateUOM).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance156).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance156).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance156).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance156).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbRateUOM).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance156;
		((AppearanceBase)appearance157).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbRateUOM).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.cmbRateUOM).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbRateUOM).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance158).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbRateUOM).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance158;
		((UltraGridBase)this.cmbRateUOM).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbRateUOM).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.cmbRateUOM).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbRateUOM).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbRateUOM).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		this.cmbRateUOM.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbRateUOM).Location = new System.Drawing.Point(213, 96);
		((System.Windows.Forms.Control)(object)this.cmbRateUOM).Name = "cmbRateUOM";
		((System.Windows.Forms.Control)(object)this.cmbRateUOM).Size = new System.Drawing.Size(98, 22);
		((System.Windows.Forms.Control)(object)this.cmbRateUOM).TabIndex = 11;
		this.cmbRateUOM.ValueChanged += new System.EventHandler(cmbRateUOM_ValueChanged);
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(211, 73);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(69, 17);
		this.label33.TabIndex = 6;
		this.label33.Text = "Rate UOM";
		this.txtOuterUnit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOuterUnit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOuterUnit.Location = new System.Drawing.Point(909, 45);
		this.txtOuterUnit.Name = "txtOuterUnit";
		this.txtOuterUnit.Size = new System.Drawing.Size(79, 22);
		this.txtOuterUnit.TabIndex = 6;
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.Location = new System.Drawing.Point(915, 22);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(65, 17);
		this.label32.TabIndex = 10;
		this.label32.Text = "Outer Qty";
		this.cmbPackUOM.AllowNull = (DefaultableBoolean)1;
		this.cmbPackUOM.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPackUOM.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPackUOM.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance132).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.Appearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance133).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance133).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPackUOM).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance133;
		((AppearanceBase)appearance134).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance134;
		((SpecialBoxBase)((UltraGridBase)this.cmbPackUOM).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance136).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance136).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance136).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance137).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance137;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance138).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance139;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance140).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance141).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance141).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance141).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance141).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance141;
		((AppearanceBase)appearance142).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPackUOM).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		this.cmbPackUOM.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPackUOM).Location = new System.Drawing.Point(193, 46);
		((System.Windows.Forms.Control)(object)this.cmbPackUOM).Name = "cmbPackUOM";
		((System.Windows.Forms.Control)(object)this.cmbPackUOM).Size = new System.Drawing.Size(99, 22);
		((System.Windows.Forms.Control)(object)this.cmbPackUOM).TabIndex = 1;
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.Location = new System.Drawing.Point(194, 22);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(69, 17);
		this.label31.TabIndex = 4;
		this.label31.Text = "Pack UOM";
		this.txtInnerUnit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtInnerUnit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtInnerUnit.Location = new System.Drawing.Point(712, 46);
		this.txtInnerUnit.Multiline = true;
		this.txtInnerUnit.Name = "txtInnerUnit";
		this.txtInnerUnit.Size = new System.Drawing.Size(90, 21);
		this.txtInnerUnit.TabIndex = 4;
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(717, 22);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(61, 17);
		this.label30.TabIndex = 8;
		this.label30.Text = "Inner Qty";
		this.cmbPackType.AllowNull = (DefaultableBoolean)1;
		this.cmbPackType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPackType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPackType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance55).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.cmbPackType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPackType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance56).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance56).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance56).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPackType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPackType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance58;
		((SpecialBoxBase)((UltraGridBase)this.cmbPackType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance59).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance59).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance59).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPackType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.cmbPackType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPackType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance60).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance60;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance61).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance62;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance63).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance64).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance64).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance64).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance64).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance64;
		((AppearanceBase)appearance65).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPackType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.cmbPackType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPackType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPackType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		this.cmbPackType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPackType).Location = new System.Drawing.Point(574, 46);
		((System.Windows.Forms.Control)(object)this.cmbPackType).Name = "cmbPackType";
		((System.Windows.Forms.Control)(object)this.cmbPackType).Size = new System.Drawing.Size(132, 22);
		((System.Windows.Forms.Control)(object)this.cmbPackType).TabIndex = 3;
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.Location = new System.Drawing.Point(575, 22);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(65, 17);
		this.label29.TabIndex = 2;
		this.label29.Text = "Pack Type";
		this.cmbItem.AllowNull = (DefaultableBoolean)1;
		this.cmbItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance69).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.cmbItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance70).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance70).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance70).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance71;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance72).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance72).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance72).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance73).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance73;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance74).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance75;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance76).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance77).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance77).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance77).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance77).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance77;
		((AppearanceBase)appearance78).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		this.cmbItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItem).Location = new System.Drawing.Point(8, 46);
		((System.Windows.Forms.Control)(object)this.cmbItem).Name = "cmbItem";
		((System.Windows.Forms.Control)(object)this.cmbItem).Size = new System.Drawing.Size(179, 22);
		((System.Windows.Forms.Control)(object)this.cmbItem).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbItem).Leave += new System.EventHandler(cmbItem_Leave);
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.Location = new System.Drawing.Point(13, 22);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(33, 17);
		this.label28.TabIndex = 0;
		this.label28.Text = "Item";
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.label53);
		this.panel4.Controls.Add(this.label52);
		this.panel4.Controls.Add(this.txtFOBValue);
		this.panel4.Controls.Add(this.label51);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbdeliveryterm);
		this.panel4.Controls.Add(this.label50);
		this.panel4.Controls.Add(this.label49);
		this.panel4.Controls.Add(this.label48);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbEFormNo);
		this.panel4.Controls.Add(this.label47);
		this.panel4.Controls.Add(this.label46);
		this.panel4.Controls.Add(this.label3);
		this.panel4.Controls.Add(this.label45);
		this.panel4.Controls.Add(this.label44);
		this.panel4.Controls.Add(this.label43);
		this.panel4.Controls.Add(this.label2);
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Controls.Add(this.txtdocno);
		this.panel4.Controls.Add(this.label42);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbImporterBank);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbExporterBank);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCarierType);
		this.panel4.Controls.Add(this.label24);
		this.panel4.Controls.Add(this.txtRemarks);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDestinationPort);
		this.panel4.Controls.Add(this.label27);
		this.panel4.Controls.Add(this.label23);
		this.panel4.Controls.Add(this.txtGrossWeight);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbLoadingPort);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbLCContractor);
		this.panel4.Controls.Add(this.label22);
		this.panel4.Controls.Add(this.txtLocalAmt);
		this.panel4.Controls.Add(this.label19);
		this.panel4.Controls.Add(this.label7);
		this.panel4.Controls.Add(this.txtBuyerLedgerValue);
		this.panel4.Controls.Add(this.label17);
		this.panel4.Controls.Add(this.label6);
		this.panel4.Controls.Add(this.txtNetWeight);
		this.panel4.Controls.Add(this.label8);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomer);
		this.panel4.Controls.Add(this.label5);
		this.panel4.Controls.Add(this.label21);
		this.panel4.Controls.Add(this.txtinvno);
		this.panel4.Controls.Add(this.label4);
		this.panel4.Controls.Add(this.invoicedate);
		this.panel4.Controls.Add(this.label10);
		this.panel4.Controls.Add(this.label20);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.comproject);
		this.panel4.Controls.Add(this.txtNoOfContainer);
		this.panel4.Controls.Add(this.label9);
		this.panel4.Controls.Add(this.label18);
		this.panel4.Controls.Add(this.label16);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.combranches);
		this.panel4.Controls.Add(this.label13);
		this.panel4.Controls.Add(this.txtConversionRate);
		this.panel4.Controls.Add(this.label15);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPaymentTerm);
		this.panel4.Controls.Add(this.txtTotalFcAmount);
		this.panel4.Controls.Add(this.label11);
		this.panel4.Controls.Add(this.label14);
		this.panel4.Controls.Add(this.label12);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbFCurrencr);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(3, 68);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1251, 270);
		this.panel4.TabIndex = 0;
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label53.ForeColor = System.Drawing.Color.Red;
		this.label53.Location = new System.Drawing.Point(647, 191);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(14, 17);
		this.label53.TabIndex = 78;
		this.label53.Text = "*";
		this.label52.AutoSize = true;
		this.label52.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label52.ForeColor = System.Drawing.Color.Red;
		this.label52.Location = new System.Drawing.Point(645, 166);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(14, 17);
		this.label52.TabIndex = 77;
		this.label52.Text = "*";
		this.txtFOBValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFOBValue.Location = new System.Drawing.Point(662, 192);
		this.txtFOBValue.Name = "txtFOBValue";
		this.txtFOBValue.Size = new System.Drawing.Size(162, 20);
		this.txtFOBValue.TabIndex = 75;
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label51.Location = new System.Drawing.Point(586, 194);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(65, 17);
		this.label51.TabIndex = 76;
		this.label51.Text = "Fob Value";
		this.cmbdeliveryterm.AllowNull = (DefaultableBoolean)1;
		this.cmbdeliveryterm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbdeliveryterm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbdeliveryterm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance30).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance30).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance31;
		((SpecialBoxBase)((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance37).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance38).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance38).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance38).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbdeliveryterm).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.cmbdeliveryterm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbdeliveryterm).Location = new System.Drawing.Point(385, 6);
		((System.Windows.Forms.Control)(object)this.cmbdeliveryterm).Name = "cmbdeliveryterm";
		((System.Windows.Forms.Control)(object)this.cmbdeliveryterm).Size = new System.Drawing.Size(159, 27);
		((System.Windows.Forms.Control)(object)this.cmbdeliveryterm).TabIndex = 8;
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label50.ForeColor = System.Drawing.Color.Red;
		this.label50.Location = new System.Drawing.Point(91, 210);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(14, 17);
		this.label50.TabIndex = 40;
		this.label50.Text = "*";
		this.label49.AutoSize = true;
		this.label49.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label49.ForeColor = System.Drawing.Color.Red;
		this.label49.Location = new System.Drawing.Point(647, 32);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(14, 17);
		this.label49.TabIndex = 39;
		this.label49.Text = "*";
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.ForeColor = System.Drawing.Color.Red;
		this.label48.Location = new System.Drawing.Point(365, 161);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(14, 17);
		this.label48.TabIndex = 38;
		this.label48.Text = "*";
		this.cmbEFormNo.AllowNull = (DefaultableBoolean)1;
		this.cmbEFormNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbEFormNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbEFormNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance200).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance200).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance200;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance201).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance201).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance201).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance201).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbEFormNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance201;
		((AppearanceBase)appearance202).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance202;
		((SpecialBoxBase)((UltraGridBase)this.cmbEFormNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance203).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance203).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance203).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance203).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance203;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance204).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance204).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance204;
		((AppearanceBase)appearance205).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance205).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance205;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance206).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance206;
		((AppearanceBase)appearance208).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance208).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance208;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance209).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance209).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance209).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance209).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance209).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance209;
		((AppearanceBase)appearance210).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance210;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance211).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance211).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance211;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance212).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance212;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		this.cmbEFormNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbEFormNo).Location = new System.Drawing.Point(662, 164);
		((System.Windows.Forms.Control)(object)this.cmbEFormNo).Name = "cmbEFormNo";
		((System.Windows.Forms.Control)(object)this.cmbEFormNo).Size = new System.Drawing.Size(164, 22);
		((System.Windows.Forms.Control)(object)this.cmbEFormNo).TabIndex = 0;
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label47.ForeColor = System.Drawing.Color.Red;
		this.label47.Location = new System.Drawing.Point(365, 129);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(14, 17);
		this.label47.TabIndex = 37;
		this.label47.Text = "*";
		this.label46.AutoSize = true;
		this.label46.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label46.ForeColor = System.Drawing.Color.Red;
		this.label46.Location = new System.Drawing.Point(91, 124);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(14, 17);
		this.label46.TabIndex = 36;
		this.label46.Text = "*";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(577, 169);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(72, 17);
		this.label3.TabIndex = 61;
		this.label3.Text = "E-Form No";
		this.label45.AutoSize = true;
		this.label45.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label45.ForeColor = System.Drawing.Color.Red;
		this.label45.Location = new System.Drawing.Point(91, 95);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(14, 17);
		this.label45.TabIndex = 35;
		this.label45.Text = "*";
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label44.ForeColor = System.Drawing.Color.Red;
		this.label44.Location = new System.Drawing.Point(91, 63);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(14, 17);
		this.label44.TabIndex = 34;
		this.label44.Text = "*";
		this.label43.AutoSize = true;
		this.label43.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label43.ForeColor = System.Drawing.Color.Red;
		this.label43.Location = new System.Drawing.Point(92, 6);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(14, 17);
		this.label43.TabIndex = 33;
		this.label43.Text = "*";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.Color.Red;
		this.label2.Location = new System.Drawing.Point(92, 35);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(14, 17);
		this.label2.TabIndex = 32;
		this.label2.Text = "*";
		this.groupBox1.Controls.Add(this.btnEformNo);
		this.groupBox1.Controls.Add(this.textBox1);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.grdEform);
		this.groupBox1.Controls.Add(this.label25);
		this.groupBox1.Controls.Add(this.textBox2);
		this.groupBox1.Controls.Add(this.label26);
		this.groupBox1.Enabled = false;
		this.groupBox1.Location = new System.Drawing.Point(842, 3);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(292, 205);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Visible = false;
		this.btnEformNo.Location = new System.Drawing.Point(169, 32);
		this.btnEformNo.Name = "btnEformNo";
		this.btnEformNo.Size = new System.Drawing.Size(27, 22);
		this.btnEformNo.TabIndex = 1;
		this.btnEformNo.UseVisualStyleBackColor = true;
		this.btnEformNo.Click += new System.EventHandler(btnEformNo_Click);
		this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.textBox1.Location = new System.Drawing.Point(202, 95);
		this.textBox1.Name = "textBox1";
		this.textBox1.Size = new System.Drawing.Size(79, 22);
		this.textBox1.TabIndex = 2;
		this.textBox1.Visible = false;
		this.grdEform.AllowDelete = (InheritableBoolean)1;
		((ScrollableBase)this.grdEform).BackColor = System.Drawing.Color.White;
		this.grdEform.ColumnAutoResize = true;
		grdEform_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdEform.DesignTimeLayout = grdEform_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdEform).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdEform.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdEform).Location = new System.Drawing.Point(6, 68);
		((System.Windows.Forms.Control)(object)this.grdEform).Name = "grdEform";
		((ScrollableBase)this.grdEform).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdEform).Size = new System.Drawing.Size(190, 104);
		((System.Windows.Forms.Control)(object)this.grdEform).TabIndex = 4;
		this.grdEform.TotalRow = (InheritableBoolean)1;
		this.grdEform.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdEform).VisualStyle = (VisualStyle)4;
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(199, 72);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(57, 17);
		this.label25.TabIndex = 2;
		this.label25.Text = "Pecent%";
		this.label25.Visible = false;
		this.textBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.textBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.textBox2.Location = new System.Drawing.Point(202, 143);
		this.textBox2.Name = "textBox2";
		this.textBox2.Size = new System.Drawing.Size(79, 22);
		this.textBox2.TabIndex = 3;
		this.textBox2.Visible = false;
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(199, 120);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(53, 17);
		this.label26.TabIndex = 64;
		this.label26.Text = "Amount";
		this.label26.Visible = false;
		this.txtdocno.BackColor = System.Drawing.Color.White;
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtdocno.Location = new System.Drawing.Point(111, 63);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.Size = new System.Drawing.Size(163, 25);
		this.txtdocno.TabIndex = 2;
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label42.Location = new System.Drawing.Point(41, 65);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(53, 17);
		this.label42.TabIndex = 31;
		this.label42.Text = "Doc No";
		this.cmbImporterBank.AllowNull = (DefaultableBoolean)1;
		this.cmbImporterBank.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbImporterBank.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbImporterBank.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance164).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Appearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance165).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance165).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance165).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbImporterBank).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance165;
		((AppearanceBase)appearance166).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance166;
		((SpecialBoxBase)((UltraGridBase)this.cmbImporterBank).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance167).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance167).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance167).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance167;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance168).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance168;
		((AppearanceBase)appearance169).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance169).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance169;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance170;
		((AppearanceBase)appearance171).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance171).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance171;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance172).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance172).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance172).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance172).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance172;
		((AppearanceBase)appearance173).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance173;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance175).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance175;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance176).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance176;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbImporterBank).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.cmbImporterBank.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbImporterBank).Location = new System.Drawing.Point(111, 151);
		((System.Windows.Forms.Control)(object)this.cmbImporterBank).Name = "cmbImporterBank";
		((System.Windows.Forms.Control)(object)this.cmbImporterBank).Size = new System.Drawing.Size(163, 27);
		((System.Windows.Forms.Control)(object)this.cmbImporterBank).TabIndex = 5;
		this.cmbExporterBank.AllowNull = (DefaultableBoolean)1;
		this.cmbExporterBank.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbExporterBank.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbExporterBank.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Appearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance57).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance57).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance57).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbExporterBank).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance57;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance68;
		((SpecialBoxBase)((UltraGridBase)this.cmbExporterBank).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance95).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance95).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance96).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance97).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance98;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance99).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance100).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance102).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbExporterBank).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.cmbExporterBank.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbExporterBank).Location = new System.Drawing.Point(111, 180);
		((System.Windows.Forms.Control)(object)this.cmbExporterBank).Name = "cmbExporterBank";
		((System.Windows.Forms.Control)(object)this.cmbExporterBank).Size = new System.Drawing.Size(163, 27);
		((System.Windows.Forms.Control)(object)this.cmbExporterBank).TabIndex = 6;
		this.cmbCarierType.AllowNull = (DefaultableBoolean)1;
		this.cmbCarierType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCarierType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCarierType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance79).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance79).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance79).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCarierType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance90;
		((SpecialBoxBase)((UltraGridBase)this.cmbCarierType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance101).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance101).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance101).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance113).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance113;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance124).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance135;
		((AppearanceBase)appearance145).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance145).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance146).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance146).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance146).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance146;
		((AppearanceBase)appearance160).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance160;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance161).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance161).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance161;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance162;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCarierType).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.cmbCarierType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCarierType).Location = new System.Drawing.Point(385, 96);
		((System.Windows.Forms.Control)(object)this.cmbCarierType).Name = "cmbCarierType";
		((System.Windows.Forms.Control)(object)this.cmbCarierType).Size = new System.Drawing.Size(159, 27);
		((System.Windows.Forms.Control)(object)this.cmbCarierType).TabIndex = 11;
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label24.Location = new System.Drawing.Point(295, 101);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(74, 17);
		this.label24.TabIndex = 30;
		this.label24.Text = "Carier Type";
		this.txtRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarks.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtRemarks.Location = new System.Drawing.Point(662, 90);
		this.txtRemarks.Multiline = true;
		this.txtRemarks.Name = "txtRemarks";
		this.txtRemarks.Size = new System.Drawing.Size(164, 66);
		this.txtRemarks.TabIndex = 19;
		this.cmbDestinationPort.AllowNull = (DefaultableBoolean)1;
		this.cmbDestinationPort.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDestinationPort.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDestinationPort.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance190).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance190).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Appearance = (AppearanceBase)(object)appearance190;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance191).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance191).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance191).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance191).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDestinationPort).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance191;
		((AppearanceBase)appearance192).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance192;
		((SpecialBoxBase)((UltraGridBase)this.cmbDestinationPort).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance193).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance193).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance193).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance193).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance193;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance194).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance194).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance194;
		((AppearanceBase)appearance195).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance195).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance195;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance197).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance197;
		((AppearanceBase)appearance198).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance198).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance198;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance199).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance199).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance199).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance199).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance199).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance199;
		((AppearanceBase)appearance12).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDestinationPort).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.cmbDestinationPort.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDestinationPort).Location = new System.Drawing.Point(385, 67);
		((System.Windows.Forms.Control)(object)this.cmbDestinationPort).Name = "cmbDestinationPort";
		((System.Windows.Forms.Control)(object)this.cmbDestinationPort).Size = new System.Drawing.Size(159, 27);
		((System.Windows.Forms.Control)(object)this.cmbDestinationPort).TabIndex = 10;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label27.Location = new System.Drawing.Point(593, 96);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(58, 17);
		this.label27.TabIndex = 10;
		this.label27.Text = "Remarks";
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label23.Location = new System.Drawing.Point(307, 71);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(62, 17);
		this.label23.TabIndex = 28;
		this.label23.Text = "Dest Port";
		this.txtGrossWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtGrossWeight.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtGrossWeight.Location = new System.Drawing.Point(662, 6);
		this.txtGrossWeight.Name = "txtGrossWeight";
		this.txtGrossWeight.Size = new System.Drawing.Size(164, 25);
		this.txtGrossWeight.TabIndex = 16;
		this.cmbLoadingPort.AllowNull = (DefaultableBoolean)1;
		this.cmbLoadingPort.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbLoadingPort.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbLoadingPort.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Appearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance17).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance17).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbLoadingPort).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance18;
		((SpecialBoxBase)((UltraGridBase)this.cmbLoadingPort).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance23).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance25).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance25).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance25).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbLoadingPort).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.cmbLoadingPort.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbLoadingPort).Location = new System.Drawing.Point(385, 37);
		((System.Windows.Forms.Control)(object)this.cmbLoadingPort).Name = "cmbLoadingPort";
		((System.Windows.Forms.Control)(object)this.cmbLoadingPort).Size = new System.Drawing.Size(159, 27);
		((System.Windows.Forms.Control)(object)this.cmbLoadingPort).TabIndex = 9;
		this.CmbLCContractor.AllowNull = (DefaultableBoolean)1;
		this.CmbLCContractor.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbLCContractor.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbLCContractor.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance226).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance226).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Appearance = (AppearanceBase)(object)appearance226;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance227).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance227).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance227).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance227).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbLCContractor).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance227;
		((AppearanceBase)appearance228).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance228;
		((SpecialBoxBase)((UltraGridBase)this.CmbLCContractor).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbLCContractor).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.CmbLCContractor.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbLCContractor).Location = new System.Drawing.Point(111, 120);
		((System.Windows.Forms.Control)(object)this.CmbLCContractor).Name = "CmbLCContractor";
		((System.Windows.Forms.Control)(object)this.CmbLCContractor).Size = new System.Drawing.Size(163, 27);
		((System.Windows.Forms.Control)(object)this.CmbLCContractor).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.CmbLCContractor).Leave += new System.EventHandler(CmbLCContractor_Leave);
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label22.Location = new System.Drawing.Point(286, 42);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(83, 17);
		this.label22.TabIndex = 26;
		this.label22.Text = "Loading Port";
		this.txtLocalAmt.BackColor = System.Drawing.Color.White;
		this.txtLocalAmt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtLocalAmt.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtLocalAmt.Location = new System.Drawing.Point(385, 216);
		this.txtLocalAmt.Name = "txtLocalAmt";
		this.txtLocalAmt.ReadOnly = true;
		this.txtLocalAmt.Size = new System.Drawing.Size(159, 25);
		this.txtLocalAmt.TabIndex = 15;
		this.txtLocalAmt.TabStop = false;
		this.txtLocalAmt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label19.Location = new System.Drawing.Point(282, 11);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(87, 17);
		this.label19.TabIndex = 24;
		this.label19.Text = "Delivery Term";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label7.Location = new System.Drawing.Point(545, 11);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(106, 17);
		this.label7.TabIndex = 6;
		this.label7.Text = "Gross Weight KG";
		this.txtBuyerLedgerValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBuyerLedgerValue.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtBuyerLedgerValue.Location = new System.Drawing.Point(1422, 186);
		this.txtBuyerLedgerValue.Name = "txtBuyerLedgerValue";
		this.txtBuyerLedgerValue.Size = new System.Drawing.Size(73, 25);
		this.txtBuyerLedgerValue.TabIndex = 17;
		this.txtBuyerLedgerValue.TabStop = false;
		this.txtBuyerLedgerValue.Visible = false;
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label17.Location = new System.Drawing.Point(1299, 190);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(120, 17);
		this.label17.TabIndex = 22;
		this.label17.Text = "Buyer Ledger Value";
		this.label17.Visible = false;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label6.Location = new System.Drawing.Point(21, 124);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(73, 17);
		this.label6.TabIndex = 4;
		this.label6.Text = "Lc Contract";
		this.txtNetWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNetWeight.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtNetWeight.Location = new System.Drawing.Point(662, 34);
		this.txtNetWeight.Name = "txtNetWeight";
		this.txtNetWeight.Size = new System.Drawing.Size(164, 25);
		this.txtNetWeight.TabIndex = 17;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label8.Location = new System.Drawing.Point(558, 37);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(93, 17);
		this.label8.TabIndex = 8;
		this.label8.Text = "Net Weight KG";
		this.CmbCustomer.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomer.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomer.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomer.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance177).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Appearance = (AppearanceBase)(object)appearance177;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance178).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance178).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance178).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomer).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance178;
		((AppearanceBase)appearance179).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance179;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomer).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance180).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance180).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance180).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance180).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance180;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance181).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance181).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance181;
		((AppearanceBase)appearance182).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance182).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance182;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance183).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance183;
		((AppearanceBase)appearance184).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance184).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance184;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance186).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance186).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance186).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance186).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance186).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance186;
		((AppearanceBase)appearance187).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance187;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance188).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance188).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance188;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance189).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance189;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomer).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomer).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.CmbCustomer.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomer).Location = new System.Drawing.Point(111, 91);
		((System.Windows.Forms.Control)(object)this.CmbCustomer).Name = "CmbCustomer";
		((System.Windows.Forms.Control)(object)this.CmbCustomer).Size = new System.Drawing.Size(163, 27);
		((System.Windows.Forms.Control)(object)this.CmbCustomer).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.CmbCustomer).Leave += new System.EventHandler(CmbCustomer_Leave);
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label5.Location = new System.Drawing.Point(30, 96);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(64, 17);
		this.label5.TabIndex = 2;
		this.label5.Text = "Customer";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label21.Location = new System.Drawing.Point(5, 185);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(89, 17);
		this.label21.TabIndex = 18;
		this.label21.Text = "Exporter Bank";
		this.txtinvno.BackColor = System.Drawing.Color.White;
		this.txtinvno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtinvno.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtinvno.Location = new System.Drawing.Point(111, 6);
		this.txtinvno.Name = "txtinvno";
		this.txtinvno.Size = new System.Drawing.Size(163, 25);
		this.txtinvno.TabIndex = 0;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label4.Location = new System.Drawing.Point(24, 10);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(70, 17);
		this.label4.TabIndex = 2;
		this.label4.Text = "Invoice No";
		this.invoicedate.CustomFormat = "dd-MMM-yy";
		this.invoicedate.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.invoicedate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.invoicedate.Location = new System.Drawing.Point(111, 34);
		this.invoicedate.Name = "invoicedate";
		this.invoicedate.Size = new System.Drawing.Size(163, 25);
		this.invoicedate.TabIndex = 1;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label10.Location = new System.Drawing.Point(15, 37);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(79, 17);
		this.label10.TabIndex = 0;
		this.label10.Text = "Invoice Date";
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label20.Location = new System.Drawing.Point(4, 156);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(90, 17);
		this.label20.TabIndex = 16;
		this.label20.Text = "Importer Bank";
		this.comproject.AllowNull = (DefaultableBoolean)1;
		this.comproject.AutoCompleteMode = (AutoCompleteMode)4;
		this.comproject.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comproject.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comproject).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.comproject).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comproject).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comproject).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance152).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comproject).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance152;
		((SpecialBoxBase)((UltraGridBase)this.comproject).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance163).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance163).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance163).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comproject).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance163;
		((UltraGridBase)this.comproject).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comproject).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance174).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance174).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comproject).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance174;
		((AppearanceBase)appearance185).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance185).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comproject).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance185;
		((UltraGridBase)this.comproject).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comproject).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance196).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance196;
		((AppearanceBase)appearance207).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance207).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance207;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance218).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance218).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance218).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance218).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance218).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comproject).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance218;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comproject).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.comproject).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comproject).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.comproject).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comproject).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comproject).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comproject).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.comproject.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comproject).Location = new System.Drawing.Point(1404, 237);
		((System.Windows.Forms.Control)(object)this.comproject).Name = "comproject";
		((System.Windows.Forms.Control)(object)this.comproject).Size = new System.Drawing.Size(91, 27);
		((System.Windows.Forms.Control)(object)this.comproject).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.comproject).TabStop = false;
		((System.Windows.Forms.Control)(object)this.comproject).Visible = false;
		this.txtNoOfContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNoOfContainer.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtNoOfContainer.Location = new System.Drawing.Point(662, 61);
		this.txtNoOfContainer.Name = "txtNoOfContainer";
		this.txtNoOfContainer.Size = new System.Drawing.Size(164, 25);
		this.txtNoOfContainer.TabIndex = 18;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label9.Location = new System.Drawing.Point(548, 66);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(103, 17);
		this.label9.TabIndex = 0;
		this.label9.Text = "No.Of Container";
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label18.Location = new System.Drawing.Point(1401, 217);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(48, 17);
		this.label18.TabIndex = 4;
		this.label18.Text = "Project";
		this.label18.Visible = false;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label16.Location = new System.Drawing.Point(285, 222);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(87, 17);
		this.label16.TabIndex = 20;
		this.label16.Text = "Local Amount";
		this.combranches.AllowNull = (DefaultableBoolean)1;
		this.combranches.AutoCompleteMode = (AutoCompleteMode)4;
		this.combranches.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combranches.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance213).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance213).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combranches).DisplayLayout.Appearance = (AppearanceBase)(object)appearance213;
		((UltraGridBase)this.combranches).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combranches).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance214).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance214).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance214).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance214).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combranches).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance214;
		((AppearanceBase)appearance215).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combranches).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance215;
		((SpecialBoxBase)((UltraGridBase)this.combranches).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance216).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance216).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance216).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance216).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combranches).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance216;
		((UltraGridBase)this.combranches).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combranches).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance217).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance217).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combranches).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance217;
		((AppearanceBase)appearance219).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance219).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combranches).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance219;
		((UltraGridBase)this.combranches).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combranches).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance220).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance220;
		((AppearanceBase)appearance221).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance221).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance221;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance222).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance222).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance222).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance222).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance222).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combranches).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance222;
		((AppearanceBase)appearance223).TextHAlignAsString = "Left";
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance223;
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance224).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance224).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combranches).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance224;
		((UltraGridBase)this.combranches).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance225).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combranches).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance225;
		((UltraGridBase)this.combranches).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combranches).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combranches).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combranches).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.combranches.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combranches).Location = new System.Drawing.Point(1307, 237);
		((System.Windows.Forms.Control)(object)this.combranches).Name = "combranches";
		((System.Windows.Forms.Control)(object)this.combranches).Size = new System.Drawing.Size(91, 27);
		((System.Windows.Forms.Control)(object)this.combranches).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.combranches).TabStop = false;
		((System.Windows.Forms.Control)(object)this.combranches).Visible = false;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label13.Location = new System.Drawing.Point(1295, 217);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(47, 17);
		this.label13.TabIndex = 2;
		this.label13.Text = "Branch";
		this.label13.Visible = false;
		this.txtConversionRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtConversionRate.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtConversionRate.Location = new System.Drawing.Point(385, 187);
		this.txtConversionRate.Name = "txtConversionRate";
		this.txtConversionRate.Size = new System.Drawing.Size(159, 25);
		this.txtConversionRate.TabIndex = 14;
		this.txtConversionRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtConversionRate.TextChanged += new System.EventHandler(txtConversionRate_TextChanged);
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label15.Location = new System.Drawing.Point(280, 191);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(93, 17);
		this.label15.TabIndex = 8;
		this.label15.Text = "Exchange Rate";
		this.cmbPaymentTerm.AllowNull = (DefaultableBoolean)1;
		this.cmbPaymentTerm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPaymentTerm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPaymentTerm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance82).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance83).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance83).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance83).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance84;
		((SpecialBoxBase)((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance85).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance85).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance85).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance86).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance86;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance87).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance88;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance89).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance91).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance91).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance91).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance91).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance91;
		((AppearanceBase)appearance92).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance93;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance94;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPaymentTerm).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.cmbPaymentTerm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPaymentTerm).Location = new System.Drawing.Point(111, 210);
		((System.Windows.Forms.Control)(object)this.cmbPaymentTerm).Name = "cmbPaymentTerm";
		((System.Windows.Forms.Control)(object)this.cmbPaymentTerm).Size = new System.Drawing.Size(163, 27);
		((System.Windows.Forms.Control)(object)this.cmbPaymentTerm).TabIndex = 7;
		this.txtTotalFcAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTotalFcAmount.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.txtTotalFcAmount.Location = new System.Drawing.Point(385, 159);
		this.txtTotalFcAmount.Name = "txtTotalFcAmount";
		this.txtTotalFcAmount.Size = new System.Drawing.Size(159, 25);
		this.txtTotalFcAmount.TabIndex = 13;
		this.txtTotalFcAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label11.Location = new System.Drawing.Point(4, 215);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(90, 17);
		this.label11.TabIndex = 2;
		this.label11.Text = "Payment Term";
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label14.Location = new System.Drawing.Point(293, 166);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(76, 17);
		this.label14.TabIndex = 6;
		this.label14.Text = "Fcy-Amount";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.label12.Location = new System.Drawing.Point(308, 131);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(61, 17);
		this.label12.TabIndex = 4;
		this.label12.Text = "Fcy Code";
		this.cmbFCurrencr.AllowNull = (DefaultableBoolean)1;
		this.cmbFCurrencr.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbFCurrencr.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbFCurrencr.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Appearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance106).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance106).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbFCurrencr).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance106;
		((AppearanceBase)appearance107).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance107;
		((SpecialBoxBase)((UltraGridBase)this.cmbFCurrencr).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance108).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance108).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance108).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance109).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance109;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance110).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance111;
		((AppearanceBase)appearance114).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance114).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance115).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance115).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance115).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance116).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbFCurrencr).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.cmbFCurrencr.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbFCurrencr).Location = new System.Drawing.Point(385, 129);
		((System.Windows.Forms.Control)(object)this.cmbFCurrencr).Name = "cmbFCurrencr";
		((System.Windows.Forms.Control)(object)this.cmbFCurrencr).Size = new System.Drawing.Size(159, 27);
		((System.Windows.Forms.Control)(object)this.cmbFCurrencr).TabIndex = 12;
		this.panel3.Controls.Add(this.panel7);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(3, 30);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1251, 38);
		this.panel3.TabIndex = 1;
		this.panel7.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel7.Controls.Add(this.label1);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1251, 38);
		this.panel7.TabIndex = 0;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI", 15.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(9, 4);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(156, 30);
		this.label1.TabIndex = 0;
		this.label1.Text = "Export Invoice";
		this.panel2.Controls.Add(this.ctrlGrdBar2);
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1251, 27);
		this.panel2.TabIndex = 0;
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[7] { this.BtnNew, this.btnSave, this.btnUpdate, this.toolStripSeparator, this.btnAttachment, this.toolStripButton3, this.helpToolStripButton });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1251, 27);
		this.toolStrip1.TabIndex = 0;
		this.toolStrip1.Text = "toolStrip1";
		this.BtnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnNew.Name = "BtnNew";
		this.BtnNew.Size = new System.Drawing.Size(35, 24);
		this.BtnNew.Text = "&New";
		this.BtnNew.Click += new System.EventHandler(BtnNew_Click);
		this.btnSave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(55, 24);
		this.btnSave.Text = "&Save";
		this.btnSave.Click += new System.EventHandler(btnSave_Click);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(69, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.toolStripSeparator.Name = "toolStripSeparator";
		this.toolStripSeparator.Size = new System.Drawing.Size(6, 27);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(94, 24);
		this.btnAttachment.Text = "&Attachment";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.toolStripButton3.Image = Architecture.WinApp.Properties.Resources.icons8_messaging_16;
		this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton3.Name = "toolStripButton3";
		this.toolStripButton3.Size = new System.Drawing.Size(54, 24);
		this.toolStripButton3.Text = "&SMS";
		this.helpToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.helpToolStripButton.Image = Architecture.WinApp.Properties.Resources.icons8_help_16;
		this.helpToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.helpToolStripButton.Name = "helpToolStripButton";
		this.helpToolStripButton.Size = new System.Drawing.Size(24, 24);
		this.helpToolStripButton.Text = "He&lp";
		this.tabPage2.Controls.Add(this.panel8);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1257, 740);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel8.Controls.Add(this.panel11);
		this.panel8.Controls.Add(this.panel10);
		this.panel8.Controls.Add(this.panel9);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel8.Location = new System.Drawing.Point(3, 3);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1251, 734);
		this.panel8.TabIndex = 0;
		this.panel11.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel11.Location = new System.Drawing.Point(0, 68);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1251, 666);
		this.panel11.TabIndex = 2;
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
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1251, 666);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 44;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.LinkClicked += new ColumnActionEventHandler(DataGridHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).DoubleClick += new System.EventHandler(DataGridHistory_DoubleClick);
		this.panel10.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel10.Controls.Add(this.label37);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(0, 30);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1251, 38);
		this.panel10.TabIndex = 1;
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.ForeColor = System.Drawing.Color.White;
		this.label37.Location = new System.Drawing.Point(17, 6);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(67, 24);
		this.label37.TabIndex = 1;
		this.label37.Text = "History";
		this.panel9.Controls.Add(this.ctrlGrdBar1);
		this.panel9.Controls.Add(this.toolStrip2);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(0, 0);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1251, 30);
		this.panel9.TabIndex = 0;
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.toolStripButton1, this.toolStripButton6, this.toolStripSeparator1 });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1251, 27);
		this.toolStrip2.TabIndex = 52;
		this.toolStrip2.Text = "toolStrip2";
		this.toolStripButton1.Image = (System.Drawing.Image)resources.GetObject("toolStripButton1.Image");
		this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton1.Name = "toolStripButton1";
		this.toolStripButton1.Size = new System.Drawing.Size(55, 24);
		this.toolStripButton1.Text = "&New";
		this.toolStripButton6.Image = (System.Drawing.Image)resources.GetObject("toolStripButton6.Image");
		this.toolStripButton6.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton6.Name = "toolStripButton6";
		this.toolStripButton6.Size = new System.Drawing.Size(56, 24);
		this.toolStripButton6.Text = "&Print";
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1200, 0);
		this.ctrlGrdBar1.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(48, 26);
		this.ctrlGrdBar1.TabIndex = 53;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1202, -4);
		this.ctrlGrdBar2.MyGrid = this.grdDetail;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(48, 29);
		this.ctrlGrdBar2.TabIndex = 1;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1265, 766);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "EximInvoice";
		this.Text = "ExpfrmCommercialinvoice";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(ExpfrmCommercialinvoice_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(ExpfrmCommercialinvoice_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel12.ResumeLayout(false);
		this.groupBox5.ResumeLayout(false);
		this.groupBox5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbOuterUOM).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbInnerUOM).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbRateUOM).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPackUOM).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPackType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbdeliveryterm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbEFormNo).EndInit();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdEform).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbImporterBank).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbExporterBank).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCarierType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDestinationPort).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbLoadingPort).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbLCContractor).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomer).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comproject).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combranches).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPaymentTerm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbFCurrencr).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		this.panel11.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}
